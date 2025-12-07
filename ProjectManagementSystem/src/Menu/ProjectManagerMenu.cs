using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem.Menu;

public class ProjectManagerMenu : IMenu
{
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly TaskDisplayer _taskDisplayer;
    private readonly TaskStatusProcessor _taskStatusProcessor;

    public ProjectManagerMenu(IUserService userService, IProjectTaskService projectTaskService,
        TaskDisplayer taskDisplayer, TaskStatusProcessor taskStatusProcessor)
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
            
            Console.WriteLine("-------Project Manager Menu-------");
            Console.WriteLine("1. View Current Tasks");
            Console.WriteLine("2. View Completed Tasks");
            Console.WriteLine("3. View All Users");
            Console.WriteLine("4. View Created Tasks");
            Console.WriteLine("5. Approve/Review Task");
            Console.WriteLine("6. Create Task");
            Console.WriteLine("7. Assign Task");
            Console.WriteLine("8. Remove Task");
            Console.WriteLine("9. Your Reported Tasks");
            Console.WriteLine("10. Exit");
            Console.Write("option: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewCurrentTasks();
                    break;
                case "2":
                    ViewCompleteTasks();
                    break;
                case "3":
                    ViewAllUsers();
                    break;
                case "4":
                    ViewCreatedTasks(user);
                    break;
                case "5":
                    ReviewCompletedTask(user);
                    break;
                case "6":
                    CreateTask(user);
                    break;
                case "7":
                    AssignTask(user);
                    break;
                case "8":
                    RemoveTask(user);
                    break;
                case "9":
                    ViewUserReportedTasks(user);
                    break;
                case "10":
                    return;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
            
        }
    }
    
     private void ViewCurrentTasks()
    {
        Console.WriteLine("-------Tasks To Do-------");
        
        foreach (ProjectTask task in _projectTaskService.GetIncomplete())
        {
            _taskDisplayer.Display(task);
        }
    }

    private void ViewUserReportedTasks(User user)
    {
        Console.WriteLine("------------Your reported tasks------------");
        foreach (ProjectTask task in _projectTaskService.GetAllTasks())
        {
            if (task.Status == ProjectTaskStatus.Reported && task.AssignedBy == user.UserId)
            {
                _taskDisplayer.Display(task);
            }
        }
    }

    private void ViewCompleteTasks()
    {
        Console.WriteLine("------Complete Tasks Ready For Review------");
        List<ProjectTask> completeTasks = _projectTaskService.GetCompleted();
        foreach (ProjectTask task in completeTasks)
        {
            _taskDisplayer.Display(task);
        }
    }

    private void ReviewCompletedTask(User user)
    {
        Console.Write("Task Id to review: ");
        int taskId = int.Parse(Console.ReadLine());
        ProjectTask task = _projectTaskService.GetTaskById(taskId);

        Console.WriteLine("1. Approve Task");
        Console.WriteLine("2. Request Revision");
        Console.Write("option: ");
        string? option = Console.ReadLine();

        if (option == "1")
        {
            if (!user.HasPermission(Permissions.ApproveTask))
            {
                Console.WriteLine("You do not have permission to approve tasks.");
                return;
            }
            _taskStatusProcessor.SetStatusStrategy(new ApproveTaskStrategy());
            
        }
        else if (option == "2")
        {
            if (!user.HasPermission(Permissions.ReportTask))
            {
                Console.WriteLine("You do not have permission report.");
            }
            _taskStatusProcessor.SetStatusStrategy(new AssignTaskStrategy());
            
        }
        _taskStatusProcessor.ProcessTaskStatus(task);

        _projectTaskService.UpdateTask(task);
    }
    

    private void ViewAllUsers()
    {
        Console.WriteLine("-------All Users-------");
        List<User> allUsers = _userService.GetAllUsers();
        foreach (User user in allUsers)
        {
            Console.WriteLine($"(#{user.UserId}) {user.FirstName} {user.LastName} ({user.Role})");
        }
    }
    
    private void ViewCreatedTasks(User user)
    {
        Console.WriteLine("-------All Created Tasks-------");
        List<ProjectTask> allTasks = _projectTaskService.GetTasksByAssignedBy(user.UserId);
        foreach (ProjectTask task in allTasks)
        {
            _taskDisplayer.Display(task);
        }
    }


    private void CreateTask(User user)
    {
        if (!user.HasPermission(Permissions.CreateTask))
        {
            Console.WriteLine("You do not have permission to create tasks.");
            return;
        }
        Console.Write("Title: ");
        string? title = Console.ReadLine();
        Console.Write("Description: ");
        string? description = Console.ReadLine();
        Console.Write("Assign To (User Id): ");
        int assignTo = int.Parse(Console.ReadLine());
        
        Console.Write("Deadline (YYYY-MM-DD): ");
        Deadline deadline = new Deadline(DateTime.Parse(Console.ReadLine()));
        Console.WriteLine("Type: ");
        Console.WriteLine("1. Standard");
        Console.WriteLine("2. Urgent");
        Console.Write("option: ");
        string? typeOption = Console.ReadLine();
        string type = null;
        if (typeOption == "1")
        {
            type = "Standard";
        }
        else if (typeOption == "2")
        {
            type = "Urgent";
        }
        
        ProjectTask task = _projectTaskService.CreateTask(title,  description, user.UserId, assignTo, deadline, type);
        
        _taskStatusProcessor.SetStatusStrategy(new AssignTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }

    
    private void AssignTask(User user)
    {
        if (!user.HasPermission(Permissions.AssignTask))
        {
            Console.WriteLine("You do not have permission to assign this task.");
            return;
        }
        Console.Write("Task (Task Id): ");
        int taskId = int.Parse(Console.ReadLine());
        Console.Write("Team Member (User Id): ");
        int teamMemberId = int.Parse(Console.ReadLine());
        
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        User teamMember = _userService.GetUserById(teamMemberId);
        task.AssignedBy = user.UserId;
        task.AssignedTo = teamMemberId;
        _projectTaskService.UpdateTask(task);
        Console.WriteLine($"Task {task.TaskId} assigned to {teamMember.FirstName}");
        
        
      
        
    }

    private void RemoveTask(User user)
    {
        if (!user.HasPermission(Permissions.DeleteTask))
        {
            Console.WriteLine("You do not have permission to delete this task.");
            return;
        }
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        _projectTaskService.DeleteTask(taskId);
    }
    
}