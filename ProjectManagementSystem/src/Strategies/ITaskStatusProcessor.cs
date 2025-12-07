using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public interface ITaskStatusProcessor
{
    void SetStatusStrategy(ITaskStatusStrategy strategy);
    void ProcessTaskStatus(ProjectTask task);
}