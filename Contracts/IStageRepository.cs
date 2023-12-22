using Entities.Models;

namespace Contracts
{
    public interface IStageRepository
    {
        Task<IEnumerable<Stage>> GetAllStagesForProjectAsync(Guid projectId, bool trackChanges);
        Task<Stage> GetStageByIdAsync(Guid projectId, Guid stageId, bool trackChanges);
        Task<Stage> GetStageByNameAsync(Guid projectId, string name, bool trackChanges);
        void CreateStage(Stage stage, Guid projectId);
        void DeleteStage(Stage stage);
    }
}
