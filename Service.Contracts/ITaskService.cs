using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<ProjectTaskDto>> GetAllTasksForProjectAsync(Guid projectId, bool trackChanges);
        Task<ProjectTaskDto> GetTaskByIdAsync(Guid projectId, Guid taskId, bool trackChanges);
        Task<ProjectTaskDto> CreateTaskAsync(Guid projectId, TaskForCreationDto taskForCreationDto, bool trackChanges);
        Task UpdateTaskAsync(Guid projectId, Guid taskId, TaskForUpdateDto taskForUpdateDto, bool trackChanges);
        Task DeleteTaskAsync(Guid projectId, Guid taskId, bool trackChanges);
    }
}
