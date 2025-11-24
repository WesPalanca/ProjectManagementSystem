namespace ProjectManagementSystem.Services;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Enums;
public interface ITaskService
{
    ProjectTask CreateTask(string title, string description, DateTime deadline, int assignedTo, int assignedBy);
    ProjectTask? GetTaskById(int id);
    List<ProjectTask> GetAllTasks();
    void DeleteTask(int id);
}