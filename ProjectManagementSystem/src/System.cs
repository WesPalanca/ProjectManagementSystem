using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Models;

using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem;

public class System
{
    private User? _activeUser;
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly TaskDisplayer _taskDisplayer;
    private readonly TaskStatusProcessor _taskStatusProcessor;
    public System(IUserService userService, IProjectTaskService projectTaskService, TaskStatusProcessor taskStatusProcessor, TaskDisplayer taskDisplayer)
    {
        _userService = userService;
        _projectTaskService = projectTaskService;
        _taskDisplayer = taskDisplayer;
        _taskStatusProcessor = taskStatusProcessor;
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Project Management System!");
        DisplayAuthentication();
    }

    
    // todo: cleanup private methods to classes
    private void DisplayAuthentication()
    { 
        Console.WriteLine("Select an option");
        Console.WriteLine("1. Log In");
        Console.WriteLine("2. Register");
        Console.Write("option: ");
        string? option = Console.ReadLine();
        if (option == "1")
        {
            LogIn();
        }
        else if (option == "2")
        {
            Register();
        }
    }

    private void LogIn()
    {
        Console.Write("\nEmail: ");
        string? email = Console.ReadLine();
        User? user = _userService.GetUserByEmail(email);
        if (user == null)
        {
            Console.WriteLine("User does not exist. Please Register");
            return;
        }

        // TODO: Add authentication logic
        Console.Write("Password: ");
        string? password = Console.ReadLine();

        
        
        if (password != user.Password)
        {
            Console.WriteLine("Incorrect credentials");
            return;
        }
        _activeUser = user;
        
        Console.WriteLine($"Glad your back {user.FirstName}!");

        switch (_activeUser.Role)
        {
            case "ProjectManager":
                DisplayProjectManagerMenu();
                break;
            case "TeamMember":
                DisplayTeamMemberMenu();
                break;
        }

        
    }

    private void Register()
    {
        string? firstName;
        string? lastName;
        string? email;
        string? password;
        string? role = null;
        Console.WriteLine("Please enter your information");
        Console.Write("First Name: ");
        firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        lastName = Console.ReadLine();
        Console.Write("Email: ");
        email = Console.ReadLine();
        if (_userService.GetUserByEmail(email) != null)
        {
            Console.WriteLine("User already exists");
            return;
        }
        Console.Write("Password: ");
        password = Console.ReadLine();
        Console.WriteLine("What is your role? ");
        Console.WriteLine("1. Project Manager");
        Console.WriteLine("2. Team Member");
        Console.Write("option: ");
        string roleOption = Console.ReadLine();
        if (roleOption == "1")
        {
            role = "ProjectManager";
                
        }
        else if (roleOption == "2")
        {
            role = "TeamMember";
        }
        User createdUser = _userService.CreateUser(firstName, lastName, email, password, role);
        Console.WriteLine($"{createdUser.FirstName} your registered!");
    }

    private void DisplayProjectManagerMenu()
    {
        while (_activeUser != null)
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
                    ViewCreatedTasks();
                    break;
                case "5":
                    ReviewCompletedTask();
                    break;
                case "6":
                    CreateTask();
                    break;
                case "7":
                    AssignTask();
                    break;
                case "8":
                    RemoveTask();
                    break;
                case "9":
                    ViewUserReportedTasks();
                    break;
                case "10":
                    _activeUser = null;
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
            
        }
    }

    private void ViewCurrentTasks()
    {
        Console.WriteLine("-------Tasks To Do-------");
        List<ProjectTask> incompleteTasks = _projectTaskService.GetIncomplete();
        foreach (ProjectTask task in incompleteTasks)
        {
            _taskDisplayer.Display(task);
        }
    }

    private void ViewUserReportedTasks()
    {
        Console.WriteLine("------------Your reported tasks------------");
        foreach (ProjectTask task in _projectTaskService.GetAllTasks())
        {
            if (task.Status == ProjectTaskStatus.Reported && task.AssignedBy == _activeUser.UserId)
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

    private void ReviewCompletedTask()
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
            _taskStatusProcessor.SetStatusStrategy(new ApproveTaskStrategy());
            
        }
        else if (option == "2")
        {
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
    
    private void ViewCreatedTasks()
    {
        Console.WriteLine("-------All Created Tasks-------");
        List<ProjectTask> allTasks = _projectTaskService.GetTasksByAssignedBy(_activeUser.UserId);
        foreach (ProjectTask task in allTasks)
        {
            _taskDisplayer.Display(task);
        }
    }


    private void CreateTask()
    {
        // todo: users should be able to create a task without a team member assigned
        Console.Write("Title: ");
        string? title = Console.ReadLine();
        Console.Write("Description: ");
        string? description = Console.ReadLine();
        Console.Write("Assign To (User Id): ");
        int assignTo = int.Parse(Console.ReadLine());
        
        Console.Write("Deadline (YYYY-MM-DD): ");
        DateTime deadline = DateTime.Parse(Console.ReadLine());
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
        
        ProjectTask task = _projectTaskService.CreateTask(title,  description, _activeUser.UserId, assignTo, deadline, type);
        
        _taskStatusProcessor.SetStatusStrategy(new AssignTaskStrategy());
        _taskStatusProcessor.ProcessTaskStatus(task);
        _projectTaskService.UpdateTask(task);
    }

    
    private void AssignTask()
    {
        Console.Write("Task (Task Id): ");
        int taskId = int.Parse(Console.ReadLine());
        Console.Write("Team Member (User Id): ");
        int teamMemberId = int.Parse(Console.ReadLine());
        
        ProjectTask task = _projectTaskService.GetTaskById(taskId);
        User teamMember = _userService.GetUserById(teamMemberId);
        task.AssignedBy = _activeUser.UserId;
        task.AssignedTo = teamMemberId;
        _projectTaskService.UpdateTask(task);
        Console.WriteLine($"Task {task.TaskId} assigned to {teamMember.FirstName}");
        
        
      
        
    }

    private void RemoveTask()
    {
        Console.Write("Task Id: ");
        int taskId = int.Parse(Console.ReadLine());
        _projectTaskService.DeleteTask(taskId);
    }

    
    // todo: create and add necessary team member action logic
    private void DisplayTeamMemberMenu()
    {
        while (_activeUser != null)
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
                    ViewAssignedTasks();
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
                    _activeUser = null;
                    break;
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }

    private void ViewAssignedTasks()
    {
        List<ProjectTask> assignedTasks = _projectTaskService.GetTasksByAssignedTo(_activeUser.UserId);
        
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