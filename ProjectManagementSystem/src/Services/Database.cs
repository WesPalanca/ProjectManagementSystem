using MySql.Data.MySqlClient;

namespace ProjectManagementSystem.Services;
using DotNetEnv;

// Singleton Pattern
public class Database
{
    private static Database? _instance = null;
    
    public MySqlConnection Connection { get; private set; }

    private Database()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
        Env.Load(Path.Combine(projectRoot, ".env"));
        string password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        string connectionString = $"Server=localhost;Database=ProjectManagementSystemDB;User ID=root;Password={password}";
        Connection = new MySqlConnection(connectionString);
        Connection.Open();
        Console.WriteLine("Connected to MySQL!");
        
        
    }

    public static Database GetInstance()
    {
        if (_instance == null) _instance = new Database();
        return _instance;
    }
}