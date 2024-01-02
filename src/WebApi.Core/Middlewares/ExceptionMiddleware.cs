using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.DTO.Messeges.Responses;

namespace WebApi.Core.Middlewares;

public class ExceptionMiddleware
{
    private const string ContentType = "application/json";

    private readonly RequestDelegate _next;

    private ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (OperationCanceledException canceledEx)
        {
            var fault = Fault.CreateFault(System.Net.HttpStatusCode.Gone, canceledEx.Message);
            httpContext.Response.ContentType = ContentType;
            httpContext.Response.StatusCode = fault.Status;
            
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(fault));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            var fault = Fault.CreateInternalServerErrorFault(ex.Message);
            httpContext.Response.ContentType = ContentType;
            httpContext.Response.StatusCode = fault.Status;

            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(fault));
        }
    }
}