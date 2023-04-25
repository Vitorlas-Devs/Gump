using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class BadgeControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetAllBadges_Works()
	{
		// Arrange
		var controller = new BadgeController(BadgeRepository, UserRepository);

		// Act
		var result = controller.GetAllBadges();

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void GetBadge_Works()
	{
		// Arrange
		var controller = new BadgeController(BadgeRepository, UserRepository);

		// Act
		var result = controller.GetBadge(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}
}