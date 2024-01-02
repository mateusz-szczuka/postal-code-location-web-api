using WebApi.DTO.Messeges.Responses;

namespace WebApi.Core.Clients.Abstractions;

public interface IHttpRequestClient
{
    Task<HttpResponseResult> GetRequest(string url, Action<SendRequestContext> ctx = null, CancellationToken cancellationToken = default);
}