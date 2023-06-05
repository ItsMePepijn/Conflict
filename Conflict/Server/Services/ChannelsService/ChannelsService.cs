using Conflict.Server.Data;
using Conflict.Server.Hubs;
using Conflict.Shared.Dto;
using Microsoft.AspNetCore.SignalR;

namespace Conflict.Server.Services.ChannelsService
{
    public class ChannelsService : IChannelsService
    {
        private readonly DataContext _dataContext;
        private readonly IHubContext<ChatHub> _chatHub;

        public ChannelsService(DataContext dataContext, IHubContext<ChatHub> chatHub)
        {
            _dataContext = dataContext;
            _chatHub = chatHub;
        }

        public List<Channel> GetAllChannels()
        {
            List<Channel> channels = _dataContext.Channels.ToList();

            return channels;
        }

        public async Task<Message> SendMessageToChannel(MessageDto messageDto, User user)
        {
            Message message = new()
            {
                Content = messageDto.Content,
                Id = FlakeId.Id.Create(),
                Author = user
            };

            await _chatHub.Clients.All.SendAsync("ReceiveMessage", message);

            return message;
        }
    }
}
