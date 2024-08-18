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
using System.Collections.ObjectModel;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public class GamerCollection<T> : ReadOnlyCollection<T>, IEnumerable<T>, IEnumerable where T : Gamer
	{
		#region Internal Variables

		internal List<T> collection;

		#endregion

		#region Internal Constructor

		internal GamerCollection(List<T> collection) : base(collection)
		{
			this.collection = collection;
		}

        internal GamerCollection() : base(new List<T>())
        {
        }

        #endregion

        #region Public Methods

        public new GamerCollectionEnumerator GetEnumerator()
		{
			return new GamerCollectionEnumerator(this);
		}

		#endregion

		#region IEnumerable Implementation

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Custom Enumerator

		public struct GamerCollectionEnumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			#region Public Properties

			public T Current
			{
				get
				{
					return collection[position];
				}
			}

			#endregion

			#region Private Variables

			private GamerCollection<T> collection;
			private int position;

			#endregion

			#region Internal Constructor

			internal GamerCollectionEnumerator(GamerCollection<T> collection)
			{
				this.collection = collection;
				position = -1;
			}

			#endregion

			#region Public Methods

			public void Dispose()
			{
				collection = null;
			}

			public bool MoveNext()
			{
				position += 1;
				return (position < collection.Count);
			}

			#endregion

			#region IEnumerator Implementation

			object IEnumerator.Current
			{
				get
				{
					return collection[position];
				}
			}

			void IEnumerator.Reset()
			{
				position = -1;
			}

			#endregion
		}

		#endregion
	}
}
