using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class AuthControllerTests : ControllerTestsBase
{
	[Fact]
	public void Login_Works()
	{
		// Arrange
		var controller = new AuthController(Pepper, JwtConfig, UserRepository);

		// Act
		var result = controller.Login(new()
		{
			Username = "TestUser",
			Password = "secret"
		});

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void Login_CannotLoginWithWrongCredentials()
	{
		// Arrange
		var controller = new AuthController(Pepper, JwtConfig, UserRepository);

		// Act
		var result = controller.Login(new()
		{
			Username = "TestUser",
			Password = "wrong"
		});

		// Assert
		Assert.NotNull(result);
		Assert.IsType<UnauthorizedResult>(result);
	}
}
