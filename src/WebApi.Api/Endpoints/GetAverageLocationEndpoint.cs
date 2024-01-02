using Carter;
using MediatR;
using WebApi.Api.Extensions;
using WebApi.DTO.Messeges.Requests;

namespace WebApi.Api.Endpoints;

public sealed class GetAverageLocationEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(new GetAverageLocationRequest(), cancellationToken);
            return response.AsOkResult();
        });
    }
}