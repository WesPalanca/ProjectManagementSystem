namespace ProjectManagementSystem.Services;

public interface ITaskService
{
    Task CreateTask(string title, string description, DateTime deadline, TaskStatus status, int assignedTo, int assignedBy);
    Task? GetTaskById(int id);
    List<Task> GetAllTasks();
    void DeleteTask(int id);
}