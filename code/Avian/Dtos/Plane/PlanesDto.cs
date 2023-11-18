using System.Text.Json.Serialization;

namespace Avian.Dtos.Plane;

public sealed class PlanesDto
{
    [JsonPropertyName("planes")]
    public IReadOnlyCollection<PlaneDto> Planes { get; set; } = null!;
}