using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
			ImageModel image;
			try
			{
				image = imageRepository.GetById(id);
			}
			catch (ArgumentNullException e)
			{
				return NotFound(e);
			}

			ulong userId = ulong.Parse(User.Identity.Name);

			UserModel user;

			try
			{
				user = userId == 0 ? new() : userRepository.GetById(userId);
			}
			catch (ArgumentNullException e)
			{
				return NotFound(e);
			}

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
