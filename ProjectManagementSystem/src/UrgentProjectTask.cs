namespace ProjectManagementSystem;
using ProjectManagementSystem.Models;
public class UrgentProjectTask : ProjectTask
{
    public UrgentProjectTask(
        string title, 
        string description, 
        int assignedBy, 
        int assignedTo, 
        Deadline deadline
    ) : base(title, description, assignedBy, assignedTo,  deadline, "Urgent")
    {
    }

    public override string Info()
    {
        return $"[URGENT] {Title} (ID: {TaskId})\nDeadline: {Deadline:yyyy-M-d dddd} | Status: {Status}\nAssigned By ID: (#{AssignedBy}) | Assigned To: (#{AssignedTo})\n";
    }

}