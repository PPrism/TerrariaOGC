namespace Terraria
{
	public struct Location
	{
		public short X;

		public short Y;

		public Location(int LocX, int LocY)
		{
			X = (short)LocX;
			Y = (short)LocY;
		}
	}
}
