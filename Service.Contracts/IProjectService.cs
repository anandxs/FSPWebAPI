namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsUserIsPartOfAsync(string requesterId, bool trackChanges);
        Task<ProjectDto> GetProjectUserIsPartOfAsync(string requesterId, Guid projectId, bool trackChanges);
        Task<ProjectDto> CreateProjectAsync(string requesterId, ProjectForCreationDto project);
        Task UpdateProjectAsync(string requesterId, Guid projectId, ProjectForUpdateDto project, bool trackChanges);
        Task ToggleArchive(string requesterId, Guid projectId, bool trackChanges);
        Task DeleteProject(string requesterId, Guid projectId, bool trackChanges);
    }
}
