using MySql.Data.MySqlClient;

using ProjectManagementSystem.Factory; // may change later because of in memory dictionary
using ProjectManagementSystem.Enums;
namespace ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;

public class ProjectTaskRepository : ITaskRepository
{
    // todo: in memory dictionary
    // private Dictionary<int, Task> tasks;
    private readonly MySqlConnection _connection;
    private readonly ITaskFactory _taskFactory;

    public ProjectTaskRepository(MySqlConnection connection, ITaskFactory taskFactory)
    {
        _connection = connection;
        _taskFactory = taskFactory;
    }

    public void Add(ProjectTask projectTask)
    {
        string query = @"
            INSERT INTO Tasks (Title, Description, AssignedBy, AssignedTo, Status, Deadline, TaskType)
            VALUES (@title, @description, @assignedBy, @assignedTo, @status, @deadline, @taskType)";

        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@title", projectTask.Title);
        cmd.Parameters.AddWithValue("@description", projectTask.Description);
        cmd.Parameters.AddWithValue("@assignedBy", projectTask.AssignedBy);
        cmd.Parameters.AddWithValue("@assignedTo", projectTask.AssignedTo);
        cmd.Parameters.AddWithValue("@status", projectTask.Status.ToString());
        cmd.Parameters.AddWithValue("@deadline", projectTask.Deadline);
        cmd.Parameters.AddWithValue("@taskType", projectTask.TaskType);

        cmd.ExecuteNonQuery();
        projectTask.TaskId = (int)cmd.LastInsertedId;
    }

    public ProjectTask? GetById(int id)
    {
        string query = @"SELECT * FROM Tasks WHERE TaskId = @id LIMIT 1";

        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        // Use TaskFactory to create the correct type
        ProjectTask? task = _taskFactory.CreateTask(
            reader.GetString("Title"),
            reader.GetString("Description"),
            reader.GetInt32("AssignedBy"),
            reader.GetInt32("AssignedTo"),
            reader.GetDateTime("Deadline"),
            reader.GetString("TaskType")
        );

        if (task != null)
        {
            task.TaskId = reader.GetInt32("TaskId");
            task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
        }

        return task;
    }

    public List<ProjectTask> GetAll()
    {
        string query = "SELECT * FROM Tasks";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();

        List<ProjectTask> tasks = new List<ProjectTask>();
        while (reader.Read())
        {
            ProjectTask? task = _taskFactory.CreateTask(
                reader.GetString("Title"),
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                reader.GetDateTime("Deadline"),
                reader.GetString("TaskType")
            );

            if (task != null)
            {
                task.TaskId = reader.GetInt32("TaskId");
                task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
            }

            if (task != null)
                tasks.Add(task);
        }

        return tasks;
    }

    public List<ProjectTask> GetByAssignedTo(int userId)
    {
        string query = "SELECT * FROM Tasks WHERE AssignedTo = @userId";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@userId", userId);

        using MySqlDataReader reader = cmd.ExecuteReader();
        List<ProjectTask> tasks = new List<ProjectTask>();

        while (reader.Read())
        {
            ProjectTask? task = _taskFactory.CreateTask(
                reader.GetString("Title"),
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                reader.GetDateTime("Deadline"),
                reader.GetString("TaskType")
            );

            if (task != null)
            {
                task.TaskId = reader.GetInt32("TaskId");
                task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
                tasks.Add(task);
            }
        }

        return tasks;
    }

    public List<ProjectTask> GetByAssignedBy(int userId)
    {
        string query = "SELECT * FROM Tasks WHERE AssignedBy = @userId";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@userId", userId);

        using MySqlDataReader reader = cmd.ExecuteReader();
        List<ProjectTask> tasks = new List<ProjectTask>();

        while (reader.Read())
        {
            ProjectTask? task = _taskFactory.CreateTask(
                reader.GetString("Title"),
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                reader.GetDateTime("Deadline"),
                reader.GetString("TaskType")
            );

            if (task != null)
            {
                task.TaskId = reader.GetInt32("TaskId");
                task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
                tasks.Add(task);
            }
        }

        return tasks;
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Tasks WHERE TaskId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
