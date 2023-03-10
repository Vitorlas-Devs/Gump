using System.Reflection;
using Gump.Data.Repositories;

namespace Gump.Data.Tests.Repositories;

public class RepositoryTestsBase : IDisposable
{
	private readonly MongoClient mongoClient;
	private readonly string connectionString = "mongodb://localhost:27017";
	private readonly string databaseName = "GumpTest";
	public AdvertRepository AdvertRepository => new(connectionString, databaseName);
	public BadgeRepository BadgeRepository => new(connectionString, databaseName);
	public CategoryRepository CategoryRepository => new(connectionString, databaseName);
	public ImageRepository ImageRepository => new(connectionString, databaseName);
	public PartnerRepository PartnerRepository => new(connectionString, databaseName);
	public RecipeRepository RecipeRepository => new(connectionString, databaseName);
	public UserRepository UserRepository => new(connectionString, databaseName);

	// examples
	public AdvertModel Advert { get; set; } = new()
	{
		ImageId = 1,
		PartnerId = 1,
		Title = "Test",
	};
	public BadgeModel Badge { get; set; } = new()
	{
		Description = "Test",
		ImageId = 1,
		Name = "Test",
	};
	public CategoryModel Category { get; set; } = new()
	{
		Name = "Test",
	};
	public ImageModel Image { get; set; } = new()
	{
		Image = "Test",
	};
	public IngredientModel Ingredient { get; set; } = new()
	{
		LinkedRecipeId = 1,
		Name = "Test",
		Value = 1,
		Volume = "Test",
	};
	public PartnerModel Partner { get; set; } = new()
	{
		Name = "Test",
		ContactUrl = new Uri("http://www.g.hu"),
		ApiUrl = new Uri("http://www.g.hu"),
		Ads = new List<ulong> { 1 },
	};
	public RecipeModel Recipe { get; set; } = new()
	{
		AuthorId = 1,
		Categories = new List<ulong> { 1 },
		Forks = new List<ulong> { 1 },
		Ingredients = new List<IngredientModel> { new() { Name = "Test", Value = 1, Volume = "Test" } },
		IsArchived = false,
		IsOriginal = true,
		IsPrivate = false,
		Language = "Test",
		Likes = new List<ulong> { 1 },
		OriginalRecipeId = 1,
		ReferenceCount = 1,
		SaveCount = 1,
		Serves = 1,
		Steps = new List<string> { "Test" },
		Tags = new List<string> { "Test" },
		Title = "Test",
		VisibleTo = new List<ulong> { 1 },
	};
	public UserModel User { get; set; } = new()
	{
		Badges = new List<ulong> { 1 },
		Email = "Test",
		Followers = new List<ulong> { 1 },
		Following = new List<ulong> { 1 },
		IsModerator = false,
		Language = "Test",
		Password = "Test",
		ProfilePictureId = 1,
		Likes = new List<RecipeModel> { new() { Id = 1, Title = "Test" } },
		Recipes = new List<ulong> { 1 },
		Token = "Test",
		Username = "Test",
	};

	public RepositoryTestsBase()
	{
		mongoClient = new MongoClient(connectionString);
		mongoClient.GetDatabase(databaseName);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(databaseName);
	}

	public static T Get<T>() where T : IEntity
	{
		var repositoryTestsBase = new RepositoryTestsBase();
		var property = repositoryTestsBase.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.FirstOrDefault(x => x.PropertyType == typeof(T));

		var result = (T)Activator.CreateInstance(typeof(T));

		var value = property.GetValue(repositoryTestsBase);
		if (value is not null)
		{
			result = (T)value;
		}

		return result;
	}
}
