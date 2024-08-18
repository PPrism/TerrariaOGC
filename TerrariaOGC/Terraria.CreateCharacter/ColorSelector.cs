using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class ColorSelector : ISelector
	{
		private const int ItemWidth = 16;
		private const int ItemHeight = 16;
		private const int SelectedItemWidth = 20; // These go unused due to the selected item having a calculated size from the bounds, which is 20x20
		private const int SelectedItemHeight = 20; // Could also be the 22s, stemming from '20 + (20 * 0.1) = 22'.
		private const int ItemSpacing = 6;

		private readonly Texture2D Palette, Picker;

		private Rectangle PaletteBounds, PickerBounds;

		private Vector2i Selected, RevertValue, ResetValue;

		private int FlashTimer;

		public Color SelectedColor;

		public ColorSelector(Texture2D Palette, Texture2D Picker, Vector2i ResetValue)
		{
			this.Palette = Palette;
			PaletteBounds = Palette.Bounds;
			this.Picker = Picker;
			PickerBounds = Picker.Bounds;
			Selected = ResetValue;
			this.ResetValue = ResetValue; // Yep, it set this variable twice.
			// this.ResetValue = ResetValue;
			UpdateColor();
		}

		public void Update()
		{
			if (FlashTimer > 0)
			{
				FlashTimer--;
			}
		}

		public void Draw(Vector2 Position, float Alpha)
		{
#if USE_ORIGINAL_CODE
			Rectangle PaletteRect = PaletteBounds;
			Rectangle SourceRect = new Rectangle(0, 0, 1, 1);
			Rectangle DestinationRect = new Rectangle(0, 0, ItemWidth, ItemHeight);
			Vector2 UnpickedPosition = default;
			Vector2i vector2i = new Vector2i(8, 8);
			int WidthAdjust = PaletteRect.Width * 22 - ItemSpacing;
			int HeightAdjust = PaletteRect.Height * 22 - ItemSpacing;
			Position.X -= WidthAdjust >> 1;
			Position.Y -= HeightAdjust >> 1;
			Position.X += 8f;
			Position.Y += 8f;
			Color WhiteOutline = Color.White * Alpha;
			Color BlackOutline = Color.Black * Alpha;

			for (int ColorInColumn = PaletteRect.Height - 1; ColorInColumn > -1; ColorInColumn--)
			{
				for (int ColorInRow = PaletteRect.Width - 1; ColorInRow > -1; ColorInRow--)
				{
					if (Selected.X != ColorInRow || Selected.Y != ColorInColumn)
					{
						SourceRect.X = ColorInRow;
						SourceRect.Y = ColorInColumn;
						DestinationRect.X = (int)Position.X + ColorInRow * 22 - vector2i.X;
						DestinationRect.Y = (int)Position.Y + ColorInColumn * 22 - vector2i.Y;
						Main.SpriteBatch.Draw(Palette, DestinationRect, (Rectangle?)SourceRect, WhiteOutline);
						UnpickedPosition.X = DestinationRect.X - (PickerBounds.Width - ItemWidth >> 1);
						UnpickedPosition.Y = DestinationRect.Y - (PickerBounds.Height - ItemHeight >> 1);
						Main.SpriteBatch.Draw(Picker, UnpickedPosition, BlackOutline);
					}
				}
			}

			Vector2 PickerPosition = default;
			PickerPosition.X = Position.X + Selected.X * 22;
			PickerPosition.Y = Position.Y + Selected.Y * 22;
			SourceRect.X = Selected.X;
			SourceRect.Y = Selected.Y;
			float Scale = (FlashTimer > 0) ? (1f + 0.1f * FlashTimer) : 1f;
			DestinationRect.X = (int)Position.X + Selected.X * 22 - (int)(vector2i.X * Scale);
			DestinationRect.Y = (int)Position.Y + Selected.Y * 22 - (int)(vector2i.Y * Scale);
			DestinationRect.Width = (int)(DestinationRect.Width * Scale);
			DestinationRect.Height = (int)(DestinationRect.Height * Scale);
			Main.SpriteBatch.Draw(Palette, DestinationRect, (Rectangle?)SourceRect, WhiteOutline);
			Main.SpriteBatch.Draw(Picker, PickerPosition, null, WhiteOutline, 0f, new Vector2(PickerBounds.Width >> 1, PickerBounds.Height >> 1), Scale, SpriteEffects.None, 0f);
#else
			Rectangle PaletteRect = PaletteBounds;
			Rectangle SourceRect = new Rectangle(0, 0, 1, 1);
			//SourceRect.Width >>= 1; // BUG: This SourceRect calculation is not in the original but it does fix a bug which causes the palette to be drawn as a gradient due to a larger selection for the palette. 
			//SourceRect.Height >>= 1; // This means the colours shown in each are not representative of what is there in the palette. This makes the selection more precise and thus, restricted to one colour.
			Rectangle DestinationRect = new Rectangle(0, 0, (int)(ItemWidth * Main.ScreenMultiplier), (int)(ItemHeight * Main.ScreenMultiplier));
			Vector2 UnpickedPosition = default;
			Vector2i vector2i = new Vector2i(8, 8);
			int Magnify = (int)(22 * Main.ScreenMultiplier);

			switch (Main.ScreenHeightPtr)
			{
				case 1:
					vector2i.X = (int)(vector2i.X * Main.ScreenMultiplier);
					vector2i.Y = (int)(vector2i.Y * Main.ScreenMultiplier);
					break;

				case 2:
					vector2i.X *= 2;
					vector2i.Y *= 2;
					break;
			}

			int WidthAdjust = PaletteRect.Width * Magnify - (int)(ItemSpacing * Main.ScreenMultiplier);
			int HeightAdjust = PaletteRect.Height * Magnify - (int)(ItemSpacing * Main.ScreenMultiplier);
			Position.X -= WidthAdjust >> 1;
			Position.Y -= HeightAdjust >> 1;
			Position.X += 8f * Main.ScreenMultiplier;
			Position.Y += 8f * Main.ScreenMultiplier;
			Color WhiteOutline = Color.White * Alpha;
			Color BlackOutline = Color.Black * Alpha;

			for (int ColorInColumn = PaletteRect.Height - 1; ColorInColumn > -1; ColorInColumn--)
			{
				for (int ColorInRow = PaletteRect.Width - 1; ColorInRow > -1; ColorInRow--)
				{
					if (Selected.X != ColorInRow || Selected.Y != ColorInColumn)
					{
						SourceRect.X = ColorInRow;
						SourceRect.Y = ColorInColumn;
						DestinationRect.X = (int)Position.X + ColorInRow * Magnify - vector2i.X;
						DestinationRect.Y = (int)Position.Y + ColorInColumn * Magnify - vector2i.Y;
						Main.SpriteBatch.Draw(Palette, DestinationRect, (Rectangle?)SourceRect, WhiteOutline);
						UnpickedPosition.X = DestinationRect.X - (PickerBounds.Width - (ItemWidth - (2 * Main.ScreenHeightPtr)) >> 1);
						UnpickedPosition.Y = DestinationRect.Y - (PickerBounds.Height - (ItemHeight - (2 * Main.ScreenHeightPtr)) >> 1);
						Main.SpriteBatch.Draw(Picker, UnpickedPosition, null, BlackOutline, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);
					}
				}
			}

			Vector2 PickerPosition = default;
			PickerPosition.X = Position.X + Selected.X * Magnify;
			PickerPosition.Y = Position.Y + Selected.Y * Magnify;
			SourceRect.X = Selected.X;
			SourceRect.Y = Selected.Y;
			float Scale = (FlashTimer > 0) ? (1f + 0.1f * FlashTimer) : 1f;
			DestinationRect.X = (int)Position.X + Selected.X * Magnify - (int)(vector2i.X * Scale);
			DestinationRect.Y = (int)Position.Y + Selected.Y * Magnify - (int)(vector2i.Y * Scale);
			DestinationRect.Width = (int)(DestinationRect.Width * Scale);
			DestinationRect.Height = (int)(DestinationRect.Height * Scale);
			Main.SpriteBatch.Draw(Palette, DestinationRect, (Rectangle?)SourceRect, WhiteOutline);
			Main.SpriteBatch.Draw(Picker, PickerPosition, null, WhiteOutline, 0f, new Vector2(PickerBounds.Width >> 1, PickerBounds.Height >> 1), Scale * Main.ScreenMultiplier, SpriteEffects.None, 0f);
#endif
		}

		private void UpdateColor()
		{
			Rectangle value = new Rectangle(Selected.X, Selected.Y, 1, 1);
			Color[] array = new Color[1];
			Palette.GetData(0, (Rectangle?)value, array, 0, 1);
			SelectedColor = array[0];
		}

		public bool SelectLeft()
		{
			if (Selected.X > 0)
			{
				Selected.X--;
			}
			else
			{
				Selected.X = PaletteBounds.Width - 1;
			}
			UpdateColor();
			return true;
		}

		public bool SelectRight()
		{
			if (Selected.X < PaletteBounds.Width - 1)
			{
				Selected.X++;
			}
			else
			{
				Selected.X = 0;
			}
			UpdateColor();
			return true;
		}

		public bool SelectUp()
		{
			if (Selected.Y > 0)
			{
				Selected.Y--;
			}
			else
			{
				Selected.Y = PaletteBounds.Height - 1;
			}
			UpdateColor();
			return true;
		}

		public bool SelectDown()
		{
			if (Selected.Y < PaletteBounds.Height - 1)
			{
				Selected.Y++;
			}
			else
			{
				Selected.Y = 0;
			}
			UpdateColor();
			return true;
		}

		public void SetCursor(Vector2i cursor)
		{
			Selected = cursor;
			UpdateColor();
		}

		public void Reset()
		{
			Selected = ResetValue;
			UpdateColor();
		}

		public void Show()
		{
			RevertValue = Selected;
			FlashTimer = 0;
		}

		public void FlashSelection(int duration)
		{
			FlashTimer = duration;
		}

		public void CancelSelection()
		{
			Selected = RevertValue;
			UpdateColor();
		}
	}
}
