using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FSPWebAPI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatRegisry _chatRegistry;

        public ChatHub(ChatRegisry chatRegisry)
        {
            _chatRegistry = chatRegisry;
        }

        public async Task<List<UserMessage>> JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            return _chatRegistry.GetMessages(groupName).OrderBy(x => x.SendAt).ToList();
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string groupName, string message)
        {
            var claims = Context.User.Claims;
            var user = new
            {
                userId = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value,
                email = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value,
                name = claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email)).Value,
            };

            _chatRegistry.AddMessage(groupName, new UserMessage(user.name, message, groupName, DateTimeOffset.Now));
            await Clients.Group(groupName).SendAsync("groupMessage", user, message);
        }
    }
}
