using ProjectManagementSystem.Menu;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Factory;

public interface IMenuFactory
{
    IMenu CreateMenu(User user);

}