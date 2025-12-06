using MySql.Data.MySqlClient;
using ProjectManagementSystem.Enums;
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
    public ProjectTask CreateTask(string title, string description, int assignedBy, int assignedTo, Deadline deadline, string taskType)
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
    

    public List<ProjectTask> GetIncomplete()
    {
        List<ProjectTask> allTasks = _projectTaskRepository.GetAll();
        
        List<ProjectTask> incompleteTasks = new List<ProjectTask>();
        foreach (ProjectTask task in allTasks)
        {
            if (task.Status != ProjectTaskStatus.Complete)
            {
                incompleteTasks.Add(task);
            }
        }
        return incompleteTasks;
        
    }

    public List<ProjectTask> GetCompleted()
    {
        List<ProjectTask> completeTasks = new List<ProjectTask>();
        foreach (ProjectTask task in _projectTaskRepository.GetAll())
        {
            if (task.Status == ProjectTaskStatus.Complete)
            {
                completeTasks.Add(task);
            }
        }
        return completeTasks;
        
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

    public void UpdateTask(ProjectTask task)
    {
        _projectTaskRepository.Update(task);
    }

  
}