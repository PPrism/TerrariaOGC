#if !USE_ORIGINAL_CODE
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Hardmode
{
	public class HardmodeLayout
	{
		private struct Image
		{
			public Vector2 Position;

			public Texture2D Texture;
		}

		private static readonly int MaxWidth = Terraria.UI.UsableWidth;

		private static readonly int MaxHeight = Terraria.UI.USABLE_HEIGHT;

		private readonly Image SideImage;

		private static Color TextColor = Color.White;

		private static Color AccentColor = new Color(255, 212, 64, 255);

		private static Color BackColor = new Color(42, 101, 43, 192); // Decided to swap Console's Green/Blue values for the BG colour, which matches Mobile more yet keeps Console's presentation.

		public HowToPlay.TextBlock Block;

		public static HardmodeLayout SideBySideLayout(string Text, int WidthOffset, Texture2D Image, int Padding = 20)
		{
			int TextBGBorderWidth = Assets.TextBGBorderWidth;
			CompiledText CompText = new CompiledText(Text, WidthOffset - TextBGBorderWidth * 2, Terraria.UI.BoldSmallTextStyle);
			int HeightOffset = Math.Min(CompText.Height + Terraria.UI.BoldSmallFont.LineSpacing + TextBGBorderWidth * 2, MaxHeight);
			int RectX = MaxWidth - (Image.Width + Padding + WidthOffset) >> 1;
			int RectY = MaxHeight - HeightOffset >> 1;
			Rectangle DialogArea = new Rectangle(RectX, RectY, WidthOffset, HeightOffset);
			Rectangle TextArea = DialogArea;
			TextArea.Inflate(-TextBGBorderWidth, -TextBGBorderWidth);
			HowToPlay.TextBlock TextBlock = new HowToPlay.TextBlock(ref DialogArea, CompText, ref TextArea, Assets.TextBG, TextBGBorderWidth, BackColor, TextColor, AccentColor);
			RectX += WidthOffset + Padding;
			RectY = MaxHeight - Image.Height >> 1;
			Image PreviewImage = new Image{ Position = new Vector2(RectX, RectY), Texture = Image };
			return new HardmodeLayout(TextBlock, PreviewImage);
		}

		private HardmodeLayout(HowToPlay.TextBlock Block, Image Image)
		{
			this.Block = Block;
			SideImage = Image;
		}

		public void Draw(int OffsetX, int OffsetY, int ScrollY)
		{
			Block.Draw(OffsetX, OffsetY, ScrollY);
			Image PreviewImage = SideImage;
			Vector2 ImagePos = PreviewImage.Position;
			ImagePos.X += OffsetX;
			ImagePos.Y += OffsetY;
			Main.SpriteBatch.Draw(PreviewImage.Texture, ImagePos, Color.White);
		}

		public void GenerateCache(GraphicsDevice GraphicsDevice)
		{
			Block.GenerateCache(GraphicsDevice);
		}
	}
}
#endif