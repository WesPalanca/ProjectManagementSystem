using MySql.Data.MySqlClient;

namespace ProjectManagementSystem.Services;

public class TaskService
{
    private readonly MySqlConnection _connection;

    public TaskService()
    {
        _connection = Database.GetInstance().Connection;
    }
}