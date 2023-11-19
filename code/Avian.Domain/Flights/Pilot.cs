namespace Avian.Domain.Flights;

public sealed class Pilot
{
    public Pilot(Guid id, string name, string qualification)
    {
        Id = id;
        Name = name;
        Qualification = qualification;
    }

    public Guid Id { get; }
    
    public string Name { get; }

    public string Qualification { get; }
}