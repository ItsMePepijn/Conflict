namespace Conflict.Server.Services.AuthService
{
	public interface IAuthService
	{
		Task<string> Register(UserLoginDto userDto);
		string? Login(UserLoginDto userDto);
	}
}
