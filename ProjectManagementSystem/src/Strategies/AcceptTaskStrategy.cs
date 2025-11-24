using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class AcceptTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        Console.WriteLine("Accepted Task");
    }
}