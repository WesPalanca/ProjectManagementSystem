using ProjectManagementSystem.Auth;
using ProjectManagementSystem.Enums;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Menu;
using ProjectManagementSystem.Models;

using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem;

public class System
{
    IAuthenticationController _authenticationController;
    IMenuFactory _menuFactory;
    
    public System(IAuthenticationController authenticationController, IMenuFactory menuFactory)
    {
        _authenticationController = authenticationController;
        _menuFactory = menuFactory;
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Project Management System!");
        Console.WriteLine("1. Log In");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        string option = Console.ReadLine();

        User user = null;

        switch (option)
        {
            case "1":
                user = _authenticationController.LogIn();
                break;
            case "2":
                user = _authenticationController.Register();
                break;
            case "3":
                Console.WriteLine("See you next time!");
                break;
            default:
                Console.WriteLine("Invalid option");
                break;
                
        }
        IMenu menu = _menuFactory.CreateMenu(user);
        menu.ShowMenu(user);
        
        

      
       
    }
    
}
   