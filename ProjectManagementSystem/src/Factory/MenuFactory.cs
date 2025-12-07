using ProjectManagementSystem.Menu;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem.Factory;

public class MenuFactory : IMenuFactory
{
    private readonly IUserService _userService;
    private readonly IProjectTaskService _projectTaskService;
    private readonly ITaskDisplayer _taskDisplayer;
    private readonly ITaskStatusProcessor _taskStatusProcessor;
    
    public MenuFactory(IUserService userService, IProjectTaskService projectTaskService, ITaskDisplayer taskDisplayer,
        ITaskStatusProcessor taskStatusProcessor)
    {
        _userService = userService;
        _projectTaskService = projectTaskService;
        _taskDisplayer = taskDisplayer;
        _taskStatusProcessor = taskStatusProcessor;
    }

    public IMenu CreateMenu(User user)
    {
        IMenu menu = null;
        switch (user.Role)
        {
            case "ProjectManager":
                menu = new ProjectManagerMenu(_userService, _projectTaskService, _taskDisplayer, _taskStatusProcessor);
                break;
            case "TeamManager":
                menu = new TeamMemberMenu(_userService, _projectTaskService, _taskDisplayer, _taskStatusProcessor);
                break;
        }

        return menu;
    }
}