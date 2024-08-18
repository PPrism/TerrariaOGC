using System;

namespace Terraria
{
	public struct Tile
	{
		[Flags]
		public enum Flags : byte
		{
			WALLFRAME_MASK = 0x3,
			NEARBY = 0x4,
			VISITED = 0x8,
			WIRE = 0x10,
			SELECTED = 0x20,
			LAVA = 0x20,
			CHECKING_LIQUID = 0x40,
			SKIP_LIQUID = 0x80,
			HIGHLIGHT_MASK = 0x24
		}

		public byte IsActive;

		public byte Type;

		public Flags CurrentFlags;

		public byte Liquid;

		public byte Lava;

		public byte WallType;

		public ushort wallFrameX;

		public byte wallFrameY;

		public byte frameNumber;

		public short FrameX;

		public short FrameY;

		public int wallFrameNumber
		{
			get
			{
				return (int)(CurrentFlags & Flags.WALLFRAME_MASK);
			}
			set
			{
				CurrentFlags = (Flags)((byte)value | (uint)(CurrentFlags & ~Flags.WALLFRAME_MASK));
			}
		}

		public int CheckingLiquid
		{
			get
			{
				return (int)(CurrentFlags & Flags.CHECKING_LIQUID);
			}
			set
			{
				CurrentFlags = (Flags)((byte)value | (uint)(CurrentFlags & ~Flags.CHECKING_LIQUID));
			}
		}

		public int SkipLiquid
		{
			get
			{
				return (int)(CurrentFlags & Flags.SKIP_LIQUID);
			}
			set
			{
				CurrentFlags = (Flags)((byte)value | (uint)(CurrentFlags & ~Flags.SKIP_LIQUID));
			}
		}

		public int wire
		{
			get
			{
				return (int)(CurrentFlags & Flags.WIRE);
			}
			set
			{
				CurrentFlags = (Flags)((byte)value | (uint)(CurrentFlags & ~Flags.WIRE));
			}
		}

		public void Clear()
		{
			IsActive = 0;
			CurrentFlags = ~(Flags.WALLFRAME_MASK | Flags.HIGHLIGHT_MASK | Flags.VISITED | Flags.WIRE | Flags.CHECKING_LIQUID | Flags.SKIP_LIQUID);
			Type = 0;
			WallType = 0;
			wallFrameX = 0;
			wallFrameY = 0;
			Liquid = 0;
			Lava = 0;
			frameNumber = 0;
		}

		public bool isTheSameAsExcludingVisibility(ref Tile compTile)
		{
			if (IsActive != compTile.IsActive)
			{
				return false;
			}
			if (IsActive != 0)
			{
				if (Type != compTile.Type)
				{
					return false;
				}
				if (Main.TileFrameImportant[Type])
				{
					if (FrameX != compTile.FrameX)
					{
						return false;
					}
					if (FrameY != compTile.FrameY)
					{
						return false;
					}
				}
			}
			if (WallType != compTile.WallType)
			{
				return false;
			}
			if (Liquid != compTile.Liquid)
			{
				return false;
			}
			if ((CurrentFlags & Flags.WIRE) != (compTile.CurrentFlags & Flags.WIRE))
			{
				return false;
			}
			return true;
		}

		public bool isTheSameAs(ref Tile compTile)
		{
			if (IsActive != compTile.IsActive)
			{
				return false;
			}
			if (IsActive != 0)
			{
				if (Type != compTile.Type)
				{
					return false;
				}
				if (Main.TileFrameImportant[Type])
				{
					if (FrameX != compTile.FrameX)
					{
						return false;
					}
					if (FrameY != compTile.FrameY)
					{
						return false;
					}
				}
			}
			if (WallType != compTile.WallType)
			{
				return false;
			}
			if (Liquid != compTile.Liquid)
			{
				return false;
			}
			if ((CurrentFlags & (Flags.VISITED | Flags.WIRE)) != (compTile.CurrentFlags & (Flags.VISITED | Flags.WIRE)))
			{
				return false;
			}
			return true;
		}

		public bool isFullTile()
		{
			if (IsActive != 0 && Type != 10 && Type != 54 && Type != 138 && Main.TileSolidNotSolidTop[Type])
			{
				int num = FrameY;
				if (num == 18)
				{
					int num2 = FrameX;
					if (num2 >= 18 && num2 <= 54)
					{
						return true;
					}
					if (num2 >= 108 && num2 <= 144)
					{
						return true;
					}
				}
				else if (num >= 90 && num <= 196)
				{
					int num3 = FrameX;
					if (num3 <= 70)
					{
						return true;
					}
					if (num3 >= 144 && num3 <= 232)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool CanStandOnTop()
		{
			if (IsActive != 0)
			{
				if (!Main.TileSolid[Type])
				{
					if (FrameY == 0)
					{
						return Main.TileSolidTop[Type];
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
