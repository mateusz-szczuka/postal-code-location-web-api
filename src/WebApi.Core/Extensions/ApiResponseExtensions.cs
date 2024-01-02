using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using WebApi.Core.Extensions;
using WebApi.DTO.Messeges.Abstractions;

namespace WebApi.Api.Extensions;

public static class ApiResponseExtensions
{
    public static IResult AsOkResult(this IApiResponse response)
        => response.BuildResult(HttpStatusCode.OK);

    public static bool IsFaulted(this IApiResponse response)
        => response.Fault.IsNotNull();

    public static bool IsNotFaulted(this IApiResponse response)
        => response.Fault.IsNull();

    private static IResult AsJsonResult(this IApiResponse response, HttpStatusCode statusCode)
        => Results.Json(response, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }, null, (int)statusCode);

    private static IResult BuildResult(this IApiResponse response, HttpStatusCode statusCode)
        => response.IsFaulted() ? response.Fault.AsJsonResult() : response.AsJsonResult(statusCode);
}