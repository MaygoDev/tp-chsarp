using Microsoft.Extensions.Configuration;

namespace CarDealer.Utils;

public class GlobalVariable
{
    static public string ConnectionString;
    static public IConfiguration Configuration;
    static public string projectPath;

    public GlobalVariable()
    {
        projectPath = Directory.GetParent(Environment.CurrentDirectory).FullName;
        // projectPath = @"C:\Users\Johann\Documents\DEV\Maygo\CarDealer"; FIXME : Modifier cette ligne si problème de configuration
        
        Console.WriteLine("Loading configuration from: " + projectPath);

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{projectPath}\\appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        ConnectionString = Configuration.GetConnectionString("DefaultConnection");
    }
}