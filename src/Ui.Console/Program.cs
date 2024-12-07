// See https://aka.ms/new-console-template for more information

using DA.Anubis.Infrastructure;
using DA.Anubis.Persistence;
using DA.DDD.ApplicationLibrary;
using DA.Guards;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

var host = ConfigureHost();
await host.StartAsync();

using var serviceScope = host.Services.CreateScope();
await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

Console.WriteLine("Database successfully created");
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
            services.AddInfrastructure();
            services.AddEfCoreManagedDatabase(configuration);
            services.AddSingleton(configuration);
            services.AddSerilogLogging(baseDir);
        })
        .Build();

    return host;
}

public partial class Program;