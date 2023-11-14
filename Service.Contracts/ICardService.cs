using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardService
    {
        Task<IEnumerable<CardDto>> GetCardsForGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges);
        Task<CardDto> GetCardByIdAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges);
        Task<CardDto> CreateCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, CardForCreationDto cardForCreation, bool trackChanges);
        Task DeleteCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges);
        Task ToggleArchiveStatusAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges);
        Task UpdateCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, CardForUpdateDto cardForUpdate, bool trackChanges);
    }
}
