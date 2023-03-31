using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using Newtonsoft.Json;

namespace Gump.Data.Models;

[CollectionName("adverts")]
public class AdvertModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("partner")]
	[JsonProperty("partner")]
	public ulong PartnerId { get; set; }

	[BsonElement("title")]
	public string Title { get; set; }

	[BsonElement("image")]
	[JsonProperty("image")]
	public ulong ImageId { get; set; }
}
