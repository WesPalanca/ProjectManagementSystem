using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem.Menu;

public class ProjectManagerMenu : IMenu
{
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly ITaskDisplayer _taskDisplayer;
    private readonly ITaskStatusProcessor _taskStatusProcessor;

    public ProjectManagerMenu(IUserService userService, IProjectTaskService projectTaskService,
        ITaskDisplayer taskDisplayer, ITaskStatusProcessor taskStatusProcessor)
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
            Console.WriteLine("4. View Tasks you Assigned");
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
                    ViewTasksYouAssigned(user);
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
                    Console.WriteLine($"See you next time, {user.FirstName}");
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
        
        string input;
        int taskId;
        do
        {
            Console.Write("Task Id to review: ");
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Task Id cannot be empty.");
            }
            if (!int.TryParse(input, out taskId))
            {
                Console.WriteLine("Please enter a valid numeric value.");
            } 
            
        } while (!int.TryParse(input, out taskId) || string.IsNullOrWhiteSpace(input));
        
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        
        string option;
        do
        {
            Console.WriteLine("\n1. Approve Task");
            Console.WriteLine("2. Request Revision");
            Console.Write("option: ");
            option = Console.ReadLine();
            if (option != "1" && option != "2")
            {
                Console.WriteLine("Invalid option. Please enter 1 or 2.");
            }
        } while (option != "1" && option != "2");

        switch (option)
        {
            case "1":
                if (!user.HasPermission(Permissions.ApproveTask))
                {
                    Console.WriteLine("You do not have permission to approve tasks.");
                    return;
                }
                _taskStatusProcessor.SetStatusStrategy(new ApproveTaskStrategy());
                break;
            case "2":
                if (!user.HasPermission(Permissions.ReportTask))
                {
                    Console.WriteLine("You do not have permission to report tasks.");
                    return;
                }
                _taskStatusProcessor.SetStatusStrategy(new RequestRevisionStrategy());
                break;
            default:
                Console.WriteLine("Invalid option, try again.");
                break;
                
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
    
    private void ViewTasksYouAssigned(User user)
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
        
        string title;
        do
        {
            Console.Write("Title: ");
            title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
            }
        } while (string.IsNullOrWhiteSpace(title));
        
        string description;
        do
        {
            Console.Write("Description: ");
            description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
            }
        } while (string.IsNullOrWhiteSpace(description));
        
        
        int assignTo;
        string input;
        do
        {
            Console.Write("Assign To (User Id): ");
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("User Id cannot be empty.");
            }
            if (!int.TryParse(input, out assignTo))
            {
                Console.WriteLine("Please enter a valid numeric value.");
            } 
            
        } while (!int.TryParse(input, out assignTo) || string.IsNullOrWhiteSpace(input));
        
        DateTime dueDate;

        do
        {
            Console.Write("Deadline (YYYY-MM-DD): ");
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Deadline cannot be empty.");
                continue;
            }

            if (!DateTime.TryParse(input, out dueDate))
            {
                Console.WriteLine("Please enter a valid date.");
            }

        } while (!DateTime.TryParse(input, out dueDate));
        
        Deadline deadline = new Deadline(dueDate);
        
        
        
        
        string option;
        string type = null;
        do
        {
            Console.WriteLine("\n1. Standard");
            Console.WriteLine("2. Urgent");
            Console.Write("option: ");
            option = Console.ReadLine();
            if (option != "1" && option != "2")
            {
                Console.WriteLine("Invalid option. Please enter 1 or 2.");
            }
        } while (option != "1" && option != "2");

        switch (option)
        {
            case "1":
                type = "Standard";
                break;
            case "2":
                type = "Urgent";
                break;
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
        string input;
        int taskId;
        do
        {
            Console.Write("Task Id: ");
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Task Id cannot be empty.");
            }
            if (!int.TryParse(input, out taskId))
            {
                Console.WriteLine("Please enter a valid numeric value.");
            } 
            
        } while (!int.TryParse(input, out taskId) || string.IsNullOrWhiteSpace(input));
        
        
        int userId;
        do
        {
            Console.Write("User Id: ");
            input = Console.ReadLine();
            if (!int.TryParse(input, out userId))
            {
                Console.WriteLine("Please enter a valid numeric value.");
            }
            
        } while (!int.TryParse(input, out userId));
        
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        User teamMember = _userService.GetUserById(userId);
        task.AssignedBy = user.UserId;
        task.AssignedTo = userId;
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
        string input;
        int taskId;
        do
        {
            Console.Write("Task Id: ");
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Task Id cannot be empty.");
            }
            if (!int.TryParse(input, out taskId))
            {
                Console.WriteLine("Please enter a valid numeric value.");
            } 
            
        } while (!int.TryParse(input, out taskId) || string.IsNullOrWhiteSpace(input));
        _projectTaskService.DeleteTask(taskId);
    }
    
}