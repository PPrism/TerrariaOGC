using System;
using System.Collections;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Terraria.Achievements;
using Terraria.Leaderboards;
using static System.Net.Mime.MediaTypeNames;
using Terraria.CreateCharacter;


#if !USE_ORIGINAL_CODE
using System.Diagnostics;
#endif

namespace Terraria
{
	public sealed class Player
	{
		public struct Adj
		{
			public bool i;

			public bool old;
		}

		public enum ExtraStorage
		{
			NONE = -1,
			PIGGYBANK = -2,
			SAFE = -3
		}

		public const int MaxNumPlayers = 8;

#if VERSION_103
		public const int NumArmorSlots = 3;
		public const int NumEquipSlots = 5;
		public const int NumVanSlots = 3;
		public const int NumDyeSlots = 3;
		public const int MaxNumArmor = NumArmorSlots + NumEquipSlots + NumVanSlots + NumDyeSlots;
#elif VERSION_FINAL
		public const int NumArmorSlots = 3;
		public const int NumEquipSlots = 5;
		public const int NumVanSlots = NumArmorSlots + NumEquipSlots;
		public const int NumDyeSlots = NumVanSlots;
		public const int MaxNumArmor = NumArmorSlots + NumEquipSlots + NumVanSlots + NumDyeSlots;
#else
		public const int NumArmorSlots = 3;
		public const int NumEquipSlots = 5;
		public const int NumVanSlots = 3;
		public const int MaxNumArmor = NumArmorSlots + NumEquipSlots + NumVanSlots;
#endif

		public const int NumInvSlots = 40;
		public const int NumCoinSlots = 4;
		public const int NumAmmoSlots = 4;
		public const int MaxNumInventory = NumInvSlots + NumCoinSlots + NumAmmoSlots;

		public const int MaxNumHairs = 36;

#if VERSION_101
		public const int MaxNumArmorHead = 52;

		public const int MaxNumArmorBody = 33;

		public const int MaxNumArmorLegs = 32;
#else
		public const int MaxNumArmorHead = 48;

		public const int MaxNumArmorBody = 29;

		public const int MaxNumArmorLegs = 28;
#endif

#if (!VERSION_INITIAL || IS_PATCHED)
		private const int NetForceUpdateTimer = 96;
#endif

		public const int NameLength = 16;

#if VERSION_103 || VERSION_FINAL
		public const int MaxNumBuffs = 16;
#else
		public const int MaxNumBuffs = 10;
#endif

		public const short MaxBreath = 200;

		private const int bodyFrameHeight = 56;

		private const int legFrameHeight = 56;

		public const ushort width = 20;

		public const ushort height = 42;

		public const int tileRangeX = 5;

		public const int tileRangeY = 4;

		private const float CURSOR_SPEED = 6f;

		private const int itemGrabRange = 38;

		private const float itemGrabSpeed = 0.45f;

		private const float itemGrabSpeedMax = 4f;

		private const int rocketTimeMax = 7;

		private const int SMART_RAYS = 3;

		public NetClient client;

		public WorldView CurrentView;

		public UI ui;

		public byte wings;

		public short wingTime;

		public byte wingFrame;

		public byte wingFrameCounter;

		public bool flapSound;

		public bool male = true;

		public bool ghost;

		public byte ghostFrameCounter;

		public bool pvpDeath;

		public bool ZoneDungeon;

		public bool ZoneEvil;

		public bool zoneHoly;

		public bool ZoneMeteor;

		public bool ZoneJungle;

		public bool boneArmor;

		public float TownNPCs;

		public Rectangle XYWH = new Rectangle(0, 0, width, height);

		public Vector2 Position;

		public Vector2 oldPosition;

		public Vector2 velocity;

#if !USE_ORIGINAL_CODE
		public Vector2 OldVelocity;
#endif

		public float bodyFrameCounter;

		public float legFrameCounter;

		public short immuneTime;

		public short immuneAlpha;

		public sbyte immuneAlphaDirection;

		public bool immune;

		public byte team;

		public sbyte netSkip;

#if (!VERSION_INITIAL || IS_PATCHED)
        private short netForceControlUpdate = NetForceUpdateTimer;

		public sbyte[] PlayerQuickAccess = new sbyte[4] { -1, -1, -1, -1 };
#endif

		public byte reuseDelay;

		private short maxRegenDelay;

		public short sign = -1;

		public sbyte SelectedItem;

		public sbyte oldSelectedItem = -1;

		public float ActiveNPCs;

		public short itemAnimation;

		public short itemAnimationMax;

		public byte itemTime;

		public byte noThrow;

		public short toolTime;

		public float itemRotation;

		public short itemWidth;

		public short itemHeight;

		public Vector2i itemLocation;

		public float ghostFade;

		public float ghostDir = 1f;

		public Buff[] buff = new Buff[MaxNumBuffs];

		public short heldProj = -1;

		public short breathCD;

		public short breath = MaxBreath;

		public bool socialShadow;

		public string setBonus;

		public Item[] armor = new Item[MaxNumArmor];

		public Item[] Inventory = new Item[MaxNumInventory + 1];

		public Chest bank = new Chest();

		public Chest safe = new Chest();

		public float headRotation;

		public float bodyRotation;

		public float legRotation;

		public Vector2 headPosition;

		public Vector2 bodyPosition;

		public Vector2 legPosition;

		public Vector2 headVelocity;

		public Vector2 bodyVelocity;

		public Vector2 legVelocity;

		public bool IsDead;

		public short respawnTimer;

		public string CharacterName = "";

		public string Name = "";

		public short attackCD;

		public ushort potionDelay;

		public byte difficulty;

		public bool IsWet;

		public byte wetCount;

		public bool lavaWet;

		public short hitTile;

		public short hitTileX;

		public short hitTileY;

		public int jump;

		public short head = -1;

		public short body = -1;

		public short legs = -1;

		private short bodyFrameY;

		private short legFrameY;

		public Vector2 controlDir = default;

		public bool controlLeft;

		public bool controlRight;

		public bool controlUp;

		public bool IsControlDown;

		public bool controlJump;

		public bool controlUseItem;

		public bool controlUseTile;

		public bool controlThrow;

		public bool controlInv;

		public bool controlHook;

		public bool releaseJump;

		public bool releaseUseItem;

		public bool releaseUseTile;

		public bool releaseHook;

		public bool delayUseItem;

		public byte Active;

		public sbyte direction = 1;

		public byte WhoAmI;

		public sbyte runSoundDelay;

		public bool fireWalk;

		private float buffR = 1f;

		private float buffG = 1f;

		private float buffB = 1f;

		public float shadow;

		public float manaCost = 1f;

		public Vector2[] shadowPos = new Vector2[3];

		public byte shadowCount;

		public bool channel;

		public short StatDefense;

		public short statAttack;

		public short healthBarLife = 100;

		public short StatLifeMax = 100;

		public short statLife = 100;

		public short statMana;

		public short statManaMax;

		public short statManaMax2;

		public int lifeRegen;

		public int lifeRegenCount;

		public int lifeRegenTime;

		public int manaRegen;

		public int manaRegenCount;

		public int manaRegenDelay;

		public bool manaRegenBuff;

		public bool noKnockback;

		public bool spaceGun;

		public sbyte gravDir = 1;

		public byte freeAmmoChance;

		public byte stickyBreak;

		public bool lightOrb;

		public bool fairy;

		public sbyte pet = (sbyte)Pet.NONE;

		public bool archery;

		public bool poisoned;

		public bool blind;

		public bool onFire;

		public bool onFire2;

		public bool noItems;

		public bool wereWolf;

		public bool wolfAcc;

		public bool rulerAcc;

		public bool bleed;

		public bool confused;

		public bool accMerman;

		public bool merman;

		public bool brokenArmor;

		public bool silence;

		public bool slow;

		public bool IsHorrified;

		public bool tongued;

		public bool kbGlove;

		public bool starCloak;

		public bool longInvince;

		public bool manaFlower;

		public short meleeCrit = 4;

		public short rangedCrit = 4;

		public short magicCrit = 4;

		public float meleeDamage = 1f;

		public float rangedDamage = 1f;

		public float magicDamage = 1f;

		public float meleeSpeed = 1f;

		public float moveSpeed = 1f;

		public float pickSpeed = 1f;

		public int SpawnX = -1;

		public int SpawnY = -1;

		public short[] spX = new short[200];

		public short[] spY = new short[200];

		public string[] spN = new string[200];

		public int[] spI = new int[200];

		public short tileTargetX;

		public short tileTargetY;

		public short tileInteractX;

		public short tileInteractY;

		private float relativeTargetX;

		private float relativeTargetY;

		public bool adjWater;

		public bool oldAdjWater;

		public Adj[] adjTile = new Adj[Main.MaxNumTilenames];

		public Color hairColor = new Color(215, 90, 55);

		public Color skinColor = new Color(255, 125, 90);

		public Color eyeColor = new Color(105, 90, 75);

		public Color shirtColor = new Color(175, 165, 140);

		public Color underShirtColor = new Color(160, 180, 215);

		public Color pantsColor = new Color(255, 230, 175);

		public Color shoeColor = new Color(160, 105, 60);

		public byte hair;

		public bool hostile;

		public byte accWatch;

		public bool accCompass;

		public bool accDepthMeter;

		public bool accDivingHelm;

		public bool accFlipper;

		public bool doubleJump;

		public bool jumpAgain;

		public bool spawnMax;

		public byte blockRange;

		public sbyte grappleItemSlot = -1;

		public short[] grappling = new short[20];

		public byte grapCount;

		public sbyte rocketTime;

		public sbyte rocketDelay;

		public sbyte rocketDelay2;

		public bool rocketRelease;

		public bool rocketFrame;

		public byte rocketBoots;

		public bool canRocket;

		public bool jumpBoost;

		public bool noFallDmg;

		public byte swimTime;

		public bool killGuide;

		public bool lavaImmune;

		public bool gills;

		public bool slowFall;

		public bool findTreasure;

		public bool invis;

		public bool detectCreature;

		public bool NightVision;

		public bool enemySpawns;

		public bool thorns;

		public bool waterWalk;

		public bool gravControl;

		public short PlayerChest = -1;

		public short chestX;

		public short chestY;

#if !USE_ORIGINAL_CODE
		public int CurrentGround = 0;
#endif

		public short fallStart;

		public ushort potionDelayTime = (ushort)Item.PotionDelay;

		public short TalkNPC = -1;

		public short npcChatBubble = -1;

		public BitArray itemsFound = new BitArray((int)Item.ID.NUM_TYPES);

		public BitArray craftingStationsFound = new BitArray(Main.MaxNumTilenames);

		public BitArray RecipesFound = new BitArray(Recipe.MaxNumRecipes);

		public BitArray recipesNew = new BitArray(Recipe.MaxNumRecipes);

		private uint totalSunMoonTransitions;

		private byte hellAndBackState;

#if !USE_ORIGINAL_CODE
		public uint HellevatorTimer = 0;
#endif

		public bool ToKill;

		public bool IsAnnounced;

		public string oldName = "";

		private static readonly sbyte[] TARGET_SEARCH_DIR_RIGHT = new sbyte[180]
		{
			20,
			42,
			0,
			-16,
			0,
			-16,
			-16,
			32,
			0,
			-16,
			0,
			-16,
			-16,
			32,
			0,
			-16,
			0,
			-16,
			48,
			32,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16
		};

		private static readonly sbyte[] TARGET_SEARCH_DIR_LEFT = new sbyte[180]
		{
			-16,
			42,
			0,
			-16,
			0,
			-16,
			16,
			32,
			0,
			-16,
			0,
			-16,
			16,
			32,
			0,
			-16,
			0,
			-16,
			-48,
			32,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			0,
			16,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16,
			0,
			-16
		};

		private Vector2i[] smartLocation = new Vector2i[3];

		public void HealEffect(int healAmount)
		{
#if USE_ORIGINAL_CODE
			CombatText.NewText(Position, width, height, healAmount);
#else
			CombatText.NewText(Position, width, height, healAmount, Type: 2);
#endif
			if (isLocal())
			{
				NetMessage.CreateMessage2(35, WhoAmI, healAmount);
				NetMessage.SendMessage();
			}
		}

		public void ManaEffect(int manaAmount)
		{
#if USE_ORIGINAL_CODE
			CombatText.NewText(Position, width, height, manaAmount);
#else
			CombatText.NewText(Position, width, height, manaAmount, Type: 3);
#endif
			if (isLocal())
			{
				NetMessage.CreateMessage2(43, WhoAmI, manaAmount);
				NetMessage.SendMessage();
			}
		}

		public static Player FindClosest(ref Rectangle rect)
		{
			Player player = null;
			int num = int.MaxValue;
			for (int i = 0; i < MaxNumPlayers; i++)
			{
				Player player2 = Main.PlayerSet[i];
				if (player2.Active != 0 && !player2.IsDead)
				{
					int num2 = Math.Abs(player2.XYWH.X + 10 - (rect.X + (rect.Width >> 1))) + Math.Abs(player2.XYWH.Y + 21 - (rect.Y + (rect.Height >> 1)));
					if (num2 < num)
					{
						num = num2;
						player = player2;
					}
				}
			}
			if (player == null)
			{
				for (int j = 0; j < MaxNumPlayers; j++)
				{
					player = Main.PlayerSet[j];
					if (player.Active != 0)
					{
						break;
					}
				}
			}
			return player;
		}

		public void toggleInv()
		{
			if (ui.InventoryMode > 0)
			{
				Main.PlaySound(11);
				ui.CloseInventory();
			}
			else if (TalkNPC >= 0)
			{
				TalkNPC = -1;
				ui.npcChatText = null;
				Main.PlaySound(11);
			}
			else if (sign >= 0)
			{
				sign = -1;
				ui.editSign = false;
				ui.npcChatText = null;
				Main.PlaySound(11);
			}
			else
			{
				Main.PlaySound(10);
				ui.OpenInventory();
			}
		}

		public void dropItemCheck()
		{
			if (ui.InventoryMode == 0)
			{
				noThrow = 0;
			}
			else if (noThrow > 0)
			{
				noThrow--;
			}
			if (noThrow == 0 && ((controlThrow && Inventory[SelectedItem].Type > 0) || ((ui.InventoryMode == 0 || ui.IsButtonUntriggered(UI.BTN_DROP)) && ui.mouseItem.Type > 0 && ui.mouseItem.Stack > 0)))
			{
				Item item = default(Item);
				bool flag = false;
				if ((ui.InventoryMode == 0 || ui.IsButtonUntriggered(UI.BTN_INVENTORY_DROP)) && ui.mouseItem.Type > 0 && ui.mouseItem.Stack > 0)
				{
					item = Inventory[SelectedItem];
					ref Item reference = ref Inventory[SelectedItem];
					reference = ui.mouseItem;
					delayUseItem = true;
					controlUseItem = false;
					flag = true;
				}
				int num = Item.NewItem(XYWH.X, XYWH.Y, 20, 42, Inventory[SelectedItem].Type, 1, DoNotBroadcast: true);
				if (!flag && Inventory[SelectedItem].Type == (int)Item.ID.TORCH && Inventory[SelectedItem].Stack > 1)
				{
					Inventory[SelectedItem].Stack--;
				}
				else
				{
					Inventory[SelectedItem].Position = Main.ItemSet[num].Position;
					ref Item reference2 = ref Main.ItemSet[num];
					reference2 = Inventory[SelectedItem];
					Inventory[SelectedItem].Init();
				}
				Main.ItemSet[num].NoGrabDelay = 100;
				Main.ItemSet[num].Velocity.Y = -2f;
				Main.ItemSet[num].Velocity.X = 4 * direction + velocity.X;
				if (ui.mouseItem.Type > 0 && (ui.InventoryMode == 0 || ui.IsButtonUntriggered(UI.BTN_INVENTORY_DROP)))
				{
					Inventory[SelectedItem] = item;
					ui.mouseItem.Init();
				}
				NetMessage.CreateMessage2(21, ui.MyPlayer, num);
				NetMessage.SendMessage();
			}
		}

		public void AddBuff(int type, int time, bool quiet = true)
		{
			if (!quiet)
			{
				NetMessage.CreateMessage3(55, WhoAmI, type, time);
				NetMessage.SendMessage();
			}
			for (int i = 0; i < MaxNumBuffs; i++)
			{
				if (buff[i].Type == type)
				{
					if (buff[i].Time < time)
					{
						buff[i].Time = (ushort)time;
					}
					return;
				}
			}
			while (true)
			{
				int num = -1;
				for (int j = 0; j < MaxNumBuffs; j++)
				{
					if (!buff[j].IsDebuff())
					{
						num = j;
						break;
					}
				}
				if (num == -1)
				{
					break;
				}
				for (int k = num; k < MaxNumBuffs; k++)
				{
					if (buff[k].Type == 0)
					{
						buff[k].Type = (ushort)type;
						buff[k].Time = (ushort)time;
						return;
					}
				}
				DelBuff(num);
			}
		}

		public void DelBuff(Buff.ID id)
		{
			for (int i = 0; i < MaxNumBuffs; i++)
			{
				if ((Buff.ID)buff[i].Type == id)
				{
					DelBuff(i);
					break;
				}
			}
		}

		public int DelBuff(int b)
		{
			if (buff[b].Type == (int)Buff.ID.PET)
			{
				pet = (sbyte)Pet.NONE;
			}
			buff[b].Type = 0;
			buff[b].Time = 0;
			int num = b + 1;
			for (int i = 0; i < MaxNumBuffs - 1; i++)
			{
				if (buff[i].Time == 0 || buff[i].Type == 0)
				{
					if (i < num)
					{
						num--;
					}
					for (int j = i + 1; j < MaxNumBuffs; j++)
					{
						ref Buff reference = ref buff[j - 1];
						reference = buff[j];
						buff[j].Time = 0;
						buff[j].Type = 0;
					}
				}
			}
			return num;
		}

		public bool canUseMana()
		{
			return statMana < statManaMax;
		}

		public bool canHeal()
		{
			return statLife < StatLifeMax;
		}

		public void QuickMana()
		{
			if (noItems || statMana == statManaMax2)
			{
				return;
			}
			for (int i = 0; i < MaxNumInventory; i++)
			{
				if (Inventory[i].Stack <= 0 || Inventory[i].Type <= 0 || Inventory[i].HealMana <= 0 || (potionDelay != 0 && Inventory[i].IsPotion))
				{
					continue;
				}
				Main.PlaySound(2, XYWH.X, XYWH.Y, Inventory[i].UseSound);
				if (Inventory[i].IsPotion)
				{
					potionDelay = potionDelayTime;
					AddBuff((int)Buff.ID.POTION_DELAY, potionDelay);
				}
				statLife += Inventory[i].HealLife;
				statMana += Inventory[i].HealMana;
				if (statLife > StatLifeMax)
				{
					statLife = StatLifeMax;
				}
				if (statMana > statManaMax2)
				{
					statMana = statManaMax2;
				}
				if (isLocal())
				{
					if (Inventory[i].HealLife > 0)
					{
						HealEffect(Inventory[i].HealLife);
					}
					if (Inventory[i].HealMana > 0)
					{
						ManaEffect(Inventory[i].HealMana);
					}
				}
				if (--Inventory[i].Stack <= 0)
				{
					Inventory[i].Init();
				}
				break;
			}
		}

		public void ApplyProjectileBuffPvP(int type)
		{
			switch (type)
			{
			case 2:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case 15:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 300, quiet: false);
				}
				break;
			case 19:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case 33:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 420, quiet: false);
				}
				break;
			case 34:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 240, quiet: false);
				}
				break;
			case 35:
				if (Main.Rand.Next(4) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case 54:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 600, quiet: false);
				}
				break;
			case 63:
				if (Main.Rand.Next(3) != 0)
				{
					AddBuff((int)Buff.ID.CONFUSED, 120);
				}
				break;
			case 85:
				AddBuff((int)Buff.ID.ON_FIRE, 1200, quiet: false);
				break;
			case 95:
			case 103:
			case 104:
				AddBuff((int)Buff.ID.ON_FIRE_2, 420);
				break;
			}
		}

		public void ApplyProjectileBuff(int type)
		{
			switch (type)
			{
			case 55:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 600);
				}
				break;
			case 44:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.BLIND, 900);
				}
				break;
			case 82:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 420);
				}
				break;
			case 96:
			case 101:
				if (Main.Rand.Next(3) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE_2, 480);
				}
				break;
			case 98:
				AddBuff((int)Buff.ID.POISONED, 600);
				break;
			}
		}

		public void ApplyWeaponBuffPvP(int type)
		{
			switch (type)
			{
			case (int)Item.ID.FIERY_GREATSWORD:
				if (Main.Rand.Next(2) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case (int)Item.ID.MOLTEN_PICKAXE:
				if (Main.Rand.Next(10) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case (int)Item.ID.BLADE_OF_GRASS:
			case (int)Item.ID.TONBOGIRI:
				if (Main.Rand.Next(4) == 0)
				{
					AddBuff((int)Buff.ID.POISONED, 420, quiet: false);
				}
				break;
			case (int)Item.ID.MOLTEN_HAMAXE:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.ON_FIRE, 180, quiet: false);
				}
				break;
			case (int)Item.ID.TIZONA:
				if (Main.Rand.Next(5) == 0)
				{
					AddBuff((int)Buff.ID.BLEED, 600, quiet: false);
				}
				break;
			}
		}

		private unsafe void FireEffect(int particleType)
		{
			buffB *= 0.6f;
			buffG *= 0.7f;
			if (Main.Rand.Next(4) == 0)
			{
				Dust* ptr = Main.DustSet.NewDust(XYWH.X - 2, XYWH.Y - 2, 24, 46, particleType, velocity.X * 0.4f, velocity.Y * 0.4f, 100, default(Color), 3.0);
				if (ptr != null)
				{
					ptr->NoGravity = true;
					ptr->Velocity.X *= 1.8f;
					ptr->Velocity.Y *= 1.8f;
					ptr->Velocity.Y -= 0.5f;
				}
			}
		}

		private void Dead()
		{
			wings = 0;
			poisoned = false;
			onFire = false;
			onFire2 = false;
			blind = false;
			gravDir = 1;
			for (int i = 0; i < MaxNumBuffs; i++)
			{
				buff[i].Time = 0;
				buff[i].Type = 0;
			}
			if (isLocal() && !ui.editSign)
			{
				sign = -1;
			}
			if (isLocal() && sign < 0)
			{
				ui.npcChatText = null;
			}
			grappling[0] = -1;
			grappling[1] = -1;
			grappling[2] = -1;
			TalkNPC = -1;
			statLife = 0;
			channel = false;
			potionDelay = 0;
			PlayerChest = -1;
			itemAnimation = 0;
			immuneAlpha += 2;
			if (immuneAlpha > 255)
			{
				immuneAlpha = 255;
			}
			headPosition += headVelocity;
			bodyPosition += bodyVelocity;
			legPosition += legVelocity;
			headRotation += headVelocity.X * 0.1f;
			bodyRotation += bodyVelocity.X * 0.1f;
			legRotation += legVelocity.X * 0.1f;
			headVelocity.Y += 0.1f;
			bodyVelocity.Y += 0.1f;
			legVelocity.Y += 0.1f;
			headVelocity.X *= 0.99f;
			bodyVelocity.X *= 0.99f;
			legVelocity.X *= 0.99f;
			// Quite the difference between versions here; the PC version does not check for a button press, but the console version does.
			// This leads to console players getting to respawn whenever they choose until the death timer runs out, which is also reduced compared to PC.
			// This means that if killed by a boss and the player instantly respawns nearby, only the Skeletron-types will properly despawn.
			if (!isLocal() || (--respawnTimer > 0 && !ui.IsButtonTriggered(UI.BTN_RESPAWN)))
			{
				return;
			}
			ui.ClearButtonTriggers();
			if (difficulty == (int)Difficulty.HARDCORE)
			{
				ghost = true;
				return;
			}
			if (ui.mouseItem.Type > 0)
			{
				ui.OpenInventory();
			}
			Spawn();
		}

		public void Ghost()
		{
			hellAndBackState = 0;
			immune = false;
			immuneAlpha = 0;
			controlUp = false;
			controlLeft = false;
			IsControlDown = false;
			controlRight = false;
			controlJump = false;
			if (Main.HasFocus && ui.CurMenuType == MenuType.NONE && sign < 0)
			{
				if (ui.PadState.ThumbSticks.Left.Y < -UI.GpDeadZone)
				{
					IsControlDown = true;
				}
				else if (ui.PadState.ThumbSticks.Left.Y > UI.GpDeadZone)
				{
					controlUp = true;
				}
				if (ui.PadState.ThumbSticks.Left.X < -UI.GpDeadZone)
				{
					controlLeft = true;
				}
				else if (ui.PadState.ThumbSticks.Left.X > UI.GpDeadZone)
				{
					controlRight = true;
				}
				if (ui.PadState.IsButtonDown(UI.BTN_JUMP) || ui.PadState.IsButtonDown(ui.BTN_JUMP2))
				{
					controlJump = true;
				}
			}
			if (controlUp || controlJump)
			{
				if (velocity.Y > 0f)
				{
					velocity.Y *= 0.9f;
				}
				velocity.Y -= 0.1f;
				if (velocity.Y < -3f)
				{
					velocity.Y = -3f;
				}
			}
			else if (IsControlDown)
			{
				if (velocity.Y < 0f)
				{
					velocity.Y *= 0.9f;
				}
				velocity.Y += 0.1f;
				if (velocity.Y > 3f)
				{
					velocity.Y = 3f;
				}
			}
			else if (velocity.Y < -0.1 || velocity.Y > 0.1)
			{
				velocity.Y *= 0.9f;
			}
			else
			{
				velocity.Y = 0f;
			}
			if (controlLeft && !controlRight)
			{
				if (velocity.X > 0f)
				{
					velocity.X *= 0.9f;
				}
				velocity.X -= 0.1f;
				if (velocity.X < -3f)
				{
					velocity.X = -3f;
				}
			}
			else if (controlRight && !controlLeft)
			{
				if (velocity.X < 0f)
				{
					velocity.X *= 0.9f;
				}
				velocity.X += 0.1f;
				if (velocity.X > 3f)
				{
					velocity.X = 3f;
				}
			}
			else if (velocity.X < -0.1f || velocity.X > 0.1f)
			{
				velocity.X *= 0.9f;
			}
			else
			{
				velocity.X = 0f;
			}
			Position.X += velocity.X;
			Position.Y += velocity.Y;
			if (velocity.X < 0f)
			{
				direction = -1;
			}
			else if (velocity.X > 0f)
			{
				direction = 1;
			}
			ghostFrameCounter++;
			if (Position.X < (Main.LeftWorld + (float)(Lighting.OffScreenTiles * 16) + 16))
			{
				Position.X = Main.LeftWorld + (float)(Lighting.OffScreenTiles * 16) + 16;
				velocity.X = 0f;
			}
			else if (Position.X + 20f > Main.RightWorld - (Lighting.OffScreenTiles * 16) - 32)
			{
				Position.X = Main.RightWorld - (Lighting.OffScreenTiles * 16) - 32 - width;
				velocity.X = 0f;
			}
			if (Position.Y < (Main.TopWorld + (float)(Lighting.OffScreenTiles * 16) + 16))
			{
				Position.Y = Main.TopWorld + (float)(Lighting.OffScreenTiles * 16) + 16;
				if (velocity.Y < -0.1)
				{
					velocity.Y = -0.1f;
				}
			}
			else if (Position.Y > (Main.BottomWorld - (Lighting.OffScreenTiles * 16) - 32 - height))
			{
				Position.Y = Main.BottomWorld - (Lighting.OffScreenTiles * 16) - 32 - height;
				velocity.Y = 0f;
			}
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
		}

		private void UpdateTileInteractionLocation()
		{
			tileInteractX = 0;
			tileInteractY = 0;
			if (ui.UsingSmartCursor)
			{
				int num = XYWH.X;
				int num2 = XYWH.Y;
				int num3 = 0;
				sbyte[] array = ((direction > 0) ? TARGET_SEARCH_DIR_RIGHT : TARGET_SEARCH_DIR_LEFT);
				bool flag;
				do
				{
					num += array[num3++];
					num2 += array[num3++];
					flag = CanInteractWithTile(num, num2);
				}
				while (!flag && num3 < TARGET_SEARCH_DIR_RIGHT.Length);
				if (flag)
				{
					tileInteractX = (short)(num >> 4);
					tileInteractY = (short)(num2 >> 4);
				}
			}
			else if (CanInteractWithTile(tileTargetX << 4, tileTargetY << 4))
			{
				tileInteractX = tileTargetX;
				tileInteractY = tileTargetY;
			}
		}

		public bool CanInteractWithNPC()
		{
			switch (Main.TileSet[tileInteractX, tileInteractY].Type)
			{
			case 10:
			case 11:
				return false;
			default:
				tileInteractX = 0;
				tileInteractY = 0;
				return true;
			}
		}

		public unsafe void UpdatePlayer(int i)
		{
			float num = 10f;
			float num2 = 0.4f;
			int JumpHeight = 15;
			float JumpSpeed = 5.01f;
			if (IsWet)
			{
				if (merman)
				{
					num2 = 0.3f;
					num = 7f;
				}
				else
				{
					num2 = 0.2f;
					num = 5f;
					JumpHeight = 30;
					JumpSpeed = 6.01f;
				}
			}
			float num5 = 3f;
			float num6 = 0.08f;
			float num7 = num5;
			heldProj = -1;
			float num8 = Main.MaxTilesX / 4200f;
			num8 *= num8;
			float num9 = (Position.Y * 0.0625f - (60f + 10f * num8)) / (Main.WorldSurface / 6);
			if ((double)num9 < 0.25)
			{
				num9 = 0.25f;
			}
			else if (num9 > 1f)
			{
				num9 = 1f;
			}
			num2 *= num9;
			if (statManaMax2 > 0)
			{
				maxRegenDelay = (short)((int)((1f - statMana / (float)statManaMax2) * 60f * 4f) + 45);
			}
			if (++shadowCount == 1)
			{
				ref Vector2 reference = ref shadowPos[2];
				reference = shadowPos[1];
			}
			else if (shadowCount == 2)
			{
				ref Vector2 reference2 = ref shadowPos[1];
				reference2 = shadowPos[0];
			}
			else
			{
				shadowCount = 0;
				ref Vector2 reference3 = ref shadowPos[0];
				reference3 = Position;
			}
			if (potionDelay > 0)
			{
				potionDelay--;
			}
			if (runSoundDelay > 0)
			{
				runSoundDelay--;
			}
			if (itemAnimation == 0)
			{
				attackCD = 0;
			}
			else if (attackCD > 0)
			{
				attackCD--;
			}
			if (isLocal())
			{
				UI.CurrentUI = ui;
				ZoneEvil = CurrentView.EvilTiles >= 200;
				zoneHoly = CurrentView.HolyTiles >= 100;
				ZoneMeteor = CurrentView.MeteorTiles >= 50;
				ZoneDungeon = false;
				if (CurrentView.DungeonTiles >= 250 && Position.Y > Main.WorldSurfacePixels)
				{
					int num10 = XYWH.X >> 4;
					int num11 = XYWH.Y >> 4;
					int wall = Main.TileSet[num10, num11].WallType;
					if (wall > 0 && !Main.WallHouse[wall])
					{
						ZoneDungeon = true;
					}
				}
				ZoneJungle = CurrentView.JungleTiles >= 80;
			}
			if (ghost)
			{
				Ghost();
			}
			else if (IsDead)
			{
				Dead();
			}
			else
			{
				if (isLocal())
				{
#if !USE_ORIGINAL_CODE
					OldVelocity.X = velocity.X;
					OldVelocity.Y = velocity.Y;
#endif
					controlUp = false;
					controlLeft = false;
					IsControlDown = false;
					controlRight = false;
					bool flag = !controlJump;
					controlJump = false;
					controlUseItem = false;
					bool flag2 = !controlUseTile;
					controlUseTile = false;
					controlThrow = false;
					controlInv = false;
					controlHook = false;
					if (Main.HasFocus && ui.CurMenuType == MenuType.NONE)
					{
						controlInv = ((ui.InventoryMode > 0) ? ui.IsButtonTriggered(UI.BTN_INVENTORY_CLOSE) : ui.IsButtonUntriggered(UI.BTN_INVENTORY_OPEN));
						if (controlInv)
						{
							toggleInv();
						}
						if (ui.InventoryMode == 0)
						{
							if (sign < 0 && TalkNPC < 0)
							{
								if (ui.PadState.ThumbSticks.Left.Y < -UI.LEFT_STICK_VERTICAL_THRESHOLD)
								{
									IsControlDown = true;
								}
								else if (ui.PadState.ThumbSticks.Left.Y > UI.LEFT_STICK_VERTICAL_THRESHOLD)
								{
									controlUp = true;
								}
								if (ui.PadState.ThumbSticks.Left.X < -UI.GpDeadZone)
								{
									controlLeft = true;
								}
								else if (ui.PadState.ThumbSticks.Left.X > UI.GpDeadZone)
								{
									controlRight = true;
								}
								if (ui.PadState.IsButtonDown(ui.BTN_GRAPPLE))
								{
									controlHook = true;
								}
								if (ui.PadState.IsButtonDown(UI.BTN_USE))
								{
									controlUseItem = true;
								}
								else if (itemTime == 0 && itemAnimation == 0)
								{
									if (ui.IsButtonTriggered(UI.BTN_PREV_ITEM))
									{
										ui.hotbarItemNameTime = UI.HOTBAR_ITEMNAME_DISPLAYTIME;
										if (oldSelectedItem >= 0)
										{
											SelectedItem = oldSelectedItem;
											oldSelectedItem = -1;
										}
										if (--SelectedItem < 0)
										{
											SelectedItem += 10;
										}
										Main.PlaySound(12);
									}
									else if (ui.IsButtonTriggered(UI.BTN_NEXT_ITEM))
									{
										ui.hotbarItemNameTime = UI.HOTBAR_ITEMNAME_DISPLAYTIME;
										if (oldSelectedItem >= 0)
										{
											SelectedItem = oldSelectedItem;
											oldSelectedItem = -1;
										}
										if (++SelectedItem >= 10)
										{
											SelectedItem -= 10;
										}
										Main.PlaySound(12);
									}
									else
									{
										int num12 = ui.UpdateQuickAccess();
										if (num12 >= 0)
										{
											if ((num12 > 9 || Inventory[num12].IsPotion) && oldSelectedItem < 0)
											{
												oldSelectedItem = SelectedItem;
											}
											SelectedItem = (sbyte)num12;
											if (num12 >= 0)
											{
												ui.hotbarItemNameTime = UI.HOTBAR_ITEMNAME_DISPLAYTIME;
												ui.quickAccessDisplayTime = UI.QUICK_ACCESS_DISPLAYTIME;
												if (Inventory[num12].IsPotion)
												{
													controlUseItem = true;
												}
											}
										}
										else if (oldSelectedItem >= 0 && (Inventory[SelectedItem].Type == 0 || Inventory[SelectedItem].IsPotion))
										{
											SelectedItem = oldSelectedItem;
											oldSelectedItem = -1;
										}
									}
								}
								controlThrow = ui.IsButtonTriggered(UI.BTN_DROP);
								if (ui.IsJumpButtonDown())
								{
									controlJump = !flag || ui.WasJumpButtonUp();
								}
								if (ui.PadState.IsButtonDown(UI.BTN_INTERACT))
								{
									controlUseTile = !flag2 || ui.gpPrevState.IsButtonUp(UI.BTN_INTERACT);
								}
							}
							else if (sign != -1 || ui.npcChatText != null)
							{
								ui.UpdateNpcChat();
							}
							if (confused)
							{
								bool flag3 = controlLeft;
								controlLeft = controlRight;
								controlRight = flag3;
								flag3 = controlUp;
								controlUp = controlRight;
								IsControlDown = flag3;
							}
							if (PlayerChest != -1)
							{
								int num13 = XYWH.X + (width / 2) >> 4;
								int num14 = XYWH.Y + (height / 2) >> 4;
								if (num13 < chestX - 5 || num13 > chestX + 6 || num14 < chestY - 4 || num14 > chestY + 5 || Main.TileSet[chestX, chestY].IsActive == 0)
								{
									Main.PlaySound(11);
									PlayerChest = -1;
								}
							}
						}
						if (delayUseItem)
						{
							delayUseItem = controlUseItem;
							controlUseItem = false;
						}
						if (itemAnimation == 0 && itemTime == 0)
						{
							dropItemCheck();
						}
					}
					if (Main.NetMode >= 1)
					{
						NetPlayer netPlayer = ui.netPlayer;
						bool flag4 = false;
						if (statLife != netPlayer.statLife || StatLifeMax != netPlayer.statLifeMax)
						{
							netPlayer.statLife = statLife;
							netPlayer.statLifeMax = StatLifeMax;
							NetMessage.CreateMessage1(16, i);
							NetMessage.SendMessage();
							flag4 = true;
						}
						if (statMana != netPlayer.statMana || statManaMax != netPlayer.statManaMax)
						{
							netPlayer.statMana = statMana;
							netPlayer.statManaMax = statManaMax;
							NetMessage.CreateMessage1(42, i);
							NetMessage.SendMessage();
							flag4 = true;
						}
						if (controlUp != netPlayer.controlUp)
						{
							netPlayer.controlUp = controlUp;
							flag4 = true;
						}
						if (IsControlDown != netPlayer.controlDown)
						{
							netPlayer.controlDown = IsControlDown;
							flag4 = true;
						}
						if (controlLeft != netPlayer.controlLeft)
						{
							netPlayer.controlLeft = controlLeft;
							flag4 = true;
						}
						if (controlRight != netPlayer.controlRight)
						{
							netPlayer.controlRight = controlRight;
							flag4 = true;
						}
						if (controlJump != netPlayer.controlJump)
						{
							netPlayer.controlJump = controlJump;
							flag4 = true;
						}
						if (controlUseItem != netPlayer.controlUseItem)
						{
							netPlayer.controlUseItem = controlUseItem;
							flag4 = true;
						}
						if (SelectedItem != netPlayer.selectedItem)
						{
							netPlayer.selectedItem = SelectedItem;
							flag4 = true;
						}
#if (VERSION_INITIAL && !IS_PATCHED)
						if (flag4)
						{
#else
                        if (flag4 || --netForceControlUpdate <= 0)
                        {
                            netForceControlUpdate = NetForceUpdateTimer; // This number is not changed in future versions.
#endif
							NetMessage.CreateMessage1(13, i);
							NetMessage.SendMessage();
						}
					}
					if (velocity.Y == 0f)
					{
						if (!noFallDmg && wings == 0)
						{
							int num15 = (XYWH.Y >> 4) - fallStart;
							int num16 = num15 * gravDir - 25;
							if (num16 > 0)
							{
								immune = false;
								Hurt(num16 * 10, 0, pvp: false, quiet: false, Lang.DeathMsgPtr(-1, 0, 0, 0));
							}
						}
						fallStart = (short)(XYWH.Y >> 4);
					}
					else if (jump > 0 || rocketDelay > 0 || IsWet || slowFall || (double)num9 < 0.8 || tongued)
					{
#if !USE_ORIGINAL_CODE
						if (ui != null)
						{
							if (ui.TriggerCheckEnabled(Trigger.SafeFall))
							{
								int FallDamage = ((XYWH.Y >> 4) - fallStart) * gravDir * 10 - 250;
								if (statLife <= Main.CalculateDamage(FallDamage, StatDefense))
								{
									ui.SetTriggerState(Trigger.SafeFall);
								}
							}
						}
#endif
						fallStart = (short)(XYWH.Y >> 4);
					}
					if (ui.InventoryMode > 0)
					{
						delayUseItem = true;
					}
					tileTargetX = (short)(ui.MouseX + CurrentView.ScreenPosition.X >> 4);
					tileTargetY = (short)(ui.MouseY + CurrentView.ScreenPosition.Y >> 4);
					UpdateTileInteractionLocation();
#if !USE_ORIGINAL_CODE
					if ((XYWH.Y < Main.WorldSurfacePixels >> 1) && 98 < CurrentView.SkyTiles)
					{
						ui.SetTriggerState(Trigger.CouldThisBeHeaven);
					}
#endif
				}
				if (immune)
				{
					if (--immuneTime <= 0)
					{
						immune = false;
					}
					immuneAlpha = (short)(immuneAlpha + immuneAlphaDirection * 50);
					if (immuneAlpha <= 50)
					{
						immuneAlphaDirection = 1;
					}
					else if (immuneAlpha >= 205)
					{
						immuneAlphaDirection = -1;
					}
				}
				else
				{
					immuneAlpha = 0;
				}
				potionDelayTime = (ushort)Item.PotionDelay;
				StatDefense = 0;
				accWatch = 0;
				accCompass = false;
				accDepthMeter = false;
				accDivingHelm = false;
				lifeRegen = 0;
				manaCost = 1f;
				meleeSpeed = 1f;
				meleeDamage = 1f;
				rangedDamage = 1f;
				magicDamage = 1f;
				moveSpeed = 1f;
				boneArmor = false;
				rocketBoots = 0;
				fireWalk = false;
				noKnockback = false;
				jumpBoost = false;
				noFallDmg = false;
				accFlipper = false;
				spawnMax = false;
				spaceGun = false;
				killGuide = false;
				lavaImmune = false;
				gills = false;
				slowFall = false;
				findTreasure = false;
				invis = false;
				NightVision = false;
				enemySpawns = false;
				thorns = false;
				waterWalk = false;
				detectCreature = false;
				gravControl = false;
				statManaMax2 = statManaMax;
				freeAmmoChance = 0;
				manaRegenBuff = false;
				meleeCrit = 4;
				rangedCrit = 4;
				magicCrit = 4;
				lightOrb = false;
				fairy = false;
				archery = false;
				poisoned = false;
				blind = false;
				onFire = false;
				onFire2 = false;
				noItems = false;
				blockRange = 0;
				pickSpeed = 1f;
				wereWolf = false;
				rulerAcc = false;
				bleed = false;
				confused = false;
				wings = 0;
				brokenArmor = false;
				silence = false;
				slow = false;
				IsHorrified = false;
				tongued = false;
				kbGlove = false;
				starCloak = false;
				longInvince = false;
				manaFlower = false;
				short crit = Inventory[SelectedItem].Crit;
				meleeCrit += crit;
				magicCrit += crit;
				rangedCrit += crit;
				buffR = 1f;
				buffG = 1f;
				buffB = 1f;
				int num17 = 0;
				for (int j = 0; j < MaxNumBuffs; j++)
				{
					if (buff[j].Type <= 0 || buff[j].Time <= 0)
					{
						continue;
					}
					if (isLocal() && buff[j].Type != (int)Buff.ID.WEREWOLF)
					{
						buff[j].Time--;
						if (!buff[j].IsDebuff() && ++num17 == 5)
						{
							ui.SetTriggerState(Trigger.Has5Buffs);
						}
					}
					switch ((Buff.ID)buff[j].Type)
					{
					case Buff.ID.LAVA_IMMUNE:
						lavaImmune = true;
						fireWalk = true;
						break;
					case Buff.ID.LIFE_REGEN:
						lifeRegen += 2;
						break;
					case Buff.ID.HASTE:
						moveSpeed += 0.25f;
						break;
					case Buff.ID.GILLS:
						gills = true;
						break;
					case Buff.ID.IRONSKIN:
						StatDefense += 8;
						break;
					case Buff.ID.MANA_REGEN:
						manaRegenBuff = true;
						break;
					case Buff.ID.MAGIC_POWER:
						magicDamage += 0.2f;
						break;
					case Buff.ID.SLOWFALL:
						slowFall = true;
						break;
					case Buff.ID.FIND_TREASURE:
						findTreasure = true;
						break;
					case Buff.ID.INVISIBLE:
						invis = true;
						break;
					case Buff.ID.SHINE:
						Lighting.AddLight(XYWH.X + (width / 2) >> 4, XYWH.Y + (height / 2) >> 4, new Vector3(0.8f, 0.95f, 1f));
						break;
					case Buff.ID.NIGHTVISION:
						NightVision = true;
						break;
					case Buff.ID.ENEMY_SPAWNS:
						enemySpawns = true;
						break;
					case Buff.ID.THORNS:
						thorns = true;
						break;
					case Buff.ID.WATER_WALK:
						waterWalk = true;
						break;
					case Buff.ID.RANGED_DAMAGE:
						archery = true;
						break;
					case Buff.ID.DETECT_CREATURE:
						detectCreature = true;
						break;
					case Buff.ID.GRAVITY_CONTROL:
						gravControl = true;
						break;
					case Buff.ID.LIGHT_ORB:
					{
						lightOrb = true;
						bool flag5 = true;
						for (int k = 0; k < Projectile.MaxNumProjs; k++)
						{
							if (Main.ProjectileSet[k].type == 18 && Main.ProjectileSet[k].active != 0 && Main.ProjectileSet[k].owner == WhoAmI)
							{
								flag5 = false;
								break;
							}
						}
						if (flag5)
						{
							Projectile.NewProjectile(Position.X + (width / 2), Position.Y + (height / 2), 0f, 0f, 18, 0, 0f, WhoAmI);
						}
						break;
					}
					case Buff.ID.POISONED:
						poisoned = true;
						if (Main.Rand.Next(52) == 0)
						{
							Dust* ptr2 = Main.DustSet.NewDust(46, ref XYWH, 0.0, 0.0, 150, default, 0.2f);
							if (ptr2 != null)
							{
								ptr2->NoGravity = true;
								ptr2->FadeIn = 1.9f;
							}
						}
						buffR *= 0.65f;
						buffB *= 0.75f;
						break;
					case Buff.ID.POTION_DELAY:
						potionDelay = buff[j].Time;
						break;
					case Buff.ID.BLIND:
						blind = true;
						buffG *= 0.65f;
						buffR *= 0.7f;
						break;
					case Buff.ID.NO_ITEMS:
						noItems = true;
						buffG *= 0.8f;
						buffR *= 0.65f;
						break;
					case Buff.ID.ON_FIRE:
						onFire = true;
						FireEffect(6);
						break;
					case Buff.ID.DRUNK:
						StatDefense -= 4;
						meleeCrit += 2;
						meleeDamage += 0.1f;
						meleeSpeed += 0.1f;
						break;
					case Buff.ID.WELL_FED:
						StatDefense++;
						meleeCrit++;
						meleeDamage += 0.05f;
						meleeSpeed += 0.05f;
						magicCrit++;
						magicDamage += 0.05f;
						rangedCrit++;
						magicDamage += 0.05f;
						moveSpeed += 0.1f;
						break;
					case Buff.ID.FAIRY:
					{
						fairy = true;
						bool flag6 = true;
						for (int l = 0; l < Projectile.MaxNumProjs; l++)
						{
							if (Main.ProjectileSet[l].active != 0 && Main.ProjectileSet[l].owner == WhoAmI && (Main.ProjectileSet[l].type == 72 || Main.ProjectileSet[l].type == 86 || Main.ProjectileSet[l].type == 87))
							{
								flag6 = false;
								break;
							}
						}
						if (flag6)
						{
							int num18 = Main.Rand.Next(3);
							switch (num18)
							{
							case 0:
								num18 = 72;
								break;
							case 1:
								num18 = 86;
								break;
							case 2:
								num18 = 87;
								break;
							}
							Projectile.NewProjectile(Position.X + (width / 2), Position.Y + (height / 2), 0f, 0f, num18, 0, 0f, WhoAmI);
						}
						break;
					}
					case Buff.ID.WEREWOLF:
						if (wolfAcc && !merman && !Main.GameTime.DayTime && Main.GameTime.MoonPhase == 0)
						{
							wereWolf = true;
							meleeCrit++;
							meleeDamage += 0.051f;
							meleeSpeed += 0.051f;
							StatDefense++;
							moveSpeed += 0.05f;
						}
						else
						{
							j = DelBuff(j);
						}
						break;
					case Buff.ID.CLARAVOYANCE:
						magicCrit += 2;
						magicDamage += 0.05f;
						statManaMax2 += 20;
						manaCost -= 0.02f;
						break;
					case Buff.ID.BLEED:
						bleed = true;
						if (!IsDead && Main.Rand.Next(32) == 0)
						{
							Dust* ptr = Main.DustSet.NewDust(5, ref XYWH);
							if (ptr != null)
							{
								ptr->Velocity.X *= 0.25f;
								ptr->Velocity.Y += 0.5f;
								ptr->Velocity.Y *= 0.25f;
							}
						}
						buffG *= 0.9f;
						buffB *= 0.9f;
						break;
					case Buff.ID.CONFUSED:
						confused = true;
						break;
					case Buff.ID.SLOW:
						slow = true;
						break;
					case Buff.ID.WEAK:
						meleeDamage -= 0.051f;
						meleeSpeed -= 0.051f;
						StatDefense -= 4;
						moveSpeed -= 0.1f;
						break;
					case Buff.ID.SILENCE:
						silence = true;
						break;
					case Buff.ID.BROKEN_ARMOR:
						brokenArmor = true;
						break;
					case Buff.ID.HORRIFIED:
						if (NPC.WoF >= 0 && Main.NPCSet[NPC.WoF].Type == (int)NPC.ID.WALL_OF_FLESH)
						{
							IsHorrified = true;
							buff[j].Time = 10;
						}
						else
						{
							j = DelBuff(j);
						}
						break;
					case Buff.ID.TONGUED:
						buff[j].Time = 10;
						tongued = true;
						break;
					case Buff.ID.ON_FIRE_2:
						onFire2 = true;
						FireEffect(75);
						break;
					case Buff.ID.PET:
						if (pet >= 0)
						{
							buff[j].Time = 18000;
							SpawnPet();
						}
						else
						{
							buff[j].Time = 0;
						}
						break;
					}
				}
				if (accMerman && IsWet && !lavaWet)
				{
					releaseJump = true;
					wings = 0;
					merman = true;
					accFlipper = true;
					AddBuff((int)Buff.ID.MERFOLK, 2);
				}
				else
				{
					merman = false;
				}
				accMerman = false;
				if (wolfAcc && !merman && !wereWolf && !Main.GameTime.DayTime && Main.GameTime.MoonPhase == 0)
				{
					AddBuff((int)Buff.ID.WEREWOLF, 60);
				}
				wolfAcc = false;
				if (isLocal())
				{
					for (int m = 0; m < MaxNumBuffs; m++)
					{
						if (buff[m].Type > 0 && buff[m].Time == 0)
						{
							m = DelBuff(m);
						}
					}
				}
				doubleJump = false;
				for (int n = 0; n < MaxNumArmor - 3; n++)
				{
					StatDefense += armor[n].Defense;
					lifeRegen += armor[n].LifeRegen;
					switch ((Item.ID)armor[n].Type)
					{
					case Item.ID.METEOR_HELMET:
					case Item.ID.METEOR_SUIT:
					case Item.ID.METEOR_LEGGINGS:
						magicDamage += 0.05f;
						break;
					case Item.ID.NECRO_HELMET:
					case Item.ID.NECRO_BREASTPLATE:
					case Item.ID.NECRO_GREAVES:
						rangedDamage += 0.05f;
						break;
					case Item.ID.WIZARD_HAT:
						magicDamage += 0.15f;
						break;
					case Item.ID.BAND_OF_STARPOWER:
						statManaMax2 += 20;
						break;
					case Item.ID.JUNGLE_HAT:
					case Item.ID.JUNGLE_SHIRT:
					case Item.ID.JUNGLE_PANTS:
						magicCrit += 3;
						statManaMax2 += 20;
						break;
					case Item.ID.SHADOW_GREAVES:
					case Item.ID.SHADOW_SCALEMAIL:
					case Item.ID.SHADOW_HELMET:
						meleeSpeed += 0.07f;
						break;
					case Item.ID.COBALT_HAT:
						magicCrit += 9;
						statManaMax2 += 40;
						break;
					case Item.ID.COBALT_HELMET:
						moveSpeed += 0.07f;
						meleeSpeed += 0.12f;
						break;
					case Item.ID.COBALT_MASK:
						rangedDamage += 0.1f;
						rangedCrit += 6;
						break;
					case Item.ID.COBALT_BREASTPLATE:
						magicCrit += 3;
						meleeCrit += 3;
						rangedCrit += 3;
						break;
					case Item.ID.COBALT_LEGGINGS:
						moveSpeed += 0.1f;
						break;
					case Item.ID.MYTHRIL_HOOD:
						magicDamage += 0.15f;
						statManaMax2 += 60;
						break;
					case Item.ID.MYTHRIL_HELMET:
						meleeCrit += 5;
						meleeDamage += 0.1f;
						break;
					case Item.ID.MYTHRIL_HAT:
						rangedDamage += 0.12f;
						rangedCrit += 7;
						break;
					case Item.ID.MYTHRIL_CHAINMAIL:
						rangedDamage += 0.05f;
						meleeDamage += 0.05f;
						magicDamage += 0.05f;
						break;
					case Item.ID.MYTHRIL_GREAVES:
						magicCrit += 3;
						meleeCrit += 3;
						rangedCrit += 3;
						break;
					case Item.ID.DIVING_HELMET:
						accDivingHelm = true;
						break;
					case Item.ID.ADAMANTITE_HEADGEAR:
						magicDamage += 0.11f;
						magicCrit += 11;
						statManaMax2 += 80;
						break;
					case Item.ID.ADAMANTITE_HELMET:
						meleeCrit += 7;
						meleeDamage += 0.14f;
						break;
					case Item.ID.ADAMANTITE_MASK:
						rangedDamage += 0.14f;
						rangedCrit += 8;
						break;
					case Item.ID.ADAMANTITE_BREASTPLATE:
						rangedDamage += 0.06f;
						meleeDamage += 0.06f;
						magicDamage += 0.06f;
						break;
					case Item.ID.ADAMANTITE_LEGGINGS:
						magicCrit += 4;
						meleeCrit += 4;
						rangedCrit += 4;
						moveSpeed += 0.05f;
						break;
					case Item.ID.HALLOWED_PLATE_MAIL:
						magicCrit += 7;
						meleeCrit += 7;
						rangedCrit += 7;
						break;
					case Item.ID.HALLOWED_GREAVES:
						rangedDamage += 0.07f;
						meleeDamage += 0.07f;
						magicDamage += 0.07f;
						moveSpeed += 0.08f;
						break;
					case Item.ID.HALLOWED_HELMET:
						rangedDamage += 0.15f;
						rangedCrit += 8;
						break;
					case Item.ID.HALLOWED_HEADGEAR:
						magicDamage += 0.12f;
						magicCrit += 12;
						statManaMax2 += 100;
						break;
					case Item.ID.HALLOWED_MASK:
						meleeCrit += 10;
						meleeDamage += 0.1f;
						meleeSpeed += 0.1f;
						break;
					case Item.ID.DRAGON_MASK:
						meleeCrit += 15;
						meleeDamage += 0.15f;
						meleeSpeed += 0.15f;
						break;
					case Item.ID.DRAGON_BREASTPLATE:
						meleeCrit += 5;
						meleeDamage += 0.05f;
						break;
					case Item.ID.DRAGON_GREAVES:
						moveSpeed += 0.12f;
						meleeSpeed += 0.02f;
						break;
					case Item.ID.TITAN_HELMET:
						rangedDamage += 0.15f;
						rangedCrit += 10;
						freeAmmoChance += 5;
						break;
					case Item.ID.TITAN_MAIL:
						rangedDamage += 0.05f;
						rangedCrit += 10;
						freeAmmoChance += 5;
						break;
					case Item.ID.TITAN_LEGGINGS:
						rangedDamage += 0.1f;
						moveSpeed += 0.1f;
						freeAmmoChance += 10;
						break;
					case Item.ID.SPECTRAL_HEADGEAR:
						magicDamage += 0.15f;
						magicCrit += 15;
						statManaMax2 += 120;
						break;
					case Item.ID.SPECTRAL_ARMOR:
						magicDamage += 0.05f;
						magicCrit += 10;
						manaCost -= 0.1f;
						break;
					case Item.ID.SPECTRAL_SUBLIGAR:
						magicDamage += 0.1f;
						moveSpeed += 0.1f;
						statManaMax2 += 30;
						break;
					}
					switch (armor[n].PrefixType)
					{
					case 62:
						StatDefense++;
						break;
					case 63:
						StatDefense += 2;
						break;
					case 64:
						StatDefense += 3;
						break;
					case 65:
						StatDefense += 4;
						break;
					case 66:
						statManaMax2 += 20;
						break;
					case 67:
						meleeCrit++;
						rangedCrit++;
						magicCrit++;
						break;
					case 68:
						meleeCrit += 2;
						rangedCrit += 2;
						magicCrit += 2;
						break;
					case 69:
						meleeDamage += 0.01f;
						rangedDamage += 0.01f;
						magicDamage += 0.01f;
						break;
					case 70:
						meleeDamage += 0.02f;
						rangedDamage += 0.02f;
						magicDamage += 0.02f;
						break;
					case 71:
						meleeDamage += 0.03f;
						rangedDamage += 0.03f;
						magicDamage += 0.03f;
						break;
					case 72:
						meleeDamage += 0.04f;
						rangedDamage += 0.04f;
						magicDamage += 0.04f;
						break;
					case 73:
						moveSpeed += 0.01f;
						break;
					case 74:
						moveSpeed += 0.02f;
						break;
					case 75:
						moveSpeed += 0.03f;
						break;
					case 76:
						moveSpeed += 0.04f;
						break;
					case 77:
						meleeSpeed += 0.01f;
						break;
					case 78:
						meleeSpeed += 0.02f;
						break;
					case 79:
						meleeSpeed += 0.03f;
						break;
					case 80:
						meleeSpeed += 0.04f;
						break;
					}
				}
				head = armor[0].HeadSlot;
				body = armor[1].BodySlot;
				legs = armor[2].LegSlot;
				for (int num19 = 3; num19 < MaxNumArmor - 3; num19++)
				{
					switch ((Item.ID)armor[num19].Type)
					{
						case Item.ID.COPPER_WATCH:
							if (accWatch < 1)
							{
								accWatch = 1;
							}
							break;
						case Item.ID.SILVER_WATCH:
							if (accWatch < 2)
							{
								accWatch = 2;
							}
							break;
						case Item.ID.GOLD_WATCH:
							accWatch = 3;
							break;
						case Item.ID.DEPTH_METER:
							accDepthMeter = true;
							break;
						case Item.ID.CLOUD_IN_A_BOTTLE:
							doubleJump = true;
							break;
						case Item.ID.HERMES_BOOTS:
							num7 = 6f;
							break;
						case Item.ID.ROCKET_BOOTS:
							rocketBoots = 1;
							break;
						case Item.ID.COBALT_SHIELD:
							noKnockback = true;
							break;
						case Item.ID.LUCKY_HORSESHOE:
							noFallDmg = true;
							break;
						case Item.ID.SHINY_RED_BALLOON:
							jumpBoost = true;
							break;
						case Item.ID.FLIPPER:
							accFlipper = true;
							break;
						case Item.ID.OBSIDIAN_SKULL:
							fireWalk = true;
							break;
						case Item.ID.FERAL_CLAWS:
							meleeSpeed += 0.12f;
							break;
						case Item.ID.ANKLET_OF_THE_WIND:
							moveSpeed += 0.1f;
							break;
						case Item.ID.NATURES_GIFT:
							manaCost -= 0.06f;
							break;
						case Item.ID.GUIDE_VOODOO_DOLL:
							killGuide = true;
							break;
						case Item.ID.AGLET:
							moveSpeed += 0.1f;
							break;
						case Item.ID.COMPASS:
							accCompass = true;
							break;
						case Item.ID.DIVING_GEAR:
							accFlipper = true;
							accDivingHelm = true;
							break;
						case Item.ID.GPS:
							accWatch = 3;
							accDepthMeter = true;
							accCompass = true;
							break;
						case Item.ID.OBSIDIAN_HORSESHOE:
							noFallDmg = true;
							fireWalk = true;
							break;
						case Item.ID.OBSIDIAN_SHIELD:
							noKnockback = true;
							fireWalk = true;
							break;
						case Item.ID.CLOUD_IN_A_BALLOON:
							jumpBoost = true;
							doubleJump = true;
							break;
						case Item.ID.SPECTRE_BOOTS:
							num7 = 6f;
							rocketBoots = 2;
							break;
						case Item.ID.TOOLBELT:
							blockRange = 1;
							break;
						case Item.ID.MOON_CHARM:
							wolfAcc = true;
							break;
						case Item.ID.RULER:
							rulerAcc = true;
							break;
						case Item.ID.SORCERER_EMBLEM:
							magicDamage += 0.15f;
							break;
						case Item.ID.WARRIOR_EMBLEM:
							meleeDamage += 0.15f;
							break;
						case Item.ID.RANGER_EMBLEM:
							rangedDamage += 0.15f;
							break;
						case Item.ID.DEMON_WINGS:
							wings = 1;
							break;
						case Item.ID.ANGEL_WINGS:
							wings = 2;
							break;
#if !VERSION_INITIAL
						case Item.ID.SPARKLY_WINGS:
							wings = 3;
							break;
#endif
						case Item.ID.NEPTUNES_SHELL:
							accMerman = true;
							break;
						case Item.ID.STAR_CLOAK:
							starCloak = true;
							break;
						case Item.ID.PHILOSOPHERS_STONE:
							potionDelayTime = (ushort)Item.PotionDelayPhilosopher;
							break;
						case Item.ID.TITAN_GLOVE:
							kbGlove = true;
							break;
						case Item.ID.CROSS_NECKLACE:
							longInvince = true;
							break;
						case Item.ID.MANA_FLOWER:
							manaFlower = true;
							manaCost -= 0.08f;
							break;
						case Item.ID.MUSIC_BOX:
							if (isLocal() && Main.Rand.Next(18000) == 0 && Main.CurrentMusic != Main.Music.NUM_SONGS)
							{
								armor[num19].SetDefaults((int)Main.SongToMusicBox[(uint)Main.CurrentMusic]);
							}
							break;
						case Item.ID.MUSIC_BOX_OVERWORLD_DAY:
						case Item.ID.MUSIC_BOX_EERIE:
						case Item.ID.MUSIC_BOX_NIGHT:
						case Item.ID.MUSIC_BOX_TITLE:
						case Item.ID.MUSIC_BOX_UNDERGROUND:
						case Item.ID.MUSIC_BOX_BOSS1:
						case Item.ID.MUSIC_BOX_JUNGLE:
						case Item.ID.MUSIC_BOX_CORRUPTION:
						case Item.ID.MUSIC_BOX_UNDERGROUND_CORRUPTION:
						case Item.ID.MUSIC_BOX_THE_HALLOW:
						case Item.ID.MUSIC_BOX_BOSS2:
						case Item.ID.MUSIC_BOX_UNDERGROUND_HALLOW:
						case Item.ID.MUSIC_BOX_BOSS3:
						case Item.ID.MUSIC_BOX_DESERT:
						case Item.ID.MUSIC_BOX_SPACE:
						case Item.ID.MUSIC_BOX_TUTORIAL:
						case Item.ID.MUSIC_BOX_BOSS4:
						case Item.ID.MUSIC_BOX_OCEAN:
						case Item.ID.MUSIC_BOX_SNOW:
							if (isLocal() && Main.MusicBox < 0)
							{
								if (armor[num19].Type < (int)Item.ID.MUSIC_BOX_DESERT)
								{
									Main.MusicBox = armor[num19].Type - (int)Item.ID.MUSIC_BOX_OVERWORLD_DAY;
								}
								else
								{
									Main.MusicBox = armor[num19].Type - (int)Item.ID.TIZONA;
								}
							}
							break;
					}
				}

#if !USE_ORIGINAL_CODE
				if (Main.AlwaysOnWatch)
				{
					accWatch = 3;
				}

				if (Main.AlwaysOnDepth)
				{
					accDepthMeter = true;
				}

				if (Main.AlwaysOnCompass)
				{
					accCompass = true;
				}
#endif

				Lighting.AddLight(XYWH.X + 10 + (direction << 3) >> 4, XYWH.Y + 2 >> 4, (head == 11) ? new Vector3(0.92f, 0.8f, 0.75f) : new Vector3(0.2f, 0.2f, 0.2f));
				// Another lesser known Old-Gen console addition is a very weak shine potion effect being permanently applied to the player, which the above line is responsible for.
				// Why? No clue, although in the PC version, where the user is often close enough to the screen, no light in a large area also obscures the player.
				// As the console version is meant to be played from a further distance, I'm guessing it was a design choice so you could still locate the player, even while in complete darkness.
				// Oddly enough, this did carry over to other 'Old-Gen' versions of the game, like the Mobile & 3DS version... where the user is even closer to the screen than on PC.
				
				setBonus = null;
				if ((head == 1 && body == 1 && legs == 1) || (head == 2 && body == 2 && legs == 2))
				{
					setBonus = Lang.SetBonus(0);
					StatDefense += 2;
				}
				else if ((head == 3 && body == 3 && legs == 3) || (head == 4 && body == 4 && legs == 4))
				{
					setBonus = Lang.SetBonus(1);
					StatDefense += 3;
				}
				else if (head == 5 && body == 5 && legs == 5)
				{
					setBonus = Lang.SetBonus(2);
					moveSpeed += 0.15f;
				}
				else if (head == 6 && body == 6 && legs == 6)
				{
					setBonus = Lang.SetBonus(3);
					spaceGun = true;
				}
				else if (head == 7 && body == 7 && legs == 7)
				{
					setBonus = Lang.SetBonus(4);
					freeAmmoChance += 20;
				}
				else if (head == 8 && body == 8 && legs == 8)
				{
					setBonus = Lang.SetBonus(5);
					manaCost -= 0.16f;
				}
				else if (head == 9 && body == 9 && legs == 9)
				{
					setBonus = Lang.SetBonus(6);
					meleeDamage += 0.17f;
				}
				else if (head == 11 && body == 20 && legs == 19)
				{
					setBonus = Lang.SetBonus(7);
					pickSpeed = 0.8f;
				}
				else if (body == 17 && legs == 16)
				{
					if (head == 29)
					{
						setBonus = Lang.SetBonus(8);
						manaCost -= 0.14f;
					}
					else if (head == 30)
					{
						setBonus = Lang.SetBonus(9);
						meleeSpeed += 0.15f;
					}
					else if (head == 31)
					{
						setBonus = Lang.SetBonus(10);
						freeAmmoChance += 20;
					}
				}
				else if (body == 18 && legs == 17)
				{
					if (head == 32)
					{
						setBonus = Lang.SetBonus(11);
						manaCost -= 0.17f;
					}
					else if (head == 33)
					{
						setBonus = Lang.SetBonus(12);
						meleeCrit += 5;
					}
					else if (head == 34)
					{
						setBonus = Lang.SetBonus(13);
						freeAmmoChance += 20;
					}
				}
				else if (body == 19 && legs == 18)
				{
					if (head == 35)
					{
						setBonus = Lang.SetBonus(14);
						manaCost -= 0.19f;
					}
					else if (head == 36)
					{
						setBonus = Lang.SetBonus(15);
						meleeSpeed += 0.18f;
						moveSpeed += 0.18f;
					}
					else if (head == 37)
					{
						setBonus = Lang.SetBonus(16);
						freeAmmoChance += 25;
					}
				}
				else if (body == 24 && legs == 23)
				{
					if (head == 42)
					{
						setBonus = Lang.SetBonus(17);
						manaCost -= 0.2f;
					}
					else if (head == 43)
					{
						setBonus = Lang.SetBonus(18);
						meleeSpeed += 0.19f;
						moveSpeed += 0.19f;
					}
					else if (head == 41)
					{
						setBonus = Lang.SetBonus(19);
						freeAmmoChance += 25;
					}
				}
				else if (head == 45 && body == 26 && legs == 25)
				{
					setBonus = Lang.SetBonus(21);
					meleeSpeed += 0.21f;
					moveSpeed += 0.21f;
				}
				else if (head == 46 && body == 27 && legs == 26)
				{
					setBonus = Lang.SetBonus(22);
					freeAmmoChance += 28;
				}
				else if (head == 47 && body == 28 && legs == 27)
				{
					setBonus = Lang.SetBonus(20);
					manaCost -= 0.23f;
				}
#if VERSION_101
				else if ((head == 50 && body == 31 && legs == 30) || (head == 51 && body == 32 && legs == 31))
				{
					setBonus = Lang.SetBonus(23);
					StatDefense++;
				}
#endif
				if (merman)
				{
					wings = 0;
				}
				if (meleeSpeed > 4f)
				{
					meleeSpeed = 4f;
				}
				if (moveSpeed > 1.4f)
				{
					moveSpeed = 1.4f;
				}
				if (slow)
				{
					moveSpeed *= 0.5f;
				}
				if (statManaMax2 > 400)
				{
					statManaMax2 = 400;
				}
				if (StatDefense < 0)
				{
					StatDefense = 0;
				}
				meleeSpeed = 1f / meleeSpeed;
				if (onFire || onFire2)
				{
					lifeRegenTime = 0;
					lifeRegen = -8;
				}
				else if (poisoned)
				{
					lifeRegenTime = 0;
					lifeRegen = -4;
				}
				else if (bleed)
				{
					lifeRegenTime = 0;
				}
				else
				{
					double num20 = 0.0;
					if (++lifeRegenTime >= 3600)
					{
						num20 = 9.0;
						lifeRegenTime = 3600;
					}
					else if (lifeRegenTime >= 3000)
					{
						num20 = 8.0;
					}
					else if (lifeRegenTime >= 2400)
					{
						num20 = 7.0;
					}
					else if (lifeRegenTime >= 1800)
					{
						num20 = 6.0;
					}
					else if (lifeRegenTime >= 1500)
					{
						num20 = 5.0;
					}
					else if (lifeRegenTime >= 1200)
					{
						num20 = 4.0;
					}
					else if (lifeRegenTime >= 900)
					{
						num20 = 3.0;
					}
					else if (lifeRegenTime >= 600)
					{
						num20 = 2.0;
					}
					else if (lifeRegenTime >= 300)
					{
						num20 = 1.0;
					}
					num20 = ((velocity.X != 0f && grappling[0] <= 0) ? (num20 * 0.5) : (num20 * 1.25));
					double num21 = StatLifeMax / 400.0 * 0.85 + 0.15;
					num20 *= num21;
					lifeRegen += (int)Math.Round(num20);
				}
				lifeRegenCount += lifeRegen;
				while (lifeRegenCount >= 120)
				{
					lifeRegenCount -= 120;
					if (statLife < StatLifeMax)
					{
						statLife++;
					}
					else if (statLife > StatLifeMax)
					{
						statLife = StatLifeMax;
						break;
					}
				}
				while (lifeRegenCount <= -120)
				{
					lifeRegenCount += 120;
					if (--statLife <= 0 && isLocal())
					{
						if (poisoned)
						{
							KillMe(10.0, 0, pvp: false, Lang.DeathMsgPtr(-1, 0, 0, 3));
						}
						else if (onFire || onFire2)
						{
							KillMe(10.0, 0, pvp: false, Lang.DeathMsgPtr(-1, 0, 0, 4));
						}
					}
				}
				if (manaRegenDelay > 0 && !channel)
				{
					manaRegenDelay--;
					if ((velocity.X == 0f && velocity.Y == 0f) || grappling[0] >= 0 || manaRegenBuff)
					{
						manaRegenDelay--;
					}
				}
				if (manaRegenBuff && manaRegenDelay > 20)
				{
					manaRegenDelay = 20;
				}
				if (manaRegenDelay <= 0 && statManaMax2 > 0)
				{
					manaRegenDelay = 0;
					manaRegen = statManaMax2 / 7 + 1;
					if ((velocity.X == 0f && velocity.Y == 0f) || grappling[0] >= 0 || manaRegenBuff)
					{
						manaRegen += statManaMax2 >> 1;
					}
					float num22 = statMana / (float)statManaMax2 * 0.8f + 0.2f;
					if (manaRegenBuff)
					{
						num22 = 1f;
					}
					manaRegen = (int)(manaRegen * num22);
				}
				else
				{
					manaRegen = 0;
				}
				manaRegenCount += manaRegen;
				while (manaRegenCount >= 120)
				{
					bool flag7 = false;
					manaRegenCount -= 120;
					if (statMana < statManaMax2)
					{
						statMana++;
						flag7 = true;
					}
					if (statMana < statManaMax2)
					{
						continue;
					}
					if (flag7 && isLocal())
					{
						Main.PlaySound(25);
						for (int num23 = 0; num23 < 4; num23++)
						{
							Dust* ptr3 = Main.DustSet.NewDust(45, ref XYWH, 0.0, 0.0, 255, default, Main.Rand.Next(20, 26) * 0.1f);
							if (ptr3 == null)
							{
								break;
							}
							ptr3->NoLight = true;
							ptr3->NoGravity = true;
							ptr3->Velocity *= 0.5f;
						}
					}
					statMana = statManaMax2;
				}
				if (manaRegenCount < 0)
				{
					manaRegenCount = 0;
				}
				if (statMana > statManaMax2)
				{
					statMana = statManaMax2;
				}
				num6 *= moveSpeed;
				num5 *= moveSpeed;
				if (jumpBoost)
				{
					JumpHeight = 20;
					JumpSpeed = 6.51f;
				}
				if (wereWolf)
				{
					JumpHeight += 2;
					JumpSpeed += 0.2f;
				}
				if (brokenArmor)
				{
					StatDefense >>= 1;
				}
				if (!doubleJump)
				{
					jumpAgain = false;
				}
				else if (velocity.Y == 0f)
				{
					jumpAgain = true;
				}
				if (grappling[0] == -1 && !tongued)
				{
					if (controlLeft && velocity.X > 0f - num5)
					{
						if (velocity.X > 0.2f)
						{
							velocity.X -= 0.2f;
						}
						velocity.X -= num6;
						if (itemAnimation == 0 || Inventory[SelectedItem].CanUseTurn)
						{
							direction = -1;
						}
					}
					else if (controlRight && velocity.X < num5)
					{
						if (velocity.X < -0.2f)
						{
							velocity.X += 0.2f;
						}
						velocity.X += num6;
						if (itemAnimation == 0 || Inventory[SelectedItem].CanUseTurn)
						{
							direction = 1;
						}
					}
					else if (controlLeft && velocity.X > 0f - num7)
					{
						if (itemAnimation == 0 || Inventory[SelectedItem].CanUseTurn)
						{
							direction = -1;
						}
						if (velocity.Y == 0f || wings > 0)
						{
							if (velocity.X > 0.2f)
							{
								velocity.X -= 0.2f;
							}
							velocity.X -= num6 * 0.2f;
						}
						if (velocity.X < (0f - (num7 + num5)) * 0.5f && velocity.Y == 0f) // Along with a few other small functions, especially achievement ones, the PS3 version likes to have them as their own little functions.
						{
							int num24 = 0;
							if (gravDir == -1)
							{
								num24 -= height;
							}
							if (runSoundDelay == 0 && velocity.Y == 0f)
							{
								Main.PlaySound(17, XYWH.X, XYWH.Y);
								runSoundDelay = 9;
							}
							Dust* ptr4 = Main.DustSet.NewDust(XYWH.X - 4, XYWH.Y + height + num24, 28, 4, 16, velocity.X * -0.5f, velocity.Y * 0.5f, 50, default(Color), 1.5);
							if (ptr4 != null)
							{
								ptr4->Velocity *= 0.2f;
							}
#if !USE_ORIGINAL_CODE
							if (ui != null)
							{
								ui.RunningTimer++;
								if (ui.RunningTimer > 3599)
								{
									ui.SetTriggerState(Trigger.GoneIn60Seconds);
								}
							}
#endif
						}
					}
					else if (controlRight && velocity.X < num7)
					{
						if (itemAnimation == 0 || Inventory[SelectedItem].CanUseTurn)
						{
							direction = 1;
						}
						if (velocity.Y == 0f || wings > 0)
						{
							if (velocity.X < -0.2f)
							{
								velocity.X += 0.2f;
							}
							velocity.X += num6 * 0.2f;
						}
						if (velocity.X > (num7 + num5) * 0.5f && velocity.Y == 0f)
						{
							int num25 = 0;
							if (gravDir == -1)
							{
								num25 -= height;
							}
							if (runSoundDelay == 0 && velocity.Y == 0f)
							{
								Main.PlaySound(17, XYWH.X, XYWH.Y);
								runSoundDelay = 9;
							}
							Dust* ptr5 = Main.DustSet.NewDust(XYWH.X - 4, XYWH.Y + height + num25, 28, 4, 16, velocity.X * -0.5f, velocity.Y * 0.5f, 50, default, 1.5);
							if (ptr5 != null)
							{
								ptr5->Velocity *= 0.2f;
							}
#if !USE_ORIGINAL_CODE
							if (ui != null)
							{
								ui.RunningTimer++;
								if (ui.RunningTimer > 3599)
								{
									ui.SetTriggerState(Trigger.GoneIn60Seconds);
								}
							}
#endif
						}
					}
					else if (velocity.Y == 0f)
					{
						if (velocity.X > 0.2f)
						{
							velocity.X -= 0.2f;
						}
						else if (velocity.X < -0.2f)
						{
							velocity.X += 0.2f;
						}
						else
						{
							velocity.X = 0f;
#if !USE_ORIGINAL_CODE
							if (ui != null)
							{
								ui.RunningTimer = 0;
							}
#endif
						}
					}
					else if (velocity.X > 0.1)
					{
						velocity.X -= 0.1f;
					}
					else if (velocity.X < -0.1)
					{
						velocity.X += 0.1f;
					}
					else
					{
						velocity.X = 0f;
#if !USE_ORIGINAL_CODE
						if (ui != null)
						{
							ui.RunningTimer = 0;
						}
#endif
					}
					if (gravControl)
					{
						if ((controlUp && gravDir == 1) || (IsControlDown && gravDir == -1))
						{
							gravDir = (sbyte)(-gravDir);
#if !USE_ORIGINAL_CODE
							if (ui != null)
							{
								if (ui.TriggerCheckEnabled(Trigger.SafeFall))
								{
									int FallDamage = ((XYWH.Y >> 4) - fallStart) * gravDir * 10 - 250;
									if (statLife <= Main.CalculateDamage(FallDamage, StatDefense))
									{
										ui.SetTriggerState(Trigger.SafeFall);
									}
								}
							}
#endif
							fallStart = (short)(XYWH.Y >> 4);
							jump = 0;
							Main.PlaySound(2, XYWH.X, XYWH.Y, 8);
						}
					}
					else
					{
						gravDir = 1;
					}
					if (controlJump)
					{
						if (jump > 0)
						{
							if (velocity.Y == 0f)
							{
								jump = 0;
							}
							else
							{
								velocity.Y = (0f - JumpSpeed) * gravDir;
								if (merman)
								{
									if (swimTime <= 10)
									{
										swimTime = 30;
									}
								}
								else
								{
									jump--;
								}
							}
						}
						else if ((velocity.Y == 0f || jumpAgain || (IsWet && accFlipper)) && releaseJump)
						{
							bool flag8 = IsWet && accFlipper;
							if (flag8 && swimTime == 0)
							{
								swimTime = 30;
							}
							jumpAgain = false;
							canRocket = false;
							rocketRelease = false;
							if (velocity.Y == 0f && doubleJump)
							{
								jumpAgain = true;
							}
							if (velocity.Y == 0f || flag8)
							{
								velocity.Y = (0f - JumpSpeed) * gravDir;
								jump = JumpHeight;
							}
							else
							{
								int num26 = height;
								if (gravDir == -1)
								{
									num26 = 0;
								}
								Main.PlaySound(16, XYWH.X, XYWH.Y);
								velocity.Y = (0f - JumpSpeed) * gravDir;
								jump = JumpHeight >> 1;
								for (int num27 = 0; num27 < 8; num27++)
								{
									Dust* ptr6 = Main.DustSet.NewDust(XYWH.X - 34, XYWH.Y + num26 - 16, 102, 32, 16, velocity.X * -0.5f, velocity.Y * 0.5f, 100, default, 1.5);
									if (ptr6 == null)
									{
										break;
									}
									ptr6->Velocity.X = ptr6->Velocity.X * 0.5f - velocity.X * 0.1f;
									ptr6->Velocity.Y = ptr6->Velocity.Y * 0.5f - velocity.Y * 0.3f;
								}
								int num28 = Gore.NewGore(new Vector2(Position.X + 10f - 16f, Position.Y + num26 - 16f), new Vector2(0f - velocity.X, 0f - velocity.Y), Main.Rand.Next(11, 14));
								Main.GoreSet[num28].Velocity.X = Main.GoreSet[num28].Velocity.X * 0.1f - velocity.X * 0.1f;
								Main.GoreSet[num28].Velocity.Y = Main.GoreSet[num28].Velocity.Y * 0.1f - velocity.Y * 0.05f;
								num28 = Gore.NewGore(new Vector2(Position.X - 36f, Position.Y + num26 - 16f), new Vector2(0f - velocity.X, 0f - velocity.Y), Main.Rand.Next(11, 14));
								Main.GoreSet[num28].Velocity.X = Main.GoreSet[num28].Velocity.X * 0.1f - velocity.X * 0.1f;
								Main.GoreSet[num28].Velocity.Y = Main.GoreSet[num28].Velocity.Y * 0.1f - velocity.Y * 0.05f;
								num28 = Gore.NewGore(new Vector2(Position.X + 20f + 4f, Position.Y + num26 - 16f), new Vector2(0f - velocity.X, 0f - velocity.Y), Main.Rand.Next(11, 14));
								Main.GoreSet[num28].Velocity.X = Main.GoreSet[num28].Velocity.X * 0.1f - velocity.X * 0.1f;
								Main.GoreSet[num28].Velocity.Y = Main.GoreSet[num28].Velocity.Y * 0.1f - velocity.Y * 0.05f;
							}
							if (ui != null)
							{
								ui.TotalJumps++;
							}
						}
						releaseJump = false;
					}
					else
					{
						jump = 0;
						releaseJump = true;
						rocketRelease = true;
					}
					if (doubleJump && !jumpAgain && ((gravDir == 1 && velocity.Y < 0f) || (gravDir == -1 && velocity.Y > 0f)) && rocketBoots == 0 && !accFlipper)
					{
						int num29 = height;
						if (gravDir == -1)
						{
							num29 = -6;
						}
						Dust* ptr7 = Main.DustSet.NewDust(XYWH.X - 4, XYWH.Y + num29, 28, 4, 16, velocity.X * -0.5f, velocity.Y * 0.5f, 100, default, 1.5);
						if (ptr7 != null)
						{
							ptr7->Velocity.X = ptr7->Velocity.X * 0.5f - velocity.X * 0.1f;
							ptr7->Velocity.Y = ptr7->Velocity.Y * 0.5f - velocity.Y * 0.3f;
						}
					}
					if (((gravDir == 1 && velocity.Y > 0f - JumpSpeed) || (gravDir == -1 && velocity.Y < JumpSpeed)) && velocity.Y != 0f)
					{
						canRocket = true;
					}
					bool flag9 = false;
					if (velocity.Y == 0f)
					{
#if !USE_ORIGINAL_CODE
						CurrentGround = XYWH.Y;
#endif
						wingTime = 90;
					}
					if (wings > 0 && controlJump && wingTime > 0 && !jumpAgain && jump == 0 && velocity.Y != 0f)
					{
						flag9 = true;
					}
					if (flag9)
					{
						velocity.Y -= 0.1f * gravDir;
						if (gravDir == 1)
						{
							if (velocity.Y > 0f)
							{
								velocity.Y -= 0.5f;
							}
							else if (velocity.Y > (double)(0f - JumpSpeed) * 0.5)
							{
								velocity.Y -= 0.1f;
							}
							if (velocity.Y < (0f - JumpSpeed) * 1.5f)
							{
								velocity.Y = (0f - JumpSpeed) * 1.5f;
							}
						}
						else
						{
							if (velocity.Y < 0f)
							{
								velocity.Y += 0.5f;
							}
							else if (velocity.Y < (double)JumpSpeed * 0.5)
							{
								velocity.Y += 0.1f;
							}
							if (velocity.Y > JumpSpeed * 1.5f)
							{
								velocity.Y = JumpSpeed * 1.5f;
							}
						}
						wingTime--;
					}
					if (flag9 || jump > 0)
					{
						if (++wingFrameCounter > 4)
						{
							wingFrameCounter = 0;
							wingFrame = (byte)((uint)(wingFrame + 1) & 3u);
						}
					}
					else if (velocity.Y != 0f)
					{
						wingFrame = 1;
					}
					else
					{
						wingFrame = 0;
					}
					if (wings > 0 && rocketBoots > 0)
					{
						wingTime = (short)(wingTime + rocketTime * 10);
						rocketTime = 0;
					}
					if (flag9)
					{
						if (wingFrame == 3)
						{
							if (!flapSound)
							{
								flapSound = true;
								Main.PlaySound(2, XYWH.X, XYWH.Y, 32);
							}
						}
						else
						{
							flapSound = false;
						}
					}
					if (velocity.Y == 0f)
					{
						rocketTime = 7;
					}
					if ((wingTime == 0 || wings == 0) && rocketBoots > 0 && controlJump && rocketDelay == 0 && canRocket && rocketRelease && !jumpAgain)
					{
						if (rocketTime > 0)
						{
							rocketTime--;
							rocketDelay = 10;
							if (rocketDelay2 <= 0)
							{
								if (rocketBoots == 1)
								{
									Main.PlaySound(2, XYWH.X, XYWH.Y, 13);
									rocketDelay2 = 30;
								}
								else if (rocketBoots == 2)
								{
									Main.PlaySound(2, XYWH.X, XYWH.Y, 24);
									rocketDelay2 = 15;
								}
							}
						}
						else
						{
							canRocket = false;
						}
					}
					if (rocketDelay2 > 0)
					{
						rocketDelay2--;
					}
					if (rocketDelay == 0)
					{
						rocketFrame = false;
					}
					if (rocketDelay > 0)
					{
						int num30 = height;
						if (gravDir == -1)
						{
							num30 = 4;
						}
						rocketFrame = true;
						if ((Main.FrameCounter & 1) == 0)
						{
							int type = 6;
							float num31 = 2.5f;
							int alpha = 100;
							if (rocketBoots == 2)
							{
								type = 16;
								num31 = 1.5f;
								alpha = 20;
							}
							else if (socialShadow)
							{
								type = 27;
								num31 = 1.5f;
							}
							int num32 = XYWH.X - 4;
							int y = XYWH.Y + num30 - 10;
							for (int num33 = 0; num33 < 2; num33++)
							{
								Dust* ptr8 = Main.DustSet.NewDust(num32, y, 8, 8, type, 0.0, 0.0, alpha, default, num31);
								if (ptr8 == null)
								{
									break;
								}
								ptr8->Velocity.X = ptr8->Velocity.X * 1f + 2f - velocity.X * 0.3f;
								ptr8->Velocity.Y = ptr8->Velocity.Y * 1f + 2 * gravDir - velocity.Y * 0.3f;
								if (rocketBoots == 1)
								{
									ptr8->NoGravity = true;
								}
								else
								{
									ptr8->Velocity.X *= 0.1f;
									ptr8->Velocity.Y *= 0.1f;
								}
								num32 += 20;
							}
						}
						rocketDelay--;
						velocity.Y -= 0.1f * gravDir;
						if (gravDir == 1)
						{
							if (velocity.Y > 0f)
							{
								velocity.Y -= 0.5f;
							}
							else if (velocity.Y > (double)(0f - JumpSpeed) * 0.5)
							{
								velocity.Y -= 0.1f;
							}
							if (velocity.Y < (0f - JumpSpeed) * 1.5f)
							{
								velocity.Y = (0f - JumpSpeed) * 1.5f;
							}
						}
						else
						{
							if (velocity.Y < 0f)
							{
								velocity.Y += 0.5f;
							}
							else if (velocity.Y < (double)JumpSpeed * 0.5)
							{
								velocity.Y += 0.1f;
							}
							if (velocity.Y > JumpSpeed * 1.5f)
							{
								velocity.Y = JumpSpeed * 1.5f;
							}
						}
					}
					else if (!flag9)
					{
						if (slowFall && ((!IsControlDown && gravDir == 1) || (!controlUp && gravDir == -1)))
						{
							if ((controlUp && gravDir == 1) || (IsControlDown && gravDir == -1))
							{
								velocity.Y += num2 / 10f * gravDir;
							}
							else
							{
								velocity.Y += num2 / 3f * gravDir;
							}
						}
						else if (wings > 0 && controlJump && velocity.Y > 0f)
						{
#if !USE_ORIGINAL_CODE
							if (ui != null)
							{
								if (ui.TriggerCheckEnabled(Trigger.SafeFall))
								{
									int FallDamage = ((XYWH.Y >> 4) - fallStart) * gravDir * 10 - 250;
									if (statLife <= Main.CalculateDamage(FallDamage, StatDefense))
									{
										ui.SetTriggerState(Trigger.SafeFall);
									}
								}
							}
#endif
							fallStart = (short)(XYWH.Y >> 4);
							if (velocity.Y > 0f)
							{
								wingFrame = 2;
							}
							velocity.Y += num2 / 3f * gravDir;
							if (gravDir == 1)
							{
								if (velocity.Y > num / 3f && !IsControlDown)
								{
									velocity.Y = num / 3f;
								}
							}
							else if (velocity.Y < (0f - num) / 3f && !controlUp)
							{
								velocity.Y = (0f - num) / 3f;
							}
						}
						else
						{
							velocity.Y += num2 * gravDir;
						}
					}
					if (gravDir == 1)
					{
						if (velocity.Y > num)
						{
							velocity.Y = num;
						}
						if (slowFall && velocity.Y > num / 3f && !IsControlDown)
						{
							velocity.Y = num / 3f;
						}
						if (slowFall && velocity.Y > num / 5f && controlUp)
						{
							velocity.Y = num / 10f;
						}
					}
					else
					{
						if (velocity.Y < 0f - num)
						{
							velocity.Y = 0f - num;
						}
						if (slowFall && velocity.Y < (0f - num) / 3f && !controlUp)
						{
							velocity.Y = (0f - num) / 3f;
						}
						if (slowFall && velocity.Y < (0f - num) / 5f && IsControlDown)
						{
							velocity.Y = (0f - num) / 10f;
						}
					}
				}
				fixed (Item* ptr9 = Main.ItemSet)
				{
					Item* ptr10 = ptr9 + Main.MaxNumItems - 1;
					for (int num34 = Main.MaxNumItems - 1; num34 >= 0; num34--)
					{
						if (ptr10->Active != 0 && ptr10->NoGrabDelay == 0 && ptr10->Owner == i)
						{
							if (XYWH.Intersects(new Rectangle((int)ptr10->Position.X, (int)ptr10->Position.Y, ptr10->Width, ptr10->Height)))
							{
								if (isLocal() && (Inventory[SelectedItem].Type != 0 || itemAnimation <= 0))
								{
									if (ptr10->Type == (int)Item.ID.HEART)
									{
										Main.PlaySound(7, XYWH.X, XYWH.Y);
										statLife += 20;
										HealEffect(20);
										if (statLife > StatLifeMax)
										{
											statLife = StatLifeMax;
										}
										ptr10->Init();
										NetMessage.CreateMessage2(21, WhoAmI, num34);
										NetMessage.SendMessage();
									}
									else if (ptr10->Type == (int)Item.ID.STAR)
									{
										Main.PlaySound(7, XYWH.X, XYWH.Y);
										statMana += 100;
										ManaEffect(100);
										if (statMana > statManaMax2)
										{
											statMana = statManaMax2;
										}
										ptr10->Init();
										NetMessage.CreateMessage2(21, WhoAmI, num34);
										NetMessage.SendMessage();
									}
									else if (GetItem(ref *ptr10))
									{
										NetMessage.CreateMessage2(21, WhoAmI, num34);
										NetMessage.SendMessage();
									}
								}
							}
							else if (new Rectangle(XYWH.X - itemGrabRange, XYWH.Y - itemGrabRange, width + (itemGrabRange * 2), height + (itemGrabRange * 2)).Intersects(new Rectangle((int)ptr10->Position.X, (int)ptr10->Position.Y, ptr10->Width, ptr10->Height)) && ItemSpace(ptr10))
							{
								ptr10->BeingGrabbed = true;
								if (XYWH.X + (width / 2) > (int)ptr10->Position.X + (ptr10->Width >> 1))
								{
									if (ptr10->Velocity.X < itemGrabSpeedMax + velocity.X)
									{
										ptr10->Velocity.X += itemGrabSpeed;
									}
									if (ptr10->Velocity.X < 0f)
									{
										ptr10->Velocity.X += itemGrabSpeed * 0.75f;
									}
								}
								else
								{
									if (ptr10->Velocity.X > -itemGrabSpeedMax + velocity.X)
									{
										ptr10->Velocity.X -= itemGrabSpeed;
									}
									if (ptr10->Velocity.X > 0f)
									{
										ptr10->Velocity.X -= itemGrabSpeed * 0.75f;
									}
								}
								if (XYWH.Y + (height / 2) > (int)ptr10->Position.Y + (ptr10->Height >> 1))
								{
									if (ptr10->Velocity.Y < itemGrabSpeedMax)
									{
										ptr10->Velocity.Y += itemGrabSpeed;
									}
									if (ptr10->Velocity.Y < 0f)
									{
										ptr10->Velocity.Y += itemGrabSpeed * 0.75f;
									}
								}
								else
								{
									if (ptr10->Velocity.Y > -itemGrabSpeedMax)
									{
										ptr10->Velocity.Y -= itemGrabSpeed;
									}
									if (ptr10->Velocity.Y > 0f)
									{
										ptr10->Velocity.Y -= itemGrabSpeed * 0.75f;
									}
								}
							}
						}
						ptr10--;
					}
				}
				if (isLocal() && TalkNPC < 0)
				{
					if (controlUseTile)
					{
						if (releaseUseTile)
						{
							releaseUseTile = false;
							controlUseTile = false;
							if (tileInteractY > 0)
							{
								InteractWithTile(tileInteractX << 4, tileInteractY << 4);
							}
							else if (npcChatBubble >= 0)
							{
								ui.npcShop = 0;
								ui.CraftGuide = false;
								dropItemCheck();
								noThrow = 2;
								sign = -1;
								PlayerChest = -1;
								ui.editSign = false;
								TalkNPC = npcChatBubble;
								ui.npcChatText = Main.NPCSet[TalkNPC].GetChat(this);
								Main.PlaySound(24);
								ui.ClearButtonTriggers();
							}
						}
					}
					else
					{
						releaseUseTile = true;
					}
				}
				if (tongued)
				{
					bool flag10 = false;
					if (NPC.WoF >= 0)
					{
						float num35 = Main.NPCSet[NPC.WoF].Position.X + (Main.NPCSet[NPC.WoF].Width >> 1);
						num35 += Main.NPCSet[NPC.WoF].Direction * 200;
						float num36 = Main.NPCSet[NPC.WoF].Position.Y + (Main.NPCSet[NPC.WoF].Height >> 1);
						Vector2 vector = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
						float num37 = num35 - vector.X;
						float num38 = num36 - vector.Y;
						float num39 = (float)Math.Sqrt(num37 * num37 + num38 * num38);
						float num40 = 11f;
						float num41 = num39;
						if (num39 > num40)
						{
							num41 = num40 / num39;
						}
						else
						{
							num41 = 1f;
							flag10 = true;
						}
						num37 *= num41;
						num38 *= num41;
						velocity.X = num37;
						velocity.Y = num38;
					}
					else
					{
						flag10 = true;
					}
					if (flag10 && isLocal())
					{
						DelBuff(Buff.ID.TONGUED);
					}
				}
				if (isLocal())
				{
					if (NPC.WoF >= 0 && Main.NPCSet[NPC.WoF].Active != 0)
					{
						int num42 = Main.NPCSet[NPC.WoF].XYWH.X + 40;
						if (Main.NPCSet[NPC.WoF].Direction > 0)
						{
							num42 -= 96;
						}
						if (XYWH.X + width > num42 && XYWH.X < num42 + 140 && IsHorrified)
						{
							noKnockback = false;
							Hurt(50, Main.NPCSet[NPC.WoF].Direction, pvp: false, quiet: false, Lang.DeathMsgPtr(-1, 113));
						}
						if (!IsHorrified)
						{
							if (XYWH.Y > (Main.MaxTilesY - 250) * 16 && XYWH.X > num42 - 1920 && XYWH.X < num42 + 1920)
							{
								AddBuff((int)Buff.ID.HORRIFIED, 10);
								Main.PlaySound(4, Main.NPCSet[NPC.WoF].XYWH.X, Main.NPCSet[NPC.WoF].XYWH.Y, 10);
							}
						}
						else if (XYWH.Y < (Main.MaxTilesY - 200) * 16)
						{
							AddBuff((int)Buff.ID.TONGUED, 10);
						}
						else if (Main.NPCSet[NPC.WoF].Direction < 0)
						{
							if (XYWH.X + (width / 2) > Main.NPCSet[NPC.WoF].XYWH.X + (Main.NPCSet[NPC.WoF].Width >> 1) + 40)
							{
								AddBuff((int)Buff.ID.TONGUED, 10);
							}
						}
						else if (XYWH.X + (width / 2) < Main.NPCSet[NPC.WoF].XYWH.X + (Main.NPCSet[NPC.WoF].Width >> 1) - 40)
						{
							AddBuff((int)Buff.ID.TONGUED, 10);
						}
						if (tongued)
						{
							controlHook = false;
							controlUseItem = false;
							for (int num43 = 0; num43 < Projectile.MaxNumProjs; num43++)
							{
								if (Main.ProjectileSet[num43].active != 0 && Main.ProjectileSet[num43].owner == i && Main.ProjectileSet[num43].aiStyle == 7)
								{
									Main.ProjectileSet[num43].Kill();
								}
							}
							Vector2 vector2 = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
							float num44 = Main.NPCSet[NPC.WoF].Position.X + Main.NPCSet[NPC.WoF].Width / 2 - vector2.X;
							float num45 = Main.NPCSet[NPC.WoF].Position.Y + Main.NPCSet[NPC.WoF].Height / 2 - vector2.Y;
							float num46 = num44 * num44 + num45 * num45;
							if (num46 > 9000000f)
							{
								KillMe(1000.0, 0, pvp: false, Lang.DeathMsgPtr(-1, 0, 0, 5));
							}
							else if (Main.NPCSet[NPC.WoF].XYWH.X < 608 || Main.NPCSet[NPC.WoF].XYWH.X > (Main.MaxTilesX - 38) * 16)
							{
								KillMe(1000.0, 0, pvp: false, Lang.DeathMsgPtr(-1, 0, 0, 6));
							}
						}
					}
					UpdateGrappleItemSlot();
					if (controlHook)
					{
						if (releaseHook)
						{
							releaseHook = false;
							QuickGrapple();
						}
					}
					else
					{
						releaseHook = true;
					}
					if (TalkNPC >= 0 && (!new Rectangle(XYWH.X + (width / 2) - (tileRangeX * 16), XYWH.Y + (height / 2) - (tileRangeY * 16), (tileRangeX * 16) * 2, (tileRangeY * 16) * 2).Intersects(Main.NPCSet[TalkNPC].XYWH) || PlayerChest != -1 || Main.NPCSet[TalkNPC].Active == 0))
					{
						if (PlayerChest == -1)
						{
							Main.PlaySound(11);
						}
						TalkNPC = -1;
						ui.npcChatText = null;
					}
					int num49;
					if (!immune)
					{
						for (int num47 = 0; num47 < NPC.MaxNumNPCs; num47++)
						{
							if (Main.NPCSet[num47].Active == 0 || Main.NPCSet[num47].IsFriendly || Main.NPCSet[num47].Damage <= 0 || !XYWH.Intersects(Main.NPCSet[num47].XYWH))
							{
								continue;
							}
							int num48 = 1;
							if (Main.NPCSet[num47].XYWH.X + (Main.NPCSet[num47].Width >> 1) < XYWH.X + (width / 2))
							{
								num48 = -1;
							}
							num49 = Main.DamageVar(Main.NPCSet[num47].Damage);
							if (isLocal() && thorns && !Main.NPCSet[num47].DontTakeDamage)
							{
								int num50 = num49 / 3;
								Main.NPCSet[num47].StrikeNPC(num50, 10f, num48);
								NetMessage.SendNpcHurt(num47, num50, 10.0, num48);
							}
							if (Main.NPCSet[num47].NetID == -6)
							{
								if (Main.Rand.Next(4) == 0)
								{
									AddBuff((int)Buff.ID.BLIND, 900);
								}
							}
							else
							{
								switch ((NPC.ID)Main.NPCSet[num47].Type)
								{
									case NPC.ID.CORRUPT_SLIME:
									case NPC.ID.SHADOW_SLIME:
										if (Main.Rand.Next(4) == 0)
										{
											AddBuff((int)Buff.ID.BLIND, 900);
										}
										break;
									case NPC.ID.DARK_MUMMY:
									case NPC.ID.SHADOW_MUMMY:
										if (Main.Rand.Next(4) == 0)
										{
											AddBuff((int)Buff.ID.BLIND, 900);
										}
										else if (Main.Rand.Next(5) == 0)
										{
											AddBuff((int)Buff.ID.SILENCE, 420);
										}
										break;
									case NPC.ID.METEOR_HEAD:
									case NPC.ID.BURNING_SPHERE:
										if (Main.Rand.Next(3) == 0)
										{
											AddBuff((int)Buff.ID.ON_FIRE, 420);
										}
										break;
									case NPC.ID.CURSED_SKULL:
									case NPC.ID.CURSED_HAMMER:
									case NPC.ID.ENCHANTED_SWORD:
									case NPC.ID.DRAGON_SKULL:
										if (Main.Rand.Next(3) == 0)
										{
											AddBuff((int)Buff.ID.NO_ITEMS, 240);
										}
										break;
									case NPC.ID.SHADOW_HAMMER:
										if (Main.Rand.Next(4) == 0)
										{
											AddBuff((int)Buff.ID.BLIND, 900);
										}
										else if (Main.Rand.Next(3) == 0)
										{
											AddBuff((int)Buff.ID.NO_ITEMS, 240);
										}
										break;
									case NPC.ID.ANGLER_FISH:
									case NPC.ID.WEREWOLF:
										if (Main.Rand.Next(8) == 0)
										{
											AddBuff((int)Buff.ID.BLEED, 2700);
										}
										break;
									case NPC.ID.PIXIE:
										if (Main.Rand.Next(10) == 0)
										{
											AddBuff((int)Buff.ID.SILENCE, 420);
										}
										else if (Main.Rand.Next(8) == 0)
										{
											AddBuff((int)Buff.ID.SLOW, 900);
										}
										break;
									case NPC.ID.GREEN_JELLYFISH:
										if (Main.Rand.Next(5) == 0)
										{
											AddBuff((int)Buff.ID.SILENCE, 420);
										}
										break;
									case NPC.ID.MUMMY:
									case NPC.ID.WRAITH:
										if (Main.Rand.Next(8) == 0)
										{
											AddBuff((int)Buff.ID.SLOW, 900);
										}
										break;
									case NPC.ID.LIGHT_MUMMY:
									case NPC.ID.GIANT_BAT:
									case NPC.ID.CLOWN:
									case NPC.ID.SPECTRAL_MUMMY:
										if (Main.Rand.Next(12) == 0)
										{
											AddBuff((int)Buff.ID.CONFUSED, 420);
										}
										break;
									case NPC.ID.ARMORED_SKELETON:
										if (Main.Rand.Next(6) == 0)
										{
											AddBuff((int)Buff.ID.BROKEN_ARMOR, 18000);
										}
										break;
									case NPC.ID.VILE_SPIT:
										if (Main.Rand.Next(20) == 0)
										{
											AddBuff((int)Buff.ID.WEAK, 18000);
										}
										break;
									case NPC.ID.TOXIC_SLUDGE:
										if (Main.Rand.Next(2) == 0)
										{
											AddBuff((int)Buff.ID.POISONED, 600);
										}
										break;
								}
							}
							Hurt(num49, -num48, pvp: false, quiet: false, Lang.DeathMsgPtr(-1, Main.NPCSet[num47].NetID));
						}
					}
					num49 = Collision.HurtTiles(ref Position, ref velocity, width, height, fireWalk);
					if (num49 != 0)
					{
						num49 = Main.DamageVar(num49);
						Hurt(num49, 0, pvp: false, quiet: false, Lang.DeathMsgPtr());
					}
				}
				if (grappling[0] >= 0)
				{
					wingFrame = 1;
					if (velocity.Y == 0f || (IsWet && velocity.Y > -0.02 && velocity.Y < 0.02))
					{
						wingFrame = 0;
					}
					wingTime = 90;
					rocketTime = rocketTimeMax;
					rocketDelay = 0;
					rocketFrame = false;
					canRocket = false;
					rocketRelease = false;
#if !USE_ORIGINAL_CODE
					if (ui != null)
					{
						if (ui.TriggerCheckEnabled(Trigger.SafeFall))
						{
							int FallDamage = ((XYWH.Y >> 4) - fallStart) * gravDir * 10 - 250;
							if (statLife <= Main.CalculateDamage(FallDamage, StatDefense))
							{
								ui.SetTriggerState(Trigger.SafeFall);
							}
						}
					}
#endif
					fallStart = (short)(XYWH.Y >> 4);
					float num51 = 0f;
					float num52 = 0f;
					for (int num53 = 0; num53 < grapCount; num53++)
					{
						num51 += Main.ProjectileSet[grappling[num53]].position.X + (Main.ProjectileSet[grappling[num53]].width >> 1);
						num52 += Main.ProjectileSet[grappling[num53]].position.Y + (Main.ProjectileSet[grappling[num53]].height >> 1);
					}
					num51 /= grapCount;
					num52 /= grapCount;
					Vector2 vector3 = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
					float num54 = num51 - vector3.X;
					float num55 = num52 - vector3.Y;
					float num56 = num54 * num54 + num55 * num55;
					if (num56 > 121f)
					{
						float num57 = 11f / (float)Math.Sqrt(num56);
						num54 *= num57;
						num55 *= num57;
					}
					velocity.X = num54;
					velocity.Y = num55;
					if (itemAnimation == 0)
					{
						if (velocity.X > 0f)
						{
							direction = 1;
						}
						else if (velocity.X < 0f)
						{
							direction = -1;
						}
					}
					if (controlJump)
					{
						if (releaseJump)
						{
							if ((velocity.Y == 0f || (IsWet && velocity.Y > -0.02 && velocity.Y < 0.02)) && !IsControlDown)
							{
								velocity.Y = 0f - JumpSpeed;
								jump = JumpHeight >> 1;
								releaseJump = false;
							}
							else
							{
								velocity.Y += 0.01f;
								releaseJump = false;
							}
							if (doubleJump)
							{
								jumpAgain = true;
							}
							grappling[0] = 0;
							grapCount = 0;
							for (int num58 = 0; num58 < Projectile.MaxNumProjs; num58++)
							{
								if (Main.ProjectileSet[num58].owner == i && Main.ProjectileSet[num58].aiStyle == 7 && Main.ProjectileSet[num58].active != 0)
								{
									Main.ProjectileSet[num58].Kill();
								}
							}
						}
					}
					else
					{
						releaseJump = true;
					}
				}
				Vector2i vector2i = Collision.StickyTiles(Position, velocity, width, height);
				if (vector2i.Y != -1 && vector2i.X != -1)
				{
					if (isLocal() && (velocity.X != 0f || velocity.Y != 0f))
					{
						stickyBreak++;
						if (stickyBreak > Main.Rand.Next(20, 100))
						{
							stickyBreak = 0;
							if (WorldGen.KillTile(vector2i.X, vector2i.Y))
							{
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, vector2i.X, vector2i.Y, 0);
								NetMessage.SendMessage();
							}
						}
					}
#if !USE_ORIGINAL_CODE
					if (ui != null)
					{
						if (ui.TriggerCheckEnabled(Trigger.SafeFall))
						{
							int FallDamage = ((XYWH.Y >> 4) - fallStart) * gravDir * 10 - 250;
							if (statLife <= Main.CalculateDamage(FallDamage, StatDefense))
							{
								ui.SetTriggerState(Trigger.SafeFall);
							}
						}
					}
#endif
					fallStart = (short)(XYWH.Y >> 4);
					jump = 0;
					if (velocity.X > 1f)
					{
						velocity.X = 1f;
					}
					else if (velocity.X < -1f)
					{
						velocity.X = -1f;
					}
					if (velocity.X > 0.75 || velocity.X < -0.75)
					{
						velocity.X *= 0.85f;
					}
					else
					{
						velocity.X *= 0.6f;
					}
					if (velocity.Y > 1f)
					{
						velocity.Y = 1f;
					}
					else if (velocity.Y < -5f)
					{
						velocity.Y = -5f;
					}
					if (velocity.Y < 0f)
					{
						velocity.Y *= 0.96f;
					}
					else
					{
						velocity.Y *= 0.3f;
					}
				}
				else
				{
					stickyBreak = 0;
				}
				bool flag11 = Collision.DrownCollision(ref Position, width, height, gravDir);
				if (armor[0].Type == (int)Item.ID.FISH_BOWL)
				{
					flag11 = true;
				}
				if (Inventory[SelectedItem].Type == (int)Item.ID.BREATHING_REED)
				{
					try
					{
						int num59 = XYWH.X + (width / 2) + 6 * direction >> 4;
						int num60 = 0;
						if (gravDir == -1)
						{
							num60 = height;
						}
						int num61 = XYWH.Y + num60 - 44 * gravDir >> 4;
						if (Main.TileSet[num59, num61].Liquid < 128 && (Main.TileSet[num59, num61].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[num59, num61].Type]))
						{
							flag11 = false;
						}
					}
					catch
					{
					}
				}
				flag11 ^= gills;
				if (isLocal())
				{
					if (merman)
					{
						flag11 = false;
					}
					if (flag11)
					{
						breathCD++;
						int num62 = 7;
						if (Inventory[SelectedItem].Type == (int)Item.ID.BREATHING_REED)
						{
							num62 *= 2;
						}
						if (accDivingHelm)
						{
							num62 *= 4;
						}
						if (breathCD >= num62)
						{
							breathCD = 0;
							breath--;
							if (breath == 0)
							{
								Main.PlaySound(23);
							}
							if (breath <= 0)
							{
								lifeRegenTime = 0;
								breath = 0;
								statLife -= 2;
								if (statLife <= 0)
								{
									statLife = 0;
									KillMe(10.0, 0, pvp: false, Lang.DeathMsgPtr(-1, 0, 0, 1));
								}
							}
						}
					}
					else
					{
						breath += 3;
						if (breath > MaxBreath)
						{
							breath = MaxBreath;
						}
						breathCD = 0;
					}
				}
				if (flag11 && Main.Rand.Next(20) == 0 && !lavaWet)
				{
					int num63 = ((gravDir == -1) ? (num63 = height - 12) : 0);
					if (Inventory[SelectedItem].Type == (int)Item.ID.BREATHING_REED)
					{
						Main.DustSet.NewDust(XYWH.X + 10 * direction + 4, XYWH.Y + num63 - 54 * gravDir, width - 8, 8, 34, 0.0, 0.0, 0, default, 1.2f);
					}
					else
					{
						Main.DustSet.NewDust(XYWH.X + 12 * direction, XYWH.Y + num63 + 4 * gravDir, width - 8, 8, 34, 0.0, 0.0, 0, default, 1.2f);
					}
				}
				int num64 = height;
				if (waterWalk)
				{
					num64 -= 6;
				}
				bool flag12 = Collision.LavaCollision(ref Position, width, num64);
				if (flag12)
				{
					if (!lavaImmune && !immune && isLocal())
					{
						AddBuff((int)Buff.ID.ON_FIRE, 420);
						Hurt(80, 0, pvp: false, quiet: false, Lang.DeathMsgPtr(-1, 0, 0, 2));
					}
					lavaWet = true;
				}
				if (Collision.WetCollision(ref Position, width, height))
				{
					if (onFire && !lavaWet)
					{
						DelBuff(Buff.ID.ON_FIRE);
					}
					if (!IsWet)
					{
						if (wetCount == 0)
						{
							wetCount = 10;
							if (!flag12)
							{
								for (int num65 = 0; num65 < 32; num65++)
								{
									Dust* ptr11 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (height / 2) - 8, width + 12, 24, 33);
									if (ptr11 == null)
									{
										break;
									}
									ptr11->Velocity.Y -= 4f;
									ptr11->Velocity.X *= 2.5f;
									ptr11->Scale = 1.3f;
									ptr11->Alpha = 100;
									ptr11->NoGravity = true;
								}
								Main.PlaySound(19, XYWH.X, XYWH.Y, 0);
							}
							else
							{
								for (int num66 = 0; num66 < 16; num66++)
								{
									Dust* ptr12 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (height / 2) - 8, width + 12, 24, 35);
									if (ptr12 == null)
									{
										break;
									}
									ptr12->Velocity.Y -= 1.5f;
									ptr12->Velocity.X *= 2.5f;
									ptr12->Scale = 1.3f;
									ptr12->Alpha = 100;
									ptr12->NoGravity = true;
								}
								Main.PlaySound(19, XYWH.X, XYWH.Y);
							}
						}
						IsWet = true;
					}
				}
				else if (IsWet)
				{
					IsWet = false;
					if (jump > JumpHeight / 5)
					{
						jump = JumpHeight / 5;
					}
					if (wetCount == 0)
					{
						wetCount = 16;
						if (!lavaWet)
						{
							for (int num67 = 0; num67 < 24; num67++)
							{
								Dust* ptr13 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (height / 2), width + 12, 24, 33);
								if (ptr13 == null)
								{
									break;
								}
								ptr13->Velocity.Y -= 4f;
								ptr13->Velocity.X *= 2.5f;
								ptr13->Scale = 1.3f;
								ptr13->Alpha = 100;
								ptr13->NoGravity = true;
							}
							Main.PlaySound(19, XYWH.X, XYWH.Y, 0);
						}
						else
						{
							for (int num68 = 0; num68 < 8; num68++)
							{
								Dust* ptr14 = Main.DustSet.NewDust(XYWH.X - 6, XYWH.Y + (height / 2) - 8, width + 12, 24, 35);
								if (ptr14 == null)
								{
									break;
								}
								ptr14->Velocity.Y -= 1.5f;
								ptr14->Velocity.X *= 2.5f;
								ptr14->Scale = 1.3f;
								ptr14->Alpha = 100;
								ptr14->NoGravity = true;
							}
							Main.PlaySound(19, XYWH.X, XYWH.Y);
						}
					}
				}
				if (!IsWet)
				{
					lavaWet = false;
				}
				if (wetCount > 0)
				{
					wetCount--;
				}
				oldPosition = Position;
				if (tongued)
				{
					Position.X += velocity.X;
					Position.Y += velocity.Y;
				}
				else if (IsWet && !merman)
				{
					Vector2 vector4 = velocity;
					Collision.TileCollision(ref Position, ref velocity, width, height, IsControlDown);
					Vector2 vector5 = velocity;
					vector5.X *= 0.5f;
					vector5.Y *= 0.5f;
					if (velocity.X != vector4.X)
					{
						vector5.X = velocity.X;
					}
					if (velocity.Y != vector4.Y)
					{
						vector5.Y = velocity.Y;
					}
					Position.X += vector5.X;
					Position.Y += vector5.Y;
				}
				else
				{
					Collision.TileCollision(ref Position, ref velocity, width, height, IsControlDown);
					if (waterWalk)
					{
						velocity = Collision.WaterCollision(Position, velocity, width, height, IsControlDown);
					}
					Position.X += velocity.X;
					Position.Y += velocity.Y;
				}
				if (velocity.Y == 0f)
				{
					if (gravDir == 1 && Collision.UpCol)
					{
						velocity.Y = 0.01f;
						if (!merman)
						{
							jump = 0;
						}
					}
					else if (gravDir == -1 && Collision.DownCol)
					{
						velocity.Y = -0.01f;
						if (!merman)
						{
							jump = 0;
						}
					}
				}
				if (isLocal())
				{
#if !USE_ORIGINAL_CODE
					bool ToRun = false;
					if (gravDir < 0 && 0f < OldVelocity.Y && velocity.Y < 0f)
					{
						ToRun = true;
					}
					if (!ToRun && OldVelocity.Y < 0f && 0f < velocity.Y)
					{
						ToRun = true;
					}

					if (ToRun)
					{
						int GroundDiff = CurrentGround - XYWH.Y;

						sbyte Sign;
						if (GroundDiff < 0)
						{
							Sign = -1;
						}
						else
						{
							Sign = 0;
						}
						if (1600 < (Sign ^ GroundDiff) - Sign)
						{
							ui.SetTriggerState(Trigger.LeapTallBuildingInSingleBound);
						}
					}
#endif

					switch (hellAndBackState)
					{
					case 0:
					case 2:
						if (XYWH.Y < Main.WorldSurfacePixels)
						{
#if !USE_ORIGINAL_CODE
							HellevatorTimer = 0;
#endif
							hellAndBackState++;
						}
						break;
					case 1:
#if !USE_ORIGINAL_CODE
						HellevatorTimer++;
						if (XYWH.Y > Main.MagmaLayerPixels)
						{
							hellAndBackState++;
							if (HellevatorTimer < 3601)
							{
								ui.SetTriggerState(Trigger.Hellevator);
							}
						}
#else
						if (XYWH.Y > Main.MagmaLayerPixels)
						{
							hellAndBackState++;
						}
#endif
						break;
					case 3:
						hellAndBackState++;
						ui.SetTriggerState(Trigger.WentDownAndUpWithoutDyingOrWarping);
						break;
					}
					Collision.SwitchTiles(Position, width, height, oldPosition);
				}
				if (Position.X < 544f + 16f)
				{
					Position.X = 544f + 16f;
					velocity.X = 0f;
				}
				else if (Position.X + width > Main.RightWorld - 544 - 32)
				{
					Position.X = Main.RightWorld - 544 - 32 - width;
					velocity.X = 0f;
				}
				if (ui != null)
				{
					if (XYWH.Y - height < 544 + 16)
					{
						ui.SetTriggerState(Trigger.HighestPosition);
					}
					else if (XYWH.Y + height > Main.BottomWorld - 544 - 32 - height)
					{
						ui.SetTriggerState(Trigger.LowestPosition);
					}
				}
				if (Position.Y < 544f + 16f)
				{
					Position.Y = 544f + 16f;
					if (velocity.Y < 0.11)
					{
						velocity.Y = 0.11f;
					}
				}
				else if (Position.Y > Main.BottomWorld - 544 - 32 - height)
				{
					Position.Y = Main.BottomWorld - 544 - 32 - height;
					velocity.Y = 0f;
				}
				XYWH.X = (int)Position.X;
				XYWH.Y = (int)Position.Y;
				ItemCheck(i);
				PlayerFrame();
				if (statLife > StatLifeMax)
				{
					statLife = StatLifeMax;
				}
				grappling[0] = -1;
				grapCount = 0;
			}
			if (!isLocal() || Main.NetMode < 1)
			{
				return;
			}
			NetPlayer netPlayer2 = ui.netPlayer;
			bool flag13 = false;
			for (int num69 = 0; num69 <= MaxNumInventory; num69++)
			{
				if (Inventory[num69].IsNotTheSameAs(ref netPlayer2.inventory[num69]))
				{
					ref Item reference4 = ref netPlayer2.inventory[num69];
					reference4 = Inventory[num69];
					NetMessage.CreateMessage2(5, i, num69);
					NetMessage.SendMessage();
				}
			}
			for (int num70 = 0; num70 < MaxNumArmor; num70++)
			{
				if (armor[num70].IsNotTheSameAs(ref netPlayer2.armor[num70]))
				{
					ref Item reference5 = ref netPlayer2.armor[num70];
					reference5 = armor[num70];
					NetMessage.CreateMessage2(5, i, num70 + MaxNumInventory + 1);
					NetMessage.SendMessage();
				}
			}
			if (PlayerChest != netPlayer2.chest)
			{
				netPlayer2.chest = PlayerChest;
				NetMessage.CreateMessage2(33, i, PlayerChest);
				NetMessage.SendMessage();
			}
			if (TalkNPC != netPlayer2.talkNPC)
			{
				netPlayer2.talkNPC = TalkNPC;
				NetMessage.CreateMessage1(40, i);
				NetMessage.SendMessage();
			}
			if (ZoneEvil != netPlayer2.zoneEvil)
			{
				netPlayer2.zoneEvil = ZoneEvil;
				flag13 = true;
			}
			if (ZoneMeteor != netPlayer2.zoneMeteor)
			{
				netPlayer2.zoneMeteor = ZoneMeteor;
				flag13 = true;
			}
			if (ZoneDungeon != netPlayer2.zoneDungeon)
			{
				netPlayer2.zoneDungeon = ZoneDungeon;
				flag13 = true;
			}
			if (ZoneJungle != netPlayer2.zoneJungle)
			{
				netPlayer2.zoneJungle = ZoneJungle;
				flag13 = true;
			}
			if (zoneHoly != netPlayer2.zoneHoly)
			{
				netPlayer2.zoneHoly = zoneHoly;
				flag13 = true;
			}
			if (flag13)
			{
				flag13 = false;
				NetMessage.CreateMessage1(36, i);
				NetMessage.SendMessage();
			}
			for (int num71 = 0; num71 < MaxNumBuffs; num71++)
			{
				if (buff[num71].Type != netPlayer2.buff[num71].Type)
				{
					netPlayer2.buff[num71].Type = buff[num71].Type;
					flag13 = true;
				}
			}
			if (flag13)
			{
				NetMessage.CreateMessage1(50, i);
				NetMessage.SendMessage();
				NetMessage.CreateMessage1(13, i);
				NetMessage.SendMessage();
			}
			if (ui.localGamer != null)
			{
				LeaderboardInfo.SubmitStatistics(ui.Statistics, ui.localGamer);
			}
		}

		private unsafe bool CanInteractWithTile(int x, int y)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			fixed (Tile* ptr = &Main.TileSet[num, num2])
			{
				if (ptr->IsActive == 0)
				{
					return false;
				}
				int type = ptr->Type;
				switch (type)
				{
				case 4:
				case 13:
				case 33:
				case 49:
					return !ui.UsingSmartCursor;
				case 10:
					return WorldGen.CanOpenDoor(num, num2);
				case 11:
					return WorldGen.CanCloseDoor(num, num2);
				case 21:
				case 29:
				case 97:
					if (TalkNPC == -1)
					{
						int num3 = -1;
						int frameX2 = ptr->FrameX;
						int frameY = ptr->FrameY;
						num -= (frameX2 / 18) & 1;
						num2 -= frameY / 18;
						switch (type)
						{
						case 29:
							num3 = -2;
							break;
						case 97:
							num3 = -3;
							break;
						}
						frameX2 = ptr->FrameX;
						if (Main.NetMode == (byte)NetModeSetting.CLIENT && num3 == -1 && (frameX2 < 72 || frameX2 > 106) && (frameX2 < 144 || frameX2 > 178))
						{
							return true;
						}
						if (num3 == -1)
						{
							bool flag = false;
							if ((frameX2 >= 72 && frameX2 <= 106) || (frameX2 >= 144 && frameX2 <= 178))
							{
								int num4 = 327;
								if (frameX2 >= 144 && frameX2 <= 178)
								{
									num4 = 329;
								}
								flag = true;
								for (int i = 0; i < MaxNumInventory; i++)
								{
									if (Inventory[i].Type == num4 && Inventory[i].Stack > 0)
									{
										return true;
									}
								}
							}
							if (!flag)
							{
								num3 = Chest.FindChest(num, num2);
							}
						}
						return num3 != -1;
					}
					return false;
				case 50:
					return !ui.UsingSmartCursor && ptr->FrameX == 90;
				case 55:
				case 85:
					return true;
				case 79:
					return true;
				case 104:
				case 125:
					return true;
				case 128:
				{
					int frameX = ptr->FrameX;
					frameX %= 100;
					frameX %= 36;
					if (frameX == 18)
					{
						frameX = ptr[-Main.LargeWorldH].FrameX;
					}
					if (frameX >= 100)
					{
						return true;
					}
					return false;
				}
				case 132:
				case 136:
				case 139:
				case 144:
					return true;
				}
			}
			return false;
		}

		private bool InteractWithTile(int x, int y)
		{
			int num = x >> 4;
			int num2 = y >> 4;
			if (Main.TileSet[num, num2].IsActive == 0)
			{
				return false;
			}
			int type = Main.TileSet[num, num2].Type;
			switch (type)
			{
			case 4:
			case 13:
			case 33:
			case 49:
				WorldGen.KillTile(num, num2);
				NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num, num2, 0);
				NetMessage.SendMessage();
				return true;
			case 10:
			{
				int num10 = WorldGen.OpenDoor(num, num2, direction);
				if (num10 != 0)
				{
					ui.TotalDoorsOpened++;
					NetMessage.CreateMessage3(19, num, num2, num10);
					NetMessage.SendMessage();
					return true;
				}
				return false;
			}
			case 11:
				if (WorldGen.CloseDoor(num, num2))
				{
					ui.TotalDoorsClosed++;
					NetMessage.CreateMessage2(24, num, num2);
					NetMessage.SendMessage();
					return true;
				}
				return false;
			case 21:
			case 29:
			case 97:
				if (TalkNPC == -1)
				{
					int num5 = -1;
					int frameX2 = Main.TileSet[num, num2].FrameX;
					int frameY = Main.TileSet[num, num2].FrameY;
					num -= (frameX2 / 18) & 1;
					num2 -= frameY / 18;
					switch (type)
					{
					case 29:
						num5 = -2;
						break;
					case 97:
						num5 = -3;
						break;
					default:
						if (frameX2 >= 216)
						{
							ui.chestText = Lang.ItemName(348);
						}
						else if (frameX2 >= 180)
						{
							ui.chestText = Lang.ItemName(343);
						}
						else
						{
							ui.chestText = Lang.ItemName(48);
						}
						break;
					}
					frameX2 = Main.TileSet[num, num2].FrameX;
					if (Main.NetMode == (byte)NetModeSetting.CLIENT && num5 == -1 && (frameX2 < 72 || frameX2 > 106) && (frameX2 < 144 || frameX2 > 178))
					{
						if (num == chestX && num2 == chestY && PlayerChest != -1)
						{
							PlayerChest = -1;
							Main.PlaySound(11);
						}
						else
						{
							NetMessage.CreateMessage3(31, WhoAmI, num, num2);
							NetMessage.SendMessage();
						}
						return true;
					}
					if (num5 == -1)
					{
						bool flag = false;
						if ((frameX2 >= 72 && frameX2 <= 106) || (frameX2 >= 144 && frameX2 <= 178))
						{
							int num6 = 327;
							if (frameX2 >= 144 && frameX2 <= 178)
							{
								num6 = 329;
							}
							flag = true;
							for (int i = 0; i < MaxNumInventory; i++)
							{
								if (Inventory[i].Type != num6 || Inventory[i].Stack <= 0)
								{
									continue;
								}
								if (num6 != 329)
								{
									Inventory[i].Stack--;
									if (Inventory[i].Stack <= 0)
									{
										Inventory[i].Init();
									}
								}
									Chest.Unlock(num, num2);
								NetMessage.CreateMessage3(52, WhoAmI, num, num2);
								NetMessage.SendMessage();
								return true;
							}
						}
						if (!flag)
						{
							num5 = Chest.FindChest(num, num2);
						}
					}
					if (num5 != -1)
					{
						if (num5 == PlayerChest)
						{
							PlayerChest = -1;
							Main.PlaySound(11);
						}
						else
						{
							if (num5 != PlayerChest && PlayerChest == -1)
							{
								Main.PlaySound(10);
							}
							else
							{
								Main.PlaySound(12);
							}
							PlayerChest = (short)num5;
							chestX = (short)num;
							chestY = (short)num2;
							ui.OpenInventory();
						}
						return true;
					}
				}
				return false;
			case 50:
				if (Main.TileSet[num, num2].FrameX != 90)
				{
					return false;
				}
				goto case 4;
			case 55:
			case 85:
			{
				bool flag2 = true;
				if (sign >= 0)
				{
					int num11 = Sign.ReadSign(num, num2);
					if (num11 == sign)
					{
						sign = -1;
						ui.npcChatText = null;
						ui.editSign = false;
						Main.PlaySound(11);
						flag2 = false;
					}
				}
				if (flag2)
				{
					if (Main.NetMode != (byte)NetModeSetting.CLIENT)
					{
						TalkNPC = -1;
						ui.CloseInventory();
						ui.editSign = false;
						Main.PlaySound(10);
						int num12 = Sign.ReadSign(num, num2);
						sign = (short)num12;
						ui.npcChatText = Main.SignSet[num12].SignString;
						ui.ClearButtonTriggers();
					}
					else
					{
						num -= (Main.TileSet[num, num2].FrameX / 18) & 1;
						num2 -= Main.TileSet[num, num2].FrameY / 18;
						type = Main.TileSet[num, num2].Type;
						if (type == 55 || type == 85)
						{
							NetMessage.CreateMessage3(46, WhoAmI, num, num2);
							NetMessage.SendMessage();
						}
					}
				}
				return true;
			}
			case 79:
			{
				int num3 = num;
				int num4 = num2;
				num3 -= Main.TileSet[num, num2].FrameX / 18;
				num3 = ((Main.TileSet[num, num2].FrameX < 72) ? (num3 + 2) : (num3 + 5));
				num4 -= Main.TileSet[num, num2].FrameY / 18;
				num4 += 2;
				if (CheckSpawn(num3, num4))
				{
					ChangeSpawn(num3, num4);
					Main.NewText(Lang.MenuText[57], 255, 240, 20);
					return true;
				}
				return false;
			}
			case 104:
			{
				string text = "AM";
				double num7 = Main.GameTime.WorldTime;
				if (!Main.GameTime.DayTime)
				{
					num7 += 54000.0;
				}
				num7 = num7 / 86400.0 * 24.0;
				num7 = num7 - 7.5 - 12.0;
				if (num7 < 0.0)
				{
					num7 += 24.0;
				}
				if (num7 >= 12.0)
				{
					text = "PM";
				}
				int num8 = (int)num7;
				int num9 = (int)((num7 - num8) * 60.0);
				string text2 = num9.ToStringLookup();
				if (num9 < 10)
				{
					text2 = "0" + text2;
				}
				if (num8 > 12)
				{
					num8 -= 12;
				}
				if (num8 == 0)
				{
					num8 = 12;
				}
				string newText = Lang.InterfaceText[34] + num8.ToStringLookup() + ":" + text2 + " " + text;
				Main.NewText(newText, 255, 240, 20);
				return true;
			}
			case 125:
				AddBuff((int)Buff.ID.CLARAVOYANCE, 36000);
				Main.PlaySound(2, XYWH.X, XYWH.Y, 4);
				return true;
			case 128:
			{
				int frameX = Main.TileSet[num, num2].FrameX;
				frameX %= 100;
				frameX %= 36;
				if (frameX == 18)
				{
					num--;
					frameX = Main.TileSet[num, num2].FrameX;
				}
				if (frameX >= 100)
				{
					WorldGen.KillTile(num, num2, KillToFail: true);
					NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num, num2, 1);
					NetMessage.SendMessage();
					return true;
				}
				return false;
			}
			case 132:
			case 136:
			case 144:
				WorldGen.HitSwitch(num, num2);
				NetMessage.CreateMessage2(59, num, num2);
				NetMessage.SendMessage();
				return true;
			case 139:
				Main.PlaySound(28, x, y, 0);
				WorldGen.SwitchMB(num, num2);
				return true;
			default:
				return false;
			}
		}

		public void NetClone(NetPlayer clonePlayer)
		{
			clonePlayer.zoneEvil = ZoneEvil;
			clonePlayer.zoneMeteor = ZoneMeteor;
			clonePlayer.zoneDungeon = ZoneDungeon;
			clonePlayer.zoneJungle = ZoneJungle;
			clonePlayer.zoneHoly = zoneHoly;
			clonePlayer.selectedItem = SelectedItem;
			clonePlayer.controlUp = controlUp;
			clonePlayer.controlDown = IsControlDown;
			clonePlayer.controlLeft = controlLeft;
			clonePlayer.controlRight = controlRight;
			clonePlayer.controlJump = controlJump;
			clonePlayer.controlUseItem = controlUseItem;
			clonePlayer.statLife = statLife;
			clonePlayer.statLifeMax = StatLifeMax;
			clonePlayer.statMana = statMana;
			clonePlayer.statManaMax = statManaMax;
			clonePlayer.chest = PlayerChest;
			clonePlayer.talkNPC = TalkNPC;
			for (int i = 0; i <= MaxNumInventory; i++)
			{
				ref Item reference = ref clonePlayer.inventory[i];
				reference = Inventory[i];
			}
			for (int j = 0; j < MaxNumArmor; j++)
			{
				ref Item reference2 = ref clonePlayer.armor[j];
				reference2 = armor[j];
			}
			for (int k = 0; k < MaxNumBuffs; k++)
			{
				clonePlayer.buff[k].Type = buff[k].Type;
			}
		}

		public bool SellItem(int price, int stack)
		{
			if (price <= 0)
			{
				return false;
			}
			Item[] array = new Item[MaxNumInventory];
			for (int i = 0; i < MaxNumInventory; i++)
			{
				ref Item reference = ref array[i];
				reference = Inventory[i];
			}
			int num = price / 5;
			num *= stack;
			if (num < 1)
			{
				num = 1;
			}
			bool flag = false;
			while (num >= 1000000 && !flag)
			{
				int num2 = -1;
				for (int num3 = NumInvSlots + NumCoinSlots - 1; num3 >= 0; num3--)
				{
					if (num2 == -1 && (Inventory[num3].Type == 0 || Inventory[num3].Stack == 0))
					{
						num2 = num3;
					}
					while (Inventory[num3].Type == (int)Item.ID.PLATINUM_COIN && Inventory[num3].Stack < Inventory[num3].MaxStack && num >= 1000000)
					{
						Inventory[num3].Stack++;
						num -= 1000000;
						DoCoins(num3);
						if (Inventory[num3].Stack == 0 && num2 == -1)
						{
							num2 = num3;
						}
					}
				}
				if (num >= 1000000)
				{
					if (num2 == -1)
					{
						flag = true;
						continue;
					}
					Inventory[num2].SetDefaults((int)Item.ID.PLATINUM_COIN);
					num -= 1000000;
				}
			}
			while (num >= 10000 && !flag)
			{
				int num4 = -1;
				for (int num5 = NumInvSlots + NumCoinSlots - 1; num5 >= 0; num5--)
				{
					if (num4 == -1 && (Inventory[num5].Type == 0 || Inventory[num5].Stack == 0))
					{
						num4 = num5;
					}
					while (Inventory[num5].Type == (int)Item.ID.GOLD_COIN && Inventory[num5].Stack < Inventory[num5].MaxStack && num >= 10000)
					{
						Inventory[num5].Stack++;
						num -= 10000;
						DoCoins(num5);
						if (Inventory[num5].Stack == 0 && num4 == -1)
						{
							num4 = num5;
						}
					}
				}
				if (num >= 10000)
				{
					if (num4 == -1)
					{
						flag = true;
						continue;
					}
					Inventory[num4].SetDefaults((int)Item.ID.GOLD_COIN);
					num -= 10000;
				}
			}
			while (num >= 100 && !flag)
			{
				int num6 = -1;
				for (int num7 = NumInvSlots + NumCoinSlots - 1; num7 >= 0; num7--)
				{
					if (num6 == -1 && (Inventory[num7].Type == 0 || Inventory[num7].Stack == 0))
					{
						num6 = num7;
					}
					while (Inventory[num7].Type == (int)Item.ID.SILVER_COIN && Inventory[num7].Stack < Inventory[num7].MaxStack && num >= 100)
					{
						Inventory[num7].Stack++;
						num -= 100;
						DoCoins(num7);
						if (Inventory[num7].Stack == 0 && num6 == -1)
						{
							num6 = num7;
						}
					}
				}
				if (num >= 100)
				{
					if (num6 == -1)
					{
						flag = true;
						continue;
					}
					Inventory[num6].SetDefaults((int)Item.ID.SILVER_COIN);
					num -= 100;
				}
			}
			while (num >= 1 && !flag)
			{
				int num8 = -1;
				for (int num9 = NumInvSlots + NumCoinSlots - 1; num9 >= 0; num9--)
				{
					if (num8 == -1 && (Inventory[num9].Type == 0 || Inventory[num9].Stack == 0))
					{
						num8 = num9;
					}
					while (Inventory[num9].Type == (int)Item.ID.COPPER_COIN && Inventory[num9].Stack < Inventory[num9].MaxStack && num >= 1)
					{
						Inventory[num9].Stack++;
						num--;
						DoCoins(num9);
						if (Inventory[num9].Stack == 0 && num8 == -1)
						{
							num8 = num9;
						}
					}
				}
				if (num >= 1)
				{
					if (num8 == -1)
					{
						flag = true;
						continue;
					}
					Inventory[num8].SetDefaults((int)Item.ID.COPPER_COIN);
					num--;
				}
			}
			if (flag)
			{
				for (int j = 0; j < MaxNumInventory; j++)
				{
					ref Item reference2 = ref Inventory[j];
					reference2 = array[j];
				}
				return false;
			}
			return true;
		}

		public bool BuyItem(int price)
		{
			if (price == 0)
			{
				return true;
			}
			int num = 0;
			int num2 = price;
			Item[] array = new Item[NumInvSlots + NumCoinSlots];
			for (int i = 0; i < NumInvSlots + NumCoinSlots; i++)
			{
				ref Item reference = ref array[i];
				reference = Inventory[i];
				if (Inventory[i].Type == (int)Item.ID.COPPER_COIN)
				{
					num += Inventory[i].Stack;
				}
				if (Inventory[i].Type == (int)Item.ID.SILVER_COIN)
				{
					num += Inventory[i].Stack * 100;
				}
				if (Inventory[i].Type == (int)Item.ID.GOLD_COIN)
				{
					num += Inventory[i].Stack * 10000;
				}
				if (Inventory[i].Type == (int)Item.ID.PLATINUM_COIN)
				{
					num += Inventory[i].Stack * 1000000;
				}
			}
			if (num >= price)
			{
				num2 = price;
				while (num2 > 0)
				{
					if (num2 >= 1000000)
					{
						for (int j = 0; j < NumInvSlots + NumCoinSlots; j++)
						{
							if (Inventory[j].Type != (int)Item.ID.PLATINUM_COIN)
							{
								continue;
							}
							while (Inventory[j].Stack > 0 && num2 >= 1000000)
							{
								num2 -= 1000000;
								Inventory[j].Stack--;
								if (Inventory[j].Stack == 0)
								{
									Inventory[j].Init();
								}
							}
						}
					}
					if (num2 >= 10000)
					{
						for (int k = 0; k < NumInvSlots + NumCoinSlots; k++)
						{
							if (Inventory[k].Type != (int)Item.ID.GOLD_COIN)
							{
								continue;
							}
							while (Inventory[k].Stack > 0 && num2 >= 10000)
							{
								num2 -= 10000;
								Inventory[k].Stack--;
								if (Inventory[k].Stack == 0)
								{
									Inventory[k].Init();
								}
							}
						}
					}
					if (num2 >= 100)
					{
						for (int l = 0; l < NumInvSlots + NumCoinSlots; l++)
						{
							if (Inventory[l].Type != (int)Item.ID.SILVER_COIN)
							{
								continue;
							}
							while (Inventory[l].Stack > 0 && num2 >= 100)
							{
								num2 -= 100;
								Inventory[l].Stack--;
								if (Inventory[l].Stack == 0)
								{
									Inventory[l].Init();
								}
							}
						}
					}
					if (num2 >= 1)
					{
						for (int m = 0; m < NumInvSlots + NumCoinSlots; m++)
						{
							if (Inventory[m].Type != (int)Item.ID.COPPER_COIN)
							{
								continue;
							}
							while (Inventory[m].Stack > 0 && num2 >= 1)
							{
								num2--;
								Inventory[m].Stack--;
								if (Inventory[m].Stack == 0)
								{
									Inventory[m].Init();
								}
							}
						}
					}
					if (num2 <= 0)
					{
						continue;
					}
					int num3 = -1;
					for (int num4 = NumInvSlots + NumCoinSlots - 1; num4 >= 0; num4--)
					{
						if (Inventory[num4].Type == 0 || Inventory[num4].Stack == 0)
						{
							num3 = num4;
							break;
						}
					}
					if (num3 >= 0)
					{
						bool flag = true;
						if (num2 >= 10000)
						{
							for (int n = 0; n < MaxNumInventory; n++)
							{
								if (Inventory[n].Type == (int)Item.ID.PLATINUM_COIN && Inventory[n].Stack >= 1)
								{
									Inventory[n].Stack--;
									if (Inventory[n].Stack == 0)
									{
										Inventory[n].Init();
									}
									Inventory[num3].SetDefaults((int)Item.ID.GOLD_COIN, 100);
									flag = false;
									break;
								}
							}
						}
						else if (num2 >= 100)
						{
							for (int num5 = 0; num5 < NumInvSlots + NumCoinSlots; num5++)
							{
								if (Inventory[num5].Type == (int)Item.ID.GOLD_COIN && Inventory[num5].Stack >= 1)
								{
									Inventory[num5].Stack--;
									if (Inventory[num5].Stack == 0)
									{
										Inventory[num5].Init();
									}
									Inventory[num3].SetDefaults((int)Item.ID.SILVER_COIN, 100);
									flag = false;
									break;
								}
							}
						}
						else if (num2 >= 1)
						{
							for (int num6 = 0; num6 < NumInvSlots + NumCoinSlots; num6++)
							{
								if (Inventory[num6].Type == (int)Item.ID.SILVER_COIN && Inventory[num6].Stack >= 1)
								{
									Inventory[num6].Stack--;
									if (Inventory[num6].Stack == 0)
									{
										Inventory[num6].Init();
									}
									Inventory[num3].SetDefaults((int)Item.ID.COPPER_COIN, 100);
									flag = false;
									break;
								}
							}
						}
						if (!flag)
						{
							continue;
						}
						if (num2 < 10000)
						{
							for (int num7 = 0; num7 < NumInvSlots + NumCoinSlots; num7++)
							{
								if (Inventory[num7].Type == (int)Item.ID.GOLD_COIN && Inventory[num7].Stack >= 1)
								{
									Inventory[num7].Stack--;
									if (Inventory[num7].Stack == 0)
									{
										Inventory[num7].Init();
									}
									Inventory[num3].SetDefaults((int)Item.ID.SILVER_COIN, 100);
									flag = false;
									break;
								}
							}
						}
						if (!flag || num2 >= 1000000)
						{
							continue;
						}
						for (int num8 = 0; num8 < NumInvSlots + NumCoinSlots; num8++)
						{
							if (Inventory[num8].Type == (int)Item.ID.PLATINUM_COIN && Inventory[num8].Stack >= 1)
							{
								Inventory[num8].Stack--;
								if (Inventory[num8].Stack == 0)
								{
									Inventory[num8].Init();
								}
								Inventory[num3].SetDefaults((int)Item.ID.GOLD_COIN, 100);
								flag = false;
								break;
							}
						}
						continue;
					}
					for (int num9 = 0; num9 < NumInvSlots + NumCoinSlots; num9++)
					{
						ref Item reference2 = ref Inventory[num9];
						reference2 = array[num9];
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public void AdjTiles()
		{
			for (int i = 0; i < Main.MaxNumTilenames; i++)
			{
				adjTile[i].old = adjTile[i].i;
				adjTile[i].i = false;
			}
			oldAdjWater = adjWater;
			adjWater = false;
			int num = (XYWH.X + (width / 2)) >> 4;
			int num2 = (XYWH.Y + height) >> 4;
			for (int j = num - 4; j <= num + 4; j++)
			{
				for (int k = num2 - 3; k < num2 + 3; k++)
				{
					if (Main.TileSet[j, k].IsActive != 0)
					{
						int type = Main.TileSet[j, k].Type;
						if (type < Main.MaxNumTilenames)
						{
							adjTile[type].i = true;

							FoundCraftingStation(type);
							switch (type)
							{
							case 77:
								adjTile[17].i = true;
								craftingStationsFound.Set(17, true);
								break;
							case 133:
								adjTile[17].i = true;
								adjTile[77].i = true;
								craftingStationsFound.Set(17, true);
								craftingStationsFound.Set(77, true);
								break;
							case 134:
								adjTile[16].i = true;
								craftingStationsFound.Set(16, true);
								break;
							}
						}
					}
					if (Main.TileSet[j, k].Liquid > 200 && Main.TileSet[j, k].Lava == 0)
					{
						adjWater = true;
					}
				}
			}
		}

		public unsafe void PlayerFrame()
		{
			if (swimTime > 0)
			{
				if (!IsWet)
				{
					swimTime = 0;
				}
				else
				{
					swimTime--;
				}
			}
			head = armor[0].HeadSlot;
			body = armor[1].BodySlot;
			legs = armor[2].LegSlot;
			if (merman)
			{
				head = 39;
				legs = 21;
				body = 22;
			}
			else if (wereWolf)
			{
				legs = 20;
				body = 21;
				head = 38;
			}
			else
			{
				int num = 0;
				if (armor[8].HeadSlot >= 0)
				{
					head = armor[8].HeadSlot;
					num++;
				}
				if (armor[9].BodySlot >= 0)
				{
					body = armor[9].BodySlot;
					num++;
				}
				if (armor[10].LegSlot >= 0)
				{
					legs = armor[10].LegSlot;
					num++;
				}
				if (num == 3 && ui != null)
				{
					ui.SetTriggerState(Trigger.AllVanitySlotsEquipped);
				}
			}
			if (head == 5 && body == 5 && legs == 5)
			{
				if (Main.Rand.Next(16) == 0)
				{
					Main.DustSet.NewDust(14, ref XYWH, 0.0, 0.0, 200, default, 1.2f);
				}
				socialShadow = true;
			}
			else
			{
				socialShadow = false;
				if (head == 6 && body == 6 && legs == 6)
				{
					if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > 1f && !rocketFrame)
					{
						for (int i = 0; i < 2; i++)
						{
							Dust* ptr = Main.DustSet.NewDust((int)(Position.X - velocity.X * 2f), (int)(Position.Y - velocity.Y * 2f) - 2, width, height, 6, 0.0, 0.0, 100, default, 2.0);
							if (ptr == null)
							{
								break;
							}
							ptr->NoGravity = true;
							ptr->NoLight = true;
							ptr->Velocity.X -= velocity.X * 0.5f;
							ptr->Velocity.Y -= velocity.Y * 0.5f;
						}
					}
				}
				else if (head == 7 && body == 7 && legs == 7)
				{
					boneArmor = true;
				}
				else if (head == 8 && body == 8 && legs == 8)
				{
					if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > 1f)
					{
						Dust* ptr2 = Main.DustSet.NewDust((int)(Position.X - velocity.X * 2f), (int)(Position.Y - velocity.Y * 2f) - 2, width, height, 40, 0.0, 0.0, 50, default, 1.4f);
						if (ptr2 != null)
						{
							ptr2->NoGravity = true;
							ptr2->Velocity *= 0.25f;
						}
					}
				}
				else if (head == 9 && body == 9 && legs == 9)
				{
					if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) > 1f && !rocketFrame)
					{
						for (int j = 0; j < 2; j++)
						{
							Dust* ptr3 = Main.DustSet.NewDust((int)(Position.X - velocity.X * 2f), (int)(Position.Y - velocity.Y * 2f) - 2, width, height, 6, 0.0, 0.0, 100, default, 2);
							if (ptr3 == null)
							{
								break;
							}
							ptr3->NoGravity = true;
							ptr3->NoLight = true;
							ptr3->Velocity.X -= velocity.X * 0.5f;
							ptr3->Velocity.Y -= velocity.Y * 0.5f;
						}
					}
				}
				else if (body == 18 && legs == 17)
				{
					if ((head == 32 || head == 33 || head == 34) && Main.Rand.Next(16) == 0)
					{
						Dust* ptr4 = Main.DustSet.NewDust((int)(Position.X - velocity.X * 2f), (int)(Position.Y - velocity.Y * 2f) - 2, width, height, 43, 0.0, 0.0, 100, default, 0.3f);
						if (ptr4 != null)
						{
							ptr4->FadeIn = 0.8f;
							ptr4->Velocity.X = 0f;
							ptr4->Velocity.Y = 0f;
						}
					}
				}
				else if (body == 24 && legs == 23 && (head == 42 || head == 43 || head == 41) && (velocity.X != 0f || velocity.Y != 0f) && Main.Rand.Next(16) == 0)
				{
					Dust* ptr5 = Main.DustSet.NewDust((int)(Position.X - velocity.X * 2f), (int)(Position.Y - velocity.Y * 2f) - 2, width, height, 43, 0.0, 0.0, 100, default, 0.3f);
					if (ptr5 != null)
					{
						ptr5->FadeIn = 0.8f;
						ptr5->Velocity.X = 0f;
						ptr5->Velocity.Y = 0f;
					}
				}
			}
			if (itemAnimation > 0 && Inventory[SelectedItem].UseStyle != 10)
			{
				if (Inventory[SelectedItem].UseStyle == 1 || Inventory[SelectedItem].Type == 0)
				{
					if (itemAnimation < itemAnimationMax / 3)
					{
						bodyFrameY = 168;
					}
					else if (itemAnimation < (itemAnimationMax << 1) / 3)
					{
						bodyFrameY = 112;
					}
					else
					{
						bodyFrameY = 56;
					}
				}
				else if (Inventory[SelectedItem].UseStyle == 2)
				{
					if (itemAnimation > itemAnimationMax >> 1)
					{
						bodyFrameY = 168;
					}
					else
					{
						bodyFrameY = 112;
					}
				}
				else if (Inventory[SelectedItem].UseStyle == 3)
				{
					if (itemAnimation > (itemAnimationMax << 1) / 3)
					{
						bodyFrameY = 168;
					}
					else
					{
						bodyFrameY = 168;
					}
				}
				else if (Inventory[SelectedItem].UseStyle == 4)
				{
					bodyFrameY = 112;
				}
				else if (Inventory[SelectedItem].UseStyle == 5)
				{
					if (Inventory[SelectedItem].Type == (int)Item.ID.BLOWPIPE)
					{
						bodyFrameY = 112;
					}
					else
					{
						float num2 = itemRotation * direction;
						bodyFrameY = 168;
						if ((double)num2 < -0.75)
						{
							bodyFrameY = 112;
							if (gravDir == -1)
							{
								bodyFrameY = 224;
							}
						}
						if ((double)num2 > 0.6)
						{
							bodyFrameY = 224;
							if (gravDir == -1)
							{
								bodyFrameY = 112;
							}
						}
					}
				}
			}
			else if (Inventory[SelectedItem].HoldStyle == 1 && (!IsWet || !Inventory[SelectedItem].CantTouchLiquid))
			{
				bodyFrameY = 168;
			}
			else if (Inventory[SelectedItem].HoldStyle == 2 && (!IsWet || !Inventory[SelectedItem].CantTouchLiquid))
			{
				bodyFrameY = 112;
			}
			else if (Inventory[SelectedItem].HoldStyle == 3)
			{
				bodyFrameY = 168;
			}
			else if (grappling[0] >= 0)
			{
				Vector2 vector = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
				float num3 = 0f;
				float num4 = 0f;
				for (int k = 0; k < grapCount; k++)
				{
					num3 += Main.ProjectileSet[grappling[k]].position.X + (Main.ProjectileSet[grappling[k]].width >> 1);
					num4 += Main.ProjectileSet[grappling[k]].position.Y + (Main.ProjectileSet[grappling[k]].height >> 1);
				}
				num3 /= grapCount;
				num4 /= grapCount;
				num3 -= vector.X;
				num4 -= vector.Y;
				if (num4 < 0f && Math.Abs(num4) > Math.Abs(num3))
				{
					bodyFrameY = 112;
					if (gravDir == -1)
					{
						bodyFrameY = 224;
					}
				}
				else if (num4 > 0f && Math.Abs(num4) > Math.Abs(num3))
				{
					bodyFrameY = 224;
					if (gravDir == -1)
					{
						bodyFrameY = 112;
					}
				}
				else
				{
					bodyFrameY = 168;
				}
			}
			else if (swimTime > 0)
			{
				if (swimTime > 20)
				{
					bodyFrameY = 0;
				}
				else if (swimTime > 10)
				{
					bodyFrameY = 280;
				}
				else
				{
					bodyFrameY = 0;
				}
			}
			else if (velocity.Y != 0f)
			{
				if (wings > 0)
				{
					if (velocity.Y > 0f)
					{
						if (controlJump)
						{
							bodyFrameY = 336;
						}
						else
						{
							bodyFrameY = 280;
						}
					}
					else
					{
						bodyFrameY = 336;
					}
				}
				else
				{
					bodyFrameY = 280;
				}
				bodyFrameCounter = 0f;
			}
			else if (velocity.X != 0f)
			{
				bodyFrameCounter += Math.Abs(velocity.X) * 1.5f;
				bodyFrameY = legFrameY;
			}
			else
			{
				bodyFrameCounter = 0f;
				bodyFrameY = 0;
			}
			if (swimTime > 0)
			{
				legFrameCounter += 2f;
				while (legFrameCounter > 8f)
				{
					legFrameCounter -= 8f;
					legFrameY += 56;
				}
				if (legFrameY < 392)
				{
					legFrameY = 1064;
				}
				else if (legFrameY > 1064)
				{
					legFrameY = 392;
				}
				ResetAirTime();
			}
#if (!VERSION_INITIAL || IS_PATCHED)
            else if (grappling[0] >= 0)
			{
                legFrameCounter = 0f;
                legFrameY = 280;
            }
            else if (velocity.Y != 0f)
            {
#else
			else if (velocity.Y != 0f || grappling[0] >= 0)
			{
#endif
                IncreaseAirTime();
				legFrameCounter = 0f;
				legFrameY = 280;
			}
			else if (velocity.X != 0f)
			{
				legFrameCounter += Math.Abs(velocity.X) * 1.3f;
				int num5 = (int)legFrameCounter >> 3;
				if (num5 > 0)
				{
					legFrameCounter -= num5 << 3;
					legFrameY = (short)(legFrameY + 56 * num5);
					if (legFrameY == 560 || legFrameY == 784 || legFrameY == 1008)
					{
						IncreaseSteps();
					}
				}
				if (legFrameY < 392)
				{
					legFrameY = 1064;
				}
				else if (legFrameY > 1064)
				{
					legFrameY = 392;
				}
				ResetAirTime();
			}
			else
			{
				legFrameCounter = 0f;
				legFrameY = 0;
				ResetAirTime();
			}
		}

		public void Init()
		{
			velocity = default;
			headPosition = default;
			bodyPosition = default;
			legPosition = default;
			headRotation = 0f;
			bodyRotation = 0f;
			legRotation = 0f;
			immune = true;
			immuneTime = 0;
			IsDead = false;
			IsWet = false;
			wetCount = 0;
			lavaWet = false;
			TalkNPC = -1;
		}

		public void Spawn()
		{
			Init();
			if (isLocal())
			{
				CurrentView.quickBG = 10;
				FindSpawn();
				if (!CheckSpawn(SpawnX, SpawnY))
				{
					SpawnX = -1;
					SpawnY = -1;
				}
#if (!VERSION_INITIAL || IS_PATCHED)
				netForceControlUpdate = NetForceUpdateTimer;
#endif
				NetMessage.CreateMessage1(12, WhoAmI);
				NetMessage.SendMessage();
			}
			if (SpawnX >= 0 && SpawnY >= 0)
			{
				Position.X = SpawnX * 16 + 8 - (width / 2);
				Position.Y = SpawnY * 16 - height;
			}
			else
			{
				Position.X = Main.SpawnTileX * 16 + 8 - (width / 2);
				Position.Y = Main.SpawnTileY * 16 - height;
				for (int i = Main.SpawnTileX - 1; i < Main.SpawnTileX + 2; i++)
				{
					for (int j = Main.SpawnTileY - 3; j < Main.SpawnTileY; j++)
					{
						if (Main.TileSolidNotSolidTop[Main.TileSet[i, j].Type])
						{
							WorldGen.KillTile(i, j);
						}
						if (Main.TileSet[i, j].Liquid > 0)
						{
							Main.TileSet[i, j].Lava = 0;
							Main.TileSet[i, j].Liquid = 0;
							WorldGen.SquareTileFrame(i, j);
						}
					}
				}
			}
			ref Vector2 reference = ref shadowPos[0];
			reference = Position;
			ref Vector2 reference2 = ref shadowPos[1];
			reference2 = Position;
			ref Vector2 reference3 = ref shadowPos[2];
			reference3 = Position;
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
			fallStart = (short)(XYWH.Y >> 4);
			if (statLife <= 0)
			{
				breath = MaxBreath;
				if (spawnMax)
				{
					statLife = StatLifeMax;
					statMana = statManaMax2;
				}
				else
				{
					statLife = 100;
				}
				healthBarLife = statLife;
			}
			if (pvpDeath)
			{
				pvpDeath = false;
				immuneTime = 300;
				healthBarLife = (statLife = StatLifeMax);
			}
			else
			{
				immuneTime = 60;
			}
			if (isLocal())
			{
				hellAndBackState = 0;
				ui.worldFade = -0.25f;
				ui.worldFadeTarget = 1f;
				CurrentView.Lighting.ScreenX = -1;
				updateScreenPosition();
				UpdateMouse();
				UpdatePlayer(WhoAmI);
			}
			Active = 1;
		}

		public unsafe double Hurt(int Damage, int hitDirection, bool pvp, bool quiet, uint deathText, bool Crit = false)
		{
			if (!immune)
			{
				int num = Damage;
				if (pvp)
				{
					num <<= 1;
				}
				double num2 = Main.CalculateDamage(num, StatDefense);
				if (Crit)
				{
					num <<= 1;
				}
				if (num2 >= 1.0)
				{
					if (isLocal() && !quiet)
					{
						NetMessage.CreateMessage1(13, WhoAmI);
						NetMessage.SendMessage();
						NetMessage.CreateMessage1(16, WhoAmI);
						NetMessage.SendMessage();
						NetMessage.SendPlayerHurt(WhoAmI, hitDirection, Damage, pvp, critical: false, deathText);
					}

#if USE_ORIGINAL_CODE
					CombatText.NewText(Position, width, height, (int)num2, Crit);
#else
					CombatText.NewText(Position, width, height, (int)num2, Crit, 1);
#endif

					statLife -= (short)num2;
					immune = true;
					immuneTime = 40;
					if (longInvince)
					{
						immuneTime += 40;
					}
					lifeRegenTime = 0;
					if (pvp)
					{
						immuneTime = 8;
					}
					if (isLocal() && starCloak)
					{
						for (int i = 0; i < 3; i++)
						{
							float num3 = Position.X + Main.Rand.Next(-400, 400);
							float num4 = Position.Y - Main.Rand.Next(500, 800);
							float num5 = Position.X + (width / 2) - num3;
							float num6 = Position.Y + (height / 2) - num4;
							num5 += Main.Rand.Next(-100, 101);
							float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
							num7 = 23f / num7;
							num5 *= num7;
							num6 *= num7;
							int num8 = Projectile.NewProjectile(num3, num4, num5, num6, 92, 30, 5f, WhoAmI);
							if (num8 < 0)
							{
								break;
							}
							Main.ProjectileSet[num8].ai1 = XYWH.Y;
						}
					}
					if (!noKnockback && hitDirection != 0)
					{
						velocity.X = 4.5f * hitDirection;
						velocity.Y = -3.5f;
					}
					if (wereWolf)
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y, 6);
					}
					else if (boneArmor)
					{
						Main.PlaySound(3, XYWH.X, XYWH.Y, 2);
					}
					else
					{
						Main.PlaySound(male ? 1 : 20, XYWH.X, XYWH.Y);
					}
					if (statLife > 0)
					{
						for (int num9 = (int)(num2 / StatLifeMax * 80.0); num9 > 0; num9--)
						{
							Main.DustSet.NewDust(boneArmor ? 26 : 5, ref XYWH, 2 * hitDirection, -2.0);
						}
					}
					else if (Main.IsTutorial())
					{
						statLife = 1;
					}
					else
					{
						statLife = 0;
						if (isLocal())
						{
							KillMe(num2, hitDirection, pvp, deathText);
						}
					}
				}
				if (pvp)
				{
					num2 = Main.CalculateDamage(num, StatDefense);
				}
				return num2;
			}
			return 0.0;
		}

		public void KillMeForGood()
		{
			ui.ErasePlayer(ui.selectedPlayer);
			ui.playerPathName = null;
		}

		public unsafe void KillMe(double dmg, int hitDirection, bool pvp, uint deathText)
		{
			if (IsDead)
			{
				return;
			}
			if (pvp)
			{
				pvpDeath = true;
			}
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				float num;
				for (num = Main.Rand.Next(-35, 36) * 0.1f; num < 2f && num > -2f; num += Main.Rand.Next(-30, 31) * 0.1f)
				{
				}
				int num2 = Projectile.NewProjectile(Position.X + (width / 2), Position.Y + (head >> 1), Main.Rand.Next(10, 30) * 0.1f * hitDirection + num, Main.Rand.Next(-40, -20) * 0.1f, 43, 0, 0f, WhoAmI);
				if (num2 >= 0)
				{
					uint num3 = Projectile.tombstoneTextIndex++ & 7;
#if VERSION_INITIAL
					Projectile.tombstoneText[num3] = Name + Lang.DeathMsgString(deathText);
#else
					Projectile.tombstoneText[num3] = CharacterName + Lang.DeathMsgString(deathText);
#endif
					Main.ProjectileSet[num2].tombstoneTextId = (byte)num3;
				}
			}
			if (difficulty != (int)Difficulty.SOFTCORE && isLocal())
			{
				ui.trashItem.Init();
				DropItems();
				if (difficulty == (int)Difficulty.HARDCORE)
				{
					KillMeForGood();
				}
			}
			Main.PlaySound(5, XYWH.X, XYWH.Y);
			headVelocity.Y = Main.Rand.Next(-40, -10) * 0.1f;
			bodyVelocity.Y = Main.Rand.Next(-40, -10) * 0.1f;
			legVelocity.Y = Main.Rand.Next(-40, -10) * 0.1f;
			headVelocity.X = Main.Rand.Next(-20, 21) * 0.1f + 2 * hitDirection;
			bodyVelocity.X = Main.Rand.Next(-20, 21) * 0.1f + 2 * hitDirection;
			legVelocity.X = Main.Rand.Next(-20, 21) * 0.1f + 2 * hitDirection;
			for (int i = 0; i < 16.0 + dmg / StatLifeMax * 100.0; i++)
			{
				Main.DustSet.NewDust(boneArmor ? 26 : 5, ref XYWH, hitDirection << 1, -2.0);
			}
			IsDead = true;
			respawnTimer = 420;
			immuneAlpha = 0;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
#if VERSION_INITIAL
				NetMessage.SendDeathText(Name, deathText, 225, 25, 25);
#else
				NetMessage.SendDeathText(CharacterName, deathText, 225, 25, 25);
#endif
			}
			if (isLocal())
			{
				NetMessage.CreateMessage5((int)NetMessageId.UNK_MSG_44, WhoAmI, hitDirection, (int)dmg, pvp ? 1 : 0, (int)deathText);
				NetMessage.SendMessage();
				if (!pvp && difficulty == (int)Difficulty.SOFTCORE)
				{
					DropCoins();
				}
			}
		}

		public unsafe bool ItemSpace(Item* pNewItem)
		{
			Item.ID type = (Item.ID)pNewItem->Type;
			if (type == Item.ID.HEART || type == Item.ID.STAR)
			{
				return true;
			}
			int num = NumInvSlots;
			if (type == Item.ID.COPPER_COIN || type == Item.ID.SILVER_COIN || type == Item.ID.GOLD_COIN || type == Item.ID.PLATINUM_COIN)
			{
				num += NumCoinSlots;
			}
			for (int i = 0; i < num; i++)
			{
				if (Inventory[i].Type == 0)
				{
					return true;
				}
				if (Inventory[i].Stack < Inventory[i].MaxStack && pNewItem->NetID == Inventory[i].NetID)
				{
					return true;
				}
			}
			if (pNewItem->CanBePlacedInAmmoSlot())
			{
				for (int j = MaxNumInventory - NumAmmoSlots; j < MaxNumInventory; j++)
				{
					if (Inventory[j].Type == 0 && pNewItem->CanBeAutoPlacedInEmptyAmmoSlot())
					{
						return true;
					}
					if (Inventory[j].Stack < Inventory[j].MaxStack && pNewItem->NetID == Inventory[j].NetID)
					{
						return true;
					}
				}
			}
			else
			{
				if (pNewItem->IsAccessory)
				{
					for (int k = NumArmorSlots; k < NumArmorSlots + NumEquipSlots; k++)
					{
						if (armor[k].NetID == pNewItem->NetID)
						{
							return false;
						}
					}
					for (int l = NumArmorSlots; l < NumArmorSlots + NumEquipSlots; l++)
					{
						if (armor[l].Type == 0)
						{
							return true;
						}
					}
					return false;
				}
				if (pNewItem->HeadSlot >= 0)
				{
					if (armor[0].Type != 0)
					{
						return armor[NumArmorSlots + NumEquipSlots].Type == 0;
					}
					return true;
				}
				if (pNewItem->BodySlot >= 0)
				{
					if (armor[1].Type != 0)
					{
						return armor[NumArmorSlots + NumEquipSlots + 1].Type == 0;
					}
					return true;
				}
				if (pNewItem->LegSlot >= 0)
				{
					if (armor[2].Type != 0)
					{
						return armor[NumArmorSlots + NumEquipSlots + 2].Type == 0;
					}
					return true;
				}
			}
			return false;
		}

		public void DoCoins(int i)
		{
			if (Inventory[i].Stack != 100 || (Inventory[i].Type != (int)Item.ID.COPPER_COIN && Inventory[i].Type != (int)Item.ID.SILVER_COIN && Inventory[i].Type != (int)Item.ID.GOLD_COIN))
			{
				return;
			}
			Inventory[i].SetDefaults(Inventory[i].Type + 1);
			for (int j = 0; j < NumInvSlots + NumCoinSlots; j++)
			{
				if (Inventory[j].NetID == Inventory[i].NetID && j != i && Inventory[j].Stack < Inventory[j].MaxStack)
				{
					Inventory[j].Stack++;
					Inventory[i].Init();
					DoCoins(j);
				}
			}
		}

		public bool AutoEquip(ref Item item)
		{
			int num = -1;
			bool flag = item.IsVanity;
			lock (this)
			{
				do
				{
					if (item.IsAccessory)
					{
						for (int i = NumArmorSlots; i < NumArmorSlots + NumEquipSlots; i++)
						{
							if (armor[i].NetID == item.NetID)
							{
								return false;
							}
						}
						for (int j = NumArmorSlots; j < NumArmorSlots + NumEquipSlots; j++)
						{
							if (armor[j].Type == 0)
							{
								num = j;
								break;
							}
						}
					}
					else if (item.HeadSlot >= 0)
					{
						num = (flag ? (NumArmorSlots + NumEquipSlots) : 0);
					}
					else if (item.BodySlot >= 0)
					{
						num = (flag ? (NumArmorSlots + NumEquipSlots + 1) : 1);
					}
					else if (item.LegSlot >= 0)
					{
						num = (flag ? (NumArmorSlots + NumEquipSlots + 2) : 2);
					}
					if (num >= 0 && armor[num].Type == 0)
					{
						ref Item reference = ref armor[num];
						reference = item;
						itemsFound.Set(item.Type, value: true);
						ui.FoundPotentialArmor(item.Type);
						item.Init();
						return true;
					}
					flag = !flag;
				}
				while (flag != item.IsVanity);
			}
			return false;
		}

		public bool FillAmmo(ref Item item)
		{
			bool result = false;
			lock (this)
			{
				for (int i = MaxNumInventory - NumAmmoSlots; i < MaxNumInventory; i++)
				{
					if (Inventory[i].Type > 0 && Inventory[i].Stack < Inventory[i].MaxStack && item.NetID == Inventory[i].NetID)
					{
						Main.PlaySound(7, XYWH.X, XYWH.Y);
						if (item.Stack + Inventory[i].Stack <= Inventory[i].MaxStack)
						{
							Inventory[i].Stack += item.Stack;
							CurrentView.itemTextLocal.NewText(ref item, item.Stack);
							item.Init();
							return true;
						}
						short num = (short)(Inventory[i].MaxStack - Inventory[i].Stack);
						item.Stack -= num;
						CurrentView.itemTextLocal.NewText(ref item, num);
						Inventory[i].Stack = Inventory[i].MaxStack;
						result = true;
					}
				}
				if (item.CanBeAutoPlacedInEmptyAmmoSlot())
				{
					for (int j = MaxNumInventory - NumAmmoSlots; j < MaxNumInventory; j++)
					{
						if (Inventory[j].Type == 0)
						{
							ref Item reference = ref Inventory[j];
							reference = item;
							itemsFound.Set(item.Type, value: true);
							CurrentView.itemTextLocal.NewText(ref item, item.Stack);
							Main.PlaySound(7, XYWH.X, XYWH.Y);
							item.Init();
							return true;
						}
					}
					return result;
				}
				return result;
			}
		}

		public bool GetItem(ref Item item)
		{
			if (item.NoGrabDelay > 0)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = isLocal() && ui.InventoryMode > 0 && ui.ActiveInvSection == UI.InventorySection.CRAFTING && ui.PadState.IsButtonUp(UI.BTN_INVENTORY_SELECT);
			int num = NumInvSlots;
			int num2 = 0;
			if (item.CanBePlacedInCoinSlot())
			{
				num2 -= NumCoinSlots;
				num += NumCoinSlots;
			}
			else if (item.CanBePlacedInAmmoSlot())
			{
				flag = FillAmmo(ref item);
				if (flag && (item.Type == 0 || item.Stack == 0))
				{
					if (flag2)
					{
						Recipe.FindRecipes(ui, ui.craftingCategory, ui.craftingShowCraftable);
					}
					return true;
				}
			}
			else if (AutoEquip(ref item))
			{
				return true;
			}
			lock (this)
			{
				for (int i = num2; i < NumInvSlots; i++)
				{
					int num3 = i;
					if (num3 < 0)
					{
						num3 += NumInvSlots + NumCoinSlots;
					}
					if (Inventory[num3].Type <= 0 || Inventory[num3].Stack >= Inventory[num3].MaxStack || item.NetID != Inventory[num3].NetID)
					{
						continue;
					}
					Main.PlaySound(7, XYWH.X, XYWH.Y);
					if (item.Stack + Inventory[num3].Stack <= Inventory[num3].MaxStack)
					{
						Inventory[num3].Stack += item.Stack;
						CurrentView.itemTextLocal.NewText(ref item, item.Stack);
						DoCoins(num3);
						item.Init();
						if (flag2)
						{
							Recipe.FindRecipes(ui, ui.craftingCategory, ui.craftingShowCraftable);
						}
						return true;
					}
					short num4 = (short)(Inventory[num3].MaxStack - Inventory[num3].Stack);
					item.Stack -= num4;
					CurrentView.itemTextLocal.NewText(ref item, num4);
					Inventory[num3].Stack = Inventory[num3].MaxStack;
					DoCoins(num3);
					flag = true;
				}
				if (item.UseStyle > 0)
				{
					for (int j = 0; j < 10; j++)
					{
						if (Inventory[j].Type == 0)
						{
							ref Item reference = ref Inventory[j];
							reference = item;
							itemsFound.Set(item.Type, value: true);
							CurrentView.itemTextLocal.NewText(ref item, item.Stack);
							DoCoins(j);
							Main.PlaySound(7, XYWH.X, XYWH.Y);
							item.Init();
							if (flag2)
							{
								Recipe.FindRecipes(ui, ui.craftingCategory, ui.craftingShowCraftable);
							}
							return true;
						}
					}
				}
				for (int num5 = num - 1; num5 >= 0; num5--)
				{
					if (Inventory[num5].Type == 0)
					{
						ref Item reference2 = ref Inventory[num5];
						reference2 = item;
						itemsFound.Set(item.Type, value: true);
						ui.FoundPotentialArmor(item.Type);
						CurrentView.itemTextLocal.NewText(ref item, item.Stack);
						DoCoins(num5);
						Main.PlaySound(7, XYWH.X, XYWH.Y);
						item.Init();
						if (flag2)
						{
							Recipe.FindRecipes(ui, ui.craftingCategory, ui.craftingShowCraftable);
						}
						return true;
					}
				}
			}
			if (flag2 && flag)
			{
				Recipe.FindRecipes(ui, ui.craftingCategory, ui.craftingShowCraftable);
			}
			return flag;
		}

		private void PlaceThing()
		{
			int createTile = Inventory[SelectedItem].CreateTile;
			if (createTile >= 0)
			{
				bool flag = false;
				if (Main.TileSet[tileTargetX, tileTargetY].Liquid > 0 && Main.TileSet[tileTargetX, tileTargetY].Lava != 0)
				{
					if (Main.TileSolid[createTile])
					{
						flag = true;
					}
					else if (Main.TileLavaDeath[createTile])
					{
						flag = true;
					}
				}
				if (((Main.TileSet[tileTargetX, tileTargetY].IsActive == 0 && !flag) || Main.TileCut[Main.TileSet[tileTargetX, tileTargetY].Type] || createTile == 23 || createTile == 2 || createTile == 109 || createTile == 60 || createTile == 70) && itemTime == 0 && itemAnimation > 0 && controlUseItem)
				{
					bool flag2 = false;
					switch (createTile)
					{
					case 2:
					case 23:
					case 109:
						if (Main.TileSet[tileTargetX, tileTargetY].IsActive != 0 && Main.TileSet[tileTargetX, tileTargetY].Type == 0)
						{
							flag2 = true;
						}
						break;
					case 60:
					case 70:
						if (Main.TileSet[tileTargetX, tileTargetY].IsActive != 0 && Main.TileSet[tileTargetX, tileTargetY].Type == 59)
						{
							flag2 = true;
						}
						break;
					case 4:
					case 136:
					{
						int num = Main.TileSet[tileTargetX, tileTargetY + 1].Type;
						int num2 = Main.TileSet[tileTargetX - 1, tileTargetY].Type;
						int num3 = Main.TileSet[tileTargetX + 1, tileTargetY].Type;
						int num4 = Main.TileSet[tileTargetX - 1, tileTargetY - 1].Type;
						int num5 = Main.TileSet[tileTargetX + 1, tileTargetY - 1].Type;
						int num6 = Main.TileSet[tileTargetX - 1, tileTargetY - 1].Type;
						int num7 = Main.TileSet[tileTargetX + 1, tileTargetY + 1].Type;
						if (Main.TileSet[tileTargetX, tileTargetY + 1].IsActive == 0)
						{
							num = -1;
						}
						if (Main.TileSet[tileTargetX - 1, tileTargetY].IsActive == 0)
						{
							num2 = -1;
						}
						if (Main.TileSet[tileTargetX + 1, tileTargetY].IsActive == 0)
						{
							num3 = -1;
						}
						if (Main.TileSet[tileTargetX - 1, tileTargetY - 1].IsActive == 0)
						{
							num4 = -1;
						}
						if (Main.TileSet[tileTargetX + 1, tileTargetY - 1].IsActive == 0)
						{
							num5 = -1;
						}
						if (Main.TileSet[tileTargetX - 1, tileTargetY + 1].IsActive == 0)
						{
							num6 = -1;
						}
						if (Main.TileSet[tileTargetX + 1, tileTargetY + 1].IsActive == 0)
						{
							num7 = -1;
						}
						if (num >= 0 && Main.TileSolidAndAttach[num])
						{
							flag2 = true;
						}
						else if (num2 >= 0 && (Main.TileSolidAndAttach[num2] || num2 == 124 || (num2 == 5 && num4 == 5 && num6 == 5)))
						{
							flag2 = true;
						}
						else if (num3 >= 0 && (Main.TileSolidAndAttach[num3] || num3 == 124 || (num3 == 5 && num5 == 5 && num7 == 5)))
						{
							flag2 = true;
						}
						break;
					}
					case 78:
					case 98:
					case 100:
						if (Main.TileSet[tileTargetX, tileTargetY + 1].IsActive != 0 && (Main.TileSolid[Main.TileSet[tileTargetX, tileTargetY + 1].Type] || Main.TileTable[Main.TileSet[tileTargetX, tileTargetY + 1].Type]))
						{
							flag2 = true;
						}
						break;
					case 13:
					case 29:
					case 33:
					case 49:
					case 50:
					case 103:
						if (Main.TileSet[tileTargetX, tileTargetY + 1].IsActive != 0 && Main.TileTable[Main.TileSet[tileTargetX, tileTargetY + 1].Type])
						{
							flag2 = true;
						}
						break;
					case 51:
						if (Main.TileSet[tileTargetX + 1, tileTargetY].IsActive != 0 || Main.TileSet[tileTargetX + 1, tileTargetY].WallType > 0 || Main.TileSet[tileTargetX - 1, tileTargetY].IsActive != 0 || Main.TileSet[tileTargetX - 1, tileTargetY].WallType > 0 || Main.TileSet[tileTargetX, tileTargetY + 1].IsActive != 0 || Main.TileSet[tileTargetX, tileTargetY + 1].WallType > 0 || Main.TileSet[tileTargetX, tileTargetY - 1].IsActive != 0 || Main.TileSet[tileTargetX, tileTargetY - 1].WallType > 0)
						{
							flag2 = true;
						}
						break;
					default:
						if ((Main.TileSet[tileTargetX + 1, tileTargetY].IsActive != 0 && Main.TileSolid[Main.TileSet[tileTargetX + 1, tileTargetY].Type]) || Main.TileSet[tileTargetX + 1, tileTargetY].WallType > 0 || (Main.TileSet[tileTargetX - 1, tileTargetY].IsActive != 0 && Main.TileSolid[Main.TileSet[tileTargetX - 1, tileTargetY].Type]) || Main.TileSet[tileTargetX - 1, tileTargetY].WallType > 0 || (Main.TileSet[tileTargetX, tileTargetY + 1].IsActive != 0 && (Main.TileSolid[Main.TileSet[tileTargetX, tileTargetY + 1].Type] || Main.TileSet[tileTargetX, tileTargetY + 1].Type == 124)) || Main.TileSet[tileTargetX, tileTargetY + 1].WallType > 0 || (Main.TileSet[tileTargetX, tileTargetY - 1].IsActive != 0 && (Main.TileSolid[Main.TileSet[tileTargetX, tileTargetY - 1].Type] || Main.TileSet[tileTargetX, tileTargetY - 1].Type == 124)) || Main.TileSet[tileTargetX, tileTargetY - 1].WallType > 0)
						{
							flag2 = true;
						}
						break;
					}
					if (createTile >= 82 && createTile <= 84)
					{
						flag2 = true;
					}
					if (Main.TileSet[tileTargetX, tileTargetY].IsActive != 0 && Main.TileCut[Main.TileSet[tileTargetX, tileTargetY].Type])
					{
						if (Main.TileSet[tileTargetX, tileTargetY + 1].Type != 78)
						{
							if (WorldGen.KillTile(tileTargetX, tileTargetY))
							{
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 4, tileTargetX, tileTargetY, 0);
								NetMessage.SendMessage();
							}
						}
						else
						{
							flag2 = false;
						}
					}
					if (flag2)
					{
						int num8 = Inventory[SelectedItem].PlaceStyle;
						if (createTile == 141)
						{
							num8 = Main.Rand.Next(2);
						}
						if (createTile == 128 || createTile == 137)
						{
							num8 = ((direction >= 0) ? 1 : (-1));
						}
						if (WorldGen.PlaceTile(tileTargetX, tileTargetY, createTile, ToMute: false, IsForced: false, WhoAmI, num8))
						{
							itemTime = Inventory[SelectedItem].UseTime;
							NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 1, tileTargetX, tileTargetY, createTile, num8);
							NetMessage.SendMessage();
							switch (createTile)
							{
							case 15:
								if (direction == 1)
								{
									Main.TileSet[tileTargetX, tileTargetY].FrameX += 18;
									Main.TileSet[tileTargetX, tileTargetY - 1].FrameX += 18;
								}
								NetMessage.SendTileSquare(tileTargetX - 1, tileTargetY - 1, 3);
								break;
							case 19:
								ui.TotalWoodPlatformsPlaced++;
								break;
							case 79:
							case 90:
								NetMessage.SendTileSquare(tileTargetX, tileTargetY, 5);
								break;
							}
						}
					}
				}
			}
			int createWall = Inventory[SelectedItem].CreateWall;
			if (createWall >= 0 && itemTime == 0 && itemAnimation > 0 && controlUseItem && (Main.TileSet[tileTargetX + 1, tileTargetY].IsActive != 0 || Main.TileSet[tileTargetX + 1, tileTargetY].WallType > 0 || Main.TileSet[tileTargetX - 1, tileTargetY].IsActive != 0 || Main.TileSet[tileTargetX - 1, tileTargetY].WallType > 0 || Main.TileSet[tileTargetX, tileTargetY + 1].IsActive != 0 || Main.TileSet[tileTargetX, tileTargetY + 1].WallType > 0 || Main.TileSet[tileTargetX, tileTargetY - 1].IsActive != 0 || Main.TileSet[tileTargetX, tileTargetY - 1].WallType > 0) && Main.TileSet[tileTargetX, tileTargetY].WallType != createWall && WorldGen.PlaceWall(tileTargetX, tileTargetY, createWall))
			{
				ui.TotalWallsPlaced++;
				itemTime = Inventory[SelectedItem].UseTime;
				NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 3, tileTargetX, tileTargetY, createWall);
				NetMessage.SendMessage();
				if (Inventory[SelectedItem].Stack > 1)
				{
					for (int i = 0; i < 4; i++)
					{
						int num9 = tileTargetX;
						int num10 = tileTargetY;
						switch (i)
						{
						case 0:
							num9--;
							break;
						case 1:
							num9++;
							break;
						case 2:
							num10--;
							break;
						default:
							num10++;
							break;
						}
						if (Main.TileSet[num9, num10].WallType != 0)
						{
							continue;
						}
						int num11 = 0;
						for (int j = 0; j < 4; j++)
						{
							int num12 = num9;
							int num13 = num10;
							switch (j)
							{
							case 0:
								num12--;
								break;
							case 1:
								num12++;
								break;
							case 2:
								num13--;
								break;
							default:
								num13++;
								break;
							}
							if (Main.TileSet[num12, num13].WallType == createWall)
							{
								num11++;
							}
						}
						if (num11 == 4 && WorldGen.PlaceWall(num9, num10, createWall))
						{
							Inventory[SelectedItem].Stack--;
							if (Inventory[SelectedItem].Stack == 0)
							{
								Inventory[SelectedItem].Init();
							}
							ui.TotalWallsPlaced++;
							NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 3, num9, num10, createWall);
							NetMessage.SendMessage();
						}
					}
				}
			}
			if (!isLocal() || CurrentView.IsFullScreen())
			{
				return;
			}
			if (createTile >= 0 || createWall >= 0)
			{
				if (!ui.UsingSmartCursor && ui.hotbarItemNameTime <= 0)
				{
					CurrentView.Zoom(1.5f);
				}
			}
			else
			{
				CurrentView.Zoom(1.25f);
			}
		}

		public unsafe void ItemCheck(int i)
		{
			fixed (Item* ptr = &Inventory[SelectedItem])
			{
				int num = ptr->Damage;
				if (num > 0)
				{
					if (ptr->IsMelee)
					{
						num = (int)(num * meleeDamage);
					}
					else if (ptr->IsRanged)
					{
						num = (int)(num * rangedDamage);
					}
					else if (ptr->IsMagic)
					{
						num = (int)(num * magicDamage);
					}
				}
				if (ptr->AutoReuse && !noItems)
				{
					releaseUseItem = true;
					if (itemAnimation == 1 && ptr->Stack > 0)
					{
						if (ptr->Shoot > 0 && !isLocal() && controlUseItem)
						{
							itemAnimation = 2;
						}
						else
						{
							itemAnimation = 0;
						}
					}
				}
				if (itemAnimation == 0 && reuseDelay > 0)
				{
					itemAnimation = reuseDelay;
					itemTime = reuseDelay;
					reuseDelay = 0;
				}
				if (controlUseItem && releaseUseItem && (ptr->HeadSlot > 0 || ptr->BodySlot > 0 || ptr->LegSlot > 0))
				{
					if (ptr->UseStyle == 0)
					{
						releaseUseItem = false;
					}
					int num2 = tileTargetX;
					int num3 = tileTargetY;
					if (Main.TileSet[num2, num3].IsActive != 0 && Main.TileSet[num2, num3].Type == 128)
					{
						int frameY = Main.TileSet[num2, num3].FrameY;
						int num4 = 0;
						if (ptr->BodySlot >= 0)
						{
							num4 = 1;
						}
						else if (ptr->LegSlot >= 0)
						{
							num4 = 2;
						}
						frameY /= 18;
						while (num4 > frameY)
						{
							num3++;
							frameY = Main.TileSet[num2, num3].FrameY / 18;
						}
						while (num4 < frameY)
						{
							num3--;
							frameY = Main.TileSet[num2, num3].FrameY / 18;
						}
						int num5 = Main.TileSet[num2, num3].FrameX % 100;
						if (num5 >= 36)
						{
							num5 -= 36;
						}
						num2 -= num5 / 18;
						int frameX = Main.TileSet[num2, num3].FrameX;
						WorldGen.KillTile(num2, num3, KillToFail: true);
						NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num2, num3, 1);
						NetMessage.SendMessage();
						frameX %= 100;
						if (frameY == 0 && ptr->HeadSlot >= 0)
						{
							Main.TileSet[num2, num3].FrameX = (short)(frameX + ptr->HeadSlot * 100);
							NetMessage.SendTile(num2, num3);
							ptr->SetDefaults(0);
							ui.mouseItem.SetDefaults(0);
							releaseUseItem = false;
						}
						else if (frameY == 1 && ptr->BodySlot >= 0)
						{
							Main.TileSet[num2, num3].FrameX = (short)(frameX + ptr->BodySlot * 100);
							NetMessage.SendTile(num2, num3);
							ptr->SetDefaults(0);
							ui.mouseItem.SetDefaults(0);
							releaseUseItem = false;
						}
						else if (frameY == 2 && ptr->LegSlot >= 0)
						{
							Main.TileSet[num2, num3].FrameX = (short)(frameX + ptr->LegSlot * 100);
							NetMessage.SendTile(num2, num3);
							ptr->SetDefaults(0);
							ui.mouseItem.SetDefaults(0);
							releaseUseItem = false;
						}
					}
				}
				if (controlUseItem && itemAnimation == 0 && releaseUseItem && ptr->UseStyle > 0)
				{
					bool flag = !noItems;
					if (ptr->Shoot == 0)
					{
						itemRotation = 0f;
					}
					if (flag)
					{
						if (ptr->Shoot == 85 || ptr->Shoot == 15 || ptr->Shoot == 34)
						{
							flag = !IsWet;
						}
						else if (ptr->Shoot == 6 || ptr->Shoot == 19 || ptr->Shoot == 33 || ptr->Shoot == 52)
						{
							for (int j = 0; j < Projectile.MaxNumProjs; j++)
							{
								if (Main.ProjectileSet[j].active != 0 && Main.ProjectileSet[j].owner == WhoAmI && Main.ProjectileSet[j].type == ptr->Shoot)
								{
									flag = false;
									break;
								}
							}
						}
						else if (ptr->Shoot == 106)
						{
							int num6 = 0;
							for (int k = 0; k < Projectile.MaxNumProjs; k++)
							{
								if (Main.ProjectileSet[k].active != 0 && Main.ProjectileSet[k].owner == WhoAmI && Main.ProjectileSet[k].type == ptr->Shoot && num6 >= ptr->Stack)
								{
									flag = false;
									break;
								}
							}
						}
						else if (ptr->Shoot == 13 || ptr->Shoot == 32)
						{
							for (int l = 0; l < Projectile.MaxNumProjs; l++)
							{
								if (Main.ProjectileSet[l].active != 0 && Main.ProjectileSet[l].owner == WhoAmI && Main.ProjectileSet[l].type == ptr->Shoot && Main.ProjectileSet[l].ai0 != 2f)
								{
									flag = false;
									break;
								}
							}
						}
						else if (ptr->Shoot == 73)
						{
							for (int m = 0; m < Projectile.MaxNumProjs; m++)
							{
								if (Main.ProjectileSet[m].active != 0 && Main.ProjectileSet[m].owner == WhoAmI && Main.ProjectileSet[m].type == 74)
								{
									flag = false;
									break;
								}
							}
						}
					}
					if (flag && ptr->IsPotion)
					{
						if (potionDelay <= 0)
						{
							potionDelay = potionDelayTime;
							AddBuff((int)Buff.ID.POTION_DELAY, potionDelay);
						}
						else
						{
							flag = false;
							itemTime = ptr->UseTime;
						}
					}
					if (ptr->Mana > 0 && silence)
					{
						flag = false;
					}
					if (ptr->Mana > 0 && flag)
					{
						if (ptr->Type != (int)Item.ID.SPACE_GUN || !spaceGun)
						{
							if (statMana >= (int)(ptr->Mana * manaCost))
							{
								statMana -= (short)(ptr->Mana * manaCost);
							}
							else if (manaFlower)
							{
								QuickMana();
								if (statMana >= (int)(ptr->Mana * manaCost))
								{
									statMana -= (short)(ptr->Mana * manaCost);
								}
								else
								{
									flag = false;
								}
							}
							else
							{
								flag = false;
							}
						}
						if (isLocal() && ptr->BuffType != 0)
						{
							AddBuff(ptr->BuffType, ptr->BuffTime);
						}
					}
					if (isLocal() && ptr->BuffType == (int)Buff.ID.PET)
					{
						ApplyPetBuff(ptr->Type);
					}
					if (Main.GameTime.DayTime)
					{
						if (ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_EYE)
						{
							flag = false;
						}
						else if (ptr->Type == (int)Item.ID.MECHANICAL_EYE)
						{
							flag = false;
						}
						else if (ptr->Type == (int)Item.ID.MECHANICAL_WORM)
						{
							flag = false;
						}
						else if (ptr->Type == (int)Item.ID.MECHANICAL_SKULL)
						{
							flag = false;
						}
					}
					if (ptr->Type == (int)Item.ID.WORM_FOOD && !ZoneEvil)
					{
						flag = false;
					}
					else if (flag && isLocal() && ptr->Shoot == 17)
					{
						int num7 = ui.MouseX + CurrentView.ScreenPosition.X >> 4;
						int num8 = ui.MouseY + CurrentView.ScreenPosition.Y >> 4;
						if (Main.TileSet[num7, num8].IsActive != 0 && (Main.TileSet[num7, num8].Type == 0 || Main.TileSet[num7, num8].Type == 2 || Main.TileSet[num7, num8].Type == 23))
						{
							WorldGen.KillTile(num7, num8, KillToFail: false, EffectOnly: false, noItem: true);
							if (Main.TileSet[num7, num8].IsActive == 0)
							{
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 4, num7, num8, 0);
								NetMessage.SendMessage();
							}
							else
							{
								flag = false;
							}
						}
						else
						{
							flag = false;
						}
					}
					if (flag && ptr->UseAmmo > 0)
					{
						flag = false;
						for (int n = 0; n < MaxNumInventory; n++)
						{
							if (Inventory[n].Ammo == ptr->UseAmmo && Inventory[n].Stack > 0)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						if (ptr->PickPower > 0 || ptr->AxePower > 0 || ptr->HammerPower > 0)
						{
							toolTime = 1;
						}
						if (grappling[0] >= 0)
						{
							if (controlRight)
							{
								direction = 1;
							}
							else if (controlLeft)
							{
								direction = -1;
							}
						}
						channel = ptr->Channelling;
						attackCD = 0;
						if (ptr->IsMelee)
						{
							itemAnimation = (short)(ptr->UseAnimation * meleeSpeed);
							itemAnimationMax = (short)(ptr->UseAnimation * meleeSpeed);
						}
						else
						{
							itemAnimation = ptr->UseAnimation;
							itemAnimationMax = ptr->UseAnimation;
							reuseDelay = ptr->ReuseDelay;
						}
						if (ptr->UseSound > 0)
						{
							Main.PlaySound(2, XYWH.X, XYWH.Y, ptr->UseSound);
						}
					}
					if (flag && (ptr->Shoot == 18 || ptr->Shoot == 72 || ptr->Shoot == 86 || ptr->Shoot == 87 || ptr->Shoot == 111))
					{
						for (int num9 = 0; num9 < Projectile.MaxNumProjs; num9++)
						{
							if (Main.ProjectileSet[num9].active != 0 && Main.ProjectileSet[num9].owner == i)
							{
								if (Main.ProjectileSet[num9].type == ptr->Shoot)
								{
									Main.ProjectileSet[num9].Kill();
								}
								else if (ptr->Shoot == 72 && (Main.ProjectileSet[num9].type == 86 || Main.ProjectileSet[num9].type == 87))
								{
									Main.ProjectileSet[num9].Kill();
								}
							}
						}
					}
				}
				if (!controlUseItem)
				{
					channel = false;
				}
				itemHeight = (short)SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.ITEM_1 - 1 + ptr->Type].Height;
				itemWidth = (short)SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.ITEM_1 - 1 + ptr->Type].Width;
				if (itemAnimation > 0)
				{
					if (ptr->IsMelee)
					{
						itemAnimationMax = (short)(ptr->UseAnimation * meleeSpeed);
					}
					else
					{
						itemAnimationMax = ptr->UseAnimation;
					}
					if (ptr->Mana > 0)
					{
						manaRegenDelay = maxRegenDelay;
					}
					itemAnimation--;
					if (ptr->UseStyle == 1)
					{
						if (itemAnimation < itemAnimationMax / 3)
						{
							int num10 = 10;
							if (itemWidth > 32)
							{
								num10 = ((itemWidth <= 64) ? 14 : 28);
							}
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - num10) * direction;
							itemLocation.Y = XYWH.Y + 24;
						}
						else if (itemAnimation < (itemAnimationMax << 1) / 3)
						{
							int num11 = 10;
							if (itemWidth > 32)
							{
								num11 = ((itemWidth <= 64) ? 18 : 28);
							}
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - num11) * direction;
							num11 = 10;
							if (itemWidth > 32)
							{
								num11 = ((itemWidth <= 64) ? 8 : 14);
							}
							itemLocation.Y = XYWH.Y + num11;
						}
						else
						{
							int num12 = 6;
							if (itemWidth > 32)
							{
								num12 = ((itemWidth <= 64) ? 14 : 28);
							}
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - num12) * direction;
							num12 = 10;
							if (itemWidth > 32)
							{
								num12 = ((itemWidth <= 64) ? 10 : 14);
							}
							itemLocation.Y = XYWH.Y + num12;
						}
						itemRotation = (itemAnimation / (float)itemAnimationMax - 0.5f) * -direction * 3.5f - direction * 0.3f;
						if (gravDir == -1)
						{
							itemRotation = 0f - itemRotation;
							itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
						}
					}
					else if (ptr->UseStyle == 2)
					{
						itemRotation = itemAnimation / (float)itemAnimationMax * (direction << 1) + -1.4f * direction;
						if (itemAnimation < itemAnimationMax >> 1)
						{
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - 9 - (int)(itemRotation * (12 * direction))) * direction;
							itemLocation.Y = XYWH.Y + 38 + (int)(itemRotation * (direction << 2));
						}
						else
						{
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - 9 - (int)(itemRotation * (direction << 4))) * direction;
							itemLocation.Y = XYWH.Y + 38 + (int)(itemRotation * direction);
						}
						if (gravDir == -1)
						{
							itemRotation = 0f - itemRotation;
							itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
						}
					}
					else if (ptr->UseStyle == 3)
					{
						if (itemAnimation > (itemAnimationMax << 1) / 3)
						{
							itemLocation.X = -1000;
							itemLocation.Y = -1000;
							itemRotation = -1.3f * direction;
						}
						else
						{
							itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - 4) * direction;
							itemLocation.Y = XYWH.Y + 24;
							float num13 = itemAnimation / (float)itemAnimationMax * itemWidth * direction * ptr->Scale * 1.2f - 10 * direction;
							if (num13 > -4f && direction == -1)
							{
								num13 = -8f;
							}
							if (num13 < 4f && direction == 1)
							{
								num13 = 8f;
							}
							itemLocation.X -= (int)num13;
							itemRotation = 0.8f * direction;
						}
						if (gravDir == -1)
						{
							itemRotation = 0f - itemRotation;
							itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
						}
					}
					else if (ptr->UseStyle == 4)
					{
						itemRotation = 0f;
						itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) - 9 - (int)(itemRotation * (14 * direction)) - 4) * direction;
						itemLocation.Y = XYWH.Y + (itemHeight >> 1) + 4;
						if (gravDir == -1)
						{
							itemRotation = 0f - itemRotation;
							itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
						}
					}
					else if (ptr->UseStyle == 5)
					{
						itemLocation.X = XYWH.X + (width / 2) - (itemWidth >> 1) - (direction << 1);
						itemLocation.Y = XYWH.Y + (height / 2) - (itemHeight >> 1);
					}
				}
				else if (ptr->HoldStyle == 1)
				{
					itemLocation.X = XYWH.X + (width / 2) + ((itemWidth >> 1) + 2) * direction;
					if (ptr->Type == (int)Item.ID.GLOWSTICK || ptr->Type == (int)Item.ID.STICKY_GLOWSTICK)
					{
						itemLocation.X -= direction << 1;
						itemLocation.Y += 4;
					}
					itemLocation.Y = XYWH.Y + 24;
					itemRotation = 0f;
					if (gravDir == -1)
					{
						itemRotation = 0f - itemRotation;
						itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
					}
				}
				else if (ptr->HoldStyle == 2)
				{
					itemLocation.X = XYWH.X + (width / 2) + 6 * direction;
					itemLocation.Y = XYWH.Y + 16;
					itemRotation = -0.79f * direction;
					if (gravDir == -1)
					{
						itemRotation = 0f - itemRotation;
						itemLocation.Y = XYWH.Y + height + (XYWH.Y - itemLocation.Y);
					}
				}
				else if (ptr->HoldStyle == 3)
				{
					itemLocation.X = XYWH.X + (width / 2) - (itemWidth >> 1) - (direction << 1);
					itemLocation.Y = XYWH.Y + (height / 2) - (itemHeight >> 1);
					itemRotation = 0f;
				}
				// BUG: Cursed Torches can be held underwater, but no light will be emitted from them until 1.03. For some reason when bringing over PC code, they buggered the handling code so cursed torches are included in the submersion check.
#if VERSION_103 || VERSION_FINAL
				if (((ptr->Type == (int)Item.ID.TORCH || (ptr->Type >= (int)Item.ID.BLUE_TORCH && ptr->Type <= (int)Item.ID.DEMON_TORCH)) && !IsWet) || ptr->Type == (int)Item.ID.CURSED_TORCH)
#else
				if ((ptr->Type == (int)Item.ID.TORCH || ptr->Type == (int)Item.ID.CURSED_TORCH || (ptr->Type >= (int)Item.ID.BLUE_TORCH && ptr->Type <= (int)Item.ID.DEMON_TORCH)) && !IsWet)
#endif
				{
					int num14 = 0;
					if (ptr->Type == (int)Item.ID.CURSED_TORCH)
					{
						num14 = 8;
					}
					else if (ptr->Type >= (int)Item.ID.BLUE_TORCH)
					{
						num14 = ptr->Type - (int)Item.ID.BLUE_TORCH - 1;
					}
					Vector3 rgb;
					switch (num14)
					{
					case 1:
						rgb = new Vector3(0f, 0.1f, 1.3f);
						break;
					case 2:
						rgb = new Vector3(1f, 0.1f, 0.1f);
						break;
					case 3:
						rgb = new Vector3(0f, 1f, 0.1f);
						break;
					case 4:
						rgb = new Vector3(0.9f, 0f, 0.9f);
						break;
					case 5:
						rgb = new Vector3(1.3f, 1.3f, 1.3f);
						break;
					case 6:
						rgb = new Vector3(0.9f, 0.9f, 0f);
						break;
					case 7:
						rgb = new Vector3(0.5f * Main.DemonTorch + 1f * (1f - Main.DemonTorch), 0.3f, Main.DemonTorch + 0.5f * (1f - Main.DemonTorch));
						break;
					case 8:
						rgb = new Vector3(0.85f, 1f, 0.7f);
						break;
					default:
						rgb = new Vector3(1f, 0.95f, 0.8f);
						break;
					}
					int num15 = num14;
					switch (num15)
					{
					case 0:
						num15 = 6;
						break;
					case 8:
						num15 = 75;
						break;
					default:
						num15 = 58 + num15;
						break;
					}
					int upperBound = 20;
					if (itemAnimation > 0)
					{
						upperBound = 7;
					}
					if (direction == -1)
					{
						if (Main.Rand.Next(upperBound) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X - 16, itemLocation.Y - 14 * gravDir, 4, 4, num15, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X - 12 + velocity.X) >> 4, (int)(itemLocation.Y - 14 + velocity.Y) >> 4, rgb);
					}
					else
					{
						if (Main.Rand.Next(upperBound) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X + 6, itemLocation.Y - 14 * gravDir, 4, 4, num15, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X + 12 + velocity.X) >> 4, (int)(itemLocation.Y - 14 + velocity.Y) >> 4, rgb);
					}
				}
				if (ptr->Type == (int)Item.ID.CANDLE && !IsWet)
				{
					int upperBound2 = 20;
					if (itemAnimation > 0)
					{
						upperBound2 = 7;
					}
					if (direction == -1)
					{
						if (Main.Rand.Next(upperBound2) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X - 12, itemLocation.Y - 20 * gravDir, 4, 4, 6, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X - 16 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(1f, 0.95f, 0.8f));
					}
					else
					{
						if (Main.Rand.Next(upperBound2) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X + 4, itemLocation.Y - 20 * gravDir, 4, 4, 6, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X + 6 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(1f, 0.95f, 0.8f));
					}
				}
				else if (ptr->Type == (int)Item.ID.WATER_CANDLE && !IsWet)
				{
					int upperBound3 = 10;
					if (itemAnimation > 0)
					{
						upperBound3 = 7;
					}
					if (direction == -1)
					{
						if (Main.Rand.Next(upperBound3) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X - 12, itemLocation.Y - 20 * gravDir, 4, 4, 29, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X - 16 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.3f, 0.3f, 0.75f));
					}
					else
					{
						if (Main.Rand.Next(upperBound3) == 0)
						{
							Main.DustSet.NewDust(itemLocation.X + 4, itemLocation.Y - 20 * gravDir, 4, 4, 29, 0.0, 0.0, 100);
						}
						Lighting.AddLight((int)(itemLocation.X + 6 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.3f, 0.3f, 0.75f));
					}
				}
				if (ptr->Type == (int)Item.ID.GLOWSTICK)
				{
					if (direction == -1)
					{
						Lighting.AddLight((int)(itemLocation.X - 16 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.7f, 1f, 0.8f));
					}
					else
					{
						Lighting.AddLight((int)(itemLocation.X + 6 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.7f, 1f, 0.8f));
					}
				}
				else if (ptr->Type == (int)Item.ID.STICKY_GLOWSTICK)
				{
					if (direction == -1)
					{
						Lighting.AddLight((int)(itemLocation.X - 16 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.7f, 0.8f, 1f));
					}
					else
					{
						Lighting.AddLight((int)(itemLocation.X + 6 + velocity.X) >> 4, itemLocation.Y - 14 >> 4, new Vector3(0.7f, 0.8f, 1f));
					}
				}
				releaseUseItem = !controlUseItem;
				if (itemTime > 0)
				{
					itemTime--;
				}
				if (isLocal())
				{
					if (ptr->Shoot > 0 && itemAnimation > 0 && itemTime == 0)
					{
						int num16 = ptr->Shoot;
						float num17 = ptr->ShootSpeed;
						if (ptr->IsMelee && num16 != 25 && num16 != 26 && num16 != 35)
						{
							num17 /= meleeSpeed;
						}
						if (num16 == 13 || num16 == 32)
						{
							grappling[0] = -1;
							grapCount = 0;
							for (int num18 = 0; num18 < Projectile.MaxNumProjs; num18++)
							{
								if (Main.ProjectileSet[num18].type == 13 && Main.ProjectileSet[num18].active != 0 && Main.ProjectileSet[num18].owner == i)
								{
									Main.ProjectileSet[num18].Kill();
								}
							}
						}
						int num19 = num;
						float num20 = ptr->Knockback;
						bool flag2 = false;
						if (ptr->UseAmmo > 0)
						{
							int num21 = -1;
							for (int num22 = MaxNumInventory - 1; num22 >= MaxNumInventory - NumAmmoSlots; num22--)
							{
								if (Inventory[num22].Ammo == ptr->UseAmmo && Inventory[num22].Stack > 0)
								{
									num21 = num22;
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								int num23 = int.MaxValue;
								for (int num24 = NumInvSlots - 1; num24 >= 0; num24--)
								{
									if (Inventory[num24].Ammo == ptr->UseAmmo && Inventory[num24].Stack > 0 && num23 > Inventory[num24].Type)
									{
										num23 = Inventory[num24].Type;
										num21 = num24;
										flag2 = true;
									}
								}
							}
							if (flag2)
							{
								if (Inventory[num21].Shoot > 0)
								{
									num16 = Inventory[num21].Shoot;
								}
								if (num16 == 42)
								{
									if (Inventory[num21].Type == (int)Item.ID.EBONSAND_BLOCK)
									{
										num16 = 65;
										num19 += 5;
									}
									else if (Inventory[num21].Type == (int)Item.ID.PEARLSAND_BLOCK)
									{
										num16 = 68;
										num19 += 5;
									}
								}
								num17 += Inventory[num21].ShootSpeed;
								num19 = ((!Inventory[num21].IsRanged) ? (num19 + Inventory[num21].Damage) : (num19 + (int)(Inventory[num21].Damage * rangedDamage)));
								if (ptr->UseAmmo == 1 && archery)
								{
									if (num17 < 20f)
									{
										num17 *= 1.2f;
										if (num17 > 20f)
										{
											num17 = 20f;
										}
									}
									num19 += num19 / 5;
								}
								num20 += Inventory[num21].Knockback;
								if ((ptr->Type != (int)Item.ID.MINISHARK || Main.Rand.Next(3) != 0) && (ptr->Type != (int)Item.ID.MEGASHARK || Main.Rand.Next(2) != 0) && (ptr->Type != (int)Item.ID.CLOCKWORK_ASSAULT_RIFLE || itemAnimation >= ptr->UseAnimation - 2) && Main.Rand.Next(100) >= freeAmmoChance && (num16 != 85 || itemAnimation >= itemAnimationMax - 6) && --Inventory[num21].Stack <= 0)
								{
									Inventory[num21].Init();
								}
							}
						}
						else
						{
							flag2 = true;
						}
						switch (num16)
						{
						case 72:
						{
							int num26 = Main.Rand.Next(3);
							if (num26 != 0)
							{
								num16 = num26 + 85;
							}
							break;
						}
						case 73:
						{
							for (int num25 = 0; num25 < Projectile.MaxNumProjs; num25++)
							{
								if (Main.ProjectileSet[num25].active != 0 && Main.ProjectileSet[num25].owner == i)
								{
									if (Main.ProjectileSet[num25].type == 73)
									{
										num16 = 74;
									}
									else if (Main.ProjectileSet[num25].type == 74)
									{
										flag2 = false;
										break;
									}
								}
							}
							break;
						}
						}
						if (flag2)
						{
							if (kbGlove && ptr->Mech)
							{
								num20 *= 1.7f;
							}
							if (ptr->Type == (int)Item.ID.MOLTEN_FURY)
							{
								if (num16 == 1)
								{
									num16 = 2;
								}
							}
							else if (ptr->Type == (int)Item.ID.SHARANGA)
							{
								num16 = 113;
							}
							else if (ptr->Type == (int)Item.ID.VULCAN_REPEATER)
							{
								num16 = 114;
							}
							itemTime = ptr->UseTime;
							direction = (sbyte)((ui.MouseX + CurrentView.ScreenPosition.X > XYWH.X + 10) ? 1 : (-1));
							Vector2 vector = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
							switch (num16)
							{
							case 9:
								vector.X += Main.Rand.Next(601) * -direction;
								vector.Y += -300 - Main.Rand.Next(100);
								num20 = 0f;
								break;
							case 51:
								vector.Y -= 6 * gravDir;
								break;
							}
							float num27 = ui.MouseX + CurrentView.ScreenPosition.X - vector.X;
							float num28 = ui.MouseY + CurrentView.ScreenPosition.Y - vector.Y;
							float num29 = (float)Math.Sqrt(num27 * num27 + num28 * num28);
							float num30 = num29;
							num29 = num17 / num29;
							num27 *= num29;
							num28 *= num29;
							switch (num16)
							{
							case 12:
								vector.X += num27 * 3f;
								vector.Y += num28 * 3f;
								break;
							case 17:
								vector.X = ui.MouseX + CurrentView.ScreenPosition.X;
								vector.Y = ui.MouseY + CurrentView.ScreenPosition.Y;
								break;
							}
							if (ptr->UseStyle == 5)
							{
								itemRotation = (float)Math.Atan2(num28 * direction, num27 * direction);
								NetMessage.CreateMessage1(13, WhoAmI);
								NetMessage.SendMessage();
								NetMessage.CreateMessage1(41, WhoAmI);
								NetMessage.SendMessage();
							}
							if (num16 == 76)
							{
								num16 += Main.Rand.Next(3);
								num30 /= (Main.ResolutionHeight / 2);
								if (num30 > 1f)
								{
									num30 = 1f;
								}
								float num31 = num27 + Main.Rand.Next(-40, 41) * 0.01f;
								float num32 = num28 + Main.Rand.Next(-40, 41) * 0.01f;
								num31 *= num30 + 0.25f;
								num32 *= num30 + 0.25f;
								int num33 = Projectile.NewProjectile(vector.X, vector.Y, num31, num32, num16, num19, num20, i, send: false);
								if (num33 >= 0)
								{
									Main.ProjectileSet[num33].ai1 = 1;
									num30 = num30 * 2f - 1f;
									if (num30 < -1f)
									{
										num30 = -1f;
									}
									else if (num30 > 1f)
									{
										num30 = 1f;
									}
									Main.ProjectileSet[num33].ai0 = num30;
									NetMessage.SendProjectile(num33);
								}
							}
							else if (ptr->Type == (int)Item.ID.MINISHARK || ptr->Type == (int)Item.ID.MEGASHARK)
							{
								float speedX = num27 + Main.Rand.Next(-40, 41) * 0.01f;
								float speedY = num28 + Main.Rand.Next(-40, 41) * 0.01f;
								Projectile.NewProjectile(vector.X, vector.Y, speedX, speedY, num16, num19, num20, i);
							}
							else if (ptr->Type == (int)Item.ID.CRYSTAL_STORM)
							{
								float num34 = num27;
								float num35 = num28;
								num34 += Main.Rand.Next(-40, 41) * 0.04f;
								num35 += Main.Rand.Next(-40, 41) * 0.04f;
								Projectile.NewProjectile(vector.X, vector.Y, num34, num35, num16, num19, num20, i);
							}
							else if (ptr->Type == (int)Item.ID.SHOTGUN)
							{
								for (int num36 = 0; num36 < 4; num36++)
								{
									float num37 = num27;
									float num38 = num28;
									num37 += Main.Rand.Next(-40, 41) * 0.05f;
									num38 += Main.Rand.Next(-40, 41) * 0.05f;
									Projectile.NewProjectile(vector.X, vector.Y, num37, num38, num16, num19, num20, i);
								}
							}
							else if (ptr->Type == (int)Item.ID.CLOCKWORK_ASSAULT_RIFLE)
							{
								float num39 = num27;
								float num40 = num28;
								if (itemAnimation < 5)
								{
									num39 += Main.Rand.Next(-40, 41) * 0.01f;
									num40 += Main.Rand.Next(-40, 41) * 0.01f;
									num39 *= 1.1f;
									num40 *= 1.1f;
								}
								else if (itemAnimation < 10)
								{
									num39 += Main.Rand.Next(-20, 21) * 0.01f;
									num40 += Main.Rand.Next(-20, 21) * 0.01f;
									num39 *= 1.05f;
									num40 *= 1.05f;
								}
								Projectile.NewProjectile(vector.X, vector.Y, num39, num40, num16, num19, num20, i);
							}
							else if (ptr->BuffType != (int)Buff.ID.PET)
							{
								int num41 = Projectile.NewProjectile(vector.X, vector.Y, num27, num28, num16, num19, num20, i);
								if (num41 >= 0 && num16 == 80)
								{
									Main.ProjectileSet[num41].ai0 = tileTargetX;
									Main.ProjectileSet[num41].ai1 = tileTargetY;
								}
							}
						}
						else if (ptr->UseStyle == 5)
						{
							itemRotation = 0f;
							NetMessage.CreateMessage1(41, WhoAmI);
							NetMessage.SendMessage();
						}
					}
					if (isLocal() && (ptr->Type == (int)Item.ID.WRENCH || ptr->Type == (int)Item.ID.WIRE_CUTTER) && itemAnimation > 0 && itemTime == 0 && controlUseItem)
					{
						int i2 = tileTargetX;
						int j2 = tileTargetY;
						if (ptr->Type == (int)Item.ID.WRENCH)
						{
							int num42 = -1;
							for (int num43 = 0; num43 < MaxNumInventory; num43++)
							{
								if (Inventory[num43].Stack > 0 && Inventory[num43].Type == (int)Item.ID.WIRE)
								{
									num42 = num43;
									break;
								}
							}
							if (num42 >= 0 && WorldGen.PlaceWire(i2, j2))
							{
								if (++ui.totalWires == (int)Achievement.WiresPlacedGoal)
								{
									ui.SetTriggerState(Trigger.PlacedLotsOfWires);
								}
								Inventory[num42].Stack--;
								if (Inventory[num42].Stack <= 0)
								{
									Inventory[num42].Init();
								}
								itemTime = ptr->UseTime;
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 5, tileTargetX, tileTargetY, 0);
								NetMessage.SendMessage();
							}
						}
						else if (WorldGen.KillWire(i2, j2))
						{
							if (ui.totalWires != 0)
							{
								ui.totalWires--;
							}
							itemTime = ptr->UseTime;
							NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 6, tileTargetX, tileTargetY, 0);
							NetMessage.SendMessage();
						}
					}
					if (itemAnimation > 0 && itemTime == 0 && (ptr->Type == (int)Item.ID.BELL || ptr->Type == (int)Item.ID.HARP))
					{
						itemTime = ptr->UseTime;
						Vector2 vector2 = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
						float num44 = ui.MouseX + CurrentView.ScreenPosition.X - vector2.X;
						float num45 = ui.MouseY + CurrentView.ScreenPosition.Y - vector2.Y;
						float num46 = (float)Math.Sqrt(num44 * num44 + num45 * num45);
						num46 /= (Main.ResolutionHeight / 2);
						if (num46 > 1f)
						{
							num46 = 1f;
						}
						num46 = num46 * 2f - 1f;
						if (num46 < -1f)
						{
							num46 = -1f;
						}
						if (num46 > 1f)
						{
							num46 = 1f;
						}
						Main.HarpNote = num46;
						int style = ((ptr->Type == (int)Item.ID.BELL) ? 35 : 26);
						Main.PlaySound(2, XYWH.X, XYWH.Y, style);
						NetMessage.CreateMessage1(58, WhoAmI);
						NetMessage.SendMessage();
					}
					if (ptr->Type >= (int)Item.ID.EMPTY_BUCKET && ptr->Type <= (int)Item.ID.LAVA_BUCKET && itemTime == 0 && itemAnimation > 0 && controlUseItem)
					{
						if (ptr->Type == (int)Item.ID.EMPTY_BUCKET)
						{
							int lava = Main.TileSet[tileTargetX, tileTargetY].Lava;
							int num47 = 0;
							for (int num48 = tileTargetX - 1; num48 <= tileTargetX + 1; num48++)
							{
								for (int num49 = tileTargetY - 1; num49 <= tileTargetY + 1; num49++)
								{
									if (Main.TileSet[num48, num49].Lava == lava)
									{
										num47 += Main.TileSet[num48, num49].Liquid;
									}
								}
							}
							if (Main.TileSet[tileTargetX, tileTargetY].Liquid > 0 && num47 > 100)
							{
								int lava2 = Main.TileSet[tileTargetX, tileTargetY].Lava;
								if (Main.TileSet[tileTargetX, tileTargetY].Lava == 0)
								{
									ptr->SetDefaults((int)Item.ID.WATER_BUCKET);
								}
								else
								{
									ptr->SetDefaults((int)Item.ID.LAVA_BUCKET);
								}
								Main.PlaySound(19, XYWH.X, XYWH.Y);
								itemTime = ptr->UseTime;
								int num50 = Main.TileSet[tileTargetX, tileTargetY].Liquid;
								Main.TileSet[tileTargetX, tileTargetY].Liquid = 0;
								Main.TileSet[tileTargetX, tileTargetY].Lava = 0;
								WorldGen.SquareTileFrame(tileTargetX, tileTargetY, 0);
								if (Main.NetMode == (byte)NetModeSetting.CLIENT)
								{
									NetMessage.SendWater(tileTargetX, tileTargetY);
								}
								else
								{
									Liquid.AddWater(tileTargetX, tileTargetY);
								}
								for (int num51 = tileTargetX - 1; num51 <= tileTargetX + 1; num51++)
								{
									for (int num52 = tileTargetY - 1; num52 <= tileTargetY + 1; num52++)
									{
										if (num50 < 256 && Main.TileSet[num51, num52].Lava == lava)
										{
											int num53 = Main.TileSet[num51, num52].Liquid;
											if (num53 + num50 > 255)
											{
												num53 = 255 - num50;
											}
											num50 += num53;
											Main.TileSet[num51, num52].Liquid -= (byte)num53;
											Main.TileSet[num51, num52].Lava = (byte)((Main.TileSet[num51, num52].Liquid != 0) ? lava2 : 0);
											WorldGen.SquareTileFrame(num51, num52, 0);
											if (Main.NetMode == (byte)NetModeSetting.CLIENT)
											{
												NetMessage.SendWater(num51, num52);
											}
											else
											{
												Liquid.AddWater(num51, num52);
											}
										}
									}
								}
							}
						}
						else if (Main.TileSet[tileTargetX, tileTargetY].Liquid < 200 && (Main.TileSet[tileTargetX, tileTargetY].IsActive == 0 || !Main.TileSolidNotSolidTop[Main.TileSet[tileTargetX, tileTargetY].Type]))
						{
							if (ptr->Type == (int)Item.ID.LAVA_BUCKET)
							{
								if (Main.TileSet[tileTargetX, tileTargetY].Liquid == 0 || Main.TileSet[tileTargetX, tileTargetY].Lava != 0)
								{
									Main.PlaySound(19, XYWH.X, XYWH.Y);
									Main.TileSet[tileTargetX, tileTargetY].Lava = 32;
									Main.TileSet[tileTargetX, tileTargetY].Liquid = byte.MaxValue;
									WorldGen.SquareTileFrame(tileTargetX, tileTargetY);
									ptr->SetDefaults((int)Item.ID.EMPTY_BUCKET);
									itemTime = ptr->UseTime;
									NetMessage.SendWater(tileTargetX, tileTargetY);
								}
							}
							else if (Main.TileSet[tileTargetX, tileTargetY].Liquid == 0 || Main.TileSet[tileTargetX, tileTargetY].Lava == 0)
							{
								Main.PlaySound(19, XYWH.X, XYWH.Y);
								Main.TileSet[tileTargetX, tileTargetY].Lava = 0;
								Main.TileSet[tileTargetX, tileTargetY].Liquid = byte.MaxValue;
								WorldGen.SquareTileFrame(tileTargetX, tileTargetY);
								ptr->SetDefaults((int)Item.ID.EMPTY_BUCKET);
								itemTime = ptr->UseTime;
								NetMessage.SendWater(tileTargetX, tileTargetY);
							}
						}
					}
					if (!channel)
					{
						toolTime = itemTime;
					}
					else if (--toolTime < 0)
					{
						if (ptr->PickPower > 0)
						{
							toolTime = ptr->UseTime;
						}
						else
						{
							toolTime = (short)(ptr->UseTime * pickSpeed);
						}
					}
					if (ptr->PickPower > 0 || ptr->AxePower > 0 || ptr->HammerPower > 0)
					{
						bool flag3 = true;
						if (Main.TileSet[tileTargetX, tileTargetY].IsActive != 0)
						{
							int type = Main.TileSet[tileTargetX, tileTargetY].Type;
							if ((ptr->PickPower > 0 && !Main.TileAxe[type] && !Main.TileHammer[type]) || (ptr->AxePower > 0 && Main.TileAxe[type]) || (ptr->HammerPower > 0 && Main.TileHammer[type]))
							{
								flag3 = false;
							}
							if (toolTime == 0 && itemAnimation > 0 && controlUseItem)
							{
								if (hitTileX != tileTargetX || hitTileY != tileTargetY)
								{
									hitTile = 0;
									hitTileX = tileTargetX;
									hitTileY = tileTargetY;
								}
								if (Main.TileNoFail[type])
								{
									hitTile = 100;
								}
								if (type != 27)
								{
									if (Main.TileHammer[type])
									{
										flag3 = false;
										switch (type)
										{
										case 48:
											hitTile += (short)(ptr->HammerPower >> 1);
											break;
										case 129:
											hitTile += (short)(ptr->HammerPower << 1);
											break;
										default:
											hitTile = (short)(hitTile + ptr->HammerPower);
											break;
										}
										if (tileTargetY > Main.RockLayer && type == 77 && ptr->HammerPower < 60)
										{
											hitTile = 0;
										}
										if (ptr->HammerPower > 0)
										{
											if (type == 26 && (ptr->HammerPower < 80 || !Main.InHardMode))
											{
												hitTile = 0;
												Hurt(statLife >> 1, -direction, pvp: false, quiet: false, Lang.DeathMsgPtr());
											}
											if (hitTile >= 100)
											{
												if (Main.NetMode == (byte)NetModeSetting.CLIENT && type == 21)
												{
													WorldGen.KillTile(tileTargetX, tileTargetY, KillToFail: true);
													NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 1);
													NetMessage.SendMessage();
													NetMessage.CreateMessage2(34, tileTargetX, tileTargetY);
												}
												else
												{
													hitTile = 0;
													WorldGen.KillTile(tileTargetX, tileTargetY);
													NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 0);
												}
											}
											else
											{
												WorldGen.KillTile(tileTargetX, tileTargetY, KillToFail: true);
												NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 1);
											}
											NetMessage.SendMessage();
											itemTime = ptr->UseTime;
										}
									}
									else if (Main.TileAxe[type])
									{
										if (ptr->AxePower > 0)
										{
											switch (type)
											{
											case 30:
											case 124:
												hitTile += (short)(ptr->AxePower * 5);
												break;
											case 80:
												hitTile += (short)(ptr->AxePower * 3);
												break;
											default:
												hitTile = (short)(hitTile + ptr->AxePower);
												break;
											}
											if (hitTile >= 100)
											{
												hitTile = 0;
												if (type == 5)
												{
													ui.TotalChopsTaken++;
													WorldGen.woodSpawned = 0;
												}
												ui.TotalWoodAxed++;
												WorldGen.KillTile(tileTargetX, tileTargetY);
												NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 0);
												NetMessage.SendMessage();
												if (type == 5)
												{
													ui.Statistics.IncreaseWoodStat(WorldGen.woodSpawned);
												}
											}
											else
											{
												WorldGen.KillTile(tileTargetX, tileTargetY, KillToFail: true);
												NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 1);
												NetMessage.SendMessage();
											}
											itemTime = ptr->UseTime;
										}
									}
									else if (ptr->PickPower > 0)
									{
										// The code below is accurate to what is present in the console-releases, however it has been rearranged for ease of reading (matches PC). See below for accounted difference.
										switch (type)
										{
											case 25:
											case 37:
											case 41:
											case 43:
											case 44:
											case 58:
											case 107:
											case 117:
												hitTile += (short)(ptr->PickPower >> 1);
												break;
											case 108:
												hitTile += (short)(ptr->PickPower / 3);
												break;
											case 111:
												hitTile += (short)(ptr->PickPower >> 2);
												break;
											default:
												hitTile += ptr->PickPower;
												break;
										}
										switch (type)
										{
											case 22:
												if (tileTargetY > Main.WorldSurface && ptr->PickPower < 55)
												{
													hitTile = 0;
												}
												break;
											case 25:
											case 56:
											case 58:
											case 117:
												if (ptr->PickPower < 65)
												{
													hitTile = 0;
												}
												break;
											case 37:
												if (ptr->PickPower < 55)
												{
													hitTile = 0;
												}
												break;
											case 41:
											case 43:
											case 44:
#if !VERSION_INITIAL
												if (ptr->PickPower < 65)
												{
													if (tileTargetX < Main.MaxTilesX * 0.25 || tileTargetX > Main.MaxTilesX * 0.75)
													{
														hitTile = 0;
													}
												}
#else
												// BUG: They forgor to add a check to see if you are using a Nightmare pick or greater when it comes to dungeon tiles.
												if (tileTargetX < Main.MaxTilesX * 0.25 || tileTargetX > Main.MaxTilesX * 0.75)
												{
													hitTile = 0;
												}
#endif
												break;
											case 107:
												if (ptr->PickPower < 100)
												{
													hitTile = 0;
												}
												break;
											case 108:
												if (ptr->PickPower < 110)
												{
													hitTile = 0;
												}
												break;
											case 111:
												if (ptr->PickPower < 120)
												{
													hitTile = 0;
												}
												break;
										}
										switch (type)
										{
											case 0:
											case 40:
											case 53:
											case 57:
											case 59:
											case 123:
												// In the console-releases, this is actually multiplied by 2, however this is due to these instructions being placed after the initial switch and its associated default.
												hitTile += ptr->PickPower;
												break;
										}
										if (hitTile >= 100 && (type == 2 || type == 23 || type == 60 || type == 70 || type == 109))
										{
											hitTile = 0;
										}
										if (hitTile >= 100)
										{
											switch (type)
											{
											case 0:
											case 1:
											case 53:
											case 57:
											case 58:
											case 59:
											case 112:
											case 116:
											case 123:
											case 147:
												ui.Statistics.IncreaseStat(StatisticEntry.Soils);
												break;
											case 7:
												ui.TotalCopperObtained++;
												ui.Statistics.IncreaseStat(StatisticEntry.Ore);
												break;
											case 6:
											case 8:
											case 9:
											case 22:
											case 56:
											case 107:
											case 108:
											case 111:
												ui.Statistics.IncreaseStat(StatisticEntry.Ore);
												break;
											case 63:
											case 64:
											case 65:
											case 66:
											case 67:
											case 68:
												ui.Statistics.IncreaseStat(StatisticEntry.Gems);
												break;
											}
											if (++ui.TotalOrePicked == (int)Achievement.LandscaperTilesGoal)
											{
												ui.SetTriggerState(Trigger.RemovedLotsOfTiles);
											}
											hitTile = 0;
											WorldGen.KillTile(tileTargetX, tileTargetY);
											NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 0);
											NetMessage.SendMessage();
										}
										else
										{
											WorldGen.KillTile(tileTargetX, tileTargetY, KillToFail: true);
											NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, tileTargetX, tileTargetY, 1);
											NetMessage.SendMessage();
										}
										itemTime = (byte)(ptr->UseTime * pickSpeed);
									}
								}
							}
						}
						int num54 = tileTargetX;
						int num55 = tileTargetY;
						if ((Main.TileSet[num54, num55].WallType == 0 || !WorldGen.CanKillWall(num54, num55)) && Main.TileSet[num54, num55].IsActive == 0)
						{
							int num56 = -1;
							if (((ui.MouseX + CurrentView.ScreenPosition.X) & 0xF) < 8)
							{
								num56 = 0;
							}
							int num57 = -1;
							if (((ui.MouseY + CurrentView.ScreenPosition.Y) & 0xF) < 8)
							{
								num57 = 0;
							}
							for (int num58 = tileTargetX + num56; num58 <= tileTargetX + num56 + 1; num58++)
							{
								bool flag = true;
								for (int num59 = tileTargetY + num57; num59 <= tileTargetY + num57 + 1; num59++)
								{
									num54 = num58;
									num55 = num59;
									int wall = Main.TileSet[num54, num55].WallType;
									if (wall > 0 && WorldGen.CanKillWall(num54, num55))
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
						if (flag3 && Main.TileSet[num54, num55].WallType > 0 && toolTime == 0 && itemAnimation > 0 && controlUseItem && ptr->HammerPower > 0 && WorldGen.CanKillWall(num54, num55))
						{
							if (hitTileX != num54 || hitTileY != num55)
							{
								hitTile = 0;
								hitTileX = (short)num54;
								hitTileY = (short)num55;
							}
							hitTile += (short)(ptr->HammerPower + (ptr->HammerPower >> 1));
							if (hitTile >= 100)
							{
								hitTile = 0;
								WorldGen.KillWall(num54, num55);
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 2, num54, num55, 0);
							}
							else
							{
								WorldGen.KillWall(num54, num55, true);
								NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 2, num54, num55, 1);
							}
							NetMessage.SendMessage();
							itemTime = (byte)(ptr->UseTime >> 1);
						}
					}
					if (ptr->Type == (int)Item.ID.LIFE_CRYSTAL)
					{
						if (itemTime == 0 && itemAnimation > 0 && StatLifeMax < 400)
						{
							itemTime = ptr->UseTime;
							StatLifeMax += 20;
							statLife += 20;
							HealEffect(20);
						}
						if (statManaMax == 200 && StatLifeMax == 400)
						{
							ui.SetTriggerState(Trigger.MaxHealthAndMana);
						}
					}
					else if (ptr->Type == (int)Item.ID.MANA_CRYSTAL)
					{
						if (itemTime == 0 && itemAnimation > 0 && statManaMax < 200)
						{
							itemTime = ptr->UseTime;
							statManaMax += 20;
							statMana += 20;
							ManaEffect(20);
						}
						if (statManaMax == 200 && StatLifeMax == 400)
						{
							ui.SetTriggerState(Trigger.MaxHealthAndMana);
						}
					}
					else
					{
						PlaceThing();
					}
				}
				if (ptr->Damage >= 0 && ptr->Type > 0 && !ptr->NoMelee && itemAnimation > 0)
				{
					bool flag4 = false;
					Rectangle rectangle = new Rectangle(itemLocation.X, itemLocation.Y, (int)(itemWidth * ptr->Scale), (int)(itemHeight * ptr->Scale));
					if (direction == -1)
					{
						rectangle.X -= rectangle.Width;
					}
					if (gravDir == 1)
					{
						rectangle.Y -= rectangle.Height;
					}
					if (ptr->UseStyle == 1)
					{
						if (itemAnimation < itemAnimationMax / 3)
						{
							if (direction == -1)
							{
								rectangle.X -= (int)(rectangle.Width * 1.4 - rectangle.Width);
							}
							rectangle.Width = (int)(rectangle.Width * 1.4);
							rectangle.Y += (int)(rectangle.Height * 0.5 * gravDir);
							rectangle.Height = (int)(rectangle.Height * 1.1);
						}
						else if (itemAnimation >= (itemAnimationMax << 1) / 3)
						{
							if (direction == 1)
							{
								rectangle.X -= (int)(rectangle.Width * 1.2);
							}
							rectangle.Width *= 2;
							rectangle.Y -= (int)((rectangle.Height * 1.4 - rectangle.Height) * gravDir);
							rectangle.Height = (int)(rectangle.Height * 1.4);
						}
					}
					else if (ptr->UseStyle == 3)
					{
						if (itemAnimation > (itemAnimationMax << 1) / 3)
						{
							flag4 = true;
						}
						else
						{
							if (direction == -1)
							{
								rectangle.X -= (int)(rectangle.Width * 1.4 - rectangle.Width);
							}
							rectangle.Width = (int)(rectangle.Width * 1.4);
							rectangle.Y += (int)(rectangle.Height * 0.6);
							rectangle.Height = (int)(rectangle.Height * 0.6);
						}
					}
					if (!flag4)
					{
						if (ptr->Type == (int)Item.ID.DEMON_BOW || ptr->Type == (int)Item.ID.WAR_AXE_OF_THE_NIGHT || ptr->Type == (int)Item.ID.LIGHTS_BANE || ptr->Type == (int)Item.ID.NIGHTMARE_PICKAXE || ptr->Type == (int)Item.ID.THE_BREAKER)
						{
							if (Main.Rand.Next(18) == 0)
							{
								Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 14, direction * 2, 0.0, 150, default, 1.3f);
							}
						}
						else if (ptr->Type == (int)Item.ID.NIGHTS_EDGE)
						{
							if (Main.Rand.Next(6) == 0)
							{
								Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 14, direction * 2, 0.0, 150, default, 1.4f);
							}
							Dust* ptr2 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 27, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 100, default, 1.2f);
							if (ptr2 != null)
							{
								ptr2->NoGravity = true;
								ptr2->Velocity *= 0.5f;
							}
						}
						else if (ptr->Type == (int)Item.ID.STARFURY)
						{
							if (Main.Rand.Next(6) == 0)
							{
								Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 58, 0.0, 0.0, 150, default, 1.2f);
							}
							if (Main.Rand.Next(12) == 0)
							{
								Gore.NewGore(new Vector2(rectangle.X, rectangle.Y), default(Vector2), Main.Rand.Next(16, 18));
							}
						}
						else if (ptr->Type == (int)Item.ID.BLADE_OF_GRASS || ptr->Type == (int)Item.ID.STAFF_OF_REGROWTH)
						{
							Dust* ptr3 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 40, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 0, default, 1.2f);
							if (ptr3 != null)
							{
								ptr3->NoGravity = true;
							}
						}
						else if (ptr->Type == (int)Item.ID.FIERY_GREATSWORD)
						{
							Dust* ptr4 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 6, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 100, default, 2.5);
							if (ptr4 != null)
							{
								ptr4->NoGravity = true;
								ptr4->Velocity.X *= 2f;
								ptr4->Velocity.Y *= 2f;
							}
						}
						else if (ptr->Type == (int)Item.ID.MOLTEN_PICKAXE || ptr->Type == (int)Item.ID.MOLTEN_HAMAXE)
						{
							Dust* ptr5 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 6, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 100, default, 1.9f);
							if (ptr5 != null)
							{
								ptr5->NoGravity = true;
							}
						}
						else if (ptr->Type == (int)Item.ID.MURAMASA)
						{
							Dust* ptr6 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 29, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 100, default, 2f);
							if (ptr6 != null)
							{
								ptr6->NoGravity = true;
								ptr6->Velocity.X *= 0.5f;
								ptr6->Velocity.Y *= 0.5f;
							}
						}
						else if (ptr->Type == (int)Item.ID.PWNHAMMER || ptr->Type == (int)Item.ID.EXCALIBUR)
						{
							if (Main.Rand.Next(4) == 0)
							{
								Dust* ptr7 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 57, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, 100, default, 1.1f);
								if (ptr7 != null)
								{
									ptr7->NoGravity = true;
									ptr7->Velocity.X *= 0.5f;
									ptr7->Velocity.X += direction << 1;
									ptr7->Velocity.Y *= 0.5f;
								}
							}
							if (Main.Rand.Next(5) == 0)
							{
								Dust* ptr8 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 43, 0.0, 0.0, 254, default, 0.3f);
								if (ptr8 != null)
								{
									ptr8->Velocity.X = 0f;
									ptr8->Velocity.Y = 0f;
								}
							}
						}
						else if (ptr->Type >= (int)Item.ID.BLUE_PHASEBLADE && ptr->Type <= (int)Item.ID.YELLOW_PHASEBLADE)
						{
							Lighting.AddLight(RGB: (ptr->Type == (int)Item.ID.BLUE_PHASEBLADE) ? new Vector3(0.05f, 0.25f, 0.6f) : ((ptr->Type == (int)Item.ID.RED_PHASEBLADE) ? new Vector3(0.5f, 0.1f, 0.05f) : ((ptr->Type == (int)Item.ID.GREEN_PHASEBLADE) ? new Vector3(0.05f, 0.5f, 0.1f) : ((ptr->Type == (int)Item.ID.PURPLE_PHASEBLADE) ? new Vector3(0.4f, 0.05f, 0.5f) : ((ptr->Type != (int)Item.ID.WHITE_PHASEBLADE) ? new Vector3(0.45f, 0.45f, 0.05f) : new Vector3(0.4f, 0.45f, 0.5f))))), LightX: (int)(itemLocation.X + 6 + velocity.X) >> 4, LightY: itemLocation.Y - 14 >> 4);
						}
						else if (ptr->Type == (int)Item.ID.TIZONA)
						{
							Dust* ptr9 = Main.DustSet.NewDust(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 6, velocity.X * 0.2f + direction * 3, velocity.Y * 0.2f, Main.Rand.Next(61, 62), default, 2.5f);
							if (ptr9 != null)
							{
								ptr9->NoGravity = true;
								ptr9->Velocity.X *= 2f;
								ptr9->Velocity.Y *= 2f;
							}
						}
						if (isLocal())
						{
							int dmg = (int)(ptr->Damage * meleeDamage);
							float num60 = ptr->Knockback;
							if (kbGlove)
							{
								num60 *= 2f;
							}
							int num61 = rectangle.X >> 4;
							int num62 = (rectangle.X + rectangle.Width >> 4) + 1;
							int num63 = rectangle.Y >> 4;
							int num64 = (rectangle.Y + rectangle.Height >> 4) + 1;
							for (int num65 = num61; num65 < num62; num65++)
							{
								for (int num66 = num63; num66 < num64; num66++)
								{
									if (Main.TileCut[Main.TileSet[num65, num66].Type] && Main.TileSet[num65, num66 + 1].Type != 78)
									{
										WorldGen.KillTile(num65, num66);
										NetMessage.CreateMessage5((int)NetMessageId.MSG_WORLD_CHANGED, 0, num65, num66, 0);
										NetMessage.SendMessage();
									}
								}
							}
							for (int num67 = 0; num67 < NPC.MaxNumNPCs; num67++)
							{
								NPC nPC = Main.NPCSet[num67];
								if (nPC.Active == 0 || nPC.Immunities[i] != 0 || attackCD != 0 || nPC.DontTakeDamage || (nPC.IsFriendly && (nPC.Type != (int)NPC.ID.GUIDE || !killGuide)) || !rectangle.Intersects(nPC.XYWH) || (!nPC.HasNoTileCollide && !Collision.CanHit(ref XYWH, ref nPC.XYWH)))
								{
									continue;
								}
								bool flag5 = Main.Rand.Next(1, 101) <= meleeCrit;
								int num68 = Main.DamageVar(dmg);
								nPC.ApplyWeaponBuff(ptr->Type);
								nPC.StrikeNPC(num68, num60, direction, flag5);
								NetMessage.SendNpcHurt(num67, num68, num60, direction, flag5);
								if (nPC.Active == 0)
								{
									StatisticEntry statisticEntryFromNetID = Statistics.GetStatisticEntryFromNetID(nPC.NetID);
									ui.Statistics.IncreaseStat(statisticEntryFromNetID);
									if (nPC.Type == (int)NPC.ID.SLIME)
									{
										ui.TotalDeadSlimes++;
									}
#if !USE_ORIGINAL_CODE
									if (nPC.Type == (int)NPC.ID.GUIDE)
									{
										ui.SetTriggerState(Trigger.Homicidal);
									}
#endif
								}
								nPC.Immunities[i] = (byte)itemAnimation;
								attackCD = (short)(itemAnimationMax / 3);
							}
							if (hostile)
							{
								for (int num69 = 0; num69 < MaxNumPlayers; num69++)
								{
									if (num69 != i && Main.PlayerSet[num69].Active != 0 && Main.PlayerSet[num69].hostile && !Main.PlayerSet[num69].immune && !Main.PlayerSet[num69].IsDead && (Main.PlayerSet[i].team == 0 || Main.PlayerSet[i].team != Main.PlayerSet[num69].team) && rectangle.Intersects(Main.PlayerSet[num69].XYWH) && Collision.CanHit(ref XYWH, ref Main.PlayerSet[num69].XYWH))
									{
										bool flag6 = false;
										if (Main.Rand.Next(1, 101) <= 10)
										{
											flag6 = true;
										}
										int num70 = Main.DamageVar(dmg);
										Main.PlayerSet[num69].ApplyWeaponBuffPvP(ptr->Type);
										Main.PlayerSet[num69].Hurt(num70, direction, true, false, Lang.DeathMsgPtr(), flag6);
										NetMessage.SendPlayerHurt(num69, direction, num70, true, flag6, Lang.DeathMsgPtr(WhoAmI));
										attackCD = (short)(itemAnimationMax / 3);
									}
								}
							}
						}
					}
				}
				if (itemTime == 0 && itemAnimation > 0)
				{
					if (ptr->HealLife > 0)
					{
						statLife += ptr->HealLife;
						itemTime = ptr->UseTime;
						if (isLocal())
						{
							HealEffect(ptr->HealLife);
						}
					}
					if (ptr->HealMana > 0)
					{
						statMana += ptr->HealMana;
						itemTime = ptr->UseTime;
						if (isLocal())
						{
							ManaEffect(ptr->HealMana);
						}
					}
					if (ptr->BuffType > 0)
					{
						if (isLocal())
						{
							AddBuff(ptr->BuffType, ptr->BuffTime);
						}
						itemTime = ptr->UseTime;
					}
					if (isLocal())
					{
						if (ptr->Type == (int)Item.ID.GOBLIN_BATTLE_STANDARD)
						{
							itemTime = ptr->UseTime;
							Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								if (Main.InvasionType == 0)
								{
									Main.InvasionDelay = 0;
									Main.StartInvasion();
								}
							}
							else
							{
								NetMessage.CreateMessage2(61, WhoAmI, -1);
								NetMessage.SendMessage();
							}
						}
						else if (ptr->Type == (int)Item.ID.SNOW_GLOBE)
						{
							itemTime = ptr->UseTime;
							Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
							if (Main.NetMode != (byte)NetModeSetting.CLIENT)
							{
								if (Main.InvasionType == 0)
								{
									Main.InvasionDelay = 0;
									Main.StartInvasion(2);
								}
							}
							else
							{
								NetMessage.CreateMessage2(61, WhoAmI, -2);
								NetMessage.SendMessage();
							}
						}
						else if (ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_EYE || ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_SKULL || ptr->Type == (int)Item.ID.WORM_FOOD || ptr->Type == (int)Item.ID.MECHANICAL_EYE || ptr->Type == (int)Item.ID.MECHANICAL_WORM || ptr->Type == (int)Item.ID.MECHANICAL_SKULL || ptr->Type == (int)Item.ID.SLIME_CROWN)
						{
							bool flag7 = false;
							for (int num71 = 0; num71 < NPC.MaxNumNPCs; num71++)
							{
								if (Main.NPCSet[num71].Active != 0 && ((ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_EYE && Main.NPCSet[num71].Type == (int)NPC.ID.EYE_OF_CTHULHU) || (ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_SKULL && Main.NPCSet[num71].Type == (int)NPC.ID.OCRAM) || (ptr->Type == (int)Item.ID.WORM_FOOD && Main.NPCSet[num71].Type == (int)NPC.ID.EATER_OF_WORLDS_HEAD) || (ptr->Type == (int)Item.ID.SLIME_CROWN && Main.NPCSet[num71].Type == (int)NPC.ID.KING_SLIME) || (ptr->Type == (int)Item.ID.MECHANICAL_EYE && Main.NPCSet[num71].Type == (int)NPC.ID.RETINAZER) || (ptr->Type == (int)Item.ID.MECHANICAL_EYE && Main.NPCSet[num71].Type == (int)NPC.ID.SPAZMATISM) || (ptr->Type == (int)Item.ID.MECHANICAL_WORM && Main.NPCSet[num71].Type == (int)NPC.ID.THE_DESTROYER_HEAD) || (ptr->Type == (int)Item.ID.MECHANICAL_SKULL && Main.NPCSet[num71].Type == (int)NPC.ID.PRIME_CANNON))) // The cannon?
								{
									flag7 = true;
									break;
								}
							}
							if (flag7)
							{
								itemTime = ptr->UseTime;
							}
							else if (ptr->Type == (int)Item.ID.SLIME_CROWN)
							{
								itemTime = ptr->UseTime;
								Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
								if (Main.NetMode != (byte)NetModeSetting.CLIENT)
								{
									NPC.SpawnOnPlayer(this, (int)NPC.ID.KING_SLIME);
								}
								else
								{
									NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.KING_SLIME);
									NetMessage.SendMessage();
								}
							}
							else if (ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_EYE)
							{
								if (!Main.GameTime.DayTime)
								{
									itemTime = ptr->UseTime;
									Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
									if (Main.NetMode != (byte)NetModeSetting.CLIENT)
									{
										NPC.SpawnOnPlayer(this, (int)NPC.ID.EYE_OF_CTHULHU);
									}
									else
									{
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.EYE_OF_CTHULHU);
										NetMessage.SendMessage();
									}
								}
							}
							else if (ptr->Type == (int)Item.ID.SUSPICIOUS_LOOKING_SKULL)
							{
								if (!Main.GameTime.DayTime && Main.InHardMode)
								{
									itemTime = ptr->UseTime;
									Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
									if (Main.NetMode != (byte)NetModeSetting.CLIENT)
									{
										NPC.SpawnOnPlayer(this, (int)NPC.ID.OCRAM);
									}
									else
									{
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.OCRAM);
										NetMessage.SendMessage();
									}
								}
							}
							else if (ptr->Type == (int)Item.ID.WORM_FOOD)
							{
								if (ZoneEvil)
								{
									itemTime = ptr->UseTime;
									Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
									if (Main.NetMode != (byte)NetModeSetting.CLIENT)
									{
										NPC.SpawnOnPlayer(this, (int)NPC.ID.EATER_OF_WORLDS_HEAD);
									}
									else
									{
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.EATER_OF_WORLDS_HEAD);
										NetMessage.SendMessage();
									}
								}
							}
							else if (ptr->Type == (int)Item.ID.MECHANICAL_EYE)
							{
								if (!Main.GameTime.DayTime)
								{
									itemTime = ptr->UseTime;
									Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
									if (Main.NetMode != (byte)NetModeSetting.CLIENT)
									{
										NPC.SpawnOnPlayer(this, (int)NPC.ID.RETINAZER);
										NPC.SpawnOnPlayer(this, (int)NPC.ID.SPAZMATISM);
									}
									else
									{
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.RETINAZER);
										NetMessage.SendMessage();
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.SPAZMATISM);
										NetMessage.SendMessage();
									}
								}
							}
							else if (ptr->Type == (int)Item.ID.MECHANICAL_WORM)
							{
								if (!Main.GameTime.DayTime)
								{
									itemTime = ptr->UseTime;
									Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
									if (Main.NetMode != (byte)NetModeSetting.CLIENT)
									{
										NPC.SpawnOnPlayer(this, (int)NPC.ID.THE_DESTROYER_HEAD);
									}
									else
									{
										NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.THE_DESTROYER_HEAD);
										NetMessage.SendMessage();
									}
								}
							}
							else if (ptr->Type == (int)Item.ID.MECHANICAL_SKULL && !Main.GameTime.DayTime)
							{
								itemTime = ptr->UseTime;
								Main.PlaySound(15, XYWH.X, XYWH.Y, 0);
								if (Main.NetMode != (byte)NetModeSetting.CLIENT)
								{
									NPC.SpawnOnPlayer(this, (int)NPC.ID.SKELETRON_PRIME);
								}
								else
								{
									NetMessage.CreateMessage2(61, WhoAmI, (int)NPC.ID.SKELETRON_PRIME);
									NetMessage.SendMessage();
								}
							}
						}
					}
				}
				if (ptr->Type == (int)Item.ID.MAGIC_MIRROR && itemAnimation > 0)
				{
					if (itemTime == 0)
					{
						itemTime = ptr->UseTime;
					}
					else if (itemTime == ptr->UseTime >> 1)
					{
						for (int num72 = 0; num72 < 16; num72++)
						{
							Main.DustSet.NewDust(15, ref XYWH, velocity.X * 0.5f, velocity.Y * 0.5f, 150, default, 1.5f);
						}
						grappling[0] = -1;
						grapCount = 0;
						for (int num73 = 0; num73 < Projectile.MaxNumProjs; num73++)
						{
							if (Main.ProjectileSet[num73].active != 0 && Main.ProjectileSet[num73].owner == i && Main.ProjectileSet[num73].aiStyle == 7)
							{
								Main.ProjectileSet[num73].Kill();
							}
						}
						Spawn();
						for (int num74 = 0; num74 < 32; num74++)
						{
							Main.DustSet.NewDust(15, ref XYWH, 0.0, 0.0, 150, default, 1.5f);
						}
					}
					else if (Main.Rand.Next(3) == 0)
					{
						Main.DustSet.NewDust(15, ref XYWH, 0.0, 0.0, 150, default, 1.1f);
					}
				}
				if (!isLocal())
				{
					return;
				}
				if (itemTime == ptr->UseTime && ptr->IsConsumable)
				{
					bool flag8 = true;
					if (ptr->IsRanged && Main.Rand.Next(100) < freeAmmoChance)
					{
						flag8 = false;
					}
					if (flag8)
					{
						if (ptr->Stack > 0)
						{
							ptr->Stack--;
						}
						if (ptr->Stack <= 0)
						{
							itemTime = (byte)itemAnimation;
						}
					}
				}
				if (ptr->Stack <= 0 && itemAnimation == 0)
				{
					ptr->Init();
				}
				if (SelectedItem == 48 && itemAnimation != 0)
				{
					ui.mouseItem = *ptr;
				}
			}
		}

		public Color GetImmuneAlpha(Color newColor)
		{
			if (immuneAlpha > 125)
			{
				return default;
			}
			double num = (double)(255 - immuneAlpha) * (1f / 255f);
			if (shadow > 0f)
			{
				num *= (double)(1f - shadow);
			}
			int r = (int)(newColor.R * num);
			int g = (int)(newColor.G * num);
			int b = (int)(newColor.B * num);
			int a = (int)(newColor.A * num);
			return new Color(r, g, b, a);
		}

		public Color GetImmuneAlpha2(Color newColor)
		{
			double num = (double)(255 - immuneAlpha) * (1f / 255f);
			if (shadow > 0f)
			{
				num *= (double)(1f - shadow);
			}
			int r = (int)(newColor.R * num);
			int g = (int)(newColor.G * num);
			int b = (int)(newColor.B * num);
			int a = (int)(newColor.A * num);
			return new Color(r, g, b, a);
		}

		public Color GetDeathAlpha(Color newColor)
		{
			int r = newColor.R + (int)(immuneAlpha * 0.9);
			int g = newColor.G + (int)(immuneAlpha * 0.5);
			int b = newColor.B + (int)(immuneAlpha * 0.5);
			int a = newColor.A + (int)(immuneAlpha * 0.4);
			return new Color(r, g, b, a);
		}

		public bool HasItemInInventory(int type)
		{
			for (int i = 0; i < MaxNumInventory + 1; i++)
			{
				if (Inventory[i].Type == type)
				{
					return true;
				}
			}
			return false;
		}

		public void DropCoins()
		{
			for (int i = 0; i <= MaxNumInventory; i++)
			{
				if (Inventory[i].CanBePlacedInCoinSlot())
				{
					short num = (short)(Inventory[i].Stack >> 1);
					num = (short)(Inventory[i].Stack - num);
					int num2 = Item.NewItem(XYWH.X, XYWH.Y, width, height, Inventory[i].Type, num);
					Inventory[i].Stack -= num;
					if (Inventory[i].Stack <= 0)
					{
						Inventory[i].Init();
					}
					Main.ItemSet[num2].Velocity.Y = Main.Rand.Next(-20, 1) * 0.2f;
					Main.ItemSet[num2].Velocity.X = Main.Rand.Next(-20, 21) * 0.2f;
					Main.ItemSet[num2].NoGrabDelay = 100;
					NetMessage.CreateMessage2(21, WhoAmI, num2);
					NetMessage.SendMessage();
					if (i == MaxNumInventory)
					{
						ui.mouseItem = Inventory[i];
					}
				}
			}
		}

		public void DropItems()
		{
			for (int i = 0; i < MaxNumInventory + 1; i++)
			{
				if (Inventory[i].Type > 0 && Inventory[i].NetID != -13 && Inventory[i].NetID != -15 && Inventory[i].NetID != -16)
				{
					int num = Item.NewItem(XYWH.X, XYWH.Y, width, height, Inventory[i].Type);
					Main.ItemSet[num].NetDefaults(Inventory[i].NetID, Inventory[i].Stack);
					Main.ItemSet[num].SetPrefix(Inventory[i].PrefixType);
					Main.ItemSet[num].Velocity.Y = Main.Rand.Next(-20, 1) * 0.2f;
					Main.ItemSet[num].Velocity.X = Main.Rand.Next(-20, 21) * 0.2f;
					Main.ItemSet[num].NoGrabDelay = 100;
					NetMessage.CreateMessage2(21, WhoAmI, num);
					NetMessage.SendMessage();
				}
				Inventory[i].Init();
				if (i < MaxNumArmor)
				{
					if (armor[i].Type > 0)
					{
						int num2 = Item.NewItem(XYWH.X, XYWH.Y, width, height, armor[i].Type);
						Main.ItemSet[num2].NetDefaults(armor[i].NetID, armor[i].Stack);
						Main.ItemSet[num2].SetPrefix(armor[i].PrefixType);
						Main.ItemSet[num2].Velocity.Y = Main.Rand.Next(-20, 1) * 0.2f;
						Main.ItemSet[num2].Velocity.X = Main.Rand.Next(-20, 21) * 0.2f;
						Main.ItemSet[num2].NoGrabDelay = 100;
						NetMessage.CreateMessage2(21, WhoAmI, num2);
						NetMessage.SendMessage();
					}
					armor[i].Init();
				}
			}
			Inventory[0].SetDefaults("Copper Shortsword");
			Inventory[0].SetPrefix(-1);
			Inventory[1].SetDefaults("Copper Pickaxe");
			Inventory[1].SetPrefix(-1);
			Inventory[2].SetDefaults("Copper Axe");
			Inventory[2].SetPrefix(-1);
			ui.mouseItem.Init();
		}

		public Player ShallowCopy()
		{
			return (Player)MemberwiseClone();
		}

		public Player DeepCopy()
		{
			Player player = (Player)MemberwiseClone();
			player.buff = new Buff[MaxNumBuffs];
			player.armor = new Item[MaxNumArmor];
			player.Inventory = new Item[MaxNumInventory + 1];
			player.bank = new Chest();
			player.safe = new Chest();
			player.shadowPos = new Vector2[3];
			player.grappling = new short[20];
			player.adjTile = new Adj[Main.MaxNumTilenames];
			player.grappling[0] = -1;
			for (int i = 0; i < MaxNumBuffs; i++)
			{
				ref Buff reference = ref player.buff[i];
				reference = buff[i];
			}
			for (int j = 0; j < MaxNumArmor; j++)
			{
				ref Item reference2 = ref player.armor[j];
				reference2 = armor[j];
			}
			for (int k = 0; k <= MaxNumInventory; k++)
			{
				ref Item reference3 = ref player.Inventory[k];
				reference3 = Inventory[k];
			}
			for (int l = 0; l < Chest.MaxNumItems; l++)
			{
				ref Item reference4 = ref player.bank.ItemSet[l];
				reference4 = bank.ItemSet[l];
				ref Item reference5 = ref player.safe.ItemSet[l];
				reference5 = safe.ItemSet[l];
			}
			player.spX = new short[200];
			player.spY = new short[200];
			player.spN = new string[200];
			player.spI = new int[200];
			for (int m = 0; m < 200; m++)
			{
				player.spX[m] = spX[m];
				player.spY[m] = spY[m];
				player.spN[m] = spN[m];
				player.spI[m] = spI[m];
			}
			return player;
		}

		public static bool CheckSpawn(int x, int y)
		{
			if (x < 10 || x > Main.MaxTilesX - 10 || y < 10 || y > Main.MaxTilesX - 10)
			{
				return false;
			}
			if (Main.TileSet[x, y - 1].IsActive == 0 || Main.TileSet[x, y - 1].Type != 79)
			{
				return false;
			}
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 3; j < y; j++)
				{
					if (Main.TileSet[i, j].IsActive != 0 && Main.TileSolidNotSolidTop[Main.TileSet[i, j].Type])
					{
						return false;
					}
				}
			}
			if (!WorldGen.StartRoomCheck(x, y - 1))
			{
				return false;
			}
			return true;
		}

		public void FindSpawn()
		{
			for (int i = 0; i < 200; i++)
			{
				if (spN[i] == null)
				{
					SpawnX = -1;
					SpawnY = -1;
					break;
				}
				if (spN[i] == Main.WorldName && spI[i] == Main.WorldID)
				{
					SpawnX = spX[i];
					SpawnY = spY[i];
					break;
				}
			}
		}

		public void ChangeSpawn(int x, int y)
		{
			for (int i = 0; i < 200 && spN[i] != null; i++)
			{
				if (spN[i] == Main.WorldName && spI[i] == Main.WorldID)
				{
					for (int num = i; num > 0; num--)
					{
						spN[num] = spN[num - 1];
						spI[num] = spI[num - 1];
						spX[num] = spX[num - 1];
						spY[num] = spY[num - 1];
					}
					spX[0] = (short)x;
					spY[0] = (short)y;
					spN[0] = Main.WorldName;
					spI[0] = Main.WorldID;
					return;
				}
			}
			for (int num2 = 199; num2 > 0; num2--)
			{
				if (spN[num2 - 1] != null)
				{
					spN[num2] = spN[num2 - 1];
					spI[num2] = spI[num2 - 1];
					spX[num2] = spX[num2 - 1];
					spY[num2] = spY[num2 - 1];
				}
			}
			spX[0] = (short)x;
			spY[0] = (short)y;
			spN[0] = Main.WorldName;
			spI[0] = Main.WorldID;
		}

		public bool Save(string playerPath)
		{
			bool result = true;
			if (ui.HasPlayerStorage())
			{
				if (playerPath == null || playerPath.Length == 0)
				{
					return false;
				}
				using (MemoryStream memoryStream = new MemoryStream(2048))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
#if USE_ORIGINAL_CODE
						binaryWriter.Write((short)Main.PlayerDataVersion);
						binaryWriter.Write(0u);
						binaryWriter.Write(CharacterName);
						binaryWriter.Write(difficulty);
						binaryWriter.Write(hair);
						binaryWriter.Write(male);
						binaryWriter.Write(statLife);
						binaryWriter.Write(StatLifeMax);
						binaryWriter.Write(statMana);
						binaryWriter.Write(statManaMax);
						binaryWriter.Write(hairColor.R);
						binaryWriter.Write(hairColor.G);
						binaryWriter.Write(hairColor.B);
						binaryWriter.Write(skinColor.R);
						binaryWriter.Write(skinColor.G);
						binaryWriter.Write(skinColor.B);
						binaryWriter.Write(eyeColor.R);
						binaryWriter.Write(eyeColor.G);
						binaryWriter.Write(eyeColor.B);
						binaryWriter.Write(shirtColor.R);
						binaryWriter.Write(shirtColor.G);
						binaryWriter.Write(shirtColor.B);
						binaryWriter.Write(underShirtColor.R);
						binaryWriter.Write(underShirtColor.G);
						binaryWriter.Write(underShirtColor.B);
						binaryWriter.Write(pantsColor.R);
						binaryWriter.Write(pantsColor.G);
						binaryWriter.Write(pantsColor.B);
						binaryWriter.Write(shoeColor.R);
						binaryWriter.Write(shoeColor.G);
						binaryWriter.Write(shoeColor.B);
						lock (this)
						{
							for (int i = 0; i < MaxNumArmor; i++)
							{
								binaryWriter.Write(armor[i].NetID);
								binaryWriter.Write(armor[i].PrefixType);
							}
							for (int j = 0; j < MaxNumInventory; j++)
							{
								binaryWriter.Write(Inventory[j].NetID);
								binaryWriter.Write(Inventory[j].Stack);
								binaryWriter.Write(Inventory[j].PrefixType);
							}
							for (int k = 0; k < Chest.MaxNumItems; k++)
							{
								binaryWriter.Write(bank.ItemSet[k].NetID);
								binaryWriter.Write(bank.ItemSet[k].Stack);
								binaryWriter.Write(bank.ItemSet[k].PrefixType);
							}
							for (int l = 0; l < Chest.MaxNumItems; l++)
							{
								binaryWriter.Write(safe.ItemSet[l].NetID);
								binaryWriter.Write(safe.ItemSet[l].Stack);
								binaryWriter.Write(safe.ItemSet[l].PrefixType);
							}
							for (int m = 0; m < MaxNumBuffs; m++)
							{
								binaryWriter.Write(buff[m].Type);
								binaryWriter.Write(buff[m].Time);
							}
						}
						binaryWriter.Write(pet);
						int num = itemsFound.Length + 7 >> 3;
						byte[] array = new byte[num];
						itemsFound.CopyTo(array, 0);
						binaryWriter.Write((ushort)num);
						binaryWriter.Write(array, 0, num);
						num = 43;
						RecipesFound.CopyTo(array, 0);
						binaryWriter.Write((ushort)num);
						binaryWriter.Write(array, 0, num);
						recipesNew.CopyTo(array, 0);
						binaryWriter.Write(array, 0, num);
						num = 17;
						craftingStationsFound.CopyTo(array, 0);
						binaryWriter.Write((ushort)num);
						binaryWriter.Write(array, 0, num);
						for (int n = 0; n < 200; n++)
						{
							if (spN[n] == null)
							{
								binaryWriter.Write((short)(-1));
								break;
							}
							binaryWriter.Write(spX[n]);
							binaryWriter.Write(spY[n]);
							binaryWriter.Write(spI[n]);
							binaryWriter.Write(spN[n]);
						}
						CRC32 cRC = new CRC32();
						cRC.Update(memoryStream.GetBuffer(), 6, (int)memoryStream.Length - 6);
						binaryWriter.Seek(2, SeekOrigin.Begin);
						binaryWriter.Write(cRC.GetValue());
						Main.ShowSaveIcon();
						try
						{
							if (!ui.TestStorageSpace("Characters", playerPath, (int)memoryStream.Length))
							{
								result = false;
							}
							else
							{
								using (StorageContainer storageContainer = ui.OpenPlayerStorage("Characters"))
								{
									using (Stream stream = storageContainer.OpenFile(playerPath, FileMode.Create))
									{
										stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
										stream.Close();
									}
								}
							}
						}
						catch (IOException)
						{
							ui.WriteError();
							result = false;
						}
						catch (Exception)
						{
						}
						binaryWriter.Close();
						Main.HideSaveIcon();
						return result;
#else
						if (Main.PlayerDataVersion == 6)
						{
							binaryWriter.Write((short)Main.PlayerDataVersion);
							binaryWriter.Write(0u);
							binaryWriter.Write(CharacterName);
							binaryWriter.Write(difficulty);
							binaryWriter.Write(hair);
							binaryWriter.Write(male);
							binaryWriter.Write(statLife);
							binaryWriter.Write(StatLifeMax);
							binaryWriter.Write(statMana);
							binaryWriter.Write(statManaMax);
							binaryWriter.Write(hairColor.R);
							binaryWriter.Write(hairColor.G);
							binaryWriter.Write(hairColor.B);
							binaryWriter.Write(skinColor.R);
							binaryWriter.Write(skinColor.G);
							binaryWriter.Write(skinColor.B);
							binaryWriter.Write(eyeColor.R);
							binaryWriter.Write(eyeColor.G);
							binaryWriter.Write(eyeColor.B);
							binaryWriter.Write(shirtColor.R);
							binaryWriter.Write(shirtColor.G);
							binaryWriter.Write(shirtColor.B);
							binaryWriter.Write(underShirtColor.R);
							binaryWriter.Write(underShirtColor.G);
							binaryWriter.Write(underShirtColor.B);
							binaryWriter.Write(pantsColor.R);
							binaryWriter.Write(pantsColor.G);
							binaryWriter.Write(pantsColor.B);
							binaryWriter.Write(shoeColor.R);
							binaryWriter.Write(shoeColor.G);
							binaryWriter.Write(shoeColor.B);
							lock (this)
							{
								for (int i = 0; i < MaxNumArmor; i++)
								{
									binaryWriter.Write(armor[i].NetID);
									binaryWriter.Write(armor[i].PrefixType);
								}
								for (int j = 0; j < MaxNumInventory; j++)
								{
									binaryWriter.Write(Inventory[j].NetID);
									binaryWriter.Write(Inventory[j].Stack);
									binaryWriter.Write(Inventory[j].PrefixType);
								}
								for (int k = 0; k < Chest.MaxNumItems; k++)
								{
									binaryWriter.Write(bank.ItemSet[k].NetID);
									binaryWriter.Write(bank.ItemSet[k].Stack);
									binaryWriter.Write(bank.ItemSet[k].PrefixType);
								}
								for (int l = 0; l < Chest.MaxNumItems; l++)
								{
									binaryWriter.Write(safe.ItemSet[l].NetID);
									binaryWriter.Write(safe.ItemSet[l].Stack);
									binaryWriter.Write(safe.ItemSet[l].PrefixType);
								}
								for (int m = 0; m < MaxNumBuffs; m++)
								{
									binaryWriter.Write(buff[m].Type);
									binaryWriter.Write(buff[m].Time);
								}
							}
							binaryWriter.Write(pet);
							int num = itemsFound.Length + 7 >> 3;
							byte[] array = new byte[num];
							itemsFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
							num = 43;
							RecipesFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
							recipesNew.CopyTo(array, 0);
							binaryWriter.Write(array, 0, num);
							num = 17;
							craftingStationsFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
						}
						else if (Main.PlayerDataVersion == 8)
						{
							binaryWriter.Write((short)Main.PlayerDataVersion);
							binaryWriter.Write(0u);
							binaryWriter.Write(CharacterName);
							binaryWriter.Write(difficulty);
							binaryWriter.Write(hair);
							binaryWriter.Write(male);
							binaryWriter.Write(statLife);
							binaryWriter.Write(StatLifeMax);
							binaryWriter.Write(statMana);
							binaryWriter.Write(statManaMax);
							binaryWriter.Write(hairColor.R);
							binaryWriter.Write(hairColor.G);
							binaryWriter.Write(hairColor.B);
							binaryWriter.Write(skinColor.R);
							binaryWriter.Write(skinColor.G);
							binaryWriter.Write(skinColor.B);
							binaryWriter.Write(eyeColor.R);
							binaryWriter.Write(eyeColor.G);
							binaryWriter.Write(eyeColor.B);
							binaryWriter.Write(shirtColor.R);
							binaryWriter.Write(shirtColor.G);
							binaryWriter.Write(shirtColor.B);
							binaryWriter.Write(underShirtColor.R);
							binaryWriter.Write(underShirtColor.G);
							binaryWriter.Write(underShirtColor.B);
							binaryWriter.Write(pantsColor.R);
							binaryWriter.Write(pantsColor.G);
							binaryWriter.Write(pantsColor.B);
							binaryWriter.Write(shoeColor.R);
							binaryWriter.Write(shoeColor.G);
							binaryWriter.Write(shoeColor.B);
							lock (this)
							{
								for (int i = 0; i < MaxNumArmor; i++)
								{
									binaryWriter.Write(armor[i].NetID);
									if (armor[i].NetID != 0)
									{
										binaryWriter.Write(armor[i].PrefixType);
									}
								}
								for (int j = 0; j < MaxNumInventory; j++)
								{
									binaryWriter.Write(Inventory[j].NetID);
									if (Inventory[j].NetID != 0)
									{
										binaryWriter.Write(Inventory[j].Stack);
										binaryWriter.Write(Inventory[j].PrefixType);
									}
								}
								for (int k = 0; k < Chest.MaxNumItems; k++)
								{
									binaryWriter.Write(bank.ItemSet[k].NetID);
									if (bank.ItemSet[k].NetID != 0)
									{
										binaryWriter.Write(bank.ItemSet[k].Stack);
										binaryWriter.Write(bank.ItemSet[k].PrefixType);
									}
								}
								for (int l = 0; l < Chest.MaxNumItems; l++)
								{
									binaryWriter.Write(safe.ItemSet[l].NetID);
									if (safe.ItemSet[l].NetID != 0)
									{
										binaryWriter.Write(safe.ItemSet[l].Stack);
										binaryWriter.Write(safe.ItemSet[l].PrefixType);
									}
								}
								for (int m = 0; m < MaxNumBuffs; m++)
								{
									binaryWriter.Write((byte)buff[m].Type);
									binaryWriter.Write(buff[m].Time);
								}
							}
							binaryWriter.Write(pet);

#if !VERSION_INITIAL
							binaryWriter.Write(PlayerQuickAccess[0]);
							binaryWriter.Write(PlayerQuickAccess[1]);
							binaryWriter.Write(PlayerQuickAccess[2]);
							binaryWriter.Write(PlayerQuickAccess[3]);
#endif

							int num = 82;	// itemsFound.Length + 7 >> 3;
							byte[] array = new byte[num];
							itemsFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
							num = 45;
							RecipesFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
							recipesNew.CopyTo(array, 0);
							binaryWriter.Write(array, 0, num);
							num = 19;
							craftingStationsFound.CopyTo(array, 0);
							binaryWriter.Write((ushort)num);
							binaryWriter.Write(array, 0, num);
						}
						for (int n = 0; n < 200; n++)
						{
							if (spN[n] == null)
							{
								binaryWriter.Write((short)(-1));
								break;
							}
							binaryWriter.Write(spX[n]);
							binaryWriter.Write(spY[n]);
							binaryWriter.Write(spI[n]);
							binaryWriter.Write(spN[n]);
						}
						CRC32 cRC = new CRC32();
						cRC.Update(memoryStream.GetBuffer(), 6, (int)memoryStream.Length - 6);
						binaryWriter.Seek(2, SeekOrigin.Begin);
						binaryWriter.Write(cRC.GetValue());
						Main.ShowSaveIcon();
						try
						{
							if (!ui.TestStorageSpace("Characters", playerPath, (int)memoryStream.Length))
							{
								result = false;
							}
							else
							{
								using (StorageContainer storageContainer = ui.OpenPlayerStorage("Characters"))
								{
									using (Stream stream = storageContainer.OpenFile(playerPath, FileMode.Create))
									{
										stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
										stream.Close();
									}
								}
							}
						}
						catch (IOException)
						{
							ui.WriteError();
							result = false;
						}
						catch (Exception)
						{
						}
						binaryWriter.Close();
						Main.HideSaveIcon();
						return result;
#endif
					}
				}
			}
			return result;
		}

#if !USE_ORIGINAL_CODE
		public void LoadOld(BinaryReader binaryReader, MemoryStream memoryStream, int num) // Taken partially from Mobile 1.2
		{
			CRC32 cRC = new CRC32();
			cRC.Update(memoryStream.GetBuffer(), 6, (int)memoryStream.Length - 6);
			if (cRC.GetValue() != binaryReader.ReadUInt32())
			{
				throw new InvalidOperationException("Invalid CRC32");
			}
			if (num <= 6)
			{
				CharacterName = binaryReader.ReadString();
				difficulty = binaryReader.ReadByte();
				hair = binaryReader.ReadByte();
				male = binaryReader.ReadBoolean();
				statLife = binaryReader.ReadInt16();
				StatLifeMax = binaryReader.ReadInt16();
				if (StatLifeMax > 400)
				{
					StatLifeMax = 400;
				}
				if (statLife > StatLifeMax)
				{
					statLife = StatLifeMax;
				}
				statMana = binaryReader.ReadInt16();
				statManaMax = binaryReader.ReadInt16();
				if (statManaMax > 200)
				{
					statManaMax = 200;
				}
				if (statMana > 400)
				{
					statMana = 400;
				}
				if (num == 4)
				{
					binaryReader.ReadUInt32();
				}
				hairColor.R = binaryReader.ReadByte();
				hairColor.G = binaryReader.ReadByte();
				hairColor.B = binaryReader.ReadByte();
				skinColor.R = binaryReader.ReadByte();
				skinColor.G = binaryReader.ReadByte();
				skinColor.B = binaryReader.ReadByte();
				eyeColor.R = binaryReader.ReadByte();
				eyeColor.G = binaryReader.ReadByte();
				eyeColor.B = binaryReader.ReadByte();
				shirtColor.R = binaryReader.ReadByte();
				shirtColor.G = binaryReader.ReadByte();
				shirtColor.B = binaryReader.ReadByte();
				underShirtColor.R = binaryReader.ReadByte();
				underShirtColor.G = binaryReader.ReadByte();
				underShirtColor.B = binaryReader.ReadByte();
				pantsColor.R = binaryReader.ReadByte();
				pantsColor.G = binaryReader.ReadByte();
				pantsColor.B = binaryReader.ReadByte();
				shoeColor.R = binaryReader.ReadByte();
				shoeColor.G = binaryReader.ReadByte();
				shoeColor.B = binaryReader.ReadByte();
				for (int i = 0; i < MaxNumArmor; i++)
				{
					int num2 = binaryReader.ReadInt16();
					int pre = binaryReader.ReadByte();
					if (num2 == 0)
					{
						armor[i].Init();
						continue;
					}
					armor[i].NetDefaults(num2);
					armor[i].SetPrefix(pre);
					itemsFound.Set(armor[i].Type, value: true);
				}
				for (int j = 0; j < MaxNumInventory; j++)
				{
					int num3 = binaryReader.ReadInt16();
					int stack = binaryReader.ReadInt16();
					int pre2 = binaryReader.ReadByte();
					if (num3 == 0)
					{
						Inventory[j].Init();
						continue;
					}
					Inventory[j].NetDefaults(num3, stack);
					Inventory[j].SetPrefix(pre2);
					itemsFound.Set(Inventory[j].Type, value: true);
				}
				for (int k = 0; k < Chest.MaxNumItems; k++)
				{
					int num4 = binaryReader.ReadInt16();
					int stack2 = binaryReader.ReadInt16();
					int pre3 = binaryReader.ReadByte();
					if (num4 == 0)
					{
						bank.ItemSet[k].Init();
						continue;
					}
					bank.ItemSet[k].NetDefaults(num4, stack2);
					bank.ItemSet[k].SetPrefix(pre3);
					itemsFound.Set(bank.ItemSet[k].Type, value: true);
				}
				for (int l = 0; l < Chest.MaxNumItems; l++)
				{
					int num5 = binaryReader.ReadInt16();
					int stack3 = binaryReader.ReadInt16();
					int pre4 = binaryReader.ReadByte();
					if (num5 == 0)
					{
						safe.ItemSet[l].Init();
						continue;
					}
					safe.ItemSet[l].NetDefaults(num5, stack3);
					safe.ItemSet[l].SetPrefix(pre4);
					itemsFound.Set(safe.ItemSet[l].Type, value: true);
				}
				for (int m = 0; m < MaxNumBuffs; m++)
				{
					buff[m].Type = binaryReader.ReadUInt16();
					buff[m].Time = binaryReader.ReadUInt16();
				}
				pet = binaryReader.ReadSByte();
				int count = binaryReader.ReadUInt16();
				itemsFound = new BitArray(binaryReader.ReadBytes(count));
				count = binaryReader.ReadUInt16();
				RecipesFound = new BitArray(binaryReader.ReadBytes(count));
				recipesNew = new BitArray(binaryReader.ReadBytes(count));
				count = binaryReader.ReadUInt16();
				craftingStationsFound = new BitArray(binaryReader.ReadBytes(count));
				if (itemsFound.Length < Item.MaxNumItemTypes)
				{
					itemsFound.Length = Item.MaxNumItemTypes;
				}
				if (RecipesFound.Length < Recipe.MaxNumRecipes)
				{
					RecipesFound.Length = Recipe.MaxNumRecipes;
				}
				if (recipesNew.Length < Recipe.MaxNumRecipes)
				{
					recipesNew.Length = Recipe.MaxNumRecipes;
				}
				if (craftingStationsFound.Length < Main.MaxNumTilenames)
				{
					craftingStationsFound.Length = Main.MaxNumTilenames;
				}
				for (int n = 0; n < 200; n++)
				{
					int num6 = binaryReader.ReadInt16();
					if (num6 == -1)
					{
						break;
					}
					spX[n] = (short)num6;
					spY[n] = binaryReader.ReadInt16();
					spI[n] = binaryReader.ReadInt32();
					spN[n] = binaryReader.ReadString();
				}
			}
			else if (num == 8)
			{
				CharacterName = binaryReader.ReadString();
				difficulty = binaryReader.ReadByte();
				hair = binaryReader.ReadByte();
				male = binaryReader.ReadBoolean();
				statLife = binaryReader.ReadInt16();
				StatLifeMax = binaryReader.ReadInt16();
				if (StatLifeMax > 400)
				{
					StatLifeMax = 400;
				}
				if (statLife > StatLifeMax)
				{
					statLife = StatLifeMax;
				}
				statMana = binaryReader.ReadInt16();
				statManaMax = binaryReader.ReadInt16();
				if (statManaMax > 200)
				{
					statManaMax = 200;
				}
				if (statMana > 400)
				{
					statMana = 400;
				}
				hairColor.R = binaryReader.ReadByte();
				hairColor.G = binaryReader.ReadByte();
				hairColor.B = binaryReader.ReadByte();
				skinColor.R = binaryReader.ReadByte();
				skinColor.G = binaryReader.ReadByte();
				skinColor.B = binaryReader.ReadByte();
				eyeColor.R = binaryReader.ReadByte();
				eyeColor.G = binaryReader.ReadByte();
				eyeColor.B = binaryReader.ReadByte();
				shirtColor.R = binaryReader.ReadByte();
				shirtColor.G = binaryReader.ReadByte();
				shirtColor.B = binaryReader.ReadByte();
				underShirtColor.R = binaryReader.ReadByte();
				underShirtColor.G = binaryReader.ReadByte();
				underShirtColor.B = binaryReader.ReadByte();
				pantsColor.R = binaryReader.ReadByte();
				pantsColor.G = binaryReader.ReadByte();
				pantsColor.B = binaryReader.ReadByte();
				shoeColor.R = binaryReader.ReadByte();
				shoeColor.G = binaryReader.ReadByte();
				shoeColor.B = binaryReader.ReadByte();
				for (int i = 0; i < MaxNumArmor; i++)
				{
					int num2 = binaryReader.ReadInt16();

					if (num2 != 0)
					{
						int pre = binaryReader.ReadByte();
						armor[i].NetDefaults(num2);
						armor[i].SetPrefix(pre);
						itemsFound.Set(armor[i].Type, value: true);
					}
					else
					{
						armor[i].Init();
					}
				}
				for (int j = 0; j < MaxNumInventory; j++)
				{
					int num3 = binaryReader.ReadInt16();

					if (num3 != 0)
					{
						int stack = binaryReader.ReadInt16();
						int pre2 = binaryReader.ReadByte();
						Inventory[j].NetDefaults(num3, stack);
						Inventory[j].SetPrefix(pre2);
						itemsFound.Set(Inventory[j].Type, value: true);
					}
					else
					{
						Inventory[j].Init();
					}
				}
				for (int k = 0; k < Chest.MaxNumItems; k++)
				{
					int num4 = binaryReader.ReadInt16();

					if (num4 != 0)
					{
						int stack2 = binaryReader.ReadInt16();
						int pre3 = binaryReader.ReadByte();
						bank.ItemSet[k].NetDefaults(num4, stack2);
						bank.ItemSet[k].SetPrefix(pre3);
						itemsFound.Set(bank.ItemSet[k].Type, value: true);
					}
					else
					{
						bank.ItemSet[k].Init();
					}
				}
				for (int l = 0; l < Chest.MaxNumItems; l++)
				{
					int num5 = binaryReader.ReadInt16();
					if (num5 != 0)
					{
						int stack3 = binaryReader.ReadInt16();
						int pre4 = binaryReader.ReadByte();
						safe.ItemSet[l].NetDefaults(num5, stack3);
						safe.ItemSet[l].SetPrefix(pre4);
						itemsFound.Set(safe.ItemSet[l].Type, value: true);
					}
					else
					{
						safe.ItemSet[l].Init();
					}
				}
				for (int m = 0; m < MaxNumBuffs; m++)
				{
					buff[m].Type = binaryReader.ReadByte();
					buff[m].Time = binaryReader.ReadUInt16();
				}
				pet = binaryReader.ReadSByte();
#if VERSION_101
				PlayerQuickAccess[0] = binaryReader.ReadSByte();
				PlayerQuickAccess[1] = binaryReader.ReadSByte();
				PlayerQuickAccess[2] = binaryReader.ReadSByte();
				PlayerQuickAccess[3] = binaryReader.ReadSByte();
#endif
				int count = binaryReader.ReadUInt16();
				itemsFound = new BitArray(binaryReader.ReadBytes(count));
				count = binaryReader.ReadUInt16();
				RecipesFound = new BitArray(binaryReader.ReadBytes(count));
				recipesNew = new BitArray(binaryReader.ReadBytes(count));
				count = binaryReader.ReadUInt16();
				craftingStationsFound = new BitArray(binaryReader.ReadBytes(count));
				if (itemsFound.Length < Item.MaxNumItemTypes)
				{
					itemsFound.Length = Item.MaxNumItemTypes;
				}
				if (RecipesFound.Length < Recipe.MaxNumRecipes)
				{
					RecipesFound.Length = Recipe.MaxNumRecipes;
				}
				if (recipesNew.Length < Recipe.MaxNumRecipes)
				{
					recipesNew.Length = Recipe.MaxNumRecipes;
				}
				if (craftingStationsFound.Length < Main.MaxNumTilenames)
				{
					craftingStationsFound.Length = Main.MaxNumTilenames;
				}
				for (int n = 0; n < 200; n++)
				{
					int num6 = binaryReader.ReadInt16();
					if (num6 == -1)
					{
						break;
					}
					spX[n] = (short)num6;
					spY[n] = binaryReader.ReadInt16();
					spI[n] = binaryReader.ReadInt32();
					spN[n] = binaryReader.ReadString();
				}
			}
			else
			{
				throw new InvalidOperationException("Invalid version");
			}
			binaryReader.Close();
		}
#endif


		public void Load(StorageContainer c, string playerPath)
		{
			try
			{
				using (Stream stream = c.OpenFile(playerPath, FileMode.Open))
				{
					using (MemoryStream memoryStream = new MemoryStream((int)stream.Length))
					{
#if USE_ORIGINAL_CODE
						memoryStream.SetLength(stream.Length);
						stream.Read(memoryStream.GetBuffer(), 0, (int)stream.Length);
						stream.Close();
						using (BinaryReader binaryReader = new BinaryReader(memoryStream))
						{
							int num = binaryReader.ReadInt16();
							if (num > Main.PlayerDataVersion)
							{
								throw new InvalidOperationException("Invalid version");
							}
							if (num >= Main.PlayerDataVersion)
							{
								CRC32 cRC = new CRC32();
								cRC.Update(memoryStream.GetBuffer(), 6, (int)memoryStream.Length - 6);
								if (cRC.GetValue() != binaryReader.ReadUInt32())
								{
									throw new InvalidOperationException("Invalid CRC32");
								}
							}
							CharacterName = binaryReader.ReadString();
							difficulty = binaryReader.ReadByte();
							hair = binaryReader.ReadByte();
							male = binaryReader.ReadBoolean();
							statLife = binaryReader.ReadInt16();
							StatLifeMax = binaryReader.ReadInt16();
							if (StatLifeMax > 400)
							{
								StatLifeMax = 400;
							}
							if (statLife > StatLifeMax)
							{
								statLife = StatLifeMax;
							}
							statMana = binaryReader.ReadInt16();
							statManaMax = binaryReader.ReadInt16();
							if (statManaMax > 200)
							{
								statManaMax = 200;
							}
							if (statMana > 400)
							{
								statMana = 400;
							}
							if (num == 4)
							{
								binaryReader.ReadUInt32();
							}
							hairColor.R = binaryReader.ReadByte();
							hairColor.G = binaryReader.ReadByte();
							hairColor.B = binaryReader.ReadByte();
							skinColor.R = binaryReader.ReadByte();
							skinColor.G = binaryReader.ReadByte();
							skinColor.B = binaryReader.ReadByte();
							eyeColor.R = binaryReader.ReadByte();
							eyeColor.G = binaryReader.ReadByte();
							eyeColor.B = binaryReader.ReadByte();
							shirtColor.R = binaryReader.ReadByte();
							shirtColor.G = binaryReader.ReadByte();
							shirtColor.B = binaryReader.ReadByte();
							underShirtColor.R = binaryReader.ReadByte();
							underShirtColor.G = binaryReader.ReadByte();
							underShirtColor.B = binaryReader.ReadByte();
							pantsColor.R = binaryReader.ReadByte();
							pantsColor.G = binaryReader.ReadByte();
							pantsColor.B = binaryReader.ReadByte();
							shoeColor.R = binaryReader.ReadByte();
							shoeColor.G = binaryReader.ReadByte();
							shoeColor.B = binaryReader.ReadByte();
							for (int i = 0; i < MaxNumArmor; i++)
							{
								int num2 = binaryReader.ReadInt16();
								int pre = binaryReader.ReadByte();
								if (num2 == 0)
								{
									armor[i].Init();
									continue;
								}
								armor[i].NetDefaults(num2);
								armor[i].SetPrefix(pre);
								itemsFound.Set(armor[i].Type, value: true);
							}
							for (int j = 0; j < MaxNumInventory; j++)
							{
								int num3 = binaryReader.ReadInt16();
								int stack = binaryReader.ReadInt16();
								int pre2 = binaryReader.ReadByte();
								if (num3 == 0)
								{
									Inventory[j].Init();
									continue;
								}
								Inventory[j].NetDefaults(num3, stack);
								Inventory[j].SetPrefix(pre2);
								itemsFound.Set(Inventory[j].Type, value: true);
							}
							for (int k = 0; k < Chest.MaxNumItems; k++)
							{
								int num4 = binaryReader.ReadInt16();
								int stack2 = binaryReader.ReadInt16();
								int pre3 = binaryReader.ReadByte();
								if (num4 == 0)
								{
									bank.ItemSet[k].Init();
									continue;
								}
								bank.ItemSet[k].NetDefaults(num4, stack2);
								bank.ItemSet[k].SetPrefix(pre3);
								itemsFound.Set(bank.ItemSet[k].Type, value: true);
							}
							for (int l = 0; l < Chest.MaxNumItems; l++)
							{
								int num5 = binaryReader.ReadInt16();
								int stack3 = binaryReader.ReadInt16();
								int pre4 = binaryReader.ReadByte();
								if (num5 == 0)
								{
									safe.ItemSet[l].Init();
									continue;
								}
								safe.ItemSet[l].NetDefaults(num5, stack3);
								safe.ItemSet[l].SetPrefix(pre4);
								itemsFound.Set(safe.ItemSet[l].Type, value: true);
							}
							for (int m = 0; m < MaxNumBuffs; m++)
							{
								buff[m].Type = binaryReader.ReadUInt16();
								buff[m].Time = binaryReader.ReadUInt16();
							}
							if (num >= 1)
							{
								pet = binaryReader.ReadSByte();
							}
							if (num >= 2)
							{
								int count = binaryReader.ReadUInt16();
								itemsFound = new BitArray(binaryReader.ReadBytes(count));
								if (itemsFound.Length < Item.MaxNumItemTypes)
								{
									itemsFound.Length = Item.MaxNumItemTypes;
								}
								count = binaryReader.ReadUInt16();
								RecipesFound = new BitArray(binaryReader.ReadBytes(count));
								recipesNew = new BitArray(binaryReader.ReadBytes(count));
								if (num >= 3)
								{
									count = binaryReader.ReadUInt16();
									craftingStationsFound = new BitArray(binaryReader.ReadBytes(count));
								}
								else
								{
									InitKnownCraftingStations();
								}
							}
							else
							{
								InitKnownItems();
							}
							for (int n = 0; n < 200; n++)
							{
								int num6 = binaryReader.ReadInt16();
								if (num6 == -1)
								{
									break;
								}
								spX[n] = (short)num6;
								spY[n] = binaryReader.ReadInt16();
								spI[n] = binaryReader.ReadInt32();
								spN[n] = binaryReader.ReadString();
							}
							binaryReader.Close();
						}
#else
						memoryStream.SetLength(stream.Length);
						stream.Read(memoryStream.GetBuffer(), 0, (int)stream.Length);
						stream.Close();
						using (BinaryReader binaryReader = new BinaryReader(memoryStream))
						{
							int num = binaryReader.ReadInt16();
							if (num > Main.PlayerDataVersion)
							{
								throw new InvalidOperationException("Invalid version");
							}

							if (num < Main.PlayerDataVersion)
							{
								LoadOld(binaryReader, memoryStream, num);
							}
							else
							{
								if (num >= Main.PlayerDataVersion) // Checking for greater values after eliminating them in the segment above, nice one.
								{
									CRC32 cRC = new CRC32();
									cRC.Update(memoryStream.GetBuffer(), 6, (int)memoryStream.Length - 6);
									if (cRC.GetValue() != binaryReader.ReadUInt32())
									{
										throw new InvalidOperationException("Invalid CRC32");
									}
								}

								if (num <= 6)
								{
									CharacterName = binaryReader.ReadString();
									difficulty = binaryReader.ReadByte();
									hair = binaryReader.ReadByte();
									male = binaryReader.ReadBoolean();
									statLife = binaryReader.ReadInt16();
									StatLifeMax = binaryReader.ReadInt16();
									if (StatLifeMax > 400)
									{
										StatLifeMax = 400;
									}
									if (statLife > StatLifeMax)
									{
										statLife = StatLifeMax;
									}
									statMana = binaryReader.ReadInt16();
									statManaMax = binaryReader.ReadInt16();
									if (statManaMax > 200)
									{
										statManaMax = 200;
									}
									if (statMana > 400)
									{
										statMana = 400;
									}
									if (num == 4) // Prototype Information
									{
										binaryReader.ReadUInt32();
									}
									hairColor.R = binaryReader.ReadByte();
									hairColor.G = binaryReader.ReadByte();
									hairColor.B = binaryReader.ReadByte();
									skinColor.R = binaryReader.ReadByte();
									skinColor.G = binaryReader.ReadByte();
									skinColor.B = binaryReader.ReadByte();
									eyeColor.R = binaryReader.ReadByte();
									eyeColor.G = binaryReader.ReadByte();
									eyeColor.B = binaryReader.ReadByte();
									shirtColor.R = binaryReader.ReadByte();
									shirtColor.G = binaryReader.ReadByte();
									shirtColor.B = binaryReader.ReadByte();
									underShirtColor.R = binaryReader.ReadByte();
									underShirtColor.G = binaryReader.ReadByte();
									underShirtColor.B = binaryReader.ReadByte();
									pantsColor.R = binaryReader.ReadByte();
									pantsColor.G = binaryReader.ReadByte();
									pantsColor.B = binaryReader.ReadByte();
									shoeColor.R = binaryReader.ReadByte();
									shoeColor.G = binaryReader.ReadByte();
									shoeColor.B = binaryReader.ReadByte();
									for (int i = 0; i < MaxNumArmor; i++)
									{
										int num2 = binaryReader.ReadInt16();
										int pre = binaryReader.ReadByte();
										if (num2 == 0)
										{
											armor[i].Init();
											continue;
										}
										armor[i].NetDefaults(num2);
										armor[i].SetPrefix(pre);
										itemsFound.Set(armor[i].Type, value: true);
									}
									for (int j = 0; j < MaxNumInventory; j++)
									{
										int num3 = binaryReader.ReadInt16();
										int stack = binaryReader.ReadInt16();
										int pre2 = binaryReader.ReadByte();
										if (num3 == 0)
										{
											Inventory[j].Init();
											continue;
										}
										Inventory[j].NetDefaults(num3, stack);
										Inventory[j].SetPrefix(pre2);
										itemsFound.Set(Inventory[j].Type, value: true);
									}
									for (int k = 0; k < Chest.MaxNumItems; k++)
									{
										int num4 = binaryReader.ReadInt16();
										int stack2 = binaryReader.ReadInt16();
										int pre3 = binaryReader.ReadByte();
										if (num4 == 0)
										{
											bank.ItemSet[k].Init();
											continue;
										}
										bank.ItemSet[k].NetDefaults(num4, stack2);
										bank.ItemSet[k].SetPrefix(pre3);
										itemsFound.Set(bank.ItemSet[k].Type, value: true);
									}
									for (int l = 0; l < Chest.MaxNumItems; l++)
									{
										int num5 = binaryReader.ReadInt16();
										int stack3 = binaryReader.ReadInt16();
										int pre4 = binaryReader.ReadByte();
										if (num5 == 0)
										{
											safe.ItemSet[l].Init();
											continue;
										}
										safe.ItemSet[l].NetDefaults(num5, stack3);
										safe.ItemSet[l].SetPrefix(pre4);
										itemsFound.Set(safe.ItemSet[l].Type, value: true);
									}
									for (int m = 0; m < MaxNumBuffs; m++)
									{
										buff[m].Type = binaryReader.ReadUInt16();
										buff[m].Time = binaryReader.ReadUInt16();
									}
									if (num >= 1) // Prototype Information; Pets were added first it seems.
									{
										pet = binaryReader.ReadSByte();
									}
									if (num >= 2) // Prototype Information; After pets, it looks like crafting memory was implemented.
									{
										int count = binaryReader.ReadUInt16();
										itemsFound = new BitArray(binaryReader.ReadBytes(count));
										if (itemsFound.Length < Item.MaxNumItemTypes)
										{
											itemsFound.Length = Item.MaxNumItemTypes;
										}
										count = binaryReader.ReadUInt16();
										RecipesFound = new BitArray(binaryReader.ReadBytes(count));
										recipesNew = new BitArray(binaryReader.ReadBytes(count));
										if (num >= 3) // Prototype Information
										{
											count = binaryReader.ReadUInt16();
											craftingStationsFound = new BitArray(binaryReader.ReadBytes(count));
										}
										else
										{
											InitKnownCraftingStations();
										}
									}
									else
									{
										InitKnownItems();
									}
									for (int n = 0; n < 200; n++)
									{
										int num6 = binaryReader.ReadInt16();
										if (num6 == -1)
										{
											break;
										}
										spX[n] = (short)num6;
										spY[n] = binaryReader.ReadInt16();
										spI[n] = binaryReader.ReadInt32();
										spN[n] = binaryReader.ReadString();
									}
								}
								else if (num == 8)
								{
									CharacterName = binaryReader.ReadString();
									difficulty = binaryReader.ReadByte();
									hair = binaryReader.ReadByte();
									male = binaryReader.ReadBoolean();
									statLife = binaryReader.ReadInt16();
									StatLifeMax = binaryReader.ReadInt16();
									if (StatLifeMax > 400)
									{
										StatLifeMax = 400;
									}
									if (statLife > StatLifeMax)
									{
										statLife = StatLifeMax;
									}
									statMana = binaryReader.ReadInt16();
									statManaMax = binaryReader.ReadInt16();
									if (statManaMax > 200)
									{
										statManaMax = 200;
									}
									if (statMana > 400)
									{
										statMana = 400;
									}
									hairColor.R = binaryReader.ReadByte();
									hairColor.G = binaryReader.ReadByte();
									hairColor.B = binaryReader.ReadByte();
									skinColor.R = binaryReader.ReadByte();
									skinColor.G = binaryReader.ReadByte();
									skinColor.B = binaryReader.ReadByte();
									eyeColor.R = binaryReader.ReadByte();
									eyeColor.G = binaryReader.ReadByte();
									eyeColor.B = binaryReader.ReadByte();
									shirtColor.R = binaryReader.ReadByte();
									shirtColor.G = binaryReader.ReadByte();
									shirtColor.B = binaryReader.ReadByte();
									underShirtColor.R = binaryReader.ReadByte();
									underShirtColor.G = binaryReader.ReadByte();
									underShirtColor.B = binaryReader.ReadByte();
									pantsColor.R = binaryReader.ReadByte();
									pantsColor.G = binaryReader.ReadByte();
									pantsColor.B = binaryReader.ReadByte();
									shoeColor.R = binaryReader.ReadByte();
									shoeColor.G = binaryReader.ReadByte();
									shoeColor.B = binaryReader.ReadByte();
									for (int i = 0; i < MaxNumArmor; i++)
									{
										int num2 = binaryReader.ReadInt16();

										if (num2 != 0)
										{
											int pre = binaryReader.ReadByte();
											armor[i].NetDefaults(num2);
											armor[i].SetPrefix(pre);
											itemsFound.Set(armor[i].Type, value: true);
										}
										else
										{
											armor[i].Init();
										}
									}
									for (int j = 0; j < MaxNumInventory; j++)
									{
										int num3 = binaryReader.ReadInt16();

										if (num3 != 0)
										{
											int stack = binaryReader.ReadInt16();
											int pre2 = binaryReader.ReadByte();
											Inventory[j].NetDefaults(num3, stack);
											Inventory[j].SetPrefix(pre2);
											itemsFound.Set(Inventory[j].Type, value: true);
										}
										else
										{
											Inventory[j].Init();
										}
									}
									for (int k = 0; k < Chest.MaxNumItems; k++)
									{
										int num4 = binaryReader.ReadInt16();

										if (num4 != 0)
										{
											int stack2 = binaryReader.ReadInt16();
											int pre3 = binaryReader.ReadByte();
											bank.ItemSet[k].NetDefaults(num4, stack2);
											bank.ItemSet[k].SetPrefix(pre3);
											itemsFound.Set(bank.ItemSet[k].Type, value: true);
										}
										else
										{
											bank.ItemSet[k].Init();
										}
									}
									for (int l = 0; l < Chest.MaxNumItems; l++)
									{
										int num5 = binaryReader.ReadInt16();
										if (num5 != 0)
										{
											int stack3 = binaryReader.ReadInt16();
											int pre4 = binaryReader.ReadByte();
											safe.ItemSet[l].NetDefaults(num5, stack3);
											safe.ItemSet[l].SetPrefix(pre4);
											itemsFound.Set(safe.ItemSet[l].Type, value: true);
										}
										else
										{
											safe.ItemSet[l].Init();
										}
									}
									for (int m = 0; m < MaxNumBuffs; m++)
									{
										buff[m].Type = binaryReader.ReadByte();
										buff[m].Time = binaryReader.ReadUInt16();
									}
									pet = binaryReader.ReadSByte();
#if VERSION_101
									PlayerQuickAccess[0] = binaryReader.ReadSByte();
									PlayerQuickAccess[1] = binaryReader.ReadSByte();
									PlayerQuickAccess[2] = binaryReader.ReadSByte();
									PlayerQuickAccess[3] = binaryReader.ReadSByte();
#endif
									int count = binaryReader.ReadUInt16();
									itemsFound = new BitArray(binaryReader.ReadBytes(count));
									count = binaryReader.ReadUInt16();
									RecipesFound = new BitArray(binaryReader.ReadBytes(count));
									recipesNew = new BitArray(binaryReader.ReadBytes(count));
									count = binaryReader.ReadUInt16();
									craftingStationsFound = new BitArray(binaryReader.ReadBytes(count));
									if (itemsFound.Length < Item.MaxNumItemTypes)
									{
										itemsFound.Length = Item.MaxNumItemTypes;
									}
									if (RecipesFound.Length < Recipe.MaxNumRecipes)
									{
										RecipesFound.Length = Recipe.MaxNumRecipes;
									}
									if (recipesNew.Length < Recipe.MaxNumRecipes)
									{
										recipesNew.Length = Recipe.MaxNumRecipes;
									}
									if (craftingStationsFound.Length < Main.MaxNumTilenames)
									{
										craftingStationsFound.Length = Main.MaxNumTilenames;
									}
									for (int n = 0; n < 200; n++)
									{
										int num6 = binaryReader.ReadInt16();
										if (num6 == -1)
										{
											break;
										}
										spX[n] = (short)num6;
										spY[n] = binaryReader.ReadInt16();
										spI[n] = binaryReader.ReadInt32();
										spN[n] = binaryReader.ReadString();
									}
								}
								else
								{
									throw new InvalidOperationException("Invalid version");
								}
								binaryReader.Close();
							}
						}
#endif
					}
				}
				PlayerFrame();
			}
			catch
			{
				Main.ShowSaveIcon();
				c.DeleteFile(playerPath);
				Name = null;
				Main.HideSaveIcon();
			}
		}

		public bool HasItem(int type)
		{
			for (int i = 0; i < MaxNumInventory; i++)
			{
				if (type == Inventory[i].Type)
				{
					return true;
				}
			}
			return false;
		}

		public void UpdateGrappleItemSlot()
		{
			int num = -1;
			if (!noItems)
			{
				for (int i = 0; i < MaxNumInventory; i++)
				{
					if (Inventory[i].Shoot == 13 || Inventory[i].Shoot == 32 || Inventory[i].Shoot == 73)
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					int shoot = Inventory[num].Shoot;
					if (shoot == 73)
					{
						int num2 = 0;
						for (int j = 0; j < Projectile.MaxNumProjs; j++)
						{
							if ((Main.ProjectileSet[j].type == 73 || Main.ProjectileSet[j].type == 74) && Main.ProjectileSet[j].active != 0 && Main.ProjectileSet[j].owner == WhoAmI && ++num2 > 1)
							{
								num = -1;
								break;
							}
						}
					}
					else
					{
						for (int k = 0; k < Projectile.MaxNumProjs; k++)
						{
							if (Main.ProjectileSet[k].type == shoot && Main.ProjectileSet[k].active != 0 && Main.ProjectileSet[k].owner == WhoAmI && Main.ProjectileSet[k].ai0 != 2f)
							{
								num = -1;
								break;
							}
						}
					}
				}
			}
			grappleItemSlot = (sbyte)num;
		}

		public void QuickGrapple()
		{
			int num = grappleItemSlot;
			if (num < 0)
			{
				return;
			}
			Main.PlaySound(2, XYWH.X, XYWH.Y, Inventory[num].UseSound);
			if (isLocal())
			{
				NetMessage.CreateMessage1(51, WhoAmI);
				NetMessage.SendMessage();
			}
			int num2 = Inventory[num].Shoot;
			float shootSpeed = Inventory[num].ShootSpeed;
			int damage = Inventory[num].Damage;
			float knockBack = Inventory[num].Knockback;
			switch (num2)
			{
			case 13:
			case 32:
			{
				grappling[0] = -1;
				grapCount = 0;
				for (int j = 0; j < Projectile.MaxNumProjs; j++)
				{
					if (Main.ProjectileSet[j].active != 0 && Main.ProjectileSet[j].owner == WhoAmI && Main.ProjectileSet[j].type == 13)
					{
						Main.ProjectileSet[j].Kill();
					}
				}
				break;
			}
			case 73:
			{
				for (int i = 0; i < Projectile.MaxNumProjs; i++)
				{
					if (Main.ProjectileSet[i].active != 0 && Main.ProjectileSet[i].owner == WhoAmI && Main.ProjectileSet[i].type == 73)
					{
						num2 = 74;
						break;
					}
				}
				break;
			}
			}
			Vector2 vector = new Vector2(Position.X + (width / 2), Position.Y + (height / 2));
			float x = controlDir.X;
			float y = controlDir.Y;
			float num3 = x * x + y * y;
			if (num3 > 0f)
			{
				num3 = shootSpeed / (float)Math.Sqrt(num3);
				x *= num3;
				y *= num3;
				Projectile.NewProjectile(vector.X, vector.Y, x, y, num2, damage, knockBack, WhoAmI);
			}
		}

		public Player()
		{
			for (int i = 0; i <= MaxNumInventory; i++)
			{
				if (i < MaxNumArmor)
				{
					armor[i].Init();
				}
				Inventory[i].Init();
			}
			for (int j = 0; j < Chest.MaxNumItems; j++)
			{
				bank.ItemSet[j].Init();
				safe.ItemSet[j].Init();
			}
			grappling[0] = -1;
			Inventory[0].SetDefaults("Copper Shortsword");
			Inventory[1].SetDefaults("Copper Pickaxe");
			Inventory[2].SetDefaults("Copper Axe");
#if !USE_ORIGINAL_CODE
			if (Main.CollectorsEditionPC)
			{
				Inventory[3].SetDefaults((int)Item.ID.PET_SPAWN_1);
			}
#endif
			InitKnownItems();
			InitKnownCraftingStations();
		}

		public void InitKnownItems()
		{
			itemsFound.Set((int)Item.ID.WOOD, value: true);
			itemsFound.Set((int)Item.ID.GEL, value: true);
			itemsFound.Set((int)Item.ID.STONE_BLOCK, value: true);
			itemsFound.Set((int)Item.ID.DIRT_BLOCK, value: true);
			itemsFound.Set((int)Item.ID.LENS, value: true);
			itemsFound.Set((int)Item.ID.BOTTLE, value: true);
			itemsFound.Set((int)Item.ID.ROTTEN_CHUNK, value: true);
		}

		public void InitKnownCraftingStations()
		{
			craftingStationsFound.Set(13, value: true);
			craftingStationsFound.Set(15, value: true);
			craftingStationsFound.Set(18, value: true);
		}

		public void UpdateEditSign()
		{
			if (sign == -1)
			{
				ui.editSign = false;
				return;
			}
			ui.npcChatText = ui.GetInputText(ui.npcChatText);
			if (ui.inputTextEnter)
			{
				ui.inputTextEnter = false;
				Main.PlaySound(12);
				int num = sign;
				Main.SignSet[num].SetText(ui.npcChatText);
				ui.editSign = false;
				if (Main.NetMode == (byte)NetModeSetting.CLIENT)
				{
					NetMessage.CreateMessage2(47, WhoAmI, num);
					NetMessage.SendMessage();
				}
			}
		}

		public void UpdateMouse()
		{
			int num = XYWH.X + (width / 2) - CurrentView.ScreenPosition.X;
			int num2 = XYWH.Y + (height / 2) - CurrentView.ScreenPosition.Y;
			if (num > CurrentView.ViewWidth || num < 0 || num2 > Main.ResolutionHeight || num2 < 0)
			{
				ui.MouseX = (short)((CurrentView.ViewWidth >> 1) + (direction << 5));
				ui.MouseY = (short)(Main.ResolutionHeight / 2);
			}
			else
			{
				int num3 = Inventory[SelectedItem].TileBoost + blockRange;
				int num4 = 5 + num3 << 4;
				relativeTargetX += ui.PadState.ThumbSticks.Right.X * 6f;
				relativeTargetY -= ui.PadState.ThumbSticks.Right.Y * 6f;
				if (relativeTargetX <= -num4)
				{
					relativeTargetX = -(num4 - 1);
				}
				else if (relativeTargetX >= num4)
				{
					relativeTargetX = num4 - 1;
				}
				int num5 = (int)relativeTargetX;
				int num6 = num + num5;
				if (num6 < 0)
				{
					relativeTargetX -= num6;
					num6 = 0;
				}
				else if (num6 >= CurrentView.ViewWidth)
				{
					int num7 = CurrentView.ViewWidth - 1 - num6;
					relativeTargetX += num7;
					num6 += num7;
				}
				ui.MouseX = (short)num6;
				num4 = 4 + num3 << 4;
				if (relativeTargetY <= -num4)
				{
					relativeTargetY = -(num4 - 1);
				}
				else if (relativeTargetY >= num4)
				{
					relativeTargetY = num4 - 1;
				}
				num6 = num2 + (int)relativeTargetY;
				if (num6 < 0)
				{
					relativeTargetY -= num6;
					num6 = 0;
				}
				else if (num6 >= Main.ResolutionHeight)
				{
					int num8 = Main.ResolutionHeight - 1 - num6;
					relativeTargetY += num8;
					num6 += num8;
				}
				ui.MouseY = (short)num6;
			}
			controlDir.X = ui.MouseX - num;
			controlDir.Y = ui.MouseY - num2;
		}

        public unsafe void UpdateMouseSmart()
        {
            int num = XYWH.X + (width / 2) - CurrentView.ScreenPosition.X;
            int num2 = XYWH.Y + (height / 2) - CurrentView.ScreenPosition.Y;
            fixed (Item* ptr = &Inventory[SelectedItem])
            {
                Vector2 right = ui.PadState.ThumbSticks.Right;
                Vector2 vector = right;
                bool flag = right.LengthSquared() <= (1f / 64f);
                if (!flag)
                {
                    vector.Normalize();
                }
                Vector2 left = ui.PadState.ThumbSticks.Left;
                Vector2 vector2 = left;
                bool flag2 = left.LengthSquared() <= (1f / 64f);
                if (!flag2)
                {
                    vector2.Normalize();
                }
                int num3 = 0;
                if (ptr->Type > 0)
                {
                    if (flag)
                    {
                        if (flag2)
                        {
                            controlDir.X = direction;
                            controlDir.Y = 0f;
                        }
                        else
                        {
                            controlDir.X = vector2.X;
                            controlDir.Y = 0f - vector2.Y;
                        }
                    }
                    else
                    {
                        controlDir.X = vector.X;
                        controlDir.Y = 0f - vector.Y;
                    }
                    int num4 = ptr->TileBoost + blockRange;
                    Vector2 vector3 = new Vector2((0f - controlDir.Y) * 16f, controlDir.X * 16f);
                    int num5 = XYWH.X;
                    int num6 = XYWH.Y + 21;
                    if (controlDir.X >= 0f)
                    {
                        num5 += 20;
                    }
                    double num7 = num5;
                    double num8 = num6;
#if (!IS_PATCHED && VERSION_INITIAL)
					for (int num9 = 2; num9 >= 0; num9--)
                    {
                        double num10 = num7 * 0.0625;
                        double num11 = num8 * 0.0625;
                        int num12 = (int)num10 + (5 + num4) * ((!(this.controlDir.X < 0f)) ? 1 : (-1));
                        int num13 = (int)num11 + (5 + num4) * ((!(this.controlDir.Y < 0f)) ? 1 : (-1));
                        while (true)
                        {
                            int num14 = (int)num10;
                            int num15 = (int)num11;
                            int type = Main.TileSet[num14, num15].Type;
                            bool flag3 = (ptr->AxePower > 0 && Main.TileAxe[type]) || (ptr->HammerPower > 0 && (Main.TileHammer[type] || (Main.TileSet[num14, num15].WallType > 0 && WorldGen.CanKillWall(num14, num15))));
                            if (flag3 || ((ptr->PickPower > 0 || ptr->CreateTile >= 0) && Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type]) || (ptr->CreateWall >= 0 && Main.TileSet[num14, num15].WallType == 0))
                            {
                                if (flag3)
                                {
                                    if (Main.TileAxe[type] && (!Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type]))
                                    {
                                        num14--;
                                        if (!Main.TileAxe[Main.TileSet[num14, num15].Type] || !Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type])
                                        {
                                            num14 += 2;
                                            if (!Main.TileAxe[Main.TileSet[num14, num15].Type] || !Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type])
                                            {
                                                num14--;
                                            }
                                        }
                                    }
                                }
                                else if (ptr->PickPower > 0)
                                {
                                    if (Main.TileAxe[type] || Main.TileHammer[type] || !WorldGen.CanKillTile(num14, num15))
                                    {
                                        goto IL_055d;
                                    }
                                }
                                else if (ptr->CreateTile >= 0)
                                {
                                    num14 = (int)(num10 - (double)this.controlDir.X);
                                    if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                    {
                                        num14 = (int)num10;
                                        num15 = (int)(num11 - (double)this.controlDir.Y);
                                        if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                        {
                                            num14 = (int)(num10 - (double)this.controlDir.X);
                                            if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                            {
                                                num14 = (int)num10;
                                                num15 = (int)num11;
                                                goto IL_055d;
                                            }
                                        }
                                    }
                                    int j = num15;
                                    if (!WorldGen.CanPlaceTile(num14, ref j, ptr->CreateTile, -1))
                                    {
                                        num14 = (int)num10;
                                        num15 = (int)num11;
                                        goto IL_055d;
                                    }
                                }
                                this.smartLocation[num3].X = (num14 << 4) + 8;
                                this.smartLocation[num3].Y = (num15 << 4) + 8;
                                num3++;
                                break;
                            }
#else
                    for (int num9 = 2; num9 >= 0; num9--)
                    {
                        double num10 = num7 * 0.0625;
                        double num11 = num8 * 0.0625;
                        int num12 = (int)num10 + (5 + num4) * ((!(controlDir.X < 0f)) ? 1 : (-1));
                        int num13 = (int)num11 + (5 + num4) * ((!(controlDir.Y < 0f)) ? 1 : (-1));
                        while (true)
                        {
                            int num14 = (int)num10;
                            int num15 = (int)num11;
                            if (num14 < 0 || num15 < 0)
                            {
                                break;
                            }
                            int num16 = Main.TileSet[num14, num15].IsActive;
                            int type = Main.TileSet[num14, num15].Type;
                            bool flag3 = (ptr->AxePower > 0 && num16 != 0 && Main.TileAxe[type]) || (ptr->HammerPower > 0 && ((num16 != 0 && Main.TileHammer[type]) || (Main.TileSet[num14, num15].WallType > 0 && WorldGen.CanKillWall(num14, num15))));
                            if (flag3 || ((ptr->PickPower > 0 || ptr->CreateTile >= 0) && num16 != 0 && Main.TileSolid[type]) || (ptr->CreateWall >= 0 && Main.TileSet[num14, num15].WallType == 0))
                            {
                                if (flag3)
                                {
                                    if (num16 != 0 && Main.TileAxe[type] && (Main.TileSet[num14, num15 - 1].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || Main.TileSet[num14, num15 - 2].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type]))
                                    {
                                        num14--;
                                        if (Main.TileSet[num14, num15].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15].Type] || Main.TileSet[num14, num15 - 1].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || Main.TileSet[num14, num15 - 2].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type])
                                        {
                                            num14 += 2;
                                            if (Main.TileSet[num14, num15].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15].Type] || Main.TileSet[num14, num15 - 1].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 1].Type] || Main.TileSet[num14, num15 - 2].IsActive == 0 || !Main.TileAxe[Main.TileSet[num14, num15 - 2].Type])
                                            {
                                                num14--;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (num16 == 0)
                                    {
                                        goto IL_055d;
                                    }
                                    if (ptr->PickPower > 0)
                                    {
                                        if (Main.TileAxe[type] || Main.TileHammer[type] || !WorldGen.CanKillTile(num14, num15))
                                        {
                                            goto IL_055d;
                                        }
                                    }
                                    else if (ptr->CreateTile >= 0)
                                    {
                                        num14 = (int)(num10 - controlDir.X);
                                        if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                        {
                                            num14 = (int)num10;
                                            num15 = (int)(num11 - controlDir.Y);
                                            if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                            {
                                                num14 = (int)(num10 - controlDir.X);
                                                if (Main.TileSet[num14, num15].IsActive != 0 && Main.TileSolid[type])
                                                {
                                                    num14 = (int)num10;
                                                    num15 = (int)num11;
                                                    goto IL_055d;
                                                }
                                            }
                                        }
                                        int j = num15;
                                        if (!WorldGen.CanPlaceTile(num14, ref j, ptr->CreateTile, -1))
                                        {
                                            num14 = (int)num10;
                                            num15 = (int)num11;
                                            goto IL_055d;
                                        }
                                    }
                                }
                                smartLocation[num3].X = (num14 << 4) + 8;
                                smartLocation[num3].Y = (num15 << 4) + 8;
                                num3++;
                                break;
                            }
#endif
                            goto IL_055d;
							IL_055d:
							if (num14 == num12 || num15 == num13)
							{
								break;
							}
							num10 += controlDir.X;
							num11 += controlDir.Y;
						}
						if (num9 == 1)
						{
							num7 -= vector3.X;
							num8 -= vector3.Y;
							num7 -= vector3.X;
							num8 -= vector3.Y;
						}
						else
						{
							num7 += vector3.X;
							num8 += vector3.Y;
						}
					}
					if (num3 > 0)
					{
						int num16 = 0;
						if (num3 > 1)
						{
							int num17 = num5;
							int num18 = num6;
							if (ptr->CreateTile == 4)
							{
								num17 += (int)(controlDir.X * 256f);
								num18 += (int)(controlDir.Y * 256f);
							}
							else if (ptr->PickPower <= 0 && ptr->HammerPower <= 0 && ptr->CreateWall < 0 && ptr->CreateTile < 0 && ptr->AxePower > 0)
							{
								num18 += 42;
							}
							int num19 = num17 - smartLocation[0].X;
							int num20 = num18 - smartLocation[0].Y;
							int num21 = num19 * num19 + num20 * num20;
							int num22 = 1;
							do
							{
								num19 = num17 - smartLocation[num22].X;
								num20 = num18 - smartLocation[num22].Y;
								int num23 = num19 * num19 + num20 * num20;
								if (num23 < num21)
								{
									num21 = num23;
									num16 = num22;
								}
							}
							while (++num22 < num3);
						}
						num = smartLocation[num16].X - CurrentView.ScreenPosition.X;
						num2 = smartLocation[num16].Y - CurrentView.ScreenPosition.Y;
					}
					else
					{
						ui.cursorHighlight = 0;
					}
				}
				if (flag)
				{
					if (flag2)
					{
						controlDir.X = direction << 4;
					}
					else if (left.LengthSquared() < 0.25f)
					{
						controlDir.X = vector2.X * 32f;
						controlDir.Y = vector2.Y * -32f;
					}
					else
					{
						controlDir.X = left.X * 80f;
						controlDir.Y = left.Y * -80f;
					}
				}
				else if (right.LengthSquared() < 0.25f)
				{
					controlDir.X = vector.X * 32f;
					controlDir.Y = vector.Y * -32f;
				}
				else
				{
					controlDir.X = right.X * 80f;
					controlDir.Y = right.Y * -80f;
				}
				if (num3 == 0 && ptr->Shoot > 0)
				{
					num += (int)controlDir.X;
					num2 += (int)controlDir.Y;
				}
				ui.MouseX = (short)num;
				ui.MouseY = (short)num2;
			}
		}

		public unsafe void Draw(WorldView drawView, bool IsMenuScr = false, bool IsIcon = false)
		{
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
			SpriteEffects spriteEffects = SpriteEffects.None;
			SpriteEffects spriteEffects2 = SpriteEffects.FlipHorizontally;
			Color newColor;
			Color newColor2;
			Color newColor3;
			Color newColor4;
			Color newColor5;
			Color newColor6;
			Color newColor7;
			Color newColor8;
			Color newColor9;
			Color newColor10;
			Color newColor11;
			Color newColor12;
			Color c;
			if (IsMenuScr)
			{
				newColor = Color.White;
				newColor2 = Color.White;
				newColor3 = Color.White;
				newColor4 = Color.White;
				newColor5 = shirtColor;
				newColor6 = underShirtColor;
				newColor7 = pantsColor;
				newColor8 = shoeColor;
				newColor9 = eyeColor;
				newColor10 = hairColor;
				newColor11 = skinColor;
				newColor12 = skinColor;
				c = skinColor;
			}
			else
			{
				int x = XYWH.X + 10 >> 4;
				int y = (int)(Position.Y + 21f) >> 4;
				int y2 = (int)(Position.Y + 10.5f) >> 4;
				int y3 = (int)(Position.Y + 31.5f) >> 4;
				newColor5 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y, shirtColor));
				newColor6 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y, underShirtColor));
				newColor7 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y, pantsColor));
				newColor8 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y3, shoeColor));
				newColor = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y2));
				newColor2 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y));
				newColor3 = GetImmuneAlpha2(drawView.Lighting.GetColorPlayer(x, y3));
				if (shadow > 0f)
				{
					newColor4 = default(Color);
					newColor9 = default(Color);
					newColor10 = default(Color);
					newColor11 = default(Color);
					newColor12 = default(Color);
					c = default(Color);
				}
				else
				{
					newColor4 = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y2));
					newColor9 = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y2, eyeColor));
					newColor10 = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y2, hairColor));
					newColor11 = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y2, skinColor));
					newColor12 = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y, skinColor));
					c = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(x, y3, skinColor));
				}
			}
			if (buffR != 1f || buffG != 1f || buffB != 1f)
			{
				if (onFire || onFire2)
				{
					newColor4 = GetImmuneAlpha(Color.White);
					newColor9 = GetImmuneAlpha(eyeColor);
					newColor10 = GetImmuneAlpha(hairColor);
					newColor11 = GetImmuneAlpha(skinColor);
					newColor12 = GetImmuneAlpha(skinColor);
					newColor5 = GetImmuneAlpha(shirtColor);
					newColor6 = GetImmuneAlpha(underShirtColor);
					newColor7 = GetImmuneAlpha(pantsColor);
					newColor8 = GetImmuneAlpha(shoeColor);
					newColor = GetImmuneAlpha(Color.White);
					newColor2 = GetImmuneAlpha(Color.White);
					newColor3 = GetImmuneAlpha(Color.White);
				}
				else
				{
					buffColor(ref newColor4, buffR, buffG, buffB);
					buffColor(ref newColor9, buffR, buffG, buffB);
					buffColor(ref newColor10, buffR, buffG, buffB);
					buffColor(ref newColor11, buffR, buffG, buffB);
					buffColor(ref newColor12, buffR, buffG, buffB);
					buffColor(ref newColor5, buffR, buffG, buffB);
					buffColor(ref newColor6, buffR, buffG, buffB);
					buffColor(ref newColor7, buffR, buffG, buffB);
					buffColor(ref newColor8, buffR, buffG, buffB);
					buffColor(ref newColor, buffR, buffG, buffB);
					buffColor(ref newColor2, buffR, buffG, buffB);
					buffColor(ref newColor3, buffR, buffG, buffB);
				}
			}
			if (gravDir == 1)
			{
				if (direction == 1)
				{
					spriteEffects = SpriteEffects.None;
					spriteEffects2 = SpriteEffects.None;
				}
				else
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
					spriteEffects2 = SpriteEffects.FlipHorizontally;
				}
				if (!IsDead)
				{
					legPosition.Y = 0f;
					headPosition.Y = 0f;
					bodyPosition.Y = 0f;
				}
			}
			else
			{
				if (direction == 1)
				{
					spriteEffects = SpriteEffects.FlipVertically;
					spriteEffects2 = SpriteEffects.FlipVertically;
				}
				else
				{
					spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					spriteEffects2 = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
				if (!IsDead)
				{
					legPosition.Y = 6f;
					headPosition.Y = 6f;
					bodyPosition.Y = 6f;
				}
			}
			Vector2 pivot = new Vector2(20f, 42f);
			Vector2 pivot2 = new Vector2(20f, 28f);
			Vector2 pivot3 = new Vector2(20f, 22.4f);
			Vector2 pos = Vector2.Zero;
			if (merman)
			{
				headRotation = velocity.Y * direction * 0.1f;
				if (headRotation < -0.3)
				{
					headRotation = -0.3f;
				}
				if (headRotation > 0.3)
				{
					headRotation = 0.3f;
				}
			}
			else if (!IsDead)
			{
				headRotation = 0f;
			}
			if (!IsIcon)
			{
				if (wings > 0)
				{
					pos = new Vector2(XYWH.X - drawView.ScreenPosition.X + 10 - 9 * direction, XYWH.Y - drawView.ScreenPosition.Y + 21 + (gravDir << 1));
					int num = (int)_sheetSprites.ID.WATER + wings;
					int num2 = SpriteSheet<_sheetSprites>.Source[num].Height >> 2;
					SpriteSheet<_sheetSprites>.DrawRotated(num, ref pos, num2 * wingFrame, num2, newColor2, bodyRotation, spriteEffects);
				}
				if (!invis)
				{
					pos = new Vector2(20 + (int)bodyPosition.X + XYWH.X - drawView.ScreenPosition.X - 20 + 10, 28 + (int)bodyPosition.Y + XYWH.Y - drawView.ScreenPosition.Y + 42 - 56 + 4);
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.SKIN_BODY, ref pos, bodyFrameY, 54, newColor12, bodyRotation, ref pivot2, spriteEffects);
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.SKIN_LEGS, ref pos, legFrameY, 54, c, legRotation, ref pivot2, spriteEffects);
				}
				pos = new Vector2((int)(Position.X - drawView.ScreenPosition.X - 20f + 10f), (int)(Position.Y - drawView.ScreenPosition.Y + 42f - 56f + 4f)) + legPosition + pivot;
				if (legs > 0 && legs < MaxNumArmorLegs)
				{
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARMOR_LEGS_1 - 1 + legs, ref pos, legFrameY, 54, newColor3, legRotation, ref pivot, spriteEffects);
				}
				else if (!invis)
				{
					if (!male)
					{
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_PANTS, ref pos, legFrameY, 54, newColor7, legRotation, ref pivot, spriteEffects);
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_SHOES, ref pos, legFrameY, 54, newColor8, legRotation, ref pivot, spriteEffects);
					}
					else
					{
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_PANTS, ref pos, legFrameY, 54, newColor7, legRotation, ref pivot, spriteEffects);
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_SHOES, ref pos, legFrameY, 54, newColor8, legRotation, ref pivot, spriteEffects);
					}
				}
				pos.X = XYWH.X - drawView.ScreenPosition.X + 10;
				pos.Y = XYWH.Y - drawView.ScreenPosition.Y + 42 - 56 + 4 + 28;
				pos.X += bodyPosition.X;
				pos.Y += bodyPosition.Y;
				if (body > 0 && body < MaxNumArmorBody)
				{
					int id = (male ? (int)_sheetSprites.ID.ARMOR_BODY_1 - 1 : (int)_sheetSprites.ID.EYE_LASER) + body;
					SpriteSheet<_sheetSprites>.DrawRotated(id, ref pos, bodyFrameY, 54, newColor2, bodyRotation, ref pivot2, spriteEffects);
					if (!invis && ((body >= 10 && body <= 16) || body == 20))
					{
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HANDS, ref pos, bodyFrameY, 54, newColor12, bodyRotation, ref pivot2, spriteEffects);
					}
				}
				else if (!invis)
				{
					if (!male)
					{
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_UNDERSHIRT, ref pos, bodyFrameY, 54, newColor6, bodyRotation, ref pivot2, spriteEffects);
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_SHIRT, ref pos, bodyFrameY, 54, newColor5, bodyRotation, ref pivot2, spriteEffects);
					}
					else
					{
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_UNDERSHIRT, ref pos, bodyFrameY, 54, newColor6, bodyRotation, ref pivot2, spriteEffects);
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_SHIRT, ref pos, bodyFrameY, 54, newColor5, bodyRotation, ref pivot2, spriteEffects);
					}
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HANDS, ref pos, bodyFrameY, 54, newColor12, bodyRotation, ref pivot2, spriteEffects);
				}
			}
			pos.X = XYWH.X - drawView.ScreenPosition.X - 20 + 10;
			pos.Y = XYWH.Y - drawView.ScreenPosition.Y + 42 - 56 + 4;
			pos.X += headPosition.X + pivot3.X;
			pos.Y += headPosition.Y + pivot3.Y;
			if (!invis && head != 38)
			{
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HEAD, ref pos, bodyFrameY, 54, newColor11, headRotation, ref pivot3, spriteEffects);
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_EYE_WHITES, ref pos, bodyFrameY, 54, newColor4, headRotation, ref pivot3, spriteEffects);
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_EYES, ref pos, bodyFrameY, 54, newColor9, headRotation, ref pivot3, spriteEffects);
			}
			if (head == 10 || head == 12 || head == 28)
			{
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARMOR_HEAD_1 - 1 + head, ref pos, bodyFrameY, 54, newColor, headRotation, ref pivot3, spriteEffects);
				if (!invis)
				{
					int num3 = bodyFrameY;
					num3 -= 336;
					if (num3 < 0)
					{
						num3 = 0;
					}
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIR_1 + hair, ref pos, num3, 54, newColor10, headRotation, ref pivot3, spriteEffects);
				}
			}
			else if (((head >= 14 && head <= 16) || head == 18 || head == 21 || (head >= 24 && head <= 26) || head == 40 || head == 44) && !invis)
			{
				int num4 = bodyFrameY;
				num4 -= 336;
				if (num4 < 0)
				{
					num4 = 0;
				}
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIRALT_1 + hair, ref pos, num4, 54, newColor10, headRotation, ref pivot3, spriteEffects);
			}
			if (head == 23)
			{
				int num5 = bodyFrameY;
				num5 -= 336;
				if (num5 < 0)
				{
					num5 = 0;
				}
				if (!invis)
				{
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIR_1 + hair, ref pos, num5, 54, newColor10, headRotation, ref pivot3, spriteEffects);
				}
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARMOR_HEAD_1 - 1 + head, ref pos, num5, 54, newColor, headRotation, ref pivot3, spriteEffects);
			}
			else if (head == 14)
			{
				int num6 = bodyFrameY;
				int num7 = 56;
				int num8 = 0;
				if (num6 == num7 * 6)
				{
					num7 -= 2;
				}
				else if (num6 == num7 * 7)
				{
					num8 = -2;
				}
				else if (num6 == num7 << 3)
				{
					num8 = -2;
				}
				else if (num6 == num7 * 9)
				{
					num8 = -2;
				}
				else if (num6 == num7 * 10)
				{
					num8 = -2;
				}
				else if (num6 == num7 * 13)
				{
					num7 -= 2;
				}
				else if (num6 == num7 * 14)
				{
					num8 = -2;
				}
				else if (num6 == num7 * 15)
				{
					num8 = -2;
				}
				else if (num6 == num7 << 4)
				{
					num8 = -2;
				}
				num6 += num8;
				pos.Y += num8;
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARMOR_HEAD_14, ref pos, num6, num7, newColor, headRotation, ref pivot3, spriteEffects);
			}
			else if (head > 0 && head < MaxNumArmorHead && head != 28)
			{
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARMOR_HEAD_1 - 1 + head, ref pos, bodyFrameY, 54, newColor, headRotation, ref pivot3, spriteEffects);
			}
			else if (!invis)
			{
				int num9 = bodyFrameY;
				num9 -= 336;
				if (num9 < 0)
				{
					num9 = 0;
				}
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIR_1 + hair, ref pos, num9, 54, newColor10, headRotation, ref pivot3, spriteEffects);
			}
			if (IsIcon)
			{
				return;
			}
			if (!IsMenuScr)
			{
				if (heldProj >= 0)
				{
					Main.ProjectileSet[heldProj].Draw(drawView);
				}
				fixed (Item* ptr = &Inventory[SelectedItem])
				{
					int type = ptr->Type;
					if (type > 0 && (itemAnimation > 0 || ptr->HoldStyle > 0) && !IsDead && !ptr->NoUseGraphic && (!IsWet || !ptr->CantTouchLiquid))
					{
						int num10 = (int)_sheetSprites.ID.ITEM_1 - 1 + type;
						int num11 = SpriteSheet<_sheetSprites>.Source[num10].Width;
						Color color = drawView.Lighting.GetColor((int)(Position.X + 10f) >> 4, (int)(Position.Y + 21f) >> 4);
						Color alpha = ptr->GetAlpha(color);
						pos.X = itemLocation.X - drawView.ScreenPosition.X;
						pos.Y = itemLocation.Y - drawView.ScreenPosition.Y;
						Vector2 pivot4 = default(Vector2);
						if (ptr->UseStyle == 5)
						{
							int num12 = 10;
							Vector2 centerPivot = SpriteSheet<_sheetSprites>.GetCenterPivot(num10);
							pivot4.X = ((direction == -1) ? (num12 + num11) : (-num12));
							pivot4.Y = centerPivot.Y;
							switch ((Item.ID)type)
							{
							case Item.ID.FLINTLOCK_PISTOL:
								centerPivot.Y += 2 * gravDir;
								break;
							case Item.ID.MUSKET:
								num12 = -5;
								break;
							case Item.ID.MINISHARK:
								num12 = -5;
								centerPivot.Y -= 2 * gravDir;
								break;
							case Item.ID.SHOTGUN:
								num12 = -2;
								centerPivot.Y += gravDir;
								break;
							case Item.ID.MEGASHARK:
								num12 = -7;
								centerPivot.Y -= 2 * gravDir;
								break;
							case Item.ID.FLAMETHROWER:
								num12 = 0;
								centerPivot.Y -= 2 * gravDir;
								break;
							case Item.ID.MAGICAL_HARP:
							case Item.ID.HARP:
								num12 = -2;
								break;
							case Item.ID.CLOCKWORK_ASSAULT_RIFLE:
								num12 = 0;
								centerPivot.Y -= 2 * gravDir;
								break;
							case Item.ID.LASER_RIFLE:
								num12 = 0;
								centerPivot.Y += 3 * gravDir;
								break;
							case Item.ID.COBALT_REPEATER:
							case Item.ID.MYTHRIL_REPEATER:
							case Item.ID.ADAMANTITE_REPEATER:
							case Item.ID.HALLOWED_REPEATER:
								num12 = -2;
								centerPivot.Y -= 2 * gravDir;
								break;
							case Item.ID.STAR_CANNON:
								num12 = -5;
								centerPivot.Y += 4 * gravDir;
								break;
							case Item.ID.BOTTLED_WATER:
								num12 = 4;
								centerPivot.Y += 4 * gravDir;
								break;
							case Item.ID.SPACE_GUN:
								num12 = 4;
								centerPivot.Y += 2 * gravDir;
								break;
							case Item.ID.AQUA_SCEPTER:
								num12 = 6;
								centerPivot.Y += 2 * gravDir;
								break;
							case Item.ID.HARPOON:
								num12 = -8;
								break;
							case Item.ID.HANDGUN:
							case Item.ID.PHOENIX_BLASTER:
								num12 = 2;
								centerPivot.Y += 4 * gravDir;
								break;
							case Item.ID.WATER_BOLT:
							case Item.ID.DEMON_SCYTHE:
								num12 = 4;
								centerPivot.Y += 4 * gravDir;
								break;
							case Item.ID.SANDGUN:
								num12 = 0;
								centerPivot.Y += 2 * gravDir;
								break;
							case Item.ID.BLOWPIPE:
								num12 = 6;
								centerPivot.Y -= 6 * gravDir;
								break;
							}
							pos.X += centerPivot.X;
							pos.Y += centerPivot.Y;
							SpriteSheet<_sheetSprites>.Draw(num10, ref pos, alpha, itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							if (ptr->Colour.PackedValue != 0)
							{
								SpriteSheet<_sheetSprites>.Draw(num10, ref pos, ptr->GetColor(color), itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							}
						}
						else if (gravDir == -1)
						{
							pivot4.X = (num11 >> 1) - (num11 >> 1) * direction;
							pivot4.Y = 0f;
							SpriteSheet<_sheetSprites>.Draw(num10, ref pos, alpha, itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							if (ptr->Colour.PackedValue != 0)
							{
								SpriteSheet<_sheetSprites>.Draw(num10, ref pos, ptr->GetColor(color), itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							}
						}
						else
						{
							if (type == (int)Item.ID.BELL || type == (int)Item.ID.FAIRY_BELL)
							{
								spriteEffects2 = ((gravDir == 1) ? ((direction != 1) ? (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically) : SpriteEffects.FlipVertically) : ((direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
							}
							pivot4 = new Vector2((num11 >> 1) - (num11 >> 1) * direction, SpriteSheet<_sheetSprites>.Source[num10].Height);
							SpriteSheet<_sheetSprites>.Draw(num10, ref pos, alpha, itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							if (ptr->Colour.PackedValue != 0)
							{
								SpriteSheet<_sheetSprites>.Draw(num10, ref pos, ptr->GetColor(color), itemRotation, ref pivot4, ptr->Scale, spriteEffects2);
							}
						}
					}
				}
			}
			pos.X = XYWH.X - drawView.ScreenPosition.X + (width / 2);
			pos.Y = XYWH.Y - drawView.ScreenPosition.Y + height - 28 + 4;
			pos.X += bodyPosition.X;
			pos.Y += bodyPosition.Y;
			if (body > 0 && body < MaxNumArmorBody)
			{
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARM_BONE_2 + body, ref pos, bodyFrameY, 54, newColor2, bodyRotation, ref pivot2, spriteEffects);
				if (!invis && ((body >= 10 && body <= 16) || body == 20))
				{
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIRALT_36 + 1, ref pos, bodyFrameY, 54, newColor12, bodyRotation, ref pivot2, spriteEffects);
				}
			}
			else if (!invis)
			{
				if (!male)
				{
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_UNDERSHIRT2, ref pos, bodyFrameY, 54, newColor6, bodyRotation, ref pivot2, spriteEffects);
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.FEMALE_SHIRT2, ref pos, bodyFrameY, 54, newColor5, bodyRotation, ref pivot2, spriteEffects);
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_UNDERSHIRT2, ref pos, bodyFrameY, 54, newColor6, bodyRotation, ref pivot2, spriteEffects);
				}
				SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.PLAYER_HAIRALT_36 + 1, ref pos, bodyFrameY, 54, newColor12, bodyRotation, ref pivot2, spriteEffects);
			}
		}

		public void DrawGhost(WorldView drawView)
		{
			XYWH.X = (int)Position.X;
			XYWH.Y = (int)Position.Y;
			SpriteEffects e = ((direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			int num = (UI.MouseTextBrightness >> 1) + 100;
			Color c = GetImmuneAlpha(drawView.Lighting.GetColorPlayer(XYWH.X + (width / 2) >> 4, XYWH.Y + (height / 2) >> 4, new Color(num, num, num, num)));
			int num2 = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.GHOST].Height >> 2;
			Vector2 pos = new Vector2(XYWH.X - drawView.ScreenPosition.X, XYWH.Y - drawView.ScreenPosition.Y);
			SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.GHOST, ref pos, num2 * ((ghostFrameCounter >> 3) & 3), num2, c, e);
		}

		public Item armorSwap(ref Item newItem)
		{
			int num = 0;
			for (int i = 0; i < armor.Length; i++)
			{
				if (newItem.NetID == armor[i].NetID)
				{
					num = i;
				}
			}
			Item item = newItem;
			if (newItem.HeadSlot != -1)
			{
				int num2 = (newItem.IsVanity ? (NumArmorSlots + NumEquipSlots + 0) : 0);
				item = armor[num2];
				ref Item reference = ref armor[num2];
				reference = newItem;
			}
			else if (newItem.BodySlot != -1)
			{
				int num3 = (newItem.IsVanity ? (NumArmorSlots + NumEquipSlots + 1) : 1);
				item = armor[num3];
				ref Item reference2 = ref armor[num3];
				reference2 = newItem;
			}
			else if (newItem.LegSlot != -1)
			{
				int num4 = (newItem.IsVanity ? (NumArmorSlots + NumEquipSlots + 2) : 2);
				item = armor[num4];
				ref Item reference3 = ref armor[num4];
				reference3 = newItem;
			}
			else
			{
				for (int j = NumArmorSlots; j < (NumArmorSlots + NumEquipSlots); j++)
				{
					if (armor[j].Type == 0)
					{
						num = j;
						break;
					}
				}
				for (int k = 0; k < armor.Length; k++)
				{
					if (newItem.NetID == armor[k].NetID)
					{
						num = k;
					}
				}
				if (num >= (NumArmorSlots + NumEquipSlots))
				{
					num = NumArmorSlots;
				}
				else if (num < NumArmorSlots)
				{
					num = (NumArmorSlots + NumEquipSlots - 1);
				}
				item = armor[num];
				ref Item reference4 = ref armor[num];
				reference4 = newItem;
			}
			Main.PlaySound(7);
			return item;
		}

		public int CountInventory(int netID)
		{
			int num = 0;
			for (int num2 = MaxNumInventory - 1; num2 >= 0; num2--)
			{
				if (Inventory[num2].NetID == netID)
				{
					num += Inventory[num2].Stack;
				}
			}
			return num;
		}

		public int CountEquipment(int netID)
		{
			int num = 0;
			for (int num2 = MaxNumArmor - 1; num2 >= 0; num2--)
			{
				if (armor[num2].NetID == netID)
				{
#if VERSION_INITIAL && !IS_PATCHED
					num += Inventory[num2].Stack;
#else
					num += armor[num2].Stack;
#endif
				}
			}
			return num;
		}

		public int CountPossession(int netID)
		{
			return CountInventory(netID) + CountEquipment(netID);
		}

		public bool IsNearCraftingStation(Recipe r)
		{
			for (int num = r.NumRequiredTiles - 1; num >= 0; num--)
			{
				if (!adjTile[r.RequiredTile[num]].i)
				{
					return false;
				}
			}
			if (!adjWater)
			{
				return !r.NeedsWater;
			}
			return true;
		}

		public bool CanCraftRecipe(Recipe r)
		{
			if (Main.TutorialState < Tutorial.CRAFT_TORCH)
			{
				return false;
			}
			if (Main.TutorialState == Tutorial.CRAFT_TORCH && r.CraftedItem.Type != 8)
			{
				return false;
			}
			for (int num = r.NumRequiredItems - 1; num >= 0; num--)
			{
				int num2 = r.RequiredItem[num].Stack;
				for (int num3 = MaxNumInventory - 1; num3 >= 0; num3--)
				{
					if (Inventory[num3].NetID == r.RequiredItem[num].NetID)
					{
						num2 -= Inventory[num3].Stack;
						if (num2 <= 0)
						{
							break;
						}
					}
				}
				if (num2 > 0)
				{
					return false;
				}
			}
			return IsNearCraftingStation(r);
		}

		public bool DiscoveredRecipe(Recipe r)
		{
			for (int num = r.NumRequiredItems - 1; num >= 0; num--)
			{
				if (!itemsFound.Get(r.RequiredItem[num].Type))
				{
					return false;
				}
			}
			for (int num2 = r.NumRequiredTiles - 1; num2 >= 0; num2--)
			{
				if (!craftingStationsFound.Get(r.RequiredTile[num2]))
				{
					return false;
				}
			}

			return true;
		}

		public void UpdateRecipes()
		{
			for (int num = Recipe.MaxNumRecipes - 1; num >= 0; num--)
			{
				if (!RecipesFound.Get(num) && DiscoveredRecipe(Main.ActiveRecipe[num]))
				{
					RecipesFound.Set(num, true);
					recipesNew.Set(num, true);
				}
			}
		}

		private void ApplyPetBuff(int itemType)
		{
			int i;
			if (pet >= 0)
			{
				int num = Projectile.petProj[pet];
				for (i = 0; i < Projectile.MaxNumProjs; i++)
				{
					if (Main.ProjectileSet[i].type == num && Main.ProjectileSet[i].active != 0 && Main.ProjectileSet[i].owner == WhoAmI)
					{
						Main.ProjectileSet[i].Kill();
						break;
					}
				}
				if (itemType == Projectile.petItem[pet])
				{
					DelBuff(Buff.ID.PET);
					return;
				}
			}
			i = Projectile.petItem.Length - 1;
			while (i >= 0 && itemType != Projectile.petItem[i])
			{
				i--;
			}
			pet = (sbyte)i;
			ui.petSpawnMask |= (byte)(1 << i);
			if (ui.petSpawnMask == 63)
			{
				ui.SetTriggerState(Trigger.SpawnedAllPets);
			}
			AddBuff((int)Buff.ID.PET, 3600);
		}

		private void SpawnPet()
		{
			int num = Projectile.petProj[pet];
			for (int i = 0; i < Projectile.MaxNumProjs; i++)
			{
				if (Main.ProjectileSet[i].type == num && Main.ProjectileSet[i].active != 0 && Main.ProjectileSet[i].owner == WhoAmI)
				{
					return;
				}
			}
			Projectile.NewProjectile(Position.X + (width / 2), Position.Y + (height / 2), 0f, 0f, num, 0, 0f, WhoAmI);

		}

		public void AchievementTrigger(Trigger trigger)
		{
			if (isLocal())
			{
				ui.SetTriggerState(trigger);
				return;
			}
			NetMessage.CreateMessage2(64, WhoAmI, (int)trigger);
			NetMessage.SendMessage(client);
		}

		public void IncreaseStatistic(StatisticEntry entry)
		{
			if (entry != StatisticEntry.Unknown)
			{
				if (isLocal())
				{
					ui.Statistics.IncreaseStat(entry);
				}
				else if (client != null)
				{
					NetMessage.CreateMessage2(65, WhoAmI, (int)entry);
					NetMessage.SendMessage(client);
				}
			}
		}

		public void SunMoonTransition(bool WasBloodMoon)
		{
			totalSunMoonTransitions++;
			if (Main.GameTime.DayTime && totalSunMoonTransitions >= 2)
			{
				AchievementTrigger(Trigger.Sunrise);
				if (WasBloodMoon)
				{
					AchievementTrigger(Trigger.SunriseAfterBloodMoon);
				}
			}
		}

		private void FoundCraftingStation(int type)
		{
			if (ui.TriggerCheckEnabled(Trigger.UsedAllCraftingStations))
			{
				craftingStationsFound.Set(type, true);
				if (craftingStationsFound.Get(133) && craftingStationsFound.Get(134) && craftingStationsFound.Get(101) && craftingStationsFound.Get(114) && craftingStationsFound.Get(106) && craftingStationsFound.Get(96) && craftingStationsFound.Get(94) && craftingStationsFound.Get(86) && craftingStationsFound.Get(26) && craftingStationsFound.Get(13) && craftingStationsFound.Get(15) && craftingStationsFound.Get(18))
				{
					ui.SetTriggerState(Trigger.UsedAllCraftingStations);
				}
			}
		}

		private void IncreaseSteps()
		{
			if (ui != null)
			{
				if (++ui.totalSteps == 42000)
				{
					ui.SetTriggerState(Trigger.Walked42KM);
				}
				StatisticEntry entry = StatisticEntry.GroundTravel;
				if (IsWet)
				{
					entry = (lavaWet ? StatisticEntry.LavaTravel : StatisticEntry.WaterTravel);
				}
				ui.Statistics.IncreaseStat(entry);
			}
		}

		private void IncreaseAirTime()
		{
			if (ui == null)
			{
				return;
			}
			ui.currentAirTime++;
			if (ui.currentAirTime >= (int)Achievement.AirTimeMinimum)
			{
				if (ui.currentAirTime == (int)Achievement.AirTimeMinimum) // BUG: This leads to the scenario where you can reach the airtime minimum numerous times, and the total will still increase.
				{
					ui.totalAirTime += 60; // Literally just this += is the issue; Using = instead would mean each time the minimum is reached, it actually counts from there, making the genuine count for the airtime goal.
				}
				else
				{
					ui.totalAirTime++;
				}
				if (ui.totalAirTime >= (int)Achievement.AirTimeGoal)
				{
					ui.SetTriggerState(Trigger.InTheSky); // Probably should also feature a reset for the airtime here.
				}
				ui.airTravel += velocity.Length();
				if (ui.airTravel > 20f)
				{
					ui.airTravel -= 20f;
					ui.Statistics.IncreaseStat(StatisticEntry.AirTravel);
				}
			}
		}

		private void ResetAirTime()
		{
			if (ui != null)
			{
				ui.currentAirTime = 0;
			}
		}

		public static void buffColor(ref Color newColor, double R, double G, double B)
		{
			newColor.R = (byte)(newColor.R * R);
			newColor.G = (byte)(newColor.G * G);
			newColor.B = (byte)(newColor.B * B);
		}

		public void updateScreenPosition()
		{
#if VERSION_103 || VERSION_FINAL // Behold, the 1.2 screen positioning code, created for the purpose of supporting scopes and binoculars.
			// This also for some reason gives the player an 'extended camera' based on their movement velocity; its small, but the camera does shift depending on direction.
			// Also, despite the inference that this code is complete, it is not; it is close to completion but there exists an inaccuracy with the camera moving up slightly upon spawning in. 
			float HorizontalBase, VerticalBase, AdjustedXMotion, HorizontalIncrease, VerticalIncrease, AdjustedView, InvertedView;
			Vector2 Motion, ReservedPosition;

			HorizontalBase = -64f;
			VerticalBase = HorizontalBase;
			Motion.X = velocity.X;
			AdjustedXMotion = Motion.X * 16f + (float)(direction << 3);

			if (HorizontalBase <= AdjustedXMotion)
			{
				HorizontalBase = AdjustedXMotion;
				if (64f < AdjustedXMotion)
				{
					HorizontalBase = 64f;
				}
			}

			Motion.Y = velocity.Y;
			Motion.Y *= 12f;
			if (VerticalBase <= Motion.Y)
			{
				VerticalBase = Motion.Y;
				if (64f < Motion.Y)
				{
					VerticalBase = 64f;
				}
			}

			HorizontalBase = (aabb.Width / 2) - (view.viewWidth >> 1) + aabb.X + HorizontalBase;
			VerticalBase = aabb.Y + (aabb.Height / 2) - (Main.ResolutionHeight / 2) + VerticalBase;

			ReservedPosition.X = HorizontalBase;
			ReservedPosition.Y = VerticalBase;

			if (scope || inventory[selectedItem].type == 1254 || inventory[selectedItem].type == 1299) // Check if they have a scope equipped or are using a rifle or binoculars
			{
				AdjustedXMotion = ui.gpState.ThumbSticks.Right.X;
				Vector2 RightStick = ui.gpState.ThumbSticks.Right;
				if ((1f / 64f) < RightStick.LengthSquared())
				{
					if (inventory[selectedItem].type == 1254 || inventory[selectedItem].type == 1299)
					{
						if (!scope || inventory[selectedItem].type == 1299)
						{
							HorizontalIncrease = 4f;
							VerticalIncrease = 3f;
						}
						else
						{
							HorizontalIncrease = 6f;
							VerticalIncrease = 4f;
						}
					}
					else
					{
						HorizontalIncrease = 3f;
						VerticalIncrease = 2.5f;
					}
					AdjustedView = HorizontalIncrease * 60f;
					AdjustedXMotion = AdjustedXMotion * HorizontalIncrease * 1.334f + (int)view.viewType;
					view.viewType = (WorldView.Type)AdjustedXMotion;
					InvertedView = -AdjustedView;
					view.PlayerSafeAreaL = (int)-(VerticalIncrease * ui.gpState.ThumbSticks.Right.Y * 1.334f - view.PlayerSafeAreaL);

					if (InvertedView <= AdjustedXMotion)
					{
						InvertedView = AdjustedView;
						if (AdjustedXMotion < AdjustedView)
						{
							InvertedView = AdjustedXMotion;
						}
					}

					HorizontalIncrease *= 33.75f;
					view.viewType = (WorldView.Type)InvertedView;
					AdjustedXMotion = view.PlayerSafeAreaL;
					if (-HorizontalIncrease <= AdjustedXMotion)
					{
						if (AdjustedXMotion < HorizontalIncrease)
						{
							HorizontalIncrease = AdjustedXMotion;
						}
						view.PlayerSafeAreaL = (int)HorizontalIncrease;
					}
					else
					{
						view.PlayerSafeAreaL = (int)-HorizontalIncrease;
					}
					goto SkipViewCheck;
				}
			}

			float ViewArea = (int)view.viewType * 0.95f;
			view.viewType = (WorldView.Type)ViewArea;
			view.PlayerSafeAreaL = (int)(view.PlayerSafeAreaL * 0.95f);
			view.PlayerSafeAreaT = (int)(view.PlayerSafeAreaT * 0.95f); // For some reason, it is very hit-or-miss whether the Top safe area is subjected to the same checks, so I'm just going to keep it here just in case.

			if (ViewArea < 0f)
			{
				ViewArea = -ViewArea;
			}
			if (ViewArea < 0.1f)
			{
				view.viewType = 0;
			}
			ViewArea = view.PlayerSafeAreaL;
			if (ViewArea < 0f)
			{
				ViewArea = -ViewArea;
			}
			if (ViewArea < 0.1f)
			{
				view.PlayerSafeAreaL = 0;
			}
			ViewArea = view.PlayerSafeAreaT;
			if (ViewArea < 0f)
			{
				ViewArea = -ViewArea;
			}
			if (ViewArea < 0.1f)
			{
				view.PlayerSafeAreaT = 0;
			}
		SkipViewCheck:
			float NewX, NewY;
			NewX = (int)view.viewType + HorizontalBase - view.cameraX;
			NewY = view.PlayerSafeAreaL + view.PlayerSafeAreaT + VerticalBase - view.cameraY;
			HorizontalBase = NewY * NewY + NewX * NewX;
			if (64f <= HorizontalBase)
			{
				if (ui.menuType != MenuType.PAUSE) // This check is not present in the decompiled 1.2 screen position function, but in a function akin to it; simplicity and reasonability will see it put here.
				{
					if (HorizontalBase <= 65536f)
					{
						view.cameraX = (float)(NewX * 0.07421875 + view.cameraX);
						view.cameraY = (float)(NewY * 0.07421875 + view.cameraY);
					}
					else
					{
						view.cameraX = ReservedPosition.X;
						view.cameraY = ReservedPosition.Y;
					}
				}
				view.screenPosition.X = (int)view.cameraX;
				view.screenPosition.Y = (int)view.cameraY;
			}
#else
			CurrentView.ScreenPosition.X = XYWH.X + (XYWH.Width / 2) - (CurrentView.ViewWidth >> 1);
			CurrentView.ScreenPosition.Y = XYWH.Y + (XYWH.Height / 2) - (Main.ResolutionHeight / 2);
#endif
		}

		public bool isLocal()
		{
			return CurrentView != null;
		}

		public void DrawInfo(WorldView view)
		{
			int num = XYWH.X + (width / 2) - view.ScreenPosition.X;
			int num2 = XYWH.Y + height - view.ScreenPosition.Y;
#if VERSION_INITIAL
			int num3 = (int)UI.DrawStringCT(UI.BoldSmallFont, Name, num, num2, Main.TeamColors[team]);
#else
			int num3 = (int)UI.DrawStringCT(UI.BoldSmallFont, CharacterName, num, num2, Main.TeamColors[team]);
#endif
			int num4 = statLife - healthBarLife;
			if (num4 != 0)
			{
				if (Math.Abs(num4) > 1)
				{
					healthBarLife += (short)(num4 >> 2);
				}
				else
				{
					healthBarLife = statLife;
				}
			}
			Rectangle rect = default;
			rect.X = num - 22;
			rect.Y = num2 + num3 - 2;
			rect.Height = 10;
			rect.Width = 52;
			Color wINDOW_OUTLINE = UI.WINDOW_OUTLINE;
			Main.DrawRect(rect, wINDOW_OUTLINE, ToCenter: false);
			rect.X += 2;
			rect.Y += 2;
			rect.Width = healthBarLife * 48 / StatLifeMax;
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
