using Avian.Dal;
using Avian.Dal.Entities;
using Avian.Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Services;

public interface IPilotService
{
    Task<Pilot?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Pilot[]> GetAllAsync(CancellationToken cancellationToken);
    Task<Pilot> CreateAsync(string name, string qualification, CancellationToken cancellationToken);
}

public sealed class PilotService : IPilotService
{
    private readonly AvianContext _context;

    public PilotService(AvianContext context)
    {
        _context = context;
    }

    public async Task<Pilot?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var pilotDal = await _context.Pilots
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (pilotDal is null)
        {
            return null;
        }

        return ToDomain(pilotDal);
    }

    public async Task<Pilot[]> GetAllAsync(CancellationToken cancellationToken)
    {
        var dals = await _context.Pilots.AsNoTracking().ToArrayAsync(cancellationToken);

        return dals.Select(ToDomain).ToArray();
    }

    public async Task<Pilot> CreateAsync(string name, string qualification, CancellationToken cancellationToken)
    {
        var pilotDal = new PilotDal
        {
            Id = Guid.NewGuid(),
            Name = name,
            Qualification = qualification,
        };

        await _context.Pilots.AddAsync(pilotDal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToDomain(pilotDal);
    }

    private static Pilot ToDomain(PilotDal dal) =>
        new (dal.Id, dal.Name, dal.Qualification);
}