namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsUserIsPartOfAsync(bool trackChanges);
        Task<ProjectDto> GetProjectUserIsPartOfAsync(Guid projectId, bool trackChanges);
        Task<ProjectDto> CreateProjectAsync(ProjectForCreationDto project);
        Task UpdateProjectAsync(Guid projectId, ProjectForUpdateDto project, bool trackChanges);
        Task ToggleArchive(Guid projectId, bool trackChanges);
        Task DeleteProject(Guid projectId, bool trackChanges);
    }
}
