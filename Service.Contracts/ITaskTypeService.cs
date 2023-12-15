using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITaskTypeService
    {
        Task<IEnumerable<TaskTypeDto>> GetAllTaskTypesForProjectAsync(Guid projectId, bool trackChanges);
        Task<TaskTypeDto> GetTaskTypeByIdAsync(Guid typeId, bool trackChanges);
        Task<TaskTypeDto> CreateTaskTypeAsync(Guid projectId, TaskTypeForCreationDto taskTypeForCreation, bool trackChanges);
        Task UpdateTaskTypeAsync(Guid typeId, TaskTypeForUpdateDto taskTypeForUpdateDto, bool trackChanges);
        Task ToggleTaskTypeArchiveStatusAsync(Guid typeId, bool trackChanges);
        Task DeleteTaskTypeAsync(Guid typeId, bool trackChanges);
    }
}
