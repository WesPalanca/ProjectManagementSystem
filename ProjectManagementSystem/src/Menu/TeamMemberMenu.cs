using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem.Menu;

public class TeamMemberMenu : IMenu
{
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly TaskDisplayer _taskDisplayer;
    private readonly TaskStatusProcessor _taskStatusProcessor;

    public TeamMemberMenu(IUserService userService, IProjectTaskService projectTaskService, TaskDisplayer taskDisplayer,
        TaskStatusProcessor taskStatusProcessor)
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
                    AcceptTask();
                    break;
                case "3":
                    CompleteTask();
                    break;
                case "4":
                    ReportTask();
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

    private void AcceptTask()
    {
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new AcceptTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);

    }

    private void CompleteTask()
    {
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new CompleteTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }

    private void ReportTask()
    {
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        _taskStatusProcessor.SetStatusStrategy(new RequestRevisionStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }
    
}
