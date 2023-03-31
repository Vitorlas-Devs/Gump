using Gump.Data;
using Gump.Data.Models;
using Gump.Data.Repositories;
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

		var image = new ImageRepository(mongoDbConfig);
		image.Create(new ImageModel
		{
			Image = "Image1",
			OwnerId = null
		});
		image.Create(new ImageModel
		{
			Image = "Image2",
			OwnerId = 1
		});

		var user = new UserRepository(mongoDbConfig, "test");
		user.Create(new()
		{
			Username = "TestUser",
			Email = "user@test.com",
			Password = "secret",
		});
		user.Create(new()
		{
			Username = "Gordon Ramsay",
			Email = "gordon.ramsay@hellskitchen.com",
			Password = "burgir"
		});
		user.Create(new()
		{
			Username = "Moderator",
			Email = "moderator@gump.live",
			Password = "moderator001",
			IsModerator = true
		});

		var category = new CategoryRepository(mongoDbConfig);
		category.Create("Pasta");
		category.Create("Breakfast");

		var recipe = new RecipeRepository(mongoDbConfig);
		recipe.Create(new()
		{
			AuthorId = 1,
			Title = "Egyszerű rántotta",
			Language = "hu_HU",
			Serves = 1,
			Categories = new() { 1 },
			Tags = new()
			{
				"egyszerű",
				"rántotta",
				"tojás",
				"olajban"
			},
			Ingredients = new()
			{
				new()
				{
					Name = "Tojás",
					Value = 2,
					Volume = "db"
				},
				new()
				{
					Name = "só",
					Value = 1,
					Volume = "csipet"
				},
				new()
				{
					Name = "olaj",
					Value = 0,
					Volume = ""
				}
			},
			Steps = new()
			{
				"Öntsünk egy kevés olajat a serpenyőbe és gyújtsunk alá.",
				"Törjük bele a tojásokat a serpenyőbe.",
				"Sózzuk meg a tojásokat.",
				"Egy fakanállal, vagy bármilyen tetszőleges eszközzel kavargassuk a tojást addig, amíg már szinte mindenhol szárazra sült a tojás. Ízlés szerint hagyható szaftosabbra, vagy süthető szárazabbra.",
				"Miután elkészült, egyből szedjük ki egy tányérra, és már fogyasztható is."
			},
			IsOriginal = true,
			OriginalRecipeId = 0,
			IsPrivate = false,
			VisibleTo = new()
		});
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(mongoDbConfig.Name);
	}
}
