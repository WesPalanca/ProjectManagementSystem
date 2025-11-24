using ProjectManagementSystem.Factory;

namespace ProjectManagementSystem.Repositories;
using MySql.Data.MySqlClient;
using ProjectManagementSystem.Models;
public class UserRepository : IUserRepository
{
    private readonly MySqlConnection _connection;
    // todo: in memory dictionary
    
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    public UserRepository(MySqlConnection connection) 
    {
        _connection = connection;
        PopulateUsers();
    }

    private void PopulateUsers()
    {
        string query = "SELECT * FROM users";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();
        IUserFactory userFactory = new UserFactory(); // todo: should be changed later 
        while (reader.Read())
        {
            User? user = userFactory.CreateUser(
                reader.GetString("FirstName"),
                reader.GetString("LastName"),
                reader.GetString("Email"),
                reader.GetString("Password"),
                reader.GetString("Role"));
            user.UserId = reader.GetInt32("UserId");
            _users[user.UserId] = user;
        }
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
        _users[user.UserId] = user;
    }

    public User? GetById(int id)
    {
        if (_users.ContainsKey(id))
        {
            return _users[id];
        }

        return null;
       
    }
    public User? GetByEmail(string email)
    {
        foreach (User user in _users.Values)
        {
            if (user.Email == email)
            {
                return user;
            }
        }

        return null;
        
    }

    public List<User> GetAll()
    {
        return _users.Values.ToList();
      
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Users WHERE UserId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        
        _users.Remove(id);
    }
}
