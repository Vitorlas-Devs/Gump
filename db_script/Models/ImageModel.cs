using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("images")]
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
