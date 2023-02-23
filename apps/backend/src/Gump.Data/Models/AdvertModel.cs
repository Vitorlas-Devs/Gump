using Gump.Data.Repositories;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class AdvertModel : IEntity
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("partner")]
	public ulong PartnerId { get; set; }

	[BsonElement("title")]
	public string Title { get; set; }

	[BsonElement("imageUrl")]
	public Uri ImageUrl { get; set; }
}
