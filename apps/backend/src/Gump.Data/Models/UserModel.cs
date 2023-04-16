using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("users")]
public class UserModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("token")]
	public string Token { get; set; }

	[BsonElement("username")]
	public string Username { get; set; }

	[BsonElement("password")]
	public string Password { get; set; }

	[BsonElement("email")]
	public string Email { get; set; }

	[BsonElement("profilePicture")]
	public ulong ProfilePictureId { get; set; }

	[BsonElement("language")]
	public string Language { get; set; }

	[BsonElement("recipes")]
	public List<ulong> Recipes { get; set; }

	[BsonElement("likes")]
	public List<ulong> Likes { get; set; }

	[BsonElement("following")]
	public List<ulong> Following { get; set; }

	[BsonElement("followers")]
	public List<ulong> Followers { get; set; }

	[BsonElement("badges")]
	public List<ulong> Badges { get; set; }

	[BsonElement("isModerator")]
	public bool IsModerator { get; set; }

	public void AddBadge(ulong badgeId)
	{
		if (!Badges.Contains(badgeId))
		{
			Badges.Add(badgeId);
		}
	}
}
