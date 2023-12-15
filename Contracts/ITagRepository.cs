using Entities.Models;

namespace Contracts
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllProjectTags(Guid projectId, bool trackChanges);
        Task<Tag> GetTagById(Guid tagId, bool trackChanges);
        void CreateTag(Guid projectId, Tag tag);
        void DeleteTag(Tag tag);
    }
}
