using CarDealer.Models;
using CarDealer.Utils;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data;

public class CarDealerDbContext : DbContext
{
    
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    
    public CarDealerDbContext(DbContextOptions<CarDealerDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Purchases)
            .HasForeignKey(p => p.client)
            .OnDelete(DeleteBehavior.Cascade);
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql(GlobalVariable.ConnectionString);

    
}