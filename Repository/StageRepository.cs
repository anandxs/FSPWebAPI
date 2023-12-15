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

        public async Task<Stage> GetStageByIdAsync(Guid stageId, bool trackChanges)
        {
            return await FindByCondition(s => s.StageId.Equals(stageId), trackChanges)
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
