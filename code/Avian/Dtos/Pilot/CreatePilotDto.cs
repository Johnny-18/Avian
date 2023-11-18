using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Avian.Dtos.Pilot;

public sealed class CreatePilotDto
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("qualification")]
    public string Qualification { get; set; } = null!;
}