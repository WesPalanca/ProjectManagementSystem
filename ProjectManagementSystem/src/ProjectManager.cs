namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class ProjectManager : User
{

    public ProjectManager(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password,"ProjectManager")
    {
       Permissions = new List<string> { "AssignTask", "ApproveTask", "ReportTask" };
    }
}