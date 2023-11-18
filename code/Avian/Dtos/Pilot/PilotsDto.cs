using System.Text.Json.Serialization;

namespace Avian.Dtos.Pilot;

public sealed class PilotsDto
{
    [JsonPropertyName("pilots")]
    public IReadOnlyCollection<PilotDto> Pilots { get; set; } = null!;
}