namespace Conflict.Server.Services.AuthService
{
	public interface IAuthService
	{
		string Register(UserDto userDto);
		string? Login(UserDto userDto);
	}
}
