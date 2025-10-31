using CarDealer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

#region

string defaultProjetPath = @"C:\Users\Johann\Documents\DEV\Maygo\CarDealer";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"{defaultProjetPath}\\appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<CarDealerDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // On enregistre la connexion PostgreSQL
        services.AddTransient<NpgsqlConnection>(_ =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

        // Et ton service Database
        services.AddTransient<Database>();
    })
    .Build();

using var scope = host.Services.CreateScope();
Database databaseService = scope.ServiceProvider.GetRequiredService<Database>();


var dbContextFactory = new PooledDbContextFactory<CarDealerDbContext>(
    new DbContextOptionsBuilder<CarDealerDbContext>()
        .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        .Options);

using var context = dbContextFactory.CreateDbContext();



#endregion