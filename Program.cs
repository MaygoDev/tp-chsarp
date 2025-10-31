using CarDealer.Data;
using CarDealer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

#region

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<CarDealerDbContext>(options =>
            options.UseNpgsql(GlobalVariable._connectionString));

        // On enregistre la connexion PostgreSQL
        services.AddTransient<NpgsqlConnection>(_ =>
            new NpgsqlConnection(GlobalVariable._connectionString));

        // Et ton service Database
        services.AddTransient<Database>();
    })
    .Build();

using var scope = host.Services.CreateScope();
Database databaseService = scope.ServiceProvider.GetRequiredService<Database>();


var dbContextFactory = new PooledDbContextFactory<CarDealerDbContext>(
    new DbContextOptionsBuilder<CarDealerDbContext>()
        .UseNpgsql(GlobalVariable._connectionString)
        .Options);

using var context = dbContextFactory.CreateDbContext();



#endregion