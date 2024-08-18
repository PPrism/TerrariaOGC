using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Leaderboards
{
	internal class Assets
	{
		public static Texture2D[] ColumnIcons;

		public static void LoadContent(ContentManager Content)
		{
			ColumnIcons = new Texture2D[32];
			string IconsDir = "UI/Leaderboards/";
			for (int Metric = 31; Metric >= 0; Metric--)
			{
				string MetricName = ((Column)Metric).ToString().ToLower(); // Could've just made all the entries in the Column enum be in lowercase and ditch the .ToLower(), but you do you Engine.
				string Asset = IconsDir + MetricName;
				ColumnIcons[Metric] = Content.Load<Texture2D>(Asset);
			}
		}
	}
}
