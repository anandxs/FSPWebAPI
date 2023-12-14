using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesForProjectAsync(Guid projectId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var member = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (member == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }

            var roles = await _repositoryManager.RoleRepository.GetAllRolesForProjectAsync(projectId, trackChanges);

            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return rolesDto;
        }

        public async Task CreateRoleAsync(Guid projectId, RoleForCreationDto role, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var existingRole = await _repositoryManager.RoleRepository.GetRoleByNameAsync(projectId, role.Name, trackChanges);

            if (existingRole != null)
            {
                throw new DuplicateEntryBadRequest();
            }

            var roleEntity = _mapper.Map<Role>(role);

            _repositoryManager.RoleRepository.CreateRole(projectId, roleEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateRoleAsync(Guid projectId, Guid roleId, RoleForUpdateDto role, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var roleEntity = await _repositoryManager.RoleRepository.GetRoleByIdAsync(roleId, trackChanges);

            if (roleEntity == null)
            {
                throw new RoleNotFoundException(roleId);
            }

            _mapper.Map(role, roleEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteRoleAsync(Guid projectId, Guid roleId, bool trackChanges)
        {
            var requesterId = GetRequesterId();

            var requester = await _repositoryManager.ProjectMemberRepository.GetProjectMemberAsync(projectId, requesterId, trackChanges);

            if (requester == null)
            {
                throw new NotAProjectMemberForbiddenRequestException();
            }
            else if (requester.Role.Name != Constants.PROJECT_ROLE_ADMIN)
            {
                throw new IncorrectRoleForbiddenRequestException();
            }

            var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(roleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }

            var members = await _repositoryManager.ProjectMemberRepository.GetAllProjectMembersAsync(projectId, trackChanges);

            var roleMemberCount = members.Where(m => m.RoleId.Equals(roleId)).Count();

            if (roleMemberCount != 0)
            {
                throw new RoleNotEmptyBadRequestException(role.Name);
            }

            _repositoryManager.RoleRepository.DeleteRole(role);
            await _repositoryManager.SaveAsync();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }
    }
}
