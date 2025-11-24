using MySql.Data.MySqlClient;

using ProjectManagementSystem.Factory; // may change later because of in memory dictionary
using ProjectManagementSystem.Enums;
namespace ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;

public class ProjectTaskRepository : IProjectTaskRepository
{
    // todo: in memory dictionary
    private Dictionary<int, ProjectTask> _tasks = new Dictionary<int, ProjectTask>();
    private readonly MySqlConnection _connection;
    private readonly IProjectTaskFactory _projectTaskFactory;

    public ProjectTaskRepository(MySqlConnection connection, IProjectTaskFactory projectTaskFactory)
    {
        _connection = connection;
        _projectTaskFactory = projectTaskFactory;
        PopulateTasks();
    }

    private void PopulateTasks()
    {
        string query = "SELECT * FROM Tasks;";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        using MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ProjectTask? task = _projectTaskFactory.CreateTask(
                reader.GetString("Title"),
                reader.GetString("Description"),
                reader.GetInt32("AssignedBy"),
                reader.GetInt32("AssignedTo"),
                reader.GetDateTime("deadline"),
                reader.GetString("TaskType"));
            task.TaskId = reader.GetInt32("TaskId");
            _tasks[task.TaskId] = task;
        }
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
        _tasks[projectTask.TaskId] = projectTask;
    }

    public ProjectTask? GetById(int id)
    {
        if (_tasks.ContainsKey(id))
        {
            return _tasks[id];
        }

        return null;
       
    }

    public List<ProjectTask> GetAll()
    {
        return _tasks.Values.ToList();
      
    }

    public List<ProjectTask> GetByAssignedTo(int userId)
    {
        List<ProjectTask> projectTasks = new List<ProjectTask>();
        foreach (ProjectTask task in _tasks.Values)
        {
            if (task.AssignedBy == userId)
            {
                projectTasks.Add(task);
            }
        }
        return projectTasks;
       
    }

    public List<ProjectTask> GetByAssignedBy(int userId)
    {
        List<ProjectTask> projectTasks = new List<ProjectTask>();
        foreach (ProjectTask task in _tasks.Values)
        {
            if (task.AssignedTo == userId)
            {
                projectTasks.Add(task);
            }
        }
        return projectTasks;

    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Tasks WHERE TaskId = @id";
        using MySqlCommand cmd = new MySqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        _tasks.Remove(id);
    }
}
