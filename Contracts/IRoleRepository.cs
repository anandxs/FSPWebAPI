using Entities.Models;

namespace Contracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync(bool trackChanges);
        Task<Role> GetRoleByIdAsync(Guid roleId, bool trackChanges);
        Task<Role> GetRoleByNameAsync(string role, bool trackChanges);
        void CreateRole(Role role);
        void DeleteRole(Role role);
    }
}
