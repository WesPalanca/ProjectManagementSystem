using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class AcceptTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        task.Status = ProjectTaskStatus.InProgress;
        Console.WriteLine("Accepted Task");
    }
}