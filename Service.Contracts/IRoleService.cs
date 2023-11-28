using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IRoleService
    {
        Task<IEnumerable<ProjectRoleDto>> GetProjectRolesAsync(string userId, Guid projectId, bool trackChanges);
    }
}
