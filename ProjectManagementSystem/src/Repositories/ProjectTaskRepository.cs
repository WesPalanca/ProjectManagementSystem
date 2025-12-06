using MySql.Data.MySqlClient;

using ProjectManagementSystem.Factory; // may change later because of in memory dictionary
using ProjectManagementSystem.Enums;
namespace ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly MySqlConnection _connection;
    private readonly IProjectTaskFactory _projectTaskFactory;

    public ProjectTaskRepository(MySqlConnection connection, IProjectTaskFactory projectTaskFactory)
    {
        _connection = connection;
        _projectTaskFactory = projectTaskFactory;
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
        cmd.Parameters.AddWithValue("@deadline", projectTask.Deadline.DueDate);
        cmd.Parameters.AddWithValue("@taskType", projectTask.TaskType);

        cmd.ExecuteNonQuery();
        projectTask.TaskId = (int)cmd.LastInsertedId;
    }

    public ProjectTask? GetById(int id)
    {
        string query = "SELECT * FROM Tasks WHERE TaskId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        using MySqlDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        Deadline deadline = new Deadline(reader.GetDateTime("Deadline"));
        ProjectTask task = _projectTaskFactory.CreateTask(reader.GetString("Title"), 
            reader.GetString("Description"),
            reader.GetInt32("AssignedBy"),
            reader.GetInt32("AssignedTo"),
            deadline,
            reader.GetString("TaskType"));
        task.TaskId = reader.GetInt32("TaskId");
        task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
        return task;
        
       
    }

    public List<ProjectTask> GetAll()
    {
        List<ProjectTask> tasks = new List<ProjectTask>();
        string query = "SELECT * FROM Tasks";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            Deadline deadline = new Deadline(reader.GetDateTime("Deadline"));
            ProjectTask task = _projectTaskFactory.CreateTask(reader.GetString("Title"), 
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                deadline,
                reader.GetString("TaskType"));
            task.TaskId = reader.GetInt32("TaskId");
            task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
            tasks.Add(task);
        }

        return tasks;
    }

    public List<ProjectTask> GetByAssignedTo(int userId)
    {
        List<ProjectTask> tasks = new List<ProjectTask>();
        string query = "SELECT * FROM Tasks WHERE AssignedTo = @userId";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@userId", userId);
        using MySqlDataReader reader = cmd.ExecuteReader();
  
        while (reader.Read())
        {
            Deadline deadline = new Deadline(reader.GetDateTime("Deadline"));
            ProjectTask task = _projectTaskFactory.CreateTask(reader.GetString("Title"), 
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                deadline,
                reader.GetString("TaskType"));
            task.TaskId = reader.GetInt32("TaskId");
            task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
            tasks.Add(task);
        
        }
        
        return tasks;
       
    }

    public List<ProjectTask> GetByAssignedBy(int userId)
    {
        List<ProjectTask> tasks = new List<ProjectTask>();
        string query = "SELECT * FROM Tasks WHERE AssignedBy = @userId";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@userId", userId);
        using MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Deadline deadline = new Deadline(reader.GetDateTime("Deadline"));
            ProjectTask task = _projectTaskFactory.CreateTask(reader.GetString("Title"), 
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                deadline,
                reader.GetString("TaskType"));
            task.TaskId = reader.GetInt32("TaskId");
            task.Status = Enum.Parse<ProjectTaskStatus>(reader.GetString("Status"));
            tasks.Add(task);
        
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

    public void Update(ProjectTask task)
    {
        string query = "UPDATE Tasks SET Title = @title, Description = @description, AssignedBy = @assignedBy, AssignedTo = @assignedTo, Deadline = @deadline, Status = @status, TaskType = @taskType WHERE TaskId = @TaskId";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@title", task.Title);
        cmd.Parameters.AddWithValue("@description", task.Description);
        cmd.Parameters.AddWithValue("@assignedBy", task.AssignedBy);
        cmd.Parameters.AddWithValue("@assignedTo", task.AssignedTo);
        cmd.Parameters.AddWithValue("@status", task.Status.ToString());
        cmd.Parameters.AddWithValue("@deadline", task.Deadline.DueDate);
        cmd.Parameters.AddWithValue("@taskType", task.TaskType);
        cmd.Parameters.AddWithValue("@TaskId", task.TaskId);

        cmd.ExecuteNonQuery();
    }
}
