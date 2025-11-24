using MySql.Data.MySqlClient;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;
namespace ProjectManagementSystem.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IProjectTaskFactory _projectTaskFactory;
    private readonly IProjectTaskRepository _projectTaskRepository;

    public ProjectTaskService(IProjectTaskFactory projectTaskFactory, IProjectTaskRepository projectTaskRepository)
    {
        _projectTaskFactory = projectTaskFactory;
        _projectTaskRepository = projectTaskRepository;
    }
    public ProjectTask CreateTask(string title, string description, int assignedBy, int assignedTo, DateTime deadline, string taskType)
    {
        // Only create the ProjectTask object via the factory
        ProjectTask? createdProjectTask =
            _projectTaskFactory.CreateTask(title, description, assignedBy, assignedTo, deadline, taskType);

        _projectTaskRepository.Add(createdProjectTask);

        return createdProjectTask;
    }
    

    public ProjectTask? GetTaskById(int id)
    {
        return _projectTaskRepository.GetById(id);
    }

    public List<ProjectTask> GetAllTasks()
    {
        return _projectTaskRepository.GetAll();
    }

    public List<ProjectTask> GetTasksByAssignedTo(int userId)
    {
        return _projectTaskRepository.GetByAssignedTo(userId);
    }
    public List<ProjectTask> GetTasksByAssignedBy(int userId)
    {
        return _projectTaskRepository.GetByAssignedBy(userId);
    }
    
    public void DeleteTask(int id)
    {
        _projectTaskRepository.Delete(id);
    }
}