using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using WebApi.DTO.Messeges.Abstractions;

namespace WebApi.Api.Extensions;

public static class FaultExtensions
{
    public static IResult AsJsonResult(this IFault fault)
        => Results.Json(fault, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }, null, fault.Status);
}