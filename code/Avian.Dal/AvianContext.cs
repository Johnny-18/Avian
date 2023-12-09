using Avian.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avian.Dal;

public sealed class AvianContext : DbContext
{
    public AvianContext(DbContextOptions<AvianContext> options)
        : base(options)
    {
    }

    // public AvianContext()
    // {
    //     
    // }
    //
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Host=localhost; Database=avian; Username=postgres; Password=1Password!");
    //     base.OnConfiguring(optionsBuilder);
    // }

    public DbSet<TicketDal> Tickets { get; set; } = null!;

    public DbSet<FlightDal> Flights { get; set; } = null!;

    public DbSet<PilotDal> Pilots { get; set; } = null!;

    public DbSet<PlaneDal> Planes { get; set; } = null!;
    
    public DbSet<UserDal> Users { get; set; } = null!;

    public DbSet<PlanePilotDal> PlanePilots { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlightDal>().ToTable("flights");
        modelBuilder.Entity<FlightDal>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.PlaneId).IsRequired(false);
            builder.Property(x => x.Pilots).IsRequired(false);
            builder.Property(x => x.From);
            builder.Property(x => x.To);
            builder.Property(x => x.Status);
            builder.Property(x => x.DepartureDate);
            builder.Property(x => x.ArrivalDate).IsRequired(false);
            builder.Property(x => x.Comment).IsRequired(false);
        });
        
        modelBuilder.Entity<PilotDal>().ToTable("pilots");
        modelBuilder.Entity<PilotDal>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name);
            builder.Property(x => x.Qualification);
        });
        
        modelBuilder.Entity<PlaneDal>().ToTable("planes");
        modelBuilder.Entity<PlaneDal>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name);
            builder.Property(x => x.Status);
        });
        
        modelBuilder.Entity<PlanePilotDal>().ToTable("plane_pilot");
        modelBuilder.Entity<PlanePilotDal>(builder =>
        {
            builder.HasKey(x => new { x.PlaneId, x.PilotId });
            builder.Property(x => x.PlaneId).ValueGeneratedNever();
            builder.Property(x => x.PilotId).ValueGeneratedNever();
        });
        
        modelBuilder.Entity<TicketDal>().ToTable("tickets");
        modelBuilder.Entity<TicketDal>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Price);
            builder.Property(x => x.Type);
            builder.Property(x => x.SeatNumber);
            builder.Property(x => x.PlaneId);
            builder.Property(x => x.UserId);
        });
        
        modelBuilder.Entity<UserDal>().ToTable("users");
        modelBuilder.Entity<UserDal>(builder =>
        {
            builder.HasKey(x => x.Email);
            builder.Property(x => x.Email);
            builder.Property(x => x.PasswordHash);
            builder.Property(x => x.Type);
        });
    }
}