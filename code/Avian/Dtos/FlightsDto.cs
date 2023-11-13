using System.Text.Json.Serialization;

namespace Avian.Dtos;

public sealed class FlightsDto
{
    [JsonPropertyName("flights")]
    public IReadOnlyCollection<FlightDto> Flights { get; set; } = null!;
}