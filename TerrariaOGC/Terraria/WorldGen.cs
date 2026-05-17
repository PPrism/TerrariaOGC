using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using Terraria.Achievements;
using static Terraria.Tile;

namespace Terraria
{
	internal static class WorldGen
	{
		public sealed class FallingSandBuffer
		{
			public int count;

			public Location[] buffer = new Location[64];

			public unsafe void Add(int i, int j)
			{
				fixed (Location* ptr = &buffer[count])
				{
					ptr->X = (short)i;
					ptr->Y = (short)j;
				}
				count++;
			}
		}

		private struct DRoom
		{
			public short X;

			public short Y;

			public short L;

			public short R;

			public short T;

			public short B;

			public float Size;
		}

		private struct DDoor
		{
			public short X;

			public short Y;

			public short Pos;
		}

#if (!VERSION_INITIAL || IS_PATCHED)
		private enum Dir
		{
			LEFT = 1,
			RIGHT = 2,
			LEFT_RIGHT = 3,
			UP = 4,
			DOWN = 8,
			UP_DOWN = 12
		}
#endif

		public const int MaxNumMech = 1000;

		public const int MaxNumWire = 1000;

		public const int MaxNumPumps = 20;

		private const float CorruptionFactorForAchievement = 50f;

		private const float HallowedFactorForAchievement = 50f;

		private const int SAND_BUFFER_SIZE = 64;

		public const int MAX_ROOM_RECURSION = 400;

		public const int MAX_ROOM_TILES = 1900;

		public const int MIN_ROOM_TILES = 60;

		public const int MAX_DROOMS = 100;

		private const int MAX_DDOORS = 300;

		private const int MAX_DPLATS = 300;

		private const int MIN_JCHEST = 6;

		private const int MAX_JCHEST = 9;

		private const double JCHEST_WORLD_SIZE_MODIFIER = 1.2;

		public static int numMechs = 0;

		public static Mech[] mech = new Mech[MaxNumMech];

#if (!VERSION_INITIAL || IS_PATCHED)
		private static WireCheck[] wiresChecked = new WireCheck[MaxNumWire];
		private static Location[] wire = new Location[MaxNumWire];
#else
		public static int numWire = 0;
		public static Location[] wire = new Location[MaxNumWire];
#endif

		public static int numNoWire = 0;

		public static Location[] noWire = new Location[MaxNumWire];

		public static int numInPump = 0;

		public static Location[] inPump = new Location[MaxNumPumps];

		public static int numOutPump = 0;

		public static Location[] outPump = new Location[MaxNumPumps];

		public static int totalEvil = 0;

		public static int totalGood = 0;

		public static int totalSolid = 0;

		public static int totalEvil2 = 0;

		public static int totalGood2 = 0;

		public static int totalSolid2 = 0;

		public static byte EvilCoverage = 0;

		public static byte GoodCoverage = 0;

		public static int totalX = 0;

		public static int totalD = 0;

		private static int currentSandBuffer = 0;

		private static FallingSandBuffer[] sandBuffer = new FallingSandBuffer[2];

		public static volatile bool hardLock = false;

		public static volatile bool saveLock = false;

		private static object padlock = new object();

		public static uint woodSpawned = 0;

		public static int lavaLine;

		public static int WaterLine;

		public static int shadowOrbCount = 0;

		public static int altarCount = 0;

		public static bool ToSpawnEye = false;

		public static bool Gen = false;

		public static bool HasShadowOrbSmashed = false;

		public static bool ToSpawnMeteor = false;

		private static short lastMaxTilesX = 0;

		private static short lastMaxTilesY = 0;

		public static Time tempTime = default;

		private static bool stopDrops = false;

		private static bool mudWall = false;

		private static int grassSpread = 0;

		public static bool noLiquidCheck = false;

		public static bool ToDestroyObject = false;

		public static FastRandom genRand = new FastRandom();

		public static int spawnDelay = 0;

		public static int ToSpawnNPC = 0;

		public static int numRoomTiles;

		public static Location[] room = new Location[MAX_ROOM_TILES];

		public static int roomX1;

		public static int roomX2;

		public static int roomY1;

		public static int roomY2;

		public static bool canSpawn;

		public static bool[] HouseTile = new bool[(short)EntityID.TileID.NUM_TILESETS];

		public static int bestX = 0;

		public static int bestY = 0;

		public static int hiScore = 0;

		public static int dungeonX;

		public static int dungeonY;

		public static Vector2i lastDungeonHall = default;

		public static int numDRooms = 0;

		private static DRoom[] dRoom = new DRoom[MAX_DROOMS];

		private static int numDDoors;

		private static DDoor[] dDoor = new DDoor[MAX_DDOORS];

		private static int numDPlats;

		private static Location[] DPlat = new Location[MAX_DPLATS];

		private static int numJChests = 0;

		private static Location[] JChest = new Location[MAX_JCHEST + 1];

		public static int dEnteranceX = 0;

		public static bool dSurface = false;

		private static double dxStrength1;

		private static double dyStrength1;

		private static double dxStrength2;

		private static double dyStrength2;

		private static int dMinX;

		private static int dMaxX;

		private static int dMinY;

		private static int dMaxY;

		private static int numIslandHouses = 0;

		private static int houseCount = 0;

		private static Location[] fih = new Location[300];

		private static int numMCaves = 0;

		private static Location[] mCave = new Location[300];

		private static int JungleX = 0;

		private static int hellChest = 0;

		private static bool roomTorch;

		private static bool roomDoor;

		public static bool roomChair;

		private static bool roomTable;

		private static bool roomOccupied;

		private static bool roomEvil;

		private static int checkRoomDepth;

		private static bool mergeUp;

		private static bool mergeDown;

		private static bool mergeLeft;

		private static bool mergeRight;

		private static bool tileFrameRecursion = true;

		private static bool mergeUp2;

		private static bool mergeDown2;

		private static bool mergeLeft2;

		private static bool mergeRight2;

		public unsafe static void UpdateSand()
		{
			FallingSandBuffer fallingSandBuffer = sandBuffer[currentSandBuffer];
			int count = fallingSandBuffer.count;
			if (count <= 0)
			{
				return;
			}
			fallingSandBuffer.count = 0;
			currentSandBuffer ^= 1;
			int num = 0;
			do
			{
				int x = fallingSandBuffer.buffer[num].X;
				int y = fallingSandBuffer.buffer[num].Y;
				try
				{
					fixed (Tile* ptr = &Main.TileSet[x, y])
					{
						ptr->IsActive = 0;
						int type;
						switch ((EntityID.TileID)ptr->Type)
						{
							case EntityID.TileID.EBONSAND:
								type = (int)EntityID.ProjectileID.EBONSAND_BALL_FALLING;
								break;
							case EntityID.TileID.PEARLSAND:
								type = (int)EntityID.ProjectileID.PEARL_SAND_BALL_FALLING;
								break;
							case EntityID.TileID.SILT:
								type = (int)EntityID.ProjectileID.SILT_BALL;
								break;
							default:
								type = (int)EntityID.ProjectileID.SAND_BALL_FALLING;
								break;
						}
						int num2 = Projectile.NewProjectile(x * 16 + 8, y * 16 + 8, 0f, 2.5f, type, 10, 0f);
						if (num2 < 0)
						{
							return;
						}
						Main.ProjectileSet[num2].velocity.Y = 0.5f;
						Main.ProjectileSet[num2].position.Y += 2f;
						Main.ProjectileSet[num2].XYWH.Y += 2;
						tileFrameRecursion = false;
						TileFrame(x, y - 1);
						TileFrame(x - 1, y);
						TileFrame(x + 1, y);
						tileFrameRecursion = true;
						NetMessage.SendTile(x, y);
					}
				}
				finally
				{
				}
			}
			while (++num < count);
		}

		public static bool MoveNPC(int x, int y, int n)
		{
			if (!StartRoomCheck(x, y))
			{
				Main.NewText(Lang.InterfaceText[40], 255, 240, 20);
				return false;
			}
			if (!RoomNeeds())
			{
				if (Lang.LangOption <= (int)Lang.ID.ENGLISH)
				{
					int num = 0;
					string[] array = new string[4];
					if (!roomTorch)
					{
						array[num] = "a light source";
						num++;
					}
					if (!roomDoor)
					{
						array[num] = "a door";
						num++;
					}
					if (!roomTable)
					{
						array[num] = "a table";
						num++;
					}
					if (!roomChair)
					{
						array[num] = "a chair";
						num++;
					}
					string text = "";
					for (int i = 0; i < num; i++)
					{
						if (num == 2 && i == 1)
						{
							text += " and ";
						}
						else if (i > 0 && i != num - 1)
						{
							text += ", and ";
						}
						else if (i > 0)
						{
							text += ", ";
						}
						text += array[i];
					}
					Main.NewText("This housing is missing " + text + ".", 255, 240, 20);
				}
				else
				{
					Main.NewText(Lang.InterfaceText[8], 255, 240, 20); // They lazy with other languages ngl.
				}
				return false;
			}
			ScoreRoom();
			if (hiScore <= 0)
			{
				if (roomOccupied)
				{
					Main.NewText(Lang.InterfaceText[41], 255, 240, 20);
				}
				else if (roomEvil)
				{
					Main.NewText(Lang.InterfaceText[42], 255, 240, 20);
				}
				else
				{
					Main.NewText(Lang.InterfaceText[8], 255, 240, 20);
				}
				return false;
			}
			return true;
		}

		public static void moveRoom(int x, int y, int n)
		{
			if (Main.NetMode >= (int)NetModeSetting.CLIENT)
			{
				NetMessage.CreateMessage4(60, n, x, y, 1);
				NetMessage.SendMessage();
			}
			else
			{
				ToSpawnNPC = Main.NPCSet[n].Type;
				Main.NPCSet[n].IsHomeless = true;
				SpawnNPC(x, y);
			}
		}

		public static void kickOut(int n)
		{
			if (Main.NetMode >= (int)NetModeSetting.CLIENT)
			{
				NetMessage.CreateMessage4(60, n, 0, 0, 0);
				NetMessage.SendMessage();
			}
			else
			{
				Main.NPCSet[n].IsHomeless = true;
			}
		}

		public static void SpawnNPC(int x, int y)
		{
			if (Main.WallHouse[Main.TileSet[x, y].WallType])
			{
				canSpawn = true;
			}
			else if (!canSpawn)
			{
				return;
			}
			if (!StartRoomCheck(x, y) || !RoomNeeds())
			{
				return;
			}
			ScoreRoom();
			if (hiScore <= 0)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < NPC.MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active != 0 && Main.NPCSet[i].IsHomeless && Main.NPCSet[i].Type == ToSpawnNPC)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				int num2 = bestX;
				int num3 = bestY;
				bool flag = false;
				if (!flag)
				{
					flag = true;
					Rectangle value = new Rectangle(num2 * 16 + 8 - (NPC.SpawnWidth / 2) - NPC.SafeRangeX, num3 * 16 + 8 - (NPC.SpawnHeight / 2) - NPC.SafeRangeY, NPC.SpawnWidth + (NPC.SafeRangeX * 2), NPC.SpawnHeight + (NPC.SafeRangeY * 2));
					for (int j = 0; j < Player.MaxNumPlayers; j++)
					{
						if (Main.PlayerSet[j].Active != 0 && Main.PlayerSet[j].XYWH.Intersects(value))
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag && num3 <= Main.WorldSurface)
				{
					for (int k = 1; k < 500; k++)
					{
						for (int l = 0; l < 2; l++)
						{
							num2 = ((l != 0) ? (bestX - k) : (bestX + k));
							if (num2 > 10 && num2 < Main.MaxTilesX - 10)
							{
								int num4 = bestY - k;
								double num5 = bestY + k;
								if (num4 < 10)
								{
									num4 = 10;
								}
								if (num5 > Main.WorldSurface)
								{
									num5 = Main.WorldSurface;
								}
								for (int m = num4; m < num5; m++)
								{
									num3 = m;
									if (Main.TileSet[num2, num3].IsActive == 0 || !Main.TileSolid[Main.TileSet[num2, num3].Type])
									{
										continue;
									}
									if (Collision.SolidTiles(num2 - 1, num2 + 1, num3 - 3, num3 - 1))
									{
										break;
									}
									flag = true;
									Rectangle value2 = new Rectangle(num2 * 16 + 8 - (NPC.SpawnWidth / 2) - NPC.SafeRangeX, num3 * 16 + 8 - (NPC.SpawnHeight / 2) - NPC.SafeRangeY, NPC.SpawnWidth + (NPC.SafeRangeX * 2), NPC.SpawnHeight + (NPC.SafeRangeY * 2));
									for (int n = 0; n < Player.MaxNumPlayers; n++)
									{
										if (Main.PlayerSet[n].Active != 0 && Main.PlayerSet[n].XYWH.Intersects(value2))
										{
											flag = false;
											break;
										}
									}
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				int num6 = NPC.NewNPC(num2 * 16, num3 * 16, ToSpawnNPC, 1);
				Main.NPCSet[num6].HomeTileX = (short)bestX;
				Main.NPCSet[num6].HomeTileY = (short)bestY;
				if (num2 < bestX)
				{
					Main.NPCSet[num6].Direction = 1;
				}
				else if (num2 > bestX)
				{
					Main.NPCSet[num6].Direction = -1;
				}
				Main.NPCSet[num6].ShouldNetUpdate = true;
				string text;
				if (Main.NPCSet[num6].HasName())
				{
					text = Main.NPCSet[num6].GetName();
					if (Lang.LangOption <= (int)Lang.ID.ENGLISH)
					{
						text = text + " the " + Main.NPCSet[num6].TypeName;
					}
				}
				else
				{
					text = Main.NPCSet[num6].DisplayName;
				}
				NetMessage.SendText(text, 18, 50, 125, 255, -1);
			}
			else
			{
				Main.NPCSet[num].HomeTileX = (short)bestX;
				Main.NPCSet[num].HomeTileY = (short)bestY;
				Main.NPCSet[num].IsHomeless = false;
			}
			if (!Main.IsTutorial())
			{
				if (ToSpawnNPC == (int)EntityID.NPCID.GUIDE)
				{
					UI.SetTriggerStateForAll(Trigger.HouseGuide);
				}
				CheckHousedNPCs();
			}
			ToSpawnNPC = 0;
		}

		public static void CheckHousedNPCs()
		{
			bool flag = true;
			int num = 0;
			for (int i = 0; i < NPC.MaxNumNPCs; i++)
			{
				NPC nPC = Main.NPCSet[i];
				if (nPC.Active != 0 && nPC.IsTownNPC && nPC.Type != (int)EntityID.NPCID.OLD_MAN && nPC.Type != (int)EntityID.NPCID.SANTA_CLAUS)
				{
					flag = flag && !nPC.IsHomeless;
					num++;
				}
			}
			if (flag && num == NPC.MaxNumTownNPCs)
			{
				UI.SetTriggerStateForAll(Trigger.HousedAllNPCs);
			}
		}

		public static bool RoomNeeds()
		{
			roomChair = EntityID.RoomChairs.Any(i => HouseTile[(int)i]);
			roomDoor = EntityID.RoomDoors.Any(i => HouseTile[(int)i]);
			roomTable = EntityID.RoomTables.Any(i => HouseTile[(int)i]);
			roomTorch = EntityID.RoomTorches.Any(i => HouseTile[(int)i]);

			if (roomChair && roomTable && roomDoor && roomTorch)
			{
				canSpawn = true;
			}
			else
			{
				canSpawn = false;
			}
			return canSpawn;
		}

		public static void QuickFindHome(int npc)
		{
			if (Main.NPCSet[npc].HomeTileX <= 10 || Main.NPCSet[npc].HomeTileY <= 10 || Main.NPCSet[npc].HomeTileX >= Main.MaxTilesX - 10 || Main.NPCSet[npc].HomeTileY >= Main.MaxTilesY)
			{
				return;
			}
			canSpawn = false;
			StartRoomCheck(Main.NPCSet[npc].HomeTileX, Main.NPCSet[npc].HomeTileY - 1);
			if (!canSpawn)
			{
				for (int i = Main.NPCSet[npc].HomeTileX - 1; i < Main.NPCSet[npc].HomeTileX + 2; i++)
				{
					for (int j = Main.NPCSet[npc].HomeTileY - 1; j < Main.NPCSet[npc].HomeTileY + 2 && !StartRoomCheck(i, j); j++)
					{
					}
				}
			}
			if (!canSpawn)
			{
				int num = 10;
				for (int k = Main.NPCSet[npc].HomeTileX - num; k <= Main.NPCSet[npc].HomeTileX + num; k += 2)
				{
					for (int l = Main.NPCSet[npc].HomeTileY - num; l <= Main.NPCSet[npc].HomeTileY + num && !StartRoomCheck(k, l); l += 2)
					{
					}
				}
			}
			if (canSpawn)
			{
				RoomNeeds();
				if (canSpawn)
				{
					ScoreRoom(npc);
				}
				if (canSpawn && hiScore > 0)
				{
					Main.NPCSet[npc].HomeTileX = (short)bestX;
					Main.NPCSet[npc].HomeTileY = (short)bestY;
					Main.NPCSet[npc].IsHomeless = false;
					canSpawn = false;
				}
				else
				{
					Main.NPCSet[npc].IsHomeless = true;
				}
			}
			else
			{
				Main.NPCSet[npc].IsHomeless = true;
			}
		}

		public static void ScoreRoom(int ignoreNPC = -1)
		{
			roomOccupied = false;
			roomEvil = false;
			for (int i = 0; i < NPC.MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active == 0 || !Main.NPCSet[i].IsTownNPC || ignoreNPC == i || Main.NPCSet[i].IsHomeless)
				{
					continue;
				}
				for (int j = 0; j < numRoomTiles; j++)
				{
					if (Main.NPCSet[i].HomeTileX != room[j].X || Main.NPCSet[i].HomeTileY != room[j].Y)
					{
						continue;
					}
					bool flag = false;
					for (int k = 0; k < numRoomTiles; k++)
					{
						if (Main.NPCSet[i].HomeTileX == room[k].X && Main.NPCSet[i].HomeTileY - 1 == room[k].Y)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						roomOccupied = true;
						hiScore = -1;
						return;
					}
				}
			}
			hiScore = 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = roomX1 - (Main.ZoneX / 2 / 16) - 1 - Lighting.OffScreenTiles;
			int num5 = roomX2 + (Main.ZoneX / 2 / 16) + 1 + Lighting.OffScreenTiles;
			int num6 = roomY1 - (Main.ZoneY / 2 / 16) - 1 - Lighting.OffScreenTiles;
			int num7 = roomY2 + (Main.ZoneY / 2 / 16) + 1 + Lighting.OffScreenTiles;
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 >= Main.MaxTilesX)
			{
				num5 = Main.MaxTilesX - 1;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.MaxTilesX)
			{
				num7 = Main.MaxTilesX;
			}
			for (int l = num4 + 1; l < num5; l++)
			{
				for (int m = num6 + 2; m < num7 + 2; m++)
				{
					if (Main.TileSet[l, m].IsActive != 0)
					{
						switch ((EntityID.TileID)Main.TileSet[l, m].Type)
						{
							case EntityID.TileID.CORRUPT_GRASS:
							case EntityID.TileID.SHORT_CORRUPT_PLANTS:
							case EntityID.TileID.EBONSTONE:
							case EntityID.TileID.CORRUPTION_THORN:
							case EntityID.TileID.EBONSAND:
								num3++;
								break;

							case EntityID.TileID.SUNFLOWER:
								num3 -= 5;
								break;

							case EntityID.TileID.HALLOWED_GRASS:
							case EntityID.TileID.SHORT_HALLOWED_PLANTS:
							case EntityID.TileID.TALL_HALLOWED_PLANTS:
							case EntityID.TileID.PEARLSAND:
								num3--;
								break;
						}
					}
				}
			}
			if (num3 < 50)
			{
				num3 = 0;
			}
			num2 = -num3;
			if (num2 <= -250)
			{
				hiScore = num2;
				roomEvil = true;
				return;
			}
			num4 = roomX1;
			num5 = roomX2;
			num6 = roomY1;
			num7 = roomY2;
			for (int n = num4 + 1; n < num5; n++)
			{
				for (int num8 = num6 + 2; num8 < num7 + 2; num8++)
				{
					if (Main.TileSet[n, num8].IsActive == 0)
					{
						continue;
					}
					num = num2;
					if (!Main.TileSolidNotSolidTop[Main.TileSet[n, num8].Type] || Collision.SolidTiles(n - 1, n + 1, num8 - 3, num8 - 1) || Main.TileSet[n - 1, num8].IsActive == 0 || !Main.TileSolid[Main.TileSet[n - 1, num8].Type] || Main.TileSet[n + 1, num8].IsActive == 0 || !Main.TileSolid[Main.TileSet[n + 1, num8].Type])
					{
						continue;
					}
					for (int num9 = n - 2; num9 < n + 3; num9++)
					{
						for (int num10 = num8 - 4; num10 < num8; num10++)
						{
							if (Main.TileSet[num9, num10].IsActive != 0)
							{
								num = ((num9 != n) ? ((Main.TileSet[num9, num10].Type != (byte)EntityID.TileID.DOOR_CLOSED && Main.TileSet[num9, num10].Type != (byte)EntityID.TileID.DOOR_OPEN) ? ((!Main.TileSolid[Main.TileSet[num9, num10].Type]) ? (num + 5) : (num - 5)) : (num - 20)) : (num - 15));
							}
						}
					}
					if (num <= hiScore)
					{
						continue;
					}
					bool flag2 = false;
					for (int num11 = 0; num11 < numRoomTiles; num11++)
					{
						if (room[num11].X == n && room[num11].Y == num8)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						hiScore = num;
						bestX = n;
						bestY = num8;
					}
				}
			}
		}

		public static bool StartRoomCheck(int x, int y)
		{
			roomX1 = x;
			roomX2 = x;
			roomY1 = y;
			roomY2 = y;
			numRoomTiles = 0;
			for (int i = 0; i < Main.MaxNumTilesets; i++)
			{
				HouseTile[i] = false;
			}
			canSpawn = true;
			if (Main.TileSet[x, y].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y].Type])
			{
				canSpawn = false;
			}
			else
			{
				checkRoomDepth = 0;
				CheckRoom(x, y);
				if (numRoomTiles < 60)
				{
					canSpawn = false;
				}
			}
			return canSpawn;
		}

		public static void CheckRoom(int x, int y)
		{
			if (x < 10 || y < 10 || x >= Main.MaxTilesX - 10 || y >= lastMaxTilesY - 10)
			{
				canSpawn = false;
				return;
			}
			for (int i = 0; i < numRoomTiles; i++)
			{
				if (room[i].X == x && room[i].Y == y)
				{
					return;
				}
			}
			room[numRoomTiles].X = (short)x;
			room[numRoomTiles].Y = (short)y;
			if (++numRoomTiles >= MAX_ROOM_TILES)
			{
				canSpawn = false;
				return;
			}
			if (++checkRoomDepth >= MAX_ROOM_RECURSION)
			{
				canSpawn = false;
				return;
			}
			if (Main.TileSet[x, y].IsActive != 0)
			{
				HouseTile[Main.TileSet[x, y].Type] = true;
				if (Main.TileSolid[Main.TileSet[x, y].Type] || Main.TileSet[x, y].Type == (byte)EntityID.TileID.DOOR_OPEN)
				{
					checkRoomDepth--;
					return;
				}
			}
			if (x < roomX1)
			{
				roomX1 = x;
			}
			if (x > roomX2)
			{
				roomX2 = x;
			}
			if (y < roomY1)
			{
				roomY1 = y;
			}
			if (y > roomY2)
			{
				roomY2 = y;
			}
			int num = 0;
			for (int j = -2; j < 3; j++)
			{
				if (Main.WallHouse[Main.TileSet[x + j, y].WallType])
				{
					num |= 1;
				}
				else if (Main.TileSet[x + j, y].IsActive != 0 && (Main.TileSolid[Main.TileSet[x + j, y].Type] || Main.TileSet[x + j, y].Type == (byte)EntityID.TileID.DOOR_OPEN))
				{
					num |= 1;
				}
				if (Main.WallHouse[Main.TileSet[x, y + j].WallType])
				{
					num |= 2;
				}
				else if (Main.TileSet[x, y + j].IsActive != 0 && (Main.TileSolid[Main.TileSet[x, y + j].Type] || Main.TileSet[x, y + j].Type == (byte)EntityID.TileID.DOOR_OPEN))
				{
					num |= 2;
				}
			}
			if (num != 3)
			{
				canSpawn = false;
				return;
			}
			for (int dx = -1; dx < 2; dx++)
			{
				for (int dy = -1; dy < 2; dy++)
				{
					if (dx == 0 && dy == 0)
						continue;

					if (canSpawn)
					{
						CheckRoom(x + dx, y + dy);
						if (dx == 1 && dy == 1 && canSpawn)
						{
							checkRoomDepth--;
						}
					}
				}
			}
		}

		public static bool StartSpaceCheck(int x, int y)
		{
			// Why the hell does the Tutorial need this function? StartRoomCheck() is the damn same.
			roomX1 = x;
			roomX2 = x;
			roomY1 = y;
			roomY2 = y;
			numRoomTiles = 0;
			for (int i = 0; i < Main.MaxNumTilesets; i++)
			{
				HouseTile[i] = false;
			}
			canSpawn = true;
			if (Main.TileSet[x, y].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y].Type])
			{
				canSpawn = false;
			}
			else
			{
				checkRoomDepth = 0;
				CheckSpace(x, y);
				if (numRoomTiles < 60)
				{
					canSpawn = false;
				}
			}
			return canSpawn;
		}

		public static void CheckSpace(int x, int y)
		{
			if (x < 10 || y < 10 || x >= Main.MaxTilesX - 10 || y >= lastMaxTilesY - 10)
			{
				canSpawn = false;
				return;
			}
			for (int i = 0; i < numRoomTiles; i++)
			{
				if (room[i].X == x && room[i].Y == y)
				{
					return;
				}
			}
			room[numRoomTiles].X = (short)x;
			room[numRoomTiles].Y = (short)y;
			if (++numRoomTiles >= MAX_ROOM_TILES)
			{
				canSpawn = false;
				return;
			}
			if (++checkRoomDepth >= MAX_ROOM_RECURSION)
			{
				canSpawn = false;
				return;
			}
			if (Main.TileSet[x, y].IsActive != 0)
			{
				HouseTile[Main.TileSet[x, y].Type] = true;
				if (Main.TileSolid[Main.TileSet[x, y].Type] || Main.TileSet[x, y].Type == (byte)EntityID.TileID.DOOR_OPEN)
				{
					checkRoomDepth--;
					return;
				}
			}
			if (x < roomX1)
			{
				roomX1 = x;
			}
			if (x > roomX2)
			{
				roomX2 = x;
			}
			if (y < roomY1)
			{
				roomY1 = y;
			}
			if (y > roomY2)
			{
				roomY2 = y;
			}
			for (int dx = -1; dx < 2; dx++)
			{
				for (int dy = -1; dy < 2; dy++)
				{
					if (dx == 0 && dy == 0)
						continue;

					if (canSpawn)
					{
						CheckSpace(x + dx, y + dy);
						if (dx == 1 && dy == 1 && canSpawn)
						{
							checkRoomDepth--;
						}
					}
				}
			}
		}

		public static void DropMeteor()
		{
			bool flag = true;
			int num = 0;
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active != 0)
				{
					flag = false;
					break;
				}
			}
			int num2 = 0;
			float num3 = Main.MaxTilesX / 4200f;
			int num4 = (int)(400f * num3);
			for (int j = 5; j < Main.MaxTilesX - 5; j++)
			{
				for (int k = 5; k < Main.WorldSurface; k++)
				{
					if (Main.TileSet[j, k].IsActive != 0 && Main.TileSet[j, k].Type == (byte)EntityID.TileID.METEORITE)
					{
						num2++;
						if (num2 > num4)
						{
							return;
						}
					}
				}
			}
			while (!flag)
			{
				float num5 = Main.MaxTilesX * 0.08f;
				int num6 = Main.Rand.Next(50, Main.MaxTilesX - 50);
				while (num6 > Main.SpawnTileX - num5 && num6 < Main.SpawnTileX + num5)
				{
					num6 = Main.Rand.Next(50, Main.MaxTilesX - 50);
				}
				for (int l = Main.Rand.Next(100); l < Main.MaxTilesY; l++)
				{
					if (Main.TileSet[num6, l].IsActive != 0 && Main.TileSolid[Main.TileSet[num6, l].Type])
					{
						flag = meteor(num6, l);
						break;
					}
				}
				num++;
				if (num >= 100)
				{
					break;
				}
			}
		}

		public static bool meteor(int i, int j)
		{
			if (i < 50 || i > Main.MaxTilesX - 50)
			{
				return false;
			}
			if (j < 50 || j > Main.MaxTilesY - 50)
			{
				return false;
			}
			Rectangle rectangle = new Rectangle((i - 25) * 16, (j - 25) * 16, 800, 800);
			for (int k = 0; k < Player.MaxNumPlayers; k++)
			{
				if (Main.PlayerSet[k].Active != 0)
				{
					Rectangle value = new Rectangle(Main.PlayerSet[k].XYWH.X + (Main.PlayerSet[k].XYWH.Width / 2) - (NPC.SpawnWidth / 2) - NPC.SafeRangeX, Main.PlayerSet[k].XYWH.Y + (Main.PlayerSet[k].XYWH.Height / 2) - (NPC.SpawnHeight / 2) - NPC.SafeRangeY, NPC.SpawnWidth + (NPC.SafeRangeX * 2), NPC.SpawnHeight + (NPC.SafeRangeY * 2));
					if (rectangle.Intersects(value))
					{
						return false;
					}
				}
			}
			for (int l = 0; l < NPC.MaxNumNPCs; l++)
			{
				if (Main.NPCSet[l].Active != 0 && rectangle.Intersects(Main.NPCSet[l].XYWH))
				{
					return false;
				}
			}
			for (int m = i - 25; m < i + 25; m++)
			{
				for (int n = j - 25; n < j + 25; n++)
				{
					if (Main.TileSet[m, n].Type == (byte)EntityID.TileID.CHEST && Main.TileSet[m, n].IsActive != 0)
					{
						return false;
					}
				}
			}
			stopDrops = true;
			for (int num = i - 15; num < i + 15; num++)
			{
				for (int num2 = j - 15; num2 < j + 15; num2++)
				{
					if (num2 > j + Main.Rand.Next(-2, 3) - 5 && Math.Abs(i - num) + Math.Abs(j - num2) < 22 + Main.Rand.Next(-5, 5))
					{
						if (!Main.TileSolid[Main.TileSet[num, num2].Type])
						{
							Main.TileSet[num, num2].IsActive = 0;
						}
						Main.TileSet[num, num2].Type = (byte)EntityID.TileID.METEORITE;
					}
				}
			}
			for (int num3 = i - 10; num3 < i + 10; num3++)
			{
				for (int num4 = j - 10; num4 < j + 10; num4++)
				{
					if (num4 > j + Main.Rand.Next(-2, 3) - 5 && Math.Abs(i - num3) + Math.Abs(j - num4) < 10 + Main.Rand.Next(-3, 4))
					{
						Main.TileSet[num3, num4].IsActive = 0;
					}
				}
			}
			for (int num5 = i - 16; num5 < i + 16; num5++)
			{
				for (int num6 = j - 16; num6 < j + 16; num6++)
				{
					int type = Main.TileSet[num5, num6].Type;
					if (type == (byte)EntityID.TileID.TREE || type == (byte)EntityID.TileID.CORRUPTION_THORN)
					{
						KillTile(num5, num6);
					}
					SquareTileFrame(num5, num6);
					SquareWallFrame(num5, num6);
				}
			}
			for (int num7 = i - 23; num7 < i + 23; num7++)
			{
				for (int num8 = j - 23; num8 < j + 23; num8++)
				{
					if (Main.TileSet[num7, num8].IsActive != 0 && Main.Rand.Next(10) == 0 && Math.Abs(i - num7) + Math.Abs(j - num8) < 29.9f)
					{
						if (Main.TileSet[num7, num8].Type == (byte)EntityID.TileID.TREE || Main.TileSet[num7, num8].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							KillTile(num7, num8);
						}
						Main.TileSet[num7, num8].Type = (byte)EntityID.TileID.METEORITE;
						SquareTileFrame(num7, num8);
					}
				}
			}
			stopDrops = false;
			NetMessage.SendText(36, 50, 255, 130, -1);
			NetMessage.SendTileSquare(i, j, 30);
			return true;
		}

		public static void setWorldSize()
		{
			Main.BottomWorld = Main.MaxTilesY * 16;
			Main.RightWorld = Main.MaxTilesX * 16;
			Main.MaxSectionsX = Main.MaxTilesX / 40;
			Main.MaxSectionsY = Main.MaxTilesY / 30;
		}

		public static void worldGenCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
			clearWorld();
			generateWorld();
			everyTileFrame();
			saveWorldWhilePlaying();
			Main.StartGame();
			Main.WorldGenThread = null;
		}

		public static void CreateNewWorld()
		{
			Netplay.StopFindingSessions();
			Thread thread = new Thread(worldGenCallBack);
			thread.IsBackground = true;
			thread.Start();
			Main.WorldGenThread = thread;
		}

		public static void SaveAndQuit()
		{
			Main.PlaySound(11);
			Thread thread = new Thread(SaveAndQuitCallBack);
			thread.Start();
		}

		public static void SaveAndQuitCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
			Main.IsGameStarted = false;
			for (int i = 0; i < 4; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.isStopping)
				{
					uI.ActivePlayer.Active = 0;
					uI.ActivePlayer.Save(uI.playerPathName);
					uI.SaveSettings();
				}
			}
#if (!IS_PATCHED && VERSION_INITIAL)
			int NetMode = Main.NetMode;
			Netplay.PlayDisconnect = true;
			if (NetMode != (byte)NetModeSetting.CLIENT && UI.MainUI.HasPlayerStorage())
#else
			Netplay.PlayDisconnect = true;
			if (WorldSelect.isLocalWorld && UI.MainUI.HasPlayerStorage())
#endif
			{
				for (int j = 0; j < 4; j++)
				{
					UI uI2 = Main.UIInstance[j];
					if (uI2.isStopping)
					{
						uI2.statusText = Lang.WorldGenText[49];
					}
				}
				saveNewWorld();
			}
			for (int k = 0; k < Main.UIInstance.Length; k++)
			{
				UI uI3 = Main.UIInstance[k];
				if (!uI3.isStopping)
				{
					continue;
				}
				uI3.isStopping = false;
				if (uI3.SignedInGamer != null)
				{
					uI3.LoadPlayers();
					if (uI3.CurMenuMode != MenuMode.ERROR)
					{
						uI3.SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
					}
				}
			}
		}

		public static void playWorldCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
			if (Main.IsTutorial())
			{
				using (Stream file = TitleContainer.OpenStream("Content/Worlds/tutorial.wld"))
				{
					loadWorld(file);
				}
				tempTime.reset(0.01f);
				Main.NPCSet[0].Position.Y -= 1120f;
				Main.NPCSet[0].XYWH.Y -= 1120;
				Main.NPCSet[1].Position.Y -= 1120f;
				Main.NPCSet[1].XYWH.Y -= 1120;
			}
			else
			{
				bool flag;
				try
				{
					using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Worlds"))
					{
						using (Stream file2 = storageContainer.OpenFile(WorldSelect.worldPathName, FileMode.Open))
						{
							flag = loadWorld(file2);
						}
					}
				}
				catch (ThreadAbortException)
				{
					flag = true;
				}
				catch (IOException)
				{
					UI.MainUI.ReadError();
					flag = false;
				}
				catch (Exception)
				{
					flag = false;
				}
				if (!flag)
				{
					UI.MainUI.SetMenu(MenuMode.LOAD_FAILED_NO_BACKUP, rememberPrevious: false);
					Main.WorldGenThread = null;
					return;
				}
			}
			everyTileFrame();
			Main.StartGame();
			Main.WorldGenThread = null;
		}

		public static void PlayWorld()
		{
			Netplay.StopFindingSessions();
			Thread thread = new Thread(playWorldCallBack);
			thread.Start();
			Main.WorldGenThread = thread;
		}

		public static void saveWorldWhilePlayingCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				4
			});
#endif
			Gen = true;
			saveNewWorld();
		}

		public static void saveWorldWhilePlaying()
		{
			if (UI.MainUI.HasPlayerStorage())
			{
				Thread thread = new Thread(saveWorldWhilePlayingCallBack);
				thread.Start();
			}
		}

		public static void savePlayerWhilePlayingCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				4
			});
#endif
			for (int i = 0; i < Main.UIInstance.Length; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.CurMenuType != MenuType.MAIN)
				{
					uI.ActivePlayer.Save(uI.playerPathName);
					uI.SaveSettings();
				}
			}
		}

		public static void savePlayerWhilePlaying()
		{
			Thread thread = new Thread(savePlayerWhilePlayingCallBack);
			thread.Start();
		}

		public static void saveAllWhilePlayingCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				4
			});
#endif
#if (!IS_PATCHED && VERSION_INITIAL)
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && UI.MainUI.HasPlayerStorage())
#else
			if (WorldSelect.isLocalWorld && UI.MainUI.HasPlayerStorage())
#endif
			{
				saveNewWorld();
			}
			for (int i = 0; i < Main.UIInstance.Length; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.CurMenuType != MenuType.MAIN)
				{
					uI.ActivePlayer.Save(uI.playerPathName);
					uI.SaveSettings();
				}
			}
		}

		public static void saveAllWhilePlaying()
		{
			Thread thread = new Thread(saveAllWhilePlayingCallBack);
			thread.Start();
		}

		public static void clearWorld()
		{
			UI.MainUI.statusText = Lang.WorldGenText[47];
			tempTime.reset(1f);
			totalSolid2 = 0;
			totalGood2 = 0;
			totalEvil2 = 0;
			totalSolid = 0;
			totalGood = 0;
			totalEvil = 0;
			totalX = 0;
			totalD = 0;
			EvilCoverage = 0;
			GoodCoverage = 0;
			NPC.ClearNames();
			ToSpawnEye = false;
			ToSpawnNPC = 0;
			shadowOrbCount = 0;
			altarCount = 0;
			Main.WorldID = 0;
			Main.WorldTimestamp = 0;
			Main.InHardMode = false;
			Main.DungeonX = 0;
			Main.DungeonY = 0;
			NPC.HasDownedBoss1 = false;
			NPC.HasDownedBoss2 = false;
			NPC.HasDownedBoss3 = false;
			NPC.HasSavedGoblin = false;
			NPC.HasSavedWizard = false;
			NPC.HasSavedMech = false;
			NPC.HasDownedGoblins = false;
			NPC.HasDownedClown = false;
			NPC.HasDownedFrost = false;
			HasShadowOrbSmashed = false;
			ToSpawnMeteor = false;
			stopDrops = false;
			Main.InvasionDelay = 0;
			Main.InvasionType = 0;
			Main.InvasionSize = 0;
			Main.InvasionWarn = 0;
			Main.InvasionX = 0f;
			Liquid.NumLiquids = 0;
			LiquidBuffer.NumLiquidBuffer = 0;
			sandBuffer[0] = new FallingSandBuffer();
			sandBuffer[1] = new FallingSandBuffer();
			if (lastMaxTilesX > Main.MaxTilesX)
			{
				for (int i = Main.MaxTilesX; i < lastMaxTilesX; i++)
				{
					for (int j = 0; j < lastMaxTilesY; j++)
					{
						Main.TileSet[i, j].Clear();
					}
				}
			}
			if (lastMaxTilesY > Main.MaxTilesY)
			{
				for (int k = 0; k < Main.MaxTilesX; k++)
				{
					for (int l = Main.MaxTilesY; l < lastMaxTilesY; l++)
					{
						Main.TileSet[k, l].Clear();
					}
				}
			}
			lastMaxTilesX = Main.MaxTilesX;
			lastMaxTilesY = Main.MaxTilesY;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				for (int m = 0; m < Main.MaxTilesX; m++)
				{
					for (int n = 0; n < Main.MaxTilesY; n++)
					{
						Main.TileSet[m, n].Clear();
					}
				}
			}
			Main.DustSet.Init();
			for (int num = 0; num < Gore.MaxNumGore; num++)
			{
				Main.GoreSet[num].Init();
			}
			for (int num2 = 0; num2 < Main.MaxNumItems; num2++)
			{
				Main.ItemSet[num2].Init();
			}
			for (int num3 = 0; num3 < NPC.MaxNumNPCs; num3++)
			{
				Main.NPCSet[num3] = new NPC();
			}
			for (int num4 = 0; num4 < Projectile.MaxNumProjs; num4++)
			{
				Main.ProjectileSet[num4].Init();
			}
			for (int num5 = 0; num5 < Main.MaxNumChests; num5++)
			{
				Main.ChestSet[num5] = null;
			}
			for (int num6 = 0; num6 < Sign.MaxNumSigns; num6++)
			{
				Main.SignSet[num6].Init();
			}
			for (int num7 = 0; num7 < LiquidBuffer.MaxNumLiquidBuffer; num7++)
			{
				Main.LiquidBuffer[num7] = default;
			}
#if (VERSION_INITIAL && !IS_PATCHED)
			setWorldSize();
#endif
		}

		public static bool loadWorld(Stream file)
		{
			bool result = true;
			Time.CheckXMas();
			using (MemoryStream memoryStream = new MemoryStream((int)file.Length))
			{
				memoryStream.SetLength(file.Length);
				file.ReadExactly(memoryStream.GetBuffer(), 0, (int)file.Length);
				file.Close();
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					try
					{
						int num = binaryReader.ReadInt32();
						if (num > Main.NewWorldDataVersion)
						{
							throw new InvalidOperationException("Invalid version");
						}
						if (num <= Main.OldWorldDataVersion)
						{
							loadOldWorld(binaryReader, num);
						}
						else
						{
							loadNewWorld(binaryReader, num, memoryStream);
						}
						Gen = true;
						UI.MainUI.NextProgressStep(Lang.WorldGenText[52]);
						for (int i = 0; i < Main.MaxTilesX; i++)
						{
							if ((i & 0x3F) == 0)
							{
								UI.MainUI.Progress = i / (float)Main.MaxTilesX;
							}
							CountTiles(i);
						}
						NPC.SetNames();
						UI.MainUI.NextProgressStep(Lang.WorldGenText[27]);
						WaterLine = Main.MaxTilesY;
						Liquid.QuickWater(0.5);
						WaterCheck();
						int num2 = 0;
						Liquid.QuickSettleOn();
						int num3 = Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer;
						float num4 = 0f;
						while (Liquid.NumLiquids > 0 && num2 < 512)
						{
							num2++;
							float num5 = (num3 - (Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer)) / (float)num3;
							if (Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer > num3)
							{
								num3 = Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer;
							}
							if (num5 > num4)
							{
								num4 = num5;
							}
							else
							{
								num5 = num4;
							}
							if (num5 <= 0.5f)
							{
								UI.MainUI.Progress = num5 + 0.5f;
							}
							Liquid.UpdateLiquid();
						}
						Liquid.QuickSettleOff();
						WaterCheck();
						Gen = false;
						return result;
					}
					catch
					{
						return false;
					}
				}
			}
		}

		// Turns out I was wrong about this function. It legit is just a prototype world loader used prior to when they developed the new 'console-formatted' worlds.
		// It still has some differences than PC 1.1.2, but the similarities are still here, like RightWorld and BottomWorld being read twice.
		private unsafe static void loadOldWorld(BinaryReader fileIO, int release)
		{
			string b = fileIO.ReadString();
			int worldID = fileIO.ReadInt32();
			Main.RightWorld = fileIO.ReadInt32();
			Main.RightWorld = fileIO.ReadInt32();
			Main.BottomWorld = fileIO.ReadInt32();
			Main.BottomWorld = fileIO.ReadInt32();
			Main.MaxTilesY = (short)fileIO.ReadInt32();
			Main.MaxTilesX = (short)fileIO.ReadInt32();
			clearWorld();
			Main.WorldID = worldID;
			UI.MainUI.FirstProgressStep(4, Lang.WorldGenText[51]);
			Main.SpawnTileX = (short)fileIO.ReadInt32();
			Main.SpawnTileY = (short)fileIO.ReadInt32();
			Main.WorldSurface = (int)fileIO.ReadDouble();
			Main.WorldSurfacePixels = Main.WorldSurface << 4;
			Main.RockLayer = (int)fileIO.ReadDouble();
			Main.RockLayerPixels = Main.RockLayer << 4;
			UpdateMagmaLayerPos();
			tempTime.DayRate = 1f;
			tempTime.WorldTime = (float)fileIO.ReadDouble();
			tempTime.DayTime = fileIO.ReadBoolean();
			tempTime.MoonPhase = (byte)fileIO.ReadInt32();
			tempTime.IsBloodMoon = fileIO.ReadBoolean();
			Main.DungeonX = (short)fileIO.ReadInt32();
			Main.DungeonY = (short)fileIO.ReadInt32();
			NPC.HasDownedBoss1 = fileIO.ReadBoolean();
			NPC.HasDownedBoss2 = fileIO.ReadBoolean();
			NPC.HasDownedBoss3 = fileIO.ReadBoolean();
			NPC.HasSavedGoblin = fileIO.ReadBoolean();
			NPC.HasSavedWizard = fileIO.ReadBoolean();
			NPC.HasSavedMech = fileIO.ReadBoolean();
			NPC.HasDownedGoblins = fileIO.ReadBoolean();
			NPC.HasDownedClown = fileIO.ReadBoolean();
			NPC.HasDownedFrost = fileIO.ReadBoolean();
			HasShadowOrbSmashed = fileIO.ReadBoolean();
			ToSpawnMeteor = fileIO.ReadBoolean();
			shadowOrbCount = fileIO.ReadByte();
			altarCount = fileIO.ReadInt32();
			Main.InHardMode = fileIO.ReadBoolean();
			Main.InvasionDelay = fileIO.ReadInt32();
			Main.InvasionSize = fileIO.ReadInt32();
			Main.InvasionType = fileIO.ReadInt32();
			Main.InvasionX = (float)fileIO.ReadDouble();
			for (int i = 0; i < Main.MaxTilesX; i++)
			{
				if ((i & 0x1F) == 0)
				{
					UI.MainUI.Progress = (float)i / (float)Main.MaxTilesX;
				}
				fixed (Tile* ptr = &Main.TileSet[i, 0])
				{
					Tile* ptr2 = ptr;
					int num = 0;
					while (num < Main.MaxTilesY)
					{
						ptr2->CurrentFlags = ~(Tile.Flags.WALLFRAME_MASK | Tile.Flags.HIGHLIGHT_MASK | Tile.Flags.VISITED | Tile.Flags.WIRE | Tile.Flags.CHECKING_LIQUID | Tile.Flags.SKIP_LIQUID);
						ptr2->IsActive = fileIO.ReadByte();
						if (ptr2->IsActive != 0)
						{
							ptr2->Type = fileIO.ReadByte();
							if (ptr2->Type == (byte)EntityID.TileID.ICE)
							{
								ptr2->IsActive = 0;
							}
							if (Main.TileFrameImportant[ptr2->Type])
							{
								ptr2->FrameX = fileIO.ReadInt16();
								ptr2->FrameY = fileIO.ReadInt16();
								if (ptr2->Type == (byte)EntityID.TileID.TIMER)
								{
									ptr2->FrameY = 0;
								}
							}
							else
							{
								ptr2->FrameX = -1;
								ptr2->FrameY = -1;
							}
						}
						if (fileIO.ReadBoolean())
						{
							ptr2->WallType = fileIO.ReadByte();
						}
						if (fileIO.ReadBoolean())
						{
							ptr2->Liquid = fileIO.ReadByte();
							if (fileIO.ReadBoolean())
							{
								ptr2->Lava = 32;
							}
						}
						if (release < Main.OldWorldDataVersion) // Here we can see how Engine was sorting out wiring before they settled with flags and eventually in the 1.0 patch, WireCheck.
						{
							if (fileIO.ReadBoolean())
							{
								ptr2->wire = 16;
							}
						}
						else
						{
							ptr2->CurrentFlags |= (Tile.Flags)fileIO.ReadByte();
							if (Main.IsTutorial())
							{
								ptr2->CurrentFlags &= ~Tile.Flags.VISITED;
							}
						}
						int num2 = fileIO.ReadInt16();
						num += num2;
						while (num2 > 0)
						{
							ptr2[1] = *ptr2;
							ptr2++;
							num2--;
						}
						num++;
						ptr2++;
					}
				}
			}
			for (int j = 0; j < Main.MaxNumChests; j++)
			{
				if (!fileIO.ReadBoolean())
				{
					continue;
				}
				Main.ChestSet[j] = new Chest();
				Main.ChestSet[j].XPos = (short)fileIO.ReadInt32();
				Main.ChestSet[j].YPos = (short)fileIO.ReadInt32();
				for (int k = 0; k < Chest.MaxNumItems; k++)
				{
					byte b2 = fileIO.ReadByte();
					if (b2 > 0)
					{
						Main.ChestSet[j].ItemSet[k].NetDefaults(fileIO.ReadInt32(), b2);
						Main.ChestSet[j].ItemSet[k].SetPrefix(fileIO.ReadByte());
					}
				}
			}
			for (int l = 0; l < Sign.MaxNumSigns; l++)
			{
				if (fileIO.ReadBoolean())
				{
					string s = fileIO.ReadString();
					int num3 = fileIO.ReadInt32();
					int num4 = fileIO.ReadInt32();
					if (Main.TileSet[num3, num4].IsActive != 0 && (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SIGN || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.TOMBSTONE))
					{
						Main.SignSet[l].SignX = (short)num3;
						Main.SignSet[l].SignY = (short)num4;
						Main.SignSet[l].SignString = s;
					}
				}
			}
			bool flag = fileIO.ReadBoolean();
			int num5 = 0;
			while (flag)
			{
				try
				{
					string value = fileIO.ReadString();
					Main.NPCSet[num5].SetDefaults(Convert.ToUInt16(value));
					Main.NPCSet[num5].Position.X = fileIO.ReadSingle();
					Main.NPCSet[num5].Position.Y = fileIO.ReadSingle();
					Main.NPCSet[num5].XYWH.X = (int)Main.NPCSet[num5].Position.X;
					Main.NPCSet[num5].XYWH.Y = (int)Main.NPCSet[num5].Position.Y;
					Main.NPCSet[num5].IsHomeless = fileIO.ReadBoolean();
					Main.NPCSet[num5].HomeTileX = (short)fileIO.ReadInt32();
					Main.NPCSet[num5].HomeTileY = (short)fileIO.ReadInt32();
					num5++;
				}
				catch (FormatException)
				{
					fileIO.ReadBytes(17);
				}
				flag = fileIO.ReadBoolean();
			}
			NPC.TypeNames[(int)EntityID.NPCID.MERCHANT] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.NURSE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.ARMS_DEALER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DRYAD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GUIDE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.CLOTHIER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DEMOLITIONIST] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GOBLIN_TINKERER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.WIZARD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.MECHANIC] = fileIO.ReadString();
			bool flag2 = fileIO.ReadBoolean();
			string a = fileIO.ReadString();
			int num6 = fileIO.ReadInt32();
			if (!flag2 || a != b || num6 != Main.WorldID)
			{
				throw new InvalidOperationException("Invalid footer");
			}
		}

		private unsafe static void loadNewWorld(BinaryReader fileIO, int release, MemoryStream stream)
		{
#if USE_ORIGINAL_CODE
			CRC32 cRC = new CRC32();
			cRC.Update(stream.GetBuffer(), 8, (int)stream.Length - 8);
			if (cRC.GetValue() != fileIO.ReadUInt32())
			{
				throw new InvalidOperationException("Invalid CRC32");
			}
			fileIO.ReadString();
			int worldID = fileIO.ReadInt32();
			int worldTimestamp = ((release >= Main.NewWorldDataVersion - 1) ? fileIO.ReadInt32() : 0);
			Main.RightWorld = fileIO.ReadInt32();
			Main.BottomWorld = fileIO.ReadInt16();
			Main.MaxTilesY = fileIO.ReadInt16();
			Main.MaxTilesX = fileIO.ReadInt16();
			clearWorld();
			Main.WorldID = worldID;
			Main.WorldTimestamp = worldTimestamp;
			UI.MainUI.FirstProgressStep(4, Lang.WorldGenText[51]);
			Main.SpawnTileX = fileIO.ReadInt16();
			Main.SpawnTileY = fileIO.ReadInt16();
			Main.WorldSurface = fileIO.ReadInt16();
			Main.WorldSurfacePixels = Main.WorldSurface << 4;
			Main.RockLayer = fileIO.ReadInt16();
			Main.RockLayerPixels = Main.RockLayer << 4;
			UpdateMagmaLayerPos();
			tempTime.DayRate = 1f;
			tempTime.WorldTime = fileIO.ReadSingle();
			tempTime.DayTime = fileIO.ReadBoolean();
			tempTime.MoonPhase = fileIO.ReadByte();
			tempTime.IsBloodMoon = fileIO.ReadBoolean();
			Main.DungeonX = fileIO.ReadInt16();
			Main.DungeonY = fileIO.ReadInt16();
			NPC.HasDownedBoss1 = fileIO.ReadBoolean();
			NPC.HasDownedBoss2 = fileIO.ReadBoolean();
			NPC.HasDownedBoss3 = fileIO.ReadBoolean();
			NPC.HasSavedGoblin = fileIO.ReadBoolean();
			NPC.HasSavedWizard = fileIO.ReadBoolean();
			NPC.HasSavedMech = fileIO.ReadBoolean();
			NPC.HasDownedGoblins = fileIO.ReadBoolean();
			NPC.HasDownedClown = fileIO.ReadBoolean();
			NPC.HasDownedFrost = fileIO.ReadBoolean();
			HasShadowOrbSmashed = fileIO.ReadBoolean();
			ToSpawnMeteor = fileIO.ReadBoolean();
			shadowOrbCount = fileIO.ReadByte();
			altarCount = fileIO.ReadInt32();
			Main.InHardMode = fileIO.ReadBoolean();
			Main.InvasionDelay = fileIO.ReadByte();
			Main.InvasionSize = fileIO.ReadInt16();
			Main.InvasionType = fileIO.ReadByte();
			Main.InvasionX = fileIO.ReadSingle();
			for (int i = 0; i < Main.MaxTilesX; i++)
			{
				if ((i & 0x1F) == 0)
				{
					UI.MainUI.Progress = i / (float)Main.MaxTilesX;
				}
				fixed (Tile* ptr = &Main.TileSet[i, 0])
				{
					Tile* ptr2 = ptr;
					int num = 0;
					while (num < Main.MaxTilesY)
					{
						ptr2->CurrentFlags = ~(Flags.WALLFRAME_MASK | Flags.HIGHLIGHT_MASK | Flags.VISITED | Flags.WIRE | Flags.CHECKING_LIQUID | Flags.SKIP_LIQUID);
						ptr2->IsActive = fileIO.ReadByte();
						if (ptr2->IsActive != 0)
						{
							ptr2->Type = fileIO.ReadByte();
							if (ptr2->Type == (byte)EntityID.TileID.ICE)
							{
								ptr2->IsActive = 0;
							}
							if (Main.TileFrameImportant[ptr2->Type])
							{
								ptr2->FrameX = fileIO.ReadInt16();
								ptr2->FrameY = fileIO.ReadInt16();
								if (ptr2->Type == (byte)EntityID.TileID.TIMER)
								{
									ptr2->FrameY = 0;
								}
							}
							else
							{
								ptr2->FrameX = -1;
								ptr2->FrameY = -1;
							}
						}
						ptr2->WallType = fileIO.ReadByte();
						ptr2->Liquid = fileIO.ReadByte();
						if (ptr2->Liquid > 0 && fileIO.ReadBoolean())
						{
							ptr2->Lava = 32;
						}
						ptr2->CurrentFlags |= (Flags)fileIO.ReadByte();
						if (Main.IsTutorial())
						{
							ptr2->CurrentFlags &= ~Flags.VISITED;
						}
						int num2 = fileIO.ReadByte();
						if (((uint)num2 & 0x80u) != 0)
						{
							num2 &= 0x7F;
							num2 |= fileIO.ReadByte() << 7;
						}
						num += num2;
						while (num2 > 0)
						{
							ptr2[1] = *ptr2;
							ptr2++;
							num2--;
						}
						num++;
						ptr2++;
					}
				}
			}
			for (int j = 0; j < Main.MaxNumChests; j++)
			{
				if (!fileIO.ReadBoolean())
				{
					continue;
				}
				Main.ChestSet[j] = new Chest();
				Main.ChestSet[j].XPos = fileIO.ReadInt16();
				Main.ChestSet[j].YPos = fileIO.ReadInt16();
				for (int k = 0; k < Chest.MaxNumItems; k++)
				{
					byte b = fileIO.ReadByte();
					if (b > 0)
					{
						Main.ChestSet[j].ItemSet[k].NetDefaults(fileIO.ReadInt16(), b);
						Main.ChestSet[j].ItemSet[k].SetPrefix(fileIO.ReadByte());
					}
				}
			}
			for (int l = 0; l < Sign.MaxNumSigns; l++)
			{
				Main.SignSet[l].Read(fileIO, release);
			}
			bool flag = fileIO.ReadBoolean();
			int num3 = 0;
			while (flag)
			{
				int type = fileIO.ReadByte();
				Main.NPCSet[num3].SetDefaults(type);
				Main.NPCSet[num3].Position.X = fileIO.ReadSingle();
				Main.NPCSet[num3].Position.Y = fileIO.ReadSingle();
				Main.NPCSet[num3].XYWH.X = (int)Main.NPCSet[num3].Position.X;
				Main.NPCSet[num3].XYWH.Y = (int)Main.NPCSet[num3].Position.Y;
				Main.NPCSet[num3].IsHomeless = fileIO.ReadBoolean();
				Main.NPCSet[num3].HomeTileX = fileIO.ReadInt16();
				Main.NPCSet[num3].HomeTileY = fileIO.ReadInt16();
				num3++;
				flag = fileIO.ReadBoolean();
			}
			NPC.TypeNames[(int)EntityID.NPCID.MERCHANT] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.NURSE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.ARMS_DEALER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DRYAD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GUIDE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.CLOTHIER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DEMOLITIONIST] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GOBLIN_TINKERER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.WIZARD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.MECHANIC] = fileIO.ReadString();
#else

			if (release < (int)EntityID.WorldID.V101)
			{
				CRC32 cRC = new CRC32();
				cRC.Update(stream.GetBuffer(), 8, (int)stream.Length - 8);
				if (cRC.GetValue() != fileIO.ReadUInt32())
				{
					throw new InvalidOperationException("Invalid CRC32");
				}
			}
			else
			{
				fileIO.ReadUInt32();
			}
			fileIO.ReadString();
			int worldID = fileIO.ReadInt32();
			int worldTimestamp = ((release >= (int)EntityID.WorldID.INITIAL - 1) ? fileIO.ReadInt32() : 0);
			Main.RightWorld = fileIO.ReadInt32();
			Main.BottomWorld = fileIO.ReadInt16();
			Main.MaxTilesY = fileIO.ReadInt16();
			Main.MaxTilesX = fileIO.ReadInt16();

#if (!VERSION_INITIAL || IS_PATCHED)
			setWorldSize();
#endif

			clearWorld();
			Main.WorldID = worldID;
			Main.WorldTimestamp = worldTimestamp;
			UI.MainUI.FirstProgressStep(4, Lang.WorldGenText[51]);
			Main.SpawnTileX = fileIO.ReadInt16();
			Main.SpawnTileY = fileIO.ReadInt16();
			Main.WorldSurface = fileIO.ReadInt16();
			Main.WorldSurfacePixels = Main.WorldSurface << 4;
			Main.RockLayer = fileIO.ReadInt16();
			Main.RockLayerPixels = Main.RockLayer << 4;
			UpdateMagmaLayerPos();
			tempTime.DayRate = 1f;
			tempTime.WorldTime = fileIO.ReadSingle();
			tempTime.DayTime = fileIO.ReadBoolean();
			tempTime.MoonPhase = fileIO.ReadByte();
			tempTime.IsBloodMoon = fileIO.ReadBoolean();
			Main.DungeonX = fileIO.ReadInt16();
			Main.DungeonY = fileIO.ReadInt16();
			NPC.HasDownedBoss1 = fileIO.ReadBoolean();
			NPC.HasDownedBoss2 = fileIO.ReadBoolean();
			NPC.HasDownedBoss3 = fileIO.ReadBoolean();
			NPC.HasSavedGoblin = fileIO.ReadBoolean();
			NPC.HasSavedWizard = fileIO.ReadBoolean();
			NPC.HasSavedMech = fileIO.ReadBoolean();
			NPC.HasDownedGoblins = fileIO.ReadBoolean();
			NPC.HasDownedClown = fileIO.ReadBoolean();
			NPC.HasDownedFrost = fileIO.ReadBoolean();
			HasShadowOrbSmashed = fileIO.ReadBoolean();
			ToSpawnMeteor = fileIO.ReadBoolean();
			shadowOrbCount = fileIO.ReadByte();
			altarCount = fileIO.ReadInt32();
			Main.InHardMode = fileIO.ReadBoolean();
			Main.InvasionDelay = fileIO.ReadByte();
			Main.InvasionSize = fileIO.ReadInt16();
			Main.InvasionType = fileIO.ReadByte();
			Main.InvasionX = fileIO.ReadSingle();
			for (int i = 0; i < Main.MaxTilesX; i++)
			{
				if ((i & 0x1F) == 0)
				{
					UI.MainUI.Progress = i / (float)Main.MaxTilesX;
				}
				fixed (Tile* ptr = &Main.TileSet[i, 0])
				{
					Tile* ptr2 = ptr;
					int num = 0;
					while (num < Main.MaxTilesY)
					{
						ptr2->CurrentFlags = ~(Flags.WALLFRAME_MASK | Flags.HIGHLIGHT_MASK | Flags.VISITED | Flags.WIRE | Flags.CHECKING_LIQUID | Flags.SKIP_LIQUID);
						ptr2->IsActive = fileIO.ReadByte();
						if (ptr2->IsActive != 0)
						{
							ptr2->Type = fileIO.ReadByte();
							if (ptr2->Type == (byte)EntityID.TileID.ICE)
							{
								ptr2->IsActive = 0;
							}
							if (Main.TileFrameImportant[ptr2->Type])
							{
								ptr2->FrameX = fileIO.ReadInt16();
								ptr2->FrameY = fileIO.ReadInt16();
								if (ptr2->Type == (byte)EntityID.TileID.TIMER)
								{
									ptr2->FrameY = 0;
								}
							}
							else
							{
								ptr2->FrameX = -1;
								ptr2->FrameY = -1;
							}
						}
						ptr2->WallType = fileIO.ReadByte();
						ptr2->Liquid = fileIO.ReadByte();
						if (ptr2->Liquid > 0 && fileIO.ReadBoolean())
						{
							ptr2->Lava = 32;
						}
						ptr2->CurrentFlags |= (Flags)fileIO.ReadByte();
						if (Main.IsTutorial())
						{
							ptr2->CurrentFlags &= ~Flags.VISITED;
						}
						int num2 = fileIO.ReadByte();
						if (((uint)num2 & 0x80u) != 0)
						{
							num2 &= 0x7F;
							num2 |= fileIO.ReadByte() << 7;
						}
						num += num2;
						while (num2 > 0)
						{
							ptr2[1] = *ptr2;
							ptr2++;
							num2--;
						}
						num++;
						ptr2++;
					}
				}
			}
			for (int j = 0; j < Main.MaxNumChests; j++)
			{
				if (!fileIO.ReadBoolean())
				{
					continue;
				}
				Main.ChestSet[j] = new Chest();
				Main.ChestSet[j].XPos = fileIO.ReadInt16();
				Main.ChestSet[j].YPos = fileIO.ReadInt16();
				for (int k = 0; k < Chest.MaxNumItems; k++)
				{
					int b = 0;

					if (release > (int)EntityID.WorldID.INITIAL)
					{
						b = fileIO.ReadInt16();
					}
					else
					{
						b = fileIO.ReadByte();
					}

					if (b > 0)
					{
						Main.ChestSet[j].ItemSet[k].NetDefaults(fileIO.ReadInt16(), b);
						Main.ChestSet[j].ItemSet[k].SetPrefix(fileIO.ReadByte());
					}
				}
			}
			for (int l = 0; l < Sign.MaxNumSigns; l++)
			{
				Main.SignSet[l].Read(fileIO, release);
			}
			bool flag = fileIO.ReadBoolean();
			int num3 = 0;
			while (flag)
			{
				int type = fileIO.ReadByte();
				Main.NPCSet[num3].SetDefaults(type);
				Main.NPCSet[num3].Position.X = fileIO.ReadSingle();
				Main.NPCSet[num3].Position.Y = fileIO.ReadSingle();
				Main.NPCSet[num3].XYWH.X = (int)Main.NPCSet[num3].Position.X;
				Main.NPCSet[num3].XYWH.Y = (int)Main.NPCSet[num3].Position.Y;
				Main.NPCSet[num3].IsHomeless = fileIO.ReadBoolean();
				Main.NPCSet[num3].HomeTileX = fileIO.ReadInt16();
				Main.NPCSet[num3].HomeTileY = fileIO.ReadInt16();
				num3++;
				flag = fileIO.ReadBoolean();
			}
			NPC.TypeNames[(int)EntityID.NPCID.MERCHANT] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.NURSE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.ARMS_DEALER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DRYAD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GUIDE] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.CLOTHIER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.DEMOLITIONIST] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.GOBLIN_TINKERER] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.WIZARD] = fileIO.ReadString();
			NPC.TypeNames[(int)EntityID.NPCID.MECHANIC] = fileIO.ReadString();
#endif
		}

		public static void saveNewWorld()
		{
			if (saveLock)
			{
				return;
			}
			saveLock = true;
			if (hardLock)
			{
				UI.MainUI.statusText = Lang.WorldGenText[48];
				do
				{
					Thread.Sleep(16);
				}
				while (hardLock);
			}
			bool flag = false;
			lock (padlock)
			{
				UI.MainUI.FirstProgressStep(1, Lang.WorldGenText[49]);
				tempTime = Main.GameTime;

				if (Gen)
				{
					tempTime.reset(1f);
					Gen = false;
				}
				using (MemoryStream memoryStream = new MemoryStream(0x600000))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
#if USE_ORIGINAL_CODE
						binaryWriter.Write(Main.NewWorldDataVersion);
						binaryWriter.Write(0u);
						binaryWriter.Write(Main.WorldName);
						binaryWriter.Write(Main.WorldID);
						binaryWriter.Write(Main.WorldTimestamp);
						binaryWriter.Write(Main.RightWorld);
						binaryWriter.Write((short)Main.BottomWorld);
						binaryWriter.Write(Main.MaxTilesY);
						binaryWriter.Write(Main.MaxTilesX);
						binaryWriter.Write(Main.SpawnTileX);
						binaryWriter.Write(Main.SpawnTileY);
						binaryWriter.Write((short)Main.WorldSurface);
						binaryWriter.Write((short)Main.RockLayer);
						binaryWriter.Write(tempTime.WorldTime);
						binaryWriter.Write(tempTime.DayTime);
						binaryWriter.Write(tempTime.MoonPhase);
						binaryWriter.Write(tempTime.IsBloodMoon);
						binaryWriter.Write(Main.DungeonX);
						binaryWriter.Write(Main.DungeonY);
						binaryWriter.Write(NPC.HasDownedBoss1);
						binaryWriter.Write(NPC.HasDownedBoss2);
						binaryWriter.Write(NPC.HasDownedBoss3);
						binaryWriter.Write(NPC.HasSavedGoblin);
						binaryWriter.Write(NPC.HasSavedWizard);
						binaryWriter.Write(NPC.HasSavedMech);
						binaryWriter.Write(NPC.HasDownedGoblins);
						binaryWriter.Write(NPC.HasDownedClown);
						binaryWriter.Write(NPC.HasDownedFrost);
						binaryWriter.Write(HasShadowOrbSmashed);
						binaryWriter.Write(ToSpawnMeteor);
						binaryWriter.Write((byte)shadowOrbCount);
						binaryWriter.Write(altarCount);
						binaryWriter.Write(Main.InHardMode);
						binaryWriter.Write((byte)Main.InvasionDelay);
						binaryWriter.Write((short)Main.InvasionSize);
						binaryWriter.Write((byte)Main.InvasionType);
						binaryWriter.Write(Main.InvasionX);
						for (int i = 0; i < Main.MaxTilesX; i++)
						{
							if ((i & 0x1F) == 0)
							{
								UI.MainUI.Progress = i / (float)Main.MaxTilesX;
							}
							int num;
							for (num = 0; num < Main.MaxTilesY; num++)
							{
								Tile tile = Main.TileSet[i, num];
								if (tile.Type == (byte)EntityID.TileID.ICE)
								{
									tile.IsActive = 0;
								}
								if (tile.IsActive != 0)
								{
									binaryWriter.Write(value: true);
									binaryWriter.Write(tile.Type);
									if (Main.TileFrameImportant[tile.Type])
									{
										binaryWriter.Write(tile.FrameX);
										binaryWriter.Write(tile.FrameY);
									}
								}
								else
								{
									binaryWriter.Write(value: false);
								}
								binaryWriter.Write(tile.WallType);
								binaryWriter.Write(tile.Liquid);
								if (tile.Liquid > 0)
								{
									binaryWriter.Write(tile.Lava != 0);
								}
								binaryWriter.Write((byte)(tile.CurrentFlags & (Flags.VISITED | Flags.WIRE)));
								int j;
								for (j = 1; num + j < Main.MaxTilesY && tile.isTheSameAs(ref Main.TileSet[i, num + j]); j++)
								{
								}
								j--;
								num += j;
								if (j < 128)
								{
									binaryWriter.Write((byte)j);
								}
								else
								{
									int num2 = (j & 0x7F) | 0x80;
									j >>= 7;
									binaryWriter.Write((ushort)(num2 | (j << 8)));
								}
							}
						}
						for (int k = 0; k < Main.MaxNumChests; k++)
						{
							if (Main.ChestSet[k] == null)
							{
								binaryWriter.Write(value: false);
								continue;
							}
							Chest chest = Main.ChestSet[k];
							binaryWriter.Write(value: true);
							binaryWriter.Write(chest.XPos);
							binaryWriter.Write(chest.YPos);
							for (int l = 0; l < Chest.MaxNumItems; l++)
							{
								if (chest.ItemSet[l].Type == 0)
								{
									chest.ItemSet[l].Stack = 0;
								}
								binaryWriter.Write((byte)chest.ItemSet[l].Stack);
								if (chest.ItemSet[l].Stack > 0)
								{
									binaryWriter.Write(chest.ItemSet[l].NetID);
									binaryWriter.Write(chest.ItemSet[l].PrefixType);
								}
							}
						}
						for (int m = 0; m < Sign.MaxNumSigns; m++)
						{
							Sign sign = Main.SignSet[m];
							sign.Write(binaryWriter);
						}
						for (int n = 0; n < NPC.MaxNumNPCs; n++)
						{
							NPC nPC = Main.NPCSet[n];
							if (nPC.IsTownNPC && nPC.Active != 0)
							{
								nPC = (NPC)nPC.Clone();
								binaryWriter.Write(value: true);
								binaryWriter.Write(nPC.Type);
								binaryWriter.Write(nPC.Position.X);
								binaryWriter.Write(nPC.Position.Y);
								binaryWriter.Write(nPC.IsHomeless);
								binaryWriter.Write(nPC.HomeTileX);
								binaryWriter.Write(nPC.HomeTileY);
							}
						}
						binaryWriter.Write(value: false);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.MERCHANT]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.NURSE]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.ARMS_DEALER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.DRYAD]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.GUIDE]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.CLOTHIER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.DEMOLITIONIST]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.GOBLIN_TINKERER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.WIZARD]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.MECHANIC]);
						CRC32 cRC = new CRC32();
						cRC.Update(memoryStream.GetBuffer(), 8, (int)memoryStream.Length - 8);
						binaryWriter.Seek(4, SeekOrigin.Begin);
						binaryWriter.Write(cRC.GetValue());
#else
						binaryWriter.Write(Main.NewWorldDataVersion);
						binaryWriter.Write(0u);
						binaryWriter.Write(Main.WorldName);
						binaryWriter.Write(Main.WorldID);
						binaryWriter.Write(Main.WorldTimestamp);
						binaryWriter.Write(Main.RightWorld);
						binaryWriter.Write((short)Main.BottomWorld);
						binaryWriter.Write(Main.MaxTilesY);
						binaryWriter.Write(Main.MaxTilesX);
						binaryWriter.Write(Main.SpawnTileX);
						binaryWriter.Write(Main.SpawnTileY);
						binaryWriter.Write((short)Main.WorldSurface);
						binaryWriter.Write((short)Main.RockLayer);
						binaryWriter.Write(tempTime.WorldTime);
						binaryWriter.Write(tempTime.DayTime);
						binaryWriter.Write(tempTime.MoonPhase);
						binaryWriter.Write(tempTime.IsBloodMoon);
						binaryWriter.Write(Main.DungeonX);
						binaryWriter.Write(Main.DungeonY);
						binaryWriter.Write(NPC.HasDownedBoss1);
						binaryWriter.Write(NPC.HasDownedBoss2);
						binaryWriter.Write(NPC.HasDownedBoss3);
						binaryWriter.Write(NPC.HasSavedGoblin);
						binaryWriter.Write(NPC.HasSavedWizard);
						binaryWriter.Write(NPC.HasSavedMech);
						binaryWriter.Write(NPC.HasDownedGoblins);
						binaryWriter.Write(NPC.HasDownedClown);
						binaryWriter.Write(NPC.HasDownedFrost);
						binaryWriter.Write(HasShadowOrbSmashed);
						binaryWriter.Write(ToSpawnMeteor);
						binaryWriter.Write((byte)shadowOrbCount);
						binaryWriter.Write(altarCount);
						binaryWriter.Write(Main.InHardMode);
						binaryWriter.Write((byte)Main.InvasionDelay);
						binaryWriter.Write((short)Main.InvasionSize);
						binaryWriter.Write((byte)Main.InvasionType);
						binaryWriter.Write(Main.InvasionX);
						for (int i = 0; i < Main.MaxTilesX; i++)
						{
							if ((i & 0x1F) == 0)
							{
								UI.MainUI.Progress = i / (float)Main.MaxTilesX;
							}
							int num;
							for (num = 0; num < Main.MaxTilesY; num++)
							{
								Tile tile = Main.TileSet[i, num];
								if (tile.Type == (byte)EntityID.TileID.ICE)
								{
									tile.IsActive = 0;
								}
								if (tile.IsActive != 0)
								{
									binaryWriter.Write(value: true);
									binaryWriter.Write(tile.Type);
									if (Main.TileFrameImportant[tile.Type])
									{
										binaryWriter.Write(tile.FrameX);
										binaryWriter.Write(tile.FrameY);
									}
								}
								else
								{
									binaryWriter.Write(value: false);
								}
								binaryWriter.Write(tile.WallType);
								binaryWriter.Write(tile.Liquid);
								if (tile.Liquid > 0)
								{
									binaryWriter.Write(tile.Lava != 0);
								}
								binaryWriter.Write((byte)(tile.CurrentFlags & (Flags.VISITED | Flags.WIRE)));
								int j;
								for (j = 1; num + j < Main.MaxTilesY && tile.isTheSameAs(ref Main.TileSet[i, num + j]); j++)
								{
								}
								j--;
								num += j;
								if (j < 128)
								{
									binaryWriter.Write((byte)j);
								}
								else
								{
									int num2 = (j & 0x7F) | 0x80;
									j >>= 7;
									binaryWriter.Write((ushort)(num2 | (j << 8)));
								}
							}
						}
						for (int k = 0; k < Main.MaxNumChests; k++)
						{
							if (Main.ChestSet[k] == null)
							{
								binaryWriter.Write(value: false);
								continue;
							}
							Chest chest = Main.ChestSet[k];
							binaryWriter.Write(value: true);
							binaryWriter.Write(chest.XPos);
							binaryWriter.Write(chest.YPos);
							for (int l = 0; l < Chest.MaxNumItems; l++)
							{
								if (chest.ItemSet[l].Type == 0)
								{
									chest.ItemSet[l].Stack = 0;
								}

								if (Main.NewWorldDataVersion >= (int)EntityID.WorldID.V101)
								{
									binaryWriter.Write(chest.ItemSet[l].Stack); // Stacks can now go up to 999, meaning no more 255 (FF) limits, bytes begone.
								}
								else
								{
									binaryWriter.Write((byte)chest.ItemSet[l].Stack);
								}
								if (chest.ItemSet[l].Stack > 0)
								{
									binaryWriter.Write(chest.ItemSet[l].NetID);
									binaryWriter.Write(chest.ItemSet[l].PrefixType);
								}
							}
						}
						for (int m = 0; m < Sign.MaxNumSigns; m++)
						{
							Sign sign = Main.SignSet[m];
							sign.Write(binaryWriter);
						}
						for (int n = 0; n < NPC.MaxNumNPCs; n++)
						{
							NPC nPC = Main.NPCSet[n];
							if (nPC.IsTownNPC && nPC.Active != 0)
							{
								nPC = (NPC)nPC.Clone();
								binaryWriter.Write(value: true);
								binaryWriter.Write(nPC.Type);
								binaryWriter.Write(nPC.Position.X);
								binaryWriter.Write(nPC.Position.Y);
								binaryWriter.Write(nPC.IsHomeless);
								binaryWriter.Write(nPC.HomeTileX);
								binaryWriter.Write(nPC.HomeTileY);
							}
						}
						binaryWriter.Write(value: false);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.MERCHANT]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.NURSE]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.ARMS_DEALER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.DRYAD]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.GUIDE]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.CLOTHIER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.DEMOLITIONIST]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.GOBLIN_TINKERER]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.WIZARD]);
						binaryWriter.Write(NPC.TypeNames[(int)EntityID.NPCID.MECHANIC]);

						if (Main.NewWorldDataVersion <= (int)EntityID.WorldID.INITIAL)
						{
							CRC32 cRC = new CRC32();
							cRC.Update(memoryStream.GetBuffer(), 8, (int)memoryStream.Length - 8);
							binaryWriter.Seek(4, SeekOrigin.Begin);
							binaryWriter.Write(cRC.GetValue());
						}
#endif
						Main.ShowSaveIcon();
						try
						{
							if (UI.MainUI.TestStorageSpace("Worlds", WorldSelect.worldPathName, (int)memoryStream.Length))
							{
								using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Worlds"))
								{
#if !USE_ORIGINAL_CODE
									// Just a small precaution to prevent worlds being permanently lost due to corruption (not the in-game kind).
									if (Main.IsGameStarted && storageContainer.FileExists(WorldSelect.worldPathName))
									{
										// Naturally, when you find out your world is corrupted (and filled with dirt or nothing at all), you go back to the menu.
										// Since the world, at this point, still registers as a world, you can exit while saving, which would ruin the backup.
										// As such, I have made it so the backup is only made if you choose the 'Save Game' option while playing, rather than exiting. 
										string BackupPathName = WorldSelect.worldPathName + ".bak";
										using (Stream BackupWorldStream = storageContainer.OpenFile(BackupPathName, FileMode.Create))
										{
											Stream CurrentWorldStream = storageContainer.OpenFile(WorldSelect.worldPathName, FileMode.Open);
											CurrentWorldStream.CopyTo(BackupWorldStream);
											CurrentWorldStream.Close();
											BackupWorldStream.Close();
										}
									}
#endif

									using (Stream stream = storageContainer.OpenFile(WorldSelect.worldPathName, FileMode.Create))
									{
										stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
										stream.Close();
										flag = true;
									}
								}
							}
						}
						catch (IOException)
						{
							UI.MainUI.WriteError();
						}
						catch (Exception)
						{
						}
						binaryWriter.Close();
						Main.HideSaveIcon();
					}
				}
				saveLock = false;
				if (!flag)
				{
					WorldSelect.LoadWorlds();
				}
			}
		}

		private static void resetGen()
		{
			mudWall = false;
			hellChest = 0;
			JungleX = 0;
			numMCaves = 0;
			numIslandHouses = 0;
			houseCount = 0;
			dEnteranceX = 0;
			numDRooms = 0;
			numDDoors = 0;
			numDPlats = 0;
			numJChests = 0;
		}

		public static bool placeTrap(int x2, int y2, int type = -1)
		{
			int num = y2;
			while (!SolidTileUnsafe(x2, num))
			{
				if (++num >= Main.MaxTilesY - 300)
				{
					return false;
				}
			}
			num--;
			if (Main.TileSet[x2, num].Liquid > 0 && Main.TileSet[x2, num].Lava != 0)
			{
				return false;
			}
			if (type == -1 && Main.Rand.Next(20) == 0)
			{
				type = 2;
			}
			else if (type == -1)
			{
				type = Main.Rand.Next(2);
			}
			if (Main.TileSet[x2, num].IsActive != 0 || Main.TileSet[x2 - 1, num].IsActive != 0 || Main.TileSet[x2 + 1, num].IsActive != 0 || Main.TileSet[x2, num - 1].IsActive != 0 || Main.TileSet[x2 - 1, num - 1].IsActive != 0 || Main.TileSet[x2 + 1, num - 1].IsActive != 0 || Main.TileSet[x2, num - 2].IsActive != 0 || Main.TileSet[x2 - 1, num - 2].IsActive != 0 || Main.TileSet[x2 + 1, num - 2].IsActive != 0)
			{
				return false;
			}
			if (Main.TileSet[x2, num + 1].Type == (byte)EntityID.TileID.SPIKE)
			{
				return false;
			}
			switch (type)
			{
				case 0:
					{
						int num7 = x2;
						int num8 = num;
						num8 -= genRand.Next(3);
						while (!SolidTileUnsafe(num7, num8))
						{
							if (--num7 < 0)
							{
								return false;
							}
						}
						int num9 = num7;
						num7 = x2;
						while (!SolidTileUnsafe(num7, num8))
						{
							if (++num7 >= Main.MaxTilesX)
							{
								return false;
							}
						}
						int num10 = num7;
						int num11 = x2 - num9;
						int num12 = num10 - x2;
						bool flag = num11 > 5 && num11 < 50;
						bool flag2 = num12 > 5 && num12 < 50;
						if (flag && !SolidTileUnsafe(num9, num8 + 1))
						{
							flag = false;
						}
						else if (flag && (Main.TileSet[num9, num8].Type == (byte)EntityID.TileID.DOOR_CLOSED || Main.TileSet[num9, num8].Type == (byte)EntityID.TileID.SPIKE || Main.TileSet[num9, num8 + 1].Type == (byte)EntityID.TileID.DOOR_CLOSED || Main.TileSet[num9, num8 + 1].Type == (byte)EntityID.TileID.SPIKE))
						{
							flag = false;
						}
						if (flag2 && !SolidTileUnsafe(num10, num8 + 1))
						{
							flag2 = false;
						}
						else if (flag2 && (Main.TileSet[num10, num8].Type == (byte)EntityID.TileID.DOOR_CLOSED || Main.TileSet[num10, num8].Type == (byte)EntityID.TileID.SPIKE || Main.TileSet[num10, num8 + 1].Type == (byte)EntityID.TileID.DOOR_CLOSED || Main.TileSet[num10, num8 + 1].Type == (byte)EntityID.TileID.SPIKE))
						{
							flag2 = false;
						}
						int num13 = 0;
						if (flag && flag2)
						{
							num13 = 1;
							num7 = num9;
							if (genRand.Next(2) == 0)
							{
								num7 = num10;
								num13 = -1;
							}
						}
						else if (flag2)
						{
							num7 = num10;
							num13 = -1;
						}
						else
						{
							if (!flag)
							{
								return false;
							}
							num7 = num9;
							num13 = 1;
						}
						PlaceTile(x2, num, (byte)EntityID.TileID.PRESSURE_PLATE, ToMute: true, IsForced: true, -1, (Main.TileSet[x2, num].WallType > 0) ? 2 : genRand.Next(2, 4));
						KillTile(num7, num8);
						PlaceTile(num7, num8, (byte)EntityID.TileID.TRAP, ToMute: true, IsForced: true, -1, num13);
						int num14 = x2;
						int num15 = num;
						while (num14 != num7 || num15 != num8)
						{
							Main.TileSet[num14, num15].wire = 16;
							if (num14 > num7)
							{
								num14--;
							}
							if (num14 < num7)
							{
								num14++;
							}
							Main.TileSet[num14, num15].wire = 16;
							if (num15 > num8)
							{
								num15--;
							}
							if (num15 < num8)
							{
								num15++;
							}
							Main.TileSet[num14, num15].wire = 16;
						}
						return true;
					}
				case 1:
					{
						int num16 = x2;
						int num17 = num - 8;
						num16 += genRand.Next(-1, 2);
						bool flag3 = true;
						while (flag3)
						{
							bool flag4 = true;
							int num18 = 0;
							for (int l = num16 - 2; l <= num16 + 3; l++)
							{
								for (int m = num17; m <= num17 + 3; m++)
								{
									if (!SolidTileUnsafe(l, m))
									{
										flag4 = false;
									}
									if (Main.TileSet[l, m].IsActive != 0 && (Main.TileSet[l, m].Type == 0 || Main.TileSet[l, m].Type == (byte)EntityID.TileID.STONE || Main.TileSet[l, m].Type == (byte)EntityID.TileID.MUD))
									{
										num18++;
									}
								}
							}
							num17--;
							if (num17 < Main.WorldSurface)
							{
								return false;
							}
							if (flag4 && num18 > 2)
							{
								flag3 = false;
							}
						}
						if (num - num17 <= 5 || num - num17 >= 40)
						{
							return false;
						}
						for (int n = num16; n <= num16 + 1; n++)
						{
							for (int num19 = num17; num19 <= num; num19++)
							{
								if (SolidTileUnsafe(n, num19))
								{
									KillTile(n, num19);
								}
							}
						}
						for (int num20 = num16 - 2; num20 <= num16 + 3; num20++)
						{
							for (int num21 = num17 - 2; num21 <= num17 + 3; num21++)
							{
								if (SolidTileUnsafe(num20, num21))
								{
									Main.TileSet[num20, num21].Type = (byte)EntityID.TileID.STONE;
								}
							}
						}
						PlaceTile(x2, num, (byte)EntityID.TileID.PRESSURE_PLATE, ToMute: true, IsForced: true, -1, genRand.Next(2, 4));
						PlaceTile(num16, num17 + 2, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						PlaceTile(num16 + 1, num17 + 2, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						PlaceTile(num16 + 1, num17 + 1, (byte)EntityID.TileID.BOULDER, ToMute: true);
						num17 += 2;
						Main.TileSet[num16, num17].wire = 16;
						Main.TileSet[num16 + 1, num17].wire = 16;
						num17++;
						PlaceTile(num16, num17, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						PlaceTile(num16 + 1, num17, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						Main.TileSet[num16, num17].wire = 16;
						Main.TileSet[num16 + 1, num17].wire = 16;
						PlaceTile(num16, num17 + 1, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						PlaceTile(num16 + 1, num17 + 1, (byte)EntityID.TileID.ACTIVE_STONE, ToMute: true);
						Main.TileSet[num16, num17 + 1].wire = 16;
						Main.TileSet[num16 + 1, num17 + 1].wire = 16;
						int num22 = x2;
						int num23 = num;
						while (num22 != num16 || num23 != num17)
						{
							Main.TileSet[num22, num23].wire = 16;
							if (num22 > num16)
							{
								num22--;
							}
							if (num22 < num16)
							{
								num22++;
							}
							Main.TileSet[num22, num23].wire = 16;
							if (num23 > num17)
							{
								num23--;
							}
							if (num23 < num17)
							{
								num23++;
							}
							Main.TileSet[num22, num23].wire = 16;
						}
						return true;
					}
				case 2:
					{
						int num2 = Main.Rand.Next(4, 7);
						int num3 = x2;
						num3 += Main.Rand.Next(-1, 2);
						int num4 = num;
						for (int i = 0; i < num2; i++)
						{
							num4++;
							if (!SolidTileUnsafe(num3, num4))
							{
								return false;
							}
						}
						for (int j = num3 - 2; j <= num3 + 2; j++)
						{
							for (int k = num4 - 2; k <= num4 + 2; k++)
							{
								if (!SolidTileUnsafe(j, k))
								{
									return false;
								}
							}
						}
						KillTile(num3, num4);
						Main.TileSet[num3, num4].IsActive = 1;
						Main.TileSet[num3, num4].Type = (byte)EntityID.TileID.EXPLOSIVES;
						Main.TileSet[num3, num4].FrameX = 0;
						Main.TileSet[num3, num4].FrameY = (short)(18 * Main.Rand.Next(2));
						PlaceTile(x2, num, (byte)EntityID.TileID.PRESSURE_PLATE, ToMute: true, IsForced: true, -1, genRand.Next(2, 4));
						int num5 = x2;
						int num6 = num;
						while (num5 != num3 || num6 != num4)
						{
							Main.TileSet[num5, num6].wire = 16;
							if (num5 > num3)
							{
								num5--;
							}
							if (num5 < num3)
							{
								num5++;
							}
							Main.TileSet[num5, num6].wire = 16;
							if (num6 > num4)
							{
								num6--;
							}
							if (num6 < num4)
							{
								num6++;
							}
							Main.TileSet[num5, num6].wire = 16;
						}
						break;
					}
			}
			return false;
		}

		public unsafe static void generateWorld(int seed = -1)
		{
			Time.CheckXMas();
			NPC.ClearNames();
			NPC.SetNames();
			Gen = true;
			resetGen();
			UI.MainUI.FirstProgressStep(47, Lang.WorldGenText[0]);
			if (seed > 0)
			{
				genRand = new FastRandom((uint)seed);
			}
			Main.WorldID = genRand.Next();
			Main.WorldTimestamp = (int)(DateTime.UtcNow.Ticks / 10000000);
			int num = 0;
			int num2 = 0;
			float num3 = Main.MaxTilesY * 0.3f;
			num3 *= genRand.Next(90, 110) * 0.005f;
			float num4 = num3 + Main.MaxTilesY * 0.2f;
			num4 *= genRand.Next(90, 110) * 0.01f;
			float num5 = num3;
			float num6 = num3;
			float num7 = num4;
			float num8 = num4;
			int num9 = (genRand.Next(2) << 1) - 1;
			float num10 = 1f / Main.MaxTilesX;
			try
			{
				fixed (Tile* ptr = Main.TileSet)
				{
					for (int i = 0; i < Main.MaxTilesX; i++)
					{
						UI.MainUI.Progress = i * num10;
						if (num3 < num5)
						{
							num5 = num3;
						}
						if (num3 > num6)
						{
							num6 = num3;
						}
						if (num4 < num7)
						{
							num7 = num4;
						}
						if (num4 > num8)
						{
							num8 = num4;
						}
						if (--num2 <= 0)
						{
							num = genRand.Next(5);
							num2 = genRand.Next(5, 40);
							if (num == 0)
							{
								num2 *= genRand.Next(1, 6);
							}
						}
						switch (num)
						{
							case 0:
								while (genRand.Next(7) == 0)
								{
									num3 += genRand.Next(-1, 2);
								}
								break;
							case 1:
								while (genRand.Next(4) == 0)
								{
									num3 -= 1f;
								}
								while (genRand.Next(10) == 0)
								{
									num3 += 1f;
								}
								break;
							case 2:
								while (genRand.Next(4) == 0)
								{
									num3 += 1f;
								}
								while (genRand.Next(10) == 0)
								{
									num3 -= 1f;
								}
								break;
							case 3:
								while (genRand.Next(2) == 0)
								{
									num3 -= 1f;
								}
								while (genRand.Next(6) == 0)
								{
									num3 += 1f;
								}
								break;
							default:
								while (genRand.Next(2) == 0)
								{
									num3 += 1f;
								}
								while (genRand.Next(5) == 0)
								{
									num3 -= 1f;
								}
								break;
						}
						if (num3 < Main.MaxTilesY * 0.17f)
						{
							num3 = Main.MaxTilesY * 0.17f;
							num2 = 0;
						}
						else if (num3 > Main.MaxTilesY * 0.3f)
						{
							num3 = Main.MaxTilesY * 0.3f;
							num2 = 0;
						}
						if ((i < 275 || i > Main.MaxTilesX - 275) && num3 > Main.MaxTilesY >> 2)
						{
							num3 = Main.MaxTilesY >> 2;
							num2 = 1;
						}
						while (genRand.Next(3) == 0)
						{
							num4 += genRand.Next(-2, 3);
						}
						if ((double)num4 < (double)num3 + Main.MaxTilesY * 0.05)
						{
							num4 += 1f;
						}
						else if ((double)num4 > (double)num3 + Main.MaxTilesY * 0.35)
						{
							num4 -= 1f;
						}
						int num11 = Main.MaxTilesY - 1;
						Tile* ptr2 = ptr + (i * (Main.LargeWorldH) + num11);
						do
						{
							ptr2->IsActive = 1;
							ptr2->Type = (byte)((num11 >= (int)num4) ? (byte)EntityID.TileID.STONE : (byte)EntityID.TileID.DIRT);
							ptr2->FrameX = -1;
							ptr2->FrameY = -1;
							ptr2--;
						}
						while (--num11 >= (int)num3);
						do
						{
							ptr2->IsActive = 0;
							ptr2->FrameX = -1;
							ptr2->FrameY = -1;
							ptr2--;
						}
						while (--num11 >= 0);
					}
					Main.WorldSurface = (int)num6 + 25;
					Main.WorldSurfacePixels = Main.WorldSurface << 4;
					int num12 = (int)((num8 - Main.WorldSurface) * (355f / (678f * (float)Math.PI))) * 6;
					Main.RockLayer = Main.WorldSurface + num12;
					Main.RockLayerPixels = Main.RockLayer << 4;
					UpdateMagmaLayerPos();
					WaterLine = Main.RockLayer + Main.MaxTilesY >> 1;
					WaterLine += genRand.Next(-100, 20);
					lavaLine = WaterLine + genRand.Next(50, 80);
					int num13 = 0;
					Location[] array = new Location[10];
					for (int j = 0; j < (int)(Main.MaxTilesX * 0.0015f); j++)
					{
						int num14 = genRand.Next(450, Main.MaxTilesX - 450);
						int k = 0;
						for (int l = 0; l < 10; l++)
						{
							for (; Main.TileSet[num14, k].IsActive == 0; k++)
							{
							}
							array[l].X = (short)num14;
							array[l].Y = (short)(k - genRand.Next(11, 16));
							num14 += genRand.Next(5, 11);
						}
						for (int m = 0; m < 10; m++)
						{
							TileRunner(array[m].X, array[m].Y, genRand.Next(5, 8), genRand.Next(6, 9), (byte)EntityID.TileID.DIRT, addTile: true, new Vector2(-2f, -0.3f));
							TileRunner(array[m].X, array[m].Y, genRand.Next(5, 8), genRand.Next(6, 9), (byte)EntityID.TileID.DIRT, addTile: true, new Vector2(2f, -0.3f));
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[1]);
					int num15 = 2 + genRand.Next((int)(Main.MaxTilesX * 0.0008f), (int)(Main.MaxTilesX * 0.0025f));
					for (int n = 0; n < num15; n++)
					{
						int num16 = genRand.Next(Main.MaxTilesX);
						while (num16 > Main.MaxTilesX * 0.4f && num16 < Main.MaxTilesX * 0.6f)
						{
							num16 = genRand.Next(Main.MaxTilesX);
						}
						int num17 = genRand.Next(35, 90);
						if (n == 1)
						{
							float num18 = Main.MaxTilesX / 4200f;
							num17 += (int)(genRand.Next(20, 40) * num18);
						}
						if (genRand.Next(3) == 0)
						{
							num17 *= 2;
						}
						if (n == 1)
						{
							num17 *= 2;
						}
						int num19 = num16 - num17;
						num17 = genRand.Next(35, 90);
						if (genRand.Next(3) == 0)
						{
							num17 *= 2;
						}
						if (n == 1)
						{
							num17 *= 2;
						}
						int num20 = num16 + num17;
						if (num19 < 0)
						{
							num19 = 0;
						}
						if (num20 > Main.MaxTilesX)
						{
							num20 = Main.MaxTilesX;
						}
						switch (n)
						{
							case 0:
								num19 = 0;
								num20 = genRand.Next(260, 300);
								if (num9 == 1)
								{
									num20 += 40;
								}
								break;
							case 2:
								num19 = Main.MaxTilesX - genRand.Next(260, 300);
								num20 = Main.MaxTilesX;
								if (num9 == -1)
								{
									num19 -= 40;
								}
								break;
						}
						int num21 = genRand.Next(50, 100);
						for (int num22 = num19; num22 < num20; num22++)
						{
							if (genRand.Next(2) == 0)
							{
								num21 += genRand.Next(-1, 2);
								if (num21 < 50)
								{
									num21 = 50;
								}
								if (num21 > 100)
								{
									num21 = 100;
								}
							}
							for (int num23 = 0; num23 < Main.WorldSurface; num23++)
							{
								if (Main.TileSet[num22, num23].IsActive == 0)
								{
									continue;
								}
								int num24 = num21;
								if (num22 - num19 < num24)
								{
									num24 = num22 - num19;
								}
								if (num20 - num22 < num24)
								{
									num24 = num20 - num22;
								}
								num24 += genRand.Next(5);
								for (int num25 = num23; num25 < num23 + num24; num25++)
								{
									if (num22 > num19 + genRand.Next(5) && num22 < num20 - genRand.Next(5))
									{
										Main.TileSet[num22, num25].Type = (byte)EntityID.TileID.SAND;
									}
								}
								break;
							}
						}
					}
					for (int num26 = (int)(Main.MaxTilesX * Main.MaxTilesY * 8E-06f); num26 > 0; num26--)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next(Main.WorldSurface, Main.RockLayer), genRand.Next(15, 70), genRand.Next(20, 130), (byte)EntityID.TileID.SAND);
					}
					numMCaves = 0;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[2]);
					for (int num27 = 0; num27 < (int)(Main.MaxTilesX * 0.0008f); num27++)
					{
						int num28 = 0;
						bool flag = false;
						int num29 = genRand.Next((int)(Main.MaxTilesX * 0.25f), (int)(Main.MaxTilesX * 0.75f));
						bool flag2;
						do
						{
							flag2 = true;
							while (num29 > (Main.MaxTilesX >> 1) - 100 && num29 < (Main.MaxTilesX >> 1) + 100)
							{
								num29 = genRand.Next((int)(Main.MaxTilesX * 0.25f), (int)(Main.MaxTilesX * 0.75f));
							}
							for (int num30 = 0; num30 < numMCaves; num30++)
							{
								if (num29 > mCave[num30].X - 50 && num29 < mCave[num30].X + 50)
								{
									num28++;
									flag2 = false;
									break;
								}
							}
							if (num28 >= 200)
							{
								flag = true;
								break;
							}
						}
						while (!flag2);
						if (flag)
						{
							continue;
						}
						for (int num31 = 0; num31 < Main.WorldSurface; num31++)
						{
							if (Main.TileSet[num29, num31].IsActive != 0)
							{
								Mountinater(num29, num31);
								mCave[numMCaves].X = (short)num29;
								mCave[numMCaves].Y = (short)num31;
								numMCaves++;
								break;
							}
						}
					}
					bool flag3 = Time.xMas;
					if (genRand.Next(3) == 0)
					{
						flag3 = true;
					}
					if (flag3)
					{
						UI.MainUI.statusText = Lang.WorldGenText[56];
						int num32 = genRand.Next(Main.MaxTilesX / 3, (Main.MaxTilesX << 1) / 3);
						int num33 = genRand.Next(35, 90);
						float num34 = Main.MaxTilesX / 2100f;
						num33 += (int)(genRand.Next(20, 40) * num34);
						int num35 = num32 - num33;
						if (num35 < 0)
						{
							num35 = 0;
						}
						num33 = genRand.Next(35, 90);
						num33 += (int)(genRand.Next(20, 40) * num34);
						int num36 = num32 + num33;
						if (num36 > Main.MaxTilesX)
						{
							num36 = Main.MaxTilesX;
						}
						int num37 = genRand.Next(50, 100);
						for (int num38 = num35; num38 < num36; num38++)
						{
							if (genRand.Next(2) == 0)
							{
								num37 += genRand.Next(-1, 2);
								if (num37 < 50)
								{
									num37 = 50;
								}
								if (num37 > 100)
								{
									num37 = 100;
								}
							}
							for (int num39 = 0; num39 < Main.WorldSurface; num39++)
							{
								if (Main.TileSet[num38, num39].IsActive == 0)
								{
									continue;
								}
								int num40 = num37;
								if (num38 - num35 < num40)
								{
									num40 = num38 - num35;
								}
								if (num36 - num38 < num40)
								{
									num40 = num36 - num38;
								}
								num40 += genRand.Next(5);
								for (int num41 = num39; num41 < num39 + num40; num41++)
								{
									if (num38 > num35 + genRand.Next(5) && num38 < num36 - genRand.Next(5))
									{
										Main.TileSet[num38, num41].Type = (byte)EntityID.TileID.SNOW;
									}
								}
								break;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[3]);
					for (int num42 = 1; num42 < Main.MaxTilesX - 1; num42++)
					{
						UI.MainUI.Progress = num42 / (float)Main.MaxTilesX;
						bool flag4 = false;
						num13 += genRand.Next(-1, 2);
						if (num13 < 0)
						{
							num13 = 0;
						}
						if (num13 > 10)
						{
							num13 = 10;
						}
						for (int num43 = 0; num43 < Main.WorldSurface + 10 && num43 <= Main.WorldSurface + num13; num43++)
						{
							if (flag4)
							{
								Main.TileSet[num42, num43].WallType = (byte)EntityID.WallID.DIRT_UNSAFE;
							}
							if (Main.TileSet[num42, num43].IsActive != 0 && Main.TileSet[num42 - 1, num43].IsActive != 0 && Main.TileSet[num42 + 1, num43].IsActive != 0 && Main.TileSet[num42, num43 + 1].IsActive != 0 && Main.TileSet[num42 - 1, num43 + 1].IsActive != 0 && Main.TileSet[num42 + 1, num43 + 1].IsActive != 0)
							{
								flag4 = true;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[4]);
					for (int num44 = 0; num44 < (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0002f); num44++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5 + 1), genRand.Next(4, 15), genRand.Next(5, 40), (byte)EntityID.TileID.STONE);
					}
					for (int num45 = 0; num45 < (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0002f); num45++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5, (int)num6 + 1), genRand.Next(4, 10), genRand.Next(5, 30), (byte)EntityID.TileID.STONE);
					}
					for (int num46 = 0; num46 < (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0045f); num46++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, (int)num8 + 1), genRand.Next(2, 7), genRand.Next(2, 23), (byte)EntityID.TileID.STONE);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[5]);
					for (int num47 = 0; num47 < (int)(Main.MaxTilesX * Main.MaxTilesY * 0.005f); num47++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(2, 6), genRand.Next(2, 40), (byte)EntityID.TileID.DIRT);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[6]);
					for (int num48 = 0; num48 < (int)(Main.MaxTilesX * Main.MaxTilesY * 2E-05f); num48++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5), genRand.Next(4, 14), genRand.Next(10, 50), (byte)EntityID.TileID.CLAY);
					}
					for (int num49 = 0; num49 < (int)(Main.MaxTilesX * Main.MaxTilesY * 5E-05f); num49++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5, (int)num6 + 1), genRand.Next(8, 14), genRand.Next(15, 45), (byte)EntityID.TileID.CLAY);
					}
					for (int num50 = 0; num50 < (int)(Main.MaxTilesX * Main.MaxTilesY * 2E-05f); num50++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, (int)num8 + 1), genRand.Next(8, 15), genRand.Next(5, 50), (byte)EntityID.TileID.CLAY);
					}
					for (int num51 = 5; num51 < Main.MaxTilesX - 5; num51++)
					{
						for (int num52 = 1; num52 < Main.WorldSurface - 1; num52++)
						{
							if (Main.TileSet[num51, num52].IsActive == 0)
							{
								continue;
							}
							for (int num53 = num52; num53 < num52 + 5; num53++)
							{
								if (Main.TileSet[num51, num53].Type == (byte)EntityID.TileID.CLAY)
								{
									Main.TileSet[num51, num53].Type = (byte)EntityID.TileID.DIRT;
								}
							}
							break;
						}
					}
					int num54 = 0;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[7]);
					int num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0015f);
					for (int num56 = 0; num56 < num55; num56++)
					{
						UI.MainUI.Progress = num56 / (float)num55;
						int type = (int)EntityID.TileID.NONE;
						if (genRand.Next(5) == 0)
						{
							type = (int)EntityID.TileID.LIQUID;
						}
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, Main.MaxTilesY), genRand.Next(2, 5), genRand.Next(2, 20), type);
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, Main.MaxTilesY), genRand.Next(8, 15), genRand.Next(7, 30), type);
					}
					if (num8 <= Main.MaxTilesY)
					{
						UI.MainUI.NextProgressStep(Lang.WorldGenText[8]);
						num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 3E-05f);
						for (int num57 = 0; num57 < num55; num57++)
						{
							UI.MainUI.Progress = num57 / (float)num55;
							int type2 = (int)EntityID.TileID.NONE;
							if (genRand.Next(6) == 0)
							{
								type2 = (int)EntityID.TileID.LIQUID;
							}
							TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5, (int)num8 + 1), genRand.Next(5, 15), genRand.Next(30, 200), type2);
						}
					}
					if (num8 <= Main.MaxTilesY)
					{
						UI.MainUI.NextProgressStep(Lang.WorldGenText[9]);
						num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.00013f);
						for (int num58 = 0; num58 < num55; num58++)
						{
							UI.MainUI.Progress = num58 / (float)num55;
							int type3 = (int)EntityID.TileID.NONE;
							if (genRand.Next(10) == 0)
							{
								type3 = (int)EntityID.TileID.LIQUID;
							}
							TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num8, Main.MaxTilesY), genRand.Next(6, 20), genRand.Next(50, 300), type3);
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[10]);
					num55 = (int)(Main.MaxTilesX * 0.0025f);
					for (int num59 = 0; num59 < num55; num59++)
					{
						num54 = genRand.Next(Main.MaxTilesX);
						for (int num60 = 0; num60 < num6; num60++)
						{
							if (Main.TileSet[num54, num60].IsActive != 0)
							{
								TileRunner(num54, num60, genRand.Next(3, 6), genRand.Next(5, 50), (int)EntityID.TileID.NONE, addTile: false, new Vector2(genRand.Next(-10, 11) * 0.1f, 1f));
								break;
							}
						}
					}
					num55 = (int)(Main.MaxTilesX * 0.0007f);
					for (int num61 = 0; num61 < num55; num61++)
					{
						num54 = genRand.Next(Main.MaxTilesX);
						for (int num62 = 0; num62 < num6; num62++)
						{
							if (Main.TileSet[num54, num62].IsActive != 0)
							{
								TileRunner(num54, num62, genRand.Next(10, 15), genRand.Next(50, 130), (int)EntityID.TileID.NONE, addTile: false, new Vector2(genRand.Next(-10, 11) * 0.1f, 2f));
								break;
							}
						}
					}
					num55 = (int)(Main.MaxTilesX * 0.0003f);
					for (int num63 = 0; num63 < num55; num63++)
					{
						num54 = genRand.Next(Main.MaxTilesX);
						for (int num64 = 0; num64 < num6; num64++)
						{
							if (Main.TileSet[num54, num64].IsActive != 0)
							{
								TileRunner(num54, num64, genRand.Next(12, 25), genRand.Next(150, 500), (int)EntityID.TileID.NONE, addTile: false, new Vector2(genRand.Next(-10, 11) * 0.1f, 4f));
								TileRunner(num54, num64, genRand.Next(8, 17), genRand.Next(60, 200), (int)EntityID.TileID.NONE, addTile: false, new Vector2(genRand.Next(-10, 11) * 0.1f, 2f));
								TileRunner(num54, num64, genRand.Next(5, 13), genRand.Next(40, 170), (int)EntityID.TileID.NONE, addTile: false, new Vector2(genRand.Next(-10, 11) * 0.1f, 2f));
								break;
							}
						}
					}
					num55 = (int)(Main.MaxTilesX * 0.0004f);
					for (int num65 = 0; num65 < num55; num65++)
					{
						num54 = genRand.Next(Main.MaxTilesX);
						for (int num66 = 0; num66 < num6; num66++)
						{
							if (Main.TileSet[num54, num66].IsActive != 0)
							{
								TileRunner(num54, num66, genRand.Next(7, 12), genRand.Next(150, 250), (int)EntityID.TileID.NONE, addTile: false, new Vector2(0f, 1f), noYChange: true);
								break;
							}
						}
					}
					num55 = (int)(Main.MaxTilesX * (5f / 4200f));
					for (int num67 = 0; num67 < num55; num67++)
					{
						Caverer(genRand.Next(100, Main.MaxTilesX - 100), genRand.Next(Main.RockLayer, Main.MaxTilesY - 400));
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.002f);
					for (int num68 = 0; num68 < num55; num68++)
					{
						int num69 = genRand.Next(1, Main.MaxTilesX - 1);
						int num70 = genRand.Next((int)num5, (int)num6);
						if (num70 >= Main.MaxTilesY)
						{
							num70 = Main.MaxTilesY - 2;
						}
						if (Main.TileSet[num69 - 1, num70].IsActive != 0 && Main.TileSet[num69 - 1, num70].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69 + 1, num70].IsActive != 0 && Main.TileSet[num69 + 1, num70].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69, num70 - 1].IsActive != 0 && Main.TileSet[num69, num70 - 1].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69, num70 + 1].IsActive != 0 && Main.TileSet[num69, num70 + 1].Type == (byte)EntityID.TileID.DIRT)
						{
							Main.TileSet[num69, num70].IsActive = 1;
							Main.TileSet[num69, num70].Type = (byte)EntityID.TileID.GRASS;
						}
						num69 = genRand.Next(1, Main.MaxTilesX - 1);
						num70 = genRand.Next((int)num5);
						if (num70 >= Main.MaxTilesY)
						{
							num70 = Main.MaxTilesY - 2;
						}
						if (Main.TileSet[num69 - 1, num70].IsActive != 0 && Main.TileSet[num69 - 1, num70].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69 + 1, num70].IsActive != 0 && Main.TileSet[num69 + 1, num70].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69, num70 - 1].IsActive != 0 && Main.TileSet[num69, num70 - 1].Type == (byte)EntityID.TileID.DIRT && Main.TileSet[num69, num70 + 1].IsActive != 0 && Main.TileSet[num69, num70 + 1].Type == (byte)EntityID.TileID.DIRT)
						{
							Main.TileSet[num69, num70].IsActive = 1;
							Main.TileSet[num69, num70].Type = (byte)EntityID.TileID.GRASS;
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[11]);
					int num71 = 0;
					float num72 = genRand.Next(15, 30) * 0.01f;
					if (num9 == -1)
					{
						num72 = 1f - num72;
					}
					num71 = (int)(Main.MaxTilesX * num72);
					int num73 = Main.MaxTilesY + Main.RockLayer >> 1;
					float num74 = Main.MaxTilesX * (1.5f / 4200f);
					num71 += genRand.Next((int)(-100f * num74), (int)(101f * num74));
					num73 += genRand.Next((int)(-100f * num74), (int)(101f * num74));
					int num75 = num71;
					int num76 = num73;
					TileRunner(num71, num73, genRand.Next((int)(250f * num74), (int)(500f * num74)), genRand.Next(50, 150), (byte)EntityID.TileID.MUD, addTile: false, new Vector2(num9 * 3, 0f));
					for (int num77 = (int)(6f * num74); num77 > 0; num77--)
					{
						TileRunner(num71 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), num73 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), genRand.Next(3, 7), genRand.Next(3, 8), genRand.Next((byte)EntityID.TileID.SAPPHIRE, (byte)EntityID.TileID.EMERALD));
					}
					UI.MainUI.Progress = 0.15f;
					num71 += genRand.Next((int)(-250f * num74), (int)(251f * num74));
					num73 += genRand.Next((int)(-150f * num74), (int)(151f * num74));
					int num78 = num71;
					int num79 = num73;
					int num80 = num71;
					int num81 = num73;
					mudWall = true;
					TileRunner(num71, num73, genRand.Next((int)(250f * num74), (int)(500f * num74)), genRand.Next(50, 150), (byte)EntityID.TileID.MUD);
					mudWall = false;
					for (int num82 = (int)(6f * num74); num82 > 0; num82--)
					{
						TileRunner(num71 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), num73 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), genRand.Next(3, 7), genRand.Next(3, 8), genRand.Next((byte)EntityID.TileID.EMERALD, (byte)EntityID.TileID.AMETHYST));
					}
					mudWall = true;
					UI.MainUI.Progress = 0.3f;
					num71 += genRand.Next((int)(-400f * num74), (int)(401f * num74));
					num73 += genRand.Next((int)(-150f * num74), (int)(151f * num74));
					int num83 = num71;
					int num84 = num73;
					TileRunner(num71, num73, genRand.Next((int)(250f * num74), (int)(500f * num74)), genRand.Next(50, 150), (byte)EntityID.TileID.MUD, addTile: false, new Vector2(num9 * -3, 0f));
					mudWall = false;
					for (int num85 = (int)(6f * num74); num85 > 0; num85--)
					{
						TileRunner(num71 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), num73 + genRand.Next(-(int)(125f * num74), (int)(125f * num74)), genRand.Next(3, 7), genRand.Next(3, 8), genRand.Next((byte)EntityID.TileID.AMETHYST, (byte)EntityID.TileID.DIAMOND + 1));
					}
					mudWall = true;
					UI.MainUI.Progress = 0.45f;
					num71 = (num75 + num78 + num83) / 3;
					num73 = (num76 + num79 + num84) / 3;
					TileRunner(num71, num73, genRand.Next((int)(400f * num74), (int)(600f * num74)), 10000, (byte)EntityID.TileID.MUD, addTile: false, new Vector2(0f, -20f), noYChange: true);
					JungleRunner(num71, num73);
					UI.MainUI.Progress = 0.6f;
					mudWall = false;
					List<uint> list = new List<uint>();
					int num86 = 0;
					for (int num87 = 20; num87 < Main.MaxTilesX - 20; num87++)
					{
						for (int num88 = Main.RockLayer; num88 < Main.MaxTilesY - 200; num88++)
						{
							if (Main.TileSet[num87, num88].WallType == (byte)EntityID.WallID.MUD_UNSAFE)
							{
								list.Add((uint)((num71 << 16) | num73));
								num86++;
							}
						}
					}
					int num89 = 0;
					while (num86 > 0 && num89 < Main.MaxTilesX / 10)
					{
						int index = genRand.Next(num86--);
						num71 = (int)(list[index] >> 16);
						num73 = (int)(list[index] & 0xFFFF);
						list.RemoveAt(index);
						MudWallRunner(num71, num73);
						num89++;
					}
					num71 = num80;
					num73 = num81;
					num55 = (int)(20f * num74);
					for (int num90 = 0; num90 <= num55; num90++)
					{
						UI.MainUI.Progress = 0.6f + 0.2f * (num90 / (float)num55);
						num71 += genRand.Next((int)(-5f * num74), (int)(6f * num74));
						num73 += genRand.Next((int)(-5f * num74), (int)(6f * num74));
						TileRunner(num71, num73, genRand.Next(40, 100), genRand.Next(300, 500), (byte)EntityID.TileID.MUD);
					}
					num55 = (int)(10f * num74);
					for (int num91 = 0; num91 <= num55; num91++)
					{
						UI.MainUI.Progress = 0.8f + 0.2f * (num91 / (float)num55);
						num71 = num80 + genRand.Next((int)(-600f * num74), (int)(600f * num74));
						num73 = num81 + genRand.Next((int)(-200f * num74), (int)(200f * num74));
						while (num71 < 1 || num71 >= Main.MaxTilesX - 1 || num73 < 1 || num73 >= Main.MaxTilesY - 1 || Main.TileSet[num71, num73].Type != (byte)EntityID.TileID.MUD)
						{
							num71 = num80 + genRand.Next((int)(-600f * num74), (int)(600f * num74));
							num73 = num81 + genRand.Next((int)(-200f * num74), (int)(200f * num74));
						}
						for (int num92 = 0; num92 < 8f * num74; num92++)
						{
							num71 += genRand.Next(-30, 31);
							num73 += genRand.Next(-30, 31);
							int type4 = (int)EntityID.TileID.NONE;
							if (genRand.Next(7) == 0)
							{
								type4 = (int)EntityID.TileID.LIQUID;
							}
							TileRunner(num71, num73, genRand.Next(10, 20), genRand.Next(30, 70), type4);
						}
					}
					for (int num93 = 0; num93 <= 300f * num74; num93++)
					{
						num71 = num80 + genRand.Next((int)(-600f * num74), (int)(600f * num74));
						num73 = num81 + genRand.Next((int)(-200f * num74), (int)(200f * num74));
						while (num71 < 1 || num71 >= Main.MaxTilesX - 1 || num73 < 1 || num73 >= Main.MaxTilesY - 1 || Main.TileSet[num71, num73].Type != (byte)EntityID.TileID.MUD)
						{
							num71 = num80 + genRand.Next((int)(-600f * num74), (int)(600f * num74));
							num73 = num81 + genRand.Next((int)(-200f * num74), (int)(200f * num74));
						}
						TileRunner(num71, num73, genRand.Next(4, 10), genRand.Next(5, 30), (byte)EntityID.TileID.STONE);
						if (genRand.Next(4) == 0)
						{
							int type5 = genRand.Next((int)EntityID.TileID.SAPPHIRE, (int)EntityID.TileID.DIAMOND + 1);
							TileRunner(num71 + genRand.Next(-1, 2), num73 + genRand.Next(-1, 2), genRand.Next(3, 7), genRand.Next(4, 8), type5);
						}
					}
					int num94 = (int)(genRand.Next(6, 10) * Main.MaxTilesX / 4200f);
					for (int num95 = num94 - 1; num95 >= 0; num95--)
					{
						do
						{
							num71 = genRand.Next(20, Main.MaxTilesX - 20);
							num73 = genRand.Next(Main.WorldSurface + Main.RockLayer >> 1, Main.MaxTilesY - 300);
						}
						while (Main.TileSet[num71, num73].Type != (byte)EntityID.TileID.MUD);
						int num96 = genRand.Next(2, 4);
						int num97 = genRand.Next(2, 4);
						for (int num98 = num71 - num96 - 1; num98 <= num71 + num96 + 1; num98++)
						{
							for (int num99 = num73 - num97 - 1; num99 <= num73 + num97 + 1; num99++)
							{
								Main.TileSet[num98, num99].IsActive = 1;
								Main.TileSet[num98, num99].Type = (byte)EntityID.TileID.GOLD_BRICK;
								Main.TileSet[num98, num99].Liquid = 0;
								Main.TileSet[num98, num99].Lava = 0;
							}
						}
						for (int num100 = num71 - num96; num100 <= num71 + num96; num100++)
						{
							for (int num101 = num73 - num97; num101 <= num73 + num97; num101++)
							{
								Main.TileSet[num100, num101].IsActive = 0;
							}
						}
						int num102 = 0;
						int i2;
						int j2;
						do
						{
							i2 = genRand.Next(num71 - num96, num71 + num96 + 1);
							j2 = genRand.Next(num73 - num97, num73 + num97 - 2);
						}
						while (!PlaceTile(i2, j2, (int)EntityID.TileID.TORCH, ToMute: true) && ++num102 < 100);
						for (int num103 = num73 + num97 - 2; num103 <= num73 + num97 - 1; num103++)
						{
							for (int num104 = num71 - num96 - 1; num104 <= num71 + num96 + 1; num104++)
							{
								Main.TileSet[num104, num103].IsActive = 0;
							}
						}
						for (int num105 = num71 - num96 - 1; num105 <= num71 + num96 + 1; num105++)
						{
							int num106 = 4;
							int num107 = num73 + num97 + 2;
							while (Main.TileSet[num105, num107].IsActive == 0 && num107 < Main.MaxTilesY && num106 > 0)
							{
								Main.TileSet[num105, num107].IsActive = 1;
								Main.TileSet[num105, num107].Type = (byte)EntityID.TileID.MUD;
								num107++;
								num106--;
							}
						}
						num96 -= genRand.Next(1, 3);
						int num108 = num73 - num97 - 2;
						while (num96 >= 0)
						{
							for (int num109 = num71 - num96 - 1; num109 <= num71 + num96 + 1; num109++)
							{
								Main.TileSet[num109, num108].IsActive = 1;
								Main.TileSet[num109, num108].Type = (byte)EntityID.TileID.GOLD_BRICK;
							}
							num96 -= genRand.Next(1, 3);
							num108--;
						}
						JChest[numJChests].X = (short)num71;
						JChest[numJChests].Y = (short)num73;
						numJChests++;
					}
					for (int num110 = 0; num110 < Main.MaxTilesY; num110++)
					{
						for (int num111 = 0; num111 < Main.MaxTilesX; num111++)
						{
							if (Main.TileSet[num111, num110].IsActive != 0)
							{
								try
								{
									grassSpread = 0;
									SpreadGrass(num111, num110, (int)EntityID.TileID.MUD, (int)EntityID.TileID.JUNGLE_GRASS);
								}
								catch
								{
									grassSpread = 0;
									SpreadGrass(num111, num110, (int)EntityID.TileID.MUD, (int)EntityID.TileID.JUNGLE_GRASS, repeat: false);
								}
							}
						}
					}
					numIslandHouses = 0;
					houseCount = 0;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[12]);
					num55 = (int)(Main.MaxTilesX * 0.0008);
					for (int num112 = 0; num112 < num55; num112++)
					{
						int num113 = 0;
						bool flag5 = false;
						int num114 = genRand.Next((int)(Main.MaxTilesX * 0.1), (int)(Main.MaxTilesX * 0.9));
						bool flag6;
						do
						{
							flag6 = true;
							while (num114 > (Main.MaxTilesX >> 1) - 80 && num114 < (Main.MaxTilesX >> 1) + 80)
							{
								num114 = genRand.Next((int)(Main.MaxTilesX * 0.1), (int)(Main.MaxTilesX * 0.9));
							}
							for (int num115 = 0; num115 < numIslandHouses; num115++)
							{
								if (num114 > fih[num115].X - 80 && num114 < fih[num115].X + 80)
								{
									num113++;
									flag6 = false;
									break;
								}
							}
							if (num113 >= 200)
							{
								flag5 = true;
								break;
							}
						}
						while (!flag6);
						if (flag5)
						{
							continue;
						}
						for (int num116 = 200; num116 < Main.WorldSurface; num116++)
						{
							if (Main.TileSet[num114, num116].IsActive != 0)
							{
								int num117 = num114;
								int num118 = genRand.Next(90, num116 - 100);
								while (num118 > num5 - 50f)
								{
									num118--;
								}
								FloatingIsland(num117, num118);
								fih[numIslandHouses].X = (short)num117;
								fih[numIslandHouses].Y = (short)num118;
								numIslandHouses++;
								break;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[13]);
					for (int num119 = Main.MaxTilesX / 500 - 1; num119 >= 0; num119--)
					{
						int i3 = genRand.Next((int)(Main.MaxTilesX * 0.3f), (int)(Main.MaxTilesX * 0.7f));
						int j3 = genRand.Next(Main.RockLayer, Main.MaxTilesY - 249);
						ShroomPatch(i3, j3);
					}
					for (int num120 = 0; num120 < Main.MaxTilesX; num120++)
					{
						for (int num121 = Main.WorldSurface; num121 < Main.MaxTilesY; num121++)
						{
							if (Main.TileSet[num120, num121].IsActive != 0)
							{
								grassSpread = 0;
								SpreadGrass(num120, num121, (int)EntityID.TileID.MUD, (int)EntityID.TileID.MUSHROOM_GRASS, repeat: false);
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[14]);
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.001f);
					for (int num122 = 0; num122 < num55; num122++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(2, 6), genRand.Next(2, 40), (byte)EntityID.TileID.MUD);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[15]);
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0001f);
					for (int num123 = 0; num123 < num55; num123++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num8, Main.MaxTilesY), genRand.Next(5, 12), genRand.Next(15, 50), (byte)EntityID.TileID.SILT);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0005f);
					for (int num124 = 0; num124 < num55; num124++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num8, Main.MaxTilesY), genRand.Next(2, 5), genRand.Next(2, 5), (byte)EntityID.TileID.SILT);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[16]);
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 6E-05f);
					for (int num125 = 0; num125 < num55; num125++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5, (int)num6), genRand.Next(3, 6), genRand.Next(2, 6), (byte)EntityID.TileID.COPPER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 8E-05f);
					for (int num126 = 0; num126 < num55; num126++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, (int)num8), genRand.Next(3, 7), genRand.Next(3, 7), (byte)EntityID.TileID.COPPER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0002f);
					for (int num127 = 0; num127 < num55; num127++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(4, 9), genRand.Next(4, 8), (byte)EntityID.TileID.COPPER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 3E-05f);
					for (int num128 = 0; num128 < num55; num128++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5, (int)num6), genRand.Next(3, 7), genRand.Next(2, 5), (byte)EntityID.TileID.IRON_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 8E-05f);
					for (int num129 = 0; num129 < num55; num129++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, (int)num8), genRand.Next(3, 6), genRand.Next(3, 6), (byte)EntityID.TileID.IRON_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0002f);
					for (int num130 = 0; num130 < num55; num130++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(4, 9), genRand.Next(4, 8), (byte)EntityID.TileID.IRON_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 2.6E-05f);
					for (int num131 = 0; num131 < num55; num131++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num6, (int)num8), genRand.Next(3, 6), genRand.Next(3, 6), (byte)EntityID.TileID.SILVER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.00015f);
					for (int num132 = 0; num132 < num55; num132++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(4, 9), genRand.Next(4, 8), (byte)EntityID.TileID.SILVER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.00017f);
					for (int num133 = 0; num133 < num55; num133++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5), genRand.Next(4, 9), genRand.Next(4, 8), (byte)EntityID.TileID.SILVER_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.00012f);
					for (int num134 = 0; num134 < num55; num134++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(4, 8), genRand.Next(4, 8), (byte)EntityID.TileID.GOLD_ORE);
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num5 - 20), genRand.Next(4, 8), genRand.Next(4, 8), (byte)EntityID.TileID.GOLD_ORE);
					}
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 2E-05f);
					for (int num135 = 0; num135 < num55; num135++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next((int)num7, Main.MaxTilesY), genRand.Next(2, 4), genRand.Next(3, 6), (byte)EntityID.TileID.DEMONITE_ORE);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[17]);
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0006f);
					for (int num136 = 0; num136 < num55; num136++)
					{
						int num137 = genRand.Next(20, Main.MaxTilesX - 20);
						int num138 = genRand.Next((int)num5, Main.MaxTilesY - 20);
						if (num136 < numMCaves)
						{
							num137 = mCave[num136].X;
							num138 = mCave[num136].Y;
						}
						if (Main.TileSet[num137, num138].IsActive == 0 && (num138 > Main.WorldSurface || Main.TileSet[num137, num138].WallType > (byte)EntityID.WallID.NONE))
						{
							while (Main.TileSet[num137, num138].IsActive == 0 && num138 > (int)num5)
							{
								num138--;
							}
							num138++;
							int num139 = 1;
							if (genRand.Next(2) == 0)
							{
								num139 = -1;
							}
							for (; Main.TileSet[num137, num138].IsActive == 0 && num137 > 10 && num137 < Main.MaxTilesX - 10; num137 += num139)
							{
							}
							num137 -= num139;
							if (num138 > Main.WorldSurface || Main.TileSet[num137, num138].WallType > 0)
							{
								TileRunner(num137, num138, genRand.Next(4, 11), genRand.Next(2, 4), (byte)EntityID.TileID.COBWEB, addTile: true, new Vector2(num139, -1f), noYChange: false, overRide: false);
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[18]);
					int num140 = Main.MaxTilesY - genRand.Next(150, 190);
					for (int num141 = 0; num141 < Main.MaxTilesX; num141++)
					{
						UI.MainUI.Progress = num141 * 0.2f / Main.MaxTilesX;
						num140 += genRand.Next(-3, 4);
						if (num140 < Main.MaxTilesY - 190)
						{
							num140 = Main.MaxTilesY - 190;
						}
						else if (num140 > Main.MaxTilesY - 160)
						{
							num140 = Main.MaxTilesY - 160;
						}
						int num142 = num140 - 20 - genRand.Next(3);
						Tile* ptr3 = ptr + (num141 * (Main.LargeWorldH) + num142);
						do
						{
							if (num142 >= num140)
							{
								ptr3->IsActive = 0;
								ptr3->Lava = 0;
								ptr3->Liquid = 0;
							}
							else
							{
								ptr3->Type = (byte)EntityID.TileID.ASH;
							}
							ptr3++;
						}
						while (++num142 < Main.MaxTilesY);
					}
					int num143 = Main.MaxTilesY - genRand.Next(40, 70);
					for (int num144 = 10; num144 < Main.MaxTilesX - 10; num144++)
					{
						num143 += genRand.Next(-10, 11);
						if (num143 > Main.MaxTilesY - 60)
						{
							num143 = Main.MaxTilesY - 60;
						}
						else if (num143 < Main.MaxTilesY - 100)
						{
							num143 = Main.MaxTilesY - 120;
						}
						int num145 = num143;
						Tile* ptr4 = ptr + (num144 * (Main.LargeWorldH) + num145);
						do
						{
							if (ptr4->IsActive == 0)
							{
								ptr4->Lava = 32;
								ptr4->Liquid = byte.MaxValue;
							}
						}
						while (++num145 < Main.MaxTilesY - 10);
					}
					for (int num146 = 0; num146 < Main.MaxTilesX; num146++)
					{
						if (genRand.Next(50) == 0)
						{
							int num147 = Main.MaxTilesY - 65;
							while (Main.TileSet[num146, num147].IsActive == 0 && num147 > Main.MaxTilesY - 135)
							{
								num147--;
							}
							TileRunner(genRand.Next(Main.MaxTilesX), num147 + genRand.Next(20, 50), genRand.Next(15, 20), 1000, (byte)EntityID.TileID.ASH, addTile: true, new Vector2(0f, genRand.Next(1, 3)), noYChange: true);
							UI.MainUI.Progress = 0.2f + num146 * 0.05f / Main.MaxTilesX;
						}
					}
					Liquid.QuickWater(0.25, 3, -1, 0.25);
					for (int num148 = 0; num148 < Main.MaxTilesX; num148++)
					{
						if (genRand.Next(13) == 0)
						{
							int num149 = Main.MaxTilesY - 65;
							while ((Main.TileSet[num148, num149].Liquid > 0 || Main.TileSet[num148, num149].IsActive != 0) && num149 > Main.MaxTilesY - 140)
							{
								num149--;
							}
							TileRunner(num148, num149 - genRand.Next(2, 5), genRand.Next(5, 30), 1000, (byte)EntityID.TileID.ASH, addTile: true, new Vector2(0f, genRand.Next(1, 3)), noYChange: true);
							int num150 = genRand.Next(1, 3);
							if (genRand.Next(3) == 0)
							{
								num150 >>= 1;
							}
							if (genRand.Next(2) == 0)
							{
								TileRunner(num148, num149 - genRand.Next(2, 5), genRand.Next(5, 15) * num150, genRand.Next(10, 15) * num150, (byte)EntityID.TileID.ASH, addTile: true, new Vector2(1f, 0.3f));
							}
							if (genRand.Next(2) == 0)
							{
								num150 = genRand.Next(1, 3);
								TileRunner(num148, num149 - genRand.Next(2, 5), genRand.Next(5, 15) * num150, genRand.Next(10, 15) * num150, (byte)EntityID.TileID.ASH, addTile: true, new Vector2(-1f, 0.3f));
							}
							TileRunner(num148 + genRand.Next(-10, 10), num149 + genRand.Next(-10, 10), genRand.Next(5, 15), genRand.Next(5, 10), (int)EntityID.TileID.LIQUID, addTile: false, new Vector2(genRand.Next(-1, 3), genRand.Next(-1, 3)));
							if (genRand.Next(3) == 0)
							{
								TileRunner(num148 + genRand.Next(-10, 10), num149 + genRand.Next(-10, 10), genRand.Next(10, 30), genRand.Next(10, 20), (int)EntityID.TileID.LIQUID, addTile: false, new Vector2(genRand.Next(-1, 3), genRand.Next(-1, 3)));
							}
							if (genRand.Next(5) == 0)
							{
								TileRunner(num148 + genRand.Next(-15, 15), num149 + genRand.Next(-15, 10), genRand.Next(15, 30), genRand.Next(5, 20), (int)EntityID.TileID.LIQUID, addTile: false, new Vector2(genRand.Next(-1, 3), genRand.Next(-1, 3)));
							}
							UI.MainUI.Progress = 0.5f + num148 * 0.4f / Main.MaxTilesX;
						}
					}
					UI.MainUI.Progress = 0.9f;
					for (int num151 = 0; num151 < Main.MaxTilesX; num151++)
					{
						TileRunner(genRand.Next(20, Main.MaxTilesX - 20), genRand.Next(Main.MaxTilesY - 180, Main.MaxTilesY - 10), genRand.Next(2, 7), genRand.Next(2, 7), (int)EntityID.TileID.LIQUID);
					}
					for (int num152 = 0; num152 < Main.MaxTilesX; num152++)
					{
						if (Main.TileSet[num152, Main.MaxTilesY - 145].IsActive == 0)
						{
							Main.TileSet[num152, Main.MaxTilesY - 145].Liquid = byte.MaxValue;
							Main.TileSet[num152, Main.MaxTilesY - 145].Lava = 32;
						}
						if (Main.TileSet[num152, Main.MaxTilesY - 144].IsActive == 0)
						{
							Main.TileSet[num152, Main.MaxTilesY - 144].Liquid = byte.MaxValue;
							Main.TileSet[num152, Main.MaxTilesY - 144].Lava = 32;
						}
					}
					UI.MainUI.Progress = 0.95f;
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0008f);
					for (int num153 = 0; num153 < num55; num153++)
					{
						TileRunner(genRand.Next(Main.MaxTilesX), genRand.Next(Main.MaxTilesY - 140, Main.MaxTilesY), genRand.Next(2, 7), genRand.Next(3, 7), (byte)EntityID.TileID.HELLSTONE);
					}
					UI.MainUI.Progress = 0.98f;
					AddHellHouses();
					UI.MainUI.NextProgressStep(Lang.WorldGenText[19]);
					num55 = genRand.Next(2, (int)(Main.MaxTilesX * 0.005f));
					for (int num154 = 0; num154 < num55; num154++)
					{
						UI.MainUI.Progress = num154 / (float)num55;
						int num155 = genRand.Next(300, Main.MaxTilesX - 300);
						while (num155 > (Main.MaxTilesX >> 1) - 50 && num155 < (Main.MaxTilesX >> 1) + 50)
						{
							num155 = genRand.Next(300, Main.MaxTilesX - 300);
						}
						int num156;
						for (num156 = (int)num5 - 20; Main.TileSet[num155, num156].IsActive == 0; num156++)
						{
						}
						Lakinater(num155, num156);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[58]);
					int num157 = 0;
					if (num9 == -1)
					{
						num157 = genRand.Next((int)(Main.MaxTilesX * 0.05f), (int)(Main.MaxTilesX * 0.2f));
						num9 = -1;
					}
					else
					{
						num157 = genRand.Next((int)(Main.MaxTilesX * 0.8f), (int)(Main.MaxTilesX * 0.95f));
						num9 = 1;
					}
					int y = (Main.RockLayer + Main.MaxTilesY >> 1) + genRand.Next(-200, 200);
					MakeDungeon(num157, y);
					UI.MainUI.NextProgressStep(Lang.WorldGenText[20]);
					num55 = (int)(Main.MaxTilesX * 0.00045f);
					for (int num158 = 0; num158 < num55; num158++)
					{
						UI.MainUI.Progress = num158 / (float)num55;
						bool flag7 = false;
						int num159 = 0;
						int num160 = 0;
						int num161 = 0;
						do
						{
							int num162 = 0;
							int num163 = Main.MaxTilesX >> 1;
							int num164 = 200;
							num159 = genRand.Next(320, Main.MaxTilesX - 320);
							num160 = num159 - genRand.Next(200) - 100;
							num161 = num159 + genRand.Next(200) + 100;
							if (num160 < 285)
							{
								num160 = 285;
							}
							if (num161 > Main.MaxTilesX - 285)
							{
								num161 = Main.MaxTilesX - 285;
							}
							if (num159 > num163 - num164 && num159 < num163 + num164)
							{
								flag7 = false;
							}
							else if (num160 > num163 - num164 && num160 < num163 + num164)
							{
								flag7 = false;
							}
							else if (num161 > num163 - num164 && num161 < num163 + num164)
							{
								flag7 = false;
							}
							else
							{
								flag7 = true;
								int num165 = num160;
								while (flag7 && num165 < num161)
								{
									for (int num166 = 0; num166 < Main.WorldSurface; num166 += 5)
									{
										if (Main.TileSet[num165, num166].IsActive != 0 && Main.TileDungeon[Main.TileSet[num165, num166].Type])
										{
											flag7 = false;
											break;
										}
									}
									num165++;
								}
							}
							if (num162 < 200 && JungleX > num160 && JungleX < num161)
							{
								num162++;
								flag7 = false;
							}
						}
						while (!flag7);
						int num167 = 0;
						for (int num168 = num160; num168 < num161; num168++)
						{
							if (num167 > 0)
							{
								num167--;
							}
							if (num168 == num159 || num167 == 0)
							{
								for (int num169 = (int)num5; num169 < Main.WorldSurface - 1; num169++)
								{
									if (Main.TileSet[num168, num169].IsActive != 0 || Main.TileSet[num168, num169].WallType > (byte)EntityID.WallID.NONE)
									{
										if (num168 == num159)
										{
											num167 = 20;
											ChasmRunner(num168, num169, genRand.Next(150) + 150, makeOrb: true);
										}
										else if (genRand.Next(35) == 0 && num167 == 0)
										{
											num167 = 30;
											ChasmRunner(num168, num169, genRand.Next(50) + 50, makeOrb: true);
										}
										break;
									}
								}
							}
							for (int num170 = (int)num5; num170 < Main.WorldSurface - 1; num170++)
							{
								if (Main.TileSet[num168, num170].IsActive == 0)
								{
									continue;
								}
								int num171 = num170 + genRand.Next(10, 14);
								for (int num172 = num170; num172 < num171; num172++)
								{
									if ((Main.TileSet[num168, num172].Type == (byte)EntityID.TileID.MUD || Main.TileSet[num168, num172].Type == (byte)EntityID.TileID.JUNGLE_GRASS) && num168 >= num160 + genRand.Next(5) && num168 < num161 - genRand.Next(5))
									{
										Main.TileSet[num168, num172].Type = (byte)EntityID.TileID.DIRT;
									}
								}
								break;
							}
						}
						double num173 = Main.WorldSurface + 40;
						for (int num174 = num160; num174 < num161; num174++)
						{
							num173 += genRand.Next(-2, 3);
							if (num173 < Main.WorldSurface + 30)
							{
								num173 = Main.WorldSurface + 30;
							}
							if (num173 > Main.WorldSurface + 50)
							{
								num173 = Main.WorldSurface + 50;
							}
							num54 = num174;
							bool flag8 = false;
							for (int num175 = (int)num5; num175 < num173; num175++)
							{
								if (Main.TileSet[num54, num175].IsActive != 0)
								{
									if (Main.TileSet[num54, num175].Type == (byte)EntityID.TileID.SAND && num54 >= num160 + genRand.Next(5) && num54 <= num161 - genRand.Next(5))
									{
										Main.TileSet[num54, num175].Type = (byte)EntityID.TileID.DIRT;
									}
									if (Main.TileSet[num54, num175].Type == (byte)EntityID.TileID.DIRT && num175 < Main.WorldSurface - 1 && !flag8)
									{
										grassSpread = 0;
										SpreadGrass(num54, num175, (int)EntityID.TileID.DIRT, (int)EntityID.TileID.CORRUPT_GRASS);
									}
									flag8 = true;
									if (Main.TileSet[num54, num175].Type == (byte)EntityID.TileID.STONE && num54 >= num160 + genRand.Next(5) && num54 <= num161 - genRand.Next(5))
									{
										Main.TileSet[num54, num175].Type = (byte)EntityID.TileID.EBONSTONE;
									}
									if (Main.TileSet[num54, num175].Type == (byte)EntityID.TileID.GRASS)
									{
										Main.TileSet[num54, num175].Type = (byte)EntityID.TileID.CORRUPT_GRASS;
									}
								}
							}
						}
						for (int num176 = num160; num176 < num161; num176++)
						{
							for (int num177 = 0; num177 < Main.MaxTilesY - 50; num177++)
							{
								if (Main.TileSet[num176, num177].IsActive == 0 || Main.TileSet[num176, num177].Type != (byte)EntityID.TileID.SHADOW_ORB)
								{
									continue;
								}
								int num178 = num176 - 13;
								int num179 = num176 + 13;
								int num180 = num177 - 13;
								int num181 = num177 + 13;
								for (int num182 = num178; num182 < num179; num182++)
								{
									if (num182 <= 10 || num182 >= Main.MaxTilesX - 10)
									{
										continue;
									}
									for (int num183 = num180; num183 < num181; num183++)
									{
										if (genRand.Next(3) != 0 && Math.Abs(num182 - num176) + Math.Abs(num183 - num177) < 9 + genRand.Next(11) && Main.TileSet[num182, num183].Type != (byte)EntityID.TileID.SHADOW_ORB)
										{
											Main.TileSet[num182, num183].IsActive = 1;
											Main.TileSet[num182, num183].Type = (byte)EntityID.TileID.EBONSTONE;
											if (Math.Abs(num182 - num176) <= 1 && Math.Abs(num183 - num177) <= 1)
											{
												Main.TileSet[num182, num183].IsActive = 0;
											}
										}
										if (Main.TileSet[num182, num183].Type != (byte)EntityID.TileID.SHADOW_ORB && Math.Abs(num182 - num176) <= 2 + genRand.Next(3) && Math.Abs(num183 - num177) <= 2 + genRand.Next(3))
										{
											Main.TileSet[num182, num183].IsActive = 0;
										}
									}
								}
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[21]);
					for (int num184 = 0; num184 < numMCaves; num184++)
					{
						int x = mCave[num184].X;
						int y2 = mCave[num184].Y;
						CaveOpenater(x, y2);
						Cavinator(x, y2, genRand.Next(40, 50));
					}
					int num185 = 0;
					int num186 = 0;
					int num187 = 20;
					int num188 = Main.MaxTilesX - 20;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[22]);
					for (int num189 = 0; num189 < 2; num189++)
					{
						int num190 = 0;
						int num191 = 0;
						if (num189 == 0)
						{
							num190 = 0;
							num191 = genRand.Next(125, 200) + 50;
							if (num9 == 1)
							{
								num191 = 275;
							}
							int num192 = 0;
							float num193 = 1f;
							int num194;
							for (num194 = 0; Main.TileSet[num191 - 1, num194].IsActive == 0; num194++)
							{
							}
							num185 = num194;
							num194 += genRand.Next(1, 5);
							for (int num195 = num191 - 1; num195 >= num190; num195--)
							{
								num192++;
								if (num192 < 3)
								{
									num193 += genRand.Next(10, 20) * 0.2f;
								}
								else if (num192 < 6)
								{
									num193 += genRand.Next(10, 20) * 0.15f;
								}
								else if (num192 < 9)
								{
									num193 += genRand.Next(10, 20) * 0.1f;
								}
								else if (num192 < 15)
								{
									num193 += genRand.Next(10, 20) * 0.07f;
								}
								else if (num192 < 50)
								{
									num193 += genRand.Next(10, 20) * 0.05f;
								}
								else if (num192 < 75)
								{
									num193 += genRand.Next(10, 20) * 0.04f;
								}
								else if (num192 < 100)
								{
									num193 += genRand.Next(10, 20) * 0.03f;
								}
								else if (num192 < 125)
								{
									num193 += genRand.Next(10, 20) * 0.02f;
								}
								else if (num192 < 150)
								{
									num193 += genRand.Next(10, 20) * 0.01f;
								}
								else if (num192 < 175)
								{
									num193 += genRand.Next(10, 20) * 0.005f;
								}
								else if (num192 < 200)
								{
									num193 += genRand.Next(10, 20) * 0.001f;
								}
								else if (num192 < 230)
								{
									num193 += genRand.Next(10, 20) * 0.01f;
								}
								else if (num192 < 235)
								{
									num193 += genRand.Next(10, 20) * 0.05f;
								}
								else if (num192 < 240)
								{
									num193 += genRand.Next(10, 20) * 0.1f;
								}
								else if (num192 < 245)
								{
									num193 += genRand.Next(10, 20) * 0.05f;
								}
								else if (num192 < 255)
								{
									num193 += genRand.Next(10, 20) * 0.01f;
								}
								if (num192 == 235)
								{
									num188 = num195;
									num187 = num195;
								}
								int num196 = genRand.Next(15, 20);
								for (int num197 = 0; num197 < num194 + num193 + num196; num197++)
								{
									if (num197 < num194 + num193 * 0.75f - 3f)
									{
										Main.TileSet[num195, num197].IsActive = 0;
										if (num197 > num194)
										{
											Main.TileSet[num195, num197].Liquid = byte.MaxValue;
										}
										else if (num197 == num194)
										{
											Main.TileSet[num195, num197].Liquid = 127;
										}
									}
									else if (num197 > num194)
									{
										Main.TileSet[num195, num197].Type = (byte)EntityID.TileID.SAND;
										Main.TileSet[num195, num197].IsActive = 1;
									}
									Main.TileSet[num195, num197].WallType = (byte)EntityID.WallID.NONE;
								}
							}
							continue;
						}
						num190 = Main.MaxTilesX - genRand.Next(125, 200) - 50;
						num191 = Main.MaxTilesX;
						if (num9 == -1)
						{
							num190 = Main.MaxTilesX - 275;
						}
						float num198 = 1f;
						int num199 = 0;
						int num200;
						for (num200 = 0; Main.TileSet[num190, num200].IsActive == 0; num200++)
						{
						}
						num186 = num200;
						num200 += genRand.Next(1, 5);
						for (int num201 = num190; num201 < num191; num201++)
						{
							num199++;
							if (num199 < 3)
							{
								num198 += genRand.Next(10, 20) * 0.2f;
							}
							else if (num199 < 6)
							{
								num198 += genRand.Next(10, 20) * 0.15f;
							}
							else if (num199 < 9)
							{
								num198 += genRand.Next(10, 20) * 0.1f;
							}
							else if (num199 < 15)
							{
								num198 += genRand.Next(10, 20) * 0.07f;
							}
							else if (num199 < 50)
							{
								num198 += genRand.Next(10, 20) * 0.05f;
							}
							else if (num199 < 75)
							{
								num198 += genRand.Next(10, 20) * 0.04f;
							}
							else if (num199 < 100)
							{
								num198 += genRand.Next(10, 20) * 0.03f;
							}
							else if (num199 < 125)
							{
								num198 += genRand.Next(10, 20) * 0.02f;
							}
							else if (num199 < 150)
							{
								num198 += genRand.Next(10, 20) * 0.01f;
							}
							else if (num199 < 175)
							{
								num198 += genRand.Next(10, 20) * 0.005f;
							}
							else if (num199 < 200)
							{
								num198 += genRand.Next(10, 20) * 0.001f;
							}
							else if (num199 < 230)
							{
								num198 += genRand.Next(10, 20) * 0.01f;
							}
							else if (num199 < 235)
							{
								num198 += genRand.Next(10, 20) * 0.05f;
							}
							else if (num199 < 240)
							{
								num198 += genRand.Next(10, 20) * 0.1f;
							}
							else if (num199 < 245)
							{
								num198 += genRand.Next(10, 20) * 0.05f;
							}
							else if (num199 < 255)
							{
								num198 += genRand.Next(10, 20) * 0.01f;
							}
							if (num199 == 235)
							{
								num188 = num201;
							}
							int num202 = genRand.Next(15, 20);
							for (int num203 = 0; num203 < num200 + num198 + num202; num203++)
							{
								if (num203 < num200 + num198 * 0.75f - 3f && num203 < Main.WorldSurface - 2)
								{
									Main.TileSet[num201, num203].IsActive = 0;
									if (num203 > num200)
									{
										Main.TileSet[num201, num203].Liquid = byte.MaxValue;
									}
									else if (num203 == num200)
									{
										Main.TileSet[num201, num203].Liquid = 127;
									}
								}
								else if (num203 > num200)
								{
									Main.TileSet[num201, num203].Type = (byte)EntityID.TileID.SAND;
									Main.TileSet[num201, num203].IsActive = 1;
								}
								Main.TileSet[num201, num203].WallType = (byte)EntityID.WallID.NONE;
							}
						}
					}
					for (; Main.TileSet[num187, num185].IsActive == 0; num185++)
					{
					}
					num185++;
					for (; Main.TileSet[num188, num186].IsActive == 0; num186++)
					{
					}
					num186++;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[23]);
					for (int num204 = (int)EntityID.TileID.SAPPHIRE; num204 <= (int)EntityID.TileID.DIAMOND; num204++)
					{
						float num205 = 0f;
						switch (num204)
						{
							case (int)EntityID.TileID.AMETHYST:
								num205 = Main.MaxTilesX * 0.5f;
								break;
							case (int)EntityID.TileID.TOPAZ:
								num205 = Main.MaxTilesX * 0.45f;
								break;
							case (int)EntityID.TileID.SAPPHIRE:
								num205 = Main.MaxTilesX * 0.3f;
								break;
							case (int)EntityID.TileID.EMERALD:
								num205 = Main.MaxTilesX * 0.25f;
								break;
							case (int)EntityID.TileID.RUBY:
								num205 = Main.MaxTilesX * 0.1f;
								break;
							case (int)EntityID.TileID.DIAMOND:
								num205 = Main.MaxTilesX * 0.05f;
								break;
						}
						num205 *= 0.2f;
						for (int num206 = 0; num206 < num205; num206++)
						{
							int num207 = genRand.Next(Main.MaxTilesX);
							int num208 = genRand.Next(Main.WorldSurface, Main.MaxTilesY);
							while (Main.TileSet[num207, num208].Type != (byte)EntityID.TileID.STONE)
							{
								num207 = genRand.Next(Main.MaxTilesX);
								num208 = genRand.Next(Main.WorldSurface, Main.MaxTilesY);
							}
							TileRunner(num207, num208, genRand.Next(2, 6), genRand.Next(3, 7), num204);
						}
					}
					for (int num209 = 0; num209 < 2; num209++)
					{
						int num210 = 1;
						int num211 = 5;
						int num212 = Main.MaxTilesX - 5;
						if (num209 == 1)
						{
							num210 = -1;
							num211 = Main.MaxTilesX - 5;
							num212 = 5;
						}
						for (int num213 = num211; num213 != num212; num213 += num210)
						{
							for (int num214 = 10; num214 < Main.MaxTilesY - 10; num214++)
							{
								if (Main.TileSet[num213, num214].IsActive == 0 || Main.TileSet[num213, num214].Type != (byte)EntityID.TileID.SAND || Main.TileSet[num213, num214 + 1].IsActive == 0 || Main.TileSet[num213, num214 + 1].Type != (byte)EntityID.TileID.SAND)
								{
									continue;
								}
								int num215 = num213 + num210;
								int num216 = num214 + 1;
								if (Main.TileSet[num215, num214].IsActive == 0 && Main.TileSet[num215, num214 + 1].IsActive == 0)
								{
									for (; Main.TileSet[num215, num216].IsActive == 0; num216++)
									{
									}
									num216--;
									Main.TileSet[num213, num214].IsActive = 0;
									Main.TileSet[num215, num216].IsActive = 1;
									Main.TileSet[num215, num216].Type = (byte)EntityID.TileID.SAND;
								}
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[24]);
					for (int num217 = 0; num217 < Main.MaxTilesX; num217++)
					{
						UI.MainUI.Progress = num217 / (float)Main.MaxTilesX;
						for (int num218 = Main.MaxTilesY - 5; num218 > 0; num218--)
						{
							if (Main.TileSet[num217, num218].IsActive != 0)
							{
								if (Main.TileSet[num217, num218].Type == (byte)EntityID.TileID.SAND)
								{
									for (int num219 = num218; Main.TileSet[num217, num219 + 1].IsActive == 0 && num219 < Main.MaxTilesY - 5; num219++)
									{
										Main.TileSet[num217, num219 + 1].IsActive = 1;
										Main.TileSet[num217, num219 + 1].Type = (byte)EntityID.TileID.SAND;
									}
								}
								else if (Main.TileSet[num217, num218].Type == (byte)EntityID.TileID.SILT)
								{
									for (int num220 = num218; Main.TileSet[num217, num220 + 1].IsActive == 0 && num220 < Main.MaxTilesY - 5; num220++)
									{
										Main.TileSet[num217, num220 + 1].IsActive = 1;
										Main.TileSet[num217, num220 + 1].Type = (byte)EntityID.TileID.SILT;
										Main.TileSet[num217, num220].IsActive = 0;
									}
								}
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[25]);
					for (int num221 = 3; num221 < Main.MaxTilesX - 3; num221++)
					{
						UI.MainUI.Progress = num221 / (float)(Main.MaxTilesX - 4);
						bool flag9 = true;
						for (int num222 = 0; num222 < Main.WorldSurface; num222++)
						{
							if (flag9)
							{
								if (Main.TileSet[num221, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE)
								{
									Main.TileSet[num221, num222].WallType = (byte)EntityID.WallID.NONE;
								}
								if (Main.TileSet[num221, num222].Type != (byte)EntityID.TileID.SAND)
								{
									if (Main.TileSet[num221 - 1, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE)
									{
										Main.TileSet[num221 - 1, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221 - 2, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE && genRand.Next(2) == 0)
									{
										Main.TileSet[num221 - 2, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221 - 3, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE && genRand.Next(2) == 0)
									{
										Main.TileSet[num221 - 3, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221 + 1, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE)
									{
										Main.TileSet[num221 + 1, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221 + 2, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE && genRand.Next(2) == 0)
									{
										Main.TileSet[num221 + 2, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221 + 3, num222].WallType == (byte)EntityID.WallID.DIRT_UNSAFE && genRand.Next(2) == 0)
									{
										Main.TileSet[num221 + 3, num222].WallType = (byte)EntityID.WallID.NONE;
									}
									if (Main.TileSet[num221, num222].IsActive != 0)
									{
										flag9 = false;
									}
								}
							}
							else if (Main.TileSet[num221, num222].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221, num222 + 1].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221, num222 + 2].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221, num222 + 3].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221, num222 + 4].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221 - 1, num222].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221 + 1, num222].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221 - 2, num222].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221 + 2, num222].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num221, num222].IsActive == 0 && Main.TileSet[num221, num222 + 1].IsActive == 0 && Main.TileSet[num221, num222 + 2].IsActive == 0 && Main.TileSet[num221, num222 + 3].IsActive == 0)
							{
								flag9 = true;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[26]);
					int y3 = (int)num6 + 20;
					num55 = Main.MaxTilesX * Main.MaxTilesY / 50000;
					for (int num223 = 0; num223 < num55; num223++)
					{
						UI.MainUI.Progress = num223 / (float)num55;
						for (int num224 = 0; num224 < 4096; num224++)
						{
							int x2 = genRand.Next(5, Main.MaxTilesX - 5);
							if (Place3x2(x2, y3, (int)EntityID.TileID.DEMON_ALTAR))
							{
								break;
							}
						}
					}
					num55 = (int)num5;
					for (int num225 = 0; num225 < Main.MaxTilesX; num225++)
					{
						int num226 = num55;
						Tile* ptr5 = ptr + (num225 * (Main.LargeWorldH) + num226);
						do
						{
							if (ptr5->IsActive != 0)
							{
								if (ptr5->Type == (byte)EntityID.TileID.JUNGLE_GRASS)
								{
									ptr5[-1].Liquid = byte.MaxValue;
									ptr5[-2].Liquid = byte.MaxValue;
								}
								break;
							}
							ptr5++;
						}
						while (++num226 < Main.WorldSurface - 1);
					}
					for (int num227 = 400; num227 < Main.MaxTilesX - 400; num227++)
					{
						int num228 = num55;
						Tile* ptr6 = ptr + (num227 * (Main.LargeWorldH) + num228);
						do
						{
							if (ptr6->IsActive != 0)
							{
								if (ptr6->Type != (byte)EntityID.TileID.SAND)
								{
									break;
								}
								Tile* ptr7 = ptr6;
								while (num228 > num55)
								{
									num228--;
									ptr7--;
									if (ptr7->Liquid <= 0)
									{
										break;
									}
									ptr7->Liquid = 0;
								}
								break;
							}
							ptr6++;
						}
						while (++num228 < Main.WorldSurface - 1);
					}
					Liquid.QuickWater(1f / 3f);
					WaterCheck();
					int num229 = 0;
					Liquid.QuickSettleOn();
					do
					{
						int num230 = Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer;
						num229++;
						float num231 = 0f;
						while (Liquid.NumLiquids > 0)
						{
							float num232 = (num230 - (Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer)) / (float)num230;
							if (Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer > num230)
							{
								num230 = Liquid.NumLiquids + LiquidBuffer.NumLiquidBuffer;
							}
							if (num232 > num231)
							{
								num231 = num232;
							}
							else
							{
								num232 = num231;
							}
							if (num229 == 1)
							{
								UI.MainUI.Progress = num232 * (1f / 3f) + (1f / 3f);
							}
							int num233 = 10;
							if (num229 > num233)
							{
								num233 = num229;
							}
							Liquid.UpdateLiquid();
						}
						WaterCheck();
					}
					while (num229 < 10);
					Liquid.QuickSettleOff();
					UI.MainUI.NextProgressStep(Lang.WorldGenText[28]);
					num55 = Main.MaxTilesX * Main.MaxTilesY / 50000;
					for (int num234 = 0; num234 < num55; num234++)
					{
						UI.MainUI.Progress = num234 / (float)num55;
						int num235 = 0;
						while (!AddLifeCrystal(genRand.Next(1, Main.MaxTilesX), genRand.Next((int)(num6 + 20f), Main.MaxTilesY)) && ++num235 < 10000)
						{
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[29]);
					float num236 = Main.MaxTilesX / 4200f;
					int num237 = 0;
					num55 = (int)(num236 * 82f);
					for (int num238 = 0; num238 < num55; num238++)
					{
						if (num237 > 41)
						{
							num237 = 0;
						}
						UI.MainUI.Progress = num238 / (float)num55;
						int num239 = 0;
						do
						{
							int num240 = genRand.Next(20, Main.MaxTilesX - 20);
							int num241;
							for (num241 = genRand.Next((int)(num6 + 20f), Main.MaxTilesY - 300); Main.TileSet[num240, num241].IsActive == 0; num241++)
							{
							}
							num241--;
							if (PlaceTile(num240, num241, (int)EntityID.TileID.STATUE, ToMute: true, IsForced: true, -1, num237))
							{
								num237++;
								break;
							}
						}
						while (++num239 < 10000);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[30]);
					num55 = Main.MaxTilesX * Main.MaxTilesY / 62500;
					for (int num242 = 0; num242 < num55; num242++)
					{
						UI.MainUI.Progress = num242 / (float)num55;
						int num243 = 0;
						while (true)
						{
							int num244 = genRand.Next(1, Main.MaxTilesX);
							int num245 = ((num242 <= 3) ? genRand.Next(Main.MaxTilesY - 200, Main.MaxTilesY - 50) : genRand.Next((int)(num6 + 20f), Main.MaxTilesY - 230));
							int wall = Main.TileSet[num244, num245].WallType;
							if (wall >= (byte)EntityID.WallID.BLUE_DUNGEON_UNSAFE && wall <= (byte)EntityID.WallID.PINK_DUNGEON_UNSAFE)
							{
								continue;
							}
							if (AddBuriedChest(num244, num245))
							{
								if (genRand.Next(2) == 0)
								{
									int num246;
									for (num246 = num245; Main.TileSet[num244, num246].Type != (byte)EntityID.TileID.CHEST && num246 < Main.MaxTilesY - 300; num246++)
									{
									}
									if (num245 < Main.MaxTilesY - 300)
									{
										MineHouse(num244, num246);
									}
								}
								break;
							}
							if (++num243 >= 5000)
							{
								break;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[31]);
					num55 = Main.MaxTilesX / 200;
					for (int num247 = 0; num247 < num55; num247++)
					{
						UI.MainUI.Progress = num247 / (float)num55;
						int num248 = 0;
						int num249;
						int num250;
						do
						{
							num249 = genRand.Next(300, Main.MaxTilesX - 300);
							num250 = genRand.Next((int)num5, Main.WorldSurface);
						}
						while ((Main.TileSet[num249, num250].WallType != (byte)EntityID.WallID.DIRT_UNSAFE || Main.TileSet[num249, num250].IsActive != 0 || !AddBuriedChest(num249, num250, 0, notNearOtherChests: true)) && ++num248 < 2000);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[32]);
					int num251 = 0;
					for (int num252 = 0; num252 < numJChests; num252++)
					{
						UI.MainUI.Progress = num252 / (float)numJChests;
						int contain = (int)EntityID.ItemID.FERAL_CLAWS;
						switch (++num251)
						{
							case 2:
								contain = (int)EntityID.ItemID.ANKLET_OF_THE_WIND;
								break;
							case 3:
								contain = (int)EntityID.ItemID.STAFF_OF_REGROWTH;
								break;
							default:
								if (Main.Rand.Next(2) == 0)
								{
									// BUG: Ever wondered why you could never seem to find Honeycomb or the Wolf Fang? It's because they can't spawn naturally. This line below is why, since it is not a line that alters the chest contents directly.
									// The variable below is responsible for determining what each chest in a jungle shrine should contain. There can only be 3 primary items in versions below Console 1.2.
									// Due to there not being a reset, each world only has 1 anklet of the wind and 1 staff of regrowth. The rest will always be feral claws.
									// It looks like they may have aimed to make each world only have one anklet, feral claws, and staff, and then have the remaining shrine chests be a 50/50 between pet item or feral claws.

									// To make the Honeycomb, Wolf Fang, and by extension, the Pet Hoarder achievement naturally obtainable: Change the 'num251' in the below variable to 'contain'.
									num251 = ((Main.Rand.Next(6) != 0) ? (int)EntityID.ItemID.PET_SPAWN_3 : (int)EntityID.ItemID.PET_SPAWN_5);

									// If they wanted the pet items to be a potential secondary item, they need to be added in AddBuriedChest, which occurred in 1.02, alongside making there be a chance for more than 1 staff or anklet.
								}
								break;
							case 1:
								break;
						}
						if (!AddBuriedChest(JChest[num252].X + genRand.Next(2), JChest[num252].Y, contain))
						{
							KillTile(JChest[num252].X, JChest[num252].Y);
							KillTile(JChest[num252].X, JChest[num252].Y + 1);
							KillTile(JChest[num252].X + 1, JChest[num252].Y);
							KillTile(JChest[num252].X + 1, JChest[num252].Y + 1);
							AddBuriedChest(JChest[num252].X, JChest[num252].Y, contain);
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[33]);
					int num253 = 0;
					num55 = (int)(9f * num236);
					for (int num254 = 0; num254 < num55; num254++)
					{
						UI.MainUI.Progress = num254 / (float)num55;
						int contain2 = (int)EntityID.ItemID.FLIPPER;
						if (++num253 == 1)
						{
							contain2 = (int)EntityID.ItemID.BREATHING_REED;
						}
						else if (num253 == 2)
						{
							contain2 = (int)EntityID.ItemID.TRIDENT;
						}
						else
						{
							num253 = 0;
						}
						bool flag10 = false;
						while (!flag10)
						{
							int num255 = genRand.Next(1, Main.MaxTilesX);
							int num256 = genRand.Next(1, Main.MaxTilesY - 200);
							while (Main.TileSet[num255, num256].Liquid < 200 || Main.TileSet[num255, num256].Lava != 0)
							{
								num255 = genRand.Next(1, Main.MaxTilesX);
								num256 = genRand.Next(1, Main.MaxTilesY - 200);
							}
							flag10 = AddBuriedChest(num255, num256, contain2);
						}
					}
					for (int num257 = 0; num257 < numIslandHouses; num257++)
					{
						IslandHouse(fih[num257].X, fih[num257].Y);
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[34]);
					num55 = (int)(Main.MaxTilesX * 0.05f);
					for (int num258 = 0; num258 < num55; num258++)
					{
						UI.MainUI.Progress = num258 / (float)num55;
						for (int num259 = 0; num259 < 1000; num259++)
						{
							int num260 = Main.Rand.Next(200, Main.MaxTilesX - 200);
							int num261 = Main.Rand.Next(Main.WorldSurface, Main.MaxTilesY - 300);
							if (Main.TileSet[num260, num261].WallType == (byte)EntityID.WallID.NONE && placeTrap(num260, num261))
							{
								break;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[35]);
					num55 = (int)(Main.MaxTilesX * Main.MaxTilesY * 0.0008);
					for (int num262 = 0; num262 < num55; num262++)
					{
						float num263 = num262 / (float)num55;
						UI.MainUI.Progress = num263;
						bool flag11 = false;
						int num264 = 0;
						while (!flag11)
						{
							int num265 = genRand.Next((int)num6, Main.MaxTilesY - 10);
							if (num263 > 0.93f)
							{
								num265 = Main.MaxTilesY - 150;
							}
							else if (num263 > 0.75f)
							{
								num265 = (int)num5;
							}
							int num266 = genRand.Next(1, Main.MaxTilesX - 1);
							bool flag12 = false;
							for (int num267 = num265; num267 < Main.MaxTilesY - 1; num267++)
							{
								if (!flag12)
								{
									if (Main.TileSet[num266, num267].IsActive != 0 && Main.TileSolid[Main.TileSet[num266, num267].Type] && Main.TileSet[num266, num267 - 1].Lava == 0)
									{
										flag12 = true;
									}
									continue;
								}
								if (PlacePot(num266, num267))
								{
									flag11 = true;
									break;
								}
								num264++;
								if (num264 >= 10000)
								{
									flag11 = true;
									break;
								}
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[36]);
					num55 = Main.MaxTilesX / 200;
					for (int num268 = 0; num268 < num55; num268++)
					{
						UI.MainUI.Progress = num268 / (float)num55;
						bool flag = false;
						int num269 = 0;
						while (!flag)
						{
							int num270 = genRand.Next(5, Main.MaxTilesX - 5);
#if (VERSION_INITIAL && !IS_PATCHED)
							int num271 = Main.MaxTilesY - 250;
#else
							int num271 = genRand.Next(Main.MaxTilesY - 250, Main.MaxTilesY - 5);
#endif
							do
							{
								if (Main.TileSet[num270, num271].IsActive != 0 || Main.TileSet[num270, num271].WallType == (int)EntityID.WallID.HELLSTONE_BRICK_UNSAFE || Main.TileSet[num270, num271].WallType == (int)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE)
								{
									num271--;
									if (WorldGen.PlaceTile(num270, num271, (int)EntityID.TileID.HELLFORGE) || ++num269 >= 10000)
									{
										flag = true;
									}
									break;
								}
							}
							while (++num271 != Main.MaxTilesY);
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[37]);
					for (int num272 = 0; num272 < Main.MaxTilesX; num272++)
					{
						num54 = num272;
						bool flag13 = true;
						for (int num273 = 0; num273 < Main.WorldSurface - 1; num273++)
						{
							if (Main.TileSet[num54, num273].IsActive != 0)
							{
								if (flag13 && Main.TileSet[num54, num273].Type == (byte)EntityID.TileID.DIRT)
								{
									try
									{
										grassSpread = 0;
										SpreadGrass(num54, num273);
									}
									catch
									{
										grassSpread = 0;
										SpreadGrass(num54, num273, repeat: false);
									}
								}
								if (num273 > num6)
								{
									break;
								}
								flag13 = false;
							}
							else if (Main.TileSet[num54, num273].WallType == (byte)EntityID.WallID.NONE)
							{
								flag13 = true;
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[38]);
					for (int num274 = 5; num274 < Main.MaxTilesX - 5; num274++)
					{
						if (genRand.Next(8) != 0)
						{
							continue;
						}
						Tile* ptr8 = ptr + (num274 * (Main.LargeWorldH) + 5);
						int num275 = 5;
						do
						{
							if (ptr8->Type == (byte)EntityID.TileID.SAND && ptr8->IsActive != 0)
							{
								ptr8--;
								if (ptr8->IsActive == 0 && ptr8->WallType == (byte)EntityID.WallID.NONE)
								{
									if (num274 < 250 || num274 > Main.MaxTilesX - 250)
									{
										ptr8--;
										if (ptr8->Liquid == byte.MaxValue)
										{
											ptr8--;
											if (ptr8->Liquid == byte.MaxValue)
											{
												ptr8--;
												if (ptr8->Liquid == byte.MaxValue)
												{
													PlaceTile(num274, num275 - 1, (int)EntityID.TileID.CORAL, ToMute: true);
												}
												ptr8++;
											}
											ptr8++;
										}
										ptr8++;
									}
									else if (num274 > 400 && num274 < Main.MaxTilesX - 400)
									{
										PlantCactus(num274, num275);
									}
								}
								ptr8++;
							}
							ptr8++;
						}
						while (++num275 < Main.WorldSurface - 1);
					}
					int num276 = 5;
					while (true)
					{
						int num277 = (Main.MaxTilesX >> 1) + genRand.Next(-num276, num276 + 1);
						for (int num278 = 5; num278 <= Main.WorldSurface; num278++)
						{
							if (Main.TileSet[num277, num278].IsActive != 0)
							{
								Main.SpawnTileX = (short)num277;
								Main.SpawnTileY = (short)num278;
								break;
							}
						}
						if (Main.TileSet[Main.SpawnTileX, Main.SpawnTileY - 1].Liquid == 0)
						{
							break;
						}
						num276++;
					}
					int num279 = NPC.NewNPC(Main.SpawnTileX * 16, Main.SpawnTileY * 16, (int)EntityID.NPCID.GUIDE);
					Main.NPCSet[num279].HomeTileX = Main.SpawnTileX;
					Main.NPCSet[num279].HomeTileY = Main.SpawnTileY;
					Main.NPCSet[num279].Direction = 1;
					Main.NPCSet[num279].IsHomeless = true;
					UI.MainUI.NextProgressStep(Lang.WorldGenText[39]);
					for (int num280 = 0; num280 < Main.MaxTilesX * 0.002; num280++)
					{
						int num281 = 0;
						int num282 = 0;
						int num283 = 0;
						_ = Main.MaxTilesX;
						num281 = genRand.Next(Main.MaxTilesX);
						num282 = num281 - genRand.Next(10) - 7;
						num283 = num281 + genRand.Next(10) + 7;
						if (num282 < 0)
						{
							num282 = 0;
						}
						if (num283 > Main.MaxTilesX - 1)
						{
							num283 = Main.MaxTilesX - 1;
						}
						for (int num284 = num282; num284 < num283; num284++)
						{
							for (int num285 = 5; num285 < Main.WorldSurface - 1; num285++)
							{
								if (Main.TileSet[num284, num285].Type == (byte)EntityID.TileID.GRASS && Main.TileSet[num284, num285].IsActive != 0 && Main.TileSet[num284, num285 - 1].IsActive == 0)
								{
									PlaceTile(num284, num285 - 1, (int)EntityID.TileID.SUNFLOWER, ToMute: true);
								}
								if (Main.TileSet[num284, num285].IsActive != 0)
								{
									break;
								}
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[40]);
					num55 = (int)(Main.MaxTilesX * 0.003) - 1;
					for (int num286 = num55; num286 >= 0; num286--)
					{
						int num287 = genRand.Next(50, Main.MaxTilesX - 50);
						int num288 = genRand.Next(25, 50);
						for (int num289 = num287 - num288; num289 < num287 + num288; num289++)
						{
							for (int num290 = 20; num290 < Main.WorldSurface; num290++)
							{
								GrowEpicTree(num289, num290);
							}
						}
					}
					AddTrees();
					UI.MainUI.NextProgressStep(Lang.WorldGenText[41]);
					for (int num291 = (int)(Main.MaxTilesX * 1.7) - 1; num291 >= 0; num291--)
					{
						PlantAlch();
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[42]);
					AddPlants();
					for (int num292 = 0; num292 < Main.MaxTilesX; num292++)
					{
						for (int num293 = 1; num293 < Main.MaxTilesY; num293++)
						{
							if (Main.TileSet[num292, num293].IsActive == 0)
							{
								continue;
							}
							if (num293 >= Main.WorldSurface && Main.TileSet[num292, num293].Type == (byte)EntityID.TileID.MUSHROOM_GRASS && Main.TileSet[num292, num293 - 1].IsActive == 0)
							{
								GrowShroom(num292, num293);
								if (Main.TileSet[num292, num293 - 1].IsActive == 0)
								{
									PlaceTile(num292, num293 - 1, (int)EntityID.TileID.GLOWING_MUSHROOM, ToMute: true);
								}
							}
							if (Main.TileSet[num292, num293].Type == (byte)EntityID.TileID.JUNGLE_GRASS && Main.TileSet[num292, num293 - 1].IsActive == 0)
							{
								PlaceTile(num292, num293 - 1, (int)EntityID.TileID.SHORT_JUNGLE_PLANTS, ToMute: true);
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[43]);
					for (int num294 = 0; num294 < Main.MaxTilesX; num294++)
					{
						int num295 = 0;
						for (int num296 = 0; num296 < Main.WorldSurface; num296++)
						{
							if (num295 > 0 && Main.TileSet[num294, num296].IsActive == 0)
							{
								Main.TileSet[num294, num296].IsActive = 1;
								Main.TileSet[num294, num296].Type = (byte)EntityID.TileID.VINE;
								num295--;
							}
							else
							{
								num295 = 0;
							}
							if (Main.TileSet[num294, num296].IsActive != 0 && Main.TileSet[num294, num296].Type == (byte)EntityID.TileID.GRASS && genRand.Next(5) < 3)
							{
								num295 = genRand.Next(1, 10);
							}
						}
						num295 = 0;
						for (int num297 = 0; num297 < Main.MaxTilesY; num297++)
						{
							if (num295 > 0 && Main.TileSet[num294, num297].IsActive == 0)
							{
								Main.TileSet[num294, num297].IsActive = 1;
								Main.TileSet[num294, num297].Type = (byte)EntityID.TileID.JUNGLE_VINE;
								num295--;
							}
							else
							{
								num295 = 0;
							}
							if (Main.TileSet[num294, num297].IsActive != 0 && Main.TileSet[num294, num297].Type == (byte)EntityID.TileID.JUNGLE_GRASS && genRand.Next(5) < 3)
							{
								num295 = genRand.Next(1, 10);
							}
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[44]);
					for (int num298 = Main.MaxTilesX / 200; num298 > 0; num298--)
					{
						int num299 = genRand.Next(20, Main.MaxTilesX - 20);
						int num300 = genRand.Next(5, 15);
						int num301 = genRand.Next(15, 30);
						for (int num302 = 30; num302 < Main.WorldSurface - 1; num302++)
						{
							if (Main.TileSet[num299, num302].IsActive == 0)
							{
								continue;
							}
							for (int num303 = num299 - num300; num303 < num299 + num300; num303++)
							{
								for (int num304 = num302 - num301; num304 < num302 + num301; num304++)
								{
									if (Main.TileSet[num303, num304].Type == (byte)EntityID.TileID.SHORT_GRASS_PLANTS || Main.TileSet[num303, num304].Type == (byte)EntityID.TileID.SHORT_CORRUPT_PLANTS)
									{
										Main.TileSet[num303, num304].FrameX = (short)(genRand.Next(6, 8) * 18);
									}
								}
							}
							break;
						}
					}
					UI.MainUI.NextProgressStep(Lang.WorldGenText[45]);
					for (int num305 = Main.MaxTilesX / 500; num305 > 0; num305--)
					{
						int num306 = genRand.Next(20, Main.MaxTilesX - 20);
						int num307 = genRand.Next(4, 10);
						int num308 = genRand.Next(15, 30);
						for (int num309 = 30; num309 < Main.WorldSurface - 1; num309++)
						{
							if (Main.TileSet[num306, num309].IsActive == 0)
							{
								continue;
							}
							for (int num310 = num306 - num307; num310 < num306 + num307; num310++)
							{
								for (int num311 = num309 - num308; num311 < num309 + num308; num311++)
								{
									if (Main.TileSet[num310, num311].Type == (byte)EntityID.TileID.SHORT_GRASS_PLANTS || Main.TileSet[num310, num311].Type == (byte)EntityID.TileID.SHORT_CORRUPT_PLANTS)
									{
										Main.TileSet[num310, num311].FrameX = 144;
									}
								}
							}
							break;
						}
					}
				}
			}
			finally
			{
			}
			Gen = false;
		}

		public static void GrowEpicTree(int i, int y)
		{
			int j;
			for (j = y; Main.TileSet[i, j].Type == (byte)EntityID.TileID.SAPLING; j++)
			{
			}

			bool IsGrassType(int TileType) => 
				TileType == (byte)EntityID.TileID.GRASS || 
				TileType == (byte)EntityID.TileID.CORRUPT_GRASS || 
				TileType == (byte)EntityID.TileID.JUNGLE_GRASS || 
				TileType == (byte)EntityID.TileID.HALLOWED_GRASS;

			var TileU = Main.TileSet[i, j - 1];
			var TileL = Main.TileSet[i - 1, j];
			var TileR = Main.TileSet[i + 1, j];

			bool InvalidTile = Main.TileSet[i, j].IsActive == 0 || Main.TileSet[i, j].Type != (byte)EntityID.TileID.GRASS;
			bool InvalidAbove = (TileU.WallType != (byte)EntityID.WallID.NONE) || (TileU.Liquid != 0);
			bool InvalidNeighbours =
				(TileL.IsActive == 0 || !IsGrassType(TileL.Type)) &&
				(TileR.IsActive == 0 || !IsGrassType(TileR.Type));

			if (InvalidTile || InvalidAbove || InvalidNeighbours)
			{
				return;
			}

			int num = 1;
			if (!EmptyTileCheckTree(i - num, i + num, j - 55, j - 1))
			{
				return;
			}
			int num2 = genRand.Next(10);
			int num3 = genRand.Next(20, 30);
			int num4;
			for (int k = j - num3; k < j; k++)
			{
				Main.TileSet[i, k].frameNumber = (byte)genRand.Next(3);
				Main.TileSet[i, k].IsActive = 1;
				Main.TileSet[i, k].Type = (byte)EntityID.TileID.TREE;
				num4 = genRand.Next(3);
				if (k == j - 1 || k == j - num3)
				{
					num2 = 0;
				}
				switch (num2)
				{
					case 1:
						Main.TileSet[i, k].FrameX = 0;
						Main.TileSet[i, k].FrameY = (short)(66 + num4 * 22);
						break;
					case 2:
						Main.TileSet[i, k].FrameX = 22;
						Main.TileSet[i, k].FrameY = (short)(num4 * 22);
						break;
					case 3:
						Main.TileSet[i, k].FrameX = 44;
						Main.TileSet[i, k].FrameY = (short)(66 + num4 * 22);
						break;
					case 4:
						Main.TileSet[i, k].FrameX = 22;
						Main.TileSet[i, k].FrameY = (short)(66 + num4 * 22);
						break;
					case 5:
						Main.TileSet[i, k].FrameX = 88;
						Main.TileSet[i, k].FrameY = (short)(num4 * 22);
						break;
					case 6:
						Main.TileSet[i, k].FrameX = 66;
						Main.TileSet[i, k].FrameY = (short)(66 + num4 * 22);
						break;
					case 7:
						Main.TileSet[i, k].FrameX = 110;
						Main.TileSet[i, k].FrameY = (short)(66 + num4 * 22);
						break;
					default:
						Main.TileSet[i, k].FrameX = 0;
						Main.TileSet[i, k].FrameY = (short)(num4 * 22);
						break;
				}
				bool flag = num2 == 5 || num2 == 7;
				bool flag2 = num2 == 6 || num2 == 7;
				if (flag)
				{
					Main.TileSet[i - 1, k].IsActive = 1;
					Main.TileSet[i - 1, k].Type = (byte)EntityID.TileID.TREE;
					num4 = genRand.Next(3);
					if (genRand.Next(3) < 2)
					{
						Main.TileSet[i - 1, k].FrameX = 44;
						Main.TileSet[i - 1, k].FrameY = (short)(198 + num4 * 22);
					}
					else
					{
						Main.TileSet[i - 1, k].FrameX = 66;
						Main.TileSet[i - 1, k].FrameY = (short)(num4 * 22);
					}
				}
				if (flag2)
				{
					Main.TileSet[i + 1, k].IsActive = 1;
					Main.TileSet[i + 1, k].Type = (byte)EntityID.TileID.TREE;
					num4 = genRand.Next(3);
					if (genRand.Next(3) < 2)
					{
						Main.TileSet[i + 1, k].FrameX = 66;
						Main.TileSet[i + 1, k].FrameY = (short)(198 + num4 * 22);
					}
					else
					{
						Main.TileSet[i + 1, k].FrameX = 88;
						Main.TileSet[i + 1, k].FrameY = (short)(66 + num4 * 22);
					}
				}
				do
				{
					num2 = genRand.Next(10);
				}
				while (((num2 == 5 || num2 == 7) && flag) || ((num2 == 6 || num2 == 7) && flag2));
			}
			int num5 = genRand.Next(3);
			bool flag3 = false;
			bool flag4 = false;
			if (Main.TileSet[i - 1, j].IsActive != 0 && (Main.TileSet[i - 1, j].Type == (byte)EntityID.TileID.GRASS || Main.TileSet[i - 1, j].Type == (byte)EntityID.TileID.CORRUPT_GRASS || Main.TileSet[i - 1, j].Type == (byte)EntityID.TileID.JUNGLE_GRASS || Main.TileSet[i - 1, j].Type == (byte)EntityID.TileID.HALLOWED_GRASS))
			{
				flag3 = true;
			}
			if (Main.TileSet[i + 1, j].IsActive != 0 && (Main.TileSet[i + 1, j].Type == (byte)EntityID.TileID.GRASS || Main.TileSet[i + 1, j].Type == (byte)EntityID.TileID.CORRUPT_GRASS || Main.TileSet[i + 1, j].Type == (byte)EntityID.TileID.JUNGLE_GRASS || Main.TileSet[i + 1, j].Type == (byte)EntityID.TileID.HALLOWED_GRASS))
			{
				flag4 = true;
			}
			if (!flag3)
			{
				if (num5 == 0)
				{
					num5 = 2;
				}
				if (num5 == 1)
				{
					num5 = 3;
				}
			}
			if (!flag4)
			{
				if (num5 == 0)
				{
					num5 = 1;
				}
				if (num5 == 2)
				{
					num5 = 3;
				}
			}
			if (flag4 && !flag3)
			{
				num5 = 1;
			}
			if (flag3 && !flag4)
			{
				num5 = 2;
			}
			if (num5 == 0 || num5 == 1)
			{
				Main.TileSet[i + 1, j - 1].IsActive = 1;
				Main.TileSet[i + 1, j - 1].Type = (byte)EntityID.TileID.TREE;
				num4 = genRand.Next(3);
				Main.TileSet[i + 1, j - 1].FrameX = 22;
				Main.TileSet[i + 1, j - 1].FrameY = (short)(132 + 22 * num4);
			}
			if (num5 == 0 || num5 == 2)
			{
				Main.TileSet[i - 1, j - 1].IsActive = 1;
				Main.TileSet[i - 1, j - 1].Type = (byte)EntityID.TileID.TREE;
				num4 = genRand.Next(3);
				Main.TileSet[i - 1, j - 1].FrameX = 44;
				Main.TileSet[i - 1, j - 1].FrameY = (short)(132 + 22 * num4);
			}
			num4 = genRand.Next(3);
			switch (num5)
			{
				case 0:
					Main.TileSet[i, j - 1].FrameX = 88;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num4);
					break;
				case 1:
					Main.TileSet[i, j - 1].FrameX = 0;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num4);
					break;
				case 2:
					Main.TileSet[i, j - 1].FrameX = 66;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num4);
					break;
			}
			num4 = genRand.Next(3);
			if (genRand.Next(3) < 2)
			{
				
				Main.TileSet[i, j - num3].FrameX = 22;
				Main.TileSet[i, j - num3].FrameY = (short)(198 + 22 * num4);
			}
			else
			{
				Main.TileSet[i, j - num3].FrameX = 0;
				Main.TileSet[i, j - num3].FrameY = (short)(198 + 22 * num4);
			}
			RangeFrame(i - 2, j - num3 - 1, i + 2, j + 1);
			if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				NetMessage.SendTileSquare(i, (int)(j - num3 * 0.5), num3 + 1);
			}
		}

		public unsafe static void GrowTree(int i, int y)
		{
			int j;
			for (j = y; Main.TileSet[i, j].Type == (byte)EntityID.TileID.SAPLING; j++)
			{
			}

			bool IsValidGrass(int TileType) =>
				TileType == (byte)EntityID.TileID.GRASS ||
				TileType == (byte)EntityID.TileID.CORRUPT_GRASS ||
				TileType == (byte)EntityID.TileID.JUNGLE_GRASS ||
				TileType == (byte)EntityID.TileID.HALLOWED_GRASS ||
				TileType == (byte)EntityID.TileID.SNOW;

			var TileL = Main.TileSet[i - 1, j];
			var TileR = Main.TileSet[i + 1, j];
			var TileLU = Main.TileSet[i - 1, j - 1];
			var TileRU = Main.TileSet[i + 1, j - 1];

			bool HasLiquid = (TileLU.Liquid != 0) || (TileRU.Liquid != 0);

			bool InvalidNeighbours =
				(TileL.IsActive == 0 || !IsValidGrass(TileL.Type)) &&
				(TileR.IsActive == 0 || !IsValidGrass(TileR.Type));

			if ((HasLiquid && Main.TileSet[i, j].Type != (byte)EntityID.TileID.JUNGLE_GRASS) ||
				Main.TileSet[i, j].IsActive == 0 ||
				!IsValidGrass(Main.TileSet[i, j].Type) ||
				Main.TileSet[i, j - 1].WallType != (byte)EntityID.WallID.NONE ||
				InvalidNeighbours)
			{
				return;
			}

			int num = 1;
			int num2 = 16;
			if (Main.TileSet[i, j].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
			{
				num2 += 5;
			}
			if (!EmptyTileCheckTree(i - num, i + num, j - num2, j - 1))
			{
				return;
			}
			int num3 = genRand.Next(10);
			int num4 = genRand.Next(5, num2 + 1);
			int num5;
			for (int k = j - num4; k < j; k++)
			{
				Main.TileSet[i, k].frameNumber = (byte)genRand.Next(3);
				Main.TileSet[i, k].IsActive = 1;
				Main.TileSet[i, k].Type = (byte)EntityID.TileID.TREE;
				num5 = genRand.Next(3);
				if (k == j - 1 || k == j - num4)
				{
					num3 = 0;
				}
				switch (num3)
				{
					case 1:
						Main.TileSet[i, k].FrameX = 0;
						Main.TileSet[i, k].FrameY = (short)(66 + num5 * 22);
						break;
					case 2:
						Main.TileSet[i, k].FrameX = 22;
						Main.TileSet[i, k].FrameY = (short)(num5 * 22);
						break;
					case 3:
						Main.TileSet[i, k].FrameX = 44;
						Main.TileSet[i, k].FrameY = (short)(66 + num5 * 22);
						break;
					case 4:
						Main.TileSet[i, k].FrameX = 22;
						Main.TileSet[i, k].FrameY = (short)(66 + num5 * 22);
						break;
					case 5:
						Main.TileSet[i, k].FrameX = 88;
						Main.TileSet[i, k].FrameY = (short)(num5 * 22);
						break;
					case 6:
						Main.TileSet[i, k].FrameX = 66;
						Main.TileSet[i, k].FrameY = (short)(66 + num5 * 22);
						break;
					case 7:
						Main.TileSet[i, k].FrameX = 110;
						Main.TileSet[i, k].FrameY = (short)(66 + num5 * 22);
						break;
					default:
						Main.TileSet[i, k].FrameX = 0;
						Main.TileSet[i, k].FrameY = (short)(num5 * 22);
						break;
				}
				bool flag = num3 == 5 || num3 == 7;
				bool flag2 = num3 == 6 || num3 == 7;
				if (flag)
				{
					Main.TileSet[i - 1, k].IsActive = 1;
					Main.TileSet[i - 1, k].Type = (byte)EntityID.TileID.TREE;
					num5 = genRand.Next(3);
					if (genRand.Next(3) < 2)
					{
						Main.TileSet[i - 1, k].FrameX = 44;
						Main.TileSet[i - 1, k].FrameY = (short)(198 + num5 * 22);
					}
					else
					{
						Main.TileSet[i - 1, k].FrameX = 66;
						Main.TileSet[i - 1, k].FrameY = (short)(num5 * 22);
					}
				}
				if (flag2)
				{
					Main.TileSet[i + 1, k].IsActive = 1;
					Main.TileSet[i + 1, k].Type = (byte)EntityID.TileID.TREE;
					num5 = genRand.Next(3);
					if (genRand.Next(3) < 2)
					{
						Main.TileSet[i + 1, k].FrameX = 66;
						Main.TileSet[i + 1, k].FrameY = (short)(198 + num5 * 22);
					}
					else
					{
						Main.TileSet[i + 1, k].FrameX = 88;
						Main.TileSet[i + 1, k].FrameY = (short)(66 + num5 * 22);
					}
				}
				do
				{
					num3 = genRand.Next(10);
				}
				while (((num3 == 5 || num3 == 7) && flag) || ((num3 == 6 || num3 == 7) && flag2));
			}
			int num6 = genRand.Next(3);
			bool flag3 = false;
			bool flag4 = false;
			fixed (Tile* ptr = &Main.TileSet[i - 1, j])
			{
				if (ptr->IsActive != 0)
				{
					switch ((EntityID.TileID)ptr->Type)
					{
						case EntityID.TileID.GRASS:
						case EntityID.TileID.CORRUPT_GRASS:
						case EntityID.TileID.JUNGLE_GRASS:
						case EntityID.TileID.HALLOWED_GRASS:
						case EntityID.TileID.SNOW:
							flag3 = true;
							break;
					}
				}
			}
			fixed (Tile* ptr2 = &Main.TileSet[i + 1, j])
			{
				if (ptr2->IsActive != 0)
				{
					switch ((EntityID.TileID)ptr2->Type)
					{
						case EntityID.TileID.GRASS:
						case EntityID.TileID.CORRUPT_GRASS:
						case EntityID.TileID.JUNGLE_GRASS:
						case EntityID.TileID.HALLOWED_GRASS:
						case EntityID.TileID.SNOW:
							flag4 = true;
							break;
					}
				}
			}
			if (!flag3)
			{
				if (num6 == 0)
				{
					num6 = 2;
				}
				if (num6 == 1)
				{
					num6 = 3;
				}
			}
			if (!flag4)
			{
				if (num6 == 0)
				{
					num6 = 1;
				}
				if (num6 == 2)
				{
					num6 = 3;
				}
			}
			if (flag4 && !flag3)
			{
				num6 = 1;
			}
			if (flag3 && !flag4)
			{
				num6 = 2;
			}
			if (num6 == 0 || num6 == 1)
			{
				Main.TileSet[i + 1, j - 1].IsActive = 1;
				Main.TileSet[i + 1, j - 1].Type = (byte)EntityID.TileID.TREE;
				num5 = genRand.Next(3);
				Main.TileSet[i + 1, j - 1].FrameX = 22;
				Main.TileSet[i + 1, j - 1].FrameY = (short)(132 + 22 * num5);
			}
			if (num6 == 0 || num6 == 2)
			{
				Main.TileSet[i - 1, j - 1].IsActive = 1;
				Main.TileSet[i - 1, j - 1].Type = (byte)EntityID.TileID.TREE;
				num5 = genRand.Next(3);
				Main.TileSet[i - 1, j - 1].FrameX = 44;
				Main.TileSet[i - 1, j - 1].FrameY = (short)(132 + 22 * num5);
			}
			num5 = genRand.Next(3);
			switch (num6)
			{
				case 0:
					Main.TileSet[i, j - 1].FrameX = 88;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num5);
					break;
				case 1:
					Main.TileSet[i, j - 1].FrameX = 0;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num5);
					break;
				case 2:
					Main.TileSet[i, j - 1].FrameX = 66;
					Main.TileSet[i, j - 1].FrameY = (short)(132 + 22 * num5);
					break;
			}
			num5 = genRand.Next(3);
			if (genRand.Next(4) < 3)
			{
				Main.TileSet[i, j - num4].FrameX = 22;
				Main.TileSet[i, j - num4].FrameY = (short)(198 + 22 * num5);
			}
			else
			{
				Main.TileSet[i, j - num4].FrameX = 0;
				Main.TileSet[i, j - num4].FrameY = (short)(198 + 22 * num5);
			}
			RangeFrame(i - 2, j - num4 - 1, i + 2, j + 1);
			if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				NetMessage.SendTileSquare(i, (int)(j - num4 * 0.5), num4 + 1);
			}
		}

		public static void GrowShroom(int i, int y)
		{
			var TileL = Main.TileSet[i - 1, y];
			var TileR = Main.TileSet[i + 1, y];

			bool HasLava = (Main.TileSet[i - 1, y - 1].Lava != 0) || (Main.TileSet[i + 1, y - 1].Lava != 0);
			bool InvalidTile = (Main.TileSet[i, y].IsActive == 0) || (Main.TileSet[i, y].Type != (byte)EntityID.TileID.MUSHROOM_GRASS);
			bool InvalidNeighbours =
				TileL.IsActive == 0 || TileL.Type != (byte)EntityID.TileID.MUSHROOM_GRASS ||
				TileR.IsActive == 0 || TileR.Type != (byte)EntityID.TileID.MUSHROOM_GRASS;

			if (HasLava || InvalidTile || InvalidNeighbours ||
				Main.TileSet[i, y - 1].WallType != (byte)EntityID.WallID.NONE ||
				!EmptyTileCheckShroom(i - 2, i + 2, y - 13, y - 1))
			{
				return;
			}

			int num = genRand.Next(4, 11);
			int num2;
			for (int j = y - num; j < y; j++)
			{
				Main.TileSet[i, j].frameNumber = (byte)genRand.Next(3);
				Main.TileSet[i, j].IsActive = 1;
				Main.TileSet[i, j].Type = (byte)EntityID.TileID.GIANT_GLOWING_MUSHROOM;
				num2 = genRand.Next(3);
				Main.TileSet[i, j].FrameX = 0;
				Main.TileSet[i, j].FrameY = (short)(0 + 18 * num2);
			}
			num2 = genRand.Next(3);
			Main.TileSet[i, y - num].FrameX = 36;
			Main.TileSet[i, y - num].FrameY = (short)(0 + (18 * num2));
			RangeFrame(i - 2, y - num - 1, i + 2, y + 1);
			if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				NetMessage.SendTileSquare(i, (int)(y - num * 0.5), num + 1);
			}
		}

		public static void AddTrees()
		{
			for (int i = 1; i < Main.MaxTilesX - 1; i++)
			{
				for (int j = 20; j < Main.WorldSurface; j++)
				{
					GrowTree(i, j);
				}
				int num = genRand.Next(12);
				if (num <= 6)
				{
					i++;
					if (num == 0)
					{
						i++;
					}
				}
			}
		}

		public unsafe static bool EmptyTileCheck(int startX, int endX, int startY, int endY)
		{
			if (startX < 0)
			{
				return false;
			}
			if (endX >= Main.MaxTilesX)
			{
				return false;
			}
			if (startY < 0)
			{
				return false;
			}
			if (endY >= Main.MaxTilesY)
			{
				return false;
			}
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num = startY;
					Tile* ptr2 = ptr + (startX * (Main.LargeWorldH) + num);
					do
					{
						if (ptr2->IsActive != 0)
						{
							return false;
						}
						ptr2++;
					}
					while (++num <= endY);
				}
				while (++startX <= endX);
			}
			return true;
		}

		public unsafe static bool EmptyTileCheckTree(int startX, int endX, int startY, int endY)
		{
			if (startX < 0)
			{
				return false;
			}
			if (endX >= Main.MaxTilesX)
			{
				return false;
			}
			if (startY < 0)
			{
				return false;
			}
			if (endY >= Main.MaxTilesY)
			{
				return false;
			}
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num = startY;
					Tile* ptr2 = ptr + (startX * (Main.LargeWorldH) + num);
					do
					{
						if (ptr2->IsActive != 0)
						{
							switch ((EntityID.TileID)ptr2->Type)
							{
								case EntityID.TileID.SHORT_GRASS_PLANTS:
								case EntityID.TileID.SAPLING:
								case EntityID.TileID.SHORT_CORRUPT_PLANTS:
								case EntityID.TileID.CORRUPTION_THORN:
								case EntityID.TileID.SHORT_JUNGLE_PLANTS:
								case EntityID.TileID.JUNGLE_THORN:
								case EntityID.TileID.TALL_GRASS_PLANTS:
								case EntityID.TileID.TALL_JUNGLE_PLANTS:
								case EntityID.TileID.SHORT_HALLOWED_PLANTS:
								case EntityID.TileID.TALL_HALLOWED_PLANTS:
									break;

								default:
									return false;
							}
						}
						ptr2++;
					}
					while (++num <= endY);
				}
				while (++startX <= endX);
			}
			return true;
		}

		public unsafe static bool EmptyTileCheckShroom(int startX, int endX, int startY, int endY)
		{
			if (startX < 0)
			{
				return false;
			}
			if (endX >= Main.MaxTilesX)
			{
				return false;
			}
			if (startY < 0)
			{
				return false;
			}
			if (endY >= Main.MaxTilesY)
			{
				return false;
			}
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num = startY;
					Tile* ptr2 = ptr + (startX * (Main.LargeWorldH) + num);
					do
					{
						if (ptr2->IsActive != 0 && ptr2->Type != (byte)EntityID.TileID.GLOWING_MUSHROOM)
						{
							return false;
						}
						ptr2++;
					}
					while (++num <= endY);
				}
				while (++startX <= endX);
			}
			return true;
		}

		public static void StartHardmodeCallBack()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				4
			});
#endif
			hardLock = true;
			float num = genRand.Next(300, 400) * 0.001f;
			int num2 = (int)(Main.MaxTilesX * num);
			int num3 = (int)(Main.MaxTilesX * (1f - num));
			int num4 = 1;
			if (genRand.Next(2) == 0)
			{
				num4 = num3;
				num3 = num2;
				num2 = num4;
				num4 = -1;
			}
			Vector2i min = default;
			min.X = Main.MaxTilesX;
			min.Y = Main.MaxTilesY;
			Vector2i maxArea = default;
			GERunner(num2, new Vector2(3 * num4, 5f), good: true, ref min, ref maxArea);
			GERunner(num3, new Vector2(-3 * num4, 5f), good: false, ref min, ref maxArea);
			Netplay.ResetSections(ref min, ref maxArea);
			Main.InHardMode = true;
			hardLock = false;
		}

		public static void StartHardmode()
		{
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && !Main.InHardMode)
			{
				Thread thread = new Thread(StartHardmodeCallBack);
				thread.IsBackground = true;
				thread.Start();
				NetMessage.SendText(15, 50, 255, 130, -1);
				UI.SetTriggerStateForAll(Trigger.UnlockedHardMode);
#if !USE_ORIGINAL_CODE
				if (Main.HardmodeAlert)
				{
					Main.PlaySound(10);
					UI.MainUI.CurMenuType = MenuType.PAUSE;
					UI.MainUI.SetMenu(MenuMode.HARDMODE_UPSELL);
				}
#endif
			}
		}

		public unsafe static bool PlaceDoor(int i, int j, int type)
		{
			if (j >= 2 && j < Main.MaxTilesY - 2)
			{
				fixed (Tile* ptr = &Main.TileSet[i, j])
				{
					if (ptr[-2].IsActive != 0 && Main.TileSolid[ptr[-2].Type] && ptr[2].IsActive != 0 && Main.TileSolid[ptr[2].Type])
					{
						ptr[-1].IsActive = 1;
						ptr[-1].Type = (byte)EntityID.TileID.DOOR_CLOSED;
						ptr[-1].FrameY = 0;
						ptr[-1].FrameX = (short)(genRand.Next(3) * 18);
						ptr->IsActive = 1;
						ptr->Type = (byte)EntityID.TileID.DOOR_CLOSED;
						ptr->FrameY = 18;
						ptr->FrameX = (short)(genRand.Next(3) * 18);
						ptr[1].IsActive = 1;
						ptr[1].Type = (byte)EntityID.TileID.DOOR_CLOSED;
						ptr[1].FrameY = 36;
						ptr[1].FrameX = (short)(genRand.Next(3) * 18);
						return true;
					}
				}
			}
			return false;
		}

		public static bool CanCloseDoor(int i, int j)
		{
			int num = i;
			int num2 = j;
			switch (Main.TileSet[i, j].FrameX)
			{
				case 18:
					num--;
					break;
				case 36:
					num++;
					break;
			}
			int frameY = Main.TileSet[i, j].FrameY;
			num2 -= frameY / 18;
			return !Collision.AnyPlayerOrNPC(num, num2, 3);
		}

		public static bool CloseDoor(int i, int j, bool forced = false)
		{
			int num = 0;
			int num2 = i;
			int num3 = j;
			switch (Main.TileSet[i, j].FrameX)
			{
				case 0:
					num = 1;
					break;
				case 18:
					num2--;
					num = 1;
					break;
				case 36:
					num2++;
					num = -1;
					break;
				case 54:
					num = -1;
					break;
			}
			int frameY = Main.TileSet[i, j].FrameY;
			num3 -= frameY / 18;
			if (!forced && Collision.AnyPlayerOrNPC(num2, num3, 3))
			{
				return false;
			}
			int num4 = num2;
			if (num == -1)
			{
				num4--;
			}
			for (int k = num4; k < num4 + 2; k++)
			{
				for (int l = num3; l < num3 + 3; l++)
				{
					if (k == num2)
					{
						Main.TileSet[k, l].Type = (byte)EntityID.TileID.DOOR_CLOSED;
						Main.TileSet[k, l].FrameX = (short)(genRand.Next(3) * 18);
					}
					else
					{
						Main.TileSet[k, l].IsActive = 0;
					}
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				for (int m = 0; m < 3; m++)
				{
					if (numNoWire < MaxNumWire - 1)
					{
						noWire[numNoWire].X = (short)num2;
						noWire[numNoWire].Y = (short)(num3 + m);
						numNoWire++;
					}
				}
			}
			bool flag = tileFrameRecursion;
			tileFrameRecursion = false;
			TileFrame(num2 - 1, num3 - 1);
			TileFrame(num2 - 1, num3);
			TileFrame(num2 - 1, num3 + 1);
			TileFrame(num2 - 1, num3 + 2);
			TileFrame(num2, num3 - 1);
			TileFrame(num2, num3);
			TileFrame(num2, num3 + 1);
			TileFrame(num2, num3 + 2);
			TileFrame(num2 + 1, num3 - 1);
			TileFrame(num2 + 1, num3);
			TileFrame(num2 + 1, num3 + 1);
			TileFrame(num2 + 1, num3 + 2);
			tileFrameRecursion = flag;
			Main.PlaySound(9, i * 16, j * 16);
			return true;
		}

		public unsafe static bool AddLifeCrystal(int i, int j)
		{
			fixed (Tile* ptr = Main.TileSet)
			{
				Tile* ptr2 = ptr + (i * (Main.LargeWorldH) + j);
				while (j < Main.MaxTilesY)
				{
					if (ptr2->IsActive != 0 && Main.TileSolid[ptr2->Type])
					{
						if (ptr2[-2].Lava != 0 || ptr2[-(Main.LargeWorldH + 2)].Lava != 0)
						{
							return false;
						}
						if (!EmptyTileCheck(i - 1, i, j - 2, j - 1))
						{
							return false;
						}
						ptr2[-(Main.LargeWorldH + 2)].IsActive = 1;
						ptr2[-(Main.LargeWorldH + 2)].Type = (byte)EntityID.TileID.LIFE_CRYSTAL;
						ptr2[-(Main.LargeWorldH + 2)].FrameX = 0;
						ptr2[-(Main.LargeWorldH + 2)].FrameY = 0;
						ptr2[-(Main.LargeWorldH + 1)].IsActive = 1;
						ptr2[-(Main.LargeWorldH + 1)].Type = (byte)EntityID.TileID.LIFE_CRYSTAL;
						ptr2[-(Main.LargeWorldH + 1)].FrameX = 0;
						ptr2[-(Main.LargeWorldH + 1)].FrameY = 18;
						ptr2[-2].IsActive = 1;
						ptr2[-2].Type = (byte)EntityID.TileID.LIFE_CRYSTAL;
						ptr2[-2].FrameX = 18;
						ptr2[-2].FrameY = 0;
						ptr2[-1].IsActive = 1;
						ptr2[-1].Type = (byte)EntityID.TileID.LIFE_CRYSTAL;
						ptr2[-1].FrameX = 18;
						ptr2[-1].FrameY = 18;
						return true;
					}
					j++;
					ptr2++;
				}
			}
			return false;
		}

		public static void AddShadowOrb(int x, int y)
		{
			if (x < 10 || x > Main.MaxTilesX - 10 || y < 10 || y > Main.MaxTilesY - 10)
			{
				return;
			}
			for (int i = x - 1; i < x + 1; i++)
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0 && Main.TileSet[i, j].Type == (byte)EntityID.TileID.SHADOW_ORB)
					{
						return;
					}
				}
			}
			Main.TileSet[x - 1, y - 1].IsActive = 1;
			Main.TileSet[x - 1, y - 1].Type = (byte)EntityID.TileID.SHADOW_ORB;
			Main.TileSet[x - 1, y - 1].FrameX = 0;
			Main.TileSet[x - 1, y - 1].FrameY = 0;
			Main.TileSet[x, y - 1].IsActive = 1;
			Main.TileSet[x, y - 1].Type = (byte)EntityID.TileID.SHADOW_ORB;
			Main.TileSet[x, y - 1].FrameX = 18;
			Main.TileSet[x, y - 1].FrameY = 0;
			Main.TileSet[x - 1, y].IsActive = 1;
			Main.TileSet[x - 1, y].Type = (byte)EntityID.TileID.SHADOW_ORB;
			Main.TileSet[x - 1, y].FrameX = 0;
			Main.TileSet[x - 1, y].FrameY = 18;
			Main.TileSet[x, y].IsActive = 1;
			Main.TileSet[x, y].Type = (byte)EntityID.TileID.SHADOW_ORB;
			Main.TileSet[x, y].FrameX = 18;
			Main.TileSet[x, y].FrameY = 18;
		}

		public static void AddHellHouses()
		{
			int num = (int)(Main.MaxTilesX * 0.25);
			for (int i = num; i < Main.MaxTilesX - num; i++)
			{
				int num2 = Main.MaxTilesY - 40;
				while (Main.TileSet[i, num2].IsActive != 0 || Main.TileSet[i, num2].Liquid > 0)
				{
					num2--;
				}
				if (Main.TileSet[i, num2 + 1].IsActive != 0)
				{
					int type;
					int wall;
					if (genRand.Next(10) == 0)
					{
						type = (byte)EntityID.TileID.HELLSTONE_BRICK;
						wall = (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE;
					}
					else
					{
						type = (byte)EntityID.TileID.OBSIDIAN_BRICK;
						wall = (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE;
					}
					HellHouse(i, num2, type, wall);
					i += genRand.Next(15, 80);
				}
			}
			float num3 = Main.MaxTilesX / 4200f;
			for (int j = 0; j < 200f * num3; j++)
			{
				int num4 = 0;
				bool flag = false;
				while (!flag)
				{
					num4++;
					int num5 = genRand.Next((int)(Main.MaxTilesX * 0.2), (int)(Main.MaxTilesX * 0.8));
					int num6 = genRand.Next(Main.MaxTilesY - 300, Main.MaxTilesY - 20);
					if (Main.TileSet[num5, num6].IsActive != 0 && (Main.TileSet[num5, num6].Type == (byte)EntityID.TileID.OBSIDIAN_BRICK || Main.TileSet[num5, num6].Type == (byte)EntityID.TileID.HELLSTONE_BRICK))
					{
						int num7 = 0;
						if (Main.TileSet[num5 - 1, num6].WallType > (byte)EntityID.WallID.NONE)
						{
							num7 = -1;
						}
						else if (Main.TileSet[num5 + 1, num6].WallType > (byte)EntityID.WallID.NONE)
						{
							num7 = 1;
						}
						if (Main.TileSet[num5 + num7, num6].IsActive == 0 && Main.TileSet[num5 + num7, num6 + 1].IsActive == 0)
						{
							bool flag2 = false;
							for (int k = num5 - 8; k < num5 + 8; k++)
							{
								for (int l = num6 - 8; l < num6 + 8; l++)
								{
									if (Main.TileSet[k, l].IsActive != 0 && Main.TileSet[k, l].Type == (byte)EntityID.TileID.TORCH)
									{
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								PlaceTile(num5 + num7, num6, (byte)EntityID.TileID.TORCH, ToMute: true, IsForced: true, -1, 7);
								flag = true;
							}
						}
					}
					if (num4 > 1000)
					{
						flag = true;
					}
				}
			}
		}

		public static void HellHouse(int i, int j, int type, int wall)
		{
			int num = genRand.Next(8, 20);
			int num2 = genRand.Next(1, 3);
			int num3 = genRand.Next(4, 13);
			int num4 = j;
			for (int k = 0; k < num2; k++)
			{
				int num5 = genRand.Next(5, 9);
				HellRoom(i, num4, num, num5, type, wall);
				num4 -= num5;
			}
			num4 = j;
			for (int l = 0; l < num3; l++)
			{
				int num6 = genRand.Next(5, 9);
				num4 += num6;
				HellRoom(i, num4, num, num6, type, wall);
			}
			for (int m = i - (num >> 1); m <= i + (num >> 1); m++)
			{
				for (num4 = j; num4 < Main.MaxTilesY && ((Main.TileSet[m, num4].IsActive != 0 && (Main.TileSet[m, num4].Type == (byte)EntityID.TileID.HELLSTONE_BRICK || Main.TileSet[m, num4].Type == (byte)EntityID.TileID.OBSIDIAN_BRICK)) || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE); num4++)
				{
				}
				int num7 = 6 + genRand.Next(3);
				while (num4 < Main.MaxTilesY && Main.TileSet[m, num4].IsActive == 0)
				{
					num7--;
					Main.TileSet[m, num4].IsActive = 1;
					Main.TileSet[m, num4].Type = (byte)EntityID.TileID.ASH;
					num4++;
					if (num7 <= 0)
					{
						break;
					}
				}
			}
			int num8 = 0;
			int num9 = 0;
			for (num4 = j; num4 < Main.MaxTilesY && ((Main.TileSet[i, num4].IsActive != 0 && (Main.TileSet[i, num4].Type == (byte)EntityID.TileID.HELLSTONE_BRICK || Main.TileSet[i, num4].Type == (byte)EntityID.TileID.OBSIDIAN_BRICK)) || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE); num4++)
			{
			}
			num4--;
			num9 = num4;
			while ((Main.TileSet[i, num4].IsActive != 0 && (Main.TileSet[i, num4].Type == (byte)EntityID.TileID.HELLSTONE_BRICK || Main.TileSet[i, num4].Type == (byte)EntityID.TileID.OBSIDIAN_BRICK)) || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE || Main.TileSet[i, num4].WallType == (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE)
			{
				num4--;
				if (Main.TileSet[i, num4].IsActive == 0 || (Main.TileSet[i, num4].Type != (byte)EntityID.TileID.HELLSTONE_BRICK && Main.TileSet[i, num4].Type != (byte)EntityID.TileID.OBSIDIAN_BRICK))
				{
					continue;
				}
				int num10 = genRand.Next(i - (num >> 1) + 1, i + (num >> 1) - 1);
				int num11 = genRand.Next(i - (num >> 1) + 1, i + (num >> 1) - 1);
				if (num10 > num11)
				{
					int num12 = num10;
					num10 = num11;
					num11 = num12;
				}
				if (num10 == num11)
				{
					if (num10 < i)
					{
						num11++;
					}
					else
					{
						num10--;
					}
				}
				for (int n = num10; n <= num11; n++)
				{
					if (Main.TileSet[n, num4 - 1].WallType == (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE)
					{
						Main.TileSet[n, num4].WallType = (byte)EntityID.WallID.HELLSTONE_BRICK_UNSAFE;
					}
					if (Main.TileSet[n, num4 - 1].WallType == (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE)
					{
						Main.TileSet[n, num4].WallType = (byte)EntityID.WallID.OBSIDIAN_BRICK_UNSAFE;
					}
					Main.TileSet[n, num4].Type = (byte)EntityID.TileID.PLATFORM;
					Main.TileSet[n, num4].IsActive = 1;
				}
				num4--;
			}
			num8 = num4;
			float num13 = (num9 - num8) * num;
			float num14 = num13 * 0.02f;
			for (int num15 = 0; num15 < num14; num15++)
			{
				int num16 = genRand.Next(i - (num >> 1), i + (num >> 1) + 1);
				int num17 = genRand.Next(num8, num9);
				int num18 = genRand.Next(3, 8);
				float num19 = num18 * 0.4f;
				num19 *= num19;
				for (int num20 = num16 - num18; num20 <= num16 + num18; num20++)
				{
					float num21 = num20 - num16;
					num21 *= num21;
					for (int num22 = num17 - num18; num22 <= num17 + num18; num22++)
					{
						float num23 = num22 - num17;
						float num24 = num21 + num23 * num23;
						if (!(num24 < num19))
						{
							continue;
						}
						try
						{
							if (Main.TileSet[num20, num22].Type == (byte)EntityID.TileID.HELLSTONE_BRICK || Main.TileSet[num20, num22].Type == (byte)EntityID.TileID.PLATFORM)
							{
								Main.TileSet[num20, num22].IsActive = 0;
							}
							Main.TileSet[num20, num22].WallType = (byte)EntityID.WallID.NONE;
						}
						catch
						{
						}
					}
				}
			}
		}

		public static void HellRoom(int i, int j, int width, int height, int type, int wall)
		{
			if (j > Main.MaxTilesY - 40)
			{
				return;
			}
			width >>= 1;
			for (int k = i - width; k <= i + width; k++)
			{
				for (int l = j - height; l <= j; l++)
				{
					try
					{
						Main.TileSet[k, l].IsActive = 1;
						Main.TileSet[k, l].Type = (byte)type;
						Main.TileSet[k, l].Liquid = 0;
						Main.TileSet[k, l].Lava = 0;
					}
					catch
					{
					}
				}
			}
			for (int m = i - width + 1; m <= i + width - 1; m++)
			{
				for (int n = j - height + 1; n <= j - 1; n++)
				{
					try
					{
						Main.TileSet[m, n].IsActive = 0;
						Main.TileSet[m, n].WallType = (byte)wall;
						Main.TileSet[m, n].Liquid = 0;
						Main.TileSet[m, n].Lava = 0;
					}
					catch
					{
					}
				}
			}
		}

		public static void MakeDungeon(int x, int y, int tileType = (byte)EntityID.TileID.BLUE_BRICK, int wallType = (byte)EntityID.WallID.BLUE_DUNGEON_UNSAFE)
		{
			int num = genRand.Next(3);
			int num2 = genRand.Next(3);
			switch (num)
			{
				case 1:
					tileType = (byte)EntityID.TileID.GREEN_BRICK;
					break;
				case 2:
					tileType = (byte)EntityID.TileID.PINK_BRICK;
					break;
			}
			switch (num2)
			{
				case 1:
					wallType = (byte)EntityID.WallID.GREEN_DUNGEON_UNSAFE;
					break;
				case 2:
					wallType = (byte)EntityID.WallID.PINK_DUNGEON_UNSAFE;
					break;
			}
			numDDoors = 0;
			numDPlats = 0;
			numDRooms = 0;
			dungeonX = x;
			dungeonY = y;
			dMinX = x;
			dMaxX = x;
			dMinY = y;
			dMaxY = y;
			dxStrength1 = genRand.Next(25, 30);
			dyStrength1 = genRand.Next(20, 25);
			dxStrength2 = genRand.Next(35, 50);
			dyStrength2 = genRand.Next(10, 15);
			int num3 = Main.MaxTilesX / 60;
			num3 += genRand.Next(num3 / 3);
			int num4 = num3;
			int num5 = 5;
			DungeonRoom(dungeonX, dungeonY, tileType, wallType);
			while (num3 > 0)
			{
				if (dungeonX < dMinX)
				{
					dMinX = dungeonX;
				}
				if (dungeonX > dMaxX)
				{
					dMaxX = dungeonX;
				}
				if (dungeonY > dMaxY)
				{
					dMaxY = dungeonY;
				}
				num3--;
				UI.MainUI.Progress = (num4 - num3) * 0.6f / num4;
				if (num5 > 0)
				{
					num5--;
				}
				if ((num5 == 0) & (genRand.Next(3) == 0))
				{
					num5 = 5;
					if (genRand.Next(2) == 0)
					{
						int num6 = dungeonX;
						int num7 = dungeonY;
						DungeonHalls(dungeonX, dungeonY, tileType, wallType);
						if (genRand.Next(2) == 0)
						{
							DungeonHalls(dungeonX, dungeonY, tileType, wallType);
						}
						DungeonRoom(dungeonX, dungeonY, tileType, wallType);
						dungeonX = num6;
						dungeonY = num7;
					}
					else
					{
						DungeonRoom(dungeonX, dungeonY, tileType, wallType);
					}
				}
				else
				{
					DungeonHalls(dungeonX, dungeonY, tileType, wallType);
				}
			}
			DungeonRoom(dungeonX, dungeonY, tileType, wallType);
			int x2 = dRoom[0].X;
			int y2 = dRoom[0].Y;
			for (int i = 1; i < numDRooms; i++)
			{
				if (dRoom[i].Y < y2)
				{
					x2 = dRoom[i].X;
					y2 = dRoom[i].Y;
				}
			}
			dungeonX = x2;
			dungeonY = y2;
			dEnteranceX = x2;
			dSurface = false;
			num5 = 5;
			while (!dSurface)
			{
				if (num5 > 0)
				{
					num5--;
				}
				if (((num5 == 0) & (genRand.Next(5) == 0)) && dungeonY > Main.WorldSurface + 50)
				{
					num5 = 10;
					int num8 = dungeonX;
					int num9 = dungeonY;
					DungeonHalls(dungeonX, dungeonY, tileType, wallType, forceX: true);
					DungeonRoom(dungeonX, dungeonY, tileType, wallType);
					dungeonX = num8;
					dungeonY = num9;
				}
				DungeonStairs(dungeonX, dungeonY, tileType, wallType);
			}
			DungeonEnt(dungeonX, dungeonY, tileType, wallType);
			UI.MainUI.Progress = 0.65f;
			for (int j = 0; j < numDRooms; j++)
			{
				for (int k = dRoom[j].L; k <= dRoom[j].R; k++)
				{
					int num10 = dRoom[j].T - 1;
					if (Main.TileSet[k, num10].IsActive == 0)
					{
						DPlat[numDPlats].X = (short)k;
						DPlat[numDPlats].Y = (short)num10;
						numDPlats++;
						break;
					}
				}
				for (int l = dRoom[j].L; l <= dRoom[j].R; l++)
				{
					int num11 = dRoom[j].B + 1;
					if (Main.TileSet[l, num11].IsActive == 0)
					{
						DPlat[numDPlats].X = (short)l;
						DPlat[numDPlats].Y = (short)num11;
						numDPlats++;
						break;
					}
				}
				for (int m = dRoom[j].T; m <= dRoom[j].B; m++)
				{
					int num12 = dRoom[j].L - 1;
					if (Main.TileSet[num12, m].IsActive == 0)
					{
						dDoor[numDDoors].X = (short)num12;
						dDoor[numDDoors].Y = (short)m;
						dDoor[numDDoors].Pos = -1;
						numDDoors++;
						break;
					}
				}
				for (int n = dRoom[j].T; n <= dRoom[j].B; n++)
				{
					int num13 = dRoom[j].R + 1;
					if (Main.TileSet[num13, n].IsActive == 0)
					{
						dDoor[numDDoors].X = (short)num13;
						dDoor[numDDoors].Y = (short)n;
						dDoor[numDDoors].Pos = 1;
						numDDoors++;
						break;
					}
				}
			}
			UI.MainUI.Progress = 0.7f;
			int num14 = 0;
			int num15 = 1000;
			int num16 = 0;
			while (num16 < Main.MaxTilesX / 100)
			{
				num14++;
				int num17 = genRand.Next(dMinX, dMaxX);
				int num18 = genRand.Next(Main.WorldSurface + 25, dMaxY);
				int num19 = num17;
				if (Main.TileSet[num17, num18].WallType == wallType && Main.TileSet[num17, num18].IsActive == 0)
				{
					int num20 = 1;
					if (genRand.Next(2) == 0)
					{
						num20 = -1;
					}
					for (; Main.TileSet[num17, num18].IsActive == 0; num18 += num20)
					{
					}
					if (Main.TileSet[num17 - 1, num18].IsActive != 0 && Main.TileSet[num17 + 1, num18].IsActive != 0 && Main.TileSet[num17 - 1, num18 - num20].IsActive == 0 && Main.TileSet[num17 + 1, num18 - num20].IsActive == 0)
					{
						num16++;
						int num21 = genRand.Next(5, 13);
						while (Main.TileSet[num17 - 1, num18].IsActive != 0 && Main.TileSet[num17, num18 + num20].IsActive != 0 && Main.TileSet[num17, num18].IsActive != 0 && Main.TileSet[num17, num18 - num20].IsActive == 0 && num21 > 0)
						{
							Main.TileSet[num17, num18].Type = (byte)EntityID.TileID.SPIKE;
							if (Main.TileSet[num17 - 1, num18 - num20].IsActive == 0 && Main.TileSet[num17 + 1, num18 - num20].IsActive == 0)
							{
								Main.TileSet[num17, num18 - num20].Type = (byte)EntityID.TileID.SPIKE;
								Main.TileSet[num17, num18 - num20].IsActive = 1;
							}
							num17--;
							num21--;
						}
						num21 = genRand.Next(5, 13);
						num17 = num19 + 1;
						while (Main.TileSet[num17 + 1, num18].IsActive != 0 && Main.TileSet[num17, num18 + num20].IsActive != 0 && Main.TileSet[num17, num18].IsActive != 0 && Main.TileSet[num17, num18 - num20].IsActive == 0 && num21 > 0)
						{
							Main.TileSet[num17, num18].Type = (byte)EntityID.TileID.SPIKE;
							if (Main.TileSet[num17 - 1, num18 - num20].IsActive == 0 && Main.TileSet[num17 + 1, num18 - num20].IsActive == 0)
							{
								Main.TileSet[num17, num18 - num20].Type = (byte)EntityID.TileID.SPIKE;
								Main.TileSet[num17, num18 - num20].IsActive = 1;
							}
							num17++;
							num21--;
						}
					}
				}
				if (num14 > num15)
				{
					num14 = 0;
					num16++;
				}
			}
			num14 = 0;
			num15 = 1000;
			num16 = 0;
			UI.MainUI.Progress = 0.75f;
			while (num16 < Main.MaxTilesX / 100)
			{
				num14++;
				int num22 = genRand.Next(dMinX, dMaxX);
				int num23 = genRand.Next(Main.WorldSurface + 25, dMaxY);
				int num24 = num23;
				if (Main.TileSet[num22, num23].WallType == wallType && Main.TileSet[num22, num23].IsActive == 0)
				{
					int num25 = 1;
					if (genRand.Next(2) == 0)
					{
						num25 = -1;
					}
					for (; num22 > 5 && num22 < Main.MaxTilesX - 5 && Main.TileSet[num22, num23].IsActive == 0; num22 += num25)
					{
					}
					if (Main.TileSet[num22, num23 - 1].IsActive != 0 && Main.TileSet[num22, num23 + 1].IsActive != 0 && Main.TileSet[num22 - num25, num23 - 1].IsActive == 0 && Main.TileSet[num22 - num25, num23 + 1].IsActive == 0)
					{
						num16++;
						int num26 = genRand.Next(5, 13);
						while (Main.TileSet[num22, num23 - 1].IsActive != 0 && Main.TileSet[num22 + num25, num23].IsActive != 0 && Main.TileSet[num22, num23].IsActive != 0 && Main.TileSet[num22 - num25, num23].IsActive == 0 && num26 > 0)
						{
							Main.TileSet[num22, num23].Type = (byte)EntityID.TileID.SPIKE;
							if (Main.TileSet[num22 - num25, num23 - 1].IsActive == 0 && Main.TileSet[num22 - num25, num23 + 1].IsActive == 0)
							{
								Main.TileSet[num22 - num25, num23].Type = (byte)EntityID.TileID.SPIKE;
								Main.TileSet[num22 - num25, num23].IsActive = 1;
							}
							num23--;
							num26--;
						}
						num26 = genRand.Next(5, 13);
						num23 = num24 + 1;
						while (Main.TileSet[num22, num23 + 1].IsActive != 0 && Main.TileSet[num22 + num25, num23].IsActive != 0 && Main.TileSet[num22, num23].IsActive != 0 && Main.TileSet[num22 - num25, num23].IsActive == 0 && num26 > 0)
						{
							Main.TileSet[num22, num23].Type = (byte)EntityID.TileID.SPIKE;
							if (Main.TileSet[num22 - num25, num23 - 1].IsActive == 0 && Main.TileSet[num22 - num25, num23 + 1].IsActive == 0)
							{
								Main.TileSet[num22 - num25, num23].Type = (byte)EntityID.TileID.SPIKE;
								Main.TileSet[num22 - num25, num23].IsActive = 1;
							}
							num23++;
							num26--;
						}
					}
				}
				if (num14 > num15)
				{
					num14 = 0;
					num16++;
				}
			}
			UI.MainUI.Progress = 0.8f;
			for (int num27 = 0; num27 < numDDoors; num27++)
			{
				int num28 = dDoor[num27].X - 10;
				int num29 = dDoor[num27].X + 10;
				int num30 = 100;
				int num31 = 0;
				int num32 = 0;
				int num33 = 0;
				for (int num34 = num28; num34 < num29; num34++)
				{
					bool flag = true;
					int num35 = dDoor[num27].Y;
					while (Main.TileSet[num34, num35].IsActive == 0)
					{
						num35--;
					}
					if (!Main.TileDungeon[Main.TileSet[num34, num35].Type])
					{
						flag = false;
					}
					num32 = num35;
					for (num35 = dDoor[num27].Y; Main.TileSet[num34, num35].IsActive == 0; num35++)
					{
					}
					if (!Main.TileDungeon[Main.TileSet[num34, num35].Type])
					{
						flag = false;
					}
					num33 = num35;
					if (num33 - num32 < 3)
					{
						continue;
					}
					int num36 = num34 - 20;
					int num37 = num34 + 20;
					int num38 = num33 - 10;
					int num39 = num33 + 10;
					for (int num40 = num36; num40 < num37; num40++)
					{
						for (int num41 = num38; num41 < num39; num41++)
						{
							if (Main.TileSet[num40, num41].IsActive != 0 && Main.TileSet[num40, num41].Type == (byte)EntityID.TileID.DOOR_CLOSED)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						for (int num42 = num33 - 3; num42 < num33; num42++)
						{
							for (int num43 = num34 - 3; num43 <= num34 + 3; num43++)
							{
								if (Main.TileSet[num43, num42].IsActive != 0)
								{
									flag = false;
									break;
								}
							}
						}
					}
					if (flag && num33 - num32 < 20 && ((dDoor[num27].Pos == 0 && num33 - num32 < num30) || (dDoor[num27].Pos == -1 && num34 > num31) || (dDoor[num27].Pos == 1 && (num34 < num31 || num31 == 0))))
					{
						num31 = num34;
						num30 = num33 - num32;
					}
				}
				if (num30 >= 20)
				{
					continue;
				}
				int num44 = num31;
				int num45 = dDoor[num27].Y;
				int num46 = num45;
				for (; Main.TileSet[num44, num45].IsActive == 0; num45++)
				{
					Main.TileSet[num44, num45].IsActive = 0;
				}
				while (Main.TileSet[num44, num46].IsActive == 0)
				{
					num46--;
				}
				num45--;
				num46++;
				for (int num47 = num46; num47 < num45 - 2; num47++)
				{
					Main.TileSet[num44, num47].IsActive = 1;
					Main.TileSet[num44, num47].Type = (byte)tileType;
				}
				PlaceTile(num44, num45, (byte)EntityID.TileID.DOOR_CLOSED, ToMute: true);
				num44--;
				int num48 = num45 - 3;
				while (Main.TileSet[num44, num48].IsActive == 0)
				{
					num48--;
				}
				if (num45 - num48 < num45 - num46 + 5 && Main.TileDungeon[Main.TileSet[num44, num48].Type])
				{
					for (int num49 = num45 - 4 - genRand.Next(3); num49 > num48; num49--)
					{
						Main.TileSet[num44, num49].IsActive = 1;
						Main.TileSet[num44, num49].Type = (byte)tileType;
					}
				}
				num44 += 2;
				num48 = num45 - 3;
				while (Main.TileSet[num44, num48].IsActive == 0)
				{
					num48--;
				}
				if (num45 - num48 < num45 - num46 + 5 && Main.TileDungeon[Main.TileSet[num44, num48].Type])
				{
					for (int num50 = num45 - 4 - genRand.Next(3); num50 > num48; num50--)
					{
						Main.TileSet[num44, num50].IsActive = 1;
						Main.TileSet[num44, num50].Type = (byte)tileType;
					}
				}
				num45++;
				num44--;
				Main.TileSet[num44 - 1, num45].IsActive = 1;
				Main.TileSet[num44 - 1, num45].Type = (byte)tileType;
				Main.TileSet[num44 + 1, num45].IsActive = 1;
				Main.TileSet[num44 + 1, num45].Type = (byte)tileType;
			}
			UI.MainUI.Progress = 0.85f;
			for (int num51 = 0; num51 < numDPlats; num51++)
			{
				int x3 = DPlat[num51].X;
				int y3 = DPlat[num51].Y;
				int num52 = Main.MaxTilesX;
				int num53 = 10;
				for (int num54 = y3 - 5; num54 <= y3 + 5; num54++)
				{
					int num55 = x3;
					int num56 = x3;
					bool flag2 = false;
					if (Main.TileSet[num55, num54].IsActive != 0)
					{
						flag2 = true;
					}
					else
					{
						while (Main.TileSet[num55, num54].IsActive == 0)
						{
							num55--;
							if (!Main.TileDungeon[Main.TileSet[num55, num54].Type])
							{
								flag2 = true;
							}
						}
						while (Main.TileSet[num56, num54].IsActive == 0)
						{
							num56++;
							if (!Main.TileDungeon[Main.TileSet[num56, num54].Type])
							{
								flag2 = true;
							}
						}
					}
					if (flag2 || num56 - num55 > num53)
					{
						continue;
					}
					bool flag3 = true;
					int num57 = x3 - (num53 >> 1) - 2;
					int num58 = x3 + (num53 >> 1) + 2;
					int num59 = num54 - 5;
					int num60 = num54 + 5;
					for (int num61 = num57; num61 <= num58; num61++)
					{
						for (int num62 = num59; num62 <= num60; num62++)
						{
							if (Main.TileSet[num61, num62].IsActive != 0 && Main.TileSet[num61, num62].Type == (byte)EntityID.TileID.PLATFORM)
							{
								flag3 = false;
								break;
							}
						}
					}
					for (int num63 = num54 + 3; num63 >= num54 - 5; num63--)
					{
						if (Main.TileSet[x3, num63].IsActive != 0)
						{
							flag3 = false;
							break;
						}
					}
					if (flag3)
					{
						num52 = num54;
						break;
					}
				}
				if (num52 > y3 - 10 && num52 < y3 + 10)
				{
					int num64 = x3;
					int num65 = num52;
					int num66 = x3 + 1;
					while (Main.TileSet[num64, num65].IsActive == 0)
					{
						Main.TileSet[num64, num65].IsActive = 1;
						Main.TileSet[num64, num65].Type = (byte)EntityID.TileID.PLATFORM;
						num64--;
					}
					for (; Main.TileSet[num66, num65].IsActive == 0; num66++)
					{
						Main.TileSet[num66, num65].IsActive = 1;
						Main.TileSet[num66, num65].Type = (byte)EntityID.TileID.PLATFORM;
					}
				}
			}
			UI.MainUI.Progress = 0.9f;
			num14 = 0;
			num15 = 1000;
			num16 = 0;
			while (num16 < Main.MaxTilesX / 20)
			{
				num14++;
				int num67 = genRand.Next(dMinX, dMaxX);
				int num68 = genRand.Next(dMinY, dMaxY);
				bool flag4 = true;
				if (Main.TileSet[num67, num68].WallType == wallType && Main.TileSet[num67, num68].IsActive == 0)
				{
					int num69 = 1;
					if (genRand.Next(2) == 0)
					{
						num69 = -1;
					}
					while (flag4 && Main.TileSet[num67, num68].IsActive == 0)
					{
						num67 -= num69;
						if (num67 < 5 || num67 > Main.MaxTilesX - 5)
						{
							flag4 = false;
						}
						else if (Main.TileSet[num67, num68].IsActive != 0 && !Main.TileDungeon[Main.TileSet[num67, num68].Type])
						{
							flag4 = false;
						}
					}
					if (flag4 && Main.TileSet[num67, num68].IsActive != 0 && Main.TileDungeon[Main.TileSet[num67, num68].Type] && Main.TileSet[num67, num68 - 1].IsActive != 0 && Main.TileDungeon[Main.TileSet[num67, num68 - 1].Type] && Main.TileSet[num67, num68 + 1].IsActive != 0 && Main.TileDungeon[Main.TileSet[num67, num68 + 1].Type])
					{
						num67 += num69;
						for (int num70 = num67 - 3; num70 <= num67 + 3; num70++)
						{
							for (int num71 = num68 - 3; num71 <= num68 + 3; num71++)
							{
								if (Main.TileSet[num70, num71].IsActive != 0 && Main.TileSet[num70, num71].Type == (byte)EntityID.TileID.PLATFORM)
								{
									flag4 = false;
									break;
								}
							}
						}
						if (flag4 && ((Main.TileSet[num67, num68 - 1].IsActive == 0) & (Main.TileSet[num67, num68 - 2].IsActive == 0) & (Main.TileSet[num67, num68 - 3].IsActive == 0)))
						{
							int num72 = num67;
							int num73 = num67;
							for (; num72 > dMinX && num72 < dMaxX && Main.TileSet[num72, num68].IsActive == 0 && Main.TileSet[num72, num68 - 1].IsActive == 0 && Main.TileSet[num72, num68 + 1].IsActive == 0; num72 += num69)
							{
							}
							num72 = Math.Abs(num67 - num72);
							bool flag5 = false;
							if (genRand.Next(2) == 0)
							{
								flag5 = true;
							}
							if (num72 > 5)
							{
								for (int num74 = genRand.Next(1, 4); num74 > 0; num74--)
								{
									Main.TileSet[num67, num68].IsActive = 1;
									Main.TileSet[num67, num68].Type = (byte)EntityID.TileID.PLATFORM;
									if (flag5)
									{
										PlaceTile(num67, num68 - 1, (byte)EntityID.TileID.BOOK, ToMute: true);
										if (genRand.Next(50) == 0 && Main.TileSet[num67, num68 - 1].Type == (byte)EntityID.TileID.BOOK)
										{
											Main.TileSet[num67, num68 - 1].FrameX = 90;
										}
									}
									num67 += num69;
								}
								num14 = 0;
								num16++;
								if (!flag5 && genRand.Next(2) == 0)
								{
									num67 = num73;
									num68--;
									int num75 = 0;
									if (genRand.Next(4) == 0)
									{
										num75 = 1;
									}
									switch (num75)
									{
										case 0:
											num75 = (byte)EntityID.TileID.BOTTLE;
											break;
										case 1:
											num75 = (byte)EntityID.TileID.WATER_CANDLE;
											break;
									}
									PlaceTile(num67, num68, num75, ToMute: true);
									if (Main.TileSet[num67, num68].Type == (byte)EntityID.TileID.BOTTLE)
									{
										if (genRand.Next(2) == 0)
										{
											Main.TileSet[num67, num68].FrameX = 18;
										}
										else
										{
											Main.TileSet[num67, num68].FrameX = 36;
										}
									}
								}
							}
						}
					}
				}
				if (num14 > num15)
				{
					num14 = 0;
					num16++;
				}
			}
			UI.MainUI.Progress = 0.95f;
			int num76 = 0;
			for (int num77 = 0; num77 < numDRooms; num77++)
			{
				int num78 = 0;
				while (num78 < 1000)
				{
					int num79 = (int)(dRoom[num77].Size * 0.4f);
					int i2 = dRoom[num77].X + genRand.Next(-num79, num79 + 1);
					int num80 = dRoom[num77].Y + genRand.Next(-num79, num79 + 1);
					int num81 = 0;
					num76++;
					int style = 2;
					switch (num76)
					{
						case 1:
							num81 = (int)EntityID.ItemID.SHADOW_KEY;
							break;
						case 2:
							num81 = (int)EntityID.ItemID.MURAMASA;
							break;
						case 3:
							num81 = (int)EntityID.ItemID.COBALT_SHIELD;
							break;
						case 4:
							num81 = (int)EntityID.ItemID.AQUA_SCEPTER;
							break;
						case 5:
							num81 = (int)EntityID.ItemID.BLUE_MOON;
							break;
						case 6:
							num81 = (int)EntityID.ItemID.MAGIC_MISSILE;
							break;
						case 7:
							num81 = (int)EntityID.ItemID.GOLDEN_KEY;
							style = 0;
							break;
						default:
							num81 = (int)EntityID.ItemID.HANDGUN;
							num76 = 0;
							break;
					}
					if (num80 < Main.WorldSurface + 50)
					{
						num81 = (int)EntityID.ItemID.GOLDEN_KEY;
						style = 0;
					}
					if (num81 == 0 && genRand.Next(2) == 0)
					{
						num78 = 1000;
						continue;
					}
					if (AddBuriedChest(i2, num80, num81, notNearOtherChests: false, style))
					{
						num78 += 1000;
					}
					num78++;
				}
			}
			dMinX -= 25;
			dMaxX += 25;
			dMinY -= 25;
			dMaxY += 25;
			if (dMinX < 0)
			{
				dMinX = 0;
			}
			if (dMaxX > Main.MaxTilesX)
			{
				dMaxX = Main.MaxTilesX;
			}
			if (dMinY < 0)
			{
				dMinY = 0;
			}
			if (dMaxY > Main.MaxTilesY)
			{
				dMaxY = Main.MaxTilesY;
			}
			num14 = 0;
			num15 = 1000;
			num16 = 0;
			while (num16 < Main.MaxTilesX / 150)
			{
				num14++;
				int num82 = genRand.Next(dMinX, dMaxX);
				int num83 = genRand.Next(dMinY, dMaxY);
				if (Main.TileSet[num82, num83].WallType == wallType)
				{
					for (int num84 = num83; num84 > dMinY; num84--)
					{
						if (Main.TileSet[num82, num84 - 1].IsActive != 0 && Main.TileSet[num82, num84 - 1].Type == tileType)
						{
							bool flag6 = false;
							for (int num85 = num82 - 15; num85 < num82 + 15; num85++)
							{
								for (int num86 = num84 - 15; num86 < num84 + 15; num86++)
								{
									if (num85 > 0 && num85 < Main.MaxTilesX && num86 > 0 && num86 < Main.MaxTilesY && Main.TileSet[num85, num86].Type == (byte)EntityID.TileID.CHAIN_LANTERN)
									{
										flag6 = true;
										break;
									}
								}
							}
							if (Main.TileSet[num82 - 1, num84].IsActive != 0 || Main.TileSet[num82 + 1, num84].IsActive != 0 || Main.TileSet[num82 - 1, num84 + 1].IsActive != 0 || Main.TileSet[num82 + 1, num84 + 1].IsActive != 0 || Main.TileSet[num82, num84 + 2].IsActive != 0)
							{
								flag6 = true;
							}
							if (flag6 || !Place1x2Top(num82, num84, (byte)EntityID.TileID.CHAIN_LANTERN))
							{
								break;
							}
							num14 = 0;
							num16++;
							Rectangle aabb = default;
							Rectangle aabb2 = default;
							aabb2.X = num82 << 4;
							aabb2.Y = (num84 << 4) + 1;
							aabb.Width = (aabb2.Width = 16);
							aabb.Height = (aabb2.Height = 16);
							for (int num87 = 0; num87 < 1000; num87++)
							{
								int num88 = num82 + genRand.Next(-12, 13);
								int num89 = num84 + genRand.Next(3, 21);
								if (Main.TileSet[num88, num89].IsActive != 0 || Main.TileSet[num88, num89 + 1].IsActive != 0 || Main.TileSet[num88 - 1, num89].Type == (byte)EntityID.TileID.SPIKE || Main.TileSet[num88 + 1, num89].Type == (byte)EntityID.TileID.SPIKE)
								{
									continue;
								}
								aabb.X = num88 << 4;
								aabb.Y = num89 << 4;
								if (!Collision.CanHit(ref aabb, ref aabb2))
								{
									continue;
								}
								PlaceTile(num88, num89, (byte)EntityID.TileID.SWITCH, ToMute: true);
								if (Main.TileSet[num88, num89].IsActive == 0)
								{
									continue;
								}
								while (num88 != num82 || num89 != num84)
								{
									Main.TileSet[num88, num89].wire = 16;
									if (num88 > num82)
									{
										num88--;
									}
									if (num88 < num82)
									{
										num88++;
									}
									Main.TileSet[num88, num89].wire = 16;
									if (num89 > num84)
									{
										num89--;
									}
									if (num89 < num84)
									{
										num89++;
									}
									Main.TileSet[num88, num89].wire = 16;
								}
								if (Main.Rand.Next(3) > 0)
								{
									Main.TileSet[num82, num84].FrameX = 18;
									Main.TileSet[num82, num84 + 1].FrameX = 18;
								}
								break;
							}
							break;
						}
					}
				}
				if (num14 > num15)
				{
					num16++;
					num14 = 0;
				}
			}
			num14 = 0;
			num15 = 1000;
			num16 = 0;
			while (num16 < Main.MaxTilesX / 500)
			{
				num14++;
				int num90 = genRand.Next(dMinX, dMaxX);
				int num91 = genRand.Next(dMinY, dMaxY);
				if (Main.TileSet[num90, num91].WallType == wallType && placeTrap(num90, num91, 0))
				{
					num14 = num15;
				}
				if (num14 > num15)
				{
					num16++;
					num14 = 0;
				}
			}
		}

		public static void DungeonStairs(int i, int j, int tileType, int wallType)
		{
			Vector2 vector = default;
			Vector2 vector2 = default;
			double num = genRand.Next(5, 9);
			int num2 = 1;
			vector.X = i;
			vector.Y = j;
			int num3 = genRand.Next(10, 30);
			num2 = ((i <= dEnteranceX) ? 1 : (-1));
			if (i > Main.MaxTilesX - 400)
			{
				num2 = -1;
			}
			else if (i < 400)
			{
				num2 = 1;
			}
			vector2.Y = -1f;
			vector2.X = num2;
			if (genRand.Next(3) == 0)
			{
				vector2.X *= 0.5f;
			}
			else if (genRand.Next(3) == 0)
			{
				vector2.Y *= 2f;
			}
			while (num3 > 0)
			{
				num3--;
				int num4 = (int)(vector.X - num - 4.0 - genRand.Next(6));
				int num5 = (int)(vector.X + num + 4.0 + genRand.Next(6));
				int num6 = (int)(vector.Y - num - 4.0);
				int num7 = (int)(vector.Y + num + 4.0 + genRand.Next(6));
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				double num8 = 1.0;
				if (vector.X > Main.MaxTilesX >> 1)
				{
					num8 = -1.0;
				}
				int num9 = (int)(vector.X + dxStrength1 * 0.6 * num8 + dxStrength2 * num8);
				double num10 = Math.Floor(dyStrength2 * 0.5);
				int num11 = (int)(vector.Y - num + num10);
				if (vector.Y < Main.WorldSurface - 5 && Main.TileSet[num9, num11 - 6].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num9, num11 - 7].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num9, num11 - 8].WallType == (byte)EntityID.WallID.NONE)
				{
					dSurface = true;
					TileRunner(num9, (int)(vector.Y - num - 6.0 + num10), genRand.Next(25, 35), genRand.Next(10, 20), (int)EntityID.TileID.NONE, addTile: false, new Vector2(0f, -1f));
				}
				for (int k = num4; k < num5; k++)
				{
					for (int l = num6; l < num7; l++)
					{
						Main.TileSet[k, l].Liquid = 0;
						if (Main.TileSet[k, l].WallType != wallType)
						{
							Main.TileSet[k, l].WallType = (byte)EntityID.WallID.NONE;
							Main.TileSet[k, l].IsActive = 1;
							Main.TileSet[k, l].Type = (byte)tileType;
						}
					}
				}
				for (int m = num4 + 1; m < num5 - 1; m++)
				{
					for (int n = num6 + 1; n < num7 - 1; n++)
					{
						if (Main.TileSet[m, n].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[m, n].WallType = (byte)wallType;
						}
					}
				}
				int num12 = 0;
				if (genRand.Next((int)num) == 0)
				{
					num12 = genRand.Next(1, 3);
				}
				num4 = (int)(vector.X - num * 0.5 - num12);
				num5 = (int)(vector.X + num * 0.5 + num12);
				num6 = (int)(vector.Y - num * 0.5 - num12);
				num7 = (int)(vector.Y + num * 0.5 + num12);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				for (int num13 = num4; num13 < num5; num13++)
				{
					for (int num14 = num6; num14 < num7; num14++)
					{
						Main.TileSet[num13, num14].IsActive = 0;
						if (Main.TileSet[num13, num14].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[num13, num14].WallType = (byte)wallType;
						}
					}
				}
				if (dSurface)
				{
					num3 = 0;
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
			}
			dungeonX = (int)vector.X;
			dungeonY = (int)vector.Y;
		}

		public static void DungeonHalls(int i, int j, int tileType, int wallType, bool forceX = false)
		{
			Vector2 vector = default;
			Vector2 vector2 = default;
			double num = genRand.Next(4, 6);
			Vector2i vector2i = default;
			Vector2i vector2i2 = default;
			int num2 = 1;
			vector.X = i;
			vector.Y = j;
			int num3 = genRand.Next(35, 80);
			if (forceX)
			{
				num3 += 20;
				lastDungeonHall = default;
			}
			else if (genRand.Next(5) == 0)
			{
				num *= 2.0;
				num3 >>= 1;
			}
			do
			{
				num2 = (genRand.Next(2) << 1) - 1;
				if (forceX || genRand.Next(2) == 0)
				{
					vector2i.Y = 0;
					vector2i.X = num2;
					vector2i2.Y = 0;
					vector2i2.X = -num2;
					vector2.Y = 0f;
					vector2.X = num2;
					if (genRand.Next(3) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.Y = -0.2f;
						}
						else
						{
							vector2.Y = 0.2f;
						}
					}
					continue;
				}
				num += 1.0;
				vector2.Y = num2;
				vector2.X = 0f;
				vector2i.X = 0;
				vector2i.Y = num2;
				vector2i2.X = 0;
				vector2i2.Y = -num2;
				if (genRand.Next(2) == 0)
				{
					if (genRand.Next(2) == 0)
					{
						vector2.X = 0.3f;
					}
					else
					{
						vector2.X = -0.3f;
					}
				}
				else
				{
					num3 >>= 1;
				}
			}
			while (lastDungeonHall.X == vector2i2.X && lastDungeonHall.Y == vector2i2.Y);
			if (!forceX)
			{
				if (vector.X > lastMaxTilesX - 200)
				{
					num2 = -1;
					vector2i.Y = 0;
					vector2i.X = num2;
					vector2.Y = 0f;
					vector2.X = num2;
					if (genRand.Next(3) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.Y = -0.2f;
						}
						else
						{
							vector2.Y = 0.2f;
						}
					}
				}
				else if (vector.X < 200f)
				{
					num2 = 1;
					vector2i.Y = 0;
					vector2i.X = num2;
					vector2.Y = 0f;
					vector2.X = num2;
					if (genRand.Next(3) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.Y = -0.2f;
						}
						else
						{
							vector2.Y = 0.2f;
						}
					}
				}
				else if (vector.Y > lastMaxTilesY - 300)
				{
					num2 = -1;
					num += 1.0;
					vector2.Y = num2;
					vector2.X = 0f;
					vector2i.X = 0;
					vector2i.Y = num2;
					if (genRand.Next(2) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.X = 0.3f;
						}
						else
						{
							vector2.X = -0.3f;
						}
					}
				}
				else if (vector.Y < Main.RockLayer)
				{
					num2 = 1;
					num += 1.0;
					vector2.Y = num2;
					vector2.X = 0f;
					vector2i.X = 0;
					vector2i.Y = num2;
					if (genRand.Next(2) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.X = 0.3f;
						}
						else
						{
							vector2.X = -0.3f;
						}
					}
				}
				else if (vector.X < Main.MaxTilesX >> 1 && vector.X > Main.MaxTilesX >> 2)
				{
					num2 = -1;
					vector2i.Y = 0;
					vector2i.X = num2;
					vector2.Y = 0f;
					vector2.X = num2;
					if (genRand.Next(3) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.Y = -0.2f;
						}
						else
						{
							vector2.Y = 0.2f;
						}
					}
				}
				else if (vector.X > Main.MaxTilesX >> 1 && vector.X < Main.MaxTilesX * 0.75)
				{
					num2 = 1;
					vector2i.Y = 0;
					vector2i.X = num2;
					vector2.Y = 0f;
					vector2.X = num2;
					if (genRand.Next(3) == 0)
					{
						if (genRand.Next(2) == 0)
						{
							vector2.Y = -0.2f;
						}
						else
						{
							vector2.Y = 0.2f;
						}
					}
				}
			}
			if (vector2i.Y == 0)
			{
				dDoor[numDDoors].X = (short)vector.X;
				dDoor[numDDoors].Y = (short)vector.Y;
				dDoor[numDDoors].Pos = 0;
				numDDoors++;
			}
			else
			{
				DPlat[numDPlats].X = (short)vector.X;
				DPlat[numDPlats].Y = (short)vector.Y;
				numDPlats++;
			}
			lastDungeonHall = vector2i;
			while (num3 > 0)
			{
				if (vector2i.X > 0 && vector.X > Main.MaxTilesX - 100)
				{
					num3 = 0;
				}
				else if (vector2i.X < 0 && vector.X < 100f)
				{
					num3 = 0;
				}
				else if (vector2i.Y > 0 && vector.Y > Main.MaxTilesY - 100)
				{
					num3 = 0;
				}
				else if (vector2i.Y < 0 && vector.Y < Main.RockLayer + 50)
				{
					num3 = 0;
				}
				num3--;
				int num4 = (int)(vector.X - num - 4.0 - genRand.Next(6));
				int num5 = (int)(vector.X + num + 4.0 + genRand.Next(6));
				int num6 = (int)(vector.Y - num - 4.0 - genRand.Next(6));
				int num7 = (int)(vector.Y + num + 4.0 + genRand.Next(6));
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				for (int k = num4; k < num5; k++)
				{
					for (int l = num6; l < num7; l++)
					{
						Main.TileSet[k, l].Liquid = 0;
						if (Main.TileSet[k, l].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[k, l].IsActive = 1;
							Main.TileSet[k, l].Type = (byte)tileType;
						}
					}
				}
				for (int m = num4 + 1; m < num5 - 1; m++)
				{
					for (int n = num6 + 1; n < num7 - 1; n++)
					{
						if (Main.TileSet[m, n].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[m, n].WallType = (byte)wallType;
						}
					}
				}
				int num8 = 0;
				if (vector2.Y == 0f && genRand.Next((int)num + 1) == 0)
				{
					num8 = genRand.Next(1, 3);
				}
				else if (vector2.X == 0f && genRand.Next((int)num - 1) == 0)
				{
					num8 = genRand.Next(1, 3);
				}
				else if (genRand.Next((int)num * 3) == 0)
				{
					num8 = genRand.Next(1, 3);
				}
				num4 = (int)(vector.X - num * 0.5 - num8);
				num5 = (int)(vector.X + num * 0.5 + num8);
				num6 = (int)(vector.Y - num * 0.5 - num8);
				num7 = (int)(vector.Y + num * 0.5 + num8);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				for (int num9 = num4; num9 < num5; num9++)
				{
					for (int num10 = num6; num10 < num7; num10++)
					{
						Main.TileSet[num9, num10].IsActive = 0;
						Main.TileSet[num9, num10].WallType = (byte)wallType;
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
			}
			dungeonX = (int)vector.X;
			dungeonY = (int)vector.Y;
			if (vector2i.Y == 0)
			{
				dDoor[numDDoors].X = (short)vector.X;
				dDoor[numDDoors].Y = (short)vector.Y;
				dDoor[numDDoors].Pos = 0;
				numDDoors++;
			}
			else
			{
				DPlat[numDPlats].X = (short)vector.X;
				DPlat[numDPlats].Y = (short)vector.Y;
				numDPlats++;
			}
		}

		public static void DungeonRoom(int i, int j, int tileType, int wallType)
		{
			Vector2 vector = default;
			Vector2 vector2 = default;
			float num = genRand.Next(15, 30);
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-10, 11) * 0.1f;
			vector.X = i;
			vector.Y = j - num * 0.5f;
			int num2 = genRand.Next(10, 20);
			float num3 = vector.X;
			float num4 = vector.X;
			float num5 = vector.Y;
			float num6 = vector.Y;
			while (num2 > 0)
			{
				num2--;
				int num7 = (int)(vector.X - num * 0.8f - 5f);
				int num8 = (int)(vector.X + num * 0.8f + 5f);
				int num9 = (int)(vector.Y - num * 0.8f - 5f);
				int num10 = (int)(vector.Y + num * 0.8f + 5f);
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesX)
				{
					num8 = Main.MaxTilesX;
				}
				if (num9 < 0)
				{
					num9 = 0;
				}
				if (num10 > Main.MaxTilesY)
				{
					num10 = Main.MaxTilesY;
				}
				for (int k = num7; k < num8; k++)
				{
					for (int l = num9; l < num10; l++)
					{
						Main.TileSet[k, l].Liquid = 0;
						if (Main.TileSet[k, l].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[k, l].IsActive = 1;
							Main.TileSet[k, l].Type = (byte)tileType;
						}
					}
				}
				for (int m = num7 + 1; m < num8 - 1; m++)
				{
					for (int n = num9 + 1; n < num10 - 1; n++)
					{
						if (Main.TileSet[m, n].WallType == (byte)EntityID.WallID.NONE)
						{
							Main.TileSet[m, n].WallType = (byte)wallType;
						}
					}
				}
				num7 = (int)(vector.X - (double)num * 0.5);
				num8 = (int)(vector.X + (double)num * 0.5);
				num9 = (int)(vector.Y - (double)num * 0.5);
				num10 = (int)(vector.Y + (double)num * 0.5);
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesX)
				{
					num8 = Main.MaxTilesX;
				}
				if (num9 < 0)
				{
					num9 = 0;
				}
				if (num10 > Main.MaxTilesY)
				{
					num10 = Main.MaxTilesY;
				}
				if (num7 < num3)
				{
					num3 = num7;
				}
				if (num8 > num4)
				{
					num4 = num8;
				}
				if (num9 < num5)
				{
					num5 = num9;
				}
				if (num10 > num6)
				{
					num6 = num10;
				}
				for (int num11 = num7; num11 < num8; num11++)
				{
					for (int num12 = num9; num12 < num10; num12++)
					{
						Main.TileSet[num11, num12].IsActive = 0;
						Main.TileSet[num11, num12].WallType = (byte)wallType;
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 1f)
				{
					vector2.X = 1f;
				}
				if (vector2.X < -1f)
				{
					vector2.X = -1f;
				}
				if (vector2.Y > 1f)
				{
					vector2.Y = 1f;
				}
				if (vector2.Y < -1f)
				{
					vector2.Y = -1f;
				}
			}
			dRoom[numDRooms].X = (short)vector.X;
			dRoom[numDRooms].Y = (short)vector.Y;
			dRoom[numDRooms].Size = num;
			dRoom[numDRooms].L = (short)num3;
			dRoom[numDRooms].R = (short)num4;
			dRoom[numDRooms].T = (short)num5;
			dRoom[numDRooms].B = (short)num6;
			numDRooms++;
		}

		public static void DungeonEnt(int i, int j, int tileType, int wallType)
		{
			int num = 60;
			for (int k = i - num; k < i + num; k++)
			{
				for (int l = j - num; l < j + num; l++)
				{
					Main.TileSet[k, l].Liquid = 0;
					Main.TileSet[k, l].Lava = 0;
				}
			}
			double num2 = dxStrength1;
			double num3 = dyStrength1;
			Vector2 vector = default;
			vector.X = i;
			vector.Y = (float)(j - num3 * 0.5);
			dMinY = (int)vector.Y;
			int num4 = 1;
			if (i > Main.MaxTilesX >> 1)
			{
				num4 = -1;
			}
			int num5 = (int)(vector.X - num2 * 0.6f - genRand.Next(2, 5));
			int num6 = (int)(vector.X + num2 * 0.6f + genRand.Next(2, 5));
			int num7 = (int)(vector.Y - num3 * 0.6f - genRand.Next(2, 5));
			int num8 = (int)(vector.Y + num3 * 0.6f + genRand.Next(8, 16));
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int m = num5; m < num6; m++)
			{
				for (int n = num7; n < num8; n++)
				{
					Main.TileSet[m, n].Liquid = 0;
					if (Main.TileSet[m, n].WallType != wallType)
					{
						Main.TileSet[m, n].WallType = (byte)EntityID.WallID.NONE;
						if (m > num5 + 1 && m < num6 - 2 && n > num7 + 1 && n < num8 - 2)
						{
							Main.TileSet[m, n].WallType = (byte)wallType;
						}
						Main.TileSet[m, n].IsActive = 1;
						Main.TileSet[m, n].Type = (byte)tileType;
					}
				}
			}
			int num9 = num5;
			int num10 = num5 + 5 + genRand.Next(4);
			int num11 = num7 - 3 - genRand.Next(3);
			int num12 = num7;
			for (int num13 = num9; num13 < num10; num13++)
			{
				for (int num14 = num11; num14 < num12; num14++)
				{
					if (Main.TileSet[num13, num14].WallType != wallType)
					{
						Main.TileSet[num13, num14].IsActive = 1;
						Main.TileSet[num13, num14].Type = (byte)tileType;
					}
				}
			}
			num9 = num6 - 5 - genRand.Next(4);
			num10 = num6;
			num11 = num7 - 3 - genRand.Next(3);
			num12 = num7;
			for (int num15 = num9; num15 < num10; num15++)
			{
				for (int num16 = num11; num16 < num12; num16++)
				{
					if (Main.TileSet[num15, num16].WallType != wallType)
					{
						Main.TileSet[num15, num16].IsActive = 1;
						Main.TileSet[num15, num16].Type = (byte)tileType;
					}
				}
			}
			int num17 = 1 + genRand.Next(2);
			int num18 = 2 + genRand.Next(4);
			int num19 = 0;
			for (int num20 = num5; num20 < num6; num20++)
			{
				for (int num21 = num7 - num17; num21 < num7; num21++)
				{
					if (Main.TileSet[num20, num21].WallType != wallType)
					{
						Main.TileSet[num20, num21].IsActive = 1;
						Main.TileSet[num20, num21].Type = (byte)tileType;
					}
				}
				num19++;
				if (num19 >= num18)
				{
					num20 += num18;
					num19 = 0;
				}
			}
			for (int num22 = num5; num22 < num6; num22++)
			{
				for (int num23 = num8; num23 < num8 + 100; num23++)
				{
					if (Main.TileSet[num22, num23].WallType == (byte)EntityID.WallID.NONE)
					{
						Main.TileSet[num22, num23].WallType = (byte)EntityID.WallID.DIRT_UNSAFE;
					}
				}
			}
			num5 = (int)(vector.X - num2 * 0.6f);
			num6 = (int)(vector.X + num2 * 0.6f);
			num7 = (int)(vector.Y - num3 * 0.6f);
			num8 = (int)(vector.Y + num3 * 0.6f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num24 = num5; num24 < num6; num24++)
			{
				for (int num25 = num7; num25 < num8; num25++)
				{
					if (Main.TileSet[num24, num25].WallType == (byte)EntityID.WallID.NONE)
					{
						Main.TileSet[num24, num25].WallType = (byte)wallType;
					}
				}
			}
			num5 = (int)(vector.X - num2 * 0.6f - 1f);
			num6 = (int)(vector.X + num2 * 0.6f + 1f);
			num7 = (int)(vector.Y - num3 * 0.6f - 1f);
			num8 = (int)(vector.Y + num3 * 0.6f + 1f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num26 = num5; num26 < num6; num26++)
			{
				for (int num27 = num7; num27 < num8; num27++)
				{
					Main.TileSet[num26, num27].WallType = (byte)wallType;
				}
			}
			num5 = (int)(vector.X - num2 * 0.5f);
			num6 = (int)(vector.X + num2 * 0.5f);
			num7 = (int)(vector.Y - num3 * 0.5f);
			num8 = (int)(vector.Y + num3 * 0.5f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num28 = num5; num28 < num6; num28++)
			{
				for (int num29 = num7; num29 < num8; num29++)
				{
					Main.TileSet[num28, num29].IsActive = 0;
					Main.TileSet[num28, num29].WallType = (byte)wallType;
				}
			}
			DPlat[numDPlats].X = (short)vector.X;
			DPlat[numDPlats].Y = (short)num8;
			numDPlats++;
			vector.X += (float)num2 * 0.6f * num4;
			vector.Y += (float)num3 * 0.5f;
			num2 = dxStrength2;
			num3 = dyStrength2;
			vector.X += (float)num2 * 0.55f * num4;
			vector.Y -= (float)num3 * 0.5f;
			num5 = (int)(vector.X - num2 * 0.6f - genRand.Next(1, 3));
			num6 = (int)(vector.X + num2 * 0.6f + genRand.Next(1, 3));
			num7 = (int)(vector.Y - num3 * 0.6f - genRand.Next(1, 3));
			num8 = (int)(vector.Y + num3 * 0.6f + genRand.Next(6, 16));
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num30 = num5; num30 < num6; num30++)
			{
				for (int num31 = num7; num31 < num8; num31++)
				{
					if (Main.TileSet[num30, num31].WallType == wallType)
					{
						continue;
					}
					bool flag = true;
					if (num4 < 0)
					{
						if (num30 < vector.X - num2 * 0.5f)
						{
							flag = false;
						}
					}
					else if (num30 > vector.X + num2 * 0.5f - 1f)
					{
						flag = false;
					}
					if (flag)
					{
						Main.TileSet[num30, num31].WallType = (byte)EntityID.WallID.NONE;
						Main.TileSet[num30, num31].IsActive = 1;
						Main.TileSet[num30, num31].Type = (byte)tileType;
					}
				}
			}
			for (int num32 = num5; num32 < num6; num32++)
			{
				for (int num33 = num8; num33 < num8 + 100; num33++)
				{
					if (Main.TileSet[num32, num33].WallType == (byte)EntityID.WallID.NONE)
					{
						Main.TileSet[num32, num33].WallType = 2;
					}
				}
			}
			num5 = (int)(vector.X - num2 * 0.5f);
			num6 = (int)(vector.X + num2 * 0.5f);
			num9 = num5;
			if (num4 < 0)
			{
				num9++;
			}
			num10 = num9 + 5 + genRand.Next(4);
			num11 = num7 - 3 - genRand.Next(3);
			num12 = num7;
			for (int num34 = num9; num34 < num10; num34++)
			{
				for (int num35 = num11; num35 < num12; num35++)
				{
					if (Main.TileSet[num34, num35].WallType != wallType)
					{
						Main.TileSet[num34, num35].IsActive = 1;
						Main.TileSet[num34, num35].Type = (byte)tileType;
					}
				}
			}
			num9 = num6 - 5 - genRand.Next(4);
			num10 = num6;
			num11 = num7 - 3 - genRand.Next(3);
			num12 = num7;
			for (int num36 = num9; num36 < num10; num36++)
			{
				for (int num37 = num11; num37 < num12; num37++)
				{
					if (Main.TileSet[num36, num37].WallType != wallType)
					{
						Main.TileSet[num36, num37].IsActive = 1;
						Main.TileSet[num36, num37].Type = (byte)tileType;
					}
				}
			}
			num17 = 1 + genRand.Next(2);
			num18 = 2 + genRand.Next(4);
			num19 = 0;
			if (num4 < 0)
			{
				num6++;
			}
			for (int num38 = num5 + 1; num38 < num6 - 1; num38++)
			{
				for (int num39 = num7 - num17; num39 < num7; num39++)
				{
					if (Main.TileSet[num38, num39].WallType != wallType)
					{
						Main.TileSet[num38, num39].IsActive = 1;
						Main.TileSet[num38, num39].Type = (byte)tileType;
					}
				}
				num19++;
				if (num19 >= num18)
				{
					num38 += num18;
					num19 = 0;
				}
			}
			num5 = (int)(vector.X - num2 * 0.6f);
			num6 = (int)(vector.X + num2 * 0.6f);
			num7 = (int)(vector.Y - num3 * 0.6f);
			num8 = (int)(vector.Y + num3 * 0.6f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num40 = num5; num40 < num6; num40++)
			{
				for (int num41 = num7; num41 < num8; num41++)
				{
					Main.TileSet[num40, num41].WallType = (byte)EntityID.WallID.NONE;
				}
			}
			num5 = (int)(vector.X - num2 * 0.5f);
			num6 = (int)(vector.X + num2 * 0.5f);
			num7 = (int)(vector.Y - num3 * 0.5f);
			num8 = (int)(vector.Y + num3 * 0.5f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num42 = num5; num42 < num6; num42++)
			{
				for (int num43 = num7; num43 < num8; num43++)
				{
					Main.TileSet[num42, num43].IsActive = 0;
					Main.TileSet[num42, num43].WallType = (byte)EntityID.WallID.NONE;
				}
			}
			for (int num44 = num5; num44 < num6; num44++)
			{
				if (Main.TileSet[num44, num8].IsActive == 0)
				{
					Main.TileSet[num44, num8].IsActive = 1;
					Main.TileSet[num44, num8].Type = (byte)EntityID.TileID.PLATFORM;
				}
			}
			Main.DungeonX = (short)vector.X;
			Main.DungeonY = (short)num8;
			int num45 = NPC.NewNPC(Main.DungeonX * 16 + 8, Main.DungeonY * 16, (int)EntityID.NPCID.OLD_MAN);
			Main.NPCSet[num45].IsHomeless = false;
			Main.NPCSet[num45].HomeTileX = Main.DungeonX;
			Main.NPCSet[num45].HomeTileY = Main.DungeonY;
			if (num4 == 1)
			{
				int num46 = 0;
				for (int num47 = num6; num47 < num6 + 25; num47++)
				{
					num46++;
					for (int num48 = num8 + num46; num48 < num8 + 25; num48++)
					{
						Main.TileSet[num47, num48].IsActive = 1;
						Main.TileSet[num47, num48].Type = (byte)tileType;
					}
				}
			}
			else
			{
				int num49 = 0;
				for (int num50 = num5; num50 > num5 - 25; num50--)
				{
					num49++;
					for (int num51 = num8 + num49; num51 < num8 + 25; num51++)
					{
						Main.TileSet[num50, num51].IsActive = 1;
						Main.TileSet[num50, num51].Type = (byte)tileType;
					}
				}
			}
			num17 = 1 + genRand.Next(2);
			num18 = 2 + genRand.Next(4);
			num19 = 0;
			num5 = (int)(vector.X - num2 * 0.5f);
			num6 = (int)(vector.X + num2 * 0.5f);
			num5 += 2;
			num6 -= 2;
			for (int num52 = num5; num52 < num6; num52++)
			{
				for (int num53 = num7; num53 < num8; num53++)
				{
					if (Main.TileSet[num52, num53].WallType == (byte)EntityID.WallID.NONE)
					{
						Main.TileSet[num52, num53].WallType = (byte)wallType;
					}
				}
				if (++num19 >= num18)
				{
					num52 += num18 * 2;
					num19 = 0;
				}
			}
			vector.X -= (float)num2 * 0.6f * num4;
			vector.Y += (float)num3 * 0.5f;
			num2 = 15.0f;
			num3 = 3.0f;
			vector.Y -= (float)num3 * 0.5f;
			num5 = (int)(vector.X - num2 * 0.5f);
			num6 = (int)(vector.X + num2 * 0.5f);
			num7 = (int)(vector.Y - num3 * 0.5f);
			num8 = (int)(vector.Y + num3 * 0.5f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.MaxTilesX)
			{
				num6 = Main.MaxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.MaxTilesY)
			{
				num8 = Main.MaxTilesY;
			}
			for (int num54 = num5; num54 < num6; num54++)
			{
				for (int num55 = num7; num55 < num8; num55++)
				{
					Main.TileSet[num54, num55].IsActive = 0;
				}
			}
			if (num4 < 0)
			{
				vector.X -= 1f;
			}
			PlaceTile((int)vector.X, (int)vector.Y + 1, (byte)EntityID.TileID.DOOR_CLOSED);
		}

		public static bool AddBuriedChest(int i, int j, int contain = 0, bool notNearOtherChests = false, int Style = -1)
		{
			for (int k = j; k < Main.MaxTilesY; k++)
			{
				if (Main.TileSet[i, k].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, k].Type])
				{
					continue;
				}
				bool flag = false;
				int num = k;
				int num2 = -1;
				int style = 0;
				if (num >= Main.WorldSurface + 25 || contain > 0)
				{
					style = 1;
				}
				if (Style >= 0)
				{
					style = Style;
				}
				if (num > Main.MaxTilesY - 205 && contain == 0)
				{
					if (hellChest == 0)
					{
						contain = (int)EntityID.ItemID.DARK_LANCE;
						style = 4;
						flag = true;
					}
					else if (hellChest == 1)
					{
						contain = (int)EntityID.ItemID.SUNFURY;
						style = 4;
						flag = true;
					}
					else if (hellChest == 2)
					{
						contain = (int)EntityID.ItemID.FLOWER_OF_FIRE;
						style = 4;
						flag = true;
					}
					else if (hellChest == 3)
					{
						contain = (int)EntityID.ItemID.FLAMELASH;
						style = 4;
						flag = true;
						hellChest = 0;
					}
				}
				num2 = PlaceChest(i - 1, num - 1, notNearOtherChests, style);
				if (num2 >= 0)
				{
					if (flag)
					{
						hellChest++;
					}
					int num3 = 0;
					do
					{
						if (num < Main.WorldSurface + 25)
						{
							if (contain > 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(contain);
								Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
							}
							else
							{
								switch (genRand.Next(7))
								{
									case 0:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SPEAR);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 1:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BLOWPIPE);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 2:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.WOODEN_BOOMERANG);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 3:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GLOWSTICK, genRand.Next(50, 75));
										break;
									case 4:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.THROWING_KNIFE, genRand.Next(25, 50));
										break;
									default:
										if (genRand.Next(32) == 0)
										{
											Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.PET_SPAWN_1);
											break;
										}
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.AGLET);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
								}
							}
							num3++;
							if (genRand.Next(3) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GRENADE, genRand.Next(3, 6));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((genRand.Next(2) == 0) ? (int)EntityID.ItemID.COPPER_BAR : (int)EntityID.ItemID.IRON_BAR, genRand.Next(3, 11));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((genRand.Next(2) == 0) ? (int)EntityID.ItemID.WOODEN_ARROW : (int)EntityID.ItemID.SHURIKEN, genRand.Next(25, 51));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.LESSER_HEALING_POTION, genRand.Next(3, 6));
								num3++;
							}
							if (genRand.Next(3) > 0)
							{
								int num4 = genRand.Next(4);
								genRand.Next(1, 3);
								switch (num4)
								{
									case 0:
										num4 = (int)EntityID.ItemID.IRONSKIN_POTION;
										break;
									case 1:
										num4 = (int)EntityID.ItemID.SHINE_POTION;
										break;
									case 2:
										num4 = (int)EntityID.ItemID.NIGHT_OWL_POTION;
										break;
									default:
										num4 = (int)EntityID.ItemID.SWIFTNESS_POTION;
										break;
								}
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(num4, genRand.Next(1, 3));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((genRand.Next(2) == 0) ? (int)EntityID.ItemID.TORCH : (int)EntityID.ItemID.BOTTLE, genRand.Next(10, 21));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SILVER_COIN, genRand.Next(10, 30));
								num3++;
							}
							continue;
						}
						if (num < Main.RockLayer)
						{
							if (contain > 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(contain);
								Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								num3++;
							}
							else
							{
								int num5 = genRand.Next(7);
								if (num5 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BAND_OF_REGENERATION);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								}
								if (num5 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.MAGIC_MIRROR);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								}
								if (num5 == 2)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.ANGEL_STATUE);
								}
								if (num5 == 3)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.CLOUD_IN_A_BOTTLE);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								}
								if (num5 == 4)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HERMES_BOOTS);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								}
								if (num5 == 5)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.ENCHANTED_BOOMERANG);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								}
								if (num5 == 6)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.JESTERS_ARROW, (genRand.Next(26) + 25));
								}
								num3++;
								if (genRand.Next(40) == 0) // BUG: Dragon armour in chests? Ah yes, right here officer.
								{
									// Let me lay it down for you: So firstly, you needed to be in the underground layer (not the caverns, so have the brown background instead of the grey)...
									// You needed to find a stray gold chest, not one generated in a ruined house...
									// Then, you needed to hit a 1/40 chance for the extra item to spawn, along with a coin flip for the item to be the Dragon mask.
#if VERSION_INITIAL && !IS_PATCHED
									Main.ChestSet[num2].ItemSet[num3].SetDefaults(genRand.Next(2) + (int)EntityID.ItemID.PET_SPAWN_1);
#else
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.PET_SPAWN_1);
#endif
									num3++;
								}
							}
							if (genRand.Next(3) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BOMB, genRand.Next(10, 20));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int type = genRand.Next((int)EntityID.ItemID.SILVER_BAR, (int)EntityID.ItemID.IRON_BAR);
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(type, genRand.Next(5, 15));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int num6 = genRand.Next(2);
								int stack = genRand.Next(25, 50);
								if (num6 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.WOODEN_ARROW, stack);
								}
								else
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SHURIKEN, stack);
								}
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.LESSER_HEALING_POTION, genRand.Next(3, 6));
								num3++;
							}
							if (genRand.Next(3) > 0)
							{
								int type2;
								switch (genRand.Next(7))
								{
									case 0:
										type2 = (int)EntityID.ItemID.REGENERATION_POTION;
										break;
									case 1:
										type2 = (int)EntityID.ItemID.SHINE_POTION;
										break;
									case 2:
										type2 = (int)EntityID.ItemID.NIGHT_OWL_POTION;
										break;
									case 3:
										type2 = (int)EntityID.ItemID.SWIFTNESS_POTION;
										break;
									case 4:
										type2 = (int)EntityID.ItemID.ARCHERY_POTION;
										break;
									case 5:
										type2 = (int)EntityID.ItemID.GILLS_POTION;
										break;
									default:
										type2 = (int)EntityID.ItemID.HUNTER_POTION;
										break;
								}
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(type2, genRand.Next(1, 3));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.TORCH, genRand.Next(10, 21));
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SILVER_COIN, genRand.Next(50, 90));
								num3++;
							}
							continue;
						}
						if (num < Main.MaxTilesY - 250)
						{
							if (contain > 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults(contain);
								Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
								num3++;
							}
							else
							{
								int num7 = genRand.Next(7);
								if (num7 == 2 && genRand.Next(2) == 0)
								{
									num7 = genRand.Next(7);
								}
								switch (num7)
								{
									case 0:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BAND_OF_REGENERATION);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 1:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.MAGIC_MIRROR);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 2:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.ANGEL_STATUE);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 3:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.CLOUD_IN_A_BOTTLE);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 4:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HERMES_BOOTS);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									case 5:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.ENCHANTED_BOOMERANG);
										Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
										break;
									default:
										Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.JESTERS_ARROW, genRand.Next(26) + 25);
										break;
								}
								num3++;
							}
							if (genRand.Next(5) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SUSPICIOUS_LOOKING_EYE);
								num3++;
							}
							if (genRand.Next(3) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.DYNAMITE);
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int num8 = genRand.Next(2);
								int num9 = genRand.Next(8) + 3;
								if (num8 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GOLD_BAR);
								}
								if (num8 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SILVER_BAR);
								}
								Main.ChestSet[num2].ItemSet[num3].Stack = (short)num9;
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int num10 = genRand.Next(2);
								int num11 = genRand.Next(26) + 25;
								if (num10 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.FLAMING_ARROW);
								}
								if (num10 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.THROWING_KNIFE);
								}
								Main.ChestSet[num2].ItemSet[num3].Stack = (short)num11;
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int num12 = genRand.Next(1); // What? WHAT!? 1 is an upper bound, it cannot get to 1, only 0.
								int num13 = genRand.Next(3) + 3;
								if (num12 == 0) // Doing an RNG check for no reason; #EngineMoment
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HEALING_POTION, num13);
								}
								num3++;
							}
							if (genRand.Next(3) > 0)
							{
								int num14 = genRand.Next(6);
								int num15 = genRand.Next(1, 3);
								if (num14 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SPELUNKER_POTION);
								}
								if (num14 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.FEATHERFALL_POTION);
								}
								if (num14 == 2)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.NIGHT_OWL_POTION);
								}
								if (num14 == 3)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.WATER_WALKING_POTION);
								}
								if (num14 == 4)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.ARCHERY_POTION);
								}
								if (num14 == 5)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GRAVITATION_POTION);
								}
								Main.ChestSet[num2].ItemSet[num3].Stack = (short)num15;
								num3++;
							}
							if (genRand.Next(3) > 1)
							{
								int num16 = genRand.Next(4);
								int num17 = genRand.Next(1, 3);
								if (num16 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.THORNS_POTION);
								}
								if (num16 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.WATER_WALKING_POTION);
								}
								if (num16 == 2)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.INVISIBILITY_POTION);
								}
								if (num16 == 3)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HUNTER_POTION);
								}
								Main.ChestSet[num2].ItemSet[num3].Stack = (short)num17;
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								int num18 = genRand.Next(2);
								int num19 = genRand.Next(15) + 15;
								if (num18 == 0)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.TORCH);
								}
								if (num18 == 1)
								{
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GLOWSTICK);
								}
								Main.ChestSet[num2].ItemSet[num3].Stack = (short)num19;
								num3++;
							}
							if (genRand.Next(2) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GOLD_COIN, genRand.Next(1, 3));
								num3++;
							}
							if (genRand.Next(32) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.PET_SPAWN_2);
							}
							else if (genRand.Next(48) == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.PET_SPAWN_4);
							}
							continue;
						}
						if (contain > 0)
						{
							Main.ChestSet[num2].ItemSet[num3].SetDefaults(contain);
							Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
							num3++;
						}
						else
						{
							switch (genRand.Next(4))
							{
								case 0:
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BAND_OF_REGENERATION);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
									break;
								case 1:
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.MAGIC_MIRROR);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
									break;
								case 2:
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.CLOUD_IN_A_BOTTLE);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
									break;
								default:
									Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HERMES_BOOTS);
									Main.ChestSet[num2].ItemSet[num3].SetPrefix(-1);
									break;
							}
							num3++;
						}
						if (genRand.Next(3) == 0)
						{
							Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.DYNAMITE);
							num3++;
						}
						if (genRand.Next(2) == 0)
						{
							int num20 = genRand.Next(2);
							int num21 = genRand.Next(15) + 15;
							if (num20 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.METEORITE_BAR);
							}
							if (num20 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GOLD_BAR);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num21;
							num3++;
						}
						if (genRand.Next(2) == 0)
						{
							int num22 = genRand.Next(2);
							int num23 = genRand.Next(25) + 50;
							if (num22 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HELLFIRE_ARROW);
							}
							if (num22 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SILVER_BULLET);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num23;
							num3++;
						}
						if (genRand.Next(2) == 0)
						{
							int num24 = genRand.Next(2);
							int num25 = genRand.Next(15) + 15;
							if (num24 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.LESSER_RESTORATION_POTION);
							}
							if (num24 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.RESTORATION_POTION);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num25;
							num3++;
						}
						if (genRand.Next(4) > 0)
						{
							int num26 = genRand.Next(7);
							int num27 = genRand.Next(1, 3);
							if (num26 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.SPELUNKER_POTION);
							}
							if (num26 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.FEATHERFALL_POTION);
							}
							if (num26 == 2)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.MANA_REGENERATION_POTION);
							}
							if (num26 == 3)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.OBSIDIAN_SKIN_POTION);
							}
							if (num26 == 4)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.MAGIC_POWER_POTION);
							}
							if (num26 == 5)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.INVISIBILITY_POTION);
							}
							if (num26 == 6)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.HUNTER_POTION);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num27;
							num3++;
						}
						if (genRand.Next(3) > 0)
						{
							int num28 = genRand.Next(5);
							int num29 = genRand.Next(1, 3);
							if (num28 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GRAVITATION_POTION);
							}
							if (num28 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.THORNS_POTION);
							}
							if (num28 == 2)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.WATER_WALKING_POTION);
							}
							if (num28 == 3)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.OBSIDIAN_SKIN_POTION);
							}
							if (num28 == 4)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.BATTLE_POTION);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num29;
							num3++;
						}
						if (genRand.Next(2) == 0)
						{
							int num30 = genRand.Next(2);
							int num31 = genRand.Next(15) + 15;
							if (num30 == 0)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.TORCH);
							}
							if (num30 == 1)
							{
								Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GLOWSTICK);
							}
							Main.ChestSet[num2].ItemSet[num3].Stack = (short)num31;
							num3++;
						}
						else if (genRand.Next(48) == 0)
						{
							Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.PET_SPAWN_6);
							num3++;
						}
						if (genRand.Next(2) == 0)
						{
							Main.ChestSet[num2].ItemSet[num3].SetDefaults((int)EntityID.ItemID.GOLD_COIN, genRand.Next(2, 5));
							num3++;
						}
					}
					while (num3 == 0);
					return true;
				}
				return false;
			}
			return false;
		}

		public static int OpenDoor(int i, int j, int direction)
		{
			if (!DoOpenDoor(i, j, direction))
			{
				direction = -direction;
				if (!DoOpenDoor(i, j, direction))
				{
					direction = 0;
				}
			}
			return direction;
		}

		public static bool CanOpenDoor(int i, int j)
		{
			bool flag = DoCanOpenDoor(i, j, 1);
			if (!flag)
			{
				flag = DoCanOpenDoor(i, j, -1);
			}
			return flag;
		}

		private static bool DoCanOpenDoor(int i, int j, int direction)
		{
			if (Main.TileSet[i, j - 1].FrameY == 0 && Main.TileSet[i, j - 1].Type == Main.TileSet[i, j].Type)
			{
				j--;
			}
			else if (Main.TileSet[i, j - 2].FrameY == 0 && Main.TileSet[i, j - 2].Type == Main.TileSet[i, j].Type)
			{
				j -= 2;
			}
			else if (Main.TileSet[i, j + 1].FrameY == 0 && Main.TileSet[i, j + 1].Type == Main.TileSet[i, j].Type)
			{
				j++;
			}
			i += direction;
			for (int k = j; k < j + 3; k++)
			{
				if (Main.TileSet[i, k].IsActive != 0)
				{
					int type = Main.TileSet[i, k].Type;

					// Can anyone see a problem here? That's right, everything listed in the switch/case, is already in TileCut[].
					// This code runs a bunch of checks that are not needed in the slightest due to prior confirmation in TileCut[].
					// #EngineMoment

					if (!Main.TileCut[type])
					{
#if VERSION_103 || VERSION_FINAL
						return false;
#else
						switch ((EntityID.TileID)type)
						{
							case EntityID.TileID.SHORT_GRASS_PLANTS:
							case EntityID.TileID.SHORT_CORRUPT_PLANTS:
							case EntityID.TileID.VINE:
							case EntityID.TileID.SHORT_JUNGLE_PLANTS:
							case EntityID.TileID.JUNGLE_VINE:
							case EntityID.TileID.JUNGLE_THORN:
							case EntityID.TileID.GLOWING_MUSHROOM:
							case EntityID.TileID.TALL_GRASS_PLANTS:
							case EntityID.TileID.TALL_JUNGLE_PLANTS:
							case EntityID.TileID.SHORT_HALLOWED_PLANTS:
							case EntityID.TileID.TALL_HALLOWED_PLANTS:
							case EntityID.TileID.HALLOWED_VINE:
								break;

							default:
								return false;
						}
#endif
					}
				}
			}
			return true;
		}

		private static bool DoOpenDoor(int i, int j, int direction)
		{
			int num = j;
			if (Main.TileSet[i, j - 1].FrameY == 0 && Main.TileSet[i, j - 1].Type == Main.TileSet[i, j].Type)
			{
				num--;
			}
			else if (Main.TileSet[i, j - 2].FrameY == 0 && Main.TileSet[i, j - 2].Type == Main.TileSet[i, j].Type)
			{
				num -= 2;
			}
			else if (Main.TileSet[i, j + 1].FrameY == 0 && Main.TileSet[i, j + 1].Type == Main.TileSet[i, j].Type)
			{
				num++;
			}
			int num2 = i;
			int num3 = i;
			int num4;
			if (direction == -1)
			{
				num2--;
				num3--;
				num4 = 36;
			}
			else
			{
				num3++;
				num4 = 0;
			}
			bool flag = true;
			for (int k = num; k < num + 3; k++)
			{
				if (Main.TileSet[num3, k].IsActive != 0)
				{
					int type = Main.TileSet[num3, k].Type;

					if (!Main.TileCut[type])
					{
#if VERSION_103 || VERSION_FINAL
						flag = false;
						break;
#else
						switch ((EntityID.TileID)type)
						{
							case EntityID.TileID.SHORT_GRASS_PLANTS:
							case EntityID.TileID.SHORT_CORRUPT_PLANTS:
							case EntityID.TileID.VINE:
							case EntityID.TileID.SHORT_JUNGLE_PLANTS:
							case EntityID.TileID.JUNGLE_VINE:
							case EntityID.TileID.JUNGLE_THORN:
							case EntityID.TileID.GLOWING_MUSHROOM:
							case EntityID.TileID.TALL_GRASS_PLANTS:
							case EntityID.TileID.TALL_JUNGLE_PLANTS:
							case EntityID.TileID.SHORT_HALLOWED_PLANTS:
							case EntityID.TileID.TALL_HALLOWED_PLANTS:
							case EntityID.TileID.HALLOWED_VINE:
								break;

							default:
								flag = false;
								break;
						}

						if (!flag)
						{
							break;
						}
#endif
					}
					KillTile(num3, k);
				}
			}
			if (flag)
			{
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					for (int l = num2; l <= num2 + 1; l++)
					{
						for (int m = num; m <= num + 2; m++)
						{
							if (numNoWire < MaxNumWire - 1)
							{
								noWire[numNoWire].X = (short)l;
								noWire[numNoWire].Y = (short)m;
								numNoWire++;
							}
						}
					}
				}
				Main.PlaySound(8, i * 16, j * 16);
				Main.TileSet[num2, num].IsActive = 1;
				Main.TileSet[num2, num].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2, num].FrameY = 0;
				Main.TileSet[num2, num].FrameX = (short)num4;
				Main.TileSet[num2 + 1, num].IsActive = 1;
				Main.TileSet[num2 + 1, num].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2 + 1, num].FrameY = 0;
				Main.TileSet[num2 + 1, num].FrameX = (short)(num4 + 18);
				Main.TileSet[num2, num + 1].IsActive = 1;
				Main.TileSet[num2, num + 1].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2, num + 1].FrameY = 18;
				Main.TileSet[num2, num + 1].FrameX = (short)num4;
				Main.TileSet[num2 + 1, num + 1].IsActive = 1;
				Main.TileSet[num2 + 1, num + 1].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2 + 1, num + 1].FrameY = 18;
				Main.TileSet[num2 + 1, num + 1].FrameX = (short)(num4 + 18);
				Main.TileSet[num2, num + 2].IsActive = 1;
				Main.TileSet[num2, num + 2].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2, num + 2].FrameY = 36;
				Main.TileSet[num2, num + 2].FrameX = (short)num4;
				Main.TileSet[num2 + 1, num + 2].IsActive = 1;
				Main.TileSet[num2 + 1, num + 2].Type = (byte)EntityID.TileID.DOOR_OPEN;
				Main.TileSet[num2 + 1, num + 2].FrameY = 36;
				Main.TileSet[num2 + 1, num + 2].FrameX = (short)(num4 + 18);
				bool flag2 = tileFrameRecursion;
				tileFrameRecursion = false;
				for (int n = num2 - 1; n <= num2 + 2; n++)
				{
					for (int num5 = num - 1; num5 <= num + 2; num5++)
					{
						TileFrame(n, num5);
					}
				}
				tileFrameRecursion = flag2;
			}
			return flag;
		}

		public static void Check1xX(int x, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = j - Main.TileSet[x, j].FrameY / 18;
			int frameX = Main.TileSet[x, j].FrameX;
			int num2 = 3;
			if (type == (byte)EntityID.TileID.LAMP_POST)
			{
				num2 = 6;
			}
			int num3 = 0;
			while (true)
			{
				if (num3 < num2)
				{
					if (Main.TileSet[x, num + num3].IsActive == 0 || Main.TileSet[x, num + num3].Type != type || Main.TileSet[x, num + num3].FrameY != num3 * 18 || Main.TileSet[x, num + num3].FrameX != frameX)
					{
						break;
					}
					num3++;
					continue;
				}
				if (Main.TileSet[x, num + num2].IsActive == 0 || !Main.TileSolid[Main.TileSet[x, num + num2].Type])
				{
					break;
				}
				return;
			}
			ToDestroyObject = true;
			for (int i = 0; i < num2; i++)
			{
				if (Main.TileSet[x, num + i].Type == type)
				{
					KillTile(x, num + i);
				}
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.LAMP_POST:
						Item.NewItem(x * 16, j * 16, 32, 32, (int)EntityID.ItemID.LAMP_POST);
						break;
					case EntityID.TileID.TIKI_TORCH:
						Item.NewItem(x * 16, j * 16, 32, 32, (int)EntityID.ItemID.TIKI_TORCH);
						break;
				}
			}
			ToDestroyObject = false;
		}

		public static void Check2xX(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = Main.TileSet[i, j].FrameX % 36;
			if (num2 == 18)
			{
				num--;
			}
			int num3 = j - Main.TileSet[num, j].FrameY / 18;
			int frameX = Main.TileSet[num, j].FrameX;
			int num4 = 3;
			if (type == (byte)EntityID.TileID.GRANDFATHERS_CLOCK)
			{
				num4 = 5;
			}
			int num5 = 0;
			while (true)
			{
				if (num5 < num4)
				{
					if (Main.TileSet[num, num3 + num5].IsActive == 0 || Main.TileSet[num, num3 + num5].Type != type || Main.TileSet[num, num3 + num5].FrameY != num5 * 18 || Main.TileSet[num, num3 + num5].FrameX != frameX || Main.TileSet[num + 1, num3 + num5].IsActive == 0 || Main.TileSet[num + 1, num3 + num5].Type != type || Main.TileSet[num + 1, num3 + num5].FrameY != num5 * 18 || Main.TileSet[num + 1, num3 + num5].FrameX != frameX + 18)
					{
						break;
					}
					num5++;
					continue;
				}
				if (Main.TileSet[num, num3 + num4].IsActive == 0 || !Main.TileSolid[Main.TileSet[num, num3 + num4].Type] || Main.TileSet[num + 1, num3 + num4].IsActive == 0 || !Main.TileSolid[Main.TileSet[num + 1, num3 + num4].Type])
				{
					break;
				}
				return;
			}
			ToDestroyObject = true;
			for (int k = 0; k < num4; k++)
			{
				if (Main.TileSet[num, num3 + k].Type == type)
				{
					KillTile(num, num3 + k);
				}
				if (Main.TileSet[num + 1, num3 + k].Type == type)
				{
					KillTile(num + 1, num3 + k);
				}
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.GRANDFATHERS_CLOCK:
						Item.NewItem(num * 16, j * 16, 32, 32, (int)EntityID.ItemID.GRANDFATHER_CLOCK);
						break;
					case EntityID.TileID.STATUE:
						{
							int num6 = frameX / 36;
							switch (num6)
							{
								case 0:
									num6 = (int)EntityID.ItemID.STATUE;
									break;
								case 1:
									num6 = (int)EntityID.ItemID.ANGEL_STATUE;
									break;
								default:
									num6 = (int)EntityID.ItemID.STAR_STATUE + num6 - 2;
									break;
							}
							Item.NewItem(num * 16, j * 16, 32, 32, num6);
							break;
						}
				}
			}
			ToDestroyObject = false;
		}

		public static bool Place1xX(int x, int y, int type, int style = 0)
		{
			int num = style * 18;
			int num2 = 3;
			if (type == (byte)EntityID.TileID.LAMP_POST)
			{
				num2 = 6;
			}
			for (int i = y - num2 + 1; i < y + 1; i++)
			{
				if (Main.TileSet[x, i].IsActive != 0 || (type == (byte)EntityID.TileID.TIKI_TORCH && Main.TileSet[x, i].Liquid > 0))
				{
					return false;
				}
			}
			if (Main.TileSet[x, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y + 1].Type])
			{
				for (int j = 0; j < num2; j++)
				{
					Main.TileSet[x, y - num2 + 1 + j].IsActive = 1;
					Main.TileSet[x, y - num2 + 1 + j].FrameY = (short)(j * 18);
					Main.TileSet[x, y - num2 + 1 + j].FrameX = (short)num;
					Main.TileSet[x, y - num2 + 1 + j].Type = (byte)type;
				}
				return true;
			}
			return false;
		}

		public static bool Place2xX(int x, int y, int type, int style = 0)
		{
			int num = style * 36;
			int num2 = 3;
			if (type == (byte)EntityID.TileID.GRANDFATHERS_CLOCK)
			{
				num2 = 5;
			}
			for (int i = y - num2 + 1; i < y + 1; i++)
			{
				if (Main.TileSet[x, i].IsActive != 0 || Main.TileSet[x + 1, i].IsActive != 0)
				{
					return false;
				}
			}
			if (Main.TileSet[x, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y + 1].Type] && Main.TileSet[x + 1, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x + 1, y + 1].Type])
			{
				for (int j = 0; j < num2; j++)
				{
					Main.TileSet[x, y - num2 + 1 + j].IsActive = 1;
					Main.TileSet[x, y - num2 + 1 + j].FrameY = (short)(j * 18);
					Main.TileSet[x, y - num2 + 1 + j].FrameX = (short)num;
					Main.TileSet[x, y - num2 + 1 + j].Type = (byte)type;
					Main.TileSet[x + 1, y - num2 + 1 + j].IsActive = 1;
					Main.TileSet[x + 1, y - num2 + 1 + j].FrameY = (short)(j * 18);
					Main.TileSet[x + 1, y - num2 + 1 + j].FrameX = (short)(num + 18);
					Main.TileSet[x + 1, y - num2 + 1 + j].Type = (byte)type;
				}
				return true;
			}
			return false;
		}

		public static void Check1x2(int x, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = j;
			bool flag = true;
			int frameY = Main.TileSet[x, num].FrameY;
			int num2 = frameY / 40;
			frameY %= 40;
			if (frameY == 18)
			{
				num--;
			}
			if (Main.TileSet[x, num].FrameY == 40 * num2 && Main.TileSet[x, num + 1].FrameY == 40 * num2 + 18 && Main.TileSet[x, num].Type == type && Main.TileSet[x, num + 1].Type == type)
			{
				flag = false;
			}
			if (Main.TileSet[x, num + 2].IsActive == 0 || !Main.TileSolid[Main.TileSet[x, num + 2].Type])
			{
				flag = true;
			}
			if (Main.TileSet[x, num + 2].Type != (byte)EntityID.TileID.GRASS && Main.TileSet[x, num + 2].Type != (byte)EntityID.TileID.HALLOWED_GRASS && Main.TileSet[x, num + 2].Type != (byte)EntityID.TileID.SNOW && Main.TileSet[x, num].Type == (byte)EntityID.TileID.SAPLING)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			ToDestroyObject = true;
			if (Main.TileSet[x, num].Type == type)
			{
				KillTile(x, num);
			}
			if (Main.TileSet[x, num + 1].Type == type)
			{
				KillTile(x, num + 1);
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.CHAIR:
						if (num2 == 1)
						{
							Item.NewItem(x * 16, num * 16, 32, 32, (int)EntityID.ItemID.TOILET);
						}
						else
						{
							Item.NewItem(x * 16, num * 16, 32, 32, (int)EntityID.ItemID.WOODEN_CHAIR);
						}
						break;
					case EntityID.TileID.MYTHRIL_ANVIL:
						Item.NewItem(x * 16, num * 16, 32, 32, (int)EntityID.ItemID.MYTHRIL_ANVIL);
						break;
				}
			}
			ToDestroyObject = false;
		}

		public static void CheckOnTableClaypot(int x, int y)
		{
			if ((Main.TileSet[x, y + 1].IsActive == 0 || !Main.TileTable[Main.TileSet[x, y + 1].Type]) && (Main.TileSet[x, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[x, y + 1].Type]))
			{
				KillTile(x, y);
			}
		}

		public static void CheckOnTable1x1(int x, int y)
		{
			if (Main.TileSet[x, y + 1].IsActive == 0 || !Main.TileTable[Main.TileSet[x, y + 1].Type])
			{
				KillTile(x, y);
			}
		}

		public static void CheckSign(int x, int y, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = x - 2;
			int num2 = x + 3;
			int num3 = y - 2;
			int num4 = y + 3;
			if (num < 0 || num2 > Main.MaxTilesX || num3 < 0 || num4 > Main.MaxTilesY)
			{
				return;
			}
			bool flag = false;
			int num5 = (Main.TileSet[x, y].FrameX / 18) & 1;
			int num6 = Main.TileSet[x, y].FrameY / 18;
			int num7 = x - num5;
			int num8 = y - num6;
			int num9 = Main.TileSet[num7, num8].FrameX / 36;
			num = num7;
			num2 = num7 + 2;
			num3 = num8;
			num4 = num8 + 2;
			num5 = 0;
			for (int i = num; i < num2; i++)
			{
				num6 = 0;
				for (int j = num3; j < num4; j++)
				{
					if (Main.TileSet[i, j].IsActive == 0 || Main.TileSet[i, j].Type != type)
					{
						flag = true;
						break;
					}
					if (Main.TileSet[i, j].FrameX / 18 != num5 + num9 * 2 || Main.TileSet[i, j].FrameY / 18 != num6)
					{
						flag = true;
						break;
					}
					num6++;
				}
				num5++;
			}
			if (!flag)
			{
				if (type == (byte)EntityID.TileID.TOMBSTONE)
				{
					if (Main.TileSet[num7, num8 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num7, num8 + 2].Type] && Main.TileSet[num7 + 1, num8 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num7 + 1, num8 + 2].Type])
					{
						num9 = 0;
					}
					else
					{
						flag = true;
					}
				}
				else if (Main.TileSet[num7, num8 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num7, num8 + 2].Type] && Main.TileSet[num7 + 1, num8 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num7 + 1, num8 + 2].Type])
				{
					num9 = 0;
				}
				else if (Main.TileSet[num7, num8 - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7, num8 - 1].Type] && Main.TileSet[num7 + 1, num8 - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7 + 1, num8 - 1].Type])
				{
					num9 = 1;
				}
				else if (Main.TileSet[num7 - 1, num8].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7 - 1, num8].Type] && Main.TileSet[num7 - 1, num8 + 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7 - 1, num8 + 1].Type])
				{
					num9 = 2;
				}
				else if (Main.TileSet[num7 + 2, num8].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7 + 2, num8].Type] && Main.TileSet[num7 + 2, num8 + 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num7 + 2, num8 + 1].Type])
				{
					num9 = 3;
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				ToDestroyObject = true;
				for (int k = num; k < num2; k++)
				{
					for (int l = num3; l < num4; l++)
					{
						if (Main.TileSet[k, l].Type == type)
						{
							KillTile(k, l);
						}
					}
				}
				Sign.KillSign(num7, num8);
				if (!Gen)
				{
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.TOMBSTONE:
							Item.NewItem(x * 16, y * 16, 32, 32, (int)EntityID.ItemID.TOMBSTONE);
							break;
						case EntityID.TileID.SIGN:
							Item.NewItem(x * 16, y * 16, 32, 32, (int)EntityID.ItemID.SIGN);
							break;
					}
				}
				ToDestroyObject = false;
				return;
			}
			int num10 = 36 * num9;
			for (int m = 0; m < 2; m++)
			{
				for (int n = 0; n < 2; n++)
				{
					Main.TileSet[num7 + m, num8 + n].IsActive = 1;
					Main.TileSet[num7 + m, num8 + n].Type = (byte)type;
					Main.TileSet[num7 + m, num8 + n].FrameX = (short)(num10 + 18 * m);
					Main.TileSet[num7 + m, num8 + n].FrameY = (short)(18 * n);
				}
			}
		}

		public static bool PlaceSign(int x, int y, int type)
		{
			int num = x - 2;
			int num2 = x + 3;
			int num3 = y - 2;
			int num4 = y + 3;
			if (num < 0)
			{
				return false;
			}
			if (num2 > Main.MaxTilesX)
			{
				return false;
			}
			if (num3 < 0)
			{
				return false;
			}
			if (num4 > Main.MaxTilesY)
			{
				return false;
			}
			int num5 = x;
			int num6 = y;
			int num7 = 0;
			switch ((EntityID.TileID)type)
			{
				case EntityID.TileID.SIGN:
					if (Main.TileSet[x, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y + 1].Type] && Main.TileSet[x + 1, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x + 1, y + 1].Type])
					{
						num6--;
						num7 = 0;
						break;
					}
					if (Main.TileSet[x, y - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x, y - 1].Type] && Main.TileSet[x + 1, y - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x + 1, y - 1].Type])
					{
						num7 = 1;
						break;
					}
					if (Main.TileSet[x - 1, y].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x - 1, y].Type] && !Main.TileNoAttach[Main.TileSet[x - 1, y].Type] && Main.TileSet[x - 1, y + 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x - 1, y + 1].Type] && !Main.TileNoAttach[Main.TileSet[x - 1, y + 1].Type])
					{
						num7 = 2;
						break;
					}
					if (Main.TileSet[x + 1, y].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x + 1, y].Type] && !Main.TileNoAttach[Main.TileSet[x + 1, y].Type] && Main.TileSet[x + 1, y + 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x + 1, y + 1].Type] && !Main.TileNoAttach[Main.TileSet[x + 1, y + 1].Type])
					{
						num5--;
						num7 = 3;
						break;
					}
					return false;
				case EntityID.TileID.TOMBSTONE:
					if (Main.TileSet[x, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y + 1].Type] && Main.TileSet[x + 1, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x + 1, y + 1].Type])
					{
						num6--;
						num7 = 0;
						break;
					}
					return false;
			}
			if (Main.TileSet[num5, num6].IsActive != 0 || Main.TileSet[num5 + 1, num6].IsActive != 0 || Main.TileSet[num5, num6 + 1].IsActive != 0 || Main.TileSet[num5 + 1, num6 + 1].IsActive != 0)
			{
				return false;
			}
			int num8 = 36 * num7;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					Main.TileSet[num5 + i, num6 + j].IsActive = 1;
					Main.TileSet[num5 + i, num6 + j].Type = (byte)type;
					Main.TileSet[num5 + i, num6 + j].FrameX = (short)(num8 + 18 * i);
					Main.TileSet[num5 + i, num6 + j].FrameY = (short)(18 * j);
				}
			}
			return true;
		}

		public static bool Place1x1(int x, int y, int type, int style = 0)
		{
			if (Main.TileSet[x, y].IsActive == 0 && SolidTileUnsafe(x, y + 1))
			{
				Main.TileSet[x, y].IsActive = 1;
				Main.TileSet[x, y].Type = (byte)type;
				if (type == (byte)EntityID.TileID.TIMER)
				{
					Main.TileSet[x, y].FrameX = (short)(style * 18);
					Main.TileSet[x, y].FrameY = 0;
				}
				else
				{
					Main.TileSet[x, y].FrameY = (short)(style * 18);
				}
				return true;
			}
			return false;
		}

		public static void Check1x1(int x, int y, int type)
		{
			if (Main.TileSet[x, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[x, y + 1].Type])
			{
				KillTile(x, y);
			}
		}

		public static bool PlaceOnTable1x1(int x, int y, int type, int style = 0)
		{
			if (Main.TileSet[x, y].IsActive == 0 && Main.TileSet[x, y + 1].IsActive != 0)
			{
				int type2 = Main.TileSet[x, y + 1].Type;
				if (Main.TileTable[type2] || (type == (byte)EntityID.TileID.CLAY_POT && Main.TileSolid[type2]))
				{
					Main.TileSet[x, y].IsActive = 1;
					Main.TileSet[x, y].FrameX = (short)(style * 18);
					Main.TileSet[x, y].FrameY = 0;
					Main.TileSet[x, y].Type = (byte)type;
					if (type == (byte)EntityID.TileID.BOOK)
					{
						Main.TileSet[x, y].FrameX = (short)(18 * genRand.Next(5));
					}
					return true;
				}
			}
			return false;
		}

		public static bool PlaceAlch(int x, int y, int style)
		{
			if (Main.TileSet[x, y].IsActive == 0 && Main.TileSet[x, y + 1].IsActive != 0)
			{
				bool flag = false;
				switch (style)
				{
					case 0: // Daybloom
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.GRASS && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.HALLOWED_GRASS)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0)
						{
							flag = true;
						}
						break;
					case 1: // Moonglow
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.JUNGLE_GRASS && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0)
						{
							flag = true;
						}
						break;
					case 2: // Blinkroot
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.DIRT && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.MUD && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0)
						{
							flag = true;
						}
						break;
					case 3: // Deathweed
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.EBONSTONE && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0)
						{
							flag = true;
						}
						break;
					case 4:	// Waterleaf
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.SAND && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.PEARLSAND)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
						{
							flag = true;
						}
						break;
					case 5:	// Fireblossom
						if (Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.ASH && Main.TileSet[x, y + 1].Type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava == 0)
						{
							flag = true;
						}
						break;
				}
				if (!flag)
				{
					Main.TileSet[x, y].IsActive = 1;
					Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_GROWING;
					Main.TileSet[x, y].FrameX = (short)(18 * style);
					Main.TileSet[x, y].FrameY = 0;
					return true;
				}
			}
			return false;
		}

		public static void GrowAlch(int x, int y)
		{
			if (Main.TileSet[x, y].IsActive == 0)
			{
				return;
			}
			if (Main.TileSet[x, y].Type == (byte)EntityID.TileID.DAYBLOOM_GROWING && genRand.Next(50) == 0)
			{
				Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_MATURE;
				SquareTileFrame(x, y);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.SendTile(x, y);
				}
			}
			else if (Main.TileSet[x, y].FrameX == 36)
			{
				if (Main.TileSet[x, y].Type == (byte)EntityID.TileID.DAYBLOOM_MATURE)
				{
					Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_BLOOMING;
				}
				else
				{
					Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_MATURE;
				}
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.SendTile(x, y);
				}
			}
		}

		public static void PlantAlch()
		{
			int num = genRand.Next(20, Main.MaxTilesX - 20);
			int i;
			switch (genRand.Next(40))
			{
				case 0:
					i = genRand.Next(Main.RockLayer + Main.MaxTilesY >> 1, Main.MaxTilesY - 20);
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					i = genRand.Next(Main.MaxTilesY - 20);
					break;
				default:
					i = genRand.Next(Main.WorldSurface, Main.MaxTilesY - 20);
					break;
			}
			for (; i < Main.MaxTilesY - 20 && Main.TileSet[num, i].IsActive == 0; i++)
			{
			}
			if (Main.TileSet[num, i].IsActive != 0 && Main.TileSet[num, i - 1].IsActive == 0 && Main.TileSet[num, i - 1].Liquid == 0)
			{
				int num2 = -1;
				switch ((EntityID.TileID)Main.TileSet[num, i].Type)
				{
					case EntityID.TileID.GRASS:
					case EntityID.TileID.HALLOWED_GRASS:
						num2 = 0;
						break;
					case EntityID.TileID.JUNGLE_GRASS:
						num2 = 1;
						break;
					case EntityID.TileID.DIRT:
					case EntityID.TileID.MUD:
						num2 = 2;
						break;
					case EntityID.TileID.CORRUPT_GRASS:
					case EntityID.TileID.EBONSTONE:
						num2 = 3;
						break;
					case EntityID.TileID.SAND:
					case EntityID.TileID.PEARLSAND:
						num2 = 4;
						break;
					case EntityID.TileID.ASH:
						num2 = 5;
						break;
				}
				if (num2 >= 0 && PlaceAlch(num, i - 1, num2) && Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.SendTile(num, i - 1);
				}
			}
		}

		public static void CheckAlch(int x, int y)
		{
			bool flag = Main.TileSet[x, y + 1].IsActive == 0;
			Main.TileSet[x, y].FrameY = 0;
			if (!flag)
			{
				int num = Main.TileSet[x, y].FrameX / 18;
				int type = Main.TileSet[x, y + 1].Type;
				switch (num)
				{
					case 0:
						if (type != (byte)EntityID.TileID.HALLOWED_GRASS && type != (byte)EntityID.TileID.GRASS && type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
						{
							flag = true;
						}
						break;
					case 1:
						if (type != (byte)EntityID.TileID.JUNGLE_GRASS && type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
						{
							flag = true;
						}
						break;
					case 2:
						if (type != (byte)EntityID.TileID.DIRT && type != (byte)EntityID.TileID.MUD && type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
						{
							flag = true;
						}
						break;
					case 3:
						if (type != (byte)EntityID.TileID.CORRUPT_GRASS && type != (byte)EntityID.TileID.EBONSTONE && type != (byte)EntityID.TileID.CLAY_POT)
						{
							flag = true;
						}
						else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
						{
							flag = true;
						}
						break;
					default:
						{
							int type2 = Main.TileSet[x, y].Type;
							switch (num)
							{
								case 4:
									if (type != (byte)EntityID.TileID.SAND && type != (byte)EntityID.TileID.CLAY_POT && type != (byte)EntityID.TileID.PEARLSAND)
									{
										flag = true;
									}
									else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava != 0)
									{
										flag = true;
									}
									if (type2 == (byte)EntityID.TileID.DAYBLOOM_GROWING || Main.TileSet[x, y].Lava != 0 || Main.NetMode == (byte)NetModeSetting.CLIENT)
									{
										break;
									}
									if (Main.TileSet[x, y].Liquid > 16)
									{
										if (type2 == (byte)EntityID.TileID.DAYBLOOM_MATURE)
										{
											Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_BLOOMING;
											if (Main.NetMode == (byte)NetModeSetting.SERVER)
											{
												NetMessage.SendTile(x, y);
											}
										}
									}
									else if (type2 == (byte)EntityID.TileID.DAYBLOOM_BLOOMING)
									{
										Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_MATURE;
										if (Main.NetMode == (byte)NetModeSetting.SERVER)
										{
											NetMessage.SendTile(x, y);
										}
									}
									break;
								case 5:
									if (type != (byte)EntityID.TileID.ASH && type != (byte)EntityID.TileID.CLAY_POT)
									{
										flag = true;
									}
									else if (Main.TileSet[x, y].Liquid > 0 && Main.TileSet[x, y].Lava == 0)
									{
										flag = true;
									}
									if (Main.NetMode == (byte)NetModeSetting.CLIENT || type2 == (byte)EntityID.TileID.DAYBLOOM_GROWING || Main.TileSet[x, y].Lava == 0)
									{
										break;
									}
									if (Main.TileSet[x, y].Liquid > 16)
									{
										if (type2 == (byte)EntityID.TileID.DAYBLOOM_MATURE)
										{
											Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_BLOOMING;
											if (Main.NetMode == (byte)NetModeSetting.SERVER)
											{
												NetMessage.SendTile(x, y);
											}
										}
									}
									else if (type2 == (byte)EntityID.TileID.DAYBLOOM_BLOOMING)
									{
										Main.TileSet[x, y].Type = (byte)EntityID.TileID.DAYBLOOM_MATURE;
										if (Main.NetMode == (byte)NetModeSetting.SERVER)
										{
											NetMessage.SendTile(x, y);
										}
									}
									break;
							}
							break;
						}
				}
			}
			if (flag)
			{
				KillTile(x, y);
			}
		}

		public static void CheckBanner(int x, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = j - Main.TileSet[x, j].FrameY / 18;
			int frameX = Main.TileSet[x, j].FrameX;
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (Main.TileSet[x, num + i].IsActive == 0)
				{
					flag = true;
					break;
				}
				if (Main.TileSet[x, num + i].Type != (byte)EntityID.TileID.BANNER)
				{
					flag = true;
					break;
				}
				if (Main.TileSet[x, num + i].FrameY != i * 18)
				{
					flag = true;
					break;
				}
				if (Main.TileSet[x, num + i].FrameX != frameX)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				flag = Main.TileSet[x, num - 1].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[x, num - 1].Type];
			}
			if (!flag)
			{
				return;
			}
			ToDestroyObject = true;
			for (int k = 0; k < 3; k++)
			{
				if (Main.TileSet[x, num + k].Type == (byte)EntityID.TileID.BANNER)
				{
					KillTile(x, num + k);
				}
			}
			if (!Gen)
			{
				Item.NewItem(x * 16, (num + 1) * 16, 32, 32, (int)EntityID.ItemID.RED_BANNER + frameX / 18);
			}
			ToDestroyObject = false;
		}

		public static bool PlaceBanner(int x, int y, int type, int style = 0)
		{
			int num = style * 18;
			if (Main.TileSet[x, y - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x, y - 1].Type] && Main.TileSet[x, y].IsActive == 0 && Main.TileSet[x, y + 1].IsActive == 0 && Main.TileSet[x, y + 2].IsActive == 0)
			{
				Main.TileSet[x, y].IsActive = 1;
				Main.TileSet[x, y].FrameY = 0;
				Main.TileSet[x, y].FrameX = (short)num;
				Main.TileSet[x, y].Type = (byte)type;
				Main.TileSet[x, y + 1].IsActive = 1;
				Main.TileSet[x, y + 1].FrameY = 18;
				Main.TileSet[x, y + 1].FrameX = (short)num;
				Main.TileSet[x, y + 1].Type = (byte)type;
				Main.TileSet[x, y + 2].IsActive = 1;
				Main.TileSet[x, y + 2].FrameY = 36;
				Main.TileSet[x, y + 2].FrameX = (short)num;
				Main.TileSet[x, y + 2].Type = (byte)type;
				return true;
			}
			return false;
		}

		public static bool PlaceMan(int i, int j, int dir)
		{
			for (int k = i; k <= i + 1; k++)
			{
				for (int l = j - 2; l <= j; l++)
				{
					if (Main.TileSet[k, l].IsActive != 0)
					{
						return false;
					}
				}
			}
			if (!SolidTileUnsafe(i, j + 1) || !SolidTileUnsafe(i + 1, j + 1))
			{
				return false;
			}
			int num = ((dir == 1) ? 36 : 0);
			Main.TileSet[i, j - 2].IsActive = 1;
			Main.TileSet[i, j - 2].FrameY = 0;
			Main.TileSet[i, j - 2].FrameX = (byte)num;
			Main.TileSet[i, j - 2].Type = (byte)EntityID.TileID.MANNEQUIN;
			Main.TileSet[i, j - 1].IsActive = 1;
			Main.TileSet[i, j - 1].FrameY = 18;
			Main.TileSet[i, j - 1].FrameX = (byte)num;
			Main.TileSet[i, j - 1].Type = (byte)EntityID.TileID.MANNEQUIN;
			Main.TileSet[i, j].IsActive = 1;
			Main.TileSet[i, j].FrameY = 36;
			Main.TileSet[i, j].FrameX = (byte)num;
			Main.TileSet[i, j].Type = (byte)EntityID.TileID.MANNEQUIN;
			Main.TileSet[i + 1, j - 2].IsActive = 1;
			Main.TileSet[i + 1, j - 2].FrameY = 0;
			Main.TileSet[i + 1, j - 2].FrameX = (byte)(18 + num);
			Main.TileSet[i + 1, j - 2].Type = (byte)EntityID.TileID.MANNEQUIN;
			Main.TileSet[i + 1, j - 1].IsActive = 1;
			Main.TileSet[i + 1, j - 1].FrameY = 18;
			Main.TileSet[i + 1, j - 1].FrameX = (byte)(18 + num);
			Main.TileSet[i + 1, j - 1].Type = (byte)EntityID.TileID.MANNEQUIN;
			Main.TileSet[i + 1, j].IsActive = 1;
			Main.TileSet[i + 1, j].FrameY = 36;
			Main.TileSet[i + 1, j].FrameX = (byte)(18 + num);
			Main.TileSet[i + 1, j].Type = (byte)EntityID.TileID.MANNEQUIN;
			return true;
		}

		public static void CheckMan(int i, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j - Main.TileSet[i, j].FrameY / 18;
			int num3 = Main.TileSet[i, j].FrameX % 100 % 36;
			num -= num3 / 18;
			bool flag = false;
			for (int k = 0; k <= 1; k++)
			{
				for (int l = 0; l <= 2; l++)
				{
					int num4 = num + k;
					int num5 = num2 + l;
					int num6 = Main.TileSet[num4, num5].FrameX % 100;
					if (num6 >= 36)
					{
						num6 -= 36;
					}
					if (Main.TileSet[num4, num5].IsActive == 0 || Main.TileSet[num4, num5].Type != (byte)EntityID.TileID.MANNEQUIN || Main.TileSet[num4, num5].FrameY != l * 18 || num6 != k * 18)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && SolidTileUnsafe(num, num2 + 3) && SolidTileUnsafe(num + 1, num2 + 3))
			{
				return;
			}
			ToDestroyObject = true;
			for (int m = 0; m <= 1; m++)
			{
				for (int n = 0; n <= 2; n++)
				{
					int num7 = num + m;
					int num8 = num2 + n;
					if (Main.TileSet[num7, num8].IsActive != 0 && Main.TileSet[num7, num8].Type == (byte)EntityID.TileID.MANNEQUIN)
					{
						KillTile(num7, num8);
					}
				}
			}
			if (!Gen)
			{
				Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MANNEQUIN);
			}
			ToDestroyObject = false;
		}

		public static bool Place1x2(int x, int y, int type, int style)
		{
			if (Main.TileSet[x, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[x, y + 1].Type] && Main.TileSet[x, y - 1].IsActive == 0)
			{
				int num = ((type == (byte)EntityID.TileID.SAPLING) ? (genRand.Next(3) * 18) : 0);
				int num2 = style * 40;
				Main.TileSet[x, y - 1].IsActive = 1;
				Main.TileSet[x, y - 1].FrameY = (short)num2;
				Main.TileSet[x, y - 1].FrameX = (short)num;
				Main.TileSet[x, y - 1].Type = (byte)type;
				Main.TileSet[x, y].IsActive = 1;
				Main.TileSet[x, y].FrameY = (short)(num2 + 18);
				Main.TileSet[x, y].FrameX = (short)num;
				Main.TileSet[x, y].Type = (byte)type;
				return true;
			}
			return false;
		}

		public static bool Place1x2Top(int x, int y, int type)
		{
			if (Main.TileSet[x, y - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[x, y - 1].Type] && Main.TileSet[x, y + 1].IsActive == 0)
			{
				Main.TileSet[x, y].IsActive = 1;
				Main.TileSet[x, y].FrameY = 0;
				Main.TileSet[x, y].FrameX = 0;
				Main.TileSet[x, y].Type = (byte)type;
				Main.TileSet[x, y + 1].IsActive = 1;
				Main.TileSet[x, y + 1].FrameY = 18;
				Main.TileSet[x, y + 1].FrameX = 0;
				Main.TileSet[x, y + 1].Type = (byte)type;
				return true;
			}
			return false;
		}

		public static void Check1x2Top(int x, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = j;
			if (Main.TileSet[x, num].FrameY == 18)
			{
				num--;
			}
			if (Main.TileSet[x, num].FrameY != 0 || Main.TileSet[x, num + 1].FrameY != 18 || Main.TileSet[x, num].Type != (byte)EntityID.TileID.CHAIN_LANTERN || Main.TileSet[x, num + 1].Type != (byte)EntityID.TileID.CHAIN_LANTERN || Main.TileSet[x, num - 1].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[x, num - 1].Type])
			{
				ToDestroyObject = true;
				if (Main.TileSet[x, num].Type == (byte)EntityID.TileID.CHAIN_LANTERN)
				{
					KillTile(x, num);
				}
				if (Main.TileSet[x, num + 1].Type == (byte)EntityID.TileID.CHAIN_LANTERN)
				{
					KillTile(x, num + 1);
				}
				if (!Gen)
				{
					Item.NewItem(x * 16, num * 16, 32, 32, (int)EntityID.ItemID.CHAIN_LANTERN);
				}
				ToDestroyObject = false;
			}
		}

		public static void Check2x1(int i, int y, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			if (Main.TileSet[num, y].FrameX == 18)
			{
				num--;
			}
			if (Main.TileSet[num, y].FrameX == 0 && Main.TileSet[num + 1, y].FrameX == 18 && Main.TileSet[num, y].Type == type && Main.TileSet[num + 1, y].Type == type)
			{
				if (type == (byte)EntityID.TileID.PIGGYBANK || type == (byte)EntityID.TileID.BOWL)
				{
					if (Main.TileSet[num, y + 1].IsActive != 0 && Main.TileTable[Main.TileSet[num, y + 1].Type] && Main.TileSet[num + 1, y + 1].IsActive != 0 && Main.TileTable[Main.TileSet[num + 1, y + 1].Type])
					{
						return;
					}
				}
				else if (Main.TileSet[num, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[num, y + 1].Type] && Main.TileSet[num + 1, y + 1].IsActive != 0 && Main.TileSolid[Main.TileSet[num + 1, y + 1].Type])
				{
					return;
				}
			}
			ToDestroyObject = true;
			if (Main.TileSet[num, y].Type == type)
			{
				KillTile(num, y);
			}
			if (Main.TileSet[num + 1, y].Type == type)
			{
				KillTile(num + 1, y);
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.ANVIL:
						Item.NewItem(num * 16, y * 16, 32, 32, (int)EntityID.ItemID.IRON_ANVIL);
						break;
					case EntityID.TileID.WORK_BENCH:
						Item.NewItem(num * 16, y * 16, 32, 32, (int)EntityID.ItemID.WORK_BENCH);
						break;
					case EntityID.TileID.PIGGYBANK:
						Item.NewItem(num * 16, y * 16, 32, 32, (int)EntityID.ItemID.PIGGY_BANK);
						Main.PlaySound(13, i * 16, y * 16);
						break;
					case EntityID.TileID.BOWL:
						Item.NewItem(num * 16, y * 16, 32, 32, (int)EntityID.ItemID.BOWL);
						Main.PlaySound(13, i * 16, y * 16);
						break;
					case EntityID.TileID.MYTHRIL_ANVIL:
						Item.NewItem(num * 16, y * 16, 32, 32, (int)EntityID.ItemID.MYTHRIL_ANVIL);
						break;
				}
			}
			ToDestroyObject = false;
			SquareTileFrame(num, y);
			SquareTileFrame(num + 1, y);
		}

		public static bool Place2x1(int x, int y, int type)
		{
			if ((type == (byte)EntityID.TileID.PIGGYBANK || type == (byte)EntityID.TileID.BOWL || Main.TileSet[x, y + 1].IsActive == 0 || Main.TileSet[x + 1, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[x, y + 1].Type] || !Main.TileSolid[Main.TileSet[x + 1, y + 1].Type] || Main.TileSet[x, y].IsActive != 0 || Main.TileSet[x + 1, y].IsActive != 0) && ((type != 29 && type != 103) || Main.TileSet[x, y + 1].IsActive == 0 || Main.TileSet[x + 1, y + 1].IsActive == 0 || !Main.TileTable[Main.TileSet[x, y + 1].Type] || !Main.TileTable[Main.TileSet[x + 1, y + 1].Type] || Main.TileSet[x, y].IsActive != 0 || Main.TileSet[x + 1, y].IsActive != 0))
			{
				return false;
			}
			Main.TileSet[x, y].IsActive = 1;
			Main.TileSet[x, y].FrameY = 0;
			Main.TileSet[x, y].FrameX = 0;
			Main.TileSet[x, y].Type = (byte)type;
			Main.TileSet[x + 1, y].IsActive = 1;
			Main.TileSet[x + 1, y].FrameY = 0;
			Main.TileSet[x + 1, y].FrameX = 18;
			Main.TileSet[x + 1, y].Type = (byte)type;
			return true;
		}

		private static void Destroy4x2(int i, int j, int x2, int y2, int type)
		{
			ToDestroyObject = true;
			for (int k = x2; k < x2 + 4; k++)
			{
				for (int l = y2; l < y2 + 3; l++)
				{
					if (Main.TileSet[k, l].Type == type && Main.TileSet[k, l].IsActive != 0)
					{
						KillTile(k, l);
					}
				}
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.BED:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.BED);
						break;
					case EntityID.TileID.BATHTUB:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.BATHTUB);
						break;
				}
			}
			ToDestroyObject = false;
			bool flag = tileFrameRecursion;
			tileFrameRecursion = false;
			for (int m = x2 - 1; m < x2 + 4; m++)
			{
				for (int n = y2 - 1; n < y2 + 4; n++)
				{
					TileFrame(m, n);
				}
			}
			tileFrameRecursion = flag;
		}

		public static void Check4x2(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			num -= Main.TileSet[i, j].FrameX / 18;
			if ((type == (byte)EntityID.TileID.BED || type == (byte)EntityID.TileID.BATHTUB) && Main.TileSet[i, j].FrameX >= 72)
			{
				num += 4;
			}
			num2 -= Main.TileSet[i, j].FrameY / 18;
			for (int k = num; k < num + 4; k++)
			{
				for (int l = num2; l < num2 + 2; l++)
				{
					int num3 = (k - num) * 18;
					if ((type == (byte)EntityID.TileID.BED || type == (byte)EntityID.TileID.BATHTUB) && Main.TileSet[i, j].FrameX >= 72)
					{
						num3 = (k - num + 4) * 18;
					}
					if (Main.TileSet[k, l].IsActive == 0 || Main.TileSet[k, l].Type != type || Main.TileSet[k, l].FrameX != num3 || Main.TileSet[k, l].FrameY != (l - num2) * 18)
					{
						Destroy4x2(i, j, num, num2, type);
						return;
					}
				}
				if (Main.TileSet[k, num2 + 2].IsActive == 0 || !Main.TileSolid[Main.TileSet[k, num2 + 2].Type])
				{
					Destroy4x2(i, j, num, num2, type);
					break;
				}
			}
		}

		private static void Destroy2x2(int i, int j, int x2, int y2, int type)
		{
			ToDestroyObject = true;
			for (int k = x2; k < x2 + 2; k++)
			{
				for (int l = y2; l < y2 + 2; l++)
				{
					if (Main.TileSet[k, l].Type == type && Main.TileSet[k, l].IsActive != 0)
					{
						KillTile(k, l);
					}
				}
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.TOMBSTONE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.TOMBSTONE);
						break;
					case EntityID.TileID.KEG:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.KEG);
						break;
					case EntityID.TileID.CHINESE_LANTERN:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.CHINESE_LANTERN);
						break;
					case EntityID.TileID.COOKING_POT:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.COOKING_POT);
						break;
					case EntityID.TileID.SAFE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.SAFE);
						break;
					case EntityID.TileID.SKULL_LANTERN:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.SKULL_LANTERN);
						break;
					case EntityID.TileID.TRASH_CAN:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.TRASH_CAN);
						break;
					case EntityID.TileID.CANDELABRA:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.CANDELABRA);
						break;
					case EntityID.TileID.CRYSTAL_BALL:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.CRYSTAL_BALL);
						break;
					case EntityID.TileID.DISCO_BALL:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.DISCO_BALL);
						break;
					case EntityID.TileID.LEVER:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.LEVER);
						break;
					case EntityID.TileID.PUMP_IN:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.INLET_PUMP);
						break;
					case EntityID.TileID.PUMP_OUT:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.OUTLET_PUMP);
						break;
					case EntityID.TileID.BOULDER:
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							Projectile.NewProjectile(x2 * 16 + 15.5f, y2 * 16 + 16, 0f, 0f, 99, 70, 10f);
						}
						break;
				}
			}
			ToDestroyObject = false;
			bool flag = tileFrameRecursion;
			tileFrameRecursion = false;
			for (int m = x2 - 1; m < x2 + 3; m++)
			{
				for (int n = y2 - 1; n < y2 + 3; n++)
				{
					TileFrame(m, n);
				}
			}
			tileFrameRecursion = flag;
		}

		public static void Check2x2(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			int num3 = 0;
			num = -(Main.TileSet[i, j].FrameX / 18);
			num2 = -(Main.TileSet[i, j].FrameY / 18);
			if (num < -1)
			{
				num += 2;
				num3 = 36;
			}
			num += i;
			num2 += j;
			for (int k = num; k < num + 2; k++)
			{
				for (int l = num2; l < num2 + 2; l++)
				{
					if (Main.TileSet[k, l].IsActive == 0 || Main.TileSet[k, l].Type != type || Main.TileSet[k, l].FrameX != (k - num) * 18 + num3 || Main.TileSet[k, l].FrameY != (l - num2) * 18)
					{
						Destroy2x2(i, j, num, num2, type);
						return;
					}
				}
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.CHINESE_LANTERN:
					case EntityID.TileID.DISCO_BALL:
						if (Main.TileSet[k, num2 - 1].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[k, num2 - 1].Type])
						{
							Destroy2x2(i, j, num, num2, type);
							return;
						}
						break;
					default:
						if (Main.TileSet[k, num2 + 2].IsActive == 0 || (!Main.TileSolid[Main.TileSet[k, num2 + 2].Type] && !Main.TileTable[Main.TileSet[k, num2 + 2].Type]))
						{
							Destroy2x2(i, j, num, num2, type);
							return;
						}
						break;
					case EntityID.TileID.BOULDER:
						break;
				}
			}
			if (type == (byte)EntityID.TileID.BOULDER && !SolidTileUnsafe(num, num2 + 2) && !SolidTileUnsafe(num + 1, num2 + 2))
			{
				Destroy2x2(i, j, num, num2, type);
			}
		}

		public static void OreRunner(int i, int j, double strength, int steps, int type)
		{
			Vector2 vector = default;
			Vector2 vector2 = default;
			double num = strength;
			float num2 = steps;
			vector.X = i;
			vector.Y = j;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-10, 11) * 0.1f;
			while (num > 0.0 && num2 > 0f)
			{
				if (vector.Y < 0f && num2 > 0f && type == (int)EntityID.TileID.MUD)
				{
					num2 = 0f;
				}
				num = strength * (double)(num2 / steps);
				num2 -= 1f;
				int num3 = (int)(vector.X - num * 0.5);
				int num4 = (int)(vector.X + num * 0.5);
				int num5 = (int)(vector.Y - num * 0.5);
				int num6 = (int)(vector.Y + num * 0.5);
				if (num3 < 0)
				{
					num3 = 0;
				}
				if (num4 > Main.MaxTilesX)
				{
					num4 = Main.MaxTilesX;
				}
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesY)
				{
					num6 = Main.MaxTilesY;
				}
				for (int k = num3; k < num4; k++)
				{
					for (int l = num5; l < num6; l++)
					{
						if ((double)(Math.Abs(k - vector.X) + Math.Abs(l - vector.Y)) < strength * 0.5 * (double)(1f + genRand.Next(-10, 11) * 0.015f) && Main.TileSet[k, l].IsActive != 0 && EntityID.ReplaceableGrounds.Contains((EntityID.TileID)Main.TileSet[k, l].Type))
						{
							Main.TileSet[k, l].Type = (byte)type;
							SquareTileFrame(k, l);
							if (Main.NetMode == (byte)NetModeSetting.SERVER)
							{
								NetMessage.SendTile(k, l);
							}
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 1f)
				{
					vector2.X = 1f;
				}
				else if (vector2.X < -1f)
				{
					vector2.X = -1f;
				}
			}
		}

		public static void SmashAltar(int i, int j)
		{
			if (!Main.InHardMode || Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			int num = altarCount % 3;
			NetMessage.SendText(12 + num, 50, 255, 130, -1);
			int num2 = altarCount / 3 + 1;
			float num3 = Main.MaxTilesX / 4200f;
			int num4 = 1 - num;
			num3 = num3 * 310f - 85 * num;
			num3 *= 0.85f;
			num3 /= num2;
			switch (num)
			{
				case 0:
					num = (byte)EntityID.TileID.COBALT_ORE;
					num3 *= 1.05f;
					break;
				case 1:
					num = (byte)EntityID.TileID.MYTHRIL_ORE;
					break;
				default:
					num = (byte)EntityID.TileID.ADAMANTITE_ORE;
					break;
			}
			for (int k = 0; k < num3; k++)
			{
				int i2 = genRand.Next(100, Main.MaxTilesX - 100);
				int lowerBound = Main.WorldSurface;
				switch (num)
				{
					case (byte)EntityID.TileID.MYTHRIL_ORE:
						lowerBound = Main.RockLayer;
						break;
					case (byte)EntityID.TileID.ADAMANTITE_ORE:
						lowerBound = (Main.RockLayer + Main.RockLayer + Main.MaxTilesY) / 3;
						break;
				}
				int j2 = genRand.Next(lowerBound, Main.MaxTilesY - 150);
				OreRunner(i2, j2, genRand.Next(5, 9 + num4), genRand.Next(5, 9 + num4), num);
			}
			int num5 = genRand.Next(3);
			while (num5 != 2)
			{
				int num6 = genRand.Next(100, Main.MaxTilesX - 100);
				int num7 = genRand.Next(Main.RockLayer + 50, Main.MaxTilesY - 300);
				if (Main.TileSet[num6, num7].IsActive != 0 && Main.TileSet[num6, num7].Type == (byte)EntityID.TileID.STONE)
				{
					if (num5 == 0)
					{
						Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.EBONSTONE;
					}
					else
					{
						Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.PEARLSTONE;
					}
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.SendTile(num6, num7);
					}
					break;
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				int num8 = Main.Rand.Next(2) + 1;
				Rectangle rect = default;
				rect.X = i << 4;
				rect.Y = j << 4;
				rect.Width = (rect.Height = 16);
				for (int l = 0; l < num8; l++)
				{
					NPC.SpawnOnPlayer(Player.FindClosest(ref rect), (int)EntityID.NPCID.WRAITH);
				}
			}
			altarCount++;
		}

		public static void Check3x2(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			num -= Main.TileSet[i, j].FrameX / 18;
			num2 -= Main.TileSet[i, j].FrameY / 18;
			int num3 = num;
			while (true)
			{
				if (num3 >= num + 3) return;

				if (Main.TileSet[num3, num2 + 2].IsActive == 0 || !Main.TileSolid[Main.TileSet[num3, num2 + 2].Type])
				{
					break;
				}

				bool invalid = false;
				for (int k = num2; k < num2 + 2; k++)
				{
					if (Main.TileSet[num3, k].IsActive == 0 || Main.TileSet[num3, k].Type != type || Main.TileSet[num3, k].FrameX != (num3 - num) * 18 || Main.TileSet[num3, k].FrameY != (k - num2) * 18)
					{
						invalid = true;
						break;
					}
				}

				if (invalid) break;

				num3++;
			}
			ToDestroyObject = true;
			for (int l = num; l < num + 3; l++)
			{
				for (int m = num2; m < num2 + 3; m++)
				{
					if (Main.TileSet[l, m].Type == type && Main.TileSet[l, m].IsActive != 0)
					{
						KillTile(l, m);
					}
				}
			}
			if (!Gen)
			{
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.TABLE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.WOODEN_TABLE);
						break;
					case EntityID.TileID.TINKERERS_WORKSHOP:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.TINKERERS_WORKSHOP);
						break;
					case EntityID.TileID.DEMON_ALTAR:
						SmashAltar(i, j);
						break;
					case EntityID.TileID.FURNACE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.FURNACE);
						break;
					case EntityID.TileID.HELLFORGE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.HELLFORGE);
						break;
					case EntityID.TileID.LOOM:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.LOOM);
						break;
					case EntityID.TileID.PIANO:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.PIANO);
						break;
					case EntityID.TileID.DRESSER:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.DRESSER);
						break;
					case EntityID.TileID.BENCH:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.BENCH);
						break;
					case EntityID.TileID.ADAMANTITE_FORGE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.ADAMANTITE_FORGE);
						break;
#if VERSION_101
					case EntityID.TileID.CAMPFIRE:
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.CAMPFIRE);
						break;
#endif
				}
			}
			ToDestroyObject = false;
			bool flag = tileFrameRecursion;
			tileFrameRecursion = false;
			TileFrame(num - 1, num2 - 1);
			TileFrame(num, num2 - 1);
			TileFrame(num + 1, num2 - 1);
			TileFrame(num + 2, num2 - 1);
			TileFrame(num - 1, num2);
			TileFrame(num, num2);
			TileFrame(num + 1, num2);
			TileFrame(num + 2, num2);
			TileFrame(num - 1, num2 + 1);
			TileFrame(num, num2 + 1);
			TileFrame(num + 1, num2 + 1);
			TileFrame(num + 2, num2 + 1);
			TileFrame(num - 1, num2 + 2);
			TileFrame(num, num2 + 2);
			TileFrame(num + 1, num2 + 2);
			TileFrame(num + 2, num2 + 2);
			TileFrame(num - 1, num2 + 3);
			TileFrame(num, num2 + 3);
			TileFrame(num + 1, num2 + 3);
			TileFrame(num + 2, num2 + 3);
			tileFrameRecursion = flag;
		}

		public static void Check3x4(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			num -= Main.TileSet[i, j].FrameX / 18;
			num2 -= Main.TileSet[i, j].FrameY / 18;
			for (int k = num; k < num + 3; k++)
			{
				int num3 = num2;
				while (true)
				{
					if (num3 < num2 + 4)
					{
						if (Main.TileSet[k, num3].IsActive != 0 && Main.TileSet[k, num3].Type == type && Main.TileSet[k, num3].FrameX == (k - num) * 18 && Main.TileSet[k, num3].FrameY == (num3 - num2) * 18)
						{
							num3++;
							continue;
						}
					}
					else if (Main.TileSet[k, num2 + 4].IsActive != 0 && Main.TileSolid[Main.TileSet[k, num2 + 4].Type])
					{
						break;
					}
					ToDestroyObject = true;
					for (int l = num; l < num + 3; l++)
					{
						for (int m = num2; m < num2 + 4; m++)
						{
							if (Main.TileSet[l, m].Type == type && Main.TileSet[l, m].IsActive != 0)
							{
								KillTile(l, m);
							}
						}
					}
					if (!Gen)
					{
						switch ((EntityID.TileID)type)
						{
							case EntityID.TileID.BOOKCASE:
								Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.BOOKCASE);
								break;
							case EntityID.TileID.THRONE:
								Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.THRONE);
								break;
						}
					}
					ToDestroyObject = false;
					bool flag = tileFrameRecursion;
					tileFrameRecursion = false;
					for (int n = num - 1; n < num + 4; n++)
					{
						for (int num4 = num2 - 1; num4 < num2 + 4; num4++)
						{
							TileFrame(n, num4);
						}
					}
					tileFrameRecursion = flag;
					return;
				}
			}
		}

		public static bool Place4x2(int x, int y, int type, int direction = -1)
		{
			if (x < 5 || x > Main.MaxTilesX - 5 || y < 5 || y > Main.MaxTilesY - 5)
			{
				return false;
			}
			for (int i = x - 1; i < x + 3; i++)
			{
				if (Main.TileSet[i, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, y + 1].Type])
				{
					return false;
				}
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return false;
					}
				}
			}
			int num = ((direction == 1) ? 72 : 0);
			Main.TileSet[x - 1, y - 1].IsActive = 1;
			Main.TileSet[x - 1, y - 1].FrameY = 0;
			Main.TileSet[x - 1, y - 1].FrameX = (short)num;
			Main.TileSet[x - 1, y - 1].Type = (byte)type;
			Main.TileSet[x, y - 1].IsActive = 1;
			Main.TileSet[x, y - 1].FrameY = 0;
			Main.TileSet[x, y - 1].FrameX = (short)(18 + num);
			Main.TileSet[x, y - 1].Type = (byte)type;
			Main.TileSet[x + 1, y - 1].IsActive = 1;
			Main.TileSet[x + 1, y - 1].FrameY = 0;
			Main.TileSet[x + 1, y - 1].FrameX = (short)(36 + num);
			Main.TileSet[x + 1, y - 1].Type = (byte)type;
			Main.TileSet[x + 2, y - 1].IsActive = 1;
			Main.TileSet[x + 2, y - 1].FrameY = 0;
			Main.TileSet[x + 2, y - 1].FrameX = (short)(54 + num);
			Main.TileSet[x + 2, y - 1].Type = (byte)type;
			Main.TileSet[x - 1, y].IsActive = 1;
			Main.TileSet[x - 1, y].FrameY = 18;
			Main.TileSet[x - 1, y].FrameX = (short)num;
			Main.TileSet[x - 1, y].Type = (byte)type;
			Main.TileSet[x, y].IsActive = 1;
			Main.TileSet[x, y].FrameY = 18;
			Main.TileSet[x, y].FrameX = (short)(18 + num);
			Main.TileSet[x, y].Type = (byte)type;
			Main.TileSet[x + 1, y].IsActive = 1;
			Main.TileSet[x + 1, y].FrameY = 18;
			Main.TileSet[x + 1, y].FrameX = (short)(36 + num);
			Main.TileSet[x + 1, y].Type = (byte)type;
			Main.TileSet[x + 2, y].IsActive = 1;
			Main.TileSet[x + 2, y].FrameY = 18;
			Main.TileSet[x + 2, y].FrameX = (short)(54 + num);
			Main.TileSet[x + 2, y].Type = (byte)type;
			return true;
		}

		public static void SwitchMB(int i, int j)
		{
			int num = i;
			int num2 = j;
			int num3 = (Main.TileSet[i, j].FrameY / 18) & 1;
			int num4 = Main.TileSet[i, j].FrameX / 18;
			if (num4 >= 2)
			{
				num4 -= 2;
			}
			num = i - num4;
			num2 = j - num3;
			for (int k = num; k < num + 2; k++)
			{
				for (int l = num2; l < num2 + 2; l++)
				{
					if (Main.TileSet[k, l].IsActive != 0 && Main.TileSet[k, l].Type == (byte)EntityID.TileID.MUSIC_BOX)
					{
						if (Main.TileSet[k, l].FrameX < 36)
						{
							Main.TileSet[k, l].FrameX += 36;
						}
						else
						{
							Main.TileSet[k, l].FrameX -= 36;
						}
						noWire[numNoWire].X = (short)k;
						noWire[numNoWire].Y = (short)l;
						numNoWire++;
					}
				}
			}
			NetMessage.SendTileSquare(num, num2, 3);
		}

		public static void CheckMusicBox(int i, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			int num3 = Main.TileSet[i, j].FrameY / 18;
			int num4 = num3 >> 1;
			num3 &= 1;
			int num5 = Main.TileSet[i, j].FrameX / 18;
			int num6 = 0;
			if (num5 >= 2)
			{
				num5 -= 2;
				num6++;
			}
			num = i - num5;
			num2 = j - num3;
			for (int k = num; k < num + 2; k++)
			{
				int num7 = num2;
				while (true)
				{
					if (num7 < num2 + 2)
					{
						if (Main.TileSet[k, num7].IsActive != 0 && Main.TileSet[k, num7].Type == (byte)EntityID.TileID.MUSIC_BOX && Main.TileSet[k, num7].FrameX == (k - num) * 18 + num6 * 36 && Main.TileSet[k, num7].FrameY == (num7 - num2) * 18 + num4 * 36)
						{
							num7++;
							continue;
						}
					}
					else if (Main.TileSolid[Main.TileSet[k, num2 + 2].Type])
					{
						break;
					}
					ToDestroyObject = true;
					for (int l = num; l < num + 2; l++)
					{
						for (int m = num2; m < num2 + 3; m++)
						{
							if (Main.TileSet[l, m].Type == (byte)EntityID.TileID.MUSIC_BOX && Main.TileSet[l, m].IsActive != 0)
							{
								KillTile(l, m);
							}
						}
					}


					// Time for some history.
					// BUG: When you destroy the console-exclusive music boxes, they will drop the incorrect items upon destruction, unlike the original music boxes.
#if VERSION_103 || VERSION_FINAL
					// Now this is what it should look like so all music boxes drop the correct items, which should of happened in the 1.01 update but...
					if (num4 > 12)
					{
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MUSIC_BOX_DESERT - 13 + num4);
					}
					else 
					{
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MUSIC_BOX_OVERWORLD_DAY + num4);
					}
#elif VERSION_101
					// They fucked up and forgot to subtract the difference, which led to the space music box (the first of the exclusive ones) to give a campfire. Basically, no one at Engine did QA for this fix.
					if (num4 > 12)
					{
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MUSIC_BOX_DESERT + num4);
					}
					else
					{
						Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MUSIC_BOX_OVERWORLD_DAY + num4);
					}
#else
					// It was even more broken in the initial versions, since they didn't even account for the difference between music boxes, so you could get items 575-580 by breaking the exclusive ones.
					// This means Old-Gen Mobile wasn't the only one that had an easy way to get the best drill; An empty music box and an ocean visit = 1 free Hamdrax. 
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.MUSIC_BOX_OVERWORLD_DAY + num4);
#endif
					bool flag = tileFrameRecursion;
					tileFrameRecursion = false;
					for (int n = num - 1; n < num + 3; n++)
					{
						for (int num8 = num2 - 1; num8 < num2 + 3; num8++)
						{
							TileFrame(n, num8);
						}
					}
					tileFrameRecursion = flag;
					ToDestroyObject = false;
					return;
				}
			}
		}

		public static bool PlaceMB(int X, int y, int type, int style)
		{
			int num = X + 1;
			if (num < 5 || num > Main.MaxTilesX - 5 || y < 5 || y > Main.MaxTilesY - 5)
			{
				return false;
			}
			for (int i = num - 1; i < num + 1; i++)
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return false;
					}
				}
				if (Main.TileSet[i, y + 1].IsActive == 0 || (!Main.TileSolid[Main.TileSet[i, y + 1].Type] && !Main.TileTable[Main.TileSet[i, y + 1].Type]))
				{
					return false;
				}
			}
			Main.TileSet[num - 1, y - 1].IsActive = 1;
			Main.TileSet[num - 1, y - 1].FrameY = (short)(style * 36);
			Main.TileSet[num - 1, y - 1].FrameX = 0;
			Main.TileSet[num - 1, y - 1].Type = (byte)type;
			Main.TileSet[num, y - 1].IsActive = 1;
			Main.TileSet[num, y - 1].FrameY = (short)(style * 36);
			Main.TileSet[num, y - 1].FrameX = 18;
			Main.TileSet[num, y - 1].Type = (byte)type;
			Main.TileSet[num - 1, y].IsActive = 1;
			Main.TileSet[num - 1, y].FrameY = (short)(style * 36 + 18);
			Main.TileSet[num - 1, y].FrameX = 0;
			Main.TileSet[num - 1, y].Type = (byte)type;
			Main.TileSet[num, y].IsActive = 1;
			Main.TileSet[num, y].FrameY = (short)(style * 36 + 18);
			Main.TileSet[num, y].FrameX = 18;
			Main.TileSet[num, y].Type = (byte)type;
			return true;
		}

		public static bool Place2x2(int x, int y, int type)
		{
			if (type == (int)EntityID.TileID.CHINESE_LANTERN || type == (int)EntityID.TileID.DISCO_BALL)
			{
				y++;
			}
			if (x < 5 || x > Main.MaxTilesX - 5 || y < 5 || y > Main.MaxTilesY - 5)
			{
				return false;
			}
			for (int i = x - 1; i < x + 1; i++)
			{
				if (type == (int)EntityID.TileID.CHINESE_LANTERN || type == (int)EntityID.TileID.DISCO_BALL)
				{
					if (Main.TileSet[i, y - 2].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[i, y - 2].Type])
					{
						return false;
					}
				}
				else if (Main.TileSet[i, y + 1].IsActive == 0 || (!Main.TileSolid[Main.TileSet[i, y + 1].Type] && !Main.TileTable[Main.TileSet[i, y + 1].Type]))
				{
					return false;
				}
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0 || (type == (int)EntityID.TileID.SKULL_LANTERN && Main.TileSet[i, j].Liquid > 0))
					{
						return false;
					}
				}
			}
			Main.TileSet[x - 1, y - 1].IsActive = 1;
			Main.TileSet[x - 1, y - 1].FrameY = 0;
			Main.TileSet[x - 1, y - 1].FrameX = 0;
			Main.TileSet[x - 1, y - 1].Type = (byte)type;
			Main.TileSet[x, y - 1].IsActive = 1;
			Main.TileSet[x, y - 1].FrameY = 0;
			Main.TileSet[x, y - 1].FrameX = 18;
			Main.TileSet[x, y - 1].Type = (byte)type;
			Main.TileSet[x - 1, y].IsActive = 1;
			Main.TileSet[x - 1, y].FrameY = 18;
			Main.TileSet[x - 1, y].FrameX = 0;
			Main.TileSet[x - 1, y].Type = (byte)type;
			Main.TileSet[x, y].IsActive = 1;
			Main.TileSet[x, y].FrameY = 18;
			Main.TileSet[x, y].FrameX = 18;
			Main.TileSet[x, y].Type = (byte)type;
			return true;
		}

		public static bool Place3x4(int x, int y, int type)
		{
			if (x < 5 || x > Main.MaxTilesX - 5 || y < 5 || y > Main.MaxTilesY - 5)
			{
				return false;
			}
			for (int i = x - 1; i < x + 2; i++)
			{
				if (Main.TileSet[i, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, y + 1].Type])
				{
					return false;
				}
				for (int j = y - 3; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return false;
					}
				}
			}
			for (int k = -3; k <= 0; k++)
			{
				short frameY = (short)((3 + k) * 18);
				Main.TileSet[x - 1, y + k].IsActive = 1;
				Main.TileSet[x - 1, y + k].FrameY = frameY;
				Main.TileSet[x - 1, y + k].FrameX = 0;
				Main.TileSet[x - 1, y + k].Type = (byte)type;
				Main.TileSet[x, y + k].IsActive = 1;
				Main.TileSet[x, y + k].FrameY = frameY;
				Main.TileSet[x, y + k].FrameX = 18;
				Main.TileSet[x, y + k].Type = (byte)type;
				Main.TileSet[x + 1, y + k].IsActive = 1;
				Main.TileSet[x + 1, y + k].FrameY = frameY;
				Main.TileSet[x + 1, y + k].FrameX = 36;
				Main.TileSet[x + 1, y + k].Type = (byte)type;
			}
			return true;
		}

		public static bool Place3x2(int x, int y, int type)
		{
			if (x < 5 || x > Main.MaxTilesX - 5 || y < 5 || y > Main.MaxTilesY - 5)
			{
				return false;
			}
			for (int i = x - 1; i < x + 2; i++)
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return false;
					}
				}
				if (Main.TileSet[i, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, y + 1].Type])
				{
					return false;
				}
			}
			Main.TileSet[x - 1, y - 1].IsActive = 1;
			Main.TileSet[x - 1, y - 1].FrameY = 0;
			Main.TileSet[x - 1, y - 1].FrameX = 0;
			Main.TileSet[x - 1, y - 1].Type = (byte)type;
			Main.TileSet[x, y - 1].IsActive = 1;
			Main.TileSet[x, y - 1].FrameY = 0;
			Main.TileSet[x, y - 1].FrameX = 18;
			Main.TileSet[x, y - 1].Type = (byte)type;
			Main.TileSet[x + 1, y - 1].IsActive = 1;
			Main.TileSet[x + 1, y - 1].FrameY = 0;
			Main.TileSet[x + 1, y - 1].FrameX = 36;
			Main.TileSet[x + 1, y - 1].Type = (byte)type;
			Main.TileSet[x - 1, y].IsActive = 1;
			Main.TileSet[x - 1, y].FrameY = 18;
			Main.TileSet[x - 1, y].FrameX = 0;
			Main.TileSet[x - 1, y].Type = (byte)type;
			Main.TileSet[x, y].IsActive = 1;
			Main.TileSet[x, y].FrameY = 18;
			Main.TileSet[x, y].FrameX = 18;
			Main.TileSet[x, y].Type = (byte)type;
			Main.TileSet[x + 1, y].IsActive = 1;
			Main.TileSet[x + 1, y].FrameY = 18;
			Main.TileSet[x + 1, y].FrameX = 36;
			Main.TileSet[x + 1, y].Type = (byte)type;
			return true;
		}

		public static void Check3x3(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			bool flag = false;
			int num = i;
			int num2 = j;
			num = Main.TileSet[i, j].FrameX / 18;
			int num3 = i - num;
			if (num >= 3)
			{
				num -= 3;
			}
			num = i - num;
			num2 += Main.TileSet[i, j].FrameY / 18 * -1;

			for (int k = num; k < num + 3; k++)
			{
				for (int l = num2; l < num2 + 3; l++)
				{
					if (Main.TileSet[k, l].IsActive == 0 || Main.TileSet[k, l].Type != type || Main.TileSet[k, l].FrameX != (k - num3) * 18 || Main.TileSet[k, l].FrameY != (l - num2) * 18)
					{
						flag = true;
					}
				}
			}
			if (type == (int)EntityID.TileID.SAWMILL)
			{
				for (int m = num; m < num + 3; m++)
				{
					if (Main.TileSet[m, num2 + 3].IsActive == 0 || !Main.TileSolid[Main.TileSet[m, num2 + 3].Type])
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				if (Main.TileSet[num + 1, num2 - 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num + 1, num2 - 1].Type] || Main.TileSolidTop[Main.TileSet[num + 1, num2 - 1].Type])
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}

			ToDestroyObject = true;
			for (int l = num; l < num + 3; l++)
			{
				for (int m = num2; m < num2 + 3; m++)
				{
					if (Main.TileSet[l, m].Type == type && Main.TileSet[l, m].IsActive != 0)
					{
						KillTile(l, m);
					}
				}
			}
			switch ((EntityID.TileID)type)
			{
				case EntityID.TileID.CHANDELIER:
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.COPPER_CHANDELIER);
					break;
				case EntityID.TileID.JACK_O_LANTERN:
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.SILVER_CHANDELIER);
					break;
				case EntityID.TileID.PRESENT:
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.GOLD_CHANDELIER);
					break;
				case EntityID.TileID.SAWMILL:
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.SAWMILL);
					break;
			}
			ToDestroyObject = false;
			bool flag2 = tileFrameRecursion;
			tileFrameRecursion = false;
			for (int n = num - 1; n < num + 4; n++)
			{
				for (int num6 = num2 - 1; num6 < num2 + 4; num6++)
				{
					TileFrame(n, num6);
				}
			}
			tileFrameRecursion = flag2;
		}

		public static bool Place3x3(int x, int y, int type)
		{
			int num = 0;
			if (type == (int)EntityID.TileID.SAWMILL)
			{
				num = -2;
				for (int i = x - 1; i < x + 2; i++)
				{
					for (int j = y - 2; j < y + 1; j++)
					{
						if (Main.TileSet[i, j].IsActive != 0)
						{
							return false;
						}
					}
				}
				for (int k = x - 1; k < x + 2; k++)
				{
					if (Main.TileSet[k, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[k, y + 1].Type])
					{
						return false;
					}
				}
			}
			else
			{
				for (int l = x - 1; l < x + 2; l++)
				{
					for (int m = y; m < y + 3; m++)
					{
						if (Main.TileSet[l, m].IsActive != 0)
						{
							return false;
						}
					}
				}
				if (Main.TileSet[x, y - 1].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[x, y - 1].Type])
				{
					return false;
				}
			}
			Main.TileSet[x - 1, y + num].IsActive = 1;
			Main.TileSet[x - 1, y + num].FrameY = 0;
			Main.TileSet[x - 1, y + num].FrameX = 0;
			Main.TileSet[x - 1, y + num].Type = (byte)type;
			Main.TileSet[x, y + num].IsActive = 1;
			Main.TileSet[x, y + num].FrameY = 0;
			Main.TileSet[x, y + num].FrameX = 18;
			Main.TileSet[x, y + num].Type = (byte)type;
			Main.TileSet[x + 1, y + num].IsActive = 1;
			Main.TileSet[x + 1, y + num].FrameY = 0;
			Main.TileSet[x + 1, y + num].FrameX = 36;
			Main.TileSet[x + 1, y + num].Type = (byte)type;
			Main.TileSet[x - 1, y + 1 + num].IsActive = 1;
			Main.TileSet[x - 1, y + 1 + num].FrameY = 18;
			Main.TileSet[x - 1, y + 1 + num].FrameX = 0;
			Main.TileSet[x - 1, y + 1 + num].Type = (byte)type;
			Main.TileSet[x, y + 1 + num].IsActive = 1;
			Main.TileSet[x, y + 1 + num].FrameY = 18;
			Main.TileSet[x, y + 1 + num].FrameX = 18;
			Main.TileSet[x, y + 1 + num].Type = (byte)type;
			Main.TileSet[x + 1, y + 1 + num].IsActive = 1;
			Main.TileSet[x + 1, y + 1 + num].FrameY = 18;
			Main.TileSet[x + 1, y + 1 + num].FrameX = 36;
			Main.TileSet[x + 1, y + 1 + num].Type = (byte)type;
			Main.TileSet[x - 1, y + 2 + num].IsActive = 1;
			Main.TileSet[x - 1, y + 2 + num].FrameY = 36;
			Main.TileSet[x - 1, y + 2 + num].FrameX = 0;
			Main.TileSet[x - 1, y + 2 + num].Type = (byte)type;
			Main.TileSet[x, y + 2 + num].IsActive = 1;
			Main.TileSet[x, y + 2 + num].FrameY = 36;
			Main.TileSet[x, y + 2 + num].FrameX = 18;
			Main.TileSet[x, y + 2 + num].Type = (byte)type;
			Main.TileSet[x + 1, y + 2 + num].IsActive = 1;
			Main.TileSet[x + 1, y + 2 + num].FrameY = 36;
			Main.TileSet[x + 1, y + 2 + num].FrameX = 36;
			Main.TileSet[x + 1, y + 2 + num].Type = (byte)type;
			return true;
		}

		public static bool PlaceSunflower(int x, int y)
		{
			if (y > Main.WorldSurface - 1)
			{
				return false;
			}
			for (int i = x; i < x + 2; i++)
			{
				for (int j = y - 3; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0 || Main.TileSet[i, j].WallType > (byte)EntityID.WallID.NONE)
					{
						return false;
					}
				}
				if (Main.TileSet[i, y + 1].IsActive == 0 || (Main.TileSet[i, y + 1].Type != (byte)EntityID.TileID.GRASS && Main.TileSet[i, y + 1].Type != (byte)EntityID.TileID.HALLOWED_GRASS))
				{
					return false;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				for (int l = -3; l < 1; l++)
				{
					int num = k * 18 + genRand.Next(3) * 36;
					int num2 = (l + 3) * 18;
					Main.TileSet[x + k, y + l].IsActive = 1;
					Main.TileSet[x + k, y + l].FrameX = (short)num;
					Main.TileSet[x + k, y + l].FrameY = (short)num2;
					Main.TileSet[x + k, y + l].Type = (byte)EntityID.TileID.SUNFLOWER;
				}
			}
			return true;
		}

		public static void CheckSunflower(int i, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = 0;
			int num2 = j;
			num += Main.TileSet[i, j].FrameX / 18;
			num2 -= Main.TileSet[i, j].FrameY / 18;
			num &= 1;
			num = -num;
			num += i;
			for (int k = num; k < num + 2; k++)
			{
				int num3 = num2;
				while (true)
				{
					if (num3 < num2 + 4)
					{
						int num4 = (Main.TileSet[k, num3].FrameX / 18) & 1;
						if (Main.TileSet[k, num3].IsActive != 0 && Main.TileSet[k, num3].Type == (byte)EntityID.TileID.SUNFLOWER && num4 == k - num && Main.TileSet[k, num3].FrameY == (num3 - num2) * 18)
						{
							num3++;
							continue;
						}
					}
					else if (Main.TileSet[k, num2 + 4].IsActive != 0 && (Main.TileSet[k, num2 + 4].Type == (byte)EntityID.TileID.GRASS || Main.TileSet[k, num2 + 4].Type == (byte)EntityID.TileID.HALLOWED_GRASS))
					{
						break;
					}
					ToDestroyObject = true;
					for (int l = num; l < num + 2; l++)
					{
						for (int m = num2; m < num2 + 4; m++)
						{
							if (Main.TileSet[l, m].Type == (byte)EntityID.TileID.SUNFLOWER && Main.TileSet[l, m].IsActive != 0)
							{
								KillTile(l, m);
							}
						}
					}
					Item.NewItem(i * 16, j * 16, 32, 32, (int)EntityID.ItemID.SUNFLOWER);
					ToDestroyObject = false;
					return;
				}
			}
		}

		public static bool PlacePot(int x, int y)
		{
			for (int i = x; i < x + 2; i++)
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return false;
					}
				}
				if (Main.TileSet[i, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, y + 1].Type])
				{
					return false;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				for (int l = -1; l < 1; l++)
				{
					Main.TileSet[x + k, y + l].IsActive = 1;
					Main.TileSet[x + k, y + l].FrameX = (short)(k * 18 + genRand.Next(3) * 36);
					Main.TileSet[x + k, y + l].FrameY = (short)((l + 1) * 18);
					Main.TileSet[x + k, y + l].Type = (byte)EntityID.TileID.POT;
				}
			}
			return true;
		}

		public static bool CheckCactus(int i, int j)
		{
			int num = j;
			int num2 = i;
			while (Main.TileSet[num2, num].IsActive != 0 && Main.TileSet[num2, num].Type == (byte)EntityID.TileID.CACTUS)
			{
				num++;
				if (Main.TileSet[num2, num].IsActive == 0 || Main.TileSet[num2, num].Type != (byte)EntityID.TileID.CACTUS)
				{
					if (Main.TileSet[num2 - 1, num].IsActive != 0 && Main.TileSet[num2 - 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 - 1, num - 1].IsActive != 0 && Main.TileSet[num2 - 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 >= i)
					{
						num2--;
					}
					if (Main.TileSet[num2 + 1, num].IsActive != 0 && Main.TileSet[num2 + 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 + 1, num - 1].IsActive != 0 && Main.TileSet[num2 + 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 <= i)
					{
						num2++;
					}
				}
			}
			if (Main.TileSet[num2, num].IsActive == 0 || (Main.TileSet[num2, num].Type != (byte)EntityID.TileID.SAND && Main.TileSet[num2, num].Type != (byte)EntityID.TileID.EBONSAND && Main.TileSet[num2, num].Type != (byte)EntityID.TileID.PEARLSAND))
			{
				KillTile(i, j);
				return true;
			}
			if (i != num2)
			{
				if ((Main.TileSet[i, j + 1].IsActive == 0 || Main.TileSet[i, j + 1].Type != (byte)EntityID.TileID.CACTUS) && (Main.TileSet[i - 1, j].IsActive == 0 || Main.TileSet[i - 1, j].Type != (byte)EntityID.TileID.CACTUS) && (Main.TileSet[i + 1, j].IsActive == 0 || Main.TileSet[i + 1, j].Type != (byte)EntityID.TileID.CACTUS))
				{
					KillTile(i, j);
					return true;
				}
			}
			else if (i == num2 && (Main.TileSet[i, j + 1].IsActive == 0 || (Main.TileSet[i, j + 1].Type != (byte)EntityID.TileID.CACTUS && Main.TileSet[i, j + 1].Type != (byte)EntityID.TileID.SAND && Main.TileSet[i, j + 1].Type != (byte)EntityID.TileID.EBONSAND && Main.TileSet[i, j + 1].Type != (byte)EntityID.TileID.PEARLSAND)))
			{
				KillTile(i, j);
				return true;
			}
			return false;
		}

		public static void PlantCactus(int i, int j)
		{
			GrowCactus(i, j);
			for (int k = 0; k < 150; k++)
			{
				int i2 = genRand.Next(i - 1, i + 2);
				int j2 = genRand.Next(j - 10, j + 2);
				GrowCactus(i2, j2);
			}
		}

		public static void CheckOrb(int i, int j, int type)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = i;
			int num2 = j;
			num = ((Main.TileSet[i, j].FrameX != 0) ? (i - 1) : i);
			num2 = ((Main.TileSet[i, j].FrameY != 0) ? (j - 1) : j);
			if (Main.TileSet[num, num2].IsActive != 0 && Main.TileSet[num, num2].Type == type && Main.TileSet[num + 1, num2].IsActive != 0 && Main.TileSet[num + 1, num2].Type == type && Main.TileSet[num, num2 + 1].IsActive != 0 && Main.TileSet[num, num2 + 1].Type == type && Main.TileSet[num + 1, num2 + 1].IsActive != 0 && Main.TileSet[num + 1, num2 + 1].Type == type)
			{
				return;
			}
			ToDestroyObject = true;
			if (Main.TileSet[num, num2].Type == type)
			{
				KillTile(num, num2);
			}
			if (Main.TileSet[num + 1, num2].Type == type)
			{
				KillTile(num + 1, num2);
			}
			if (Main.TileSet[num, num2 + 1].Type == type)
			{
				KillTile(num, num2 + 1);
			}
			if (Main.TileSet[num + 1, num2 + 1].Type == type)
			{
				KillTile(num + 1, num2 + 1);
			}
			if (!Gen)
			{
				Main.PlaySound(13, i * 16, j * 16);
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.LIFE_CRYSTAL:
							Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.LIFE_CRYSTAL);
							break;
						case EntityID.TileID.SHADOW_ORB:
							{
								if (genRand.Next(2) == 0)
								{
									ToSpawnMeteor = true;
								}
								int num3 = Main.Rand.Next(5);
								if (!HasShadowOrbSmashed)
								{
									num3 = 0;
								}
								switch (num3)
								{
									case 0:
										{
											Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.MUSKET, 1, DoNotBroadcast: false, -1);
											Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.MUSKET_BALL, genRand.Next(25, 51));
											break;
										}
									case 1:
										Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.VILETHORN, 1, DoNotBroadcast: false, -1);
										break;
									case 2:
										Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.BALL_O_HURT, 1, DoNotBroadcast: false, -1);
										break;
									case 3:
										Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.SHADOW_ORB, 1, DoNotBroadcast: false, -1);
										break;
									case 4:
										Item.NewItem(num * 16, num2 * 16, 32, 32, (int)EntityID.ItemID.BAND_OF_STARPOWER, 1, DoNotBroadcast: false, -1);
										break;
								}
								HasShadowOrbSmashed = true;
								shadowOrbCount++;
								if (shadowOrbCount >= 3)
								{
									shadowOrbCount = 0;
									Rectangle rect = default;
									rect.X = num << 4;
									rect.Y = num2 << 4;
									rect.Width = (rect.Height = 0);
									NPC.SpawnOnPlayer(Player.FindClosest(ref rect), (int)EntityID.NPCID.EATER_OF_WORLDS_HEAD);
								}
								else
								{
									int textId = 10;
									if (shadowOrbCount == 2)
									{
										textId = 11;
									}
									NetMessage.SendText(textId, 50, 255, 130, -1);
								}
								break;
							}
					}
				}
			}
			ToDestroyObject = false;
		}

		public static void CheckTree(int i, int j)
		{
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			if (Main.TileSet[i - 1, j].IsActive != 0)
			{
				num2 = Main.TileSet[i - 1, j].Type;
			}
			if (Main.TileSet[i + 1, j].IsActive != 0)
			{
				num3 = Main.TileSet[i + 1, j].Type;
			}
			if (Main.TileSet[i, j - 1].IsActive != 0)
			{
				num = Main.TileSet[i, j - 1].Type;
			}
			if (Main.TileSet[i, j + 1].IsActive != 0)
			{
				num4 = Main.TileSet[i, j + 1].Type;
			}
			if (num2 >= 0 && Main.TileStone[num2])
			{
				num2 = (int)EntityID.TileID.STONE;
			}
			if (num3 >= 0 && Main.TileStone[num3])
			{
				num3 = (int)EntityID.TileID.STONE;
			}
			if (num >= 0 && Main.TileStone[num])
			{
				num = (int)EntityID.TileID.STONE;
			}
			if (num4 >= 0 && Main.TileStone[num4])
			{
				num4 = (int)EntityID.TileID.STONE;
			}
			switch ((EntityID.TileID)num4)
			{
				case EntityID.TileID.CORRUPT_GRASS:
					num4 = 2;
					break;
				case EntityID.TileID.JUNGLE_GRASS:
					num4 = 2;
					break;
				case EntityID.TileID.HALLOWED_GRASS:
					num4 = 2;
					break;
				case EntityID.TileID.SNOW:
					num4 = 2;
					break;
			}
			int frameNumber = Main.TileSet[i, j].frameNumber;
			int type = Main.TileSet[i, j].Type;
			int frameX;
			int num5 = (frameX = Main.TileSet[i, j].FrameX);
			int frameY;
			int num6 = (frameY = Main.TileSet[i, j].FrameY);
			if (frameX >= 22 && frameX <= 44 && frameY >= 132 && frameY <= 176)
			{
				if (num4 != 2)
				{
					KillTile(i, j);
				}
				else if ((frameX != 22 || num2 != type) && (frameX != 44 || num3 != type))
				{
					KillTile(i, j);
				}
			}
			else if ((frameX == 88 && frameY >= 0 && frameY <= 44) || (frameX == 66 && frameY >= 66 && frameY <= 130) || (frameX == 110 && frameY >= 66 && frameY <= 110) || (frameX == 132 && frameY >= 0 && frameY <= 176))
			{
				if (num2 == type && num3 == type)
				{
					Main.TileSet[i, j].FrameX = 110;
					Main.TileSet[i, j].FrameY = (short)(66 + 22 * frameNumber);
				}
				else if (num2 == type)
				{
					Main.TileSet[i, j].FrameX = 88;
					Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
				}
				else if (num3 == type)
				{
					Main.TileSet[i, j].FrameX = 66;
					Main.TileSet[i, j].FrameY = (short)(66 + 22 * frameNumber);
				}
				else
				{
					Main.TileSet[i, j].FrameX = 0;
					Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
				}
			}
			frameX = Main.TileSet[i, j].FrameX;
			frameY = Main.TileSet[i, j].FrameY;
			if (frameY >= 132 && frameY <= 176)
			{
				if (frameX == 0 || frameX == 66 || frameX == 88)
				{
					if (num4 != 2)
					{
						KillTile(i, j);
					}
					if (num2 != type && num3 != type)
					{
						Main.TileSet[i, j].FrameX = 0;
						Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
					}
					else if (num2 != type)
					{
						Main.TileSet[i, j].FrameX = 0;
						Main.TileSet[i, j].FrameY = (short)(132 + 22 * frameNumber);
					}
					else if (num3 != type)
					{
						Main.TileSet[i, j].FrameX = 66;
						Main.TileSet[i, j].FrameY = (short)(132 + 22 * frameNumber);
					}
					else
					{
						Main.TileSet[i, j].FrameX = 88;
						Main.TileSet[i, j].FrameY = (short)(132 + 22 * frameNumber);
					}
				}
			}
			else if ((frameX == 66 && (frameY == 0 || frameY == 22 || frameY == 44)) || (frameX == 44 && (frameY == 198 || frameY == 220 || frameY == 242)))
			{
				if (num3 != type)
				{
					KillTile(i, j);
				}
			}
			else if ((frameX == 88 && (frameY == 66 || frameY == 88 || frameY == 110)) || (frameX == 66 && (frameY == 198 || frameY == 220 || frameY == 242)))
			{
				if (num2 != type)
				{
					KillTile(i, j);
				}
			}
			else if (num4 == (int)EntityID.TileID.NONE || num4 == (int)EntityID.TileID.CORRUPT_GRASS)
			{
				KillTile(i, j);
			}
			else if (num != type && frameY < 198 && ((frameX != 22 && frameX != 44) || frameY < 132))
			{
				if (num2 == type || num3 == type)
				{
					if (num4 == type)
					{
						if (num2 == type && num3 == type)
						{
							Main.TileSet[i, j].FrameX = 132;
							Main.TileSet[i, j].FrameY = (short)(132 + 22 * frameNumber);
						}
						else if (num2 == type)
						{
							Main.TileSet[i, j].FrameX = 132;
							Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
						}
						else if (num3 == type)
						{
							Main.TileSet[i, j].FrameX = 132;
							Main.TileSet[i, j].FrameY = (short)(66 + 22 * frameNumber);
						}
					}
					else if (num2 == type && num3 == type)
					{
						Main.TileSet[i, j].FrameX = 154;
						Main.TileSet[i, j].FrameY = (short)(132 + 22 * frameNumber);
					}
					else if (num2 == type)
					{
						Main.TileSet[i, j].FrameX = 154;
						Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
					}
					else if (num3 == type)
					{
						Main.TileSet[i, j].FrameX = 154;
						Main.TileSet[i, j].FrameY = (short)(66 + 22 * frameNumber);
					}
				}
				else
				{
					Main.TileSet[i, j].FrameX = 110;
					Main.TileSet[i, j].FrameY = (short)(22 * frameNumber);
				}
			}
			if (num5 >= 0 && num6 >= 0 && Main.TileSet[i, j].FrameX != num5 && Main.TileSet[i, j].FrameY != num6)
			{
				TileFrame(i - 1, j);
				TileFrame(i + 1, j);
				TileFrame(i, j - 1);
				TileFrame(i, j + 1);
			}
		}

		public static void CactusFrame(int i, int j)
		{
			try
			{
				int num = j;
				int num2 = i;
				if (CheckCactus(i, j))
				{
					return;
				}
				while (Main.TileSet[num2, num].IsActive != 0 && Main.TileSet[num2, num].Type == (byte)EntityID.TileID.CACTUS)
				{
					num++;
					if (Main.TileSet[num2, num].IsActive == 0 || Main.TileSet[num2, num].Type != (byte)EntityID.TileID.CACTUS)
					{
						if (Main.TileSet[num2 - 1, num].IsActive != 0 && Main.TileSet[num2 - 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 - 1, num - 1].IsActive != 0 && Main.TileSet[num2 - 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 >= i)
						{
							num2--;
						}
						if (Main.TileSet[num2 + 1, num].IsActive != 0 && Main.TileSet[num2 + 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 + 1, num - 1].IsActive != 0 && Main.TileSet[num2 + 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 <= i)
						{
							num2++;
						}
					}
				}
				num--;
				int num3 = i - num2;
				num2 = i;
				num = j;
				int type = Main.TileSet[i - 2, j].Type;
				int num4 = Main.TileSet[i - 1, j].Type;
				int num5 = Main.TileSet[i + 1, j].Type;
				int num6 = Main.TileSet[i, j - 1].Type;
				int num7 = Main.TileSet[i, j + 1].Type;
				int num8 = Main.TileSet[i - 1, j + 1].Type;
				int num9 = Main.TileSet[i + 1, j + 1].Type;
				if (Main.TileSet[i - 1, j].IsActive == 0)
				{
					num4 = (int)EntityID.TileID.NONE;
				}
				if (Main.TileSet[i + 1, j].IsActive == 0)
				{
					num5 = (int)EntityID.TileID.NONE;
				}
				if (Main.TileSet[i, j - 1].IsActive == 0)
				{
					num6 = (int)EntityID.TileID.NONE;
				}
				if (Main.TileSet[i, j + 1].IsActive == 0)
				{
					num7 = (int)EntityID.TileID.NONE;
				}
				if (Main.TileSet[i - 1, j + 1].IsActive == 0)
				{
					num8 = (int)EntityID.TileID.NONE;
				}
				if (Main.TileSet[i + 1, j + 1].IsActive == 0)
				{
					num9 = (int)EntityID.TileID.NONE;
				}
				short num10 = Main.TileSet[i, j].FrameX;
				short num11 = Main.TileSet[i, j].FrameY;
				switch (num3)
				{
					case 0:
						if (num6 != 80)
						{
							if (num4 == 80 && num5 == 80 && num8 != 80 && num9 != 80 && type != 80)
							{
								num10 = 90;
								num11 = 0;
							}
							else if (num4 == 80 && num8 != 80 && type != 80)
							{
								num10 = 72;
								num11 = 0;
							}
							else if (num5 == 80 && num9 != 80)
							{
								num10 = 18;
								num11 = 0;
							}
							else
							{
								num10 = 0;
								num11 = 0;
							}
						}
						else if (num4 == 80 && num5 == 80 && num8 != 80 && num9 != 80 && type != 80)
						{
							num10 = 90;
							num11 = 36;
						}
						else if (num4 == 80 && num8 != 80 && type != 80)
						{
							num10 = 72;
							num11 = 36;
						}
						else if (num5 == 80 && num9 != 80)
						{
							num10 = 18;
							num11 = 36;
						}
						else if (num7 >= 0 && Main.TileSolid[num7])
						{
							num10 = 0;
							num11 = 36;
						}
						else
						{
							num10 = 0;
							num11 = 18;
						}
						break;
					case -1:
						if (num5 == 80)
						{
							if (num6 != 80 && num7 != 80)
							{
								num10 = 108;
								num11 = 36;
							}
							else if (num7 != 80)
							{
								num10 = 54;
								num11 = 36;
							}
							else if (num6 != 80)
							{
								num10 = 54;
								num11 = 0;
							}
							else
							{
								num10 = 54;
								num11 = 18;
							}
						}
						else if (num6 != 80)
						{
							num10 = 54;
							num11 = 0;
						}
						else
						{
							num10 = 54;
							num11 = 18;
						}
						break;
					case 1:
						if (num4 == 80)
						{
							if (num6 != 80 && num7 != 80)
							{
								num10 = 108;
								num11 = 16;
							}
							else if (num7 != 80)
							{
								num10 = 36;
								num11 = 36;
							}
							else if (num6 != 80)
							{
								num10 = 36;
								num11 = 0;
							}
							else
							{
								num10 = 36;
								num11 = 18;
							}
						}
						else if (num6 != 80)
						{
							num10 = 36;
							num11 = 0;
						}
						else
						{
							num10 = 36;
							num11 = 18;
						}
						break;
				}
				if (num10 != Main.TileSet[i, j].FrameX || num11 != Main.TileSet[i, j].FrameY)
				{
					Main.TileSet[i, j].FrameX = num10;
					Main.TileSet[i, j].FrameY = num11;
					SquareTileFrame(i, j);
				}
			}
			catch
			{
				Main.TileSet[i, j].FrameX = 0;
				Main.TileSet[i, j].FrameY = 0;
			}
		}

		public static void GrowCactus(int i, int j)
		{
			int num = j;
			int num2 = i;
			if (Main.TileSet[i, j].IsActive == 0 || Main.TileSet[i, j - 1].Liquid > 0 || (Main.TileSet[i, j].Type != (byte)EntityID.TileID.SAND && Main.TileSet[i, j].Type != (byte)EntityID.TileID.CACTUS && Main.TileSet[i, j].Type != (byte)EntityID.TileID.EBONSAND && Main.TileSet[i, j].Type != (byte)EntityID.TileID.PEARLSAND))
			{
				return;
			}
			if (Main.TileSet[i, j].Type == (byte)EntityID.TileID.SAND || Main.TileSet[i, j].Type == (byte)EntityID.TileID.EBONSAND || Main.TileSet[i, j].Type == (byte)EntityID.TileID.PEARLSAND)
			{
				if (Main.TileSet[i, j - 1].IsActive != 0 || Main.TileSet[i - 1, j - 1].IsActive != 0 || Main.TileSet[i + 1, j - 1].IsActive != 0)
				{
					return;
				}
				int num3 = 0;
				int num4 = 0;
				for (int k = i - 6; k <= i + 6; k++)
				{
					for (int l = j - 3; l <= j + 1; l++)
					{
						try
						{
							if (Main.TileSet[k, l].IsActive == 0)
							{
								continue;
							}
							if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.CACTUS)
							{
								num3++;
								if (num3 >= 4)
								{
									return;
								}
							}
							if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.SAND || Main.TileSet[k, l].Type == (byte)EntityID.TileID.EBONSAND || Main.TileSet[k, l].Type == (byte)EntityID.TileID.PEARLSAND)
							{
								num4++;
							}
						}
						catch
						{
						}
					}
				}
				if (num4 > 10)
				{
					Main.TileSet[i, j - 1].IsActive = 1;
					Main.TileSet[i, j - 1].Type = (byte)EntityID.TileID.CACTUS;
					SquareTileFrame(num2, num - 1);
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.SendTile(i, j - 1);
					}
				}
			}
			else
			{
				if (Main.TileSet[i, j].Type != (byte)EntityID.TileID.CACTUS)
				{
					return;
				}
				while (Main.TileSet[num2, num].IsActive != 0 && Main.TileSet[num2, num].Type == (byte)EntityID.TileID.CACTUS)
				{
					num++;
					if (Main.TileSet[num2, num].IsActive == 0 || Main.TileSet[num2, num].Type != (byte)EntityID.TileID.CACTUS)
					{
						if (Main.TileSet[num2 - 1, num].IsActive != 0 && Main.TileSet[num2 - 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 - 1, num - 1].IsActive != 0 && Main.TileSet[num2 - 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 >= i)
						{
							num2--;
						}
						if (Main.TileSet[num2 + 1, num].IsActive != 0 && Main.TileSet[num2 + 1, num].Type == (byte)EntityID.TileID.CACTUS && Main.TileSet[num2 + 1, num - 1].IsActive != 0 && Main.TileSet[num2 + 1, num - 1].Type == (byte)EntityID.TileID.CACTUS && num2 <= i)
						{
							num2++;
						}
					}
				}
				num--;
				int num5 = num - j;
				int num6 = i - num2;
				num2 = i - num6;
				num = j;
				int num7 = 11 - num5;
				int num8 = 0;
				for (int m = num2 - 2; m <= num2 + 2; m++)
				{
					for (int n = num - num7; n <= num + num5; n++)
					{
						if (Main.TileSet[m, n].IsActive != 0 && Main.TileSet[m, n].Type == (byte)EntityID.TileID.CACTUS)
						{
							num8++;
						}
					}
				}
				if (num8 >= genRand.Next(11, 13))
				{
					return;
				}
				num2 = i;
				num = j;
				if (num6 == 0)
				{
					if (num5 == 0)
					{
						if (Main.TileSet[num2, num - 1].IsActive == 0)
						{
							Main.TileSet[num2, num - 1].IsActive = 1;
							Main.TileSet[num2, num - 1].Type = (byte)EntityID.TileID.CACTUS;
							SquareTileFrame(num2, num - 1);
							if (Main.NetMode == (byte)NetModeSetting.SERVER)
							{
								NetMessage.SendTile(num2, num - 1);
							}
						}
						return;
					}
					bool flag = false;
					bool flag2 = false;
					if (Main.TileSet[num2, num - 1].IsActive != 0 && Main.TileSet[num2, num - 1].Type == (byte)EntityID.TileID.CACTUS)
					{
						if (Main.TileSet[num2 - 1, num].IsActive == 0 && Main.TileSet[num2 - 2, num + 1].IsActive == 0 && Main.TileSet[num2 - 1, num - 1].IsActive == 0 && Main.TileSet[num2 - 1, num + 1].IsActive == 0 && Main.TileSet[num2 - 2, num].IsActive == 0)
						{
							flag = true;
						}
						if (Main.TileSet[num2 + 1, num].IsActive == 0 && Main.TileSet[num2 + 2, num + 1].IsActive == 0 && Main.TileSet[num2 + 1, num - 1].IsActive == 0 && Main.TileSet[num2 + 1, num + 1].IsActive == 0 && Main.TileSet[num2 + 2, num].IsActive == 0)
						{
							flag2 = true;
						}
					}
					int num9 = genRand.Next(3);
					if (num9 == 0 && flag)
					{
						Main.TileSet[num2 - 1, num].IsActive = 1;
						Main.TileSet[num2 - 1, num].Type = (byte)EntityID.TileID.CACTUS;
						SquareTileFrame(num2 - 1, num);
						if (Main.NetMode == (byte)NetModeSetting.SERVER)
						{
							NetMessage.SendTile(num2 - 1, num);
						}
					}
					else if (num9 == 1 && flag2)
					{
						Main.TileSet[num2 + 1, num].IsActive = 1;
						Main.TileSet[num2 + 1, num].Type = (byte)EntityID.TileID.CACTUS;
						SquareTileFrame(num2 + 1, num);
						if (Main.NetMode == (byte)NetModeSetting.SERVER)
						{
							NetMessage.SendTile(num2 + 1, num);
						}
					}
					else
					{
						if (num5 >= genRand.Next(2, 8))
						{
							return;
						}
						if ((Main.TileSet[num2 + 1, num - 1].IsActive == 0 || Main.TileSet[num2 + 1, num - 1].Type != 80) && Main.TileSet[num2, num - 1].IsActive == 0)
						{
							Main.TileSet[num2, num - 1].IsActive = 1;
							Main.TileSet[num2, num - 1].Type = (byte)EntityID.TileID.CACTUS;
							SquareTileFrame(num2, num - 1);
							if (Main.NetMode == (byte)NetModeSetting.SERVER)
							{
								NetMessage.SendTile(num2, num - 1);
							}
						}
					}
				}
				else if (Main.TileSet[num2, num - 1].IsActive == 0 && Main.TileSet[num2, num - 2].IsActive == 0 && Main.TileSet[num2 + num6, num - 1].IsActive == 0 && Main.TileSet[num2 - num6, num - 1].IsActive != 0 && Main.TileSet[num2 - num6, num - 1].Type == (byte)EntityID.TileID.CACTUS)
				{
					Main.TileSet[num2, num - 1].IsActive = 1;
					Main.TileSet[num2, num - 1].Type = (byte)EntityID.TileID.CACTUS;
					SquareTileFrame(num2, num - 1);
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.SendTile(num2, num - 1);
					}
				}
			}
		}

		public static void CheckPot(int i, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = 0;
			int num2 = j;
			num += Main.TileSet[i, j].FrameX / 18;
			num2 -= Main.TileSet[i, j].FrameY / 18;
			num &= 1;
			num = -num;
			num += i;
			for (int k = num; k < num + 2; k++)
			{
				int num3 = num2;
				while (true)
				{
					if (num3 < num2 + 2)
					{
						int num4 = (Main.TileSet[k, num3].FrameX / 18) & 1;
						if (num4 == k - num && Main.TileSet[k, num3].IsActive != 0 && Main.TileSet[k, num3].Type == (byte)EntityID.TileID.POT && Main.TileSet[k, num3].FrameY == (num3 - num2) * 18)
						{
							num3++;
							continue;
						}
					}
					else if (Main.TileSet[k, num2 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[k, num2 + 2].Type])
					{
						break;
					}
					ToDestroyObject = true;
					for (int l = num; l < num + 2; l++)
					{
						for (int m = num2; m < num2 + 2; m++)
						{
							if (Main.TileSet[l, m].Type == 28 && Main.TileSet[l, m].IsActive != 0)
							{
								KillTile(l, m);
							}
						}
					}
					if (!Gen)
					{
						Rectangle rect = default;
						rect.X = i << 4;
						rect.Y = j << 4;
						rect.Width = (rect.Height = 16);
						Main.PlaySound(13, rect.X, rect.Y);
						Gore.NewGore(new Vector2(rect.X, rect.Y), default, (int)EntityID.GoreID.LIGHT_BROWN_POT1);
						Gore.NewGore(new Vector2(rect.X, rect.Y), default, (int)EntityID.GoreID.LIGHT_BROWN_POT2);
						Gore.NewGore(new Vector2(rect.X, rect.Y), default, (int)EntityID.GoreID.LIGHT_BROWN_POT3);
						if (genRand.Next(40) == 0 && (Main.TileSet[num, num2].WallType == (byte)EntityID.WallID.BLUE_DUNGEON_UNSAFE || Main.TileSet[num, num2].WallType == (byte)EntityID.WallID.GREEN_DUNGEON_UNSAFE || Main.TileSet[num, num2].WallType == (byte)EntityID.WallID.PINK_DUNGEON_UNSAFE))
						{
							Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GOLDEN_KEY);
						}
						else if (genRand.Next(45) == 0)
						{
							if (j < Main.WorldSurface)
							{
								int num5 = genRand.Next(4);
								if (num5 == 0)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.IRONSKIN_POTION);
								}
								if (num5 == 1)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SHINE_POTION);
								}
								if (num5 == 2)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.NIGHT_OWL_POTION);
								}
								if (num5 == 3)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SWIFTNESS_POTION);
								}
							}
							else if (j < Main.RockLayer)
							{
								int num6 = genRand.Next(7);
								if (num6 == 0)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.REGENERATION_POTION);
								}
								if (num6 == 1)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SHINE_POTION);
								}
								if (num6 == 2)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.NIGHT_OWL_POTION);
								}
								if (num6 == 3)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SWIFTNESS_POTION);
								}
								if (num6 == 4)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.ARCHERY_POTION);
								}
								if (num6 == 5)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GILLS_POTION);
								}
								if (num6 == 6)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.HUNTER_POTION);
								}
							}
							else if (j < Main.MaxTilesY - 200)
							{
								int num7 = genRand.Next(10);
								if (num7 == 0)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SPELUNKER_POTION);
								}
								if (num7 == 1)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.FEATHERFALL_POTION);
								}
								if (num7 == 2)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.NIGHT_OWL_POTION);
								}
								if (num7 == 3)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.WATER_WALKING_POTION);
								}
								if (num7 == 4)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.ARCHERY_POTION);
								}
								if (num7 == 5)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GRAVITATION_POTION);
								}
								if (num7 == 6)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.THORNS_POTION);
								}
								if (num7 == 7)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.WATER_WALKING_POTION);
								}
								if (num7 == 8)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.INVISIBILITY_POTION);
								}
								if (num7 == 9)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.HUNTER_POTION);
								}
							}
							else
							{
								int num8 = genRand.Next(12);
								if (num8 == 0)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SPELUNKER_POTION);
								}
								if (num8 == 1)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.FEATHERFALL_POTION);
								}
								if (num8 == 2)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.MANA_REGENERATION_POTION);
								}
								if (num8 == 3)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.OBSIDIAN_SKIN_POTION);
								}
								if (num8 == 4)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.MAGIC_POWER_POTION);
								}
								if (num8 == 5)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.INVISIBILITY_POTION);
								}
								if (num8 == 6)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.HUNTER_POTION);
								}
								if (num8 == 7)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GRAVITATION_POTION);
								}
								if (num8 == 8)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.THORNS_POTION);
								}
								if (num8 == 9)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.WATER_WALKING_POTION);
								}
								if (num8 == 10)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.OBSIDIAN_SKIN_POTION);
								}
								if (num8 == 11)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.BATTLE_POTION);
								}
							}
						}
						else
						{
							int num9 = Main.Rand.Next(8);
							if (num9 == 0)
							{
								Player player = Player.FindClosest(ref rect);
								if (player.statLife < player.StatLifeMax)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.HEART);
								}
							}
							else if (num9 == 1)
							{
								Player player2 = Player.FindClosest(ref rect);
								if (player2.statMana < player2.statManaMax)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.STAR);
								}
							}
							else if (num9 == 2)
							{
								int stack = Main.Rand.Next(1, 6);
								if (Main.TileSet[i, j].Liquid > 0)
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GLOWSTICK, stack);
								}
								else
								{
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.TORCH, stack);
								}
							}
							else if (num9 == 3)
							{
								int type = (int)EntityID.ItemID.WOODEN_ARROW;
								if (j < Main.RockLayer && genRand.Next(2) == 0)
								{
									type = ((!Main.InHardMode) ? (int)EntityID.ItemID.SHURIKEN : (int)EntityID.ItemID.GRENADE);
								}
								if (j > Main.MaxTilesY - 200)
								{
									type = (int)EntityID.ItemID.HELLFIRE_ARROW;
								}
								else if (Main.InHardMode)
								{
									type = ((Main.Rand.Next(2) != 0) ? (int)EntityID.ItemID.UNHOLY_ARROW : (int)EntityID.ItemID.SILVER_BULLET);
								}
								Item.NewItem(rect.X, rect.Y, 16, 16, type, Main.Rand.Next(8) + 3);
							}
							else if (num9 == 4)
							{
								int type2 = (int)EntityID.ItemID.LESSER_HEALING_POTION;
								if (j > Main.MaxTilesY - 200 || Main.InHardMode)
								{
									type2 = (int)EntityID.ItemID.HEALING_POTION;
								}
								Item.NewItem(rect.X, rect.Y, 16, 16, type2);
							}
							else if (num9 == 5 && j > Main.RockLayer)
							{
								Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.BOMB, Main.Rand.Next(4) + 1);
							}
							else
							{
								float num10 = 200 + genRand.Next(-100, 101);
								if (j < Main.WorldSurface)
								{
									num10 *= 0.5f;
								}
								else if (j < Main.RockLayer)
								{
									num10 *= 0.75f;
								}
								else if (j > Main.MaxTilesY - 250)
								{
									num10 *= 1.25f;
								}
								num10 *= 1f + Main.Rand.Next(-20, 21) * 0.01f;
								if (Main.Rand.Next(5) == 0)
								{
									num10 *= 1f + Main.Rand.Next(5, 11) * 0.01f;
								}
								if (Main.Rand.Next(10) == 0)
								{
									num10 *= 1f + Main.Rand.Next(10, 21) * 0.01f;
								}
								if (Main.Rand.Next(15) == 0)
								{
									num10 *= 1f + Main.Rand.Next(20, 41) * 0.01f;
								}
								if (Main.Rand.Next(20) == 0)
								{
									num10 *= 1f + Main.Rand.Next(40, 81) * 0.01f;
								}
								if (Main.Rand.Next(25) == 0)
								{
									num10 *= 1f + Main.Rand.Next(50, 101) * 0.01f;
								}
								while ((int)num10 > 0) // Pretty sure the max num10 can reach is 2990, so unless I'm wrong, why is 10000f an option? Let alone 1000000f.
								{
									if (num10 > 1000000f)
									{
										int num11 = (int)(num10 / 1000000f);
										if (num11 > 50 && Main.Rand.Next(2) == 0)
										{
											num11 /= Main.Rand.Next(3) + 1;
										}
										if (Main.Rand.Next(2) == 0)
										{
											num11 /= Main.Rand.Next(3) + 1;
										}
										if (num11 > 0)
										{
											num10 -= 1000000 * num11;
											Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.PLATINUM_COIN, num11);
										}
										continue;
									}
									if (num10 > 10000f)
									{
										int num12 = (int)(num10 / 10000f);
										if (num12 > 50 && Main.Rand.Next(2) == 0)
										{
											num12 /= Main.Rand.Next(3) + 1;
										}
										if (Main.Rand.Next(2) == 0)
										{
											num12 /= Main.Rand.Next(3) + 1;
										}
										if (num12 > 0)
										{
											num10 -= 10000 * num12;
											Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.GOLD_COIN, num12);
										}
										continue;
									}
									if (num10 > 100f)
									{
										int num13 = (int)(num10 / 100f);
										if (num13 > 50 && Main.Rand.Next(2) == 0)
										{
											num13 /= Main.Rand.Next(3) + 1;
										}
										if (Main.Rand.Next(2) == 0)
										{
											num13 /= Main.Rand.Next(3) + 1;
										}
										if (num13 > 0)
										{
											num10 -= 100 * num13;
											Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.SILVER_COIN, num13);
										}
										continue;
									}
									int num14 = (int)num10;
									if (num14 > 50 && Main.Rand.Next(2) == 0)
									{
										num14 /= Main.Rand.Next(3) + 1;
									}
									if (Main.Rand.Next(2) == 0)
									{
										num14 /= Main.Rand.Next(4) + 1;
									}
									if (num14 < 1)
									{
										num14 = 1;
									}
									num10 -= num14;
									Item.NewItem(rect.X, rect.Y, 16, 16, (int)EntityID.ItemID.COPPER_COIN, num14);
								}
							}
						}
					}
					ToDestroyObject = false;
					return;
				}
			}
		}

		public static int PlaceChest(int x, int y, bool notNearOtherChests = false, int style = 0)
		{
			for (int i = x; i < x + 2; i++)
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0)
					{
						return -1;
					}
					if (Main.TileSet[i, j].Lava != 0)
					{
						return -1;
					}
				}
				if (Main.TileSet[i, y + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, y + 1].Type])
				{
					return -1;
				}
			}
			if (notNearOtherChests)
			{
				for (int k = x - 25; k < x + 25; k++)
				{
					for (int l = y - 8; l < y + 8; l++)
					{
						try
						{
							if (Main.TileSet[k, l].IsActive != 0 && Main.TileSet[k, l].Type == (byte)EntityID.TileID.CHEST)
							{
								return -1;
							}
						}
						catch
						{
						}
					}
				}
			}
			int num = Chest.CreateChest(x, y - 1);
			if (num != -1)
			{
				Main.TileSet[x, y - 1].IsActive = 1;
				Main.TileSet[x, y - 1].FrameY = 0;
				Main.TileSet[x, y - 1].FrameX = (short)(36 * style);
				Main.TileSet[x, y - 1].Type = (byte)EntityID.TileID.CHEST;
				Main.TileSet[x + 1, y - 1].IsActive = 1;
				Main.TileSet[x + 1, y - 1].FrameY = 0;
				Main.TileSet[x + 1, y - 1].FrameX = (short)(18 + 36 * style);
				Main.TileSet[x + 1, y - 1].Type = (byte)EntityID.TileID.CHEST;
				Main.TileSet[x, y].IsActive = 1;
				Main.TileSet[x, y].FrameY = 18;
				Main.TileSet[x, y].FrameX = (short)(36 * style);
				Main.TileSet[x, y].Type = (byte)EntityID.TileID.CHEST;
				Main.TileSet[x + 1, y].IsActive = 1;
				Main.TileSet[x + 1, y].FrameY = 18;
				Main.TileSet[x + 1, y].FrameX = (short)(18 + 36 * style);
				Main.TileSet[x + 1, y].Type = (byte)EntityID.TileID.CHEST;
			}
			return num;
		}

		public static void CheckChest(int i, int j)
		{
			if (ToDestroyObject)
			{
				return;
			}
			int num = 0;
			int num2 = j;
			num += Main.TileSet[i, j].FrameX / 18;
			num2 -= Main.TileSet[i, j].FrameY / 18;
			num &= 1;
			num = -num;
			num += i;
			for (int k = num; k < num + 2; k++)
			{
				int num3 = num2;
				while (true)
				{
					if (num3 < num2 + 2)
					{
						int num4 = (Main.TileSet[k, num3].FrameX / 18) & 1;
						if (Main.TileSet[k, num3].IsActive != 0 && Main.TileSet[k, num3].Type == (byte)EntityID.TileID.CHEST && num4 == k - num && Main.TileSet[k, num3].FrameY == (num3 - num2) * 18)
						{
							num3++;
							continue;
						}
					}
					else if (Main.TileSet[k, num2 + 2].IsActive != 0 && Main.TileSolid[Main.TileSet[k, num2 + 2].Type])
					{
						break;
					}
					int type = (int)EntityID.ItemID.CHEST;
					if (Main.TileSet[i, j].FrameX >= 216)
					{
						type = (int)EntityID.ItemID.TRASH_CAN;
					}
					else if (Main.TileSet[i, j].FrameX >= 180)
					{
						type = (int)EntityID.ItemID.BARREL;
					}
					else if (Main.TileSet[i, j].FrameX >= 108)
					{
						type = (int)EntityID.ItemID.SHADOW_CHEST;
					}
					else if (Main.TileSet[i, j].FrameX >= 36)
					{
						type = (int)EntityID.ItemID.GOLD_CHEST;
					}
					ToDestroyObject = true;
					for (int l = num; l < num + 2; l++)
					{
						for (int m = num2; m < num2 + 3; m++)
						{
							if (Main.TileSet[l, m].Type == (byte)EntityID.TileID.CHEST && Main.TileSet[l, m].IsActive != 0)
							{
								Chest.DestroyChest(l, m);
								KillTile(l, m);
							}
						}
					}
					if (!Gen)
					{
						Item.NewItem(i * 16, j * 16, 32, 32, type);
					}
					ToDestroyObject = false;
					return;
				}
			}
		}

		public static bool PlaceWire(int i, int j)
		{
			if (Main.TileSet[i, j].wire == 0)
			{
				Main.TileSet[i, j].wire = 16;
				Main.PlaySound(0, i << 4, j << 4);
				return true;
			}
			return false;
		}

		public unsafe static bool KillWire(int i, int j)
		{
			if (Main.TileSet[i, j].wire != 0)
			{
				Main.TileSet[i, j].wire = 0;
				i <<= 4;
				j <<= 4;
				Main.PlaySound(0, i, j);
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					Item.NewItem(i, j, 16, 16, (int)EntityID.ItemID.WIRE);
				}
				for (int k = 0; k < 3; k++)
				{
					if (null == Main.DustSet.NewDust(i, j, 16, 16, 50))
					{
						break;
					}
				}
				return true;
			}
			return false;
		}

		public static bool CanPlaceTile(int i, ref int j, int type, int style = 0)
		{
			if (i >= 0 && j >= 0 && i < Main.MaxTilesX && j < Main.MaxTilesY)
			{
				if (style >= 0 && Main.TileSet[i, j].IsActive == 0 && Main.TileSolid[type] && Collision.AnyPlayerOrNPC(i, j, 1))
				{
					return false;
				}

				var leftTile = Main.TileSet[i - 1, j];
				var rightTile = Main.TileSet[i + 1, j];
				var belowTile = Main.TileSet[i, j + 1];
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.SHORT_GRASS_PLANTS:
					case EntityID.TileID.SHORT_CORRUPT_PLANTS:
					case EntityID.TileID.SUNFLOWER:
					case EntityID.TileID.CORRUPTION_THORN:
					case EntityID.TileID.COBWEB:
					case EntityID.TileID.JUNGLE_THORN:
					case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
						return Main.TileSet[i, j].Liquid == 0;
					case EntityID.TileID.TORCH:
						bool leftObstructed =
							leftTile.IsActive != 0 &&
							(
								Main.TileSolid[leftTile.Type] ||
								leftTile.Type == (byte)EntityID.TileID.WOODEN_BEAM ||
								(
									leftTile.Type == (byte)EntityID.TileID.TREE &&
									Main.TileSet[i - 1, j - 1].Type == (byte)EntityID.TileID.TREE &&
									Main.TileSet[i - 1, j + 1].Type == (byte)EntityID.TileID.TREE
								)
							);

						bool rightObstructed =
							rightTile.IsActive != 0 &&
							(
								Main.TileSolid[rightTile.Type] ||
								rightTile.Type == (byte)EntityID.TileID.WOODEN_BEAM ||
								(
									rightTile.Type == (byte)EntityID.TileID.TREE &&
									Main.TileSet[i + 1, j - 1].Type == (byte)EntityID.TileID.TREE &&
									Main.TileSet[i + 1, j + 1].Type == (byte)EntityID.TileID.TREE
								)
							);

						if (leftObstructed || rightObstructed || (belowTile.IsActive != 0 && Main.TileSolid[belowTile.Type]))
						{
							if (style != 8)
							{
								return Main.TileSet[i, j].Liquid == 0;
							}
							return true;
						}
						return false;
					case EntityID.TileID.DOOR_CLOSED:
						if (Main.TileSet[i, j - 1].IsActive == 0 && Main.TileSet[i, j - 2].IsActive == 0 && Main.TileSet[i, j - 3].IsActive != 0 && Main.TileSolid[Main.TileSet[i, j - 3].Type])
						{
							j--;
							return true;
						}
						if (Main.TileSet[i, j + 1].IsActive == 0 && Main.TileSet[i, j + 2].IsActive == 0 && Main.TileSet[i, j + 3].IsActive != 0 && Main.TileSolid[Main.TileSet[i, j + 3].Type])
						{
							j++;
							return true;
						}
						return false;
					case EntityID.TileID.SAPLING:
						if (belowTile.IsActive != 0 && (belowTile.Type == (byte)EntityID.TileID.GRASS || belowTile.Type == (byte)EntityID.TileID.HALLOWED_GRASS || belowTile.Type == (byte)EntityID.TileID.SNOW))
						{
							return Main.TileSet[i, j].Liquid == 0;
						}
						return false;
					case EntityID.TileID.GRASS:
					case EntityID.TileID.CORRUPT_GRASS:
					case EntityID.TileID.HALLOWED_GRASS:
						if (Main.TileSet[i, j].Type == 0)
						{
							return Main.TileSet[i, j].IsActive != 0;
						}
						return false;
					case EntityID.TileID.JUNGLE_GRASS:
					case EntityID.TileID.MUSHROOM_GRASS:
						if (Main.TileSet[i, j].Type == (byte)EntityID.TileID.MUD)
						{
							return Main.TileSet[i, j].IsActive != 0;
						}
						return false;
					case EntityID.TileID.SHORT_JUNGLE_PLANTS:
					case EntityID.TileID.GLOWING_MUSHROOM:
						if (j + 1 < Main.MaxTilesY && belowTile.IsActive != 0)
						{
							return belowTile.Type == type - 1;
						}
						return false;
					case EntityID.TileID.CORAL:
						if (leftTile.IsActive != 0 || rightTile.IsActive != 0 || Main.TileSet[i, j - 1].IsActive != 0)
						{
							return false;
						}
						if (belowTile.IsActive == 0 || !Main.TileSolid[belowTile.Type])
						{
							return false;
						}
						break;
					case EntityID.TileID.SWITCH:
						bool leftUnobstructed =
							leftTile.IsActive == 0 ||
							(
								!Main.TileSolid[leftTile.Type] &&
								leftTile.Type != (byte)EntityID.TileID.WOODEN_BEAM &&
								(
									leftTile.Type != (byte)EntityID.TileID.TREE ||
									Main.TileSet[i - 1, j - 1].Type != (byte)EntityID.TileID.TREE ||
									Main.TileSet[i - 1, j + 1].Type != (byte)EntityID.TileID.TREE
								)
							);

						bool rightUnobstructed =
							rightTile.IsActive == 0 ||
							(
								!Main.TileSolid[rightTile.Type] &&
								rightTile.Type != (byte)EntityID.TileID.WOODEN_BEAM &&
								(
									rightTile.Type != (byte)EntityID.TileID.TREE ||
									Main.TileSet[i + 1, j - 1].Type != (byte)EntityID.TileID.TREE ||
									Main.TileSet[i + 1, j + 1].Type != (byte)EntityID.TileID.TREE
								)
							);

						if (leftUnobstructed && rightUnobstructed)
						{
							if (belowTile.IsActive != 0)
							{
								return Main.TileSolid[belowTile.Type];
							}
							return false;
						}
						return true;
					case EntityID.TileID.CRYSTAL_SHARD:
					case EntityID.TileID.XMAS_LIGHT:
						if (!SolidTileUnsafe(i - 1, j) && !SolidTileUnsafe(i + 1, j) && !SolidTileUnsafe(i, j - 1))
						{
							return SolidTileUnsafe(i, j + 1);
						}
						return true;
				}
				return true;
			}
			return false;
		}

		public unsafe static bool PlaceTile(int i, int j, int type, bool ToMute = false, bool IsForced = false, int plr = -1, int style = 0)
		{
			bool flag = false;
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (CanPlaceTile(i, ref j, type, style) || IsForced)
				{
					ptr->FrameX = 0;
					ptr->FrameY = 0;
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.SHORT_GRASS_PLANTS:
						case EntityID.TileID.SHORT_CORRUPT_PLANTS:
						case EntityID.TileID.SHORT_HALLOWED_PLANTS:
							if (j + 1 >= Main.MaxTilesY || ptr[1].IsActive == 0 || 
								(
									(type != (int)EntityID.TileID.SHORT_GRASS_PLANTS || ptr[1].Type != (byte)EntityID.TileID.GRASS) && 
									(type != (int)EntityID.TileID.SHORT_CORRUPT_PLANTS || ptr[1].Type != (byte)EntityID.TileID.CORRUPT_GRASS) && 
									(type != (int)EntityID.TileID.SHORT_GRASS_PLANTS || ptr[1].Type != (byte)EntityID.TileID.CLAY_POT) && 
									(type != (int)EntityID.TileID.SHORT_HALLOWED_PLANTS || ptr[1].Type != (byte)EntityID.TileID.HALLOWED_GRASS)
								)) { break; }
							if (type == (int)EntityID.TileID.SHORT_CORRUPT_PLANTS && genRand.Next(13) == 0)
							{
								type = (int)EntityID.TileID.CORRUPTION_THORN;
								flag = true;
							}
							else if (ptr[1].Type == (byte)EntityID.TileID.CLAY_POT)
							{
								ptr->FrameX = (short)(genRand.Next(2) * 18 + 108);
								flag = true;
							}
							else if (ptr->WallType == 0 && ptr[1].WallType == (byte)EntityID.WallID.NONE)
							{
								if (genRand.Next(50) == 0 || (type == (int)EntityID.TileID.SHORT_CORRUPT_PLANTS && genRand.Next(40) == 0))
								{
									ptr->FrameX = 144;
								}
								else if (genRand.Next(35) == 0)
								{
									ptr->FrameX = (short)(genRand.Next(2) * 18 + 108);
								}
								else
								{
									ptr->FrameX = (short)(genRand.Next(6) * 18);
								}
								flag = true;
							}
							if (flag)
							{
								ptr->IsActive = 1;
								ptr->Type = (byte)type;
							}
							break;
						case EntityID.TileID.SHORT_JUNGLE_PLANTS:
							ptr->IsActive = 1;
							if (genRand.Next(16) == 0 && j > Main.WorldSurface)
							{
								ptr->Type = (byte)EntityID.TileID.JUNGLE_THORN;
							}
							else
							{
								ptr->Type = (byte)type;
								if (j > Main.RockLayer && genRand.Next(60) == 0)
								{
									ptr->FrameX = 144;
								}
								else if (j > Main.RockLayer && genRand.Next(1000) == 0)
								{
									ptr->FrameX = 162;
								}
								else if (genRand.Next(15) == 0)
								{
									ptr->FrameX = (short)(genRand.Next(2) * 18 + 108);
								}
								else
								{
									ptr->FrameX = (short)(genRand.Next(6) * 18);
								}
							}
							flag = true;
							break;
						case EntityID.TileID.GLOWING_MUSHROOM:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							ptr->FrameX = (short)(genRand.Next(5) * 18);
							flag = true;
							break;
						case EntityID.TileID.CRYSTAL_SHARD:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							ptr->FrameX = (short)(genRand.Next(8) * 18);
							flag = true;
							break;
						case EntityID.TileID.SWITCH:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							flag = true;
							break;
						case EntityID.TileID.TORCH:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							ptr->FrameY = (short)(22 * style);
							flag = true;
							break;
						case EntityID.TileID.DOOR_CLOSED:
							flag = PlaceDoor(i, j, type);
							break;
						case EntityID.TileID.MANNEQUIN:
							flag = PlaceMan(i, j, style);
							break;
						case EntityID.TileID.XMAS_LIGHT:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							ptr->FrameX = (short)(18 * style);
							flag = true;
							break;
						case EntityID.TileID.MUSIC_BOX:
							flag = PlaceMB(i, j, type, style);
							break;
						case EntityID.TileID.CHANDELIER:
						case EntityID.TileID.JACK_O_LANTERN:
						case EntityID.TileID.PRESENT:
						case EntityID.TileID.SAWMILL:
							flag = Place3x3(i, j, type);
							break;
						case EntityID.TileID.BOTTLE:
						case EntityID.TileID.CANDLE:
						case EntityID.TileID.WATER_CANDLE:
						case EntityID.TileID.BOOK:
						case EntityID.TileID.CLAY_POT:
							flag = PlaceOnTable1x1(i, j, type, style);
							break;
						case EntityID.TileID.TABLE:
						case EntityID.TileID.DEMON_ALTAR:
						case EntityID.TileID.LOOM:
						case EntityID.TileID.PIANO:
						case EntityID.TileID.DRESSER:
						case EntityID.TileID.BENCH:
						case EntityID.TileID.TINKERERS_WORKSHOP:
#if VERSION_101
						case EntityID.TileID.CAMPFIRE:
#endif
							flag = Place3x2(i, j, type);
							break;
						case EntityID.TileID.CHAIR:
						case EntityID.TileID.SAPLING:
							flag = Place1x2(i, j, type, style);
							break;
						case EntityID.TileID.ANVIL:
						case EntityID.TileID.WORK_BENCH:
						case EntityID.TileID.PIGGYBANK:
						case EntityID.TileID.BOWL:
						case EntityID.TileID.MYTHRIL_ANVIL:
							flag = Place2x1(i, j, type);
							break;
						case EntityID.TileID.LAMP_POST:
						case EntityID.TileID.TIKI_TORCH:
							flag = Place1xX(i, j, type);
							break;
						case EntityID.TileID.GRANDFATHERS_CLOCK:
						case EntityID.TileID.STATUE:
							flag = Place2xX(i, j, type, style);
							break;
						case EntityID.TileID.FURNACE:
						case EntityID.TileID.HELLFORGE:
						case EntityID.TileID.ADAMANTITE_FORGE:
							flag = Place3x2(i, j, type);
							break;
						case EntityID.TileID.BED:
						case EntityID.TileID.BATHTUB:
							flag = Place4x2(i, j, type, (plr < 0) ? 1 : Main.PlayerSet[plr].direction);
							break;
						case EntityID.TileID.CHEST:
							flag = PlaceChest(i, j, notNearOtherChests: false, style) >= 0;
							break;
						case EntityID.TileID.BANNER:
							flag = PlaceBanner(i, j, type, style);
							break;
						case EntityID.TileID.PRESSURE_PLATE:
						case EntityID.TileID.EXPLOSIVES:
						case EntityID.TileID.TIMER:
							flag = Place1x1(i, j, type, style);
							break;
						case EntityID.TileID.BOOKCASE:
						case EntityID.TileID.THRONE:
							flag = Place3x4(i, j, type);
							break;
						case EntityID.TileID.SUNFLOWER:
							flag = PlaceSunflower(i, j);
							break;
						case EntityID.TileID.POT:
							flag = PlacePot(i, j);
							break;
						case EntityID.TileID.CHAIN_LANTERN:
							flag = Place1x2Top(i, j, type);
							break;
						case EntityID.TileID.SIGN:
						case EntityID.TileID.TOMBSTONE:
							flag = PlaceSign(i, j, type);
							break;
						case EntityID.TileID.DAYBLOOM_GROWING:
						case EntityID.TileID.DAYBLOOM_MATURE:
						case EntityID.TileID.DAYBLOOM_BLOOMING:
							flag = PlaceAlch(i, j, style);
							break;
						case EntityID.TileID.KEG:
						case EntityID.TileID.CHINESE_LANTERN:
						case EntityID.TileID.COOKING_POT:
						case EntityID.TileID.SAFE:
						case EntityID.TileID.SKULL_LANTERN:
						case EntityID.TileID.TRASH_CAN:
						case EntityID.TileID.CANDELABRA:
						case EntityID.TileID.CRYSTAL_BALL:
						case EntityID.TileID.DISCO_BALL:
						case EntityID.TileID.LEVER:
						case EntityID.TileID.BOULDER:
						case EntityID.TileID.PUMP_IN:
						case EntityID.TileID.PUMP_OUT:
							flag = Place2x2(i, j, type);
							break;
						default:
							ptr->IsActive = 1;
							ptr->Type = (byte)type;
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.CORAL:
									ptr->FrameX = (short)(26 * genRand.Next(6));
									break;
								case EntityID.TileID.TRAP:
									if (style == 1)
									{
										ptr->FrameX = 18;
									}
									break;
							}
							flag = true;
							break;
					}
					if (flag && !ToMute && !Gen)
					{
						SquareTileFrame(i, j);
						if (type == (int)EntityID.TileID.ICE)
						{
							Main.PlaySound(2, i * 16, j * 16, 30);
						}
						else
						{
							Main.PlaySound(0, i * 16, j * 16);
							if (type == (int)EntityID.TileID.DEMONITE_ORE || type == (int)EntityID.TileID.DEMONITE_BRICK)
							{
								Main.DustSet.NewDust(i * 16, j * 16, 16, 16, 14);
								Main.DustSet.NewDust(i * 16, j * 16, 16, 16, 14);
							}
						}
					}
				}
			}
			return flag;
		}

#if DEBUG
		public static void countOre()
		{
			int Copper = 0;
			int Iron = 0;
			int Silver = 0;
			int Gold = 0;
			int Cobalt = 0;
			int Mythril = 0;
			int Adamantite = 0;

			for (int i = 0; i < Main.MaxTilesX; i++)
			{
				for (int j = 0; j < Main.MaxTilesY; j++)
				{
					int OreType = 0;
					if (Main.TileSet[i, j].IsActive != 0)
					{
						OreType = Main.TileSet[i, j].Type;
						if (OreType == (int)EntityID.TileID.COPPER_ORE)
						{
							Copper++;
						}
						if (OreType == (int)EntityID.TileID.IRON_ORE)
						{
							Iron++;
						}
						if (OreType == (int)EntityID.TileID.SILVER_ORE)
						{
							Silver++;
						}
						if (OreType == (int)EntityID.TileID.GOLD_ORE)
						{
							Gold++;
						}
						if (OreType == (int)EntityID.TileID.COBALT_ORE)
						{
							Cobalt++;
						}
						if (OreType == (int)EntityID.TileID.MYTHRIL_ORE)
						{
							Mythril++;
						}
						if (OreType == (int)EntityID.TileID.ADAMANTITE_ORE)
						{
							Adamantite++;
						}
					}
				}
			}
			Main.NewText("Copper: " + $"{Copper:0,0}" + " Iron: " + $"{Iron:0,0}" + " Silver: " + $"{Silver:0,0}" + " Gold:" + $"{Gold:0,0}", 250, 250, 0);
			Main.NewText("Cobalt: " + $"{Cobalt:0,0}" + " Mythril: " + $"{Mythril:0,0}" + " Adamantite: " + $"{Adamantite:0,0}", 250, 250, 0);
		}
#endif

		public static void UpdateMech()
		{
			for (int num = numMechs - 1; num >= 0; num--)
			{
				mech[num].Time--;
				if (Main.TileSet[mech[num].X, mech[num].Y].IsActive != 0 && Main.TileSet[mech[num].X, mech[num].Y].Type == (byte)EntityID.TileID.TIMER)
				{
					if (Main.TileSet[mech[num].X, mech[num].Y].FrameY == 0)
					{
						mech[num].Time = 0;
					}
					else
					{
						int num2 = Main.TileSet[mech[num].X, mech[num].Y].FrameX / 18;
#if (!VERSION_INITIAL || IS_PATCHED)
						if (mech[num].Time % (num2 * 120 + 60) == 0)
#else
						switch (num2)
						{
						case 0:
							num2 = 60;
							break;
						case 1:
							num2 = 180;
							break;
						case 2:
							num2 = 300;
							break;
						}
						if (Math.IEEERemainder(mech[num].Time, num2) == 0.0)
#endif
						{
							mech[num].Time = 18000;
							TripWire(mech[num].X, mech[num].Y);
						}
					}
				}
				if (mech[num].Time <= 0)
				{
					if (Main.TileSet[mech[num].X, mech[num].Y].IsActive != 0 && Main.TileSet[mech[num].X, mech[num].Y].Type == (byte)EntityID.TileID.TIMER)
					{
						Main.TileSet[mech[num].X, mech[num].Y].FrameY = 0;
						NetMessage.SendTile(mech[num].X, mech[num].Y);
					}
					for (int i = num; i < numMechs; i++)
					{
						ref Mech reference = ref mech[i];
						reference = mech[i + 1];
					}
					numMechs--;
				}
			}
		}

		public static bool checkMech(int i, int j, int time)
		{
			for (int k = 0; k < numMechs; k++)
			{
				if (mech[k].X == i && mech[k].Y == j)
				{
					return false;
				}
			}
			if (numMechs < MaxNumWire - 1)
			{
				mech[numMechs].X = (short)i;
				mech[numMechs].Y = (short)j;
				mech[numMechs].Time = time;
				numMechs++;
				return true;
			}
			return false;
		}

		public static void HitSwitch(int i, int j)
		{
			switch ((EntityID.TileID)Main.TileSet[i, j].Type)
			{
				case EntityID.TileID.PRESSURE_PLATE:
					Main.PlaySound(28, i * 16, j * 16, 0);
					TripWire(i, j);
					break;
				case EntityID.TileID.SWITCH:
					if (Main.TileSet[i, j].FrameY == 0)
					{
						Main.TileSet[i, j].FrameY = 18;
					}
					else
					{
						Main.TileSet[i, j].FrameY = 0;
					}
					Main.PlaySound(28, i * 16, j * 16, 0);
					TripWire(i, j);
					break;
				case EntityID.TileID.TIMER:
					if (Main.TileSet[i, j].FrameY == 0)
					{
						Main.TileSet[i, j].FrameY = 18;
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							checkMech(i, j, 18000);
						}
					}
					else
					{
						Main.TileSet[i, j].FrameY = 0;
					}
					Main.PlaySound(28, i * 16, j * 16, 0);
					break;
				case EntityID.TileID.LEVER:
					{
						int num = i;
						int num2 = j;
						short num3 = 36;
						num = Main.TileSet[i, j].FrameX / 18 * -1;
						num2 = Main.TileSet[i, j].FrameY / 18 * -1;
						if (num < -1)
						{
							num += 2;
							num3 = -36;
						}
						num += i;
						num2 += j;
						for (int k = num; k < num + 2; k++)
						{
							for (int l = num2; l < num2 + 2; l++)
							{
								if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.LEVER)
								{
									Main.TileSet[k, l].FrameX += num3;
								}
							}
						}
						TileFrame(num, num2);
						Main.PlaySound(28, i * 16, j * 16, 0);
						for (int m = num; m < num + 2; m++)
						{
							for (int n = num2; n < num2 + 2; n++)
							{
								if (Main.TileSet[m, n].Type == (byte)EntityID.TileID.LEVER && Main.TileSet[m, n].IsActive != 0 && Main.TileSet[m, n].wire != 0)
								{
									TripWire(m, n);
									return;
								}
							}
						}
						break;
					}
			}
		}

		public static void TripWire(int i, int j)
		{
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
#if (VERSION_INITIAL && !IS_PATCHED)
				numWire = 0;
#endif
				numNoWire = 0;
				numInPump = 0;
				numOutPump = 0;
				NoWire(i, j);
#if (!VERSION_INITIAL || IS_PATCHED)
				wireCheck(i, j);
#else
				hitWire(i, j);
#endif
				if (numInPump > 0 && numOutPump > 0)
				{
					xferWater();
				}
			}
		}

		public static void xferWater()
		{
			for (int i = 0; i < numInPump; i++)
			{
				int x = inPump[i].X;
				int y = inPump[i].Y;
				int liquid = Main.TileSet[x, y].Liquid;
				if (liquid <= 0)
				{
					continue;
				}
				int lava = Main.TileSet[x, y].Lava;
				for (int j = 0; j < numOutPump; j++)
				{
					int x2 = outPump[j].X;
					int y2 = outPump[j].Y;
					int liquid2 = Main.TileSet[x2, y2].Liquid;
					if (liquid2 >= byte.MaxValue)
					{
						continue;
					}
					int num = Main.TileSet[x2, y2].Lava;
					if (liquid2 == 0)
					{
						num = lava;
					}
					if (lava == num)
					{
						int num2 = liquid;
						if (num2 + liquid2 > byte.MaxValue)
						{
							num2 = byte.MaxValue - liquid2;
						}
						Main.TileSet[x2, y2].Liquid += (byte)num2;
						Main.TileSet[x, y].Liquid -= (byte)num2;
						liquid = Main.TileSet[x, y].Liquid;
						Main.TileSet[x2, y2].Lava = (byte)lava;
						SquareTileFrame(x2, y2);
						if (Main.TileSet[x, y].Liquid == 0)
						{
							Main.TileSet[x, y].Lava = 0;
							SquareTileFrame(x, y);
							break;
						}
					}
				}
				SquareTileFrame(x, y);
			}
		}

		public static void NoWire(int i, int j)
		{
			if (numNoWire < MaxNumWire - 1)
			{
				noWire[numNoWire].X = (short)i;
				noWire[numNoWire].Y = (short)j;
				numNoWire++;
			}
		}

#if (!IS_PATCHED && VERSION_INITIAL)
		public static void hitWire(int i, int j) // PS3 had this until 1.01, along with a few other changes. This leads to some oddities that are not present in the equivalent X360 release, like the King statue not affecting Santa.
		{
			if (numWire >= MaxNumWire - 1 || Main.TileSet[i, j].wire == 0)
			{
				return;
			}
			for (int k = 0; k < numWire; k++)
			{
				if (wire[k].X == i && wire[k].Y == j)
				{
					return;
				}
			}
			wire[numWire].X = (short)i;
			wire[numWire].Y = (short)j;
			numWire++;
			int type = Main.TileSet[i, j].Type;
			bool flag = true;
			for (int l = 0; l < numNoWire; l++)
			{
				if (noWire[l].X == i && noWire[l].Y == j)
				{
					flag = false;
				}
			}
			if (flag && Main.TileSet[i, j].IsActive != 0)
			{
				switch ((EntityID.TileID)type)
				{
				case EntityID.TileID.TIMER:
					HitSwitch(i, j);
					SquareTileFrame(i, j);
					NetMessage.SendTile(i, j);
					break;
				case EntityID.TileID.ACTIVE_STONE:
					Main.TileSet[i, j].Type = (byte)EntityID.TileID.INACTIVE_STONE;
					SquareTileFrame(i, j);
					NetMessage.SendTile(i, j);
					break;
				case EntityID.TileID.INACTIVE_STONE:
					Main.TileSet[i, j].Type = (byte)EntityID.TileID.ACTIVE_STONE;
					SquareTileFrame(i, j);
					NetMessage.SendTile(i, j);
					break;
				case EntityID.TileID.DOOR_OPEN:
					CloseDoor(i, j, forced: true);
					NetMessage.CreateMessage2(24, i, j);
					NetMessage.SendMessage();
					break;
				case EntityID.TileID.DOOR_CLOSED:
				{
					int direction = (Main.Rand.Next(2) << 1) - 1;
					direction = OpenDoor(i, j, direction);
					if (direction != 0)
					{
						NetMessage.CreateMessage3(19, i, j, direction);
						NetMessage.SendMessage();
					}
					break;
				}
				case EntityID.TileID.TORCH:
					if (Main.TileSet[i, j].FrameX < 66)
					{
						Main.TileSet[i, j].FrameX += 66;
					}
					else
					{
						Main.TileSet[i, j].FrameX -= 66;
					}
					NetMessage.SendTile(i, j);
					break;
				case EntityID.TileID.XMAS_LIGHT:
					if (Main.TileSet[i, j].FrameX < 54)
					{
						Main.TileSet[i, j].FrameX += 54;
					}
					else
					{
						Main.TileSet[i, j].FrameX -= 54;
					}
					NetMessage.SendTile(i, j);
					break;
				case EntityID.TileID.CHAIN_LANTERN:
				{
					int num31 = j - Main.TileSet[i, j].FrameY / 18;
					short num32 = 18;
					if (Main.TileSet[i, j].FrameX > 0)
					{
						num32 = -18;
					}
					Main.TileSet[i, num31].FrameX += num32;
					Main.TileSet[i, num31 + 1].FrameX += num32;
					NoWire(i, num31);
					NoWire(i, num31 + 1);
					NetMessage.SendTileSquare(i, j, 2);
					break;
				}
				case EntityID.TileID.TIKI_TORCH:
				{
					int num28 = j - Main.TileSet[i, j].FrameY / 18;
					short num29 = 18;
					if (Main.TileSet[i, j].FrameX > 0)
					{
						num29 = -18;
					}
					Main.TileSet[i, num28].FrameX += num29;
					Main.TileSet[i, num28 + 1].FrameX += num29;
					Main.TileSet[i, num28 + 2].FrameX += num29;
					NoWire(i, num28);
					NoWire(i, num28 + 1);
					NoWire(i, num28 + 2);
					NetMessage.SendTileSquare(i, num28 + 1, 3);
					break;
				}
				case EntityID.TileID.CHINESE_LANTERN:
				case EntityID.TileID.CANDELABRA:
				case EntityID.TileID.DISCO_BALL:
				{
					int num11 = j - Main.TileSet[i, j].FrameY / 18;
					int num12 = Main.TileSet[i, j].FrameX / 18;
					if (num12 > 1)
					{
						num12 -= 2;
					}
					num12 = i - num12;
					short num13 = 36;
					if (Main.TileSet[num12, num11].FrameX > 0)
					{
						num13 = -36;
					}
					Main.TileSet[num12, num11].FrameX += num13;
					Main.TileSet[num12, num11 + 1].FrameX += num13;
					Main.TileSet[num12 + 1, num11].FrameX += num13;
					Main.TileSet[num12 + 1, num11 + 1].FrameX += num13;
					NoWire(num12, num11);
					NoWire(num12, num11 + 1);
					NoWire(num12 + 1, num11);
					NoWire(num12 + 1, num11 + 1);
					NetMessage.SendTileSquare(num12, num11, 3);
					break;
				}
				case EntityID.TileID.CHANDELIER:
				case EntityID.TileID.JACK_O_LANTERN:
				case EntityID.TileID.PRESENT:
				{
					int num23 = j - Main.TileSet[i, j].FrameY / 18;
					int num24 = Main.TileSet[i, j].FrameX / 18;
					if (num24 > 2)
					{
						num24 -= 3;
					}
					num24 = i - num24;
					short num25 = 54;
					if (Main.TileSet[num24, num23].FrameX > 0)
					{
						num25 = -54;
					}
					for (int num26 = num24; num26 < num24 + 3; num26++)
					{
						for (int num27 = num23; num27 < num23 + 3; num27++)
						{
							Main.TileSet[num26, num27].FrameX += num25;
							NoWire(num26, num27);
						}
					}
					NetMessage.SendTileSquare(num24 + 1, num23 + 1, 3);
					break;
				}
				case EntityID.TileID.CANDLE:
				{
					short num14 = 18;
					if (Main.TileSet[i, j].FrameX > 0)
					{
						num14 = -18;
					}
					Main.TileSet[i, j].FrameX += num14;
					NetMessage.SendTileSquare(i, j, 3);
					break;
				}
				case EntityID.TileID.LAMP_POST:
				{
					int num33 = j - Main.TileSet[i, j].FrameY / 18;
					short num34 = 18;
					if (Main.TileSet[i, j].FrameX > 0)
					{
						num34 = -18;
					}
					Main.TileSet[i, num33].FrameX += num34;
					Main.TileSet[i, num33 + 1].FrameX += num34;
					Main.TileSet[i, num33 + 2].FrameX += num34;
					Main.TileSet[i, num33 + 3].FrameX += num34;
					Main.TileSet[i, num33 + 4].FrameX += num34;
					Main.TileSet[i, num33 + 5].FrameX += num34;
					NoWire(i, num33);
					NoWire(i, num33 + 1);
					NoWire(i, num33 + 2);
					NoWire(i, num33 + 3);
					NoWire(i, num33 + 4);
					NoWire(i, num33 + 5);
					NetMessage.SendTileSquare(i, num33 + 3, 7);
					break;
				}
				case EntityID.TileID.TRAP:
					if (checkMech(i, j, 180))
					{
						int num30 = -1;
						if (Main.TileSet[i, j].FrameX != 0)
						{
							num30 = 1;
						}
						float speedX = 12 * num30;
						Vector2 vector = new Vector2(i * 16 + 8, j * 16 + 7);
						vector.X += 10 * num30;
						vector.Y += 2f;
						Projectile.NewProjectile((int)vector.X, (int)vector.Y, speedX, 0f, (int)EntityID.ProjectileID.POISON_DART, 20, 2f);
					}
					break;
				case EntityID.TileID.MUSIC_BOX:
					SwitchMB(i, j);
					break;
				case EntityID.TileID.EXPLOSIVES:
					KillTile(i, j, KillToFail: false, EffectOnly: false, noItem: true);
					NetMessage.SendTile(i, j);
					Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0f, 0f, (int)EntityID.ProjectileID.EXPLOSIVES, 250, 10f);
					break;
				case EntityID.TileID.PUMP_IN:
				case EntityID.TileID.PUMP_OUT:
				{
					int num15 = j - Main.TileSet[i, j].FrameY / 18;
					int num16 = Main.TileSet[i, j].FrameX / 18;
					if (num16 > 1)
					{
						num16 -= 2;
					}
					num16 = i - num16;
					NoWire(num16, num15);
					NoWire(num16, num15 + 1);
					NoWire(num16 + 1, num15);
					NoWire(num16 + 1, num15 + 1);
					if (type == (int)EntityID.TileID.PUMP_IN)
					{
						int num17 = num16;
						int num18 = num15;
						for (int num19 = 0; num19 < 4; num19++)
						{
							if (numInPump >= 19)
							{
								break;
							}
							switch (num19)
							{
							case 0:
								num17 = num16;
								num18 = num15 + 1;
								break;
							case 1:
								num17 = num16 + 1;
								num18 = num15 + 1;
								break;
							case 2:
								num17 = num16;
								num18 = num15;
								break;
							default:
								num17 = num16 + 1;
								num18 = num15;
								break;
							}
							inPump[numInPump].X = (short)num17;
							inPump[numInPump].Y = (short)num18;
							numInPump++;
						}
						break;
					}
					int num20 = num16;
					int num21 = num15;
					for (int num22 = 0; num22 < 4; num22++)
					{
						if (numOutPump >= 19)
						{
							break;
						}
						switch (num22)
						{
						case 0:
							num20 = num16;
							num21 = num15 + 1;
							break;
						case 1:
							num20 = num16 + 1;
							num21 = num15 + 1;
							break;
						case 2:
							num20 = num16;
							num21 = num15;
							break;
						default:
							num20 = num16 + 1;
							num21 = num15;
							break;
						}
						outPump[numOutPump].X = (short)num20;
						outPump[numOutPump].Y = (short)num21;
						numOutPump++;
					}
					break;
				}
				case EntityID.TileID.STATUE:
				{
					int num = j - Main.TileSet[i, j].FrameY / 18;
					int num2 = Main.TileSet[i, j].FrameX / 18;
					int num3 = num2 >> 1;
					num2 = i - (num2 & 1);
					NoWire(num2, num);
					NoWire(num2, num + 1);
					NoWire(num2, num + 2);
					NoWire(num2 + 1, num);
					NoWire(num2 + 1, num + 1);
					NoWire(num2 + 1, num + 2);
					int num4 = num2 * 16 + 16;
					int num5 = (num + 3) * 16;
					int num6 = -1;
					switch (num3)
					{
					case 4:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.SLIME))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.SLIME);
						}
						break;
					case 7:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.CAVE_BAT))
						{
							num6 = NPC.NewNPC(num4 - 4, num5 - 6, (int)EntityID.NPCID.CAVE_BAT);
						}
						break;
					case 8:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.GOLDFISH))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.GOLDFISH);
						}
						break;
					case 9:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.BUNNY))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.BUNNY);
						}
						break;
					case 10:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.SKELETON))
						{
							num6 = NPC.NewNPC(num4, num5, (int)EntityID.NPCID.SKELETON);
						}
						break;
					case 18:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.CRAB))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.CRAB);
						}
						break;
					case 23:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.BLUE_JELLYFISH))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.BLUE_JELLYFISH);
						}
						break;
					case 27:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.MIMIC))
						{
							num6 = NPC.NewNPC(num4 - 9, num5, (int)EntityID.NPCID.MIMIC);
						}
						break;
					case 28:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.BIRD))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.BIRD);
						}
						break;
					case 42:
						if (checkMech(i, j, 30) && NPC.MechSpawn(num4, num5, (int)EntityID.NPCID.PIRANHA))
						{
							num6 = NPC.NewNPC(num4, num5 - 12, (int)EntityID.NPCID.PIRANHA);
						}
						break;
					case 37:
						if (checkMech(i, j, 600) && Item.MechSpawn(num4, num5, (int)EntityID.ItemID.HEART))
						{
							Item.NewItem(num4, num5 - 16, 0, 0, (int)EntityID.ItemID.HEART);
						}
						break;
					case 2:
						if (checkMech(i, j, 600) && Item.MechSpawn(num4, num5, (int)EntityID.ItemID.STAR))
						{
							Item.NewItem(num4, num5 - 16, 0, 0, (int)EntityID.ItemID.STAR);
						}
						break;
					case 17:
						if (checkMech(i, j, 600) && Item.MechSpawn(num4, num5, (int)EntityID.ItemID.BOMB))
						{
							Item.NewItem(num4, num5 - 20, 0, 0, (int)EntityID.ItemID.BOMB);
						}
						break;
					case 40:
					{
						if (!checkMech(i, j, 300))
						{
							break;
						}
						int[] array2 = new int[10];
						int num9 = 0;
						for (int n = 0; n < NPC.MaxNumNPCs; n++) // The 'if' statement below is why the King statue does not affect Santa.
						{
							if (Main.NPCSet[n].Active != 0 && (Main.NPCSet[n].Type == (int)EntityID.NPCID.MERCHANT || Main.NPCSet[n].Type == (int)EntityID.NPCID.ARMS_DEALER || Main.NPCSet[n].Type == (int)EntityID.NPCID.GUIDE || Main.NPCSet[n].Type == (int)EntityID.NPCID.DEMOLITIONIST || Main.NPCSet[n].Type == (int)EntityID.NPCID.CLOTHIER || Main.NPCSet[n].Type == (int)EntityID.NPCID.GOBLIN_TINKERER || Main.NPCSet[n].Type == (int)EntityID.NPCID.WIZARD))
							{
								array2[num9] = n;
								num9++;
								if (num9 >= 9)
								{
									break;
								}
							}
						}
						if (num9 > 0)
						{
							int num10 = array2[Main.Rand.Next(num9)];
							Main.NPCSet[num10].XYWH.X = num4 - (Main.NPCSet[num10].Width >> 1);
							Main.NPCSet[num10].XYWH.Y = num5 - Main.NPCSet[num10].Height - 1;
							Main.NPCSet[num10].Position.X = Main.NPCSet[num10].XYWH.X;
							Main.NPCSet[num10].Position.Y = Main.NPCSet[num10].XYWH.Y;
							NetMessage.CreateMessage1(23, num10);
							NetMessage.SendMessage();
						}
						break;
					}
					case 41:
					{
						if (!checkMech(i, j, 300))
						{
							break;
						}
						int[] array = new int[10];
						int num7 = 0;
						for (int m = 0; m < NPC.MaxNumNPCs; m++)
						{
							if (Main.NPCSet[m].Active != 0 && (Main.NPCSet[m].Type == (int)EntityID.NPCID.NURSE || Main.NPCSet[m].Type == (int)EntityID.NPCID.DRYAD || Main.NPCSet[m].Type == (int)EntityID.NPCID.MECHANIC))
							{
								array[num7] = m;
								num7++;
								if (num7 >= 9)
								{
									break;
								}
							}
						}
						if (num7 > 0)
						{
							int num8 = array[Main.Rand.Next(num7)];
							Main.NPCSet[num8].XYWH.X = num4 - (Main.NPCSet[num8].Width >> 1);
							Main.NPCSet[num8].XYWH.Y = num5 - Main.NPCSet[num8].Height - 1;
							Main.NPCSet[num8].Position.X = Main.NPCSet[num8].XYWH.X;
							Main.NPCSet[num8].Position.Y = Main.NPCSet[num8].XYWH.Y;
							NetMessage.CreateMessage1(23, num8);
							NetMessage.SendMessage();
						}
						break;
					}
					}
					if (num6 >= 0)
					{
						Main.NPCSet[num6].Value = 0f;
						Main.NPCSet[num6].NpcSlots = 0f;
					}
					break;
				}
				}
			}
			hitWire(i - 1, j);
			hitWire(i + 1, j);
			hitWire(i, j - 1);
			hitWire(i, j + 1);
		}
#else
		public unsafe static void wireCheck(int i, int j) // This is the Engine version of the wire handling code, it was added in the 1.0 patch.
		{
			int num = 0;
			int num2 = 1;
			Dir dir = Dir.LEFT;
			wiresChecked[0].X = (short)i;
			wiresChecked[0].Y = (short)j;
			wiresChecked[0].DirsChecked = 0;
			wire[0].X = (short)i;
			wire[0].Y = (short)j;
			bool flag = false;
			while (true)
			{
				switch (dir)
				{
					case Dir.LEFT:
						i--;
						break;
					case Dir.RIGHT:
						i++;
						break;
					case Dir.UP:
						j--;
						break;
					case Dir.DOWN:
						j++;
						break;
				}
				wiresChecked[num].DirsChecked |= (byte)dir;
				if (Main.TileSet[i, j].wire > 0)
				{
					int num3 = num2 - 1;
					flag = false;
					while (true)
					{
						if (num3 < 0)
						{
							if (num2 == MaxNumWire)
							{
								return;
							}
							wire[num2].X = (short)i;
							wire[num2].Y = (short)j;
							num2++;
							num++;
							wiresChecked[num].X = (short)i;
							wiresChecked[num].Y = (short)j;
							switch (dir)
							{
								case Dir.LEFT:
								case Dir.RIGHT:
									wiresChecked[num].DirsChecked = (byte)(dir ^ Dir.LEFT_RIGHT);
									break;
								case Dir.UP:
								case Dir.DOWN:
									wiresChecked[num].DirsChecked = (byte)(dir ^ Dir.UP_DOWN);
									dir = Dir.LEFT;
									break;
							}
							if (Main.TileSet[i, j].IsActive == 0)
							{
								break;
							}
							for (int num4 = numNoWire - 1; num4 >= 0; num4--)
							{
								if (noWire[num4].X == i && noWire[num4].Y == j)
								{
									return;
								}
							}
							int type = Main.TileSet[i, j].Type;
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.TIMER:
									HitSwitch(i, j);
									SquareTileFrame(i, j);
									NetMessage.SendTile(i, j);
									break;
								case EntityID.TileID.ACTIVE_STONE:
									Main.TileSet[i, j].Type = (byte)EntityID.TileID.INACTIVE_STONE;
									SquareTileFrame(i, j);
									NetMessage.SendTile(i, j);
									break;
								case EntityID.TileID.INACTIVE_STONE:
									Main.TileSet[i, j].Type = (byte)EntityID.TileID.ACTIVE_STONE;
									SquareTileFrame(i, j);
									NetMessage.SendTile(i, j);
									break;
								case EntityID.TileID.DOOR_OPEN:
									CloseDoor(i, j, forced: true);
									NetMessage.CreateMessage2(24, i, j);
									NetMessage.SendMessage();
									break;
								case EntityID.TileID.DOOR_CLOSED:
									int direction = (Main.Rand.Next(2) << 1) - 1;
									direction = OpenDoor(i, j, direction);
									if (direction != 0)
									{
										NetMessage.CreateMessage3(19, i, j, direction);
										NetMessage.SendMessage();
									}
									break;
								case EntityID.TileID.TORCH:
									if (Main.TileSet[i, j].FrameX < 66)
									{
										Main.TileSet[i, j].FrameX += 66;
									}
									else
									{
										Main.TileSet[i, j].FrameX -= 66;
									}
									NetMessage.SendTile(i, j);
									break;
								case EntityID.TileID.XMAS_LIGHT:
									if (Main.TileSet[i, j].FrameX < 54)
									{
										Main.TileSet[i, j].FrameX += 54;
									}
									else
									{
										Main.TileSet[i, j].FrameX -= 54;
									}
									NetMessage.SendTile(i, j);
									break;
								case EntityID.TileID.CHAIN_LANTERN:
									int num35 = j - Main.TileSet[i, j].FrameY / 18;
									short num36 = 18;
									if (Main.TileSet[i, j].FrameX > 0)
									{
										num36 = -18;
									}
									Main.TileSet[i, num35].FrameX += num36;
									Main.TileSet[i, num35 + 1].FrameX += num36;
									NoWire(i, num35);
									NoWire(i, num35 + 1);
									NetMessage.SendTileSquare(i, j, 2);
									break;
								case EntityID.TileID.TIKI_TORCH:
									int num25 = j - Main.TileSet[i, j].FrameY / 18;
									short num26 = 18;
									if (Main.TileSet[i, j].FrameX > 0)
									{
										num26 = -18;
									}
									Main.TileSet[i, num25].FrameX += num26;
									Main.TileSet[i, num25 + 1].FrameX += num26;
									Main.TileSet[i, num25 + 2].FrameX += num26;
									NoWire(i, num25);
									NoWire(i, num25 + 1);
									NoWire(i, num25 + 2);
									NetMessage.SendTileSquare(i, num25 + 1, 3);
									break;
								case EntityID.TileID.CHINESE_LANTERN:
								case EntityID.TileID.CANDELABRA:
								case EntityID.TileID.DISCO_BALL:
									{
										int num15 = j - Main.TileSet[i, j].FrameY / 18;
										int num16 = Main.TileSet[i, j].FrameX / 18;
										if (num16 > 1)
										{
											num16 -= 2;
										}
										num16 = i - num16;
										short num17 = 36;
										if (Main.TileSet[num16, num15].FrameX > 0)
										{
											num17 = -36;
										}
										Main.TileSet[num16, num15].FrameX += num17;
										Main.TileSet[num16, num15 + 1].FrameX += num17;
										Main.TileSet[num16 + 1, num15].FrameX += num17;
										Main.TileSet[num16 + 1, num15 + 1].FrameX += num17;
										NoWire(num16, num15);
										NoWire(num16, num15 + 1);
										NoWire(num16 + 1, num15);
										NoWire(num16 + 1, num15 + 1);
										NetMessage.SendTileSquare(num16, num15, 3);
										break;
									}
								case EntityID.TileID.CHANDELIER:
								case EntityID.TileID.JACK_O_LANTERN:
								case EntityID.TileID.PRESENT:
									{
										int num27 = j - Main.TileSet[i, j].FrameY / 18;
										int num28 = Main.TileSet[i, j].FrameX / 18;
										if (num28 > 2)
										{
											num28 -= 3;
										}
										num28 = i - num28;
										short num29 = 54;
										if (Main.TileSet[num28, num27].FrameX > 0)
										{
											num29 = -54;
										}
										for (int num30 = num28; num30 < num28 + 3; num30++)
										{
											for (int num31 = num27; num31 < num27 + 3; num31++)
											{
												Main.TileSet[num30, num31].FrameX += num29;
												NoWire(num30, num31);
											}
										}
										NetMessage.SendTileSquare(num28 + 1, num27 + 1, 3);
										break;
									}
								case EntityID.TileID.CANDLE:
									short num24 = 18;
									if (Main.TileSet[i, j].FrameX > 0)
									{
										num24 = -18;
									}
									Main.TileSet[i, j].FrameX += num24;
									NetMessage.SendTileSquare(i, j, 3);
									break;
								case EntityID.TileID.LAMP_POST:
									int num37 = j - Main.TileSet[i, j].FrameY / 18;
									short num38 = 18;
									if (Main.TileSet[i, j].FrameX > 0)
									{
										num38 = -18;
									}
									Main.TileSet[i, num37].FrameX += num38;
									Main.TileSet[i, num37 + 1].FrameX += num38;
									Main.TileSet[i, num37 + 2].FrameX += num38;
									Main.TileSet[i, num37 + 3].FrameX += num38;
									Main.TileSet[i, num37 + 4].FrameX += num38;
									Main.TileSet[i, num37 + 5].FrameX += num38;
									NoWire(i, num37);
									NoWire(i, num37 + 1);
									NoWire(i, num37 + 2);
									NoWire(i, num37 + 3);
									NoWire(i, num37 + 4);
									NoWire(i, num37 + 5);
									NetMessage.SendTileSquare(i, num37 + 3, 7);
									break;
								case EntityID.TileID.TRAP:
									if (checkMech(i, j, 180))
									{
										int num32 = ((Main.TileSet[i, j].FrameX != 0) ? 1 : (-1));
										float speedX = 12 * num32;
										int num33 = i * 16 + 8 + 10 * num32;
										int num34 = j * 16 + 9;
										Projectile.NewProjectile(num33, num34, speedX, 0f, (int)EntityID.ProjectileID.POISON_DART, 20, 2f);
									}
									break;
								case EntityID.TileID.MUSIC_BOX:
									SwitchMB(i, j);
									break;
								case EntityID.TileID.EXPLOSIVES:
									KillTile(i, j, KillToFail: false, EffectOnly: false, noItem: true);
									NetMessage.SendTile(i, j);
									Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0f, 0f, (int)EntityID.ProjectileID.EXPLOSIVES, 250, 10f);
									break;
								case EntityID.TileID.PUMP_IN:
								case EntityID.TileID.PUMP_OUT:
									int num18 = j - Main.TileSet[i, j].FrameY / 18;
									int num19 = Main.TileSet[i, j].FrameX / 18;
									if (num19 > 1)
									{
										num19 -= 2;
									}
									num19 = i - num19;
									NoWire(num19, num18);
									NoWire(num19, num18 + 1);
									NoWire(num19 + 1, num18);
									NoWire(num19 + 1, num18 + 1);
									if (type == (int)EntityID.TileID.PUMP_IN)
									{
										int num20 = num19;
										int num21 = num18;
										for (int m = 0; m < 4; m++)
										{
											if (numInPump >= 19)
											{
												break;
											}
											switch (m)
											{
												case 0:
													num20 = num19;
													num21 = num18 + 1;
													break;
												case 1:
													num20 = num19 + 1;
													num21 = num18 + 1;
													break;
												case 2:
													num20 = num19;
													num21 = num18;
													break;
												default:
													num20 = num19 + 1;
													num21 = num18;
													break;
											}
											inPump[numInPump].X = (short)num20;
											inPump[numInPump].Y = (short)num21;
											numInPump++;
										}
										break;
									}
									int num22 = num19;
									int num23 = num18;
									for (int n = 0; n < 4; n++)
									{
										if (numOutPump >= 19)
										{
											break;
										}
										switch (n)
										{
											case 0:
												num22 = num19;
												num23 = num18 + 1;
												break;
											case 1:
												num22 = num19 + 1;
												num23 = num18 + 1;
												break;
											case 2:
												num22 = num19;
												num23 = num18;
												break;
											default:
												num22 = num19 + 1;
												num23 = num18;
												break;
										}
										outPump[numOutPump].X = (short)num22;
										outPump[numOutPump].Y = (short)num23;
										numOutPump++;
									}
									break;
								case EntityID.TileID.STATUE:
									int num5 = j - Main.TileSet[i, j].FrameY / 18;
									int num6 = Main.TileSet[i, j].FrameX / 18;
									int num7 = num6 >> 1;
									num6 = i - (num6 & 1);
									NoWire(num6, num5);
									NoWire(num6, num5 + 1);
									NoWire(num6, num5 + 2);
									NoWire(num6 + 1, num5);
									NoWire(num6 + 1, num5 + 1);
									NoWire(num6 + 1, num5 + 2);
									int num8 = num6 * 16 + 16;
									int num9 = (num5 + 3) * 16;
									int num10 = -1;
									switch (num7)
									{
										case 4:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.SLIME))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.SLIME);
											}
											break;
										case 7:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.CAVE_BAT))
											{
												num10 = NPC.NewNPC(num8 - 4, num9 - 6, (int)EntityID.NPCID.CAVE_BAT);
											}
											break;
										case 8:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.GOLDFISH))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.GOLDFISH);
											}
											break;
										case 9:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.BUNNY))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.BUNNY);
											}
											break;
										case 10:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.SKELETON))
											{
												num10 = NPC.NewNPC(num8, num9, (int)EntityID.NPCID.SKELETON);
											}
											break;
										case 18:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.CRAB))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.CRAB);
											}
											break;
										case 23:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.BLUE_JELLYFISH))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.BLUE_JELLYFISH);
											}
											break;
										case 27:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.MIMIC))
											{
												num10 = NPC.NewNPC(num8 - 9, num9, (int)EntityID.NPCID.MIMIC);
											}
											break;
										case 28:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.BIRD))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.BIRD);
											}
											break;
										case 42:
											if (checkMech(i, j, 30) && NPC.MechSpawn(num8, num9, (int)EntityID.NPCID.PIRANHA))
											{
												num10 = NPC.NewNPC(num8, num9 - 12, (int)EntityID.NPCID.PIRANHA);
											}
											break;
										case 37:
											if (checkMech(i, j, 600) && Item.MechSpawn(num8, num9, (int)EntityID.ItemID.HEART))
											{
												Item.NewItem(num8, num9 - 16, 0, 0, (int)EntityID.ItemID.HEART);
											}
											break;
										case 2:
											if (checkMech(i, j, 600) && Item.MechSpawn(num8, num9, (int)EntityID.ItemID.STAR))
											{
												Item.NewItem(num8, num9 - 16, 0, 0, (int)EntityID.ItemID.STAR);
											}
											break;
										case 17:
											if (checkMech(i, j, 600) && Item.MechSpawn(num8, num9, (int)EntityID.ItemID.BOMB))
											{
												Item.NewItem(num8, num9 - 20, 0, 0, (int)EntityID.ItemID.BOMB);
											}
											break;
										case 40:
											{
												if (!checkMech(i, j, 300))
												{
													break;
												}
												int[] array2 = new int[8];
												int num13 = 0;
												for (int l = 0; l < NPC.MaxNumNPCs; l++)
												{
													if (Main.NPCSet[l].Active == 0)
													{
														continue;
													}
													int type3 = Main.NPCSet[l].Type;
													if (type3 == (int)EntityID.NPCID.MERCHANT || type3 == (int)EntityID.NPCID.ARMS_DEALER || type3 == (int)EntityID.NPCID.GUIDE || type3 == (int)EntityID.NPCID.DEMOLITIONIST || type3 == (int)EntityID.NPCID.CLOTHIER || type3 == (int)EntityID.NPCID.GOBLIN_TINKERER || type3 == (int)EntityID.NPCID.WIZARD || type3 == (int)EntityID.NPCID.SANTA_CLAUS)
													{
														array2[num13] = l;
														if (++num13 == 8)
														{
															break;
														}
													}
												}
												if (num13 > 0)
												{
													int num14 = array2[Main.Rand.Next(num13)];
													Main.NPCSet[num14].XYWH.X = num8 - (Main.NPCSet[num14].Width >> 1);
													Main.NPCSet[num14].XYWH.Y = num9 - Main.NPCSet[num14].Height - 1;
													Main.NPCSet[num14].Position.X = Main.NPCSet[num14].XYWH.X;
													Main.NPCSet[num14].Position.Y = Main.NPCSet[num14].XYWH.Y;
													NetMessage.CreateMessage1(23, num14);
													NetMessage.SendMessage();
												}
												break;
											}
										case 41:
											{
												if (!checkMech(i, j, 300))
												{
													break;
												}
												int[] array = new int[4];
												int num11 = 0;
												for (int k = 0; k < NPC.MaxNumNPCs; k++)
												{
													if (Main.NPCSet[k].Active == 0)
													{
														continue;
													}
													int type2 = Main.NPCSet[k].Type;
													if (type2 == (int)EntityID.NPCID.NURSE || type2 == (int)EntityID.NPCID.DRYAD || type2 == (int)EntityID.NPCID.MECHANIC)
													{
														array[num11] = k;
														if (++num11 == 4)
														{
															break;
														}
													}
												}
												if (num11 > 0)
												{
													int num12 = array[Main.Rand.Next(num11)];
													Main.NPCSet[num12].XYWH.X = num8 - (Main.NPCSet[num12].Width >> 1);
													Main.NPCSet[num12].XYWH.Y = num9 - Main.NPCSet[num12].Height - 1;
													Main.NPCSet[num12].Position.X = Main.NPCSet[num12].XYWH.X;
													Main.NPCSet[num12].Position.Y = Main.NPCSet[num12].XYWH.Y;
													NetMessage.CreateMessage1(23, num12);
													NetMessage.SendMessage();
												}
												break;
											}
									}
									if (num10 >= 0)
									{
										Main.NPCSet[num10].Value = 0f;
										Main.NPCSet[num10].NpcSlots = 0f;
									}
									break;
							}
							break;
						}
						if (wire[num3].X != i || wire[num3].Y != j)
						{
							num3--;
							continue;
						}

						flag = true;
						break;
					}

					if (flag)
					{
						continue;
					}
				}
				switch (dir)
				{
					case Dir.LEFT:
						i++;
						break;
					case Dir.RIGHT:
						i--;
						break;
					case Dir.UP:
						j++;
						break;
					case Dir.DOWN:
						j--;
						break;
				}
				while (true)
				{
					dir = (Dir)wiresChecked[num].DirsChecked;
					dir = ~dir & (dir + 1);
					if (dir <= Dir.DOWN)
					{
						break;
					}
					if (--num < 0)
					{
						return;
					}
					i = wiresChecked[num].X;
					j = wiresChecked[num].Y;
				}
			}
		}
#endif

		public unsafe static bool CanKillTile(int i, int j)
		{
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (ptr->IsActive != 0 && j >= 1)
				{
					Tile* ptr2 = ptr - 1;
					if (ptr2->IsActive != 0)
					{
						int type = ptr->Type;
						int type2 = ptr2->Type;
						switch ((EntityID.TileID)type2)
						{
							case EntityID.TileID.TREE:
								if (type != type2)
								{
									return (ptr[-1].FrameX == 66 && ptr[-1].FrameY >= 0 && ptr[-1].FrameY <= 44) || (ptr[-1].FrameX == 88 && ptr[-1].FrameY >= 66 && ptr[-1].FrameY <= 110) || ptr[-1].FrameY >= 198;
								}
								return true;
							case EntityID.TileID.LIFE_CRYSTAL:
							case EntityID.TileID.CHEST:
							case EntityID.TileID.DEMON_ALTAR:
							case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
								return type == type2;
						}
					}
				}
			}
			return true;
		}

		public static bool CanKillWall(int i, int j)
		{
			int wall = Main.TileSet[i, j].WallType;
			if (Main.WallHouse[wall])
			{
				return true;
			}
			for (int k = i - 1; k < i + 2; k++)
			{
				for (int l = j - 1; l < j + 2; l++)
				{
					if (Main.TileSet[k, l].WallType != wall)
					{
						return true;
					}
				}
			}
			return false;
		}

		public unsafe static void KillWall(int i, int j, bool fail = false)
		{
			if (i < 0 || j < 0 || i >= Main.MaxTilesX || j >= Main.MaxTilesY)
			{
				return;
			}
			int wall = Main.TileSet[i, j].WallType;
			if (wall <= 0)
			{
				return;
			}
			int type = 0;
			int type2 = 0;
			switch ((EntityID.WallID)wall)
			{
				case EntityID.WallID.STONE:
				case EntityID.WallID.GREY_BRICK:
				case EntityID.WallID.RED_BRICK:
				case EntityID.WallID.BLUE_DUNGEON_UNSAFE:
				case EntityID.WallID.GREEN_DUNGEON_UNSAFE:
				case EntityID.WallID.PINK_DUNGEON_UNSAFE:
					type = 1;
					break;
				case EntityID.WallID.WOOD:
					type = 7;
					break;
				case EntityID.WallID.COPPER_BRICK:
					type = 9;
					break;
				case EntityID.WallID.GOLD_BRICK:
				case EntityID.WallID.SILVER_BRICK:
					type = wall;
					break;
				case EntityID.WallID.GLASS:
					type2 = 13;
					type = 13;
					break;
				case EntityID.WallID.PEARLSTONE_BRICK:
				case EntityID.WallID.PEARLSTONE_BRICK_UNSAFE:
					type = 51;
					break;
				case EntityID.WallID.IRIDESCENT_BRICK:
					type = 38;
					break;
				case EntityID.WallID.MUDSTONE_BRICK:
					type = 36;
					break;
				case EntityID.WallID.COBALT_BRICK:
					type = 48;
					break;
				case EntityID.WallID.MYTHRIL_BRICK:
				case EntityID.WallID.GREEN_CANDY_CANE:
					type = 49;
					break;
				case EntityID.WallID.CANDY_CANE:
					type = 50;
					break;
				case EntityID.WallID.SNOW_BRICK:
					type = 51;
					break;
#if VERSION_101
				case EntityID.WallID.PURPLE_STAINED_GLASS:
				case EntityID.WallID.YELLOW_STAINED_GLASS:
				case EntityID.WallID.BLUE_STAINED_GLASS:
				case EntityID.WallID.GREEN_STAINED_GLASS:
				case EntityID.WallID.RED_STAINED_GLASS:
				case EntityID.WallID.RAINBOW_STAINED_GLASS:
					type = 86 + wall - (byte)EntityID.WallID.PURPLE_STAINED_GLASS;
					if (wall == (byte)EntityID.WallID.RAINBOW_STAINED_GLASS)
					{
						type = genRand.Next(88, 94);
					}
					break;
#endif
			}
			Main.PlaySound(type2, i * 16, j * 16);
			for (int num = (fail ? 1 : 5); num >= 0; num--)
			{
				switch ((EntityID.WallID)wall)
				{
					case EntityID.WallID.EBONSTONE_UNSAFE:
						type = 1 + 13 * genRand.Next(2);
						break;
					case EntityID.WallID.PLANKED:
						type = 1 + 6 * genRand.Next(2);
						break;
				}
				Main.DustSet.NewDust(i * 16, j * 16, 16, 16, type);
			}
			if (!fail)
			{
				EntityID.ItemID num2 = 0;
				switch ((EntityID.WallID)wall)
				{
					case EntityID.WallID.STONE:
						num2 = EntityID.ItemID.STONE_WALL;
						break;
					case EntityID.WallID.WOOD:
						num2 = EntityID.ItemID.WOOD_WALL;
						break;
					case EntityID.WallID.GREY_BRICK:
						num2 = EntityID.ItemID.GRAY_BRICK_WALL;
						break;
					case EntityID.WallID.RED_BRICK:
						num2 = EntityID.ItemID.RED_BRICK_WALL;
						break;
					case EntityID.WallID.BLUE_DUNGEON_UNSAFE:
						num2 = EntityID.ItemID.BLUE_BRICK_WALL;
						break;
					case EntityID.WallID.GREEN_DUNGEON_UNSAFE:
						num2 = EntityID.ItemID.GREEN_BRICK_WALL;
						break;
					case EntityID.WallID.PINK_DUNGEON_UNSAFE:
						num2 = EntityID.ItemID.PINK_BRICK_WALL;
						break;
					case EntityID.WallID.GOLD_BRICK:
						num2 = EntityID.ItemID.GOLD_BRICK_WALL;
						break;
					case EntityID.WallID.SILVER_BRICK:
						num2 = EntityID.ItemID.SILVER_BRICK_WALL;
						break;
					case EntityID.WallID.COPPER_BRICK:
						num2 = EntityID.ItemID.COPPER_BRICK_WALL;
						break;
					case EntityID.WallID.OBSIDIAN_BRICK_UNSAFE:
						num2 = EntityID.ItemID.OBSIDIAN_BRICK_WALL;
						break;
					case EntityID.WallID.DIRT:
						num2 = EntityID.ItemID.DIRT_WALL;
						break;
					case EntityID.WallID.BLUE_DUNGEON:
						num2 = EntityID.ItemID.BLUE_BRICK_WALL;
						break;
					case EntityID.WallID.GREEN_DUNGEON:
						num2 = EntityID.ItemID.GREEN_BRICK_WALL;
						break;
					case EntityID.WallID.PINK_DUNGEON:
						num2 = EntityID.ItemID.PINK_BRICK_WALL;
						break;
					case EntityID.WallID.OBSIDIAN_BRICK:
						num2 = EntityID.ItemID.OBSIDIAN_BRICK_WALL;
						break;
					case EntityID.WallID.GLASS:
						num2 = EntityID.ItemID.GLASS_WALL;
						break;
					case EntityID.WallID.PEARLSTONE_BRICK:
						num2 = EntityID.ItemID.PEARLSTONE_BRICK_WALL;
						break;
					case EntityID.WallID.IRIDESCENT_BRICK:
						num2 = EntityID.ItemID.IRIDESCENT_BRICK_WALL;
						break;
					case EntityID.WallID.MUDSTONE_BRICK:
						num2 = EntityID.ItemID.MUDSTONE_BRICK_WALL;
						break;
					case EntityID.WallID.COBALT_BRICK:
						num2 = EntityID.ItemID.COBALT_BRICK_WALL;
						break;
					case EntityID.WallID.MYTHRIL_BRICK:
						num2 = EntityID.ItemID.MYTHRIL_BRICK_WALL;
						break;
					case EntityID.WallID.PLANKED:
						num2 = EntityID.ItemID.PLANKED_WALL;
						break;
					case EntityID.WallID.CANDY_CANE:
						num2 = EntityID.ItemID.CANDY_CANE_WALL;
						break;
					case EntityID.WallID.GREEN_CANDY_CANE:
						num2 = EntityID.ItemID.GREEN_CANDY_CANE_WALL;
						break;
					case EntityID.WallID.SNOW_BRICK:
						num2 = EntityID.ItemID.SNOW_BRICK_WALL;
						break;
#if VERSION_101
					case EntityID.WallID.PURPLE_STAINED_GLASS:
					case EntityID.WallID.YELLOW_STAINED_GLASS:
					case EntityID.WallID.BLUE_STAINED_GLASS:
					case EntityID.WallID.GREEN_STAINED_GLASS:
					case EntityID.WallID.RED_STAINED_GLASS:
					case EntityID.WallID.RAINBOW_STAINED_GLASS:
						num2 = (EntityID.ItemID)((int)EntityID.ItemID.PURPLE_STAINED_GLASS_WALL + wall - (byte)EntityID.WallID.PURPLE_STAINED_GLASS);
						break;
#endif
				}
				if ((int)num2 > 0)
				{
					Item.NewItem(i * 16, j * 16, 16, 16, (int)num2);
				}
				Main.TileSet[i, j].WallType = (byte)EntityID.WallID.NONE;
				SquareWallFrame(i, j);
			}
		}

		public unsafe static void KillTileFast(int i, int j)
		{
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (ptr->IsActive == 0)
				{
					return;
				}
				int type = ptr->Type;
				if (j >= 1)
				{
					Tile* ptr2 = ptr - 1;
					if (ptr2->IsActive != 0)
					{
						int type2 = ptr2->Type;
						switch ((EntityID.TileID)type2)
						{
							case EntityID.TileID.TREE:
								if (type != type2 && (ptr[-1].FrameX != 66 || ptr[-1].FrameY < 0 || ptr[-1].FrameY > 44) && (ptr[-1].FrameX != 88 || ptr[-1].FrameY < 66 || ptr[-1].FrameY > 110) && ptr[-1].FrameY < 198)
								{
									return;
								}
								break;
							case EntityID.TileID.LIFE_CRYSTAL:
							case EntityID.TileID.CHEST:
							case EntityID.TileID.DEMON_ALTAR:
							case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
								if (type != type2)
								{
									return;
								}
								break;
						}
					}
				}
				switch ((EntityID.TileID)type)
				{
					case EntityID.TileID.MANNEQUIN:
						{
							int num2 = i;
							int frameX = ptr->FrameX;
							int num3 = ptr->FrameX % 100 % 36;
							if (num3 == 18)
							{
								frameX = ptr[-(Main.LargeWorldH)].FrameX;
								num2--;
							}
							if (frameX >= 100)
							{
								int num4 = frameX / 100;
								frameX %= 100;
								switch (Main.TileSet[num2, j].FrameY / 18)
								{
									case 0:
										Item.NewItem(i * 16, j * 16, 16, 16, Item.HeadType[num4]);
										break;
									case 1:
										Item.NewItem(i * 16, j * 16, 16, 16, Item.BodyType[num4]);
										break;
									case 2:
										Item.NewItem(i * 16, j * 16, 16, 16, Item.LegType[num4]);
										break;
								}
								frameX = Main.TileSet[num2, j].FrameX % 100;
								Main.TileSet[num2, j].FrameX = (short)frameX;
							}
							break;
						}
					case EntityID.TileID.CHEST:
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							int num = ptr->FrameX / 18;
							int y = j - ptr->FrameY / 18;
							num = i - (num & 1);
							if (!Chest.DestroyChest(num, y))
							{
								return;
							}
						}
						break;
				}
				ptr->IsActive = 0;
				ptr->Type = 0;
				ptr->FrameX = -1;
				ptr->FrameY = -1;
				ptr->frameNumber = 0;
				if (type == (byte)EntityID.TileID.HELLSTONE && j > Main.MaxTilesY - 200)
				{
					ptr->Lava = 32;
					ptr->Liquid = 128;
				}
				SquareTileFrame(i, j);
			}
		}

		public unsafe static bool KillTile(int i, int j)
		{
			if (i >= 0 && j >= 0 && i < Main.MaxTilesX && j < Main.MaxTilesY)
			{
				fixed (Tile* ptr = &Main.TileSet[i, j])
				{
					if (ptr->IsActive != 0)
					{
						int type = ptr->Type;
						if (j >= 1)
						{
							Tile* ptr2 = ptr - 1;
							if (ptr2->IsActive != 0)
							{
								int type2 = ptr2->Type;
								switch ((EntityID.TileID)type2)
								{
									case EntityID.TileID.TREE:
										if (type != type2 && (ptr[-1].FrameX != 66 || ptr[-1].FrameY < 0 || ptr[-1].FrameY > 44) && (ptr[-1].FrameX != 88 || ptr[-1].FrameY < 66 || ptr[-1].FrameY > 110) && ptr[-1].FrameY < 198)
										{
											return false;
										}
										break;
									case EntityID.TileID.LIFE_CRYSTAL:
									case EntityID.TileID.CHEST:
									case EntityID.TileID.DEMON_ALTAR:
									case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
										if (type != type2)
										{
											return false;
										}
										break;
								}
							}
						}
						if (!Gen && !stopDrops)
						{
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.STONE:
								case EntityID.TileID.IRON_ORE:
								case EntityID.TileID.COPPER_ORE:
								case EntityID.TileID.GOLD_ORE:
								case EntityID.TileID.SILVER_ORE:
								case EntityID.TileID.DEMONITE_ORE:
								case EntityID.TileID.EBONSTONE:
								case EntityID.TileID.METEORITE:
								case EntityID.TileID.GRAY_BRICK:
								case EntityID.TileID.RED_BRICK:
								case EntityID.TileID.BLUE_BRICK:
								case EntityID.TileID.GREEN_BRICK:
								case EntityID.TileID.PINK_BRICK:
								case EntityID.TileID.GOLD_BRICK:
								case EntityID.TileID.SILVER_BRICK:
								case EntityID.TileID.COPPER_BRICK:
								case EntityID.TileID.SPIKE:
								case EntityID.TileID.OBSIDIAN:
								case EntityID.TileID.HELLSTONE:
								case EntityID.TileID.SAPPHIRE:
								case EntityID.TileID.RUBY:
								case EntityID.TileID.EMERALD:
								case EntityID.TileID.TOPAZ:
								case EntityID.TileID.AMETHYST:
								case EntityID.TileID.DIAMOND:
								case EntityID.TileID.OBSIDIAN_BRICK:
								case EntityID.TileID.HELLSTONE_BRICK:
								case EntityID.TileID.COBALT_ORE:
								case EntityID.TileID.MYTHRIL_ORE:
								case EntityID.TileID.ADAMANTITE_ORE:
								case EntityID.TileID.PEARLSTONE:
								case EntityID.TileID.PEARLSTONE_BRICK:
								case EntityID.TileID.IRIDESCENT_BRICK:
								case EntityID.TileID.MUDSTONE:
								case EntityID.TileID.COBALT_BRICK:
								case EntityID.TileID.MYTHRIL_BRICK:
								case EntityID.TileID.DEMONITE_BRICK:
									Main.PlaySound(21, i * 16, j * 16);
									break;
								case EntityID.TileID.SHORT_GRASS_PLANTS:
								case EntityID.TileID.SHORT_HALLOWED_PLANTS:
									Main.PlaySound(6, i * 16, j * 16);
									if (ptr->FrameX == 144)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.MUSHROOM);
									}
									break;
								case EntityID.TileID.SHORT_CORRUPT_PLANTS:
									Main.PlaySound(6, i * 16, j * 16);
									if (ptr->FrameX == 144)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.VILE_MUSHROOM);
									}
									break;
								case EntityID.TileID.CORRUPTION_THORN:
								case EntityID.TileID.COBWEB:
								case EntityID.TileID.VINE:
								case EntityID.TileID.SHORT_JUNGLE_PLANTS:
								case EntityID.TileID.JUNGLE_VINE:
								case EntityID.TileID.JUNGLE_THORN:
								case EntityID.TileID.GLOWING_MUSHROOM:
								case EntityID.TileID.TALL_GRASS_PLANTS:
								case EntityID.TileID.TALL_JUNGLE_PLANTS:
								case EntityID.TileID.DAYBLOOM_GROWING:
								case EntityID.TileID.DAYBLOOM_MATURE:
								case EntityID.TileID.DAYBLOOM_BLOOMING:
								case EntityID.TileID.TALL_HALLOWED_PLANTS:
								case EntityID.TileID.HALLOWED_VINE:
									Main.PlaySound(6, i * 16, j * 16);
									break;
								case EntityID.TileID.ICE:
									Main.PlaySound(2, i * 16, j * 16, 27);
									break;
								case EntityID.TileID.BOULDER:
									break;
								default:
									Main.PlaySound(0, i * 16, j * 16);
									if (type == (int)EntityID.TileID.CRYSTAL_SHARD)
									{
										Main.PlaySound(2, i * 16, j * 16, 27);
									}
									break;
							}
						}
						if (type != (int)EntityID.TileID.BOULDER)
						{
							if (type == (int)EntityID.TileID.MANNEQUIN)
							{
								int num = i;
								int frameX = ptr->FrameX;
								int num2 = ptr->FrameX % 100 % 36;
								if (num2 == 18)
								{
									frameX = ptr[-(Main.LargeWorldH)].FrameX;
									num--;
								}
								if (frameX >= 100)
								{
									int num3 = frameX / 100;
									frameX %= 100;
									switch (Main.TileSet[num, j].FrameY / 18)
									{
										case 0:
											Item.NewItem(i * 16, j * 16, 16, 16, Item.HeadType[num3]);
											break;
										case 1:
											Item.NewItem(i * 16, j * 16, 16, 16, Item.BodyType[num3]);
											break;
										case 2:
											Item.NewItem(i * 16, j * 16, 16, 16, Item.LegType[num3]);
											break;
									}
									frameX = Main.TileSet[num, j].FrameX % 100;
									Main.TileSet[num, j].FrameX = (short)frameX;
								}
							}
							if (!Gen)
							{
								int num4 = 0;
								if (type != 0)
								{
									switch ((EntityID.TileID)type)
									{
										case EntityID.TileID.STONE:
										case EntityID.TileID.ANVIL:
										case EntityID.TileID.FURNACE:
										case EntityID.TileID.GRAY_BRICK:
										case EntityID.TileID.RED_BRICK:
										case EntityID.TileID.BLUE_BRICK:
										case EntityID.TileID.GREEN_BRICK:
										case EntityID.TileID.PINK_BRICK:
										case EntityID.TileID.SPIKE:
										case EntityID.TileID.SAPPHIRE:
										case EntityID.TileID.RUBY:
										case EntityID.TileID.EMERALD:
										case EntityID.TileID.TOPAZ:
										case EntityID.TileID.AMETHYST:
										case EntityID.TileID.DIAMOND:
										case EntityID.TileID.TOMBSTONE:
										case EntityID.TileID.BATHTUB:
										case EntityID.TileID.LAMP_POST:
										case EntityID.TileID.COOKING_POT:
										case EntityID.TileID.SAFE:
										case EntityID.TileID.TRASH_CAN:
										case EntityID.TileID.STATUE:
										case EntityID.TileID.PEARLSTONE:
										case EntityID.TileID.ACTIVE_STONE:
										case EntityID.TileID.INACTIVE_STONE:
										case EntityID.TileID.LEVER:
										case EntityID.TileID.PRESSURE_PLATE:
										case EntityID.TileID.TRAP:
										case EntityID.TileID.PUMP_IN:
										case EntityID.TileID.PUMP_OUT:
										case EntityID.TileID.TIMER:
											num4 = 1;
											break;
										case EntityID.TileID.CANDLE:
										case EntityID.TileID.CHINESE_LANTERN:
										case EntityID.TileID.SKULL_LANTERN:
										case EntityID.TileID.CANDELABRA:
#if VERSION_101
										case EntityID.TileID.CAMPFIRE:
#endif
											num4 = 6;
											break;
										case EntityID.TileID.TREE:
										case EntityID.TileID.DOOR_CLOSED:
										case EntityID.TileID.DOOR_OPEN:
										case EntityID.TileID.TABLE:
										case EntityID.TileID.CHAIR:
										case EntityID.TileID.PLATFORM:
										case EntityID.TileID.WOOD:
										case EntityID.TileID.LOOM:
										case EntityID.TileID.PIANO:
										case EntityID.TileID.DRESSER:
										case EntityID.TileID.BENCH:
										case EntityID.TileID.TIKI_TORCH:
										case EntityID.TileID.KEG:
										case EntityID.TileID.GRANDFATHERS_CLOCK:
										case EntityID.TileID.SAWMILL:
										case EntityID.TileID.TINKERERS_WORKSHOP:
										case EntityID.TileID.WOODEN_BEAM:
										case EntityID.TileID.MANNEQUIN:
										case EntityID.TileID.MUSIC_BOX:
											num4 = 7;
											break;
										case EntityID.TileID.CHEST:
											num4 = ((ptr->FrameX < 108) ? ((ptr->FrameX < 36) ? 7 : 10) : 37);
											break;
										case EntityID.TileID.ICE:
											num4 = 67;
											break;
										case EntityID.TileID.BANNER:
											num4 = -1;
											break;
										case EntityID.TileID.IRON_ORE:
										case EntityID.TileID.DEMON_ALTAR:
											num4 = 8;
											break;
										case EntityID.TileID.COPPER_ORE:
										case EntityID.TileID.CHANDELIER:
										case EntityID.TileID.COPPER_BRICK:
											num4 = 9;
											break;
										case EntityID.TileID.GOLD_ORE:
										case EntityID.TileID.PRESENT:
										case EntityID.TileID.GOLD_BRICK:
										case EntityID.TileID.THRONE:
											num4 = 10;
											break;
										case EntityID.TileID.SILVER_ORE:
										case EntityID.TileID.JACK_O_LANTERN:
										case EntityID.TileID.CHAIN_LANTERN:
										case EntityID.TileID.SILVER_BRICK:
										case EntityID.TileID.DISCO_BALL:
										case EntityID.TileID.SWITCH:
											num4 = 11;
											break;
										case EntityID.TileID.LIFE_CRYSTAL:
											num4 = 12;
											break;
										case EntityID.TileID.SHORT_GRASS_PLANTS:
										case EntityID.TileID.TALL_GRASS_PLANTS:
										case EntityID.TileID.VINE:
											num4 = 3;
											break;
										case EntityID.TileID.BOTTLE:
										case EntityID.TileID.GLASS:
											num4 = 13;
											break;
										case EntityID.TileID.DEMONITE_ORE:
										case EntityID.TileID.DEMONITE_BRICK:
										case EntityID.TileID.EBONSAND:
											num4 = 14;
											break;
										case EntityID.TileID.POT:
										case EntityID.TileID.CLAY_POT:
										case EntityID.TileID.BOOK:
											num4 = 22;
											break;
										case EntityID.TileID.PIGGYBANK:
											num4 = 23;
											break;
										case EntityID.TileID.CLAY:
										case EntityID.TileID.BOWL:
											num4 = 28;
											break;
										case EntityID.TileID.WATER_CANDLE:
											num4 = 29;
											break;
										case EntityID.TileID.COBWEB:
											num4 = 30;
											break;
										case EntityID.TileID.SAND:
										case EntityID.TileID.CORAL:
											num4 = 32;
											break;
										case EntityID.TileID.OBSIDIAN:
										case EntityID.TileID.OBSIDIAN_BRICK:
											num4 = 37;
											break;
										case EntityID.TileID.ASH:
										case EntityID.TileID.IRIDESCENT_BRICK:
										case EntityID.TileID.EXPLOSIVES:
											num4 = 36;
											break;
										case EntityID.TileID.MUD:
										case EntityID.TileID.MUDSTONE:
											num4 = 38;
											break;
										case EntityID.TileID.SHORT_JUNGLE_PLANTS:
										case EntityID.TileID.JUNGLE_VINE:
										case EntityID.TileID.TALL_JUNGLE_PLANTS:
										case EntityID.TileID.CACTUS:
											num4 = 40;
											break;
										case EntityID.TileID.JUNGLE_THORN:
											num4 = 7;
											break;
										case EntityID.TileID.GLOWING_MUSHROOM:
										case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
											num4 = 26;
											break;
										case EntityID.TileID.MUSHROOM_GRASS:
											num4 = 17;
											break;
										case EntityID.TileID.SILT:
											num4 = 53;
											break;
										case EntityID.TileID.PEARLSAND:
										case EntityID.TileID.PEARLSTONE_BRICK:
										case EntityID.TileID.SNOW:
										case EntityID.TileID.SNOW_BRICK:
											num4 = 51;
											break;
										case EntityID.TileID.SHORT_HALLOWED_PLANTS:
										case EntityID.TileID.TALL_HALLOWED_PLANTS:
										case EntityID.TileID.HALLOWED_VINE:
											num4 = 47;
											break;
										case EntityID.TileID.COBALT_ORE:
										case EntityID.TileID.COBALT_BRICK:
											num4 = 48;
											break;
										case EntityID.TileID.MYTHRIL_ORE:
										case EntityID.TileID.MYTHRIL_BRICK:
										case EntityID.TileID.MYTHRIL_ANVIL:
										case EntityID.TileID.CANDY_CANE_GREEN:
										case EntityID.TileID.XMAS_LIGHT:
											num4 = 49;
											break;
										case EntityID.TileID.ADAMANTITE_ORE:
										case EntityID.TileID.ADAMANTITE_FORGE:
										case EntityID.TileID.CANDY_CANE_RED:
											num4 = 50;
											break;
										case EntityID.TileID.DAYBLOOM_GROWING:
										case EntityID.TileID.DAYBLOOM_MATURE:
										case EntityID.TileID.DAYBLOOM_BLOOMING:
											switch (ptr->FrameX / 18)
											{
												case 0:
													num4 = 3;
													break;
												case 1:
													num4 = 3;
													break;
												case 2:
													num4 = 7;
													break;
												case 3:
													num4 = 17;
													break;
												case 4:
													num4 = 3;
													break;
												case 5:
													num4 = 6;
													break;
											}
											break;
										default:
											switch ((EntityID.TileID)type)
											{
												case EntityID.TileID.CRYSTAL_SHARD:
													num4 = ((ptr->FrameX != 0 && ptr->FrameX != 54 && ptr->FrameX != 108) ? ((ptr->FrameX != 18 && ptr->FrameX != 72 && ptr->FrameX != 126) ? 70 : 69) : 68);
													break;
												case EntityID.TileID.TORCH:
													int num5 = ptr->FrameY / 22;
													switch (num5)
													{
														case 0:
															num4 = 6;
															break;
														case 8:
															num4 = 75;
															break;
														default:
															num4 = 58 + num5;
															break;
													}
													break;
											}
											break;
									}
								}
								if (num4 >= 0)
								{
									for (int num6 = 4; num6 >= 0; num6--)
									{
										switch ((EntityID.TileID)type)
										{
											case EntityID.TileID.GRASS:
												num4 = genRand.Next(2) << 1;
												break;
											case EntityID.TileID.SAPLING:
												num4 = ((genRand.Next(2) == 0) ? 7 : 2);
												break;
											case EntityID.TileID.CORRUPT_GRASS:
											case EntityID.TileID.SHORT_CORRUPT_PLANTS:
												num4 = ((genRand.Next(2) == 0) ? 14 : 17);
												break;
											case EntityID.TileID.SUNFLOWER:
												num4 = ((genRand.Next(2) == 0) ? 3 : 19);
												break;
											case EntityID.TileID.EBONSTONE:
											case EntityID.TileID.SHADOW_ORB:
												num4 = ((genRand.Next(2) != 0) ? 1 : 14);
												break;
											case EntityID.TileID.CORRUPTION_THORN:
												num4 = ((genRand.Next(2) == 0) ? 14 : 24);
												break;
											case EntityID.TileID.CHANDELIER:
											case EntityID.TileID.JACK_O_LANTERN:
											case EntityID.TileID.PRESENT:
											case EntityID.TileID.CHAIN_LANTERN:
												num4 = genRand.Next(2) * 6;
												break;
											case EntityID.TileID.METEORITE:
												num4 = ((genRand.Next(2) == 0) ? 6 : 23);
												break;
											case EntityID.TileID.SHORT_JUNGLE_PLANTS:
												num4 = 38 + genRand.Next(2);
												break;
											case EntityID.TileID.HELLSTONE:
											case EntityID.TileID.HELLSTONE_BRICK:
											case EntityID.TileID.HELLFORGE:
												num4 = ((genRand.Next(2) == 0) ? 6 : 25);
												break;
											case EntityID.TileID.HALLOWED_GRASS:
												num4 = genRand.Next(2) * 47;
												break;
										}
										Main.DustSet.NewDust(i * 16, j * 16, 16, 16, num4);
									}
								}
							}
						}
						if (type == (int)EntityID.TileID.CHEST && Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							int num7 = ptr->FrameX / 18;
							int y = j - ptr->FrameY / 18;
							num7 = i - (num7 & 1);
							if (!Chest.DestroyChest(num7, y))
							{
								return false;
							}
						}
						if (!Gen && !stopDrops && Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							int num8 = (int)EntityID.ItemID.NONE;
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.DIRT:
								case EntityID.TileID.GRASS:
								case EntityID.TileID.HALLOWED_GRASS:
									num8 = (int)EntityID.ItemID.DIRT_BLOCK;
									break;
								case EntityID.TileID.STONE:
									num8 = (int)EntityID.ItemID.STONE_BLOCK;
									break;
								case EntityID.TileID.SHORT_GRASS_PLANTS:
								case EntityID.TileID.TALL_GRASS_PLANTS:
									if (Main.Rand.Next(2) == 0)
									{
										Rectangle rect = default;
										rect.X = i << 4;
										rect.Y = j << 4;
										rect.Width = (rect.Height = 16);
										if (Player.FindClosest(ref rect).HasItem((int)EntityID.ItemID.BLOWPIPE))
										{
											num8 = (int)EntityID.ItemID.SEED;
										}
									}
									break;
								case EntityID.TileID.TORCH:
									int num9 = ptr->FrameY / 22;
									switch (num9)
									{
										case 0:
											num8 = (int)EntityID.ItemID.TORCH;
											break;
										case 8:
											num8 = (int)EntityID.ItemID.CURSED_TORCH;
											break;
										default:
											num8 = (int)EntityID.ItemID.BLUE_TORCH - 1 + num9;
											break;
									}
									break;
								case EntityID.TileID.TREE:
									if (ptr->FrameX >= 22 && ptr->FrameY >= 198)
									{
										if (Main.NetMode != (byte)NetModeSetting.CLIENT)
										{
											num8 = (int)EntityID.ItemID.WOOD;
											if (genRand.Next(2) == 0)
											{
												int num10 = j - 1;
												int type3;
												do
												{
													type3 = Main.TileSet[i, ++num10].Type;
												}
												while (!Main.TileSolidNotSolidTop[type3] || Main.TileSet[i, num10].IsActive == 0);
												if (type3 == (int)EntityID.TileID.GRASS || type3 == (int)EntityID.TileID.HALLOWED_GRASS)
												{
													num8 = (int)EntityID.ItemID.ACORN;
												}
											}
										}
									}
									else
									{
										num8 = (int)EntityID.ItemID.WOOD;
									}
									woodSpawned++;
									break;
								case EntityID.TileID.IRON_ORE:
									num8 = (int)EntityID.ItemID.IRON_ORE;
									break;
								case EntityID.TileID.COPPER_ORE:
									num8 = (int)EntityID.ItemID.COPPER_ORE;
									break;
								case EntityID.TileID.GOLD_ORE:
									num8 = (int)EntityID.ItemID.GOLD_ORE;
									break;
								case EntityID.TileID.SILVER_ORE:
									num8 = (int)EntityID.ItemID.SILVER_ORE;
									break;
								case EntityID.TileID.SILT:
									num8 = (int)EntityID.ItemID.SILT_BLOCK;
									break;
								case EntityID.TileID.WOODEN_BEAM:
									num8 = (int)EntityID.ItemID.WOODEN_BEAM;
									break;
								case EntityID.TileID.XMAS_LIGHT:
									if (ptr->FrameX == 0 || ptr->FrameX == 54)
									{
										num8 = (int)EntityID.ItemID.BLUE_LIGHT;
									}
									else if (ptr->FrameX == 18 || ptr->FrameX == 72)
									{
										num8 = (int)EntityID.ItemID.RED_LIGHT;
									}
									else if (ptr->FrameX == 36 || ptr->FrameX == 90)
									{
										num8 = (int)EntityID.ItemID.GREEN_LIGHT;
									}
									break;
								case EntityID.TileID.BOTTLE:
									Main.PlaySound(13, i * 16, j * 16);
									switch (ptr->FrameX)
									{
										case 18:
											num8 = (int)EntityID.ItemID.LESSER_HEALING_POTION;
											break;
										case 36:
											num8 = (int)EntityID.ItemID.LESSER_MANA_POTION;
											break;
										case 54:
											num8 = (int)EntityID.ItemID.PINK_VASE;
											break;
										case 72:
											num8 = (int)EntityID.ItemID.MUG;
											break;
										default:
											num8 = (int)EntityID.ItemID.BOTTLE;
											break;
									}
									break;
								case EntityID.TileID.PLATFORM:
									num8 = (int)EntityID.ItemID.WOOD_PLATFORM;
									break;
								case EntityID.TileID.DEMONITE_ORE:
									num8 = (int)EntityID.ItemID.DEMONITE_ORE;
									break;
								case EntityID.TileID.DEMONITE_BRICK:
									num8 = (int)EntityID.ItemID.DEMONITE_BRICK;
									break;
								case EntityID.TileID.CORRUPT_GRASS:
									num8 = (int)EntityID.ItemID.DIRT_BLOCK;
									break;
								case EntityID.TileID.EBONSTONE:
									num8 = (int)EntityID.ItemID.EBONSTONE_BLOCK;
									break;
								case EntityID.TileID.WOOD:
									num8 = (int)EntityID.ItemID.WOOD;
									break;
								case EntityID.TileID.CANDLE:
									num8 = (int)EntityID.ItemID.CANDLE;
									break;
								case EntityID.TileID.METEORITE:
									num8 = (int)EntityID.ItemID.METEORITE;
									break;
								case EntityID.TileID.GRAY_BRICK:
									num8 = (int)EntityID.ItemID.GRAY_BRICK;
									break;
								case EntityID.TileID.RED_BRICK:
									num8 = (int)EntityID.ItemID.RED_BRICK;
									break;
								case EntityID.TileID.CLAY:
									num8 = (int)EntityID.ItemID.CLAY_BLOCK;
									break;
								case EntityID.TileID.BLUE_BRICK:
									num8 = (int)EntityID.ItemID.BLUE_BRICK;
									break;
								case EntityID.TileID.GREEN_BRICK:
									num8 = (int)EntityID.ItemID.GREEN_BRICK;
									break;
								case EntityID.TileID.PINK_BRICK:
									num8 = (int)EntityID.ItemID.PINK_BRICK;
									break;
								case EntityID.TileID.GOLD_BRICK:
									num8 = (int)EntityID.ItemID.GOLD_BRICK;
									break;
								case EntityID.TileID.SILVER_BRICK:
									num8 = (int)EntityID.ItemID.SILVER_BRICK;
									break;
								case EntityID.TileID.COPPER_BRICK:
									num8 = (int)EntityID.ItemID.COPPER_BRICK;
									break;
								case EntityID.TileID.SPIKE:
									num8 = (int)EntityID.ItemID.SPIKE;
									break;
								case EntityID.TileID.WATER_CANDLE:
									num8 = (int)EntityID.ItemID.WATER_CANDLE;
									break;
								case EntityID.TileID.COBWEB:
									num8 = (int)EntityID.ItemID.COBWEB;
									break;
								case EntityID.TileID.SAND:
									num8 = (int)EntityID.ItemID.SAND_BLOCK;
									break;
								case EntityID.TileID.GLASS:
									num8 = (int)EntityID.ItemID.GLASS;
									Main.PlaySound(13, i * 16, j * 16);
									break;
								case EntityID.TileID.OBSIDIAN:
									num8 = (int)EntityID.ItemID.OBSIDIAN;
									break;
								case EntityID.TileID.ASH:
									num8 = (int)EntityID.ItemID.ASH_BLOCK;
									break;
								case EntityID.TileID.HELLSTONE:
									num8 = (int)EntityID.ItemID.HELLSTONE;
									break;
								case EntityID.TileID.MUSHROOM_GRASS:
									num8 = (int)EntityID.ItemID.MUD_BLOCK;
									break;
								case EntityID.TileID.OBSIDIAN_BRICK:
									num8 = (int)EntityID.ItemID.OBSIDIAN_BRICK;
									break;
								case EntityID.TileID.HELLSTONE_BRICK:
									num8 = (int)EntityID.ItemID.HELLSTONE_BRICK;
									break;
								case EntityID.TileID.CLAY_POT:
									num8 = (int)EntityID.ItemID.CLAY_POT;
									break;
								case EntityID.TileID.CORAL:
									num8 = (int)EntityID.ItemID.CORAL;
									break;
								case EntityID.TileID.CACTUS:
									num8 = (int)EntityID.ItemID.CACTUS;
									break;
								case EntityID.TileID.COBALT_ORE:
									num8 = (int)EntityID.ItemID.COBALT_ORE;
									break;
								case EntityID.TileID.MYTHRIL_ORE:
									num8 = (int)EntityID.ItemID.MYTHRIL_ORE;
									break;
								case EntityID.TileID.ADAMANTITE_ORE:
									num8 = (int)EntityID.ItemID.ADAMANTITE_ORE;
									break;
								case EntityID.TileID.EBONSAND:
									num8 = (int)EntityID.ItemID.EBONSAND_BLOCK;
									break;
								case EntityID.TileID.PEARLSAND:
									num8 = (int)EntityID.ItemID.PEARLSAND_BLOCK;
									break;
								case EntityID.TileID.PEARLSTONE:
									num8 = (int)EntityID.ItemID.PEARLSTONE_BLOCK;
									break;
								case EntityID.TileID.PEARLSTONE_BRICK:
									num8 = (int)EntityID.ItemID.PEARLSTONE_BRICK;
									break;
								case EntityID.TileID.IRIDESCENT_BRICK:
									num8 = (int)EntityID.ItemID.IRIDESCENT_BRICK;
									break;
								case EntityID.TileID.MUDSTONE:
									num8 = (int)EntityID.ItemID.MUDSTONE_BRICK;
									break;
								case EntityID.TileID.COBALT_BRICK:
									num8 = (int)EntityID.ItemID.COBALT_BRICK;
									break;
								case EntityID.TileID.MYTHRIL_BRICK:
									num8 = (int)EntityID.ItemID.MYTHRIL_BRICK;
									break;
								case EntityID.TileID.CRYSTAL_SHARD:
									num8 = (int)EntityID.ItemID.CRYSTAL_SHARD;
									break;
								case EntityID.TileID.ACTIVE_STONE:
									num8 = (int)EntityID.ItemID.ACTIVE_STONE_BLOCK;
									break;
								case EntityID.TileID.INACTIVE_STONE:
									num8 = (int)EntityID.ItemID.INACTIVE_STONE_BLOCK;
									break;
								case EntityID.TileID.SWITCH:
									num8 = (int)EntityID.ItemID.SWITCH;
									break;
								case EntityID.TileID.TRAP:
									num8 = (int)EntityID.ItemID.DART_TRAP;
									break;
								case EntityID.TileID.EXPLOSIVES:
									num8 = (int)EntityID.ItemID.EXPLOSIVES;
									break;
								case EntityID.TileID.CANDY_CANE_RED:
									num8 = (int)EntityID.ItemID.CANDY_CANE_BLOCK;
									break;
								case EntityID.TileID.CANDY_CANE_GREEN:
									num8 = (int)EntityID.ItemID.GREEN_CANDY_CANE_BLOCK;
									break;
								case EntityID.TileID.SNOW:
									num8 = (int)EntityID.ItemID.SNOW_BLOCK;
									break;
								case EntityID.TileID.SNOW_BRICK:
									num8 = (int)EntityID.ItemID.SNOW_BRICK;
									break;
								case EntityID.TileID.PRESSURE_PLATE:
									if (ptr->FrameY == 0)
									{
										num8 = (int)EntityID.ItemID.RED_PRESSURE_PLATE;
									}
									else if (ptr->FrameY == 18)
									{
										num8 = (int)EntityID.ItemID.GREEN_PRESSURE_PLATE;
									}
									else if (ptr->FrameY == 36)
									{
										num8 = (int)EntityID.ItemID.GRAY_PRESSURE_PLATE;
									}
									else if (ptr->FrameY == 54)
									{
										num8 = (int)EntityID.ItemID.BROWN_PRESSURE_PLATE;
									}
									break;
								case EntityID.TileID.TIMER:
									if (ptr->FrameX == 0)
									{
										num8 = (int)EntityID.ItemID.ONE_SECOND_TIMER;
									}
									else if (ptr->FrameX == 18)
									{
										num8 = (int)EntityID.ItemID.THREE_SECOND_TIMER;
									}
									else if (ptr->FrameX == 36)
									{
										num8 = (int)EntityID.ItemID.FIVE_SECOND_TIMER;
									}
									break;
								case EntityID.TileID.SHORT_JUNGLE_PLANTS:
								case EntityID.TileID.TALL_JUNGLE_PLANTS:
									if (ptr->FrameX == 144)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.JUNGLE_SPORES, genRand.Next(2, 4));
									}
									else if (ptr->FrameX == 162)
									{
										num8 = (int)EntityID.ItemID.NATURES_GIFT;
									}
									else if (ptr->FrameX >= 108 && ptr->FrameX <= 126 && genRand.Next(100) == 0)
									{
										num8 = (int)EntityID.ItemID.JUNGLE_ROSE;
									}
									else if (genRand.Next(100) == 0)
									{
										num8 = (int)EntityID.ItemID.JUNGLE_GRASS_SEEDS;
									}
									break;
								case EntityID.TileID.MUD:
								case EntityID.TileID.JUNGLE_GRASS:
									num8 = (int)EntityID.ItemID.MUD_BLOCK;
									break;
								case EntityID.TileID.GLOWING_MUSHROOM:
								case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
									{
										int num12 = genRand.Next(50);
										if (num12 < 25)
										{
											num8 = ((num12 != 0) ? (int)EntityID.ItemID.GLOWING_MUSHROOM : (int)EntityID.ItemID.MUSHROOM_GRASS_SEEDS);
										}
										break;
									}
								default:
									if (ptr->Type >= (byte)EntityID.TileID.SAPPHIRE && ptr->Type <= (byte)EntityID.TileID.DIAMOND)
									{
										num8 = ptr->Type - (byte)EntityID.TileID.SAPPHIRE + (int)EntityID.ItemID.SAPPHIRE;
										break;
									}
									switch ((EntityID.TileID)type)
									{
										case EntityID.TileID.BOOK:
											num8 = ((ptr->FrameX != 90) ? (int)EntityID.ItemID.BOOK : (int)EntityID.ItemID.WATER_BOLT);
											break;
										case EntityID.TileID.DAYBLOOM_MATURE:
										case EntityID.TileID.DAYBLOOM_BLOOMING:
											{
												int num11 = ptr->FrameX / 18;
												bool flag = type == (int)EntityID.TileID.DAYBLOOM_BLOOMING;
												if (!flag)
												{
													if (num11 == 0 && Main.GameTime.DayTime)
													{
														flag = true;
													}
													else if (num11 == 1 && !Main.GameTime.DayTime)
													{
														flag = true;
													}
													else if (num11 == 3 && Main.GameTime.IsBloodMoon)
													{
														flag = true;
													}
												}
												if (flag)
												{
													Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.DAYBLOOM_SEEDS + num11, genRand.Next(1, 4));
												}
												num8 = (int)EntityID.ItemID.DAYBLOOM + num11;
												break;
											}
									}
									break;
							}
							if (num8 > 0)
							{
								Item.NewItem(i * 16, j * 16, 16, 16, num8, 1, DoNotBroadcast: false, -1);
							}
						}
						ptr->IsActive = 0;
						ptr->Type = 0;
						ptr->FrameX = -1;
						ptr->FrameY = -1;
						ptr->frameNumber = 0;
						if (type == (int)EntityID.TileID.HELLSTONE && j > Main.MaxTilesY - 200)
						{
							ptr->Lava = 32;
							ptr->Liquid = 128;
						}
						SquareTileFrame(i, j);
						return true;
					}
				}
			}
			return false;
		}

		public unsafe static void KillTile(int i, int j, bool KillToFail, bool EffectOnly = false, bool noItem = false)
		{
			if (i < 0 || j < 0 || i >= Main.MaxTilesX || j >= Main.MaxTilesY)
			{
				return;
			}
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (ptr->IsActive == 0)
				{
					return;
				}
				int type = ptr->Type;
				if (j >= 1)
				{
					Tile* ptr2 = ptr - 1;
					if (ptr2->IsActive != 0)
					{
						int type2 = ptr2->Type;
						switch ((EntityID.TileID)type2)
						{
							case EntityID.TileID.TREE:
								if (type != type2 && (ptr[-1].FrameX != 66 || ptr[-1].FrameY < 0 || ptr[-1].FrameY > 44) && (ptr[-1].FrameX != 88 || ptr[-1].FrameY < 66 || ptr[-1].FrameY > 110) && ptr[-1].FrameY < 198)
								{
									return;
								}
								break;
							case EntityID.TileID.LIFE_CRYSTAL:
							case EntityID.TileID.CHEST:
							case EntityID.TileID.DEMON_ALTAR:
							case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
								if (type != type2)
								{
									return;
								}
								break;
						}
					}
				}
				if (!Gen && !EffectOnly && !stopDrops)
				{
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.STONE:
						case EntityID.TileID.IRON_ORE:
						case EntityID.TileID.COPPER_ORE:
						case EntityID.TileID.GOLD_ORE:
						case EntityID.TileID.SILVER_ORE:
						case EntityID.TileID.DEMONITE_ORE:
						case EntityID.TileID.EBONSTONE:
						case EntityID.TileID.METEORITE:
						case EntityID.TileID.GRAY_BRICK:
						case EntityID.TileID.RED_BRICK:
						case EntityID.TileID.BLUE_BRICK:
						case EntityID.TileID.GREEN_BRICK:
						case EntityID.TileID.PINK_BRICK:
						case EntityID.TileID.GOLD_BRICK:
						case EntityID.TileID.SILVER_BRICK:
						case EntityID.TileID.COPPER_BRICK:
						case EntityID.TileID.SPIKE:
						case EntityID.TileID.OBSIDIAN:
						case EntityID.TileID.HELLSTONE:
						case EntityID.TileID.SAPPHIRE:
						case EntityID.TileID.RUBY:
						case EntityID.TileID.EMERALD:
						case EntityID.TileID.TOPAZ:
						case EntityID.TileID.AMETHYST:
						case EntityID.TileID.DIAMOND:
						case EntityID.TileID.OBSIDIAN_BRICK:
						case EntityID.TileID.HELLSTONE_BRICK:
						case EntityID.TileID.COBALT_ORE:
						case EntityID.TileID.MYTHRIL_ORE:
						case EntityID.TileID.ADAMANTITE_ORE:
						case EntityID.TileID.PEARLSTONE:
						case EntityID.TileID.PEARLSTONE_BRICK:
						case EntityID.TileID.IRIDESCENT_BRICK:
						case EntityID.TileID.MUDSTONE:
						case EntityID.TileID.COBALT_BRICK:
						case EntityID.TileID.MYTHRIL_BRICK:
						case EntityID.TileID.DEMONITE_BRICK:
							Main.PlaySound(21, i * 16, j * 16);
							break;
						case EntityID.TileID.SHORT_GRASS_PLANTS:
						case EntityID.TileID.SHORT_HALLOWED_PLANTS:
							Main.PlaySound(6, i * 16, j * 16);
							if (ptr->FrameX == 144)
							{
								Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.MUSHROOM);
							}
							break;
						case EntityID.TileID.SHORT_CORRUPT_PLANTS:
							Main.PlaySound(6, i * 16, j * 16);
							if (ptr->FrameX == 144)
							{
								Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.VILE_MUSHROOM);
							}
							break;
						case EntityID.TileID.CORRUPTION_THORN:
						case EntityID.TileID.COBWEB:
						case EntityID.TileID.VINE:
						case EntityID.TileID.SHORT_JUNGLE_PLANTS:
						case EntityID.TileID.JUNGLE_VINE:
						case EntityID.TileID.JUNGLE_THORN:
						case EntityID.TileID.GLOWING_MUSHROOM:
						case EntityID.TileID.TALL_GRASS_PLANTS:
						case EntityID.TileID.TALL_JUNGLE_PLANTS:
						case EntityID.TileID.DAYBLOOM_GROWING:
						case EntityID.TileID.DAYBLOOM_MATURE:
						case EntityID.TileID.DAYBLOOM_BLOOMING:
						case EntityID.TileID.TALL_HALLOWED_PLANTS:
						case EntityID.TileID.HALLOWED_VINE:
							Main.PlaySound(6, i * 16, j * 16);
							break;
						case EntityID.TileID.ICE:
							Main.PlaySound(2, i * 16, j * 16, 27);
							break;
						case EntityID.TileID.BOULDER:
							break;
						default:
							Main.PlaySound(0, i * 16, j * 16);
							if (type == (int)EntityID.TileID.CRYSTAL_SHARD && !KillToFail)
							{
								Main.PlaySound(2, i * 16, j * 16, 27);
							}
							break;
					}
				}
				if (type != (int)EntityID.TileID.BOULDER)
				{
					if (type == (int)EntityID.TileID.MANNEQUIN)
					{
						int num = i;
						int frameX = ptr->FrameX;
						int num2 = ptr->FrameX % 100 % 36;
						if (num2 == 18)
						{
							frameX = ptr[-(Main.LargeWorldH)].FrameX;
							num--;
						}
						if (frameX >= 100)
						{
							int num3 = frameX / 100;
							frameX %= 100;
							switch (Main.TileSet[num, j].FrameY / 18)
							{
								case 0:
									Item.NewItem(i * 16, j * 16, 16, 16, Item.HeadType[num3]);
									break;
								case 1:
									Item.NewItem(i * 16, j * 16, 16, 16, Item.BodyType[num3]);
									break;
								case 2:
									Item.NewItem(i * 16, j * 16, 16, 16, Item.LegType[num3]);
									break;
							}
							frameX = Main.TileSet[num, j].FrameX % 100;
							Main.TileSet[num, j].FrameX = (short)frameX;
						}
					}
					if (!Gen)
					{
						int num4 = 0;
						if (type != 0)
						{
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.STONE:
								case EntityID.TileID.ANVIL:
								case EntityID.TileID.FURNACE:
								case EntityID.TileID.GRAY_BRICK:
								case EntityID.TileID.RED_BRICK:
								case EntityID.TileID.BLUE_BRICK:
								case EntityID.TileID.GREEN_BRICK:
								case EntityID.TileID.PINK_BRICK:
								case EntityID.TileID.SPIKE:
								case EntityID.TileID.SAPPHIRE:
								case EntityID.TileID.RUBY:
								case EntityID.TileID.EMERALD:
								case EntityID.TileID.TOPAZ:
								case EntityID.TileID.AMETHYST:
								case EntityID.TileID.DIAMOND:
								case EntityID.TileID.TOMBSTONE:
								case EntityID.TileID.BATHTUB:
								case EntityID.TileID.LAMP_POST:
								case EntityID.TileID.COOKING_POT:
								case EntityID.TileID.SAFE:
								case EntityID.TileID.TRASH_CAN:
								case EntityID.TileID.STATUE:
								case EntityID.TileID.PEARLSTONE:
								case EntityID.TileID.ACTIVE_STONE:
								case EntityID.TileID.INACTIVE_STONE:
								case EntityID.TileID.LEVER:
								case EntityID.TileID.PRESSURE_PLATE:
								case EntityID.TileID.TRAP:
								case EntityID.TileID.PUMP_IN:
								case EntityID.TileID.PUMP_OUT:
								case EntityID.TileID.TIMER:
									num4 = 1;
									break;
								case EntityID.TileID.CANDLE:
								case EntityID.TileID.CHINESE_LANTERN:
								case EntityID.TileID.SKULL_LANTERN:
								case EntityID.TileID.CANDELABRA:
#if VERSION_101
								case EntityID.TileID.CAMPFIRE:
#endif
									num4 = 6;
									break;
								case EntityID.TileID.TREE:
								case EntityID.TileID.DOOR_CLOSED:
								case EntityID.TileID.DOOR_OPEN:
								case EntityID.TileID.TABLE:
								case EntityID.TileID.CHAIR:
								case EntityID.TileID.PLATFORM:
								case EntityID.TileID.WOOD:
								case EntityID.TileID.LOOM:
								case EntityID.TileID.PIANO:
								case EntityID.TileID.DRESSER:
								case EntityID.TileID.BENCH:
								case EntityID.TileID.TIKI_TORCH:
								case EntityID.TileID.KEG:
								case EntityID.TileID.GRANDFATHERS_CLOCK:
								case EntityID.TileID.SAWMILL:
								case EntityID.TileID.TINKERERS_WORKSHOP:
								case EntityID.TileID.WOODEN_BEAM:
								case EntityID.TileID.MANNEQUIN:
								case EntityID.TileID.MUSIC_BOX:
									num4 = 7;
									break;
								case EntityID.TileID.CHEST:
									num4 = ((ptr->FrameX < 108) ? ((ptr->FrameX < 36) ? 7 : 10) : 37);
									break;
								case EntityID.TileID.ICE:
									num4 = 67;
									break;
								case EntityID.TileID.BANNER:
									num4 = -1;
									break;
								case EntityID.TileID.IRON_ORE:
								case EntityID.TileID.DEMON_ALTAR:
									num4 = 8;
									break;
								case EntityID.TileID.COPPER_ORE:
								case EntityID.TileID.CHANDELIER:
								case EntityID.TileID.COPPER_BRICK:
									num4 = 9;
									break;
								case EntityID.TileID.GOLD_ORE:
								case EntityID.TileID.PRESENT:
								case EntityID.TileID.GOLD_BRICK:
								case EntityID.TileID.THRONE:
									num4 = 10;
									break;
								case EntityID.TileID.SILVER_ORE:
								case EntityID.TileID.JACK_O_LANTERN:
								case EntityID.TileID.CHAIN_LANTERN:
								case EntityID.TileID.SILVER_BRICK:
								case EntityID.TileID.DISCO_BALL:
								case EntityID.TileID.SWITCH:
									num4 = 11;
									break;
								case EntityID.TileID.LIFE_CRYSTAL:
									num4 = 12;
									break;
								case EntityID.TileID.SHORT_GRASS_PLANTS:
								case EntityID.TileID.TALL_GRASS_PLANTS:
								case EntityID.TileID.VINE:
									num4 = 3;
									break;
								case EntityID.TileID.BOTTLE:
								case EntityID.TileID.GLASS:
									num4 = 13;
									break;
								case EntityID.TileID.DEMONITE_ORE:
								case EntityID.TileID.DEMONITE_BRICK:
								case EntityID.TileID.EBONSAND:
									num4 = 14;
									break;
								case EntityID.TileID.POT:
								case EntityID.TileID.CLAY_POT:
								case EntityID.TileID.BOOK:
									num4 = 22;
									break;
								case EntityID.TileID.PIGGYBANK:
									num4 = 23;
									break;
								case EntityID.TileID.CLAY:
								case EntityID.TileID.BOWL:
									num4 = 28;
									break;
								case EntityID.TileID.WATER_CANDLE:
									num4 = 29;
									break;
								case EntityID.TileID.COBWEB:
									num4 = 30;
									break;
								case EntityID.TileID.SAND:
								case EntityID.TileID.CORAL:
									num4 = 32;
									break;
								case EntityID.TileID.OBSIDIAN:
								case EntityID.TileID.OBSIDIAN_BRICK:
									num4 = 37;
									break;
								case EntityID.TileID.ASH:
								case EntityID.TileID.IRIDESCENT_BRICK:
								case EntityID.TileID.EXPLOSIVES:
									num4 = 36;
									break;
								case EntityID.TileID.MUD:
								case EntityID.TileID.MUDSTONE:
									num4 = 38;
									break;
								case EntityID.TileID.SHORT_JUNGLE_PLANTS:
								case EntityID.TileID.JUNGLE_VINE:
								case EntityID.TileID.TALL_JUNGLE_PLANTS:
								case EntityID.TileID.CACTUS:
									num4 = 40;
									break;
								case EntityID.TileID.JUNGLE_THORN:
									num4 = 7;
									break;
								case EntityID.TileID.GLOWING_MUSHROOM:
								case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
									num4 = 26;
									break;
								case EntityID.TileID.MUSHROOM_GRASS:
									num4 = 17;
									break;
								case EntityID.TileID.SILT:
									num4 = 53;
									break;
								case EntityID.TileID.PEARLSAND:
								case EntityID.TileID.PEARLSTONE_BRICK:
								case EntityID.TileID.SNOW:
								case EntityID.TileID.SNOW_BRICK:
									num4 = 51;
									break;
								case EntityID.TileID.SHORT_HALLOWED_PLANTS:
								case EntityID.TileID.TALL_HALLOWED_PLANTS:
								case EntityID.TileID.HALLOWED_VINE:
									num4 = 47;
									break;
								case EntityID.TileID.COBALT_ORE:
								case EntityID.TileID.COBALT_BRICK:
									num4 = 48;
									break;
								case EntityID.TileID.MYTHRIL_ORE:
								case EntityID.TileID.MYTHRIL_BRICK:
								case EntityID.TileID.MYTHRIL_ANVIL:
								case EntityID.TileID.CANDY_CANE_GREEN:
								case EntityID.TileID.XMAS_LIGHT:
									num4 = 49;
									break;
								case EntityID.TileID.ADAMANTITE_ORE:
								case EntityID.TileID.ADAMANTITE_FORGE:
								case EntityID.TileID.CANDY_CANE_RED:
									num4 = 50;
									break;
								case EntityID.TileID.DAYBLOOM_GROWING:
								case EntityID.TileID.DAYBLOOM_MATURE:
								case EntityID.TileID.DAYBLOOM_BLOOMING:
									switch (ptr->FrameX / 18)
									{
										case 0:
											num4 = 3;
											break;
										case 1:
											num4 = 3;
											break;
										case 2:
											num4 = 7;
											break;
										case 3:
											num4 = 17;
											break;
										case 4:
											num4 = 3;
											break;
										case 5:
											num4 = 6;
											break;
									}
									break;
								default:
									switch ((EntityID.TileID)type)
									{
										case EntityID.TileID.CRYSTAL_SHARD:
											num4 = ((ptr->FrameX != 0 && ptr->FrameX != 54 && ptr->FrameX != 108) ? ((ptr->FrameX != 18 && ptr->FrameX != 72 && ptr->FrameX != 126) ? 70 : 69) : 68);
											break;
										case EntityID.TileID.TORCH:
											int num5 = ptr->FrameY / 22;
											switch (num5)
											{
												case 0:
													num4 = 6;
													break;
												case 8:
													num4 = 75;
													break;
												default:
													num4 = 58 + num5;
													break;
											}
											break;
									}
									break;
							}
						}
						if (num4 >= 0)
						{
							for (int num6 = (KillToFail ? 1 : 4); num6 >= 0; num6--)
							{
								switch ((EntityID.TileID)type)
								{
									case EntityID.TileID.GRASS:
										num4 = genRand.Next(2) << 1;
										break;
									case EntityID.TileID.SAPLING:
										num4 = ((genRand.Next(2) == 0) ? 7 : 2);
										break;
									case EntityID.TileID.CORRUPT_GRASS:
									case EntityID.TileID.SHORT_CORRUPT_PLANTS:
										num4 = ((genRand.Next(2) == 0) ? 14 : 17);
										break;
									case EntityID.TileID.SUNFLOWER:
										num4 = ((genRand.Next(2) == 0) ? 3 : 19);
										break;
									case EntityID.TileID.EBONSTONE:
									case EntityID.TileID.SHADOW_ORB:
										num4 = ((genRand.Next(2) != 0) ? 1 : 14);
										break;
									case EntityID.TileID.CORRUPTION_THORN:
										num4 = ((genRand.Next(2) == 0) ? 14 : 24);
										break;
									case EntityID.TileID.CHANDELIER:
									case EntityID.TileID.JACK_O_LANTERN:
									case EntityID.TileID.PRESENT:
									case EntityID.TileID.CHAIN_LANTERN:
										num4 = genRand.Next(2) * 6;
										break;
									case EntityID.TileID.METEORITE:
										num4 = ((genRand.Next(2) == 0) ? 6 : 23);
										break;
									case EntityID.TileID.SHORT_JUNGLE_PLANTS:
										num4 = 38 + genRand.Next(2);
										break;
									case EntityID.TileID.HELLSTONE:
									case EntityID.TileID.HELLSTONE_BRICK:
									case EntityID.TileID.HELLFORGE:
										num4 = ((genRand.Next(2) == 0) ? 6 : 25);
										break;
									case EntityID.TileID.HALLOWED_GRASS:
										num4 = genRand.Next(2) * 47;
										break;
								}
								Main.DustSet.NewDust(i * 16, j * 16, 16, 16, num4);
							}
						}
					}
				}
				if (EffectOnly)
				{
					return;
				}
				if (KillToFail)
				{
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.GRASS:
						case EntityID.TileID.CORRUPT_GRASS:
						case EntityID.TileID.HALLOWED_GRASS:
							ptr->Type = (byte)EntityID.TileID.DIRT;
							break;
						case EntityID.TileID.JUNGLE_GRASS:
						case EntityID.TileID.MUSHROOM_GRASS:
							ptr->Type = (byte)EntityID.TileID.MUD;
							break;
					}
					SquareTileFrame(i, j);
					return;
				}
				if (type == (int)EntityID.TileID.CHEST && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					int num7 = ptr->FrameX / 18;
					int y = j - ptr->FrameY / 18;
					num7 = i - (num7 & 1);
					if (!Chest.DestroyChest(num7, y))
					{
						return;
					}
				}
				if (!noItem && !Gen && !stopDrops && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					int num8 = (int)EntityID.ItemID.NONE;
					switch ((EntityID.TileID)type)
					{
						case EntityID.TileID.DIRT:
						case EntityID.TileID.GRASS:
						case EntityID.TileID.HALLOWED_GRASS:
							num8 = (int)EntityID.ItemID.DIRT_BLOCK;
							break;
						case EntityID.TileID.STONE:
							num8 = (int)EntityID.ItemID.STONE_BLOCK;
							break;
						case EntityID.TileID.SHORT_GRASS_PLANTS:
						case EntityID.TileID.TALL_GRASS_PLANTS:
							if (Main.Rand.Next(2) == 0)
							{
								Rectangle rect = default;
								rect.X = i << 4;
								rect.Y = j << 4;
								rect.Width = (rect.Height = 16);
								if (Player.FindClosest(ref rect).HasItem((int)EntityID.ItemID.BLOWPIPE))
								{
									num8 = (int)EntityID.ItemID.SEED;
								}
							}
							break;
						case EntityID.TileID.TORCH:
							int num9 = ptr->FrameY / 22;
							switch (num9)
							{
								case 0:
									num8 = (int)EntityID.ItemID.TORCH;
									break;
								case 8:
									num8 = (int)EntityID.ItemID.CURSED_TORCH;
									break;
								default:
									num8 = (int)EntityID.ItemID.BLUE_TORCH - 1 + num9;
									break;
							}
							break;
						case EntityID.TileID.TREE:
							if (ptr->FrameX >= 22 && ptr->FrameY >= 198)
							{
								if (Main.NetMode != (byte)NetModeSetting.CLIENT)
								{
									num8 = (int)EntityID.ItemID.WOOD;
									if (genRand.Next(2) == 0)
									{
										int num10 = j - 1;
										int type3;
										do
										{
											type3 = Main.TileSet[i, ++num10].Type;
										}
										while (!Main.TileSolidNotSolidTop[type3] || Main.TileSet[i, num10].IsActive == 0);
										if (type3 == (int)EntityID.TileID.GRASS || type3 == (int)EntityID.TileID.HALLOWED_GRASS)
										{
											num8 = (int)EntityID.ItemID.ACORN;
										}
									}
								}
							}
							else
							{
								num8 = (int)EntityID.ItemID.WOOD;
							}
							woodSpawned++;
							break;
						case EntityID.TileID.IRON_ORE:
							num8 = (int)EntityID.ItemID.IRON_ORE;
							break;
						case EntityID.TileID.COPPER_ORE:
							num8 = (int)EntityID.ItemID.COPPER_ORE;
							break;
						case EntityID.TileID.GOLD_ORE:
							num8 = (int)EntityID.ItemID.GOLD_ORE;
							break;
						case EntityID.TileID.SILVER_ORE:
							num8 = (int)EntityID.ItemID.SILVER_ORE;
							break;
						case EntityID.TileID.SILT:
							num8 = (int)EntityID.ItemID.SILT_BLOCK;
							break;
						case EntityID.TileID.WOODEN_BEAM:
							num8 = (int)EntityID.ItemID.WOODEN_BEAM;
							break;
						case EntityID.TileID.XMAS_LIGHT:
							if (ptr->FrameX == 0 || ptr->FrameX == 54)
							{
								num8 = (int)EntityID.ItemID.BLUE_LIGHT;
							}
							else if (ptr->FrameX == 18 || ptr->FrameX == 72)
							{
								num8 = (int)EntityID.ItemID.RED_LIGHT;
							}
							else if (ptr->FrameX == 36 || ptr->FrameX == 90)
							{
								num8 = (int)EntityID.ItemID.GREEN_LIGHT;
							}
							break;
						case EntityID.TileID.BOTTLE:
							Main.PlaySound(13, i * 16, j * 16);
							switch (ptr->FrameX)
							{
								case 18:
									num8 = (int)EntityID.ItemID.LESSER_HEALING_POTION;
									break;
								case 36:
									num8 = (int)EntityID.ItemID.LESSER_MANA_POTION;
									break;
								case 54:
									num8 = (int)EntityID.ItemID.PINK_VASE;
									break;
								case 72:
									num8 = (int)EntityID.ItemID.MUG;
									break;
								default:
									num8 = (int)EntityID.ItemID.BOTTLE;
									break;
							}
							break;
						case EntityID.TileID.PLATFORM:
							num8 = (int)EntityID.ItemID.WOOD_PLATFORM;
							break;
						case EntityID.TileID.DEMONITE_ORE:
							num8 = (int)EntityID.ItemID.DEMONITE_ORE;
							break;
						case EntityID.TileID.DEMONITE_BRICK:
							num8 = (int)EntityID.ItemID.DEMONITE_BRICK;
							break;
						case EntityID.TileID.CORRUPT_GRASS:
							num8 = (int)EntityID.ItemID.DIRT_BLOCK;
							break;
						case EntityID.TileID.EBONSTONE:
							num8 = (int)EntityID.ItemID.EBONSTONE_BLOCK;
							break;
						case EntityID.TileID.WOOD:
							num8 = (int)EntityID.ItemID.WOOD;
							break;
						case EntityID.TileID.CANDLE:
							num8 = (int)EntityID.ItemID.CANDLE;
							break;
						case EntityID.TileID.METEORITE:
							num8 = (int)EntityID.ItemID.METEORITE;
							break;
						case EntityID.TileID.GRAY_BRICK:
							num8 = (int)EntityID.ItemID.GRAY_BRICK;
							break;
						case EntityID.TileID.RED_BRICK:
							num8 = (int)EntityID.ItemID.RED_BRICK;
							break;
						case EntityID.TileID.CLAY:
							num8 = (int)EntityID.ItemID.CLAY_BLOCK;
							break;
						case EntityID.TileID.BLUE_BRICK:
							num8 = (int)EntityID.ItemID.BLUE_BRICK;
							break;
						case EntityID.TileID.GREEN_BRICK:
							num8 = (int)EntityID.ItemID.GREEN_BRICK;
							break;
						case EntityID.TileID.PINK_BRICK:
							num8 = (int)EntityID.ItemID.PINK_BRICK;
							break;
						case EntityID.TileID.GOLD_BRICK:
							num8 = (int)EntityID.ItemID.GOLD_BRICK;
							break;
						case EntityID.TileID.SILVER_BRICK:
							num8 = (int)EntityID.ItemID.SILVER_BRICK;
							break;
						case EntityID.TileID.COPPER_BRICK:
							num8 = (int)EntityID.ItemID.COPPER_BRICK;
							break;
						case EntityID.TileID.SPIKE:
							num8 = (int)EntityID.ItemID.SPIKE;
							break;
						case EntityID.TileID.WATER_CANDLE:
							num8 = (int)EntityID.ItemID.WATER_CANDLE;
							break;
						case EntityID.TileID.COBWEB:
							num8 = (int)EntityID.ItemID.COBWEB;
							break;
						case EntityID.TileID.SAND:
							num8 = (int)EntityID.ItemID.SAND_BLOCK;
							break;
						case EntityID.TileID.GLASS:
							num8 = (int)EntityID.ItemID.GLASS;
							Main.PlaySound(13, i * 16, j * 16);
							break;
						case EntityID.TileID.OBSIDIAN:
							num8 = (int)EntityID.ItemID.OBSIDIAN;
							break;
						case EntityID.TileID.ASH:
							num8 = (int)EntityID.ItemID.ASH_BLOCK;
							break;
						case EntityID.TileID.HELLSTONE:
							num8 = (int)EntityID.ItemID.HELLSTONE;
							break;
						case EntityID.TileID.MUSHROOM_GRASS:
							num8 = (int)EntityID.ItemID.MUD_BLOCK;
							break;
						case EntityID.TileID.OBSIDIAN_BRICK:
							num8 = (int)EntityID.ItemID.OBSIDIAN_BRICK;
							break;
						case EntityID.TileID.HELLSTONE_BRICK:
							num8 = (int)EntityID.ItemID.HELLSTONE_BRICK;
							break;
						case EntityID.TileID.CLAY_POT:
							num8 = (int)EntityID.ItemID.CLAY_POT;
							break;
						case EntityID.TileID.CORAL:
							num8 = (int)EntityID.ItemID.CORAL;
							break;
						case EntityID.TileID.CACTUS:
							num8 = (int)EntityID.ItemID.CACTUS;
							break;
						case EntityID.TileID.COBALT_ORE:
							num8 = (int)EntityID.ItemID.COBALT_ORE;
							break;
						case EntityID.TileID.MYTHRIL_ORE:
							num8 = (int)EntityID.ItemID.MYTHRIL_ORE;
							break;
						case EntityID.TileID.ADAMANTITE_ORE:
							num8 = (int)EntityID.ItemID.ADAMANTITE_ORE;
							break;
						case EntityID.TileID.EBONSAND:
							num8 = (int)EntityID.ItemID.EBONSAND_BLOCK;
							break;
						case EntityID.TileID.PEARLSAND:
							num8 = (int)EntityID.ItemID.PEARLSAND_BLOCK;
							break;
						case EntityID.TileID.PEARLSTONE:
							num8 = (int)EntityID.ItemID.PEARLSTONE_BLOCK;
							break;
						case EntityID.TileID.PEARLSTONE_BRICK:
							num8 = (int)EntityID.ItemID.PEARLSTONE_BRICK;
							break;
						case EntityID.TileID.IRIDESCENT_BRICK:
							num8 = (int)EntityID.ItemID.IRIDESCENT_BRICK;
							break;
						case EntityID.TileID.MUDSTONE:
							num8 = (int)EntityID.ItemID.MUDSTONE_BRICK;
							break;
						case EntityID.TileID.COBALT_BRICK:
							num8 = (int)EntityID.ItemID.COBALT_BRICK;
							break;
						case EntityID.TileID.MYTHRIL_BRICK:
							num8 = (int)EntityID.ItemID.MYTHRIL_BRICK;
							break;
						case EntityID.TileID.CRYSTAL_SHARD:
							num8 = (int)EntityID.ItemID.CRYSTAL_SHARD;
							break;
						case EntityID.TileID.ACTIVE_STONE:
							num8 = (int)EntityID.ItemID.ACTIVE_STONE_BLOCK;
							break;
						case EntityID.TileID.INACTIVE_STONE:
							num8 = (int)EntityID.ItemID.INACTIVE_STONE_BLOCK;
							break;
						case EntityID.TileID.SWITCH:
							num8 = (int)EntityID.ItemID.SWITCH;
							break;
						case EntityID.TileID.TRAP:
							num8 = (int)EntityID.ItemID.DART_TRAP;
							break;
						case EntityID.TileID.EXPLOSIVES:
							num8 = (int)EntityID.ItemID.EXPLOSIVES;
							break;
						case EntityID.TileID.CANDY_CANE_RED:
							num8 = (int)EntityID.ItemID.CANDY_CANE_BLOCK;
							break;
						case EntityID.TileID.CANDY_CANE_GREEN:
							num8 = (int)EntityID.ItemID.GREEN_CANDY_CANE_BLOCK;
							break;
						case EntityID.TileID.SNOW:
							num8 = (int)EntityID.ItemID.SNOW_BLOCK;
							break;
						case EntityID.TileID.SNOW_BRICK:
							num8 = (int)EntityID.ItemID.SNOW_BRICK;
							break;
						case EntityID.TileID.PRESSURE_PLATE:
							if (ptr->FrameY == 0)
							{
								num8 = (int)EntityID.ItemID.RED_PRESSURE_PLATE;
							}
							else if (ptr->FrameY == 18)
							{
								num8 = (int)EntityID.ItemID.GREEN_PRESSURE_PLATE;
							}
							else if (ptr->FrameY == 36)
							{
								num8 = (int)EntityID.ItemID.GRAY_PRESSURE_PLATE;
							}
							else if (ptr->FrameY == 54)
							{
								num8 = (int)EntityID.ItemID.BROWN_PRESSURE_PLATE;
							}
							break;
						case EntityID.TileID.TIMER:
							if (ptr->FrameX == 0)
							{
								num8 = (int)EntityID.ItemID.ONE_SECOND_TIMER;
							}
							else if (ptr->FrameX == 18)
							{
								num8 = (int)EntityID.ItemID.THREE_SECOND_TIMER;
							}
							else if (ptr->FrameX == 36)
							{
								num8 = (int)EntityID.ItemID.FIVE_SECOND_TIMER;
							}
							break;
						case EntityID.TileID.SHORT_JUNGLE_PLANTS:
						case EntityID.TileID.TALL_JUNGLE_PLANTS:
							if (ptr->FrameX == 144)
							{
								Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.JUNGLE_SPORES, genRand.Next(2, 4));
							}
							else if (ptr->FrameX == 162)
							{
								num8 = (int)EntityID.ItemID.NATURES_GIFT;
							}
							else if (ptr->FrameX >= 108 && ptr->FrameX <= 126 && genRand.Next(100) == 0)
							{
								num8 = (int)EntityID.ItemID.JUNGLE_ROSE;
							}
							else if (genRand.Next(100) == 0)
							{
								num8 = (int)EntityID.ItemID.JUNGLE_GRASS_SEEDS;
							}
							break;
						case EntityID.TileID.MUD:
						case EntityID.TileID.JUNGLE_GRASS:
							num8 = (int)EntityID.ItemID.MUD_BLOCK;
							break;
						case EntityID.TileID.GLOWING_MUSHROOM:
						case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
							{
								int num12 = genRand.Next(50);
								if (num12 < 25)
								{
									num8 = ((num12 != 0) ? (int)EntityID.ItemID.GLOWING_MUSHROOM : (int)EntityID.ItemID.MUSHROOM_GRASS_SEEDS);
								}
								break;
							}
						default:
							if (ptr->Type >= (byte)EntityID.TileID.SAPPHIRE && ptr->Type <= (byte)EntityID.TileID.DIAMOND)
							{
								num8 = ptr->Type - (byte)EntityID.TileID.SAPPHIRE + (int)EntityID.ItemID.SAPPHIRE;
								break;
							}
							switch ((EntityID.TileID)type)
							{
								case EntityID.TileID.BOOK:
									num8 = ((ptr->FrameX != 90) ? (int)EntityID.ItemID.BOOK : (int)EntityID.ItemID.WATER_BOLT);
									break;
								case EntityID.TileID.DAYBLOOM_MATURE:
								case EntityID.TileID.DAYBLOOM_BLOOMING:
									{
										int num11 = ptr->FrameX / 18;
										bool flag = type == (int)EntityID.TileID.DAYBLOOM_BLOOMING;
										if (!flag)
										{
											if (num11 == 0 && Main.GameTime.DayTime)
											{
												flag = true;
											}
											else if (num11 == 1 && !Main.GameTime.DayTime)
											{
												flag = true;
											}
											else if (num11 == 3 && Main.GameTime.IsBloodMoon)
											{
												flag = true;
											}
										}
										if (flag)
										{
											Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.DAYBLOOM_SEEDS + num11, genRand.Next(1, 4));
										}
										num8 = (int)EntityID.ItemID.DAYBLOOM + num11;
										break;
									}
							}
							break;
					}
					if (num8 > 0)
					{
						Item.NewItem(i * 16, j * 16, 16, 16, num8, 1, DoNotBroadcast: false, -1);
					}
				}
				ptr->IsActive = 0;
				ptr->Type = 0;
				ptr->FrameX = -1;
				ptr->FrameY = -1;
				ptr->frameNumber = 0;
				if (type == (int)EntityID.TileID.HELLSTONE && j > Main.MaxTilesY - 200)
				{
					ptr->Lava = 32;
					ptr->Liquid = 128;
				}
				SquareTileFrame(i, j);
			}
		}

		public static bool PlayerLOS(int x, int y)
		{
			Rectangle rectangle = new Rectangle(x * 16, y * 16, 16, 16);
			Rectangle value = default;
			bool result = false;
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active != 0)
				{
					value.X = (int)((double)Main.PlayerSet[i].Position.X + (Player.width / 2) - (NPC.SpawnWidth * 0.6f));
					value.Y = (int)((double)Main.PlayerSet[i].Position.Y + (Player.height / 2) - (NPC.SpawnHeight * 0.6f));
					value.Width = (int)(NPC.SpawnWidth * 1.2f);
					value.Height = (int)(NPC.SpawnHeight * 1.2f);
					rectangle.Intersects(ref value, out result);
					if (result)
					{
						break;
					}
				}
			}
			return result;
		}

		public static void hardUpdateWorld(int i, int j)
		{
			int type = Main.TileSet[i, j].Type;
			if (type == (byte)EntityID.TileID.PEARLSTONE && j > Main.RockLayer && Main.Rand.Next(110) == 0)
			{
				int num = genRand.Next(4);
				int num2 = 0;
				int num3 = 0;
				switch (num)
				{
					case 0:
						num2 = -1;
						break;
					case 1:
						num2 = 1;
						break;
					default:
						num3 = ((num != 0) ? 1 : (-1));
						break;
				}
				if (Main.TileSet[i + num2, j + num3].IsActive == 0)
				{
					int num4 = 0;
					int num5 = 6;
					for (int k = i - num5; k <= i + num5; k++)
					{
						for (int l = j - num5; l <= j + num5; l++)
						{
							if (Main.TileSet[k, l].IsActive != 0 && Main.TileSet[k, l].Type == (byte)EntityID.TileID.CRYSTAL_SHARD)
							{
								num4++;
							}
						}
					}
					if (num4 < 2)
					{
						PlaceTile(i + num2, j + num3, (byte)EntityID.TileID.CRYSTAL_SHARD, ToMute: true);
						NetMessage.SendTile(i + num2, j + num3);
					}
				}
			}
			else if (type == (byte)EntityID.TileID.CORRUPT_GRASS || type == (byte)EntityID.TileID.EBONSTONE || type == (byte)EntityID.TileID.CORRUPTION_THORN || type == (byte)EntityID.TileID.EBONSAND)
			{
				while (true)
				{
					int num6 = i + genRand.Next(-3, 4);
					int num7 = j + genRand.Next(-3, 4);

					switch ((EntityID.TileID)Main.TileSet[num6, num7].Type)
					{
						case EntityID.TileID.GRASS:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.CORRUPT_GRASS;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) == 0)
							{
								break;
							}
							return;
						case EntityID.TileID.STONE:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.EBONSTONE;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) == 0)
							{
								break;
							}
							return;
						case EntityID.TileID.SAND:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.EBONSAND;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) != 0)
							{
								return;
							}
							break;
						case EntityID.TileID.MUD:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.DIRT;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) == 0)
							{
								break;
							}
							return;
						case EntityID.TileID.JUNGLE_GRASS:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.CORRUPT_GRASS;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) == 0)
							{
								break;
							}
							return;
						case EntityID.TileID.JUNGLE_THORN:
							Main.TileSet[num6, num7].Type = (byte)EntityID.TileID.CORRUPTION_THORN;
							SquareTileFrame(num6, num7);
							NetMessage.SendTile(num6, num7);
							if (genRand.Next(2) != 0)
							{
								return;
							}
							break;
						default:
							return;
					}
				}
			}
			switch ((EntityID.TileID)type)
			{
				case EntityID.TileID.HALLOWED_GRASS:
				case EntityID.TileID.SHORT_HALLOWED_PLANTS:
				case EntityID.TileID.TALL_HALLOWED_PLANTS:
				case EntityID.TileID.HALLOWED_VINE:
				case EntityID.TileID.PEARLSAND:
				case EntityID.TileID.PEARLSTONE:
				case EntityID.TileID.PEARLSTONE_BRICK:
					break;

				default:
					return;
			}
			while (true)
			{
				int num8 = i + genRand.Next(-3, 4);
				int num9 = j + genRand.Next(-3, 4);
				switch ((EntityID.TileID)Main.TileSet[num8, num9].Type)
				{
					case EntityID.TileID.GRASS:
						Main.TileSet[num8, num9].Type = (byte)EntityID.TileID.HALLOWED_GRASS;
						SquareTileFrame(num8, num9);
						NetMessage.SendTile(num8, num9);
						if (genRand.Next(2) == 0)
						{
							break;
						}
						return;
					case EntityID.TileID.STONE:
						Main.TileSet[num8, num9].Type = (byte)EntityID.TileID.PEARLSTONE;
						SquareTileFrame(num8, num9);
						NetMessage.SendTile(num8, num9);
						if (genRand.Next(2) == 0)
						{
							break;
						}
						return;
					case EntityID.TileID.SAND:
						Main.TileSet[num8, num9].Type = (byte)EntityID.TileID.PEARLSAND;
						SquareTileFrame(num8, num9);
						NetMessage.SendTile(num8, num9);
						if (genRand.Next(2) != 0)
						{
							return;
						}
						break;
					default:
						return;
				}
			}
		}

		public static bool SolidTile(int i, int j)
		{
			try
			{
				return Main.TileSet[i, j].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[i, j].Type];
			}
			catch
			{
				return false;
			}
		}

		public static bool SolidTileUnsafe(int i, int j)
		{
			return Main.TileSet[i, j].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[i, j].Type];
		}

		public static bool CanStandOnTop(int i, int j)
		{
			try
			{
				return Main.TileSet[i, j].CanStandOnTop();
			}
			catch
			{
				return false;
			}
		}

		public unsafe static void MineHouse(int i, int j)
		{
			if (i < 50 || i > Main.MaxTilesX - 50 || j < 50 || j > Main.MaxTilesY - 50 || SolidTileUnsafe(i, j) || Main.TileSet[i, j].WallType > (byte)EntityID.WallID.NONE)
			{
				return;
			}
			int num = genRand.Next(6, 12);
			int num2 = genRand.Next(3, 6);
			int num3 = genRand.Next(15, 30);
			int num4 = genRand.Next(15, 30);
			int num5 = j - num;
			int num6 = j + num2;
			for (int k = 0; k < 2; k++)
			{
				int num7 = i;
				int num8 = j;
				int num9 = -1;
				int num10 = num3;
				if (k == 1)
				{
					num9 = 1;
					num10 = num4;
					num7++;
				}
				do
				{
					if (num8 - num < num5)
					{
						num5 = num8 - num;
					}
					if (num8 + num2 > num6)
					{
						num6 = num8 + num2;
					}
					for (int l = 0; l < 2; l++)
					{
						int num11 = num8;
						int num12 = num;
						int num13 = -1;
						if (l == 1)
						{
							num11++;
							num12 = num2;
							num13 = 1;
						}
						bool flag = true;
						do
						{
							if (num7 != i)
							{
								fixed (Tile* ptr = &Main.TileSet[num7 - num9, num11])
								{
									if (ptr->WallType != (byte)EntityID.WallID.PLANKED && (ptr->IsActive == 0 || Main.TileSolidNotSolidTop[ptr->Type]))
									{
										ptr->IsActive = 1;
										ptr->Type = (byte)EntityID.TileID.WOOD;
									}
								}
							}
							if (SolidTileUnsafe(num7 - 1, num11))
							{
								Main.TileSet[num7 - 1, num11].Type = (byte)EntityID.TileID.WOOD;
							}
							if (SolidTileUnsafe(num7 + 1, num11))
							{
								Main.TileSet[num7 + 1, num11].Type = (byte)EntityID.TileID.WOOD;
							}
							if (SolidTileUnsafe(num7, num11))
							{
								int num14 = 0;
								if (SolidTileUnsafe(num7 - 1, num11))
								{
									num14 = 1;
								}
								if (SolidTileUnsafe(num7 + 1, num11))
								{
									num14++;
								}
								if (SolidTileUnsafe(num7, num11 - 1))
								{
									num14++;
								}
								if (SolidTileUnsafe(num7, num11 + 1))
								{
									num14++;
								}
								if (num14 < 2)
								{
									Main.TileSet[num7, num11].IsActive = 0;
								}
								else
								{
									flag = false;
									Main.TileSet[num7, num11].Type = (byte)EntityID.TileID.WOOD;
								}
							}
							else
							{
								Main.TileSet[num7, num11].WallType = (byte)EntityID.WallID.PLANKED;
								Main.TileSet[num7, num11].Liquid = 0;
								Main.TileSet[num7, num11].Lava = 0;
							}
							num11 += num13;
							if (--num12 <= 0)
							{
								if (Main.TileSet[num7, num11].IsActive == 0)
								{
									Main.TileSet[num7, num11].IsActive = 1;
									Main.TileSet[num7, num11].Type = (byte)EntityID.TileID.WOOD;
								}
								break;
							}
						}
						while (flag);
					}
					num10--;
					num7 += num9;
					if (!SolidTileUnsafe(num7, num8))
					{
						continue;
					}
					int num15 = 0;
					int num16 = 0;
					int num17 = num8;
					do
					{
						num17--;
						num15++;
						if (SolidTileUnsafe(num7 - num9, num17))
						{
							num15 = 999;
							break;
						}
					}
					while (SolidTileUnsafe(num7, num17));
					num17 = num8;
					do
					{
						num17++;
						num16++;
						if (SolidTileUnsafe(num7 - num9, num17))
						{
							num16 = 999;
							break;
						}
					}
					while (SolidTileUnsafe(num7, num17));
					if (num16 <= num15)
					{
						if (num16 > num2)
						{
							num10 = 0;
						}
						else
						{
							num8 += num16 + 1;
						}
					}
					else if (num15 > num)
					{
						num10 = 0;
					}
					else
					{
						num8 -= num15 + 1;
					}
				}
				while (num10 > 0);
			}
			int num18 = i - num3 - 1;
			int num19 = i + num4 + 2;
			int num20 = num5 - 1;
			int num21 = num6 + 2;
			for (int m = num18; m < num19; m++)
			{
				for (int n = num20; n < num21; n++)
				{
					if (Main.TileSet[m, n].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[m, n].IsActive == 0)
					{
						if (Main.TileSet[m - 1, n].WallType != (byte)EntityID.WallID.PLANKED && m < i && !SolidTileUnsafe(m - 1, n))
						{
							PlaceTile(m, n, (byte)EntityID.TileID.WOOD, ToMute: true);
							Main.TileSet[m, n].WallType = (byte)EntityID.WallID.NONE;
						}
						if (Main.TileSet[m + 1, n].WallType != (byte)EntityID.WallID.PLANKED && m > i && !SolidTileUnsafe(m + 1, n))
						{
							PlaceTile(m, n, (byte)EntityID.TileID.WOOD, ToMute: true);
							Main.TileSet[m, n].WallType = (byte)EntityID.WallID.NONE;
						}
						for (int num22 = m - 1; num22 <= m + 1; num22++)
						{
							for (int num23 = n - 1; num23 <= n + 1; num23++)
							{
								if (SolidTileUnsafe(num22, num23))
								{
									Main.TileSet[num22, num23].Type = (byte)EntityID.TileID.WOOD;
								}
							}
						}
					}
					if (Main.TileSet[m, n].Type == (byte)EntityID.TileID.WOOD && Main.TileSet[m - 1, n].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[m + 1, n].WallType == (byte)EntityID.WallID.PLANKED && (Main.TileSet[m, n - 1].WallType == (byte)EntityID.WallID.PLANKED || Main.TileSet[m, n - 1].IsActive != 0) && (Main.TileSet[m, n + 1].WallType == (byte)EntityID.WallID.PLANKED || Main.TileSet[m, n + 1].IsActive != 0))
					{
						Main.TileSet[m, n].IsActive = 0;
						Main.TileSet[m, n].WallType = (byte)EntityID.WallID.PLANKED;
					}
				}
			}
			for (int num24 = num18; num24 < num19; num24++)
			{
				for (int num25 = num20; num25 < num21; num25++)
				{
					if (Main.TileSet[num24, num25].Type == (byte)EntityID.TileID.WOOD)
					{
						if (Main.TileSet[num24 - 1, num25].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24 + 1, num25].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24 - 1, num25].IsActive == 0 && Main.TileSet[num24 + 1, num25].IsActive == 0)
						{
							Main.TileSet[num24, num25].IsActive = 0;
							Main.TileSet[num24, num25].WallType = (byte)EntityID.WallID.PLANKED;
						}
						if (Main.TileSet[num24, num25 - 1].Type != (byte)EntityID.TileID.CHEST && Main.TileSet[num24 - 1, num25].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24 + 1, num25].Type == (byte)EntityID.TileID.WOOD && Main.TileSet[num24 + 2, num25].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24 - 1, num25].IsActive == 0 && Main.TileSet[num24 + 2, num25].IsActive == 0)
						{
							Main.TileSet[num24, num25].IsActive = 0;
							Main.TileSet[num24, num25].WallType = (byte)EntityID.WallID.PLANKED;
							Main.TileSet[num24 + 1, num25].IsActive = 0;
							Main.TileSet[num24 + 1, num25].WallType = (byte)EntityID.WallID.PLANKED;
						}
						if (Main.TileSet[num24, num25 - 1].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24, num25 + 1].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num24, num25 - 1].IsActive == 0 && Main.TileSet[num24, num25 + 1].IsActive == 0)
						{
							Main.TileSet[num24, num25].IsActive = 0;
							Main.TileSet[num24, num25].WallType = (byte)EntityID.WallID.PLANKED;
						}
					}
				}
			}
			for (int num26 = num18; num26 < num19; num26++)
			{
				for (int num27 = num21; num27 > num20; num27--)
				{
					bool flag2 = false;
					if (Main.TileSet[num26, num27].IsActive != 0 && Main.TileSet[num26, num27].Type == (byte)EntityID.TileID.WOOD)
					{
						int num28 = -1;
						for (int num29 = 0; num29 < 2; num29++)
						{
							if (!SolidTileUnsafe(num26 + num28, num27) && Main.TileSet[num26 + num28, num27].WallType == (byte)EntityID.WallID.NONE)
							{
								int num30 = 0;
								int num31 = num27;
								int num32 = num27;
								while (Main.TileSet[num26, num31].IsActive != 0 && Main.TileSet[num26, num31].Type == (byte)EntityID.TileID.WOOD && !SolidTileUnsafe(num26 + num28, num31) && Main.TileSet[num26 + num28, num31].WallType == (byte)EntityID.WallID.NONE)
								{
									num31--;
									num30++;
								}
								num31++;
								int num33 = num31 + 1;
								if (num30 > 4)
								{
									if (genRand.Next(2) == 0)
									{
										num31 = num32 - 1;
										bool flag3 = true;
										for (int num34 = num26 - 2; num34 <= num26 + 2; num34++)
										{
											for (int num35 = num31 - 2; num35 <= num31; num35++)
											{
												if (num34 != num26 && Main.TileSet[num34, num35].IsActive != 0)
												{
													flag3 = false;
												}
											}
										}
										if (flag3)
										{
											Main.TileSet[num26, num31].IsActive = 0;
											Main.TileSet[num26, num31 - 1].IsActive = 0;
											Main.TileSet[num26, num31 - 2].IsActive = 0;
											PlaceTile(num26, num31, (byte)EntityID.TileID.DOOR_CLOSED, ToMute: true);
											flag2 = true;
										}
									}
									if (!flag2)
									{
										for (int num36 = num33; num36 < num32; num36++)
										{
											Main.TileSet[num26, num36].Type = (byte)EntityID.TileID.WOODEN_BEAM;
										}
									}
								}
							}
							num28 = 1;
						}
					}
					if (flag2)
					{
						break;
					}
				}
			}
			int num37;
			for (num37 = num18; num37 < num19; num37++)
			{
				bool flag4 = true;
				for (int num38 = num20; num38 < num21; num38++)
				{
					for (int num39 = num37 - 2; num39 <= num37 + 2; num39++)
					{
						if (Main.TileSet[num39, num38].IsActive != 0 && (!SolidTileUnsafe(num39, num38) || Main.TileSet[num39, num38].Type == (byte)EntityID.TileID.DOOR_CLOSED))
						{
							flag4 = false;
						}
					}
				}
				if (flag4)
				{
					for (int num40 = num20; num40 < num21; num40++)
					{
						if (Main.TileSet[num37, num40].WallType == (byte)EntityID.WallID.PLANKED && Main.TileSet[num37, num40].IsActive == 0)
						{
							PlaceTile(num37, num40, (byte)EntityID.TileID.WOODEN_BEAM, ToMute: true);
						}
					}
				}
				num37 += genRand.Next(3);
			}
			for (int num41 = 0; num41 < 4; num41++)
			{
				int num42 = genRand.Next(num18 + 2, num19 - 1);
				int num43 = genRand.Next(num20 + 2, num21 - 1);
				while (Main.TileSet[num42, num43].WallType != (byte)EntityID.WallID.PLANKED)
				{
					num42 = genRand.Next(num18 + 2, num19 - 1);
					num43 = genRand.Next(num20 + 2, num21 - 1);
				}
				while (Main.TileSet[num42, num43].IsActive != 0)
				{
					num43--;
				}
				for (; Main.TileSet[num42, num43].IsActive == 0; num43++)
				{
				}
				num43--;
				if (Main.TileSet[num42, num43].WallType != (byte)EntityID.WallID.PLANKED)
				{
					continue;
				}
				if (genRand.Next(3) == 0)
				{
					EntityID.TileID type;
					switch (genRand.Next(9))
					{
						case 0:
							type = EntityID.TileID.TABLE;
							break;
						case 1:
							type = EntityID.TileID.ANVIL;
							break;
						case 2:
							type = EntityID.TileID.WORK_BENCH;
							break;
						case 3:
							type = EntityID.TileID.LOOM;
							break;
						case 4:
							type = EntityID.TileID.PIANO;
							break;
						case 5:
							type = EntityID.TileID.KEG;
							break;
						case 6:
							type = EntityID.TileID.BOOKCASE;
							break;
						case 7:
							type = EntityID.TileID.GRANDFATHERS_CLOCK;
							break;
						default:
							type = EntityID.TileID.SAWMILL;
							break;
					}
					PlaceTile(num42, num43, (int)type, ToMute: true);
				}
				else
				{
					int style = genRand.Next(2, 43);
					PlaceTile(num42, num43, (byte)EntityID.TileID.STATUE, ToMute: true, IsForced: true, -1, style);
				}
			}
		}

		public static void CountTiles(int X)
		{
			if (X == 0)
			{
				totalEvil = totalEvil2;
				totalSolid = totalSolid2;
				totalGood = totalGood2;
				if (!Gen)
				{
					if (totalSolid > 0)
					{
						GoodCoverage = (byte)Math.Round(totalGood * 100 / (float)totalSolid);
						EvilCoverage = (byte)Math.Round(totalEvil * 100 / (float)totalSolid);
						if (EvilCoverage >= 50f)
						{
							UI.SetTriggerStateForAll(Trigger.CorruptedWorld);
						}
						if (GoodCoverage >= 50f)
						{
							UI.SetTriggerStateForAll(Trigger.HallowedWorld);
						}
					}
					else
					{
						GoodCoverage = 0;
						EvilCoverage = 0;
					}
					NetMessage.CreateMessage0(57);
					NetMessage.SendMessage();
				}
				totalEvil2 = 0;
				totalSolid2 = 0;
				totalGood2 = 0;
			}
			int worldSurface = Main.WorldSurface;
			int num = Main.MaxTilesY - 1;
			do
			{
				if (SolidTileUnsafe(X, num))
				{
					switch ((EntityID.TileID)Main.TileSet[X, num].Type)
					{
						case EntityID.TileID.HALLOWED_GRASS:
						case EntityID.TileID.PEARLSAND:
						case EntityID.TileID.PEARLSTONE:
							totalGood2++;
							break;
						case EntityID.TileID.CORRUPT_GRASS:
						case EntityID.TileID.EBONSTONE:
						case EntityID.TileID.EBONSAND:
							totalEvil2++;
							break;
					}
					totalSolid2++;
				}
			}
			while (--num > worldSurface);
			do
			{
				if (SolidTileUnsafe(X, num))
				{
					switch ((EntityID.TileID)Main.TileSet[X, num].Type)
					{
						case EntityID.TileID.HALLOWED_GRASS:
						case EntityID.TileID.PEARLSAND:
						case EntityID.TileID.PEARLSTONE:
							totalGood2 += 5;
							break;
						case EntityID.TileID.CORRUPT_GRASS:
						case EntityID.TileID.EBONSTONE:
						case EntityID.TileID.EBONSAND:
							totalEvil2 += 5;
							break;
					}
					totalSolid2 += 5;
				}
			}
			while (--num >= 0);
		}

		public unsafe static void UpdateWorld()
		{
			UpdateSand();
			UpdateMech();
			if ((++Liquid.SkipCount & 1) == 0)
			{
				Liquid.UpdateLiquid();
			}
			if (hardLock)
			{
				return;
			}
			if ((++totalD & 0xF) == 0)
			{
				CountTiles(totalX);
				if (++totalX >= Main.MaxTilesX)
				{
					totalX = 0;
				}
			}
			bool flag = false;
			if (Main.InvasionType > 0)
			{
				spawnDelay = 0;
			}
			if (++spawnDelay >= 20)
			{
				flag = true;
				spawnDelay = 0;
				if (ToSpawnNPC != (int)EntityID.NPCID.OLD_MAN)
				{
					for (int i = 0; i < NPC.MaxNumNPCs; i++)
					{
						if (Main.NPCSet[i].Active != 0 && Main.NPCSet[i].IsHomeless && Main.NPCSet[i].IsTownNPC)
						{
							ToSpawnNPC = Main.NPCSet[i].Type;
							break;
						}
					}
				}
			}
			float num = 3E-05f * Main.WorldRate;
			float num2 = 1.5E-05f * Main.WorldRate;
			for (int j = 0; j < Main.MaxTilesX * Main.MaxTilesY * num; j++)
			{
				int num3 = genRand.Next(10, Main.MaxTilesX - 10);
				int num4 = genRand.Next(10, Main.WorldSurface - 1);
				int num5 = num3 - 1;
				int num6 = num3 + 2;
				int num7 = num4 - 1;
				int num8 = num4 + 2;
				if (num5 < 10)
				{
					num5 = 10;
				}
				if (num6 > Main.MaxTilesX - 10)
				{
					num6 = Main.MaxTilesX - 10;
				}
				if (num7 < 10)
				{
					num7 = 10;
				}
				if (num8 > Main.MaxTilesY - 10)
				{
					num8 = Main.MaxTilesY - 10;
				}
				if (Main.TileSet[num3, num4].Type >= (byte)EntityID.TileID.DAYBLOOM_GROWING && Main.TileSet[num3, num4].Type <= (byte)EntityID.TileID.DAYBLOOM_BLOOMING)
				{
					GrowAlch(num3, num4);
				}
				if (Main.TileSet[num3, num4].Liquid > 32)
				{
					if (Main.TileSet[num3, num4].IsActive != 0 && (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SHORT_GRASS_PLANTS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SAPLING || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SHORT_CORRUPT_PLANTS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SUNFLOWER || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.TALL_GRASS_PLANTS))
					{
						KillTile(num3, num4);
						if (Main.NetMode == (int)NetModeSetting.SERVER)
						{
							NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num3, num4, 0);
							NetMessage.SendMessage();
						}
					}
				}
				else if (Main.TileSet[num3, num4].IsActive != 0)
				{
					hardUpdateWorld(num3, num4);
					if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CACTUS)
					{
						if (genRand.Next(15) == 0)
						{
							GrowCactus(num3, num4);
						}
					}
					else if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SAND)
					{
						if (Main.TileSet[num3, num7].IsActive == 0)
						{
							if (num3 < 250 || num3 > Main.MaxTilesX - 250)
							{
								if (genRand.Next(500) == 0 && Main.TileSet[num3, num7].Liquid == byte.MaxValue && Main.TileSet[num3, num7 - 1].Liquid == byte.MaxValue && Main.TileSet[num3, num7 - 2].Liquid == byte.MaxValue && Main.TileSet[num3, num7 - 3].Liquid == byte.MaxValue && Main.TileSet[num3, num7 - 4].Liquid == byte.MaxValue && PlaceTile(num3, num7, (byte)EntityID.TileID.CORAL, true))
								{
									NetMessage.SendTile(num3, num7);
								}
							}
							else if (num3 > 400 && num3 < Main.MaxTilesX - 400 && genRand.Next(300) == 0)
							{
								GrowCactus(num3, num4);
							}
						}
					}
					else if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.PEARLSAND || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.EBONSAND)
					{
						if (Main.TileSet[num3, num7].IsActive == 0 && num3 > 400 && num3 < Main.MaxTilesX - 400 && genRand.Next(300) == 0)
						{
							GrowCactus(num3, num4);
						}
					}
					else if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CLAY_POT)
					{
						if (Main.TileSet[num3, num7].IsActive == 0 && PlaceTile(num3, num7, (byte)EntityID.TileID.SHORT_GRASS_PLANTS, true))
						{
							NetMessage.SendTile(num3, num7);
						}
					}
					else if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.GRASS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CORRUPT_GRASS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CORRUPTION_THORN || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.HALLOWED_GRASS)
					{
						int num9 = Main.TileSet[num3, num4].Type;
						if (Main.TileSet[num3, num7].IsActive == 0 && genRand.Next(12) == 0 && num9 == (int)EntityID.TileID.GRASS && PlaceTile(num3, num7, (int)EntityID.TileID.SHORT_GRASS_PLANTS, true))
						{
							NetMessage.SendTile(num3, num7);
						}
						if (Main.TileSet[num3, num7].IsActive == 0 && genRand.Next(10) == 0 && num9 == (int)EntityID.TileID.CORRUPT_GRASS && PlaceTile(num3, num7, (int)EntityID.TileID.SHORT_CORRUPT_PLANTS, true))
						{
							NetMessage.SendTile(num3, num7);
						}
						if (Main.TileSet[num3, num7].IsActive == 0 && genRand.Next(10) == 0 && num9 == (int)EntityID.TileID.HALLOWED_GRASS && PlaceTile(num3, num7, (int)EntityID.TileID.SHORT_HALLOWED_PLANTS, true))
						{
							NetMessage.SendTile(num3, num7);
						}
						bool flag2 = false;
						for (int k = num5; k < num6; k++)
						{
							for (int l = num7; l < num8; l++)
							{
								if ((num3 == k && num4 == l) || Main.TileSet[k, l].IsActive == 0)
								{
									continue;
								}
								if (num9 == (int)EntityID.TileID.CORRUPTION_THORN)
								{
									num9 = (int)EntityID.TileID.CORRUPT_GRASS;
								}
								if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.DIRT || (num9 == (int)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[k, l].Type == (byte)EntityID.TileID.GRASS) || (num9 == (int)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[k, l].Type == (byte)EntityID.TileID.HALLOWED_GRASS))
								{
									SpreadGrass(k, l, 0, num9, false);
									if (num9 == (int)EntityID.TileID.CORRUPT_GRASS)
									{
										SpreadGrass(k, l, (int)EntityID.TileID.GRASS, num9, false);
										SpreadGrass(k, l, (int)EntityID.TileID.HALLOWED_GRASS, num9, false);
									}
									if (Main.TileSet[k, l].Type == num9)
									{
										SquareTileFrame(k, l);
										flag2 = true;
									}
								}
								if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.DIRT || (num9 == (int)EntityID.TileID.HALLOWED_GRASS && Main.TileSet[k, l].Type == (byte)EntityID.TileID.GRASS) || (num9 == (int)EntityID.TileID.HALLOWED_GRASS && Main.TileSet[k, l].Type == (byte)EntityID.TileID.CORRUPT_GRASS))
								{
									SpreadGrass(k, l, 0, num9, false);
									if (num9 == (int)EntityID.TileID.HALLOWED_GRASS)
									{
										SpreadGrass(k, l, (int)EntityID.TileID.GRASS, num9, false);
										SpreadGrass(k, l, (int)EntityID.TileID.CORRUPT_GRASS, num9, false);
									}
									if (Main.TileSet[k, l].Type == num9)
									{
										SquareTileFrame(k, l);
										flag2 = true;
									}
								}
							}
						}
						if (flag2)
						{
							NetMessage.SendTileSquare(num3, num4, 3);
						}
					}
					else if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SAPLING && genRand.Next(20) == 0 && !PlayerLOS(num3, num4))
					{
						GrowTree(num3, num4);
					}
					if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SHORT_GRASS_PLANTS && genRand.Next(20) == 0 && Main.TileSet[num3, num4].FrameX < 144)
					{
						Main.TileSet[num3, num4].Type = (byte)EntityID.TileID.TALL_GRASS_PLANTS;
						NetMessage.SendTileSquare(num3, num4, 3);
					}
					if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SHORT_HALLOWED_PLANTS && genRand.Next(20) == 0 && Main.TileSet[num3, num4].FrameX < 144)
					{
						Main.TileSet[num3, num4].Type = (byte)EntityID.TileID.TALL_HALLOWED_PLANTS;
						NetMessage.SendTileSquare(num3, num4, 3);
					}
					if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CORRUPTION_THORN && genRand.Next(3) == 0)
					{
						int num10 = num3;
						int num11 = num4;
						int num12 = 0;
						if (Main.TileSet[num10 + 1, num11].IsActive != 0 && Main.TileSet[num10 + 1, num11].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num12++;
						}
						if (Main.TileSet[num10 - 1, num11].IsActive != 0 && Main.TileSet[num10 - 1, num11].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num12++;
						}
						if (Main.TileSet[num10, num11 + 1].IsActive != 0 && Main.TileSet[num10, num11 + 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num12++;
						}
						if (Main.TileSet[num10, num11 - 1].IsActive != 0 && Main.TileSet[num10, num11 - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num12++;
						}
						if (num12 < 3 || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.CORRUPT_GRASS)
						{
							switch (genRand.Next(4))
							{
								case 0:
									num11--;
									break;
								case 1:
									num11++;
									break;
								case 2:
									num10--;
									break;
								case 3:
									num10++;
									break;
							}
							if (Main.TileSet[num10, num11].IsActive == 0)
							{
								num12 = 0;
								if (Main.TileSet[num10 + 1, num11].IsActive != 0 && Main.TileSet[num10 + 1, num11].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num12++;
								}
								if (Main.TileSet[num10 - 1, num11].IsActive != 0 && Main.TileSet[num10 - 1, num11].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num12++;
								}
								if (Main.TileSet[num10, num11 + 1].IsActive != 0 && Main.TileSet[num10, num11 + 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num12++;
								}
								if (Main.TileSet[num10, num11 - 1].IsActive != 0 && Main.TileSet[num10, num11 - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num12++;
								}
								if (num12 < 2)
								{
									int num13 = 7;
									int num14 = num10 - num13;
									int num15 = num10 + num13;
									int num16 = num11 - num13;
									int num17 = num11 + num13;
									bool flag3 = false;
									for (int m = num14; m < num15; m++)
									{
										for (int n = num16; n < num17; n++)
										{
											if (Math.Abs(m - num10) * 2 + Math.Abs(n - num11) < 9 && Main.TileSet[m, n].IsActive != 0 && Main.TileSet[m, n].Type == (byte)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[m, n - 1].IsActive != 0 && Main.TileSet[m, n - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN && Main.TileSet[m, n - 1].Liquid == 0)
											{
												flag3 = true;
												break;
											}
										}
									}
									if (flag3)
									{
										Main.TileSet[num10, num11].Type = (byte)EntityID.TileID.CORRUPTION_THORN;
										Main.TileSet[num10, num11].IsActive = 1;
										SquareTileFrame(num10, num11);
										NetMessage.SendTileSquare(num10, num11, 3);
									}
								}
							}
						}
					}
				}
				else if (flag && ToSpawnNPC > 0)
				{
					SpawnNPC(num3, num4);
				}
				if (Main.TileSet[num3, num4].IsActive == 0)
				{
					continue;
				}
				if ((Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.GRASS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.VINE) && genRand.Next(40) == 0 && Main.TileSet[num3, num4 + 1].IsActive == 0 && Main.TileSet[num3, num4 + 1].Lava == 0)
				{
					bool flag4 = false;
					for (int num18 = num4; num18 > num4 - 10; num18--)
					{
						if (Main.TileSet[num3, num18].IsActive != 0 && Main.TileSet[num3, num18].Type == (byte)EntityID.TileID.GRASS)
						{
							flag4 = true;
							break;
						}
					}
					if (flag4)
					{
						int num19 = num3;
						int num20 = num4 + 1;
						Main.TileSet[num19, num20].Type = (byte)EntityID.TileID.VINE;
						Main.TileSet[num19, num20].IsActive = 1;
						SquareTileFrame(num19, num20);
						NetMessage.SendTileSquare(num19, num20, 3);
					}
				}
				if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
				{
					int type = Main.TileSet[num3, num4].Type;
					if (Main.TileSet[num3, num7].IsActive == 0 && genRand.Next(7) == 0)
					{
						if (PlaceTile(num3, num7, (byte)EntityID.TileID.SHORT_JUNGLE_PLANTS, true))
						{
							NetMessage.SendTile(num3, num7);
						}
					}
					else if (genRand.Next(500) == 0 && (Main.TileSet[num3, num7].IsActive == 0 || Main.TileSet[num3, num7].Type == (byte)EntityID.TileID.SHORT_JUNGLE_PLANTS || Main.TileSet[num3, num7].Type == (byte)EntityID.TileID.TALL_JUNGLE_PLANTS || Main.TileSet[num3, num7].Type == (byte)EntityID.TileID.JUNGLE_THORN) && !PlayerLOS(num3, num4))
					{
						GrowTree(num3, num4);
					}
					bool flag5 = false;
					for (int num21 = num5; num21 < num6; num21++)
					{
						for (int num22 = num7; num22 < num8; num22++)
						{
							if ((num3 != num21 || num4 != num22) && Main.TileSet[num21, num22].IsActive != 0 && Main.TileSet[num21, num22].Type == (byte)EntityID.TileID.MUD)
							{
								SpreadGrass(num21, num22, (byte)EntityID.TileID.MUD, type, false);
								if (Main.TileSet[num21, num22].Type == type)
								{
									SquareTileFrame(num21, num22);
									flag5 = true;
								}
							}
						}
					}
					if (flag5)
					{
						NetMessage.SendTileSquare(num3, num4, 3);
					}
				}
				if (Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.SHORT_JUNGLE_PLANTS && genRand.Next(3) == 0 && Main.TileSet[num3, num4].FrameX < 144)
				{
					Main.TileSet[num3, num4].Type = (byte)EntityID.TileID.TALL_JUNGLE_PLANTS;
					NetMessage.SendTileSquare(num3, num4, 3);
				}
				if ((Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.JUNGLE_GRASS || Main.TileSet[num3, num4].Type == (byte)EntityID.TileID.JUNGLE_VINE) && genRand.Next(15) == 0 && Main.TileSet[num3, num4 + 1].IsActive == 0 && Main.TileSet[num3, num4 + 1].Lava == 0)
				{
					bool flag6 = false;
					for (int num23 = num4; num23 > num4 - 10; num23--)
					{
						if (Main.TileSet[num3, num23].IsActive != 0 && Main.TileSet[num3, num23].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
						{
							flag6 = true;
							break;
						}
					}
					if (flag6)
					{
						int num24 = num3;
						int num25 = num4 + 1;
						Main.TileSet[num24, num25].Type = (byte)EntityID.TileID.JUNGLE_VINE;
						Main.TileSet[num24, num25].IsActive = 1;
						SquareTileFrame(num24, num25);
						NetMessage.SendTileSquare(num24, num25, 3);
					}
				}
				if ((Main.TileSet[num3, num4].Type != (byte)EntityID.TileID.HALLOWED_GRASS && Main.TileSet[num3, num4].Type != (byte)EntityID.TileID.HALLOWED_VINE) || genRand.Next(15) != 0 || Main.TileSet[num3, num4 + 1].IsActive != 0 || Main.TileSet[num3, num4 + 1].Lava != 0)
				{
					continue;
				}
				bool flag7 = false;
				for (int num26 = num4; num26 > num4 - 10; num26--)
				{
					if (Main.TileSet[num3, num26].IsActive != 0 && Main.TileSet[num3, num26].Type == (byte)EntityID.TileID.HALLOWED_GRASS)
					{
						flag7 = true;
						break;
					}
				}
				if (flag7)
				{
					int num27 = num3;
					int num28 = num4 + 1;
					Main.TileSet[num27, num28].Type = (byte)EntityID.TileID.HALLOWED_VINE;
					Main.TileSet[num27, num28].IsActive = 1;
					SquareTileFrame(num27, num28);
					NetMessage.SendTileSquare(num27, num28, 3);
				}
			}
			for (int num29 = 0; num29 < Main.MaxTilesX * Main.MaxTilesY * num2; num29++)
			{
				int num30 = genRand.Next(10, Main.MaxTilesX - 10);
				int num31 = genRand.Next(Main.WorldSurface - 1, Main.MaxTilesY - 20);
				int num32 = num30 - 1;
				int num33 = num30 + 2;
				int num34 = num31 - 1;
				int num35 = num31 + 2;
				if (num32 < 10)
				{
					num32 = 10;
				}
				if (num33 > Main.MaxTilesX - 10)
				{
					num33 = Main.MaxTilesX - 10;
				}
				if (num34 < 10)
				{
					num34 = 10;
				}
				if (num35 > Main.MaxTilesY - 10)
				{
					num35 = Main.MaxTilesY - 10;
				}
				if (Main.TileSet[num30, num31].Type >= (byte)EntityID.TileID.DAYBLOOM_GROWING && Main.TileSet[num30, num31].Type <= (byte)EntityID.TileID.DAYBLOOM_BLOOMING)
				{
					GrowAlch(num30, num31);
				}
				if (Main.TileSet[num30, num31].Liquid > 32)
				{
					continue;
				}
				if (Main.TileSet[num30, num31].IsActive != 0)
				{
					hardUpdateWorld(num30, num31);
					if (Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[num30, num34].IsActive == 0 && genRand.Next(1) == 0 && PlaceTile(num30, num34, (byte)EntityID.TileID.SHORT_CORRUPT_PLANTS, true))
					{
						NetMessage.SendTile(num30, num34);
					}
					if (Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.CORRUPTION_THORN && genRand.Next(3) == 0)
					{
						int num36 = num30;
						int num37 = num31;
						int num38 = 0;
						if (Main.TileSet[num36 + 1, num37].IsActive != 0 && Main.TileSet[num36 + 1, num37].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num38++;
						}
						if (Main.TileSet[num36 - 1, num37].IsActive != 0 && Main.TileSet[num36 - 1, num37].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num38++;
						}
						if (Main.TileSet[num36, num37 + 1].IsActive != 0 && Main.TileSet[num36, num37 + 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num38++;
						}
						if (Main.TileSet[num36, num37 - 1].IsActive != 0 && Main.TileSet[num36, num37 - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
						{
							num38++;
						}
						if (num38 < 3 || Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.CORRUPT_GRASS)
						{
							switch (genRand.Next(4))
							{
								case 0:
									num37--;
									break;
								case 1:
									num37++;
									break;
								case 2:
									num36--;
									break;
								case 3:
									num36++;
									break;
							}
							if (Main.TileSet[num36, num37].IsActive == 0)
							{
								num38 = 0;
								if (Main.TileSet[num36 + 1, num37].IsActive != 0 && Main.TileSet[num36 + 1, num37].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num38++;
								}
								if (Main.TileSet[num36 - 1, num37].IsActive != 0 && Main.TileSet[num36 - 1, num37].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num38++;
								}
								if (Main.TileSet[num36, num37 + 1].IsActive != 0 && Main.TileSet[num36, num37 + 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num38++;
								}
								if (Main.TileSet[num36, num37 - 1].IsActive != 0 && Main.TileSet[num36, num37 - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN)
								{
									num38++;
								}
								if (num38 < 2)
								{
									int num39 = 7;
									int num40 = num36 - num39;
									int num41 = num36 + num39;
									int num42 = num37 - num39;
									int num43 = num37 + num39;
									bool flag8 = false;
									for (int num44 = num40; num44 < num41; num44++)
									{
										for (int num45 = num42; num45 < num43; num45++)
										{
											if (Math.Abs(num44 - num36) * 2 + Math.Abs(num45 - num37) < 9 && Main.TileSet[num44, num45].IsActive != 0 && Main.TileSet[num44, num45].Type == (byte)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[num44, num45 - 1].IsActive != 0 && Main.TileSet[num44, num45 - 1].Type == (byte)EntityID.TileID.CORRUPTION_THORN && Main.TileSet[num44, num45 - 1].Liquid == 0)
											{
												flag8 = true;
												break;
											}
										}
									}
									if (flag8)
									{
										Main.TileSet[num36, num37].Type = (byte)EntityID.TileID.CORRUPTION_THORN;
										Main.TileSet[num36, num37].IsActive = 1;
										SquareTileFrame(num36, num37);
										NetMessage.SendTileSquare(num36, num37, 3);
									}
								}
							}
						}
					}
					if (Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
					{
						int type2 = Main.TileSet[num30, num31].Type;
						if (Main.TileSet[num30, num34].IsActive == 0 && genRand.Next(10) == 0)
						{
							if (PlaceTile(num30, num34, (byte)EntityID.TileID.SHORT_JUNGLE_PLANTS, true))
							{
								NetMessage.SendTile(num30, num34);
							}
						}
						bool flag9 = false;
						for (int num46 = num32; num46 < num33; num46++)
						{
							for (int num47 = num34; num47 < num35; num47++)
							{
								if ((num30 != num46 || num31 != num47) && Main.TileSet[num46, num47].IsActive != 0 && Main.TileSet[num46, num47].Type == (byte)EntityID.TileID.MUD)
								{
									SpreadGrass(num46, num47, (byte)EntityID.TileID.MUD, type2, false);
									if (Main.TileSet[num46, num47].Type == type2)
									{
										SquareTileFrame(num46, num47);
										flag9 = true;
									}
								}
							}
						}
						if (flag9)
						{
							NetMessage.SendTileSquare(num30, num31, 3);
						}
					}
					if (Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.SHORT_JUNGLE_PLANTS && genRand.Next(3) == 0 && Main.TileSet[num30, num31].FrameX < 144)
					{
						Main.TileSet[num30, num31].Type = (byte)EntityID.TileID.TALL_JUNGLE_PLANTS;
						NetMessage.SendTileSquare(num30, num31, 3);
					}
					if ((Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.JUNGLE_GRASS || Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.JUNGLE_VINE) && genRand.Next(5) == 0 && Main.TileSet[num30, num31 + 1].IsActive == 0 && Main.TileSet[num30, num31 + 1].Lava == 0)
					{
						bool flag10 = false;
						for (int num48 = num31; num48 > num31 - 10; num48--)
						{
							if (Main.TileSet[num30, num48].IsActive != 0 && Main.TileSet[num30, num48].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
							{
								flag10 = true;
								break;
							}
						}
						if (flag10)
						{
							int num49 = num30;
							int num50 = num31 + 1;
							Main.TileSet[num49, num50].Type = (byte)EntityID.TileID.JUNGLE_VINE;
							Main.TileSet[num49, num50].IsActive = 1;
							SquareTileFrame(num49, num50);
							NetMessage.SendTileSquare(num49, num50, 3);
						}
					}
					if (Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.JUNGLE_THORN && genRand.Next(3) == 0)
					{
						int num51 = num30;
						int num52 = num31;
						int num53 = 0;
						if (Main.TileSet[num51 + 1, num52].IsActive != 0 && Main.TileSet[num51 + 1, num52].Type == (byte)EntityID.TileID.JUNGLE_THORN)
						{
							num53++;
						}
						if (Main.TileSet[num51 - 1, num52].IsActive != 0 && Main.TileSet[num51 - 1, num52].Type == (byte)EntityID.TileID.JUNGLE_THORN)
						{
							num53++;
						}
						if (Main.TileSet[num51, num52 + 1].IsActive != 0 && Main.TileSet[num51, num52 + 1].Type == (byte)EntityID.TileID.JUNGLE_THORN)
						{
							num53++;
						}
						if (Main.TileSet[num51, num52 - 1].IsActive != 0 && Main.TileSet[num51, num52 - 1].Type == (byte)EntityID.TileID.JUNGLE_THORN)
						{
							num53++;
						}
						if (num53 < 3 || Main.TileSet[num30, num31].Type == (byte)EntityID.TileID.JUNGLE_GRASS)
						{
							switch (genRand.Next(4))
							{
								case 0:
									num52--;
									break;
								case 1:
									num52++;
									break;
								case 2:
									num51--;
									break;
								case 3:
									num51++;
									break;
							}
							if (Main.TileSet[num51, num52].IsActive == 0)
							{
								num53 = 0;
								if (Main.TileSet[num51 + 1, num52].IsActive != 0 && Main.TileSet[num51 + 1, num52].Type == (byte)EntityID.TileID.JUNGLE_THORN)
								{
									num53++;
								}
								if (Main.TileSet[num51 - 1, num52].IsActive != 0 && Main.TileSet[num51 - 1, num52].Type == (byte)EntityID.TileID.JUNGLE_THORN)
								{
									num53++;
								}
								if (Main.TileSet[num51, num52 + 1].IsActive != 0 && Main.TileSet[num51, num52 + 1].Type == (byte)EntityID.TileID.JUNGLE_THORN)
								{
									num53++;
								}
								if (Main.TileSet[num51, num52 - 1].IsActive != 0 && Main.TileSet[num51, num52 - 1].Type == (byte)EntityID.TileID.JUNGLE_THORN)
								{
									num53++;
								}
								if (num53 < 2)
								{
									int num54 = 7;
									int num55 = num51 - num54;
									int num56 = num51 + num54;
									int num57 = num52 - num54;
									int num58 = num52 + num54;
									bool flag11 = false;
									for (int num59 = num55; num59 < num56; num59++)
									{
										for (int num60 = num57; num60 < num58; num60++)
										{
											if (Math.Abs(num59 - num51) * 2 + Math.Abs(num60 - num52) < 9 && Main.TileSet[num59, num60].IsActive != 0 && Main.TileSet[num59, num60].Type == (byte)EntityID.TileID.JUNGLE_GRASS && Main.TileSet[num59, num60 - 1].IsActive != 0 && Main.TileSet[num59, num60 - 1].Type == (byte)EntityID.TileID.JUNGLE_THORN && Main.TileSet[num59, num60 - 1].Liquid == 0)
											{
												flag11 = true;
												break;
											}
										}
									}
									if (flag11)
									{
										Main.TileSet[num51, num52].Type = (byte)EntityID.TileID.JUNGLE_THORN;
										Main.TileSet[num51, num52].IsActive = 1;
										SquareTileFrame(num51, num52);
										NetMessage.SendTileSquare(num51, num52, 3);
									}
								}
							}
						}
					}
					if (Main.TileSet[num30, num31].Type != (byte)EntityID.TileID.MUSHROOM_GRASS)
					{
						continue;
					}
					int type3 = Main.TileSet[num30, num31].Type;
					if (Main.TileSet[num30, num34].IsActive == 0 && genRand.Next(10) == 0)
					{
						if (PlaceTile(num30, num34, (byte)EntityID.TileID.GLOWING_MUSHROOM, true))
						{
							NetMessage.SendTile(num30, num34);
						}
					}
					if (genRand.Next(200) == 0 && !PlayerLOS(num30, num31))
					{
						GrowShroom(num30, num31);
					}
					bool flag12 = false;
					for (int num61 = num32; num61 < num33; num61++)
					{
						for (int num62 = num34; num62 < num35; num62++)
						{
							if ((num30 != num61 || num31 != num62) && Main.TileSet[num61, num62].IsActive != 0 && Main.TileSet[num61, num62].Type == (byte)EntityID.TileID.MUD)
							{
								SpreadGrass(num61, num62, (byte)EntityID.TileID.MUD, type3, false);
								if (Main.TileSet[num61, num62].Type == type3)
								{
									SquareTileFrame(num61, num62);
									flag12 = true;
								}
							}
						}
					}
					if (flag12)
					{
						NetMessage.SendTileSquare(num30, num31, 3);
					}
				}
				else if (flag && ToSpawnNPC > 0)
				{
					SpawnNPC(num30, num31);
				}
			}
			if (Main.Rand.Next(100) == 0)
			{
				PlantAlch();
			}
			if (!Main.GameTime.DayTime)
			{
				float num55 = Main.MaxTilesX / 4200f;
				if (Main.Rand.Next(8000) < 10f * num55)
				{
					int num56 = 12;
					int num57 = Main.Rand.Next(Main.MaxTilesX - 50) + 100;
					num57 *= 16;
					int num58 = Main.Rand.Next((int)(Main.MaxTilesY * 0.05));
					num58 *= 16;
					Vector2 vector = new Vector2(num57, num58);
					float num59 = Main.Rand.Next(-100, 101);
					float num60 = Main.Rand.Next(200) + 100;
					float num61 = (float)Math.Sqrt(num59 * num59 + num60 * num60);
					num61 = num56 / num61;
					num59 *= num61;
					num60 *= num61;
					Projectile.NewProjectile(vector.X, vector.Y, num59, num60, (int)EntityID.ProjectileID.FALLING_STAR, 1000, 10f);
				}
			}
		}

		public static bool PlaceWall(int i, int j, int type)
		{
			if (i <= 1 || j <= 1 || i >= Main.MaxTilesX - 2 || j >= Main.MaxTilesY - 2)
			{
				return false;
			}
			if (Main.TileSet[i, j].WallType == (byte)EntityID.WallID.NONE)
			{
				Main.TileSet[i, j].WallType = (byte)type;
				WallFrame(i - 1, j - 1);
				WallFrame(i - 1, j);
				WallFrame(i - 1, j + 1);
				WallFrame(i, j - 1);
				WallFrame(i, j, resetFrame: true);
				WallFrame(i, j + 1);
				WallFrame(i + 1, j - 1);
				WallFrame(i + 1, j);
				WallFrame(i + 1, j + 1);
				Main.PlaySound(0, i << 4, j << 4);
				return true;
			}
			return false;
		}

		public unsafe static void AddPlants()
		{
			fixed (Tile* ptr = Main.TileSet)
			{
				for (int i = 0; i < Main.MaxTilesX; i++)
				{
					Tile* ptr2 = ptr + (i * (Main.LargeWorldH) + 5);
					int num = 5;
					while (num < Main.MaxTilesY)
					{
						if (ptr2->IsActive != 0)
						{
							if (ptr2->Type == (int)EntityID.TileID.GRASS)
							{
								ptr2--;
								if (ptr2->IsActive == 0)
								{
									PlaceTile(i, num - 1, (int)EntityID.TileID.SHORT_GRASS_PLANTS, ToMute: true);
								}
								ptr2++;
							}
							else if (ptr2->Type == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								ptr2--;
								if (ptr2->IsActive == 0)
								{
									PlaceTile(i, num - 1, (int)EntityID.TileID.SHORT_CORRUPT_PLANTS, ToMute: true);
								}
								ptr2++;
							}
						}
						num++;
						ptr2++;
					}
				}
			}
		}

		public static void SpreadGrass(int i, int j, int dirt = (int)EntityID.TileID.DIRT, int grass = (int)EntityID.TileID.GRASS, bool repeat = true)
		{
			try
			{
				if (Main.TileSet[i, j].Type != dirt || Main.TileSet[i, j].IsActive == 0 || (j < Main.WorldSurface && grass == (int)EntityID.TileID.MUSHROOM_GRASS) || (j >= Main.WorldSurface && dirt == (int)EntityID.TileID.DIRT))
				{
					return;
				}
				int num = i - 1;
				int num2 = i + 2;
				int num3 = j - 1;
				int num4 = j + 2;
				if (num < 0)
				{
					num = 0;
				}
				if (num2 > Main.MaxTilesX)
				{
					num2 = Main.MaxTilesX;
				}
				if (num3 < 0)
				{
					num3 = 0;
				}
				if (num4 > Main.MaxTilesY)
				{
					num4 = Main.MaxTilesY;
				}
				bool flag = true;
				for (int k = num; k < num2; k++)
				{
					for (int l = num3; l < num4; l++)
					{
						if (Main.TileSet[k, l].IsActive == 0 || !Main.TileSolid[Main.TileSet[k, l].Type])
						{
							flag = false;
						}
						if (Main.TileSet[k, l].Lava != 0 && Main.TileSet[k, l].Liquid > 0)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag || (grass == (int)EntityID.TileID.CORRUPT_GRASS && Main.TileSet[i, j - 1].Type == (int)EntityID.TileID.SUNFLOWER))
				{
					return;
				}
				Main.TileSet[i, j].Type = (byte)grass;
				for (int m = num; m < num2; m++)
				{
					for (int n = num3; n < num4; n++)
					{
						if (Main.TileSet[m, n].IsActive == 0 || Main.TileSet[m, n].Type != dirt)
						{
							continue;
						}
						try
						{
							if (repeat && grassSpread < 400)
							{
								grassSpread++;
								SpreadGrass(m, n, dirt, grass);
								grassSpread--;
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		public static void ChasmRunnerSideways(int i, int j, int direction, int steps)
		{
			Vector2 vector = default;
			Vector2 vector2 = default;
			float num = steps;
			vector.X = i;
			vector.Y = j;
			vector2.X = genRand.Next(10, 21) * 0.1f * direction;
			vector2.Y = genRand.Next(-10, 10) * 0.01f;
			int num2 = genRand.Next(5) + 7;
			while (num2 > 0)
			{
				if (num > 0f)
				{
					num2 += genRand.Next(3);
					num2 -= genRand.Next(3);
					if (num2 < 7)
					{
						num2 = 7;
					}
					else if (num2 > 20)
					{
						num2 = 20;
					}
					if (num == 1f && num2 < 10)
					{
						num2 = 10;
					}
				}
				else
				{
					num2 -= genRand.Next(4);
				}
				if (vector.Y > Main.RockLayer && num > 0f)
				{
					num = 0f;
				}
				num -= 1f;
				int num3 = (int)(vector.X - num2 * 0.5f);
				int num4 = (int)(vector.X + num2 * 0.5f);
				int num5 = (int)(vector.Y - num2 * 0.5f);
				int num6 = (int)(vector.Y + num2 * 0.5f);
				if (num3 < 0)
				{
					num3 = 0;
				}
				if (num4 > Main.MaxTilesX - 1)
				{
					num4 = Main.MaxTilesX - 1;
				}
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesY)
				{
					num6 = Main.MaxTilesY;
				}
				for (int k = num3; k < num4; k++)
				{
					for (int l = num5; l < num6; l++)
					{
						if (Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < num2 * 0.5f * (1f + genRand.Next(-10, 11) * 0.015f) && Main.TileSet[k, l].Type != (byte)EntityID.TileID.SHADOW_ORB && Main.TileSet[k, l].Type != (byte)EntityID.TileID.DEMONITE_ORE)
						{
							Main.TileSet[k, l].IsActive = 0;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.Y += genRand.Next(-10, 10) * 0.1f;
				if (vector.Y < j - 20)
				{
					vector2.Y += genRand.Next(20) * 0.01f;
				}
				else if (vector.Y > j + 20)
				{
					vector2.Y -= genRand.Next(20) * 0.01f;
				}
				if (vector2.Y < -0.5)
				{
					vector2.Y = -0.5f;
				}
				else if (vector2.Y > 0.5)
				{
					vector2.Y = 0.5f;
				}
				vector2.X += genRand.Next(-10, 11) * 0.01f;
				switch (direction)
				{
					case -1:
						if (vector2.X > -0.5)
						{
							vector2.X = -0.5f;
						}
						else if (vector2.X < -2f)
						{
							vector2.X = -2f;
						}
						break;
					case 1:
						if (vector2.X < 0.5)
						{
							vector2.X = 0.5f;
						}
						else if (vector2.X > 2f)
						{
							vector2.X = 2f;
						}
						break;
				}
				num3 = (int)(vector.X - num2 * 1.1f);
				num4 = (int)(vector.X + num2 * 1.1f);
				num5 = (int)(vector.Y - num2 * 1.1f);
				num6 = (int)(vector.Y + num2 * 1.1f);
				if (num3 < 1)
				{
					num3 = 1;
				}
				if (num4 > Main.MaxTilesX - 1)
				{
					num4 = Main.MaxTilesX - 1;
				}
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesY)
				{
					num6 = Main.MaxTilesY;
				}
				for (int m = num3; m < num4; m++)
				{
					for (int n = num5; n < num6; n++)
					{
						if (Math.Abs(m - vector.X) + Math.Abs(n - vector.Y) < num2 * 1.1f * (1f + genRand.Next(-10, 11) * 0.015f) && Main.TileSet[m, n].WallType != (byte)EntityID.WallID.EBONSTONE_UNSAFE)
						{
							if (Main.TileSet[m, n].Type != (byte)EntityID.TileID.EBONSTONE && n > j + genRand.Next(3, 20))
							{
								Main.TileSet[m, n].IsActive = 1;
							}
							Main.TileSet[m, n].IsActive = 1;
							if (Main.TileSet[m, n].Type != (byte)EntityID.TileID.SHADOW_ORB && Main.TileSet[m, n].Type != (byte)EntityID.TileID.DEMONITE_ORE)
							{
								Main.TileSet[m, n].Type = (byte)EntityID.TileID.EBONSTONE;
							}
							if (Main.TileSet[m, n].WallType == (byte)EntityID.WallID.DIRT_UNSAFE)
							{
								Main.TileSet[m, n].WallType = (byte)EntityID.WallID.NONE;
							}
						}
					}
				}
				for (int num7 = num3; num7 < num4; num7++)
				{
					for (int num8 = num5; num8 < num6; num8++)
					{
						if (Math.Abs(num7 - vector.X) + Math.Abs(num8 - vector.Y) < num2 * 1.1f * (1f + genRand.Next(-10, 11) * 0.015f) && Main.TileSet[num7, num8].WallType != (byte)EntityID.WallID.EBONSTONE_UNSAFE)
						{
							if (Main.TileSet[num7, num8].Type != (byte)EntityID.TileID.SHADOW_ORB && Main.TileSet[num7, num8].Type != (byte)EntityID.TileID.DEMONITE_ORE)
							{
								Main.TileSet[num7, num8].Type = (byte)EntityID.TileID.EBONSTONE;
							}
							Main.TileSet[num7, num8].IsActive = 1;
							if (Main.TileSet[num7, num8].WallType == (byte)EntityID.WallID.NONE)
							{
								Main.TileSet[num7, num8].WallType = (byte)EntityID.WallID.EBONSTONE_UNSAFE;
							}
						}
					}
				}
			}
			if (genRand.Next(3) == 0)
			{
				int num9 = (int)vector.X;
				int num10;
				for (num10 = (int)vector.Y; Main.TileSet[num9, num10].IsActive == 0; num10++)
				{
				}
				TileRunner(num9, num10, genRand.Next(2, 6), genRand.Next(3, 7), (byte)EntityID.TileID.DEMONITE_ORE);
			}
		}

		public static void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
		{
			bool flag = false;
			bool flag2 = !makeOrb;
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			float num = steps;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(11) * 0.2f + 0.5f;
			int num2 = 5;
			int num3 = genRand.Next(5) + 7;
			while (num3 > 0)
			{
				if (num > 0f)
				{
					num3 += genRand.Next(3);
					num3 -= genRand.Next(3);
					if (num3 < 7)
					{
						num3 = 7;
					}
					else if (num3 > 20)
					{
						num3 = 20;
					}
					if (num == 1f && num3 < 10)
					{
						num3 = 10;
					}
				}
				else if (vector.Y > Main.WorldSurface + 45)
				{
					num3 -= genRand.Next(4);
				}
				if (vector.Y > Main.RockLayer && num > 0f)
				{
					num = 0f;
				}
				num -= 1f;
				if (!flag && vector.Y > Main.WorldSurface + 20)
				{
					flag = true;
					ChasmRunnerSideways((int)vector.X, (int)vector.Y, -1, genRand.Next(20, 40));
					ChasmRunnerSideways((int)vector.X, (int)vector.Y, 1, genRand.Next(20, 40));
				}
				int num4;
				int num5;
				int num6;
				int num7;
				if (num > num2)
				{
					num4 = (int)(vector.X - num3 * 0.5f);
					num5 = (int)(vector.X + num3 * 0.5f);
					num6 = (int)(vector.Y - num3 * 0.5f);
					num7 = (int)(vector.Y + num3 * 0.5f);
					if (num4 < 0)
					{
						num4 = 0;
					}
					if (num5 > Main.MaxTilesX - 1)
					{
						num5 = Main.MaxTilesX - 1;
					}
					if (num6 < 0)
					{
						num6 = 0;
					}
					if (num7 > Main.MaxTilesY)
					{
						num7 = Main.MaxTilesY;
					}
					for (int k = num4; k < num5; k++)
					{
						for (int l = num6; l < num7; l++)
						{
							if (Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < num3 * 0.5f * (1f + genRand.Next(-10, 11) * 0.015f) && Main.TileSet[k, l].Type != (byte)EntityID.TileID.SHADOW_ORB && Main.TileSet[k, l].Type != (byte)EntityID.TileID.DEMONITE_ORE)
							{
								Main.TileSet[k, l].IsActive = 0;
							}
						}
					}
				}
				if (num <= 2f && vector.Y < Main.WorldSurface + 45)
				{
					num = 2f;
				}
				if (num <= 0f)
				{
					if (!flag2)
					{
						flag2 = true;
						AddShadowOrb((int)vector.X, (int)vector.Y);
					}
					else
					{
						for (int m = 0; m < 10000; m++)
						{
							int num8 = genRand.Next((int)vector.Y - 50, (int)vector.Y);
							if (num8 <= Main.WorldSurface)
							{
								break;
							}
							if (num8 > Main.MaxTilesY - 5)
							{
								num8 = Main.MaxTilesY - 5;
							}
							int num9 = genRand.Next((int)vector.X - 25, (int)vector.X + 25);
							if (num9 < 5)
							{
								num9 = 5;
							}
							else if (num9 > Main.MaxTilesX - 5)
							{
								num9 = Main.MaxTilesX - 5;
							}
							if (Place3x2(num9, num8, (byte)EntityID.TileID.DEMON_ALTAR))
							{
								break;
							}
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.01f;
				if (vector2.X > 0.3f)
				{
					vector2.X = 0.3f;
				}
				else if (vector2.X < -0.3f)
				{
					vector2.X = -0.3f;
				}
				num4 = (int)(vector.X - num3 * 1.1f);
				num5 = (int)(vector.X + num3 * 1.1f);
				num6 = (int)(vector.Y - num3 * 1.1f);
				num7 = (int)(vector.Y + num3 * 1.1f);
				if (num4 < 1)
				{
					num4 = 1;
				}
				if (num5 > Main.MaxTilesX - 1)
				{
					num5 = Main.MaxTilesX - 1;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				for (int n = num4; n < num5; n++)
				{
					for (int num10 = num6; num10 < num7; num10++)
					{
						if (Math.Abs(n - vector.X) + Math.Abs(num10 - vector.Y) < num3 * 1.1f * (1f + genRand.Next(-10, 11) * 0.015f))
						{
							if (Main.TileSet[n, num10].Type != (byte)EntityID.TileID.EBONSTONE && num10 > j + genRand.Next(3, 20))
							{
								Main.TileSet[n, num10].IsActive = 1;
							}
							if (steps <= num2)
							{
								Main.TileSet[n, num10].IsActive = 1;
							}
							if (Main.TileSet[n, num10].Type != (byte)EntityID.TileID.SHADOW_ORB)
							{
								Main.TileSet[n, num10].Type = (byte)EntityID.TileID.EBONSTONE;
							}
							if (Main.TileSet[n, num10].WallType == (byte)EntityID.WallID.DIRT_UNSAFE)
							{
								Main.TileSet[n, num10].WallType = (byte)EntityID.WallID.NONE;
							}
						}
					}
				}
				for (int num11 = num4; num11 < num5; num11++)
				{
					for (int num12 = num6; num12 < num7; num12++)
					{
						if (Math.Abs(num11 - vector.X) + Math.Abs(num12 - vector.Y) < num3 * 1.1f * (1f + genRand.Next(-10, 11) * 0.015f))
						{
							if (Main.TileSet[num11, num12].Type != (byte)EntityID.TileID.SHADOW_ORB)
							{
								Main.TileSet[num11, num12].Type = (byte)EntityID.TileID.EBONSTONE;
							}
							if (steps <= num2)
							{
								Main.TileSet[num11, num12].IsActive = 1;
							}
							if (num12 > j + genRand.Next(3, 20) && Main.TileSet[num11, num12].WallType == (byte)EntityID.WallID.NONE)
							{
								Main.TileSet[num11, num12].WallType = (byte)EntityID.WallID.EBONSTONE_UNSAFE;
							}
						}
					}
				}
			}
		}

		public static void JungleRunner(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			double num = genRand.Next(5, 11);
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(10, 20) * 0.1f;
			int num2 = 0;
			bool flag = true;
			do
			{
				int num3 = (int)vector.X;
				int num4 = (int)vector.Y;
				if (num4 < Main.WorldSurface && Main.TileSet[num3, num4].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4].IsActive == 0 && Main.TileSet[num3, num4 - 3].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4 - 3].IsActive == 0 && Main.TileSet[num3, num4 - 1].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4 - 1].IsActive == 0 && Main.TileSet[num3, num4 - 4].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4 - 4].IsActive == 0 && Main.TileSet[num3, num4 - 2].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4 - 2].IsActive == 0 && Main.TileSet[num3, num4 - 5].WallType == (byte)EntityID.WallID.NONE && Main.TileSet[num3, num4 - 5].IsActive == 0)
				{
					flag = false;
				}
				JungleX = num3;
				num += (double)(genRand.Next(-20, 21) * 0.1f);
				if (num < 5.0)
				{
					num = 5.0;
				}
				else if (num > 10.0)
				{
					num = 10.0;
				}
				int num5 = (int)(vector.X - num * 0.5);
				int num6 = (int)(vector.X + num * 0.5);
				int num7 = (int)(vector.Y - num * 0.5);
				int num8 = (int)(vector.Y + num * 0.5);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesX)
				{
					num6 = Main.MaxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesY)
				{
					num8 = Main.MaxTilesY;
				}
				for (int k = num5; k < num6; k++)
				{
					for (int l = num7; l < num8; l++)
					{
						if ((double)(Math.Abs(k - vector.X) + Math.Abs(l - vector.Y)) < num * 0.5 * (1.0 + genRand.Next(-10, 11) * 0.015))
						{
							KillTileFast(k, l);
						}
					}
				}
				if (++num2 > 10 && genRand.Next(50) < num2)
				{
					num2 = 0;
					int num9 = -2;
					if (genRand.Next(2) == 0)
					{
						num9 = 2;
					}
					TileRunner((int)vector.X, (int)vector.Y, genRand.Next(3, 20), genRand.Next(10, 100), (int)EntityID.TileID.NONE, addTile: false, new Vector2(num9, 0f));
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.Y += genRand.Next(-10, 11) * 0.01f;
				if (vector2.Y > 0f)
				{
					vector2.Y = 0f;
				}
				else if (vector2.Y < -2f)
				{
					vector2.Y = -2f;
				}
				vector2.X += genRand.Next(-10, 11) * 0.1f;
				if (vector.X < i - 200)
				{
					vector2.X += genRand.Next(5, 21) * 0.1f;
				}
				if (vector.X > i + 200)
				{
					vector2.X -= genRand.Next(5, 21) * 0.1f;
				}
				if (vector2.X > 1.5f)
				{
					vector2.X = 1.5f;
				}
				else if (vector2.X < -1.5f)
				{
					vector2.X = -1.5f;
				}
			}
			while (flag);
		}

		public static void GERunner(int i, Vector2 speed, bool good, ref Vector2i minArea, ref Vector2i maxArea)
		{
			Vector2 vector = new Vector2(i, 0f);
			Vector2 vector2 = speed;
			int num = genRand.Next(200, 250);
			float num2 = Main.MaxTilesX / 4200f;
			num = (int)(num * num2);
			int num3 = num;
			while (true)
			{
				int num4 = (int)(vector.X - num3 * 0.5f);
				int num5 = (int)(vector.X + num3 * 0.5f);
				int num6 = (int)(vector.Y - num3 * 0.5f);
				int num7 = (int)(vector.Y + num3 * 0.5f);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				for (int j = num4; j < num5; j++)
				{
					for (int k = num6; k < num7; k++)
					{
						if (!((double)(Math.Abs(j - vector.X) + Math.Abs(k - vector.Y)) < num * 0.5 * (1.0 + genRand.Next(-10, 11) * 0.015)))
						{
							continue;
						}
						EntityID.TileID num8 = EntityID.TileID.DIRT;
						if (good)
						{
							if (Main.TileSet[j, k].WallType == (byte)EntityID.WallID.EBONSTONE_UNSAFE)
							{
								Main.TileSet[j, k].WallType = (byte)EntityID.WallID.PEARLSTONE_BRICK_UNSAFE;
							}
							switch ((EntityID.TileID)Main.TileSet[j, k].Type)
							{
								case EntityID.TileID.STONE:
								case EntityID.TileID.EBONSTONE:
									num8 = EntityID.TileID.PEARLSTONE;
									break;
								case EntityID.TileID.GRASS:
								case EntityID.TileID.CORRUPT_GRASS:
									num8 = EntityID.TileID.HALLOWED_GRASS;
									break;
								case EntityID.TileID.SAND:
								case EntityID.TileID.EBONSAND:
								case EntityID.TileID.SILT:
									num8 = EntityID.TileID.PEARLSAND;
									break;
							}
						}
						else
						{
							switch ((EntityID.TileID)Main.TileSet[j, k].Type)
							{
								case EntityID.TileID.STONE:
								case EntityID.TileID.PEARLSTONE:
									num8 = EntityID.TileID.EBONSTONE;
									break;
								case EntityID.TileID.GRASS:
								case EntityID.TileID.HALLOWED_GRASS:
									num8 = EntityID.TileID.CORRUPT_GRASS;
									break;
								case EntityID.TileID.SAND:
								case EntityID.TileID.PEARLSAND:
								case EntityID.TileID.SILT:
									num8 = EntityID.TileID.EBONSAND;
									break;
							}
						}
						if ((byte)num8 > 0)
						{
							if (j < minArea.X)
							{
								minArea.X = j;
							}
							if (k < minArea.Y)
							{
								minArea.Y = k;
							}
							if (j > maxArea.X)
							{
								maxArea.X = j;
							}
							if (k > maxArea.Y)
							{
								maxArea.Y = k;
							}
							Main.TileSet[j, k].Type = (byte)num8;
							SquareTileFrameNoLiquid(j, k);
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				if (vector.X < -num || vector.Y < -num || vector.X > Main.MaxTilesX + num || vector.Y > Main.MaxTilesX + num)
				{
					break;
				}
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > speed.X + 1f)
				{
					vector2.X = speed.X + 1f;
				}
				else if (vector2.X < speed.X - 1f)
				{
					vector2.X = speed.X - 1f;
				}
			}
		}

		public unsafe static void TileRunner(int i, int j, int strength, int steps, int type, bool addTile = false, Vector2 velocity = default, bool noYChange = false, bool overRide = true)
		{
			Vector2 vector = new Vector2(i, j);
			float num = strength;
			int num2 = steps;
			float num3 = 1f / steps;
			if (velocity.X == 0f && velocity.Y == 0f)
			{
				velocity.X = genRand.Next(-10, 11) * 0.1f;
				velocity.Y = genRand.Next(-10, 11) * 0.1f;
			}
			while (num > 0f && num2 > 0)
			{
				if (vector.Y < 0f && type == (int)EntityID.TileID.MUD)
				{
					num2 = 0;
				}
				num = strength * (num2 * num3);
				num2--;
				int num4 = (int)(vector.Y - num * 0.5f);
				int num5 = (int)(vector.Y + num * 0.5f);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesY)
				{
					num5 = Main.MaxTilesY;
				}
				if (num4 < num5)
				{
					int num6 = (int)(vector.X - num * 0.5f);
					int num7 = (int)(vector.X + num * 0.5f);
					if (num6 < 0)
					{
						num6 = 0;
					}
					if (num7 > Main.MaxTilesX)
					{
						num7 = Main.MaxTilesX;
					}
					fixed (Tile* ptr = Main.TileSet)
					{
						for (int k = num6; k < num7; k++)
						{
							int num8 = num4;
							Tile* ptr2 = ptr + (k * (Main.LargeWorldH) + num8);
							do
							{
								if (Math.Abs(k - vector.X) + Math.Abs(num8 - vector.Y) < strength * 0.5f * (1f + genRand.Next(-10, 11) * 0.015f))
								{
									if (mudWall && num8 > Main.WorldSurface && num8 < Main.MaxTilesY - 210 - genRand.Next(3) && ptr2->WallType == (byte)EntityID.WallID.NONE)
									{
										ptr2->WallType = (byte)EntityID.WallID.MUD_UNSAFE;
									}
									if (type < 0)
									{
										if (type == (int)EntityID.TileID.LIQUID && ptr2->IsActive != 0 && (num8 < WaterLine || num8 > lavaLine))
										{
											ptr2->Liquid = byte.MaxValue;
											if (num8 > lavaLine)
											{
												ptr2->Lava = 32;
											}
										}
										ptr2->IsActive = 0;
									}
									else
									{
										if (overRide || ptr2->IsActive == 0)
										{
											int type2 = ptr2->Type;
											if ((type != (int)EntityID.TileID.CLAY || type2 != (int)EntityID.TileID.SAND) && (!Main.TileStone[type] || type2 == (int)EntityID.TileID.STONE) && type2 != (int)EntityID.TileID.GOLD_BRICK && type2 != (int)EntityID.TileID.SNOW && (type2 != (int)EntityID.TileID.STONE || type != (int)EntityID.TileID.MUD || num8 >= Main.WorldSurface + genRand.Next(-50, 50)))
											{
												if (type2 != (int)EntityID.TileID.SAND || num8 >= Main.WorldSurface)
												{
													ptr2->Type = (byte)type;
												}
												else if (type == (int)EntityID.TileID.MUD)
												{
													ptr2->Type = (byte)type;
												}
											}
										}
										if (addTile)
										{
											ptr2->IsActive = 1;
											ptr2->Liquid = 0;
											ptr2->Lava = 0;
										}
										if (type == (int)EntityID.TileID.MUD)
										{
											if (num8 > WaterLine && ptr2->Liquid > 0)
											{
												ptr2->Liquid = 0;
												ptr2->Lava = 0;
											}
										}
										else if (noYChange && num8 < Main.WorldSurface)
										{
											ptr2->WallType = (byte)EntityID.WallID.DIRT_UNSAFE;
										}
									}
								}
								ptr2++;
							}
							while (++num8 < num5);
						}
					}
				}
				vector.X += velocity.X;
				vector.Y += velocity.Y;

				// Why have this:
				/*
				if (num > 50f)
				{
					vector.X += velocity.X;
					vector.Y += velocity.Y;
					num2--;
					velocity.Y += genRand.Next(-10, 11) * 0.05f;
					velocity.X += genRand.Next(-10, 11) * 0.05f;
					if (num > 100f)
					{
						vector.X += velocity.X;
						vector.Y += velocity.Y;
						num2--;
						velocity.Y += genRand.Next(-10, 11) * 0.05f;
						velocity.X += genRand.Next(-10, 11) * 0.05f;
						if (num > 150f)
						{
							vector.X += velocity.X;
							vector.Y += velocity.Y;
							num2--;
							velocity.Y += genRand.Next(-10, 11) * 0.05f;
							velocity.X += genRand.Next(-10, 11) * 0.05f;
							if (num > 200f)
							{
								vector.X += velocity.X;
								vector.Y += velocity.Y;
								num2--;
								velocity.Y += genRand.Next(-10, 11) * 0.05f;
								velocity.X += genRand.Next(-10, 11) * 0.05f;
								if (num > 250f)
								{
									vector.X += velocity.X;
									vector.Y += velocity.Y;
									num2--;
									velocity.Y += genRand.Next(-10, 11) * 0.05f;
									velocity.X += genRand.Next(-10, 11) * 0.05f;
									if (num > 300f)
									{
										vector.X += velocity.X;
										vector.Y += velocity.Y;
										num2--;
										velocity.Y += genRand.Next(-10, 11) * 0.05f;
										velocity.X += genRand.Next(-10, 11) * 0.05f;
										if (num > 400f)
										{
											vector.X += velocity.X;
											vector.Y += velocity.Y;
											num2--;
											velocity.Y += genRand.Next(-10, 11) * 0.05f;
											velocity.X += genRand.Next(-10, 11) * 0.05f;
											if (num > 500f)
											{
												vector.X += velocity.X;
												vector.Y += velocity.Y;
												num2--;
												velocity.Y += genRand.Next(-10, 11) * 0.05f;
												velocity.X += genRand.Next(-10, 11) * 0.05f;
												if (num > 600f)
												{
													vector.X += velocity.X;
													vector.Y += velocity.Y;
													num2--;
													velocity.Y += genRand.Next(-10, 11) * 0.05f;
													velocity.X += genRand.Next(-10, 11) * 0.05f;
													if (num > 700f)
													{
														vector.X += velocity.X;
														vector.Y += velocity.Y;
														num2--;
														velocity.Y += genRand.Next(-10, 11) * 0.05f;
														velocity.X += genRand.Next(-10, 11) * 0.05f;
														if (num > 800f)
														{
															vector.X += velocity.X;
															vector.Y += velocity.Y;
															num2--;
															velocity.Y += genRand.Next(-10, 11) * 0.05f;
															velocity.X += genRand.Next(-10, 11) * 0.05f;
															if (num > 900f)
															{
																vector.X += velocity.X;
																vector.Y += velocity.Y;
																num2--;
																velocity.Y += genRand.Next(-10, 11) * 0.05f;
																velocity.X += genRand.Next(-10, 11) * 0.05f;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				*/

				int steps50 = (int)Math.Max(0.0, Math.Min(6.0, Math.Ceiling(((double)num - 50.0) / 50.0)));
				int steps100 = (int)Math.Max(0.0, Math.Min(6.0, Math.Ceiling(((double)num - 400.0) / 100.0)));

				for (int step = 0; step < steps50 + steps100; step++)
				{
					vector.X += velocity.X;
					vector.Y += velocity.Y;
					num2--;
					velocity.Y += genRand.Next(-10, 11) * 0.05f;
					velocity.X += genRand.Next(-10, 11) * 0.05f;
				}

				velocity.X += genRand.Next(-10, 11) * 0.05f;
				if (velocity.X > 1f)
				{
					velocity.X = 1f;
				}
				else if (velocity.X < -1f)
				{
					velocity.X = -1f;
				}
				if (!noYChange)
				{
					velocity.Y += genRand.Next(-10, 11) * 0.05f;
					if (velocity.Y > 1f)
					{
						velocity.Y = 1f;
					}
					else if (velocity.Y < -1f)
					{
						velocity.Y = -1f;
					}
					if (type == (int)EntityID.TileID.MUD)
					{
						int num9 = (int)vector.Y;
						if (num9 < Main.RockLayer + 100)
						{
							velocity.Y = 1f;
						}
						else if (num9 > Main.MaxTilesY - 300)
						{
							velocity.Y = -1f;
						}
						else if (velocity.Y > 0.5f)
						{
							velocity.Y = 0.5f;
						}
						else if (velocity.Y < -0.5f)
						{
							velocity.Y = -0.5f;
						}
					}
				}
				else if (type != (int)EntityID.TileID.MUD && num < 3f)
				{
					if (velocity.Y > 1f)
					{
						velocity.Y = 1f;
					}
					else if (velocity.Y < -1f)
					{
						velocity.Y = -1f;
					}
				}
			}
		}

		public static void MudWallRunner(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			float num = genRand.Next(5, 15);
			float num2 = genRand.Next(5, 20);
			float num3 = num2;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-10, 11) * 0.1f;
			while (num > 0f && num3 > 0f)
			{
				float num4 = num * (num3 / num2);
				num3 -= 1f;
				int num5 = (int)(vector.X - num4 * 0.5f);
				int num6 = (int)(vector.X + num4 * 0.5f);
				int num7 = (int)(vector.Y - num4 * 0.5f);
				int num8 = (int)(vector.Y + num4 * 0.5f);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesX)
				{
					num6 = Main.MaxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesY)
				{
					num8 = Main.MaxTilesY;
				}
				for (int k = num5; k < num6; k++)
				{
					float num9 = Math.Abs(k - vector.X);
					for (int l = num7; l < num8; l++)
					{
						if (num9 + Math.Abs(l - vector.Y) < num * 0.5f * (1f + genRand.Next(-10, 11) * 0.015f))
						{
							Main.TileSet[k, l].WallType = (byte)EntityID.WallID.NONE;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 1f)
				{
					vector2.X = 1f;
				}
				else if (vector2.X < -1f)
				{
					vector2.X = -1f;
				}
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.Y > 1f)
				{
					vector2.Y = 1f;
				}
				else if (vector2.Y < -1f)
				{
					vector2.Y = -1f;
				}
			}
		}

		public static void FloatingIsland(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			float num = genRand.Next(80, 120);
			float num2 = num;
			float num3 = genRand.Next(20, 25);
			vector2.X = genRand.Next(-20, 21) * 0.2f;
			while (vector2.X > -2f && vector2.X < 2f)
			{
				vector2.X = genRand.Next(-20, 21) * 0.2f;
			}
			vector2.Y = genRand.Next(-20, -10) * 0.02f;
			while (num > 0f && num3 > 0f)
			{
				num -= genRand.Next(4);
				num3 -= 1f;
				int num4 = (int)(vector.X - num * 0.5f);
				int num5 = (int)(vector.X + num * 0.5f);
				int num6 = (int)(vector.Y - num * 0.5f);
				int num7 = (int)(vector.Y + num * 0.5f);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01f;
				float num8 = num2 * 0.4f;
				num8 *= num8;
				int num9 = (int)vector.Y + 1;
				for (int k = num4; k < num5; k++)
				{
					if (genRand.Next(2) == 0)
					{
						num9 += genRand.Next(-1, 2);
					}
					if (num9 < (int)vector.Y)
					{
						num9 = (int)vector.Y;
					}
					else if (num9 > (int)vector.Y + 2)
					{
						num9 = (int)vector.Y + 2;
					}
					float num10 = k - vector.X;
					num10 *= num10;
					for (int l = ((num6 < num9) ? num9 : num6); l < num7; l++)
					{
						float num11 = (l - vector.Y) * 2f;
						float num12 = num10 + num11 * num11;
						if (num12 < num8)
						{
							Main.TileSet[k, l].IsActive = 1;
							if (Main.TileSet[k, l].Type == (byte)EntityID.TileID.MUD)
							{
								Main.TileSet[k, l].Type = (byte)EntityID.TileID.DIRT;
							}
						}
					}
				}
				TileRunner(genRand.Next(num4 + 10, num5 - 10), (int)(vector.Y + num2 * 0.1f + 5f), genRand.Next(5, 10), genRand.Next(10, 15), (int)EntityID.TileID.DIRT, addTile: true, new Vector2(0f, 2f), noYChange: true);
				num4 = (int)(vector.X - num * 0.4f);
				num5 = (int)(vector.X + num * 0.4f);
				num6 = (int)(vector.Y - num * 0.4f);
				num7 = (int)(vector.Y + num * 0.4f);
				num9 = (int)vector.Y + 2;
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < num9)
				{
					num6 = num9;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01f;
				num2 *= 0.4f;
				num2 *= num2;
				for (int m = num4; m < num5; m++)
				{
					float num13 = m - vector.X;
					num13 *= num13;
					for (int n = num6; n < num7; n++)
					{
						float num14 = (n - vector.Y) * 2f;
						float num15 = num13 + num14 * num14;
						if (num15 < num2)
						{
							Main.TileSet[m, n].WallType = (byte)EntityID.WallID.DIRT_UNSAFE;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 1f)
				{
					vector2.X = 1f;
				}
				else if (vector2.X < -1f)
				{
					vector2.X = -1f;
				}
				if (vector2.Y > 0.2f)
				{
					vector2.Y = -0.2f;
				}
				else if (vector2.Y < -0.2f)
				{
					vector2.Y = -0.2f;
				}
			}
		}

		public static void Caverer(int X, int Y)
		{
			int num = genRand.Next(2);
			double num2 = genRand.Next(100) * 0.01;
			double num3 = 1.0 - num2;
			if (genRand.Next(2) == 0)
			{
				num2 = 0.0 - num2;
			}
			if (genRand.Next(2) == 0)
			{
				num3 = 0.0 - num3;
			}
			Vector2 pos = new Vector2(X, Y);
			if (num == 0)
			{
				for (int num4 = genRand.Next(6, 8); num4 >= 0; num4--)
				{
					digTunnel(ref pos, num2, num3, genRand.Next(6, 20), genRand.Next(4, 9));
					num2 += genRand.Next(-20, 21) * 0.1;
					num3 += genRand.Next(-20, 21) * 0.1;
					if (num2 < -1.5)
					{
						num2 = -1.5;
					}
					else if (num2 > 1.5)
					{
						num2 = 1.5;
					}
					if (num3 < -1.5)
					{
						num3 = -1.5;
					}
					else if (num3 > 1.5)
					{
						num3 = 1.5;
					}
					double num5 = genRand.Next(100) * 0.01;
					double num6 = 1.0 - num5;
					if (genRand.Next(2) == 0)
					{
						num5 = 0.0 - num5;
					}
					if (genRand.Next(2) == 0)
					{
						num6 = 0.0 - num6;
					}
					Vector2 pos2 = pos;
					digTunnel(ref pos2, num5, num6, genRand.Next(30, 50), genRand.Next(3, 6));
					TileRunner((int)pos2.X, (int)pos2.Y, genRand.Next(10, 20), genRand.Next(5, 10), (int)EntityID.TileID.NONE);
				}
				return;
			}
			for (int num7 = genRand.Next(14, 29); num7 >= 0; num7--)
			{
				digTunnel(ref pos, num2, num3, genRand.Next(5, 15), genRand.Next(2, 6), Wet: true);
				num2 += genRand.Next(-20, 21) * 0.1;
				num3 += genRand.Next(-20, 21) * 0.1;
				if (num2 < -1.5)
				{
					num2 = -1.5;
				}
				else if (num2 > 1.5)
				{
					num2 = 1.5;
				}
				if (num3 < -1.5)
				{
					num3 = -1.5;
				}
				else if (num3 > 1.5)
				{
					num3 = 1.5;
				}
			}
		}

		public static void digTunnel(ref Vector2 pos, double xDir, double yDir, int Steps, int Size, bool Wet = false)
		{
			try
			{
				double num = 0.0;
				double num2 = 0.0;
				double num3 = Size;
				while (Steps > 0)
				{
					Steps--;
					for (int i = (int)(pos.X - num3); i <= pos.X + num3; i++)
					{
						float num4 = Math.Abs(i - pos.X);
						for (int j = (int)(pos.Y - num3); j <= pos.Y + num3; j++)
						{
							if ((double)(num4 + Math.Abs(j - pos.Y)) < num3 * (1.0 + genRand.Next(-10, 11) * 0.005))
							{
								Main.TileSet[i, j].IsActive = 0;
								if (Wet)
								{
									Main.TileSet[i, j].Liquid = byte.MaxValue;
								}
							}
						}
					}
					num3 += genRand.Next(-50, 51) * 0.03;
					if (num3 < Size * 0.6)
					{
						num3 = Size * 0.6;
					}
					else if (num3 > Size * 2)
					{
						num3 = Size * 2;
					}
					num += genRand.Next(-20, 21) * 0.01;
					num2 += genRand.Next(-20, 21) * 0.01;
					if (num < -1.0)
					{
						num = -1.0;
					}
					else if (num > 1.0)
					{
						num = 1.0;
					}
					if (num2 < -1.0)
					{
						num2 = -1.0;
					}
					else if (num2 > 1.0)
					{
						num2 = 1.0;
					}
					pos.X = (float)(pos.X + (xDir + num) * 0.6);
					pos.Y = (float)(pos.Y + (yDir + num2) * 0.6);
				}
			}
			catch
			{
			}
		}

		public static void IslandHouse(int i, int j)
		{
			byte type = (byte)genRand.Next((int)EntityID.TileID.GOLD_BRICK, (int)EntityID.TileID.COPPER_BRICK + 1);
			byte wall = (byte)genRand.Next((int)EntityID.WallID.GOLD_BRICK, (int)EntityID.WallID.COPPER_BRICK + 1);
			Vector2 vector = new Vector2(i, j);
			int num = 1;
			if (genRand.Next(2) == 0)
			{
				num = -1;
			}
			int num2 = genRand.Next(7, 12);
			int num3 = genRand.Next(5, 7);
			vector.X = i + (num2 + 2) * num;
			for (int k = j - 15; k < j + 30; k++)
			{
				if (Main.TileSet[(int)vector.X, k].IsActive != 0)
				{
					vector.Y = k - 1;
					break;
				}
			}
			vector.X = i;
			int num4 = (int)(vector.X - num2 - 2f);
			int num5 = (int)(vector.X + num2 + 2f);
			int num6 = (int)(vector.Y - num3 - 2f);
			int num7 = (int)(vector.Y + 2f + genRand.Next(3, 5));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.MaxTilesX)
			{
				num5 = Main.MaxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.MaxTilesY)
			{
				num7 = Main.MaxTilesY;
			}
			for (int l = num4; l <= num5; l++)
			{
				for (int m = num6; m < num7; m++)
				{
					Main.TileSet[l, m].IsActive = 1;
					Main.TileSet[l, m].Type = type;
					Main.TileSet[l, m].WallType = (byte)EntityID.WallID.NONE;
				}
			}
			num4 = (int)(vector.X - num2);
			num5 = (int)(vector.X + num2);
			num6 = (int)(vector.Y - num3);
			num7 = (int)(vector.Y + 1f);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.MaxTilesX)
			{
				num5 = Main.MaxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.MaxTilesY)
			{
				num7 = Main.MaxTilesY;
			}
			for (int n = num4; n <= num5; n++)
			{
				for (int num8 = num6; num8 < num7; num8++)
				{
					if (Main.TileSet[n, num8].WallType == (byte)EntityID.WallID.NONE)
					{
						Main.TileSet[n, num8].IsActive = 0;
						Main.TileSet[n, num8].WallType = wall;
					}
				}
			}
			int num9 = i + (num2 + 1) * num;
			int num10 = (int)vector.Y;
			for (int num11 = num9 - 2; num11 <= num9 + 2; num11++)
			{
				Main.TileSet[num11, num10].IsActive = 0;
				Main.TileSet[num11, num10 - 1].IsActive = 0;
				Main.TileSet[num11, num10 - 2].IsActive = 0;
			}
			PlaceTile(num9, num10, (int)EntityID.TileID.DOOR_CLOSED, ToMute: true);
			int contain = 0;
			int num12 = houseCount;
			if (num12 > 2)
			{
				num12 = genRand.Next(3);
			}
			switch (num12)
			{
				case 0:
					contain = (int)EntityID.ItemID.SHINY_RED_BALLOON;
					break;
				case 1:
					contain = (int)EntityID.ItemID.STARFURY;
					break;
				case 2:
					contain = (int)EntityID.ItemID.LUCKY_HORSESHOE;
					break;
			}
			AddBuriedChest(i, num10 - 3, contain, notNearOtherChests: false, 2);
			houseCount++;
		}

		public static void Mountinater(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			double num = genRand.Next(80, 120);
			double num2 = num;
			float num3 = genRand.Next(40, 55);
			vector.Y += num3 * 0.5f;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-20, -10) * 0.1f;
			while (num > 0.0 && num3 > 0f)
			{
				num -= genRand.Next(4);
				num3 -= 1f;
				int num4 = (int)(vector.X - num * 0.5);
				int num5 = (int)(vector.X + num * 0.5);
				int num6 = (int)(vector.Y - num * 0.5);
				int num7 = (int)(vector.Y + num * 0.5);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01;
				num2 *= 0.4;
				num2 *= num2;
				for (int k = num4; k < num5; k++)
				{
					double num8 = k - vector.X;
					num8 *= num8;
					for (int l = num6; l < num7; l++)
					{
						double num9 = l - vector.Y;
						double num10 = num8 + num9 * num9;
						if (num10 < num2 && Main.TileSet[k, l].IsActive == 0)
						{
							Main.TileSet[k, l].IsActive = 1;
							Main.TileSet[k, l].Type = (int)EntityID.TileID.DIRT;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 0.5f)
				{
					vector2.X = 0.5f;
				}
				else if (vector2.X < -0.5f)
				{
					vector2.X = -0.5f;
				}
				if (vector2.Y > -0.5f)
				{
					vector2.Y = -0.5f;
				}
				else if (vector2.Y < -1.5f)
				{
					vector2.Y = -1.5f;
				}
			}
		}

		public static void Lakinater(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			double num = genRand.Next(25, 50);
			double num2 = num;
			double num3 = genRand.Next(30, 80);
			if (genRand.Next(5) == 0)
			{
				num *= 1.5;
				num2 *= 1.5;
				num3 *= 1.2;
			}
			vector.Y = (float)(vector.Y - num3 * 0.3);
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-20, -10) * 0.1f;
			while (num > 0.0 && num3 > 0.0)
			{
				if (vector.Y + num2 * 0.5 > Main.WorldSurface)
				{
					num3 = 0.0;
				}
				num -= genRand.Next(3);
				num3 -= 1.0;
				int num4 = (int)(vector.X - num * 0.5);
				int num5 = (int)(vector.X + num * 0.5);
				int num6 = (int)(vector.Y - num * 0.5);
				int num7 = (int)(vector.Y + num * 0.5);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01;
				num2 *= 0.4;
				num2 *= num2;
				for (int k = num4; k < num5; k++)
				{
					double num8 = k - vector.X;
					num8 *= num8;
					for (int l = num6; l < num7; l++)
					{
						double num9 = l - vector.Y;
						double num10 = num8 + num9 * num9;
						if (num10 < num2 && Main.TileSet[k, l].IsActive != 0)
						{
							Main.TileSet[k, l].IsActive = 0;
							Main.TileSet[k, l].Liquid = byte.MaxValue;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > 0.5f)
				{
					vector2.X = 0.5f;
				}
				else if (vector2.X < -0.5f)
				{
					vector2.X = -0.5f;
				}
				if (vector2.Y > 1.5f)
				{
					vector2.Y = 1.5f;
				}
				else if (vector2.Y < 0.5f)
				{
					vector2.Y = 0.5f;
				}
			}
		}

		public static void ShroomPatch(int i, int j)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			double num = genRand.Next(40, 70);
			double num2 = num;
			double num3 = genRand.Next(20, 30);
			if (genRand.Next(5) == 0)
			{
				num *= 1.5;
				num2 *= 1.5;
				num3 *= 1.2;
			}
			vector.Y -= (float)(num3 * 0.3);
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-20, -10) * 0.1f;
			while (num > 0.0 && num3 > 0.0)
			{
				num -= genRand.Next(3);
				num3 -= 1.0;
				int num4 = (int)(vector.X - num * 0.5);
				int num5 = (int)(vector.X + num * 0.5);
				int num6 = (int)(vector.Y - num * 0.5);
				int num7 = (int)(vector.Y + num * 0.5);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.MaxTilesX)
				{
					num5 = Main.MaxTilesX;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.MaxTilesY)
				{
					num7 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01;
				float num8 = (float)num2 * 0.4f;
				num8 *= num8;
				for (int k = num4; k < num5; k++)
				{
					float num9 = k - vector.X;
					num9 *= num9;
					for (int l = num6; l < num7; l++)
					{
						float num10 = (l - vector.Y) * 2.3f;
						float num11 = num9 + num10 * num10;
						if (!(num11 < num8))
						{
							continue;
						}
						if (l < vector.Y + num2 * 0.02)
						{
							if (Main.TileSet[k, l].Type != (byte)EntityID.TileID.MUD)
							{
								Main.TileSet[k, l].IsActive = 0;
							}
						}
						else
						{
							Main.TileSet[k, l].Type = (byte)EntityID.TileID.MUD;
						}
						Main.TileSet[k, l].Liquid = 0;
						Main.TileSet[k, l].Lava = 0;
					}
				}
				vector.X += vector2.X;
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y -= genRand.Next(11) * 0.05f;
				if (vector2.X > -0.5f && vector2.X < 0.5f)
				{
					if (vector2.X < 0f)
					{
						vector2.X = -0.5f;
					}
					else
					{
						vector2.X = 0.5f;
					}
				}
				if (vector2.X > 2f)
				{
					vector2.X = 1f;
				}
				else if (vector2.X < -2f)
				{
					vector2.X = -1f;
				}
				if (vector2.Y > 1f)
				{
					vector2.Y = 1f;
				}
				else if (vector2.Y < -1f)
				{
					vector2.Y = -1f;
				}
				int num12 = (int)vector.X;
				int num13 = (int)vector.Y;
				for (int m = 0; m < 2; m++)
				{
					int num14;
					int num15;
					do
					{
						num14 = num12 + genRand.Next(-20, 20);
						num15 = num13 + genRand.Next(20);
					}
					while (Main.TileSet[num14, num15].IsActive == 0 && Main.TileSet[num14, num15].Type != (byte)EntityID.TileID.MUD);
					int num16 = genRand.Next(7, 10);
					int num17 = genRand.Next(7, 10);
					TileRunner(num14, num15, num16, num17, (int)EntityID.TileID.MUD, addTile: false, new Vector2(0f, 2f), noYChange: true);
					if (genRand.Next(3) == 0)
					{
						TileRunner(num14, num15, num16 - 3, num17 - 3, (int)EntityID.TileID.NONE, addTile: false, new Vector2(0f, 2f), noYChange: true);
					}
				}
			}
		}

		public static void Cavinator(int i, int j, int steps)
		{
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = default;
			double num = genRand.Next(7, 15);
			double num2 = num;
			int num3 = 1;
			if (genRand.Next(2) == 0)
			{
				num3 = -1;
			}
			int num4 = genRand.Next(20, 40);
			vector2.Y = genRand.Next(10, 20) * 0.01f;
			vector2.X = num3;
			while (num4 > 0)
			{
				num4--;
				int num5 = (int)(vector.X - num * 0.5);
				int num6 = (int)(vector.X + num * 0.5);
				int num7 = (int)(vector.Y - num * 0.5);
				int num8 = (int)(vector.Y + num * 0.5);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesX)
				{
					num6 = Main.MaxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesY)
				{
					num8 = Main.MaxTilesY;
				}
				num2 = num * genRand.Next(80, 120) * 0.01;
				num2 *= 0.4;
				num2 *= num2;
				for (int k = num5; k < num6; k++)
				{
					double num9 = k - vector.X;
					num9 *= num9;
					for (int l = num7; l < num8; l++)
					{
						double num10 = l - vector.Y;
						double num11 = num9 + num10 * num10;
						if (num11 < num2)
						{
							Main.TileSet[k, l].IsActive = 0;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > num3 + 0.5f)
				{
					vector2.X = num3 + 0.5f;
				}
				else if (vector2.X < num3 - 0.5f)
				{
					vector2.X = num3 - 0.5f;
				}
				if (vector2.Y > 2f)
				{
					vector2.Y = 2f;
				}
				else if (vector2.Y < 0f)
				{
					vector2.Y = 0f;
				}
			}
			if (steps > 0 && (int)vector.Y < Main.RockLayer + 50)
			{
				Cavinator((int)vector.X, (int)vector.Y, steps - 1);
			}
		}

		public static void CaveOpenater(int i, int j)
		{
			int num = (genRand.Next(2) << 1) - 1;
			Vector2 vector = new Vector2(i, j);
			Vector2 vector2 = new Vector2(0f, num);
			double num2 = genRand.Next(7, 12);
			double num3 = num2;
			int num4 = 100;
			do
			{
				num4 = ((Main.TileSet[(int)vector.X, (int)vector.Y].WallType != (byte)EntityID.WallID.NONE) ? (num4 - 1) : 0);
				int num5 = (int)(vector.X - num2 * 0.5);
				int num6 = (int)(vector.X + num2 * 0.5);
				int num7 = (int)(vector.Y - num2 * 0.5);
				int num8 = (int)(vector.Y + num2 * 0.5);
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (num6 > Main.MaxTilesX)
				{
					num6 = Main.MaxTilesX;
				}
				if (num7 < 0)
				{
					num7 = 0;
				}
				if (num8 > Main.MaxTilesY)
				{
					num8 = Main.MaxTilesY;
				}
				num3 = num2 * genRand.Next(80, 120) * 0.01;
				num3 *= 0.4;
				num3 *= num3;
				for (int k = num5; k < num6; k++)
				{
					double num9 = k - vector.X;
					num9 *= num9;
					for (int l = num7; l < num8; l++)
					{
						double num10 = l - vector.Y;
						double num11 = num9 + num10 * num10;
						if (num11 < num3)
						{
							Main.TileSet[k, l].IsActive = 0;
						}
					}
				}
				vector.X += vector2.X;
				vector.Y += vector2.Y;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				if (vector2.X > num + 0.5f)
				{
					vector2.X = num + 0.5f;
				}
				else if (vector2.X < num - 0.5f)
				{
					vector2.X = num - 0.5f;
				}
				if (vector2.Y > 0f)
				{
					vector2.Y = 0f;
				}
				else if (vector2.Y < -0.5f)
				{
					vector2.Y = -0.5f;
				}
			}
			while (num4 > 0);
		}

		public static void SquareTileFrame(int i, int j, int frameNumber = -1)
		{
			if (!Gen)
			{
				bool flag = tileFrameRecursion;
				tileFrameRecursion = false;
				TileFrame(i - 1, j - 1);
				TileFrame(i - 1, j);
				TileFrame(i - 1, j + 1);
				TileFrame(i, j - 1);
				TileFrame(i, j, frameNumber);
				TileFrame(i, j + 1);
				TileFrame(i + 1, j - 1);
				TileFrame(i + 1, j);
				TileFrame(i + 1, j + 1);
				tileFrameRecursion = flag;
			}
		}

		public static void SquareTileFrameNoLiquid(int i, int j, int frameNumber = -1)
		{
			TileFrameNoLiquid(i - 1, j - 1);
			TileFrameNoLiquid(i - 1, j);
			TileFrameNoLiquid(i - 1, j + 1);
			TileFrameNoLiquid(i, j - 1);
			TileFrameNoLiquid(i, j, frameNumber);
			TileFrameNoLiquid(i, j + 1);
			TileFrameNoLiquid(i + 1, j - 1);
			TileFrameNoLiquid(i + 1, j);
			TileFrameNoLiquid(i + 1, j + 1);
		}

		public static void SquareWallFrame(int i, int j, bool resetFrame = true)
		{
			WallFrame(i - 1, j - 1);
			WallFrame(i - 1, j);
			WallFrame(i - 1, j + 1);
			WallFrame(i, j - 1);
			WallFrame(i, j, resetFrame);
			WallFrame(i, j + 1);
			WallFrame(i + 1, j - 1);
			WallFrame(i + 1, j);
			WallFrame(i + 1, j + 1);
		}

		public static void SectionTileFrame(int startX, int startY)
		{
			// The initial version will skip over an edge case due to a clamped boundary.
#if (!IS_PATCHED && VERSION_INITIAL)
			int num = startX;
			int num2 = startX + 40;
			int num3 = startY;
			int num4 = startY + 30;
			if (num < 6)
			{
				num = 6;
			}
			if (num3 < 6)
			{
				num3 = 6;
			}
			if (num > Main.MaxTilesX - 6)
			{
				num = Main.MaxTilesX - 6;
			}
			if (num3 > Main.MaxTilesY - 6)
			{
				num3 = Main.MaxTilesY - 6;
			}
			WorldGen.tileFrameRecursion = false;
			for (int i = num - 1; i < num2 + 1; i++)
			{
				for (int j = num3 - 1; j < num4 + 1; j++)
#else
			int num = startX - 1;
			int num2 = startX + 40;
			int num3 = startY - 1;
			int num4 = startY + 30;
			if (num < 5)
			{
				num = 5;
			}
			if (num3 < 5)
			{
				num3 = 5;
			}
			if (num2 > Main.MaxTilesX - 5)
			{
				num2 = Main.MaxTilesX - 5;
			}
			if (num4 > Main.MaxTilesY - 5)
			{
				num4 = Main.MaxTilesY - 5;
			}
			tileFrameRecursion = false;
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
#endif
				{
					int type = Main.TileSet[i, j].Type;
					if (type == (int)EntityID.TileID.TORCH || !Main.TileFrameImportant[type])
					{
						TileFrame(i, j, -1);
					}
					WallFrame(i, j, resetFrame: true);
				}
			}
			tileFrameRecursion = true;
		}

		public static void RangeFrame(int startX, int startY, int endX, int endY)
		{
			if (Gen)
			{
				return;
			}
			bool flag = tileFrameRecursion;
			tileFrameRecursion = false;
			for (int i = startX - 1; i < endX + 2; i++)
			{
				for (int j = startY - 1; j < endY + 2; j++)
				{
					TileFrame(i, j);
					WallFrame(i, j);
				}
			}
			tileFrameRecursion = flag;
		}

		public unsafe static void WaterCheck()
		{
			Liquid.NumLiquids = 0;
			LiquidBuffer.NumLiquidBuffer = 0;
			fixed (Tile* ptr = Main.TileSet)
			{
				for (int num = Main.MaxTilesX - 2; num > 0; num--)
				{
					Tile* ptr2 = ptr + (num * (Main.LargeWorldH) + Main.MaxTilesY - 2);
					int num2 = Main.MaxTilesY - 2;
					while (num2 > 0)
					{
						ptr2->CheckingLiquid = 0;
						if (ptr2->Liquid > 0)
						{
							if (ptr2->IsActive != 0 && Main.TileSolidNotSolidTop[ptr2->Type])
							{
								ptr2->Liquid = 0;
							}
							else
							{
								if (ptr2->IsActive != 0)
								{
									if (Main.TileWaterDeath[ptr2->Type] && (ptr2->Type != (byte)EntityID.TileID.TORCH || ptr2->FrameY != 176))
									{
										KillTile(num, num2);
									}
									if (ptr2->Lava != 0 && Main.TileLavaDeath[ptr2->Type])
									{
										KillTile(num, num2);
									}
								}
								Tile* ptr3 = ptr2 + 1;
								if ((ptr3->IsActive == 0 || !Main.TileSolidNotSolidTop[ptr3->Type]) && ptr3->Liquid < byte.MaxValue)
								{
									if (ptr3->Liquid > 250)
									{
										ptr3->Liquid = byte.MaxValue;
									}
									else
									{
										Liquid.AddWater(num, num2);
									}
								}
								ptr3 = ptr2 - (Main.LargeWorldH);
								if ((ptr3->IsActive == 0 || !Main.TileSolidNotSolidTop[ptr3->Type]) && ptr3->Liquid != ptr2->Liquid)
								{
									Liquid.AddWater(num, num2);
								}
								else
								{
									ptr3 = ptr2 + (Main.LargeWorldH);
									if ((ptr3->IsActive == 0 || !Main.TileSolidNotSolidTop[ptr3->Type]) && ptr3->Liquid != ptr2->Liquid)
									{
										Liquid.AddWater(num, num2);
									}
								}
								if (ptr2->Lava != 0)
								{
									ptr3 = ptr2 - 1;
									if (ptr3->Liquid > 0 && ptr3->Lava == 0)
									{
										Liquid.AddWater(num, num2);
									}
									else
									{
										ptr3 = ptr2 + 1;
										if (ptr3->Liquid > 0 && ptr3->Lava == 0)
										{
											Liquid.AddWater(num, num2);
										}
										else
										{
											ptr3 = ptr2 - (Main.LargeWorldH);
											if (ptr3->Liquid > 0 && ptr3->Lava == 0)
											{
												Liquid.AddWater(num, num2);
											}
											else
											{
												ptr3 = ptr2 + (Main.LargeWorldH);
												if (ptr3->Liquid > 0 && ptr3->Lava == 0)
												{
													Liquid.AddWater(num, num2);
												}
											}
										}
									}
								}
							}
						}
						num2--;
						ptr2--;
					}
				}
			}
		}

		public unsafe static void everyTileFrame()
		{
			UI.MainUI.NextProgressStep(Lang.WorldGenText[55]);
			Gen = true;
			fixed (Tile* ptr = Main.TileSet)
			{
				for (int i = 0; i < Main.MaxTilesX; i++)
				{
					if ((i & 0x3F) == 0)
					{
						UI.MainUI.Progress = i / (float)Main.MaxTilesX;
					}
					Tile* ptr2 = ptr + ((Main.LargeWorldH) * i + Main.MaxTilesY);
					for (int num = Main.MaxTilesY - 1; num >= 0; num--)
					{
						ptr2--;
						if (ptr2->IsActive != 0)
						{
							TileFrameNoLiquid(i, num, -1);
						}
						if (ptr2->WallType > 0)
						{
							WallFrame(i, num, resetFrame: true);
						}
					}
				}
			}
			Gen = false;
		}

		private static void PlantCheck(int i, int j)
		{
			EntityID.TileID num = EntityID.TileID.NONE;
			EntityID.TileID num2 = (EntityID.TileID)Main.TileSet[i, j].Type;
			if (j + 1 >= Main.MaxTilesY)
			{
				num = num2;
			}
			if (j + 1 < Main.MaxTilesY && Main.TileSet[i, j + 1].IsActive != 0)
			{
				num = (EntityID.TileID)Main.TileSet[i, j + 1].Type;
			}
			if ((num2 != EntityID.TileID.SHORT_GRASS_PLANTS || num == EntityID.TileID.GRASS || num == EntityID.TileID.CLAY_POT) && 
				(num2 != EntityID.TileID.SHORT_CORRUPT_PLANTS || num == EntityID.TileID.CORRUPT_GRASS) && 
				(num2 != EntityID.TileID.SHORT_JUNGLE_PLANTS || num == EntityID.TileID.JUNGLE_GRASS) && 
				(num2 != EntityID.TileID.GLOWING_MUSHROOM || num == EntityID.TileID.MUSHROOM_GRASS) && 
				(num2 != EntityID.TileID.TALL_GRASS_PLANTS || num == EntityID.TileID.GRASS || num == EntityID.TileID.CLAY_POT) && 
				(num2 != EntityID.TileID.TALL_JUNGLE_PLANTS || num == EntityID.TileID.JUNGLE_GRASS) && 
				(num2 != EntityID.TileID.SHORT_HALLOWED_PLANTS || num == EntityID.TileID.HALLOWED_GRASS) && 
				(num2 != EntityID.TileID.TALL_HALLOWED_PLANTS || num == EntityID.TileID.HALLOWED_GRASS))
			{
				return;
			}
			switch (num)
			{
				case EntityID.TileID.CORRUPT_GRASS:
					num2 = EntityID.TileID.SHORT_CORRUPT_PLANTS;
					if (Main.TileSet[i, j].FrameX >= 162)
					{
						Main.TileSet[i, j].FrameX = 126;
					}
					break;
				case EntityID.TileID.GRASS:
					num2 = ((num2 != EntityID.TileID.TALL_HALLOWED_PLANTS) ? EntityID.TileID.SHORT_GRASS_PLANTS : EntityID.TileID.TALL_GRASS_PLANTS);
					break;
				case EntityID.TileID.HALLOWED_GRASS:
					num2 = ((num2 != EntityID.TileID.TALL_GRASS_PLANTS) ? EntityID.TileID.SHORT_HALLOWED_PLANTS : EntityID.TileID.TALL_HALLOWED_PLANTS);
					break;
			}
			if (num2 != (EntityID.TileID)Main.TileSet[i, j].Type)
			{
				Main.TileSet[i, j].Type = (byte)num2;
			}
			else
			{
				KillTile(i, j);
			}
		}

		public unsafe static void WallFrame(int i, int j, bool resetFrame = false)
		{
			if (i < 0 || j < 0 || i >= Main.MaxTilesX || j >= Main.MaxTilesY)
			{
				return;
			}
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				int wall = ptr->WallType;
				if (wall == (int)EntityID.WallID.NONE)
				{
					return;
				}
				int num = wall;
				int num2 = wall;
				int num3 = wall;
				int num4 = wall;
				int num5 = wall;
				int num6 = wall;
				int num7 = wall;
				int num8 = wall;
				int num9 = -1;
				int num10 = -1;

				if (j - 1 >= 0)
				{
					num2 = ptr[-1].WallType;
				}
				if (j + 1 < Main.MaxTilesY)
				{
					num7 = ptr[1].WallType;
				}
				if (i - 1 >= 0)
				{
					num4 = ptr[-Main.LargeWorldH].WallType;
					if (j - 1 >= 0)
					{
						num = ptr[-Main.LargeWorldH - 1].WallType;
					}
					if (j + 1 < Main.MaxTilesY)
					{
						num6 = ptr[-Main.LargeWorldH + 1].WallType;
					}
				}
				if (i + 1 < Main.MaxTilesX)
				{
					num5 = ptr[Main.LargeWorldH].WallType;
					if (j - 1 >= 0)
					{
						num3 = ptr[Main.LargeWorldH - 1].WallType;
					}
					if (j + 1 < Main.MaxTilesY)
					{
						num8 = ptr[Main.LargeWorldH + 1].WallType;
					}
				}
				if (wall == (int)EntityID.WallID.DIRT_UNSAFE && j >= Main.WorldSurface)
				{
					num7 = wall;
					num6 = wall;
					num8 = wall;
					if (j > Main.WorldSurface)
					{
						num2 = wall;
						num = wall;
						num3 = wall;
						num4 = wall;
						num5 = wall;
					}
				}
				else
				{
					if (num7 > 0)
					{
						num7 = wall;
					}
					if (num6 > 0)
					{
						num6 = wall;
					}
					if (num8 > 0)
					{
						num8 = wall;
					}
				}
				if (num2 > (int)EntityID.WallID.NONE)
				{
					num2 = wall;
				}
				if (num > (int)EntityID.WallID.NONE)
				{
					num = wall;
				}
				if (num3 > (int)EntityID.WallID.NONE)
				{
					num3 = wall;
				}
				if (num4 > (int)EntityID.WallID.NONE)
				{
					num4 = wall;
				}
				if (num5 > (int)EntityID.WallID.NONE)
				{
					num5 = wall;
				}
				int num11;
				if (resetFrame)
				{
					num11 = genRand.Next(3);
					Main.TileSet[i, j].wallFrameNumber = num11;
				}
				else
				{
					num11 = Main.TileSet[i, j].wallFrameNumber;
				}

				// 'if/case' removal and inlining as a calculation for the frame X & Y done for optimisation.

				if (num9 < 0 || num10 < 0)
				{
					if (num2 == wall && num7 == wall && num4 == wall && num5 == wall)
					{
						if (num != wall && num3 != wall)
						{
							num9 = 108 + num11 * 18;
							num10 = 18;
						}
						else if (num6 != wall && num8 != wall)
						{
							num9 = 108 + num11 * 18;
							num10 = 36;
						}
						else if (num != wall && num6 != wall)
						{
							num9 = 180;
							num10 = num11 * 18;
						}
						else if (num3 != wall && num8 != wall)
						{
							num9 = 198;
							num10 = num11 * 18;
						}
						else
						{
							num9 = 18 + num11 * 18;
							num10 = 18;
						}
					}
					else if (num2 != wall && num7 == wall && num4 == wall && num5 == wall)
					{
						num9 = 18 + num11 * 18;
						num10 = 0;
					}
					else if (num2 == wall && num7 != wall && num4 == wall && num5 == wall)
					{
						num9 = 18 + num11 * 18;
						num10 = 36;
					}
					else if (num2 == wall && num7 == wall && num4 != wall && num5 == wall)
					{
						num9 = 0;
						num10 = num11 * 18;
					}
					else if (num2 == wall && num7 == wall && num4 == wall && num5 != wall)
					{
						num9 = 72;
						num10 = num11 * 18;
					}
					else if (num2 != wall && num7 == wall && num4 != wall && num5 == wall)
					{
						num9 = num11 * 36;
						num10 = 54;
					}
					else if (num2 != wall && num7 == wall && num4 == wall && num5 != wall)
					{
						num9 = 18 + num11 * 36;
						num10 = 54;
					}
					else if (num2 == wall && num7 != wall && num4 != wall && num5 == wall)
					{
						num9 = num11 * 36;
						num10 = 72;
					}
					else if (num2 == wall && num7 != wall && num4 == wall && num5 != wall)
					{
						num9 = 18 + num11 * 36;
						num10 = 72;
					}
					else if (num2 == wall && num7 == wall && num4 != wall && num5 != wall)
					{
						num9 = 90;
						num10 = num11 * 18;
					}
					else if (num2 != wall && num7 != wall && num4 == wall && num5 == wall)
					{
						num9 = 108 + num11 * 18;
						num10 = 72;
					}
					else if (num2 != wall && num7 == wall && num4 != wall && num5 != wall)
					{
						num9 = 108 + num11 * 18;
						num10 = 0;
					}
					else if (num2 == wall && num7 != wall && num4 != wall && num5 != wall)
					{
						num9 = 108 + num11 * 18;
						num10 = 54;
					}
					else if (num2 != wall && num7 != wall && num4 != wall && num5 == wall)
					{
						num9 = 162;
						num10 = num11 * 18;
					}
					else if (num2 != wall && num7 != wall && num4 == wall && num5 != wall)
					{
						num9 = 216;
						num10 = num11 * 18;
					}
					else if (num2 != wall && num7 != wall && num4 != wall && num5 != wall)
					{
						num9 = 162 + num11 * 18;
						num10 = 54;
					}
				}
				if (num9 < 0 || num10 < 0)
				{
					num9 = 18 + num11 * 18;
					num10 = 18;
				}
				ptr->wallFrameX = (ushort)num9;
				ptr->wallFrameY = (byte)num10;
			}
		}
		public unsafe static void TileFrame(int i, int j, int frameNumber = 0)
		{
			if (i <= 5 || j <= 5 || i >= Main.MaxTilesX - 5 || j >= Main.MaxTilesY - 5)
			{
				return;
			}
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (ptr->Liquid > 0 && Main.NetMode != (int)NetModeSetting.CLIENT)
				{
					Liquid.AddWater(i, j);
				}
				if (ptr->IsActive == 0)
				{
					return;
				}
				int num = ptr->Type;
				if (Main.TileStone[num])
				{
					num = (int)EntityID.TileID.STONE;
				}
				int frameX = ptr->FrameX;
				int frameY = ptr->FrameY;
				int num2 = -1;
				int num3 = -1;
				if (Main.TileFrameImportant[num])
				{
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.POT:
							CheckPot(i, j);
							break;
						case EntityID.TileID.TREE:
							CheckTree(i, j);
							break;
						case EntityID.TileID.SHORT_GRASS_PLANTS:
						case EntityID.TileID.SHORT_CORRUPT_PLANTS:
						case EntityID.TileID.SHORT_JUNGLE_PLANTS:
						case EntityID.TileID.GLOWING_MUSHROOM:
						case EntityID.TileID.TALL_GRASS_PLANTS:
						case EntityID.TileID.TALL_JUNGLE_PLANTS:
						case EntityID.TileID.SHORT_HALLOWED_PLANTS:
						case EntityID.TileID.TALL_HALLOWED_PLANTS:
							PlantCheck(i, j);
							break;
						case EntityID.TileID.TORCH:
							{
								int num19 = ((ptr->FrameX >= 66) ? 66 : 0);
								int num20 = -1;
								int num21 = -1;
								int num22 = -1;
								int num23 = -1;
								int num24 = -1;
								int num25 = -1;
								int num26 = -1;
								if (ptr[-1].IsActive != 0)
								{
									_ = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num20 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num21 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num22 = ptr[Main.LargeWorldH].Type;
								}
								if (ptr[-Main.LargeWorldH + 1].IsActive != 0)
								{
									num23 = ptr[-Main.LargeWorldH + 1].Type;
								}
								if (ptr[Main.LargeWorldH + 1].IsActive != 0)
								{
									num24 = ptr[Main.LargeWorldH + 1].Type;
								}
								if (ptr[-Main.LargeWorldH - 1].IsActive != 0)
								{
									num25 = ptr[-Main.LargeWorldH - 1].Type;
								}
								if (ptr[Main.LargeWorldH - 1].IsActive != 0)
								{
									num26 = ptr[Main.LargeWorldH - 1].Type;
								}
								if (num20 >= 0 && Main.TileSolidAndAttach[num20])
								{
									ptr->FrameX = (short)num19;
								}
								else if (num21 >= 0 && (Main.TileSolidAndAttach[num21] || num21 == (int)EntityID.TileID.WOODEN_BEAM || (num21 == (int)EntityID.TileID.TREE && num25 == (int)EntityID.TileID.TREE && num23 == (int)EntityID.TileID.TREE)))
								{
									ptr->FrameX = (short)(22 + num19);
								}
								else if (num22 >= 0 && (Main.TileSolidAndAttach[num22] || num22 == (int)EntityID.TileID.WOODEN_BEAM || (num22 == (int)EntityID.TileID.TREE && num26 == (int)EntityID.TileID.TREE && num24 == (int)EntityID.TileID.TREE)))
								{
									ptr->FrameX = (short)(44 + num19);
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.SWITCH:
							{
								int num28 = -1;
								int num29 = -1;
								int num30 = -1;
								if (ptr[-1].IsActive != 0)
								{
									_ = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num28 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num29 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num30 = ptr[Main.LargeWorldH].Type;
								}
								if (num28 >= 0 && Main.TileSolidAndAttach[num28])
								{
									ptr->FrameX = 0;
								}
								else if (num29 >= 0 && (Main.TileSolidAndAttach[num29] || num29 == (int)EntityID.TileID.WOODEN_BEAM || num29 == (int)EntityID.TileID.TREE))
								{
									ptr->FrameX = 18;
								}
								else if (num30 >= 0 && (Main.TileSolidAndAttach[num30] || num30 == (int)EntityID.TileID.WOODEN_BEAM || num30 == (int)EntityID.TileID.TREE))
								{
									ptr->FrameX = 36;
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.CRYSTAL_SHARD:
						case EntityID.TileID.XMAS_LIGHT:
							{
								int num15 = -1;
								int num16 = -1;
								int num17 = -1;
								int num18 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num16 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num15 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num17 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num18 = ptr[Main.LargeWorldH].Type;
								}
								if (num15 >= 0 && Main.TileSolidNotSolidTop[num15])
								{
									ptr->FrameY = 0;
								}
								else if (num17 >= 0 && Main.TileSolidNotSolidTop[num17])
								{
									ptr->FrameY = 54;
								}
								else if (num18 >= 0 && Main.TileSolidNotSolidTop[num18])
								{
									ptr->FrameY = 36;
								}
								else if (num16 >= 0 && Main.TileSolidNotSolidTop[num16])
								{
									ptr->FrameY = 18;
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.LIFE_CRYSTAL:
						case EntityID.TileID.SHADOW_ORB:
							CheckOrb(i, j, num);
							break;
						case EntityID.TileID.DOOR_CLOSED:
							{
								if (ToDestroyObject)
								{
									break;
								}
								int num27 = j - frameY / 18;
								bool flag2 = false;
								if (Main.TileSet[i, num27 - 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, num27 - 1].Type])
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 3].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, num27 + 3].Type])
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27].IsActive == 0 || Main.TileSet[i, num27].Type != num)
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 1].IsActive == 0 || Main.TileSet[i, num27 + 1].Type != num)
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 2].IsActive == 0 || Main.TileSet[i, num27 + 2].Type != num)
								{
									flag2 = true;
								}
								if (flag2)
								{
									ToDestroyObject = true;
									KillTile(i, num27);
									KillTile(i, num27 + 1);
									KillTile(i, num27 + 2);
									if (!Gen)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
									}
								}
								ToDestroyObject = false;
								break;
							}
						case EntityID.TileID.DOOR_OPEN:
							{
								if (ToDestroyObject)
								{
									break;
								}
								int num10 = 0;
								int num11 = 0;
								int num12 = i;
								int num13 = j;
								bool flag = false;
								switch (frameX)
								{
									case 0:
										num10 = 1;
										break;
									case 18:
										num12 = i - 1;
										num11 = -Main.LargeWorldH;
										num10 = 1;
										break;
									case 36:
										num11 = Main.LargeWorldH;
										num12 = i + 1;
										num10 = -1;
										break;
									case 54:
										num10 = -1;
										break;
								}
								switch (frameY)
								{
									case 18:
										num11--;
										num13 = j - 1;
										break;
									case 36:
										num13 = j - 2;
										num11 -= 2;
										break;
								}
								if (ptr[num11 - 1].IsActive == 0 || !Main.TileSolid[ptr[num11 - 1].Type] || ptr[num11 + 3].IsActive == 0 || !Main.TileSolid[ptr[num11 + 3].Type])
								{
									flag = true;
									ToDestroyObject = true;
									if (!Gen)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
									}
								}
								int num14 = num12;
								if (num10 == -1)
								{
									num14 = num12 - 1;
								}
								for (int k = num14; k < num14 + 2; k++)
								{
									for (int l = num13; l < num13 + 3; l++)
									{
										if (!flag)
										{
											fixed (Tile* ptr2 = &Main.TileSet[k, l])
											{
												if (ptr2->Type != (int)EntityID.TileID.DOOR_OPEN || ptr2->IsActive == 0)
												{
													ToDestroyObject = true;
													flag = true;
													k = num14;
													l = num13;
													if (!Gen)
													{
														Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
													}
												}
											}
										}
										if (flag)
										{
											KillTile(k, l);
										}
									}
								}
								ToDestroyObject = false;
								break;
							}
						case EntityID.TileID.CHANDELIER:
						case EntityID.TileID.JACK_O_LANTERN:
						case EntityID.TileID.PRESENT:
						case EntityID.TileID.SAWMILL:
							Check3x3(i, j, num);
							break;
						case EntityID.TileID.CHAIR:
						case EntityID.TileID.SAPLING:
							Check1x2(i, j, num);
							break;
						case EntityID.TileID.TABLE:
						case EntityID.TileID.FURNACE:
						case EntityID.TileID.DEMON_ALTAR:
						case EntityID.TileID.HELLFORGE:
						case EntityID.TileID.LOOM:
						case EntityID.TileID.PIANO:
						case EntityID.TileID.DRESSER:
						case EntityID.TileID.BENCH:
						case EntityID.TileID.TINKERERS_WORKSHOP:
						case EntityID.TileID.ADAMANTITE_FORGE:
#if VERSION_101
						case EntityID.TileID.CAMPFIRE:
#endif
							Check3x2(i, j, num);
							break;
						case EntityID.TileID.PRESSURE_PLATE:
						case EntityID.TileID.EXPLOSIVES:
						case EntityID.TileID.TIMER:
							Check1x1(i, j, num);
							break;
						case EntityID.TileID.ANVIL:
						case EntityID.TileID.WORK_BENCH:
						case EntityID.TileID.PIGGYBANK:
						case EntityID.TileID.BOWL:
						case EntityID.TileID.MYTHRIL_ANVIL:
							Check2x1(i, j, num);
							break;
						case EntityID.TileID.BOTTLE:
						case EntityID.TileID.CANDLE:
						case EntityID.TileID.BOOK:
							CheckOnTable1x1(i, j);
							break;
						case EntityID.TileID.CLAY_POT:
							CheckOnTableClaypot(i, j);
							break;
						case EntityID.TileID.CHEST:
							CheckChest(i, j);
							break;
						case EntityID.TileID.SUNFLOWER:
							CheckSunflower(i, j);
							break;
						case EntityID.TileID.MANNEQUIN:
							CheckMan(i, j);
							break;
						case EntityID.TileID.KEG:
						case EntityID.TileID.CHINESE_LANTERN:
						case EntityID.TileID.COOKING_POT:
						case EntityID.TileID.SAFE:
						case EntityID.TileID.SKULL_LANTERN:
						case EntityID.TileID.TRASH_CAN:
						case EntityID.TileID.CANDELABRA:
						case EntityID.TileID.CRYSTAL_BALL:
						case EntityID.TileID.DISCO_BALL:
						case EntityID.TileID.LEVER:
						case EntityID.TileID.BOULDER:
						case EntityID.TileID.PUMP_IN:
						case EntityID.TileID.PUMP_OUT:
							Check2x2(i, j, num);
							break;
						case EntityID.TileID.BANNER:
							CheckBanner(i, j);
							break;
						case EntityID.TileID.MUSIC_BOX:
							CheckMusicBox(i, j);
							break;
						case EntityID.TileID.LAMP_POST:
						case EntityID.TileID.TIKI_TORCH:
							Check1xX(i, j, num);
							break;
						case EntityID.TileID.BOOKCASE:
						case EntityID.TileID.THRONE:
							Check3x4(i, j, num);
							break;
						case EntityID.TileID.GRANDFATHERS_CLOCK:
						case EntityID.TileID.STATUE:
							Check2xX(i, j, num);
							break;
						case EntityID.TileID.CHAIN_LANTERN:
							Check1x2Top(i, j);
							break;
						case EntityID.TileID.DAYBLOOM_GROWING:
						case EntityID.TileID.DAYBLOOM_MATURE:
						case EntityID.TileID.DAYBLOOM_BLOOMING:
							CheckAlch(i, j);
							break;
						case EntityID.TileID.SIGN:
						case EntityID.TileID.TOMBSTONE:
							CheckSign(i, j, num);
							break;
						case EntityID.TileID.BED:
						case EntityID.TileID.BATHTUB:
							Check4x2(i, j, num);
							break;
						case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
							{
								int num8 = -1;
								int num9 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num9 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num8 = ptr[1].Type;
								}
								if (num8 != num && num8 != (int)EntityID.TileID.MUSHROOM_GRASS)
								{
									KillTile(i, j);
								}
								else if (num9 != num && ptr->FrameX == 0)
								{
									ptr->frameNumber = (byte)genRand.Next(3);
									ptr->FrameX = 18;
									ptr->FrameY = (short)(18 * ptr->frameNumber);
								}
								break;
							}
						case EntityID.TileID.CORAL:
							{
								int num4 = -1;
								int num5 = -1;
								int num6 = -1;
								int num7 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num5 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num4 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num6 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num7 = ptr[Main.LargeWorldH].Type;
								}
								if (num6 != -1 || num5 != -1 || num7 != -1)
								{
									KillTile(i, j);
								}
								else if (num4 < 0 || !Main.TileSolid[num4])
								{
									KillTile(i, j);
								}
								break;
							}
					}
					return;
				}
				int num31 = (int)EntityID.TileID.NONE;
				int num32 = (int)EntityID.TileID.NONE;
				int num33 = (int)EntityID.TileID.NONE;
				int num34 = (int)EntityID.TileID.NONE;
				int num35 = (int)EntityID.TileID.NONE;
				int num36 = (int)EntityID.TileID.NONE;
				int num37 = (int)EntityID.TileID.NONE;
				int num38 = (int)EntityID.TileID.NONE;
				if (ptr[-Main.LargeWorldH].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH].Type;
					num34 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH].Type;
					num35 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-1].IsActive != 0)
				{
					int type = ptr[-1].Type;
					num32 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[1].IsActive != 0)
				{
					int type = ptr[1].Type;
					num37 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-Main.LargeWorldH - 1].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH - 1].Type;
					num31 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH - 1].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH - 1].Type;
					num33 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-Main.LargeWorldH + 1].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH + 1].Type;
					num36 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH + 1].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH + 1].Type;
					num38 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				switch ((EntityID.TileID)num)
				{
					case EntityID.TileID.PLATFORM:
						if (num35 >= 0 && !Main.TileSolid[num35])
						{
							num35 = -1;
						}
						if (num34 >= 0 && !Main.TileSolid[num34])
						{
							num34 = -1;
						}
						num2 = ((num34 == num) ? ((num35 != num) ? ((num35 >= 0) ? 72 : 18) : 0) : ((num34 < 0) ? ((num35 == num) ? 36 : ((num35 <= 0) ? 90 : 126)) : ((num35 == num) ? 54 : ((num35 >= 0) ? 90 : 108))));
						num3 = 18 * ptr->frameNumber;
						break;
					case EntityID.TileID.CACTUS:
						CactusFrame(i, j);
						return;
					case EntityID.TileID.WATER_CANDLE:
						CheckOnTable1x1(i, j);
						return;
				}
				mergeUp = false;
				mergeDown = false;
				mergeLeft = false;
				mergeRight = false;
				if (frameNumber < 0)
				{
					frameNumber = genRand.Next(3);
					ptr->frameNumber = (byte)frameNumber;
				}
				else
				{
					frameNumber = ptr->frameNumber;
				}
				if (num == (int)EntityID.TileID.DIRT)
				{
					if (num32 >= 0 && Main.TileMergeDirt[num32])
					{
						TileFrame(i, j - 1);
						if (mergeDown)
						{
							num32 = num;
						}
					}
					if (num37 >= 0 && Main.TileMergeDirt[num37])
					{
						TileFrame(i, j + 1);
						if (mergeUp)
						{
							num37 = num;
						}
					}
					if (num34 >= 0 && Main.TileMergeDirt[num34])
					{
						TileFrame(i - 1, j);
						if (mergeRight)
						{
							num34 = num;
						}
					}
					if (num35 >= 0 && Main.TileMergeDirt[num35])
					{
						TileFrame(i + 1, j);
						if (mergeLeft)
						{
							num35 = num;
						}
					}
					if (num32 == (int)EntityID.TileID.GRASS || num32 == (int)EntityID.TileID.CORRUPT_GRASS || num32 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num32 = num;
					}
					if (num37 == (int)EntityID.TileID.GRASS || num37 == (int)EntityID.TileID.CORRUPT_GRASS || num37 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num37 = num;
					}
					if (num34 == (int)EntityID.TileID.GRASS || num34 == (int)EntityID.TileID.CORRUPT_GRASS || num34 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num34 = num;
					}
					if (num35 == (int)EntityID.TileID.GRASS || num35 == (int)EntityID.TileID.CORRUPT_GRASS || num35 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num35 = num;
					}
					if (num31 >= (int)EntityID.TileID.DIRT && Main.TileMergeDirt[num31])
					{
						num31 = num;
					}
					else if (num31 == (int)EntityID.TileID.GRASS || num31 == (int)EntityID.TileID.CORRUPT_GRASS || num31 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num31 = num;
					}
					if (num33 >= 0 && Main.TileMergeDirt[num33])
					{
						num33 = num;
					}
					else if (num33 == (int)EntityID.TileID.GRASS || num33 == (int)EntityID.TileID.CORRUPT_GRASS || num33 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num33 = num;
					}
					if (num36 >= 0 && Main.TileMergeDirt[num36])
					{
						num36 = num;
					}
					else if (num36 == (int)EntityID.TileID.GRASS || num36 == (int)EntityID.TileID.CORRUPT_GRASS || num33 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num36 = num;
					}
					if (num38 >= (int)EntityID.TileID.DIRT && Main.TileMergeDirt[num38])
					{
						num38 = num;
					}
					else if (num38 == (int)EntityID.TileID.GRASS || num38 == (int)EntityID.TileID.CORRUPT_GRASS || num38 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num38 = num;
					}
					if (j < Main.RockLayer)
					{
						if (num32 == (int)EntityID.TileID.MUD)
						{
							num32 = (int)EntityID.TileID.LIQUID;
						}
						if (num37 == (int)EntityID.TileID.MUD)
						{
							num37 = (int)EntityID.TileID.LIQUID;
						}
						if (num34 == (int)EntityID.TileID.MUD)
						{
							num34 = (int)EntityID.TileID.LIQUID;
						}
						if (num35 == (int)EntityID.TileID.MUD)
						{
							num35 = (int)EntityID.TileID.LIQUID;
						}
						if (num31 == (int)EntityID.TileID.MUD)
						{
							num31 = (int)EntityID.TileID.LIQUID;
						}
						if (num33 == (int)EntityID.TileID.MUD)
						{
							num33 = (int)EntityID.TileID.LIQUID;
						}
						if (num36 == (int)EntityID.TileID.MUD)
						{
							num36 = (int)EntityID.TileID.LIQUID;
						}
						if (num38 == (int)EntityID.TileID.MUD)
						{
							num38 = (int)EntityID.TileID.LIQUID;
						}
					}
				}
				else if (Main.TileMergeDirt[num])
				{
					if (num32 == (int)EntityID.TileID.DIRT)
					{
						num32 = (int)EntityID.TileID.LIQUID;
					}
					if (num37 == (int)EntityID.TileID.DIRT)
					{
						num37 = (int)EntityID.TileID.LIQUID;
					}
					if (num34 == (int)EntityID.TileID.DIRT)
					{
						num34 = (int)EntityID.TileID.LIQUID;
					}
					if (num35 == (int)EntityID.TileID.DIRT)
					{
						num35 = (int)EntityID.TileID.LIQUID;
					}
					if (num31 == (int)EntityID.TileID.DIRT)
					{
						num31 = (int)EntityID.TileID.LIQUID;
					}
					if (num33 == (int)EntityID.TileID.DIRT)
					{
						num33 = (int)EntityID.TileID.LIQUID;
					}
					if (num36 == (int)EntityID.TileID.DIRT)
					{
						num36 = (int)EntityID.TileID.LIQUID;
					}
					if (num38 == (int)EntityID.TileID.DIRT)
					{
						num38 = (int)EntityID.TileID.LIQUID;
					}
					if (num == (int)EntityID.TileID.STONE)
					{
						if (j > Main.RockLayer)
						{
							if (num32 == (int)EntityID.TileID.MUD)
							{
								TileFrame(i, j - 1);
								if (mergeDown)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.MUD)
							{
								TileFrame(i, j + 1);
								if (mergeUp)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.MUD)
							{
								TileFrame(i - 1, j);
								if (mergeRight)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.MUD)
							{
								TileFrame(i + 1, j);
								if (mergeLeft)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.MUD)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.MUD)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.MUD)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.MUD)
							{
								num38 = num;
							}
						}
						if (num32 == (int)EntityID.TileID.ASH)
						{
							TileFrame(i, j - 1);
							if (mergeDown)
							{
								num32 = num;
							}
						}
						if (num37 == (int)EntityID.TileID.ASH)
						{
							TileFrame(i, j + 1);
							if (mergeUp)
							{
								num37 = num;
							}
						}
						if (num34 == (int)EntityID.TileID.ASH)
						{
							TileFrame(i - 1, j);
							if (mergeRight)
							{
								num34 = num;
							}
						}
						if (num35 == (int)EntityID.TileID.ASH)
						{
							TileFrame(i + 1, j);
							if (mergeLeft)
							{
								num35 = num;
							}
						}
						if (num31 == (int)EntityID.TileID.ASH)
						{
							num31 = num;
						}
						if (num33 == (int)EntityID.TileID.ASH)
						{
							num33 = num;
						}
						if (num36 == (int)EntityID.TileID.ASH)
						{
							num36 = num;
						}
						if (num38 == (int)EntityID.TileID.ASH)
						{
							num38 = num;
						}
					}
				}
				else
				{
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.HELLSTONE:
						case EntityID.TileID.OBSIDIAN_BRICK:
						case EntityID.TileID.HELLSTONE_BRICK:
							if (num32 == (int)EntityID.TileID.ASH)
							{
								num32 = (int)EntityID.TileID.LIQUID;
							}
							if (num37 == (int)EntityID.TileID.ASH)
							{
								num37 = (int)EntityID.TileID.LIQUID;
							}
							if (num34 == (int)EntityID.TileID.ASH)
							{
								num34 = (int)EntityID.TileID.LIQUID;
							}
							if (num35 == (int)EntityID.TileID.ASH)
							{
								num35 = (int)EntityID.TileID.LIQUID;
							}
							if (num31 == (int)EntityID.TileID.ASH)
							{
								num31 = (int)EntityID.TileID.LIQUID;
							}
							if (num33 == (int)EntityID.TileID.ASH)
							{
								num33 = (int)EntityID.TileID.LIQUID;
							}
							if (num36 == (int)EntityID.TileID.ASH)
							{
								num36 = (int)EntityID.TileID.LIQUID;
							}
							if (num38 == (int)EntityID.TileID.ASH)
							{
								num38 = (int)EntityID.TileID.LIQUID;
							}
							break;
						case EntityID.TileID.MUD:
							if (j > Main.RockLayer)
							{
								if (num32 == (int)EntityID.TileID.STONE)
								{
									num32 = (int)EntityID.TileID.LIQUID;
								}
								if (num37 == (int)EntityID.TileID.STONE)
								{
									num37 = (int)EntityID.TileID.LIQUID;
								}
								if (num34 == (int)EntityID.TileID.STONE)
								{
									num34 = (int)EntityID.TileID.LIQUID;
								}
								if (num35 == (int)EntityID.TileID.STONE)
								{
									num35 = (int)EntityID.TileID.LIQUID;
								}
								if (num31 == (int)EntityID.TileID.STONE)
								{
									num31 = (int)EntityID.TileID.LIQUID;
								}
								if (num33 == (int)EntityID.TileID.STONE)
								{
									num33 = (int)EntityID.TileID.LIQUID;
								}
								if (num36 == (int)EntityID.TileID.STONE)
								{
									num36 = (int)EntityID.TileID.LIQUID;
								}
								if (num38 == (int)EntityID.TileID.STONE)
								{
									num38 = (int)EntityID.TileID.LIQUID;
								}
							}
							if (num32 == (int)EntityID.TileID.JUNGLE_GRASS || num32 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num32 = num;
							}
							if (num37 == (int)EntityID.TileID.JUNGLE_GRASS || num37 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num37 = num;
							}
							if (num34 == (int)EntityID.TileID.JUNGLE_GRASS || num34 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num34 = num;
							}
							if (num35 == (int)EntityID.TileID.JUNGLE_GRASS || num35 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num35 = num;
							}
							if (num31 == (int)EntityID.TileID.JUNGLE_GRASS || num31 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.JUNGLE_GRASS || num33 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.JUNGLE_GRASS || num36 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.JUNGLE_GRASS || num38 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num38 = num;
							}
							if (j >= Main.RockLayer)
							{
								break;
							}
							if (num32 == (int)EntityID.TileID.DIRT)
							{
								TileFrame(i, j - 1);
								if (mergeDown)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.DIRT)
							{
								TileFrame(i, j + 1);
								if (mergeUp)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.DIRT)
							{
								TileFrame(i - 1, j);
								if (mergeRight)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.DIRT)
							{
								TileFrame(i + 1, j);
								if (mergeLeft)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.DIRT)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.DIRT)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.DIRT)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.DIRT)
							{
								num38 = num;
							}
							break;
						case EntityID.TileID.ASH:
							if (num32 == (int)EntityID.TileID.STONE)
							{
								num32 = (int)EntityID.TileID.LIQUID;
							}
							if (num37 == (int)EntityID.TileID.STONE)
							{
								num37 = (int)EntityID.TileID.LIQUID;
							}
							if (num34 == (int)EntityID.TileID.STONE)
							{
								num34 = (int)EntityID.TileID.LIQUID;
							}
							if (num35 == (int)EntityID.TileID.STONE)
							{
								num35 = (int)EntityID.TileID.LIQUID;
							}
							if (num31 == (int)EntityID.TileID.STONE)
							{
								num31 = (int)EntityID.TileID.LIQUID;
							}
							if (num33 == (int)EntityID.TileID.STONE)
							{
								num33 = (int)EntityID.TileID.LIQUID;
							}
							if (num36 == (int)EntityID.TileID.STONE)
							{
								num36 = (int)EntityID.TileID.LIQUID;
							}
							if (num38 == (int)EntityID.TileID.STONE)
							{
								num38 = (int)EntityID.TileID.LIQUID;
							}
							if (num32 == (int)EntityID.TileID.HELLSTONE || num32 == (int)EntityID.TileID.HELLSTONE_BRICK || num32 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrame(i, j - 1);
								if (mergeDown)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.HELLSTONE || num37 == (int)EntityID.TileID.HELLSTONE_BRICK || num37 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrame(i, j + 1);
								if (mergeUp)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.HELLSTONE || num34 == (int)EntityID.TileID.HELLSTONE_BRICK || num34 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrame(i - 1, j);
								if (mergeRight)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.HELLSTONE || num35 == (int)EntityID.TileID.HELLSTONE_BRICK || num35 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrame(i + 1, j);
								if (mergeLeft)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.HELLSTONE || num31 == (int)EntityID.TileID.HELLSTONE_BRICK || num31 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.HELLSTONE || num33 == (int)EntityID.TileID.HELLSTONE_BRICK || num33 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.HELLSTONE || num36 == (int)EntityID.TileID.HELLSTONE_BRICK || num36 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.HELLSTONE || num38 == (int)EntityID.TileID.HELLSTONE_BRICK || num38 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num38 = num;
							}
							break;
						case EntityID.TileID.CORRUPTION_THORN:
							if (num37 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num37 = num;
							}
							break;
						case EntityID.TileID.JUNGLE_THORN:
							if (num37 == (int)EntityID.TileID.JUNGLE_GRASS)
							{
								num37 = num;
							}
							break;
						case EntityID.TileID.COBWEB:
							if (num32 >= 0 && !Main.TileNoAttach[num32])
							{
								num32 = num;
							}
							if (num37 >= 0 && !Main.TileNoAttach[num37])
							{
								num37 = num;
							}
							if (num34 >= 0 && !Main.TileNoAttach[num34])
							{
								num34 = num;
							}
							if (num35 >= 0 && !Main.TileNoAttach[num35])
							{
								num35 = num;
							}
							if (num31 >= 0 && !Main.TileNoAttach[num31])
							{
								num31 = num;
							}
							if (num33 >= 0 && !Main.TileNoAttach[num33])
							{
								num33 = num;
							}
							if (num36 >= 0 && !Main.TileNoAttach[num36])
							{
								num36 = num;
							}
							if (num38 >= 0 && !Main.TileNoAttach[num38])
							{
								num38 = num;
							}
							break;
					}
				}
				bool flag3 = false;
				if (num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.CORRUPT_GRASS || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS)
				{
					flag3 = true;
					if (num32 >= 0 && num32 != num && !Main.TileSolid[num32])
					{
						num32 = (int)EntityID.TileID.NONE;
					}
					if (num37 >= 0 && num37 != num && !Main.TileSolid[num37])
					{
						num37 = (int)EntityID.TileID.NONE;
					}
					if (num34 >= 0 && num34 != num && !Main.TileSolid[num34])
					{
						num34 = (int)EntityID.TileID.NONE;
					}
					if (num35 >= 0 && num35 != num && !Main.TileSolid[num35])
					{
						num35 = (int)EntityID.TileID.NONE;
					}
					if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
					{
						num31 = (int)EntityID.TileID.NONE;
					}
					if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
					{
						num33 = (int)EntityID.TileID.NONE;
					}
					if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
					{
						num36 = (int)EntityID.TileID.NONE;
					}
					if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
					{
						num38 = (int)EntityID.TileID.NONE;
					}
					int num39 = 0;
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.JUNGLE_GRASS:
						case EntityID.TileID.MUSHROOM_GRASS:
							num39 = (int)EntityID.TileID.MUD;
							break;
						case EntityID.TileID.GRASS:
							if (num32 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num32 = num39;
							}
							if (num37 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num37 = num39;
							}
							if (num34 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num34 = num39;
							}
							if (num35 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num35 = num39;
							}
							if (num31 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num31 = num39;
							}
							if (num33 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num33 = num39;
							}
							if (num36 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num36 = num39;
							}
							if (num38 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num38 = num39;
							}
							break;
						case EntityID.TileID.CORRUPT_GRASS:
							if (num32 == (int)EntityID.TileID.GRASS)
							{
								num32 = num39;
							}
							if (num37 == (int)EntityID.TileID.GRASS)
							{
								num37 = num39;
							}
							if (num34 == (int)EntityID.TileID.GRASS)
							{
								num34 = num39;
							}
							if (num35 == (int)EntityID.TileID.GRASS)
							{
								num35 = num39;
							}
							if (num31 == (int)EntityID.TileID.GRASS)
							{
								num31 = num39;
							}
							if (num33 == (int)EntityID.TileID.GRASS)
							{
								num33 = num39;
							}
							if (num36 == (int)EntityID.TileID.GRASS)
							{
								num36 = num39;
							}
							if (num38 == (int)EntityID.TileID.GRASS)
							{
								num38 = num39;
							}
							break;
					}
					if (num32 != num && num32 != num39 && (num37 == num || num37 == num39))
					{
						if (num34 == num39 && num35 == num)
						{
							num2 = 18 * frameNumber;
							num3 = 198;
						}
						else if (num34 == num && num35 == num39)
						{
							num2 = 54 + 18 * frameNumber;
							num3 = 198;
						}
					}
					else if (num37 != num && num37 != num39 && (num32 == num || num32 == num39))
					{
						if (num34 == num39 && num35 == num)
						{
							num2 = 18 * frameNumber;
							num3 = 216;
						}
						else if (num34 == num && num35 == num39)
						{
							num2 = 54 + 18 * frameNumber;
							num3 = 216;
						}
					}
					else if (num34 != num && num34 != num39 && (num35 == num || num35 == num39))
					{
						if (num32 == num39 && num37 == num)
						{
							num2 = 72;
							num3 = 144 + 18 * frameNumber;
						}
						else if (num37 == num && num35 == num32)
						{
							num2 = 72;
							num3 = 90 + 18 * frameNumber;
						}
					}
					else if (num35 != num && num35 != num39 && (num34 == num || num34 == num39))
					{
						if (num32 == num39 && num37 == num)
						{
							num2 = 90;
							num3 = 144 + 18 * frameNumber;
						}
						else if (num37 == num && num35 == num32)
						{
							num2 = 90;
							num3 = 90 + 18 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num)
					{
						if (num31 != num && num33 != num && num36 != num && num38 != num)
						{
							if (num38 == num39)
							{
								num3 = 324;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num33 == num39)
							{
								num3 = 342;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num36 == num39)
							{
								num3 = 360;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num31 == num39)
							{
								num3 = 378;
								num2 = 108 + 18 * frameNumber;
							}
							else
							{
								num3 = 234;
								num2 = 144 + 54 * frameNumber;
							}
						}
						else if (num31 != num && num38 != num)
						{
							num3 = 306;
							num2 = 36 + 18 * frameNumber;
						}
						else if (num33 != num && num36 != num)
						{
							num3 = 306;
							num2 = 90 + 18 * frameNumber;
						}
						else if (num31 != num && num33 == num && num36 == num && num38 == num)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 == num && num33 != num && num36 == num && num38 == num)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 == num && num33 == num && num36 != num && num38 == num)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num31 == num && num33 == num && num36 == num && num38 != num)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num39 && num34 == num && num35 == num && num31 == (int)EntityID.TileID.NONE && num33 == (int)EntityID.TileID.NONE)
					{
						num3 = 18;
						num2 = 108 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num && num34 == num && num35 == num && num36 == (int)EntityID.TileID.NONE && num38 == (int)EntityID.TileID.NONE)
					{
						num3 = 36;
						num2 = 108 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num39 && num35 == num && num33 == (int)EntityID.TileID.NONE && num38 == (int)EntityID.TileID.NONE)
					{
						num2 = 198;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num39 && num31 == (int)EntityID.TileID.NONE && num36 == (int)EntityID.TileID.NONE)
					{
						num2 = 180;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num39 && num34 == num && num35 == num)
					{
						if (num33 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if (num32 == num39 && num37 == num && num34 == num && num35 == num)
					{
						if (num38 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num36 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num39)
					{
						if (num31 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num36 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num39 && num35 == num)
					{
						if (num33 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num38 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if ((num32 == num39 && num37 == num && num34 == num && num35 == num) || (num32 == num && num37 == num39 && num34 == num && num35 == num) || (num32 == num && num37 == num && num34 == num39 && num35 == num) || (num32 == num && num37 == num && num34 == num && num35 == num39))
					{
						num3 = 18;
						num2 = 18 + 18 * frameNumber;
					}
					if ((num32 == num || num32 == num39) && (num37 == num || num37 == num39) && (num34 == num || num34 == num39) && (num35 == num || num35 == num39))
					{
						if (num31 != num && num31 != num39 && (num33 == num || num33 == num39) && (num36 == num || num36 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num33 != num && num33 != num39 && (num31 == num || num31 == num39) && (num36 == num || num36 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num36 != num && num36 != num39 && (num31 == num || num31 == num39) && (num33 == num || num33 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num38 != num && num38 != num39 && (num31 == num || num31 == num39) && (num36 == num || num36 == num39) && (num33 == num || num33 == num39))
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					if (num32 != num39 && num32 != num && num37 == num && num34 != num39 && num34 != num && num35 == num && num38 != num39 && num38 != num)
					{
						num3 = 270;
						num2 = 90 + 18 * frameNumber;
					}
					else if (num32 != num39 && num32 != num && num37 == num && num34 == num && num35 != num39 && num35 != num && num36 != num39 && num36 != num)
					{
						num3 = 270;
						num2 = 144 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && num32 == num && num34 != num39 && num34 != num && num35 == num && num33 != num39 && num33 != num)
					{
						num3 = 288;
						num2 = 90 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && num32 == num && num34 == num && num35 != num39 && num35 != num && num31 != num39 && num31 != num)
					{
						num3 = 288;
						num2 = 144 + 18 * frameNumber;
					}
					else if (num32 != num && num32 != num39 && num37 == num && num34 == num && num35 == num && num36 != num && num36 != num39 && num38 != num && num38 != num39)
					{
						num3 = 216;
						num2 = 144 + 54 * frameNumber;
					}
					else if (num37 != num && num37 != num39 && num32 == num && num34 == num && num35 == num && num31 != num && num31 != num39 && num33 != num && num33 != num39)
					{
						num3 = 252;
						num2 = 144 + 54 * frameNumber;
					}
					else if (num34 != num && num34 != num39 && num37 == num && num32 == num && num35 == num && num33 != num && num33 != num39 && num38 != num && num38 != num39)
					{
						num3 = 234;
						num2 = 126 + 54 * frameNumber;
					}
					else if (num35 != num && num35 != num39 && num37 == num && num32 == num && num34 == num && num31 != num && num31 != num39 && num36 != num && num36 != num39)
					{
						num3 = 234;
						num2 = 162 + 54 * frameNumber;
					}
					else if (num32 != num39 && num32 != num && (num37 == num39 || num37 == num) && num34 == num39 && num35 == num39)
					{
						num3 = 270;
						num2 = 36 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && (num32 == num39 || num32 == num) && num34 == num39 && num35 == num39)
					{
						num3 = 288;
						num2 = 36 + 18 * frameNumber;
					}
					else if (num34 != num39 && num34 != num && (num35 == num39 || num35 == num) && num32 == num39 && num37 == num39)
					{
						num2 = 0;
						num3 = 270 + 18 * frameNumber;
					}
					else if (num35 != num39 && num35 != num && (num34 == num39 || num34 == num) && num32 == num39 && num37 == num39)
					{
						num2 = 18;
						num3 = 270 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num39 && num34 == num39 && num35 == num39)
					{
						num3 = 288;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num && num34 == num39 && num35 == num39)
					{
						num3 = 270;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num39 && num34 == num && num35 == num39)
					{
						num3 = 306;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num39 && num34 == num39 && num35 == num)
					{
						num3 = 306;
						num2 = 144 + 18 * frameNumber;
					}
					if (num32 != num && num32 != num39 && num37 == num && num34 == num && num35 == num)
					{
						if ((num36 == num39 || num36 == num) && num38 != num39 && num38 != num)
						{
							num3 = 324;
							num2 = 18 * frameNumber;
						}
						else if ((num38 == num39 || num38 == num) && num36 != num39 && num36 != num)
						{
							num3 = 324;
							num2 = 54 + 18 * frameNumber;
						}
					}
					else if (num37 != num && num37 != num39 && num32 == num && num34 == num && num35 == num)
					{
						if ((num31 == num39 || num31 == num) && num33 != num39 && num33 != num)
						{
							num3 = 342;
							num2 = 18 * frameNumber;
						}
						else if ((num33 == num39 || num33 == num) && num31 != num39 && num31 != num)
						{
							num3 = 342;
							num2 = 54 + 18 * frameNumber;
						}
					}
					else if (num34 != num && num34 != num39 && num32 == num && num37 == num && num35 == num)
					{
						if ((num33 == num39 || num33 == num) && num38 != num39 && num38 != num)
						{
							num3 = 360;
							num2 = 54 + 18 * frameNumber;
						}
						else if ((num38 == num39 || num38 == num) && num33 != num39 && num33 != num)
						{
							num3 = 360;
							num2 = 18 * frameNumber;
						}
					}
					else if (num35 != num && num35 != num39 && num32 == num && num37 == num && num34 == num)
					{
						if ((num31 == num39 || num31 == num) && num36 != num39 && num36 != num)
						{
							num3 = 378;
							num2 = 18 * frameNumber;
						}
						else if ((num36 == num39 || num36 == num) && num31 != num39 && num31 != num)
						{
							num3 = 378;
							num2 = 54 + 18 * frameNumber;
						}
					}
					if ((num32 == num || num32 == num39) && (num37 == num || num37 == num39) && (num34 == num || num34 == num39) && (num35 == num || num35 == num39) && num31 != (int)EntityID.TileID.NONE && num33 != (int)EntityID.TileID.NONE && num36 != (int)EntityID.TileID.NONE && num38 != (int)EntityID.TileID.NONE)
					{
						num3 = 18;
						num2 = 18 + 18 * frameNumber;
					}
					if (num32 == num39)
					{
						num32 = (int)EntityID.TileID.LIQUID;
					}
					if (num37 == num39)
					{
						num37 = (int)EntityID.TileID.LIQUID;
					}
					if (num34 == num39)
					{
						num34 = (int)EntityID.TileID.LIQUID;
					}
					if (num35 == num39)
					{
						num35 = (int)EntityID.TileID.LIQUID;
					}
					if (num31 == num39)
					{
						num31 = (int)EntityID.TileID.LIQUID;
					}
					if (num33 == num39)
					{
						num33 = (int)EntityID.TileID.LIQUID;
					}
					if (num36 == num39)
					{
						num36 = (int)EntityID.TileID.LIQUID;
					}
					if (num38 == num39)
					{
						num38 = (int)EntityID.TileID.LIQUID;
					}
				}
				if (num2 == (int)EntityID.TileID.NONE && (Main.TileMergeDirt[num] || num == (int)EntityID.TileID.DIRT || num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.ASH || num == (int)EntityID.TileID.HELLSTONE || num == (int)EntityID.TileID.MUD || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS || num == (int)EntityID.TileID.HELLSTONE_BRICK || num == (int)EntityID.TileID.OBSIDIAN_BRICK))
				{
					if (!flag3)
					{
						flag3 = true;
						if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
						{
							num31 = (int)EntityID.TileID.NONE;
						}
						if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
						{
							num33 = (int)EntityID.TileID.NONE;
						}
						if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
						{
							num36 = (int)EntityID.TileID.NONE;
						}
						if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
						{
							num38 = (int)EntityID.TileID.NONE;
						}
					}
					if (num32 >= 0 && num32 != num)
					{
						num32 = (int)EntityID.TileID.NONE;
					}
					if (num37 >= 0 && num37 != num)
					{
						num37 = (int)EntityID.TileID.NONE;
					}
					if (num34 >= 0 && num34 != num)
					{
						num34 = (int)EntityID.TileID.NONE;
					}
					if (num35 >= 0 && num35 != num)
					{
						num35 = (int)EntityID.TileID.NONE;
					}
					if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
					{
						if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == num && num35 == num)
						{
							num3 = 108;
							num2 = 144 + 18 * frameNumber;
							mergeUp = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
						{
							num3 = 90;
							num2 = 144 + 18 * frameNumber;
							mergeDown = true;
						}
						else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 162;
							num3 = 126 + 18 * frameNumber;
							mergeLeft = true;
						}
						else if (num32 == num && num37 == num && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 144;
							num3 = 126 + 18 * frameNumber;
							mergeRight = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
							mergeUp = true;
							mergeLeft = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
							mergeUp = true;
							mergeRight = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
							mergeDown = true;
							mergeLeft = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
							mergeDown = true;
							mergeRight = true;
						}
						else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 180;
							num3 = 126 + 18 * frameNumber;
							mergeLeft = true;
							mergeRight = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
						{
							num3 = 180;
							num2 = 144 + 18 * frameNumber;
							mergeUp = true;
							mergeDown = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 198;
							num3 = 90 + 18 * frameNumber;
							mergeUp = true;
							mergeLeft = true;
							mergeRight = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 198;
							num3 = 144 + 18 * frameNumber;
							mergeDown = true;
							mergeLeft = true;
							mergeRight = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 216;
							num3 = 144 + 18 * frameNumber;
							mergeUp = true;
							mergeDown = true;
							mergeRight = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 216;
							num3 = 90 + 18 * frameNumber;
							mergeUp = true;
							mergeDown = true;
							mergeLeft = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num3 = 198;
							num2 = 108 + 18 * frameNumber;
							mergeUp = true;
							mergeDown = true;
							mergeLeft = true;
							mergeRight = true;
						}
						else if (num32 == num && num37 == num && num34 == num && num35 == num)
						{
							if (num38 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 0;
								num3 = 90 + 36 * frameNumber;
							}
							else if (num36 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 18;
								num3 = 90 + 36 * frameNumber;
							}
							else if (num33 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 0;
								num3 = 108 + 36 * frameNumber;
							}
							else if (num31 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 18;
								num3 = 108 + 36 * frameNumber;
							}
						}
					}
					else
					{
						if (num != (int)EntityID.TileID.GRASS && num != (int)EntityID.TileID.CORRUPT_GRASS && num != (int)EntityID.TileID.JUNGLE_GRASS && num != (int)EntityID.TileID.MUSHROOM_GRASS && num != (int)EntityID.TileID.HALLOWED_GRASS)
						{
							if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
							{
								num3 = 0;
								num2 = 234 + 18 * frameNumber;
								mergeDown = true;
							}
							else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.NONE && num34 == num && num35 == num)
							{
								num3 = 18;
								num2 = 234 + 18 * frameNumber;
								mergeUp = true;
							}
							else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 36;
								num2 = 234 + 18 * frameNumber;
								mergeRight = true;
							}
							else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.NONE)
							{
								num3 = 54;
								num2 = 234 + 18 * frameNumber;
								mergeLeft = true;
							}
						}
						if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == num)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == num)
							{
								num2 = 72;
								num3 = 144 + 18 * frameNumber;
								mergeUp = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID && num32 == num)
							{
								num2 = 72;
								num3 = 90 + 18 * frameNumber;
								mergeDown = true;
							}
						}
						else if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == num && num35 == (int)EntityID.TileID.NONE)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == num)
							{
								num2 = 90;
								num3 = 144 + 18 * frameNumber;
								mergeUp = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID && num32 == num)
							{
								num2 = 90;
								num3 = 90 + 18 * frameNumber;
								mergeDown = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == num && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == num)
							{
								num2 = 18 * frameNumber;
								num3 = 198;
								mergeLeft = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID && num34 == num)
							{
								num2 = 54 + 18 * frameNumber;
								num3 = 198;
								mergeRight = true;
							}
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == num)
							{
								num2 = 18 * frameNumber;
								num3 = 216;
								mergeLeft = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID && num34 == num)
							{
								num2 = 54 + 18 * frameNumber;
								num3 = 216;
								mergeRight = true;
							}
						}
						else if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 108;
								num3 = 216 + 18 * frameNumber;
								mergeUp = true;
								mergeDown = true;
							}
							else if (num32 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 126;
								num3 = 144 + 18 * frameNumber;
								mergeUp = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 126;
								num3 = 90 + 18 * frameNumber;
								mergeDown = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 198;
								num2 = 162 + 18 * frameNumber;
								mergeLeft = true;
								mergeRight = true;
							}
							else if (num34 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 252;
								num2 = 18 * frameNumber;
								mergeLeft = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 252;
								num2 = 54 + 18 * frameNumber;
								mergeRight = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							num2 = 108;
							num3 = 144 + 18 * frameNumber;
							mergeUp = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							num2 = 108;
							num3 = 90 + 18 * frameNumber;
							mergeDown = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.NONE)
						{
							num3 = 234;
							num2 = 18 * frameNumber;
							mergeLeft = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.LIQUID)
						{
							num3 = 234;
							num2 = 54 + 18 * frameNumber;
							mergeRight = true;
						}
					}
				}
				if (num2 < 0)
				{
					if (!flag3)
					{
						flag3 = true;
						if (num32 >= 0 && num32 != num && !Main.TileSolid[num32])
						{
							num32 = (int)EntityID.TileID.NONE;
						}
						if (num37 >= 0 && num37 != num && !Main.TileSolid[num37])
						{
							num37 = (int)EntityID.TileID.NONE;
						}
						if (num34 >= 0 && num34 != num && !Main.TileSolid[num34])
						{
							num34 = (int)EntityID.TileID.NONE;
						}
						if (num35 >= 0 && num35 != num && !Main.TileSolid[num35])
						{
							num35 = (int)EntityID.TileID.NONE;
						}
						if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
						{
							num31 = (int)EntityID.TileID.NONE;
						}
						if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
						{
							num33 = (int)EntityID.TileID.NONE;
						}
						if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
						{
							num36 = (int)EntityID.TileID.NONE;
						}
						if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
						{
							num38 = (int)EntityID.TileID.NONE;
						}
					}
					if (num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.CORRUPT_GRASS || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						if (num32 == (int)EntityID.TileID.LIQUID)
						{
							num32 = num;
						}
						if (num37 == (int)EntityID.TileID.LIQUID)
						{
							num37 = num;
						}
						if (num34 == (int)EntityID.TileID.LIQUID)
						{
							num34 = num;
						}
						if (num35 == (int)EntityID.TileID.LIQUID)
						{
							num35 = num;
						}
						if (num31 == (int)EntityID.TileID.LIQUID)
						{
							num31 = num;
						}
						if (num33 == (int)EntityID.TileID.LIQUID)
						{
							num33 = num;
						}
						if (num36 == (int)EntityID.TileID.LIQUID)
						{
							num36 = num;
						}
						if (num38 == (int)EntityID.TileID.LIQUID)
						{
							num38 = num;
						}
					}
					if (num32 == num && num37 == num && num34 == num && num35 == num)
					{
						if (num31 != num && num33 != num)
						{
							num3 = 18;
							num2 = 108 + 18 * frameNumber;
						}
						else if (num36 != num && num38 != num)
						{
							num3 = 36;
							num2 = 108 + 18 * frameNumber;
						}
						else if (num31 != num && num36 != num)
						{
							num2 = 180;
							num3 = 18 * frameNumber;
						}
						else if (num33 != num && num38 != num)
						{
							num2 = 198;
							num3 = 18 * frameNumber;
						}
						else
						{
							num3 = 18;
							num2 = 18 + 18 * frameNumber;
						}
					}
					else if (num32 != num && num37 == num && num34 == num && num35 == num)
					{
						num3 = 0;
						num2 = 18 + 18 * frameNumber;
					}
					else if (num32 == num && num37 != num && num34 == num && num35 == num)
					{
						num3 = 36;
						num2 = 18 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 != num && num35 == num)
					{
						num2 = 0;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num && num35 != num)
					{
						num2 = 72;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 == num && num34 != num && num35 == num)
					{
						num2 = 36 * frameNumber;
						num3 = 54;
					}
					else if (num32 != num && num37 == num && num34 == num && num35 != num)
					{
						num2 = 18 + 36 * frameNumber;
						num3 = 54;
					}
					else if (num32 == num && num37 != num && num34 != num && num35 == num)
					{
						num2 = 36 * frameNumber;
						num3 = 72;
					}
					else if (num32 == num && num37 != num && num34 == num && num35 != num)
					{
						num2 = 18 + 36 * frameNumber;
						num3 = 72;
					}
					else if (num32 == num && num37 == num && num34 != num && num35 != num)
					{
						num2 = 90;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 == num && num35 == num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 72;
					}
					else if (num32 != num && num37 == num && num34 != num && num35 != num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 0;
					}
					else if (num32 == num && num37 != num && num34 != num && num35 != num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 54;
					}
					else if (num32 != num && num37 != num && num34 != num && num35 == num)
					{
						num2 = 162;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 == num && num35 != num)
					{
						num2 = 216;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 != num && num35 != num)
					{
						num2 = 162 + 18 * frameNumber;
						num3 = 54;
					}
				}
				if (num2 < 0)
				{
					num3 = 18;
					num2 = 18 + 18 * frameNumber;
				}
				ptr->FrameX = (short)num2;
				ptr->FrameY = (short)num3;
				if (num == (int)EntityID.TileID.VINE || num == (int)EntityID.TileID.JUNGLE_VINE || num == (int)EntityID.TileID.HALLOWED_VINE)
				{
					num32 = ((ptr[-1].IsActive != 0) ? ptr[-1].Type : (int)EntityID.TileID.NONE);
					if (num == (int)EntityID.TileID.VINE && (num32 == (int)EntityID.TileID.HALLOWED_GRASS || num32 == (int)EntityID.TileID.HALLOWED_VINE))
					{
						ptr->Type = (int)EntityID.TileID.HALLOWED_VINE;
						SquareTileFrame(i, j);
						return;
					}
					if (num == (int)EntityID.TileID.HALLOWED_VINE && (num32 == (int)EntityID.TileID.GRASS || num32 == (int)EntityID.TileID.VINE))
					{
						ptr->Type = (int)EntityID.TileID.VINE;
						SquareTileFrame(i, j);
						return;
					}
					if (num32 != num && (num32 == (int)EntityID.TileID.NONE || (num == (int)EntityID.TileID.VINE && num32 != (int)EntityID.TileID.GRASS) || (num == (int)EntityID.TileID.JUNGLE_VINE && num32 != (int)EntityID.TileID.JUNGLE_GRASS) || (num == (int)EntityID.TileID.HALLOWED_VINE && num32 != (int)EntityID.TileID.HALLOWED_GRASS)))
					{
						KillTile(i, j);
					}
				}
				if (!Gen && Main.NetMode != (int)NetModeSetting.CLIENT && (num == (int)EntityID.TileID.SAND || num == (int)EntityID.TileID.EBONSAND || num == (int)EntityID.TileID.PEARLSAND || num == (int)EntityID.TileID.SILT))
				{
					Tile* ptr3 = ptr + 1;
					if (ptr3->IsActive== 0)
					{
						ptr3 = ptr - 1;
						if (ptr3->IsActive== 0 || ptr3->Type != (int)EntityID.TileID.CHEST)
						{
							ptr->IsActive= 0;
							sandBuffer[currentSandBuffer].Add(i, j);
							return;
						}
					}
				}
				if (frameX >= 0 && num2 != frameX && frameY >= 0 && num3 != frameY && tileFrameRecursion)
				{
					bool flag4 = mergeUp;
					bool flag5 = mergeDown;
					bool flag6 = mergeLeft;
					bool flag7 = mergeRight;
					TileFrame(i, j - 1);
					TileFrame(i, j + 1);
					TileFrame(i - 1, j);
					TileFrame(i + 1, j);
					mergeUp = flag4;
					mergeDown = flag5;
					mergeLeft = flag6;
					mergeRight = flag7;
				}
			}
		}

		public unsafe static void TileFrameNoLiquid(int i, int j, int frameNumber = 0)
		{
			if (i <= 5 || j <= 5 || i >= Main.MaxTilesX - 5 || j >= Main.MaxTilesY - 5)
			{
				return;
			}
			fixed (Tile* ptr = &Main.TileSet[i, j])
			{
				if (ptr->IsActive == 0)
				{
					return;
				}
				int num = ptr->Type;
				if (Main.TileStone[num])
				{
					num = (int)EntityID.TileID.STONE;
				}
				int frameX = ptr->FrameX;
				int frameY = ptr->FrameY;
				int num2 = -1;
				int num3 = -1;
				if (Main.TileFrameImportant[num])
				{
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.POT:
							break;
						case EntityID.TileID.TREE:
							break;
						case EntityID.TileID.SHORT_GRASS_PLANTS:
						case EntityID.TileID.SHORT_CORRUPT_PLANTS:
						case EntityID.TileID.SHORT_JUNGLE_PLANTS:
						case EntityID.TileID.GLOWING_MUSHROOM:
						case EntityID.TileID.TALL_GRASS_PLANTS:
						case EntityID.TileID.TALL_JUNGLE_PLANTS:
						case EntityID.TileID.SHORT_HALLOWED_PLANTS:
						case EntityID.TileID.TALL_HALLOWED_PLANTS:
							PlantCheck(i, j);
							break;
						case EntityID.TileID.TORCH:
							{
								int num19 = ((ptr->FrameX >= 66) ? 66 : 0);
								int num20 = -1;
								int num21 = -1;
								int num22 = -1;
								int num23 = -1;
								int num24 = -1;
								int num25 = -1;
								int num26 = -1;
								if (ptr[-1].IsActive != 0)
								{
									_ = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num20 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num21 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num22 = ptr[Main.LargeWorldH].Type;
								}
								if (ptr[-Main.LargeWorldH + 1].IsActive != 0)
								{
									num23 = ptr[-Main.LargeWorldH + 1].Type;
								}
								if (ptr[Main.LargeWorldH + 1].IsActive != 0)
								{
									num24 = ptr[Main.LargeWorldH + 1].Type;
								}
								if (ptr[-Main.LargeWorldH - 1].IsActive != 0)
								{
									num25 = ptr[-Main.LargeWorldH - 1].Type;
								}
								if (ptr[Main.LargeWorldH - 1].IsActive != 0)
								{
									num26 = ptr[Main.LargeWorldH - 1].Type;
								}
								if (num20 >= 0 && Main.TileSolidAndAttach[num20])
								{
									ptr->FrameX = (short)num19;
								}
								else if (num21 >= 0 && (Main.TileSolidAndAttach[num21] || num21 == (int)EntityID.TileID.WOODEN_BEAM || (num21 == (int)EntityID.TileID.TREE && num25 == (int)EntityID.TileID.TREE && num23 == (int)EntityID.TileID.TREE)))
								{
									ptr->FrameX = (short)(22 + num19);
								}
								else if (num22 >= 0 && (Main.TileSolidAndAttach[num22] || num22 == (int)EntityID.TileID.WOODEN_BEAM || (num22 == (int)EntityID.TileID.TREE && num26 == (int)EntityID.TileID.TREE && num24 == (int)EntityID.TileID.TREE)))
								{
									ptr->FrameX = (short)(44 + num19);
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.SWITCH:
							{
								int num28 = -1;
								int num29 = -1;
								int num30 = -1;
								if (ptr[-1].IsActive != 0)
								{
									_ = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num28 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num29 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num30 = ptr[Main.LargeWorldH].Type;
								}
								if (num28 >= 0 && Main.TileSolidAndAttach[num28])
								{
									ptr->FrameX = 0;
								}
								else if (num29 >= 0 && (Main.TileSolidAndAttach[num29] || num29 == (int)EntityID.TileID.WOODEN_BEAM || num29 == (int)EntityID.TileID.TREE))
								{
									ptr->FrameX = 18;
								}
								else if (num30 >= 0 && (Main.TileSolidAndAttach[num30] || num30 == (int)EntityID.TileID.WOODEN_BEAM || num30 == (int)EntityID.TileID.TREE))
								{
									ptr->FrameX = 36;
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.CRYSTAL_SHARD:
						case EntityID.TileID.XMAS_LIGHT:
							{
								int num15 = -1;
								int num16 = -1;
								int num17 = -1;
								int num18 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num16 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num15 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num17 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num18 = ptr[Main.LargeWorldH].Type;
								}
								if (num15 >= 0 && Main.TileSolidNotSolidTop[num15])
								{
									ptr->FrameY = 0;
								}
								else if (num17 >= 0 && Main.TileSolidNotSolidTop[num17])
								{
									ptr->FrameY = 54;
								}
								else if (num18 >= 0 && Main.TileSolidNotSolidTop[num18])
								{
									ptr->FrameY = 36;
								}
								else if (num16 >= 0 && Main.TileSolidNotSolidTop[num16])
								{
									ptr->FrameY = 18;
								}
								else
								{
									KillTile(i, j);
								}
								break;
							}
						case EntityID.TileID.LIFE_CRYSTAL:
						case EntityID.TileID.SHADOW_ORB:
							break;
						case EntityID.TileID.DOOR_CLOSED:
							{
								if (ToDestroyObject)
								{
									break;
								}
								int num27 = j - frameY / 18;
								bool flag2 = false;
								if (Main.TileSet[i, num27 - 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, num27 - 1].Type])
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 3].IsActive == 0 || !Main.TileSolid[Main.TileSet[i, num27 + 3].Type])
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27].IsActive == 0 || Main.TileSet[i, num27].Type != num)
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 1].IsActive == 0 || Main.TileSet[i, num27 + 1].Type != num)
								{
									flag2 = true;
								}
								else if (Main.TileSet[i, num27 + 2].IsActive == 0 || Main.TileSet[i, num27 + 2].Type != num)
								{
									flag2 = true;
								}
								if (flag2)
								{
									ToDestroyObject = true;
									KillTile(i, num27);
									KillTile(i, num27 + 1);
									KillTile(i, num27 + 2);
									if (!Gen)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
									}
								}
								ToDestroyObject = false;
								break;
							}
						case EntityID.TileID.DOOR_OPEN:
							{
								if (ToDestroyObject)
								{
									break;
								}
								int num10 = 0;
								int num11 = 0;
								int num12 = i;
								int num13 = j;
								bool flag = false;
								switch (frameX)
								{
									case 0:
										num10 = 1;
										break;
									case 18:
										num12 = i - 1;
										num11 = -Main.LargeWorldH;
										num10 = 1;
										break;
									case 36:
										num11 = Main.LargeWorldH;
										num12 = i + 1;
										num10 = -1;
										break;
									case 54:
										num10 = -1;
										break;
								}
								switch (frameY)
								{
									case 18:
										num11--;
										num13 = j - 1;
										break;
									case 36:
										num13 = j - 2;
										num11 -= 2;
										break;
								}
								if (ptr[num11 - 1].IsActive == 0 || !Main.TileSolid[ptr[num11 - 1].Type] || ptr[num11 + 3].IsActive == 0 || !Main.TileSolid[ptr[num11 + 3].Type])
								{
									flag = true;
									ToDestroyObject = true;
									if (!Gen)
									{
										Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
									}
								}
								int num14 = num12;
								if (num10 == -1)
								{
									num14 = num12 - 1;
								}
								for (int k = num14; k < num14 + 2; k++)
								{
									for (int l = num13; l < num13 + 3; l++)
									{
										if (!flag)
										{
											fixed (Tile* ptr2 = &Main.TileSet[k, l])
											{
												if (ptr2->Type != (int)EntityID.TileID.DOOR_OPEN || ptr2->IsActive == 0)
												{
													ToDestroyObject = true;
													flag = true;
													k = num14;
													l = num13;
													if (!Gen)
													{
														Item.NewItem(i * 16, j * 16, 16, 16, (int)EntityID.ItemID.WOODEN_DOOR);
													}
												}
											}
										}
										if (flag)
										{
											KillTile(k, l);
										}
									}
								}
								ToDestroyObject = false;
								break;
							}
						case EntityID.TileID.CHANDELIER:
						case EntityID.TileID.JACK_O_LANTERN:
						case EntityID.TileID.PRESENT:
						case EntityID.TileID.SAWMILL:
							break;
						case EntityID.TileID.CHAIR:
						case EntityID.TileID.SAPLING:
							break;
						case EntityID.TileID.TABLE:
						case EntityID.TileID.FURNACE:
						case EntityID.TileID.DEMON_ALTAR:
						case EntityID.TileID.HELLFORGE:
						case EntityID.TileID.LOOM:
						case EntityID.TileID.PIANO:
						case EntityID.TileID.DRESSER:
						case EntityID.TileID.BENCH:
						case EntityID.TileID.TINKERERS_WORKSHOP:
						case EntityID.TileID.ADAMANTITE_FORGE:
#if VERSION_101
						case EntityID.TileID.CAMPFIRE:
#endif
							break;
						case EntityID.TileID.PRESSURE_PLATE:
						case EntityID.TileID.EXPLOSIVES:
						case EntityID.TileID.TIMER:
							break;
						case EntityID.TileID.ANVIL:
						case EntityID.TileID.WORK_BENCH:
						case EntityID.TileID.PIGGYBANK:
						case EntityID.TileID.BOWL:
						case EntityID.TileID.MYTHRIL_ANVIL:
							break;
						case EntityID.TileID.BOTTLE:
						case EntityID.TileID.CANDLE:
						case EntityID.TileID.BOOK:
							break;
						case EntityID.TileID.CLAY_POT:
							break;
						case EntityID.TileID.CHEST:
							break;
						case EntityID.TileID.SUNFLOWER:
							break;
						case EntityID.TileID.MANNEQUIN:
							break;
						case EntityID.TileID.KEG:
						case EntityID.TileID.CHINESE_LANTERN:
						case EntityID.TileID.COOKING_POT:
						case EntityID.TileID.SAFE:
						case EntityID.TileID.SKULL_LANTERN:
						case EntityID.TileID.TRASH_CAN:
						case EntityID.TileID.CANDELABRA:
						case EntityID.TileID.CRYSTAL_BALL:
						case EntityID.TileID.DISCO_BALL:
						case EntityID.TileID.LEVER:
						case EntityID.TileID.BOULDER:
						case EntityID.TileID.PUMP_IN:
						case EntityID.TileID.PUMP_OUT:
							break;
						case EntityID.TileID.BANNER:
							break;
						case EntityID.TileID.MUSIC_BOX:
							break;
						case EntityID.TileID.LAMP_POST:
						case EntityID.TileID.TIKI_TORCH:
							break;
						case EntityID.TileID.BOOKCASE:
						case EntityID.TileID.THRONE:
							break;
						case EntityID.TileID.GRANDFATHERS_CLOCK:
						case EntityID.TileID.STATUE:
							break;
						case EntityID.TileID.CHAIN_LANTERN:
							break;
						case EntityID.TileID.DAYBLOOM_GROWING:
						case EntityID.TileID.DAYBLOOM_MATURE:
						case EntityID.TileID.DAYBLOOM_BLOOMING:
							break;
						case EntityID.TileID.SIGN:
						case EntityID.TileID.TOMBSTONE:
							break;
						case EntityID.TileID.BED:
						case EntityID.TileID.BATHTUB:
							break;
						case EntityID.TileID.GIANT_GLOWING_MUSHROOM:
							{
								int num8 = -1;
								int num9 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num9 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num8 = ptr[1].Type;
								}
								if (num8 != num && num8 != (int)EntityID.TileID.MUSHROOM_GRASS)
								{
									KillTile(i, j);
								}
								else if (num9 != num && ptr->FrameX == 0)
								{
									ptr->frameNumber = (byte)genRand.Next(3);
									ptr->FrameX = 18;
									ptr->FrameY = (short)(18 * ptr->frameNumber);
								}
								break;
							}
						case EntityID.TileID.CORAL:
							{
								int num4 = -1;
								int num5 = -1;
								int num6 = -1;
								int num7 = -1;
								if (ptr[-1].IsActive != 0)
								{
									num5 = ptr[-1].Type;
								}
								if (ptr[1].IsActive != 0)
								{
									num4 = ptr[1].Type;
								}
								if (ptr[-Main.LargeWorldH].IsActive != 0)
								{
									num6 = ptr[-Main.LargeWorldH].Type;
								}
								if (ptr[Main.LargeWorldH].IsActive != 0)
								{
									num7 = ptr[Main.LargeWorldH].Type;
								}
								if (num6 != -1 || num5 != -1 || num7 != -1)
								{
									KillTile(i, j);
								}
								else if (num4 < 0 || !Main.TileSolid[num4])
								{
									KillTile(i, j);
								}
								break;
							}
					}
					return;
				}
				int num31 = (int)EntityID.TileID.NONE;
				int num32 = (int)EntityID.TileID.NONE;
				int num33 = (int)EntityID.TileID.NONE;
				int num34 = (int)EntityID.TileID.NONE;
				int num35 = (int)EntityID.TileID.NONE;
				int num36 = (int)EntityID.TileID.NONE;
				int num37 = (int)EntityID.TileID.NONE;
				int num38 = (int)EntityID.TileID.NONE;
				if (ptr[-Main.LargeWorldH].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH].Type;
					num34 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH].Type;
					num35 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-1].IsActive != 0)
				{
					int type = ptr[-1].Type;
					num32 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[1].IsActive != 0)
				{
					int type = ptr[1].Type;
					num37 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-Main.LargeWorldH - 1].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH - 1].Type;
					num31 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH - 1].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH - 1].Type;
					num33 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[-Main.LargeWorldH + 1].IsActive != 0)
				{
					int type = ptr[-Main.LargeWorldH + 1].Type;
					num36 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				if (ptr[Main.LargeWorldH + 1].IsActive != 0)
				{
					int type = ptr[Main.LargeWorldH + 1].Type;
					num38 = (Main.TileStone[type] ? (int)EntityID.TileID.STONE : type);
				}
				switch ((EntityID.TileID)num)
				{
					case EntityID.TileID.PLATFORM:
						if (num35 >= 0 && !Main.TileSolid[num35])
						{
							num35 = -1;
						}
						if (num34 >= 0 && !Main.TileSolid[num34])
						{
							num34 = -1;
						}
						num2 = ((num34 == num) ? ((num35 != num) ? ((num35 >= 0) ? 72 : 18) : 0) : ((num34 < 0) ? ((num35 == num) ? 36 : ((num35 <= 0) ? 90 : 126)) : ((num35 == num) ? 54 : ((num35 >= 0) ? 90 : 108))));
						num3 = 18 * ptr->frameNumber;
						break;
					case EntityID.TileID.CACTUS:
						CactusFrame(i, j);
						return;
					case EntityID.TileID.WATER_CANDLE:
						return;
				}
				mergeUp2 = false;
				mergeDown2 = false;
				mergeLeft2 = false;
				mergeRight2 = false;
				if (frameNumber < 0)
				{
					frameNumber = genRand.Next(3);
					ptr->frameNumber = (byte)frameNumber;
				}
				else
				{
					frameNumber = ptr->frameNumber;
				}
				if (num == (int)EntityID.TileID.DIRT)
				{
					if (num32 >= 0 && Main.TileMergeDirt[num32])
					{
						TileFrameNoLiquid(i, j - 1);
						if (mergeDown2)
						{
							num32 = num;
						}
					}
					if (num37 >= 0 && Main.TileMergeDirt[num37])
					{
						TileFrameNoLiquid(i, j + 1);
						if (mergeUp2)
						{
							num37 = num;
						}
					}
					if (num34 >= 0 && Main.TileMergeDirt[num34])
					{
						TileFrameNoLiquid(i - 1, j);
						if (mergeRight2)
						{
							num34 = num;
						}
					}
					if (num35 >= 0 && Main.TileMergeDirt[num35])
					{
						TileFrameNoLiquid(i + 1, j);
						if (mergeLeft2)
						{
							num35 = num;
						}
					}
					if (num32 == (int)EntityID.TileID.GRASS || num32 == (int)EntityID.TileID.CORRUPT_GRASS || num32 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num32 = num;
					}
					if (num37 == (int)EntityID.TileID.GRASS || num37 == (int)EntityID.TileID.CORRUPT_GRASS || num37 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num37 = num;
					}
					if (num34 == (int)EntityID.TileID.GRASS || num34 == (int)EntityID.TileID.CORRUPT_GRASS || num34 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num34 = num;
					}
					if (num35 == (int)EntityID.TileID.GRASS || num35 == (int)EntityID.TileID.CORRUPT_GRASS || num35 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num35 = num;
					}
					if (num31 >= (int)EntityID.TileID.DIRT && Main.TileMergeDirt[num31])
					{
						num31 = num;
					}
					else if (num31 == (int)EntityID.TileID.GRASS || num31 == (int)EntityID.TileID.CORRUPT_GRASS || num31 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num31 = num;
					}
					if (num33 >= 0 && Main.TileMergeDirt[num33])
					{
						num33 = num;
					}
					else if (num33 == (int)EntityID.TileID.GRASS || num33 == (int)EntityID.TileID.CORRUPT_GRASS || num33 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num33 = num;
					}
					if (num36 >= 0 && Main.TileMergeDirt[num36])
					{
						num36 = num;
					}
					else if (num36 == (int)EntityID.TileID.GRASS || num36 == (int)EntityID.TileID.CORRUPT_GRASS || num33 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num36 = num;
					}
					if (num38 >= (int)EntityID.TileID.DIRT && Main.TileMergeDirt[num38])
					{
						num38 = num;
					}
					else if (num38 == (int)EntityID.TileID.GRASS || num38 == (int)EntityID.TileID.CORRUPT_GRASS || num38 == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						num38 = num;
					}
					if (j < Main.RockLayer)
					{
						if (num32 == (int)EntityID.TileID.MUD)
						{
							num32 = (int)EntityID.TileID.LIQUID;
						}
						if (num37 == (int)EntityID.TileID.MUD)
						{
							num37 = (int)EntityID.TileID.LIQUID;
						}
						if (num34 == (int)EntityID.TileID.MUD)
						{
							num34 = (int)EntityID.TileID.LIQUID;
						}
						if (num35 == (int)EntityID.TileID.MUD)
						{
							num35 = (int)EntityID.TileID.LIQUID;
						}
						if (num31 == (int)EntityID.TileID.MUD)
						{
							num31 = (int)EntityID.TileID.LIQUID;
						}
						if (num33 == (int)EntityID.TileID.MUD)
						{
							num33 = (int)EntityID.TileID.LIQUID;
						}
						if (num36 == (int)EntityID.TileID.MUD)
						{
							num36 = (int)EntityID.TileID.LIQUID;
						}
						if (num38 == (int)EntityID.TileID.MUD)
						{
							num38 = (int)EntityID.TileID.LIQUID;
						}
					}
				}
				else if (Main.TileMergeDirt[num])
				{
					if (num32 == (int)EntityID.TileID.DIRT)
					{
						num32 = (int)EntityID.TileID.LIQUID;
					}
					if (num37 == (int)EntityID.TileID.DIRT)
					{
						num37 = (int)EntityID.TileID.LIQUID;
					}
					if (num34 == (int)EntityID.TileID.DIRT)
					{
						num34 = (int)EntityID.TileID.LIQUID;
					}
					if (num35 == (int)EntityID.TileID.DIRT)
					{
						num35 = (int)EntityID.TileID.LIQUID;
					}
					if (num31 == (int)EntityID.TileID.DIRT)
					{
						num31 = (int)EntityID.TileID.LIQUID;
					}
					if (num33 == (int)EntityID.TileID.DIRT)
					{
						num33 = (int)EntityID.TileID.LIQUID;
					}
					if (num36 == (int)EntityID.TileID.DIRT)
					{
						num36 = (int)EntityID.TileID.LIQUID;
					}
					if (num38 == (int)EntityID.TileID.DIRT)
					{
						num38 = (int)EntityID.TileID.LIQUID;
					}
					if (num == (int)EntityID.TileID.STONE)
					{
						if (j > Main.RockLayer)
						{
							if (num32 == (int)EntityID.TileID.MUD)
							{
								TileFrameNoLiquid(i, j - 1);
								if (mergeDown2)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.MUD)
							{
								TileFrameNoLiquid(i, j + 1);
								if (mergeUp2)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.MUD)
							{
								TileFrameNoLiquid(i - 1, j);
								if (mergeRight2)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.MUD)
							{
								TileFrameNoLiquid(i + 1, j);
								if (mergeLeft2)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.MUD)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.MUD)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.MUD)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.MUD)
							{
								num38 = num;
							}
						}
						if (num32 == (int)EntityID.TileID.ASH)
						{
							TileFrameNoLiquid(i, j - 1);
							if (mergeDown2)
							{
								num32 = num;
							}
						}
						if (num37 == (int)EntityID.TileID.ASH)
						{
							TileFrameNoLiquid(i, j + 1);
							if (mergeUp2)
							{
								num37 = num;
							}
						}
						if (num34 == (int)EntityID.TileID.ASH)
						{
							TileFrameNoLiquid(i - 1, j);
							if (mergeRight2)
							{
								num34 = num;
							}
						}
						if (num35 == (int)EntityID.TileID.ASH)
						{
							TileFrameNoLiquid(i + 1, j);
							if (mergeLeft2)
							{
								num35 = num;
							}
						}
						if (num31 == (int)EntityID.TileID.ASH)
						{
							num31 = num;
						}
						if (num33 == (int)EntityID.TileID.ASH)
						{
							num33 = num;
						}
						if (num36 == (int)EntityID.TileID.ASH)
						{
							num36 = num;
						}
						if (num38 == (int)EntityID.TileID.ASH)
						{
							num38 = num;
						}
					}
				}
				else
				{
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.HELLSTONE:
						case EntityID.TileID.OBSIDIAN_BRICK:
						case EntityID.TileID.HELLSTONE_BRICK:
							if (num32 == (int)EntityID.TileID.ASH)
							{
								num32 = (int)EntityID.TileID.LIQUID;
							}
							if (num37 == (int)EntityID.TileID.ASH)
							{
								num37 = (int)EntityID.TileID.LIQUID;
							}
							if (num34 == (int)EntityID.TileID.ASH)
							{
								num34 = (int)EntityID.TileID.LIQUID;
							}
							if (num35 == (int)EntityID.TileID.ASH)
							{
								num35 = (int)EntityID.TileID.LIQUID;
							}
							if (num31 == (int)EntityID.TileID.ASH)
							{
								num31 = (int)EntityID.TileID.LIQUID;
							}
							if (num33 == (int)EntityID.TileID.ASH)
							{
								num33 = (int)EntityID.TileID.LIQUID;
							}
							if (num36 == (int)EntityID.TileID.ASH)
							{
								num36 = (int)EntityID.TileID.LIQUID;
							}
							if (num38 == (int)EntityID.TileID.ASH)
							{
								num38 = (int)EntityID.TileID.LIQUID;
							}
							break;
						case EntityID.TileID.MUD:
							if (j > Main.RockLayer)
							{
								if (num32 == (int)EntityID.TileID.STONE)
								{
									num32 = (int)EntityID.TileID.LIQUID;
								}
								if (num37 == (int)EntityID.TileID.STONE)
								{
									num37 = (int)EntityID.TileID.LIQUID;
								}
								if (num34 == (int)EntityID.TileID.STONE)
								{
									num34 = (int)EntityID.TileID.LIQUID;
								}
								if (num35 == (int)EntityID.TileID.STONE)
								{
									num35 = (int)EntityID.TileID.LIQUID;
								}
								if (num31 == (int)EntityID.TileID.STONE)
								{
									num31 = (int)EntityID.TileID.LIQUID;
								}
								if (num33 == (int)EntityID.TileID.STONE)
								{
									num33 = (int)EntityID.TileID.LIQUID;
								}
								if (num36 == (int)EntityID.TileID.STONE)
								{
									num36 = (int)EntityID.TileID.LIQUID;
								}
								if (num38 == (int)EntityID.TileID.STONE)
								{
									num38 = (int)EntityID.TileID.LIQUID;
								}
							}
							if (num32 == (int)EntityID.TileID.JUNGLE_GRASS || num32 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num32 = num;
							}
							if (num37 == (int)EntityID.TileID.JUNGLE_GRASS || num37 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num37 = num;
							}
							if (num34 == (int)EntityID.TileID.JUNGLE_GRASS || num34 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num34 = num;
							}
							if (num35 == (int)EntityID.TileID.JUNGLE_GRASS || num35 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num35 = num;
							}
							if (num31 == (int)EntityID.TileID.JUNGLE_GRASS || num31 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.JUNGLE_GRASS || num33 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.JUNGLE_GRASS || num36 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.JUNGLE_GRASS || num38 == (int)EntityID.TileID.MUSHROOM_GRASS)
							{
								num38 = num;
							}
							if (j >= Main.RockLayer)
							{
								break;
							}
							if (num32 == (int)EntityID.TileID.DIRT)
							{
								TileFrameNoLiquid(i, j - 1);
								if (mergeDown2)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.DIRT)
							{
								TileFrameNoLiquid(i, j + 1);
								if (mergeUp2)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.DIRT)
							{
								TileFrameNoLiquid(i - 1, j);
								if (mergeRight2)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.DIRT)
							{
								TileFrameNoLiquid(i + 1, j);
								if (mergeLeft2)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.DIRT)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.DIRT)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.DIRT)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.DIRT)
							{
								num38 = num;
							}
							break;
						case EntityID.TileID.ASH:
							if (num32 == (int)EntityID.TileID.STONE)
							{
								num32 = (int)EntityID.TileID.LIQUID;
							}
							if (num37 == (int)EntityID.TileID.STONE)
							{
								num37 = (int)EntityID.TileID.LIQUID;
							}
							if (num34 == (int)EntityID.TileID.STONE)
							{
								num34 = (int)EntityID.TileID.LIQUID;
							}
							if (num35 == (int)EntityID.TileID.STONE)
							{
								num35 = (int)EntityID.TileID.LIQUID;
							}
							if (num31 == (int)EntityID.TileID.STONE)
							{
								num31 = (int)EntityID.TileID.LIQUID;
							}
							if (num33 == (int)EntityID.TileID.STONE)
							{
								num33 = (int)EntityID.TileID.LIQUID;
							}
							if (num36 == (int)EntityID.TileID.STONE)
							{
								num36 = (int)EntityID.TileID.LIQUID;
							}
							if (num38 == (int)EntityID.TileID.STONE)
							{
								num38 = (int)EntityID.TileID.LIQUID;
							}
							if (num32 == (int)EntityID.TileID.HELLSTONE || num32 == (int)EntityID.TileID.HELLSTONE_BRICK || num32 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrameNoLiquid(i, j - 1);
								if (mergeDown2)
								{
									num32 = num;
								}
							}
							if (num37 == (int)EntityID.TileID.HELLSTONE || num37 == (int)EntityID.TileID.HELLSTONE_BRICK || num37 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrameNoLiquid(i, j + 1);
								if (mergeUp2)
								{
									num37 = num;
								}
							}
							if (num34 == (int)EntityID.TileID.HELLSTONE || num34 == (int)EntityID.TileID.HELLSTONE_BRICK || num34 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrameNoLiquid(i - 1, j);
								if (mergeRight2)
								{
									num34 = num;
								}
							}
							if (num35 == (int)EntityID.TileID.HELLSTONE || num35 == (int)EntityID.TileID.HELLSTONE_BRICK || num35 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								TileFrameNoLiquid(i + 1, j);
								if (mergeLeft2)
								{
									num35 = num;
								}
							}
							if (num31 == (int)EntityID.TileID.HELLSTONE || num31 == (int)EntityID.TileID.HELLSTONE_BRICK || num31 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num31 = num;
							}
							if (num33 == (int)EntityID.TileID.HELLSTONE || num33 == (int)EntityID.TileID.HELLSTONE_BRICK || num33 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num33 = num;
							}
							if (num36 == (int)EntityID.TileID.HELLSTONE || num36 == (int)EntityID.TileID.HELLSTONE_BRICK || num36 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num36 = num;
							}
							if (num38 == (int)EntityID.TileID.HELLSTONE || num38 == (int)EntityID.TileID.HELLSTONE_BRICK || num38 == (int)EntityID.TileID.OBSIDIAN_BRICK)
							{
								num38 = num;
							}
							break;
						case EntityID.TileID.CORRUPTION_THORN:
							if (num37 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num37 = num;
							}
							break;
						case EntityID.TileID.JUNGLE_THORN:
							if (num37 == (int)EntityID.TileID.JUNGLE_GRASS)
							{
								num37 = num;
							}
							break;
						case EntityID.TileID.COBWEB:
							if (num32 >= 0 && !Main.TileNoAttach[num32])
							{
								num32 = num;
							}
							if (num37 >= 0 && !Main.TileNoAttach[num37])
							{
								num37 = num;
							}
							if (num34 >= 0 && !Main.TileNoAttach[num34])
							{
								num34 = num;
							}
							if (num35 >= 0 && !Main.TileNoAttach[num35])
							{
								num35 = num;
							}
							if (num31 >= 0 && !Main.TileNoAttach[num31])
							{
								num31 = num;
							}
							if (num33 >= 0 && !Main.TileNoAttach[num33])
							{
								num33 = num;
							}
							if (num36 >= 0 && !Main.TileNoAttach[num36])
							{
								num36 = num;
							}
							if (num38 >= 0 && !Main.TileNoAttach[num38])
							{
								num38 = num;
							}
							break;
					}
				}
				bool flag3 = false;
				if (num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.CORRUPT_GRASS || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS)
				{
					flag3 = true;
					if (num32 >= 0 && num32 != num && !Main.TileSolid[num32])
					{
						num32 = (int)EntityID.TileID.NONE;
					}
					if (num37 >= 0 && num37 != num && !Main.TileSolid[num37])
					{
						num37 = (int)EntityID.TileID.NONE;
					}
					if (num34 >= 0 && num34 != num && !Main.TileSolid[num34])
					{
						num34 = (int)EntityID.TileID.NONE;
					}
					if (num35 >= 0 && num35 != num && !Main.TileSolid[num35])
					{
						num35 = (int)EntityID.TileID.NONE;
					}
					if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
					{
						num31 = (int)EntityID.TileID.NONE;
					}
					if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
					{
						num33 = (int)EntityID.TileID.NONE;
					}
					if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
					{
						num36 = (int)EntityID.TileID.NONE;
					}
					if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
					{
						num38 = (int)EntityID.TileID.NONE;
					}
					int num39 = 0;
					switch ((EntityID.TileID)num)
					{
						case EntityID.TileID.JUNGLE_GRASS:
						case EntityID.TileID.MUSHROOM_GRASS:
							num39 = (int)EntityID.TileID.MUD;
							break;
						case EntityID.TileID.GRASS:
							if (num32 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num32 = num39;
							}
							if (num37 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num37 = num39;
							}
							if (num34 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num34 = num39;
							}
							if (num35 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num35 = num39;
							}
							if (num31 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num31 = num39;
							}
							if (num33 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num33 = num39;
							}
							if (num36 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num36 = num39;
							}
							if (num38 == (int)EntityID.TileID.CORRUPT_GRASS)
							{
								num38 = num39;
							}
							break;
						case EntityID.TileID.CORRUPT_GRASS:
							if (num32 == (int)EntityID.TileID.GRASS)
							{
								num32 = num39;
							}
							if (num37 == (int)EntityID.TileID.GRASS)
							{
								num37 = num39;
							}
							if (num34 == (int)EntityID.TileID.GRASS)
							{
								num34 = num39;
							}
							if (num35 == (int)EntityID.TileID.GRASS)
							{
								num35 = num39;
							}
							if (num31 == (int)EntityID.TileID.GRASS)
							{
								num31 = num39;
							}
							if (num33 == (int)EntityID.TileID.GRASS)
							{
								num33 = num39;
							}
							if (num36 == (int)EntityID.TileID.GRASS)
							{
								num36 = num39;
							}
							if (num38 == (int)EntityID.TileID.GRASS)
							{
								num38 = num39;
							}
							break;
					}
					if (num32 != num && num32 != num39 && (num37 == num || num37 == num39))
					{
						if (num34 == num39 && num35 == num)
						{
							num2 = 18 * frameNumber;
							num3 = 198;
						}
						else if (num34 == num && num35 == num39)
						{
							num2 = 54 + 18 * frameNumber;
							num3 = 198;
						}
					}
					else if (num37 != num && num37 != num39 && (num32 == num || num32 == num39))
					{
						if (num34 == num39 && num35 == num)
						{
							num2 = 18 * frameNumber;
							num3 = 216;
						}
						else if (num34 == num && num35 == num39)
						{
							num2 = 54 + 18 * frameNumber;
							num3 = 216;
						}
					}
					else if (num34 != num && num34 != num39 && (num35 == num || num35 == num39))
					{
						if (num32 == num39 && num37 == num)
						{
							num2 = 72;
							num3 = 144 + 18 * frameNumber;
						}
						else if (num37 == num && num35 == num32)
						{
							num2 = 72;
							num3 = 90 + 18 * frameNumber;
						}
					}
					else if (num35 != num && num35 != num39 && (num34 == num || num34 == num39))
					{
						if (num32 == num39 && num37 == num)
						{
							num2 = 90;
							num3 = 144 + 18 * frameNumber;
						}
						else if (num37 == num && num35 == num32)
						{
							num2 = 90;
							num3 = 90 + 18 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num)
					{
						if (num31 != num && num33 != num && num36 != num && num38 != num)
						{
							if (num38 == num39)
							{
								num3 = 324;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num33 == num39)
							{
								num3 = 342;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num36 == num39)
							{
								num3 = 360;
								num2 = 108 + 18 * frameNumber;
							}
							else if (num31 == num39)
							{
								num3 = 378;
								num2 = 108 + 18 * frameNumber;
							}
							else
							{
								num3 = 234;
								num2 = 144 + 54 * frameNumber;
							}
						}
						else if (num31 != num && num38 != num)
						{
							num3 = 306;
							num2 = 36 + 18 * frameNumber;
						}
						else if (num33 != num && num36 != num)
						{
							num3 = 306;
							num2 = 90 + 18 * frameNumber;
						}
						else if (num31 != num && num33 == num && num36 == num && num38 == num)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 == num && num33 != num && num36 == num && num38 == num)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 == num && num33 == num && num36 != num && num38 == num)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num31 == num && num33 == num && num36 == num && num38 != num)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num39 && num34 == num && num35 == num && num31 == (int)EntityID.TileID.NONE && num33 == (int)EntityID.TileID.NONE)
					{
						num3 = 18;
						num2 = 108 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num && num34 == num && num35 == num && num36 == (int)EntityID.TileID.NONE && num38 == (int)EntityID.TileID.NONE)
					{
						num3 = 36;
						num2 = 108 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num39 && num35 == num && num33 == (int)EntityID.TileID.NONE && num38 == (int)EntityID.TileID.NONE)
					{
						num2 = 198;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num39 && num31 == (int)EntityID.TileID.NONE && num36 == (int)EntityID.TileID.NONE)
					{
						num2 = 180;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num39 && num34 == num && num35 == num)
					{
						if (num33 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num31 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if (num32 == num39 && num37 == num && num34 == num && num35 == num)
					{
						if (num38 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num36 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num && num35 == num39)
					{
						if (num31 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num36 != (int)EntityID.TileID.NONE)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if (num32 == num && num37 == num && num34 == num39 && num35 == num)
					{
						if (num33 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num38 != (int)EntityID.TileID.NONE)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
					}
					else if ((num32 == num39 && num37 == num && num34 == num && num35 == num) || (num32 == num && num37 == num39 && num34 == num && num35 == num) || (num32 == num && num37 == num && num34 == num39 && num35 == num) || (num32 == num && num37 == num && num34 == num && num35 == num39))
					{
						num3 = 18;
						num2 = 18 + 18 * frameNumber;
					}
					if ((num32 == num || num32 == num39) && (num37 == num || num37 == num39) && (num34 == num || num34 == num39) && (num35 == num || num35 == num39))
					{
						if (num31 != num && num31 != num39 && (num33 == num || num33 == num39) && (num36 == num || num36 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num33 != num && num33 != num39 && (num31 == num || num31 == num39) && (num36 == num || num36 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
						}
						else if (num36 != num && num36 != num39 && (num31 == num || num31 == num39) && (num33 == num || num33 == num39) && (num38 == num || num38 == num39))
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
						}
						else if (num38 != num && num38 != num39 && (num31 == num || num31 == num39) && (num36 == num || num36 == num39) && (num33 == num || num33 == num39))
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
						}
					}
					if (num32 != num39 && num32 != num && num37 == num && num34 != num39 && num34 != num && num35 == num && num38 != num39 && num38 != num)
					{
						num3 = 270;
						num2 = 90 + 18 * frameNumber;
					}
					else if (num32 != num39 && num32 != num && num37 == num && num34 == num && num35 != num39 && num35 != num && num36 != num39 && num36 != num)
					{
						num3 = 270;
						num2 = 144 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && num32 == num && num34 != num39 && num34 != num && num35 == num && num33 != num39 && num33 != num)
					{
						num3 = 288;
						num2 = 90 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && num32 == num && num34 == num && num35 != num39 && num35 != num && num31 != num39 && num31 != num)
					{
						num3 = 288;
						num2 = 144 + 18 * frameNumber;
					}
					else if (num32 != num && num32 != num39 && num37 == num && num34 == num && num35 == num && num36 != num && num36 != num39 && num38 != num && num38 != num39)
					{
						num3 = 216;
						num2 = 144 + 54 * frameNumber;
					}
					else if (num37 != num && num37 != num39 && num32 == num && num34 == num && num35 == num && num31 != num && num31 != num39 && num33 != num && num33 != num39)
					{
						num3 = 252;
						num2 = 144 + 54 * frameNumber;
					}
					else if (num34 != num && num34 != num39 && num37 == num && num32 == num && num35 == num && num33 != num && num33 != num39 && num38 != num && num38 != num39)
					{
						num3 = 234;
						num2 = 126 + 54 * frameNumber;
					}
					else if (num35 != num && num35 != num39 && num37 == num && num32 == num && num34 == num && num31 != num && num31 != num39 && num36 != num && num36 != num39)
					{
						num3 = 234;
						num2 = 162 + 54 * frameNumber;
					}
					else if (num32 != num39 && num32 != num && (num37 == num39 || num37 == num) && num34 == num39 && num35 == num39)
					{
						num3 = 270;
						num2 = 36 + 18 * frameNumber;
					}
					else if (num37 != num39 && num37 != num && (num32 == num39 || num32 == num) && num34 == num39 && num35 == num39)
					{
						num3 = 288;
						num2 = 36 + 18 * frameNumber;
					}
					else if (num34 != num39 && num34 != num && (num35 == num39 || num35 == num) && num32 == num39 && num37 == num39)
					{
						num2 = 0;
						num3 = 270 + 18 * frameNumber;
					}
					else if (num35 != num39 && num35 != num && (num34 == num39 || num34 == num) && num32 == num39 && num37 == num39)
					{
						num2 = 18;
						num3 = 270 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num39 && num34 == num39 && num35 == num39)
					{
						num3 = 288;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num && num34 == num39 && num35 == num39)
					{
						num3 = 270;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num39 && num34 == num && num35 == num39)
					{
						num3 = 306;
						num2 = 198 + 18 * frameNumber;
					}
					else if (num32 == num39 && num37 == num39 && num34 == num39 && num35 == num)
					{
						num3 = 306;
						num2 = 144 + 18 * frameNumber;
					}
					if (num32 != num && num32 != num39 && num37 == num && num34 == num && num35 == num)
					{
						if ((num36 == num39 || num36 == num) && num38 != num39 && num38 != num)
						{
							num3 = 324;
							num2 = 18 * frameNumber;
						}
						else if ((num38 == num39 || num38 == num) && num36 != num39 && num36 != num)
						{
							num3 = 324;
							num2 = 54 + 18 * frameNumber;
						}
					}
					else if (num37 != num && num37 != num39 && num32 == num && num34 == num && num35 == num)
					{
						if ((num31 == num39 || num31 == num) && num33 != num39 && num33 != num)
						{
							num3 = 342;
							num2 = 18 * frameNumber;
						}
						else if ((num33 == num39 || num33 == num) && num31 != num39 && num31 != num)
						{
							num3 = 342;
							num2 = 54 + 18 * frameNumber;
						}
					}
					else if (num34 != num && num34 != num39 && num32 == num && num37 == num && num35 == num)
					{
						if ((num33 == num39 || num33 == num) && num38 != num39 && num38 != num)
						{
							num3 = 360;
							num2 = 54 + 18 * frameNumber;
						}
						else if ((num38 == num39 || num38 == num) && num33 != num39 && num33 != num)
						{
							num3 = 360;
							num2 = 18 * frameNumber;
						}
					}
					else if (num35 != num && num35 != num39 && num32 == num && num37 == num && num34 == num)
					{
						if ((num31 == num39 || num31 == num) && num36 != num39 && num36 != num)
						{
							num3 = 378;
							num2 = 18 * frameNumber;
						}
						else if ((num36 == num39 || num36 == num) && num31 != num39 && num31 != num)
						{
							num3 = 378;
							num2 = 54 + 18 * frameNumber;
						}
					}
					if ((num32 == num || num32 == num39) && (num37 == num || num37 == num39) && (num34 == num || num34 == num39) && (num35 == num || num35 == num39) && num31 != (int)EntityID.TileID.NONE && num33 != (int)EntityID.TileID.NONE && num36 != (int)EntityID.TileID.NONE && num38 != (int)EntityID.TileID.NONE)
					{
						num3 = 18;
						num2 = 18 + 18 * frameNumber;
					}
					if (num32 == num39)
					{
						num32 = (int)EntityID.TileID.LIQUID;
					}
					if (num37 == num39)
					{
						num37 = (int)EntityID.TileID.LIQUID;
					}
					if (num34 == num39)
					{
						num34 = (int)EntityID.TileID.LIQUID;
					}
					if (num35 == num39)
					{
						num35 = (int)EntityID.TileID.LIQUID;
					}
					if (num31 == num39)
					{
						num31 = (int)EntityID.TileID.LIQUID;
					}
					if (num33 == num39)
					{
						num33 = (int)EntityID.TileID.LIQUID;
					}
					if (num36 == num39)
					{
						num36 = (int)EntityID.TileID.LIQUID;
					}
					if (num38 == num39)
					{
						num38 = (int)EntityID.TileID.LIQUID;
					}
				}
				if (num2 == (int)EntityID.TileID.NONE && (Main.TileMergeDirt[num] || num == (int)EntityID.TileID.DIRT || num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.ASH || num == (int)EntityID.TileID.HELLSTONE || num == (int)EntityID.TileID.MUD || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS || num == (int)EntityID.TileID.HELLSTONE_BRICK || num == (int)EntityID.TileID.OBSIDIAN_BRICK))
				{
					if (!flag3)
					{
						flag3 = true;
						if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
						{
							num31 = (int)EntityID.TileID.NONE;
						}
						if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
						{
							num33 = (int)EntityID.TileID.NONE;
						}
						if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
						{
							num36 = (int)EntityID.TileID.NONE;
						}
						if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
						{
							num38 = (int)EntityID.TileID.NONE;
						}
					}
					if (num32 >= 0 && num32 != num)
					{
						num32 = (int)EntityID.TileID.NONE;
					}
					if (num37 >= 0 && num37 != num)
					{
						num37 = (int)EntityID.TileID.NONE;
					}
					if (num34 >= 0 && num34 != num)
					{
						num34 = (int)EntityID.TileID.NONE;
					}
					if (num35 >= 0 && num35 != num)
					{
						num35 = (int)EntityID.TileID.NONE;
					}
					if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
					{
						if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == num && num35 == num)
						{
							num3 = 108;
							num2 = 144 + 18 * frameNumber;
							mergeUp2 = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
						{
							num3 = 90;
							num2 = 144 + 18 * frameNumber;
							mergeDown2 = true;
						}
						else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 162;
							num3 = 126 + 18 * frameNumber;
							mergeLeft2 = true;
						}
						else if (num32 == num && num37 == num && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 144;
							num3 = 126 + 18 * frameNumber;
							mergeRight2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 36;
							num3 = 90 + 36 * frameNumber;
							mergeUp2 = true;
							mergeLeft2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 54;
							num3 = 90 + 36 * frameNumber;
							mergeUp2 = true;
							mergeRight2 = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 36;
							num3 = 108 + 36 * frameNumber;
							mergeDown2 = true;
							mergeLeft2 = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 54;
							num3 = 108 + 36 * frameNumber;
							mergeDown2 = true;
							mergeRight2 = true;
						}
						else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 180;
							num3 = 126 + 18 * frameNumber;
							mergeLeft2 = true;
							mergeRight2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
						{
							num3 = 180;
							num2 = 144 + 18 * frameNumber;
							mergeUp2 = true;
							mergeDown2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 198;
							num3 = 90 + 18 * frameNumber;
							mergeUp2 = true;
							mergeLeft2 = true;
							mergeRight2 = true;
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 198;
							num3 = 144 + 18 * frameNumber;
							mergeDown2 = true;
							mergeLeft2 = true;
							mergeRight2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == (int)EntityID.TileID.LIQUID)
						{
							num2 = 216;
							num3 = 144 + 18 * frameNumber;
							mergeUp2 = true;
							mergeDown2 = true;
							mergeRight2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == num)
						{
							num2 = 216;
							num3 = 90 + 18 * frameNumber;
							mergeUp2 = true;
							mergeDown2 = true;
							mergeLeft2 = true;
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
						{
							num3 = 198;
							num2 = 108 + 18 * frameNumber;
							mergeUp2 = true;
							mergeDown2 = true;
							mergeLeft2 = true;
							mergeRight2 = true;
						}
						else if (num32 == num && num37 == num && num34 == num && num35 == num)
						{
							if (num38 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 0;
								num3 = 90 + 36 * frameNumber;
							}
							else if (num36 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 18;
								num3 = 90 + 36 * frameNumber;
							}
							else if (num33 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 0;
								num3 = 108 + 36 * frameNumber;
							}
							else if (num31 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 18;
								num3 = 108 + 36 * frameNumber;
							}
						}
					}
					else
					{
						if (num != (int)EntityID.TileID.GRASS && num != (int)EntityID.TileID.CORRUPT_GRASS && num != (int)EntityID.TileID.JUNGLE_GRASS && num != (int)EntityID.TileID.MUSHROOM_GRASS && num != (int)EntityID.TileID.HALLOWED_GRASS)
						{
							if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.LIQUID && num34 == num && num35 == num)
							{
								num3 = 0;
								num2 = 234 + 18 * frameNumber;
								mergeDown2 = true;
							}
							else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.NONE && num34 == num && num35 == num)
							{
								num3 = 18;
								num2 = 234 + 18 * frameNumber;
								mergeUp2 = true;
							}
							else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 36;
								num2 = 234 + 18 * frameNumber;
								mergeRight2 = true;
							}
							else if (num32 == num && num37 == num && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.NONE)
							{
								num3 = 54;
								num2 = 234 + 18 * frameNumber;
								mergeLeft2 = true;
							}
						}
						if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == num)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == num)
							{
								num2 = 72;
								num3 = 144 + 18 * frameNumber;
								mergeUp2 = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID && num32 == num)
							{
								num2 = 72;
								num3 = 90 + 18 * frameNumber;
								mergeDown2 = true;
							}
						}
						else if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == num && num35 == (int)EntityID.TileID.NONE)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == num)
							{
								num2 = 90;
								num3 = 144 + 18 * frameNumber;
								mergeUp2 = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID && num32 == num)
							{
								num2 = 90;
								num3 = 90 + 18 * frameNumber;
								mergeDown2 = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == num && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == num)
							{
								num2 = 18 * frameNumber;
								num3 = 198;
								mergeLeft2 = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID && num34 == num)
							{
								num2 = 54 + 18 * frameNumber;
								num3 = 198;
								mergeRight2 = true;
							}
						}
						else if (num32 == num && num37 == (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == num)
							{
								num2 = 18 * frameNumber;
								num3 = 216;
								mergeLeft2 = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID && num34 == num)
							{
								num2 = 54 + 18 * frameNumber;
								num3 = 216;
								mergeRight2 = true;
							}
						}
						else if (num32 != (int)EntityID.TileID.NONE && num37 != (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 108;
								num3 = 216 + 18 * frameNumber;
								mergeUp2 = true;
								mergeDown2 = true;
							}
							else if (num32 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 126;
								num3 = 144 + 18 * frameNumber;
								mergeUp2 = true;
							}
							else if (num37 == (int)EntityID.TileID.LIQUID)
							{
								num2 = 126;
								num3 = 90 + 18 * frameNumber;
								mergeDown2 = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 != (int)EntityID.TileID.NONE && num35 != (int)EntityID.TileID.NONE)
						{
							if (num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 198;
								num2 = 162 + 18 * frameNumber;
								mergeLeft2 = true;
								mergeRight2 = true;
							}
							else if (num34 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 252;
								num2 = 18 * frameNumber;
								mergeLeft2 = true;
							}
							else if (num35 == (int)EntityID.TileID.LIQUID)
							{
								num3 = 252;
								num2 = 54 + 18 * frameNumber;
								mergeRight2 = true;
							}
						}
						else if (num32 == (int)EntityID.TileID.LIQUID && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							num2 = 108;
							num3 = 144 + 18 * frameNumber;
							mergeUp2 = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.LIQUID && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.NONE)
						{
							num2 = 108;
							num3 = 90 + 18 * frameNumber;
							mergeDown2 = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.LIQUID && num35 == (int)EntityID.TileID.NONE)
						{
							num3 = 234;
							num2 = 18 * frameNumber;
							mergeLeft2 = true;
						}
						else if (num32 == (int)EntityID.TileID.NONE && num37 == (int)EntityID.TileID.NONE && num34 == (int)EntityID.TileID.NONE && num35 == (int)EntityID.TileID.LIQUID)
						{
							num3 = 234;
							num2 = 54 + 18 * frameNumber;
							mergeRight2 = true;
						}
					}
				}
				if (num2 < 0)
				{
					if (!flag3)
					{
						flag3 = true;
						if (num32 >= 0 && num32 != num && !Main.TileSolid[num32])
						{
							num32 = (int)EntityID.TileID.NONE;
						}
						if (num37 >= 0 && num37 != num && !Main.TileSolid[num37])
						{
							num37 = (int)EntityID.TileID.NONE;
						}
						if (num34 >= 0 && num34 != num && !Main.TileSolid[num34])
						{
							num34 = (int)EntityID.TileID.NONE;
						}
						if (num35 >= 0 && num35 != num && !Main.TileSolid[num35])
						{
							num35 = (int)EntityID.TileID.NONE;
						}
						if (num31 >= 0 && num31 != num && !Main.TileSolid[num31])
						{
							num31 = (int)EntityID.TileID.NONE;
						}
						if (num33 >= 0 && num33 != num && !Main.TileSolid[num33])
						{
							num33 = (int)EntityID.TileID.NONE;
						}
						if (num36 >= 0 && num36 != num && !Main.TileSolid[num36])
						{
							num36 = (int)EntityID.TileID.NONE;
						}
						if (num38 >= 0 && num38 != num && !Main.TileSolid[num38])
						{
							num38 = (int)EntityID.TileID.NONE;
						}
					}
					if (num == (int)EntityID.TileID.GRASS || num == (int)EntityID.TileID.CORRUPT_GRASS || num == (int)EntityID.TileID.JUNGLE_GRASS || num == (int)EntityID.TileID.MUSHROOM_GRASS || num == (int)EntityID.TileID.HALLOWED_GRASS)
					{
						if (num32 == (int)EntityID.TileID.LIQUID)
						{
							num32 = num;
						}
						if (num37 == (int)EntityID.TileID.LIQUID)
						{
							num37 = num;
						}
						if (num34 == (int)EntityID.TileID.LIQUID)
						{
							num34 = num;
						}
						if (num35 == (int)EntityID.TileID.LIQUID)
						{
							num35 = num;
						}
						if (num31 == (int)EntityID.TileID.LIQUID)
						{
							num31 = num;
						}
						if (num33 == (int)EntityID.TileID.LIQUID)
						{
							num33 = num;
						}
						if (num36 == (int)EntityID.TileID.LIQUID)
						{
							num36 = num;
						}
						if (num38 == (int)EntityID.TileID.LIQUID)
						{
							num38 = num;
						}
					}
					if (num32 == num && num37 == num && num34 == num && num35 == num)
					{
						if (num31 != num && num33 != num)
						{
							num3 = 18;
							num2 = 108 + 18 * frameNumber;
						}
						else if (num36 != num && num38 != num)
						{
							num3 = 36;
							num2 = 108 + 18 * frameNumber;
						}
						else if (num31 != num && num36 != num)
						{
							num2 = 180;
							num3 = 18 * frameNumber;
						}
						else if (num33 != num && num38 != num)
						{
							num2 = 198;
							num3 = 18 * frameNumber;
						}
						else
						{
							num3 = 18;
							num2 = 18 + 18 * frameNumber;
						}
					}
					else if (num32 != num && num37 == num && num34 == num && num35 == num)
					{
						num3 = 0;
						num2 = 18 + 18 * frameNumber;
					}
					else if (num32 == num && num37 != num && num34 == num && num35 == num)
					{
						num3 = 36;
						num2 = 18 + 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 != num && num35 == num)
					{
						num2 = 0;
						num3 = 18 * frameNumber;
					}
					else if (num32 == num && num37 == num && num34 == num && num35 != num)
					{
						num2 = 72;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 == num && num34 != num && num35 == num)
					{
						num2 = 36 * frameNumber;
						num3 = 54;
					}
					else if (num32 != num && num37 == num && num34 == num && num35 != num)
					{
						num2 = 18 + 36 * frameNumber;
						num3 = 54;
					}
					else if (num32 == num && num37 != num && num34 != num && num35 == num)
					{
						num2 = 36 * frameNumber;
						num3 = 72;
					}
					else if (num32 == num && num37 != num && num34 == num && num35 != num)
					{
						num2 = 18 + 36 * frameNumber;
						num3 = 72;
					}
					else if (num32 == num && num37 == num && num34 != num && num35 != num)
					{
						num2 = 90;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 == num && num35 == num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 72;
					}
					else if (num32 != num && num37 == num && num34 != num && num35 != num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 0;
					}
					else if (num32 == num && num37 != num && num34 != num && num35 != num)
					{
						num2 = 108 + 18 * frameNumber;
						num3 = 54;
					}
					else if (num32 != num && num37 != num && num34 != num && num35 == num)
					{
						num2 = 162;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 == num && num35 != num)
					{
						num2 = 216;
						num3 = 18 * frameNumber;
					}
					else if (num32 != num && num37 != num && num34 != num && num35 != num)
					{
						num2 = 162 + 18 * frameNumber;
						num3 = 54;
					}
				}
				if (num2 < 0)
				{
					num3 = 18;
					num2 = 18 + 18 * frameNumber;
				}
				ptr->FrameX = (short)num2;
				ptr->FrameY = (short)num3;
				if (num == (int)EntityID.TileID.VINE || num == (int)EntityID.TileID.JUNGLE_VINE || num == (int)EntityID.TileID.HALLOWED_VINE)
				{
					num32 = ((ptr[-1].IsActive != 0) ? ptr[-1].Type : (int)EntityID.TileID.NONE);
					if (num == (int)EntityID.TileID.VINE && (num32 == (int)EntityID.TileID.HALLOWED_GRASS || num32 == (int)EntityID.TileID.HALLOWED_VINE))
					{
						ptr->Type = (int)EntityID.TileID.HALLOWED_VINE;
						SquareTileFrameNoLiquid(i, j);
						return;
					}
					if (num == (int)EntityID.TileID.HALLOWED_VINE && (num32 == (int)EntityID.TileID.GRASS || num32 == (int)EntityID.TileID.VINE))
					{
						ptr->Type = (int)EntityID.TileID.VINE;
						SquareTileFrameNoLiquid(i, j);
						return;
					}
					if (num32 != num && (num32 == (int)EntityID.TileID.NONE || (num == (int)EntityID.TileID.VINE && num32 != (int)EntityID.TileID.GRASS) || (num == (int)EntityID.TileID.JUNGLE_VINE && num32 != (int)EntityID.TileID.JUNGLE_GRASS) || (num == (int)EntityID.TileID.HALLOWED_VINE && num32 != (int)EntityID.TileID.HALLOWED_GRASS)))
					{
						KillTile(i, j);
					}
				}
				if (Gen || Main.NetMode == (int)NetModeSetting.CLIENT || (num != (int)EntityID.TileID.SAND && num != (int)EntityID.TileID.EBONSAND && num != (int)EntityID.TileID.PEARLSAND && num != (int)EntityID.TileID.SILT))
				{
					return;
				}
				Tile* ptr3 = ptr + 1;
				if (ptr3->IsActive == 0)
				{
					ptr3 = ptr - 1;
					if (ptr3->IsActive == 0 || ptr3->Type != (int)EntityID.TileID.CHEST)
					{
						ptr->IsActive = 0;
						sandBuffer[currentSandBuffer].Add(i, j);
						return;
					}
				}
			}
		}

		public static void UpdateMagmaLayerPos()
		{
			int num = Main.MaxTilesY - 230;
			int magmaVal = (int)((num - Main.WorldSurface) / 6.0) * 6;
			Main.MagmaLayer = Main.WorldSurface + magmaVal - 5;
			Main.MagmaLayerPixels = Main.MagmaLayer << 4;
		}
	}
}
