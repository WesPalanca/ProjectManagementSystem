namespace ProjectManagementSystem.Strategies;
using ProjectManagementSystem.Models;
public interface ITaskStatusStrategy
{
    void Handle(ProjectTask task);
}