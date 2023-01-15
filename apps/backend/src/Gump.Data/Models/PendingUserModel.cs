using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class PendingUserModel
{
	[BsonId]
	public int Id { get; set; }

	[BsonElement("token")]
	public string Token { get; set; }

	[BsonElement("username")]
	public string Username { get; set; }

	[BsonElement("password")]
	public string Password { get; set; }

	[BsonElement("email")]
	public string Email { get; set; }
}
