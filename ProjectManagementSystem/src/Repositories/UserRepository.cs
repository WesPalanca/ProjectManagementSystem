using ProjectManagementSystem.Factory;

namespace ProjectManagementSystem.Repositories;
using MySql.Data.MySqlClient;
using ProjectManagementSystem.Models;
public class UserRepository : IUserRepository
{
    private readonly MySqlConnection _connection;
    private readonly IUserFactory _userFactory;
    public UserRepository(MySqlConnection connection, IUserFactory userFactory) 
    {
        _connection = connection;
        _userFactory = userFactory;
    }
    

    public void Add(User user, string password)
    {
        string query = @"INSERT INTO Users (FirstName, LastName, Email, Password, Role)
                         VALUES (@first, @last, @email, @pass, @role)";

        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@first", user.FirstName);
        cmd.Parameters.AddWithValue("@last", user.LastName);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@pass", password); // raw should be hashed later
        cmd.Parameters.AddWithValue("@role", user.Role);

        cmd.ExecuteNonQuery();
        user.UserId = (int)cmd.LastInsertedId;
    }

    public User? GetById(int id)
    {
        string query = @"SELECT * FROM Users WHERE UserId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        using MySqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        User user = _userFactory.CreateUser(reader.GetString("FirstName"), 
            reader.GetString("LastName"), 
            reader.GetString("Email"), 
            reader.GetString("Password"),
            reader.GetString("Role"));
        user.UserId = reader.GetInt32("UserId");
        return user;

    }
    public User? GetByEmail(string email)
    {
        
        string query = @"SELECT * FROM Users WHERE Email = @email";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@email", email);
        using MySqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        
        User user = _userFactory.CreateUser(reader.GetString("FirstName"), 
            reader.GetString("LastName"), 
            reader.GetString("Email"), 
            reader.GetString("Password"),
            reader.GetString("Role"));
        user.UserId = reader.GetInt32("UserId");
        
        

        return user;
        
    }

    public List<User> GetAll()
    {
        List<User> users = new List<User>();
        string query = @"SELECT * FROM Users";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            User user = _userFactory.CreateUser(reader.GetString("FirstName"), 
                reader.GetString("LastName"), 
                reader.GetString("Email"), 
                reader.GetString("Password"),
                reader.GetString("Role"));
            user.UserId = reader.GetInt32("UserId");
            users.Add(user);
        }

        return users;
      
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Users WHERE UserId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
