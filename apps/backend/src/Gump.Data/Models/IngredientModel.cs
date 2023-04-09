using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Gump.Data.Models;

public class IngredientModel
{
	[BsonElement("name")]
	public string Name { get; set; }

	[BsonElement("value")]
	public ushort Value { get; set; }

	[BsonElement("volume")]
	public string Volume { get; set; }

	[BsonElement("linkedRecipe")]
	[JsonProperty("linkedRecipe")]
	public ulong LinkedRecipeId { get; set; }
}
