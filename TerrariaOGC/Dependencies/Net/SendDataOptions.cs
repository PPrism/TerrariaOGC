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
	[Flags]
	public enum SendDataOptions
	{
		None,
		Reliable,
		InOrder,
		ReliableInOrder,
		Chat
	}
}
