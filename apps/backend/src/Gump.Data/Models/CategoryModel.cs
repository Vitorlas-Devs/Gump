using Gump.Data.Repositories;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class CategoryModel : IEntity
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }
}
