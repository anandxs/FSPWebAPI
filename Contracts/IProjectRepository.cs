using Entities.Models;

namespace Contracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
    }
}
