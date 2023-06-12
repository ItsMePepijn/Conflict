using Microsoft.AspNetCore.Mvc;

namespace Conflict.Server.Services.AuthService
{
	public interface IAuthService
	{
		Task<ActionResult<string>> Register(UserLoginDto userDto);
		ActionResult<string> Login(UserLoginDto userDto);
	}
}
