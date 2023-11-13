namespace Avian.Dal.Entities;

public class PilotDal
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Qualification { get; set; } = null!;
}