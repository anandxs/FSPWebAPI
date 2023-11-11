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
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var projectsFromDb = await _repositoryManager.ProjectRepository.GetProjectsOwnedByUserAsync(userId, trackChanges);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);

            return projectsDto;
        }

        public async Task<ProjectDto> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(userId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(string ownerId, ProjectForCreationDto project)
        {
            var user = await _userManager.FindByIdAsync(ownerId);

            if (user is null)
            {
                throw new UserNotFoundException(ownerId);
            }

            var projectEntity = _mapper.Map<Project>(project);
            projectEntity.CreatedAt = DateTime.Now;
            projectEntity.OwnerId = ownerId;

            _repositoryManager.ProjectRepository.CreateProject(projectEntity);
            await _repositoryManager.SaveAsync();

            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);

            return projectToReturn;
        }
    }
}
