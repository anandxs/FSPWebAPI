using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetGroupsForProjectAsync(string userId, Guid projectId, bool trackChanges);
        Task<GroupDto> GetGroupByIdAsync(string userId, Guid projectId, Guid groupId, bool trackChanges);
        Task<GroupDto> CreateGroupAsync(string userId, Guid projectId, GroupForCreationDto groupForCreation, bool trackChanges);
        Task UpdateGroupAsync(string userId, Guid projectId, Guid groupId, GroupForUpdateDto groupForUpdate, bool trackChanges);
        Task ToggleArchive(string userId, Guid projectId, Guid groupId, bool trackChanges);
        Task DeleteGroup(string userId, Guid projectId, Guid groupId, bool trackChanges);
    }
}
