using ClashofClans.Files.CsvHelpers;
using ClashofClans.Utilities.Netty;

namespace ClashofClans.Extensions
{
	public static class CustomWriter
	{
		public static void WriteData(this ByteBuffer buffer, Data value)
		{
			buffer.WriteInt(value.GetDataType());
			buffer.WriteInt(value.GetInstanceId());
		}
	}
}