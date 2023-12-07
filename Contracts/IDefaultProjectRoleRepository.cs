using Entities.Models;

namespace Contracts
{
    public interface IDefaultProjectRoleRepository
    {
        Task<IEnumerable<DefaultProjectRole>> GetAllRolesAsync(bool trackChanges);
        Task<DefaultProjectRole> GetRoleByIdAsync(Guid roleId, bool trackChanges);
        Task<DefaultProjectRole> GetRoleByNameAsync(string role, bool trackChanges);
        void CreateRole(DefaultProjectRole role);
        void DeleteRole(DefaultProjectRole role);
    }
}
