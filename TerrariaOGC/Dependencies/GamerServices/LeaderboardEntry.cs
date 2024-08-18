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
	public sealed class LeaderboardEntry
	{
		#region Public Properties

		public PropertyDictionary Columns
		{
			// TODO: https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.gamerservices.leaderboardentry.columns.aspx
			get;
			private set;
		}

		public Gamer Gamer
		{
			get;
			private set;
		}

		public long Rating
		{
			get
			{
				return rating;
			}
			set
			{
				rating = value;
			}
		}

		public int RankingEXT
		{
			get;
			private set;
		}


		#endregion

		#region Private Variables

		private long rating;

		#endregion

		#region Internal Constructor

		internal LeaderboardEntry(
			Gamer gamer,
			long rating,
			int ranking
		) {
			Gamer = gamer;
			this.rating = rating;

			RankingEXT = ranking;

			Columns = new PropertyDictionary(
				new System.Collections.Generic.Dictionary<string, object>()
			);
		}

		#endregion
	}
}
