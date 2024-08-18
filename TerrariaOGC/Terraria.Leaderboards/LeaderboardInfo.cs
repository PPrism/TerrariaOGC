using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace Terraria.Leaderboards
{
	internal class LeaderboardInfo
	{
		private enum TitleTextID
		{
			DISTANCE = 58,
			MINING_GATHERING,
			CRAFTING,
			USED,
			NORMAL_BOSSES,
			HARD_BOSSES,
			DEATHS
		}

		private class ColumnMapping
		{
			public StatisticEntry Statistic;

			public Column ColumnType;

			public string ColumnName;

			public ColumnMapping(StatisticEntry Entry, Column Column, string Name)
			{
				Statistic = Entry;
				ColumnType = Column;
				ColumnName = Name;
			}
		}

        private readonly LeaderboardIdentity Identity;

		private readonly ColumnMapping[] Columns;

		private readonly int NameID;

		private static readonly LeaderboardInfo[] LeaderboardsInfo = CreateAll();

		private static ColumnMapping Map(StatisticEntry Entry, Column Column)
		{
			string Name = Column.ToString();
			return new ColumnMapping(Entry, Column, Name);
		}

        private static LeaderboardIdentity CreateId(string Key)
		{
			LeaderboardIdentity Result = default;
            Result.Key = Key;
            Result.GameMode = 0;
			return Result;
		}

		private LeaderboardInfo(LeaderboardIdentity Identity, ColumnMapping[] Columns, TitleTextID NameID)
		{
			this.Identity = Identity;
			this.Columns = Columns;
			this.NameID = (int)NameID;
		}

		public static LeaderboardInfo[] CreateAll()
		{
			LeaderboardInfo[] MetricCategories = new LeaderboardInfo[5];
			ColumnMapping[] RelatedColumns = new ColumnMapping[4]
			{
				Map(StatisticEntry.AirTravel, Column.AIR_COLUMN),
				Map(StatisticEntry.GroundTravel, Column.GROUND_COLUMN),
				Map(StatisticEntry.WaterTravel, Column.WATER_COLUMN),
				Map(StatisticEntry.LavaTravel, Column.LAVA_COLUMN)
			};
			MetricCategories[0] = new LeaderboardInfo(CreateId("Distance"), RelatedColumns, TitleTextID.DISTANCE);
			RelatedColumns = new ColumnMapping[4]
			{
				Map(StatisticEntry.Ore, Column.ORE_COLUMN),
				Map(StatisticEntry.Gems, Column.GEMS_COLUMN),
				Map(StatisticEntry.Soils, Column.SOILS_COLUMN),
				Map(StatisticEntry.Wood, Column.WOOD_COLUMN)
			};
			MetricCategories[1] = new LeaderboardInfo(CreateId("Mining"), RelatedColumns, TitleTextID.MINING_GATHERING);
			RelatedColumns = new ColumnMapping[6]
			{
				Map(StatisticEntry.FurnitureCrafted, Column.FURNITURE_COLUMN),
				Map(StatisticEntry.ToolsCrafted, Column.TOOLS_COLUMN),
				Map(StatisticEntry.WeaponsCrafted, Column.WEAPONS_COLUMN),
				Map(StatisticEntry.ArmorCrafted, Column.ARMOR_COLUMN),
				Map(StatisticEntry.ConsumablesCrafted, Column.CONSUMABLES_COLUMN),
				Map(StatisticEntry.MiscCrafted, Column.MISC_COLUMN)
			};
			MetricCategories[2] = new LeaderboardInfo(CreateId("Crafting"), RelatedColumns, TitleTextID.CRAFTING);
			RelatedColumns = new ColumnMapping[5]
			{
				Map(StatisticEntry.KingSlime, Column.KING_SLIME_COLUMN),
				Map(StatisticEntry.EyeOfCthulhu, Column.CTHULHU_COLUMN),
				Map(StatisticEntry.EaterOfWorlds, Column.EATER_COLUMN),
				Map(StatisticEntry.Skeletron, Column.SKELETRON_COLUMN),
				Map(StatisticEntry.WallOfFlesh, Column.WALL_COLUMN)
			};
			MetricCategories[3] = new LeaderboardInfo(CreateId("Normal bosses"), RelatedColumns, TitleTextID.NORMAL_BOSSES);
			RelatedColumns = new ColumnMapping[4]
			{
				Map(StatisticEntry.TheTwins, Column.TWINS_COLUMN),
				Map(StatisticEntry.TheDestroyer, Column.DESTROYER_COLUMN),
				Map(StatisticEntry.SkeletronPrime, Column.SKELETRON_PRIME_COLUMN),
				Map(StatisticEntry.Ocram, Column.OCRAM_COLUMN)
			};
			MetricCategories[4] = new LeaderboardInfo(CreateId("Hard bosses"), RelatedColumns, TitleTextID.HARD_BOSSES);
			return MetricCategories;
		}

        public static LeaderboardIdentity GetIdentity(Leaderboard BoardType)
		{
			return LeaderboardsInfo[(int)BoardType].Identity;
		}

		public static Column[] GetColumns(Leaderboard BoardType)
		{
			LeaderboardInfo LeaderboardInfo = LeaderboardsInfo[(int)BoardType];
			Column[] ColumnsOnBoard = new Column[LeaderboardInfo.Columns.Length];
			for (int Index = LeaderboardInfo.Columns.Length - 1; Index > -1; Index--)
			{
				ColumnsOnBoard[Index] = LeaderboardInfo.Columns[Index].ColumnType;
			}
			return ColumnsOnBoard;
		}

		public static string GetName(Leaderboard BoardType)
		{
			LeaderboardInfo LeaderboardInfo = LeaderboardsInfo[(int)BoardType];
			return Lang.MenuText[LeaderboardInfo.NameID];
		}

        public static void SubmitStatistics(Statistics Stats, NetworkGamer Gamer)
		{
			if (Netplay.Session.SessionState != NetworkSessionState.Playing)
			{
				return;
			}
			LeaderboardWriter Writer = Gamer.LeaderboardWriter;
			if (Writer != null)
			{
				LeaderboardInfo[] Leaderboards = LeaderboardsInfo;
				foreach (LeaderboardInfo Leaderboard in Leaderboards)
				{
					Leaderboard.Submit(Writer, Stats);
				}
			}
		}

		public static void SubmitStatisticsToLeaderboard(Leaderboard BoardType, Statistics Stats, Gamer CurrentGamer) // Unused
		{
			LeaderboardWriter Writer = CurrentGamer.LeaderboardWriter;
			if (Writer != null)
			{
				LeaderboardInfo LeaderboardInfo = LeaderboardsInfo[(int)BoardType];
				LeaderboardInfo.Submit(Writer, Stats);
			}
		}

		private void Submit(LeaderboardWriter Writer, Statistics Stats)
		{
			long Rating = 0L;
			LeaderboardEntry Leaderboard = Writer.GetLeaderboard(Identity);
			ColumnMapping[] AllColumns = Columns;
			foreach (ColumnMapping IndividualMapping in AllColumns)
			{
				uint MetricSet = Stats[IndividualMapping.Statistic];
				Leaderboard.Columns[IndividualMapping.ColumnName] = (int)MetricSet; // Initially this set an int as 0, then set that int as (int)MetricSet in the very next line, and then assigned it to this...
				Rating += MetricSet;
			}

			Leaderboard.Rating = Rating;
        }
    }
}
