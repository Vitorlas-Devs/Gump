namespace Gump.Data.Tests.Repositories;

public class BaseRepositoryTests : IDisposable
{
	public readonly CategoryRepository Repository;
	private readonly MongoClient _mongoClient;
	private readonly IMongoDatabase _database;
	protected readonly BaseRepositoryTests fixture;

	public BaseRepositoryTests()
	{
		var connectionString = "mongodb://localhost:27017";
		var databaseName = "GumpTest";
		_mongoClient = new MongoClient(connectionString);
		_database = _mongoClient.GetDatabase(databaseName);
		Repository = new CategoryRepository(connectionString, databaseName);
		fixture = this;
	}

	public void Dispose()
	{
		_mongoClient.DropDatabase(_database.DatabaseNamespace.DatabaseName);
	}
}
