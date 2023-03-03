using System.Text.RegularExpressions;
using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class RecipeRepository : RepositoryBase<RecipeModel>
{
	private readonly UserRepository userRepository;
	private readonly CategoryRepository categoryRepository;

	public RecipeRepository(string connectionString) : base(connectionString)
	{
		userRepository = new UserRepository(connectionString);
		categoryRepository = new CategoryRepository(connectionString);
	}

	public RecipeModel Create(RecipeModel recipe)
	{
		recipe.Id = GetId();

		ValidateFields(recipe, "Title", "AuthorId", "Language", "Serves", "Categories", "Ingredients", "Steps", "OriginalRecipeId", "IsPrivate");
		NullifyFields(recipe, "Forks", "Likes", "SaveCount", "ReferenceCount");

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

	public RecipeModel Update(RecipeModel recipe)
	{
		ValidateFields(recipe, "Title", "AuthorId", "Language", "Serves", "Categories", "Ingredients", "Steps", "OriginalRecipeId", "IsPrivate");

		RecipeStuff(recipe);

		// check if recipes in forks list exists
		if (recipe.Forks.Count > 0)
		{
			foreach (var forkRecipeId in recipe.Forks)
			{
				GetById(forkRecipeId);
			}
		}

		// check if recipe in originalRecipeId exists
		if (recipe.OriginalRecipeId != 0)
		{
			GetById(recipe.OriginalRecipeId);
		}

		// originalRecipeId cannot be changed
		if (recipe.OriginalRecipeId != GetById(recipe.Id).OriginalRecipeId)
		{
			throw new ArgumentException($"OriginalRecipeId cannot be changed");
		}

		// check if the referenceCount has changed
		var existingRecipe = GetById(recipe.Id);
		if (recipe.ReferenceCount != existingRecipe.ReferenceCount)
		{
			// if yes determine if it has increased or decreased and update the referenceCount of the linked recipe
			var newReferenceCount = recipe.ReferenceCount;
			if (recipe.ReferenceCount > existingRecipe.ReferenceCount)
			{
				newReferenceCount = existingRecipe.ReferenceCount + (recipe.ReferenceCount - existingRecipe.ReferenceCount);
			}
			else
			{
				newReferenceCount = existingRecipe.ReferenceCount - (existingRecipe.ReferenceCount - recipe.ReferenceCount);
			}
			recipe.ReferenceCount = newReferenceCount;
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == recipe.Id, recipe);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while updating recipe", ex);
		}

		return recipe;
	}

	// checks that the create and update methods have in common
	private void RecipeStuff(RecipeModel recipe)
	{
		userRepository.GetById(recipe.AuthorId);

		foreach (var ingredient in recipe.Ingredients)
		{
			// check if ingredient has linkedRecipe
			if (ingredient.LinkedRecipeId != 0)
			{
				// if it has, check if the linked recipe exists
				var linkedRecipe = GetById(ingredient.LinkedRecipeId);

			}

			// if ingredient has value or volume, it must have both
			if (ingredient.Value != 0 || ingredient.Volume != null)
			{
				if (ingredient.Value == 0 || ingredient.Volume == null)
				{
					throw new ArgumentException($"Ingredient must have both value and volume");
				}
			}
		}

		// check if visibleTo users exist
		foreach (var userId in recipe.VisibleTo)
		{
			userRepository.GetById(userId);
		}

		// check if categories exist
		foreach (var categoryId in recipe.Categories)
		{
			categoryRepository.GetById(categoryId);
		}

		// check language format
		if (!Regex.IsMatch(recipe.Language, "^[a-z]{2}_[A-Z]{2}$"))
		{
			throw new ArgumentException($"Language format is invalid");
		}

		// serves must be at least 1
		if (recipe.Serves < 1)
		{
			throw new ArgumentException($"Serves must be at least 1");
		}
	}

	public void Delete(ulong id)
	{
		var recipe = GetById(id);


		if (recipe.Forks.Count > 0)
			throw new ArgumentException($"Recipe can only be archived, because it has forks");

		if (recipe.ReferenceCount != 0)
			throw new ArgumentException($"Recipe can only be archived, because it has references");

		if (recipe.SaveCount != 0)
			throw new ArgumentException($"Recipe can only be archived, because it has saves");

		// Get all the users who have liked this recipe
		var users = recipe.Likes.Select(x => userRepository.GetById(x)).ToList();

		// For each user, remove the recipe from their list of liked recipes
		foreach (var user in users)
		{
			user.Likes.Remove(recipe);
			userRepository.Update(user);
		}

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting recipe", ex);
		}
	}
}
