using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class RecipeController : ControllerBase
{
	private readonly RecipeRepository recipeRepository;
	private readonly UserRepository userRepository;

	public RecipeController(
		RecipeRepository recipeRepository,
		UserRepository userRepository)
	{
		this.recipeRepository = recipeRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetRecipe(ulong id) => this.Run(() =>
	{
		RecipeModel recipe = recipeRepository.GetById(id);

		var user = User.Identity.IsAuthenticated ?
			userRepository.GetById(ulong.Parse(User.Identity.Name)) :
			new UserModel();

		if (recipe.IsPrivate &&
			recipe.AuthorId != user.Id &&
			!recipe.VisibleTo.Contains(user.Id) &&
			!user.IsModerator)
		{
			return Unauthorized();
		}

		recipe.ViewCount++;
		recipeRepository.Update(recipe);

		return Ok(new
		{
			id = recipe.Id,
			title = recipe.Title,
			author = recipe.AuthorId,
			language = recipe.Language,
			serves = recipe.Serves,
			categories = recipe.Categories,
			tags = recipe.Tags,
			ingredients = recipe.Ingredients,
			steps = recipe.Steps,
			saveCount = recipe.SaveCount,
			isLiked = recipe.Likes.Contains(user.Id),
			likeCount = recipe.Likes.Count,
			referenceCount = recipe.ReferenceCount,
			isArchived = recipe.IsArchived,
			isOriginal = recipe.IsOriginal,
			originalRecipe = recipe.OriginalRecipeId,
			isPrivate = recipe.IsPrivate,
			forks = recipe.Forks,
		});
	});

	[AllowAnonymous]
	[HttpGet("random")]
	public IActionResult GetRandomRecipe() => this.Run(() =>
	{
		if (!recipeRepository.GetAll().Any())
		{
			return NotFound();
		}

		object recipe;
		do
		{
			recipe = GetRecipe(recipeRepository.GetRandomId());
		} while (recipe is UnauthorizedResult || recipe is NotFoundResult);

		return Ok(recipe);
	});

	[AllowAnonymous]
	[HttpGet("search")]
	public IActionResult SearchRecipes([FromBody] SearchRecipeDto search) => this.Run(() =>
	{
		return Ok(recipeRepository.Search(
			search.SearchTerm,
			search.Limit,
			search.Offset,
			search.AuthorId,
			search.CategoryId
		));
	});
}
