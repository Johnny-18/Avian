using Avian.Domain.ValueObjects;

namespace Avian.Domain.Flights;

public sealed class Plane
{
    public Plane(Guid id, string name, PlaneStatuses status)
    {
        Id = id;
        Name = name;
        Status = status;
    }

    public Guid Id { get; }
    
    public string Name { get; }
    
    public PlaneStatuses Status { get; }
}