using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
    }
}
