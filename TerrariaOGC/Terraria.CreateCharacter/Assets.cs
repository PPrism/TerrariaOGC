using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	internal class Assets
	{
		public const int HairTypes = 36;

		public static Texture2D CategoryContainer, SelectedCategoryContainer, HairContainer, SelectedHairContainer, Palette, SelectedColorContainer, 
			MaleIcon, FemaleIcon, SoftcoreIcon, MediumcoreIcon, HardcoreIcon, SelectedHorizontalContainer, HorizontalContainer, Frame;

		public static Texture2D[] CategoryIcons;

		public static Rectangle[] Sources;

		public static void LoadContent(ContentManager Content)
		{
			CategoryContainer = Content.Load<Texture2D>("UI/CharacterCreation/Tile01");
			SelectedCategoryContainer = Content.Load<Texture2D>("UI/CharacterCreation/Tile01_Bigger");
			CategoryIcons = new Texture2D[10]
			{
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Gender"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Hair"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_HairColor"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Eyes"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Skin"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Shirt"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Undershirt"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Pants"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Shoes"),
				Content.Load<Texture2D>("UI/CharacterCreation/Category_Difficulty")
			};
			Sources = new Rectangle[HairTypes];
			for (int i = 0; i < HairTypes; i++)
			{
				ref Rectangle RectReference = ref Sources[i];
				RectReference = new Rectangle(0, 4, 40, 30);
			}
			ref Rectangle Type2Adjust = ref Sources[2];
			Type2Adjust = new Rectangle(0, 6, 40, 40);
			ref Rectangle Type8Adjust = ref Sources[8];
			Type8Adjust = new Rectangle(4, 4, 40, 30);
			ref Rectangle Type9Adjust = ref Sources[9];
			Type9Adjust = new Rectangle(0, 4, 35, 30);
			ref Rectangle Type10Adjust = ref Sources[10];
			Type10Adjust = new Rectangle(2, 4, 40, 28);
			ref Rectangle Type16Adjust = ref Sources[16];
			Type16Adjust = new Rectangle(2, 0, 30, 28);
			ref Rectangle Type17Adjust = ref Sources[17];
			Type17Adjust = new Rectangle(0, 6, 40, 32);
			ref Rectangle Type23Adjust = ref Sources[23];
			Type23Adjust = new Rectangle(0, 0, 40, 28);
			ref Rectangle Type26Adjust = ref Sources[26];
			Type26Adjust = new Rectangle(0, 4, 35, 30);
			SelectedHairContainer = Content.Load<Texture2D>("UI/CharacterCreation/Hair_Selector");
			HairContainer = Content.Load<Texture2D>("UI/CharacterCreation/Hair_Background");
			Palette = Content.Load<Texture2D>("UI/CharacterCreation/Palette");
			SelectedColorContainer = Content.Load<Texture2D>("UI/CharacterCreation/Color_Selector");
			MaleIcon = Content.Load<Texture2D>("UI/CharacterCreation/Gender_Male");
			FemaleIcon = Content.Load<Texture2D>("UI/CharacterCreation/Gender_Female");
			SoftcoreIcon = Content.Load<Texture2D>("UI/CharacterCreation/Button_Difficulty_Soft");
			MediumcoreIcon = Content.Load<Texture2D>("UI/CharacterCreation/Button_Difficulty_Med");
			HardcoreIcon = Content.Load<Texture2D>("UI/CharacterCreation/Button_Difficulty_Hard");
			SelectedHorizontalContainer = Content.Load<Texture2D>("UI/CharacterCreation/Hair_Selector");
			HorizontalContainer = Content.Load<Texture2D>("UI/CharacterCreation/Hair_Background");
			Frame = Content.Load<Texture2D>("UI/CharacterCreation/Frame01");
		}
	}
}
