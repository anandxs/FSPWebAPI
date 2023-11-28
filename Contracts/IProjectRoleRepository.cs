using Entities.Models;

namespace Contracts
{
    public interface IProjectRoleRepository
    {
        void DefaultProjectRoleCreation(Guid projectId);
        Task<ProjectRole> GetProjectRoleByName(Guid projectId, string role, bool trackChanges);
        Task<ProjectRole> GetProjectRoleById(Guid projectId, Guid roleId, bool trackChanges);
        Task<IEnumerable<ProjectRole>> GetAllProjectRoles(Guid projectId, bool trackChanges);
    }
}
