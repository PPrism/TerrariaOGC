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
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public static class GamerServicesDispatcher
	{
		#region Public Static Properties

		public static bool IsInitialized
		{
			get;
			private set;
		}

		public static IntPtr WindowHandle
		{
			get;
			set;
		}

		#endregion

		#region Public Static Events

#pragma warning disable 0067
		// This should never happen, but lol XNA4 compliance -flibit
		public static event EventHandler<EventArgs> InstallingTitleUpdate;
#pragma warning restore 0067

		#endregion

		#region Public Static Methods

		public static void Initialize(IServiceProvider serviceProvider)
		{
			IsInitialized = true;

			AppDomain.CurrentDomain.ProcessExit += (o, e) =>
			{
				IsInitialized = false;
			};

			List<SignedInGamer> startGamers = new List<SignedInGamer>(1);
			startGamers.Add(new SignedInGamer(
				"Stub Gamer",
				IsInitialized
			));

			// FIXME: This is stupid -flibit
			startGamers.Add(new SignedInGamer(
				"Stub Gamer (1)",
				IsInitialized,
				true,
				PlayerIndex.Two
			));
			startGamers.Add(new SignedInGamer(
				"Stub Gamer (2)",
				IsInitialized,
				true,
				PlayerIndex.Three
			));
			startGamers.Add(new SignedInGamer(
				"Stub Gamer (3)",
				IsInitialized,
				true,
				PlayerIndex.Four
			));

			Gamer.SignedInGamers = new SignedInGamerCollection(startGamers);
			foreach (SignedInGamer gamer in Gamer.SignedInGamers)
			{
				SignedInGamer.OnSignIn(gamer);
			}
		}

		public static void Update()
		{
		}

		internal static bool UpdateAsync()
		{
			// If a thread's calling this after we quit, tell it to panic!
			if (IsInitialized)
			{
				Update();
			}
			return IsInitialized;
		}

		#endregion
	}
}
