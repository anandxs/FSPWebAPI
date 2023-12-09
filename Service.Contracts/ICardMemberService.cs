using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICardMemberService
    {
        Task<IEnumerable<ProjectUserDto>> GetAllCardAssigneesAsync(Guid projectId, Guid cardId, bool 
trackChanges);
        Task<IEnumerable<CardDto>> GetAllAssignedCardsForMemberAsync(Guid projectId, string memberId, bool trackChanges);
        Task AssignMemberToCardAsync(Guid projectId, Guid cardId, string memberId, bool trackChanges);
        Task UnassignMemberFromCardAsync(Guid projectid, Guid cardId, string memberId, bool trackChanges);
    }
}
