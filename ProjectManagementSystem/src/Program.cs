using MySql.Data.MySqlClient;
using ProjectManagementSystem.Services;
using DotNetEnv;
using ProjectManagementSystem.Auth;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem;


// Tests connection to MySql DB
class Program
{
    static void Main(string[] args)
    {
        MySqlConnection connection = Database.GetInstance().Connection;
        IUserFactory userFactory = new UserFactory();
        IUserRepository userRepository = new UserRepository(connection, userFactory);
        IUserService userService = new UserService(userFactory, userRepository);
        IProjectTaskFactory projectTaskFactory = new ProjectTaskFactory();
        IProjectTaskRepository projectTaskRepository = new ProjectTaskRepository(connection, projectTaskFactory);
        IProjectTaskService projectTaskService = new ProjectTaskService(projectTaskFactory, projectTaskRepository);
        ITaskStatusProcessor taskStatusProcessor = new TaskStatusProcessor();
        ITaskDisplayer taskDisplayer = new TaskDisplayer(userService);
        IAuthenticationController authenticationController = new AuthenticationController(userService);
        IMenuFactory menuFactory = new MenuFactory(userService, projectTaskService, taskDisplayer, taskStatusProcessor);
        System sys = new System(authenticationController, menuFactory);
        sys.Run();
       
        
    }
}