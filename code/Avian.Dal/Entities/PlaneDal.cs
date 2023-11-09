using Avian.Domain.ValueObjects;

namespace Avian.Dal.Entities;

public class PlaneDal
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public PlaneStatuses Status { get; set; }
}