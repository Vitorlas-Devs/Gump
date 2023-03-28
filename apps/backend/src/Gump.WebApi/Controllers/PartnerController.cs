using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class PartnerController : ControllerBase
{
	private readonly PartnerRepository partnerRepository;
	private readonly UserRepository userRepository;

	public PartnerController(PartnerRepository partnerRepository, UserRepository userRepository)
	{
		this.partnerRepository = partnerRepository;
		this.userRepository = userRepository;
	}

	[HttpGet]
	public IActionResult GetAllPartners() => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		return Ok(partnerRepository.GetAll());
	});

	[HttpGet("{id}")]
	public IActionResult GetPartner(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		return Ok(partnerRepository.GetById(id));
	});

	[HttpPost("create")]
	public IActionResult CreatePartner([FromBody] CreatePartnerDto partner) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		PartnerModel newPartner = new()
		{
			Name = partner.Name,
			ContactUrl = partner.ContactUrl,
			ApiUrl = partner.ApiUrl
		};

		partnerRepository.Create(newPartner);

		return Created(nameof(newPartner), newPartner.Id);
	});

	[HttpPatch("update")]
	public IActionResult UpdatePartner([FromBody] UpdatePartnerDto partner) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		PartnerModel modifedPartner = partnerRepository.GetById(partner.Id);

		modifedPartner.Name = partner.Name;
		modifedPartner.ContactUrl = partner.ContactUrl;
		modifedPartner.ApiUrl = partner.ApiUrl;

		partnerRepository.Update(modifedPartner);

		return Ok();
	});

	[HttpDelete("delete/{id}")]
	public IActionResult DeletePartner(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		partnerRepository.Delete(id);

		return Ok();
	});
}
