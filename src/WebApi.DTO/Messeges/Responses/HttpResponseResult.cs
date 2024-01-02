using WebApi.DTO.Messeges.Abstractions;

namespace WebApi.DTO.Messeges.Responses;

public sealed record HttpResponseResult : ApiResponse
{
    public string Content { get; }

    public HttpResponseResult(string content)
    {
        Content = content;
    }

    public HttpResponseResult(IFault fault) : base(fault)
    {
    }
}