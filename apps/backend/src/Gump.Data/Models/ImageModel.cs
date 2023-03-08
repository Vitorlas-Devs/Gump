using Gump.Data.Repositories;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class ImageModel : IEntity
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("image")]
	public string Image { get; set; }

	[BsonElement("owner")]
	public ulong? OwnerId { get; set; }
}
