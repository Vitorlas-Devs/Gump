using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class UserControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetUser_Works()
	{
		// Arrange
		var controller = new UserController(RecipeRepository, UserRepository);

		// Act
		var result = controller.GetUser(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void SearchUsers_Works()
	{
		// Arrange
		var controller = new UserController(RecipeRepository, UserRepository);

		// Act
		var result = controller.SearchUsers("test");

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void CreateUser_Works()
	{
		// Arrange
		var controller = new UserController(RecipeRepository, UserRepository);

		// Act
		var result = controller.CreateUser(new()
		{
			Username = "test",
			Email = "test@test.test",
			Password = "test"
		});

		// Assert
		Assert.NotNull(result);
		Assert.IsType<CreatedResult>(result);
		Assert.NotNull(((CreatedResult)result).Value);
	}
}
