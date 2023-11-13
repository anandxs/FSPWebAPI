namespace Contracts
{
    public interface IProjectMemberRepository
    {
        void AddProjectMember(Guid projectId, string memberId, Guid roleId);
    }
}
