using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Terraria.Achievements;
using Terraria.Leaderboards;

namespace Terraria
{
	public sealed class UI
	{
		public enum InventorySection : byte
		{
			CRAFTING,
			ITEMS,
			CHEST,
			EQUIP,
			HOUSING,
			NUM_SECTIONS
		}

		public enum CraftingSection : byte
		{
			RECIPES,
			INGREDIENTS
		}

		private enum StackType
		{
			NONE,
			INGREDIENT,
			INVENTORY,
			HOTBAR
		}

		//private const bool TEST_WATCH = false; // Debug features; removed in final version, re-added in the project through the added settings.

		//private const bool TEST_DEPTH_METER = false;

		//private const bool TEST_COMPASS = false;

		private const ulong MARKETPLACE_OFFER_ID = 6359384554213474305uL; // Used in original code since it would open up the X360 marketplace. Unused here.

#if USE_ORIGINAL_CODE
		public const int FULLSCREEN_SAFE_AREA_OFFSET_X = 48;

		public const int FULLSCREEN_SAFE_AREA_OFFSET_Y = 27;
#else
		public static int FULLSCREEN_SAFE_AREA_OFFSET_X = Main.ResolutionWidth / 20;

		public static int FULLSCREEN_SAFE_AREA_OFFSET_Y = Main.ResolutionHeight / 20;
#endif

#if USE_ORIGINAL_CODE
		public const int UsableWidth = 864;

		public const int USABLE_HEIGHT = 454;
#else
		public static int UsableWidth = (int)(864 * Main.ScreenMultiplier);

		public static int USABLE_HEIGHT = (int)(454 * Main.ScreenMultiplier);
#endif

		public const int CONTROLS_HUD_X = 0;

		public const int UI_DELAY = 12;

		public const int MOUSE_DELAY = 4;

		public const float GpDeadZone = 0.125f;

		public const float SquaredDeadZone = 0.015625f;

		public const float WORLD_FADE_START = -0.25f;

		private const float WORLD_FADE_SPEED = 71f / (678f * (float)Math.PI); // Complex way of saying 1/30...

		private const float UI_FADE_SPEED = WORLD_FADE_SPEED; // Unused I think

		private const int MAX_ITEMS = 14;

		private const int MAX_DEPTH = 16;

#if VERSION_INITIAL
		private const int MAX_LOAD_PLAYERS = 5;
#else
		public const int MAX_LOAD_PLAYERS = 5;
#endif

		private const int MAX_LOAD_WORLDS = 5; // Where is this used? Max worlds is 8 on PS3/X360.

		public const int mcColorR = 125;

		public const int mcColorG = 125;

		public const int mcColorB = 255;

		public const int hcColorR = 200;

		public const int hcColorG = 125;

		public const int hcColorB = 255;

		public const float FONT_STACK_EXTRA_SCALE = 0.1f;

		public const Buttons BTN_JUMP = Buttons.A;

		public const Buttons BTN_INTERACT = Buttons.B;

		public const Buttons BTN_USE = Buttons.RightTrigger;

		public const Buttons BTN_DROP = Buttons.X;

		public const Buttons BTN_RESPAWN = Buttons.A;

		public const Buttons BTN_CURSOR_MODE = Buttons.RightStick;

		public const Buttons BTN_PREV_ITEM = Buttons.LeftShoulder;

		public const Buttons BTN_NEXT_ITEM = Buttons.RightShoulder;

		public const Buttons BTN_INVENTORY_SELL_OR_TRASH = Buttons.X;

		public const Buttons BTN_INVENTORY_SELECT = Buttons.A;

		public const Buttons BTN_INVENTORY_ACTION = Buttons.RightTrigger;

		public const Buttons BTN_INVENTORY_DROP = Buttons.X;

		public const Buttons BTN_INVENTORY_HOUSING = Buttons.LeftTrigger;

		public const Buttons BTN_INVENTORY_OPEN = Buttons.Y;

		public const Buttons BTN_INVENTORY_CLOSE = Buttons.B;

		public const Buttons BTN_NPC_CHAT_SELECT = Buttons.A;

		public const Buttons BTN_NPC_CHAT_CLOSE = Buttons.B;

		public const float LEFT_STICK_VERTICAL_THRESHOLD = 0.5f;

		public const int HOTBAR_ITEMNAME_DISPLAYTIME = 210;

		public const int QUICK_ACCESS_DISPLAYTIME = 120;

		private const int CRAFTING_INGREDIENT_COLS = 4;

		private const int CRAFTING_INGREDIENT_ROWS = 3;

		private const float CRAFTING_DEFAULT_SCROLL_MUL = 0.8125f;

		private const float CRAFTING_MIN_SCROLL_MUL = 0.25f;

		private const float CRAFTING_SCROLL_MUL_DECREMENT = 0.075f;

		private const int cooldownLen = 180;

		private const int MENU_TITLE_W = 1;

		private const int MENU_TITLE_H = 7;

		private const int MENU_PAUSE_W = 1;

		private const int MENU_PAUSE_H = 7;

		private const int MENU_SELECT_W = 1;

		private const int MENU_SELECT_H = 6;

		private const int MENU_CONFIRM_DELETE_W = 1;

		private const int MENU_CONFIRM_DELETE_H = 2;

		private const int MENU_WORLD_SIZE_W = 1;

		private const int MENU_WORLD_SIZE_H = 3;

		private const int MENU_OPTIONS_W = 1;

		private const int MENU_OPTIONS_H = 4;

		private const int MENU_SETTINGS_W = 1;

		private const int MENU_SETTINGS_H = 3;

#if USE_ORIGINAL_CODE
		private const int INVENTORY_W = 864;

		private const int INVENTORY_H = 446;

		private const int INVENTORY_CLIENT_Y_OFFSET = 80;

		private const int INVENTORY_CLIENT_H = 366;

		private const int TOOLTIP_W = 322;
#else
		private static readonly int INVENTORY_W = UsableWidth;

		private static int INVENTORY_H = USABLE_HEIGHT - 8;

		private static readonly int INVENTORY_CLIENT_Y_OFFSET = (int)(80 * Main.ScreenMultiplier);

		private static readonly int INVENTORY_CLIENT_H = (int)(366 * Main.ScreenMultiplier);

		private static readonly int TOOLTIP_W = (int)(322 * Main.ScreenMultiplier);
#endif

		private const float INVENTORY_FADE = 0.5f;

		public static Color DISABLED_COLOR = new Color(16, 16, 16, 128);

		public static Color WINDOW_OUTLINE = new Color(12, 24, 24, 255);

		public static Color DefaultDialogColor = new Color(42, 43, 101, 192);

		private static int FONT_STACK_EXTRA_OFFSET = -5;

		public static UI MainUI;

		public static UI CurrentUI;

		public static int NumActiveViews;

		public static WorldView[] activeView = new WorldView[4];

		public WorldView CurrentView;

		public LocalNetworkGamer localGamer;

		public SignedInGamer SignedInGamer;

		public PlayerIndex controller;

		public Player ActivePlayer;

		public byte MyPlayer = Player.MaxNumPlayers;

		private byte privateSlots;

		public NetPlayer netPlayer = new NetPlayer();

		public bool wasRemovedFromSessionWithoutOurConsent;

		public MenuType CurMenuType;

		public bool isStopping;

		public float worldFade;

		public float worldFadeTarget = 1f;

		public float uiFade = 1f;

		public float uiFadeTarget = 1f;

		public short oldMouseX;

		public short oldMouseY;

		public short MouseX;

		public short MouseY;

		public bool UsingSmartCursor = true;

		public bool alternateGrappleControls;

		public Buttons BTN_JUMP2 = Buttons.LeftStick;

		public Buttons BTN_GRAPPLE = Buttons.LeftTrigger;

#if VERSION_INITIAL
		private sbyte quickAccessUp = -1;

		private sbyte quickAccessDown = -1;

		private sbyte quickAccessLeft = -1;

		private sbyte quickAccessRight = -1;
#else
		public sbyte quickAccessUp = -1; // There may be a function that gets these values and keeps the variables private; Conflicting evidence found.

		public sbyte quickAccessDown = -1;

		public sbyte quickAccessLeft = -1;

		public sbyte quickAccessRight = -1;
#endif

		public GamePadState gpPrevState;

		public GamePadState PadState;

		public StorageDeviceManager playerStorage;

		private List<string> transferredPlayerStorage = new List<string>(3);

		public sbyte numLoadPlayers;

		public string playerPathName;

		public Player[] loadPlayer = new Player[MAX_LOAD_PLAYERS];

		public string[] loadPlayerPath = new string[MAX_LOAD_PLAYERS];

		private float logoRotation;

		private float logoRotationDirection = 1f;

		private float logoRotationSpeed = 1f;

		private float logoScale = 1f;

		private float logoScaleDirection = 1f;

		private float logoScaleSpeed = 1f;

		private short LogoA = 255;

		private short LogoB;

		public string statusText;

		private static string errorDescription;

		private static string errorCaption;

		private static CompiledText errorCompiledText;

		private static CompiledText saveIconMessage = null;

		private static int saveIconMessageTime = 0;

		public float MusicVolume = 0.75f;

		public float SoundVolume = 1f;

		public bool autoSave;

		public bool ShowItemText = true;

		public bool IsOnline;

		public bool IsInviteOnly;

		public bool SettingsDirty;

		private Stopwatch saveTime = new Stopwatch();

		private Color selColor = Color.White;

		private bool[] noFocus = new bool[MAX_ITEMS];

		private bool[] blockFocus = new bool[MAX_ITEMS];

		private short[] menuY = new short[MAX_ITEMS];

		private byte[] menuHC = new byte[MAX_ITEMS];

		private float[] menuScale = new float[MAX_ITEMS];

		private float[] menuItemScale = new float[MAX_ITEMS];

		private sbyte focusMenu = -1;

		private sbyte selectedMenu = -1;

		public sbyte selectedPlayer;

		public int menuDepth;

		public MenuMode CurMenuMode = MenuMode.WELCOME;

		public MenuMode[] prevMenuMode = new MenuMode[16];

		public float Progress;

		private float progressTotal;

		private float numProgressStepsInv;

		private short uiX;

		private short uiY;

		private sbyte uiWidth;

		private sbyte uiHeight;

		private sbyte uiDelayValue;

		public sbyte uiDelay;

		public Location[] uiCoords;

		private Location[] uiPos = new Location[(int)MenuMode.NUM_MENUS];

		public int cursorHighlight;

		public Terraria.CreateCharacter.UI CreateCharacterGUI;

		private Terraria.SoundUI.UI soundUI;

		private Terraria.HowToPlay.UI howtoUI;

#if !USE_ORIGINAL_CODE
		private Terraria.Hardmode.UI hardmodeUpsell;

		private AchievementsUI Achievements;
#endif

		private TextSequenceBlock tips;

		private LeaderboardsUI Leaderboards;

		public int hotbarItemNameTime = HOTBAR_ITEMNAME_DISPLAYTIME;

		public int quickAccessDisplayTime;

		public float[] hotbarScale = new float[10]
		{
			1f,
			0.75f,
			0.75f,
			0.75f,
			0.75f,
			0.75f,
			0.75f,
			0.75f,
			0.75f,
			0.75f
		};

		private static float inventoryScale;

		public float[] inventoryMenuSectionScale = new float[5]
		{
			0.75f,
			0.75f,
			1f,
			0.75f,
			0.75f
		};

		public byte InventoryMode;

		private bool restoreOldInventorySection;

		private InventorySection oldInventorySection;

		public InventorySection ActiveInvSection = InventorySection.ITEMS;

		private sbyte inventoryItemX;

		private sbyte inventoryItemY;

		private sbyte inventoryChestX;

		private sbyte inventoryChestY;

		private sbyte inventoryEquipX;

		private sbyte inventoryEquipY = 1;

#if VERSION_103 || VERSION_FINAL
		private float inventoryBuffScrollX; // change later

		private int dx2;
		private int dy2;
#endif

		private sbyte inventoryBuffX;

		private sbyte inventoryHousingX;

		private sbyte inventoryHousingY;

		private short inventoryHousingNpc = -1;

		public Recipe.Category craftingCategory;

		private CraftingSection craftingSection;

		public bool craftingShowCraftable;

		public sbyte CraftingRecipeX;

		public sbyte CraftingRecipeY;

		private sbyte craftingIngredientX;

		private sbyte craftingIngredientY;

		private float craftingRecipeScrollX;

		private float craftingRecipeScrollY;

		public Recipe CraftingRecipe;

		private float craftingRecipeScrollMul = CRAFTING_DEFAULT_SCROLL_MUL;

		public short stackSplit;

		public short stackCounter;

		public short stackDelay = 7;

		private sbyte mouseItemSrcX;

		private sbyte mouseItemSrcY;

		private InventorySection mouseItemSrcSection = InventorySection.NUM_SECTIONS;

		public Item mouseItem = default(Item);

		public Item trashItem = default(Item);

		public Item GuideItem = default(Item);

		private Item toolTip = default(Item);

		public List<Recipe.SubCategoryList> CurrentRecipeCategory = new List<Recipe.SubCategoryList>();

		public byte npcShop;

		public bool CraftGuide;

		public bool reforge;

		public string chestText;

		public bool editSign;

		public bool signBubble;

		public int signX;

		public int signY;

		public UserString npcChatText;

		public string npcCompiledChatText;

		private CompiledText npcChatCompiledText;

		public short helpText;

		public sbyte npcChatSelectedItem;

		public bool showNPCs;

		public MiniMap miniMap = new MiniMap();

		private int mapScreenCursorX;

		private int mapScreenCursorY;

		public byte teamSelected;

		public bool pvpSelected;

		public short teamCooldown;

		public short pvpCooldown;

		public Statistics Statistics;

		public TriggerSystem AchievementTriggers;

		public uint TotalJumps;

		public uint TotalChopsTaken;

		public uint TotalDeadSlimes;

		public uint TotalWoodAxed;

		public uint TotalCopperObtained;

		public uint TotalDoorsOpened;

		public uint TotalDoorsClosed;

		public uint TotalWoodPlatformsPlaced;

		public uint TotalWallsPlaced;

		public uint TotalTorchesCrafted;

		public uint TotalWoodPlatformsCrafted;

		public uint TotalWallsCrafted;

		public uint totalSteps;

		public uint TotalBarsCrafted;

		public uint TotalOrePicked;

		public uint TotalAnvilCrafting;

		public uint totalWires;

		public uint totalAirTime;

		public uint currentAirTime;

#if !USE_ORIGINAL_CODE
		public uint RunningTimer = 0;
#endif

		public float airTravel;

		public byte petSpawnMask;

		private BitArray armorFound = new BitArray((int)Item.ID.NUM_TYPES);

		private readonly List<ulong> blacklist = new List<ulong>();

		private static Main theGame;

		public static byte MouseTextBrightness = 175;

		private static sbyte mouseTextColorChange = 2;

		public static Color mouseTextColor = new Color(175, 175, 175, 175);

		public static Color mouseColor = new Color(255, 95, 180);

		public static Color CursorColour = Color.White;

		public static float cursorAlpha = 0f;

		public static float cursorScale = 0f;

		public static byte invAlpha = 180;

		private static sbyte invDir = 1;

		public static float PulseScale = 1f;

		private static float essDir = -0.01f;

		public static float BlueWave = 1f;

		private static float blueDelta = -0.0005f;

		public static bool HasQuit = false;

		public static Texture2D logoTexture, logo2Texture, controlsTexture;

#if !USE_ORIGINAL_CODE
		public static Texture2D dpadTexture;
#endif

		public static Texture2D progressBarTexture, textBackTexture, chatBackTexture, cursorTexture;

		public static SpriteFont BigFont, SmallFont, BoldSmallFont, ItemStackFont;

		public static SpriteFont[] CombatTextFont = new SpriteFont[2];

		public static CompiledText.Style BoldSmallTextStyle;

		private static readonly short ViewportHalfWidth = (short)(Main.ResolutionWidth / 2);

		private static readonly Location[] MENU_TITLE_COORDS = new Location[MENU_TITLE_H + 1]
		{
			new Location(Main.ResolutionWidth / 2, 199),
			new Location(Main.ResolutionWidth / 2, 237),
			new Location(Main.ResolutionWidth / 2, 275),
			new Location(Main.ResolutionWidth / 2, 313),
			new Location(Main.ResolutionWidth / 2, 351),
			new Location(Main.ResolutionWidth / 2, 399),
			new Location(Main.ResolutionWidth / 2, 437),
			new Location(Main.ResolutionWidth / 2, 475)
		};

		private static readonly Location[] MENU_PAUSE_COORDS = new Location[MENU_PAUSE_H]
		{
			new Location(Main.ResolutionWidth / 2, 221),
			new Location(Main.ResolutionWidth / 2, 259),
			new Location(Main.ResolutionWidth / 2, 297),
			new Location(Main.ResolutionWidth / 2, 334),
			new Location(Main.ResolutionWidth / 2, 372),
			new Location(Main.ResolutionWidth / 2, 421),
			new Location(Main.ResolutionWidth / 2, 459)
		};

		private static Location[] MENU_SELECT_COORDS = new Location[MENU_SELECT_H]
		{
			new Location(Main.ResolutionWidth / 2, 216),
			new Location(Main.ResolutionWidth / 2, 253),
			new Location(Main.ResolutionWidth / 2, 291),
			new Location(Main.ResolutionWidth / 2, 329),
			new Location(Main.ResolutionWidth / 2, 367),
			new Location(Main.ResolutionWidth / 2, 410)
		};

		private static Location[] MENU_CONFIRM_DELETE_COORDS = new Location[MENU_CONFIRM_DELETE_H]
		{
			new Location(Main.ResolutionWidth / 2, 356),
			new Location(Main.ResolutionWidth / 2, 437)
		};

		private static readonly Location[] MENU_WORLD_SIZE_COORDS = new Location[MENU_WORLD_SIZE_H]
		{
			new Location(Main.ResolutionWidth / 2, 307),
			new Location(Main.ResolutionWidth / 2, 367),
			new Location(Main.ResolutionWidth / 2, 415)
		};

		private static readonly Location[] MENU_OPTIONS_COORDS = new Location[MENU_OPTIONS_H]
		{
			new Location(Main.ResolutionWidth / 2, 253),
			new Location(Main.ResolutionWidth / 2, 313),
			new Location(Main.ResolutionWidth / 2, 372),
			new Location(Main.ResolutionWidth / 2, 432)
		};

		private static readonly Location[] MENU_SETTINGS_COORDS = new Location[MENU_SETTINGS_H]
		{
			new Location(Main.ResolutionWidth / 2, 270),
			new Location(Main.ResolutionWidth / 2, 340),
			new Location(Main.ResolutionWidth / 2, 410)
		};

		private string[] menuString = new string[MAX_ITEMS];

		private byte numMenuItems;

		private sbyte oldMenu;

#if VERSION_INITIAL
		private sbyte showPlayer = -1;
#else
		public sbyte showPlayer = -1;
#endif

		private byte menuSpace = 80;

		private short menuTop = 250;

		private short menuLeft = 480;

		public bool inputTextEnter;

		public bool inputTextCanceled;

		private IAsyncResult kbResult;

		private string focusText;

		private string focusText3;

		private Color focusColor;

		private static CompiledText compiledToolTipText;

		private static string toolTipText;

		private static Item cpItem = default(Item);

		public static void Error(string caption, string desc, bool rememberPreviousMenu = false)
		{
			errorCompiledText = null;
			errorCaption = caption;
			errorDescription = desc;
			MainUI.SetMenu(MenuMode.ERROR, rememberPreviousMenu);
		}

		public void InitGame()
		{
			wasRemovedFromSessionWithoutOurConsent = false;
			restoreOldInventorySection = false;
			InventoryMode = 0;
			ActiveInvSection = InventorySection.ITEMS;
			inventoryItemX = 0;
			inventoryItemY = 0;
			inventoryChestX = 0;
			inventoryChestY = 0;
			inventoryEquipX = 0;
			inventoryEquipY = 1;
			inventoryBuffX = 0;
			inventoryHousingX = 0;
			inventoryHousingY = 0;
			inventoryHousingNpc = -1;
			craftingCategory = Recipe.Category.STRUCTURES;
			craftingSection = CraftingSection.RECIPES;
			craftingShowCraftable = false;
			CraftingRecipeX = 0;
			CraftingRecipeY = 0;
			craftingIngredientX = 0;
			craftingIngredientY = 0;
			craftingRecipeScrollX = 0f;
			craftingRecipeScrollY = 0f;
			mouseItem.Init();
			trashItem.Init();
			GuideItem.Init();
			toolTip.Init();
			helpText = 0;
			showNPCs = false;
			mapScreenCursorX = 0;
			mapScreenCursorY = 0;
			teamSelected = 0;
			pvpSelected = false;
			teamCooldown = 0;
			pvpCooldown = 0;
			npcShop = 0;
			CraftGuide = false;
			reforge = false;
			editSign = false;
			signBubble = false;
			npcChatText = null;
			npcChatSelectedItem = 0;
			ActivePlayer.hostile = false;
			ActivePlayer.NetClone(netPlayer);
#if !USE_ORIGINAL_CODE
			if (!Main.IsTrial)
			{
				InitializeAchievementTriggers();
			}
#else
			InitializeAchievementTriggers();
#endif
#if !VERSION_INITIAL
			quickAccessUp = ActivePlayer.PlayerQuickAccess[0];
			quickAccessDown = ActivePlayer.PlayerQuickAccess[1];
			quickAccessLeft = ActivePlayer.PlayerQuickAccess[2];
			quickAccessRight = ActivePlayer.PlayerQuickAccess[3];
#endif
		}

		private void InitializeAchievementTriggers()
		{
			AchievementTriggers.ReadProfile(SignedInGamer);
		}

		public bool TriggerCheckEnabled(Trigger trigger)
		{
			return AchievementTriggers.CheckEnabled(trigger);
		}

		public void SetTriggerState(Trigger trigger)
		{
			AchievementTriggers.SetState(trigger, State: true);
		}

		public static void SetTriggerStateForAll(Trigger trigger)
		{
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				Player player = Main.PlayerSet[i];
				if (player.Active != 0)
				{
					player.AchievementTrigger(trigger);
				}
			}
		}

		public static void IncreaseStatisticForAll(StatisticEntry entry)
		{
			if (entry == StatisticEntry.Unknown)
			{
				return;
			}
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				Player player = Main.PlayerSet[i];
				if (player.Active != 0)
				{
					player.IncreaseStatistic(entry);
				}
			}
		}

		private void UpdateAchievements()
		{
			if (Statistics.AllSlimeTypesKilled)
			{
				SetTriggerState(Trigger.AllSlimesKilled);
			}
			if (Statistics.AllBossesKilled)
			{
				SetTriggerState(Trigger.AllBossesKilled);
			}
#if !USE_ORIGINAL_CODE
			if (Statistics.MechBossesKilledTwice)
			{
				SetTriggerState(Trigger.BackForSeconds);
			}
#endif
			AchievementTriggers.UpdateAchievements(SignedInGamer);
		}

		public static void Initialize(Main game)
		{
			theGame = game;
			HowToPlay.UI.GenerateCache(game.GraphicsDevice);
			TextSequenceBlock.GenerateCache(game.GraphicsDevice);
#if !USE_ORIGINAL_CODE
			if (Main.HardmodeAlert)
			{
				Hardmode.UI.GenerateCache(theGame.GraphicsDevice);
			}
#endif
		}

		public void Initialize(PlayerIndex controller)
		{
			this.controller = controller;
			CurrentUI = this;
			if (MainUI == null)
			{
				MainUI = this;
			}
			for (int num = MAX_ITEMS - 1; num >= 0; num--)
			{
				menuItemScale[num] = 0.8f;
			}
			for (int num2 = 4; num2 >= 0; num2--)
			{
				loadPlayer[num2] = new Player();
			}
			CreateCharacterGUI = CreateCharacter.UI.Create(this);
			soundUI = SoundUI.UI.Create(this);
			howtoUI = HowToPlay.UI.Create(this);
			tips = TextSequenceBlock.CreateTips();
			Statistics = Statistics.Create();
			AchievementTriggers = new TriggerSystem();
			Leaderboards = new LeaderboardsUI(this);
#if !USE_ORIGINAL_CODE
			Achievements = new AchievementsUI(this);
			if (Main.HardmodeAlert)
			{
				hardmodeUpsell = Hardmode.UI.Create(this);
			}
#endif
		}

		private void SetDefaultSettings()
		{
			SoundVolume = 1f;
			MusicVolume = 0.75f;
			if (this == MainUI)
			{
				Main.MusicVolume = MusicVolume;
				Main.SoundVolume = SoundVolume;
			}
			autoSave = true;
			ShowItemText = true;
			alternateGrappleControls = false;
			UpdateAlternateGrappleControls();
			Statistics.Init();
			totalSteps = 0;
			TotalOrePicked = 0;
			TotalBarsCrafted = 0;
			TotalAnvilCrafting = 0;
			totalWires = 0;
			totalAirTime = 0;
			petSpawnMask = 0;
			armorFound.SetAll(value: false);
			IsOnline = false;
			IsInviteOnly = false;
			blacklist.Clear();
			SettingsDirty = false;
		}

		private void InitPlayerStorage()
		{
			SetDefaultSettings();
			numLoadPlayers = 0;
			if (SignedInGamer.IsGuest || Main.IsTrial)
			{
				playerStorage = null;
				return;
			}
			if (playerStorage == null)
			{
				playerStorage = new StorageDeviceManager(theGame, controller, 196608);
				playerStorage.DeviceSelectorCanceled += DeviceSelectorCanceled;
				playerStorage.DeviceDisconnected += DeviceDisconnected;
				playerStorage.DeviceSelected += DeviceSelected;
				((Collection<IGameComponent>)(object)theGame.Components).Add(playerStorage);
			}
			if (playerStorage.Device == null)
			{
				playerStorage.PromptForDevice();
			}
		}

		public bool HasPlayerStorage()
		{
			if (playerStorage != null)
			{
				return playerStorage.Device != null;
			}
			return false;
		}

		public bool CanViewGamerCard()
		{
			if (SignedInGamer.IsSignedInToLive && SignedInGamer.Privileges.AllowProfileViewing != 0)
			{
#if USE_ORIGINAL_CODE
				return !GuideExtensions.IsNetworkCableUnplugged;
#else
				return false;
#endif
			}
			return false;
		}

		public bool HasOnline()
		{
			if (!Main.IsTrial && SignedInGamer.IsSignedInToLive)
			{
#if USE_ORIGINAL_CODE
				return !GuideExtensions.IsNetworkCableUnplugged;
#else
                return false;
#endif
            }
            return false;
		}

		public bool HasOnlineWithPrivileges()
		{
			if (!Main.IsTrial && SignedInGamer.IsSignedInToLive && SignedInGamer.Privileges.AllowOnlineSessions)
			{
#if USE_ORIGINAL_CODE
				return !GuideExtensions.IsNetworkCableUnplugged;
#else
                return false;
#endif
            }
            return false;
		}

		public static bool IsUserGeneratedContentAllowed()
		{
			GamerCollection<NetworkGamer> gamerCollection = ((Netplay.Session != null) ? Netplay.Session.RemoteGamers : null);
			SignedInGamerCollection signedInGamers = Gamer.SignedInGamers;
			for (int num = ((ReadOnlyCollection<SignedInGamer>)(object)signedInGamers).Count - 1; num >= 0; num--)
			{
				SignedInGamer signedInGamer = ((ReadOnlyCollection<SignedInGamer>)(object)signedInGamers)[num];
				if (!signedInGamer.IsGuest && signedInGamer.IsSignedInToLive)
				{
					if (signedInGamer.Privileges.AllowUserCreatedContent == GamerPrivilegeSetting.Blocked)
					{
						return false;
					}
					if (gamerCollection != null && signedInGamer.Privileges.AllowUserCreatedContent == GamerPrivilegeSetting.FriendsOnly)
					{
						for (int num2 = ((ReadOnlyCollection<NetworkGamer>)(object)gamerCollection).Count - 1; num2 >= 0; num2--)
						{
							NetworkGamer gamer = ((ReadOnlyCollection<NetworkGamer>)(object)gamerCollection)[num2];
							if (!signedInGamer.IsFriend(gamer))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		public bool CanPlayOnline()
		{
			if (HasOnlineWithPrivileges())
			{
				return IsUserGeneratedContentAllowed();
			}
			return false;
		}

		public bool CanCommunicate()
		{
			return SignedInGamer.Privileges.AllowCommunication != GamerPrivilegeSetting.Blocked;
		}

		public static bool AllPlayersCanPlayOnline()
		{
			if (IsUserGeneratedContentAllowed())
			{
				for (int i = 0; i < 4; i++)
				{
					UI uI = Main.UIInstance[i];
					if (uI.SignedInGamer != null && !uI.HasOnlineWithPrivileges())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static void LoadContent(ContentManager Content)
		{
			CreateCharacter.Assets.LoadContent(Content);
			SoundUI.Assets.LoadContent(Content);
			HowToPlay.Assets.LoadContent(Content);
			Terraria.Leaderboards.Assets.LoadContent(Content);
			
			logoTexture = Content.Load<Texture2D>("Images/Logo");
			logo2Texture = Content.Load<Texture2D>("Images/Logo2");

#if USE_ORIGINAL_CODE
			controlsTexture = Content.Load<Texture2D>("UI/Controller_Layout01");
#else
			Terraria.Achievements.Assets.LoadContent(Content);

			if (Main.HardmodeAlert)
			{
				Hardmode.Assets.LoadContent(Content);
			}

			string ControllerPath = "UI/XBController";
			string DPadPath = "UI/XBDPad";
			if (Main.PSMode)
			{
				ControllerPath = "UI/PSController";
				DPadPath = "UI/PSDPad";
			}
			if (Main.ScreenHeightPtr == 2)
			{
				ControllerPath += "HD";
			}

			controlsTexture = Content.Load<Texture2D>(ControllerPath);
			dpadTexture = Content.Load<Texture2D>(DPadPath);
#endif

			progressBarTexture = Content.Load<Texture2D>("UI/ProgressBar");

#if VERSION_INITIAL
			textBackTexture = Content.Load<Texture2D>("Images/Text_Back"); // Unused
#endif

#if !VERSION_103 && !VERSION_FINAL
			chatBackTexture = Content.Load<Texture2D>("Images/Chat_Back"); // The 1.2 versions ditched the use of Chat_Back and relied on simply drawing it...
#endif

			cursorTexture = Content.Load<Texture2D>("Images/Cursor");
			LoadFonts(Content);

#if USE_ORIGINAL_CODE && (!VERSION_INITIAL || IS_PATCHED) // Here for archival purposes; Can't be used.
            FontSmallOutlineFull = Content.Load<SpriteFont>("Fonts/small2"); // But... why?
            FontSmallOutlineFull.Spacing = -5f;
            FontSmallOutlineFull.LineSpacing = 22;
#endif
		}

		public static void LoadFonts(ContentManager Content)
		{
#if !USE_ORIGINAL_CODE
			string Directory = "Fonts/";
			string BigFontAsset = "big";
			string StackFontAsset = "stack";
			string CombatFontAsset = "combat";
			string Combat2FontAsset = "combat2";
			string SmallFontAsset = "small";
			string Small2FontAsset = "small2";

			if (Main.PSMode)
			{
				BigFontAsset += "_PS3";
				SmallFontAsset += "_PS3";
				Small2FontAsset += "_PS3";
			}

			if (Main.ScreenHeightPtr == 2)
			{
				Directory += "HD/";
			}

			Texture2D BigTex = Content.Load<Texture2D>(Directory + BigFontAsset);
			Texture2D StackTex = Content.Load<Texture2D>(Directory + StackFontAsset);
			Texture2D CombatTex = Content.Load<Texture2D>(Directory + CombatFontAsset);
			Texture2D Combat2Tex = Content.Load<Texture2D>(Directory + Combat2FontAsset);
			Texture2D SmallTex = Content.Load<Texture2D>(Directory + SmallFontAsset);
			Texture2D Small2Tex = Content.Load<Texture2D>(Directory + Small2FontAsset);

			if (Main.ScreenHeightPtr == 2)
			{
				FontHD FontHD = new FontHD();
				FontHDBig FontHDBig = new FontHDBig();
				FontHDSmall FontHDSmall = new FontHDSmall();

				BigFont = new SpriteFont(BigTex, FontHDBig.BigGlyphBounds, FontHDBig.BigGlyphCrops, FontHD.BigSmlChars, 152, -24f, FontHDBig.BigKerning, Convert.ToChar("~"));
				ItemStackFont = new SpriteFont(StackTex, FontHD.StackGlyphBounds, FontHD.StackGlyphCrops, FontHD.StackChars, 28, -0.5f, FontHD.StackKerning, Convert.ToChar("~"));
				CombatTextFont[0] = new SpriteFont(CombatTex, FontHD.CombatGlyphBounds, FontHD.CombatGlyphCrops, FontHD.CombatChars, 21, -1f, FontHD.CombatKerning, Convert.ToChar("~"));
				CombatTextFont[1] = new SpriteFont(Combat2Tex, FontHD.Combat2GlyphBounds, FontHD.Combat2GlyphCrops, FontHD.CombatChars, 22, -1.5f, FontHD.Combat2Kerning, Convert.ToChar("~"));
				SmallFont = new SpriteFont(SmallTex, FontHDSmall.SmallGlyphBounds, FontHDSmall.SmallGlyphCrops, FontHD.BigSmlChars, 30, -0.5f, FontHDSmall.SmallKerning, Convert.ToChar("~"));
				BoldSmallFont = new SpriteFont(Small2Tex, FontHDSmall.Small2GlyphBounds, FontHDSmall.Small2GlyphCrops, FontHD.BigSmlChars, 33, -4f, FontHDSmall.Small2Kerning, Convert.ToChar("~"));
			}
			else
			{
				Font Font = new Font();
				FontBig FontBig = new FontBig();
				FontSmall FontSmall = new FontSmall();

				if (!Main.PSMode)
				{
					FontSmall.SmallGlyphBounds.RemoveRange(95, 16);
					FontSmall.SmallGlyphBounds.InsertRange(96, Font.XBSmlBoundReplace);
					FontSmall.SmallGlyphCrops.RemoveRange(95, 16);
					FontSmall.SmallGlyphCrops.InsertRange(96, Font.XBSmlCropReplace);
					FontSmall.SmallKerning.RemoveRange(95, 16);
					FontSmall.SmallKerning.InsertRange(96, Font.XBSmlKerningReplace);

					FontSmall.Small2GlyphBounds.RemoveRange(95, 16);
					FontSmall.Small2GlyphBounds.InsertRange(96, Font.XBSml2BoundReplace);
					FontSmall.Small2GlyphCrops.RemoveRange(95, 16);
					FontSmall.Small2GlyphCrops.InsertRange(96, Font.XBSml2CropReplace);
					FontSmall.Small2Kerning.RemoveRange(95, 16);
					FontSmall.Small2Kerning.InsertRange(96, Font.XBSml2KerningReplace);
				}

				BigFont = new SpriteFont(BigTex, FontBig.BigGlyphBounds, FontBig.BigGlyphCrops, Font.BigSmlChars, 109, -24f, FontBig.BigKerning, Convert.ToChar("~"));
				ItemStackFont = new SpriteFont(StackTex, Font.StackGlyphBounds, Font.StackGlyphCrops, Font.StackChars, 20, -4f, Font.StackKerning, Convert.ToChar("~"));
				CombatTextFont[0] = new SpriteFont(CombatTex, Font.CombatGlyphBounds, Font.CombatGlyphCrops, Font.CombatChars, 21, -3f, Font.CombatKerning, Convert.ToChar("~"));
				CombatTextFont[1] = new SpriteFont(Combat2Tex, Font.Combat2GlyphBounds, Font.Combat2GlyphCrops, Font.CombatChars, 22, -4f, Font.Combat2Kerning, Convert.ToChar("~"));
				SmallFont = new SpriteFont(SmallTex, FontSmall.SmallGlyphBounds, FontSmall.SmallGlyphCrops, Font.BigSmlChars, 20, -2f, FontSmall.SmallKerning, Convert.ToChar("~"));
				BoldSmallFont = new SpriteFont(Small2Tex, FontSmall.Small2GlyphBounds, FontSmall.Small2GlyphCrops, Font.BigSmlChars, 22, -5.11f, FontSmall.Small2Kerning, Convert.ToChar("~"));
			}
#else
			BigFont = Content.Load<SpriteFont>("Fonts/big");
			BigFont.Spacing = -24f;
			ItemStackFont = Content.Load<SpriteFont>("Fonts/stack");
			ItemStackFont.Spacing = -4f;
			FONT_STACK_EXTRA_OFFSET = -5;
			CombatTextFont[0] = Content.Load<SpriteFont>("Fonts/combat");
			CombatTextFont[0].Spacing = -3f;
			CombatTextFont[1] = Content.Load<SpriteFont>("Fonts/combat2");
			CombatTextFont[1].Spacing = -4f;
			SmallFont = Content.Load<SpriteFont>("Fonts/small");
			SmallFont.Spacing = -2f;
			SmallFont.LineSpacing = 20;
			BoldSmallFont = Content.Load<SpriteFont>("Fonts/small2");
			BoldSmallFont.Spacing = -5f;
			BoldSmallFont.LineSpacing = 22;
#endif
			BoldSmallTextStyle = new CompiledText.Style(BoldSmallFont);
		}

		public static void LoadSplitscreenFonts(ContentManager Content)
		{
#if !USE_ORIGINAL_CODE
			string Directory = "Fonts/";
			string BigFontAsset = "big_sc";
			string StackFontAsset = "stack_sc";
			string CombatFontAsset = "combat_sc";
			string Combat2FontAsset = "combat2_sc";
			string SmallFontAsset = "small_sc";
			string Small2FontAsset = "small2_sc";

			if (Main.PSMode)
			{
				BigFontAsset += "_PS3";
				// None for small_sc as they have no graphics.
				Small2FontAsset += "_PS3";
			}

			if (Main.ScreenHeightPtr == 2)
			{
				Directory += "HD/";
			}

			Texture2D BigTex = Content.Load<Texture2D>(Directory + BigFontAsset);
			Texture2D StackTex = Content.Load<Texture2D>(Directory + StackFontAsset);
			Texture2D CombatTex = Content.Load<Texture2D>(Directory + CombatFontAsset);
			Texture2D Combat2Tex = Content.Load<Texture2D>(Directory + Combat2FontAsset);
			Texture2D SmallTex = Content.Load<Texture2D>(Directory + SmallFontAsset);
			Texture2D Small2Tex = Content.Load<Texture2D>(Directory + Small2FontAsset);

			if (Main.ScreenHeightPtr == 2)
			{
				FontHD FontHD = new FontHD();
				FontHDBig FontHDBig = new FontHDBig();
				FontHDSmall FontHDSmall = new FontHDSmall();

				BigFont = new SpriteFont(BigTex, FontHDBig.BigSCGlyphBounds, FontHDBig.BigSCGlyphCrops, FontHD.BigSmlChars, 28, -9.75f, FontHDBig.BigSCKerning, Convert.ToChar("~"));
				ItemStackFont = new SpriteFont(StackTex, FontHD.StackSCGlyphBounds, FontHD.StackSCGlyphCrops, FontHD.StackChars, 21, -0.5f, FontHD.StackSCKerning, Convert.ToChar("~"));
				CombatTextFont[0] = new SpriteFont(CombatTex, FontHD.CombatBothSCGlyphBounds, FontHD.CombatBothSCGlyphCrops, FontHD.CombatChars, 17, 0.5f, FontHD.CombatBothSCKerning, Convert.ToChar("~"));
				CombatTextFont[1] = new SpriteFont(Combat2Tex, FontHD.CombatBothSCGlyphBounds, FontHD.CombatBothSCGlyphCrops, FontHD.CombatChars, 17, -0.5f, FontHD.CombatBothSCKerning, Convert.ToChar("~"));
				SmallFont = new SpriteFont(SmallTex, FontHDSmall.SmallSCGlyphBounds, FontHDSmall.SmallSCGlyphCrops, FontHD.BigSmlChars, 15, -3.3f, FontHDSmall.SmallSCKerning, Convert.ToChar("~"));
				BoldSmallFont = new SpriteFont(Small2Tex, FontHDSmall.Small2SCGlyphBounds, FontHDSmall.Small2SCGlyphCrops, FontHD.BigSmlChars, 22, -3f, FontHDSmall.Small2SCKerning, Convert.ToChar("~"));
			}
			else
			{
				Font Font = new Font();
				FontBig FontBig = new FontBig();
				FontSmall FontSmall = new FontSmall();

				if (!Main.PSMode)
				{
					FontSmall.Small2SCGlyphBounds.RemoveRange(95, 16);
					FontSmall.Small2SCGlyphBounds.InsertRange(96, Font.XBSml2SCBoundReplace);
					FontSmall.Small2SCGlyphCrops.RemoveRange(95, 16);
					FontSmall.Small2SCGlyphCrops.InsertRange(96, Font.XBSml2SCCropReplace);
					FontSmall.Small2SCKerning.RemoveRange(95, 16);
					FontSmall.Small2SCKerning.InsertRange(96, Font.XBSml2SCKerningReplace);
				}

				BigFont = new SpriteFont(BigTex, FontBig.BigSCGlyphBounds, FontBig.BigSCGlyphCrops, Font.BigSmlChars, 19, -13f, FontBig.BigSCKerning, Convert.ToChar("~"));
				ItemStackFont = new SpriteFont(StackTex, Font.StackSCGlyphBounds, Font.StackSCGlyphCrops, Font.StackChars, 15, -4f, Font.StackSCKerning, Convert.ToChar("~"));
				FONT_STACK_EXTRA_OFFSET = -8;
				CombatTextFont[0] = new SpriteFont(CombatTex, Font.CombatBothSCGlyphBounds, Font.CombatBothSCGlyphCrops, Font.CombatChars, 17, -2f, Font.CombatBothSCKerning, Convert.ToChar("~"));
				CombatTextFont[1] = new SpriteFont(Combat2Tex, Font.CombatBothSCGlyphBounds, Font.CombatBothSCGlyphCrops, Font.CombatChars, 17, -3f, Font.CombatBothSCKerning, Convert.ToChar("~"));
				SmallFont = new SpriteFont(SmallTex, FontSmall.SmallSCGlyphBounds, FontSmall.SmallSCGlyphCrops, Font.BigSmlChars, 10, -2f, FontSmall.SmallSCKerning, Convert.ToChar("~"));
				BoldSmallFont = new SpriteFont(Small2Tex, FontSmall.Small2SCGlyphBounds, FontSmall.Small2SCGlyphCrops, Font.BigSmlChars, 13, -2f, FontSmall.Small2SCKerning, Convert.ToChar("~"));
			}
#else
			BigFont = Content.Load<SpriteFont>("Fonts/big_sc");
			BigFont.Spacing = -13f;
			BigFont.LineSpacing = 19;
			ItemStackFont = Content.Load<SpriteFont>("Fonts/stack_sc");
			ItemStackFont.Spacing = -4f;
			FONT_STACK_EXTRA_OFFSET = -8;
			CombatTextFont[0] = Content.Load<SpriteFont>("Fonts/combat_sc");
			CombatTextFont[0].Spacing = -2f;
			CombatTextFont[1] = Content.Load<SpriteFont>("Fonts/combat2_sc");
			CombatTextFont[1].Spacing = -3f;
			SmallFont = Content.Load<SpriteFont>("Fonts/small_sc");
			SmallFont.Spacing = -2f;
			SmallFont.LineSpacing = 10;
			BoldSmallFont = Content.Load<SpriteFont>("Fonts/small2_sc");
			BoldSmallFont.Spacing = -2f;
			BoldSmallFont.LineSpacing = 13;
#endif
			BoldSmallTextStyle = new CompiledText.Style(BoldSmallFont);
		}

		private void InvalidateCachedText()
		{
			toolTipText = null;
			npcCompiledChatText = null;
			errorCompiledText = null;
			HowToPlay.UI.GenerateCache(theGame.GraphicsDevice);
			TextSequenceBlock.GenerateCache(theGame.GraphicsDevice);
#if !USE_ORIGINAL_CODE
			if (Main.HardmodeAlert)
			{
				Hardmode.UI.GenerateCache(theGame.GraphicsDevice);
			}
#endif
		}

		public static float Spacing(SpriteFont font)
		{
			float num = font.Spacing;
			if (NumActiveViews > 1)
			{
				num *= 2f;
			}
			return num;
		}

		public static int LineSpacing(SpriteFont font)
		{
			int num = font.LineSpacing;
			if (NumActiveViews > 1)
			{
				num <<= 1;
			}
			return num;
		}

		public static Vector2 MeasureString(SpriteFont font)
		{
			Vector2 result = font.MeasureString(Main.StrBuilder);
			if (NumActiveViews > 1)
			{
				result.X *= 2f;
				result.Y *= 2f;
			}
			return result;
		}

		public static float MeasureStringX(SpriteFont font)
		{
			float num = font.MeasureString(Main.StrBuilder).X;
			if (NumActiveViews > 1)
			{
				num *= 2f;
			}
			return num;
		}

		public static Vector2 MeasureString(SpriteFont font, string text)
		{
			Vector2 result = font.MeasureString(text);
			if (NumActiveViews > 1)
			{
				result.X *= 2f;
				result.Y *= 2f;
			}
			return result;
		}

		// If we want the text to be larger in 720p since we have no font files, *= 1.25f is a good look for it.
		private static readonly float TextScale = 1; // (Main.ScreenHeightPtr != 1) ? 1 : 1.25f;

		public static void DrawStringLB(SpriteFont font, int x, int y)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 vector = font.MeasureString(Main.StrBuilder);
			Main.SpriteBatch.DrawString(font, Main.StrBuilder, new Vector2(x, Main.ResolutionHeight - y), Color.White, 0f, new Vector2(0f, vector.Y), scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringLT(SpriteFont font, int x, int y, Color c)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Main.SpriteBatch.DrawString(font, Main.StrBuilder, new Vector2(x, y), c, 0f, default, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringLT(SpriteFont font, string s, int x, int y, Color c)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Main.SpriteBatch.DrawString(font, s, new Vector2(x, y), c, 0f, default, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringScaled(SpriteFont font, string s, Vector2 pos, Color c, Vector2 pivot, float scale)
		{
			scale *= TextScale;
			if (NumActiveViews > 1)
			{
				scale *= 2f;
				pivot.X *= 0.5f;
				pivot.Y *= 0.5f;
			}
			Main.SpriteBatch.DrawString(font, s, pos, c, 0f, pivot, scale, SpriteEffects.None, 0f);
		}

		public static void DrawString(SpriteFont font, string s, Vector2 pos, Color c, float rot, Vector2 pivot, float scale)
		{
			scale *= TextScale;
			if (NumActiveViews > 1)
			{
				scale *= 2f;
				pivot.X *= 0.5f;
				pivot.Y *= 0.5f;
			}
			Main.SpriteBatch.DrawString(font, s, pos, c, rot, pivot, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringScaled(SpriteFont font, Vector2 pos, Color c, Vector2 pivot, float scale)
		{
			scale *= TextScale;
			if (NumActiveViews > 1)
			{
				scale *= 2f;
				pivot.X *= 0.5f;
				pivot.Y *= 0.5f;
			}
			Main.SpriteBatch.DrawString(font, Main.StrBuilder, pos, c, 0f, pivot, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringCC(SpriteFont font, string s, int x, int y, Color c)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 origin = font.MeasureString(s);
			origin.X = (float)Math.Round(origin.X * 0.5);
			origin.Y = (float)Math.Round(origin.Y * 0.5);
			Main.SpriteBatch.DrawString(font, s, new Vector2(x, y), c, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringLC(SpriteFont font, string s, int x, int y, Color c)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 origin = font.MeasureString(s);
			origin.X = 0f;
			origin.Y = (float)Math.Round(origin.Y * 0.5);
			Main.SpriteBatch.DrawString(font, s, new Vector2(x, y), c, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public static void DrawStringRC(SpriteFont font, string s, int x, int y, Color c)
		{
			float scale = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 origin = font.MeasureString(s);
			origin.Y = (float)Math.Round(origin.Y * 0.5);
			Main.SpriteBatch.DrawString(font, s, new Vector2(x, y), c, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public static float DrawStringCT(SpriteFont font, string s, int x, int y, Color c)
		{
			float num = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 origin = font.MeasureString(s);
			origin.X = (float)Math.Round(origin.X * 0.5);
			float result = origin.Y * num;
			origin.Y = 0f;
			Main.SpriteBatch.DrawString(font, s, new Vector2(x, y), c, 0f, origin, num, SpriteEffects.None, 0f);
			return result;
		}

		public static float DrawStringCT(SpriteFont font, int x, int y, Color c)
		{
			float num = (NumActiveViews <= 1) ? TextScale : (TextScale * 2);
			Vector2 vector = font.MeasureString(Main.StrBuilder);
			vector.X *= 0.5f;
			float result = vector.Y * num;
			vector.Y = 0f;
			Main.SpriteBatch.DrawString(font, Main.StrBuilder, new Vector2(x, y), c, 0f, vector, num, SpriteEffects.None, 0f);
			return result;
		}

		public void PrevMenu(int depth = -1)
		{
			Main.PlaySound(11);
			if (depth < 0)
			{
				menuDepth += depth;
				if (menuDepth >= 0)
				{
					SetMenu(prevMenuMode[menuDepth], rememberPrevious: false);
				}
				else
				{
					SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
				}
			}
			else if (depth < menuDepth)
			{
				menuDepth = depth;
				SetMenu(prevMenuMode[depth], rememberPrevious: false);
			}
			else
			{
				SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
			}
		}

		private void ResetPlayerMenuSelection()
		{
			uiX = 0;
			uiY = (short)((numLoadPlayers <= 0) ? 5 : 0);
		}

		public void SetMenu(MenuMode mode, bool rememberPrevious = true, bool reset = false)
		{
			if (SettingsDirty)
			{
				SaveSettings();
			}
			numMenuItems = 0;
			if (reset)
			{
				menuDepth = 0;
			}
			if (mode == MenuMode.TITLE)
			{
				Main.SetTutorial(Tutorial.NUM_TUTORIALS);
				if (!Main.IsTrial && saveIconMessage == null)
				{
#if USE_ORIGINAL_CODE
					saveIconMessage = new CompiledText(Lang.MenuText[4], 470, BoldSmallTextStyle);
#else
					int WrapWidth = 470;
					switch (Main.ScreenHeightPtr)
					{
						case 1:
							WrapWidth = 548;
							break;
						case 2:
							WrapWidth = 705;
							break;
					}
					saveIconMessage = new CompiledText(Lang.MenuText[4], WrapWidth, BoldSmallTextStyle);
#endif
					saveIconMessageTime = Main.SaveIconMessageTime;
				}
				if (SignedInGamer != null)
				{
#if USE_ORIGINAL_CODE
					SignedInGamer.Presence.SetPresenceModeString("Menu");
#else
                    SignedInGamer.Presence.SetPresenceModeStringEXT("Menu");
#endif
                }
                if (Netplay.isJoiningRemoteInvite)
				{
					if (!Netplay.gamersWaitingToJoinInvite.Contains(SignedInGamer))
					{
						Exit();
						return;
					}
					mode = MenuMode.CHARACTER_SELECT;
					if (this == MainUI)
					{
						for (int num = Netplay.gamersWaitingToJoinInvite.Count - 1; num >= 0; num--)
						{
							SignedInGamer signedInGamer = Netplay.gamersWaitingToJoinInvite[num];
							if (signedInGamer != SignedInGamer)
							{
								UI uI = Main.UIInstance[(int)signedInGamer.PlayerIndex];
								uI.SetMenu(MenuMode.CHARACTER_SELECT, rememberPrevious: false, reset: true);
								uI.OpenView();
							}
						}
					}
				}
			}
			if (CurMenuMode != 0)
			{
				for (int num2 = menuHC.Length - 1; num2 >= 0; num2--)
				{
					menuHC[num2] = 0;
				}
				if (rememberPrevious)
				{
					prevMenuMode[menuDepth++] = CurMenuMode;
				}
				uiPos[(uint)CurMenuMode].X = uiX;
				uiPos[(uint)CurMenuMode].Y = uiY;
			}
			CurMenuMode = mode;
			uiX = uiPos[(uint)mode].X;
			uiY = uiPos[(uint)mode].Y;
			uiDelay = 0;
			uiDelayValue = UI_DELAY;
			switch (mode)
			{
			case MenuMode.PAUSE:
				worldFadeTarget = 0.375f;
				uiWidth = MENU_PAUSE_W;
				uiHeight = MENU_PAUSE_H;
				uiCoords = MENU_PAUSE_COORDS;
				return;
			case MenuMode.TITLE:
				uiWidth = MENU_TITLE_W;
				uiHeight = MENU_TITLE_H;
				uiCoords = MENU_TITLE_COORDS;
				return;
#if VERSION_INITIAL
			case MenuMode.CHARACTER_SELECT:
				uiWidth = MENU_SELECT_W;
				uiHeight = MENU_SELECT_H;
				uiCoords = MENU_SELECT_COORDS;
				initCharacterSelectCoordinates();
				return;
#endif
			case MenuMode.CONFIRM_LEAVE_CREATE_CHARACTER:
			case MenuMode.CONFIRM_DELETE_CHARACTER:
			case MenuMode.CONFIRM_DELETE_WORLD:
				uiWidth = MENU_CONFIRM_DELETE_W;
				uiHeight = MENU_CONFIRM_DELETE_H;
				uiCoords = MENU_CONFIRM_DELETE_COORDS;
				return;
			case MenuMode.VOLUME:
				soundUI.UpdateVolumes();
				break;
			case MenuMode.WORLD_SIZE:
				uiWidth = MENU_WORLD_SIZE_W;
				uiHeight = MENU_WORLD_SIZE_H;
				uiCoords = MENU_WORLD_SIZE_COORDS;
				return;
			case MenuMode.OPTIONS:
				uiWidth = MENU_OPTIONS_W;
				uiHeight = MENU_OPTIONS_H;
				uiCoords = MENU_OPTIONS_COORDS;
				return;
			case MenuMode.SETTINGS:
				uiWidth = MENU_SETTINGS_W;
				uiHeight = MENU_SETTINGS_H;
				uiCoords = MENU_SETTINGS_COORDS;
				return;
			case MenuMode.STATUS_SCREEN:
			case MenuMode.NETPLAY:
				Progress = 0f;
				progressTotal = 0f;
				uiWidth = 0;
				uiHeight = 1;
				uiCoords = null;
				statusText = null;
				return;
			case MenuMode.WORLD_SELECT:
				if (Netplay.availableSessions.Count == 0 && !Netplay.IsFindingSessions())
				{
					Netplay.FindSessions();
				}
				break;
			case MenuMode.ERROR:
				if (Main.WorldGenThread != null)
				{
						Main.WorldGenThread.Abort();
						Main.WorldGenThread = null;
						WorldGen.Gen = false;
				}
				break;
			case MenuMode.MAP:
				worldFadeTarget = 0.375f;
				uiFade = 0f;
				uiFadeTarget = 1f;
				break;
			case MenuMode.CREDITS:
				Credits.Init();
				break;
			case MenuMode.QUIT:
				MessageBox.Show(controller, Lang.MenuText[15], Lang.InterfaceText[35], new string[2]
				{
					Lang.MenuText[105],
					Lang.MenuText[104]
				}, ShouldAutoUpdate: false);
				break;
			case MenuMode.UPSELL:
				theGame.LoadUpsell();
				break;
			}
			uiWidth = 0;
			uiHeight = 0;
			uiCoords = null;
		}

		private void Exit()
		{
			if (NumActiveViews == 1)
			{
				if (Main.IsTrial)
				{
					SetMenu(MenuMode.UPSELL, rememberPrevious: false, reset: true);
				}
				else
				{
					HasQuit = true;
				}
				return;
			}
			SetPlayer(null);
			SignedInGamer = null;
			CurMenuType = MenuType.MAIN;
			CurMenuMode = MenuMode.WELCOME;
			selectedMenu = -1;
			focusMenu = -1;
			uiX = 0;
			uiY = 0;
			uiPos = new Location[(int)MenuMode.NUM_MENUS];
			worldFade = 0f;
			worldFadeTarget = 1f;
			isStopping = false;
			if (playerStorage != null && !Netplay.isJoiningRemoteInvite)
			{
				((Collection<IGameComponent>)(object)theGame.Components).Remove(playerStorage);
				playerStorage.Dispose();
				playerStorage = null;
			}
		}

		public void ExitGame()
		{
			Main.IsGameStarted = false;
			for (int i = 0; i < 4; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.CurrentView != null && uI.CurMenuType != 0)
				{
					uI.CurrentView.onStopGame();
				}
			}
			for (int j = 0; j < 4; j++)
			{
				UI uI2 = Main.UIInstance[j];
				if (uI2.CurrentView != null && uI2 != MainUI)
				{
					if (uI2.CurMenuType == MenuType.MAIN)
					{
						uI2.Exit();
					}
					else
					{
						uI2.StopGame();
					}
				}
			}
			MainUI.StopGame();
		}

		public void StopGame()
		{
			CloseInventory();
			ActiveInvSection = InventorySection.ITEMS;
			hotbarItemNameTime = HOTBAR_ITEMNAME_DISPLAYTIME;
			quickAccessDisplayTime = 0;
#if !VERSION_INITIAL
			ActivePlayer.PlayerQuickAccess[0] = quickAccessUp;
			ActivePlayer.PlayerQuickAccess[1] = quickAccessDown;
			ActivePlayer.PlayerQuickAccess[2] = quickAccessLeft;
			ActivePlayer.PlayerQuickAccess[3] = quickAccessRight;
#endif
			quickAccessUp = -1;
			quickAccessDown = -1;
			quickAccessLeft = -1;
			quickAccessRight = -1;
			isStopping = true;
			worldFadeTarget = 1f;
			if (Main.SaveOnExit)
			{
				statusText = Lang.MenuText[54];
			}
			else
			{
				statusText = "";
			}
			if (CurMenuMode != MenuMode.ERROR)
			{
				SetMenu(MenuMode.STATUS_SCREEN, rememberPrevious: false);
			}
			CurMenuType = MenuType.MAIN;
			if (this == MainUI)
			{
				if (Main.SaveOnExit)
				{
					Main.SaveOnExit = false;
					WorldGen.SaveAndQuit();
					return;
				}
				Netplay.PlayDisconnect = true;
				LoadPlayers();
				if (CurMenuMode != MenuMode.ERROR)
				{
					SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
				}
			}
			else if (!Main.SaveOnExit)
			{
				Exit();
			}
		}

		public void PrepareDraw(int pass)
		{
			if (CurrentView != null && CurMenuType != 0)
			{
				CurrentUI = this;
				CurrentView.PrepareDraw(pass);
			}
		}

		public void Draw()
		{
			if (CurrentView != null)
			{
				CurrentUI = this;
				CurrentView.DrawBg(this);
				if (CurMenuType != 0)
				{
					CurrentView.DrawWorld();
					DrawCursor();
					CurrentView.SetScreenView();
				}
				DrawTopLayer();
				if (CurMenuType != MenuType.NONE)
				{
					DrawMenu();
				}
				else if (Main.TutorialState < Tutorial.THE_END)
				{
					DrawTutorial();
				}
				Main.SpriteBatch.End();
			}
		}

		private void DrawTopLayer()
		{
			if (worldFade < 1f)
			{
#if USE_ORIGINAL_CODE
				Terraria.Main.DrawSolidRect(new Rectangle(-1, -1, CurrentView.ViewWidth, Terraria.Main.ResolutionHeight), new Color(0f, 0f, 0f, 1f - worldFade));
#else
				Main.DrawSolidRect(new Rectangle(0, 0, CurrentView.ViewWidth, Main.ResolutionHeight), new Color(0f, 0f, 0f, 1f - worldFade)); // Just like with the sky, initially was buggered due to starting at X:-1, Y:-1;
#endif
			}
			if (worldFade != worldFadeTarget)
			{
				if (worldFadeTarget < worldFade)
				{
					worldFade -= WORLD_FADE_SPEED;
					if (worldFadeTarget > worldFade)
					{
						worldFade = worldFadeTarget;
					}
				}
				else
				{
					worldFade += WORLD_FADE_SPEED;
					if (worldFadeTarget < worldFade)
					{
						worldFade = worldFadeTarget;
					}
				}
			}
			if (CurMenuType == MenuType.NONE)
			{
				DrawInterface();
				if (InventoryMode == 0 && !ActivePlayer.ghost)
				{
					DrawHud();
				}
			}
		}

		private void DrawTutorial()
		{
#if USE_ORIGINAL_CODE
			DrawDialog(new Vector2(CurrentView.ViewWidth - chatBackTexture.Width >> 1, Main.ResolutionHeight - CurrentView.SafeAreaOffsetBottom - 36), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), Main.TutorialText, null, anchorBottom: true);
#else
			int ChatBackTexSize = 500;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.15f);
					break;
				case 2:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.534f);
					break;
			}
			DrawDialog(new Vector2(CurrentView.ViewWidth - ChatBackTexSize >> 1, Main.ResolutionHeight - CurrentView.SafeAreaOffsetBottom - (int)(36 * Main.ScreenMultiplier)), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), Main.TutorialText, null, anchorBottom: true);
#endif
		}

		private void UpdateMenu()
		{
			MenuMode menuMode = CurMenuMode;
			numMenuItems = 0;
			menuTop = 250;
			menuLeft = (short)((CurrentView != null) ? (CurrentView.ViewWidth >> 1) : (Main.ResolutionWidth / 2));
			menuSpace = 80;
			showPlayer = -1;
			for (int i = 0; i < MAX_ITEMS; i++)
			{
				noFocus[i] = false;
				blockFocus[i] = false;
				menuY[i] = 0;
				menuScale[i] = 1f;
			}
			if (CurMenuMode == MenuMode.ERROR)
			{
				numMenuItems = 0;
				if (IsBackButtonTriggered())
				{
					SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
				}
			}
			else if (CurMenuMode == MenuMode.LOAD_FAILED_NO_BACKUP)
			{
				numMenuItems = 1;
				menuString[0] = Lang.MenuText[9];
				noFocus[0] = true;
				menuTop = 300;
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.STATUS_SCREEN)
			{
				numMenuItems = 1;
				menuString[0] = statusText;
				noFocus[0] = true;
				menuTop = 175;
				tips.Update();
			}
			else if (CurMenuMode == MenuMode.NETPLAY)
			{
				numMenuItems = 1;
				menuString[0] = statusText;
				noFocus[0] = true;
				menuTop = 175;
				tips.Update();
				if (IsBackButtonTriggered())
				{
					if (Netplay.sessionThread != null)
					{
						Netplay.PlayDisconnect = true;
					}
					PrevMenu();
				}
				else if (Netplay.sessionThread == null)
				{
					bool flag = true;
					for (int j = 0; j < 4; j++)
					{
						UI uI = Main.UIInstance[j];
						if (uI.CurrentView != null && uI.CurMenuMode != MenuMode.NETPLAY)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Netplay.StartClient();
					}
				}
			}
			else if (CurMenuMode == MenuMode.WAITING_SCREEN)
			{
				numMenuItems = 1;
				menuString[0] = Lang.MenuText[51];
				noFocus[0] = true;
				menuTop = 300;
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else if (Main.IsGameStarted && !Main.IsGamePaused)
				{
					if (Netplay.Session == null)
					{
						Exit();
						return;
					}
					int count = ((ReadOnlyCollection<NetworkGamer>)(object)Netplay.Session.AllGamers).Count;
					if (count < Player.MaxNumPlayers)
					{
						int privateGamerSlots = Netplay.Session.PrivateGamerSlots;
						if (privateGamerSlots > 0)
						{
							if (Main.NetMode == (byte)NetModeSetting.CLIENT)
							{
								NetMessage.CreateMessage0(66);
								NetMessage.SendMessage();
								privateSlots = (byte)privateGamerSlots;
								SetMenu(MenuMode.WAITING_FOR_PUBLIC_SLOT, rememberPrevious: false);
								return;
							}
							Netplay.Session.PrivateGamerSlots--;
						}
						try
						{
							Netplay.Session.AddLocalGamer(SignedInGamer);
							SetMenu(MenuMode.WAITING_FOR_PLAYER_ID, rememberPrevious: false);
						}
						catch
						{
							Exit();
							return;
						}
					}
					else
					{
						menuString[0] = Lang.WorldGenText[57];
					}
				}
			}
			else if (CurMenuMode == MenuMode.WAITING_FOR_PUBLIC_SLOT)
			{
				numMenuItems = 1;
				menuString[0] = Lang.MenuText[51];
				noFocus[0] = true;
				menuTop = 300;
				if (Netplay.Session.PrivateGamerSlots < privateSlots)
				{
					try
					{
						Netplay.Session.AddLocalGamer(SignedInGamer);
						SetMenu(MenuMode.WAITING_FOR_PLAYER_ID, rememberPrevious: false);
					}
					catch
					{
						Exit();
						return;
					}
				}
				else if (IsBackButtonTriggered())
				{
					NetMessage.CreateMessage0(67);
					NetMessage.SendMessage();
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.WAITING_FOR_PLAYER_ID)
			{
				numMenuItems = 1;
				menuString[0] = Lang.MenuText[51];
				noFocus[0] = true;
				menuTop = 300;
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.LEADERBOARDS)
			{
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else
				{
					Leaderboards.Update();
				}
			}
#if !USE_ORIGINAL_CODE
			else if (CurMenuMode == MenuMode.ACHIEVEMENTS)
			{
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else
				{
					Achievements.Update();
				}
			}
#endif
			else if (CurMenuMode == MenuMode.TITLE)
			{
				if (MainUI != this)
				{
					Exit();
					return;
				}
				menuTop = 172;
				menuSpace = 42;
				menuString[0] = Lang.MenuText[13];
				menuString[1] = Lang.MenuText[89];
				menuString[2] = Lang.MenuText[106];
				menuString[3] = Lang.MenuText[107];
				menuString[4] = Lang.MenuText[108];

#if !USE_ORIGINAL_CODE
				bool AlreadyOpen = false;
#endif

				if (!Main.IsTrial)
				{
					MENU_TITLE_COORDS[0].X = ViewportHalfWidth;
					menuHC[0] = 0;
				}
				else
				{
					MENU_TITLE_COORDS[0].X = 0;
					menuHC[0] = 3;
					if (uiY == 0)
					{
						uiY = 1;
					}
				}
				if (HasOnline())
				{
					MENU_TITLE_COORDS[2].X = ViewportHalfWidth;
					menuHC[2] = 0;
				}
				else
				{
					MENU_TITLE_COORDS[2].X = 0;
					menuHC[2] = 3;
					if (uiY == 2)
					{
						uiY = 3;
					}
				}
				if (!SignedInGamer.IsGuest && !Main.IsTrial)
				{
					MENU_TITLE_COORDS[3].X = ViewportHalfWidth;
					menuHC[3] = 0;
				}
				else
				{
					MENU_TITLE_COORDS[3].X = 0;
					menuHC[3] = 3;
					if (uiY == 3)
					{
						uiY = 4;
					}
				}
				if (Main.IsTrial)
				{
					menuString[5] = Lang.MenuText[109];
					if (SignedInGamer.Privileges.AllowPurchaseContent)
					{
						MENU_TITLE_COORDS[5].X = ViewportHalfWidth;
						menuHC[5] = 0;
					}
					else
					{
						MENU_TITLE_COORDS[5].X = 0;
						menuHC[5] = 3;
						if (uiY == 5)
						{
							uiY = 6;
						}
					}
					menuString[6] = Lang.MenuText[15];
					numMenuItems = 7;
					MENU_TITLE_COORDS[6].X = ViewportHalfWidth;
				}
				else
				{
					menuString[5] = Lang.MenuText[15];
					numMenuItems = 6;
					MENU_TITLE_COORDS[6].X = 0;
				}
				for (int num = numMenuItems - 1; num >= 0; num--)
				{
					menuScale[num] = 0.75f;
				}
				if (selectedMenu == 0)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.CHARACTER_SELECT);
					ResetPlayerMenuSelection();
				}
				else if (selectedMenu == 1)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.STATUS_SCREEN);
					Main.StartTutorial();
				}
				else if (selectedMenu == 2)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.LEADERBOARDS);
					Leaderboards.InitializeData();
				}
				else if (selectedMenu == 3)
				{
#if VERSION_103 || VERSION_FINAL
					Main.PlaySound(10);
#endif
					ShowAchievements();
				}
				else if (selectedMenu == 4)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.OPTIONS);
				}
				else if (selectedMenu == 5)
				{
					if (Main.IsTrial)
					{
						if (!Guide.IsVisible)
						{
#if USE_ORIGINAL_CODE
                            bool flag2;
							do
							{
								flag2 = false;
								try
								{
									GuideExtensions.ShowMarketplace(controller, MARKETPLACE_OFFER_ID);
								}
								catch (GuideAlreadyVisibleException)
								{
									Thread.Sleep(32);
									flag2 = true;
								}
							}
							while (flag2);
#else
							if (!AlreadyOpen) // Marketplace is getting shut down, so we'll open the game's website instead so one could make a legit purchase that supports the devs.
							{
								AlreadyOpen = true;
								Main.PlaySound(12);
								Process.Start("http://www.terraria.org"); // Only gonna open once per title menu visit.
							}
							else
							{
								Main.PlaySound(11);
							}
#endif
						}
					}
					else
					{
						SetMenu(MenuMode.QUIT);
					}
				}
				else if (selectedMenu == 6)
				{
					SetMenu(MenuMode.QUIT);
				}
				else if (IsButtonTriggered(Buttons.X))
				{
					if (playerStorage != null)
					{
						playerStorage.PromptForDevice();
					}
				}
				else if (IsButtonTriggered(Buttons.Y))
				{
					ShowParty();
				}
			}
			else if (CurMenuMode == MenuMode.PAUSE)
			{
				menuString[0] = Lang.MenuText[112];
				int num2 = 1;
				if (Main.IsTrial)
				{
					menuString[num2] = Lang.MenuText[109];
					if (SignedInGamer.Privileges.AllowPurchaseContent)
					{
						MENU_PAUSE_COORDS[num2].X = ViewportHalfWidth;
						menuHC[num2] = 0;
					}
					else
					{
						MENU_PAUSE_COORDS[num2].X = 0;
						menuHC[num2] = 3;
						if (uiY == num2)
						{
							uiY = 0;
						}
					}
					num2++;
					numMenuItems = 7;
					MENU_PAUSE_COORDS[6].X = ViewportHalfWidth;
				}
				else
				{
					numMenuItems = 6;
					MENU_PAUSE_COORDS[6].X = 0;
				}
				if (!HasOnline())
				{
					MENU_PAUSE_COORDS[num2 + 1].X = 0;
					menuHC[num2 + 1] = 3;
					if (uiY == num2 + 1)
					{
						uiY = 0;
					}
				}
				else
				{
					MENU_PAUSE_COORDS[num2 + 1].X = ViewportHalfWidth;
					menuHC[num2 + 1] = 0;
				}
				if (SignedInGamer.IsGuest || Main.IsTrial)
				{
					MENU_PAUSE_COORDS[num2 + 2].X = 0;
					menuHC[num2 + 2] = 3;
					if (uiY == num2 + 2)
					{
						uiY = 0;
					}
				}
				else
				{
					MENU_PAUSE_COORDS[num2 + 2].X = ViewportHalfWidth;
					menuHC[num2 + 2] = 0;
				}
				if (WorldGen.saveLock || Main.IsTutorial() || !HasPlayerStorage())
				{
					MENU_PAUSE_COORDS[num2 + 3].X = 0;
					menuHC[num2 + 3] = 3;
					if (uiY == num2 + 3)
					{
						uiY = 0;
					}
				}
				else
				{
					MENU_PAUSE_COORDS[num2 + 3].X = ViewportHalfWidth;
					menuHC[num2 + 3] = 0;
				}
				if (WorldGen.saveLock)
				{
					MENU_PAUSE_COORDS[num2 + 4].X = 0;
					menuHC[num2 + 4] = 3;
					if (uiY == num2 + 4)
					{
						uiY = 0;
					}
				}
				else
				{
					MENU_PAUSE_COORDS[num2 + 4].X = ViewportHalfWidth;
					menuHC[num2 + 4] = 0;
				}
				menuString[num2] = Lang.MenuText[108];
				menuString[num2 + 1] = Lang.MenuText[106];
				menuString[num2 + 2] = Lang.MenuText[107];
				menuString[num2 + 3] = Lang.MenuText[99];
				menuString[num2 + 4] = Lang.MenuText[101];
				for (int k = 0; k < numMenuItems; k++)
				{
					menuScale[k] = 0.75f;
				}
				menuTop = 200;
				menuSpace = 40;
				if (selectedMenu == 0 || IsButtonUntriggered(Buttons.Start) || IsBackButtonTriggered())
				{
					ResumeGame();
				}
				else if (Main.NetMode == (byte)NetModeSetting.CLIENT && IsButtonTriggered(Buttons.RightShoulder))
				{
					MessageBox.Show(controller, Lang.InterfaceText[72], Lang.InterfaceText[73], new string[2]
					{
						Lang.MenuText[105],
						Lang.MenuText[104]
					}, ShouldAutoUpdate: false);
					SetMenu(MenuMode.BLACKLIST);
				}
				else if (num2 > 1 && selectedMenu == 1)
				{
					if (!Guide.IsVisible)
					{
#if USE_ORIGINAL_CODE
                        bool flag3;
						do
						{
							flag3 = false;
							try
							{
								GuideExtensions.ShowMarketplace(controller, MARKETPLACE_OFFER_ID);
							}
							catch (GuideAlreadyVisibleException)
							{
								Thread.Sleep(32);
								flag3 = true;
							}
						}
						while (flag3);
#endif
					}
				}
				else if (selectedMenu == num2)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.OPTIONS);
				}
				else if (selectedMenu == num2 + 1)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.LEADERBOARDS);
					Leaderboards.InitializeData();
				}
				else if (selectedMenu == num2 + 2)
				{
#if VERSION_103 || VERSION_FINAL
					Main.PlaySound(10);
#endif
					ShowAchievements();
				}
				else if (selectedMenu == num2 + 3)
				{
					Main.PlaySound(10);
					WorldGen.saveAllWhilePlaying();
				}
				else if (selectedMenu == num2 + 4)
				{
					Main.PlaySound(10);
					Main.SaveOnExit = !Main.IsTutorial() && autoSave && IsStorageEnabledForAnyPlayer();
					if (!Main.SaveOnExit && !Main.IsTutorial())
					{
						MessageBox.Show(BoxOptions: (!IsStorageEnabledForAnyPlayer()) ? new string[2]
						{
							Lang.InterfaceText[0],
							Lang.InterfaceText[1]
						} : new string[3]
						{
							Lang.InterfaceText[0],
							Lang.InterfaceText[1],
							Lang.InterfaceText[2]
						}, Controller: controller, BoxCaption: Lang.MenuText[100], BoxContents: Lang.InterfaceText[5], ShouldAutoUpdate: false);
					}
					SetMenu(MenuMode.EXIT);
				}
			}
			else if (CurMenuMode == MenuMode.EXIT)
			{
				if (MessageBox.IsVisible())
				{
					if (!MessageBox.IsAutoUpdate() && MessageBox.Update())
					{
						int result = MessageBox.GetResult();
						if (result <= 0)
						{
							PrevMenu();
						}
						else
						{
							Main.SaveOnExit = result == 2;
						}
					}
				}
				else
				{
					ExitGame();
				}
			}
			else if (CurMenuMode == MenuMode.EXIT_UGC_BLOCKED)
			{
				if (MessageBox.IsVisible())
				{
					if (!MessageBox.IsAutoUpdate() && MessageBox.Update())
					{
						int result2 = MessageBox.GetResult();
						Main.SaveOnExit = result2 != 1;
					}
				}
				else
				{
					ExitGame();
				}
			}
			else if (CurMenuMode == MenuMode.BLACKLIST)
			{
				if (MessageBox.IsVisible() && !MessageBox.IsAutoUpdate() && MessageBox.Update())
				{
					int result3 = MessageBox.GetResult();
					if (result3 <= 0)
					{
						PrevMenu();
					}
					else
					{
						blacklist.Add(Main.GetWorldId());
						SaveSettings();
						ExitGame();
					}
				}
			}
			else if (CurMenuMode == MenuMode.BLACKLIST_REMOVE)
			{
				if (MessageBox.IsVisible() && !MessageBox.IsAutoUpdate() && MessageBox.Update())
				{
					int result4 = MessageBox.GetResult();
					if (result4 <= 0)
					{
						if (CurMenuType == MenuType.MAIN)
						{
							Exit();
							return;
						}
						ExitGame();
					}
					else
					{
						blacklist.Remove(Main.GetWorldId());
						SettingsDirty = true;
						PrevMenu();
						if (CurMenuType != 0)
						{
							CurMenuType = MenuType.NONE;
						}
						Main.CheckWorldId = true;
					}
				}
			}
			else if (CurMenuMode == MenuMode.MAP)
			{
				if (miniMap.IsThreadDone)
				{
					if (IsBackButtonTriggered())
					{
						miniMap.DestroyMap();
						ResumeGame();
					}
					else
					{
						miniMap.UpdateMap(this);
					}
				}
			}
			else if (CurMenuMode == MenuMode.CHARACTER_SELECT)
			{
#if VERSION_INITIAL
				menuLeft += 80;
				menuTop = 200;
				menuSpace = 40;
				for (int l = 0; l < MAX_LOAD_PLAYERS; l++)
				{
					if (l < numLoadPlayers)
					{
						menuString[l] = loadPlayer[l].CharacterName;
						menuHC[l] = loadPlayer[l].difficulty;
						MENU_SELECT_COORDS[l].X = ViewportHalfWidth;
					}
					else
					{
						menuString[l] = null;
						MENU_SELECT_COORDS[l].X = 0;
					}
				}
				if (numLoadPlayers == MAX_LOAD_PLAYERS)
				{
					blockFocus[5] = true;
					menuString[5] = null;
					MENU_SELECT_COORDS[5].X = 0;
				}
				else
				{
					menuString[5] = Lang.MenuText[16];
					MENU_SELECT_COORDS[5].X = ViewportHalfWidth;
				}
				numMenuItems = 6;
				for (int m = 0; m < 6; m++)
				{
					menuScale[m] = 0.8f;
				}
				if (IsBackButtonTriggered() && (Netplay.gamersWhoReceivedInvite.Count < 2 || !Netplay.gamersWhoReceivedInvite.Contains(this.SignedInGamer)))
				{
					CancelInvite(this.SignedInGamer);
					SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
				}
				else if (selectedMenu == 5)
				{
					Terraria.Main.PlaySound(10);
					loadPlayer[numLoadPlayers] = new Player();
					loadPlayer[numLoadPlayers].CharacterName = this.SignedInGamer.Gamertag;
					loadPlayer[numLoadPlayers].Inventory[0].SetDefaults("Copper Shortsword");
					loadPlayer[numLoadPlayers].Inventory[0].SetPrefix(-1);
					loadPlayer[numLoadPlayers].Inventory[1].SetDefaults("Copper Pickaxe");
					loadPlayer[numLoadPlayers].Inventory[1].SetPrefix(-1);
					loadPlayer[numLoadPlayers].Inventory[2].SetDefaults("Copper Axe");
					loadPlayer[numLoadPlayers].Inventory[2].SetPrefix(-1);
					CreateCharacterGUI.ApplyDefaultAttributes(loadPlayer[numLoadPlayers]);
					SetMenu(MenuMode.CREATE_CHARACTER);
				}
				else if (selectedMenu >= 0)
				{
					Terraria.Main.PlaySound(10);
					selectedPlayer = selectedMenu;
					SetPlayer(loadPlayer[selectedPlayer].DeepCopy());
					playerPathName = loadPlayerPath[selectedPlayer];
					if (Netplay.isJoiningRemoteInvite)
					{
						SetMenu(MenuMode.NETPLAY);
						statusText = Lang.MenuText[75];
					}
					else if (this != MainUI)
					{
						SetMenu(MenuMode.WAITING_SCREEN);
					}
					else
					{
						SetMenu(MenuMode.WORLD_SELECT);
					}
				}
				else if (focusMenu >= 0 && focusMenu < numLoadPlayers)
				{
					if (IsButtonTriggered(Buttons.X))
					{
						selectedPlayer = focusMenu;
						Terraria.Main.PlaySound(10);
						SetMenu(MenuMode.CONFIRM_DELETE_CHARACTER);
					}
					else
					{
						showPlayer = focusMenu;
					}
				}
#else
				CharacterSelect.Update(this);
#endif
			}
			else if (CurMenuMode == MenuMode.CREATE_CHARACTER)
			{
				Player player = loadPlayer[numLoadPlayers];
				CreateCharacterGUI.Update(player);
			}
			else if (CurMenuMode == MenuMode.CONFIRM_LEAVE_CREATE_CHARACTER)
			{
				menuString[0] = Lang.MenuText[49];
				noFocus[0] = true;
				menuString[1] = Lang.MenuText[104];
				menuString[2] = Lang.MenuText[105];
				numMenuItems = 3;
				if (selectedMenu == 1)
				{
					PrevMenu(-2);
				}
				else if (selectedMenu == 2 || IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.NAME_CHARACTER)
			{
				string characterName = loadPlayer[numLoadPlayers].CharacterName;
				string text = GetInputText(characterName, Lang.MenuText[53], Lang.MenuText[45], validate: false).UserText;
				numMenuItems = 0;
				if (inputTextEnter)
				{
					if (inputTextCanceled || text.Length == 0)
					{
						PrevMenu();
					}
					else
					{
						Main.PlaySound(10);
						if (text.Length > 16)
						{
							text = text.Substring(0, 16);
						}
						Player player2 = loadPlayer[numLoadPlayers];
						player2.CharacterName = text;
						player2.ui = this;
						loadPlayerPath[numLoadPlayers] = nextLoadPlayer();
						player2.Save(loadPlayerPath[numLoadPlayers]);
						PrevMenu(-2);
						selectedPlayer = numLoadPlayers;
						showPlayer = numLoadPlayers;
						uiY = numLoadPlayers;
						MENU_SELECT_COORDS[uiY].X = ViewportHalfWidth;
						numLoadPlayers++;
					}
				}
			}
			else if (CurMenuMode == MenuMode.CONFIRM_DELETE_CHARACTER)
			{
#if VERSION_INITIAL
				menuString[0] = Lang.MenuText[46] + loadPlayer[selectedPlayer].Name + "?";
#else
				menuString[0] = Lang.MenuText[46] + loadPlayer[selectedPlayer].CharacterName + "?";
#endif
				// Here lies a problem; As we can see, the above string is made by getting the text "Delete", appending the character name, and then adding a question mark.
				// Simple? No, since .name is a parameter that has the majority of one setting, the gamertag (or on PS3, the PSN name), and as such, very few references to the actual character name is seen in-game.
				// Instead, if we use .characterName or set the .name property to the .characterName, this will reference the character's name, rather than the player's. I will be preferring the former when we use it.

				noFocus[0] = true;
				menuString[1] = Lang.MenuText[104];
				menuString[2] = Lang.MenuText[105];
				numMenuItems = 3;
				if (selectedMenu == 2 || IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else if (selectedMenu == 1)
				{
					ErasePlayer(selectedPlayer);
					Main.PlaySound(10);
					PrevMenu();
					ResetPlayerMenuSelection();
				}
			}
			else if (CurMenuMode == MenuMode.WORLD_SELECT)
			{
				WorldSelect.Update();
			}
			else if (CurMenuMode == MenuMode.GAME_MODE)
			{
				GameMode.Update();
			}
			else if (CurMenuMode == MenuMode.NAME_WORLD)
			{
				string text2 = GetInputText(Lang.MenuText[56], Lang.MenuText[55], Lang.MenuText[48], validate: false).UserText;
				numMenuItems = 0;
				if (inputTextEnter)
				{
					if (inputTextCanceled || text2.Length == 0)
					{
						PrevMenu();
					}
					else
					{
						if (text2.Length > 20)
						{
							text2 = text2.Substring(0, 20);
						}
						SetMenu(MenuMode.STATUS_SCREEN, rememberPrevious: false);
						WorldSelect.CreateWorld(text2);
					}
				}
			}
			else if (CurMenuMode == MenuMode.CONFIRM_DELETE_WORLD)
			{
				menuString[0] = Lang.MenuText[46] + WorldSelect.WorldName() + '?';
				noFocus[0] = true;
				menuString[1] = Lang.MenuText[104];
				menuString[2] = Lang.MenuText[105];
				numMenuItems = 3;
				if (selectedMenu == 2 || IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else if (selectedMenu == 1)
				{
					WorldSelect.EraseWorld();
					Main.PlaySound(10);
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.OPTIONS)
			{

				menuTop = 220;
				menuSpace = 57;
				menuString[0] = Lang.MenuText[110];
				menuString[1] = Lang.MenuText[111];
				menuString[2] = Lang.MenuText[14];
				if (CurMenuType == MenuType.MAIN)
				{
					menuString[3] = Lang.MenuText[47];
					MENU_OPTIONS_COORDS[3].X = ViewportHalfWidth;
					numMenuItems = 4;
				}
				else
				{
					MENU_OPTIONS_COORDS[3].X = 0;
					if (uiY == 3)
					{
						uiY = 0;
					}
					numMenuItems = 3;
				}
				if (selectedMenu == 0)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.HOW_TO_PLAY);
				}
				else if (selectedMenu == 1)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.CONTROLS);
				}
				else if (selectedMenu == 2)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.SETTINGS);
				}
				else if (selectedMenu == 3)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.CREDITS);
				}
				// 1.2 adds a 5th entry in the title only options to the leaderboards??? Anyway, since the menu items never increase to 5, its not used.
				else if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.HOW_TO_PLAY)
			{
				howtoUI.Update();
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.SHOW_SIGN_IN)
			{
				if (!Guide.IsVisible)
				{
					try
					{
						Guide.ShowSignIn(1, this != MainUI && Main.NetMode != (byte)NetModeSetting.LOCAL);
						CurMenuMode = MenuMode.SIGN_IN;
                    }
					catch (GuideAlreadyVisibleException)
					{
					}
				}
			}
			else if (CurMenuMode == MenuMode.SIGN_IN)
			{
				if (!Guide.IsVisible)
				{
					if (SignedInGamer == null)
					{
						foreach (SignedInGamer signedInGamer2 in Gamer.SignedInGamers)
						{
							if (signedInGamer2.PlayerIndex == controller)
							{
                                SignedInGamer = signedInGamer2;
#if USE_ORIGINAL_CODE
								SignedInGamer.Presence.SetPresenceModeString("Menu");
#else
                                SignedInGamer.Presence.SetPresenceModeStringEXT("Menu");
#endif
                                InitPlayerStorage();
								break;
							}
						}
					}
					if (SignedInGamer != null)
					{
						if (!IsGamerValid())
						{
							MessageBox.Show(controller, Lang.MenuText[5], Lang.InterfaceText[43], new string[1]
							{
								Lang.MenuText[90]
							});
							CurMenuMode = MenuMode.SIGN_IN_FAILED;
						}
						else if (playerStorage == null || playerStorage.IsDone)
						{
							PrevMenu();
						}
					}
					else
					{
						CurMenuMode = MenuMode.SIGN_IN_FAILED;
					}
				}
			}
			else if (CurMenuMode == MenuMode.CONTROLS)
			{
				numMenuItems = 0;
				if (IsButtonTriggered(Buttons.A))
				{
					alternateGrappleControls = !alternateGrappleControls;
					UpdateAlternateGrappleControls();
					SettingsDirty = true;
				}
				else if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.SETTINGS)
			{
				menuSpace = 60;
				numMenuItems = 3;
#if VERSION_103 || VERSION_FINAL
				menuTop = 190;
				numMenuItems += 1;
#endif
				if (this == MainUI)
				{
					menuString[0] = Lang.MenuText[65];
					MENU_SETTINGS_COORDS[0].X = ViewportHalfWidth;
				}
				else
				{
					menuString[0] = "";
					MENU_SETTINGS_COORDS[0].X = 0;
					if (uiY == 0)
					{
						uiY = 1;
					}
				}
				if (autoSave)
				{
					menuString[1] = Lang.MenuText[67];
				}
				else
				{
					menuString[1] = Lang.MenuText[68];
				}
				if (HasPlayerStorage())
				{
					MENU_SETTINGS_COORDS[1].X = ViewportHalfWidth;
					menuHC[1] = 0;
				}
				else
				{
					MENU_SETTINGS_COORDS[1].X = 0;
					menuHC[1] = 3;
					if (uiY == 1)
					{
						uiY = 2;
					}
				}
				if (ShowItemText)
				{
					menuString[2] = Lang.MenuText[71];
				}
				else
				{
					menuString[2] = Lang.MenuText[72];
				}
				if (selectedMenu == 2)
				{
					Main.PlaySound(12);
					ShowItemText = !ShowItemText;
					SettingsDirty = true;
				}
				else if (selectedMenu == 1)
				{
					Main.PlaySound(12);
					autoSave = !autoSave;
					SettingsDirty = true;
				}
				else if (selectedMenu == 0)
				{
					Main.PlaySound(10);
					SetMenu(MenuMode.VOLUME);
				}
				else if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.VOLUME)
			{
				soundUI.Update();
				menuTop = 200;

#if !USE_ORIGINAL_CODE
				if (Main.ScreenHeightPtr == 2)
				{
					menuTop -= 10;
				}
#endif

				menuSpace = 60;
				numMenuItems = 1;
				menuString[0] = Lang.MenuText[65];
				noFocus[0] = true;
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.WORLD_SIZE)
			{
				menuTop = 190;
				menuSpace = 60;
				menuY[1] = 30;
				menuY[2] = 30;
				menuY[3] = 30;
				menuY[4] = 40;
				menuString[0] = Lang.MenuText[91];
				noFocus[0] = true;
				menuString[1] = Lang.MenuText[92];
				menuString[2] = Lang.MenuText[93];
				menuString[3] = Lang.MenuText[94];
				numMenuItems = 4;
				if (IsBackButtonTriggered())
				{
					PrevMenu();
				}
				else if (selectedMenu > 0)
				{
					Main.PlaySound(10); // This is moved below the if statements before the 1.0 patch for some reason
                    if (selectedMenu == 1)
					{
						Main.MaxTilesX = Main.SmallWorldW;
						Main.MaxTilesY = Main.SmallWorldH;
					}
					else if (selectedMenu == 2)
					{
						Main.MaxTilesX = Main.MediumWorldW;
						Main.MaxTilesY = Main.MediumWorldH;
					}
					else
					{
						Main.MaxTilesX = Main.LargeWorldW;
						Main.MaxTilesY = Main.LargeWorldH;
					}
					WorldGen.setWorldSize();
					ClearInput();
					SetMenu(MenuMode.NAME_WORLD);
				}
			}
			else if (CurMenuMode == MenuMode.QUIT)
			{
				if (!MessageBox.IsAutoUpdate() && MessageBox.Update())
				{
					int result5 = MessageBox.GetResult();
					if (result5 > 0)
					{
						Exit();
						return;
					}
					PrevMenu();
				}
			}
			else if (CurMenuMode == MenuMode.SIGN_IN_FAILED)
			{
				if (!MessageBox.IsVisible())
				{
					if (this == MainUI)
					{
						SetMenu(MenuMode.WELCOME, rememberPrevious: false, reset: true);
					}
					else
					{
						Exit();
					}
				}
			}
			else if (CurMenuMode == MenuMode.CREDITS)
			{
				numMenuItems = 0;
				Credits.Update();
			}
			else if (CurMenuMode == MenuMode.UPSELL)
			{
				numMenuItems = 0;
				if (IsBackButtonTriggered())
				{
					SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
				}
				else if (IsButtonTriggered(Buttons.A))
				{
					HasQuit = true;
				}
				else if (IsButtonTriggered(Buttons.X) && SignedInGamer.Privileges.AllowPurchaseContent && !Guide.IsVisible)
				{
#if USE_ORIGINAL_CODE
                    bool flag4;
					do
					{
						flag4 = false;
						try
						{
							GuideExtensions.ShowMarketplace(controller, MARKETPLACE_OFFER_ID);
						}
						catch (GuideAlreadyVisibleException)
						{
							Thread.Sleep(32);
							flag4 = true;
						}
					}
					while (flag4);
#endif
				}
			}

#if !USE_ORIGINAL_CODE
			if (Main.HardmodeAlert)
			{
				if (CurMenuMode == MenuMode.HARDMODE_UPSELL)
				{
					hardmodeUpsell.Update();
					if (IsBackButtonTriggered())
					{
						ResumeGame();
					}
				}
			}
#endif
			if (CurMenuMode != menuMode)
			{
				numMenuItems = 0;
				for (int n = 0; n < MAX_ITEMS; n++)
				{
					menuItemScale[n] = 0.8f;
				}
			}
			focusMenu = -1;
			selectedMenu = -1;
			for (int num3 = 0; num3 < numMenuItems; num3++)
			{
				if (menuString[num3] != null && MouseY > menuTop + menuSpace * num3 + menuY[num3] && MouseY < menuTop + menuSpace * num3 + menuY[num3] + 50f * menuScale[num3] && Main.HasFocus && !noFocus[num3] && !blockFocus[num3])
				{
					focusMenu = (sbyte)num3;
					if (oldMenu != focusMenu)
					{
						Main.PlaySound(12);
					}
					if ((CurMenuMode != MenuMode.PAUSE) ? IsSelectButtonTriggered() : IsButtonTriggered(Buttons.A))
					{
						selectedMenu = (sbyte)num3;
					}
				}
			}
			oldMenu = focusMenu;
		}

		private void DrawSaveIconMessage()
		{
#if USE_ORIGINAL_CODE
			DrawDialog(new Vector2(Main.ResolutionWidth - chatBackTexture.Width >> 1, 220f), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), saveIconMessage, Lang.MenuText[3]);
			SpriteSheet<_sheetSprites>.Draw(642, Main.ResolutionWidth / 2, 304, Color.White, (float)((double)Main.FrameCounter * (-Math.PI / 60.0)), 1f);
#else
			int ChatBackTexSize = 500;
			int IconY = 304;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.15f);
					IconY = 371;
					break;
				case 2:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.5f);
					IconY = 572;
					break;
			}

			DrawDialog(new Vector2(Main.ResolutionWidth - ChatBackTexSize >> 1, (float)(220f * Main.ScreenMultiplier)), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), saveIconMessage, Lang.MenuText[3]);
			float RotationSpeed = 60f;

#if VERSION_103 || VERSION_FINAL
			RotationSpeed += 60f; // -Math.PI / 60 becomes -Math.PI / 120 when the save icon switches to Plantera's spiked ball.
#endif
			SpriteSheet<_sheetSprites>.Draw(Main.SaveIconSprite, Main.ResolutionWidth / 2, IconY, Color.White, (float)(Main.FrameCounter * (-Math.PI / RotationSpeed)), 1f);
#endif
		}

		private void DrawMenu()
		{
#if !USE_ORIGINAL_CODE
			if (CurMenuMode != MenuMode.CONTROLS && CurMenuMode != MenuMode.HARDMODE_UPSELL && CurMenuMode != MenuMode.MAP && CurMenuMode != MenuMode.HOW_TO_PLAY && CurMenuMode != MenuMode.LEADERBOARDS && CurMenuMode != MenuMode.ACHIEVEMENTS && CurMenuMode != MenuMode.WORLD_SELECT && CurMenuMode != MenuMode.CREATE_CHARACTER && CurMenuMode != MenuMode.CREDITS && CurMenuMode != MenuMode.UPSELL)
#else
			if (CurMenuMode != MenuMode.CONTROLS && CurMenuMode != MenuMode.MAP && CurMenuMode != MenuMode.HOW_TO_PLAY && CurMenuMode != MenuMode.LEADERBOARDS && CurMenuMode != MenuMode.WORLD_SELECT && CurMenuMode != MenuMode.CREATE_CHARACTER && CurMenuMode != MenuMode.CREDITS && CurMenuMode != MenuMode.UPSELL)
#endif
			{
				DrawLogo();
			}
			if (saveIconMessageTime > 0)
			{
				if (!Guide.IsVisible)
				{
					saveIconMessageTime--;
					if (IsButtonTriggered(Buttons.B))
					{
						saveIconMessageTime = 0;
					}
					DrawSaveIconMessage();
					DrawControls();
				}
				return;
			}

#if !USE_ORIGINAL_CODE
			int ChatBackTexSize = 500;
			float PosY = 460f;
			int WrapWidth = 470;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.15f);
					PosY = 600f;
					WrapWidth = 548;
					break;
				case 2:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.534f);
					PosY = 960f;
					WrapWidth = 705;
					break;
			}
#endif

			if (CurMenuMode == MenuMode.STATUS_SCREEN || CurMenuMode == MenuMode.NETPLAY)
			{
				int num = menuTop + 100;
				float num2 = Progress * numProgressStepsInv + progressTotal;
#if USE_ORIGINAL_CODE
				if (num2 > 0f)
				{
					Rectangle value = default(Rectangle);
					value.Height = progressBarTexture.Height >> 1;
					value.Width = (int)((float)progressBarTexture.Width * num2);
					Vector2 vector = default(Vector2);
					vector.X = CurrentView.ViewWidth - progressBarTexture.Width >> 1;
					vector.Y = (float)(num);
					Main.SpriteBatch.Draw(progressBarTexture, vector, (Rectangle?)value, Color.White);
					value.X = value.Width;
					value.Y = value.Height;
					vector.X += value.Width;
					value.Width = progressBarTexture.Width - value.Width;
					Main.SpriteBatch.Draw(progressBarTexture, vector, (Rectangle?)value, Color.White);
				}
#else
				// The original version's method is to draw the black and progress at the same time and change which segment of both is being drawn depending on the progress.
				// As such, I'll be using this alternative due to both game mode's (540p and 1080p) handling being convoluted and inefficient. This just draws the black and the progress over it.
				Rectangle value = default;
				value.Height = progressBarTexture.Height >> 1;
				value.Width = (int)((float)progressBarTexture.Width);
				value.Y = value.Height;
				Vector2 vector = default;
				vector.X = CurrentView.ViewWidth - (int)(progressBarTexture.Width * Main.ScreenMultiplier) >> 1;
				vector.Y = num * Main.ScreenMultiplier;
				Main.SpriteBatch.Draw(progressBarTexture, vector, (Rectangle?)value, Color.White, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);

				if (num2 > 0f)
				{
					value.Y = 0;
					value.Width = (int)(progressBarTexture.Width * num2);
					Main.SpriteBatch.Draw(progressBarTexture, vector, (Rectangle?)value, Color.White, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);
				}
#endif
				CurrentView.SetScreenViewWideCentered();
				tips.Draw();
				CurrentView.SetScreenView();
			}
			else if (CurMenuMode == MenuMode.CONTROLS)
			{
				int num3 = CurrentView.ViewWidth >> 1;
				Vector2 position = default;
				position.X = num3 - (controlsTexture.Width >> 1); // Position could easily be set to 0,0 for version 1.0 since the controller graphic is 960x540, but instead they used this which works across all versions.
				position.Y = Main.ResolutionHeight - controlsTexture.Height >> 1; // This makes me wonder if they already had plans on updating everything to be more efficient from the beginning.
				Main.SpriteBatch.Draw(controlsTexture, position, Color.White);
				int num4 = (int)MeasureString(SmallFont, Lang.InterfaceText[24]).X + 60;
				num3 = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetRight - num4;
#if USE_ORIGINAL_CODE
				int num5 = 464 - CurrentView.SafeAreaOffsetBottom;
#else
				float Offset = CurrentView.SafeAreaOffsetBottom;
				switch (Main.ScreenHeightPtr)
				{
					case 1:
						Offset = -7.39f;
						break;

					case 2:
						Offset = -32;
						break;
				}
				int num5 = (int)((464 * Main.ScreenMultiplier) - Offset);
#endif
				CurrentView.Ui.DrawInventoryCursor(num3, num5, 1.0);
				if (alternateGrappleControls)
				{
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHECK, num3 + 10, num5 + 10, Color.White);
				}
				DrawStringLC(SmallFont, Lang.InterfaceText[24], num3 + 60, num5 + 26, Color.White);
				//------------------------------	Mockup of how 1.2.4's extra controls need to look like.
				/*
				num5 += 32 * 2;
				view.ui.DrawInventoryCursor(num3, num5, 1.0);
				if (alternateGrappleControls)
				{
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHECK, num3 + 10, num5 + 10, Color.White);
				}
				DrawStringLC(SmallFont, "Alternate controller layout", num3 + 60, num5 + 26, Color.White);

				num5 += 32 * 2;
				view.ui.DrawInventoryCursor(num3, num5, 1.0);
				if (alternateGrappleControls)
				{
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHECK, num3 + 10, num5 + 10, Color.White);
				}
				DrawStringLC(SmallFont, "Use Quick Shortcuts", num3 + 60, num5 + 26, Color.White);
				*/
				//------------------------------


				ControlDesc[] array = Lang.Controls();
				for (int num6 = array.Length - 1; num6 >= 0; num6--)
				{
					int alignment = array[num6].Alignment;
					int num7 = array[num6].X;
					if (CurrentView.ViewWidth > Main.ResolutionWidth)
					{
						num7 += Main.ResolutionWidth / 2;
					}
					int num8 = array[num6].Y;
					string text = array[num6].Label;
					switch (num6)
					{
					case 0:
						if (alternateGrappleControls)
						{
							text = array[9].Label;
						}
						break;
#if !USE_ORIGINAL_CODE
					case 4:
						if (Main.PSMode)
						{
							text = array[5].Label;
							text = ((!alternateGrappleControls) ? (text + array[9].Label) : (text + array[0].Label));
						}
						break;

					case 5:
						if (Main.PSMode)
						{
							text = array[4].Label;
						}
						else
						{
							text = ((!alternateGrappleControls) ? (text + array[9].Label) : (text + array[0].Label));
						}
						break;
#else
					case 5:
						text = ((!alternateGrappleControls) ? (text + array[9].Label) : (text + array[0].Label));
						break;
#endif
					}
					Vector2 vector2 = MeasureString(BoldSmallFont, text);
					if (alignment < 2)
					{
						num7 -= (int)vector2.X >> 1;
						if (alignment == 0)
						{
							num8 -= (int)vector2.Y;
						}
					}
					else
					{
						if (alignment == 3)
						{
							num7 -= (int)vector2.X;
						}
						num8 -= (int)vector2.Y >> 1;
					}
					DrawStringLT(BoldSmallFont, text, num7, num8, Color.White);
				}
			}
			else if (CurMenuMode == MenuMode.MAP)
			{
				if (Netplay.Session != null)
				{
					DrawMiniMap();
				}
			}
			else if (CurMenuMode == MenuMode.HOW_TO_PLAY)
			{
				CurrentView.SetScreenViewWideCentered();
				howtoUI.Draw();
				CurrentView.SetScreenView();
			}
#if !VERSION_INITIAL
			else if (CurMenuMode == MenuMode.CHARACTER_SELECT)
			{
				CharacterSelect.Draw(CurrentView);
			}
#endif
			else if (CurMenuMode == MenuMode.WORLD_SELECT)
			{
				WorldSelect.Draw(CurrentView);
			}
			else if (CurMenuMode == MenuMode.GAME_MODE)
			{
				GameMode.Draw(CurrentView);
			}
			else if (CurMenuMode == MenuMode.CREATE_CHARACTER)
			{
				CurrentView.SetScreenViewWideCentered();
				CreateCharacterGUI.Draw(CurrentView);
				CurrentView.SetScreenView();
				showPlayer = -1;
			}
			else if (CurMenuMode == MenuMode.VOLUME)
			{
				soundUI.Draw(Main.SpriteBatch);
			}
			else if (CurMenuMode == MenuMode.WELCOME)
			{
				string text2 = Lang.MenuText[52];
				if (text2 == null)
				{
					text2 = "";
				}
				SpriteFont spriteFont = BigFont;
				Vector2 value2 = spriteFont.MeasureString(text2);
				Vector2 origin = value2 * 0.5f;
#if USE_ORIGINAL_CODE
				Vector2 position2 = new Vector2(Main.ResolutionWidth / 2, 460f);
#else
				Vector2 position2 = new Vector2(Main.ResolutionWidth / 2, PosY);
#endif
				float num9 = 0.75f;
				num9 *= 1f + cursorAlpha * 0.1f;
				Color color = new Color(CursorColour.A, CursorColour.A, 100, 255);
				Main.SpriteBatch.DrawString(spriteFont, text2, position2, color, 0f, origin, num9, SpriteEffects.None, 0f);
			}
			else if (CurMenuMode == MenuMode.LEADERBOARDS)
			{
				CurrentView.SetScreenViewWideCentered();
				Leaderboards.Draw(CurrentView);
				CurrentView.SetScreenView();
			}
#if !USE_ORIGINAL_CODE
			else if (CurMenuMode == MenuMode.ACHIEVEMENTS)
			{
				CurrentView.SetScreenViewWideCentered();
				Achievements.Draw(CurrentView);
				CurrentView.SetScreenView();
			}
#endif
			else if (CurMenuMode == MenuMode.ERROR)
			{
#if USE_ORIGINAL_CODE
				if (errorCompiledText == null)
				{
					errorCompiledText = new CompiledText(errorDescription, 470, BoldSmallTextStyle);
				}
				DrawDialog(new Vector2(CurrentView.ViewWidth - chatBackTexture.Width >> 1, 260f), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), errorCompiledText, errorCaption);
#else
				float VectorY = (float)(260 * Main.ScreenMultiplier);
				if (errorCompiledText == null)
				{
					errorCompiledText = new CompiledText(errorDescription, WrapWidth, BoldSmallTextStyle);
				}
				DrawDialog(new Vector2(CurrentView.ViewWidth - ChatBackTexSize >> 1, VectorY), new Color(128, 128, 128, 64), new Color(255, 255, 255, 255), errorCompiledText, errorCaption);
#endif
			}
			else if (CurMenuMode == MenuMode.CREDITS)
			{
				Credits.Draw();
			}
			else if (CurMenuMode == MenuMode.UPSELL)
			{
				theGame.DrawUpsell();
			}
#if !USE_ORIGINAL_CODE
			if (Main.HardmodeAlert)
			{
				if (CurMenuMode == MenuMode.HARDMODE_UPSELL)
				{
					CurrentView.SetScreenViewWideCentered();
					hardmodeUpsell.Draw();
					CurrentView.SetScreenView();
				}
			}
#endif
			for (int i = 0; i < numMenuItems; i++)
			{
				if (menuString[i] != null)
				{
#if !VERSION_INITIAL // Not how 1.1+ does it, as that adds a separate game-only options menu, retaining original size. At least it looks that way... anyway I'm putting this here since it does the same thing.
					if (CurMenuMode == MenuMode.OPTIONS && CurMenuType == MenuType.MAIN)
					{
						menuTop = 200;
						menuSpace = 50;
						menuScale[i] = 0.78f;
					}
#endif
					float num10 = menuItemScale[i];
					Color c;
					if (menuHC[i] == 3)
					{
						c = new Color(120, 120, 120);
					}
					else if (menuHC[i] == 2) // Hardcore Purple
					{
						c = new Color(hcColorR, hcColorG, hcColorB);
					}
					else if (menuHC[i] == 1) // Mediumcore Violet
					{
						c = new Color(mcColorR, mcColorG, mcColorB);
					}
					else if (noFocus[i])
					{
						c = new Color(255, 200, 62, 255);
					}
					else if (i != focusMenu)
					{
						c = new Color(240, 240, 240, 240);
					}
					else
					{
						num10 *= 1f + cursorAlpha * 0.1f;
						c = new Color(CursorColour.A, CursorColour.A, 100, 255);
					}
					Vector2 pivot = MeasureString(BigFont, menuString[i]);
					pivot.X *= 0.5f;
					pivot.Y *= 0.5f;
					num10 *= menuScale[i];
#if USE_ORIGINAL_CODE
					DrawStringScaled(pos: new Vector2(menuLeft, (float)(menuTop + menuSpace * i) + pivot.Y * menuScale[i] + (float)menuY[i]), font: BigFont, s: menuString[i], c: c, pivot: pivot, scale: num10);
#else
					DrawStringScaled(pos: new Vector2(menuLeft, (float)((menuTop * Main.ScreenMultiplier) + (menuSpace * Main.ScreenMultiplier) * i) + pivot.Y * menuScale[i] + menuY[i]), font: BigFont, s: menuString[i], c: c, pivot: pivot, scale: num10);
#endif
				}
			}
			for (int j = 0; j < MAX_ITEMS; j++)
			{
				if (j == focusMenu)
				{
					if (menuItemScale[j] < 1f)
					{
						menuItemScale[j] += 0.05f;
					}
					if (menuItemScale[j] > 1f)
					{
						menuItemScale[j] = 1f;
					}
				}
				else if (menuItemScale[j] > 0.8)
				{
					menuItemScale[j] -= 0.05f;
				}
			}
			if (showPlayer >= 0)
			{
				Player player = loadPlayer[showPlayer];
				player.velocity.X = 1f;
				player.PlayerFrame();
#if USE_ORIGINAL_CODE
				DrawPlayer(player, new Vector2(148f, 244f), 4f);
#else
				float VectorX = (float)(148f * Main.ScreenMultiplier);
				float VectorY = (float)(244f * Main.ScreenMultiplier);
				float PlayerScale = (float)(4f * Main.ScreenMultiplier);
				DrawPlayer(player, new Vector2(VectorX, VectorY), PlayerScale);
#endif
			}
			DrawControls();
		}

		private void ResumeGame()
		{
			Main.PlaySound(10);
			PrevMenu();
			ClearButtonTriggers();
			CurMenuType = MenuType.NONE;
			worldFadeTarget = 1f;
		}

		public void DrawPlayer(Player player, Vector2 position, float scale)
		{
			Main.SpriteBatch.End();
			Vector2 position2 = player.Position;
			player.Position.X = CurrentView.ScreenPosition.X;
			player.Position.Y = CurrentView.ScreenPosition.Y;
			Matrix world = Matrix.CreateScale(scale, scale, 1f);
			world.M41 = position.X;
			world.M42 = position.Y;
			CurrentView.ScreenProjection.World = world;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, CurrentView.ScreenProjection);
			player.Draw(CurrentView, IsMenuScr: true);
			Main.SpriteBatch.End();
			CurrentView.ScreenProjection.World = Matrix.Identity;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, CurrentView.ScreenProjection);
			player.Position = position2;
			player.XYWH.X = (int)position2.X;
			player.XYWH.Y = (int)position2.Y;
		}

		public void DrawPlayerIcon(Player player, Vector2 position, float scale)
		{
			Vector2 position2 = player.Position;
			player.Position.X = CurrentView.ScreenPosition.X;
			player.Position.Y = CurrentView.ScreenPosition.Y;
			Vector2 velocity = player.velocity;
			player.velocity.X = 0f;
			player.velocity.Y = 0f;
			Matrix world = Matrix.CreateScale(scale, scale, 1f);
			world.M41 = position.X;
			world.M42 = position.Y;
			CurrentView.ScreenProjection.World = world;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, CurrentView.ScreenProjection);
			player.Draw(CurrentView, IsMenuScr: true, IsIcon: true);
			Main.SpriteBatch.End();
			CurrentView.ScreenProjection.World = Matrix.Identity;
			player.velocity = velocity;
			player.Position = position2;
			player.XYWH.X = (int)position2.X;
			player.XYWH.Y = (int)position2.Y;
		}

		private bool IsGamerValid()
		{
			if (this == MainUI && SignedInGamer.IsGuest)
			{
				return false;
			}
			if (Main.NetMode > 0 && !CanPlayOnline())
			{
				return false;
			}
			return true;
		}

		private void ShowSignInPortal()
		{
			foreach (SignedInGamer signedInGamer2 in Gamer.SignedInGamers)
			{
				if (signedInGamer2.PlayerIndex == controller)
				{
					SignedInGamer = signedInGamer2;
					if (IsGamerValid())
					{
#if USE_ORIGINAL_CODE
						signedInGamer2.Presence.SetPresenceModeString("Menu");
#else
                        signedInGamer2.Presence.SetPresenceModeStringEXT("Menu");
#endif
                        InitPlayerStorage();
						return;
					}
					SignedInGamer = null;
				}
			}
			SetMenu(MenuMode.SHOW_SIGN_IN);
		}

		private void ShowAchievements()
		{
			if (Guide.IsVisible)
			{
				return;
			}
			Main.PlaySound(10);
#if USE_ORIGINAL_CODE
            bool flag;
			do
			{
				flag = false;
				try
				{
					GuideExtensions.ShowAchievements(controller);
				}
				catch (GuideAlreadyVisibleException)
				{
					Thread.Sleep(32);
					flag = true;
				}
			}
			while (flag);
#else
			SetMenu(MenuMode.ACHIEVEMENTS);
			AchievementSystem.LoadAchievements();
			AchievementData.SetAchievements();
			Achievements.InitializeData();
#endif
		}

        public void ShowParty()
		{
			if (!CanPlayOnline() || Guide.IsVisible)
			{
				return;
			}
			Main.PlaySound(10);
			bool flag;
			do
			{
				flag = false;
				try
				{
					Guide.ShowPartySessions(controller);
				}
				catch (GuideAlreadyVisibleException)
				{
					Thread.Sleep(32);
					flag = true;
				}
			}
			while (flag);
		}

		private void DrawLogo()
		{
			logoRotation += logoRotationSpeed * 3E-05f;
			if (logoRotation > 0.1)
			{
				logoRotationDirection = -1f;
			}
			else if (logoRotation < -0.1)
			{
				logoRotationDirection = 1f;
			}
			if ((logoRotationSpeed < 20f) & (logoRotationDirection == 1f))
			{
				logoRotationSpeed += 1f;
			}
			else if ((logoRotationSpeed > -20f) & (logoRotationDirection == -1f))
			{
				logoRotationSpeed -= 1f;
			}
			logoScale += logoScaleSpeed * 1E-05f;
			if (logoScale > 1.1)
			{
				logoScaleDirection = -1f;
			}
			else if (logoScale < 0.9)
			{
				logoScaleDirection = 1f;
			}
			if ((logoScaleSpeed < 50f) & (logoScaleDirection == 1f))
			{
				logoScaleSpeed += 1f;
			}
			else if ((logoScaleSpeed > -50f) & (logoScaleDirection == -1f))
			{
				logoScaleSpeed -= 1f;
			}
			Color color = new Color(LogoA, LogoA, LogoA, LogoA);
			Color color2 = new Color(LogoB, LogoB, LogoB, LogoB);
			float x = ((CurrentView != null) ? (CurrentView.ViewWidth >> 1) : Main.ResolutionWidth / 2);
#if !USE_ORIGINAL_CODE
			float ScaleMultiplier = 1f;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ScaleMultiplier = 1.15f;
					break;
				case 2:
					ScaleMultiplier = 1.5f;
					break;
			}

			Main.SpriteBatch.Draw(logoTexture, new Vector2(x, (float)(100f * Main.ScreenMultiplier)), (Rectangle?)new Rectangle(0, 0, logoTexture.Width, logoTexture.Height), color, logoRotation, new Vector2(logoTexture.Width >> 1, logoTexture.Height >> 1), (float)(logoScale * ScaleMultiplier), SpriteEffects.None, 0f);
			Main.SpriteBatch.Draw(logo2Texture, new Vector2(x, (float)(100f * Main.ScreenMultiplier)), (Rectangle?)new Rectangle(0, 0, logoTexture.Width, logoTexture.Height), color2, logoRotation, new Vector2(logoTexture.Width >> 1, logoTexture.Height >> 1), (float)(logoScale * ScaleMultiplier), SpriteEffects.None, 0f);
#else
			Main.SpriteBatch.Draw(logoTexture, new Vector2(x, 100f), (Rectangle?)new Rectangle(0, 0, logoTexture.Width, logoTexture.Height), color, logoRotation, new Vector2(logoTexture.Width >> 1, logoTexture.Height >> 1), logoScale, SpriteEffects.None, 0f);
			Main.SpriteBatch.Draw(logo2Texture, new Vector2(x, 100f), (Rectangle?)new Rectangle(0, 0, logoTexture.Width, logoTexture.Height), color2, logoRotation, new Vector2(logoTexture.Width >> 1, logoTexture.Height >> 1), logoScale, SpriteEffects.None, 0f);
#endif
			if (CurrentView.WorldTime.DayTime)
			{
				LogoA += 2;
				if (LogoA > 255)
				{
					LogoA = 255;
				}
				LogoB--;
				if (LogoB < 0)
				{
					LogoB = 0;
				}
			}
			else
			{
				LogoB += 2;
				if (LogoB > 255)
				{
					LogoB = 255;
				}
				LogoA--;
				if (LogoA < 0)
				{
					LogoA = 0;
				}
			}
		}

		private void initCharacterSelectCoordinates()
		{
			if (numLoadPlayers == 0)
			{
				return;
			}
			for (int i = 0; i < MAX_LOAD_PLAYERS; i++)
			{
				if (i < numLoadPlayers)
				{
					MENU_SELECT_COORDS[i].X = ViewportHalfWidth;
				}
				else
				{
					MENU_SELECT_COORDS[i].X = 0;
				}
			}
		}

		private void DrawControls()
		{
			if (CurMenuType == MenuType.NONE)
			{
				return;
			}
			Main.StrBuilder.Length = 0;
			if (saveIconMessageTime > 0)
			{
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
			}
			else
			{
				switch (CurMenuMode)
				{
				case MenuMode.WAITING_SCREEN:
				case MenuMode.NETPLAY:
				case MenuMode.ERROR:
				case MenuMode.LOAD_FAILED_NO_BACKUP:
				case MenuMode.CREDITS:
#if !USE_ORIGINAL_CODE
				case MenuMode.ACHIEVEMENTS:
#endif
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					break;
				case MenuMode.MAP:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.MOVE_MAP));
						Main.StrBuilder.Append(' ');
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.ZOOM));
						Main.StrBuilder.Append(' ');
					if (mapScreenCursorY >= 2)
					{
						if (CanViewGamerCard() && Netplay.Session != null)
						{
							GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
							int num = mapScreenCursorY - 2;
							if (num < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count)
							{
									Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_GAMERCARD));
									Main.StrBuilder.Append(' ');
							}
						}
					}
					else if (mapScreenCursorX == 0)
					{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.TOGGLE_PVP));
							Main.StrBuilder.Append(' ');
					}
					else
					{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT_TEAM));
							Main.StrBuilder.Append(' ');
					}
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					if (Netplay.gamer != null && Main.NetMode > 0)
					{
						if (CanCommunicate())
						{
								Main.StrBuilder.Append(' ');
								Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.INVITE_PLAYER));
						}
						if (SignedInGamer.PartySize > 1)
						{
								Main.StrBuilder.Append(' ');
								Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.INVITE_PARTY));
						}
					}
					break;
				case MenuMode.TITLE:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
					if (playerStorage != null)
					{
							Main.StrBuilder.Append(' ');
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_STORAGE));
					}
					if (CanPlayOnline())
					{
							Main.StrBuilder.Append(' ');
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_PARTY));
					}
					break;
				case MenuMode.CREATE_CHARACTER:
					CreateCharacterGUI.ControlDescription(Main.StrBuilder);
					break;
				case MenuMode.VOLUME:
					soundUI.ControlDescription(Main.StrBuilder);
					break;
				case MenuMode.HOW_TO_PLAY:
					howtoUI.ControlDescription(Main.StrBuilder);
					break;
				case MenuMode.LEADERBOARDS:
					Leaderboards.ControlDescription(Main.StrBuilder);
					break;
				case MenuMode.CHARACTER_SELECT:
#if VERSION_INITIAL
					Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
					if (Netplay.gamersWhoReceivedInvite.Count < 2 || !Netplay.gamersWhoReceivedInvite.Contains(SignedInGamer))
					{
						Terraria.Main.StrBuilder.Append(' ');
						Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					}
					if (focusMenu < numLoadPlayers)
					{
						Terraria.Main.StrBuilder.Append(' ');
						Terraria.Main.StrBuilder.Append(Lang.MenuText[17]); // Delete
					}
#else
					CharacterSelect.ControlDescription(Main.StrBuilder);
#endif
					break;
				case MenuMode.WORLD_SELECT:
					WorldSelect.ControlDescription(Main.StrBuilder);
					break;
				case MenuMode.UPSELL:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.EXIT));
						Main.StrBuilder.Append(' ');
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK_TO_GAME));
					if (SignedInGamer.Privileges.AllowPurchaseContent)
					{
							Main.StrBuilder.Append(' ');
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.UNLOCK_FULL_GAME));
					}
					break;
				case MenuMode.CONTROLS:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.TOGGLE_GRAPPLE_MODE));
						Main.StrBuilder.Append(' ');
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					break;
				case MenuMode.PAUSE:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
						Main.StrBuilder.Append(' ');
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					if (Main.NetMode == (byte)NetModeSetting.CLIENT)
					{
							Main.StrBuilder.Append(' ');
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BLACKLIST));
					}
					break;
				default:
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
						Main.StrBuilder.Append(' ');
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
					break;
				case MenuMode.WELCOME:
				case MenuMode.STATUS_SCREEN:
					break;
				}
#if !USE_ORIGINAL_CODE
				if (Main.HardmodeAlert)
				{
					if (CurMenuMode == MenuMode.HARDMODE_UPSELL)
					{
						Main.StrBuilder.Length = 0;
						hardmodeUpsell.ControlDescription(Main.StrBuilder);
					}
				}
#endif
			}

#if !USE_ORIGINAL_CODE
			if (CurMenuMode == MenuMode.WELCOME) // ADDITION: To make use of the version string provided and to assist with identifying built versions, this code will draw a version string.
			{
				string VersionString = "Version: ";

				// All version strings all start with X360, which is due to TerrariaOGC being based off of that version, with the PS3, HD, and version changes being added from what I have decompiled.

				switch (Main.VersionNumber) // Yeah, the IDE thinks it unnecessary, but whenever the user changes the version being built, it will adapt on-the-fly.
				{
					case "Xbox360 v0.7.6":
						VersionString += "X360 1.0 (Initial 1.1.2)";
						break;

					case "Xbox360 v0.7.8":
						VersionString += "X360 1.0 (Patched 1.1.2)";
						break;

					case "Xbox360 v1.01": // This and below are entirely custom, since versions past Patched 1.0 no longer included the version number string, indicating it was removed entirely.
						VersionString += "X360 1.01 (1.1.2 Plus)";
						break;

					case "Xbox360 v1.03":
						VersionString += "X360 1.03 (1.2.1.2)";
						break;

					case "Xbox360 v1.09":
						VersionString += "X360 1.09 (Final 1.2.4.1)";
						break;

					default:
						VersionString += "Custom (Custom)";
						break;
				}

				Vector2 StringVector = BoldSmallFont.MeasureString(VersionString);
				int StringX = Main.ResolutionWidth - CurrentView.SafeAreaOffsetLeft;
				int StringY = CurrentView.SafeAreaOffsetBottom;
				Main.SpriteBatch.DrawString(BoldSmallFont, VersionString, new Vector2(StringX - StringVector.X, Main.ResolutionHeight - StringY), Color.White, 0f, new Vector2(0f, StringVector.Y), (NumActiveViews <= 1) ? 1 : 2, SpriteEffects.None, 0f);
			}
#endif

			if (Main.StrBuilder.Length > 0)
			{
				DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
			}
		}

		private void DrawHud()
		{
			Vector2 pos = default(Vector2);
			Color c = new Color(128, 128, 128, 128);
			for (int i = 1; i < ActivePlayer.StatLifeMax / 20 + 1; i++)
			{
				int num = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetRight - 460 + 26 * (i - 1) + 160 + 11;
				int num2 = CurrentView.SafeAreaOffsetTop;
				if (i > 10)
				{
					num -= 260;
					num2 += 26;
				}
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.HEART_EMPTY, num, num2, c);
				int id = (int)_sheetSprites.ID.HEART;
				if (ActivePlayer.statLife < i * 20)
				{
					float num3 = (ActivePlayer.statLife - (i - 1) * 20) / 20f;
					if (num3 <= 0f)
					{
						continue;
					}
					id = (((double)num3 < 0.25) ? (int)_sheetSprites.ID.HEART3 : ((!((double)num3 < 0.5)) ? (int)_sheetSprites.ID.HEART1 : (int)_sheetSprites.ID.HEART2));
				}
				SpriteSheet<_sheetSprites>.Draw(id, num, num2);
			}
			if (ActivePlayer.breath < Player.MaxBreath)
			{
				for (int j = 1; j < 11; j++)
				{
					int num4 = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetRight - 460 + 26 * (j - 1) + 160 + 11;
					int num5 = CurrentView.SafeAreaOffsetTop + 52;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.BUBBLE_EMPTY, num4, num5, c);
					if (ActivePlayer.breath < j * 20)
					{
						float num6 = (ActivePlayer.breath - (j - 1) * 20) / 20f;
						if (num6 > 0f)
						{
							float scaleCenter = num6 * 0.25f + 0.75f;
							int num7 = (int)(30f + 225f * num6);
							if (num7 < 30)
							{
								num7 = 30;
							}
							c.R = (byte)num7;
							c.G = (byte)num7;
							c.B = (byte)num7;
							c.A = (byte)num7;
							SpriteSheet<_sheetSprites>.DrawScaled((int)_sheetSprites.ID.BUBBLE, num4 + 11, num5 + 11, scaleCenter, c);
							c = new Color(128, 128, 128, 128);
						}
					}
					else
					{
						SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.BUBBLE, num4, num5);
					}
				}
			}
			if (ActivePlayer.statManaMax2 > 0)
			{
				int num8 = ActivePlayer.statManaMax2 / 20;
				int num9 = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetRight - 22;
				pos.X = num9 + 11;
				c.R = byte.MaxValue;
				c.G = byte.MaxValue;
				c.B = byte.MaxValue;
				c.A = 229;
				int num10 = 0;
				do
				{
					bool flag = false;
					float num11 = 1f;
					if (ActivePlayer.statMana >= (num10 + 1) * 20)
					{
						if (ActivePlayer.statMana == (num10 + 1) * 20)
						{
							flag = true;
						}
					}
					else
					{
						float num12 = (ActivePlayer.statMana - num10 * 20) / 20f;
						int num13 = (int)(30f + 225f * num12);
						if (num13 < 30)
						{
							num13 = 30;
						}
						c.R = (byte)num13;
						c.G = (byte)num13;
						c.B = (byte)num13;
						c.A = (byte)(num13 * 0.9);
						num11 = num12 * 0.25f + 0.5f;
						if (num11 < 0.5f)
						{
							num11 = 0.5f;
						}
						if (num12 > 0f)
						{
							flag = true;
						}
					}
					if (flag)
					{
						num11 += cursorScale - 1f;
					}
					int num14 = CurrentView.SafeAreaOffsetTop + 23 * num10;
					pos.Y = num14 + 11 + (6f - 6f * num11);
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.MANA_EMPTY, num9, num14, new Color(128, 128, 128, 128));
					SpriteSheet<_sheetSprites>.DrawScaled((int)_sheetSprites.ID.MANA, ref pos, c, num11);
				}
				while (++num10 < num8);
			}
			c = new Color(99, 99, 99, 99);
			for (int k = 0; k < Player.MaxNumBuffs; k++)
			{
				int type = ActivePlayer.buff[k].Type;
				if (type <= 0)
				{
					continue;
				}
#if USE_ORIGINAL_CODE
				int x = 32 + CurrentView.SafeAreaOffsetLeft + k * 38;
				int num15 = 76 + CurrentView.SafeAreaOffsetTop;
				int num16 = 141 + type;
				if (type == (int)Buff.ID.PET)
				{
					num16 += ActivePlayer.pet;
				}
				SpriteSheet<_sheetSprites>.Draw(num16, x, num15, c);
#else
				int x = (int)(32 * Main.ScreenMultiplier) + CurrentView.SafeAreaOffsetLeft + k * (int)(38 * Main.ScreenMultiplier);
				int num15 = (int)(76 * Main.ScreenMultiplier) + CurrentView.SafeAreaOffsetTop;
				int num16 = (int)_sheetSprites.ID.BUFF_1 - 1 + type;
				if (type == (int)Buff.ID.PET)
				{
					num16 += ActivePlayer.pet;
				}
				if (Main.ScreenHeightPtr == 0)
				{
					SpriteSheet<_sheetSprites>.Draw(num16, x, num15, c);
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawScaled(num16, x, num15, Main.ScreenMultiplier, c);
				}
#endif
				if (type != (int)Buff.ID.WEREWOLF && type != (int)Buff.ID.MERFOLK && type != (int)Buff.ID.HORRIFIED && type != (int)Buff.ID.TONGUED && type != (int)Buff.ID.PET)
				{
					int num17 = ActivePlayer.buff[k].Time / 60;
					Main.StrBuilder.Length = 0;
					Main.StrBuilder.Append((num17 / 60).ToStringLookup());
					Main.StrBuilder.Append(':');
					num17 %= 60;
					if (num17 < 10)
					{
						Main.StrBuilder.Append('0');
					}
					Main.StrBuilder.Append(num17.ToStringLookup());
#if !USE_ORIGINAL_CODE
					switch (Main.ScreenHeightPtr)
					{
						case 1:
							x -= 24;
							num15 -= 10;
							break;

						case 2:
							x -= 38;
							num15 += 4;
							break;
					}
#endif
					DrawStringLT(ItemStackFont, x, num15 + 32, Color.White);
				}
			}
			if (npcChatText == null)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				for (int l = 0; l < 3; l++)
				{
					Main.StrBuilder.Length = 0;
					if (ActivePlayer.accCompass && !flag4)
					{
						Main.StrBuilder.Append(Lang.MenuText[95]);
						int num18 = (ActivePlayer.XYWH.X + 10 >> 3) - Main.MaxTilesX;
						num18 >>= 2;
						if (num18 > 0)
						{
							Main.StrBuilder.Append(num18.ToStringLookup());
							Main.StrBuilder.Append(Lang.MenuText[96]);
						}
						else if (num18 < 0)
						{
							num18 = -num18;
							Main.StrBuilder.Append(num18.ToStringLookup());
							Main.StrBuilder.Append(Lang.MenuText[97]);
						}
						else
						{
							Main.StrBuilder.Append(Lang.MenuText[98]);
						}
						flag4 = true;
					}
					else if (ActivePlayer.accDepthMeter && !flag3)
					{
						Main.StrBuilder.Append(Lang.MenuText[85]);
						int num19 = (ActivePlayer.XYWH.Y + 42 >> 3) - Main.WorldSurface * 2;
						num19 >>= 2;
						if (num19 > 0)
						{
							Main.StrBuilder.Append(num19.ToStringLookup());
							Main.StrBuilder.Append(Lang.MenuText[86]);
						}
						else if (num19 < 0)
						{
							num19 = -num19;
							Main.StrBuilder.Append(num19.ToStringLookup());
							Main.StrBuilder.Append(Lang.MenuText[87]);
						}
						else
						{
							Main.StrBuilder.Append(Lang.MenuText[88]);
						}
						flag3 = true;
					}
					else if (ActivePlayer.accWatch > 0 && !flag2)
					{
						string value = " AM";
						double num20 = Main.GameTime.WorldTime;
						if (!Main.GameTime.DayTime)
						{
							num20 += Time.dayLength;
						}
						num20 = num20 / 86400.0 * 24.0;
						num20 = num20 - 7.5 - 12.0;
						if (num20 < 0.0)
						{
							num20 += 24.0;
						}
						if (num20 >= 12.0)
						{
							value = " PM";
						}
						int num21 = (int)num20;
						int num22 = (int)((num20 - num21) * 60.0);
						if (num21 > 12)
						{
							num21 -= 12;
						}
						if (num21 == 0)
						{
							num21 = 12;
						}
						string text;
						if (ActivePlayer.accWatch == 1)
						{
							text = "00";
						}
						else if (ActivePlayer.accWatch == 2)
						{
							text = ((num22 >= 30) ? "30" : "00");
						}
						else
						{
							text = num22.ToStringLookup();
							if (num22 < 10)
							{
								text = "0" + text;
							}
						}
						Main.StrBuilder.Append(Lang.InterfaceText[34]);
						Main.StrBuilder.Append(num21.ToStringLookup());
						Main.StrBuilder.Append(':');
						Main.StrBuilder.Append(text);
						Main.StrBuilder.Append(value);
						flag2 = true;
					}
					if (Main.StrBuilder.Length > 0)
					{
#if USE_ORIGINAL_CODE
						DrawStringLT(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, 110 + 22 * l + 48, mouseTextColor);
#else
						int Multiplier = 22;
						switch (Main.ScreenHeightPtr)
						{
							case 1:
								Multiplier = 25;
								break;

							case 2: 
								Multiplier = 33;
								break;
						}
						DrawStringLT(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, (int)((110 * Main.ScreenMultiplier) + Multiplier * l + (int)(48 * Main.ScreenMultiplier)), mouseTextColor);
#endif
					}
				}
			}
			int oldSelectedItem = ActivePlayer.oldSelectedItem;
			int num23 = ((oldSelectedItem >= 0) ? oldSelectedItem : ActivePlayer.SelectedItem);
#if USE_ORIGINAL_CODE
			float num24 = ((NumActiveViews > 1) ? 1.25f : 1f); 
#else
			float MP = 1.25f;
			float SP = 1f;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					MP = 1.40625f;
					SP = 1.15f;
					break;

				case 2:
					MP = 1.875f;
					SP = 1.534f;
					break;
			}
			float num24 = ((NumActiveViews > 1) ? MP : SP);
#endif
			int num25 = CurrentView.SafeAreaOffsetLeft;
			for (int m = 0; m < 10; m++)
			{
				float num26 = hotbarScale[m];
				if (m == num23)
				{
					if (num26 < 1f)
					{
						num26 += 0.05f;
						hotbarScale[m] = num26;
					}
				}
				else if (num26 > 0.75f)
				{
					num26 -= 0.05f;
					hotbarScale[m] = num26;
				}
				num26 *= num24;
				int y = (int)(CurrentView.SafeAreaOffsetTop + 22f * (1f - num26));
				int num27 = (int)(65f + 180f * num26);
				Color itemColor = new Color(num27, num27, num27, num27);
				if (m == num23)
				{
					c.R = 200;
					c.G = 200;
					c.B = 200;
					c.A = 200;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, num25, y, c, num26);
				}
				else
				{
					c.R = 100;
					c.G = 100;
					c.B = 100;
					c.A = 100;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK, num25, y, c, num26);
				}
				int num28 = ((m == oldSelectedItem) ? ActivePlayer.SelectedItem : m);
				if (ActivePlayer.Inventory[num28].Type > 0 && ActivePlayer.Inventory[num28].Stack > 0)
				{
					inventoryScale = num26;
					DrawInventoryItem(ref ActivePlayer.Inventory[num28], num25, y, itemColor, StackType.HOTBAR);
				}
				num25 += (int)(52f * num26) + 4;
			}
			if (quickAccessDisplayTime > 0)
			{
				quickAccessDisplayTime--;
				int alpha = ((quickAccessDisplayTime < 64) ? (quickAccessDisplayTime << 2) : 255);
#if USE_ORIGINAL_CODE
				DrawQuickAccess(ActivePlayer.SelectedItem, CurrentView.SafeAreaOffsetLeft, Main.ResolutionHeight - CurrentView.SafeAreaOffsetBottom - 32 - 128, alpha, StackType.HOTBAR);
#else
				DrawQuickAccess(ActivePlayer.SelectedItem, CurrentView.SafeAreaOffsetLeft, Main.ResolutionHeight - CurrentView.SafeAreaOffsetBottom - (int)(160 * Main.ScreenMultiplier), alpha, StackType.HOTBAR);
#endif
			}
			if (hotbarItemNameTime > 0)
			{
				hotbarItemNameTime--;
				if (ActivePlayer.Inventory[ActivePlayer.SelectedItem].Type != 0)
				{
					string s = ActivePlayer.Inventory[ActivePlayer.SelectedItem].AffixName();
					int num29 = ((hotbarItemNameTime < 64) ? (hotbarItemNameTime << 2) : 255);
					DrawStringCT(SmallFont, s, (int)(216f * num24) + CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetTop + (int)(44f * num24), new Color(num29, num29, num29, num29));
				}
			}
			DrawControlsIngame();
		}

		private void DrawInventoryItem(int itemTexId, int x, int y, Color itemColor)
		{
			int width = SpriteSheet<_sheetSprites>.Source[itemTexId].Width;
			int height = SpriteSheet<_sheetSprites>.Source[itemTexId].Height;
			float num = ((width <= height) ? (41f / height) : (41f / width));

#if USE_ORIGINAL_CODE
			num *= inventoryScale;
			if (num > 1.25f)
			{
				num = 1.25f;
			}
#else
#if VERSION_INITIAL
			if (Main.ScreenHeightPtr == 0)
			{
				num *= inventoryScale; // BUG: In the initial and patched 1.0 versions, items were incorrectly scaled due to limiting post-scale, which you can see if you get a mushroom in your hotbar and compare between 1.0 and 1.01.
			}                               // Moving the *= to after the 1.25f limit fixes this, leading to items being more uniform with their scaled appearance in the inventory, hotbar, etc.
			if (num > 1.25f)
			{
				num = 1.25f;
			}
			if (Main.ScreenHeightPtr != 0) // Having the bugged scaling was only available on versions playable in 540p, and it does not look good enough on 720p/1080p so I'm using the fixed method for those sizes...
			{
				num *= inventoryScale; // ..otherwise you end up with really small inventory sprites.
			}
#else
			if (num > 1.25f)
			{
				num = 1.25f;
			}
			num *= inventoryScale;
#endif
#endif
			Vector2 pos = default;
			pos.X = x + 26f * inventoryScale;
			pos.Y = y + 26f * inventoryScale;
			SpriteSheet<_sheetSprites>.DrawScaled(itemTexId, ref pos, itemColor, num);
		}

		private void DrawInventoryItem(ref Item item, int x, int y, Color itemColor, StackType stackType = StackType.NONE)
		{
			int num = item.Type + (int)_sheetSprites.ID.ITEM_1 - 1;
			int width = SpriteSheet<_sheetSprites>.Source[num].Width;
			int height = SpriteSheet<_sheetSprites>.Source[num].Height;
			float num2 = ((width <= height) ? (41f / height) : (41f / width));

#if USE_ORIGINAL_CODE
			num2 *= inventoryScale;
			if (num2 > 1.25f)
			{
				num2 = 1.25f;
			}
#else
#if VERSION_INITIAL
			if (Terraria.Main.ScreenHeightPtr == 0)
			{
				num2 *= inventoryScale;
			}
			if (num2 > 1.25f)
			{
				num2 = 1.25f;
			}
			if (Terraria.Main.ScreenHeightPtr != 0)
			{
				num2 *= inventoryScale;
			}
#else
			if (num2 > 1.25f)
			{
				num2 = 1.25f;
			}
			num2 *= inventoryScale;
#endif
#endif

			Vector2 pos = default(Vector2);
			pos.X = x + 26f * inventoryScale;
			pos.Y = y + 26f * inventoryScale;
			SpriteSheet<_sheetSprites>.DrawScaled(num, ref pos, item.GetAlphaInventory(itemColor), num2);
			if (item.Colour.PackedValue != 0)
			{
				SpriteSheet<_sheetSprites>.DrawScaled(num, ref pos, item.GetColor(itemColor), num2);
			}
			switch (stackType)
			{
			case StackType.INGREDIENT:
				DrawIngredientStack(ref item, x, y, itemColor);
				return;
			case StackType.NONE:
				return;
			}
			DrawInventoryItemStack(ref item, x, y, ref itemColor);
			if (stackType != StackType.HOTBAR)
			{
				return;
			}
			int useAmmo = item.UseAmmo;

#if USE_ORIGINAL_CODE
			if (useAmmo > 0)
			{
				int num3 = 0;
				for (int i = 0; i < ActivePlayer.Inventory.Length - 1; i++)
				{
					if (ActivePlayer.Inventory[i].Ammo == useAmmo)
					{
						num3 += ActivePlayer.Inventory[i].Stack;
					}
				}
				DrawStringScaled(ItemStackFont, num3.ToStringLookup(), new Vector2((float)(x + FONT_STACK_EXTRA_OFFSET) + 10f * inventoryScale, (float)y + 26f * inventoryScale), itemColor, default(Vector2), inventoryScale + 0.1f);
			}
			else if (item.Type == (int)Item.ID.WRENCH)
			{
				int num4 = 0;
				for (int j = 0; j < ActivePlayer.Inventory.Length - 1; j++)
				{
					if (ActivePlayer.Inventory[j].Type == (int)Item.ID.WIRE)
					{
						num4 += ActivePlayer.Inventory[j].Stack;
					}
				}
				DrawStringScaled(ItemStackFont, num4.ToStringLookup(), new Vector2((float)(x + FONT_STACK_EXTRA_OFFSET) + 10f * inventoryScale, (float)y + 26f * inventoryScale), itemColor, default(Vector2), inventoryScale + 0.1f);
			}
#else
			float StringScale = inventoryScale;
			if (Main.ScreenHeightPtr == 2)
			{
				StringScale *= 0.5f;
				StringScale -= 0.1f;
				y += 7;
			}

			if (useAmmo > 0)
			{
				int num3 = 0;
				for (int i = 0; i < ActivePlayer.Inventory.Length - 1; i++)
				{
					if (ActivePlayer.Inventory[i].Ammo == useAmmo)
					{
						num3 += ActivePlayer.Inventory[i].Stack;
					}
				}
				DrawStringScaled(ItemStackFont, num3.ToStringLookup(), new Vector2(x + FONT_STACK_EXTRA_OFFSET + 10f * inventoryScale, y + 26f * inventoryScale), itemColor, default(Vector2), StringScale + 0.1f);
			}
			else if (item.Type == (int)Item.ID.WRENCH)
			{
				int num4 = 0;
				for (int j = 0; j < ActivePlayer.Inventory.Length - 1; j++)
				{
					if (ActivePlayer.Inventory[j].Type == (int)Item.ID.WIRE)
					{
						num4 += ActivePlayer.Inventory[j].Stack;
					}
				}
				DrawStringScaled(ItemStackFont, num4.ToStringLookup(), new Vector2(x + FONT_STACK_EXTRA_OFFSET + 10f * inventoryScale, y + 26f * inventoryScale), itemColor, default(Vector2), StringScale + 0.1f);
			}
#endif
			if (item.IsPotion)
			{
				Color alphaInventory = item.GetAlphaInventory(itemColor);
				float num5 = ActivePlayer.potionDelay / (float)(int)ActivePlayer.potionDelayTime;
				alphaInventory *= num5;
				pos.X = x + 26f * inventoryScale;
				pos.Y = y + 26f * inventoryScale;
				SpriteSheet<_sheetSprites>.DrawScaled((int)_sheetSprites.ID.COOLDOWN, ref pos, alphaInventory, inventoryScale);
			}
		}

		private static void DrawInventoryItemStack(ref Item item, int x, int y, ref Color itemColor)
		{
			if (item.Stack > 1)
			{
#if USE_ORIGINAL_CODE
				DrawStringScaled(ItemStackFont, item.Stack.ToStringLookup(), new Vector2((float)(x + FONT_STACK_EXTRA_OFFSET) + 10f * inventoryScale, (float)y + 26f * inventoryScale), itemColor, default(Vector2), inventoryScale + 0.1f);
#else
				float StringScale = inventoryScale;
				if (Main.ScreenHeightPtr == 2)
				{
					StringScale *= 0.5f;
					StringScale -= 0.1f;
					y += 7;
				}
				DrawStringScaled(ItemStackFont, item.Stack.ToStringLookup(), new Vector2(x + FONT_STACK_EXTRA_OFFSET + 10f * inventoryScale, y + 26f * inventoryScale), itemColor, default(Vector2), StringScale + 0.1f);
#endif
			}
		}

		private void DrawIngredientStack(ref Item item, int x, int y, Color itemColor)
		{
			int num = Math.Min(item.Stack, ActivePlayer.CountInventory(item.NetID));
			Main.StrBuilder.Length = 0;
			Main.StrBuilder.Append(item.Stack.ToStringLookup());
			if (num < item.Stack)
			{
				itemColor.G >>= 1;
				itemColor.B >>= 1;
			}

#if USE_ORIGINAL_CODE
			DrawStringScaled(ItemStackFont, new Vector2((float)(x + FONT_STACK_EXTRA_OFFSET) + 10f * inventoryScale, (float)y + 26f * inventoryScale), itemColor, default(Vector2), inventoryScale - 0.1f);
#else
			float StringScale = inventoryScale;
			if (Main.ScreenHeightPtr == 2)
			{
				StringScale *= 0.5f;
				y += 7;
			}
			DrawStringScaled(ItemStackFont, new Vector2(x + FONT_STACK_EXTRA_OFFSET + 10f * inventoryScale, y + 26f * inventoryScale), itemColor, default(Vector2), StringScale - 0.1f);
#endif
		}

		public void DrawInterface()
		{
			if (showNPCs)
			{
				CurrentView.DrawNPCHouse();
			}
			Vector2 pos = default(Vector2);
			if (ActivePlayer.rulerAcc)
			{
				CurrentView.DrawGrid();
			}
			if (signBubble)
			{
				signBubble = false;
				int num = signX - CurrentView.ScreenPosition.X;
				int num2 = signY - CurrentView.ScreenPosition.Y;
				SpriteEffects se = SpriteEffects.None;
				if (signX > ActivePlayer.Position.X + 20f)
				{
					se = SpriteEffects.FlipHorizontally;
					num -= 40;
				}
				else
				{
					num += 8;
				}
				num2 -= 22;
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHAT2, num, num2, mouseTextColor, se);
			}
			Main.SpriteBatch.End();
			for (int num3 = Player.MaxNumPlayers - 1; num3 >= 0; num3--)
			{
				if (MyPlayer != num3)
				{
					Player player = Main.PlayerSet[num3];
					if (player.Active != 0 && !player.IsDead && !player.XYWH.Intersects(CurrentView.ViewArea) && (ActivePlayer.team == 0 || player.team == 0 || ActivePlayer.team == player.team))
					{
						Vector2 a = default(Vector2);
						Vector2 a2 = default(Vector2);
						a.X = CurrentView.ViewWidth >> 1;
						a.Y = Main.ResolutionHeight / 2;
						a2.X = player.XYWH.X + (Player.width / 2) - CurrentView.ScreenPosition.X;
						a2.Y = player.XYWH.Y + (Player.height / 2) - CurrentView.ScreenPosition.Y;
						Vector2 intersection = default(Vector2);
						bool flag = false;
						int sAFE_AREA_OFFSET_L = CurrentView.SafeAreaOffsetLeft;
						int num4 = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetRight - 40;
						int num5 = CurrentView.SafeAreaOffsetTop + 20 + 40;
						int num6 = Main.ResolutionHeight - CurrentView.SafeAreaOffsetBottom - 40;
						if (a2.X <= sAFE_AREA_OFFSET_L)
						{
							flag = Collision.LineIntersection(ref a, ref a2, new Vector2(sAFE_AREA_OFFSET_L, num5), new Vector2(sAFE_AREA_OFFSET_L, num6), ref intersection);
						}
						else if (a2.X >= num4)
						{
							flag = Collision.LineIntersection(ref a, ref a2, new Vector2(num4, num5), new Vector2(num4, num6), ref intersection);
						}
						if (!flag)
						{
							if (a2.Y <= num5)
							{
								flag = Collision.LineIntersection(ref a, ref a2, new Vector2(sAFE_AREA_OFFSET_L, num5), new Vector2(num4, num5), ref intersection);
							}
							else if (a2.Y >= num6)
							{
								flag = Collision.LineIntersection(ref a, ref a2, new Vector2(sAFE_AREA_OFFSET_L, num6), new Vector2(num4, num6), ref intersection);
							}
						}
						float num7 = CurrentView.ViewWidth * 1.5f / (a2 - a).Length();
						if (num7 < 0.5f)
						{
							num7 = 0.5f;
						}
						Matrix matrix = Matrix.CreateTranslation(-10f, -8f, 0f) * Matrix.CreateScale(num7, num7, 1f);
						Vector2 position = player.Position;
						player.Position.X = CurrentView.ScreenPosition.X;
						player.Position.Y = CurrentView.ScreenPosition.Y;
						CurrentView.ScreenProjection.World = matrix * Matrix.CreateTranslation(intersection.X, intersection.Y, 0f);
						Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, CurrentView.ScreenProjection);
						player.Draw(CurrentView, IsMenuScr: true, IsIcon: true);
						Main.SpriteBatch.End();
						player.Position = position;
						player.XYWH.X = (int)position.X;
						player.XYWH.Y = (int)position.Y;
					}
				}
			}
			CurrentView.ScreenProjection.World = Matrix.Identity;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, CurrentView.ScreenProjection);
#if (!IS_PATCHED && VERSION_INITIAL)
			if (InventoryMode == 0 && (npcChatText != null || this.ActivePlayer.sign != -1))
			{
				DrawNpcChat();
			}
#endif
			if (ActivePlayer.IsDead)
			{
				CloseInventory();
				string s = Lang.InterfaceText[38];
				DrawStringCC(BigFont, s, CurrentView.ViewWidth >> 1, Main.ResolutionHeight / 2, ActivePlayer.GetDeathAlpha(default(Color)));
				return;
			}
			if (InventoryMode != 0)
			{
				DrawInventoryMenu();
				return;
			}
			CurrentView.SetWorldView();
			Rectangle rectangle = new Rectangle(ActivePlayer.XYWH.X + 10 - 80, ActivePlayer.XYWH.Y + 21 - 64, 160, 128);
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active != 0 && MyPlayer != i && !Main.PlayerSet[i].IsDead)
				{
					Rectangle value = new Rectangle(Main.PlayerSet[i].XYWH.X + -6, Main.PlayerSet[i].XYWH.Y + 42 - 48, 32, 48);
					if (rectangle.Intersects(value))
					{
						Main.PlayerSet[i].DrawInfo(CurrentView);
					}
				}
			}
			if (!ActivePlayer.IsDead)
			{
				for (int num8 = NPC.MaxNumNPCs - 1; num8 >= 0; num8--)
				{
					CurrentView.drawNpcName[num8] = true;
				}
				ActivePlayer.npcChatBubble = -1;
				int num9 = 10496;
				Rectangle value2 = default(Rectangle);
				Point center = rectangle.Center;
				for (int j = 0; j < NPC.MaxNumNPCs; j++)
				{
					NPC nPC = Main.NPCSet[j];
					if (nPC.Active == 0)
					{
						continue;
					}
					int type = nPC.Type;
					if (type == (int)NPC.ID.MIMIC && Main.NPCSet[j].AI0 == 0f)
					{
						continue;
					}
					if ((type >= (int)NPC.ID.WYVERN_HEAD && type <= (int)NPC.ID.WYVERN_TAIL) || (type >= (int)NPC.ID.ARCH_WYVERN_HEAD && type <= (int)NPC.ID.ARCH_WYVERN_TAIL))
					{
						value2.X = nPC.XYWH.X + (nPC.Width >> 1) - 32;
						value2.Y = nPC.XYWH.Y + (nPC.Height >> 1) - 32;
						value2.Width = 64;
						value2.Height = 64;
					}
					else
					{
						int width = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.NPC_1 - 1 + type].Width;
						value2.X = nPC.XYWH.X + (nPC.Width >> 1) - (width >> 1);
						value2.Y = nPC.XYWH.Y + nPC.Height - nPC.FrameHeight;
						value2.Width = width;
						value2.Height = nPC.FrameHeight;
					}
					rectangle.Intersects(ref value2, out var result);
					if (result)
					{
						if (nPC.CanTalk() && ActivePlayer.CanInteractWithNPC())
						{
							Point value3 = value2.Center;
							int num10 = Math.Abs(center.X - value3.X);
							if (num10 <= 80)
							{
								int num11 = num10 * num10;
								num10 = Math.Abs(center.Y - value3.Y);
								if (num10 <= 64)
								{
									num11 += num10 * num10;
									if (num11 < num9)
									{
										bool result2;
										if (UsingSmartCursor)
										{
											result2 = true;
										}
										else
										{
											value3.X = MouseX + CurrentView.ScreenPosition.X;
											value3.Y = MouseY + CurrentView.ScreenPosition.Y;
											value2.Contains(ref value3, out result2);
										}
										if (result2)
										{
											ActivePlayer.npcChatBubble = (short)j;
										}
									}
								}
							}
						}
						if (nPC.DrawMyName < 32)
						{
							nPC.DrawMyName = 32;
						}
					}
					if (nPC.DrawMyName > 0 && CurrentView.ClipArea.Intersects(value2))
					{
						nPC.DrawMyName--;
						nPC.DrawInfo(CurrentView);
					}
				}
				if (ActivePlayer.npcChatBubble >= 0)
				{
					NPC nPC2 = Main.NPCSet[ActivePlayer.npcChatBubble];
					int num12 = -((nPC2.Width >> 1) + 8);
					SpriteEffects se2 = SpriteEffects.None;
					if (nPC2.SpriteDirection == -1)
					{
						se2 = SpriteEffects.FlipHorizontally;
						num12 = -num12;
					}
					pos.X = nPC2.XYWH.X + (nPC2.Width >> 1) - CurrentView.ScreenPosition.X - 16 - num12;
					pos.Y = nPC2.XYWH.Y - 26 - CurrentView.ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHAT, ref pos, mouseTextColor, se2);
				}
			}
			CurrentView.SetScreenView();
#if (!VERSION_INITIAL || IS_PATCHED)
			if (npcChatText != null || ActivePlayer.sign != -1)
			{
				DrawNpcChat();
			}
#endif
		}

		public void ClearInput()
		{
			inputTextEnter = false;
			inputTextCanceled = false;
		}

		public UserString GetInputText(UserString oldString, string title = null, string desc = null, bool validate = true)
		{
			if (!inputTextEnter && oldString.IsEditable())
			{
				if (!Guide.IsVisible)
				{
					try
					{
						kbResult = Guide.BeginShowKeyboardInput(controller, title, desc, oldString.IsCensored ? "" : oldString.UserText, null, null);
#if USE_ORIGINAL_CODE
						return oldString;
#endif
					}
					catch (GuideAlreadyVisibleException)
					{
						return oldString;
					}
				}
				if (kbResult != null && kbResult.IsCompleted)
				{
					inputTextEnter = true;
					string text = Guide.EndShowKeyboardInput(kbResult);
					kbResult = null;
					inputTextCanceled = text == null;
					if (!inputTextCanceled)
					{
						text = text.Trim();
						char[] array = text.ToCharArray();
						bool flag = false;
						for (int num = array.Length - 1; num >= 0; num--)
						{
							if (array[num] == '¤')
							{
								array[num] = 'Ʃ';
								flag = true;
							}
							else if (!BoldSmallFont.Characters.Contains(array[num]))
							{
								char c = array[num];
								if (c == '€')
								{
									array[num] = 'Ɛ';
								}
								else
								{
									array[num] = '?';
								}
								flag = true;
							}
						}
						if (flag)
						{
							text = new string(array);
						}
						if (validate)
						{
							oldString.SetUserString(text);
						}
						else
						{
							oldString.SetSystemString(text);
						}
					}
				}
			}
			return oldString;
		}

		public void FirstProgressStep(int numSteps, string text = null)
		{
			Progress = 0f;
			progressTotal = 0f;
			numProgressStepsInv = 1f / numSteps;
			if (text != null)
			{
				statusText = text;
			}
		}

		public void NextProgressStep(string text = null)
		{
			Progress = 0f;
			progressTotal += numProgressStepsInv;
			if (text != null)
			{
				statusText = text;
			}
		}

		private static void UpdateCursorColor()
		{
			cursorAlpha = (float)(0.8 + 0.2 * Math.Sin(Main.FrameCounter * 0.0625));
			double num = cursorAlpha * 0.3 + 0.7;
			byte r = (byte)(mouseColor.R * cursorAlpha);
			byte g = (byte)(mouseColor.G * cursorAlpha);
			byte b = (byte)(mouseColor.B * cursorAlpha);
			byte a = (byte)(255.0 * num);
			CursorColour = new Color(r, g, b, a);
			cursorScale = (float)(num + 0.1);
		}

		private void UpdateMouse()
		{
			if (uiCoords == null)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 3;
#if VERSION_INITIAL
				bool flag = InventoryMode > 0 || CurMenuMode == MenuMode.MAP || CurMenuMode == MenuMode.WORLD_SELECT || CurMenuMode == MenuMode.GAME_MODE;
#else
				bool flag = InventoryMode > 0 || CurMenuMode == MenuMode.MAP || CurMenuMode == MenuMode.CHARACTER_SELECT || CurMenuMode == MenuMode.WORLD_SELECT || CurMenuMode == MenuMode.GAME_MODE;
#endif
				if (flag)
				{
					num3 = 1;
				}
				else
				{
					if (CurMenuType == MenuType.NONE)
					{
						if (IsButtonTriggered(BTN_CURSOR_MODE))
						{
							UsingSmartCursor = !UsingSmartCursor;
						}
						if (UsingSmartCursor)
						{
							ActivePlayer.UpdateMouseSmart();
						}
						else
						{
							ActivePlayer.UpdateMouse();
						}
						return;
					}
					uiDelay = 0;
					uiDelayValue = UI_DELAY;
				}
				if (CurMenuMode != MenuMode.MAP)
				{
					if (PadState.ThumbSticks.Right.X < -GpDeadZone)
					{
						num = -num3;
					}
					else if (PadState.ThumbSticks.Right.X > GpDeadZone)
					{
						num = num3;
					}
					if (PadState.ThumbSticks.Right.Y < -GpDeadZone)
					{
						num2 = num3;
					}
					else if (PadState.ThumbSticks.Right.Y > GpDeadZone)
					{
						num2 = -num3;
					}
				}
				if (CurMenuType != MenuType.NONE || InventoryMode > 0)
				{
					if (PadState.ThumbSticks.Left.X < -GpDeadZone)
					{
						num = -num3;
					}
					else if (PadState.ThumbSticks.Left.X > GpDeadZone)
					{
						num = num3;
					}
					if (PadState.ThumbSticks.Left.Y < -GpDeadZone)
					{
						num2 = num3;
					}
					else if (PadState.ThumbSticks.Left.Y > GpDeadZone)
					{
						num2 = -num3;
					}
					if (InventoryMode == 0 || ActiveInvSection != InventorySection.ITEMS)
					{
						if (PadState.DPad.Left == ButtonState.Pressed)
						{
							num = -num3;
						}
						else if (PadState.DPad.Right == ButtonState.Pressed)
						{
							num = num3;
						}
						if (PadState.DPad.Down == ButtonState.Pressed)
						{
							num2 = num3;
						}
						else if (PadState.DPad.Up == ButtonState.Pressed)
						{
							num2 = -num3;
						}
					}
				}
				if (uiDelay > 0)
				{
					if (num == 0 && num2 == 0)
					{
						uiDelay = 0;
						uiDelayValue = UI_DELAY;
					}
					else
					{
						uiDelay--;
					}
				}
				if (uiDelay != 0)
				{
					return;
				}
				if (flag)
				{
					if (num != 0 || num2 != 0)
					{
						uiDelay = uiDelayValue;
						uiDelayValue = UI_DELAY / 2;
					}
					if (CurMenuMode == MenuMode.MAP)
					{
						PositionMapScreenCursor(num, num2);
						return;
					}
#if !VERSION_INITIAL
					if (CurMenuMode == MenuMode.CHARACTER_SELECT)
					{
						CharacterSelect.UpdateCursor(num, num2);
						return;
					}
#endif
					if (CurMenuMode == MenuMode.WORLD_SELECT)
					{
						WorldSelect.UpdateCursor(num, num2);
						return;
					}
					if (CurMenuMode == MenuMode.GAME_MODE)
					{
						GameMode.UpdateCursor(num, num2);
						return;
					}
					UpdateInventoryMenu();
					if (ActiveInvSection == InventorySection.CRAFTING)
					{
						PositionCraftingCursor(num, num2);
					}
					else
					{
						PositionInventoryCursor(num, num2);
					}
				}
				else if (num != 0 || num2 != 0)
				{
					uiDelay = MOUSE_DELAY;
					MouseX += (short)num;
					if (MouseX < 0)
					{
						MouseX = 0;
					}
					else if (MouseX > Main.ResolutionWidth - 16)
					{
						MouseX = (short)(Main.ResolutionWidth - 16);
					}
					MouseY += (short)num2;
					if (MouseY < 0)
					{
						MouseY = 0;
					}
					else if (MouseY > Main.ResolutionHeight - 16)
					{
						MouseY = (short)(Main.ResolutionHeight - 16);
					}
				}
				return;
			}
			if (uiDelay > 0)
			{
				if (PadState.ThumbSticks.Left.LengthSquared() <= SquaredDeadZone && PadState.ThumbSticks.Right.LengthSquared() <= SquaredDeadZone && PadState.DPad.Up == ButtonState.Released && PadState.DPad.Down == ButtonState.Released && PadState.DPad.Left == ButtonState.Released && PadState.DPad.Right == ButtonState.Released)
				{
					uiDelay = 0;
					uiDelayValue = UI_DELAY;
				}
				else
				{
					uiDelay--;
				}
				return;
			}
			int num4 = (IsLeftButtonDown() ? (-1) : (IsRightButtonDown() ? 1 : 0));
			if (num4 < 0)
			{
				if (--uiX < 0)
				{
					uiX = (short)(uiWidth - 1);
				}
				uiDelay = uiDelayValue;
				uiDelayValue = UI_DELAY / 2;
			}
			else if (num4 > 0)
			{
				if (++uiX == uiWidth)
				{
					uiX = 0;
				}
				uiDelay = uiDelayValue;
				uiDelayValue = UI_DELAY / 2;
			}
			num4 = (IsDownButtonDown() ? (-1) : (IsUpButtonDown() ? 1 : 0));
			while (true)
			{
				if (num4 > 0)
				{
					if (--uiY < 0)
					{
						uiY = (short)(uiHeight - 1);
					}
					uiDelay = uiDelayValue;
					uiDelayValue = UI_DELAY / 2;
				}
				else if (num4 < 0)
				{
					if (++uiY == uiHeight)
					{
						uiY = 0;
					}
					uiDelay = uiDelayValue;
					uiDelayValue = UI_DELAY / 2;
				}
				MouseX = uiCoords[uiX + uiY * uiWidth].X;
				if (MouseX != 0)
				{
					break;
				}
				if (num4 == 0 && ++uiY >= uiHeight)
				{
					uiY = 0;
				}
			}
			MouseY = uiCoords[uiX + uiY * uiWidth].Y;

#if !USE_ORIGINAL_CODE
			// This addition is not here in any of the updated versions, but as I cannot locate updated coordinates for the UI or even confirm their existence,
			// coupled with the fact that other code relating to UI and mouseY appears similar or different (depending on the version),
			// this is the workaround to allow for different menu UI options and sizes. I have no idea how the coordinates were calculated in the original,
			// but this is what I could come up with that best mimics the functionality.
			MouseY += 1;
			// That simple...
#endif
		}

		public static void UpdateOnce()
		{
			UpdateCursorColor();
			MouseTextBrightness = (byte)(MouseTextBrightness + mouseTextColorChange);
			if (MouseTextBrightness <= 175 || MouseTextBrightness >= 250)
			{
				mouseTextColorChange = (sbyte)(-mouseTextColorChange);
			}
			mouseTextColor.R = MouseTextBrightness;
			mouseTextColor.G = MouseTextBrightness;
			mouseTextColor.B = MouseTextBrightness;
			mouseTextColor.A = MouseTextBrightness;
			invAlpha = (byte)(invAlpha + invDir);
			if (invAlpha > 240)
			{
				invAlpha = 240;
				invDir = (sbyte)(-invDir);
			}
			else if (invAlpha < 180)
			{
				invAlpha = 180;
				invDir = (sbyte)(-invDir);
			}
			PulseScale += essDir;
			if (PulseScale > 1f)
			{
				PulseScale = 1f;
				essDir = 0f - essDir;
			}
			else if (PulseScale < 0.7f)
			{
				PulseScale = 0.7f;
				essDir = 0f - essDir;
			}
			BlueWave += blueDelta;
			if (BlueWave > 1f)
			{
				BlueWave = 1f;
				blueDelta = 0f - blueDelta;
			}
			else if (BlueWave < 0.97f)
			{
				BlueWave = 0.97f;
				blueDelta = 0f - blueDelta;
			}
			if (MessageBox.CurrentMess.AutoUpdate)
			{
				MessageBox.Update();
			}
		}

		public void UpdateGamePad()
		{
			if (Main.HasFocus)
			{
				gpPrevState = PadState;
				PadState = GamePad.GetState(controller);
			}
			else
			{
				gpPrevState = (PadState = (PadState = default(GamePadState)));
			}
		}

		public void Update()
		{
			if (MainUI.CurMenuMode == MenuMode.WELCOME)
			{
				if (IsSelectButtonTriggered())
				{
					ClearButtonTriggers();
					OpenMainView();
				}
			}
			else if (CurrentView == null)
			{
				if (Main.IsGameStarted && IsSelectButtonTriggered() && !Main.IsTutorial() && (Netplay.Session == null || ((ReadOnlyCollection<NetworkGamer>)(object)Netplay.Session.AllGamers).Count != 8))
				{
					ClearButtonTriggers();
					SetMenu(MenuMode.CHARACTER_SELECT, rememberPrevious: false, reset: true);
					OpenView();
				}
				return;
			}
			CurrentUI = this;
			if (CurMenuType == MenuType.NONE)
			{
				if (IsButtonUntriggered(Buttons.Start))
				{
					Main.PlaySound(10);
					uiFade = 0f;
					uiFadeTarget = 1f;
					CurMenuType = MenuType.PAUSE;
					SetMenu(MenuMode.PAUSE);
					ClearButtonTriggers();
				}
				else if (IsButtonTriggered(Buttons.Back))
				{
					miniMap.CreateMap(this);
					Main.PlaySound(10);
					if (Main.NetMode == (byte)NetModeSetting.CLIENT)
					{
						NetMessage.CreateMessage0(11);
						NetMessage.SendMessage();
					}
					SetMenu(MenuMode.MAP);
					CurMenuType = MenuType.PAUSE;
					ClearButtonTriggers();
				}
			}
			else
			{
				if (transferredPlayerStorage.Count > 0 && !MessageBox.IsVisible())
				{
					DeleteTransferredPlayerStorage();
				}
				if (saveIconMessageTime <= 0)
				{
					UpdateMenu();
				}
			}
			if (CurMenuType != 0)
			{
				UpdateIngame();
			}
			if (teamCooldown > 0 && --teamCooldown == 0 && teamSelected != ActivePlayer.team)
			{
				ActivePlayer.team = teamSelected;
				NetMessage.CreateMessage1(45, MyPlayer);
				NetMessage.SendMessage();
			}
			if (pvpCooldown > 0 && --pvpCooldown == 0 && pvpSelected != ActivePlayer.hostile)
			{
				ActivePlayer.hostile = pvpSelected;
				NetMessage.CreateMessage1(30, MyPlayer);
				NetMessage.SendMessage();
			}
			UpdateMouse();
			if (SignedInGamer != null && !SignedInGamer.IsGuest && !Main.IsTrial)
			{
				UpdateAchievements();
			}
		}

		private void UpdateIngame()
		{
			if (editSign)
			{
				ActivePlayer.UpdateEditSign();
			}
			if (autoSave && Main.TutorialState == Tutorial.NUM_TUTORIALS && HasPlayerStorage())
			{
				if (!saveTime.IsRunning)
				{
					saveTime.Start();
				}
				else if (saveTime.ElapsedMilliseconds > 600000)
				{
					saveTime.Reset();
					if (Main.NetMode == (byte)NetModeSetting.CLIENT || MainUI != this)
					{
						WorldGen.savePlayerWhilePlaying();
					}
					else
					{
						WorldGen.saveAllWhilePlaying();
					}
				}
			}
			else if (saveTime.IsRunning)
			{
				saveTime.Stop();
			}
			CurrentView.itemTextLocal.Update();
			CurrentView.dustLocal.UpdateDust();
			CurrentView.spawnSnow();
		}

		private void OpenMainView(SignedInGamer gamer = null)
		{
			if (MainUI != this)
			{
				CurrentView = MainUI.CurrentView;
				CurrentView.Ui = this;
				CurrentView.Player = ActivePlayer;
				MainUI.CurrentView = null;
				MainUI = this;
				Main.MusicVolume = MusicVolume;
				Main.SoundVolume = SoundVolume;
			}
			SignedInGamer = gamer;
			SetMenu(MenuMode.TITLE);
			if (gamer == null)
			{
#if USE_ORIGINAL_CODE
                ShowSignInPortal();
#else
				SignedInGamer = new SignedInGamer(Main.Gamertag); // ADDITION: Having the gamer set here depending on the settings will allow us to proceed.
				InitPlayerStorage();
#endif
			}
			else
			{
				InitPlayerStorage();
			}
		}

		private void OpenView()
		{
			CheckHDTV();
			MessageBox.Update();
			for (int i = 0; i < 4; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.CurrentView != null)
				{
					uI.SetUIView(WorldView.getViewType(uI.controller, this), NoAutoFullScreen: true);
				}
			}
			SetUIView(WorldView.getViewType(controller, this), NoAutoFullScreen: true);
			ShowSignInPortal();
		}

		public void ClearButtonTriggers()
		{
			gpPrevState = PadState;
		}

		public bool IsBackButtonTriggered()
		{
			if (!PadState.IsButtonDown(Buttons.Back) || !gpPrevState.IsButtonUp(Buttons.Back))
			{
				if (PadState.IsButtonDown(Buttons.B))
				{
					return gpPrevState.IsButtonUp(Buttons.B);
				}
				return false;
			}
			return true;
		}

		public bool IsSelectButtonTriggered()
		{
			if (!PadState.IsButtonDown(Buttons.Start) || !gpPrevState.IsButtonUp(Buttons.Start))
			{
				if (PadState.IsButtonDown(Buttons.A))
				{
					return gpPrevState.IsButtonUp(Buttons.A);
				}
				return false;
			}
			return true;
		}

		public bool IsLeftButtonDown()
		{
			if (!PadState.IsButtonDown(Buttons.DPadLeft) && !(PadState.ThumbSticks.Left.X < -GpDeadZone))
			{
				return PadState.ThumbSticks.Right.X < -GpDeadZone;
			}
			return true;
		}

		public bool IsRightButtonDown()
		{
			if (!PadState.IsButtonDown(Buttons.DPadRight) && !(PadState.ThumbSticks.Left.X > GpDeadZone))
			{
				return PadState.ThumbSticks.Right.X > GpDeadZone;
			}
			return true;
		}

		public bool IsUpButtonDown()
		{
			if (!PadState.IsButtonDown(Buttons.DPadUp) && !(PadState.ThumbSticks.Left.Y > GpDeadZone))
			{
				return PadState.ThumbSticks.Right.Y > GpDeadZone;
			}
			return true;
		}

		public bool IsDownButtonDown()
		{
			if (!PadState.IsButtonDown(Buttons.DPadDown) && !(PadState.ThumbSticks.Left.Y < -GpDeadZone))
			{
				return PadState.ThumbSticks.Right.Y < -GpDeadZone;
			}
			return true;
		}

		public bool IsAlternateLeftButtonDown()
		{
			return PadState.ThumbSticks.Right.X < -GpDeadZone;
		}

		public bool IsAlternateRightButtonDown()
		{
			return PadState.ThumbSticks.Right.X > GpDeadZone;
		}

		public bool IsAlternateUpButtonDown()
		{
			return PadState.ThumbSticks.Right.Y > GpDeadZone;
		}

		public bool IsAlternateDownButtonDown()
		{
			return PadState.ThumbSticks.Right.Y < -GpDeadZone;
		}

		public bool IsLeftButtonTriggered()
		{
			if (!PadState.IsButtonDown(Buttons.DPadLeft) || !gpPrevState.IsButtonUp(Buttons.DPadLeft))
			{
				if (PadState.ThumbSticks.Left.X < -GpDeadZone)
				{
					return gpPrevState.ThumbSticks.Left.X >= -GpDeadZone;
				}
				return false;
			}
			return true;
		}

		public bool IsRightButtonTriggered()
		{
			if (!PadState.IsButtonDown(Buttons.DPadRight) || !gpPrevState.IsButtonUp(Buttons.DPadRight))
			{
				if (PadState.ThumbSticks.Left.X > GpDeadZone)
				{
					return gpPrevState.ThumbSticks.Left.X <= GpDeadZone;
				}
				return false;
			}
			return true;
		}

		public bool IsButtonDown(Buttons b)
		{
			return PadState.IsButtonDown(b);
		}

		public bool IsButtonTriggered(Buttons b)
		{
			if (PadState.IsButtonDown(b))
			{
				return gpPrevState.IsButtonUp(b);
			}
			return false;
		}

		public bool IsButtonUntriggered(Buttons b)
		{
			if (PadState.IsButtonUp(b))
			{
				return gpPrevState.IsButtonDown(b);
			}
			return false;
		}

		private void DrawCursor()
		{
			if (CurMenuType != MenuType.NONE || InventoryMode != 0 || npcChatText != null || ActivePlayer.IsDead)
			{
				return;
			}
			bool flag = ActivePlayer.Inventory[ActivePlayer.SelectedItem].PickPower > 0 || ActivePlayer.Inventory[ActivePlayer.SelectedItem].AxePower > 0 || ActivePlayer.Inventory[ActivePlayer.SelectedItem].HammerPower > 0 || ActivePlayer.Inventory[ActivePlayer.SelectedItem].CreateTile >= 0 || ActivePlayer.Inventory[ActivePlayer.SelectedItem].CreateWall >= 0;
			if (cursorHighlight > 0)
			{
				int num = (16 - (CurrentView.ScreenPosition.X & 0xF)) & 0xF;
				int x = ((MouseX - num) & -16) + num;
				num = (16 - (CurrentView.ScreenPosition.Y & 0xF)) & 0xF;
				int y = ((MouseY - num) & -16) + num;
				Main.DrawRect(new Rectangle(x, y, 16, 16), new Color(cursorHighlight << 1, cursorHighlight << 1, 0, cursorHighlight << 1));
			}
			if (flag && ActivePlayer.velocity.X == 0f && ActivePlayer.velocity.Y == 0f)
			{
				if (cursorHighlight < 64)
				{
					cursorHighlight += 4;
				}
			}
			else if (cursorHighlight > 0)
			{
				cursorHighlight -= 2;
			}
			Rectangle value = default(Rectangle);
			value.Y = (int)(Main.FrameCounter & 16);
			value.Width = 16;
			value.Height = 16;
			Vector2 vector = default(Vector2);
			if (!UsingSmartCursor)
			{
				value.X = 16;
				vector.X = MouseX - 8;
				vector.Y = MouseY - 8;
			}
			else
			{
				if (ActivePlayer.controlDir.LengthSquared() <= 576f)
				{
					return;
				}
				vector.X = ActivePlayer.XYWH.X + (Player.width / 2) - CurrentView.ScreenPosition.X + (int)ActivePlayer.controlDir.X - 8;
				vector.Y = ActivePlayer.XYWH.Y + (Player.height / 2) - CurrentView.ScreenPosition.Y + (int)ActivePlayer.controlDir.Y - 8;
			}
			Main.SpriteBatch.Draw(cursorTexture, vector, (Rectangle?)value, Color.White);
		}

		public void DrawInventoryCursor(int x, int y, double scale, int alpha = 255)
		{
			alpha = MouseTextBrightness * alpha >> 8;
			SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x, y, new Color(alpha, alpha, alpha, alpha), (float)scale);
			MouseX = (short)x;
			MouseY = (short)y;
		}

		public static bool IsStorageEnabledForAnyPlayer()
		{
			if (Main.IsTrial)
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				if (Main.UIInstance[i].HasPlayerStorage())
				{
					return true;
				}
			}
			return false;
		}

		public void CheckPlayerStorage(string name)
		{
#if USE_ORIGINAL_CODE
			bool isTransferredFromOtherPlayer;
			IAsyncResult asyncResult = playerStorage.Device.BeginOpenContainer(name, allowTransferBetweenPlayers: true, out isTransferredFromOtherPlayer, null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			StorageContainer storageContainer = playerStorage.Device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			if (isTransferredFromOtherPlayer)
			{
				if (storageContainer.GetFileNames().Length == 0)
				{
					Main.ShowSaveIcon();
					Main.HideSaveIcon();
				}
				else
				{
					MessageBox.Show(controller, Lang.MenuText[9], string.Format(Lang.InterfaceText[78], name), new string[1]
					{
						Lang.MenuText[90]
					});
					transferredPlayerStorage.Add(name);
				}
			}
			storageContainer.Dispose();
#else
			Main.ShowSaveIcon();
			Main.HideSaveIcon();
#endif
        }

		public void DeleteTransferredPlayerStorage()
		{
			for (int num = transferredPlayerStorage.Count - 1; num >= 0; num--)
			{
				playerStorage.Device.DeleteContainer(transferredPlayerStorage[num]);
			}
			transferredPlayerStorage.Clear();
			DeviceSelected(null, null);
		}

		public StorageContainer OpenPlayerStorage(string name)
		{
			IAsyncResult asyncResult = playerStorage.Device.BeginOpenContainer(name, null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			StorageContainer result = playerStorage.Device.EndOpenContainer(asyncResult);
			asyncResult.AsyncWaitHandle.Close();
			return result;
		}

		private void DeviceDisconnected(object sender, EventArgs args)
		{
			LoadPlayers();
			WorldSelect.LoadWorlds();
			MessageBox.Show(controller, Lang.MenuText[69], Lang.MenuText[70], new string[1]
			{
				Lang.MenuText[90]
			});
			if (CurMenuMode == MenuMode.CONFIRM_DELETE_CHARACTER || CurMenuMode == MenuMode.CONFIRM_DELETE_WORLD)
			{
				PrevMenu();
			}
		}

		private void DeviceSelectorCanceled(object sender, EventArgs args)
		{
			LoadPlayers();
			WorldSelect.LoadWorlds();
			MessageBox.Show(controller, Lang.MenuText[69], Lang.MenuText[66], new string[1]
			{
				Lang.MenuText[90]
			});
		}

		private void DeviceSelected(object sender, EventArgs e)
		{
			try
			{
				CheckPlayerStorage("Settings");
				CheckPlayerStorage("Characters");
				CheckPlayerStorage("Worlds");
			}
			catch (Exception)
			{
				transferredPlayerStorage.Clear();
				return;
			}
			if (transferredPlayerStorage.Count <= 0)
			{
				if (!OpenSettings())
				{
					MessageBox.Show(controller, Lang.MenuText[9], Lang.MenuText[102], new string[1]
					{
						Lang.MenuText[90]
					});
				}
				LoadPlayers();
				if (this == MainUI && !WorldSelect.LoadWorlds())
				{
					MessageBox.Show(controller, Lang.MenuText[9], Lang.MenuText[103], new string[1]
					{
						Lang.MenuText[90]
					});
				}
			}
		}

		public void LoadPlayers()
		{
			if (HasPlayerStorage())
			{
				try
				{
					using (StorageContainer storageContainer = OpenPlayerStorage("Characters"))
					{
						while (true)
						{
							bool flag = true;
							string[] fileNames = storageContainer.GetFileNames("player?.plr");
							int num = fileNames.Length;
							if (num > MAX_LOAD_PLAYERS)
							{
								num = MAX_LOAD_PLAYERS;
							}
							int num2 = 0;
							while (true)
							{
								if (num2 < MAX_LOAD_PLAYERS)
								{
									if (num2 < num)
									{
										loadPlayerPath[num2] = fileNames[num2];
										loadPlayer[num2].Load(storageContainer, loadPlayerPath[num2]);
										if (loadPlayer[num2].Name == null)
										{
											MessageBox.Show(controller, Lang.MenuText[9], Lang.MenuText[12], new string[1]
											{
												Lang.MenuText[90]
											});
											flag = false;
											break;
										}
									}
									else
									{
										loadPlayer[num2] = new Player();
									}
									num2++;
									continue;
								}
								numLoadPlayers = (sbyte)num;
								break;
							}

							if (!flag)
							{
								continue;
							}

							break;
						}
					}
				}
				catch (IOException)
				{
					ReadError();
					numLoadPlayers = 0;
				}
				catch (Exception)
				{
					numLoadPlayers = 0;
				}
			}
			else
			{
				numLoadPlayers = 0;
			}
			if (CurMenuMode == MenuMode.CHARACTER_SELECT)
			{
				ResetPlayerMenuSelection();
			}
		}

		public void SaveSettings()
		{
			if (!HasPlayerStorage())
			{
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(512))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(Main.SettingsDataVersion);
					binaryWriter.Write(0u);
					binaryWriter.Write(SoundVolume);
					binaryWriter.Write(MusicVolume);
					binaryWriter.Write(autoSave);
					binaryWriter.Write(ShowItemText);
					binaryWriter.Write(alternateGrappleControls);
					byte[] buffer = Statistics.Serialize();
					binaryWriter.Write(buffer);
					binaryWriter.Write(totalSteps);
					binaryWriter.Write(TotalOrePicked);
					binaryWriter.Write(TotalBarsCrafted);
					binaryWriter.Write(TotalAnvilCrafting);
					binaryWriter.Write(totalWires);
					binaryWriter.Write(totalAirTime);
					binaryWriter.Write(petSpawnMask);
					int num = armorFound.Length + 7 >> 3;
					byte[] array = new byte[num];
					armorFound.CopyTo(array, 0);
					binaryWriter.Write((ushort)num);
					binaryWriter.Write(array, 0, num);
					binaryWriter.Write(IsOnline);
					binaryWriter.Write(IsInviteOnly);

#if VERSION_101
					binaryWriter.Write(IsOnline); // This is not what is written but 1.01 accounts for 2 new booleans; I currently do not know what they are.
					binaryWriter.Write(IsInviteOnly);
#endif

					num = blacklist.Count;
					int num2 = num;
					if (num > 65535)
					{
						num = 65535;
					}
					binaryWriter.Write((ushort)num);
					for (int i = num2 - num; i < num2; i++)
					{
						binaryWriter.Write(blacklist[i]);
					}
					CRC32 cRC = new CRC32();
					cRC.Update(memoryStream.GetBuffer(), 8, (int)memoryStream.Length - 8);
					binaryWriter.Seek(4, SeekOrigin.Begin);
					binaryWriter.Write(cRC.GetValue());
					Main.ShowSaveIcon();
					try
					{
						if (TestStorageSpace("Settings", "config.dat", (int)memoryStream.Length))
						{
							using (StorageContainer storageContainer = OpenPlayerStorage("Settings"))
							{
								using (Stream stream = storageContainer.OpenFile("config.dat", FileMode.Create))
								{
									stream.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
									stream.Close();
								}
								SettingsDirty = false;
							}
						}
					}
					catch (IOException)
					{
						WriteError();
					}
					catch (Exception)
					{
					}
					binaryWriter.Close();
					Main.HideSaveIcon();
				}
			}
		}

		private bool OpenSettings()
		{
			bool flag = true;
			try
			{
				using (StorageContainer storageContainer = OpenPlayerStorage("Settings"))
				{
					if (storageContainer.FileExists("config.dat"))
					{
						try
						{
							using (Stream stream = storageContainer.OpenFile("config.dat", FileMode.Open))
							{
								using (MemoryStream memoryStream = new MemoryStream((int)stream.Length))
								{
									memoryStream.SetLength(stream.Length);
									stream.Read(memoryStream.GetBuffer(), 0, (int)stream.Length);
									stream.Close();
									using (BinaryReader binaryReader = new BinaryReader(memoryStream))
									{
										int num = binaryReader.ReadInt32();
										if (num > Main.SettingsDataVersion)
										{
											throw new InvalidOperationException("Invalid version");
										}
										if (num >= 4) // Like the Player saving, this went through revisions in development too. Here, we can see the CRC being added.
										{
											CRC32 cRC = new CRC32();
											cRC.Update(memoryStream.GetBuffer(), 8, (int)memoryStream.Length - 8);
											if (cRC.GetValue() != binaryReader.ReadUInt32())
											{
												throw new InvalidOperationException("Invalid CRC32");
											}
										}
										SoundVolume = binaryReader.ReadSingle();
										MusicVolume = binaryReader.ReadSingle();
										autoSave = binaryReader.ReadBoolean();
										ShowItemText = binaryReader.ReadBoolean();
										alternateGrappleControls = binaryReader.ReadBoolean();
										if (num <= 3) // Rev 4 appeared to add saved alternative controls.
										{
											alternateGrappleControls = false;
										}
										UpdateAlternateGrappleControls();
										if (this == MainUI)
										{
											Main.MusicVolume = MusicVolume;
											Main.SoundVolume = SoundVolume;
										}
										int count = Statistics.CalculateSerialisationSize();
										byte[] stream2 = binaryReader.ReadBytes(count);
										Statistics.Deserialize(stream2);
										if (num >= 2) // Rev 2 appears to have added online play settings.
										{
											if (num >= 3) // Rev 3 appears to have added achievements.
											{
												totalSteps = binaryReader.ReadUInt32();
												TotalOrePicked = binaryReader.ReadUInt32();
												TotalBarsCrafted = binaryReader.ReadUInt32();
												TotalAnvilCrafting = binaryReader.ReadUInt32();
												totalWires = binaryReader.ReadUInt32();
												totalAirTime = binaryReader.ReadUInt32();
												petSpawnMask = binaryReader.ReadByte();
												int count2 = binaryReader.ReadUInt16();
												armorFound = new BitArray(binaryReader.ReadBytes(count2));
												if (armorFound.Length < (int)Item.ID.NUM_TYPES)
												{
													armorFound.Length = (int)Item.ID.NUM_TYPES;
												}
											}
											IsOnline = binaryReader.ReadBoolean();
											IsInviteOnly = binaryReader.ReadBoolean();
										}

#if VERSION_101
										binaryReader.ReadBoolean(); // See SaveSettings() for why this is here.
										binaryReader.ReadBoolean();
#endif

										blacklist.Clear();
										if (num >= 5) // The initial release appears to have added blacklisting.
										{
											int num2 = binaryReader.ReadUInt16();
											blacklist.Capacity = num2 + 4;
											while (num2 > 0)
											{
												blacklist.Add(binaryReader.ReadUInt64());
												num2--;
											}
										}
										binaryReader.Close();
									}
								}
							}
						}
						catch (InvalidOperationException)
						{
							Main.ShowSaveIcon();
							flag = false;
							storageContainer.DeleteFile("config.dat");
							armorFound = new BitArray((int)Item.ID.NUM_TYPES);
							Main.HideSaveIcon();
						}
						catch (Exception)
						{
						}
					}
				}
				SettingsDirty = !flag;
			}
			catch (IOException)
			{
				if (!flag)
				{
					WriteError();
					flag = true;
				}
				else
				{
					ReadError();
				}
			}
			catch (Exception)
			{
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT && Main.IsGameStarted)
			{
				CheckBlacklist();
			}
			return flag;
		}

		public void ErasePlayer(int i)
		{
			if (HasPlayerStorage())
			{
				Main.ShowSaveIcon();
				try
				{
					using (StorageContainer storageContainer = OpenPlayerStorage("Characters"))
					{
						storageContainer.DeleteFile(loadPlayerPath[i]);
					}
				}
				catch (IOException)
				{
					WriteError();
				}
				catch (Exception)
				{
				}
				Main.HideSaveIcon();
			}
			numLoadPlayers--;
			loadPlayer[i] = loadPlayer[numLoadPlayers];
			loadPlayerPath[i] = loadPlayerPath[numLoadPlayers];
		}

		private string nextLoadPlayer()
		{
			int num = 0;
			string result = null;
			if (HasPlayerStorage())
			{
				try
				{
					using (StorageContainer storageContainer = OpenPlayerStorage("Characters"))
					{
						do
						{
							num++;
							result = "player" + num + ".plr";
						}
						while (storageContainer.FileExists(result));
						return result;
					}
				}
				catch (IOException)
				{
					ReadError();
					return null;
				}
				catch (Exception)
				{
					return null;
				}
			}
			return result;
		}

		public void SetUIView(WorldView.Type viewType, bool NoAutoFullScreen = false)
		{
			if (CurrentView != null)
			{
				for (int i = 0; i < NumActiveViews; i++)
				{
					if (activeView[i] == CurrentView)
					{
						activeView[i] = activeView[--NumActiveViews];
						activeView[NumActiveViews] = null;
						break;
					}
				}
			}
			if (viewType != WorldView.Type.NONE)
			{
				if (CurrentView == null)
				{
					CurrentView = new WorldView();
					CurrentView.Player = ActivePlayer;
					CurrentView.Ui = this;
				}
				activeView[NumActiveViews++] = CurrentView;
				CurrentUI = this;
				if (NumActiveViews == 2)
				{
					LoadSplitscreenFonts(theGame.Content);
					InvalidateCachedText();
				}
				if (CurrentView.setViewType(viewType) && CurMenuType != 0)
				{
					worldFade = WORLD_FADE_START;
				}
			}
			else if (CurrentView != null)
			{
				CurrentView.Dispose();
				CurrentView = null;
				if (NumActiveViews == 1)
				{
					LoadFonts(theGame.Content);
					InvalidateCachedText();
				}
				if (MainUI == this)
				{
					if (NumActiveViews > 0)
					{
						for (int j = 0; j < 4; j++)
						{
							if (Main.UIInstance[j].CurrentView != null)
							{
								MainUI = Main.UIInstance[j];
								WorldSelect.LoadWorlds();
								break;
							}
						}
					}
					else
					{
						MainUI = null;
					}
				}
			}
			else
			{
				NoAutoFullScreen = true;
			}
			if (ActivePlayer != null)
			{
				ActivePlayer.CurrentView = CurrentView;
				if (CurrentView == null)
				{
					ActivePlayer.Active = 0;
				}
			}
			if (NoAutoFullScreen)
			{
				return;
			}
			if (NumActiveViews == 1 && MainUI != null && MainUI.CurrentView != null && !MainUI.CurrentView.IsFullScreen())
			{
				MainUI.CurrentView.setViewType();
				if (MainUI.CurMenuType != 0)
				{
					MainUI.worldFade = WORLD_FADE_START;
				}
				return;
			}
			for (int k = 0; k < 4; k++)
			{
				UI uI = Main.UIInstance[k];
				if (uI.CurrentView != null)
				{
					uI.CurrentView.setViewType(WorldView.getViewType(uI.controller));
				}
			}
		}

		public void setPlayer(int id, bool swapIfUsed = true)
		{
			if (id < 0)
			{
				for (int num = Player.MaxNumPlayers - 1; num >= 0; num--)
				{
					if (Main.PlayerSet[num].Active == 0 && Main.PlayerSet[num].CurrentView == null)
					{
						id = num;
						break;
					}
				}
				if (id < 0)
				{
					MyPlayer = Player.MaxNumPlayers;
					ActivePlayer = null;
					return;
				}
			}
			if (ActivePlayer != null && id != MyPlayer)
			{
				Player player = ActivePlayer.DeepCopy();
				player.WhoAmI = (byte)id;
				ActivePlayer.ui = null;
				ActivePlayer.CurrentView = null;
				if (swapIfUsed)
				{
					for (int i = 0; i < 4; i++)
					{
						UI uI = Main.UIInstance[i];
						if (uI != this && uI.MyPlayer == id)
						{
							uI.setPlayer(MyPlayer, swapIfUsed: false);
							break;
						}
					}
				}
				if (id != MyPlayer)
				{
					Main.PlayerSet[id] = player;
				}
			}
			MyPlayer = (byte)id;
			ActivePlayer = Main.PlayerSet[id];
			ActivePlayer.ui = this;
			ActivePlayer.CurrentView = CurrentView;
			if (SignedInGamer != null)
			{
				ActivePlayer.Name = SignedInGamer.Gamertag;
			}
			teamCooldown = 0;
			teamSelected = ActivePlayer.team;
			pvpCooldown = 0;
			pvpSelected = ActivePlayer.hostile;
			if (CurrentView != null)
			{
				CurrentView.Player = ActivePlayer;
			}
			else
			{
				ActivePlayer.Active = 0;
			}
		}

		public void SetPlayer(Player p)
		{
			if (ActivePlayer != null && p != ActivePlayer)
			{
				ActivePlayer.ui = null;
				ActivePlayer.CurrentView = null;
			}
			ActivePlayer = p;
			teamCooldown = 0;
			pvpCooldown = 0;
			if (CurrentView != null)
			{
				CurrentView.Player = p;
			}
			if (p != null)
			{
				teamSelected = p.team;
				pvpSelected = p.hostile;
				p.ui = this;
				p.CurrentView = CurrentView;
				p.WhoAmI = MyPlayer;
				if (SignedInGamer != null)
				{
					p.Name = SignedInGamer.Gamertag;
				}
				p.Active = 0;
				Main.PlayerSet[MyPlayer] = p;
			}
			else
			{
				MyPlayer = Player.MaxNumPlayers;
				SetUIView(WorldView.Type.NONE);
			}
		}

		public void JoinSession(int newPlayerIndex)
		{
			setPlayer(newPlayerIndex);
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				NetMessage.CreateMessage0(11);
				NetMessage.SendMessage();
			}
		}

		public void LeaveSession()
		{
			localGamer = null;
		}

		public void InviteAccepted(InviteAcceptedEventArgs e)
		{
			if (e.IsCurrentSession || Netplay.isJoiningRemoteInvite)
			{
				if (Netplay.Session == null)
				{
					if (!Netplay.gamersWhoReceivedInvite.Contains(e.Gamer))
					{
						Netplay.gamersWhoReceivedInvite.Add(e.Gamer);
					}
					if (!Netplay.gamersWaitingToJoinInvite.Contains(e.Gamer))
					{
						Netplay.gamersWaitingToJoinInvite.Add(e.Gamer);
					}
				}
				if (CurrentView == null)
				{
					SetMenu(MenuMode.CHARACTER_SELECT, rememberPrevious: false, reset: true);
					OpenView();
				}
				return;
			}
			Netplay.isJoiningRemoteInvite = true;
			Netplay.gamersWhoReceivedInvite.Add(e.Gamer);
			Netplay.gamersWaitingToJoinInvite.Add(e.Gamer);
			for (int i = 0; i < 4; i++)
			{
				SignedInGamer signedInGamer = Main.UIInstance[i].SignedInGamer;
				if (signedInGamer != null && !Netplay.gamersWaitingToJoinInvite.Contains(signedInGamer))
				{
					Netplay.gamersWaitingToJoinInvite.Add(signedInGamer);
				}
			}
			if (Netplay.Session != null)
			{
				ExitGame();
				return;
			}
			if (MainUI.CurMenuMode == MenuMode.WELCOME)
			{
				OpenMainView(e.Gamer);
				return;
			}
			if (Main.WorldGenThread != null)
			{
				Main.WorldGenThread.Abort();
				Main.WorldGenThread = null;
				WorldGen.Gen = false;
			}
			MainUI.SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
		}

		private static void CancelInvite(SignedInGamer gamer)
		{
			Netplay.gamersWhoReceivedInvite.Remove(gamer);
			if (Netplay.gamersWhoReceivedInvite.Count == 0)
			{
				Netplay.isJoiningRemoteInvite = false;
				Netplay.gamersWaitingToJoinInvite.Clear();
			}
			else
			{
				Netplay.gamersWaitingToJoinInvite.Remove(gamer);
			}
		}

		private unsafe void DrawControlsIngame()
		{
			if (CurMenuType != MenuType.NONE)
			{
				return;
			}
			Main.StrBuilder.Length = 0;
			if (!Main.TutorialMaskY)
			{
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.INVENTORY));
				Main.StrBuilder.Append(' ');
			}
			if (ActivePlayer.grappleItemSlot >= 0)
			{
				Main.StrBuilder.Append(Lang.Controls(alternateGrappleControls ? Lang.CONTROLS.GRAPPLE_ALT : Lang.CONTROLS.GRAPPLE));
				Main.StrBuilder.Append(' ');
			}
			fixed (Item* ptr = &ActivePlayer.Inventory[ActivePlayer.SelectedItem])
			{
				if (!Main.TutorialMaskRT && ptr->Type > 0)
				{
					if (ptr->PickPower > 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.DIG));
					}
					else if (ptr->AxePower > 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHOP));
					}
					else if (ptr->HammerPower > 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.HIT));
					}
					else if (ptr->CreateTile >= 0 || ptr->CreateWall >= 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BUILD));
					}
					else if (ptr->Ammo > 0 || ptr->Damage > 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.ATTACK));
					}
					else
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.USE));
					}
					Main.StrBuilder.Append(' ');
				}
				if (!Main.TutorialMaskB)
				{
					if (ActivePlayer.npcChatBubble >= 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.TALK));
					}
					else if (ActivePlayer.tileInteractX != 0)
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.INTERACT));
					}
				}
			}
			DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
		}

		private void DrawControlsInventory()
		{
			if (CurMenuType != MenuType.NONE)
			{
				return;
			}
			Main.StrBuilder.Length = 0;
#if !VERSION_INITIAL
			Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
			Main.StrBuilder.Append(' ');
#endif
			Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_MENU));
			Main.StrBuilder.Append(' ');
			if (toolTip.Type > 0 && !reforge && !CraftGuide)
			{
				if (toolTip.IsEquipable() && (ActiveInvSection != InventorySection.EQUIP || inventoryEquipX == 0))
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.EQUIP));
					Main.StrBuilder.Append(' ');
				}
				if (toolTip.Type >= (int)Item.ID.BLUE_PRESENT && toolTip.Type <= (int)Item.ID.YELLOW_PRESENT)
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.OPEN));
					Main.StrBuilder.Append(' ');
				}
				else if (toolTip.Stack > 1 && (mouseItem.Type == 0 || mouseItem.NetID == toolTip.NetID)) // BUG: In later releases, it will show 'Take one' even on items that cannot stack.
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT_ONE));
					Main.StrBuilder.Append(' ');
				}
			}
			if (mouseItem.Type == 0)
			{
				if (toolTip.Type > 0)
				{
					if (reforge)
					{
						if (toolTip.SetPrefix(-3))
						{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.REFORGE));
							Main.StrBuilder.Append(' ');
						}
					}
					else if (CraftGuide)
					{
						if (toolTip.IsMaterial)
						{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_RECIPES));
							Main.StrBuilder.Append(' ');
						}
					}
					else
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT_ALL));
						Main.StrBuilder.Append(' ');
					}
				}
				else if (ActiveInvSection == InventorySection.CHEST)
				{
					if (inventoryChestX < 0)
					{
						switch (inventoryChestY)
						{
						case 1:
								Main.StrBuilder.Append(Lang.InterfaceText[29]);
							break;
						case 2:
								Main.StrBuilder.Append(Lang.InterfaceText[30]);
							break;
						case 3:
								Main.StrBuilder.Append(Lang.InterfaceText[31]);
							break;
						}
						Main.StrBuilder.Append(' ');
					}
				}
				else if (ActiveInvSection == InventorySection.EQUIP && inventoryEquipY == 0)
				{
					int num = inventoryEquipX + inventoryBuffX;
					num %= Player.MaxNumBuffs;
					if (ActivePlayer.buff[num].Time > 0 && !ActivePlayer.buff[num].IsDebuff())
					{
						Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CANCEL_BUFF));
						Main.StrBuilder.Append(' ');
					}
				}
			}
			else
			{
				bool flag = false;
				bool flag2 = true;
				if (ActiveInvSection == InventorySection.EQUIP)
				{
					switch (inventoryEquipY)
					{
					case 0:
						flag = mouseItem.HeadSlot >= 0;
						break;
					case 1:
						flag = mouseItem.BodySlot >= 0;
						break;
					case 2:
						flag = mouseItem.LegSlot >= 0;
						break;
					default:
						flag = mouseItem.IsAccessory;
						break;
					}
				}
				else if (ActiveInvSection == InventorySection.ITEMS && mouseItem.Type > 0 && mouseItem.Stack > 0)
				{
					switch (inventoryItemY)
					{
					case 4:
						flag2 = mouseItem.CanBePlacedInAmmoSlot();
						break;
					case 5:
						flag2 = mouseItem.CanBePlacedInCoinSlot();
						break;
					}
				}
				if (flag2)
				{
					if (toolTip.Type > 0 && (toolTip.NetID != mouseItem.NetID || toolTip.Stack == toolTip.MaxStack || mouseItem.Stack == mouseItem.MaxStack))
					{
						if (ActiveInvSection != InventorySection.EQUIP || flag)
						{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SWAP));
							Main.StrBuilder.Append(' ');
						}
					}
					else if (toolTip.Type == 0 || toolTip.Stack < toolTip.MaxStack)
					{
						if (ActiveInvSection == InventorySection.EQUIP)
						{
							if (flag)
							{
								Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.PLACE_EQUIPMENT));
								Main.StrBuilder.Append(' ');
							}
						}
						else
						{
							Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.PLACE));
							Main.StrBuilder.Append(' ');
						}
					}
				}
			}
#if VERSION_INITIAL
			Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
			Terraria.Main.StrBuilder.Append(' ');
#endif
			if (!reforge && !CraftGuide)
			{
				if (mouseItem.Type > 0)
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.DROP));
					Main.StrBuilder.Append(' ');
				}
				else if (ActiveInvSection == InventorySection.ITEMS && toolTip.Type > 0)
				{
					Lang.CONTROLS i = ((npcShop <= 0) ? Lang.CONTROLS.TRASH : Lang.CONTROLS.SELL);
					Main.StrBuilder.Append(Lang.Controls(i));
					Main.StrBuilder.Append(' ');
				}
			}
			DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
		}

		private void DrawControlsShop()
		{
			if (CurMenuType != MenuType.NONE)
			{
				return;
			}
			Main.StrBuilder.Length = 0;
#if !VERSION_INITIAL
			Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
			Main.StrBuilder.Append(' ');
#endif
			Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_MENU));
			Main.StrBuilder.Append(' ');
			if (toolTip.Type > 0 && toolTip.Stack > 1 && (mouseItem.Type == 0 || mouseItem.NetID == toolTip.NetID))
			{
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BUY_ONE));
				Main.StrBuilder.Append(' ');
			}
			if (mouseItem.Type == 0)
			{
				if (toolTip.Type > 0)
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BUY_ALL));
					Main.StrBuilder.Append(' ');
				}
			}
			else if (toolTip.Type == 0)
			{
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.SELL_ITEM_IN_HAND));
				Main.StrBuilder.Append(' ');
			}
#if VERSION_INITIAL
			Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
			Terraria.Main.StrBuilder.Append(' ');
#endif
			if (mouseItem.Type > 0)
			{
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.DROP));
				Main.StrBuilder.Append(' ');
			}
			DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
		}

		private void DrawControlsCrafting()
		{
			if (CurMenuType == MenuType.NONE)
			{
				Main.StrBuilder.Length = 0;
#if !VERSION_INITIAL
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
				Main.StrBuilder.Append(' ');
#endif
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_MENU));
				Main.StrBuilder.Append(' ');
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CRAFTING_CATEGORY));
				Main.StrBuilder.Append(' ');
				if (ActivePlayer.CanCraftRecipe(CraftingRecipe))
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CRAFT));
					Main.StrBuilder.Append(' ');
				}
#if VERSION_INITIAL
				Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
				Terraria.Main.StrBuilder.Append(' ');
#endif
				if (mouseItem.Type > 0)
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.DROP)); // What?
					Main.StrBuilder.Append(' ');
				}
				else
				{
					Main.StrBuilder.Append(Lang.Controls(craftingShowCraftable ? Lang.CONTROLS.SHOW_ALL : Lang.CONTROLS.SHOW_AVAILABLE));
					Main.StrBuilder.Append(' ');
				}
				Lang.CONTROLS i = ((craftingSection == CraftingSection.RECIPES) ? Lang.CONTROLS.INGREDIENTS : Lang.CONTROLS.RECIPES);
				Main.StrBuilder.Append(Lang.Controls(i));
				DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
			}
		}

		private void DrawControlsHousing()
		{
			if (CurMenuType == MenuType.NONE)
			{
				Main.StrBuilder.Length = 0;
#if !VERSION_INITIAL
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
				Main.StrBuilder.Append(' ');
#endif
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_MENU));
				Main.StrBuilder.Append(' ');
				Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHECK_HOUSING));
				Main.StrBuilder.Append(' ');
				Main.StrBuilder.Append(Lang.Controls(showNPCs ? Lang.CONTROLS.HIDE_BANNERS : Lang.CONTROLS.SHOW_BANNERS));
				Main.StrBuilder.Append(' ');
				if (inventoryHousingNpc >= 0)
				{
					Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.ASSIGN_TO_ROOM));
					Main.StrBuilder.Append(' ');
				}
#if VERSION_INITIAL
				Terraria.Main.StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CLOSE));
				Terraria.Main.StrBuilder.Append(' ');
#endif
				DrawStringLB(BoldSmallFont, CurrentView.SafeAreaOffsetLeft, CurrentView.SafeAreaOffsetBottom);
			}
		}

		public int DrawDialog(Vector2 pos, Color backColor, Color textColor, CompiledText ct, string caption = null, bool anchorBottom = false)
		{

		// This is hella messy, primarily due to there being no 'Decomp-equivalent' that I can reasonably decipher with what I have at the moment, so I have to account for it all here.

#if !USE_ORIGINAL_CODE
			int num = (int)(30 * Main.ScreenMultiplier);
			if (Main.ScreenHeightPtr != 0)
			{
				if (anchorBottom)
				{
					switch (Main.ScreenHeightPtr)
					{
						case 1:
							num = (int)(num / 1.33f);
							break;
						case 2:
							num = (int)(num / 2f);
							break;
					}
				}
				else
				{
					switch (Main.ScreenHeightPtr)
					{
						case 1:
							num = (int)(num / 1.33f);
							break;
						case 2:
							num = (int)(num / 1.25f);
							break;
					}
				}
			}
#else
			int num = 30;
#endif
			if (anchorBottom)
			{
				pos.Y -= ct.Height + num;
				num = 0;
			}

#if USE_ORIGINAL_CODE
			Main.SpriteBatch.Draw(chatBackTexture, pos, (Rectangle?)new Rectangle(0, 0, chatBackTexture.Width, ct.Height + num), backColor);
			pos.Y += ct.Height + num;
			Main.SpriteBatch.Draw(chatBackTexture, pos, (Rectangle?)new Rectangle(0, chatBackTexture.Height - 30, chatBackTexture.Width, 30), backColor);
			pos.Y -= ct.Height + num;
#else
			int HeightOffset = 30;
			int WrapWidth = 470;
			int ChatBackTexSize = 500; // As previously mentioned, the 1.2 versions stopped using the Chat_Back texture, including when running in 540p mode.
			// For clarity purposes, most instances of chatBackTexture.Width/Height will be replaced with 500 if 540p mode is detected, since the result will be the same.
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.15f);
					WrapWidth = 548;

					if (CurMenuType != MenuType.NONE)
					{
						HeightOffset = (int)(HeightOffset / 1.33f);
					}
					break;
				case 2:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.534f);
					WrapWidth = 705;
					break;
			}

			if (Main.PSMode && Main.ScreenHeightPtr == 1)
			{
				HeightOffset -= 7; // There is no line spacing difference between PS3 and X360, but it does treat the text as if there is one, so we account for it with this and some changes in CompiledText.
			}

#if VERSION_103 || VERSION_FINAL
			// In the 1.2 versions, all 'old-gen' versions utilised DrawRect instead of a 2D Texture.
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, pos, new Rectangle(0, 0, ChatBackTexSize, ct.Height + num + 30), backColor);
#else
			if (Main.ScreenHeightPtr != 0)
			{
				Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, pos, new Rectangle(0, 0, ChatBackTexSize, ct.Height + num + HeightOffset), backColor);
			}
			else
			{
				Main.SpriteBatch.Draw(chatBackTexture, pos, (Rectangle?)new Rectangle(0, 0, chatBackTexture.Width, ct.Height + num), backColor);
				pos.Y += ct.Height + num;
				Main.SpriteBatch.Draw(chatBackTexture, pos, (Rectangle?)new Rectangle(0, chatBackTexture.Height - 30, chatBackTexture.Width, HeightOffset), backColor);
				pos.Y -= ct.Height + num;
			}
#endif
#endif

			if (caption != null)
			{
				int num2 = (int)BoldSmallFont.MeasureString(caption).X;
#if !USE_ORIGINAL_CODE
				int num3 = ChatBackTexSize - num2 >> 1;
				int num4 = 0;
				if (Main.ScreenHeightPtr == 2)
				{
					num4 += HeightOffset / 2;
				}
#else
				int num3 = chatBackTexture.Width - num2 >> 1;
				int num4 = 0;
#endif
				Main.SpriteBatch.DrawString(BoldSmallFont, caption, new Vector2(pos.X + num3, pos.Y + num4), Color.LightGreen);
				pos.Y += num;
			}
			else
			{
				pos.Y += 10f;
			}
			pos.X += 20f;

#if !USE_ORIGINAL_CODE
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					if (caption == null)
					{
						pos.Y += 0.62f;
					}

					if (CurMenuType != MenuType.NONE)
					{
						pos.Y -= 5f;
						pos.X -= 6.25f;
					}
					pos.X -= 1.25f;
					break;
				case 2:
					pos.X += 8f;
					pos.Y += 4f;

					if (caption != null)
					{
						pos.Y += 12f;
					}
					break;
			}
#endif

#if USE_ORIGINAL_CODE
			ct.Draw(Main.SpriteBatch, new Rectangle((int)pos.X, (int)pos.Y, 470, Main.ResolutionHeight), textColor, new Color(255, 212, 64, 255));
#else
			ct.Draw(Main.SpriteBatch, new Rectangle((int)pos.X, (int)pos.Y, WrapWidth, Main.ResolutionHeight), textColor, new Color(255, 212, 64, 255));
#endif
			return ct.Height;
		}

		private void HelpText()
		{
			bool flag = ActivePlayer.StatLifeMax > 100;
			bool flag2 = ActivePlayer.statManaMax > 0;
			bool flag3 = true;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			for (int i = 0; i < Player.MaxNumInventory; i++)
			{
				if (ActivePlayer.Inventory[i].PickPower > 0 && ActivePlayer.Inventory[i].NetID != -13)
				{
					flag3 = false;
				}
				else if (ActivePlayer.Inventory[i].AxePower > 0 && ActivePlayer.Inventory[i].NetID != -16)
				{
					flag3 = false;
				}
				else if (ActivePlayer.Inventory[i].HammerPower > 0)
				{
					flag3 = false;
				}
				switch ((Item.ID)ActivePlayer.Inventory[i].Type)
				{
				case Item.ID.IRON_ORE:
				case Item.ID.COPPER_ORE:
				case Item.ID.GOLD_ORE:
				case Item.ID.SILVER_ORE:
					flag4 = true;
					break;
				case Item.ID.GOLD_BAR:
				case Item.ID.COPPER_BAR:
				case Item.ID.SILVER_BAR:
				case Item.ID.IRON_BAR:
					flag5 = true;
					break;
				case Item.ID.FALLEN_STAR:
					flag6 = true;
					break;
				case Item.ID.LENS:
					flag7 = true;
					break;
				case Item.ID.ROTTEN_CHUNK:
				case Item.ID.WORM_FOOD:
					flag8 = true;
					break;
				case Item.ID.GRAPPLING_HOOK:
					flag9 = true;
					break;
				}
			}
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			bool flag16 = false;
			bool flag17 = false;
			bool flag18 = false;
			for (int j = 0; j < NPC.MaxNumNPCs; j++)
			{
				if (Main.NPCSet[j].Active != 0)
				{
					switch ((NPC.ID)Main.NPCSet[j].Type)
					{
					case NPC.ID.MERCHANT:
						flag10 = true;
						break;
					case NPC.ID.NURSE:
						flag11 = true;
						break;
					case NPC.ID.ARMS_DEALER:
						flag13 = true;
						break;
					case NPC.ID.DRYAD:
						flag12 = true;
						break;
					case NPC.ID.GOBLIN_TINKERER:
						flag17 = true;
						break;
					case NPC.ID.CLOTHIER:
						flag18 = true;
						break;
					case NPC.ID.MECHANIC:
						flag15 = true;
						break;
					case NPC.ID.DEMOLITIONIST:
						flag14 = true;
						break;
					case NPC.ID.WIZARD:
						flag16 = true;
						break;
					}
				}
			}
			while (true)
			{
				helpText++;
				if (flag3)
				{
					if (helpText == 1)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 177);
						return;
					}
					if (helpText == 2)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 178);
						return;
					}
					if (helpText == 3)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 179);
						return;
					}
					if (helpText == 4)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 180);
						return;
					}
					if (helpText == 5)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 181);
						return;
					}
					if (helpText == 6)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 182);
						return;
					}
				}
				if (flag3 && !flag4 && !flag5 && helpText == 11)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 183);
					return;
				}
				if (flag3 && flag4 && !flag5)
				{
					if (helpText == 21)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 184);
						return;
					}
					if (helpText == 22)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 185);
						return;
					}
				}
				if (flag3 && flag5)
				{
					if (helpText == 31)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 186);
						return;
					}
					if (helpText == 32)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 187);
						return;
					}
				}
				if (!flag && helpText == 41)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 188);
					return;
				}
				if (!flag2 && helpText == 42)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 189);
					return;
				}
				if (!flag2 && !flag6 && helpText == 43)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 190);
					return;
				}
				if (!flag10 && !flag11)
				{
					if (helpText == 51)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 191);
						return;
					}
					if (helpText == 52)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 192);
						return;
					}
					if (helpText == 53)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 193);
						return;
					}
					if (helpText == 54)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 194);
						return;
					}
				}
				if (!flag10 && helpText == 61)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 195);
					return;
				}
				if (!flag11 && helpText == 62)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 196);
					return;
				}
				if (!flag13 && helpText == 63)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 197);
					return;
				}
				if (!flag12 && helpText == 64)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 198);
					return;
				}
				if (!flag15 && helpText == 65 && NPC.HasDownedBoss3)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 199);
					return;
				}
				if (!flag18 && helpText == 66 && NPC.HasDownedBoss3)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 200);
					return;
				}
				if (!flag14 && helpText == 67)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 201);
					return;
				}
				if (!flag17 && NPC.HasDownedBoss2 && helpText == 68)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 202);
					return;
				}
				if (!flag16 && Main.InHardMode && helpText == 69)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 203);
					return;
				}
				if (flag7 && helpText == 71)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 204);
					return;
				}
				if (flag8 && helpText == 72)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 205);
					return;
				}
				if ((flag7 || flag8) && helpText == 80)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 206);
					return;
				}
				if (!flag9 && helpText == 201 && !Main.InHardMode && !NPC.HasDownedBoss3 && !NPC.HasDownedBoss2)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 207);
					return;
				}
				if (helpText == 1000 && !NPC.HasDownedBoss1 && !NPC.HasDownedBoss2)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 208);
					return;
				}
				if (helpText == 1001 && !NPC.HasDownedBoss1 && !NPC.HasDownedBoss2)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 209);
					return;
				}
				if (helpText == 1002 && !NPC.HasDownedBoss3)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 210);
					return;
				}
				if (helpText == 1050 && !NPC.HasDownedBoss1)
				{
					if (ActivePlayer.StatLifeMax < 200)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 211);
						return;
					}
					continue;
				}
				if (helpText == 1051 && !NPC.HasDownedBoss1)
				{
					if (ActivePlayer.StatDefense <= 10)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 212);
						return;
					}
					continue;
				}
				if (helpText == 1052 && !NPC.HasDownedBoss1)
				{
					if (ActivePlayer.StatLifeMax >= 200 && ActivePlayer.StatDefense > 10)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 213);
						return;
					}
					continue;
				}
				if (helpText == 1053 && NPC.HasDownedBoss1 && !NPC.HasDownedBoss2)
				{
					if (ActivePlayer.StatLifeMax < 300)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 214);
						return;
					}
					continue;
				}
				if (helpText == 1054 && NPC.HasDownedBoss1 && !NPC.HasDownedBoss2)
				{
					if (ActivePlayer.StatLifeMax >= 300)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 215);
						return;
					}
					continue;
				}
				if (helpText == 1055 && NPC.HasDownedBoss1 && !NPC.HasDownedBoss2)
				{
					if (ActivePlayer.StatLifeMax >= 300)
					{
						npcChatText = Lang.NPCDialog(ActivePlayer, 216);
						return;
					}
					continue;
				}
				if (helpText == 1056 && NPC.HasDownedBoss1 && NPC.HasDownedBoss2 && !NPC.HasDownedBoss3)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 217);
					return;
				}
				if (helpText == 1057 && NPC.HasDownedBoss1 && NPC.HasDownedBoss2 && NPC.HasDownedBoss3 && !Main.InHardMode && ActivePlayer.StatLifeMax < 400)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 218);
					return;
				}
				if (helpText == 1058 && NPC.HasDownedBoss1 && NPC.HasDownedBoss2 && NPC.HasDownedBoss3 && !Main.InHardMode && ActivePlayer.StatLifeMax >= 400)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 219);
					return;
				}
				if (helpText == 1059 && NPC.HasDownedBoss1 && NPC.HasDownedBoss2 && NPC.HasDownedBoss3 && !Main.InHardMode && ActivePlayer.StatLifeMax >= 400)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 220);
					return;
				}
				if (helpText == 1060 && NPC.HasDownedBoss1 && NPC.HasDownedBoss2 && NPC.HasDownedBoss3 && !Main.InHardMode && ActivePlayer.StatLifeMax >= 400)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 221);
					return;
				}
				if (helpText == 1061 && Main.InHardMode)
				{
					npcChatText = Lang.NPCDialog(ActivePlayer, 222);
					return;
				}
				if (helpText == 1062 && Main.InHardMode)
				{
					break;
				}
				if (helpText > 1100)
				{
					helpText = 0;
				}
			}
			npcChatText = Lang.NPCDialog(ActivePlayer, 223);
		}

		public void UpdateNpcChat()
		{
			focusText = null;
			focusText3 = null;
			int num = (MouseTextBrightness * 2 + 255) / 3;
			focusColor = new Color(num, (int)((double)num * (10f / 11f)), num >> 1, num);
			int num2 = ActivePlayer.StatLifeMax - ActivePlayer.statLife;
			if (ActivePlayer.sign >= 0)
			{
				focusText = Lang.InterfaceText[48];
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.DRYAD)
			{
				focusText = Lang.InterfaceText[28];
				focusText3 = Lang.InterfaceText[49];
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GOBLIN_TINKERER)
			{
				focusText = Lang.InterfaceText[28];
				focusText3 = Lang.InterfaceText[19];
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.MERCHANT || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.ARMS_DEALER || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.DEMOLITIONIST || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.CLOTHIER || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.WIZARD || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.MECHANIC || Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.SANTA_CLAUS)
			{
				focusText = Lang.InterfaceText[28];
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.OLD_MAN)
			{
				if (!Main.GameTime.DayTime)
				{
					focusText = Lang.InterfaceText[50];
				}
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GUIDE)
			{
				focusText = Lang.InterfaceText[51];
				if (!Main.IsTutorial())
				{
					focusText3 = Lang.InterfaceText[25];
				}
			}
			else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.NURSE)
			{
				focusText = Lang.InterfaceText[54];
				for (int i = 0; i < Player.MaxNumBuffs; i++)
				{
					if (ActivePlayer.buff[i].IsHealable())
					{
						num2 += 1000;
					}
				}
				if (num2 > 0)
				{
					string text = "";
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = (int)(num2 * 0.75);
					if (num7 < 1)
					{
						num7 = 1;
					}
					num2 = num7;
					if (num7 >= 1000000)
					{
						num3 = num7 / 1000000;
						num7 -= num3 * 1000000;
					}
					if (num7 >= 10000)
					{
						num4 = num7 / 10000;
						num7 -= num4 * 10000;
					}
					if (num7 >= 100)
					{
						num5 = num7 / 100;
						num7 -= num5 * 100;
					}
					if (num7 > 0)
					{
						num6 = num7;
					}
					if (num3 > 0)
					{
						text = num3 + Lang.InterfaceText[15];
					}
					if (num4 > 0)
					{
						text = text + num4 + Lang.InterfaceText[16];
					}
					if (num5 > 0)
					{
						text = text + num5 + Lang.InterfaceText[17];
					}
					if (num6 > 0)
					{
						text = text + num6 + Lang.InterfaceText[18];
					}
					float num8 = MouseTextBrightness * (1f / 255f);
					if (num3 > 0)
					{
						focusColor = new Color((byte)(220f * num8), (byte)(220f * num8), (byte)(198f * num8), MouseTextBrightness);
					}
					else if (num4 > 0)
					{
						focusColor = new Color((byte)(224f * num8), (byte)(201f * num8), (byte)(92f * num8), MouseTextBrightness);
					}
					else if (num5 > 0)
					{
						focusColor = new Color((byte)(181f * num8), (byte)(192f * num8), (byte)(193f * num8), MouseTextBrightness);
					}
					else if (num6 > 0)
					{
						focusColor = new Color((byte)(246f * num8), (byte)(138f * num8), (byte)(96f * num8), MouseTextBrightness);
					}
					focusText = focusText + " (" + text + ")";
				}
			}
			ActivePlayer.releaseUseItem = false;
			if (focusText == null && focusText3 == null)
			{
				npcChatSelectedItem = 1;
			}
			else if (IsLeftButtonTriggered())
			{
				Main.PlaySound(12);
				if (--npcChatSelectedItem < 0)
				{
					npcChatSelectedItem = (sbyte)((focusText3 == null) ? 1 : 2);
				}
			}
			else if (IsRightButtonTriggered())
			{
				Main.PlaySound(12);
				npcChatSelectedItem++;
				if (npcChatSelectedItem == 3 || (npcChatSelectedItem == 2 && focusText3 == null))
				{
					npcChatSelectedItem = 0;
				}
			}
			if (IsButtonTriggered(BTN_NPC_CHAT_CLOSE))
			{
				ActivePlayer.TalkNPC = -1;
				ActivePlayer.sign = -1;
				editSign = false;
				npcChatText = null;
				Main.PlaySound(11);
				ClearButtonTriggers();
			}
			else
			{
				if (!IsButtonTriggered(BTN_NPC_CHAT_SELECT))
				{
					return;
				}
				if (npcChatSelectedItem == 0)
				{
					if (ActivePlayer.sign != -1)
					{
						Main.PlaySound(12);
						editSign = true;
						ClearInput();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.MERCHANT)
					{
						npcChatText = null;
						npcShop = 1;
						Main.Shop[npcShop].SetupShop(npcShop, ActivePlayer);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.ARMS_DEALER)
					{
						npcChatText = null;
						npcShop = 2;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.MECHANIC)
					{
						npcChatText = null;
						npcShop = 8;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.SANTA_CLAUS)
					{
						npcChatText = null;
						npcShop = 9;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.OLD_MAN)
					{
						if (Main.NetMode != (byte)NetModeSetting.CLIENT)
						{
							NPC.SpawnSkeletron();
						}
						else
						{
							NetMessage.CreateMessage0(62);
							NetMessage.SendMessage();
						}
						npcChatText = null;
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.DRYAD)
					{
						npcChatText = null;
						npcShop = 3;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.DEMOLITIONIST)
					{
						npcChatText = null;
						npcShop = 4;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.CLOTHIER)
					{
						npcChatText = null;
						npcShop = 5;
						Main.Shop[npcShop].SetupShop(npcShop, ActivePlayer);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GOBLIN_TINKERER)
					{
						npcChatText = null;
						npcShop = 6;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.WIZARD)
					{
						npcChatText = null;
						npcShop = 7;
						Main.Shop[npcShop].SetupShop(npcShop);
						Main.PlaySound(12);
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GUIDE)
					{
						Main.PlaySound(12);
						HelpText();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.NURSE)
					{
						Main.PlaySound(12);
						if (num2 > 0)
						{
							if (ActivePlayer.BuyItem(num2))
							{
								Main.PlaySound(2, -1, -1, 4);
								ActivePlayer.HealEffect(ActivePlayer.StatLifeMax - ActivePlayer.statLife);
								if (ActivePlayer.statLife < ActivePlayer.StatLifeMax * 0.25f)
								{
									npcChatText = Lang.NPCDialog(ActivePlayer, 227);
								}
								else if (ActivePlayer.statLife < ActivePlayer.StatLifeMax * 0.5f)
								{
									npcChatText = Lang.NPCDialog(ActivePlayer, 228);
								}
								else if (ActivePlayer.statLife < ActivePlayer.StatLifeMax * 0.75f)
								{
									npcChatText = Lang.NPCDialog(ActivePlayer, 229);
								}
								else
								{
									npcChatText = Lang.NPCDialog(ActivePlayer, 230);
								}
								ActivePlayer.statLife = ActivePlayer.StatLifeMax;
								for (int j = 0; j < Player.MaxNumBuffs; j++)
								{
									if (ActivePlayer.buff[j].IsHealable())
									{
										j = ActivePlayer.DelBuff(j);
									}
								}
							}
							else
							{
								switch (Main.Rand.Next(3))
								{
								case 0:
										npcChatText = Lang.NPCDialog(ActivePlayer, 52);
									break;
								case 1:
										npcChatText = Lang.NPCDialog(ActivePlayer, 53);
									break;
								default:
										npcChatText = Lang.NPCDialog(ActivePlayer, 54);
									break;
								}
							}
						}
						else
						{
							switch (Main.Rand.Next(3))
							{
							case 0:
									npcChatText = Lang.NPCDialog(ActivePlayer, 55);
								break;
							case 1:
									npcChatText = Lang.NPCDialog(ActivePlayer, 56);
								break;
							default:
									npcChatText = Lang.NPCDialog(ActivePlayer, 57);
								break;
							}
						}
					}
				}
				else if (npcChatSelectedItem == 1)
				{
					ActivePlayer.TalkNPC = -1;
					ActivePlayer.sign = -1;
					editSign = false;
					npcChatText = null;
					Main.PlaySound(11);
				}
				else if (ActivePlayer.TalkNPC >= 0)
				{
					if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.DRYAD)
					{
						Main.PlaySound(12);
						npcChatText = Lang.DryadEvilGood();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GUIDE)
					{
						npcChatText = null;
						Main.PlaySound(12);
						CraftGuide = true;
						GuideItem.Init();
						OpenInventory();
					}
					else if (Main.NPCSet[ActivePlayer.TalkNPC].Type == (int)NPC.ID.GOBLIN_TINKERER)
					{
						npcChatText = null;
						Main.PlaySound(12);
						reforge = true;
						OpenInventory();
					}
				}
				ClearButtonTriggers();
			}
		}

		private void DrawNpcChat()
		{
			string @string = npcChatText.GetString();
#if !USE_ORIGINAL_CODE
			int ChatBackTexSize = 500;
			int WrapWidth = 470;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.15f);
					WrapWidth = 548;
					break;
				case 2:
					ChatBackTexSize = (int)(ChatBackTexSize * 1.534f);
					WrapWidth = 705;
					break;
			}
#endif

			if (@string != npcCompiledChatText)
			{
				npcCompiledChatText = @string;
#if USE_ORIGINAL_CODE
				npcChatCompiledText = new CompiledText(@string, 470, BoldSmallTextStyle);
#else
				npcChatCompiledText = new CompiledText(@string, WrapWidth, BoldSmallTextStyle);
#endif
			}
			int num = (MouseTextBrightness * 2 + 255) / 3;
#if USE_ORIGINAL_CODE
			int num2 = DrawDialog(textColor: new Color(num, num, num, num), pos: new Vector2(CurrentView.ViewWidth - chatBackTexture.Width >> 1, 100f), backColor: new Color(200, 200, 200, 200), ct: npcChatCompiledText);
#else
			int num2 = DrawDialog(textColor: new Color(num, num, num, num), pos: new Vector2(CurrentView.ViewWidth - ChatBackTexSize >> 1, (100f * Main.ScreenMultiplier)), backColor: new Color(200, 200, 200, 200), ct: npcChatCompiledText);
#endif
			num = MouseTextBrightness;
			int num3 = 180 + (CurrentView.ViewWidth - 800 >> 1);
			int num4 = 128 + num2;

#if !USE_ORIGINAL_CODE
			// Hardcoding these since the 1080p mode calculations are incredibly difficult to decipher with what I got decompiled currently.
			switch (Main.ScreenHeightPtr) // These will bear a striking resemblance to what it should look like in the official modes. (I say this, knowing 720p is custom, but still)
			{
				case 1:
					num3 = 370;
					num4 = 150 + num2;
					break;

				case 2:
					num3 = 602;
					num4 = 215 + num2;
					break;
			}
#endif

			Vector2 pivot = default;
			if (focusText != null)
			{
				pivot = MeasureString(BoldSmallFont, focusText);
				pivot.X *= 0.5f;
				pivot.Y *= 0.5f;
				DrawStringScaled(BoldSmallFont, focusText, new Vector2(num3 + pivot.X , num4 + pivot.Y), focusColor, pivot, (npcChatSelectedItem == 0) ? 1.1f : 0.9f);
			}
			string text = Lang.InterfaceText[52];
			Color c = new Color(num, (int)((double)num * (10f / 11f)), num >> 1, num);

#if USE_ORIGINAL_CODE
			num3 = num3 + (int)(pivot.X * 2f) + 30;
#else
			float AddSpace = 30;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					AddSpace *= 1.15f;
					break;

				case 2:
					AddSpace *= 1.5f;
					break;
			}
			num3 = num3 + (int)(pivot.X * 2f) + (int)AddSpace;
#endif
			Vector2 pivot2 = MeasureString(BoldSmallFont, text);
			pivot2.X *= 0.5f;
			pivot2.Y *= 0.5f;
			DrawStringScaled(BoldSmallFont, text, new Vector2(num3 + pivot2.X, num4 + pivot2.Y), c, pivot2, (npcChatSelectedItem == 1) ? 1.1f : 0.9f);
			if (focusText3 != null)
			{
				num3 = num3 + (int)(pivot2.X * 2f) + 30;
				Vector2 pivot3 = MeasureString(BoldSmallFont, focusText3);
				pivot3.X *= 0.5f;
				pivot3.Y *= 0.5f;
				DrawStringScaled(BoldSmallFont, focusText3, new Vector2(num3 + pivot3.X, num4 + pivot3.Y), c, pivot3, (npcChatSelectedItem == 2) ? 1.1f : 0.9f);
			}
		}

		private void Reforge(int slot, bool isArmor = false)
		{
			if (toolTip.SetPrefix(-3) && ActivePlayer.BuyItem(toolTip.Value))
			{
				int prefix = toolTip.PrefixType;
				toolTip.NetDefaults(toolTip.NetID);
				do
				{
					toolTip.SetPrefix(-2);
				}
				while (toolTip.PrefixType == prefix);
				toolTip.Position.X = ActivePlayer.XYWH.X + (Player.width / 2) - (toolTip.Width >> 1);
				toolTip.Position.Y = ActivePlayer.XYWH.Y + (Player.height / 2) - (toolTip.Height >> 1);
				if (isArmor)
				{
					ref Item reference = ref ActivePlayer.armor[slot];
					reference = toolTip;
				}
				else
				{
					ref Item reference2 = ref ActivePlayer.Inventory[slot];
					reference2 = toolTip;
				}
				Main.PlaySound(2, ActivePlayer.XYWH.X, ActivePlayer.XYWH.Y, 37);
			}
		}

		private void CraftingGuide()
		{
			if (toolTip.Type <= 0 || !toolTip.IsMaterial)
			{
				return;
			}
			GuideItem = toolTip;
			ActiveInvSection = InventorySection.CRAFTING;
			craftingCategory = Recipe.Category.MISC;
			for (int i = 0; i < (int)Recipe.Category.NUM_CATEGORIES; i++)
			{
				NextCraftingCategory();
				if (CurrentRecipeCategory.Count > 0)
				{
					break;
				}
			}
		}

		private bool IsSlotAssignedToQuickAccess(int slot)
		{
			if (quickAccessUp != slot && quickAccessDown != slot && quickAccessLeft != slot)
			{
				return quickAccessRight == slot;
			}
			return true;
		}

		private void UpdateInventory()
		{
			if (inventoryItemX == 9 && inventoryItemY == 6)
			{
				if (!IsButtonTriggered(BTN_INVENTORY_SELECT))
				{
					return;
				}
				if (mouseItem.Type != 0)
				{
					trashItem.Init();
				}
				Item item = mouseItem;
				mouseItem = trashItem;
				trashItem = item;
				mouseItemSrcSection = InventorySection.ITEMS;
				mouseItemSrcX = inventoryItemX;
				mouseItemSrcY = inventoryItemY;
				if (trashItem.Type == 0 || trashItem.Stack < 1)
				{
					trashItem.Init();
				}
				if (mouseItem.NetID == trashItem.NetID && trashItem.Stack != trashItem.MaxStack && mouseItem.Stack != mouseItem.MaxStack)
				{
					if (mouseItem.Stack + trashItem.Stack <= mouseItem.MaxStack)
					{
						trashItem.Stack += mouseItem.Stack;
						mouseItem.Stack = 0;
					}
					else
					{
						short num = (short)(mouseItem.MaxStack - trashItem.Stack);
						trashItem.Stack += num;
						mouseItem.Stack -= num;
					}
				}
				if (mouseItem.Type == 0 || mouseItem.Stack < 1)
				{
					mouseItem.Init();
				}
				if (mouseItem.Type > 0 || trashItem.Type > 0)
				{
					Main.PlaySound(7);
				}
				return;
			}
			bool flag = true;
			int num2;
			switch (inventoryItemY)
			{
			case 4:
				num2 = Player.NumInvSlots + Player.NumAmmoSlots + inventoryItemX - 6;
				flag = mouseItem.CanBePlacedInAmmoSlot();
				break;
			case 5:
				num2 = Player.NumInvSlots + inventoryItemX - 6;
				flag = mouseItem.CanBePlacedInCoinSlot();
				break;
			default:
				num2 = inventoryItemX + inventoryItemY * 10;
				break;
			}
			if (num2 < Player.NumInvSlots && mouseItem.Type == 0)
			{
				if (IsButtonTriggered(Buttons.DPadUp))
				{
					if (quickAccessUp == num2)
					{
						quickAccessUp = -1;
					}
					else
					{
						quickAccessUp = (sbyte)num2;
						if (quickAccessDown == num2)
						{
							quickAccessDown = -1;
						}
						else if (quickAccessLeft == num2)
						{
							quickAccessLeft = -1;
						}
						else if (quickAccessRight == num2)
						{
							quickAccessRight = -1;
						}
					}
					Main.PlaySound(7);
				}
				else if (IsButtonTriggered(Buttons.DPadDown))
				{
					if (quickAccessDown == num2)
					{
						quickAccessDown = -1;
					}
					else
					{
						quickAccessDown = (sbyte)num2;
						if (quickAccessUp == num2)
						{
							quickAccessUp = -1;
						}
						else if (quickAccessLeft == num2)
						{
							quickAccessLeft = -1;
						}
						else if (quickAccessRight == num2)
						{
							quickAccessRight = -1;
						}
					}
					Main.PlaySound(7);
				}
				else if (IsButtonTriggered(Buttons.DPadLeft))
				{
					if (quickAccessLeft == num2)
					{
						quickAccessLeft = -1;
					}
					else
					{
						quickAccessLeft = (sbyte)num2;
						if (quickAccessUp == num2)
						{
							quickAccessUp = -1;
						}
						else if (quickAccessDown == num2)
						{
							quickAccessDown = -1;
						}
						else if (quickAccessRight == num2)
						{
							quickAccessRight = -1;
						}
					}
					Main.PlaySound(7);
				}
				else if (IsButtonTriggered(Buttons.DPadRight))
				{
					if (quickAccessRight == num2)
					{
						quickAccessRight = -1;
					}
					else
					{
						quickAccessRight = (sbyte)num2;
						if (quickAccessUp == num2)
						{
							quickAccessUp = -1;
						}
						else if (quickAccessDown == num2)
						{
							quickAccessDown = -1;
						}
						else if (quickAccessLeft == num2)
						{
							quickAccessLeft = -1;
						}
					}
					Main.PlaySound(7);
				}
			}
			if (IsButtonTriggered(BTN_INVENTORY_SELECT))
			{
				if (reforge)
				{
					Reforge(num2);
				}
				else if (CraftGuide)
				{
					CraftingGuide();
				}
				else
				{
					if (mouseItem.Type != 0 && (!flag || (ActivePlayer.SelectedItem == num2 && ActivePlayer.itemAnimation > 0)))
					{
						return;
					}
					Item item2 = mouseItem;
					mouseItem = ActivePlayer.Inventory[num2];
					ActivePlayer.Inventory[num2] = item2;
					if (ActivePlayer.Inventory[num2].Type == 0 || ActivePlayer.Inventory[num2].Stack < 1)
					{
						ActivePlayer.Inventory[num2].Init();
					}
					bool flag2 = false;
					if (mouseItem.NetID == ActivePlayer.Inventory[num2].NetID && ActivePlayer.Inventory[num2].Stack != ActivePlayer.Inventory[num2].MaxStack && mouseItem.Stack != mouseItem.MaxStack)
					{
						if (mouseItem.Stack + ActivePlayer.Inventory[num2].Stack <= mouseItem.MaxStack)
						{
							ActivePlayer.Inventory[num2].Stack += mouseItem.Stack;
							mouseItem.Init();
						}
						else
						{
							short num3 = (short)(mouseItem.MaxStack - ActivePlayer.Inventory[num2].Stack);
							ActivePlayer.Inventory[num2].Stack += num3;
							mouseItem.Stack -= num3;
							flag2 = true;
						}
					}
					if (mouseItem.Type > 0 && item2.Type > 0 && !flag2 && mouseItemSrcSection == InventorySection.ITEMS && mouseItemSrcX < 10 && mouseItemSrcY < 4)
					{
						int num4 = mouseItemSrcX + mouseItemSrcY * 10;
						if (ActivePlayer.Inventory[num4].Type == 0)
						{
							ref Item reference = ref ActivePlayer.Inventory[num4];
							reference = mouseItem;
							mouseItem.Init();
						}
						if (quickAccessUp == num4)
						{
							quickAccessUp = (sbyte)num2;
						}
						else if (quickAccessDown == num4)
						{
							quickAccessDown = (sbyte)num2;
						}
						else if (quickAccessLeft == num4)
						{
							quickAccessLeft = (sbyte)num2;
						}
						else if (quickAccessRight == num4)
						{
							quickAccessRight = (sbyte)num2;
						}
					}
					mouseItemSrcSection = InventorySection.ITEMS;
					mouseItemSrcX = inventoryItemX;
					mouseItemSrcY = inventoryItemY;
					if (mouseItem.Type > 0 || ActivePlayer.Inventory[num2].Type > 0)
					{
						Main.PlaySound(7);
					}
				}
			}
			else if (PadState.IsButtonDown(BTN_INVENTORY_ACTION))
			{
				if (gpPrevState.IsButtonUp(BTN_INVENTORY_ACTION))
				{
					if (ActivePlayer.Inventory[num2].Type >= (int)Item.ID.BLUE_PRESENT && ActivePlayer.Inventory[num2].Type <= (int)Item.ID.YELLOW_PRESENT)
					{
						Main.PlaySound(7);
						stackSplit = 30;
						int num5 = Main.Rand.Next(14);
						if (num5 == 0 && Main.InHardMode)
						{
							ActivePlayer.Inventory[num2].SetDefaults((int)Item.ID.SNOW_GLOBE);
							return;
						}
						ActivePlayer.Inventory[num2].SetDefaults((num5 <= 7) ? (int)Item.ID.CANDY_CANE_BLOCK : (int)Item.ID.GREEN_CANDY_CANE_BLOCK);
						ActivePlayer.Inventory[num2].Stack = (short)Main.Rand.Next(20, 50);
					}
					else if (ActivePlayer.Inventory[num2].IsEquipable())
					{
						ref Item reference2 = ref ActivePlayer.Inventory[num2];
						reference2 = ActivePlayer.armorSwap(ref ActivePlayer.Inventory[num2]);
					}
				}
				else if (stackSplit <= 1 && ActivePlayer.Inventory[num2].MaxStack > 1 && ActivePlayer.Inventory[num2].Type > 0 && (mouseItem.NetID == ActivePlayer.Inventory[num2].NetID || mouseItem.Type == 0) && (mouseItem.Stack < mouseItem.MaxStack || mouseItem.Type == 0))
				{
					if (mouseItem.Type == 0)
					{
						mouseItem = ActivePlayer.Inventory[num2];
						mouseItem.Stack = 0;
						mouseItemSrcSection = InventorySection.ITEMS;
						mouseItemSrcX = inventoryItemX;
						mouseItemSrcY = inventoryItemY;
					}
					mouseItem.Stack++;
					ActivePlayer.Inventory[num2].Stack--;
					if (ActivePlayer.Inventory[num2].Stack <= 0)
					{
						ActivePlayer.Inventory[num2].Init();
					}
					Main.PlaySound(12);
					if (stackSplit == 0)
					{
						stackSplit = 15;
					}
					else
					{
						stackSplit = stackDelay;
					}
				}
			}
			else
			{
				if (mouseItem.Type != 0 || reforge || !IsButtonTriggered(BTN_INVENTORY_SELL_OR_TRASH) || ActivePlayer.Inventory[num2].Type <= 0)
				{
					return;
				}
				if (npcShop > 0 && !ActivePlayer.Inventory[num2].CanBePlacedInCoinSlot())
				{
					if (ActivePlayer.SellItem(ActivePlayer.Inventory[num2].Value, ActivePlayer.Inventory[num2].Stack))
					{
						Main.Shop[npcShop].AddShop(ref ActivePlayer.Inventory[num2]);
						ActivePlayer.Inventory[num2].Init();
						Main.PlaySound(18);
					}
					else if (ActivePlayer.Inventory[num2].Value == 0)
					{
						Main.Shop[npcShop].AddShop(ref ActivePlayer.Inventory[num2]);
						ActivePlayer.Inventory[num2].Init();
						Main.PlaySound(7);
					}
				}
				else
				{
					Main.PlaySound(7);
					trashItem = ActivePlayer.Inventory[num2];
					ActivePlayer.Inventory[num2].Init();
				}
			}
		}

		// Let the conditional games, begin...
		private void DrawInventory(int itemsSectionX, int itemsSectionY)
		{
			// There exist various rounding issues when the game is run in 1080p leading to weird instances where a number may be ±1 from the intended x2 of the original 540p number.
			Vector2 pos = default;
			Color color = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
			inventoryScale = 149f / 160f;
#if USE_ORIGINAL_CODE
			DrawStringRC(SmallFont, Lang.InterfaceText[3], itemsSectionX + 469, itemsSectionY + 338, Color.White);
			int num = 469 + itemsSectionX;
			int num2 = 312 + itemsSectionY;
#else
			inventoryScale *= Main.ScreenMultiplier;
			int SectionXAdjust = (int)(469 * Main.ScreenMultiplier);

#if VERSION_103 || VERSION_FINAL
			SectionXAdjust -= (int)(4 * Main.ScreenMultiplier);
#endif

			DrawStringRC(SmallFont, Lang.InterfaceText[3], itemsSectionX + SectionXAdjust, (int)(itemsSectionY + (338 * Main.ScreenMultiplier)), Color.White);
			int num = (int)((469 * Main.ScreenMultiplier) + itemsSectionX);
			int num2 = (int)((312 * Main.ScreenMultiplier) + itemsSectionY);
#endif
			if (inventoryItemX == 9 && inventoryItemY == 6)
			{
				DrawInventoryCursor(num, num2, inventoryScale);
				toolTip = trashItem;
			}
			else
			{
				SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK7, num, num2, color, inventoryScale);
			}
			if (trashItem.Type == 0 || trashItem.Stack == 0)
			{
				pos.X = num + 26f * inventoryScale;
				pos.Y = num2 + 26f * inventoryScale;
				SpriteSheet<_sheetSprites>.DrawScaled((int)_sheetSprites.ID.TRASH, ref pos, new Color(100, 100, 100, 100), inventoryScale);
			}
			else
			{
				DrawInventoryItem(ref trashItem, num, num2, Color.White, StackType.INVENTORY);
			}
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
#if USE_ORIGINAL_CODE
					int x = itemsSectionX + (int)((float)i * 52.15f);
					int y = itemsSectionY + (int)((float)j * 52.15f);
#else
					int x = itemsSectionX + (int)(i * (52.15f * Main.ScreenMultiplier));
					int y = itemsSectionY + (int)(j * (52.15f * Main.ScreenMultiplier));
#endif
					int num3 = i + j * 10;
					if (inventoryItemX == i && inventoryItemY == j)
					{
						toolTip = ActivePlayer.Inventory[num3];
						DrawInventoryCursor(x, y, inventoryScale);
					}
					else
					{
						Color c = color;
						int id;
						if (!IsSlotAssignedToQuickAccess(num3))
						{
							id = ((j == 0) ? (int)_sheetSprites.ID.INVENTORY_BACK9 : (int)_sheetSprites.ID.INVENTORY_BACK);
						}
						else
						{
							id = (int)_sheetSprites.ID.INVENTORY_BACK12;
							c = mouseTextColor;
						}
						SpriteSheet<_sheetSprites>.DrawTL(id, x, y, c, inventoryScale);
					}
					if (ActivePlayer.Inventory[num3].Type > 0 && ActivePlayer.Inventory[num3].Stack > 0)
					{
						StackType stackType;
						Color itemColor;
						if ((CraftGuide && !ActivePlayer.Inventory[num3].IsMaterial) || (reforge && !ActivePlayer.Inventory[num3].SetPrefix(-3)))
						{
							stackType = StackType.NONE;
							itemColor = DISABLED_COLOR;
						}
						else
						{
							stackType = StackType.INVENTORY;
							itemColor = new Color(255, 255, 255, 255);
						}
						DrawInventoryItem(ref ActivePlayer.Inventory[num3], x, y, itemColor, stackType);
					}
				}
			}

#if USE_ORIGINAL_CODE
			DrawStringRC(SmallFont, Lang.InterfaceText[26], itemsSectionX + 312, itemsSectionY + 286, Color.White);
			for (int k = 0; k < 4; k++)
			{
				int x2 = (int)((6 + k) * 52.15f) + itemsSectionX;
				int y2 = 260 + itemsSectionY;
				int num4 = k + Player.NumInvSlots;
				if (inventoryItemY == 5 && inventoryItemX - 6 == k)
				{
					DrawInventoryCursor(x2, y2, inventoryScale);
					toolTip = ActivePlayer.Inventory[num4];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL(451, x2, y2, color, inventoryScale);
				}
				if (ActivePlayer.Inventory[num4].Type > 0 && ActivePlayer.Inventory[num4].Stack > 0)
				{
					DrawInventoryItem(ref ActivePlayer.Inventory[num4], x2, y2, Color.White, StackType.INVENTORY);
				}
			}
			DrawStringRC(SmallFont, Lang.InterfaceText[27], itemsSectionX + 312, itemsSectionY + 234, Color.White);
#else
			int SectionXAdjust2 = (int)(312 * Main.ScreenMultiplier);
#if VERSION_103 || VERSION_FINAL
			SectionXAdjust2 -= (int)(4 * Main.ScreenMultiplier);
#endif

			DrawStringRC(SmallFont, Lang.InterfaceText[26], itemsSectionX + SectionXAdjust2, (int)(itemsSectionY + (286 * Main.ScreenMultiplier)), Color.White); // minus 10 from the x for this, inter[27] and inter[3]
			for (int k = 0; k < 4; k++)
			{
				int x2 = (int)((6 + k) * (52.15f * Main.ScreenMultiplier)) + itemsSectionX;
				int y2 = (int)((260 * Main.ScreenMultiplier) + itemsSectionY);
				int num4 = k + Player.NumInvSlots;
				if (inventoryItemY == 5 && inventoryItemX - 6 == k)
				{
					DrawInventoryCursor(x2, y2, inventoryScale);
					toolTip = ActivePlayer.Inventory[num4];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK, x2, y2, color, inventoryScale);
				}
				if (ActivePlayer.Inventory[num4].Type > 0 && ActivePlayer.Inventory[num4].Stack > 0)
				{
					DrawInventoryItem(ref ActivePlayer.Inventory[num4], x2, y2, Color.White, StackType.INVENTORY);
				}
			}
			DrawStringRC(SmallFont, Lang.InterfaceText[27], itemsSectionX + SectionXAdjust2, (int)(itemsSectionY + (234 * Main.ScreenMultiplier)), Color.White);
#endif

			for (int l = 0; l < 4; l++)
			{
#if USE_ORIGINAL_CODE
				int x3 = (int)((float)(6 + l) * 52.15f) + itemsSectionX;
				int y3 = 208 + itemsSectionY;
#else
				int x3 = (int)((6 + l) * (52.15f * Main.ScreenMultiplier)) + itemsSectionX;
				int y3 = (int)((208 * Main.ScreenMultiplier) + itemsSectionY);
#endif
				int num5 = l + Player.NumInvSlots + Player.NumAmmoSlots;
				if (ActiveInvSection == InventorySection.ITEMS && inventoryItemY == 4 && inventoryItemX - 6 == l)
				{
					DrawInventoryCursor(x3, y3, inventoryScale);
					toolTip = ActivePlayer.Inventory[num5];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK, x3, y3, color, inventoryScale);
				}
				if (ActivePlayer.Inventory[num5].Type > 0 && ActivePlayer.Inventory[num5].Stack > 0)
				{
					StackType stackType2;
					Color itemColor2;
					if ((CraftGuide && !ActivePlayer.Inventory[num5].IsMaterial) || reforge)
					{
						stackType2 = StackType.NONE;
						itemColor2 = DISABLED_COLOR;
					}
					else
					{
						stackType2 = StackType.INVENTORY;
						itemColor2 = new Color(255, 255, 255, 255);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num5], x3, y3, itemColor2, stackType2);
				}
			}

#if USE_ORIGINAL_CODE
			DrawQuickAccess(-1, itemsSectionX + 26, itemsSectionY + 234, 255, StackType.INVENTORY);
			UpdateToolTipText(null);
			DrawToolTip(itemsSectionX + INVENTORY_W - TOOLTIP_W - 8, itemsSectionY + 8, 344);
#else
			DrawQuickAccess(-1, (int)(itemsSectionX + (26 * Main.ScreenMultiplier)), (int)(itemsSectionY + (234 * Main.ScreenMultiplier)), 255, StackType.INVENTORY);
			UpdateToolTipText(null);
			int Offset = (int)(8 * Main.ScreenMultiplier);
			DrawToolTip(itemsSectionX + INVENTORY_W - TOOLTIP_W - Offset, itemsSectionY + Offset, (int)(344 * Main.ScreenMultiplier));
#endif
			DrawControlsInventory();
		}

		public int UpdateQuickAccess()
		{
			int result = -1;
			int num = quickAccessUp;
			if (num >= 0)
			{
				if (ActivePlayer.Inventory[num].Type == 0)
				{
					quickAccessUp = -1;
				}
				else if (PadState.DPad.Up == ButtonState.Pressed && gpPrevState.DPad.Up == ButtonState.Released)
				{
					result = num;
				}
			}
			num = quickAccessDown;
			if (num >= 0)
			{
				if (ActivePlayer.Inventory[num].Type == 0)
				{
					quickAccessDown = -1;
				}
				else if (PadState.DPad.Down == ButtonState.Pressed && gpPrevState.DPad.Down == ButtonState.Released)
				{
					result = num;
				}
			}
			num = quickAccessLeft;
			if (num >= 0)
			{
				if (ActivePlayer.Inventory[num].Type == 0)
				{
					quickAccessLeft = -1;
				}
				else if (PadState.DPad.Left == ButtonState.Pressed && gpPrevState.DPad.Left == ButtonState.Released)
				{
					result = num;
				}
			}
			num = quickAccessRight;
			if (num >= 0)
			{
				if (ActivePlayer.Inventory[num].Type == 0)
				{
					quickAccessRight = -1;
				}
				else if (PadState.DPad.Right == ButtonState.Pressed && gpPrevState.DPad.Right == ButtonState.Released)
				{
					result = num;
				}
			}
			return result;
		}

		private void DrawQuickAccess(int selectedItem, int x, int y, int alpha, StackType stackType)
		{
			Color color = new Color(alpha, alpha, alpha, alpha);
			Color color2 = new Color(0, 0, 0, alpha >> 1);
			alpha = alpha * MouseTextBrightness >> 8;
			Color c = new Color(alpha, alpha, alpha, alpha);
#if USE_ORIGINAL_CODE
			SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.DPAD, x, y, color); // This draws the D-Pad graphic from the actual spritesheet; Later versions had this as a separate file.
#else       // 1080p mode uses a larger sprite but since we are assuming X360 assets, this is the best we can do.                                                                                                            
			int xOff = 56;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					xOff = 75;
					break;
				case 2:
					xOff = 112;
					break;
			}
			int yOff = xOff;
			Main.SpriteBatch.Draw(dpadTexture, new Vector2(x + xOff, y + yOff), new Rectangle(0, 0, 112, 112), color, 0f, new Vector2(112 >> 1, 112 >> 1), Main.ScreenMultiplier, SpriteEffects.None, 0f);
#endif

#if USE_ORIGINAL_CODE
			inventoryScale = 1f;
			int num = quickAccessUp;
			if (num >= 0)
			{
				if (selectedItem == num)
				{
					SpriteSheet<_sheetSprites>.Draw(448, x + 32 + 4, y - 4, color2);
					SpriteSheet<_sheetSprites>.Draw(448, x + 32, y - 4 - 4, c);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 32 + 4, y - 4, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 32, y - 4 - 4, color, stackType);
				}
			}
			num = quickAccessDown;
			if (num >= 0)
			{
				if (selectedItem == num)
				{
					SpriteSheet<_sheetSprites>.Draw(448, x + 32 + 4, y + 112 - 42 + 4, color2);
					SpriteSheet<_sheetSprites>.Draw(448, x + 32, y + 112 - 42, c);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 32 + 4, y + 112 - 42 + 4, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 32, y + 112 - 42, color, stackType);
				}
			}
			num = quickAccessLeft;
			if (num >= 0)
			{
				if (selectedItem == num)
				{
					SpriteSheet<_sheetSprites>.Draw(448, x - 4, y + 30 + 4, color2);
					SpriteSheet<_sheetSprites>.Draw(448, x - 4 - 4, y + 30, c);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x - 4, y + 30 + 4, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x - 4 - 4, y + 30, color, stackType);
				}
			}
			num = quickAccessRight;
			if (num < 0)
			{
				return;
			}
			if (selectedItem == num)
			{
				SpriteSheet<_sheetSprites>.Draw(448, x + 112 - 42 + 4, y + 30 + 4, color2);
				SpriteSheet<_sheetSprites>.Draw(448, x + 112 - 42, y + 30, c);
			}
			if (ActivePlayer.Inventory[num].Type > 0)
			{
				if (selectedItem != num)
				{
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 112 - 42 + 4, y + 30 + 4, color2, stackType);
				}
				DrawInventoryItem(ref ActivePlayer.Inventory[num], x + 112 - 42, y + 30, color, stackType);
			}
#else
			inventoryScale = Main.ScreenMultiplier;
			int num = quickAccessUp;
			int MidXOffset = (int)(32 * Main.ScreenMultiplier);
			int MidYOffset = (int)(30 * Main.ScreenMultiplier);
			int FarOffset = (int)(70 * Main.ScreenMultiplier);
			int OffsetAdd = (int)(4 * Main.ScreenMultiplier);
			if (num >= 0)
			{
				if (selectedItem == num)
				{ // These were initially done with .Draw rather than .DrawTL, which lacked scaling needed for 720p and 1080p.
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + MidXOffset + OffsetAdd, y - OffsetAdd, color2, Main.ScreenMultiplier);
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + MidXOffset, y - (OffsetAdd * 2), c, Main.ScreenMultiplier);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x + MidXOffset + OffsetAdd, y - OffsetAdd, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + MidXOffset, y - (OffsetAdd * 2), color, stackType);
				}
			}
			num = quickAccessDown;
			if (num >= 0)
			{
				if (selectedItem == num)
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + MidXOffset + OffsetAdd, y + FarOffset + OffsetAdd, color2, Main.ScreenMultiplier);
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + MidXOffset, y + FarOffset, c, Main.ScreenMultiplier);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x + MidXOffset + OffsetAdd, y + FarOffset + OffsetAdd, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + MidXOffset, y + FarOffset, color, stackType);
				}
			}
			num = quickAccessLeft;
			if (num >= 0)
			{
				if (selectedItem == num)
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x - OffsetAdd, y + MidYOffset + OffsetAdd, color2, Main.ScreenMultiplier);
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x - (OffsetAdd * 2), y + MidYOffset, c, Main.ScreenMultiplier);
				}
				if (ActivePlayer.Inventory[num].Type > 0)
				{
					if (selectedItem != num)
					{
						DrawInventoryItem(ref ActivePlayer.Inventory[num], x - OffsetAdd, y + MidYOffset + OffsetAdd, color2, stackType);
					}
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x - (OffsetAdd * 2), y + MidYOffset, color, stackType);
				}
			}
			num = quickAccessRight;
			if (num < 0)
			{
				return;
			}
			if (selectedItem == num)
			{
				SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + FarOffset + OffsetAdd, y + MidYOffset + OffsetAdd, color2, Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, x + FarOffset, y + MidYOffset, c, Main.ScreenMultiplier);
			}
			if (ActivePlayer.Inventory[num].Type > 0)
			{
				if (selectedItem != num)
				{
					DrawInventoryItem(ref ActivePlayer.Inventory[num], x + FarOffset + OffsetAdd, y + MidYOffset + OffsetAdd, color2, stackType);
				}
				DrawInventoryItem(ref ActivePlayer.Inventory[num], x + FarOffset, y + MidYOffset, color, stackType);
			}
#endif
		}

		private void UpdateStorage()
		{
			if (inventoryChestX < 0)
			{
				if (!IsButtonTriggered(BTN_INVENTORY_SELECT))
				{
					return;
				}
				switch (inventoryChestY)
				{
				case 1:
					if (ActivePlayer.PlayerChest >= 0)
					{
							Main.ChestSet[ActivePlayer.PlayerChest].LootAll(ActivePlayer);
					}
					else if (ActivePlayer.PlayerChest == -3)
					{
						ActivePlayer.safe.LootAll(ActivePlayer);
					}
					else
					{
						ActivePlayer.bank.LootAll(ActivePlayer);
					}
					break;
				case 2:
					if (ActivePlayer.PlayerChest >= 0)
					{
							Main.ChestSet[ActivePlayer.PlayerChest].Deposit(ActivePlayer);
					}
					else if (ActivePlayer.PlayerChest == -3)
					{
						ActivePlayer.safe.Deposit(ActivePlayer);
					}
					else
					{
						ActivePlayer.bank.Deposit(ActivePlayer);
					}
					break;
				case 3:
					if (ActivePlayer.PlayerChest >= 0)
					{
							Main.ChestSet[ActivePlayer.PlayerChest].QuickStack(ActivePlayer);
					}
					else if (ActivePlayer.PlayerChest == -3)
					{
						ActivePlayer.safe.QuickStack(ActivePlayer);
					}
					else
					{
						ActivePlayer.bank.QuickStack(ActivePlayer);
					}
					break;
				}
				return;
			}
			int chest = ActivePlayer.PlayerChest;
			Chest chest2;
			switch (chest)
			{
			case -2:
				chest2 = ActivePlayer.bank;
				break;
			case -3:
				chest2 = ActivePlayer.safe;
				break;
			default:
				chest2 = Main.ChestSet[chest];
				break;
			}
			int num = inventoryChestX + inventoryChestY * 5;
			if (IsButtonTriggered(BTN_INVENTORY_SELECT))
			{
				if (ActivePlayer.SelectedItem == num && ActivePlayer.itemAnimation > 0)
				{
					return;
				}
				Item item = mouseItem;
				mouseItem = chest2.ItemSet[num];
				mouseItemSrcSection = InventorySection.CHEST;
				mouseItemSrcX = inventoryChestX;
				mouseItemSrcY = inventoryChestY;
				chest2.ItemSet[num] = item;
				if (chest2.ItemSet[num].Type == 0 || chest2.ItemSet[num].Stack < 1)
				{
					chest2.ItemSet[num].Init();
				}
				if (mouseItem.NetID == chest2.ItemSet[num].NetID && chest2.ItemSet[num].Stack != chest2.ItemSet[num].MaxStack && mouseItem.Stack != mouseItem.MaxStack)
				{
					if (mouseItem.Stack + chest2.ItemSet[num].Stack <= mouseItem.MaxStack)
					{
						chest2.ItemSet[num].Stack += mouseItem.Stack;
						mouseItem.Stack = 0;
					}
					else
					{
						short num2 = (short)(mouseItem.MaxStack - chest2.ItemSet[num].Stack);
						chest2.ItemSet[num].Stack += num2;
						mouseItem.Stack -= num2;
					}
				}
				if (mouseItem.Type == 0 || mouseItem.Stack < 1)
				{
					mouseItem.Init();
				}
				if (mouseItem.Type > 0 || chest2.ItemSet[num].Type > 0)
				{
					Main.PlaySound(7);
				}
				if (chest >= 0)
				{
					NetMessage.CreateMessage2(32, chest, num);
					NetMessage.SendMessage();
				}
			}
			else if (IsButtonTriggered(BTN_INVENTORY_ACTION) && chest2.ItemSet[num].IsEquipable())
			{
				ref Item reference = ref chest2.ItemSet[num];
				reference = ActivePlayer.armorSwap(ref chest2.ItemSet[num]);
				if (chest >= 0)
				{
					NetMessage.CreateMessage2(32, chest, num);
					NetMessage.SendMessage();
				}
			}
			else if (stackSplit <= 1 && PadState.IsButtonDown(BTN_INVENTORY_ACTION) && chest2.ItemSet[num].MaxStack > 1 && (mouseItem.NetID == chest2.ItemSet[num].NetID || mouseItem.Type == 0) && (mouseItem.Stack < mouseItem.MaxStack || mouseItem.Type == 0))
			{
				if (mouseItem.Type == 0)
				{
					mouseItem = chest2.ItemSet[num];
					mouseItem.Stack = 0;
					mouseItemSrcSection = InventorySection.CHEST;
					mouseItemSrcX = inventoryChestX;
					mouseItemSrcY = inventoryChestY;
				}
				mouseItem.Stack++;
				chest2.ItemSet[num].Stack--;
				if (chest2.ItemSet[num].Stack <= 0)
				{
					chest2.ItemSet[num].Init();
				}
				Main.PlaySound(12);
				if (stackSplit == 0)
				{
					stackSplit = 15;
				}
				else
				{
					stackSplit = stackDelay;
				}
				if (chest >= 0)
				{
					NetMessage.CreateMessage2(32, chest, num);
					NetMessage.SendMessage();
				}
			}
		}

		private void DrawStorage(int INVENTORY_X, int INVENTORY_Y)
		{
			int chest = ActivePlayer.PlayerChest;
			Chest chest2;
			switch (chest)
			{
			case -1:
				return;
			case -2:
				chest2 = ActivePlayer.bank;
				break;
			case -3:
				chest2 = ActivePlayer.safe;
				break;
			default:
				chest2 = Main.ChestSet[chest];
#if (!VERSION_INITIAL || IS_PATCHED)
                if (chest2 == null)
                {
                    return;
                }
#endif
                break;
            }
            Color c = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
#if USE_ORIGINAL_CODE
			inventoryScale = 1f;
			int x = 112 + INVENTORY_X - 56;
			int num = 56 + INVENTORY_Y + 56;
			int id = ((inventoryChestX < 0 && inventoryChestY == 1) ? 448 : 451);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = 1085;
			DrawInventoryItem(id, x, num, Color.White);
			num += 56;
			id = ((inventoryChestX < 0 && inventoryChestY == 2) ? 448 : 451);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = 213;
			DrawInventoryItem(id, x, num, Color.White);
			num += 56;
			id = ((inventoryChestX < 0 && inventoryChestY == 3) ? 448 : 451);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = 1469;
			DrawInventoryItem(id, x, num, Color.White);
			if (inventoryChestX < 0)
			{
				toolTip.Init();
			}
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					x = 112 + INVENTORY_X + i * 56;
					num = 56 + INVENTORY_Y + j * 56;
#else
			inventoryScale = Main.ScreenMultiplier;
			int Offset = (int)(56 * inventoryScale);
			int x = (Offset * 2) + INVENTORY_X - Offset;
			int num = Offset + INVENTORY_Y + Offset;
			int id = ((inventoryChestX < 0 && inventoryChestY == 1) ? (int)_sheetSprites.ID.INVENTORY_BACK10 : (int)_sheetSprites.ID.INVENTORY_BACK);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = (int)_sheetSprites.ID.LOOT;
			DrawInventoryItem(id, x, num, Color.White);
			num += Offset;
			id = ((inventoryChestX < 0 && inventoryChestY == 2) ? (int)_sheetSprites.ID.INVENTORY_BACK10 : (int)_sheetSprites.ID.INVENTORY_BACK);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = (int)_sheetSprites.ID.DEPOSIT;
			DrawInventoryItem(id, x, num, Color.White);
			num += Offset;
			id = ((inventoryChestX < 0 && inventoryChestY == 3) ? (int)_sheetSprites.ID.INVENTORY_BACK10 : (int)_sheetSprites.ID.INVENTORY_BACK);
			SpriteSheet<_sheetSprites>.DrawTL(id, x, num, c, inventoryScale);
			id = (int)_sheetSprites.ID.QUICKSTACK;
			DrawInventoryItem(id, x, num, Color.White);
			if (inventoryChestX < 0)
			{
				toolTip.Init();
			}
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					x = (Offset * 2) + INVENTORY_X + i * Offset;
					num = Offset + INVENTORY_Y + j * Offset;
#endif
					int num2 = i + j * 5;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK5, x, num, c, inventoryScale);
					if (inventoryChestX == i && inventoryChestY == j)
					{
						DrawInventoryCursor(x, num, inventoryScale);
						toolTip = chest2.ItemSet[num2];
					}
					if (chest2.ItemSet[num2].Type > 0 && chest2.ItemSet[num2].Stack > 0)
					{
						Color white = Color.White; // stunner
						DrawInventoryItem(ref chest2.ItemSet[num2], x, num, white, StackType.INVENTORY);
					}
				}
			}
			UpdateToolTipText(null);
#if USE_ORIGINAL_CODE
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - 8, INVENTORY_Y + 8, 344);
#else
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - (int)(8 * Main.ScreenMultiplier), INVENTORY_Y + (int)(8 * Main.ScreenMultiplier), (int)(344 * Main.ScreenMultiplier));
#endif
			DrawControlsInventory();
		}

		private void UpdateShop()
		{
			int num = inventoryChestX + inventoryChestY * 5;
			if (IsButtonTriggered(BTN_INVENTORY_SELECT))
			{
				if (mouseItem.Type == 0)
				{
					if ((ActivePlayer.SelectedItem == num && ActivePlayer.itemAnimation > 0) || !ActivePlayer.BuyItem(Main.Shop[npcShop].ItemSet[num].Value))
					{
						return;
					}
					if (Main.Shop[npcShop].ItemSet[num].OnlyBuyOnce)
					{
						int prefix = Main.Shop[npcShop].ItemSet[num].PrefixType;
						mouseItem.NetDefaults(Main.Shop[npcShop].ItemSet[num].NetID);
						mouseItem.SetPrefix(prefix);
					}
					else
					{
						mouseItem.NetDefaults(Main.Shop[npcShop].ItemSet[num].NetID);
						mouseItem.SetPrefix(-1);
					}
					mouseItem.Position.X = ActivePlayer.Position.X + (Player.width / 2) - (mouseItem.Width >> 1);
					mouseItem.Position.Y = ActivePlayer.Position.Y + (Player.height / 2) - (mouseItem.Height >> 1);
					if (Main.Shop[npcShop].ItemSet[num].OnlyBuyOnce)
					{
						Main.Shop[npcShop].ItemSet[num].Stack--;
						if (Main.Shop[npcShop].ItemSet[num].Stack <= 0)
						{
							Main.Shop[npcShop].ItemSet[num].Init();
						}
					}
					Main.PlaySound(18);
				}
				else if (Main.Shop[npcShop].ItemSet[num].Type == 0)
				{
					if (ActivePlayer.SellItem(mouseItem.Value, mouseItem.Stack))
					{
						Main.Shop[npcShop].AddShop(ref mouseItem);
						mouseItem.Stack = 0;
						mouseItem.Type = 0;
						Main.PlaySound(18);
					}
					else if (mouseItem.Value == 0)
					{
						Main.Shop[npcShop].AddShop(ref mouseItem);
						mouseItem.Stack = 0;
						mouseItem.Type = 0;
						Main.PlaySound(7);
					}
				}
			}
			else
			{
				if (stackSplit > 1 || !PadState.IsButtonDown(BTN_INVENTORY_ACTION) || (mouseItem.NetID != Main.Shop[npcShop].ItemSet[num].NetID && mouseItem.Type != 0) || (mouseItem.Stack >= mouseItem.MaxStack && mouseItem.Type != 0) || !ActivePlayer.BuyItem(Main.Shop[npcShop].ItemSet[num].Value))
				{
					return;
				}
				Main.PlaySound(18);
				if (mouseItem.Type == 0)
				{
					mouseItem.NetDefaults(Main.Shop[npcShop].ItemSet[num].NetID);
					mouseItem.Stack = 0;
				}
				mouseItem.Stack++;
				if (stackSplit == 0)
				{
					stackSplit = 15;
				}
				else
				{
					stackSplit = stackDelay;
				}
				if (Main.Shop[npcShop].ItemSet[num].OnlyBuyOnce)
				{
					Main.Shop[npcShop].ItemSet[num].Stack--;
					if (Main.Shop[npcShop].ItemSet[num].Stack <= 0)
					{
						Main.Shop[npcShop].ItemSet[num].Init();
					}
				}
			}
		}

		private void DrawShop(int INVENTORY_X, int INVENTORY_Y)
		{
			Color c = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
#if USE_ORIGINAL_CODE
			inventoryScale = 1f;
#else
			inventoryScale = Main.ScreenMultiplier;
#endif
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{

#if USE_ORIGINAL_CODE
					int x = 112 + INVENTORY_X + i * 56;
					int y = 56 + INVENTORY_Y + j * 56;
#else
					int Offset = (int)(56 * Main.ScreenMultiplier);
					int x = (Offset * 2) + INVENTORY_X + i * Offset;
					int y = Offset + INVENTORY_Y + j * Offset;
#endif

					int num = i + j * 5;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK6, x, y, c, inventoryScale);
					if (ActiveInvSection == InventorySection.CHEST && inventoryChestX == i && inventoryChestY == j)
					{
						DrawInventoryCursor(x, y, inventoryScale);
						toolTip = Main.Shop[npcShop].ItemSet[num];
						toolTip.CanBuy = true;
					}
					if (Main.Shop[npcShop].ItemSet[num].Type > 0 && Main.Shop[npcShop].ItemSet[num].Stack > 0)
					{
						Color white = Color.White;
						DrawInventoryItem(ref Main.Shop[npcShop].ItemSet[num], x, y, white, StackType.INVENTORY);
					}
				}
			}
			UpdateToolTipText(null);
#if USE_ORIGINAL_CODE
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - 8, INVENTORY_Y + 8, 344);
#else
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - (int)(8 * Main.ScreenMultiplier), INVENTORY_Y + (int)(8 * Main.ScreenMultiplier), (int)(344 * Main.ScreenMultiplier));
#endif
			DrawControlsShop();
		}

		private void UpdateEquip()
		{
			if (inventoryEquipY == 0)
			{
				if (IsButtonTriggered(BTN_INVENTORY_SELECT))
				{
					int num = inventoryEquipX + inventoryBuffX;
					num %= Player.MaxNumBuffs;
					if (ActivePlayer.buff[num].Time > 0 && !ActivePlayer.buff[num].IsDebuff())
					{
						ActivePlayer.DelBuff(num);
						Main.PlaySound(12);
					}
				}
				return;
			}
			int num2 = ((inventoryEquipY == 4) ? (3 + inventoryEquipX) : ((inventoryEquipX != 0) ? (inventoryEquipY + 7) : (inventoryEquipY - 1)));
			if (IsButtonTriggered(BTN_INVENTORY_SELECT))
			{
				if (reforge)
				{
					Reforge(num2, isArmor: true);
				}
				else if (CraftGuide)
				{
					CraftingGuide();
				}
				else if (mouseItem.Type == 0 || (mouseItem.HeadSlot >= 0 && (num2 == 0 || num2 == 8)) || (mouseItem.BodySlot >= 0 && (num2 == 1 || num2 == 9)) || (mouseItem.LegSlot >= 0 && (num2 == 2 || num2 == 10)) || (mouseItem.IsAccessory && num2 > 2 && !AccCheck(ref mouseItem, num2)))
				{
					Item item = mouseItem;
					mouseItem = ActivePlayer.armor[num2];
					ActivePlayer.armor[num2] = item;
					mouseItemSrcSection = InventorySection.EQUIP;
					mouseItemSrcX = inventoryEquipX;
					mouseItemSrcY = inventoryEquipY;
					if (ActivePlayer.armor[num2].Type == 0 || ActivePlayer.armor[num2].Stack < 1)
					{
						ActivePlayer.armor[num2].Init();
					}
					if (mouseItem.Type > 0 || ActivePlayer.armor[num2].Type > 0)
					{
						Main.PlaySound(7);
					}
				}
			}
			else if (IsButtonTriggered(BTN_INVENTORY_ACTION) && ActivePlayer.armor[num2].IsEquipable() && !reforge)
			{
				ref Item reference = ref ActivePlayer.armor[num2];
				reference = ActivePlayer.armorSwap(ref ActivePlayer.armor[num2]);
			}
		}

		private void DrawEquip(int INVENTORY_X, int INVENTORY_Y) // Needs overhaul in 1.03; smaller and more buff icons + dye slots
		{
			Color c = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
			Color c2 = new Color(invAlpha >> 1, invAlpha, invAlpha >> 1, invAlpha);
#if USE_ORIGINAL_CODE
			inventoryScale = 1f;
			int num = INVENTORY_X + 112;
			int y = INVENTORY_Y;
			Rectangle rect = default(Rectangle);
			rect.Y = y;
			rect.Width = 16;
			rect.Height = 56;
			if (inventoryBuffX > 0)
			{
				rect.X = num - 16;
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect, SpriteEffects.FlipHorizontally);
			}
			if (inventoryBuffX < 5)
			{
				rect.X = num + 280;
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect);
			}
			string extraInfo = null;
			for (int i = 0; i < 5; i++)
			{
				int num2 = ActivePlayer.buff[inventoryBuffX + i].Type;
				if (inventoryEquipY == 0 && inventoryEquipX == i)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip.Init();
					extraInfo = ((!Buff.IsDebuff(num2)) ? "<f c='#C0FFC0'>" : "<f c='#FFC0C0'>");
					if (num2 == 40)
					{
						num2 += ActivePlayer.pet;
					}
					extraInfo += Buff.BuffName[num2];
					extraInfo += "</f>\n";
					extraInfo += Buff.BuffTip[num2];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL(442, num, y, c2, inventoryScale);
				}
				if (num2 > 0)
				{
					int num3 = 141 + num2;
					if (num2 == 40)
					{
						num3 += ActivePlayer.pet;
					}
					DrawInventoryItem(num3, num, y, Color.White);
				}
				num += 56;
			}
			num = INVENTORY_X + 112;
			y = INVENTORY_Y + 88;
			for (int j = 0; j < 3; j++)
			{
				if (inventoryEquipX == 0 && inventoryEquipY == j + 1)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[j];
					toolTip.WornArmor = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL(442, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[j].Type > 0 && ActivePlayer.armor[j].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[j].IsMaterial) || (reforge && !ActivePlayer.armor[j].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[j], x: num, y: y);
				}
				y += 56;
			}
			y += 32;
			for (int k = 3; k < 8; k++)
			{
				if (inventoryEquipY == 4 && inventoryEquipX == k - 3)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[k];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL(442, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[k].Type > 0 && ActivePlayer.armor[k].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[k].IsMaterial) || (reforge && !ActivePlayer.armor[k].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[k], x: num, y: y);
				}
				num += 56;
			}
			num -= 56;
			y = INVENTORY_Y + 88;
			for (int l = 8; l < 11; l++)
			{
				if (inventoryEquipX == 4 && inventoryEquipY == l - 7)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[l];
					toolTip.IsSocial = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL(442, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[l].Type > 0 && ActivePlayer.armor[l].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[l].IsMaterial) || (reforge && !ActivePlayer.armor[l].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[l], x: num, y: y);
				}
				y += 56;
			}
			DrawPlayer(ActivePlayer, new Vector2(INVENTORY_X + 112 + 112, INVENTORY_Y + 88), 3.75f);
			DrawStringCT(SmallFont, Lang.InterfaceText[6], INVENTORY_X + 112 + 140, INVENTORY_Y + 56 - 12, Color.White);
			DrawStringCT(SmallFont, Lang.InterfaceText[45], INVENTORY_X + 112 + 28, y - 12, Color.White);
			DrawStringCT(SmallFont, Lang.InterfaceText[11], num + 28, y - 12, Color.White);
			DrawStringCT(BoldSmallFont, ((int)ActivePlayer.StatDefense).ToStringLookup() + Lang.InterfaceText[10], INVENTORY_X + 112 + 140, y - 6, new Color(100, 255, 200, 255));
			UpdateToolTipText(extraInfo);
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - 8, INVENTORY_Y + 8, 344);
#else

			// What's this? Another massive function directly decompiled? Nah but tbh its mostly small diffs (more inv slots) but the big taker is the 16 buff scroller as opposed to the 10 buff slider.

#if VERSION_FINAL
			// 1.09 code goes here
#elif VERSION_103
			inventoryScale = Main.ScreenMultiplier * 0.8571429f; // Why this precise? Idk.
			int Offset = (int)(56 * Main.ScreenMultiplier);
			int num = INVENTORY_X;
			int y = INVENTORY_Y;
			string extraInfo = null;
			Rectangle ArrowPosition = new Rectangle(0, y, (int)(16 * Main.ScreenMultiplier), (int)(48 * Main.ScreenMultiplier));

			if ((Main.frameCounter & 0x40) == 0)
			{
				ArrowPosition.X = num + (int)(32 * Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition, SpriteEffects.FlipHorizontally); // These are for the 2 blue arrows going left and right.
				ArrowPosition.X = num + (int)(480 * Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition);
			}

			Main.SpriteBatch.End();
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, view.screenProjection);
			float ScrollXMultiplier = inventoryBuffScrollX * 0.8125f;
			inventoryBuffScrollX = ScrollXMultiplier;
			if (ScrollXMultiplier < 0)
			{
				ScrollXMultiplier = -ScrollXMultiplier;
			}
			if (ScrollXMultiplier < 0.01f)
			{
				inventoryBuffScrollX = 0;
			}

			for (int i = -1; i < 10; i++) // Buff Bar
			{
				int buffholder = inventoryBuffX + i;
				buffholder %= 16;
				if (buffholder < 0)
				{
					buffholder += 16;
				}

				float invX2 = inventoryBuffScrollX * -(48f * Main.ScreenMultiplier);
				int BuffX = (int)(num - invX2);
				int num2 = player.buff[buffholder].Type;

				Main.SpriteBatch.End();
				Rectangle scissorRectangle = default;
				scissorRectangle.X = INVENTORY_X + (int)(48 * Main.ScreenMultiplier);
				scissorRectangle.Y = INVENTORY_Y;
				scissorRectangle.Width = (int)(432 * Main.ScreenMultiplier);
				scissorRectangle.Height = (int)(48 * Main.ScreenMultiplier);
				if (!view.isFullScreen())
				{
					scissorRectangle.X >>= 1;
					scissorRectangle.X += view.activeViewport.X;
					scissorRectangle.Y >>= 1;
					scissorRectangle.Y += view.activeViewport.Y;
					scissorRectangle.Width >>= 1;
					scissorRectangle.Height >>= 1;
				}
				WorldView.graphicsDevice.ScissorRectangle = scissorRectangle;
				Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, WorldView.scissorTest, view.screenProjection);
				if (inventoryEquipY == 0 && inventoryEquipX == i)
				{
					DrawInventoryCursor(BuffX, y, inventoryScale);
					toolTip.Init();
					extraInfo = ((!Buff.IsDebuff(num2)) ? "<f c='#C0FFC0'>" : "<f c='#FFC0C0'>");
					if (num2 == 40)
					{
						num2 += player.pet;
					}
					extraInfo += Buff.buffName[num2];
					extraInfo += "</f>\n";
					extraInfo += Buff.buffTip[num2];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, BuffX, y, c2, inventoryScale);
				}
				if (num2 > 0)
				{
					int num3 = (int)_sheetSprites.ID.BUFF_1 - 1 + num2;
					if (num2 == 40)
					{
						num3 += player.pet;
					}
					DrawInventoryItem(num3, BuffX, y, Color.White);
				}
                else
                {
					DrawStringCC(fontSmall, (buffholder + 1).ToString(), (int)(BuffX + 26f * inventoryScale), (int)(y + 26f * inventoryScale), new Color(40, 64, 40, 32));
				}
                num += Offset - (int)(8f * Main.ScreenMultiplier);
			}
			Main.SpriteBatch.End();
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, view.screenProjection);
			inventoryScale = Main.ScreenMultiplier;

			num = INVENTORY_X + (2 * Offset);
			y = INVENTORY_Y + (int)(88 * Main.ScreenMultiplier);
			for (int j = 0; j < 3; j++) // Armour
			{
				if (inventoryEquipX == 0 && inventoryEquipY == j + 1)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = player.armor[j];
					toolTip.wornArmor = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (player.armor[j].type > 0 && player.armor[j].stack > 0)
				{
					DrawInventoryItem(itemColor: ((craftGuide && !player.armor[j].material) || (reforge && !player.armor[j].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref player.armor[j], x: num, y: y);
				}
				y += Offset;
			}
			y += (int)(32 * Main.ScreenMultiplier);
			for (int k = 3; k < 8; k++) // Accessories
			{
				if (inventoryEquipY == 4 && inventoryEquipX == k - 3)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = player.armor[k];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (player.armor[k].type > 0 && player.armor[k].stack > 0)
				{
					DrawInventoryItem(itemColor: ((craftGuide && !player.armor[k].material) || (reforge && !player.armor[k].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref player.armor[k], x: num, y: y);
				}
				num += Offset;
			}
			num -= Offset;
			y = INVENTORY_Y + (int)(88 * Main.ScreenMultiplier);
			for (int l = 8; l < 11; l++) // Vanity
			{
				if (inventoryEquipX == 4 && inventoryEquipY == l - 7)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = player.armor[l];
					toolTip.social = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (player.armor[l].type > 0 && player.armor[l].stack > 0)
				{
					DrawInventoryItem(itemColor: ((craftGuide && !player.armor[l].material) || (reforge && !player.armor[l].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref player.armor[l], x: num, y: y);
				}
				y += Offset;
			}
			int yAddOffset = (int)(6 * Main.ScreenMultiplier);
			DrawPlayer(player, new Vector2(INVENTORY_X + (Offset * 4), INVENTORY_Y + (int)(88 * Main.ScreenMultiplier)), 3.75f * (float)Main.ScreenMultiplier);
			DrawStringCT(fontSmall, Lang.inter[6], INVENTORY_X + (2 * Offset) + (int)(140 * Main.ScreenMultiplier), INVENTORY_Y + Offset - (2 * yAddOffset), Color.White);
			DrawStringCT(fontSmall, Lang.inter[45], INVENTORY_X + (Offset * 2) + (int)(28 * Main.ScreenMultiplier), y - (2 * yAddOffset), Color.White);
			DrawStringCT(fontSmall, Lang.inter[11], num + (int)(28 * Main.ScreenMultiplier), y - (yAddOffset * 2), Color.White);
			DrawStringCT(FontSmallOutline, ((int)player.statDefense).ToStringLookup() + Lang.inter[10], INVENTORY_X + (Offset * 2) + (int)(140 * Main.ScreenMultiplier), y - yAddOffset, new Color(100, 255, 200, 255));
			UpdateToolTipText(extraInfo);
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - (int)(8 * Main.ScreenMultiplier), INVENTORY_Y + (int)(8 * Main.ScreenMultiplier), (int)(344 * Main.ScreenMultiplier));
#else
			inventoryScale = Main.ScreenMultiplier;
			int Offset = (int)(56 * Main.ScreenMultiplier);
			int num = INVENTORY_X + (Offset * 2);
			int y = INVENTORY_Y;

			Rectangle rect = default(Rectangle);
			rect.Y = y;
			rect.Width = (int)(16 * Main.ScreenMultiplier);
			rect.Height = Offset;
			if (inventoryBuffX > 0)
			{
				rect.X = num - (int)(16 * Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect, SpriteEffects.FlipHorizontally);
			}
			if (inventoryBuffX < 5)
			{
				rect.X = num + (int)(280 * Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect);
			}
			string extraInfo = null;
			for (int i = 0; i < 5; i++)
			{
				int num2 = ActivePlayer.buff[inventoryBuffX + i].Type;
				if (inventoryEquipY == 0 && inventoryEquipX == i)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip.Init();
					extraInfo = ((!Buff.IsDebuff(num2)) ? "<f c='#C0FFC0'>" : "<f c='#FFC0C0'>");
					if (num2 == (int)Buff.ID.PET)
					{
						num2 += ActivePlayer.pet;
					}
					extraInfo += Buff.BuffName[num2];
					extraInfo += "</f>\n";
					extraInfo += Buff.BuffTip[num2];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c2, inventoryScale);
				}
				if (num2 > 0)
				{
					int num3 = (int)_sheetSprites.ID.BUFF_1 - 1 + num2;
					if (num2 == (int)Buff.ID.PET)
					{
						num3 += ActivePlayer.pet;
					}
					DrawInventoryItem(num3, num, y, Color.White);
				}
				num += Offset;
			}

			num = INVENTORY_X + (2 * Offset);
			y = INVENTORY_Y + (int)(88 * Main.ScreenMultiplier);
			for (int j = 0; j < Player.NumArmorSlots; j++) // Armour
			{
				if (inventoryEquipX == 0 && inventoryEquipY == j + 1)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[j];
					toolTip.WornArmor = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[j].Type > 0 && ActivePlayer.armor[j].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[j].IsMaterial) || (reforge && !ActivePlayer.armor[j].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[j], x: num, y: y);
				}
				y += Offset;
			}
			y += (int)(32 * Main.ScreenMultiplier);
			for (int k = Player.NumArmorSlots; k < (Player.NumArmorSlots + Player.NumEquipSlots); k++) // Accessories
			{
				if (inventoryEquipY == 4 && inventoryEquipX == k - 3)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[k];
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[k].Type > 0 && ActivePlayer.armor[k].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[k].IsMaterial) || (reforge && !ActivePlayer.armor[k].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[k], x: num, y: y);
				}
				num += Offset;
			}
			num -= Offset;
			y = INVENTORY_Y + (int)(88 * Main.ScreenMultiplier);
			for (int l = (Player.NumArmorSlots + Player.NumEquipSlots); l < Player.MaxNumArmor; l++) // Vanity
			{
				if (inventoryEquipX == 4 && inventoryEquipY == l - 7)
				{
					DrawInventoryCursor(num, y, inventoryScale);
					toolTip = ActivePlayer.armor[l];
					toolTip.IsSocial = true;
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK3, num, y, c, inventoryScale);
				}
				if (ActivePlayer.armor[l].Type > 0 && ActivePlayer.armor[l].Stack > 0)
				{
					DrawInventoryItem(itemColor: ((CraftGuide && !ActivePlayer.armor[l].IsMaterial) || (reforge && !ActivePlayer.armor[l].SetPrefix(-3))) ? DISABLED_COLOR : new Color(255, 255, 255, 255), item: ref ActivePlayer.armor[l], x: num, y: y);
				}
				y += Offset;
			}
			int yAddOffset = (int)(6 * Main.ScreenMultiplier);
			DrawPlayer(ActivePlayer, new Vector2(INVENTORY_X + (Offset * 4), INVENTORY_Y + (int)(88 * Main.ScreenMultiplier)), 3.75f * Main.ScreenMultiplier);
			DrawStringCT(SmallFont, Lang.InterfaceText[6], INVENTORY_X + (2 * Offset) + (int)(140 * Main.ScreenMultiplier), INVENTORY_Y + Offset - (2 * yAddOffset), Color.White);
			DrawStringCT(SmallFont, Lang.InterfaceText[45], INVENTORY_X + (Offset * 2) + (int)(28 * Main.ScreenMultiplier), y - (2 * yAddOffset), Color.White);
			DrawStringCT(SmallFont, Lang.InterfaceText[11], num + (int)(28 * Main.ScreenMultiplier), y - (yAddOffset * 2), Color.White);
			DrawStringCT(BoldSmallFont, ((int)ActivePlayer.StatDefense).ToStringLookup() + Lang.InterfaceText[10], INVENTORY_X + (Offset * 2) + (int)(140 * Main.ScreenMultiplier), y - yAddOffset, new Color(100, 255, 200, 255));
			UpdateToolTipText(extraInfo);
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - (int)(8 * Main.ScreenMultiplier), INVENTORY_Y + (int)(8 * Main.ScreenMultiplier), (int)(344 * Main.ScreenMultiplier));
#endif
#endif
			DrawControlsInventory();
		}

		private void UpdateHousing()
		{
			int x = ActivePlayer.XYWH.X >> 4;
			int y = ActivePlayer.XYWH.Y >> 4;
			if (IsButtonTriggered(Buttons.X))
			{
				Main.PlaySound(12);
				if (WorldGen.MoveNPC(x, y, -1))
				{
					Main.NewText(Lang.InterfaceText[39], 255, 240, 20);
				}
			}
			else if (IsButtonTriggered(Buttons.Y))
			{
				Main.PlaySound(12);
				showNPCs = !showNPCs;
			}
			else if (IsButtonTriggered(Buttons.A) && inventoryHousingNpc >= 0)
			{
				Main.PlaySound(12);
				if (WorldGen.MoveNPC(x, y, inventoryHousingNpc))
				{
					WorldGen.moveRoom(x, y, inventoryHousingNpc);
					Main.PlaySound(12);
				}
			}
		}

		private void DrawHousing(int INVENTORY_X, int INVENTORY_Y) // Will need an overhaul for 1.2; every 7 NPCs instead of every 6
		{
			Color c = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
#if USE_ORIGINAL_CODE
			inventoryScale = 1f;
#else
			inventoryScale = Main.ScreenMultiplier;
#endif
			string extraInfo = null;
			int num = INVENTORY_X;
			int num2 = INVENTORY_Y;
			for (int i = 0; i < 11; i++)
			{
				if (i == 6)
				{
#if USE_ORIGINAL_CODE
					num += 240;
#else
					num += (int)(240 * Main.ScreenMultiplier);
#endif
					num2 = INVENTORY_Y;
				}
				string text = null;
				NPC nPC = null;
				int num3 = -1;
				for (int j = 0; j < NPC.MaxNumNPCs; j++)
				{
					nPC = Main.NPCSet[j];
					if (nPC.Active != 0 && i + 1 == nPC.GetHeadTextureID())
					{
						num3 = j;
						text = ((!nPC.HasName()) ? nPC.DisplayName : nPC.GetName());
						break;
					}
				}
				if (inventoryHousingX == i / 6 && inventoryHousingY == i % 6)
				{
					DrawInventoryCursor(num, num2, inventoryScale);
					inventoryHousingNpc = (short)num3;
					if (text != null)
					{
						extraInfo = text;
						if (i != 10 && Lang.LangOption <= (int)Lang.ID.ENGLISH)
						{
							extraInfo = extraInfo + " the " + nPC.TypeName;
						}
						extraInfo += '\n';
						extraInfo += Lang.InterfaceText[55 + i];
					}
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK11, num, num2, c, inventoryScale);
				}
				if (num3 >= 0)
				{
					int itemTexId = i + (int)_sheetSprites.ID.NPC_HEAD_1;
					DrawInventoryItem(itemTexId, num, num2, Color.White);
#if USE_ORIGINAL_CODE
					if (!nPC.IsHomeless)
					{
						SpriteSheet<_sheetSprites>.DrawTL(437, num + 60 - 16, num2 - 8, Color.White, inventoryScale);
					}
					DrawStringLC(BoldSmallFont, text, num + 60, num2 + 30, Color.White);
				}
				num2 += 60;
			}
			UpdateToolTipText(extraInfo);
			DrawToolTip(INVENTORY_X + INVENTORY_W - TOOLTIP_W - 8, INVENTORY_Y + 8, 344);
#else
					if (!nPC.IsHomeless)
					{
						SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.HOUSE_1, num + (int)(44 * Main.ScreenMultiplier), num2 - (int)(8 * Main.ScreenMultiplier), Color.White, inventoryScale);
					}
					DrawStringLC(BoldSmallFont, text, num + (int)(60 * Main.ScreenMultiplier), num2 + (int)(30 * Main.ScreenMultiplier), Color.White);
				}
				num2 += (int)(60 * Main.ScreenMultiplier);
			}
			UpdateToolTipText(extraInfo);
			DrawToolTip((INVENTORY_X + INVENTORY_W - TOOLTIP_W - (int)(8 * Main.ScreenMultiplier)), INVENTORY_Y + (int)(8 * Main.ScreenMultiplier), (int)(344 * Main.ScreenMultiplier));
#endif
			DrawControlsHousing();
		}

		private void DrawMouseItem()
		{
			if (mouseItem.Stack <= 0)
			{
				mouseItem.Init();
			}
			else if (mouseItem.Type > 0 && mouseItem.Stack > 0)
			{
#if USE_ORIGINAL_CODE
				inventoryScale = cursorScale;
#else
				inventoryScale = cursorScale * Main.ScreenMultiplier;
#endif
				DrawInventoryItem(ref mouseItem, MouseX + 6, MouseY + 6, new Color(0, 0, 0, 128), StackType.INVENTORY);
				DrawInventoryItem(ref mouseItem, MouseX, MouseY, Color.White, StackType.INVENTORY);
			}
		}

		private void DrawInventoryMenu()
		{
			int num = CurrentView.SafeAreaOffsetLeft;
			int sAFE_AREA_OFFSET_T = CurrentView.SafeAreaOffsetTop;

			/*bool thing = true;	This is here for something; TO-DO: confirm on PPC 1.2 if this exists in the function
			if ((6 < (int)view.viewType) || ((0x58 >> ((int)view.viewType & 0x1f) & 1) == 0))
			{
				thing = false;
			}
			int sAFE_AREA_OFFSET_T = view.SafeAreaOffsetTop + 0x28;
			if (!thing)
			{
				sAFE_AREA_OFFSET_T = view.SafeAreaOffsetTop;
			}*/

			if (CurrentView.ViewWidth > Main.ResolutionWidth)
			{
				num = CurrentView.ViewWidth - INVENTORY_W >> 1;
			}
			bool flag = ActivePlayer.PlayerChest != -1;
			bool flag2 = npcShop > 0;
			bool flag3 = flag || flag2;
			int texId;
			switch (ActiveInvSection)
			{
			case InventorySection.CRAFTING:
				texId = (int)_sheetSprites.ID.INVENTORY_BACK2;
				break;
			case InventorySection.EQUIP:
				texId = (int)_sheetSprites.ID.INVENTORY_BACK3;
				break;
			case InventorySection.CHEST:
				texId = (flag2 ? (int)_sheetSprites.ID.INVENTORY_BACK6 : (int)_sheetSprites.ID.INVENTORY_BACK5);
				break;
			case InventorySection.HOUSING:
				texId = (int)_sheetSprites.ID.INVENTORY_BACK11;
				break;
			default:
				texId = (int)_sheetSprites.ID.INVENTORY_BACK;
				break;
			}

#if !USE_ORIGINAL_CODE
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					INVENTORY_H = USABLE_HEIGHT;
					break;
				case 2:
					INVENTORY_H = USABLE_HEIGHT + 16;
					break;
			}
#endif

			Main.DrawRect(texId, new Rectangle(num, sAFE_AREA_OFFSET_T, INVENTORY_W, INVENTORY_H), 96);
			Color c = default;
#if USE_ORIGINAL_CODE
			int num2 = num + (INVENTORY_W - ((flag3 ? 4 : 3) * 43 + 56)) / 2;
#else
			int num2 = (num + (INVENTORY_W - (int)(((flag3 ? 4 : 3) * 43 + 56) * Main.ScreenMultiplier)) / 2);
#endif
			for (int i = 0; i < 5; i++)
			{
				switch ((byte)i)
				{
				case 1:
					texId = (int)_sheetSprites.ID.INVENTORY;
					break;
				case 0:
					texId = (int)_sheetSprites.ID.CRAFT;
					break;
				case 3:
					texId = (int)_sheetSprites.ID.EQUIP;
					break;
				case 2:
					if (flag2)
					{
						texId = Chest.GetShopOwnerHeadTextureId(npcShop);
						break;
					}
					if (!flag)
					{
						continue;
					}
					switch (ActivePlayer.PlayerChest)
					{
					case -2:
						texId = (int)_sheetSprites.ID.ITEM_87;
						break;
					case -3:
						texId = (int)_sheetSprites.ID.ITEM_346;
						break;
					default:
						texId = (int)_sheetSprites.ID.ITEM_48;
						break;
					}
					break;
				default:
					texId = (int)_sheetSprites.ID.HOUSE_1;
					break;
				}
				int num3 = (int)ActiveInvSection;
				float num4 = inventoryMenuSectionScale[i];
				if (i == num3)
				{
					if (num4 < 1f)
					{
						num4 += 0.05f;
						inventoryMenuSectionScale[i] = num4;
					}
				}
				else if (num4 > 0.75f)
				{
					num4 -= 0.05f;
					inventoryMenuSectionScale[i] = num4;
				}
#if USE_ORIGINAL_CODE
				int y = (int)((float)sAFE_AREA_OFFSET_T + 22f * (1f - num4));
#else
				int y = (int)(sAFE_AREA_OFFSET_T + (22f * Main.ScreenMultiplier) * (1f - num4));
#endif
				int num5 = (int)(65f + 180f * num4);
#if !USE_ORIGINAL_CODE
				num4 *= Main.ScreenMultiplier;
#endif

				if (i == num3)
				{
					c.R = 200;
					c.G = 200;
					c.B = 200;
					c.A = 200;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK10, num2, y, c, num4);
				}
				else
				{
					c.R = 100;
					c.G = 100;
					c.B = 100;
					c.A = 100;
					SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK, num2, y, c, num4);
				}
				inventoryScale = num4;
				Color itemColor = ((!IsInventorySectionAvailable((InventorySection)i)) ? DISABLED_COLOR : new Color(num5, num5, num5, num5));
				DrawInventoryItem(texId, num2, y, itemColor);
#if USE_ORIGINAL_CODE
				num2 += (int)(52f * num4) + 4;
#else
				num2 += (int)(52f * num4) + (int)(4 * Main.ScreenMultiplier);
#endif
			}
			string text;
			switch (ActiveInvSection)
			{
			case InventorySection.CRAFTING:
				text = Lang.InterfaceText[25];
				break;
			case InventorySection.ITEMS:
				text = Lang.InterfaceText[4];
				break;
			case InventorySection.CHEST:
				if (flag2)
				{
					text = Lang.InterfaceText[28];
					break;
				}
				switch ((Player.ExtraStorage)ActivePlayer.PlayerChest)
				{
				case Player.ExtraStorage.PIGGYBANK:
					text = Lang.InterfaceText[32];
					break;
				case Player.ExtraStorage.SAFE:
					text = Lang.InterfaceText[33];
					break;
				default:
					text = chestText;
					break;
				}
				break;
			case InventorySection.EQUIP:
				text = Lang.InterfaceText[45];
				break;
			default:
				text = Lang.InterfaceText[7];
				break;
			}
			if (reforge)
			{
				text += " (";
				text += Lang.InterfaceText[19];
				text += ')';
			}
#if USE_ORIGINAL_CODE
			DrawStringCT(SmallFont, text, num + 432, sAFE_AREA_OFFSET_T + 44, Color.White);
#else
			DrawStringCT(SmallFont, text, (int)(num + (432 * Main.ScreenMultiplier)), (int)(sAFE_AREA_OFFSET_T + (44 * Main.ScreenMultiplier)), Color.White);
#endif
			switch (ActiveInvSection)
			{
			case InventorySection.ITEMS:
				DrawInventory(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				break;
			case InventorySection.CHEST:
				if (flag2)
				{
					DrawShop(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				}
				else
				{
					DrawStorage(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				}
				break;
			case InventorySection.CRAFTING:
				DrawCrafting(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				break;
			case InventorySection.EQUIP:
				DrawEquip(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				break;
			case InventorySection.HOUSING:
				DrawHousing(num, sAFE_AREA_OFFSET_T + INVENTORY_CLIENT_Y_OFFSET);
				break;
			}
			DrawMouseItem();
		}

		private void DrawCrafting(int CRAFTING_X, int CRAFTING_Y)
		{
			Color color = default;
			Color color2 = new Color(invAlpha, invAlpha, invAlpha, invAlpha);
			string text = null;
#if USE_ORIGINAL_CODE
			Main.DrawRectOpenAtTop(441, new Rectangle(CRAFTING_X, CRAFTING_Y + 48, INVENTORY_W, 318), 192);
			int num = CRAFTING_X + 3;

			for (int i = 0; i < (int)Recipe.Category.NUM_CATEGORIES; i++)
			{
				Rectangle rect = new Rectangle(num, CRAFTING_Y + 8, 127, 32);
				if ((int)craftingCategory != i)
				{
					Main.DrawRectStraightBottom(441, rect, 192, 2);
				}
				else
				{
					Main.DrawRectOpenAtBottom(441, rect, 192);
				}
				SpriteSheet<_sheetSprites>.DrawCentered(206 + i, ref rect);
				num += 146;
			}
			int num2 = CRAFTING_X + 4 + 16;
			int num3 = CRAFTING_Y + 12 + 48;
			int count = CurrentRecipeCategory.Count;
			if (count > 0)
			{
				inventoryScale = 1f;
				int num4 = CraftingRecipeY - Math.Sign(craftingRecipeScrollY);
				int num5 = Math.Min(8, CurrentRecipeCategory[CraftingRecipeY].RecipeList.Count) * 56;
				int width = ((craftingRecipeScrollX == 0f) ? 448 : num5);
				int num6 = Math.Min(5, count) * 56;
				Rectangle scissorRectangle = default;
				for (int j = 0; j < 2; j++)
				{
					Main.SpriteBatch.End();
					scissorRectangle.X = num2;
					scissorRectangle.Y = num3;
					if (j == 0)
					{
						scissorRectangle.Width = 56;
						scissorRectangle.Height = num6;
					}
					else
					{
						scissorRectangle.Y += 56;
						scissorRectangle.Width = width;
						scissorRectangle.Height = 56;
					}
					if (!CurrentView.IsFullScreen())
					{
						scissorRectangle.X >>= 1;
						scissorRectangle.X += CurrentView.ActiveViewport.X;
						scissorRectangle.Y >>= 1;
						scissorRectangle.Y += CurrentView.ActiveViewport.Y;
						scissorRectangle.Width >>= 1;
						scissorRectangle.Height >>= 1;
					}
					WorldView.GraphicsDevice.ScissorRectangle = scissorRectangle;
					Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, WorldView.ScissorRastTest, CurrentView.ScreenProjection);
					int num7 = num3;
					int num8 = 1;
					if (craftingRecipeScrollY != 0f)
					{
						num8 = 2;
						num7 -= 56;
						num7 -= (int)((1f - Math.Abs(craftingRecipeScrollY)) * 56f) * Math.Sign(craftingRecipeScrollY);
						if (j == 1)
						{
							craftingRecipeScrollY *= craftingRecipeScrollMul;
							if (Math.Abs(craftingRecipeScrollY) < 0.0178571437f)
							{
								craftingRecipeScrollY = 0f;
							}
						}
					}
					for (int k = -num8; k < 5 + num8; k++)
					{
						int num9 = num4 + k;
						if (num9 >= 0)
						{
							if (num9 >= count)
							{
								int num10 = num7 - num3;
								if (num10 < num6)
								{
									num6 = num10;
								}
								break;
							}
							int num11 = 0;
							int num12 = num2;
							if (craftingRecipeScrollX != 0f && num9 == CraftingRecipeY)
							{
								num11 = 1;
								num12 -= 56;
								if (j == 0)
								{
									craftingRecipeScrollX *= craftingRecipeScrollMul;
									if (Math.Abs(craftingRecipeScrollX) < 0.0178571437f)
									{
										craftingRecipeScrollX = 0f;
									}
								}
								num12 -= (int)((1f - Math.Abs(craftingRecipeScrollX)) * 56f) * Math.Sign(craftingRecipeScrollX);
							}
							Recipe.SubCategoryList subCategoryList = CurrentRecipeCategory[num9];
							int num13 = ((num9 == CraftingRecipeY) ? (CraftingRecipeX - Math.Sign(craftingRecipeScrollX)) : 0);
							int count2 = subCategoryList.RecipeList.Count;
							for (int l = -num11; l < 8 + num11; l++)
							{
								int num14 = num13 + l;
								if (num14 < 0)
								{
									num14 += count2;
								}
								if (num11 == 0 && l >= count2)
								{
									break;
								}
								num14 %= subCategoryList.RecipeList.Count;
								int num15 = subCategoryList.RecipeList[num14];
								Recipe recipe = Main.ActiveRecipe[num15];
								bool flag = ActivePlayer.CanCraftRecipe(recipe);
								bool flag2 = num9 == CraftingRecipeY && num14 == CraftingRecipeX;
								if (flag2)
								{
									CraftingRecipe = recipe;
									DrawInventoryCursor(num12, num7, inventoryScale, (craftingSection == CraftingSection.RECIPES) ? 255 : 96);
									if (j == 0)
									{
										toolTip = recipe.CraftedItem;
										text = "(";
										text += ActivePlayer.CountPossession(toolTip.NetID).ToStringLookup();
										text += Lang.MenuText[1];
										UpdateToolTipText(text);
									}
								}
								else
								{
									int id = ((j == 0) ? 443 : 450);
									color = color2;
									if (!flag)
									{
										color.R >>= 1;
										color.G >>= 1;
										color.B >>= 1;
										color.A >>= 1;
									}
									SpriteSheet<_sheetSprites>.DrawTL(id, num12, num7, color, inventoryScale);
								}
								color = (flag ? new Color(255, 255, 255, 255) : new Color(16, 16, 16, 128));
								DrawInventoryItem(ref recipe.CraftedItem, num12, num7, color, StackType.INVENTORY);
								if (ActivePlayer.recipesNew.Get(num15))
								{
									if (flag2)
									{
										ActivePlayer.recipesNew.Set(num15, value: false);
									}
									else
									{
										color = new Color(cursorAlpha, cursorAlpha, cursorAlpha, cursorAlpha);
										SpriteSheet<_sheetTiles>.Draw(23, num12 + 56 - 16, num7 + 12, color, (float)((double)Main.FrameCounter * (1.0 / (8.0 * Math.PI))), (float)(0.8 + Math.Sin((double)Main.FrameCounter * (1.0 / (16.0 * Math.PI))) * 0.2));
										SpriteSheet<_sheetTiles>.Draw(23, num12 + 28, num7 + 28, color, (float)((double)Main.FrameCounter * (1.0 / (6.0 * Math.PI))), (float)(0.6 + Math.Sin((double)Main.FrameCounter * (1.0 / (24.0 * Math.PI))) * 0.4));
										SpriteSheet<_sheetTiles>.Draw(23, num12 + 28 - 12, num7 + 28 - 8, color, (float)((double)Main.FrameCounter * 0.079577471545947673), (float)(0.7 + Math.Sin((double)Main.FrameCounter * (1.0 / (32.0 * Math.PI))) * 0.3));
									}
								}
								num12 += 56;
							}
						}
						num7 += 56;
					}
				}
				Main.SpriteBatch.End();
				Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, CurrentView.ScreenProjection);
				if (craftingRecipeScrollX == 0f && craftingRecipeScrollY == 0f)
				{
					Rectangle rect2 = default(Rectangle);
					if (num5 > 56)
					{
						rect2.X = num2 - 16;
						rect2.Y = num3 + 56;
						rect2.Width = 16;
						rect2.Height = 56;
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2, SpriteEffects.FlipHorizontally);
						rect2.X += 10 + num5;
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2);
					}
					if (count > 1)
					{
						if (CraftingRecipeY == 0)
						{
							num3 += 56;
							num6 -= 56;
						}
						rect2.X = num2;
						rect2.Y = num3 - 16;
						rect2.Width = 56;
						rect2.Height = 16;
						SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect2, SpriteEffects.FlipVertically);
						rect2.Y += num6 + 10;
						SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect2);
						if (CraftingRecipeY == 0)
						{
							num3 -= 56;
							num6 += 56;
						}
					}
				}
				int num16 = num2 + 56 + 8;
				int num17 = num3 + 112 + 8;
				Main.DrawRect(441, new Rectangle(num16, num17, CRAFTING_X + INVENTORY_W - TOOLTIP_W - 32 - num16, 174), 96);
				SmallFont.MeasureString(Lang.MenuText[0]);
				DrawStringCT(SmallFont, Lang.MenuText[0], num16 + 100, num17 + 150 - 6, Color.White);
				inventoryScale = 0.9f;
				int num18 = 0;
				for (int m = 0; m < 3; m++)
				{
					int num19 = 0;
					while (num19 < 4)
					{
						int num12 = num16 + 50 * num19;
						int num7 = num17 + 50 * m;
						if (craftingSection == CraftingSection.INGREDIENTS && num19 == craftingIngredientX && m == craftingIngredientY)
						{
							DrawInventoryCursor(num12, num7, inventoryScale);
							if (num18 < CraftingRecipe.NumRequiredItems)
							{
								toolTip = CraftingRecipe.RequiredItem[num18];
								text = "(";
								text += ActivePlayer.CountPossession(toolTip.NetID).ToStringLookup();
								text += Lang.MenuText[1];
								UpdateToolTipText(text);
							}
						}
						else
						{
							int id2 = 442;
							color = ((num18 < CraftingRecipe.NumRequiredItems) ? color2 : new Color(64, 64, 64, 64));
							SpriteSheet<_sheetSprites>.DrawTL(id2, num12, num7, color, inventoryScale);
						}
						if (num18 < CraftingRecipe.NumRequiredItems)
						{
							DrawInventoryItem(ref CraftingRecipe.RequiredItem[num18], num12, num7, new Color(255, 255, 255, 255), StackType.INGREDIENT);
						}
						num19++;
						num18++;
					}
				}
				int num20 = -1;
				Main.StrBuilder.Length = 0;
				if (CraftingRecipe.NumRequiredTiles > 0)
				{
					Main.StrBuilder.Append(Main.TileNames[CraftingRecipe.RequiredTile[0]]);
					switch (CraftingRecipe.RequiredTile[0])
					{
					case 13:
						num20 = 1;
						break;
					case 14:
						num20 = 1480;
						Main.StrBuilder.Append(" & ");
						Main.StrBuilder.Append(Main.TileNames[15]);
						break;
					case 16:
						num20 = 486;
						break;
					case 17:
						num20 = 484;
						break;
					case 18:
						if (CraftingRecipe.RequiredTile[1] == 15)
						{
							num20 = 1487;
							Main.StrBuilder.Append(" & ");
							Main.StrBuilder.Append(Main.TileNames[15]);
						}
						else
						{
							num20 = 487;
						}
						break;
					case 26:
						num20 = 212;
						break;
					case 86:
						num20 = 783;
						break;
					case 94:
						num20 = 803;
						break;
					case 96:
						num20 = 796;
						break;
					case 101:
						num20 = 139;
						break;
					case 106:
						num20 = 814;
						break;
					case 114:
						num20 = 849;
						break;
					case 134:
						num20 = 976;
						break;
					}
				}
				else if (CraftingRecipe.NeedsWater)
				{
					num20 = 1484;
					Main.StrBuilder.Append(Lang.InterfaceText[53]);
				}
				if (num20 >= 0)
				{
					Rectangle rect3 = default(Rectangle);
					rect3.X = num16 + 200 + 80;
					rect3.Y = num17 + 18 + 28;
					rect3.Width = 68;
					rect3.Height = 68;
					Main.DrawRect(442, rect3, 192);
					int width2 = SpriteSheet<_sheetSprites>.Source[num20].Width;
					int height = SpriteSheet<_sheetSprites>.Source[num20].Height;
					float scaleCenter = ((width2 <= height) ? (64f / (float)height) : (64f / (float)width2));
					Vector2 pos = default(Vector2);
					pos.X = rect3.Center.X;
					pos.Y = rect3.Center.Y;
					SpriteSheet<_sheetSprites>.DrawScaled(num20, ref pos, Color.White, scaleCenter);
					DrawStringCT(SmallFont, rect3.Center.X, rect3.Bottom + 2, ActivePlayer.IsNearCraftingStation(CraftingRecipe) ? Color.White : new Color(255, 64, 64, 255));
				}
				if (text != null)
				{
					DrawToolTip(CRAFTING_X + INVENTORY_W - TOOLTIP_W - 8, num3 + 8, INVENTORY_CLIENT_H - INVENTORY_CLIENT_Y_OFFSET);
				}
			}
#else
			int yOffset = 48;
			int InventoryH = 318;
			switch (Main.ScreenHeightPtr)
			{ // So for some reason, 1080p mode doesn't have the values directly *= 2 from the 540p ones.
				case 1:
					yOffset = 60;
					InventoryH = 438; // ~13.333 Rnd'd up diff
					break;
				case 2:
					yOffset = 88;
					InventoryH = 676; // 40 diff
					break;
			}
			Main.DrawRectOpenAtTop((int)_sheetSprites.ID.INVENTORY_BACK2, new Rectangle(CRAFTING_X, CRAFTING_Y + yOffset, INVENTORY_W, InventoryH), 192);
			int num = (int)(CRAFTING_X + (3 * Main.ScreenMultiplier));

			for (int i = 0; i < (int)Recipe.Category.NUM_CATEGORIES; i++)
			{
				Rectangle rect = new Rectangle(num, (int)(CRAFTING_Y + (8 * Main.ScreenMultiplier)), (int)(127 * Main.ScreenMultiplier), (int)(32 * Main.ScreenMultiplier));
				if ((int)craftingCategory != i)
				{
					Main.DrawRectStraightBottom((int)_sheetSprites.ID.INVENTORY_BACK2, rect, 192, 2);
				}
				else
				{
					Main.DrawRectOpenAtBottom((int)_sheetSprites.ID.INVENTORY_BACK2, rect, 192);
				}
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.CRAFTCATEGORY_1 + i, ref rect, Main.ScreenMultiplier); // This is actually pointing to a custom draw function, since in the original, it just uses a bigger spritesheet for 1080p mode;
				// However, we cannot use the spritesheet since it's exclusive to 8th gen consoles (e.g. PS4), and even if we had it, XNA (and by extension, FNA) cannot use spritesheets bigger than 4096x4096.
				// As a result, we need to keep it centered, yet scaled up.
				num += (int)(146 * Main.ScreenMultiplier);
			}
			int num2 = (int)(CRAFTING_X + (20 * Main.ScreenMultiplier)); // Control X/Y of the row of icons
			int num3 = (int)(CRAFTING_Y + (60 * Main.ScreenMultiplier));
			int count = CurrentRecipeCategory.Count;
			int Base = (int)(56 * Main.ScreenMultiplier); // Spacing
			byte XCount = 8;
			byte YCount = 5;
			if (count > 0)
			{
				inventoryScale = Main.ScreenMultiplier; // Controls icon size
				int num4 = CraftingRecipeY - Math.Sign(craftingRecipeScrollY);
				int num5 = Math.Min(XCount, CurrentRecipeCategory[CraftingRecipeY].RecipeList.Count) * Base; // How many are accounted for from the left by the arrow
				int width = ((craftingRecipeScrollX == 0f) ? (int)(448 * Main.ScreenMultiplier) : num5);
				int num6 = Math.Min(YCount, count) * Base; // How many from the top are shown
				Rectangle scissorRectangle = default;
				for (int j = 0; j < 2; j++) // When j== 0, y-axis; j ==1 is x-axis
				{
					Main.SpriteBatch.End();
					scissorRectangle.X = num2;
					scissorRectangle.Y = num3;
					if (j == 0)
					{
						scissorRectangle.Width = Base;
						scissorRectangle.Height = num6;
					}
					else
					{
						scissorRectangle.Y += Base;
						scissorRectangle.Width = width;
						scissorRectangle.Height = Base;
					}
					if (!CurrentView.IsFullScreen())
					{
						scissorRectangle.X >>= 1;
						scissorRectangle.X += CurrentView.ActiveViewport.X;
						scissorRectangle.Y >>= 1;
						scissorRectangle.Y += CurrentView.ActiveViewport.Y;
						scissorRectangle.Width >>= 1;
						scissorRectangle.Height >>= 1;
					}
					WorldView.GraphicsDevice.ScissorRectangle = scissorRectangle;
					Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, WorldView.ScissorRastTest, CurrentView.ScreenProjection);
					int num7 = num3;
					int num8 = 1;
					if (craftingRecipeScrollY != 0f)
					{
						num8 = 2;
						num7 -= Base;
						num7 -= (int)((1f - Math.Abs(craftingRecipeScrollY)) * Base) * Math.Sign(craftingRecipeScrollY);
						if (j == 1)
						{
							craftingRecipeScrollY *= craftingRecipeScrollMul;
							if (Math.Abs(craftingRecipeScrollY) < 0.0178571437f)
							{
								craftingRecipeScrollY = 0f;
							}
						}
					}
					for (int k = -num8; k < YCount + num8; k++)
					{
						int num9 = num4 + k;
						if (num9 >= 0)
						{
							if (num9 >= count)
							{
								int num10 = num7 - num3;
								if (num10 < num6)
								{
									num6 = num10;
								}
								break;
							}
							int num11 = 0;
							int num12 = num2;
							if (craftingRecipeScrollX != 0f && num9 == CraftingRecipeY)
							{
								num11 = 1;
								num12 -= Base;
								if (j == 0)
								{
									craftingRecipeScrollX *= craftingRecipeScrollMul;
									if (Math.Abs(craftingRecipeScrollX) < 0.0178571437f)
									{
										craftingRecipeScrollX = 0f;
									}
								}
								num12 -= (int)((1f - Math.Abs(craftingRecipeScrollX)) * Base) * Math.Sign(craftingRecipeScrollX);
							}
							Recipe.SubCategoryList subCategoryList = CurrentRecipeCategory[num9];
							int num13 = ((num9 == CraftingRecipeY) ? (CraftingRecipeX - Math.Sign(craftingRecipeScrollX)) : 0);
							int count2 = subCategoryList.RecipeList.Count;
							for (int l = -num11; l < XCount + num11; l++)
							{
								int num14 = num13 + l;
								if (num14 < 0)
								{
									num14 += count2;
								}
								if (num11 == 0 && l >= count2)
								{
									break;
								}
								num14 %= subCategoryList.RecipeList.Count;
								int num15 = subCategoryList.RecipeList[num14];
								Recipe recipe = Main.ActiveRecipe[num15];
								bool flag = ActivePlayer.CanCraftRecipe(recipe);
								bool flag2 = num9 == CraftingRecipeY && num14 == CraftingRecipeX;
								if (flag2) // Selected item in row
								{
									CraftingRecipe = recipe;
									DrawInventoryCursor(num12, num7, inventoryScale, (craftingSection == CraftingSection.RECIPES) ? 255 : 96);
									if (j == 0)
									{
										toolTip = recipe.CraftedItem;
										text = "(";
										text += ActivePlayer.CountPossession(toolTip.NetID).ToStringLookup();
										text += Lang.MenuText[1];
										UpdateToolTipText(text);
									}
								}
								else
								{
									int id = ((j == 0) ? (int)_sheetSprites.ID.INVENTORY_BACK4 : (int)_sheetSprites.ID.INVENTORY_BACK12); // Inactive icon colours
									color = color2;
									if (!flag)
									{
										color.R >>= 1;
										color.G >>= 1;
										color.B >>= 1;
										color.A >>= 1;
									}
									SpriteSheet<_sheetSprites>.DrawTL(id, num12, num7, color, inventoryScale);
								}
								color = (flag ? new Color(255, 255, 255, 255) : new Color(16, 16, 16, 128)); // Make item have colour if craftable
								DrawInventoryItem(ref recipe.CraftedItem, num12, num7, color, StackType.INVENTORY);
								if (ActivePlayer.recipesNew.Get(num15))
								{
									if (flag2)
									{
										ActivePlayer.recipesNew.Set(num15, value: false);
									}
									else
									{
										color = new Color(cursorAlpha, cursorAlpha, cursorAlpha, cursorAlpha);
										SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.STAR_4, num12 + (int)(40 * Main.ScreenMultiplier), num7 + (int)(12 * Main.ScreenMultiplier), color, (float)(Main.FrameCounter * (1.0 / (8.0 * Math.PI))), (float)(0.8 + Math.Sin(Main.FrameCounter * (1.0 / (16.0 * Math.PI))) * 0.2));
										SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.STAR_4, num12 + (int)(28 * Main.ScreenMultiplier), num7 + (int)(28 * Main.ScreenMultiplier), color, (float)(Main.FrameCounter * (1.0 / (6.0 * Math.PI))), (float)(0.6 + Math.Sin(Main.FrameCounter * (1.0 / (24.0 * Math.PI))) * 0.4));
										SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.STAR_4, num12 + (int)(16 * Main.ScreenMultiplier), num7 + (int)(20 * Main.ScreenMultiplier), color, (float)(Main.FrameCounter * 0.079577471545947673), (float)(0.7 + Math.Sin(Main.FrameCounter * (1.0 / (32.0 * Math.PI))) * 0.3));
									}
								}
								num12 += Base;
							}
						}
						num7 += Base;
					}
				}
				Main.SpriteBatch.End();
				Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, CurrentView.ScreenProjection);
				if (craftingRecipeScrollX == 0f && craftingRecipeScrollY == 0f)
				{
					Rectangle rect2 = default;

#if VERSION_103 || VERSION_FINAL
					if (num5 > Base)
					{
						if ((Main.frameCounter & 0x40) == 0)
						{
							rect2.X = num2 - (int)(16 * Main.ScreenMultiplier);
							rect2.Y = num3 + Base;
							rect2.Width = (int)(16 * Main.ScreenMultiplier);
							rect2.Height = Base;
							SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2, SpriteEffects.FlipHorizontally);
							rect2.X += (int)(10 * Main.ScreenMultiplier) + num5;
							SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2);
						}
					}
					if (count > 1)
					{
						if (craftingRecipeY == 0)
						{
							num3 += Base;
							num6 -= Base;
						}
						if ((Main.frameCounter & 0x40) == 0)
						{
							rect2.X = num2;
							rect2.Y = num3 - (int)(16 * Main.ScreenMultiplier);
							rect2.Width = Base;
							rect2.Height = (int)(16 * Main.ScreenMultiplier);
							SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect2, SpriteEffects.FlipVertically);
							rect2.Y += num6 + (int)(10 * Main.ScreenMultiplier);
							SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect2);
						}
						if (craftingRecipeY == 0)
						{
							num3 -= Base;
							num6 += Base;
						}
					}
#else
					if (num5 > Base)
					{
						rect2.X = num2 - (int)(16 * Main.ScreenMultiplier);
						rect2.Y = num3 + Base;
						rect2.Width = (int)(16 * Main.ScreenMultiplier);
						rect2.Height = Base;
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2, SpriteEffects.FlipHorizontally);
						rect2.X += (int)(10 * Main.ScreenMultiplier) + num5;
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref rect2);
					}
					if (count > 1)
					{
						if (CraftingRecipeY == 0)
						{
							num3 += Base;
							num6 -= Base;
						}
						rect2.X = num2;
						rect2.Y = num3 - (int)(16 * Main.ScreenMultiplier);
						rect2.Width = Base;
						rect2.Height = (int)(16 * Main.ScreenMultiplier);
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWD, ref rect2, SpriteEffects.FlipVertically);
						rect2.Y += num6 + (int)(10 * Main.ScreenMultiplier);
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWD, ref rect2);
						if (CraftingRecipeY == 0)
						{
							num3 -= Base;
							num6 += Base;
						}
					}
#endif
				}
				int num16 = (int)(num2 + (64 * Main.ScreenMultiplier));
				int num17 = (int)(num3 + (120 * Main.ScreenMultiplier));
				Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK2, new Rectangle(num16, num17, (int)(CRAFTING_X + INVENTORY_W - TOOLTIP_W - (32 * Main.ScreenMultiplier) - num16), (int)(174 * Main.ScreenMultiplier)), 96);
				SmallFont.MeasureString(Lang.MenuText[0]);
				DrawStringCT(SmallFont, Lang.MenuText[0], (int)(num16 + (100 * Main.ScreenMultiplier)), (int)(num17 + (144 * Main.ScreenMultiplier)), Color.White);
				inventoryScale = 0.9f * Main.ScreenMultiplier;
				int num18 = 0;
				for (int m = 0; m < 3; m++)
				{
					int num19 = 0;
					while (num19 < 4)
					{
						int PosOffset = (int)(50 * Main.ScreenMultiplier);
						int num12 = num16 + PosOffset * num19;
						int num7 = num17 + PosOffset * m;
						if (craftingSection == CraftingSection.INGREDIENTS && num19 == craftingIngredientX && m == craftingIngredientY)
						{
							DrawInventoryCursor(num12, num7, inventoryScale);
							if (num18 < CraftingRecipe.NumRequiredItems)
							{
								toolTip = CraftingRecipe.RequiredItem[num18];
								text = "(";
								text += ActivePlayer.CountPossession(toolTip.NetID).ToStringLookup();
								text += Lang.MenuText[1];
								UpdateToolTipText(text);
							}
						}
						else
						{
							int id2 = (int)_sheetSprites.ID.INVENTORY_BACK3;
							color = ((num18 < CraftingRecipe.NumRequiredItems) ? color2 : new Color(64, 64, 64, 64));
							SpriteSheet<_sheetSprites>.DrawTL(id2, num12, num7, color, inventoryScale);
						}
						if (num18 < CraftingRecipe.NumRequiredItems)
						{
							DrawInventoryItem(ref CraftingRecipe.RequiredItem[num18], num12, num7, new Color(255, 255, 255, 255), StackType.INGREDIENT);
						}
						num19++;
						num18++;
					}
				}
				int num20 = -1;
				Main.StrBuilder.Length = 0;
				if (CraftingRecipe.NumRequiredTiles > 0)
				{
					Main.StrBuilder.Append(Main.TileNames[CraftingRecipe.RequiredTile[0]]);
					switch (CraftingRecipe.RequiredTile[0])
					{
						case 13:
							num20 = (int)_sheetSprites.ID.ALCHEMYSTATION;
							break;
						case 14:
							num20 = (int)_sheetSprites.ID.TABLECHAIR;
							Main.StrBuilder.Append(" & ");
							Main.StrBuilder.Append(Main.TileNames[15]);
							break;
						case 16:
							num20 = (int)_sheetSprites.ID.ITEM_35;
							break;
						case 17:
							num20 = (int)_sheetSprites.ID.ITEM_33;
							break;
						case 18:
							if (CraftingRecipe.RequiredTile[1] == 15)
							{
								num20 = (int)_sheetSprites.ID.WORKBENCHCHAIR;
								Main.StrBuilder.Append(" & ");
								Main.StrBuilder.Append(Main.TileNames[15]);
							}
							else
							{
								num20 = (int)_sheetSprites.ID.ITEM_36;
							}
							break;
						case 26:
							num20 = (int)_sheetSprites.ID.DEMONALTAR;
							break;
						case 86:
							num20 = (int)_sheetSprites.ID.ITEM_332;
							break;
						case 94:
							num20 = (int)_sheetSprites.ID.ITEM_352;
							break;
						case 96:
							num20 = (int)_sheetSprites.ID.ITEM_345;
							break;
						case 101:
							num20 = (int)_sheetSprites.ID.BOOKCASE;
							break;
						case 106:
							num20 = (int)_sheetSprites.ID.ITEM_363;
							break;
						case 114:
							num20 = (int)_sheetSprites.ID.ITEM_398;
							break;
						case 134:
							num20 = (int)_sheetSprites.ID.ITEM_525;
							break;
					}
				}
				else if (CraftingRecipe.NeedsWater)
				{
					num20 = (int)_sheetSprites.ID.WATER;
					Main.StrBuilder.Append(Lang.InterfaceText[53]);
				}
				if (num20 >= 0)
				{
					Rectangle rect3 = default(Rectangle);
					rect3.X = (int)(num16 + (280 * Main.ScreenMultiplier));
					rect3.Y = (int)(num17 + (46 * Main.ScreenMultiplier));
					rect3.Width = (int)(68 * Main.ScreenMultiplier);
					rect3.Height = (int)(68 * Main.ScreenMultiplier);
					Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK3, rect3, 192);
					int width2 = SpriteSheet<_sheetSprites>.Source[num20].Width;
					int height = SpriteSheet<_sheetSprites>.Source[num20].Height;
					float scaleCenter = ((width2 <= height) ? (64f / height) : (64f / width2));
					Vector2 pos = default(Vector2);
					pos.X = rect3.Center.X;
					pos.Y = rect3.Center.Y;
					SpriteSheet<_sheetSprites>.DrawScaled(num20, ref pos, Color.White, (float)(scaleCenter * Main.ScreenMultiplier));
					DrawStringCT(SmallFont, rect3.Center.X, rect3.Bottom + 2, ActivePlayer.IsNearCraftingStation(CraftingRecipe) ? Color.White : new Color(255, 64, 64, 255));
				}
				if (text != null)
				{
					DrawToolTip((int)(CRAFTING_X + INVENTORY_W - TOOLTIP_W - (8 * Main.ScreenMultiplier)), (int)(num3 + (8 * Main.ScreenMultiplier)), INVENTORY_CLIENT_H - INVENTORY_CLIENT_Y_OFFSET);
				}
			}
#endif
					DrawControlsCrafting();
		}

		private void DrawMiniMap()
		{
			int sAFE_AREA_OFFSET_L = CurrentView.SafeAreaOffsetLeft;
			int sAFE_AREA_OFFSET_T = CurrentView.SafeAreaOffsetTop;
#if USE_ORIGINAL_CODE
			int width = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetLeft - CurrentView.SafeAreaOffsetRight;
			int height = Main.ResolutionHeight - CurrentView.SafeAreaOffsetTop - CurrentView.SafeAreaOffsetBottom - 36;
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, new Rectangle(sAFE_AREA_OFFSET_L, sAFE_AREA_OFFSET_T, width, height), 128);
			int num = 31 + sAFE_AREA_OFFSET_L;
			int num2 = 2 + sAFE_AREA_OFFSET_T;
			if (mapScreenCursorX == 0 && mapScreenCursorY < 2)
			{
				DrawInventoryCursor(num, num2, 1.0);
			}
			else
			{
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.INVENTORY_BACK, num, num2);
			}
			Color white = Color.White;
			if (pvpSelected)
			{
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.ITEM_4, num + 9, num2 + 11, white);
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.ITEM_4, num + 11, num2 + 11, white, SpriteEffects.FlipHorizontally);
			}
			else
			{
				SpriteSheet<_sheetSprites>.DrawRotatedTL((int)_sheetSprites.ID.ITEM_4, num - 7, num2 + 25, white, -0.785f);
				SpriteSheet<_sheetSprites>.DrawRotatedTL((int)_sheetSprites.ID.ITEM_4, num + 11, num2 + 25, white, -0.785f);
			}
			int num3 = num + 123;
			for (int i = 0; i < 5; i++)
			{
				white = Main.TeamColors[i];
				if (i == teamSelected)
				{
					white.A = MouseTextBrightness;
				}
				else
				{
					white.R >>= 1;
					white.G >>= 1;
					white.B >>= 1;
					white.A >>= 1;
				}
				int num4 = num3;
				int num5 = num2 + 3;
				switch (i)
				{
				case 0:
					num4 -= 32;
					num5 += 16;
					break;
				case 2:
					num4 += 32;
					break;
				case 3:
					num5 += 32;
					break;
				default:
					num4 += 32;
					num5 += 32;
					break;
				case 1:
					break;
				}
				num4 -= 8;
				num5 -= 8;
				if ((i == 0 && mapScreenCursorX == 1) || (i == 1 && mapScreenCursorX == 2 && mapScreenCursorY == 0) || (i == 2 && mapScreenCursorX == 3 && mapScreenCursorY == 0) || (i == 3 && mapScreenCursorX == 2 && mapScreenCursorY == 1) || (i == 4 && mapScreenCursorX == 3 && mapScreenCursorY == 1))
				{
					DrawInventoryCursor(num4, num5, 8.0 / 13); // Selected Team
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num4, num5, Color.White, 8f / 13);
				}
				num4 += 8;
				num5 += 8;
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.TEAM, num4, num5, white);
			}
			int num6 = sAFE_AREA_OFFSET_L;
			int num7 = 98 + sAFE_AREA_OFFSET_T;
			for (int num8 = 8; num8 > 0; num8--)
			{
				int num9 = num7 + 44 * (num8 - 1);
				if (mapScreenCursorY - 1 == num8)
				{
					DrawInventoryCursor(num6 - 2, num9 - 2, 12.0 / 13); // Selected Player
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num6 + 2, num9 + 2, Color.White, 10f / 13);
				}
			}
			Main.SpriteBatch.End();
			GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
			for (int j = 0; j < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count; j++)
			{
				NetworkGamer networkGamer = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[j];
				Player player = networkGamer.Tag as Player;
				if (player != null)
				{
					DrawPlayerIcon(player, new Vector2(num6 + 8, num7 + 8 + j * 44), 1.5f);
				}
			}
			miniMap.DrawMap(CurrentView);
			DrawStringCC(SmallFont, Lang.MenuText[81], num + 26, num2 + 69, Color.White);
			DrawStringCC(SmallFont, Lang.MenuText[82], num3 + 16, num2 + 69, Color.White);
			for (int k = 0; k < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count; k++)
			{
				NetworkGamer networkGamer2 = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[k];
				Player player2 = networkGamer2.Tag as Player;
				if (player2 != null)
				{
					int y = num7 + 4 + 44 * k;
					DrawStringLT(BoldSmallFont, player2.Name, num6 + 52, y, Main.TeamColors[player2.team]); // I think this can stay as .name since it is network-related and as such, used in multiplayer.
				}
			}
#else
			int width = CurrentView.ViewWidth - CurrentView.SafeAreaOffsetLeft - CurrentView.SafeAreaOffsetRight;
			int height = Main.ResolutionHeight - CurrentView.SafeAreaOffsetTop - CurrentView.SafeAreaOffsetBottom - (int)(36 * Main.ScreenMultiplier);
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, new Rectangle(sAFE_AREA_OFFSET_L, sAFE_AREA_OFFSET_T, width, height), 128);
			int num = (int)(31 * Main.ScreenMultiplier) + sAFE_AREA_OFFSET_L;
			int num2 = (int)(2 * Main.ScreenMultiplier) + sAFE_AREA_OFFSET_T;
			Color white = Color.White;
			if (mapScreenCursorX == 0 && mapScreenCursorY < 2)
			{
				DrawInventoryCursor(num, num2, Main.ScreenMultiplier);
			}
			else
			{
				SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.INVENTORY_BACK, num, num2, white, Main.ScreenMultiplier);
			}
			if (pvpSelected)
			{
				Vector2 Position = new Vector2(num + (int)(9 * Main.ScreenMultiplier), num2 + (int)(11 * Main.ScreenMultiplier));
				Vector2 Default = default;
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.ITEM_4, ref Position, white, 0f, ref Default, Main.ScreenMultiplier, SpriteEffects.None);
				Position.X += (int)(2 * Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.ITEM_4, ref Position, white, 0f, ref Default, Main.ScreenMultiplier, SpriteEffects.FlipHorizontally);
			}
			else
			{
				SpriteSheet<_sheetSprites>.DrawRotatedTL((int)_sheetSprites.ID.ITEM_4, num - (int)(7 * Main.ScreenMultiplier), num2 + (int)(25 * Main.ScreenMultiplier), white, -0.785f, Main.ScreenMultiplier);
				SpriteSheet<_sheetSprites>.DrawRotatedTL((int)_sheetSprites.ID.ITEM_4, num + (int)(11 * Main.ScreenMultiplier), num2 + (int)(25 * Main.ScreenMultiplier), white, -0.785f, Main.ScreenMultiplier);
			}
			int num3 = num + (int)(123 * Main.ScreenMultiplier);
			for (int i = 0; i < 5; i++)
			{
				white = Main.TeamColors[i];
				if (i == teamSelected)
				{
					white.A = MouseTextBrightness;
				}
				else
				{
					white.R >>= 1;
					white.G >>= 1;
					white.B >>= 1;
					white.A >>= 1;
				}
				int num4 = num3;
				int num5 = num2 + (int)(3 * Main.ScreenMultiplier);
				switch (i)
				{
					case 0:
						num4 -= (int)(32 * Main.ScreenMultiplier);
						num5 += (int)(16 * Main.ScreenMultiplier);
						break;
					case 2:
						num4 += (int)(32 * Main.ScreenMultiplier);
						break;
					case 3:
						num5 += (int)(32 * Main.ScreenMultiplier);
						break;
					default:
						num4 += (int)(32 * Main.ScreenMultiplier);
						num5 += (int)(32 * Main.ScreenMultiplier);
						break;
					case 1:
						break;
				}
				num4 -= 8;
				num5 -= 8;
				if ((i == 0 && mapScreenCursorX == 1) || (i == 1 && mapScreenCursorX == 2 && mapScreenCursorY == 0) || (i == 2 && mapScreenCursorX == 3 && mapScreenCursorY == 0) || (i == 3 && mapScreenCursorX == 2 && mapScreenCursorY == 1) || (i == 4 && mapScreenCursorX == 3 && mapScreenCursorY == 1))
				{
					DrawInventoryCursor(num4, num5, (8f * Main.ScreenMultiplier) / 13); // Selected Team
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num4, num5, Color.White, (8f * Main.ScreenMultiplier) / 13);
				}
				num4 += (int)(8 * Main.ScreenMultiplier);
				num5 += (int)(8 * Main.ScreenMultiplier);
				if (Main.ScreenHeightPtr == 1)
				{
					num4 += 1;
					num5 += 1;
				}
				SpriteSheet<_sheetSprites>.DrawTL((int)_sheetSprites.ID.TEAM, num4, num5, white, Main.ScreenMultiplier);
			}
			int num6 = sAFE_AREA_OFFSET_L;
			int num7 = (int)(98 * Main.ScreenMultiplier) + sAFE_AREA_OFFSET_T;
			for (int num8 = 8; num8 > 0; num8--)
			{
				int num9 = num7 + (int)(44 * Main.ScreenMultiplier) * (num8 - 1);
				if (mapScreenCursorY - 1 == num8)
				{
					DrawInventoryCursor(num6 - (int)(2 * Main.ScreenMultiplier), num9 - (int)(2 * Main.ScreenMultiplier), (12f * Main.ScreenMultiplier) / 13); // Selected Player
				}
				else
				{
					SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num6 + (int)(2 * Main.ScreenMultiplier), num9 + (int)(2 * Main.ScreenMultiplier), Color.White, (10.5f * Main.ScreenMultiplier) / 13);
				}
			}
			Main.SpriteBatch.End();
			GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
			for (int j = 0; j < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count; j++)
			{
				NetworkGamer networkGamer = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[j];
				Player player = networkGamer.Tag as Player;
				if (player != null)
				{
					DrawPlayerIcon(player, new Vector2(num6 + (int)(8 * Main.ScreenMultiplier), num7 + (int)(8 * Main.ScreenMultiplier) + j * (int)(44 * Main.ScreenMultiplier)), 1.5f * Main.ScreenMultiplier);
				}
			}
			miniMap.DrawMap(CurrentView);
			DrawStringCC(SmallFont, Lang.MenuText[81], num + (int)(26 * Main.ScreenMultiplier), num2 + (int)(69 * Main.ScreenMultiplier), Color.White);
			DrawStringCC(SmallFont, Lang.MenuText[82], num3 + (int)(16 * Main.ScreenMultiplier), num2 + (int)(69 * Main.ScreenMultiplier), Color.White);
			for (int k = 0; k < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count; k++)
			{
				NetworkGamer networkGamer2 = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[k];
				Player player2 = networkGamer2.Tag as Player;
				if (player2 != null)
				{
					int y = num7 + 8 + 88 * k;
					DrawStringLT(BoldSmallFont, player2.Name, num6 + (int)(52 * Main.ScreenMultiplier), y, Main.TeamColors[player2.team]); // I think this can stay as .Name since it is network-related and as such, used in multiplayer.
				}
			}
#endif
		}

		private bool AccCheck(ref Item newItem, int slot)
		{
			if (ActivePlayer.armor[slot].NetID == newItem.NetID)
			{
				return false;
			}
			for (int i = Player.NumArmorSlots; i < Player.MaxNumArmor; i++)
			{
				if (newItem.NetID == ActivePlayer.armor[i].NetID)
				{
					return true;
				}
			}
			return false;
		}

		private void UpdateToolTipText(string extraInfo)
		{
			Main.StrBuilder.Length = 0;
			string text2;
			if (toolTip.Type > 0)
			{
				switch (toolTip.Rarity)
				{
				case -1:
						Main.StrBuilder.Append("<f c='#828282'>");
					break;
				case 1:
						Main.StrBuilder.Append("<f c='#9696FF'>");
					break;
				case 2:
						Main.StrBuilder.Append("<f c='#96FF96'>");
					break;
				case 3:
						Main.StrBuilder.Append("<f c='#FFC896'>");
					break;
				case 4:
						Main.StrBuilder.Append("<f c='#FF9696'>");
					break;
				case 5:
						Main.StrBuilder.Append("<f c='#FF96FF'>");
					break;
				case 6:
						Main.StrBuilder.Append("<f c='#D2A0FF'>");
					break;
				default:
						Main.StrBuilder.Append("<f c='#FFFFD2'>");
					break;
				}
				Main.StrBuilder.Append(toolTip.AffixName());
				if (toolTip.Stack > 1)
				{
					Main.StrBuilder.Append(toolTip.Stack.ToStackString());
				}
				Main.StrBuilder.Append("</f>\n");
				if (extraInfo != null)
				{
					Main.StrBuilder.Append(extraInfo);
					Main.StrBuilder.Append('\n');
				}
				if (npcShop > 0 || (reforge && toolTip.SetPrefix(-3)))
				{
					if (toolTip.Value > 0)
					{
						int num = toolTip.Value * toolTip.Stack;
						if (reforge)
						{
							Main.StrBuilder.Append(Lang.InterfaceText[46]);
						}
						else if (toolTip.CanBuy)
						{
							Main.StrBuilder.Append(Lang.TipText[50]);
						}
						else
						{
							num /= 5;
							Main.StrBuilder.Append(Lang.TipText[49]);
						}
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int num5 = 0;
						if (num < 1)
						{
							num = 1;
						}
						if (num >= 1000000)
						{
							num2 = num / 1000000;
							num -= num2 * 1000000;
						}
						if (num >= 10000)
						{
							num3 = num / 10000;
							num -= num3 * 10000;
						}
						if (num >= 100)
						{
							num4 = num / 100;
							num -= num4 * 100;
						}
						if (num >= 1)
						{
							num5 = num;
						}
						if (num2 > 0)
						{
							Main.StrBuilder.Append("<f c='#DCDCC6'>");
							Main.StrBuilder.Append(num2.ToStringLookup());
							Main.StrBuilder.Append(Lang.InterfaceText[15]);
							Main.StrBuilder.Append("</f>");
						}
						if (num3 > 0)
						{
							Main.StrBuilder.Append("<f c='#E0C95C'>");
							Main.StrBuilder.Append(num3.ToStringLookup());
							Main.StrBuilder.Append(Lang.InterfaceText[16]);
							Main.StrBuilder.Append("</f>");
						}
						if (num4 > 0)
						{
							Main.StrBuilder.Append("<f c='#B5C0C1'>");
							Main.StrBuilder.Append(num4.ToStringLookup());
							Main.StrBuilder.Append(Lang.InterfaceText[17]);
							Main.StrBuilder.Append("</f>");
						}
						if (num5 > 0)
						{
							Main.StrBuilder.Append("<f c='#F68A60'>");
							Main.StrBuilder.Append(num5.ToStringLookup());
							Main.StrBuilder.Append(Lang.InterfaceText[18]);
							Main.StrBuilder.Append("</f>");
						}
						Main.StrBuilder.Append('\n');
					}
					else
					{
						Main.StrBuilder.Append("<f c='#787878'>");
						Main.StrBuilder.Append(Lang.TipText[51]);
						Main.StrBuilder.Append("</f>\n");
					}
				}
				if (toolTip.IsMaterial)
				{
					Main.StrBuilder.Append("¤ ");
					Main.StrBuilder.Append(Lang.TipText[36]);
					Main.StrBuilder.Append('\n');
				}
				if (toolTip.CreateWall > 0 || toolTip.CreateTile >= 0)
				{
					if (toolTip.Type != (int)Item.ID.STAFF_OF_REGROWTH)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[33]);
						Main.StrBuilder.Append('\n');
					}
				}
				else if (toolTip.Ammo > 0)
				{
					Main.StrBuilder.Append("¤ ");
					Main.StrBuilder.Append(Lang.TipText[34]);
					Main.StrBuilder.Append('\n');
				}
				else if (toolTip.IsConsumable)
				{
					Main.StrBuilder.Append("¤ ");
					Main.StrBuilder.Append(Lang.TipText[35]);
					Main.StrBuilder.Append('\n');
				}
				if (toolTip.IsSocial)
				{
					Main.StrBuilder.Append(Lang.TipText[0]);
					Main.StrBuilder.Append('\n');
					Main.StrBuilder.Append(Lang.TipText[1]);
					Main.StrBuilder.Append('\n');
				}
				else
				{
					if (toolTip.Damage > 0)
					{
						int damage = toolTip.Damage;
						int num6 = 0;
						if (toolTip.IsMelee)
						{
							Main.StrBuilder.Append("¤ ");
							Main.StrBuilder.Append(((int)(ActivePlayer.meleeDamage * damage)).ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[2]);
							num6 = ActivePlayer.meleeCrit;
						}
						else if (toolTip.IsRanged)
						{
							Main.StrBuilder.Append("¤ ");
							Main.StrBuilder.Append(((int)(ActivePlayer.rangedDamage * damage)).ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[3]);
							num6 = ActivePlayer.rangedCrit;
						}
						else if (toolTip.IsMagic)
						{
							Main.StrBuilder.Append("¤ ");
							Main.StrBuilder.Append(((int)(ActivePlayer.magicDamage * damage)).ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[4]);
							num6 = ActivePlayer.magicCrit;
						}
						num6 -= ActivePlayer.Inventory[ActivePlayer.SelectedItem].Crit;
						num6 += toolTip.Crit;
						Main.StrBuilder.Append("\n¤ ");
						Main.StrBuilder.Append(num6.ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[5]);
						Main.StrBuilder.Append('\n');
						if (toolTip.UseStyle > 0)
						{
							int num7 = 13;
							if (toolTip.UseAnimation <= 8)
							{
								num7 = 6;
							}
							else if (toolTip.UseAnimation <= 20)
							{
								num7 = 7;
							}
							else if (toolTip.UseAnimation <= 25)
							{
								num7 = 8;
							}
							else if (toolTip.UseAnimation <= 30)
							{
								num7 = 9;
							}
							else if (toolTip.UseAnimation <= 35)
							{
								num7 = 10;
							}
							else if (toolTip.UseAnimation <= 45)
							{
								num7 = 11;
							}
							else if (toolTip.UseAnimation <= 55)
							{
								num7 = 12;
							}
							Main.StrBuilder.Append("¤ ");
							Main.StrBuilder.Append(Lang.TipText[num7]);
							Main.StrBuilder.Append('\n');
						}
						int num8 = 22;
						double num9 = toolTip.Knockback;
						if (ActivePlayer.kbGlove)
						{
							num9 *= 1.7;
						}
						if (num9 == 0.0)
						{
							num8 = 14;
						}
						else if (num9 <= 1.5)
						{
							num8 = 15;
						}
						else if (num9 <= 3.0)
						{
							num8 = 16;
						}
						else if (num9 <= 4.0)
						{
							num8 = 17;
						}
						else if (num9 <= 6.0)
						{
							num8 = 18;
						}
						else if (num9 <= 7.0)
						{
							num8 = 19;
						}
						else if (num9 <= 9.0)
						{
							num8 = 20;
						}
						else if (num9 <= 11.0)
						{
							num8 = 21;
						}
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[num8]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.IsEquipable())
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[23]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.IsVanity)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[24]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.Defense > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(toolTip.Defense.ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[25]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.PickPower > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(((int)toolTip.PickPower).ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[26]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.AxePower > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append((toolTip.AxePower * 5).ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[27]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.HammerPower > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(((int)toolTip.HammerPower).ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[28]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.HealLife > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[29]);
						Main.StrBuilder.Append(toolTip.HealLife.ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[30]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.HealMana > 0)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[29]);
						Main.StrBuilder.Append(toolTip.HealMana.ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[31]);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.Mana > 0 && (toolTip.Type != (int)Item.ID.SPACE_GUN || !ActivePlayer.spaceGun))
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[32]);
						Main.StrBuilder.Append(((int)(toolTip.Mana * ActivePlayer.manaCost)).ToStringLookup());
						Main.StrBuilder.Append(Lang.TipText[31]);
						Main.StrBuilder.Append('\n');
					}
					string text = Lang.ToolTip(toolTip.NetID);
					if (text != null)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(text);
						Main.StrBuilder.Append('\n');
					}
					text = Lang.ToolTip2(toolTip.NetID);
					if (text != null)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(text);
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.BuffTime > 0)
					{
						Main.StrBuilder.Append("¤ ");
						if (toolTip.BuffTime / 60 >= 60)
						{
							Main.StrBuilder.Append(((int)Math.Round(toolTip.BuffTime / 60 / 60.0)).ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[37]);
						}
						else
						{
							Main.StrBuilder.Append(((int)Math.Round(toolTip.BuffTime / 60.0)).ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[38]);
						}
						Main.StrBuilder.Append('\n');
					}
					if (toolTip.PrefixType > 0)
					{
						if (cpItem.NetID != toolTip.NetID)
						{
							cpItem.NetDefaults(toolTip.NetID);
						}
						if (cpItem.Damage != toolTip.Damage)
						{
							int num10 = toolTip.Damage - cpItem.Damage;
							num10 = (int)Math.Round(num10 * 100.0 / cpItem.Damage);
							if (num10 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num10.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[39]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.UseAnimation != toolTip.UseAnimation)
						{
							int num11 = toolTip.UseAnimation - cpItem.UseAnimation;
#if (!VERSION_INITIAL || IS_PATCHED)
							num11 = (int)Math.Round(num11 * -100.0 / cpItem.UseAnimation);
#else
							num11 = (int)Math.Round((double)num11 * 100.0 / (double)(int)cpItem.UseAnimation);
#endif
							if (num11 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num11.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[40]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.Crit != toolTip.Crit)
						{
							int num12 = toolTip.Crit - cpItem.Crit;
							if (num12 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num12.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[41]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.Mana != toolTip.Mana)
						{
							int num13 = toolTip.Mana - cpItem.Mana;
							num13 = (int)Math.Round(num13 * 100.0 / cpItem.Mana);
							if (num13 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num13.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[42]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.Scale != toolTip.Scale)
						{
							int num14 = (int)Math.Round((double)(toolTip.Scale - cpItem.Scale) * 100.0 / cpItem.Scale);
							if (num14 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num14.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[43]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.ShootSpeed != toolTip.ShootSpeed)
						{
							int num15 = (int)Math.Round((double)(toolTip.ShootSpeed - cpItem.ShootSpeed) * 100.0 / cpItem.ShootSpeed);
							if (num15 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num15.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[44]);
							Main.StrBuilder.Append("</f>\n");
						}
						if (cpItem.Knockback != toolTip.Knockback)
						{
							int num16 = (int)Math.Round((double)(toolTip.Knockback - cpItem.Knockback) * 100.0 / cpItem.Knockback);
							if (num16 > 0)
							{
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+");
							}
							else
							{
								Main.StrBuilder.Append("¤ <f c='#BE7878'>");
							}
							Main.StrBuilder.Append(num16.ToStringLookup());
							Main.StrBuilder.Append(Lang.TipText[45]);
							Main.StrBuilder.Append("</f>\n");
						}
						switch (toolTip.PrefixType)
						{
						case 62:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+1");
								Main.StrBuilder.Append(Lang.TipText[25]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 63:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+2");
								Main.StrBuilder.Append(Lang.TipText[25]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 64:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+3");
								Main.StrBuilder.Append(Lang.TipText[25]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 65:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+4");
								Main.StrBuilder.Append(Lang.TipText[25]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 66:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+20");
								Main.StrBuilder.Append(Lang.TipText[31]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 67:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+1");
								Main.StrBuilder.Append(Lang.TipText[5]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 68:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+2");
								Main.StrBuilder.Append(Lang.TipText[5]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 69:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+1");
								Main.StrBuilder.Append(Lang.TipText[39]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 70:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+2");
								Main.StrBuilder.Append(Lang.TipText[39]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 71:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+3");
								Main.StrBuilder.Append(Lang.TipText[39]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 72:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+4");
								Main.StrBuilder.Append(Lang.TipText[39]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 73:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+1");
								Main.StrBuilder.Append(Lang.TipText[46]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 74:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+2");
								Main.StrBuilder.Append(Lang.TipText[46]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 75:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+3");
								Main.StrBuilder.Append(Lang.TipText[46]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 76:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+4");
								Main.StrBuilder.Append(Lang.TipText[46]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 77:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+1");
								Main.StrBuilder.Append(Lang.TipText[47]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 78:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+2");
								Main.StrBuilder.Append(Lang.TipText[47]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 79:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+3");
								Main.StrBuilder.Append(Lang.TipText[47]);
								Main.StrBuilder.Append("</f>\n");
							break;
						case 80:
								Main.StrBuilder.Append("¤ <f c='#78BE78'>+4");
								Main.StrBuilder.Append(Lang.TipText[47]);
								Main.StrBuilder.Append("</f>\n");
							break;
						}
					}
					if (toolTip.WornArmor && ActivePlayer.setBonus != null)
					{
						Main.StrBuilder.Append("¤ ");
						Main.StrBuilder.Append(Lang.TipText[48]);
						Main.StrBuilder.Append(ActivePlayer.setBonus);
						Main.StrBuilder.Append('\n');
					}
				}
				text2 = Main.StrBuilder.ToString();
			}
			else
			{
				text2 = extraInfo;
				if (text2 == null)
				{
					text2 = "";
				}
			}
			if (text2 != toolTipText)
			{
				toolTipText = text2;
				compiledToolTipText = new CompiledText(text2, TOOLTIP_W, BoldSmallTextStyle);
			}
		}

		private void DrawToolTip(int TOOLTIP_X, int TOOLTIP_Y, int TOOLTIP_H)
		{
			Rectangle rectangle = new Rectangle(TOOLTIP_X, TOOLTIP_Y, TOOLTIP_W, TOOLTIP_H);
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, rectangle, 255);
			compiledToolTipText.Draw(Main.SpriteBatch, rectangle, new Color(255, 255, 255, 255), new Color(255, 212, 64, 255));
		}

		public void OpenInventory()
		{
			InventoryMode = 1;
			oldMouseX = MouseX;
			oldMouseY = MouseY;
			ClearButtonTriggers();
			if (worldFadeTarget > INVENTORY_FADE)
			{
				worldFadeTarget = INVENTORY_FADE;
			}
			CraftingRecipeX = 0;
			CraftingRecipeY = 0;
			ActivePlayer.AdjTiles();
			if (npcShop > 0)
			{
				restoreOldInventorySection = true;
				oldInventorySection = ActiveInvSection;
				ActiveInvSection = InventorySection.CHEST;
				inventoryChestX = 0;
				inventoryChestY = 0;
			}
			else if (ActivePlayer.PlayerChest != -1)
			{
				restoreOldInventorySection = true;
				oldInventorySection = ActiveInvSection;
				ActiveInvSection = InventorySection.CHEST;
				inventoryChestX = -1;
				inventoryChestY = 1;
			}
			else if (reforge || CraftGuide)
			{
				restoreOldInventorySection = true;
				oldInventorySection = ActiveInvSection;
				ActiveInvSection = InventorySection.ITEMS;
			}
			else if (ActiveInvSection == InventorySection.CRAFTING)
			{
				Recipe.FindRecipes(this, craftingCategory, craftingShowCraftable);
			}
		}

		public void CloseInventory()
		{
			if (InventoryMode > 0)
			{
				if (restoreOldInventorySection)
				{
					restoreOldInventorySection = false;
					ActiveInvSection = oldInventorySection;
				}
				InventoryMode = 0;
				ActivePlayer.PlayerChest = -1;
				ActivePlayer.TalkNPC = -1;
				npcShop = 0;
				reforge = false;
				CraftGuide = false;
				MouseX = oldMouseX;
				MouseY = oldMouseY;
				toolTip.Init();
				ClearButtonTriggers();
				if (worldFadeTarget == INVENTORY_FADE)
				{
					worldFadeTarget = 1f;
				}
				hotbarItemNameTime = HOTBAR_ITEMNAME_DISPLAYTIME;
			}
		}

		private bool IsInventorySectionAvailable(InventorySection section)
		{
#if (!VERSION_INITIAL || IS_PATCHED)
            bool result = ActivePlayer.PlayerChest < -1 || (ActivePlayer.PlayerChest >= 0 && Main.ChestSet[ActivePlayer.PlayerChest] != null) || npcShop > 0;
#else
			bool result = ActivePlayer.PlayerChest != -1 || npcShop > 0;
#endif
            switch (section)
			{
			case InventorySection.CHEST:
				return result;
			case InventorySection.HOUSING:
				if (mouseItem.Type == 0 && !reforge)
				{
					return !CraftGuide;
				}
				return false;
			case InventorySection.CRAFTING:
				if (mouseItem.Type == 0 && !reforge)
				{
					if (CraftGuide)
					{
						return GuideItem.Type > 0;
					}
					return true;
				}
				return false;
			case InventorySection.EQUIP:
				if (mouseItem.Type != 0)
				{
					return mouseItem.IsEquipable();
				}
				return true;
			default:
				return true;
			}
		}

		private void UpdateInventoryMenu()
		{
			int num = (int)ActiveInvSection;
			bool flag = true;
			bool flag2 = IsButtonTriggered(BTN_NEXT_ITEM);
			bool flag3 = IsButtonTriggered(BTN_PREV_ITEM);
			do
			{
				if (flag2)
				{
					if (++num == 5)
					{
						num = 0;
					}
				}
				else if (flag3)
				{
					if (--num < 0)
					{
						num = 4;
					}
				}
				else if (!flag)
				{
					num = 1;
				}
				flag = IsInventorySectionAvailable((InventorySection)num);
			}
			while (!flag);
			ActiveInvSection = (InventorySection)num;
			if (flag3 || flag2)
			{
				toolTip.Init();
				if (ActiveInvSection == InventorySection.CRAFTING)
				{
					Recipe.FindRecipes(this, craftingCategory, craftingShowCraftable);
				}
			}
			if (PadState.IsButtonUp(BTN_INVENTORY_ACTION) && PadState.IsButtonUp(BTN_INVENTORY_SELECT))
			{
				stackSplit = 0;
			}
			else if (stackSplit > 0)
			{
				stackSplit--;
			}
			if (stackSplit == 0)
			{
				stackCounter = 0;
				stackDelay = 7;
			}
			else if (++stackCounter >= 30)
			{
				stackCounter = 0;
				if (--stackDelay < 2)
				{
					stackDelay = 2;
				}
			}
		}

		public void PositionInventoryCursor(int dx, int dy)
		{
			while (true)
			{
				switch (ActiveInvSection)
				{
				case InventorySection.ITEMS:
					inventoryItemX += (sbyte)dx;
					if (inventoryItemX < 0)
					{
						inventoryItemX += 10;
					}
					else if (inventoryItemX >= 10)
					{
						inventoryItemX -= 10;
					}
					inventoryItemY += (sbyte)dy;
					if (inventoryItemY < 0)
					{
						inventoryItemY += 7;
					}
					else if (inventoryItemY >= 7)
					{
						inventoryItemY -= 7;
					}
					if (inventoryItemY == 4)
					{
						if (reforge)
						{
							dy |= 1;
							break;
						}
						if (inventoryItemX < 6)
						{
							break;
						}
					}
					else if (inventoryItemY == 5)
					{
						if (reforge)
						{
							dy |= 1;
							break;
						}
						if (inventoryItemX < 6)
						{
							break;
						}
					}
					else if (inventoryItemY == 6 && inventoryItemX < 9)
					{
						break;
					}
					UpdateInventory();
					return;
				case InventorySection.CHEST:
					inventoryChestX += (sbyte)dx;
					if (inventoryChestX < -1)
					{
						inventoryChestX += 6;
					}
					else if (inventoryChestX >= 5)
					{
						inventoryChestX -= 6;
					}
					inventoryChestY += (sbyte)dy;
					if (inventoryChestY < 0)
					{
						inventoryChestY += 4;
					}
					else if (inventoryChestY >= 4)
					{
						inventoryChestY -= 4;
					}
					if (inventoryChestX < 0)
					{
						if (npcShop > 0 || mouseItem.Type > 0)
						{
							if (dx == 0)
							{
								inventoryChestX = 0;
							}
							break;
						}
						if (inventoryChestY == 0)
						{
							inventoryChestY = 1;
						}
					}
					if (npcShop > 0)
					{
						UpdateShop();
					}
					else
					{
						UpdateStorage();
					}
					return;
				case InventorySection.EQUIP:
#if VERSION_FINAL
#elif VERSION_103
					dx2 = dx;
					dy2 = dy;
					if (dy != 0)
					{
						dy2 += inventoryEquipY;
						inventoryEquipY = (sbyte)dy2;
						if (dy2 < 0)
						{
							dy2 += 5;
							inventoryEquipY = (sbyte)dy2;
						}
						else if (4 < dy2)
						{
							dy2 -= 5;
							inventoryEquipY = (sbyte)dy2;
						}
						if (inventoryEquipY == 0)
						{
							inventoryEquipX = 4;
						}
						else if (inventoryEquipY == 1)
						{
							if (0 < dy)
							{
								inventoryEquipX = 0;
								inventoryEquipY = 1;
							}
						}
						else if ((inventoryEquipY == 4) && (dy < 0))
						{
							inventoryEquipX = 2;
							inventoryEquipY = 4;
						}
					}
					dx2 += inventoryEquipX;
					inventoryEquipX = (sbyte)dx2;
					if (inventoryEquipY == 0)
					{
						if (dx2 - 4 != 0)
						{
							dy2 = inventoryBuffX;
							if (dx2 - 4 < 0)
							{
								inventoryEquipX++;
								inventoryBuffX = (sbyte)(dy2 - 1);
								if (dy2 - 1 < 0)
								{
									inventoryBuffX = (sbyte)(dy2 + 15);
								}
								inventoryBuffScrollX -= 1;
							}
							else
							{
								inventoryEquipX--;
								inventoryBuffX = (sbyte)(dy2 + 1);
								if (15 < dy2 + 1)
								{
									inventoryBuffX = (sbyte)(dy2 - 15);
								}
								inventoryBuffScrollX += 1;
							}
						}
					}
					else
					{
						if (inventoryEquipY < 4)
						{
							if (dx2 < 0)
							{
								dx2 = inventoryEquipX + 6;
							}
							else
							{
								if (inventoryEquipX < 6) { goto LAB_8212b7a8; }
								dx2 = inventoryEquipX - 6;
							}
						}
						else if (dx2 < 0)
						{
							dx2 = inventoryEquipX + 5;
						}
						else
						{
							if (inventoryEquipX < 5) { goto LAB_8212b7a8; }
							dx2 = inventoryEquipX - 5;
						}
						inventoryEquipX = (sbyte)dx2;
					}
LAB_8212b7a8:
#else
					inventoryEquipY += (sbyte)dy;
					if (inventoryEquipY < 0)
					{
						inventoryEquipY += 5;
					}
					else if (inventoryEquipY >= 5)
					{
						inventoryEquipY -= 5;
					}
					inventoryEquipX += (sbyte)dx;
					if (inventoryEquipX < 0)
					{
						if (inventoryEquipY == 0)
						{
							inventoryEquipX = 0;
							if (--inventoryBuffX < 0)
							{
								inventoryBuffX = 0;
							}
						}
						else
						{
							inventoryEquipX += 5;
						}
					}
					else if (inventoryEquipX >= 5)
					{
						if (inventoryEquipY == 0)
						{
							inventoryEquipX = 4;
							if (++inventoryBuffX > 5)
							{
								inventoryBuffX = 5;
							}
						}
						else
						{
							inventoryEquipX -= 5;
						}
					}
#endif
					if (inventoryEquipX < 1 || inventoryEquipX > 3 || inventoryEquipY < 1 || inventoryEquipY > 3)
					{
						UpdateEquip();
						return;
					}
					break;
				case InventorySection.HOUSING:
					if (dx != 0)
					{
						inventoryHousingX ^= 1;
					}
					inventoryHousingY += (sbyte)dy;
					if (inventoryHousingY < 0)
					{
						inventoryHousingY += 6;
					}
					else if (inventoryHousingY >= 6)
					{
						inventoryHousingY -= 6;
					}
					if (inventoryHousingX != 1 || inventoryHousingY != 5)
					{
						UpdateHousing();
						return;
					}
					break;
				}
			}
		}

		private bool UpdateCraftButtonInput(Recipe r)
		{
			if (stackSplit <= 1 && PadState.IsButtonDown(Buttons.A) && ActivePlayer.CanCraftRecipe(r))
			{
				int type = mouseItem.Type;
				short stack = mouseItem.Stack;
				mouseItem = r.CraftedItem;
				mouseItem.SetPrefix(-1);
				mouseItem.Stack += stack;
				mouseItem.Position.X = ActivePlayer.XYWH.X + (Player.width / 2) - (mouseItem.Width >> 1);
				mouseItem.Position.Y = ActivePlayer.XYWH.Y + (Player.height / 2) - (mouseItem.Height >> 1);
				mouseItemSrcSection = InventorySection.CRAFTING;
				mouseItemSrcX = -1;
				mouseItemSrcY = -1;
				r.Create(this);
				if (mouseItem.Type > 0 || r.CraftedItem.Type > 0)
				{
					Main.PlaySound(7);
				}
				if (type == 0)
				{
					if (stackSplit == 0)
					{
						stackSplit = 15;
					}
					else
					{
						stackSplit = stackDelay;
					}
					ActivePlayer.GetItem(ref mouseItem);
				}
				return true;
			}
			return false;
		}

		private void PrevCraftingCategory()
		{
			int num = (int)craftingCategory;
			if (--num < 0)
			{
				num = 5;
			}
			Recipe.FindRecipes(this, (Recipe.Category)num, craftingShowCraftable);
			craftingCategory = (Recipe.Category)num;
			CraftingRecipeX = 0;
			CraftingRecipeY = 0;
			craftingRecipeScrollX = 0f;
			craftingRecipeScrollY = 0f;
		}

		private void NextCraftingCategory()
		{
			int num = (int)craftingCategory;
			if (++num == 6)
			{
				num = 0;
			}
			Recipe.FindRecipes(this, (Recipe.Category)num, craftingShowCraftable);
			craftingCategory = (Recipe.Category)num;
			CraftingRecipeX = 0;
			CraftingRecipeY = 0;
			craftingRecipeScrollX = 0f;
			craftingRecipeScrollY = 0f;
		}

		public void PositionCraftingCursor(int dx, int dy)
		{
			if (IsButtonUntriggered(Buttons.X) && mouseItem.Type == 0)
			{
				craftingShowCraftable = !craftingShowCraftable;
				CraftingRecipeX = 0;
				craftingRecipeScrollX = 0f;
				craftingRecipeScrollY = 0f;
				Recipe.FindRecipes(this, craftingCategory, craftingShowCraftable);
			}
			else if (IsButtonUntriggered(Buttons.A))
			{
#if !VERSION_INITIAL
				// This code prevents the crafting cursor from moving from one item to another after the previous item is no longer craftable. Liberties were taken due to state of the decompiled code.
				Recipe PreviousRecipe = CraftingRecipe;
				Recipe.FindRecipes(this, craftingCategory, craftingShowCraftable);
				if (!craftingShowCraftable) // No need to run if we are only showing what is readily available.
				{
					sbyte NewYPos = 0;
					bool HasMatched = false;
					for (int SectionIdx = 0; SectionIdx < CurrentRecipeCategory.Count; SectionIdx++)
					{
						List<short> RecipeSubsection = CurrentRecipeCategory[SectionIdx].RecipeList;
						for (int RecipeIdx = 0; RecipeIdx < RecipeSubsection.Count; RecipeIdx++)
						{
							short RecipeID = RecipeSubsection[RecipeIdx];
							if (Main.ActiveRecipe[RecipeID].CraftedItem.NetID == PreviousRecipe.CraftedItem.NetID)
							{
								bool IsSameRecipe = true;
								for (int IngredientIdx = 0; IngredientIdx < Recipe.MaxItemRequirements; IngredientIdx++)
								{
									if (Main.ActiveRecipe[RecipeID].RequiredItem[IngredientIdx].NetID != PreviousRecipe.RequiredItem[IngredientIdx].NetID)
									{
										IsSameRecipe = false;
									}
								}
								if (IsSameRecipe)
								{
									CraftingRecipeY = NewYPos;
									HasMatched = true;
									break;
								}
							}
						}
						if (HasMatched)
						{
							break;
						}
						else
						{
							NewYPos++;
						}
					}
				}
#else
				Recipe.FindRecipes(this, craftingCategory, craftingShowCraftable);
#endif
			}
			else
			{
				if (UpdateCraftButtonInput(CraftingRecipe))
				{
					return;
				}
				if (IsButtonTriggered(Buttons.Y))
				{
					craftingSection ^= CraftingSection.INGREDIENTS;
				}
				else if (IsButtonTriggered(Buttons.LeftTrigger))
				{
					PrevCraftingCategory();
				}
				else if (IsButtonTriggered(Buttons.RightTrigger))
				{
					NextCraftingCategory();
				}
				else if (dx != 0 || dy != 0)
				{
					if (craftingRecipeScrollMul >= 0.325f)
					{
						craftingRecipeScrollMul -= CRAFTING_SCROLL_MUL_DECREMENT;
					}
					if (craftingSection == CraftingSection.RECIPES)
					{
						if (dx != 0)
						{
							if (craftingRecipeScrollX != 0f || craftingRecipeScrollY != 0f || CurrentRecipeCategory.Count == 0)
							{
								return;
							}
							int count = CurrentRecipeCategory[CraftingRecipeY].RecipeList.Count;
							if (count != 1)
							{
								int num = CraftingRecipeX + dx;
								if (num < 0)
								{
									num += count;
								}
								else if (num >= count)
								{
									num -= count;
								}
								CraftingRecipeX = (sbyte)num;
								craftingRecipeScrollX = dx;
							}
						}
						else if (craftingRecipeScrollX == 0f && craftingRecipeScrollY == 0f)
						{
							CraftingRecipeX = 0;
							int num2 = CraftingRecipeY + dy;
							int count2 = CurrentRecipeCategory.Count;
							if (num2 < 0)
							{
								num2 += count2;
							}
							else if (num2 >= count2)
							{
								num2 -= count2;
							}
							else
							{
								craftingRecipeScrollY = dy;
							}
							CraftingRecipeY = (sbyte)num2;
						}
					}
					else
					{
						int num3 = craftingIngredientX;
						num3 += dx;
						if (num3 < 0)
						{
							num3 += CRAFTING_INGREDIENT_COLS;
						}
						else if (num3 >= CRAFTING_INGREDIENT_COLS)
						{
							num3 -= CRAFTING_INGREDIENT_COLS;
						}
						craftingIngredientX = (sbyte)num3;
						int num4 = craftingIngredientY;
						num4 += dy;
						if (num4 < 0)
						{
							num4 += CRAFTING_INGREDIENT_ROWS;
						}
						else if (num4 >= CRAFTING_INGREDIENT_ROWS)
						{
							num4 -= CRAFTING_INGREDIENT_ROWS;
						}
						craftingIngredientY = (sbyte)num4;
					}
				}
				else
				{
					craftingRecipeScrollMul = CRAFTING_DEFAULT_SCROLL_MUL;
				}
			}
		}

		public void PositionMapScreenCursor(int dx, int dy)
		{
			if (IsButtonTriggered(Buttons.A))
			{
				Main.PlaySound(10);
				if (mapScreenCursorX == 0)
				{
					if (mapScreenCursorY < 2)
					{
						pvpCooldown = cooldownLen;
						pvpSelected = !pvpSelected;
					}
					else if (CanViewGamerCard() && Netplay.Session != null)
					{
						GamerCollection<NetworkGamer> allGamers = Netplay.Session.AllGamers;
						int num = mapScreenCursorY - 2;
						if (num < ((ReadOnlyCollection<NetworkGamer>)(object)allGamers).Count)
						{
							NetworkGamer gamer = ((ReadOnlyCollection<NetworkGamer>)(object)allGamers)[num];
							ShowGamerCard(gamer);
						}
					}
				}
				else
				{
					teamCooldown = cooldownLen;
					if (mapScreenCursorY == 0)
					{
						teamSelected = (byte)(mapScreenCursorX - 1);
					}
					else
					{
						teamSelected = (byte)((mapScreenCursorX != 1) ? (mapScreenCursorX + 1) : 0);
					}
				}
				return;
			}
			if (IsButtonTriggered(Buttons.X) && CanCommunicate())
			{
				if (Main.NetMode <= 0 || Netplay.gamer == null || Guide.IsVisible)
				{
					return;
				}
				Main.PlaySound(10);
				bool flag;
				do
				{
					flag = false;
					try
					{
						Guide.ShowGameInvite(controller, null);
					}
					catch (GuideAlreadyVisibleException)
					{
						Thread.Sleep(32);
						flag = true;
					}
				}
				while (flag);
				return;
			}
			if (IsButtonTriggered(Buttons.Y))
			{
				if (SignedInGamer.PartySize > 1 && Main.NetMode > 0 && Netplay.gamer != null && localGamer != null)
				{
					localGamer.SendPartyInvites();
				}
				return;
			}
			if (dy != 0)
			{
				do
				{
					mapScreenCursorY += dy;
				}
				while (mapScreenCursorX == 0 && mapScreenCursorY == 1);
				dy = ((mapScreenCursorX > 0) ? 2 : 10);
				if (mapScreenCursorY < 0)
				{
					mapScreenCursorY += dy;
				}
				else if (mapScreenCursorY >= dy)
				{
					mapScreenCursorY -= dy;
				}
			}
			if (mapScreenCursorY < 2)
			{
				mapScreenCursorX += dx;
				if (mapScreenCursorX < 0)
				{
					mapScreenCursorX += 4;
				}
				else if (mapScreenCursorX >= 4)
				{
					mapScreenCursorX -= 4;
				}
			}
		}

		public void FoundPotentialArmor(int itemType)
		{
			if (TriggerCheckEnabled(Trigger.CollectedAllArmor))
			{
				armorFound.Set(itemType, value: true);

#if !USE_ORIGINAL_CODE
				bool AllArmorsFound = true; // Revised how this is structured compared to the original code. The original was a fucking mess.
				foreach (Item.ID ArmorID in Item.ArmorIDs)
				{
					if (!armorFound.Get((int)ArmorID))
					{
						AllArmorsFound = false;
						break; // No need to continue checking if one armor is not yet found.
					}
				}
				if (AllArmorsFound)
				{
					SetTriggerState(Trigger.CollectedAllArmor);
				}
#else
				// See? A mess.
				if (armorFound.Get(604) && armorFound.Get(607) && armorFound.Get(610) && armorFound.Get(605) && armorFound.Get(608) && armorFound.Get(611) && armorFound.Get(606) && armorFound.Get(609) && armorFound.Get(612) && armorFound.Get(558) && armorFound.Get(559) && armorFound.Get(553) && armorFound.Get(551) && armorFound.Get(552) && armorFound.Get(400) && armorFound.Get(402) && armorFound.Get(401) && armorFound.Get(403) && armorFound.Get(404) && armorFound.Get(376) && armorFound.Get(377) && armorFound.Get(378) && armorFound.Get(379) && armorFound.Get(380) && armorFound.Get(371) && armorFound.Get(372) && armorFound.Get(373) && armorFound.Get(374) && armorFound.Get(375) && armorFound.Get(231) && armorFound.Get(232) && armorFound.Get(233) && armorFound.Get(151) && armorFound.Get(152) && armorFound.Get(153) && armorFound.Get(228) && armorFound.Get(229) && armorFound.Get(230) && armorFound.Get(102) && armorFound.Get(101) && armorFound.Get(100) && armorFound.Get(123) && armorFound.Get(124) && armorFound.Get(125) && armorFound.Get(92) && armorFound.Get(83) && armorFound.Get(79) && armorFound.Get(91) && armorFound.Get(82) && armorFound.Get(78) && armorFound.Get(90) && armorFound.Get(81) && armorFound.Get(77) && armorFound.Get(89) && armorFound.Get(80) && armorFound.Get(76) && armorFound.Get(88) && armorFound.Get(410) && armorFound.Get(411))
				{
					SetTriggerState(Trigger.CollectedAllArmor);
				}
#endif
			}
		}

		private void UpdateAlternateGrappleControls()
		{
			if (alternateGrappleControls)
			{
				BTN_JUMP2 = Buttons.LeftTrigger;
				BTN_GRAPPLE = Buttons.LeftStick;
			}
			else
			{
				BTN_JUMP2 = Buttons.LeftStick;
				BTN_GRAPPLE = Buttons.LeftTrigger;
			}
		}

		public bool IsJumpButtonDown()
		{
			if (!PadState.IsButtonDown(BTN_JUMP))
			{
				return PadState.IsButtonDown(BTN_JUMP2);
			}
			return true;
		}

		public bool WasJumpButtonUp()
		{
			if (gpPrevState.IsButtonUp(BTN_JUMP))
			{
				return gpPrevState.IsButtonUp(BTN_JUMP2);
			}
			return false;
		}

		public void ShowGamerCard(Gamer gamer)
		{
			if (gamer == null || Guide.IsVisible)
			{
				return;
			}
			bool flag;
			do
			{
				flag = false;
				try
				{
					Guide.ShowGamerCard(controller, gamer);
				}
				catch (GuideAlreadyVisibleException)
				{
					Thread.Sleep(32);
					flag = true;
				}
				catch (ObjectDisposedException)
				{
					gamer = Gamer.GetFromGamertag(gamer.Gamertag);
					flag = true;
				}
				catch (Exception)
				{
				}
			}
			while (flag);
		}

		public void SignOut()
		{
			if (SignedInGamer == null)
			{
				return;
			}
			SignedInGamer signedInGamer = SignedInGamer;
			SignedInGamer = null;
			foreach (SignedInGamer signedInGamer4 in Gamer.SignedInGamers)
			{
				if (signedInGamer4.PlayerIndex == controller && signedInGamer4.Gamertag == signedInGamer.Gamertag)
				{
					SignedInGamer = signedInGamer4;
				}
			}
			if (SignedInGamer != null)
			{
				if (Main.NetMode > 0 && !HasOnline())
				{
					Error(Lang.MenuText[5], Lang.InterfaceText[36]);
					MainUI.ExitGame();
				}
				else
				{
					if (!Main.IsGameStarted)
					{
						return;
					}
					wasRemovedFromSessionWithoutOurConsent = true;
					if (this == MainUI)
					{
						for (int i = 0; i < ((ReadOnlyCollection<LocalNetworkGamer>)(object)Netplay.Session.LocalGamers).Count; i++)
						{
							SignedInGamer signedInGamer3 = ((ReadOnlyCollection<LocalNetworkGamer>)(object)Netplay.Session.LocalGamers)[i].SignedInGamer;
							Main.UIInstance[(int)signedInGamer3.PlayerIndex].wasRemovedFromSessionWithoutOurConsent = true;
						}
					}
				}
				return;
			}
			wasRemovedFromSessionWithoutOurConsent = false;
			MessageBox.RemoveMessagesFor(controller);
			CancelInvite(signedInGamer);
			blacklist.Clear();
			if (this == MainUI)
			{
				Netplay.StopFindingSessions();
				if (Main.WorldGenThread != null)
				{
					Main.WorldGenThread.Abort();
					Main.WorldGenThread = null;
					WorldGen.Gen = false;
				}
				if (playerStorage != null)
				{
					((Collection<IGameComponent>)(object)theGame.Components).Remove(playerStorage);
					playerStorage.Dispose();
					playerStorage = null;
				}
				if (NumActiveViews == 1 || Main.IsGameStarted)
				{
					if (Main.IsGameStarted)
					{
						ExitGame();
					}
					SetMenu(MenuMode.WELCOME, rememberPrevious: false, reset: true);
					return;
				}
			}
			Exit();
		}

		public bool TestStorageSpace(string container, string destinationPath, int writeSize)
		{
			using (StorageContainer storageContainer = OpenPlayerStorage(container))
			{
				long num = storageContainer.StorageDevice.FreeSpace;
				if (num >= writeSize)
				{
					return true;
				}
				if (storageContainer.FileExists(destinationPath))
				{
					using (Stream stream = storageContainer.OpenFile(destinationPath, FileMode.Open, FileAccess.Read))
					{
						num += stream.Length;
					}
					if (num >= writeSize)
					{
						storageContainer.DeleteFile(destinationPath);
						return true;
					}
				}
			}
			MessageBox.Show(controller, Lang.MenuText[5], Lang.InterfaceText[70], new string[1]
			{
				Lang.MenuText[90]
			});
			return false;
		}

		public void WriteError()
		{
			MessageBox.Show(controller, Lang.MenuText[5], Lang.WorldGenText[54], new string[1]
			{
				Lang.MenuText[90]
			});
		}

		public void ReadError()
		{
			MessageBox.Show(controller, Lang.MenuText[9], Lang.WorldGenText[53], new string[1]
			{
				Lang.MenuText[90]
			});
		}

		public void CheckHDTV()
		{
			if (!Main.IsHDTV)
			{
				Main.IsHDTV = true;
				MessageBox.Show(controller, Lang.MenuText[3], Lang.InterfaceText[71], new string[1]
				{
					Lang.MenuText[90]
				});
			}
		}

		public bool CheckBlacklist()
		{
			ulong worldId = Main.GetWorldId();
			for (int num = blacklist.Count - 1; num >= 0; num--)
			{
				if (blacklist[num] == worldId)
				{
					if (CurMenuType != 0)
					{
						CurMenuType = MenuType.PAUSE;
					}
					string[] array = new string[2];
					if (CurMenuType == MenuType.MAIN)
					{
						array[0] = Lang.MenuText[15];
					}
					else
					{
						array[0] = Lang.MenuText[100];
					}
					array[1] = Lang.InterfaceText[75];
					MessageBox.Show(controller, Lang.MenuText[3], Lang.InterfaceText[74], array, ShouldAutoUpdate: false);
					SetMenu(MenuMode.BLACKLIST_REMOVE);
					return true;
				}
			}
			return false;
		}

		public void CheckUserGeneratedContent()
		{
			if (!IsUserGeneratedContentAllowed())
			{
				CurMenuType = MenuType.PAUSE;
				MessageBox.Show(BoxOptions: (!autoSave && IsStorageEnabledForAnyPlayer()) ? new string[2]
				{
					Lang.InterfaceText[2],
					Lang.InterfaceText[1]
				} : new string[1]
				{
					Lang.MenuText[15]
				}, Controller: controller, BoxCaption: Lang.MenuText[3], BoxContents: Lang.InterfaceText[79], ShouldAutoUpdate: false);
				SetMenu(MenuMode.EXIT_UGC_BLOCKED);
			}
		}
	}
}
