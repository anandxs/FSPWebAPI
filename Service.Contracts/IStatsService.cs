using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IStatsService
    {
        Task<IEnumerable<TasksPerStageDto>> GetTasksPerStageAsync(Guid projectId, bool trackChanges);
        Task<IEnumerable<TasksPerTypeDto>> GetTasksPerTypeAsync(Guid projectId, bool trackChanges);
    }
}
