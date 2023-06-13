global using Conflict.Shared.Models;
global using Conflict.Shared.Dto;
global using Conflict.Server.Services.AuthService;
using Conflict.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Conflict.Server.Data;
using Conflict.Server.Services.ChannelsService;
using Conflict.Server;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetSection("AppSettings:JwtKey").Value is null)
	throw new Exception("JwtKey is not set!");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Jwt token validation
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience = false,
		ValidateIssuer = false,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				builder.Configuration.GetSection("AppSettings:JwtKey").Value!))
	};
});

// Authentication support for swagger
builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.OperationFilter<SecurityRequirementsOperationFilter>();
});


// SignalR configuration
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChannelsService, ChannelsService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Database configuration
string? connectionString = builder.Configuration.GetConnectionString("PlanetScaleDbConnection") ?? throw new Exception("Db connection string is not set!");

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseResponseCompression();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.MapFallbackToFile("index.html");

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
