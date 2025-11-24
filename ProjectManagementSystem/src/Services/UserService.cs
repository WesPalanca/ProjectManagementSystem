namespace ProjectManagementSystem.Services;
using MySql.Data.MySqlClient;
using ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Factory;
using ProjectManagementSystem.Models;
public class UserService : IUserService
{
    private readonly IUserFactory _userFactory;
    private readonly IUserRepository _userRepository;

    public UserService(IUserFactory userFactory, IUserRepository userRepository)
    {
        _userFactory = userFactory;
        _userRepository = userRepository;
    }

    public User CreateUser(string firstname, string lastname, string email, string password, string role)
    {
        // Only create the User object via the factory
        User createdUser = _userFactory.CreateUser(firstname, lastname, email, password, role);

        // Persist user to the database (password sent separately)
        _userRepository.Add(createdUser, password);

        return createdUser;
    }

    public User? GetUserById(int id)
    {
        return _userRepository.GetById(id);
    }

    public User? GetUserByEmail(string email)
    {
        return _userRepository.GetByEmail(email);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }

    public void DeleteUser(int id)
    {
        _userRepository.Delete(id);
    }
}