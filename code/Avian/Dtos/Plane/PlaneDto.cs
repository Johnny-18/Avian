using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.Plane;

public sealed class PlaneDto
{
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("status")]
    public PlaneStatuses Status { get; set; }

    public static PlaneDto FromDomain(Domain.Flights.Plane plane)
    {
        return new PlaneDto
        {
            Id = plane.Id,
            Name = plane.Name,
            Status = plane.Status,
        };
    }
}