using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        
        public RoleService(
            IRepositoryManager repositoryManager, 
            ILoggerManager logger, 
            IMapper mapper, 
            UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ProjectRoleDto>> GetProjectRolesAsync(string userId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            var roles = await _repositoryManager.ProjectRoleRepository.GetAllProjectRoles(projectId, trackChanges);

            var rolesDto = _mapper.Map<IEnumerable<ProjectRoleDto>>(roles);

            return rolesDto;
        }

        private async Task CheckIfUserAndProjectExistsAsync(string userId, Guid projectId, bool trackChanges)
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
        }
    }
}
