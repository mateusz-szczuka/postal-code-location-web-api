using Microsoft.Extensions.Caching.Memory;
using WebApi.Core.Components.Abstractions;
using WebApi.Core.Extensions;
using WebApi.DTO.Messeges.Responses.Externals.Zippo;

namespace WebApi.Core.Components;

public sealed class CachedZippoIntegrationComponent : IZippoIntegrationComponent
{
    private readonly TimeSpan AbsoluteExpirationInDays = TimeSpan.FromDays(20);

    private readonly IZippoIntegrationComponent _decorated;

    private readonly IMemoryCache _cache;

    public CachedZippoIntegrationComponent(
        IZippoIntegrationComponent decorated,
        IMemoryCache cache)
	{
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
	}

    public async Task<LocationResponse> GetLocationAsync(string postalCode, CancellationToken cancellationToken = default)
    {
        if (!_cache.TryGetValue(postalCode, out LocationResponse cachedLocationResponse))
        {
            var locationResponse = await _decorated.GetLocationAsync(postalCode, cancellationToken);

            if (locationResponse.CheckIfLocationResultIsCorrect())
            {
                _cache.Set(postalCode, locationResponse, AbsoluteExpirationInDays);
            }

            return locationResponse;
        }

        return cachedLocationResponse;
    }
}