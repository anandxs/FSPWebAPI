using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsForProjectAsync(Guid projectId, bool trackChanges);
        Task<TagDto> GetTagByIdAsync(Guid projectId, Guid tagId, bool trackChanges);
        Task<TagDto> CreateTagAsync(Guid projectId, TagForCreationDto tagForCreationDto, bool trackChanges);
        Task UpdateTagAsync(Guid projectId, Guid tagId, TagForUpdateDto tagForUpdateDto, bool trackChanges);
        Task DeleteTagAsync(Guid projectId, Guid tagId, bool trackChanges);
    }
}
