using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class PartnerModel
{
	[BsonId]
	public int Id { get; set; }

	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("contactUrl")]
	public Uri ContactUrl { get; set; }

	[BsonElement("apiUrl")]
	public Uri ApiUrl { get; set; }

	[BsonElement("ads")]
	public List<ulong> Ads { get; set; }
}
