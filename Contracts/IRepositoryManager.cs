namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        IStageRepository StageRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ICardRepository CardRepository { get; }
        IRoleRepository RoleRepository { get; }
        ICardMemberRepository CardMemberRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}
