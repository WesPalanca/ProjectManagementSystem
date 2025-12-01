namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class UrgentProjectTask : ProjectTask
{
    public UrgentProjectTask(
        string title, 
        string description, 
        int assignedBy, 
        int assignedTo, 
        DateTime deadline
    ) : base(title, description, assignedBy, assignedTo, deadline, "Urgent")
    {
    }
}