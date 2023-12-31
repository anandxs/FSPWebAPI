namespace Service.Contracts
{
    public interface IMemberService
    {
        Task<IEnumerable<ProjectMemberDto>> GetAllProjectMembersAsync(Guid projectId, string requesterId, bool trackChanges);
        Task<ProjectMemberDto> GetProjectMemberAsync(Guid projectId, string memberId,string requesterId, bool trackChanges);
        Task<string> AddMemberAsync(Guid projectId, string requesterId, MemberForCreationDto memberDto, bool trackChanges);
        Task<bool> AcceptInviteAsync(Guid projectId, string requesterId);
        Task ChangeMemberRoleAsync(Guid projectId, string requesterId, MemberForUpdateDto memberDto, bool trackChanges);
        Task RemoveMemberAsync(Guid projectId, string memberId, string requesterId, bool trackChanges);
        Task ExitProjectAsync(Guid projectId, string requesterId, bool trackChanges);
    }
}
