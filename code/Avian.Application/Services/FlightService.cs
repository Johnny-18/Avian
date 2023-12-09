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
        Guid? planeId,
        Guid[]? pilotIds,
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
        Guid? planeId,
        Guid[]? pilotIds,
        FlightStatuses status,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to,
        CancellationToken cancellationToken)
    {
        await ValidateCreation(status, departureDate, arrivalDate, from, to, pilotIds, planeId, cancellationToken);

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

        _context.Flights.Add(flightDal);
        await _context.SaveChangesAsync(cancellationToken);

        return ToDomain(flightDal);
    }

    private async Task ValidateCreation(
        FlightStatuses status,
        DateTimeOffset departureDate,
        DateTimeOffset? arrivalDate,
        string from,
        string to,
        Guid[]? pilotIds,
        Guid? planeId,
        CancellationToken cancellationToken)
    {
        if (pilotIds is not null)
        {
            await ValidatePilots(pilotIds, cancellationToken);
        }
                
        if (planeId is not null)
        {
            await ValidatePlane(planeId.Value, cancellationToken);
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

    private async Task ValidatePlane(Guid planeId, CancellationToken cancellationToken)
    {
        var planeDal = await _context.Planes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == planeId, cancellationToken);
        if (planeDal is null)
        {
            throw new ApplicationException("Plane not found! Invalid id");
        }
    }

    private async Task ValidatePilots(Guid[] pilotIds, CancellationToken cancellationToken)
    {
        pilotIds = pilotIds.Distinct().ToArray();
        if (pilotIds.Length > 3)
        {
            throw new ApplicationException("Invalid count of pilots!");
        }
        
        var countPilots = await _context.Pilots
            .AsNoTracking()
            .Where(x => pilotIds.Contains(x.Id))
            .CountAsync(cancellationToken: cancellationToken);
        if (countPilots != pilotIds.Length)
        {
            throw new ApplicationException("Pilots not found! Invalid ids");
        }
    }

    private static Flight ToDomain(FlightDal dal) =>
        new (dal.Id, dal.PlaneId, dal.Pilots, dal.Status, dal.Comment, dal.DepartureDate, dal.ArrivalDate, dal.From, dal.To);
}