using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class CreatePartnerDto
{
	[Required, JsonProperty("name")]
	public string Name { get; init; } = string.Empty;

	[Required, JsonProperty("contactUrl")]
	public Uri ContactUrl { get; init; }

	[Required, JsonProperty("apiUrl")]
	public Uri ApiUrl { get; init; }
}
