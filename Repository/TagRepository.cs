﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Tag>> GetAllProjectTags(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(t => t.ProjectId.Equals(projectId), trackChanges)
                    .ToListAsync();
        }

        public async Task<Tag> GetTagById(Guid tagId, bool trackChanges)
        {
            return await FindByCondition(t => t.TagId.Equals(tagId), trackChanges)
                    .SingleOrDefaultAsync();
        }

        public void CreateTag(Guid projectId, Tag tag)
        {
            tag.ProjectId = projectId;
            Create(tag);
        }

        public void DeleteTag(Tag tag)
        {
            Delete(tag);
        }
    }
}
