using Gump.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class BadgeRepository : RepositoryBase<BadgeModel>
{
	private readonly ImageRepository imageRepository;
	private readonly UserRepository userRepository;

	public BadgeRepository(IOptions<MongoDbConfig> mongoDbConfig) : base(mongoDbConfig)
	{
		imageRepository = new(mongoDbConfig);
		userRepository = new(mongoDbConfig);
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
			throw new AggregateException($"Error while creating {nameof(badge)}", ex);
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
			throw new AggregateException($"Error while updating {nameof(badge)}", ex);
		}

		return badge;
	}

	public void Delete(ulong id)
	{
		var badge = GetById(id);
		var users = userRepository
			.GetAll()
			.Where(u => u.Badges.Contains(id));

		foreach (var user in users)
		{
			user.Badges.Remove(id);
			userRepository.Update(user);
		}

		var image = imageRepository.GetById(badge.ImageId);
		imageRepository.Delete(image.Id);

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
