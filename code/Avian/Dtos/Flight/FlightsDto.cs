using System.Text.Json.Serialization;

namespace Avian.Dtos.Flight;

public sealed class FlightsDto
{
    [JsonPropertyName("flights")]
    public IReadOnlyCollection<FlightDto> Flights { get; set; } = null!;
}