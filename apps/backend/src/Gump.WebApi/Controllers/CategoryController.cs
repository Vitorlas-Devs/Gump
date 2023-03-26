using Gump.Data.Models;
using Gump.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gump.WebApi.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class CategoryController : ControllerBase
{
	private readonly CategoryRepository categoryRepository;
	private readonly UserRepository userRepository;

	public CategoryController(CategoryRepository categoryRepository, UserRepository userRepository)
	{
		this.categoryRepository = categoryRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	public IActionResult GetAllCategories() => this.Run(() =>
	{
		return Ok(categoryRepository.GetAll());
	});

	[AllowAnonymous]
	[HttpGet("{id}")]
	public IActionResult GetCategory(ulong id) => this.Run(() =>
	{
		return Ok(categoryRepository.GetById(id));
	});

	[HttpPost("create")]
	public IActionResult CreateCategory([FromBody] CreateCategoryDto category) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		CategoryModel newCategory = new()
		{
			Name = category.Name
		};

		newCategory = categoryRepository.Create(newCategory.Name);

		return Created(nameof(newCategory), newCategory.Id);
	});

	[HttpPut("update")]
	public IActionResult UpdateCategory([FromBody] CategoryModel category) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		categoryRepository.Update(category);

		return Ok();
	});

	[HttpDelete("delete/{id}")]
	public IActionResult DeleteCategory(ulong id) => this.Run(() =>
	{
		UserModel user = userRepository.GetById(ulong.Parse(User.Identity.Name));

		if (!user.IsModerator)
		{
			return Unauthorized();
		}

		categoryRepository.Delete(id);

		return Ok();
	});

}
