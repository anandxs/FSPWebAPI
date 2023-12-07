using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Group>> GetAllGroupsForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(g => g.ProjectId.Equals(projectId), trackChanges)
                    .ToListAsync();
        }

        public async Task<Group> GetGroupByIdAsync(Guid projectId, Guid groupId, bool trackChanges)
        {
            return await FindByCondition(g => g.ProjectId.Equals(projectId) && g.GroupId.Equals(groupId), trackChanges).SingleOrDefaultAsync();
        }

        public void CreateGroup(Group group, Guid projectId)
        {
            group.ProjectId = projectId;
            Create(group);
        }

        public void DeleteGroup(Group group)
        {
            Delete(group);
        }
    }
}
