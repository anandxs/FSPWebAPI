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
        private readonly Lazy<ITaskRepository> _taskRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<IAttachmentRepository> _attachmentRepository;
        private readonly Lazy<IProjectMemberRepository> _projectMemberRepository;
        private readonly Lazy<IUserInviteRepository> _userInviteRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IChatRepository> _chatRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _projectRepository = new Lazy<IProjectRepository>(() =>  new ProjectRepository(repositoryContext));
            _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(repositoryContext));
            _stageRepository = new Lazy<IStageRepository>(() => new StageRepository(repositoryContext));
            _taskTypeRepository = new Lazy<ITaskTypeRepository>(() => new TaskTypeRepository(repositoryContext));
            _tagRepository = new Lazy<ITagRepository>(() => new TagRepository(repositoryContext));
            _taskRepository = new Lazy<ITaskRepository>(() => new TaskRepository(repositoryContext));   
            _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(repositoryContext));
            _attachmentRepository = new Lazy<IAttachmentRepository>(() => new AttachmentRepository(repositoryContext));
            _projectMemberRepository = new Lazy<IProjectMemberRepository>(() => new ProjectMemberRepository(repositoryContext));
            _userInviteRepository = new Lazy<IUserInviteRepository>(() => new UserInviteRepository(repositoryContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
            _chatRepository = new Lazy<IChatRepository>(() => new ChatRepository(repositoryContext));
        }

        public IProjectRepository ProjectRepository => _projectRepository.Value;
        public IRoleRepository RoleRepository => _roleRepository.Value;
        public IStageRepository StageRepository => _stageRepository.Value;
        public ITaskTypeRepository TaskTypeRepository => _taskTypeRepository.Value;
        public ITagRepository TagRepository => _tagRepository.Value;
        public ITaskRepository TaskRepository => _taskRepository.Value;
        public ICommentRepository CommentRepository => _commentRepository.Value;
        public IAttachmentRepository AttachmentRepository => _attachmentRepository.Value;
        public IProjectMemberRepository ProjectMemberRepository => _projectMemberRepository.Value;
        public IUserInviteRepository UserInviteRepository => _userInviteRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;
        public IChatRepository ChatRepository => _chatRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
