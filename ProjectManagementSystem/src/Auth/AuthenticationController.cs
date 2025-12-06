using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Auth;

public class AuthenticationController : IAuthenticationController
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    public User LogIn()
    {
        while (true)
        {
            Console.Write("\nEmail: ");
            string email = Console.ReadLine();
            User user = _userService.GetUserByEmail(email);
            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (user != null && password == user.Password) return user;
            Console.WriteLine("Wrong credentials! Please try again.\n");
        }
    }

    public User Register()
    {
        Console.WriteLine("Please Enter Your Information Below: ");
        string? firstName;
        do
        {
            Console.Write("First Name: ");
            firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName)) 
            {
                Console.WriteLine("First Name is required.");
            }
          
        } while (string.IsNullOrWhiteSpace(firstName));
        string? lastName;
        
        do
        {
            Console.Write("Last Name: ");
            lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last Name is required.");
            } 
        } while (string.IsNullOrWhiteSpace(lastName));
        
        string? email;
        while (true)
        {
            Console.Write("Email: ");
            email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email is required.");
                continue;
            }

            if (_userService.GetUserByEmail(email) != null)
            {
                Console.WriteLine("User already exists.");
                continue;
            }
            break;
        }
        
        string? password;
        do
        {
            Console.Write("Password: ");
            password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password is required.");
            }
            
        } while (string.IsNullOrWhiteSpace(password));
        
        string? role = null;

        while (role == null)
        {
            Console.WriteLine("What is your role? ");
            Console.WriteLine("1. Project Manager");
            Console.WriteLine("2. Team Member");
            Console.Write("option: ");
            string roleOption = Console.ReadLine();

            switch (roleOption)
            {
                case "1":
                    role = "ProjectManager";
                    break;
                case "2":
                    role = "TeamMember";
                    break;
                default:
                    Console.WriteLine("Invalid role option. Please try again.\n");
                    break;
            }
        }
        
        User createdUser = _userService.CreateUser(firstName, lastName, email, password, role);
        Console.WriteLine($"{createdUser.FirstName} your registered!");
        return createdUser;

    }
}