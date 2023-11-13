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

        public async Task<IEnumerable<ProjectDto>> GetProjectsOwnedByUserAsync(string userId, string requesterId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(userId);

            CheckIfRequesterIdAndRouteUserIdMatch(userId, requesterId);

            var projectsFromDb = await _repositoryManager.ProjectRepository.GetProjectsOwnedByUserAsync(userId, trackChanges);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectOwnedByUserAsync(string userId, Guid projectId, string requesterId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(userId);

            CheckIfRequesterIdAndRouteUserIdMatch(userId, requesterId);

            var project = await GetProjectAndCheckIfItExistsAsync(userId, projectId, trackChanges);

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

            _repositoryManager.ProjectRoleRepository.DefaultProjectRoleCreation(projectEntity.ProjectId);
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

        private async Task<Project> GetProjectAndCheckIfItExistsAsync(string userId, Guid projectId, bool trackChanges)
        {
            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(userId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            return project;
        }

        private async Task CheckIfRequesterIsAuthorized(Guid projectId, string requesterId, HashSet<string> allowedRoles)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMember(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberException();
            }

            var requesterRole = await _repositoryManager.ProjectRoleRepository.GetProjectRoleById(projectId, (Guid)requester.ProjectRoleId, false);

            if (!allowedRoles.Contains(requesterRole.Name))
            {
                throw new IncorrectRoleException();
            }
        }

        private void CheckIfRequesterIdAndRouteUserIdMatch(string userId, string requesterId)
        {
            if (userId != requesterId)
            {
                throw new IncorrectRoleException();
            }
        }

        #endregion
    }
}
