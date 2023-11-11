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
    }
}
