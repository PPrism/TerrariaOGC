#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class GamerPrivileges
	{
		#region Public Properties

		public GamerPrivilegeSetting AllowCommunication
		{
			get;
			private set;
		}

		public bool AllowOnlineSessions
		{
			get;
			private set;
		}

		public bool AllowPremiumContent
		{
			get;
			private set;
		}

		public GamerPrivilegeSetting AllowProfileViewing
		{
			get;
			private set;
		}

		public bool AllowPurchaseContent
		{
			get;
			private set;
		}

		public bool AllowTradeContent
		{
			get;
			private set;
		}

		public GamerPrivilegeSetting AllowUserCreatedContent
		{
			get;
			private set;
		}

		#endregion

		#region Internal Constructor

		internal GamerPrivileges()
		{
			// TODO: Everything below -flibit
			AllowCommunication = GamerPrivilegeSetting.Everyone;
			AllowOnlineSessions = true;
			AllowPremiumContent = true;
			AllowProfileViewing = GamerPrivilegeSetting.Everyone;
			AllowPurchaseContent = true;
			AllowTradeContent = true;
			AllowUserCreatedContent = GamerPrivilegeSetting.Everyone;
		}

		#endregion
	}
}
