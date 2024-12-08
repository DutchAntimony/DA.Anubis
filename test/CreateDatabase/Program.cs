// See https://aka.ms/new-console-template for more information

using DA.Anubis.Persistence;
using DA.Anubis.Tests.CreateDatabase.Fakers;
using DA.DDD.CoreLibrary.ServiceDefinitions;
using DA.Guards;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        var host = ConfigureHost();

        await host.StartAsync();

        using var serviceScope = host.Services.CreateScope();
        await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        Console.WriteLine("Database successfully created.");
        
        await Task.Run(() =>
        {
            var dataInserter = new RandomDataInserter(serviceScope.ServiceProvider.GetRequiredService<ILogger<RandomDataInserter>>());
            dataInserter.GenerateAndInsert(dbContext, 2);
        });
        await dbContext.SaveChangesAsync();
        
        Console.WriteLine("Database successfully filled.");
        Console.WriteLine("Closing application");
        return;

        static IHost ConfigureHost()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                .Build();

            var baseDir = configuration.GetValue<string>("BaseDir").EnsureNotEmpty();

            var host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IDateTimeProvider, DatabaseDateTimeProvider>();
                    services.AddSingleton<ICurrentUserProvider, DatabaseUserProvider>();
                    services.AddEfCoreManagedDatabase(configuration);
                    services.AddSingleton(configuration);
                })
                .UseSerilog((_, configuration) =>
                {
                    configuration.WriteTo.Console();
                    configuration.MinimumLevel.Debug();
                })
                .Build();

            return host;
        }
    }
}

public class DatabaseDateTimeProvider : IDateTimeProvider
{
    public DateOnly Today { get; } = new DateOnly(2024, 1, 1);
    public DateTime Now { get; } = new DateTime(2024, 1, 1, 12,15,0);
    public DateTime UtcNow { get; } = new DateTime(2024, 1, 1, 11, 15,0);
}

public class DatabaseUserProvider : ICurrentUserProvider
{
    public string Name { get; } = "Bogus";
    public string Email { get; } = "Bogus@anubis.com";
    public string ProfilePath { get; } = "\\Anubis";
}