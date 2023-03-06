using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class ImageRepository : RepositoryBase<ImageModel>
{
	private readonly string connectionString;
	private readonly string databaseName;

	private UserRepository UserRepository => new(connectionString, databaseName);

	public ImageRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		this.connectionString = connectionString;
		this.databaseName = databaseName;
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
			throw new AggregateException("Error while creating image", ex);
		}

		return image;
	}

	public void Delete(ulong id)
	{
		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting image", ex);
		}
	}
}
