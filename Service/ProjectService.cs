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

        public async Task<IEnumerable<ProjectDto>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(userId);

            var projectsFromDb = await _repositoryManager.ProjectRepository.GetProjectsOwnedByUserAsync(userId, trackChanges);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(userId);

            var project = await GetProjectAndCheckIfItExistsAsync(userId, projectId, trackChanges);

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(string ownerId, ProjectForCreationDto project)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = _mapper.Map<Project>(project);

            _repositoryManager.ProjectRepository.CreateProject(projectEntity, ownerId);
            await _repositoryManager.SaveAsync();

            _repositoryManager.ProjectRoleRepository.DefaultProjectRoleCreation(projectEntity.ProjectId);
            await _repositoryManager.SaveAsync();

            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);

            return projectToReturn;
        }

        public async Task UpdateProject(string ownerId, Guid projectId, ProjectForUpdateDto projectForUpdate, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            _mapper.Map(projectForUpdate, projectEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchive(string ownerId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            projectEntity.IsActive = !projectEntity.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProject(string ownerId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserExistsAsync(ownerId);

            var projectEntity = await GetProjectAndCheckIfItExistsAsync(ownerId, projectId, trackChanges);

            _repositoryManager.ProjectRepository.DeleteProject(projectEntity);
            await _repositoryManager.SaveAsync();
        }

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
    }
}
