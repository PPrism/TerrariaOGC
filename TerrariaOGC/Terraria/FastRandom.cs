using System;

namespace Terraria
{
	public sealed class FastRandom // Lots of unused functions here; wonder if this was reused from another Engine product.
	{
		private const double IntRealUnit = 4.6566128730773926E-10;

		//private const double UIntRealUnit = 2.3283064365386963E-10; // Used, but since the above is identical to this variable *= 2, which would be the only usage, I'm just using the above.

		private const uint YSetting = 0x32378FC7;

		private const uint ZSetting = 0xD55F8767;

		private const uint WSetting = 0x104AA1AD;

		private uint X;

		private uint Y;

		private uint Z;

		private uint W;

		private uint BitBuffer;

		private uint BitMask = 1u;

		public FastRandom()
		{
			Reinitialise((uint)Environment.TickCount);
		}

		public FastRandom(uint Seed)
		{
			Reinitialise(Seed);
		}

		public void Reinitialise(uint Seed)
		{
			X = Seed;
			Y = YSetting;
			Z = ZSetting;
			W = WSetting;
		}

		public int Next()
		{
			uint Result;
			do
			{
				uint Shift = X ^ (X << 11);
				X = Y;
				Y = Z;
				Z = W;
				W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8));
				Result = W & 0x7FFFFFFFu;
			}
			while (Result == int.MaxValue);
			return (int)Result;
		}

		public int Next(int UpperBound)
		{
			uint Shift = X ^ (X << 11);
			X = Y;
			Y = Z;
			Z = W;
			return (int)(IntRealUnit * (int)(0x7FFFFFFF & (W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8)))) * UpperBound);
		}

		public int Next(int LowerBound, int UpperBound)
		{
			uint Shift = X ^ (X << 11);
			X = Y;
			Y = Z;
			Z = W;
			return LowerBound + (int)(IntRealUnit * (int)(0x7FFFFFFF & (W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8)))) * (UpperBound - LowerBound));
		}

		public double NextDouble()
		{
			uint Shift = X ^ (X << 11);
			X = Y;
			Y = Z;
			Z = W;
			return IntRealUnit * (int)(0x7FFFFFFF & (W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8))));
		}

		public void NextBytes(byte[] Buffer)
		{
			uint SetX = X;
			uint SetY = Y;
			uint SetZ = Z;
			uint SetW = W;
			int BufferIdx = 0;
			int MaxLength = Buffer.Length - 3;
			while (BufferIdx < MaxLength)
			{
				uint Shift = SetX ^ (SetX << 11);
				SetX = SetY;
				SetY = SetZ;
				SetZ = SetW;
				SetW = SetW ^ (SetW >> 19) ^ (Shift ^ (Shift >> 8));
				Buffer[BufferIdx++] = (byte)SetW;
				Buffer[BufferIdx++] = (byte)(SetW >> 8);
				Buffer[BufferIdx++] = (byte)(SetW >> 16);
				Buffer[BufferIdx++] = (byte)(SetW >> 24);
			}
			if (BufferIdx < Buffer.Length)
			{
				uint Shift2 = SetX ^ (SetX << 11);
				SetX = SetY;
				SetY = SetZ;
				SetZ = SetW;
				SetW = SetW ^ (SetW >> 19) ^ (Shift2 ^ (Shift2 >> 8));
				Buffer[BufferIdx++] = (byte)SetW;
				if (BufferIdx < Buffer.Length)
				{
					Buffer[BufferIdx++] = (byte)(SetW >> 8);
					if (BufferIdx < Buffer.Length)
					{
						Buffer[BufferIdx++] = (byte)(SetW >> 16);
						if (BufferIdx < Buffer.Length)
						{
							Buffer[BufferIdx] = (byte)(SetW >> 24);
						}
					}
				}
			}
			X = SetX;
			Y = SetY;
			Z = SetZ;
			W = SetW;
		}

		public uint NextUInt()
		{
			uint Shift = X ^ (X << 11);
			X = Y;
			Y = Z;
			Z = W;
			return W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8));
		}

		public int NextInt()
		{
			uint Shift = X ^ (X << 11);
			X = Y;
			Y = Z;
			Z = W;
			return (int)(0x7FFFFFFF & (W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8))));
		}

		public bool NextBool()
		{
			if (BitMask == 1)
			{
				uint Shift = X ^ (X << 11);
				X = Y;
				Y = Z;
				Z = W;
				BitBuffer = (W = W ^ (W >> 19) ^ (Shift ^ (Shift >> 8)));
				BitMask = 0x80000000;
				return (BitBuffer & BitMask) == 0;
			}
			return (BitBuffer & (BitMask >>= 1)) == 0;
		}
	}
}
