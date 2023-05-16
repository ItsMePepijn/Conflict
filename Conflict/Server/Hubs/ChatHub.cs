using Microsoft.AspNetCore.SignalR;
using Conflict.Shared;
using FlakeId;

namespace Conflict.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string content)
        {
            Message Message = new(content, Id.Create());
            await Clients.All.SendAsync("ReceiveMessage", user, Message);
        }
    }
}
