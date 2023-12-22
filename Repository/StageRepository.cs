using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class StageRepository : RepositoryBase<Stage>, IStageRepository
    {
        public StageRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Stage>> GetAllStagesForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(s => s.ProjectId.Equals(projectId), trackChanges)
                    .ToListAsync();
        }

        public async Task<Stage> GetStageByIdAsync(Guid projectId, Guid stageId, bool trackChanges)
        {
            return await FindByCondition(s => s.ProjectId.Equals(projectId) && s.StageId.Equals(stageId), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public async Task<Stage> GetStageByNameAsync(Guid projectId, string name, bool trackChanges)
        {
            return await FindByCondition(s => s.ProjectId.Equals(projectId) && s.Name.Equals(name), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public void CreateStage(Stage group, Guid projectId)
        {
            group.ProjectId = projectId;
            Create(group);
        }

        public void DeleteStage(Stage group)
        {
            Delete(group);
        }
    }
}
