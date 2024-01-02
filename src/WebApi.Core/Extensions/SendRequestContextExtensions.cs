using WebApi.Core.Clients;

namespace WebApi.Core.Extensions;

public static class SendRequestContextExtensions
{
    public static void AddRetryDelays(
        this SendRequestContext context,
        IEnumerable<TimeSpan> retryDelays)
    {
        context.RetryDelays = retryDelays;
    }
}

