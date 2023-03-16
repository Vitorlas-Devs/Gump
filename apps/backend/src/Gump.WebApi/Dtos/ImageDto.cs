using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class ImageDto
{
	[Required, JsonProperty("image")]
	public string Image { get; init; } = string.Empty;

	[Required, JsonProperty("isPrivate")]
	public bool IsPrivate { get; init; } = false;

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
