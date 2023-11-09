using Avian.Domain.ValueObjects;

namespace Avian.Domain.Flights;

public sealed class Flight
{
    private Flight(
        Guid id,
        Guid planeId,
        Guid[] pilots,
        FlightStatuses status,
        string? comment,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        City from,
        City to)
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

    public static Flight Create(
        Guid id,
        Guid planeId,
        Guid[] pilots,
        FlightStatuses status,
        string? comment,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        City from,
        City to)
    {
        if (from == to)
        {
            throw new DomainException("cities_is_equal", "The city of departure cannot be the city of arrival");
        }
        
        return new Flight(id, planeId, pilots, status, comment, departureDate, arrivalDate, from, to);
    }

    public Guid Id { get; }
    
    public Guid PlaneId { get; }
    
    public Guid[] Pilots { get; }

    public FlightStatuses Status { get; }
    
    public string? Comment { get; }
    
    public DateTimeOffset DepartureDate { get; }
    
    public DateTimeOffset? ArrivalDate { get; }
    
    public City From { get; }
    
    public City To { get; }
}

public sealed record City(string Name);