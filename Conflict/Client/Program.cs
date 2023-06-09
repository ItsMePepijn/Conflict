using Conflict.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Conflict.Client.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddSingleton<ChannelStateProvider>();
builder.Services.AddSingleton<HubConnectionProvider>();
builder.Services.AddSingleton<AddChannelModalStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
