using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.HowToPlay
{
	public class HowToPlayLayout
	{
		private struct Image
		{
			public Vector2 Position;

			public Texture2D Texture;
		}

		private static readonly int MaxWidth = Terraria.UI.UsableWidth;

		private static readonly int MaxHeight = Terraria.UI.USABLE_HEIGHT;

		private readonly Image[] HowToPlayAssets;

		private static Color TextColor = Color.White;

		private static Color AccentColor = new Color(255, 212, 64, 255);

		private static Color BackColor = Terraria.UI.DefaultDialogColor;

		public TextBlock Block;

		public static HowToPlayLayout TextOnlyLayout(string PageText, int WidthOffset)
		{
			int TextBGBorderWidth = Assets.TextBGBorderWidth;
			CompiledText CompText = new CompiledText(PageText, WidthOffset - TextBGBorderWidth * 2, Terraria.UI.BoldSmallTextStyle);
			int HeightOffset = Math.Min(CompText.Height + Terraria.UI.BoldSmallFont.LineSpacing + TextBGBorderWidth * 2, MaxHeight);
			int RectX = MaxWidth - WidthOffset >> 1;
			int RectY = MaxHeight - HeightOffset >> 1;
			Rectangle DialogArea = new Rectangle(RectX, RectY, WidthOffset, HeightOffset);
			Rectangle TextArea = DialogArea;
			TextArea.Inflate(-TextBGBorderWidth, -TextBGBorderWidth);
			TextBlock textBlock = new TextBlock(ref DialogArea, CompText, ref TextArea, Assets.TextBG, TextBGBorderWidth, BackColor, TextColor, AccentColor);
			return new HowToPlayLayout(textBlock);
		}

		public static HowToPlayLayout SideBySideLayout(string Text, int WidthOffset, Texture2D Image, int Padding = 20)
		{
			int TextBGBorderWidth = Assets.TextBGBorderWidth;
			CompiledText CompText = new CompiledText(Text, WidthOffset - TextBGBorderWidth * 2, Terraria.UI.BoldSmallTextStyle);
			int HeightOffset = Math.Min(CompText.Height + Terraria.UI.BoldSmallFont.LineSpacing + TextBGBorderWidth * 2, MaxHeight);
			int RectX = MaxWidth - (Image.Width + Padding + WidthOffset) >> 1;
			int RectY = MaxHeight - HeightOffset >> 1;
			Rectangle DialogArea = new Rectangle(RectX, RectY, WidthOffset, HeightOffset);
			Rectangle TextArea = DialogArea;
			TextArea.Inflate(-TextBGBorderWidth, -TextBGBorderWidth);
			TextBlock TextBlock = new TextBlock(ref DialogArea, CompText, ref TextArea, Assets.TextBG, TextBGBorderWidth, BackColor, TextColor, AccentColor);
			RectX += WidthOffset + Padding;
			RectY = MaxHeight - Image.Height >> 1;
			Image[] PreviewImage = new Image[1]
			{
				new Image
				{
					Position = new Vector2(RectX, RectY),
					Texture = Image
				}
			};
			return new HowToPlayLayout(TextBlock, PreviewImage);
		}

		public static HowToPlayLayout StackedLayout(string Text, int DialogWidth, int DialogHeight, Texture2D Image, int Padding = 20)
		{
#if !USE_ORIGINAL_CODE
			if (Main.ScreenHeightPtr == 2)
			{
				DialogWidth += 15; // 1080p has the width as 675 and not 660
			}
#endif
			int TextBGBorderWidth = Assets.TextBGBorderWidth;
			CompiledText CompText = new CompiledText(Text, DialogWidth - TextBGBorderWidth * 2, Terraria.UI.BoldSmallTextStyle); 
			DialogHeight = Math.Min(CompText.Height + Terraria.UI.BoldSmallFont.LineSpacing + TextBGBorderWidth * 2, DialogHeight);
			int RectX = MaxWidth - CompText.Width >> 1;
			int RectY = MaxHeight - (DialogHeight + Padding + Image.Height) >> 1;
			Rectangle DialogArea = new Rectangle(RectX, RectY, DialogWidth, DialogHeight);
			Rectangle TextArea = DialogArea;
			TextArea.Inflate(-TextBGBorderWidth, -TextBGBorderWidth);
			TextBlock TextBlock = new TextBlock(ref DialogArea, CompText, ref TextArea, Assets.TextBG, TextBGBorderWidth, BackColor, TextColor, AccentColor);
			RectX = MaxWidth - Image.Width >> 1;
			RectY += DialogHeight + Padding;
			Image[] PreviewImage = new Image[1]
			{
				new Image
				{
					Position = new Vector2(RectX, RectY),
					Texture = Image
				}
			};
			return new HowToPlayLayout(TextBlock, PreviewImage);
		}

		private HowToPlayLayout(TextBlock Block)
		{
			this.Block = Block;
			HowToPlayAssets = new Image[0];
		}

		private HowToPlayLayout(TextBlock Block, Image[] Previews)
		{
			this.Block = Block;
			HowToPlayAssets = Previews;
		}

		public void Draw(int OffsetX, int OffsetY, int ScrollY)
		{
			Block.Draw(OffsetX, OffsetY, ScrollY);
			Image[] Images = HowToPlayAssets;
			for (int i = 0; i < Images.Length; i++)
			{
				Image PreviewImage = Images[i];
				Vector2 ImagePos = PreviewImage.Position;
				ImagePos.X += OffsetX;
				ImagePos.Y += OffsetY;
				Main.SpriteBatch.Draw(PreviewImage.Texture, ImagePos, Color.White);
			}
		}

		public void GenerateCache(GraphicsDevice GraphicsDevice)
		{
			Block.GenerateCache(GraphicsDevice);
		}
	}
}
