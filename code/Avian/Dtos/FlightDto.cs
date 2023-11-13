using System.Text.Json.Serialization;
using Avian.Domain.Flights;

namespace Avian.Dtos;

public sealed class FlightDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("plane_id")]
    public Guid PlaneId { get; set; }

    [JsonPropertyName("pilots")]
    public Guid[] Pilots { get; set; } = null!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
    
    [JsonPropertyName("departure_date")]
    public DateTimeOffset DepartureDate { get; set; }
    
    [JsonPropertyName("arrival_date")]
    public DateTimeOffset? ArrivalDate { get; set; }
    
    [JsonPropertyName("from")]
    public string From { get; set; } = null!;
    
    [JsonPropertyName("to")]
    public string To { get; set; } = null!;

    public static FlightDto FromDomain(Flight flight)
    {
        return new FlightDto
        {
            Id = flight.Id,
            PlaneId = flight.PlaneId,
            Pilots = flight.Pilots,
            Status = flight.Status.ToString(),
            Comment = flight.Comment,
            DepartureDate = flight.DepartureDate,
            ArrivalDate = flight.ArrivalDate,
            From = flight.From,
            To = flight.To,
        };
    }
}