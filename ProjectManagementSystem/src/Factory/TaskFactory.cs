namespace ProjectManagementSystem.Factory;
using ProjectManagementSystem.Models;
public class TaskFactory
{
    public ProjectTask? CreateTask(string title, string description, int assignedBy, int assignedTo, DateTime deadline,
        string taskType)
    {
        switch (taskType)
        {
            case "Standard":
                return new StandardProjectTask(title, description, assignedBy, assignedTo, deadline);
                
            case "Urgent":
                return new UrgentProjectTask(title, description, assignedBy, assignedTo, deadline);
        }

        return null;
    }
}