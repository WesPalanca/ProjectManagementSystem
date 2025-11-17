namespace ProjectManagementSystem.Models;
using ProjectManagementSystem.Enums;

public abstract class Task
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int AssignedBy { get; set; } // UserId
    public int AssignedTo { get; set; } // UserId
    public TaskStatus Status { get; set; }
    public DateTime Deadline { get; set; }
  

}