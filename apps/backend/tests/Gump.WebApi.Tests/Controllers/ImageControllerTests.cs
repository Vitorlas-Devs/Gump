using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class ImageControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetById_Works()
	{
		// Arrange
		var controller = new ImageController(ImageRepository, UserRepository);

		// Act
		var result = controller.GetImage(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}
}
