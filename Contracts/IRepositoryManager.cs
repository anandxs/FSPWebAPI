namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IGroupRepository GroupRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ICardRepository CardRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICardMemberRepository CardMemberRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
