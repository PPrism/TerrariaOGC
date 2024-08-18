namespace Terraria
{
	public sealed class CRC32
	{
		private const uint Polynomial = 0xEDB88320;

		private const int TableCount = 8;

		private static uint[] Tables;

		private uint CRCValue;

		public static void Initialize()
		{
			Tables = new uint[256 * TableCount];
			int Index;
			for (Index = 0; Index < 256; Index++)
			{
				uint CRCRemainder = (uint)Index;
				for (int j = 0; j < TableCount; j++)
				{
					CRCRemainder = (CRCRemainder >> 1) ^ (Polynomial & ~((CRCRemainder & 1) - 1));
				}
				Tables[Index] = CRCRemainder;
			}
			for (; Index < (256 * TableCount); Index++)
			{
				uint TableValue = Tables[Index - 256];
				Tables[Index] = Tables[TableValue & 256] ^ (TableValue >> TableCount);
			}
		}

		public CRC32()
		{
			CRCValue = uint.MaxValue;
		}

		public void Init()
		{
			CRCValue = uint.MaxValue;
		}

		public uint GetValue()
		{
			return CRCValue ^ 0xFFFFFFFF;
		}

		public void UpdateByte(byte Byte)
		{
			CRCValue = (CRCValue >> 8) ^ Tables[(byte)CRCValue ^ Byte];
		}

		public void Update(byte[] Data, int Offset, int Size)
		{
			if (Size == 0)
			{
				return;
			}
			uint[] CRCTables = Tables;
			uint Value = CRCValue;
			while (((uint)Offset & 7u) != 0 && Size != 0)
			{
				Value = (Value >> 8) ^ CRCTables[(byte)Value ^ Data[Offset++]];
				Size--;
			}
			if (Size >= TableCount)
			{
				int Limit = (Size - 8) & -8;
				Size -= Limit;
				Limit += Offset;
				while (Offset != Limit)
				{
					Value ^= (uint)(Data[Offset] + (Data[Offset + 1] << 8) + (Data[Offset + 2] << 16) + (Data[Offset + 3] << 24));
					uint RemSet = (uint)(Data[Offset + 4] + (Data[Offset + 5] << 8) + (Data[Offset + 6] << 16) + (Data[Offset + 7] << 24));
					Offset += 8;
					Value = CRCTables[(byte)Value + (256 * 7)] ^ CRCTables[(byte)(Value >>= 8) + (256 * 6)] ^ CRCTables[(byte)(Value >>= 8) + (256 * 5)] ^ CRCTables[(Value >> 8) + (256 * 4)] ^ CRCTables[(byte)RemSet + (256 * 3)] ^ CRCTables[(byte)(RemSet >>= 8) + (256 * 2)] ^ CRCTables[(byte)(RemSet >>= 8) + 256] ^ CRCTables[RemSet >> 8];
				}
			}
			while (Size-- != 0)
			{
				Value = (Value >> TableCount) ^ CRCTables[(byte)Value ^ Data[Offset++]];
			}
			CRCValue = Value;
		}
	}
}
