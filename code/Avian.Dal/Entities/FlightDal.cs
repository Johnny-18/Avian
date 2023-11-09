using Avian.Domain.ValueObjects;

namespace Avian.Dal.Entities;

public class FlightDal
{
    public Guid Id { get; set; }
    
    public Guid PlaneId { get; set; }
    
    public Guid[] Pilots { get; set; } = null!;

    public FlightStatuses Status { get; set; }
    
    public string? Comment { get; set; }
    
    public DateTimeOffset DepartureDate { get; set; }
    
    public DateTimeOffset? ArrivalDate { get; set; }
    
    public string From { get; set; } = null!;
    
    public string To { get; set; } = null!;
}