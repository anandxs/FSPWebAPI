using Entities.Models;

namespace Contracts
{
    public interface IProjectRoleRepository
    {
        void DefaultProjectRoleCreation(Guid projectId);
        Task<ProjectRole> GetProjectRole(Guid projectId, string role, bool trackChanges);
    }
}
