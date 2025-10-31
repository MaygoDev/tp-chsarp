using CarDealer.Models;
using CarDealer.Utils;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data;

public class CarDealerDbContext : DbContext
{
    
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Client> Clients { get; set; }
    
    
    public CarDealerDbContext(DbContextOptions<CarDealerDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(GlobalVariable.ConnectionString);

    
}