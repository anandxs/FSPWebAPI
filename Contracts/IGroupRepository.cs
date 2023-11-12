using Entities.Models;

namespace Contracts
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsForProjectAsync(Guid projectId, bool trackChanges);
        Task<Group> GetGroupByIdAsync(Guid projectId, Guid groupId, bool trackChanges);
        void CreateGroup(Group group, Guid projectId);
    }
}
