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
}
