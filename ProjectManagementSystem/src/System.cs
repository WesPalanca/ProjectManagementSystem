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
    private readonly ITaskService _taskService;
    public System()
    {
        MySqlConnection connection = Database.GetInstance().Connection;
        IUserFactory userFactory = new UserFactory();
        IUserRepository userRepository = new UserRepository(connection);
        _userService = new UserService(userFactory, userRepository);
        // todo: taskService, taskFactory, taskRepository
        
        
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Project Management System!");
        DisplayAuthentication();
    }

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

    public void LogIn()
    {
        Console.Write("\nEmail: ");
        string? email = Console.ReadLine();
        User? user = _userService.GetUserByEmail(email);
        if (user == null)
        {
            Console.WriteLine("User does not exist. Please Register");
            return;
        }

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        

        if (password != user.Password)
        {
            Console.WriteLine("Incorrect credentials");
            return;
        }
        _activeUser = user;
        

        // TODO: Add authentication logic 
        Console.WriteLine($"Welcome {user.FirstName}!");

        
    }

    public void Register()
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
    }
}