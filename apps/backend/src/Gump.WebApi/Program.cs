using System.Text;
using Gump.Data;
using Gump.Data.Repositories;
using Gump.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();

var jwtConfig = builder.Configuration.GetSection("JwtConfig");

builder.Services.Configure<MongoDbConfig>(
	builder.Configuration.GetSection("MongoDbConfig")
);
builder.Services.Configure<JwtConfig>(jwtConfig);

builder.Services.AddSingleton<AdvertRepository>();
builder.Services.AddSingleton<BadgeRepository>();
builder.Services.AddSingleton<CategoryRepository>();
builder.Services.AddSingleton<ImageRepository>();
builder.Services.AddSingleton<PartnerRepository>();
builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<UserRepository>();

builder.Services.AddControllers();

#if DEBUG
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
#endif

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
				Encoding.ASCII.GetBytes(jwtConfig.Get<JwtConfig>().Secret)
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
}
#if DEBUG
app.UseCors("EnableCors");
#endif

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
