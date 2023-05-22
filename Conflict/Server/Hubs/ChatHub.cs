using Microsoft.AspNetCore.SignalR;
using Conflict.Shared;
using FlakeId;

namespace Conflict.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string content)
        {
            User user = new("Username", Id.Create());
            Message Message = new(content, user, Id.Create());

            await Clients.All.SendAsync("ReceiveMessage", Message);
        }
    }
}
