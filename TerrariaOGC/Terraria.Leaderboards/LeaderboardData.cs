using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.GamerServices;

namespace Terraria.Leaderboards
{
	public class LeaderboardData
	{
		public class Row
		{
			public bool Available;

			public int Rank;

			public uint[] Statistics;

            public Gamer Gamer;

			public string Gamertag => Gamer.Gamertag;
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

        private LeaderboardReader Reader;

		private IAsyncResult ReqResult;

		private AsyncRequest ReadStateReq;

		private readonly int DefaultBatchSize;

		public int BatchSize, BatchStart, Selected;

		public Column[] Columns;

		public string LeaderboardName;

        public bool Ready => Reader != null;

		private bool CanPageDown => Reader.CanPageDown;

		private bool CanPageUp => Reader.CanPageUp;

		// I based the achievement viewer off of the leaderboard designs to fit with the UI, and to do that, I utilised the MockReader instead of GamerServices.
		// With enough workarounds and replacements, the leaderboards can be viewable with dummy data.
		private static void ProcessEntries(LeaderboardReader Reader, Column[] Columns, ref Cache BoardCache)
		{
			ReadOnlyCollection<LeaderboardEntry> entries = Reader.Entries;
            int NumEntries = entries.Count;
			int EntryIdx = 0;
			int NumColumns = Columns.Length;
			string[] ColumnSet = new string[NumColumns];

			for (int i = NumColumns - 1; i > -1; i--)
			{
				ColumnSet[i] = Columns[i].ToString();
			}
			for (int j = 0; j < entries.Count; j++)
			{
				LeaderboardEntry Entry = entries[j];
#if USE_ORIGINAL_CODE
				if (Entry.GetRank() == 0)
				{
					NumEntries--;
					continue;
				}
				Row BoardRow = BoardCache.Entries[EntryIdx];
				BoardRow.Rank = Entry.GetRank();
#else
				if (Entry.RankingEXT == 0)
                {
                    NumEntries--;
                    continue;
                }
                Row BoardRow = BoardCache.Entries[EntryIdx];
                BoardRow.Rank = Entry.RankingEXT;
#endif
                BoardRow.Gamer = Entry.Gamer;
				BoardRow.Statistics = new uint[NumColumns];

				for (int k = NumColumns - 1; k > -1; k--)
				{
					string Key = ColumnSet[k];
					BoardRow.Statistics[k] = (uint)Entry.Columns[Key]; // Initially had 2 variables, 1 of which was an int of Entry.Columns[Key] and the other, a uint; they then became one, waste of space.
				}
				BoardRow.Available = true;
				EntryIdx++;
			}
			BoardCache.StartIndex = Reader.PageStart;
			BoardCache.NumEntries = NumEntries;
		}

        public LeaderboardData(int Size)
		{
			DefaultBatchSize = Size;
			BatchStart = 0;
			MergedCache = new Cache
			{
				Entries = new Row[0],
				StartIndex = 0
			};
		}

		public void LoadLeaderboard(Leaderboard BoardType) // Gets the board in a general context.
		{
            LeaderboardIdentity Identity = LeaderboardInfo.GetIdentity(BoardType);
			ReqResult = LeaderboardReader.BeginRead(Identity, 0, PageSize, null, null);
            Reader = null;
			ReadStateReq = AsyncRequest.FullRead;
			NumEntries = 0;
			ResetCaches();
			Columns = LeaderboardInfo.GetColumns(BoardType);
			LeaderboardName = LeaderboardInfo.GetName(BoardType);
		}

        public void LoadLeaderboard(Leaderboard BoardType, Gamer CurrentGamer) // Gets the board in the context for the current user.
		{
			LeaderboardIdentity Identity = LeaderboardInfo.GetIdentity(BoardType);
            ReqResult = LeaderboardReader.BeginRead(Identity, CurrentGamer, PageSize, null, CurrentGamer);
            Reader = null;
			ReadStateReq = AsyncRequest.FullRead;
			NumEntries = 0;
			ResetCaches();
			Columns = LeaderboardInfo.GetColumns(BoardType);
			LeaderboardName = LeaderboardInfo.GetName(BoardType);
		}

		public void LoadLeaderboard(Leaderboard BoardType, FriendCollection FriendsList, Gamer CurrentGamer) // Gets the board for the current user with any friends they have.
		{
			LeaderboardIdentity Identity = LeaderboardInfo.GetIdentity(BoardType);
			ReqResult = LeaderboardReader.BeginRead(Identity, (IEnumerable<Gamer>)(object)FriendsList, CurrentGamer, ((ReadOnlyCollection<FriendGamer>)(object)FriendsList).Count + 1, null, CurrentGamer);
			Reader = null;
			ReadStateReq = AsyncRequest.FullRead;
			NumEntries = 0;
			ResetCaches();
			Columns = LeaderboardInfo.GetColumns(BoardType);
			LeaderboardName = LeaderboardInfo.GetName(BoardType);
		}

		private void ResetCaches()
		{
			Row[] PrimarySet = new Row[PageSize];
			Row[] SecondarySet = new Row[PageSize];
			for (int Idx = 49; Idx >= 0; Idx--)
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
					Reader = LeaderboardReader.EndRead(ReqResult);
				}
				catch
				{
					return false;
				}
				BatchStart = Reader.PageStart;
				Selected = 0;
                if (ReqResult.AsyncState != null)
				{
					Gamer GamerInContext = (Gamer)ReqResult.AsyncState;
					int BatchEntryIndex = BatchStart;
					foreach (LeaderboardEntry Entry in Reader.Entries)
					{
						if (Entry.Gamer.Gamertag == GamerInContext.Gamertag)
						{
							Selected = BatchEntryIndex;
							BatchStart = BatchEntryIndex;
							break;
						}
						BatchEntryIndex++;
					}
				}
			}
			else if (ReadStateReq == AsyncRequest.NextPage)
			{
				Reader.EndPageDown(ReqResult);
			}
			else if (ReadStateReq == AsyncRequest.PreviousPage)
			{
				Reader.EndPageUp(ReqResult);
			}

			ProcessEntries(Reader, Columns, ref CurrentCache);
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
					NumEntries = Reader.TotalLeaderboardSize;
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
            ReqResult = Reader.BeginPageDown(null, null);
			ReadStateReq = AsyncRequest.NextPage;
		}

		private void CachePreviousPage()
		{
			int PrevCachePtr = Math.Min(CurrentCache.NumEntries, CurrentCache.StartIndex);
			int NewCacheStart = CurrentCache.StartIndex - PrevCachePtr;
			FreeCache(NewCacheStart);
			CreateMergedCache(CurrentCache, StaleCache);
            ReqResult = Reader.BeginPageUp(null, null);
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
				// MergerEntry = MergeResult.Entries[Difference + SecondIdx]; Why?
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

        public Gamer GetSelectedGamer()
		{
			int Selection = Selected - MergedCache.StartIndex;
			if (Selection >= 0 && Selection < MergedCache.NumEntries)
			{
				return MergedCache.Entries[Selection].Gamer;
			}
			return null;
		}
	}
}
