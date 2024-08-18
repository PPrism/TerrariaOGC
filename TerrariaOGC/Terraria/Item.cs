using Microsoft.Xna.Framework;
using System;

namespace Terraria
{
	public struct Item
	{
		public enum ID
		{
			NONE,
			IRON_PICKAXE,
			DIRT_BLOCK,
			STONE_BLOCK,
			IRON_BROADSWORD,
			MUSHROOM,
			IRON_SHORTSWORD,
			IRON_HAMMER,
			TORCH,
			WOOD,
			IRON_AXE,
			IRON_ORE,
			COPPER_ORE,
			GOLD_ORE,
			SILVER_ORE,
			COPPER_WATCH,
			SILVER_WATCH,
			GOLD_WATCH,
			DEPTH_METER,
			GOLD_BAR,
			COPPER_BAR,
			SILVER_BAR,
			IRON_BAR,
			GEL,
			WOODEN_SWORD,
			WOODEN_DOOR,
			STONE_WALL,
			ACORN,
			LESSER_HEALING_POTION,
			LIFE_CRYSTAL,
			DIRT_WALL,
			BOTTLE,
			WOODEN_TABLE,
			FURNACE,
			WOODEN_CHAIR,
			IRON_ANVIL,
			WORK_BENCH,
			GOGGLES,
			LENS,
			WOODEN_BOW,
			WOODEN_ARROW,
			FLAMING_ARROW,
			SHURIKEN,
			SUSPICIOUS_LOOKING_EYE,
			DEMON_BOW,
			WAR_AXE_OF_THE_NIGHT,
			LIGHTS_BANE,
			UNHOLY_ARROW,
			CHEST,
			BAND_OF_REGENERATION,
			MAGIC_MIRROR,
			JESTERS_ARROW,
			ANGEL_STATUE,
			CLOUD_IN_A_BOTTLE,
			HERMES_BOOTS,
			ENCHANTED_BOOMERANG,
			DEMONITE_ORE,
			DEMONITE_BAR,
			HEART,
			CORRUPT_SEEDS,
			VILE_MUSHROOM,
			EBONSTONE_BLOCK,
			GRASS_SEEDS,
			SUNFLOWER,
			VILETHORN,
			STARFURY,
			PURIFICATION_POWDER,
			VILE_POWDER,
			ROTTEN_CHUNK,
			WORM_TOOTH,
			WORM_FOOD,
			COPPER_COIN,
			SILVER_COIN,
			GOLD_COIN,
			PLATINUM_COIN,
			FALLEN_STAR,
			COPPER_GREAVES,
			IRON_GREAVES,
			SILVER_GREAVES,
			GOLD_GREAVES,
			COPPER_CHAINMAIL,
			IRON_CHAINMAIL,
			SILVER_CHAINMAIL,
			GOLD_CHAINMAIL,
			GRAPPLING_HOOK,
			IRON_CHAIN,
			SHADOW_SCALE,
			PIGGY_BANK,
			MINING_HELMET,
			COPPER_HELMET,
			IRON_HELMET,
			SILVER_HELMET,
			GOLD_HELMET,
			WOOD_WALL,
			WOOD_PLATFORM,
			FLINTLOCK_PISTOL,
			MUSKET,
			MUSKET_BALL,
			MINISHARK,
			IRON_BOW,
			SHADOW_GREAVES,
			SHADOW_SCALEMAIL,
			SHADOW_HELMET,
			NIGHTMARE_PICKAXE,
			THE_BREAKER,
			CANDLE,
			COPPER_CHANDELIER,
			SILVER_CHANDELIER,
			GOLD_CHANDELIER,
			MANA_CRYSTAL,
			LESSER_MANA_POTION,
			BAND_OF_STARPOWER,
			FLOWER_OF_FIRE,
			MAGIC_MISSILE,
			DIRT_ROD,
			ORB_OF_LIGHT,
			METEORITE,
			METEORITE_BAR,
			HOOK,
			FLAMARANG,
			MOLTEN_FURY,
			FIERY_GREATSWORD,
			MOLTEN_PICKAXE,
			METEOR_HELMET,
			METEOR_SUIT,
			METEOR_LEGGINGS,
			BOTTLED_WATER,
			SPACE_GUN,
			ROCKET_BOOTS,
			GRAY_BRICK,
			GRAY_BRICK_WALL,
			RED_BRICK,
			RED_BRICK_WALL,
			CLAY_BLOCK,
			BLUE_BRICK,
			BLUE_BRICK_WALL,
			CHAIN_LANTERN,
			GREEN_BRICK,
			GREEN_BRICK_WALL,
			PINK_BRICK,
			PINK_BRICK_WALL,
			GOLD_BRICK,
			GOLD_BRICK_WALL,
			SILVER_BRICK,
			SILVER_BRICK_WALL,
			COPPER_BRICK,
			COPPER_BRICK_WALL,
			SPIKE,
			WATER_CANDLE,
			BOOK,
			COBWEB,
			NECRO_HELMET,
			NECRO_BREASTPLATE,
			NECRO_GREAVES,
			BONE,
			MURAMASA,
			COBALT_SHIELD,
			AQUA_SCEPTER,
			LUCKY_HORSESHOE,
			SHINY_RED_BALLOON,
			HARPOON,
			SPIKY_BALL,
			BALL_O_HURT,
			BLUE_MOON,
			HANDGUN,
			WATER_BOLT,
			BOMB,
			DYNAMITE,
			GRENADE,
			SAND_BLOCK,
			GLASS,
			SIGN,
			ASH_BLOCK,
			OBSIDIAN,
			HELLSTONE,
			HELLSTONE_BAR,
			MUD_BLOCK,
			SAPPHIRE,
			RUBY,
			EMERALD,
			TOPAZ,
			AMETHYST,
			DIAMOND,
			GLOWING_MUSHROOM,
			STAR,
			IVY_WHIP,
			BREATHING_REED,
			FLIPPER,
			HEALING_POTION,
			MANA_POTION,
			BLADE_OF_GRASS,
			THORN_CHAKRAM,
			OBSIDIAN_BRICK,
			OBSIDIAN_SKULL,
			MUSHROOM_GRASS_SEEDS,
			JUNGLE_GRASS_SEEDS,
			WOODEN_HAMMER,
			STAR_CANNON,
			BLUE_PHASEBLADE,
			RED_PHASEBLADE,
			GREEN_PHASEBLADE,
			PURPLE_PHASEBLADE,
			WHITE_PHASEBLADE,
			YELLOW_PHASEBLADE,
			METEOR_HAMAXE,
			EMPTY_BUCKET,
			WATER_BUCKET,
			LAVA_BUCKET,
			JUNGLE_ROSE,
			STINGER,
			VINE,
			FERAL_CLAWS,
			ANKLET_OF_THE_WIND,
			STAFF_OF_REGROWTH,
			HELLSTONE_BRICK,
			WHOOPIE_CUSHION,
			SHACKLE,
			MOLTEN_HAMAXE,
			FLAMELASH,
			PHOENIX_BLASTER,
			SUNFURY,
			HELLFORGE,
			CLAY_POT,
			NATURES_GIFT,
			BED,
			SILK,
			LESSER_RESTORATION_POTION,
			RESTORATION_POTION,
			JUNGLE_HAT,
			JUNGLE_SHIRT,
			JUNGLE_PANTS,
			MOLTEN_HELMET,
			MOLTEN_BREASTPLATE,
			MOLTEN_GREAVES,
			METEOR_SHOT,
			STICKY_BOMB,
			BLACK_LENS,
			SUNGLASSES,
			WIZARD_HAT,
			TOP_HAT,
			TUXEDO_SHIRT,
			TUXEDO_PANTS,
			SUMMER_HAT,
			BUNNY_HOOD,
			PLUMBERS_HAT,
			PLUMBERS_SHIRT,
			PLUMBERS_PANTS,
			HEROS_HAT,
			HEROS_SHIRT,
			HEROS_PANTS,
			FISH_BOWL,
			ARCHAEOLOGISTS_HAT,
			ARCHAEOLOGISTS_JACKET,
			ARCHAEOLOGISTS_PANTS,
			BLACK_DYE,
			PURPLE_DYE,
			NINJA_HOOD,
			NINJA_SHIRT,
			NINJA_PANTS,
			LEATHER,
			RED_HAT,
			GOLDFISH,
			ROBE,
			ROBOT_HAT,
			GOLD_CROWN,
			HELLFIRE_ARROW,
			SANDGUN,
			GUIDE_VOODOO_DOLL,
			DIVING_HELMET,
			FAMILIAR_SHIRT,
			FAMILIAR_PANTS,
			FAMILIAR_WIG,
			DEMON_SCYTHE,
			NIGHTS_EDGE,
			DARK_LANCE,
			CORAL,
			CACTUS,
			TRIDENT,
			SILVER_BULLET,
			THROWING_KNIFE,
			SPEAR,
			BLOWPIPE,
			GLOWSTICK,
			SEED,
			WOODEN_BOOMERANG,
			AGLET,
			STICKY_GLOWSTICK,
			POISONED_KNIFE,
			OBSIDIAN_SKIN_POTION,
			REGENERATION_POTION,
			SWIFTNESS_POTION,
			GILLS_POTION,
			IRONSKIN_POTION,
			MANA_REGENERATION_POTION,
			MAGIC_POWER_POTION,
			FEATHERFALL_POTION,
			SPELUNKER_POTION,
			INVISIBILITY_POTION,
			SHINE_POTION,
			NIGHT_OWL_POTION,
			BATTLE_POTION,
			THORNS_POTION,
			WATER_WALKING_POTION,
			ARCHERY_POTION,
			HUNTER_POTION,
			GRAVITATION_POTION,
			GOLD_CHEST,
			DAYBLOOM_SEEDS,
			MOONGLOW_SEEDS,
			BLINKROOT_SEEDS,
			DEATHWEED_SEEDS,
			WATERLEAF_SEEDS,
			FIREBLOSSOM_SEEDS,
			DAYBLOOM,
			MOONGLOW,
			BLINKROOT,
			DEATHWEED,
			WATERLEAF,
			FIREBLOSSOM,
			SHARK_FIN,
			FEATHER,
			TOMBSTONE,
			MIME_MASK,
			ANTLION_MANDIBLE,
			ILLEGAL_GUN_PARTS,
			THE_DOCTORS_SHIRT,
			THE_DOCTORS_PANTS,
			GOLDEN_KEY,
			SHADOW_CHEST,
			SHADOW_KEY,
			OBSIDIAN_BRICK_WALL,
			JUNGLE_SPORES,
			LOOM,
			PIANO,
			DRESSER,
			BENCH,
			BATHTUB,
			RED_BANNER,
			GREEN_BANNER,
			BLUE_BANNER,
			YELLOW_BANNER,
			LAMP_POST,
			TIKI_TORCH,
			BARREL,
			CHINESE_LANTERN,
			COOKING_POT,
			SAFE,
			SKULL_LANTERN,
			TRASH_CAN,
			CANDELABRA,
			PINK_VASE,
			MUG,
			KEG,
			ALE,
			BOOKCASE,
			THRONE,
			BOWL,
			BOWL_OF_SOUP,
			TOILET,
			GRANDFATHER_CLOCK,
			STATUE,
			GOBLIN_BATTLE_STANDARD,
			TATTERED_CLOTH,
			SAWMILL,
			COBALT_ORE,
			MYTHRIL_ORE,
			ADAMANTITE_ORE,
			PWNHAMMER,
			EXCALIBUR,
			HALLOWED_SEEDS,
			EBONSAND_BLOCK,
			COBALT_HAT,
			COBALT_HELMET,
			COBALT_MASK,
			COBALT_BREASTPLATE,
			COBALT_LEGGINGS,
			MYTHRIL_HOOD,
			MYTHRIL_HELMET,
			MYTHRIL_HAT,
			MYTHRIL_CHAINMAIL,
			MYTHRIL_GREAVES,
			COBALT_BAR,
			MYTHRIL_BAR,
			COBALT_CHAINSAW,
			MYTHRIL_CHAINSAW,
			COBALT_DRILL,
			MYTHRIL_DRILL,
			ADAMANTITE_CHAINSAW,
			ADAMANTITE_DRILL,
			DAO_OF_POW,
			MYTHRIL_HALBERD,
			ADAMANTITE_BAR,
			GLASS_WALL,
			COMPASS,
			DIVING_GEAR,
			GPS,
			OBSIDIAN_HORSESHOE,
			OBSIDIAN_SHIELD,
			TINKERERS_WORKSHOP,
			CLOUD_IN_A_BALLOON,
			ADAMANTITE_HEADGEAR,
			ADAMANTITE_HELMET,
			ADAMANTITE_MASK,
			ADAMANTITE_BREASTPLATE,
			ADAMANTITE_LEGGINGS,
			SPECTRE_BOOTS,
			ADAMANTITE_GLAIVE,
			TOOLBELT,
			PEARLSAND_BLOCK,
			PEARLSTONE_BLOCK,
			MINING_SHIRT,
			MINING_PANTS,
			PEARLSTONE_BRICK,
			IRIDESCENT_BRICK,
			MUDSTONE_BRICK,
			COBALT_BRICK,
			MYTHRIL_BRICK,
			PEARLSTONE_BRICK_WALL,
			IRIDESCENT_BRICK_WALL,
			MUDSTONE_BRICK_WALL,
			COBALT_BRICK_WALL,
			MYTHRIL_BRICK_WALL,
			HOLY_WATER,
			UNHOLY_WATER,
			SILT_BLOCK,
			FAIRY_BELL,
			BREAKER_BLADE,
			BLUE_TORCH,
			RED_TORCH,
			GREEN_TORCH,
			PURPLE_TORCH,
			WHITE_TORCH,
			YELLOW_TORCH,
			DEMON_TORCH,
			CLOCKWORK_ASSAULT_RIFLE,
			COBALT_REPEATER,
			MYTHRIL_REPEATER,
			DUAL_HOOK,
			STAR_STATUE,
			SWORD_STATUE,
			SLIME_STATUE,
			GOBLIN_STATUE,
			SHIELD_STATUE,
			BAT_STATUE,
			FISH_STATUE,
			BUNNY_STATUE,
			SKELETON_STATUE,
			REAPER_STATUE,
			WOMAN_STATUE,
			IMP_STATUE,
			GARGOYLE_STATUE,
			GLOOM_STATUE,
			HORNET_STATUE,
			BOMB_STATUE,
			CRAB_STATUE,
			HAMMER_STATUE,
			POTION_STATUE,
			SPEAR_STATUE,
			CROSS_STATUE,
			JELLYFISH_STATUE,
			BOW_STATUE,
			BOOMERANG_STATUE,
			BOOT_STATUE,
			CHEST_STATUE,
			BIRD_STATUE,
			AXE_STATUE,
			CORRUPT_STATUE,
			TREE_STATUE,
			ANVIL_STATUE,
			PICKAXE_STATUE,
			MUSHROOM_STATUE,
			EYEBALL_STATUE,
			PILLAR_STATUE,
			HEART_STATUE,
			POT_STATUE,
			SUNFLOWER_STATUE,
			KING_STATUE,
			QUEEN_STATUE,
			PIRANHA_STATUE,
			PLANKED_WALL,
			WOODEN_BEAM,
			ADAMANTITE_REPEATER,
			ADAMANTITE_SWORD,
			COBALT_SWORD,
			MYTHRIL_SWORD,
			MOON_CHARM,
			RULER,
			CRYSTAL_BALL,
			DISCO_BALL,
			SORCERER_EMBLEM,
			WARRIOR_EMBLEM,
			RANGER_EMBLEM,
			DEMON_WINGS,
			ANGEL_WINGS,
			MAGICAL_HARP,
			RAINBOW_ROD,
			ICE_ROD,
			NEPTUNES_SHELL,
			MANNEQUIN,
			GREATER_HEALING_POTION,
			GREATER_MANA_POTION,
			PIXIE_DUST,
			CRYSTAL_SHARD,
			CLOWN_HAT,
			CLOWN_SHIRT,
			CLOWN_PANTS,
			FLAMETHROWER,
			BELL,
			HARP,
			WRENCH,
			WIRE_CUTTER,
			ACTIVE_STONE_BLOCK,
			INACTIVE_STONE_BLOCK,
			LEVER,
			LASER_RIFLE,
			CRYSTAL_BULLET,
			HOLY_ARROW,
			MAGIC_DAGGER,
			CRYSTAL_STORM,
			CURSED_FLAMES,
			SOUL_OF_LIGHT,
			SOUL_OF_NIGHT,
			CURSED_FLAME,
			CURSED_TORCH,
			ADAMANTITE_FORGE,
			MYTHRIL_ANVIL,
			UNICORN_HORN,
			DARK_SHARD,
			LIGHT_SHARD,
			RED_PRESSURE_PLATE,
			WIRE,
			SPELL_TOME,
			STAR_CLOAK,
			MEGASHARK,
			SHOTGUN,
			PHILOSOPHERS_STONE,
			TITAN_GLOVE,
			COBALT_NAGINATA,
			SWITCH,
			DART_TRAP,
			BOULDER,
			GREEN_PRESSURE_PLATE,
			GRAY_PRESSURE_PLATE,
			BROWN_PRESSURE_PLATE,
			MECHANICAL_EYE,
			CURSED_ARROW,
			CURSED_BULLET,
			SOUL_OF_FRIGHT,
			SOUL_OF_MIGHT,
			SOUL_OF_SIGHT,
			GUNGNIR,
			HALLOWED_PLATE_MAIL,
			HALLOWED_GREAVES,
			HALLOWED_HELMET,
			CROSS_NECKLACE,
			MANA_FLOWER,
			MECHANICAL_WORM,
			MECHANICAL_SKULL,
			HALLOWED_HEADGEAR,
			HALLOWED_MASK,
			SLIME_CROWN,
			LIGHT_DISC,
			MUSIC_BOX_OVERWORLD_DAY,
			MUSIC_BOX_EERIE,
			MUSIC_BOX_NIGHT,
			MUSIC_BOX_TITLE,
			MUSIC_BOX_UNDERGROUND,
			MUSIC_BOX_BOSS1,
			MUSIC_BOX_JUNGLE,
			MUSIC_BOX_CORRUPTION,
			MUSIC_BOX_UNDERGROUND_CORRUPTION,
			MUSIC_BOX_THE_HALLOW,
			MUSIC_BOX_BOSS2,
			MUSIC_BOX_UNDERGROUND_HALLOW,
			MUSIC_BOX_BOSS3,
			SOUL_OF_FLIGHT,
			MUSIC_BOX,
			DEMONITE_BRICK,
			HALLOWED_REPEATER,
			HAMDRAX,
			EXPLOSIVES,
			INLET_PUMP,
			OUTLET_PUMP,
			ONE_SECOND_TIMER,
			THREE_SECOND_TIMER,
			FIVE_SECOND_TIMER,
			CANDY_CANE_BLOCK,
			CANDY_CANE_WALL,
			SANTA_HAT,
			SANTA_SHIRT,
			SANTA_PANTS,
			GREEN_CANDY_CANE_BLOCK,
			GREEN_CANDY_CANE_WALL,
			SNOW_BLOCK,
			SNOW_BRICK,
			SNOW_BRICK_WALL,
			BLUE_LIGHT,
			RED_LIGHT,
			GREEN_LIGHT,
			BLUE_PRESENT,
			GREEN_PRESENT,
			YELLOW_PRESENT,
			SNOW_GLOBE,
			PET_SPAWN_1, // Guinea Pig
			DRAGON_MASK,
			TITAN_HELMET,
			SPECTRAL_HEADGEAR,
			DRAGON_BREASTPLATE,
			TITAN_MAIL,
			SPECTRAL_ARMOR,
			DRAGON_GREAVES,
			TITAN_LEGGINGS,
			SPECTRAL_SUBLIGAR,
			TIZONA,
			TONBOGIRI,
			SHARANGA,
			SPECTRAL_ARROW,
			VULCAN_REPEATER,
			VULCAN_BOLT,
			SUSPICIOUS_LOOKING_SKULL,
			SOUL_OF_BLIGHT,
			PET_SPAWN_2, // Slime
			PET_SPAWN_3, // Hornet
			PET_SPAWN_4, // Bat
			PET_SPAWN_5, // Werewolf
			PET_SPAWN_6, // Zombie
			MUSIC_BOX_DESERT,
			MUSIC_BOX_SPACE,
			MUSIC_BOX_TUTORIAL,
			MUSIC_BOX_BOSS4,
			MUSIC_BOX_OCEAN,
			MUSIC_BOX_SNOW,
#if VERSION_101
			FABULOUS_RIBBON,
			GEORGES_HAT,
			FABULOUS_DRESS,
			GEORGES_SUIT,
			FABULOUS_SLIPPERS,
			GEORGES_PANTS,
			SPARKLY_WINGS,
			CAMPFIRE,
			WOOD_HELMET,
			WOOD_BREASTPLATE,
			WOOD_GREAVES,
			CACTUS_SWORD,
			CACTUS_PICKAXE,
			CACTUS_HELMET,
			CACTUS_BREASTPLATE,
			CACTUS_LEGGINGS,
			PURPLE_STAINED_GLASS,
			YELLOW_STAINED_GLASS,
			BLUE_STAINED_GLASS,
			GREEN_STAINED_GLASS,
			RED_STAINED_GLASS,
			MULTICOLORED_STAINED_GLASS,
#endif
			NUM_TYPES
		}

		public const int MaxNumItemTypes = (int)ID.NUM_TYPES;
		
		public const uint PotionDelay = 3600u;

		public const uint PotionDelayPhilosopher = 2700u;

		public static short[] HeadType = new short[Player.MaxNumArmorHead];

		public static short[] BodyType = new short[Player.MaxNumArmorBody];

		public static short[] LegType = new short[Player.MaxNumArmorLegs];

		public short Type;

		public byte Active;

		public bool BeingGrabbed;

		public bool WornArmor;

		public bool Mech;

		public bool IsWet;

		public byte WetCount;

		public bool IsInLava;

		public bool Channelling;

		public bool IsAccessory;

		public bool IsPotion;

		public bool IsConsumable;

		public bool AutoReuse;

		public bool CanUseTurn;

		public bool OnlyBuyOnce;

		public bool NoUseGraphic;

		public bool NoMelee;

		public bool CanBuy;

		public bool IsSocial;

		public bool IsVanity;

		public bool IsMaterial;

		public bool CantTouchLiquid;

		public bool IsMelee;

		public bool IsMagic;

		public bool IsRanged;

		public byte PrefixType;

		public byte NoGrabDelay;

		public byte HoldStyle;

		public byte UseStyle;

		public byte UseAnimation;

		public byte UseTime;

		public byte PickPower;

		public byte AxePower;

		public byte HammerPower;

		public sbyte TileBoost;

		public byte PlaceStyle;

		public byte Alpha;

		public byte Owner;

		public byte OwnIgnore;

		public byte OwnTime;

		public byte KeepTime;

		public byte UseSound;

		public short Stack;

		public short MaxStack;

		public short CreateTile;

		public short CreateWall;

		public short Damage;

		public short HealLife;

		public short HealMana;

		public uint SpawnTime;

		public ushort Width;

		public ushort Height;

		public Vector2 Position;

		public Vector2 Velocity;

		public float Knockback;

		public Color Colour;

		public float Scale;

		public short Defense;

		public short HeadSlot;

		public short BodySlot;

		public short LegSlot;

		public ushort BuffTime;

		public byte BuffType;

		public byte ReuseDelay;

		public short NetID;

		public short Crit;

		public sbyte Rarity;

		public byte Ammo;

		public byte UseAmmo;

		public byte Shoot;

		public float ShootSpeed;

		public byte LifeRegen;

		public byte Mana;

		public ushort Release;

		public int Value;

		private static readonly byte[] ToolPrefixs = new byte[40]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			36,
			37,
			38,
			53,
			54,
			55,
			39,
			40,
			56,
			41,
			57,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			59,
			60,
			61,
			81
		};

		private static readonly byte[] SpearPrefixs = new byte[14]
		{
			36,
			37,
			38,
			53,
			54,
			55,
			39,
			40,
			56,
			41,
			57,
			59,
			60,
			61
		};

		private static readonly byte[] GunPrefixs = new byte[36]
		{
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			58,
			36,
			37,
			38,
			53,
			54,
			55,
			39,
			40,
			56,
			41,
			57,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			59,
			60,
			61,
			82
		};

		private static readonly byte[] MagicPrefixs = new byte[36]
		{
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			52,
			36,
			37,
			38,
			53,
			54,
			55,
			39,
			40,
			56,
			41,
			57,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			59,
			60,
			61,
			83
		};

		private static readonly byte[] BoomerangPrefixs = new byte[14]
		{
			36,
			37,
			38,
			53,
			54,
			55,
			39,
			40,
			56,
			41,
			57,
			59,
			60,
			61
		};

#if !USE_ORIGINAL_CODE
		public static readonly ID[] ArmorIDs =
		{
			ID.MINING_HELMET,
			ID.MINING_SHIRT,
			ID.MINING_PANTS,

#if VERSION_103 || VERSION_FINAL // BUG: In 1.01, they added Cactus and Wood as armours from PC 1.2, but they didn't account for the armour achievement until Console 1.2.
			ID.WOOD_HELMET,
			ID.WOOD_BREASTPLATE,
			ID.WOOD_GREAVES,
			ID.CACTUS_HELMET,
			ID.CACTUS_BREASTPLATE,
			ID.CACTUS_LEGGINGS,
#endif

			ID.COPPER_HELMET,
			ID.COPPER_CHAINMAIL,
			ID.COPPER_GREAVES,
			ID.IRON_HELMET,
			ID.IRON_CHAINMAIL,
			ID.IRON_GREAVES,
			ID.SILVER_HELMET,
			ID.SILVER_CHAINMAIL,
			ID.SILVER_GREAVES,
			ID.GOLD_HELMET,
			ID.GOLD_CHAINMAIL,
			ID.GOLD_GREAVES,
			ID.METEOR_HELMET,
			ID.METEOR_SUIT,
			ID.METEOR_LEGGINGS,
			ID.SHADOW_HELMET,
			ID.SHADOW_SCALEMAIL,
			ID.SHADOW_GREAVES,
			ID.JUNGLE_HAT,
			ID.JUNGLE_SHIRT,
			ID.JUNGLE_PANTS,
			ID.NECRO_HELMET,
			ID.NECRO_BREASTPLATE,
			ID.NECRO_GREAVES,
			ID.MOLTEN_HELMET,
			ID.MOLTEN_BREASTPLATE,
			ID.MOLTEN_GREAVES,
			ID.COBALT_HAT,
			ID.COBALT_HELMET,
			ID.COBALT_MASK,
			ID.COBALT_BREASTPLATE,
			ID.COBALT_LEGGINGS,
			ID.MYTHRIL_HOOD,
			ID.MYTHRIL_HELMET,
			ID.MYTHRIL_HAT,
			ID.MYTHRIL_CHAINMAIL,
			ID.MYTHRIL_GREAVES,
			ID.ADAMANTITE_HEADGEAR,
			ID.ADAMANTITE_MASK,
			ID.ADAMANTITE_HELMET,
			ID.ADAMANTITE_BREASTPLATE,
			ID.ADAMANTITE_LEGGINGS,
			ID.HALLOWED_HEADGEAR,
			ID.HALLOWED_MASK,
			ID.HALLOWED_HELMET,
			ID.HALLOWED_PLATE_MAIL,
			ID.HALLOWED_GREAVES,
			ID.DRAGON_MASK,
			ID.DRAGON_BREASTPLATE,
			ID.DRAGON_GREAVES,
			ID.TITAN_HELMET,
			ID.TITAN_MAIL,
			ID.TITAN_LEGGINGS,
			ID.SPECTRAL_HEADGEAR,
			ID.SPECTRAL_ARMOR,
			ID.SPECTRAL_SUBLIGAR,
		};
#endif

		private static uint LastItemIndex = 0;

		public void Init()
		{
			Active = 0;
			Owner = 8;
			Type = 0;
			NetID = 0;
			PrefixType = 0;
			Crit = 0;
			WornArmor = false;
			Mech = false;
			ReuseDelay = 0;
			IsMelee = false;
			IsMagic = false;
			IsRanged = false;
			PlaceStyle = 0;
			BuffTime = 0;
			BuffType = 0;
			IsMaterial = false;
			CantTouchLiquid = false;
			IsVanity = false;
			Mana = 0;
			IsWet = false;
			WetCount = 0;
			IsInLava = false;
			Channelling = false;
			OnlyBuyOnce = false;
			IsSocial = false;
			Release = 0;
			NoMelee = false;
			NoUseGraphic = false;
			LifeRegen = 0;
			ShootSpeed = 0f;
			Alpha = 0;
			Ammo = 0;
			UseAmmo = 0;
			AutoReuse = false;
			IsAccessory = false;
			AxePower = 0;
			HealMana = 0;
			BodySlot = -1;
			LegSlot = -1;
			HeadSlot = -1;
			IsPotion = false;
			IsConsumable = false;
			CreateTile = -1;
			CreateWall = -1;
			Damage = 0;
			Defense = 0;
			HammerPower = 0;
			HealLife = 0;
			Knockback = 0f;
			PickPower = 0;
			Rarity = 0;
			Scale = 1f;
			Shoot = 0;
			Stack = 0;
			MaxStack = 0;
			TileBoost = 0;
			HoldStyle = 0;
			UseStyle = 0;
			UseSound = 0;
			UseTime = 100;
			UseAnimation = 100;
			Value = 0;
			CanUseTurn = false;
			CanBuy = false;
			OwnIgnore = 8;
			OwnTime = 0;
			KeepTime = 0;
		}

		public bool IsLocal()
		{
			if (Owner < 8)
			{
				return Main.PlayerSet[Owner].isLocal();
			}
			return false;
		}

		public bool IsEquipable()
		{
			if (!IsAccessory && HeadSlot < 0 && BodySlot < 0)
			{
				return LegSlot >= 0;
			}
			return true;
		}

		public bool SetPrefix(int Setting)
		{
			if (Setting == 0 || Type == 0)
			{
				return false;
			}
			int PrefixID = Setting;
			float PrefixDamage = 1f;
			float PrefixKB = 1f;
			float PrefixDelay = 1f;
			float PrefixScale = 1f;
			float PrefixFireRate = 1f;
			float PrefixManaSave = 1f;
			int PrefixCrit = 0;
			bool flag = true;
			while (flag)
			{
				PrefixDamage = 1f;
				PrefixKB = 1f;
				PrefixDelay = 1f;
				PrefixScale = 1f;
				PrefixFireRate = 1f;
				PrefixManaSave = 1f;
				PrefixCrit = 0;
				flag = false;
				if (PrefixID == -1 && Main.Rand.Next(4) == 0)
				{
					PrefixID = 0;
				}
				if (Setting < -1)
				{
					PrefixID = -1;
				}
				if (PrefixID == -1 || PrefixID == -2 || PrefixID == -3)
				{
#if !VERSION_INITIAL
					if (Type == (int)Item.ID.IRON_PICKAXE || Type == (int)Item.ID.IRON_BROADSWORD || Type == (int)Item.ID.IRON_SHORTSWORD || Type == (int)Item.ID.IRON_HAMMER || Type == (int)Item.ID.IRON_AXE || Type == (int)Item.ID.WOODEN_SWORD || Type == (int)Item.ID.WAR_AXE_OF_THE_NIGHT || Type == (int)Item.ID.LIGHTS_BANE || Type == (int)Item.ID.NIGHTMARE_PICKAXE || Type == (int)Item.ID.THE_BREAKER || Type == (int)Item.ID.FIERY_GREATSWORD || Type == (int)Item.ID.MOLTEN_PICKAXE || Type == (int)Item.ID.MURAMASA || Type == (int)Item.ID.BLADE_OF_GRASS || Type == (int)Item.ID.WOODEN_HAMMER || Type == (int)Item.ID.BLUE_PHASEBLADE || Type == (int)Item.ID.RED_PHASEBLADE || Type == (int)Item.ID.GREEN_PHASEBLADE || Type == (int)Item.ID.PURPLE_PHASEBLADE || Type == (int)Item.ID.WHITE_PHASEBLADE || Type == (int)Item.ID.YELLOW_PHASEBLADE || Type == (int)Item.ID.METEOR_HAMAXE || Type == (int)Item.ID.STAFF_OF_REGROWTH || Type == (int)Item.ID.MOLTEN_HAMAXE || Type == (int)Item.ID.NIGHTS_EDGE || Type == (int)Item.ID.PWNHAMMER || Type == (int)Item.ID.EXCALIBUR || Type == (int)Item.ID.BREAKER_BLADE || Type == (int)Item.ID.ADAMANTITE_SWORD || Type == (int)Item.ID.COBALT_SWORD || Type == (int)Item.ID.MYTHRIL_SWORD || Type == (int)Item.ID.TIZONA || Type == (int)ID.CACTUS_SWORD || Type == (int)ID.CACTUS_PICKAXE)
#else
					if (Type == (int)Item.ID.IRON_PICKAXE || Type == (int)Item.ID.IRON_BROADSWORD || Type == (int)Item.ID.IRON_SHORTSWORD || Type == (int)Item.ID.IRON_HAMMER || Type == (int)Item.ID.IRON_AXE || Type == (int)Item.ID.WOODEN_SWORD || Type == (int)Item.ID.WAR_AXE_OF_THE_NIGHT || Type == (int)Item.ID.LIGHTS_BANE || Type == (int)Item.ID.NIGHTMARE_PICKAXE || Type == (int)Item.ID.THE_BREAKER || Type == (int)Item.ID.FIERY_GREATSWORD || Type == (int)Item.ID.MOLTEN_PICKAXE || Type == (int)Item.ID.MURAMASA || Type == (int)Item.ID.BLADE_OF_GRASS || Type == (int)Item.ID.WOODEN_HAMMER || Type == (int)Item.ID.BLUE_PHASEBLADE || Type == (int)Item.ID.RED_PHASEBLADE || Type == (int)Item.ID.GREEN_PHASEBLADE || Type == (int)Item.ID.PURPLE_PHASEBLADE || Type == (int)Item.ID.WHITE_PHASEBLADE || Type == (int)Item.ID.YELLOW_PHASEBLADE || Type == (int)Item.ID.METEOR_HAMAXE || Type == (int)Item.ID.STAFF_OF_REGROWTH || Type == (int)Item.ID.MOLTEN_HAMAXE || Type == (int)Item.ID.NIGHTS_EDGE || Type == (int)Item.ID.PWNHAMMER || Type == (int)Item.ID.EXCALIBUR || Type == (int)Item.ID.BREAKER_BLADE || Type == (int)Item.ID.ADAMANTITE_SWORD || Type == (int)Item.ID.COBALT_SWORD || Type == (int)Item.ID.MYTHRIL_SWORD || Type == (int)Item.ID.TIZONA)
#endif
					{
						PrefixID = ToolPrefixs[Main.Rand.Next(ToolPrefixs.Length)];
					}
					else if (Type == (int)Item.ID.BALL_O_HURT || Type == (int)Item.ID.HARPOON || Type == (int)Item.ID.BLUE_MOON || Type == (int)Item.ID.SUNFURY || Type == (int)Item.ID.DARK_LANCE || Type == (int)Item.ID.TRIDENT || Type == (int)Item.ID.SPEAR || Type == (int)Item.ID.COBALT_CHAINSAW || Type == (int)Item.ID.MYTHRIL_CHAINSAW || Type == (int)Item.ID.COBALT_DRILL || Type == (int)Item.ID.MYTHRIL_DRILL || Type == (int)Item.ID.ADAMANTITE_CHAINSAW || Type == (int)Item.ID.ADAMANTITE_DRILL || Type == (int)Item.ID.DAO_OF_POW || Type == (int)Item.ID.MYTHRIL_HALBERD || Type == (int)Item.ID.ADAMANTITE_GLAIVE || Type == (int)Item.ID.COBALT_NAGINATA || Type == (int)Item.ID.GUNGNIR || Type == (int)Item.ID.HAMDRAX || Type == (int)Item.ID.TONBOGIRI)
					{
						PrefixID = SpearPrefixs[Main.Rand.Next(SpearPrefixs.Length)];
					}
					else if (Type == (int)Item.ID.WOODEN_BOW || Type == (int)Item.ID.DEMON_BOW || Type == (int)Item.ID.FLINTLOCK_PISTOL || Type == (int)Item.ID.MUSKET || Type == (int)Item.ID.MINISHARK || Type == (int)Item.ID.IRON_BOW || Type == (int)Item.ID.MOLTEN_FURY || Type == (int)Item.ID.HANDGUN || Type == (int)Item.ID.STAR_CANNON || Type == (int)Item.ID.PHOENIX_BLASTER || Type == (int)Item.ID.SANDGUN || Type == (int)Item.ID.BLOWPIPE || Type == (int)Item.ID.CLOCKWORK_ASSAULT_RIFLE || Type == (int)Item.ID.COBALT_REPEATER || Type == (int)Item.ID.MYTHRIL_REPEATER || Type == (int)Item.ID.ADAMANTITE_REPEATER || Type == (int)Item.ID.FLAMETHROWER || Type == (int)Item.ID.MEGASHARK || Type == (int)Item.ID.SHOTGUN || Type == (int)Item.ID.HALLOWED_REPEATER || Type == (int)Item.ID.SHARANGA || Type == (int)Item.ID.VULCAN_REPEATER)
					{
						PrefixID = GunPrefixs[Main.Rand.Next(GunPrefixs.Length)];
					}
					else if (Type == (int)Item.ID.VILETHORN || Type == (int)Item.ID.STARFURY || Type == (int)Item.ID.FLOWER_OF_FIRE || Type == (int)Item.ID.MAGIC_MISSILE || Type == (int)Item.ID.SPACE_GUN || Type == (int)Item.ID.AQUA_SCEPTER || Type == (int)Item.ID.WATER_BOLT || Type == (int)Item.ID.FLAMELASH || Type == (int)Item.ID.DEMON_SCYTHE || Type == (int)Item.ID.MAGICAL_HARP || Type == (int)Item.ID.RAINBOW_ROD || Type == (int)Item.ID.ICE_ROD || Type == (int)Item.ID.LASER_RIFLE || Type == (int)Item.ID.MAGIC_DAGGER || Type == (int)Item.ID.CRYSTAL_STORM || Type == (int)Item.ID.CURSED_FLAMES)
					{
						PrefixID = MagicPrefixs[Main.Rand.Next(MagicPrefixs.Length)];
					}
					else if (Type == (int)Item.ID.ENCHANTED_BOOMERANG || Type == (int)Item.ID.FLAMARANG || Type == (int)Item.ID.THORN_CHAKRAM || Type == (int)Item.ID.WOODEN_BOOMERANG)
					{
						PrefixID = BoomerangPrefixs[Main.Rand.Next(BoomerangPrefixs.Length)];
					}
					else
					{
						if (!IsAccessory || Type == (int)Item.ID.GUIDE_VOODOO_DOLL || Type == (int)Item.ID.MUSIC_BOX_OVERWORLD_DAY || Type == (int)Item.ID.MUSIC_BOX_EERIE || Type == (int)Item.ID.MUSIC_BOX_NIGHT || Type == (int)Item.ID.MUSIC_BOX_TITLE || Type == (int)Item.ID.MUSIC_BOX_UNDERGROUND || Type == (int)Item.ID.MUSIC_BOX_BOSS1 || Type == (int)Item.ID.MUSIC_BOX_JUNGLE || Type == (int)Item.ID.MUSIC_BOX_CORRUPTION || Type == (int)Item.ID.MUSIC_BOX_UNDERGROUND_CORRUPTION || Type == (int)Item.ID.MUSIC_BOX_THE_HALLOW || Type == (int)Item.ID.MUSIC_BOX_BOSS2 || Type == (int)Item.ID.MUSIC_BOX_UNDERGROUND_HALLOW || Type == (int)Item.ID.MUSIC_BOX_BOSS3 || Type == (int)Item.ID.MUSIC_BOX)
						// BUG: Despite adding the checks for the cactus sword and pick, they do not exclude the exclusive music boxes in either 1.0 or 1.01. This means you can get accessory modifiers on the console music boxes.
						{
							return false;
						}
						PrefixID = Main.Rand.Next(62, 81);
					}
				}

				switch (Setting)
				{
				case -3:
					return true;
				case -1:
					if (((PrefixID >= 7 && PrefixID <= 11) || PrefixID == 22 || PrefixID == 23 || PrefixID == 24 || PrefixID == 29 || PrefixID == 30 || PrefixID == 31 || PrefixID == 39 || PrefixID == 40 || PrefixID == 41 || PrefixID == 47 || PrefixID == 48 || PrefixID == 49 || PrefixID == 56) && Main.Rand.Next(3) != 0)
					{
						PrefixID = 0;
					}
					break;
				}
				switch (PrefixID)
				{
				case 1:
					PrefixScale = 1.12f;
					break;
				case 2:
					PrefixScale = 1.18f;
					break;
				case 3:
					PrefixDamage = 1.05f;
					PrefixCrit = 2;
					PrefixScale = 1.05f;
					break;
				case 4:
					PrefixDamage = 1.1f;
					PrefixScale = 1.1f;
					PrefixKB = 1.1f;
					break;
				case 5:
					PrefixDamage = 1.15f;
					break;
				case 6:
					PrefixDamage = 1.1f;
					break;
				case 81:
					PrefixKB = 1.15f;
					PrefixDamage = 1.15f;
					PrefixCrit = 5;
					PrefixDelay = 0.9f;
					PrefixScale = 1.1f;
					break;
				case 7:
					PrefixScale = 0.82f;
					break;
				case 8:
					PrefixKB = 0.85f;
					PrefixDamage = 0.85f;
					PrefixScale = 0.87f;
					break;
				case 9:
					PrefixScale = 0.9f;
					break;
				case 10:
					PrefixDamage = 0.85f;
					break;
				case 11:
					PrefixDelay = 1.1f;
					PrefixKB = 0.9f;
					PrefixScale = 0.9f;
					break;
				case 12:
					PrefixKB = 1.1f;
					PrefixDamage = 1.05f;
					PrefixScale = 1.1f;
					PrefixDelay = 1.15f;
					break;
				case 13:
					PrefixKB = 0.8f;
					PrefixDamage = 0.9f;
					PrefixScale = 1.1f;
					break;
				case 14:
					PrefixKB = 1.15f;
					PrefixDelay = 1.1f;
					break;
				case 15:
					PrefixKB = 0.9f;
					PrefixDelay = 0.85f;
					break;
				case 16:
					PrefixDamage = 1.1f;
					PrefixCrit = 3;
					break;
				case 17:
					PrefixDelay = 0.85f;
					PrefixFireRate = 1.1f;
					break;
				case 18:
					PrefixDelay = 0.9f;
					PrefixFireRate = 1.15f;
					break;
				case 19:
					PrefixKB = 1.15f;
					PrefixFireRate = 1.05f;
					break;
				case 20:
					PrefixKB = 1.05f;
					PrefixFireRate = 1.05f;
					PrefixDamage = 1.1f;
					PrefixDelay = 0.95f;
					PrefixCrit = 2;
					break;
				case 21:
					PrefixKB = 1.15f;
					PrefixDamage = 1.1f;
					break;
				case 82:
					PrefixKB = 1.15f;
					PrefixDamage = 1.15f;
					PrefixCrit = 5;
					PrefixDelay = 0.9f;
					PrefixFireRate = 1.1f;
					break;
				case 22:
					PrefixKB = 0.9f;
					PrefixFireRate = 0.9f;
					PrefixDamage = 0.85f;
					break;
				case 23:
					PrefixDelay = 1.15f;
					PrefixFireRate = 0.9f;
					break;
				case 24:
					PrefixDelay = 1.1f;
					PrefixKB = 0.8f;
					break;
				case 25:
					PrefixDelay = 1.1f;
					PrefixDamage = 1.15f;
					PrefixCrit = 1;
					break;
				case 58:
					PrefixDelay = 0.85f;
					PrefixDamage = 0.85f;
					break;
				case 26:
					PrefixManaSave = 0.85f;
					PrefixDamage = 1.1f;
					break;
				case 27:
					PrefixManaSave = 0.85f;
					break;
				case 28:
					PrefixManaSave = 0.85f;
					PrefixDamage = 1.15f;
					PrefixKB = 1.05f;
					break;
				case 83:
					PrefixKB = 1.15f;
					PrefixDamage = 1.15f;
					PrefixCrit = 5;
					PrefixDelay = 0.9f;
					PrefixManaSave = 0.9f;
					break;
				case 29:
					PrefixManaSave = 1.1f;
					break;
				case 30:
					PrefixManaSave = 1.2f;
					PrefixDamage = 0.9f;
					break;
				case 31:
					PrefixKB = 0.9f;
					PrefixDamage = 0.9f;
					break;
				case 32:
					PrefixManaSave = 1.15f;
					PrefixDamage = 1.1f;
					break;
				case 33:
					PrefixManaSave = 1.1f;
					PrefixKB = 1.1f;
					PrefixDelay = 0.9f;
					break;
				case 34:
					PrefixManaSave = 0.9f;
					PrefixKB = 1.1f;
					PrefixDelay = 1.1f;
					PrefixDamage = 1.1f;
					break;
				case 35:
					PrefixManaSave = 1.2f;
					PrefixDamage = 1.15f;
					PrefixKB = 1.15f;
					break;
				case 52:
					PrefixManaSave = 0.9f;
					PrefixDamage = 0.9f;
					PrefixDelay = 0.9f;
					break;
				case 36:
					PrefixCrit = 3;
					break;
				case 37:
					PrefixDamage = 1.1f;
					PrefixCrit = 3;
					PrefixKB = 1.1f;
					break;
				case 38:
					PrefixKB = 1.15f;
					break;
				case 53:
					PrefixDamage = 1.1f;
					break;
				case 54:
					PrefixKB = 1.15f;
					break;
				case 55:
					PrefixKB = 1.15f;
					PrefixDamage = 1.05f;
					break;
				case 59:
					PrefixKB = 1.15f;
					PrefixDamage = 1.15f;
					PrefixCrit = 5;
					break;
				case 60:
					PrefixDamage = 1.15f;
					PrefixCrit = 5;
					break;
				case 61:
					PrefixCrit = 5;
					break;
				case 39:
					PrefixDamage = 0.7f;
					PrefixKB = 0.8f;
					break;
				case 40:
					PrefixDamage = 0.85f;
					break;
				case 56:
					PrefixKB = 0.8f;
					break;
				case 41:
					PrefixKB = 0.85f;
					PrefixDamage = 0.9f;
					break;
				case 57:
					PrefixKB = 0.9f;
					PrefixDamage = 1.18f;
					break;
				case 42:
					PrefixDelay = 0.9f;
					break;
				case 43:
					PrefixDamage = 1.1f;
					PrefixDelay = 0.9f;
					break;
				case 44:
					PrefixDelay = 0.9f;
					PrefixCrit = 3;
					break;
				case 45:
					PrefixDelay = 0.95f;
					break;
				case 46:
					PrefixCrit = 3;
					PrefixDelay = 0.94f;
					PrefixDamage = 1.07f;
					break;
				case 47:
					PrefixDelay = 1.15f;
					break;
				case 48:
					PrefixDelay = 1.2f;
					break;
				case 49:
					PrefixDelay = 1.08f;
					break;
				case 50:
					PrefixDamage = 0.8f;
					PrefixDelay = 1.15f;
					break;
				case 51:
					PrefixKB = 0.9f;
					PrefixDelay = 0.9f;
					PrefixDamage = 1.05f;
					PrefixCrit = 2;
					break;
				}
				if (PrefixDamage != 1f && Math.Round(Damage * PrefixDamage) == Damage)
				{
					flag = true;
					PrefixID = -1;
				}
				if (PrefixDelay != 1f && Math.Round(UseAnimation * PrefixDelay) == UseAnimation)
				{
					flag = true;
					PrefixID = -1;
				}
				if (PrefixManaSave != 1f && Math.Round(Mana * PrefixManaSave) == Mana)
				{
					flag = true;
					PrefixID = -1;
				}
				if (PrefixKB != 1f && Knockback == 0f)
				{
					flag = true;
					PrefixID = -1;
				}
				if (Setting == -2 && PrefixID == 0)
				{
					PrefixID = -1;
					flag = true;
				}
			}
			Damage = (short)Math.Round(Damage * PrefixDamage);
			UseAnimation = (byte)Math.Round(UseAnimation * PrefixDelay);
			UseTime = (byte)Math.Round(UseTime * PrefixDelay);
			ReuseDelay = (byte)Math.Round(ReuseDelay * PrefixDelay);
			Mana = (byte)Math.Round(Mana * PrefixManaSave);
			Knockback *= PrefixKB;
			Scale *= PrefixScale;
			ShootSpeed *= PrefixFireRate;
			Crit += (short)PrefixCrit;
			float FinalMultiplier = PrefixDamage * (2f - PrefixDelay) * (2f - PrefixManaSave) * PrefixScale * PrefixKB * PrefixFireRate * (1f + Crit * 0.02f);
			switch (PrefixID)
			{
			case 62:
			case 69:
			case 73:
			case 77:
				FinalMultiplier *= 1.05f;
				break;
			case 63:
			case 67:
			case 70:
			case 74:
			case 78:
				FinalMultiplier *= 1.1f;
				break;
			case 64:
			case 66:
			case 71:
			case 75:
			case 79:
				FinalMultiplier *= 1.15f;
				break;
			case 65:
			case 68:
			case 72:
			case 76:
			case 80:
				FinalMultiplier *= 1.2f;
				break;
			}
			PrefixType = (byte)PrefixID;
			if (FinalMultiplier >= 1.2f)
			{
				Rarity += 2;
			}
			else if (FinalMultiplier >= 1.05f)
			{
				Rarity++;
			}
			else if (FinalMultiplier <= 0.8f)
			{
				Rarity -= 2;
			}
			else if (FinalMultiplier <= 0.95f)
			{
				Rarity--;
			}
			if (Rarity < -1)
			{
				Rarity = -1;
			}
			else if (Rarity > 6)
			{
				Rarity = 6;
			}
			FinalMultiplier *= FinalMultiplier;
			Value = (int)(Value * FinalMultiplier);
			return true;
		}

		public string Name()
		{
			return Lang.ItemName(NetID);
		}

		public string AffixName()
		{
			return Lang.ItemAffixName(PrefixType, NetID);
		}

		public void SetDefaults(string ItemName)
		{
			bool IsNotMaterial = false;
			switch (ItemName)
			{
			case "Gold Pickaxe":
				SetDefaults((int)ID.IRON_PICKAXE);
				Colour = new Color(210, 190, 0, 100);
				UseTime = 17;
				PickPower = 55;
				UseAnimation = 20;
				Scale = 1.05f;
				Damage = 6;
				Value = 10000;
				NetID = -1;
				break;
			case "Gold Broadsword":
				SetDefaults((int)ID.IRON_BROADSWORD);
				Colour = new Color(210, 190, 0, 100);
				UseAnimation = 20;
				Damage = 13;
				Scale = 1.05f;
				Value = 9000;
				NetID = -2;
				break;
			case "Gold Shortsword":
				SetDefaults((int)ID.IRON_SHORTSWORD);
				Colour = new Color(210, 190, 0, 100);
				Damage = 11;
				UseAnimation = 11;
				Scale = 0.95f;
				Value = 7000;
				NetID = -3;
				break;
			case "Gold Axe":
				SetDefaults((int)ID.IRON_AXE);
				Colour = new Color(210, 190, 0, 100);
				UseTime = 18;
				AxePower = 11;
				UseAnimation = 26;
				Scale = 1.15f;
				Damage = 7;
				Value = 8000;
				NetID = -4;
				break;
			case "Gold Hammer":
				SetDefaults((int)ID.IRON_HAMMER);
				Colour = new Color(210, 190, 0, 100);
				UseAnimation = 28;
				UseTime = 23;
				Scale = 1.25f;
				Damage = 9;
				HammerPower = 55;
				Value = 8000;
				NetID = -5;
				break;
			case "Gold Bow":
				SetDefaults((int)ID.IRON_BOW);
				UseAnimation = 26;
				UseTime = 26;
				Colour = new Color(210, 190, 0, 100);
				Damage = 11;
				Value = 7000;
				NetID = -6;
				break;
			case "Silver Pickaxe":
				SetDefaults((int)ID.IRON_PICKAXE);
				Colour = new Color(180, 180, 180, 100);
				UseTime = 11;
				PickPower = 45;
				UseAnimation = 19;
				Scale = 1.05f;
				Damage = 6;
				Value = 5000;
				NetID = -7;
				break;
			case "Silver Broadsword":
				SetDefaults((int)ID.IRON_BROADSWORD);
				Colour = new Color(180, 180, 180, 100);
				UseAnimation = 21;
				Damage = 11;
				Value = 4500;
				NetID = -8;
				break;
			case "Silver Shortsword":
				SetDefaults((int)ID.IRON_SHORTSWORD);
				Colour = new Color(180, 180, 180, 100);
				Damage = 9;
				UseAnimation = 12;
				Scale = 0.95f;
				Value = 3500;
				NetID = -9;
				break;
			case "Silver Axe":
				SetDefaults((int)ID.IRON_AXE);
				Colour = new Color(180, 180, 180, 100);
				UseTime = 18;
				AxePower = 10;
				UseAnimation = 26;
				Scale = 1.15f;
				Damage = 6;
				Value = 4000;
				NetID = -10;
				break;
			case "Silver Hammer":
				SetDefaults((int)ID.IRON_HAMMER);
				Colour = new Color(180, 180, 180, 100);
				UseAnimation = 29;
				UseTime = 19;
				Scale = 1.25f;
				Damage = 9;
				HammerPower = 45;
				Value = 4000;
				NetID = -11;
				break;
			case "Silver Bow":
				SetDefaults((int)ID.IRON_BOW);
				UseAnimation = 27;
				UseTime = 27;
				Colour = new Color(180, 180, 180, 100);
				Damage = 9;
				Value = 3500;
				NetID = -12;
				break;
			case "Copper Pickaxe":
				SetDefaults((int)ID.IRON_PICKAXE);
				Colour = new Color(180, 100, 45, 80);
				UseTime = 15;
				PickPower = 35;
				UseAnimation = 23;
				Damage = 4;
				Scale = 0.9f;
				TileBoost = -1;
				Value = 500;
				NetID = -13;
				break;
			case "Copper Broadsword":
				SetDefaults((int)ID.IRON_BROADSWORD);
				Colour = new Color(180, 100, 45, 80);
				UseAnimation = 23;
				Damage = 8;
				Value = 450;
				NetID = -14;
				break;
			case "Copper Shortsword":
				SetDefaults((int)ID.IRON_SHORTSWORD);
				Colour = new Color(180, 100, 45, 80);
				Damage = 5;
				UseAnimation = 13;
				Scale = 0.8f;
				Value = 350;
				NetID = -15;
				break;
			case "Copper Axe":
				SetDefaults((int)ID.IRON_AXE);
				Colour = new Color(180, 100, 45, 80);
				UseTime = 21;
				AxePower = 7;
				UseAnimation = 30;
				Scale = 1f;
				Damage = 3;
				TileBoost = -1;
				Value = 400;
				NetID = -16;
				break;
			case "Copper Hammer":
				SetDefaults((int)ID.IRON_HAMMER);
				Colour = new Color(180, 100, 45, 80);
				UseAnimation = 33;
				UseTime = 23;
				Scale = 1.1f;
				Damage = 4;
				HammerPower = 35;
				TileBoost = -1;
				Value = 400;
				NetID = -17;
				break;
			case "Copper Bow":
				SetDefaults((int)ID.IRON_BOW);
				UseAnimation = 29;
				UseTime = 29;
				Colour = new Color(180, 100, 45, 80);
				Damage = 6;
				Value = 350;
				NetID = -18;
				break;
			case "Blue Phasesaber":
				SetDefaults((int)ID.BLUE_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -19;
				break;
			case "Red Phasesaber":
				SetDefaults((int)ID.RED_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -20;
				break;
			case "Green Phasesaber":
				SetDefaults((int)ID.GREEN_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -21;
				break;
			case "Purple Phasesaber":
				SetDefaults((int)ID.PURPLE_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -22;
				break;
			case "White Phasesaber":
				SetDefaults((int)ID.WHITE_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -23;
				break;
			case "Yellow Phasesaber":
				SetDefaults((int)ID.YELLOW_PHASEBLADE);
				Damage = 41;
				Scale = 1.15f;
				IsNotMaterial = true;
				AutoReuse = true;
				CanUseTurn = true;
				Rarity = 4;
				NetID = -24;
				break;
			}
			if (IsNotMaterial)
			{
				IsMaterial = false;
			}
			else
			{
				CheckMaterial();
			}
		}

		public bool CheckMaterial()
		{
			if (CanBePlacedInCoinSlot())
			{
				IsMaterial = false;
				return false;
			}
			for (int RecipeIdx = 0; RecipeIdx < Recipe.NumRecipes; RecipeIdx++)
			{
				int NumRecipeItems = Main.ActiveRecipe[RecipeIdx].NumRequiredItems - 1;
				do
				{
					if (NetID == Main.ActiveRecipe[RecipeIdx].RequiredItem[NumRecipeItems].NetID)
					{
						IsMaterial = true;
						return true;
					}
				}
				while (--NumRecipeItems >= 0);
			}
			IsMaterial = false;
			return false;
		}

		public void NetDefaults(int Type, int Stack = 1)
		{
			if (Type < 0)
			{
				switch (Type)
				{
				case -1:
					SetDefaults("Gold Pickaxe");
					break;
				case -2:
					SetDefaults("Gold Broadsword");
					break;
				case -3:
					SetDefaults("Gold Shortsword");
					break;
				case -4:
					SetDefaults("Gold Axe");
					break;
				case -5:
					SetDefaults("Gold Hammer");
					break;
				case -6:
					SetDefaults("Gold Bow");
					break;
				case -7:
					SetDefaults("Silver Pickaxe");
					break;
				case -8:
					SetDefaults("Silver Broadsword");
					break;
				case -9:
					SetDefaults("Silver Shortsword");
					break;
				case -10:
					SetDefaults("Silver Axe");
					break;
				case -11:
					SetDefaults("Silver Hammer");
					break;
				case -12:
					SetDefaults("Silver Bow");
					break;
				case -13:
					SetDefaults("Copper Pickaxe");
					break;
				case -14:
					SetDefaults("Copper Broadsword");
					break;
				case -15:
					SetDefaults("Copper Shortsword");
					break;
				case -16:
					SetDefaults("Copper Axe");
					break;
				case -17:
					SetDefaults("Copper Hammer");
					break;
				case -18:
					SetDefaults("Copper Bow");
					break;
				case -19:
					SetDefaults("Blue Phasesaber");
					break;
				case -20:
					SetDefaults("Red Phasesaber");
					break;
				case -21:
					SetDefaults("Green Phasesaber");
					break;
				case -22:
					SetDefaults("Purple Phasesaber");
					break;
				case -23:
					SetDefaults("White Phasesaber");
					break;
				case -24:
					SetDefaults("Yellow Phasesaber");
					break;
				}
			}
			else
			{
				SetDefaults(Type, Stack);
			}
		}

		public void SetDefaults(int ItemType, int ItemStack = 1, bool NoMaterialCheck = false)
		{
			Active = 1;
			Owner = 8;
			Type = (short)ItemType;
			NetID = (short)ItemType;
			PrefixType = 0;
			Crit = 0;
			WornArmor = false;
			Mech = false;
			ReuseDelay = 0;
			IsMelee = false;
			IsMagic = false;
			IsRanged = false;
			PlaceStyle = 0;
			BuffTime = 0;
			BuffType = 0;
			IsMaterial = false;
			CantTouchLiquid = false;
			IsVanity = false;
			Mana = 0;
			IsWet = false;
			WetCount = 0;
			IsInLava = false;
			Channelling = false;
			OnlyBuyOnce = false;
			IsSocial = false;
			Release = 0;
			NoMelee = false;
			NoUseGraphic = false;
			LifeRegen = 0;
			ShootSpeed = 0f;
			Alpha = 0;
			Ammo = 0;
			UseAmmo = 0;
			AutoReuse = false;
			IsAccessory = false;
			AxePower = 0;
			HealMana = 0;
			BodySlot = -1;
			LegSlot = -1;
			HeadSlot = -1;
			IsPotion = false;
			Colour = default;
			IsConsumable = false;
			CreateTile = -1;
			CreateWall = -1;
			Damage = 0;
			Defense = 0;
			HammerPower = 0;
			HealLife = 0;
			Knockback = 0f;
			PickPower = 0;
			Rarity = 0;
			Scale = 1f;
			Shoot = 0;
			Stack = (short)ItemStack;
			MaxStack = (short)ItemStack;
			TileBoost = 0;
			HoldStyle = 0;
			UseStyle = 0;
			UseSound = 0;
			UseTime = 100;
			UseAnimation = 100;
			Value = 0;
			CanUseTurn = false;
			CanBuy = false;
			OwnIgnore = 8;
			OwnTime = 0;
			KeepTime = 0;
			switch ((ID)ItemType)
			{
				case ID.IRON_PICKAXE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 20;
					UseTime = 13;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 5;
					PickPower = 40;
					UseSound = 1;
					Knockback = 2f;
					Value = 2000;
					IsMelee = true;
					break;
				case ID.DIRT_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 0;
					Width = 12;
					Height = 12;
					break;
				case ID.STONE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 1;
					Width = 12;
					Height = 12;
					break;
				case ID.IRON_BROADSWORD:
					UseStyle = 1;
					CanUseTurn = false;
					UseAnimation = 21;
					UseTime = 21;
					Width = 24;
					Height = 28;
					Damage = 10;
					Knockback = 5f;
					UseSound = 1;
					Scale = 1f;
					Value = 1800;
					IsMelee = true;
					break;
				case ID.MUSHROOM:
					UseStyle = 2;
					UseSound = 2;
					CanUseTurn = false;
					UseAnimation = 17;
					UseTime = 17;
					Width = 16;
					Height = 18;
					HealLife = 15;
					MaxStack = 99;
					IsConsumable = true;
					IsPotion = true;
					Value = 25;
					break;
				case ID.IRON_SHORTSWORD:
					UseStyle = 3;
					CanUseTurn = false;
					UseAnimation = 12;
					UseTime = 12;
					Width = 24;
					Height = 28;
					Damage = 8;
					Knockback = 4f;
					Scale = 0.9f;
					UseSound = 1;
					CanUseTurn = true;
					Value = 1400;
					IsMelee = true;
					break;
				case ID.IRON_HAMMER:
					AutoReuse = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 30;
					UseTime = 20;
					HammerPower = 45;
					Width = 24;
					Height = 28;
					Damage = 7;
					Knockback = 5.5f;
					Scale = 1.2f;
					UseSound = 1;
					Value = 1600;
					IsMelee = true;
					break;
				case ID.TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					Width = 10;
					Height = 12;
					Value = 50;
					break;
				case ID.WOOD:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 30;
					Width = 8;
					Height = 10;
					break;
				case ID.IRON_AXE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 27;
					Knockback = 4.5f;
					UseTime = 19;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 5;
					AxePower = 9;
					Scale = 1.1f;
					UseSound = 1;
					Value = 1600;
					IsMelee = true;
					break;
				case ID.IRON_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 6;
					Width = 12;
					Height = 12;
					Value = 500;
					break;
				case ID.COPPER_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 7;
					Width = 12;
					Height = 12;
					Value = 250;
					break;
				case ID.GOLD_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 8;
					Width = 12;
					Height = 12;
					Value = 2000;
					break;
				case ID.SILVER_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 9;
					Width = 12;
					Height = 12;
					Value = 1000;
					break;
				case ID.COPPER_WATCH:
					Width = 24;
					Height = 28;
					IsAccessory = true;
					Value = 1000;
					break;
				case ID.SILVER_WATCH:
					Width = 24;
					Height = 28;
					IsAccessory = true;
					Value = 5000;
					break;
				case ID.GOLD_WATCH:
					Width = 24;
					Height = 28;
					IsAccessory = true;
					Rarity = 1;
					Value = 10000;
					break;
				case ID.DEPTH_METER:
					Width = 24;
					Height = 18;
					IsAccessory = true;
					Rarity = 1;
					Value = 10000;
					break;
				case ID.GOLD_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 6000;
					break;
				case ID.COPPER_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 750;
					break;
				case ID.SILVER_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 3000;
					break;
				case ID.IRON_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 1500;
					break;
				case ID.GEL:
					Width = 10;
					Height = 12;
					MaxStack = 250;
					Alpha = 175;
					Ammo = 23;
					Colour = new Color(0, 80, 255, 100);
					Value = 5;
					break;
				case ID.WOODEN_SWORD:
					UseStyle = 1;
					CanUseTurn = false;
					UseAnimation = 25;
					Width = 24;
					Height = 28;
					Damage = 7;
					Knockback = 4f;
					Scale = 0.95f;
					UseSound = 1;
					Value = 100;
					IsMelee = true;
					break;
				case ID.WOODEN_DOOR:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 10;
					Width = 14;
					Height = 28;
					Value = 200;
					break;
				case ID.STONE_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 1;
					Width = 12;
					Height = 12;
					break;
				case ID.ACORN:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 20;
					Width = 18;
					Height = 18;
					Value = 10;
					break;
				case ID.LESSER_HEALING_POTION:
					UseSound = 3;
					HealLife = 50;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					IsPotion = true;
					Value = 300;
					break;
				case ID.LIFE_CRYSTAL:
					MaxStack = 99;
					IsConsumable = true;
					Width = 18;
					Height = 18;
					UseStyle = 4;
					UseTime = 30;
					UseSound = 4;
					UseAnimation = 30;
					Rarity = 2;
					Value = 75000;
					break;
				case ID.DIRT_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 16;
					Width = 12;
					Height = 12;
					break;
				case ID.BOTTLE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 13;
					Width = 16;
					Height = 24;
					Value = 20;
					break;
				case ID.WOODEN_TABLE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 14;
					Width = 26;
					Height = 20;
					Value = 300;
					break;
				case ID.FURNACE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 17;
					Width = 26;
					Height = 24;
					Value = 300;
					break;
				case ID.WOODEN_CHAIR:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 15;
					Width = 12;
					Height = 30;
					Value = 150;
					break;
				case ID.IRON_ANVIL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 16;
					Width = 28;
					Height = 14;
					Value = 5000;
					break;
				case ID.WORK_BENCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 18;
					Width = 28;
					Height = 14;
					Value = 150;
					break;
				case ID.GOGGLES:
					Width = 28;
					Height = 12;
					Defense = 1;
					HeadSlot = 10;
					Rarity = 1;
					Value = 1000;
					break;
				case ID.LENS:
					Width = 12;
					Height = 20;
					MaxStack = 99;
					Value = 500;
					break;
				case ID.WOODEN_BOW:
					UseStyle = 5;
					UseAnimation = 30;
					UseTime = 30;
					Width = 12;
					Height = 28;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 4;
					ShootSpeed = 6.1f;
					NoMelee = true;
					Value = 100;
					IsRanged = true;
					break;
				case ID.WOODEN_ARROW:
					ShootSpeed = 3f;
					Shoot = 1;
					Damage = 4;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 2f;
					Value = 10;
					IsRanged = true;
					break;
				case ID.FLAMING_ARROW:
					ShootSpeed = 3.5f;
					Shoot = 2;
					Damage = 6;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 2f;
					Value = 15;
					IsRanged = true;
					break;
				case ID.SHURIKEN:
					UseStyle = 1;
					ShootSpeed = 9f;
					Shoot = 3;
					Damage = 10;
					Width = 18;
					Height = 20;
					MaxStack = 250;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 20;
					IsRanged = true;
					break;
				case ID.SUSPICIOUS_LOOKING_EYE:
					UseStyle = 4;
					Width = 22;
					Height = 14;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					break;
				case ID.SUSPICIOUS_LOOKING_SKULL:
					UseStyle = 4;
					Width = 26;
					Height = 26;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					break;
				case ID.DEMON_BOW:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 25;
					Width = 12;
					Height = 28;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 14;
					ShootSpeed = 6.7f;
					Knockback = 1f;
					Alpha = 30;
					Rarity = 1;
					NoMelee = true;
					Value = 18000;
					IsRanged = true;
					break;
				case ID.WAR_AXE_OF_THE_NIGHT:
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 30;
					Knockback = 6f;
					UseTime = 15;
					Width = 24;
					Height = 28;
					Damage = 20;
					AxePower = 15;
					Scale = 1.2f;
					UseSound = 1;
					Rarity = 1;
					Value = 13500;
					IsMelee = true;
					break;
				case ID.LIGHTS_BANE:
					UseStyle = 1;
					UseAnimation = 20;
					Knockback = 5f;
					Width = 24;
					Height = 28;
					Damage = 17;
					Scale = 1.1f;
					UseSound = 1;
					Rarity = 1;
					Value = 13500;
					IsMelee = true;
					break;
				case ID.UNHOLY_ARROW:
					ShootSpeed = 3.4f;
					Shoot = 4;
					Damage = 8;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 3f;
					Alpha = 30;
					Rarity = 1;
					Value = 40;
					IsRanged = true;
					break;
				case ID.CHEST:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 21;
					Width = 26;
					Height = 22;
					Value = 500;
					break;
				case ID.BAND_OF_REGENERATION:
					Width = 22;
					Height = 22;
					IsAccessory = true;
					LifeRegen = 1;
					Rarity = 1;
					Value = 50000;
					break;
				case ID.MAGIC_MIRROR:
					Mana = 20;
					CanUseTurn = true;
					Width = 20;
					Height = 20;
					UseStyle = 4;
					UseTime = 90;
					UseSound = 6;
					UseAnimation = 90;
					Rarity = 1;
					Value = 50000;
					break;
				case ID.JESTERS_ARROW:
					ShootSpeed = 0.5f;
					Shoot = 5;
					Damage = 9;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 4f;
					Rarity = 1;
					Value = 100;
					IsRanged = true;
					break;
				case ID.ANGEL_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 1;
					break;
				case ID.CLOUD_IN_A_BOTTLE:
					Width = 16;
					Height = 24;
					IsAccessory = true;
					Rarity = 1;
					Value = 50000;
					break;
				case ID.HERMES_BOOTS:
					Width = 28;
					Height = 24;
					IsAccessory = true;
					Rarity = 1;
					Value = 50000;
					break;
				case ID.ENCHANTED_BOOMERANG:
					NoMelee = true;
					UseStyle = 1;
					ShootSpeed = 10f;
					Shoot = 6;
					Damage = 13;
					Knockback = 8f;
					Width = 14;
					Height = 28;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					Rarity = 1;
					Value = 50000;
					IsMelee = true;
					break;
				case ID.DEMONITE_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 22;
					Width = 12;
					Height = 12;
					Rarity = 1;
					Value = 4000;
					break;
				case ID.DEMONITE_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Rarity = 1;
					Value = 16000;
					break;
				case ID.HEART:
					Width = 12;
					Height = 12;
					break;
				case ID.CORRUPT_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 23;
					Width = 14;
					Height = 14;
					Value = 500;
					break;
				case ID.VILE_MUSHROOM:
					Width = 16;
					Height = 18;
					MaxStack = 99;
					Value = 50;
					break;
				case ID.EBONSTONE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 25;
					Width = 12;
					Height = 12;
					break;
				case ID.GRASS_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 2;
					Width = 14;
					Height = 14;
					Value = 20;
					break;
				case ID.SUNFLOWER:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 27;
					Width = 26;
					Height = 26;
					Value = 200;
					break;
				case ID.VILETHORN:
					Mana = 12;
					Damage = 8;
					UseStyle = 1;
					ShootSpeed = 32f;
					Shoot = 7;
					Width = 26;
					Height = 28;
					UseSound = 8;
					UseAnimation = 30;
					UseTime = 30;
					Rarity = 1;
					NoMelee = true;
					Knockback = 1f;
					Value = 10000;
					IsMagic = true;
					break;
				case ID.STARFURY:
					AutoReuse = true;
					Mana = 16;
					Knockback = 5f;
					Alpha = 100;
					Colour = new Color(150, 150, 150, 0);
					Damage = 16;
					UseStyle = 1;
					Scale = 1.15f;
					ShootSpeed = 12f;
					Shoot = 9;
					Width = 14;
					Height = 28;
					UseSound = 9;
					UseAnimation = 25;
					UseTime = 10;
					Rarity = 1;
					Value = 50000;
					IsMagic = true;
					break;
				case ID.PURIFICATION_POWDER:
					UseStyle = 1;
					ShootSpeed = 4f;
					Shoot = 10;
					Width = 16;
					Height = 24;
					MaxStack = 99;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoMelee = true;
					Value = 75;
					break;
				case ID.VILE_POWDER:
					Damage = 0;
					UseStyle = 1;
					ShootSpeed = 4f;
					Shoot = 11;
					Width = 16;
					Height = 24;
					MaxStack = 99;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoMelee = true;
					Value = 100;
					break;
				case ID.ROTTEN_CHUNK:
					Width = 18;
					Height = 20;
					MaxStack = 99;
					Value = 10;
					break;
				case ID.WORM_TOOTH:
					Width = 8;
					Height = 20;
					MaxStack = 99;
					Value = 100;
					break;
				case ID.WORM_FOOD:
					UseStyle = 4;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					Width = 28;
					Height = 28;
					MaxStack = 20;
					break;
				case ID.COPPER_COIN:
					Width = 10;
					Height = 12;
					MaxStack = 100;
					Value = 5;
					break;
				case ID.SILVER_COIN:
					Width = 10;
					Height = 12;
					MaxStack = 100;
					Value = 500;
					break;
				case ID.GOLD_COIN:
					Width = 10;
					Height = 12;
					MaxStack = 100;
					Value = 50000;
					break;
				case ID.PLATINUM_COIN:
					Width = 10;
					Height = 12;
					MaxStack = 100;
					Value = 5000000;
					break;
				case ID.FALLEN_STAR:
					Width = 18;
					Height = 20;
					MaxStack = 100;
					Alpha = 75;
					Ammo = 15;
					Value = 500;
					UseStyle = 4;
					UseSound = 4;
					CanUseTurn = false;
					UseAnimation = 17;
					UseTime = 17;
					IsConsumable = true;
					Rarity = 1;
					break;
				case ID.COPPER_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 1;
					LegSlot = 1;
					Value = 750;
					break;
				case ID.IRON_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 2;
					LegSlot = 2;
					Value = 3000;
					break;
				case ID.SILVER_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 3;
					LegSlot = 3;
					Value = 7500;
					break;
				case ID.GOLD_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 4;
					LegSlot = 4;
					Value = 15000;
					break;
				case ID.COPPER_CHAINMAIL:
					Width = 18;
					Height = 18;
					Defense = 2;
					BodySlot = 1;
					Value = 1000;
					break;
				case ID.IRON_CHAINMAIL:
					Width = 18;
					Height = 18;
					Defense = 3;
					BodySlot = 2;
					Value = 4000;
					break;
				case ID.SILVER_CHAINMAIL:
					Width = 18;
					Height = 18;
					Defense = 4;
					BodySlot = 3;
					Value = 10000;
					break;
				case ID.GOLD_CHAINMAIL:
					Width = 18;
					Height = 18;
					Defense = 5;
					BodySlot = 4;
					Value = 20000;
					break;
				case ID.GRAPPLING_HOOK:
					NoUseGraphic = true;
					Damage = 0;
					Knockback = 7f;
					UseStyle = 5;
					ShootSpeed = 11f;
					Shoot = 13;
					Width = 18;
					Height = 28;
					UseSound = 1;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 1;
					NoMelee = true;
					Value = 20000;
					break;
				case ID.IRON_CHAIN:
					Width = 14;
					Height = 20;
					MaxStack = 99;
					Value = 1000;
					break;
				case ID.SHADOW_SCALE:
					Width = 14;
					Height = 18;
					MaxStack = 99;
					Rarity = 1;
					Value = 500;
					break;
				case ID.PIGGY_BANK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 29;
					Width = 20;
					Height = 12;
					Value = 10000;
					break;
				case ID.MINING_HELMET:
					Width = 22;
					Height = 16;
					Defense = 1;
					HeadSlot = 11;
					Rarity = 1;
					Value = 80000;
					break;
				case ID.COPPER_HELMET:
					Width = 18;
					Height = 18;
					Defense = 1;
					HeadSlot = 1;
					Value = 1250;
					break;
				case ID.IRON_HELMET:
					Width = 18;
					Height = 18;
					Defense = 2;
					HeadSlot = 2;
					Value = 5000;
					break;
				case ID.SILVER_HELMET:
					Width = 18;
					Height = 18;
					Defense = 3;
					HeadSlot = 3;
					Value = 12500;
					break;
				case ID.GOLD_HELMET:
					Width = 18;
					Height = 18;
					Defense = 4;
					HeadSlot = 4;
					Value = 25000;
					break;
				case ID.WOOD_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 4;
					Width = 12;
					Height = 12;
					break;
				case ID.WOOD_PLATFORM:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 19;
					Width = 8;
					Height = 10;
					break;
				case ID.FLINTLOCK_PISTOL:
					UseStyle = 5;
					UseAnimation = 16;
					UseTime = 16;
					Width = 24;
					Height = 28;
					Shoot = 14;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 10;
					ShootSpeed = 5f;
					NoMelee = true;
					Value = 50000;
					Scale = 0.9f;
					Rarity = 1;
					IsRanged = true;
					break;
				case ID.MUSKET:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 43;
					UseTime = 43;
					Width = 44;
					Height = 14;
					Shoot = 10;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 23;
					ShootSpeed = 8f;
					NoMelee = true;
					Value = 100000;
					Knockback = 4f;
					Rarity = 1;
					IsRanged = true;
					break;
				case ID.MUSKET_BALL:
					ShootSpeed = 4f;
					Shoot = 14;
					Damage = 7;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 14;
					Knockback = 2f;
					Value = 7;
					IsRanged = true;
					break;
				case ID.MINISHARK:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 8;
					UseTime = 8;
					Width = 50;
					Height = 18;
					Shoot = 10;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 6;
					ShootSpeed = 7f;
					NoMelee = true;
					Value = 350000;
					Rarity = 2;
					IsRanged = true;
					break;
				case ID.IRON_BOW:
					UseStyle = 5;
					UseAnimation = 28;
					UseTime = 28;
					Width = 12;
					Height = 28;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 8;
					ShootSpeed = 6.6f;
					NoMelee = true;
					Value = 1400;
					IsRanged = true;
					break;
				case ID.SHADOW_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 6;
					LegSlot = 5;
					Rarity = 1;
					Value = 22500;
					break;
				case ID.SHADOW_SCALEMAIL:
					Width = 18;
					Height = 18;
					Defense = 7;
					BodySlot = 5;
					Rarity = 1;
					Value = 30000;
					break;
				case ID.SHADOW_HELMET:
					Width = 18;
					Height = 18;
					Defense = 6;
					HeadSlot = 5;
					Rarity = 1;
					Value = 37500;
					break;
				case ID.NIGHTMARE_PICKAXE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 20;
					UseTime = 15;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 9;
					PickPower = 65;
					UseSound = 1;
					Knockback = 3f;
					Rarity = 1;
					Value = 18000;
					Scale = 1.15f;
					IsMelee = true;
					break;
				case ID.THE_BREAKER:
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 45;
					UseTime = 19;
					HammerPower = 55;
					Width = 24;
					Height = 28;
					Damage = 24;
					Knockback = 6f;
					Scale = 1.3f;
					UseSound = 1;
					Rarity = 1;
					Value = 15000;
					IsMelee = true;
					break;
				case ID.CANDLE:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 33;
					Width = 8;
					Height = 18;
					HoldStyle = 1;
					break;
				case ID.COPPER_CHANDELIER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 34;
					Width = 26;
					Height = 26;
					break;
				case ID.SILVER_CHANDELIER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 35;
					Width = 26;
					Height = 26;
					break;
				case ID.GOLD_CHANDELIER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 36;
					Width = 26;
					Height = 26;
					break;
				case ID.MANA_CRYSTAL:
					MaxStack = 99;
					IsConsumable = true;
					Width = 18;
					Height = 18;
					UseStyle = 4;
					UseTime = 30;
					UseSound = 29;
					UseAnimation = 30;
					Rarity = 2;
					break;
				case ID.LESSER_MANA_POTION:
					UseSound = 3;
					HealMana = 50;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 20;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					Value = 100;
					break;
				case ID.BAND_OF_STARPOWER:
					Width = 22;
					Height = 22;
					IsAccessory = true;
					Rarity = 1;
					Value = 50000;
					break;
				case ID.FLOWER_OF_FIRE:
					Mana = 17;
					Damage = 44;
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 15;
					Width = 26;
					Height = 28;
					UseSound = 20;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Knockback = 5.5f;
					Value = 10000;
					IsMagic = true;
					break;
				case ID.MAGIC_MISSILE:
					Mana = 10;
					Channelling = true;
					Damage = 22;
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 16;
					Width = 26;
					Height = 28;
					UseSound = 9;
					UseAnimation = 17;
					UseTime = 17;
					Rarity = 2;
					NoMelee = true;
					Knockback = 5f;
					TileBoost = 64;
					Value = 10000;
					IsMagic = true;
					break;
				case ID.DIRT_ROD:
					Mana = 5;
					Channelling = true;
					Damage = 0;
					UseStyle = 1;
					Shoot = 17;
					Width = 26;
					Height = 28;
					UseSound = 8;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 1;
					NoMelee = true;
					Knockback = 5f;
					Value = 200000;
					break;
				case ID.ORB_OF_LIGHT:
					Mana = 40;
					Channelling = true;
					Damage = 0;
					UseStyle = 4;
					Shoot = 18;
					Width = 24;
					Height = 24;
					UseSound = 8;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 1;
					NoMelee = true;
					Value = 10000;
					BuffType = (int)Buff.ID.LIGHT_ORB;
					BuffTime = 18000;
					break;
				case ID.METEORITE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 37;
					Width = 12;
					Height = 12;
					Value = 1000;
					break;
				case ID.METEORITE_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Rarity = 1;
					Value = 7000;
					break;
				case ID.HOOK:
					MaxStack = 99;
					Width = 18;
					Height = 18;
					Value = 1000;
					break;
				case ID.FLAMARANG:
					NoMelee = true;
					UseStyle = 1;
					ShootSpeed = 11f;
					Shoot = 19;
					Damage = 32;
					Knockback = 8f;
					Width = 14;
					Height = 28;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					Rarity = 3;
					Value = 100000;
					IsMelee = true;
					break;
				case ID.MOLTEN_FURY:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 25;
					Width = 14;
					Height = 32;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 29;
					ShootSpeed = 8f;
					Knockback = 2f;
					Alpha = 30;
					Rarity = 3;
					NoMelee = true;
					Scale = 1.1f;
					Value = 27000;
					IsRanged = true;
					break;
				case ID.SHARANGA:
					UseStyle = 5;
					UseAnimation = 20;
					UseTime = 20;
					Width = 14;
					Height = 32;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 35;
					ShootSpeed = 10f;
					Knockback = 2.3f;
					Alpha = 30;
					Rarity = 5;
					NoMelee = true;
					Scale = 1.1f;
					Value = 60000;
					IsRanged = true;
					break;
				case ID.FIERY_GREATSWORD:
					UseStyle = 1;
					UseAnimation = 34;
					Knockback = 6.5f;
					Width = 24;
					Height = 28;
					Damage = 36;
					Scale = 1.3f;
					UseSound = 1;
					Rarity = 3;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.MOLTEN_PICKAXE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 25;
					UseTime = 25;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 12;
					PickPower = 100;
					Scale = 1.15f;
					UseSound = 1;
					Knockback = 2f;
					Rarity = 3;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.METEOR_HELMET:
					Width = 18;
					Height = 18;
					Defense = 3;
					HeadSlot = 6;
					Rarity = 1;
					Value = 45000;
					break;
				case ID.METEOR_SUIT:
					Width = 18;
					Height = 18;
					Defense = 4;
					BodySlot = 6;
					Rarity = 1;
					Value = 30000;
					break;
				case ID.METEOR_LEGGINGS:
					Width = 18;
					Height = 18;
					Defense = 3;
					LegSlot = 6;
					Rarity = 1;
					Value = 30000;
					break;
				case ID.BOTTLED_WATER:
					UseSound = 3;
					HealLife = 20;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					IsPotion = true;
					Value = 20;
					break;
				case ID.SPACE_GUN:
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 19;
					UseTime = 19;
					Width = 24;
					Height = 28;
					Shoot = 20;
					Mana = 8;
					UseSound = 12;
					Knockback = 0.5f;
					Damage = 17;
					ShootSpeed = 10f;
					NoMelee = true;
					Scale = 0.8f;
					Rarity = 1;
					IsMagic = true;
					Value = 20000;
					break;
				case ID.ROCKET_BOOTS:
					Width = 28;
					Height = 24;
					IsAccessory = true;
					Rarity = 3;
					Value = 50000;
					break;
				case ID.GRAY_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 38;
					Width = 12;
					Height = 12;
					break;
				case ID.GRAY_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 5;
					Width = 12;
					Height = 12;
					break;
				case ID.RED_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 39;
					Width = 12;
					Height = 12;
					break;
				case ID.RED_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 6;
					Width = 12;
					Height = 12;
					break;
				case ID.CLAY_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 40;
					Width = 12;
					Height = 12;
					break;
				case ID.BLUE_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 41;
					Width = 12;
					Height = 12;
					break;
				case ID.BLUE_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 17;
					Width = 12;
					Height = 12;
					break;
				case ID.CHAIN_LANTERN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 42;
					Width = 12;
					Height = 28;
					break;
				case ID.GREEN_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 43;
					Width = 12;
					Height = 12;
					break;
				case ID.GREEN_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 18;
					Width = 12;
					Height = 12;
					break;
				case ID.PINK_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 44;
					Width = 12;
					Height = 12;
					break;
				case ID.PINK_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 19;
					Width = 12;
					Height = 12;
					break;
				case ID.GOLD_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 45;
					Width = 12;
					Height = 12;
					break;
				case ID.GOLD_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 10;
					Width = 12;
					Height = 12;
					break;
				case ID.SILVER_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 46;
					Width = 12;
					Height = 12;
					break;
				case ID.SILVER_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 11;
					Width = 12;
					Height = 12;
					break;
				case ID.COPPER_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 47;
					Width = 12;
					Height = 12;
					break;
				case ID.COPPER_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 12;
					Width = 12;
					Height = 12;
					break;
				case ID.SPIKE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 48;
					Width = 12;
					Height = 12;
					break;
				case ID.WATER_CANDLE:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 49;
					Width = 8;
					Height = 18;
					HoldStyle = 1;
					break;
				case ID.BOOK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 50;
					Width = 24;
					Height = 28;
					break;
				case ID.COBWEB:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 51;
					Width = 20;
					Height = 24;
					Alpha = 100;
					break;
				case ID.NECRO_HELMET:
					Width = 18;
					Height = 18;
					Defense = 5;
					HeadSlot = 7;
					Rarity = 2;
					Value = 45000;
					break;
				case ID.NECRO_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 6;
					BodySlot = 7;
					Rarity = 2;
					Value = 30000;
					break;
				case ID.NECRO_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 5;
					LegSlot = 7;
					Rarity = 2;
					Value = 30000;
					break;
				case ID.BONE:
					MaxStack = 99;
					IsConsumable = true;
					Width = 12;
					Height = 14;
					Value = 50;
					UseAnimation = 12;
					UseTime = 12;
					UseStyle = 1;
					UseSound = 1;
					ShootSpeed = 8f;
					NoUseGraphic = true;
					Damage = 22;
					Knockback = 4f;
					Shoot = 21;
					IsRanged = true;
					break;
				case ID.MURAMASA:
					AutoReuse = true;
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 20;
					Width = 40;
					Height = 40;
					Damage = 18;
					Scale = 1.1f;
					UseSound = 1;
					Rarity = 2;
					Value = 27000;
					Knockback = 1f;
					IsMelee = true;
					break;
				case ID.COBALT_SHIELD:
					Width = 24;
					Height = 28;
					Rarity = 2;
					Value = 27000;
					IsAccessory = true;
					Defense = 1;
					break;
				case ID.AQUA_SCEPTER:
					Mana = 7;
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 16;
					UseTime = 8;
					Knockback = 5f;
					Width = 38;
					Height = 10;
					Damage = 14;
					Scale = 1f;
					Shoot = 22;
					ShootSpeed = 11f;
					UseSound = 13;
					Rarity = 2;
					Value = 27000;
					IsMagic = true;
					break;
				case ID.LUCKY_HORSESHOE:
					Width = 20;
					Height = 22;
					Rarity = 1;
					Value = 27000;
					IsAccessory = true;
					break;
				case ID.SHINY_RED_BALLOON:
					Width = 14;
					Height = 28;
					Rarity = 1;
					Value = 27000;
					IsAccessory = true;
					break;
				case ID.HARPOON:
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 30;
					UseTime = 30;
					Knockback = 6f;
					Width = 30;
					Height = 10;
					Damage = 25;
					Scale = 1.1f;
					Shoot = 23;
					ShootSpeed = 11f;
					UseSound = 10;
					Rarity = 2;
					Value = 27000;
					IsRanged = true;
					break;
				case ID.SPIKY_BALL:
					UseStyle = 1;
					ShootSpeed = 5f;
					Shoot = 24;
					Knockback = 1f;
					Damage = 15;
					Width = 10;
					Height = 10;
					MaxStack = 250;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 80;
					IsRanged = true;
					break;
				case ID.BALL_O_HURT:
					UseStyle = 5;
					UseAnimation = 45;
					UseTime = 45;
					Knockback = 6.5f;
					Width = 30;
					Height = 10;
					Damage = 15;
					Scale = 1.1f;
					NoUseGraphic = true;
					Shoot = 25;
					ShootSpeed = 12f;
					UseSound = 1;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					Channelling = true;
					NoMelee = true;
					break;
				case ID.BLUE_MOON:
					UseStyle = 5;
					UseAnimation = 45;
					UseTime = 45;
					Knockback = 7f;
					Width = 30;
					Height = 10;
					Damage = 23;
					Scale = 1.1f;
					NoUseGraphic = true;
					Shoot = 26;
					ShootSpeed = 12f;
					UseSound = 1;
					Rarity = 2;
					Value = 27000;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.HANDGUN:
					AutoReuse = false;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 12;
					Width = 24;
					Height = 24;
					Shoot = 14;
					Knockback = 3f;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 14;
					ShootSpeed = 10f;
					NoMelee = true;
					Value = 50000;
					Scale = 0.75f;
					Rarity = 2;
					IsRanged = true;
					break;
				case ID.WATER_BOLT:
					AutoReuse = true;
					Rarity = 2;
					Mana = 14;
					UseSound = 21;
					UseStyle = 5;
					Damage = 17;
					UseAnimation = 17;
					UseTime = 17;
					Width = 24;
					Height = 28;
					Shoot = 27;
					Scale = 0.9f;
					ShootSpeed = 4.5f;
					Knockback = 5f;
					IsMagic = true;
					Value = 50000;
					break;
				case ID.BOMB:
					UseStyle = 1;
					ShootSpeed = 5f;
					Shoot = 28;
					Width = 20;
					Height = 20;
					MaxStack = 50;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 25;
					UseTime = 25;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 500;
					Damage = 0;
					break;
				case ID.DYNAMITE:
					UseStyle = 1;
					ShootSpeed = 4f;
					Shoot = 29;
					Width = 8;
					Height = 28;
					MaxStack = 5;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 40;
					UseTime = 40;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 5000;
					Rarity = 1;
					break;
				case ID.GRENADE:
					UseStyle = 5;
					ShootSpeed = 5.5f;
					Shoot = 30;
					Width = 20;
					Height = 20;
					MaxStack = 99;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 45;
					UseTime = 45;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 400;
					Damage = 60;
					Knockback = 8f;
					IsRanged = true;
					break;
				case ID.SAND_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 53;
					Width = 12;
					Height = 12;
					Ammo = 42;
					break;
				case ID.GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 54;
					Width = 12;
					Height = 12;
					break;
				case ID.SIGN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 55;
					Width = 28;
					Height = 28;
					break;
				case ID.ASH_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 57;
					Width = 12;
					Height = 12;
					break;
				case ID.OBSIDIAN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 56;
					Width = 12;
					Height = 12;
					break;
				case ID.HELLSTONE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 58;
					Width = 12;
					Height = 12;
					Rarity = 2;
					break;
				case ID.HELLSTONE_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Rarity = 2;
					Value = 20000;
					break;
				case ID.MUD_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 59;
					Width = 12;
					Height = 12;
					break;
				case ID.AMETHYST:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 1875;
					break;
				case ID.TOPAZ:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 3750;
					break;
				case ID.SAPPHIRE:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 5625;
					break;
				case ID.EMERALD:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 7500;
					break;
				case ID.RUBY:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 11250;
					break;
				case ID.DIAMOND:
					MaxStack = 99;
					Alpha = 50;
					Width = 10;
					Height = 14;
					Value = 15000;
					break;
				case ID.GLOWING_MUSHROOM:
					UseStyle = 2;
					UseSound = 2;
					CanUseTurn = false;
					UseAnimation = 17;
					UseTime = 17;
					Width = 16;
					Height = 18;
					HealLife = 25;
					MaxStack = 99;
					IsConsumable = true;
					IsPotion = true;
					Value = 50;
					break;
				case ID.STAR:
					Width = 12;
					Height = 12;
					break;
				case ID.IVY_WHIP:
					NoUseGraphic = true;
					Damage = 0;
					Knockback = 7f;
					UseStyle = 5;
					ShootSpeed = 13f;
					Shoot = 32;
					Width = 18;
					Height = 28;
					UseSound = 1;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 20000;
					break;
				case ID.BREATHING_REED:
					Width = 44;
					Height = 44;
					Rarity = 1;
					Value = 10000;
					HoldStyle = 2;
					break;
				case ID.FLIPPER:
					Width = 28;
					Height = 28;
					Rarity = 1;
					Value = 10000;
					IsAccessory = true;
					break;
				case ID.HEALING_POTION:
					UseSound = 3;
					HealLife = 100;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					Rarity = 1;
					IsPotion = true;
					Value = 1000;
					break;
				case ID.MANA_POTION:
					UseSound = 3;
					HealMana = 100;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 50;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					Rarity = 1;
					Value = 250;
					break;
				case ID.BLADE_OF_GRASS:
					UseStyle = 1;
					UseAnimation = 30;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 28;
					Scale = 1.4f;
					UseSound = 1;
					Rarity = 3;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.THORN_CHAKRAM:
					NoMelee = true;
					UseStyle = 1;
					ShootSpeed = 11f;
					Shoot = 33;
					Damage = 25;
					Knockback = 8f;
					Width = 14;
					Height = 28;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					Rarity = 3;
					Value = 50000;
					IsMelee = true;
					break;
				case ID.OBSIDIAN_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 75;
					Width = 12;
					Height = 12;
					break;
				case ID.OBSIDIAN_SKULL:
					Width = 20;
					Height = 22;
					Rarity = 2;
					Value = 27000;
					IsAccessory = true;
					Defense = 1;
					break;
				case ID.MUSHROOM_GRASS_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 70;
					Width = 14;
					Height = 14;
					Value = 150;
					break;
				case ID.JUNGLE_GRASS_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 60;
					Width = 14;
					Height = 14;
					Value = 150;
					break;
				case ID.WOODEN_HAMMER:
					AutoReuse = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 37;
					UseTime = 25;
					HammerPower = 25;
					Width = 24;
					Height = 28;
					Damage = 2;
					Knockback = 5.5f;
					Scale = 1.2f;
					UseSound = 1;
					TileBoost = -1;
					Value = 50;
					IsMelee = true;
					break;
				case ID.STAR_CANNON:
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 12;
					Width = 50;
					Height = 18;
					Shoot = 12;
					UseAmmo = 15;
					UseSound = 9;
					Damage = 55;
					ShootSpeed = 14f;
					NoMelee = true;
					Value = 500000;
					Rarity = 2;
					IsRanged = true;
					break;
				case ID.BLUE_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.RED_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.GREEN_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.PURPLE_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.WHITE_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.YELLOW_PHASEBLADE:
					UseStyle = 1;
					UseAnimation = 25;
					Knockback = 3f;
					Width = 40;
					Height = 40;
					Damage = 21;
					Scale = 1f;
					UseSound = 15;
					Rarity = 1;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.METEOR_HAMAXE:
					CanUseTurn = true;
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 30;
					UseTime = 16;
					HammerPower = 60;
					AxePower = 20;
					Width = 24;
					Height = 28;
					Damage = 20;
					Knockback = 7f;
					Scale = 1.2f;
					UseSound = 1;
					Rarity = 1;
					Value = 15000;
					IsMelee = true;
					break;
				case ID.EMPTY_BUCKET:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					Width = 20;
					Height = 20;
					HeadSlot = 13;
					Defense = 1;
					break;
				case ID.WATER_BUCKET:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					Width = 20;
					Height = 20;
					break;
				case ID.LAVA_BUCKET:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					Width = 20;
					Height = 20;
					break;
				case ID.JUNGLE_ROSE:
					Width = 20;
					Height = 20;
					Value = 100;
					HeadSlot = 23;
					IsVanity = true;
					break;
				case ID.STINGER:
					Width = 16;
					Height = 18;
					MaxStack = 99;
					Value = 200;
					break;
				case ID.VINE:
					Width = 14;
					Height = 20;
					MaxStack = 99;
					Value = 1000;
					break;
				case ID.FERAL_CLAWS:
					Width = 20;
					Height = 20;
					IsAccessory = true;
					Rarity = 3;
					Value = 50000;
					break;
				case ID.ANKLET_OF_THE_WIND:
					Width = 20;
					Height = 20;
					IsAccessory = true;
					Rarity = 3;
					Value = 50000;
					break;
				case ID.STAFF_OF_REGROWTH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 25;
					UseTime = 13;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 7;
					CreateTile = 2;
					Scale = 1.2f;
					UseSound = 1;
					Knockback = 3f;
					Rarity = 3;
					Value = 2000;
					IsMelee = true;
					break;
				case ID.HELLSTONE_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 76;
					Width = 12;
					Height = 12;
					break;
				case ID.WHOOPIE_CUSHION:
					Width = 18;
					Height = 18;
					CanUseTurn = true;
					UseTime = 30;
					UseAnimation = 30;
					NoUseGraphic = true;
#if (!VERSION_INITIAL || IS_PATCHED)
					NoMelee = true;
#endif
					UseStyle = 10;
					UseSound = 16;
					Rarity = 2;
					Value = 100;
					break;
				case ID.SHACKLE:
					Width = 20;
					Height = 20;
					Rarity = 1;
					Value = 1500;
					IsAccessory = true;
					Defense = 1;
					break;
				case ID.MOLTEN_HAMAXE:
					CanUseTurn = true;
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 27;
					UseTime = 14;
					HammerPower = 70;
					AxePower = 30;
					Width = 24;
					Height = 28;
					Damage = 20;
					Knockback = 7f;
					Scale = 1.4f;
					UseSound = 1;
					Rarity = 3;
					Value = 15000;
					IsMelee = true;
					break;
				case ID.FLAMELASH:
					Mana = 16;
					Channelling = true;
					Damage = 34;
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 34;
					Width = 26;
					Height = 28;
					UseSound = 20;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Knockback = 6.5f;
					TileBoost = 64;
					Value = 10000;
					IsMagic = true;
					break;
				case ID.PHOENIX_BLASTER:
					AutoReuse = false;
					UseStyle = 5;
					UseAnimation = 11;
					UseTime = 11;
					Width = 24;
					Height = 22;
					Shoot = 14;
					Knockback = 2f;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 23;
					ShootSpeed = 13f;
					NoMelee = true;
					Value = 50000;
					Scale = 0.75f;
					Rarity = 3;
					IsRanged = true;
					break;
				case ID.SUNFURY:
					NoMelee = true;
					UseStyle = 5;
					UseAnimation = 45;
					UseTime = 45;
					Knockback = 7f;
					Width = 30;
					Height = 10;
					Damage = 33;
					Scale = 1.1f;
					NoUseGraphic = true;
					Shoot = 35;
					ShootSpeed = 12f;
					UseSound = 1;
					Rarity = 3;
					Value = 27000;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.HELLFORGE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 77;
					Width = 26;
					Height = 24;
					Value = 3000;
					break;
				case ID.CLAY_POT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 78;
					Width = 14;
					Height = 14;
					Value = 100;
					break;
				case ID.NATURES_GIFT:
					Width = 20;
					Height = 22;
					Rarity = 3;
					Value = 27000;
					IsAccessory = true;
					break;
				case ID.BED:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 79;
					Width = 28;
					Height = 20;
					Value = 2000;
					break;
				case ID.SILK:
					MaxStack = 99;
					Width = 22;
					Height = 22;
					Value = 1000;
					break;
				case ID.LESSER_RESTORATION_POTION:
					UseSound = 3;
					HealMana = 50;
					HealLife = 50;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 20;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					IsPotion = true;
					Value = 2000;
					break;
				case ID.RESTORATION_POTION:
					UseSound = 3;
					HealMana = 100;
					HealLife = 100;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 20;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					IsPotion = true;
					Value = 4000;
					break;
				case ID.JUNGLE_HAT:
					Width = 18;
					Height = 18;
					Defense = 4;
					HeadSlot = 8;
					Rarity = 3;
					Value = 45000;
					break;
				case ID.JUNGLE_SHIRT:
					Width = 18;
					Height = 18;
					Defense = 5;
					BodySlot = 8;
					Rarity = 3;
					Value = 30000;
					break;
				case ID.JUNGLE_PANTS:
					Width = 18;
					Height = 18;
					Defense = 4;
					LegSlot = 8;
					Rarity = 3;
					Value = 30000;
					break;
				case ID.MOLTEN_HELMET:
					Width = 18;
					Height = 18;
					Defense = 8;
					HeadSlot = 9;
					Rarity = 3;
					Value = 45000;
					break;
				case ID.MOLTEN_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 9;
					BodySlot = 9;
					Rarity = 3;
					Value = 30000;
					break;
				case ID.MOLTEN_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 8;
					LegSlot = 9;
					Rarity = 3;
					Value = 30000;
					break;
				case ID.METEOR_SHOT:
					ShootSpeed = 3f;
					Shoot = 36;
					Damage = 9;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 14;
					Knockback = 1f;
					Value = 8;
					Rarity = 1;
					IsRanged = true;
					break;
				case ID.STICKY_BOMB:
					UseStyle = 1;
					ShootSpeed = 5f;
					Shoot = 37;
					Width = 20;
					Height = 20;
					MaxStack = 50;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 25;
					UseTime = 25;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 500;
					Damage = 0;
					break;
				case ID.BLACK_LENS:
					Width = 12;
					Height = 20;
					MaxStack = 99;
					Value = 5000;
					break;
				case ID.SUNGLASSES:
					Width = 28;
					Height = 12;
					HeadSlot = 12;
					Rarity = 2;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.WIZARD_HAT:
					Width = 28;
					Height = 20;
					HeadSlot = 14;
					Rarity = 2;
					Value = 10000;
					Defense = 2;
					break;
				case ID.TOP_HAT:
					Width = 18;
					Height = 18;
					HeadSlot = 15;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.TUXEDO_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 10;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.TUXEDO_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 10;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.SUMMER_HAT:
					Width = 18;
					Height = 18;
					HeadSlot = 16;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.BUNNY_HOOD:
					Width = 18;
					Height = 18;
					HeadSlot = 17;
					Value = 20000;
					IsVanity = true;
					break;
				case ID.PLUMBERS_HAT:
					Width = 18;
					Height = 12;
					HeadSlot = 18;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.PLUMBERS_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 11;
					Value = 250000;
					IsVanity = true;
					break;
				case ID.PLUMBERS_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 11;
					Value = 250000;
					IsVanity = true;
					break;
				case ID.HEROS_HAT:
					Width = 18;
					Height = 12;
					HeadSlot = 19;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.HEROS_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 12;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.HEROS_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 12;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.FISH_BOWL:
					Width = 18;
					Height = 18;
					HeadSlot = 20;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.ARCHAEOLOGISTS_HAT:
					Width = 18;
					Height = 12;
					HeadSlot = 21;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.ARCHAEOLOGISTS_JACKET:
					Width = 18;
					Height = 18;
					BodySlot = 13;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.ARCHAEOLOGISTS_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 13;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.BLACK_DYE:
					MaxStack = 99;
					Width = 12;
					Height = 20;
					Value = 10000;
					break;
				case ID.PURPLE_DYE:
					MaxStack = 99;
					Width = 12;
					Height = 20;
					Value = 2000;
					break;
				case ID.NINJA_HOOD:
					Width = 18;
					Height = 12;
					HeadSlot = 22;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.NINJA_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 14;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.NINJA_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 14;
					Value = 5000;
					IsVanity = true;
					break;
				case ID.LEATHER:
					Width = 18;
					Height = 20;
					MaxStack = 99;
					Value = 50;
					break;
				case ID.RED_HAT:
					Width = 18;
					Height = 14;
					HeadSlot = 24;
					Value = 1000;
					IsVanity = true;
					break;
				case ID.GOLDFISH:
					UseStyle = 2;
					UseSound = 2;
					CanUseTurn = false;
					UseAnimation = 17;
					UseTime = 17;
					Width = 20;
					Height = 10;
					MaxStack = 99;
					HealLife = 20;
					IsConsumable = true;
					Value = 1000;
					IsPotion = true;
					break;
				case ID.ROBE:
					Width = 18;
					Height = 14;
					BodySlot = 15;
					Value = 2000;
					IsVanity = true;
					break;
				case ID.ROBOT_HAT:
					Width = 18;
					Height = 18;
					HeadSlot = 25;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.GOLD_CROWN:
					Width = 18;
					Height = 18;
					HeadSlot = 26;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.HELLFIRE_ARROW:
					ShootSpeed = 6.5f;
					Shoot = 41;
					Damage = 10;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 8f;
					Value = 100;
					Rarity = 2;
					IsRanged = true;
					break;
				case ID.VULCAN_BOLT:
					ShootSpeed = 6.6f;
					Shoot = 114;
					Damage = 12;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 8.2f;
					Value = 150;
					Rarity = 3;
					IsRanged = true;
					break;
				case ID.SANDGUN:
					UseStyle = 5;
					UseAnimation = 16;
					UseTime = 16;
					AutoReuse = true;
					Width = 40;
					Height = 20;
					Shoot = 42;
					UseAmmo = 42;
					UseSound = 11;
					Damage = 30;
					ShootSpeed = 12f;
					NoMelee = true;
					Knockback = 5f;
					Value = 10000;
					Rarity = 2;
					IsRanged = true;
					break;
				case ID.GUIDE_VOODOO_DOLL:
					IsAccessory = true;
					Width = 14;
					Height = 26;
					Value = 1000;
					break;
				case ID.DIVING_HELMET:
					HeadSlot = 27;
					Defense = 2;
					Width = 20;
					Height = 20;
					Value = 1000;
					Rarity = 2;
					break;
				case ID.FAMILIAR_SHIRT:
					BodySlot = 0;
					Width = 20;
					Height = 20;
					Value = 10000;
					if (UI.CurrentUI.ActivePlayer != null)
					{
						Colour = UI.CurrentUI.ActivePlayer.shirtColor;
					}
					break;
				case ID.FAMILIAR_PANTS:
					LegSlot = 0;
					Width = 20;
					Height = 20;
					Value = 10000;
					if (UI.CurrentUI.ActivePlayer != null)
					{
						Colour = UI.CurrentUI.ActivePlayer.pantsColor;
					}
					break;
				case ID.FAMILIAR_WIG:
					HeadSlot = 0;
					Width = 20;
					Height = 20;
					Value = 10000;
					if (UI.CurrentUI.ActivePlayer != null)
					{
						Colour = UI.CurrentUI.ActivePlayer.hairColor;
					}
					break;
				case ID.DEMON_SCYTHE:
					Mana = 14;
					Damage = 35;
					UseStyle = 5;
					ShootSpeed = 0.2f;
					Shoot = 45;
					Width = 26;
					Height = 28;
					UseSound = 8;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Knockback = 5f;
					Scale = 0.9f;
					Value = 10000;
					IsMagic = true;
					break;
				case ID.NIGHTS_EDGE:
					UseStyle = 1;
					UseAnimation = 27;
					UseTime = 27;
					Knockback = 4.5f;
					Width = 40;
					Height = 40;
					Damage = 42;
					Scale = 1.15f;
					UseSound = 1;
					Rarity = 3;
					Value = 27000;
					IsMelee = true;
					break;
				case ID.DARK_LANCE:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 25;
					ShootSpeed = 5f;
					Knockback = 4f;
					Width = 40;
					Height = 40;
					Damage = 27;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 46;
					Rarity = 3;
					Value = 27000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.CORAL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 81;
					Width = 20;
					Height = 22;
					Value = 400;
					break;
				case ID.CACTUS:
					MaxStack = 250;
					Width = 12;
					Height = 12;
					Value = 10;
					break;
				case ID.TRIDENT:
					UseStyle = 5;
					UseAnimation = 31;
					UseTime = 31;
					ShootSpeed = 4f;
					Knockback = 5f;
					Width = 40;
					Height = 40;
					Damage = 10;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 47;
					Rarity = 1;
					Value = 10000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.SILVER_BULLET:
					ShootSpeed = 4.5f;
					Shoot = 14;
					Damage = 9;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 14;
					Knockback = 3f;
					Value = 15;
					IsRanged = true;
					break;
				case ID.THROWING_KNIFE:
					UseStyle = 1;
					ShootSpeed = 10f;
					Shoot = 48;
					Damage = 12;
					Width = 18;
					Height = 20;
					MaxStack = 250;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 50;
					Knockback = 2f;
					IsRanged = true;
					break;
				case ID.SPEAR:
					UseStyle = 5;
					UseAnimation = 31;
					UseTime = 31;
					ShootSpeed = 3.7f;
					Knockback = 6.5f;
					Width = 32;
					Height = 32;
					Damage = 8;
					Scale = 1f;
					UseSound = 1;
					Shoot = 49;
					Value = 1000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.BLOWPIPE:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 45;
					UseTime = 45;
					Width = 38;
					Height = 6;
					Shoot = 10;
					UseAmmo = 51;
					UseSound = 5;
					Damage = 9;
					ShootSpeed = 11f;
					NoMelee = true;
					Value = 10000;
					Knockback = 4f;
					UseAmmo = 51;
					IsRanged = true;
					break;
				case ID.GLOWSTICK:
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 50;
					Width = 12;
					Height = 12;
					MaxStack = 99;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoMelee = true;
					Value = 10;
					HoldStyle = 1;
					break;
				case ID.SEED:
					Shoot = 51;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					Ammo = 51;
					break;
				case ID.WOODEN_BOOMERANG:
					NoMelee = true;
					UseStyle = 1;
					ShootSpeed = 6.5f;
					Shoot = 52;
					Damage = 7;
					Knockback = 5f;
					Width = 14;
					Height = 28;
					UseSound = 1;
					UseAnimation = 16;
					UseTime = 16;
					NoUseGraphic = true;
					Value = 5000;
					IsMelee = true;
					break;
				case ID.AGLET:
					Width = 24;
					Height = 8;
					IsAccessory = true;
					Value = 5000;
					break;
				case ID.STICKY_GLOWSTICK:
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 53;
					Width = 12;
					Height = 12;
					MaxStack = 99;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoMelee = true;
					Value = 20;
					HoldStyle = 1;
					break;
				case ID.POISONED_KNIFE:
					UseStyle = 1;
					ShootSpeed = 11f;
					Shoot = 54;
					Damage = 13;
					Width = 18;
					Height = 20;
					MaxStack = 250;
					IsConsumable = true;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 60;
					Knockback = 2f;
					IsRanged = true;
					break;
				case ID.OBSIDIAN_SKIN_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.LAVA_IMMUNE;
					BuffTime = 14400;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.REGENERATION_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.LIFE_REGEN;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.SWIFTNESS_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.HASTE;
					BuffTime = 14400;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.GILLS_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.GILLS;
					BuffTime = 7200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.IRONSKIN_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.IRONSKIN;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.MANA_REGENERATION_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.MANA_REGEN;
					BuffTime = 7200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.MAGIC_POWER_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.MAGIC_POWER;
					BuffTime = 7200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.FEATHERFALL_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.SLOWFALL;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.SPELUNKER_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.FIND_TREASURE;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.INVISIBILITY_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.INVISIBLE;
					BuffTime = 7200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.SHINE_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.SHINE;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.NIGHT_OWL_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.NIGHTVISION;
					BuffTime = 14400;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.BATTLE_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.ENEMY_SPAWNS;
					BuffTime = 25200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.THORNS_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.THORNS;
					BuffTime = 7200;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.WATER_WALKING_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.WATER_WALK;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.ARCHERY_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.RANGED_DAMAGE;
					BuffTime = 14400;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.HUNTER_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.DETECT_CREATURE;
					BuffTime = 18000;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.GRAVITATION_POTION:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					BuffType = (int)Buff.ID.GRAVITY_CONTROL;
					BuffTime = 10800;
					Value = 1000;
					Rarity = 1;
					break;
				case ID.GOLD_CHEST:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 21;
					PlaceStyle = 1;
					Width = 26;
					Height = 22;
					Value = 5000;
					break;
				case ID.DAYBLOOM_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 0;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.MOONGLOW_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 1;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.BLINKROOT_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 2;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.DEATHWEED_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 3;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.WATERLEAF_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 4;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.FIREBLOSSOM_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 82;
					PlaceStyle = 5;
					Width = 12;
					Height = 14;
					Value = 80;
					break;
				case ID.DAYBLOOM:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.MOONGLOW:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.BLINKROOT:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.DEATHWEED:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.WATERLEAF:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.FIREBLOSSOM:
					MaxStack = 99;
					Width = 12;
					Height = 14;
					Value = 100;
					break;
				case ID.SHARK_FIN:
					MaxStack = 99;
					Width = 16;
					Height = 14;
					Value = 200;
					break;
				case ID.FEATHER:
					MaxStack = 99;
					Width = 16;
					Height = 14;
					Value = 50;
					break;
				case ID.TOMBSTONE:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 85;
					Width = 20;
					Height = 20;
					break;
				case ID.MIME_MASK:
					HeadSlot = 28;
					Width = 20;
					Height = 20;
					Value = 20000;
					break;
				case ID.ANTLION_MANDIBLE:
					Width = 10;
					Height = 20;
					MaxStack = 99;
					Value = 50;
					break;
				case ID.ILLEGAL_GUN_PARTS:
					Width = 10;
					Height = 20;
					MaxStack = 99;
					Value = 750000;
					break;
				case ID.THE_DOCTORS_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 16;
					Value = 200000;
					IsVanity = true;
					break;
				case ID.THE_DOCTORS_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 15;
					Value = 200000;
					IsVanity = true;
					break;
				case ID.GOLDEN_KEY:
					Width = 14;
					Height = 20;
					MaxStack = 99;
					break;
				case ID.SHADOW_CHEST:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 21;
					PlaceStyle = 3;
					Width = 26;
					Height = 22;
					Value = 5000;
					break;
				case ID.SHADOW_KEY:
					Width = 14;
					Height = 20;
					MaxStack = 1;
					Value = 75000;
					break;
				case ID.OBSIDIAN_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 20;
					Width = 12;
					Height = 12;
					break;
				case ID.JUNGLE_SPORES:
					Width = 18;
					Height = 16;
					MaxStack = 99;
					Value = 100;
					break;
				case ID.LOOM:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 86;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.PIANO:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 87;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.DRESSER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 88;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.BENCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 89;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.BATHTUB:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 90;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.RED_BANNER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 91;
					PlaceStyle = 0;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.GREEN_BANNER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 91;
					PlaceStyle = 1;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.BLUE_BANNER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 91;
					PlaceStyle = 2;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.YELLOW_BANNER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 91;
					PlaceStyle = 3;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.LAMP_POST:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 92;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.TIKI_TORCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 93;
					Width = 10;
					Height = 24;
					Value = 500;
					break;
				case ID.BARREL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 21;
					PlaceStyle = 5;
					Width = 20;
					Height = 20;
					Value = 500;
					break;
				case ID.CHINESE_LANTERN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 95;
					Width = 20;
					Height = 20;
					Value = 500;
					break;
				case ID.COOKING_POT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 96;
					Width = 20;
					Height = 20;
					Value = 500;
					break;
				case ID.SAFE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 97;
					Width = 20;
					Height = 20;
					Value = 500000;
					break;
				case ID.SKULL_LANTERN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 98;
					Width = 20;
					Height = 20;
					Value = 500;
					break;
				case ID.TRASH_CAN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 21;
					PlaceStyle = 6;
					Width = 20;
					Height = 20;
					Value = 1000;
					break;
				case ID.CANDELABRA:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 100;
					Width = 20;
					Height = 20;
					Value = 1500;
					break;
				case ID.PINK_VASE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 13;
					PlaceStyle = 3;
					Width = 16;
					Height = 24;
					Value = 70;
					break;
				case ID.MUG:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 13;
					PlaceStyle = 4;
					Width = 16;
					Height = 24;
					Value = 20;
					break;
				case ID.KEG:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 94;
					Width = 24;
					Height = 24;
					Value = 600;
					break;
				case ID.ALE:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 10;
					Height = 10;
					BuffType = (int)Buff.ID.DRUNK;
					BuffTime = 7200;
					Value = 100;
					break;
				case ID.BOOKCASE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 101;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.THRONE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 102;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.BOWL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 103;
					Width = 16;
					Height = 24;
					Value = 20;
					break;
				case ID.BOWL_OF_SOUP:
					UseSound = 3;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 10;
					Height = 10;
					BuffType = (int)Buff.ID.WELL_FED;
					BuffTime = 36000;
					Rarity = 1;
					Value = 1000;
					break;
				case ID.TOILET:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 15;
					PlaceStyle = 1;
					Width = 12;
					Height = 30;
					Value = 150;
					break;
				case ID.GRANDFATHER_CLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 104;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.GOBLIN_BATTLE_STANDARD:
					UseStyle = 4;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					Width = 28;
					Height = 28;
					break;
				case ID.TATTERED_CLOTH:
					MaxStack = 99;
					Width = 24;
					Height = 24;
					Value = 30;
					break;
				case ID.SAWMILL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 106;
					Width = 20;
					Height = 20;
					Value = 300;
					break;
				case ID.COBALT_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 107;
					Width = 12;
					Height = 12;
					Value = 3500;
					Rarity = 3;
					break;
				case ID.MYTHRIL_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 108;
					Width = 12;
					Height = 12;
					Value = 5500;
					Rarity = 3;
					break;
				case ID.ADAMANTITE_ORE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 111;
					Width = 12;
					Height = 12;
					Value = 7500;
					Rarity = 3;
					break;
				case ID.PWNHAMMER:
					CanUseTurn = true;
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 27;
					UseTime = 14;
					HammerPower = 80;
					Width = 24;
					Height = 28;
					Damage = 26;
					Knockback = 7.5f;
					Scale = 1.2f;
					UseSound = 1;
					Rarity = 4;
					Value = 39000;
					IsMelee = true;
					break;
				case ID.EXCALIBUR:
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 25;
					UseTime = 25;
					Knockback = 4.5f;
					Width = 40;
					Height = 40;
					Damage = 47;
					Scale = 1.15f;
					UseSound = 1;
					Rarity = 5;
					Value = 230000;
					IsMelee = true;
					break;
				case ID.TIZONA:
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 25;
					UseTime = 25;
					Knockback = 4.6f;
					Width = 40;
					Height = 40;
					Damage = 55;
					Scale = 1.15f;
					UseSound = 1;
					Rarity = 5;
					Value = 300000;
					IsMelee = true;
					break;
				case ID.HALLOWED_SEEDS:
					CanUseTurn = true;
					UseStyle = 1;
					UseAnimation = 15;
					UseTime = 10;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 109;
					Width = 14;
					Height = 14;
					Value = 2000;
					Rarity = 3;
					break;
				case ID.EBONSAND_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 112;
					Width = 12;
					Height = 12;
					Ammo = 42;
					break;
				case ID.COBALT_HAT:
					Width = 18;
					Height = 18;
					Defense = 2;
					HeadSlot = 29;
					Rarity = 4;
					Value = 75000;
					break;
				case ID.COBALT_HELMET:
					Width = 18;
					Height = 18;
					Defense = 11;
					HeadSlot = 30;
					Rarity = 4;
					Value = 75000;
					break;
				case ID.COBALT_MASK:
					Width = 18;
					Height = 18;
					Defense = 4;
					HeadSlot = 31;
					Rarity = 4;
					Value = 75000;
					break;
				case ID.COBALT_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 8;
					BodySlot = 17;
					Rarity = 4;
					Value = 60000;
					break;
				case ID.COBALT_LEGGINGS:
					Width = 18;
					Height = 18;
					Defense = 7;
					LegSlot = 16;
					Rarity = 4;
					Value = 45000;
					break;
				case ID.MYTHRIL_HOOD:
					Width = 18;
					Height = 18;
					Defense = 3;
					HeadSlot = 32;
					Rarity = 4;
					Value = 112500;
					break;
				case ID.MYTHRIL_HELMET:
					Width = 18;
					Height = 18;
					Defense = 16;
					HeadSlot = 33;
					Rarity = 4;
					Value = 112500;
					break;
				case ID.MYTHRIL_HAT:
					Width = 18;
					Height = 18;
					Defense = 6;
					HeadSlot = 34;
					Rarity = 4;
					Value = 112500;
					break;
				case ID.MYTHRIL_CHAINMAIL:
					Width = 18;
					Height = 18;
					Defense = 12;
					BodySlot = 18;
					Rarity = 4;
					Value = 90000;
					break;
				case ID.MYTHRIL_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 9;
					LegSlot = 17;
					Rarity = 4;
					Value = 67500;
					break;
				case ID.COBALT_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 10500;
					Rarity = 3;
					break;
				case ID.MYTHRIL_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 22000;
					Rarity = 3;
					break;
				case ID.COBALT_CHAINSAW:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 8;
					ShootSpeed = 40f;
					Knockback = 2.75f;
					Width = 20;
					Height = 12;
					Damage = 23;
					AxePower = 14;
					UseSound = 23;
					Shoot = 57;
					Rarity = 4;
					Value = 54000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.MYTHRIL_CHAINSAW:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 8;
					ShootSpeed = 40f;
					Knockback = 3f;
					Width = 20;
					Height = 12;
					Damage = 29;
					AxePower = 17;
					UseSound = 23;
					Shoot = 58;
					Rarity = 4;
					Value = 81000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.COBALT_DRILL:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 13;
					ShootSpeed = 32f;
					Knockback = 0f;
					Width = 20;
					Height = 12;
					Damage = 10;
					PickPower = 110;
					UseSound = 23;
					Shoot = 59;
					Rarity = 4;
					Value = 54000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.MYTHRIL_DRILL:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 10;
					ShootSpeed = 32f;
					Knockback = 0f;
					Width = 20;
					Height = 12;
					Damage = 15;
					PickPower = 150;
					UseSound = 23;
					Shoot = 60;
					Rarity = 4;
					Value = 81000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.ADAMANTITE_CHAINSAW:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 6;
					ShootSpeed = 40f;
					Knockback = 4.5f;
					Width = 20;
					Height = 12;
					Damage = 33;
					AxePower = 20;
					UseSound = 23;
					Shoot = 61;
					Rarity = 4;
					Value = 108000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.ADAMANTITE_DRILL:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 7;
					ShootSpeed = 32f;
					Knockback = 0f;
					Width = 20;
					Height = 12;
					Damage = 20;
					PickPower = 180;
					UseSound = 23;
					Shoot = 62;
					Rarity = 4;
					Value = 108000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.DAO_OF_POW:
					NoMelee = true;
					UseStyle = 5;
					UseAnimation = 45;
					UseTime = 45;
					Knockback = 7f;
					Width = 30;
					Height = 10;
					Damage = 49;
					Scale = 1.1f;
					NoUseGraphic = true;
					Shoot = 63;
					ShootSpeed = 15f;
					UseSound = 1;
					Rarity = 5;
					Value = 144000;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.MYTHRIL_HALBERD:
					UseStyle = 5;
					UseAnimation = 26;
					UseTime = 26;
					ShootSpeed = 4.5f;
					Knockback = 5f;
					Width = 40;
					Height = 40;
					Damage = 35;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 64;
					Rarity = 4;
					Value = 67500;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.ADAMANTITE_BAR:
					Width = 20;
					Height = 20;
					MaxStack = 99;
					Value = 37500;
					Rarity = 3;
					break;
				case ID.GLASS_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 21;
					Width = 12;
					Height = 12;
					break;
				case ID.COMPASS:
					Width = 24;
					Height = 28;
					Rarity = 3;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.DIVING_GEAR:
					Width = 24;
					Height = 28;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.GPS:
					Width = 24;
					Height = 28;
					Rarity = 4;
					Value = 150000;
					IsAccessory = true;
					break;
				case ID.OBSIDIAN_HORSESHOE:
					Width = 24;
					Height = 28;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.OBSIDIAN_SHIELD:
					Width = 24;
					Height = 28;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					Defense = 2;
					break;
				case ID.TINKERERS_WORKSHOP:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 114;
					Width = 26;
					Height = 20;
					Value = 100000;
					break;
				case ID.CLOUD_IN_A_BALLOON:
					Width = 14;
					Height = 28;
					Rarity = 4;
					Value = 150000;
					IsAccessory = true;
					break;
				case ID.ADAMANTITE_HEADGEAR:
					Width = 18;
					Height = 18;
					Defense = 4;
					HeadSlot = 35;
					Rarity = 4;
					Value = 150000;
					break;
				case ID.ADAMANTITE_HELMET:
					Width = 18;
					Height = 18;
					Defense = 22;
					HeadSlot = 36;
					Rarity = 4;
					Value = 150000;
					break;
				case ID.ADAMANTITE_MASK:
					Width = 18;
					Height = 18;
					Defense = 8;
					HeadSlot = 37;
					Rarity = 4;
					Value = 150000;
					break;
				case ID.ADAMANTITE_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 14;
					BodySlot = 19;
					Rarity = 4;
					Value = 120000;
					break;
				case ID.ADAMANTITE_LEGGINGS:
					Width = 18;
					Height = 18;
					Defense = 10;
					LegSlot = 18;
					Rarity = 4;
					Value = 90000;
					break;
				case ID.SPECTRE_BOOTS:
					Width = 28;
					Height = 24;
					IsAccessory = true;
					Rarity = 4;
					Value = 100000;
					break;
				case ID.ADAMANTITE_GLAIVE:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 25;
					ShootSpeed = 5f;
					Knockback = 6f;
					Width = 40;
					Height = 40;
					Damage = 38;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 66;
					Rarity = 4;
					Value = 90000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.TOOLBELT:
					Width = 28;
					Height = 24;
					IsAccessory = true;
					Rarity = 3;
					Value = 100000;
					break;
				case ID.PEARLSAND_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 116;
					Width = 12;
					Height = 12;
					Ammo = 42;
					break;
				case ID.PEARLSTONE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 117;
					Width = 12;
					Height = 12;
					break;
				case ID.MINING_SHIRT:
					Width = 18;
					Height = 18;
					Defense = 1;
					BodySlot = 20;
					Value = 5000;
					Rarity = 1;
					break;
				case ID.MINING_PANTS:
					Width = 18;
					Height = 18;
					Defense = 1;
					LegSlot = 19;
					Value = 5000;
					Rarity = 1;
					break;
				case ID.PEARLSTONE_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 118;
					Width = 12;
					Height = 12;
					break;
				case ID.IRIDESCENT_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 119;
					Width = 12;
					Height = 12;
					break;
				case ID.MUDSTONE_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 120;
					Width = 12;
					Height = 12;
					break;
				case ID.COBALT_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 121;
					Width = 12;
					Height = 12;
					break;
				case ID.MYTHRIL_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 122;
					Width = 12;
					Height = 12;
					break;
				case ID.PEARLSTONE_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 22;
					Width = 12;
					Height = 12;
					break;
				case ID.IRIDESCENT_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 23;
					Width = 12;
					Height = 12;
					break;
				case ID.MUDSTONE_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 24;
					Width = 12;
					Height = 12;
					break;
				case ID.COBALT_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 25;
					Width = 12;
					Height = 12;
					break;
				case ID.MYTHRIL_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 26;
					Width = 12;
					Height = 12;
					break;
				case ID.HOLY_WATER:
					UseStyle = 1;
					ShootSpeed = 9f;
					Rarity = 3;
					Damage = 20;
					Shoot = 69;
					Width = 18;
					Height = 20;
					MaxStack = 250;
					IsConsumable = true;
					Knockback = 3f;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 200;
					break;
				case ID.UNHOLY_WATER:
					UseStyle = 1;
					ShootSpeed = 9f;
					Rarity = 3;
					Damage = 20;
					Shoot = 70;
					Width = 18;
					Height = 20;
					MaxStack = 250;
					IsConsumable = true;
					Knockback = 3f;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 200;
					break;
				case ID.SILT_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 123;
					Width = 12;
					Height = 12;
					break;
				case ID.FAIRY_BELL:
					Mana = 40;
					Channelling = true;
					Damage = 0;
					UseStyle = 1;
					Shoot = 72;
					Width = 24;
					Height = 24;
					UseSound = 25;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 5;
					NoMelee = true;
					Value = (Value = 250000);
					BuffType = (int)Buff.ID.FAIRY;
					BuffTime = 18000;
					break;
				case ID.BREAKER_BLADE:
					UseStyle = 1;
					UseAnimation = 30;
					Knockback = 8f;
					Width = 60;
					Height = 70;
					Damage = 39;
					Scale = 1.05f;
					UseSound = 1;
					Rarity = 4;
					Value = 150000;
					IsMelee = true;
					break;
				case ID.BLUE_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 1;
					Width = 10;
					Height = 12;
					Value = 200;
					break;
				case ID.RED_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 2;
					Width = 10;
					Height = 12;
					Value = 200;
					break;
				case ID.GREEN_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 3;
					Width = 10;
					Height = 12;
					Value = 200;
					break;
				case ID.PURPLE_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 4;
					Width = 10;
					Height = 12;
					Value = 200;
					break;
				case ID.WHITE_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 5;
					Width = 10;
					Height = 12;
					Value = 500;
					break;
				case ID.YELLOW_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 6;
					Width = 10;
					Height = 12;
					Value = 200;
					break;
				case ID.DEMON_TORCH:
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 7;
					Width = 10;
					Height = 12;
					Value = 300;
					break;
				case ID.CLOCKWORK_ASSAULT_RIFLE:
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 4;
					ReuseDelay = 14;
					Width = 50;
					Height = 18;
					Shoot = 10;
					UseAmmo = 14;
					UseSound = 31;
					Damage = 19;
					ShootSpeed = 7.75f;
					NoMelee = true;
					Value = 150000;
					Rarity = 4;
					IsRanged = true;
					break;
				case ID.COBALT_REPEATER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 25;
					UseTime = 25;
					Width = 50;
					Height = 18;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 30;
					ShootSpeed = 9f;
					NoMelee = true;
					Value = 60000;
					IsRanged = true;
					Rarity = 4;
					Knockback = 1.5f;
					break;
				case ID.MYTHRIL_REPEATER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 23;
					UseTime = 23;
					Width = 50;
					Height = 18;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 34;
					ShootSpeed = 9.5f;
					NoMelee = true;
					Value = 90000;
					IsRanged = true;
					Rarity = 4;
					Knockback = 2f;
					break;
				case ID.DUAL_HOOK:
					NoUseGraphic = true;
					Damage = 0;
					Knockback = 7f;
					UseStyle = 5;
					ShootSpeed = 14f;
					Shoot = 73;
					Width = 18;
					Height = 28;
					UseSound = 1;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 4;
					NoMelee = true;
					Value = 200000;
					break;
				case ID.STAR_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 2;
					break;
				case ID.SWORD_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 3;
					break;
				case ID.SLIME_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 4;
					break;
				case ID.GOBLIN_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 5;
					break;
				case ID.SHIELD_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 6;
					break;
				case ID.BAT_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 7;
					break;
				case ID.FISH_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 8;
					break;
				case ID.BUNNY_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 9;
					break;
				case ID.SKELETON_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 10;
					break;
				case ID.REAPER_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 11;
					break;
				case ID.WOMAN_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 12;
					break;
				case ID.IMP_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 13;
					break;
				case ID.GARGOYLE_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 14;
					break;
				case ID.GLOOM_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 15;
					break;
				case ID.HORNET_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 16;
					break;
				case ID.BOMB_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 17;
					break;
				case ID.CRAB_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 18;
					break;
				case ID.HAMMER_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 19;
					break;
				case ID.POTION_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 20;
					break;
				case ID.SPEAR_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 21;
					break;
				case ID.CROSS_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 22;
					break;
				case ID.JELLYFISH_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 23;
					break;
				case ID.BOW_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 24;
					break;
				case ID.BOOMERANG_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 25;
					break;
				case ID.BOOT_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 26;
					break;
				case ID.CHEST_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 27;
					break;
				case ID.BIRD_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 28;
					break;
				case ID.AXE_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 29;
					break;
				case ID.CORRUPT_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 30;
					break;
				case ID.TREE_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 31;
					break;
				case ID.ANVIL_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 32;
					break;
				case ID.PICKAXE_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 33;
					break;
				case ID.MUSHROOM_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 34;
					break;
				case ID.EYEBALL_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 35;
					break;
				case ID.PILLAR_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 36;
					break;
				case ID.HEART_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 37;
					break;
				case ID.POT_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 38;
					break;
				case ID.SUNFLOWER_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 39;
					break;
				case ID.KING_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 40;
					break;
				case ID.QUEEN_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 41;
					break;
				case ID.PIRANHA_STATUE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 105;
					Width = 20;
					Height = 20;
					Value = 300;
					PlaceStyle = 42;
					break;
				case ID.PLANKED_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 27;
					Width = 12;
					Height = 12;
					break;
				case ID.WOODEN_BEAM:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 124;
					Width = 12;
					Height = 12;
					break;
				case ID.ADAMANTITE_REPEATER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 20;
					UseTime = 20;
					Width = 50;
					Height = 18;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 37;
					ShootSpeed = 10f;
					NoMelee = true;
					Value = 120000;
					IsRanged = true;
					Rarity = 4;
					Knockback = 2.5f;
					break;
				case ID.ADAMANTITE_SWORD:
					UseStyle = 1;
					UseAnimation = 27;
					UseTime = 27;
					Knockback = 6f;
					Width = 40;
					Height = 40;
					Damage = 44;
					Scale = 1.2f;
					UseSound = 1;
					Rarity = 4;
					Value = 138000;
					IsMelee = true;
					break;
				case ID.COBALT_SWORD:
					CanUseTurn = true;
					AutoReuse = true;
					UseStyle = 1;
					UseAnimation = 23;
					UseTime = 23;
					Knockback = 3.85f;
					Width = 40;
					Height = 40;
					Damage = 34;
					Scale = 1.1f;
					UseSound = 1;
					Rarity = 4;
					Value = 69000;
					IsMelee = true;
					break;
				case ID.MYTHRIL_SWORD:
					UseStyle = 1;
					UseAnimation = 26;
					UseTime = 26;
					Knockback = 6f;
					Width = 40;
					Height = 40;
					Damage = 39;
					Scale = 1.15f;
					UseSound = 1;
					Rarity = 4;
					Value = 103500;
					IsMelee = true;
					break;
				case ID.MOON_CHARM:
					Rarity = 4;
					Width = 24;
					Height = 28;
					IsAccessory = true;
					Value = 150000;
					break;
				case ID.RULER:
					Width = 10;
					Height = 26;
					IsAccessory = true;
					Value = 10000;
					Rarity = 1;
					break;
				case ID.CRYSTAL_BALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 125;
					Width = 22;
					Height = 22;
					Value = 100000;
					Rarity = 3;
					break;
				case ID.DISCO_BALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 126;
					Width = 22;
					Height = 26;
					Value = 10000;
					break;
				case ID.SORCERER_EMBLEM:
					Width = 24;
					Height = 24;
					IsAccessory = true;
					Value = 100000;
					Rarity = 4;
					break;
				case ID.RANGER_EMBLEM:
					Width = 24;
					Height = 24;
					IsAccessory = true;
					Value = 100000;
					break;
				case ID.WARRIOR_EMBLEM:
					Width = 24;
					Height = 24;
					IsAccessory = true;
					Value = 100000;
					Rarity = 4;
					break;
				case ID.DEMON_WINGS:
				case ID.ANGEL_WINGS:
#if !VERSION_INITIAL
				case ID.SPARKLY_WINGS:
#endif
					Width = 24;
					Height = 8;
					IsAccessory = true;
					Value = 400000;
					Rarity = 5;
					break;
				case ID.MAGICAL_HARP:
					Rarity = 5;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 12;
					Width = 12;
					Height = 28;
					Shoot = 76;
					HoldStyle = 3;
					AutoReuse = true;
					Damage = 30;
					ShootSpeed = 4.5f;
					NoMelee = true;
					Value = 200000;
					Mana = 4;
					IsMagic = true;
					break;
				case ID.RAINBOW_ROD:
					Rarity = 5;
					Mana = 10;
					Channelling = true;
					Damage = 53;
					UseStyle = 1;
					ShootSpeed = 6f;
					Shoot = 79;
					Width = 26;
					Height = 28;
					UseSound = 28;
					UseAnimation = 15;
					UseTime = 15;
					NoMelee = true;
					Knockback = 5f;
					TileBoost = 64;
					Value = 200000;
					IsMagic = true;
					break;
				case ID.ICE_ROD:
					Rarity = 4;
					Mana = 7;
					Damage = 26;
					UseStyle = 1;
					ShootSpeed = 12f;
					Shoot = 80;
					Width = 26;
					Height = 28;
					UseSound = 28;
					UseAnimation = 17;
					UseTime = 17;
					Rarity = 4;
					AutoReuse = true;
					NoMelee = true;
					Knockback = 0f;
					Value = 1000000;
					IsMagic = true;
					Knockback = 2f;
					break;
				case ID.NEPTUNES_SHELL:
					Width = 24;
					Height = 28;
					IsAccessory = true;
					Value = 150000;
					Rarity = 5;
					break;
				case ID.MANNEQUIN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 128;
					Width = 12;
					Height = 12;
					break;
				case ID.GREATER_HEALING_POTION:
					UseSound = 3;
					HealLife = 150;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 30;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					Rarity = 3;
					IsPotion = true;
					Value = 5000;
					break;
				case ID.GREATER_MANA_POTION:
					UseSound = 3;
					HealMana = 200;
					UseStyle = 2;
					CanUseTurn = true;
					UseAnimation = 17;
					UseTime = 17;
					MaxStack = 99;
					IsConsumable = true;
					Width = 14;
					Height = 24;
					Rarity = 3;
					Value = 500;
					break;
				case ID.PIXIE_DUST:
					Width = 16;
					Height = 14;
					MaxStack = 99;
					Value = 500;
					Rarity = 1;
					break;
				case ID.CRYSTAL_SHARD:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 129;
					Width = 24;
					Height = 24;
					Value = 8000;
					Rarity = 1;
					break;
				case ID.CLOWN_HAT:
					Width = 18;
					Height = 18;
					HeadSlot = 40;
					Value = 20000;
					IsVanity = true;
					Rarity = 2;
					break;
				case ID.CLOWN_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 23;
					Value = 10000;
					IsVanity = true;
					Rarity = 2;
					break;
				case ID.CLOWN_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 22;
					Value = 10000;
					IsVanity = true;
					Rarity = 2;
					break;
				case ID.FLAMETHROWER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 30;
					UseTime = 6;
					Width = 50;
					Height = 18;
					Shoot = 85;
					UseAmmo = 23;
					UseSound = 34;
					Damage = 27;
					Knockback = 0.3f;
					ShootSpeed = 7f;
					NoMelee = true;
					Value = 500000;
					Rarity = 5;
					IsRanged = true;
					break;
				case ID.BELL:
					Rarity = 3;
					UseStyle = 1;
					UseAnimation = 12;
					UseTime = 12;
					Width = 12;
					Height = 28;
					AutoReuse = true;
					NoMelee = true;
					Value = 10000;
					break;
				case ID.HARP:
					Rarity = 3;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 12;
					Width = 12;
					Height = 28;
					AutoReuse = true;
					NoMelee = true;
					Value = 10000;
					break;
				case ID.WRENCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Rarity = 1;
					Value = 20000;
					Mech = true;
					break;
				case ID.WIRE_CUTTER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Rarity = 1;
					Value = 20000;
					Mech = true;
					break;
				case ID.ACTIVE_STONE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 130;
					Width = 12;
					Height = 12;
					Value = 1000;
					Mech = true;
					break;
				case ID.INACTIVE_STONE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 131;
					Width = 12;
					Height = 12;
					Value = 1000;
					Mech = true;
					break;
				case ID.LEVER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 132;
					Width = 24;
					Height = 24;
					Value = 3000;
					Mech = true;
					break;
				case ID.LASER_RIFLE:
					AutoReuse = true;
					UseStyle = 5;
					UseAnimation = 12;
					UseTime = 12;
					Width = 36;
					Height = 22;
					Shoot = 88;
					Mana = 8;
					UseSound = 12;
					Knockback = 2.5f;
					Damage = 29;
					ShootSpeed = 17f;
					NoMelee = true;
					Rarity = 4;
					IsMagic = true;
					Value = 500000;
					break;
				case ID.CRYSTAL_BULLET:
					ShootSpeed = 5f;
					Shoot = 89;
					Damage = 9;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 14;
					Knockback = 1f;
					Value = 30;
					IsRanged = true;
					Rarity = 3;
					break;
				case ID.HOLY_ARROW:
					ShootSpeed = 3.5f;
					Shoot = 91;
					Damage = 6;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 2f;
					Value = 80;
					IsRanged = true;
					Rarity = 3;
					break;
				case ID.MAGIC_DAGGER:
					UseStyle = 1;
					ShootSpeed = 10f;
					Shoot = 93;
					Damage = 28;
					Width = 18;
					Height = 20;
					Mana = 7;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					NoMelee = true;
					Value = 1000000;
					Knockback = 2f;
					IsMagic = true;
					Rarity = 4;
					break;
				case ID.CRYSTAL_STORM:
					AutoReuse = true;
					Rarity = 4;
					Mana = 5;
					UseSound = 9;
					UseStyle = 5;
					Damage = 26;
					UseAnimation = 7;
					UseTime = 7;
					Width = 24;
					Height = 28;
					Shoot = 94;
					Scale = 0.9f;
					ShootSpeed = 16f;
					Knockback = 5f;
					IsMagic = true;
					Value = 500000;
					break;
				case ID.CURSED_FLAMES:
					AutoReuse = true;
					Rarity = 4;
					Mana = 14;
					UseSound = 20;
					UseStyle = 5;
					Damage = 35;
					UseAnimation = 20;
					UseTime = 20;
					Width = 24;
					Height = 28;
					Shoot = 95;
					Scale = 0.9f;
					ShootSpeed = 10f;
					Knockback = 6.5f;
					IsMagic = true;
					Value = 500000;
					break;
				case ID.SOUL_OF_LIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 1000;
					Rarity = 3;
					break;
				case ID.SOUL_OF_NIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 1000;
					Rarity = 3;
					break;
				case ID.SOUL_OF_BLIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 100000;
					Rarity = 5;
					break;
				case ID.CURSED_FLAME:
					Width = 12;
					Height = 14;
					MaxStack = 99;
					Value = 4000;
					Rarity = 3;
					break;
				case ID.CURSED_TORCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					HoldStyle = 1;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 4;
					PlaceStyle = 8;
					Width = 10;
					Height = 12;
					Value = 300;
					Rarity = 1;
					break;
				case ID.ADAMANTITE_FORGE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 133;
					Width = 44;
					Height = 30;
					Value = 50000;
					Rarity = 3;
					break;
				case ID.MYTHRIL_ANVIL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 134;
					Width = 28;
					Height = 14;
					Value = 25000;
					Rarity = 3;
					break;
				case ID.UNICORN_HORN:
					Width = 14;
					Height = 14;
					MaxStack = 99;
					Value = 15000;
					Rarity = 1;
					break;
				case ID.DARK_SHARD:
					Width = 14;
					Height = 14;
					MaxStack = 99;
					Value = 4500;
					Rarity = 2;
					break;
				case ID.LIGHT_SHARD:
					Width = 14;
					Height = 14;
					MaxStack = 99;
					Value = 4500;
					Rarity = 2;
					break;
				case ID.RED_PRESSURE_PLATE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 135;
					Width = 12;
					Height = 12;
					PlaceStyle = 0;
					Mech = true;
					Value = 5000;
					Mech = true;
					break;
				case ID.WIRE:
					Width = 12;
					Height = 18;
					MaxStack = 250;
					Value = 500;
					Mech = true;
					break;
				case ID.SPELL_TOME:
					Width = 12;
					Height = 18;
					MaxStack = 99;
					Value = 50000;
					Rarity = 1;
					break;
				case ID.STAR_CLOAK:
					Width = 20;
					Height = 24;
					Value = 100000;
					IsAccessory = true;
					Rarity = 4;
					break;
				case ID.MEGASHARK:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 7;
					UseTime = 7;
					Width = 50;
					Height = 18;
					Shoot = 10;
					UseAmmo = 14;
					UseSound = 11;
					Damage = 23;
					ShootSpeed = 10f;
					NoMelee = true;
					Value = 300000;
					Rarity = 5;
					Knockback = 1f;
					IsRanged = true;
					break;
				case ID.SHOTGUN:
					Knockback = 6.5f;
					UseStyle = 5;
					UseAnimation = 45;
					UseTime = 45;
					Width = 50;
					Height = 14;
					Shoot = 10;
					UseAmmo = 14;
					UseSound = 36;
					Damage = 18;
					ShootSpeed = 6f;
					NoMelee = true;
					Value = 700000;
					Rarity = 4;
					IsRanged = true;
					break;
				case ID.PHILOSOPHERS_STONE:
					Width = 12;
					Height = 18;
					Value = 100000;
					IsAccessory = true;
					Rarity = 4;
					break;
				case ID.TITAN_GLOVE:
					Width = 12;
					Height = 18;
					Value = 100000;
					Rarity = 4;
					IsAccessory = true;
					break;
				case ID.COBALT_NAGINATA:
					UseStyle = 5;
					UseAnimation = 28;
					UseTime = 28;
					ShootSpeed = 4.3f;
					Knockback = 4f;
					Width = 40;
					Height = 40;
					Damage = 29;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 97;
					Rarity = 4;
					Value = 45000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.SWITCH:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 136;
					Width = 12;
					Height = 12;
					Value = 2000;
					Mech = true;
					break;
				case ID.DART_TRAP:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 137;
					Width = 12;
					Height = 12;
					Value = 10000;
					Mech = true;
					break;
				case ID.BOULDER:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 138;
					Width = 12;
					Height = 12;
					Mech = true;
					break;
				case ID.GREEN_PRESSURE_PLATE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 135;
					Width = 12;
					Height = 12;
					PlaceStyle = 1;
					Mech = true;
					Value = 5000;
					break;
				case ID.GRAY_PRESSURE_PLATE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 135;
					Width = 12;
					Height = 12;
					PlaceStyle = 2;
					Mech = true;
					Value = 5000;
					break;
				case ID.BROWN_PRESSURE_PLATE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 135;
					Width = 12;
					Height = 12;
					PlaceStyle = 3;
					Mech = true;
					Value = 5000;
					break;
				case ID.MECHANICAL_EYE:
					UseStyle = 4;
					Width = 22;
					Height = 14;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					Rarity = 3;
					break;
				case ID.CURSED_ARROW:
					ShootSpeed = 4f;
					Shoot = 103;
					Damage = 14;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 3f;
					Value = 80;
					IsRanged = true;
					Rarity = 3;
					break;
				case ID.SPECTRAL_ARROW:
					ShootSpeed = 4.2f;
					Shoot = 113;
					Damage = 16;
					Width = 10;
					Height = 28;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 1;
					Knockback = 3.1f;
					Value = 90;
					IsRanged = true;
					Rarity = 4;
					break;
				case ID.CURSED_BULLET:
					ShootSpeed = 5f;
					Shoot = 104;
					Damage = 12;
					Width = 8;
					Height = 8;
					MaxStack = 250;
					IsConsumable = true;
					Ammo = 14;
					Knockback = 4f;
					Value = 30;
					Rarity = 1;
					IsRanged = true;
					Rarity = 3;
					break;
				case ID.SOUL_OF_FRIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 100000;
					Rarity = 5;
					break;
				case ID.SOUL_OF_MIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 100000;
					Rarity = 5;
					break;
				case ID.SOUL_OF_SIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 100000;
					Rarity = 5;
					break;
				case ID.GUNGNIR:
					UseStyle = 5;
					UseAnimation = 22;
					UseTime = 22;
					ShootSpeed = 5.6f;
					Knockback = 6.4f;
					Width = 40;
					Height = 40;
					Damage = 42;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 105;
					Rarity = 5;
					Value = 1500000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.TONBOGIRI:
					UseStyle = 5;
					UseAnimation = 20;
					UseTime = 20;
					ShootSpeed = 5.75f;
					Knockback = 6.7f;
					Width = 40;
					Height = 40;
					Damage = 50;
					Scale = 1.1f;
					UseSound = 1;
					Shoot = 112;
					Rarity = 5;
					Value = 2000000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					break;
				case ID.HALLOWED_PLATE_MAIL:
					Width = 18;
					Height = 18;
					Defense = 15;
					BodySlot = 24;
					Rarity = 5;
					Value = 200000;
					break;
				case ID.HALLOWED_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 11;
					LegSlot = 23;
					Rarity = 5;
					Value = 150000;
					break;
				case ID.HALLOWED_HELMET:
					Width = 18;
					Height = 18;
					Defense = 9;
					HeadSlot = 41;
					Rarity = 5;
					Value = 250000;
					break;
				case ID.HALLOWED_HEADGEAR:
					Width = 18;
					Height = 18;
					Defense = 5;
					HeadSlot = 42;
					Rarity = 5;
					Value = 250000;
					break;
				case ID.HALLOWED_MASK:
					Width = 18;
					Height = 18;
					Defense = 24;
					HeadSlot = 43;
					Rarity = 5;
					Value = 250000;
					break;
				case ID.CROSS_NECKLACE:
					Width = 20;
					Height = 24;
					Value = 1500;
					IsAccessory = true;
					Rarity = 4;
					break;
				case ID.MANA_FLOWER:
					Width = 20;
					Height = 24;
					Value = 50000;
					IsAccessory = true;
					Rarity = 4;
					break;
				case ID.MECHANICAL_WORM:
					UseStyle = 4;
					Width = 22;
					Height = 14;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					Rarity = 3;
					break;
				case ID.MECHANICAL_SKULL:
					UseStyle = 4;
					Width = 22;
					Height = 14;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					Rarity = 3;
					break;
				case ID.SLIME_CROWN:
					UseStyle = 4;
					Width = 22;
					Height = 14;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					MaxStack = 20;
					Rarity = 1;
					break;
				case ID.LIGHT_DISC:
					IsMelee = true;
					AutoReuse = true;
					NoMelee = true;
					UseStyle = 1;
					ShootSpeed = 13f;
					Shoot = 106;
					Damage = 35;
					Knockback = 8f;
					Width = 24;
					Height = 24;
					UseSound = 1;
					UseAnimation = 15;
					UseTime = 15;
					NoUseGraphic = true;
					Rarity = 5;
					MaxStack = 5;
					Value = 500000;
					break;
				case ID.MUSIC_BOX_OVERWORLD_DAY:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 0;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_EERIE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 1;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_NIGHT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 2;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_TITLE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 3;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_UNDERGROUND:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 4;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_BOSS1:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 5;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_JUNGLE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 6;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_CORRUPTION:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 7;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_UNDERGROUND_CORRUPTION:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 8;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_THE_HALLOW:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 9;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_BOSS2:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 10;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_UNDERGROUND_HALLOW:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 11;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_BOSS3:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 12;
					Width = 24;
					Height = 24;
					Rarity = 3;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_DESERT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 13;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_SPACE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 14;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_TUTORIAL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 15;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_BOSS4:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 16;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_OCEAN:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 17;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.MUSIC_BOX_SNOW:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					IsConsumable = true;
					CreateTile = 139;
					PlaceStyle = 18;
					Width = 24;
					Height = 24;
					Rarity = 4;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.SOUL_OF_FLIGHT:
					Width = 18;
					Height = 18;
					MaxStack = 250;
					Value = 1000;
					Rarity = 3;
					break;
				case ID.MUSIC_BOX:
					Width = 24;
					Height = 24;
					Rarity = 3;
					Value = 100000;
					IsAccessory = true;
					break;
				case ID.DEMONITE_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 140;
					Width = 12;
					Height = 12;
					break;
				case ID.HALLOWED_REPEATER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 19;
					UseTime = 19;
					Width = 50;
					Height = 18;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 39;
					ShootSpeed = 11f;
					NoMelee = true;
					Value = 200000;
					IsRanged = true;
					Rarity = 4;
					Knockback = 2.5f;
					break;
				case ID.VULCAN_REPEATER:
					UseStyle = 5;
					AutoReuse = true;
					UseAnimation = 18;
					UseTime = 18;
					Width = 50;
					Height = 18;
					Shoot = 1;
					UseAmmo = 1;
					UseSound = 5;
					Damage = 42;
					ShootSpeed = 12f;
					NoMelee = true;
					Value = 250000;
					IsRanged = true;
					Rarity = 5;
					Knockback = 2.65f;
					break;
				case ID.HAMDRAX:
					UseStyle = 5;
					UseAnimation = 25;
					UseTime = 7;
					ShootSpeed = 36f;
					Knockback = 4.75f;
					Width = 20;
					Height = 12;
					Damage = 35;
					PickPower = 200;
					AxePower = 22;
					HammerPower = 85;
					UseSound = 23;
					Shoot = 107;
					Rarity = 4;
					Value = 220000;
					NoMelee = true;
					NoUseGraphic = true;
					IsMelee = true;
					Channelling = true;
					break;
				case ID.EXPLOSIVES:
					Mech = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 141;
					Width = 12;
					Height = 12;
					break;
				case ID.INLET_PUMP:
					Mech = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 142;
					Width = 12;
					Height = 12;
					break;
				case ID.OUTLET_PUMP:
					Mech = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 143;
					Width = 12;
					Height = 12;
					break;
				case ID.ONE_SECOND_TIMER:
					Mech = true;
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 144;
					PlaceStyle = 0;
					Width = 10;
					Height = 12;
					Value = 50;
					break;
				case ID.THREE_SECOND_TIMER:
					Mech = true;
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 144;
					PlaceStyle = 1;
					Width = 10;
					Height = 12;
					Value = 50;
					break;
				case ID.FIVE_SECOND_TIMER:
					Mech = true;
					CantTouchLiquid = true;
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 99;
					IsConsumable = true;
					CreateTile = 144;
					PlaceStyle = 2;
					Width = 10;
					Height = 12;
					Value = 50;
					break;
				case ID.CANDY_CANE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 145;
					Width = 12;
					Height = 12;
					break;
				case ID.CANDY_CANE_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 29;
					Width = 12;
					Height = 12;
					break;
				case ID.SANTA_HAT:
					Width = 18;
					Height = 12;
					HeadSlot = 44;
					Value = 150000;
					IsVanity = true;
					break;
				case ID.SANTA_SHIRT:
					Width = 18;
					Height = 18;
					BodySlot = 25;
					Value = 150000;
					IsVanity = true;
					break;
				case ID.SANTA_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 24;
					Value = 150000;
					IsVanity = true;
					break;
				case ID.GREEN_CANDY_CANE_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 146;
					Width = 12;
					Height = 12;
					break;
				case ID.GREEN_CANDY_CANE_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 30;
					Width = 12;
					Height = 12;
					break;
				case ID.SNOW_BLOCK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 147;
					Width = 12;
					Height = 12;
					break;
				case ID.SNOW_BRICK:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 148;
					Width = 12;
					Height = 12;
					break;
				case ID.SNOW_BRICK_WALL:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateWall = 31;
					Width = 12;
					Height = 12;
					break;
				case ID.BLUE_LIGHT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 149;
					PlaceStyle = 0;
					Width = 12;
					Height = 12;
					Value = 500;
					break;
				case ID.RED_LIGHT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 149;
					PlaceStyle = 1;
					Width = 12;
					Height = 12;
					Value = 500;
					break;
				case ID.GREEN_LIGHT:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 250;
					IsConsumable = true;
					CreateTile = 149;
					PlaceStyle = 2;
					Width = 12;
					Height = 12;
					Value = 500;
					break;
				case ID.BLUE_PRESENT:
					Width = 12;
					Height = 12;
					Rarity = 1;
					break;
				case ID.GREEN_PRESENT:
					Width = 12;
					Height = 12;
					Rarity = 1;
					break;
				case ID.YELLOW_PRESENT:
					Width = 12;
					Height = 12;
					Rarity = 1;
					break;
				case ID.SNOW_GLOBE:
					UseStyle = 4;
					IsConsumable = true;
					UseAnimation = 45;
					UseTime = 45;
					Width = 28;
					Height = 28;
					Rarity = 2;
					break;
				case ID.PET_SPAWN_1:
					Damage = 0;
					UseStyle = 1;
					Shoot = 111;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.PET_SPAWN_2:
					Damage = 0;
					UseStyle = 1;
					Shoot = 115;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.PET_SPAWN_3:
					Damage = 0;
					UseStyle = 1;
					Shoot = 116;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.PET_SPAWN_4:
					Damage = 0;
					UseStyle = 1;
					Shoot = 117;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.PET_SPAWN_5:
					Damage = 0;
					UseStyle = 1;
					Shoot = 118;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.PET_SPAWN_6:
					Damage = 0;
					UseStyle = 1;
					Shoot = 119;
					Width = 16;
					Height = 30;
					UseSound = 2;
					UseAnimation = 20;
					UseTime = 20;
					Rarity = 3;
					NoMelee = true;
					Value = 0;
					BuffType = (int)Buff.ID.PET;
					break;
				case ID.DRAGON_MASK:
					Width = 26;
					Height = 18;
					Defense = 26;
					HeadSlot = 45;
					Rarity = 5;
					Value = 500000;
					break;
				case ID.TITAN_HELMET:
					Width = 26;
					Height = 22;
					Defense = 14;
					HeadSlot = 46;
					Rarity = 5;
					Value = 500000;
					break;
				case ID.SPECTRAL_HEADGEAR:
					Width = 22;
					Height = 20;
					Defense = 10;
					HeadSlot = 47;
					Rarity = 5;
					Value = 500000;
					break;
				case ID.DRAGON_BREASTPLATE:
					Width = 26;
					Height = 18;
					Defense = 20;
					BodySlot = 26;
					Rarity = 5;
					Value = 1000000;
					break;
				case ID.TITAN_MAIL:
					Width = 30;
					Height = 18;
					Defense = 18;
					BodySlot = 27;
					Rarity = 5;
					Value = 1000000;
					break;
				case ID.SPECTRAL_ARMOR:
					Width = 30;
					Height = 28;
					Defense = 15;
					BodySlot = 28;
					Rarity = 5;
					Value = 1000000;
					break;
				case ID.DRAGON_GREAVES:
					Width = 22;
					Height = 18;
					Defense = 14;
					LegSlot = 25;
					Rarity = 5;
					Value = 750000;
					break;
				case ID.TITAN_LEGGINGS:
					Width = 22;
					Height = 18;
					Defense = 13;
					LegSlot = 26;
					Rarity = 5;
					Value = 750000;
					break;
				case ID.SPECTRAL_SUBLIGAR:
					Width = 22;
					Height = 18;
					Defense = 15;
					LegSlot = 27;
					Rarity = 5;
					Value = 750000;
					break;
#if VERSION_101
				case ID.FABULOUS_RIBBON:
					Width = 18;
					Height = 12;
					HeadSlot = 48;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.GEORGES_HAT:
					Width = 18;
					Height = 12;
					HeadSlot = 49;
					Value = 10000;
					IsVanity = true;
					break;
				case ID.FABULOUS_DRESS:
					Width = 18;
					Height = 18;
					BodySlot = 29;
					Value = 250000;
					IsVanity = true;
					break;
				case ID.GEORGES_SUIT:
					Width = 18;
					Height = 18;
					BodySlot = 30;
					Value = 250000;
					IsVanity = true;
					break;
				case ID.FABULOUS_SLIPPERS:
					Width = 18;
					Height = 18;
					LegSlot = 28;
					Value = 250000;
					IsVanity = true;
					break;
				case ID.GEORGES_PANTS:
					Width = 18;
					Height = 18;
					LegSlot = 29;
					Value = 250000; 
					IsVanity = true;
					break;
				case ID.CAMPFIRE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 10;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateTile = 150;
					Width = 12;
					Height = 12;
					break;
				case ID.WOOD_HELMET:
					Width = 18;
					Height = 18;
					Defense = 1;
					HeadSlot = 51;
					break;
				case ID.WOOD_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 1;
					BodySlot = 32;
					break;
				case ID.WOOD_GREAVES:
					Width = 18;
					Height = 18;
					Defense = 0;
					LegSlot = 31;
					break;
				case ID.CACTUS_SWORD:
					UseStyle = 1;
					CanUseTurn = false;
					UseAnimation = 25;
					UseTime = 25;
					Width = 24;
					Height = 28;
					Damage = 9;
					Knockback = 5.0f;
					UseSound = 1;
					Scale = 1.0f;
					Value = 1800;
					IsMelee = true;
					break;
				case ID.CACTUS_PICKAXE:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 23;
					UseTime = 15;
					AutoReuse = true;
					Width = 24;
					Height = 28;
					Damage = 5;
					PickPower = 35;
					UseSound = 1;
					Knockback = 2.0f;
					Value = 2000;
					IsMelee = true;
					break;
				case ID.CACTUS_HELMET:
					Width = 18;
					Height = 18;
					Defense = 1;
					HeadSlot = 50;
					break;
				case ID.CACTUS_BREASTPLATE:
					Width = 18;
					Height = 18;
					Defense = 2;
					BodySlot = 31;
					break;
				case ID.CACTUS_LEGGINGS:
					Width = 18;
					Height = 18;
					Defense = 1;
					LegSlot = 30;
					break;
				case ID.PURPLE_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 32;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
				case ID.YELLOW_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 33;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
				case ID.BLUE_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 34;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
				case ID.GREEN_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 35;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
				case ID.RED_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 36;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
				case ID.MULTICOLORED_STAINED_GLASS:
					UseStyle = 1;
					CanUseTurn = true;
					UseAnimation = 15;
					UseTime = 7;
					AutoReuse = true;
					MaxStack = 999;
					IsConsumable = true;
					CreateWall = 37;
					Width = 12;
					Height = 12;
					Value = 125;
					break;
#endif
				default:
					Active = 0;
					Stack = 0;
					break;
			}
#if VERSION_101
			if (MaxStack == 250) // Quick and easy fix for 1.01
			{
				MaxStack = 999;
			}
#endif
			if (!NoMaterialCheck)
			{
				CheckMaterial();
			}
		}

		public Color GetAlpha(Color ItemColour)
		{
			if (Type == (int)ID.FALLEN_STAR)
			{
				return new Color(255, 255, 255, ItemColour.A - Alpha);
			}
			if ((Type >= (int)ID.FLAMARANG && Type <= (int)ID.MOLTEN_PICKAXE) || (Type >= (int)ID.BLUE_PHASEBLADE && Type <= (int)ID.YELLOW_PHASEBLADE) || Type == (int)ID.MOLTEN_HAMAXE || Type == (int)ID.FLAMELASH || Type == (int)ID.PHOENIX_BLASTER || Type == (int)ID.SUNFURY)
			{
				return new Color(255, 255, 255, 255);
			}
#if VERSION_103 || VERSION_FINAL // BUG: Souls of Blight are not lit up correctly in versions prior to the console 1.2 update.
			if (Type == (int)ID.SOUL_OF_LIGHT || Type == (int)ID.SOUL_OF_NIGHT || Type == (int)ID.CURSED_FLAME || Type == (int)ID.SOUL_OF_FRIGHT || Type == (int)ID.SOUL_OF_MIGHT || Type == (int)ID.SOUL_OF_SIGHT || Type == (int)ID.SOUL_OF_FLIGHT || Type == (int)ID.SOUL_OF_BLIGHT)
#else
			if (Type == (int)ID.SOUL_OF_LIGHT || Type == (int)ID.SOUL_OF_NIGHT || Type == (int)ID.CURSED_FLAME || Type == (int)ID.SOUL_OF_FRIGHT || Type == (int)ID.SOUL_OF_MIGHT || Type == (int)ID.SOUL_OF_SIGHT || Type == (int)ID.SOUL_OF_FLIGHT)
#endif
			{
				return new Color(255, 255, 255, 50);
			}
			if (Type == (int)ID.HEART || Type == (int)ID.STAR || Type == (int)ID.PIXIE_DUST)
			{
				return new Color(200, 200, 200, 200);
			}
			int RemainderA = 256 - Alpha;
			int R = ItemColour.R * RemainderA >> 8;
			int G = ItemColour.G * RemainderA >> 8;
			int B = ItemColour.B * RemainderA >> 8;
			int A = ItemColour.A - Alpha;
			return new Color(R, G, B, A);
		}

		public Color GetAlphaInventory(Color ItemColour)
		{
			int InvA = 256 - Alpha;
			int R = ItemColour.R * InvA >> 8;
			int G = ItemColour.G * InvA >> 8;
			int B = ItemColour.B * InvA >> 8;
			int A = ItemColour.A - Alpha;
			return new Color(R, G, B, A);
		}

		public Color GetColor(Color ItemColour)
		{
			int R = Colour.R - (255 - ItemColour.R);
			int G = Colour.G - (255 - ItemColour.G);
			int B = Colour.B - (255 - ItemColour.B);
			int A = Colour.A - (255 - ItemColour.A);
			return new Color(R, G, B, A);
		}

		public static bool MechSpawn(int ItemX, int ItemY, int ItemType) // For the statues for stars, hearts, and bombs.
		{
			int Counter1 = 0;
			int Counter2 = 0;
			int Counter3 = 0;
			for (int ItemNPCIdx = 0; ItemNPCIdx < NPC.MaxNumNPCs; ItemNPCIdx++) // Bit confusing; not actually related to NPCs.
			{ // Also don't know why the max was 196, such an arbitrary number, but I linked it to the NPC limit anyway.
				if (Main.ItemSet[ItemNPCIdx].Active == 0 || Main.ItemSet[ItemNPCIdx].Type != ItemType)
				{
					continue;
				}
				Counter1++;
				Vector2 WorldCoords = new Vector2(ItemX, ItemY);
				float XCoord = Main.ItemSet[ItemNPCIdx].Position.X - WorldCoords.X;
				float YCoord = Main.ItemSet[ItemNPCIdx].Position.Y - WorldCoords.Y;
				float Marker = XCoord * XCoord + YCoord * YCoord;
				if (Marker < 640000f)
				{
					Counter3++;
					if (Marker < 90000f)
					{
						Counter2++;
					}
				}
			}
			if (Counter2 >= 3 || Counter3 >= 6 || Counter1 >= 10)
			{
				return false;
			}
			return true;
		}

		public unsafe void UpdateItem(int ActiveItemIdx)
		{
			float Acceleration = 0.1f;
			float TerminalVelocity = 7f;
			if (IsWet)
			{
				TerminalVelocity = 5f;
				Acceleration = 0.08f;
			}
			Vector2 ItemVelocity = Velocity;
			ItemVelocity.X *= 0.5f;
			ItemVelocity.Y *= 0.5f;
			if (OwnTime > 0 && --OwnTime == 0)
			{
				OwnIgnore = Player.MaxNumPlayers;
			}
			if (KeepTime > 0)
			{
				KeepTime--;
			}
			else if (IsLocal() || (Main.NetMode != (byte)NetModeSetting.CLIENT && (Owner == 8 || Main.PlayerSet[Owner].Active == 0)))
			{
				FindOwner(ActiveItemIdx);
			}
			if (!BeingGrabbed)
			{
				Velocity.X *= 0.95f;
				if (Velocity.X < 0.1f && Velocity.X > -0.1f)
				{
					Velocity.X = 0f;
				}
#if VERSION_103 || VERSION_FINAL // BUG: Souls of Blight don't float in versions prior to the console 1.2 update.
				if (Type == (int)ID.SOUL_OF_LIGHT || Type == (int)ID.SOUL_OF_NIGHT || Type == (int)ID.SOUL_OF_FRIGHT || Type == (int)ID.SOUL_OF_MIGHT || Type == (int)ID.SOUL_OF_SIGHT || Type == (int)ID.SOUL_OF_FLIGHT || Type == (int)ID.SOUL_OF_BLIGHT)
#else
				if (Type == (int)ID.SOUL_OF_LIGHT || Type == (int)ID.SOUL_OF_NIGHT || Type == (int)ID.SOUL_OF_FRIGHT || Type == (int)ID.SOUL_OF_MIGHT || Type == (int)ID.SOUL_OF_SIGHT || Type == (int)ID.SOUL_OF_FLIGHT) 
#endif
				{
					Velocity.Y *= 0.95f;
					if (Velocity.Y < 0.1f && Velocity.Y > -0.1f)
					{
						Velocity.Y = 0f;
					}
				}
				else
				{
					Velocity.Y += Acceleration;
					if (Velocity.Y > TerminalVelocity)
					{
						Velocity.Y = TerminalVelocity;
					}
				}
				bool HasHitLava = Collision.LavaCollision(ref Position, Width, Height);
				IsInLava |= HasHitLava;
				if (Collision.WetCollision(ref Position, Width, Height))
				{
					if (!IsWet)
					{
						if (WetCount == 0)
						{
							WetCount = 20;
							if (!HasHitLava)
							{
								for (int i = 0; i < 8; i++)
								{
									Dust* CreatedDust = Main.DustSet.NewDust((int)Position.X - 6, (int)Position.Y + (Height >> 1) - 8, Width + 12, 24, 33);
									if (CreatedDust == null)
									{
										break;
									}
									CreatedDust->Velocity.Y -= 4f;
									CreatedDust->Velocity.X *= 2.5f;
									CreatedDust->Scale = 1.3f;
									CreatedDust->Alpha = 100;
									CreatedDust->NoGravity = true;
								}
								Main.PlaySound(19, (int)Position.X, (int)Position.Y);
							}
							else
							{
								for (int j = 0; j < 4; j++)
								{
									Dust* CreatedDust = Main.DustSet.NewDust((int)Position.X - 6, (int)Position.Y + (Height >> 1) - 8, Width + 12, 24, 35);
									if (CreatedDust == null)
									{
										break;
									}
									CreatedDust->Velocity.Y -= 1.5f;
									CreatedDust->Velocity.X *= 2.5f;
									CreatedDust->Scale = 1.3f;
									CreatedDust->Alpha = 100;
									CreatedDust->NoGravity = true;
								}
								Main.PlaySound(19, (int)Position.X, (int)Position.Y);
							}
						}
						IsWet = true;
					}
				}
				else if (IsWet)
				{
					IsWet = false;
				}
				if (WetCount > 0)
				{
					WetCount--;
				}
				if (IsWet)
				{
					Vector2 CurrentVelocity = Velocity;
					Collision.TileCollision(ref Position, ref Velocity, Width, Height);
					if (Velocity.X != CurrentVelocity.X)
					{
						ItemVelocity.X = Velocity.X;
					}
					if (Velocity.Y != CurrentVelocity.Y)
					{
						ItemVelocity.Y = Velocity.Y;
					}
				}
				else
				{
					IsInLava = false;
					Collision.TileCollision(ref Position, ref Velocity, Width, Height);
				}
				if (IsInLava)
				{
					if (Type == (int)ID.GUIDE_VOODOO_DOLL)
					{
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							Active = 0;
							Type = 0;
							Stack = 0;
							for (int NpcSlotIdx = 0; NpcSlotIdx < NPC.MaxNumNPCs; NpcSlotIdx++)
							{
								if (Main.NPCSet[NpcSlotIdx].Type == (int)NPC.ID.GUIDE && Main.NPCSet[NpcSlotIdx].Active != 0)
								{
									NetMessage.SendNpcHurt(NpcSlotIdx, 8192, 10.0, -Main.NPCSet[NpcSlotIdx].Direction);
									Main.NPCSet[NpcSlotIdx].StrikeNPC(8192, 10f, -Main.NPCSet[NpcSlotIdx].Direction);
									NPC.SpawnWOF(ref Position);
									break;
								}
							}
							NetMessage.CreateMessage2(21, UI.MainUI.MyPlayer, ActiveItemIdx);
							NetMessage.SendMessage();
						}
					}
					else if (IsLocal() && Type != (int)ID.FIREBLOSSOM_SEEDS && Type != (int)ID.FIREBLOSSOM && Type != (int)ID.OBSIDIAN && Type != (int)ID.HELLSTONE && Type != (int)ID.HELLSTONE_BAR && Rarity == 0)
					{
						Active = 0;
						Type = 0;
						Stack = 0;
						NetMessage.CreateMessage2(21, UI.MainUI.MyPlayer, ActiveItemIdx);
						NetMessage.SendMessage();
					}
				}
				if (Type == (int)ID.SOUL_OF_LIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.5f * LightMultiplier, 0.1f * LightMultiplier, 0.25f * LightMultiplier));
				}
				else if (Type == (int)ID.SOUL_OF_NIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.25f * LightMultiplier, 0.1f * LightMultiplier, 0.5f * LightMultiplier));
				}
				else if (Type == (int)ID.SOUL_OF_FRIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.5f * LightMultiplier, 0.3f * LightMultiplier, 0.05f * LightMultiplier));
				}
				else if (Type == (int)ID.SOUL_OF_MIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.1f * LightMultiplier, 0.1f * LightMultiplier, 0.6f * LightMultiplier));
				}
				else if (Type == (int)ID.SOUL_OF_FLIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.1f * LightMultiplier, 0.3f * LightMultiplier, 0.5f * LightMultiplier));
				}
				else if (Type == (int)ID.SOUL_OF_SIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.1f * LightMultiplier, 0.5f * LightMultiplier, 0.2f * LightMultiplier));
				}
#if VERSION_103 || VERSION_FINAL // BUG: Souls of Blight are not lit up correctly in versions prior to the console 1.2 update.
				else if (Type == (int)ID.SOUL_OF_BLIGHT)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.4f * LightMultiplier, 0.6f * LightMultiplier, 0.1f * LightMultiplier));
				}
#endif
				else if (Type == (int)ID.HEART)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale * 0.5f;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.5f * LightMultiplier, 0.1f * LightMultiplier, 0.1f * LightMultiplier));
				}
				else if (Type == (int)ID.STAR)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale * 0.5f;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.1f * LightMultiplier, 0.1f * LightMultiplier, 0.5f * LightMultiplier));
				}
				else if (Type == (int)ID.CURSED_FLAME)
				{
					float LightMultiplier = Main.Rand.Next(90, 111) * 0.01f;
					LightMultiplier *= UI.PulseScale * 0.2f;
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.5f * LightMultiplier, 1f * LightMultiplier, 0.1f * LightMultiplier));
				}
				else if (Type == (int)ID.FALLEN_STAR && Main.GameTime.DayTime)
				{
					for (int i = 0; i < 8; i++)
					{
						if (null == Main.DustSet.NewDust((int)Position.X, (int)Position.Y, Width, Height, 15, Velocity.X, Velocity.Y, 150, default, 1.2))
						{
							break;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						Gore.NewGore(Position, Velocity, Main.Rand.Next(16, 18));
					}
					Active = 0;
					Type = 0;
					Stack = 0;
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.CreateMessage2(21, UI.MainUI.MyPlayer, ActiveItemIdx);
						NetMessage.SendMessage();
					}
				}
			}
			else
			{
				BeingGrabbed = false;
			}
			if (Type == (int)ID.PIXIE_DUST)
			{
				if (Main.Rand.Next(6) == 0)
				{
					Dust* ActiveDust = Main.DustSet.NewDust((int)Position.X, (int)Position.Y, Width, Height, 55, 0.0, 0.0, 200, Colour);
					if (ActiveDust != null)
					{
						ActiveDust->Velocity.X *= 0.3f;
						ActiveDust->Velocity.Y *= 0.3f;
						ActiveDust->Scale *= 0.5f;
					}
				}
			}
			else if (Type == (int)ID.TORCH || Type == (int)ID.CANDLE)
			{
				if (!IsWet)
				{
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(1f, 0.95f, 0.8f));
				}
			}
			else if (Type == (int)ID.CURSED_TORCH)
			{
				Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.85f, 1f, 0.7f));
			}
			else if (Type >= (int)ID.BLUE_TORCH && Type <= (int)ID.YELLOW_TORCH)
			{
				if (!IsWet)
				{
					Vector3 TorchLight;
					switch (Type)
					{
					case (int)ID.BLUE_TORCH:
						TorchLight = new Vector3(0.1f, 0.2f, 1.1f);
						break;
					case (int)ID.RED_TORCH:
						TorchLight = new Vector3(1f, 0.1f, 0.1f);
						break;
					case (int)ID.GREEN_TORCH:
						TorchLight = new Vector3(0f, 1f, 0.1f);
						break;
					case (int)ID.PURPLE_TORCH:
						TorchLight = new Vector3(0.9f, 0f, 0.9f);
						break;
					case (int)ID.WHITE_TORCH:
						TorchLight = new Vector3(1.3f, 1.3f, 1.3f);
						break;
					default: // Yellow Torch
						TorchLight = new Vector3(0.9f, 0.9f, 0f);
						break;
					}
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, TorchLight);
				}
			}
			else if (Type == (int)ID.FLAMING_ARROW)
			{
				if (!IsWet)
				{
					Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(1f, 0.75f, 0.55f));
				}
			}
			else if (Type == (int)ID.SPECTRAL_ARROW)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, IsWet ? new Vector3(0.25f, 0.5f, 0.5f) : new Vector3(0.5f, 1f, 1f));
			}
			else if (Type == (int)ID.GLOWSTICK)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.7f, 1f, 0.8f));
			}
			else if (Type == (int)ID.STICKY_GLOWSTICK)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.7f, 0.8f, 1f));
			}
			else if (Type == (int)ID.JUNGLE_SPORES)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.55f, 0.75f, 0.6f));
			}
			else if (Type == (int)ID.GLOWING_MUSHROOM)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.15f, 0.45f, 0.9f));
			}
			else if (Type == (int)ID.FALLEN_STAR)
			{
				Lighting.AddLight((int)Position.X + Width >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.8f, 0.7f, 0.1f));
				if (Main.Rand.Next(32) == 0)
				{
					Main.DustSet.NewDust((int)Position.X, (int)Position.Y, Width, Height, 58, Velocity.X * 0.5f, Velocity.Y * 0.5f, 150, default, 1.2);
				}
				else if (Main.Rand.Next(64) == 0)
				{
					Gore.NewGore(Position, new Vector2(Velocity.X * 0.2f, Velocity.Y * 0.2f), Main.Rand.Next(16, 18));
				}
			}
			SpawnTime++;
			if (Main.NetMode == (byte)NetModeSetting.SERVER && !IsLocal() && ++Release >= 300)
			{
				Release = 0;
				FindOwner(ActiveItemIdx);
			}
			if (IsWet)
			{
				Position.X += ItemVelocity.X;
				Position.Y += ItemVelocity.Y;
			}
			else
			{
				Position.X += Velocity.X;
				Position.Y += Velocity.Y;
			}
			if (NoGrabDelay > 0)
			{
				NoGrabDelay--;
			}
		}

		public unsafe static int NewItem(int X, int Y, int Width, int Height, int Type, int Stack = 1, bool DoNotBroadcast = false, int Prefix = 0)
		{
			int ItemLimit = Main.MaxNumItems;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				uint LastIdx = LastItemIndex;
				uint ItemSpawnTime = Main.ItemSet[LastIdx].SpawnTime;
				uint LatestIdx = LastIdx;
				for (int i = Main.MaxNumItems - 1; i >= 0; i--)
				{
					if (Main.ItemSet[LastIdx].Active == 0)
					{
						ItemLimit = (int)LastIdx;
						break;
					}
					if (++LastIdx == Main.MaxNumItems)
					{
						LastIdx = 0;
					}
					uint PrevSpawnTime = Main.ItemSet[LastIdx].SpawnTime;
					if (PrevSpawnTime > ItemSpawnTime)
					{
						ItemSpawnTime = PrevSpawnTime;
						LatestIdx = LastIdx;
					}
				}
				if (ItemLimit == Main.MaxNumItems)
				{
					ItemLimit = (int)LatestIdx;
				}
				if (++LastIdx == Main.MaxNumItems)
				{
					LastIdx = 0;
				}
				LastItemIndex = LastIdx;
			}
			fixed (Item* ActiveItem = &Main.ItemSet[ItemLimit])
			{
				ActiveItem->SetDefaults(Type, Stack);
				ActiveItem->SetPrefix(Prefix);
				ActiveItem->Position.X = X + (Width - ActiveItem->Width >> 1);
				ActiveItem->Position.Y = Y + (Height - ActiveItem->Height >> 1);
				ActiveItem->IsWet = Collision.WetCollision(ref ActiveItem->Position, ActiveItem->Width, ActiveItem->Height);
				ActiveItem->Velocity.X = Main.Rand.Next(-30, 31) * 0.1f;
				if (Type == (int)ID.SOUL_OF_LIGHT || Type == (int)ID.SOUL_OF_NIGHT)
				{
					ActiveItem->Velocity.Y = Main.Rand.Next(-30, 31) * 0.1f;
				}
				else
				{
					ActiveItem->Velocity.Y = Main.Rand.Next(-40, -15) * 0.1f;
				}
				ActiveItem->SpawnTime = 0;
				if (!DoNotBroadcast && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					NetMessage.CreateMessage2(21, UI.MainUI.MyPlayer, ItemLimit);
					NetMessage.SendMessage();
					ActiveItem->FindOwner(ItemLimit);
				}
			}
			return ItemLimit;
		}

		public unsafe void FindOwner(int WhoAmI)
		{
			if (KeepTime > 0)
			{
				return;
			}
			int OwnerIdx = Player.MaxNumPlayers;
			int RegisteredSpace = NPC.SpawnWidth;
			int XCoord = (int)Position.X - (Width >> 1);
			int YCoord = (int)Position.Y - Height;
			fixed (Item* ActiveItem = &this)
			{
				for (int CurrentIdx = 0; CurrentIdx < Player.MaxNumPlayers; CurrentIdx++)
				{
					if (OwnIgnore != CurrentIdx && Main.PlayerSet[CurrentIdx].Active != 0 && Main.PlayerSet[CurrentIdx].ItemSpace(ActiveItem))
					{
						int num5 = Math.Abs(Main.PlayerSet[CurrentIdx].XYWH.X + 10 - XCoord) + Math.Abs(Main.PlayerSet[CurrentIdx].XYWH.Y + 21 - YCoord);
						if (num5 < RegisteredSpace)
						{
							RegisteredSpace = num5;
							OwnerIdx = CurrentIdx;
						}
					}
				}
			}
			int ItemOwner = Owner;
			if (OwnerIdx != ItemOwner)
			{
				bool LocalOwner = IsLocal();
				Owner = (byte)OwnerIdx;
				if (((LocalOwner && Main.NetMode >= 1) || (ItemOwner == 8 && Main.NetMode == (byte)NetModeSetting.SERVER) || Main.PlayerSet[ItemOwner].Active == 0) && Active != 0)
				{
					NetMessage.CreateMessage1(22, WhoAmI);
					NetMessage.SendMessage();
				}
			}
		}

		public bool IsNotTheSameAs(ref Item ComparedItem)
		{
			if (NetID == ComparedItem.NetID && Stack == ComparedItem.Stack)
			{
				return PrefixType != ComparedItem.PrefixType;
			}
			return true;
		}

		public bool CanBePlacedInAmmoSlot()
		{
			if (Ammo <= 0)
			{
				return Type == (int)ID.WIRE;
			}
			return true;
		}

		public bool CanBeAutoPlacedInEmptyAmmoSlot()
		{
			if (Type != (int)ID.SAND_BLOCK && Type != (int)ID.FALLEN_STAR && Type != (int)ID.GEL && Type != (int)ID.EBONSAND_BLOCK)
			{
				return Type != (int)ID.PEARLSAND_BLOCK;
			}
			return false;
		}

		public bool CanBePlacedInCoinSlot()
		{
			if (Type >= (int)ID.COPPER_COIN)
			{
				return Type <= (int)ID.PLATINUM_COIN;
			}
			return false;
		}
	}
}
