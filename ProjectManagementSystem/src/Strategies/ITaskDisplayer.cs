using ProjectManagementSystem.Models;

namespace ProjectManagementSystem;

public interface ITaskDisplayer
{
    void Display(ProjectTask task);
}