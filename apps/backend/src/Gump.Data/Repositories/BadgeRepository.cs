using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class BadgeRepository : RepositoryBase<BadgeModel>
{
	private readonly MongoDbConfig mongoDbConfig;
	private ImageRepository ImageRepository => new(mongoDbConfig);
	private UserRepository UserRepository => new(mongoDbConfig);

	public BadgeRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.mongoDbConfig = mongoDbConfig;
	}

	public BadgeModel Create(BadgeModel badge)
	{
		if (GetAll().Any(x => x.Name == badge.Name))
		{
			throw new DuplicateException($"Badge already exists with name {badge.Name}");
		}

		badge.Id = GetId();

		ValidateFields(badge, "Name", "Description", "ImageId");

		var image = ImageRepository.GetById(badge.ImageId);

		if (image.OwnerId.HasValue)
		{
			throw new RestrictedException($"Private images cannot be used in badges");
		}

		try
		{
			Collection.InsertOne(badge);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(badge)}", ex);
		}

		return badge;
	}

	public BadgeModel Update(BadgeModel badge)
	{
		GetById(badge.Id);

		ValidateAllFields(badge);

		ImageRepository.GetById(badge.ImageId);

		if (GetAll().Any(x => x.Name == badge.Name && x.Id != badge.Id))
		{
			throw new DuplicateException($"Badge already exists with name {badge.Name}");
		}

		try
		{
			Collection.ReplaceOne(x => x.Id == badge.Id, badge);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while updating {nameof(badge)}", ex);
		}

		return badge;
	}

	public void Delete(ulong id)
	{
		var badge = GetById(id);
		var users = UserRepository
			.GetAll()
			.Where(u => u.Badges.Contains(id));

		foreach (var user in users)
		{
			user.Badges.Remove(id);
			UserRepository.Update(user);
		}

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(badge)}", ex);
		}
	}
}
