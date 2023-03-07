namespace Gump.Data.Tests.Repositories;

public class RepositoryTestsBase<T> : IDisposable
	where T : class
{
	private readonly MongoClient mongoClient;
	private readonly IMongoDatabase database;

	public T Repository { get; set; }

	public RepositoryTestsBase()
	{
		const string connectionString = "mongodb://localhost:27017";
		const string databaseName = "GumpTest";
		mongoClient = new MongoClient(connectionString);
		database = mongoClient.GetDatabase(databaseName);
		Repository = Activator.CreateInstance(typeof(T), connectionString, databaseName) as T;
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(database.DatabaseNamespace.DatabaseName);
	}
}
