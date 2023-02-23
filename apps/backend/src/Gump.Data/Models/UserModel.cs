using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class UserModel
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("token")]
	public string Token { get; set; }

	[BsonElement("username")]
	public string Username { get; set; }

	[BsonElement("password")]
	public string Password { get; set; }

	[BsonElement("email")]
	public string Email { get; set; }

	[BsonElement("pfpUrl")]
	public Uri PfpUrl { get; set; }

	[BsonElement("language")]
	public string Language { get; set; }

	[BsonElement("recipes")]
	public List<ulong> Recipes { get; set; }

	[BsonElement("likes")]
	public List<RecipeModel> Likes { get; set; }

	[BsonElement("following")]
	public List<ulong> Following { get; set; }

	[BsonElement("followers")]
	public List<ulong> Followers { get; set; }

	[BsonElement("badges")]
	public List<ulong> Badges { get; set; }

	[BsonElement("isModerator")]
	public bool IsModerator { get; set; }
}
