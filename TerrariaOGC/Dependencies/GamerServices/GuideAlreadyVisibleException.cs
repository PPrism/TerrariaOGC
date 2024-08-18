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
	public class GuideAlreadyVisibleException : Exception
	{
		#region Public Constructors

		public GuideAlreadyVisibleException() : base()
		{
		}

		public GuideAlreadyVisibleException(string message) : base(message)
		{
		}

		public GuideAlreadyVisibleException(
			string message,
			Exception innerException
		) : base(message, innerException) {
		}

		#endregion

		#region Protected Constructor

		protected GuideAlreadyVisibleException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) {
		}

		#endregion
	}
}
