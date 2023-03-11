using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("partners")]
public class PartnerModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("contactUrl")]
	public Uri ContactUrl { get; set; }

	[BsonElement("apiUrl")]
	public Uri ApiUrl { get; set; }

	[BsonElement("ads")]
	public List<ulong> Ads { get; set; }
}
