using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

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
	public IActionResult GetImage(ulong id) => this.Run(() =>
	{
		ImageModel image = imageRepository.GetById(id);

		ulong userId = 0;

		if (User.Identity.IsAuthenticated)
		{
			userId = ulong.Parse(User.Identity.Name);
		}

		UserModel user = userId == 0 ? new() : userRepository.GetById(userId);

		if (image.OwnerId.HasValue &&
			image.OwnerId.Value != userId &&
			!user.IsModerator)
		{
			return Unauthorized();
		}

		string[] m = image.Image.Split(',');
		string type = m[0].Split(':')[1].Split(';')[0];

		return File(Convert.FromBase64String(m[1]), type);
	});

	[HttpPost]
	public IActionResult CreateImage([FromBody] ImageDto imageDto) => this.Run(() =>
	{
		var user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		ImageModel image = new()
		{
			Image = imageDto.Image,
			OwnerId = imageDto.IsPrivate ? user.Id : null
		};

		this.imageRepository.Create(image);

		return Created(nameof(image), image.Id);
	});
}
