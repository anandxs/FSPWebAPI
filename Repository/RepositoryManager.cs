﻿using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<IGroupRepository> _groupRepository;
        private readonly Lazy<IProjectRoleRepository> _projectRoleRepository;
        private readonly Lazy<IProjectMemberRepository> _projectMemberRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _projectRepository = new Lazy<IProjectRepository>(() =>  new ProjectRepository(repositoryContext));
            _groupRepository = new Lazy<IGroupRepository>(() => new GroupRepository(repositoryContext));
            _projectRoleRepository = new Lazy<IProjectRoleRepository>(() => new ProjectRoleRepository(repositoryContext));
            _projectMemberRepository = new Lazy<IProjectMemberRepository>(() => new ProjectMemberRepository(repositoryContext));
        }

        public IProjectRepository ProjectRepository => _projectRepository.Value;
        public IGroupRepository GroupRepository => _groupRepository.Value;
        public IProjectRoleRepository ProjectRoleRepository => _projectRoleRepository.Value;
        public IProjectMemberRepository ProjectMemberRepository => _projectMemberRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
