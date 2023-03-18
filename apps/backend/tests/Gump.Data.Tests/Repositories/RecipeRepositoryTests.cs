namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class RecipeRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public RecipeRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Theory]
	[InlineData("Category", "Recipe")]
	public void GetById_Works(string categoryName, string recipeTitle)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create(categoryName);

		RecipeModel recipe = Get<RecipeModel>();
		recipe.Title = recipeTitle;
		fixture.RecipeRepository.Create(recipe);

		// Act
		RecipeModel recipe2 = fixture.RecipeRepository.GetById(recipe.Id);

		// Assert
		Assert.Equal(recipe.Id, recipe2.Id);
		Assert.Equal(recipe.Title, recipe2.Title);
	}

	[Theory]
	[InlineData("Category")]
	public void Create_CannotCreateWithNonExistentAuthor(string categoryName)
	{
		// Arrange & Act
		fixture.CategoryRepository.Create(categoryName);

		RecipeModel recipe = Get<RecipeModel>();

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Theory]
	[InlineData(2)]
	public void Create_CannotCreateWithNonExistentVisibleTo(uint visibleToId)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		recipe.IsPrivate = true;

		// Act
		recipe.VisibleTo.Add(visibleToId);

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_CannotCreateWithNonExistentCategory()
	{
		// Arrange & Act
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		RecipeModel recipe = Get<RecipeModel>();

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_CannontCreateWithNonExistentLinkedRecipe()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		IngredientModel ingredient = new IngredientModel
		{
			LinkedRecipeId = 2,
			Name = "Test",
			Value = 1,
			Volume = "Test",
		};

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Ingredients.Add(ingredient);

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Theory]
	[InlineData(2)]
	public void Create_CannontCreateWithNonExistentOriginalRecipe(uint originalId)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.OriginalRecipeId = originalId;

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_CannotCreateWithEmptySteps()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Steps.Clear();

		//Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_CannotCreateWithEmptyIngredients()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Ingredients.Clear();

		//Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_CannotCreateWithEmptyTitle()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Title = string.Empty;

		//Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Theory]
	[InlineData("hun_HUN")]
	public void Create_CannotCreateWithInvalidLanguageFormat(string languageFormat)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Language = languageFormat;

		//Assert
		Assert.Throws<ArgumentException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Fact]
	public void Create_ValueAndVolumeIsBothReqired()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		IngredientModel ingredient = new IngredientModel()
		{
			Name = "Ingredient",
			Value = 1,
		};

		RecipeModel recipe = Get<RecipeModel>();

		// Act
		recipe.Ingredients.Add(ingredient);

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Create(recipe));
	}

	[Theory]
	[InlineData(3)]
	public void Update_CannotUpdateOriginalRecipeId(uint newOriginalId)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel originalRecipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(originalRecipe);

		RecipeModel recipe = Get<RecipeModel>();
		recipe.OriginalRecipeId = originalRecipe.Id;
		fixture.RecipeRepository.Create(recipe);

		// Act
		recipe.OriginalRecipeId = newOriginalId;

		//Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Update(recipe));
	}

	[Theory]
	[InlineData(2)]
	public void Update_CannotIncreaseReferenceCountMoreThanOne(int increase)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		// Act
		recipe.ReferenceCount += increase;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Update(recipe));
	}

	[Theory]
	[InlineData(2)]
	public void Update_CannotUpdateWithNonExistentFork(uint forkId)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		//Act
		recipe.Forks.Add(forkId);

		//Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Update(recipe));
	}

	[Fact]
	public void Delete_Works()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		user.Likes.Add(recipe);
		fixture.UserRepository.Update(user);

		// Act
		fixture.RecipeRepository.Delete(recipe.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.GetById(recipe.Id));

		Assert.DoesNotContain(recipe, fixture.UserRepository.GetById(user.Id).Likes);
	}

	[Fact]
	public void Delete_CannotDeleteNonExistent()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		// Act
		fixture.RecipeRepository.Delete(recipe.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.RecipeRepository.Delete(recipe.Id));
	}

	[Fact]
	public void Delete_RecipeWithForksCanOnlyBeArchived()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		// Act
		recipe.Forks.Add(1);
		fixture.RecipeRepository.Update(recipe);

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.RecipeRepository.Delete(recipe.Id));
	}
}
