using Gump.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class CreateRecipeDto
{
	[Required, JsonProperty("title")]
	public string Title { get; init; } = string.Empty;

	[Required, JsonProperty("image")]
	public ulong ImageId { get; init; }

	[Required, JsonProperty("language")]
	public string Language { get; init; } = string.Empty;

	[Required, JsonProperty("serves")]
	public ushort Serves { get; init; }

	[Required, JsonProperty("categories")]
	public List<ulong> Categories { get; init; } = new();

	[Required, JsonProperty("tags")]
	public List<string> Tags { get; init; } = new();

	[Required, JsonProperty("ingredients")]
	public List<IngredientModel> Ingredients { get; init; } = new();

	[Required, JsonProperty("steps")]
	public List<string> Steps { get; init; } = new();

	[Required, JsonProperty("isOriginal")]
	public bool IsOriginal { get; init; } = true;

	[Required, JsonProperty("originalRecipe")]
	public ulong OriginalRecipeId { get; init; }

	[Required, JsonProperty("isPrivate")]
	public bool IsPrivate { get; init; }

	[Required, JsonProperty("visibleTo")]
	public List<ulong> VisibleTo { get; init; } = new();
}
