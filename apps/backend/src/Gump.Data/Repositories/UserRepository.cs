using Gump.Data.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace Gump.Data.Repositories;

public class UserRepository : RepositoryBase<UserModel>
{
	private readonly ImageRepository imageRepository;
	private readonly string pepper;

	public UserRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.imageRepository = new(mongoDbConfig);
	}

	public UserRepository(MongoDbConfig mongoDbConfig, string pepper) : base(mongoDbConfig)
	{
		this.imageRepository = new(mongoDbConfig);
		this.pepper = pepper;
	}

	public UserModel Create(UserModel user)
	{
		if (GetAll().Any(x => x.Username == user.Username))
		{
			throw new ArgumentException($"User already exists with username {user.Username}");
		}

		user.Id = GetId();

		ValidateFields(user, "Username", "Password", "Email");
		NullifyFields(user, "ProfilePictureId", "Language", "Recipes", "Likes", "Following", "Followers", "Badges", "IsModerator", "Verified");

		try
		{
			imageRepository.GetById(user.ProfilePictureId);
		}
		catch (Exception)
		{
			user.ProfilePictureId = 1; // A default pfp Id-je 1 lesz
		}

		user.Language = "en_US";
		user.Token = GenerateSalt();
		user.Password = ComputeHash(user.Password, user.Token, this.pepper, 10);

		try
		{
			Collection.InsertOne(user);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(user)}", ex);
		}

		return CopyExcept(user, "Password", "Token");
	}

	public UserModel Update(UserModel user)
	{
		ValidateFields(user, "Id", "Username", "Password", "Email");

		if (GetAll().Any(x => x.Username == user.Username && x.Id != user.Id))
		{
			throw new ArgumentException($"User already exists with username {user.Username}");
		}

		try
		{
			imageRepository.GetById(user.ProfilePictureId);
		}
		catch (Exception)
		{
			throw new ArgumentException($"Image with id {user.ProfilePictureId} does not exist");
		}

		if (user.Password != GetById(user.Id).Password)
		{
			if (string.IsNullOrWhiteSpace(this.pepper))
			{
				throw new ArgumentNullException(nameof(this.pepper), "Pepper is not set");
			}
			user.Token = GenerateSalt();
			user.Password = ComputeHash(user.Password, user.Token, this.pepper, 10);
		}

		return CopyExcept(user, "Password", "Token");
	}

	public void Delete(ulong id)
	{
		UserModel user = GetById(id);

		// other users' followers and following lists need to be updated
		foreach (ulong followerId in user.Followers)
		{
			UserModel follower = GetById(followerId);
			follower.Following.Remove(id);
			Update(follower);
		}

		foreach (ulong followingId in user.Following)
		{
			UserModel following = GetById(followingId);
			following.Followers.Remove(id);
			Update(following);
		}

		if (user.ProfilePictureId != 1)
		{
			imageRepository.Delete(user.ProfilePictureId);
		}

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(user)}", ex);
		}
	}

	public static string ComputeHash(string password, string salt, string pepper, int iterations)
	{
		if (iterations <= 0)
		{
			return password;
		}
		var passwordSaltPepper = $"{password}{salt}{pepper}";
		var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
		var byteHash = SHA256.HashData(byteValue);
		var hash = Convert.ToBase64String(byteHash);
		return ComputeHash(hash, salt, pepper, iterations - 1);
	}

	public static string GenerateSalt()
	{
		using var rng = RandomNumberGenerator.Create();
		var saltByte = new byte[16];
		rng.GetBytes(saltByte);
		return Convert.ToBase64String(saltByte);
	}
}
