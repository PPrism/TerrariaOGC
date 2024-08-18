using Microsoft.Xna.Framework;
using System.Threading;

namespace Terraria
{
	public sealed class Lighting
	{
		private struct TempLight
		{
			public short X;

			public short Y;

			public Vector3 ColorRGB;
		}

		public const int MaxRenderCount = 4;

		public const int OffScreenTiles = 34;

		public const int OffScreenTiles2 = 14;

#if USE_ORIGINAL_CODE
		private const int MaxLightArrayY = 107;

		private const int ColourH = 117;
#else
		private int MaxLightArrayY;

		private int ColourH;
#endif

		public const float MaxBrightness = 1.2f;

		private const int FirstToLightY7 = 0;

		private const int MaxTempLights = 1024;

		private int MaxLightArrayX;

		private int ColourW;

		public float BrightnessLevel;

		public float DefBrightness;

		public float OldSkyColor;

		public float SkyColor;

		private float LightColorR;

		private float LightColorG;

		private float LightColorB;

		public Vector3[,] Colour;

		public Vector3[,] Colour2;

		public byte[] StopAndWetLight;

		private int FirstTileX;

		private int FirstTileY;

		private int LastTileX;

		private int LastTileY;

		private int FirstToLightX;

		private int FirstToLightY;

		public short ScreenX = -1;

		public short ScreenY;

		public int MinimumX;

		public int MaximumX;

		public int MinimumY;

		public int MaximumY;

		private float NegLight = 0.04f;

		private float NegLight2 = 0.16f;

		private float WetLightR = 0.16f;

		private float WetLightG = 0.16f;

		private int MinimumX7;

		private int MaximumX7;

		private int MinimumY7;

		private int MaximumY7;

		private int FirstTileX7;

		private int LastTileX7;

		private int LastTileY7;

		private int FirstTileY7;

		private int LastToLightY7;

		private int FirstToLightX27;

		private int LastToLightX27;

		private int FirstToLightY27;

		private int LastToLightY27;

		private Thread ActiveWorkerThread;

		private readonly AutoResetEvent WorkerThreadReady = new AutoResetEvent(initialState: false);

		private readonly AutoResetEvent WorkerThreadGo = new AutoResetEvent(initialState: false);

		private volatile bool QuitWorkerThread;

		public static int TempLightCount;

		private static readonly TempLight[] TempLightSet = new TempLight[MaxTempLights];

		public void StartWorkerThread()
		{
			if (ActiveWorkerThread != null)
			{
				StopWorkerThread();
			}
			QuitWorkerThread = false;
			WorkerThreadReady.Reset();
			WorkerThreadGo.Reset();
			ActiveWorkerThread = new Thread(WorkerThread);
			ActiveWorkerThread.IsBackground = true;
			ActiveWorkerThread.Start();
		}

		public void StopWorkerThread()
		{
			if (ActiveWorkerThread != null)
			{
				QuitWorkerThread = true;
				WorkerThreadGo.Set();
				ActiveWorkerThread.Join();
				ActiveWorkerThread = null;
			}
		}

		private void WorkerThread()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				3
			});
#endif
			do
			{
				WorkerThreadReady.Set();
				WorkerThreadGo.WaitOne();
				lock (this)
				{
					DoColors();
				}
			}
			while (!QuitWorkerThread);
			WorkerThreadReady.Set();
		}

		public void SetWidth(int ResolutionWidth)
		{
			lock (this)
			{
				MaxLightArrayX = (ResolutionWidth + 64 >> 4) + 68;
				ColourW = MaxLightArrayX + 10;
#if !USE_ORIGINAL_CODE
				MaxLightArrayY = (Main.ResolutionHeight + 64 >> 4) + 70;
				ColourH = MaxLightArrayY + 10;
#endif
				Colour = new Vector3[MaxLightArrayX, MaxLightArrayY];
				Colour2 = new Vector3[ColourW, ColourH];
				StopAndWetLight = new byte[ColourW * ColourH];
			}
		}

		public unsafe void LightTiles(WorldView CurrentView)
		{
			FirstTileX = CurrentView.FirstTileX;
			LastTileX = CurrentView.LastTileX;
			FirstTileY = CurrentView.FirstTileY;
			LastTileY = CurrentView.LastTileY;
			FirstToLightX = FirstTileX - OffScreenTiles;
			FirstToLightY = FirstTileY - OffScreenTiles;
			int OffScreenX = LastTileX + OffScreenTiles;
			int OffScreenY = LastTileY + OffScreenTiles;
			if (FirstToLightX < 0)
			{
				FirstToLightX = 0;
			}
			if (OffScreenX >= Main.MaxTilesX)
			{
				OffScreenX = Main.MaxTilesX - 1;
			}
			if (FirstToLightY < 0)
			{
				FirstToLightY = 0;
			}
			if (OffScreenY >= Main.MaxTilesY)
			{
				OffScreenY = Main.MaxTilesY - 1;
			}
			if (Main.RenderCount <= MaxRenderCount)
			{
				int SectorIdx = (CurrentView.ScreenPosition.X >> 4) - (CurrentView.ScreenLastPosition.X >> 4);
				if (SectorIdx < 0 && SectorIdx >= -MaxRenderCount)
				{
					fixed (Vector3* ActiveLights = Colour)
					{
						int SectLightCount = (MaxLightArrayX + SectorIdx) * MaxLightArrayY - 1;
						SectorIdx *= MaxLightArrayY;
						Vector3* ToLightSet = ActiveLights + (MaxLightArrayX * MaxLightArrayY - 1);
						do
						{
							*ToLightSet = ToLightSet[SectorIdx];
							ToLightSet--;
						}
						while (--SectLightCount >= 0);
					}
				}
				else if (SectorIdx > 0 && SectorIdx <= MaxRenderCount)
				{
					fixed (Vector3* ActiveLights = Colour)
					{
						int SectLightCount = (MaxLightArrayX - SectorIdx) * MaxLightArrayY - 1;
						SectorIdx *= MaxLightArrayY;
						Vector3* ToLightSet = ActiveLights;
						do
						{
							*ToLightSet = ToLightSet[SectorIdx];
							ToLightSet++;
						}
						while (--SectLightCount >= 0);
					}
				}
				SectorIdx = (CurrentView.ScreenPosition.Y >> 4) - (CurrentView.ScreenLastPosition.Y >> 4);
				if (SectorIdx < 0 && SectorIdx >= -MaxRenderCount)
				{
					for (int LightX = 0; LightX < MaxLightArrayX; LightX++)
					{
						fixed (Vector3* ActiveLight = &Colour[LightX, 0])
						{
							for (int LightIdx = MaxLightArrayY + SectorIdx; LightIdx > -SectorIdx; LightIdx--)
							{
								ActiveLight[LightIdx] = ActiveLight[LightIdx + SectorIdx];
							}
						}
					}
				}
				else if (SectorIdx > 0 && SectorIdx <= MaxRenderCount)
				{
					for (int LightX = 0; LightX < MaxLightArrayX; LightX++)
					{
						fixed (Vector3* ActiveLight = &Colour[LightX, 0])
						{
							for (int LightIdx = 0; LightIdx < MaxLightArrayY - SectorIdx; LightIdx++)
							{
								ActiveLight[LightIdx] = ActiveLight[LightIdx + SectorIdx];
							}
						}
					}
				}
				OldSkyColor = SkyColor;
				SkyColor = (CurrentView.WorldTime.TileColorFore.X + CurrentView.WorldTime.TileColorFore.Y + CurrentView.WorldTime.TileColorFore.Z) * (1f / 3f);
				if (OldSkyColor == SkyColor)
				{
					return;
				}
				int LeftToLight = ((OffScreenY <= Main.WorldSurface) ? OffScreenY : Main.WorldSurface);
				fixed (Tile* TileSet = Main.TileSet)
				{
					for (int CurrentTile = FirstToLightX; CurrentTile < OffScreenX; CurrentTile++)
					{
						Tile* ToLightTile = TileSet + (CurrentTile * 1440 + FirstToLightY);

						for (int ToLightIdx = FirstToLightY; ToLightIdx < LeftToLight; ToLightIdx++)
						{
							if ((ToLightTile->IsActive == 0 || !Main.TileNoSunLight[ToLightTile->Type]) && (ToLightTile->WallType == 0 || ToLightTile->WallType == 21) && ToLightTile->Liquid < 200)
							{
								fixed (Vector3* ActiveLight = &Colour[CurrentTile - FirstToLightX, ToLightIdx - FirstToLightY])
								{
									if (ActiveLight->X < SkyColor)
									{
										ActiveLight->X = CurrentView.WorldTime.TileColorFore.X;
										if (ActiveLight->Y < SkyColor)
										{
											ActiveLight->Y = CurrentView.WorldTime.TileColorFore.Y;
										}
										if (ActiveLight->Z < SkyColor)
										{
											ActiveLight->Z = CurrentView.WorldTime.TileColorFore.Z;
										}
									}
								}
							}
							ToLightTile++;
						}
					}
				}
				return;
			}
			WorkerThreadReady.WaitOne();
			int LeftQuadX = CurrentView.ScreenPosition.X >> 4;
			int TopQuadY = CurrentView.ScreenPosition.Y >> 4;
			if (ScreenX >= 0)
			{
				LeftQuadX -= ScreenX;
				TopQuadY -= ScreenY;
				int FocusX = ((LeftQuadX < 0) ? (-LeftQuadX) : 0);
				int FocusY = ((TopQuadY < 0) ? (-TopQuadY) : 0);
				fixed (Vector3* ActiveLight = Colour)
				{
					fixed (Vector3* ActiveColour = Colour2)
					{
						for (int XIdx = FocusX; XIdx < MaxLightArrayX; XIdx++)
						{
							Vector3* LightSet = ActiveLight + (XIdx * MaxLightArrayY + FocusY);
							Vector3* ColourSet = ActiveColour + ((XIdx + LeftQuadX) * ColourH + FocusY + TopQuadY);
							for (int YIdx = FocusY; YIdx < MaxLightArrayY; YIdx++)
							{
								*(LightSet++) = *(ColourSet++);
							}
						}
					}
				}
			}
			fixed (Vector3* ActiveColour = Colour2)
			{
				fixed (byte* SWLight = StopAndWetLight)
				{
					Vector3* ColourPtr = ActiveColour;
					byte* ToSWLight = SWLight;
					for (int Colour = ColourW * ColourH - 1; Colour >= 0; Colour--)
					{
						ColourPtr->X = 0f;
						ColourPtr->Y = 0f;
						ColourPtr->Z = 0f;
						*ToSWLight = 0;
						ColourPtr++;
						ToSWLight++;
					}
				}
			}
			fixed (TempLight* TempLight = TempLightSet)
			{
				fixed (Vector3* ActiveColour = Colour2)
				{
					TempLight* CurrentTemp = TempLight;
					for (int TempLightIdx = TempLightCount - 1; TempLightIdx >= 0; TempLightIdx--)
					{
						int X = CurrentTemp->X - FirstToLightX;
						if (X >= 0 && X < ColourW)
						{
							int Y = CurrentTemp->Y - FirstToLightY;
							if (Y >= 0 && Y < ColourH)
							{
								Vector3* Colour = ActiveColour + (Y + X * ColourH);
								Colour->X = CurrentTemp->ColorRGB.X;
								Colour->Y = CurrentTemp->ColorRGB.Y;
								Colour->Z = CurrentTemp->ColorRGB.Z;
							}
						}
						CurrentTemp++;
					}
				}
			}
			int MinX = FirstTileX - OffScreenTiles2;
			int MinY = FirstTileY - OffScreenTiles2;
			int MaxX = LastTileX + OffScreenTiles2;
			int MaxY = LastTileY + OffScreenTiles2;
			if (MinX < 0)
			{
				MinX = 0;
			}
			if (MaxX >= Main.MaxTilesX)
			{
				MaxX = Main.MaxTilesX - 1;
			}
			if (MinY < 0)
			{
				MinY = 0;
			}
			if (MaxY >= Main.MaxTilesY)
			{
				MaxY = Main.MaxTilesY - 1;
			}
			if (NPC.WoF >= 0 && CurrentView.Player.IsHorrified)
			{
				try
				{
					int YSectStart = (CurrentView.ScreenPosition.Y >> 4) - 10;
					int YSectMax = (CurrentView.ScreenPosition.Y + Main.ResolutionHeight >> 4) + 10;
					int XSectStart = Main.NPCSet[NPC.WoF].XYWH.X >> 4;
					XSectStart = ((Main.NPCSet[NPC.WoF].Direction <= 0) ? (XSectStart + 2) : (XSectStart - 3));
					int XSectMax = XSectStart + 8;
					Vector3 WoFEffect = new Vector3(0.2f * (0.5f * Main.DemonTorch + 1f * (1f - Main.DemonTorch)), 0.03f, 0.3f * (Main.DemonTorch + 0.5f * (1f - Main.DemonTorch)));
					for (int X = XSectStart; X <= XSectMax; X++)
					{
						for (int Y = YSectStart; Y <= YSectMax; Y++)
						{
							Vector3.Max(ref WoFEffect, ref Colour2[X - FirstToLightX, Y - FirstToLightY], out Colour2[X - FirstToLightX, Y - FirstToLightY]);
						}
					}
				}
				catch
				{
				}
			}
			int FTLX = FirstToLightX;
			int OffX = OffScreenX;
			int FTLY = FirstToLightY;
			int OffY = OffScreenY;
			int Limit = ((OffY < Main.WorldSurface) ? OffY : Main.WorldSurface);
			fixed (Tile* CurrentTile = Main.TileSet)
			{
				for (int X = FTLX; X < OffX; X++)
				{
					Tile* ActiveTile = CurrentTile + (X * 1440 + FTLY);
					for (int Y = FTLY; Y < Limit; Y++)
					{
						int WallID = ActiveTile->WallType;
						if ((WallID == 0 || WallID == 21) && ActiveTile->Liquid < 200 && (ActiveTile->IsActive == 0 || !Main.TileNoSunLight[ActiveTile->Type]))
						{
							fixed (Vector3* ActiveColour = &Colour2[X - FirstToLightX, Y - FirstToLightY])
							{
								if (ActiveColour->X < SkyColor)
								{
									ActiveColour->X = CurrentView.WorldTime.TileColorFore.X;
									if (ActiveColour->Y < SkyColor)
									{
										ActiveColour->Y = CurrentView.WorldTime.TileColorFore.Y;
									}
									if (ActiveColour->Z < SkyColor)
									{
										ActiveColour->Z = CurrentView.WorldTime.TileColorFore.Z;
									}
								}
							}
						}
						ActiveTile++;
					}
				}
			}
			NegLight = 0.91f;
			NegLight2 = 0.72f;
			WetLightG = 0.97f * NegLight * UI.BlueWave;
			WetLightR = 0.88f * NegLight * UI.BlueWave;
			if (CurrentView.Player.NightVision)
			{
				NegLight *= 1.03f;
				NegLight2 *= 1.03f;
			}
			if (CurrentView.Player.blind)
			{
				NegLight *= 0.95f;
				NegLight2 *= 0.95f;
			}
			CurrentView.InactiveTiles = 0;
			CurrentView.SandTiles = 0;
			CurrentView.EvilTiles = 0;
			CurrentView.SnowTiles = 0;
			CurrentView.HolyTiles = 0;
			CurrentView.MeteorTiles = 0;
			CurrentView.JungleTiles = 0;
			CurrentView.DungeonTiles = 0;
#if !USE_ORIGINAL_CODE
			CurrentView.SkyTiles = 0;
#endif
			CurrentView.MusicBox = -1;
			MinimumX = ColourW;
			MaximumX = 0;
			MinimumY = ColourH;
			MaximumY = 0;
			fixed (Tile* CurrentTile = Main.TileSet)
			{
#if VERSION_101
				for (int X = FTLX; X < OffX; X++)
				{
					Tile* ActiveTile = CurrentTile + (X * 1440 + FTLY);

					for (int Y = FTLY; Y < OffY; Y++)
					{
						int ColourX = X - FirstToLightX;
						int ColourY = Y - FirstToLightY;
						fixed (Vector3* ActiveColour = &Colour2[ColourX, ColourY])
						{
							if ((ActiveTile->IsActive == 0 || !Main.TileNoSunLight[ActiveTile->Type]) && ActiveTile->WallType >= 32 && ActiveTile->WallType <= 37 && Y < Main.WorldSurface && ActiveTile->Liquid < 255)
							{
								float num42 = CurrentView.WorldTime.TileColorFore.X;
								float num43 = CurrentView.WorldTime.TileColorFore.Y;
								float num44 = CurrentView.WorldTime.TileColorFore.Z;
								switch (ActiveTile->WallType - 32)
								{
									case 0:
										num42 *= 0.9f;
										num43 *= 0.15f;
										num44 *= 0.9f;
										break;
									case 1:
										num42 *= 0.9f;
										num43 *= 0.9f;
										num44 *= 0.15f;
										break;
									case 2:
										num42 *= 0.15f;
										num43 *= 0.15f;
										num44 *= 0.9f;
										break;
									case 3:
										num42 *= 0.15f;
										num43 *= 0.9f;
										num44 *= 0.15f;
										break;
									case 4:
										num42 *= 0.9f;
										num43 *= 0.15f;
										num44 *= 0.15f;
										break;
									case 5:
										float num45 = 0.2f;
										float num46 = 0.7f - num45;
										num42 *= num46 + Main.DiscoRGB.X * num45;
										num43 *= num46 + Main.DiscoRGB.Y * num45;
										num44 *= num46 + Main.DiscoRGB.Z * num45;
										break;
								}
								if (ActiveColour->X < num42) // BUG: There exists an oddity with these checks and their placements in 1.01, which results in stained glass looking fucked, unlike in PC 1.2.
								{
									ActiveColour->X = num42; // I cannot find a consistent source for these checks however as a couple rounds of Ghidra and an IDA check had different results, so I will keep them here.
								}
								if (ActiveColour->Y < num43)
								{
									ActiveColour->Y = num43;
								}
								if (ActiveColour->Z < num44)
								{
									ActiveColour->Z = num44;
								}
							}
						}
						ActiveTile++;
					}
				}
#endif
				for (int X = FTLX; X < OffX; X++)
				{
					Tile* ActiveTile = CurrentTile + (X * 1440 + FTLY);

					for (int Y = FTLY; Y < OffY; Y++)
					{
						int ColourX = X - FirstToLightX;
						int ColourY = Y - FirstToLightY;
						fixed (Vector3* ActiveColour = &Colour2[ColourX, ColourY])
						{
							if (ActiveTile->IsActive == 0)
							{
								CurrentView.InactiveTiles++;
							}
							else
							{
								int TileID = ActiveTile->Type;
								int XOffset = OffX - FTLX - 99 >> 1;
								int YOffset = OffY - FTLY - 87 >> 1;
								if (X > FTLX + XOffset && X < OffX - XOffset && Y > FTLY + YOffset && Y < OffY - YOffset)
								{
									switch (TileID)
									{
									case 23:
									case 24:
									case 25:
									case 32:
										CurrentView.EvilTiles++;
										break;
									case 112:
										CurrentView.SandTiles++;
										CurrentView.EvilTiles++;
										break;
									case 109:
									case 110:
									case 113:
									case 117:
										CurrentView.HolyTiles++;
										break;
									case 116:
										CurrentView.SandTiles++;
										CurrentView.HolyTiles++;
										break;
									case 27:
										CurrentView.EvilTiles -= 5;
										break;
									case 37:
										CurrentView.MeteorTiles++;
										break;
									case 41:
									case 43:
									case 44:
										CurrentView.DungeonTiles++;
										break;
									case 60:
									case 61:
									case 62:
									case 84:
										CurrentView.JungleTiles++;
										break;
									case 53:
										CurrentView.SandTiles++;
										break;
									case 147:
									case 148:
										CurrentView.SnowTiles++;
										break;
#if !USE_ORIGINAL_CODE
									// This was made for the Floating Island achievement, and due to how it is done, you can fabricate it by building into space and just placing over 98 in total of these bricks.
#if VERSION_103 || VERSION_FINAL
									case 202: // Skyplate
#else
									case 45: // Gold Brick
									case 46: // Silver Brick
									case 47: // Copper Brick
#endif
										CurrentView.SkyTiles++;
										break;
#endif
										case 139:
										if (ActiveTile->FrameX >= 36)
										{
											CurrentView.MusicBox = ActiveTile->FrameY / 36;
										}
										break;
									}
								}
								if (Main.TileBlockLight[TileID])
								{
									StopAndWetLight[ColourX * ColourH + ColourY] = 1;
								}
								if (Main.TileLighted[TileID])
								{
									switch (TileID)
									{
#if VERSION_101
									case 150:
										float Flicker = Main.Rand.Next(28, 42) * 0.005f;
										Flicker += (270 - UI.MouseTextBrightness) / 700f;
										if (ActiveTile->FrameY <= 18 && ActiveTile->FrameX == 0)
										{
											float R = 0.9f + Flicker;
											float G = 0.3f + Flicker;
											float B = 0.1f + Flicker;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
#endif
									case 92:
										if (ActiveTile->FrameY <= 18 && ActiveTile->FrameX == 0)
										{
											float R = 1f;
											float G = 1f;
											float B = 1f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 93:
										if (ActiveTile->FrameY == 0 && ActiveTile->FrameX == 0)
										{
											float R = 1f;
											float G = 0.97f;
											float B = 0.85f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 98:
										if (ActiveTile->FrameY == 0)
										{
											float R = 1f;
											float G = 0.97f;
											float B = 0.85f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 4:
										if (ActiveTile->FrameX < 66)
										{
											float R;
											float G;
											float B;
											switch (ActiveTile->FrameY)
											{
											case 22:
												R = 0f;
												G = 0.1f;
												B = 1.3f;
												break;
											case 44:
												R = 1f;
												G = 0.1f;
												B = 0.1f;
												break;
											case 66:
												R = 0f;
												G = 1f;
												B = 0.1f;
												break;
											case 88:
												R = 0.95f;
												G = 0.1f;
												B = 0.95f;
												break;
											case 110:
												R = 1.3f;
												G = 1.3f;
												B = 1.3f;
												break;
											case 132:
												R = 1f;
												G = 1f;
												B = 0.1f;
												break;
											case 154:
												R = 0.5f * Main.DemonTorch + 1f * (1f - Main.DemonTorch);
												G = 0.3f;
												B = Main.DemonTorch + 0.5f * (1f - Main.DemonTorch);
												break;
											case 176:
												R = 0.85f;
												G = 1f;
												B = 0.7f;
												break;
											default:
												R = 1f;
												G = 0.95f;
												B = 0.8f;
												break;
											}
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 33:
										if (ActiveTile->FrameX == 0)
										{
											float R = 1f;
											float G = 0.95f;
											float B = 0.65f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 36:
										if (ActiveTile->FrameX < 54)
										{
											float R = 1f;
											float G = 0.95f;
											float B = 0.65f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 100:
										if (ActiveTile->FrameX < 36)
										{
											float R = 1f;
											float G = 0.95f;
											float B = 0.65f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 34:
									case 35:
										if (ActiveTile->FrameX < 54)
										{
											float R = 1f;
											float G = 0.95f;
											float B = 0.8f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 95:
										if (ActiveTile->FrameX < 36)
										{
											float R = 1f;
											float G = 0.95f;
											float B = 0.8f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 17:
									case 133:
									{
										float R = 0.83f;
										float G = 0.6f;
										float B = 0.5f;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 77:
									{
										float R = 0.75f;
										float G = 0.45f;
										float B = 0.25f;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 37:
									{
										float R = 0.56f;
										float G = 0.43f;
										float B = 0.15f;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 22:
									case 140:
									{
										float R = 0.12f;
										float G = 0.07f;
										float B = 0.32f;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 42:
										if (ActiveTile->FrameX == 0)
										{
											float R = 0.65f;
											float G = 0.8f;
											float B = 0.54f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 49:
									{
										float R = 0.3f;
										float G = 0.3f;
										float B = 0.75f;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 70:
									case 71:
									case 72:
									{
										float A = Main.Rand.Next(28, 42) * 0.005f;
										A += (270 - UI.MouseTextBrightness) * 0.002f;
										float R = 0.1f;
										float G = 0.3f + A;
										float B = 0.6f + A;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 61:
										if (ActiveTile->FrameX == 144)
										{
											float R = 0.42f;
											float G = 0.81f;
											float B = 0.52f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 26:
									case 31:
									{
										float A = Main.Rand.Next(-5, 6) * 0.0025f;
										float R = 0.31f + A;
										float G = 0.1f;
										float B = 0.44f + A;
										if (R > ActiveColour->X)
										{
											ActiveColour->X = R;
										}
										if (G > ActiveColour->Y)
										{
											ActiveColour->Y = G;
										}
										if (B > ActiveColour->Z)
										{
											ActiveColour->Z = B;
										}
										break;
									}
									case 84:
										switch (ActiveTile->FrameX / 18)
										{
											case 2:
											{
												float A = (270 - UI.MouseTextBrightness) * 0.00125f;
												if (A > 1f)
												{
													A = 1f;
												}
												else if (A < 0f)
												{
													A = 0f;
												}
												float R = 0.7f * A;
												float G = A;
												float B = 0.1f * A;
												if (R > ActiveColour->X)
												{
													ActiveColour->X = R;
												}
												if (G > ActiveColour->Y)
												{
													ActiveColour->Y = G;
												}
												if (B > ActiveColour->Z)
												{
													ActiveColour->Z = B;
												}
												break;
											}
											case 5:
											{
												float R = 0.9f;
												float G = 0.71999997f;
												float B = 0.17999999f;
												if (R > ActiveColour->X)
												{
													ActiveColour->X = R;
												}
												if (G > ActiveColour->Y)
												{
													ActiveColour->Y = G;
												}
												if (B > ActiveColour->Z)
												{
													ActiveColour->Z = B;
												}
												break;
											}
										}
										break;
									case 83:
										if (ActiveTile->FrameX == 18 && !CurrentView.WorldTime.DayTime)
										{
											float R = 0.1f;
											float G = 0.4f;
											float B = 0.6f;
											if (R > ActiveColour->X)
											{
												ActiveColour->X = R;
											}
											if (G > ActiveColour->Y)
											{
												ActiveColour->Y = G;
											}
											if (B > ActiveColour->Z)
											{
												ActiveColour->Z = B;
											}
										}
										break;
									case 126:
										if (ActiveTile->FrameX < 36)
										{
											if (Main.DiscoRGB.X > ActiveColour->X)
											{
												ActiveColour->X = Main.DiscoRGB.X;
											}
											if (Main.DiscoRGB.Y > ActiveColour->Y)
											{
												ActiveColour->Y = Main.DiscoRGB.Y;
											}
											if (Main.DiscoRGB.Z > ActiveColour->Z)
											{
												ActiveColour->Z = Main.DiscoRGB.Z;
											}
										}
										break;
									case 125:
									{
										float A = Main.Rand.Next(28, 42) * 0.01f;
										A += (270 - UI.MouseTextBrightness) * 0.00125f;
										if (ActiveColour->Y < 0.1f * A)
										{
											ActiveColour->Y = 0.3f * A;
										}
										if (ActiveColour->Z < 0.3f * A)
										{
											ActiveColour->Z = 0.6f * A;
										}
										break;
									}
									case 129:
									{
										float R;
										float G;
										float B;
										if (ActiveTile->FrameX == 0 || ActiveTile->FrameX == 54 || ActiveTile->FrameX == 108)
										{
											R = 0f;
											G = 0.05f;
											B = 0.25f;
										}
										else if (ActiveTile->FrameX == 18 || ActiveTile->FrameX == 72 || ActiveTile->FrameX == 126)
										{
											R = 0.2f;
											G = 0f;
											B = 0.15f;
										}
										else
										{
											R = 0.1f;
											G = 0f;
											B = 0.2f;
										}
										if (ActiveColour->X < R)
										{
											ActiveColour->X = R * Main.Rand.Next(970, 1031) * 0.001f;
										}
										if (ActiveColour->Y < G)
										{
											ActiveColour->Y = G * Main.Rand.Next(970, 1031) * 0.001f;
										}
										if (ActiveColour->Z < B)
										{
											ActiveColour->Z = B * Main.Rand.Next(970, 1031) * 0.001f;
										}
										break;
									}
									case 149:
										if (ActiveTile->FrameX <= 36)
										{
											float R;
											float G;
											float B;
											if (ActiveTile->FrameX == 0)
											{
												R = 0.1f;
												G = 0.2f;
												B = 0.5f;
											}
											else if (ActiveTile->FrameX == 18)
											{
												R = 0.5f;
												G = 0.1f;
												B = 0.1f;
											}
											else
											{
												R = 0.2f;
												G = 0.5f;
												B = 0.1f;
											}
											if (ActiveColour->X < R)
											{
												ActiveColour->X = R * Main.Rand.Next(970, 1031) * 0.001f;
											}
											if (ActiveColour->Y < G)
											{
												ActiveColour->Y = G * Main.Rand.Next(970, 1031) * 0.001f;
											}
											if (ActiveColour->Z < B)
											{
												ActiveColour->Z = B * Main.Rand.Next(970, 1031) * 0.001f;
											}
										}
										break;
									}
								}
							}
							if (ActiveTile->Liquid > 0)
							{
								if (ActiveTile->Lava != 0)
								{
									float A = 0.55f;
									A += (270 - UI.MouseTextBrightness) * 0.0011111111f;
									if (ActiveColour->X < A)
									{
										ActiveColour->X = A;
									}
									if (ActiveColour->Y < A)
									{
										ActiveColour->Y = A * 0.6f;
									}
									if (ActiveColour->Z < A)
									{
										ActiveColour->Z = A * 0.2f;
									}
								}
								else if (ActiveTile->Liquid > 128)
								{
									StopAndWetLight[ColourX * ColourH + ColourY] |= 2;
								}
							}
							if (ActiveColour->X > 0f || ActiveColour->Y > 0f || ActiveColour->Z > 0f)
							{
								if (MinimumX > ColourX)
								{
									MinimumX = ColourX;
								}
								if (MaximumX < ColourX + 1)
								{
									MaximumX = ColourX + 1;
								}
								if (MinimumY > ColourY)
								{
									MinimumY = ColourY;
								}
								if (MaximumY < ColourY + 1)
								{
									MaximumY = ColourY + 1;
								}
							}
						}
						ActiveTile++;
					}
				}
			}
			if (CurrentView.EvilTiles < 0)
			{
				CurrentView.EvilTiles = 0;
			}
			int HolyTileCount = CurrentView.HolyTiles;
			CurrentView.HolyTiles -= CurrentView.EvilTiles;
			CurrentView.EvilTiles -= HolyTileCount;
			if (CurrentView.HolyTiles < 0)
			{
				CurrentView.HolyTiles = 0;
			}
			if (CurrentView.EvilTiles < 0)
			{
				CurrentView.EvilTiles = 0;
			}
			MinimumX7 = MinimumX;
			MaximumX7 = MaximumX;
			MinimumY7 = MinimumY;
			MaximumY7 = MaximumY;
			MinimumX += FirstToLightX;
			MaximumX += FirstToLightX;
			MinimumY += FirstToLightY;
			MaximumY += FirstToLightY;
			FirstTileX7 = FirstTileX - FirstToLightX;
			LastTileX7 = LastTileX - FirstToLightX;
			LastTileY7 = LastTileY - FirstToLightY;
			FirstTileY7 = FirstTileY - FirstToLightY;
			LastToLightY7 = OffScreenY - FirstToLightY;
			FirstToLightX27 = MinX - FirstToLightX;
			LastToLightX27 = MaxX - FirstToLightX;
			FirstToLightY27 = MinY - FirstToLightY;
			LastToLightY27 = MaxY - FirstToLightY;
			WorkerThreadGo.Set();
			ScreenX = (short)(CurrentView.ScreenPosition.X >> 4);
			ScreenY = (short)(CurrentView.ScreenPosition.Y >> 4);
		}

		private unsafe void DoColors()
		{
			fixed (Vector3* ActiveColour = Colour2)
			{
				int LightCol = MinimumX7 * ColourH;
				for (int TileLight = MinimumX7; TileLight < MaximumX7; TileLight++)
				{
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					for (int MinLastCounter = MinimumY7; MinLastCounter < LastToLightY27 + 4; MinLastCounter++)
					{
						int VertOffset = LightCol + MinLastCounter;
						LightColor(ActiveColour + VertOffset, VertOffset, 1);
					}
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					for (int MaxLastCounter = MaximumY7; MaxLastCounter >= FirstTileY7 - 4; MaxLastCounter--)
					{
						int VertOffset = LightCol + MaxLastCounter;
						LightColor(ActiveColour + VertOffset, VertOffset, -1);
					}
					LightCol += ColourH;
				}
			}
			fixed (Vector3* ActiveColour = Colour2)
			{
				for (int TileLight = FirstToLightY7; TileLight < LastToLightY7; TileLight++)
				{
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					int VertOffset = TileLight + MinimumX7 * ColourH;
					for (int MinLastCounter = MinimumX7; MinLastCounter < LastTileX7 + 4; MinLastCounter++)
					{
						LightColor(ActiveColour + VertOffset, VertOffset, ColourH);
						VertOffset += ColourH;
					}
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					VertOffset = TileLight + MaximumX7 * ColourH;
					for (int MaxLastCounter = MaximumX7; MaxLastCounter >= FirstTileX7 - 4; MaxLastCounter--)
					{
						LightColor(ActiveColour + VertOffset, VertOffset, -ColourH);
						VertOffset -= ColourH;
					}
				}
			}
			fixed (Vector3* ActiveColour = Colour2)
			{
				int LightCol = FirstToLightX27 * ColourH;
				for (int TileLight = FirstToLightX27; TileLight < LastToLightX27; TileLight++)
				{
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					for (int MinTileLight = FirstToLightY27; MinTileLight < LastTileY7 + 4; MinTileLight++)
					{
						int VertOffset = MinTileLight + LightCol;
						LightColor(ActiveColour + VertOffset, VertOffset, 1);
					}
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					for (int MaxTileLight = LastToLightY27; MaxTileLight >= FirstTileY7 - 4; MaxTileLight--)
					{
						int VertOffset = MaxTileLight + LightCol;
						LightColor(ActiveColour + VertOffset, VertOffset, -1);
					}
					LightCol += ColourH;
				}
			}
			fixed (Vector3* ActiveColour = Colour2)
			{
				for (int TileLight = FirstToLightY27; TileLight < LastToLightY27; TileLight++)
				{
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					int VertOffset = TileLight + FirstToLightX27 * ColourH;
					for (int MinLastCounter = FirstToLightX27; MinLastCounter < LastTileX7 + 4; MinLastCounter++)
					{
						LightColor(ActiveColour + VertOffset, VertOffset, ColourH);
						VertOffset += ColourH;
					}
					LightColorR = 0f;
					LightColorG = 0f;
					LightColorB = 0f;
					VertOffset = TileLight + LastToLightX27 * ColourH;
					for (int MaxLastCounter = LastToLightX27; MaxLastCounter >= FirstTileX7 - 4; MaxLastCounter--)
					{
						LightColor(ActiveColour + VertOffset, VertOffset, -ColourH);
						VertOffset -= ColourH;
					}
				}
			}
		}

		public unsafe static void AddLight(int LightX, int LightY, Vector3 RGB)
		{
			if (TempLightCount == MaxTempLights || !WorldView.AnyViewContains(LightX << 4, LightY << 4))
			{
				return;
			}
			fixed (TempLight* ActiveTempLights = TempLightSet)
			{
				int NumLights = TempLightCount - 1;
				TempLight* ActiveLight = ActiveTempLights + NumLights;
				while (NumLights >= 0)
				{
					if (ActiveLight->X == LightX && ActiveLight->Y == LightY)
					{
						if (ActiveLight->ColorRGB.X < RGB.X)
						{
							ActiveLight->ColorRGB.X = RGB.X;
						}
						if (ActiveLight->ColorRGB.Y < RGB.Y)
						{
							ActiveLight->ColorRGB.Y = RGB.Y;
						}
						if (ActiveLight->ColorRGB.Z < RGB.Z)
						{
							ActiveLight->ColorRGB.Z = RGB.Z;
						}
						return;
					}
					ActiveLight--;
					NumLights--;
				}
				ActiveLight = ActiveTempLights + TempLightCount++;
				ActiveLight->X = (short)LightX;
				ActiveLight->Y = (short)LightY;
				ActiveLight->ColorRGB = RGB;
			}
		}

		private unsafe void LightColor(Vector3* RGB, int NoLight, int LightIdx)
		{
			NoLight = StopAndWetLight[NoLight];
			float LightValue = LightColorR;
			if (RGB->X > LightValue)
			{
				LightValue = RGB->X;
			}
			if (LightValue > 0.0185f)
			{
				RGB->X = LightValue;
				if (RGB[LightIdx].X <= LightValue)
				{
					LightValue = ((NoLight == 0) ? (LightValue * NegLight) : (((NoLight & 1) == 0) ? (LightValue * (WetLightR * Main.Rand.Next(98, 100) * 0.01f)) : (LightValue * NegLight2)));
				}
				LightColorR = LightValue;
			}
			LightValue = LightColorG;
			if (RGB->Y > LightValue)
			{
				LightValue = RGB->Y;
			}
			if (LightValue > 0.0185f)
			{
				RGB->Y = LightValue;
				if (RGB[LightIdx].Y <= LightValue)
				{
					LightValue = ((NoLight == 0) ? (LightValue * NegLight) : (((NoLight & 1) == 0) ? (LightValue * (WetLightG * Main.Rand.Next(97, 100) * 0.01f)) : (LightValue * NegLight2)));
				}
				LightColorG = LightValue;
			}
			LightValue = LightColorB;
			if (RGB->Z > LightValue)
			{
				LightValue = RGB->Z;
			}
			if (LightValue > 0.0185f)
			{
				RGB->Z = LightValue;
				if (RGB[LightIdx].Z < LightValue)
				{
					LightValue = (((NoLight & 1) == 0) ? (LightValue * NegLight) : (LightValue * NegLight2));
				}
				LightColorB = LightValue;
			}
		}

		public Color GetColorPlayer(int X, int Y, Color OldColor)
		{
			int LightX = X - FirstToLightX;
			int LightY = Y - FirstToLightY;
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return Color.Black;
			}
			float VisibilityOffset = Colour[LightX, LightY].X * 2.5f;
			if (VisibilityOffset > 1f)
			{
				VisibilityOffset = 1f;
			}
			int R = (int)(OldColor.R * VisibilityOffset * BrightnessLevel);
			VisibilityOffset = Colour[LightX, LightY].Y * 2.5f;
			if (VisibilityOffset > 1f)
			{
				VisibilityOffset = 1f;
			}
			int G = (int)(OldColor.G * VisibilityOffset * BrightnessLevel);
			VisibilityOffset = Colour[LightX, LightY].Z * 2.5f;
			if (VisibilityOffset > 1f)
			{
				VisibilityOffset = 1f;
			}
			int B = (int)(OldColor.B * VisibilityOffset * BrightnessLevel);
			return new Color(R, G, B, 255);
		}

		public Color GetColorPlayer(int X, int Y)
		{
			int LightX = X - FirstToLightX;
			int LightY = Y - FirstToLightY;
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return Color.Black;
			}
			return new Color(Colour[LightX, LightY] * (BrightnessLevel * 2.5f));
		}

		public Color GetColorUnsafe(int X, int Y)
		{
			return new Color(Colour[X - FirstToLightX, Y - FirstToLightY] * BrightnessLevel);
		}

		public Color GetColor(int X, int Y)
		{
			int LightX = X - FirstToLightX;
			int LightY = Y - FirstToLightY;
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return Color.Black;
			}
			return new Color(Colour[LightX, LightY] * BrightnessLevel);
		}

		public unsafe float Brightness(int X, int Y)
		{
			int LightX = X - FirstToLightX;
			int LightY = Y - FirstToLightY;
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return 0f;
			}
			fixed (Vector3* ActiveLight = &Colour[LightX, LightY])
			{
				return (ActiveLight->X + ActiveLight->Y + ActiveLight->Z) * (1f / 3f);
			}
		}

		public unsafe float BrightnessUnsafe(int X, int Y)
		{
			fixed (Vector3* ActiveLight = &Colour[X - FirstToLightX, Y - FirstToLightY])
			{
				return (ActiveLight->X + ActiveLight->Y + ActiveLight->Z) * (1f / 3f);
			}
		}

		public unsafe bool IsNotBlackUnsafe(int X, int Y)
		{
			fixed (Vector3* ActiveLight = &Colour[X - FirstToLightX, Y - FirstToLightY])
			{
				return ActiveLight->X > 0f || ActiveLight->Y > 0f || ActiveLight->Z > 0f;
			}
		}

		public unsafe bool Brighter(int X, int Y, int X2, int Y2)
		{
			int LightX = X - FirstToLightX;
			int LightY = Y - FirstToLightY;
#if VERSION_INITIAL && !IS_PATCHED
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return true;
			}
			int ColourX = X2 - FirstToLightX;
			int ColourY = Y2 - FirstToLightY;
			if (ColourX < 0 || ColourY < 0 || ColourX >= MaxLightArrayX || ColourY >= MaxLightArrayY)
			{
				return false;
			}
			fixed (Vector3* ActiveLight = &Colour[LightX, LightY])
			{
				fixed (Vector3* ActiveColour = &Colour2[ColourX, ColourY])
				{
					return ActiveLight->X + ActiveLight->Y + ActiveLight->Z <= ActiveColour->X + ActiveColour->Y + ActiveColour->Z;
				}
			}
#else
			if (LightX < 0 || LightY < 0 || LightX >= MaxLightArrayX || LightY >= MaxLightArrayY)
			{
				return false;
			}
			int ColourX = X2 - FirstToLightX;
			int ColourY = Y2 - FirstToLightY;
			if (ColourX < 0 || ColourY < 0 || ColourX >= MaxLightArrayX || ColourY >= MaxLightArrayY)
			{
				return true;
			}
			fixed (Vector3* ActiveLight = &Colour[LightX, LightY])
			{
				fixed (Vector3* ActiveColour = &Colour2[ColourX, ColourY])
				{
					return ActiveLight->X + ActiveLight->Y + ActiveLight->Z >= ActiveColour->X + ActiveColour->Y + ActiveColour->Z;
				}
			}
#endif
		}
	}
}