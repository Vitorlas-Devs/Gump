using System.Security.Cryptography;
using System.Text;

namespace Gump.Data.Helpers;

public static class HashHelper
{
	public static string ComputeHash(string password, string salt, string pepper, int iterations)
	{
		if (iterations <= 0)
		{
			return password;
		}
		var passwordSaltPepper = $"{password}{salt}{pepper}";
		var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
		var byteHash = SHA256.HashData(byteValue);
		var hash = Convert.ToBase64String(byteHash);
		return ComputeHash(hash, salt, pepper, iterations - 1);
	}

	public static string GenerateSalt()
	{
		using var rng = RandomNumberGenerator.Create();
		var saltByte = new byte[16];
		rng.GetBytes(saltByte);
		return Convert.ToBase64String(saltByte);
	}
}
