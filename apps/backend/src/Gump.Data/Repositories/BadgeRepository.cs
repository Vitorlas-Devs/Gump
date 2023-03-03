using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class BadgeRepository : RepositoryBase<BadgeModel>
{
	private readonly ImageRepository imageRepository;
	private readonly UserRepository userRepository;

	public BadgeRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		this.imageRepository = new(connectionString, databaseName);
		this.userRepository = new(connectionString, databaseName);
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
		var badge = GetById(id);

		foreach (var user in userRepository.GetAll())
		{
			if (user.Badges.Contains(id))
			{
				user.Badges.Remove(id);
				userRepository.Update(user);
			}
		}

		var image = imageRepository.GetById(badge.ImageId);
		imageRepository.Delete(image.Id);

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
