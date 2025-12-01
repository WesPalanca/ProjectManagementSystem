using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Factory;

public class UserFactory : IUserFactory
{
    public User? CreateUser(string firstName, string lastName, string email, string password, string role)
    {
       
        switch (role)
        {
            case "ProjectManager":
                return new ProjectManager(firstName, lastName, email, password);
                
            
            case "TeamMember":
                return new TeamMember(firstName, lastName, email, password);
        }

        return null;

    }
}