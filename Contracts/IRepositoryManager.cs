namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IGroupRepository GroupRepository { get; }
        IProjectRoleRepository ProjectRoleRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ICardRepository CardRepository { get; }
        Task SaveAsync();
    }
}
