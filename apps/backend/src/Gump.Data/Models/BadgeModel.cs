using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("badges")]
public class BadgeModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("description")]
	public string Description { get; set; }

	[BsonElement("image")]
	public ulong ImageId { get; set; }
}
