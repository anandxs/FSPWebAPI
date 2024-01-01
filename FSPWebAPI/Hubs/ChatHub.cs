using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace FSPWebAPI.Hubs
{
    [Authorize(Roles = Constants.GLOBAL_ROLE_USER)]
    public class ChatHub : Hub
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ChatHub(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatMessageDto>> JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _repositoryManager.ChatRepository.GetAllMessagesForProjectAsync(Guid.Parse(groupName), false);
            var messagesDto = _mapper.Map<List<ChatMessageDto>>(messages);

            return messagesDto.OrderBy(x => x.SentAt);
        }

        public async Task SendMessage(string groupName, string senderName, string message)
        {
            if (message == "")
            {
                return;
            }

            var claims = Context.User.Claims;
            var user = new
            {
                userId = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value,
                email = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value,
                name = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email)).Value,
            };

            var chatMessage = new ChatMessage
            {
                Message = message,
                ProjectId = Guid.Parse(groupName),
                SenderId = user.userId,
                SentAt = DateTime.Now,
            };
            _repositoryManager.ChatRepository.AddMessage(chatMessage);
            await _repositoryManager.SaveAsync();

            var nameArray = senderName.Split(' ');
            chatMessage.Sender = new();
            chatMessage.Sender.Id = chatMessage.SenderId;
            chatMessage.Sender.FirstName = nameArray[0];
            chatMessage.Sender.LastName = nameArray[1];

            var messageDto = _mapper.Map<ChatMessageDto>(chatMessage);

            await Clients.Group(groupName).SendAsync("groupMessage", messageDto);
        }
    }
}
