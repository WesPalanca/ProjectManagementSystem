using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class CompleteTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        Console.WriteLine("Completed Task");
    }
    
}