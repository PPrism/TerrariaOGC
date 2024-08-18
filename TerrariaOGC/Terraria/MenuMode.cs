namespace Terraria
{
	public enum MenuMode : byte
	{
		NONE,
		PAUSE,
		BLACKLIST,
		BLACKLIST_REMOVE,
		MAP,
		WELCOME,
		TITLE,
		CHARACTER_SELECT,
		CREATE_CHARACTER,
		CONFIRM_LEAVE_CREATE_CHARACTER,
		NAME_CHARACTER,
		CONFIRM_DELETE_CHARACTER,
		WORLD_SELECT,
		GAME_MODE,
		WORLD_SIZE,
		NAME_WORLD,
		CONFIRM_DELETE_WORLD,
		STATUS_SCREEN,
		WAITING_SCREEN,
		WAITING_FOR_PLAYER_ID,
		WAITING_FOR_PUBLIC_SLOT,
		OPTIONS,
		CONTROLS,
		SETTINGS,
		VOLUME,
		NETPLAY,
		ERROR,
		LOAD_FAILED_NO_BACKUP,
		LEADERBOARDS,
		HOW_TO_PLAY,
		SHOW_SIGN_IN,
		SIGN_IN,
		SIGN_IN_FAILED,
		CREDITS,
		EXIT,
		EXIT_UGC_BLOCKED,
		QUIT,
		UPSELL,
#if !USE_ORIGINAL_CODE
		ACHIEVEMENTS,
		HARDMODE_UPSELL,
#endif
		NUM_MENUS
	}
}