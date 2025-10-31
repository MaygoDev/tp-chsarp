using Microsoft.Extensions.Configuration;

namespace CarDealer.Utils;

public class GlobalVariable
{
    static public string ConnectionString;
    static public IConfiguration Configuration;

    public GlobalVariable()
    {
        string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        
        Console.WriteLine("Loading configuration from: " + projectPath);

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{projectPath}\\appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        ConnectionString = Configuration.GetConnectionString("DefaultConnection");
    }
}