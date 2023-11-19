using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Avian.Dtos.Pilot;

public sealed class PilotDto
{
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonPropertyName("qualification")]
    public string Qualification { get; set; } = null!;
    
    public static PilotDto FromDomain(Domain.Flights.Pilot pilot)
    {
        return new PilotDto
        {
            Id = pilot.Id,
            Name = pilot.Name,
            Qualification = pilot.Qualification,
        };
    }
}