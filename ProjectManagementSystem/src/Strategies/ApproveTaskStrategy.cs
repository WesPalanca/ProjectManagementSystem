using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class ApproveTaskStrategy : ITaskStatusStrategy
{
    public void Handle(ProjectTask task)
    {
        task.Status = ProjectTaskStatus.Approved;
        Console.WriteLine("Task has been approved");
    }
}