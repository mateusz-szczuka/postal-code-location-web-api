namespace WebApi.Core.Options;

public sealed record CachedPostalCodeLocationsOptions
{
    public bool Enabled { get; init; }
}