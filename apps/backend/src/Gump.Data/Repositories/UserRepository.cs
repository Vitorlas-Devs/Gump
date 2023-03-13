using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories
{
	public class UserRepository : RepositoryBase<UserModel>
	{
		private readonly MongoDbConfig mongoDbConfig;
		private ImageRepository ImageRepository => new(mongoDbConfig);
		private readonly string pepper;

		public UserRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
		{
			this.mongoDbConfig = mongoDbConfig;
		}

		public UserRepository(MongoDbConfig mongoDbConfig, string pepper) : base(mongoDbConfig)
		{
			this.mongoDbConfig = mongoDbConfig;
			this.pepper = pepper;
		}

		public UserModel GetByName(string username)
		{
			return Collection.AsQueryable().FirstOrDefault(x => x.Username == username);
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
				_ = ImageRepository.GetById(user.ProfilePictureId);
			}
			catch (Exception)
			{
				user.ProfilePictureId = 1; // A default pfp Id-je 1 lesz
			}

			user.Language = "en_US";
			user.Token = BCrypt.Net.BCrypt.GenerateSalt();
			user.Password = BCrypt.Net.BCrypt.HashPassword($"{user.Password}{user.Token}{pepper}", 10);

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
				_ = ImageRepository.GetById(user.ProfilePictureId);
			}
			catch (Exception)
			{
				throw new ArgumentException($"Image with id {user.ProfilePictureId} does not exist");
			}

			if (user.Password != GetById(user.Id).Password)
			{
				if (string.IsNullOrWhiteSpace(pepper))
				{
					throw new ArgumentNullException(nameof(pepper), "Pepper is not set");
				}
				user.Token = BCrypt.Net.BCrypt.GenerateSalt();
				user.Password = BCrypt.Net.BCrypt.HashPassword($"{user.Password}{user.Token}{pepper}", 10);
			}

			return CopyExcept(user, "Password", "Token");
		}

		public void Delete(ulong id)
		{
			var user = GetById(id);

			// other users' followers and following lists need to be updated
			foreach (var followerId in user.Followers)
			{
				var follower = GetById(followerId);
				_ = follower.Following.Remove(id);
				_ = Update(follower);
			}

			foreach (var followingId in user.Following)
			{
				var following = GetById(followingId);
				_ = following.Followers.Remove(id);
				_ = Update(following);
			}

			if (user.ProfilePictureId != 1)
			{
				ImageRepository.Delete(user.ProfilePictureId);
			}

			try
			{
				_ = Collection.DeleteOne(x => x.Id == id);
			}
			catch (MongoException ex)
			{
				throw new AggregateException($"Error while deleting {nameof(user)}", ex);
			}
		}
	}
}
