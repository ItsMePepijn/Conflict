﻿using Conflict.Server.Data;
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

		public async Task<string> Register(UserDto userDto)
		{
			// Checks if username already exists
			User? dbUser = _dataContext.Users.SingleOrDefault(user => user.Name == userDto.Name);
			if (dbUser is not null)
				return "Username already exists!";

			// Hash password and create a new user object
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
			User NewUser = new()
			{
				Name = userDto.Name,
				PasswordHash = passwordHash,
			};

			_dataContext.Add(NewUser);
			await _dataContext.SaveChangesAsync();

			return NewUser.Name;
		}

		public string? Login(UserDto userDto)
		{
			// Verify user info
			User dbUser = _dataContext.Users.SingleOrDefault(user => user.Name == userDto.Name)!;
			if (dbUser.Name != userDto.Name || !BCrypt.Net.BCrypt.Verify(userDto.Password, dbUser.PasswordHash))
			{
				return null;
			}

			string token = GenerateToken(dbUser);
			return token;
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
				expires: DateTime.Now.AddDays(1),
				signingCredentials: cred
				);

			string jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}