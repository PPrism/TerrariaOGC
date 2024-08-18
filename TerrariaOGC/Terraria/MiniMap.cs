using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public sealed class MiniMap
	{
		private const int ScrollingSpeed = 8;

		private const float ScalingSpeed = 0.05f;

		private const float MinScale = 1f;

		private const float MaxScale = 4f;

		private const int MapOffsetX = 290;

		private const int SCMapOffsetX = 340;

		private const int MapTexSlices = 4;

		private static short Width, Height, TexWidth;

		private short MapX, MapY;

		private float MapScale = 1f;

		public volatile bool IsThreadDone;

		private short MapDestH = 486;

		private int Alpha;

		private Texture2D[] MapTexture;

		private static readonly uint[] WallColors = new uint[Main.MaxNumWallTypes] // This updates in the 1.2 versions to match PC colours.
		{
			0,
			0xFF424242,
			0xFF583D2E,
			0xFF312545,
			0xFF4A2E1C,
			0xFF454545,
			0xFF500000,
			0xFF000060,
			0xFF005000,
			0xFF48004F,
			0xFF774707,
			0xFF828282,
			0xFF3F1907,
			0xFF3F0707,
			0xFF000060,
			0xFF005500,
			0xFF583D2E,
			0xFF000043,
			0xFF003700,
			0xFF330037,
			0xFF000060,
			0xFF12242C,
			0xFF716363,
			0xFF4E4042,
			0xFF403033,
			0xFF0B233E,
			0xFF3C5B3A,
			0xFF3A291D,
			0xFF515465,
			0xFFAA0000,
			0xFF00AA00,
			0xFF9999BB,
#if VERSION_101
			// It appears the colours for the walls are merged with the tile colours however I cannot confirm this, so I am using a functional workaround.
			0xFF916A4F,
			0xFF808080,
			0xFF1CD85E,
			0xFF0D6524,
			0xFFFD3E03,
			0xFF634631
#endif
		};

		private static readonly uint[] TileColors = new uint[Main.MaxNumTilesets]
		{
			0xFF916A4F,
			0xFF808080,
			0xFF1CD85E,
			0xFF0D6524,
			0xFFFD3E03,
			0xFF634631,
			0xFF6B594E,
			0xFFC6561D,
			0xFFB9A417,
			0xFFD9DFDF,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFFFF0000,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF6B3A18,
			0xFF0D6524,
			0xFFDDB800,
			0xFF625FA7,
			0xFF8D89DF,
			0xFF8D89DF,
			0xFF4B4A82,
			0xFF9000FF,
			0xFFC4FF14,
			0xFF8C2726,
			0xFF00FFF2,
			0xFF684934,
			0xFF000000,
			0xFF7A618F,
			0xFFFD3E03,
			0xFFFD3E03,
			0xFFFD3E03,
			0xFFFD3E03,
			0xFFDF9F89,
			0xFF909090,
			0xFFB200FF,
			0xFFAC5B4D,
			0xFF545498,
			0xFFEF904B,
			0xFF39A851,
			0xFFB200FF,
			0xFFFFD514,
			0xFFE5E5E5,
			0xFFFF5900,
			0xFF6D6D6D,
			0xFF2B8FFF,
			0xFF00FFF2,
			0xFFFFFFFF,
			0xFF0D6524,
			0xFFFFDA38,
			0x40FFFFFF,
			0xFFFFAE5E,
			0xFF5751AD,
			0xFF44444C,
			0xFF662222,
			0xFF5C4449,
			0xFF8FD71D,
			0xFF8FD71D,
			0xFF8ACE1C,
			0xFF2A82FA,
			0xFF2A82FA,
			0xFF2A82FA,
			0xFF2A82FA,
			0xFF2A82FA,
			0xFF2A82FA,
			0xFF5E3037,
			0xFF5D7FFF,
			0xFFB1AE83,
			0xFF968F6E,
			0xFF0D6524,
			0xFF0D6524,
			0xFFB200FF,
			0xFFC6001D,
			0xFFD50010,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00A500,
			0xFFE55340,
			0xFFFF7800,
			0xFFFF7800,
			0xFFFF7800,
			0xFFC0C0C0,
			0xFFFF00FF,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFFFF00FF,
			0xFFFF00FF,
			0xFFFF00D0,
			0xFFFF00FF,
			0xFFFF00FF,
			0xFFFF00FF,
			0xFFFF00FF,
			0xFFFF00FF,
			0xFF00FFF2,
			0xFF00FFF2,
			0xFF0B508F,
			0xFF5BA9A9,
			0xFF4EC1E3,
			0xFF1E9648,
			0xFF801A34,
			0xFF67627A,
			0xFF1E9648,
			0xFF7F5C45,
			0xFF327FA1,
			0xFFD5C4C5,
			0xFFB5ACBE,
			0xFFD5C4C5,
			0xFF3F3F49,
			0xFF967A7D,
			0xFF2576AB,
			0xFF91BF75,
			0xFF595353,
			0xFF5C4436,
			0xFF81A5FF,
			0xFFDBDBDB,
			0xFF68B3C8,
			0xFF906850,
			0xFF004979,
			0xFFA5A5A5,
			0xFF1A1A1A,
			0xFFC90303,
			0xFF891012,
			0xFF96AE87,
			0xFFFD7272,
			0xFFCCC0C0,
			0xFF8C8C8C,
			0xFF636363,
			0xFF996343,
			0xFF7875B3,
			0xFFAD2323,
			0xFFC90303,
			0xFFC90303,
			0xFFC90303,
			0xFFFF0000,
			0xFF00FF00,
			0xFFEEEEFF,
			0xFFCCCCEE,
			0xFFFF0000,
#if !VERSION_INITIAL
			0xFFFD3E03
#endif
		};

		public static void OnStartGame()
		{
			Width = (short)(Main.MaxTilesX - 68);
			Height = (short)(Main.MaxTilesY - 68);
			TexWidth = (short)(Width / 4);
		}

		public void UpdateMap(UI ActiveUI)
		{
			int PosX = ActiveUI.CurrentView.SafeAreaOffsetLeft + ((UI.NumActiveViews > 1) ? SCMapOffsetX : MapOffsetX);
			int MapDestW = ActiveUI.CurrentView.ViewWidth - ActiveUI.CurrentView.SafeAreaOffsetRight - PosX;
			if (ActiveUI.IsAlternateLeftButtonDown())
			{
				MapX -= ScrollingSpeed;
			}
			if (ActiveUI.IsAlternateRightButtonDown())
			{
				MapX += ScrollingSpeed;
			}
			if (ActiveUI.IsAlternateUpButtonDown())
			{
				MapY -= ScrollingSpeed;
			}
			if (ActiveUI.IsAlternateDownButtonDown())
			{
				MapY += ScrollingSpeed;
			}
			float GPTrigger = ActiveUI.PadState.Triggers.Left;
			if (GPTrigger > UI.GpDeadZone)
			{
				MapScale -= ScalingSpeed * GPTrigger;
				if (MapScale < MinScale)
				{
					MapScale = MinScale;
				}
			}
			GPTrigger = ActiveUI.PadState.Triggers.Right;
			if (GPTrigger > UI.GpDeadZone)
			{
				MapScale += ScalingSpeed * GPTrigger;
				if (MapScale > MaxScale)
				{
					MapScale = MaxScale;
				}
			}
			int MapMin = (int)((double)(MapDestW * (1f / MapScale - 1f)) * 0.5);
			int MaxX = Width - MapDestW - MapMin;
			if (MapX < MapMin)
			{
				MapX = (short)MapMin;
			}
			else if (MapX > MaxX)
			{
				MapX = (short)MaxX;
			}
			MapMin = (int)((double)(MapDestH * (1f / MapScale - 1f)) * 0.5);
			MaxX = Height - MapDestH - MapMin;
			if (MapY < MapMin)
			{
				MapY = (short)MapMin;
			}
			else if (MapY > MaxX)
			{
				MapY = (short)MaxX;
			}
		}

		public unsafe void CreateMap(UI CurrentUI)
		{
			if (IsThreadDone)
			{
				DestroyMap();
			}
			Alpha = 0;
			MapTexture = new Texture2D[MapTexSlices];
			for (int TexID = MapTexSlices - 1; TexID >= 0; TexID--)
			{
				MapTexture[TexID] = new Texture2D(WorldView.GraphicsDevice, TexWidth, Height, mipMap: false, SurfaceFormat.Bgr565);
			}
			Thread MapThread = new Thread(CreateMapThread);
			MapThread.IsBackground = true;
			MapThread.Start(CurrentUI);
		}

		public unsafe void CreateMapThread(object UIArg)
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				4
			});
#endif
			UI CurrentUI = (UI)UIArg;
			int SliceCount = TexWidth * Height;
			ushort[] Slices = new ushort[SliceCount];
			sbyte[] VertSects = new sbyte[Height];
			sbyte[] HorizSects = new sbyte[Width];
			Player ActivePlayer = CurrentUI.ActivePlayer;
			for (int TexID = MapTexSlices - 1; TexID >= 0; TexID--)
			{
				fixed (ushort* SlicePtr = &Slices[SliceCount - 1])
				{
					ushort* Slice = SlicePtr;
					for (int VertIdx = Height - 1; VertIdx >= 0; VertIdx--)
					{
						int WidthCount = TexWidth - 1;
						int HorizIdx = WidthCount + TexID * TexWidth;
						while (WidthCount >= 0)
						{
							fixed (Tile* TileIdx = &Main.TileSet[HorizIdx + Lighting.OffScreenTiles, VertIdx + Lighting.OffScreenTiles])
							{
								if ((TileIdx->CurrentFlags & Tile.Flags.VISITED) == 0)
								{
									if (VertSects[VertIdx] > 0)
									{
										VertSects[VertIdx]--;
									}
									if (HorizSects[HorizIdx] > 0)
									{
										HorizSects[HorizIdx]--;
									}
								}
								else
								{
									if (VertSects[VertIdx] < 4)
									{
										VertSects[VertIdx]++;
									}
									if (HorizSects[HorizIdx] < 4)
									{
										HorizSects[HorizIdx]++;
									}
								}
								if (VertSects[VertIdx] <= 0 && HorizSects[HorizIdx] <= 0)
								{
									*Slice = 0;
								}
								else
								{
									int num6 = ((VertSects[VertIdx] < HorizSects[HorizIdx]) ? ((VertSects[VertIdx] + 1) * 4 + HorizSects[HorizIdx] * HorizSects[HorizIdx]) : ((VertSects[VertIdx] <= HorizSects[HorizIdx]) ? (VertSects[VertIdx] * VertSects[VertIdx] + HorizSects[HorizIdx] * HorizSects[HorizIdx]) : (VertSects[VertIdx] * VertSects[VertIdx] + (HorizSects[HorizIdx] + 1) * 4)));
									uint TileColour;
									uint TileR;
									uint TileG;
									uint TileB;
									if (TileIdx->IsActive != 0)
									{
										TileColour = TileColors[TileIdx->Type];
										TileB = TileColour & 0xFF;
										TileG = (TileColour >> 8) & 0xFF;
										TileR = (TileColour >> 16) & 0xFF;
									}
									else if (TileIdx->WallType == 0)
									{
										if (VertIdx < Main.WorldSurface)
										{
											TileR = (uint)((VertIdx >> 1) * CurrentUI.CurrentView.WorldTime.bgColor.R / Main.WorldSurface);
											TileG = (uint)(VertIdx * CurrentUI.CurrentView.WorldTime.bgColor.G / (int)(Main.WorldSurface * Lighting.MaxBrightness));
											TileB = (uint)((VertIdx << 1) * CurrentUI.CurrentView.WorldTime.bgColor.B / Main.WorldSurface);
											if (TileB > 255)
											{
												TileB = 255;
											}
										}
										else if (VertIdx < Main.RockLayer)
										{
											TileR = 84;
											TileG = 57;
											TileB = 42;
										}
										else if (VertIdx >= Main.MaxTilesY - 200)
										{
											TileR = 51;
											TileG = 0;
											TileB = 0;
										}
										else
										{
											TileR = 72;
											TileG = 64;
											TileB = 57;
										}
									}
									else
									{
										TileColour = WallColors[TileIdx->WallType];
										TileB = TileColour & 0xFF;
										TileG = (TileColour >> 8) & 0xFF;
										TileR = (TileColour >> 16) & 0xFF;
									}
									uint liquid = TileIdx->Liquid;
									if (liquid != 0)
									{
										if (TileIdx->Lava == 32)
										{
											TileR = (TileR * (255 - liquid) >> 8) + liquid;
											TileB >>= 1;
										}
										else
										{
											TileR >>= 1;
											TileB = (TileB * (255 - liquid) >> 8) + liquid;
										}
										TileG >>= 1;
									}
									if (num6 < 32)
									{
										TileR = (uint)(TileR * num6) >> 5;
										TileG = (uint)(TileG * num6) >> 5;
										TileB = (uint)(TileB * num6) >> 5;
									}
									*Slice = (ushort)((TileR >> 3 << 11) | (TileG >> 2 << 5) | (TileB >> 3));
								}
								Slice--;
							}
							WidthCount--;
							HorizIdx--;
						}
					}
				}
				MapTexture[TexID].SetData(Slices);
			}
			int PosX = CurrentUI.CurrentView.SafeAreaOffsetLeft + ((UI.NumActiveViews > 1) ? SCMapOffsetX : MapOffsetX);
			int MapDestW = CurrentUI.CurrentView.ViewWidth - CurrentUI.CurrentView.SafeAreaOffsetRight - PosX;
			int MaxX = Width - MapDestW;
			int MaxY = Height - MapDestH;
			MapX = (short)((ActivePlayer.XYWH.X >> 4) - Lighting.OffScreenTiles - (MapDestW >> 1));
			if (MapX < 0)
			{
				MapX = 0;
			}
			else if (MapX > MaxX)
			{
				MapX = (short)MaxX;
			}
			MapY = (short)((ActivePlayer.XYWH.Y >> 4) - Lighting.OffScreenTiles - (MapDestH >> 1));
			if (MapY < 0)
			{
				MapY = 0;
			}
			else if (MapY > MaxY)
			{
				MapY = (short)MaxY;
			}
			IsThreadDone = true;
		}

		public void DestroyMap()
		{
			IsThreadDone = false;
			if (MapTexture != null)
			{
				for (int TexID = 0; TexID < MapTexSlices; TexID++)
				{
					MapTexture[TexID].Dispose();
					MapTexture[TexID] = null;
				}
				MapTexture = null;
			}
		}

		public void DrawMap(WorldView ActiveView)
		{
#if USE_ORIGINAL_CODE
			int PosX = ActiveView.SafeAreaOffsetLeft + ((UI.NumActiveViews > 1) ? SCMapOffsetX : MapOffsetX);
			int TopSafeAreaOffset = ActiveView.SafeAreaOffsetTop;
			MapDestH = (short)(Main.ResolutionHeight - ActiveView.SafeAreaOffsetTop - ActiveView.SafeAreaOffsetBottom - 36);
#else
			int PosX = ActiveView.SafeAreaOffsetLeft + ((UI.NumActiveViews > 1) ? (int)(SCMapOffsetX * Main.ScreenMultiplier) : (int)(MapOffsetX * Main.ScreenMultiplier));
			int TopSafeAreaOffset = ActiveView.SafeAreaOffsetTop;
			MapDestH = (short)(Main.ResolutionHeight - ActiveView.SafeAreaOffsetTop - ActiveView.SafeAreaOffsetBottom - (int)(36 * Main.ScreenMultiplier));
#endif
			int MapDestW = ActiveView.ViewWidth - ActiveView.SafeAreaOffsetRight - PosX;
			if (!IsThreadDone)
			{
				Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ActiveView.ScreenProjection);
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.ITEM_561, PosX + (MapDestW >> 1), TopSafeAreaOffset + (MapDestH >> 1), Color.White, (float)(Main.FrameCounter * (Math.PI / 30.0)), 1f);
				return;
			}
			Alpha += 16;
			if (Alpha > 255)
			{
				Alpha = 255;
			}
			int NewMapX = PosX + (MapDestW >> 1);
			int NewMapY = TopSafeAreaOffset + (MapDestH >> 1);
			Matrix Translation = Matrix.CreateTranslation(-NewMapX, -NewMapY, 0f) * Matrix.CreateScale(MapScale, MapScale, 1f) * Matrix.CreateTranslation(NewMapX, NewMapY, 0f);
			ActiveView.ScreenProjection.View = Translation;
			Rectangle ScissoredRectangle = default;
			if (ActiveView.IsFullScreen())
			{
				ScissoredRectangle.X = PosX;
				ScissoredRectangle.Y = TopSafeAreaOffset;
				ScissoredRectangle.Width = MapDestW;
				ScissoredRectangle.Height = MapDestH;
			}
			else
			{
				ScissoredRectangle.X = (PosX >> 1) + ActiveView.ActiveViewport.X;
				ScissoredRectangle.Y = (TopSafeAreaOffset >> 1) + ActiveView.ActiveViewport.Y;
				ScissoredRectangle.Width = MapDestW >> 1;
				ScissoredRectangle.Height = MapDestH >> 1;
			}
			WorldView.GraphicsDevice.ScissorRectangle = ScissoredRectangle;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, WorldView.ScissorRastTest, ActiveView.ScreenProjection);
			int TexCount = MapX / TexWidth;
			int MaxTexs = (MapX + MapDestW - 1) / TexWidth;
			if (MaxTexs >= MapTexture.Length)
			{
				MaxTexs = MapTexture.Length - 1;
			}
			Vector2 DrawPos = new Vector2(PosX, TopSafeAreaOffset);
			Rectangle Source = default;
			Source.X = MapX - TexCount * TexWidth;
			Source.Y = MapY;
			Source.Width = Math.Min(MapDestW, TexWidth - Source.X);
			Source.Height = MapDestH;
			Color Colour = new Color(Alpha, Alpha, Alpha, Alpha);
			for (int TexID = TexCount; TexID <= MaxTexs; TexID++)
			{
				Main.SpriteBatch.Draw(MapTexture[TexID], DrawPos, (Rectangle?)Source, Colour);
				DrawPos.X += Source.Width;
				Source.X = 0;
				Source.Width = TexWidth;
			}
			switch (Main.MagmaBGFrame)
			{
			case 0:
				Source.X = 659;
				Source.Y = 10;
				break;
			case 1:
				Source.X = 659;
				Source.Y = 0;
				break;
			default:
				Source.X = 759;
				Source.Y = 10;
				break;
			}
			Source.Width = 10;
			Source.Height = 10;
			for (int NPCIdx = NPC.MaxNumNPCs - 1; NPCIdx >= 0; NPCIdx--)
			{
				NPC ActiveNPC = Main.NPCSet[NPCIdx];
				if (ActiveNPC.Active != 0)
				{
					int TileX = ActiveNPC.XYWH.X >> 4;
					int TileY = ActiveNPC.XYWH.Y >> 4;
					if (TileX >= 0 && TileY >= 0 && TileX < Main.MaxTilesX && TileY < Main.MaxTilesY && (Main.TileSet[TileX, TileY].CurrentFlags & Tile.Flags.VISITED) == Tile.Flags.VISITED)
					{
						TileX -= Lighting.OffScreenTiles + MapX;
						if (TileX >= 0 && TileX + 4 < MapDestW)
						{
							TileY -= Lighting.OffScreenTiles + MapY;
							if (TileY >= 0 && TileY + 4 < MapDestH)
							{
								int HeadTextureID = ActiveNPC.GetHeadTextureID();
								if (HeadTextureID < 0)
								{
									Colour = new Color(106, 0, 66, 127);
									Colour.R = (byte)(Colour.R * UI.CursorColour.A >> 8);
									Colour.G = (byte)(Colour.G * UI.CursorColour.A >> 8);
									Colour.B = (byte)(Colour.B * UI.CursorColour.A >> 8);
									Colour.A = (byte)(Colour.A * UI.CursorColour.A >> 8);
									SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.DUST, PosX + TileX, TopSafeAreaOffset + TileY, Source, Colour);
								}
								else
								{
									SpriteSheet<_sheetSprites>.DrawScaled((int)_sheetSprites.ID.NPC_HEAD_1 - 1 + HeadTextureID, PosX + TileX, TopSafeAreaOffset + TileY, 0.5f, new Color(248, 248, 248, 248), (ActiveNPC.SpriteDirection >= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
								}
							}
						}
					}
				}
			}
			Main.SpriteBatch.End();
			Matrix ViewedWorld = ActiveView.ScreenProjection.World;
			float FrameScale = (float)(0.6875f + 0.0625f * Math.Sin(Main.FrameCounter * (1f/12f)));
			Translation = Matrix.CreateTranslation(-10f, -8f, 0f) * Matrix.CreateScale(FrameScale, FrameScale, 1f);
			for (int PlayerIdx = Player.MaxNumPlayers - 1; PlayerIdx >= 0; PlayerIdx--)
			{
				Player ActivePlayer = Main.PlayerSet[PlayerIdx];
				if (ActivePlayer.Active != 0 && !ActivePlayer.IsDead)
				{
					int PlayerTileX = ActivePlayer.XYWH.X >> 4;
					int PlayerTileY = ActivePlayer.XYWH.Y >> 4;
					if ((Main.TileSet[PlayerTileX, PlayerTileY].CurrentFlags & Tile.Flags.VISITED) != 0)
					{
						PlayerTileX -= Lighting.OffScreenTiles + MapX;
						PlayerTileY -= Lighting.OffScreenTiles + MapY;
						Vector2 PlayerPos = ActivePlayer.Position;
						ActivePlayer.Position.X = ActiveView.ScreenPosition.X;
						ActivePlayer.Position.Y = ActiveView.ScreenPosition.Y;
						ActiveView.ScreenProjection.World = Translation * Matrix.CreateTranslation(PosX + PlayerTileX, TopSafeAreaOffset + PlayerTileY, 0f);
						Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, WorldView.ScissorRastTest, ActiveView.ScreenProjection);
						ActivePlayer.Draw(ActiveView, IsMenuScr: true, IsIcon: true);
						Main.SpriteBatch.End();
						ActivePlayer.Position = PlayerPos;
						ActivePlayer.XYWH.X = (int)PlayerPos.X;
						ActivePlayer.XYWH.Y = (int)PlayerPos.Y;
					}
				}
			}
			ActiveView.ScreenProjection.World = ViewedWorld;
			ActiveView.ScreenProjection.View = Matrix.Identity;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ActiveView.ScreenProjection);
		}
	}
}
