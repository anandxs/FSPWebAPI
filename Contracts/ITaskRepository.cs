namespace Contracts;

public interface ITaskRepository
{
    Task<IEnumerable<ProjectTask>> GetAllTasksForProjectAsync(Guid projectId, bool trackChanges);
    Task<ProjectTask> GetTaskByIdAsync(Guid taskId, bool trackChanges);
    Task<float> GetTotalHoursRequiredForProjectAsync(Guid projectId);
    void CreateTask(ProjectTask task);
    void DeleteTask(ProjectTask task);
}
