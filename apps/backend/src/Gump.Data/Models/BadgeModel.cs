using Gump.Data.Repositories;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class BadgeModel : IEntity
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("description")]
	public string Description { get; set; }

	[BsonElement("image")]
	public ulong ImageId { get; set; }
}
