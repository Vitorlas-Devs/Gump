using MongoDB.Bson.Serialization.Attributes;

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
	public ulong LinkedRecipeId { get; set; }
}
