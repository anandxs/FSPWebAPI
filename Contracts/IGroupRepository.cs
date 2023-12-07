using Entities.Models;

namespace Contracts
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllGroupsForProjectAsync(Guid projectId, bool trackChanges);
        Task<Group> GetGroupByIdAsync(Guid projectId, Guid groupId, bool trackChanges);
        void CreateGroup(Group group, Guid projectId);
        void DeleteGroup(Group group);
    }
}
