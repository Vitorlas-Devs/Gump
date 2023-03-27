namespace Gump.Data;

public static class RandomExtensions
{
	public static ulong NextUInt64(this Random rnd, ulong max)
	{
		int rawsize = System.Runtime.InteropServices.Marshal.SizeOf(max);
		var buffer = new byte[rawsize];
		rnd.NextBytes(buffer);
		return BitConverter.ToUInt64(buffer, 0);
	}
}
