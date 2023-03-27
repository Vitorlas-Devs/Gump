using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly RecipeRepository recipeRepository;
	private readonly UserRepository userRepository;

	public UserController(
		RecipeRepository recipeRepository,
		UserRepository userRepository)
	{
		this.recipeRepository = recipeRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetUser(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(id);
		foreach (var recipe in user.Recipes.Select(recipeRepository.GetById).ToList())
		{
			if (recipe.AuthorId != user.Id)
			{
				user.Recipes.Remove(recipe.Id);
			}
		}

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

	[HttpGet("me")]
	public IActionResult GetCurrentUser() => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		return Ok(new
		{
			id = user.Id,
			username = user.Username,
			profilePicture = user.ProfilePictureId,
			language = user.Language,
			recipes = user.Recipes,
			likes = user.Likes,
			following = user.Following,
			follower = user.Followers,
			badges = user.Badges,
			isModerator = user.IsModerator
		});
	});

	[AllowAnonymous]
	[HttpGet("search")]
	public IActionResult SearchUsers([FromBody] SearchUserDto search) => this.Run(() =>
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

	[HttpPatch("update")]
	public IActionResult UpdateUser([FromBody] UpdateUserDto update) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));
		UserModel modifiedUser = userRepository.GetById(update.Id);

		if (update.Id != user.Id && !user.IsModerator)
		{
			return Unauthorized();
		}

		modifiedUser.Username = update.Username;
		modifiedUser.Email = update.Email;
		modifiedUser.Password = update.Password;
		modifiedUser.ProfilePictureId = update.ProfilePictureId;
		modifiedUser.Language = update.Language;

		userRepository.Update(modifiedUser);

		return Ok();
	});

	[HttpDelete("delete/{id}")]
	public IActionResult DeleteUser(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));
		UserModel deletedUser = userRepository.GetById(id);

		if (id != user.Id && !user.IsModerator)
		{
			return Unauthorized();
		}

		userRepository.Delete(deletedUser.Id);

		return Ok();
	});
}
