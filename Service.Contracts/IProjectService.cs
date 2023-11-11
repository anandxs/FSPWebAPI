using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
        Task<ProjectDto> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges);
        Task<ProjectDto> CreateProjectAsync(string ownerId, ProjectForCreationDto project);
    }
}
