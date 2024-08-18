#if !USE_ORIGINAL_CODE
using System;
using System.Collections.Generic;
using System.Threading;
using static Terraria.Achievements.AchievementSystem;

namespace Terraria.Achievements
{
	internal class CompletedAsyncResult : IAsyncResult
	{
		public CompletedAsyncResult(object State)
		{
			AsyncState = State;
		}

		public object AsyncState { get; set; }

		public WaitHandle AsyncWaitHandle => new ManualResetEvent(true);

		public bool CompletedSynchronously => true;

		public bool IsCompleted => true;
	}
	public class AchievementData
	{
		public class Row
		{
			public bool Available;

			public int Rank;
		}

		private class Cache
		{
			public Row[] Entries;

			public int StartIndex;

			public int NumEntries;
		}

		private enum AsyncRequest
		{
			None,
			FullRead,
			NextPage,
			PreviousPage
		}

		private const int PageSize = 50;

		public int NumEntries;

		private Cache StaleCache, CurrentCache, MergedCache;

        private MockReader Reader;

		public static Dictionary<Achievement, AchievementSystem.TerrariaAchievement> CurrentAchievements;

		private IAsyncResult ReqResult;

		private AsyncRequest ReadStateReq;

		private readonly int DefaultBatchSize;

		public int BatchSize, BatchStart, Selected;

        public bool Ready => Reader != null;

		private bool CanPageDown => Reader.CanPageDown;

		private bool CanPageUp => Reader.CanPageUp;

		private static void ProcessEntries(MockReader Reader, ref Cache BoardCache)
		{
			Row[] entries = Reader.Entries;
            int NumEntries = entries.Length;
			int EntryIdx = 0;
			for (int j = 0; j < entries.Length; j++)
			{
				Row Entry = entries[j];
				if (Entry.Rank == 0)
                {
                    NumEntries--;
                    continue;
                }
                Row BoardRow = BoardCache.Entries[EntryIdx];
                BoardRow.Rank = Entry.Rank;
				BoardRow.Available = true;
				EntryIdx++;
			}
			BoardCache.StartIndex = Reader.PageStart;
			BoardCache.NumEntries = NumEntries;
		}

		public static void SetAchievements()
		{
			CurrentAchievements = Main.AchievementSystem.GetCurrentAchievements();
		}

		public TerrariaAchievement GetAchievement(Achievement Achievement)
		{
			return CurrentAchievements[Achievement];
		}

        public AchievementData(int Size)
		{
			CurrentAchievements = null;
			DefaultBatchSize = Size;
			BatchStart = 0;
			MergedCache = new Cache
			{
				Entries = new Row[0],
				StartIndex = 0
			};
		}

		public void LoadLeaderboard()
		{
			ReqResult = new CompletedAsyncResult(true);
			Reader = null;
			ReadStateReq = AsyncRequest.FullRead;
			NumEntries = 0;
			ResetCaches();
		}

		private void ResetCaches()
		{
			Row[] PrimarySet = new Row[PageSize];
			Row[] SecondarySet = new Row[PageSize];
			for (int Idx = PageSize - 1; Idx >= 0; Idx--)
			{
				PrimarySet[Idx] = new Row { Available = false };
				SecondarySet[Idx] = new Row { Available = false };
			}
			CurrentCache = new Cache
			{
				Entries = PrimarySet,
				StartIndex = 0,
				NumEntries = 0
			};
			StaleCache = new Cache
			{
				Entries = SecondarySet,
				StartIndex = 0,
				NumEntries = 0
			};
			MergedCache = new Cache
			{
				Entries = PrimarySet,
				StartIndex = 0,
				NumEntries = 0
			};
			BatchStart = 0;
		}

		public bool Update()
		{
			if (ReadStateReq == AsyncRequest.None || !ReqResult.IsCompleted)
			{
				return true;
			}
			if (ReadStateReq == AsyncRequest.FullRead)
			{
				try
				{
					Reader = MockReader.Create((byte)Achievement.AchievementCount, PageSize);
				}
				catch
				{
					return false;
				}
				BatchStart = Reader.PageStart;
				Selected = 0;
                if (ReqResult.AsyncState != null)
				{
					bool Active = (bool)ReqResult.AsyncState;
					int BatchEntryIndex = BatchStart;
					foreach (Row Entry in Reader.Entries)
					{
						if (Entry.Available == Active)
						{
							Selected = BatchEntryIndex;
							BatchStart = BatchEntryIndex;
							break;
						}
						BatchEntryIndex++;
					}
				}
			}

			ProcessEntries(Reader, ref CurrentCache);
			if (StaleCache.StartIndex <= CurrentCache.StartIndex)
			{
				CreateMergedCache(StaleCache, CurrentCache);
			}
			else
			{
				CreateMergedCache(CurrentCache, StaleCache);
			}

			if (ReadStateReq == AsyncRequest.FullRead)
			{
				BatchSize = Math.Min(CurrentCache.NumEntries, DefaultBatchSize);
				if (CurrentCache.NumEntries > 0 && (Reader.CanPageDown || Reader.CanPageUp))
				{
					NumEntries = (byte)Achievement.AchievementCount;
				}
				else
				{
					NumEntries = CurrentCache.NumEntries;
				}
				if (BatchStart > NumEntries - BatchSize)
				{
					BatchStart = NumEntries - BatchSize;
				}
			}
			ReadStateReq = AsyncRequest.None;
			ReqResult = null;
			return true;
		}

		public bool MoveDown()
		{
			Selected++;
			if (Selected < BatchStart + BatchSize)
			{
				return true;
			}
			BatchStart++;
			bool NotAllCached = CurrentCache.StartIndex + CurrentCache.NumEntries < BatchStart + BatchSize;
            if (NotAllCached && !CanPageDown && ReadStateReq == AsyncRequest.None)
			{
				Selected--;
				BatchStart--;
				return false;
			}
			bool Moved = true;
			if (NotAllCached)
			{
				if (ReadStateReq == AsyncRequest.None)
				{
					CacheNextPage();
				}
				else
				{
					BatchStart--;
					Selected--;
					Moved = false;
				}
			}
			return Moved;
		}

		public bool MoveUp()
		{
			Selected--;
			if (Selected >= BatchStart)
			{
				return true;
			}
			BatchStart--;
			bool NotInCache = BatchStart < CurrentCache.StartIndex;
            if (NotInCache && !CanPageUp && ReadStateReq == AsyncRequest.None)
			{
				Selected++;
				BatchStart++;
				return false;
			}
			bool Moved = true;
			if (NotInCache)
			{
				if (ReadStateReq == AsyncRequest.None)
				{
					CachePreviousPage();
				}
				else
				{
					BatchStart++;
					Selected++;
					Moved = false;
				}
			}
			return Moved;
		}

		private void CacheNextPage()
		{
			int NewCacheStart = CurrentCache.StartIndex + CurrentCache.NumEntries;
			FreeCache(NewCacheStart);
			CreateMergedCache(StaleCache, CurrentCache);
			ReadStateReq = AsyncRequest.NextPage;
		}

		private void CachePreviousPage()
		{
			int PrevCachePtr = Math.Min(CurrentCache.NumEntries, CurrentCache.StartIndex);
			int NewCacheStart = CurrentCache.StartIndex - PrevCachePtr;
			FreeCache(NewCacheStart);
			CreateMergedCache(CurrentCache, StaleCache);
            ReadStateReq = AsyncRequest.PreviousPage;
		}

		private void FreeCache(int NewCacheStart)
		{
			var TempCache = CurrentCache;
			CurrentCache = StaleCache;
			StaleCache = TempCache;
			if (CurrentCache.StartIndex != NewCacheStart)
			{
				CurrentCache.StartIndex = NewCacheStart;
				for (int Entry = 0; Entry < CurrentCache.NumEntries; Entry++)
				{
					CurrentCache.Entries[Entry].Available = false;
				}
			}
		}

		private void CreateMergedCache(Cache FirstCache, Cache SecondCache)
		{
			int FirstSize = SecondCache.StartIndex - FirstCache.StartIndex + SecondCache.NumEntries;
			Cache SetupCache = new Cache
			{
				Entries = new Row[FirstSize],
				StartIndex = FirstCache.StartIndex,
				NumEntries = FirstSize
			};
			Cache MergeResult = SetupCache;
			for (int FirstIdx = FirstCache.NumEntries - 1; FirstIdx >= 0; FirstIdx--)
			{
				MergeResult.Entries[FirstIdx] = FirstCache.Entries[FirstIdx];
			}
			int Difference = SecondCache.StartIndex - FirstCache.StartIndex;
			for (int SecondIdx = SecondCache.NumEntries - 1; SecondIdx >= 0; SecondIdx--)
			{
				Row MergerEntry = MergeResult.Entries[Difference + SecondIdx];
				if (MergerEntry == null || !MergerEntry.Available)
				{
					MergeResult.Entries[Difference + SecondIdx] = SecondCache.Entries[SecondIdx];
				}
			}
			MergedCache = MergeResult;
		}

		public Row[] GetRows()
		{
			int MinBatchSize = Math.Min(BatchSize, MergedCache.NumEntries);
			int StartSelection = BatchStart - MergedCache.StartIndex;
			if (StartSelection < 0)
			{
				StartSelection = 0;
			}
			if (StartSelection + MinBatchSize > MergedCache.NumEntries)
			{
				StartSelection = MergedCache.NumEntries - MinBatchSize;
			}
			Row[] Rows = new Row[MinBatchSize];
			CopyRows(ref MergedCache.Entries, StartSelection, ref Rows, 0, MinBatchSize);
			return Rows;
		}

		private void CopyRows(ref Row[] Source, int SourceIdx, ref Row[] Destination, int DestinationIdx, int RowCount)
		{
			for (int RowIdx = RowCount - 1; RowIdx >= 0; RowIdx--)
			{
				Destination[DestinationIdx + RowIdx] = Source[SourceIdx + RowIdx];
			}
		}
	}
}
#endif