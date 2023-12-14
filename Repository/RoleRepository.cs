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

        public async Task<Role> GetRoleByNameAsync(string role, bool trackChanges)
        {
            return await FindByCondition(r => r.Name.Equals(role), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void CreateRole(Role role)
        {
            Create(role);
        }

        public void DeleteRole(Role role)
        {
            Delete(role);
        }
    }
}
