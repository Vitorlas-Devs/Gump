namespace Gump.WebApi;

public class JwtConfig
{
	public string Secret { get; init; }
	public int Expiration { get; init; }
}
