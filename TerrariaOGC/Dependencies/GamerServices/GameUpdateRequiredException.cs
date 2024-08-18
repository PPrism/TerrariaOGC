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
using System.Runtime.Serialization;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public class GameUpdateRequiredException : Exception
	{
		#region Public Constructors

		public GameUpdateRequiredException() : base()
		{
		}

		public GameUpdateRequiredException(string message) : base(message)
		{
		}

		public GameUpdateRequiredException(
			string message,
			Exception innerException
		) : base(message, innerException) {
		}

		#endregion

		#region Protected Constructor

		protected GameUpdateRequiredException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) {
		}

		#endregion
	}
}
