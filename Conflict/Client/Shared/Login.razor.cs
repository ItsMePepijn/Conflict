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
            if (result.StatusCode is not System.Net.HttpStatusCode.OK) return;

            var token = await result.Content.ReadAsStringAsync();

            // Put token in localstorage and refresh state
            await LocalStorage.SetItemAsync("token", token);
            await AuthStateProvider.GetAuthenticationStateAsync();
        }
    }
}
