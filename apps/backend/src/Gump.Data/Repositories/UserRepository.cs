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

		Guid guid = Guid.NewGuid();
		user.Token = guid.ToString();
		byte[] guidBytes = guid.ToByteArray();

		byte[] salt;
		using (var rng = RandomNumberGenerator.Create())
		{
			salt = new byte[16];
			rng.GetBytes(salt);
		}

		string pepper = "GumpIsAwesome";
		string passwordWithPepper = user.Password + pepper;

		// hash password with salt and pepper
		var pbkdf2 = new Rfc2898DeriveBytes(passwordWithPepper, salt, 10000, HashAlgorithmName.SHA256);
		byte[] hash = pbkdf2.GetBytes(20);

		// combine salt and hash
		byte[] hashBytes = new byte[36];
		Array.Copy(salt, 0, hashBytes, 0, 16);
		Array.Copy(hash, 0, hashBytes, 16, 20);

		user.Password = Convert.ToBase64String(hashBytes);

		try
		{
			Collection.InsertOne(user);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating user", ex);
		}

		// return user without password, token and salt
		return CopyExcept(user, "Password", "Token");

	}

	public List<UserModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
	}

}
