using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<IRoleRepository> _roleRepository;
        private readonly Lazy<IStageRepository> _stageRepository;
        private readonly Lazy<ITaskTypeRepository> _taskTypeRepository;
        private readonly Lazy<ITagRepository> _tagRepository;
        private readonly Lazy<IProjectMemberRepository> _projectMemberRepository;
        private readonly Lazy<ICardRepository> _cardRepository;
        private readonly Lazy<ICardMemberRepository> _cardMemberRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _projectRepository = new Lazy<IProjectRepository>(() =>  new ProjectRepository(repositoryContext));
            _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(repositoryContext));
            _stageRepository = new Lazy<IStageRepository>(() => new StageRepository(repositoryContext));
            _taskTypeRepository = new Lazy<ITaskTypeRepository>(() => new TaskTypeRepository(repositoryContext));
            _tagRepository = new Lazy<ITagRepository>>(() => new TagRepository(repositoryContext));
            _projectMemberRepository = new Lazy<IProjectMemberRepository>(() => new ProjectMemberRepository(repositoryContext));
            _cardRepository = new Lazy<ICardRepository>(() => new CardRepository(repositoryContext));
            _cardMemberRepository = new Lazy<ICardMemberRepository>(() => new CardMemberRepository(repositoryContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        }

        public IProjectRepository ProjectRepository => _projectRepository.Value;
        public IRoleRepository RoleRepository => _roleRepository.Value;
        public IStageRepository StageRepository => _stageRepository.Value;
        public ITaskTypeRepository TaskTypeRepository => _taskTypeRepository.Value;
        public ITagRepository TagRepository => _tagRepository.Value;
        public IProjectMemberRepository ProjectMemberRepository => _projectMemberRepository.Value;
        public ICardRepository CardRepository => _cardRepository.Value;
        public ICardMemberRepository CardMemberRepository => _cardMemberRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
