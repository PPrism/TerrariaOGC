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
	public class NetworkSessionEndedEventArgs : EventArgs
	{
		#region Public Properties

		public NetworkSessionEndReason EndReason
		{
			get;
			private set;
		}

		#endregion

		#region Public Constructor

		public NetworkSessionEndedEventArgs(
			NetworkSessionEndReason endReason
		) {
			EndReason = endReason;
		}

		#endregion
	}
}
