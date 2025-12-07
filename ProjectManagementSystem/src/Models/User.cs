using ProjectManagementSystem.Enums;

namespace ProjectManagementSystem.Models;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; } // should only hold hashed value
    public string Role { get; set; }
    
    public List<Permissions> UserPermissions { get; set; }

    public User(string firstName, string lastName, string email, string password, string role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = role;
    }

    public bool HasPermission(Permissions permission)
    {
        return UserPermissions.Contains(permission);
    }


    }