namespace Terraria.Leaderboards
{
	public enum Column
	{
		// There exist some unused entries, but we know what these would be used for thanks to v1.01 and higher. Unused sprite entries exist but they just take the form of clothing sprites.
		AIR_COLUMN,
		GROUND_COLUMN,
		WATER_COLUMN,
		LAVA_COLUMN,
		ORE_COLUMN,
		GEMS_COLUMN,
		SOILS_COLUMN,
		WOOD_COLUMN,
		FURNITURE_COLUMN,
		TOOLS_COLUMN,
		WEAPONS_COLUMN,
		ARMOR_COLUMN,
		CONSUMABLES_COLUMN,
		MISC_COLUMN,
		POTIONS_COLUMN, // This and the 4 below are unused; This one was the number of potions consumed.
		LIGHTS_COLUMN, // This one, torches placed. Would most likely include normal, colored, demon, and cursed varieties of torches.
		AMMO_COLUMN, // This, ammo fired. Would most likely include bullet types, arrow types, sand, gel, stars, and seeds.
		SEEDS_COLUMN, // This one would be for how many ingredient seeds were planted.
		KEYS_COLUMN, // This one was for the number of keys used, which is odd considering it could only be golden keys can be used up. Since shadow keys are reusable, it would make sense this was for the number of chests unlocked.
		KING_SLIME_COLUMN,
		CTHULHU_COLUMN,
		EATER_COLUMN,
		SKELETRON_COLUMN,
		WALL_COLUMN,
		TWINS_COLUMN,
		SKELETRON_PRIME_COLUMN,
		DESTROYER_COLUMN,
		OCRAM_COLUMN,
		DROWNED_COLUMN, // This and the 3 below are unused; These would be for how many times a player had uniquely committed suicide and how. This one, self-explanatory.
		FALLEN_COLUMN, // This, fall damage.
		BURIED_COLUMN, // This one would be sand or silt.
		BURNED_COLUMN, // Despite saying burned, it would be for lava deaths.
		COLUMN_COUNT
	}
}
