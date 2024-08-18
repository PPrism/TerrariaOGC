using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

public class _sheetTiles : SpriteSheet<_sheetTiles>
{
	public enum ID
	{
		BACKGROUND_0,
		BACKGROUND_1,
		BACKGROUND_2,
		BACKGROUND_3,
		BACKGROUND_4,
		BACKGROUND_5,
		BACKGROUND_6,
		BLACK_TILE,
		CLOUD_0,
		CLOUD_1,
		CLOUD_2,
		CLOUD_3,
		EVIL_CACTUS,
		GOOD_CACTUS,
		LIQUID_0,
		LIQUID_1,
		MOON,
		MUSIC_BOX,
		SHROOM_TOPS,
		STAR_0,
		STAR_1,
		STAR_2,
		STAR_3,
		STAR_4,
#if VERSION_INITIAL
		SUN2,
		SUN,
#else
		SUN,
		SUN2,
#endif
		TILES_0,
		TILES_1,
		TILES_2,
		TILES_3,
		TILES_4,
		TILES_5,
		TILES_6,
		TILES_7,
		TILES_8,
		TILES_9,
		TILES_10,
		TILES_11,
		TILES_12,
		TILES_13,
		TILES_14,
		TILES_15,
		TILES_16,
		TILES_17,
		TILES_18,
		TILES_19,
		TILES_20,
		TILES_21,
		TILES_22,
		TILES_23,
		TILES_24,
		TILES_25,
		TILES_26,
		TILES_27,
		TILES_28,
		TILES_29,
		TILES_30,
		TILES_31,
		TILES_32,
		TILES_33,
		TILES_34,
		TILES_35,
		TILES_36,
		TILES_37,
		TILES_38,
		TILES_39,
		TILES_40,
		TILES_41,
		TILES_42,
		TILES_43,
		TILES_44,
		TILES_45,
		TILES_46,
		TILES_47,
		TILES_48,
		TILES_49,
		TILES_50,
		TILES_51,
		TILES_52,
		TILES_53,
		TILES_54,
		TILES_55,
		TILES_56,
		TILES_57,
		TILES_58,
		TILES_59,
		TILES_60,
		TILES_61,
		TILES_62,
		TILES_63,
		TILES_64,
		TILES_65,
		TILES_66,
		TILES_67,
		TILES_68,
		TILES_69,
		TILES_70,
		TILES_71,
		TILES_72,
		TILES_73,
		TILES_74,
		TILES_75,
		TILES_76,
		TILES_77,
		TILES_78,
		TILES_79,
		TILES_80,
		TILES_81,
		TILES_82,
		TILES_83,
		TILES_84,
		TILES_85,
		TILES_86,
		TILES_87,
		TILES_88,
		TILES_89,
		TILES_90,
		TILES_91,
		TILES_92,
		TILES_93,
		TILES_94,
		TILES_95,
		TILES_96,
		TILES_97,
		TILES_98,
		TILES_99,
		TILES_100,
		TILES_101,
		TILES_102,
		TILES_103,
		TILES_104,
		TILES_105,
		TILES_106,
		TILES_107,
		TILES_108,
		TILES_109,
		TILES_110,
		TILES_111,
		TILES_112,
		TILES_113,
		TILES_114,
		TILES_115,
		TILES_116,
		TILES_117,
		TILES_118,
		TILES_119,
		TILES_120,
		TILES_121,
		TILES_122,
		TILES_123,
		TILES_124,
		TILES_125,
		TILES_126,
		TILES_127,
		TILES_128,
		TILES_129,
		TILES_130,
		TILES_131,
		TILES_132,
		TILES_133,
		TILES_134,
		TILES_135,
		TILES_136,
		TILES_137,
		TILES_138,
		TILES_139,
		TILES_140,
		TILES_141,
		TILES_142,
		TILES_143,
		TILES_144,
		TILES_145,
		TILES_146,
		TILES_147,
		TILES_148,
		TILES_149,
#if VERSION_101
		CAMPFIRE,
#endif
		TIMER,
		TREE_BRANCHES_0,
		TREE_BRANCHES_1,
		TREE_BRANCHES_2,
		TREE_BRANCHES_3,
		TREE_BRANCHES_4,
		TREE_TOPS_0,
		TREE_TOPS_1,
		TREE_TOPS_2,
		TREE_TOPS_3,
		TREE_TOPS_4,
		WALL_1,
		WALL_2,
		WALL_3,
		WALL_4,
		WALL_5,
		WALL_6,
		WALL_7,
		WALL_8,
		WALL_9,
		WALL_10,
		WALL_11,
		WALL_12,
		WALL_13,
		WALL_14,
		WALL_15,
		WALL_16,
		WALL_17,
		WALL_18,
		WALL_19,
		WALL_20,
		WALL_21,
		WALL_22,
		WALL_23,
		WALL_24,
		WALL_25,
		WALL_26,
		WALL_27,
		WALL_28,
		WALL_29,
		WALL_30,
		WALL_31,
#if VERSION_101
		WALL_32, // WALL_32-37 are stained glass, added in the not-1.2 update
		WALL_33,
		WALL_34,
		WALL_35,
		WALL_36,
		WALL_37,
#endif
		WIRES
	}

	static _sheetTiles()
	{
#if VERSION_INITIAL
		Source = new Rectangle[219]
		{
			new Rectangle(1393, 1927, 48, 1300),
			new Rectangle(1549, 29, 128, 16),
			new Rectangle(177, 2837, 128, 96),
			new Rectangle(420, 2596, 128, 96),
			new Rectangle(289, 2073, 128, 16),
			new Rectangle(993, 2019, 128, 288),
			new Rectangle(524, 2488, 128, 48),
			new Rectangle(1678, 29, 16, 16),
			new Rectangle(0, 2550, 256, 104),
			new Rectangle(289, 2008, 178, 64),
			new Rectangle(257, 2561, 162, 78),
			new Rectangle(910, 2235, 74, 52),
			new Rectangle(1138, 1909, 126, 54),
			new Rectangle(1240, 1818, 126, 54),
			new Rectangle(920, 100, 16, 20),
			new Rectangle(920, 121, 16, 20),
			new Rectangle(1122, 2187, 50, 400),
			new Rectangle(471, 2693, 72, 468),
			new Rectangle(739, 55, 186, 44),
			new Rectangle(902, 183, 12, 12),
			new Rectangle(739, 192, 2, 2),
			new Rectangle(926, 55, 6, 6),
			new Rectangle(926, 62, 6, 6),
			new Rectangle(915, 183, 12, 12),
			new Rectangle(910, 2323, 64, 64),
			new Rectangle(549, 2664, 64, 64),
			new Rectangle(1516, 1754, 288, 270),
			new Rectangle(1876, 0, 288, 270),
			new Rectangle(1407, 960, 288, 396),
			new Rectangle(739, 169, 162, 22),
			new Rectangle(777, 2235, 132, 594),
			new Rectangle(0, 2837, 176, 264),
			new Rectangle(3430, 813, 288, 270),
			new Rectangle(3539, 1355, 288, 270),
			new Rectangle(0, 2279, 288, 270),
			new Rectangle(469, 1827, 288, 270),
			new Rectangle(411, 2783, 54, 162),
			new Rectangle(1265, 1873, 72, 162),
			new Rectangle(380, 2640, 36, 36),
			new Rectangle(380, 2693, 90, 54),
			new Rectangle(1122, 2146, 54, 40),
			new Rectangle(724, 2588, 36, 80),
			new Rectangle(418, 2073, 36, 20),
			new Rectangle(544, 2729, 54, 38),
			new Rectangle(524, 2537, 36, 20),
			new Rectangle(1706, 0, 144, 54),
			new Rectangle(0, 3282, 54, 38),
			new Rectangle(289, 2098, 252, 114),
			new Rectangle(2563, 813, 288, 270),
			new Rectangle(1227, 1421, 288, 396),
			new Rectangle(739, 146, 162, 22),
			new Rectangle(1805, 1628, 288, 270),
			new Rectangle(176, 3110, 54, 38),
			new Rectangle(669, 2371, 106, 72),
			new Rectangle(1876, 813, 108, 36),
			new Rectangle(0, 3321, 36, 54),
			new Rectangle(2094, 1355, 288, 270),
			new Rectangle(1262, 2071, 36, 36),
			new Rectangle(289, 2379, 234, 90),
			new Rectangle(544, 2768, 36, 66),
			new Rectangle(1396, 1357, 106, 52),
			new Rectangle(1876, 896, 106, 52),
			new Rectangle(0, 3102, 106, 52),
			new Rectangle(3032, 542, 288, 270),
			new Rectangle(3321, 271, 288, 270),
			new Rectangle(3610, 0, 288, 270),
			new Rectangle(2563, 1084, 288, 270),
			new Rectangle(2852, 813, 288, 270),
			new Rectangle(902, 146, 34, 36),
			new Rectangle(2094, 1626, 288, 270),
			new Rectangle(2383, 1355, 288, 270),
			new Rectangle(3321, 542, 288, 270),
			new Rectangle(3610, 271, 288, 270),
			new Rectangle(2852, 1084, 288, 270),
			new Rectangle(542, 2189, 234, 90),
			new Rectangle(306, 2822, 18, 66),
			new Rectangle(177, 2934, 106, 48),
			new Rectangle(0, 2746, 234, 90),
			new Rectangle(289, 2470, 234, 90),
			new Rectangle(3141, 813, 288, 270),
			new Rectangle(758, 2001, 234, 90),
			new Rectangle(235, 2655, 144, 108),
			new Rectangle(2383, 1626, 288, 270),
			new Rectangle(2672, 1355, 288, 270),
			new Rectangle(3610, 542, 288, 270),
			new Rectangle(3141, 1084, 288, 270),
			new Rectangle(1696, 960, 288, 396),
			new Rectangle(739, 123, 180, 22),
			new Rectangle(536, 2280, 234, 90),
			new Rectangle(2672, 1626, 288, 270),
			new Rectangle(2961, 1355, 288, 270),
			new Rectangle(3430, 1084, 288, 270),
			new Rectangle(2961, 1626, 288, 270),
			new Rectangle(3250, 1355, 288, 270),
			new Rectangle(3250, 1626, 288, 270),
			new Rectangle(1005, 1818, 234, 90),
			new Rectangle(1516, 1357, 288, 396),
			new Rectangle(235, 2799, 90, 22),
			new Rectangle(262, 2983, 54, 54),
			new Rectangle(777, 2200, 144, 34),
			new Rectangle(420, 2561, 144, 34),
			new Rectangle(3539, 1626, 288, 270),
			new Rectangle(0, 2008, 288, 270),
			new Rectangle(1338, 1927, 54, 38),
			new Rectangle(1851, 37, 16, 16),
			new Rectangle(524, 2379, 144, 108),
			new Rectangle(1138, 1964, 126, 54),
			new Rectangle(1549, 0, 156, 28),
			new Rectangle(1287, 1395, 108, 22),
			new Rectangle(1876, 850, 108, 22),
			new Rectangle(1876, 873, 108, 22),
			new Rectangle(993, 1910, 144, 108),
			new Rectangle(1440, 1859, 52, 34),
			new Rectangle(262, 3038, 52, 34),
			new Rectangle(1262, 2036, 52, 34),
			new Rectangle(1338, 1966, 52, 34),
			new Rectangle(235, 2764, 142, 34),
			new Rectangle(922, 2165, 70, 50),
			new Rectangle(614, 2664, 36, 106),
			new Rectangle(1177, 2146, 36, 52),
			new Rectangle(1338, 2001, 34, 34),
			new Rectangle(107, 3102, 68, 34),
			new Rectangle(195, 3149, 34, 34),
			new Rectangle(55, 3282, 34, 102),
			new Rectangle(1028, 2308, 34, 34),
			new Rectangle(910, 2441, 34, 34),
			new Rectangle(380, 2748, 70, 34),
			new Rectangle(940, 2092, 52, 70),
			new Rectangle(724, 2517, 52, 70),
			new Rectangle(959, 2216, 32, 16),
			new Rectangle(262, 3073, 34, 264),
			new Rectangle(0, 0, 1548, 54),
			new Rectangle(910, 2388, 52, 52),
			new Rectangle(1876, 271, 288, 270),
			new Rectangle(2165, 0, 288, 270),
			new Rectangle(938, 1421, 288, 396),
			new Rectangle(739, 100, 180, 22),
			new Rectangle(1876, 542, 288, 270),
			new Rectangle(2165, 271, 288, 270),
			new Rectangle(777, 2165, 144, 34),
			new Rectangle(1440, 1818, 54, 40),
			new Rectangle(542, 2098, 234, 90),
			new Rectangle(2454, 0, 288, 270),
			new Rectangle(2165, 542, 288, 270),
			new Rectangle(2454, 271, 288, 270),
			new Rectangle(2743, 0, 288, 270),
			new Rectangle(1985, 813, 288, 270),
			new Rectangle(2454, 542, 288, 270),
			new Rectangle(2743, 271, 288, 270),
			new Rectangle(3032, 0, 288, 270),
			new Rectangle(0, 2655, 234, 90),
			new Rectangle(975, 2360, 34, 102),
			new Rectangle(910, 2288, 68, 34),
			new Rectangle(758, 1910, 234, 90),
			new Rectangle(653, 2517, 70, 156),
			new Rectangle(777, 2092, 162, 72),
			new Rectangle(1985, 1084, 288, 270),
			new Rectangle(2274, 813, 288, 270),
			new Rectangle(1367, 1818, 72, 108),
			new Rectangle(975, 2323, 52, 36),
			new Rectangle(922, 2216, 36, 18),
			new Rectangle(760, 2444, 16, 72),
			new Rectangle(1207, 2019, 54, 108),
			new Rectangle(1440, 1894, 34, 18),
			new Rectangle(284, 2934, 36, 36),
			new Rectangle(3828, 813, 72, 2268),
			new Rectangle(1805, 1357, 288, 270),
			new Rectangle(1851, 0, 18, 36),
			new Rectangle(140, 3186, 36, 36),
			new Rectangle(651, 2674, 36, 36),
			new Rectangle(85, 3155, 54, 108),
			new Rectangle(2743, 542, 288, 270),
			new Rectangle(3032, 271, 288, 270),
			new Rectangle(3321, 0, 288, 270),
			new Rectangle(2274, 1084, 288, 270),
			new Rectangle(1287, 1322, 108, 72),
			new Rectangle(140, 3149, 54, 36),
			new Rectangle(565, 2537, 84, 126),
			new Rectangle(1122, 2019, 84, 126),
			new Rectangle(0, 3155, 84, 126),
			new Rectangle(326, 2799, 84, 378),
			new Rectangle(177, 2983, 84, 126),
			new Rectangle(289, 2213, 246, 82),
			new Rectangle(289, 2296, 246, 82),
			new Rectangle(938, 1322, 348, 98),
			new Rectangle(0, 55, 738, 142),
			new Rectangle(758, 1827, 246, 82),
			new Rectangle(0, 198, 468, 180),
			new Rectangle(1407, 55, 468, 180),
			new Rectangle(0, 1465, 468, 180),
			new Rectangle(469, 1284, 468, 180),
			new Rectangle(0, 1827, 468, 180),
			new Rectangle(938, 960, 468, 180),
			new Rectangle(469, 1465, 468, 180),
			new Rectangle(938, 1141, 468, 180),
			new Rectangle(469, 1646, 468, 180),
			new Rectangle(0, 379, 468, 180),
			new Rectangle(0, 560, 468, 180),
			new Rectangle(469, 198, 468, 180),
			new Rectangle(0, 741, 468, 180),
			new Rectangle(469, 379, 468, 180),
			new Rectangle(469, 560, 468, 180),
			new Rectangle(938, 55, 468, 180),
			new Rectangle(938, 236, 468, 180),
			new Rectangle(469, 741, 468, 180),
			new Rectangle(938, 417, 468, 180),
			new Rectangle(938, 598, 468, 180),
			new Rectangle(1407, 236, 468, 180),
			new Rectangle(938, 779, 468, 180),
			new Rectangle(1407, 417, 468, 180),
			new Rectangle(1407, 598, 468, 180),
			new Rectangle(1407, 779, 468, 180),
			new Rectangle(0, 922, 468, 180),
			new Rectangle(0, 1103, 468, 180),
			new Rectangle(0, 1284, 468, 180),
			new Rectangle(469, 922, 468, 180),
			new Rectangle(469, 1103, 468, 180),
			new Rectangle(0, 1646, 468, 180),
			new Rectangle(669, 2444, 90, 72)
		};
#else
		Source = new Rectangle[226]
		{
			new Rectangle(73, 0, 48, 1300), // BACKGROUND_0
			new Rectangle(1480, 1879, 128, 16), // BACKGROUND_1
			new Rectangle(2887, 2206, 128, 96), // BACKGROUND_2
			new Rectangle(2887, 2303, 128, 96), // BACKGROUND_3
			new Rectangle(1949, 1970, 128, 16), // BACKGROUND_4
			new Rectangle(2052, 0, 128, 288), // BACKGROUND_5
			new Rectangle(2887, 2479, 128, 48), // BACKGROUND_6
			new Rectangle(460, 401, 4, 4), // BLACK_TILE : In both, it is a white tile and in updated versions, this is 4x4 instead of 16x16
			new Rectangle(2814, 2646, 256, 104), // CLOUD_0
			new Rectangle(986, 2929, 178, 64), // CLOUD_1
			new Rectangle(2887, 2400, 162, 78), // CLOUD_2
			new Rectangle(1679, 2901, 74, 52), // CLOUD_3
			new Rectangle(2943, 2933, 126, 54), // EVIL_CACTUS
			new Rectangle(247, 2981, 126, 54), // GOOD_CACTUS : Good is Hallowed
			new Rectangle(902, 3049, 272, 24), // LIQUID_0 : Water; now animated
			new Rectangle(1175, 3049, 272, 24), // LIQUID_1 : Lava; also animated
			new Rectangle(434, 0, 50, 400), // MOON : Now with 1.2 Graphics
			new Rectangle(255, 0, 72, 468), // MUSIC_BOX : Just the notes when active
			new Rectangle(715, 3012, 186, 44), // SHROOM_TOPS
			new Rectangle(2038, 379, 12, 12), // STAR_0
			new Rectangle(2045, 405, 2, 2), // STAR_1
			new Rectangle(453, 401, 6, 6), // STAR_2
			new Rectangle(2038, 405, 6, 6), // STAR_3
			new Rectangle(2038, 392, 12, 12), // STAR_4
			new Rectangle(255, 469, 114, 114), // SUN : The 1.2 sun, but this takes the cool sun's position.
			new Rectangle(411, 602, 64, 64), // SUN2 : This is now the cool sun, which before was SUN2.
			new Rectangle(2181, 0, 288, 270), // TILES_0 : Dirt
			new Rectangle(2470, 0, 288, 270), // TILES_1 : Stone
			new Rectangle(774, 0, 288, 396), // TILES_2 : Green Tops
			new Rectangle(1973, 3049, 162, 22), // TILES_3 : Reg Grass + Mushroom
			new Rectangle(122, 0, 132, 594), // TILES_4 : Torches
			new Rectangle(2797, 542, 176, 264), // TILES_5 : Tree
			new Rectangle(1567, 1210, 288, 270), // TILES_6 : Iron
			new Rectangle(651, 1481, 288, 270), // TILES_7 : Copper
			new Rectangle(1518, 1481, 288, 270), // TILES_8 : Gold
			new Rectangle(1807, 1626, 288, 270), // TILES_9 : Silver
			new Rectangle(2125, 289, 54, 162), // TILES_10 : Closed Door
			new Rectangle(2052, 289, 72, 162), // TILES_11 : Open Door
			new Rectangle(1930, 0, 36, 396), // TILES_12 : Heart Crystal, now animated
			new Rectangle(1756, 2802, 90, 54), // TILES_13 : Placed Bottles/Vase/Mug
			new Rectangle(3012, 1427, 54, 40), // TILES_14 : Table
			new Rectangle(2038, 452, 36, 80), // TILES_15 : Chair/Toilet
			new Rectangle(3034, 1620, 36, 20), // TILES_16 : Anvil
			new Rectangle(328, 0, 52, 456), // TILES_17 : Furnace, now 1.2 animated vers.
			new Rectangle(3034, 1641, 36, 20), // TILES_18 : Workbench
			new Rectangle(1209, 2838, 144, 54), // TILES_19 : Platforms
			new Rectangle(3012, 1468, 54, 38), // TILES_20 : Saplings
			new Rectangle(2814, 2531, 252, 114), // TILES_21 : Chests/Barrel/Bin
			new Rectangle(2145, 813, 288, 270), // TILES_22 : Demonite
			new Rectangle(1063, 0, 288, 396), // TILES_23 : Corrupt Grass
			new Rectangle(1810, 3049, 162, 22), // TILES_24 : Corrupt Plants
			new Rectangle(2434, 813, 288, 270), // TILES_25 : Ebonstone; retains its 1.1 look
			new Rectangle(3012, 1507, 54, 38), // TILES_26 : Altar
			new Rectangle(1949, 1897, 106, 72), // TILES_27 : Sunflowers
			new Rectangle(2696, 2893, 108, 36), // TILES_28 : Pots
			new Rectangle(370, 524, 36, 54), // TILES_29 : Piggy Bank
			new Rectangle(2723, 813, 288, 270), // TILES_30 : Wood
			new Rectangle(3012, 1014, 70, 70), // TILES_31 : Shadow Orb, now animated with the crimson heart sprite included
			new Rectangle(2814, 2842, 234, 90), // TILES_32 : Corrupt Thorns
			new Rectangle(370, 457, 36, 66), // TILES_33 : Candles
			new Rectangle(501, 2981, 106, 52), // TILES_34 : Copper Chandelier
			new Rectangle(608, 2981, 106, 52), // TILES_35 : Silver Chandelier
			new Rectangle(2535, 2984, 106, 52), // TILES_36 : Gold Chandelier
			new Rectangle(122, 866, 288, 270), // TILES_37 : Meteorite
			new Rectangle(411, 939, 288, 270), // TILES_38 : Grey Bricks
			new Rectangle(700, 939, 288, 270), // TILES_39 : Red Bricks
			new Rectangle(989, 939, 288, 270), // TILES_40 : Clay
			new Rectangle(1278, 939, 288, 270), // TILES_41 : Blue Bricks
			new Rectangle(1807, 1481, 34, 36), // TILES_42 : Lanterns
			new Rectangle(1567, 939, 288, 270), // TILES_43 : Green Bricks
			new Rectangle(1856, 1084, 288, 270), // TILES_44 : Red Bricks
			new Rectangle(2145, 1084, 288, 270), // TILES_45 : Gold Bricks
			new Rectangle(2434, 1084, 288, 270), // TILES_46 : Silver Bricks
			new Rectangle(2723, 1084, 288, 270), // TILES_47 : Copper Bricks
			new Rectangle(1756, 2893, 234, 90), // TILES_48 : Spikes
			new Rectangle(3048, 471, 18, 66), // TILES_49 : Water Candle
			new Rectangle(2943, 2988, 106, 48), // TILES_50 : Books
			new Rectangle(1991, 2893, 234, 90), // TILES_51 : Webs
			new Rectangle(2226, 2893, 234, 90), // TILES_52 : Vines
			new Rectangle(122, 1137, 288, 270), // TILES_53 : Sand
			new Rectangle(2461, 2893, 234, 90), // TILES_54 : Glass
			new Rectangle(1650, 1752, 144, 108), // TILES_55 : Sign
			new Rectangle(411, 1210, 288, 270), // TILES_56 : Obsidian
			new Rectangle(700, 1210, 288, 270), // TILES_57 : Ash
			new Rectangle(989, 1210, 288, 270), // TILES_58 : Hellstone
			new Rectangle(1278, 1210, 288, 270), // TILES_59 : Mud
			new Rectangle(1352, 0, 288, 396), // TILES_60 : Jungle Grass
			new Rectangle(1629, 3049, 180, 22), // TILES_61 : Jungle Plants
			new Rectangle(1209, 2901, 234, 90), // TILES_62 : Jungle Vines
			new Rectangle(1856, 1355, 288, 270), // TILES_63 : Sapphire Stone
			new Rectangle(2145, 1355, 288, 270), // TILES_64 : Ruby Stone
			new Rectangle(2434, 1355, 288, 270), // TILES_65 : Emerald Stone
			new Rectangle(2723, 1355, 288, 270), // TILES_66 : Topaz Stone
			new Rectangle(73, 1408, 288, 270), // TILES_67 : Amethyst Stone
			new Rectangle(362, 1481, 288, 270), // TILES_68 : Diamond Stone
			new Rectangle(1444, 2901, 234, 90), // TILES_69 : Jungle Thorns
			new Rectangle(1641, 0, 288, 396), // TILES_70 : Mushroom Grass
			new Rectangle(404, 3036, 90, 22), // TILES_71 : Glowshrooms
			new Rectangle(3012, 1227, 54, 54), // TILES_72 : Glowshroom Stalk
			new Rectangle(2642, 3016, 144, 34), // TILES_73 : Tall Reg Grass
			new Rectangle(2787, 3016, 144, 34), // TILES_74 : Tall Jungle Grass
			new Rectangle(940, 1481, 288, 270), // TILES_75 : Obsidian Bricks
			new Rectangle(1229, 1481, 288, 270), // TILES_76 : Hellstone Bricks
			new Rectangle(381, 0, 52, 456), // TILES_77 : Hellforge; Like Furnace, has been updated
			new Rectangle(434, 438, 16, 16), // TILES_78 : Clay Pot
			new Rectangle(2887, 1988, 144, 108), // TILES_79 : Bed
			new Rectangle(374, 2981, 126, 54), // TILES_80 : Reg Cactus
			new Rectangle(247, 3036, 156, 28), // TILES_81 : Coral
			new Rectangle(1756, 2857, 108, 22), // TILES_82 : Flowers Stg1
			new Rectangle(2932, 3037, 108, 22), // TILES_83 : Flowers Stg2
			new Rectangle(2136, 3049, 108, 22), // TILES_84 : Flowers Stg2
			new Rectangle(2887, 2097, 144, 108), // TILES_85 : Headstones
			new Rectangle(3016, 2206, 52, 34), // TILES_86 : Loom
			new Rectangle(3016, 2241, 52, 34), // TILES_87 : Piano
			new Rectangle(3016, 2276, 52, 34), // TILES_88 : Dresser
			new Rectangle(3016, 2311, 52, 34), // TILES_89 : Bench
			new Rectangle(501, 3034, 142, 34), // TILES_90 : Bathtubs
			new Rectangle(3012, 1335, 70, 50), // TILES_91 : Banners (Before the influx)
			new Rectangle(1930, 397, 36, 106), // TILES_92 : Lampposts
			new Rectangle(73, 1301, 36, 52), // TILES_93 : Tiki Torch
			new Rectangle(362, 1445, 34, 34), // TILES_94 : Keg
			new Rectangle(1407, 2330, 68, 34), // TILES_95 : Chinese Lantern
			new Rectangle(1807, 1518, 34, 34), // TILES_96 : Cooking Pot
			new Rectangle(3048, 368, 34, 102), // TILES_97 : Safe
			new Rectangle(1807, 1553, 34, 34), // TILES_98 : Skull Lantern
			new Rectangle(1807, 1588, 34, 34), // TILES_99 : Trash Can 
			new Rectangle(1856, 777, 70, 34), // TILES_100 : Candelabrum
			new Rectangle(3012, 1085, 52, 70), // TILES_101 : Bookcase
			new Rectangle(3012, 1156, 52, 70), // TILES_102 : Throne
			new Rectangle(2075, 525, 32, 16), // TILES_103 : Bowl
			new Rectangle(3048, 0, 34, 264), // TILES_104 : Clock
			new Rectangle(986, 2994, 1548, 54), // TILES_105 : Statues
			new Rectangle(3012, 1282, 52, 52), // TILES_106 : Sawmill
			new Rectangle(2759, 0, 288, 270), // TILES_107 : Cobalt
			new Rectangle(2181, 271, 288, 270), // TILES_108 : Mythril
			new Rectangle(485, 0, 288, 396), // TILES_109 : Hallowed Grass
			new Rectangle(1448, 3049, 180, 22), // TILES_110 : Hallowed Plants
			new Rectangle(2470, 271, 288, 270), // TILES_111 : Adamantite
			new Rectangle(2759, 271, 288, 270), // TILES_112 : Ebonsand
			new Rectangle(1650, 1861, 144, 34), // TILES_113 : Tall Hallowed Grass
			new Rectangle(3012, 1386, 54, 40), // TILES_114 : Tinkerer's Workshop
			new Rectangle(2814, 2751, 234, 90), // TILES_115 : Hallowed Vines
			new Rectangle(485, 397, 288, 270), // TILES_116 : Pearlsand
			new Rectangle(774, 397, 288, 270), // TILES_117 : Pearlstone
			new Rectangle(1063, 397, 288, 270), // TILES_118 : Pearlstone Brick
			new Rectangle(1352, 397, 288, 270), // TILES_119 : Iridescent Brick
			new Rectangle(1641, 397, 288, 270), // TILES_120 : Mudstone
			new Rectangle(1930, 542, 288, 270), // TILES_121 : Cobalt Brick
			new Rectangle(2219, 542, 288, 270), // TILES_122 : Mythril Brick
			new Rectangle(2508, 542, 288, 270), // TILES_123 : Silt
			new Rectangle(739, 2838, 234, 90), // TILES_124 : Wooden Beam
			new Rectangle(3048, 265, 34, 102), // TILES_125 : Crystal Ball
			new Rectangle(1407, 2295, 68, 34), // TILES_126 : Disco Ball
			new Rectangle(974, 2838, 234, 90), // TILES_127 : Magic Ice Block
			new Rectangle(1967, 379, 70, 156), // TILES_128 : Mannequin
			new Rectangle(73, 1679, 162, 72), // TILES_129 : Crystal Shards
			new Rectangle(122, 595, 288, 270), // TILES_130 : Active Stone
			new Rectangle(411, 668, 288, 270), // TILES_131 : Inactive Stone
			new Rectangle(1856, 668, 72, 108), // TILES_132 : Lever
			new Rectangle(3012, 1583, 52, 36), // TILES_133 : Adamantite Forge
			new Rectangle(3034, 1662, 36, 18), // TILES_134 : Mythril Anvil
			new Rectangle(466, 401, 16, 72), // TILES_135 : Pressure Plates
			new Rectangle(3012, 796, 54, 108), // TILES_136 : Switch
			new Rectangle(3034, 1681, 34, 18), // TILES_137 : Dart Trap
			new Rectangle(1930, 504, 36, 36), // TILES_138 : Boulder
			new Rectangle(0, 0, 72, 2268), // TILES_139 : Music Box; The actual tiles
			new Rectangle(700, 668, 288, 270), // TILES_140 : Demonite Brick
			new Rectangle(434, 401, 18, 36), // TILES_141 : Explosives
			new Rectangle(73, 1354, 36, 36), // TILES_142 : Inlet Pump
			new Rectangle(362, 1408, 36, 36), // TILES_143 : Outlet Pump
			new Rectangle(3012, 905, 54, 108), // TILES_144 : Timers
			new Rectangle(989, 668, 288, 270), // TILES_145 : Red Candy Cane
			new Rectangle(1278, 668, 288, 270), // TILES_146 : Green Candy Cane
			new Rectangle(1567, 668, 288, 270), // TILES_147 : Snow
			new Rectangle(1856, 813, 288, 270), // TILES_148 : Snow Bricks
			new Rectangle(236, 1679, 108, 72), // TILES_149 : Xmas Lights
			new Rectangle(411, 457, 54, 144), // CAMPFIRE ------------------------------------- New Addition
			new Rectangle(3012, 1546, 54, 36), // TIMER
			new Rectangle(2974, 542, 84, 126), // TREE_BRANCHES_0 : Reg
			new Rectangle(2974, 669, 84, 126), // TREE_BRANCHES_1 : Corrupt
			new Rectangle(1480, 1752, 84, 126), // TREE_BRANCHES_2 : Jungle
			new Rectangle(1967, 0, 84, 378), // TREE_BRANCHES_3 : Hallow
			new Rectangle(1565, 1752, 84, 126), // TREE_BRANCHES_4 : Snow
			new Rectangle(739, 2929, 246, 82), // TREE_TOPS_0 : Reg
			new Rectangle(2696, 2933, 246, 82), // TREE_TOPS_1 : Corrupt
			new Rectangle(1407, 2802, 348, 98), // TREE_TOPS_2 : Jungle
			new Rectangle(0, 2838, 738, 142), // TREE_TOPS_3 : Hallow
			new Rectangle(0, 2981, 246, 82), // TREE_TOPS_4 : Snow
			new Rectangle(2096, 1626, 468, 180), // WALL_1 : Stone
			new Rectangle(1949, 1988, 468, 180), // WALL_2 : Dirt (Natural)
			new Rectangle(938, 2295, 468, 180), // WALL_3 : Ebonstone (Natural)
			new Rectangle(1407, 2621, 468, 180), // WALL_4 : Wood
			new Rectangle(0, 2657, 468, 180), // WALL_5 : Grey Brick
			new Rectangle(469, 2657, 468, 180), // WALL_6 : Red Brick
			new Rectangle(938, 2657, 468, 180), // WALL_7 : Blue Brick (Natural)
			new Rectangle(1876, 2712, 468, 180), // WALL_8 : Green Brick (Natural)
			new Rectangle(2345, 2712, 468, 180), // WALL_9 : Pink Brick (Natural)
			new Rectangle(2565, 1626, 468, 180), // WALL_10 : Gold
			new Rectangle(73, 1752, 468, 180), // WALL_11 : Silver
			new Rectangle(542, 1752, 468, 180), // WALL_12 : Copper
			new Rectangle(1011, 1752, 468, 180), // WALL_13 : Hellstone (Natural)
			new Rectangle(2096, 1807, 468, 180), // WALL_14 : Obsidian (Natural)
			new Rectangle(2565, 1807, 468, 180), // WALL_15 : Mud (Natural)
			new Rectangle(1480, 1897, 468, 180), // WALL_16 : Dirt
			new Rectangle(73, 1933, 468, 180), // WALL_17 : Blue Brick
			new Rectangle(542, 1933, 468, 180), // WALL_18 : Green Brick
			new Rectangle(1011, 1933, 468, 180), // WALL_19 : Pink Brick
			new Rectangle(2418, 1988, 468, 180), // WALL_20 : Obsidian
			new Rectangle(1480, 2078, 468, 180), // WALL_21 : Glass
			new Rectangle(73, 2114, 468, 180), // WALL_22 : Pearlstone
			new Rectangle(542, 2114, 468, 180), // WALL_23 : Iridescent
			new Rectangle(1011, 2114, 468, 180), // WALL_24 : Mudstone
			new Rectangle(1949, 2169, 468, 180), // WALL_25 : Cobalt
			new Rectangle(2418, 2169, 468, 180), // WALL_26 : Mythril
			new Rectangle(1480, 2259, 468, 180), // WALL_27 : Planked Wood
			new Rectangle(0, 2295, 468, 180), // WALL_28 : Pearlstone (Natural)
			new Rectangle(469, 2295, 468, 180), // WALL_29 : Red Candy Cane
			new Rectangle(1949, 2350, 468, 180), // WALL_30 : Green Candy Cane
			new Rectangle(2418, 2350, 468, 180), // WALL_31 : Snow Brick
			new Rectangle(1407, 2440, 468, 180), // WALL_32 : Purple Stained Glass ------------ New Additions
			new Rectangle(0, 2476, 468, 180), // WALL_33 : Yellow Stained Glass								|
			new Rectangle(469, 2476, 468, 180), // WALL_34 : Blue Stained Glass								|
			new Rectangle(938, 2476, 468, 180), // WALL_35 : Green Stained Glass							|
			new Rectangle(1876, 2531, 468, 180), // WALL_36 : Red Stained Glass								|
			new Rectangle(2345, 2531, 468, 180), // WALL_37 : Multicolour Stained Glass ------- - - - - - - -
			new Rectangle(2075, 452, 90, 72), // WIRES
		};
#endif
	}

	public static void LoadContent(ContentManager Content)
	{
		Tex = Content.Load<Texture2D>("Images/_sheetTiles");
	}
}
