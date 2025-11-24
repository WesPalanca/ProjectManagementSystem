namespace ProjectManagementSystem.Factory;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Enums;
public interface ITaskFactory
{
    ProjectTask? CreateTask(string title, string description, int assignedBy, int assignedTo, DateTime deadline, string taskType);
}