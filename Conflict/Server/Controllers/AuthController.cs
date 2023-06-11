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
			var result = await _authService.Register(userDto);

			// TODO: fix always returning Ok()
			return Ok(result);
		}

		[HttpPost("login")]
		public ActionResult<string> Login(UserLoginDto userDto)
		{
			string? result = _authService.Login(userDto);

			if (result is null)
				return BadRequest("Invalid credentials!");

			return Ok(result);
		}


	}
}
