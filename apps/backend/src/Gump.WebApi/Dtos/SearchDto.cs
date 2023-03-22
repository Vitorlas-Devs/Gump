using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class SearchDto
{
	[Required, JsonProperty("searchTerm")]
	public string SearchTerm { get; init; } = string.Empty;

	[Required, JsonProperty("limit")]
	public int Limit { get; init; } = 10;

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
