using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.Flight;

public sealed class ChangeFlightDto
{
    [JsonPropertyName("status")]
    public FlightStatuses? Status { get; set; }
    
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}