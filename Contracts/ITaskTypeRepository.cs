using Entities.Models;

namespace Contracts
{
    public interface ITaskTypeRepository
    {
        Task<IEnumerable<TaskType>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges);
        Task<TaskType> GetTaskTypeByIdAsync(Guid projectId, Guid typeId, bool trackChanges);
        void CreateTaskType(TaskType taskType, Guid projectId);
        void DeleteTaskType(TaskType taskType);
    }
}
