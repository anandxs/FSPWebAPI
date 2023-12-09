﻿using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<IGroupRepository> _groupRepository;
        private readonly Lazy<IProjectMemberRepository> _projectMemberRepository;
        private readonly Lazy<ICardRepository> _cardRepository;
        private readonly Lazy<IDefaultProjectRoleRepository> _defaultProjectRoleRepository;
        private readonly Lazy<ICardMemberRepository> _cardMemberRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _projectRepository = new Lazy<IProjectRepository>(() =>  new ProjectRepository(repositoryContext));
            _groupRepository = new Lazy<IGroupRepository>(() => new GroupRepository(repositoryContext));
            _projectMemberRepository = new Lazy<IProjectMemberRepository>(() => new ProjectMemberRepository(repositoryContext));
            _cardRepository = new Lazy<ICardRepository>(() => new CardRepository(repositoryContext));
            _defaultProjectRoleRepository = new Lazy<IDefaultProjectRoleRepository>(() => new DefaultProjectRoleRepository(repositoryContext));
            _cardMemberRepository = new Lazy<ICardMemberRepository>(() => new CardMemberRepository(repositoryContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        }

        public IProjectRepository ProjectRepository => _projectRepository.Value;
        public IGroupRepository GroupRepository => _groupRepository.Value;
        public IProjectMemberRepository ProjectMemberRepository => _projectMemberRepository.Value;
        public ICardRepository CardRepository => _cardRepository.Value;
        public IDefaultProjectRoleRepository DefaultProjectRoleRepository => _defaultProjectRoleRepository.Value;
        public ICardMemberRepository CardMemberRepository => _cardMemberRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
