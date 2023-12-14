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

        public async Task CreateRoleAsync(RoleForCreationDto role, bool trackChanges)
        {
            var existingRole = await _repositoryManager.RoleRepository.GetRoleByNameAsync(role.Name, trackChanges);

            if (existingRole != null)
            {
                throw new DuplicateEntryBadRequest();
            }

            var roleEntity = _mapper.Map<Role>(role);

            _repositoryManager.RoleRepository.CreateRole(roleEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateRoleAsync(Guid roleId, RoleForUpdateDto role, bool trackChanges)
        {
            var roleEntity = await GetRoleAndCheckIfItExistsAsync(roleId, trackChanges);

            _mapper.Map(role, roleEntity);

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteRoleAsync(Guid roleId, bool trackChanges)
        {
            var role = await GetRoleAndCheckIfItExistsAsync(roleId, trackChanges);

            _repositoryManager.RoleRepository.DeleteRole(role);
            await _repositoryManager.SaveAsync();
        }

        private string GetRequesterId()
        {
            var claimsIdentity = (ClaimsIdentity)_contextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return claim!.Value;
        }

        private async Task<Role> GetRoleAndCheckIfItExistsAsync(Guid roleId, bool trackChanges)
        {
            var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(roleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }

            return role;
        }
    }
}
