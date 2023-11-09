using Avian.Dal;
using Microsoft.EntityFrameworkCore;

namespace Avian.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddDbContext<AvianContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("Avian");

            options.UseNpgsql(connectionString);
        });

        return services;
    }
}