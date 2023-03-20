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

	[AllowAnonymous]
	[HttpGet("search")]
	public IActionResult SearchUsers([FromBody] SearchDto search) => this.Run(() =>
	{
		var users = userRepository.Search(
			search.SearchTerm,
			search.Limit > 100 ? 100 : search.Limit
		);

		return Ok(users.Select(user => new
		{
			id = user.Id,
			username = user.Username,
			profilePicture = user.ProfilePictureId,
		}));
	});

	[HttpPatch("follow/{id}")]
	public IActionResult FollowUser(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(id);
		UserModel currentUser = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (user.Followers.Contains(currentUser.Id))
		{
			user.Followers.Remove(currentUser.Id);
			currentUser.Following.Remove(user.Id);
			userRepository.Update(user);
			userRepository.Update(currentUser);

			return Ok("unfollowed");
		}

		user.Followers.Add(currentUser.Id);
		currentUser.Following.Add(user.Id);
		userRepository.Update(user);
		userRepository.Update(currentUser);

		return Ok("followed");
	});

	[AllowAnonymous]
	[HttpPost("create")]
	public IActionResult CreateUser([FromBody] RegisterDto register) => this.Run(() =>
	{
		UserModel user = new()
		{
			Username = register.Username,
			Email = register.Email,
			Password = register.Password,
		};

		this.userRepository.Create(user);

		return Created(nameof(user), user.Id);
	});
}
