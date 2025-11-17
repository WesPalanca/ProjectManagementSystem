using MySql.Data.MySqlClient;
using ProjectManagementSystem.Services;
using DotNetEnv;
namespace ProjectManagementSystem;


// Tests connection to MySql DB
class Program
{
    static void Main(string[] args)
    {
        try
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            Env.Load(Path.Combine(projectRoot, ".env"));
            string secret = Environment.GetEnvironmentVariable("SECRET");
            Console.WriteLine($"SECRET = {secret}");
            
            var db = Database.GetInstance();

            
            string query = "SELECT * FROM Users;";
            using var cmd = new MySqlCommand(query, db.Connection);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("Users in the database:");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["UserId"]}, Name: {reader["FirstName"]} {reader["LastName"]}, Email: {reader["Email"]}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Test complete.");
        }
        
    }
}