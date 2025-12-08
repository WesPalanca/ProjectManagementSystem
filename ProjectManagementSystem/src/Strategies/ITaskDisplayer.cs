using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Strategies;

public interface ITaskDisplayer
{
    void Display(ProjectTask task);
}