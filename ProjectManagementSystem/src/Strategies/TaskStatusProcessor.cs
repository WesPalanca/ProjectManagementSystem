using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public class TaskStatusProcessor : ITaskStatusProcessor
{
    private ITaskStatusStrategy _strategy;



    public void SetStatusStrategy(ITaskStatusStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ProcessTaskStatus(ProjectTask task)
    {
        _strategy.Handle(task);
    }
    
}