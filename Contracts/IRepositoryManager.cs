namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IGroupRepository GroupRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ICardRepository CardRepository { get; }
        IDefaultProjectRoleRepository DefaultProjectRoleRepository { get; }
        ICardMemberRepository CardMemberRepository { get; }
        Task SaveAsync();
    }
}
