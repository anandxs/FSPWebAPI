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

            var projects = entities.Select(m => m.Project);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectUserIsPartOfAsync(string requesterId, Guid projectId, bool trackChanges)
        {
            var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

            var project = entity?.Project;

            if (entity is null || project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(string requesterId, ProjectForCreationDto project)
        {
            var user = await _userManager.FindByIdAsync(requesterId);

            if (user is null)
            {
                throw new UserNotFoundException(requesterId);
            }

            var projectEntity = _mapper.Map<Project>(project);

            _repositoryManager.ProjectRepository.CreateProject(projectEntity, requesterId);
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

            _repositoryManager.ProjectMemberRepository.AddProjectMember(projectEntity.ProjectId, requesterId, adminRole.RoleId);
            await _repositoryManager.SaveAsync();

            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);

            return projectToReturn;
        }

        public async Task UpdateProjectAsync(string requesterId, Guid projectId, ProjectForUpdateDto projectForUpdateDto, bool trackChanges)
        {
            var requester = await _userManager.FindByIdAsync(requesterId);

            if (requester == null)
            {
                throw new UserNotFoundException(requesterId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(requesterId, projectId, trackChanges);

            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            _mapper.Map(projectForUpdateDto, project);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchive(string requesterId, Guid projectId, bool trackChanges)
        {
            var requester = await _userManager.FindByIdAsync(requesterId);

            if (requester == null)
            {
                throw new UserNotFoundException(requesterId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(requesterId, projectId, trackChanges);

            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            project.IsActive = !project.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProject(string requesterId, Guid projectId, bool trackChanges)
        {
            var requester = await _userManager.FindByIdAsync(requesterId);

            if (requester == null)
            {
                throw new UserNotFoundException(requesterId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(requesterId, projectId, trackChanges);

            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            _repositoryManager.ProjectRepository.DeleteProject(project);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS


        #endregion
    }
}
