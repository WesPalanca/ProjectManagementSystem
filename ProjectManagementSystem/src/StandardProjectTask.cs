namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class StandardProjectTask : ProjectTask
{
    public StandardProjectTask(
        string title, 
        string description, 
        int assignedBy, 
        int assignedTo, 
        Deadline deadline
    ) : base(title, description, assignedBy, assignedTo, deadline, "Standard")
    {

    }
}