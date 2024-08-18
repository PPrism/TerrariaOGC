#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class LeaderboardReader : IDisposable
	{
		#region Public Properties

		public bool IsDisposed
		{
			get;
			private set;
		}

		public bool CanPageDown
		{
			get
			{
				if (entryCache.Count == 0)
				{
					return false;
				}
				if (isFriendBoard)
				{
					return (PageStart + pageSize) < entryCache.Count;
				}
				return (	PageStart < entryCache.Count ||
						entryCache[entryCache.Count - 1].RankingEXT < TotalLeaderboardSize	);
			}
		}

		public bool CanPageUp
		{
			get
			{
				if (entryCache.Count == 0)
				{
					return false;
				}
				if (isFriendBoard)
				{
					return (PageStart - pageSize) >= 0;
				}
				return (	PageStart > 0 ||
						entryCache[0].RankingEXT > 1	);
			}
		}

		public ReadOnlyCollection<LeaderboardEntry> Entries
		{
			get
			{
				return new ReadOnlyCollection<LeaderboardEntry>(entries);
			}
		}

		public LeaderboardIdentity LeaderboardIdentity
		{
			get;
			private set;
		}

		public int PageStart
		{
			get;
			private set;
		}

		public int TotalLeaderboardSize
		{
			get;
			private set;
		}

		#endregion

		#region Private Variables

		private int pageSize;
		private List<LeaderboardEntry> entries;
		private List<LeaderboardEntry> entryCache;
		private bool isFriendBoard;

		#endregion

		#region Internal Constructor

		internal LeaderboardReader(
			LeaderboardIdentity identity,
			int start,
			int size,
			List<LeaderboardEntry> entries,
			bool friends
		) {
			LeaderboardIdentity = identity;
			PageStart = start;
			pageSize = size;
			TotalLeaderboardSize = 0;
			isFriendBoard = friends;

			entryCache = entries;
			this.entries = new List<LeaderboardEntry>(pageSize);
			for (int i = PageStart; i < pageSize && i < entryCache.Count; i += 1)
			{
				this.entries.Add(entryCache[i]);
			}

			IsDisposed = false;
		}

		#endregion

		#region Public Methods

		public void Dispose()
		{
			IsDisposed = true;
		}

		public void PageDown()
		{
			IAsyncResult result = BeginPageDown(null, null);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					// readAction.IsCompleted = true;
				}
			}
			EndPageDown(result);
		}

		public IAsyncResult BeginPageDown(
			AsyncCallback callback,
			object asyncState
		) {
			throw new NotSupportedException();
		}

		public void EndPageDown(IAsyncResult result)
		{
			throw new NotSupportedException();
		}

		public void PageUp()
		{
			IAsyncResult result = BeginPageUp(null, null);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					// readAction.IsCompleted = true;
				}
			}
			EndPageUp(result);
		}

		public IAsyncResult BeginPageUp(
			AsyncCallback callback,
			object asyncState
		) {
			throw new NotSupportedException();
		}

		public void EndPageUp(IAsyncResult result)
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Public Static Methods

		public static LeaderboardReader Read(
			LeaderboardIdentity leaderboardId,
			int pageStart,
			int pageSize
		) {
			IAsyncResult result = BeginRead(
				leaderboardId,
				pageStart,
				pageSize,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					// readAction.IsCompleted = true;
				}
			}
			return EndRead(result);
		}

		public static LeaderboardReader Read(
			LeaderboardIdentity leaderboardId,
			Gamer pivotGamer,
			int pageSize
		) {
			IAsyncResult result = BeginRead(
				leaderboardId,
				pivotGamer,
				pageSize,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					// readAction.IsCompleted = true;
				}
			}
			return EndRead(result);
		}

		public static LeaderboardReader Read(
			LeaderboardIdentity leaderboardId,
			IEnumerable<Gamer> gamers,
			Gamer pivotGamer,
			int pageSize
		) {
			IAsyncResult result = BeginRead(
				leaderboardId,
				gamers,
				pivotGamer,
				pageSize,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					// readAction.IsCompleted = true;
				}
			}
			return EndRead(result);
		}

		public static IAsyncResult BeginRead(
			LeaderboardIdentity leaderboardId,
			int pageStart,
			int pageSize,
			AsyncCallback callback,
			object asyncState
		) {
			throw new NotSupportedException();
		}

		public static IAsyncResult BeginRead(
			LeaderboardIdentity leaderboardId,
			Gamer pivotGamer,
			int pageSize,
			AsyncCallback callback,
			object asyncState
		) {
			throw new NotSupportedException();
		}

		public static IAsyncResult BeginRead(
			LeaderboardIdentity leaderboardId,
			IEnumerable<Gamer> gamers,
			Gamer pivotGamer,
			int pageSize,
			AsyncCallback callback,
			object asyncState
		) {
			throw new NotSupportedException();
		}

		public static LeaderboardReader EndRead(IAsyncResult result)
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
