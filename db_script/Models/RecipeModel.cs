using Gump.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Gump.Data.Models;

public class RecipeModel
{
	[BsonId]
	public ulong Id { get; set; }

	[BsonElement("title")]
	public string Title { get; set; }

	[BsonElement("author")]
	public ulong AuthorId { get; set; }

	[BsonElement("langauge")]
	public string Language { get; set; }

	[BsonElement("serves")]
	public ushort Serves { get; set; }

	[BsonElement("categories")]
	public List<ulong> Categories { get; set; }

	[BsonElement("tags")]
	public List<string> Tags { get; set; }

	[BsonElement("ingredients")]
	public List<IngredientModel> Ingredients { get; set; }

	[BsonElement("steps")]
	public List<string> Steps { get; set; }

	[BsonElement("saveCount")]
	public int SaveCount { get; set; }

	[BsonElement("isArchived")]
	public bool IsArchived { get; set; }

	[BsonElement("isOriginal")]
	public bool IsOriginal { get; set; }

	[BsonElement("originalRecipe")]
	public ulong OriginalRecipeId { get; set; }

	[BsonElement("visibility")]
	public Visibility Visibility { get; set; }

	[BsonElement("visibleTo")]
	public List<ulong> VisibleTo { get; set; }

	[BsonElement("forks")]
	public List<ulong> Forks { get; set; }
}
