using System.Text;
using Gump.Data;
using Gump.Data.Repositories;
using Gump.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();

var mongoDbConfig = builder.Configuration.GetSection("MongoDbConfig").Get<MongoDbConfig>();
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
var gitHubConfig = builder.Configuration.GetSection("GitHubConfig").Get<GitHubConfig>();
var pepper = builder.Configuration["Pepper"];

builder.Services.AddSingleton(mongoDbConfig);
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton(gitHubConfig);
builder.Services.AddSingleton(pepper);

builder.Services.AddSingleton<AdvertRepository>();
builder.Services.AddSingleton<BadgeRepository>();
builder.Services.AddSingleton<CategoryRepository>();
builder.Services.AddSingleton<ImageRepository>();
builder.Services.AddSingleton<PartnerRepository>();
builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<UserRepository>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddHttpClient();

builder.Services.AddCors(option =>
{
	option.AddPolicy("EnableCors", policy =>
	{
		policy
			.SetIsOriginAllowed(origin => true)
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials()
			.Build();
	});
});

builder.Services
	.AddAuthentication(option =>
	{
		option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(option =>
	{
		option.SaveToken = true;
		option.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.ASCII.GetBytes(jwtConfig.Secret)
			),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors("EnableCors");
}

app.UseCors("EnableGump");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
