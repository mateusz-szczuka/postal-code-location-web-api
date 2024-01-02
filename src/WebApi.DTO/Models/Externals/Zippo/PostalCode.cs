using Newtonsoft.Json;

namespace WebApi.DTO.Models.Externals.Zippo;

public sealed record PostalCode
{
    [JsonProperty("places")]
    public IEnumerable<Place> Places { get; init; }
}