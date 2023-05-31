using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;


namespace Conflict.Client
{
	public class AuthStateProvider : AuthenticationStateProvider
	{
		private readonly ILocalStorageService _localStorage;
		private readonly HttpClient _http;

		public AuthStateProvider(ILocalStorageService localStorage, HttpClient http)
		{
			_localStorage = localStorage;
			_http = http;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			string token = await _localStorage.GetItemAsStringAsync("token");

            ClaimsIdentity identity = new();
			_http.DefaultRequestHeaders.Authorization = null;

			if (!string.IsNullOrEmpty(token))
			{
				identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
				_http.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(token.Replace("\"", ""));
			}

            ClaimsPrincipal user = new(identity);
            AuthenticationState state = new(user);

			NotifyAuthenticationStateChanged(Task.FromResult(state));

			return state;
		}

		public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
            string payload = jwt.Split('.')[1];
			byte[] jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;

			return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
		}

		private static byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}
