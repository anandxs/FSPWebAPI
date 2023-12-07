using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardService
    {
        Task<IEnumerable<CardDto>> GetAllCardsForProjectAsync(Guid projectId, string requesterId, bool trackChanges);
        Task<IEnumerable<CardDto>> GetCardsForGroupAsync(string userId, Guid projectId, string requesterId, Guid groupId, bool trackChanges);
        Task<CardDto> GetCardByIdAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges);
        Task<(CardDto cardDto, Guid projectId)> CreateCardAsync(Guid groupId, string requesterId, CardForCreationDto cardForCreation, bool trackChanges);
        Task DeleteCardAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges);
        Task ToggleArchiveStatusAsync(Guid projectId, Guid cardId, string requesterId, bool trackChanges);
        Task UpdateCardAsync(Guid cardId, string requesterId, CardForUpdateDto cardForUpdate, bool trackChanges);
    }
}
