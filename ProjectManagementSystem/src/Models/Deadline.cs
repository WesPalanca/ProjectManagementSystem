namespace ProjectManagementSystem.Models;

public struct Deadline
{
    public DateTime DueDate { get; }

    public Deadline(DateTime dueDate)
    {
        DueDate = dueDate;
    }

    public bool IsOverdue()
    {
        return DateTime.Now > DueDate;
    }

    public override string ToString()
    {
        return DueDate.ToString("yyyy-M-d dddd");
    }
    
}