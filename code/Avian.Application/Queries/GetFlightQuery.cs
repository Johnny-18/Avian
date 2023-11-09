using Avian.Application.Interfaces;
using Avian.Dal;
using Avian.Dal.Entities;
using Avian.Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Queries;

public sealed class GetFlightQuery : IQuery<Flight?>
{
    public required Guid FlightId { get; init; }
}

public sealed class GetFlightQueryHandler : IQueryHandler<GetFlightQuery, Flight?>
{
    private readonly AvianContext _context;

    public GetFlightQueryHandler(AvianContext context)
    {
        _context = context;
    }

    public async Task<Flight?> Handle(GetFlightQuery request, CancellationToken cancellationToken)
    {
        var dal = await _context.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.FlightId, cancellationToken);

        return ToDomain(dal);
    }

    private static Flight? ToDomain(FlightDal? dal) =>
        dal is null
            ? null
            : Flight.Create(dal.Id, dal.PlaneId, dal.Pilots, dal.Status, dal.Comment, dal.DepartureDate, dal.ArrivalDate, dal.From, dal.To);
}