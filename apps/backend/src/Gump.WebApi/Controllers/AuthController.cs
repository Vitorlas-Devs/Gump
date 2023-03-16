using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gump.WebApi.Controllers
{
	[ApiController, Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly string pepper;
		private readonly JwtConfig jwtConfig;
		private readonly UserRepository userRepository;

		public AuthController(
			string pepper,
			JwtConfig jwtConfig,
			UserRepository userRepository)
		{
			this.pepper = pepper;
			this.jwtConfig = jwtConfig;
			this.userRepository = userRepository;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto loginDto) => this.Run(() =>
		{
			var user = userRepository.GetByName(loginDto.Username);

			if (user is null)
			{
				return Unauthorized();
			}

			var passwordHash = BCrypt.Net.BCrypt.HashPassword($"{loginDto.Password}{user.Token}{pepper}", 10);

			if (user.Password != passwordHash)
			{
				return Unauthorized();
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
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

			var token = tokenHandler.WriteToken(securityToken);

			return string.IsNullOrWhiteSpace(token)
				? Unauthorized()
				: Ok(new
				{
					token,
					user.Id
				});
		});
	}
}
