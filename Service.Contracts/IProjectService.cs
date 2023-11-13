﻿using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsOwnedByUserAsync(string userId, bool trackChanges);
        Task<ProjectDto> GetProjectOwnedByUserAsync(string userId, Guid projectId, bool trackChanges);
        Task<ProjectDto> CreateProjectAsync(string ownerId, ProjectForCreationDto project);
        Task UpdateProjectAsync(string ownerId, Guid projectId, string requesterId, ProjectForUpdateDto project, bool trackChanges);
        Task ToggleArchive(string ownerId, Guid projectId, bool trackChanges);
        Task DeleteProject(string ownerId, Guid projectId, bool trackChanges);
    }
}
