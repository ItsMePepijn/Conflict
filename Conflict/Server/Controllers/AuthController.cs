using Microsoft.AspNetCore.Mvc;

namespace Conflict.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<string>> Register(UserLoginDto userDto)
		{
			ActionResult<string> result = await _authService.Register(userDto);

			return result;
		}

		[HttpPost("login")]
		public ActionResult<string> Login(UserLoginDto userDto)
		{
			ActionResult<string> result = _authService.Login(userDto);

			return result;
		}


	}
}
