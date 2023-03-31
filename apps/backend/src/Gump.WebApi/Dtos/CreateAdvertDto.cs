using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gump.WebApi;

public class CreateAdvertDto
{

	[Required, JsonProperty("partner")]
	public ulong PartnerId { get; init; }
	
	[Required, JsonProperty("title")]
	public string Title { get; init; } = string.Empty;

	[Required, JsonProperty("image")]
	public ulong ImageId { get; init; }

}
