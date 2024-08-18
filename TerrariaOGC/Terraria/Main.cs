using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Terraria.Achievements;

#if !USE_ORIGINAL_CODE
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Microsoft.Win32;
using System.Diagnostics;
#endif

namespace Terraria
{
	public sealed class Main : Game
	{
		public enum Music : byte
		{
			OVERWORLD_DAY,
			EERIE,
			OVERWORLD_NIGHT,
			UNDERGROUND,
			BOSS1,
			TITLE,
			JUNGLE,
			CORRUPTION,
			THE_HALLOW,
			UNDERGROUND_CORRUPTION,
			UNDERGROUND_HALLOW,
			BOSS2,
			BOSS3,
			DESERT,
			FLOATING_ISLAND,
			TUTORIAL,
			BOSS4,
			OCEAN,
			SNOW,
#if VERSION_103 || VERSION_FINAL
			PLANTERA,
			BOSS5,
			CRIMSON,
			RAIN,
			ALTERNATE_DAY,
			DUNGEON,
			ECLIPSE,
			PUMPKIN_MOON,
			ICE,
			LIHZAHRD,
			RAIN_SFX,
			ALTERNATE_UNDERGROUND,
			MUSHROOMS,
#if VERSION_FINAL
			FROST_MOON,
			UNDERGROUND_CRIMSON,
#endif
#endif
			NUM_SONGS,
			NONE = NUM_SONGS
		}

#if USE_ORIGINAL_CODE
		public const int ResolutionWidth = 960;

		public const int ResolutionHeight = 540;
#else
        public static int ResolutionWidth;

        public static int ResolutionHeight;
#endif

		public const int SmallWorldW = 4200;

		public const int SmallWorldH = 1200;

		public const int LargeWorldW = 5040;

		public const int LargeWorldH = 1440;

		public const int MediumWorldW = 4620;

		public const int MediumWorldH = 1320;

		public const bool NET_VERBOSE = false; // This variable is either unused or cannot be located. Presumed to be related to the extension of net messages.

		public const int MaxNumChests = 1000;

		public const int MaxNumItems = 200;

		public const int MaxNumItemSounds = 37;

		public const int MaxNumNPCHitSounds = 11;

		public const int MaxNumNPCKillSounds = 15;

		public const int MaxNumLiquidTypes = 2;

		public const int MaxNumMusic = (int)Music.NUM_SONGS;

		public const int OldWorldDataVersion = 46;

#if (IS_PATCHED)
		public const int PlayerDataVersion = 6;

		public const int SettingsDataVersion = 5;

		public const int NewWorldDataVersion = 49;

		public const int NetworkVersion = 2; // Not bothering with network version or related code, Network is inoperable currently anyway, so we can worry about this later.

		public const string VersionNumber = "Xbox360 v0.7.8"; // Initially unused, but now used for a version string in UI.cs; Discarding the v0.7.x since initial version still reported 1.0
#elif (VERSION_INITIAL)
		public const int PlayerDataVersion = 6;

		public const int SettingsDataVersion = 5;

		public const int NewWorldDataVersion = 49;

		public const int NETWORK_VERSION = 1;

		public const string VersionNumber = "Xbox360 v0.7.6";
#elif (VERSION_101)
		public const int PlayerDataVersion = 8;

		public const int SettingsDataVersion = 6;

		public const int NewWorldDataVersion = 50;

		public const int NETWORK_VERSION = 3;

		public const string VersionNumber = "Xbox360 v1.01";
#elif (VERSION_103)
		public const int PlayerDataVersion = 12;

		public const int NETWORK_VERSION = 3;

		public const string VersionNumber = "Xbox360 v1.03";
#elif (VERSION_FINAL)
		public const int PlayerDataVersion = 21;

		public const int NETWORK_VERSION = 3;

		public const string VersionNumber = "Xbox360 v1.09";
#endif

		public const int WorldRate = 1;

		public const bool IgnoreErrors = false;

		public const int ZoneX = 99;

		public const int ZoneY = 87;

		public const bool ShowFrameRate = false; // Initially unused, but now a new variable has been created for the same purpose with the game settings.

		public const int LeftWorld = 0;

		public const int TopWorld = 0;

		public const int SectionWidth = 40;

		public const int SectionHeight = 30;

#if VERSION_101
		public const int MaxNumTilesets = 151; // Just +1 for campfires.
		public const int MaxNumWallTypes = 38;
#else
		public const int MaxNumTilesets = 150;
		public const int MaxNumWallTypes = 32;
#endif

		public const int MaxNumTilenames = 135; // Highest crafting station ID

		public const int MaxNumCombatText = 32;

		public const int ChatLength = 600;

		public const int MaxNumChatLines = 7;

		private const int MaxNumItemUpdates = 5;

		public const int SaveIconSprite = (int)_sheetSprites.ID.ITEM_191;

		public const int SaveIconMessageTime = 480;

		private const int SaveIconMinTime = 180;

		private const int NumSplashLogos = 4;

		private const int SplashFadeIn = 16;

		private const int SplashFadeOut = 16;

		private const int SplashDelayBase = 120;

		private const int SplashDelayRating = 240;

		private const int UpsellNumScreens = 1; // Could there have been more than 1?

		private const int UpsellDelay = 600; // This does not appear to have a use in the code.

        private static readonly string[] MusicCueNames = new string[MaxNumMusic]
		{
			"Music_1",
			"Music_2",
			"Music_3",
			"Music_4",
			"Music_5",
			"Music_6",
			"Music_7",
			"Music_8",
			"Music_9",
			"Music_10",
			"Music_11",
			"Music_12",
			"Music_13",
			"Desert",
			"FloatingIsland",
			"Tutorial",
			"Boss4",
			"Ocean",
			"Snow",
#if VERSION_103 || VERSION_FINAL
			"boss_plant",
			"boss5",
			"crimson",
			"day_rainy",
			"day2",
			"dungeon",
			"eclipse",
			"halloween",
			"ice",
			"lihzahrd",
			"rain",
			"underground2",
			"shroom",
#if VERSION_FINAL
			"frostmoon",
			"Music_33",
#endif
#endif
		};

		private static readonly Music[] MusicBoxToSong = new Music[MaxNumMusic]
		{
			Music.OVERWORLD_DAY,
			Music.EERIE,
			Music.OVERWORLD_NIGHT,
			Music.TITLE,
			Music.UNDERGROUND,
			Music.BOSS1,
			Music.JUNGLE,
			Music.CORRUPTION,
			Music.UNDERGROUND_CORRUPTION,
			Music.THE_HALLOW,
			Music.BOSS2,
			Music.UNDERGROUND_HALLOW,
			Music.BOSS3,
			Music.DESERT,
			Music.FLOATING_ISLAND,
			Music.TUTORIAL,
			Music.BOSS4,
			Music.OCEAN,
			Music.SNOW,
#if VERSION_103 || VERSION_FINAL
			Music.PLANTERA,
			Music.BOSS5,
			Music.CRIMSON,
			Music.RAIN,
			Music.ALTERNATE_DAY,
			Music.DUNGEON,
			Music.ECLIPSE,
			Music.PUMPKIN_MOON,
			Music.ICE,
			Music.LIHZAHRD,
			Music.RAIN_SFX,
			Music.ALTERNATE_UNDERGROUND,
			Music.MUSHROOMS,
#if VERSION_FINAL
			Music.FROST_MOON,
			Music.UNDERGROUND_CRIMSON
#endif
#endif
		};

		public static readonly Item.ID[] SongToMusicBox = new Item.ID[MaxNumMusic]
		{
			Item.ID.MUSIC_BOX_OVERWORLD_DAY,
			Item.ID.MUSIC_BOX_EERIE,
			Item.ID.MUSIC_BOX_NIGHT,
			Item.ID.MUSIC_BOX_UNDERGROUND,
			Item.ID.MUSIC_BOX_BOSS1,
			Item.ID.MUSIC_BOX_TITLE,
			Item.ID.MUSIC_BOX_JUNGLE,
			Item.ID.MUSIC_BOX_CORRUPTION,
			Item.ID.MUSIC_BOX_THE_HALLOW,
			Item.ID.MUSIC_BOX_UNDERGROUND_CORRUPTION,
			Item.ID.MUSIC_BOX_UNDERGROUND_HALLOW,
			Item.ID.MUSIC_BOX_BOSS2,
			Item.ID.MUSIC_BOX_BOSS3,
			Item.ID.MUSIC_BOX_DESERT,
			Item.ID.MUSIC_BOX_SPACE,
			Item.ID.MUSIC_BOX_TUTORIAL,
			Item.ID.MUSIC_BOX_BOSS4,
			Item.ID.MUSIC_BOX_OCEAN,
			Item.ID.MUSIC_BOX_SNOW,
#if VERSION_103 || VERSION_FINAL
			Item.ID.MUSIC_BOX_PLANTERA,
			Item.ID.MUSIC_BOX_BOSS5,
			Item.ID.MUSIC_BOX_CRIMSON,
			Item.ID.MUSIC_BOX_RAIN,
			Item.ID.MUSIC_BOX_ALTERNATE_DAY,
			Item.ID.MUSIC_BOX_DUNGEON,
			Item.ID.MUSIC_BOX_ECLIPSE,
			Item.ID.MUSIC_BOX_PUMPKIN_MOON,
			Item.ID.MUSIC_BOX_ICE,
			Item.ID.MUSIC_BOX_LIHZAHRD,
			Item.ID.MUSIC_BOX_RAIN, // This one won't actually be used since it is the rain sfx.
			Item.ID.MUSIC_BOX_ALTERNATE_UNDERGROUND,
			Item.ID.MUSIC_BOX_MUSHROOMS,
#if VERSION_FINAL
			Item.ID.MUSIC_BOX_FROST_MOON,
			Item.ID.MUSIC_BOX_UNDERGROUND_CRIMSON
#endif
#endif
		};

		public static UI[] UIInstance = new UI[4];

		public static AchievementSystem AchievementSystem = new AchievementSystem();

		public static Thread WorldGenThread;

		public static int MusicBox = -1;

		public static float MusicVolume = 0.75f;

		public static float SoundVolume = 1f;

		private Thread ContentThread;

		public static float HarpNote = 0f;

		public static bool[] ProjHostile = new bool[Projectile.MaxNumProjTypes];

		public static Recipe[] ActiveRecipe = new Recipe[Recipe.MaxNumRecipes];

		public static Chest[] Shop = new Chest[NPC.MaxNumTownNPCs];

		public static int RenderCount = 0;

		private static GraphicsDeviceManager GraphicsGame;

		public static SpriteBatch SpriteBatch;

		public static StringBuilder StrBuilder = new StringBuilder(4096, 4096);

		public static bool IsGameStarted = false;

		public static bool IsGamePaused = false;

		public static bool InHardMode = false;

		public static int DiscoStyle = 0;

		public static Vector3 DiscoRGB = new Vector3(1f, 0f, 0f);

		public static uint FrameCounter = 0u;

		public static int MagmaBGFrame = 0;

		public static int RightWorld = 80640;

		public static int BottomWorld = 23040;

		public static short MaxTilesX = (short)(RightWorld / 16);

		public static short MaxTilesY = (short)(BottomWorld / 16);

		public static int MaxSectionsX = MaxTilesX / 40;

		public static int MaxSectionsY = MaxTilesY / 30;

		public static string[] TileNames = new string[MaxNumTilenames];

		public static short DungeonX;

		public static short DungeonY;

		public static Liquid[] Liquid = new Liquid[Terraria.Liquid.MaxNumLiquids];

		public static LiquidBuffer[] LiquidBuffer = new LiquidBuffer[Terraria.LiquidBuffer.MaxNumLiquidBuffer];

		public static Music CurrentMusic = Music.NUM_SONGS;

		public static Music NewMusic = Music.NUM_SONGS;

		public static string WorldName;

		public static int WorldID;

		public static int WorldTimestamp;

		public static bool CheckWorldId = false;

		public static bool CheckUserGeneratedContent = false;

		public static int WorldSurface = 360;

		public static int WorldSurfacePixels = WorldSurface << 4;

		public static int RockLayer;

		public static int RockLayerPixels;

		public static int MagmaLayer;

		public static int MagmaLayerPixels;

		public static Color[] TeamColors = new Color[5];

		public static Time GameTime = default;

		public static Time MenuTime = default;

		public static float DemonTorch = 1f;

		public static float DemonTorchDir = -0.01f;

		public static FastRandom Rand = new FastRandom();

		public static Texture2D WhiteTexture;

		public static SfxInstancePool SoundMech;

		public static SfxInstancePool[] SoundDig = new SfxInstancePool[3];

		public static SfxInstancePool[] SoundTink = new SfxInstancePool[3];

		public static SfxInstancePool[] SoundPlayerHit = new SfxInstancePool[3];

		public static SfxInstancePool[] SoundFemaleHit = new SfxInstancePool[3];

		public static SfxInstancePool SoundPlayerKilled;

		public static SfxInstancePool SoundGrass;

		public static SfxInstancePool SoundGrab;

		public static SfxInstancePool SoundPixie;

		public static SfxInstancePool[] SoundItem = new SfxInstancePool[MaxNumItemSounds];

		public static SfxInstancePool[] SoundNPCHit = new SfxInstancePool[MaxNumNPCHitSounds];

		public static SfxInstancePool[] SoundNPCKilled = new SfxInstancePool[MaxNumNPCKillSounds];

		public static SfxInstancePool SoundDoorOpen;

		public static SfxInstancePool SoundDoorClosed;

		public static SfxInstancePool SoundMenuOpen;

		public static SfxInstancePool SoundMenuClose;

		public static SfxInstancePool SoundMenuTick;

		public static SfxInstancePool SoundShatter;

		public static SfxInstancePool[] SoundZombie = new SfxInstancePool[5];

		public static SfxInstancePool[] SoundRoar = new SfxInstancePool[2];

		public static SfxInstancePool[] SoundSplash = new SfxInstancePool[2];

		public static SfxInstancePool SoundDoubleJump;

		public static SfxInstancePool SoundRun;

		public static SfxInstancePool SoundCoins;

		public static SfxInstancePool SoundUnlock;

		public static SfxInstancePool SoundChat;

		public static SfxInstancePool SoundMaxMana;

		public static SfxInstancePool SoundDrown;

		public static AudioEngine UsedAudioEngine;

		public static SoundBank UsedSoundBank;

		public static WaveBank UsedWaveBank;

		public static Cue[] MusicCues = new Cue[MaxNumMusic];

		public static float[] MusicFade = new float[MaxNumMusic];

		public static bool[] TileLighted = new bool[MaxNumTilesets];

		public static bool[] TileMergeDirt = new bool[MaxNumTilesets];

		public static bool[] TileCut = new bool[MaxNumTilesets];

		public static short[] TileShine = new short[MaxNumTilesets];

		public static bool[] TileShine2 = new bool[MaxNumTilesets];

		public static bool[] WallHouse = new bool[MaxNumWallTypes];

		public static bool[] TileStone = new bool[MaxNumTilesets];

		public static bool[] TileAxe = new bool[MaxNumTilesets];

		public static bool[] TileHammer = new bool[MaxNumTilesets];

		public static bool[] TileWaterDeath = new bool[MaxNumTilesets];

		public static bool[] TileLavaDeath = new bool[MaxNumTilesets];

		public static bool[] TileTable = new bool[MaxNumTilesets];

		public static bool[] TileBlockLight = new bool[MaxNumTilesets];

		public static bool[] TileNoSunLight = new bool[MaxNumTilesets];

		public static bool[] TileDungeon = new bool[MaxNumTilesets];

		public static bool[] TileSolidTop = new bool[MaxNumTilesets];

		public static bool[] TileSolid = new bool[MaxNumTilesets];

		public static bool[] TileSolidNotSolidTop = new bool[MaxNumTilesets];

		public static bool[] TileSolidAndAttach = new bool[MaxNumTilesets];

		public static bool[] TileNoAttach = new bool[MaxNumTilesets];

		public static bool[] TileNoFail = new bool[MaxNumTilesets];

		public static bool[] TileFrameImportant = new bool[MaxNumTilesets];

		public static DustPool DustSet = new DustPool(null, Dust.MaxNumGlobalDust);

		public static Tile[,] TileSet = new Tile[LargeWorldW, LargeWorldH];

		public static Item[] ItemSet = new Item[MaxNumItems + 1];

		public static NPC[] NPCSet = new NPC[NPC.MaxNumNPCs + 1];

		public static Gore[] GoreSet = new Gore[Gore.MaxNumGore];

		public static Projectile[] ProjectileSet = new Projectile[Projectile.MaxNumProjs];

		public static CombatText[] CombatTextSet = new CombatText[MaxNumCombatText];

		public static Chest[] ChestSet = new Chest[MaxNumChests];

		public static Sign[] SignSet = new Sign[Sign.MaxNumSigns];

		public static ChatLine[] ChatLineSet = new ChatLine[MaxNumChatLines];

		public static Player[] PlayerSet = new Player[Player.MaxNumPlayers + 1];

		public static short SpawnTileX;

		public static short SpawnTileY;

		public static bool HasFocus = true;

		public static int InvasionType = 0;

		public static float InvasionX = 0f;

		public static int InvasionSize = 0;

		public static int InvasionDelay = 0;

		public static int InvasionWarn = 0;

		public static int NetMode = (byte)NetModeSetting.LOCAL;

		public static int NetPlayCounter;

		public static int LastItemUpdate; // Unused

		private static int SaveIconCounter = 0;

		private static int ActiveSaves = 0;

		public static bool SaveOnExit;

		private static readonly Texture2D[] SplashTextures = new Texture2D[NumSplashLogos];

		private static short ShowSplash = 0;

		private short SplashDelay = SplashDelayRating;

		private short SplashCounter;

		private short SplashLogo;

		private bool HasUpsellLoaded;

		public static bool IsRunningSlowly;

		public static bool IsTrial = true;

		public static bool IsHDTV;

		public static Tutorial TutorialState = Tutorial.NUM_TUTORIALS;

		public static bool TutorialMaskLS;

		public static bool TutorialMaskRS;

		public static bool TutorialMaskRSpress;

		public static bool TutorialMaskA;

		public static bool TutorialMaskB;

		public static bool TutorialMaskX;

		public static bool TutorialMaskY;

		public static bool TutorialMaskLB;

		public static bool TutorialMaskRB;

		public static bool TutorialMaskLT;

		public static bool TutorialMaskRT;

		public static bool TutorialMaskBack;

		public static CompiledText TutorialText = null;

		private static int TutorialInputDelay;

		private static uint TutorialVar;

		private static uint TutorialVar2;

		private static Location TutorialHouse;

#if !USE_ORIGINAL_CODE
		public const string SettingsFile = "Settings.ini";
		public static string Gamertag;
		public static bool PSMode;
		public static bool UnlockAllRecipes;
		public static bool ShowFPS;
		public static bool AlwaysOnWatch;
		public static bool AlwaysOnDepth;
		public static bool AlwaysOnCompass;
		public static int ScreenHeightPtr;
		public static float ScreenMultiplier;
		public static bool HardmodeAlert;
#if VERSION_101
		public static float LiquidFrCounter;
		public static float LiquidFrame = 0;
		public static byte HeartCrystalFrCounter;
		public static byte HeartCrystalFrame = 0;
		public static byte ShadowOrbFrCounter;
		public static byte ShadowOrbFrame = 0;
		public static byte FurnaceFrCounter;
		public static byte FurnaceFrame = 0;
		public static byte CampfireFrCounter;
		public static byte CampfireFrame = 0;
#endif
		public static bool CollectorsEditionPC = false;
		private readonly bool StartFullscreen;
		private float ZoomLevel;
		public static Stopwatch FPSTimer = new Stopwatch();
		public static short FPS, FPSCount;
		KeyboardState CurrentKeyState;
		KeyboardState PrevKeyState;

		public static Stopwatch RunTimer = new Stopwatch();

		public void CheckKeys()
		{
			PrevKeyState = CurrentKeyState;
			CurrentKeyState = Keyboard.GetState();
		}

		public bool HasKeyBeenPressed(Keys key)
		{
			return CurrentKeyState.IsKeyDown(key) && !PrevKeyState.IsKeyDown(key);
		}

		private void SpecialCheck()
		{
			try
			{
				RegistryKey CurrentUser = Registry.CurrentUser;
				CurrentUser = CurrentUser.CreateSubKey("Software\\Terraria");
				if (CurrentUser != null && CurrentUser.GetValue("Bunny") != null && CurrentUser.GetValue("Bunny").ToString() == "1")
				{
					CollectorsEditionPC = true; // If Bunny, give Guinea Pig
				}
			}
			catch
			{
				CollectorsEditionPC = false;
			}
		}
#endif

		public static ulong GetWorldId()
		{
			return (ulong)(((long)WorldID << 32) | (uint)WorldTimestamp);
		}

		public Main()
		{
			GraphicsGame = new GraphicsDeviceManager(this);
			GraphicsGame.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true;
			Content.RootDirectory = "Content";

#if !USE_ORIGINAL_CODE
			Settings ExistingSettings = new Settings(SettingsFile);

            if (!ExistingSettings.AlreadyExists("Game", "Gamertag"))
            {
                ExistingSettings.Set("Game", "Gamertag", "Terrarian", "Sets the username of the player, since this is typically given by the console.");
            }
            if (!ExistingSettings.AlreadyExists("Game", "TrialActive"))
			{
                ExistingSettings.Set("Game", "TrialActive", "True", "Determines if trial mode is active; Included for archival/legal purposes, as this is not the full version.");
            }
			if (!ExistingSettings.AlreadyExists("Game", "PSMode"))
			{
				ExistingSettings.Set("Game", "PSMode", "False", "Determines whether the game will run in 'PlayStation 3' mode, rather than 'Xbox 360' mode.\n; This will change the font, controller, D-Pad, and button sprites used in game.");
			}
			if (!ExistingSettings.AlreadyExists("Game", "UnlockAllRecipes"))
			{
				ExistingSettings.Set("Game", "UnlockAllRecipes", "False");
			}

			if (!ExistingSettings.AlreadyExists("Display", "ScreenHeight"))
            {
                ExistingSettings.Set("Display", "ScreenHeight", "0", "This setting will determine the rendering height in pixels, allowing for upscaling from the original 540p.\n; There are 3 options: (0) 540p, (1) 720p, or (2) 1080p.");
            }
			if (!ExistingSettings.AlreadyExists("Display", "ShowFPS"))
			{
				ExistingSettings.Set("Display", "ShowFPS", "False");
			}
			if (!ExistingSettings.AlreadyExists("Display", "StartFullscreen"))
			{
				ExistingSettings.Set("Display", "StartFullscreen", "False", "Determines if the game is started in full-screen mode; This will not upscale, but stretch the current resolution to the display's max.\n; You can also toggle full-screen mode in-game by pressing 'F'.");
			}
			if (!ExistingSettings.AlreadyExists("Display", "ZoomLevel"))
			{
				ExistingSettings.Set("Display", "ZoomLevel", "0", "Sets how zoomed in the camera will be during initial gameplay; This must be a floating-point number between 0 and 1.\n; You can adjust this in-game by pressing '+' or '-'.");
			}
			if (!ExistingSettings.AlreadyExists("Display", "AlwaysOnWatch"))
			{
				ExistingSettings.Set("Display", "AlwaysOnWatch", "False", "Determines whether the information of the 3rd-tier Watch accessory will always active in-game.");
			}
			if (!ExistingSettings.AlreadyExists("Display", "AlwaysOnDepth"))
			{
				ExistingSettings.Set("Display", "AlwaysOnDepth", "False", "Determines whether the information of the Depth Meter accessory will always active in-game.");
			}
			if (!ExistingSettings.AlreadyExists("Display", "AlwaysOnCompass"))
			{
				ExistingSettings.Set("Display", "AlwaysOnCompass", "False", "Determines whether the information of the Compass accessory will always active in-game.");
			}

			if (!ExistingSettings.AlreadyExists("Additions", "HardmodeAlert"))
			{
				ExistingSettings.Set("Additions", "HardmodeAlert", "False", "Implements a pop-up alert once hardmode is activated within a world à la Pre-1.3 Terraria Mobile.\n; This basically just shows an introductory message about what hardmode is and what you can do from there.");
			}

			Gamertag = ExistingSettings.Get("Game", "Gamertag");

            IsTrial = ExistingSettings.Get("Game", "TrialActive").ToLower().Equals("true");

			PSMode = ExistingSettings.Get("Game", "PSMode").ToLower().Equals("true");

			UnlockAllRecipes = ExistingSettings.Get("Game", "UnlockAllRecipes").ToLower().Equals("true");

			ScreenHeightPtr = Convert.ToInt32(ExistingSettings.Get("Display", "ScreenHeight"));
			ScreenMultiplier = (float)((1f / 6f) * Math.Pow(ScreenHeightPtr, 2)) + ((1f / 6f) * ScreenHeightPtr) + 1;
			if (ScreenHeightPtr < 1 || ScreenHeightPtr > 2)
			{
				ResolutionHeight = 540;
			}
			else
			{
				ResolutionHeight = (int)(540 * ScreenMultiplier);
			}
			ResolutionWidth = ResolutionHeight * 16 / 9;

			ShowFPS = ExistingSettings.Get("Display", "ShowFPS").ToLower().Equals("true");

			StartFullscreen = ExistingSettings.Get("Display", "StartFullscreen").ToLower().Equals("true");
			
			ZoomLevel = Convert.ToSingle(ExistingSettings.Get("Display", "ZoomLevel"));
			if (ZoomLevel < 0 || ZoomLevel > 1)
			{
				ZoomLevel = 0;
			}
			ZoomLevel += 1;

			AlwaysOnWatch = ExistingSettings.Get("Display", "AlwaysOnWatch").ToLower().Equals("true");
			AlwaysOnDepth = ExistingSettings.Get("Display", "AlwaysOnDepth").ToLower().Equals("true");
			AlwaysOnCompass = ExistingSettings.Get("Display", "AlwaysOnCompass").ToLower().Equals("true");

			HardmodeAlert = ExistingSettings.Get("Additions", "HardmodeAlert").ToLower().Equals("true");

			ExistingSettings.Close();
#endif
        }

        protected override void Initialize()
		{
			base.Initialize();
			MenuTime.reset(86.4f);
			GameTime.reset(1f);
			NPC.ClearNames();
			NPC.SetNames();
			TileShine2[6] = true;
			TileShine2[7] = true;
			TileShine2[8] = true;
			TileShine2[9] = true;
			TileShine2[12] = true;
			TileShine2[21] = true;
			TileShine2[22] = true;
			TileShine2[25] = true;
			TileShine2[45] = true;
			TileShine2[46] = true;
			TileShine2[47] = true;
			TileShine2[63] = true;
			TileShine2[64] = true;
			TileShine2[65] = true;
			TileShine2[66] = true;
			TileShine2[67] = true;
			TileShine2[68] = true;
			TileShine2[107] = true;
			TileShine2[108] = true;
			TileShine2[111] = true;
			TileShine2[117] = true;
			TileShine2[121] = true;
			TileShine2[122] = true;
			TileShine[6] = 1150;
			TileShine[7] = 1100;
			TileShine[8] = 1000;
			TileShine[9] = 1050;
			TileShine[12] = 1000;
			TileShine[21] = 1200;
			TileShine[22] = 1150;
			TileShine[45] = 1900;
			TileShine[46] = 2000;
			TileShine[47] = 2100;
			TileShine[63] = 900;
			TileShine[64] = 900;
			TileShine[65] = 900;
			TileShine[66] = 900;
			TileShine[67] = 900;
			TileShine[68] = 900;
			TileShine[107] = 950;
			TileShine[108] = 900;
			TileShine[109] = 9000;
			TileShine[110] = 9000;
			TileShine[111] = 850;
			TileShine[116] = 9000;
			TileShine[117] = 9000;
			TileShine[118] = 8000;
			TileShine[121] = 1850;
			TileShine[122] = 1800;
			TileShine[125] = 600;
			TileShine[129] = 300;
			TileHammer[141] = true;
			TileHammer[4] = true;
			TileHammer[10] = true;
			TileHammer[11] = true;
			TileHammer[12] = true;
			TileHammer[13] = true;
			TileHammer[14] = true;
			TileHammer[15] = true;
			TileHammer[16] = true;
			TileHammer[17] = true;
			TileHammer[18] = true;
			TileHammer[19] = true;
			TileHammer[21] = true;
			TileHammer[26] = true;
			TileHammer[28] = true;
			TileHammer[29] = true;
			TileHammer[31] = true;
			TileHammer[33] = true;
			TileHammer[34] = true;
			TileHammer[35] = true;
			TileHammer[36] = true;
			TileHammer[42] = true;
			TileHammer[48] = true;
			TileHammer[49] = true;
			TileHammer[50] = true;
			TileHammer[54] = true;
			TileHammer[55] = true;
			TileHammer[77] = true;
			TileHammer[78] = true;
			TileHammer[79] = true;
			TileHammer[81] = true;
			TileHammer[85] = true;
			TileHammer[86] = true;
			TileHammer[87] = true;
			TileHammer[88] = true;
			TileHammer[89] = true;
			TileHammer[90] = true;
			TileHammer[91] = true;
			TileHammer[92] = true;
			TileHammer[93] = true;
			TileHammer[94] = true;
			TileHammer[95] = true;
			TileHammer[96] = true;
			TileHammer[97] = true;
			TileHammer[98] = true;
			TileHammer[99] = true;
			TileHammer[100] = true;
			TileHammer[101] = true;
			TileHammer[102] = true;
			TileHammer[103] = true;
			TileHammer[104] = true;
			TileHammer[105] = true;
			TileHammer[106] = true;
			TileHammer[114] = true;
			TileHammer[125] = true;
			TileHammer[126] = true;
			TileHammer[128] = true;
			TileHammer[129] = true;
			TileHammer[132] = true;
			TileHammer[133] = true;
			TileHammer[134] = true;
			TileHammer[135] = true;
			TileHammer[136] = true;
			TileFrameImportant[139] = true;
			TileHammer[139] = true;
			TileLighted[149] = true;
			TileFrameImportant[149] = true;
			TileHammer[149] = true;
			TileFrameImportant[142] = true;
			TileHammer[142] = true;
			TileFrameImportant[143] = true;
			TileHammer[143] = true;
			TileFrameImportant[144] = true;
			TileHammer[144] = true;
			TileStone[131] = true;
			TileFrameImportant[136] = true;
			TileFrameImportant[137] = true;
			TileFrameImportant[138] = true;
			TileBlockLight[137] = true;
			TileSolid[137] = true;
			TileBlockLight[145] = true;
			TileSolid[145] = true;
			TileMergeDirt[145] = true;
			TileBlockLight[146] = true;
			TileSolid[146] = true;
			TileMergeDirt[146] = true;
			TileBlockLight[147] = true;
			TileSolid[147] = true;
			TileMergeDirt[147] = true;
			TileBlockLight[148] = true;
			TileSolid[148] = true;
			TileMergeDirt[148] = true;
			TileBlockLight[138] = true;
			TileSolid[138] = true;
			TileBlockLight[140] = true;
			TileSolid[140] = true;
			TileAxe[5] = true;
			TileAxe[30] = true;
			TileAxe[72] = true;
			TileAxe[80] = true;
			TileAxe[124] = true;
			TileLighted[4] = true;
			TileLighted[17] = true;
			TileLighted[19] = true;
			TileLighted[22] = true;
			TileLighted[26] = true;
			TileLighted[31] = true;
			TileLighted[33] = true;
			TileLighted[34] = true;
			TileLighted[35] = true;
			TileLighted[36] = true;
			TileLighted[37] = true;
			TileLighted[42] = true;
			TileLighted[49] = true;
			TileLighted[58] = true;
			TileLighted[61] = true;
			TileLighted[70] = true;
			TileLighted[71] = true;
			TileLighted[72] = true;
			TileLighted[76] = true;
			TileLighted[77] = true;
			TileLighted[83] = true;
			TileLighted[84] = true;
			TileLighted[92] = true;
			TileLighted[93] = true;
			TileLighted[95] = true;
			TileLighted[98] = true;
			TileLighted[100] = true;
			TileLighted[109] = true;
			TileLighted[125] = true;
			TileLighted[126] = true;
			TileLighted[129] = true;
			TileLighted[133] = true;
			TileLighted[140] = true;
			TileMergeDirt[1] = true;
			TileMergeDirt[6] = true;
			TileMergeDirt[7] = true;
			TileMergeDirt[8] = true;
			TileMergeDirt[9] = true;
			TileMergeDirt[22] = true;
			TileMergeDirt[25] = true;
			TileMergeDirt[30] = true;
			TileMergeDirt[37] = true;
			TileMergeDirt[38] = true;
			TileMergeDirt[39] = true;
			TileMergeDirt[40] = true;
			TileMergeDirt[41] = true;
			TileMergeDirt[43] = true;
			TileMergeDirt[44] = true;
			TileMergeDirt[45] = true;
			TileMergeDirt[46] = true;
			TileMergeDirt[47] = true;
			TileMergeDirt[53] = true;
			TileMergeDirt[56] = true;
			TileMergeDirt[107] = true;
			TileMergeDirt[108] = true;
			TileMergeDirt[111] = true;
			TileMergeDirt[112] = true;
			TileMergeDirt[116] = true;
			TileMergeDirt[117] = true;
			TileMergeDirt[123] = true;
			TileMergeDirt[140] = true;
			TileMergeDirt[122] = true;
			TileMergeDirt[121] = true;
			TileMergeDirt[120] = true;
			TileMergeDirt[119] = true;
			TileMergeDirt[118] = true;
			TileFrameImportant[3] = true;
			TileFrameImportant[4] = true;
			TileFrameImportant[5] = true;
			TileFrameImportant[10] = true;
			TileFrameImportant[11] = true;
			TileFrameImportant[12] = true;
			TileFrameImportant[13] = true;
			TileFrameImportant[14] = true;
			TileFrameImportant[15] = true;
			TileFrameImportant[16] = true;
			TileFrameImportant[17] = true;
			TileFrameImportant[18] = true;
			TileFrameImportant[20] = true;
			TileFrameImportant[21] = true;
			TileFrameImportant[24] = true;
			TileFrameImportant[26] = true;
			TileFrameImportant[27] = true;
			TileFrameImportant[28] = true;
			TileFrameImportant[29] = true;
			TileFrameImportant[31] = true;
			TileFrameImportant[33] = true;
			TileFrameImportant[34] = true;
			TileFrameImportant[35] = true;
			TileFrameImportant[36] = true;
			TileFrameImportant[42] = true;
			TileFrameImportant[50] = true;
			TileFrameImportant[55] = true;
			TileFrameImportant[61] = true;
			TileFrameImportant[71] = true;
			TileFrameImportant[72] = true;
			TileFrameImportant[73] = true;
			TileFrameImportant[74] = true;
			TileFrameImportant[77] = true;
			TileFrameImportant[78] = true;
			TileFrameImportant[79] = true;
			TileFrameImportant[81] = true;
			TileFrameImportant[82] = true;
			TileFrameImportant[83] = true;
			TileFrameImportant[84] = true;
			TileFrameImportant[85] = true;
			TileFrameImportant[86] = true;
			TileFrameImportant[87] = true;
			TileFrameImportant[88] = true;
			TileFrameImportant[89] = true;
			TileFrameImportant[90] = true;
			TileFrameImportant[91] = true;
			TileFrameImportant[92] = true;
			TileFrameImportant[93] = true;
			TileFrameImportant[94] = true;
			TileFrameImportant[95] = true;
			TileFrameImportant[96] = true;
			TileFrameImportant[97] = true;
			TileFrameImportant[98] = true;
			TileFrameImportant[99] = true;
			TileFrameImportant[101] = true;
			TileFrameImportant[102] = true;
			TileFrameImportant[103] = true;
			TileFrameImportant[104] = true;
			TileFrameImportant[105] = true;
			TileFrameImportant[100] = true;
			TileFrameImportant[106] = true;
			TileFrameImportant[110] = true;
			TileFrameImportant[113] = true;
			TileFrameImportant[114] = true;
			TileFrameImportant[125] = true;
			TileFrameImportant[126] = true;
			TileFrameImportant[128] = true;
			TileFrameImportant[129] = true;
			TileFrameImportant[132] = true;
			TileFrameImportant[133] = true;
			TileFrameImportant[134] = true;
			TileFrameImportant[135] = true;
			TileFrameImportant[141] = true;
			TileCut[3] = true;
			TileCut[24] = true;
			TileCut[28] = true;
			TileCut[32] = true;
			TileCut[51] = true;
			TileCut[52] = true;
			TileCut[61] = true;
			TileCut[62] = true;
			TileCut[69] = true;
			TileCut[71] = true;
			TileCut[73] = true;
			TileCut[74] = true;
			TileCut[82] = true;
			TileCut[83] = true;
			TileCut[84] = true;
			TileCut[110] = true;
			TileCut[113] = true;
			TileCut[115] = true;
			TileLavaDeath[104] = true;
			TileLavaDeath[110] = true;
			TileLavaDeath[113] = true;
			TileLavaDeath[115] = true;
			TileSolid[127] = true;
			TileSolid[130] = true;
			TileBlockLight[130] = true;
			TileSolid[107] = true;
			TileBlockLight[107] = true;
			TileSolid[108] = true;
			TileBlockLight[108] = true;
			TileSolid[111] = true;
			TileBlockLight[111] = true;
			TileSolid[109] = true;
			TileBlockLight[109] = true;
			TileSolid[110] = false;
			TileNoAttach[110] = true;
			TileNoFail[110] = true;
			TileSolid[112] = true;
			TileBlockLight[112] = true;
			TileSolid[116] = true;
			TileBlockLight[116] = true;
			TileSolid[117] = true;
			TileBlockLight[117] = true;
			TileSolid[123] = true;
			TileBlockLight[123] = true;
			TileSolid[118] = true;
			TileBlockLight[118] = true;
			TileSolid[119] = true;
			TileBlockLight[119] = true;
			TileSolid[120] = true;
			TileBlockLight[120] = true;
			TileSolid[121] = true;
			TileBlockLight[121] = true;
			TileSolid[122] = true;
			TileBlockLight[122] = true;
			TileBlockLight[115] = true;
			TileSolid[0] = true;
			TileBlockLight[0] = true;
			TileSolid[1] = true;
			TileBlockLight[1] = true;
			TileSolid[2] = true;
			TileBlockLight[2] = true;
			TileSolid[3] = false;
			TileNoAttach[3] = true;
			TileNoFail[3] = true;
			TileSolid[4] = false;
			TileNoAttach[4] = true;
			TileNoFail[4] = true;
			TileNoFail[24] = true;
			TileSolid[5] = false;
			TileSolid[6] = true;
			TileBlockLight[6] = true;
			TileSolid[7] = true;
			TileBlockLight[7] = true;
			TileSolid[8] = true;
			TileBlockLight[8] = true;
			TileSolid[9] = true;
			TileBlockLight[9] = true;
			TileBlockLight[10] = true;
			TileSolid[10] = true;
			TileNoAttach[10] = true;
			TileBlockLight[10] = true;
			TileSolid[11] = false;
			TileSolidTop[19] = true;
			TileSolid[19] = true;
			TileSolid[22] = true;
			TileSolid[23] = true;
			TileSolid[25] = true;
			TileSolid[30] = true;
			TileNoFail[32] = true;
			TileBlockLight[32] = true;
			TileSolid[37] = true;
			TileBlockLight[37] = true;
			TileSolid[38] = true;
			TileBlockLight[38] = true;
			TileSolid[39] = true;
			TileBlockLight[39] = true;
			TileSolid[40] = true;
			TileBlockLight[40] = true;
			TileSolid[41] = true;
			TileBlockLight[41] = true;
			TileSolid[43] = true;
			TileBlockLight[43] = true;
			TileSolid[44] = true;
			TileBlockLight[44] = true;
			TileSolid[45] = true;
			TileBlockLight[45] = true;
			TileSolid[46] = true;
			TileBlockLight[46] = true;
			TileSolid[47] = true;
			TileBlockLight[47] = true;
			TileSolid[48] = true;
			TileBlockLight[48] = true;
			TileSolid[53] = true;
			TileBlockLight[53] = true;
			TileSolid[54] = true;
			TileBlockLight[52] = true;
			TileSolid[56] = true;
			TileBlockLight[56] = true;
			TileSolid[57] = true;
			TileBlockLight[57] = true;
			TileSolid[58] = true;
			TileBlockLight[58] = true;
			TileSolid[59] = true;
			TileBlockLight[59] = true;
			TileSolid[60] = true;
			TileBlockLight[60] = true;
			TileSolid[63] = true;
			TileBlockLight[63] = true;
			TileStone[63] = true;
			TileStone[130] = true;
			TileSolid[64] = true;
			TileBlockLight[64] = true;
			TileStone[64] = true;
			TileSolid[65] = true;
			TileBlockLight[65] = true;
			TileStone[65] = true;
			TileSolid[66] = true;
			TileBlockLight[66] = true;
			TileStone[66] = true;
			TileSolid[67] = true;
			TileBlockLight[67] = true;
			TileStone[67] = true;
			TileSolid[68] = true;
			TileBlockLight[68] = true;
			TileStone[68] = true;
			TileSolid[75] = true;
			TileBlockLight[75] = true;
			TileSolid[76] = true;
			TileBlockLight[76] = true;
			TileSolid[70] = true;
			TileBlockLight[70] = true;
			TileNoFail[50] = true;
			TileNoAttach[50] = true;
			TileDungeon[41] = true;
			TileDungeon[43] = true;
			TileDungeon[44] = true;
			TileBlockLight[30] = true;
			TileBlockLight[25] = true;
			TileBlockLight[23] = true;
			TileBlockLight[22] = true;
			TileBlockLight[62] = true;
			TileSolidTop[18] = true;
			TileSolidTop[14] = true;
			TileSolidTop[16] = true;
			TileSolidTop[114] = true;
			TileNoAttach[13] = true;
			TileNoAttach[14] = true;
			TileNoAttach[15] = true;
			TileNoAttach[16] = true;
			TileNoAttach[17] = true;
			TileNoAttach[18] = true;
			TileNoAttach[19] = true;
			TileNoAttach[21] = true;
			TileNoAttach[20] = true;
			TileNoAttach[27] = true;
			TileNoAttach[114] = true;
			TileTable[14] = true;
			TileTable[18] = true;
			TileTable[19] = true;
			TileTable[114] = true;
			TileNoAttach[86] = true;
			TileNoAttach[87] = true;
			TileNoAttach[88] = true;
			TileNoAttach[89] = true;
			TileNoAttach[90] = true;
			TileLavaDeath[86] = true;
			TileLavaDeath[87] = true;
			TileLavaDeath[88] = true;
			TileLavaDeath[89] = true;
			TileLavaDeath[125] = true;
			TileLavaDeath[126] = true;
			TileLavaDeath[101] = true;
			TileTable[101] = true;
			TileNoAttach[101] = true;
			TileLavaDeath[102] = true;
			TileNoAttach[102] = true;
			TileNoAttach[94] = true;
			TileNoAttach[95] = true;
			TileNoAttach[96] = true;
			TileNoAttach[97] = true;
			TileNoAttach[98] = true;
			TileNoAttach[99] = true;
			TileLavaDeath[94] = true;
			TileLavaDeath[95] = true;
			TileLavaDeath[96] = true;
			TileLavaDeath[97] = true;
			TileLavaDeath[98] = true;
			TileLavaDeath[99] = true;
			TileLavaDeath[100] = true;
			TileLavaDeath[103] = true;
			TileTable[87] = true;
			TileTable[88] = true;
			TileSolidTop[87] = true;
			TileSolidTop[88] = true;
			TileSolidTop[101] = true;
			TileNoAttach[91] = true;
			TileLavaDeath[91] = true;
			TileNoAttach[92] = true;
			TileLavaDeath[92] = true;
			TileNoAttach[93] = true;
			TileLavaDeath[93] = true;
			TileWaterDeath[4] = true;
			TileWaterDeath[51] = true;
			TileWaterDeath[93] = true;
			TileWaterDeath[98] = true;
			TileLavaDeath[3] = true;
			TileLavaDeath[5] = true;
			TileLavaDeath[10] = true;
			TileLavaDeath[11] = true;
			TileLavaDeath[12] = true;
			TileLavaDeath[13] = true;
			TileLavaDeath[14] = true;
			TileLavaDeath[15] = true;
			TileLavaDeath[16] = true;
			TileLavaDeath[17] = true;
			TileLavaDeath[18] = true;
			TileLavaDeath[19] = true;
			TileLavaDeath[20] = true;
			TileLavaDeath[27] = true;
			TileLavaDeath[28] = true;
			TileLavaDeath[29] = true;
			TileLavaDeath[32] = true;
			TileLavaDeath[33] = true;
			TileLavaDeath[34] = true;
			TileLavaDeath[35] = true;
			TileLavaDeath[36] = true;
			TileLavaDeath[42] = true;
			TileLavaDeath[49] = true;
			TileLavaDeath[50] = true;
			TileLavaDeath[52] = true;
			TileLavaDeath[55] = true;
			TileLavaDeath[61] = true;
			TileLavaDeath[62] = true;
			TileLavaDeath[69] = true;
			TileLavaDeath[71] = true;
			TileLavaDeath[72] = true;
			TileLavaDeath[73] = true;
			TileLavaDeath[74] = true;
			TileLavaDeath[79] = true;
			TileLavaDeath[80] = true;
			TileLavaDeath[81] = true;
			TileLavaDeath[106] = true;

#if VERSION_101
			TileLighted[150] = true;
			TileFrameImportant[150] = true;
#endif

			WallHouse[1] = true;
			WallHouse[4] = true;
			WallHouse[5] = true;
			WallHouse[6] = true;
			WallHouse[10] = true;
			WallHouse[11] = true;
			WallHouse[12] = true;
			WallHouse[16] = true;
			WallHouse[17] = true;
			WallHouse[18] = true;
			WallHouse[19] = true;
			WallHouse[20] = true;
			WallHouse[21] = true;
			WallHouse[22] = true;
			WallHouse[23] = true;
			WallHouse[24] = true;
			WallHouse[25] = true;
			WallHouse[26] = true;
			WallHouse[27] = true;
			WallHouse[29] = true;
			WallHouse[30] = true;
			WallHouse[31] = true;
			for (int TilesetIdx = MaxNumTilesets - 1; TilesetIdx >= 0; TilesetIdx--)
			{
				TileSolidNotSolidTop[TilesetIdx] = TileSolid[TilesetIdx] & !TileSolidTop[TilesetIdx];
				TileSolidAndAttach[TilesetIdx] = TileSolid[TilesetIdx] & !TileNoAttach[TilesetIdx];
			}
			TileNoFail[32] = true;
			TileNoFail[61] = true;
			TileNoFail[69] = true;
			TileNoFail[73] = true;
			TileNoFail[74] = true;
			TileNoFail[82] = true;
			TileNoFail[83] = true;
			TileNoFail[84] = true;
			TileNoFail[110] = true;
			TileNoFail[113] = true;
			for (int TilesetIdx = 0; TilesetIdx < MaxNumTilesets; TilesetIdx++)
			{
				if (TileSolid[TilesetIdx])
				{
					TileNoSunLight[TilesetIdx] = true;
				}
			}
			TileNoSunLight[19] = false;
			TileNoSunLight[11] = true;
			DustSet.Init();
			for (int j = 0; j < MaxNumItems + 1; j++)
			{
				ItemSet[j].Init();
			}
			for (int NPCIdx = 0; NPCIdx < NPC.MaxNumNPCs + 1; NPCIdx++)
			{
				NPCSet[NPCIdx] = new NPC();
				NPCSet[NPCIdx].WhoAmI = (short)NPCIdx;
			}
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers + 1; PlayerIdx++)
			{
				PlayerSet[PlayerIdx] = new Player();
				PlayerSet[PlayerIdx].WhoAmI = (byte)PlayerIdx;
			}
			for (int ProjIdx = 0; ProjIdx < Projectile.MaxNumProjs; ProjIdx++)
			{
				ProjectileSet[ProjIdx].Init();
			}
			for (int GoreIdx = 0; GoreIdx < Gore.MaxNumGore; GoreIdx++)
			{
				GoreSet[GoreIdx].Init();
			}
			Cloud.Initialize();
			for (int CbtTextIdx = 0; CbtTextIdx < MaxNumCombatText; CbtTextIdx++)
			{
				CombatTextSet[CbtTextIdx].Init();
			}
			Terraria.Recipe.SetupRecipes();
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				ChatLineSet[LineIdx].Init();
			}
			ref Color NoTeamColour = ref TeamColors[0];
			NoTeamColour = Color.White;
			ref Color RedTeamColour = ref TeamColors[1];
			RedTeamColour = new Color(230, 40, 20);
			ref Color GreenTeamColour = ref TeamColors[2];
			GreenTeamColour = new Color(20, 200, 30);
			ref Color BlueTeamColour = ref TeamColors[3];
			BlueTeamColour = new Color(75, 90, 255);
			ref Color YellowTeamColour = ref TeamColors[4];
			YellowTeamColour = new Color(200, 180, 0);
			Projectile InitProj = default;
			for (int ProjType = 1; ProjType < Projectile.MaxNumProjTypes; ProjType++)
			{
				InitProj.SetDefaults(ProjType);
				ProjHostile[ProjType] = InitProj.hostile;
			}
		}

		private void InitializePostSplash()
		{
			UI.Initialize(this);
			WorldView.Initialize(GraphicsDevice);
			for (int Instance = 0; Instance < 4; Instance++)
			{
				UIInstance[Instance] = new UI();
				if (Instance == 0)
				{
					UIInstance[Instance].SetUIView(WorldView.Type.FULLSCREEN, NoAutoFullScreen: true);
				}
				UIInstance[Instance].Initialize((PlayerIndex)Instance);
			}
			Item LoadedItem = default;
			for (int ItemIdx = Item.MaxNumItemTypes - 1; ItemIdx > 0; ItemIdx--)
			{
				LoadedItem.SetDefaults(ItemIdx);
				if (LoadedItem.HeadSlot > 0)
				{
					Item.HeadType[LoadedItem.HeadSlot] = LoadedItem.Type;
				}
				else if (LoadedItem.BodySlot > 0)
				{
					Item.BodyType[LoadedItem.BodySlot] = LoadedItem.Type;
				}
				else if (LoadedItem.LegSlot > 0)
				{
					Item.LegType[LoadedItem.LegSlot] = LoadedItem.Type;
				}
			}
			for (int TownIdx = 0; TownIdx < NPC.MaxNumTownNPCs; TownIdx++)
			{
				Shop[TownIdx] = new Chest();
				if (TownIdx > 0)
				{
					Shop[TownIdx].SetupShop(TownIdx);
				}
			}
			Star.SpawnStars();
			Projectile.Initialize();
			((Collection<IGameComponent>)(object)Components).Add(new GamerServicesComponent(this));
#if USE_ORIGINAL_CODE
			SignedInGamer.add_SignedIn((EventHandler<SignedInEventArgs>)SignedInGamer_SignedIn);
            SignedInGamer.add_SignedOut((EventHandler<SignedOutEventArgs>)SignedInGamer_SignedOut);
			NetworkSession.add_InviteAccepted((EventHandler<InviteAcceptedEventArgs>)Netplay.NetworkSession_InviteAccepted);
#else
			SpecialCheck();

			if (ShowFPS)
			{
				FPSTimer.Start();
			}

			SignedInGamer.SignedIn += SignedInGamer_SignedIn;
            SignedInGamer.SignedOut += SignedInGamer_SignedOut;
			NetworkSession.InviteAccepted += Netplay.NetworkSession_InviteAccepted;
#endif
		}

        protected override void LoadContent()
		{
			GraphicsDevice.DepthStencilState = DepthStencilState.None;
			GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsGame.PreferredBackBufferWidth = ResolutionWidth;
			GraphicsGame.PreferredBackBufferHeight = ResolutionHeight;

#if !USE_ORIGINAL_CODE
			if (StartFullscreen)
			{
				GraphicsGame.IsFullScreen = true;
			}
#endif
			GraphicsGame.ApplyChanges();
			IsHDTV = GraphicsGame.GraphicsDevice.Adapter.CurrentDisplayMode.Height >= 720;
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			string Rating = Lang.SetSystemLang();
			if (Rating != null)
			{
				SplashTextures[0] = Content.Load<Texture2D>(Rating);
			}
			else
			{
				SplashLogo = 1; // If the system region is not accounted for, it will go straight to the 505 Games logo.
				SplashDelay = SplashDelayBase;
			}
			SplashTextures[1] = Content.Load<Texture2D>("Images/logo_1");
            ContentThread = new Thread(LoadingThread);
			ContentThread.IsBackground = true;
			ContentThread.Start();
		}

		private void LoadingThread()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
            try
            {
				for (int LogoIdx = 2; LogoIdx < NumSplashLogos; LogoIdx++)
				{
					SplashTextures[LogoIdx] = Content.Load<Texture2D>("Images/logo_" + LogoIdx);
				}
				UsedAudioEngine = new AudioEngine("Content/TerrariaMusic.xgs");
				UsedSoundBank = new SoundBank(UsedAudioEngine, "Content/Sound Bank.xsb");
				UsedWaveBank = new WaveBank(UsedAudioEngine, "Content/Wave Bank.xwb");
				for (int j = 0; j < MaxNumMusic; j++)
				{
					MusicCues[j] = UsedSoundBank.GetCue(MusicCueNames[j]);
				}
				SoundMech = new SfxInstancePool(Content, "Sounds/Mech_0", 3);
				SoundGrab = new SfxInstancePool(Content, "Sounds/Grab", 3);
				SoundPixie = new SfxInstancePool(Content, "Sounds/Pixie", 2);
				SoundDig[0] = new SfxInstancePool(Content, "Sounds/Dig_0", 3);
				SoundDig[1] = new SfxInstancePool(Content, "Sounds/Dig_1", 3);
				SoundDig[2] = new SfxInstancePool(Content, "Sounds/Dig_2", 3);
				SoundTink[0] = new SfxInstancePool(Content, "Sounds/Tink_0", 3);
				SoundTink[1] = new SfxInstancePool(Content, "Sounds/Tink_1", 3);
				SoundTink[2] = new SfxInstancePool(Content, "Sounds/Tink_2", 3);
				SoundPlayerHit[0] = new SfxInstancePool(Content, "Sounds/Player_Hit_0", 3);
				SoundPlayerHit[1] = new SfxInstancePool(Content, "Sounds/Player_Hit_1", 3);
				SoundPlayerHit[2] = new SfxInstancePool(Content, "Sounds/Player_Hit_2", 3);
				SoundFemaleHit[0] = new SfxInstancePool(Content, "Sounds/Female_Hit_0", 3);
				SoundFemaleHit[1] = new SfxInstancePool(Content, "Sounds/Female_Hit_1", 3);
				SoundFemaleHit[2] = new SfxInstancePool(Content, "Sounds/Female_Hit_2", 3);
				SoundPlayerKilled = new SfxInstancePool(Content, "Sounds/Player_Killed", 3);
				SoundChat = new SfxInstancePool(Content, "Sounds/Chat", 2);
				SoundGrass = new SfxInstancePool(Content, "Sounds/Grass", 6);
				SoundDoorOpen = new SfxInstancePool(Content, "Sounds/Door_Opened", 3);
				SoundDoorClosed = new SfxInstancePool(Content, "Sounds/Door_Closed", 3);
				SoundMenuTick = new SfxInstancePool(Content, "Sounds/Menu_Tick", 3);
				SoundMenuOpen = new SfxInstancePool(Content, "Sounds/Menu_Open", 3);
				SoundMenuClose = new SfxInstancePool(Content, "Sounds/Menu_Close", 3);
				SoundShatter = new SfxInstancePool(Content, "Sounds/Shatter", 4);
				SoundZombie[0] = new SfxInstancePool(Content, "Sounds/Zombie_0", 4);
				SoundZombie[1] = new SfxInstancePool(Content, "Sounds/Zombie_1", 4);
				SoundZombie[2] = new SfxInstancePool(Content, "Sounds/Zombie_2", 4);
				SoundZombie[3] = new SfxInstancePool(Content, "Sounds/Zombie_3", 4);
				SoundZombie[4] = new SfxInstancePool(Content, "Sounds/Zombie_4", 4);
				SoundRoar[0] = new SfxInstancePool(Content, "Sounds/Roar_0", 2);
				SoundRoar[1] = new SfxInstancePool(Content, "Sounds/Roar_1", 2);
				SoundSplash[0] = new SfxInstancePool(Content, "Sounds/Splash_0", 4);
				SoundSplash[1] = new SfxInstancePool(Content, "Sounds/Splash_1", 4);
				SoundDoubleJump = new SfxInstancePool(Content, "Sounds/Double_Jump", 3);
				SoundRun = new SfxInstancePool(Content, "Sounds/Run", 7);
				SoundCoins = new SfxInstancePool(Content, "Sounds/Coins", 4);
				SoundUnlock = new SfxInstancePool(Content, "Sounds/Unlock", 4);
				SoundMaxMana = new SfxInstancePool(Content, "Sounds/MaxMana", 4);
				SoundDrown = new SfxInstancePool(Content, "Sounds/Drown", 4);
				// There is no instance of Sounds/Jump being used, meaning this is unused across all old-gen console versions of the game.
				for (int SoundIdx = 0; SoundIdx < MaxNumItemSounds; SoundIdx++)
				{
					int SoundMatch = SoundIdx + 1;
					int MaxInstances = 3;
					if (SoundMatch != 9 && SoundMatch != 10 && SoundMatch != 24 && SoundMatch != 26 && SoundMatch != 34)
					{
						MaxInstances = 2;
					}
					SoundItem[SoundIdx] = new SfxInstancePool(Content, "Sounds/Item_" + SoundMatch, MaxInstances);
				}
				for (int SoundIdx = 0; SoundIdx < MaxNumNPCHitSounds; SoundIdx++)
				{
					SoundNPCHit[SoundIdx] = new SfxInstancePool(Content, "Sounds/NPC_Hit_" + (SoundIdx + 1), 4);
				}
				for (int SoundIdx = 0; SoundIdx < MaxNumNPCKillSounds; SoundIdx++)
				{
					SoundNPCKilled[SoundIdx] = new SfxInstancePool(Content, "Sounds/NPC_Killed_" + (SoundIdx + 1), 3);
				}
			}
			catch
			{
				MusicVolume = 0f;
				SoundVolume = 0f;
			}
			_sheetTiles.LoadContent(Content);
			_sheetSprites.LoadContent(Content);
			WorldView.LoadContent(Content);
			WhiteTexture = new Texture2D(GraphicsDevice, 1, 1, mipMap: false, SurfaceFormat.Bgr565);
			WhiteTexture.SetData(new ushort[1]
			{
				65535
			});
			UI.LoadContent(Content);
			CRC32.Initialize();
		}

		protected override void UnloadContent()
		{
		}

		private void UpdateMusic(Player MainPlayer)
		{
			try
			{
				if (CurrentMusic != Music.NUM_SONGS && MusicCues[(uint)CurrentMusic].IsPaused)
				{
					MusicCues[(uint)CurrentMusic].Resume();
				}
				if (MusicVolume == 0f)
				{
					NewMusic = Music.NUM_SONGS;
				}
				else
				{
					bool InMenu = true;
					for (int Instance = 0; Instance < 4; Instance++)
					{
						if (UIInstance[Instance].CurMenuType != 0)
						{
							InMenu = false;
							break;
						}
					}
					if (InMenu)
					{
						if (UI.MainUI.CurMenuMode == MenuMode.CREDITS)
						{
							NewMusic = Music.TUTORIAL;
						}
						else
						{
							NewMusic = Music.TITLE;
						}
					}
					else if (IsTutorial())
					{
						NewMusic = Music.TUTORIAL;
					}
					else
					{
						int BossMusic = 0;
						int Area = 10000;
						bool InArea = false;
						Rectangle GameArea = default;
						GameArea.Width = Area;
						GameArea.Height = Area;
						WorldView CurrentView = MainPlayer.CurrentView;
						for (int NPCIdx = 0; NPCIdx < NPC.MaxNumNPCs; NPCIdx++)
						{
							if (NPCSet[NPCIdx].Active == 0)
							{
								continue;
							}
							int NPCType = NPCSet[NPCIdx].Type;
							switch (NPCType)
							{
							case (int)NPC.ID.THE_DESTROYER_HEAD:
							case (int)NPC.ID.SNOWMAN_GANGSTA:
							case (int)NPC.ID.MISTER_STABBY:
							case (int)NPC.ID.SNOW_BALLA:
								GameArea.X = NPCSet[NPCIdx].XYWH.X + (NPCSet[NPCIdx].Width >> 1) - (Area / 2);
								GameArea.Y = NPCSet[NPCIdx].XYWH.Y + (NPCSet[NPCIdx].Height >> 1) - (Area / 2);
								CurrentView.ViewArea.Intersects(ref GameArea, out InArea);
								if (!InArea)
								{
									continue;
								}
								BossMusic = 3;
								break;
							default:
								switch (NPCType)
								{
								case (int)NPC.ID.WALL_OF_FLESH: // For some reason it checks if both the eyes and the mouth of the WoF are active... like as if they can be separated.
								case (int)NPC.ID.WALL_OF_FLESH_EYE: // Interesting since literally 17 lines up is a case where only The Destroyer's head is checked, rather than each segment.
								case (int)NPC.ID.RETINAZER:
								case (int)NPC.ID.SPAZMATISM:
									GameArea.X = NPCSet[NPCIdx].XYWH.X + (NPCSet[NPCIdx].Width >> 1) - (Area / 2);
									GameArea.Y = NPCSet[NPCIdx].XYWH.Y + (NPCSet[NPCIdx].Height >> 1) - (Area / 2);
									CurrentView.ViewArea.Intersects(ref GameArea, out InArea);
									if (!InArea)
									{
										continue;
									}
									BossMusic = 2;
									break;
								case (int)NPC.ID.EYE_OF_CTHULHU: // Console Exclusive: If an Eye of Cthulhu is active, play the Ocram music
								case (int)NPC.ID.OCRAM:
									GameArea.X = NPCSet[NPCIdx].XYWH.X + (NPCSet[NPCIdx].Width >> 1) - (Area / 2);
									GameArea.Y = NPCSet[NPCIdx].XYWH.Y + (NPCSet[NPCIdx].Height >> 1) - (Area / 2);
									CurrentView.ViewArea.Intersects(ref GameArea, out InArea);
									if (!InArea)
									{
										continue;
									}
									BossMusic = 4;
									break;
								default:
									if (!NPCSet[NPCIdx].IsBoss && (NPCType < (int)NPC.ID.EATER_OF_WORLDS_HEAD || NPCType > (int)NPC.ID.EATER_OF_WORLDS_TAIL) && (NPCType < (int)NPC.ID.GOBLIN_PEON || NPCType > (int)NPC.ID.GOBLIN_SORCERER) && NPCType != (int)NPC.ID.GOBLIN_ARCHER) // Checks for the EoW's segments (again, like they can be separated) or the Goblin Army.
									{
										continue;
									}
									GameArea.X = NPCSet[NPCIdx].XYWH.X + (NPCSet[NPCIdx].Width >> 1) - (Area / 2);
									GameArea.Y = NPCSet[NPCIdx].XYWH.Y + (NPCSet[NPCIdx].Height >> 1) - (Area / 2);
									CurrentView.ViewArea.Intersects(ref GameArea, out InArea);
									if (!InArea)
									{
										continue;
									}
									BossMusic = 1;
									break;
								}
								break;
							}
							break;
						}
						if (BossMusic > 0)
						{
							switch (BossMusic)
							{
							case 1:
								NewMusic = Music.BOSS1;
								break;
							case 2:
								NewMusic = Music.BOSS2;
								break;
							case 3:
								NewMusic = Music.BOSS3;
								break;
							case 4:
								NewMusic = Music.BOSS4;
								break;
							}
						}
						else if (CurrentView.ScreenPosition.Y > MaxTilesY - 200 << 4)
						{
							NewMusic = Music.EERIE;
						}
						else if (MainPlayer.ZoneEvil)
						{
							if (CurrentView.ScreenPosition.Y > WorldSurfacePixels + ResolutionHeight)
							{
								NewMusic = Music.UNDERGROUND_CORRUPTION;
							}
							else
							{
								NewMusic = Music.CORRUPTION;
							}
						}
						else if (CurrentView.AtmoOpacity < 1f)
						{
							NewMusic = Music.FLOATING_ISLAND;
						}
						else if ((CurrentView.ScreenPosition.X < 3200 || CurrentView.ScreenPosition.X > (MaxTilesX - 200 << 4) - ResolutionWidth) && CurrentView.ScreenPosition.Y <= WorldSurfacePixels)
						{
							NewMusic = Music.OCEAN;
						}
						else if (MainPlayer.ZoneMeteor || MainPlayer.ZoneDungeon)
						{
							NewMusic = Music.EERIE;
						}
						else if (MainPlayer.ZoneJungle)
						{
							NewMusic = Music.JUNGLE;
						}
						else if (MainPlayer.CurrentView.SandTiles > 1000)
						{
							NewMusic = Music.DESERT;
						}
						else if (MainPlayer.CurrentView.SnowTiles > 80)
						{
							NewMusic = Music.SNOW;
						}
						else if (CurrentView.ScreenPosition.Y > WorldSurfacePixels)
						{
							if (MainPlayer.zoneHoly)
							{
								NewMusic = Music.UNDERGROUND_HALLOW;
							}
							else
							{
								NewMusic = Music.UNDERGROUND;
							}
						}
						else if (GameTime.DayTime)
						{
							if (MainPlayer.zoneHoly)
							{
								NewMusic = Music.THE_HALLOW;
							}
							else
							{
								NewMusic = Music.OVERWORLD_DAY;
							}
						}
						else if (!GameTime.DayTime)
						{
							if (GameTime.IsBloodMoon)
							{
								NewMusic = Music.EERIE;
							}
							else
							{
								NewMusic = Music.OVERWORLD_NIGHT;
							}
						}
						int Instance = 0;
						while (MusicBox < 0 && Instance < 4)
						{
							if (UIInstance[Instance].CurrentView != null)
							{
								MusicBox = UIInstance[Instance].CurrentView.MusicBox;
							}
							Instance++;
						}
						if (MusicBox >= 0)
						{
							NewMusic = MusicBoxToSong[MusicBox];
						}
					}
				}
				CurrentMusic = NewMusic;
				for (int Cue = 0; Cue < MaxNumMusic; Cue++)
				{
					if (Cue == (int)CurrentMusic)
					{
						if (!MusicCues[Cue].IsPlaying)
						{
							MusicCues[Cue] = UsedSoundBank.GetCue(MusicCueNames[Cue]);
							MusicCues[Cue].Play();
						}
						else
						{
							MusicFade[Cue] += 0.005f;
							if (MusicFade[Cue] > 1f)
							{
								MusicFade[Cue] = 1f;
							}
						}
						MusicCues[Cue].SetVariable("Volume", MusicFade[Cue] * MusicVolume);
					}
					else if (MusicCues[Cue].IsPlaying)
					{
						if (CurrentMusic == Music.NUM_SONGS)
						{
							MusicFade[Cue] = 0f;
							MusicCues[Cue].Stop(AudioStopOptions.Immediate);
						}
						else if (MusicFade[(uint)CurrentMusic] > 0.25f)
						{
							MusicFade[Cue] -= 0.005f;
							if (MusicFade[Cue] <= 0f)
							{
								MusicFade[Cue] = 0f;
								MusicCues[Cue].Stop(AudioStopOptions.Immediate);
							}
							else
							{
								MusicCues[Cue].SetVariable("Volume", MusicFade[Cue] * MusicVolume);
							}
						}
					}
					else
					{
						MusicFade[Cue] = 0f;
					}
				}
			}
			catch
			{
				MusicVolume = 0f;
			}
		}

		protected override void Update(GameTime GTime)
		{
			try
			{
				base.Update(GTime);
			}
			catch
			{
			}
			IsRunningSlowly = GTime.IsRunningSlowly;
			FrameCounter++;
			switch (ShowSplash)
			{
			case 0:
				return;
			case 1:
				ContentThread.Join();
				ContentThread = null;
				ShowSplash = 2;
				InitializePostSplash();
				return;
			}
			for (int Instance = 0; Instance < 4; Instance++)
			{
				UIInstance[Instance].UpdateGamePad();
			}
			if (TutorialState < Tutorial.THE_END)
			{
				UpdateTutorial();
			}
			for (int Instance = 0; Instance < 4; Instance++)
			{
				UIInstance[Instance].Update();
			}
			DustSet.UpdateDust();
			if (UI.HasQuit)
			{
				Quit();
				return;
			}
			UI.UpdateOnce();
			AchievementSystem.Update();
			UsedAudioEngine.Update();
			WorldGen.ToDestroyObject = false;
			UpdateMusic(UI.MainUI.ActivePlayer);
			HasFocus = IsActive;
			IsGamePaused = !HasFocus && NetMode == (byte)NetModeSetting.LOCAL;
			if (IsGamePaused)
			{
				return;
			}

#if !USE_ORIGINAL_CODE
			if (ShowFPS && FPSTimer.ElapsedMilliseconds >= 1000)
			{
				FPS = FPSCount;
				FPSCount = 0;
				FPSTimer.Restart();
			}

			CheckKeys();
			if (HasKeyBeenPressed(Keys.F))
			{
				GraphicsGame.ToggleFullScreen();
			}

			if (HasKeyBeenPressed(Keys.OemPlus))
			{
				if (ZoomLevel < 2f)
				{
					if ((ZoomLevel += 0.1f) > 2f)
					{
						ZoomLevel = 2f;
					}

					UI.MainUI.ActivePlayer.CurrentView.Zoom(ZoomLevel);
				}
			}

			if (HasKeyBeenPressed(Keys.OemMinus))
			{
				if (ZoomLevel > 1f)
				{
					if ((ZoomLevel -= 0.1f) < 1f)
					{
						ZoomLevel = 1f;
					}
					UI.MainUI.ActivePlayer.CurrentView.Zoom(ZoomLevel);
				}
			}
#endif

			if (Netplay.Session != null)
			{
				if (!Netplay.PlayDisconnect)
				{
					if (Netplay.HasHookEvents)
					{
						Netplay.HookSessionEvents();
					}
					if (NetMode == (byte)NetModeSetting.CLIENT)
					{
						UpdateClient();
					}
					else
					{
						UpdateServer();
					}
				}
				else if (Netplay.ToStopSession)
				{
					Netplay.Disconnect();
				}
			}
			if (Netplay.CurInvite != null)
			{
				Netplay.InviteAccepted();
			}
			if (NetMode == (byte)NetModeSetting.LOCAL)
			{
				if (UI.MainUI.CurMenuType == MenuType.PAUSE)
				{
					bool Paused = true;
					for (int Instance = 0; Instance < 4; Instance++)
					{
						if (UIInstance[Instance].CurMenuType == MenuType.NONE)
						{
							Paused = false;
							break;
						}
					}
					if (Paused)
					{
						IsGamePaused = true;
						return;
					}
				}
			}
			else if (CheckWorldId)
			{
				for (int Instance = 0; Instance < 4; Instance++)
				{
					UI ActiveUI = UIInstance[Instance];
					if (ActiveUI.CurrentView != null && ActiveUI.CurMenuType == MenuType.NONE)
					{
						CheckWorldId = false;
						if (ActiveUI.CheckBlacklist())
						{
							break;
						}
					}
				}
			}
			Star.UpdateStars();
			Cloud.UpdateClouds();
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
					ChatLineSet[LineIdx].ShowTime--;
				}
			}
			for (int Instance = 0; Instance < 4; Instance++)
			{
				if (UIInstance[Instance].CurrentView != null && UIInstance[Instance].CurMenuType == MenuType.MAIN)
				{
					UpdateMenuTime();
					break;
				}
			}
			if (!IsGameStarted)
			{
				return;
			}
			if (NetMode != (byte)NetModeSetting.LOCAL && CheckUserGeneratedContent)
			{
				CheckUserGeneratedContent = false;
				UI.MainUI.CheckUserGeneratedContent();
			}
			if (DiscoStyle == 0)
			{
				DiscoRGB.Y += 7f / 255f;
				if (DiscoRGB.Y >= 1f)
				{
					DiscoRGB.Y = 1f;
					DiscoStyle = 1;
				}
				DiscoRGB.X -= 7f / 255f;
				if (DiscoRGB.X < 0f)
				{
					DiscoRGB.X = 0f;
				}
			}
			else if (DiscoStyle == 1)
			{
				DiscoRGB.Z += 7f / 255f;
				if (DiscoRGB.Z >= 1f)
				{
					DiscoRGB.Z = 1f;
					DiscoStyle = 2;
				}
				DiscoRGB.Y -= 7f / 255f;
				if (DiscoRGB.Y < 0f)
				{
					DiscoRGB.Y = 0f;
				}
			}
			else
			{
				DiscoRGB.X += 7f / 255f;
				if (DiscoRGB.X >= 1f)
				{
					DiscoRGB.X = 1f;
					DiscoStyle = 0;
				}
				DiscoRGB.Z -= 7f / 255f;
				if (DiscoRGB.Z < 0f)
				{
					DiscoRGB.Z = 0f;
				}
			}
			if ((FrameCounter & 7) == 0 && ++MagmaBGFrame >= 3)
			{
				MagmaBGFrame = 0;
			}
			DemonTorch += DemonTorchDir;
			if (DemonTorch > 1f)
			{
				DemonTorch = 1f;
				DemonTorchDir = 0f - DemonTorchDir;
			}
			else if (DemonTorch < 0f)
			{
				DemonTorch = 0f;
				DemonTorchDir = 0f - DemonTorchDir;
			}

#if VERSION_101
			LiquidFrCounter += Cloud.WindSpeed * 2f;
			if (LiquidFrCounter > 4f)
			{
				LiquidFrCounter = 0f;
				LiquidFrame += 1f;
			}
			if (LiquidFrCounter < 0f)
			{
				LiquidFrCounter = 4f;
				LiquidFrame -= 1f;
			}
			if (LiquidFrame > 16f)
			{
				LiquidFrame = 1f;
			}
			if (LiquidFrame < 1f)
			{
				LiquidFrame = 16f;
			}

			HeartCrystalFrCounter++;
			if (HeartCrystalFrCounter > 5)
			{
				HeartCrystalFrCounter = 0;
				HeartCrystalFrame++;
				if (HeartCrystalFrame >= 10)
				{
					HeartCrystalFrame = 0;
				}
			}
			FurnaceFrCounter++;
			if (FurnaceFrCounter > 5)
			{
				FurnaceFrCounter = 0;
				FurnaceFrame++;
				if (FurnaceFrame >= 12)
				{
					FurnaceFrame = 0;
				}
			}
			ShadowOrbFrCounter++;
			if (ShadowOrbFrCounter > 10)
			{
				ShadowOrbFrCounter = 0;
				ShadowOrbFrame++;
				if (ShadowOrbFrame > 1)
				{
					ShadowOrbFrame = 0;
				}
			}
			CampfireFrCounter++;
			if (CampfireFrCounter > 4)
			{
				CampfireFrCounter = 0;
				CampfireFrame++;
				if (CampfireFrame >= 4)
				{
					CampfireFrame = 0;
				}
			}
#endif

			if (NetMode != (byte)NetModeSetting.CLIENT)
			{
				WorldGen.UpdateWorld();
				UpdateInvasion();
			}
			MusicBox = -1;
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (PlayerSet[PlayerIdx].Active != 0)
				{
					PlayerSet[PlayerIdx].UpdatePlayer(PlayerIdx);
				}
			}
			if (NetMode != (byte)NetModeSetting.CLIENT && TutorialState >= Tutorial.THE_END)
			{
				NPC.SpawnNPC();
			}
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				PlayerSet[PlayerIdx].ActiveNPCs = 0f;
				PlayerSet[PlayerIdx].TownNPCs = 0f;
			}
			if (NPC.WoF >= 0 && NPCSet[NPC.WoF].Active == 0)
			{
				NPC.WoF = -1;
			}
			for (int NPCIdx = NPC.MaxNumNPCs - 1; NPCIdx >= 0; NPCIdx--)
			{
				if (NPCSet[NPCIdx].Active != 0)
				{
					NPCSet[NPCIdx].UpdateNPC(NPCIdx);
				}
			}
			for (int GoreIdx = 0; GoreIdx < Gore.MaxNumGore; GoreIdx++)
			{
				if (GoreSet[GoreIdx].Active != 0)
				{
					GoreSet[GoreIdx].Update();
				}
			}
			for (int ProjIdx = 0; ProjIdx < Projectile.MaxNumProjs; ProjIdx++)
			{
				if (ProjectileSet[ProjIdx].active != 0)
				{
					ProjectileSet[ProjIdx].Update(ProjIdx);
				}
			}
			for (int ItemIdx = 0; ItemIdx < MaxNumItems; ItemIdx++)
			{
				if (ItemSet[ItemIdx].Active != 0)
				{
					ItemSet[ItemIdx].UpdateItem(ItemIdx);
				}
			}
			CombatText.UpdateCombatText();
			UpdateTime();
		}

		private static void UpdateMenuTime()
		{
			MenuTime.Update();
			if (NetMode == (byte)NetModeSetting.CLIENT)
			{
				UpdateTime();
			}
		}

		private void DrawSplash(GameTime GTime)
		{
			GraphicsDevice.Clear(default);
			base.Draw(GTime);
			if (SplashCounter == SplashDelay + 16 + 16)
			{
				SplashTextures[SplashLogo].Dispose();
				SplashTextures[SplashLogo] = null;
				SplashDelay = SplashDelayBase;
				SplashCounter = 0;
				if (++SplashLogo == NumSplashLogos)
				{
					ShowSplash = 1;
					return;
				}
			}
			SplashCounter++;
			SpriteBatch.Begin();
			int FadeVal;
			if (SplashCounter < SplashFadeOut)
			{
				FadeVal = SplashCounter * 255 / SplashFadeOut;
			}
			else if (SplashCounter <= SplashFadeIn + SplashDelay)
			{
				FadeVal = 255;
			}
			else
			{
				FadeVal = SplashCounter - SplashDelay - SplashFadeIn;
				FadeVal = 255 - FadeVal * 255 / SplashFadeIn;
			}
			Vector2 Position = default;
			Position.X = ResolutionWidth - SplashTextures[SplashLogo].Width >> 1;
			Position.Y = ResolutionHeight - SplashTextures[SplashLogo].Height >> 1;
			SpriteBatch.Draw(SplashTextures[SplashLogo], Position, new Color(FadeVal, FadeVal, FadeVal, FadeVal)); // Was thinking of stretching the logos but think it looks better without it.
			SpriteBatch.End();
		}

		public void LoadUpsell()
		{
			if (!HasUpsellLoaded)
			{
				HasUpsellLoaded = true;
				for (int SplashIdx = 0; SplashIdx < UpsellNumScreens; SplashIdx++)
				{
					SplashTextures[SplashIdx] = Content.Load<Texture2D>("UI/Upsell/0" + (SplashIdx + UpsellNumScreens) + "_" + Lang.LanguageId);
				}
			}
			SplashLogo = 0;
			SplashCounter = 0;
		}

		public void DrawUpsell()
		{
			SplashCounter++;
			int FadeVal = ((SplashCounter >= 16) ? 255 : (SplashCounter * 255 / 16));
#if USE_ORIGINAL_CODE // Unsurprisingly, the upsell sprite is 960x540, so Engine only needed to display it as-is on the Xbox 360 version...
			Vector2 Position = default; 
			Position.X = ResolutionWidth - SplashTextures[SplashLogo].Width >> 1;
			Position.Y = ResolutionHeight - SplashTextures[SplashLogo].Height >> 1;
			SpriteBatch.Draw(SplashTextures[SplashLogo], Position, new Color(FadeVal, FadeVal, FadeVal, FadeVal));
#else       // But since I'm allowing for greater screen resolutions, we draw with a rectangle, which will display perfectly on 540p, or stretch with higher resolutions.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
			SpriteBatch.Draw(SplashTextures[SplashLogo], new Rectangle(0, 0, ResolutionWidth, ResolutionHeight), new Color(FadeVal, FadeVal, FadeVal, FadeVal));
#endif
		}

#if !USE_ORIGINAL_CODE
		public void DrawFPS()
		{
			if (ShowFPS)
			{
				Vector2 StringVector = UI.BoldSmallFont.MeasureString(FPS.ToString());
				int StringX = ResolutionWidth - UI.CurrentUI.CurrentView.SafeAreaOffsetLeft;
				int StringY = UI.CurrentUI.CurrentView.SafeAreaOffsetTop;
				SpriteBatch.DrawString(UI.BoldSmallFont, "FPS: " + FPS, new Vector2(StringX - StringVector.X, 0 + StringY), Color.White, 0f, new Vector2(0f, StringVector.Y), (UI.NumActiveViews <= 1) ? 1 : 2, SpriteEffects.None, 0f);
			}
		}
#endif

		protected override void Draw(GameTime GTime)
		{
			if (ShowSplash == 0)
			{
				DrawSplash(GTime);
				return;
			}
#if !USE_ORIGINAL_CODE
			FPSCount++;
#endif
			RenderCount++;
			for (int Instance = 0; Instance < 4; Instance++)
			{
				UIInstance[Instance].PrepareDraw(RenderCount);
			}
			if (RenderCount > 4)
			{
				RenderCount = 0;
				Lighting.TempLightCount = 0;
			}
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(default);
			base.Draw(GTime);
			for (int Instance = 0; Instance < 4; Instance++)
			{
				UIInstance[Instance].Draw();
			}
			WorldView.RestoreViewport();
			SpriteBatch.Begin();
			for (int Instance = 0; Instance < 4; Instance++)
			{
				if (UIInstance[Instance].CurMenuType != 0)
				{
					DrawChat();
					break;
				}
			}

#if !USE_ORIGINAL_CODE
			DrawFPS();
#endif

			if (SaveIconCounter > 0 || ActiveSaves > 0)
			{
				SaveIconCounter--;
				int IconX = 878;
				int IconY = 479;
				Color IconColour = Color.White;

#if VERSION_103 || VERSION_FINAL // 1.02+ added a dope little fade effect to the save icon.
				if (SaveIconCounter < 62)
				{
					if (ActiveSaves < 1)
					{
						IconColour.R = (byte)(SaveIconCounter << 2);
						IconColour.G = (byte)(SaveIconCounter << 2);
						IconColour.B = (byte)(SaveIconCounter << 2);
						IconColour.A = (byte)(SaveIconCounter << 2);
					}
					else
					{
						SaveIconCounter = 64;
					}
				}
#endif

#if !USE_ORIGINAL_CODE
				switch (ScreenHeightPtr)
				{
					case 1:
						IconX = 1182;
						IconY = 650;
						break;
					case 2:
						IconX = 1790;
						IconY = 992;
						break;
				}
#endif
				SpriteSheet<_sheetSprites>.Draw(SaveIconSprite, IconX, IconY, IconColour, (float)(SaveIconCounter * (Math.PI / 60.0)), 1f);
			}
			SpriteBatch.End();
		}

		private static void UpdateInvasion()
		{
			if (InvasionType <= 0)
			{
				return;
			}
			if (InvasionSize <= 0)
			{
				if (InvasionType == 1)
				{
					NPC.HasDownedGoblins = true;
					UI.SetTriggerStateForAll(Trigger.KilledGoblinArmy);
					NetMessage.CreateMessage0(7);
					NetMessage.SendMessage();
				}
				else if (InvasionType == 2)
				{
					NPC.HasDownedFrost = true;
				}
				InvasionWarning();
				InvasionType = 0;
				InvasionDelay = 7;
			}
			if (InvasionX == SpawnTileX)
			{
				return;
			}
			float Increment = 1f;
			if (InvasionX > SpawnTileX)
			{
				InvasionX -= Increment;
				if (InvasionX <= SpawnTileX)
				{
					InvasionX = SpawnTileX;
					InvasionWarning();
				}
				else
				{
					InvasionWarn--;
				}
			}
			else if (InvasionX < SpawnTileX)
			{
				InvasionX += Increment;
				if (InvasionX >= SpawnTileX)
				{
					InvasionX = SpawnTileX;
					InvasionWarning();
				}
				else
				{
					InvasionWarn--;
				}
			}
			if (InvasionWarn <= 0)
			{
				InvasionWarn = 3600;
				InvasionWarning();
			}
		}

		private static void InvasionWarning()
		{
			int InvadeTextId = ((InvasionSize <= 0) ? ((InvasionType == 2) ? 4 : 0) : ((InvasionX < SpawnTileX) ? ((InvasionType != 2) ? 1 : 5) : ((InvasionX > SpawnTileX) ? ((InvasionType != 2) ? 2 : 6) : ((InvasionType != 2) ? 3 : 7))));
			NetMessage.SendText(InvadeTextId, 175, 75, 255, -1);
		}

		public static void StartInvasion(int InvadeType = 1)
		{
			if (InvasionType != 0 || InvasionDelay != 0)
			{
				return;
			}
			int DiffMultiplier = 0;
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (PlayerSet[PlayerIdx].Active != 0 && PlayerSet[PlayerIdx].StatLifeMax >= 200)
				{
					DiffMultiplier++;
				}
			}
			if (DiffMultiplier > 0)
			{
				InvasionType = InvadeType;
				InvasionSize = 80 + 40 * DiffMultiplier;
				InvasionWarn = 0;
				if (Rand.Next(2) == 0)
				{
					InvasionX = 0f;
				}
				else
				{
					InvasionX = MaxTilesX;
				}
			}
		}

		private static void UpdateClient()
		{
			if (IsGameStarted)
			{
				NetPlayCounter++;
			}
			Netplay.Session.Update();
			NetMessage.CheckBytesClient();
		}

		private static void UpdateServer()
		{
			if (IsGameStarted)
			{
				NetPlayCounter++;
				for (int ClientIdx = Netplay.ClientList.Count - 1; ClientIdx >= 0; ClientIdx--)
				{
					NetClient ActiveNetClient = Netplay.ClientList[ClientIdx];
					for (int GamerIdx = ((ReadOnlyCollection<NetworkGamer>)(object)ActiveNetClient.Machine.Gamers).Count - 1; GamerIdx >= 0; GamerIdx--)
					{
						Player OnlinePlayer = ((ReadOnlyCollection<NetworkGamer>)(object)ActiveNetClient.Machine.Gamers)[GamerIdx].Tag as Player;
						if (OnlinePlayer.Active != 0)
						{
							int SectX = (OnlinePlayer.XYWH.X >> 4) / 40;
							int SectY = (OnlinePlayer.XYWH.Y >> 4) / 30;
							NetMessage.SendSection(ActiveNetClient, SectX, SectY);
							if (OnlinePlayer.velocity.X > 0f)
							{
								if (!NetMessage.SendSection(ActiveNetClient, SectX + 1, SectY) && !NetMessage.SendSection(ActiveNetClient, SectX + 1, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 1, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 2, SectY) && !NetMessage.SendSection(ActiveNetClient, SectX + 2, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 2, SectY + 1))
								{
								}
							}
							else if (OnlinePlayer.velocity.X < 0f && !NetMessage.SendSection(ActiveNetClient, SectX - 1, SectY) && !NetMessage.SendSection(ActiveNetClient, SectX - 1, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX - 1, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX - 2, SectY) && !NetMessage.SendSection(ActiveNetClient, SectX - 2, SectY - 1))
							{
								NetMessage.SendSection(ActiveNetClient, SectX - 2, SectY + 1);
							}
							if (OnlinePlayer.velocity.Y > 0f)
							{
								if (!NetMessage.SendSection(ActiveNetClient, SectX, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 1, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX - 1, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 2, SectY + 1) && !NetMessage.SendSection(ActiveNetClient, SectX - 2, SectY + 1))
								{
								}
							}
							else if (OnlinePlayer.velocity.Y < 0f && !NetMessage.SendSection(ActiveNetClient, SectX, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 1, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX - 1, SectY - 1) && !NetMessage.SendSection(ActiveNetClient, SectX + 2, SectY - 1))
							{
								NetMessage.SendSection(ActiveNetClient, SectX - 2, SectY - 1);
							}
						}
					}
				}
			}
			try
			{
				Netplay.Session.Update();
			}
			catch (Exception)
			{
			}
			if (NetMode == (byte)NetModeSetting.LOCAL)
			{
				Netplay.CheckOfflineSession();
				return;
			}
			NetMessage.CheckBytesServer();
			foreach (NetworkGamer RemoteGamer in Netplay.Session.RemoteGamers)
			{
				Player RemotePlayer = RemoteGamer.Tag as Player;
				if (RemotePlayer.ToKill)
				{
					RemotePlayer.ToKill = false;
					RemotePlayer.Active = 0;
					RemoteGamer.Machine.RemoveFromSession();
				}
			}
        }

        public static void NewText(string Text, int R, int G, int B)
		{
			for (int LineIdx = MaxNumChatLines - 1; LineIdx > 0; LineIdx--)
			{
				ref ChatLine reference = ref ChatLineSet[LineIdx];
				reference = ChatLineSet[LineIdx - 1];
			}
			ChatLineSet[0].Color = new Color(R, G, B);
			ChatLineSet[0].Text = Text;

#if (!VERSION_INITIAL || IS_PATCHED)
            ChatLineSet[0].Size = UI.BoldSmallFont.MeasureString(Text);
#endif

			ChatLineSet[0].ShowTime = ChatLength;
			PlaySound(12);
		}

		public static void DrawChat()
		{
#if USE_ORIGINAL_CODE
			int YOffset = 0;
			float LineWidth = 0f;
			for (int ChatIdx = 0; ChatIdx < MaxNumChatLines; ChatIdx++)
			{
				if (ChatLineSet[ChatIdx].ShowTime > 0)
				{
					Vector2 FontSize = UI.BoldSmallFont.MeasureString(ChatLineSet[ChatIdx].Text);
					if (FontSize.X > LineWidth)
					{
						LineWidth = FontSize.X;
					}
					YOffset++;
				}
			}
			if (YOffset == 0 || LineWidth == 0f)
			{
				return;
			}
			DrawRect(new Rectangle(48, 440 - YOffset * 22, (int)LineWidth + 12, YOffset * 22 + 12), new Color(32, 32, 32, 32));
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
					float Alpha = (float)(int)UI.MouseTextBrightness * (1f / 255f);
					SpriteBatch.DrawString(UI.BoldSmallFont, ChatLineSet[LineIdx].Text, new Vector2(54f, 439 - (LineIdx + 1) * 22), new Color((byte)(ChatLineSet[LineIdx].Color.R * Alpha), (byte)(ChatLineSet[LineIdx].Color.G * Alpha), (byte)(ChatLineSet[LineIdx].Color.B * Alpha), UI.MouseTextBrightness));
				}
			}
#else
#if (VERSION_INITIAL && !IS_PATCHED)
			int YOffset = 0;
			float LineWidth = 0f;
			int PosX = 48;
			int PosY = 440;
			int SpaceMult = 22;

			switch (ScreenHeightPtr)
			{
				case 1:
					PosX = 66;
					PosY = 620;
					break;
				case 2:
					PosX = 98;
					PosY = 980;
					SpaceMult = 50;
					break;
			}

			float ChatX = PosX + 6;
			int ChatY = PosY - 1;

			if (ScreenHeightPtr == 2)
			{
				ChatY = PosY + 6;
			}

			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
					Vector2 FontSize = UI.BoldSmallFont.MeasureString(ChatLineSet[LineIdx].Text);
					if (FontSize.X > LineWidth)
					{
						LineWidth = FontSize.X;
					}
					YOffset++;
                }
			}
			if (YOffset == 0 || LineWidth == 0f)
			{
				return;
			}
			DrawRect(new Rectangle(PosX, PosY - YOffset * SpaceMult, (int)LineWidth + 12, YOffset * SpaceMult + 12), new Color(32, 32, 32, 32));
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
					float Alpha = UI.MouseTextBrightness * (1f / 255f);
					SpriteBatch.DrawString(UI.BoldSmallFont, ChatLineSet[LineIdx].Text, new Vector2(ChatX, ChatY - (LineIdx + 1) * SpaceMult), new Color((byte)(ChatLineSet[LineIdx].Color.R * Alpha), (byte)(ChatLineSet[LineIdx].Color.G * Alpha), (byte)(ChatLineSet[LineIdx].Color.B * Alpha), UI.MouseTextBrightness));
				}
			}
#else
			int YOffset = 0;
			float LineWidth = 0f;
			int LineHeight = 0;
			int PosX = 48;
			int PosY = 440;

			switch (ScreenHeightPtr)
			{
				case 1:
					PosX = 66;
					PosY = 620;
					break;
				case 2:
					PosX = 98;
					PosY = 980;
					break;
			}

			float ChatX = PosX + 6;
			int ChatY = PosY + 6;

			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
                    LineHeight += (int)ChatLineSet[LineIdx].Size.Y;
                    if (ChatLineSet[LineIdx].Size.X > LineWidth)
                    {
                        LineWidth = ChatLineSet[LineIdx].Size.X;
                    }
                    YOffset++;
                }
			}
			if (YOffset == 0 || LineWidth == 0f)
			{
				return;
			}
			DrawRect(new Rectangle(PosX, PosY - LineHeight, (int)LineWidth + 12, LineHeight + 12), new Color(32, 32, 32, 32));
			float TextAdjust = 0f;
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				if (ChatLineSet[LineIdx].ShowTime > 0)
				{
                    TextAdjust += ChatLineSet[LineIdx].Size.Y;
					float Alpha = UI.MouseTextBrightness * (1f / 255f);
					SpriteBatch.DrawString(UI.BoldSmallFont, ChatLineSet[LineIdx].Text, new Vector2(ChatX, ChatY - TextAdjust), new Color((byte)(ChatLineSet[LineIdx].Color.R * Alpha), (byte)(ChatLineSet[LineIdx].Color.G * Alpha), (byte)(ChatLineSet[LineIdx].Color.B * Alpha), UI.MouseTextBrightness));
				}
			}
#endif
#endif
		}

		private static void UpdateTime()
		{
			bool IsBloodMoon = GameTime.IsBloodMoon;
			if (GameTime.Update())
			{
				WorldGen.ToSpawnNPC = 0;
				NPC.CheckForSpawnsTimer = 0;
				if (GameTime.DayTime)
				{
					Time.CheckXMas();
					if (InvasionDelay > 0)
					{
						InvasionDelay--;
					}
					if (NetMode != (byte)NetModeSetting.CLIENT)
					{
						if (WorldGen.HasShadowOrbSmashed && Rand.Next(NPC.HasDownedGoblins ? 15 : 3) == 0)
						{
							StartInvasion();
						}
						for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
						{
							if (PlayerSet[PlayerIdx].Active != 0)
							{
								PlayerSet[PlayerIdx].SunMoonTransition(IsBloodMoon);
							}
						}
					}
				}
				else if (NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (WorldGen.HasShadowOrbSmashed && Rand.Next(50) == 0)
					{
						WorldGen.ToSpawnMeteor = true;
					}
					if (!NPC.HasDownedBoss1)
					{
						for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
						{
							if (PlayerSet[PlayerIdx].Active == 0 || PlayerSet[PlayerIdx].StatLifeMax < 200 || PlayerSet[PlayerIdx].StatDefense <= 10)
							{
								continue;
							}
							if (Rand.Next(3) != 0)
							{
								break;
							}
							int ActiveTownNPCs = 0;
							for (int NPCIdx = 0; NPCIdx < NPC.MaxNumNPCs; NPCIdx++)
							{
								if (NPCSet[NPCIdx].IsTownNPC && NPCSet[NPCIdx].Active != 0 && ++ActiveTownNPCs >= 4)
								{
									WorldGen.ToSpawnEye = true;
									NetMessage.SendText(9, 50, 255, 130, -1);
									break;
								}
							}
							break;
						}
					}
					if (!WorldGen.ToSpawnEye && GameTime.MoonPhase != 4 && Rand.Next(9) == 0)
					{
						for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
						{
							if (PlayerSet[PlayerIdx].Active != 0 && PlayerSet[PlayerIdx].StatLifeMax > 120)
							{
								GameTime.IsBloodMoon = true;
								NetMessage.SendText(8, 50, 255, 130, -1);
								break;
							}
						}
					}
					for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
					{
						if (PlayerSet[PlayerIdx].Active != 0)
						{
							PlayerSet[PlayerIdx].SunMoonTransition(WasBloodMoon: false);
						}
					}
				}
				if (NetMode == (byte)NetModeSetting.SERVER)
				{
					NetMessage.CreateMessage0(7);
					NetMessage.SendMessage();
				}
			}
			else if (GameTime.DayTime)
			{
				if (NetMode != (byte)NetModeSetting.CLIENT)
				{
					NPC.CheckForTownSpawns();
				}
			}
			else if (GameTime.WorldTime > (Time.NightLength / 2))
			{
				if (WorldGen.ToSpawnMeteor)
				{
					WorldGen.ToSpawnMeteor = false;
					WorldGen.DropMeteor();
				}
			}
			else
			{
				if (!(GameTime.WorldTime > 4860f) || !WorldGen.ToSpawnEye || NetMode == (byte)NetModeSetting.CLIENT)
				{
					return;
				}
				for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
				{
					Player CurrentPlayer = PlayerSet[PlayerIdx];
					if (CurrentPlayer.Active != 0 && !CurrentPlayer.IsDead && CurrentPlayer.XYWH.Y < WorldSurfacePixels)
					{
						NPC.SpawnOnPlayer(CurrentPlayer, (int)NPC.ID.EYE_OF_CTHULHU);
						WorldGen.ToSpawnEye = false;
						break;
					}
				}
			}
		}

		public static int DamageVar(int InitialDamage)
		{
			double FinalDamage = InitialDamage * (1.0 + Rand.Next(-15, 16) * 0.01);
			return (int)Math.Round(FinalDamage);
		}

		public static double CalculateDamage(int Damage, int Defense)
		{
			float ActualDamage = Damage - Defense * 0.5f;
			if (ActualDamage < 1f)
			{
				ActualDamage = 1f;
			}
			return (double)ActualDamage;
		}

		public static void PlaySound(int SoundType, int X, int Y, int Style = 1)
		{
			if (SoundVolume == 0f)
			{
				return;
			}
			try
			{
				float Volume = SoundVolume;
				float Pan = 0f;
				bool IsVisible;
				if (UI.NumActiveViews > 1)
				{
					IsVisible = WorldView.AnyViewContains(X, Y);

				}
				else
				{
					Rectangle ScreenArea = default;
					ScreenArea.X = UI.CurrentUI.CurrentView.ScreenPosition.X - ResolutionWidth;
					ScreenArea.Y = UI.CurrentUI.CurrentView.ScreenPosition.Y - ResolutionHeight;
					ScreenArea.Width = ResolutionWidth * 3;
					ScreenArea.Height = ResolutionHeight * 3;
					IsVisible = ScreenArea.Contains(X, Y);
					if (IsVisible)
					{
						Vector2 Area = new Vector2(UI.CurrentUI.CurrentView.ScreenPosition.X + (ResolutionWidth / 2), UI.CurrentUI.CurrentView.ScreenPosition.Y + (ResolutionHeight / 2));
						float XDistance = Math.Abs(X - Area.X);
						float YDistance = Math.Abs(Y - Area.Y);
						float Distance = (float)Math.Sqrt(XDistance * XDistance + YDistance * YDistance);
						Volume = 1f - Distance / (ResolutionWidth * (2f / 3f));

						// Something interesting to note is that these distance calculations with the volume are different compared to the PC version. This leads to some inaccuracies between versions when it comes to sound.
						// One of the most notable instances of this, is the roar of the Dungeon Guardian which ends up being rather quiet, if not inaudible, on Console compared to PC.
						// My test with a sound volume of 100 resulted in the Guardian's roar being close to 0.045. As you can expect, this makes the surprise far more effective since you do not know if they even spawn until its too late.

						if (Volume > 1f)
						{
							Volume = 1f;
						}
						Volume *= SoundVolume;
						if (Volume <= 0f)
						{
							return;
						}
						Pan = (X - Area.X) / (ResolutionWidth * 2);
						if (Pan < -1f)
						{
							Pan = -1f;
						}
						else if (Pan > 1f)
						{
							Pan = 1f;
						}
					}
				}
				if (!IsVisible)
				{
					return;
				}
				switch (SoundType)
				{
				case 0:
				{
					int DigType = Rand.Next(3);
					SoundDig[DigType].Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				}
				case 1:
				{
					int HurtType = Rand.Next(3);
					SoundPlayerHit[HurtType].Play(Volume, Pan);
					break;
				}
				case 2:
				{
					if (Style == 1)
					{
						int ItemType = Rand.Next(3);
						if (ItemType != 0)
						{
							Style = ItemType + 17;
						}
					}
					double NewVolume = Volume;
					double Pitch;
					if (Style == 26 || Style == 35)
					{
						NewVolume *= 0.75f;
						Pitch = HarpNote;
					}
					else
					{
						Pitch = Rand.Next(-6, 7) * 0.01f;
					}
					SoundItem[Style - 1].Play(NewVolume, Pan, Pitch);
					break;
				}
				case 3:
					SoundNPCHit[Style - 1].Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 4:
					if (Style != 10 || !SoundNPCKilled[Style - 1].IsPlaying())
					{
						SoundNPCKilled[Style - 1].Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					}
					break;
				case 5:
					SoundPlayerKilled.Play(Volume, Pan);
					break;
				case 6:
					SoundGrass.Play(Volume, Pan, Rand.Next(-30, 31) * 0.01f);
					break;
				case 7:
					SoundGrab.Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 8:
					SoundDoorOpen.Play(Volume, Pan, Rand.Next(-20, 21) * 0.01f);
					break;
				case 9:
					SoundDoorClosed.Play(Volume, Pan, Rand.Next(-20, 21) * 0.01f);
					break;
				case 13:
					SoundShatter.Play(Volume, Pan);
					break;
				case 14:
				{
					int num11 = Rand.Next(3);
					SoundZombie[num11].Play(Volume * 0.4f, Pan);
					break;
				}
				case 15:
					if (!SoundRoar[Style].IsPlaying())
					{
						SoundRoar[Style].Play(Volume, Pan);
					}
					break;
				case 16:
					SoundDoubleJump.Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 17:
					SoundRun.Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 19:
					SoundSplash[Style].Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 20:
				{
					int FemType = Rand.Next(3);
					SoundFemaleHit[FemType].Play(Volume, Pan);
					break;
				}
				case 21:
				{
					int TinkType = Rand.Next(3);
					SoundTink[TinkType].Play(Volume, Pan);
					break;
				}
				case 22:
					SoundUnlock.Play(Volume, Pan);
					break;
				case 26:
				{
					int ZombieType = Rand.Next(3, 5);
					SoundZombie[ZombieType].Play(Volume * 0.9f, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				}
				case 27:
					SoundPixie.UpdateOrPlay(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					break;
				case 28:
					if (!SoundMech.IsPlaying())
					{
						SoundMech.Play(Volume, Pan, Rand.Next(-10, 11) * 0.01f);
					}
					break;
				}
			}
			catch
			{
			}
		}

		public static void PlaySound(int SoundType)
		{
			if (SoundVolume == 0f)
			{
				return;
			}
			try
			{
				float Volume = SoundVolume;
				switch (SoundType)
				{
				case 1:
				{
					int HurtType = Rand.Next(3);
					SoundPlayerHit[HurtType].Play(Volume);
					break;
				}
				case 7:
					SoundGrab.Play(Volume, 0, Rand.Next(-10, 11) * 0.01f);
					break;
				case 10:
					SoundMenuOpen.Play(Volume);
					break;
				case 11:
					SoundMenuClose.Play(Volume);
					break;
				case 12:
					SoundMenuTick.Play(Volume);
					break;
				case 18:
					SoundCoins.Play(Volume);
					break;
				case 20:
				{
					int FemType = Rand.Next(3);
					SoundFemaleHit[FemType].Play(Volume);
					break;
				}
				case 23:
					SoundDrown.Play(Volume);
					break;
				case 24:
					SoundChat.Play(Volume);
					break;
				case 25:
					SoundMaxMana.Play(Volume);
					break;
				}
			}
			catch
			{
			}
		}

		private static void UpdateTutorial()
		{
			if (UI.MainUI.CurMenuType != MenuType.NONE)
			{
				return;
			}
			if (TutorialInputDelay > 0)
			{
				TutorialInputDelay--;
			}
			Vector2 LeftThumbStick = (TutorialMaskLS ? default : UI.MainUI.PadState.ThumbSticks.Left);
			Vector2 RightThumbStick = (TutorialMaskRS ? default : UI.MainUI.PadState.ThumbSticks.Right);
			float LeftTrigger = (TutorialMaskLT ? 0f : UI.MainUI.PadState.Triggers.Left);
			float RightTrigger = (TutorialMaskRT ? 0f : UI.MainUI.PadState.Triggers.Right);
			Buttons PadButtons = 0;
			if (!TutorialMaskA && UI.MainUI.PadState.IsButtonDown(Buttons.A))
			{
				PadButtons |= Buttons.A;
			}
			if (!TutorialMaskB && UI.MainUI.PadState.IsButtonDown(Buttons.B))
			{
				PadButtons |= Buttons.B;
			}
			if (!TutorialMaskX && UI.MainUI.PadState.IsButtonDown(Buttons.X))
			{
				PadButtons |= Buttons.X;
			}
			if (!TutorialMaskY && UI.MainUI.PadState.IsButtonDown(Buttons.Y))
			{
				PadButtons |= Buttons.Y;
			}
			if (!TutorialMaskBack && UI.MainUI.PadState.IsButtonDown(Buttons.Back))
			{
				PadButtons |= Buttons.Back;
			}
			if (!TutorialMaskLB && UI.MainUI.PadState.IsButtonDown(Buttons.LeftShoulder))
			{
				PadButtons |= Buttons.LeftShoulder;
			}
			if (!TutorialMaskRB && UI.MainUI.PadState.IsButtonDown(Buttons.RightShoulder))
			{
				PadButtons |= Buttons.RightShoulder;
			}
			if (!TutorialMaskLS && UI.MainUI.PadState.IsButtonDown(Buttons.DPadLeft))
			{
				PadButtons |= Buttons.DPadLeft;
			}
			if (!TutorialMaskLS && UI.MainUI.PadState.IsButtonDown(Buttons.DPadRight))
			{
				PadButtons |= Buttons.DPadRight;
			}
			if (!TutorialMaskLS && UI.MainUI.PadState.IsButtonDown(Buttons.DPadUp))
			{
				PadButtons |= Buttons.DPadUp;
			}
			if (!TutorialMaskLS && UI.MainUI.PadState.IsButtonDown(Buttons.DPadDown))
			{
				PadButtons |= Buttons.DPadDown;
			}
			if (!TutorialMaskLS && UI.MainUI.PadState.IsButtonDown(Buttons.LeftStick))
			{
				PadButtons |= Buttons.LeftStick;
			}
			if (!TutorialMaskRSpress && UI.MainUI.PadState.IsButtonDown(Buttons.RightStick))
			{
				PadButtons |= Buttons.RightStick;
			}
			if (UI.MainUI.PadState.IsButtonDown(Buttons.Start))
			{
				PadButtons |= Buttons.Start;
			}
			UI.MainUI.PadState = new GamePadState(LeftThumbStick, RightThumbStick, LeftTrigger, RightTrigger, PadButtons);
			switch (TutorialState)
			{
			case Tutorial.INTRO:
			case Tutorial.MONSTER_INFO_1:
			case Tutorial.POTIONS_1:
			case Tutorial.TORCH_1:
			case Tutorial.INVENTORY_2:
			case Tutorial.MOVEMENT_1:
			case Tutorial.DROP_1:
			case Tutorial.EQUIPSCREEN_1:
			case Tutorial.CHEST_1:
			case Tutorial.CRAFTSCREEN_1:
			case Tutorial.MINED_ORE_1:
			case Tutorial.CURSOR_SWITCH_1:
			case Tutorial.DAY_NIGHT_1:
			case Tutorial.USE_BENCH_1:
			case Tutorial.BACK_WALL_INFO_1:
			case Tutorial.HOUSE_INFO_1:
			case Tutorial.HOUSE_DONE_1:
			case Tutorial.THE_GUIDE_1:
			case Tutorial.HAMMER_1:
			case Tutorial.CONGRATS_1:
				if (TutorialInputDelay == 0)
				{
					NextTutorial();
				}
				break;
			}
			switch (TutorialState)
			{
			case Tutorial.INTRO_2:
			case Tutorial.MONSTER_INFO_2:
			case Tutorial.POTIONS_2:
			case Tutorial.TORCH_2:
			case Tutorial.INVENTORY_3:
			case Tutorial.MOVEMENT_2:
			case Tutorial.DROP_2:
			case Tutorial.EQUIPSCREEN_2:
			case Tutorial.CHEST_2:
			case Tutorial.CRAFTSCREEN_2:
			case Tutorial.MINED_ORE_2:
			case Tutorial.CURSOR_SWITCH_2:
			case Tutorial.DAY_NIGHT_2:
			case Tutorial.USE_BENCH_2:
			case Tutorial.BACK_WALL_INFO_2:
			case Tutorial.HOUSE_INFO_2:
			case Tutorial.HOUSE_DONE_2:
			case Tutorial.THE_GUIDE_2:
			case Tutorial.HAMMER_2:
			case Tutorial.CONGRATS_2:
				if (UI.MainUI.IsButtonTriggered(Buttons.B))
				{
					UI.MainUI.ClearButtonTriggers();
					TutorialMaskB = TutorialVar != 0;
					NextTutorial();
				}
				return;
			}
			Player ActivePlayer = UI.MainUI.ActivePlayer;
			switch (TutorialState)
			{
			case Tutorial.MOVE:
				if (UI.MainUI.PadState.ThumbSticks.Left.LengthSquared() > UI.SquaredDeadZone)
				{
					TutorialVar++;
				}
				if (TutorialVar > 4 && TutorialInputDelay == 0)
				{
					UI.MainUI.AchievementTriggers.SetState(Trigger.FirstTutorialTaskCompleted, State: true);
					NextTutorial();
				}
				break;
			case Tutorial.JUMP:
				if (UI.MainUI.TotalJumps - TutorialVar >= 1)
				{
					NextTutorial();
				}
				break;
			case Tutorial.FALL_DOWN:
				if (ActivePlayer.IsControlDown)
				{
					int TileX = (int)ActivePlayer.Position.X + 10 >> 4;
					int TileY = (int)ActivePlayer.Position.Y + 42 >> 4;
					if (TileSet[TileX, TileY - 1].Type == 19 || TileSet[TileX - 1, TileY - 1].Type == 19 || TileSet[TileX + 1, TileY - 1].Type == 19)
					{
						NextTutorial();
					}
				}
				break;
			case Tutorial.JUMP_OUT:
				if (UI.MainUI.TotalJumps - TutorialVar >= 1)
				{
					int TileX = (int)ActivePlayer.Position.X + 10 >> 4;
					int TileY = (int)ActivePlayer.Position.Y + 42 >> 4;
					if (TileSet[TileX, TileY + 1].Type == 19 || TileSet[TileX - 1, TileY + 1].Type == 19 || TileSet[TileX + 1, TileY + 1].Type == 19)
					{
						NextTutorial();
					}
				}
				break;
			case Tutorial.CURSOR:
				if (UI.MainUI.PadState.ThumbSticks.Right.LengthSquared() > UI.SquaredDeadZone)
				{
					TutorialVar++;
				}
				if (UI.MainUI.IsButtonTriggered(Buttons.RightTrigger))
				{
					TutorialVar2++;
				}
				if (TutorialInputDelay == 0 && TutorialVar != 0 && TutorialVar2 != 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.HOTBAR:
				if (TutorialInputDelay == 0 && ActivePlayer.SelectedItem == 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.SWORD_ATTACK:
				if (UI.MainUI.TotalDeadSlimes > TutorialVar)
				{
					Item.NewItem((int)ActivePlayer.Position.X, (int)ActivePlayer.Position.Y, 1, 1, (int)Item.ID.GEL);
					NextTutorial();
				}
				else if (TutorialVar2 != 0 && --TutorialVar2 == 0)
				{
					TutorialVar2 = 900;
					int SlimeIdx = NPC.NewNPC(ActivePlayer.XYWH.X - (ResolutionWidth / 2), ActivePlayer.XYWH.Y - ResolutionHeight, (int)NPC.ID.SLIME);
					NPCSet[SlimeIdx].SetDefaults("Green Slime");
				}
				break;
			case Tutorial.SELECT_AXE:
				if (TutorialInputDelay == 0 && ActivePlayer.Inventory[ActivePlayer.SelectedItem].AxePower > 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.USE_AXE:
				if (UI.MainUI.TotalChopsTaken > TutorialVar)
				{
					NextTutorial();
				}
				break;
			case Tutorial.INVENTORY:
				if (UI.MainUI.InventoryMode == 1)
				{
					NextTutorial();
				}
				break;
			case Tutorial.EQUIPMENT:
				if (UI.MainUI.ActiveInvSection == UI.InventorySection.EQUIP)
				{
					NextTutorial();
				}
				break;
			case Tutorial.CRAFTING:
				if (UI.MainUI.ActiveInvSection == UI.InventorySection.CRAFTING)
				{
					NextTutorial();
				}
				break;
			case Tutorial.CRAFT_TORCH:
				if (UI.MainUI.TotalTorchesCrafted > TutorialVar)
				{
					NextTutorial();
				}
				break;
			case Tutorial.CRAFT_CATEGORIES:
				if (UI.MainUI.IsButtonTriggered(Buttons.LeftTrigger) || UI.MainUI.IsButtonTriggered(Buttons.RightTrigger))
				{
					TutorialVar++;
				}
				if (TutorialVar != 0 && TutorialInputDelay == 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.CRAFTING_EXIT:
				if (UI.MainUI.InventoryMode == 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.SELECT_PICK:
				if (TutorialInputDelay == 0 && ActivePlayer.Inventory[ActivePlayer.SelectedItem].PickPower > 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.USE_PICK:
				if (UI.MainUI.TotalCopperObtained - TutorialVar >= 55)
				{
					NextTutorial();
				}
				break;
			case Tutorial.WOOD_PLATFORM:
				if (UI.MainUI.TotalWoodPlatformsCrafted - TutorialVar >= MaxNumItemUpdates)
				{
					NextTutorial(2);
				}
				else if (--TutorialVar2 == 0)
				{
					NextTutorial();
				}
				break;
			case Tutorial.WOOD_PLATFORM_TIME_OUT:
				if (UI.MainUI.TotalWoodPlatformsCrafted - TutorialVar >= MaxNumItemUpdates) // Unintentionally supporting 1.2's platform recipe since 2013
				{
					NextTutorial();
				}
				break;
			case Tutorial.SELECT_PLATFORM:
				if (TutorialInputDelay == 0 && ActivePlayer.Inventory[ActivePlayer.SelectedItem].Type == (int)Item.ID.WOOD_PLATFORM)
				{
					NextTutorial();
				}
				break;
			case Tutorial.BUILD_CURSOR:
				if (TutorialInputDelay == 0 && !UI.MainUI.UsingSmartCursor)
				{
					TutorialMaskRSpress = true;
					NextTutorial();
				}
				break;
			case Tutorial.PLACING_1:
				if (UI.MainUI.TotalWoodPlatformsPlaced > TutorialVar)
				{
					NextTutorial();
				}
				break;
			case Tutorial.PLACING_2:
				if (ActivePlayer.XYWH.Y < 3360)
				{
					NextTutorial();
				}
				break;
			case Tutorial.BUILD_HOUSE:
				if (TutorialInputDelay == 0)
				{
					NextTutorial();
				}
				goto case Tutorial.BUILD_HOUSE_EXTRA_INFO;
			case Tutorial.BUILD_HOUSE_EXTRA_INFO:
			{
				if ((FrameCounter & 31u) != 0)
				{
					break;
				}
				int PlaceX = UI.MainUI.MouseX + UI.MainUI.CurrentView.ScreenPosition.X >> 4;
				int PlaceY = UI.MainUI.MouseY + UI.MainUI.CurrentView.ScreenPosition.Y >> 4;
				bool EnoughSpace = WorldGen.StartSpaceCheck(PlaceX, PlaceY);
				if (!EnoughSpace)
				{
					PlaceX--;
					EnoughSpace = WorldGen.StartSpaceCheck(PlaceX, PlaceY);
					if (!EnoughSpace)
					{
						PlaceX += 2;
						EnoughSpace = WorldGen.StartSpaceCheck(PlaceX, PlaceY);
					}
					if (!EnoughSpace)
					{
						PlaceX--;
						PlaceY++;
						EnoughSpace = WorldGen.StartSpaceCheck(PlaceX, PlaceY);
					}
				}
				if (EnoughSpace)
				{
					TutorialHouse.X = (short)PlaceX;
					TutorialHouse.Y = (short)PlaceY;
					NextTutorial((TutorialState != Tutorial.BUILD_HOUSE) ? 1 : 2);
				}
				break;
			}
			case Tutorial.BUILD_HOUSE_2:
				if (TutorialInputDelay == 0)
				{
					NextTutorial();
				}
				goto case Tutorial.BUILD_HOUSE_2_EXTRA_INFO;
			case Tutorial.BUILD_HOUSE_2_EXTRA_INFO:
				if ((FrameCounter & 31) == 0 && (UI.MainUI.TotalWoodAxed - TutorialVar >= 3 || UI.MainUI.TotalOrePicked - TutorialVar2 >= 3) && !WorldGen.StartSpaceCheck(TutorialHouse.X, TutorialHouse.Y))
				{
					NextTutorial((TutorialState != Tutorial.BUILD_HOUSE_2) ? 1 : 2);
				}
				break;
			case Tutorial.CRAFT_WORKBENCH:
			case Tutorial.CRAFT_WORKBENCH_EXTRA_INFO:
			{
				if ((FrameCounter & 31u) != 0)
				{
					break;
				}
				int PlayerX = (int)ActivePlayer.Position.X + 10 >> 4;
				int PlayerY = ((int)ActivePlayer.Position.Y + 42 >> 4) - 1;
				for (int TileX = PlayerX - 5; TileX < PlayerX + 5; TileX++)
				{
					for (int TileY = PlayerY - 5; TileY < PlayerY + 5; TileY++)
					{
						if (TileSet[TileX, TileY].Type == 18)
						{
							NextTutorial((TutorialState != Tutorial.CRAFT_WORKBENCH) ? 1 : 2);
							return;
						}
					}
				}
				break;
			}
			case Tutorial.CRAFT_DOOR:
			case Tutorial.CRAFT_DOOR_EXTRA_INFO:
				if (TutorialInputDelay == 0 && ActivePlayer.HasItemInInventory((int)Item.ID.WOODEN_DOOR))
				{
					NextTutorial((TutorialState != Tutorial.CRAFT_DOOR) ? 1 : 2);
				}
				break;
			case Tutorial.PLACE_DOOR:
				if ((FrameCounter & 31) == 0 && WorldGen.StartSpaceCheck(TutorialHouse.X, TutorialHouse.Y) && (WorldGen.HouseTile[10] || WorldGen.HouseTile[11]))
				{
					NextTutorial();
				}
				break;
			case Tutorial.USE_DOOR:
				if (TutorialInputDelay == 0 && (UI.MainUI.TotalDoorsOpened > TutorialVar || UI.MainUI.TotalDoorsClosed > TutorialVar2))
				{
					NextTutorial();
				}
				break;
			case Tutorial.CRAFT_WALL:
			case Tutorial.CRAFT_WALL_EXTRA_INFO:
				if (TutorialInputDelay == 0 && UI.MainUI.TotalWallsCrafted - TutorialVar >= 32)
				{
					NextTutorial((TutorialState != Tutorial.CRAFT_WALL) ? 1 : 2);
				}
				break;
			case Tutorial.PLACE_WALL:
				if (TutorialInputDelay == 0 && UI.MainUI.TotalWallsPlaced - TutorialVar >= 8)
				{
					NextTutorial();
				}
				break;
			case Tutorial.BACK_WALL:
				if ((FrameCounter & 31) == 0 && WorldGen.StartRoomCheck(TutorialHouse.X, TutorialHouse.Y))
				{
					NextTutorial();
				}
				break;
			case Tutorial.PLACE_CHAIR:
				if ((FrameCounter & 31) == 0 && WorldGen.StartRoomCheck(TutorialHouse.X, TutorialHouse.Y))
				{
					WorldGen.RoomNeeds();
					if (WorldGen.roomChair)
					{
						NextTutorial();
					}
				}
				break;
			case Tutorial.PLACE_TORCH:
				if ((FrameCounter & 31) == 0 && WorldGen.StartRoomCheck(TutorialHouse.X, TutorialHouse.Y) && WorldGen.RoomNeeds())
				{
					NextTutorial();
				}
				break;
			case Tutorial.MONSTER_INFO_1:
			case Tutorial.MONSTER_INFO_2:
			case Tutorial.POTIONS_1:
			case Tutorial.POTIONS_2:
			case Tutorial.TORCH_1:
			case Tutorial.TORCH_2:
			case Tutorial.INVENTORY_2:
			case Tutorial.INVENTORY_3:
			case Tutorial.MOVEMENT_1:
			case Tutorial.MOVEMENT_2:
			case Tutorial.DROP_1:
			case Tutorial.DROP_2:
			case Tutorial.EQUIPSCREEN_1:
			case Tutorial.EQUIPSCREEN_2:
			case Tutorial.CHEST_1:
			case Tutorial.CHEST_2:
			case Tutorial.CRAFTSCREEN_1:
			case Tutorial.CRAFTSCREEN_2:
			case Tutorial.MINED_ORE_1:
			case Tutorial.MINED_ORE_2:
			case Tutorial.CURSOR_SWITCH_1:
			case Tutorial.CURSOR_SWITCH_2:
			case Tutorial.DAY_NIGHT_1:
			case Tutorial.DAY_NIGHT_2:
			case Tutorial.USE_BENCH_1:
			case Tutorial.USE_BENCH_2:
			case Tutorial.BACK_WALL_INFO_1:
			case Tutorial.BACK_WALL_INFO_2:
			case Tutorial.HOUSE_INFO_1:
			case Tutorial.HOUSE_INFO_2:
				break;
			}
		}

		private static void NextTutorial(int Increment = 1)
		{
			if (TutorialState < Tutorial.THE_END)
			{
				SetTutorial(TutorialState + Increment);
			}
		}

		public static void SetTutorial(Tutorial State)
		{
			TutorialState = State;
			if (State == Tutorial.NUM_TUTORIALS)
			{
				TutorialMaskLS = false;
				TutorialMaskRS = false;
				TutorialMaskRSpress = false;
				TutorialMaskA = false;
				TutorialMaskB = false;
				TutorialMaskX = false;
				TutorialMaskY = false;
				TutorialMaskLB = false;
				TutorialMaskRB = false;
				TutorialMaskLT = false;
				TutorialMaskRT = false;
				TutorialMaskBack = false;
				return;
			}
			TutorialInputDelay = 180;
			string TutText = Lang.TutorialLocale(State);
			Player ActivePlayer = UI.MainUI.ActivePlayer;
			switch (State)
			{
			case Tutorial.INTRO:
				TutorialMaskLS = true;
				TutorialMaskRS = true;
				TutorialMaskRSpress = true;
				TutorialMaskA = true;
				TutorialMaskB = true;
				TutorialMaskX = true;
				TutorialMaskY = true;
				TutorialMaskLB = true;
				TutorialMaskRB = true;
				TutorialMaskLT = true;
				TutorialMaskRT = true;
				TutorialMaskBack = true;
				UI.MainUI.UsingSmartCursor = true;
				break;
			case Tutorial.INTRO_2:
			case Tutorial.MONSTER_INFO_2:
			case Tutorial.POTIONS_2:
			case Tutorial.TORCH_2:
			case Tutorial.INVENTORY_3:
			case Tutorial.MOVEMENT_2:
			case Tutorial.DROP_2:
			case Tutorial.EQUIPSCREEN_2:
			case Tutorial.CHEST_2:
			case Tutorial.CRAFTSCREEN_2:
			case Tutorial.MINED_ORE_2:
			case Tutorial.CURSOR_SWITCH_2:
			case Tutorial.DAY_NIGHT_2:
			case Tutorial.USE_BENCH_2:
			case Tutorial.BACK_WALL_INFO_2:
			case Tutorial.HOUSE_INFO_2:
			case Tutorial.HOUSE_DONE_2:
			case Tutorial.THE_GUIDE_2:
			case Tutorial.HAMMER_2:
			case Tutorial.CONGRATS_2:
				TutText = Lang.TutorialLocale(State - 1) + TutText;
				TutorialVar = (TutorialMaskB ? 1u : 0u);
				TutorialMaskB = false;
				break;
			case Tutorial.MOVE:
				TutorialMaskLS = false;
				TutorialVar = 0;
				break;
			case Tutorial.JUMP:
				TutorialMaskA = false;
				TutorialVar = UI.MainUI.TotalJumps;
				break;
			case Tutorial.JUMP_OUT:
				TutorialVar = UI.MainUI.TotalJumps;
				break;
			case Tutorial.CURSOR:
				TutorialMaskRT = false;
				TutorialMaskRS = false;
				TutorialVar = 0;
				TutorialVar2 = 0;
				break;
			case Tutorial.HOTBAR:
				TutorialMaskLB = false;
				TutorialMaskRB = false;
				break;
			case Tutorial.SWORD_ATTACK:
				TutorialVar = UI.MainUI.TotalDeadSlimes;
				TutorialVar2 = 120;
				break;
			case Tutorial.MONSTER_INFO_1:
			{
				for (int NPCIdx = NPC.MaxNumNPCs - 1; NPCIdx >= 0; NPCIdx--)
				{
					if (NPCSet[NPCIdx].Type == (int)NPC.ID.SLIME && NPCSet[NPCIdx].Active != 0)
					{
						NPCSet[NPCIdx].HitEffect(0, 999);
						NPCSet[NPCIdx].Active = 0;
					}
				}
				break;
			}
			case Tutorial.USE_AXE:
				TutorialVar = UI.MainUI.TotalChopsTaken;
				break;
			case Tutorial.INVENTORY:
				TutorialMaskY = false;
				TutorialMaskB = true;
				break;
			case Tutorial.INVENTORY_2:
				TutorialMaskLB = true;
				TutorialMaskRB = true;
				break;
			case Tutorial.EQUIPMENT:
				TutorialMaskRB = false;
				TutorialMaskLB = false;
				TutorialMaskRT = false;
				TutorialMaskLT = false;
				break;
			case Tutorial.CRAFT_TORCH:
				TutorialMaskX = false;
				TutorialVar = UI.MainUI.TotalTorchesCrafted;
				break;
			case Tutorial.CRAFT_CATEGORIES:
				TutorialVar = 0;
				break;
			case Tutorial.CRAFTING_EXIT:
				TutorialMaskB = false;
				break;
			case Tutorial.USE_PICK:
				TutorialVar = UI.MainUI.TotalCopperObtained;
				break;
			case Tutorial.WOOD_PLATFORM:
				TutorialVar = UI.MainUI.TotalWoodPlatformsCrafted;
#if VERSION_103 || VERSION_FINAL
				TutorialVar2 = 720; // Altered bc platform recipe
#else
				TutorialVar2 = 600;
#endif
				break;
			case Tutorial.BUILD_CURSOR:
				TutorialMaskRSpress = false;
				break;
			case Tutorial.PLACING_1:
				TutorialVar = UI.MainUI.TotalWoodPlatformsPlaced;
				break;
			case Tutorial.PLACING_2:
				TutText = Lang.TutorialLocale(State - 1) + TutText;
				break;
			case Tutorial.CURSOR_SWITCH_1:
				TutorialMaskRSpress = false;
				break;
			case Tutorial.BUILD_HOUSE:
				TutorialInputDelay = 1800;
				break;
			case Tutorial.BUILD_HOUSE_2:
				TutorialVar = UI.MainUI.TotalWoodAxed;
				TutorialVar2 = UI.MainUI.TotalOrePicked;
				TutorialInputDelay = 600;
				break;
			case Tutorial.BUILD_HOUSE_EXTRA_INFO:
			case Tutorial.BUILD_HOUSE_2_EXTRA_INFO:
				TutText = Lang.TutorialLocale(State - 1) + TutText;
				break;
			case Tutorial.CRAFT_WORKBENCH:
				if (ActivePlayer.CountInventory((int)Item.ID.WOOD) < 10)
				{
					TutorialState = State + 1;
					TutText += Lang.TutorialLocale(TutorialState);
				}
				break;
			case Tutorial.CRAFT_DOOR:
				if (ActivePlayer.CountInventory((int)Item.ID.WOOD) < 6)
				{
					TutorialState = State + 1;
					TutText += Lang.TutorialLocale(TutorialState);
				}
				break;
			case Tutorial.USE_DOOR:
				TutorialVar = UI.MainUI.TotalDoorsOpened;
				TutorialVar2 = UI.MainUI.TotalDoorsClosed;
				break;
			case Tutorial.CRAFT_WALL:
				TutorialVar = UI.MainUI.TotalWallsCrafted;
				if (ActivePlayer.CountInventory((int)Item.ID.WOOD) < 6)
				{
					TutorialState = State + 1;
					TutText += Lang.TutorialLocale(TutorialState);
				}
				break;
			case Tutorial.PLACE_WALL:
				TutorialVar = UI.MainUI.TotalWallsPlaced;
				break;
			case Tutorial.THE_END:
				GameTime.DayRate = 1f;
				UI.MainUI.AchievementTriggers.SetState(Trigger.AllTutorialTasksCompleted, State: true);
				break;
			}
			if (TutText != null)
			{
#if USE_ORIGINAL_CODE
				TutorialText = new CompiledText(TutText, 470, UI.BoldSmallTextStyle);
#else
				int WrapWidth = 470;
				switch (ScreenHeightPtr)
				{
					case 1:
						WrapWidth = 548;
						break;
					case 2:
						WrapWidth = 705;
						break;
				}
				TutorialText = new CompiledText(TutText, WrapWidth, UI.BoldSmallTextStyle);
#endif
			}
			else
			{
				TutorialText = null;
			}
		}

		public static bool IsTutorial()
		{
			return TutorialState != Tutorial.NUM_TUTORIALS;
		}

		public static void StartTutorial()
		{
			Player NewPlayer = new Player();
			NewPlayer.Name = UI.MainUI.SignedInGamer.Gamertag; // All references to the .Gamertag property being used for .name are removed in 1.01+, but this leaves the death message blank in the tutorial.
			// I will be changing this up so the Gamertag remains in all versions for non-CharacterName uses.
			NewPlayer.SelectedItem = 1;
			UI.MainUI.CreateCharacterGUI.Randomize(NewPlayer);
			UI.MainUI.SetPlayer(NewPlayer);
			SetTutorial(Tutorial.INTRO);
			WorldGen.PlayWorld();
		}

		public static void StartGame()
		{
			UI MainUI = UI.MainUI;
			PlaySound(11);
			for (int LineIdx = 0; LineIdx < MaxNumChatLines; LineIdx++)
			{
				ChatLineSet[LineIdx].Init();
			}
			for (int PlayerIdx = 0; PlayerIdx < Player.MaxNumPlayers; PlayerIdx++)
			{
				if (PlayerIdx != MainUI.MyPlayer)
				{
					PlayerSet[PlayerIdx].Active = 0;
				}
				PlayerSet[PlayerIdx].IsAnnounced = false;
			}
#if USE_ORIGINAL_CODE
			if (IsTutorial())
			{
				UI.MainUI.SignedInGamer.Presence.SetPresenceModeString("Tutorial");
			}
			else if (MainUI.IsOnline)
			{
				NetMode = (byte)NetModeSetting.SERVER;
				MainUI.SignedInGamer.Presence.SetPresenceModeString("Online");
			}
			else
			{
				MainUI.SignedInGamer.Presence.SetPresenceModeString("Offline");
			}
#else
			if (IsTutorial())
            {
                UI.MainUI.SignedInGamer.Presence.SetPresenceModeStringEXT("Tutorial");
            }
            else if (MainUI.IsOnline)
            {
                NetMode = (byte)NetModeSetting.SERVER;
                MainUI.SignedInGamer.Presence.SetPresenceModeStringEXT("Online");
            }
            else
            {
                MainUI.SignedInGamer.Presence.SetPresenceModeStringEXT("Offline");
            }
#endif
            Netplay.StartServer();
			MusicBox = -1;
			GameTime = WorldGen.tempTime;
			MainUI.InitGame();
			Netplay.SessionReadyEvent.WaitOne(); 
			MainUI.ActivePlayer.Spawn();
			MainUI.CurMenuType = MenuType.NONE;
			MainUI.CurrentView.OnStartGame();
			MiniMap.OnStartGame();
			GC.Collect();
			IsGameStarted = true;
		}

		public static void JoinGame(UI StartUI)
		{
#if USE_ORIGINAL_CODE
			StartUI.SignedInGamer.Presence.SetPresenceModeString((NetMode == (byte)NetModeSetting.LOCAL) ? "Offline" : "Online");
#else
			StartUI.SignedInGamer.Presence.SetPresenceModeStringEXT((NetMode == (byte)NetModeSetting.LOCAL) ? "Offline" : "Online");
#endif
			PlaySound(11);
			StartUI.InitGame();
			StartUI.ActivePlayer.Spawn();
			if (NetMode == (byte)NetModeSetting.SERVER)
			{
				NetMessage.SyncPlayer(StartUI.MyPlayer);
			}
			StartUI.CurMenuType = MenuType.NONE;
			StartUI.CurrentView.OnStartGame();
			MiniMap.OnStartGame();
			IsGameStarted = true;
		}

		public static void DrawSolidRect(Rectangle DestRect, Color Colour)
		{
			SpriteBatch.Draw(WhiteTexture, DestRect, Colour);
		}

		public static void DrawSolidRect(ref Rectangle DestRect, Color Colour)
		{
			SpriteBatch.Draw(WhiteTexture, DestRect, Colour);
		}

#if USE_ORIGINAL_CODE
		public static void DrawRect(int TexId, Rectangle DestRect, int Alpha, int Shift = 0)
		{
			Rectangle Source = default;
			Rectangle Dest = DestRect;
			Vector2 Position = default;
			Color Colour = new Color(Alpha >> Shift, Alpha >> Shift, Alpha >> Shift, Alpha);
			Source.X = (Source.Y = 8);
			Source.Width = (Source.Height = 36);
			Position.X = DestRect.X - 8;
			Position.Y = DestRect.Y - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 9;
			Source.Y = 0;
			Source.Width = 34;
			Source.Height = 8;
			Dest.Y -= 8;
			Dest.Height = 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Y = 44;
			Dest.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Width = 8;
			Source.Y = 0;
			Source.X = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Y = 44;
			Position.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.X = DestRect.X + DestRect.Width;
			Source.X = 44;
			Source.Y = 44;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.Y = DestRect.Y - 8;
			Source.Y = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Height = 34;
			Source.Y = 9;
			Dest.Width = 8;
			Dest.Height = DestRect.Height;
			Dest.X += DestRect.Width;
			Dest.Y = DestRect.Y;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 0;
			Dest.X = DestRect.X - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
		}
#else
		public static void DrawRect(int TexId, Rectangle DestRect, Color Colour)
		{
			Rectangle Source = default;
			Vector2 Position = default;
			Rectangle Dest = DestRect;
			Source.X = (Source.Y = 8);
			Source.Width = (Source.Height = 36);
			Position.X = DestRect.X - 8;
			Position.Y = DestRect.Y - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 9;
			Source.Y = 0;
			Source.Width = 34;
			Source.Height = 8;
			Dest.Y -= 8;
			Dest.Height = 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Y = 44;
			Dest.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Width = 8;
			Source.Y = 0;
			Source.X = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Y = 44;
			Position.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.X = DestRect.X + DestRect.Width;
			Source.X = 44;
			Source.Y = 44;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.Y = DestRect.Y - 8;
			Source.Y = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Height = 34;
			Source.Y = 9;
			Dest.Width = 8;
			Dest.Height = DestRect.Height;
			Dest.X += DestRect.Width;
			Dest.Y = DestRect.Y;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 0;
			Dest.X = DestRect.X - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
		}

		public static void DrawRect(int TexId, Rectangle DestRect, int Alpha, int Shift = 0)
		{
			Color Colour = new Color(Alpha >> Shift, Alpha >> Shift, Alpha >> Shift, Alpha);
			DrawRect(TexId, DestRect, Colour);
		}
		// This is a custom DrawRect which replaces the instances where chatBackTexture is drawn as that texture was rendered obsolete in the 1.2 versions.
		public static void DrawRect(int TexId, Vector2 Position, Rectangle Source, Color Colour)
		{
			Rectangle DestRect = new Rectangle((int)Position.X, (int)Position.Y, Source.Width, Source.Height);
			DrawRect(TexId, DestRect, Colour);
		}
#endif
		public static void DrawRectStraightBottom(int TexId, Rectangle DestRect, int Alpha, int Shift = 0)
		{
			Rectangle Source = default;
			Rectangle Dest = DestRect;
			Vector2 Position = default;
			Color Colour = new Color(Alpha >> Shift, Alpha >> Shift, Alpha >> Shift, Alpha);
			Source.X = (Source.Y = 8);
			Source.Width = (Source.Height = 36);
			Position.X = DestRect.X - 8;
			Position.Y = DestRect.Y - 8;
			Dest.Height += 7;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 9;
			Source.Y = 0;
			Source.Width = 34;
			Source.Height = 8;
			Dest.Y -= 8;
			Dest.Height = 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Y = 51;
			Source.Height = 1;
			Dest.X -= 7;
			Dest.Y = DestRect.Y + DestRect.Height + 7;
			Dest.Width += 14;
			Dest.Height = 1;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Width = 8;
			Source.Height = 8;
			Source.Y = 0;
			Source.X = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.X = DestRect.X + DestRect.Width;
			Position.Y = DestRect.Y - 8;
			Source.X = 44;
			Source.Y = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Height = 34;
			Source.Y = 9;
			Dest.Width = 8;
			Dest.Height = DestRect.Height + 8;
			Dest.X = DestRect.X + DestRect.Width;
			Dest.Y = DestRect.Y;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 0;
			Dest.X = DestRect.X - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
		}

		public static void DrawRectOpenAtBottom(int TexId, Rectangle DestRect, int Alpha, int Shift = 0)
		{
			Rectangle Source = default;
			Rectangle Dest = DestRect;
			Vector2 Position = default;
			Color Colour = new Color(Alpha >> Shift, Alpha >> Shift, Alpha >> Shift, Alpha);
			Source.X = (Source.Y = 8);
			Source.Width = 36;
			Source.Height = 36;
			Dest.Height += 8;
			Position.X = DestRect.X - 8;
			Position.Y = DestRect.Y - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 9;
			Source.Y = 0;
			Source.Width = 34;
			Source.Height = 8;
			Dest.Y -= 8;
			Dest.Height = 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Width = 8;
			Source.Y = 0;
			Source.X = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.X = DestRect.X + DestRect.Width;
			Position.Y = DestRect.Y - 8;
			Source.X = 44;
			Source.Y = 0;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Height = 34;
			Source.Y = 9;
			Dest.Width = 8;
			Dest.Height = DestRect.Height + 8;
			Dest.X += DestRect.Width;
			Dest.Y = DestRect.Y;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 0;
			Dest.X = DestRect.X - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
		}

		public static void DrawRectOpenAtTop(int TexId, Rectangle DestRect, int Alpha, int Shift = 0)
		{
			Rectangle Source = default;
			Rectangle Dest = DestRect;
			Vector2 Position = default;
			Color Colour = new Color(Alpha >> Shift, Alpha >> Shift, Alpha >> Shift, Alpha);
			Source.X = (Source.Y = 8);
			Source.Width = (Source.Height = 36);
			Position.X = DestRect.X - 8;
			Position.Y = DestRect.Y - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 9;
			Source.Width = 34;
			Source.Height = 8;
			Dest.Height = 8;
			Source.Y = 44;
			Dest.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.Width = 8;
			Source.X = 0;
			Source.Y = 44;
			Position.Y = DestRect.Y + DestRect.Height;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Position.X = DestRect.X + DestRect.Width;
			Source.X = 44;
			Source.Y = 44;
			SpriteSheet<_sheetSprites>.Draw(TexId, ref Position, ref Source, Colour);
			Source.Height = 34;
			Source.Y = 9;
			Dest.Width = 8;
			Dest.Height = DestRect.Height;
			Dest.X += DestRect.Width;
			Dest.Y = DestRect.Y;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
			Source.X = 0;
			Dest.X = DestRect.X - 8;
			SpriteSheet<_sheetSprites>.DrawStretched(TexId, Source, ref Dest, Colour);
		}

		public static void DrawRect(Rectangle DestRect, Color Colour, bool ToCenter = true)
		{
			DestRect.X += 2;
			DestRect.Y += 2;
			DestRect.Width -= 4;
			DestRect.Height -= 4;
			if (ToCenter)
			{
				SpriteBatch.Draw(WhiteTexture, DestRect, Colour);
				Colour.A >>= 3;
			}
			Rectangle Dest = DestRect;
			Dest.Y -= 2;
			Dest.Height = 2;
			SpriteBatch.Draw(WhiteTexture, Dest, Colour);
			Dest.X -= 2;
			Dest.Y += 2;
			Dest.Height = DestRect.Height;
			Dest.Width = 2;
			SpriteBatch.Draw(WhiteTexture, Dest, Colour);
			Dest.X += DestRect.Width + 2;
			SpriteBatch.Draw(WhiteTexture, Dest, Colour);
			Dest.X = DestRect.X;
			Dest.Y += DestRect.Height;
			Dest.Width = DestRect.Width;
			Dest.Height = 2;
			SpriteBatch.Draw(WhiteTexture, Dest, Colour);
		}

		public static void ShowSaveIcon()
		{
			if (SaveIconCounter <= 0)
			{
				SaveIconCounter = SaveIconMinTime;
			}
			ActiveSaves++;
		}

		public static void HideSaveIcon()
		{
			ActiveSaves--;
		}

		public static bool IsSaveIconVisible()
		{
			if (SaveIconCounter <= 0)
			{
				return ActiveSaves > 0;
			}
			return true;
		}

		private void Quit()
		{
			Netplay.PlayDisconnect = true;
			Exit();
		}

        private static void SignedInGamer_SignedIn(object Sender, SignedInEventArgs Event)
		{
			IsTrial = Guide.IsTrialMode;
			CheckUserGeneratedContent = true;
		}

		private static void SignedInGamer_SignedOut(object Sender, SignedOutEventArgs Event)
		{
			SignedInGamer SIGamer = Event.Gamer;
			PlayerIndex PlayerIdx = SIGamer.PlayerIndex;
			UI Instance = UIInstance[(int)PlayerIdx];
			Instance.SignOut();
		}
	}
}
