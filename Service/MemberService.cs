﻿using AutoMapper;
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

        public async Task<IEnumerable<ProjectMemberDto>> GetAllProjectMembersAsync(Guid projectId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var members = await _repositoryManager.ProjectMemberRepository.GetAllProjectMembersAsync(projectId, trackChanges);

            var membersDto = _mapper.Map<IEnumerable<ProjectMemberDto>>(members);

            return membersDto;
        }

        public async Task AddMemberAsync(Guid projectId, string requesterId, MemberForCreationDto memberDto, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(projectId, memberDto.RoleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(memberDto.RoleId);
            }

            var newMember = await _userManager.FindByEmailAsync(memberDto.Email);

            if (newMember == null)
            {
            }
            else
            {
                var existingMember = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, newMember.Id, trackChanges);

                if (existingMember != null)
                {
                    throw new MemberAlreadyExistsBadRequest(memberDto.Email);
                }

                _repositoryManager.ProjectMemberRepository.AddProjectMember(new ProjectMember
                {
                    MemberId = newMember.Id,
                    ProjectId = projectId,
                    RoleId = memberDto.RoleId
                });
                await _repositoryManager.SaveAsync();

                string subject = "Added To Project";
                string message = $"<p>You have been added to the {requester.Project.Name} project by {requester.User.FirstName} {requester.User.LastName}.</p>";
                await _emailService.SendAsync(memberDto.Email, subject, message, true);
            }
        }

        public async Task<ProjectMemberDto> GetProjectMemberAsync(Guid projectId, string memberId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, memberId, trackChanges);

            if (member == null)
            {
                throw new MemberNotFoundException(memberId);
            }

            var projectMemberDto = _mapper.Map<ProjectMemberDto>(member);
            return projectMemberDto;
        }

        public async Task ChangeMemberRoleAsync(Guid projectId, string requesterId, MemberForUpdateDto memberDto, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var existingMember = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, memberDto.MemberId, trackChanges);

            if (existingMember == null)
            {
                throw new NotAProjectMemberBadRequestException(memberDto.MemberId);
            }

            var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(projectId, memberDto.RoleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(memberDto.RoleId);
            }

            existingMember.RoleId = memberDto.RoleId;
            await _repositoryManager.SaveAsync();
        }

        public async Task RemoveMemberAsync(Guid projectId, string memberId, string requesterId, bool trackChanges)
        {
            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, memberId, false);

            if (member == null)
            {
                throw new MemberNotFoundException(memberId);
            }

            if (memberId.Equals(member.Project.OwnerId))
            {
                throw new OwnerCannotBeRemovedBadRequestException();
            }

            if (member.Role.Name == Constants.PROJECT_ROLE_ADMIN)
            {
                var members = await _repositoryManager.ProjectMemberRepository.GetAllProjectMembersAsync(projectId, trackChanges);
                var adminCount = members.Where(m => m.Role.Name == Constants.PROJECT_ROLE_ADMIN).Count();


                if (adminCount == 1)
                {
                    throw new NotEnoughAdminsBadRequestException();
                }
            }

            _repositoryManager.ProjectMemberRepository.RemoveMember(member);
            await _repositoryManager.SaveAsync();
        }

        public async Task ExitProjectAsync(Guid projectId, string requesterId, bool trackChanges)
        {
            var members = await _repositoryManager.ProjectMemberRepository.GetAllProjectMembersAsync(projectId, trackChanges);

            var requester = members.Where(m => m.MemberId.Equals(requesterId)).SingleOrDefault();

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var entity = await _repositoryManager.ProjectMemberRepository.GetProjectForMemberAsync(requesterId, projectId, trackChanges);

            if (entity == null || entity.Project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            if (entity.Project.OwnerId == requesterId)
            {
                throw new OwnerCannotBeRemovedBadRequestException();
            }

            var adminCount = members.Where(m => m.Role.Name == Constants.PROJECT_ROLE_ADMIN).Count();

            if (requester.Role.Name == Constants.PROJECT_ROLE_ADMIN && adminCount == 1) 
            {
                throw new NotEnoughAdminsBadRequestException();
            }
            else
            {
                _repositoryManager.ProjectMemberRepository.RemoveMember(requester);
                await _repositoryManager.SaveAsync();
            }
        }
    }
}
