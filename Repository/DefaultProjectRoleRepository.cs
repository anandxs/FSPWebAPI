using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DefaultProjectRoleRepository : RepositoryBase<DefaultProjectRole>, IDefaultProjectRoleRepository
    {
        public DefaultProjectRoleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<DefaultProjectRole>> GetAllRolesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                        .ToListAsync();
        }

        public async Task<DefaultProjectRole> GetRoleByIdAsync(Guid roleId, bool trackChanges)
        {
            return await FindByCondition(r => r.RoleId.Equals(roleId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<DefaultProjectRole> GetRoleByNameAsync(string role, bool trackChanges)
        {
            return await FindByCondition(r => r.Name.Equals(role), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public void CreateRole(DefaultProjectRole role)
        {
            Create(role);
        }

        public void DeleteRole(DefaultProjectRole role)
        {
            Delete(role);
        }
    }
}
