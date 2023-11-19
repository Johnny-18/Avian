using Avian.Dal;
using Avian.Dal.Entities;
using Avian.Domain.Flights;
using Avian.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Services;

public interface IFlightService
{
    Task<Flight?> GetFlightByIdAsync(Guid flightId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Flight>> GetFlightsAsync(CancellationToken cancellationToken);
    Task<Flight> CreateAsync(
        Guid planeId,
        Guid[] pilotIds,
        FlightStatuses status,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to,
        CancellationToken cancellationToken);
}

public sealed class FlightService : IFlightService
{
    private readonly AvianContext _context;

    public FlightService(AvianContext context)
    {
        _context = context;
    }
    
    public async Task<Flight?> GetFlightByIdAsync(Guid flightId, CancellationToken cancellationToken)
    {
        var dal = await _context.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == flightId, cancellationToken);
        if (dal is null)
        {
            return null;
        }

        return ToDomain(dal);
    }

    public async Task<IReadOnlyCollection<Flight>> GetFlightsAsync(CancellationToken cancellationToken)
    {
        var dals = await _context.Flights
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return dals.Select(ToDomain).ToArray();
    }

    public async Task<Flight> CreateAsync(
        Guid planeId,
        Guid[] pilotIds,
        FlightStatuses status,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        ValidateCreation(status, departureDate, arrivalDate, from, to, pilotIds);
        
        var planeDal = await _context.Planes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == planeId, cancellationToken);
        if (planeDal is null)
        {
            throw new ApplicationException("Plane not found! Invalid id");
        }

        var countPilots = await _context.Pilots
            .AsNoTracking()
            .Where(x => pilotIds.Contains(x.Id))
            .CountAsync(cancellationToken: cancellationToken);
        if (countPilots != pilotIds.Length)
        {
            throw new ApplicationException("Pilots not found! Invalid ids");
        }
        
        var flightDal = new FlightDal
        {
            Id = Guid.NewGuid(),
            PlaneId = planeId,
            Pilots = pilotIds,
            Status = status,
            Comment = null,
            DepartureDate = departureDate,
            ArrivalDate = arrivalDate,
            From = from,
            To = to,
        };

        await _context.Flights.AddAsync(flightDal, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return ToDomain(flightDal);
    }

    private void ValidateCreation(
        FlightStatuses status,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to,
        IReadOnlyCollection<Guid> pilotIds)
    {
        pilotIds = pilotIds.Distinct().ToArray();
        if (!pilotIds.Any() || pilotIds.Count > 3)
        {
            throw new ApplicationException("Invalid count of pilots!");
        }
        
        if (status is FlightStatuses.Canceled or FlightStatuses.Completed)
        {
            throw new ApplicationException("Invalid status for flight!");
        }

        if (departureDate < DateTimeOffset.UtcNow)
        {
            throw new ApplicationException("Invalid departure date!");
        }

        if (departureDate > arrivalDate)
        {
            throw new ApplicationException("Arrival date must be after departure date!");
        }

        if (from == to || string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
        {
            throw new ApplicationException("Invalid cities!");
        }
    }

    private static Flight ToDomain(FlightDal dal) =>
        new (dal.Id, dal.PlaneId, dal.Pilots, dal.Status, dal.Comment, dal.DepartureDate, dal.ArrivalDate, dal.From, dal.To);
}