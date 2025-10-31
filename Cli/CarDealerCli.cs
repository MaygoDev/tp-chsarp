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
                addClient();
                break;
            case "4":
                addVehicle();
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

    private void addVehicle()
    {
        Console.Clear();
        Vehicle vehicle = new Vehicle();
        Console.Write("Manufacturer: ");
        vehicle.manufacturer = Console.ReadLine();
        Console.Write("Model: ");
        vehicle.model = Console.ReadLine();
        Console.Write("Year: ");
        vehicle.year = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Price Excl. Tax: ");
        vehicle.priceExclTax = double.Parse(Console.ReadLine() ?? "0");
        vehicle.priceInclTax = vehicle.priceExclTax * 1.2;
        Console.Write("Color: ");
        vehicle.color = Console.ReadLine();

        this.DbContext.Vehicles.Add(vehicle);
        this.DbContext.SaveChanges();
        
        Console.WriteLine($"Vehicle {vehicle.manufacturer} {vehicle.model} added successfully.");
        
        this.back();
    }

    private void addClient()
    {
        Console.Clear();
        Client client = new Client();
        Console.Write("First Name: ");
        client.firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        client.lastName = Console.ReadLine();
        Console.Write("Birth Date (dd/MM/yyyy): ");
        string birthDateInput = Console.ReadLine();
        client.birthDate = DateTImeFormater.FormatDateTime(birthDateInput);
        Console.Write("Phone Number: ");
        client.phoneNumber = Console.ReadLine();
        Console.Write("Email: ");
        client.email = Console.ReadLine();

        this.DbContext.Clients.Add(client);
        this.DbContext.SaveChanges();
        
        Console.WriteLine($"Client {client.firstName} {client.lastName} added successfully.");
        
        this.back();
    }

    private void showPurchaseHistory()
    {
        Console.Clear();
        List<Purchase> purchases = this.DbContext.Purchases
            .Include(purchase => purchase.Client)
            .Include(purchase => purchase.Vehicle)
            .ToList();
        
        purchases.Sort((a, b) => a.date.CompareTo(b.date));
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{"Date",-20} {"Client",-30} {"Vehicle",-30}");
        Console.ResetColor();

        Console.WriteLine(new string('-', 85));
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        foreach (Purchase purchase in purchases)
        {
            Client? client = purchase.Client;
            Vehicle? vehicle = purchase.Vehicle;
          
            string date = $"{purchase.date:dd/MM/yyyy HH:mm}";
            string clientName = $"{client?.firstName} {client?.lastName}";
            string vehicleName = $"{vehicle?.manufacturer} {vehicle?.model} ({vehicle?.year})";

            Console.WriteLine($"{date,-20} {clientName,-30} {vehicleName,-30}");
            
        }
        
        this.back();
    }

    private void showVehicles()
    {
        Console.Clear();

        var vehicles = this.DbContext.Vehicles
            .Include(vehicle => vehicle.purchase)
            .ToList();

        // Tri : d’abord les non vendus, puis les vendus
        vehicles.Sort((a, b) => a.isSold().CompareTo(b.isSold()));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{"Manufacturer",-15} {"Model",-15} {"Year",-6} {"Color",-20} {"Excl.Tax",-10} {"Incl.Tax",-10} {"Status",-10}");
        Console.ResetColor();

        Console.WriteLine(new string('-', 85));

        foreach (var vehicle in vehicles)
        {
            bool sold = vehicle.isSold();
            Console.ForegroundColor = sold ? ConsoleColor.DarkGray : ConsoleColor.Green;

            string status = sold ? "SOLD" : "AVAILABLE";

            Console.WriteLine($"{vehicle.manufacturer,-15} {vehicle.model,-15} {vehicle.year,-6} {vehicle.color,-20} {vehicle.priceExclTax,-10:C0} {vehicle.priceInclTax,-10:C0} {status,-10}");
        }

        this.back();
    }

    private void back()
    {

        Console.ResetColor();
        Console.WriteLine("Press any key to return to main menu.");
        Console.ReadKey();
        start();
    }

    private void sellVehicle()
    {
        Console.Clear();
        Vehicle vehicle = this.selectVehicle();
        if (vehicle == null)
        {
            this.back();
            return;
        }
        Client client = this.selectClient();
        if (client == null)
        {
            this.back();
            return;
        }
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

    private Vehicle? selectVehicle(Vehicle selected = null)
    {
        Console.Clear();
        Console.WriteLine("Select a vehicle using arrow keys to sell (Press C to Cancel):");

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
        else if (key.Key == ConsoleKey.C)
        {
            return null;
        }
        else
        {
            return selectVehicle(vehicles[selectedIndex]);
        }
    }

    
    private Client? selectClient(Client selected = null)
    {
        Console.Clear();
        Console.WriteLine("Select a client using arrow keys to sell (Press C to Cancel):");

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
        else if (key.Key == ConsoleKey.C)
        {
            return null;
        }
        else
        {
            return selectClient(clients[selectedIndex]);
        }
    }

}