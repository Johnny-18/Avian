using Avian.Domain.ValueObjects;

namespace Avian.Domain.Flights;

public sealed class Flight
{
    public Flight(
        Guid id,
        Guid planeId,
        Guid[] pilots,
        FlightStatuses status,
        string? comment,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to)
    {
        Id = id;
        PlaneId = planeId;
        Pilots = pilots;
        Status = status;
        Comment = comment;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        From = from;
        To = to;
    }

    public Guid Id { get; }
    
    public Guid PlaneId { get; }
    
    public Guid[] Pilots { get; }

    public FlightStatuses Status { get; }
    
    public string? Comment { get; }
    
    public DateTimeOffset DepartureDate { get; }
    
    public DateTimeOffset? ArrivalDate { get; }
    
    public string From { get; }
    
    public string To { get; }
}