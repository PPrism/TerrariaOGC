using System;
using System.Security.Policy;
using Microsoft.Xna.Framework;

namespace Terraria
{
	public sealed class Collision
	{
		public static bool UpCol;

		public static bool DownCol;

		public static bool CanHit(ref Rectangle HitboxA, ref Rectangle HitboxB)
		{
			int RectAWidth = HitboxA.X + (HitboxA.Width >> 1) >> 4;
			int RectAHeight = HitboxA.Y + (HitboxA.Height >> 1) >> 4;
			int RectBWidth = HitboxB.X + (HitboxB.Width >> 1) >> 4;
			int RectBHeight = HitboxB.Y + (HitboxB.Height >> 1) >> 4;
			try
			{
				do
				{
					int WidthRemainder = Math.Abs(RectAWidth - RectBWidth);
					int HeightRemainder = Math.Abs(RectAHeight - RectBHeight);
					if (RectAWidth == RectBWidth && RectAHeight == RectBHeight)
					{
						return true;
					}
					if (WidthRemainder > HeightRemainder)
					{
						RectAWidth = ((RectAWidth >= RectBWidth) ? (RectAWidth - 1) : (RectAWidth + 1));
						if (Main.TileSet[RectAWidth, RectAHeight - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[RectAWidth, RectAHeight - 1].Type] && Main.TileSet[RectAWidth, RectAHeight + 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[RectAWidth, RectAHeight + 1].Type])
						{
							return false;
						}
					}
					else
					{
						RectAHeight = ((RectAHeight >= RectBHeight) ? (RectAHeight - 1) : (RectAHeight + 1));
						if (Main.TileSet[RectAWidth - 1, RectAHeight].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[RectAWidth - 1, RectAHeight].Type] && Main.TileSet[RectAWidth + 1, RectAHeight].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[RectAWidth + 1, RectAHeight].Type])
						{
							return false;
						}
					}
				}
				while (Main.TileSet[RectAWidth, RectAHeight].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[RectAWidth, RectAHeight].Type]);
				return false;
			}
			catch
			{
				return false;
			}
		}

		public static bool AnyPlayerOrNPC(int X, int Y, int Height)
		{
			Rectangle IntersectRect = default;
			IntersectRect.X = X << 4;
			IntersectRect.Y = Y << 4;
			IntersectRect.Width = 16;
			IntersectRect.Height = Height << 4;
			for (int PlayerIdx = Player.MaxNumPlayers - 1; PlayerIdx >= 0; PlayerIdx--)
			{
				if (Main.PlayerSet[PlayerIdx].Active != 0 && IntersectRect.Intersects(Main.PlayerSet[PlayerIdx].XYWH))
				{
					return true;
				}
			}
			for (int NpcIdx = NPC.MaxNumNPCs - 1; NpcIdx >= 0; NpcIdx--)
			{
				if (Main.NPCSet[NpcIdx].Active != 0 && IntersectRect.Intersects(Main.NPCSet[NpcIdx].XYWH))
				{
					return true;
				}
			}
			return false;
		}

		public unsafe static bool DrownCollision(ref Vector2 Position, int Width, int Height, int GravDir)
		{
			Vector2 PosVector = Position;
			int BaseW = 10;
			int BaseY = 12;
			if (BaseW > Width)
			{
				BaseW = Width;
			}
			if (BaseY > Height)
			{
				BaseY = Height;
			}
			PosVector.X += Width >> 1;
			PosVector.X -= BaseW >> 1;
			PosVector.Y -= 2;
			if (GravDir == -1)
			{
				PosVector.Y += (Height >> 1) - 6;
			}
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);
					for (int Y = MinY; Y < MaxY; Y++)
					{
						int LiquidType = TileIdx->Liquid;
						if (LiquidType > 0)
						{
							int XVar = X << 4;
							float YVar = Y << 4;
							float num = 256 - LiquidType;
							num *= 0.0625f;
							YVar += num;
							int num2 = 16 - (int)num;
							if (PosVector.X + BaseW > XVar && PosVector.X < XVar + 16 && PosVector.Y + BaseY > YVar && PosVector.Y < YVar + num2)
							{
								return true;
							}
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public unsafe static bool WetCollision(ref Vector2 Position, int Width, int Height)
		{
			Vector2 PosVector = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
			int BaseW = 10;
			int BaseH = Height >> 1;
			if (BaseW > Width)
			{
				BaseW = Width;
			}
			if (BaseH > Height)
			{
				BaseH = Height;
			}
			PosVector.X -= BaseW >> 1;
			PosVector.Y -= BaseH >> 1;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						int LiquidType = TileIdx->Liquid;
						if (LiquidType > 0)
						{
							int XVar = X << 4;
							float YVar = Y << 4;
							int num = 16;
							float num2 = 256 - LiquidType;
							num2 *= 0.0625f;
							YVar += num2;
							num -= (int)num2;
							if (PosVector.X + BaseW > XVar && PosVector.X < XVar + 16 && PosVector.Y + BaseH > YVar && PosVector.Y < YVar + num)
							{
								return true;
							}
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public unsafe static bool LavaCollision(ref Vector2 Position, int Width, int Height)
		{
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				Vector2 PosVector = default;
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						int LiquidType = TileIdx->Liquid;
						if (LiquidType > 0 && TileIdx->Lava != 0)
						{
							PosVector.X = X << 4;
							PosVector.Y = Y << 4;
							int num = 16;
							float num2 = 256 - LiquidType;
							num2 *= 0.0625f;
							PosVector.Y += num2;
							num -= (int)num2;
							if (Position.X + Width > PosVector.X && Position.X < PosVector.X + 16 && Position.Y + Height > PosVector.Y && Position.Y < PosVector.Y + num)
							{
								return true;
							}
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public unsafe static void TileCollision(ref Vector2 Position, ref Vector2 Velocity, int Width, int Height, bool CanFallThrough = false, bool CanAllowFall = false)
		{
			UpCol = false;
			DownCol = false;
			Vector2 VelVector = Velocity;
			float PosWidth = Position.X + Width;
			float PosHeight = Position.Y + Height;
			float DirectionX = Position.X + Velocity.X;
			float DirectionY = Position.Y + Velocity.Y;
			float DistanceX = DirectionX + Width;
			float DistanceY = DirectionY + Height;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)PosWidth >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)PosHeight >> 4) + 2;
			int XBase = -1;
			int YBase = -1;
			int XHigh = -1;
			int YHigh = -1;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					int XVar = X << 4;
					if (!(DistanceX > XVar) || !(DirectionX < XVar + 16))
					{
						continue;
					}
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->IsActive != 0)
						{
							int TileType = TileIdx->Type;
							bool IsTopSolid = Main.TileSolidTop[TileType];
							if ((IsTopSolid && TileIdx->FrameY == 0) || Main.TileSolid[TileType])
							{
								int YVar = Y << 4;
								if (DistanceY > YVar && DirectionY < YVar + 16)
								{
									if (PosHeight <= YVar)
									{
										DownCol = true;
										if (!IsTopSolid || !CanFallThrough || (!(VelVector.Y <= 1) && !CanAllowFall))
										{
											XHigh = X;
											YHigh = Y;
											if (XHigh != XBase)
											{
												Velocity.Y = YVar - PosHeight;
											}
										}
									}
									else if (!IsTopSolid)
									{
										if (PosWidth <= XVar)
										{
											XBase = X;
											YBase = Y;
											if (YBase != YHigh)
											{
												Velocity.X = XVar - PosWidth;
											}
											if (XHigh == XBase)
											{
												Velocity.Y = VelVector.Y;
											}
										}
										else if (Position.X >= XVar + 16)
										{
											XBase = X;
											YBase = Y;
											if (YBase != YHigh)
											{
												Velocity.X = XVar + 16 - Position.X;
											}
											if (XHigh == XBase)
											{
												Velocity.Y = VelVector.Y;
											}
										}
										else if (Position.Y >= YVar + 16)
										{
											UpCol = true;
											XHigh = X;
											YHigh = Y;
											Velocity.Y = YVar + 16 - Position.Y;
											if (YHigh == YBase)
											{
												Velocity.X = VelVector.X;
											}
										}
									}
								}
							}
						}
						TileIdx++;
					}
				}
			}
		}

		public unsafe static bool SolidCollision(ref Vector2 Position, int Width, int Height)
		{
			float PosWidth = Position.X + Width;
			float PosHeight = Position.Y + Height;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)PosWidth >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)PosHeight >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					if (!(PosWidth > X << 4) || !(Position.X < X + 1 << 4))
					{
						continue;
					}
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);
					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->IsActive != 0 && PosHeight > Y << 4 && Position.Y < Y + 1 << 4 && Main.TileSolidNotSolidTop[TileIdx->Type])
						{
							return true;
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public unsafe static Vector2 WaterCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool CanFallThrough = false)
		{
			Vector2 VelVector = Velocity;
			Vector2 Direction = Position + Velocity;
			Vector2 PosVector = Position;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				Vector2 Base = default;
				for (int X = MinX; X < MaxX; X++)
				{
					Base.X = X << 4;
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						int LiquidType = TileIdx->Liquid;
						if (LiquidType > 0)
						{
							int num = LiquidType + 16 >> 5 << 1;
							Base.Y = (Y << 4) + 16 - num;
							if (Direction.X + Width > Base.X && Direction.X < Base.X + 16 && Direction.Y + Height > Base.Y && Direction.Y < Base.Y + num && PosVector.Y + Height <= Base.Y && !CanFallThrough)
							{
								VelVector.Y = Base.Y - (PosVector.Y + Height);
							}
						}
						TileIdx++;
					}
				}
			}
			return VelVector;
		}

		public unsafe static void AnyCollision(ref Vector2 Position, ref Vector2 Velocity, int Width, int Height)
		{
			Vector2 Direction = Position + Velocity;
			Vector2 PosVector = Position;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			int XBase = -1;
			int YBase = -1;
			int XHigh = -1;
			int YHigh = -1;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					float XVar = X << 4;
					if (!((Direction.X + Width) > XVar) || !(Direction.X < XVar + 16))
					{
						continue;
					}
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

                    for (int Y = MinY; Y < MaxY; Y++)
                    {
						if (TileIdx->IsActive != 0)
						{
							float YVar = Y << 4;
							if ((Direction.Y + Height) > YVar && Direction.Y < YVar + 16)
							{
								if ((PosVector.Y + Height) <= YVar)
								{
									XHigh = X;
									YHigh = Y;
									if (XHigh != XBase)
									{
										Velocity.Y = YVar - (PosVector.Y + Height);
									}
								}
								else if (!Main.TileSolidTop[TileIdx->Type])
								{
									if ((PosVector.X + Width) <= XVar)
									{
										XBase = X;
										YBase = Y;
										if (YBase != YHigh)
										{
											Velocity.X = XVar - (PosVector.X + Width);
										}
									}
									else if (PosVector.X >= XVar + 16)
									{
										XBase = X;
										YBase = Y;
										if (YBase != YHigh)
										{
											Velocity.X = XVar + 16 - PosVector.X;
										}
									}
									else if (PosVector.Y >= YVar + 16)
									{
										XHigh = X;
										YHigh = Y;
										Velocity.Y = YVar + 16 - PosVector.Y + 0.01f;
										if (YHigh == YBase)
										{
											Velocity.X += 0.01f;
										}
									}
								}
							}
						}
						TileIdx++;
					}
				}
			}
		}

		public unsafe static void HitTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
		{
			Vector2 Direction = Position + Velocity;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 1;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 1;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->CanStandOnTop())
						{
							float XVar = X << 4;
							float YVar = Y << 4;
							if ((Direction.X + Width) >= XVar && Direction.X <= XVar + 16 && (Direction.Y + Height) >= YVar && Direction.Y <= YVar + 16)
							{
								WorldGen.KillTile(X, Y, KillToFail: true, EffectOnly: true);
							}
						}
						TileIdx++;
					}
				}
			}
		}

		public unsafe static int HurtTiles(ref Vector2 Position, ref Vector2 Velocity, int Width, int Height, bool DontBurnPlayer = false)
		{
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 1;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 1;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);

					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->IsActive != 0)
						{
							int TileType = TileIdx->Type;
							if (TileType == 32 || TileType == 37 || TileType == 48 || TileType == 53 || TileType == 57 || TileType == 58 || TileType == 69 || TileType == 76 || TileType == 112 || TileType == 116 || TileType == 123)
							{
								float XVar = X << 4;
								float YVar = Y << 4;
								int Damage = 0;
								switch (TileType)
								{
									case 32:
									case 69:
									case 80:
										if ((Position.X + Width) > XVar && Position.X < XVar + 16 && (Position.Y + Height) > YVar && Position.Y < YVar + 16.01f)
										{
											Damage = 10;
											switch (TileType)
											{
												case 69:
													Damage = 17;
													break;
												case 80:
													Damage = 6;
													break;
											}
											if ((TileType == 32 || TileType == 69) && WorldGen.KillTile(X, Y))
											{
												NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 4, X, Y, 0);
												NetMessage.SendMessage();
											}
											return Damage;
										}
										break;
									case 53:
									case 112:
									case 116:
									case 123:
										if ((Position.X + Width - 2) >= XVar && (Position.X + 2) <= XVar + 16 && (Position.Y + Height - 2) >= YVar && (Position.Y + 2f) <= YVar + 16)
										{
											return 20;
										}
										break;
									default:
										if ((Position.X + Width) >= XVar && Position.X <= XVar + 16 && (Position.Y + Height) >= YVar && Position.Y <= YVar + 16.01f)
										{
											if (TileType == 48)
											{
												Damage = 40;
											}
											else if (!DontBurnPlayer && (TileType == 37 || TileType == 58 || TileType == 76))
											{
												Damage = 20;
											}
											return Damage;
										}
										break;
								}
							}
						}
						TileIdx++;
					}
				}
			}
			return 0;
		}

		public unsafe static bool SwitchTiles(Vector2 Position, int Width, int Height, Vector2 OldPosition)
		{
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 1;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 1;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);
					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->Type == 135 && TileIdx->IsActive != 0)
						{
							float XVar = X << 4;
							float YVar = (Y << 4) + 12;
							if ((Position.X + Width) > XVar && Position.X < XVar + 16 && (Position.Y + Height) > YVar && Position.Y < YVar + 4.01f && (!((OldPosition.X + Width) > XVar) || !(OldPosition.X < XVar + 16) || !((OldPosition.Y + Height) > YVar) || !(OldPosition.Y < YVar + 16.01f)))
							{
								WorldGen.HitSwitch(X, Y);
								NetMessage.CreateMessage2(59, X, Y);
								NetMessage.SendMessage();
								return true;
							}
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public unsafe static Vector2i StickyTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
		{
			Vector2 PosVector = Position;
			int MinX = ((int)Position.X >> 4) - 1;
			int MaxX = ((int)Position.X + Width >> 4) + 2;
			int MinY = ((int)Position.Y >> 4) - 1;
			int MaxY = ((int)Position.Y + Height >> 4) + 2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX > Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY > Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);
					for (int Y = MinY; Y < MaxY; Y++)
					{
						if (TileIdx->Type == 51 && TileIdx->IsActive != 0)
						{
							float XVar = X << 4;
							float YVar = Y << 4;
							if ((PosVector.X + Width) > XVar && PosVector.X < XVar + 16 && (PosVector.Y + Height) > YVar && PosVector.Y < YVar + 16.01f)
							{
								if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) > 0.7f && Main.Rand.Next(30) == 0)
								{
									Main.DustSet.NewDust((int)XVar, (int)YVar, 16, 16, 30);
								}
								return new Vector2i(X, Y);
							}
						}
						TileIdx++;
					}
				}
			}
			return new Vector2i(-1, -1);
		}

		public unsafe static bool SolidTiles(int MinX, int MaxX, int MinY, int MaxY)
		{
			if (MinX < 0)
			{
				return true;
			}
			if (MaxX >= Main.MaxTilesX)
			{
				return true;
			}
			if (MinY < 0)
			{
				return true;
			}
			if (MaxY >= Main.MaxTilesY)
			{
				return true;
			}
			fixed (Tile* CreatedTile = Main.TileSet)
			{
				for (int X = MinX; X < MaxX + 1; X++)
				{
					Tile* TileIdx = CreatedTile + (X * Main.LargeWorldH + MinY);
					for (int Y = MinY; Y < MaxY + 1; Y++)
					{
						if (TileIdx->IsActive != 0 && Main.TileSolidNotSolidTop[TileIdx->Type])
						{
							return true;
						}
						TileIdx++;
					}
				}
			}
			return false;
		}

		public static bool LineIntersection(ref Vector2 GameView, ref Vector2 PlayerView, Vector2 HorizToVert, Vector2 MirrorHorizToVert, ref Vector2 Intersection)
		{
			Vector2 Remainder = PlayerView - GameView;
			Vector2 MidSection = MirrorHorizToVert - HorizToVert;
			Vector2 AnglePoint = HorizToVert - GameView;
			float Base = Remainder.X * MidSection.Y - Remainder.Y * MidSection.X;
			if (Base == 0f)
			{
				return false;
			}
			float SlopeA = (AnglePoint.X * MidSection.Y - AnglePoint.Y * MidSection.X) / Base;
			if (SlopeA < 0f || SlopeA > 1f)
			{
				return false;
			}
			float SlopeB = (AnglePoint.X * Remainder.Y - AnglePoint.Y * Remainder.X) / Base;
			if (SlopeB < 0f || SlopeB > 1f)
			{
				return false;
			}
			Intersection = GameView + SlopeA * Remainder;
			return true;
		}
	}
}
