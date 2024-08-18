using System.IO;
using Microsoft.Xna.Framework.Net;

namespace Terraria
{
	public struct Sign
	{
		public const int MaxNumSigns = 1000;

		public short SignX;

		public short SignY;

		public UserString SignString;

		public void Init()
		{
			SignX = -1;
			SignY = -1;
			SignString = null;
		}

		public static void KillSign(int X, int Y)
		{
			for (int SignIdx = 0; SignIdx < MaxNumSigns; SignIdx++)
			{
				if (Main.SignSet[SignIdx].SignX == X && Main.SignSet[SignIdx].SignY == Y)
				{
					Main.SignSet[SignIdx].Init();
					break;
				}
			}
		}

		public static int ReadSign(int X, int Y)
		{
			int XOffset = (Main.TileSet[X, Y].FrameX / 18) & 1;
			int YOffset = Main.TileSet[X, Y].FrameY / 18;
			int SignX = X - XOffset;
			int SignY = Y - YOffset;
			if (Main.TileSet[SignX, SignY].Type != 55 && Main.TileSet[SignX, SignY].Type != 85)
			{
				KillSign(SignX, SignY);
				return -1;
			}
			int Starter = 0;
			for (int SignIdx = 0; SignIdx < MaxNumSigns; SignIdx++)
			{
				if (Main.SignSet[SignIdx].SignX < 0)
				{
					Starter = SignIdx;
				}
				else if (Main.SignSet[SignIdx].SignX == SignX && Main.SignSet[SignIdx].SignY == SignY)
				{
					return SignIdx;
				}
			}
			for (int SignIdx = Starter; SignIdx < MaxNumSigns; SignIdx++)
			{
				if (Main.SignSet[SignIdx].SignX < 0)
				{
					Main.SignSet[SignIdx].SignX = (short)SignX;
					Main.SignSet[SignIdx].SignY = (short)SignY;
					Main.SignSet[SignIdx].SignString = new UserString("", NowVerified: true);
					return SignIdx;
				}
			}
			return -1;
		}

		private unsafe bool Validate()
		{
			fixed (Tile* SignTile = &Main.TileSet[SignX, SignY])
			{
				if (SignTile->IsActive == 0 || (SignTile->Type != 55 && SignTile->Type != 85))
				{
					Init();
					return false;
				}
			}
			return true;
		}

		public void SetText(UserString Text)
		{
			if (Validate())
			{
				SignString = Text;
			}
		}

		public void Read(PacketReader Input)
		{
			SignX = ((BinaryReader)(object)Input).ReadInt16();
			SignY = ((BinaryReader)(object)Input).ReadInt16();
			SignString = new UserString((BinaryReader)(object)Input);
		}

		public void Read(BinaryReader FileIO, int Release)
		{
			if (FileIO.ReadBoolean())
			{
				if (Release >= Main.NewWorldDataVersion)
				{
					SignX = FileIO.ReadInt16();
					SignY = FileIO.ReadInt16();
					SignString = new UserString(FileIO);
				}
				else
				{
					SignString = FileIO.ReadString();
					SignX = FileIO.ReadInt16();
					SignY = FileIO.ReadInt16();
				}
				Validate();
			}
		}

		public void Write(BinaryWriter FileIO)
		{
			if (SignX < 0 || SignString == null)
			{
				FileIO.Write(value: false);
				return;
			}
			FileIO.Write(value: true);
			FileIO.Write(SignX);
			FileIO.Write(SignY);
			SignString.Write(FileIO);
		}
	}
}
