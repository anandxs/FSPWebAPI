using Entities.Models;

namespace Contracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
        Task<Project> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges);
        void CreateProject(Project project);
    }
}
