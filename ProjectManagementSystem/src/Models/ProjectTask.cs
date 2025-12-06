namespace ProjectManagementSystem.Models;

using ProjectManagementSystem.Enums;

public abstract class ProjectTask
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AssignedBy { get; set; } // UserId
    public int AssignedTo { get; set; } // UserId
    public ProjectTaskStatus Status { get; set; }
    public Deadline Deadline { get; set; }
    public string TaskType { get; set; }

    public ProjectTask(string title, string description, int assignedBy, int assignedTo, Deadline deadline,
        string taskType)
    {
        Title = title;
        Description = description;
        AssignedBy = assignedBy;
        AssignedTo = assignedTo;
        Status = ProjectTaskStatus.Unassigned;
        Deadline = deadline;
        TaskType = taskType;
    }
  

}