using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class ImageModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("image")]
	public string Image { get; set; }

	[BsonElement("owner")]
	public ulong? OwnerId { get; set; }
}
