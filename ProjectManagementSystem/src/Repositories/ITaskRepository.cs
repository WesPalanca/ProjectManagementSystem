namespace ProjectManagementSystem.Repositories;

public interface ITaskRepository
{
    void Add(ProjectManagementSystem.Models.ProjectTask projectTask);
    ProjectManagementSystem.Models.ProjectTask? GetById(int id);
    List<ProjectManagementSystem.Models.ProjectTask> GetAll();
    List<ProjectManagementSystem.Models.ProjectTask> GetByAssignedTo(int userId);
    List<ProjectManagementSystem.Models.ProjectTask> GetByAssignedBy(int userId);
    void Delete(int id);
}