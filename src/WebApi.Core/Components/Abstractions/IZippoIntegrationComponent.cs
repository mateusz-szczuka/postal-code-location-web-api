using WebApi.DTO.Messeges.Responses.Externals.Zippo;

namespace WebApi.Core.Components.Abstractions;

public interface IZippoIntegrationComponent
{
    Task<LocationResponse> GetLocationAsync(string postalCode, CancellationToken cancellationToken = default);
}