using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesForProjectAsync(Guid projectId, bool trackChanges);
        Task CreateRoleAsync(RoleForCreationDto role, bool trackChanges);
        Task UpdateRoleAsync(Guid roleId, RoleForUpdateDto role, bool trackChanges);
        Task DeleteRoleAsync(Guid roleId, bool trackChanges);
    }
}
