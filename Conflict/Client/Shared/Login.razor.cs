using Conflict.Shared.Dto;
using System.Net.Http.Json;

namespace Conflict.Client.Shared
{
	partial class Login
	{
        UserLoginDto User = new();

        async Task HandleLogin()
        {
            var result = await Http.PostAsJsonAsync("api/auth/login", User);
            var token = await result.Content.ReadAsStringAsync();
            await LocalStorage.SetItemAsync("token", token);
            await AuthStateProvider.GetAuthenticationStateAsync();
        }
    }
}
