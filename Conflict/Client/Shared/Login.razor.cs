using Conflict.Shared.Dto;
using System.Net.Http.Json;

namespace Conflict.Client.Shared
{
	partial class Login
	{
        UserLoginDto User = new();
        string Error = string.Empty;
        string CanPress = "can-press";

        async Task HandleLogin()
        {
            Error = string.Empty;
            CanPress = string.Empty;
            var result = await Http.PostAsJsonAsync("api/auth/login", User);
            if (result.StatusCode is not System.Net.HttpStatusCode.OK)
            {
                Error = await result.Content.ReadAsStringAsync();
                User = new();
                CanPress = "can-press";
                return;
            }
            CanPress = "can-press";

            var token = await result.Content.ReadAsStringAsync();

            // Put token in localstorage and refresh state
            await LocalStorage.SetItemAsync("token", token);
            await AuthStateProvider.GetAuthenticationStateAsync();
        }
    }
}
