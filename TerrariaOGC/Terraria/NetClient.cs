using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace Terraria
{
	public sealed class NetClient
	{
        public NetworkMachine Machine;

		public NetworkGamer NetGamer;

		public short ServerState;

		private bool IsPublicSlotRequest;

		public bool[] PlayerSlotsTaken;

		public bool[,] TileSectionsTaken;

        public NetClient(NetworkGamer Gamer)
		{
			Machine = Gamer.Machine;
			NetGamer = Gamer;
			ServerState = 0;
			IsPublicSlotRequest = false;
			PlayerSlotsTaken = new bool[16];
			TileSectionsTaken = new bool[127, 49];
		}

		public void RequestedPublicSlot()
		{
			Netplay.Session.PrivateGamerSlots--;
			IsPublicSlotRequest = true;
		}

		public void CanceledPublicSlot()
		{
			Netplay.Session.PrivateGamerSlots++;
			IsPublicSlotRequest = false;
		}

		public void GamerJoined(Player JoinedPlayer)
		{
			JoinedPlayer.client = this;
			int PlayerIdx = JoinedPlayer.WhoAmI;
			PlayerSlotsTaken[PlayerIdx] = true;
			PlayerSlotsTaken[PlayerIdx + Player.MaxNumPlayers] = IsPublicSlotRequest;
			IsPublicSlotRequest = false;
		}

		public bool GamerLeft(Player LeftPlayer)
		{
			int PlayerIdx = LeftPlayer.WhoAmI;
			PlayerSlotsTaken[PlayerIdx] = false;
			if (PlayerSlotsTaken[PlayerIdx + Player.MaxNumPlayers])
			{
				PlayerSlotsTaken[PlayerIdx + Player.MaxNumPlayers] = false;
				Netplay.Session.PrivateGamerSlots++;
			}
			LeftPlayer.client = null;
            return ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers).Count == 0;
		}

		public void ResetSections()
		{
			for (int SectX = 0; SectX < Main.MaxSectionsX; SectX++)
			{
				for (int SectY = 0; SectY < Main.MaxSectionsY; SectY++)
				{
					TileSectionsTaken[SectX, SectY] = false;
				}
			}
		}

		public void ResetSections(ref Vector2i Min, ref Vector2i Max)
		{
			int MinX = Min.X / 40;
			int MinY = Min.Y / 30;
			int MaxX = Max.X / 40;
			int MaxY = Max.Y / 30;
			for (int SectX = MinX; SectX <= MaxX; SectX++)
			{
				for (int SectY = MinY; SectY <= MaxY; SectY++)
				{
					TileSectionsTaken[SectX, SectY] = false;
				}
			}
		}

		public bool SectionRange(int Size, int FirstX, int FirstY)
		{
			int SectX = FirstX / 40;
			int SectY = FirstY / 30;
			if (TileSectionsTaken[SectX, SectY])
			{
				return true;
			}
			int SectY2 = (FirstY + Size) / 30;
			if (TileSectionsTaken[SectX, SectY2])
			{
				return true;
			}
			SectX = (FirstX + Size) / 40;
			if (TileSectionsTaken[SectX, SectY])
			{
				return true;
			}
			return TileSectionsTaken[SectX, SectY2];
		}

		public bool IsReadyToReceive(byte[] Packets)
		{
			if (ServerState < 10)
			{
				return false;
			}
			switch (Packets[0])
			{
			case 13:
			{
				Player NetPlayer = Main.PlayerSet[Packets[1] & (Player.MaxNumPlayers-1)];
				if (NetPlayer.netSkip == 0)
				{
					return true;
				}
				Rectangle PlayerRect = NetPlayer.XYWH;
				PlayerRect.X -= 2500;
				PlayerRect.Y -= 2500;
				PlayerRect.Width += 5000;
				PlayerRect.Height += 5000;
				for (int GamerIdx = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers).Count - 1; GamerIdx >= 0; GamerIdx--)
				{
					NetworkGamer NetGamer = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers)[GamerIdx];
					Player ActivePlayer = NetGamer.Tag as Player;
					if (PlayerRect.Intersects(ActivePlayer.XYWH))
					{
						return true;
					}
				}
				return false;
			}
			case 20:
				return SectionRange(Packets[1], Packets[2] | (Packets[3] << 8), Packets[4] | (Packets[5] << 8));
			case 23:
			case 28:
			{
				NPC NetNPC = Main.NPCSet[Packets[1]];
				if (NetNPC.Life <= 0)
				{
					return true;
				}
				if (NetNPC.IsTownNPC)
				{
					return true;
				}
				Rectangle NPCRect = NetNPC.XYWH;
#if !IS_PATCHED && VERSION_INITIAL
				NPCRect.X -= 3000;
				NPCRect.Y -= 3000;
				NPCRect.Width += 6000;
				NPCRect.Height += 6000;
#else
				NPCRect.X -= 2400;
                NPCRect.Y -= 2400;
                NPCRect.Width += 4800;
                NPCRect.Height += 4800;
#endif
				for (int GamerIdx = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers).Count - 1; GamerIdx >= 0; GamerIdx--)
				{
					NetworkGamer NetGamer = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers)[GamerIdx];
					Player ActivePlayer = NetGamer.Tag as Player;
					if (NPCRect.Intersects(ActivePlayer.XYWH))
					{
						return true;
					}
				}
				return false;
			}
			default:
				return true;
			}
		}

		public bool IsReadyToReceiveProjectile(ref Projectile NetProj)
		{
			if (ServerState == 10)
			{
				if (NetProj.type == 12)
				{
					return true;
				}
				Rectangle ProjRect = NetProj.XYWH;
#if !IS_PATCHED && VERSION_INITIAL
				ProjRect.X -= 5000;
				ProjRect.Y -= 5000;
				ProjRect.Width += 10000;
				ProjRect.Height += 10000;
#else
                ProjRect.X -= 4000;
                ProjRect.Y -= 4000;
                ProjRect.Width += 8000;
                ProjRect.Height += 8000;
#endif
                for (int GamerIdx = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers).Count - 1; GamerIdx >= 0; GamerIdx--)
				{
					NetworkGamer networkGamer = ((ReadOnlyCollection<NetworkGamer>)(object)Machine.Gamers)[GamerIdx];
					Player ActivePlayer = networkGamer.Tag as Player;
					if (ProjRect.Intersects(ActivePlayer.XYWH))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
