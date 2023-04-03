using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

[ApiController, Route("api/[controller]")]
public class GitHubController : ControllerBase
{
	private readonly HttpClient httpClient;
	private readonly GitHubConfig configuration;

	public GitHubController(HttpClient httpClient, GitHubConfig configuration)
	{
		this.httpClient = httpClient;
		this.configuration = configuration;
	}

	[AllowAnonymous]
	[HttpGet("access_token")]
	public Task<IActionResult> GetAccessToken(string code) => this.Run(async () =>
	{
		var clientId = configuration.ClientId;
		var clientSecret = configuration.ClientSecret;

		var authUrl = $"https://github.com/login/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&code={code}";

		var response = await httpClient.PostAsync(authUrl, null);

		var data = await response.Content.ReadAsStringAsync();

		return Ok(data);
	});
}
