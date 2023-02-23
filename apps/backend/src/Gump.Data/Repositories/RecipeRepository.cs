using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class RecipeRepository : RepositoryBase<RecipeModel>
{
	public RecipeRepository(string connectionString) : base(connectionString) { }

	public RecipeModel Create(RecipeModel recipe)
	{
		if (!GetAll().Any(x => x.AuthorId == recipe.AuthorId))
		{
			throw new ArgumentException($"Partner does not exists with id {recipe.AuthorId}");
		}

		recipe.Id = GetId();

		ValidateFields(recipe, "Title", "AuthorId", "Language", "Serves", "Categories", "Ingredients", "Steps", "OriginalRecipeId", "IsPrivate" );

		try
		{
			Collection.InsertOne(recipe);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating recipe", ex);
		}

		return recipe;

	}

	public List<RecipeModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
	}
}
