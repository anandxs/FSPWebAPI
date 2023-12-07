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

        public async Task<GroupDto> GetGroupByIdAsync(Guid groupId, string requesterId, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group is null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var groupDto = _mapper.Map<GroupDto>(group);

            return groupDto;
        }

        public async Task<GroupDto> CreateGroupAsync(Guid projectId, string requesterId, GroupForCreationDto groupForCreation, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var project = await _repositoryManager.ProjectRepository.GetProjectOwnedByUserAsync(requesterId, projectId, trackChanges);

            if (project is null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var group = _mapper.Map<Group>(groupForCreation);

            _repositoryManager.GroupRepository.CreateGroup(group, projectId);
            await _repositoryManager.SaveAsync();

            var groupDto = _mapper.Map<GroupDto>(group);

            return groupDto;
        }
        public async Task UpdateGroupAsync(Guid groupId, string requesterId, GroupForUpdateDto groupForUpdate, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group == null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            _mapper.Map(groupForUpdate, group);
            await _repositoryManager.SaveAsync();
        }

        public async Task ToggleArchiveAsync(Guid groupId, string requesterId, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group == null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            group.IsActive = !group.IsActive; // maybe move to repository layer
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteGroupAsync(Guid groupId, string requesterId, bool trackChanges)
        {
            var group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

            if (group == null)
            {
                throw new GroupNotFoundException(groupId);
            }

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(group.ProjectId, requesterId, false);

            if (requester is null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            _repositoryManager.GroupRepository.DeleteGroup(group);
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

        private async Task<Group> GetGroupAndCheckIfItExistsAsync(Guid groupId, bool trackChanges)
        {
            var groupEntity = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

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
