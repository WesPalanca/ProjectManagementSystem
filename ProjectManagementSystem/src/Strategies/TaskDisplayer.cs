using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Strategies;

namespace ProjectManagementSystem;

public class TaskDisplayer : ITaskDisplayer
{
    private readonly IUserService _userService;
    private ITaskDisplayStrategy _strategy = null;

    public TaskDisplayer(IUserService userService)
    {
        _userService = userService;
    }
    public void Display(ProjectTask task)
    {
      

        switch (task.TaskType)
        {
            case "Standard":
                _strategy = new StandardDisplayStrategy();
                break;
            case "Urgent":
                _strategy = new UrgentDisplayStrategy();
                break;
        }
        _strategy.Display(task, _userService);
    }
}