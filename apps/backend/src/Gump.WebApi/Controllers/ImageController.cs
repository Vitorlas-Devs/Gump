using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gump.WebApi.Controllers
{
	[Authorize]
	[ApiController, Route("api/[controller]")]
	public class ImageController : ControllerBase
	{
		private readonly ImageRepository imageRepository;
		private readonly UserRepository userRepository;

		public ImageController(
			ImageRepository imageRepository,
			UserRepository userRepository)
		{
			this.imageRepository = imageRepository;
			this.userRepository = userRepository;
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public IActionResult GetImage(ulong id)
		{
			var image = imageRepository.GetById(id);

			if (image is null)
			{
				return NotFound();
			}

			ulong userId = ulong.Parse(User.Identity.Name);
			var user = userId == 0 ? new() : userRepository.GetById(userId);

			if (
				image.OwnerId.HasValue &&
				image.OwnerId.Value != userId &&
				!user.IsModerator)
			{
				return Unauthorized();
			}

			return Ok(image.Image);
		}
	}
}
