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

		CreateImages();
		CreateUsers();
		CreateCategories();
		CreateRecipes();
		CreateBadge();
		CreatePartner();
		CreateAdvert();
	}

	private void CreateImages()
	{
		var image = new ImageRepository(mongoDbConfig);
		image.Create(new ImageModel
		{
			Image = "data:image/png;base64,DefaultPfp",
			OwnerId = null
		});
		image.Create(new ImageModel
		{
			Image = "data:image/png;base64,Badge",
			OwnerId = null
		});
		image.Create(new ImageModel
		{
			Image = "data:image/png;base64,Advert",
			OwnerId = null
		});
		image.Create(new ImageModel
		{
			Image = "data:image/png;base64,PrivateImage",
			OwnerId = 1
		});
	}

	private void CreateUsers()
	{
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
	}

	private void CreateCategories()
	{
		var category = new CategoryRepository(mongoDbConfig);
		category.Create("Pasta");
		category.Create("Breakfast");
	}

	private void CreateRecipes()
	{
		var recipe = new RecipeRepository(mongoDbConfig);
		recipe.Create(new()
		{
			AuthorId = 1,
			Title = "Egyszerű rántotta",
			Language = "hu_HU",
			Serves = 1,
			Categories = new() { 2 },
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
					Name = "Só",
					Value = 1,
					Volume = "csipet"
				},
				new()
				{
					Name = "Olaj"
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
		recipe.Create(new()
		{
			AuthorId = 1,
			Title = "Főtt kockatészta",
			Language = "hu_HU",
			Serves = 4,
			Categories = new() { 1 },
			Tags = new()
			{
				"főtt",
				"kockatészta",
				"kocka",
				"tészta"
			},
			Ingredients = new()
			{
				new()
				{
					Name = "Kockatészta",
					Value = 250,
					Volume = "g"
				},
				new()
				{
					Name = "Só",
					Value = 2,
					Volume = "tk"
				},
				new()
				{
					Name = "Víz",
					Value = 2,
					Volume = "l"
				}
			},
			Steps = new()
			{
				"Öntsük bele a vizet egy kellően nagy fazékba.",
				"Tegyük bele a vízbe a sót.",
				"Gyújtsunk alá, és forrásig melegítsük a vizet.",
				"Szórjuk bele a tésztát a vízbe, majd vegyük lejebb a hőfokot, hogy csak halkan bugyogjon.",
				"5-10 percig főzzük.",
				"Amint elkészült, szűrjük le és már fogyasztható is."
			},
			IsOriginal = true,
			OriginalRecipeId = 0,
			IsPrivate = false,
			VisibleTo = new()
		});

		recipe.Create(new()
		{
			AuthorId = 1,
			Title = "Diós tészta",
			Language = "hu_HU",
			Serves = 4,
			Categories = new() { 1 },
			Tags = new()
			{
				"diós",
				"tészta",
				"dióval"
			},
			Ingredients = new()
			{
				new()
				{
					Name = "Főtt kockatészta",
					Value = 4,
					Volume = "adag",
					LinkedRecipeId = 2
				},
				new()
				{
					Name = "Darált dió"
				},
				new()
				{
					Name = "Porcukor"
				},
				new()
				{
					Name = "Olvasztott vaj"
				}
			},
			Steps = new()
			{
				"<@2>",
				"Egy adag főtt tésztát szedjünk ki egy tányérba.",
				"Szórjunk rá ízlés szerint diót, cukrot és öntsünk rá olvasztott vajat."
			},
			IsOriginal = true,
			OriginalRecipeId = 0,
			IsPrivate = false,
			VisibleTo = new()
		});
	}

	private void CreateBadge()
	{
		new BadgeRepository(mongoDbConfig).Create(new()
		{
			Name = "MasterChefBadge",
			Description = "MasterChefBadgeDescription",
			ImageId = 2,
		});
	}

	private void CreatePartner()
	{
		new PartnerRepository(mongoDbConfig).Create(new()
		{
			Name = "Tesco",
			ContactUrl = new("https://www.tesco.test/contact"),
			ApiUrl = new("https://www.tesco.test/gump/api")
		});
	}

	private void CreateAdvert()
	{
		new AdvertRepository(mongoDbConfig).Create(new()
		{
			PartnerId = 1,
			Title = "Cheap Beef at Tesco",
			ImageId = 3,
		});
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		mongoClient.DropDatabase(mongoDbConfig.Name);
	}
}
