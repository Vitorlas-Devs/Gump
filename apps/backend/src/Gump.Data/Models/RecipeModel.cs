using Gump.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Gump.Data.Models;

[CollectionName("recipes")]
public class RecipeModel : IEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.Int64)]
	public ulong Id { get; set; }

	[BsonElement("title")]
	public string Title { get; set; }

	[BsonElement("author")]
	public ulong AuthorId { get; set; }

	[BsonElement("image")]
	public ulong ImageId { get; set; }

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

	[BsonElement("viewCount")]
	public int ViewCount { get; set; }

	[BsonElement("saveCount")]
	public int SaveCount { get; set; }

	[BsonElement("likes")]
	public List<ulong> Likes { get; set; }

	[BsonElement("referenceCount")]
	public int ReferenceCount { get; set; }

	[BsonElement("isArchived")]
	public bool IsArchived { get; set; }

	[BsonElement("isOriginal")]
	public bool IsOriginal { get; set; }

	[BsonElement("originalRecipe")]
	public ulong OriginalRecipeId { get; set; }

	[BsonElement("isPrivate")]
	public bool IsPrivate { get; set; }

	[BsonElement("visibleTo")]
	public List<ulong> VisibleTo { get; set; }

	[BsonElement("forks")]
	public List<ulong> Forks { get; set; }
}
