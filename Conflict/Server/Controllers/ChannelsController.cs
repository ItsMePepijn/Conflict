using Microsoft.AspNetCore.Mvc;
using Conflict.Server.Services.ChannelsService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Conflict.Shared;

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
        public async Task<ActionResult<Message>> SendMessage(MessageDto messageDto)
        {
            IEnumerable<Claim> claims = JwtProvider.ParseClaimsFromJwt(Request.Headers.Authorization!);
            User user = new()
            {
                Id = long.Parse(claims.Where(claim => claim.Type == "id").First().Value),
                Name = claims.Where(claim => claim.Type == ClaimTypes.Name).First().Value
            };
            Message message = await _channelsService.SendMessageToChannel(messageDto, user);
            return Ok(message);
        }
    }
}
