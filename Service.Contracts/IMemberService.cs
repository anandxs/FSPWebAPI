namespace Service.Contracts;

public interface IMemberService
{
    Task<IEnumerable<ProjectMemberDto>> GetAllProjectMembersAsync(Guid projectId, bool trackChanges);
    Task<ProjectMemberDto> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges);
    Task<string> AddMemberAsync(Guid projectId, MemberForCreationDto memberDto, bool trackChanges);
    Task<bool> AcceptInviteAsync(Guid projectId);
    Task ChangeMemberRoleAsync(Guid projectId, MemberForUpdateDto memberDto, bool trackChanges);
    Task RemoveMemberAsync(Guid projectId, string memberId, bool trackChanges);
    Task ExitProjectAsync(Guid projectId, bool trackChanges);
}
