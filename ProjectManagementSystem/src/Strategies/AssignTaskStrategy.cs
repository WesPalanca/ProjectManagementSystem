using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class AssignTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        task.Status = ProjectTaskStatus.Assigned;
        Console.WriteLine("Task has been assigned");
    }
}