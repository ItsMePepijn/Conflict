using Conflict.Shared.Models;
using Conflict.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Conflict.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public static User user = new();

		public IConfiguration _config { get; }

		public AuthController(IConfiguration config)
        {
			_config = config;
		}

        [HttpPost("register")]
		public async Task<ActionResult<User>> Register(UserDto userDto)
		{
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

			user.Name = userDto.Name;
			user.PasswordHash = passwordHash;

			return Ok(user.Name);
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> Login(UserDto userDto)
		{
			if (userDto.Name != user.Name || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
			{
				return BadRequest("Invalid credentials!");
			}

			string token = GenerateToken(user);
			return Ok(token);
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
