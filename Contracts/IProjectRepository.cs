namespace Contracts;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
    Task<Project> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges);
    Task<Project> GetProjectByIdAsync(Guid projectId, bool trackChanges);
    void CreateProject(Project project, string ownerId);
    void DeleteProject(Project project);
}
