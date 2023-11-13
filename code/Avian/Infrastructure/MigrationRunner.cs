using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

namespace Avian.Infrastructure;

public sealed class MigrationRunner
{
    private const string RunEnvironmentVariable = "MIGRATIONS_RUN";
    private const string TimeoutEnvironmentVariable = "MIGRATIONS_TIMEOUT";
    private const string VersionEnvironmentVariable = "MIGRATIONS_VERSION";
    private const string None = "none";
    private const string Up = "up";
    private const string Down = "down";

    // Set "MIGRATIONS_RUN=up" for init container
    // Set "MIGRATIONS_RUN=none" for actual pod after migrations were applied
    // Set MIGRATIONS_VERSION to exact name of the migration to migrate to the specific version, if required
    // Set MIGRATIONS_TIMEOUT to specify database operation integer timeout in seconds

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public enum Result
    {
        TerminateService,
        RunService
    }

    private MigrationRunner(IServiceProvider serviceProvider, ILogger<MigrationRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public static MigrationRunner Create(IHost host)
    {
        var logger = host.Services.GetRequiredService<ILogger<MigrationRunner>>();
        return new MigrationRunner(host.Services, logger);
    }

    public async Task<Result> Apply<TContext>() where TContext : DbContext
    {
        var contextName = typeof(TContext).Name;

        var runValue = Environment.GetEnvironmentVariable(RunEnvironmentVariable);
        var timeoutValue = Environment.GetEnvironmentVariable(TimeoutEnvironmentVariable);
        var versionValue = Environment.GetEnvironmentVariable(VersionEnvironmentVariable);

        if (string.Equals(runValue, None, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Skipped applying migrations for context {ContextName}", contextName);
            return Result.RunService;
        }

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        if (timeoutValue is not null && int.TryParse(timeoutValue, out int commandTimeout))
        {
            _logger.LogInformation("Command timeout was set to {CommandTimeout} seconds for context {ContextName}", commandTimeout, contextName);
            context.Database.SetCommandTimeout(commandTimeout);
        }

        if (runValue is null)
        {
            _logger.LogWarning($"'{RunEnvironmentVariable}' environment variable was not specified. Consider specifying this variable on production to separate migrations from service start on production environment.");
            await MigrateDatabase(context, migrateUp: true, migrationName: null);
            await using var conn = context.Database.GetDbConnection();
            if (conn is NpgsqlConnection npgsqlConnection)
            {
                await npgsqlConnection.OpenAsync();
                await npgsqlConnection.ReloadTypesAsync();
            }

            return Result.RunService;
        }

        if (string.Equals(runValue, Up, StringComparison.OrdinalIgnoreCase))
        {
            await MigrateDatabase(context, migrateUp: true, versionValue);
        }
        else if (string.Equals(runValue, Down, StringComparison.OrdinalIgnoreCase))
        {
            await MigrateDatabase(context, migrateUp: false, versionValue);
        }
        else
        {
            throw new ArgumentException($"Possible values for '{RunEnvironmentVariable}' environment variable is '{Up}', '{Down}' or '{None}'");
        }

        _logger.LogInformation("Requested service termination");
        return Result.TerminateService;
    }

    private async Task MigrateDatabase<TContext>(TContext context, bool migrateUp, string? migrationName) where TContext : DbContext
    {
        var contextName = typeof(TContext).Name;
        if (migrationName == null)
        {
            if (!migrateUp)
            {
                throw new ArgumentException("Migration name should be specified to migrate down");
            }

            _logger.LogInformation("Starting migration of {ContextName} to latest migration", contextName);
            await context.Database.MigrateAsync();
        }
        else
        {
            var migrationDirection = migrateUp ? Up : Down;
            _logger.LogInformation("Starting migration of {ContextName} {MigrationDirection} to migration {MigrationName}", contextName, migrationDirection, migrationName);

            var targetMigrationCollection = migrateUp
                ? await context.Database.GetPendingMigrationsAsync()
                : await context.Database.GetAppliedMigrationsAsync();

            if (!targetMigrationCollection.Contains(migrationName))
            {
                var migrationType = migrateUp ? "pending" : "applied";
                throw new ArgumentException($"Could not migrate {migrationDirection} to migration {migrationName} because it was not found in {migrationType} migrations for context {contextName}.");
            }

            var migrator = context.GetInfrastructure().GetRequiredService<IMigrator>();
            await migrator.MigrateAsync(migrationName);
        }
        _logger.LogInformation("Migration of {ContextName} completed", contextName);
    }
}