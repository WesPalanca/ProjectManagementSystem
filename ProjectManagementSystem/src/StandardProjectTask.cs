namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class StandardProjectTask : ProjectTask
{
    public StandardProjectTask(
        string title, 
        string description, 
        int assignedBy, 
        int assignedTo, 
        DateTime deadline
    ) : base(title, description, assignedBy, assignedTo, deadline, "Standard")
    {
        // SummaryStrategy = new StandardSummaryStrategy();
    }
}