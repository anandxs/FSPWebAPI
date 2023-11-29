using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IDefaultProjectRoleService
    {
        Task<IEnumerable<DefaultProjectRoleDto>> GetRolesAsync(bool trackChanges);
        Task CreateRole(DefaultProjectRoleForCreationDto role);
        Task DeleteRole(Guid roleId, bool trackChanges);
        Task UpdateRole(Guid roleId, DefaultProjectRoleForUpdateDto role, bool trackChanges);
    }
}
