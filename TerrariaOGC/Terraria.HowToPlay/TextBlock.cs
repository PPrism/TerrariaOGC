using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.HowToPlay
{
	public class TextBlock
	{
		private readonly BoxGraphic Background;

		private readonly CompiledText CompText;

		public bool IsScrollable;

		public short MinOffsetY;

		private Texture2D TextTexture;

		private Color TextColor, AccentColor ,BackColor;

		private Rectangle TextArea, BGDet;

		private Vector2i DialogPosition;

		public TextBlock(ref Rectangle DialogArea, CompiledText Text, ref Rectangle TextArea, Texture2D BG, int BorderWidth, Color BackColor, Color TextColor, Color AccentColor)
		{
			Background = BoxGraphic.Create(DialogArea.Width, DialogArea.Height, BG, BorderWidth, BackColor);
			DialogPosition = new Vector2i(DialogArea.X, DialogArea.Y);
			CompText = Text;
			this.TextArea = TextArea;
			IsScrollable = Text.Height > TextArea.Height;
			MinOffsetY = (short)(-Math.Max(0, Text.Height - TextArea.Height));
			this.TextColor = TextColor;
			this.AccentColor = AccentColor;
			TextTexture = null;

#if !USE_ORIGINAL_CODE
			BGDet = DialogArea;
			this.BackColor = BackColor;
#endif
		}


#if USE_ORIGINAL_CODE
		public void Draw(int FrontX = 0, int FrontY = 0, int ScrollY = 0)
		{
			Rectangle BackLocation = TextArea;
			BackLocation.X = 0;
			BackLocation.Y = -ScrollY;
			Rectangle FrontLocation = TextArea;
			FrontLocation.X += FrontX;
			FrontLocation.Y += FrontY;
			Vector2i Position = DialogPosition;
			Position.X += FrontX;
			Position.Y += FrontY;
			Background.Draw(Position, 1f);

			Main.SpriteBatch.Draw(TextTexture, FrontLocation, (Rectangle?)BackLocation, Color.White);
			if (IsScrollable)
			{
				int Xpos = Position.X + (Background.Width >> 1) - 8;
				Rectangle rect = new Rectangle(Xpos, Position.Y + 2, 16, 16);
				if (ScrollY < 0)
				{
					SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect, SpriteEffects.FlipVertically);
				}
				if (ScrollY > MinOffsetY)
				{
					rect.Y = Position.Y + Background.Height - 16 - 2;
					SpriteSheet<_sheetSprites>.DrawCentered(135, ref rect);
				}
			}
		}
#else
		public void Draw(int FrontX = 0, int FrontY = 0, int ScrollY = 0)
		{
			Rectangle BackLocation = TextArea;
			BackLocation.X = 0;
			BackLocation.Y = -ScrollY;
			Rectangle FrontLocation;
			Vector2 FinalPosition = default;

#if VERSION_INITIAL
			if (Main.ScreenHeightPtr == 0) 
			{
				FrontLocation = TextArea;
				FrontLocation.X += FrontX;
				FrontLocation.Y += FrontY;
				Vector2i Position = DialogPosition;
				Position.X += FrontX;
				Position.Y += FrontY;
				Background.Draw(Position, 1f);
				FinalPosition.X = Position.X;
				FinalPosition.Y = Position.Y;
			}
			else
			{
#endif
				BackLocation.Height += 8; // With the same change to FrontLocation.Height, changes the scrollbar height.

				Vector2 Position = new Vector2(DialogPosition.X, DialogPosition.Y);
				Position.X += FrontX;
				Position.Y += FrontY;

				BackColor.A = 196;
				BackColor.R = (byte)(BackColor.A >> 0);
				BackColor.G = (byte)(BackColor.A >> 0);
				BackColor.B = (byte)(BackColor.A >> 0);

				Rectangle NewShape = BGDet;
				if (!IsScrollable)
				{
					Position.Y += 4 * Main.ScreenMultiplier;
					NewShape.Height -= 8;
				}

                if (Terraria.UI.MainUI.CurMenuMode == MenuMode.HARDMODE_UPSELL)
                {
					Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK3, Position, NewShape, BackColor);
				}
				else
				{
					Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, Position, NewShape, BackColor);
				}

				FrontLocation = TextArea;
				FrontLocation.X += FrontX - 6; // Changes the X/Y top-left position of the text itself.
				FrontLocation.Y += FrontY - 6;
				FrontLocation.Height += 8;

				if (!IsScrollable)
				{
					FrontLocation.Y += 6;
					FrontLocation.Width += 8;
				}
				FinalPosition.X = Position.X;
				FinalPosition.Y = Position.Y;
#if VERSION_INITIAL
			}
#endif

			Main.SpriteBatch.Draw(TextTexture, FrontLocation, (Rectangle?)BackLocation, Color.White);
			if (IsScrollable)
			{
				int Xpos = (int)(FinalPosition.X + (Background.Width >> 1) - 8);
				Rectangle rect = new Rectangle(Xpos, (int)(FinalPosition.Y + 2), 16, 16);
				if (ScrollY < 0)
				{
					SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWD, ref rect, SpriteEffects.FlipVertically);
				}
				if (ScrollY > MinOffsetY)
				{
					rect.Y = (int)(FinalPosition.Y + Background.Height - 16 - 2);
					SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWD, ref rect);
				}
			}
		}
#endif

		public void GenerateCache(GraphicsDevice GraphicsDevice)
		{
			RenderTarget2D Target = new RenderTarget2D(GraphicsDevice, CompText.Width, CompText.Height + Terraria.UI.BoldSmallFont.LineSpacing);
			GraphicsDevice.SetRenderTarget(Target);
			GraphicsDevice.Clear(Color.Transparent);
			Main.SpriteBatch.Begin();
			CompText.Draw(Main.SpriteBatch, new Rectangle(0, 0, CompText.Width, CompText.Height), TextColor, AccentColor);
			Main.SpriteBatch.End();
			GraphicsDevice.SetRenderTarget(null);
			TextTexture = Target;
		}
	}
}
