namespace ProjectManagementSystem.Repositories;
using ProjectManagementSystem.Models;
public interface IProjectTaskRepository
{
    void Add(ProjectTask projectTask);
    ProjectTask? GetById(int id);
    List<ProjectTask> GetAll();
    List<ProjectTask> GetByAssignedTo(int userId);
    List<ProjectTask> GetByAssignedBy(int userId);
    void Delete(int id);
}