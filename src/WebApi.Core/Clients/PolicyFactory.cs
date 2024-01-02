using Microsoft.Extensions.Logging;
using Polly;
using WebApi.DTO.Messeges.Responses;
using WebApi.Core.Clients.Abstractions;

namespace WebApi.Core.Clients;

public sealed class PolicyFactory : IPolicyFactory
{
    private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

    private readonly ILogger<PolicyFactory> _logger;

    public PolicyFactory(ILogger<PolicyFactory> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IAsyncPolicy CreateHttpRequestPolicy(SendRequestContext context)
    {
        var retryPolicy = context.IsRetryPolicyEnabled
        ? CreateRetryPolicy(context.RetryDelays)
        : null;

        var timeoutPolicy = CreateTimeoutPolicy();

        return CreatePolicies(
            retryPolicy,
            timeoutPolicy);
    }

    private IAsyncPolicy CreateTimeoutPolicy()
    {
        return Policy.TimeoutAsync(_defaultTimeout,
            (context, timeSpan, task) =>
            {
                var error = $"{nameof(PolicyFactory)}: Request timeout: {context.OperationKey} duration: {timeSpan}.";
                _logger.LogWarning(error);

                return Task.FromResult(new HttpResponseResult(Fault.CreateRequestTimeoutFault(
                    $"Request timeout, duration: {timeSpan}.")));
            }
        );
    }

    private IAsyncPolicy CreateRetryPolicy(IEnumerable<TimeSpan> retryDelays)
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryDelays,
                (exception, timeSpan, retryCount, context) =>
                {
                    var error = $"{nameof(PolicyFactory)}: Retry policy {exception.Message}. Trying again in {timeSpan}, attempt: {retryCount}.";
                    _logger.LogWarning(error);
                }
            );
    }

    private IAsyncPolicy CreatePolicies(params IAsyncPolicy[] policies)
    {
        IAsyncPolicy combinedPolicy = null;

        foreach (var policy in policies)
        {
            if (policy is not null)
            {
                combinedPolicy = combinedPolicy == null ? policy : policy.WrapAsync(combinedPolicy);
            }
        }

        return combinedPolicy ?? CreateTimeoutPolicy();
    }
}