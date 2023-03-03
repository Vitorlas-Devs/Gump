using Gump.Data.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Gump.Data.Repositories;

public class UserRepository : RepositoryBase<UserModel>
{
	private readonly ImageRepository imageRepository;
  
	public UserRepository(string connectionString, string databaseName) : base(connectionString, databaseName) { }
	{
		this.imageRepository = new(connectionString);
	}

	public UserModel Create(UserModel user, string pepper)
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

		string salt;
		using (var rng = RandomNumberGenerator.Create())
		{
			var saltByte = new byte[16];
			rng.GetBytes(saltByte);
			salt = Convert.ToBase64String(saltByte);
		}


		var password = HashPassword(user.Password, salt, pepper);
		user.Password = password;
		user.Token = salt;

		try
		{
			Collection.InsertOne(user);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating user", ex);
		}

		return CopyExcept(user, "Password", "Token");
	}

	public UserModel Update(UserModel user) => Update(user, null);
	public UserModel Update(UserModel user, string pepper)
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

		// token is not modifiable so we need to get the old one
		user.Token = GetById(user.Id).Token;

		// if password is modified, we need to hash it
		if (user.Password != GetById(user.Id).Password)
		{
			user.Password = HashPassword(user.Password, user.Token, pepper);
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
			throw new AggregateException("Error while deleting user", ex);
		}
	}

	private static string HashPassword(string password, string salt, string pepper)
	{
		// convert salt and pepper to byte arrays
		byte[] saltByte = Convert.FromBase64String(salt);
		byte[] pepperByte = Convert.FromBase64String(pepper);

		// hash password with salt
		var passwordSalty = new Rfc2898DeriveBytes(password, saltByte, 10000, HashAlgorithmName.SHA256);
		byte[] hash = passwordSalty.GetBytes(32);

		// hash password with pepper
		var passwordHashed = new Rfc2898DeriveBytes(hash, pepperByte, 10000, HashAlgorithmName.SHA256);

		// return the hashed password
		return Convert.ToBase64String(passwordHashed.GetBytes(32));
	}
}
