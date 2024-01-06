namespace Contracts;

public interface ITaskTypeRepository
{
    Task<IEnumerable<TaskType>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges);
    Task<TaskType> GetTaskTypeByIdAsync(Guid projectId, Guid typeId, bool trackChanges);
    Task<TaskType> GetTaskTypeByNameAsync(Guid project, string name, bool trackChanges);
    void CreateTaskType(TaskType taskType, Guid projectId);
    void DeleteTaskType(TaskType taskType);
}
