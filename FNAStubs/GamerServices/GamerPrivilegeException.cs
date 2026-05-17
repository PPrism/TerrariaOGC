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
	[Serializable]
	public class GamerPrivilegeException : Exception
	{
		#region Public Constructors

		public GamerPrivilegeException() : base()
		{
		}

		public GamerPrivilegeException(string message) : base(message)
		{
		}

		public GamerPrivilegeException(
			string message,
			Exception innerException
		) : base(message, innerException) {
		}

		#endregion

		#region Protected Constructor

		protected GamerPrivilegeException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) {
		}

		#endregion
	}
}
