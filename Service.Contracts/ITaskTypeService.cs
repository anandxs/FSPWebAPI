using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITaskTypeService
    {
        Task<IEnumerable<TaskTypeDto>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges);
        Task<TaskTypeDto> GetTaskTypeByIdAsync(Guid projectId, Guid typeId, bool trackChanges);
        Task<TaskTypeDto> CreateTaskTypeAsync(Guid projectId, TaskTypeForCreationDto taskTypeForCreation, bool trackChanges);
        Task UpdateTaskTypeAsync(Guid projectId, Guid typeId, TaskTypeForUpdateDto taskTypeForUpdateDto, bool trackChanges);
        Task ToggleTaskTypeArchiveStatusAsync(Guid projectId, Guid typeId, bool trackChanges);
        Task DeleteTaskTypeAsync(Guid projectId, Guid typeId, bool trackChanges);
    }
}
