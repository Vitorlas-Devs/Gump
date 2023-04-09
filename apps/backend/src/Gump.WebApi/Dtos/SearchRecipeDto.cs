using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class SearchRecipeDto
{
	[Required, JsonProperty("searchTerm")]
	public string SearchTerm { get; init; } = string.Empty;

	[Required, JsonProperty("limit")]
	public int Limit { get; init; } = 10;

	[Required, JsonProperty("offset")]
	public int Offset { get; init; }

	[Required, JsonProperty("author")]
	public ulong AuthorId { get; init; }

	[Required, JsonProperty("category")]
	public ulong CategoryId { get; init; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
