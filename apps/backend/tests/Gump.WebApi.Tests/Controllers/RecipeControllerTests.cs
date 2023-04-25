using Gump.Data.Models;
using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class RecipeControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetRecipe_Works()
	{
		// Arrange
		var controller = new RecipeController(RecipeRepository, UserRepository);

		// Act
		var result = controller.GetRecipe(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}

	[Fact]
	public void Create_Works()
	{
		// Arrange
		var controller = new RecipeController(RecipeRepository, UserRepository);

		// Act
		var result = controller.CreateRecipe(new CreateRecipeDto
		{
			Title = "Test",
			ImageId = 1,
			Language = "en_US",
			Serves = 1,
			Categories = new List<ulong> { 1 },
			Tags = new List<string> { "Test" },
			Ingredients = new List<IngredientModel>
			{
				new() { Name = "Test" }
			},
			Steps = new List<string> { "Test" },
			IsOriginal = true,
			OriginalRecipeId = 0,
			IsPrivate = false,
			VisibleTo = new List<ulong> { }
		});

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}

	[Fact]
	public void Update_Works()
	{
		// Arrange
		var controller = new RecipeController(RecipeRepository, UserRepository);

		// Act
		var result = controller.UpdateRecipe(new UpdateRecipeDto
		{
			Id = 1,
			ImageId = 1,
			Serves = 1,
			Categories = new List<ulong> { 1 },
			Tags = new List<string> { "Test" },
			Ingredients = new List<IngredientModel>
			{
				new() { Name = "Test" }
			},
			Steps = new List<string> { "Test" },
			IsPrivate = false,
		});

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}

	[Fact]
	public void Delete_Works()
	{
		// Arrange
		var controller = new RecipeController(RecipeRepository, UserRepository);

		// Act
		var result = controller.DeleteRecipe(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
	}
}
