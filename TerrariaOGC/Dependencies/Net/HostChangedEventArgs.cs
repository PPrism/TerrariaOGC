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
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public class HostChangedEventArgs : EventArgs
	{
		#region Public Properties

		public NetworkGamer OldHost
		{
			get;
			private set;
		}

		public NetworkGamer NewHost
		{
			get;
			private set;
		}

		#endregion

		#region Public Constructor

		public HostChangedEventArgs(
			NetworkGamer oldHost,
			NetworkGamer newHost
		) {
			OldHost = oldHost;
			NewHost = newHost;
		}

		#endregion
	}
}
