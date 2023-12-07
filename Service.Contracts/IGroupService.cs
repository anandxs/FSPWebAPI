using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAllGroupsForProjectAsync(Guid projectId, string requesterId, bool trackChanges);
        Task<GroupDto> GetGroupByIdAsync(Guid groupId, string requesterId, bool trackChanges);
        Task<GroupDto> CreateGroupAsync(Guid projectId, string requesterId, GroupForCreationDto groupForCreation, bool trackChanges);
        Task UpdateGroupAsync(Guid groupId, string requesterId, GroupForUpdateDto groupForUpdate, bool trackChanges);
        Task ToggleArchive(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges);
        Task DeleteGroup(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges);
    }
}
