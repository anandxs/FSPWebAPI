﻿namespace Repository;

public class ProjectMemberRepository : RepositoryBase<ProjectMember>, IProjectMemberRepository
{
    public ProjectMemberRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<ProjectMember>> GetAllProjectMembersAsync(Guid projectId, bool trackChanges)
    {
        return await FindByCondition(m => m.ProjectId.Equals(projectId), trackChanges)
                .Include(m => m.User)
                .Include(m => m.Role)
                .ToListAsync();
    }

    public void AddProjectMember(ProjectMember member)
    {
        Create(member);
    }

    public async Task<ProjectMember> GetProjectMemberAsync(Guid projectId, string memberId, bool trackChanges)
    {
        return await FindByCondition(m => m.ProjectId.Equals(projectId) && m.MemberId.Equals(memberId), trackChanges)
                .Include(m => m.User)
                .Include(m => m.Role)
                .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<ProjectMember>> GetProjectsForMemberAsync(string requesterdId, bool trackChanges)
    {
        return await FindByCondition(m => m.MemberId.Equals(requesterdId), trackChanges)
                .Include(m => m.Project)
                .Include(m => m.Role)
                .ToListAsync();
    }

    public async Task<ProjectMember> GetProjectForMemberAsync(string requesterdId, Guid projectId,bool trackChanges)
    {
        return await FindByCondition(m => m.MemberId.Equals(requesterdId) && m.ProjectId.Equals(projectId), trackChanges)
                .Include(m => m.Project)
                .Include(m => m.Role)
                .SingleOrDefaultAsync();
    }

    public void RemoveMember(ProjectMember projectMember)
    {
        Delete(projectMember);
    }
}
