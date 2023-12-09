using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.Flight;

public sealed class CreateFlightDto
{
    [JsonPropertyName("plane_id")]
    public Guid? PlaneId { get; set; }

    [JsonPropertyName("pilots")]
    public Guid[]? Pilots { get; set; }
    
    [JsonPropertyName("status")]
    public FlightStatuses Status { get; set; } = FlightStatuses.Planned;
    
    [Required]
    [JsonPropertyName("departure_date")]
    public DateTimeOffset DepartureDate { get; set; }
    
    [JsonPropertyName("arrival_date")]
    public DateTimeOffset? ArrivalDate { get; set; }
    
    [Required]
    [MinLength(2)]
    [JsonPropertyName("from")]
    public string From { get; set; } = null!;
    
    [Required]
    [MinLength(2)]
    [JsonPropertyName("to")]
    public string To { get; set; } = null!;
}