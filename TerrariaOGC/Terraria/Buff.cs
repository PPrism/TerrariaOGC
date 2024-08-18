namespace Terraria
{
	public struct Buff
	{
		public enum ID
		{
			NONE,
			LAVA_IMMUNE,
			LIFE_REGEN,
			HASTE, // Haste? It's not even like the Minecraft haste potion, it's just a speed boost... which is haste, yes I know, but still.
			GILLS,
			IRONSKIN,
			MANA_REGEN,
			MAGIC_POWER,
			SLOWFALL,
			FIND_TREASURE,
			INVISIBLE,
			SHINE,
			NIGHTVISION,
			ENEMY_SPAWNS,
			THORNS,
			WATER_WALK,
			RANGED_DAMAGE,
			DETECT_CREATURE,
			GRAVITY_CONTROL,
			LIGHT_ORB,
			POISONED,
			POTION_DELAY,
			BLIND,
			NO_ITEMS,
			ON_FIRE,
			DRUNK,
			WELL_FED,
			FAIRY,
			WEREWOLF,
			CLARAVOYANCE, // Ah yes, Claravoyance.
			BLEED,
			CONFUSED,
			SLOW,
			WEAK,
			MERFOLK,
			SILENCE,
			BROKEN_ARMOR,
			HORRIFIED,
			TONGUED,
			ON_FIRE_2, // Cursed Inferno.
			PET,
			NUM_TYPES
		}

		public const int MaxNumBuffs = (int)ID.NUM_TYPES;

		public const int MaxNumBuffStrings = 46;

		public static string[] BuffName = new string[MaxNumBuffStrings];

		public static string[] BuffTip = new string[MaxNumBuffStrings];

		public ushort Type;

		public ushort Time;

		public bool IsPvpBuff()
		{
			switch (Type)
			{
			case (int)ID.POISONED:
			case (int)ID.ON_FIRE:
			case (int)ID.BLEED:
			case (int)ID.CONFUSED:
			case (int)ID.ON_FIRE_2:
				return true;
			default:
				return false;
			}
		}

		public static bool IsDebuff(int type)
		{
			switch (type)
			{
			case (int)ID.POISONED:
			case (int)ID.POTION_DELAY:
			case (int)ID.BLIND:
			case (int)ID.NO_ITEMS:
			case (int)ID.ON_FIRE:
			case (int)ID.DRUNK:
			case (int)ID.WEREWOLF: // Apparently this and merfolk are debuffs???
			case (int)ID.BLEED:
			case (int)ID.CONFUSED:
			case (int)ID.SLOW:
			case (int)ID.WEAK:
			case (int)ID.MERFOLK:
			case (int)ID.SILENCE:
			case (int)ID.BROKEN_ARMOR:
			case (int)ID.HORRIFIED:
			case (int)ID.TONGUED:
			case (int)ID.ON_FIRE_2:
				return true;
			default:
				return false;
			}
		}

		public bool IsDebuff()
		{
			return IsDebuff(Type);
		}

		public bool IsHealable()
		{
			switch (Type)
			{
			case (int)ID.POISONED:
			case (int)ID.POTION_DELAY:
			case (int)ID.BLIND:
			case (int)ID.NO_ITEMS:
			case (int)ID.ON_FIRE:
			case (int)ID.DRUNK:
			case (int)ID.BLEED:
			case (int)ID.CONFUSED:
			case (int)ID.SLOW:
			case (int)ID.WEAK:
			case (int)ID.SILENCE:
			case (int)ID.BROKEN_ARMOR:
			case (int)ID.HORRIFIED:
			case (int)ID.TONGUED:
			case (int)ID.ON_FIRE_2:
				return Time > 0;
			default:
				return false;
			}
		}

		public void Init()
		{
			Type = 0;
			Time = 0;
		}
	}
}
