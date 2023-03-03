using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class BadgeRepository : RepositoryBase<BadgeModel>
{
	private readonly ImageRepository imageRepository;

	public BadgeRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		this.imageRepository = new(connectionString, databaseName);
	}

	public BadgeModel Create(BadgeModel badge)
	{

		if (GetAll().Any(x => x.Name == badge.Name))
		{
			throw new ArgumentException($"Badge already exists with name {badge.Name}");
		}

		badge.Id = GetId();

		ValidateFields(badge, "Name", "Description", "ImageId");

		imageRepository.GetById(badge.ImageId);

		try
		{
			Collection.InsertOne(badge);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating badge", ex);
		}

		return badge;
	}

	public BadgeModel Update(BadgeModel badge)
	{
		ValidateAllFields(badge);

		if (GetAll().Any(x => x.Name == badge.Name && x.Id != badge.Id))
		{
			throw new ArgumentException($"Badge already exists with name {badge.Name}");
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == badge.Id, badge);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while updating badge", ex);
		}

		return badge;
	}

	public void Delete(ulong id)
	{
		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting badge", ex);
		}
	}
}
