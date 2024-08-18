using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.SoundUI
{
	public class Assets
	{
		public static Texture2D SliderTex, SoundIconsTex;

		public static Rectangle SliderEmptyRect, SliderEmptyInactiveRect, SliderFullRect, SliderFullInactiveRect, SoundIconRect, MusicIconRect;

		public static void LoadContent(ContentManager Content)
		{
			SliderTex = Content.Load<Texture2D>("UI/SoundBar");
			SoundIconsTex = Content.Load<Texture2D>("UI/SoundIcons");
			SliderEmptyRect = new Rectangle(0, 0, 244, 58);
			SliderEmptyInactiveRect = new Rectangle(0, 58, 244, 58);
			SliderFullRect = new Rectangle(0, 116, 244, 58);
			SliderFullInactiveRect = new Rectangle(0, 174, 244, 58);
			SoundIconRect = new Rectangle(0, 36, 40, 36);
			MusicIconRect = new Rectangle(0, 0, 40, 36);
		}
	}
}
