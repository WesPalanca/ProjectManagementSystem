    namespace ProjectManagementSystem;
    using ProjectManagementSystem.Models;

    public class TeamMember : User
    {
        
        public TeamMember(string firstName, string lastName, string email, string password)
            : base(firstName, lastName, email, password,"TeamMember")
        {
            Permissions = new List<string> { "AcceptTask" ,"CompleteTask", "ReportTask" };
        }

    }