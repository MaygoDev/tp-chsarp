using System.Runtime.CompilerServices;
using CarDealer.Cli;
using CarDealer.Data;
using CarDealer.Models;
using CarDealer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

// Used only to load configuration at startup
new GlobalVariable();

#region

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<CarDealerDbContext>(options =>
            options.UseNpgsql(GlobalVariable.ConnectionString));

        services.AddTransient<NpgsqlConnection>(_ =>
            new NpgsqlConnection(GlobalVariable.ConnectionString));
        
        services.AddTransient<Database>();
        
        services.AddTransient<ICSVReader, CSVReader>();

    })
    .Build();


using var scope = host.Services.CreateScope();
var provider = scope.ServiceProvider;

var databaseService = provider.GetRequiredService<Database>();
var csvReader = provider.GetRequiredService<ICSVReader>();

var clients = csvReader.GetClients(GlobalVariable.projectPath + "\\Data\\clients.csv");
var vehicles = csvReader.GetVehicle(GlobalVariable.projectPath + "\\Data\\voitures.csv");

var dbContextFactory = new PooledDbContextFactory<CarDealerDbContext>(
    new DbContextOptionsBuilder<CarDealerDbContext>()
        .UseNpgsql(GlobalVariable.ConnectionString)
        .Options);

using var context = dbContextFactory.CreateDbContext();

#region Insertion

// Check if database is empty before inserting sample data
if (!context.Vehicles.Any() && !context.Clients.Any())
{
    Console.WriteLine("Inserting sample data into the database...");

    context.Clients.AddRange(clients);
    context.Vehicles.AddRange(vehicles);
    
    /*
     * TODO : Purchases
     * Avec dates aléatoires
     */
    
    context.SaveChanges();
} else {
    Console.WriteLine("Database already contains data, skipping sample data insertion.");
}

#endregion

CarDealerCli app = new CarDealerCli(context);
app.start();

#endregion