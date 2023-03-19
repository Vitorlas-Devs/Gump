using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly UserRepository userRepository;

	public UserController(
		UserRepository userRepository)
	{
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetUser(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(id);

		return Ok(new
		{
			id = user.Id,
			username = user.Username,
			profilePicture = user.ProfilePictureId,
			recipes = user.Recipes,
			followingCount = user.Following.Count,
			followerCount = user.Followers.Count,
			badges = user.Badges
		});
	});
}
