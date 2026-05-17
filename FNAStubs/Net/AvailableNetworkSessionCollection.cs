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
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public sealed class AvailableNetworkSessionCollection : ReadOnlyCollection<AvailableNetworkSession>, IDisposable
	{
		#region Public Properties

		public bool IsDisposed
		{
			get;
			private set;
		}

		#endregion

		#region Private Variables

		private List<AvailableNetworkSession> collection;

		#endregion

		#region Internal Constructor

		internal AvailableNetworkSessionCollection(List<AvailableNetworkSession> collection) : base(collection)
		{
			this.collection = collection;
			IsDisposed = false;
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

		#endregion
	}
}
