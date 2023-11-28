using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsUserIsPartOfAsync(string requesterId, bool trackChanges);
        Task<ProjectDto> GetProjectUserIsPartOfAsync(string userId, Guid projectId, string requesterId, bool trackChanges);
        Task<ProjectDto> CreateProjectAsync(string ownerId, string requesterId, ProjectForCreationDto project);
        Task UpdateProjectAsync(string ownerId, Guid projectId, string requesterId, ProjectForUpdateDto project, bool trackChanges);
        Task ToggleArchive(string ownerId, Guid projectId, string requesterId, bool trackChanges);
        Task DeleteProject(string ownerId, Guid projectId, string requesterId, bool trackChanges);
    }
}
