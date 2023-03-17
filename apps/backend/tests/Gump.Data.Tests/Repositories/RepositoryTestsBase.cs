using System.Reflection;

namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class RepositoryTestsBase : IDisposable
{
	private readonly MongoClient mongoClient;
	private readonly MongoDbConfig mongoDbConfig = new()
	{
		Name = "GumpTest",
		Host = "localhost",
		Port = 27017
	};
	public AdvertRepository AdvertRepository => new(mongoDbConfig);
	public BadgeRepository BadgeRepository => new(mongoDbConfig);
	public CategoryRepository CategoryRepository => new(mongoDbConfig);
	public ImageRepository ImageRepository => new(mongoDbConfig);
	public PartnerRepository PartnerRepository => new(mongoDbConfig);
	public RecipeRepository RecipeRepository => new(mongoDbConfig);
	public UserRepository UserRepository => new(mongoDbConfig, "test");

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
		Language = "hu_HU",
		Likes = new List<ulong> { 1 },
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
		Email = "test@test.test",
		IsModerator = false,
		Language = "Test",
		Password = "Test",
		Token = "Test",
		Username = "Test",
	};

	public RepositoryTestsBase()
	{
		mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
		mongoClient.GetDatabase(mongoDbConfig.Name);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(mongoDbConfig.Name);
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
