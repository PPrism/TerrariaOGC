#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Net
{
	public sealed class NetworkMachine
	{
		#region Public Properties

		public GamerCollection<NetworkGamer> Gamers
		{
			get;
			private set;
		}

		#endregion

		#region Internal Constructor

		internal NetworkMachine()
		{
			// FIXME: This is one huge joke. -ade
			Gamers = new GamerCollection<NetworkGamer>(new List<NetworkGamer>());
		}

		#endregion

		#region Public Methods

		public void RemoveFromSession()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
