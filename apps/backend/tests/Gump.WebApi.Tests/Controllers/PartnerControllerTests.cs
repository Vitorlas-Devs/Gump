using Gump.Data.Models;
using Gump.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class PartnerControllerTests : ControllerTestsBase
{
	[Fact]
	public void GetById_Works()
	{
		// Arrange
		var controller = new PartnerController(PartnerRepository, UserRepository);

		// Act
		var result = controller.GetPartner(1);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<ObjectResult>(result);
		Assert.NotNull(((ObjectResult)result).Value);
	}
}
