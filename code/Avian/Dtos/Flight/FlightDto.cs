using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Avian.Domain.ValueObjects;

namespace Avian.Dtos.Flight;

public sealed class FlightDto
{
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("plane_id")]
    public Guid? PlaneId { get; set; }

    [JsonPropertyName("pilots")]
    public Guid[]? Pilots { get; set; }

    [Required]
    [JsonPropertyName("status")]
    public FlightStatuses Status { get; set; }
    
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
    
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

    public static FlightDto FromDomain(Domain.Flights.Flight flight)
    {
        return new FlightDto
        {
            Id = flight.Id,
            PlaneId = flight.PlaneId,
            Pilots = flight.Pilots,
            Status = flight.Status,
            Comment = flight.Comment,
            DepartureDate = flight.DepartureDate,
            ArrivalDate = flight.ArrivalDate,
            From = flight.From,
            To = flight.To,
        };
    }
}