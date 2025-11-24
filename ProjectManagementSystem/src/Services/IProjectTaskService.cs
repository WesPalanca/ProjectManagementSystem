namespace ProjectManagementSystem.Services;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Enums;
public interface IProjectTaskService
{
    ProjectTask CreateTask(string title, string description, int assignedBy, int assignedTo, DateTime deadline, string taskType);
    ProjectTask? GetTaskById(int id);
    List<ProjectTask> GetAllTasks();
    List<ProjectTask> GetTasksByAssignedTo(int userId);
    List<ProjectTask> GetTasksByAssignedBy(int userId);
    void DeleteTask(int id);
}