namespace ProjectManagementSystem.Services;
using ProjectManagementSystem.Models;
public interface IUserService
{
    User CreateUser(string firstname, string lastname, string email, string password, string role);
    User? GetUserById(int id);
    User? GetUserByEmail(string email);
    List<User> GetAllUsers();
    void DeleteUser(int id);
    
}