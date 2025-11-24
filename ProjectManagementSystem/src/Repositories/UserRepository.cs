namespace ProjectManagementSystem.Repositories;
using MySql.Data.MySqlClient;
using ProjectManagementSystem.Models;
public class UserRepository : IUserRepository
{
    private readonly MySqlConnection _connection;
    // todo: in memory dictionary
    // private Dictionary<int, Task> tasks;

        public UserRepository(MySqlConnection connection)
        {
            _connection = connection;
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
            string query = "SELECT UserId, FirstName, LastName, Email, Password, Role FROM Users WHERE UserId = @id LIMIT 1";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@id", id);

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User(
                reader.GetString("FirstName"),
                reader.GetString("LastName"),
                reader.GetString("Email"),
                reader.GetString("Password"),
                reader.GetString("Role")
            )
            {
                UserId = reader.GetInt32("UserId")
            };
        }
        public User? GetByEmail(string email)
        {
            string query = "SELECT UserId, FirstName, LastName, Email, Password, Role FROM Users WHERE Email = @email LIMIT 1";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@email", email);
            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User(
                reader.GetString("FirstName"),
                reader.GetString("LastName"),
                reader.GetString("Email"),
                reader.GetString("Password"),
                reader.GetString("Role")
            )
            {
                UserId = reader.GetInt32("UserId")
            };
        }

        public List<User> GetAll()
        {
            string query = "SELECT UserId, FirstName, LastName, Email, Password, Role FROM Users";

            using MySqlCommand cmd = new MySqlCommand(query, _connection);
            using MySqlDataReader reader = cmd.ExecuteReader();

            List<User> users = new List<User>();
            while (reader.Read())
            {
                users.Add(new User(
                    reader.GetString("FirstName"),
                    reader.GetString("LastName"),
                    reader.GetString("Email"),
                    reader.GetString("Password"),
                    reader.GetString("Role")
                )
                {
                    UserId = reader.GetInt32("UserId")
                });
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
