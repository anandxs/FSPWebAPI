namespace Contracts;

public interface IChatRepository
{
    Task<IEnumerable<ChatMessage>> GetAllMessagesForProjectAsync(Guid projectId, bool trackChanges);
    void AddMessage(ChatMessage message);
}
