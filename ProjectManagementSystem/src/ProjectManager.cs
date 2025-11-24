namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class ProjectManager : User
{
    public List<ProjectTask> TasksCreated { get; set; }

    public ProjectManager(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password,"ProjectManager")
    {
        TasksCreated = new List<ProjectTask>();
    }
}