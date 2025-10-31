using Microsoft.Extensions.Configuration;

namespace CarDealer.Utils;

public class GlobalVariable
{
    static public string _connectionString;
    static public IConfiguration configuration;
    static public string projectPath;

    public GlobalVariable()
    {
        projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        Console.WriteLine(projectPath);
        Console.WriteLine(projectPath.ToString() + "\\appsettings.json");

        
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{projectPath}\\appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        string _connectionString = configuration.GetConnectionString("DefaultConnection");
        
        Console.WriteLine(_connectionString);
        
    }
}