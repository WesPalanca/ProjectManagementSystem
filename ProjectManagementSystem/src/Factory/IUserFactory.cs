namespace ProjectManagementSystem.Factory;
using ProjectManagementSystem.Models;
public interface IUserFactory
{
    User? CreateUser(string firstName, string lastName, string email, string password, string role);
}