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
            List<Channel> channels = new List<Channel>();
            channels.AddRange(_channelsService.GetAllChannels());

            return Ok(channels);
        }

        [HttpPost("{id}/messages")]
        public async Task<ActionResult<MessageDto>> SendMessage(SendMessageDto messageDto, long id)
        {
            IEnumerable<Claim> claims = JwtProvider.ParseClaimsFromJwt(Request.Headers.Authorization!);
            long userId = long.Parse(claims.Where(claim => claim.Type == "id").First().Value);

            MessageDto message = await _channelsService.SendMessageToChannel(id, messageDto, userId);
            return Ok(message);
        }

        [HttpGet("{id}/messages")]
        public ActionResult<List<MessageDto>> GetMessagesFromChannel(long id)
        {
            List<MessageDto> messages = _channelsService.GetMessagesFromChannel(id);
            if (messages is null)
                return BadRequest();

            return Ok(messages);
        }
    }
}
