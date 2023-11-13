using Avian.Application.Generators;
using Avian.Application.Services;
using Avian.Dal;
using Microsoft.EntityFrameworkCore;

namespace Avian.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddDbContext<AvianContext>((_, options) =>
        {
            var connectionString = configuration.GetConnectionString("Avian");

            options.UseNpgsql(connectionString);
        });

        services.AddSingleton<HashGenerator>();
        
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IFlightService, FlightService>();

        return services;
    }
}