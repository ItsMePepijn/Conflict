using Microsoft.AspNetCore.SignalR;
using FlakeId;
using Conflict.Shared.Models;

namespace Conflict.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string content)
        {
            User User = new()
            {
                Name = "Username",
                //Id = Id.Create()
            };
            Message Message = new()
            {
                Content = content,
                //Id = Id.Create(),
                Author = User,
            };

            await Clients.All.SendAsync("ReceiveMessage", Message);
        }
    }
}
