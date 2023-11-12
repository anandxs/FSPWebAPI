using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IGroupService
    {
        Task<GroupDto> GetGroupByIdAsync(string userId, Guid projectId, Guid groupId, bool trackChanges);
        Task<GroupDto> CreateGroupAsync(string userId, Guid projectId, GroupForCreationDto groupForCreation, bool trackChanges);
    }
}
