using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Strategies;

public class StandardDisplayStrategy : ITaskDisplayStrategy
{
    public void Display(ProjectTask task, IUserService userService)
    {
        User assignedBy = userService.GetUserById(task.AssignedBy);
        User assignedTo = userService.GetUserById(task.AssignedTo);
        string assignedByName = assignedBy.FirstName + " " + assignedBy.LastName;
        string assignedToName = assignedTo.FirstName + " " + assignedTo.LastName;
        Console.WriteLine($"[STANDARD] {task.Title} (ID: {task.TaskId})");
        Console.WriteLine($"Assigned By: (#{task.AssignedBy}) {assignedByName}, Assigned To: (#{task.AssignedTo}) {assignedToName}");
        Console.WriteLine($"Deadline: {task.Deadline:yyyy-M-d dddd}, Status: {task.Status}");
        Console.WriteLine("------------------------------------------------------");
    }
}