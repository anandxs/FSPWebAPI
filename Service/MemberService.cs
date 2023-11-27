using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class MemberService : IMemberService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public MemberService(
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

        public async Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(string userId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            var members = await _repositoryManager.ProjectMemberRepository.GetProjectMembersAsync(projectId, trackChanges);

            var membersDto = _mapper.Map<IEnumerable<ProjectMemberDto>>(members);

            return membersDto;
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
