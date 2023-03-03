using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class ImageRepository : RepositoryBase<ImageModel>
{
	private readonly UserRepository userRepository;

	public ImageRepository(string connectionString) : base(connectionString)
	{
		this.userRepository = new(connectionString);
	}

	public ImageModel Create(ImageModel image)
	{
		image.Id = GetId();

		ValidateFields(image, "Image");

		if (image.OwnerId.HasValue)
		{
			userRepository.GetById(image.OwnerId.Value);
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
