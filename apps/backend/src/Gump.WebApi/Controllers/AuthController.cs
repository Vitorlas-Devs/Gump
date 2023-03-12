using Gump.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gump.WebApi;

[ApiController, Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly JwtConfig jwtConfig;
	private readonly UserRepository userRepository;

	public AuthController(
		IOptions<JwtConfig> jwtConfig,
		UserRepository userRepository)
	{
		this.jwtConfig = jwtConfig.Value;
		this.userRepository = userRepository;
	}

	// [HttpPost("login")]
	// public async Task<ActionResult> Login([FromBody] LoginDto login)
	// {
	// 	var user = userRepository.GetAll().ToList()
	// 		.FirstOrDefault(x => x.Username == login.Username);
	// }
}
