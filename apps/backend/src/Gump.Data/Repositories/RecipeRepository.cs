using System.Text.RegularExpressions;
using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public partial class RecipeRepository : RepositoryBase<RecipeModel>
{
	private readonly MongoDbConfig mongoDbConfig;
	private CategoryRepository CategoryRepository => new(mongoDbConfig);
	private ImageRepository ImageRepository => new(mongoDbConfig);
	private UserRepository UserRepository => new(mongoDbConfig);

	public RecipeRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.mongoDbConfig = mongoDbConfig;
	}

	public ulong GetRandomId() => GetRandomId(0);
	public ulong GetRandomId(ulong categoryId)
	{
		if (categoryId != 0 && !GetAll().Any(x => x.Categories.Contains(categoryId)))
		{
			throw new NotFoundException($"No recipes found with category {categoryId}");
		}

		ulong biggestId = Collection.AsQueryable().Max(x => x.Id);

		ulong randomId = 0;

		do
		{
			randomId = new Random().NextUInt64(biggestId + 1);
		}
		while (!Collection.AsQueryable().Any(
			x => x.Id == randomId &&
			(categoryId == 0 || x.Categories.Contains(categoryId))));

		return randomId;
	}

	public IEnumerable<RecipeModel> Search(
		string searchTerm, int limit, int offset, ulong authorId, ulong categoryId)
	{
		var a = GetAll();
		var b = a.Where(r => FilterLogic(r, authorId, categoryId));
		var c = b.OrderByDescending(CalculatePopularity);
		var d = c.GroupBy(r => CalculateScore(r, searchTerm));
		var e = d.OrderByDescending(g => g.Key);
		var f = e.SelectMany(g => g);
		var g = f.Skip(offset);
		var h = g.Take(limit);
		return h;

		// return GetAll()
		// 	.Where(r => FilterLogic(r, authorId, categoryId))
		// 	.OrderByDescending(CalculatePopularity)
		// 	.GroupBy(r => CalculateScore(r, searchTerm))
		// 	.OrderByDescending(g => g.Key)
		// 	.SelectMany(g => g)
		// 	.Skip(offset)
		// 	.Take(limit);
	}

	private static bool FilterLogic(
		RecipeModel recipe, ulong authorId, ulong categoryId)
	{
		if (authorId != 0 && recipe.AuthorId != authorId ||
			categoryId != 0 && !recipe.Categories.Contains(categoryId))
		{
			return false;
		}
		return true;
	}

	private static double CalculatePopularity(RecipeModel recipe)
	{
		double ratio = (double)recipe.Likes.Count / recipe.ViewCount;
		double popularity = (recipe.Likes.Count + recipe.ViewCount) * ratio;
		return popularity;
	}

	private static int CalculateScore(RecipeModel recipe, string searchTerm)
	{
		int score = 0;

		foreach (var term in searchTerm.Split(' '))
		{
			if (recipe.Tags.Any(t => t.ToLowerInvariant().Contains(term.ToLowerInvariant())))
			{
				score++;
			}
			if (recipe.Tags.Any(t => term.ToLowerInvariant().Contains(t.ToLowerInvariant())))
			{
				score++;
			}
		}

		return score;
	}

	public RecipeModel Create(RecipeModel recipe)
	{
		recipe.Id = GetId();

		ValidateFields(recipe, "Title", "AuthorId", "ImageId", "Language", "Serves", "Categories", "Ingredients", "Steps", "IsPrivate");
		NullifyFields(recipe, "Forks", "Likes", "SaveCount", "ReferenceCount");

		ImageRepository.GetById(recipe.ImageId);

		RecipeStuff(recipe);

		foreach (var ingredient in recipe.Ingredients)
		{
			// check if ingredient has linkedRecipe
			if (ingredient.LinkedRecipeId != 0)
			{
				// if it has, increase its referenceCount
				var linkedRecipe = GetById(ingredient.LinkedRecipeId);
				linkedRecipe.ReferenceCount++;
				Update(linkedRecipe);
			}
		}

		try
		{
			Collection.InsertOne(recipe);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(recipe)}", ex);
		}

		// if originalRecipeId is not 0, check if it exists
		// and add this recipe to the original recipe's forks list
		// and increase the referenceCount of the original recipe
		if (recipe.OriginalRecipeId != 0)
		{
			var originalRecipe = GetById(recipe.OriginalRecipeId);

			originalRecipe.Forks.Add(recipe.Id);
			originalRecipe.ReferenceCount++;
			Update(originalRecipe);
		}

		return recipe;
	}

	public RecipeModel Update(RecipeModel recipe)
	{
		GetById(recipe.Id);

		ValidateFields(recipe, "Title", "AuthorId", "ImageId", "Language", "Serves", "Categories", "Ingredients", "Steps", "OriginalRecipeId", "IsPrivate");

		ImageRepository.GetById(recipe.ImageId);

		RecipeStuff(recipe);

		// check if recipes in forks list exists
		if (recipe.Forks.Count > 0)
		{
			foreach (var forkRecipeId in recipe.Forks)
			{
				GetById(forkRecipeId);
			}
		}

		// originalRecipeId cannot be changed
		if (recipe.OriginalRecipeId != GetById(recipe.Id).OriginalRecipeId)
		{
			throw new RestrictedException($"{nameof(recipe.OriginalRecipeId)} cannot be changed");
		}

		// check if the referenceCount has changed
		var existingRecipe = GetById(recipe.Id);
		if (Math.Abs(recipe.ReferenceCount - existingRecipe.ReferenceCount) > 1)
		{
			throw new RestrictedException($"{nameof(recipe.ReferenceCount)} can only be increased/decreased by 1");
		}
		if (Math.Abs(recipe.SaveCount - existingRecipe.SaveCount) > 1)
		{
			throw new RestrictedException($"{nameof(recipe.SaveCount)} can only be increased/decreased by 1");
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == recipe.Id, recipe);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while updating {nameof(recipe)}", ex);
		}

		return recipe;
	}

	// checks that the create and update methods have in common
	private void RecipeStuff(RecipeModel recipe)
	{
		UserRepository.GetById(recipe.AuthorId);

		foreach (var ingredient in recipe.Ingredients)
		{
			// check if ingredient has linkedRecipe
			if (ingredient.LinkedRecipeId != 0)
			{
				// if it has, check if the linked recipe exists
				GetById(ingredient.LinkedRecipeId);
			}

			// if ingredient has value or volume, it must have both
			if ((ingredient.Value != 0 || !string.IsNullOrWhiteSpace
				(ingredient.Volume)) &&
				(ingredient.Value == 0 || string.IsNullOrWhiteSpace(ingredient.Volume)))
			{
				throw new RestrictedException($"{nameof(ingredient)} must have both value and volume or neither");
			}
		}

		// check if visibleTo users exist
		if (recipe.IsPrivate)
		{
			foreach (var userId in recipe.VisibleTo)
			{
				UserRepository.GetById(userId);
			}
		}

		// check if categories exist
		foreach (var categoryId in recipe.Categories)
		{
			CategoryRepository.GetById(categoryId);
		}

		// check language format
		if (!LanguageFormat().IsMatch(recipe.Language))
		{
			throw new ArgumentException($"{nameof(recipe.Language)} format is invalid");
		}

		// serves must be at least 1
		if (recipe.Serves < 1)
		{
			throw new ArgumentException($"{nameof(recipe.Serves)} must be at least 1");
		}
	}

	[GeneratedRegex("^[a-z]{2}_[A-Z]{2}$")]
	private static partial Regex LanguageFormat();

	public void Delete(ulong id)
	{
		var recipe = GetById(id);

		if (recipe.Forks.Count > 0)
		{
			throw new RestrictedException("Recipe can only be archived, because it has forks");
		}

		if (recipe.ReferenceCount != 0)
		{
			throw new RestrictedException("Recipe can only be archived, because it has references");
		}

		if (recipe.SaveCount != 0)
		{
			throw new RestrictedException("Recipe can only be archived, because it has saves");
		}

		var image = ImageRepository.GetById(recipe.ImageId);
		ImageRepository.Delete(image.Id);

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(recipe)}", ex);
		}

		// Get all the users who have liked this recipe
		var users = recipe.Likes.Select(UserRepository.GetById).ToList();
		foreach (var user in users)
		{
			user.Likes.Remove(id);

			UserRepository.Update(user);
		}
	}
}
