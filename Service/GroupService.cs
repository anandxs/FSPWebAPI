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

        public async Task<IEnumerable<GroupDto>> GetAllGroupsForProjectAsync(Guid projectId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var groups = await _repositoryManager.GroupRepository.GetAllGroupsForProjectAsync(projectId, trackChanges);
            var groupsDto = _mapper.Map<IEnumerable<GroupDto>>(groups);

            return groupsDto;
        }

        public async Task<GroupDto> GetGroupByIdAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member", "Observer" });

            var group = await GetGroupAndCheckIfItExistsAsync(projectId, groupId, trackChanges);

            var groupDto = _mapper.Map<GroupDto>(group);

            return groupDto;
        }

        public async Task<GroupDto> CreateGroupAsync(string userId, Guid projectId, string requesterId, GroupForCreationDto groupForCreation, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            var groupEntity = _mapper.Map<Group>(groupForCreation);

            _repositoryManager.GroupRepository.CreateGroup(groupEntity, projectId);
            await _repositoryManager.SaveAsync();

            var groupToReturn = _mapper.Map<GroupDto>(groupEntity);

            return groupToReturn;
        }
        public async Task UpdateGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, GroupForUpdateDto groupForUpdate, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            var groupEntity = await GetGroupAndCheckIfItExistsAsync(projectId, groupId, trackChanges);

            _mapper.Map(groupForUpdate, groupEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchive(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            var groupEntity = await GetGroupAndCheckIfItExistsAsync(projectId, groupId, trackChanges);

            groupEntity.IsActive = !groupEntity.IsActive;
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteGroup(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRequesterIsAuthorized(projectId, requesterId, new HashSet<string> { "Admin", "Member" });

            var groupEntity = await GetGroupAndCheckIfItExistsAsync(projectId, groupId, trackChanges);

            _repositoryManager.GroupRepository.DeleteGroup(groupEntity);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS

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

        private async Task<Group> GetGroupAndCheckIfItExistsAsync(Guid projectId, Guid groupId, bool trackChanges)
        {
            var groupEntity = await _repositoryManager.GroupRepository.GetGroupByIdAsync(projectId, groupId, trackChanges);

            if (groupEntity is null)
            {
                throw new GroupNotFoundException(groupId);
            }

            return groupEntity;
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

        #endregion
    }
}
