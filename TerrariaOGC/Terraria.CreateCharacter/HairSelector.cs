using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class HairSelector : ISelector
	{
		private const int Spacing = 2;

		private readonly Texture2D Container, Picker;

		private readonly Vector2 PickerOrigin;

		private readonly int Rows, Columns;

		private readonly Rectangle[] Slots;

		private int FlashTimer;

		private Vector2i SelectedVect, RevertValue, ResetValue;

		public int Selected => SelectedVect.X + SelectedVect.Y * Columns;

		public HairSelector(int Columns, Rectangle[] Sources, Texture2D Background, Texture2D Picker, Vector2i Default)
		{
			Container = Background;
			this.Picker = Picker;
			PickerOrigin = new Vector2(Picker.Bounds.Width >> 1, Picker.Bounds.Height >> 1);
			this.Columns = Columns;
			Rows = Sources.Length / Columns;
			Slots = Sources;
			SelectedVect = Default;
			RevertValue = Default;
			ResetValue = Default;
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
			Rectangle Bounds = Container.Bounds;
			Color Color = Color.White * Alpha;
			Vector2 SelectedPos = default;
			Vector2 UnselectedPos = default;

#if USE_ORIGINAL_CODE
			int RowSlotSize = Bounds.Width + Spacing;
			int ColumnSlotSize = Bounds.Height + Spacing;
			int HalfWidth = Bounds.Width >> 1;
			int HalfHeight = Bounds.Height >> 1;
#else
			int RowSlotSize = (int)((Bounds.Width + Spacing) * Main.ScreenMultiplier);
			int ColumnSlotSize = (int)((Bounds.Height + Spacing) * Main.ScreenMultiplier);
			int HalfWidth = (int)((Bounds.Width >> 1) * Main.ScreenMultiplier);
			int HalfHeight = (int)((Bounds.Height >> 1) * Main.ScreenMultiplier);
#endif


			int RowArea = RowSlotSize * Columns - Spacing;
			int ColumnArea = ColumnSlotSize * Rows - Spacing;

			Position.X -= RowArea >> 1;
			Position.Y -= ColumnArea >> 1;
			Position.X += HalfWidth;
			Position.Y += HalfHeight;

			for (int HairInRow = Rows - 1; HairInRow > -1; HairInRow--)
			{
				int UnselectedY = (int)Position.Y + HairInRow * ColumnSlotSize;
				for (int HairInColumn = Columns - 1; HairInColumn > -1; HairInColumn--)
				{
					if (HairInRow != SelectedVect.Y || HairInColumn != SelectedVect.X)
					{
						int UnselectedX = (int)Position.X + HairInColumn * RowSlotSize;
						int Unselected = HairInRow * Columns + HairInColumn;
						UnselectedPos.X = UnselectedX - HalfWidth;
						UnselectedPos.Y = UnselectedY - HalfHeight;
#if USE_ORIGINAL_CODE
						Main.SpriteBatch.Draw(Container, UnselectedPos, Color);
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.PLAYER_HAIR_1 + Unselected, UnselectedX, UnselectedY, Slots[Unselected], Color);
#else
						Main.SpriteBatch.Draw(Container, UnselectedPos, null, Color, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);
						SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.PLAYER_HAIR_1 + Unselected, UnselectedX, UnselectedY, Slots[Unselected], Color, Main.ScreenMultiplier);
#endif
					}
				}
			}

			float Scale = (FlashTimer > 0) ? (1f + 0.1f * FlashTimer) : 1f;
			SelectedPos.X = Position.X + SelectedVect.X * RowSlotSize;
			SelectedPos.Y = Position.Y + SelectedVect.Y * ColumnSlotSize;
#if USE_ORIGINAL_CODE
			Main.SpriteBatch.Draw(Picker, SelectedPos, null, Color, 0f, PickerOrigin, Scale, SpriteEffects.None, 0f);
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.PLAYER_HAIR_1 + Selected, (int)SelectedPos.X, (int)SelectedPos.Y, Slots[Selected], Color, Scale);
#else
			Main.SpriteBatch.Draw(Picker, SelectedPos, null, Color, 0f, PickerOrigin, Scale * Main.ScreenMultiplier, SpriteEffects.None, 0f);
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.PLAYER_HAIR_1 + Selected, (int)SelectedPos.X, (int)SelectedPos.Y, Slots[Selected], Color, Scale * Main.ScreenMultiplier);
#endif
		}

		public bool SelectLeft()
		{
			if (SelectedVect.X > 0)
			{
				SelectedVect.X--;
			}
			else
			{
				SelectedVect.X = Columns - 1;
			}
			return true;
		}

		public bool SelectRight()
		{
			if (SelectedVect.X < Columns - 1)
			{
				SelectedVect.X++;
			}
			else
			{
				SelectedVect.X = 0;
			}
			return true;
		}

		public bool SelectUp()
		{
			if (SelectedVect.Y > 0)
			{
				SelectedVect.Y--;
			}
			else
			{
				SelectedVect.Y = Rows - 1;
			}
			return true;
		}

		public bool SelectDown()
		{
			if (SelectedVect.Y < Rows - 1)
			{
				SelectedVect.Y++;
			}
			else
			{
				SelectedVect.Y = 0;
			}
			return true;
		}

		public void SetCursor(Vector2i CursorVect)
		{
			SelectedVect = CursorVect;
		}

		public void Reset()
		{
			SelectedVect = ResetValue;
		}

		public void Show()
		{
			RevertValue = SelectedVect;
			FlashTimer = 0;
		}

		public void FlashSelection(int Duration)
		{
			FlashTimer = Duration;
		}

		public void CancelSelection()
		{
			SelectedVect = RevertValue;
		}
	}
}
