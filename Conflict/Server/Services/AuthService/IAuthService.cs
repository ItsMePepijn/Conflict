namespace Conflict.Server.Services.AuthService
{
	public interface IAuthService
	{
		Task<string> Register(UserDto userDto);
		string? Login(UserDto userDto);
	}
}
