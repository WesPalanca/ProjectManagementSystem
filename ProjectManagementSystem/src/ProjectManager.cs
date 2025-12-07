using ProjectManagementSystem.Enums;

namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class ProjectManager : User
{

    public ProjectManager(string firstName, string lastName, string email, string password)
        : base(firstName, lastName, email, password,"ProjectManager")
    {
       UserPermissions = new List<Permissions> { Permissions.CreateTask, Permissions.AssignTask, Permissions.ApproveTask, Permissions.ReportTask, Permissions.DeleteTask };
    }
}