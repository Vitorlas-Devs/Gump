using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class CreateCategoryDto
{
	[Required, JsonProperty("name")]
	public string Name { get; init; } = string.Empty;

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
