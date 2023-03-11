namespace Gump.WebAPI;

public class JwtConfig
{
	public string Secret { get; init; }
	public int Expiration { get; init; }
}
