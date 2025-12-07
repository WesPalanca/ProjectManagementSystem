using ProjectManagementSystem.Auth;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Menu;
using ProjectManagementSystem.Models;



namespace ProjectManagementSystem;

public class System
{
    private readonly IAuthenticationController _authenticationController;
    private readonly IMenuFactory _menuFactory;
    
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
                
        }
        IMenu menu = _menuFactory.CreateMenu(user);
        menu.ShowMenu(user);
        
        

      
       
    }
    
}
   