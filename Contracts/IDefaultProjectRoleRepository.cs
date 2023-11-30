﻿using Entities.Models;

namespace Contracts
{
    public interface IDefaultProjectRoleRepository
    {
        Task<IEnumerable<DefaultProjectRole>> GetRolesAsync(bool trackChanges);
        Task<DefaultProjectRole> GetRoleAsync(Guid roleId, bool trackChanges);
        Task<DefaultProjectRole> GetRoleByNameAsync(string role, bool trackChanges);
        void CreateRole(DefaultProjectRole role);
        void DeleteRole(DefaultProjectRole role);
    }
}