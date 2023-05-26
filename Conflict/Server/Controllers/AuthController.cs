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
		public ActionResult<string> Register(UserDto userDto)
		{
			string result = _authService.Register(userDto);

			return Ok(result);
		}

		[HttpPost("login")]
		public ActionResult<string> Login(UserDto userDto)
		{
			string? result = _authService.Login(userDto);

			if (result is null)
				return BadRequest("Invalid credentials!");

			return Ok(result);
		}


	}
}
