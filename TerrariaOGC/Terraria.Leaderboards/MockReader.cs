using System;

namespace Terraria.Leaderboards
{
	public class MockReader 
	{
		// Hard to say with this project in particular, but I think this is a way for them (Engine) to test leaderboard functionality so that XBLive can be integrated.
		// Could be used in the future to create a private server if XBLive for 360 gets terminated.
		private const int SEED = 1234567;

		private readonly int PageSize;

		private readonly LeaderboardData.Row[] Database;

		public int PageStart;

		public bool Loaded;

		public bool CanPageDown => PageStart + PageSize < TotalLeaderboardSize;

		public bool CanPageUp => PageStart > 0;

		public int TotalLeaderboardSize
		{
			get
			{
				if (!Loaded)
				{
					return 0;
				}
				return Database.Length;
			}
		}

		public LeaderboardData.Row[] Entries
		{
			get
			{
				LeaderboardData.Row[] Rows = new LeaderboardData.Row[PageSize];
				for (int Entry = PageSize - 1; Entry >= 0; Entry--)
				{
					Rows[Entry] = Database[PageStart + Entry];
				}
				return Rows;
			}
		}

		public static MockReader Create(int NumEntries, int PageSize, int NumColumns)
		{
			FastRandom RNG = new FastRandom(SEED);
			LeaderboardData.Row[] Rows = new LeaderboardData.Row[NumEntries];
			for (int Entry = NumEntries - 1; Entry >= 0; Entry--)
			{
				Rows[Entry] = new LeaderboardData.Row
				{
					Available = true,
					Rank = Entry + 1,
					Statistics = new uint[NumColumns]
				};
				for (int i = 0; i < NumColumns; i++)
				{
					Rows[Entry].Statistics[i] = (uint)RNG.Next(109999998);
				}
			}
			return new MockReader(Rows, 0, Math.Min(NumEntries, PageSize));
		}

		private MockReader(LeaderboardData.Row[] Database, int PageStart, int PageSize)
		{
			this.Database = Database;
			this.PageSize = PageSize;
			this.PageStart = PageStart;
		}

		public void PageDown()
		{
			PageStart = Math.Min(PageStart + PageSize, TotalLeaderboardSize - PageSize);
		}

		public void PageUp()
		{
			PageStart = Math.Max(PageStart - PageSize, 0);
		}
	}
}
