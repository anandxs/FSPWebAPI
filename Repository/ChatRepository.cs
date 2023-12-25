using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ChatRepository : RepositoryBase<ChatMessage>, IChatRepository
    {
        public ChatRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesForProjectAsync(Guid projectId, bool trackChanges)
        {
            return await FindByCondition(c => c.ProjectId.Equals(projectId), trackChanges)
		    .Include(c => c.Sender)
                    .ToListAsync();
        }

        public void AddMessage(ChatMessage message)
        {
            Create(message);
        }
    }
}
