using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class CategoryControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetAllCategories_Works()
	{
		// Arrange
		var controller = new CategoryController(CategoryRepository, UserRepository);

		// Act
		var result = controller.GetAllCategories();

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void GetCategory_Works()
	{
		// Arrange
		var controller = new CategoryController(CategoryRepository, UserRepository);

		// Act
		var result = controller.GetCategory(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}
}
