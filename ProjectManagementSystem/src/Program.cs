using MySql.Data.MySqlClient;
using ProjectManagementSystem.Services;
using DotNetEnv;
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
        TaskStatusProcessor taskStatusProcessor = new TaskStatusProcessor();
        TaskDisplayer taskDisplayer = new TaskDisplayer(userService);
        System sys = new System(userService, projectTaskService, taskStatusProcessor, taskDisplayer);
        sys.Run();
       
        
    }
}