using Conflict.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Conflict.Server.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private IConfiguration _config { get; }
		private readonly DataContext _dataContext;

		public AuthService(IConfiguration config, DataContext dataContext)
		{
			_config = config;
			_dataContext = dataContext;
		}

		public async Task<ActionResult<string>> Register(UserLoginDto userDto)
		{
			// Checks if username already exists
			User? dbUser = _dataContext.Users.SingleOrDefault(user => user.Name == userDto.Name);
			if (dbUser is not null)
				return new BadRequestObjectResult("Username already exists!");

			// Hash password and create a new user object
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
			User NewUser = new()
			{
				Name = userDto.Name,
				PasswordHash = passwordHash,
			};

			_dataContext.Add(NewUser);
			await _dataContext.SaveChangesAsync();

			return new OkObjectResult(NewUser.Name);
		}

		public ActionResult<string> Login(UserLoginDto userDto)
		{
			// Verify user info
			User dbUser;
			try
			{
				dbUser = _dataContext.Users.SingleOrDefault(user => user.Name == userDto.Name)!;
				if (string.IsNullOrEmpty(dbUser.Name) || !BCrypt.Net.BCrypt.Verify(userDto.Password, dbUser.PasswordHash))
				{
					return new BadRequestObjectResult("Invalid username or password!");
				}
			}
			catch (Exception)
			{
				return new BadRequestObjectResult("Invalid username or password!");
			}

			string token = GenerateToken(dbUser);
			return new OkObjectResult(token);
		}

		private string GenerateToken(User user)
		{
			List<Claim> claims = new()
			{
				new Claim("id", user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Name)
			};

			SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:JwtKey").Value!));

			SigningCredentials cred = new(key, SecurityAlgorithms.HmacSha512Signature);

			JwtSecurityToken token = new(
				claims: claims,
				expires: DateTime.Now.AddDays(999),
				signingCredentials: cred
				);

			string jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
