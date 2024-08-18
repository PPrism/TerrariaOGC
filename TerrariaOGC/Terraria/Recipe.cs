using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria
{
	public sealed class Recipe
	{
		public enum Category : byte
		{
			STRUCTURES,
			TOOLS,
			WEAPONS,
			ARMOR,
			POTIONS,
			MISC,
			NUM_CATEGORIES
		}

		public enum SubCategory : byte
		{
			NONE = 0,
			TORCHES = 1,
			WALLS = 2,
			BRICKS = 3,
			LANTERNS = 4,
			CHANDELIERS = 5,
			CHESTS = 6,
			CRAFTING_STATIONS = 7,
			ANVILS = 8,
			STATUES = 9,
			MECHANISM = 10,
			TIMERS = 11,
			PUMPS = 12,
			TABLEWARE = 0,
			PICKAXES = 1,
			DRILLS = 2,
			AXES = 3,
			CHAINSAWS = 4,
			HAMMERS = 5,
			HAMAXES = 6,
			BARS = 7,
			GRAPPLING = 8,
			SWORDS = 1,
			BROADSWORDS = 2,
			SHORTSWORDS = 3,
			FLAILS = 4,
			BOOMERANGS = 5,
			BOWS = 6,
			ARROWS = 7,
			GUNS = 8,
			MAGIC_GUNS = 9,
			SPELLS = 10,
			REPEATERS = 11,
			BULLETS = 12,
			PHASEBLADES = 13,
			PHASESABERS = 14,
			SPEARS = 15,
			EXPLOSIVES = 16,
			HEADGEAR = 1,
			HELMETS = 2,
			MASKS = 3,
			HOODS = 4,
			HATS = 5,
			GOGGLES = 6,
			BODY = 7,
			SHIRTS = 8,
			LEGS = 9,
			PANTS = 10,
			WINGS = 11,
			BOOTS = 0,
			HEALING = 1,
			MANA = 2,
			RESTORATION = 3,
			BANNERS = 1,
			WATCHES = 2,
			MUSICBOXES = 3,
			EYES = 4,
			SKULLS = 5
		}

		public sealed class SubCategoryList
		{
			public bool CanCraft;

			public List<short> RecipeList;

			public SubCategoryList(int ReserveIdx)
			{
				CanCraft = false;
				RecipeList = new List<short>(ReserveIdx);
			}

			public void Add(Player CurPlayer, int RecipeIdx)
			{
				RecipeList.Add((short)RecipeIdx);
				if (!CanCraft)
				{
					CanCraft = CurPlayer.CanCraftRecipe(Main.ActiveRecipe[RecipeIdx]);
				}
			}
		}

		public const int MaxItemRequirements = 12;

		public const int MaxTileRequirements = 3;

#if VERSION_INITIAL
		public const int MaxNumRecipes = 342;
#elif VERSION_101
		public const int MaxNumRecipes = 358;
#else

#endif

		public static Recipe NewRecipe = new Recipe();

		public Item CraftedItem = default;

		public Item[] RequiredItem = new Item[MaxItemRequirements];

		public short[] RequiredTile = new short[MaxTileRequirements];

		public byte NumRequiredItems;

		public byte NumRequiredTiles;

		public bool NeedsWater;

		public Category RecipeCategory;

		public SubCategory RecipeSubCategory;

		public static int NumRecipes = 0;

		public static Dictionary<int, SubCategoryList> CurrentRecipes = new Dictionary<int, SubCategoryList>();

		public Recipe()
		{
			for (int ItemSlot = 0; ItemSlot < MaxItemRequirements; ItemSlot++)
			{
				RequiredItem[ItemSlot].Init();
			}
			for (int TileSlot = 0; TileSlot < MaxTileRequirements; TileSlot++)
			{
				RequiredTile[TileSlot] = -1;
			}
		}

		public void Create(UI ActiveUI)
		{
			for (int ItemSlot = 0; ItemSlot < NumRequiredItems; ItemSlot++)
			{
				int ItemCount = RequiredItem[ItemSlot].Stack;
				for (int InvIdx = 0; InvIdx < Player.MaxNumInventory; InvIdx++)
				{
					if (ActiveUI.ActivePlayer.Inventory[InvIdx].NetID == RequiredItem[ItemSlot].NetID)
					{
						if (ActiveUI.ActivePlayer.Inventory[InvIdx].Stack > ItemCount)
						{
							ActiveUI.ActivePlayer.Inventory[InvIdx].Stack -= (short)ItemCount;
							ItemCount = 0;
						}
						else
						{
							ItemCount -= ActiveUI.ActivePlayer.Inventory[InvIdx].Stack;
							ActiveUI.ActivePlayer.Inventory[InvIdx].Init();
						}
					}
					if (ItemCount <= 0)
					{
						break;
					}
				}
			}
			if ((RequiredTile[0] == 16 || RequiredTile[0] == 134) && ++ActiveUI.TotalAnvilCrafting == (int)Achievement.AnvilItemsGoal)
			{
				ActiveUI.AchievementTriggers.SetState(Trigger.UsedLotsOfAnvils, State: true);
			}
			switch (RecipeCategory)
			{
			case Category.STRUCTURES:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.FurnitureCrafted);
				break;
			case Category.TOOLS:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.ToolsCrafted);
				break;
			case Category.WEAPONS:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.WeaponsCrafted);
				break;
			case Category.ARMOR:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.ArmorCrafted);
				break;
			case Category.POTIONS:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.ConsumablesCrafted);
				break;
			default:
				ActiveUI.Statistics.IncreaseStat(StatisticEntry.MiscCrafted);
				break;
			}
			switch ((Item.ID)CraftedItem.Type)
			{
			case Item.ID.TORCH:
			case Item.ID.BLUE_TORCH:
			case Item.ID.RED_TORCH:
			case Item.ID.GREEN_TORCH:
			case Item.ID.PURPLE_TORCH:
			case Item.ID.WHITE_TORCH:
			case Item.ID.YELLOW_TORCH:
			case Item.ID.DEMON_TORCH:
				ActiveUI.TotalTorchesCrafted += (uint)CraftedItem.Stack;
				break;
			case Item.ID.WOOD_PLATFORM:
				ActiveUI.TotalWoodPlatformsCrafted += (uint)CraftedItem.Stack;
				break;
			case Item.ID.STONE_WALL:
			case Item.ID.DIRT_WALL:
			case Item.ID.WOOD_WALL:
			case Item.ID.PLANKED_WALL:
				ActiveUI.TotalWallsCrafted += (uint)CraftedItem.Stack;
				break;
			case Item.ID.GOLD_BAR:
			case Item.ID.COPPER_BAR:
			case Item.ID.SILVER_BAR:
			case Item.ID.IRON_BAR:
			case Item.ID.DEMONITE_BAR:
			case Item.ID.METEORITE_BAR:
			case Item.ID.HELLSTONE_BAR:
			case Item.ID.COBALT_BAR:
			case Item.ID.MYTHRIL_BAR:
			case Item.ID.ADAMANTITE_BAR:
				ActiveUI.TotalBarsCrafted += (uint)CraftedItem.Stack;
				if (ActiveUI.TotalBarsCrafted >= (int)Achievement.SmelterBarsGoal)
				{
					ActiveUI.ActivePlayer.AchievementTrigger(Trigger.CreatedLotsOfBars);
				}
				break;
			}
		}

		public static void FindRecipes(UI ActiveUI, Category RecipeCategory, bool IsCraftable)
		{
			int Counter = 0;
			int Spacer = 1024;
			ActiveUI.ActivePlayer.UpdateRecipes();
			for (int RecipeIdx = 0; RecipeIdx < MaxNumRecipes; RecipeIdx++)
			{
				Recipe PotentialRecipe = Main.ActiveRecipe[RecipeIdx];
				if (RecipeCategory != PotentialRecipe.RecipeCategory)
				{
					continue;
				}
				if (IsCraftable)
				{
					if (!ActiveUI.ActivePlayer.CanCraftRecipe(PotentialRecipe))
					{
						continue;
					}
				}
				else if (ActiveUI.CraftGuide)
				{
					bool IsMatched = false;
					for (int ItemSlot = PotentialRecipe.NumRequiredItems - 1; ItemSlot >= 0; ItemSlot--)
					{
						if (PotentialRecipe.RequiredItem[ItemSlot].NetID == ActiveUI.GuideItem.NetID)
						{
							IsMatched = true;
							break;
						}
					}
					if (!IsMatched)
					{
						continue;
					}
				}
#if USE_ORIGINAL_CODE
				else if (!ActiveUI.ActivePlayer.RecipesFound[RecipeIdx])
				{
					continue;
				}
#else
				else if (!Main.UnlockAllRecipes)
				{
					if (!ActiveUI.ActivePlayer.RecipesFound[RecipeIdx])
					{
						continue;
					}

				}
#endif
				int ItemSubCategory = (int)PotentialRecipe.RecipeSubCategory;
				if (ItemSubCategory == 0)
				{
					Counter++;
					ItemSubCategory = Spacer++;
					CurrentRecipes.Add(ItemSubCategory, new SubCategoryList(1));
				}
				else if (!CurrentRecipes.ContainsKey(ItemSubCategory))
				{
					Counter++;
					CurrentRecipes.Add(ItemSubCategory, new SubCategoryList(32));
				}
				CurrentRecipes[ItemSubCategory].Add(ActiveUI.ActivePlayer, RecipeIdx);
			}
			Dictionary<int, SubCategoryList>.ValueCollection PassedRecipes = CurrentRecipes.Values;
			ActiveUI.CurrentRecipeCategory.Clear();
			foreach (SubCategoryList ItemType in PassedRecipes)
			{
				if (ItemType.CanCraft)
				{
					ActiveUI.CurrentRecipeCategory.Add(ItemType);
				}
			}
			foreach (SubCategoryList ItemType in PassedRecipes)
			{
				if (!ItemType.CanCraft)
				{
					ActiveUI.CurrentRecipeCategory.Add(ItemType);
				}
			}
			int Count = CurrentRecipes.Count;
			if (Count == 0)
			{
				ActiveUI.CraftingRecipe = NewRecipe;
				ActiveUI.CraftingRecipeX = 0;
				ActiveUI.CraftingRecipeY = 0;
			}
			else
			{
				if (ActiveUI.CraftingRecipeY >= Count)
				{
					ActiveUI.CraftingRecipeY = (sbyte)(Count - 1);
				}
				int CategoryCount = ActiveUI.CurrentRecipeCategory[ActiveUI.CraftingRecipeY].RecipeList.Count;
				if (ActiveUI.CraftingRecipeX >= CategoryCount)
				{
					ActiveUI.CraftingRecipeX = (sbyte)(CategoryCount - 1);
				}
			}
			CurrentRecipes.Clear();
		}

		public static void SetupRecipes()
		{
			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WORK_BENCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 10);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GEL);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUG);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOWL_OF_SOUP);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOWL);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MUSHROOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.GOLDFISH);
			NewRecipe.RequiredTile[0] = 96;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOTTLE, 2);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLE);
			NewRecipe.NeedsWater = true;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HOLY_WATER, 5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PIXIE_DUST, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.HALLOWED_SEEDS);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.UNHOLY_WATER, 5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.EBONSAND_BLOCK);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.CORRUPT_SEEDS);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ALE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUG);
			NewRecipe.RequiredTile[0] = 94;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LESSER_HEALING_POTION, 2);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSHROOM);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GEL, 2);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BOTTLE, 2);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HEALING_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LESSER_HEALING_POTION, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GLOWING_MUSHROOM);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREATER_HEALING_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PIXIE_DUST, 3);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.CRYSTAL_SHARD);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LESSER_MANA_POTION, 2);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FALLEN_STAR);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GEL, 2);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BOTTLE, 2);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MANA_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LESSER_MANA_POTION, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GLOWING_MUSHROOM);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LESSER_RESTORATION_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LESSER_HEALING_POTION);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.LESSER_MANA_POTION);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RESTORATION_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HEALING_POTION);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MANA_POTION);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_SKIN_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.FIREBLOSSOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.WATERLEAF);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.OBSIDIAN);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.REGENERATION_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MUSHROOM);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SWIFTNESS_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.CACTUS);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GILLS_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WATERLEAF);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.CORAL);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRONSKIN_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_ORE);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MANA_REGENERATION_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MOONGLOW);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.FALLEN_STAR);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MAGIC_POWER_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MOONGLOW);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.DEATHWEED);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.FALLEN_STAR);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FEATHERFALL_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.FEATHER);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPELUNKER_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MOONGLOW);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.GOLD_ORE);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.INVISIBILITY_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MOONGLOW);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SHINE_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.GLOWING_MUSHROOM);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NIGHT_OWL_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BATTLE_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DEATHWEED);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ROTTEN_CHUNK);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.THORNS_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DEATHWEED);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.CACTUS);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.WORM_TOOTH);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.STINGER);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WATER_WALKING_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WATERLEAF);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SHARK_FIN);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ARCHERY_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.LENS);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HUNTER_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DAYBLOOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SHARK_FIN);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GRAVITATION_POTION);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.FIREBLOSSOM);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.DEATHWEED);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.BLINKROOT);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.FEATHER);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_ARROW, 5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FLAMING_ARROW, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOODEN_ARROW, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.UNHOLY_ARROW, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOODEN_ARROW, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WORM_TOOTH);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HELLFIRE_ARROW, 10);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOODEN_ARROW, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.HELLSTONE_BAR);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.TORCH, 5);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CURSED_ARROW, 15);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOODEN_ARROW, 15);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CURSED_FLAME);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HOLY_ARROW, 35);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOODEN_ARROW, 35);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PIXIE_DUST, 6);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.UNICORN_HORN);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PUMPS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEOR_SHOT, 25);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSKET_BALL, 25);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.METEORITE_BAR);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PUMPS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CRYSTAL_BULLET, 25);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSKET_BALL, 25);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PUMPS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CURSED_BULLET, 25);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSKET_BALL, 25);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CURSED_FLAME);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PURPLE_DYE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOTTLE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MUSHROOM_GRASS_SEEDS, 3);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.VILE_POWDER, 5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.VILE_MUSHROOM);
			NewRecipe.RequiredTile[0] = 13;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.POISONED_KNIFE, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.THROWING_KNIFE, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.VILE_POWDER);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BLUE_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SAPPHIRE);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.RUBY);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREEN_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.EMERALD);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.YELLOW_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TOPAZ);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PURPLE_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.AMETHYST);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WHITE_TORCH, 3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DIAMOND);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CURSED_TORCH, 33);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH, 33);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CURSED_FLAME);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CHINESE_LANTERN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 5);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 1);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TIKI_TORCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TORCH);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LAMP_POST);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GLASS, 2);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.TORCH);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SKULL_LANTERN, 1);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.EXPLOSIVES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.STICKY_BOMB);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BOMB);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GEL, 5);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.STICKY_GLOWSTICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLOWSTICK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GEL);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GLASS, 1);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SAND_BLOCK, 2);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GLASS_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CLAY_POT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CLAY_BLOCK, 6);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PINK_VASE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CLAY_BLOCK, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOWL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CLAY_BLOCK, 2);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GRAY_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 2);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GRAY_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GRAY_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CLAY_BLOCK, 2);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.RED_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COPPER_ORE, 1);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SILVER_ORE, 1);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GOLD_ORE, 1);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HELLSTONE_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.OBSIDIAN, 2);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.OBSIDIAN_BRICK);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SNOW_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SNOW_BLOCK, 2);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SNOW_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SNOW_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CANDY_CANE_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CANDY_CANE_BLOCK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREEN_CANDY_CANE_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GREEN_CANDY_CANE_BLOCK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PEARLSTONE_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.PEARLSAND_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PEARLSTONE_BLOCK);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PEARLSTONE_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.PEARLSTONE_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRIDESCENT_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ASH_BLOCK);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRIDESCENT_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRIDESCENT_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUDSTONE_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MUD_BLOCK);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUDSTONE_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUDSTONE_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_ORE);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_BRICK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_ORE);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_BRICK_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BRICK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DEMONITE_BRICK, 5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_ORE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.EBONSTONE_BLOCK, 5);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUD_BLOCK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DIRT_BLOCK);
			NewRecipe.NeedsWater = true;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DIRT_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DIRT_BLOCK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.STONE_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOOD_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOOD_PLATFORM);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_DOOR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 6);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_CHAIR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 4);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SIGN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 6);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CHEST);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 8);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 2);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_TABLE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 8);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PLANKED_WALL, 4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 1);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_BEAM, 2);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 1);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MANNEQUIN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 20);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BED);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 15);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SILK, 5);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOOKCASE, 1);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BOOK, 10);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BARREL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 9);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 1);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GRANDFATHER_CLOCK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GLASS, 6);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.WOOD, 10);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.KEG);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 14);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PIANO);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 15);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BOOK);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LOOM);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 12);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DRESSER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 16);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BENCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 8);
			NewRecipe.RequiredTile[0] = 106;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SAWMILL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 2);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

#if VERSION_101
			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.HELMETS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOOD_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 20);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BODY;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOOD_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 30);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.LEGS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOOD_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 25);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();
#endif

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_SWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 7);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_HAMMER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 8);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WOODEN_BOW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 10);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOBLIN_BATTLE_STANDARD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.TATTERED_CLOTH, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 5);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBWEB, 10);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_BANNER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREEN_BANNER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BLUE_BANNER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.YELLOW_BANNER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HEROS_HAT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PURPLE_DYE, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HEROS_SHIRT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PURPLE_DYE, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HEROS_PANTS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PURPLE_DYE, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TUXEDO_SHIRT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BLACK_DYE, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TUXEDO_PANTS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BLACK_DYE, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ROBE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.RUBY, 3);
			NewRecipe.RequiredTile[0] = 86;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LEATHER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ROTTEN_CHUNK, 5);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ARCHAEOLOGISTS_JACKET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LEATHER, 15);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ARCHAEOLOGISTS_PANTS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LEATHER, 15);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FISH_BOWL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLDFISH, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.BOTTLED_WATER);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FURNACE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 4);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.STATUE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 100);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_ORE, 3);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.NetDefaults(-13);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 4);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-16);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 9);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.NetDefaults(-17);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.NetDefaults(-14);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-15);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.NetDefaults(-18);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_CHAINMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_WATCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 14;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COPPER_CHANDELIER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 4);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_ORE, 3);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_ANVIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.EMPTY_BUCKET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TRASH_CAN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BATHTUB);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 14);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TOILET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 6);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COOKING_POT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 2);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_PICKAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_AXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 9);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_HAMMER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_BROADSWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_SHORTSWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_BOW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_CHAINMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_ORE, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.NetDefaults(-7);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 4);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-10);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 9);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.NetDefaults(-11);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.NetDefaults(-8);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-9);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 6);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.NetDefaults(-12);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_CHAINMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_WATCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 14;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SILVER_CHANDELIER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_BAR, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 4);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_ORE, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.NetDefaults(-1);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 4);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-4);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 9);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.NetDefaults(-5);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WOOD, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.NetDefaults(-2);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.NetDefaults(-3);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.NetDefaults(-6);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 7);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_CHAINMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_WATCH);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 14;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_CROWN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.RUBY);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOLD_CHANDELIER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 4);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_CHAIN);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.THRONE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILK, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GOLD_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CANDLE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CANDELABRA, 1);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_BAR, 5);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 3);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DEMONITE_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_ORE, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DEMON_BOW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WAR_AXE_OF_THE_NIGHT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LIGHTS_BANE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SHADOW_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 15);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHADOW_SCALE, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SHADOW_SCALEMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 25);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHADOW_SCALE, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SHADOW_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHADOW_SCALE, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NIGHTMARE_PICKAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHADOW_SCALE, 6);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.THE_BREAKER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DEMONITE_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHADOW_SCALE, 5);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GRAPPLING_HOOK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_CHAIN, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.HOOK, 1);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEORITE_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BLUE_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SAPPHIRE, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.RUBY, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREEN_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.EMERALD, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PURPLE_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.AMETHYST, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WHITE_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DIAMOND, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASEBLADES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.YELLOW_PHASEBLADE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TOPAZ, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-19);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BLUE_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.RED_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-21);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GREEN_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-22);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.PURPLE_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-23);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WHITE_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.PHASESABERS;
			NewRecipe.CraftedItem.NetDefaults(-24);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.YELLOW_PHASEBLADE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 50);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEOR_HAMAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPACE_GUN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FLINTLOCK_PISTOL);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.METEORITE_BAR, 30);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.FALLEN_STAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.STAR_CANNON);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MINISHARK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.FALLEN_STAR, 5);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEOR_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEOR_SUIT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.METEOR_LEGGINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.METEORITE_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NECRO_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 40);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBWEB, 40);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NECRO_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 60);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBWEB, 50);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NECRO_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 50);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBWEB, 45);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BLADE_OF_GRASS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.JUNGLE_SPORES, 12);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.STINGER, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.THORN_CHAKRAM);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.JUNGLE_SPORES, 6);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.STINGER, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.IVY_WHIP);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GRAPPLING_HOOK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.JUNGLE_SPORES, 12);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.VINE, 3);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.JUNGLE_HAT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.EMERALD, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SAPPHIRE, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.JUNGLE_SPORES, 8);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.JUNGLE_SHIRT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.RUBY, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DIAMOND, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.JUNGLE_SPORES, 16);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.STINGER, 12);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.JUNGLE_PANTS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.AMETHYST, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TOPAZ, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.JUNGLE_SPORES, 8);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.VINE, 2);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HELLSTONE_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.OBSIDIAN, 1);
			NewRecipe.RequiredTile[0] = 77;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FLAMARANG);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 15);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ENCHANTED_BOOMERANG);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_FURY);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FIERY_GREATSWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_PICKAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_HAMAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PHOENIX_BLASTER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.HANDGUN);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 25);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 35);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MOLTEN_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HELLSTONE_BAR, 30);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NIGHTS_EDGE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LIGHTS_BANE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MURAMASA);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.BLADE_OF_GRASS);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.FIERY_GREATSWORD);
			NewRecipe.RequiredTile[0] = 26;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DAO_OF_POW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DARK_SHARD);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.LIGHT_SHARD);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 10);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_ORE, 3);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_MASK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_HAT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 20);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_LEGGINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_DRILL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 15);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_CHAINSAW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_REPEATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_SWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SPEARS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.COBALT_NAGINATA);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_ORE, 4);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_HAT);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_HOOD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_CHAINMAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 15);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_DRILL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 15);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_CHAINSAW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_REPEATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_SWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SPEARS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_HALBERD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MYTHRIL_ANVIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_BAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_ORE, 5);
			NewRecipe.RequiredTile[0] = 133;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_MASK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_HEADGEAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 24);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_LEGGINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 18);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_DRILL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 18);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_CHAINSAW);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_REPEATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_SWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SPEARS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_GLAIVE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 12);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ADAMANTITE_FORGE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ADAMANTITE_ORE, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.HELLFORGE);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_MASK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_HELMET);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_HELMET);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_HELMET);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_HEADGEAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_HAT);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_HOOD);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_HEADGEAR);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_MASK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_HAT);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_MASK);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_PLATE_MAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BREASTPLATE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_CHAINMAIL);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_BREASTPLATE);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_LEGGINGS);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_GREAVES);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_LEGGINGS);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HAMDRAX);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_DRILL);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_DRILL);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_DRILL);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.COBALT_CHAINSAW);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.MYTHRIL_CHAINSAW);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.ADAMANTITE_CHAINSAW);
			NewRecipe.RequiredItem[6].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 5);
			NewRecipe.RequiredItem[7].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 5);
			NewRecipe.RequiredItem[8].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 5);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.HALLOWED_REPEATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_REPEATER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_REPEATER);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_REPEATER);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.EXCALIBUR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_SWORD);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_SWORD);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_SWORD);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SPEARS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GUNGNIR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_NAGINATA);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_HALBERD);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_GLAIVE);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MEGASHARK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MINISHARK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ILLEGAL_GUN_PARTS);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SHARK_FIN, 5);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.LIGHT_DISC);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MYTHRIL_BAR, 10);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 5);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 5);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FLAMETHROWER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 20);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ILLEGAL_GUN_PARTS);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.CURSED_FLAME, 35);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MAGICAL_HARP);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HARP);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 25);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 15);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.GOLD_BAR, 15);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FAIRY_BELL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BELL);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.PIXIE_DUST, 80);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 15);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.GOLD_BAR, 15);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.NEPTUNES_SHELL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CORAL, 15);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHARK_FIN, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.GOLDFISH, 15);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 5);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 5);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RAINBOW_ROD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.UNICORN_HORN, 4);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.PIXIE_DUST, 60);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 10);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ANGEL_WINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FEATHER, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_FLIGHT, 25);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 30);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DEMON_WINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FEATHER, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_FLIGHT, 25);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 30);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DIVING_GEAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FLIPPER);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DIVING_HELMET);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GPS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_WATCH);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.DEPTH_METER);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.COMPASS);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_HORSESHOE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LUCKY_HORSESHOE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.OBSIDIAN_SKULL);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_SHIELD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COBALT_SHIELD);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.OBSIDIAN_SKULL);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CLOUD_IN_A_BALLOON);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CLOUD_IN_A_BOTTLE);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SHINY_RED_BALLOON);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPECTRE_BOOTS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HERMES_BOOTS);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ROCKET_BOOTS);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MANA_FLOWER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.NATURES_GIFT);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MANA_POTION);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ACTIVE_STONE_BLOCK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WIRE);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.INACTIVE_STONE_BLOCK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_WALL, 4);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WIRE);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.EXPLOSIVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.DYNAMITE, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WIRE);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.PUMPS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.INLET_PUMP);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WIRE, 2);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.PUMPS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OUTLET_PUMP);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.IRON_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.WIRE, 2);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.ONE_SECOND_TIMER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GOLD_WATCH);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.WIRE, 1);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.THREE_SECOND_TIMER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SILVER_WATCH);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.WIRE, 1);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.FIVE_SECOND_TIMER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_WATCH);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.WIRE, 1);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BOULDER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.STONE_BLOCK, 6);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUSIC_BOX_TITLE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSIC_BOX_OVERWORLD_DAY);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MUSIC_BOX_EERIE);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MUSIC_BOX_NIGHT);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.MUSIC_BOX_UNDERGROUND);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.MUSIC_BOX_BOSS1);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.MUSIC_BOX_JUNGLE);
			NewRecipe.RequiredItem[6].SetDefaults((int)Item.ID.MUSIC_BOX_CORRUPTION);
			NewRecipe.RequiredItem[7].SetDefaults((int)Item.ID.MUSIC_BOX_UNDERGROUND_CORRUPTION);
			NewRecipe.RequiredItem[8].SetDefaults((int)Item.ID.MUSIC_BOX_THE_HALLOW);
			NewRecipe.RequiredItem[9].SetDefaults((int)Item.ID.MUSIC_BOX_BOSS2);
			NewRecipe.RequiredItem[10].SetDefaults((int)Item.ID.MUSIC_BOX_UNDERGROUND_HALLOW);
			NewRecipe.RequiredItem[11].SetDefaults((int)Item.ID.MUSIC_BOX_BOSS3);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MUSIC_BOX_TUTORIAL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MUSIC_BOX_DESERT);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MUSIC_BOX_SPACE);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MUSIC_BOX_BOSS4);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.MUSIC_BOX_OCEAN);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.MUSIC_BOX_SNOW);
			NewRecipe.RequiredTile[0] = 114;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DEPTH_METER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.COPPER_BAR, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SILVER_BAR, 8);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.GOLD_BAR, 6);
			NewRecipe.RequiredTile[0] = 14;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.OBSIDIAN_SKULL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.OBSIDIAN, 20);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CRYSTAL_STORM);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SPELL_TOME);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CRYSTAL_SHARD, 30);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 20);
			NewRecipe.RequiredTile[0] = 101;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.MECHANISM;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CURSED_FLAMES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.SPELL_TOME);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.CURSED_FLAME, 30);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 20);
			NewRecipe.RequiredTile[0] = 101;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.ANVILS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SANDGUN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ILLEGAL_GUN_PARTS);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ANTLION_MANDIBLE, 10);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.TOPAZ, 5);
			NewRecipe.RequiredTile[0] = 17;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GOGGLES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LENS, 2);
			NewRecipe.RequiredTile[0] = 18;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SUNGLASSES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BLACK_LENS, 2);
			NewRecipe.RequiredTile[0] = 18;
			NewRecipe.RequiredTile[1] = 15;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.POTIONS;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MANA_CRYSTAL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FALLEN_STAR, 10);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SUSPICIOUS_LOOKING_EYE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LENS, 6);
			NewRecipe.RequiredTile[0] = 26;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.WORM_FOOD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.VILE_POWDER, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.ROTTEN_CHUNK, 15);
			NewRecipe.RequiredTile[0] = 26;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SLIME_CROWN);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GEL, 99);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.GOLD_CROWN);
			NewRecipe.RequiredTile[0] = 26;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.LANTERNS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MECHANICAL_EYE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.LENS, 3);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COPPER_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 7);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.NONE;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MECHANICAL_WORM);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.ROTTEN_CHUNK, 6);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COPPER_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 7);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MECHANICAL_SKULL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.BONE, 30);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COPPER_BAR, 5);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.IRON_BAR, 5);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 5);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 5);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.MISC;
			NewRecipe.RecipeSubCategory = SubCategory.CHANDELIERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SUSPICIOUS_LOOKING_SKULL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MECHANICAL_EYE, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.MECHANICAL_SKULL, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 20);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SOUL_OF_NIGHT, 10);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_LIGHT, 10);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BRICKS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DRAGON_MASK);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_MASK, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_HELMET, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_HELMET, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_HELMET, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DRAGON_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_PLATE_MAIL, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_BREASTPLATE, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_CHAINMAIL, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_BREASTPLATE, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.DRAGON_GREAVES);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_GREAVES, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_LEGGINGS, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_GREAVES, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_LEGGINGS, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_MIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TITAN_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_HELMET, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_MASK, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_HAT, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_MASK, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TITAN_MAIL);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_PLATE_MAIL, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_BREASTPLATE, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_CHAINMAIL, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_BREASTPLATE, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TITAN_LEGGINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_GREAVES, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_LEGGINGS, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_GREAVES, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_LEGGINGS, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_SIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPECTRAL_HEADGEAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_HEADGEAR, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_HAT, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_HOOD, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_HEADGEAR, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.CRAFTING_STATIONS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPECTRAL_ARMOR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_PLATE_MAIL, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_BREASTPLATE, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_CHAINMAIL, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_BREASTPLATE, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.STATUES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPECTRAL_SUBLIGAR);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_GREAVES, 1);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.COBALT_LEGGINGS, 1);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.MYTHRIL_GREAVES, 1);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.ADAMANTITE_LEGGINGS, 1);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.SOUL_OF_FRIGHT, 20);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TIZONA);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.EXCALIBUR, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 15);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SPEARS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.TONBOGIRI);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GUNGNIR, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 15);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.CHESTS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SHARANGA);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.MOLTEN_FURY, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.HELLSTONE_BAR, 10);
			NewRecipe.RequiredTile[0] = 16;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.TIMERS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.VULCAN_REPEATER);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.HALLOWED_REPEATER, 2);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 15);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.ADAMANTITE_BAR, 20);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

#if VERSION_101
			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.WINGS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.SPARKLY_WINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.FEATHER, 10);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SOUL_OF_FLIGHT, 25);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.SOUL_OF_BLIGHT, 30);
			NewRecipe.RequiredTile[0] = 134;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.TORCHES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CAMPFIRE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.WOOD, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TORCH, 5);
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.HELMETS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CACTUS_HELMET);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CACTUS, 20);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.BODY;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CACTUS_BREASTPLATE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CACTUS, 30);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.ARMOR;
			NewRecipe.RecipeSubCategory = SubCategory.LEGS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CACTUS_LEGGINGS);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CACTUS, 25);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.WEAPONS;
			NewRecipe.RecipeSubCategory = SubCategory.SWORDS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CACTUS_SWORD);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CACTUS, 10);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.TOOLS;
			NewRecipe.RecipeSubCategory = SubCategory.PICKAXES;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.CACTUS_PICKAXE);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.CACTUS, 15);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.PURPLE_STAINED_GLASS, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.AMETHYST);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.YELLOW_STAINED_GLASS, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.TOPAZ);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.BLUE_STAINED_GLASS, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.SAPPHIRE);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.GREEN_STAINED_GLASS, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.EMERALD);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.RED_STAINED_GLASS, 20);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 20);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.RUBY);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();

			NewRecipe.RecipeCategory = Category.STRUCTURES;
			NewRecipe.RecipeSubCategory = SubCategory.WALLS;
			NewRecipe.CraftedItem.SetDefaults((int)Item.ID.MULTICOLORED_STAINED_GLASS, 50);
			NewRecipe.RequiredItem[0].SetDefaults((int)Item.ID.GLASS_WALL, 50);
			NewRecipe.RequiredItem[1].SetDefaults((int)Item.ID.AMETHYST);
			NewRecipe.RequiredItem[2].SetDefaults((int)Item.ID.TOPAZ);
			NewRecipe.RequiredItem[3].SetDefaults((int)Item.ID.SAPPHIRE);
			NewRecipe.RequiredItem[4].SetDefaults((int)Item.ID.EMERALD);
			NewRecipe.RequiredItem[5].SetDefaults((int)Item.ID.RUBY);
			NewRecipe.RequiredTile[0] = 18;
			AddRecipe();
#endif

			for (int RecipeIdx = 0; RecipeIdx < NumRecipes; RecipeIdx++)
			{
				for (int ItemSlot = 0; ItemSlot < MaxItemRequirements && Main.ActiveRecipe[RecipeIdx].RequiredItem[ItemSlot].Type > 0; ItemSlot++)
				{
					Main.ActiveRecipe[RecipeIdx].RequiredItem[ItemSlot].CheckMaterial();
				}
				Main.ActiveRecipe[RecipeIdx].CraftedItem.CheckMaterial();
			}
		}

		private static void AddRecipe()
		{
			for (int ItemSlot = 0; ItemSlot < MaxItemRequirements && NewRecipe.RequiredItem[ItemSlot].Type > 0; ItemSlot++)
			{
				NewRecipe.NumRequiredItems++;
			}
			for (int TileSlot = 0; TileSlot < MaxTileRequirements && NewRecipe.RequiredTile[TileSlot] >= 0; TileSlot++)
			{
				NewRecipe.NumRequiredTiles++;
			}
			Main.ActiveRecipe[NumRecipes++] = NewRecipe;
			NewRecipe = new Recipe();
		}
	}
}