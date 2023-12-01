using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class ProjectService : IProjectService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ProjectService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsUserIsPartOfAsync(string requesterId, bool trackChanges)
        {
            var entities = await _repositoryManager.ProjectMemberRepository.GetProjectsForMemberAsync(requesterId, trackChanges);

            var x = entities.Select(m => m.Project);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(x);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectUserIsPartOfAsync(string userId, Guid projectId, string requesterId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(userId);

            var project = await GetProjectAndCheckIfItExistsAsync(requesterId, projectId, trackChanges);

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(string ownerId, string requesterId, ProjectForCreationDto project)
        {
            await CheckIfUserExistsAsync(ownerId);

            CheckIfRequesterIdAndRouteUserIdMatch(ownerId, requesterId);

            var projectEntity = _mapper.Map<Project>(project);

            _repositoryManager.ProjectRepository.CreateProject(projectEntity, ownerId);
            await _repositoryManager.SaveAsync();

            var defaultRoles = await _repositoryManager.DefaultProjectRoleRepository.GetRolesAsync(false);

            foreach (var defaultRole in defaultRoles)
            {
                var role = new ProjectRole
                {
                    Name = defaultRole.Name,
                    ProjectId = projectEntity.ProjectId
                };
                _repositoryManager.ProjectRoleRepository.CreateRole(role);  
            }
            await _repositoryManager.SaveAsync();

            var adminRole = await _repositoryManager.ProjectRoleRepository.GetProjectRoleByName(projectEntity.ProjectId, "Admin", false);

            _repositoryManager.ProjectMemberRepository.AddProjectMember(projectEntity.ProjectId, ownerId, adminRole.RoleId);
            await _repositoryManager.SaveAsync();

            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);

            return projectToReturn;
        }

        public async Task UpdateProjectAsync(string ownerId, Guid projectId, string requesterId, ProjectForUpdateDto projectForUpdate, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin" });

            _mapper.Map(projectForUpdate, projectEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchive(string ownerId, Guid projectId, string requesterId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin" });

            projectEntity.IsActive = !projectEntity.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProject(string ownerId, Guid projectId, string requesterId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin" });

            _repositoryManager.ProjectRepository.DeleteProject(projectEntity);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS

        private async Task CheckIfUserExistsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
        }

        private async Task<Project> GetProjectAndCheckIfItExistsAsync(string requesterId, Guid projectId, bool trackChanges)
        {
            var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

            var project = entity.Project;

            if (entity is null || project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            return project;
        }

        private async Task CheckIfRequesterIsAuthorized(Guid projectId, string requesterId, HashSet<string> allowedRoles)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var requesterRole = await _repositoryManager.ProjectRoleRepository.GetProjectRoleById(projectId, (Guid)requester.ProjectRoleId, false);

            if (!allowedRoles.Contains(requesterRole.Name))
            {
                throw new IncorrectRoleForbiddenRequestException();
            }
        }

        private void CheckIfRequesterIdAndRouteUserIdMatch(string userId, string requesterId)
        {
            if (userId != requesterId)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }
        }

        #endregion
    }
}
