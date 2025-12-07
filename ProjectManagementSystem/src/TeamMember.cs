    using ProjectManagementSystem.Enums;

    namespace ProjectManagementSystem;
    using ProjectManagementSystem.Models;

    public class TeamMember : User
    {
        
        public TeamMember(string firstName, string lastName, string email, string password)
            : base(firstName, lastName, email, password,"TeamMember")
        {
            UserPermissions = new List<Permissions> { Permissions.AcceptTask, Permissions.CompleteTask, Permissions.ReportTask };
        }

    }