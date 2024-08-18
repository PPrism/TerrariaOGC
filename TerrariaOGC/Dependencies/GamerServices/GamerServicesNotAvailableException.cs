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
	public class GamerServicesNotAvailableException : Exception
	{
		#region Public Constructors

		public GamerServicesNotAvailableException() : base()
		{
		}

		public GamerServicesNotAvailableException(string message) : base(message)
		{
		}

		public GamerServicesNotAvailableException(
			string message,
			Exception innerException
		) : base(message, innerException) {
		}

		#endregion

		#region Protected Constructor

		protected GamerServicesNotAvailableException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) {
		}

		#endregion
	}
}
