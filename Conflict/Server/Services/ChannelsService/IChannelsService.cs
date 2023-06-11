
namespace Conflict.Server.Services.ChannelsService
{
    public interface IChannelsService
    {
        List<Channel> GetAllChannels();
        Task<MessageDto> SendMessageToChannel(long channelToSendTo, SendMessageDto messageDto, long userId);
        List<MessageDto> GetMessagesFromChannel(long channelId);

    }
}
