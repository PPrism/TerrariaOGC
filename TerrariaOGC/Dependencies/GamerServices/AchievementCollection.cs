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
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public class AchievementCollection : IList<Achievement>, ICollection<Achievement>, IEnumerable<Achievement>, IEnumerable, IDisposable
	{
		#region Public Properties

		public int Count
		{
			get
			{
				return collection.Count;
			}
		}

		public bool IsDisposed
		{
			get;
			private set;
		}

		public Achievement this[int index]
		{
			get
			{
				return collection[index];
			}
		}

		public Achievement this[string achievementKey]
		{
			get
			{
				foreach (Achievement ach in collection)
				{
					if (ach.Key == achievementKey)
					{
						return ach;
					}
				}
				throw new IndexOutOfRangeException();
			}
		}

		#endregion

		#region Private Variables

		private List<Achievement> collection;

		#endregion

		#region Internal Constructor

		internal AchievementCollection(List<Achievement> collection)
		{
			this.collection = collection;
			IsDisposed = false;
		}
		public AchievementCollection()
		{
			collection = new List<Achievement>();
		}

		#endregion

		#region Public Methods

		public void Dispose()
		{
			if (!IsDisposed)
			{
				collection.Clear();
				IsDisposed = true;
			}
		}

		public IEnumerator<Achievement> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		#endregion

		#region IList Implementation

		Achievement IList<Achievement>.this[int index]
		{
			get
			{
				return collection[index];
			}
			set
			{
				// FIXME: This should not be here, but IList...?! -flibit
			}
		}

		int IList<Achievement>.IndexOf(Achievement item)
		{
			return collection.IndexOf(item);
		}

		void IList<Achievement>.Insert(int index, Achievement item)
		{
			collection.Insert(index, item);
		}

		void IList<Achievement>.RemoveAt(int index)
		{
			collection.RemoveAt(index);
		}

		#endregion

		#region ICollection Implementation

		bool ICollection<Achievement>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		void ICollection<Achievement>.Add(Achievement item)
		{
			collection.Add(item);
		}

		bool ICollection<Achievement>.Remove(Achievement item)
		{
			return collection.Remove(item);
		}

		void ICollection<Achievement>.Clear()
		{
			collection.Clear();
		}

		bool ICollection<Achievement>.Contains(Achievement item)
		{
			return collection.Contains(item);
		}

		void ICollection<Achievement>.CopyTo(Achievement[] array, int arrayIndex)
		{
			collection.CopyTo(array, arrayIndex);
		}

		#endregion

		#region IEnumerable Implementation

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
