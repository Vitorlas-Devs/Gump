using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class CreateBadgeDto
{
	[Required, JsonProperty("name")]
	public string Name { get; init; } = string.Empty;

	[Required, JsonProperty("description")]
	public string Description { get; init; } = string.Empty;

	[Required, JsonProperty("image")]
	public ulong ImageId { get; init; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
