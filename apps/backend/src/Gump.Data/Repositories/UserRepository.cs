using Gump.Data.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Gump.Data.Repositories;

public class UserRepository : RepositoryBase<UserModel>
{
	public UserRepository(string connectionString) : base(connectionString) { }

	public UserModel Create(UserModel user)
	{
		if (GetAll().Any(x => x.Username == user.Username))
		{
			throw new ArgumentException($"User already exists with username {user.Username}");
		}

		user.Id = GetId();

		ValidateFields(user, "Username", "Password", "Email");
		NullifyFields(user, "PfpUrl", "Language", "Recipes", "Likes", "Following", "Followers", "Badges", "IsModerator", "Verified");

		user.PfpUrl = new Uri("https://cdn.discordapp.com/embed/avatars/0.png");
		user.Language = "en_US";

		user.Token = Guid.NewGuid().ToString();
		user.Password = HashPassword(user.Password, user.Token);

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

	public UserModel GetById(ulong id)
	{
		UserModel user = Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

		ValidateFields(user, "Id");

		// ⚠️ Password and Token are returned here ⚠️
		return user;
	}

	public List<UserModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
	}

	public UserModel Update(UserModel user)
	{
		ValidateFields(user, "Id", "Username", "Password", "Email");

		if (GetAll().Any(x => x.Username == user.Username && x.Id != user.Id))
		{
			throw new ArgumentException($"User already exists with username {user.Username}");
		}

		// token is not modifiable so we need to get the old one
		user.Token = GetById(user.Id).Token;

		// if password is modified, we need to hash it
		if (user.Password != GetById(user.Id).Password)
		{
			user.Password = HashPassword(user.Password, user.Token);
		}

		return CopyExcept(user, "Password", "Token");

	}

	public void Delete(ulong id)
	{
		UserModel user = GetById(id);
		if (user == null)
		{
			throw new ArgumentException($"User with id {id} does not exist");
		}

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

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting user", ex);
		}
	}

	private string HashPassword(string password, string token)
	{
		byte[] salt;
		using (var rng = RandomNumberGenerator.Create())
		{
			salt = new byte[16];
			rng.GetBytes(salt);
		}

		string pepper = "GumpIsAwesome";
		string passwordConcant = password + token + pepper;

		// hash password with salt and pepper
		var pbkdf2 = new Rfc2898DeriveBytes(passwordConcant, salt, 10000, HashAlgorithmName.SHA256);
		byte[] hash = pbkdf2.GetBytes(20);

		// combine salt and hash
		byte[] hashBytes = new byte[36];
		Array.Copy(salt, 0, hashBytes, 0, 16);
		Array.Copy(hash, 0, hashBytes, 16, 20);

		return Convert.ToBase64String(hashBytes);
	}
}
