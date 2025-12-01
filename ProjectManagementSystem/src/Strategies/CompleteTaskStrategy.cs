using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class CompleteTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        task.Status = ProjectTaskStatus.Complete;
        Console.WriteLine("Completed Task");
    }
    
}