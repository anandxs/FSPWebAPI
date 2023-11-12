using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class GroupService : IGroupService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GroupService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsForProjectAsync(string userId, Guid projectId, bool trackChanges)
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

            var groups = await _repositoryManager.GroupRepository.GetGroupsForProjectAsync(projectId, trackChanges);
            var groupsDto = _mapper.Map<IEnumerable<GroupDto>>(groups);

            return groupsDto;
        }

        public async Task<GroupDto> GetGroupByIdAsync(string userId, Guid projectId, Guid groupId, bool trackChanges)
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

            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(projectId, groupId, trackChanges);

            if (group is null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var groupDto = _mapper.Map<GroupDto>(group);

            return groupDto;
        }

        public async Task<GroupDto> CreateGroupAsync(string userId, Guid projectId, GroupForCreationDto groupForCreation, bool trackChanges)
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

            var groupEntity = _mapper.Map<Group>(groupForCreation);

            _repositoryManager.GroupRepository.CreateGroup(groupEntity, projectId);
            await _repositoryManager.SaveAsync();

            var groupToReturn = _mapper.Map<GroupDto>(groupEntity);

            return groupToReturn;
        }
    }
}
