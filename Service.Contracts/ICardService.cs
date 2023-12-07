using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardService
    {
        Task<IEnumerable<CardDto>> GetCardsForProjectAsync(string userId, Guid projectId, string requesterId, bool trackChanges);
        Task<IEnumerable<CardDto>> GetCardsForGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges);
        Task<CardDto> GetCardByIdAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges);
        Task<(CardDto cardDto, Guid projectId)> CreateCardAsync(Guid groupId, string requesterId, CardForCreationDto cardForCreation, bool trackChanges);
        Task DeleteCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges);
        Task ToggleArchiveStatusAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, bool trackChanges);
        Task UpdateCardAsync(string userId, Guid projectId, string requesterId, Guid groupId, Guid cardId, CardForUpdateDto cardForUpdate, bool trackChanges);
    }
}
