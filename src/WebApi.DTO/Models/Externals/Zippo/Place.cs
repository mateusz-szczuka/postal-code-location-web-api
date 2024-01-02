using Newtonsoft.Json;

namespace WebApi.DTO.Models.Externals.Zippo;

public sealed record Place
{
    [JsonProperty("longitude")]
    public double Longitude { get; init; }

    [JsonProperty("latitude")]
    public double Latitude { get; init; }
}