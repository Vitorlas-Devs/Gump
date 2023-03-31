using Gump.Data;
using MongoDB.Driver;

namespace Gump.WebApi.Tests.Controllers;

[Collection("ControllerTests")]
public class ControllerTestsBase : IDisposable
{
	private readonly MongoClient mongoClient;
	private readonly MongoDbConfig mongoDbConfig = new()
	{
		Name = "GumpWebApiTest",
		Host = "localhost",
		Port = 27017
	};

	public ControllerTestsBase()
	{
		this.mongoClient = new MongoClient(mongoDbConfig.ConnectionString);
		this.mongoClient.GetDatabase(mongoDbConfig.Name);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(mongoDbConfig.Name);
	}
}
