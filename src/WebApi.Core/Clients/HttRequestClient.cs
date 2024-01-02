using Microsoft.Extensions.Logging;
using WebApi.DTO.Messeges.Responses;
using WebApi.Core.Clients.Abstractions;

namespace WebApi.Core.Clients;

public sealed class HttpRequestClient : IHttpRequestClient
{
    private readonly HttpClient _httpClient;

    private readonly IPolicyFactory _policyFactory;

    private readonly ILogger<HttpRequestClient> _logger;

    public HttpRequestClient(
        HttpClient httpClient,
        IPolicyFactory policyFactory,
        ILogger<HttpRequestClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _policyFactory = policyFactory ?? throw new ArgumentNullException(nameof(policyFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<HttpResponseResult> GetRequest(
        string url,
        Action<SendRequestContext> ctx = null,
        CancellationToken cancellationToken = default)
    {
        return SendRequest(url, HttpMethod.Get, null, ctx, cancellationToken);
    }

    private async Task<HttpResponseResult> SendRequest(
        string url,
        HttpMethod method,
        HttpContent httpContent = null,
        Action<SendRequestContext> ctx = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var context = CreateSendRequestContext();
            ctx(context);

            var policy = _policyFactory.CreateHttpRequestPolicy(context);

            return await policy.ExecuteAsync(async () =>
            {
                using var request = new HttpRequestMessage(method, url) { Content = httpContent };
                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (context.IsRetryPolicyEnabled)
                {
                    response.EnsureSuccessStatusCode();
                    return new HttpResponseResult(await response.Content.ReadAsStringAsync(cancellationToken));
                }

                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseResult(await response.Content.ReadAsStringAsync(cancellationToken));
                }

                var error = $"Request {url} failed with status code: {response.StatusCode}.";
                _logger.LogWarning($"{nameof(SendRequest)}: {error}");

                return new HttpResponseResult(Fault.CreateFault(response.StatusCode, error));
            });
        }
        catch (Exception ex)
        {
            var error = $"Exception occured. Message: {ex.Message}";
            _logger.LogWarning(ex, $"{nameof(SendRequest)}: {error}");

            return new HttpResponseResult(
                Fault.CreateInternalServerErrorFault(error));
        }
    }

    private SendRequestContext CreateSendRequestContext() => new();
}