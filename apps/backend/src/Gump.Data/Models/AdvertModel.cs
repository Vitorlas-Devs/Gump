using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("adverts")]
public class AdvertModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("partner")]
	public ulong PartnerId { get; set; }

	[BsonElement("title")]
	public string Title { get; set; }

	[BsonElement("image")]
	public ulong ImageId { get; set; }
}
