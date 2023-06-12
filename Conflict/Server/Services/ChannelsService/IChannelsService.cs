
namespace Conflict.Server.Services.ChannelsService
{
    public interface IChannelsService
    {
        List<Channel> GetAllChannels();
        Task<Channel> CreateChannel(CreateChannelDto channelDto, long ownerId);
        Task<Channel?> DeleteChannel(long channelId);
        Task<MessageDto> SendMessageToChannel(long channelToSendTo, SendMessageDto messageDto, long userId);
        List<MessageDto> GetMessagesFromChannel(long channelId);

    }
}
