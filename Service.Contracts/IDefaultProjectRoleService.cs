using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IDefaultProjectRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync(bool trackChanges);
        Task CreateRoleAsync(RoleForCreationDto role, bool trackChanges);
        Task UpdateRoleAsync(Guid roleId, RoleForUpdateDto role, bool trackChanges);
        Task DeleteRoleAsync(Guid roleId, bool trackChanges);
    }
}
