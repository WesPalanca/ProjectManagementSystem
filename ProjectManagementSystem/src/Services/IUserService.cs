namespace ProjectManagementSystem.Services;
using ProjectManagementSystem.Models;
public interface IUserService
{
    User CreateUser(string firstname, string lastname, string email, string role);
    User? GetUserById(int id);
    List<User> GetAllUsers();
    void DeleteUser(int id);
    
}