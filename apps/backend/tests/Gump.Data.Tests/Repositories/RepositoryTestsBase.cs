namespace Gump.Data.Tests.Repositories;

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
	public UserRepository UserRepository => new(mongoDbConfig);


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
}
