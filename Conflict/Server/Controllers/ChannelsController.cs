using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Conflict.Shared;
using Conflict.Server.Services.ChannelsService;

namespace Conflict.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : Controller
    {
        private readonly IChannelsService _channelsService;

        public ChannelsController(IChannelsService channelsService)
        {
            _channelsService = channelsService;
        }

        [HttpGet]
        public ActionResult<List<Channel>> GetAllChannels()
        {
            ActionResult<List<Channel>> channels = _channelsService.GetAllChannels();

            return channels;
        }

        [HttpPost]
        public async Task<ActionResult<Channel>> CreateChannel(CreateChannelDto channelDto)
        {
			IEnumerable<Claim> claims = JwtProvider.ParseClaimsFromJwt(Request.Headers.Authorization!);
			long userId = long.Parse(claims.Where(claim => claim.Type == "id").First().Value);

			ActionResult<Channel> channel = await _channelsService.CreateChannel(channelDto, userId);
            return channel;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Channel>> DeleteChannel(long id)
        {
            ActionResult<Channel> deletedChannel = await _channelsService.DeleteChannel(id);

            return deletedChannel;
        }

        [HttpPost("{id}/messages")]
        public async Task<ActionResult<MessageDto>> SendMessage(SendMessageDto messageDto, long id)
        {
            IEnumerable<Claim> claims = JwtProvider.ParseClaimsFromJwt(Request.Headers.Authorization!);
            long userId = long.Parse(claims.Where(claim => claim.Type == "id").First().Value);

            ActionResult<MessageDto> message = await _channelsService.SendMessageToChannel(id, messageDto, userId);
            return message;
        }

        [HttpGet("{id}/messages")]
        public ActionResult<List<MessageDto>> GetMessagesFromChannel(long id)
        {
            ActionResult<List<MessageDto>> messages = _channelsService.GetMessagesFromChannel(id);

            return messages;
        }
    }
}
