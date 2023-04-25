using Gump.Data.Models;
using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class AdvertControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetById_Works()
	{
		// Arrange
		var controller = new AdvertController(AdvertRepository, UserRepository);

		// Act
		var result = controller.GetAdvert(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<OkObjectResult>(result);
		Assert.NotNull(((OkObjectResult)result).Value);
	}

	[Fact]
	public void Create_Works()
	{
		// Arrange
		var controller = new AdvertController(AdvertRepository, UserRepository);

		// Act
		var result = controller.CreateAdvert(new CreateAdvertDto
		{
			PartnerId = 1,
			Title = "Test",
			ImageId = 1
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
		var controller = new AdvertController(AdvertRepository, UserRepository);

		// Act
		var result = controller.UpdateAdvert(new AdvertModel
		{
			Id = 1,
			PartnerId = 1,
			Title = "Test",
			ImageId = 1
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
		var controller = new AdvertController(AdvertRepository, UserRepository);

		// Act
		var result = controller.DeleteAdvert(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}
}
