using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Role>> GetAllRolesForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(r => r.ProjectId.Equals(projectId), trackChanges)
                    .ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId, bool trackChanges)
        {
            return await FindByCondition(r => r.RoleId.Equals(roleId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<Role> GetRoleByNameAsync(Guid projectId, string role, bool trackChanges)
        {
            return await FindByCondition(r => r.Name.Equals(role) && r.ProjectId.Equals(projectId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void CreateRole(Guid projectId, Role role)
        {
            role.ProjectId = projectId;
            Create(role);
        }

        public void DeleteRole(Role role)
        {
            Delete(role);
        }
    }
}
