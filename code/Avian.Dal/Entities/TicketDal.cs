using Avian.Domain.ValueObjects;

namespace Avian.Dal.Entities;

public class TicketDal
{
    public Guid Id { get; set; }
    
    public int SeatNumber { get; set; }
    
    public decimal Price { get; set; }
    
    public TicketTypes Types { get; set; }
    
    public Guid? UserId { get; set; }
    
    public Guid PlaneId { get; set; }
}