namespace Service.Contracts;

public interface IStageService
{
    Task<IEnumerable<StageDto>> GetAllStagesForProjectAsync(Guid projectId, bool trackChanges);
    Task<StageDto> GetStageByIdAsync(Guid projectId, Guid stageId, bool trackChanges);
    Task<StageDto> CreateStageAsync(Guid projectId, StageForCreationDto stageForCreation, bool trackChanges);
    Task UpdateStageAsync(Guid projectId, Guid stageId, StageForUpdateDto stageForUpdate, bool trackChanges);
    Task ToggleStageArchiveStatusAsync(Guid projectId, Guid stageId, bool trackChanges);
    Task DeleteStageAsync(Guid projectId, Guid stageId, bool trackChanges);
}
