using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem;

public class TaskDisplayer
{
    private readonly IUserService _userService;

    public TaskDisplayer(IUserService userService)
    {
        _userService = userService;
    }
    public void Display(ProjectTask task)
    {
        ITaskDisplayStrategy? strategy = null;

        switch (task.TaskType)
        {
            case "Standard":
                strategy = new StandardDisplayStrategy();
                break;
            case "Urgent":
                strategy = new UrgentDisplayStrategy();
                break;
        }
        strategy.Display(task, _userService);
    }
}