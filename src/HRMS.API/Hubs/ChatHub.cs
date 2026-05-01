using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HRMS.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinChannel(string channelId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, channelId);

    public async Task LeaveChannel(string channelId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);

    public async Task SendMessageToChannel(string channelId, string content) =>
        await Clients.Group(channelId).SendAsync("ReceiveMessage", new
        {
            ChannelId = channelId,
            SenderId = Context.UserIdentifier,
            Content = content,
            SentAt = DateTime.UtcNow
        });

    public async Task SendDirectMessage(string receiverUserId, string content) =>
        await Clients.User(receiverUserId).SendAsync("ReceiveDirectMessage", new
        {
            SenderId = Context.UserIdentifier,
            Content = content,
            SentAt = DateTime.UtcNow
        });
}
