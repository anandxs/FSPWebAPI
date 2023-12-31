namespace Contracts
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllProjectTagsAsync(Guid projectId, bool trackChanges);
        Task<Tag> GetTagByIdAsync(Guid projectId, Guid tagId, bool trackChanges);
        void CreateTag(Guid projectId, Tag tag);
        void DeleteTag(Tag tag);
    }
}
