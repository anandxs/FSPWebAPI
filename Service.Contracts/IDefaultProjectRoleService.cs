using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IDefaultProjectRoleService
    {
        Task<IEnumerable<DefaultProjectRoleDto>> GetAllRolesAsync(bool trackChanges);
        Task CreateRoleAsync(DefaultProjectRoleForCreationDto role, bool trackChanges);
        Task UpdateRoleAsync(Guid roleId, DefaultProjectRoleForUpdateDto role, bool trackChanges);
        Task DeleteRoleAsync(Guid roleId, bool trackChanges);
    }
}
