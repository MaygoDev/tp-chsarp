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
        try
        {
            var FileContent = File.ReadAllLines(filePath);
            
            foreach (var line in FileContent)
            {
                Console.WriteLine(line);
            }
                
            List<Vehicle> vehicles = new List<Vehicle>();
            
            return vehicles;

        }
        catch (Exception ex)
        {
            throw new Exception("Error reading vehicles from CSV file", ex);
        }
    }
}