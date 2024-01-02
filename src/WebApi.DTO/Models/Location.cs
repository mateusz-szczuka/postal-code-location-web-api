namespace WebApi.DTO.Models;

public sealed record Location
{
    public double Longitude { get; init; }

    public double Latitude { get; init; }
}