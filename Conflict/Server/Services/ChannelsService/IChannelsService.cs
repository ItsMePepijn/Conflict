using Conflict.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Conflict.Server.Services.ChannelsService
{
    public interface IChannelsService
    {
        List<Channel> GetAllChannels();
        Task<Message> SendMessageToChannel(MessageDto messageDto, User user);

    }
}
