namespace Terraria
{
	public struct Liquid
	{
		public const int MaxNumLiquids = 4096;

		public static int SkipCount = 0;

		public static int StuckCount = 0;

		public static int StuckAmount = 0;

		private static int Cycles = 7;

		public static int NumLiquids;

		public static bool IsStuck = false;

		public static bool QuickFall = false;

		private static bool QuickSettle = false;

		private static int WetCounter;

		public static int PanicCounter = 0;

		public static bool PanicMode = false;

		public static int PanicY = 0;

		public short X;

		public short Y;

		public short Kill;

		public short Delay;

		public void Init(int NewX, int NewY)
		{
			X = (short)NewX;
			Y = (short)NewY;
			Kill = 0;
			Delay = 0;
		}

		public static void QuickSettleOn()
		{
			QuickSettle = true;
			Cycles = 1;
		}

		public static void QuickSettleOff()
		{
			QuickSettle = false;
			Cycles = 14;
		}

		public static void QuickWater(double ProgressionRate, int MinY = 3, int MaxY = -1, double StartPercent = 0.0)
		{
			if (MaxY == -1)
			{
				MaxY = Main.MaxTilesY - 3;
			}
			for (int Y = MaxY; Y >= MinY; Y--)
			{
				UI.MainUI.Progress = (float)((MaxY - Y) / (double)(MaxY - MinY + 1) * ProgressionRate + StartPercent);
				for (int LiquidType = 0; LiquidType < Main.MaxNumLiquidTypes; LiquidType++)
				{
					int StartPtr = Main.MaxNumLiquidTypes;
					int Endpoint = Main.MaxTilesX - 2;
					int Increment = 1;
					if (LiquidType == 1)
					{
						StartPtr = Endpoint;
						Endpoint = 2;
						Increment = -1;
					}
					for (int BaseX = StartPtr; BaseX != Endpoint; BaseX += Increment)
					{
						int LiquidLevel = Main.TileSet[BaseX, Y].Liquid;
						if (LiquidLevel <= 0)
						{
							continue;
						}
						Main.TileSet[BaseX, Y].Liquid = 0;
						int XOffset = -Increment;
						bool NoLiquid = false;
						int TileX = BaseX;
						int TileY = Y;
						int LavaLevel = Main.TileSet[BaseX, Y].Lava;
						bool NoTouch = true;
						int XAdjust = 0;
						while (NoTouch && TileX > 3 && TileX < Main.MaxTilesX - 3 && TileY < Main.MaxTilesY - 3)
						{
							NoTouch = false;
							while (Main.TileSet[TileX, TileY + 1].Liquid == 0 && TileY < Main.MaxTilesY - 5 && (Main.TileSet[TileX, TileY + 1].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[TileX, TileY + 1].Type]))
							{
								NoTouch = true;
								NoLiquid = true;
								XOffset = Increment;
								XAdjust = 0;
								TileY++;
								if (TileY > WorldGen.WaterLine)
								{
									LavaLevel = 32;
								}
							}
							if (Main.TileSet[TileX, TileY + 1].Liquid > 0 && Main.TileSet[TileX, TileY + 1].Liquid < byte.MaxValue && Main.TileSet[TileX, TileY + 1].Lava == LavaLevel)
							{
								int LiquidRem = 255 - Main.TileSet[TileX, TileY + 1].Liquid;
								if (LiquidRem > LiquidLevel)
								{
									LiquidRem = LiquidLevel;
								}
								Main.TileSet[TileX, TileY + 1].Liquid += (byte)LiquidRem;
								LiquidLevel -= (byte)LiquidRem;
								if (LiquidLevel <= 0)
								{
									break;
								}
							}
							if (XAdjust == 0)
							{
								if (Main.TileSet[TileX + XOffset, TileY].Liquid == 0 && (Main.TileSet[TileX + XOffset, TileY].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[TileX + XOffset, TileY].Type]))
								{
									XAdjust = XOffset;
								}
								else if (Main.TileSet[TileX - XOffset, TileY].Liquid == 0 && (Main.TileSet[TileX - XOffset, TileY].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[TileX - XOffset, TileY].Type]))
								{
									XAdjust = -XOffset;
								}
							}
							if (XAdjust != 0 && Main.TileSet[TileX + XAdjust, TileY].Liquid == 0 && (Main.TileSet[TileX + XAdjust, TileY].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[TileX + XAdjust, TileY].Type]))
							{
								NoTouch = true;
								TileX += XAdjust;
							}
							if (NoLiquid && !NoTouch)
							{
								NoLiquid = false;
								NoTouch = true;
								XOffset = -Increment;
								XAdjust = 0;
							}
						}
						Main.TileSet[TileX, TileY].Liquid = (byte)LiquidLevel;
						Main.TileSet[TileX, TileY].Lava = (byte)LavaLevel;
						if (Main.TileSet[TileX - 1, TileY].Liquid > 0 && Main.TileSet[TileX - 1, TileY].Lava != LavaLevel)
						{
							if (LavaLevel != 0)
							{
								LavaCheck(TileX, TileY);
							}
							else
							{
								LavaCheck(TileX - 1, TileY);
							}
						}
						else if (Main.TileSet[TileX + 1, TileY].Liquid > 0 && Main.TileSet[TileX + 1, TileY].Lava != LavaLevel)
						{
							if (LavaLevel != 0)
							{
								LavaCheck(TileX, TileY);
							}
							else
							{
								LavaCheck(TileX + 1, TileY);
							}
						}
						else if (Main.TileSet[TileX, TileY - 1].Liquid > 0 && Main.TileSet[TileX, TileY - 1].Lava != LavaLevel)
						{
							if (LavaLevel != 0)
							{
								LavaCheck(TileX, TileY);
							}
							else
							{
								LavaCheck(TileX, TileY - 1);
							}
						}
						else if (Main.TileSet[TileX, TileY + 1].Liquid > 0 && Main.TileSet[TileX, TileY + 1].Lava != LavaLevel)
						{
							if (LavaLevel != 0)
							{
								LavaCheck(TileX, TileY);
							}
							else
							{
								LavaCheck(TileX, TileY + 1);
							}
						}
					}
				}
			}
		}

		public unsafe void Update()
		{
			fixed (Tile* ActiveTile = &Main.TileSet[X, Y])
			{
				if (ActiveTile->IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile->Type])
				{
					Kill = 9;
					return;
				}
				int LiquidLevel = ActiveTile->Liquid;
				int Liquid = LiquidLevel;
				int LiquidRem = 0;
				if (Y > Main.MaxTilesY - 200 && LiquidLevel > 0 && ActiveTile->Lava == 0)
				{
					int LowerLimit = 2;
					if (LiquidLevel < LowerLimit)
					{
						LowerLimit = LiquidLevel;
					}
					LiquidLevel -= LowerLimit;
					ActiveTile->Liquid = (byte)LiquidLevel;
				}
				if (LiquidLevel == 0)
				{
					Kill = 9;
					return;
				}
				if (ActiveTile->Lava != 0)
				{
					LavaCheck(X, Y);
					if (!QuickFall)
					{
						if (Delay < 5)
						{
							Delay++;
							return;
						}
						Delay = 0;
					}
				}
				else
				{
					if (ActiveTile[-(Main.LargeWorldH)].Lava != 0)
					{
						AddWater(X - 1, Y);
					}
					if (ActiveTile[Main.LargeWorldH].Lava != 0)
					{
						AddWater(X + 1, Y);
					}
					if (ActiveTile[-1].Lava != 0)
					{
						AddWater(X, Y - 1);
					}
					if (ActiveTile[1].Lava != 0)
					{
						AddWater(X, Y + 1);
					}
				}
				if (ActiveTile[1].IsActive == 0 || !Main.TileSolidNotSolidTop[ActiveTile[1].Type])
				{
					LiquidLevel = ActiveTile[1].Liquid;
					if ((LiquidLevel <= 0 || ActiveTile[1].Lava == ActiveTile->Lava) && LiquidLevel < 255)
					{
						LiquidRem = 255 - LiquidLevel;
						if (LiquidRem > ActiveTile->Liquid)
						{
							LiquidRem = ActiveTile->Liquid;
						}
						ActiveTile->Liquid -= (byte)LiquidRem;
						LiquidLevel += LiquidRem;
						ActiveTile[1].Liquid = (byte)LiquidLevel;
						ActiveTile[1].Lava = ActiveTile->Lava;
						AddWater(X, Y + 1);
						ActiveTile[1].SkipLiquid = 128;
						ActiveTile->SkipLiquid = 128;
						if (ActiveTile->Liquid > 250)
						{
							ActiveTile->Liquid = byte.MaxValue;
						}
						else
						{
							AddWater(X - 1, Y);
							AddWater(X + 1, Y);
						}
					}
				}
				if (ActiveTile->Liquid > 0)
				{
					bool IsActiveEntryM1 = true;
					bool IsActiveEntryP1 = true;
					bool IsActiveEntryM2 = true;
					bool IsActiveEntryP2 = true;
					if (ActiveTile[-(Main.LargeWorldH)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[-(Main.LargeWorldH)].Type])
					{
						IsActiveEntryM1 = false;
					}
					else if (ActiveTile[-(Main.LargeWorldH)].Liquid > 0 && ActiveTile[-(Main.LargeWorldH)].Lava != ActiveTile->Lava)
					{
						IsActiveEntryM1 = false;
					}
					else if (ActiveTile[-(Main.LargeWorldH * 2)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[-(Main.LargeWorldH * 2)].Type])
					{
						IsActiveEntryM2 = false;
					}
					else if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid == 0)
					{
						IsActiveEntryM2 = false;
					}
					else if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid > 0 && ActiveTile[-(Main.LargeWorldH * 2)].Lava != ActiveTile->Lava)
					{
						IsActiveEntryM2 = false;
					}
					if (ActiveTile[(Main.LargeWorldH)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[(Main.LargeWorldH)].Type])
					{
						IsActiveEntryP1 = false;
					}
					else if (ActiveTile[(Main.LargeWorldH)].Liquid > 0 && ActiveTile[(Main.LargeWorldH)].Lava != ActiveTile->Lava)
					{
						IsActiveEntryP1 = false;
					}
					else if (ActiveTile[(Main.LargeWorldH * 2)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[(Main.LargeWorldH * 2)].Type])
					{
						IsActiveEntryP2 = false;
					}
					else if (ActiveTile[(Main.LargeWorldH * 2)].Liquid == 0)
					{
						IsActiveEntryP2 = false;
					}
					else if (ActiveTile[(Main.LargeWorldH * 2)].Liquid > 0 && ActiveTile[(Main.LargeWorldH * 2)].Lava != ActiveTile->Lava)
					{
						IsActiveEntryP2 = false;
					}
					int Min = 0;
					if (ActiveTile->Liquid < 3)
					{
						Min = -1;
					}
					if (IsActiveEntryM1 && IsActiveEntryP1)
					{
						if (IsActiveEntryM2 && IsActiveEntryP2)
						{
							bool IsActiveEntryM3 = true;
							bool IsActiveEntryP3 = true;
							if (ActiveTile[-(Main.LargeWorldH * 3)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[-(Main.LargeWorldH * 3)].Type])
							{
								IsActiveEntryM3 = false;
							}
							else if (ActiveTile[-(Main.LargeWorldH * 3)].Liquid == 0)
							{
								IsActiveEntryM3 = false;
							}
							else if (ActiveTile[-(Main.LargeWorldH * 3)].Lava != ActiveTile->Lava)
							{
								IsActiveEntryM3 = false;
							}
							if (ActiveTile[(Main.LargeWorldH * 3)].IsActive != 0 && Main.TileSolidNotSolidTop[ActiveTile[(Main.LargeWorldH * 3)].Type])
							{
								IsActiveEntryP3 = false;
							}
							else if (ActiveTile[(Main.LargeWorldH * 3)].Liquid == 0)
							{
								IsActiveEntryP3 = false;
							}
							else if (ActiveTile[(Main.LargeWorldH * 3)].Lava != ActiveTile->Lava)
							{
								IsActiveEntryP3 = false;
							}
							if (IsActiveEntryM3 && IsActiveEntryP3)
							{
								LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile[-(Main.LargeWorldH * 2)].Liquid + ActiveTile[(Main.LargeWorldH * 2)].Liquid + ActiveTile[-(Main.LargeWorldH * 3)].Liquid + ActiveTile[(Main.LargeWorldH * 3)].Liquid + ActiveTile->Liquid + Min;
								LiquidRem = (LiquidRem + 3) / 7;
								int WaterCheck = 0;
								ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
								if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X - 1, Y);
									ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
								if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X + 1, Y);
									ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[-(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
								if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X - 2, Y);
									ActiveTile[-(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
								if (ActiveTile[(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X + 2, Y);
									ActiveTile[(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[-(Main.LargeWorldH * 3)].Lava = ActiveTile->Lava;
								if (ActiveTile[-(Main.LargeWorldH * 3)].Liquid != LiquidRem)
								{
									AddWater(X - 3, Y);
									ActiveTile[-(Main.LargeWorldH * 3)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[(Main.LargeWorldH * 3)].Lava = ActiveTile->Lava;
								if (ActiveTile[(Main.LargeWorldH * 3)].Liquid != LiquidRem)
								{
									AddWater(X + 3, Y);
									ActiveTile[(Main.LargeWorldH * 3)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X - 1, Y);
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X + 1, Y);
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[-(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X - 2, Y);
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X + 2, Y);
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[-(Main.LargeWorldH * 3)].Liquid != LiquidRem)
								{
									AddWater(X - 3, Y);
								}
								if (ActiveTile->Liquid != LiquidRem || ActiveTile[(Main.LargeWorldH * 3)].Liquid != LiquidRem)
								{
									AddWater(X + 3, Y);
								}
								if (WaterCheck != 6 || ActiveTile[-1].Liquid <= 0)
								{
									ActiveTile->Liquid = (byte)LiquidRem;
								}
							}
							else
							{
								int WaterCheck = 0;
								LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile[-(Main.LargeWorldH * 2)].Liquid + ActiveTile[(Main.LargeWorldH * 2)].Liquid + ActiveTile->Liquid + Min;
								LiquidRem = (LiquidRem + 2) / 5;
								ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
								if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X - 1, Y);
									ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
								if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
								{
									AddWater(X + 1, Y);
									ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[-(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
								if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X - 2, Y);
									ActiveTile[-(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								ActiveTile[(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
								if (ActiveTile[(Main.LargeWorldH * 2)].Liquid != LiquidRem)
								{
									AddWater(X + 2, Y);
									ActiveTile[(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								}
								else
								{
									WaterCheck++;
								}
								if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
								{
									AddWater(X - 1, Y);
								}
								if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
								{
									AddWater(X + 1, Y);
								}
								if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
								{
									AddWater(X - 2, Y);
								}
								if (ActiveTile[(Main.LargeWorldH * 2)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
								{
									AddWater(X + 2, Y);
								}
								if (WaterCheck != 4 || ActiveTile[-1].Liquid <= 0)
								{
									ActiveTile->Liquid = (byte)LiquidRem;
								}
							}
						}
						else if (IsActiveEntryM2)
						{
							LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile[-(Main.LargeWorldH * 2)].Liquid + ActiveTile->Liquid + Min;
							LiquidRem = LiquidRem + 2 >> 2;
							ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								AddWater(X - 1, Y);
								ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								AddWater(X + 1, Y);
								ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							ActiveTile[-(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
							if (ActiveTile[-(Main.LargeWorldH * 2)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								ActiveTile[-(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								AddWater(X - 2, Y);
							}
							ActiveTile->Liquid = (byte)LiquidRem;
						}
						else if (IsActiveEntryP2)
						{
							LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH * 2)].Liquid + ActiveTile->Liquid + Min;
							LiquidRem = LiquidRem + 2 >> 2;
							ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								AddWater(X - 1, Y);
								ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								AddWater(X + 1, Y);
								ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							ActiveTile[(Main.LargeWorldH * 2)].Lava = ActiveTile->Lava;
							if (ActiveTile[(Main.LargeWorldH * 2)].Liquid != LiquidRem || ActiveTile->Liquid != LiquidRem)
							{
								ActiveTile[(Main.LargeWorldH * 2)].Liquid = (byte)LiquidRem;
								AddWater(X + 2, Y);
							}
							ActiveTile->Liquid = (byte)LiquidRem;
						}
						else
						{
							LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile->Liquid + Min;
							LiquidRem = (LiquidRem + 1) / 3;
							ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
							{
								ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							if (ActiveTile->Liquid != LiquidRem || ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
							{
								AddWater(X - 1, Y);
							}
							ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
							if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
							{
								ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
							}
							if (ActiveTile->Liquid != LiquidRem || ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
							{
								AddWater(X + 1, Y);
							}
							ActiveTile->Liquid = (byte)LiquidRem;
						}
					}
					else if (IsActiveEntryM1)
					{
						LiquidRem = ActiveTile[-(Main.LargeWorldH)].Liquid + ActiveTile->Liquid + Min;
						LiquidRem = LiquidRem + 1 >> 1;
						if (ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
						{
							ActiveTile[-(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
						}
						ActiveTile[-(Main.LargeWorldH)].Lava = ActiveTile->Lava;
						if (ActiveTile->Liquid != LiquidRem || ActiveTile[-(Main.LargeWorldH)].Liquid != LiquidRem)
						{
							AddWater(X - 1, Y);
						}
						ActiveTile->Liquid = (byte)LiquidRem;
					}
					else if (IsActiveEntryP1)
					{
						LiquidRem = ActiveTile[(Main.LargeWorldH)].Liquid + ActiveTile->Liquid + Min;
						LiquidRem = LiquidRem + 1 >> 1;
						if (ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
						{
							ActiveTile[(Main.LargeWorldH)].Liquid = (byte)LiquidRem;
						}
						ActiveTile[(Main.LargeWorldH)].Lava = ActiveTile->Lava;
						if (ActiveTile->Liquid != LiquidRem || ActiveTile[(Main.LargeWorldH)].Liquid != LiquidRem)
						{
							AddWater(X + 1, Y);
						}
						ActiveTile->Liquid = (byte)LiquidRem;
					}
				}
				if (ActiveTile->Liquid != Liquid)
				{
					if (ActiveTile->Liquid == 254 && Liquid == 255)
					{
						ActiveTile->Liquid = 255;
						Kill++;
					}
					else
					{
						AddWater(X, Y - 1);
						Kill = 0;
					}
				}
				else
				{
					Kill++;
				}
			}
		}

		public static void StartPanic()
		{
			if (!PanicMode)
			{
				WorldGen.WaterLine = Main.MaxTilesY;
				NumLiquids = 0;
				LiquidBuffer.NumLiquidBuffer = 0;
				PanicCounter = 0;
				PanicMode = true;
				PanicY = Main.MaxTilesY - 3;
			}
		}

		public static void UpdateLiquid()
		{
			if (!WorldGen.Gen)
			{
				if (!PanicMode)
				{
					if (NumLiquids + LiquidBuffer.NumLiquidBuffer > 4000)
					{
						PanicCounter++;
						if (PanicCounter > 1800 || NumLiquids + LiquidBuffer.NumLiquidBuffer > 13500)
						{
							StartPanic();
						}
					}
					else
					{
						PanicCounter = 0;
					}
				}
				if (PanicMode)
				{
					int SettleTick = 0;
					while (PanicY >= 3 && SettleTick < 5)
					{
						SettleTick++;
						QuickWater(0.0, PanicY, PanicY);
						PanicY--;
						if (PanicY < 3)
						{
							PanicCounter = 0;
							PanicMode = false;
							WorldGen.WaterCheck();
							if (Main.NetMode == (byte)NetModeSetting.SERVER)
							{
								Netplay.ResetSections();
							}
						}
					}
					return;
				}
			}
			QuickFall = QuickSettle || NumLiquids > 2000;
			int CycleSets = MaxNumLiquids / Cycles;
			int CycleCounter = CycleSets * WetCounter;
			int NextCycleCounter = CycleSets * ++WetCounter;
			if (WetCounter == Cycles)
			{
				NextCycleCounter = NumLiquids;
			}
			if (NextCycleCounter > NumLiquids)
			{
				NextCycleCounter = NumLiquids;
				WetCounter = Cycles;
			}
			if (QuickFall)
			{
				for (int LiquidIdx = CycleCounter; LiquidIdx < NextCycleCounter; LiquidIdx++)
				{
					Main.Liquid[LiquidIdx].Delay = 10;
					Main.Liquid[LiquidIdx].Update();
					Main.TileSet[Main.Liquid[LiquidIdx].X, Main.Liquid[LiquidIdx].Y].SkipLiquid = 0;
				}
			}
			else
			{
				for (int LiquidIdx = CycleCounter; LiquidIdx < NextCycleCounter; LiquidIdx++)
				{
					if (Main.TileSet[Main.Liquid[LiquidIdx].X, Main.Liquid[LiquidIdx].Y].SkipLiquid == 0)
					{
						Main.Liquid[LiquidIdx].Update();
					}
					else
					{
						Main.TileSet[Main.Liquid[LiquidIdx].X, Main.Liquid[LiquidIdx].Y].SkipLiquid = 0;
					}
				}
			}
			if (WetCounter < Cycles)
			{
				return;
			}
			WetCounter = 0;
			for (int LiquidIdx = NumLiquids - 1; LiquidIdx >= 0; LiquidIdx--)
			{
				if (Main.Liquid[LiquidIdx].Kill > 3)
				{
					DelWater(LiquidIdx);
				}
			}
			int UpperLimit = MaxNumLiquids - (MaxNumLiquids - NumLiquids);
			int BufferIdx = LiquidBuffer.NumLiquidBuffer;
			if (UpperLimit > BufferIdx)
			{
				UpperLimit = BufferIdx;
			}
			BufferIdx = (LiquidBuffer.NumLiquidBuffer = BufferIdx - UpperLimit);
			while (--UpperLimit >= 0)
			{
				Main.TileSet[Main.LiquidBuffer[BufferIdx].X, Main.LiquidBuffer[BufferIdx].Y].CheckingLiquid = 0;
				AddWater(Main.LiquidBuffer[BufferIdx].X, Main.LiquidBuffer[BufferIdx].Y);
				BufferIdx++;
			}
			if (NumLiquids > 0 && NumLiquids > StuckAmount - 50 && NumLiquids < StuckAmount + 50)
			{
				if (++StuckCount >= 10000)
				{
					IsStuck = true;
					for (int LiquidIdx = NumLiquids - 1; LiquidIdx >= 0; LiquidIdx--)
					{
						DelWater(LiquidIdx);
					}
					IsStuck = false;
					StuckCount = 0;
				}
			}
			else
			{
				StuckCount = 0;
				StuckAmount = NumLiquids;
			}
		}

		public unsafe static void AddWater(int WaterX, int WaterY)
		{
			if (WaterX < 5 || WaterY < 5 || WaterX >= Main.MaxTilesX - 5 || WaterY >= Main.MaxTilesY - 5)
			{
				return;
			}
			fixed (Tile* CurrentTile = &Main.TileSet[WaterX, WaterY])
			{
				if (CurrentTile->Liquid == 0 || CurrentTile->CheckingLiquid != 0)
				{
					return;
				}
				if (NumLiquids >= MaxNumLiquids - 1)
				{
					LiquidBuffer.AddBuffer(WaterX, WaterY);
					return;
				}
				CurrentTile->CheckingLiquid = 64;
				Main.Liquid[NumLiquids].Init(WaterX, WaterY);
				CurrentTile->SkipLiquid = 0;
				NumLiquids++;
				if (Main.NetMode == (byte)NetModeSetting.SERVER && NumLiquids < 1365)
				{
					NetMessage.SendWater(WaterX, WaterY);
				}
				if (CurrentTile->IsActive == 0)
				{
					return;
				}
				int TileID = CurrentTile->Type;
				if ((TileID != 4 || CurrentTile->FrameY != 176) && (Main.TileWaterDeath[TileID] || (CurrentTile->Lava != 0 && Main.TileLavaDeath[TileID])))
				{
					if (WorldGen.Gen)
					{
						CurrentTile->IsActive = 0;
					}
					else if (WorldGen.KillTile(WaterX, WaterY) && Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, WaterX, WaterY, 0);
						NetMessage.SendMessage();
					}
				}
			}
		}

		public static void LavaCheck(int TileX, int TileY)
		{
			int LiquidLevelAbove = Main.TileSet[TileX, TileY - 1].Liquid;
			bool NoLavaAbove = Main.TileSet[TileX, TileY - 1].Lava == 0;
			int LiquidLevelLeft = Main.TileSet[TileX - 1, TileY].Liquid;
			bool NoLavaLeft = Main.TileSet[TileX - 1, TileY].Lava == 0;
			int LiquidLevelRight = Main.TileSet[TileX + 1, TileY].Liquid;
			bool NoLavaRight = Main.TileSet[TileX + 1, TileY].Lava == 0;
			if ((LiquidLevelLeft > 0 && NoLavaLeft) || (LiquidLevelRight > 0 && NoLavaRight) || (LiquidLevelAbove > 0 && NoLavaAbove))
			{
				int LiquidLevel = 0;
				if (NoLavaLeft)
				{
					LiquidLevel = LiquidLevelLeft;
					Main.TileSet[TileX - 1, TileY].Liquid = 0;
				}
				if (NoLavaRight)
				{
					LiquidLevel += LiquidLevelRight;
					Main.TileSet[TileX + 1, TileY].Liquid = 0;
				}
				if (NoLavaAbove)
				{
					LiquidLevel += LiquidLevelAbove;
					Main.TileSet[TileX, TileY - 1].Liquid = 0;
				}
				if (LiquidLevel >= 32 && Main.TileSet[TileX, TileY].IsActive == 0)
				{
					Main.TileSet[TileX, TileY].Liquid = 0;
					Main.TileSet[TileX, TileY].Lava = 0;
					WorldGen.PlaceTile(TileX, TileY, 56, ToMute: true, IsForced: true);
					WorldGen.SquareTileFrame(TileX, TileY);
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.SendTileSquare(TileX - 1, TileY - 1, 3);
					}
				}
			}
			else if (Main.TileSet[TileX, TileY + 1].IsActive == 0 && Main.TileSet[TileX, TileY + 1].Liquid > 0 && Main.TileSet[TileX, TileY + 1].Lava == 0)
			{
				Main.TileSet[TileX, TileY].Liquid = 0;
				Main.TileSet[TileX, TileY].Lava = 0;
				Main.TileSet[TileX, TileY + 1].Liquid = 0;
				WorldGen.PlaceTile(TileX, TileY + 1, 56, ToMute: true, IsForced: true);
				WorldGen.SquareTileFrame(TileX, TileY + 1);
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.SendTileSquare(TileX - 1, TileY, 3);
				}
			}
		}

		public unsafe static void DelWater(int LiquidIdx)
		{
			int LiquidX = Main.Liquid[LiquidIdx].X;
			int LiquidY = Main.Liquid[LiquidIdx].Y;
			fixed (Tile* ActiveTile = &Main.TileSet[LiquidX, LiquidY])
			{
				int LiquidLevel = ActiveTile->Liquid;
				if (LiquidLevel < 2)
				{
					ActiveTile->Liquid = 0;
					LiquidLevel = 0;
					if (ActiveTile[-(Main.LargeWorldH)].Liquid == 1)
					{
						ActiveTile[-(Main.LargeWorldH)].Liquid = 0;
					}
					if (ActiveTile[(Main.LargeWorldH)].Liquid == 1)
					{
						ActiveTile[(Main.LargeWorldH)].Liquid = 0;
					}
				}
				else if (LiquidLevel < 20)
				{
					if ((ActiveTile[1].Liquid < 255 && (ActiveTile[1].IsActive == 0 || !Main.TileSolidNotSolidTop[ActiveTile[1].Type])) || (ActiveTile[-(Main.LargeWorldH)].Liquid < LiquidLevel && (ActiveTile[-(Main.LargeWorldH)].IsActive == 0 || !Main.TileSolidNotSolidTop[ActiveTile[-(Main.LargeWorldH)].Type])) || (ActiveTile[(Main.LargeWorldH)].Liquid < LiquidLevel && (ActiveTile[(Main.LargeWorldH)].IsActive == 0 || !Main.TileSolidNotSolidTop[ActiveTile[(Main.LargeWorldH)].Type])))
					{
						ActiveTile->Liquid = 0;
						LiquidLevel = 0;
					}
				}
				else if (ActiveTile[1].Liquid < 255 && (ActiveTile[1].IsActive == 0 || !Main.TileSolidNotSolidTop[ActiveTile[1].Type]) && !IsStuck)
				{
					Main.Liquid[LiquidIdx].Kill = 0;
					return;
				}
				if (LiquidLevel < 250 && ActiveTile[-1].Liquid > 0)
				{
					AddWater(LiquidX, LiquidY - 1);
				}
				if (LiquidLevel == 0)
				{
					ActiveTile->Lava = 0;
				}
				else
				{
					if ((ActiveTile[(Main.LargeWorldH)].Liquid > 0 && ActiveTile[(Main.LargeWorldH + 1)].Liquid < 250 && ActiveTile[(Main.LargeWorldH + 1)].IsActive == 0) || (ActiveTile[-(Main.LargeWorldH)].Liquid > 0 && ActiveTile[-(Main.LargeWorldH - 1)].Liquid < 250 && ActiveTile[-(Main.LargeWorldH - 1)].IsActive == 0))
					{
						AddWater(LiquidX - 1, LiquidY);
						AddWater(LiquidX + 1, LiquidY);
					}
					if (ActiveTile->Lava != 0)
					{
						LavaCheck(LiquidX, LiquidY);
						for (int TileX = LiquidX - 1; TileX <= LiquidX + 1; TileX++)
						{
							for (int TileY = LiquidY - 1; TileY <= LiquidY + 1; TileY++)
							{
								if (Main.TileSet[TileX, TileY].IsActive == 0)
								{
									continue;
								}
								switch (Main.TileSet[TileX, TileY].Type)
								{
								case 2:
								case 23:
								case 109:
									Main.TileSet[TileX, TileY].Type = 0;
									WorldGen.SquareTileFrame(TileX, TileY);
									if (Main.NetMode == (byte)NetModeSetting.SERVER)
									{
										NetMessage.SendTileSquare(LiquidX, LiquidY, 3);
									}
									break;
								case 60:
								case 70:
									Main.TileSet[TileX, TileY].Type = 59;
									WorldGen.SquareTileFrame(TileX, TileY);
									if (Main.NetMode == (byte)NetModeSetting.SERVER)
									{
										NetMessage.SendTileSquare(LiquidX, LiquidY, 3);
									}
									break;
								}
							}
						}
					}
				}
				if (Main.NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.SendWater(LiquidX, LiquidY);
				}
				NumLiquids--;
				ActiveTile->CheckingLiquid = 0;
				Main.Liquid[LiquidIdx].X = Main.Liquid[NumLiquids].X;
				Main.Liquid[LiquidIdx].Y = Main.Liquid[NumLiquids].Y;
				Main.Liquid[LiquidIdx].Kill = Main.Liquid[NumLiquids].Kill;
				if (ActiveTile->Type >= 82 && ActiveTile->Type <= 84)
				{
					WorldGen.CheckAlch(LiquidX, LiquidY);
				}
			}
		}
	}
}
