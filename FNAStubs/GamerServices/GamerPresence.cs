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
	public sealed class GamerPresence
	{
		#region Public Properties

		public GamerPresenceMode PresenceMode
		{
			get
			{
				return presenceMode;
			}
			set
			{
				string s = presenceModeStrings[(int) value];
				if (s != presence)
				{
					presence = s;
					SetPresenceModeStringEXT(presence);
				}
				presenceMode = value;
			}
		}

		public int PresenceValue
		{
			get
			{
				return presenceValue;
			}
			set
			{
				if (value != presenceValue)
				{
					presenceValue = value;
					SetPresenceModeStringEXT(presence);
				}
			}
		}

		#endregion

		#region Private Variables

		private string presence;
		private GamerPresenceMode presenceMode;
		private int presenceValue;

		#endregion

		#region Private Static Variables

		private static readonly string[] presenceModeStrings = new string[]
		{
			"Arcade Mode",
			"At Menu",
			"Battling Boss",
			"Campaign Mode",
			"Challenge Mode",
			"Configuring Settings",
			"Co-Op: Level {0}",
			"Co-Op: Stage {0}",
			"Cornflower Blue",
			"Customizing Player",
			"Difficulty: Easy",
			"Difficulty: Extreme",
			"Difficulty: Hard",
			"Difficulty: Medium",
			"Editing Level",
			"Exploration Mode",
			"Found Secret",
			"Free Play",
			"Game Over",
			"In Combat",
			"In Game Store",
			"Level {0}",
			"Local Co-Op",
			"Local Versus",
			"Looking For Games",
			"Losing",
			"Multiplayer",
			"Nearly Finished",
			string.Empty,
			"On a Roll",
			"Online Co-Op",
			"Online Versus",
			"Outnumbered",
			"Paused",
			"Playing Minigame",
			"Playing With Friends",
			"Practice Mode",
			"Puzzle Mode",
			"Scenario Mode",
			"Score {0}",
			"Score is Tied",
			"Setting Up Match",
			"Single Player",
			"Stage {0}",
			"Starting Game",
			"Story Mode",
			"Stuck on a Hard Bit",
			"Survival Mode",
			"Time Attack",
			"Trying For Record",
			"Tutorial Mode",
			"Versus Computer",
			"Versus: Score {0}",
			"Waiting For Players",
			"Waiting In Lobby",
			"Wasting Time",
			"Watching Credits",
			"Watching Cutscene",
			"Winning",
			"Won the Game"
		};

		#endregion

		#region Internal Constructor

		internal GamerPresence()
		{
			presenceMode = GamerPresenceMode.None;
			PresenceValue = 0;
		}

		#endregion

		#region Public Extensions

		public void SetPresenceModeStringEXT(string mode)
		{
		}

		#endregion
	}
}
