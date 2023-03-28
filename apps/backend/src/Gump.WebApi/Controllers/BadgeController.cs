using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class BadgeController : ControllerBase
{
	private readonly BadgeRepository badgeRepository;
	private readonly UserRepository userRepository;

	public BadgeController(BadgeRepository badgeRepository, UserRepository userRepository)
	{
		this.badgeRepository = badgeRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	public IActionResult GetAllBadges() => this.Run(() =>
	{
		return Ok(badgeRepository.GetAll());
	});

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetBadge(ulong id) => this.Run(() =>
	{
		return Ok(badgeRepository.GetById(id));
	});

	[HttpPost("create")]
	public IActionResult CreateBadge([FromBody] CreateBadgeDto badge) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		BadgeModel newBadge = new()
		{
			Name = badge.Name,
			Description = badge.Description,
			ImageId = badge.ImageId
		};

		badgeRepository.Create(newBadge);

		return Created(nameof(newBadge), newBadge.Id);
	});

	[HttpPatch("update")]
	public IActionResult UpdateBadge([FromBody] BadgeModel badge) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		badgeRepository.Update(badge);

		return Ok();
	});

	[HttpDelete("delete/{id}")]
	public IActionResult DeleteBadge(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		badgeRepository.Delete(id);

		return Ok();
	});
}
