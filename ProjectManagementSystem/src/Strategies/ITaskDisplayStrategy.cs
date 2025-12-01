using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Strategies;

public interface ITaskDisplayStrategy
{
    void Display(ProjectTask task, IUserService userService);
}