using WebApi.Core.Extensions;

namespace WebApi.Core.Clients;

public sealed record SendRequestContext
{
    internal IEnumerable<TimeSpan> RetryDelays { get; set; }

    internal bool IsRetryPolicyEnabled => RetryDelays.IsNotNullOrEmpty();
}