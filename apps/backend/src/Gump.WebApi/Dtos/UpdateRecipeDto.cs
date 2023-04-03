using Gump.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class UpdateRecipeDto
{
	[Required, JsonProperty("id")]
	public ulong Id { get; init; }

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

	[Required, JsonProperty("isPrivate")]
	public bool IsPrivate { get; init; }

	[Required, JsonProperty("visibleTo")]
	public List<ulong> VisibleTo { get; init; } = new();
}
