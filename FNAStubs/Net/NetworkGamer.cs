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

using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public class NetworkGamer : Gamer
	{
		#region Public Properties

		public bool HasLeftSession
		{
			get;
			private set;
		}

        long remoteUniqueIdentifier = -1;

        public bool HasVoice
		{
			get;
			private set;
		}

		public byte Id
		{
			get
			{
				return 0;
			}
		}

		public bool IsGuest
		{
			get;
			private set;
		}

		public bool IsHost
		{
			get
			{
				return true;
			}
		}

		public bool IsLocal
		{
			get
			{
				return (this is LocalNetworkGamer);
			}
		}

		public bool IsMutedByLocalUser
		{
			get;
			private set;
		}

		public bool IsPrivateSlot
		{
			get;
			private set;
		}

		public bool IsReady
		{
			// InvalidOperationException: This operation is only valid when the session state is NetworkSessionState.Lobby.
			// InvalidOperationException: This method cannot be called on remote gamer instances. It is only valid when NetworkGamer.IsLocal is true.
			// ObjectDisposedException: This NetworkGamer is no longer valid. The gamer may have left the session.
			get;
			set;
		}

		public bool IsTalking
		{
			get;
			private set;
		}
        internal int State
        {
            get
			{
				int result = 0x0;
				if (IsLocal) { result += 0x000001; }
                if (IsHost) { result += 0x000010; }
                if (HasVoice) { result += 0x000100; }
                if (IsGuest) { result += 0x001000; }
                if (IsMutedByLocalUser) { result += 0x010000; }
                if (IsPrivateSlot) { result += 0x100000; }
                if (IsReady) { result += 0x1000000; }
				return result;
            }
            set
			{
				int result = value;
				if (result >= 0x1000000)
				{
					IsReady = true;
					result -= 0x1000000;

                }
				else
				{
					IsReady = false;
				}

                if (result >= 0x100000)
                {
                    IsPrivateSlot = true;
                    result -= 0x100000;

                }
                else
                {
                    IsPrivateSlot = false;
                }

                if (result >= 0x010000)
                {
                    IsMutedByLocalUser = true;
                    result -= 0x010000;

                }
                else
                {
                    IsMutedByLocalUser = false;
                }

                if (result >= 0x001000)
                {
                    IsGuest = true;
                    result -= 0x001000;

                }
                else
                {
                    IsGuest = false;
                }

                if (result >= 0x000100)
                {
                    HasVoice = true;
                    result -= 0x000100;

                }
                else
                {
                    HasVoice = false;
                }
            }
        }

        public NetworkMachine Machine
		{
			get;
			set;
		}

		public TimeSpan RoundtripTime
		{
			get;
			private set;
		}

		public NetworkSession Session
		{
			get;
			private set;
		}
        internal long RemoteUniqueIdentifier
        {
            get { return remoteUniqueIdentifier; }
            set { remoteUniqueIdentifier = value; }
        }

        #endregion

        #region Internal Constructor

        internal NetworkGamer(
			NetworkSession session
		) : base(
			"Stub Gamer",
			"Stub Gamer"
		) {
			Session = session;

			// TODO: Everything below
			HasLeftSession = false;
			HasVoice = false;
			IsGuest = false;
			IsMutedByLocalUser = false;
			IsPrivateSlot = false;
			IsReady = false;
			IsTalking = false;
			Machine = new NetworkMachine();
			RoundtripTime = TimeSpan.Zero;
		}

		#endregion
	}
}
