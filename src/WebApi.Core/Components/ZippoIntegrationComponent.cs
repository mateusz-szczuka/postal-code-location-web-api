using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Api.Extensions;
using WebApi.Core.Clients.Abstractions;
using WebApi.Core.Components.Abstractions;
using WebApi.Core.Extensions;
using WebApi.DTO.Messeges.Responses;
using WebApi.DTO.Messeges.Responses.Externals.Zippo;
using WebApi.DTO.Models.Externals.Zippo;

namespace WebApi.Core.Components;

public sealed class ZippoIntegrationComponent : IZippoIntegrationComponent
{
	private const string Url = "http://api.zippopotam.us/PL/{0}";

	private readonly ILogger<ZippoIntegrationComponent> _logger;

	private readonly IHttpRequestClient _httpRequestClient;

	private readonly IEnumerable<TimeSpan> _retryDelays = new[]
    {
        TimeSpan.FromMilliseconds(400),
        TimeSpan.FromMilliseconds(800)
    };

    public ZippoIntegrationComponent(
        ILogger<ZippoIntegrationComponent> logger,
        IHttpRequestClient httpRequestClient)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_httpRequestClient = httpRequestClient ?? throw new ArgumentNullException(nameof(httpRequestClient));
    }

	public async Task<LocationResponse> GetLocationAsync(string postalCode, CancellationToken cancellationToken = default)
	{
        var url = string.Format(Url, postalCode);
		var response = await _httpRequestClient.GetRequest(url, ctx => ctx.AddRetryDelays(_retryDelays), cancellationToken);

        if (response.IsFaulted())
            return new LocationResponse(response.Fault);

		try
		{
            var postalCodeRes = JsonConvert.DeserializeObject<PostalCode>(response.Content);
            return new LocationResponse(postalCodeRes);
        }
        catch (Exception ex)
        {
            var error = $"Deserialization exception. Message: {ex.Message}";
            _logger.LogWarning(ex, $"{nameof(GetLocationAsync)}: {error}");

            return new LocationResponse(Fault.CreateInternalServerErrorFault(ex.Message));
        }
    }
}