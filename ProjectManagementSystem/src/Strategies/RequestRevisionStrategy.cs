using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class RequestRevisionStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        task.Status = ProjectTaskStatus.Reported;
        Console.WriteLine("Task has been reported");
    }
}