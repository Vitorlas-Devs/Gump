using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class BadgeModel
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("description")]
	public string Description { get; set; }

	[BsonElement("imageUrl")]
	public Uri ImageUrl { get; set; }
}
