using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.HowToPlay
{
	public class Assets
	{
		public static int TextBGBorderWidth;

		public static Texture2D TextBG, GameLogo, MovementSection, HotbarSection, BuffSection, InventorySection;

		public static void LoadContent(ContentManager Content)
		{

#if USE_ORIGINAL_CODE
			TextBG = Content.Load<Texture2D>("UI/HowToPlay/Text_Back"); 
			TextBGBorderWidth = 20;
#else
#if VERSION_INITIAL
			TextBG = Content.Load<Texture2D>("UI/HowToPlay/Text_Back");
#else
			TextBG = Content.Load<Texture2D>("UI/HowToPlay/Howto_Logo"); // Since it is not used with the code equivalent, stubbing it out with the game logo.
#endif
			switch (Main.ScreenHeightPtr)
			{
				case 0:
					TextBGBorderWidth = 20;
					break;
				case 1:
					TextBGBorderWidth = 24;
					break;
				case 2:
					TextBGBorderWidth = 32;
					break;
			}
#endif
			MovementSection = Content.Load<Texture2D>("UI/HowToPlay/Howto_Movement"); // If you have a sharp eye, you would notice the mouse cursor from the PC version in this image right next to the player.
			HotbarSection = Content.Load<Texture2D>("UI/HowToPlay/Howto_Hotbar2");
			BuffSection = Content.Load<Texture2D>("UI/HowToPlay/Howto_Debuff"); // The image shows 4 buffs, why the hell is it calling itself Howto_Debuff???
			InventorySection = Content.Load<Texture2D>("UI/HowToPlay/Howto_Inventory");
			GameLogo = Content.Load<Texture2D>("UI/HowToPlay/Howto_Logo");
		}
	}
}
