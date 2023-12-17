using Entities.Models;

namespace Contracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesForProjectAsync(Guid projectId, bool trackChanges);
        Task<Role> GetRoleByIdAsync(Guid projectId, Guid roleId, bool trackChanges);
        Task<Role> GetRoleByNameAsync(Guid projectId, string role, bool trackChanges);
        void CreateRole(Guid projectId, Role role);
        void DeleteRole(Role role);
    }
}
