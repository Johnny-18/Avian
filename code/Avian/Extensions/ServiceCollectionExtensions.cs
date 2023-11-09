using System.Windows.Input;
using Avian.Application.Services;
using Avian.Dal;
using MediatR;
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

        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
    
    internal static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<ICommand>();
        });

        return services;
    }
}