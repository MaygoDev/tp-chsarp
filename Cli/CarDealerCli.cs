using System.Runtime.Serialization;
using CarDealer.Data;
using CarDealer.Models;
using CarDealer.Utils;
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
        List<Purchase> purchases = this.DbContext.Purchases
            .Include(purchase => purchase.Client)
            .Include(purchase => purchase.Vehicle)
            .ToList();
        
        purchases.Sort((a, b) => a.date.CompareTo(b.date));
        foreach (Purchase purchase in purchases)
        {
            Client? client = purchase.Client;
            Vehicle? vehicle = purchase.Vehicle;
            Console.WriteLine(
                $"Date: {purchase.date.ToShortDateString()} {purchase.date.ToShortTimeString()}, Client: {client?.firstName} {client?.lastName}, Vehicle: {vehicle?.manufacturer} {vehicle?.model} ({vehicle?.year})");
        }
        
        this.back();
    }

    private void showVehicles()
    {
        Console.Clear();
        
        List<Vehicle> vehicles = this.DbContext.Vehicles
            .Include(vehicle => vehicle.purchase).ToList();
        // Sort by sold
        vehicles.Sort((a, b) => a.isSold().CompareTo(b.isSold()));
        
        foreach (Vehicle vehicle in vehicles) {
            Console.WriteLine($"ID: {vehicle.Id}, Manufacturer: {vehicle.manufacturer}, Model: {vehicle.model}, Year: {vehicle.year}");
            
        }

        this.back();
    }

    private void back()
    {
        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
        start();
    }

    private void sellVehicle()
    {
        Console.Clear();
        Vehicle vehicle = this.selectVehicle();
        Client client = this.selectClient();
        Console.Clear();
        
        Purchase purchase = new Purchase();
        purchase.Vehicle = vehicle;
        purchase.date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        purchase.Client = client;

        this.DbContext.Purchases.Add(purchase);
        this.DbContext.SaveChanges();
        
        Console.WriteLine($"Vehicle {vehicle.manufacturer} {vehicle.model} sold to {client.firstName} {client.lastName}.");
        
        this.back();
    }

    private Vehicle selectVehicle(Vehicle selected = null)
    {
        Console.Clear();
        Console.WriteLine("Select a vehicle using arrow keys to sell:");

        var vehicles = this.DbContext.Vehicles
            .Include(vehicle => vehicle.purchase)
            .Where(vehicle => vehicle.purchase == null)
            .ToList();

        if (vehicles.Count == 0)
        {
            Console.WriteLine("No vehicles available.");
            return null;
        }

        int selectedIndex = selected == null ? 0 : vehicles.FindIndex(v => v.Id == selected.Id);
        if (selectedIndex == -1) selectedIndex = 0;

        int start = Math.Max(0, selectedIndex - 3);
        int end = Math.Min(vehicles.Count - 1, selectedIndex + 3);

        for (int i = start; i <= end; i++)
        {
            var vehicle = vehicles[i];
            if (i == selectedIndex)
            {
                Console.WriteLine($"> {i + 1}. {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) - ID: {vehicle.Id}");
            }
            else
            {
                Console.WriteLine($"  {i + 1}. {vehicle.manufacturer} {vehicle.model} ({vehicle.year}) - ID: {vehicle.Id}");
            }
        }

        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.UpArrow)
        {
            selectedIndex = (selectedIndex - 1 + vehicles.Count) % vehicles.Count;
            return selectVehicle(vehicles[selectedIndex]);
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            selectedIndex = (selectedIndex + 1) % vehicles.Count;
            return selectVehicle(vehicles[selectedIndex]);
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            return vehicles[selectedIndex];
        }
        else
        {
            return selectVehicle(vehicles[selectedIndex]);
        }
    }

    
    private Client selectClient(Client selected = null)
    {
        Console.Clear();
        Console.WriteLine("Select a client using arrow keys to sell:");

        var clients = this.DbContext.Clients.ToList();

        if (clients.Count == 0)
        {
            Console.WriteLine("No clients available.");
            return null;
        }

        int selectedIndex = selected == null ? 0 : clients.FindIndex(c => c.Id == selected.Id);
        if (selectedIndex == -1) selectedIndex = 0;

        int start = Math.Max(0, selectedIndex - 3);
        int end = Math.Min(clients.Count - 1, selectedIndex + 3);

        for (int i = start; i <= end; i++)
        {
            var client = clients[i];
            if (i == selectedIndex)
            {
                Console.WriteLine($"> {i + 1}. {client.firstName} {client.lastName} ({client.email}) - ID: {client.Id}");
            }
            else
            {
                Console.WriteLine($"  {i + 1}. {client.firstName} {client.lastName} ({client.email}) - ID: {client.Id}");
            }
        }

        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.UpArrow)
        {
            selectedIndex = (selectedIndex - 1 + clients.Count) % clients.Count;
            return selectClient(clients[selectedIndex]);
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            selectedIndex = (selectedIndex + 1) % clients.Count;
            return selectClient(clients[selectedIndex]);
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            return clients[selectedIndex];
        }
        else
        {
            return selectClient(clients[selectedIndex]);
        }
    }

}