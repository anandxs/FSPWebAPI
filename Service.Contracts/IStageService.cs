using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IStageService
    {
        Task<IEnumerable<StageDto>> GetAllStagesForProjectAsync(Guid projectId, string requesterId, bool trackChanges);
        Task<StageDto> GetStageByIdAsync(Guid stageId, string requesterId, bool trackChanges);
        Task<StageDto> CreateStageAsync(Guid projectId, string requesterId, StageForCreationDto stageForCreation, bool trackChanges);
        Task UpdateStageAsync(Guid stageId, string requesterId, StageForUpdateDto stageForUpdate, bool trackChanges);
        Task ToggleStageArchiveStatusAsync(Guid stageId, string requesterId, bool trackChanges);
        Task DeleteStageAsync(Guid stageId, string requesterId, bool trackChanges);
    }
}
