using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
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
        private readonly IEmailService _emailService;

        public MemberService(
            IRepositoryManager repositoryManager,
            ILoggerManager logger,
            IMapper mapper,
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(string userId, Guid projectId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            var members = await _repositoryManager.ProjectMemberRepository.GetProjectMembersAsync(projectId, trackChanges);

            var membersDto = _mapper.Map<IEnumerable<ProjectMemberDto>>(members);

            return membersDto;
        }

        public async Task InviteUserAsync(string requesterId, string userId, Guid projectId, MemberForCreationDto memberDto, bool trackChanges)
        {
            await CheckIfRequesterIsAuthorizedAsync(projectId, requesterId, new HashSet<string> { "Admin" });

            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);

            await CheckIfRoleExistsAsync(projectId, memberDto.RoleId, trackChanges);

            var newMember = await _userManager.FindByEmailAsync(memberDto.Email);

            if (newMember == null)
            {
                _logger.LogWarn($"User with email : {memberDto.Email} not found");
                throw new UserNotFoundException(memberDto.Email);
            }

            var existingMember = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, newMember.Id, trackChanges);

            if (existingMember != null)
            {
                throw new MemberAlreadyExistsBadRequest(memberDto.Email);
            }

            _repositoryManager.ProjectMemberRepository.AddProjectMember(projectId, newMember.Id, memberDto.RoleId);
            await _repositoryManager.SaveAsync();
        }

        public async Task RemoveMemberAsync(string requesterId, string userId, Guid projectId, string memberId, bool trackChanges)
        {
            await CheckIfUserAndProjectExistsAsync(userId, projectId, trackChanges);
            
            await CheckIfRequesterIsAuthorizedAsync(projectId, requesterId, new HashSet<string> { "Admin" });

            var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, memberId, false);

            if (member == null)
            {
                throw new MemberNotFoundException(memberId);
            }

            _repositoryManager.ProjectMemberRepository.RemoveMember(member);
            await _repositoryManager.SaveAsync();
        }

        #region HELPER METHODS
        private async Task CheckIfRequesterIsAuthorizedAsync(Guid projectId, string requesterId, HashSet<string> allowedRoles)
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

        private async Task CheckIfRoleExistsAsync(Guid projectId, Guid roleId, bool trackChanges)
        {
            var role = await _repositoryManager.ProjectRoleRepository.GetProjectRoleById(projectId, roleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }
        }

        #endregion
    }
}
