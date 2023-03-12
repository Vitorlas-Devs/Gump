using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gump.Data.Helpers;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gump.WebApi;

[ApiController, Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly string pepper;
	private readonly JwtConfig jwtConfig;
	private readonly UserRepository userRepository;

	public AuthController(
		string pepper,
		IOptions<JwtConfig> jwtConfig,
		UserRepository userRepository)
	{
		this.pepper = pepper;
		this.jwtConfig = jwtConfig.Value;
		this.userRepository = userRepository;
	}

	[HttpPost("login")]
	public IActionResult Login([FromBody] LoginDto login)
	{
		var user = userRepository.GetByName(login.Username);

		if (user is null)
		{
			return Unauthorized();
		}

		var passwordHash = HashHelper.ComputeHash(login.Password, user.Token, this.pepper, 10);

		if (user.Password != passwordHash)
		{
			return Unauthorized();
		}

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.Id.ToString())
			}),
			Expires = DateTime.UtcNow.AddMinutes(jwtConfig.Expiration),
			SigningCredentials = new(
				new SymmetricSecurityKey(
					Encoding.ASCII.GetBytes(jwtConfig.Secret)
				),
				SecurityAlgorithms.HmacSha256Signature
			)
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);

		string token = tokenHandler.WriteToken(securityToken);

		if (string.IsNullOrWhiteSpace(token))
			return Unauthorized();

		return Ok(new
		{
			token,
			user.Id
		});
	}
}
