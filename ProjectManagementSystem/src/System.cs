using MySql.Data.MySqlClient;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem;

public class System
{
    private User? _activeUser;
    private readonly IUserService _userService;
    // todo: taskService
    private readonly IProjectTaskService _projectTaskService;
    public System()
    {
        MySqlConnection connection = Database.GetInstance().Connection;
        IUserFactory userFactory = new UserFactory();
        IUserRepository userRepository = new UserRepository(connection);
        _userService = new UserService(userFactory, userRepository);
        // todo: taskService, taskFactory, taskRepository
        IProjectTaskFactory projectTaskFactory = new ProjectTaskFactory();
        IProjectTaskRepository projectTaskRepository = new ProjectTaskRepository(connection, projectTaskFactory);
        _projectTaskService = new ProjectTaskService(projectTaskFactory, projectTaskRepository);
        



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
            Console.WriteLine("1. View All Tasks");
            Console.WriteLine("2. View All Users");
            Console.WriteLine("3. Create Task");
            Console.WriteLine("4. Assign Task");
            Console.WriteLine("5. Remove Task");
            Console.WriteLine("6. Exit");
            Console.Write("option: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewAllTasks();
                    break;
                case "2":
                    ViewAllUsers();
                    break;
                case "3":
                    CreateTask();
                    break;
                case "4":
                    AssignTask();
                    break;
                case "5":
                    RemoveTask();
                    break;
                case "6":
                    _activeUser = null;
                    break;
                default:
                    _activeUser = null;
                    break;
            }
            
        }
    }

    private void ViewAllTasks()
    {
        Console.WriteLine("-------All Tasks-------");
        List<ProjectTask> allTasks = _projectTaskService.GetAllTasks();
        foreach (ProjectTask task in allTasks)
        {
            Console.WriteLine(task.Title);
        }
    }

    private void ViewAllUsers()
    {
        Console.WriteLine("-------All Users-------");
        List<User> allUsers = _userService.GetAllUsers();
        foreach (User user in allUsers)
        {
            Console.WriteLine(user.FirstName);
        }
    }

    private void CreateTask()
    {
        Console.Write("Title");
        string? title = Console.ReadLine();
        Console.Write("Description");
        string? description = Console.ReadLine();
        Console.Write("Assign To (User Id): ");
        int assignTo = int.Parse(Console.ReadLine());
        Console.Write("Deadline (YYYY-MM-DD): ");
        DateTime deadline = DateTime.Parse(Console.ReadLine());
        Console.Write("Type: ");
        Console.WriteLine("1. Standard");
        Console.WriteLine("1. Urgent");
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
        
        _projectTaskService.CreateTask(title,  description, _activeUser.UserId, assignTo, deadline, type);
    }

    private void AssignTask()
    {
        Console.Write("Team Member (User Id): ");
        int assignTo = int.Parse(Console.ReadLine());
        
        _projectTaskService.GetTaskById(assignTo);
        
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
        Console.WriteLine("-------Team Member Menu-------");
        Console.WriteLine("1. View Assigned Tasks");
        Console.WriteLine("2. Accept Task");
        Console.WriteLine("3. Mark Task Complete");
        Console.WriteLine("4. Report/Flag Task");
        Console.WriteLine("5. Exit");
    }
    
}