using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.SignalR
{
    public class ChatHub(IUserAccessor userAccessor, ILogger<ChatHub> logger) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = await userAccessor.GetUserAsync();
            if (user == null)
            {
                logger.LogError("user not connected");
                throw new HubException("User not authenticated");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, user.Id);

            logger.LogInformation("User {Id} connected", user.Id);
        }

        public async Task SendPrivateMessage(string receiverId, string message)
        {
            var senderUser = await userAccessor.GetUserAsync();

            if (senderUser == null)
            {
                throw new HubException("User not authenticated");
            }

            var chatMessage = new
            {
                SenderId = senderUser.Id,
                SenderName = senderUser.DisplayName,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            await Clients.Group(receiverId).SendAsync("ReceiveMessage", message);
        }

    }
}
