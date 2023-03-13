using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace Gump.Data.Helpers;

public static class HashHelper
{
		public static string ComputeHash(string password, string salt, string pepper, int iterations)
	{
		return BCrypt.Net.BCrypt.HashPassword($"{password}{salt}{pepper}", iterations);
	}

	public static string GenerateSalt()
	{
		return BCrypt.Net.BCrypt.GenerateSalt();
	}
}
