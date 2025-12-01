namespace ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;
public interface IUserRepository
{
    void Add(User user, string password);
    User? GetById(int id);
    User? GetByEmail(string email);
    List<User> GetAll();
    void Delete(int id);
}