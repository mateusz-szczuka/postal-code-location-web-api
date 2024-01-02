using MediatR;
using WebApi.Core.Components.Abstractions;
using WebApi.Core.Extensions;
using WebApi.Core.Generators.Abstractions;
using WebApi.DTO.Messeges.Requests;
using WebApi.DTO.Messeges.Responses;
using WebApi.DTO.Messeges.Responses.Externals.Zippo;
using WebApi.DTO.Models;

namespace WebApi.Core.Handlers;

public sealed class GetAverageLocationHandler : IRequestHandler<GetAverageLocationRequest, GetAverageLocationResponse>
{
    private const int RoundUpTo = 4;

    private readonly IPostalCodesGenerator _postalCodesGenerator;

    private readonly IZippoIntegrationComponent _zippoIntegrationComponent;

    public GetAverageLocationHandler(
        IPostalCodesGenerator postalCodesGenerator,
        IZippoIntegrationComponent zippoIntegrationComponent)
	{
        _postalCodesGenerator = postalCodesGenerator ?? throw new ArgumentNullException(nameof(postalCodesGenerator));
        _zippoIntegrationComponent = zippoIntegrationComponent ?? throw new ArgumentNullException(nameof(zippoIntegrationComponent));
	}

    public async Task<GetAverageLocationResponse> Handle(GetAverageLocationRequest request, CancellationToken cancellationToken)
    {
        var postalCodes = _postalCodesGenerator.GetRandomPostalCodes();
        var tasks = postalCodes.Select(postalCode => _zippoIntegrationComponent.GetLocationAsync(postalCode, cancellationToken));

        var locationsResponses = await Task.WhenAll(tasks);
        var filteredLocations = GetFilteredLocations(locationsResponses);

        if (!filteredLocations.Any())
            return new GetAverageLocationResponse(Fault.CreateNotFoundFault("All retrieved locations were filtered out, resulting in no valid data."));

        var averageLongitude = Math.Round(filteredLocations.Average(p => p.Longitude), RoundUpTo);
        var averageLatitude = Math.Round(filteredLocations.Average(p => p.Latitude), RoundUpTo);

        return new GetAverageLocationResponse(averageLongitude, averageLatitude, filteredLocations.Count);
    }

    private IReadOnlyList<Location> GetFilteredLocations(IEnumerable<LocationResponse> locationsResponses)
    {
        return locationsResponses
            .Where(x => x.CheckIfLocationResultIsCorrect())
            .SelectMany(s => s.PostalCode.Places)
            .Select(s => new Location
            {
                Longitude = s.Longitude,
                Latitude = s.Latitude
            })
            .ToList();
    }
}