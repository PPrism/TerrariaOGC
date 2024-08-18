namespace Terraria
{
	public sealed class Chest
	{
		public const int MaxNumItems = 20;

		public Item[] ItemSet = new Item[MaxNumItems];

		public short XPos;

		public short YPos;

		public Chest()
		{
		}

		public Chest(int X, int Y)
		{
			XPos = (short)X;
			YPos = (short)Y;
			for (int Idx = 0; Idx < MaxNumItems; Idx++)
			{
				ItemSet[Idx].Init();
			}
		}

		public unsafe static void Unlock(int X, int Y)
		{
			Main.PlaySound(22, X * 16, Y * 16);
			for (int TileX = X; TileX <= X + 1; TileX++)
			{
				for (int TileY = Y; TileY <= Y + 1; TileY++)
				{
					if ((Main.TileSet[TileX, TileY].FrameX < 72 || Main.TileSet[TileX, TileY].FrameX > 106) && (Main.TileSet[TileX, TileY].FrameX < 144 || Main.TileSet[TileX, TileY].FrameX > 178))
					{
						continue;
					}
					Main.TileSet[TileX, TileY].FrameX -= 36;
					for (int i = 0; i < 3; i++)
					{
						if (null == Main.DustSet.NewDust(TileX * 16, TileY * 16, 16, 16, 11))
						{
							break;
						}
					}
				}
			}
		}

		public static int UsingChest(int SelectedChest)
		{
			if (Main.ChestSet[SelectedChest] != null)
			{
				for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
				{
					if (Main.PlayerSet[PlayerIdx].Active != 0 && Main.PlayerSet[PlayerIdx].PlayerChest == SelectedChest)
					{
						return PlayerIdx;
					}
				}
			}
			return -1;
		}

		public static int FindChest(int X, int Y)
		{
			for (int ChestIdx = 0; ChestIdx < Main.MaxNumChests; ChestIdx++)
			{
				if (Main.ChestSet[ChestIdx] != null && Main.ChestSet[ChestIdx].XPos == X && Main.ChestSet[ChestIdx].YPos == Y)
				{
					return ChestIdx;
				}
			}
			return -1;
		}

		public static int CreateChest(int X, int Y)
		{
			for (int ChestIdx = 0; ChestIdx < Main.MaxNumChests; ChestIdx++)
			{
				if (Main.ChestSet[ChestIdx] != null && Main.ChestSet[ChestIdx].XPos == X && Main.ChestSet[ChestIdx].YPos == Y)
				{
					return -1;
				}
			}
			for (int NewChestIdx = 0; NewChestIdx < Main.MaxNumChests; NewChestIdx++)
			{
				if (Main.ChestSet[NewChestIdx] == null)
				{
					Main.ChestSet[NewChestIdx] = new Chest(X, Y);
					return NewChestIdx;
				}
			}
			return -1;
		}

		public static bool DestroyChest(int X, int Y)
		{
			for (int ChestIdx = 0; ChestIdx < Main.MaxNumChests; ChestIdx++)
			{
				if (Main.ChestSet[ChestIdx] == null || Main.ChestSet[ChestIdx].XPos != X || Main.ChestSet[ChestIdx].YPos != Y)
				{
					continue;
				}
				for (int j = 0; j < MaxNumItems; j++)
				{
					if (Main.ChestSet[ChestIdx].ItemSet[j].Type > 0 && Main.ChestSet[ChestIdx].ItemSet[j].Stack > 0)
					{
						return false;
					}
				}
				Main.ChestSet[ChestIdx] = null;
				break;
			}
			return true;
		}

		public void AddShop(ref Item NewItem)
		{
			for (int ItemIdx = 0; ItemIdx < MaxNumItems - 1; ItemIdx++)
			{
				if (ItemSet[ItemIdx].Type != 0)
				{
					continue;
				}
				ref Item RefItem = ref ItemSet[ItemIdx];
				RefItem = NewItem;
				ItemSet[ItemIdx].OnlyBuyOnce = true;
				if (ItemSet[ItemIdx].Value > 0)
				{
					ItemSet[ItemIdx].Value = ItemSet[ItemIdx].Value / 5;
					if (ItemSet[ItemIdx].Value < 1)
					{
						ItemSet[ItemIdx].Value = 1;
					}
				}
				break;
			}
		}

		public static int GetShopOwnerHeadTextureId(int NPCVendor)
		{
			switch (NPCVendor)
			{
			case 1:
				return (int)_sheetSprites.ID.NPC_HEAD_2;
			case 2:
				return (int)_sheetSprites.ID.NPC_HEAD_6;
			case 3:
				return (int)_sheetSprites.ID.NPC_HEAD_5;
			case 4:
				return (int)_sheetSprites.ID.NPC_HEAD_4;
			case 5:
				return (int)_sheetSprites.ID.NPC_HEAD_7;
			case 6:
				return (int)_sheetSprites.ID.NPC_HEAD_9;
			case 7:
				return (int)_sheetSprites.ID.NPC_HEAD_10;
			case 8:
				return (int)_sheetSprites.ID.NPC_HEAD_8;
			case 9:
				return (int)_sheetSprites.ID.NPC_HEAD_11;
			default:
				return -1;
			}
		}

		public void SetupShop(int Type, Player CurrentPlayer = null)
		{
			int ItemIdx = MaxNumItems;
			while (ItemIdx > 0)
			{
				ItemSet[--ItemIdx].Init();
			}
			switch (Type)
			{
			case 1:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.MINING_HELMET);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.PIGGY_BANK);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.IRON_ANVIL);
				ItemSet[ItemIdx++].NetDefaults(-13); // Copper Pickaxe
				ItemSet[ItemIdx++].NetDefaults(-16); // Copper Axe
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.TORCH);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.LESSER_HEALING_POTION);
				if (CurrentPlayer != null && CurrentPlayer.statManaMax >= 200)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.LESSER_MANA_POTION);
				}
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.WOODEN_ARROW);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SHURIKEN);
				if (Main.GameTime.IsBloodMoon)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.THROWING_KNIFE);
				}
				if (!Main.GameTime.DayTime)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GLOWSTICK);
				}
				if (NPC.HasDownedBoss3)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SAFE);
				}
				if (Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.DISCO_BALL);
				}
				break;
			case 2:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.MUSKET_BALL);
				if (Main.GameTime.IsBloodMoon || Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SILVER_BULLET);
				}
				if ((NPC.HasDownedBoss2 && !Main.GameTime.DayTime) || Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.UNHOLY_ARROW);
				}
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FLINTLOCK_PISTOL);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.MINISHARK);
				if (!Main.GameTime.DayTime)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.ILLEGAL_GUN_PARTS);
				}
				if (Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SHOTGUN);
				}
				break;
			case 3:
				if (Main.GameTime.IsBloodMoon)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.VILE_POWDER);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.CORRUPT_SEEDS);
				}
				else
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.PURIFICATION_POWDER);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GRASS_SEEDS);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SUNFLOWER);
				}
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.ACORN);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.DIRT_ROD);
				if (Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.HALLOWED_SEEDS);
				}
#if !VERSION_INITIAL
				if (Main.InHardMode && Main.GameTime.IsBloodMoon)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SPARKLY_WINGS);
				}
#endif
				break;
			case 4:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GRENADE);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.BOMB);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.DYNAMITE);
				if (Main.InHardMode)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.HELLFIRE_ARROW);
				}
				break;
			case 5:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.BLACK_DYE);
				if (Main.GameTime.DayTime)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SUMMER_HAT);
				}
				if (Main.GameTime.MoonPhase == 0)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.PLUMBERS_SHIRT);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.PLUMBERS_PANTS);
				}
				else if (Main.GameTime.MoonPhase == 1)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.THE_DOCTORS_SHIRT);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.THE_DOCTORS_PANTS);
				}
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FAMILIAR_SHIRT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FAMILIAR_PANTS);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FAMILIAR_WIG);
				if (NPC.HasDownedClown)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.CLOWN_HAT);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.CLOWN_SHIRT);
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.CLOWN_PANTS);
				}
				if (Main.GameTime.IsBloodMoon)
				{
					ItemSet[ItemIdx++].SetDefaults((int)Item.ID.MIME_MASK);
				}
#if !VERSION_INITIAL
				if (Main.GameTime.IsBloodMoon)
				{
					if (CurrentPlayer.male)
					{
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GEORGES_HAT);
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GEORGES_SUIT);
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GEORGES_PANTS);
					}
					else
					{
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FABULOUS_RIBBON);
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FABULOUS_DRESS);
						ItemSet[ItemIdx++].SetDefaults((int)Item.ID.FABULOUS_SLIPPERS);
					}
				}
#endif
				break;
			case 6:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.ROCKET_BOOTS);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.RULER);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.TINKERERS_WORKSHOP);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GRAPPLING_HOOK);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.TOOLBELT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SPIKY_BALL);
				break;
			case 7:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.CRYSTAL_BALL);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.ICE_ROD);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GREATER_MANA_POTION);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.BELL);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.HARP);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SPELL_TOME);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.MUSIC_BOX);
				break;
			case 8:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.WRENCH);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.WIRE_CUTTER);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.WIRE);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.LEVER);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SWITCH);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.RED_PRESSURE_PLATE);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GREEN_PRESSURE_PLATE);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GRAY_PRESSURE_PLATE);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.BROWN_PRESSURE_PLATE);
				break;
			case 9:
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SANTA_HAT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SANTA_SHIRT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.SANTA_PANTS);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.RED_LIGHT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.GREEN_LIGHT);
				ItemSet[ItemIdx++].SetDefaults((int)Item.ID.BLUE_LIGHT);
				break;
			}
		}

		private void ConvertCoins(int ID)
		{
			for (int ItemIdx = 0; ItemIdx < MaxNumItems; ItemIdx++)
			{
#if !VERSION_INITIAL
				if (ItemSet[ItemIdx].Stack != ItemSet[ItemIdx].MaxStack || ItemSet[ItemIdx].Type < (int)Item.ID.COPPER_COIN || ItemSet[ItemIdx].Type > (int)Item.ID.GOLD_COIN)
#else
				// BUG: This checks if the coins need conversion (have a stack of 100) and if they are actually coins. This is so the rest of the code can upgrade those coins to the next tier.
				// The problem is that !Item.CanBePlacedInCoinSlot() will also allow for platinum coins to pass, and since the next ID is a fallen star, any stacks over 100 convert to a fallen star.
				// 1.01 fixed this by limiting the restriction to the first 3 coin tiers, meaning platinums do not get converted.
				if (ItemSet[ItemIdx].Stack != ItemSet[ItemIdx].MaxStack || !ItemSet[ItemIdx].CanBePlacedInCoinSlot())
#endif
				{
					continue;
				}
				ItemSet[ItemIdx].SetDefaults(ItemSet[ItemIdx].Type + 1);
				for (int ItemIdx2 = 0; ItemIdx2 < MaxNumItems; ItemIdx2++)
				{
					if (ItemIdx2 != ItemIdx && ItemSet[ItemIdx2].Type == ItemSet[ItemIdx].Type && ItemSet[ItemIdx2].Stack < ItemSet[ItemIdx2].MaxStack)
					{
						if (ID >= 0)
						{
							NetMessage.CreateMessage2(32, ID, ItemIdx2);
							NetMessage.SendMessage();
						}
						ItemSet[ItemIdx2].Stack++;
						ItemSet[ItemIdx].Init();
						ConvertCoins(ID);
					}
				}
			}
		}

		public void LootAll(Player CurrentPlayer)
		{
			int Chest = CurrentPlayer.PlayerChest;
			for (int i = 0; i < MaxNumItems; i++)
			{
				if (ItemSet[i].Type > 0)
				{
					CurrentPlayer.GetItem(ref ItemSet[i]);
					if (Chest >= 0)
					{
						NetMessage.CreateMessage2(32, Chest, i);
						NetMessage.SendMessage();
					}
				}
			}
		}

		public void Deposit(Player CurrentPlayer)
		{
			int Chest = CurrentPlayer.PlayerChest;
			for (int InvItem = Player.NumInvSlots; InvItem >= 10; InvItem--)
			{
				if (CurrentPlayer.Inventory[InvItem].Stack > 0 && CurrentPlayer.Inventory[InvItem].Type > 0)
				{
					if (CurrentPlayer.Inventory[InvItem].MaxStack > 1)
					{
						for (int ItemIdx = 0; ItemIdx < MaxNumItems; ItemIdx++)
						{
							if (ItemSet[ItemIdx].Stack >= ItemSet[ItemIdx].MaxStack || CurrentPlayer.Inventory[InvItem].NetID != ItemSet[ItemIdx].NetID)
							{
								continue;
							}
							short Stack = CurrentPlayer.Inventory[InvItem].Stack;
							if (CurrentPlayer.Inventory[InvItem].Stack + ItemSet[ItemIdx].Stack > ItemSet[ItemIdx].MaxStack)
							{
								Stack = (short)(ItemSet[ItemIdx].MaxStack - ItemSet[ItemIdx].Stack);
							}
							CurrentPlayer.Inventory[InvItem].Stack -= Stack;
							ItemSet[ItemIdx].Stack += Stack;
							ConvertCoins(Chest);
							Main.PlaySound(7);
							if (CurrentPlayer.Inventory[InvItem].Stack <= 0)
							{
								CurrentPlayer.Inventory[InvItem].Init();
								if (Chest >= 0)
								{
									NetMessage.CreateMessage2(32, Chest, ItemIdx);
									NetMessage.SendMessage();
								}
								break;
							}
							if (ItemSet[ItemIdx].Type == 0)
							{
								ref Item reference = ref ItemSet[ItemIdx];
								reference = CurrentPlayer.Inventory[InvItem];
								CurrentPlayer.Inventory[InvItem].Init();
							}
							if (Chest >= 0)
							{
								NetMessage.CreateMessage2(32, Chest, ItemIdx);
								NetMessage.SendMessage();
							}
						}
					}
					if (CurrentPlayer.Inventory[InvItem].Stack > 0)
					{
						for (int InvItem2 = 0; InvItem2 < MaxNumItems; InvItem2++)
						{
							if (ItemSet[InvItem2].Type == 0)
							{
								Main.PlaySound(7);
								ref Item RefItem = ref ItemSet[InvItem2];
								RefItem = CurrentPlayer.Inventory[InvItem2];
								CurrentPlayer.Inventory[InvItem2].Init();
								if (Chest >= 0)
								{
									NetMessage.CreateMessage2(32, Chest, InvItem2);
									NetMessage.SendMessage();
								}
								break;
							}
						}
					}
				}
			}
		}

		public void QuickStack(Player CurrentPlayer)
		{
			int Chest = CurrentPlayer.PlayerChest;
			for (int ItemIdx = 0; ItemIdx < MaxNumItems; ItemIdx++)
			{
				if (ItemSet[ItemIdx].Type <= 0 || ItemSet[ItemIdx].Stack >= ItemSet[ItemIdx].MaxStack)
				{
					continue;
				}
				for (int InvItem = 0; InvItem < Player.MaxNumInventory; InvItem++)
				{
					if (ItemSet[ItemIdx].NetID == CurrentPlayer.Inventory[InvItem].NetID)
					{
						short InvStack = CurrentPlayer.Inventory[InvItem].Stack;
						if (ItemSet[ItemIdx].Stack + InvStack > ItemSet[ItemIdx].MaxStack)
						{
							InvStack = (short)(ItemSet[ItemIdx].MaxStack - ItemSet[ItemIdx].Stack);
						}
						Main.PlaySound(7);
						ItemSet[ItemIdx].Stack += InvStack;
						CurrentPlayer.Inventory[InvItem].Stack -= InvStack;
						ConvertCoins(Chest);
						if (CurrentPlayer.Inventory[InvItem].Stack == 0)
						{
							CurrentPlayer.Inventory[InvItem].Init();
						}
						else if (ItemSet[ItemIdx].Type == 0)
						{
							ref Item RefItem = ref ItemSet[ItemIdx];
							RefItem = CurrentPlayer.Inventory[InvItem];
							CurrentPlayer.Inventory[InvItem].Init();
						}
						if (Chest >= 0)
						{
							NetMessage.CreateMessage2(32, Chest, ItemIdx);
							NetMessage.SendMessage();
						}
					}
				}
			}
		}
	}
}
