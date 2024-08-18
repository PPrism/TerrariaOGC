using System;
using Microsoft.Xna.Framework;
using Terraria.Achievements;

namespace Terraria
{
	public sealed class NPC
	{
		public enum ID
		{
			NONE,
			SLIME,
			DEMON_EYE,
			ZOMBIE,
			EYE_OF_CTHULHU,
			SERVANT_OF_CTHULHU,
			EATER_OF_SOULS,
			DEVOURER_HEAD,
			DEVOURER_BODY,
			DEVOURER_TAIL,
			GIANT_WORM_HEAD,
			GIANT_WORM_BODY,
			GIANT_WORM_TAIL,
			EATER_OF_WORLDS_HEAD,
			EATER_OF_WORLDS_BODY,
			EATER_OF_WORLDS_TAIL,
			MOTHER_SLIME,
			MERCHANT,
			NURSE,
			ARMS_DEALER,
			DRYAD,
			SKELETON,
			GUIDE,
			METEOR_HEAD,
			FIRE_IMP,
			BURNING_SPHERE,
			GOBLIN_PEON,
			GOBLIN_THIEF,
			GOBLIN_WARRIOR,
			GOBLIN_SORCERER,
			CHAOS_BALL,
			BONES,
			DARK_CASTER,
			WATER_SPHERE,
			CURSED_SKULL,
			SKELETRON_HEAD,
			SKELETRON_HAND,
			OLD_MAN,
			DEMOLITIONIST,
			BONE_SERPENT_HEAD,
			BONE_SERPENT_BODY,
			BONE_SERPENT_TAIL,
			HORNET,
			MAN_EATER,
			UNDEAD_MINER,
			TIM,
			BUNNY,
			CORRUPT_BUNNY,
			HARPY,
			CAVE_BAT,
			KING_SLIME,
			JUNGLE_BAT,
			DOCTOR_BONES,
			THE_GROOM,
			CLOTHIER,
			GOLDFISH,
			SNATCHER,
			CORRUPT_GOLDFISH,
			PIRANHA,
			LAVA_SLIME,
			HELLBAT,
			VULTURE,
			DEMON,
			BLUE_JELLYFISH,
			PINK_JELLYFISH,
			SHARK,
			VOODOO_DEMON,
			CRAB,
			DUNGEON_GUARDIAN,
			ANTLION,
			SPIKE_BALL,
			DUNGEON_SLIME,
			BLAZING_WHEEL,
			GOBLIN_SCOUT,
			BIRD,
			PIXIE,
			XXX_UNUSED_XXX, // Known as None2, has a wandering eye sprite
			ARMORED_SKELETON,
			MUMMY,
			DARK_MUMMY,
			LIGHT_MUMMY,
			CORRUPT_SLIME,
			WRAITH,
			CURSED_HAMMER,
			ENCHANTED_SWORD,
			MIMIC,
			UNICORN,
			WYVERN_HEAD,
			WYVERN_LEGS,
			WYVERN_BODY1,
			WYVERN_BODY2,
			WYVERN_BODY3,
			WYVERN_TAIL,
			GIANT_BAT,
			CORRUPTOR,
			DIGGER_HEAD,
			DIGGER_BODY,
			DIGGER_TAIL,
			SEEKER_HEAD, // World Feeder
			SEEKER_BODY,
			SEEKER_TAIL,
			CLINGER,
			ANGLER_FISH,
			GREEN_JELLYFISH,
			WEREWOLF,
			BOUND_GOBLIN,
			BOUND_WIZARD,
			GOBLIN_TINKERER,
			WIZARD,
			CLOWN,
			SKELETON_ARCHER,
			GOBLIN_ARCHER,
			VILE_SPIT,
			WALL_OF_FLESH,
			WALL_OF_FLESH_EYE,
			THE_HUNGRY,
			THE_HUNGRY_II,
			LEECH_HEAD,
			LEECH_BODY,
			LEECH_TAIL,
			CHAOS_ELEMENTAL,
			SLIMER,
			GASTROPOD,
			BOUND_MECHANIC,
			MECHANIC,
			RETINAZER,
			SPAZMATISM,
			SKELETRON_PRIME,
			PRIME_CANNON,
			PRIME_SAW,
			PRIME_VICE,
			PRIME_LASER,
			BALD_ZOMBIE,
			WANDERING_EYE,
			THE_DESTROYER_HEAD,
			THE_DESTROYER_BODY,
			THE_DESTROYER_TAIL,
			ILLUMINANT_BAT,
			ILLUMINANT_SLIME,
			PROBE,
			POSSESSED_ARMOR,
			TOXIC_SLUDGE,
			SANTA_CLAUS,
			SNOWMAN_GANGSTA,
			MISTER_STABBY,
			SNOW_BALLA,
			SUICIDE_SNOWMAN, // The unused exploding one
			ALBINO_ANTLION,
			ORKA, // Not how its spelt...
			VAMPIRE_MINER,
			SHADOW_SLIME,
			SHADOW_HAMMER,
			SHADOW_MUMMY,
			SPECTRAL_GASTROPOD,
			SPECTRAL_ELEMENTAL,
			SPECTRAL_MUMMY,
			DRAGON_SNATCHER,
			DRAGON_HORNET,
			DRAGON_SKULL,
			ARCH_WYVERN_HEAD,
			ARCH_WYVERN_LEGS,
			ARCH_WYVERN_BODY1,
			ARCH_WYVERN_BODY2,
			ARCH_WYVERN_BODY3,
			ARCH_WYVERN_TAIL,
			ARCH_DEMON,
			OCRAM,
			SERVANT_OF_OCRAM,
			CATARACT_EYE,
			SLEEPY_EYE,
			DIALATED_EYE,
			GREEN_EYE,
			PURPLE_EYE,
			PINCUSHION_ZOMBIE,
			SLIMED_ZOMBIE,
			SWAMP_ZOMBIE,
			TWIGGY_ZOMBIE,
			FEMALE_ZOMBIE,
			ZOMBIE_MUSHROOM,
			ZOMBIE_MUSHROOM_HAT,
			NUM_TYPES
		}

		public const int MaxNumNPCs = 196;

		private const int SpawnSpaceX = 3;

		private const int SpawnSpaceY = 3;

		public const int SpawnWidth = 1920;

		public const int SpawnHeight = 1080;

		public const int SafeRangeX = (int)(SpawnWidth / 16 * 0.52);

		public const int SafeRangeY = (int)(SpawnHeight / 16 * 0.52);

		private const int SpawnRangeX = (int)(SpawnWidth / 16 * 0.7);

		private const int SpawnRangeY = (int)(SpawnHeight / 16 * 0.7);

		private const int ActiveRangeX = (int)(SpawnWidth * 1.7);

		private const int ActiveRangeY = (int)(SpawnHeight * 1.7);

		private const int TownRangeX = SpawnWidth;

		private const int TownRangeY = SpawnHeight;

		private const int NPCActiveTime = 750;

		private const int DefaultSpawnRate = 600;

		private const int DefaultMaxSpawns = 5;

		private const int DrawOnStrike = 96;

		public const int DrawNameWhenNearby = 32;

		public const int MaxNumNPCTypes = (int)ID.NUM_TYPES;

		public const int MaxNumNamedNPCs = 125;

		public const int MaxNumNPCBuffs = 5;

		public const int MaxNumTownNPCs = 10;

		public static string[] TypeNames = new string[MaxNumNamedNPCs];

		public static byte[] NpcFrameCount = new byte[MaxNumNPCTypes]
		{
			1,
			2,
			2,
			3,
			6,
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
			1,
			2,
			16,
			14,
			16,
			14,
			15,
			16,
			2,
			10,
			1,
			16,
			16,
			16,
			3,
			1,
			15,
			3,
			1,
			3,
			1,
			1,
			16,
			16,
			1,
			1,
			1,
			3,
			3,
			15,
			3,
			7,
			7,
			4,
			5,
			5,
			5,
			3,
			3,
			16,
			6,
			3,
			6,
			6,
			2,
			5,
			3,
			2,
			7,
			7,
			4,
			2,
			8,
			1,
			5,
			1,
			2,
			4,
			16,
			5,
			4,
			4,
			15,
			15,
			15,
			15,
			2,
			4,
			6,
			6,
			18,
			16,
			1,
			1,
			1,
			1,
			1,
			1,
			4,
			3,
			1,
			1,
			1,
			1,
			1,
			1,
			5,
			6,
			7,
			16,
			1,
			1,
			16,
			16,
			12,
			20,
			21,
			1,
			2,
			2,
			3,
			6,
			1,
			1,
			1,
			15,
			4,
			11,
			1,
			14,
			6,
			6,
			3,
			1,
			2,
			2,
			1,
			3,
			4,
			1,
			2,
			1,
			4,
			2,
			1,
			15,
			3,
			16,
			4,
			5,
			7,
			3,
			5,
			4,
			15,
			2,
			6,
			15,
			11,
			15,
			15,
			3,
			3,
			3,
			1,
			1,
			1,
			1,
			1,
			1,
			2,
			6,
			2,

			2, // 1.01 additions here
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			3,
			3,
			3
		};

		public static int WoF = -1;

		public static int WoFTop;

		public static int WoFBottom;

		public static int WoFFront = 0;

		private static bool NoSpawnCycle = false;

		public static short CheckForSpawnsTimer = 0;

		public Vector2[] OldPos = new Vector2[10];

		public short NetSpam;

		public short NetSkip;

		public bool NetAlways;

		public int RealLife = -1;

		public float NpcSlots = 1f;

		public bool IsWet;

		public byte WetCount;

		public bool LavaWet;

		public Buff[] ActiveBuffs = new Buff[MaxNumNPCBuffs];

		public bool[] BuffImmune = new bool[(int)Buff.ID.NUM_TYPES];

		public bool IsPoisoned;

		public bool IsConfused;

		public int LifeRegenCount;

		public static bool HasDownedBoss1 = false;

		public static bool HasDownedBoss2 = false;

		public static bool HasDownedBoss3 = false;

		public static bool HasSavedGoblin = false;

		public static bool HasSavedWizard = false;

		public static bool HasSavedMech = false;

		public static bool HasDownedGoblins = false;

		public static bool HasDownedFrost = false;

		public static bool HasDownedClown = false;

		private static int SpawnRate = DefaultSpawnRate;

		private static int MaxSpawns = DefaultMaxSpawns;

		public byte Active;

		public byte Type;

		public bool WasJustHit;

		public bool HasNoGravity;

		public bool HasNoTileCollide;

		public bool ShouldNetUpdate;

		public bool ShouldNetUpdate2;

		public bool HasXCollision;

		public bool HasYCollision;

		public bool IsBoss;

		public bool IsBehindTiles;

		public bool IsLavaImmune;

		public bool DontTakeDamage;

		public short DrawMyName;

		public bool IsTownNPC;

		public bool IsHomeless;

		public bool IsFriendly;

		private bool HasClosedDoor;

		private bool WasHomeless;

		public Vector2 OldPosition;

		public Vector2 OldVelocity;

		public Vector2 Position;

		public Vector2 Velocity;

		public Rectangle XYWH;

		public ushort Width;

		public ushort Height;

		public byte[] Immunities = new byte[Player.MaxNumPlayers + 1];

		public sbyte Direction = 1;

		public sbyte DirectionY = 1;

		public byte AIAction;

		public byte AIStyle;

		public byte Target = Player.MaxNumPlayers;

		public float AI0;

		public float AI1;

		public float AI2;

		public float AI3;

		public int LocalAI0;

		public int LocalAI1;

		public int LocalAI2;

		public int LocalAI3;

		public int TimeLeft;

		public int Damage;

		public int Defense;

		public int DefDamage;

		public short DefDefense;

		public short SoundDelay;

		public short SoundHit;

		public short SoundKilled;

		public int HealthBarLife;

		public int Life;

		public int LifeMax;

		public Rectangle TargetRect;

		public float FrameCounter;

		public short FrameY;

		public short FrameHeight;

		public Color Colour;

		public float Scale = 1f;

		public float KnockBackResist = 1f;

		public byte Alpha;

		public sbyte SpriteDirection = -1;

		public sbyte OldDirection;

		public sbyte OldDirectionY;

		public short OldTarget;

		public short WhoAmI;

		public float Rotation;

		public float Value;

		public short NetID;

		public short HomeTileX = -1;

		public short HomeTileY = -1;

		public short OldHomeTileX = -1;

		public short OldHomeTileY = -1;

		public short DoorX;

		public short DoorY;

		public short FriendlyRegen;

		public string TypeName;

		public string DisplayName;

		public static void ClearNames()
		{
			for (int NamedIdx = 0; NamedIdx < MaxNumNamedNPCs; NamedIdx++)
			{
				TypeNames[NamedIdx] = null;
			}
		}

		public bool HasName()
		{
			if (Type < MaxNumNamedNPCs)
			{
				return TypeNames[Type] != null;
			}
			return false;
		}

		public string GetName()
		{
			return TypeNames[Type];
		}

		public static void SetNames()
		{
			if (TypeNames[(int)ID.NURSE] == null)
			{
				string NurseName;
				switch (WorldGen.genRand.Next(23))
				{
				case 0:
					NurseName = "Molly";
					break;
				case 1:
					NurseName = "Amy";
					break;
				case 2:
					NurseName = "Claire";
					break;
				case 3:
					NurseName = "Emily";
					break;
				case 4:
					NurseName = "Katie";
					break;
				case 5:
					NurseName = "Madeline";
					break;
				case 6:
					NurseName = "Katelyn";
					break;
				case 7:
					NurseName = "Emma";
					break;
				case 8:
					NurseName = "Abigail";
					break;
				case 9:
					NurseName = "Carly";
					break;
				case 10:
					NurseName = "Jenna";
					break;
				case 11:
					NurseName = "Heather";
					break;
				case 12:
					NurseName = "Katherine";
					break;
				case 13:
					NurseName = "Caitlin";
					break;
				case 14:
					NurseName = "Kaitlin";
					break;
				case 15:
					NurseName = "Holly";
					break;
				case 16:
					NurseName = "Kaitlyn";
					break;
				case 17:
					NurseName = "Hannah";
					break;
				case 18:
					NurseName = "Kathryn";
					break;
				case 19:
					NurseName = "Lorraine";
					break;
				case 20:
					NurseName = "Helen";
					break;
				case 21:
					NurseName = "Kayla";
					break;
				default:
					NurseName = "Allison";
					break;
				}
				TypeNames[(int)ID.NURSE] = NurseName;
			}
			if (TypeNames[(int)ID.MECHANIC] == null)
			{
				string MechName;
				switch (WorldGen.genRand.Next(24))
				{
				case 0:
					MechName = "Shayna";
					break;
				case 1:
					MechName = "Korrie";
					break;
				case 2:
					MechName = "Ginger";
					break;
				case 3:
					MechName = "Brooke";
					break;
				case 4:
					MechName = "Jenny";
					break;
				case 5:
					MechName = "Autumn";
					break;
				case 6:
					MechName = "Nancy";
					break;
				case 7:
					MechName = "Ella";
					break;
				case 8:
					MechName = "Kayla";
					break;
				case 9:
					MechName = "Beth";
					break;
				case 10:
					MechName = "Sophia";
					break;
				case 11:
					MechName = "Marshanna";
					break;
				case 12:
					MechName = "Lauren";
					break;
				case 13:
					MechName = "Trisha";
					break;
				case 14:
					MechName = "Shirlena";
					break;
				case 15:
					MechName = "Sheena";
					break;
				case 16:
					MechName = "Ellen";
					break;
				case 17:
					MechName = "Amy";
					break;
				case 18:
					MechName = "Dawn";
					break;
				case 19:
					MechName = "Susana";
					break;
				case 20:
					MechName = "Meredith";
					break;
				case 21:
					MechName = "Selene";
					break;
				case 22:
					MechName = "Terra";
					break;
				default:
					MechName = "Sally";
					break;
				}
				TypeNames[(int)ID.MECHANIC] = MechName;
			}
			if (TypeNames[(int)ID.ARMS_DEALER] == null)
			{
				string DealerName;
				switch (WorldGen.genRand.Next(23))
				{
				case 0:
					DealerName = "DeShawn";
					break;
				case 1:
					DealerName = "DeAndre";
					break;
				case 2:
					DealerName = "Marquis";
					break;
				case 3:
					DealerName = "Darnell";
					break;
				case 4:
					DealerName = "Terrell";
					break;
				case 5:
					DealerName = "Malik";
					break;
				case 6:
					DealerName = "Trevon";
					break;
				case 7:
					DealerName = "Tyrone";
					break;
				case 8:
					DealerName = "Willie";
					break;
				case 9:
					DealerName = "Dominique";
					break;
				case 10:
					DealerName = "Demetrius";
					break;
				case 11:
					DealerName = "Reginald";
					break;
				case 12:
					DealerName = "Jamal";
					break;
				case 13:
					DealerName = "Maurice";
					break;
				case 14:
					DealerName = "Jalen";
					break;
				case 15:
					DealerName = "Darius";
					break;
				case 16:
					DealerName = "Xavier";
					break;
				case 17:
					DealerName = "Terrance";
					break;
				case 18:
					DealerName = "Andre";
					break;
				case 19:
					DealerName = "Dante";
					break;
				case 20:
					DealerName = "Brimst";
					break;
				case 21:
					DealerName = "Bronson";
					break;
				default:
					DealerName = "Darryl";
					break;
				}
				TypeNames[(int)ID.ARMS_DEALER] = DealerName;
			}
			if (TypeNames[(int)ID.GUIDE] == null)
			{
				string GuideName;
				switch (WorldGen.genRand.Next(35))
				{
				case 0:
					GuideName = "Jake";
					break;
				case 1:
					GuideName = "Connor";
					break;
				case 2:
					GuideName = "Tanner";
					break;
				case 3:
					GuideName = "Wyatt";
					break;
				case 4:
					GuideName = "Cody";
					break;
				case 5:
					GuideName = "Dustin";
					break;
				case 6:
					GuideName = "Luke";
					break;
				case 7:
					GuideName = "Jack";
					break;
				case 8:
					GuideName = "Scott";
					break;
				case 9:
					GuideName = "Logan";
					break;
				case 10:
					GuideName = "Cole";
					break;
				case 11:
					GuideName = "Lucas";
					break;
				case 12:
					GuideName = "Bradley";
					break;
				case 13:
					GuideName = "Jacob";
					break;
				case 14:
					GuideName = "Garrett";
					break;
				case 15:
					GuideName = "Dylan";
					break;
				case 16:
					GuideName = "Maxwell";
					break;
				case 17:
					GuideName = "Steve";
					break;
				case 18:
					GuideName = "Brett";
					break;
				case 19:
					GuideName = "Andrew";
					break;
				case 20:
					GuideName = "Harley";
					break;
				case 21:
					GuideName = "Kyle";
					break;
				case 22:
					GuideName = "Jake";
					break;
				case 23:
					GuideName = "Ryan";
					break;
				case 24:
					GuideName = "Jeffrey";
					break;
				case 25:
					GuideName = "Seth";
					break;
				case 26:
					GuideName = "Marty";
					break;
				case 27:
					GuideName = "Brandon";
					break;
				case 28:
					GuideName = "Zach";
					break;
				case 29:
					GuideName = "Jeff";
					break;
				case 30:
					GuideName = "Daniel";
					break;
				case 31:
					GuideName = "Trent";
					break;
				case 32:
					GuideName = "Kevin";
					break;
				case 33:
					GuideName = "Brian";
					break;
				default:
					GuideName = "Colin";
					break;
				}
				TypeNames[(int)ID.GUIDE] = GuideName;
			}
			if (TypeNames[(int)ID.DRYAD] == null)
			{
				string DryadName;
				switch (WorldGen.genRand.Next(22))
				{
				case 0:
					DryadName = "Alalia";
					break;
				case 1:
					DryadName = "Alalia";
					break;
				case 2:
					DryadName = "Alura";
					break;
				case 3:
					DryadName = "Ariella";
					break;
				case 4:
					DryadName = "Caelia";
					break;
				case 5:
					DryadName = "Calista";
					break;
				case 6:
					DryadName = "Chryseis";
					break;
				case 7:
					DryadName = "Emerenta";
					break;
				case 8:
					DryadName = "Elysia";
					break;
				case 9:
					DryadName = "Evvie";
					break;
				case 10:
					DryadName = "Faye";
					break;
				case 11:
					DryadName = "Felicitae";
					break;
				case 12:
					DryadName = "Lunette";
					break;
				case 13:
					DryadName = "Nata";
					break;
				case 14:
					DryadName = "Nissa";
					break;
				case 15:
					DryadName = "Tatiana";
					break;
				case 16:
					DryadName = "Rosalva";
					break;
				case 17:
					DryadName = "Shea";
					break;
				case 18:
					DryadName = "Tania";
					break;
				case 19:
					DryadName = "Isis";
					break;
				case 20:
					DryadName = "Celestia";
					break;
				default:
					DryadName = "Xylia";
					break;
				}
				TypeNames[(int)ID.DRYAD] = DryadName;
			}
			if (TypeNames[(int)ID.DEMOLITIONIST] == null)
			{
				string DemoName;
				switch (WorldGen.genRand.Next(22))
				{
				case 0:
					DemoName = "Dolbere";
					break;
				case 1:
					DemoName = "Bazdin";
					break;
				case 2:
					DemoName = "Durim";
					break;
				case 3:
					DemoName = "Tordak";
					break;
				case 4:
					DemoName = "Garval";
					break;
				case 5:
					DemoName = "Morthal";
					break;
				case 6:
					DemoName = "Oten";
					break;
				case 7:
					DemoName = "Dolgen";
					break;
				case 8:
					DemoName = "Gimli";
					break;
				case 9:
					DemoName = "Gimut";
					break;
				case 10:
					DemoName = "Duerthen";
					break;
				case 11:
					DemoName = "Beldin";
					break;
				case 12:
					DemoName = "Jarut";
					break;
				case 13:
					DemoName = "Ovbere";
					break;
				case 14:
					DemoName = "Norkas";
					break;
				case 15:
					DemoName = "Dolgrim";
					break;
				case 16:
					DemoName = "Boften";
					break;
				case 17:
					DemoName = "Norsun";
					break;
				case 18:
					DemoName = "Dias";
					break;
				case 19:
					DemoName = "Fikod";
					break;
				case 20:
					DemoName = "Urist";
					break;
				default:
					DemoName = "Darur";
					break;
				}
				TypeNames[(int)ID.DEMOLITIONIST] = DemoName;
			}
			if (TypeNames[(int)ID.WIZARD] == null)
			{
				string WizardName;
				switch (WorldGen.genRand.Next(21))
				{
				case 0:
					WizardName = "Dalamar";
					break;
				case 1:
					WizardName = "Dulais";
					break;
				case 2:
					WizardName = "Elric";
					break;
				case 3:
					WizardName = "Arddun";
					break;
				case 4:
					WizardName = "Maelor";
					break;
				case 5:
					WizardName = "Leomund";
					break;
				case 6:
					WizardName = "Hirael";
					break;
				case 7:
					WizardName = "Gwentor";
					break;
				case 8:
					WizardName = "Greum";
					break;
				case 9:
					WizardName = "Gearroid";
					break;
				case 10:
					WizardName = "Fizban";
					break;
				case 11:
					WizardName = "Ningauble";
					break;
				case 12:
					WizardName = "Seonag";
					break;
				case 13:
					WizardName = "Sargon";
					break;
				case 14:
					WizardName = "Merlyn";
					break;
				case 15:
					WizardName = "Magius";
					break;
				case 16:
					WizardName = "Berwyn";
					break;
				case 17:
					WizardName = "Arwyn";
					break;
				case 18:
					WizardName = "Alasdair";
					break;
				case 19:
					WizardName = "Tagar";
					break;
				default:
					WizardName = "Xanadu";
					break;
				}
				TypeNames[(int)ID.WIZARD] = WizardName;
			}
			if (TypeNames[(int)ID.MERCHANT] == null)
			{
				string MerchName;
				switch (WorldGen.genRand.Next(23))
				{
				case 0:
					MerchName = "Alfred";
					break;
				case 1:
					MerchName = "Barney";
					break;
				case 2:
					MerchName = "Calvin";
					break;
				case 3:
					MerchName = "Edmund";
					break;
				case 4:
					MerchName = "Edwin";
					break;
				case 5:
					MerchName = "Eugene";
					break;
				case 6:
					MerchName = "Frank";
					break;
				case 7:
					MerchName = "Frederick";
					break;
				case 8:
					MerchName = "Gilbert";
					break;
				case 9:
					MerchName = "Gus";
					break;
				case 10:
					MerchName = "Wilbur";
					break;
				case 11:
					MerchName = "Seymour";
					break;
				case 12:
					MerchName = "Louis";
					break;
				case 13:
					MerchName = "Humphrey";
					break;
				case 14:
					MerchName = "Harold";
					break;
				case 15:
					MerchName = "Milton";
					break;
				case 16:
					MerchName = "Mortimer";
					break;
				case 17:
					MerchName = "Howard";
					break;
				case 18:
					MerchName = "Walter";
					break;
				case 19:
					MerchName = "Finn";
					break;
				case 20:
					MerchName = "Isacc";
					break;
				case 21:
					MerchName = "Joseph";
					break;
				default:
					MerchName = "Ralph";
					break;
				}
				TypeNames[(int)ID.MERCHANT] = MerchName;
			}
			if (TypeNames[(int)ID.CLOTHIER] == null)
			{
				string ClothName;
				switch (WorldGen.genRand.Next(24))
				{
				case 0:
					ClothName = "Sebastian";
					break;
				case 1:
					ClothName = "Rupert";
					break;
				case 2:
					ClothName = "Clive";
					break;
				case 3:
					ClothName = "Nigel";
					break;
				case 4:
					ClothName = "Mervyn";
					break;
				case 5:
					ClothName = "Cedric";
					break;
				case 6:
					ClothName = "Pip";
					break;
				case 7:
					ClothName = "Cyril";
					break;
				case 8:
					ClothName = "Fitz";
					break;
				case 9:
					ClothName = "Lloyd";
					break;
				case 10:
					ClothName = "Arthur";
					break;
				case 11:
					ClothName = "Rodney";
					break;
				case 12:
					ClothName = "Graham";
					break;
				case 13:
					ClothName = "Edward";
					break;
				case 14:
					ClothName = "Alfred";
					break;
				case 15:
					ClothName = "Edmund";
					break;
				case 16:
					ClothName = "Henry";
					break;
				case 17:
					ClothName = "Herald";
					break;
				case 18:
					ClothName = "Roland";
					break;
				case 19:
					ClothName = "Lincoln";
					break;
				case 20:
					ClothName = "Lloyd";
					break;
				case 21:
					ClothName = "Edgar";
					break;
				case 22:
					ClothName = "Eustace";
					break;
				default:
					ClothName = "Rodrick";
					break;
				}
				TypeNames[(int)ID.CLOTHIER] = ClothName;
			}
			if (TypeNames[(int)ID.GOBLIN_TINKERER] == null)
			{
				string GoblinName;
				switch (WorldGen.genRand.Next(25))
				{
				case 0:
					GoblinName = "Grodax";
					break;
				case 1:
					GoblinName = "Sarx";
					break;
				case 2:
					GoblinName = "Xon";
					break;
				case 3:
					GoblinName = "Mrunok";
					break;
				case 4:
					GoblinName = "Nuxatk";
					break;
				case 5:
					GoblinName = "Tgerd";
					break;
				case 6:
					GoblinName = "Darz";
					break;
				case 7:
					GoblinName = "Smador";
					break;
				case 8:
					GoblinName = "Stazen";
					break;
				case 9:
					GoblinName = "Mobart";
					break;
				case 10:
					GoblinName = "Knogs";
					break;
				case 11:
					GoblinName = "Tkanus";
					break;
				case 12:
					GoblinName = "Negurk";
					break;
				case 13:
					GoblinName = "Nort";
					break;
				case 14:
					GoblinName = "Durnok";
					break;
				case 15:
					GoblinName = "Trogem";
					break;
				case 16:
					GoblinName = "Stezom";
					break;
				case 17:
					GoblinName = "Gnudar";
					break;
				case 18:
					GoblinName = "Ragz";
					break;
				case 19:
					GoblinName = "Fahd";
					break;
				case 20:
					GoblinName = "Xanos";
					break;
				case 21:
					GoblinName = "Arback";
					break;
				case 22:
					GoblinName = "Fjell";
					break;
				case 23:
					GoblinName = "Dalek";
					break;
				default:
					GoblinName = "Knub";
					break;
				}
				TypeNames[(int)ID.GOBLIN_TINKERER] = GoblinName;
			}
		}

		public void NetDefaults(int NPCType)
		{
			if (NPCType < 0)
			{
				switch (NPCType)
				{
				case -1:
					SetDefaults("Slimeling");
					break;
				case -2:
					SetDefaults("Slimer2");
					break;
				case -3:
					SetDefaults("Green Slime");
					break;
				case -4:
					SetDefaults("Pinky");
					break;
				case -5:
					SetDefaults("Baby Slime");
					break;
				case -6:
					SetDefaults("Black Slime");
					break;
				case -7:
					SetDefaults("Purple Slime");
					break;
				case -8:
					SetDefaults("Red Slime");
					break;
				case -9:
					SetDefaults("Yellow Slime");
					break;
				case -10:
					SetDefaults("Jungle Slime");
					break;
				case -11:
					SetDefaults("Little Eater");
					break;
				case -12:
					SetDefaults("Big Eater");
					break;
				case -13:
					SetDefaults("Short Bones");
					break;
				case -14:
					SetDefaults("Big Boned");
					break;
				case -15:
					SetDefaults("Heavy Skeleton");
					break;
				case -16:
					SetDefaults("Little Stinger");
					break;
				case -17:
					SetDefaults("Big Stinger");
					break;
				case -18:
					SetDefaults("Slimeling2");
					break;
				}
			}
			else
			{
				SetDefaults(NPCType);
			}
		}

		public void SetDefaults(string Name)
		{
			switch (Name)
			{
			case "Slimeling":
				SetDefaults((int)ID.CORRUPT_SLIME, 0.6);
				TypeName = Name;
				Damage = 45;
				Defense = 10;
				Life = 90;
				KnockBackResist = 1.2f;
				Value = 100f;
				NetID = -1;
				break;
			case "Slimeling2":
				SetDefaults((int)ID.SHADOW_SLIME, 0.6);
				TypeName = Name;
				Damage = 45;
				Defense = 10;
				Life = 105;
				KnockBackResist = 1.2f;
				Value = 100f;
				NetID = -18;
				break;
			case "Slimer2":
				SetDefaults((int)ID.CORRUPT_SLIME, 0.9);
				TypeName = Name;
				Damage = 45;
				Defense = 20;
				Life = 90;
				KnockBackResist = 1.2f;
				Value = 100f;
				NetID = -2;
				break;
			case "Green Slime":
				SetDefaults((int)ID.SLIME, 0.9);
				TypeName = Name;
				Damage = 6;
				Defense = 0;
				Life = 14;
				KnockBackResist = 1.2f;
				Colour = new Color(0, 220, 40, 100);
				Value = 3f;
				NetID = -3;
				break;
			case "Pinky":
				SetDefaults((int)ID.SLIME, 0.6);
				TypeName = Name;
				Damage = 5;
				Defense = 5;
				Life = 150;
				KnockBackResist = 1.4f;
				Colour = new Color(250, 30, 90, 90);
				Value = 10000f;
				NetID = -4;
				break;
			case "Baby Slime":
				SetDefaults((int)ID.SLIME, 0.9);
				TypeName = Name;
				Damage = 13;
				Defense = 4;
				Life = 30;
				KnockBackResist = 0.95f;
				Alpha = 120;
				Colour = new Color(0, 0, 0, 50);
				Value = 10f;
				NetID = -5;
				break;
			case "Black Slime":
				SetDefaults((int)ID.SLIME);
				TypeName = Name;
				Damage = 15;
				Defense = 4;
				Life = 45;
				Colour = new Color(0, 0, 0, 50);
				Value = 20f;
				NetID = -6;
				break;
			case "Purple Slime":
				SetDefaults((int)ID.SLIME, 1.2);
				TypeName = Name;
				Damage = 12;
				Defense = 6;
				Life = 40;
				KnockBackResist = 0.9f;
				Colour = new Color(200, 0, 255, 150);
				Value = 10f;
				NetID = -7;
				break;
			case "Red Slime":
				SetDefaults((int)ID.SLIME);
				TypeName = Name;
				Damage = 12;
				Defense = 4;
				Life = 35;
				Colour = new Color(255, 30, 0, 100);
				Value = 8f;
				NetID = -8;
				break;
			case "Yellow Slime":
				SetDefaults((int)ID.SLIME, 1.2);
				TypeName = Name;
				Damage = 15;
				Defense = 7;
				Life = 45;
				Colour = new Color(255, 255, 0, 100);
				Value = 10f;
				NetID = -9;
				break;
			case "Jungle Slime":
				SetDefaults((int)ID.SLIME, 1.1);
				TypeName = Name;
				Damage = 18;
				Defense = 6;
				Life = 60;
				Colour = new Color(143, 215, 93, 100);
				Value = 500f;
				NetID = -10;
				break;
			case "Little Eater":
				SetDefaults((int)ID.EATER_OF_SOULS, 0.85);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -11;
				break;
			case "Big Eater":
				SetDefaults((int)ID.EATER_OF_SOULS, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -12;
				break;
			case "Short Bones":
				SetDefaults((int)ID.BONES, 0.9);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NetID = -13;
				break;
			case "Big Boned":
				SetDefaults((int)ID.BONES, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)((double)(Damage * Scale) * 1.1);
				Life = (int)((double)(Life * Scale) * 1.1);
				Value = (int)(Value * Scale);
				NpcSlots = 2f;
				KnockBackResist *= 2f - Scale;
				NetID = -14;
				break;
			case "Heavy Skeleton":
				SetDefaults((int)ID.ARMORED_SKELETON, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)((double)(Damage * Scale) * 1.1);
				Life = 400;
				Value = (int)(Value * Scale);
				NpcSlots = 2f;
				KnockBackResist *= 2f - Scale;
				Height = 44;
				NetID = -15;
				break;
			case "Little Stinger":
				SetDefaults((int)ID.HORNET, 0.85);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -16;
				break;
			case "Big Stinger":
				SetDefaults((int)ID.HORNET, 1.2);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -17;
				break;
#if VERSION_101
			case "Small Zombie":
				SetDefaults((int)ID.ZOMBIE, 0.9);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -26;
				break;
			case "Big Zombie":
				SetDefaults((int)ID.ZOMBIE, 1.1);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -27;
				break;
			case "Small Bald Zombie":
				SetDefaults((int)ID.BALD_ZOMBIE, 0.85);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -28;
				break;
			case "Big Bald Zombie":
				SetDefaults((int)ID.BALD_ZOMBIE, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -29;
				break;
			case "Small Pincushion Zombie":
				SetDefaults((int)ID.PINCUSHION_ZOMBIE, 0.93);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -30;
				break;
			case "Big Pincushion Zombie":
				SetDefaults((int)ID.PINCUSHION_ZOMBIE, 1.13);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -31;
				break;
			case "Small Slimed Zombie":
				SetDefaults((int)ID.SLIMED_ZOMBIE, 0.89);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -32;
				break;
			case "Big Slimed Zombie":
				SetDefaults((int)ID.SLIMED_ZOMBIE, 1.11);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -33;
				break;
			case "Small Swamp Zombie":
				SetDefaults((int)ID.SWAMP_ZOMBIE, 0.87);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -34;
				break;
			case "Big Swamp Zombie":
				SetDefaults((int)ID.SWAMP_ZOMBIE, 1.13);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -35;
				break;
			case "Small Twiggy Zombie":
				SetDefaults((int)ID.TWIGGY_ZOMBIE, 0.92);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -36;
				break;
			case "Big Twiggy Zombie":
				SetDefaults((int)ID.TWIGGY_ZOMBIE, 1.08);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -37;
				break;
			case "Cataract Eye 2":
				SetDefaults((int)ID.CATARACT_EYE, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -38;
				break;
			case "Sleepy Eye 2":
				SetDefaults((int)ID.SLEEPY_EYE, 1.1);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -39;
				break;
			case "Dialated Eye 2":
				SetDefaults((int)ID.DIALATED_EYE, 0.9);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -40;
				break;
			case "Green Eye 2":
				SetDefaults((int)ID.GREEN_EYE, 0.85);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -41;
				break;
			case "Purple Eye 2":
				SetDefaults((int)ID.PURPLE_EYE, 1.1);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -42;
				break;
			case "Demon Eye 2":
				SetDefaults((int)ID.DEMON_EYE, 1.15);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -43;
				break;
			case "Small Female Zombie":
				SetDefaults((int)ID.FEMALE_ZOMBIE, 0.87);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -44;
				break;
			case "Big Female Zombie":
				SetDefaults((int)ID.FEMALE_ZOMBIE, 1.05);
				TypeName = Name;
				Defense = (int)(Defense * Scale);
				Damage = (int)(Damage * Scale);
				Life = (int)(Life * Scale);
				Value = (int)(Value * Scale);
				NpcSlots *= Scale;
				KnockBackResist *= 2f - Scale;
				NetID = -45;
				break;
#endif
			}
			DisplayName = Lang.NpcName(NetID);
			LifeMax = Life;
			HealthBarLife = Life;
			DefDamage = Damage;
			DefDefense = (short)Defense;
		}

		public bool CanTalk()
		{
			if (!IsTownNPC && Type != (int)ID.BOUND_GOBLIN && Type != (int)ID.BOUND_WIZARD)
			{
				return Type == (int)ID.BOUND_MECHANIC;
			}
			return true;
		}

		public static bool MechSpawn(int NPCX, int NPCY, int NPCType)
		{
			int Counter1 = 0;
			int Counter2 = 0;
			int Counter3 = 0;
			for (int NPCIdx = 0; NPCIdx < MaxNumNPCs; NPCIdx++) // So this is for the statues spawning NPCs
			{
				if (Main.NPCSet[NPCIdx].Active != 0 && Main.NPCSet[NPCIdx].Type == NPCType)
				{
					Counter1++;
					Vector2 WorldCoords = new Vector2(NPCX, NPCY);
					float XCoord = Main.NPCSet[NPCIdx].Position.X - WorldCoords.X;
					float YCoord = Main.NPCSet[NPCIdx].Position.Y - WorldCoords.Y;
					float Marker = XCoord * XCoord + YCoord * YCoord;
					if (Marker < 40000f)
					{
						Counter2++;
					}
					if (Marker < 360000f)
					{
						Counter3++;
					}
				}
			}
			if (Counter2 >= 3 || Counter3 >= 6 || Counter1 >= 10)
			{
				return false;
			}
			return true;
		}

		public int GetHeadTextureID()
		{
			switch ((ID)Type)
			{
			case ID.MERCHANT:
				return 2;
			case ID.NURSE:
				return 3;
			case ID.ARMS_DEALER:
				return 6;
			case ID.DRYAD:
				return 5;
			case ID.GUIDE:
				return 1;
			case ID.DEMOLITIONIST:
				return 4;
			case ID.CLOTHIER:
				return 7;
			case ID.GOBLIN_TINKERER:
				return 9;
			case ID.WIZARD:
				return 10;
			case ID.MECHANIC:
				return 8;
			case ID.SANTA_CLAUS:
				return 11;
			default:
				return -1;
			}
		}

		public void SetDefaults(int NPCType, double ScaleOverride = -1.0)
		{
			Type = (byte)NPCType;
			NetID = (short)NPCType;
			NetAlways = false;
			NetSpam = 0;
			DrawMyName = 0;
			for (int StoredIdx = 0; StoredIdx < OldPos.Length; StoredIdx++)
			{
				OldPos[StoredIdx].X = 0f;
				OldPos[StoredIdx].Y = 0f;
			}
			for (int NPCBuffIdx = 0; NPCBuffIdx < MaxNumNPCBuffs; NPCBuffIdx++)
			{
				ActiveBuffs[NPCBuffIdx].Time = 0;
				ActiveBuffs[NPCBuffIdx].Type = 0;
			}
			for (int BuffIdx = 0; BuffIdx < (int)Buff.ID.NUM_TYPES; BuffIdx++)
			{
				BuffImmune[BuffIdx] = false;
			}
			BuffImmune[(int)Buff.ID.CONFUSED] = true;
			NetSkip = -2;
			RealLife = -1;
			LifeRegenCount = 0;
			IsPoisoned = false;
			IsConfused = false;
			WasJustHit = false;
			DontTakeDamage = false;
			NpcSlots = 1f;
			IsLavaImmune = false;
			LavaWet = false;
			WetCount = 0;
			IsWet = false;
			IsTownNPC = false;
			IsHomeless = false;
			HomeTileX = -1;
			HomeTileY = -1;
			IsFriendly = false;
			IsBehindTiles = false;
			IsBoss = false;
			HasNoTileCollide = false;
			Rotation = 0f;
			Active = 1;
			Alpha = 0;
			Colour = default;
			HasXCollision = false;
			HasYCollision = false;
			Direction = 0;
			OldDirection = 0;
			FrameCounter = 0f;
			ShouldNetUpdate = true;
			ShouldNetUpdate2 = false;
			KnockBackResist = 1f;
			TypeName = "";
			HasNoGravity = false;
			Scale = 1f;
			SoundHit = 0;
			SoundKilled = 0;
			SpriteDirection = -1;
			Target = Player.MaxNumPlayers;
			OldTarget = Target;
			TargetRect = default;
			TimeLeft = NPCActiveTime;
			Value = 0f;
			AI0 = 0f;
			AI1 = 0f;
			AI2 = 0f;
			AI3 = 0f;
			LocalAI0 = 0;
			LocalAI1 = 0;
			LocalAI2 = 0;
			LocalAI3 = 0;
			switch ((ID)NPCType)
			{
				case ID.SLIME:
					TypeName = "Blue Slime";
					Width = 24;
					Height = 18;
					AIStyle = 1;
					Damage = 7;
					Defense = 2;
					LifeMax = 25;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 175;
					Colour = new Color(0, 80, 255, 100);
					Value = 25f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.DEMON_EYE:
					TypeName = "Demon Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 18;
					Defense = 2;
					LifeMax = 60;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.ZOMBIE:
					TypeName = "Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 14;
					Defense = 6;
					LifeMax = 45;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 60f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.EYE_OF_CTHULHU:
					TypeName = "Eye of Cthulhu";
					Width = 100;
					Height = 110;
					AIStyle = 4;
					Damage = 15;
					Defense = 12;
					LifeMax = 2800;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					HasNoGravity = true;
					HasNoTileCollide = true;
					TimeLeft = 22500;
					IsBoss = true;
					Value = 30000f;
					NpcSlots = 5f;
					break;
				case ID.OCRAM:
					TypeName = "Ocram";
					Width = 100; // Ocram's dimensions should not be 100/110, but the programmer copied over the EoC's values without adjustment; they should be more like 195/155.
					Height = 110;
					AIStyle = 39;
					Damage = 65;
					Defense = 20;
					LifeMax = 35000;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					HasNoGravity = true;
					HasNoTileCollide = true;
					TimeLeft = 22500;
					IsBoss = true;
					Value = 100000f;
					NpcSlots = 5f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.SERVANT_OF_CTHULHU:
					TypeName = "Servant of Cthulhu";
					Width = 20;
					Height = 20;
					AIStyle = 5;
					Damage = 13;
					Defense = 0;
					LifeMax = 10;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					break;
				case ID.SERVANT_OF_OCRAM:
					TypeName = "Servant of Ocram";
					Width = 20;
					Height = 20;
					AIStyle = 5;
					Damage = 35;
					Defense = 5;
					LifeMax = 130;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					break;
				case ID.EATER_OF_SOULS:
					NpcSlots = 1f;
					TypeName = "Eater of Souls";
					Width = 30;
					Height = 30;
					AIStyle = 5;
					Damage = 22;
					Defense = 8;
					LifeMax = 40;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					KnockBackResist = 0.5f;
					Value = 90f;
					break;
				case ID.DEVOURER_HEAD:
					NpcSlots = 3.5f;
					TypeName = "Devourer Head";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					Damage = 31;
					Defense = 2;
					LifeMax = 100;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 140f;
					NetAlways = true;
					break;
				case ID.DEVOURER_BODY:
					TypeName = "Devourer Body";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 16;
					Defense = 6;
					LifeMax = 100;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 140f;
					break;
				case ID.DEVOURER_TAIL:
					TypeName = "Devourer Tail";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 13;
					Defense = 10;
					LifeMax = 100;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 140f;
					break;
				case ID.GIANT_WORM_HEAD:
					TypeName = "Giant Worm Head";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 8;
					Defense = 0;
					LifeMax = 30;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 40f;
					break;
				case ID.GIANT_WORM_BODY:
					TypeName = "Giant Worm Body";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 4;
					Defense = 4;
					LifeMax = 30;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 40f;
					break;
				case ID.GIANT_WORM_TAIL:
					TypeName = "Giant Worm Tail";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 4;
					Defense = 6;
					LifeMax = 30;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 40f;
					break;
				case ID.EATER_OF_WORLDS_HEAD:
					NpcSlots = 5f;
					TypeName = "Eater of Worlds Head";
					Width = 38;
					Height = 38;
					AIStyle = 6;
					NetAlways = true;
					Damage = 22;
					Defense = 2;
					LifeMax = 65;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 300f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.EATER_OF_WORLDS_BODY:
					TypeName = "Eater of Worlds Body";
					Width = 38;
					Height = 38;
					AIStyle = 6;
					NetAlways = true;
					Damage = 13;
					Defense = 4;
					LifeMax = 150;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 300f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.EATER_OF_WORLDS_TAIL:
					TypeName = "Eater of Worlds Tail";
					Width = 38;
					Height = 38;
					AIStyle = 6;
					NetAlways = true;
					Damage = 11;
					Defense = 8;
					LifeMax = 220;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 300f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.MOTHER_SLIME:
					NpcSlots = 2f;
					TypeName = "Mother Slime";
					Width = 36;
					Height = 24;
					AIStyle = 1;
					Damage = 20;
					Defense = 7;
					LifeMax = 90;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 120;
					Colour = new Color(0, 0, 0, 50);
					Value = 75f;
					Scale = 1.25f;
					KnockBackResist = 0.6f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.MERCHANT:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Merchant";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.NURSE:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Nurse";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.ARMS_DEALER:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Arms Dealer";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.DRYAD:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Dryad";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.SKELETON:
					TypeName = "Skeleton";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 20;
					Defense = 8;
					LifeMax = 60;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 100f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GUIDE:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Guide";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.METEOR_HEAD:
					TypeName = "Meteor Head";
					Width = 22;
					Height = 22;
					AIStyle = 5;
					Damage = 40;
					Defense = 6;
					LifeMax = 26;
					SoundHit = 3;
					SoundKilled = 3;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Value = 80f;
					KnockBackResist = 0.4f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.FIRE_IMP:
					NpcSlots = 3f;
					TypeName = "Fire Imp";
					Width = 18;
					Height = 40;
					AIStyle = 8;
					Damage = 30;
					Defense = 16;
					LifeMax = 70;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					IsLavaImmune = true;
					Value = 350f;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.BURNING_SPHERE:
					TypeName = "Burning Sphere";
					Width = 16;
					Height = 16;
					AIStyle = 9;
					Damage = 30;
					Defense = 0;
					LifeMax = 1;
					SoundHit = 3;
					SoundKilled = 3;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					Alpha = 100;
					break;
				case ID.GOBLIN_PEON:
					TypeName = "Goblin Peon";
					Scale = 0.9f;
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 12;
					Defense = 4;
					LifeMax = 60;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.8f;
					Value = 100f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GOBLIN_THIEF:
					TypeName = "Goblin Thief";
					Scale = 0.95f;
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 20;
					Defense = 6;
					LifeMax = 80;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.7f;
					Value = 200f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GOBLIN_WARRIOR:
					TypeName = "Goblin Warrior";
					Scale = 1.1f;
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 25;
					Defense = 8;
					LifeMax = 110;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					Value = 150f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GOBLIN_SORCERER:
					TypeName = "Goblin Sorcerer";
					Width = 18;
					Height = 40;
					AIStyle = 8;
					Damage = 20;
					Defense = 2;
					LifeMax = 40;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.6f;
					Value = 200f;
					break;
				case ID.CHAOS_BALL:
					TypeName = "Chaos Ball";
					Width = 16;
					Height = 16;
					AIStyle = 9;
					Damage = 20;
					Defense = 0;
					LifeMax = 1;
					SoundHit = 3;
					SoundKilled = 3;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Alpha = 100;
					KnockBackResist = 0f;
					break;
				case ID.BONES:
					TypeName = "Angry Bones";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 26;
					Defense = 8;
					LifeMax = 80;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.8f;
					Value = 130f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.DARK_CASTER:
					TypeName = "Dark Caster";
					Width = 18;
					Height = 40;
					AIStyle = 8;
					Damage = 20;
					Defense = 2;
					LifeMax = 50;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.6f;
					Value = 140f;
					NpcSlots = 2f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.WATER_SPHERE:
					TypeName = "Water Sphere";
					Width = 16;
					Height = 16;
					AIStyle = 9;
					Damage = 20;
					Defense = 0;
					LifeMax = 1;
					SoundHit = 3;
					SoundKilled = 3;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Alpha = 100;
					KnockBackResist = 0f;
					break;
				case ID.CURSED_SKULL:
					TypeName = "Cursed Skull";
					Width = 26;
					Height = 28;
					AIStyle = 10;
					Damage = 35;
					Defense = 6;
					LifeMax = 40;
					SoundHit = 2;
					SoundKilled = 2;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Value = 150f;
					KnockBackResist = 0.2f;
					NpcSlots = 0.75f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.DRAGON_SKULL:
					TypeName = "Dragon Skull";
					Width = 56;
					Height = 28;
					AIStyle = 10;
					Damage = 45;
					Defense = 8;
					LifeMax = 50;
					SoundHit = 2;
					SoundKilled = 2;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Value = 150f;
					KnockBackResist = 0.2f;
					NpcSlots = 0.75f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.SKELETRON_HEAD:
					TypeName = "Skeletron Head";
					Width = 80;
					Height = 102;
					AIStyle = 11;
					Damage = 32;
					Defense = 10;
					LifeMax = 4400;
					SoundHit = 2;
					SoundKilled = 2;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Value = 50000f;
					KnockBackResist = 0f;
					IsBoss = true;
					NpcSlots = 6f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.SKELETRON_HAND:
					TypeName = "Skeletron Hand";
					Width = 52;
					Height = 52;
					AIStyle = 12;
					Damage = 20;
					Defense = 14;
					LifeMax = 600;
					SoundHit = 2;
					SoundKilled = 2;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.OLD_MAN:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Old Man";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.DEMOLITIONIST:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Demolitionist";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.BONE_SERPENT_HEAD:
					NpcSlots = 6f;
					TypeName = "Bone Serpent Head";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 30;
					Defense = 10;
					LifeMax = 250;
					SoundHit = 2;
					SoundKilled = 5;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 1200f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.BONE_SERPENT_BODY:
					TypeName = "Bone Serpent Body";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 15;
					Defense = 12;
					LifeMax = 250;
					SoundHit = 2;
					SoundKilled = 5;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 1200f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.BONE_SERPENT_TAIL:
					TypeName = "Bone Serpent Tail";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 10;
					Defense = 18;
					LifeMax = 250;
					SoundHit = 2;
					SoundKilled = 5;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 1200f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.HORNET:
					TypeName = "Hornet";
					Width = 34;
					Height = 32;
					AIStyle = 5;
					Damage = 34;
					Defense = 12;
					LifeMax = 50;
					SoundHit = 1;
					KnockBackResist = 0.5f;
					SoundKilled = 1;
					Value = 200f;
					HasNoGravity = true;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.DRAGON_HORNET:
					TypeName = "Dragon Hornet";
					Width = 34;
					Height = 32;
					AIStyle = 5;
					Damage = 39;
					Defense = 17;
					LifeMax = 65;
					SoundHit = 1;
					KnockBackResist = 0.5f;
					SoundKilled = 1;
					Value = 200f;
					HasNoGravity = true;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.MAN_EATER:
					HasNoGravity = true;
					HasNoTileCollide = true;
					TypeName = "Man Eater";
					Width = 30;
					Height = 30;
					AIStyle = 13;
					Damage = 42;
					Defense = 14;
					LifeMax = 130;
					SoundHit = 1;
					KnockBackResist = 0f;
					SoundKilled = 1;
					Value = 350f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.UNDEAD_MINER:
					TypeName = "Undead Miner";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 22;
					Defense = 9;
					LifeMax = 70;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 250f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.VAMPIRE_MINER:
					TypeName = "Vampire Miner";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 30;
					Defense = 9;
					LifeMax = 90;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.7f;
					Value = 250f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.TIM:
					TypeName = "Tim";
					Width = 18;
					Height = 40;
					AIStyle = 8;
					Damage = 20;
					Defense = 4;
					LifeMax = 200;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.6f;
					Value = 5000f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.BUNNY:
					TypeName = "Bunny";
					Width = 18;
					Height = 20;
					AIStyle = 7;
					Damage = 0;
					Defense = 0;
					LifeMax = 5;
					SoundHit = 1;
					SoundKilled = 1;
					break;
				case ID.CORRUPT_BUNNY:
					TypeName = "Corrupt Bunny";
					Width = 18;
					Height = 20;
					AIStyle = 3;
					Damage = 20;
					Defense = 4;
					LifeMax = 70;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 500f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.HARPY:
					TypeName = "Harpy";
					Width = 24;
					Height = 34;
					AIStyle = 14;
					Damage = 25;
					Defense = 8;
					LifeMax = 100;
					SoundHit = 1;
					KnockBackResist = 0.6f;
					SoundKilled = 1;
					Value = 300f;
					break;
				case ID.CAVE_BAT:
					NpcSlots = 0.5f;
					TypeName = "Cave Bat";
					Width = 22;
					Height = 18;
					AIStyle = 14;
					Damage = 13;
					Defense = 2;
					LifeMax = 16;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 4;
					Value = 90f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.KING_SLIME:
					IsBoss = true;
					TypeName = "King Slime";
					Width = 98;
					Height = 92;
					AIStyle = 15;
					Damage = 40;
					Defense = 10;
					LifeMax = 2000;
					KnockBackResist = 0f;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 30;
					Value = 10000f;
					Scale = 1.25f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.JUNGLE_BAT:
					NpcSlots = 0.5f;
					TypeName = "Jungle Bat";
					Width = 22;
					Height = 18;
					AIStyle = 14;
					Damage = 20;
					Defense = 4;
					LifeMax = 34;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 4;
					Value = 80f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.DOCTOR_BONES:
					TypeName = "Doctor Bones";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 20;
					Defense = 10;
					LifeMax = 500;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.THE_GROOM:
					TypeName = "The Groom";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 14;
					Defense = 8;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.CLOTHIER:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Clothier";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.GOLDFISH:
					HasNoGravity = true;
					TypeName = "Goldfish";
					Width = 20;
					Height = 18;
					AIStyle = 16;
					Damage = 0;
					Defense = 0;
					LifeMax = 5;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.SNATCHER:
					HasNoTileCollide = true;
					HasNoGravity = true;
					TypeName = "Snatcher";
					Width = 30;
					Height = 30;
					AIStyle = 13;
					Damage = 25;
					Defense = 10;
					LifeMax = 60;
					SoundHit = 1;
					KnockBackResist = 0f;
					SoundKilled = 1;
					Value = 90f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.DRAGON_SNATCHER:
					HasNoTileCollide = true;
					HasNoGravity = true;
					TypeName = "Dragon Snatcher";
					Width = 30;
					Height = 30;
					AIStyle = 13;
					Damage = 30;
					Defense = 15;
					LifeMax = 75;
					SoundHit = 1;
					KnockBackResist = 0f;
					SoundKilled = 1;
					Value = 90f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.CORRUPT_GOLDFISH:
					HasNoGravity = true;
					TypeName = "Corrupt Goldfish";
					Width = 18;
					Height = 20;
					AIStyle = 16;
					Damage = 30;
					Defense = 6;
					LifeMax = 100;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 500f;
					break;
				case ID.PIRANHA:
					NpcSlots = 0.5f;
					HasNoGravity = true;
					TypeName = "Piranha";
					Width = 18;
					Height = 20;
					AIStyle = 16;
					Damage = 25;
					Defense = 2;
					LifeMax = 30;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 50f;
					break;
				case ID.LAVA_SLIME:
					TypeName = "Lava Slime";
					Width = 24;
					Height = 18;
					AIStyle = 1;
					Damage = 15;
					Defense = 10;
					LifeMax = 50;
					SoundHit = 1;
					SoundKilled = 1;
					Scale = 1.1f;
					Alpha = 50;
					IsLavaImmune = true;
					Value = 120f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.HELLBAT:
					NpcSlots = 0.5f;
					TypeName = "Hellbat";
					Width = 22;
					Height = 18;
					AIStyle = 14;
					Damage = 35;
					Defense = 8;
					LifeMax = 46;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 4;
					Value = 120f;
					Scale = 1.1f;
					IsLavaImmune = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.VULTURE:
					TypeName = "Vulture";
					Width = 36;
					Height = 36;
					AIStyle = 17;
					Damage = 15;
					Defense = 4;
					LifeMax = 40;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 60f;
					break;
				case ID.DEMON:
					NpcSlots = 2f;
					TypeName = "Demon";
					Width = 28;
					Height = 48;
					AIStyle = 14;
					Damage = 32;
					Defense = 8;
					LifeMax = 120;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 300f;
					IsLavaImmune = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_DEMON:
					NpcSlots = 2f;
					TypeName = "Arch Demon";
					Width = 28;
					Height = 48;
					AIStyle = 14;
					Damage = 42;
					Defense = 8;
					LifeMax = 140;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 300f;
					IsLavaImmune = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.BLUE_JELLYFISH:
					HasNoGravity = true;
					TypeName = "Blue Jellyfish";
					Width = 26;
					Height = 26;
					AIStyle = 18;
					Damage = 20;
					Defense = 2;
					LifeMax = 30;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 100f;
					Alpha = 20;
					break;
				case ID.PINK_JELLYFISH:
					HasNoGravity = true;
					TypeName = "Pink Jellyfish";
					Width = 26;
					Height = 26;
					AIStyle = 18;
					Damage = 30;
					Defense = 6;
					LifeMax = 70;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 100f;
					Alpha = 20;
					break;
				case ID.SHARK:
					HasNoGravity = true;
					TypeName = "Shark";
					Width = 100;
					Height = 24;
					AIStyle = 16;
					Damage = 40;
					Defense = 2;
					LifeMax = 300;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 400f;
					KnockBackResist = 0.7f;
					break;
				case ID.ORKA:
					HasNoGravity = true;
					TypeName = "Orka";
					Width = 100;
					Height = 24;
					AIStyle = 16;
					Damage = 30;
					Defense = 4;
					LifeMax = 350;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 400f;
					KnockBackResist = 0.6f;
					break;
				case ID.VOODOO_DEMON:
					NpcSlots = 2f;
					TypeName = "Voodoo Demon";
					Width = 28;
					Height = 48;
					AIStyle = 14;
					Damage = 32;
					Defense = 8;
					LifeMax = 140;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 1000f;
					IsLavaImmune = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.CRAB:
					TypeName = "Crab";
					Width = 28;
					Height = 20;
					AIStyle = 3;
					Damage = 20;
					Defense = 10;
					LifeMax = 40;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 60f;
					break;
				case ID.DUNGEON_GUARDIAN:
					TypeName = "Dungeon Guardian";
					Width = 80;
					Height = 102;
					AIStyle = 11;
					Damage = 9000;
					Defense = 9000;
					LifeMax = 9999;
					SoundHit = 2;
					SoundKilled = 2;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ANTLION:
					TypeName = "Antlion";
					Width = 24;
					Height = 24;
					AIStyle = 19;
					Damage = 10;
					Defense = 6;
					LifeMax = 45;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					Value = 60f;
					IsBehindTiles = true;
					break;
				case ID.ALBINO_ANTLION:
					TypeName = "Albino Antlion";
					Width = 24;
					Height = 24;
					AIStyle = 19;
					Damage = 12;
					Defense = 8;
					LifeMax = 60;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					Value = 60f;
					IsBehindTiles = true;
					break;
				case ID.SPIKE_BALL:
					NpcSlots = 0.3f;
					TypeName = "Spike Ball";
					Width = 34;
					Height = 34;
					AIStyle = 20;
					Damage = 32;
					Defense = 100;
					LifeMax = 100;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					HasNoGravity = true;
					HasNoTileCollide = true;
					DontTakeDamage = true;
					Scale = 1.5f;
					break;
				case ID.DUNGEON_SLIME:
					NpcSlots = 2f;
					TypeName = "Dungeon Slime";
					Width = 36;
					Height = 24;
					AIStyle = 1;
					Damage = 30;
					Defense = 7;
					LifeMax = 150;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 60;
					Value = 150f;
					Scale = 1.25f;
					KnockBackResist = 0.6f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.BLAZING_WHEEL:
					NpcSlots = 0.3f;
					TypeName = "Blazing Wheel";
					Width = 34;
					Height = 34;
					AIStyle = 21;
					Damage = 24;
					Defense = 100;
					LifeMax = 100;
					Alpha = 100;
					IsBehindTiles = true;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0f;
					HasNoGravity = true;
					DontTakeDamage = true;
					Scale = 1.2f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.GOBLIN_SCOUT:
					TypeName = "Goblin Scout";
					Scale = 0.95f;
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 20;
					Defense = 6;
					LifeMax = 80;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.7f;
					Value = 200f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.BIRD:
					TypeName = "Bird";
					Width = 14;
					Height = 14;
					AIStyle = 24;
					Damage = 0;
					Defense = 0;
					LifeMax = 5;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					break;
				case ID.PIXIE:
					HasNoGravity = true;
					TypeName = "Pixie";
					Width = 20;
					Height = 20;
					AIStyle = 22;
					Damage = 55;
					Defense = 20;
					LifeMax = 150;
					SoundHit = 5;
					KnockBackResist = 0.6f;
					SoundKilled = 7;
					Value = 350f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.ARMORED_SKELETON:
					TypeName = "Armored Skeleton";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 60;
					Defense = 36;
					LifeMax = 340;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.4f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.MUMMY:
					TypeName = "Mummy";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 50;
					Defense = 16;
					LifeMax = 130;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.6f;
					Value = 600f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.DARK_MUMMY:
					TypeName = "Dark Mummy";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 60;
					Defense = 18;
					LifeMax = 180;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.5f;
					Value = 700f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SHADOW_MUMMY:
					TypeName = "Shadow Mummy";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 60;
					Defense = 25;
					LifeMax = 190;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.5f;
					Value = 700f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.LIGHT_MUMMY:
					TypeName = "Light Mummy";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 55;
					Defense = 18;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.55f;
					Value = 700f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SPECTRAL_MUMMY:
					TypeName = "Spectral Mummy";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 65;
					Defense = 10;
					LifeMax = 270;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.55f;
					Value = 700f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.CORRUPT_SLIME:
					TypeName = "Corrupt Slime";
					Width = 40;
					Height = 30;
					AIStyle = 1;
					Damage = 55;
					Defense = 20;
					LifeMax = 170;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 55;
					Value = 400f;
					Scale = 1.1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SHADOW_SLIME:
					TypeName = "Shadow Slime";
					Width = 40;
					Height = 30;
					AIStyle = 1;
					Damage = 60;
					Defense = 25;
					LifeMax = 180;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 55;
					Value = 400f;
					Scale = 1.1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.WRAITH:
					HasNoGravity = true;
					HasNoTileCollide = true;
					TypeName = "Wraith";
					Width = 24;
					Height = 44;
					AIStyle = 22;
					Damage = 75;
					Defense = 18;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 6;
					Alpha = 100;
					Value = 500f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					KnockBackResist = 0.7f;
					break;
				case ID.CURSED_HAMMER:
					TypeName = "Cursed Hammer";
					Width = 40;
					Height = 40;
					AIStyle = 23;
					Damage = 80;
					Defense = 18;
					LifeMax = 200;
					SoundHit = 4;
					SoundKilled = 6;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					KnockBackResist = 0.4f;
					break;
				case ID.SHADOW_HAMMER:
					TypeName = "Shadow Hammer";
					Width = 40;
					Height = 40;
					AIStyle = 23;
					Damage = 95;
					Defense = 18;
					LifeMax = 180;
					SoundHit = 4;
					SoundKilled = 6;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					KnockBackResist = 0.4f;
					break;
				case ID.ENCHANTED_SWORD:
					TypeName = "Enchanted Sword";
					Width = 40;
					Height = 40;
					AIStyle = 23;
					Damage = 80;
					Defense = 18;
					LifeMax = 200;
					SoundHit = 4;
					SoundKilled = 6;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					KnockBackResist = 0.4f;
					break;
				case ID.MIMIC:
					TypeName = "Mimic";
					Width = 24;
					Height = 24;
					AIStyle = 25;
					Damage = 80;
					Defense = 30;
					LifeMax = 500;
					SoundHit = 4;
					SoundKilled = 6;
					Value = 100000f;
					KnockBackResist = 0.3f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.UNICORN:
					TypeName = "Unicorn";
					Width = 46;
					Height = 42;
					AIStyle = 26;
					Damage = 65;
					Defense = 30;
					LifeMax = 400;
					SoundHit = 10;
					SoundKilled = 1;
					KnockBackResist = 0.3f;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.WYVERN_HEAD:
					HasNoTileCollide = true;
					NpcSlots = 5f;
					TypeName = "Wyvern Head";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 80;
					Defense = 10;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_HEAD:
					HasNoTileCollide = true;
					NpcSlots = 5f;
					TypeName = "Arch Wyvern Head";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 100;
					Defense = 15;
					LifeMax = 4700;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.WYVERN_LEGS:
					HasNoTileCollide = true;
					TypeName = "Wyvern Legs";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_LEGS:
					HasNoTileCollide = true;
					TypeName = "Arch Wyvern Legs";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 50;
					Defense = 25;
					LifeMax = 4500;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.WYVERN_BODY1:
					HasNoTileCollide = true;
					TypeName = "Wyvern Body";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 2000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_BODY1:
					HasNoTileCollide = true;
					TypeName = "Arch Wyvern Body";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 45;
					Defense = 20;
					LifeMax = 4300;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 2000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.WYVERN_BODY2:
					HasNoTileCollide = true;
					TypeName = "Wyvern Body 2";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_BODY2:
					HasNoTileCollide = true;
					TypeName = "Arch Wyvern Body 2";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.WYVERN_BODY3:
					HasNoTileCollide = true;
					TypeName = "Wyvern Body 3";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_BODY3:
					HasNoTileCollide = true;
					TypeName = "Arch Wyvern Body 3";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 45;
					Defense = 20;
					LifeMax = 4300;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.WYVERN_TAIL:
					HasNoTileCollide = true;
					TypeName = "Wyvern Tail";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 20;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ARCH_WYVERN_TAIL:
					HasNoTileCollide = true;
					TypeName = "Arch Wyvern Tail";
					Width = 32;
					Height = 32;
					AIStyle = 6;
					NetAlways = true;
					Damage = 55;
					Defense = 15;
					LifeMax = 4000;
					SoundHit = 7;
					SoundKilled = 8;
					HasNoGravity = true;
					KnockBackResist = 0f;
					Value = 10000f;
					Scale = 1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.GIANT_BAT:
					NpcSlots = 0.5f;
					TypeName = "Giant Bat";
					Width = 26;
					Height = 20;
					AIStyle = 14;
					Damage = 70;
					Defense = 20;
					LifeMax = 160;
					SoundHit = 1;
					KnockBackResist = 0.75f;
					SoundKilled = 4;
					Value = 400f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.CORRUPTOR:
					NpcSlots = 1f;
					TypeName = "Corruptor";
					Width = 44;
					Height = 44;
					AIStyle = 5;
					Damage = 60;
					Defense = 32;
					LifeMax = 230;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					KnockBackResist = 0.55f;
					Value = 500f;
					break;
				case ID.DIGGER_HEAD:
					TypeName = "Digger Head";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 45;
					Defense = 10;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Scale = 0.9f;
					Value = 300f;
					break;
				case ID.DIGGER_BODY:
					TypeName = "Digger Body";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 28;
					Defense = 20;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Scale = 0.9f;
					Value = 300f;
					break;
				case ID.DIGGER_TAIL:
					TypeName = "Digger Tail";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 26;
					Defense = 30;
					LifeMax = 200;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Scale = 0.9f;
					Value = 300f;
					break;
				case ID.SEEKER_HEAD:
					NpcSlots = 3.5f;
					TypeName = "Seeker Head";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 70;
					Defense = 36;
					LifeMax = 500;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 700f;
					break;
				case ID.SEEKER_BODY:
					TypeName = "Seeker Body";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 55;
					Defense = 40;
					LifeMax = 500;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 700f;
					break;
				case ID.SEEKER_TAIL:
					TypeName = "Seeker Tail";
					Width = 22;
					Height = 22;
					AIStyle = 6;
					NetAlways = true;
					Damage = 40;
					Defense = 44;
					LifeMax = 500;
					SoundHit = 1;
					SoundKilled = 1;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 700f;
					break;
				case ID.CLINGER:
					HasNoGravity = true;
					HasNoTileCollide = true;
					IsBehindTiles = true;
					TypeName = "Clinger";
					Width = 30;
					Height = 30;
					AIStyle = 13;
					Damage = 70;
					Defense = 30;
					LifeMax = 320;
					SoundHit = 1;
					KnockBackResist = 0.2f;
					SoundKilled = 1;
					Value = 600f;
					break;
				case ID.ANGLER_FISH:
					NpcSlots = 0.5f;
					HasNoGravity = true;
					TypeName = "Angler Fish";
					Width = 18;
					Height = 20;
					AIStyle = 16;
					Damage = 80;
					Defense = 22;
					LifeMax = 90;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 500f;
					break;
				case ID.GREEN_JELLYFISH:
					HasNoGravity = true;
					TypeName = "Green Jellyfish";
					Width = 26;
					Height = 26;
					AIStyle = 18;
					Damage = 80;
					Defense = 30;
					LifeMax = 120;
					SoundHit = 1;
					SoundKilled = 1;
					Value = 800f;
					Alpha = 20;
					break;
				case ID.WEREWOLF:
					TypeName = "Werewolf";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 70;
					Defense = 40;
					LifeMax = 400;
					SoundHit = 6;
					SoundKilled = 1;
					KnockBackResist = 0.4f;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.BOUND_GOBLIN:
					IsFriendly = true;
					TypeName = "Bound Goblin";
					Width = 18;
					Height = 34;
					AIStyle = 0;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					Scale = 0.9f;
					break;
				case ID.BOUND_WIZARD:
					IsFriendly = true;
					TypeName = "Bound Wizard";
					Width = 18;
					Height = 40;
					AIStyle = 0;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.GOBLIN_TINKERER:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Goblin Tinkerer";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					Scale = 0.9f;
					break;
				case ID.WIZARD:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Wizard";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.CLOWN:
					TypeName = "Clown";
					Width = 34;
					Height = 78;
					AIStyle = 3;
					Damage = 50;
					Defense = 20;
					LifeMax = 400;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.4f;
					Value = 8000f;
					break;
				case ID.SKELETON_ARCHER:
					TypeName = "Skeleton Archer";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 55;
					Defense = 28;
					LifeMax = 260;
					SoundHit = 2;
					SoundKilled = 2;
					KnockBackResist = 0.55f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GOBLIN_ARCHER:
					TypeName = "Goblin Archer";
					Scale = 0.95f;
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 20;
					Defense = 6;
					LifeMax = 80;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.7f;
					Value = 200f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.VILE_SPIT:
					TypeName = "Vile Spit";
					Width = 16;
					Height = 16;
					AIStyle = 9;
					Damage = 65;
					Defense = 0;
					LifeMax = 1;
					SoundHit = 0;
					SoundKilled = 9;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					Scale = 0.9f;
					Alpha = 80;
					break;
				case ID.WALL_OF_FLESH:
					NpcSlots = 10f;
					TypeName = "Wall of Flesh";
					Width = 100;
					Height = 100;
					AIStyle = 27;
					Damage = 50;
					Defense = 12;
					LifeMax = 8000;
					SoundHit = 8;
					SoundKilled = 10;
					HasNoGravity = true;
					HasNoTileCollide = true;
					IsBehindTiles = true;
					KnockBackResist = 0f;
					Scale = 1.2f;
					IsBoss = true;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					Value = 80000f;
					break;
				case ID.WALL_OF_FLESH_EYE:
					TypeName = "Wall of Flesh Eye";
					Width = 100;
					Height = 100;
					AIStyle = 28;
					Damage = 50;
					Defense = 0;
					LifeMax = 8000;
					SoundHit = 8;
					SoundKilled = 10;
					HasNoGravity = true;
					HasNoTileCollide = true;
					IsBehindTiles = true;
					KnockBackResist = 0f;
					Scale = 1.2f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					Value = 80000f;
					break;
				case ID.THE_HUNGRY:
					TypeName = "The Hungry";
					Width = 30;
					Height = 30;
					AIStyle = 29;
					Damage = 30;
					Defense = 10;
					LifeMax = 240;
					SoundHit = 9;
					SoundKilled = 11;
					HasNoGravity = true;
					IsBehindTiles = true;
					HasNoTileCollide = true;
					KnockBackResist = 1.1f;
					break;
				case ID.THE_HUNGRY_II:
					TypeName = "The Hungry II";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 30;
					Defense = 6;
					LifeMax = 80;
					SoundHit = 9;
					KnockBackResist = 0.8f;
					SoundKilled = 12;
					break;
				case ID.LEECH_HEAD:
					TypeName = "Leech Head";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 26;
					Defense = 2;
					LifeMax = 60;
					SoundHit = 9;
					SoundKilled = 12;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					break;
				case ID.LEECH_BODY:
					TypeName = "Leech Body";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 22;
					Defense = 6;
					LifeMax = 60;
					SoundHit = 9;
					SoundKilled = 12;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					break;
				case ID.LEECH_TAIL:
					TypeName = "Leech Tail";
					Width = 14;
					Height = 14;
					AIStyle = 6;
					NetAlways = true;
					Damage = 18;
					Defense = 10;
					LifeMax = 60;
					SoundHit = 9;
					SoundKilled = 12;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					break;
				case ID.CHAOS_ELEMENTAL:
					TypeName = "Chaos Elemental";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 40;
					Defense = 30;
					LifeMax = 370;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.4f;
					Value = 600f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SPECTRAL_ELEMENTAL:
					TypeName = "Spectral Elemental";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 50;
					Defense = 35;
					LifeMax = 400;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.4f;
					Value = 600f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SLIMER:
					TypeName = "Slimer";
					Width = 40;
					Height = 30;
					AIStyle = 14;
					Damage = 45;
					Defense = 20;
					LifeMax = 60;
					SoundHit = 1;
					Alpha = 55;
					KnockBackResist = 0.8f;
					Scale = 1.1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GASTROPOD:
					HasNoGravity = true;
					TypeName = "Gastropod";
					Width = 20;
					Height = 20;
					AIStyle = 22;
					Damage = 60;
					Defense = 22;
					LifeMax = 220;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 600f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.SPECTRAL_GASTROPOD:
					HasNoGravity = true;
					TypeName = "Spectral Gastropod";
					Width = 20;
					Height = 20;
					AIStyle = 22;
					Damage = 60;
					Defense = 22;
					LifeMax = 220;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 600f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					break;
				case ID.BOUND_MECHANIC:
					IsFriendly = true;
					TypeName = "Bound Mechanic";
					Width = 18;
					Height = 34;
					AIStyle = 0;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					Scale = 0.9f;
					break;
				case ID.MECHANIC:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Mechanic";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.RETINAZER:
					TypeName = "Retinazer";
					Width = 100;
					Height = 110;
					AIStyle = 30;
					Damage = 50;
					Defense = 10;
					LifeMax = 24000;
					SoundHit = 1;
					SoundKilled = 14;
					KnockBackResist = 0f;
					HasNoGravity = true;
					HasNoTileCollide = true;
					TimeLeft = 22500;
					IsBoss = true;
					Value = 120000f;
					NpcSlots = 5f;
					break;
				case ID.SPAZMATISM:
					TypeName = "Spazmatism";
					Width = 100;
					Height = 110;
					AIStyle = 31;
					Damage = 50;
					Defense = 10;
					LifeMax = 24000;
					SoundHit = 1;
					SoundKilled = 14;
					KnockBackResist = 0f;
					HasNoGravity = true;
					HasNoTileCollide = true;
					TimeLeft = 22500;
					IsBoss = true;
					Value = 120000f;
					NpcSlots = 5f;
					break;
				case ID.SKELETRON_PRIME:
					TypeName = "Skeletron Prime";
					Width = 80;
					Height = 102;
					AIStyle = 32;
					Damage = 50;
					Defense = 25;
					LifeMax = 30000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					Value = 120000f;
					KnockBackResist = 0f;
					IsBoss = true;
					NpcSlots = 6f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.PRIME_CANNON:
					TypeName = "Prime Cannon";
					Width = 52;
					Height = 52;
					AIStyle = 35;
					Damage = 30;
					Defense = 25;
					LifeMax = 7000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					NetAlways = true;
					break;
				case ID.PRIME_SAW:
					TypeName = "Prime Saw";
					Width = 52;
					Height = 52;
					AIStyle = 33;
					Damage = 52;
					Defense = 40;
					LifeMax = 10000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					NetAlways = true;
					break;
				case ID.PRIME_VICE:
					TypeName = "Prime Vice";
					Width = 52;
					Height = 52;
					AIStyle = 34;
					Damage = 45;
					Defense = 35;
					LifeMax = 10000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					NetAlways = true;
					break;
				case ID.PRIME_LASER:
					TypeName = "Prime Laser";
					Width = 52;
					Height = 52;
					AIStyle = 36;
					Damage = 29;
					Defense = 20;
					LifeMax = 6000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					NetAlways = true;
					break;
				case ID.BALD_ZOMBIE:
					TypeName = "Bald Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 14;
					Defense = 6;
					LifeMax = 45;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.5f;
					Value = 60f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.WANDERING_EYE:
					TypeName = "Wandering Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 40;
					Defense = 20;
					LifeMax = 300;
					SoundHit = 1;
					KnockBackResist = 0.8f;
					SoundKilled = 1;
					Value = 500f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.THE_DESTROYER_HEAD:
					NpcSlots = 5f;
					TypeName = "The Destroyer";
					Width = 38;
					Height = 38;
					AIStyle = 37;
					Damage = 60;
					Defense = 0;
					LifeMax = 80000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Value = 120000f;
					Scale = 1.25f;
					IsBoss = true;
					NetAlways = true;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.THE_DESTROYER_BODY:
					NpcSlots = 5f;
					TypeName = "The Destroyer Body";
					Width = 38;
					Height = 38;
					AIStyle = 37;
					Damage = 40;
					Defense = 30;
					LifeMax = 80000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					NetAlways = true;
					Scale = 1.25f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.THE_DESTROYER_TAIL:
					NpcSlots = 5f;
					TypeName = "The Destroyer Tail";
					Width = 38;
					Height = 38;
					AIStyle = 37;
					Damage = 20;
					Defense = 35;
					LifeMax = 80000;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					HasNoTileCollide = true;
					KnockBackResist = 0f;
					IsBehindTiles = true;
					Scale = 1.25f;
					NetAlways = true;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.ILLUMINANT_BAT:
					TypeName = "Illuminant Bat";
					Width = 26;
					Height = 20;
					AIStyle = 14;
					Damage = 75;
					Defense = 30;
					LifeMax = 200;
					SoundHit = 1;
					KnockBackResist = 0.75f;
					SoundKilled = 6;
					Value = 500f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.ILLUMINANT_SLIME:
					TypeName = "Illuminant Slime";
					Width = 24;
					Height = 18;
					AIStyle = 1;
					Damage = 70;
					Defense = 30;
					LifeMax = 180;
					SoundHit = 1;
					SoundKilled = 6;
					Alpha = 100;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					KnockBackResist = 0.85f;
					Scale = 1.05f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.PROBE:
					NpcSlots = 1f;
					TypeName = "Probe";
					Width = 30;
					Height = 30;
					AIStyle = 5;
					Damage = 50;
					Defense = 20;
					LifeMax = 200;
					SoundHit = 4;
					SoundKilled = 14;
					HasNoGravity = true;
					KnockBackResist = 0.8f;
					HasNoTileCollide = true;
					break;
				case ID.POSSESSED_ARMOR:
					TypeName = "Possessed Armor";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 55;
					Defense = 28;
					LifeMax = 260;
					SoundHit = 4;
					SoundKilled = 6;
					KnockBackResist = 0.4f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					break;
				case ID.TOXIC_SLUDGE:
					TypeName = "Toxic Sludge";
					Width = 34;
					Height = 28;
					AIStyle = 1;
					Damage = 50;
					Defense = 18;
					LifeMax = 150;
					SoundHit = 1;
					SoundKilled = 1;
					Alpha = 55;
					Value = 400f;
					Scale = 1.1f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					KnockBackResist = 0.8f;
					break;
				case ID.SANTA_CLAUS:
					IsTownNPC = true;
					IsFriendly = true;
					TypeName = "Santa Claus";
					Width = 18;
					Height = 40;
					AIStyle = 7;
					Damage = 10;
					Defense = 15;
					LifeMax = 250;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.5f;
					break;
				case ID.SNOWMAN_GANGSTA:
					TypeName = "Snowman Gangsta";
					Width = 26;
					Height = 40;
					AIStyle = 38;
					Damage = 50;
					Defense = 20;
					LifeMax = 200;
					SoundHit = 11;
					SoundKilled = 15;
					KnockBackResist = 0.6f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.MISTER_STABBY:
					TypeName = "Mister Stabby";
					Width = 26;
					Height = 40;
					AIStyle = 38;
					Damage = 65;
					Defense = 26;
					LifeMax = 240;
					SoundHit = 11;
					SoundKilled = 15;
					KnockBackResist = 0.6f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
				case ID.SNOW_BALLA:
					TypeName = "Snow Balla";
					Width = 26;
					Height = 40;
					AIStyle = 38;
					Damage = 55;
					Defense = 22;
					LifeMax = 220;
					SoundHit = 11;
					SoundKilled = 15;
					KnockBackResist = 0.6f;
					Value = 400f;
					BuffImmune[(int)Buff.ID.POISONED] = true;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					BuffImmune[(int)Buff.ID.ON_FIRE] = true;
					BuffImmune[(int)Buff.ID.ON_FIRE_2] = true;
					break;
#if VERSION_101
				case ID.PINCUSHION_ZOMBIE:
					TypeName = "Pincushion Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 16;
					Defense = 8;
					LifeMax = 50;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.45f;
					Value = 65f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SLIMED_ZOMBIE:
					TypeName = "Slimed Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 13;
					Defense = 6;
					LifeMax = 40;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.55f;
					Value = 55f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SWAMP_ZOMBIE:
					TypeName = "Swamp Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 13;
					Defense = 8;
					LifeMax = 45;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.45f;
					Value = 80f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.TWIGGY_ZOMBIE:
					TypeName = "Twiggy Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 16;
					Defense = 4;
					LifeMax = 45;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.55f;
					Value = 70f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.CATARACT_EYE:
					TypeName = "Cataract Eye"; // Why the IDs are out of order I got no idea; PC does it as well.
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 18;
					Defense = 4;
					LifeMax = 65;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.7f;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.SLEEPY_EYE:
					TypeName = "Sleepy Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 16;
					Defense = 2;
					LifeMax = 60;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.85f;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.DIALATED_EYE:
					TypeName = "Dialated Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 18;
					Defense = 2;
					LifeMax = 50;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.8f;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.GREEN_EYE:
					TypeName = "Green Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 20;
					Defense = 0;
					LifeMax = 60;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.8f;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.PURPLE_EYE:
					TypeName = "Purple Eye";
					Width = 30;
					Height = 32;
					AIStyle = 2;
					Damage = 14;
					Defense = 4;
					LifeMax = 60;
					SoundHit = 1;
					SoundKilled = 1;
					KnockBackResist = 0.8f;
					Value = 75f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.FEMALE_ZOMBIE:
					TypeName = "Female Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 12;
					Defense = 4;
					LifeMax = 38;
					SoundHit = 1;
					SoundKilled = 2;
					KnockBackResist = 0.6f;
					Value = 65f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.ZOMBIE_MUSHROOM:
					TypeName = "Spore Zombie"; // It wasn't until 1.3 that these 2 became known as Spore Zombies...
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 40;
					Defense = 10;
					LifeMax = 180;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.4f;
					Value = 1000f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
				case ID.ZOMBIE_MUSHROOM_HAT:
					TypeName = "Spore Zombie";
					Width = 18;
					Height = 40;
					AIStyle = 3;
					Damage = 38;
					Defense = 16;
					LifeMax = 220;
					SoundHit = 1;
					SoundKilled = 6;
					KnockBackResist = 0.3f;
					Value = 1200f;
					BuffImmune[(int)Buff.ID.CONFUSED] = false;
					break;
#endif
			}
			FrameY = 0;
			FrameHeight = (short)(SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.NINJA + Type].Height / NpcFrameCount[Type]);
			if (ScaleOverride > 0)
			{
				int ScaledWidth = (int)(Width * Scale);
				int ScaledHeight = (int)(Height * Scale);
				Position.X += ScaledWidth >> 1;
				Position.Y += ScaledHeight;
				Scale = (float)ScaleOverride;
				Width = (ushort)(Width * Scale);
				Height = (ushort)(Height * Scale);
				if (Height == 16 || Height == 32)
				{
					Height++;
				}
				Position.X -= Width >> 1;
				Position.Y -= Height;
			}
			else
			{
				Width = (ushort)(Width * Scale);
				Height = (ushort)(Height * Scale);
			}
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
			XYWH.Width = Width;
			XYWH.Height = Height;
			HealthBarLife = (Life = LifeMax);
			DefDamage = Damage;
			DefDefense = (short)Defense;
			DisplayName = Lang.NpcName(NetID);
		}

		private void BoundAI()
		{
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				for (int i = 0; i < Player.MaxNumPlayers; i++)
				{
					if (Main.PlayerSet[i].Active != 0 && Main.PlayerSet[i].TalkNPC == WhoAmI)
					{
						if (Type == (int)ID.BOUND_GOBLIN)
						{
							Transform((int)ID.GOBLIN_TINKERER);
							return;
						}
						if (Type == (int)ID.BOUND_WIZARD)
						{
							Transform((int)ID.WIZARD);
							return;
						}
						if (Type == (int)ID.BOUND_MECHANIC)
						{
							Transform((int)ID.MECHANIC);
							return;
						}
					}
				}
			}
			Velocity.X *= 0.93f;
			if ((double)Velocity.X > -0.1f && (double)Velocity.X < 0.1f)
			{
				Velocity.X = 0f;
			}
			TargetClosest();
			SpriteDirection = Direction;
		}

		private unsafe void SlimeAI()
		{
			bool flag = !Main.GameTime.DayTime || Life != LifeMax || XYWH.Y > Main.WorldSurface << 4;
			if (Type == (int)ID.CORRUPT_SLIME)
			{
				flag = true;
				if (Main.Rand.Next(32) == 0)
				{
					Dust* ptr = Main.DustSet.NewDust(14, ref XYWH, 0.0, 0.0, Alpha, Colour);
					if (ptr != null)
					{
						ptr->Velocity.X *= 0.3f;
						ptr->Velocity.Y *= 0.3f;
					}
				}
			}
			else if (Type == (int)ID.LAVA_SLIME)
			{
				Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(1f, 0.3f, 0.1f));
				Dust* ptr2 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 1.7000000476837158);
				if (ptr2 != null)
				{
					ptr2->NoGravity = true;
				}
			}
			if (AI2 > 1f)
			{
				AI2 -= 1f;
			}
			if (IsWet)
			{
				if (HasYCollision)
				{
					Velocity.Y = -2f;
				}
				if (Velocity.Y < 0f)
				{
					if (AI3 == Position.X)
					{
						Direction = (sbyte)(-Direction);
						AI2 = 200f;
					}
				}
				else if (Velocity.Y > 0f)
				{
					AI3 = Position.X;
				}
				if (Type == (int)ID.LAVA_SLIME)
				{
					if (Velocity.Y > 2f)
					{
						Velocity.Y *= 0.9f;
					}
					else if (DirectionY < 0)
					{
						Velocity.Y -= 0.8f;
					}
					Velocity.Y -= 0.5f;
					if (Velocity.Y < -10f)
					{
						Velocity.Y = -10f;
					}
				}
				else
				{
					if (Velocity.Y > 2f)
					{
						Velocity.Y *= 0.9f;
					}
					Velocity.Y -= 0.5f;
					if (Velocity.Y < -4f)
					{
						Velocity.Y = -4f;
					}
				}
				if (AI2 == 1f && flag)
				{
					TargetClosest();
				}
			}
			AIAction = 0;
			if (AI2 == 0f)
			{
				AI0 = -100f;
				AI2 = 1f;
				TargetClosest();
			}
			if (Velocity.Y == 0f)
			{
				if (AI3 == Position.X)
				{
					Direction = (sbyte)(-Direction);
					AI2 = 200f;
				}
				AI3 = 0f;
				Velocity.X *= 0.8f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				if (flag)
				{
					AI0 += 1f;
				}
				AI0 += 1f;
				if (Type == (int)ID.LAVA_SLIME)
				{
					AI0 += 2f;
				}
				else if (Type == (int)ID.DUNGEON_SLIME)
				{
					AI0 += 3f;
				}
				else if (Type == (int)ID.ILLUMINANT_SLIME)
				{
					AI0 += 2f;
				}
				else if (Type == (int)ID.CORRUPT_SLIME)
				{
					if (Scale >= 0f)
					{
						AI0 += 4f;
					}
					else
					{
						AI0 += 1f;
					}
				}
				if (AI0 >= 0f)
				{
					ShouldNetUpdate = true;
					if (flag && AI2 == 1f)
					{
						TargetClosest();
					}
					if (AI1 == 2f)
					{
						if (Type == (int)ID.LAVA_SLIME)
						{
							Velocity.X += 3.5f * Direction;
							Velocity.Y = -10f;
						}
						else
						{
							Velocity.X += 3 * Direction;
							Velocity.Y = -8f;
						}
						AI0 = -200f;
						AI1 = 0f;
						AI3 = Position.X;
					}
					else
					{
						Velocity.Y = -6f;
						Velocity.X += 2 * Direction;
						if (Type == (int)ID.LAVA_SLIME)
						{
							Velocity.X += 2 * Direction;
						}
						AI0 = -120f;
						AI1 += 1f;
					}
					if (Type == (int)ID.TOXIC_SLUDGE)
					{
						Velocity.Y *= 1.3f;
						Velocity.X *= 1.2f;
					}
				}
				else if (AI0 >= -30f)
				{
					AIAction = 1;
				}
			}
			else if (Target < Player.MaxNumPlayers && ((Direction == 1 && Velocity.X < 3f) || (Direction == -1 && Velocity.X > -3f)))
			{
				if ((Direction == -1 && Velocity.X < 0.1) || (Direction == 1 && Velocity.X > -0.1))
				{
					Velocity.X += 0.2f * Direction;
				}
				else
				{
					Velocity.X *= 0.93f;
				}
			}
		}

		private unsafe void FloatingEyeballAI()
		{
			HasNoGravity = true;
			if (!HasNoTileCollide)
			{
				if (HasXCollision)
				{
					Velocity.X = OldVelocity.X * -0.5f;
					if (Direction == -1 && Velocity.X > 0f && Velocity.X < 2f)
					{
						Velocity.X = 2f;
					}
					else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -2f)
					{
						Velocity.X = -2f;
					}
				}
				if (HasYCollision)
				{
					Velocity.Y = OldVelocity.Y * -0.5f;
					if (Velocity.Y > 0f && Velocity.Y < 1f)
					{
						Velocity.Y = 1f;
					}
					else if (Velocity.Y < 0f && Velocity.Y > -1f)
					{
						Velocity.Y = -1f;
					}
				}
			}
#if VERSION_INITIAL
			if ((Type == (int)ID.DEMON_EYE || Type == (int)ID.WANDERING_EYE) && Main.GameTime.DayTime && XYWH.Y <= Main.WorldSurfacePixels)
#else
			if ((Type == (int)ID.DEMON_EYE || Type == (int)ID.WANDERING_EYE || Type == (int)ID.CATARACT_EYE || Type == (int)ID.SLEEPY_EYE || Type == (int)ID.DIALATED_EYE || Type == (int)ID.GREEN_EYE || Type == (int)ID.PURPLE_EYE) && Main.GameTime.DayTime && XYWH.Y <= Main.WorldSurfacePixels)
#endif
			{
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				DirectionY = (sbyte)((Velocity.Y > 0f) ? 1 : (-1));
				Direction = (sbyte)((Velocity.X > 0f) ? 1 : (-1));
			}
			else
			{
				TargetClosest();
			}
			if (Type == (int)ID.THE_HUNGRY_II)
			{
				TargetClosest();
				Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(0.3f, 0.2f, 0.1f));
				if (Direction == -1 && Velocity.X > -6f)
				{
					Velocity.X -= 0.1f;
					if (Velocity.X > 6f)
					{
						Velocity.X -= 0.1f;
					}
					else if (Velocity.X > 0f)
					{
						Velocity.X -= 0.2f;
					}
					if (Velocity.X < -6f)
					{
						Velocity.X = -6f;
					}
				}
				else if (Direction == 1 && Velocity.X < 6f)
				{
					Velocity.X += 0.1f;
					if (Velocity.X < -6f)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X < 0f)
					{
						Velocity.X += 0.2f;
					}
					if (Velocity.X > 6f)
					{
						Velocity.X = 6f;
					}
				}
				if (DirectionY == -1 && Velocity.Y > -2.5)
				{
					Velocity.Y -= 0.04f;
					if (Velocity.Y > 2.5)
					{
						Velocity.Y -= 0.05f;
					}
					else if (Velocity.Y > 0f)
					{
						Velocity.Y -= 0.15f;
					}
					if (Velocity.Y < -2.5)
					{
						Velocity.Y = -2.5f;
					}
				}
				else if (DirectionY == 1 && Velocity.Y < 1.5)
				{
					Velocity.Y += 0.04f;
					if (Velocity.Y < -2.5)
					{
						Velocity.Y += 0.05f;
					}
					else if (Velocity.Y < 0f)
					{
						Velocity.Y += 0.15f;
					}
					if (Velocity.Y > 2.5)
					{
						Velocity.Y = 2.5f;
					}
				}
				if (Main.Rand.Next(40) == 0)
				{
					Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
					if (ptr != null)
					{
						ptr->Velocity.X *= 0.5f;
						ptr->Velocity.Y *= 0.1f;
					}
				}
			}
			else if (Type == (int)ID.WANDERING_EYE)
			{
				if (Life < LifeMax >> 1)
				{
					if (Direction == -1 && Velocity.X > -6f)
					{
						Velocity.X -= 0.1f;
						if (Velocity.X > 6f)
						{
							Velocity.X -= 0.1f;
						}
						else if (Velocity.X > 0f)
						{
							Velocity.X += 0.05f;
						}
						if (Velocity.X < -6f)
						{
							Velocity.X = -6f;
						}
					}
					else if (Direction == 1 && Velocity.X < 6f)
					{
						Velocity.X += 0.1f;
						if (Velocity.X < -6f)
						{
							Velocity.X += 0.1f;
						}
						else if (Velocity.X < 0f)
						{
							Velocity.X -= 0.05f;
						}
						if (Velocity.X > 6f)
						{
							Velocity.X = 6f;
						}
					}
					if (DirectionY == -1 && Velocity.Y > -4f)
					{
						Velocity.Y -= 0.1f;
						if (Velocity.Y > 4f)
						{
							Velocity.Y -= 0.1f;
						}
						else if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.05f;
						}
						if (Velocity.Y < -4f)
						{
							Velocity.Y = -4f;
						}
					}
					else if (DirectionY == 1 && Velocity.Y < 4f)
					{
						Velocity.Y += 0.1f;
						if (Velocity.Y < -4f)
						{
							Velocity.Y += 0.1f;
						}
						else if (Velocity.Y < 0f)
						{
							Velocity.Y -= 0.05f;
						}
						if (Velocity.Y > 4f)
						{
							Velocity.Y = 4f;
						}
					}
				}
				else
				{
					if (Direction == -1 && Velocity.X > -4f)
					{
						Velocity.X -= 0.1f;
						if (Velocity.X > 4f)
						{
							Velocity.X -= 0.1f;
						}
						else if (Velocity.X > 0f)
						{
							Velocity.X += 0.05f;
						}
						if (Velocity.X < -4f)
						{
							Velocity.X = -4f;
						}
					}
					else if (Direction == 1 && Velocity.X < 4f)
					{
						Velocity.X += 0.1f;
						if (Velocity.X < -4f)
						{
							Velocity.X += 0.1f;
						}
						else if (Velocity.X < 0f)
						{
							Velocity.X -= 0.05f;
						}
						else if (Velocity.X > 4f)
						{
							Velocity.X = 4f;
						}
					}
					if (DirectionY == -1 && Velocity.Y > -1.5)
					{
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 1.5)
						{
							Velocity.Y -= 0.05f;
						}
						else if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.03f;
						}
						else if (Velocity.Y < -1.5)
						{
							Velocity.Y = -1.5f;
						}
					}
					else if (DirectionY == 1 && Velocity.Y < 1.5)
					{
						Velocity.Y += 0.04f;
						if (Velocity.Y < -1.5)
						{
							Velocity.Y += 0.05f;
						}
						else if (Velocity.Y < 0f)
						{
							Velocity.Y -= 0.03f;
						}
						else if (Velocity.Y > 1.5)
						{
							Velocity.Y = 1.5f;
						}
					}
				}
			}
			else
			{
				float TopSpeedX = 4f;
				float TopSpeedY = 1.5f;
				TopSpeedX *= 1f + (1f - Scale);
				TopSpeedY *= 1f + (1f - Scale); // This is different pre-1.1/1.2, but no functional differences exist.
				if (Direction == -1 && Velocity.X > 0f - TopSpeedX)
				{
					Velocity.X -= 0.1f;
					if (Velocity.X > TopSpeedX)
					{
						Velocity.X -= 0.1f;
					}
					else if (Velocity.X > 0f)
					{
						Velocity.X += 0.05f;
					}
					if (Velocity.X < 0f - TopSpeedX)
					{
						Velocity.X = 0f - TopSpeedX;
					}
				}
				else if (Direction == 1 && Velocity.X < TopSpeedX)
				{
					Velocity.X += 0.1f;
					if (Velocity.X < 0f - TopSpeedX)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X < 0f)
					{
						Velocity.X -= 0.05f;
					}
					if (Velocity.X > TopSpeedX)
					{
						Velocity.X = TopSpeedX;
					}
				}
				if (DirectionY == -1 && Velocity.Y > 0f - TopSpeedY)
				{
					Velocity.Y -= 0.04f;
					if (Velocity.Y > TopSpeedY)
					{
						Velocity.Y -= 0.05f;
					}
					else if (Velocity.Y > 0f)
					{
						Velocity.Y += 0.03f;
					}
					if (Velocity.Y < 0f - TopSpeedY)
					{
						Velocity.Y = 0f - TopSpeedY;
					}
				}
				else if (DirectionY == 1 && Velocity.Y < TopSpeedY)
				{
					Velocity.Y += 0.04f;
					if (Velocity.Y < 0f - TopSpeedY)
					{
						Velocity.Y += 0.05f;
					}
					else if (Velocity.Y < 0f)
					{
						Velocity.Y -= 0.03f;
					}
					if (Velocity.Y > TopSpeedY)
					{
						Velocity.Y = TopSpeedY;
					}
				}
			}
#if VERSION_INITIAL
			if ((Type == (int)ID.DEMON_EYE || Type == (int)ID.WANDERING_EYE) && Main.Rand.Next(40) == 0)
#else
			if ((Type == (int)ID.DEMON_EYE || Type == (int)ID.WANDERING_EYE || Type == (int)ID.CATARACT_EYE || Type == (int)ID.SLEEPY_EYE || Type == (int)ID.DIALATED_EYE || Type == (int)ID.GREEN_EYE || Type == (int)ID.PURPLE_EYE) && Main.Rand.Next(40) == 0)
#endif
			{
				Dust* ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr2 != null)
				{
					ptr2->Velocity.X *= 0.5f;
					ptr2->Velocity.Y *= 0.1f;
				}
			}
			if (IsWet)
			{
				if (Velocity.Y > 0f)
				{
					Velocity.Y *= 0.95f;
				}
				Velocity.Y -= 0.5f;
				if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
				TargetClosest();
			}
		}

		private unsafe void WalkAI()
		{
			int num = 60;
			if (Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL)
			{
				num = 20;
				if (AI3 == -120f)
				{
					Velocity.X = 0f;
					Velocity.Y = 0f;
					AI3 = 0f;
					Main.PlaySound(2, XYWH.X, XYWH.Y, 8);
					Vector2 vector = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
					float num2 = OldPos[2].X + (Width >> 1) - vector.X;
					float num3 = OldPos[2].Y + (Height >> 1) - vector.Y;
					float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
					num4 = 2f / num4;
					num2 *= num4;
					num3 *= num4;
					for (int i = 0; i < 16; i++)
					{
						Dust* ptr = Main.DustSet.NewDust(71, ref XYWH, num2, num3, 200, default(Color), 2.0);
						if (ptr == null)
						{
							break;
						}
						ptr->NoGravity = true;
						ptr->Velocity.X *= 2f;
					}
					for (int j = 0; j < 16; j++)
					{
						Dust* ptr2 = Main.DustSet.NewDust((int)OldPos[2].X, (int)OldPos[2].Y, Width, Height, 71, 0f - num2, 0f - num3, 200, default(Color), 2.0);
						if (ptr2 == null)
						{
							break;
						}
						ptr2->NoGravity = true;
						ptr2->Velocity.X *= 2f;
					}
				}
			}
			bool flag = false;
			bool flag2 = true;
			if (Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.CRAB || Type == (int)ID.CLOWN || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.GOBLIN_ARCHER || Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL)
			{
				flag2 = false;
			}
			if ((Type != (int)ID.SKELETON_ARCHER && Type != (int)ID.GOBLIN_ARCHER) || !(AI2 > 0f))
			{
				if (Velocity.Y == 0f && ((Velocity.X > 0f && Direction < 0) || (Velocity.X < 0f && Direction > 0)))
				{
					flag = true;
				}
				if (Position.X == OldPosition.X || AI3 >= num || flag)
				{
					AI3 += 1f;
				}
				else if ((double)Math.Abs(Velocity.X) > 0.9 && AI3 > 0f)
				{
					AI3 -= 1f;
				}
				if (AI3 > num * 10)
				{
					AI3 = 0f;
				}
				if (WasJustHit)
				{
					AI3 = 0f;
				}
				if (AI3 == num)
				{
					ShouldNetUpdate = true;
				}
			}
#if VERSION_INITIAL
			if (AI3 < (float)num && (!Main.GameTime.DayTime || XYWH.Y > Main.WorldSurfacePixels || Type == (int)ID.GOBLIN_PEON || Type == (int)ID.GOBLIN_THIEF || Type == (int)ID.GOBLIN_WARRIOR || Type == (int)ID.BONES || Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.CRAB || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.GOBLIN_ARCHER || Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.SPECTRAL_MUMMY))
#else
			if (AI3 < num && (!Main.GameTime.DayTime || XYWH.Y > Main.WorldSurfacePixels || Type == (int)ID.GOBLIN_PEON || Type == (int)ID.GOBLIN_THIEF || Type == (int)ID.GOBLIN_WARRIOR || Type == (int)ID.BONES || Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.CRAB || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.GOBLIN_ARCHER || Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.SPECTRAL_MUMMY || Type == (int)ID.ZOMBIE_MUSHROOM || Type == (int)ID.ZOMBIE_MUSHROOM_HAT))
#endif
			{
#if VERSION_INITIAL
				if (Type == (int)ID.ZOMBIE || Type == (int)ID.SKELETON || Type == (int)ID.BONES || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.BALD_ZOMBIE)
#else
				if (Type == (int)ID.ZOMBIE || Type == (int)ID.SKELETON || Type == (int)ID.BONES || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.BALD_ZOMBIE || Type == (int)ID.PINCUSHION_ZOMBIE || Type == (int)ID.SLIMED_ZOMBIE || Type == (int)ID.SWAMP_ZOMBIE || Type == (int)ID.TWIGGY_ZOMBIE || Type == (int)ID.FEMALE_ZOMBIE)
#endif
				{
					if (Main.Rand.Next(1000) == 0)
					{
						Main.PlaySound(14, XYWH.X, XYWH.Y);
					}
				}
				else if ((Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_MUMMY) && Main.Rand.Next(500) == 0)
				{
					Main.PlaySound(26, XYWH.X, XYWH.Y);
				}
				TargetClosest();
			}
			else if ((Type != (int)ID.SKELETON_ARCHER && Type != (int)ID.GOBLIN_ARCHER) || !(AI2 > 0f))
			{
				if (Main.GameTime.DayTime && XYWH.Y >> 4 < Main.WorldSurface && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Velocity.X == 0f)
				{
					if (Velocity.Y == 0f)
					{
						AI0 += 1f;
						if (AI0 >= 2f)
						{
							Direction = (sbyte)(-Direction);
							SpriteDirection = Direction;
							AI0 = 0f;
						}
					}
				}
				else
				{
					AI0 = 0f;
				}
				if (Direction == 0)
				{
					Direction = 1;
				}
			}
			if (Type == (int)ID.CHAOS_ELEMENTAL) // BUG: There is no equivalent check for the spectral elemental, and this results in a slower move speed compared to the chaos elemental (120); Can be fixed by adding a spectral elemental in the check.
			{
				if (Velocity.X < -3f || Velocity.X > 3f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 3f && Direction == 1)
				{
					if (Velocity.Y == 0f && Velocity.X < 0f)
					{
						Velocity.X *= 0.99f;
					}
					Velocity.X += 0.07f;
					if (Velocity.X > 3f)
					{
						Velocity.X = 3f;
					}
				}
				else if (Velocity.X > -3f && Direction == -1)
				{
					if (Velocity.Y == 0f && Velocity.X > 0f)
					{
						Velocity.X *= 0.99f;
					}
					Velocity.X -= 0.07f;
					if (Velocity.X < -3f)
					{
						Velocity.X = -3f;
					}
				}
			}
			else if (Type == (int)ID.GOBLIN_THIEF || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.WEREWOLF)
			{
				if (Velocity.X < -2f || Velocity.X > 2f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 2f && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > 2f)
					{
						Velocity.X = 2f;
					}
				}
				else if (Velocity.X > -2f && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X < -2f)
					{
						Velocity.X = -2f;
					}
				}
			}
			else if (Type == (int)ID.CLOWN)
			{
				if (Velocity.X < -2f || Velocity.X > 2f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 2f && Direction == 1)
				{
					Velocity.X += 0.04f;
					if (Velocity.X > 2f)
					{
						Velocity.X = 2f;
					}
				}
				else if (Velocity.X > -2f && Direction == -1)
				{
					Velocity.X -= 0.04f;
					if (Velocity.X < -2f)
					{
						Velocity.X = -2f;
					}
				}
			}
#if VERSION_INITIAL
			else if (Type == (int)ID.SKELETON || Type == (int)ID.GOBLIN_PEON || Type == (int)ID.BONES || Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.POSSESSED_ARMOR)
#else
			else if (Type == (int)ID.SKELETON || Type == (int)ID.GOBLIN_PEON || Type == (int)ID.BONES || Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.POSSESSED_ARMOR || Type == (int)ID.ZOMBIE_MUSHROOM)
#endif
			{
				if (Velocity.X < -1.5f || Velocity.X > 1.5f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 1.5f && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > 1.5f)
					{
						Velocity.X = 1.5f;
					}
				}
				else if (Velocity.X > -1.5f && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X < -1.5f)
					{
						Velocity.X = -1.5f;
					}
				}
			}
			else if (Type == (int)ID.CRAB)
			{
				if (Velocity.X < -0.5f || Velocity.X > 0.5f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.7f;
					}
				}
				else if (Velocity.X < 0.5f && Direction == 1)
				{
					Velocity.X += 0.03f;
					if (Velocity.X > 0.5f)
					{
						Velocity.X = 0.5f;
					}
				}
				else if (Velocity.X > -0.5f && Direction == -1)
				{
					Velocity.X -= 0.03f;
					if (Velocity.X < -0.5f)
					{
						Velocity.X = -0.5f;
					}
				}
			}
			else if (Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_MUMMY)
			{
				float num5 = 1f;
				float num6 = 0.05f;
				if (Life < LifeMax >> 1)
				{
					num5 = 2f;
					num6 = 0.1f;
				}
				if (Type == (int)ID.DARK_MUMMY || Type == (int)ID.SHADOW_MUMMY)
				{
					num5 *= 1.5f;
				}
				if (Velocity.X < 0f - num5 || Velocity.X > num5)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.7f;
					}
				}
				else if (Velocity.X < num5 && Direction == 1)
				{
					Velocity.X += num6;
					if (Velocity.X > num5)
					{
						Velocity.X = num5;
					}
				}
				else if (Velocity.X > 0f - num5 && Direction == -1)
				{
					Velocity.X -= num6;
					if (Velocity.X < 0f - num5)
					{
						Velocity.X = 0f - num5;
					}
				}
			}
			else if (Type != (int)ID.SKELETON_ARCHER && Type != (int)ID.GOBLIN_ARCHER)
			{
#if VERSION_INITIAL
				if (Velocity.X < -1f || Velocity.X > 1f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 1f && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > 1f)
					{
						Velocity.X = 1f;
					}
				}
				else if (Velocity.X > -1f && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X < -1f)
					{
						Velocity.X = -1f;
					}
				}
#else
				float Modifier = 1f;
				if (Type == (int)ID.PINCUSHION_ZOMBIE)
				{
					Modifier = 1.1f;
				}
				if (Type == (int)ID.SLIMED_ZOMBIE)
				{
					Modifier = 0.9f;
				}
				if (Type == (int)ID.SWAMP_ZOMBIE)
				{
					Modifier = 1.2f;
				}
				if (Type == (int)ID.TWIGGY_ZOMBIE)
				{
					Modifier = 0.8f;
				}
				if (Type == (int)ID.BALD_ZOMBIE)
				{
					Modifier = 0.95f;
				}
				if (Type == (int)ID.FEMALE_ZOMBIE)
				{
					Modifier = 0.87f;
				}
				if (Type == (int)ID.ZOMBIE || Type == (int)ID.BALD_ZOMBIE || Type == (int)ID.PINCUSHION_ZOMBIE || Type == (int)ID.SLIMED_ZOMBIE || Type == (int)ID.SWAMP_ZOMBIE || Type == (int)ID.TWIGGY_ZOMBIE || Type == (int)ID.FEMALE_ZOMBIE)
				{
					Modifier *= 1f + (1f - Scale);
				}
				if (Velocity.X < 0f - Modifier || Velocity.X > Modifier)
				{
					if (Velocity.Y == 0f)
					{
						Velocity *= 0.8f;
					}
				}
				else if (Velocity.X < Modifier && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > Modifier)
					{
						Velocity.X = Modifier;
					}
				}
				else if (Velocity.X > 0f - Modifier && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X < 0f - Modifier)
					{
						Velocity.X = 0f - Modifier;
					}
				}
#endif
			}
			if (Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.GOBLIN_ARCHER)
			{
				if (IsConfused)
				{
					AI2 = 0f;
				}
				else
				{
					if (AI1 > 0f)
					{
						AI1 -= 1f;
					}
					if (WasJustHit)
					{
						AI1 = 30f;
						AI2 = 0f;
					}
					int num7 = ((Type == (int)ID.GOBLIN_ARCHER) ? 180 : 70);
					if (AI2 > 0f)
					{
						TargetClosest();
						if (AI1 == num7 >> 1)
						{
							float num8 = 11f;
							int num9 = 35;
							int num10 = 82;
							if (Type == (int)ID.GOBLIN_ARCHER)
							{
								num8 = 9f;
								num9 = 11;
								num10 = 81;
							}
							Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
							float num11 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
							float num12 = Math.Abs(num11) * 0.1f;
							float num13 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y - num12;
							num11 += Main.Rand.Next(-40, 41);
							num13 += Main.Rand.Next(-40, 41);
							float num14 = (float)Math.Sqrt(num11 * num11 + num13 * num13);
							ShouldNetUpdate = true;
							num14 = num8 / num14;
							num11 *= num14;
							num13 *= num14;
							vector2.X += num11;
							vector2.Y += num13;
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								Projectile.NewProjectile(vector2.X, vector2.Y, num11, num13, num10, num9, 0f);
							}
							if (Math.Abs(num13) > Math.Abs(num11) * 2f)
							{
								if (num13 > 0f)
								{
									AI2 = 1f;
								}
								else
								{
									AI2 = 5f;
								}
							}
							else if (Math.Abs(num11) > Math.Abs(num13) * 2f)
							{
								AI2 = 3f;
							}
							else if (num13 > 0f)
							{
								AI2 = 2f;
							}
							else
							{
								AI2 = 4f;
							}
						}
						if (Velocity.Y != 0f || AI1 <= 0f)
						{
							AI1 = 0f;
							AI2 = 0f;
						}
						else
						{
							Velocity.X *= 0.9f;
							SpriteDirection = Direction;
						}
					}
					if (AI2 <= 0f && Velocity.Y == 0f && AI1 <= 0f && !Main.PlayerSet[Target].IsDead && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num15 = Main.PlayerSet[Target].Position.X + 10f - vector3.X;
						float num16 = Math.Abs(num15) * 0.1f;
						float num17 = Main.PlayerSet[Target].Position.Y + 21f - vector3.Y - num16;
						num15 += Main.Rand.Next(-40, 41);
						num17 += Main.Rand.Next(-40, 41);
						float num18 = num15 * num15 + num17 * num17;
						if (num18 < 490000f)
						{
							ShouldNetUpdate = true;
							Velocity.X *= 0.5f;
							num18 = 10f / (float)Math.Sqrt(num18);
							num15 *= num18;
							num17 *= num18;
							AI2 = 3f;
							AI1 = num7;
							if (Math.Abs(num17) > Math.Abs(num15) * 2f)
							{
								if (num17 > 0f)
								{
									AI2 = 1f;
								}
								else
								{
									AI2 = 5f;
								}
							}
							else if (Math.Abs(num15) > Math.Abs(num17) * 2f)
							{
								AI2 = 3f;
							}
							else if (num17 > 0f)
							{
								AI2 = 2f;
							}
							else
							{
								AI2 = 4f;
							}
						}
					}
					if (AI2 <= 0f)
					{
						if (Velocity.X < -1f || Velocity.X > 1f)
						{
							if (Velocity.Y == 0f)
							{
								Velocity.X *= 0.8f;
							}
						}
						else if (Velocity.X < 1f && Direction == 1)
						{
							Velocity.X += 0.07f;
							if (Velocity.X > 1f)
							{
								Velocity.X = 1f;
							}
						}
						else if (Velocity.X > -1f && Direction == -1)
						{
							Velocity.X -= 0.07f;
							if (Velocity.X < -1f)
							{
								Velocity.X = -1f;
							}
						}
					}
				}
			}
			else if (Type == (int)ID.CLOWN && Main.NetMode != (byte)NetModeSetting.CLIENT && !Main.PlayerSet[Target].IsDead)
			{
				if (WasJustHit)
				{
					AI2 = 0f;
				}
				AI2 += 1f;
				if (AI2 > 450f)
				{
					Vector2 vector4 = new Vector2(Position.X + Width * 0.5f - Direction * 24, Position.Y + 4f);
					int num19 = 3 * Direction;
					int num20 = -5;
					int num21 = Projectile.NewProjectile(vector4.X, vector4.Y, num19, num20, 75, 0, 0f);
					if (num21 >= 0)
					{
						Main.ProjectileSet[num21].timeLeft = 300;
					}
					AI2 = 0f;
				}
			}
			bool flag3 = false;
			if (Velocity.Y == 0f)
			{
				int num22 = XYWH.Y + Height + 8 >> 4;
				int num23 = XYWH.X >> 4;
				int num24 = XYWH.X + Width >> 4;
				for (int k = num23; k <= num24; k++)
				{
					if (Main.TileSet[k, num22].IsActive != 0 && Main.TileSolid[Main.TileSet[k, num22].Type])
					{
						flag3 = true;
						break;
					}
				}
			}
			if (flag3)
			{
				int num25 = XYWH.Y + Height - 15 >> 4;
				int num26 = ((Type != (int)ID.CLOWN) ? (XYWH.X + (Width >> 1) + 15 * Direction) : (XYWH.X + (Width >> 1) + ((Width >> 1) + 16) * Direction));
				num26 >>= 4;
				if (flag2 && Main.TileSet[num26, num25 - 1].Type == 10 && Main.TileSet[num26, num25 - 1].IsActive != 0)
				{
					AI2 += 1f;
					AI3 = 0f;
					if (AI2 >= 60f)
					{
#if VERSION_INITIAL
						if (!Main.GameTime.IsBloodMoon && (Type == (int)ID.ZOMBIE || Type == (int)ID.BALD_ZOMBIE))
#else
						if (!Main.GameTime.IsBloodMoon && (Type == (int)ID.ZOMBIE || Type == (int)ID.BALD_ZOMBIE || Type == (int)ID.PINCUSHION_ZOMBIE || Type == (int)ID.SLIMED_ZOMBIE || Type == (int)ID.SWAMP_ZOMBIE || Type == (int)ID.TWIGGY_ZOMBIE || Type == (int)ID.FEMALE_ZOMBIE))
#endif
						{
							AI1 = 0f;
						}
						Velocity.X = -0.5f * Direction;
#if VERSION_INITIAL
						AI1 += 1f;
#else
						AI1 += 5f;
#endif
						if (Type == (int)ID.GOBLIN_THIEF)
						{
							AI1 += 1f;
						}
						else if (Type == (int)ID.BONES)
						{
							AI1 += 6f;
						}
						AI2 = 0f;
						bool flag4 = false;
						if (AI1 >= 10f)
						{
							flag4 = true;
							AI1 = 10f;
						}
						WorldGen.KillTile(num26, num25 - 1, KillToFail: true);
						if ((Main.NetMode != (byte)NetModeSetting.CLIENT || !flag4) && flag4 && Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							if (Type == (int)ID.GOBLIN_PEON)
							{
								WorldGen.KillTile(num26, num25 - 1);
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num26, num25 - 1, 0);
								NetMessage.SendMessage();
							}
							else
							{
								int num27 = WorldGen.OpenDoor(num26, num25, Direction);
								if (num27 != 0)
								{
									NetMessage.CreateMessage3(19, num26, num25, num27);
									NetMessage.SendMessage();
								}
								else
								{
									AI3 = num;
									ShouldNetUpdate = true;
								}
							}
						}
					}
				}
				else
				{
					if ((Velocity.X < 0f && SpriteDirection == -1) || (Velocity.X > 0f && SpriteDirection == 1))
					{
						if (Main.TileSet[num26, num25 - 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num26, num25 - 2].Type])
						{
							if (Main.TileSet[num26, num25 - 3].IsActive != 0 && Main.TileSolid[Main.TileSet[num26, num25 - 3].Type])
							{
								Velocity.Y = -8f;
							}
							else
							{
								Velocity.Y = -7f;
							}
							ShouldNetUpdate = true;
						}
						else if (Main.TileSet[num26, num25 - 1].IsActive != 0 && Main.TileSolid[Main.TileSet[num26, num25 - 1].Type])
						{
							Velocity.Y = -6f;
							ShouldNetUpdate = true;
						}
						else if (Main.TileSet[num26, num25].IsActive != 0 && Main.TileSolid[Main.TileSet[num26, num25].Type])
						{
							Velocity.Y = -5f;
							ShouldNetUpdate = true;
						}
						else if (DirectionY < 0 && Type != (int)ID.CRAB && (Main.TileSet[num26, num25 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num26, num25 + 1].Type]) && (Main.TileSet[num26 + Direction, num25 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num26 + Direction, num25 + 1].Type]))
						{
							Velocity.Y = -8f;
							Velocity.X *= 1.5f;
							ShouldNetUpdate = true;
						}
						else if (flag2)
						{
							AI1 = 0f;
							AI2 = 0f;
						}
					}
					if (Type == (int)ID.BONES || Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.WEREWOLF)
					{
						if (Velocity.Y == 0f && Math.Abs(Position.X + (Width >> 1) - (Main.PlayerSet[Target].Position.X + 10f)) < 100f && Math.Abs(Position.Y + (Height >> 1) - (Main.PlayerSet[Target].Position.Y + 21f)) < 50f && ((Direction > 0 && Velocity.X >= 1f) || (Direction < 0 && Velocity.X <= -1f)))
						{
							Velocity.X *= 2f;
							if (Velocity.X > 3f)
							{
								Velocity.X = 3f;
							}
							if (Velocity.X < -3f)
							{
								Velocity.X = -3f;
							}
							Velocity.Y = -4f;
							ShouldNetUpdate = true;
						}
					}
					else if ((Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL) && Velocity.Y < 0f)
					{
						Velocity.Y *= 1.1f;
					}
				}
			}
			else if (flag2)
			{
				AI1 = 0f;
				AI2 = 0f;
			}
			if ((Type != (int)ID.CHAOS_ELEMENTAL && Type != (int)ID.SPECTRAL_ELEMENTAL) || Main.NetMode == (byte)NetModeSetting.CLIENT || !(AI3 >= num))
			{
				return;
			}
			int num28 = Main.PlayerSet[Target].XYWH.X >> 4;
			int num29 = Main.PlayerSet[Target].XYWH.Y >> 4;
			int num30 = XYWH.X >> 4;
			int num31 = XYWH.Y >> 4;
			if (Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) + Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 2000)
			{
				return;
			}
			int num32 = 0;
			do
			{
				int num33 = Main.Rand.Next(num28 - 20, num28 + 20);
				int num34 = Main.Rand.Next(num29 - 20, num29 + 20);
				for (int l = num34; l < num29 + 20; l++)
				{
					if ((l < num29 - 4 || l > num29 + 4 || num33 < num28 - 4 || num33 > num28 + 4) && (l < num31 - 1 || l > num31 + 1 || num33 < num30 - 1 || num33 > num30 + 1) && Main.TileSet[num33, l].IsActive != 0 && (Type != (int)ID.DARK_CASTER || Main.TileSet[num33, l - 1].WallType != 0) && Main.TileSet[num33, l - 1].Lava == 0 && Main.TileSolid[Main.TileSet[num33, l].Type] && !Collision.SolidTiles(num33 - 1, num33 + 1, l - 4, l - 1))
					{
						Position.X = (XYWH.X = num33 * 16 - (Width >> 1));
						Position.Y = (XYWH.Y = l * 16 - Height);
						ShouldNetUpdate = true;
						AI3 = -120f;
						num32 = 32;
						break;
					}
				}
			}
			while (++num32 < 32);
		}

		private unsafe void EyeOfCthulhuAI()
		{
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead || Main.PlayerSet[Target].Active == 0)
			{
				TargetClosest();
			}
			bool dead = Main.PlayerSet[Target].IsDead;
			float num = Position.X + (Width >> 1) - Main.PlayerSet[Target].Position.X - 10f;
			float num2 = Position.Y + Height - 59f - Main.PlayerSet[Target].Position.Y - 21f;
			float num3 = (float)Math.Atan2(num2, num) + 1.57f;
			if (num3 < 0f)
			{
				num3 += 6.283f;
			}
			else if (num3 > 6.283f)
			{
				num3 -= 6.283f;
			}
			float num4 = 0f;
			if (AI0 == 0f && AI1 == 0f)
			{
				num4 = 0.02f;
			}
			if (AI0 == 0f && AI1 == 2f && AI2 > 40f)
			{
				num4 = 0.05f;
			}
			if (AI0 == 3f && AI1 == 0f)
			{
				num4 = 0.05f;
			}
			if (AI0 == 3f && AI1 == 2f && AI2 > 40f)
			{
				num4 = 0.08f;
			}
			if (Rotation < num3)
			{
				if ((double)(num3 - Rotation) > 3.1415)
				{
					Rotation -= num4;
				}
				else
				{
					Rotation += num4;
				}
			}
			else if (Rotation > num3)
			{
				if ((double)(Rotation - num3) > 3.1415)
				{
					Rotation += num4;
				}
				else
				{
					Rotation -= num4;
				}
			}
			if (Rotation > num3 - num4 && Rotation < num3 + num4)
			{
				Rotation = num3;
			}
			if (Rotation < 0f)
			{
				Rotation += 6.283f;
			}
			else if (Rotation > 6.283f)
			{
				Rotation -= 6.283f;
			}
			if (Rotation > num3 - num4 && Rotation < num3 + num4)
			{
				Rotation = num3;
			}
			if (Main.Rand.Next(6) == 0)
			{
				Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr != null)
				{
					ptr->Velocity.X *= 0.5f;
					ptr->Velocity.Y *= 0.1f;
				}
			}
			if (Main.GameTime.DayTime || dead)
			{
				Velocity.Y -= 0.04f;
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				return;
			}
			if (AI0 == 0f)
			{
				if (AI1 == 0f)
				{
					float num5 = 5f;
					float num6 = 0.04f;
					Vector2 vector = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
					float num7 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
					float num8 = Main.PlayerSet[Target].Position.Y + 21f - 200f - vector.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					float num10 = num9;
					num9 = num5 / num9;
					num7 *= num9;
					num8 *= num9;
					if (Velocity.X < num7)
					{
						Velocity.X += num6;
						if (Velocity.X < 0f && num7 > 0f)
						{
							Velocity.X += num6;
						}
					}
					else if (Velocity.X > num7)
					{
						Velocity.X -= num6;
						if (Velocity.X > 0f && num7 < 0f)
						{
							Velocity.X -= num6;
						}
					}
					if (Velocity.Y < num8)
					{
						Velocity.Y += num6;
						if (Velocity.Y < 0f && num8 > 0f)
						{
							Velocity.Y += num6;
						}
					}
					else if (Velocity.Y > num8)
					{
						Velocity.Y -= num6;
						if (Velocity.Y > 0f && num8 < 0f)
						{
							Velocity.Y -= num6;
						}
					}
					AI2 += 1f;
					if (AI2 >= 600f)
					{
						AI1 = 1f;
						AI2 = 0f;
						AI3 = 0f;
						Target = Player.MaxNumPlayers;
						ShouldNetUpdate = true;
					}
					else if (XYWH.Y + Height < Main.PlayerSet[Target].XYWH.Y && num10 < 500f)
					{
						if (!Main.PlayerSet[Target].IsDead)
						{
							AI3 += 1f;
						}
						if (AI3 >= 110f)
						{
							AI3 = 0f;
							Rotation = num3;
							float num11 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
							float num12 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
							float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
							num13 = 5f / num13;
							Vector2 vector2 = vector;
							Vector2 vector3 = default(Vector2);
							vector3.X = num11 * num13;
							vector3.Y = num12 * num13;
							vector2.X += vector3.X * 10f;
							vector2.Y += vector3.Y * 10f;
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								int num14 = NewNPC((int)vector2.X, (int)vector2.Y, (int)ID.SERVANT_OF_CTHULHU);
								if (num14 < MaxNumNPCs)
								{
									Main.NPCSet[num14].Velocity.X = vector3.X;
									Main.NPCSet[num14].Velocity.Y = vector3.Y;
									NetMessage.CreateMessage1(23, num14);
									NetMessage.SendMessage();
								}
							}
							Main.PlaySound(3, (int)vector2.X, (int)vector2.Y);
							for (int i = 0; i < 8; i++)
							{
								if (null == Main.DustSet.NewDust((int)vector2.X, (int)vector2.Y, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f))
								{
									break;
								}
							}
						}
					}
				}
				else if (AI1 == 1f)
				{
					Rotation = num3;
					float num15 = 6f;
					Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num16 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
					float num17 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
					float num18 = (float)Math.Sqrt(num16 * num16 + num17 * num17);
					num18 = num15 / num18;
					Velocity.X = num16 * num18;
					Velocity.Y = num17 * num18;
					AI1 = 2f;
				}
				else if (AI1 == 2f)
				{
					AI2 += 1f;
					if (AI2 >= 40f)
					{
						Velocity.X *= 0.98f;
						Velocity.Y *= 0.98f;
						if (Velocity.X > -0.1 && Velocity.X < 0.1)
						{
							Velocity.X = 0f;
						}
						if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
						{
							Velocity.Y = 0f;
						}
					}
					else
					{
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
					}
					if (AI2 >= 150f)
					{
						AI3 += 1f;
						AI2 = 0f;
						Target = Player.MaxNumPlayers;
						Rotation = num3;
						if (AI3 >= 3f)
						{
							AI1 = 0f;
							AI3 = 0f;
						}
						else
						{
							AI1 = 1f;
						}
					}
				}
				if (Life < LifeMax >> 1)
				{
					AI0 = 1f;
					AI1 = 0f;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				return;
			}
			if (AI0 == 1f || AI0 == 2f)
			{
				if (AI0 == 1f)
				{
					AI2 += 0.005f;
					if (AI2 > 0.5)
					{
						AI2 = 0.5f;
					}
				}
				else
				{
					AI2 -= 0.005f;
					if (AI2 < 0f)
					{
						AI2 = 0f;
					}
				}
				Rotation += AI2;
				AI1 += 1f;
				if (AI1 == 100f)
				{
					AI0 += 1f;
					AI1 = 0f;
					if (AI0 == 3f)
					{
						AI2 = 0f;
					}
					else
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y);
						Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
						for (int j = 0; j < 2; j++)
						{
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 8);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int k = 0; k < 16; k++)
						{
							if (null == Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2, Main.Rand.Next(-30, 31) * 0.2))
							{
								break;
							}
						}
					}
				}
				Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2, Main.Rand.Next(-30, 31) * 0.2);
				Velocity.X *= 0.98f;
				Velocity.Y *= 0.98f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
				{
					Velocity.Y = 0f;
				}
				return;
			}
			Damage = 23;
			Defense = 0;
			if (AI1 == 0f)
			{
				float num19 = 6f;
				float num20 = 0.07f;
				Vector2 vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num21 = Main.PlayerSet[Target].Position.X + 10f - vector5.X;
				float num22 = Main.PlayerSet[Target].Position.Y + 21f - 120f - vector5.Y;
				float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
				num23 = num19 / num23;
				num21 *= num23;
				num22 *= num23;
				if (Velocity.X < num21)
				{
					Velocity.X += num20;
					if (Velocity.X < 0f && num21 > 0f)
					{
						Velocity.X += num20;
					}
				}
				else if (Velocity.X > num21)
				{
					Velocity.X -= num20;
					if (Velocity.X > 0f && num21 < 0f)
					{
						Velocity.X -= num20;
					}
				}
				if (Velocity.Y < num22)
				{
					Velocity.Y += num20;
					if (Velocity.Y < 0f && num22 > 0f)
					{
						Velocity.Y += num20;
					}
				}
				else if (Velocity.Y > num22)
				{
					Velocity.Y -= num20;
					if (Velocity.Y > 0f && num22 < 0f)
					{
						Velocity.Y -= num20;
					}
				}
				AI2 += 1f;
				if (AI2 >= 200f)
				{
					AI1 = 1f;
					AI2 = 0f;
					AI3 = 0f;
					Target = Player.MaxNumPlayers;
					ShouldNetUpdate = true;
				}
			}
			else if (AI1 == 1f)
			{
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				Rotation = num3;
				float num24 = 6.8f;
				Vector2 vector6 = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
				float num25 = Main.PlayerSet[Target].Position.X + 10f - vector6.X;
				float num26 = Main.PlayerSet[Target].Position.Y + 21f - vector6.Y;
				float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
				num27 = num24 / num27;
				Velocity.X = num25 * num27;
				Velocity.Y = num26 * num27;
				AI1 = 2f;
			}
			else
			{
				if (AI1 != 2f)
				{
					return;
				}
				AI2 += 1f;
				if (AI2 >= 40f)
				{
					Velocity.X *= 0.97f;
					Velocity.Y *= 0.97f;
					if (Velocity.X > -0.1 && Velocity.X < 0.1)
					{
						Velocity.X = 0f;
					}
					if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
					{
						Velocity.Y = 0f;
					}
				}
				else
				{
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
				}
				if (AI2 >= 130f)
				{
					AI3 += 1f;
					AI2 = 0f;
					Target = Player.MaxNumPlayers;
					Rotation = num3;
					if (AI3 >= 3f)
					{
						AI1 = 0f;
						AI3 = 0f;
					}
					else
					{
						AI1 = 1f;
					}
				}
			}
		}

		private unsafe void AggressiveFlyerAI()
		{
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead)
			{
				TargetClosest();
			}
			float num;
			float num2;
			switch (Type)
			{
			case (int)ID.SERVANT_OF_CTHULHU:
				num = 5f;
				num2 = 0.03f;
				break;
			case (int)ID.SERVANT_OF_OCRAM:
				Lighting.AddLight(XYWH.X >> 4, XYWH.Y >> 4, new Vector3(1f, 1f, 1f));
				num = 9f;
				num2 = 0.1f;
				break;
			case (int)ID.EATER_OF_SOULS:
				num = 4f;
				num2 = 0.02f;
				break;
			case (int)ID.HORNET:
			case (int)ID.DRAGON_HORNET:
				num = 3.5f;
				num2 = 0.021f;
				break;
			case (int)ID.METEOR_HEAD:
				num = 1f;
				num2 = 0.03f;
				break;
			case (int)ID.CORRUPTOR:
				num = 4.2f;
				num2 = 0.022f;
				break;
			default:
				num = 6f;
				num2 = 0.05f;
				break;
			}
			int num3 = (XYWH.X + (Width >> 1)) & -8;
			int num4 = (XYWH.Y + (Height >> 1)) & -8;
			float num5 = ((Main.PlayerSet[Target].XYWH.X + 10) & -8) - num3;
			float num6 = ((Main.PlayerSet[Target].XYWH.Y + 21) & -8) - num4;
			float num7 = num5 * num5 + num6 * num6;
			float num8 = num7;
			bool flag = false;
			if (num7 == 0f)
			{
				num5 = Velocity.X;
				num6 = Velocity.Y;
			}
			else
			{
				if (num7 > 360000f)
				{
					flag = true;
				}
				num7 = num / (float)Math.Sqrt(num7);
				num5 *= num7;
				num6 *= num7;
			}
			if (Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET || Type == (int)ID.CORRUPTOR || Type == (int)ID.PROBE)
			{
				if (Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET || Type == (int)ID.CORRUPTOR || num8 > 10000f)
				{
					AI0 += 1f;
					if (AI0 > 0f)
					{
						Velocity.Y += 0.023f;
					}
					else
					{
						Velocity.Y -= 0.023f;
					}
					if (AI0 < -100f || AI0 > 100f)
					{
						Velocity.X += 0.023f;
					}
					else
					{
						Velocity.X -= 0.023f;
					}
					if (AI0 > 200f)
					{
						AI0 = -200f;
					}
				}
				if ((Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.CORRUPTOR) && num8 < 22500f)
				{
					Velocity.X += num5 * 0.007f;
					Velocity.Y += num6 * 0.007f;
				}
			}
			if (Main.PlayerSet[Target].IsDead)
			{
				num5 = Direction * num * 0.5f;
				num6 = num * -0.5f;
			}
			if (Velocity.X < num5)
			{
				Velocity.X += num2;
				if (Velocity.X < 0f && num5 > 0f && Type != (int)ID.EATER_OF_SOULS && Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.CORRUPTOR && Type != (int)ID.PROBE)
				{
					Velocity.X += num2;
				}
			}
			else if (Velocity.X > num5)
			{
				Velocity.X -= num2;
				if (Velocity.X > 0f && num5 < 0f && Type != (int)ID.EATER_OF_SOULS && Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.CORRUPTOR && Type != (int)ID.PROBE)
				{
					Velocity.X -= num2;
				}
			}
			if (Velocity.Y < num6)
			{
				Velocity.Y += num2;
				if (Velocity.Y < 0f && num6 > 0f && Type != (int)ID.EATER_OF_SOULS && Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.CORRUPTOR && Type != (int)ID.PROBE)
				{
					Velocity.Y += num2;
				}
			}
			else if (Velocity.Y > num6)
			{
				Velocity.Y -= num2;
				if (Velocity.Y > 0f && num6 < 0f && Type != (int)ID.EATER_OF_SOULS && Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.CORRUPTOR && Type != (int)ID.PROBE)
				{
					Velocity.Y -= num2;
				}
			}
			if (Type == (int)ID.METEOR_HEAD)
			{
				if (num5 > 0f)
				{
					SpriteDirection = 1;
					Rotation = (float)Math.Atan2(num6, num5);
				}
				else if (num5 < 0f)
				{
					SpriteDirection = -1;
					Rotation = (float)Math.Atan2(num6, num5) + 3.14f;
				}
			}
			else if (Type == (int)ID.PROBE)
			{
				if (WasJustHit)
				{
					LocalAI0 = 0;
				}
				else if (++LocalAI0 >= 120 && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI0 = 0;
					if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						int num9 = 25;
						int num10 = 84;
						Projectile.NewProjectile(num3, num4, num5, num6, num10, num9, 0f);
					}
				}
				int num11 = XYWH.X + (Width >> 1);
				int num12 = XYWH.Y + (Height >> 1);
				num11 >>= 4;
				num12 >>= 4;
				if (!WorldGen.SolidTile(num11, num12))
				{
					Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(0.3f, 0.1f, 0.05f));
				}
				if (num5 > 0f)
				{
					SpriteDirection = 1;
					Rotation = (float)Math.Atan2(num6, num5);
				}
				if (num5 < 0f)
				{
					SpriteDirection = -1;
					Rotation = (float)Math.Atan2(num6, num5) + 3.14f;
				}
			}
			else if (Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.CORRUPTOR)
			{
				Rotation = (float)Math.Atan2(num6, num5) - 1.57f;
			}
			else if (Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET)
			{
				if (num5 > 0f)
				{
					SpriteDirection = 1;
				}
				else if (num5 < 0f)
				{
					SpriteDirection = -1;
				}
				Rotation = Velocity.X * 0.1f;
			}
			else
			{
				Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
			}
			if (Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.METEOR_HEAD || Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET || Type == (int)ID.CORRUPTOR || Type == (int)ID.PROBE)
			{
				float num13 = 0.7f;
				if (Type == (int)ID.EATER_OF_SOULS)
				{
					num13 = 0.4f;
				}
				if (HasXCollision)
				{
					ShouldNetUpdate = true;
					Velocity.X = OldVelocity.X * (0f - num13);
					if (Direction == -1 && Velocity.X > 0f && Velocity.X < 2f)
					{
						Velocity.X = 2f;
					}
					else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -2f)
					{
						Velocity.X = -2f;
					}
				}
				if (HasYCollision)
				{
					ShouldNetUpdate = true;
					Velocity.Y = OldVelocity.Y * (0f - num13);
					if (Velocity.Y > 0f && Velocity.Y < 1.5)
					{
						Velocity.Y = 2f;
					}
					else if (Velocity.Y < 0f && Velocity.Y > -1.5)
					{
						Velocity.Y = -2f;
					}
				}
				if (Type == (int)ID.METEOR_HEAD)
				{
					Dust* ptr = Main.DustSet.NewDust((int)(Position.X - Velocity.X), (int)(Position.Y - Velocity.Y), Width, Height, 6, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
					if (ptr != null)
					{
						ptr->NoGravity = true;
						ptr->Velocity.X *= 0.3f;
						ptr->Velocity.Y *= 0.3f;
					}
				}
				else if (Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.PROBE && Main.Rand.Next(24) == 0)
				{
					Dust* ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 18, Velocity.X, 2.0, 75, Colour, Scale);
					if (ptr2 != null)
					{
						ptr2->Velocity.X *= 0.5f;
						ptr2->Velocity.Y *= 0.1f;
					}
				}
			}
			else if (Main.Rand.Next(48) == 0)
			{
				Dust* ptr3 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr3 != null)
				{
					ptr3->Velocity.X *= 0.5f;
					ptr3->Velocity.Y *= 0.1f;
				}
			}
			if (Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.CORRUPTOR)
			{
				if (IsWet)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.95f;
					}
					Velocity.Y -= 0.3f;
					if (Velocity.Y < -2f)
					{
						Velocity.Y = -2f;
					}
				}
			}
			else if (Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET)
			{
				if (IsWet)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.95f;
					}
					Velocity.Y -= 0.5f;
					if (Velocity.Y < -4f)
					{
						Velocity.Y = -4f;
					}
					TargetClosest();
				}
				if (AI1 == 101f)
				{
					Main.PlaySound(2, XYWH.X, XYWH.Y, 17);
					AI1 = 0f;
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					AI1 += Main.Rand.Next(5, 20) * 0.1f * Scale;
					if (AI1 >= 130f)
					{
						if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
						{
							Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + (Height >> 1));
							float num14 = Main.PlayerSet[Target].Position.X + 10f - vector.X + Main.Rand.Next(-20, 21);
							float num15 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y + Main.Rand.Next(-20, 21);
							if ((num14 < 0f && Velocity.X < 0f) || (num14 > 0f && Velocity.X > 0f))
							{
								float num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
								num16 = 8f / num16;
								num14 *= num16;
								num15 *= num16;
								int num17 = (int)(13f * Scale);
								int num18 = 55;
								int num19 = Projectile.NewProjectile(vector.X, vector.Y, num14, num15, num18, num17, 0f);
								if (num19 >= 0)
								{
									Main.ProjectileSet[num19].timeLeft = 300;
								}
								AI1 = 101f;
								ShouldNetUpdate = true;
							}
							else
							{
								AI1 = 0f;
							}
						}
						else
						{
							AI1 = 0f;
						}
					}
				}
			}
			else if (Type == (int)ID.PROBE && flag)
			{
				if ((Velocity.X > 0f && num5 > 0f) || (Velocity.X < 0f && num5 < 0f))
				{
					if (Math.Abs(Velocity.X) < 12f)
					{
						Velocity.X *= 1.05f;
					}
				}
				else
				{
					Velocity.X *= 0.9f;
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && Type == (int)ID.CORRUPTOR && !Main.PlayerSet[Target].IsDead)
			{
				if (WasJustHit)
				{
					LocalAI0 = 0;
				}
				LocalAI0++;
				if (LocalAI0 == 180)
				{
					if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						NewNPC((int)(Position.X + Velocity.X) + (Width >> 1), (int)(Position.Y + Velocity.Y) + (Height >> 1), (int)ID.VILE_SPIT);
					}
					LocalAI0 = 0;
				}
			}
			if ((Main.GameTime.DayTime && Type != (int)ID.EATER_OF_SOULS && Type != (int)ID.METEOR_HEAD && Type != (int)ID.HORNET && Type != (int)ID.DRAGON_HORNET && Type != (int)ID.CORRUPTOR) || Main.PlayerSet[Target].IsDead)
			{
				Velocity.Y -= num2 * 2f;
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
			}
			if (((Velocity.X > 0f && OldVelocity.X < 0f) || (Velocity.X < 0f && OldVelocity.X > 0f) || (Velocity.Y > 0f && OldVelocity.Y < 0f) || (Velocity.Y < 0f && OldVelocity.Y > 0f)) && !WasJustHit)
			{
				ShouldNetUpdate = true;
			}
		}

		private unsafe void WormAI()
		{
			if (Type == (int)ID.LEECH_HEAD && LocalAI1 == 0)
			{
				LocalAI1 = 1;
				Main.PlaySound(4, XYWH.X, XYWH.Y, 13);
				int num = 1;
				if (Velocity.X < 0f)
				{
					num = -1;
				}
				for (int i = 0; i < 16; i++)
				{
					if (null == Main.DustSet.NewDust(XYWH.X - 20, XYWH.Y - 20, Width + 40, Height + 40, 5, num * 8, -1.0))
					{
						break;
					}
				}
			}
			if (Type >= (int)ID.EATER_OF_WORLDS_HEAD && Type <= (int)ID.EATER_OF_WORLDS_TAIL)
			{
				RealLife = -1;
			}
			else if (AI3 > 0f)
			{
				RealLife = (int)AI3;
			}
#if VERSION_INITIAL
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead)
#else
			if (Target < 0 || Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead)
#endif
			{
				TargetClosest();
			}
			if (Main.PlayerSet[Target].IsDead && TimeLeft > 300)
			{
				TimeLeft = 300;
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				if (Type == (int)ID.WYVERN_HEAD || Type == (int)ID.ARCH_WYVERN_HEAD)
				{
					if (AI0 == 0f)
					{
						AI3 = WhoAmI;
						RealLife = WhoAmI;
						int num2 = 0;
						int num3 = WhoAmI;
						int num4 = Type - (int)ID.WYVERN_HEAD;
						for (int j = 0; j < 14; j++)
						{
							int num5 = (int)ID.WYVERN_BODY1;
							switch (j)
							{
							case 1:
							case 8:
								num5 = (int)ID.WYVERN_LEGS;
								break;
							case 11:
								num5 = (int)ID.WYVERN_BODY2;
								break;
							case 12:
								num5 = (int)ID.WYVERN_BODY3;
								break;
							case 13:
								num5 = (int)ID.WYVERN_TAIL;
								break;
							}
							num2 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, num5 + num4, WhoAmI);
							Main.NPCSet[num2].AI3 = WhoAmI;
							Main.NPCSet[num2].RealLife = WhoAmI;
							Main.NPCSet[num2].AI1 = num3;
							Main.NPCSet[num3].AI0 = num2;
							NetMessage.CreateMessage1(23, num2);
							NetMessage.SendMessage();
							num3 = num2;
						}
					}
				}
				else if ((Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.DEVOURER_BODY || Type == (int)ID.GIANT_WORM_HEAD || Type == (int)ID.GIANT_WORM_BODY || Type == (int)ID.EATER_OF_WORLDS_HEAD || Type == (int)ID.EATER_OF_WORLDS_BODY || Type == (int)ID.BONE_SERPENT_HEAD || Type == (int)ID.BONE_SERPENT_BODY || Type == (int)ID.DIGGER_HEAD || Type == (int)ID.DIGGER_BODY || Type == (int)ID.SEEKER_HEAD || Type == (int)ID.SEEKER_BODY || Type == (int)ID.LEECH_HEAD || Type == (int)ID.LEECH_BODY) && AI0 == 0f)
				{
					if (Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.GIANT_WORM_HEAD || Type == (int)ID.EATER_OF_WORLDS_HEAD || Type == (int)ID.BONE_SERPENT_HEAD || Type == (int)ID.DIGGER_HEAD || Type == (int)ID.SEEKER_HEAD || Type == (int)ID.LEECH_HEAD)
					{
						if (Type < (int)ID.EATER_OF_WORLDS_HEAD || Type > (int)ID.EATER_OF_WORLDS_TAIL)
						{
							AI3 = WhoAmI;
							RealLife = WhoAmI;
						}
						AI2 = Main.Rand.Next(8, 13);
						if (Type == (int)ID.GIANT_WORM_HEAD)
						{
							AI2 = Main.Rand.Next(4, 7);
						}
						else if (Type == (int)ID.EATER_OF_WORLDS_HEAD)
						{
							AI2 = Main.Rand.Next(45, 56);
						}
						else if (Type == (int)ID.BONE_SERPENT_HEAD)
						{
							AI2 = Main.Rand.Next(12, 19);
						}
						else if (Type == (int)ID.DIGGER_HEAD)
						{
							AI2 = Main.Rand.Next(6, 12);
						}
						else if (Type == (int)ID.SEEKER_HEAD)
						{
							AI2 = Main.Rand.Next(20, 26);
						}
						else if (Type == (int)ID.LEECH_HEAD)
						{
							AI2 = Main.Rand.Next(3, 6);
						}
						AI0 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, Type + 1, WhoAmI);
					}
					else if ((Type == (int)ID.DEVOURER_BODY || Type == (int)ID.GIANT_WORM_BODY || Type == (int)ID.EATER_OF_WORLDS_BODY || Type == (int)ID.BONE_SERPENT_BODY || Type == (int)ID.DIGGER_BODY || Type == (int)ID.SEEKER_BODY || Type == (int)ID.LEECH_BODY) && AI2 > 0f)
					{
						AI0 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, Type, WhoAmI);
					}
					else
					{
						AI0 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, Type + 1, WhoAmI);
					}
					if (Type < (int)ID.EATER_OF_WORLDS_HEAD || Type > (int)ID.EATER_OF_WORLDS_TAIL)
					{
						Main.NPCSet[(int)AI0].AI3 = AI3;
						Main.NPCSet[(int)AI0].RealLife = RealLife;
					}
					Main.NPCSet[(int)AI0].AI1 = WhoAmI;
					Main.NPCSet[(int)AI0].AI2 = AI2 - 1f;
					ShouldNetUpdate = true;
				}
				if ((Type == (int)ID.DEVOURER_BODY || Type == (int)ID.DEVOURER_TAIL || Type == (int)ID.GIANT_WORM_BODY || Type == (int)ID.GIANT_WORM_TAIL || Type == (int)ID.BONE_SERPENT_BODY || Type == (int)ID.BONE_SERPENT_TAIL || Type == (int)ID.DIGGER_BODY || Type == (int)ID.DIGGER_TAIL || Type == (int)ID.SEEKER_BODY || Type == (int)ID.SEEKER_TAIL || (Type > (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type > (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL) || Type == (int)ID.LEECH_BODY || Type == (int)ID.LEECH_TAIL) && (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != AIStyle))
				{
					Life = 0;
					HitEffect();
					Active = 0;
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.SendNpcHurt(WhoAmI, -1);
					}
					return;
				}
				if (Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.DEVOURER_BODY || Type == (int)ID.GIANT_WORM_HEAD || Type == (int)ID.GIANT_WORM_BODY || Type == (int)ID.BONE_SERPENT_HEAD || Type == (int)ID.BONE_SERPENT_BODY || Type == (int)ID.DIGGER_HEAD || Type == (int)ID.DIGGER_BODY || Type == (int)ID.SEEKER_HEAD || Type == (int)ID.SEEKER_BODY || (Type >= (int)ID.WYVERN_HEAD && Type < (int)ID.WYVERN_TAIL) || (Type >= (int)ID.ARCH_WYVERN_HEAD && Type < (int)ID.ARCH_WYVERN_TAIL) || Type == (int)ID.LEECH_HEAD || Type == (int)ID.LEECH_BODY)
				{
					if (Main.NPCSet[(int)AI0].Active == 0 || Main.NPCSet[(int)AI0].AIStyle != AIStyle)
					{
						Life = 0;
						HitEffect();
						Active = 0;
						if (Main.NetMode == (byte)NetModeSetting.SERVER)
						{
							NetMessage.SendNpcHurt(WhoAmI, -1);
						}
						return;
					}
				}
				else if (Type >= (int)ID.EATER_OF_WORLDS_HEAD && Type <= (int)ID.EATER_OF_WORLDS_TAIL)
				{
					if ((Main.NPCSet[(int)AI1].Active == 0 && Main.NPCSet[(int)AI0].Active == 0) || (Type == (int)ID.EATER_OF_WORLDS_HEAD && Main.NPCSet[(int)AI0].Active == 0) || (Type == (int)ID.EATER_OF_WORLDS_TAIL && Main.NPCSet[(int)AI1].Active == 0))
					{
						Life = 0;
						HitEffect();
						Active = 0;
					}
					if (Type == (int)ID.EATER_OF_WORLDS_BODY)
					{
						if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != AIStyle)
						{
							Type = (int)ID.EATER_OF_WORLDS_HEAD;
							int num6 = WhoAmI;
							float num7 = Life / (float)LifeMax;
							float num8 = AI0;
							SetDefaults(Type);
							Life = (int)(LifeMax * num7);
							AI0 = num8;
							TargetClosest();
							ShouldNetUpdate = true;
							WhoAmI = (short)num6;
						}
						else if (Main.NPCSet[(int)AI0].Active == 0 || Main.NPCSet[(int)AI0].AIStyle != AIStyle)
						{
							int num9 = WhoAmI;
							float num10 = Life / (float)LifeMax;
							float num11 = AI1;
							SetDefaults(Type);
							Life = (int)(LifeMax * num10);
							AI1 = num11;
							TargetClosest();
							ShouldNetUpdate = true;
							WhoAmI = (short)num9;
						}
					}
					if (Life == 0)
					{
						bool flag = true;
						for (int k = 0; k < MaxNumNPCs; k++)
						{
							if (Main.NPCSet[k].Type >= (int)ID.EATER_OF_WORLDS_HEAD && Main.NPCSet[k].Type <= (int)ID.EATER_OF_WORLDS_TAIL && Main.NPCSet[k].Active != 0)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							IsBoss = true;
							NPCLoot();
						}
#if !VERSION_INITIAL
					}
				}
				if (Active == 0)
				{
					NetMessage.SendNpcHurt(WhoAmI, -1);
					return;
				}
#else
						// BUG: Having this included in the EoW check means it will be viewed as damaged by the game after it has been declared inactive and at 0 HP.
						// This results in a bug which can allow for one to kill the EoW with a magic mirror... or any method to prime it to despawn.
						if (Active == 0) 
						{
							NetMessage.SendNpcHurt(WhoAmI, -1);
							return;
						}
					}
				}
#endif
			}
			int num12 = (XYWH.X >> 4) - 1;
			int num13 = (XYWH.X + Width >> 4) + 2;
			int num14 = (XYWH.Y >> 4) - 1;
			int num15 = (XYWH.Y + Height >> 4) + 2;
			if (num12 < 0)
			{
				num12 = 0;
			}
			if (num13 > Main.MaxTilesX)
			{
				num13 = Main.MaxTilesX;
			}
			if (num14 < 0)
			{
				num14 = 0;
			}
			if (num15 > Main.MaxTilesY)
			{
				num15 = Main.MaxTilesY;
			}
			bool flag2 = (Type >= (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type >= (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL);
			if (!flag2)
			{
				Vector2 vector = default;
				for (int l = num12; l < num13; l++)
				{
					for (int m = num14; m < num15; m++)
					{
						if (!Main.TileSet[l, m].CanStandOnTop() && Main.TileSet[l, m].Liquid <= 64)
						{
							continue;
						}
						vector.X = l * 16;
						vector.Y = m * 16;
						if (Position.X + Width > vector.X && Position.X < vector.X + 16f && Position.Y + Height > vector.Y && Position.Y < vector.Y + 16f)
						{
							flag2 = true;
							if (Main.Rand.Next(100) == 0 && Type != (int)ID.LEECH_HEAD && Main.TileSet[l, m].IsActive != 0)
							{
								WorldGen.KillTile(l, m, KillToFail: true, EffectOnly: true);
							}
						}
					}
				}
			}
			if (!flag2 && (Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.GIANT_WORM_HEAD || Type == (int)ID.EATER_OF_WORLDS_HEAD || Type == (int)ID.BONE_SERPENT_HEAD || Type == (int)ID.DIGGER_HEAD || Type == (int)ID.SEEKER_HEAD || Type == (int)ID.LEECH_HEAD))
			{
				bool flag3 = true;
				for (int n = 0; n < Player.MaxNumPlayers; n++)
				{
					if (Main.PlayerSet[n].Active != 0)
					{
						Rectangle rectangle = new Rectangle(Main.PlayerSet[n].XYWH.X - 1000, Main.PlayerSet[n].XYWH.Y - 1000, 2000, 2000);
						if (XYWH.Intersects(rectangle))
						{
							flag3 = false;
							break;
						}
					}
				}
				if (flag3)
				{
					flag2 = true;
				}
			}
			if ((Type >= (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type >= (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL))
			{
				if (Velocity.X < 0f)
				{
					SpriteDirection = 1;
				}
				else if (Velocity.X > 0f)
				{
					SpriteDirection = -1;
				}
			}
			float num16 = 8f;
			float num17 = 0.07f;
			if (Type == (int)ID.DIGGER_HEAD)
			{
				num16 = 5.5f;
				num17 = 0.045f;
			}
			else if (Type == (int)ID.GIANT_WORM_HEAD)
			{
				num16 = 6f;
				num17 = 0.05f;
			}
			else if (Type == (int)ID.EATER_OF_WORLDS_HEAD)
			{
				num16 = 10f;
				num17 = 0.07f;
			}
			else if (Type == (int)ID.WYVERN_HEAD || Type == (int)ID.ARCH_WYVERN_HEAD)
			{
				num16 = 11f;
				num17 = 0.25f;
			}
			else if (Type == (int)ID.LEECH_HEAD && WoF >= 0)
			{
				float num18 = Main.NPCSet[WoF].Life / (float)Main.NPCSet[WoF].LifeMax;
				if ((double)num18 < 0.5)
				{
					num16 += 1f;
					num17 += 0.1f;
				}
				if ((double)num18 < 0.25)
				{
					num16 += 1f;
					num17 += 0.1f;
				}
				if ((double)num18 < 0.1)
				{
					num16 += 2f;
					num17 += 0.1f;
				}
			}
			Vector2 vector2 = Position;
			vector2.X += Width >> 1;
			vector2.Y += Height >> 1;
			float num19;
			float num20;
			if (AI1 > 0f && AI1 < MaxNumNPCs)
			{
				vector2.X = Position.X + (Width >> 1);
				vector2.Y = Position.Y + (Height >> 1);
				num19 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - vector2.X;
				num20 = Main.NPCSet[(int)AI1].Position.Y + (Main.NPCSet[(int)AI1].Height >> 1) - vector2.Y;
				Rotation = (float)(Math.Atan2(num20, num19) + Math.PI / 2.0);
				float num21 = num19 * num19 + num20 * num20;
				bool flag4 = (Type >= (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type >= (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL);
				if (num21 > 0f)
				{
					num21 = (float)Math.Sqrt(num21);
					num21 = (num21 - (flag4 ? 30 : Width)) / num21;
					num19 *= num21;
					num20 *= num21;
					Position.X += num19;
					Position.Y += num20;
					XYWH.X = (int)Position.X;
					XYWH.Y = (int)Position.Y;
				}
				Velocity.X = 0f;
				Velocity.Y = 0f;
				if (flag4)
				{
					if (num19 < 0f)
					{
						SpriteDirection = 1;
					}
					else if (num19 > 0f)
					{
						SpriteDirection = -1;
					}
				}
				return;
			}
			num19 = (Main.PlayerSet[Target].XYWH.X + 10) & -16;
			num20 = (Main.PlayerSet[Target].XYWH.Y + 21) & -16;
			vector2.X = (int)vector2.X & -16;
			vector2.Y = (int)vector2.Y & -16;
			num19 -= vector2.X;
			num20 -= vector2.Y;
			if (!flag2)
			{
				TargetClosest();
				Velocity.Y += 0.11f;
				if (Velocity.Y > num16)
				{
					Velocity.Y = num16;
				}
				if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num16 * 0.4)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X -= num17 * 1.1f;
					}
					else
					{
						Velocity.X += num17 * 1.1f;
					}
				}
				else if (Velocity.Y == num16)
				{
					if (Velocity.X < num19)
					{
						Velocity.X += num17;
					}
					else if (Velocity.X > num19)
					{
						Velocity.X -= num17;
					}
				}
				else if (Velocity.Y > 4f)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X += num17 * 0.9f;
					}
					else
					{
						Velocity.X -= num17 * 0.9f;
					}
				}
			}
			else
			{
				float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
				if (SoundDelay == 0 && Type != (int)ID.WYVERN_HEAD && Type != (int)ID.ARCH_WYVERN_HEAD && Type != (int)ID.LEECH_HEAD)
				{
					int num22 = (int)(num21 * 0.025f);
					if (num22 < 10)
					{
						num22 = 10;
					}
					else if (num22 > 20)
					{
						num22 = 20;
					}
					SoundDelay = (short)num22;
					Main.PlaySound(15, XYWH.X, XYWH.Y);
				}
				float num23 = Math.Abs(num19);
				float num24 = Math.Abs(num20);
				float num25 = num16 / num21;
				num19 *= num25;
				num20 *= num25;
				if ((Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.EATER_OF_WORLDS_HEAD) && !Main.PlayerSet[Target].ZoneEvil)
				{
					bool flag5 = true;
					for (int num26 = 0; num26 < 8; num26++)
					{
						if (Main.PlayerSet[num26].Active != 0 && !Main.PlayerSet[num26].IsDead && Main.PlayerSet[num26].ZoneEvil)
						{
							flag5 = false;
							break;
						}
					}
					if (flag5)
					{
						if (Main.NetMode != (byte)NetModeSetting.CLIENT && XYWH.Y >> 4 > Main.RockLayer + Main.MaxTilesY >> 1)
						{
							Active = 0;
							int num27 = (int)AI0;
							while (num27 > 0 && num27 < MaxNumNPCs && Main.NPCSet[num27].Active != 0 && Main.NPCSet[num27].AIStyle == AIStyle)
							{
								int num28 = (int)Main.NPCSet[num27].AI0;
								Main.NPCSet[num27].Active = 0;
								Life = 0;
								NetMessage.CreateMessage1(23, num27);
								NetMessage.SendMessage();
								num27 = num28;
							}
							NetMessage.CreateMessage1(23, WhoAmI);
							NetMessage.SendMessage();
							return;
						}
						num19 = 0f;
						num20 = num16;
					}
				}
				bool flag6 = false;
				if (Type == (int)ID.WYVERN_HEAD || Type == (int)ID.ARCH_WYVERN_HEAD)
				{
					if (((Velocity.X > 0f && num19 < 0f) || (Velocity.X < 0f && num19 > 0f) || (Velocity.Y > 0f && num20 < 0f) || (Velocity.Y < 0f && num20 > 0f)) && Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) > num17 * 0.5f && num21 < 300f)
					{
						flag6 = true;
						if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) < num16)
						{
							Velocity.X *= 1.1f;
							Velocity.Y *= 1.1f;
						}
					}
					if (XYWH.Y > Main.PlayerSet[Target].XYWH.Y || Main.PlayerSet[Target].XYWH.Y > Main.WorldSurfacePixels || Main.PlayerSet[Target].IsDead)
					{
						flag6 = true;
						if (Math.Abs(Velocity.X) < num16 * 0.5f)
						{
							if (Velocity.X == 0f)
							{
								Velocity.X -= Direction;
							}
							Velocity.X *= 1.1f;
						}
						else if (Velocity.Y > 0f - num16)
						{
							Velocity.Y -= num17;
						}
					}
				}
				if (!flag6)
				{
					if ((Velocity.X > 0f && num19 > 0f) || (Velocity.X < 0f && num19 < 0f) || (Velocity.Y > 0f && num20 > 0f) || (Velocity.Y < 0f && num20 < 0f))
					{
						if (Velocity.X < num19)
						{
							Velocity.X += num17;
						}
						else if (Velocity.X > num19)
						{
							Velocity.X -= num17;
						}
						if (Velocity.Y < num20)
						{
							Velocity.Y += num17;
						}
						else if (Velocity.Y > num20)
						{
							Velocity.Y -= num17;
						}
						if ((double)Math.Abs(num20) < (double)num16 * 0.2 && ((Velocity.X > 0f && num19 < 0f) || (Velocity.X < 0f && num19 > 0f)))
						{
							if (Velocity.Y > 0f)
							{
								Velocity.Y += num17 * 2f;
							}
							else
							{
								Velocity.Y -= num17 * 2f;
							}
						}
						if ((double)Math.Abs(num19) < (double)num16 * 0.2 && ((Velocity.Y > 0f && num20 < 0f) || (Velocity.Y < 0f && num20 > 0f)))
						{
							if (Velocity.X > 0f)
							{
								Velocity.X += num17 * 2f;
							}
							else
							{
								Velocity.X -= num17 * 2f;
							}
						}
					}
					else if (num23 > num24)
					{
						if (Velocity.X < num19)
						{
							Velocity.X += num17 * 1.1f;
						}
						else if (Velocity.X > num19)
						{
							Velocity.X -= num17 * 1.1f;
						}
						if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num16 * 0.5)
						{
							if (Velocity.Y > 0f)
							{
								Velocity.Y += num17;
							}
							else
							{
								Velocity.Y -= num17;
							}
						}
					}
					else
					{
						if (Velocity.Y < num20)
						{
							Velocity.Y += num17 * 1.1f;
						}
						else if (Velocity.Y > num20)
						{
							Velocity.Y -= num17 * 1.1f;
						}
						if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num16 * 0.5)
						{
							if (Velocity.X > 0f)
							{
								Velocity.X += num17;
							}
							else
							{
								Velocity.X -= num17;
							}
						}
					}
				}
			}
			Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 1.57f;
			if (Type != (int)ID.DEVOURER_HEAD && Type != (int)ID.GIANT_WORM_HEAD && Type != (int)ID.EATER_OF_WORLDS_HEAD && Type != (int)ID.BONE_SERPENT_HEAD && Type != (int)ID.DIGGER_HEAD && Type != (int)ID.SEEKER_HEAD && Type != (int)ID.LEECH_HEAD)
			{
				return;
			}
			if (flag2)
			{
				if (LocalAI0 != 1)
				{
					ShouldNetUpdate = true;
				}
				LocalAI0 = 1;
			}
			else
			{
				if (LocalAI0 != 0)
				{
					ShouldNetUpdate = true;
				}
				LocalAI0 = 0;
			}
			if (((Velocity.X > 0f && OldVelocity.X < 0f) || (Velocity.X < 0f && OldVelocity.X > 0f) || (Velocity.Y > 0f && OldVelocity.Y < 0f) || (Velocity.Y < 0f && OldVelocity.Y > 0f)) && !WasJustHit)
			{
				ShouldNetUpdate = true;
			}
		}

		private void TownsfolkAI()
		{
			if (Type == (int)ID.BUNNY)
			{
				if (Target == Player.MaxNumPlayers)
				{
					TargetClosest();
				}
			}
			else if (Type == (int)ID.GOBLIN_TINKERER)
			{
				HasSavedGoblin = true;
			}
			else if (Type == (int)ID.WIZARD)
			{
				HasSavedWizard = true;
			}
			else if (Type == (int)ID.MECHANIC)
			{
				HasSavedMech = true;
			}
			else if (Type == (int)ID.SANTA_CLAUS && Main.NetMode != (byte)NetModeSetting.CLIENT && !Time.xMas)
			{
				StrikeNPC(9999, 0f, 0); // Fucking R.I.P Santa bro; 9999 damage on arrival.
				NetMessage.SendNpcHurt(WhoAmI, 9999, 0.0, 0);
			}
			int num = XYWH.X + (Width >> 1) >> 4;
			int num2 = XYWH.Y + Height + 1 >> 4;
			bool flag = false;
			DirectionY = -1;
			Direction |= 1;
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active != 0 && Main.PlayerSet[i].TalkNPC == WhoAmI)
				{
					flag = true;
					if (AI0 != 0f)
					{
						ShouldNetUpdate = true;
					}
					AI0 = 0f;
					AI1 = 300f;
					AI2 = 100f;
					if (Main.PlayerSet[i].XYWH.X + 10 < XYWH.X + (Width >> 1))
					{
						Direction = -1;
					}
					else
					{
						Direction = 1;
					}
				}
			}
			if (AI3 > 0f && Active != 0)
			{
				Life = -1;
				HitEffect();
				Active = 0;
				if (Type == (int)ID.OLD_MAN)
				{
					Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				}
			}
			if (Type == (int)ID.OLD_MAN && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				IsHomeless = false;
				HomeTileX = Main.DungeonX;
				HomeTileY = Main.DungeonY;
				if (HasDownedBoss3)
				{
					AI3 = 1f;
					ShouldNetUpdate = true;
				}
			}
			int j = HomeTileY;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && j > 0)
			{
				for (; !WorldGen.SolidTile(HomeTileX, j) && j < Main.MaxTilesY - 20; j++)
				{
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && IsTownNPC && !IsHomeless && (num != HomeTileX || num2 != j) && (!Main.GameTime.DayTime || Main.TileDungeon[Main.TileSet[num, num2].Type]))
			{
				bool flag2 = true;
				Rectangle rectangle = default(Rectangle);
				rectangle.X = XYWH.X + (Width >> 1) - (SpawnWidth / 2) - SafeRangeX;
				rectangle.Y = XYWH.Y + (Height >> 1) - (SpawnHeight / 2) - SafeRangeY;
				rectangle.Width = SpawnWidth + SafeRangeX * 2;
				rectangle.Height = SpawnHeight + SafeRangeY * 2;
				for (int k = 0; k < 8; k++)
				{
					if (Main.PlayerSet[k].Active != 0 && rectangle.Intersects(Main.PlayerSet[k].XYWH))
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					rectangle.X = HomeTileX * 16 + 8 - (SpawnWidth / 2) - SafeRangeX;
					rectangle.Y = j * 16 + 8 - (SpawnHeight / 2) - SafeRangeY;
					for (int l = 0; l < 8; l++)
					{
						if (Main.PlayerSet[l].Active != 0 && rectangle.Intersects(Main.PlayerSet[l].XYWH))
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						if (Type == (int)ID.OLD_MAN || !Collision.SolidTiles(HomeTileX - 1, HomeTileX + 1, j - 3, j - 1))
						{
							Velocity.X = 0f;
							Velocity.Y = 0f;
							Position.X = (XYWH.X = (HomeTileX << 4) + 8 - (Width >> 1));
							Position.Y = (j << 4) - Height - 0.1f;
							XYWH.Y = (int)Position.Y;
							ShouldNetUpdate = true;
						}
						else
						{
							IsHomeless = true;
							WorldGen.QuickFindHome(WhoAmI);
						}
					}
				}
			}
			if (AI0 == 0f)
			{
				if (AI2 > 0f)
				{
					AI2 -= 1f;
				}
				if (!Main.GameTime.DayTime && !flag && Type != (int)ID.BUNNY)
				{
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						if (num == HomeTileX && num2 == j)
						{
							if (Velocity.X != 0f)
							{
								ShouldNetUpdate = true;
							}
							if (Velocity.X > 0.1)
							{
								Velocity.X -= 0.1f;
							}
							else if (Velocity.X < -0.1)
							{
								Velocity.X += 0.1f;
							}
							else
							{
								Velocity.X = 0f;
							}
						}
						else if (!flag)
						{
							if (num > HomeTileX)
							{
								Direction = -1;
							}
							else
							{
								Direction = 1;
							}
							AI0 = 1f;
							AI1 = 200 + Main.Rand.Next(200);
							AI2 = 0f;
							ShouldNetUpdate = true;
						}
					}
				}
				else
				{
					if (Velocity.X > 0.1)
					{
						Velocity.X -= 0.1f;
					}
					else if (Velocity.X < -0.1)
					{
						Velocity.X += 0.1f;
					}
					else
					{
						Velocity.X = 0f;
					}
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						if (AI1 > 0f)
						{
							AI1 -= 1f;
						}
						if (AI1 <= 0f)
						{
							AI0 = 1f;
							AI1 = 200 + Main.Rand.Next(200);
							if (Type == (int)ID.BUNNY)
							{
								AI1 += Main.Rand.Next(200, 400);
							}
							AI2 = 0f;
							ShouldNetUpdate = true;
						}
					}
				}
				if (Main.NetMode == (byte)NetModeSetting.CLIENT || (!Main.GameTime.DayTime && (num != HomeTileX || num2 != j)))
				{
					return;
				}
				if (num < HomeTileX - 25 || num > HomeTileX + 25)
				{
					if (AI2 == 0f)
					{
						if (num < HomeTileX - 50 && Direction == -1)
						{
							Direction = 1;
							ShouldNetUpdate = true;
						}
						else if (num > HomeTileX + 50 && Direction == 1)
						{
							Direction = -1;
							ShouldNetUpdate = true;
						}
					}
				}
				else if (Main.Rand.Next(80) == 0 && AI2 == 0f)
				{
					AI2 = 200f;
					Direction = (sbyte)(-Direction);
					ShouldNetUpdate = true;
				}
			}
			else
			{
				if (AI0 != 1f)
				{
					return;
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT && !Main.GameTime.DayTime && num == HomeTileX && num2 == HomeTileY && Type != (int)ID.BUNNY)
				{
					AI0 = 0f;
					AI1 = 200 + Main.Rand.Next(200);
					AI2 = 60f;
					ShouldNetUpdate = true;
					return;
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT && !IsHomeless && !Main.TileDungeon[Main.TileSet[num, num2].Type] && (num < HomeTileX - 35 || num > HomeTileX + 35))
				{
					if (XYWH.X < HomeTileX << 4 && Direction == -1)
					{
						AI1 -= 5f;
					}
					else if (XYWH.X > HomeTileX << 4 && Direction == 1)
					{
						AI1 -= 5f;
					}
				}
				AI1 -= 1f;
				if (AI1 <= 0f)
				{
					AI0 = 0f;
					AI1 = 300 + Main.Rand.Next(300);
					if (Type == (int)ID.BUNNY)
					{
						AI1 -= Main.Rand.Next(100);
					}
					AI2 = 60f;
					ShouldNetUpdate = true;
				}
				if (HasClosedDoor)
				{
					int num3 = XYWH.X + (Width >> 1) >> 4;
					if (num3 > DoorX + 2 || num3 < DoorX - 2)
					{
						if (WorldGen.CloseDoor(DoorX, DoorY))
						{
							HasClosedDoor = false;
							NetMessage.CreateMessage2(24, DoorX, DoorY);
							NetMessage.SendMessage();
						}
						else
						{
							int num4 = XYWH.Y + (Height >> 1) >> 4;
							if (num3 > DoorX + 4 || num3 < DoorX - 4 || num4 > DoorY + 4 || num4 < DoorY - 4)
							{
								HasClosedDoor = false;
							}
						}
					}
				}
				if (Velocity.X < -1f || Velocity.X > 1f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 1.15 && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > 1f)
					{
						Velocity.X = 1f;
					}
				}
				else if (Velocity.X > -1f && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X > 1f)
					{
						Velocity.X = 1f;
					}
				}
				if (Velocity.Y != 0f)
				{
					return;
				}
				if (Position.X == AI2)
				{
					Direction = (sbyte)(-Direction);
				}
				AI2 = -1f;
				int num5 = XYWH.X + (Width >> 1) + 15 * Direction >> 4;
				int num6 = XYWH.Y + Height - 16 >> 4;
				if (IsTownNPC && Main.TileSet[num5, num6 - 2].IsActive != 0 && Main.TileSet[num5, num6 - 2].Type == 10 && (Main.Rand.Next(10) == 0 || !Main.GameTime.DayTime))
				{
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						int num7 = WorldGen.OpenDoor(num5, num6 - 2, Direction);
						if (num7 != 0)
						{
							HasClosedDoor = true;
							DoorX = (short)num5;
							DoorY = (short)(num6 - 2);
							NetMessage.CreateMessage3(19, num5, num6 - 2, num7);
							NetMessage.SendMessage();
							AI1 += 80f;
						}
						else
						{
							Direction = (sbyte)(-Direction);
						}
						ShouldNetUpdate = true;
					}
					return;
				}
				if ((Velocity.X < 0f && SpriteDirection == -1) || (Velocity.X > 0f && SpriteDirection == 1))
				{
					if (Main.TileSet[num5, num6 - 2].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num5, num6 - 2].Type])
					{
						if ((Direction == 1 && !Collision.SolidTiles(num5 - 2, num5 - 1, num6 - 5, num6 - 1)) || (Direction == -1 && !Collision.SolidTiles(num5 + 1, num5 + 2, num6 - 5, num6 - 1)))
						{
							if (!Collision.SolidTiles(num5, num5, num6 - 5, num6 - 3))
							{
								Velocity.Y = -6f;
							}
							else
							{
								Direction = (sbyte)(-Direction);
							}
						}
						else
						{
							Direction = (sbyte)(-Direction);
						}
						ShouldNetUpdate = true;
					}
					else if (Main.TileSet[num5, num6 - 1].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num5, num6 - 1].Type])
					{
						if ((Direction == 1 && !Collision.SolidTiles(num5 - 2, num5 - 1, num6 - 4, num6 - 1)) || (Direction == -1 && !Collision.SolidTiles(num5 + 1, num5 + 2, num6 - 4, num6 - 1)))
						{
							if (!Collision.SolidTiles(num5, num5, num6 - 4, num6 - 2))
							{
								Velocity.Y = -5f;
							}
							else
							{
								Direction = (sbyte)(-Direction);
							}
						}
						else
						{
							Direction = (sbyte)(-Direction);
						}
						ShouldNetUpdate = true;
					}
					else if (Main.TileSet[num5, num6].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[num5, num6].Type])
					{
						if ((Direction == 1 && !Collision.SolidTiles(num5 - 2, num5, num6 - 3, num6 - 1)) || (Direction == -1 && !Collision.SolidTiles(num5, num5 + 2, num6 - 3, num6 - 1)))
						{
							Velocity.Y = -3.6f;
						}
						else
						{
							Direction = (sbyte)(-Direction);
						}
						ShouldNetUpdate = true;
					}
					if (num >= HomeTileX - 35 && num <= HomeTileX + 35 && (Main.TileSet[num5, num6 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5, num6 + 1].Type]) && (Main.TileSet[num5 - Direction, num6 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5 - Direction, num6 + 1].Type]) && (Main.TileSet[num5, num6 + 2].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5, num6 + 2].Type]) && (Main.TileSet[num5 - Direction, num6 + 2].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5 - Direction, num6 + 2].Type]) && (Main.TileSet[num5, num6 + 3].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5, num6 + 3].Type]) && (Main.TileSet[num5 - Direction, num6 + 3].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5 - Direction, num6 + 3].Type]) && (Main.TileSet[num5, num6 + 4].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5, num6 + 4].Type]) && (Main.TileSet[num5 - Direction, num6 + 4].IsActive == 0 || !Main.TileSolid[Main.TileSet[num5 - Direction, num6 + 4].Type]) && Type != (int)ID.BUNNY)
					{
						Direction = (sbyte)(-Direction);
						Velocity.X = 0f - Velocity.X;
						ShouldNetUpdate = true;
					}
					if (Velocity.Y < 0f)
					{
						AI2 = Position.X;
					}
				}
				if (Velocity.Y < 0f)
				{
					if (IsWet)
					{
						Velocity.Y *= 1.2f;
					}
					if (Type == (int)ID.BUNNY)
					{
						Velocity.Y *= 1.2f;
					}
				}
			}
		}

		private unsafe void SorcererAI()
		{
			TargetClosest();
			Velocity.X *= 0.93f;
			if (Velocity.X > -0.1 && Velocity.X < 0.1)
			{
				Velocity.X = 0f;
			}
			if (AI0 == 0f)
			{
				AI0 = 500f;
			}
			if (AI2 != 0f && AI3 != 0f)
			{
				Main.PlaySound(2, XYWH.X, XYWH.Y, 8);
				for (int i = 0; i < 42; i++)
				{
					Dust* ptr;
					if (Type == (int)ID.GOBLIN_SORCERER || Type == (int)ID.TIM)
					{
						ptr = Main.DustSet.NewDust(27, ref XYWH, 0.0, 0.0, 100, default(Color), Main.Rand.Next(1, 3));
						if (ptr == null)
						{
							break;
						}
						if (ptr->Scale > 1f)
						{
							ptr->NoGravity = true;
						}
					}
					else if (Type == (int)ID.DARK_CASTER)
					{
						ptr = Main.DustSet.NewDust(29, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
						if (ptr == null)
						{
							break;
						}
						ptr->NoGravity = true;
					}
					else
					{
						ptr = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
						if (ptr == null)
						{
							break;
						}
						ptr->NoGravity = true;
					}
					ptr->Velocity.X *= 3f;
					ptr->Velocity.Y *= 3f;
				}
				Position.X = AI2 * 16f - (Width >> 1) + 8f;
				Position.Y = AI3 * 16f - Height;
				XYWH.X = (int)Position.X;
				XYWH.Y = (int)Position.Y;
				Velocity.X = 0f;
				Velocity.Y = 0f;
				AI2 = 0f;
				AI3 = 0f;
				Main.PlaySound(2, XYWH.X, XYWH.Y, 8);
				for (int j = 0; j < 42; j++)
				{
					Dust* ptr2;
					if (Type == (int)ID.GOBLIN_SORCERER || Type == (int)ID.TIM)
					{
						ptr2 = Main.DustSet.NewDust(27, ref XYWH, 0.0, 0.0, 100, default(Color), Main.Rand.Next(1, 3));
						if (ptr2 == null)
						{
							break;
						}
						if (ptr2->Scale > 1f)
						{
							ptr2->NoGravity = true;
						}
					}
					else if (Type == (int)ID.DARK_CASTER)
					{
						ptr2 = Main.DustSet.NewDust(29, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
						if (ptr2 == null)
						{
							break;
						}
						ptr2->NoGravity = true;
					}
					else
					{
						ptr2 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
						if (ptr2 == null)
						{
							break;
						}
						ptr2->NoGravity = true;
					}
					ptr2->Velocity.X *= 3f;
					ptr2->Velocity.Y *= 3f;
				}
			}
			AI0 += 1f;
			if (AI0 == 100f || AI0 == 200f || AI0 == 300f)
			{
				AI1 = 30f;
				ShouldNetUpdate = true;
			}
			else if (AI0 >= 650f && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				AI0 = 1f;
				int num = Main.PlayerSet[Target].XYWH.X >> 4;
				int num2 = Main.PlayerSet[Target].XYWH.Y >> 4;
				int num3 = XYWH.X >> 4;
				int num4 = XYWH.Y >> 4;
				int num5 = 20;
				int num6 = 0;
				bool flag = false;
				if (Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) + Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 2000)
				{
					num6 = 100;
					flag = true;
				}
				while (!flag && num6 < 100)
				{
					num6++;
					int num7 = Main.Rand.Next(num - num5, num + num5);
					int num8 = Main.Rand.Next(num2 - num5, num2 + num5);
					for (int k = num8; k < num2 + num5; k++)
					{
						if ((k < num2 - 4 || k > num2 + 4 || num7 < num - 4 || num7 > num + 4) && (k < num4 - 1 || k > num4 + 1 || num7 < num3 - 1 || num7 > num3 + 1) && Main.TileSet[num7, k].IsActive != 0)
						{
							bool flag2 = true;
							if (Type == (int)ID.DARK_CASTER && Main.TileSet[num7, k - 1].WallType == 0)
							{
								flag2 = false;
							}
							else if (Main.TileSet[num7, k - 1].Lava != 0)
							{
								flag2 = false;
							}
							if (flag2 && Main.TileSolid[Main.TileSet[num7, k].Type] && !Collision.SolidTiles(num7 - 1, num7 + 1, k - 4, k - 1))
							{
								AI1 = 20f;
								AI2 = num7;
								AI3 = k;
								flag = true;
								break;
							}
						}
					}
				}
				ShouldNetUpdate = true;
			}
			if (AI1 > 0f)
			{
				AI1 -= 1f;
				if (AI1 == 25f)
				{
					Main.PlaySound(2, XYWH.X, XYWH.Y, 8);
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						if (Type == (int)ID.GOBLIN_SORCERER || Type == (int)ID.TIM)
						{
							NewNPC(XYWH.X + (Width >> 1), XYWH.Y - 8, (int)ID.CHAOS_BALL);
						}
						else if (Type == (int)ID.DARK_CASTER)
						{
							NewNPC(XYWH.X + (Width >> 1), XYWH.Y - 8, (int)ID.WATER_SPHERE);
						}
						else
						{
							NewNPC(XYWH.X + (Width >> 1) + Direction * 8, XYWH.Y + 20, (int)ID.BURNING_SPHERE);
						}
					}
				}
			}
			if (Type == (int)ID.GOBLIN_SORCERER || Type == (int)ID.TIM)
			{
				if (Main.Rand.Next(5) == 0)
				{
					Dust* ptr3 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 27, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 1.5);
					if (ptr3 != null)
					{
						ptr3->NoGravity = true;
						ptr3->Velocity.X *= 0.5f;
						ptr3->Velocity.Y = -2f;
					}
				}
			}
			else if (Type == (int)ID.DARK_CASTER)
			{
				if (Main.Rand.Next(2) == 0)
				{
					Dust* ptr4 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 29, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
					if (ptr4 != null)
					{
						ptr4->NoGravity = true;
						ptr4->Velocity.X *= 1f;
						ptr4->Velocity.Y *= 1f;
					}
				}
			}
			else if (Main.Rand.Next(2) == 0)
			{
				Dust* ptr5 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 6, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
				if (ptr5 != null)
				{
					ptr5->NoGravity = true;
					ptr5->Velocity.X *= 1f;
					ptr5->Velocity.Y *= 1f;
				}
			}
		}

		private unsafe void SphereAI()
		{
			if (Target == Player.MaxNumPlayers)
			{
				TargetClosest();
				float num = 6f;
				if (Type == (int)ID.BURNING_SPHERE)
				{
					num = 5f;
				}
				if (Type == (int)ID.VILE_SPIT)
				{
					num = 7f;
				}
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				num4 = num / num4;
				Velocity.X = num2 * num4;
				Velocity.Y = num3 * num4;
			}
			if (Type == (int)ID.VILE_SPIT)
			{
				AI0 += 1f;
				if (AI0 > 3f)
				{
					AI0 = 3f;
				}
				if (AI0 == 2f)
				{
					Position.X += Velocity.X;
					Position.Y += Velocity.Y;
					XYWH.X = (int)Position.X;
					XYWH.Y = (int)Position.Y;
					Main.PlaySound(4, XYWH.X, XYWH.Y, 9);
					for (int i = 0; i < 16; i++)
					{
						Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 18, 0.0, 0.0, 100, default(Color), 1.8f);
						if (ptr == null)
						{
							break;
						}
						ptr->Velocity.X *= 1.3f;
						ptr->Velocity.Y *= 1.3f;
						ptr->Velocity.X += Velocity.X;
						ptr->Velocity.Y += Velocity.Y;
						ptr->NoGravity = true;
					}
				}
				if (Collision.SolidCollision(ref Position, Width, Height))
				{
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						int num5 = XYWH.X + (Width >> 1) >> 4;
						int num6 = XYWH.Y + (Height >> 1) >> 4;
						int num7 = 8;
						for (int j = num5 - num7; j <= num5 + num7; j++)
						{
							for (int k = num6 - num7; k < num6 + num7; k++)
							{
								if (Math.Abs(j - num5) + Math.Abs(k - num6) < num7 * 0.5)
								{
									if (Main.TileSet[j, k].Type == 2)
									{
										Main.TileSet[j, k].Type = 23;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
									else if (Main.TileSet[j, k].Type == 1)
									{
										Main.TileSet[j, k].Type = 25;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
									else if (Main.TileSet[j, k].Type == 53)
									{
										Main.TileSet[j, k].Type = 112;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
									else if (Main.TileSet[j, k].Type == 109)
									{
										Main.TileSet[j, k].Type = 23;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
									else if (Main.TileSet[j, k].Type == 117)
									{
										Main.TileSet[j, k].Type = 25;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
									else if (Main.TileSet[j, k].Type == 116)
									{
										Main.TileSet[j, k].Type = 112;
										WorldGen.SquareTileFrame(j, k);
										NetMessage.SendTile(j, k);
									}
								}
							}
						}
					}
					StrikeNPC(999, 0f, 0);
				}
			}
			if (TimeLeft > 100)
			{
				TimeLeft = 100;
			}
			for (int l = 0; l < 2; l++)
			{
				Dust* ptr2;
				if (Type == (int)ID.CHAOS_BALL)
				{
					ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 27, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
					if (ptr2 == null)
					{
						break;
					}
				}
				else if (Type == (int)ID.WATER_SPHERE)
				{
					ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 29, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
					if (ptr2 == null)
					{
						break;
					}
				}
				else if (Type == (int)ID.VILE_SPIT)
				{
					ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 18, Velocity.X * 0.1f, Velocity.Y * 0.1f, 80, default(Color), 1.3f);
					if (ptr2 == null)
					{
						break;
					}
				}
				else
				{
					Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(1f, 0.3f, 0.1f));
					ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 6, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
				}
				if (ptr2 != null)
				{
					ptr2->NoGravity = true;
					ptr2->Velocity.X *= 0.3f;
					ptr2->Velocity.Y *= 0.3f;
					if (Type == (int)ID.CHAOS_BALL)
					{
						ptr2->Velocity.X -= Velocity.X * 0.2f;
						ptr2->Velocity.Y -= Velocity.Y * 0.2f;
					}
				}
			}
			Rotation += 0.4f * Direction;
		}

		private void SkullHeadAI()
		{
			float num = 1f;
			float num2 = 0.011f;
			TargetClosest();
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num3 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
			float num4 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
			float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
			float num6 = num5;
			AI1 += 1f;
			if (AI1 > 600f)
			{
				num2 *= 8f;
				num = 4f;
				if (AI1 > 650f)
				{
					AI1 = 0f;
				}
			}
			else if (num6 < 250f)
			{
				AI0 += 0.9f;
				if (AI0 > 0f)
				{
					Velocity.Y += 0.019f;
				}
				else
				{
					Velocity.Y -= 0.019f;
				}
				if (AI0 < -100f || AI0 > 100f)
				{
					Velocity.X += 0.019f;
				}
				else
				{
					Velocity.X -= 0.019f;
				}
				if (AI0 > 200f)
				{
					AI0 = -200f;
				}
			}
			if (num6 > 350f)
			{
				num = 5f;
				num2 = 0.3f;
			}
			else if (num6 > 300f)
			{
				num = 3f;
				num2 = 0.2f;
			}
			else if (num6 > 250f)
			{
				num = 1.5f;
				num2 = 0.1f;
			}
			num5 = num / num5;
			num3 *= num5;
			num4 *= num5;
			if (Main.PlayerSet[Target].IsDead)
			{
				num3 = Direction * num * 0.5f;
				num4 = num * -0.5f;
			}
			if (Velocity.X < num3)
			{
				Velocity.X += num2;
			}
			else if (Velocity.X > num3)
			{
				Velocity.X -= num2;
			}
			if (Velocity.Y < num4)
			{
				Velocity.Y += num2;
			}
			else if (Velocity.Y > num4)
			{
				Velocity.Y -= num2;
			}
			if (num3 > 0f)
			{
				SpriteDirection = -1;
				Rotation = (float)Math.Atan2(num4, num3);
			}
			else if (num3 < 0f)
			{
				SpriteDirection = 1;
				Rotation = (float)Math.Atan2(num4, num3) + 3.14f;
			}
		}

		private unsafe void SkeletronAI()
		{
			if (AI0 == 0f && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				TargetClosest();
				AI0 = 1f;
				if (Type != (int)ID.DUNGEON_GUARDIAN)
				{
					int num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.SKELETRON_HAND, WhoAmI);
					Main.NPCSet[num].AI0 = -1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].ShouldNetUpdate = true;
					num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.SKELETRON_HAND, WhoAmI);
					Main.NPCSet[num].AI0 = 1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].AI3 = 150f;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].ShouldNetUpdate = true;
				}
			}
			if (Type == (int)ID.DUNGEON_GUARDIAN && AI1 != 3f && AI1 != 2f)
			{
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				AI1 = 2f;
			}
			if (Main.PlayerSet[Target].IsDead || Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) > 2000 || Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 2000)
			{
				TargetClosest();
				if (Main.PlayerSet[Target].IsDead || Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) > 2000 || Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 2000)
				{
					AI1 = 3f;
				}
			}
			if (Main.GameTime.DayTime && AI1 != 3f && AI1 != 2f)
			{
				AI1 = 2f;
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
			}
			if (AI1 == 0f)
			{
				Defense = 10;
				AI2 += 1f;
				if (AI2 >= 800f)
				{
					AI2 = 0f;
					AI1 = 1f;
					TargetClosest();
					ShouldNetUpdate = true;
				}
				Rotation = Velocity.X * (71f / (339f * (float)Math.PI));
				if (XYWH.Y > Main.PlayerSet[Target].XYWH.Y - 250)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.98f;
					}
					Velocity.Y -= 0.02f;
					if (Velocity.Y > 2f)
					{
						Velocity.Y = 2f;
					}
				}
				else if (XYWH.Y < Main.PlayerSet[Target].XYWH.Y - 250)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.98f;
					}
					Velocity.Y += 0.02f;
					if (Velocity.Y < -2f)
					{
						Velocity.Y = -2f;
					}
				}
				if (XYWH.X + (Width >> 1) > Main.PlayerSet[Target].XYWH.X + 10)
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.98f;
					}
					Velocity.X -= 0.05f;
					if (Velocity.X > 8f)
					{
						Velocity.X = 8f;
					}
				}
				else if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 10)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.98f;
					}
					Velocity.X += 0.05f;
					if (Velocity.X < -8f)
					{
						Velocity.X = -8f;
					}
				}
			}
			else if (AI1 == 1f)
			{
				Defense = 0;
				AI2 += 1f;
				if (AI2 == 2f)
				{
					Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				}
				if (AI2 >= 400f)
				{
					AI2 = 0f;
					AI1 = 0f;
				}
				Rotation += Direction * 0.3f;
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				num4 = 1.5f / num4;
				Velocity.X = num2 * num4;
				Velocity.Y = num3 * num4;
			}
			else if (AI1 == 2f)
			{
				Damage = 9999;
				Defense = 9999;
				Rotation += Direction * 0.3f;
				Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num5 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
				float num6 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
				float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
				num7 = 8f / num7;
				Velocity.X = num5 * num7;
				Velocity.Y = num6 * num7;
			}
			else if (AI1 == 3f)
			{
				Velocity.Y += 0.1f;
				if (Velocity.Y < 0f)
				{
					Velocity.Y *= 0.95f;
				}
				Velocity.X *= 0.95f;
				if (TimeLeft > 500)
				{
					TimeLeft = 500;
				}
			}
			if (AI1 == 2f || AI1 == 3f || Type == (int)ID.DUNGEON_GUARDIAN)
			{
				return;
			}
			Dust* ptr = Main.DustSet.NewDust(XYWH.X + (Width >> 1) - 15 - (int)(Velocity.X * 5f), XYWH.Y + Height - 2, 30, 10, 5, Velocity.X * -0.2f, 3.0, 0, default(Color), 2.0);
			if (ptr != null)
			{
				ptr->NoGravity = true;
				ptr->Velocity.X *= 1.3f;
				ptr->Velocity.X += Velocity.X * 0.4f;
				ptr->Velocity.Y += 2f + Velocity.Y;
			}
			for (int i = 0; i < 2; i++)
			{
				ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 120, Width, 60, 5, Velocity.X, Velocity.Y, 0, default(Color), 2.0);
				if (ptr == null)
				{
					break;
				}
				ptr->NoGravity = true;
				ptr->Velocity -= Velocity;
				ptr->Velocity.Y += 5f;
			}
		}

		private void SkeletronHandAI()
		{
			SpriteDirection = (sbyte)(0f - AI0);
			if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != 11)
			{
				AI2 += 10f;
				if (AI2 > 50f || Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					Life = -1;
					HitEffect();
					Active = 0;
					return;
				}
			}
			if (AI2 == 0f || AI2 == 3f)
			{
				if (Main.NPCSet[(int)AI1].AI1 == 3f && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Main.NPCSet[(int)AI1].AI1 != 0f)
				{
					if (XYWH.Y > Main.NPCSet[(int)AI1].XYWH.Y - 100)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.07f;
						if (Velocity.Y > 6f)
						{
							Velocity.Y = 6f;
						}
					}
					else if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y - 100)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.07f;
						if (Velocity.Y < -6f)
						{
							Velocity.Y = -6f;
						}
					}
					if (XYWH.X + (Width >> 1) > Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120 * (int)AI0)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.1f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					else if (XYWH.X + (Width >> 1) < Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120 * (int)AI0)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.1f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				else
				{
					AI3 += 1f;
					if (AI3 >= 300f)
					{
						AI2 += 1f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
					if (XYWH.Y > Main.NPCSet[(int)AI1].XYWH.Y + 230)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
					}
					else if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y + 230)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.04f;
						if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					if (XYWH.X + (Width >> 1) > Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200 * (int)AI0)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.07f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					else if (XYWH.X + (Width >> 1) < Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200 * (int)AI0)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.07f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector.X;
				float num2 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector.Y;
				Rotation = (float)Math.Atan2(num2, num) + 1.57f;
			}
			else if (AI2 == 1f)
			{
				Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num3 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector2.X;
				float num4 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector2.Y;
				Rotation = (float)Math.Atan2(num4, num3) + 1.57f;
				Velocity.X *= 0.95f;
				Velocity.Y -= 0.1f;
				if (Velocity.Y < -8f)
				{
					Velocity.Y = -8f;
				}
				if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y - 200)
				{
					TargetClosest();
					AI2 = 2f;
					vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num3 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
					num4 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
					float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
					num5 = 18f / num5;
					Velocity.X = num3 * num5;
					Velocity.Y = num4 * num5;
					ShouldNetUpdate = true;
				}
			}
			else if (AI2 == 2f)
			{
				if (XYWH.Y > Main.PlayerSet[Target].XYWH.Y || Velocity.Y < 0f)
				{
					AI2 = 3f;
				}
			}
			else if (AI2 == 4f)
			{
				Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num6 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector3.X;
				float num7 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector3.Y;
				Rotation = (float)Math.Atan2(num7, num6) + 1.57f;
				Velocity.Y *= 0.95f;
				Velocity.X += 0.1f * (0f - AI0);
				if (Velocity.X < -8f)
				{
					Velocity.X = -8f;
				}
				if (Velocity.X > 8f)
				{
					Velocity.X = 8f;
				}
				if (XYWH.X + (Width >> 1) < Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 500 || XYWH.X + (Width >> 1) > Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) + 500)
				{
					TargetClosest();
					AI2 = 5f;
					vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num6 = Main.PlayerSet[Target].Position.X + 10f - vector3.X;
					num7 = Main.PlayerSet[Target].Position.Y + 21f - vector3.Y;
					float num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
					num8 = 17f / num8;
					Velocity.X = num6 * num8;
					Velocity.Y = num7 * num8;
					ShouldNetUpdate = true;
				}
			}
			else if (AI2 == 5f && ((Velocity.X > 0f && XYWH.X + (Width >> 1) > Main.PlayerSet[Target].XYWH.X + 10) || (Velocity.X < 0f && XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 10)))
			{
				AI2 = 0f;
			}
		}

		private void PlantAI()
		{
			if (Main.TileSet[(int)AI0, (int)AI1].IsActive == 0)
			{
				Life = -1;
				HitEffect();
				Active = 0;
				return;
			}
			TargetClosest();
			float num = 0.035f;
			float num2 = 150f;
			if (Type == (int)ID.MAN_EATER)
			{
				num2 = 250f;
			}
			if (Type == (int)ID.CLINGER)
			{
				num2 = 175f;
			}
			AI2 += 1f;
			if (AI2 > 300f)
			{
				num2 = (int)(num2 * 1.3f);
				if (AI2 > 450f)
				{
					AI2 = 0f;
				}
			}
			Vector2 vector = new Vector2(AI0 * 16f + 8f, AI1 * 16f + 8f);
			float num3 = Main.PlayerSet[Target].Position.X + 10f - (Width >> 1) - vector.X;
			float num4 = Main.PlayerSet[Target].Position.Y + 21f - (Height >> 1) - vector.Y;
			float num5 = num3 * num3 + num4 * num4;
			if (num5 > num2 * num2)
			{
				num5 = num2 / (float)Math.Sqrt(num5);
				num3 *= num5;
				num4 *= num5;
			}
			if (Position.X < AI0 * 16f + 8f + num3)
			{
				Velocity.X += num;
				if (Velocity.X < 0f && num3 > 0f)
				{
					Velocity.X += num * 1.5f;
				}
			}
			else if (Position.X > AI0 * 16f + 8f + num3)
			{
				Velocity.X -= num;
				if (Velocity.X > 0f && num3 < 0f)
				{
					Velocity.X -= num * 1.5f;
				}
			}
			if (Position.Y < AI1 * 16f + 8f + num4)
			{
				Velocity.Y += num;
				if (Velocity.Y < 0f && num4 > 0f)
				{
					Velocity.Y += num * 1.5f;
				}
			}
			else if (Position.Y > AI1 * 16f + 8f + num4)
			{
				Velocity.Y -= num;
				if (Velocity.Y > 0f && num4 < 0f)
				{
					Velocity.Y -= num * 1.5f;
				}
			}
			if (Type == (int)ID.MAN_EATER)
			{
				if (Velocity.X > 3f)
				{
					Velocity.X = 3f;
				}
				else if (Velocity.X < -3f)
				{
					Velocity.X = -3f;
				}
				if (Velocity.Y > 3f)
				{
					Velocity.Y = 3f;
				}
				else if (Velocity.Y < -3f)
				{
					Velocity.Y = -3f;
				}
			}
			else
			{
				if (Velocity.X > 2f)
				{
					Velocity.X = 2f;
				}
				else if (Velocity.X < -2f)
				{
					Velocity.X = -2f;
				}
				if (Velocity.Y > 2f)
				{
					Velocity.Y = 2f;
				}
				else if (Velocity.Y < -2f)
				{
					Velocity.Y = -2f;
				}
			}
			if (num3 > 0f)
			{
				SpriteDirection = 1;
				Rotation = (float)Math.Atan2(num4, num3);
			}
			else if (num3 < 0f)
			{
				SpriteDirection = -1;
				Rotation = (float)Math.Atan2(num4, num3) + 3.14f;
			}
			if (HasXCollision)
			{
				ShouldNetUpdate = true;
				Velocity.X = OldVelocity.X * -0.7f;
				if (Velocity.X > 0f && Velocity.X < 2f)
				{
					Velocity.X = 2f;
				}
				else if (Velocity.X < 0f && Velocity.X > -2f)
				{
					Velocity.X = -2f;
				}
			}
			if (HasYCollision)
			{
				ShouldNetUpdate = true;
				Velocity.Y = OldVelocity.Y * -0.7f;
				if (Velocity.Y > 0f && Velocity.Y < 2f)
				{
					Velocity.Y = 2f;
				}
				else if (Velocity.Y < 0f && Velocity.Y > -2f)
				{
					Velocity.Y = -2f;
				}
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT || Type != (int)ID.CLINGER || Main.PlayerSet[Target].IsDead)
			{
				return;
			}
			if (WasJustHit)
			{
				LocalAI0 = 0;
			}
			if (++LocalAI0 < 120)
			{
				return;
			}
			if (!Collision.SolidCollision(ref Position, Width, Height) && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
			{
				vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				num3 = Main.PlayerSet[Target].Position.X + 10f - vector.X + Main.Rand.Next(-10, 11);
				num4 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y + Main.Rand.Next(-10, 11);
				num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
				num5 = 10f / num5;
				num3 *= num5;
				num4 *= num5;
				int num6 = Projectile.NewProjectile(vector.X, vector.Y, num3, num4, 96, 22, 0f);
				if (num6 >= 0)
				{
					Main.ProjectileSet[num6].timeLeft = 300;
				}
				LocalAI0 = 0;
			}
			else
			{
				LocalAI0 = 100;
			}
		}

		private unsafe void FlyerAI()
		{
			if (Type == (int)ID.HELLBAT)
			{
				Dust* ptr = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * 0.2f, Velocity.Y * 0.2f, 100, default(Color), 2.0);
				if (ptr != null)
				{
					ptr->NoGravity = true;
				}
			}
			HasNoGravity = true;
			if (HasXCollision)
			{
				Velocity.X = OldVelocity.X * -0.5f;
				if (Direction == -1 && Velocity.X > 0f && Velocity.X < 2f)
				{
					Velocity.X = 2f;
				}
				else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -2f)
				{
					Velocity.X = -2f;
				}
			}
			if (HasYCollision)
			{
				Velocity.Y = OldVelocity.Y * -0.5f;
				if (Velocity.Y > 0f && Velocity.Y < 1f)
				{
					Velocity.Y = 1f;
				}
				else if (Velocity.Y < 0f && Velocity.Y > -1f)
				{
					Velocity.Y = -1f;
				}
			}
			TargetClosest();
			if (Direction == -1 && Velocity.X > -4f)
			{
				Velocity.X -= 0.1f;
				if (Velocity.X > 4f)
				{
					Velocity.X -= 0.1f;
				}
				else if (Velocity.X > 0f)
				{
					Velocity.X += 0.05f;
				}
				else if (Velocity.X < -4f)
				{
					Velocity.X = -4f;
				}
			}
			else if (Direction == 1 && Velocity.X < 4f)
			{
				Velocity.X += 0.1f;
				if (Velocity.X < -4f)
				{
					Velocity.X += 0.1f;
				}
				else if (Velocity.X < 0f)
				{
					Velocity.X -= 0.05f;
				}
				else if (Velocity.X > 4f)
				{
					Velocity.X = 4f;
				}
			}
			if (DirectionY == -1 && Velocity.Y > -1.5)
			{
				Velocity.Y -= 0.04f;
				if (Velocity.Y > 1.5)
				{
					Velocity.Y -= 0.05f;
				}
				else if (Velocity.Y > 0f)
				{
					Velocity.Y += 0.03f;
				}
				else if (Velocity.Y < -1.5)
				{
					Velocity.Y = -1.5f;
				}
			}
			else if (DirectionY == 1 && Velocity.Y < 1.5)
			{
				Velocity.Y += 0.04f;
				if (Velocity.Y < -1.5)
				{
					Velocity.Y += 0.05f;
				}
				else if (Velocity.Y < 0f)
				{
					Velocity.Y -= 0.03f;
				}
				else if (Velocity.Y > 1.5)
				{
					Velocity.Y = 1.5f;
				}
			}
			if (Type == (int)ID.CAVE_BAT || Type == (int)ID.JUNGLE_BAT || Type == (int)ID.HELLBAT || Type == (int)ID.DEMON || Type == (int)ID.ARCH_DEMON || Type == (int)ID.VOODOO_DEMON || Type == (int)ID.GIANT_BAT || Type == (int)ID.ILLUMINANT_BAT)
			{
				if (IsWet)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.95f;
					}
					Velocity.Y -= 0.5f;
					if (Velocity.Y < -4f)
					{
						Velocity.Y = -4f;
					}
					TargetClosest();
				}
				if (Type == (int)ID.HELLBAT)
				{
					if (Direction == -1 && Velocity.X > -4f)
					{
						Velocity.X -= 0.1f;
						if (Velocity.X > 4f)
						{
							Velocity.X -= 0.07f;
						}
						else if (Velocity.X > 0f)
						{
							Velocity.X += 0.03f;
						}
						if (Velocity.X < -4f)
						{
							Velocity.X = -4f;
						}
					}
					else if (Direction == 1 && Velocity.X < 4f)
					{
						Velocity.X += 0.1f;
						if (Velocity.X < -4f)
						{
							Velocity.X += 0.07f;
						}
						else if (Velocity.X < 0f)
						{
							Velocity.X -= 0.03f;
						}
						if (Velocity.X > 4f)
						{
							Velocity.X = 4f;
						}
					}
					if (DirectionY == -1 && Velocity.Y > -1.5)
					{
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 1.5)
						{
							Velocity.Y -= 0.03f;
						}
						else if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.02f;
						}
						if (Velocity.Y < -1.5)
						{
							Velocity.Y = -1.5f;
						}
					}
					else if (DirectionY == 1 && Velocity.Y < 1.5)
					{
						Velocity.Y += 0.04f;
						if (Velocity.Y < -1.5)
						{
							Velocity.Y += 0.03f;
						}
						else if (Velocity.Y < 0f)
						{
							Velocity.Y -= 0.02f;
						}
						if (Velocity.Y > 1.5)
						{
							Velocity.Y = 1.5f;
						}
					}
				}
				else
				{
					if (Direction == -1 && Velocity.X > -4f)
					{
						Velocity.X -= 0.1f;
						if (Velocity.X > 4f)
						{
							Velocity.X -= 0.1f;
						}
						else if (Velocity.X > 0f)
						{
							Velocity.X += 0.05f;
						}
						if (Velocity.X < -4f)
						{
							Velocity.X = -4f;
						}
					}
					else if (Direction == 1 && Velocity.X < 4f)
					{
						Velocity.X += 0.1f;
						if (Velocity.X < -4f)
						{
							Velocity.X += 0.1f;
						}
						else if (Velocity.X < 0f)
						{
							Velocity.X -= 0.05f;
						}
						if (Velocity.X > 4f)
						{
							Velocity.X = 4f;
						}
					}
					if (DirectionY == -1 && Velocity.Y > -1.5)
					{
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 1.5)
						{
							Velocity.Y -= 0.05f;
						}
						else if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.03f;
						}
						if (Velocity.Y < -1.5)
						{
							Velocity.Y = -1.5f;
						}
					}
					else if (DirectionY == 1 && Velocity.Y < 1.5)
					{
						Velocity.Y += 0.04f;
						if (Velocity.Y < -1.5)
						{
							Velocity.Y += 0.05f;
						}
						else if (Velocity.Y < 0f)
						{
							Velocity.Y -= 0.03f;
						}
						if (Velocity.Y > 1.5)
						{
							Velocity.Y = 1.5f;
						}
					}
				}
			}
			AI1 += 1f;
			if (AI1 > 200f)
			{
				if (!Main.PlayerSet[Target].IsWet && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
				{
					AI1 = 0f;
				}
				float num = 0.2f;
				float num2 = 0.1f;
				float num3 = 4f;
				float num4 = 1.5f;
				if (Type == (int)ID.HARPY || Type == (int)ID.DEMON || Type == (int)ID.ARCH_DEMON || Type == (int)ID.VOODOO_DEMON)
				{
					num = 0.12f;
					num2 = 0.07f;
					num3 = 3f;
					num4 = 1.25f;
				}
				if (AI1 > 1000f)
				{
					AI1 = 0f;
				}
				AI2 += 1f;
				if (AI2 > 0f)
				{
					if (Velocity.Y < num4)
					{
						Velocity.Y += num2;
					}
				}
				else if (Velocity.Y > 0f - num4)
				{
					Velocity.Y -= num2;
				}
				if (AI2 < -150f || AI2 > 150f)
				{
					if (Velocity.X < num3)
					{
						Velocity.X += num;
					}
				}
				else if (Velocity.X > 0f - num3)
				{
					Velocity.X -= num;
				}
				if (AI2 > 300f)
				{
					AI2 = -300f;
				}
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			if (Type == (int)ID.HARPY)
			{
				AI0 += 1f;
				if (AI0 == 30f || AI0 == 60f || AI0 == 90f)
				{
					if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num5 = Main.PlayerSet[Target].Position.X + 10f - vector.X + Main.Rand.Next(-100, 101);
						float num6 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y + Main.Rand.Next(-100, 101);
						float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
						num7 = 6f / num7;
						num5 *= num7;
						num6 *= num7;
						int num8 = Projectile.NewProjectile(vector.X, vector.Y, num5, num6, 38, 15, 0f);
						if (num8 >= 0)
						{
							Main.ProjectileSet[num8].timeLeft = 300;
						}
					}
				}
				else if (AI0 >= 400 + Main.Rand.Next(400))
				{
					AI0 = 0f;
				}
			}
			else
			{
				if (Type != (int)ID.DEMON && Type != (int)ID.ARCH_DEMON && Type != (int)ID.VOODOO_DEMON)
				{
					return;
				}
				AI0 += 1f;
				if (AI0 == 20f || AI0 == 40f || AI0 == 60f || AI0 == 80f)
				{
					if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num9 = Main.PlayerSet[Target].Position.X + 10f - vector2.X + Main.Rand.Next(-100, 101);
						float num10 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y + Main.Rand.Next(-100, 101);
						float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
						num11 = 0.2f / num11;
						num9 *= num11;
						num10 *= num11;
						int num12 = Projectile.NewProjectile(vector2.X, vector2.Y, num9, num10, 44, 21, 0f);
						if (num12 >= 0)
						{
							Main.ProjectileSet[num12].timeLeft = 300;
						}
					}
				}
				else if (AI0 >= 300 + Main.Rand.Next(300))
				{
					AI0 = 0f;
				}
			}
		}

		private unsafe void KingSlimeAI()
		{
			AIAction = 0;
			if (AI3 == 0f && Life > 0)
			{
				AI3 = LifeMax;
			}
			if (AI2 == 0f)
			{
				AI0 = -100f;
				AI2 = 1f;
				TargetClosest();
			}
			if (Velocity.Y == 0f)
			{
				Velocity.X *= 0.8f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				double num = Life / (double)LifeMax;
				if (num < 0.1)
				{
					AI0 += 13f;
				}
				else if (num < 0.2)
				{
					AI0 += 9f;
				}
				else if (num < 0.4)
				{
					AI0 += 6f;
				}
				else if (num < 0.6)
				{
					AI0 += 4f;
				}
				else if (num < 0.8)
				{
					AI0 += 3f;
				}
				else
				{
					AI0 += 2f;
				}
				if (AI0 >= 0f)
				{
					ShouldNetUpdate = true;
					TargetClosest();
					if (AI1 == 3f)
					{
						Velocity.Y = -13f;
						Velocity.X += 3.5f * Direction;
						AI0 = -200f;
						AI1 = 0f;
					}
					else if (AI1 == 2f)
					{
						Velocity.Y = -6f;
						Velocity.X += 4.5f * Direction;
						AI0 = -120f;
						AI1 += 1f;
					}
					else
					{
						Velocity.Y = -8f;
						Velocity.X += 4f * Direction;
						AI0 = -120f;
						AI1 += 1f;
					}
				}
				else if (AI0 >= -30f)
				{
					AIAction = 1;
				}
			}
			else if (Target < 8 && ((Direction == 1 && Velocity.X < 3f) || (Direction == -1 && Velocity.X > -3f)))
			{
				if ((Direction == -1 && Velocity.X < 0.1) || (Direction == 1 && Velocity.X > -0.1))
				{
					Velocity.X += 0.2f * Direction;
				}
				else
				{
					Velocity.X *= 0.93f;
				}
			}
			Dust* ptr = Main.DustSet.NewDust(4, ref XYWH, Velocity.X, Velocity.Y, 255, new Color(0, 80, 255, 80), Scale * 1.2f);
			if (ptr != null)
			{
				ptr->NoGravity = true;
				ptr->Velocity.X *= 0.5f;
				ptr->Velocity.Y *= 0.5f;
			}
			if (Life <= 0)
			{
				return;
			}
			float num2 = Life / (float)LifeMax;
			num2 = num2 * 0.5f + 0.75f;
			if (num2 != Scale)
			{
				Position.X += Width >> 1;
				Position.Y += Height;
				Scale = num2;
				Width = (ushort)(98f * Scale);
				Height = (ushort)(92f * Scale);
				Position.X -= Width >> 1;
				Position.Y -= Height;
				XYWH.X = (int)Position.X;
				XYWH.Y = (int)Position.Y;
				XYWH.Width = Width;
				XYWH.Height = Height;
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			int num3 = (int)(LifeMax * 0.05);
			if (!(Life + num3 < AI3))
			{
				return;
			}
			AI3 = Life;
			int num4 = Main.Rand.Next(1, 4);
			for (int i = 0; i < num4; i++)
			{
				int x = XYWH.X + Main.Rand.Next(Width - 32);
				int y = XYWH.Y + Main.Rand.Next(Height - 32);
				int num5 = NewNPC(x, y, (int)ID.SLIME);
				if (num5 < MaxNumNPCs)
				{
					Main.NPCSet[num5].SetDefaults((int)ID.SLIME);
					Main.NPCSet[num5].Velocity.X = Main.Rand.Next(-15, 16) * 0.1f;
					Main.NPCSet[num5].Velocity.Y = Main.Rand.Next(-30, 1) * 0.1f;
					Main.NPCSet[num5].AI1 = Main.Rand.Next(3);
					NetMessage.CreateMessage1(23, num5);
					NetMessage.SendMessage();
				}
			}
		}

		private void FishAI()
		{
			if (Direction == 0)
			{
				TargetClosest();
			}
			if (IsWet)
			{
				bool flag = false;
				if (Type != (int)ID.GOLDFISH)
				{
					TargetClosest(ShouldFaceTarget: false);
					if (Main.PlayerSet[Target].IsWet && !Main.PlayerSet[Target].IsDead)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (HasXCollision)
					{
						Velocity.X = 0f - Velocity.X;
						Direction = (sbyte)(-Direction);
						ShouldNetUpdate = true;
					}
					if (HasYCollision)
					{
						ShouldNetUpdate = true;
						Velocity.Y = 0f - Velocity.Y;
						if (Velocity.Y < 0f)
						{
							DirectionY = -1;
							AI0 = -1f;
						}
						else if (Velocity.Y > 0f)
						{
							DirectionY = 1;
							AI0 = 1f;
						}
					}
				}
				if (Type == (int)ID.ANGLER_FISH)
				{
					Lighting.AddLight(XYWH.X + (Width >> 1) + Direction * (Width + 8) >> 4, XYWH.Y + 2 >> 4, new Vector3(0.07f, 0.04f, 0.025f));
				}
				if (flag)
				{
					TargetClosest();
					if (Type == (int)ID.SHARK || Type == (int)ID.ANGLER_FISH || Type == (int)ID.ORKA)
					{
						Velocity.X += Direction * 0.15f;
						Velocity.Y += DirectionY * 0.15f;
						if (Velocity.X > 5f)
						{
							Velocity.X = 5f;
						}
						else if (Velocity.X < -5f)
						{
							Velocity.X = -5f;
						}
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
						else if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					else
					{
						Velocity.X += Direction * 0.1f;
						Velocity.Y += DirectionY * 0.1f;
						if (Velocity.X > 3f)
						{
							Velocity.X = 3f;
						}
						else if (Velocity.X < -3f)
						{
							Velocity.X = -3f;
						}
						if (Velocity.Y > 2f)
						{
							Velocity.Y = 2f;
						}
						else if (Velocity.Y < -2f)
						{
							Velocity.Y = -2f;
						}
					}
				}
				else
				{
					Velocity.X += Direction * 0.1f;
					if (Velocity.X < -1f || Velocity.X > 1f)
					{
						Velocity.X *= 0.95f;
					}
					if (AI0 == -1f)
					{
						Velocity.Y -= 0.01f;
						if (Velocity.Y < -0.3)
						{
							AI0 = 1f;
						}
					}
					else
					{
						Velocity.Y += 0.01f;
						if (Velocity.Y > 0.3)
						{
							AI0 = -1f;
						}
					}
					int num = XYWH.X + (Width >> 1) >> 4;
					int num2 = XYWH.Y + (Height >> 1) >> 4;
					if (Main.TileSet[num, num2 - 1].Liquid > 128)
					{
						if (Main.TileSet[num, num2 + 1].IsActive != 0)
						{
							AI0 = -1f;
						}
						else if (Main.TileSet[num, num2 + 2].IsActive != 0)
						{
							AI0 = -1f;
						}
					}
					if (Velocity.Y > 0.4 || Velocity.Y < -0.4)
					{
						Velocity.Y *= 0.95f;
					}
				}
			}
			else
			{
				if (Velocity.Y == 0f)
				{
					if (Type == (int)ID.SHARK || Type == (int)ID.ORKA)
					{
						Velocity.X *= 0.94f;
						if (Velocity.X > -0.2f && Velocity.X < 0.2f)
						{
							Velocity.X = 0f;
						}
					}
					else if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						Velocity.Y = Main.Rand.Next(-50, -20) * 0.1f;
						Velocity.X = Main.Rand.Next(-20, 20) * 0.1f;
						ShouldNetUpdate = true;
					}
				}
				Velocity.Y += 0.3f;
				if (Velocity.Y > 10f)
				{
					Velocity.Y = 10f;
				}
				AI0 = 1f;
			}
			Rotation = Velocity.Y * Direction * 0.1f;
			if (Rotation < -0.2f)
			{
				Rotation = -0.2f;
			}
			else if (Rotation > 0.2f)
			{
				Rotation = 0.2f;
			}
		}

		private void VultureAI()
		{
			HasNoGravity = true;
			if (AI0 == 0f)
			{
				HasNoGravity = false;
				TargetClosest();
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (Velocity.X != 0f || Velocity.Y < 0f || Velocity.Y > 0.3f)
					{
						AI0 = 1f;
						ShouldNetUpdate = true;
					}
					else if (Life < LifeMax || Main.PlayerSet[Target].XYWH.Intersects(new Rectangle(XYWH.X - 100, XYWH.Y - 100, Width + 200, Height + 200)))
					{
						AI0 = 1f;
						Velocity.Y -= 6f;
						ShouldNetUpdate = true;
					}
				}
			}
			else if (!Main.PlayerSet[Target].IsDead)
			{
				if (HasXCollision)
				{
					Velocity.X = OldVelocity.X * -0.5f;
					if (Direction == -1 && Velocity.X > 0f && Velocity.X < 2f)
					{
						Velocity.X = 2f;
					}
					else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -2f)
					{
						Velocity.X = -2f;
					}
				}
				if (HasYCollision)
				{
					Velocity.Y = OldVelocity.Y * -0.5f;
					if (Velocity.Y > 0f && Velocity.Y < 1f)
					{
						Velocity.Y = 1f;
					}
					else if (Velocity.Y < 0f && Velocity.Y > -1f)
					{
						Velocity.Y = -1f;
					}
				}
				TargetClosest();
				if (Direction == -1 && Velocity.X > -3f)
				{
					Velocity.X -= 0.1f;
					if (Velocity.X > 3f)
					{
						Velocity.X -= 0.1f;
					}
					else if (Velocity.X > 0f)
					{
						Velocity.X -= 0.05f;
					}
					else if (Velocity.X < -3f)
					{
						Velocity.X = -3f;
					}
				}
				else if (Direction == 1 && Velocity.X < 3f)
				{
					Velocity.X += 0.1f;
					if (Velocity.X < -3f)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X < 0f)
					{
						Velocity.X += 0.05f;
					}
					else if (Velocity.X > 3f)
					{
						Velocity.X = 3f;
					}
				}
				int num = Math.Abs(XYWH.X + (Width >> 1) - (Main.PlayerSet[Target].XYWH.X + 10));
				int num2 = Main.PlayerSet[Target].XYWH.Y - (Height >> 1);
				if (num > 50)
				{
					num2 -= 100;
				}
				if (XYWH.Y < num2)
				{
					Velocity.Y += 0.05f;
					if (Velocity.Y < 0f)
					{
						Velocity.Y += 0.01f;
					}
				}
				else
				{
					Velocity.Y -= 0.05f;
					if (Velocity.Y > 0f)
					{
						Velocity.Y -= 0.01f;
					}
				}
				if (Velocity.Y < -3f)
				{
					Velocity.Y = -3f;
				}
				if (Velocity.Y > 3f)
				{
					Velocity.Y = 3f;
				}
			}
			if (IsWet)
			{
				if (Velocity.Y > 0f)
				{
					Velocity.Y *= 0.95f;
				}
				Velocity.Y -= 0.5f;
				if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
				TargetClosest();
			}
		}

		private void JellyfishAI()
		{
			Lighting.AddLight(RGB: (Type == (int)ID.BLUE_JELLYFISH) ? new Vector3(0.05f, 0.15f, 0.4f) : ((Type == (int)ID.GREEN_JELLYFISH) ? new Vector3(0.05f, 0.45f, 0.1f) : new Vector3(0.35f, 0.05f, 0.2f)), LightX: XYWH.X + (Height >> 1) >> 4, LightY: XYWH.Y + (Height >> 1) >> 4);
			if (Direction == 0)
			{
				TargetClosest();
			}
			if (IsWet)
			{
				if (HasXCollision)
				{
					Velocity.X = 0f - Velocity.X;
					Direction = (sbyte)(-Direction);
				}
				if (HasYCollision)
				{
					Velocity.Y = 0f - Velocity.Y;
					if (Velocity.Y < 0f)
					{
						DirectionY = -1;
						AI0 = -1f;
					}
					else if (Velocity.Y > 0f)
					{
						DirectionY = 1;
						AI0 = 1f;
					}
				}
				bool flag = false;
				if (!IsFriendly)
				{
					TargetClosest(ShouldFaceTarget: false);
					if (Main.PlayerSet[Target].IsWet && !Main.PlayerSet[Target].IsDead)
					{
						flag = true;
					}
				}
				if (flag)
				{
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 1.57f;
					Velocity.X *= 0.98f;
					Velocity.Y *= 0.98f;
					float num = 0.2f;
					if (Type == (int)ID.GREEN_JELLYFISH)
					{
						Velocity.X *= 0.98f;
						Velocity.Y *= 0.98f;
						num = 0.6f;
					}
					if (Velocity.X > 0f - num && Velocity.X < num && Velocity.Y > 0f - num && Velocity.Y < num)
					{
						TargetClosest();
						float num2 = ((Type == (int)ID.GREEN_JELLYFISH) ? 9 : 7);
						Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num3 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
						float num4 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
						float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
						num5 = num2 / num5;
						num3 *= num5;
						num4 *= num5;
						Velocity.X = num3;
						Velocity.Y = num4;
					}
					return;
				}
				Velocity.X += Direction * 0.02f;
				Rotation = Velocity.X * 0.4f;
				if (Velocity.X < -1f || Velocity.X > 1f)
				{
					Velocity.X *= 0.95f;
				}
				if (AI0 == -1f)
				{
					Velocity.Y -= 0.01f;
					if (Velocity.Y < -1f)
					{
						AI0 = 1f;
					}
				}
				else
				{
					Velocity.Y += 0.01f;
					if (Velocity.Y > 1f)
					{
						AI0 = -1f;
					}
				}
				int num6 = XYWH.X + (Width >> 1) >> 4;
				int num7 = XYWH.Y + (Height >> 1) >> 4;
				if (Main.TileSet[num6, num7 - 1].Liquid > 128)
				{
					if (Main.TileSet[num6, num7 + 1].IsActive != 0)
					{
						AI0 = -1f;
					}
					else if (Main.TileSet[num6, num7 + 2].IsActive != 0)
					{
						AI0 = -1f;
					}
				}
				else
				{
					AI0 = 1f;
				}
				if (Velocity.Y > 1.2 || Velocity.Y < -1.2)
				{
					Velocity.Y *= 0.99f;
				}
				return;
			}
			Rotation += Velocity.X * 0.1f;
			if (Velocity.Y == 0f)
			{
				Velocity.X *= 0.98f;
				if (Velocity.X > -0.01 && Velocity.X < 0.01)
				{
					Velocity.X = 0f;
				}
			}
			Velocity.Y += 0.2f;
			if (Velocity.Y > 10f)
			{
				Velocity.Y = 10f;
			}
			AI0 = 1f;
		}

		private unsafe void AntlionAI()
		{
			TargetClosest();
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num = Main.PlayerSet[Target].Position.X + 10f - vector.X;
			float num2 = Main.PlayerSet[Target].Position.Y - vector.Y;
			float num3 = (float)Math.Sqrt(num * num + num2 * num2);
			num3 = 12f / num3;
			num *= num3;
			num2 *= num3;
			bool flag = false;
			if (DirectionY < 0)
			{
				Rotation = (float)(Math.Atan2(num2, num) + 1.57);
				flag = ((!(Rotation < -1.2) && !(Rotation > 1.2)) ? true : false);
				if (Rotation < -0.8)
				{
					Rotation = -0.8f;
				}
				else if (Rotation > 0.8)
				{
					Rotation = 0.8f;
				}
				if (Velocity.X != 0f)
				{
					Velocity.X *= 0.9f;
					if (Velocity.X > -0.1 || Velocity.X < 0.1)
					{
						ShouldNetUpdate = true;
						Velocity.X = 0f;
					}
				}
			}
			if (AI0 > 0f)
			{
				if (AI0 == 200f)
				{
					Main.PlaySound(2, XYWH.X, XYWH.Y, 5);
				}
				AI0 -= 1f;
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && flag && AI0 == 0f && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
			{
				AI0 = 200f;
				int num4 = 10;
				int num5 = 31;
				int num6 = Projectile.NewProjectile(vector.X, vector.Y, num, num2, num5, num4, 0f, 8, send: false);
				if (num6 >= 0)
				{
					Main.ProjectileSet[num6].ai0 = 2f;
					Main.ProjectileSet[num6].timeLeft = 300;
					Main.ProjectileSet[num6].friendly = false;
					NetMessage.SendProjectile(num6);
					ShouldNetUpdate = true;
				}
			}
			try
			{
				int num7 = XYWH.X >> 4;
				int num8 = XYWH.X + (Width >> 1) >> 4;
				int num9 = XYWH.X + Width >> 4;
				int num10 = XYWH.Y + Height >> 4;
				if ((Main.TileSet[num7, num10].IsActive != 0 && Main.TileSolid[Main.TileSet[num7, num10].Type]) || (Main.TileSet[num8, num10].IsActive != 0 && Main.TileSolid[Main.TileSet[num8, num10].Type]) || (Main.TileSet[num9, num10].IsActive != 0 && Main.TileSolid[Main.TileSet[num9, num10].Type]))
				{
					HasNoGravity = true;
					HasNoTileCollide = true;
					Velocity.Y = -0.2f;
					return;
				}
				HasNoGravity = false;
				HasNoTileCollide = false;
				if (Main.Rand.Next(3) != 0)
				{
					return;
				}
				Dust* ptr = Main.DustSet.NewDust(XYWH.X - 4, XYWH.Y + Height - 8, Width + 8, 24, 32, 0.0, Velocity.Y * 0.5f);
				if (ptr != null)
				{
					ptr->Velocity.X *= 0.4f;
					ptr->Velocity.Y *= -1f;
					if (Main.Rand.Next(2) == 0)
					{
						ptr->NoGravity = true;
						ptr->Scale += 0.2f;
					}
				}
			}
			catch
			{
			}
		}

		private void SpinningSpikeballAI()
		{
			if (AI0 == 0f)
			{
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					TargetClosest();
					Direction = (sbyte)(-Direction);
					DirectionY = (sbyte)(-DirectionY);
					Position.Y += (Height >> 1) + 8;
					AI1 = Position.X + (Width >> 1);
					AI2 = Position.Y + (Height >> 1);
					if (Direction == 0)
					{
						Direction = 1;
					}
					if (DirectionY == 0)
					{
						DirectionY = 1;
					}
					AI3 = 1f + Main.Rand.Next(15) * 0.1f;
					Velocity.Y = DirectionY * 6 * AI3;
					AI0 += 1f;
					ShouldNetUpdate = true;
				}
				else
				{
					AI1 = Position.X + (Width >> 1);
					AI2 = Position.Y + (Height >> 1);
				}
				return;
			}
			float num = 6f * AI3;
			float num2 = 0.2f * AI3;
			float num3 = num / num2 * 0.5f;
			if (AI0 >= 1f && AI0 < (int)num3)
			{
				Velocity.Y = DirectionY * num;
				AI0 += 1f;
				return;
			}
			if (AI0 >= (int)num3)
			{
				ShouldNetUpdate = true;
				Velocity.Y = 0f;
				DirectionY = (sbyte)(-DirectionY);
				Velocity.X = num * Direction;
				AI0 = -1f;
				return;
			}
			if (DirectionY > 0)
			{
				if (Velocity.Y >= num)
				{
					ShouldNetUpdate = true;
					DirectionY = (sbyte)(-DirectionY);
					Velocity.Y = num;
				}
			}
			else if (DirectionY < 0 && Velocity.Y <= 0f - num)
			{
				DirectionY = (sbyte)(-DirectionY);
				Velocity.Y = 0f - num;
			}
			if (Direction > 0)
			{
				if (Velocity.X >= num)
				{
					Direction = (sbyte)(-Direction);
					Velocity.X = num;
				}
			}
			else if (Direction < 0 && Velocity.X <= 0f - num)
			{
				Direction = (sbyte)(-Direction);
				Velocity.X = 0f - num;
			}
			Velocity.X += num2 * Direction;
			Velocity.Y += num2 * DirectionY;
		}

		private void GravityDiskAI()
		{
			if (AI0 == 0f)
			{
				TargetClosest();
				DirectionY = 1;
				AI0 = 1f;
			}
			int num = 6;
			if (AI1 == 0f)
			{
				Rotation += Direction * DirectionY * 0.13f;
				if (HasYCollision)
				{
					AI0 = 2f;
				}
				if (!HasYCollision && AI0 == 2f)
				{
					Direction = (sbyte)(-Direction);
					AI1 = 1f;
					AI0 = 1f;
				}
				if (HasXCollision)
				{
					DirectionY = (sbyte)(-DirectionY);
					AI1 = 1f;
				}
			}
			else
			{
				Rotation -= Direction * DirectionY * 0.13f;
				if (HasXCollision)
				{
					AI0 = 2f;
				}
				if (!HasXCollision && AI0 == 2f)
				{
					DirectionY = (sbyte)(-DirectionY);
					AI1 = 0f;
					AI0 = 1f;
				}
				if (HasYCollision)
				{
					Direction = (sbyte)(-Direction);
					AI1 = 0f;
				}
			}
			Velocity.X = num * Direction;
			Velocity.Y = num * DirectionY;
			float num2 = (270 - UI.MouseTextBrightness) * 0.0025f;
			Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(0.9f, 0.3f + num2, 0.2f));
		}

		private unsafe void MoreFlyerAI()
		{
			bool flag = false;
			if (WasJustHit)
			{
				AI2 = 0f;
			}
			if (AI2 >= 0f)
			{
				float num = 16f;
				bool flag2 = false;
				bool flag3 = false;
				if (Position.X > AI0 - num && Position.X < AI0 + num)
				{
					flag2 = true;
				}
				else if ((Velocity.X < 0f && Direction > 0) || (Velocity.X > 0f && Direction < 0))
				{
					flag2 = true;
				}
				num += 24f;
				if (Position.Y > AI1 - num && Position.Y < AI1 + num)
				{
					flag3 = true;
				}
				if (flag2 && flag3)
				{
					AI2 += 1f;
					if (AI2 >= 30f && num == 16f)
					{
						flag = true;
					}
					if (AI2 >= 60f)
					{
						AI2 = -200f;
						Direction = (sbyte)(-Direction);
						Velocity.X = 0f - Velocity.X;
						HasXCollision = false;
					}
				}
				else
				{
					AI0 = Position.X;
					AI1 = Position.Y;
					AI2 = 0f;
				}
				TargetClosest();
			}
			else
			{
				AI2 += 1f;
				if (Main.PlayerSet[Target].XYWH.X + 10 > XYWH.X + (Width >> 1))
				{
					Direction = -1;
				}
				else
				{
					Direction = 1;
				}
			}
			int num2 = (XYWH.X + (Width >> 1) >> 4) + (Direction << 1);
			int num3 = XYWH.Y + Height >> 4;
			bool flag4 = true;
			bool flag5 = false;
			int num4 = 3;
			if (Type == (int)ID.GASTROPOD || Type == (int)ID.SPECTRAL_GASTROPOD)
			{
				if (WasJustHit)
				{
					AI3 = 0f;
					LocalAI1 = 0;
				}
				float num5 = 7f;
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num6 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num7 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
				num8 = num5 / num8;
				num6 *= num8;
				num7 *= num8;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT && AI3 == 32f)
				{
					Projectile.NewProjectile(vector.X, vector.Y, num6, num7, 84, 25, 0f);
				}
				num4 = 8;
				if (AI3 > 0f)
				{
					AI3 += 1f;
					if (AI3 >= 64f)
					{
						AI3 = 0f;
					}
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT && AI3 == 0f)
				{
					LocalAI1++;
					if (LocalAI1 > 120 && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						LocalAI1 = 0;
						AI3 = 1f;
						ShouldNetUpdate = true;
					}
				}
			}
			else if (Type == (int)ID.PIXIE)
			{
				num4 = 4;
				if (Main.Rand.Next(7) == 0)
				{
					Dust* ptr = Main.DustSet.NewDust(55, ref XYWH, 0.0, 0.0, 200, Colour);
					if (ptr != null)
					{
						ptr->Velocity.X *= 0.3f;
						ptr->Velocity.Y *= 0.3f;
					}
				}
				if (Main.Rand.Next(42) == 0)
				{
					Main.PlaySound(27, XYWH.X, XYWH.Y);
				}
			}
			for (int i = num3; i < num3 + num4; i++)
			{
				if ((Main.TileSet[num2, i].IsActive != 0 && Main.TileSolid[Main.TileSet[num2, i].Type]) || Main.TileSet[num2, i].Liquid > 0)
				{
					if (i <= num3 + 1)
					{
						flag5 = true;
					}
					flag4 = false;
					break;
				}
			}
			if (flag)
			{
				flag5 = false;
				flag4 = true;
			}
			if (flag4)
			{
				if (Type == (int)ID.PIXIE)
				{
					Velocity.Y += 0.2f;
					if (Velocity.Y > 2f)
					{
						Velocity.Y = 2f;
					}
				}
				else
				{
					Velocity.Y += 0.1f;
					if (Velocity.Y > 3f)
					{
						Velocity.Y = 3f;
					}
				}
			}
			else
			{
				if (Type == (int)ID.PIXIE)
				{
					if ((DirectionY < 0 && Velocity.Y > 0f) || flag5)
					{
						Velocity.Y -= 0.2f;
					}
				}
				else if (DirectionY < 0 && Velocity.Y > 0f)
				{
					Velocity.Y -= 0.1f;
				}
				if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
			}
			if (Type == (int)ID.PIXIE && IsWet)
			{
				Velocity.Y -= 0.2f;
				if (Velocity.Y < -2f)
				{
					Velocity.Y = -2f;
				}
			}
			if (HasXCollision)
			{
				Velocity.X = OldVelocity.X * -0.4f;
				if (Direction == -1 && Velocity.X > 0f && Velocity.X < 1f)
				{
					Velocity.X = 1f;
				}
				else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -1f)
				{
					Velocity.X = -1f;
				}
			}
			if (HasYCollision)
			{
				Velocity.Y = OldVelocity.Y * -0.25f;
				if (Velocity.Y > 0f && Velocity.Y < 1f)
				{
					Velocity.Y = 1f;
				}
				else if (Velocity.Y < 0f && Velocity.Y > -1f)
				{
					Velocity.Y = -1f;
				}
			}
			float num9 = ((Type == (int)ID.PIXIE) ? 3 : 2);
			if (Direction == -1 && Velocity.X > 0f - num9)
			{
				Velocity.X -= 0.1f;
				if (Velocity.X > num9)
				{
					Velocity.X -= 0.1f;
				}
				else if (Velocity.X > 0f)
				{
					Velocity.X += 0.05f;
				}
				else if (Velocity.X < 0f - num9)
				{
					Velocity.X = 0f - num9;
				}
			}
			else if (Direction == 1 && Velocity.X < num9)
			{
				Velocity.X += 0.1f;
				if (Velocity.X < 0f - num9)
				{
					Velocity.X += 0.1f;
				}
				else if (Velocity.X < 0f)
				{
					Velocity.X -= 0.05f;
				}
				else if (Velocity.X > num9)
				{
					Velocity.X = num9;
				}
			}
			if (DirectionY == -1 && Velocity.Y > -1.5)
			{
				Velocity.Y -= 0.04f;
				if (Velocity.Y > 1.5)
				{
					Velocity.Y -= 0.05f;
				}
				else if (Velocity.Y > 0f)
				{
					Velocity.Y += 0.03f;
				}
				else if (Velocity.Y < -1.5)
				{
					Velocity.Y = -1.5f;
				}
			}
			else if (DirectionY == 1 && Velocity.Y < 1.5)
			{
				Velocity.Y += 0.04f;
				if (Velocity.Y < -1.5)
				{
					Velocity.Y += 0.05f;
				}
				else if (Velocity.Y < 0f)
				{
					Velocity.Y -= 0.03f;
				}
				else if (Velocity.Y > 1.5)
				{
					Velocity.Y = 1.5f;
				}
			}
			if (Type == (int)ID.GASTROPOD || Type == (int)ID.SPECTRAL_GASTROPOD)
			{
				Lighting.AddLight(XYWH.X >> 4, XYWH.Y >> 4, new Vector3(0.4f, 0f, 0.25f));
			}
		}

		private void EnchantedWeaponAI()
		{
			HasNoGravity = true;
			HasNoTileCollide = true;
			Vector3 rgb = new Vector3(0.05f, 0.2f, 0.3f);
			switch (Type)
			{
			case (int)ID.CURSED_HAMMER:
				rgb = new Vector3(0.2f, 0.05f, 0.3f);
				break;
			case (int)ID.SHADOW_HAMMER:
				rgb = new Vector3(0.3f, 0.05f, 0.2f);
				break;
			}
			Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, rgb);
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead)
			{
				TargetClosest();
			}
			if (AI0 == 0f)
			{
				float num = 9f;
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				num4 = num / num4;
				num2 *= num4;
				num3 *= num4;
				Velocity.X = num2;
				Velocity.Y = num3;
				Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 0.785f;
				AI0 = 1f;
				AI1 = 0f;
				return;
			}
			if (AI0 == 1f)
			{
				if (WasJustHit)
				{
					AI0 = 2f;
					AI1 = 0f;
				}
				Velocity.X *= 0.99f;
				Velocity.Y *= 0.99f;
				AI1 += 1f;
				if (AI1 >= 100f)
				{
					AI0 = 2f;
					AI1 = 0f;
					Velocity.X = 0f;
					Velocity.Y = 0f;
				}
				return;
			}
			if (WasJustHit)
			{
				AI0 = 2f;
				AI1 = 0f;
			}
			Velocity.X *= 0.96f;
			Velocity.Y *= 0.96f;
			AI1 += 1f;
			float num5 = AI1 / 120f;
			num5 = 0.1f + num5 * 0.4f;
			Rotation += num5 * Direction;
			if (AI1 >= 120f)
			{
				ShouldNetUpdate = true;
				AI0 = 0f;
				AI1 = 0f;
			}
		}

		private void BirdAI()
		{
			HasNoGravity = true;
			if (AI0 == 0f)
			{
				HasNoGravity = false;
				TargetClosest();
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (Velocity.X != 0f || Velocity.Y < 0f || Velocity.Y > 0.3)
					{
						AI0 = 1f;
						ShouldNetUpdate = true;
						Direction = (sbyte)(-Direction);
					}
					else if (Life < LifeMax || Main.PlayerSet[Target].XYWH.Intersects(new Rectangle(XYWH.X - 100, XYWH.Y - 100, Width + 200, Height + 200)))
					{
						AI0 = 1f;
						Velocity.Y -= 6f;
						ShouldNetUpdate = true;
						Direction = (sbyte)(-Direction);
					}
				}
			}
			else if (!Main.PlayerSet[Target].IsDead)
			{
				if (HasXCollision)
				{
					Direction = (sbyte)(-Direction);
					Velocity.X = OldVelocity.X * -0.5f;
					if (Direction == -1 && Velocity.X > 0f && Velocity.X < 2f)
					{
						Velocity.X = 2f;
					}
					else if (Direction == 1 && Velocity.X < 0f && Velocity.X > -2f)
					{
						Velocity.X = -2f;
					}
				}
				if (HasYCollision)
				{
					Velocity.Y = OldVelocity.Y * -0.5f;
					if (Velocity.Y > 0f && Velocity.Y < 1f)
					{
						Velocity.Y = 1f;
					}
					else if (Velocity.Y < 0f && Velocity.Y > -1f)
					{
						Velocity.Y = -1f;
					}
				}
				if (Direction == -1 && Velocity.X > -3f)
				{
					Velocity.X -= 0.1f;
					if (Velocity.X > 3f)
					{
						Velocity.X -= 0.1f;
					}
					else if (Velocity.X > 0f)
					{
						Velocity.X -= 0.05f;
					}
					else if (Velocity.X < -3f)
					{
						Velocity.X = -3f;
					}
				}
				else if (Direction == 1 && Velocity.X < 3f)
				{
					Velocity.X += 0.1f;
					if (Velocity.X < -3f)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X < 0f)
					{
						Velocity.X += 0.05f;
					}
					else if (Velocity.X > 3f)
					{
						Velocity.X = 3f;
					}
				}
				int num = (XYWH.X + (Width >> 1) >> 4) + Direction;
				int num2 = XYWH.Y + Height >> 4;
				bool flag = true;
				bool flag2 = false;
				try
				{
					for (int i = num2; i < num2 + 15; i++)
					{
						if ((Main.TileSet[num, i].IsActive != 0 && Main.TileSolid[Main.TileSet[num, i].Type]) || Main.TileSet[num, i].Liquid > 0)
						{
							if (i < num2 + 5)
							{
								flag2 = true;
							}
							flag = false;
							break;
						}
					}
				}
				catch
				{
					flag2 = true;
					flag = false;
				}
				if (flag)
				{
					Velocity.Y += 0.1f;
				}
				else
				{
					Velocity.Y -= 0.1f;
				}
				if (flag2)
				{
					Velocity.Y -= 0.2f;
				}
				if (Velocity.Y > 3f)
				{
					Velocity.Y = 3f;
				}
				else if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
			}
			if (IsWet)
			{
				if (Velocity.Y > 0f)
				{
					Velocity.Y *= 0.95f;
				}
				Velocity.Y -= 0.5f;
				if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
				TargetClosest();
			}
		}

		private void MimicAI()
		{
			if (AI3 == 0f)
			{
				Position.X += 8f;
				XYWH.X += 8;
				int num = XYWH.Y >> 4;
				if (num > Main.MaxTilesY - 200)
				{
					AI3 = 3f;
				}
				else if (num > Main.WorldSurface)
				{
					AI3 = 2f;
				}
				else
				{
					AI3 = 1f;
				}
			}
			if (AI0 == 0f)
			{
				TargetClosest();
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (Velocity.X != 0f || Velocity.Y < 0f || Velocity.Y > 0.3)
					{
						AI0 = 1f;
						ShouldNetUpdate = true;
					}
					else if (Life < LifeMax || new Rectangle(XYWH.X - 100, XYWH.Y - 100, Width + 200, Height + 200).Intersects(Main.PlayerSet[Target].XYWH))
					{
						AI0 = 1f;
						ShouldNetUpdate = true;
					}
				}
			}
			else if (Velocity.Y == 0f)
			{
				AI2 += 1f;
				int num2 = 20;
				if (AI1 == 0f)
				{
					num2 = 12;
				}
				if (AI2 < num2)
				{
					Velocity.X *= 0.9f;
					return;
				}
				AI2 = 0f;
				TargetClosest();
				SpriteDirection = Direction;
				AI1 += 1f;
				if (AI1 == 2f)
				{
					Velocity.X = Direction * 2.5f;
					Velocity.Y = -8f;
					AI1 = 0f;
				}
				else
				{
					Velocity.X = Direction * 3.5f;
					Velocity.Y = -4f;
				}
				ShouldNetUpdate = true;
			}
			else if (Direction == 1 && Velocity.X < 1f)
			{
				Velocity.X += 0.1f;
			}
			else if (Direction == -1 && Velocity.X > -1f)
			{
				Velocity.X -= 0.1f;
			}
		}

		private void UnicornAI()
		{
			int num = 30;
			bool flag = false;
			if (Velocity.Y == 0f && ((Velocity.X > 0f && Direction < 0) || (Velocity.X < 0f && Direction > 0)))
			{
				flag = true;
				AI3 += 1f;
			}
			if (Position.X == OldPosition.X || AI3 >= num || flag)
			{
				AI3 += 1f;
			}
			else if (AI3 > 0f)
			{
				AI3 -= 1f;
			}
			if (AI3 > num * 10)
			{
				AI3 = 0f;
			}
			if (WasJustHit)
			{
				AI3 = 0f;
			}
			if (AI3 == num)
			{
				ShouldNetUpdate = true;
			}
			if (AI3 < num)
			{
				TargetClosest();
			}
			else
			{
				if (Velocity.X == 0f)
				{
					if (Velocity.Y == 0f)
					{
						AI0 += 1f;
						if (AI0 >= 2f)
						{
							Direction = (sbyte)(-Direction);
							SpriteDirection = Direction;
							AI0 = 0f;
						}
					}
				}
				else
				{
					AI0 = 0f;
				}
				DirectionY = -1;
				if (Direction == 0)
				{
					Direction = 1;
				}
			}
			if (Velocity.Y == 0f || IsWet || (Velocity.X <= 0f && Direction < 0) || (Velocity.X >= 0f && Direction > 0))
			{
				if (Velocity.X < -6f || Velocity.X > 6f)
				{
					if (Velocity.Y == 0f)
					{
						Velocity.X *= 0.8f;
					}
				}
				else if (Velocity.X < 6f && Direction == 1)
				{
					Velocity.X += 0.07f;
					if (Velocity.X > 6f)
					{
						Velocity.X = 6f;
					}
				}
				else if (Velocity.X > -6f && Direction == -1)
				{
					Velocity.X -= 0.07f;
					if (Velocity.X < -6f)
					{
						Velocity.X = -6f;
					}
				}
			}
			if (Velocity.Y != 0f)
			{
				return;
			}
			int num2 = (int)(Position.X + Velocity.X * 5f) + (Width >> 1) + ((Width >> 1) + 2) * Direction >> 4;
			int num3 = XYWH.Y + Height - 15 >> 4;
			if ((!(Velocity.X < 0f) || SpriteDirection != -1) && (!(Velocity.X > 0f) || SpriteDirection != 1))
			{
				return;
			}
			if (Main.TileSet[num2, num3 - 2].IsActive != 0 && Main.TileSolid[Main.TileSet[num2, num3 - 2].Type])
			{
				if (Main.TileSet[num2, num3 - 3].IsActive != 0 && Main.TileSolid[Main.TileSet[num2, num3 - 3].Type])
				{
					Velocity.Y = -8.5f;
					ShouldNetUpdate = true;
				}
				else
				{
					Velocity.Y = -7.5f;
					ShouldNetUpdate = true;
				}
			}
			else if (Main.TileSet[num2, num3 - 1].IsActive != 0 && Main.TileSolid[Main.TileSet[num2, num3 - 1].Type])
			{
				Velocity.Y = -7f;
				ShouldNetUpdate = true;
			}
			else if (Main.TileSet[num2, num3].IsActive != 0 && Main.TileSolid[Main.TileSet[num2, num3].Type])
			{
				Velocity.Y = -6f;
				ShouldNetUpdate = true;
			}
			else if ((DirectionY < 0 || Math.Abs(Velocity.X) > 3f) && (Main.TileSet[num2, num3 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num2, num3 + 1].Type]) && (Main.TileSet[num2 + Direction, num3 + 1].IsActive == 0 || !Main.TileSolid[Main.TileSet[num2 + Direction, num3 + 1].Type]))
			{
				Velocity.Y = -8f;
				ShouldNetUpdate = true;
			}
		}
		private void WallOfFleshMouthAI()
		{
			if (XYWH.X < 160 || XYWH.X > (Main.MaxTilesX - 10) * 16)
			{
				Active = 0;
				return;
			}
			if (LocalAI0 == 0)
			{
				LocalAI0 = 1;
				WoFBottom = -1;
				WoFTop = -1;
			}
			AI1 += 1f;
			if (AI2 == 0f)
			{
				if (Life < LifeMax * 0.5)
				{
					AI1 += 1f;
				}
				if (Life < LifeMax * 0.2)
				{
					AI1 += 1f;
				}
				if (AI1 > 2700f)
				{
					AI2 = 1f;
				}
			}
			if (AI2 > 0f && AI1 > 60f)
			{
				int num = 3;
				if (Life < LifeMax * 0.3)
				{
					num++;
				}
				AI2 += 1f;
				AI1 = 0f;
				if (AI2 > num)
				{
					AI2 = 0f;
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					int num2 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1) + 20, (int)ID.LEECH_HEAD, 1);
					Main.NPCSet[num2].Velocity.X = Direction << 3;
				}
			}
			LocalAI3++;
			if (LocalAI3 >= 600 + Main.Rand.Next(1000))
			{
				LocalAI3 = -Main.Rand.Next(200);
				Main.PlaySound(4, XYWH.X, XYWH.Y, 10);
			}
			WoF = WhoAmI;
			int num3 = XYWH.X >> 4;
			int num4 = XYWH.X + Width >> 4;
			int num5 = XYWH.Y + (Height >> 1) >> 4;
			int num6 = 0;
			int num7 = num5 + 7;
			while (num6 < 15 && num7 < Main.MaxTilesY - 10)
			{
				num7++;
				for (int i = num3; i <= num4; i++)
				{
					try
					{
						if (WorldGen.SolidTile(i, num7) || Main.TileSet[i, num7].Liquid > 0)
						{
							num6++;
						}
					}
					catch
					{
						num6 += 15;
					}
				}
			}
			num7 += 4;
			if (WoFBottom == -1)
			{
				WoFBottom = num7 * 16;
			}
			else if (WoFBottom > num7 * 16)
			{
				WoFBottom--;
				if (WoFBottom < num7 * 16)
				{
					WoFBottom = num7 * 16;
				}
			}
			else if (WoFBottom < num7 * 16)
			{
				WoFBottom++;
				if (WoFBottom > num7 * 16)
				{
					WoFBottom = num7 * 16;
				}
			}
			num6 = 0;
			num7 = num5 - 7;
			while (num6 < 15 && num7 > Main.MaxTilesY - 200)
			{
				num7--;
				for (int j = num3; j <= num4; j++)
				{
					try
					{
						if (WorldGen.SolidTile(j, num7) || Main.TileSet[j, num7].Liquid > 0)
						{
							num6++;
						}
					}
					catch
					{
						num6 += 15;
					}
				}
			}
			num7 -= 4;
			if (WoFTop == -1)
			{
				WoFTop = num7 * 16;
			}
			else if (WoFTop > num7 * 16)
			{
				WoFTop--;
				if (WoFTop < num7 * 16)
				{
					WoFTop = num7 * 16;
				}
			}
			else if (WoFTop < num7 * 16)
			{
				WoFTop++;
				if (WoFTop > num7 * 16)
				{
					WoFTop = num7 * 16;
				}
			}
			int num8 = (WoFBottom + WoFTop >> 1) - (Height >> 1);
			Velocity.Y = 0f;
			Position.Y = num8;
			XYWH.Y = num8;
			float num9 = 1.5f;
			if (Life < (LifeMax >> 1) + (LifeMax >> 2))
			{
				num9 += 0.25f;
			}
			if (Life < LifeMax >> 1)
			{
				num9 += 0.4f;
			}
			if (Life < LifeMax >> 2)
			{
				num9 += 0.5f;
			}
			if (Life < LifeMax / 10)
			{
				num9 += 0.6f;
			}
			// BUG: I am unable to tell if this is a TerrariaOGC-made bug or if it exists in the original game. Basically, the WoF spawns with a target set to 8, which is not the player.
			// The problem lies with it's spawn velocity, as that seems to start at random variables, like one time, it was 2.88? This means the check below cannot pass.
			// This results in the mouth sometimes not targeting the player and remaining stationary at various angles. Leech spawning, roars, and animations remain intact however.
			if (Velocity.X == 0f) 
			{
				TargetClosest();
				Velocity.X = Direction;
			}
			if (Velocity.X < 0f)
			{
				Velocity.X = 0f - num9;
				Direction = -1;
			}
			else
			{
				Velocity.X = num9;
				Direction = 1;
			}
			SpriteDirection = Direction;
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num10 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
			float num11 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
			float num12 = (float)Math.Sqrt(num10 * num10 + num11 * num11);
			num10 *= num12;
			num11 *= num12;
			if (Direction > 0)
			{
				if (Main.PlayerSet[Target].XYWH.X + 10 > XYWH.X + (Width >> 1))
				{
					Rotation = (float)Math.Atan2(0f - num11, 0f - num10) + 3.14f;
				}
				else
				{
					Rotation = 0f;
				}
			}
			else if (Main.PlayerSet[Target].XYWH.X + 10 < XYWH.X + (Width >> 1))
			{
				Rotation = (float)Math.Atan2(num11, num10) + 3.14f;
			}
			else
			{
				Rotation = 0f;
			}
			if (LocalAI0 == 1 && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				LocalAI0 = 2;
				num8 = WoFBottom + WoFTop >> 1;
				num8 = num8 + WoFTop >> 1;
				int num13 = NewNPC(XYWH.X, num8, (int)ID.WALL_OF_FLESH_EYE, WhoAmI);
				Main.NPCSet[num13].AI0 = 1f;
				num8 = WoFBottom + WoFTop >> 1;
				num8 = num8 + WoFBottom >> 1;
				num13 = NewNPC(XYWH.X, num8, (int)ID.WALL_OF_FLESH_EYE, WhoAmI);
				Main.NPCSet[num13].AI0 = -1f;
				num8 = WoFBottom + WoFTop >> 1;
				num8 = num8 + WoFBottom >> 1;
				for (int k = 0; k < 11; k++)
				{
					num13 = NewNPC(XYWH.X, num8, (int)ID.THE_HUNGRY, WhoAmI);
					Main.NPCSet[num13].AI0 = k * 0.1f - 0.05f;
				}
			}
		}

		private void WallOfFleshEyesAI()
		{
			if (WoF < 0)
			{
				Active = 0;
				return;
			}
			RealLife = WoF;
			TargetClosest();
			Position.X = Main.NPCSet[WoF].Position.X;
			XYWH.X = Main.NPCSet[WoF].XYWH.X;
			Direction = Main.NPCSet[WoF].Direction;
			SpriteDirection = Direction;
			int num = WoFBottom + WoFTop >> 1;
			num = ((!(AI0 > 0f)) ? (num + WoFBottom >> 1) : (num + WoFTop >> 1));
			num -= Height >> 1;
			if (XYWH.Y > num + 1)
			{
				Velocity.Y = -1f;
			}
			else if (XYWH.Y < num - 1)
			{
				Velocity.Y = 1f;
			}
			else
			{
				Velocity.Y = 0f;
				XYWH.Y = num;
				Position.Y = num;
			}
			if (Velocity.Y > 5f)
			{
				Velocity.Y = 5f;
			}
			else if (Velocity.Y < -5f)
			{
				Velocity.Y = -5f;
			}
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
			float num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
			float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
			num2 *= num4;
			num3 *= num4;
			bool flag = true;
			if (Direction > 0)
			{
				if (Main.PlayerSet[Target].XYWH.X + 10 > XYWH.X + (Width >> 1))
				{
					Rotation = (float)Math.Atan2(0f - num3, 0f - num2) + 3.14f;
				}
				else
				{
					Rotation = 0f;
					flag = false;
				}
			}
			else if (Main.PlayerSet[Target].XYWH.X + 10 < XYWH.X + (Width >> 1))
			{
				Rotation = (float)Math.Atan2(num3, num2) + 3.14f;
			}
			else
			{
				Rotation = 0f;
				flag = false;
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			int num5 = 4;
			LocalAI1++;
			if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.75)
			{
				LocalAI1++;
				num5++;
			}
			if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.5)
			{
				LocalAI1++;
				num5++;
			}
			if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.25)
			{
				LocalAI1++;
				num5 += 2;
			}
			if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.1)
			{
				LocalAI1 += 2;
				num5 += 3;
			}
			if (LocalAI2 == 0)
			{
				if (LocalAI1 > 600)
				{
					LocalAI2 = 1;
					LocalAI1 = 0;
				}
			}
			else
			{
				if (LocalAI1 <= 45 || !Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
				{
					return;
				}
				LocalAI1 = 0;
				LocalAI2++;
				if (LocalAI2 >= num5)
				{
					LocalAI2 = 0;
				}
				if (flag)
				{
					float num6 = 9f;
					int num7 = 11;
					if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.5)
					{
						num7++;
						num6 += 1f;
					}
					if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.25)
					{
						num7++;
						num6 += 1f;
					}
					if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax * 0.1)
					{
						num7 += 2;
						num6 += 2f;
					}
					vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
					num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
					num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
					num4 = num6 / num4;
					num2 *= num4;
					num3 *= num4;
					vector.X += num2;
					vector.Y += num3;
					Projectile.NewProjectile(vector.X, vector.Y, num2, num3, 83, num7, 0f);
				}
			}
		}

		private void WallOfFleshTentacleAI()
		{
			if (WoF < 0)
			{
				Active = 0;
				return;
			}
			if (WasJustHit)
			{
				AI1 = 10f;
			}
			TargetClosest();
			float num = 0.1f;
			float num2 = 300f;
			if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax >> 2)
			{
				Damage = 75;
				Defense = 40;
				num2 = 900f;
			}
			else if (Main.NPCSet[WoF].Life < Main.NPCSet[WoF].LifeMax >> 1)
			{
				Damage = 60;
				Defense = 30;
				num2 = 700f;
			}
			else if (Main.NPCSet[WoF].Life < (Main.NPCSet[WoF].LifeMax >> 1) + (Main.NPCSet[WoF].LifeMax >> 2))
			{
				Damage = 45;
				Defense = 20;
				num2 = 500f;
			}
			float num3 = Main.NPCSet[WoF].Position.X + (Main.NPCSet[WoF].Width >> 1);
			float y = Main.NPCSet[WoF].Position.Y;
			float num4 = WoFBottom - WoFTop;
			y = WoFTop + num4 * AI0;
			AI2 += 1f;
			if (AI2 > 100f)
			{
				num2 = (int)(num2 * 1.3f);
				if (AI2 > 200f)
				{
					AI2 = 0f;
				}
			}
			Vector2 vector = new Vector2(num3, y);
			float num5 = Main.PlayerSet[Target].Position.X + 10f - (Width >> 1) - vector.X;
			float num6 = Main.PlayerSet[Target].Position.Y + 21f - (Height >> 1) - vector.Y;
			if (AI1 == 0f)
			{
				float num7 = num5 * num5 + num6 * num6;
				if (num7 > num2 * num2)
				{
					num7 = num2 / (float)Math.Sqrt(num7);
					num5 *= num7;
					num6 *= num7;
				}
				if (Position.X < num3 + num5)
				{
					Velocity.X += num;
					if (Velocity.X < 0f && num5 > 0f)
					{
						Velocity.X += num * 2.5f;
					}
				}
				else if (Position.X > num3 + num5)
				{
					Velocity.X -= num;
					if (Velocity.X > 0f && num5 < 0f)
					{
						Velocity.X -= num * 2.5f;
					}
				}
				if (Position.Y < y + num6)
				{
					Velocity.Y += num;
					if (Velocity.Y < 0f && num6 > 0f)
					{
						Velocity.Y += num * 2.5f;
					}
				}
				else if (Position.Y > y + num6)
				{
					Velocity.Y -= num;
					if (Velocity.Y > 0f && num6 < 0f)
					{
						Velocity.Y -= num * 2.5f;
					}
				}
				if (Velocity.X > 4f)
				{
					Velocity.X = 4f;
				}
				else if (Velocity.X < -4f)
				{
					Velocity.X = -4f;
				}
				if (Velocity.Y > 4f)
				{
					Velocity.Y = 4f;
				}
				else if (Velocity.Y < -4f)
				{
					Velocity.Y = -4f;
				}
			}
			else if (AI1 > 0f)
			{
				AI1 -= 1f;
			}
			else
			{
				AI1 = 0f;
			}
			if (num5 > 0f)
			{
				SpriteDirection = 1;
				Rotation = (float)Math.Atan2(num6, num5);
			}
			else if (num5 < 0f)
			{
				SpriteDirection = -1;
				Rotation = (float)(Math.Atan2(num6, num5) + Math.PI);
			}
			Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + (Height >> 1) >> 4, new Vector3(0.3f, 0.2f, 0.1f));
		}

		private unsafe void RetinazerAI()
		{
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead || Main.PlayerSet[Target].Active == 0)
			{
				TargetClosest();
			}
			bool dead = Main.PlayerSet[Target].IsDead;
			float num = Position.X + (Width >> 1) - Main.PlayerSet[Target].Position.X - 10f;
			float num2 = Position.Y + Height - 59f - Main.PlayerSet[Target].Position.Y - 21f;
			float num3 = (float)Math.Atan2(num2, num) + 1.57f;
			if (num3 < 0f)
			{
				num3 += 6.283f;
			}
			else if (num3 > 6.283f)
			{
				num3 -= 6.283f;
			}
			if (Rotation < num3)
			{
				if ((double)(num3 - Rotation) > 3.1415)
				{
					Rotation -= 0.1f;
				}
				else
				{
					Rotation += 0.1f;
				}
			}
			else if (Rotation > num3)
			{
				if ((double)(Rotation - num3) > 3.1415)
				{
					Rotation += 0.1f;
				}
				else
				{
					Rotation -= 0.1f;
				}
			}
			if (Rotation > num3 - 0.1f && Rotation < num3 + 0.1f)
			{
				Rotation = num3;
			}
			if (Rotation < 0f)
			{
				Rotation += 6.283f;
			}
			else if (Rotation > 6.283f)
			{
				Rotation -= 6.283f;
			}
			if (Rotation > num3 - 0.1f && Rotation < num3 + 0.1f)
			{
				Rotation = num3;
			}
			if (Main.Rand.Next(6) == 0)
			{
				Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr != null)
				{
					ptr->Velocity.X *= 0.5f;
					ptr->Velocity.Y *= 0.1f;
				}
			}
			if (Main.GameTime.DayTime || dead)
			{
				Velocity.Y -= 0.04f;
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				return;
			}
			if (AI0 == 0f)
			{
				if (AI1 == 0f)
				{
					float num4 = 7f;
					float num5 = 0.1f;
					int num6 = 1;
					if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 20)
					{
						num6 = -1;
					}
					Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num7 = Main.PlayerSet[Target].Position.X + 10f + num6 * 300 - vector.X;
					float num8 = Main.PlayerSet[Target].Position.Y + 21f - 300f - vector.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					float num10 = num9;
					num9 = num4 / num9;
					num7 *= num9;
					num8 *= num9;
					if (Velocity.X < num7)
					{
						Velocity.X += num5;
						if (Velocity.X < 0f && num7 > 0f)
						{
							Velocity.X += num5;
						}
					}
					else if (Velocity.X > num7)
					{
						Velocity.X -= num5;
						if (Velocity.X > 0f && num7 < 0f)
						{
							Velocity.X -= num5;
						}
					}
					if (Velocity.Y < num8)
					{
						Velocity.Y += num5;
						if (Velocity.Y < 0f && num8 > 0f)
						{
							Velocity.Y += num5;
						}
					}
					else if (Velocity.Y > num8)
					{
						Velocity.Y -= num5;
						if (Velocity.Y > 0f && num8 < 0f)
						{
							Velocity.Y -= num5;
						}
					}
					AI2 += 1f;
					if (AI2 >= 600f)
					{
						AI1 = 1f;
						AI2 = 0f;
						AI3 = 0f;
						Target = Player.MaxNumPlayers;
						ShouldNetUpdate = true;
					}
					else if (XYWH.Y + Height < Main.PlayerSet[Target].XYWH.Y && num10 < 400f)
					{
						if (!Main.PlayerSet[Target].IsDead)
						{
							AI3 += 1f;
						}
						if (AI3 >= 60f)
						{
							AI3 = 0f;
							vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
							num7 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
							num8 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
								num9 = 9f / num9;
								num7 *= num9;
								num8 *= num9;
								num7 += Main.Rand.Next(-40, 41) * 0.08f;
								num8 += Main.Rand.Next(-40, 41) * 0.08f;
								vector.X += num7 * 15f;
								vector.Y += num8 * 15f;
								Projectile.NewProjectile(vector.X, vector.Y, num7, num8, 83, 20, 0f);
							}
						}
					}
				}
				else if (AI1 == 1f)
				{
					Rotation = num3;
					float num11 = 12f;
					Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num12 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
					float num13 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
					float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
					num14 = num11 / num14;
					Velocity.X = num12 * num14;
					Velocity.Y = num13 * num14;
					AI1 = 2f;
				}
				else if (AI1 == 2f)
				{
					AI2 += 1f;
					if (AI2 >= 25f)
					{
						Velocity.X *= 0.96f;
						Velocity.Y *= 0.96f;
						if (Velocity.X > -0.1 && Velocity.X < 0.1)
						{
							Velocity.X = 0f;
						}
						if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
						{
							Velocity.Y = 0f;
						}
					}
					else
					{
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
					}
					if (AI2 >= 70f)
					{
						AI3 += 1f;
						AI2 = 0f;
						Target = Player.MaxNumPlayers;
						Rotation = num3;
						if (AI3 >= 4f)
						{
							AI1 = 0f;
							AI3 = 0f;
						}
						else
						{
							AI1 = 1f;
						}
					}
				}
				if (Life < LifeMax * 0.5)
				{
					AI0 = 1f;
					AI1 = 0f;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				return;
			}
			if (AI0 == 1f || AI0 == 2f)
			{
				if (AI0 == 1f)
				{
					AI2 += 0.005f;
					if (AI2 > 0.5)
					{
						AI2 = 0.5f;
					}
				}
				else
				{
					AI2 -= 0.005f;
					if (AI2 < 0f)
					{
						AI2 = 0f;
					}
				}
				Rotation += AI2;
				AI1 += 1f;
				if (AI1 == 100f)
				{
					AI0 += 1f;
					AI1 = 0f;
					if (AI0 == 3f)
					{
						AI2 = 0f;
					}
					else
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y);
						Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
						for (int i = 0; i < 2; i++)
						{
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 143);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int j = 0; j < 16; j++)
						{
							if (null == Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f))
							{
								break;
							}
						}
					}
				}
				Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f);
				Velocity.X *= 0.98f;
				Velocity.Y *= 0.98f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
				{
					Velocity.Y = 0f;
				}
				return;
			}
			Damage = (int)(DefDamage * 1.5);
			Defense = DefDefense + 15;
			SoundHit = 4;
			if (AI1 == 0f)
			{
				float num15 = 8f;
				float num16 = 0.15f;
				Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num17 = Main.PlayerSet[Target].Position.X + 10f - vector3.X;
				float num18 = Main.PlayerSet[Target].Position.Y + 21f - 300f - vector3.Y;
				float num19 = (float)Math.Sqrt(num17 * num17 + num18 * num18);
				num19 = num15 / num19;
				num17 *= num19;
				num18 *= num19;
				if (Velocity.X < num17)
				{
					Velocity.X += num16;
					if (Velocity.X < 0f && num17 > 0f)
					{
						Velocity.X += num16;
					}
				}
				else if (Velocity.X > num17)
				{
					Velocity.X -= num16;
					if (Velocity.X > 0f && num17 < 0f)
					{
						Velocity.X -= num16;
					}
				}
				if (Velocity.Y < num18)
				{
					Velocity.Y += num16;
					if (Velocity.Y < 0f && num18 > 0f)
					{
						Velocity.Y += num16;
					}
				}
				else if (Velocity.Y > num18)
				{
					Velocity.Y -= num16;
					if (Velocity.Y > 0f && num18 < 0f)
					{
						Velocity.Y -= num16;
					}
				}
				AI2 += 1f;
				if (AI2 >= 300f)
				{
					AI1 = 1f;
					AI2 = 0f;
					AI3 = 0f;
					TargetClosest();
					ShouldNetUpdate = true;
				}
				vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				num17 = Main.PlayerSet[Target].Position.X + 10f - vector3.X;
				num18 = Main.PlayerSet[Target].Position.Y + 21f - vector3.Y;
				Rotation = (float)Math.Atan2(num18, num17) - 1.57f;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI1++;
					if (Life < LifeMax * 0.75)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.5)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.25)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.1)
					{
						LocalAI1 += 2;
					}
					if (LocalAI1 > 140 && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
					{
						LocalAI1 = 0;
						num19 = (float)Math.Sqrt(num17 * num17 + num18 * num18);
						num19 = 9f / num19;
						num17 *= num19;
						num18 *= num19;
						vector3.X += num17 * 15f;
						vector3.Y += num18 * 15f;
						Projectile.NewProjectile(vector3.X, vector3.Y, num17, num18, 100, 25, 0f);
					}
				}
				return;
			}
			int num20 = 1;
			if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 20)
			{
				num20 = -1;
			}
			float num21 = 8f;
			float num22 = 0.2f;
			Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num23 = Main.PlayerSet[Target].Position.X + 10f + num20 * 340 - vector4.X;
			float num24 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
			float num25 = (float)Math.Sqrt(num23 * num23 + num24 * num24);
			num25 = num21 / num25;
			num23 *= num25;
			num24 *= num25;
			if (Velocity.X < num23)
			{
				Velocity.X += num22;
				if (Velocity.X < 0f && num23 > 0f)
				{
					Velocity.X += num22;
				}
			}
			else if (Velocity.X > num23)
			{
				Velocity.X -= num22;
				if (Velocity.X > 0f && num23 < 0f)
				{
					Velocity.X -= num22;
				}
			}
			if (Velocity.Y < num24)
			{
				Velocity.Y += num22;
				if (Velocity.Y < 0f && num24 > 0f)
				{
					Velocity.Y += num22;
				}
			}
			else if (Velocity.Y > num24)
			{
				Velocity.Y -= num22;
				if (Velocity.Y > 0f && num24 < 0f)
				{
					Velocity.Y -= num22;
				}
			}
			vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			num23 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
			num24 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
			Rotation = (float)Math.Atan2(num24, num23) - 1.57f;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				LocalAI1++;
				if (Life < LifeMax * 0.75)
				{
					LocalAI1++;
				}
				if (Life < LifeMax * 0.5)
				{
					LocalAI1++;
				}
				if (Life < LifeMax * 0.25)
				{
					LocalAI1++;
				}
				if (Life < LifeMax * 0.1)
				{
					LocalAI1 += 2;
				}
				if (LocalAI1 > 45 && Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
				{
					LocalAI1 = 0;
					num25 = (float)Math.Sqrt(num23 * num23 + num24 * num24);
					num25 = 9f / num25;
					num23 *= num25;
					num24 *= num25;
					vector4.X += num23 * 15f;
					vector4.Y += num24 * 15f;
					Projectile.NewProjectile(vector4.X, vector4.Y, num23, num24, 100, 20, 0f);
				}
			}
			AI2 += 1f;
			if (AI2 >= 200f)
			{
				AI1 = 0f;
				AI2 = 0f;
				AI3 = 0f;
				TargetClosest();
				ShouldNetUpdate = true;
			}
		}

		private unsafe void SpazmatismAI()
		{
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead || Main.PlayerSet[Target].Active == 0)
			{
				TargetClosest();
			}
			bool dead = Main.PlayerSet[Target].IsDead;
			float num = Position.X + (Width >> 1) - Main.PlayerSet[Target].Position.X - 10f;
			float num2 = Position.Y + Height - 59f - Main.PlayerSet[Target].Position.Y - 21f;
			float num3 = (float)Math.Atan2(num2, num) + 1.57f;
			if (num3 < 0f)
			{
				num3 += 6.283f;
			}
			else if (num3 > 6.283f)
			{
				num3 -= 6.283f;
			}
			if (Rotation < num3)
			{
				if ((double)(num3 - Rotation) > 3.1415)
				{
					Rotation -= 0.15f;
				}
				else
				{
					Rotation += 0.15f;
				}
			}
			else if (Rotation > num3)
			{
				if ((double)(Rotation - num3) > 3.1415)
				{
					Rotation += 0.15f;
				}
				else
				{
					Rotation -= 0.15f;
				}
			}
			if (Rotation > num3 - 0.15f && Rotation < num3 + 0.15f)
			{
				Rotation = num3;
			}
			if (Rotation < 0f)
			{
				Rotation += 6.283f;
			}
			else if (Rotation > 6.283f)
			{
				Rotation -= 6.283f;
			}
			if (Rotation > num3 - 0.15f && Rotation < num3 + 0.15f)
			{
				Rotation = num3;
			}
			if (Main.Rand.Next(6) == 0)
			{
				Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr != null)
				{
					ptr->Velocity.X *= 0.5f;
					ptr->Velocity.Y *= 0.1f;
				}
			}
			if (Main.GameTime.DayTime || dead)
			{
				Velocity.Y -= 0.04f;
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				return;
			}
			if (AI0 == 0f)
			{
				if (AI1 == 0f)
				{
					TargetClosest();
					float num4 = 12f;
					float num5 = 0.4f;
					int num6 = 1;
					if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 20)
					{
						num6 = -1;
					}
					Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num7 = Main.PlayerSet[Target].Position.X + 10f + num6 * 400 - vector.X;
					float num8 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					num9 = num4 / num9;
					num7 *= num9;
					num8 *= num9;
					if (Velocity.X < num7)
					{
						Velocity.X += num5;
						if (Velocity.X < 0f && num7 > 0f)
						{
							Velocity.X += num5;
						}
					}
					else if (Velocity.X > num7)
					{
						Velocity.X -= num5;
						if (Velocity.X > 0f && num7 < 0f)
						{
							Velocity.X -= num5;
						}
					}
					if (Velocity.Y < num8)
					{
						Velocity.Y += num5;
						if (Velocity.Y < 0f && num8 > 0f)
						{
							Velocity.Y += num5;
						}
					}
					else if (Velocity.Y > num8)
					{
						Velocity.Y -= num5;
						if (Velocity.Y > 0f && num8 < 0f)
						{
							Velocity.Y -= num5;
						}
					}
					AI2 += 1f;
					if (AI2 >= 600f)
					{
						AI1 = 1f;
						AI2 = 0f;
						AI3 = 0f;
						Target = Player.MaxNumPlayers;
						ShouldNetUpdate = true;
					}
					else
					{
						if (!Main.PlayerSet[Target].IsDead)
						{
							AI3 += 1f;
						}
						if (AI3 >= 60f)
						{
							AI3 = 0f;
							vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
							num7 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
							num8 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
								num9 = 12f / num9;
								num7 *= num9;
								num8 *= num9;
								num7 += Main.Rand.Next(-40, 41) * 0.05f;
								num8 += Main.Rand.Next(-40, 41) * 0.05f;
								vector.X += num7 * 4f;
								vector.Y += num8 * 4f;
								Projectile.NewProjectile(vector.X, vector.Y, num7, num8, 96, 25, 0f);
							}
						}
					}
				}
				else if (AI1 == 1f)
				{
					Rotation = num3;
					float num10 = 13f;
					Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num11 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
					float num12 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
					float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
					num13 = num10 / num13;
					Velocity.X = num11 * num13;
					Velocity.Y = num12 * num13;
					AI1 = 2f;
				}
				else if (AI1 == 2f)
				{
					AI2 += 1f;
					if (AI2 >= 8f)
					{
						Velocity.X *= 0.9f;
						Velocity.Y *= 0.9f;
						if (Velocity.X > -0.1 && Velocity.X < 0.1)
						{
							Velocity.X = 0f;
						}
						if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
						{
							Velocity.Y = 0f;
						}
					}
					else
					{
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
					}
					if (AI2 >= 42f)
					{
						AI3 += 1f;
						AI2 = 0f;
						Target = Player.MaxNumPlayers;
						Rotation = num3;
						if (AI3 >= 10f)
						{
							AI1 = 0f;
							AI3 = 0f;
						}
						else
						{
							AI1 = 1f;
						}
					}
				}
				if (Life < LifeMax * 0.5)
				{
					AI0 = 1f;
					AI1 = 0f;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				return;
			}
			if (AI0 == 1f || AI0 == 2f)
			{
				if (AI0 == 1f)
				{
					AI2 += 0.005f;
					if (AI2 > 0.5)
					{
						AI2 = 0.5f;
					}
				}
				else
				{
					AI2 -= 0.005f;
					if (AI2 < 0f)
					{
						AI2 = 0f;
					}
				}
				Rotation += AI2;
				AI1 += 1f;
				if (AI1 == 100f)
				{
					AI0 += 1f;
					AI1 = 0f;
					if (AI0 == 3f)
					{
						AI2 = 0f;
					}
					else
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y);
						Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
						for (int i = 0; i < 2; i++)
						{
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 144);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int j = 0; j < 16; j++)
						{
							if (null == Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f))
							{
								break;
							}
						}
					}
				}
				Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f);
				Velocity.X *= 0.98f;
				Velocity.Y *= 0.98f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
				{
					Velocity.Y = 0f;
				}
				return;
			}
			SoundHit = 4;
			Damage = DefDamage + (DefDamage >> 1);
			Defense = DefDefense + 25;
			if (AI1 == 0f)
			{
				float num14 = 4f;
				float num15 = 0.1f;
				int num16 = 1;
				if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 20)
				{
					num16 = -1;
				}
				Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num17 = Main.PlayerSet[Target].Position.X + 10f + num16 * 180 - vector3.X;
				float num18 = Main.PlayerSet[Target].Position.Y + 21f - vector3.Y;
				float num19 = (float)Math.Sqrt(num17 * num17 + num18 * num18);
				num19 = num14 / num19;
				num17 *= num19;
				num18 *= num19;
				if (Velocity.X < num17)
				{
					Velocity.X += num15;
					if (Velocity.X < 0f && num17 > 0f)
					{
						Velocity.X += num15;
					}
				}
				else if (Velocity.X > num17)
				{
					Velocity.X -= num15;
					if (Velocity.X > 0f && num17 < 0f)
					{
						Velocity.X -= num15;
					}
				}
				if (Velocity.Y < num18)
				{
					Velocity.Y += num15;
					if (Velocity.Y < 0f && num18 > 0f)
					{
						Velocity.Y += num15;
					}
				}
				else if (Velocity.Y > num18)
				{
					Velocity.Y -= num15;
					if (Velocity.Y > 0f && num18 < 0f)
					{
						Velocity.Y -= num15;
					}
				}
				AI2 += 1f;
				if (AI2 >= 400f)
				{
					AI1 = 1f;
					AI2 = 0f;
					AI3 = 0f;
					Target = Player.MaxNumPlayers;
					ShouldNetUpdate = true;
				}
				if (!Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
				{
					return;
				}
				LocalAI2++;
				if (LocalAI2 > 22)
				{
					LocalAI2 = 0;
					Main.PlaySound(2, XYWH.X, XYWH.Y, 34);
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI1++;
					if (Life < LifeMax * 0.75f)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.5f)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.25f)
					{
						LocalAI1++;
					}
					if (Life < LifeMax * 0.1f)
					{
						LocalAI1 += 2;
					}
					if (LocalAI1 > 8)
					{
						LocalAI1 = 0;
						vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						num17 = Main.PlayerSet[Target].Position.X + 10f - vector3.X;
						num18 = Main.PlayerSet[Target].Position.Y + 21f - vector3.Y;
						num19 = (float)Math.Sqrt(num17 * num17 + num18 * num18);
						num19 = 6f / num19;
						num17 *= num19;
						num18 *= num19;
						num18 += Main.Rand.Next(-40, 41) * 0.01f;
						num17 += Main.Rand.Next(-40, 41) * 0.01f;
						num18 += Velocity.Y * 0.5f;
						num17 += Velocity.X * 0.5f;
						vector3.X -= num17 * 1f;
						vector3.Y -= num18 * 1f;
						Projectile.NewProjectile(vector3.X, vector3.Y, num17, num18, 101, 30, 0f);
					}
				}
			}
			else if (AI1 == 1f)
			{
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				Rotation = num3;
				float num20 = 14f;
				Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num21 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
				float num22 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
				float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
				num23 = num20 / num23;
				Velocity.X = num21 * num23;
				Velocity.Y = num22 * num23;
				AI1 = 2f;
			}
			else
			{
				if (AI1 != 2f)
				{
					return;
				}
				AI2 += 1f;
				if (AI2 >= 50f)
				{
					Velocity.X *= 0.93f;
					Velocity.Y *= 0.93f;
					if (Velocity.X > -0.1 && Velocity.X < 0.1)
					{
						Velocity.X = 0f;
					}
					if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
					{
						Velocity.Y = 0f;
					}
				}
				else
				{
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
				}
				if (AI2 >= 80f)
				{
					AI3 += 1f;
					AI2 = 0f;
					Target = Player.MaxNumPlayers;
					Rotation = num3;
					if (AI3 >= 6f)
					{
						AI1 = 0f;
						AI3 = 0f;
					}
					else
					{
						AI1 = 1f;
					}
				}
			}
		}

		private void SkeletronPrimeAI()
		{
			Damage = DefDamage;
			Defense = DefDefense;
			if (AI0 == 0f && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				TargetClosest();
				AI0 = 1f;
				if (Type != (int)ID.DUNGEON_GUARDIAN)
				{
					int num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.PRIME_CANNON, WhoAmI);
					Main.NPCSet[num].AI0 = -1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].ShouldNetUpdate = true;
					num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.PRIME_SAW, WhoAmI);
					Main.NPCSet[num].AI0 = 1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].ShouldNetUpdate = true;
					num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.PRIME_VICE, WhoAmI);
					Main.NPCSet[num].AI0 = -1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].AI3 = 150f;
					Main.NPCSet[num].ShouldNetUpdate = true;
					num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + (Height >> 1), (int)ID.PRIME_LASER, WhoAmI);
					Main.NPCSet[num].AI0 = 1f;
					Main.NPCSet[num].AI1 = WhoAmI;
					Main.NPCSet[num].Target = Target;
					Main.NPCSet[num].ShouldNetUpdate = true;
					Main.NPCSet[num].AI3 = 150f;
				}
			}
			if (Type == (int)ID.DUNGEON_GUARDIAN && AI1 != 3f && AI1 != 2f)
			{
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				AI1 = 2f;
			}
			if (Main.PlayerSet[Target].IsDead || Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) > 6000 || Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 6000)
			{
				TargetClosest();
				if (Main.PlayerSet[Target].IsDead || Math.Abs(XYWH.X - Main.PlayerSet[Target].XYWH.X) > 6000 || Math.Abs(XYWH.Y - Main.PlayerSet[Target].XYWH.Y) > 6000)
				{
					AI1 = 3f;
				}
			}
			if (Main.GameTime.DayTime && AI1 != 3f && AI1 != 2f)
			{
				AI1 = 2f;
				Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
			}
			if (AI1 == 0f)
			{
				AI2 += 1f;
				if (AI2 >= 600f)
				{
					AI2 = 0f;
					AI1 = 1f;
					TargetClosest();
					ShouldNetUpdate = true;
				}
				Rotation = Velocity.X * (71f / (339f * (float)Math.PI));
				if (XYWH.Y > Main.PlayerSet[Target].XYWH.Y - 200)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.98f;
					}
					Velocity.Y -= 0.1f;
					if (Velocity.Y > 2f)
					{
						Velocity.Y = 2f;
					}
				}
				else if (XYWH.Y < Main.PlayerSet[Target].XYWH.Y - 500)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.98f;
					}
					Velocity.Y += 0.1f;
					if (Velocity.Y < -2f)
					{
						Velocity.Y = -2f;
					}
				}
				if (XYWH.X + (Width >> 1) > Main.PlayerSet[Target].XYWH.X + 10 + 100)
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.98f;
					}
					Velocity.X -= 0.1f;
					if (Velocity.X > 8f)
					{
						Velocity.X = 8f;
					}
				}
				else if (XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 10 - 100)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.98f;
					}
					Velocity.X += 0.1f;
					if (Velocity.X < -8f)
					{
						Velocity.X = -8f;
					}
				}
			}
			else if (AI1 == 1f)
			{
				Defense *= 2;
				Damage *= 2;
				AI2 += 1f;
				if (AI2 == 2f)
				{
					Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				}
				if (AI2 >= 400f)
				{
					AI2 = 0f;
					AI1 = 0f;
				}
				Rotation += Direction * 0.3f;
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num2 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num3 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				num4 = 2f / num4;
				Velocity.X = num2 * num4;
				Velocity.Y = num3 * num4;
			}
			else if (AI1 == 2f)
			{
				Damage = 9999;
				Defense = 9999;
				Rotation += Direction * 0.3f;
				Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num5 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
				float num6 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
				float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
				num7 = 8f / num7;
				Velocity.X = num5 * num7;
				Velocity.Y = num6 * num7;
			}
			else if (AI1 == 3f)
			{
				Velocity.Y += 0.1f;
				if (Velocity.Y < 0f)
				{
					Velocity.Y *= 0.95f;
				}
				Velocity.X *= 0.95f;
				if (TimeLeft > 500)
				{
					TimeLeft = 500;
				}
			}
		}

		private void SkeletronPrimeSawHand()
		{
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector.X;
			float num2 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector.Y;
			float num3 = (float)Math.Sqrt(num * num + num2 * num2);
			if (AI2 != 99f)
			{
				if (num3 > 800f)
				{
					AI2 = 99f;
				}
			}
			else if (num3 < 400f)
			{
				AI2 = 0f;
			}
			SpriteDirection = (sbyte)(0f - AI0);
			if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != 32)
			{
				AI2 += 10f;
				if (AI2 > 50f || Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					Life = -1;
					HitEffect();
					Active = 0;
					return;
				}
			}
			if (AI2 == 99f)
			{
				if (XYWH.Y > Main.NPCSet[(int)AI1].XYWH.Y)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.96f;
					}
					Velocity.Y -= 0.1f;
					if (Velocity.Y > 8f)
					{
						Velocity.Y = 8f;
					}
				}
				else if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.96f;
					}
					Velocity.Y += 0.1f;
					if (Velocity.Y < -8f)
					{
						Velocity.Y = -8f;
					}
				}
				if (XYWH.X + (Width >> 1) > Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1))
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.96f;
					}
					Velocity.X -= 0.5f;
					if (Velocity.X > 12f)
					{
						Velocity.X = 12f;
					}
				}
				else if (XYWH.X + (Width >> 1) < Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1))
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.96f;
					}
					Velocity.X += 0.5f;
					if (Velocity.X < -12f)
					{
						Velocity.X = -12f;
					}
				}
			}
			else if (AI2 == 0f || AI2 == 3f)
			{
				if (Main.NPCSet[(int)AI1].AI1 == 3f && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Main.NPCSet[(int)AI1].AI1 != 0f)
				{
					TargetClosest();
					if (Main.PlayerSet[Target].IsDead)
					{
						Velocity.Y += 0.1f;
						if (Velocity.Y > 16f)
						{
							Velocity.Y = 16f;
						}
					}
					else
					{
						Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num4 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
						float num5 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
						float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
						num6 = 7f / num6;
						num4 *= num6;
						num5 *= num6;
						Rotation = (float)Math.Atan2(num5, num4) - 1.57f;
						if (Velocity.X > num4)
						{
							if (Velocity.X > 0f)
							{
								Velocity.X *= 0.97f;
							}
							Velocity.X -= 0.05f;
						}
						if (Velocity.X < num4)
						{
							if (Velocity.X < 0f)
							{
								Velocity.X *= 0.97f;
							}
							Velocity.X += 0.05f;
						}
						if (Velocity.Y > num5)
						{
							if (Velocity.Y > 0f)
							{
								Velocity.Y *= 0.97f;
							}
							Velocity.Y -= 0.05f;
						}
						if (Velocity.Y < num5)
						{
							if (Velocity.Y < 0f)
							{
								Velocity.Y *= 0.97f;
							}
							Velocity.Y += 0.05f;
						}
					}
					AI3 += 1f;
					if (AI3 >= 600f)
					{
						AI2 = 0f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
				}
				else
				{
					AI3 += 1f;
					if (AI3 >= 300f)
					{
						AI2 += 1f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
					if (XYWH.Y > Main.NPCSet[(int)AI1].XYWH.Y + 320)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
					}
					else if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y + 260)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.04f;
						if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					if (XYWH.X + (Width >> 1) > Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1))
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.3f;
						if (Velocity.X > 12f)
						{
							Velocity.X = 12f;
						}
					}
					else if (XYWH.X + (Width >> 1) < Main.NPCSet[(int)AI1].XYWH.X + (Main.NPCSet[(int)AI1].Width >> 1) - 250)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.3f;
						if (Velocity.X < -12f)
						{
							Velocity.X = -12f;
						}
					}
				}
				Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num7 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector3.X;
				float num8 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector3.Y;
				Math.Sqrt(num7 * num7 + num8 * num8);
				Rotation = (float)Math.Atan2(num8, num7) + 1.57f;
			}
			else if (AI2 == 1f)
			{
				Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num9 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector4.X;
				float num10 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector4.Y;
				float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
				Rotation = (float)Math.Atan2(num10, num9) + 1.57f;
				Velocity.X *= 0.95f;
				Velocity.Y -= 0.1f;
				if (Velocity.Y < -8f)
				{
					Velocity.Y = -8f;
				}
				if (XYWH.Y < Main.NPCSet[(int)AI1].XYWH.Y - 200)
				{
					TargetClosest();
					AI2 = 2f;
					vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num9 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
					num10 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
					num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
					num11 = 22f / num11;
					Velocity.X = num9 * num11;
					Velocity.Y = num10 * num11;
					ShouldNetUpdate = true;
				}
			}
			else if (AI2 == 2f)
			{
				if (Velocity.Y < 0f || XYWH.Y > Main.PlayerSet[Target].XYWH.Y)
				{
					AI2 = 3f;
				}
			}
			else if (AI2 == 4f)
			{
				TargetClosest();
				Vector2 vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num12 = Main.PlayerSet[Target].Position.X + 10f - vector5.X;
				float num13 = Main.PlayerSet[Target].Position.Y + 21f - vector5.Y;
				float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
				num14 = 7f / num14;
				num12 *= num14;
				num13 *= num14;
				if (Velocity.X > num12)
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.97f;
					}
					Velocity.X -= 0.05f;
				}
				if (Velocity.X < num12)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.97f;
					}
					Velocity.X += 0.05f;
				}
				if (Velocity.Y > num13)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.97f;
					}
					Velocity.Y -= 0.05f;
				}
				if (Velocity.Y < num13)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.97f;
					}
					Velocity.Y += 0.05f;
				}
				AI3 += 1f;
				if (AI3 >= 600f)
				{
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				num12 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector5.X;
				num13 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector5.Y;
				num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
				Rotation = (float)Math.Atan2(num13, num12) + 1.57f;
			}
			else if (AI2 == 5f && ((Velocity.X > 0f && XYWH.X + (Width >> 1) > Main.PlayerSet[Target].XYWH.X + 10) || (Velocity.X < 0f && XYWH.X + (Width >> 1) < Main.PlayerSet[Target].XYWH.X + 10)))
			{
				AI2 = 0f;
			}
		}

		private void SkeletronPrimeViceHand()
		{
			SpriteDirection = (sbyte)(0f - AI0);
			Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
			float num = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector.X;
			float num2 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector.Y;
			float num3 = (float)Math.Sqrt(num * num + num2 * num2);
			if (AI2 != 99f)
			{
				if (num3 > 800f)
				{
					AI2 = 99f;
				}
			}
			else if (num3 < 400f)
			{
				AI2 = 0f;
			}
			if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != 32)
			{
				AI2 += 10f;
				if (AI2 > 50f || Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					Life = -1;
					HitEffect();
					Active = 0;
					return;
				}
			}
			if (AI2 == 99f)
			{
				if (Position.Y > Main.NPCSet[(int)AI1].Position.Y)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.96f;
					}
					Velocity.Y -= 0.1f;
					if (Velocity.Y > 8f)
					{
						Velocity.Y = 8f;
					}
				}
				else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.96f;
					}
					Velocity.Y += 0.1f;
					if (Velocity.Y < -8f)
					{
						Velocity.Y = -8f;
					}
				}
				if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1))
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.96f;
					}
					Velocity.X -= 0.5f;
					if (Velocity.X > 12f)
					{
						Velocity.X = 12f;
					}
				}
				if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1))
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.96f;
					}
					Velocity.X += 0.5f;
					if (Velocity.X < -12f)
					{
						Velocity.X = -12f;
					}
				}
			}
			else if (AI2 == 0f || AI2 == 3f)
			{
				if (Main.NPCSet[(int)AI1].AI1 == 3f && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Main.NPCSet[(int)AI1].AI1 != 0f)
				{
					TargetClosest();
					TargetClosest();
					if (Main.PlayerSet[Target].IsDead)
					{
						Velocity.Y += 0.1f;
						if (Velocity.Y > 16f)
						{
							Velocity.Y = 16f;
						}
					}
					else
					{
						Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
						float num4 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
						float num5 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
						float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
						num6 = 12f / num6;
						num4 *= num6;
						num5 *= num6;
						Rotation = (float)Math.Atan2(num5, num4) - 1.57f;
						if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) < 2f)
						{
							Rotation = (float)Math.Atan2(num5, num4) - 1.57f;
							Velocity.X = num4;
							Velocity.Y = num5;
							ShouldNetUpdate = true;
						}
						else
						{
							Velocity.X *= 0.97f;
							Velocity.Y *= 0.97f;
						}
						AI3 += 1f;
						if (AI3 >= 600f)
						{
							AI2 = 0f;
							AI3 = 0f;
							ShouldNetUpdate = true;
						}
					}
				}
				else
				{
					AI3 += 1f;
					if (AI3 >= 600f)
					{
						AI2 += 1f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
					if (Position.Y > Main.NPCSet[(int)AI1].Position.Y + 300f)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.1f;
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
					}
					else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y + 230f)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.1f;
						if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) + 250f)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.94f;
						}
						Velocity.X -= 0.3f;
						if (Velocity.X > 9f)
						{
							Velocity.X = 9f;
						}
					}
					if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1))
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.94f;
						}
						Velocity.X += 0.2f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				Vector2 vector3 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num7 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector3.X;
				float num8 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector3.Y;
				Math.Sqrt(num7 * num7 + num8 * num8);
				Rotation = (float)Math.Atan2(num8, num7) + 1.57f;
			}
			else if (AI2 == 1f)
			{
				if (Velocity.Y > 0f)
				{
					Velocity.Y *= 0.9f;
				}
				Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num9 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 280f * AI0 - vector4.X;
				float num10 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector4.Y;
				float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
				Rotation = (float)Math.Atan2(num10, num9) + 1.57f;
				Velocity.X = (Velocity.X * 5f + Main.NPCSet[(int)AI1].Velocity.X) / 6f;
				Velocity.X += 0.5f;
				Velocity.Y -= 0.5f;
				if (Velocity.Y < -9f)
				{
					Velocity.Y = -9f;
				}
				if (Position.Y < Main.NPCSet[(int)AI1].Position.Y - 280f)
				{
					TargetClosest();
					AI2 = 2f;
					vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num9 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
					num10 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
					num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
					num11 = 20f / num11;
					Velocity.X = num9 * num11;
					Velocity.Y = num10 * num11;
					ShouldNetUpdate = true;
				}
			}
			else if (AI2 == 2f)
			{
				if (Position.Y > Main.PlayerSet[Target].Position.Y || Velocity.Y < 0f)
				{
					if (AI3 >= 4f)
					{
						AI2 = 3f;
						AI3 = 0f;
					}
					else
					{
						AI2 = 1f;
						AI3 += 1f;
					}
				}
			}
			else if (AI2 == 4f)
			{
				Vector2 vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num12 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector5.X;
				float num13 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector5.Y;
				float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
				Rotation = (float)Math.Atan2(num13, num12) + 1.57f;
				Velocity.Y = (Velocity.Y * 5f + Main.NPCSet[(int)AI1].Velocity.Y) / 6f;
				Velocity.X += 0.5f;
				if (Velocity.X > 12f)
				{
					Velocity.X = 12f;
				}
				if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 500f || Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) + 500f)
				{
					TargetClosest();
					AI2 = 5f;
					vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					num12 = Main.PlayerSet[Target].Position.X + 10f - vector5.X;
					num13 = Main.PlayerSet[Target].Position.Y + 21f - vector5.Y;
					num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
					num14 = 17f / num14;
					Velocity.X = num12 * num14;
					Velocity.Y = num13 * num14;
					ShouldNetUpdate = true;
				}
			}
			else if (AI2 == 5f && Position.X + (Width >> 1) < Main.PlayerSet[Target].Position.X + 10f - 100f)
			{
				if (AI3 >= 4f)
				{
					AI2 = 0f;
					AI3 = 0f;
				}
				else
				{
					AI2 = 4f;
					AI3 += 1f;
				}
			}
		}

		private void SkeletronPrimeCannonHand()
		{
			SpriteDirection = (sbyte)(0f - AI0);
			if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != 32)
			{
				AI2 += 10f;
				if (AI2 > 50f || Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					Life = -1;
					HitEffect();
					Active = 0;
					return;
				}
			}
			if (AI2 == 0f)
			{
				if (Main.NPCSet[(int)AI1].AI1 == 3f && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Main.NPCSet[(int)AI1].AI1 != 0f)
				{
					LocalAI0 += 2;
					if (Position.Y > Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.07f;
						if (Velocity.Y > 6f)
						{
							Velocity.Y = 6f;
						}
					}
					else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.07f;
						if (Velocity.Y < -6f)
						{
							Velocity.Y = -6f;
						}
					}
					if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120f * AI0)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.1f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120f * AI0)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.1f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				else
				{
					AI3 += 1f;
					if (AI3 >= 1100f)
					{
						LocalAI0 = 0;
						AI2 = 1f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
					if (Position.Y > Main.NPCSet[(int)AI1].Position.Y - 150f)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.04f;
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
					}
					else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y - 150f)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.04f;
						if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) + 200f)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.2f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) + 160f)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.2f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 200f * AI0 - vector.X;
				float num2 = Main.NPCSet[(int)AI1].Position.Y + 230f - vector.Y;
				float num3 = (float)Math.Sqrt(num * num + num2 * num2);
				Rotation = (float)Math.Atan2(num2, num) + 1.57f;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI0++;
					if (LocalAI0 > 140)
					{
						LocalAI0 = 0;
						num3 = 12f / num3;
						num = (0f - num) * num3;
						num2 = (0f - num2) * num3;
						num += Main.Rand.Next(-40, 41) * 0.01f;
						num2 += Main.Rand.Next(-40, 41) * 0.01f;
						vector.X += num * 4f;
						vector.Y += num2 * 4f;
						Projectile.NewProjectile(vector.X, vector.Y, num, num2, 102, 0, 0f);
					}
				}
			}
			else
			{
				if (AI2 != 1f)
				{
					return;
				}
				AI3 += 1f;
				if (AI3 >= 300f)
				{
					LocalAI0 = 0;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num4 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - vector2.X;
				float num5 = Main.NPCSet[(int)AI1].Position.Y - vector2.Y;
				num5 = Main.PlayerSet[Target].Position.Y + 21f - 80f - vector2.Y;
				float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
				num6 = 6f / num6;
				num4 *= num6;
				num5 *= num6;
				if (Velocity.X > num4)
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.9f;
					}
					Velocity.X -= 0.04f;
				}
				if (Velocity.X < num4)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.9f;
					}
					Velocity.X += 0.04f;
				}
				if (Velocity.Y > num5)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.9f;
					}
					Velocity.Y -= 0.08f;
				}
				if (Velocity.Y < num5)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.9f;
					}
					Velocity.Y += 0.08f;
				}
				TargetClosest();
				vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				num4 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
				num5 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
				num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
				Rotation = (float)Math.Atan2(num5, num4) - 1.57f;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI0++;
					if (LocalAI0 > 40)
					{
						LocalAI0 = 0;
						num6 = 10f / num6;
						num4 *= num6;
						num5 *= num6;
						num4 += Main.Rand.Next(-40, 41) * 0.01f;
						num5 += Main.Rand.Next(-40, 41) * 0.01f;
						vector2.X += num4 * 4f;
						vector2.Y += num5 * 4f;
						Projectile.NewProjectile(vector2.X, vector2.Y, num4, num5, 102, 0, 0f);
					}
				}
			}
		}

		private void SkeletronPrimeLaserHand()
		{
			SpriteDirection = (sbyte)(0f - AI0);
			if (Main.NPCSet[(int)AI1].Active == 0 || Main.NPCSet[(int)AI1].AIStyle != 32)
			{
				AI2 += 10f;
				if (AI2 > 50f || Main.NetMode != (byte)NetModeSetting.SERVER)
				{
					Life = -1;
					HitEffect();
					Active = 0;
					return;
				}
			}
			if (AI2 == 0f || AI2 == 3f)
			{
				if (Main.NPCSet[(int)AI1].AI1 == 3f && TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				if (Main.NPCSet[(int)AI1].AI1 != 0f)
				{
					LocalAI0 += 3;
					if (Position.Y > Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.07f;
						if (Velocity.Y > 6f)
						{
							Velocity.Y = 6f;
						}
					}
					else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.07f;
						if (Velocity.Y < -6f)
						{
							Velocity.Y = -6f;
						}
					}
					if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120f * AI0)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.1f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 120f * AI0)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.1f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				else
				{
					AI3 += 1f;
					if (AI3 >= 800f)
					{
						AI2 += 1f;
						AI3 = 0f;
						ShouldNetUpdate = true;
					}
					if (Position.Y > Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y -= 0.1f;
						if (Velocity.Y > 3f)
						{
							Velocity.Y = 3f;
						}
					}
					else if (Position.Y < Main.NPCSet[(int)AI1].Position.Y - 100f)
					{
						if (Velocity.Y < 0f)
						{
							Velocity.Y *= 0.96f;
						}
						Velocity.Y += 0.1f;
						if (Velocity.Y < -3f)
						{
							Velocity.Y = -3f;
						}
					}
					if (Position.X + (Width >> 1) > Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 180f * AI0)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X -= 0.14f;
						if (Velocity.X > 8f)
						{
							Velocity.X = 8f;
						}
					}
					if (Position.X + (Width >> 1) < Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - 180f * AI0)
					{
						if (Velocity.X < 0f)
						{
							Velocity.X *= 0.96f;
						}
						Velocity.X += 0.14f;
						if (Velocity.X < -8f)
						{
							Velocity.X = -8f;
						}
					}
				}
				TargetClosest();
				Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num = Main.PlayerSet[Target].Position.X + 10f - vector.X;
				float num2 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
				float num3 = (float)Math.Sqrt(num * num + num2 * num2);
				Rotation = (float)Math.Atan2(num2, num) - 1.57f;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					LocalAI0++;
					if (LocalAI0 > 200)
					{
						LocalAI0 = 0;
						num3 = 8f / num3;
						num *= num3;
						num2 *= num3;
						num += Main.Rand.Next(-40, 41) * 0.05f;
						num2 += Main.Rand.Next(-40, 41) * 0.05f;
						vector.X += num * 8f;
						vector.Y += num2 * 8f;
						Projectile.NewProjectile(vector.X, vector.Y, num, num2, 100, 25, 0f);
					}
				}
			}
			else
			{
				if (AI2 != 1f)
				{
					return;
				}
				AI3 += 1f;
				if (AI3 >= 200f)
				{
					LocalAI0 = 0;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				Vector2 vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num4 = Main.PlayerSet[Target].Position.X + 10f - 350f - vector2.X;
				float num5 = Main.PlayerSet[Target].Position.Y + 21f - 20f - vector2.Y;
				float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
				num6 = 7f / num6;
				num4 *= num6;
				num5 *= num6;
				if (Velocity.X > num4)
				{
					if (Velocity.X > 0f)
					{
						Velocity.X *= 0.9f;
					}
					Velocity.X -= 0.1f;
				}
				if (Velocity.X < num4)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X *= 0.9f;
					}
					Velocity.X += 0.1f;
				}
				if (Velocity.Y > num5)
				{
					if (Velocity.Y > 0f)
					{
						Velocity.Y *= 0.9f;
					}
					Velocity.Y -= 0.03f;
				}
				if (Velocity.Y < num5)
				{
					if (Velocity.Y < 0f)
					{
						Velocity.Y *= 0.9f;
					}
					Velocity.Y += 0.03f;
				}
				TargetClosest();
				vector2 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				num4 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
				num5 = Main.PlayerSet[Target].Position.Y + 21f - vector2.Y;
				num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
				Rotation = (float)Math.Atan2(num5, num4) - 1.57f;
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					LocalAI0++;
					if (LocalAI0 > 80)
					{
						LocalAI0 = 0;
						num6 = 10f / num6;
						num4 *= num6;
						num5 *= num6;
						num4 += Main.Rand.Next(-40, 41) * 0.05f;
						num5 += Main.Rand.Next(-40, 41) * 0.05f;
						vector2.X += num4 * 8f;
						vector2.Y += num5 * 8f;
						Projectile.NewProjectile(vector2.X, vector2.Y, num4, num5, 100, 25, 0f);
					}
				}
			}
		}

		private void DestroyerAI()
		{
			if (AI3 > 0f)
			{
				RealLife = (int)AI3;
			}
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead)
			{
				TargetClosest();
			}
			if (Type > (int)ID.THE_DESTROYER_HEAD)
			{
				bool flag = false;
				if (AI1 <= 0f)
				{
					flag = true;
				}
				else if (Main.NPCSet[(int)AI1].Life <= 0)
				{
					flag = true;
				}
				if (flag)
				{
					Life = 0;
					if (Active != 0)
					{
						HitEffect();
					}
					CheckDead();
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				if (AI0 == 0f && Type == (int)ID.THE_DESTROYER_HEAD)
				{
					AI3 = WhoAmI;
					RealLife = WhoAmI;
					int num = 0;
					int num2 = WhoAmI;
					int num3 = 80;
					for (int i = 0; i <= num3; i++)
					{
						int num4 = (int)ID.THE_DESTROYER_BODY;
						if (i == num3)
						{
							num4 = (int)ID.THE_DESTROYER_TAIL;
						}
						num = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, num4, WhoAmI);
						Main.NPCSet[num].AI3 = WhoAmI;
						Main.NPCSet[num].RealLife = WhoAmI;
						Main.NPCSet[num].AI1 = num2;
						Main.NPCSet[num2].AI0 = num;
						NetMessage.CreateMessage1(23, num);
						NetMessage.SendMessage();
						num2 = num;
					}
				}
				if (Type == (int)ID.THE_DESTROYER_BODY)
				{
					LocalAI0 += Main.Rand.Next(4);
					if (LocalAI0 >= Main.Rand.Next(1400, 26000))
					{
						LocalAI0 = 0;
						TargetClosest();
						if (Collision.CanHit(ref XYWH, ref Main.PlayerSet[Target].XYWH))
						{
							Vector2 vector = new Vector2(Position.X + Width * 0.5f, Position.Y + (Height >> 1));
							float num5 = Main.PlayerSet[Target].Position.X + 10f - vector.X + Main.Rand.Next(-20, 21);
							float num6 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y + Main.Rand.Next(-20, 21);
							float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
							num7 = 8f / num7;
							num5 *= num7;
							num6 *= num7;
							num5 += Main.Rand.Next(-20, 21) * 0.05f;
							num6 += Main.Rand.Next(-20, 21) * 0.05f;
							vector.X += num5 * 5f;
							vector.Y += num6 * 5f;
							int num8 = Projectile.NewProjectile(vector.X, vector.Y, num5, num6, 100, 22, 0f);
							if (num8 >= 0)
							{
								Main.ProjectileSet[num8].timeLeft = 300;
								ShouldNetUpdate = true;
							}
						}
					}
				}
			}
			int num9 = ((int)Position.X >> 4) - 1;
			int num10 = ((int)Position.X + Width >> 4) + 2;
			int num11 = ((int)Position.Y >> 4) - 1;
			int num12 = ((int)Position.Y + Height >> 4) + 2;
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num10 > Main.MaxTilesX)
			{
				num10 = Main.MaxTilesX;
			}
			if (num11 < 0)
			{
				num11 = 0;
			}
			if (num12 > Main.MaxTilesY)
			{
				num12 = Main.MaxTilesY;
			}
			bool flag2 = false;
			if (!flag2)
			{
				Vector2 vector2 = default(Vector2);
				for (int j = num9; j < num10; j++)
				{
					for (int k = num11; k < num12; k++)
					{
						if (Main.TileSet[j, k].CanStandOnTop() || Main.TileSet[j, k].Liquid > 64)
						{
							vector2.X = j * 16;
							vector2.Y = k * 16;
							if (Position.X + Width > vector2.X && Position.X < vector2.X + 16f && Position.Y + Height > vector2.Y && Position.Y < vector2.Y + 16f)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
			}
			if (!flag2)
			{
				if (Type != (int)ID.THE_DESTROYER_BODY || AI2 != 1f)
				{
					Lighting.AddLight((int)Position.X + (Width >> 1) >> 4, (int)Position.Y + (Height >> 1) >> 4, new Vector3(0.3f, 0.1f, 0.05f));
				}
				LocalAI1 = 1;
				if (Type == (int)ID.THE_DESTROYER_HEAD)
				{
					Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
					bool flag3 = true;
					if (Position.Y > Main.PlayerSet[Target].Position.Y)
					{
						for (int l = 0; l < Player.MaxNumPlayers; l++)
						{
							if (Main.PlayerSet[l].Active != 0)
							{
								Rectangle rectangle2 = new Rectangle(Main.PlayerSet[l].XYWH.X - 1000, Main.PlayerSet[l].XYWH.Y - 1000, 2000, 2000);
								if (rectangle.Intersects(rectangle2))
								{
									flag3 = false;
									break;
								}
							}
						}
						if (flag3)
						{
							flag2 = true;
						}
					}
				}
			}
			else
			{
				LocalAI1 = 0;
			}
			float num13 = 16f;
			if (Main.GameTime.DayTime || Main.PlayerSet[Target].IsDead)
			{
				flag2 = false;
				Velocity.Y += 1f;
				if (Position.Y > Main.WorldSurface << 4)
				{
					Velocity.Y += 1f;
					num13 = 32f;
				}
				if (Position.Y > Main.RockLayer << 4)
				{
					for (int m = 0; m < MaxNumNPCs; m++)
					{
						if (Main.NPCSet[m].AIStyle == AIStyle)
						{
							Main.NPCSet[m].Active = 0;
						}
					}
				}
			}
			Vector2 vector3 = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
			float num14;
			float num15;
			if (AI1 > 0f && AI1 < MaxNumNPCs)
			{
				num14 = Main.NPCSet[(int)AI1].Position.X + (Main.NPCSet[(int)AI1].Width >> 1) - vector3.X;
				num15 = Main.NPCSet[(int)AI1].Position.Y + (Main.NPCSet[(int)AI1].Height >> 1) - vector3.Y;
				Rotation = (float)(Math.Atan2(num15, num14) + Math.PI / 2.0);
				float num16 = num14 * num14 + num15 * num15;
				if (num16 > 0f)
				{
					num16 = (float)Math.Sqrt(num16);
					num16 = (num16 - 44f * Scale) / num16;
					num14 *= num16;
					num15 *= num16;
					Position.X += num14;
					Position.Y += num15;
					XYWH.X = (int)Position.X;
					XYWH.Y = (int)Position.Y;
				}
				Velocity.X = 0f;
				Velocity.Y = 0f;
				return;
			}
			num14 = (Main.PlayerSet[Target].XYWH.X + 10) & -16;
			num15 = (Main.PlayerSet[Target].XYWH.Y + 21) & -16;
			vector3.X = (int)vector3.X & -16;
			vector3.Y = (int)vector3.Y & -16;
			num14 -= vector3.X;
			num15 -= vector3.Y;
			if (!flag2)
			{
				TargetClosest();
				Velocity.Y += 0.15f;
				if (Velocity.Y > num13)
				{
					Velocity.Y = num13;
				}
				if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num13 * 0.4)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X -= 0.11f;
					}
					else
					{
						Velocity.X += 0.11f;
					}
				}
				else if (Velocity.Y == num13)
				{
					if (Velocity.X < num14)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X > num14)
					{
						Velocity.X -= 0.1f;
					}
				}
				else if (Velocity.Y > 4f)
				{
					if (Velocity.X < 0f)
					{
						Velocity.X += 0.09f;
					}
					else
					{
						Velocity.X -= 0.09f;
					}
				}
			}
			else
			{
				float num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
				if (SoundDelay == 0)
				{
					float num17 = num16 * 0.025f;
					if (num17 < 10f)
					{
						num17 = 10f;
					}
					else if (num17 > 20f)
					{
						num17 = 20f;
					}
					SoundDelay = (short)num17;
					Main.PlaySound(15, XYWH.X, XYWH.Y);
				}
				float num18 = Math.Abs(num14);
				float num19 = Math.Abs(num15);
				float num20 = num13 / num16;
				num14 *= num20;
				num15 *= num20;
				if (((Velocity.X > 0f && num14 > 0f) || (Velocity.X < 0f && num14 < 0f)) && ((Velocity.Y > 0f && num15 > 0f) || (Velocity.Y < 0f && num15 < 0f)))
				{
					if (Velocity.X < num14)
					{
						Velocity.X += 0.15f;
					}
					else if (Velocity.X > num14)
					{
						Velocity.X -= 0.15f;
					}
					if (Velocity.Y < num15)
					{
						Velocity.Y += 0.15f;
					}
					else if (Velocity.Y > num15)
					{
						Velocity.Y -= 0.15f;
					}
				}
				if ((Velocity.X > 0f && num14 > 0f) || (Velocity.X < 0f && num14 < 0f) || (Velocity.Y > 0f && num15 > 0f) || (Velocity.Y < 0f && num15 < 0f))
				{
					if (Velocity.X < num14)
					{
						Velocity.X += 0.1f;
					}
					else if (Velocity.X > num14)
					{
						Velocity.X -= 0.1f;
					}
					if (Velocity.Y < num15)
					{
						Velocity.Y += 0.1f;
					}
					else if (Velocity.Y > num15)
					{
						Velocity.Y -= 0.1f;
					}
					if ((double)Math.Abs(num15) < (double)num13 * 0.2 && ((Velocity.X > 0f && num14 < 0f) || (Velocity.X < 0f && num14 > 0f)))
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.2f;
						}
						else
						{
							Velocity.Y -= 0.2f;
						}
					}
					if ((double)Math.Abs(num14) < (double)num13 * 0.2 && ((Velocity.Y > 0f && num15 < 0f) || (Velocity.Y < 0f && num15 > 0f)))
					{
						if (Velocity.X > 0f)
						{
							Velocity.X += 0.2f;
						}
						else
						{
							Velocity.X -= 0.2f;
						}
					}
				}
				else if (num18 > num19)
				{
					if (Velocity.X < num14)
					{
						Velocity.X += 0.11f;
					}
					else if (Velocity.X > num14)
					{
						Velocity.X -= 0.11f;
					}
					if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num13 * 0.5)
					{
						if (Velocity.Y > 0f)
						{
							Velocity.Y += 0.1f;
						}
						else
						{
							Velocity.Y -= 0.1f;
						}
					}
				}
				else
				{
					if (Velocity.Y < num15)
					{
						Velocity.Y += 0.11f;
					}
					else if (Velocity.Y > num15)
					{
						Velocity.Y -= 0.11f;
					}
					if ((double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) < (double)num13 * 0.5)
					{
						if (Velocity.X > 0f)
						{
							Velocity.X += 0.1f;
						}
						else
						{
							Velocity.X -= 0.1f;
						}
					}
				}
			}
			Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 1.57f;
			if (Type != (int)ID.THE_DESTROYER_HEAD)
			{
				return;
			}
			if (flag2)
			{
				if (LocalAI0 != 1)
				{
					ShouldNetUpdate = true;
				}
				LocalAI0 = 1;
			}
			else
			{
				if (LocalAI0 != 0)
				{
					ShouldNetUpdate = true;
				}
				LocalAI0 = 0;
			}
			if (((Velocity.X > 0f && OldVelocity.X < 0f) || (Velocity.X < 0f && OldVelocity.X > 0f) || (Velocity.Y > 0f && OldVelocity.Y < 0f) || (Velocity.Y < 0f && OldVelocity.Y > 0f)) && !WasJustHit)
			{
				ShouldNetUpdate = true;
			}
		}

		private void SnowmanAI()
		{
			float num = 4f;
			float num2 = 1f;
			if (Type == (int)ID.SNOWMAN_GANGSTA)
			{
				num = 3f;
				num2 = 0.7f;
			}
			if (Type == (int)ID.SNOW_BALLA)
			{
				num = 3.5f;
				num2 = 0.8f;
			}
			if (Type == (int)ID.SNOWMAN_GANGSTA)
			{
				AI2 += 1f;
				if (AI2 >= 120f)
				{
					AI2 = 0f;
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						Vector2 vector = new Vector2(Position.X + Width * 0.5f - Direction * 12, Position.Y + Height * 0.5f);
						float speedX = 12 * SpriteDirection;
						float speedY = 0f;
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							int num3 = Projectile.NewProjectile(vector.X, vector.Y, speedX, speedY, 110, 25, 0f, 8, send: false);
							if (num3 >= 0)
							{
								Main.ProjectileSet[num3].ai0 = 2f;
								Main.ProjectileSet[num3].timeLeft = 300;
								Main.ProjectileSet[num3].friendly = false;
								NetMessage.SendProjectile(num3);
								ShouldNetUpdate = true;
							}
						}
					}
				}
			}
			if (Type == (int)ID.MISTER_STABBY && AI1 >= 3f)
			{
				TargetClosest();
				SpriteDirection = Direction;
				if (Velocity.Y == 0f)
				{
					Velocity.X *= 0.9f;
					AI2 += 1f;
					if (Velocity.X > -0.3 && Velocity.X < 0.3)
					{
						Velocity.X = 0f;
					}
					if (AI2 >= 200f)
					{
						AI2 = 0f;
						AI1 = 0f;
					}
				}
			}
			else if (Type == (int)ID.SNOW_BALLA && AI1 >= 3f)
			{
				TargetClosest();
				if (Velocity.Y == 0f)
				{
					Velocity.X *= 0.9f;
					AI2 += 1f;
					if (Velocity.X > -0.3 && Velocity.X < 0.3)
					{
						Velocity.X = 0f;
					}
					if (AI2 >= 16f)
					{
						AI2 = 0f;
						AI1 = 0f;
					}
				}
				if (Velocity.X == 0f && Velocity.Y == 0f && AI2 == 8f)
				{
					Vector2 vector2 = new Vector2(Position.X + Width * 0.5f - Direction * 12, Position.Y + Height * 0.25f);
					float num4 = Main.PlayerSet[Target].Position.X + 10f - vector2.X;
					float num5 = Main.PlayerSet[Target].Position.Y - vector2.Y;
					float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
					num6 = 10f / num6;
					num4 *= num6;
					num5 *= num6;
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						int num7 = Projectile.NewProjectile(vector2.X, vector2.Y, num4, num5, 109, 35, 0f);
						if (num7 >= 0)
						{
							Main.ProjectileSet[num7].ai0 = 2f;
							Main.ProjectileSet[num7].timeLeft = 300;
							Main.ProjectileSet[num7].friendly = false;
							NetMessage.SendProjectile(num7);
							ShouldNetUpdate = true;
						}
					}
				}
			}
			else
			{
				if (Velocity.Y == 0f)
				{
					if (LocalAI2 == XYWH.X)
					{
						Direction = (sbyte)(-Direction);
						AI3 = 60f;
					}
					LocalAI2 = XYWH.X;
					if (AI3 == 0f)
					{
						TargetClosest();
					}
					AI0 += 1f;
					if (AI0 > 2f)
					{
						AI0 = 0f;
						AI1 += 1f;
						Velocity.Y = -8.2f;
						Velocity.X += Direction * num2 * 1.1f;
					}
					else
					{
						Velocity.Y = -6f;
						Velocity.X += Direction * num2 * 0.9f;
					}
					SpriteDirection = Direction;
				}
				Velocity.X += Direction * num2 * 0.01f;
			}
			if (AI3 > 0f)
			{
				AI3 -= 1f;
			}
			if (Velocity.X > num && Direction > 0)
			{
				Velocity.X = 4f;
			}
			else if (Velocity.X < 0f - num && Direction < 0)
			{
				Velocity.X = -4f;
			}
		}

		private unsafe void OcramAI()
		{
			Lighting.AddLight(XYWH.X >> 4, XYWH.Y >> 4, new Vector3(1f, 1f, 1f));
			if (Target == Player.MaxNumPlayers || Main.PlayerSet[Target].IsDead || Main.PlayerSet[Target].Active == 0)
			{
				TargetClosest();
			}
			bool dead = Main.PlayerSet[Target].IsDead;
			float num = Position.X + (Width >> 1) - Main.PlayerSet[Target].Position.X - 10f;
			float num2 = Position.Y + Height - 59f - Main.PlayerSet[Target].Position.Y - 21f;
			float num3 = (float)Math.Atan2(num2, num) + 1.57f;
			if (num3 < 0f)
			{
				num3 += 6.283f;
			}
			else if (num3 > 6.283f)
			{
				num3 -= 6.283f;
			}
			float num4 = 0f;
			if (AI0 == 0f && AI1 == 0f)
			{
				num4 = 0.02f;
			}
			if (AI0 == 0f && AI1 == 2f && AI2 > 40f)
			{
				num4 = 0.05f;
			}
			if (AI0 == 3f && AI1 == 0f)
			{
				num4 = 0.05f;
			}
			if (AI0 == 3f && AI1 == 2f && AI2 > 40f)
			{
				num4 = 0.08f;
			}
			if (Rotation < num3)
			{
				if ((double)(num3 - Rotation) > 3.1415)
				{
					Rotation -= num4;
				}
				else
				{
					Rotation += num4;
				}
			}
			else if (Rotation > num3)
			{
				if ((double)(Rotation - num3) > 3.1415)
				{
					Rotation += num4;
				}
				else
				{
					Rotation -= num4;
				}
			}
			if (Rotation > num3 - num4 && Rotation < num3 + num4)
			{
				Rotation = num3;
			}
			if (Rotation < 0f)
			{
				Rotation += 6.283f;
			}
			else if (Rotation > 6.283f)
			{
				Rotation -= 6.283f;
			}
			if (Rotation > num3 - num4 && Rotation < num3 + num4)
			{
				Rotation = num3;
			}
			if (Main.Rand.Next(6) == 0)
			{
				Dust* ptr = Main.DustSet.NewDust(XYWH.X, XYWH.Y + (Height >> 2), Width, Height >> 1, 5, Velocity.X, 2.0);
				if (ptr != null)
				{
					ptr->Velocity.X *= 0.5f;
					ptr->Velocity.Y *= 0.1f;
				}
			}
			if (Main.GameTime.DayTime || dead)
			{
				Velocity.Y -= 0.04f;
				if (TimeLeft > 10)
				{
					TimeLeft = 10;
				}
				return;
			}
			if (AI0 == 0f)
			{
				if (AI1 == 0f)
				{
					float num5 = 8f;
					float num6 = 0.12f;
					Vector2 vector = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
					float num7 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
					float num8 = Main.PlayerSet[Target].Position.Y + 21f - 200f - vector.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					float num10 = num9;
					num9 = num5 / num9;
					num7 *= num9;
					num8 *= num9;
					if (Velocity.X < num7)
					{
						Velocity.X += num6;
						if (Velocity.X < 0f && num7 > 0f)
						{
							Velocity.X += num6;
						}
					}
					else if (Velocity.X > num7)
					{
						Velocity.X -= num6;
						if (Velocity.X > 0f && num7 < 0f)
						{
							Velocity.X -= num6;
						}
					}
					if (Velocity.Y < num8)
					{
						Velocity.Y += num6;
						if (Velocity.Y < 0f && num8 > 0f)
						{
							Velocity.Y += num6;
						}
					}
					else if (Velocity.Y > num8)
					{
						Velocity.Y -= num6;
						if (Velocity.Y > 0f && num8 < 0f)
						{
							Velocity.Y -= num6;
						}
					}
					AI2 += 1f;
					if (AI2 >= 600f)
					{
						AI1 = 1f;
						AI2 = 0f;
						AI3 = 0f;
						Target = Player.MaxNumPlayers;
						ShouldNetUpdate = true;
					}
					else if (XYWH.Y + Height < Main.PlayerSet[Target].XYWH.Y && num10 < 500f)
					{
						if (!Main.PlayerSet[Target].IsDead)
						{
							AI3 += 1f;
						}
						if (AI3 >= 90f)
						{
							TargetClosest();
							num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
							num9 = 9f / num9;
							num7 *= num9;
							num8 *= num9;
							vector.X += num7 * 15f;
							vector.Y += num8 * 15f;
							Projectile.NewProjectile(vector.X, vector.Y, num7, num8, 100, 20, 0f);
						}
						if (AI3 == 60f || AI3 == 70f || AI3 == 80f || AI3 == 90f)
						{
							Rotation = num3;
							float num11 = Main.PlayerSet[Target].Position.X + 10f - vector.X;
							float num12 = Main.PlayerSet[Target].Position.Y + 21f - vector.Y;
							float num13 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
							num13 = 5f / num13;
							Vector2 vector2 = vector;
							Vector2 vector3 = default(Vector2);
							vector3.X = num11 * num13;
							vector3.Y = num12 * num13;
							vector2.X += vector3.X * 10f;
							vector2.Y += vector3.Y * 10f;
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								int num14 = NewNPC((int)vector2.X, (int)vector2.Y, (int)ID.SERVANT_OF_OCRAM);
								if (num14 < MaxNumNPCs)
								{
									Main.NPCSet[num14].Velocity.X = vector3.X;
									Main.NPCSet[num14].Velocity.Y = vector3.Y;
									NetMessage.CreateMessage1(23, num14);
									NetMessage.SendMessage();
								}
							}
							Main.PlaySound(3, (int)vector2.X, (int)vector2.Y);
							for (int i = 0; i < 8; i++)
							{
								if (null == Main.DustSet.NewDust((int)vector2.X, (int)vector2.Y, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f))
								{
									break;
								}
							}
						}
						if (AI3 == 103f)
						{
							AI3 = 0f;
						}
					}
				}
				else if (AI1 == 1f)
				{
					Rotation = num3;
					float num15 = 6f;
					Vector2 vector4 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num16 = Main.PlayerSet[Target].Position.X + 10f - vector4.X;
					float num17 = Main.PlayerSet[Target].Position.Y + 21f - vector4.Y;
					float num18 = (float)Math.Sqrt(num16 * num16 + num17 * num17);
					num18 = num15 / num18;
					Velocity.X = num16 * num18;
					Velocity.Y = num17 * num18;
					AI1 = 2f;
				}
				else if (AI1 == 2f)
				{
					AI2 += 1f;
					if (AI2 >= 40f)
					{
						Velocity.X *= 0.98f;
						Velocity.Y *= 0.98f;
						if (Velocity.X > -0.1 && Velocity.X < 0.1)
						{
							Velocity.X = 0f;
						}
						if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
						{
							Velocity.Y = 0f;
						}
					}
					else
					{
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
					}
					if (AI2 >= 150f)
					{
						AI3 += 1f;
						AI2 = 0f;
						Target = Player.MaxNumPlayers;
						Rotation = num3;
						if (AI3 >= 3f)
						{
							AI1 = 0f;
							AI3 = 0f;
						}
						else
						{
							AI1 = 1f;
						}
					}
				}
				if (Life < LifeMax >> 1)
				{
					AI0 = 1f;
					AI1 = 0f;
					AI2 = 0f;
					AI3 = 0f;
					ShouldNetUpdate = true;
				}
				return;
			}
			if (AI0 == 1f || AI0 == 2f)
			{
				if (AI0 == 1f)
				{
					AI2 += 0.005f;
					if (AI2 > 0.5)
					{
						AI2 = 0.5f;
					}
				}
				else
				{
					AI2 -= 0.005f;
					if (AI2 < 0f)
					{
						AI2 = 0f;
					}
				}
				Rotation += AI2;
				AI1 += 1f;
				if (AI1 == 100f)
				{
					AI0 += 1f;
					AI1 = 0f;
					if (AI0 == 3f)
					{
						AI2 = 0f;
					}
					else
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y);
						for (int j = 0; j < 2; j++)
						{
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 174);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 173);
							Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 172);
						}
						for (int k = 0; k < 16; k++)
						{
							if (null == Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f))
							{
								break;
							}
						}
						Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
					}
				}
				Main.DustSet.NewDust(5, ref XYWH, Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f);
				Velocity.X *= 0.98f;
				Velocity.Y *= 0.98f;
				if (Velocity.X > -0.1 && Velocity.X < 0.1)
				{
					Velocity.X = 0f;
				}
				if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
				{
					Velocity.Y = 0f;
				}
				return;
			}
			Damage = 50;
			Defense = 0;
			if (AI1 == 0f)
			{
				float num19 = 9f;
				float num20 = 0.2f;
				Vector2 vector5 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num21 = Main.PlayerSet[Target].Position.X + 10f - vector5.X;
				float num22 = Main.PlayerSet[Target].Position.Y + 21f - 120f - vector5.Y;
				float num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
				num23 = num19 / num23;
				num21 *= num23;
				num22 *= num23;
				if (Velocity.X < num21)
				{
					Velocity.X += num20;
					if (Velocity.X < 0f && num21 > 0f)
					{
						Velocity.X += num20;
					}
				}
				else if (Velocity.X > num21)
				{
					Velocity.X -= num20;
					if (Velocity.X > 0f && num21 < 0f)
					{
						Velocity.X -= num20;
					}
				}
				if (Velocity.Y < num22)
				{
					Velocity.Y += num20;
					if (Velocity.Y < 0f && num22 > 0f)
					{
						Velocity.Y += num20;
					}
				}
				else if (Velocity.Y > num22)
				{
					Velocity.Y -= num20;
					if (Velocity.Y > 0f && num22 < 0f)
					{
						Velocity.Y -= num20;
					}
				}
				AI2 += 1f;
				if (AI2 >= 100f)
				{
					if (AI2 >= 200f)
					{
						AI1 = 1f;
						AI2 = 0f;
						AI3 = 0f;
						Target = Player.MaxNumPlayers;
						ShouldNetUpdate = true;
					}
					num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
					num23 = 9f / num23;
					num21 *= num23;
					num22 *= num23;
					num21 += Main.Rand.Next(-40, 41) * 0.08f;
					num22 += Main.Rand.Next(-40, 41) * 0.08f;
					vector5.X += num21 * 15f;
					vector5.Y += num22 * 15f;
					Projectile.NewProjectile(vector5.X, vector5.Y, num21, num22, 83, 45, 0f);
				}
			}
			else if (AI1 == 1f)
			{
				Main.PlaySound(15, (int)Position.X, (int)Position.Y, 0);
				Rotation = num3;
				float num24 = 6.8f;
				Vector2 vector6 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
				float num25 = Main.PlayerSet[Target].Position.X + 10f - vector6.X;
				float num26 = Main.PlayerSet[Target].Position.Y + 21f - vector6.Y;
				float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
				num27 = num24 / num27;
				Velocity.X = num25 * num27;
				Velocity.Y = num26 * num27;
				if (AI1 == 1f)
				{
					num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
					num27 = 6f / num27;
					num25 *= num27;
					num26 *= num27;
					num25 += Main.Rand.Next(-40, 41) * 0.08f;
					num26 += Main.Rand.Next(-40, 41) * 0.08f;
					for (int l = 1; l <= 10; l++)
					{
						vector6.X += Main.Rand.Next(-50, 50) * 2f;
						vector6.Y += Main.Rand.Next(-50, 50) * 2f;
						Projectile.NewProjectile(vector6.X, vector6.Y, num25, num26, 44, 45, 0f);
					}
				}
				AI1 = 2f;
			}
			else
			{
				if (AI1 != 2f)
				{
					return;
				}
				AI2 += 1f;
				if (AI2 >= 40f)
				{
					Velocity.X *= 1f;
					Velocity.Y *= 1f;
					if (Velocity.X > -0.1 && Velocity.X < 0.1)
					{
						Velocity.X = 0f;
					}
					if (Velocity.Y > -0.1 && Velocity.Y < 0.1)
					{
						Velocity.Y = 0f;
					}
					if (AI2 >= 135f)
					{
						AI3 += 1f;
						AI2 = 0f;
						Target = Player.MaxNumPlayers;
						Rotation = num3;
						if (AI3 >= 3f)
						{
							AI1 = 0f;
							AI3 = 0f;
						}
						else
						{
							AI1 = 1f;
						}
					}
					if (AI2 != 110f && AI2 != 100f && AI2 != 130f && AI2 != 120f)
					{
						return;
					}
					Rotation = num3;
					Vector2 vector7 = new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f);
					float num28 = Main.PlayerSet[Target].Position.X + 10f - vector7.X;
					float num29 = Main.PlayerSet[Target].Position.Y + 21f - vector7.Y;
					float num30 = (float)Math.Sqrt(num28 * num28 + num29 * num29);
					num30 = 5f / num30;
					Vector2 vector8 = vector7;
					Vector2 vector9 = default(Vector2);
					vector9.X = num28 * num30;
					vector9.Y = num29 * num30;
					vector8.X += vector9.X * 10f;
					vector8.Y += vector9.Y * 10f;
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						int num31 = NewNPC((int)vector8.X, (int)vector8.Y, (int)ID.SERVANT_OF_OCRAM);
						if (num31 < MaxNumNPCs)
						{
							Main.NPCSet[num31].Velocity.X = vector9.X;
							Main.NPCSet[num31].Velocity.Y = vector9.Y;
							NetMessage.CreateMessage1(23, num31);
							NetMessage.SendMessage();
						}
					}
				}
				else
				{
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) - 1.57f;
				}
			}
		}

		public void FindFrame()
		{
			int State = 0;
			if (AIAction == 0)
			{
				State = ((Velocity.Y < 0f) ? 2 : ((Velocity.Y > 0f) ? 3 : ((Velocity.X != 0f) ? 1 : 0)));
			}
			else if (AIAction == 1)
			{
				State = 4;
			}
			if (Type == (int)ID.SLIME || Type == (int)ID.MOTHER_SLIME || Type == (int)ID.LAVA_SLIME || Type == (int)ID.DUNGEON_SLIME || Type == (int)ID.CORRUPT_SLIME || Type == (int)ID.SHADOW_SLIME || Type == (int)ID.ILLUMINANT_SLIME)
			{
				FrameCounter += 1f;
				if (State > 0)
				{
					FrameCounter += 1f;
				}
				if (State == 4)
				{
					FrameCounter += 1f;
				}
				if (FrameCounter >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.TOXIC_SLUDGE)
			{
				SpriteDirection = Direction;
				if (Velocity.Y != 0f)
				{
					FrameY = (short)(FrameHeight << 1);
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY > FrameHeight)
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.SNOWMAN_GANGSTA)
			{
				if (Velocity.Y > 0f)
				{
					FrameCounter += 1f;
				}
				else if (Velocity.Y < 0f)
				{
					FrameCounter -= 1f;
				}
				if (FrameCounter < 6f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 12f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight * 3);
				}
				if (FrameCounter < 0f)
				{
					FrameCounter = 0f;
				}
				if (FrameCounter > 17f)
				{
					FrameCounter = 17f;
				}
			}
			else if (Type == (int)ID.MISTER_STABBY)
			{
				if (Velocity.X == 0f && Velocity.Y == 0f)
				{
					LocalAI3++;
					if (LocalAI3 < 6)
					{
						FrameY = 0;
					}
					else if (LocalAI3 < 12)
					{
						FrameY = FrameHeight;
					}
					if (LocalAI3 >= 11)
					{
						LocalAI3 = 0;
					}
					return;
				}
				if (Velocity.Y > 0f)
				{
					FrameCounter += 1f;
				}
				else if (Velocity.Y < 0f)
				{
					FrameCounter -= 1f;
				}
				if (FrameCounter < 6f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 12f)
				{
					FrameY = (short)(FrameHeight * 3);
				}
				else if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight << 2);
				}
				if (FrameCounter < 0f)
				{
					FrameCounter = 0f;
				}
				else if (FrameCounter > 17f)
				{
					FrameCounter = 17f;
				}
			}
			else if (Type == (int)ID.SNOW_BALLA)
			{
				if (Velocity.X == 0f && Velocity.Y == 0f)
				{
					if (AI2 < 4f)
					{
						FrameY = 0;
					}
					else if (AI2 < 8f)
					{
						FrameY = FrameHeight;
					}
					else if (AI2 < 12f)
					{
						FrameY = (short)(FrameHeight << 1);
					}
					else if (AI2 < 16f)
					{
						FrameY = (short)(FrameHeight * 3);
					}
					return;
				}
				if (Velocity.Y > 0f)
				{
					FrameCounter += 1f;
				}
				else if (Velocity.Y < 0f)
				{
					FrameCounter -= 1f;
				}
				if (FrameCounter < 6f)
				{
					FrameY = (short)(FrameHeight << 2);
				}
				else if (FrameCounter < 12f)
				{
					FrameY = (short)(FrameHeight * 5);
				}
				else if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight * 6);
				}
				if (FrameCounter < 0f)
				{
					FrameCounter = 0f;
				}
				if (FrameCounter > 17f)
				{
					FrameCounter = 17f;
				}
			}
			else if (Type == (int)ID.KING_SLIME)
			{
				if (Velocity.Y != 0f)
				{
					FrameY = (short)(FrameHeight << 2);
					return;
				}
				FrameCounter += 1f;
				if (State > 0)
				{
					FrameCounter += 1f;
				}
				if (State == 4)
				{
					FrameCounter += 1f;
				}
				if (FrameCounter >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * 4)
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.THE_DESTROYER_BODY)
			{
				if (AI2 == 0f)
				{
					FrameY = 0;
				}
				else
				{
					FrameY = FrameHeight;
				}
			}
			else if (Type == (int)ID.MIMIC)
			{
				if (AI0 == 0f)
				{
					FrameCounter = 0f;
					FrameY = 0;
				}
				else
				{
					if (Velocity.Y == 0f)
					{
						FrameCounter -= 1f;
					}
					else
					{
						FrameCounter += 1f;
					}
					if (FrameCounter < 0f)
					{
						FrameCounter = 0f;
					}
					else if (FrameCounter > 12f)
					{
						FrameCounter = 12f;
					}
					if (FrameCounter < 3f)
					{
						FrameY = FrameHeight;
					}
					else if (FrameCounter < 6f)
					{
						FrameY = (short)(FrameHeight << 1);
					}
					else if (FrameCounter < 9f)
					{
						FrameY = (short)(FrameHeight * 3);
					}
					else if (FrameCounter < 12f)
					{
						FrameY = (short)(FrameHeight << 2);
					}
					else if (FrameCounter < 15f)
					{
						FrameY = (short)(FrameHeight * 5);
					}
					else if (FrameCounter < 18f)
					{
						FrameY = (short)(FrameHeight << 2);
					}
					else if (FrameCounter < 21f)
					{
						FrameY = (short)(FrameHeight * 3);
					}
					else
					{
						FrameY = (short)(FrameHeight << 1);
						if (FrameCounter >= 24f)
						{
							FrameCounter = 3f;
						}
					}
				}
				if (AI3 == 2f)
				{
					FrameY = (short)(FrameY + FrameHeight * 6);
				}
				else if (AI3 == 3f)
				{
					FrameY = (short)(FrameY + FrameHeight * 12);
				}
			}
			else if (Type == (int)ID.WALL_OF_FLESH || Type == (int)ID.WALL_OF_FLESH_EYE)
			{
				if (AI2 == 0f)
				{
					FrameCounter += 1f;
					if (FrameCounter >= 12f)
					{
						FrameY += FrameHeight;
						FrameCounter = 0f;
					}
					if (FrameY >= FrameHeight * NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
				else
				{
					FrameY = 0;
					FrameCounter = -60f;
				}
			}
			else if (Type == (int)ID.VULTURE)
			{
				SpriteDirection = Direction;
				Rotation = Velocity.X * 0.1f;
				if (Velocity.X == 0f && Velocity.Y == 0f)
				{
					FrameY = 0;
					FrameCounter = 0f;
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter < 4f)
				{
					FrameY = FrameHeight;
					return;
				}
				FrameY = (short)(FrameHeight << 1);
				if (FrameCounter >= 7f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.GASTROPOD || Type == (int)ID.SPECTRAL_GASTROPOD)
			{
				SpriteDirection = Direction;
				Rotation = Velocity.X * 0.05f;
				if (AI3 > 0f)
				{
					FrameCounter = 0f;
					FrameY = (short)((((int)AI3 >> 3) + 3) * FrameHeight);
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * 3)
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.BIRD)
			{
				SpriteDirection = Direction;
				Rotation = Velocity.X * 0.1f;
				if (Velocity.X == 0f && Velocity.Y == 0f)
				{
					FrameY = (short)(FrameHeight << 2);
					FrameCounter = 0f;
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 4f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.DEMON || Type == (int)ID.ARCH_DEMON || Type == (int)ID.VOODOO_DEMON)
			{
				SpriteDirection = Direction;
				Rotation = Velocity.X * 0.1f;
				FrameCounter += 1f;
				if (FrameCounter < 6f)
				{
					FrameY = 0;
					return;
				}
				FrameY = FrameHeight;
				if (FrameCounter >= 11f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.BLUE_JELLYFISH || Type == (int)ID.PINK_JELLYFISH || Type == (int)ID.GREEN_JELLYFISH)
			{
				FrameCounter += 1f;
				if (FrameCounter < 6f)
				{
					FrameY = 0;
					return;
				}
				if (FrameCounter < 12f)
				{
					FrameY = FrameHeight;
					return;
				}
				if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight << 1);
					return;
				}
				FrameY = (short)(FrameHeight * 3);
				if (FrameCounter >= 23f)
				{
					FrameCounter = 0f;
				}
			}
#if VERSION_INITIAL
			else if (Type == (int)ID.DEMON_EYE || Type == (int)ID.METEOR_HEAD || Type == (int)ID.SLIMER)
			{
				if (Type == (int)ID.DEMON_EYE)
#else
			else if (Type == (int)ID.DEMON_EYE || Type == (int)ID.METEOR_HEAD || Type == (int)ID.SLIMER || Type == (int)ID.CATARACT_EYE || Type == (int)ID.SLEEPY_EYE || Type == (int)ID.DIALATED_EYE || Type == (int)ID.GREEN_EYE || Type == (int)ID.PURPLE_EYE)
			{
				if (Type == (int)ID.DEMON_EYE || Type == (int)ID.CATARACT_EYE || Type == (int)ID.SLEEPY_EYE || Type == (int)ID.DIALATED_EYE || Type == (int)ID.GREEN_EYE || Type == (int)ID.PURPLE_EYE)
#endif
				{
					if (Velocity.X > 0f)
					{
						SpriteDirection = 1;
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
					}
					if (Velocity.X < 0f)
					{
						SpriteDirection = -1;
						Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 3.14f;
					}
				}
				else if (Type == (int)ID.SLIMER)
				{
					if (Velocity.X > 0f)
					{
						SpriteDirection = 1;
					}
					if (Velocity.X < 0f)
					{
						SpriteDirection = -1;
					}
					Rotation = Velocity.X * 0.1f;
				}
				if ((FrameCounter += 1f) >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.WANDERING_EYE)
			{
				if (Velocity.X > 0f)
				{
					SpriteDirection = 1;
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
				}
				if (Velocity.X < 0f)
				{
					SpriteDirection = -1;
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 3.14f;
				}
				if ((FrameCounter += 1f) >= 8f)
				{
					FrameY = FrameHeight;
				}
				else
				{
					FrameY = 0;
				}
				if (FrameCounter >= 16f)
				{
					FrameY = 0;
					FrameCounter = 0f;
				}
				if (Life < LifeMax * 0.5)
				{
					FrameY = (short)(FrameY + (FrameHeight << 1));
				}
			}
			else if (Type == (int)ID.THE_HUNGRY_II)
			{
				if (Velocity.X > 0f)
				{
					SpriteDirection = 1;
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
				}
				if (Velocity.X < 0f)
				{
					SpriteDirection = -1;
					Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X) + 3.14f;
				}
				if ((FrameCounter += 1f) >= 5f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.PIXIE)
			{
				if (Velocity.X > 0f)
				{
					SpriteDirection = 1;
				}
				else
				{
					SpriteDirection = -1;
				}
				Rotation = Velocity.X * 0.1f;
				if ((FrameCounter += 1f) >= 4f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.GOLDFISH || Type == (int)ID.CORRUPT_GOLDFISH || Type == (int)ID.PIRANHA || Type == (int)ID.ANGLER_FISH)
			{
				SpriteDirection = Direction;
				FrameCounter += 1f;
				if (IsWet)
				{
					if (FrameCounter < 6f)
					{
						FrameY = 0;
					}
					else if (FrameCounter < 12f)
					{
						FrameY = FrameHeight;
					}
					else if (FrameCounter < 18f)
					{
						FrameY = (short)(FrameHeight << 1);
					}
					else if (FrameCounter < 24f)
					{
						FrameY = (short)(FrameHeight * 3);
					}
					else
					{
						FrameCounter = 0f;
					}
				}
				else if (FrameCounter < 6f)
				{
					FrameY = (short)(FrameHeight << 2);
				}
				else if (FrameCounter < 12f)
				{
					FrameY = (short)(FrameHeight * 5);
				}
				else
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.ANTLION || Type == (int)ID.ALBINO_ANTLION)
			{
				if (AI0 < 190f)
				{
					if ((FrameCounter += 1f) >= 6f)
					{
						FrameCounter = 0f;
						FrameY += FrameHeight;
						if (FrameY / FrameHeight >= NpcFrameCount[Type] - 1)
						{
							FrameY = 0;
						}
					}
				}
				else
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight * (NpcFrameCount[Type] - 1));
				}
			}
			else if (Type == (int)ID.UNICORN)
			{
				if (Velocity.Y == 0f || IsWet)
				{
					if (Velocity.X < -2f)
					{
						SpriteDirection = -1;
					}
					else if (Velocity.X > 2f)
					{
						SpriteDirection = 1;
					}
					else
					{
						SpriteDirection = Direction;
					}
				}
				if (Velocity.Y != 0f)
				{
					FrameY = (short)(FrameHeight * 15);
					FrameCounter = 0f;
					return;
				}
				if (Velocity.X == 0f)
				{
					FrameCounter = 0f;
					FrameY = 0;
					return;
				}
				if (Math.Abs(Velocity.X) < 3f)
				{
					FrameCounter += Math.Abs(Velocity.X);
					if (FrameCounter >= 6f)
					{
						FrameCounter = 0f;
						FrameY += FrameHeight;
						if (FrameY / FrameHeight >= 9)
						{
							FrameY = FrameHeight;
						}
						if (FrameY / FrameHeight <= 0)
						{
							FrameY = FrameHeight;
						}
					}
					return;
				}
				FrameCounter += Math.Abs(Velocity.X);
				if (FrameCounter >= 10f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= 15)
					{
						FrameY = (short)(FrameHeight * 9);
					}
					if (FrameY / FrameHeight <= 8)
					{
						FrameY = (short)(FrameHeight * 9);
					}
				}
			}
			else if (Type == (int)ID.SKELETRON_PRIME)
			{
				if (AI1 == 0f)
				{
					FrameCounter += 1f;
					if (FrameCounter >= 12f)
					{
						FrameCounter = 0f;
						FrameY += FrameHeight;
						if (FrameY / FrameHeight >= 2)
						{
							FrameY = 0;
						}
					}
				}
				else
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight << 1);
				}
			}
			else if (Type == (int)ID.PRIME_SAW)
			{
				if (Velocity.Y == 0f)
				{
					SpriteDirection = Direction;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 2f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.PRIME_VICE)
			{
				if (Velocity.Y == 0f)
				{
					SpriteDirection = Direction;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 8f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.CRAB)
			{
				if (Velocity.Y == 0f)
				{
					SpriteDirection = Direction;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 6f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.CLOWN)
			{
				if (Velocity.Y == 0f && ((Velocity.X <= 0f && Direction < 0) || (Velocity.X >= 0f && Direction > 0)))
				{
					SpriteDirection = Direction;
				}
				FrameCounter += Math.Abs(Velocity.X);
				if (FrameCounter >= 7f)
				{
					FrameCounter -= 7f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.CURSED_HAMMER || Type == (int)ID.ENCHANTED_SWORD || Type == (int)ID.SHADOW_HAMMER)
			{
				if (AI0 == 2f)
				{
					FrameCounter = 0f;
					FrameY = 0;
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter >= 4f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.BLAZING_WHEEL)
			{
				FrameCounter += 1f;
				if (FrameCounter >= 3f)
				{
					FrameCounter = 0f;
					FrameY += FrameHeight;
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
			}
			else if (Type == (int)ID.SHARK || Type == (int)ID.ORKA)
			{
				SpriteDirection = Direction;
				FrameCounter += 1f;
				if (IsWet)
				{
					if (FrameCounter < 6f)
					{
						FrameY = 0;
					}
					else if (FrameCounter < 12f)
					{
						FrameY = FrameHeight;
					}
					else if (FrameCounter < 18f)
					{
						FrameY = (short)(FrameHeight << 1);
					}
					else if (FrameCounter < 24f)
					{
						FrameY = (short)(FrameHeight * 3);
					}
					else
					{
						FrameCounter = 0f;
					}
				}
			}
			else if (Type == (int)ID.HARPY || Type == (int)ID.CAVE_BAT || Type == (int)ID.JUNGLE_BAT || Type == (int)ID.HELLBAT || Type == (int)ID.WRAITH || Type == (int)ID.GIANT_BAT || Type == (int)ID.ILLUMINANT_BAT)
			{
				if (Velocity.X > 0f)
				{
					SpriteDirection = 1;
				}
				if (Velocity.X < 0f)
				{
					SpriteDirection = -1;
				}
				Rotation = Velocity.X * 0.1f;
				FrameCounter += 1f;
				if (FrameCounter >= 6f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * 4)
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET)
			{
				FrameCounter += 1f;
				if (FrameCounter < 2f)
				{
					FrameY = 0;
				}
				else if (FrameCounter < 4f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 6f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 8f)
				{
					FrameY = FrameHeight;
				}
				else
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.MAN_EATER || Type == (int)ID.SNATCHER || Type == (int)ID.DRAGON_SNATCHER)
			{
				FrameCounter += 1f;
				if (FrameCounter < 6f)
				{
					FrameY = 0;
				}
				else if (FrameCounter < 12f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 24f)
				{
					FrameY = FrameHeight;
				}
				if (FrameCounter == 23f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.THE_HUNGRY)
			{
				FrameCounter += 1f;
				if (FrameCounter < 3f)
				{
					FrameY = 0;
				}
				else if (FrameCounter < 6f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 12f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 15f)
				{
					FrameY = FrameHeight;
				}
				if (FrameCounter == 15f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.CLINGER)
			{
				FrameCounter += 1f;
				if (FrameCounter > 6f)
				{
					FrameY = (short)(FrameY + (FrameHeight << 1));
					FrameCounter = 0f;
				}
				if (FrameY > FrameHeight * 2)
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.MERCHANT || Type == (int)ID.NURSE || Type == (int)ID.ARMS_DEALER || Type == (int)ID.DRYAD || Type == (int)ID.GUIDE || Type == (int)ID.SANTA_CLAUS || Type == (int)ID.DEMOLITIONIST || Type == (int)ID.GOBLIN_PEON || Type == (int)ID.GOBLIN_THIEF || Type == (int)ID.GOBLIN_WARRIOR || Type == (int)ID.BONES || Type == (int)ID.SKELETON || Type == (int)ID.UNDEAD_MINER || Type == (int)ID.CLOTHIER || Type == (int)ID.OLD_MAN || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.WEREWOLF || Type == (int)ID.GOBLIN_TINKERER || Type == (int)ID.WIZARD || Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.MECHANIC || Type == (int)ID.POSSESSED_ARMOR || Type == (int)ID.VAMPIRE_MINER || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_MUMMY)
			{
				if (Velocity.Y == 0f)
				{
					if (Direction == 1)
					{
						SpriteDirection = 1;
					}
					else if (Direction == -1)
					{
						SpriteDirection = -1;
					}
					if (Velocity.X == 0f)
					{
						if (Type == (int)ID.POSSESSED_ARMOR)
						{
							FrameY = FrameHeight;
						}
						else
						{
							FrameY = 0;
						}
						FrameCounter = 0f;
						return;
					}
					FrameCounter += Math.Abs(Velocity.X) * 2f;
					if ((FrameCounter += 1f) > 6f)
					{
						FrameY += FrameHeight;
						FrameCounter = 0f;
					}
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = (short)(FrameHeight << 1);
					}
				}
				else
				{
					FrameCounter = 0f;
					if (Type == (int)ID.SKELETON || Type == (int)ID.BONES || Type == (int)ID.UNDEAD_MINER || Type == (int)ID.VAMPIRE_MINER || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.POSSESSED_ARMOR || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_MUMMY)
					{
						FrameY = 0;
					}
					else
					{
						FrameY = FrameHeight;
					}
				}
			}
			else if (Type == (int)ID.SKELETON_ARCHER)
			{
				if (Velocity.Y == 0f)
				{
					if (Direction != 0)
					{
						SpriteDirection = Direction;
					}
					if (AI2 > 0f)
					{
						SpriteDirection = Direction;
						FrameY = (short)(FrameHeight * (int)AI2);
						FrameCounter = 0f;
						return;
					}
					if (FrameY < FrameHeight * 6)
					{
						FrameY = (short)(FrameHeight * 6);
					}
					FrameCounter += Math.Abs(Velocity.X) * 2f;
					FrameCounter += Velocity.X;
					if (FrameCounter > 6f)
					{
						FrameY += FrameHeight;
						FrameCounter = 0f;
					}
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = (short)(FrameHeight * 6);
					}
				}
				else
				{
					FrameCounter = 0f;
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.GOBLIN_ARCHER)
			{
				if (Velocity.Y == 0f)
				{
					if (Direction != 0)
					{
						SpriteDirection = Direction;
					}
					if (AI2 > 0f)
					{
						SpriteDirection = Direction;
						FrameY = (short)(FrameHeight * ((int)AI2 - 1));
						FrameCounter = 0f;
						return;
					}
					if (FrameY < FrameHeight * 7)
					{
						FrameY = (short)(FrameHeight * 7);
					}
					FrameCounter += Math.Abs(Velocity.X) * 2f;
					FrameCounter += Velocity.X * 1.3f;
					if (FrameCounter > 6f)
					{
						FrameY += FrameHeight;
						FrameCounter = 0f;
					}
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = (short)(FrameHeight * 7);
					}
				}
				else
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight * 6);
				}
			}
#if VERSION_INITIAL
			else if (Type == (int)ID.ZOMBIE || Type == (int)ID.DOCTOR_BONES || Type == (int)ID.THE_GROOM || Type == (int)ID.BALD_ZOMBIE)
#else
			else if (Type == (int)ID.ZOMBIE || Type == (int)ID.DOCTOR_BONES || Type == (int)ID.THE_GROOM || Type == (int)ID.BALD_ZOMBIE || Type == (int)ID.PINCUSHION_ZOMBIE || Type == (int)ID.SLIMED_ZOMBIE || Type == (int)ID.SWAMP_ZOMBIE || Type == (int)ID.TWIGGY_ZOMBIE || Type == (int)ID.FEMALE_ZOMBIE || Type == (int)ID.ZOMBIE_MUSHROOM || Type == (int)ID.ZOMBIE_MUSHROOM_HAT)
#endif
			{
				if (Velocity.Y == 0f && Direction != 0)
				{
					SpriteDirection = Direction;
				}
				if (Velocity.Y != 0f || (Direction == -1 && Velocity.X > 0f) || (Direction == 1 && Velocity.X < 0f))
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight << 1);
					return;
				}
				if (Velocity.X == 0f)
				{
					FrameCounter = 0f;
					FrameY = 0;
					return;
				}
				FrameCounter += Math.Abs(Velocity.X);
				if (FrameCounter < 8f)
				{
					FrameY = 0;
				}
				else if (FrameCounter < 16f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 24f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else if (FrameCounter < 32f)
				{
					FrameY = FrameHeight;
				}
				else
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.BUNNY || Type == (int)ID.CORRUPT_BUNNY)
			{
				if (Velocity.Y == 0f)
				{
					if (Direction != 0)
					{
						SpriteDirection = Direction;
					}
					if (Velocity.X == 0f)
					{
						FrameY = 0;
						FrameCounter = 0f;
						return;
					}
					FrameCounter += Math.Abs(Velocity.X);
					FrameCounter += 1f;
					if (FrameCounter > 6f)
					{
						FrameY += FrameHeight;
						FrameCounter = 0f;
					}
					if (FrameY / FrameHeight >= NpcFrameCount[Type])
					{
						FrameY = 0;
					}
				}
				else if (Velocity.Y < 0f)
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight << 2);
				}
				else if (Velocity.Y > 0f)
				{
					FrameCounter = 0f;
					FrameY = (short)(FrameHeight * 6);
				}
			}
			else if (Type == (int)ID.EYE_OF_CTHULHU || Type == (int)ID.OCRAM || Type == (int)ID.RETINAZER || Type == (int)ID.SPAZMATISM)
			{
				if ((FrameCounter += 1f) < 7f)
				{
					FrameY = 0;
				}
				else if (FrameCounter < 14f)
				{
					FrameY = FrameHeight;
				}
				else if (FrameCounter < 21f)
				{
					FrameY = (short)(FrameHeight << 1);
				}
				else
				{
					FrameCounter = 0f;
					FrameY = 0;
				}
				if (AI0 > 1f)
				{
					FrameY = (short)(FrameY + FrameHeight * 3);
				}
			}
			else if (Type == (int)ID.SERVANT_OF_CTHULHU || Type == (int)ID.SERVANT_OF_OCRAM)
			{
				if ((FrameCounter += 1f) >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.CORRUPTOR)
			{
				if ((FrameCounter += 1f) < 6f)
				{
					FrameY = 0;
					return;
				}
				if (FrameCounter < 12f)
				{
					FrameY = FrameHeight;
					return;
				}
				if (FrameCounter < 18f)
				{
					FrameY = (short)(FrameHeight << 1);
					return;
				}
				FrameY = FrameHeight;
				if (FrameCounter >= 23f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.EATER_OF_SOULS)
			{
				FrameCounter += 1f;
				if (FrameCounter >= 8f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
			else if (Type == (int)ID.FIRE_IMP)
			{
				if (Velocity.Y == 0f && Direction != 0)
				{
					SpriteDirection = Direction;
				}
				if (AI1 > 0f)
				{
					if (FrameY < 4)
					{
						FrameCounter = 0f;
					}
					FrameCounter += 1f;
					if (FrameCounter <= 4f)
					{
						FrameY = (short)(FrameHeight << 2);
						return;
					}
					if (FrameCounter <= 8f)
					{
						FrameY = (short)(FrameHeight * 5);
						return;
					}
					if (FrameCounter <= 12f)
					{
						FrameY = (short)(FrameHeight * 6);
						return;
					}
					if (FrameCounter <= 16f)
					{
						FrameY = (short)(FrameHeight * 7);
						return;
					}
					if (FrameCounter <= 20f)
					{
						FrameY = (short)(FrameHeight << 3);
						return;
					}
					FrameY = (short)(FrameHeight * 9);
					FrameCounter = 100f;
					return;
				}
				FrameCounter += 1f;
				if (FrameCounter <= 4f)
				{
					FrameY = 0;
					return;
				}
				if (FrameCounter <= 8f)
				{
					FrameY = FrameHeight;
					return;
				}
				if (FrameCounter <= 12f)
				{
					FrameY = (short)(FrameHeight << 1);
					return;
				}
				FrameY = (short)(FrameHeight * 3);
				if (FrameCounter >= 16f)
				{
					FrameCounter = 0f;
				}
			}
			else if (Type == (int)ID.GOBLIN_SORCERER || Type == (int)ID.DARK_CASTER || Type == (int)ID.TIM)
			{
				if (Velocity.Y == 0f && Direction != 0)
				{
					SpriteDirection = Direction;
				}
				FrameY = 0;
				if (Velocity.Y != 0f)
				{
					FrameY += FrameHeight;
				}
				else if (AI1 > 0f)
				{
					FrameY = (short)(FrameY + (FrameHeight << 1));
				}
			}
			else if (Type == (int)ID.CURSED_SKULL || Type == (int)ID.DRAGON_SKULL)
			{
				if ((FrameCounter += 1f) >= 4f)
				{
					FrameY += FrameHeight;
					FrameCounter = 0f;
				}
				if (FrameY >= FrameHeight * NpcFrameCount[Type])
				{
					FrameY = 0;
				}
			}
		}

		public void TargetClosest(bool ShouldFaceTarget = true)
		{
			int AbsPos = -1;
			Target = 0;
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (Main.PlayerSet[PlayerIdx].Active != 0 && !Main.PlayerSet[PlayerIdx].IsDead && (AbsPos == -1 || Math.Abs(Main.PlayerSet[PlayerIdx].XYWH.X + 10 - XYWH.X + (Width >> 1)) + Math.Abs(Main.PlayerSet[PlayerIdx].XYWH.Y + 21 - XYWH.Y + (Height >> 1)) < AbsPos))
				{
					AbsPos = Math.Abs(Main.PlayerSet[PlayerIdx].XYWH.X + 10 - XYWH.X + (Width >> 1)) + Math.Abs(Main.PlayerSet[PlayerIdx].XYWH.Y + 21 - XYWH.Y + (Height >> 1));
					Target = (byte)PlayerIdx;
				}
			}
			TargetRect = Main.PlayerSet[Target].XYWH;
			if (Main.PlayerSet[Target].IsDead)
			{
				ShouldFaceTarget = false;
			}
			if (ShouldFaceTarget)
			{
				Direction = 1;
				if (TargetRect.X + (TargetRect.Width >> 1) < XYWH.X + (Width >> 1))
				{
					Direction = -1;
				}
				DirectionY = 1;
				if (TargetRect.Y + (TargetRect.Height >> 1) < XYWH.Y + (Height >> 1))
				{
					DirectionY = -1;
				}
			}
			if (IsConfused)
			{
				Direction *= -1;
			}
			if ((Direction != OldDirection || DirectionY != OldDirectionY || Target != OldTarget) && !HasXCollision && !HasYCollision)
			{
				ShouldNetUpdate = true;
			}
		}

		public void CheckActive()
		{
			if (Active == 0 || Type == (int)ID.DEVOURER_BODY || Type == (int)ID.DEVOURER_TAIL || Type == (int)ID.GIANT_WORM_BODY || Type == (int)ID.GIANT_WORM_TAIL || Type == (int)ID.EATER_OF_WORLDS_BODY || Type == (int)ID.EATER_OF_WORLDS_TAIL || Type == (int)ID.BONE_SERPENT_BODY || Type == (int)ID.BONE_SERPENT_TAIL || Type == (int)ID.DIGGER_BODY || Type == (int)ID.DIGGER_TAIL || Type == (int)ID.SEEKER_BODY || Type == (int)ID.SEEKER_TAIL || (Type > (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type > (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL) || Type == (int)ID.LEECH_BODY || Type == (int)ID.LEECH_TAIL || Type == (int)ID.WALL_OF_FLESH || Type == (int)ID.WALL_OF_FLESH_EYE || Type == (int)ID.THE_HUNGRY || (Type >= (int)ID.THE_DESTROYER_HEAD && Type <= (int)ID.THE_DESTROYER_TAIL))
			{
				return;
			}
			if (IsTownNPC)
			{
				Rectangle TownArea = new Rectangle(XYWH.X + (Width >> 1) - TownRangeX, XYWH.Y + (Height >> 1) - TownRangeY, TownRangeX * 2, TownRangeY * 2);
				for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
				{
					if (Main.PlayerSet[PlayerIdx].Active != 0 && TownArea.Intersects(Main.PlayerSet[PlayerIdx].XYWH))
					{
						Main.PlayerSet[PlayerIdx].TownNPCs += NpcSlots;
					}
				}
				return;
			}
			bool Director = false;
			Rectangle ActiveArea = new Rectangle(XYWH.X + (Width >> 1) - ActiveRangeX, XYWH.Y + (Height >> 1) - ActiveRangeY, ActiveRangeX * 2, ActiveRangeY * 2);
			Rectangle SpawnArea = new Rectangle(XYWH.X + (Width >> 1) - (SpawnWidth / 2) - Width, XYWH.Y + (Height >> 1) - (SpawnHeight / 2) - Height, SpawnWidth + Width * 2, SpawnHeight + Height * 2);
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (Main.PlayerSet[PlayerIdx].Active == 0)
				{
					continue;
				}
				if (ActiveArea.Intersects(Main.PlayerSet[PlayerIdx].XYWH))
				{
					Director = true;
					if (Type != (int)ID.BURNING_SPHERE && Type != (int)ID.CHAOS_BALL && Type != (int)ID.WATER_SPHERE && LifeMax > 0)
					{
						Main.PlayerSet[PlayerIdx].ActiveNPCs += NpcSlots;
					}
				}
				else if (IsBoss || Type == (int)ID.DEVOURER_HEAD || Type == (int)ID.GIANT_WORM_HEAD || Type == (int)ID.EATER_OF_WORLDS_HEAD || Type == (int)ID.BONE_SERPENT_HEAD || Type == (int)ID.WYVERN_HEAD || Type == (int)ID.ARCH_WYVERN_HEAD || Type == (int)ID.SKELETRON_HEAD || Type == (int)ID.SKELETRON_HAND || (Type >= (int)ID.SKELETRON_PRIME && Type <= (int)ID.PRIME_LASER))
				{
					Director = true;
				}
				if (SpawnArea.Intersects(Main.PlayerSet[PlayerIdx].XYWH))
				{
					TimeLeft = NPCActiveTime;
				}
				
			}
			TimeLeft--;
			if (TimeLeft <= 0)
			{
				Director = false;
			}
			if (Director || Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			NoSpawnCycle = true;
			Active = 0;
			NetSkip = -1;
			Life = 0;
			NetMessage.CreateMessage1(23, WhoAmI);
			NetMessage.SendMessage();
			if (AIStyle != 6)
			{
				return;
			}
			for (int NPCIdx = (int)AI0; NPCIdx > 0; NPCIdx = (int)Main.NPCSet[NPCIdx].AI0)
			{
				if (Main.NPCSet[NPCIdx].Active != 0)
				{
					Main.NPCSet[NPCIdx].Active = 0;
					Main.NPCSet[NPCIdx].Life = 0;
					Main.NPCSet[NPCIdx].NetSkip = -1;
					NetMessage.CreateMessage1(23, NPCIdx);
					NetMessage.SendMessage();
				}
			}
		}

		public static void SpawnNPC()
		{
			if (NoSpawnCycle)
			{
				NoSpawnCycle = false;
				return;
			}
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			int num2 = 0;
			int SpawnMultiplier = 0;
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (Main.PlayerSet[PlayerIdx].Active != 0)
				{
					SpawnMultiplier++;
				}
			}
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (Main.PlayerSet[PlayerIdx].Active == 0 || Main.PlayerSet[PlayerIdx].IsDead)
				{
					continue;
				}
				bool flag3 = false;
				bool InInvasion = false;
				bool InWater = false;
				if (Main.InvasionType > 0 && Main.InvasionDelay == 0 && Main.InvasionSize > 0 && Main.PlayerSet[PlayerIdx].Position.Y < Main.WorldSurfacePixels + 1080)
				{
					int InvasionOffset = 3000;
					if (Main.PlayerSet[PlayerIdx].Position.X > Main.InvasionX * 16f - InvasionOffset && Main.PlayerSet[PlayerIdx].Position.X < Main.InvasionX * 16f + InvasionOffset)
					{
						InInvasion = true;
					}
				}
				flag = false;
				SpawnRate = DefaultSpawnRate;
				MaxSpawns = DefaultMaxSpawns;
				if (Main.InHardMode)
				{
					SpawnRate = (int)(DefaultSpawnRate * 0.9f);
					MaxSpawns = DefaultMaxSpawns + 1;
				}
				if (Main.PlayerSet[PlayerIdx].Position.Y > (Main.MaxTilesY - 200) * 16)
				{
					MaxSpawns *= 2;
				}
				else if (Main.PlayerSet[PlayerIdx].Position.Y > (Main.RockLayer << 4) + SpawnHeight)
				{
					SpawnRate = (int)(SpawnRate * 0.4);
					MaxSpawns = (int)(MaxSpawns * 1.9);
				}
				else if (Main.PlayerSet[PlayerIdx].Position.Y > (Main.WorldSurface << 4) + SpawnHeight)
				{
					if (Main.InHardMode)
					{
						SpawnRate = (int)(SpawnRate * 0.45);
						MaxSpawns = (int)(MaxSpawns * 1.8);
					}
					else
					{
						SpawnRate >>= 1;
						MaxSpawns = (int)(MaxSpawns * 1.7);
					}
				}
				else if (!Main.GameTime.DayTime)
				{
					SpawnRate = (int)(SpawnRate * 0.6);
					MaxSpawns = (int)(MaxSpawns * 1.3);
					if (Main.GameTime.IsBloodMoon)
					{
						SpawnRate = (int)(SpawnRate * 0.3);
						MaxSpawns = (int)(MaxSpawns * 1.8);
					}
				}
				if (Main.PlayerSet[PlayerIdx].ZoneDungeon)
				{
					SpawnRate = (int)(SpawnRate * 0.4);
					MaxSpawns = (int)(MaxSpawns * 1.7f);
				}
				else if (Main.PlayerSet[PlayerIdx].ZoneJungle)
				{
					SpawnRate = (int)(SpawnRate * 0.4);
					MaxSpawns = (int)(MaxSpawns * 1.5f);
				}
				else if (Main.PlayerSet[PlayerIdx].ZoneEvil)
				{
					SpawnRate = (int)(SpawnRate * 0.65);
					MaxSpawns = (int)(MaxSpawns * 1.3f);
				}
				else if (Main.PlayerSet[PlayerIdx].ZoneMeteor)
				{
					SpawnRate = (int)(SpawnRate * 0.4);
					MaxSpawns = (int)(MaxSpawns * 1.1f);
				}
				if (Main.PlayerSet[PlayerIdx].zoneHoly && Main.PlayerSet[PlayerIdx].Position.Y > (Main.RockLayer << 4) + 1080)
				{
					SpawnRate = (int)(SpawnRate * 0.65);
					MaxSpawns = (int)(MaxSpawns * 1.3f);
				}
				if (WoF >= 0 && Main.PlayerSet[PlayerIdx].Position.Y > (Main.MaxTilesY - 200) * 16)
				{
					MaxSpawns = (int)(MaxSpawns * 0.3f);
					SpawnRate *= 3;
				}
				if (Main.PlayerSet[PlayerIdx].ActiveNPCs < (int)(MaxSpawns * 0.2))
				{
					SpawnRate = (int)(SpawnRate * 0.6f);
				}
				else if (Main.PlayerSet[PlayerIdx].ActiveNPCs < (int)(MaxSpawns * 0.4))
				{
					SpawnRate = (int)(SpawnRate * 0.7f);
				}
				else if (Main.PlayerSet[PlayerIdx].ActiveNPCs < (int)(MaxSpawns * 0.6))
				{
					SpawnRate = (int)(SpawnRate * 0.8f);
				}
				else if (Main.PlayerSet[PlayerIdx].ActiveNPCs < (int)(MaxSpawns * 0.8))
				{
					SpawnRate = (int)(SpawnRate * 0.9f);
				}
				if (Main.PlayerSet[PlayerIdx].Position.Y * 16f > Main.WorldSurface + Main.RockLayer >> 1 || Main.PlayerSet[PlayerIdx].ZoneEvil)
				{
					if (Main.PlayerSet[PlayerIdx].ActiveNPCs < MaxSpawns * 0.2)
					{
						SpawnRate = (int)(SpawnRate * 0.7f);
					}
					else if (Main.PlayerSet[PlayerIdx].ActiveNPCs < MaxSpawns * 0.4)
					{
						SpawnRate = (int)(SpawnRate * 0.9f);
					}
				}
				if (Main.PlayerSet[PlayerIdx].Inventory[Main.PlayerSet[PlayerIdx].SelectedItem].Type == (int)Item.ID.WATER_CANDLE)
				{
					SpawnRate = (int)(SpawnRate * 0.75);
					MaxSpawns = (int)(MaxSpawns * 1.5f);
				}
				if (Main.PlayerSet[PlayerIdx].enemySpawns)
				{
					SpawnRate = (int)(SpawnRate * 0.5);
					MaxSpawns = (int)(MaxSpawns * 2f);
				}
				if ((double)SpawnRate < DefaultSpawnRate * 0.1f)
				{
					SpawnRate = (int)(DefaultSpawnRate * 0.1f);
				}
				if (MaxSpawns > DefaultMaxSpawns * 3)
				{
					MaxSpawns = DefaultMaxSpawns * 3;
				}
				if (InInvasion)
				{
					MaxSpawns = (int)(5.0 * (2.0 + 0.3 * SpawnMultiplier));
					SpawnRate = 20;
				}
				if (Main.PlayerSet[PlayerIdx].ZoneDungeon && !HasDownedBoss3)
				{
					SpawnRate = 10;
				}
				bool flag6 = false;
				if (!InInvasion && (!Main.GameTime.IsBloodMoon || Main.GameTime.DayTime) && !Main.PlayerSet[PlayerIdx].ZoneDungeon && !Main.PlayerSet[PlayerIdx].ZoneEvil && !Main.PlayerSet[PlayerIdx].ZoneMeteor)
				{
					if (Main.PlayerSet[PlayerIdx].TownNPCs == 1f)
					{
						flag3 = true;
						if (Main.Rand.Next(3) <= 1)
						{
							flag6 = true;
							MaxSpawns = (int)((double)(float)MaxSpawns * 0.6);
						}
						else
						{
							SpawnRate = (int)(SpawnRate * 2f);
						}
					}
					else if (Main.PlayerSet[PlayerIdx].TownNPCs == 2f)
					{
						flag3 = true;
						if (Main.Rand.Next(3) == 0)
						{
							flag6 = true;
							MaxSpawns = (int)((double)(float)MaxSpawns * 0.6);
						}
						else
						{
							SpawnRate = (int)(SpawnRate * 3f);
						}
					}
					else if (Main.PlayerSet[PlayerIdx].TownNPCs >= 3f)
					{
						flag3 = true;
						flag6 = true;
						MaxSpawns = (int)((double)(float)MaxSpawns * 0.6);
					}
				}
				if (Main.PlayerSet[PlayerIdx].ActiveNPCs < MaxSpawns && Main.Rand.Next(SpawnRate) == 0)
				{
					int PlayerX = Main.PlayerSet[PlayerIdx].XYWH.X >> 4;
					int PlayerCoord = Main.PlayerSet[PlayerIdx].XYWH.Y >> 4;
					int num7 = PlayerX - SpawnRangeX;
					int num8 = PlayerX + SpawnRangeX;
					int num9 = PlayerCoord - SpawnRangeY;
					int num10 = PlayerCoord + SpawnRangeY;
					int num11 = PlayerX - SafeRangeX;
					int num12 = PlayerX + SafeRangeX;
					int num13 = PlayerCoord - SafeRangeY;
					int num14 = PlayerCoord + SafeRangeY;
					if (num7 < 0)
					{
						num7 = 0;
					}
					else if (num8 > Main.MaxTilesX)
					{
						num8 = Main.MaxTilesX;
					}
					if (num9 < 0)
					{
						num9 = 0;
					}
					else if (num10 > Main.MaxTilesY)
					{
						num10 = Main.MaxTilesY;
					}
					for (int k = 0; k < 48; k++)
					{
						int num15 = Main.Rand.Next(num7, num8);
						int num16 = Main.Rand.Next(num9, num10);
						if ((Main.TileSet[num15, num16].IsActive != 0 && Main.TileSolid[Main.TileSet[num15, num16].Type]) || Main.WallHouse[Main.TileSet[num15, num16].WallType])
						{
							continue;
						}
						if (!InInvasion && !flag6 && ((num16 < (int)(Main.WorldSurface * 0.35f) && (num15 < (int)(Main.MaxTilesX * 0.45) || num15 > (int)(Main.MaxTilesX * 0.55) || Main.InHardMode)) || (num16 < (int)(Main.WorldSurface * 0.45f) && Main.InHardMode && Main.Rand.Next(10) == 0)))
						{
							num = num15;
							num2 = num16;
							flag = true;
							flag2 = true;
						}
						if (!flag)
						{
							for (int l = num16; l < Main.MaxTilesY; l++)
							{
								if (Main.TileSet[num15, l].IsActive != 0 && Main.TileSolid[Main.TileSet[num15, l].Type])
								{
									if (num15 < num11 || num15 > num12 || l < num13 || l > num14)
									{
										num = num15;
										num2 = l;
										flag = true;
									}
									break;
								}
							}
						}
						if (!flag)
						{
							continue;
						}
						int num17 = num - 1;
						int num18 = num + 1;
						int num19 = num2 - 3;
						int num20 = num2;
						if (num17 < 0 || num18 > Main.MaxTilesX || num19 < 0 || num20 > Main.MaxTilesY)
						{
							flag = false;
						}
						else
						{
							for (int m = num17; m < num18; m++)
							{
								for (int n = num19; n < num20; n++)
								{
									if (Main.TileSet[m, n].IsActive != 0 && Main.TileSolid[Main.TileSet[m, n].Type])
									{
										flag = false;
										break;
									}
									if (Main.TileSet[m, n].Lava != 0)
									{
										flag = false;
										break;
									}
								}
								if (!flag)
								{
									break;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (flag)
				{
					Rectangle rectangle = new Rectangle(num * 16, num2 * 16, 16, 16);
					for (int num21 = 0; num21 < Player.MaxNumPlayers; num21++)
					{
						if (Main.PlayerSet[num21].Active != 0)
						{
							Rectangle rectangle2 = new Rectangle(Main.PlayerSet[num21].XYWH.X + (Main.PlayerSet[num21].XYWH.Width / 2) - (SpawnWidth / 2) - SafeRangeX, Main.PlayerSet[num21].XYWH.Y + (Main.PlayerSet[num21].XYWH.Height / 2) - (SpawnHeight / 2) - SafeRangeY, SpawnWidth + (SafeRangeX * 2), SpawnHeight + (SafeRangeY * 2));
							if (rectangle.Intersects(rectangle2))
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag)
				{
					if (Main.PlayerSet[PlayerIdx].ZoneDungeon && (!Main.TileDungeon[Main.TileSet[num, num2].Type] || Main.TileSet[num, num2 - 1].WallType == 0))
					{
						flag = false;
					}
					if (Main.TileSet[num, num2 - 1].Liquid > 0 && Main.TileSet[num, num2 - 2].Liquid > 0 && Main.TileSet[num, num2 - 1].Lava == 0)
					{
						InWater = true;
					}
				}
				if (!flag)
				{
					continue;
				}
				flag = false;
				int num22 = Main.TileSet[num, num2].Type;
				int num23 = MaxNumNPCs;
				int x = (num << 4) + 8;
				int y = num2 << 4;
				if (flag2)
				{
					if (Main.InHardMode && Main.Rand.Next(10) == 0 && !AnyNPCs((int)ID.WYVERN_HEAD, (int)ID.ARCH_WYVERN_HEAD))
					{
						int Type = ((Main.Rand.Next(2) == 0) ? (int)ID.WYVERN_HEAD : (int)ID.ARCH_WYVERN_HEAD);
						NewNPC(x, y, Type, 1);
					}
					else
					{
						NewNPC(x, y, (int)ID.HARPY);
					}
				}
				else if (InInvasion)
				{
					if (Main.InvasionType == 1)
					{
						if (Main.Rand.Next(9) == 0)
						{
							NewNPC(x, y, (int)ID.GOBLIN_SORCERER);
						}
						else if (Main.Rand.Next(5) == 0)
						{
							NewNPC(x, y, (int)ID.GOBLIN_PEON);
						}
						else if (Main.Rand.Next(3) == 0)
						{
							NewNPC(x, y, (int)ID.GOBLIN_ARCHER);
						}
						else if (Main.Rand.Next(3) == 0)
						{
							NewNPC(x, y, (int)ID.GOBLIN_THIEF);
						}
						else
						{
							NewNPC(x, y, (int)ID.GOBLIN_WARRIOR);
						}
					}
					else if (Main.InvasionType == 2)
					{
						if (Main.Rand.Next(7) == 0)
						{
							NewNPC(x, y, (int)ID.SNOW_BALLA);
						}
						else if (Main.Rand.Next(3) == 0)
						{
							NewNPC(x, y, (int)ID.SNOWMAN_GANGSTA);
						}
						else
						{
							NewNPC(x, y, (int)ID.MISTER_STABBY);
						}
					}
				}
				else if (InWater && (num < 250 || num > Main.MaxTilesX - 250) && num22 == 53 && num2 < Main.RockLayer)
				{
					int Type;
					switch (Main.Rand.Next(16))
					{
					case 0:
						Type = (int)ID.SHARK;
						break;
					case 1:
						Type = (int)ID.ORKA;
						break;
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
						Type = (int)ID.CRAB;
						break;
					default:
						Type = (int)ID.PINK_JELLYFISH;
						break;
					}
					NewNPC(x, y, Type);
				}
				else if (InWater && ((num2 > Main.RockLayer && Main.Rand.Next(2) == 0) || num22 == 60))
				{
					if (Main.InHardMode && Main.Rand.Next(3) > 0)
					{
						NewNPC(x, y, (int)ID.ANGLER_FISH);
					}
					else
					{
						NewNPC(x, y, (int)ID.PIRANHA);
					}
				}
				else if (InWater && num2 > Main.WorldSurface && Main.Rand.Next(3) == 0)
				{
					if (Main.InHardMode)
					{
						NewNPC(x, y, (int)ID.GREEN_JELLYFISH);
					}
					else
					{
						NewNPC(x, y, (int)ID.BLUE_JELLYFISH);
					}
				}
				else if (InWater && Main.Rand.Next(4) == 0)
				{
					if (Main.PlayerSet[PlayerIdx].ZoneEvil)
					{
						NewNPC(x, y, (int)ID.CORRUPT_GOLDFISH);
					}
					else
					{
						NewNPC(x, y, (int)ID.GOLDFISH);
					}
				}
				else if (HasDownedGoblins && Main.Rand.Next(20) == 0 && !InWater && num2 >= Main.RockLayer && num2 < Main.MaxTilesY - 210 && !HasSavedGoblin && !AnyNPCs((int)ID.BOUND_GOBLIN))
				{
					NewNPC(x, y, (int)ID.BOUND_GOBLIN);
				}
				else if (Main.InHardMode && Main.Rand.Next(20) == 0 && !InWater && num2 >= Main.RockLayer && num2 < Main.MaxTilesY - 210 && !HasSavedWizard && !AnyNPCs((int)ID.BOUND_WIZARD))
				{
					NewNPC(x, y, (int)ID.BOUND_WIZARD);
				}
				else if (flag6)
				{
					if (InWater)
					{
						NewNPC(x, y, (int)ID.GOLDFISH);
					}
					else
					{
						if (num22 != 2 && num22 != 109 && num22 != 147 && num2 <= Main.WorldSurface)
						{
							break;
						}
						if (Main.Rand.Next(2) == 0 && num2 <= Main.WorldSurface)
						{
							NewNPC(x, y, (int)ID.BIRD);
						}
						else
						{
							NewNPC(x, y, (int)ID.BUNNY);
						}
					}
				}
				else if (Main.PlayerSet[PlayerIdx].ZoneDungeon)
				{
					if (!HasDownedBoss3)
					{
						num23 = NewNPC(x, y, (int)ID.DUNGEON_GUARDIAN);
					}
					else if (!HasSavedMech && !InWater && Main.Rand.Next(5) == 0 && num2 > Main.RockLayer && !AnyNPCs((int)ID.BOUND_MECHANIC))
					{
						NewNPC(x, y, (int)ID.BOUND_MECHANIC);
					}
					else if (Main.Rand.Next(37) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.DUNGEON_SLIME);
					}
					else if (Main.Rand.Next(4) == 0 && !NearSpikeBall(num, num2))
					{
						num23 = NewNPC(x, y, (int)ID.SPIKE_BALL);
					}
					else if (Main.Rand.Next(15) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.BLAZING_WHEEL);
					}
					else if (Main.Rand.Next(9) == 0)
					{
						num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.CURSED_SKULL : (int)ID.DRAGON_SKULL);
					}
					else if (Main.Rand.Next(7) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.DARK_CASTER);
					}
					else
					{
						num23 = NewNPC(x, y, (int)ID.BONES);
						if (Main.Rand.Next(4) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Big Boned");
						}
						else if (Main.Rand.Next(5) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Short Bones");
						}
					}
				}
				else if (Main.PlayerSet[PlayerIdx].ZoneMeteor)
				{
					num23 = NewNPC(x, y, (int)ID.METEOR_HEAD);
				}
#if !VERSION_INITIAL
				else if (num22 == 70 && (double)num2 <= Main.WorldSurface && Main.Rand.Next(3) != 0)
				{
					if (Main.Rand.Next(3) != 0)
					{
						num23 = ((Main.Rand.Next(2) != 0) ? NewNPC(x, y, (int)ID.ZOMBIE_MUSHROOM_HAT) : NewNPC(x, y, (int)ID.ZOMBIE_MUSHROOM));
					}
				}
#endif
				else if (Main.PlayerSet[PlayerIdx].ZoneEvil && Main.Rand.Next(65) == 0)
				{
					num23 = ((!Main.InHardMode || Main.Rand.Next(4) == 0) ? NewNPC(x, y, (int)ID.DEVOURER_HEAD, 1) : NewNPC(x, y, (int)ID.SEEKER_HEAD, 1));
				}
				else if (Main.InHardMode && num2 > Main.WorldSurface && Main.Rand.Next(75) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.MIMIC);
				}
				else if (Main.InHardMode && Main.TileSet[num, num2 - 1].WallType == 2 && Main.Rand.Next(20) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.MIMIC);
				}
				else if (Main.InHardMode && num2 <= Main.WorldSurface && !Main.GameTime.DayTime && (Main.Rand.Next(20) == 0 || (Main.Rand.Next(5) == 0 && Main.GameTime.MoonPhase == 4)))
				{
					num23 = NewNPC(x, y, (int)ID.WRAITH);
				}
				else if (num22 == 60 && Main.Rand.Next(500) == 0 && !Main.GameTime.DayTime)
				{
					num23 = NewNPC(x, y, (int)ID.DOCTOR_BONES);
				}
				else if (num22 == 60 && num2 > Main.WorldSurface + Main.RockLayer >> 1)
				{
					if (Main.Rand.Next(3) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.MAN_EATER);
						Main.NPCSet[num23].AI0 = num;
						Main.NPCSet[num23].AI1 = num2;
						Main.NPCSet[num23].ShouldNetUpdate = true;
					}
					else if (Main.Rand.Next(2) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.HORNET);
						switch (Main.Rand.Next(8))
						{
						case 0:
						case 1:
							Main.NPCSet[num23].SetDefaults("Little Stinger");
							break;
						case 2:
							Main.NPCSet[num23].SetDefaults("Big Stinger");
							break;
						}
					}
					else
					{
						num23 = NewNPC(x, y, (int)ID.DRAGON_HORNET);
					}
				}
				else if (num22 == 60 && Main.Rand.Next(4) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.JUNGLE_BAT);
				}
				else if (num22 == 60 && Main.Rand.Next(8) == 0)
				{
					num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.SNATCHER : (int)ID.DRAGON_SNATCHER);
					Main.NPCSet[num23].AI0 = num;
					Main.NPCSet[num23].AI1 = num2;
					Main.NPCSet[num23].ShouldNetUpdate = true;
				}
				else if (Main.InHardMode && num22 == 53 && Main.Rand.Next(3) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.MUMMY);
				}
				else if (Main.InHardMode && num22 == 112 && Main.Rand.Next(2) == 0)
				{
					num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.DARK_MUMMY : (int)ID.SHADOW_MUMMY);
				}
				else if (Main.InHardMode && num22 == 116 && Main.Rand.Next(2) == 0)
				{
					num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.LIGHT_MUMMY : (int)ID.SPECTRAL_MUMMY);
				}
				else if (Main.InHardMode && !InWater && num2 < Main.RockLayer && (num22 == 116 || num22 == 117 || num22 == 109))
				{
					num23 = ((!Main.GameTime.DayTime && Main.Rand.Next(2) == 0) ? NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.GASTROPOD : (int)ID.SPECTRAL_GASTROPOD) : ((Main.Rand.Next(10) != 0) ? NewNPC(x, y, (int)ID.PIXIE) : NewNPC(x, y, (int)ID.UNICORN)));
				}
				else if (!flag3 && Main.InHardMode && Main.Rand.Next(50) == 0 && !InWater && num2 >= Main.RockLayer && (num22 == 116 || num22 == 117 || num22 == 109))
				{
					num23 = NewNPC(x, y, (int)ID.ENCHANTED_SWORD);
				}
				else if ((num22 == 22 && Main.PlayerSet[PlayerIdx].ZoneEvil) || num22 == 23 || num22 == 25 || num22 == 112)
				{
					if (Main.InHardMode && num2 >= Main.RockLayer && Main.Rand.Next(3) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.CLINGER);
						Main.NPCSet[num23].AI0 = num;
						Main.NPCSet[num23].AI1 = num2;
						Main.NPCSet[num23].ShouldNetUpdate = true;
					}
					else if (Main.InHardMode && Main.Rand.Next(3) == 0)
					{
						int Type;
						switch (Main.Rand.Next(3))
						{
						case 0:
							Type = (int)ID.SHADOW_SLIME;
							break;
						case 1:
							Type = (int)ID.CORRUPT_SLIME;
							break;
						default:
							Type = (int)ID.SLIMER;
							break;
						}
						num23 = NewNPC(x, y, Type);
					}
					else if (Main.InHardMode && num2 >= Main.RockLayer && Main.Rand.Next(40) == 0)
					{
						num23 = ((Main.Rand.Next(2) != 0) ? NewNPC(x, y, (int)ID.SHADOW_HAMMER) : NewNPC(x, y, (int)ID.CURSED_HAMMER));
					}
					else if (Main.InHardMode && (Main.Rand.Next(2) == 0 || num2 > Main.RockLayer))
					{
						num23 = NewNPC(x, y, (int)ID.CORRUPTOR);
					}
					else
					{
						num23 = NewNPC(x, y, (int)ID.EATER_OF_SOULS);
						if (Main.Rand.Next(3) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Little Eater");
						}
						else if (Main.Rand.Next(3) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Big Eater");
						}
					}
				}
				else if (num2 <= Main.WorldSurface)
				{
					if (Main.GameTime.DayTime)
					{
						int num27 = Math.Abs(num - Main.SpawnTileX);
						if (num27 < Main.MaxTilesX / 3 && Main.Rand.Next(15) == 0 && (num22 == 2 || num22 == 109 || num22 == 147))
						{
							NewNPC(x, y, (int)ID.BUNNY);
						}
						else if (num27 < Main.MaxTilesX / 3 && Main.Rand.Next(15) == 0 && (num22 == 2 || num22 == 109 || num22 == 147))
						{
							NewNPC(x, y, (int)ID.BIRD);
						}
						else if (num27 > Main.MaxTilesX / 3 && num22 == 2 && Main.Rand.Next(300) == 0 && !AnyNPCs((int)ID.KING_SLIME))
						{
							num23 = NewNPC(x, y, (int)ID.KING_SLIME);
						}
						else if (num22 == 53 && Main.Rand.Next(5) == 0 && !InWater)
						{
							num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.ANTLION : (int)ID.ALBINO_ANTLION);
						}
						else if (num22 == 53 && !InWater)
						{
							num23 = NewNPC(x, y, (int)ID.VULTURE);
						}
						else if (num27 > Main.MaxTilesX / 3 && Main.Rand.Next(15) == 0)
						{
							num23 = NewNPC(x, y, (int)ID.GOBLIN_SCOUT);
						}
						else
						{
							num23 = NewNPC(x, y, (int)ID.SLIME);
							if (num22 == 60)
							{
								Main.NPCSet[num23].SetDefaults("Jungle Slime");
							}
							else if (Main.Rand.Next(3) == 0 || num27 < 200)
							{
								Main.NPCSet[num23].SetDefaults("Green Slime");
							}
							else if (Main.Rand.Next(10) == 0 && num27 > 400)
							{
								Main.NPCSet[num23].SetDefaults("Purple Slime");
							}
						}
					}
					else if (Main.Rand.Next(6) == 0 || (Main.GameTime.MoonPhase == 4 && Main.Rand.Next(2) == 0))
					{
#if VERSION_INITIAL
						num23 = ((!Main.InHardMode || Main.Rand.Next(3) != 0) ? NewNPC(x, y, (int)NPC.ID.DEMON_EYE) : NewNPC(x, y, (int)NPC.ID.WANDERING_EYE));
#else
						if (Main.InHardMode && Main.Rand.Next(3) == 0) // The decompiled chances are buggered, using PC spawn code
						{
							num23 = NewNPC(x, y, (int)ID.WANDERING_EYE);
						}
						else if (Main.Rand.Next(2) == 0)
						{
							num23 = NewNPC(x, y, (int)ID.DEMON_EYE);
							if (Main.Rand.Next(4) == 0)
							{
								Main.NPCSet[num23].SetDefaults("Demon Eye 2");
							}
						}
						else
						{
							switch (Main.Rand.Next(5))
							{
							case 0:
								num23 = NewNPC(x, y, (int)ID.CATARACT_EYE);
								if (Main.Rand.Next(3) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Cataract Eye 2");
								}
								break;
							case 1:
								num23 = NewNPC(x, y, (int)ID.SLEEPY_EYE);
								if (Main.Rand.Next(3) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Sleepy Eye 2");
								}
								break;
							case 2:
								num23 = NewNPC(x, y, (int)ID.DIALATED_EYE);
								if (Main.Rand.Next(3) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Dialated Eye 2");
								}
								break;
							case 3:
								num23 = NewNPC(x, y, (int)ID.GREEN_EYE);
								if (Main.Rand.Next(3) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Green Eye 2");
								}
								break;
							case 4:
								num23 = NewNPC(x, y, (int)ID.PURPLE_EYE);
								if (Main.Rand.Next(3) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Purple Eye 2");
								}
								break;
							}
						}
#endif
					}
					else if (Main.InHardMode && Main.Rand.Next(50) == 0 && Main.GameTime.IsBloodMoon && !AnyNPCs((int)ID.CLOWN))
					{
						NewNPC(x, y, (int)ID.CLOWN);
					}
					else if (Main.Rand.Next(250) == 0 && Main.GameTime.IsBloodMoon)
					{
						NewNPC(x, y, (int)ID.THE_GROOM);
					}
					else if (Main.GameTime.MoonPhase == 0 && Main.InHardMode && Main.Rand.Next(3) != 0)
					{
						NewNPC(x, y, (int)ID.WEREWOLF);
					}
					else if (Main.InHardMode && Main.Rand.Next(3) == 0)
					{
						NewNPC(x, y, (int)ID.POSSESSED_ARMOR);
					}
#if VERSION_INITIAL
					else if (Main.Rand.Next(3) == 0)
					{
						NewNPC(x, y, (int)NPC.ID.BALD_ZOMBIE);
					}
					else
					{
						NewNPC(x, y, (int)NPC.ID.ZOMBIE);
					}
#else
					else
					{
						switch (Main.Rand.Next(7))
						{
						case 0:
							num23 = NewNPC(x, y, (int)ID.ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Zombie");
								}
							}
							break;
						case 1:
							num23 = NewNPC(x, y, (int)ID.BALD_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Bald Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Bald Zombie");
								}
							}
							break;
						case 2:
							num23 = NewNPC(x, y, (int)ID.PINCUSHION_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Pincushion Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Pincushion Zombie");
								}
							}
							break;
						case 3:
							num23 = NewNPC(x, y, (int)ID.SLIMED_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Slimed Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Slimed Zombie");
								}
							}
							break;
						case 4:
							num23 = NewNPC(x, y, (int)ID.SWAMP_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Swamp Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Swamp Zombie");
								}
							}
							break;
						case 5:
							num23 = NewNPC(x, y, (int)ID.TWIGGY_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Twiggy Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Twiggy Zombie");
								}
							}
							break;
						case 6:
							num23 = NewNPC(x, y, (int)ID.FEMALE_ZOMBIE);
							if (Main.Rand.Next(3) == 0)
							{
								if (Main.Rand.Next(2) == 0)
								{
									Main.NPCSet[num23].SetDefaults("Small Female Zombie");
								}
								else
								{
									Main.NPCSet[num23].SetDefaults("Big Female Zombie");
								}
							}
							break;
						}
					}
#endif
				}
				else if (num2 <= Main.RockLayer)
				{
					if (!flag3 && Main.Rand.Next(50) == 0)
					{
						num23 = ((!Main.InHardMode) ? NewNPC(x, y, (int)ID.GIANT_WORM_HEAD, 1) : NewNPC(x, y, (int)ID.DIGGER_HEAD, 1));
					}
					else if (Main.InHardMode && Main.Rand.Next(3) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.POSSESSED_ARMOR);
					}
					else if (Main.InHardMode && Main.Rand.Next(4) != 0)
					{
						num23 = NewNPC(x, y, (int)ID.TOXIC_SLUDGE);
					}
					else
					{
						num23 = NewNPC(x, y, (int)ID.SLIME); // Interesting difference between versions is that PC adds a random chance below for the default to be a Blue slime...
						if (Main.Rand.Next(5) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Yellow Slime");
						}
						// ...The thing is, it already starts trying to spawn a Blue slime, and random chance makes it yellow or red...
						else if (Main.Rand.Next(2) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Red Slime");
						}
						// ...So after trying to spawn a Blue slime and adding chance for it to change, it also adds a chance to change back. Weird.
					}
				}
				else if (num2 > Main.MaxTilesY - 190)
				{
					int Type = (int)ID.HELLBAT;
					int start = 0;
					if (Main.Rand.Next(40) == 0 && !AnyNPCs((int)ID.BONE_SERPENT_HEAD))
					{
						Type = (int)ID.BONE_SERPENT_HEAD;
						start = 1;
					}
					else if (Main.Rand.Next(14) == 0)
					{
						Type = (int)ID.FIRE_IMP;
					}
					else if (Main.Rand.Next(8) == 0)
					{
						switch (Main.Rand.Next(7))
						{
						case 0:
							Type = (int)ID.VOODOO_DEMON;
							break;
						case 1:
						case 2:
						case 3:
							Type = (int)ID.DEMON;
							break;
						default:
							Type = (int)ID.ARCH_DEMON;
							break;
						}
					}
					else if (Main.Rand.Next(3) == 0)
					{
						Type = (int)ID.LAVA_SLIME;
					}
					num23 = NewNPC(x, y, Type, start);
				}
				else if ((num22 == 116 || num22 == 117) && !flag3 && Main.Rand.Next(8) == 0)
				{
					num23 = NewNPC(x, y, (Main.Rand.Next(2) == 0) ? (int)ID.CHAOS_ELEMENTAL : (int)ID.SPECTRAL_ELEMENTAL);
				}
				else if (!flag3 && Main.Rand.Next(75) == 0 && !Main.PlayerSet[PlayerIdx].zoneHoly)
				{
					num23 = NewNPC(x, y, Main.InHardMode ? (int)ID.DIGGER_HEAD : (int)ID.GIANT_WORM_HEAD, 1);
				}
				else if (!Main.InHardMode && Main.Rand.Next(10) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.MOTHER_SLIME);
				}
				else if (!Main.InHardMode && Main.Rand.Next(4) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.SLIME);
					if (Main.PlayerSet[PlayerIdx].ZoneJungle)
					{
						Main.NPCSet[num23].SetDefaults("Jungle Slime");
					}
					else
					{
						Main.NPCSet[num23].SetDefaults("Black Slime");
					}
				}
				else if (Main.Rand.Next(2) != 0)
				{
					num23 = ((Main.InHardMode && (Main.PlayerSet[PlayerIdx].zoneHoly & (Main.Rand.Next(2) == 0))) ? NewNPC(x, y, (int)ID.ILLUMINANT_SLIME) : (Main.PlayerSet[PlayerIdx].ZoneJungle ? NewNPC(x, y, (int)ID.JUNGLE_BAT) : ((Main.InHardMode && Main.PlayerSet[PlayerIdx].zoneHoly) ? NewNPC(x, y, (int)ID.ILLUMINANT_BAT) : ((!Main.InHardMode || Main.Rand.Next(6) <= 0) ? NewNPC(x, y, (int)ID.CAVE_BAT) : NewNPC(x, y, (int)ID.GIANT_BAT)))));
				}
				else if (num2 > Main.RockLayer + Main.MaxTilesY >> 1 && Main.Rand.Next(700) == 0)
				{
					num23 = NewNPC(x, y, (int)ID.TIM);
				}
				else if (Main.InHardMode && Main.Rand.Next(10) != 0)
				{
					if (Main.Rand.Next(2) == 0)
					{
						num23 = NewNPC(x, y, (int)ID.ARMORED_SKELETON);
						if (num2 > Main.RockLayer + Main.MaxTilesY >> 1 && Main.Rand.Next(5) == 0)
						{
							Main.NPCSet[num23].SetDefaults("Heavy Skeleton");
						}
					}
					else
					{
						num23 = NewNPC(x, y, (int)ID.SKELETON_ARCHER);
					}
				}
				else if (Main.Rand.Next(15) == 0)
				{
					int num29 = ((Main.Rand.Next(2) == 0) ? (int)ID.UNDEAD_MINER : (int)ID.VAMPIRE_MINER);
					num23 = NewNPC(x, y, num29);
				}
				else
				{
					num23 = NewNPC(x, y, (int)ID.SKELETON);
				}
				if (num23 < MaxNumNPCs)
				{
					if (Main.NPCSet[num23].Type == (int)ID.SLIME && Main.Rand.Next(250) == 0)
					{
						Main.NPCSet[num23].SetDefaults("Pinky");
					}
					NetMessage.CreateMessage1(23, num23);
					NetMessage.SendMessage();
				}
				break;
			}
		}

		public static bool SpawnWOF(ref Vector2 pos, bool force = false) // So they made a debug parameter allowing you to summon above hell... interesting.
		{
			if ((!force && (int)pos.Y >> 4 < Main.MaxTilesY - 205) || WoF >= 0 || Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return false;
			}
			int num = -16;
			int num2 = (int)pos.X;
			if ((num2 >> 4) > Main.MaxTilesX >> 1)
			{
				num = 16;
			}
			bool flag;
			int num1 = num2 + 1200;
			do
			{
				flag = true;
				for (int i = 0; i < Player.MaxNumPlayers; i++)
				{
					if (Main.PlayerSet[i].Active != 0 && Main.PlayerSet[i].XYWH.X > ((Main.SmallWorldH * -2) + num1) && Main.PlayerSet[i].XYWH.X < num1)
					{
						num2 += num;
						num1 += num;
						flag = false;
						break;
					}
				}
				if ((num < 0 && ((num2 >> 4) < 42)) || (num > 0 && ((num2 >> 4) > Main.MaxTilesX - 34)))
				{
					flag = true;
				}
			}
			while (!flag);
			int num3 = (int)pos.Y;
			int num4 = num2 >> 4;
			int num5 = num3 >> 4;
			int num6 = 0;
			try
			{
				while (true)
				{
					if (!WorldGen.SolidTile(num4, num5 - num6) && Main.TileSet[num4, num5 - num6].Liquid < 100)
					{
						num5 -= num6;
						break;
					}
					if (!WorldGen.SolidTile(num4, num5 + num6) && Main.TileSet[num4, num5 + num6].Liquid < 100)
					{
						num5 += num6;
						break;
					}
					num6++;
				}
			}
			catch
			{
			}
			num3 = num5 << 4;
			int num7 = NewNPC(num2, num3, (int)ID.WALL_OF_FLESH); // The X calculations are different to PC in that Console will spawn the WoF closer to you or at the same position as PC, depending where you are.
			Main.NPCSet[num7].Direction = 1;
			if (num > -1)
			{
				Main.NPCSet[num7].Direction = -1;
			}
			if (Main.NPCSet[num7].DisplayName.Length == 0)
			{
				Main.NPCSet[num7].DisplayName = Main.NPCSet[num7].TypeName;
			}
			NetMessage.SendText(Main.NPCSet[num7].DisplayName, 16, 175, 75, 255, -1);
			return true;
		}

		public static void SpawnOnPlayer(Player p, int Type)
		{
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			bool flag = false;
			int num = 0;
			int num2 = 0;
			int num3 = (p.XYWH.X >> 4) - 168;
			int num4 = (p.XYWH.X >> 4) + 168;
			int num5 = (p.XYWH.Y >> 4) - 92;
			int num6 = (p.XYWH.Y >> 4) + 92;
			int num7 = (p.XYWH.X >> 4) - 62;
			int num8 = (p.XYWH.X >> 4) + 62;
			int num9 = (p.XYWH.Y >> 4) - 34;
			int num10 = (p.XYWH.Y >> 4) + 34;
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
			for (int i = 0; i < 1000; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					int num11 = Main.Rand.Next(num3, num4);
					int num12 = Main.Rand.Next(num5, num6);
					if (Main.TileSet[num11, num12].IsActive == 0 || !Main.TileSolid[Main.TileSet[num11, num12].Type])
					{
						if (Main.WallHouse[Main.TileSet[num11, num12].WallType] && i < 999)
						{
							continue;
						}
						for (int k = num12; k < Main.MaxTilesY; k++)
						{
							if (Main.TileSet[num11, k].IsActive != 0 && Main.TileSolid[Main.TileSet[num11, k].Type])
							{
								if (num11 < num7 || num11 > num8 || k < num9 || k > num10 || i == 999)
								{
									num = num11;
									num2 = k;
									flag = true;
								}
								break;
							}
						}
						if (flag && i < 999)
						{
							int num13 = num - 1;
							int num14 = num + 1;
							int num15 = num2 - 3;
							int num16 = num2;
							if (num13 < 0)
							{
								flag = false;
							}
							if (num14 > Main.MaxTilesX)
							{
								flag = false;
							}
							if (num15 < 0)
							{
								flag = false;
							}
							if (num16 > Main.MaxTilesY)
							{
								flag = false;
							}
							if (flag)
							{
								for (int l = num13; l < num14; l++)
								{
									for (int m = num15; m < num16; m++)
									{
										if (Main.TileSet[l, m].IsActive != 0 && Main.TileSolid[Main.TileSet[l, m].Type])
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
					}
					if (flag || flag)
					{
						break;
					}
				}
				if (flag && i < 999)
				{
					Rectangle rectangle = new Rectangle(num * 16, num2 * 16, 16, 16);
					for (int n = 0; n < Player.MaxNumPlayers; n++)
					{
						if (Main.PlayerSet[n].Active != 0)
						{
							Rectangle rectangle2 = new Rectangle(Main.PlayerSet[n].XYWH.X + (Main.PlayerSet[n].XYWH.Width / 2) - (SpawnWidth / 2) - SafeRangeX, Main.PlayerSet[n].XYWH.Y + (Main.PlayerSet[n].XYWH.Height / 2) - (SpawnHeight / 2) - SafeRangeY, SpawnWidth + (SafeRangeX * 2), SpawnHeight + (SafeRangeY * 2));
							if (rectangle.Intersects(rectangle2))
							{
								flag = false;
							}
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			int num17 = MaxNumNPCs; // Why is this even here? #EngineMoment
			num17 = NewNPC(num * 16 + 8, num2 * 16, Type, 1);
			if (num17 != MaxNumNPCs)
			{
				Main.NPCSet[num17].Target = p.WhoAmI;
				Main.NPCSet[num17].TimeLeft *= 20;
				string text = Main.NPCSet[num17].DisplayName;
				if (text.Length == 0)
				{
					text = Main.NPCSet[num17].TypeName;
				}
				NetMessage.CreateMessage1(23, num17);
				NetMessage.SendMessage();
				switch (Type)
				{
				case (int)ID.RETINAZER:
					NetMessage.SendText(34, 175, 75, 255, -1);
					break;
				default:
					NetMessage.SendText(text, 16, 175, 75, 255, -1);
					break;
				case (int)ID.KING_SLIME:
				case (int)ID.WRAITH:
				case (int)ID.SPAZMATISM:
					break;
				}
			}
		}

		public static int NewNPC(int X, int Y, int Type, int Start = 0)
		{
			int num = MaxNumNPCs;
			for (int i = Start; i < MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active == 0)
				{
					num = i;
					break;
				}
			}
			if (num < MaxNumNPCs)
			{
				Main.NPCSet[num].SetDefaults(Type);
				Main.NPCSet[num].Position.X = (Main.NPCSet[num].XYWH.X = X - (Main.NPCSet[num].Width >> 1));
				Main.NPCSet[num].Position.Y = (Main.NPCSet[num].XYWH.Y = Y - Main.NPCSet[num].Height);
				Main.NPCSet[num].Active = 1;
				Main.NPCSet[num].TimeLeft = NPCActiveTime;
				Main.NPCSet[num].IsWet = Collision.WetCollision(ref Main.NPCSet[num].Position, Main.NPCSet[num].Width, Main.NPCSet[num].Height);
				if (Type == (int)ID.KING_SLIME)
				{
					NetMessage.SendText(Main.NPCSet[num].TypeName, 16, 175, 75, 255, -1);
				}
			}
			return num;
		}

		public void Transform(int newType)
		{
			Vector2 vector = Velocity;
			Position.Y += Height;
			sbyte b = SpriteDirection;
			SetDefaults(newType);
			SpriteDirection = b;
			TargetClosest();
			Velocity = vector;
			Position.Y -= Height;
			if (newType == (int)ID.GOBLIN_TINKERER || newType == (int)ID.WIZARD)
			{
				HomeTileX = (short)((int)Position.X + (Width >> 1) >> 4);
				HomeTileY = (short)((int)Position.Y + Height >> 4);
				IsHomeless = true;
			}
			ShouldNetUpdate = true;
			NetMessage.CreateMessage1(23, WhoAmI);
			NetMessage.SendMessage();
		}

		public double StrikeNPC(int Damage, float knockBack, int hitDirection, bool crit = false, bool noEffect = false)
		{
			if (Active == 0 || Life <= 0)
			{
				return 0.0;
			}
			double num = Main.CalculateDamage(Damage, Defense);
			if (crit)
			{
				num *= 2.0;
			}
			if (Damage != 9999 && LifeMax > 1)
			{
				if (IsFriendly)
				{
					CombatText.NewText(Position, Width, Height, (int)num, crit);
				}
				else
				{
					CombatText.NewText(Position, Width, Height, (int)num, crit); // ...you serious?
				}
				// Look, it is just a leftover from the PC version since colours are specified here. Just that is not featured on console.
				if (DrawMyName < DrawOnStrike)
				{
					DrawMyName = DrawOnStrike;
				}
			}
			if (num >= 1.0)
			{
				WasJustHit = true;
				if (IsTownNPC)
				{
					AI0 = 1f;
					AI1 = 300 + Main.Rand.Next(300);
					AI2 = 0f;
					Direction = (sbyte)hitDirection;
					ShouldNetUpdate = true;
				}
				if (AIStyle == 8 && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					AI0 = 400f;
					TargetClosest();
				}
				if (RealLife >= 0)
				{
					Main.NPCSet[RealLife].Life -= (int)num;
					Life = Main.NPCSet[RealLife].Life;
					LifeMax = Main.NPCSet[RealLife].LifeMax;
				}
				else
				{
					Life -= (int)num;
				}
				if (knockBack > 0f && KnockBackResist > 0f)
				{
					float num2 = knockBack * KnockBackResist;
					if (num2 > 8f)
					{
						num2 = 8f;
					}
					if (crit)
					{
						num2 *= 1.4f;
					}
					if (num * 10.0 < LifeMax)
					{
						if (hitDirection < 0 && Velocity.X > 0f - num2)
						{
							if (Velocity.X > 0f)
							{
								Velocity.X -= num2;
							}
							Velocity.X -= num2;
							if (Velocity.X < 0f - num2)
							{
								Velocity.X = 0f - num2;
							}
						}
						else if (hitDirection > 0 && Velocity.X < num2)
						{
							if (Velocity.X < 0f)
							{
								Velocity.X += num2;
							}
							Velocity.X += num2;
							if (Velocity.X > num2)
							{
								Velocity.X = num2;
							}
						}
						num2 = (HasNoGravity ? (num2 * -0.5f) : (num2 * -0.75f));
						if (Velocity.Y > num2)
						{
							Velocity.Y += num2;
							if (Velocity.Y < num2)
							{
								Velocity.Y = num2;
							}
						}
					}
					else
					{
						if (!HasNoGravity)
						{
							Velocity.Y = (0f - num2) * 0.75f * KnockBackResist;
						}
						else
						{
							Velocity.Y = (0f - num2) * 0.5f * KnockBackResist;
						}
						Velocity.X = num2 * hitDirection * KnockBackResist;
					}
				}
				if ((Type == (int)ID.WALL_OF_FLESH || Type == (int)ID.WALL_OF_FLESH_EYE) && Life <= 0)
				{
					for (int i = 0; i < MaxNumNPCs; i++)
					{
						if (Main.NPCSet[i].Active != 0 && (Main.NPCSet[i].Type == (int)ID.WALL_OF_FLESH || Main.NPCSet[i].Type == (int)ID.WALL_OF_FLESH_EYE))
						{
							Main.NPCSet[i].HitEffect(hitDirection, num);
						}
					}
				}
				else if (Active != 0)
				{
					HitEffect(hitDirection, num);
				}
				if (SoundHit > 0)
				{
					Main.PlaySound(3, (int)Position.X, (int)Position.Y, SoundHit);
				}
				if (RealLife >= 0)
				{
					Main.NPCSet[RealLife].CheckDead();
				}
				else
				{
					CheckDead();
				}
				return num;
			}
			return 0.0;
		}

		public void CheckDead()
		{
			if (Active == 0 || (RealLife >= 0 && RealLife != WhoAmI) || Life > 0)
			{
				return;
			}
			NoSpawnCycle = true;
			if (IsTownNPC && Type != (int)ID.OLD_MAN && Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				string prefix = DisplayName;
				if (DisplayName.Length == 0)
				{
					prefix = TypeName;
				}
				NetMessage.SendText(prefix, 19, 255, 25, 25, -1);
				TypeNames[Type] = null;
				SetNames();
				NetMessage.CreateMessage1((int)SendDataId.SERVER_NPC_NAMES, Type);
				NetMessage.SendMessage();
			}
			if (IsTownNPC && Main.NetMode != (byte)NetModeSetting.CLIENT && IsHomeless && WorldGen.ToSpawnNPC == Type)
			{
				WorldGen.ToSpawnNPC = 0;
			}
			if (SoundKilled > 0)
			{
				Main.PlaySound(4, (int)Position.X, (int)Position.Y, SoundKilled);
			}
			NPCLoot();
			if ((Type >= (int)ID.GOBLIN_PEON && Type <= (int)ID.GOBLIN_SORCERER) || Type == (int)ID.GOBLIN_ARCHER || (Type >= (int)ID.SNOWMAN_GANGSTA && Type <= (int)ID.SNOW_BALLA))
			{
				Main.InvasionSize--;
			}
			Active = 0;
		}

		public unsafe void NPCLoot()
		{
			if (Main.InHardMode && LifeMax > 1 && Damage > 0 && !IsFriendly && XYWH.Y > Main.RockLayerPixels && Type != (int)ID.SLIMER && Value > 0f && Main.Rand.Next(7) == 0)
			{
				Player player = Player.FindClosest(ref XYWH);
				if (player.ZoneEvil)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_NIGHT);
				}
				if (player.zoneHoly)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_LIGHT);
				}
			}
			if (Time.xMas && LifeMax > 1 && Damage > 0 && !IsFriendly && Type != (int)ID.SLIMER && Value > 0f && Main.Rand.Next(13) == 0)
			{
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, Main.Rand.Next((int)Item.ID.BLUE_PRESENT, (int)Item.ID.YELLOW_PRESENT + 1));
			}
			switch ((ID)Type)
			{
			case ID.CLOWN:
				if (!HasDownedClown)
				{
					HasDownedClown = true;
					if (Main.NetMode == (byte)NetModeSetting.SERVER)
					{
						NetMessage.CreateMessage0(7);
						NetMessage.SendMessage();
					}
				}
				break;
			case ID.MIMIC:
				if (Value > 0f)
				{
					switch (Main.Rand.Next(7))
					{
					case 0:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DUAL_HOOK, 1, DoNotBroadcast: false, -1);
						break;
					case 1:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.MAGIC_DAGGER, 1, DoNotBroadcast: false, -1);
						break;
					case 2:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.PHILOSOPHERS_STONE, 1, DoNotBroadcast: false, -1);
						break;
					case 3:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.TITAN_GLOVE, 1, DoNotBroadcast: false, -1);
						break;
					case 4:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.STAR_CLOAK, 1, DoNotBroadcast: false, -1);
						break;
					case 5:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.COMPASS, 1, DoNotBroadcast: false, -1);
						break;
					default:
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.CROSS_NECKLACE, 1, DoNotBroadcast: false, -1);
						break;
					}
				}
				break;
			case ID.WYVERN_HEAD:
			case ID.ARCH_WYVERN_HEAD:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_FLIGHT, Main.Rand.Next(5, 11));
				break;
			case ID.SNOWMAN_GANGSTA:
			case ID.MISTER_STABBY:
			case ID.SNOW_BALLA:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SNOW_BLOCK, Main.Rand.Next(5, 11));
				break;
			case ID.DARK_MUMMY:
				if (Main.Rand.Next(10) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DARK_SHARD);
				}
				break;
			case ID.LIGHT_MUMMY:
				if (Main.Rand.Next(10) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.LIGHT_SHARD);
				}
				break;
			case ID.SEEKER_HEAD:
			case ID.CLINGER:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.CURSED_FLAME, Main.Rand.Next(2, 6));
				break;
			case ID.UNICORN:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.UNICORN_HORN);
				break;
			case ID.WALL_OF_FLESH:
			{
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.PWNHAMMER, 1, DoNotBroadcast: false, -1);
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, Main.Rand.Next((int)Item.ID.SORCERER_EMBLEM, (int)Item.ID.RANGER_EMBLEM+1), 1, DoNotBroadcast: false, -1);
				}
				else
				{
					int num3;
					switch (Main.Rand.Next(3))
					{
					case 0:
						num3 = (int)Item.ID.LASER_RIFLE;
						break;
					case 1:
						num3 = (int)Item.ID.BREAKER_BLADE;
						break;
					default:
						num3 = (int)Item.ID.CLOCKWORK_ASSAULT_RIFLE;
						break;
					}
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, num3, 1, DoNotBroadcast: false, -1);
				}
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					break;
				}
				int num4 = XYWH.X + (Width >> 1) >> 4;
				int num5 = XYWH.Y + (Height >> 1) >> 4;
				int num6 = (Width >> 5) + 1;
				for (int i = num4 - num6; i <= num4 + num6; i++)
				{
					for (int j = num5 - num6; j <= num5 + num6; j++)
					{
						bool flag = false;
						fixed (Tile* ptr = &Main.TileSet[i, j])
						{
							if ((i == num4 - num6 || i == num4 + num6 || j == num5 - num6 || j == num5 + num6) && ptr->IsActive == 0)
							{
								ptr->IsActive = 1;
								ptr->Type = 140;
								WorldGen.SquareTileFrame(i, j);
								flag = true;
							}
							if (ptr->Liquid > 0)
							{
								ptr->Lava = 0;
								ptr->Liquid = 0;
								flag = true;
							}
						}
						if (flag)
						{
							NetMessage.SendTile(i, j);
						}
					}
				}
				break;
			}
#if !VERSION_INITIAL
			case ID.PINCUSHION_ZOMBIE:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.WOODEN_ARROW, Main.Rand.Next(1, 10));
				break;
			case ID.SLIMED_ZOMBIE:
#endif
			case ID.SLIME:
			case ID.MOTHER_SLIME:
			case ID.ILLUMINANT_SLIME:
			case ID.TOXIC_SLUDGE:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GEL, Main.Rand.Next(1, 3));
				break;
			case ID.CORRUPT_SLIME:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GEL, Main.Rand.Next(2, 5));
				break;
			case ID.GASTROPOD:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GEL, Main.Rand.Next(5, 11));
				break;
			case ID.DUNGEON_SLIME:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GOLDEN_KEY);
				break;
			case ID.PIXIE:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.PIXIE_DUST, Main.Rand.Next(1, 4));
				break;
			case ID.DEMON_EYE:
				{
					int num7 = Main.Rand.Next(150);
					if (num7 < 50)
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (num7 == 38) ? (int)Item.ID.BLACK_LENS : (int)Item.ID.LENS);
					}
					break;
				}
			case ID.WEREWOLF:
				if (Main.Rand.Next(60) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.MOON_CHARM, 1, DoNotBroadcast: false, -1);
				}
				break;
			case ID.PIRANHA:
				{
					int num2 = Main.Rand.Next(500);
					if (num2 < 13)
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (num2 == 0) ? (int)Item.ID.ROBOT_HAT : (int)Item.ID.HOOK);
					}
					break;
				}
			case ID.ANGLER_FISH:
				if (Main.Rand.Next(500) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ROBOT_HAT);
				}
				break;
			case ID.ZOMBIE:
			case ID.BALD_ZOMBIE:
				if (Main.Rand.Next(50) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SHACKLE, 1, DoNotBroadcast: false, -1);
				}
				break;
			case ID.DEMON:
				if (Main.Rand.Next(50) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMON_SCYTHE, 1, DoNotBroadcast: false, -1);
				}
				break;
			case ID.VOODOO_DEMON:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GUIDE_VOODOO_DOLL);
				break;
			case ID.DOCTOR_BONES:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ARCHAEOLOGISTS_HAT);
				break;
			case ID.THE_GROOM:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.TOP_HAT);
				break;
			case ID.CLOTHIER:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.RED_HAT);
				break;
			case ID.GOLDFISH:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GOLDFISH);
				break;
			case ID.ANTLION:
			case ID.ALBINO_ANTLION:
				if (Main.Rand.Next(7) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ANTLION_MANDIBLE);
				}
				break;
			case ID.GOBLIN_SCOUT:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.TATTERED_CLOTH, Main.Rand.Next(1, 3));
				break;
			case ID.EYE_OF_CTHULHU:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.UNHOLY_ARROW, Main.Rand.Next(20, 50));
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(10, 30));
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(10, 30));
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(10, 30));
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.CORRUPT_SEEDS, Main.Rand.Next(1, 4));
				break;
			case ID.OCRAM:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ADAMANTITE_ORE, Main.Rand.Next(10, 30));
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_BLIGHT, Main.Rand.Next(5, 10));
				if (Main.Rand.Next(3) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DRAGON_MASK + Main.Rand.Next(9));
				}
				break;
			case ID.EATER_OF_SOULS:
			case ID.CORRUPTOR:
				if (Main.Rand.Next(3) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ROTTEN_CHUNK);
				}
				break;
			case ID.DEVOURER_HEAD:
			case ID.DEVOURER_BODY:
			case ID.DEVOURER_TAIL:
				if (Main.Rand.Next(3) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.ROTTEN_CHUNK, Main.Rand.Next(1, 3));
				}
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.WORM_TOOTH, Main.Rand.Next(3, 9));
				break;
			case ID.GIANT_WORM_HEAD:
			case ID.GIANT_WORM_BODY:
			case ID.GIANT_WORM_TAIL:
			case ID.DIGGER_HEAD:
			case ID.DIGGER_BODY:
			case ID.DIGGER_TAIL:
				if (Main.Rand.Next(500) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.WHOOPIE_CUSHION);
				}
				break;
			case ID.CORRUPT_BUNNY:
				if (Main.Rand.Next(75) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.BUNNY_HOOD);
				}
				break;
			case ID.EATER_OF_WORLDS_HEAD:
			case ID.EATER_OF_WORLDS_BODY:
			case ID.EATER_OF_WORLDS_TAIL:
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SHADOW_SCALE, Main.Rand.Next(1, 3));
				}
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(2, 6));
				}
				if (IsBoss)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(10, 30));
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DEMONITE_ORE, Main.Rand.Next(10, 31));
				}
				if (Main.Rand.Next(3) == 0 && Player.FindClosest(ref XYWH).canHeal())
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HEART);
				}
				break;
			case ID.THE_HUNGRY_II:
			case ID.LEECH_HEAD:
			case ID.LEECH_BODY:
			case ID.LEECH_TAIL:
			case ID.PROBE:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HEART);
				break;
			case ID.BLUE_JELLYFISH:
			case ID.PINK_JELLYFISH:
			case ID.GREEN_JELLYFISH:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GLOWSTICK, Main.Rand.Next(1, 5));
				break;
			case ID.SKELETON:
			case ID.UNDEAD_MINER:
			case ID.VAMPIRE_MINER:
				if (Main.Rand.Next(25) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HOOK);
				}
				else if (Type != (int)ID.SKELETON)
				{
					if (Main.Rand.Next(20) == 0)
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, Main.Rand.Next((int)Item.ID.MINING_SHIRT, (int)Item.ID.MINING_PANTS + 1));
					}
					else
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.BOMB, Main.Rand.Next(1, 4));
					}
				}
				break;
			case ID.TIM:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.WIZARD_HAT);
				break;
			case ID.KING_SLIME:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, Main.Rand.Next((int)Item.ID.NINJA_HOOD, (int)Item.ID.NINJA_PANTS + 1));
				break;
			case ID.METEOR_HEAD:
				if (Main.Rand.Next(50) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.METEORITE);
				}
				break;
			case ID.FIRE_IMP:
				if (Main.Rand.Next(300) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.PLUMBERS_HAT);
				}
				break;
			case ID.BONES:
			case ID.DARK_CASTER:
			case ID.CURSED_SKULL:
				if (Main.Rand.Next(65) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GOLDEN_KEY);
				}
				else
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.BONE, Main.Rand.Next(1, 4));
				}
				break;
			case ID.GOBLIN_PEON:
			case ID.GOBLIN_THIEF:
			case ID.GOBLIN_WARRIOR:
			case ID.GOBLIN_SORCERER:
			case ID.GOBLIN_ARCHER:
				{
					int num = Main.Rand.Next(200);
					if (num < 100)
					{
						if (num == 0)
						{
							Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HARPOON);
						}
						else
						{
							Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SPIKY_BALL, Main.Rand.Next(1, 6));
						}
					}
					break;
				}
			case ID.HORNET:
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.STINGER);
				}
				break;
#if VERSION_103 || VERSION_FINAL
			case ID.DRAGON_HORNET:
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.STINGER, Main.Rand.Next(1, 4));
				}
				break;
#endif
			case ID.MAN_EATER:
				if (Main.Rand.Next(4) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.VINE);
				}
				break;
			case ID.SHARK:
				if (Main.Rand.Next(50) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DIVING_HELMET);
				}
				else
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SHARK_FIN);
				}
				break;
			case ID.ORKA:
				if (Main.Rand.Next(25) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.DIVING_HELMET);
				}
				break;
			case ID.HARPY:
				if (Main.Rand.Next(2) == 0)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.FEATHER);
				}
				break;
			case ID.RETINAZER:
			case ID.SPAZMATISM:
				if (!AnyNPCs((Type == (int)ID.RETINAZER) ? (int)ID.SPAZMATISM : (int)ID.RETINAZER))
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_SIGHT, Main.Rand.Next(20, 31));
					break;
				}
				Value = 0f;
				IsBoss = false;
				break;
			case ID.SKELETRON_PRIME:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_FRIGHT, Main.Rand.Next(20, 31));
				break;
			case ID.THE_DESTROYER_HEAD:
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SOUL_OF_MIGHT, Main.Rand.Next(20, 31));
				break;
			}
			if (IsBoss)
			{
				string prefix = DisplayName;
				switch ((ID)Type)
				{
					case ID.EYE_OF_CTHULHU:
						HasDownedBoss1 = true;
						break;
					case ID.EATER_OF_WORLDS_HEAD:
					case ID.EATER_OF_WORLDS_BODY:
					case ID.EATER_OF_WORLDS_TAIL:
						HasDownedBoss2 = true;
						break;
					case ID.SKELETRON_HEAD:
						HasDownedBoss3 = true;
						break;
					case ID.RETINAZER:
					case ID.SPAZMATISM:
						prefix = Lang.MiscText[20];
						UI.SetTriggerStateForAll(Trigger.KilledTheTwins);
						break;
					case ID.SKELETRON_PRIME:
						UI.SetTriggerStateForAll(Trigger.KilledSkeletronPrime);
						break;
					case ID.THE_DESTROYER_HEAD:
						UI.SetTriggerStateForAll(Trigger.KilledDestroyer);
						break;
				}
				short num8 = NetID;
				if (RealLife > 0)
				{
					num8 = Main.NPCSet[RealLife].NetID;
				}
				UI.IncreaseStatisticForAll(Statistics.GetBossStatisticEntryFromNetID(num8));
				int num9 = (int)Item.ID.LESSER_HEALING_POTION;
				if (Type == (int)ID.WALL_OF_FLESH)
				{
					num9 = (int)Item.ID.HEALING_POTION;
				}
				else if (Type > (int)ID.WALL_OF_FLESH)
				{
					num9 = (int)Item.ID.GREATER_HEALING_POTION;
				}
				Item.NewItem(XYWH.X, XYWH.Y, Width, Height, num9, Main.Rand.Next(5, 16));
				for (int num10 = Main.Rand.Next(5, 10); num10 > 0; num10--)
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HEART);
				}
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (Type == (int)ID.WALL_OF_FLESH)
					{
						WorldGen.StartHardmode();
					}
					NetMessage.SendText(prefix, 17, 175, 75, 255, -1);
					NetMessage.CreateMessage0(7);
					NetMessage.SendMessage();
				}
			}
			if (LifeMax > 1 && Damage > 0)
			{
				Player player2 = Player.FindClosest(ref XYWH);
				if (Main.Rand.Next(6) == 0)
				{
					if (Main.Rand.Next(2) == 0 && player2.canUseMana())
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.STAR);
					}
					else if (Main.Rand.Next(2) == 0 && player2.canHeal())
					{
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.HEART);
					}
				}
				if (Main.Rand.Next(2) == 0 && player2.canUseMana())
				{
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.STAR);
				}
			}
			float num11 = Value;
			num11 *= 1f + Main.Rand.Next(-20, 21) * 0.01f;
			if (Main.Rand.Next(5) == 0)
			{
				num11 *= 1f + Main.Rand.Next(5, 11) * 0.01f;
			}
			if (Main.Rand.Next(10) == 0)
			{
				num11 *= 1f + Main.Rand.Next(10, 21) * 0.01f;
			}
			if (Main.Rand.Next(15) == 0)
			{
				num11 *= 1f + Main.Rand.Next(15, 31) * 0.01f;
			}
			if (Main.Rand.Next(20) == 0)
			{
				num11 *= 1f + Main.Rand.Next(20, 41) * 0.01f;
			}
			int num12 = (int)num11;
			while (num12 > 0)
			{
				if (num12 > 1000000)
				{
					int num13 = num12 / 1000000;
					if (num13 > 50 && Main.Rand.Next(5) == 0)
					{
						num13 /= Main.Rand.Next(1, 4);
					}
					if (Main.Rand.Next(5) == 0)
					{
						num13 /= Main.Rand.Next(1, 4);
					}
					if (num13 > 0)
					{
						num12 -= 1000000 * num13;
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.PLATINUM_COIN, num13);
					}
				}
				else if (num12 > 10000)
				{
					int num14 = num12 / 10000;
					if (num14 > 50 && Main.Rand.Next(5) == 0)
					{
						num14 /= Main.Rand.Next(1, 4);
					}
					if (Main.Rand.Next(5) == 0)
					{
						num14 /= Main.Rand.Next(1, 4);
					}
					if (num14 > 0)
					{
						num12 -= 10000 * num14;
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.GOLD_COIN, num14);
					}
				}
				else if (num12 > 100)
				{
					int num15 = num12 / 100;
					if (num15 > 50 && Main.Rand.Next(5) == 0)
					{
						num15 /= Main.Rand.Next(1, 4);
					}
					if (Main.Rand.Next(5) == 0)
					{
						num15 /= Main.Rand.Next(1, 4);
					}
					if (num15 > 0)
					{
						num12 -= 100 * num15;
						Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.SILVER_COIN, num15);
					}
				}
				else if (num12 > 0)
				{
					int num16 = num12;
					if (num16 > 50 && Main.Rand.Next(5) == 0)
					{
						num16 /= Main.Rand.Next(1, 4);
					}
					if (Main.Rand.Next(5) == 0)
					{
						num16 /= Main.Rand.Next(1, 4);
					}
					if (num16 < 1)
					{
						num16 = 1;
					}
					num12 -= num16;
					Item.NewItem(XYWH.X, XYWH.Y, Width, Height, (int)Item.ID.COPPER_COIN, num16);
				}
			}
		}

		public unsafe void HitEffect(int hitDirection = 0, double dmg = 10.0)
		{
			if (Type == (int)ID.SLIME || Type == (int)ID.MOTHER_SLIME || Type == (int)ID.DUNGEON_SLIME)
			{
				if (Life > 0)
				{
					int num = (int)(dmg / LifeMax * 80.0);
					while (num > 0 && null != Main.DustSet.NewDust(4, ref XYWH, hitDirection, -1.0, Alpha, Colour))
					{
						num--;
					}
					return;
				}
				for (int i = 0; i < 48; i++)
				{
					if (null == Main.DustSet.NewDust(4, ref XYWH, 2 * hitDirection, -2.0, Alpha, Colour))
					{
						break;
					}
				}
				if (Type != (int)ID.MOTHER_SLIME || Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					return;
				}
				for (int num2 = Main.Rand.Next(1, 3); num2 >= 0; num2--)
				{
					int num3 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, (int)ID.SLIME);
					if (num3 < MaxNumNPCs)
					{
						Main.NPCSet[num3].SetDefaults("Baby Slime");
						Main.NPCSet[num3].Velocity.X = Velocity.X * 2f;
						Main.NPCSet[num3].Velocity.Y = Velocity.Y;
						Main.NPCSet[num3].Velocity.X += Main.Rand.Next(-20, 20) * 0.1f + num2 * Direction * 0.3f;
						Main.NPCSet[num3].Velocity.Y -= Main.Rand.Next(10) * 0.1f + num2;
						Main.NPCSet[num3].AI1 = num2;
						NetMessage.CreateMessage1(23, num3);
						NetMessage.SendMessage();
					}
				}
			}
			else if (Type == (int)ID.SNOWMAN_GANGSTA || Type == (int)ID.MISTER_STABBY || Type == (int)ID.SNOW_BALLA)
			{
				if (Life > 0)
				{
					for (int num4 = (int)(dmg / LifeMax * 80.0); num4 > 0; num4--)
					{
						Dust* ptr = Main.DustSet.NewDust(76, ref XYWH, hitDirection, -1.0);
						if (ptr == null)
						{
							break;
						}
						ptr->NoGravity = true;
					}
					return;
				}
				for (int j = 0; j < 32; j++)
				{
					Dust* ptr = Main.DustSet.NewDust(76, ref XYWH, hitDirection, -1.0);
					if (ptr == null)
					{
						break;
					}
					ptr->NoGravity = true;
					ptr->Scale *= 1.2f;
				}
			}
			else if (Type == (int)ID.TOXIC_SLUDGE)
			{
				if (Life > 0)
				{
					int num5 = (int)(dmg / LifeMax * 80.0);
					while (num5 > 0 && null != Main.DustSet.NewDust(4, ref XYWH, hitDirection, -1.0, Alpha, new Color(210, 230, 140)))
					{
						num5--;
					}
					return;
				}
				for (int k = 0; k < 40; k++)
				{
					if (null == Main.DustSet.NewDust(4, ref XYWH, 2 * hitDirection, -2.0, Alpha, new Color(210, 230, 140)))
					{
						break;
					}
				}
			}
			else if (Type == (int)ID.VILE_SPIT)
			{
				for (int l = 0; l < 16; l++)
				{
					Dust* ptr2 = Main.DustSet.NewDust(XYWH.X, XYWH.Y + 2, Width, Height, 18, 0.0, 0.0, 100, default(Color), 2.0);
					if (ptr2 == null)
					{
						break;
					}
					if (Main.Rand.Next(2) == 0)
					{
						ptr2->Scale *= 0.6f;
						continue;
					}
					ptr2->Velocity.X *= 1.4f;
					ptr2->Velocity.Y *= 1.4f;
					ptr2->NoGravity = true;
				}
			}
			else if (Type == (int)ID.CORRUPT_SLIME || Type == (int)ID.SHADOW_SLIME || Type == (int)ID.SLIMER)
			{
				if (Life > 0)
				{
					int num6 = (int)(dmg / LifeMax * 80.0);
					while (num6 > 0 && null != Main.DustSet.NewDust(14, ref XYWH, 0.0, 0.0, Alpha, Colour))
					{
						num6--;
					}
					return;
				}
				for (int m = 0; m < 42; m++)
				{
					Dust* ptr3 = Main.DustSet.NewDust(14, ref XYWH, hitDirection, 0.0, Alpha, Colour);
					if (ptr3 == null)
					{
						break;
					}
					ptr3->Velocity.X *= 2f;
					ptr3->Velocity.Y *= 2f;
				}
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					return;
				}
				if (Type == (int)ID.SLIMER)
				{
					int num7 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, (int)ID.CORRUPT_SLIME);
					if (num7 < MaxNumNPCs)
					{
						Main.NPCSet[num7].SetDefaults("Slimer2");
						Main.NPCSet[num7].Velocity.X = Velocity.X;
						Main.NPCSet[num7].Velocity.Y = Velocity.Y;
						Gore.NewGore(Position, Velocity, 94, Scale);
						NetMessage.CreateMessage1(23, num7);
						NetMessage.SendMessage();
					}
				}
				else
				{
					if (!(Scale >= 1f))
					{
						return;
					}
					string defaults = ((Type == (int)ID.CORRUPT_SLIME) ? "Slimeling" : "Slimeling2");
					for (int num8 = Main.Rand.Next(1, 3); num8 >= 0; num8--)
					{
						int num9 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, (int)ID.SLIME);
						if (num9 >= MaxNumNPCs)
						{
							break;
						}
						Main.NPCSet[num9].SetDefaults(defaults);
						Main.NPCSet[num9].Velocity.X = Velocity.X * 3f;
						Main.NPCSet[num9].Velocity.Y = Velocity.Y;
						Main.NPCSet[num9].Velocity.X += Main.Rand.Next(-10, 10) * 0.1f + num8 * Direction * 0.3f;
						Main.NPCSet[num9].Velocity.Y -= Main.Rand.Next(10) * 0.1f + num8;
						Main.NPCSet[num9].AI1 = num8;
						NetMessage.CreateMessage1(23, num9);
						NetMessage.SendMessage();
					}
				}
			}
			else if (Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.ILLUMINANT_BAT || Type == (int)ID.ILLUMINANT_SLIME)
			{
				if (Life > 0)
				{
					for (int n = 0; n < dmg / LifeMax * 50.0; n++)
					{
						Dust* ptr4 = Main.DustSet.NewDust(71, ref XYWH, 0.0, 0.0, 200);
						if (ptr4 == null)
						{
							break;
						}
						ptr4->Velocity.X *= 1.5f;
						ptr4->Velocity.Y *= 1.5f;
					}
					return;
				}
				for (int num10 = 0; num10 < 42; num10++)
				{
					Dust* ptr5 = Main.DustSet.NewDust(71, ref XYWH, hitDirection, 0.0, 200);
					if (ptr5 == null)
					{
						break;
					}
					ptr5->Velocity.X *= 1.5f;
					ptr5->Velocity.Y *= 1.5f;
				}
			}
			else if (Type == (int)ID.GASTROPOD || Type == (int)ID.SPECTRAL_GASTROPOD)
			{
				if (Life > 0)
				{
					for (int num11 = 0; num11 < dmg / LifeMax * 50.0; num11++)
					{
						Dust* ptr6 = Main.DustSet.NewDust(72, ref XYWH, 0.0, 0.0, 200);
						if (ptr6 == null)
						{
							break;
						}
						ptr6->Velocity.X *= 1.5f;
						ptr6->Velocity.Y *= 1.5f;
					}
					return;
				}
				for (int num12 = 0; num12 < 42; num12++)
				{
					Dust* ptr7 = Main.DustSet.NewDust(72, ref XYWH, hitDirection, 0.0, 200);
					if (ptr7 == null)
					{
						break;
					}
					ptr7->Velocity.X *= 1.5f;
					ptr7->Velocity.Y *= 1.5f;
				}
			}
			else if (Type == (int)ID.PIXIE)
			{
				if (Life > 0)
				{
					for (int num13 = 0; num13 < dmg / LifeMax * 50.0; num13++)
					{
						if (null == Main.DustSet.NewDust(55, ref XYWH, 0.0, 0.0, 200, Colour))
						{
							break;
						}
					}
					return;
				}
				for (int num14 = 0; num14 < 42; num14++)
				{
					Dust* ptr8 = Main.DustSet.NewDust(55, ref XYWH, hitDirection, 0.0, 200, Colour);
					if (ptr8 == null)
					{
						break;
					}
					ptr8->Velocity.X *= 2f;
					ptr8->Velocity.Y *= 2f;
				}
			}
			else if (Type == (int)ID.BLUE_JELLYFISH || Type == (int)ID.PINK_JELLYFISH || Type == (int)ID.GREEN_JELLYFISH)
			{
				Color newColor = new Color(50, 120, 255, 100);
				if (Type == (int)ID.PINK_JELLYFISH)
				{
					newColor = new Color(225, 70, 140, 100);
				}
				else if (Type == (int)ID.GREEN_JELLYFISH)
				{
					newColor = new Color(70, 225, 140, 100);
				}
				if (Life > 0)
				{
					for (int num15 = 0; num15 < dmg / LifeMax * 50.0; num15++)
					{
						if (null == Main.DustSet.NewDust(4, ref XYWH, hitDirection, -1.0, 0, newColor))
						{
							break;
						}
					}
					return;
				}
				for (int num16 = 0; num16 < 16; num16++)
				{
					if (null == Main.DustSet.NewDust(4, ref XYWH, 2 * hitDirection, -2.0, 0, newColor))
					{
						break;
					}
				}
			}
			else if (Type == (int)ID.LAVA_SLIME || Type == (int)ID.HELLBAT)
			{
				if (Life > 0)
				{
					for (int num17 = 0; num17 < dmg / LifeMax * 80.0; num17++)
					{
						if (null == Main.DustSet.NewDust(6, ref XYWH, hitDirection * 2, -1.0, Alpha, default(Color), 1.5))
						{
							break;
						}
					}
					return;
				}
				for (int num18 = 0; num18 < 32; num18++)
				{
					if (null == Main.DustSet.NewDust(6, ref XYWH, hitDirection * 2, -1.0, Alpha, default(Color), 1.5))
					{
						break;
					}
				}
			}
			else if (Type == (int)ID.KING_SLIME)
			{
				if (Life > 0)
				{
					for (int num19 = 0; num19 < dmg / LifeMax * 300.0; num19++)
					{
						if (null == Main.DustSet.NewDust(4, ref XYWH, hitDirection, -1.0, 175, new Color(0, 80, 255, 100)))
						{
							break;
						}
					}
					return;
				}
				for (int num20 = 0; num20 < 128; num20++)
				{
					if (null == Main.DustSet.NewDust(4, ref XYWH, hitDirection << 1, -2.0, 175, new Color(0, 80, 255, 100)))
					{
						break;
					}
				}
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					return;
				}
				for (int num21 = Main.Rand.Next(3, 7); num21 >= 0; num21--)
				{
					int x = XYWH.X + Main.Rand.Next(Width - 32);
					int y = XYWH.Y + Main.Rand.Next(Height - 32);
					int num22 = NewNPC(x, y, (int)ID.SLIME);
					if (num22 < MaxNumNPCs)
					{
						Main.NPCSet[num22].SetDefaults((int)ID.SLIME);
						Main.NPCSet[num22].Velocity.X = Main.Rand.Next(-15, 16) * 0.1f;
						Main.NPCSet[num22].Velocity.Y = Main.Rand.Next(-30, 1) * 0.1f;
						Main.NPCSet[num22].AI1 = Main.Rand.Next(3);
						NetMessage.CreateMessage1(23, num22);
						NetMessage.SendMessage();
					}
				}
			}
			else if (Type == (int)ID.CAVE_BAT || Type == (int)ID.JUNGLE_BAT || Type == (int)ID.GIANT_BAT)
			{
				if (Life > 0)
				{
					for (int num23 = 0; num23 < dmg / LifeMax * 30.0; num23++)
					{
						if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
						{
							break;
						}
					}
					return;
				}
				for (int num24 = 0; num24 < 12; num24++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				if (Type == (int)ID.JUNGLE_BAT)
				{
					Gore.NewGore(Position, Velocity, 83);
				}
				else if (Type == (int)ID.GIANT_BAT)
				{
					Gore.NewGore(Position, Velocity, 107);
				}
				else
				{
					Gore.NewGore(Position, Velocity, 82);
				}
			}
			else if (Type == (int)ID.BUNNY || Type == (int)ID.GOLDFISH || Type == (int)ID.CRAB || Type == (int)ID.BIRD || Type == (int)ID.ANGLER_FISH)
			{
				if (Life > 0)
				{
					for (int num25 = 0; num25 < dmg / LifeMax * 20.0; num25++)
					{
						if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
						{
							break;
						}
					}
					return;
				}
				for (int num26 = 0; num26 < 8; num26++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				if (Type == (int)ID.BUNNY)
				{
					Gore.NewGore(Position, Velocity, 76);
					Gore.NewGore(Position, Velocity, 77);
				}
				else if (Type == (int)ID.CRAB)
				{
					Gore.NewGore(Position, Velocity, 95);
					Gore.NewGore(Position, Velocity, 95);
					Gore.NewGore(Position, Velocity, 96);
				}
				else if (Type == (int)ID.BIRD)
				{
					Gore.NewGore(Position, Velocity, 100);
				}
				else if (Type == (int)ID.ANGLER_FISH)
				{
					Gore.NewGore(Position, Velocity, 116);
				}
			}
			else if (Type == (int)ID.CORRUPT_BUNNY || Type == (int)ID.CORRUPT_GOLDFISH || Type == (int)ID.PIRANHA)
			{
				if (Life > 0)
				{
					for (int num27 = 0; num27 < dmg / LifeMax * 20.0; num27++)
					{
						if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
						{
							break;
						}
					}
					return;
				}
				for (int num28 = 0; num28 < 8; num28++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				if (Type == (int)ID.CORRUPT_GOLDFISH)
				{
					Gore.NewGore(Position, Velocity, 84);
					return;
				}
				if (Type == (int)ID.PIRANHA)
				{
					Gore.NewGore(Position, Velocity, 85);
					return;
				}
				Gore.NewGore(Position, Velocity, 78);
				Gore.NewGore(Position, Velocity, 79);
			}
			else if (Type == (int)ID.DEMON_EYE || Type == (int)ID.CATARACT_EYE || Type == (int)ID.SLEEPY_EYE || Type == (int)ID.DIALATED_EYE || Type == (int)ID.GREEN_EYE || Type == (int)ID.PURPLE_EYE)
			{
				if (Life > 0)
				{
					int num29 = (int)(dmg / LifeMax * 80.0);
					while (num29 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num29--;
					}
					return;
				}
				for (int num30 = 0; num30 < 42; num30++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				if (Type == (int)ID.CATARACT_EYE)
				{
					Gore.NewGore(Position, Velocity, 175, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 2, Scale);
				}
				else if (Type == (int)ID.SLEEPY_EYE)
				{
					Gore.NewGore(Position, Velocity, 176, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 2, Scale);
				}
				else if (Type == (int)ID.DIALATED_EYE)
				{
					Gore.NewGore(Position, Velocity, 177, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 2, Scale);
				}
				else if (Type == (int)ID.GREEN_EYE)
				{
					Gore.NewGore(Position, Velocity, 178, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 179, Scale);
				}
				else if (Type == (int)ID.PURPLE_EYE)
				{
					Gore.NewGore(Position, Velocity, 180, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 181, Scale);
				}
				else
				{
					Gore.NewGore(Position, Velocity, 1, Scale);
					Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 2, Scale);
				}
			}
			else if (Type == (int)ID.WANDERING_EYE)
			{
				if (Life > 0)
				{
					int num31 = (int)(dmg / LifeMax * 80.0);
					while (num31 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num31--;
					}
					if (Life < LifeMax >> 1 && LocalAI0 == 0)
					{
						LocalAI0 = 1;
						Gore.NewGore(Position, Velocity, 1);
					}
					return;
				}
				for (int num32 = 0; num32 < 48; num32++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				Gore.NewGore(Position, Velocity, 155);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 14f), Velocity, 155);
			}
			else if (Type == (int)ID.ANTLION || Type == (int)ID.ALBINO_ANTLION)
			{
				if (Life > 0)
				{
					int num33 = (int)(dmg / LifeMax * 80.0);
					while (num33 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num33--;
					}
					return;
				}
				for (int num34 = 0; num34 < 42; num34++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				int num35 = ((Type == (int)ID.ANTLION) ? 97 : 160);
				Gore.NewGore(Position, Velocity, num35);
				Gore.NewGore(Position, Velocity, ++num35);
			}
			else if (Type == (int)ID.VULTURE)
			{
				if (Life > 0)
				{
					int num36 = (int)(dmg / LifeMax * 80.0);
					while (num36 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num36--;
					}
					return;
				}
				for (int num37 = 0; num37 < 42; num37++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				Gore.NewGore(Position, Velocity, 86);
				Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 87);
				Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), Velocity, 88);
			}
			else if (Type == (int)ID.SHARK || Type == (int)ID.ORKA)
			{
				if (Life > 0)
				{
					for (int num38 = 0; num38 < dmg / LifeMax * 150.0; num38++)
					{
						if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
						{
							break;
						}
					}
					return;
				}
				for (int num39 = 0; num39 < 60; num39++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				int num40 = ((Type == (int)ID.SHARK) ? 89 : 162);
				Vector2 vector = Velocity;
				vector.X *= 0.8f;
				vector.Y *= 0.8f;
				Gore.NewGore(Position, vector, num40);
				Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), vector, ++num40);
				Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), vector, ++num40);
				Gore.NewGore(new Vector2(Position.X + 14f, Position.Y), vector, ++num40);
			}
			else if (Type == (int)ID.ZOMBIE || Type == (int)ID.DOCTOR_BONES || Type == (int)ID.THE_GROOM || Type == (int)ID.WEREWOLF || Type == (int)ID.CLOWN || Type == (int)ID.BALD_ZOMBIE || Type == (int)ID.PINCUSHION_ZOMBIE || Type == (int)ID.SLIMED_ZOMBIE || Type == (int)ID.SWAMP_ZOMBIE || Type == (int)ID.TWIGGY_ZOMBIE || Type == (int)ID.FEMALE_ZOMBIE)
			{
				if (Life > 0)
				{
					int num41 = (int)(dmg / LifeMax * 80.0);
					while (num41 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num41--;
					}
					if (Type == (int)ID.PINCUSHION_ZOMBIE && Main.Rand.Next(5) == 0)
					{
						Gore.NewGore(Position, Velocity, 183);
					}
					if (Type == (int)ID.SLIMED_ZOMBIE)
					{
						Rectangle ZombieGore = XYWH;
						ZombieGore.Height = 24;
						for (int num160 = 0; num160 < dmg / LifeMax * 200.0; num160++)
						{
							Main.DustSet.NewDust(4, ref ZombieGore, hitDirection, -1f, 125, new Color(0, 80, 255, 100));
						}
					}
					return;
				}
				for (int num42 = 0; num42 < 42; num42++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5))
					{
						break;
					}
				}
				if (Type == (int)ID.SLIMED_ZOMBIE)
				{
					Rectangle ZombieGore = XYWH;
					ZombieGore.Height = 24;
					for (int num160 = 0; num160 < 25; num160++)
					{
						Main.DustSet.NewDust(4, ref ZombieGore, hitDirection, -1f, 125, new Color(0, 80, 255, 100));
					}
				}
				if (Type == (int)ID.WEREWOLF)
				{
					Gore.NewGore(Position, Velocity, 117, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 118, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 118, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 119, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 119, Scale);
					return;
				}
				if (Type == (int)ID.CLOWN)
				{
					Gore.NewGore(Position, Velocity, 121, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 122, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 122, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 123, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 123, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 46f), Velocity, 120, Scale);
					return;
				}
				if (Type == (int)ID.SWAMP_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 184, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 185, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 185, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 186, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 186, Scale);
					return;
				}
				if (Type == (int)ID.FEMALE_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 187, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 188, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 188, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 189, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 189, Scale);
					return;
				}
				if (Type == (int)ID.BALD_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 154);
				}
				else if (Type == (int)ID.PINCUSHION_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 182);
				}
				else if (Type == (int)ID.TWIGGY_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 190);
				}
				else if (Type != (int)ID.SLIMED_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 3);
				}
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 4);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 4);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 5);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 5);
				if (Type == (int)ID.PINCUSHION_ZOMBIE)
				{
					Gore.NewGore(Position, Velocity, 183);
				}
				if (Type == (int)ID.PINCUSHION_ZOMBIE && Main.Rand.Next(2) == 0)
				{
					Gore.NewGore(Position, Velocity, 183);
				}
			}
			else if (Type == (int)ID.CURSED_HAMMER || Type == (int)ID.ENCHANTED_SWORD || Type == (int)ID.SHADOW_HAMMER)
			{
				if (Life > 0)
				{
					for (int num43 = 0; num43 < dmg / LifeMax * 50.0; num43++)
					{
						Dust* ptr9 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
						if (ptr9 == null)
						{
							break;
						}
						ptr9->NoGravity = true;
					}
					return;
				}
				for (int num44 = 0; num44 < 16; num44++)
				{
					Dust* ptr10 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
					if (ptr10 == null)
					{
						break;
					}
					ptr10->Velocity.X *= 2f;
					ptr10->Velocity.Y *= 2f;
					ptr10->NoGravity = true;
				}
				int num45 = Gore.NewGore(new Vector2(Position.X, Position.Y + ((Height >> 1) - 10)), new Vector2(Main.Rand.Next(-2, 3), Main.Rand.Next(-2, 3)), 61, Scale);
				Main.GoreSet[num45].Velocity *= 0.5f;
				num45 = Gore.NewGore(new Vector2(Position.X, Position.Y + ((Height >> 1) - 10)), new Vector2(Main.Rand.Next(-2, 3), Main.Rand.Next(-2, 3)), 61, Scale);
				Main.GoreSet[num45].Velocity *= 0.5f;
				num45 = Gore.NewGore(new Vector2(Position.X, Position.Y + ((Height >> 1) - 10)), new Vector2(Main.Rand.Next(-2, 3), Main.Rand.Next(-2, 3)), 61, Scale);
				Main.GoreSet[num45].Velocity *= 0.5f;
			}
			else if (Type == (int)ID.EYE_OF_CTHULHU || Type == (int)ID.SPAZMATISM || Type == (int)ID.RETINAZER)
			{
				if (Life > 0)
				{
					int num46 = (int)(dmg / LifeMax * 80.0);
					while (num46 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num46--;
					}
					return;
				}
				for (int num47 = 0; num47 < Dust.MaxNumLocalDust; num47++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				for (int num48 = 0; num48 < 2; num48++)
				{
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 2);
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 7);
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 9);
					if (Type == (int)ID.EYE_OF_CTHULHU)
					{
						Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 10);
						Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
					}
					else if (Type == (int)ID.RETINAZER)
					{
						Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 146);
					}
					else if (Type == (int)ID.SPAZMATISM)
					{
						Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 145);
					}
				}
				if (Type != (int)ID.RETINAZER && Type != (int)ID.SPAZMATISM)
				{
					return;
				}
				for (int num49 = 0; num49 < 8; num49++)
				{
					Dust* ptr11 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr11 == null)
					{
						break;
					}
					ptr11->Velocity.X *= 1.4f;
					ptr11->Velocity.Y *= 1.4f;
				}
				for (int num50 = 0; num50 < 4; num50++)
				{
					Dust* ptr12 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
					if (ptr12 == null)
					{
						break;
					}
					ptr12->NoGravity = true;
					ptr12->Velocity.X *= 5f;
					ptr12->Velocity.Y *= 5f;
					ptr12 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr12 == null)
					{
						break;
					}
					ptr12->Velocity.X *= 3f;
					ptr12->Velocity.Y *= 3f;
				}
				int num51 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num51].Velocity.X *= 0.4f;
				Main.GoreSet[num51].Velocity.X += 1f;
				Main.GoreSet[num51].Velocity.Y *= 0.4f;
				Main.GoreSet[num51].Velocity.Y += 1f;
				num51 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num51].Velocity.X *= 0.4f;
				Main.GoreSet[num51].Velocity.X -= 1f;
				Main.GoreSet[num51].Velocity.Y *= 0.4f;
				Main.GoreSet[num51].Velocity.Y += 1f;
				num51 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num51].Velocity.X *= 0.4f;
				Main.GoreSet[num51].Velocity.X += 1f;
				Main.GoreSet[num51].Velocity.Y *= 0.4f;
				Main.GoreSet[num51].Velocity.Y -= 1f;
				num51 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num51].Velocity.X *= 0.4f;
				Main.GoreSet[num51].Velocity.X -= 1f;
				Main.GoreSet[num51].Velocity.Y *= 0.4f;
				Main.GoreSet[num51].Velocity.Y -= 1f;
			}
			else if (Type == (int)ID.OCRAM)
			{
				if (Life > 0)
				{
					int num52 = (int)(dmg / LifeMax * 80.0);
					while (num52 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num52--;
					}
					return;
				}
				for (int num53 = 0; num53 < 128; num53++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				for (int num54 = 0; num54 < 2; num54++)
				{
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 172);
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 173);
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 174);
					Gore.NewGore(Position, new Vector2(Main.Rand.Next(-30, 31) * 0.2f, Main.Rand.Next(-30, 31) * 0.2f), 172);
					Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
				}
			}
			else if (Type == (int)ID.SERVANT_OF_CTHULHU)
			{
				if (Life > 0)
				{
					for (int num55 = 0; num55 < dmg / LifeMax * 50.0; num55++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num56 = 0; num56 < 16; num56++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0);
				}
				Gore.NewGore(Position, Velocity, 6);
				Gore.NewGore(Position, Velocity, 7);
			}
			else if (Type == (int)ID.SERVANT_OF_OCRAM)
			{
				if (Life > 0)
				{
					for (int num57 = 0; num57 < dmg / LifeMax * 50.0; num57++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
				}
				else
				{
					for (int num58 = 0; num58 < 16; num58++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0);
					}
				}
			}
			else if (Type == (int)ID.WALL_OF_FLESH || Type == (int)ID.WALL_OF_FLESH_EYE)
			{
				if (Life > 0)
				{
					for (int num59 = 0; num59 < 16; num59++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num60 = 0; num60 < 42; num60++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -1.0);
				}
				Gore.NewGore(Position, Velocity, 137, Scale);
				if (Type == (int)ID.WALL_OF_FLESH_EYE)
				{
					Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1)), Velocity, 139, Scale);
					Gore.NewGore(new Vector2(Position.X + (Width >> 1), Position.Y), Velocity, 139, Scale);
					Gore.NewGore(new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1)), Velocity, 137, Scale);
				}
				else
				{
					Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1)), Velocity, 138, Scale);
					Gore.NewGore(new Vector2(Position.X + (Width >> 1), Position.Y), Velocity, 138, Scale);
					Gore.NewGore(new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1)), Velocity, 137, Scale);
				}
			}
			else if (Type == (int)ID.THE_HUNGRY || Type == (int)ID.THE_HUNGRY_II)
			{
				if (Life > 0)
				{
					for (int num61 = 0; num61 < 4; num61++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				if (Type == (int)ID.THE_HUNGRY && Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, (int)ID.THE_HUNGRY_II);
					for (int num62 = 0; num62 < 8; num62++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num63 = 0; num63 < 16; num63++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
				}
				Gore.NewGore(Position, Velocity, 132, Scale);
				Gore.NewGore(Position, Velocity, 133, Scale);
			}
			else if (Type >= (int)ID.LEECH_HEAD && Type <= (int)ID.LEECH_TAIL)
			{
				if (Life > 0)
				{
					for (int num64 = 0; num64 < 4; num64++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num65 = 0; num65 < 8; num65++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
				}
				Gore.NewGore(Position, Velocity, 134 + Type - (int)ID.LEECH_HEAD, Scale);
			}
			else if (Type == (int)ID.EATER_OF_SOULS || Type == (int)ID.CORRUPTOR)
			{
				if (Life > 0)
				{
					for (int num66 = (int)(dmg / LifeMax * 80.0); num66 > 0; num66--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num67 = 0; num67 < 42; num67++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				if (Type == (int)ID.CORRUPTOR)
				{
					int num68 = Gore.NewGore(Position, Velocity, 108, Scale);
					num68 = Gore.NewGore(Position, Velocity, 108, Scale);
					num68 = Gore.NewGore(Position, Velocity, 109, Scale);
					num68 = Gore.NewGore(Position, Velocity, 110, Scale);
				}
				else
				{
					int num68 = Gore.NewGore(Position, Velocity, 14, Scale);
					Main.GoreSet[num68].Alpha = Alpha;
					num68 = Gore.NewGore(Position, Velocity, 15, Scale);
					Main.GoreSet[num68].Alpha = Alpha;
				}
			}
			else if (Type == (int)ID.CLINGER)
			{
				if (Life > 0)
				{
					for (int num69 = (int)(dmg / LifeMax * 80.0); num69 > 0; num69--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num70 = 0; num70 < 42; num70++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				Gore.NewGore(Position, Velocity, 110, Scale);
				Gore.NewGore(Position, Velocity, 114, Scale);
				Gore.NewGore(Position, Velocity, 114, Scale);
				Gore.NewGore(Position, Velocity, 115, Scale);
			}
			else if (Type >= (int)ID.DEVOURER_HEAD && Type <= (int)ID.DEVOURER_TAIL)
			{
				if (Life > 0)
				{
					for (int num71 = (int)(dmg / LifeMax * 80.0); num71 > 0; num71--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num72 = 0; num72 < 42; num72++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				int num73 = Gore.NewGore(Position, Velocity, Type - (int)ID.DEVOURER_HEAD + 18);
				Main.GoreSet[num73].Alpha = Alpha;
			}
			else if (Type >= (int)ID.SEEKER_HEAD && Type <= (int)ID.SEEKER_TAIL)
			{
				if (Life > 0)
				{
					for (int num74 = (int)(dmg / LifeMax * 80.0); num74 > 0; num74--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num75 = 0; num75 < 42; num75++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				int num76 = Gore.NewGore(Position, Velocity, 110);
				Main.GoreSet[num76].Alpha = Alpha;
			}
			else if (Type >= (int)ID.GIANT_WORM_HEAD && Type <= (int)ID.GIANT_WORM_TAIL)
			{
				if (Life > 0)
				{
					for (int num77 = 0; num77 < dmg / LifeMax * 50.0; num77++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num78 = 0; num78 < 8; num78++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, Type - (int)ID.GIANT_WORM_HEAD + 21);
			}
			else if (Type >= (int)ID.DIGGER_HEAD && Type <= (int)ID.DIGGER_TAIL)
			{
				if (Life > 0)
				{
					for (int num79 = 0; num79 < dmg / LifeMax * 50.0; num79++)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num80 = 0; num80 < 8; num80++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, Type - (int)ID.DIGGER_HEAD + 111);
			}
			else if (Type >= (int)ID.EATER_OF_WORLDS_HEAD && Type <= (int)ID.EATER_OF_WORLDS_TAIL)
			{
				if (Life > 0)
				{
					for (int num81 = (int)(dmg / LifeMax * 80.0); num81 > 0; num81--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num82 = 0; num82 < 42; num82++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				if (Type == (int)ID.EATER_OF_WORLDS_HEAD)
				{
					Gore.NewGore(Position, Velocity, 24);
					Gore.NewGore(Position, Velocity, 25);
				}
				else if (Type == (int)ID.EATER_OF_WORLDS_BODY)
				{
					Gore.NewGore(Position, Velocity, 26);
					Gore.NewGore(Position, Velocity, 27);
				}
				else
				{
					Gore.NewGore(Position, Velocity, 28);
					Gore.NewGore(Position, Velocity, 29);
				}
			}
			else if (Type == (int)ID.MERCHANT)
			{
				if (Life > 0)
				{
					for (int num83 = (int)(dmg / LifeMax * 80.0); num83 > 0; num83--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num84 = 0; num84 < 42; num84++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 30);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 31);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 31);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 32);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 32);
			}
			else if (Type == (int)ID.UNICORN)
			{
				if (Life > 0)
				{
					for (int num85 = (int)(dmg / LifeMax * 80.0); num85 > 0; num85--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num86 = 0; num86 < 42; num86++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 101);
				Gore.NewGore(Position, Velocity, 102);
				Gore.NewGore(Position, Velocity, 103);
				Gore.NewGore(Position, Velocity, 103);
				Gore.NewGore(Position, Velocity, 104);
				Gore.NewGore(Position, Velocity, 104);
				Gore.NewGore(Position, Velocity, 105);
			}
			else if (Type >= (int)ID.BOUND_GOBLIN && Type <= (int)ID.WIZARD)
			{
				if (Life > 0)
				{
					for (int num87 = (int)(dmg / LifeMax * 80.0); num87 > 0; num87--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num88 = 0; num88 < 42; num88++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				if (Type == (int)ID.BOUND_GOBLIN || Type == (int)ID.GOBLIN_TINKERER)
				{
					Gore.NewGore(Position, Velocity, 124);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 125);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 125);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 126);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 126);
				}
				else
				{
					Gore.NewGore(Position, Velocity, 127);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 128);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 128);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 129);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 129);
				}
			}
			else if (Type == (int)ID.BOUND_MECHANIC || Type == (int)ID.MECHANIC)
			{
				if (Life > 0)
				{
					for (int num89 = (int)(dmg / LifeMax * 80.0); num89 > 0; num89--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num90 = 0; num90 < 42; num90++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 151);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 152);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 152);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 153);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 153);
			}
			else if (Type == (int)ID.GUIDE)
			{
				if (Life > 0)
				{
					for (int num91 = (int)(dmg / LifeMax * 80.0); num91 > 0; num91--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num92 = 0; num92 < 42; num92++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 73);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 74);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 74);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 75);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 75);
			}
			else if (Type == (int)ID.SANTA_CLAUS)
			{
				if (Life > 0)
				{
					for (int num93 = (int)(dmg / LifeMax * 80.0); num93 > 0; num93--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num94 = 0; num94 < 42; num94++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 157);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 158);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 158);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 159);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 159);
			}
			else if (Type == (int)ID.OLD_MAN || Type == (int)ID.CLOTHIER)
			{
				if (Life > 0)
				{
					for (int num95 = (int)(dmg / LifeMax * 80.0); num95 > 0; num95--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num96 = 0; num96 < 42; num96++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 58);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 59);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 59);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 60);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 60);
			}
			else if (Type == (int)ID.NURSE)
			{
				if (Life > 0)
				{
					for (int num97 = (int)(dmg / LifeMax * 80.0); num97 > 0; num97--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num98 = 0; num98 < 42; num98++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 33);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 34);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 34);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 35);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 35);
			}
			else if (Type == (int)ID.ARMS_DEALER)
			{
				if (Life > 0)
				{
					for (int num99 = (int)(dmg / LifeMax * 80.0); num99 > 0; num99--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num100 = 0; num100 < 42; num100++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 36);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 37);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 37);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 38);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 38);
			}
			else if (Type == (int)ID.DEMOLITIONIST)
			{
				if (Life > 0)
				{
					for (int num101 = (int)(dmg / LifeMax * 80.0); num101 > 0; num101--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num102 = 0; num102 < 42; num102++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 64);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 65);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 65);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 66);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 66);
			}
			else if (Type == (int)ID.DRYAD)
			{
				if (Life > 0)
				{
					for (int num103 = (int)(dmg / LifeMax * 80.0); num103 > 0; num103--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num104 = 0; num104 < 42; num104++)
				{
					Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, 39);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 40);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 40);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 41);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 41);
			}
			else if (Type == (int)ID.SKELETON || Type == (int)ID.BONES || Type == (int)ID.DARK_CASTER || Type == (int)ID.UNDEAD_MINER || Type == (int)ID.TIM || Type == (int)ID.ARMORED_SKELETON || Type == (int)ID.SKELETON_ARCHER || Type == (int)ID.VAMPIRE_MINER)
			{
				if (Life > 0)
				{
					for (int num105 = 0; num105 < dmg / LifeMax * 50.0; num105++)
					{
						Main.DustSet.NewDust(26, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num106 = 0; num106 < 16; num106++)
				{
					Main.DustSet.NewDust(26, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				int num107 = ((Type == (int)ID.VAMPIRE_MINER) ? 166 : 42);
				Gore.NewGore(Position, Velocity, num107, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, ++num107, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, num107, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, ++num107, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, num107, Scale);
				if (Type == (int)ID.ARMORED_SKELETON)
				{
					Gore.NewGore(Position, Velocity, 106, Scale);
				}
				else if (Type == (int)ID.SKELETON_ARCHER)
				{
					Gore.NewGore(Position, Velocity, 130, Scale);
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 131, Scale);
				}
			}
			else if (Type == (int)ID.MIMIC)
			{
				int num108 = 7;
				if (AI3 == 2f)
				{
					num108 = 10;
				}
				else if (AI3 == 3f)
				{
					num108 = 37;
				}
				if (Life > 0)
				{
					for (int num109 = 0; num109 < dmg / LifeMax * 50.0; num109++)
					{
						if (null == Main.DustSet.NewDust(num108, ref XYWH))
						{
							break;
						}
					}
					return;
				}
				for (int num110 = 0; num110 < 16; num110++)
				{
					if (null == Main.DustSet.NewDust(num108, ref XYWH))
					{
						break;
					}
				}
				int num111 = Gore.NewGore(new Vector2(Position.X, Position.Y - 10f), new Vector2(hitDirection, 0f), 61, Scale);
				Main.GoreSet[num111].Velocity *= 0.3f;
				num111 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1) - 10f), new Vector2(hitDirection, 0f), 62, Scale);
				Main.GoreSet[num111].Velocity *= 0.3f;
				num111 = Gore.NewGore(new Vector2(Position.X, Position.Y + Height - 10f), new Vector2(hitDirection, 0f), 63, Scale);
				Main.GoreSet[num111].Velocity *= 0.3f;
			}
			else if ((Type >= (int)ID.WYVERN_HEAD && Type <= (int)ID.WYVERN_TAIL) || (Type >= (int)ID.ARCH_WYVERN_HEAD && Type <= (int)ID.ARCH_WYVERN_TAIL))
			{
				if (Life > 0)
				{
					for (int num112 = 0; num112 < dmg / LifeMax * 50.0; num112++)
					{
						Dust* ptr13 = Main.DustSet.NewDust(16, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
						if (ptr13 == null)
						{
							break;
						}
						ptr13->Velocity.X *= 1.5f;
						ptr13->Velocity.Y *= 1.5f;
						ptr13->NoGravity = true;
					}
					return;
				}
				for (int num113 = 0; num113 < 8; num113++)
				{
					Dust* ptr14 = Main.DustSet.NewDust(16, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
					if (ptr14 == null)
					{
						break;
					}
					ptr14->Velocity.X *= 2f;
					ptr14->Velocity.Y *= 2f;
					ptr14->NoGravity = true;
				}
				for (int num114 = Main.Rand.Next(1, 4); num114 > 0; num114--)
				{
					int num115 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1) - 10f), new Vector2(hitDirection, 0f), Main.Rand.Next(11, 14), Scale);
					Main.GoreSet[num115].Velocity *= 0.8f;
				}
			}
			else if (Type == (int)ID.MUMMY || Type == (int)ID.DARK_MUMMY || Type == (int)ID.LIGHT_MUMMY || Type == (int)ID.SHADOW_MUMMY || Type == (int)ID.SPECTRAL_MUMMY)
			{
				if (Life > 0)
				{
					for (int num116 = 0; num116 < dmg / LifeMax * 50.0; num116++)
					{
						Dust* ptr15 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
						if (ptr15 == null)
						{
							break;
						}
						ptr15->Velocity.X *= 2f;
						ptr15->Velocity.Y *= 2f;
						ptr15->NoGravity = true;
					}
					return;
				}
				for (int num117 = 0; num117 < 16; num117++)
				{
					Dust* ptr16 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 0, default(Color), 1.5);
					if (ptr16 == null)
					{
						break;
					}
					ptr16->Velocity.X *= 2f;
					ptr16->Velocity.Y *= 2f;
					ptr16->NoGravity = true;
				}
				int num118 = Gore.NewGore(new Vector2(Position.X, Position.Y - 10f), new Vector2(hitDirection, 0f), 61, Scale);
				Main.GoreSet[num118].Velocity *= 0.3f;
				num118 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1) - 10f), new Vector2(hitDirection, 0f), 62, Scale);
				Main.GoreSet[num118].Velocity *= 0.3f;
				num118 = Gore.NewGore(new Vector2(Position.X, Position.Y + Height - 10f), new Vector2(hitDirection, 0f), 63, Scale);
				Main.GoreSet[num118].Velocity *= 0.3f;
			}
			else if (Type == (int)ID.WRAITH)
			{
				if (Life > 0)
				{
					for (int num119 = 0; num119 < dmg / LifeMax * 50.0; num119++)
					{
						Dust* ptr17 = Main.DustSet.NewDust(54, ref XYWH, 0.0, 0.0, 50, default(Color), 1.5);
						if (ptr17 == null)
						{
							break;
						}
						ptr17->Velocity.X *= 2f;
						ptr17->Velocity.Y *= 2f;
						ptr17->NoGravity = true;
					}
					return;
				}
				for (int num120 = 0; num120 < 16; num120++)
				{
					Dust* ptr18 = Main.DustSet.NewDust(54, ref XYWH, 0.0, 0.0, 50, default(Color), 1.5);
					if (ptr18 == null)
					{
						break;
					}
					ptr18->Velocity.X *= 2f;
					ptr18->Velocity.Y *= 2f;
					ptr18->NoGravity = true;
				}
				int num121 = Gore.NewGore(new Vector2(Position.X, Position.Y - 10f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num121].Velocity *= 0.3f;
				num121 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1) - 15f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num121].Velocity *= 0.3f;
				num121 = Gore.NewGore(new Vector2(Position.X, Position.Y + Height - 20f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num121].Velocity *= 0.3f;
			}

			else if (Type == (int)ID.ZOMBIE_MUSHROOM || Type == (int)ID.ZOMBIE_MUSHROOM_HAT)
			{
				if (Life > 0)
				{
					for (int num347 = 0; num347 < dmg / LifeMax * 50.0; num347++)
					{
						Dust* ptr348 = Main.DustSet.NewDust(165, ref XYWH, 0, 0, 50, default(Color), 1.5);
						if (ptr348 == null)
						{
							break;
						}
						ptr348->Velocity.X *= 2f;
						ptr348->Velocity.Y *= 2f;
						ptr348->NoGravity = true;
					}
					return;
				}
				for (int num349 = 0; num349 < 20; num349++)
				{
					Dust* ptr350 = Main.DustSet.NewDust(165, ref XYWH, 0, 0, 50, default(Color), 1.5);
					if (ptr350 == null)
					{
						break;
					}
					ptr350->Velocity.X *= 2f;
					ptr350->Velocity.Y *= 2f;
					ptr350->NoGravity = true;
				}
				int num351 = Gore.NewGore(new Vector2(Position.X, Position.Y - 10f), new Vector2(hitDirection, 0f), 191, Scale);
				Main.GoreSet[num351].Velocity *= 0.3f;
				num351 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height / 2) - 15f), new Vector2(hitDirection, 0f), 192, Scale);
				Main.GoreSet[num351].Velocity *= 0.3f;
				num351 = Gore.NewGore(new Vector2(Position.X, Position.Y + Height - 20f), new Vector2(hitDirection, 0f), 193, Scale);
				Main.GoreSet[num351].Velocity *= 0.3f;
			}
			else if (Type == (int)ID.POSSESSED_ARMOR)
			{
				if (Life > 0)
				{
					return;
				}
				for (int num122 = 0; num122 < 16; num122++)
				{
					Dust* ptr19 = Main.DustSet.NewDust(54, ref XYWH, 0.0, 0.0, 50, default(Color), 1.5);
					if (ptr19 == null)
					{
						break;
					}
					ptr19->Velocity.X *= 2f;
					ptr19->Velocity.Y *= 2f;
					ptr19->NoGravity = true;
				}
				int num123 = Gore.NewGore(new Vector2(Position.X, Position.Y - 10f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num123].Velocity *= 0.3f;
				num123 = Gore.NewGore(new Vector2(Position.X, Position.Y + (Height >> 1) - 15f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num123].Velocity *= 0.3f;
				num123 = Gore.NewGore(new Vector2(Position.X, Position.Y + Height - 20f), new Vector2(hitDirection, 0f), 99, Scale);
				Main.GoreSet[num123].Velocity *= 0.3f;
			}
			else if (Type >= (int)ID.BONE_SERPENT_HEAD && Type <= (int)ID.BONE_SERPENT_TAIL)
			{
				if (Life > 0)
				{
					for (int num124 = 0; num124 < dmg / LifeMax * 50.0; num124++)
					{
						Main.DustSet.NewDust(26, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num125 = 0; num125 < 16; num125++)
				{
					Main.DustSet.NewDust(26, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				Gore.NewGore(Position, Velocity, Type - (int)ID.BONE_SERPENT_HEAD + 67);
			}
			else if (Type == (int)ID.CURSED_SKULL || Type == (int)ID.DRAGON_SKULL)
			{
				if (Life > 0)
				{
					for (int num126 = 0; num126 < dmg / LifeMax * 30.0; num126++)
					{
						Dust* ptr20 = Main.DustSet.NewDust(15, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100, default(Color), 1.8);
						if (ptr20 == null)
						{
							break;
						}
						ptr20->NoLight = true;
						ptr20->NoGravity = true;
						ptr20->Velocity.X *= 1.3f;
						ptr20->Velocity.Y *= 1.3f;
						ptr20 = Main.DustSet.NewDust(26, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 0, default(Color), 0.9);
						if (ptr20 == null)
						{
							break;
						}
						ptr20->NoLight = true;
						ptr20->Velocity.X *= 1.3f;
						ptr20->Velocity.Y *= 1.3f;
					}
					return;
				}
				for (int num127 = 0; num127 < 12; num127++)
				{
					Dust* ptr21 = Main.DustSet.NewDust(15, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100, default(Color), 1.8);
					if (ptr21 == null)
					{
						break;
					}
					ptr21->NoLight = true;
					ptr21->NoGravity = true;
					ptr21->Velocity.X *= 1.3f;
					ptr21->Velocity.Y *= 1.3f;
					ptr21 = Main.DustSet.NewDust(26, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 0, default(Color), 0.9);
					if (ptr21 == null)
					{
						break;
					}
					ptr21->NoLight = true;
					ptr21->Velocity.X *= 1.3f;
					ptr21->Velocity.Y *= 1.3f;
				}
			}
			else if (Type == (int)ID.SKELETRON_HEAD || Type == (int)ID.SKELETRON_HAND)
			{
				if (Life > 0)
				{
					for (int num128 = (int)(dmg / LifeMax * 80.0); num128 > 0; num128--)
					{
						Main.DustSet.NewDust(26, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num129 = 0; num129 < 128; num129++)
				{
					Main.DustSet.NewDust(26, ref XYWH, 2.5 * hitDirection, -2.5);
				}
				if (Type == (int)ID.SKELETRON_HEAD)
				{
					Gore.NewGore(Position, Velocity, 54);
					Gore.NewGore(Position, Velocity, 55);
					return;
				}
				Gore.NewGore(Position, Velocity, 56);
				Gore.NewGore(Position, Velocity, 57);
				Gore.NewGore(Position, Velocity, 57);
				Gore.NewGore(Position, Velocity, 57);
			}
			else if (Type == (int)ID.PROBE)
			{
				if (Life > 0)
				{
					return;
				}
				for (int num130 = 0; num130 < 8; num130++)
				{
					Dust* ptr22 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr22 == null)
					{
						break;
					}
					ptr22->Velocity.X *= 1.4f;
					ptr22->Velocity.Y *= 1.4f;
				}
				for (int num131 = 0; num131 < 4; num131++)
				{
					Dust* ptr23 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
					if (ptr23 == null)
					{
						break;
					}
					ptr23->NoGravity = true;
					ptr23->Velocity.X *= 5f;
					ptr23->Velocity.Y *= 5f;
					ptr23 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr23 == null)
					{
						break;
					}
					ptr23->Velocity.X *= 3f;
					ptr23->Velocity.Y *= 3f;
				}
				int num132 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num132].Velocity *= 0.4f;
				Main.GoreSet[num132].Velocity.X += 1f;
				Main.GoreSet[num132].Velocity.Y += 1f;
				num132 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num132].Velocity *= 0.4f;
				Main.GoreSet[num132].Velocity.X -= 1f;
				Main.GoreSet[num132].Velocity.Y += 1f;
				num132 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num132].Velocity *= 0.4f;
				Main.GoreSet[num132].Velocity.X += 1f;
				Main.GoreSet[num132].Velocity.Y -= 1f;
				num132 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num132].Velocity *= 0.4f;
				Main.GoreSet[num132].Velocity.X -= 1f;
				Main.GoreSet[num132].Velocity.Y -= 1f;
			}
			else if (Type >= (int)ID.THE_DESTROYER_HEAD && Type <= (int)ID.THE_DESTROYER_TAIL)
			{
				if (Type == (int)ID.THE_DESTROYER_BODY && Life > 0 && Main.NetMode != (byte)NetModeSetting.CLIENT && AI2 == 0f && Main.Rand.Next(25) == 0)
				{
					AI2 = 1f;
					int num133 = NewNPC(XYWH.X + (Width >> 1), XYWH.Y + Height, (int)ID.PROBE);
					if (Main.NetMode == (byte)NetModeSetting.SERVER && num133 < MaxNumNPCs)
					{
						NetMessage.CreateMessage1(23, num133);
						NetMessage.SendMessage();
					}
					ShouldNetUpdate = true;
				}
				if (Life > 0)
				{
					return;
				}
				Gore.NewGore(Position, Velocity, 156);
				if (Main.Rand.Next(2) != 0)
				{
					return;
				}
				for (int num134 = 0; num134 < 8; num134++)
				{
					Dust* ptr24 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr24 == null)
					{
						break;
					}
					ptr24->Velocity.X *= 1.4f;
					ptr24->Velocity.Y *= 1.4f;
				}
				for (int num135 = 0; num135 < 4; num135++)
				{
					Dust* ptr25 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
					if (ptr25 == null)
					{
						break;
					}
					ptr25->NoGravity = true;
					ptr25->Velocity.X *= 5f;
					ptr25->Velocity.Y *= 5f;
					ptr25 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr25 == null)
					{
						break;
					}
					ptr25->Velocity.X *= 3f;
					ptr25->Velocity.Y *= 3f;
				}
				int num136 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num136].Velocity.X *= 0.4f;
				Main.GoreSet[num136].Velocity.X += 1f;
				Main.GoreSet[num136].Velocity.Y *= 0.4f;
				Main.GoreSet[num136].Velocity.Y += 1f;
				num136 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num136].Velocity.X *= 0.4f;
				Main.GoreSet[num136].Velocity.X -= 1f;
				Main.GoreSet[num136].Velocity.Y *= 0.4f;
				Main.GoreSet[num136].Velocity.Y += 1f;
				num136 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num136].Velocity.X *= 0.4f;
				Main.GoreSet[num136].Velocity.X += 1f;
				Main.GoreSet[num136].Velocity.Y *= 0.4f;
				Main.GoreSet[num136].Velocity.Y -= 1f;
				num136 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num136].Velocity.X *= 0.4f;
				Main.GoreSet[num136].Velocity.X -= 1f;
				Main.GoreSet[num136].Velocity.Y *= 0.4f;
				Main.GoreSet[num136].Velocity.Y -= 1f;
			}
			else if (Type == (int)ID.SKELETRON_PRIME)
			{
				if (Life > 0)
				{
					return;
				}
				Gore.NewGore(Position, Velocity, 149);
				Gore.NewGore(Position, Velocity, 150);
				for (int num137 = 0; num137 < 8; num137++)
				{
					Dust* ptr26 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr26 == null)
					{
						break;
					}
					ptr26->Velocity.X *= 1.4f;
					ptr26->Velocity.Y *= 1.4f;
				}
				for (int num138 = 0; num138 < 4; num138++)
				{
					Dust* ptr27 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
					if (ptr27 == null)
					{
						break;
					}
					ptr27->NoGravity = true;
					ptr27->Velocity.X *= 5f;
					ptr27->Velocity.Y *= 5f;
					ptr27 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr27 == null)
					{
						break;
					}
					ptr27->Velocity.X *= 3f;
					ptr27->Velocity.Y *= 3f;
				}
				int num139 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num139].Velocity.X *= 0.4f;
				Main.GoreSet[num139].Velocity.X += 1f;
				Main.GoreSet[num139].Velocity.Y *= 0.4f;
				Main.GoreSet[num139].Velocity.Y += 1f;
				num139 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num139].Velocity.X *= 0.4f;
				Main.GoreSet[num139].Velocity.X -= 1f;
				Main.GoreSet[num139].Velocity.Y *= 0.4f;
				Main.GoreSet[num139].Velocity.Y += 1f;
				num139 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num139].Velocity.X *= 0.4f;
				Main.GoreSet[num139].Velocity.X += 1f;
				Main.GoreSet[num139].Velocity.Y *= 0.4f;
				Main.GoreSet[num139].Velocity.Y -= 1f;
				num139 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num139].Velocity.X *= 0.4f;
				Main.GoreSet[num139].Velocity.X -= 1f;
				Main.GoreSet[num139].Velocity.Y *= 0.4f;
				Main.GoreSet[num139].Velocity.Y -= 1f;
			}
			else if (Type >= (int)ID.PRIME_CANNON && Type <= (int)ID.PRIME_LASER)
			{
				if (Life > 0)
				{
					return;
				}
				Gore.NewGore(Position, Velocity, 147);
				Gore.NewGore(Position, Velocity, 148);
				for (int num140 = 0; num140 < 8; num140++)
				{
					Dust* ptr28 = Main.DustSet.NewDust(31, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr28 == null)
					{
						break;
					}
					ptr28->Velocity.X *= 1.4f;
					ptr28->Velocity.Y *= 1.4f;
				}
				for (int num141 = 0; num141 < 4; num141++)
				{
					Dust* ptr29 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 2.5);
					if (ptr29 == null)
					{
						break;
					}
					ptr29->NoGravity = true;
					ptr29->Velocity.X *= 5f;
					ptr29->Velocity.Y *= 5f;
					ptr29 = Main.DustSet.NewDust(6, ref XYWH, 0.0, 0.0, 100, default(Color), 1.5);
					if (ptr29 == null)
					{
						break;
					}
					ptr29->Velocity.X *= 3f;
					ptr29->Velocity.Y *= 3f;
				}
				int num142 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num142].Velocity.X *= 0.4f;
				Main.GoreSet[num142].Velocity.X += 1f;
				Main.GoreSet[num142].Velocity.Y *= 0.4f;
				Main.GoreSet[num142].Velocity.Y += 1f;
				num142 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num142].Velocity.X *= 0.4f;
				Main.GoreSet[num142].Velocity.X -= 1f;
				Main.GoreSet[num142].Velocity.Y *= 0.4f;
				Main.GoreSet[num142].Velocity.Y += 1f;
				num142 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num142].Velocity.X *= 0.4f;
				Main.GoreSet[num142].Velocity.X += 1f;
				Main.GoreSet[num142].Velocity.Y *= 0.4f;
				Main.GoreSet[num142].Velocity.Y -= 1f;
				num142 = Gore.NewGore(Position, default(Vector2), Main.Rand.Next(61, 64));
				Main.GoreSet[num142].Velocity.X *= 0.4f;
				Main.GoreSet[num142].Velocity.X -= 1f;
				Main.GoreSet[num142].Velocity.Y *= 0.4f;
				Main.GoreSet[num142].Velocity.Y -= 1f;
			}
			else if (Type == (int)ID.METEOR_HEAD)
			{
				if (Life > 0)
				{
					for (int num143 = (int)(dmg / LifeMax * 80.0); num143 > 0; num143--)
					{
						int num144 = ((Main.Rand.Next(2) == 0) ? 6 : 25);
						Main.DustSet.NewDust(num144, ref XYWH, hitDirection, -1.0);
						Dust* ptr30 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * 0.2, Velocity.Y * 0.2, 100, default(Color), 2.0);
						if (ptr30 == null)
						{
							break;
						}
						ptr30->NoGravity = true;
					}
					return;
				}
				for (int num145 = 0; num145 < 42; num145++)
				{
					int num146 = ((Main.Rand.Next(2) == 0) ? 6 : 25);
					if (null == Main.DustSet.NewDust(num146, ref XYWH, hitDirection << 1, -2.0))
					{
						break;
					}
				}
				for (int num147 = 0; num147 < 42; num147++)
				{
					Dust* ptr31 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * 0.2, Velocity.Y * 0.2, 100, default(Color), 2.5);
					if (ptr31 == null)
					{
						break;
					}
					ptr31->Velocity.X *= 6f;
					ptr31->Velocity.Y *= 6f;
					ptr31->NoGravity = true;
				}
			}
			else if (Type == (int)ID.FIRE_IMP)
			{
				if (Life > 0)
				{
					for (int num148 = (int)(dmg / LifeMax * 80.0); num148 > 0; num148--)
					{
						Dust* ptr32 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X, Velocity.Y, 100, default(Color), 2.5);
						if (ptr32 == null)
						{
							break;
						}
						ptr32->NoGravity = true;
					}
					return;
				}
				for (int num149 = 0; num149 < 42; num149++)
				{
					Dust* ptr33 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X, Velocity.Y, 100, default(Color), 2.5);
					if (ptr33 == null)
					{
						break;
					}
					ptr33->NoGravity = true;
					ptr33->Velocity.X *= 2f;
					ptr33->Velocity.Y *= 2f;
				}
				Gore.NewGore(Position, Velocity, 45);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 46);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 46);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 47);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 47);
			}
			else if (Type == (int)ID.BURNING_SPHERE)
			{
				Main.PlaySound(2, XYWH.X, XYWH.Y, 10);
				for (int num150 = 0; num150 < 16; num150++)
				{
					Dust* ptr34 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100, default(Color), 2.0);
					if (ptr34 == null)
					{
						break;
					}
					ptr34->NoGravity = true;
					ptr34->Velocity *= 2f;
					ptr34 = Main.DustSet.NewDust(6, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100);
					if (ptr34 == null)
					{
						break;
					}
					ptr34->Velocity.X *= 2f;
					ptr34->Velocity.Y *= 2f;
				}
			}
			else if (Type == (int)ID.WATER_SPHERE)
			{
				Main.PlaySound(2, XYWH.X, XYWH.Y, 10);
				for (int num151 = 0; num151 < 16; num151++)
				{
					Dust* ptr35 = Main.DustSet.NewDust(29, ref XYWH, Velocity.X * -0.2f, Velocity.Y * -0.2, 100, default(Color), 2.0);
					if (ptr35 == null)
					{
						break;
					}
					ptr35->NoGravity = true;
					ptr35->Velocity.X *= 2f;
					ptr35->Velocity.Y *= 2f;
					ptr35 = Main.DustSet.NewDust(29, ref XYWH, Velocity.X * -0.2f, Velocity.Y * -0.2, 100);
					if (ptr35 == null)
					{
						break;
					}
					ptr35->Velocity.X *= 2f;
					ptr35->Velocity.Y *= 2f;
				}
			}
			else if ((Type >= (int)ID.GOBLIN_PEON && Type <= (int)ID.GOBLIN_SORCERER) || Type == (int)ID.GOBLIN_SCOUT || Type == (int)ID.GOBLIN_ARCHER)
			{
				if (Life > 0)
				{
					int num152 = (int)(dmg / LifeMax * 80.0);
					while (num152 > 0 && null != Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0))
					{
						num152--;
					}
					return;
				}
				for (int num153 = 0; num153 < 42; num153++)
				{
					if (null == Main.DustSet.NewDust(5, ref XYWH, 2.5 * hitDirection, -2.5))
					{
						break;
					}
				}
				Gore.NewGore(Position, Velocity, 48, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 49, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 20f), Velocity, 49, Scale);
				if (Type == (int)ID.GOBLIN_ARCHER)
				{
					Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 131, Scale);
				}
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 50, Scale);
				Gore.NewGore(new Vector2(Position.X, Position.Y + 34f), Velocity, 50, Scale);
			}
			else if (Type == (int)ID.CHAOS_BALL)
			{
				Main.PlaySound(2, (int)Position.X, (int)Position.Y, 10);
				for (int num154 = 0; num154 < 15; num154++)
				{
					Dust* ptr36 = Main.DustSet.NewDust(27, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100, default(Color), 2.0);
					if (ptr36 == null)
					{
						break;
					}
					ptr36->NoGravity = true;
					ptr36->Velocity.X *= 2f;
					ptr36->Velocity.Y *= 2f;
					ptr36 = Main.DustSet.NewDust(27, ref XYWH, Velocity.X * -0.2, Velocity.Y * -0.2, 100);
					if (ptr36 == null)
					{
						break;
					}
					ptr36->Velocity.X *= 2f;
					ptr36->Velocity.Y *= 2f;
				}
			}
			else if (Type == (int)ID.HORNET || Type == (int)ID.DRAGON_HORNET)
			{
				if (Life > 0)
				{
					for (int num155 = (int)(dmg / LifeMax * 80.0); num155 > 0; num155--)
					{
						Main.DustSet.NewDust(18, ref XYWH, hitDirection, -1.0, Alpha, Colour, Scale);
					}
					return;
				}
				for (int num156 = 0; num156 < 42; num156++)
				{
					Main.DustSet.NewDust(18, ref XYWH, hitDirection, -2.0, Alpha, Colour, Scale);
				}
				int num157 = ((Type == (int)ID.HORNET) ? 70 : 169);
				Gore.NewGore(Position, Velocity, num157, Scale);
				Gore.NewGore(Position, Velocity, ++num157, Scale);
			}
			else if (Type == (int)ID.MAN_EATER || Type == (int)ID.SNATCHER || Type == (int)ID.DRAGON_SNATCHER)
			{
				if (Life > 0)
				{
					for (int num158 = (int)(dmg / LifeMax * 80.0); num158 > 0; num158--)
					{
						Main.DustSet.NewDust(40, ref XYWH, hitDirection, -1.0, Alpha, Colour, 1.2);
					}
					return;
				}
				for (int num159 = 0; num159 < 42; num159++)
				{
					Main.DustSet.NewDust(40, ref XYWH, hitDirection, -2.0, Alpha, Colour, 1.2);
				}
				Gore.NewGore(Position, Velocity, 72);
				Gore.NewGore(Position, Velocity, 72);
			}
			else if (Type == (int)ID.HARPY)
			{
				if (Life > 0)
				{
					for (int num160 = (int)(dmg / LifeMax * 80.0); num160 > 0; num160--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num161 = 0; num161 < 42; num161++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0);
				}
				Gore.NewGore(Position, Velocity, 80);
				Gore.NewGore(Position, Velocity, 81);
			}
			else
			{
				if (Type != (int)ID.DEMON && Type != (int)ID.ARCH_DEMON && Type != (int)ID.VOODOO_DEMON)
				{
					return;
				}
				if (Life > 0)
				{
					for (int num162 = (int)(dmg / LifeMax * 80.0); num162 > 0; num162--)
					{
						Main.DustSet.NewDust(5, ref XYWH, hitDirection, -1.0);
					}
					return;
				}
				for (int num163 = 0; num163 < 42; num163++)
				{
					Main.DustSet.NewDust(5, ref XYWH, hitDirection << 1, -2.0);
				}
				if (Type == (int)ID.ARCH_DEMON)
				{
					Gore.NewGore(Position, Velocity, 171);
					Gore.NewGore(Position, Velocity, 0);
					Gore.NewGore(Position, Velocity, 0);
				}
				else
				{
					Gore.NewGore(Position, Velocity, 93);
					Gore.NewGore(Position, Velocity, 94);
					Gore.NewGore(Position, Velocity, 94);
				}
			}
		}

		public static bool AnyNPCs(int Type)
		{
			for (int num = MaxNumNPCs - 1; num >= 0; num--)
			{
				if (Main.NPCSet[num].Type == Type && Main.NPCSet[num].Active != 0)
				{
					return true;
				}
			}
			return false;
		}

		public static bool AnyNPCs(int Type1, int Type2)
		{
			for (int num = MaxNumNPCs - 1 ; num >= 0; num--)
			{
				if (Main.NPCSet[num].Active != 0 && (Main.NPCSet[num].Type == Type1 || Main.NPCSet[num].Type == Type2))
				{
					return true;
				}
			}
			return false;
		}

		public static void SpawnSkeletron()
		{
			int num = -1;
			for (int i = 0; i < MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active != 0)
				{
					if (Main.NPCSet[i].Type == (int)ID.SKELETRON_HEAD)
					{
						return;
					}
					if (Main.NPCSet[i].Type == (int)ID.OLD_MAN)
					{
						num = i;
						break;
					}
				}
			}
			if (num >= 0)
			{
				Main.NPCSet[num].AI3 = 1f;
				int num2 = NewNPC(Main.NPCSet[num].XYWH.X + (Main.NPCSet[num].Width >> 1), Main.NPCSet[num].XYWH.Y + (Main.NPCSet[num].Height >> 1), (int)ID.SKELETRON_HEAD);
				Main.NPCSet[num2].ShouldNetUpdate = true;
				NetMessage.CreateMessage1((int)SendDataId.SERVER_WORLD_NPC, num);
				NetMessage.SendMessage();
				NetMessage.SendText("Skeletron", 16, 175, 75, 255, -1);
			}
		}

		public static bool NearSpikeBall(int x, int y)
		{
			Rectangle rectangle = new Rectangle(x * 16 - 300, y * 16 - 300, 600, 600);
			for (int i = 0; i < MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].AIStyle == 20 && Main.NPCSet[i].Active != 0)
				{
					Rectangle rectangle2 = new Rectangle((int)Main.NPCSet[i].AI1, (int)Main.NPCSet[i].AI2, 20, 20);
					if (rectangle.Intersects(rectangle2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddBuff(int BuffType, int time, bool quiet = false)
		{
			if (BuffImmune[BuffType])
			{
				return;
			}
			if (!quiet)
			{
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					NetMessage.CreateMessage3(53, WhoAmI, BuffType, time);
				}
				else
				{
					NetMessage.CreateMessage1(54, WhoAmI);
				}
				NetMessage.SendMessage();
			}
			for (int i = 0; i < MaxNumNPCBuffs; i++)
			{
				if (ActiveBuffs[i].Type == BuffType)
				{
					if (ActiveBuffs[i].Time < time)
					{
						ActiveBuffs[i].Time = (ushort)time;
					}
					return;
				}
			}
			int num = -1;
			do
			{
				int num2 = -1;
				for (int j = 0; j < MaxNumNPCBuffs; j++)
				{
					if (!ActiveBuffs[j].IsDebuff())
					{
						num2 = j;
						break;
					}
				}
				if (num2 == -1)
				{
					return;
				}
				for (int k = num2; k < MaxNumNPCBuffs; k++)
				{
					if (ActiveBuffs[k].Type == 0)
					{
						num = k;
						break;
					}
				}
				if (num == -1)
				{
					DelBuff(num2);
				}
			}
			while (num == -1);
			ActiveBuffs[num].Type = (ushort)BuffType;
			ActiveBuffs[num].Time = (ushort)time;
		}

		public void DelBuff(int b)
		{
			ActiveBuffs[b].Time = 0;
			ActiveBuffs[b].Type = 0;
			for (int i = 0; i < MaxNumNPCBuffs - 1; i++)
			{
				if (ActiveBuffs[i].Time == 0 || ActiveBuffs[i].Type == 0)
				{
					for (int j = i + 1; j < MaxNumNPCBuffs; j++)
					{
						ref Buff reference = ref ActiveBuffs[j - 1];
						reference = ActiveBuffs[j];
						ActiveBuffs[j].Time = 0;
						ActiveBuffs[j].Type = 0;
					}
				}
			}
			if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				NetMessage.CreateMessage1(54, WhoAmI);
				NetMessage.SendMessage();
			}
		}

		private unsafe void FireEffect(int particleType)
		{
			if (Main.Rand.Next(4) < 2)
			{
				Dust* ptr = Main.DustSet.NewDust((int)Position.X - 2, (int)Position.Y - 2, Width + 4, Height + 4, particleType, Velocity.X * 0.4f, Velocity.Y * 0.4f, 100, default(Color), 3.5);
				if (ptr != null)
				{
					ptr->NoGravity = true;
					ptr->Velocity.X *= 1.8f;
					ptr->Velocity.Y *= 1.8f;
					ptr->Velocity.Y -= 0.5f;
					if (Main.Rand.Next(4) == 0)
					{
						ptr->NoGravity = false;
						ptr->Scale *= 0.5f;
					}
				}
			}
			Lighting.AddLight((int)Position.X >> 4, ((int)Position.Y >> 4) + 1, new Vector3(1f, 0.3f, 0.1f));
		}

		public unsafe void UpdateNPC(int i)
		{
			WhoAmI = (short)i;
			if (XYWH.X <= 0 || XYWH.X + Width >= Main.RightWorld || XYWH.Y <= 0 || XYWH.Y + Height >= Main.BottomWorld)
			{
				Active = 0;
				return;
			}
			int num = 0;
			bool flag = false;
			IsPoisoned = false;
			IsConfused = false;
			for (int j = 0; j < MaxNumNPCBuffs; j++)
			{
				if (ActiveBuffs[j].Type > 0 && ActiveBuffs[j].Time > 0)
				{
#if !VERSION_INITIAL
					// BUG: Quite powerful this one. The bug here is that there is no way for debuffs on NPCs to decrement after application, essentially resulting in infinite fire-type, poison, confusion, or bleed debuffs.
					// 1.01 fixed this by just adding the decrement operator for the time variable, just like what PC had.
					ActiveBuffs[j].Time--;
#endif
					switch (ActiveBuffs[j].Type)
					{
						case (int)Buff.ID.CONFUSED:
							IsConfused = true;
							break;
						case (int)Buff.ID.POISONED:
							IsPoisoned = true;
							if (num > -4)
							{
								num = -4;
							}
							if (Main.Rand.Next(30) == 0)
							{
								Dust* ptr2 = Main.DustSet.NewDust(46, ref XYWH, 0.0, 0.0, 120, default(Color), 0.2);
								if (ptr2 != null)
								{
									ptr2->NoGravity = true;
									ptr2->FadeIn = 1.9f;
								}
							}
							break;
						case (int)Buff.ID.ON_FIRE:
							flag = true;
							if (num > -8)
							{
								num = -8;
							}
							FireEffect(6);
							break;
						case (int)Buff.ID.ON_FIRE_2:
							if (num > -12)
							{
								num = -12;
							}
							FireEffect(75);
							break;
						case (int)Buff.ID.BLEED:
							if (num > -16)
							{
								num = -16;
							}
							if (Main.Rand.Next(30) == 0)
							{
								Dust* ptr = Main.DustSet.NewDust(5, ref XYWH);
								if (ptr != null)
								{
									ptr->Velocity.Y += 0.5f;
									ptr->Velocity *= 0.25f;
								}
							}
							break;
					}
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				for (int k = 0; k < MaxNumNPCBuffs; k++)
				{
#if !VERSION_INITIAL
					if (ActiveBuffs[k].Type > 0 && ActiveBuffs[k].Time <= 0)
#else
					if (ActiveBuffs[k].Type > 0 && ActiveBuffs[k].Time == 0)
#endif
					{
						DelBuff(k);
					}
				}
			}
			if (!DontTakeDamage)
			{
				// Seems they sacked off the check to see if life regen exceeds 120, which isn't really possible to my knowledge, but that would make infinite regen possible here.
				LifeRegenCount += num;
				while (LifeRegenCount <= -120)
				{
					LifeRegenCount += 120;
					int num2 = WhoAmI;
					if (RealLife >= 0)
					{
						num2 = RealLife;
					}
					if (--Main.NPCSet[num2].Life <= 0)
					{
						Main.NPCSet[num2].Life = 1;
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							Main.NPCSet[num2].StrikeNPC(9999, 0f, 0);
							NetMessage.SendNpcHurt(num2, 9999, 0.0, 0);
						}
					}
				}
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && Main.GameTime.IsBloodMoon)
			{
				if (Type == (int)ID.BUNNY)
				{
					Transform((int)ID.CORRUPT_BUNNY);
				}
				else if (Type == (int)ID.GOLDFISH)
				{
					Transform((int)ID.CORRUPT_GOLDFISH);
				}
			}
			float num3 = 10f;
			float CurGrav = 0.3f;
			float num5 = Main.MaxTilesX / 4200f;
			num5 *= num5;
			float num6 = (Position.Y * 0.0625f - (60f + 10f * num5)) / (Main.WorldSurface / 6);
			if ((double)num6 < 0.25)
			{
				num6 = 0.25f;
			}
			else if (num6 > 1f)
			{
				num6 = 1f;
			}
			CurGrav *= num6;
			if (IsWet)
			{
				CurGrav = 0.2f;
				num3 = 7f;
			}
			if (SoundDelay > 0)
			{
				SoundDelay--;
			}
			if (Life <= 0)
			{
				Active = 0;
				return;
			}
			OldTarget = Target;
			OldDirection = Direction;
			OldDirectionY = DirectionY;
			try
			{
				switch (AIStyle)
				{
				case 0:
					BoundAI();
					break;
				case 1:
					SlimeAI();
					break;
				case 2:
					FloatingEyeballAI();
					break;
				case 3:
					WalkAI();
					break;
				case 4:
					EyeOfCthulhuAI();
					break;
				case 5:
					AggressiveFlyerAI();
					break;
				case 6:
					WormAI();
					break;
				case 7:
					TownsfolkAI();
					break;
				case 8:
					SorcererAI();
					break;
				case 9:
					SphereAI();
					break;
				case 10:
					SkullHeadAI();
					break;
				case 11:
					SkeletronAI();
					break;
				case 12:
					SkeletronHandAI();
					break;
				case 13:
					PlantAI();
					break;
				case 14:
					FlyerAI();
					break;
				case 15:
					KingSlimeAI();
					break;
				case 16:
					FishAI();
					break;
				case 17:
					VultureAI();
					break;
				case 18:
					JellyfishAI();
					break;
				case 19:
					AntlionAI();
					break;
				case 20:
					SpinningSpikeballAI();
					break;
				case 21:
					GravityDiskAI();
					break;
				case 22:
					MoreFlyerAI();
					break;
				case 23:
					EnchantedWeaponAI();
					break;
				case 24:
					BirdAI();
					break;
				case 25:
					MimicAI();
					break;
				case 26:
					UnicornAI();
					break;
				case 27:
					WallOfFleshMouthAI();
					break;
				case 28:
					WallOfFleshEyesAI();
					break;
				case 29:
					WallOfFleshTentacleAI();
					break;
				case 30:
					RetinazerAI();
					break;
				case 31:
					SpazmatismAI();
					break;
				case 32:
					SkeletronPrimeAI();
					break;
				case 33:
					SkeletronPrimeSawHand();
					break;
				case 34:
					SkeletronPrimeViceHand();
					break;
				case 35:
					SkeletronPrimeCannonHand();
					break;
				case 36:
					SkeletronPrimeLaserHand();
					break;
				case 37:
					DestroyerAI();
					break;
				case 38:
					SnowmanAI();
					break;
				case 39:
					OcramAI();
					break;
				}
			}
			catch (Exception)
			{
				Active = 0;
				return;
			}
			if (Type == (int)ID.UNDEAD_MINER || Type == (int)ID.VAMPIRE_MINER)
			{
				Lighting.AddLight(XYWH.X + (Width >> 1) >> 4, XYWH.Y + 4 >> 4, new Vector3(0.9f, 0.75f, 0.5f));
			}
			for (int l = 0; l < Player.MaxNumPlayers + 1; l++)
			{
				if (Immunities[l] > 0)
				{
					Immunities[l]--;
				}
			}
			if (!HasNoGravity)
			{
				Velocity.Y += CurGrav;
				if (Velocity.Y > num3)
				{
					Velocity.Y = num3;
				}
			}
			if (Velocity.X < 0.005f && Velocity.X > -0.005f)
			{
				Velocity.X = 0f;
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && Type != (int)ID.OLD_MAN && (IsFriendly || Type == (int)ID.BUNNY || Type == (int)ID.GOLDFISH || Type == (int)ID.BIRD))
			{
				if (Life < LifeMax)
				{
					FriendlyRegen++;
					if (FriendlyRegen > 300)
					{
						FriendlyRegen = 0;
						Life++;
						ShouldNetUpdate = true;
					}
				}
				if (Immunities[Player.MaxNumPlayers] == 0)
				{
					for (int m = 0; m < MaxNumNPCs; m++)
					{
						if (Main.NPCSet[m].Active != 0 && !Main.NPCSet[m].IsFriendly && Main.NPCSet[m].Damage > 0 && XYWH.Intersects(Main.NPCSet[m].XYWH))
						{
							int dmg = Main.NPCSet[m].Damage;
							int num7 = 6;
							int num8 = 1;
							if (Main.NPCSet[m].XYWH.X + (Main.NPCSet[m].Width >> 1) > XYWH.X + (Width >> 1))
							{
								num8 = -1;
							}
							Main.NPCSet[i].StrikeNPC(dmg, num7, num8);
							NetMessage.SendNpcHurt(i, dmg, num7, num8);
							ShouldNetUpdate = true;
							Immunities[Player.MaxNumPlayers] = 30;
						}
					}
				}
			}
			if (!HasNoTileCollide)
			{
				bool flag2 = Collision.LavaCollision(ref Position, Width, Height);
				if (flag2)
				{
					LavaWet = true;
					if (!IsLavaImmune && !DontTakeDamage && Main.NetMode != (byte)NetModeSetting.CLIENT && Immunities[Player.MaxNumPlayers] == 0)
					{
						AddBuff((int)Buff.ID.ON_FIRE, 420);
						Immunities[Player.MaxNumPlayers] = 30;
						StrikeNPC(50, 0f, 0);
						NetMessage.SendNpcHurt(WhoAmI, 50, 0.0, 0);
#if !USE_ORIGINAL_CODE
						if (Type == (int)ID.GUIDE && Life <= 0)
						{
							UI.SetTriggerStateForAll(Trigger.OldFashioned);
						}
#endif
					}
				}
				bool flag3 = false;
				if (Type == (int)ID.BLAZING_WHEEL)
				{
					flag3 = false;
					WetCount = 0;
					flag2 = false;
				}
				else
				{
					flag3 = Collision.WetCollision(ref Position, Width, Height);
				}
				if (flag3)
				{
					if (flag && !LavaWet && Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						for (int n = 0; n < MaxNumNPCBuffs; n++)
						{
							if (ActiveBuffs[n].Type == (int)Buff.ID.ON_FIRE)
							{
								DelBuff(n);
								break;
							}
						}
					}
					if (!IsWet && WetCount == 0)
					{
						WetCount = 10;
						if (!flag2)
						{
							for (int num9 = 0; num9 < 24; num9++)
							{
								Dust* ptr3 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (Height >> 1) - 8, Width + 12, 24, 33);
								if (ptr3 == null)
								{
									break;
								}
								ptr3->Velocity.Y -= 4f;
								ptr3->Velocity.X *= 2.5f;
								ptr3->Scale = 1.3f;
								ptr3->Alpha = 100;
								ptr3->NoGravity = true;
							}
							if (Type != (int)ID.SLIME && Type != (int)ID.LAVA_SLIME && !HasNoGravity)
							{
								Main.PlaySound(19, XYWH.X, XYWH.Y, 0);
							}
						}
						else
						{
							for (int num10 = 0; num10 < 7; num10++)
							{
								Dust* ptr4 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (Height >> 1) - 8, Width + 12, 24, 35);
								if (ptr4 == null)
								{
									break;
								}
								ptr4->Velocity.Y -= 1.5f;
								ptr4->Velocity.X *= 2.5f;
								ptr4->Scale = 1.3f;
								ptr4->Alpha = 100;
								ptr4->NoGravity = true;
							}
							if (Type != (int)ID.SLIME && Type != (int)ID.LAVA_SLIME && !HasNoGravity)
							{
								Main.PlaySound(19, XYWH.X, XYWH.Y);
							}
						}
					}
					IsWet = true;
				}
				else if (IsWet)
				{
					Velocity.X *= 0.5f;
					IsWet = false;
					if (WetCount == 0)
					{
						WetCount = 10;
						if (!LavaWet)
						{
							for (int num11 = 0; num11 < 24; num11++)
							{
								Dust* ptr5 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (Height >> 1) - 8, Width + 12, 24, 33);
								if (ptr5 == null)
								{
									break;
								}
								ptr5->Velocity.Y -= 4f;
								ptr5->Velocity.X *= 2.5f;
								ptr5->Scale = 1.3f;
								ptr5->Alpha = 100;
								ptr5->NoGravity = true;
							}
							if (Type != (int)ID.SLIME && Type != (int)ID.LAVA_SLIME && !HasNoGravity)
							{
								Main.PlaySound(19, XYWH.X, XYWH.Y, 0);
							}
						}
						else
						{
							for (int num12 = 0; num12 < 7; num12++)
							{
								Dust* ptr6 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (Height >> 1) - 8, Width + 12, 24, 35);
								if (ptr6 == null)
								{
									break;
								}
								ptr6->Velocity.Y -= 1.5f;
								ptr6->Velocity.X *= 2.5f;
								ptr6->Scale = 1.3f;
								ptr6->Alpha = 100;
								ptr6->NoGravity = true;
							}
							if (Type != (int)ID.SLIME && Type != (int)ID.LAVA_SLIME && !HasNoGravity)
							{
								Main.PlaySound(19, XYWH.X, XYWH.Y);
							}
						}
					}
				}
				if (!IsWet)
				{
					LavaWet = false;
				}
				if (WetCount > 0)
				{
					WetCount--;
				}
				bool flag4 = false;
				if (AIStyle == 10)
				{
					flag4 = true;
				}
				else if (AIStyle == 14)
				{
					flag4 = true;
				}
				else if (AIStyle == 3 && DirectionY == 1)
				{
					flag4 = true;
				}
				OldVelocity = Velocity;
				HasXCollision = false;
				HasYCollision = false;
				if (IsWet)
				{
					Vector2 vector = Velocity;
					Collision.TileCollision(ref Position, ref Velocity, Width, Height, flag4, flag4);
					if (Collision.UpCol)
					{
						Velocity.Y = 0.01f;
					}
					Vector2 vector2 = Velocity;
					vector2.X *= 0.5f;
					vector2.Y *= 0.5f;
					if (Velocity.X != vector.X)
					{
						vector2.X = Velocity.X;
						HasXCollision = true;
					}
					if (Velocity.Y != vector.Y)
					{
						vector2.Y = Velocity.Y;
						HasYCollision = true;
					}
					OldPosition = Position;
					Position.X += vector2.X;
					Position.Y += vector2.Y;
				}
				else
				{
					if (Type == (int)ID.BLAZING_WHEEL)
					{
						Vector2 NewPosition = new Vector2(Position.X + (Width >> 1), Position.Y + (Height >> 1));
						int num13 = 12;
						int num14 = 12;
						NewPosition.X -= num13 >> 1;
						NewPosition.Y -= num14 >> 1;
						Collision.TileCollision(ref NewPosition, ref Velocity, num13, num14, CanFallThrough: true, CanAllowFall: true);
					}
					else
					{
						Collision.TileCollision(ref Position, ref Velocity, Width, Height, flag4, flag4);
					}
					if (Collision.UpCol)
					{
						Velocity.Y = 0.01f;
					}
					if (OldVelocity.X != Velocity.X)
					{
						HasXCollision = true;
					}
					if (OldVelocity.Y != Velocity.Y)
					{
						HasYCollision = true;
					}
					OldPosition = Position;
					Position.X += Velocity.X;
					Position.Y += Velocity.Y;
				}
			}
			else
			{
				OldPosition = Position;
				Position.X += Velocity.X;
				Position.Y += Velocity.Y;
			}
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT && !HasNoTileCollide && LifeMax > 1 && Collision.SwitchTiles(Position, Width, Height, OldPosition) && Type == (int)ID.BUNNY)
			{
				AI0 = 1f;
				AI1 = 400f;
				AI2 = 0f;
			}
			if (Active == 0)
			{
				ShouldNetUpdate = true;
			}
			if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				if (IsTownNPC)
				{
					NetSpam = 0;
				}
				if (ShouldNetUpdate2)
				{
					ShouldNetUpdate = true;
				}
				if (Active == 0)
				{
					NetSpam = 0;
				}
				if (ShouldNetUpdate)
				{
					if (NetSpam <= 180)
					{
						NetSpam += 60;
						NetMessage.CreateMessage1(23, i);
						NetMessage.SendMessage();
						ShouldNetUpdate2 = false;
					}
					else
					{
						ShouldNetUpdate2 = true;
					}
				}
				if (NetSpam > 0)
				{
					NetSpam--;
				}
				if (Active != 0 && IsTownNPC && GetHeadTextureID() != -1)
				{
					if (IsHomeless != WasHomeless || HomeTileX != OldHomeTileX || HomeTileY != OldHomeTileY)
					{
						NetMessage.CreateMessage4(60, i, Main.NPCSet[i].HomeTileX, Main.NPCSet[i].HomeTileY, IsHomeless ? 1 : 0);
						NetMessage.SendMessage();
					}
					WasHomeless = IsHomeless;
					OldHomeTileX = HomeTileX;
					OldHomeTileY = HomeTileY;
				}
			}
			FindFrame();
			CheckActive();
			ShouldNetUpdate = false;
			WasJustHit = false;
			if (Type == (int)ID.CHAOS_ELEMENTAL || Type == (int)ID.SPECTRAL_ELEMENTAL || Type == (int)ID.ILLUMINANT_BAT || Type == (int)ID.ILLUMINANT_SLIME)
			{
				for (int num15 = OldPos.Length - 1; num15 > 0; num15--)
				{
					ref Vector2 reference = ref OldPos[num15];
					reference = OldPos[num15 - 1];
					Lighting.AddLight(XYWH.X >> 4, XYWH.Y >> 4, new Vector3(0.3f, 0f, 0.2f));
				}
				ref Vector2 reference2 = ref OldPos[0];
				reference2 = Position;
			}
			else if (Type == (int)ID.CORRUPTOR || (Type >= (int)ID.RETINAZER && Type <= (int)ID.PRIME_LASER) || Type == (int)ID.PROBE || Type == (int)ID.POSSESSED_ARMOR)
			{
				for (int num16 = OldPos.Length - 1; num16 > 0; num16--)
				{
					ref Vector2 reference3 = ref OldPos[num16];
					reference3 = OldPos[num16 - 1];
				}
				ref Vector2 reference4 = ref OldPos[0];
				reference4 = Position;
			}
		}

		public Color GetAlpha(Color newColor)
		{
			float num = (255 - Alpha) / 255f;
			int num2 = (int)(newColor.R * num);
			int num3 = (int)(newColor.G * num);
			int num4 = (int)(newColor.B * num);
			int num5 = newColor.A - Alpha;
			if (Type == (int)ID.BURNING_SPHERE || Type == (int)ID.CHAOS_BALL || Type == (int)ID.WATER_SPHERE || Type == (int)ID.LAVA_SLIME || Type == (int)ID.HELLBAT)
			{
				return new Color(200, 200, 200, 0);
			}
			if (Type == (int)ID.BLAZING_WHEEL)
			{
				num2 = newColor.R;
				num3 = newColor.G;
				num4 = newColor.B;
			}
			else if (Type == (int)ID.PINK_JELLYFISH || Type == (int)ID.BLUE_JELLYFISH || Type == (int)ID.PIXIE || Type == (int)ID.GREEN_JELLYFISH)
			{
				num2 = (int)(newColor.R * 1.5);
				num3 = (int)(newColor.G * 1.5);
				num4 = (int)(newColor.B * 1.5);
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				if (num4 > 255)
				{
					num4 = 255;
				}
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num5 > 255)
			{
				num5 = 255;
			}
			return new Color(num2, num3, num4, num5);
		}

		public Color GetColor(Color newColor)
		{
			int num = Colour.R - (255 - newColor.R);
			int num2 = Colour.G - (255 - newColor.G);
			int num3 = Colour.B - (255 - newColor.B);
			int num4 = Colour.A - (255 - newColor.A);
			if (num < 0)
			{
				num = 0;
			}
			if (num > 255)
			{
				num = 255;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > 255)
			{
				num4 = 255;
			}
			return new Color(num, num2, num3, num4);
		}

		public string GetChat(Player player)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			for (int i = 0; i < MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active != 0)
				{
					if (Main.NPCSet[i].Type == (int)ID.MERCHANT)
					{
						flag = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.NURSE)
					{
						flag2 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.ARMS_DEALER)
					{
						flag3 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.DRYAD)
					{
						flag4 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.OLD_MAN)
					{
						flag5 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.DEMOLITIONIST)
					{
						flag6 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.MECHANIC)
					{
						flag7 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.GOBLIN_TINKERER)
					{
						flag8 = true;
					}
					else if (Main.NPCSet[i].Type == (int)ID.GUIDE)
					{
						flag9 = true;
					}
				}
			}
			string result = "";
			if (Type == (int)ID.MERCHANT)
			{
				if (!HasDownedBoss1 && Main.Rand.Next(3) == 0)
				{
					result = ((player.StatLifeMax < 200) ? Lang.NPCDialog(player, 1) : ((player.StatDefense > 10) ? Lang.NPCDialog(player, 3) : Lang.NPCDialog(player, 2)));
				}
				else if (Main.GameTime.DayTime)
				{
					if ((double)Main.GameTime.WorldTime < 16200f)
					{
						switch (Main.Rand.Next(3))
						{
						case 0:
							result = Lang.NPCDialog(player, 4);
							break;
						case 1:
							result = Lang.NPCDialog(player, 5);
							break;
						default:
							result = Lang.NPCDialog(player, 6);
							break;
						}
					}
					else if ((double)Main.GameTime.WorldTime > 37800f)
					{
						switch (Main.Rand.Next(3))
						{
						case 0:
							result = Lang.NPCDialog(player, 7);
							break;
						case 1:
							result = Lang.NPCDialog(player, 8);
							break;
						default:
							result = Lang.NPCDialog(player, 9);
							break;
						}
					}
					else
					{
						switch (Main.Rand.Next(3))
						{
						case 0:
							result = Lang.NPCDialog(player, 10);
							break;
						case 1:
							result = Lang.NPCDialog(player, 11);
							break;
						default:
							result = Lang.NPCDialog(player, 12);
							break;
						}
					}
				}
				else if (Main.GameTime.IsBloodMoon)
				{
					if (flag2 && flag7 && Main.Rand.Next(3) == 0)
					{
						result = Lang.NPCDialog(player, 13);
					}
					else
					{
						switch (Main.Rand.Next(4))
						{
						case 0:
							result = Lang.NPCDialog(player, 14);
							break;
						case 1:
							result = Lang.NPCDialog(player, 15);
							break;
						case 2:
							result = Lang.NPCDialog(player, 16);
							break;
						default:
							result = Lang.NPCDialog(player, 17);
							break;
						}
					}
				}
				else if (Main.GameTime.WorldTime < 9720.0)
				{
					result = ((Main.Rand.Next(2) != 0) ? Lang.NPCDialog(player, 19) : Lang.NPCDialog(player, 18));
				}
				else if (Main.GameTime.WorldTime > 22680.0)
				{
					result = ((Main.Rand.Next(2) != 0) ? Lang.NPCDialog(player, 21) : Lang.NPCDialog(player, 20));
				}
				else
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 22);
						break;
					case 1:
						result = Lang.NPCDialog(player, 23);
						break;
					default:
						result = Lang.NPCDialog(player, 24);
						break;
					}
				}
			}
			else if (Type == (int)ID.NURSE)
			{
				if (Main.GameTime.IsBloodMoon)
				{
					if (player.statLife < player.StatLifeMax * 0.66)
					{
						switch (Main.Rand.Next(3))
						{
						case 0:
							result = Lang.NPCDialog(player, 25);
							break;
						case 1:
							result = Lang.NPCDialog(player, 26);
							break;
						default:
							result = Lang.NPCDialog(player, 27);
							break;
						}
					}
					else
					{
						switch (Main.Rand.Next(4))
						{
						case 0:
							result = Lang.NPCDialog(player, 28);
							break;
						case 1:
							result = Lang.NPCDialog(player, 29);
							break;
						case 2:
							result = Lang.NPCDialog(player, 30);
							break;
						default:
							result = Lang.NPCDialog(player, 31);
							break;
						}
					}
				}
				else if (Main.Rand.Next(3) == 0 && !HasDownedBoss3)
				{
					result = Lang.NPCDialog(player, 32);
				}
				else if (flag6 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 33);
				}
				else if (flag3 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 34);
				}
				else if (flag9 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 35);
				}
				else if (player.statLife < player.StatLifeMax * 0.33)
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 36);
						break;
					case 1:
						result = Lang.NPCDialog(player, 37);
						break;
					case 2:
						result = Lang.NPCDialog(player, 38);
						break;
					case 3:
						result = Lang.NPCDialog(player, 39);
						break;
					default:
						result = Lang.NPCDialog(player, 40);
						break;
					}
				}
				else if (player.statLife < player.StatLifeMax * 0.66)
				{
					switch (Main.Rand.Next(7))
					{
					case 0:
						result = Lang.NPCDialog(player, 41);
						break;
					case 1:
						result = Lang.NPCDialog(player, 42);
						break;
					case 2:
						result = Lang.NPCDialog(player, 43);
						break;
					case 3:
						result = Lang.NPCDialog(player, 44);
						break;
					case 4:
						result = Lang.NPCDialog(player, 45);
						break;
					case 5:
						result = Lang.NPCDialog(player, 46);
						break;
					default:
						result = Lang.NPCDialog(player, 47);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 48);
						break;
					case 1:
						result = Lang.NPCDialog(player, 49);
						break;
					case 2:
						result = Lang.NPCDialog(player, 50);
						break;
					default:
						result = Lang.NPCDialog(player, 51);
						break;
					}
				}
			}
			else if (Type == (int)ID.ARMS_DEALER)
			{
				if (HasDownedBoss3 && !Main.InHardMode)
				{
					result = Lang.NPCDialog(player, 58);
				}
				else if (flag2 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 59);
				}
				else if (flag2 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 60);
				}
				else if (flag4 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 61);
				}
				else if (flag6 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 62);
				}
				else if (flag6 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 63);
				}
				else if (Main.GameTime.IsBloodMoon)
				{
					result = ((Main.Rand.Next(2) != 0) ? Lang.NPCDialog(player, 65) : Lang.NPCDialog(player, 64));
				}
				else
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 66);
						break;
					case 1:
						result = Lang.NPCDialog(player, 67);
						break;
					default:
						result = Lang.NPCDialog(player, 68);
						break;
					}
				}
			}
			else if (Type == (int)ID.DRYAD)
			{
				if (!HasDownedBoss2 && Main.Rand.Next(3) == 0)
				{
					result = Lang.NPCDialog(player, 69);
				}
				else if (flag3 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 70);
				}
				else if (flag && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 71);
				}
				else if (flag5 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 72);
				}
				else if (Main.GameTime.IsBloodMoon)
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 73);
						break;
					case 1:
						result = Lang.NPCDialog(player, 74);
						break;
					case 2:
						result = Lang.NPCDialog(player, 75);
						break;
					default:
						result = Lang.NPCDialog(player, 76);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 77);
						break;
					case 1:
						result = Lang.NPCDialog(player, 78);
						break;
					case 2:
						result = Lang.NPCDialog(player, 79);
						break;
					case 3:
						result = Lang.NPCDialog(player, 80);
						break;
					default:
						result = Lang.NPCDialog(player, 81);
						break;
					}
				}
			}
			else if (Type == (int)ID.OLD_MAN)
			{
				if (Main.GameTime.DayTime)
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 82);
						break;
					case 1:
						result = Lang.NPCDialog(player, 83);
						break;
					default:
						result = Lang.NPCDialog(player, 84);
						break;
					}
				}
				else if (player.StatLifeMax < 300 || player.StatDefense < 10)
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 85);
						break;
					case 1:
						result = Lang.NPCDialog(player, 86);
						break;
					case 2:
						result = Lang.NPCDialog(player, 87);
						break;
					default:
						result = Lang.NPCDialog(player, 88);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 89);
						break;
					case 1:
						result = Lang.NPCDialog(player, 90);
						break;
					case 2:
						result = Lang.NPCDialog(player, 91);
						break;
					default:
						result = Lang.NPCDialog(player, 92);
						break;
					}
				}
			}
			else if (Type == (int)ID.DEMOLITIONIST)
			{
				if (!HasDownedBoss2 && Main.Rand.Next(3) == 0)
				{
					result = Lang.NPCDialog(player, 93);
				}
				if (Main.GameTime.IsBloodMoon)
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 94);
						break;
					case 1:
						result = Lang.NPCDialog(player, 95);
						break;
					default:
						result = Lang.NPCDialog(player, 96);
						break;
					}
				}
				else if (flag3 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 97);
				}
				else if (flag3 && Main.Rand.Next(5) == 0)
				{
					result = Lang.NPCDialog(player, 98);
				}
				else if (flag2 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 99);
				}
				else if (flag4 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 100);
				}
				else if (!Main.GameTime.DayTime)
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 101);
						break;
					case 1:
						result = Lang.NPCDialog(player, 102);
						break;
					case 2:
						result = Lang.NPCDialog(player, 103);
						break;
					default:
						result = Lang.NPCDialog(player, 104);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 105);
						break;
					case 1:
						result = Lang.NPCDialog(player, 106);
						break;
					case 2:
						result = Lang.NPCDialog(player, 107);
						break;
					case 3:
						result = Lang.NPCDialog(player, 108);
						break;
					default:
						result = Lang.NPCDialog(player, 109);
						break;
					}
				}
			}
			else if (Type == (int)ID.CLOTHIER)
			{
				if (!flag7 && Main.Rand.Next(2) == 0)
				{
					result = Lang.NPCDialog(player, 110);
				}
				else if (Main.GameTime.IsBloodMoon)
				{
					result = Lang.NPCDialog(player, 111);
				}
				else if (flag2 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 112);
				}
				else if (player.head == 24)
				{
					result = Lang.NPCDialog(player, 113);
				}
				else
				{
					switch (Main.Rand.Next(6))
					{
					case 0:
						result = Lang.NPCDialog(player, 114);
						break;
					case 1:
						result = Lang.NPCDialog(player, 115);
						break;
					case 2:
						result = Lang.NPCDialog(player, 116);
						break;
					case 3:
						result = Lang.NPCDialog(player, 117);
						break;
					case 4:
						result = Lang.NPCDialog(player, 118);
						break;
					default:
						result = Lang.NPCDialog(player, 119);
						break;
					}
				}
			}
			else if (Type == (int)ID.BOUND_GOBLIN)
			{
				result = Lang.NPCDialog(player, 120);
			}
			else if (Type == (int)ID.GOBLIN_TINKERER)
			{
				if (IsHomeless)
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 121);
						break;
					case 1:
						result = Lang.NPCDialog(player, 122);
						break;
					case 2:
						result = Lang.NPCDialog(player, 123);
						break;
					case 3:
						result = Lang.NPCDialog(player, 124);
						break;
					default:
						result = Lang.NPCDialog(player, 125);
						break;
					}
				}
				else if (flag7 && Main.Rand.Next(4) == 0)
				{
					result = Lang.NPCDialog(player, 126);
				}
				else if (!Main.GameTime.DayTime)
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 127);
						break;
					case 1:
						result = Lang.NPCDialog(player, 128);
						break;
					case 2:
						result = Lang.NPCDialog(player, 129);
						break;
					case 3:
						result = Lang.NPCDialog(player, 130);
						break;
					default:
						result = Lang.NPCDialog(player, 131);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 132);
						break;
					case 1:
						result = Lang.NPCDialog(player, 133);
						break;
					case 2:
						result = Lang.NPCDialog(player, 134);
						break;
					case 3:
						result = Lang.NPCDialog(player, 135);
						break;
					default:
						result = Lang.NPCDialog(player, 136);
						break;
					}
				}
			}
			else if (Type == (int)ID.BOUND_WIZARD)
			{
				result = Lang.NPCDialog(player, 137);
			}
			else if (Type == (int)ID.WIZARD)
			{
				if (IsHomeless)
				{
					int num = Main.Rand.Next(3);
					if (num == 0)
					{
						result = Lang.NPCDialog(player, 138);
					}
					else if (num == 1 && !player.male)
					{
						result = Lang.NPCDialog(player, 139);
					}
					else
					{
						switch (num)
						{
						case 1:
							result = Lang.NPCDialog(player, 140);
							break;
						case 2:
							result = Lang.NPCDialog(player, 141);
							break;
						}
					}
				}
				else if (player.male && flag9 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 142);
				}
				else if (player.male && flag6 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 143);
				}
				else if (player.male && flag8 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 144);
				}
				else if (!player.male && flag2 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 145);
				}
				else if (!player.male && flag7 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 146);
				}
				else if (!player.male && flag4 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 147);
				}
				else if (!Main.GameTime.DayTime)
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 148);
						break;
					case 1:
						result = Lang.NPCDialog(player, 149);
						break;
					case 2:
						result = Lang.NPCDialog(player, 150);
						break;
					}
				}
				else
				{
					switch (Main.Rand.Next(5))
					{
					case 0:
						result = Lang.NPCDialog(player, 151);
						break;
					case 1:
						result = Lang.NPCDialog(player, 152);
						break;
					case 2:
						result = Lang.NPCDialog(player, 153);
						break;
					case 3:
						result = Lang.NPCDialog(player, 154);
						break;
					default:
						result = Lang.NPCDialog(player, 155);
						break;
					}
				}
			}
			else if (Type == (int)ID.BOUND_MECHANIC)
			{
				result = Lang.NPCDialog(player, 156);
			}
			else if (Type == (int)ID.MECHANIC)
			{
				if (IsHomeless)
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 157);
						break;
					case 1:
						result = Lang.NPCDialog(player, 158);
						break;
					case 2:
						result = Lang.NPCDialog(player, 159);
						break;
					default:
						result = Lang.NPCDialog(player, 160);
						break;
					}
				}
				else if (Main.GameTime.IsBloodMoon)
				{
					switch (Main.Rand.Next(4))
					{
					case 0:
						result = Lang.NPCDialog(player, 161);
						break;
					case 1:
						result = Lang.NPCDialog(player, 162);
						break;
					case 2:
						result = Lang.NPCDialog(player, 163);
						break;
					default:
						result = Lang.NPCDialog(player, 164);
						break;
					}
				}
				else if (flag8 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 165);
				}
				else if (flag3 && Main.Rand.Next(6) == 0)
				{
					result = Lang.NPCDialog(player, 166);
				}
				else
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 167);
						break;
					case 1:
						result = Lang.NPCDialog(player, 168);
						break;
					default:
						result = Lang.NPCDialog(player, 169);
						break;
					}
				}
			}
			else if (Type == (int)ID.GUIDE)
			{
				if (Main.GameTime.IsBloodMoon)
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 170);
						break;
					case 1:
						result = Lang.NPCDialog(player, 171);
						break;
					default:
						result = Lang.NPCDialog(player, 172);
						break;
					}
				}
				else if (!Main.GameTime.DayTime)
				{
					result = Lang.NPCDialog(player, 173);
				}
				else
				{
					switch (Main.Rand.Next(3))
					{
					case 0:
						result = Lang.NPCDialog(player, 174);
						break;
					case 1:
						result = Lang.NPCDialog(player, 175);
						break;
					default:
						result = Lang.NPCDialog(player, 176);
						break;
					}
				}
			}
			else if (Type == (int)ID.SANTA_CLAUS)
			{
				switch (Main.Rand.Next(3))
				{
				case 0:
					result = Lang.NPCDialog(player, 224);
					break;
				case 1:
					result = Lang.NPCDialog(player, 225);
					break;
				case 2:
					result = Lang.NPCDialog(player, 226);
					break;
				}
			}
			return result;
		}

		public static void CheckForTownSpawns()
		{
			if (++CheckForSpawnsTimer < 7200)
			{
				return;
			}
			CheckForSpawnsTimer = 0;
			int num = 0;
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active != 0)
				{
					num++;
				}
			}
			WorldGen.ToSpawnNPC = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			int num14 = 0;
			bool flag = true;
			for (int j = 0; j < MaxNumNPCs; j++)
			{
				if (Main.NPCSet[j].Active != 0 && Main.NPCSet[j].IsTownNPC)
				{
					if (Main.NPCSet[j].Type != (int)ID.OLD_MAN && !Main.NPCSet[j].IsHomeless)
					{
						WorldGen.QuickFindHome(j);
					}
					bool flag2 = Main.NPCSet[j].IsHomeless;
					if (Main.NPCSet[j].Type == (int)ID.OLD_MAN)
					{
						num7++;
						flag2 = false;
					}
					else if (Main.NPCSet[j].Type == (int)ID.MERCHANT)
					{
						num2++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.NURSE)
					{
						num3++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.ARMS_DEALER)
					{
						num5++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.DRYAD)
					{
						num4++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.GUIDE)
					{
						num6++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.DEMOLITIONIST)
					{
						num8++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.CLOTHIER)
					{
						num9++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.GOBLIN_TINKERER)
					{
						num11++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.WIZARD)
					{
						num10++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.MECHANIC)
					{
						num12++;
					}
					else if (Main.NPCSet[j].Type == (int)ID.SANTA_CLAUS)
					{
						num13++;
						flag2 = false;
					}
					flag = flag && !flag2;
					num14++;
				}
			}
			if (WorldGen.ToSpawnNPC != 0)
			{
				return;
			}
			int num15 = 0;
			bool flag3 = false;
			int num16 = 0;
			bool flag4 = false;
			bool flag5 = false;
			for (int k = 0; k < Player.MaxNumPlayers; k++)
			{
				if (Main.PlayerSet[k].Active == 0)
				{
					continue;
				}
				for (int l = 0; l < Player.MaxNumInventory; l++)
				{
					if (Main.PlayerSet[k].Inventory[l].Type > 0 && Main.PlayerSet[k].Inventory[l].Stack > 0)
					{
						if (Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.COPPER_COIN)
						{
							num15 += Main.PlayerSet[k].Inventory[l].Stack;
						}
						else if (Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.SILVER_COIN)
						{
							num15 += Main.PlayerSet[k].Inventory[l].Stack * 100;
						}
						else if (Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.GOLD_COIN)
						{
							num15 += Main.PlayerSet[k].Inventory[l].Stack * 10000;
						}
						else if (Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.PLATINUM_COIN)
						{
							num15 += Main.PlayerSet[k].Inventory[l].Stack * 1000000;
						}
						if (Main.PlayerSet[k].Inventory[l].Ammo == 14 || Main.PlayerSet[k].Inventory[l].UseAmmo == 14)
						{
							flag4 = true;
						}
						if (Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.BOMB || Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.DYNAMITE || Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.GRENADE || Main.PlayerSet[k].Inventory[l].Type == (int)Item.ID.STICKY_BOMB)
						{
							flag5 = true;
						}
					}
				}
				int num17 = Main.PlayerSet[k].StatLifeMax / 20;
				if (num17 > 5)
				{
					flag3 = true;
				}
				num16 += num17;
			}
			if (!HasDownedBoss3 && num7 == 0)
			{
				int num18 = NewNPC(Main.DungeonX * 16 + 8, Main.DungeonY * 16, (int)ID.OLD_MAN);
				Main.NPCSet[num18].IsHomeless = false;
				Main.NPCSet[num18].HomeTileX = Main.DungeonX;
				Main.NPCSet[num18].HomeTileY = Main.DungeonY;
			}
			if (num6 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.GUIDE;
			}
			else if (num15 > 5000.0 && num2 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.MERCHANT;
			}
			else if (flag3 && num3 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.NURSE;
			}
			else if (flag4 && num5 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.ARMS_DEALER;
			}
			else if ((HasDownedBoss1 || HasDownedBoss2 || HasDownedBoss3) && num4 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.DRYAD;
			}
			else if (flag5 && num2 > 0 && num8 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.DEMOLITIONIST;
			}
			else if (HasDownedBoss3 && num9 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.CLOTHIER;
			}
			else if (HasSavedGoblin && num11 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.GOBLIN_TINKERER;
			}
			else if (HasSavedWizard && num10 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.WIZARD;
			}
			else if (HasSavedMech && num12 < 1)
			{
				WorldGen.ToSpawnNPC = (int)ID.MECHANIC;
			}
			else if (HasDownedFrost && num13 < 1 && Time.xMas)
			{
				WorldGen.ToSpawnNPC = (int)ID.SANTA_CLAUS;
			}
		}

		public void ApplyProjectileBuff(int ProjType)
		{
			switch (ProjType)
			{
			case 2:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case 15:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 300);
				}
				break;
			case 19:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case 33:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 420);
				}
				break;
			case 34:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 240);
				}
				break;
			case 35:
				if (Main.Rand.Next(4) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case 54:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 600);
				}
				break;
			case 63:
				if (Main.Rand.Next(3) != 0)
				{
					AddBuff((int)Buff.ID.CONFUSED, 120);
				}
				break;
			case 85:
				AddBuff((int)Buff.ID.ON_FIRE, 1200);
				break;
			case 95:
			case 103:
			case 104:
			case 113:
				AddBuff((int)Buff.ID.ON_FIRE_2, 420);
				break;
			case 98:
				AddBuff((int)Buff.ID.POISONED, 600);
				break;
			}
		}

		public void ApplyWeaponBuff(int WeaponType)
		{
			switch (WeaponType)
			{
			case (int)Item.ID.FIERY_GREATSWORD:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case (int)Item.ID.MOLTEN_PICKAXE:
				if (Main.Rand.Next(10) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case (int)Item.ID.BLADE_OF_GRASS:
			case (int)Item.ID.TONBOGIRI:
				if (Main.Rand.Next(4) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 420);
				}
				break;
			case (int)Item.ID.MOLTEN_HAMAXE:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180);
				}
				break;
			case (int)Item.ID.TIZONA:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.BLEED, 600);
				}
				break;
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public void DrawInfo(WorldView view)
		{
			if (RealLife >= 0 && RealLife != WhoAmI)
			{
				if (view.drawNpcName[RealLife])
				{
					Main.NPCSet[RealLife].DrawInfo(view);
				}
				return;
			}
			view.drawNpcName[WhoAmI] = false;
			string s = ((!HasName()) ? DisplayName : GetName());
			int num = XYWH.X + (Width >> 1) - view.ScreenPosition.X;
			int num2 = XYWH.Y + Height - view.ScreenPosition.Y - 10;
#if VERSION_103 || VERSION_FINAL
			num2 += (int)UI.DrawStringCT(UI.fontSmallOutlineFull, s, num, num2, UI.mouseTextColor);
#else
			num2 += (int)UI.DrawStringCT(UI.SmallFont, s, num, num2, UI.mouseTextColor);
#endif
			if (LifeMax <= 1 || DontTakeDamage)
			{
				return;
			}
			int num3 = Life - HealthBarLife;
			if (num3 != 0)
			{
				if (Math.Abs(num3) > 1)
				{
					HealthBarLife += num3 >> 2;
				}
				else
				{
					HealthBarLife = Life;
				}
			}
			Rectangle rect = default(Rectangle);
			rect.X = num - 22;
			rect.Y = num2 - 4;
			rect.Height = 10;
			rect.Width = 52;
			Color wINDOW_OUTLINE = UI.WINDOW_OUTLINE;
			Main.DrawRect(rect, wINDOW_OUTLINE, ToCenter: false);
			rect.X += 2;
			rect.Y += 2;
			rect.Width = HealthBarLife * 48 / LifeMax;
			rect.Height = 6;
			wINDOW_OUTLINE = new Color((48 - rect.Width) * 5, rect.Width * 5, 16, 128);
			Main.DrawSolidRect(ref rect, wINDOW_OUTLINE);
			if (rect.Width < 48)
			{
				wINDOW_OUTLINE = new Color(0, 0, 0, 128);
				rect.X += rect.Width;
				rect.Width = 48 - rect.Width;
				Main.DrawSolidRect(ref rect, wINDOW_OUTLINE);
			}
		}
	}
}
