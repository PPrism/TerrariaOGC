using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	internal class PlayerModifier
	{
		public static void Hair(Player SelPlayer, int ModValue)
		{
			SelPlayer.hair = (byte)ModValue;
		}

		public static void HairColor(Player SelPlayer, Color ModValue)
		{
			SelPlayer.hairColor = ModValue;
		}

		public static void Shirt(Player SelPlayer, Color ModValue)
		{
			SelPlayer.shirtColor = ModValue;
		}

		public static void Undershirt(Player SelPlayer, Color ModValue)
		{
			SelPlayer.underShirtColor = ModValue;
		}

		public static void Pants(Player SelPlayer, Color ModValue)
		{
			SelPlayer.pantsColor = ModValue;
		}

		public static void Shoes(Player SelPlayer, Color ModValue)
		{
			SelPlayer.shoeColor = ModValue;
		}

		public static void Gender(Player SelPlayer, bool ModValue)
		{
			SelPlayer.male = ModValue;
		}

		public static void Difficulty(Player SelPlayer, byte ModValue)
		{
			SelPlayer.difficulty = ModValue;
		}

		public static void Eyes(Player SelPlayer, Color ModValue)
		{
			SelPlayer.eyeColor = ModValue;
		}

		public static void Skin(Player SelPlayer, Color ModValue)
		{
			SelPlayer.skinColor = ModValue;
		}
	}
}
