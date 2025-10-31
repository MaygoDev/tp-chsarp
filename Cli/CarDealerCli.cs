using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Cli;

public class CarDealerCli
{

    private CarDealerDbContext DbContext;

    public CarDealerCli(CarDealerDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Car Dealer CLI!");
        Console.WriteLine("1. View Vehicles");
        Console.WriteLine("2. Purchase History");
        Console.WriteLine("3. Add Client");
        Console.WriteLine("4. Add Vehicle");
        Console.WriteLine("5. Sell Vehicle");
        Console.WriteLine("6. Exit");
        Console.Write("Select an option: ");
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                showVehicles();
                break;
            case "2":
                showPurchaseHistory();
                break;
            case "3":
                sellVehicle();
                break;
            case "4":
                sellVehicle();
                break;
            case "5":
                sellVehicle();
                break;
            case "6":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option. Press any key to try again.");
                Console.ReadKey();
                start();
                break;
        }
    }

    private void showPurchaseHistory()
    {
        Console.Clear();
        this.DbContext.Vehicles.ToList()
            .Where(vehicle => vehicle.IsSold);

    }

    private void showVehicles()
    {
        Console.Clear();
        this.DbContext.Vehicles.ToList().ForEach(vehicle =>
        {
            Console.WriteLine($"ID: {vehicle.Id}, Manufacturer: {vehicle.manufacturer}, Model: {vehicle.model}, Year: {vehicle.year}");
        });
        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
        start();
    }

    private void sellVehicle()
    {
        Console.Clear();
        Vehicle vehicle = this.selectVehicle();
        Console.Clear();
        Console.WriteLine($"You selected {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) to sell.");
        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
        start();
    }

    private Vehicle selectVehicle(Vehicle selected = null)
    {
        Console.Clear();
        Console.WriteLine("Select a vehicle using arrow keys to sell:");
        // Test with sample data

        var vehicles = this.DbContext.Vehicles.ToList();
        
        for (int i = 0; i < vehicles.Count; i++)
        {
            var vehicle = vehicles[i];
            if (selected != null && vehicle.Id == selected.Id)
            {
                Console.WriteLine(
                    $"> {i + 1}. {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) - ID: {vehicle.Id}");
            }
            else
            {
                Console.WriteLine(
                    $"  {i + 1}. {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) - ID: {vehicle.Id}");
            }
        }
        // Read the key input, if arrow up or down, change selection, enter to select
        var key = Console.ReadKey();
        if (key.Key == ConsoleKey.UpArrow)
        {
            var index = selected == null ? 0 : vehicles.FindIndex(v => v.Id == selected.Id);
            index = (index - 1 + vehicles.Count) % vehicles.Count;
            return selectVehicle(vehicles[index]);
        }else if (key.Key == ConsoleKey.DownArrow)
        {
            var index = selected == null ? 0 : vehicles.FindIndex(v => v.Id == selected.Id);
            index = (index + 1) % vehicles.Count;
            return selectVehicle(vehicles[index]);
        }else if (key.Key == ConsoleKey.Enter) 
        {
            return selected;
        }
        else
        {
            return selectVehicle(selected);
        }
    }
}