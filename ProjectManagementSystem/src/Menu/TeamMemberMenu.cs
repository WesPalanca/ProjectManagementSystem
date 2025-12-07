using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem.Menu;

public class TeamMemberMenu : IMenu
{
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly ITaskDisplayer _taskDisplayer;
    private readonly ITaskStatusProcessor _taskStatusProcessor;

    public TeamMemberMenu(IUserService userService, IProjectTaskService projectTaskService, ITaskDisplayer taskDisplayer,
        ITaskStatusProcessor taskStatusProcessor)
    {
        _userService = userService;
        _projectTaskService = projectTaskService;
        _taskDisplayer = taskDisplayer;
        _taskStatusProcessor = taskStatusProcessor;
    }

    public void ShowMenu(User user)
    {
        while (true)
        {
            Console.WriteLine("-------Team Member Menu-------");
            Console.WriteLine("1. View Assigned Tasks");
            Console.WriteLine("2. Accept Task");
            Console.WriteLine("3. Mark Task Complete");
            Console.WriteLine("4. Report/Flag Task");
            Console.WriteLine("5. Exit");
            Console.Write("option: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewAssignedTasks(user);
                    break;
                case "2":
                    AcceptTask(user);
                    break;
                case "3":
                    CompleteTask(user);
                    break;
                case "4":
                    ReportTask(user);
                    break;
                case "5":
                    return;
                
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
    
    private void ViewAssignedTasks(User user)
    {
        List<ProjectTask> assignedTasks = _projectTaskService.GetTasksByAssignedTo(user.UserId);
        
        Console.WriteLine("----------------Your Tasks----------------");

        foreach (ProjectTask task in assignedTasks)
        {
            _taskDisplayer.Display(task);
        }
    }

    private void AcceptTask(User user)
    {
        if (!user.HasPermission(Permissions.AcceptTask))
        {
            Console.WriteLine("You don't have permission to accept this task.");
            return;
        }
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new AcceptTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);

    }

    private void CompleteTask(User user)
    {
        if (!user.HasPermission(Permissions.CompleteTask))
        {
            Console.WriteLine("You don't have permission to complete this task.");
            return;
        }
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new CompleteTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }

    private void ReportTask(User user)
    {
        if (!user.HasPermission(Permissions.ReportTask))
        {
            Console.WriteLine("You don't have permission to report this task.");
            return;
        }
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new RequestRevisionStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }
    
}
