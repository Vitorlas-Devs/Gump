namespace Gump.Data.Tests.Repositories;

public class BaseRepositoryTests : IDisposable
{
	private readonly MongoClient mongoClient;
	private readonly IMongoDatabase database;

	public CategoryRepository Repository { get; }

	public BaseRepositoryTests()
	{
		const string connectionString = "mongodb://localhost:27017";
		const string databaseName = "GumpTest";
		this.mongoClient = new MongoClient(connectionString);
		this.database = this.mongoClient.GetDatabase(databaseName);
		Repository = new CategoryRepository(connectionString, databaseName);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		this.mongoClient.DropDatabase(this.database.DatabaseNamespace.DatabaseName);
	}
}
