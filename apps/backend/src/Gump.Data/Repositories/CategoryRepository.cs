using Gump.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class CategoryRepository : RepositoryBase<CategoryModel>
{
	private readonly RecipeRepository recipeRepository;

	public CategoryRepository(IOptions<MongoDbConfig> mongoDbConfig) : base(mongoDbConfig)
	{
		recipeRepository = new(mongoDbConfig);
	}

	public CategoryModel Create(string name)
	{
		if (GetAll().Any(x => x.Name == name))
		{
			throw new ArgumentException("Category already exists", nameof(name));
		}

		CategoryModel category = new CategoryModel
		{
			Id = GetId(),
			Name = name
		};

		ValidateFields(category, "Name");

		try
		{
			Collection.InsertOne(category);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(category)}", ex);
		}

		return category;
	}

	public void Update(CategoryModel category)
	{
		ValidateAllFields(category);

		if (GetAll().Any(x => x.Name == category.Name))
		{
			throw new ArgumentException($"Category {nameof(category.Name)} already exists");
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == category.Id, category);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while updating {nameof(category)}", ex);
		}
	}

	public void Delete(ulong id)
	{
		var category = GetById(id);

		ValidateFields(category, "Id");

		var recipes = recipeRepository
			.GetAll()
			.Where(r => r.Categories.Contains(id));

		foreach (var recipe in recipes)
		{
			recipe.Categories.Remove(id);
			recipeRepository.Update(recipe);
		}

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(category)}", ex);
		}
	}
}
