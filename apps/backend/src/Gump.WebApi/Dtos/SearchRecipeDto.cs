using Newtonsoft.Json;

namespace Gump.WebApi;

public class SearchRecipeDto
{
	[JsonProperty("searchTerm")]
	public string SearchTerm { get; init; } = string.Empty;

	[JsonProperty("sort")]
	public string Sort { get; init; } = string.Empty;

	[JsonProperty("limit")]
	public int Limit { get; init; } = 10;

	[JsonProperty("offset")]
	public int Offset { get; init; }

	[JsonProperty("author")]
	public ulong AuthorId { get; init; }

	[JsonProperty("category")]
	public ulong CategoryId { get; init; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
