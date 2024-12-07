using DA.DDD.CoreLibrary.ServiceDefinitions;
using DA.Guards;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Anubis.Persistence;

public static class PersistenceDependencyInjectionExtensions
{
    public static IServiceCollection AddEfCoreManagedDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var dbName = configuration.GetValue<string>("DbName");
        var baseDir = configuration.GetValue<string>("BaseDir");
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder()
        {
            DataSource = Path.Combine(
                baseDir.EnsureNotEmpty("Configure baseDir in config.json"),
                "Data",
                dbName.EnsureNotEmpty("Configure dbName in config.json"))
        }.ToString());
        
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connection, sqliteOptions
                        => sqliteOptions.MigrationsAssembly(typeof(IPersistenceMarker).Assembly))
#if DEBUG
                    .EnableSensitiveDataLogging(true)
#endif
        );

        services.AddScoped<IDbContext, ApplicationDbContext>();
        services.AddScoped<IUnitOfWork, ApplicationDbContext>();

        // todo: deze mag denk ik naar de Application zodra daar wat gevuld is.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IPersistenceMarker>());
        return services;
    }
}