using Polly;

namespace WebApi.Core.Clients.Abstractions;

public interface IPolicyFactory
{
    IAsyncPolicy CreateHttpRequestPolicy(SendRequestContext context);
}