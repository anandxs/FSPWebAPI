namespace Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository ProjectRepository { get; }
        Task SaveAsync();
    }
}
