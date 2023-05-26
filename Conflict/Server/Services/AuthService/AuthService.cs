using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Conflict.Server.Services.AuthService
{
	public class AuthService : IAuthService
	{
		public static User user = new();
		public IConfiguration _config { get; }

		public AuthService(IConfiguration config)
		{
			_config = config;
		}
		public string Register(UserDto userDto)
		{
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

			user.Name = userDto.Name;
			user.PasswordHash = passwordHash;

			return user.Name;
		}

		public string? Login(UserDto userDto)
		{
			if (userDto.Name != user.Name || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
			{
				return null;
			}

			string token = GenerateToken(user);
			return token;
		}


		private string GenerateToken(User user)
		{
			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, user.Name)
			};

			SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:JwtKey").Value!));

			SigningCredentials cred = new(key, SecurityAlgorithms.HmacSha512Signature);

			JwtSecurityToken token = new(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: cred
				);

			string jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
