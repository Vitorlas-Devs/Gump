using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class AdvertController : ControllerBase
{
	private readonly AdvertRepository advertRepository;
	private readonly UserRepository userRepository;

	public AdvertController(AdvertRepository advertRepository, UserRepository userRepository)
	{
		this.advertRepository = advertRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	public IActionResult GetAllAdverts() => this.Run(() =>
	{
		return Ok(advertRepository.GetAll());
	});

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetAdvert(ulong id) => this.Run(() =>
	{
		return Ok(advertRepository.GetById(id));
	});

	[HttpPost("create")]
	public IActionResult CreateAdvert([FromBody] CreateAdvertDto advert) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		AdvertModel newAdvert = new()
		{
			PartnerId = advert.PartnerId,
			Title = advert.Title,
			ImageId = advert.ImageId
		};

		advertRepository.Create(newAdvert);

		return Created(nameof(newAdvert), newAdvert.Id);
	});

	[HttpPatch("update")]
	public IActionResult UpdateAdvert([FromBody] AdvertModel advert) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		advertRepository.Update(advert);

		return Ok();
	});

	[HttpDelete("delete/{id}")]
	public IActionResult DeleteAdvert(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		advertRepository.Delete(id);

		return Ok();
	});
}
