using System.Runtime.Serialization;
using CarDealer.Models;
using CarDealer.Utils;

namespace CarDealer.Data;

public interface ICSVReader
{
    List<Client> GetClients(String filePath);
    List<Vehicle> GetVehicle(String filePath);
}

public class CSVReader : ICSVReader
{
    public List<Client> GetClients(string filePath)
    { 
        // Méthode pour lire les clients depuis un fichier CSV
        try
        {
            var FileContent = File.ReadAllLines(filePath);
            List<Client> clients = new List<Client>();
            
            foreach (var line in FileContent.Skip(1))
            {
                Client newClient = new Client();

                string[] fields = line.Split("%");
                
                newClient.firstName =fields[1];
                newClient.lastName = fields[0];

                string rawBirthDate = fields[2];

                newClient.birthDate = DateTImeFormater.FormatDateTime(rawBirthDate);
                newClient.phoneNumber = fields[3];
                newClient.email = fields[4];
                clients.Add(newClient);
                
            }
            
            Console.WriteLine(clients.Count + " clients lus depuis le fichier CSV.");
            
            return clients;

        }
        catch (Exception ex)
        {
            throw new Exception("Error reading clients from CSV file", ex);
        }
    }
    
    public List<Vehicle> GetVehicle(string filePath)
    {
        // Méthode pour lire les véhicules depuis un fichier CSV
        try
        {
            var FileContent = File.ReadAllLines(filePath);
            List<Vehicle> vehicles = new List<Vehicle>();
            
            foreach (var line in FileContent.Skip(1))
            {
                Vehicle newVehicle = new Vehicle();

                string[] fields = line.Split("/");
                
                newVehicle.manufacturer =fields[0];
                newVehicle.model = fields[1];
                
                newVehicle.year = Convert.ToInt16(fields[2]);
                newVehicle.priceExclTax =Convert.ToDouble(fields[3]);
                newVehicle.priceInclTax =Math.Round(Convert.ToDouble(fields[3])*1.2,2);
                newVehicle.color = fields[4];
                newVehicle.csvPurchased = bool.Parse(fields[5]);
                
                vehicles.Add(newVehicle);
            }
            
            Console.WriteLine(vehicles.Count + " vehicules lus depuis le fichier CSV.");
            
            return vehicles;

        }
        catch (Exception ex)
        {
            throw new Exception("Error reading vehicles from CSV file", ex);
        }
    }
}