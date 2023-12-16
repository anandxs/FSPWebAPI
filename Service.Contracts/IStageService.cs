using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IStageService
    {
        Task<IEnumerable<StageDto>> GetAllStagesForProjectAsync(Guid projectId, string requesterId, bool trackChanges);
        Task<StageDto> GetStageByIdAsync(Guid projectId, Guid stageId, string requesterId, bool trackChanges);
        Task<StageDto> CreateStageAsync(Guid projectId, string requesterId, StageForCreationDto stageForCreation, bool trackChanges);
        Task UpdateStageAsync(Guid projectId, Guid stageId, string requesterId, StageForUpdateDto stageForUpdate, bool trackChanges);
        Task ToggleStageArchiveStatusAsync(Guid projectId, Guid stageId, string requesterId, bool trackChanges);
        Task DeleteStageAsync(Guid projectId, Guid stageId, string requesterId, bool trackChanges);
    }
}
