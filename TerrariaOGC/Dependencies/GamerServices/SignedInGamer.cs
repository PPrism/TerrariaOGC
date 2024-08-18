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
using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class SignedInGamer : Gamer
	{
		#region Public Properties

		public GameDefaults GameDefaults
		{
			get;
			private set;
		}

		public bool IsGuest
		{
			get;
			private set;
		}

		public bool IsSignedInToLive
		{
			get;
			private set;
		}

		public int PartySize
		{
			get;
			set; // FIXME: Actually read-only but okay guys -flibit
		}

		public PlayerIndex PlayerIndex
		{
			// ObjectDisposedException: Thrown if the associated profile is no longer valid. For example, the profile may have signed out.
			get;
			private set;
		}

		public GamerPresence Presence
		{
			get;
			private set;
		}

		public GamerPrivileges Privileges
		{
			get;
			private set;
		}

		#endregion

		#region Private Variables

		private GamerAction statStoreAction;
		private GamerAction statReceiveAction;
		private AchievementCollection gamerAchievements;

		#endregion

		#region Public Events

		public static event EventHandler<SignedInEventArgs> SignedIn;
		public static event EventHandler<SignedOutEventArgs> SignedOut;

		#endregion

		#region Internal Constructor

		public SignedInGamer(//was internal
            string gamertag,
			bool isSignedInToLive = false,
			bool isGuest = false,
			PlayerIndex playerIndex = PlayerIndex.One
		) : base(gamertag, gamertag) {
			IsGuest = isGuest;
			IsSignedInToLive = isSignedInToLive;
			PlayerIndex = playerIndex;

			// TODO: Everything below
			GameDefaults = new GameDefaults();
			Presence = new GamerPresence();
			Privileges = new GamerPrivileges();
			PartySize = 1;
		}

		#endregion

		#region Public Methods

		public bool IsFriend(Gamer gamer)
		{
			return false;
		}

		public bool IsHeadset(Microphone microphone)
		{
			// FIXME: Check against Gamer? -flibit
			return microphone.IsHeadset;
		}

		public FriendCollection GetFriends()
		{
			List<FriendGamer> friends = new List<FriendGamer>();
			return new FriendCollection(friends);
		}

		public void AwardAchievement(string achievementKey)
		{
		}

		public IAsyncResult BeginAwardAchievement(
			string achievementKey,
			AsyncCallback callback,
			object state
		) {
			if (statStoreAction != null)
			{
				// FIXME: Pray that we don't get overlap -flibit
				// throw new InvalidOperationException();
			}
			statStoreAction = new GamerAction(state, callback);
			statStoreAction.IsCompleted = true;
			return statStoreAction;
		}

		public void EndAwardAchievement(IAsyncResult result)
		{
			statStoreAction = null;
		}

		public AchievementCollection GetAchievements()
		{
			if (IsSignedInToLive)
			{
				if (gamerAchievements == null)
				{
					gamerAchievements = new AchievementCollection();
				}

			}
			return gamerAchievements;
		}

		delegate AchievementCollection GetAchievementsDelegate();

		public IAsyncResult BeginGetAchievements(
			AsyncCallback callback,
			object asyncState
		) {
			GetAchievementsDelegate gad = GetAchievements;
			return gad.BeginInvoke(callback, gad);
		}

		public AchievementCollection EndGetAchievements(IAsyncResult result)
		{
			GetAchievementsDelegate gad = (GetAchievementsDelegate)result.AsyncState;
			gamerAchievements = gad.EndInvoke(result);
			return gamerAchievements;
		}

		#endregion

		#region Internal Static Methods

		internal static void OnSignIn(SignedInGamer gamer)
		{
			if (SignedIn != null)
			{
				SignedIn(null, new SignedInEventArgs(gamer));
			}
		}

		internal static void OnSignOut(SignedInGamer gamer)
		{
			if (SignedOut != null)
			{
				SignedOut(null, new SignedOutEventArgs(gamer));
			}
		}

		#endregion
	}
}
