using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class DefaultProjectRoleService : IDefaultProjectRoleService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public DefaultProjectRoleService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync(bool trackChanges)
        {
            var roles = await _repositoryManager.RoleRepository.GetAllRolesAsync(trackChanges);

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
