namespace Terraria
{
	public struct LiquidBuffer
	{
		public const int MaxNumLiquidBuffer = 8192;

		public static int NumLiquidBuffer;

		public short X;

		public short Y;

		public static void AddBuffer(int TileX, int TileY)
		{
			if (NumLiquidBuffer != MaxNumLiquidBuffer - 1 && Main.TileSet[TileX, TileY].CheckingLiquid == 0)
			{
				Main.TileSet[TileX, TileY].CheckingLiquid = 64;
				Main.LiquidBuffer[NumLiquidBuffer].X = (short)TileX;
				Main.LiquidBuffer[NumLiquidBuffer].Y = (short)TileY;
				NumLiquidBuffer++;
			}
		}
	}
}
