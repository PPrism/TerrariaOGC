#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System.Collections.Generic;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class SignedInGamerCollection : GamerCollection<SignedInGamer>
	{
		#region Public Properties

		public SignedInGamer this[PlayerIndex index]
		{
			get
			{
				int id = (int) index;
				if (id >= collection.Count)
				{
					return null;
				}
				return collection[id];
			}
		}

		#endregion

		#region Internal Constructor

		internal SignedInGamerCollection(List<SignedInGamer> collection) : base(collection)
		{
		}

		#endregion
	}
}
