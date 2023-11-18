﻿using Avian.Dal;
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

    private static Flight ToDomain(FlightDal dal) =>
        new (dal.Id, dal.PlaneId, dal.Pilots, dal.Status, dal.Comment, dal.DepartureDate, dal.ArrivalDate, dal.From, dal.To);
}