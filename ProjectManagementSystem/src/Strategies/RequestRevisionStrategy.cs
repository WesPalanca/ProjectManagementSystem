using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class RequestRevisionStrategy
{
    public void Handle(ProjectTask task)
    {
        Console.WriteLine("Task has been reported");
    }
}