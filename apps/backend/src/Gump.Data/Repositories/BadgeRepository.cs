using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class BadgeRepository : RepositoryBase<BadgeModel>
{
	private readonly string connectionString;
	private readonly string databaseName;

	private ImageRepository ImageRepository => new(connectionString, databaseName);
	private UserRepository UserRepository => new(connectionString, databaseName);

	public BadgeRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
	{
		this.connectionString = connectionString;
		this.databaseName = databaseName;
	}

	public BadgeModel Create(BadgeModel badge)
	{

		if (GetAll().Any(x => x.Name == badge.Name))
		{
			throw new ArgumentException($"Badge already exists with name {badge.Name}");
		}

		badge.Id = GetId();

		ValidateFields(badge, "Name", "Description", "ImageId");

		ImageRepository.GetById(badge.ImageId);

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
		var users = UserRepository
			.GetAll()
			.Where(u => u.Badges.Contains(id));

		foreach (var user in users)
		{
			user.Badges.Remove(id);
			UserRepository.Update(user);
		}

		var image = ImageRepository.GetById(badge.ImageId);
		ImageRepository.Delete(image.Id);

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
