
using Microsoft.AspNetCore.Mvc;

namespace Conflict.Server.Services.ChannelsService
{
    public interface IChannelsService
    {
        ActionResult<List<Channel>> GetAllChannels();
        Task<ActionResult<Channel>> CreateChannel(CreateChannelDto channelDto, long ownerId);
        Task<ActionResult<Channel>> DeleteChannel(long channelId);
        Task<ActionResult<MessageDto>> SendMessageToChannel(long channelToSendTo, SendMessageDto messageDto, long userId);
        ActionResult<List<MessageDto>> GetMessagesFromChannel(long channelId);

    }
}
