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

        public async Task<IEnumerable<DefaultProjectRoleDto>> GetAllRolesAsync(bool trackChanges)
        {
            var roles = await _repositoryManager.DefaultProjectRoleRepository.GetAllRolesAsync(trackChanges);

            var rolesDto = _mapper.Map<IEnumerable<DefaultProjectRoleDto>>(roles);

            return rolesDto;
        }

        public async Task CreateRoleAsync(DefaultProjectRoleForCreationDto role, bool trackChanges)
        {
            var existingRole = await _repositoryManager.DefaultProjectRoleRepository.GetRoleByNameAsync(role.Name, trackChanges);

            if (existingRole != null)
            {
                throw new DuplicateEntryBadRequest();
            }

            var roleEntity = _mapper.Map<DefaultProjectRole>(role);

            _repositoryManager.DefaultProjectRoleRepository.CreateRole(roleEntity);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateRoleAsync(Guid roleId, DefaultProjectRoleForUpdateDto role, bool trackChanges)
        {
            var roleEntity = await GetRoleAndCheckIfItExistsAsync(roleId, trackChanges);

            _mapper.Map(role, roleEntity);

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteRoleAsync(Guid roleId, bool trackChanges)
        {
            var role = await GetRoleAndCheckIfItExistsAsync(roleId, trackChanges);

            _repositoryManager.DefaultProjectRoleRepository.DeleteRole(role);
            await _repositoryManager.SaveAsync();
        }

        private async Task<DefaultProjectRole> GetRoleAndCheckIfItExistsAsync(Guid roleId, bool trackChanges)
        {
            var role = await _repositoryManager.DefaultProjectRoleRepository.GetRoleByIdAsync(roleId, trackChanges);

            if (role == null)
            {
                throw new RoleNotFoundException(roleId);
            }

            return role;
        }
    }
}
