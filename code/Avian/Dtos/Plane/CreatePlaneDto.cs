using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.Plane;

public sealed class CreatePlaneDto
{
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("status")]
    public PlaneStatuses Status { get; set; }
}