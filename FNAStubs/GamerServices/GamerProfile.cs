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
using System.IO;
using System.Globalization;
#endregion

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed class GamerProfile : IDisposable
	{
		#region Public Properties

		public int GamerScore
		{
			get;
			private set;
		}

		public GamerZone GamerZone
		{
			get;
			private set;
		}

		public string Motto
		{
			get;
			private set;
		}

		public RegionInfo Region
		{
			get;
			private set;
		}

		public float Reputation
		{
			get;
			private set;
		}

		public int TitlesPlayed
		{
			get;
			private set;
		}

		public int TotalAchievements
		{
			get;
			private set;
		}

		public bool IsDisposed
		{
			get;
			private set;
		}

		#endregion

		#region Internal Constructor

		internal GamerProfile()
		{
			IsDisposed = false;

			// TODO: Everything below
			GamerScore = 0;
			GamerZone = GamerZone.Pro; // WUBWUBWUBWUBWUB
			Motto = string.Empty;
			Region = RegionInfo.CurrentRegion;
			Reputation = 5.0f;
			TitlesPlayed = 1;
			TotalAchievements = 0;
		}

		#endregion

		#region Public Methods

		public void Dispose()
		{
			IsDisposed = true;
		}

		public Stream GetGamerPicture()
		{
			return null;
		}

		#endregion
	}
}
