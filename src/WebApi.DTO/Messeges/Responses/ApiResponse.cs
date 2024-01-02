using System.Net;
using WebApi.DTO.Messeges.Abstractions;

namespace WebApi.DTO.Messeges.Responses;

public record ApiResponse : IApiResponse
{
    public IFault Fault { get; }

    public ApiResponse()
    {
    }

    public ApiResponse(IFault fault)
        => Fault = fault;
}

public sealed record Fault : IFault
{
    public string Title { get; private set; }

    public string Detail { get; private set; }

    public int Status { get; private set; }

    public string Type { get; private set; }

    public static Fault CreateBadRequestFault(string detail = null)
        => CreateFault(HttpStatusCode.BadRequest, detail);

    public static Fault CreateInternalServerErrorFault(string detail = null)
        => CreateFault(HttpStatusCode.InternalServerError, detail);

    public static Fault CreateNotFoundFault(string detail = null)
        => CreateFault(HttpStatusCode.NotFound, detail);

    public static Fault CreateRequestTimeoutFault(string detail = null)
        => CreateFault(HttpStatusCode.RequestTimeout, detail);

    public static Fault CreateFault(HttpStatusCode status, string detail = null)
    {
        if (status >= HttpStatusCode.OK && status <= HttpStatusCode.PartialContent)
            throw new ArgumentException($"The http error status code cannot be {status}.");

        return new()
        {
            Title = status.ToString(),
            Detail = detail,
            Status = (int)status
        };
    }
}