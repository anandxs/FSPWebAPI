using Entities.Models;

namespace Contracts
{
    public interface IGroupRepository
    {
        Task<Group> GetGroupByIdAsync(Guid projectId, Guid groupId, bool trackChanges);
        void CreateGroup(Group group, Guid projectId);
    }
}
