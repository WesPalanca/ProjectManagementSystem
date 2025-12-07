using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Strategies;

public class UrgentDisplayStrategy : ITaskDisplayStrategy
{
    public void Display(ProjectTask task, IUserService userService)
    {
        User assignedBy = userService.GetUserById(task.AssignedBy);
        User assignedTo = userService.GetUserById(task.AssignedTo);
        string assignedByName = assignedBy.FirstName + " " + assignedBy.LastName;
        string assignedToName = assignedTo.FirstName + " " + assignedTo.LastName;
        string taskInfo = task.Info();
        taskInfo += $"{assignedByName} ---> {assignedToName}";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(taskInfo);
        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------");
    }
}