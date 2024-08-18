using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Terraria.Achievements;

namespace Terraria
{
	public sealed class NetMessage // Not touching this one.
	{
		private static readonly PacketWriter PacketOut = new PacketWriter(65536);

		public static PacketReader PacketIn = new PacketReader(65536);

		private static readonly byte[] PRIORITY = new byte[68]
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			3,
			1,
			1,
			3,
			1,
			1,
			1,
			1,
			1,
			1,
			2,
			1,
			1,
			1,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			1,
			1,
			3,
			1,
			2,
			2,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			0,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		};

		public static void CheckBytesServer()
		{
			LocalNetworkGamer gamer = Netplay.gamer;
			if (gamer == null)
			{
				return;
			}
			lock (PacketIn)
			{
				while (gamer.IsDataAvailable)
				{
					gamer.ReceiveData(PacketIn, out var sender);
					Player player = sender.Tag as Player;
					GetData(player.client);
				}
			}
		}

		public static void CheckBytesClient()
		{
			for (int num = Netplay.gamersWaitingForPlayerId.Count - 1; num >= 0; num--)
			{
				UI uI = Netplay.gamersWaitingForPlayerId[num];
				if (uI.localGamer.IsDataAvailable)
				{
					lock (PacketIn)
					{
						uI.localGamer.ReceiveData(PacketIn, out var _);
						int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
						num2 = ((BinaryReader)(object)PacketIn).ReadByte();
						uI.JoinSession(num2);
						uI.localGamer.Tag = Main.PlayerSet[num2];
						SendHello(num2);
						Netplay.gamersWaitingForPlayerId.RemoveAt(num);
						Netplay.gamersWaitingToSendSpawn.Add(uI);
						if (Netplay.clientState <= Netplay.ClientState.WAITING_FOR_PLAYER_ID)
						{
							Netplay.clientState = Netplay.ClientState.WAITING_FOR_PLAYER_DATA_REQ;
						}
					}
				}
			}
			if (Netplay.gamersWaitingToSendSpawn.Count > 0 && Netplay.clientState >= Netplay.ClientState.ANNOUNCING_SPAWN_LOCATION)
			{
				UI uI2 = Netplay.gamersWaitingToSendSpawn[0];
				Netplay.gamersWaitingToSendSpawn.RemoveAt(0);
				uI2.ActivePlayer.FindSpawn();
				CreateMessage3(8, uI2.MyPlayer, uI2.ActivePlayer.SpawnX, uI2.ActivePlayer.SpawnY);
				SendMessage();
				if (Netplay.clientState == Netplay.ClientState.ANNOUNCING_SPAWN_LOCATION)
				{
					Netplay.clientState = Netplay.ClientState.WAITING_FOR_TILE_DATA;
				}
			}
			else if (Netplay.gamersWaitingToSpawn.Count > 0 && Netplay.clientState >= Netplay.ClientState.PLAYING)
			{
				Main.JoinGame(Netplay.gamersWaitingToSpawn[0]);
				Netplay.gamersWaitingToSpawn.RemoveAt(0);
			}
			LocalNetworkGamer gamer = Netplay.gamer;
			if (gamer == null)
			{
				return;
			}
			lock (PacketIn)
			{
				while (gamer.IsDataAvailable)
				{
					gamer.ReceiveData(PacketIn, out var _);
					GetData(null);
				}
			}
		}

		private static void WriteCompacted(uint value)
		{
			if (value < 128)
			{
				((BinaryWriter)(object)PacketOut).Write((byte)value);
				return;
			}
			uint num = (value & 0x7Fu) | 0x80u | (value >> 7 << 8);
			if (value < 16384)
			{
				((BinaryWriter)(object)PacketOut).Write((ushort)num);
				return;
			}
			((BinaryWriter)(object)PacketOut).Write((ushort)((num & 0x7FFFu) | 0x8000u));
			value >>= 14;
			((BinaryWriter)(object)PacketOut).Write((byte)value);
		}

		private static uint ReadCompacted()
		{
			uint num = ((BinaryReader)(object)PacketIn).ReadByte();
			if (num >= 128)
			{
				num &= 0x7Fu;
				num |= (uint)(((BinaryReader)(object)PacketIn).ReadByte() << 7);
				if (num >= 16384)
				{
					num &= 0x3FFFu;
					num |= (uint)(((BinaryReader)(object)PacketIn).ReadByte() << 14);
				}
			}
			return num;
		}

		public static void CreateMessage0(int msgType)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			switch ((NetMessageId)msgType)
			{
			case NetMessageId.CLIENT_WORLD_DATA:
			{
				((BinaryWriter)(object)PacketOut).Write(Main.GameTime.WorldTime);
				int num2 = Main.GameTime.MoonPhase << 2;
				if (Main.GameTime.DayTime)
				{
					num2 |= 1;
				}
				if (Main.GameTime.IsBloodMoon)
				{
					num2 |= 2;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)num2);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.MaxTilesX);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.MaxTilesY);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.SpawnTileX);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.SpawnTileY);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.WorldSurface);
				((BinaryWriter)(object)PacketOut).Write((ushort)Main.RockLayer);
				((BinaryWriter)(object)PacketOut).Write(Main.WorldID);
				((BinaryWriter)(object)PacketOut).Write(Main.WorldTimestamp);
				num2 = (WorldGen.HasShadowOrbSmashed ? 1 : 0);
				if (NPC.HasDownedBoss1)
				{
					num2 |= 2;
				}
				if (NPC.HasDownedBoss2)
				{
					num2 |= 4;
				}
				if (NPC.HasDownedBoss3)
				{
					num2 |= 8;
				}
				if (Main.InHardMode)
				{
					num2 |= 0x10;
				}
				if (NPC.HasDownedClown)
				{
					num2 |= 0x20;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)num2);
				((BinaryWriter)(object)PacketOut).Write(Main.WorldName);
				break;
			}
			case (NetMessageId)11: // Don't think this is tile frame-related
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
					int num = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count;
					((BinaryWriter)(object)PacketOut).Write((byte)num);
					do
					{
						Player player = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[--num].Tag as Player;
						((BinaryWriter)(object)PacketOut).Write(player.WhoAmI);
					}
					while (num > 0);
				}
				break;
			case (NetMessageId)57:
				((BinaryWriter)(object)PacketOut).Write(WorldGen.GoodCoverage);
				((BinaryWriter)(object)PacketOut).Write(WorldGen.EvilCoverage);
				break;
			}
		}

		public static void CreateMessage1(int msgType, int number)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			switch ((NetMessageId)msgType)
			{
			case NetMessageId.NEVERCALLED:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.CLIENT_SAY_HELLO:
#if !IS_PATCHED && VERSION_INITIAL
				((BinaryWriter)(object)PacketOut).Write((byte)1);
#else
				((BinaryWriter)(object)PacketOut).Write((byte)2);
#endif
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.CLIENT_KICK:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				break;
			case NetMessageId.CLIENT_PLAYER_INFO:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.CLIENT_NAME_HEAD:
			{
				Player player2 = Main.PlayerSet[number];
				int num6 = number | (player2.hair << 4) | (player2.difficulty << 11);
				if (player2.male)
				{
					num6 |= 0x400;
				}
				((BinaryWriter)(object)PacketOut).Write((ushort)num6);
				((BinaryWriter)(object)PacketOut).Write(player2.hairColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.hairColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.hairColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.skinColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.skinColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.skinColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.eyeColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.eyeColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.eyeColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.shirtColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.shirtColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.shirtColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.underShirtColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.underShirtColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.underShirtColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.pantsColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.pantsColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.pantsColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.shoeColor.R);
				((BinaryWriter)(object)PacketOut).Write(player2.shoeColor.G);
				((BinaryWriter)(object)PacketOut).Write(player2.shoeColor.B);
				((BinaryWriter)(object)PacketOut).Write(player2.Name);
				break;
			}
			case NetMessageId.STATUS_TEXT_SIZE:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.CLIENT_PLAYER_SPAWN:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((short)Main.PlayerSet[number].SpawnX);
				((BinaryWriter)(object)PacketOut).Write((short)Main.PlayerSet[number].SpawnY);
				break;
			case NetMessageId.CLIENT_PLAYER_CONTROLS:
			{
				Player player = Main.PlayerSet[number];
				int num5 = 0;
				if (player.controlUp)
				{
					num5 = 1;
				}
				if (player.IsControlDown)
				{
					num5 |= 2;
				}
				if (player.controlLeft)
				{
					num5 |= 4;
				}
				if (player.controlRight)
				{
					num5 |= 8;
				}
				if (player.controlJump)
				{
					num5 |= 0x10;
				}
				if (player.controlUseItem)
				{
					num5 |= 0x20;
				}
#if (!VERSION_INITIAL || IS_PATCHED)
				number |= 0x20;
#endif
                if (player.direction == 1)
				{
					number |= 0x40;
				}
				if (num5 != 0)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)(number | 128));
					((BinaryWriter)(object)PacketOut).Write((byte)num5);

#if !IS_PATCHED && VERSION_INITIAL
					if (((uint)num5 & 0x20u) != 0)
					{
						((BinaryWriter)(object)PacketOut).Write(player.SelectedItem);
					}
#endif
                }
                else
				{
					((BinaryWriter)(object)PacketOut).Write((byte)number);
				}
#if (!VERSION_INITIAL || IS_PATCHED)
                if (((uint)number & 0x20u) != 0)
                {
                    ((BinaryWriter)(object)PacketOut).Write(player.SelectedItem);
                }
#endif
				PacketOut.Write(player.Position);
				HalfVector2 halfVector3 = new HalfVector2(player.velocity);
				((BinaryWriter)(object)PacketOut).Write(halfVector3.PackedValue);
				if (Main.NetMode == (byte)NetModeSetting.SERVER && ++player.netSkip > 2)
				{
					player.netSkip = 0;
				}
				break;
			}
			case NetMessageId.MSG_PLAYER_LIFE:
			{
				int value = number | ((Main.PlayerSet[number].statLife & 0xFFF) << 4) | (Main.PlayerSet[number].StatLifeMax << 16);
				((BinaryWriter)(object)PacketOut).Write(value);
				break;
			}
			case NetMessageId.MSG_ITEM_OWNER:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				int num = Main.ItemSet[number].Owner;
				if (num < 8)
				{
					Vector2 velocity = Main.ItemSet[number].Velocity;
					if (velocity.X != 0f || velocity.Y != 0f)
					{
						num |= 0x80;
					}
					((BinaryWriter)(object)PacketOut).Write((byte)num);
					PacketOut.Write(Main.ItemSet[number].Position);
					if (((uint)num & 0x80u) != 0)
					{
						HalfVector2 halfVector = new HalfVector2(velocity);
						((BinaryWriter)(object)PacketOut).Write(halfVector.PackedValue);
					}
				}
				else
				{
					((BinaryWriter)(object)PacketOut).Write((byte)num);
				}
				break;
			}
			case NetMessageId.MSG_SYNC_NPC:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				int num3 = ((Main.NPCSet[number].Active != 0) ? Main.NPCSet[number].Life : 0);
				if (num3 <= 0)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)0);
					Main.NPCSet[number].NetSkip = 0;
					break;
				}
				WriteCompacted((uint)num3);
				((BinaryWriter)(object)PacketOut).Write(Main.NPCSet[number].NetID);
				PacketOut.Write(Main.NPCSet[number].Position);
				HalfVector2 halfVector2 = new HalfVector2(Main.NPCSet[number].Velocity);
				((BinaryWriter)(object)PacketOut).Write(halfVector2.PackedValue);
				((BinaryWriter)(object)PacketOut).Write((sbyte)(Main.NPCSet[number].Target | ((Main.NPCSet[number].Direction & 3) << 4) | (Main.NPCSet[number].DirectionY << 6)));
				int num4 = 0;
				float ai = Main.NPCSet[number].AI0;
				if (ai != 0f)
				{
					num4 = 1;
				}
				float ai2 = Main.NPCSet[number].AI1;
				if (ai2 != 0f)
				{
					num4 |= 2;
				}
				float ai3 = Main.NPCSet[number].AI2;
				if (ai3 != 0f)
				{
					num4 |= 4;
				}
				float ai4 = Main.NPCSet[number].AI3;
				if (ai4 != 0f)
				{
					num4 |= 8;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)num4);
				if (((uint)num4 & (true ? 1u : 0u)) != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(ai);
				}
				if (((uint)num4 & 2u) != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(ai2);
				}
				if (((uint)num4 & 4u) != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(ai3);
				}
				if (((uint)num4 & 8u) != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(ai4);
				}
				break;
			}
			case NetMessageId.MSG_PLAYER_VS_PLAYER:
				if (Main.PlayerSet[number].hostile)
				{
					number |= 0x80;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.MSG_PLAYER_ZONE_INFO:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				int num2 = 0;
				if (Main.PlayerSet[number].ZoneEvil)
				{
					num2 = 1;
				}
				if (Main.PlayerSet[number].ZoneMeteor)
				{
					num2 |= 2;
				}
				if (Main.PlayerSet[number].ZoneDungeon)
				{
					num2 |= 4;
				}
				if (Main.PlayerSet[number].ZoneJungle)
				{
					num2 |= 8;
				}
				if (Main.PlayerSet[number].zoneHoly)
				{
					num2 |= 0x10;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)num2);
				break;
			}
			case NetMessageId.MSG_PLAYER_NPC_TALK:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write(Main.PlayerSet[number].TalkNPC);
				break;
			case NetMessageId.MSG_PLAYER_GUN_ROTATION:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write(Main.PlayerSet[number].itemRotation);
				((BinaryWriter)(object)PacketOut).Write(Main.PlayerSet[number].itemAnimation);
				break;
			case NetMessageId.MSG_PLAYER_MANA:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write(Main.PlayerSet[number].statMana);
				((BinaryWriter)(object)PacketOut).Write(Main.PlayerSet[number].statManaMax);
				break;
			case NetMessageId.UNK_MSG_45:
				((BinaryWriter)(object)PacketOut).Write((byte)(number | (Main.PlayerSet[number].team << 4)));
				break;
			case NetMessageId.MSG_INITIAL_SPAWN:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case NetMessageId.MSG_PLAYER_BUFFS:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				for (int j = 0; j < Player.MaxNumBuffs; j++)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)Main.PlayerSet[number].buff[j].Type);
				}
				break;
			}
			case (NetMessageId)51:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				break;
			case (NetMessageId)54:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				for (int i = 0; i < NPC.MaxNumNPCBuffs; i++)
				{
					uint type = Main.NPCSet[number].ActiveBuffs[i].Type;
					((BinaryWriter)(object)PacketOut).Write((byte)type);
					if (type != 0)
					{
						WriteCompacted(Main.NPCSet[number].ActiveBuffs[i].Time);
					}
				}
				break;
			}
			case (NetMessageId)(int)SendDataId.SERVER_NPC_NAMES:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write(NPC.TypeNames[number]);
				break;
			case (NetMessageId)58:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write(Main.HarpNote);
				break;
			}
		}

		public unsafe static void CreateMessage2(int msgType, int number, int number2)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			switch ((NetMessageId)msgType)
			{
#if (!VERSION_INITIAL || IS_PATCHED)
			case NetMessageId.CLIENT_INVENTORY:
			{
				int num6 = number;
				int num7 = number2;
				((BinaryWriter)(object)PacketOut).Write((byte)num6);
				((BinaryWriter)(object)PacketOut).Write((byte)num7);
				int stack2;
				int netID;
				int prefix;
				if (num7 < 49)
				{
					stack2 = Main.PlayerSet[num6].Inventory[num7].Stack;
					netID = Main.PlayerSet[num6].Inventory[num7].NetID;
					prefix = Main.PlayerSet[num6].Inventory[num7].PrefixType;
				}
				else
				{
					num7 -= 49;
					stack2 = Main.PlayerSet[num6].armor[num7].Stack;
					netID = Main.PlayerSet[num6].armor[num7].NetID;
					prefix = Main.PlayerSet[num6].armor[num7].PrefixType;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)stack2);
				if (stack2 > 0)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)prefix);
					((BinaryWriter)(object)PacketOut).Write((short)netID);
				}
				break;
			}
			case NetMessageId.CLIENT_TILE_SECTION:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				int num8 = number * 40;
				int num9 = number2 * 30;
				fixed (Tile* ptr3 = Main.TileSet)
				{
					Tile* ptr2 = null;
					uint num10 = 0;
					for (int i = num8; i < num8 + 40; i++)
					{
						Tile* ptr4 = ptr3 + (num9 + i * Main.LargeWorldH);
						for (int num11 = 29; num11 >= 0; num11--)
						{
							if (ptr2 != null && ptr4->isTheSameAsExcludingVisibility(ref *ptr2))
							{
								num10++;
							}
							else
							{
								if (ptr2 != null)
								{
											WriteCompacted(num10);
								}
								num10 = 0;
								ptr2 = ptr4;
								int active2 = ptr4->IsActive;
								int num12 = active2;
								int wall2 = ptr4->WallType;
								if (wall2 > 0)
								{
									num12 |= 8;
								}
								int liquid2 = ptr4->Liquid;
								if (liquid2 > 0)
								{
									num12 |= ptr4->Lava;
									num12 = ((liquid2 != 255) ? (num12 | 4) : (num12 | 2));
								}
								num12 |= ptr4->wire;
								((BinaryWriter)(object)PacketOut).Write((byte)num12);
								if (active2 != 0)
								{
									int type2 = ptr4->Type;
									((BinaryWriter)(object)PacketOut).Write((byte)type2);
									if (Main.TileFrameImportant[type2])
									{
												WriteCompacted((uint)ptr4->FrameX);
												WriteCompacted((uint)ptr4->FrameY);
									}
								}
								if (wall2 > 0)
								{
									((BinaryWriter)(object)PacketOut).Write((byte)wall2);
								}
								if (((uint)num12 & 4u) != 0)
								{
									((BinaryWriter)(object)PacketOut).Write((byte)liquid2);
								}
							}
							ptr4++;
						}
					}
							WriteCompacted(num10);
				}
				break;
			}
			case NetMessageId.CLIENT_PLAYER_ACTIVE:
				((BinaryWriter)(object)PacketOut).Write((byte)(number | (number2 << 7)));
				break;
			case NetMessageId.CLIENT_REQ_RESYNC:
			{
				int num3 = number;
				((BinaryWriter)(object)PacketOut).Write((ushort)num3);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				fixed (Tile* ptr = &Main.TileSet[num3, number2])
				{
					int active = ptr->IsActive;
					int num4 = active;
					int wall = ptr->WallType;
					if (wall > 0)
					{
						num4 |= 4;
					}
					int num5 = ((Main.NetMode == (byte)NetModeSetting.SERVER) ? ptr->Liquid : 0);
					if (num5 > 0)
					{
						num4 |= 8 | ptr->Lava;
					}
					num4 |= ptr->wire;
					((BinaryWriter)(object)PacketOut).Write((byte)num4);
					if (active != 0)
					{
						int type = ptr->Type;
						((BinaryWriter)(object)PacketOut).Write((byte)type);
						if (Main.TileFrameImportant[type])
						{
									WriteCompacted((uint)ptr->FrameX);
									WriteCompacted((uint)ptr->FrameY);
						}
					}
					if (wall > 0)
					{
						((BinaryWriter)(object)PacketOut).Write((byte)wall);
					}
					if (num5 > 0)
					{
						((BinaryWriter)(object)PacketOut).Write((byte)num5);
					}
				}
				break;
			}
            case NetMessageId.MSG_SYNC_ITEM:
            {
                int num2 = 0;
                int stack = Main.ItemSet[number2].Stack;
                if (stack > 0 && Main.ItemSet[number2].Active != 0)
                {
                    num2 = Main.ItemSet[number2].NetID;
                }
                if (num2 == 0 && number2 >= 200)
                {
							ClearMessage();
                    break;
                }
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
                ((BinaryWriter)(object)PacketOut).Write((short)((num2 << 5) | number));
                if (num2 != 0)
                {
                    ((BinaryWriter)(object)PacketOut).Write(Main.ItemSet[number2].PrefixType);
                    ((BinaryWriter)(object)PacketOut).Write((byte)stack);
							PacketOut.Write(Main.ItemSet[number2].Position);
                    HalfVector2 halfVector = new HalfVector2(Main.ItemSet[number2].Velocity);
                    ((BinaryWriter)(object)PacketOut).Write(halfVector.PackedValue);
                }
                break;
            }
            case NetMessageId.MSG_UNUSED_MELEE_STRIKE:
                ((BinaryWriter)(object)PacketOut).Write((ushort)number);
                ((BinaryWriter)(object)PacketOut).Write((ushort)number2);
                break;
            case NetMessageId.MSG_KILL_PROJECTILE:
					WriteCompacted((uint)number);
                ((BinaryWriter)(object)PacketOut).Write((byte)number2);
                break;
            case NetMessageId.MSG_CHEST_ITEM:
            {
						WriteCompacted((uint)number);
                ((BinaryWriter)(object)PacketOut).Write((byte)number2);
                int netID2 = Main.ChestSet[number].ItemSet[number2].NetID;
                ((BinaryWriter)(object)PacketOut).Write((short)netID2);
                if (netID2 != 0)
                {
                    ((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number].ItemSet[number2].PrefixType);
                    ((BinaryWriter)(object)PacketOut).Write((byte)Main.ChestSet[number].ItemSet[number2].Stack);
                }
                break;
            }
            case NetMessageId.MSG_PLAYER_CHEST_VAR:
                ((BinaryWriter)(object)PacketOut).Write((short)((number2 << 5) | number));
                if (number2 >= 0)
                {
                    ((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number2].XPos);
                    ((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number2].YPos);
                }
                break;
            case NetMessageId.CLIENT_REQ_KILL_TILE:
                ((BinaryWriter)(object)PacketOut).Write((ushort)number);
                ((BinaryWriter)(object)PacketOut).Write((ushort)number2);
                break;
            case NetMessageId.MSG_PLAYER_HEAL_EFFECT:
                ((BinaryWriter)(object)PacketOut).Write((byte)number);
					WriteCompacted((uint)number2);
                break;
            case NetMessageId.UNK_MSG_43:
                ((BinaryWriter)(object)PacketOut).Write((byte)number);
                ((BinaryWriter)(object)PacketOut).Write((short)number2);
                break;
            case NetMessageId.UNK_MSG_47:
                ((BinaryWriter)(object)PacketOut).Write((byte)number);
					WriteCompacted((uint)number2);
                ((BinaryWriter)(object)PacketOut).Write(Main.SignSet[number2].SignX);
                ((BinaryWriter)(object)PacketOut).Write(Main.SignSet[number2].SignY);
                Main.SignSet[number2].SignString.Write((BinaryWriter)(object)PacketOut);
                break;
            case NetMessageId.MSG_LIQUID_UPDATE:
            {
                int num = number;
                int liquid = Main.TileSet[num, number2].Liquid;
                if (liquid > 0)
                {
                    number = ((liquid >= 255) ? (number | 0x4000) : (number | 0x8000));
                }
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
                ((BinaryWriter)(object)PacketOut).Write((ushort)(number2 | (Main.TileSet[num, number2].Lava << 10)));
                if (((uint)number & 0x8000u) != 0)
                {
                    ((BinaryWriter)(object)PacketOut).Write((byte)liquid);
                }
                break;
            }
#else
			case NetMessageId.CLIENT_INVENTORY:
			{
				int num9 = number2;
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)num9);
				int stack2;
				int netID2;
				int prefix;
				if (num9 < 49)
				{
					stack2 = Main.PlayerSet[number].Inventory[num9].Stack;
					netID2 = Main.PlayerSet[number].Inventory[num9].NetID;
					prefix = Main.PlayerSet[number].Inventory[num9].PrefixType;
				}
				else
				{
					num9 -= 49;
					stack2 = Main.PlayerSet[number].armor[num9].Stack;
					netID2 = Main.PlayerSet[number].armor[num9].NetID;
					prefix = Main.PlayerSet[number].armor[num9].PrefixType;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)stack2);
				if (stack2 > 0)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)prefix);
					((BinaryWriter)(object)PacketOut).Write((short)netID2);
				}
				break;
			}
			case NetMessageId.CLIENT_TILE_SECTION:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				int num = number * 40;
				int num2 = number2 * 30;
				fixed (Tile* ptr2 = Main.TileSet)
				{
					Tile* ptr = null;
					uint num3 = 0;
					for (int i = num; i < num + 40; i++)
					{
						Tile* ptr3 = ptr2 + (num2 + i * Main.LargeWorldH);
						for (int num4 = 29; num4 >= 0; num4--)
						{
							if (ptr != null && ptr3->isTheSameAsExcludingVisibility(ref *ptr))
							{
								num3++;
							}
							else
							{
								if (ptr != null)
								{
									WriteCompacted(num3);
								}
								num3 = 0;
								ptr = ptr3;
								int active = ptr3->IsActive;
								int num5 = active;
								int wall = ptr3->WallType;
								if (wall > 0)
								{
									num5 |= 4;
								}
								int liquid = ptr3->Liquid;
								if (liquid > 0)
								{
									num5 |= 8 | ptr3->Lava;
								}
								num5 |= ptr3->wire;
								((BinaryWriter)(object)PacketOut).Write((byte)num5);
								if (active != 0)
								{
									int type = ptr3->Type;
									((BinaryWriter)(object)PacketOut).Write((byte)type);
									if (Main.TileFrameImportant[type])
									{
										WriteCompacted((uint)ptr3->FrameX);
										WriteCompacted((uint)ptr3->FrameY);
									}
								}
								if (wall > 0)
								{
									((BinaryWriter)(object)PacketOut).Write((byte)wall);
								}
								if (liquid > 0)
								{
									((BinaryWriter)(object)PacketOut).Write((byte)liquid);
								}
							}
							ptr3++;
						}
					}
					WriteCompacted(num3);
				}
				break;
			}
			case NetMessageId.CLIENT_PLAYER_ACTIVE:
				((BinaryWriter)(object)PacketOut).Write((byte)(number | (number2 << 7)));
				break;
			case NetMessageId.CLIENT_REQ_RESYNC:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				fixed (Tile* ptr4 = &Main.TileSet[number, number2])
				{
					int active2 = ptr4->IsActive;
					int num7 = active2;
					int wall2 = ptr4->WallType;
					if (wall2 > 0)
					{
						num7 |= 4;
					}
					int num8 = ((Main.NetMode == (byte)NetModeSetting.SERVER) ? ptr4->Liquid : 0);
					if (num8 > 0)
					{
						num7 |= 8 | ptr4->Lava;
					}
					num7 |= ptr4->wire;
					((BinaryWriter)(object)PacketOut).Write((byte)num7);
					if (active2 != 0)
					{
						int type2 = ptr4->Type;
						((BinaryWriter)(object)PacketOut).Write((byte)type2);
						if (Main.TileFrameImportant[type2])
						{
							WriteCompacted((uint)ptr4->FrameX);
							WriteCompacted((uint)ptr4->FrameY);
						}
					}
					if (wall2 > 0)
					{
						((BinaryWriter)(object)PacketOut).Write((byte)wall2);
					}
					if (num8 > 0)
					{
						((BinaryWriter)(object)PacketOut).Write((byte)num8);
					}
				}
				break;
			case NetMessageId.MSG_SYNC_ITEM:
			{
				int num6 = 0;
				int stack = Main.ItemSet[number2].Stack;
				if (stack > 0 && Main.ItemSet[number2].Active != 0)
				{
					num6 = Main.ItemSet[number2].NetID;
				}
				if (num6 == 0 && number2 >= Main.MaxNumItems)
				{
					ClearMessage();
					break;
				}
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				((BinaryWriter)(object)PacketOut).Write((short)((num6 << 5) | number));
				if (num6 != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(Main.ItemSet[number2].PrefixType);
					((BinaryWriter)(object)PacketOut).Write((byte)stack);
					PacketOut.Write(Main.ItemSet[number2].Position);
					HalfVector2 halfVector = new HalfVector2(Main.ItemSet[number2].Velocity);
					((BinaryWriter)(object)PacketOut).Write(halfVector.PackedValue);
				}
				break;
			}
			case NetMessageId.MSG_UNUSED_MELEE_STRIKE:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				break;
			case NetMessageId.MSG_KILL_PROJECTILE:
				WriteCompacted((uint)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				break;
			case NetMessageId.MSG_CHEST_ITEM:
			{
				WriteCompacted((uint)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				int netID = Main.ChestSet[number].ItemSet[number2].NetID;
				((BinaryWriter)(object)PacketOut).Write((short)netID);
				if (netID != 0)
				{
					((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number].ItemSet[number2].PrefixType);
					((BinaryWriter)(object)PacketOut).Write((byte)Main.ChestSet[number].ItemSet[number2].Stack);
				}
				break;
			}
			case NetMessageId.MSG_PLAYER_CHEST_VAR:
				((BinaryWriter)(object)PacketOut).Write((short)((number2 << 5) | number));
				if (number2 >= 0)
				{
					((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number2].XPos);
					((BinaryWriter)(object)PacketOut).Write(Main.ChestSet[number2].YPos);
				}
				break;
			case NetMessageId.CLIENT_REQ_KILL_TILE:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				break;
			case NetMessageId.MSG_PLAYER_HEAL_EFFECT:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				WriteCompacted((uint)number2);
				break;
			case NetMessageId.UNK_MSG_43:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((short)number2);
				break;
			case NetMessageId.UNK_MSG_47:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				WriteCompacted((uint)number2);
				((BinaryWriter)(object)PacketOut).Write(Main.SignSet[number2].SignX);
				((BinaryWriter)(object)PacketOut).Write(Main.SignSet[number2].SignY);
				Main.SignSet[number2].SignString.Write((BinaryWriter)(object)PacketOut);
				break;
			case NetMessageId.MSG_LIQUID_UPDATE:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)(number2 | (Main.TileSet[number, number2].Lava << 10)));
				((BinaryWriter)(object)PacketOut).Write(Main.TileSet[number, number2].Liquid);
				break;
#endif
            case (NetMessageId)59:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				break;
			case (NetMessageId)61:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((short)number2);
				break;
			case (NetMessageId)64:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				break;
			case (NetMessageId)65:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				break;
			}
		}

		public static void CreateMessage3(int msgType, int number, int number2, int number3)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			switch ((NetMessageId)msgType)
			{
			case NetMessageId.CLIENT_REQ_STARTING_DATA:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((short)number2);
				((BinaryWriter)(object)PacketOut).Write((short)number3);
				break;
			case NetMessageId.MSG_TOGGLE_DOOR_STATE:
				((BinaryWriter)(object)PacketOut).Write((ushort)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((sbyte)number3);
				break;
			case NetMessageId.UNK_MSG_20:
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((ushort)number3);
				for (int i = number2; i < number2 + number; i++)
				{
					for (int j = number3; j < number3 + number; j++)
					{
						int active = Main.TileSet[i, j].IsActive;
						int num = active;
						int wall = Main.TileSet[i, j].WallType;
						if (wall > 0)
						{
							num |= 4;
						}
						int num2 = ((Main.NetMode == (byte)NetModeSetting.SERVER) ? Main.TileSet[i, j].Liquid : 0);
						if (num2 > 0)
						{
							num |= 8 | Main.TileSet[i, j].Lava;
						}
						num |= Main.TileSet[i, j].wire;
						((BinaryWriter)(object)PacketOut).Write((byte)num);
						if (active != 0)
						{
							int type = Main.TileSet[i, j].Type;
							((BinaryWriter)(object)PacketOut).Write((byte)type);
							if (Main.TileFrameImportant[type])
							{
								WriteCompacted((uint)Main.TileSet[i, j].FrameX);
								WriteCompacted((uint)Main.TileSet[i, j].FrameY);
							}
						}
						if (wall > 0)
						{
							((BinaryWriter)(object)PacketOut).Write((byte)wall);
						}
						if (num2 > 0)
						{
							((BinaryWriter)(object)PacketOut).Write((byte)num2);
						}
					}
				}
				break;
			}
			case NetMessageId.MSG_REQ_CHEST_ITEM:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((ushort)number3);
				break;
			case NetMessageId.UNK_MSG_46:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((ushort)number3);
				break;
			case (NetMessageId)52:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((ushort)number3);
				break;
			case (NetMessageId)53:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				WriteCompacted((uint)number3);
				break;
			case (NetMessageId)55:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((byte)number2);
				WriteCompacted((uint)number3);
				break;
			}
		}

		public static void CreateMessage4(int msgType, int number, int number2, int number3, int number4)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			if (msgType == 60)
			{
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((short)number2);
				((BinaryWriter)(object)PacketOut).Write((short)number3);
				((BinaryWriter)(object)PacketOut).Write((byte)number4);
			}
		}

		public static void CreateMessage5(int msgType, int number, int number2, int number3, int number4, int number5 = 0)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)msgType);
			switch ((NetMessageId)msgType)
			{
			case NetMessageId.MSG_WORLD_CHANGED:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((ushort)number2);
				((BinaryWriter)(object)PacketOut).Write((ushort)number3);
				if (number <= 4)
				{
					((BinaryWriter)(object)PacketOut).Write((byte)number4);
					if (number == 1)
					{
						((BinaryWriter)(object)PacketOut).Write((byte)number5);
					}
				}
				break;
			case NetMessageId.UNK_MSG_44:
				((BinaryWriter)(object)PacketOut).Write((byte)number);
				((BinaryWriter)(object)PacketOut).Write((sbyte)number2);
				((BinaryWriter)(object)PacketOut).Write((short)number3);
				((BinaryWriter)(object)PacketOut).Write((byte)number4);
				((BinaryWriter)(object)PacketOut).Write((uint)number5);
				break;
			}
		}

		public static void SendPlayerId(NetworkGamer gamer, int playerId)
		{
			CreateMessage1(0, playerId);
			SendMessage(gamer);
		}

		public static void SendHello(int playerId)
		{
			CreateMessage1(1, playerId);
			SendMessage();
		}

		public static void SendKick(NetClient client, int textId)
		{
			CreateMessage1(2, textId);
			SendMessage(client);
		}

		public static void SendPlayerInfoRequest(NetClient client, int playerId)
		{
			CreateMessage1(3, playerId);
			SendMessage(client);
		}

		public static void SendPlayerHurt(int playerId, int dir, int dmg, bool pvp, bool critical, uint deathText)
		{
#if (!VERSION_INITIAL || IS_PATCHED)
            if (pvp)
            {
                playerId |= 0x40;
            }
            if (critical)
            {
                playerId |= 0x80;
            }
#endif
			((BinaryWriter)(object)PacketOut).Write((byte)26);
			((BinaryWriter)(object)PacketOut).Write((byte)playerId);
			((BinaryWriter)(object)PacketOut).Write((sbyte)dir);
			((BinaryWriter)(object)PacketOut).Write((short)dmg);
#if !IS_PATCHED && VERSION_INITIAL
			((BinaryWriter)(object)PacketOut).Write(pvp);
			((BinaryWriter)(object)PacketOut).Write(critical);
#endif
			((BinaryWriter)(object)PacketOut).Write(deathText);
			SendMessage();
		}

		public static void SendProjectile(int number, SendDataOptions sendOptions = SendDataOptions.Reliable)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)27);
			int num = 0;
			float knockBack = Main.ProjectileSet[number].knockBack;
			if (knockBack != 0f)
			{
				num = 1;
			}
			int damage = Main.ProjectileSet[number].damage;
			if (damage != 0)
			{
				num |= 2;
			}
			float ai = Main.ProjectileSet[number].ai0;
			if (ai != 0f)
			{
				num |= 4;
			}
			int ai2 = Main.ProjectileSet[number].ai1;
			if (ai2 != 0)
			{
				num |= 8;
			}
			((BinaryWriter)(object)PacketOut).Write((byte)(Main.ProjectileSet[number].owner | (num << 4)));
			((BinaryWriter)(object)PacketOut).Write(Main.ProjectileSet[number].type);
			WriteCompacted(Main.ProjectileSet[number].identity);
			PacketOut.Write(Main.ProjectileSet[number].position);
			HalfVector2 halfVector = new HalfVector2(Main.ProjectileSet[number].velocity);
			((BinaryWriter)(object)PacketOut).Write(halfVector.PackedValue);
			if (((uint)num & (true ? 1u : 0u)) != 0)
			{
				HalfSingle halfSingle = new HalfSingle(knockBack);
				((BinaryWriter)(object)PacketOut).Write(halfSingle.PackedValue);
			}
			if (((uint)num & 2u) != 0)
			{
				((BinaryWriter)(object)PacketOut).Write((short)damage);
			}
			if (((uint)num & 4u) != 0)
			{
				((BinaryWriter)(object)PacketOut).Write(ai);
			}
			if (((uint)num & 8u) != 0)
			{
				((BinaryWriter)(object)PacketOut).Write((short)ai2);
			}
			SendProjectileMessage(ref Main.ProjectileSet[number], sendOptions);
		}

		public static void SendNpcHurt(int npcId, int dmg)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)28);
			((BinaryWriter)(object)PacketOut).Write((byte)npcId);
			((BinaryWriter)(object)PacketOut).Write((short)dmg);
			SendMessage();
		}

		public static void SendNpcHurt(int npcId, int dmg, double kb, int dir, bool critical = false)
		{
			((BinaryWriter)(object)PacketOut).Write((byte)28);
			((BinaryWriter)(object)PacketOut).Write((byte)npcId);
			((BinaryWriter)(object)PacketOut).Write((short)dmg);
			if (dmg >= 0)
			{
				HalfSingle halfSingle = new HalfSingle((float)kb);
				((BinaryWriter)(object)PacketOut).Write(halfSingle.PackedValue);
				dir <<= 1;
				if (critical)
				{
					dir |= 1;
				}
				((BinaryWriter)(object)PacketOut).Write((sbyte)dir);
			}
			SendMessage();
		}

		public static void SendText(int textId, int r, int g, int b, int player)
		{
			if (player < 0 || Main.PlayerSet[player].client == null)
			{
				Main.NewText(Lang.MiscText[textId], r, g, b);
				if (player >= 0 && Main.PlayerSet[player].client == null)
				{
					return;
				}
			}
			((BinaryWriter)(object)PacketOut).Write((byte)18);
			((BinaryWriter)(object)PacketOut).Write((byte)r);
			((BinaryWriter)(object)PacketOut).Write((byte)g);
			((BinaryWriter)(object)PacketOut).Write((byte)b);
			((BinaryWriter)(object)PacketOut).Write((ushort)textId);
			if (player < 0)
			{
				SendMessage();
			}
			else
			{
				SendMessage(Main.PlayerSet[player].client);
			}
		}

		public static void SendText(string prefix, int textId, int r, int g, int b, int player)
		{
			if (player < 0 || Main.PlayerSet[player].client == null)
			{
				Main.NewText(prefix + Lang.MiscText[textId], r, g, b);
				if (player >= 0 && Main.PlayerSet[player].client == null)
				{
					return;
				}
			}
			((BinaryWriter)(object)PacketOut).Write((byte)37);
			((BinaryWriter)(object)PacketOut).Write((byte)r);
			((BinaryWriter)(object)PacketOut).Write((byte)g);
			((BinaryWriter)(object)PacketOut).Write((byte)b);
			((BinaryWriter)(object)PacketOut).Write((ushort)textId);
			((BinaryWriter)(object)PacketOut).Write(prefix);
			if (player < 0)
			{
				SendMessage();
			}
			else
			{
				SendMessage(Main.PlayerSet[player].client);
			}
		}

		public static void SendText(int textId, string postfix, int r, int g, int b, int player)
		{
			if (player < 0 || Main.PlayerSet[player].client == null)
			{
				Main.NewText(Lang.MiscText[textId] + postfix, r, g, b);
				if (player >= 0 && Main.PlayerSet[player].client == null)
				{
					return;
				}
			}
			((BinaryWriter)(object)PacketOut).Write((byte)38);
			((BinaryWriter)(object)PacketOut).Write((byte)r);
			((BinaryWriter)(object)PacketOut).Write((byte)g);
			((BinaryWriter)(object)PacketOut).Write((byte)b);
			((BinaryWriter)(object)PacketOut).Write((ushort)textId);
			((BinaryWriter)(object)PacketOut).Write(postfix);
			if (player < 0)
			{
				SendMessage();
			}
			else
			{
				SendMessage(Main.PlayerSet[player].client);
			}
		}

		public static void SendText(string text, int r, int g, int b, int player) // Unused
		{
			if (player < 0 || Main.PlayerSet[player].client == null)
			{
				Main.NewText(text, r, g, b);
				if (player >= 0 && Main.PlayerSet[player].client == null)
				{
					return;
				}
			}
			((BinaryWriter)(object)PacketOut).Write((byte)25);
			((BinaryWriter)(object)PacketOut).Write((byte)r);
			((BinaryWriter)(object)PacketOut).Write((byte)g);
			((BinaryWriter)(object)PacketOut).Write((byte)b);
			((BinaryWriter)(object)PacketOut).Write(text);
			if (player < 0)
			{
				SendMessage();
			}
			else
			{
				SendMessage(Main.PlayerSet[player].client);
			}
		}

		public static void SendDeathText(string name, uint deathText, int r, int g, int b)
		{
			Main.NewText(name + Lang.DeathMsgString(deathText), r, g, b);
			((BinaryWriter)(object)PacketOut).Write((byte)63);
			((BinaryWriter)(object)PacketOut).Write((byte)r);
			((BinaryWriter)(object)PacketOut).Write((byte)g);
			((BinaryWriter)(object)PacketOut).Write((byte)b);
			((BinaryWriter)(object)PacketOut).Write(deathText);
			((BinaryWriter)(object)PacketOut).Write(name);
			SendMessage();
		}

		public static void SendMessage()
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions options = (SendDataOptions)PRIORITY[num];
			if (Main.NetMode == (byte)NetModeSetting.CLIENT && Netplay.Session.Host != null)
			{
				try
				{
					Netplay.gamer.SendData(PacketOut, options, Netplay.Session.Host);
				}
				catch
				{
				}
			}
			else
			{
				for (int num2 = Netplay.ClientList.Count - 1; num2 >= 0; num2--)
				{
					NetClient netClient = Netplay.ClientList[num2];
					if (netClient.IsReadyToReceive(memoryStream.GetBuffer()))
					{
						try
						{
							Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketOut.Length, options, netClient.NetGamer);
						}
						catch
						{
						}
					}
				}
			}
			memoryStream.Position = 0L;
			memoryStream.SetLength(0L);
		}

		private static void SendProjectileMessage(ref Projectile projectile, SendDataOptions sendOptions)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				try
				{
					Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketOut.Length, sendOptions, Netplay.Session.Host);
				}
				catch
				{
				}
			}
			else
			{
				for (int num = Netplay.ClientList.Count - 1; num >= 0; num--)
				{
					NetClient netClient = Netplay.ClientList[num];
					if (netClient.IsReadyToReceiveProjectile(ref projectile))
					{
						try
						{
							Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketOut.Length, sendOptions, netClient.NetGamer);
						}
						catch
						{
						}
					}
				}
			}
			memoryStream.Position = 0L;
			memoryStream.SetLength(0L);
		}

		public static void SendMessage(NetClient client)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions sendDataOptions = (SendDataOptions)PRIORITY[num];
			switch (sendDataOptions)
			{
			case SendDataOptions.None:
				sendDataOptions = SendDataOptions.Reliable;
				break;
			case SendDataOptions.InOrder:
				sendDataOptions = SendDataOptions.ReliableInOrder;
				break;
			}
			try
			{
				Netplay.gamer.SendData(PacketOut, sendDataOptions, client.NetGamer);
			}
			catch
			{
				memoryStream.Position = 0L;
				memoryStream.SetLength(0L);
			}
		}

		public static void SendMessageNoClear(NetClient client)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions sendDataOptions = (SendDataOptions)PRIORITY[num];
			switch (sendDataOptions)
			{
			case SendDataOptions.None:
				sendDataOptions = SendDataOptions.Reliable;
				break;
			case SendDataOptions.InOrder:
				sendDataOptions = SendDataOptions.ReliableInOrder;
				break;
			}
			try
			{
				Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketOut.Length, sendDataOptions, client.NetGamer);
			}
			catch
			{
			}
		}

		public static void ClearMessage()
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			memoryStream.Position = 0L;
			memoryStream.SetLength(0L);
		}

		public static void SendMessageIgnore(NetClient ignoreClient)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions options = (SendDataOptions)PRIORITY[num];
			for (int num2 = Netplay.ClientList.Count - 1; num2 >= 0; num2--)
			{
				NetClient netClient = Netplay.ClientList[num2];
				if (netClient != ignoreClient && netClient.IsReadyToReceive(memoryStream.GetBuffer()))
				{
					try
					{
						Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketOut.Length, options, netClient.NetGamer);
					}
					catch
					{
					}
				}
			}
			memoryStream.Position = 0L;
			memoryStream.SetLength(0L);
		}

		public static void SendMessage(NetworkGamer gamer)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryWriter)(object)PacketOut).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions options = (SendDataOptions)PRIORITY[num];
			try
			{
				Netplay.gamer.SendData(PacketOut, options, gamer);
			}
			catch
			{
				memoryStream.Position = 0L;
				memoryStream.SetLength(0L);
			}
		}

		private static void EchoMessage(NetClient sender)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryReader)(object)PacketIn).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions options = (SendDataOptions)PRIORITY[num];
			for (int num2 = Netplay.ClientList.Count - 1; num2 >= 0; num2--)
			{
				NetClient netClient = Netplay.ClientList[num2];
				if (netClient != sender && netClient.IsReadyToReceive(memoryStream.GetBuffer()))
				{
					try
					{
						Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketIn.Length, options, netClient.NetGamer);
					}
					catch
					{
					}
				}
			}
		}

		private static void EchoProjectileMessage(NetClient sender, ref Projectile projectile)
		{
			MemoryStream memoryStream = (MemoryStream)((BinaryReader)(object)PacketIn).BaseStream;
			int num = memoryStream.GetBuffer()[0];
			SendDataOptions options = (SendDataOptions)PRIORITY[num];
			for (int num2 = Netplay.ClientList.Count - 1; num2 >= 0; num2--)
			{
				NetClient netClient = Netplay.ClientList[num2];
				if (netClient != sender && netClient.IsReadyToReceiveProjectile(ref projectile))
				{
					try
					{
						Netplay.gamer.SendData(memoryStream.GetBuffer(), 0, PacketIn.Length, options, netClient.NetGamer);
					}
					catch
					{
					}
				}
			}
		}

		public unsafe static void GetData(NetClient sender)
		{
			int num = ((BinaryReader)(object)PacketIn).ReadByte();
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				UI main = UI.MainUI;
				if (Netplay.clientStatusMax > 0)
				{
					Netplay.clientStatusCount++;
				}
				switch (num)
				{
				case 2:
					Netplay.PlayDisconnect = true;
					main.statusText = Lang.MiscText[((BinaryReader)(object)PacketIn).ReadUInt16()];
					break;
				case 3:
				{
					if (Netplay.clientState == Netplay.ClientState.WAITING_FOR_PLAYER_DATA_REQ)
					{
						Netplay.clientState = Netplay.ClientState.RECEIVED_PLAYER_DATA_REQ;
					}
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					CreateMessage1(4, num2);
					SendMessage();
					CreateMessage1(16, num2);
					SendMessage();
					CreateMessage1(42, num2);
					SendMessage();
					CreateMessage1(50, num2);
					SendMessage();
					for (int k = 0; k < 49; k++)
					{
						CreateMessage2(5, num2, k);
						SendMessage();
					}
					for (int l = 0; l < 11; l++)
					{
						CreateMessage2(5, num2, l + 49);
						SendMessage();
					}
					CreateMessage0(6);
					SendMessage();
					break;
				}
				case 7:
				{
					Main.GameTime.WorldTime = ((BinaryReader)(object)PacketIn).ReadSingle();
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					Main.GameTime.DayTime = (num2 & 1) != 0;
					Main.GameTime.IsBloodMoon = (num2 & 2) != 0;
					Main.GameTime.MoonPhase = (byte)(num2 >> 2);
					Main.MaxTilesX = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.MaxTilesY = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.SpawnTileX = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.SpawnTileY = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.WorldSurface = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.WorldSurfacePixels = Main.WorldSurface << 4;
					Main.RockLayer = ((BinaryReader)(object)PacketIn).ReadInt16();
					Main.RockLayerPixels = Main.RockLayer << 4;
					num2 = ((BinaryReader)(object)PacketIn).ReadInt32();
					if (num2 != Main.WorldID)
					{
						Main.WorldID = num2;
						Main.CheckWorldId = true;
					}
					num2 = ((BinaryReader)(object)PacketIn).ReadInt32();
					if (num2 != Main.WorldTimestamp)
					{
						Main.WorldTimestamp = num2;
						Main.CheckWorldId = true;
					}
					num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					WorldGen.HasShadowOrbSmashed = (num2 & 1) != 0;
					NPC.HasDownedBoss1 = (num2 & 2) != 0;
					NPC.HasDownedBoss2 = (num2 & 4) != 0;
					NPC.HasDownedBoss3 = (num2 & 8) != 0;
					Main.InHardMode = (num2 & 0x10) != 0;
					NPC.HasDownedClown = (num2 & 0x20) != 0;
					Main.WorldName = ((BinaryReader)(object)PacketIn).ReadString();
#if (!VERSION_INITIAL || IS_PATCHED)
					WorldGen.setWorldSize();
#endif

					WorldGen.UpdateMagmaLayerPos();
					if (Netplay.clientState <= Netplay.ClientState.WAITING_FOR_WORLD_INFO)
					{
						Netplay.clientState = Netplay.ClientState.ANNOUNCING_SPAWN_LOCATION;
						UI.MainUI.NextProgressStep(Lang.MenuText[74]);
					}
					break;
				}
				case 9:
					Netplay.clientStatusMax += ((BinaryReader)(object)PacketIn).ReadByte();
					break;
				case 10:
				{
					int num11 = ((BinaryReader)(object)PacketIn).ReadByte() * 40;
					int num12 = ((BinaryReader)(object)PacketIn).ReadByte() * 30;
					fixed (Tile* ptr2 = Main.TileSet)
					{
						uint num13 = 0;
						Tile* ptr = null;
						for (int m = num11; m < num11 + 40; m++)
						{
							Tile* ptr3 = ptr2 + (num12 + m * Main.LargeWorldH);
							for (int num14 = 29; num14 >= 0; num14--)
							{
								if (num13 != 0)
								{
									num13--;
									*ptr3 = *ptr;
								}
								else
								{
									ptr = ptr3;
									int num15 = ((BinaryReader)(object)PacketIn).ReadByte();
									int num16 = num15 & 1;
									if (num16 != 0)
									{
										int type = ptr3->Type;
										int num17 = (ptr3->Type = ((BinaryReader)(object)PacketIn).ReadByte());
										if (Main.TileFrameImportant[num17])
										{
											ptr3->FrameX = (short)ReadCompacted();
											ptr3->FrameY = (short)ReadCompacted();
										}
										else if (num17 != type || ptr3->IsActive == 0)
										{
											ptr3->FrameX = -1;
											ptr3->FrameY = -1;
										}
									}
									ptr3->IsActive = (byte)num16;
#if (!VERSION_INITIAL || IS_PATCHED)
                                    ptr3->WallType = (byte)((((uint)num15 & 8u) != 0) ? ((BinaryReader)(object)PacketIn).ReadByte() : 0);
                                    if (((uint)num15 & 2u) != 0)
                                    {
                                        ptr3->Liquid = byte.MaxValue;
                                    }
                                    else
                                    {
                                        ptr3->Liquid = (byte)((((uint)num15 & 4u) != 0) ? ((BinaryReader)(object)PacketIn).ReadByte() : 0);
                                    }
#else
									ptr3->WallType = (byte)((((uint)num15 & 4u) != 0) ? ((BinaryReader)(object)PacketIn).ReadByte() : 0);
									ptr3->Liquid = (byte)((((uint)num15 & 8u) != 0) ? ((BinaryReader)(object)PacketIn).ReadByte() : 0);
#endif
                                            ptr3->wire = num15 & 0x10;
									ptr3->Lava = (byte)((uint)num15 & 0x20u);
									num13 = ReadCompacted();
								}
								ptr3++;
							}
						}
					}
					WorldGen.SectionTileFrame(num11, num12);
					break;
				}
				case 11:
				{
					int num5 = ((BinaryReader)(object)PacketIn).ReadByte();
					GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
					if (num5 != ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count)
					{
						CreateMessage0(11);
						SendMessage();
						break;
					}
					do
					{
						NetworkGamer networkGamer = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[--num5];
						int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
						Player player = Main.PlayerSet[num2];
						_ = networkGamer.IsLocal;
						networkGamer.Tag = player;
					}
					while (num5 > 0);
					break;
				}
				case 14:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int num3 = num2 & 0x80;
					num2 ^= num3;
					Player player = Main.PlayerSet[num2];
					if (num3 != 0)
					{
						if (player.Active == 0)
						{
							player.Init();
							player.Active = 1;
						}
						Netplay.SetAsRemotePlayerSlot(num2);
					}
					else
					{
						player.Active = 0;
					}
					break;
				}
				case 23:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int num6 = (int)ReadCompacted();
					Main.NPCSet[num2].Life = num6;
					if (num6 == 0)
					{
						Main.NPCSet[num2].Active = 0;
						break;
					}
					int num7 = ((BinaryReader)(object)PacketIn).ReadInt16();
					if (Main.NPCSet[num2].Active == 0 || Main.NPCSet[num2].NetID != num7)
					{
						Main.NPCSet[num2].NetDefaults(num7);
					}
					Main.NPCSet[num2].Position = PacketIn.ReadVector2();
					Main.NPCSet[num2].XYWH.X = (int)Main.NPCSet[num2].Position.X;
					Main.NPCSet[num2].XYWH.Y = (int)Main.NPCSet[num2].Position.Y;
					HalfVector2 halfVector = default(HalfVector2);
					halfVector.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
					Main.NPCSet[num2].Velocity = halfVector.ToVector2();
					int num8 = ((BinaryReader)(object)PacketIn).ReadSByte();
					Main.NPCSet[num2].Target = (byte)((uint)num8 & 0xFu);
					Main.NPCSet[num2].Direction = (sbyte)(num8 << 26 >> 30);
					Main.NPCSet[num2].DirectionY = (sbyte)(num8 >> 6);
					int num9 = ((BinaryReader)(object)PacketIn).ReadByte();
					Main.NPCSet[num2].AI0 = ((((uint)num9 & (true ? 1u : 0u)) != 0) ? ((BinaryReader)(object)PacketIn).ReadSingle() : 0f);
					Main.NPCSet[num2].AI1 = ((((uint)num9 & 2u) != 0) ? ((BinaryReader)(object)PacketIn).ReadSingle() : 0f);
					Main.NPCSet[num2].AI2 = ((((uint)num9 & 4u) != 0) ? ((BinaryReader)(object)PacketIn).ReadSingle() : 0f);
					Main.NPCSet[num2].AI3 = ((((uint)num9 & 8u) != 0) ? ((BinaryReader)(object)PacketIn).ReadSingle() : 0f);
					break;
				}
				case 18:
				case 25:
				case 37:
				case 38:
				case 63:
				{
					byte r = ((BinaryReader)(object)PacketIn).ReadByte();
					byte g = ((BinaryReader)(object)PacketIn).ReadByte();
					byte b = ((BinaryReader)(object)PacketIn).ReadByte();
					uint num10 = 0;
					string text;
					if (num == 18)
					{
						num10 = ((BinaryReader)(object)PacketIn).ReadUInt16();
						text = Lang.MiscText[num10];
					}
					else if (num == 63)
					{
						num10 = ((BinaryReader)(object)PacketIn).ReadUInt32();
						text = ((BinaryReader)(object)PacketIn).ReadString();
						text += Lang.DeathMsgString(num10);
					}
					else
					{
						if (num != 25)
						{
							num10 = ((BinaryReader)(object)PacketIn).ReadUInt16();
						}
						text = ((BinaryReader)(object)PacketIn).ReadString();
						switch (num)
						{
						case 37:
							text += Lang.MiscText[num10];
							break;
						case 38:
							text = Lang.MiscText[num10] + text;
							break;
						}
					}
					Main.NewText(text, r, g, b);
					break;
				}
				case 49:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					for (int j = 0; j < 4; j++)
					{
						UI uI = Main.UIInstance[j];
						if (uI.localGamer != null && uI.MyPlayer == num2)
						{
							Netplay.gamersWaitingToSpawn.Add(uI);
							break;
						}
					}
					if (Netplay.clientState >= Netplay.ClientState.WAITING_FOR_TILE_DATA)
					{
						Netplay.clientState = Netplay.ClientState.PLAYING;
					}
					break;
				}
				case 54:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					for (int i = 0; i < 5; i++)
					{
						uint num4 = ((BinaryReader)(object)PacketIn).ReadByte();
						Main.NPCSet[num2].ActiveBuffs[i].Type = (ushort)num4;
						Main.NPCSet[num2].ActiveBuffs[i].Time = (ushort)((num4 != 0) ? ReadCompacted() : 0u);
					}
					break;
				}
				case (int)SendDataId.SERVER_NPC_NAMES:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					NPC.TypeNames[num2] = ((BinaryReader)(object)PacketIn).ReadString();
					break;
				}
				case (int)SendDataId.SERVER_GOOD_EVIL_STATUS:
					WorldGen.GoodCoverage = ((BinaryReader)(object)PacketIn).ReadByte();
					WorldGen.EvilCoverage = ((BinaryReader)(object)PacketIn).ReadByte();
					break;
				case 64:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					UI ui2 = Main.PlayerSet[num2].ui;
					num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					ui2?.SetTriggerState((Trigger)num2);
					break;
				}
				case 65:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					UI ui = Main.PlayerSet[num2].ui;
					num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					ui?.Statistics.IncreaseStat((StatisticEntry)num2);
					break;
				}
				}
			}
			else if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				switch (num)
				{
				case 1:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					if (sender.ServerState == 0)
					{
#if (!VERSION_INITIAL || IS_PATCHED)
                        if (num2 != 2)
#else
						if (num2 != 1)
#endif
						{
							num2 = ((BinaryReader)(object)PacketIn).ReadByte();
							BootPlayer(num2, 22);
							break;
						}
						sender.ServerState = 1;
					}
					num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					SendPlayerInfoRequest(sender, num2);
					break;
				}
				case 6:
					if (sender.ServerState == 1)
					{
						sender.ServerState = 2;
					}
					CreateMessage0(7);
					SendMessage(sender);
					break;
				case 8:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int num19 = ((BinaryReader)(object)PacketIn).ReadInt16();
					int num20 = ((BinaryReader)(object)PacketIn).ReadInt16();
					bool flag2 = num19 >= 0 && num20 >= 0;
					if (flag2)
					{
						if (num19 < 10 || num19 > Main.MaxTilesX - 10)
						{
							flag2 = false;
						}
						else if (num20 < 10 || num20 > Main.MaxTilesY - 10)
						{
							flag2 = false;
						}
					}
					int num21 = 9;
					if (flag2)
					{
						num21 <<= 1;
					}
					if (sender.ServerState == 2)
					{
						sender.ServerState = 3;
					}
					CreateMessage1(9, num21);
					SendMessage(sender);
					int sectionX = Main.SpawnTileX / 40;
					int sectionY = Main.SpawnTileY / 30;
					SendSectionSquare(sender, sectionX, sectionY, 3);
					if (flag2)
					{
						num19 /= 40;
						num20 /= 30;
						SendSectionSquare(sender, num19, num20, 3);
					}
					for (int num22 = 0; num22 < Main.MaxNumItems; num22++)
					{
						if (Main.ItemSet[num22].Active != 0)
						{
							CreateMessage2(21, UI.MainUI.MyPlayer, num22);
							SendMessage(sender);
							CreateMessage1(22, num22);
							SendMessage(sender);
						}
					}
					for (int num23 = 0; num23 < NPC.MaxNumNPCs; num23++)
					{
						NPC nPC = Main.NPCSet[num23];
						if (nPC.Active != 0)
						{
							if (nPC.IsTownNPC)
							{
								int sectionX2 = nPC.XYWH.X / 640;
								int sectionY2 = nPC.XYWH.X / 640;
								SendSectionSquare(sender, sectionX2, sectionY2, 3);
							}
							CreateMessage1((int)SendDataId.SERVER_WORLD_NPC, num23);
							SendMessage(sender);
						}
					}
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.MERCHANT);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.NURSE);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.ARMS_DEALER);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.DRYAD);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.GUIDE);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.DEMOLITIONIST);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.CLOTHIER);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.GOBLIN_TINKERER);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.WIZARD);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, (int)NPC.ID.MECHANIC);
					SendMessage(sender);
					CreateMessage0((int)SendDataId.SERVER_GOOD_EVIL_STATUS);
					SendMessage(sender);
					CreateMessage1((int)SendDataId.SERVER_WORLD_DONE, num2);
					SendMessage(sender);
					break;
				}
				case 11:
					CreateMessage0(11);
					SendMessage(sender);
					break;
				case 31:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					Player player = Main.PlayerSet[num2];
					int x = ((BinaryReader)(object)PacketIn).ReadUInt16();
					int y = ((BinaryReader)(object)PacketIn).ReadUInt16();
					int num26 = Chest.FindChest(x, y);
					if (num26 >= 0 && Chest.UsingChest(num26) == -1)
					{
						for (int num27 = 0; num27 < 20; num27++)
						{
							CreateMessage2(32, num26, num27);
							SendMessage(sender);
						}
						CreateMessage2(33, num2, num26);
						SendMessage(sender);
						player.PlayerChest = (short)num26;
					}
					break;
				}
				case 34:
				{
					int num24 = ((BinaryReader)(object)PacketIn).ReadUInt16();
					int num25 = ((BinaryReader)(object)PacketIn).ReadUInt16();
					if (Main.TileSet[num24, num25].Type == 21 && WorldGen.KillTile(num24, num25))
					{
						CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num24, num25, 0);
						SendMessage();
					}
					break;
				}
				case 46:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int i2 = ((BinaryReader)(object)PacketIn).ReadUInt16();
					int j2 = ((BinaryReader)(object)PacketIn).ReadUInt16();
					int num28 = Sign.ReadSign(i2, j2);
					if (num28 >= 0)
					{
						CreateMessage2(47, num2, num28);
						SendMessage(sender);
					}
					break;
				}
				case 53:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int type2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int time = (int)ReadCompacted();
					Main.NPCSet[num2].AddBuff(type2, time, quiet: true);
					CreateMessage1(54, num2);
					SendMessage();
					break;
				}
				case 61:
				{
					int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
					int num18 = ((BinaryReader)(object)PacketIn).ReadInt16();
					if (num18 < 0)
					{
						if (Main.InvasionType == 0)
						{
							Main.InvasionDelay = 0;
							Main.StartInvasion(-num18);
						}
						break;
					}
					bool flag = true;
					for (int n = 0; n < NPC.MaxNumNPCs; n++)
					{
						if (Main.NPCSet[n].Type == num18 && Main.NPCSet[n].Active != 0)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						NPC.SpawnOnPlayer(Main.PlayerSet[num2], num18);
					}
					break;
				}
				case 62:
					NPC.SpawnSkeletron();
					break;
				case 66:
					sender.RequestedPublicSlot();
					break;
				case 67:
					sender.CanceledPublicSlot();
					break;
				}
			}
			switch (num)
			{
			case 4:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				Player player = Main.PlayerSet[num2 & 7];
				num2 >>= 4;
				player.hair = (byte)((uint)num2 & 0x3Fu);
				num2 >>= 6;
				player.male = (num2 & 1) != 0;
				num2 >>= 1;
				player.difficulty = (byte)num2;
				player.hairColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.hairColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.hairColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.skinColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.skinColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.skinColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.eyeColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.eyeColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.eyeColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shirtColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shirtColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shirtColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.underShirtColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.underShirtColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.underShirtColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.pantsColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.pantsColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.pantsColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shoeColor.R = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shoeColor.G = ((BinaryReader)(object)PacketIn).ReadByte();
				player.shoeColor.B = ((BinaryReader)(object)PacketIn).ReadByte();
				player.oldName = player.Name;
				player.Name = ((BinaryReader)(object)PacketIn).ReadString();
				break;
			}
			case 5:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				int num65 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num66 = ((BinaryReader)(object)PacketIn).ReadByte();
				int pre3 = 0;
				int type6 = 0;
				if (num66 > 0)
				{
					pre3 = ((BinaryReader)(object)PacketIn).ReadByte();
					type6 = ((BinaryReader)(object)PacketIn).ReadInt16();
				}
				if (num65 < 49)
				{
					if (num66 > 0)
					{
						player.Inventory[num65].NetDefaults(type6, num66);
						player.Inventory[num65].SetPrefix(pre3);
					}
					else
					{
						player.Inventory[num65].Init();
					}
					break;
				}
				num65 -= 49;
				if (num66 > 0)
				{
					player.armor[num65].NetDefaults(type6, num66);
					player.armor[num65].SetPrefix(pre3);
				}
				else
				{
					player.armor[num65].Init();
				}
				break;
			}
			case 12:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					if (player.ui != null)
					{
						player.ui.SetPlayer(null);
					}
				}
				else
				{
					EchoMessage(sender);
				}
				player.SpawnX = ((BinaryReader)(object)PacketIn).ReadInt16();
				player.SpawnY = ((BinaryReader)(object)PacketIn).ReadInt16();
				player.Spawn();
				if (Main.NetMode == (byte)NetModeSetting.SERVER && sender.ServerState >= 3)
				{
					if (sender.ServerState == 3)
					{
						sender.ServerState = 10;
						greetPlayer(num2);
						syncPlayers();
					}
					else
					{
						SyncPlayer(num2);
					}
				}
				break;
			}
			case 13:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
#if (!VERSION_INITIAL || IS_PATCHED)
				Player player = Main.PlayerSet[num2 & 0x1F];
#else
				Player player = Main.PlayerSet[num2 & 0x3F];
#endif
				player.direction = (sbyte)((((uint)num2 & 0x40u) != 0) ? 1 : (-1));
				int num49 = ((((uint)num2 & 0x80u) != 0) ? ((BinaryReader)(object)PacketIn).ReadByte() : 0);
				player.controlUp = (num49 & 1) != 0;
				player.IsControlDown = (num49 & 2) != 0;
				player.controlLeft = (num49 & 4) != 0;
				player.controlRight = (num49 & 8) != 0;
				player.controlJump = (num49 & 0x10) != 0;
				player.controlUseItem = (num49 & 0x20) != 0;
#if (!VERSION_INITIAL || IS_PATCHED)
				if (((uint)num2 & 0x20u) != 0)
#else
				if (((uint)num49 & 0x20u) != 0)
#endif
				{
					player.SelectedItem = ((BinaryReader)(object)PacketIn).ReadSByte();
				}
				player.Position = PacketIn.ReadVector2();
				player.XYWH.X = (int)player.Position.X;
				player.XYWH.Y = (int)player.Position.Y;
				HalfVector2 halfVector5 = default(HalfVector2);
				halfVector5.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
				player.velocity = halfVector5.ToVector2();
				player.fallStart = (short)(player.XYWH.Y >> 4);
				if (Main.NetMode == (byte)NetModeSetting.SERVER && sender.ServerState == 10)
				{
					EchoMessage(sender);
				}
				break;
			}
			case 15:
			{
				int num38 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num39 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num40 = ((BinaryReader)(object)PacketIn).ReadByte();
				int active = Main.TileSet[num38, num39].IsActive;
				Main.TileSet[num38, num39].IsActive = (byte)((uint)num40 & 1u);
				Main.TileSet[num38, num39].wire = num40 & 0x10;
				if (Main.TileSet[num38, num39].IsActive != 0)
				{
					int type3 = Main.TileSet[num38, num39].Type;
					int num41 = (Main.TileSet[num38, num39].Type = ((BinaryReader)(object)PacketIn).ReadByte());
					if (Main.TileFrameImportant[num41])
					{
						Main.TileSet[num38, num39].FrameX = (short)ReadCompacted();
						Main.TileSet[num38, num39].FrameY = (short)ReadCompacted();
					}
					else if (active == 0 || num41 != type3)
					{
						Main.TileSet[num38, num39].FrameX = -1;
						Main.TileSet[num38, num39].FrameY = -1;
					}
				}
				if (((uint)num40 & 4u) != 0)
				{
					Main.TileSet[num38, num39].WallType = ((BinaryReader)(object)PacketIn).ReadByte();
				}
				if (Main.NetMode != (byte)NetModeSetting.SERVER && ((uint)num40 & 8u) != 0)
				{
					Main.TileSet[num38, num39].Lava = (byte)((uint)num40 & 0x20u);
					Main.TileSet[num38, num39].Liquid = ((BinaryReader)(object)PacketIn).ReadByte();
				}
				WorldGen.TileFrame(num38, num39);
				WorldGen.WallFrame(num38, num39);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					CreateMessage2(15, num38, num39);
					SendMessageIgnore(sender);
				}
				break;
			}
			case 16:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadInt32();
				Player player = Main.PlayerSet[num2 & 0xF];
				int num33 = num2 << 16 >> 20;
				player.statLife = (short)num33;
				if (num33 <= 0)
				{
					player.IsDead = true;
				}
				player.StatLifeMax = (short)(num2 >> 16);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				break;
			}
			case 17:
			{
				int num60 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num61 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num62 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num63 = ((num60 > 4) ? 1 : ((BinaryReader)(object)PacketIn).ReadByte());
				bool flag5 = num63 == 1;
				int num64 = 0;
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					if (!flag5 && (num60 == 0 || num60 == 2 || num60 == 4) && !sender.TileSectionsTaken[num61 / 40, num62 / 30])
					{
						flag5 = true;
					}
					EchoMessage(sender);
				}
				switch (num60)
				{
				case 0:
					WorldGen.KillTile(num61, num62, flag5);
					break;
				case 1:
					num64 = ((BinaryReader)(object)PacketIn).ReadByte();
					WorldGen.PlaceTile(num61, num62, num63, ToMute: false, IsForced: true, -1, num64);
					if (num63 == 53 && Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						SendTile(num61, num62);
					}
					break;
				case 2:
					WorldGen.KillWall(num61, num62, flag5);
					break;
				case 3:
					WorldGen.PlaceWall(num61, num62, num63);
					break;
				case 4:
					WorldGen.KillTile(num61, num62, flag5, EffectOnly: false, noItem: true);
					break;
				case 5:
					WorldGen.PlaceWire(num61, num62);
					break;
				case 6:
					WorldGen.KillWire(num61, num62);
					break;
				}
				break;
			}
			case 19:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int i5 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int j5 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int direction = ((BinaryReader)(object)PacketIn).ReadSByte();
				WorldGen.OpenDoor(i5, j5, direction);
				break;
			}
			case 20:
			{
				int num52 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num53 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num54 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				for (int num55 = num53; num55 < num53 + num52; num55++)
				{
					for (int num56 = num54; num56 < num54 + num52; num56++)
					{
						int num57 = ((BinaryReader)(object)PacketIn).ReadByte();
						int active2 = Main.TileSet[num55, num56].IsActive;
						Main.TileSet[num55, num56].IsActive = (byte)((uint)num57 & 1u);
						Main.TileSet[num55, num56].wire = num57 & 0x10;
						if (Main.TileSet[num55, num56].IsActive != 0)
						{
							int type5 = Main.TileSet[num55, num56].Type;
							int num58 = (Main.TileSet[num55, num56].Type = ((BinaryReader)(object)PacketIn).ReadByte());
							if (Main.TileFrameImportant[num58])
							{
								Main.TileSet[num55, num56].FrameX = (short)ReadCompacted();
								Main.TileSet[num55, num56].FrameY = (short)ReadCompacted();
							}
							else if (active2 == 0 || num58 != type5)
							{
								Main.TileSet[num55, num56].FrameX = -1;
								Main.TileSet[num55, num56].FrameY = -1;
							}
						}
						if (((uint)num57 & 4u) != 0)
						{
							Main.TileSet[num55, num56].WallType = ((BinaryReader)(object)PacketIn).ReadByte();
						}
						if (Main.NetMode != (byte)NetModeSetting.SERVER && ((uint)num57 & 8u) != 0)
						{
							Main.TileSet[num55, num56].Lava = (byte)((uint)num57 & 0x20u);
							Main.TileSet[num55, num56].Liquid = ((BinaryReader)(object)PacketIn).ReadByte();
						}
					}
				}
				WorldGen.RangeFrame(num53, num54, num53 + num52, num54 + num52);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					CreateMessage3(20, num52, num53, num54);
					SendMessageIgnore(sender);
				}
				break;
			}
			case 21:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num45 = ((BinaryReader)(object)PacketIn).ReadInt16();
				int num46 = num45 & 0x1F;
				num45 >>= 5;
#if (!VERSION_INITIAL || IS_PATCHED)
				int owner = Main.ItemSet[num2].Owner;
#endif
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					if (num45 == 0)
					{
						Main.ItemSet[num2].Active = 0;
						break;
					}
					Main.ItemSet[num2].NetDefaults(num45);
					Main.ItemSet[num2].SetPrefix(((BinaryReader)(object)PacketIn).ReadByte());
					Main.ItemSet[num2].Stack = ((BinaryReader)(object)PacketIn).ReadByte();
					Main.ItemSet[num2].Position = PacketIn.ReadVector2();
					HalfVector2 halfVector3 = default(HalfVector2);
					halfVector3.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
					Main.ItemSet[num2].Velocity = halfVector3.ToVector2();
					Main.ItemSet[num2].IsWet = Collision.WetCollision(ref Main.ItemSet[num2].Position, Main.ItemSet[num2].Width, Main.ItemSet[num2].Height);
#if (!VERSION_INITIAL || IS_PATCHED)
					Main.ItemSet[num2].Owner = (byte)owner;
#endif
					break;
				}
				if (num45 == 0)
				{
					Main.ItemSet[num2].Active = 0;
					CreateMessage2(21, num46, num2);
					SendMessage();
					break;
				}
				int pre2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int stack2 = ((BinaryReader)(object)PacketIn).ReadByte();
				float num47 = ((BinaryReader)(object)PacketIn).ReadSingle();
				float num48 = ((BinaryReader)(object)PacketIn).ReadSingle();
				bool flag4 = num2 == Main.MaxNumItems;
				if (flag4)
				{
					Item item = default(Item);
					item.NetDefaults(num45, stack2);
					num2 = (short)Item.NewItem((int)num47, (int)num48, item.Width, item.Height, item.Type, stack2, DoNotBroadcast: true);
				}
				else
				{
					Main.ItemSet[num2].Position.X = num47;
					Main.ItemSet[num2].Position.Y = num48;
				}
				Main.ItemSet[num2].NetDefaults(num45, stack2);
				Main.ItemSet[num2].SetPrefix(pre2);
				HalfVector2 halfVector4 = default(HalfVector2);
				halfVector4.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
				Main.ItemSet[num2].Velocity = halfVector4.ToVector2();
#if (!IS_PATCHED && VERSION_INITIAL)
				Main.ItemSet[num2].Owner = 8;
#endif
				CreateMessage2(21, num46, num2);
				if (flag4)
				{
					SendMessage();
					Main.ItemSet[num2].OwnIgnore = (byte)num46;
					Main.ItemSet[num2].OwnTime = 100;
				}
				else
				{
#if (!VERSION_INITIAL || IS_PATCHED)
					Main.ItemSet[num2].Owner = (byte)owner;
#endif
					SendMessageIgnore(Main.PlayerSet[num46].client);
				}
				break;
			}
			case 22:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num34 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num35 = num34 & 0x80;
				num34 ^= num35;
				Main.ItemSet[num2].Owner = (byte)num34;
				if (num34 < 8)
				{
					Main.ItemSet[num2].KeepTime = 15;
					Main.ItemSet[num2].Position = PacketIn.ReadVector2();
					if (num35 != 0)
					{
						HalfVector2 halfVector2 = default(HalfVector2);
						halfVector2.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
						Main.ItemSet[num2].Velocity = halfVector2.ToVector2();
					}
					else
					{
						Main.ItemSet[num2].Velocity.X = 0f;
						Main.ItemSet[num2].Velocity.Y = 0f;
					}
				}
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				break;
			}
			case 24:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int i4 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int j4 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				WorldGen.CloseDoor(i4, j4, forced: true);
				break;
			}
			case 26:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int hitDirection2 = ((BinaryReader)(object)PacketIn).ReadSByte();
				int damage = ((BinaryReader)(object)PacketIn).ReadInt16();
#if (!IS_PATCHED && VERSION_INITIAL)
				bool pvp = ((BinaryReader)(object)PacketIn).ReadBoolean();
				bool crit2 = ((BinaryReader)(object)PacketIn).ReadBoolean();
				uint deathText2 = ((BinaryReader)(object)PacketIn).ReadUInt32();
				Main.PlayerSet[num2].Hurt(damage, hitDirection2, pvp, quiet: true, deathText2, crit2);
#else
                bool pvp = (num2 & 0x40) != 0;
                bool crit2 = (num2 & 0x80) != 0;
                uint deathText2 = ((BinaryReader)(object)PacketIn).ReadUInt32();
                Main.PlayerSet[num2 & 7].Hurt(damage, hitDirection2, pvp, quiet: true, deathText2, crit2);
#endif
                break;
			}
			case 27:
			{
				int num73 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num74 = num73 >> 4;
				num73 &= 0xF;
				int num75 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num2 = (int)ReadCompacted();
				int num76 = Projectile.MaxNumProjs;
				for (int num77 = 0; num77 < Projectile.MaxNumProjs; num77++)
				{
					if (Main.ProjectileSet[num77].owner == num73 && Main.ProjectileSet[num77].identity == num2 && Main.ProjectileSet[num77].active != 0)
					{
						num76 = num77;
						break;
					}
				}
				if (num76 == Projectile.MaxNumProjs)
				{
					for (int num78 = 0; num78 < Projectile.MaxNumProjs; num78++)
					{
						if (Main.ProjectileSet[num78].active == 0)
						{
							num76 = num78;
							break;
						}
					}
				}
				if (Main.ProjectileSet[num76].active == 0 || Main.ProjectileSet[num76].type != num75)
				{
					Main.ProjectileSet[num76].SetDefaults(num75);
				}
				Main.ProjectileSet[num76].type = (byte)num75;
				Main.ProjectileSet[num76].owner = (byte)num73;
				Main.ProjectileSet[num76].identity = (ushort)num2;
				Main.ProjectileSet[num76].position = PacketIn.ReadVector2();
				Main.ProjectileSet[num76].XYWH.X = (int)Main.ProjectileSet[num76].position.X;
				Main.ProjectileSet[num76].XYWH.Y = (int)Main.ProjectileSet[num76].position.Y;
				HalfVector2 halfVector6 = default(HalfVector2);
				halfVector6.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt32();
				Main.ProjectileSet[num76].velocity = halfVector6.ToVector2();
				if (((uint)num74 & (true ? 1u : 0u)) != 0)
				{
					HalfSingle halfSingle2 = default(HalfSingle);
					halfSingle2.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt16();
					Main.ProjectileSet[num76].knockBack = halfSingle2.ToSingle();
				}
				else
				{
					Main.ProjectileSet[num76].knockBack = 0f;
				}
				Main.ProjectileSet[num76].damage = (short)((((uint)num74 & 2u) != 0) ? ((BinaryReader)(object)PacketIn).ReadInt16() : 0);
				Main.ProjectileSet[num76].ai0 = ((((uint)num74 & 4u) != 0) ? ((BinaryReader)(object)PacketIn).ReadSingle() : 0f);
				Main.ProjectileSet[num76].ai1 = ((((uint)num74 & 8u) != 0) ? ((BinaryReader)(object)PacketIn).ReadInt16() : 0);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoProjectileMessage(sender, ref Main.ProjectileSet[num76]);
				}
				break;
			}
			case 28:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num36 = ((BinaryReader)(object)PacketIn).ReadInt16();
				if (num36 >= 0)
				{
					HalfSingle halfSingle = default(HalfSingle);
					halfSingle.PackedValue = ((BinaryReader)(object)PacketIn).ReadUInt16();
					float knockBack = halfSingle.ToSingle();
					int num37 = ((BinaryReader)(object)PacketIn).ReadSByte();
					bool crit = (num37 & 1) != 0;
					num37 >>= 1;
					Main.NPCSet[num2].StrikeNPC(num36, knockBack, num37, crit);
				}
				else
				{
					Main.NPCSet[num2].Life = 0;
					if (Main.NPCSet[num2].Active == 0)
					{
						break;
					}
					Main.NPCSet[num2].HitEffect();
					Main.NPCSet[num2].Active = 0;
				}
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					if (Main.NPCSet[num2].Life <= 0)
					{
						CreateMessage1(23, num2);
						SendMessage();
					}
					else
					{
						Main.NPCSet[num2].ShouldNetUpdate = true;
					}
				}
				break;
			}
			case 29:
			{
				int num2 = (int)ReadCompacted();
				int num81 = ((BinaryReader)(object)PacketIn).ReadByte();
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				for (int num82 = 0; num82 < Projectile.MaxNumProjs; num82++)
				{
					if (Main.ProjectileSet[num82].owner == num81 && Main.ProjectileSet[num82].identity == num2 && Main.ProjectileSet[num82].active != 0)
					{
						Main.ProjectileSet[num82].Kill();
						break;
					}
				}
				break;
			}
			case 30:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num69 = num2 & 0x80;
				num2 ^= num69;
				Player player = Main.PlayerSet[num2];
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
					int textId = ((num69 != 0) ? 24 : 25);
					SendText(player.Name, textId, Main.TeamColors[player.team].R, Main.TeamColors[player.team].G, Main.TeamColors[player.team].B, -1);
				}
				player.hostile = num69 != 0;
				break;
			}
			case 32:
			{
				int num42 = (int)ReadCompacted();
				int num43 = ((BinaryReader)(object)PacketIn).ReadByte();
				if (Main.ChestSet[num42] == null)
				{
					Main.ChestSet[num42] = new Chest();
				}
				int num44 = ((BinaryReader)(object)PacketIn).ReadInt16();
				if (num44 != 0)
				{
					int pre = ((BinaryReader)(object)PacketIn).ReadByte();
					int stack = ((BinaryReader)(object)PacketIn).ReadByte();
					Main.ChestSet[num42].ItemSet[num43].NetDefaults(num44, stack);
					Main.ChestSet[num42].ItemSet[num43].SetPrefix(pre);
				}
				else
				{
					Main.ChestSet[num42].ItemSet[num43].Init();
				}
				break;
			}
			case 33:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadInt16();
				Player player = Main.PlayerSet[num2 & 0x1F];
				num2 >>= 5;
				if (player.isLocal())
				{
					int chest = player.PlayerChest;
					player.PlayerChest = (short)num2;
					if (num2 >= 0)
					{
						player.chestX = ((BinaryReader)(object)PacketIn).ReadInt16();
						player.chestY = ((BinaryReader)(object)PacketIn).ReadInt16();
					}
					if (chest == -1)
					{
						player.ui.OpenInventory();
						Main.PlaySound(10);
					}
					else if (num2 == -1)
					{
						if (player.ui.ActiveInvSection == UI.InventorySection.CHEST)
						{
							player.ui.CloseInventory();
							Main.PlaySound(11);
						}
					}
					else if (chest != num2)
					{
						player.ui.OpenInventory();
						Main.PlaySound(12);
					}
				}
				else
				{
					player.PlayerChest = (short)num2;
					if (num2 >= 0)
					{
						PacketIn.Position += 4;
					}
				}
				break;
			}
			case 35:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int healAmount = (int)ReadCompacted();
				Player player = Main.PlayerSet[num2];
				player.HealEffect(healAmount);
				break;
			}
			case 36:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				int num59 = ((BinaryReader)(object)PacketIn).ReadByte();
				player.ZoneEvil = (num59 & 1) != 0;
				player.ZoneMeteor = (num59 & 2) != 0;
				player.ZoneDungeon = (num59 & 4) != 0;
				player.ZoneJungle = (num59 & 8) != 0;
				player.zoneHoly = (num59 & 0x10) != 0;
				break;
			}
			case 40:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				player.TalkNPC = ((BinaryReader)(object)PacketIn).ReadInt16();
				break;
			}
			case 41:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				player.itemRotation = ((BinaryReader)(object)PacketIn).ReadSingle();
				player.itemAnimation = ((BinaryReader)(object)PacketIn).ReadInt16();
				player.channel = player.Inventory[player.SelectedItem].Channelling;
				break;
			}
			case 42:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				player.statMana = ((BinaryReader)(object)PacketIn).ReadInt16();
				player.statManaMax = ((BinaryReader)(object)PacketIn).ReadInt16();
				break;
			}
			case 43:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int manaAmount = ((BinaryReader)(object)PacketIn).ReadInt16();
				Player player = Main.PlayerSet[num2];
				player.ManaEffect(manaAmount);
				break;
			}
			case 44:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int hitDirection = ((BinaryReader)(object)PacketIn).ReadSByte();
				int num31 = ((BinaryReader)(object)PacketIn).ReadInt16();
				int num32 = ((BinaryReader)(object)PacketIn).ReadByte();
				uint deathText = ((BinaryReader)(object)PacketIn).ReadUInt32();
				Player player = Main.PlayerSet[num2];
				player.KillMe(num31, hitDirection, num32 != 0, deathText);
				break;
			}
			case 45:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num79 = num2 >> 4;
				num2 &= 0xF;
				Player player = Main.PlayerSet[num2];
				int team = player.team;
				player.team = (byte)num79;
				if (Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					break;
				}
				EchoMessage(sender);
				int textId2 = 26 + num79;
				for (int num80 = 0; num80 < 8; num80++)
				{
					if (num80 == num2 || (team > 0 && Main.PlayerSet[num80].team == team) || (num79 > 0 && Main.PlayerSet[num80].team == num79))
					{
						SendText(player.Name, textId2, Main.TeamColors[num79].R, Main.TeamColors[num79].G, Main.TeamColors[num79].B, num80);
					}
				}
				break;
			}
			case 47:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				num2 = (int)ReadCompacted();
				Main.SignSet[num2].Read(PacketIn);
				if (Main.NetMode == (byte)NetModeSetting.CLIENT && num2 != player.sign)
				{
					player.ui.CloseInventory();
					player.TalkNPC = -1;
					player.ui.editSign = false;
					Main.PlaySound(10);
					player.sign = (short)num2;
					player.ui.npcChatText = Main.SignSet[num2].SignString;
				}
				break;
			}
			case 48:
			{
#if (!IS_PATCHED && VERSION_INITIAL)
				int num70 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num71 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num72 = num71 & 0x7FFF;
				Main.TileSet[num70, num72].Liquid = ((BinaryReader)(object)PacketIn).ReadByte();
				Main.TileSet[num70, num72].Lava = (byte)((uint)(num71 >> 10) & 0x20u);
#else
				int num69 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num70 = num69 & -49153;
				int num71 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num72 = num71 & 0x7FFF;
				fixed (Tile* ptr4 = &Main.TileSet[num70, num72])
				{
					if (((uint)num69 & 0x8000u) != 0)
					{
						ptr4->Liquid = ((BinaryReader)(object)PacketIn).ReadByte();
					}
					else if (((uint)num69 & 0x4000u) != 0)
					{
						ptr4->Liquid = byte.MaxValue;
					}
					else
					{
						ptr4->Liquid = 0;
					}
					ptr4->Lava = (byte)((uint)(num71 >> 10) & 0x20u);
				}
#endif
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					WorldGen.SquareTileFrame(num70, num72);
				}
				break;
			}
			case 50:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				for (int num67 = 0; num67 < Player.MaxNumBuffs; num67++)
				{
					int num68 = ((BinaryReader)(object)PacketIn).ReadByte();
					player.buff[num67].Type = (ushort)num68;
					player.buff[num67].Time = (ushort)((num68 > 0) ? 60u : 0u);
				}
				break;
			}
			case 51:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				Player player = Main.PlayerSet[((BinaryReader)(object)PacketIn).ReadByte()];
				Main.PlaySound(2, player.XYWH.X, player.XYWH.Y);
				break;
			}
			case 52:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num50 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int num51 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				Chest.Unlock(num50, num51);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					SendTileSquare(num50, num51, 2);
				}
				break;
			}
			case 55:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				int type4 = ((BinaryReader)(object)PacketIn).ReadByte();
				int time2 = (int)ReadCompacted();
				player.AddBuff(type4, time2);
				break;
			}
			case 58:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				Player player = Main.PlayerSet[num2];
				Main.HarpNote = ((BinaryReader)(object)PacketIn).ReadSingle();
				Main.PlaySound(2, player.XYWH.X, player.XYWH.Y, (player.Inventory[player.SelectedItem].Type == (int)Item.ID.BELL) ? 35 : 26);
				break;
			}
			case 59:
			{
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					EchoMessage(sender);
				}
				int i3 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				int j3 = ((BinaryReader)(object)PacketIn).ReadUInt16();
				WorldGen.HitSwitch(i3, j3);
				break;
			}
			case 60:
			{
				int num2 = ((BinaryReader)(object)PacketIn).ReadByte();
				int num29 = ((BinaryReader)(object)PacketIn).ReadInt16();
				int num30 = ((BinaryReader)(object)PacketIn).ReadInt16();
				bool flag3 = ((BinaryReader)(object)PacketIn).ReadBoolean();
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					Main.NPCSet[num2].IsHomeless = flag3;
					Main.NPCSet[num2].HomeTileX = (short)num29;
					Main.NPCSet[num2].HomeTileY = (short)num30;
				}
				else if (!flag3)
				{
					WorldGen.kickOut(num2);
				}
				else
				{
					WorldGen.moveRoom(num29, num30, num2);
				}
				break;
			}
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 14:
			case 18:
			case 23:
			case 25:
			case 31:
			case 34:
			case 37:
			case 38:
			case 39:
			case 46:
			case 49:
			case 53:
			case 54:
			case 56:
			case 57:
				break;
			}
		}

		public static void BootPlayer(int plr, int stringId)
		{
			SendKick(Main.PlayerSet[plr].client, stringId);
			Main.PlayerSet[plr].ToKill = true;
		}

		public static void SendTileSquare(int tileX, int tileY, int size)
		{
			int num = size - 1 >> 1;
			CreateMessage3(20, size, tileX - num, tileY - num);
			SendMessage();
		}

		public static void SendTile(int tileX, int tileY)
		{
			CreateMessage2(15, tileX, tileY);
			SendMessage();
		}

		public static bool SendSection(NetClient client, int sectionX, int sectionY)
		{
			if (sectionX >= 0 && sectionY >= 0 && sectionX < Main.MaxSectionsX && sectionY < Main.MaxSectionsY && !client.TileSectionsTaken[sectionX, sectionY])
			{
				client.TileSectionsTaken[sectionX, sectionY] = true;
				CreateMessage2(10, sectionX, sectionY);
				SendMessage(client);
				return true;
			}
			return false;
		}

		public static void SendSectionSquare(NetClient client, int sectionX, int sectionY, int size)
		{
			int num = size - 1 >> 1;
			for (int i = sectionX - num; i <= sectionX + num; i++)
			{
				for (int j = sectionY - num; j <= sectionY + num; j++)
				{
					SendSection(client, i, j);
				}
			}
		}

		public static void greetPlayer(int plr)
		{
			SendText(31, Main.PlayerSet[plr].Name + "!", 255, 240, 20, plr);
			string text = null;
			for (int i = 0; i < 8; i++)
			{
				if (Main.PlayerSet[i].Active != 0)
				{
#if (!IS_PATCHED && VERSION_INITIAL)
					text = (text != null) ? (text + ", " + Main.PlayerSet[i].Name) : Main.PlayerSet[i].Name;
				}
			}
			text += '.';
#else
#if VERSION_INITIAL
					text = (text != null) ? (text + "\n- " + Main.PlayerSet[i].Name) : ("\n- " + Main.PlayerSet[i].Name);
#else
					text = (text != null) ? (text + "\n- " + Main.PlayerSet[i].CharacterName) : ("\n- " + Main.PlayerSet[i].CharacterName);
#endif
				}
			}
#endif
			SendText(23, text, 255, 240, 20, plr);
		}

		public static void SendWater(int x, int y)
		{
			CreateMessage2(48, x, y);
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				SendMessage();
				return;
			}
			for (int num = Netplay.ClientList.Count - 1; num >= 0; num--)
			{
				NetClient netClient = Netplay.ClientList[num];
				if (netClient.ServerState >= 3)
				{
					int num2 = x / 40;
					int num3 = y / 30;
					if (netClient.TileSectionsTaken[num2, num3])
					{
						SendMessageNoClear(netClient);
					}
				}
			}
			ClearMessage();
		}

		public static void SyncPlayer(int plr)
		{
			Player player = Main.PlayerSet[plr];
			NetClient client = player.client;
			CreateMessage2(14, plr, player.Active);
			SendMessageIgnore(client);
			if (player.Active != 0 && (client == null || client.ServerState == 10))
			{
				CreateMessage1(4, plr);
				SendMessageIgnore(client);
				CreateMessage1(13, plr);
				SendMessageIgnore(client);
				CreateMessage1(16, plr);
				SendMessageIgnore(client);
				CreateMessage1(30, plr);
				SendMessageIgnore(client);
				CreateMessage1(42, plr);
				SendMessageIgnore(client);
				CreateMessage1(45, plr);
				SendMessageIgnore(client);
				CreateMessage1(50, plr);
				SendMessageIgnore(client);
				for (int i = 0; i < 49; i++)
				{
					CreateMessage2(5, plr, i);
					SendMessage();
				}
				for (int j = 0; j < 11; j++)
				{
					CreateMessage2(5, plr, j + 49);
					SendMessage();
				}
				if (!Main.PlayerSet[plr].IsAnnounced)
				{
					Main.PlayerSet[plr].IsAnnounced = true;
					SendText(player.Name, 32, 255, 240, 20, -1);
					player.oldName = player.Name;
				}
			}
			else if (player.IsAnnounced)
			{
				player.IsAnnounced = false;
				SendText(player.oldName, 33, 255, 240, 20, -1);
			}
		}

		public static void syncPlayers()
		{
			for (int i = 0; i < 8; i++)
			{
				SyncPlayer(i);
			}
			for (int j = 0; j < NPC.MaxNumNPCs; j++)
			{
				if (Main.NPCSet[j].Active != 0 && Main.NPCSet[j].IsTownNPC && -1 != Main.NPCSet[j].GetHeadTextureID())
				{
					CreateMessage4(60, j, Main.NPCSet[j].HomeTileX, Main.NPCSet[j].HomeTileY, Main.NPCSet[j].IsHomeless ? 1 : 0);
					SendMessage();
				}
			}
		}
	}
}
