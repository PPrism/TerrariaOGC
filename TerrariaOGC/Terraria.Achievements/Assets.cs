using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Achievements
{
	internal class Assets
	{
		public static Texture2D[] Advancements;

		public static void LoadContent(ContentManager Content)
		{
			Advancements = new Texture2D[(byte)Achievement.AchievementCount];
			string IconsDir = "Achievements/";
			for (byte AchievementNum = 0; AchievementNum < Advancements.Length; AchievementNum++)
			{
				string Appendage = AchievementNum.ToString();
				if (AchievementNum < 10)
				{
					Appendage = "0" + Appendage;
				}
				Advancements[AchievementNum] = Content.Load<Texture2D>(IconsDir + "Achievement" + Appendage);
			}
		}
	}
}
