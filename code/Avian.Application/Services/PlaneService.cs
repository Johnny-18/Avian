using Avian.Dal;
using Avian.Dal.Entities;
using Avian.Domain.Flights;
using Avian.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Avian.Application.Services;

public interface IPlaneService
{
    Task<Plane?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Plane[]> GetAllAsync(CancellationToken cancellationToken);
    Task<Plane> CreateAsync(string name, PlaneStatuses status, CancellationToken cancellationToken);
}

public sealed class PlaneService : IPlaneService
{
    private readonly AvianContext _context;

    public PlaneService(AvianContext context)
    {
        _context = context;
    }

    public async Task<Plane?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var dal = await _context.Planes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (dal is null)
        {
            return null;
        }

        return ToDomain(dal);
    }

    public async Task<Plane[]> GetAllAsync(CancellationToken cancellationToken)
    {
        var dals = await _context.Planes
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return dals.Select(ToDomain).ToArray();
    }

    public async Task<Plane> CreateAsync(string name, PlaneStatuses status, CancellationToken cancellationToken)
    {
        var planeDal = new PlaneDal
        {
            Id = Guid.NewGuid(),
            Name = name,
            Status = status,
        };

        await _context.Planes.AddAsync(planeDal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ToDomain(planeDal);
    }

    private static Plane ToDomain(PlaneDal dal) =>
        new (dal.Id, dal.Name, dal.Status);
}