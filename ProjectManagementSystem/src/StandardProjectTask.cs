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

    public override string Info()
    {
        return $"[STANDARD] {Title} (ID: {TaskId})\nDeadline: {Deadline:yyyy-M-d dddd} | Status: {Status}\nAssigned By ID: (#{AssignedBy}) | Assigned To: (#{AssignedTo})\n";
    }

 
}