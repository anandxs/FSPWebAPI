using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
using Service.Contracts;
using Shared;
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

        public async Task AddMemberAsync(Guid projectId, string requesterId, MemberForCreationDto memberDto, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

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

            string role;
            if (memberDto.Role == Constants.PROJECT_ROLE_ADMIN || memberDto.Role == Constants.PROJECT_ROLE_MEMBER)
            {
                role = memberDto.Role;
            }
            else
            {
                role = Constants.PROJECT_ROLE_OBSERVER;
            }

            _repositoryManager.ProjectMemberRepository.AddProjectMember(new ProjectMember
            {
                MemberId = newMember.Id,
                ProjectId = projectId,
                Role = role,
            });
            await _repositoryManager.SaveAsync();
        }

        public async Task RemoveMemberAsync(Guid projectId, string memberId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

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
