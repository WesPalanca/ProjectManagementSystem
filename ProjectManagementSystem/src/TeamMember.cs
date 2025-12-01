namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;

public class TeamMember : User
{
    public List<ProjectTask> AssignedTasks { get; set; }
    
    public TeamMember(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password,"TeamMember")
    {
        AssignedTasks = new List<ProjectTask>();
    }

}