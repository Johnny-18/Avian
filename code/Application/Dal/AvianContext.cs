using Application.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Dal;

public sealed class AvianContext : DbContext
{
    public AvianContext(DbContextOptions<AvianContext> options)
        : base(options)
    {
    }

    public DbSet<TicketDal> Tickets => Set<TicketDal>();

    public DbSet<FlightDal> Flights => Set<FlightDal>();

    public DbSet<PilotDal> Pilots => Set<PilotDal>();

    public DbSet<PlaneDal> Planes => Set<PlaneDal>();
    
    public DbSet<UserDal> Users => Set<UserDal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<OrderWebhook>().ToTable("order_webhooks");
        // modelBuilder.Entity<OrderWebhook>(builder =>
        // {
        //     builder.HasKey(x => x.CustomerId);
        //     builder.Property(x => x.CustomerId).ValueGeneratedNever().HasColumnName("customer_id");
        //     builder.Property(x => x.Key).HasColumnName("key");
        //     builder.Property(x => x.Url).HasColumnName("url");
        //     builder.Property<uint>("xmin").IsRowVersion();
        // });
        //
        // modelBuilder.Entity<DriverWebhook>().ToTable("driver_webhooks");
        // modelBuilder.Entity<DriverWebhook>(builder =>
        // {
        //     builder.HasKey(x => x.CustomerId);
        //     builder.Property(x => x.CustomerId).HasColumnName("customer_id").ValueGeneratedNever();
        //     builder.Property(x => x.Key).HasColumnName("key");
        //     builder.Property(x => x.Url).HasColumnName("url");
        //     builder.Property<uint>("xmin").IsRowVersion();
        // });
        //
        // modelBuilder.Entity<User>().ToTable("users");
        // modelBuilder.Entity<User>(builder =>
        // {
        //     builder.HasKey(x => x.Id);
        //     builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        //     builder.Property(x => x.Name).HasColumnName("name");
        //     builder.Property(x => x.PhoneNumber)
        //         .HasColumnName("phone_number")
        //         .HasConversion(
        //             x => x.Value,
        //             x => PhoneNumber.Create(x));
        //     builder.HasIndex(x => x.PhoneNumber);
        //     builder.Property(x => x.Locale).HasColumnName("locale");
        //     builder.Property(x => x.CompanyId).HasColumnName("company_id");
        //     builder.HasIndex(x => x.CompanyId);
        // });
        //
        // modelBuilder.Entity<Company>().ToTable("companies");
        // modelBuilder.Entity<Company>(builder =>
        // {
        //     builder.HasKey(x => x.Id);
        //     builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
        //     builder.Property(x => x.IsPurchaseAllowed).HasColumnName("is_purchase_allowed");
        // });
    }
}