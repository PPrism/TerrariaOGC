using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public static class AnimationUtils
	{
		public enum SqueezeTextFlags
		{
			NoWordBreak,
			WordBreak
		}

		public static Color StringToColor(string Text)
		{
			if (Text.Length == 7 && Text[0] == '#')
			{
				int HexR = HexValue(Text[1]) * 0x10 + HexValue(Text[2]);
				int HexG = HexValue(Text[3]) * 0x10 + HexValue(Text[4]);
				int HexB = HexValue(Text[5]) * 0x10 + HexValue(Text[6]);
				return new Color(HexR, HexG, HexB);
			}
			if (Text.Length >= 11 && Text[0] == '{' && Text[Text.Length - 1] == '}')
			{
				int HexR = 0;
				int HexG = 0;
				int HexB = 0;
				int HexA = 0;
				int CharIdx = 1;
				while (CharIdx + 2 < Text.Length && (char.ToLower(Text[CharIdx]) == 'a' || char.ToLower(Text[CharIdx]) == 'r' || char.ToLower(Text[CharIdx]) == 'g' || char.ToLower(Text[CharIdx]) == 'b') && Text[CharIdx + 1] == ':')
				{
					char ColourPointer = char.ToLower(Text[CharIdx]);
					int ByteValue = 0;
					for (CharIdx += 2; char.IsDigit(Text[CharIdx]); CharIdx++)
					{
						ByteValue = ByteValue * 10 + (Text[CharIdx] - 0x30);
					}
					switch (ColourPointer)
					{
					case 'r':
						HexR = ByteValue;
						break;
					case 'g':
						HexG = ByteValue;
						break;
					case 'b':
						HexB = ByteValue;
						break;
					default:
						HexA = ByteValue;
						break;
					}
					for (; Text[CharIdx] == ' '; CharIdx++)
					{
					}
				}
				return new Color(HexR, HexG, HexB, HexA);
			}
			return Color.White;
		}

		private static int HexValue(char TextChar)
		{
			if (TextChar < '0' || TextChar > '9')
			{
				if (TextChar < 'a' || TextChar > 'z')
				{
					if (TextChar < 'A' || TextChar > 'Z')
					{
						return 0;
					}
					return TextChar - 0x41 + 0xA;
				}
				return TextChar - 0x61 + 0xA;
			}
			return TextChar - 0x30;
		}

		public static int SqueezeText(SpriteFont Font, string Text, int Width, SqueezeTextFlags ActiveFlags)
		{
			if (Width <= 0 || Text.Length == 0)
			{
				return 0;
			}
			if ((int)UI.MeasureString(Font, Text).X + UI.Spacing(Font) <= Width)
			{
				return Text.Length;
			}
			int ReducedLength = 0;
			int ActiveLength = Text.Length;
			do
			{
				int NewLength = ReducedLength + (ActiveLength - ReducedLength + 1 >> 1);
				if ((int)UI.MeasureString(Font, Text.Substring(0, NewLength)).X + UI.Spacing(Font) <= Width)
				{
					ReducedLength = NewLength;
				}
				else
				{
					ActiveLength = NewLength;
				}
			}
			while (ActiveLength > ReducedLength + 1);
			if ((ActiveFlags & SqueezeTextFlags.WordBreak) != 0)
			{
				while (ReducedLength >= 1)
				{
					char SlimTextChar = Text[ReducedLength - 1];
					if (SlimTextChar == ' ' || SlimTextChar == '\t' || SlimTextChar == '\r' || SlimTextChar == '\n')
					{
						break;
					}
					ReducedLength--;
				}
			}
			return ReducedLength;
		}
	}
}
