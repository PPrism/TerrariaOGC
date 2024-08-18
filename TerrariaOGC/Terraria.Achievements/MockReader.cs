#if !USE_ORIGINAL_CODE
using System;

namespace Terraria.Achievements
{
	public class MockReader
	{
		private readonly int PageSize;

		private readonly AchievementData.Row[] Database;

		public int PageStart;

		public bool Loaded;

		public bool CanPageDown => PageStart + PageSize < Database.Length;

		public bool CanPageUp => PageStart > 0;

		public AchievementData.Row[] Entries
		{
			get
			{
				AchievementData.Row[] Rows = new AchievementData.Row[PageSize];
				for (int Entry = PageSize - 1; Entry >= 0; Entry--)
				{
					Rows[Entry] = Database[PageStart + Entry];
				}
				return Rows;
			}
		}

		public static MockReader Create(int NumEntries, int PageSize)
		{
			AchievementData.Row[] Rows = new AchievementData.Row[NumEntries];
			for (int Entry = NumEntries - 1; Entry >= 0; Entry--)
			{
				Rows[Entry] = new AchievementData.Row
				{
					Available = true,
					Rank = Entry + 1,
				};
			}
			return new MockReader(Rows, 0, Math.Min(NumEntries, PageSize));
		}

		private MockReader(AchievementData.Row[] Database, int PageStart, int PageSize)
		{
			this.Database = Database;
			this.PageSize = PageSize;
			this.PageStart = PageStart;
		}
	}
}
#endif