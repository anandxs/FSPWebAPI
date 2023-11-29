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

        public void CreateRole(DefaultProjectRole role)
        {
            Create(role);
        }

        public void DeleteRole(DefaultProjectRole role)
        {
            Delete(role);
        }

        public async Task<DefaultProjectRole> GetRoleAsync(Guid roleId, bool trackChanges)
        {
            return await FindByCondition(r => r.RoleId.Equals(roleId), trackChanges)
                        .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<DefaultProjectRole>> GetRolesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                        .ToListAsync();
        }
    }
}
