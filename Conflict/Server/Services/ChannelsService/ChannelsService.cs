using AutoMapper;
using Conflict.Server.Data;
using Conflict.Server.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Conflict.Server.Services.ChannelsService
{
    public class ChannelsService : IChannelsService
    {
        private readonly DataContext _dataContext;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IMapper _mapper;

        public ChannelsService(DataContext dataContext, IHubContext<ChatHub> chatHub, IMapper mapper)
        {
            _dataContext = dataContext;
            _chatHub = chatHub;
            _mapper = mapper;
        }

        public List<Channel> GetAllChannels()
        {
            List<Channel> channels = _dataContext.Channels.ToList();

            return channels;
        }

        public async Task<MessageDto> SendMessageToChannel(long channelToSendTo, SendMessageDto messageDto, long userId)
        {
            Message message = new()
            {
                Content = messageDto.Content,
                Id = FlakeId.Id.Create(),
                AuthorId = userId,
                ChannelId = channelToSendTo,
            };

            _dataContext.Messages.Add(message);
            await _dataContext.SaveChangesAsync();

            MessageDto returnMessage = _mapper.Map<MessageDto>(_dataContext.Messages.Include(m => m.Author).SingleOrDefault(m => m.Id == message.Id));
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", returnMessage);

            return returnMessage;
        }

        public List<MessageDto> GetMessagesFromChannel(long channelId)
        {
			List<Message> MessagesFromDb = _dataContext.Messages.Where(message => message.ChannelId == channelId).Include(message => message.Author).OrderByDescending(message => message.Id).ToList();
            List<MessageDto> Messages = _mapper.Map<List<MessageDto>>(MessagesFromDb);

            return Messages;
		}
    }
}
