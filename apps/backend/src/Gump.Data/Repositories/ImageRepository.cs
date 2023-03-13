using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class ImageRepository : RepositoryBase<ImageModel>
{
	private readonly MongoDbConfig mongoDbConfig;
	private UserRepository UserRepository => new(mongoDbConfig);

	public ImageRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.mongoDbConfig = mongoDbConfig;
	}

	public ImageModel Create(ImageModel image)
	{
		image.Id = GetId();

		ValidateFields(image, "Image");

		if (image.OwnerId.HasValue)
		{
			UserRepository.GetById(image.OwnerId.Value);
		}

		try
		{
			Collection.InsertOne(image);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(image)}", ex);
		}

		return image;
	}

	public void Delete(ulong id)
	{
		var image = GetById(id);

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(image)}", ex);
		}
	}
}
