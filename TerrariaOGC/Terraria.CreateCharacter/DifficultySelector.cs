using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class DifficultySelector : ISelector
	{
		private enum Direction
		{
			PREVIOUS = -1,
			NEXT = 1
		}

		private const float TransitionSpeed = 0.1f;

		private const int DifficultyTitleStartIdx = 24;
		private const int DifficultyDescStartIdx = 29;

		private const int DifficultySettings = 2;
		private static int ArrowOffset = 10;

#if USE_ORIGINAL_CODE
		private static Vector2 TitleOffset = new Vector2(0f, -50f);
		private static Vector2 DescOffset = new Vector2(0f, 50f);
		private static Vector2 SlideOffset = new Vector2(120f, 0f);
#else
		private static Vector2 TitleOffset = new Vector2(0f, -50f * Main.ScreenMultiplier);
		private static Vector2 DescOffset = new Vector2(0f, 50f * Main.ScreenMultiplier);
		private static Vector2 SlideOffset = new Vector2(120f * Main.ScreenMultiplier, 0f);
#endif

		public Difficulty Selected;

		private Difficulty ResetValue, PreviousSelected;

		private readonly Texture2D[] DifficultyIcons;

		private int FlashTimer;

		private float TransitionTween;

		private Direction TransitionDirection;

		public DifficultySelector(Texture2D[] Icons, Difficulty ResetDefault)
		{
			DifficultyIcons = Icons;
			ResetValue = ResetDefault;
			Selected = ResetDefault;
			TransitionTween = 0f;
		}

		public void Draw(Vector2 Position, float Alpha)
		{
			SpriteFont Font = Terraria.UI.BoldSmallFont;
			float Scale = 1f;
#if !USE_ORIGINAL_CODE
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					Scale = 1.1f;
					ArrowOffset = 18;
					break;

				case 2:
					Scale = 1.3f;
					ArrowOffset = 35;
					break;
			}
#endif
			if (TransitionTween > 0f)
			{
				float Visibility = 1f - Math.Min(TransitionTween, 0.5f) / 0.5f;
				float OldVisibility = (Math.Max(0.5f, TransitionTween) - 0.5f) / 0.5f;
				DrawDescription(PreviousSelected, Position, Scale, Alpha * OldVisibility);
				DrawDescription(Selected, Position, Scale, Alpha * Visibility);
				Vector2 OldTitleVector = Position + TitleOffset - SlideOffset * (1f - TransitionTween) * (int)TransitionDirection;
				DrawTitle(PreviousSelected, OldTitleVector, Scale, TransitionTween);
				Vector2 TitleVector = Position + TitleOffset + SlideOffset * TransitionTween * (int)TransitionDirection;
				DrawTitle(Selected, TitleVector, Scale, 1f - TransitionTween);
			}
			else
			{
				DrawTitle(Selected, Position + TitleOffset, Scale + 0.1f * FlashTimer, Alpha);
				string DifficultyName = Lang.MenuText[(int)(DifficultyTitleStartIdx + DifficultySettings - Selected)];
				Vector2 NamePivot = Terraria.UI.MeasureString(Font, DifficultyName);
				if (Alpha > 0.9f)
				{
					DrawArrows(Position + TitleOffset, new Vector2(NamePivot.X * 0.5f + ArrowOffset, 0f));
				}
				DrawDescription(Selected, Position, Scale, Alpha);
			}
		}

		private void DrawTitle(Difficulty Difficulty, Vector2 Position, float Scale, float Alpha)
		{
			SpriteFont Font = Terraria.UI.BoldSmallFont;
			string DifficultyName = Lang.MenuText[(int)(DifficultyTitleStartIdx + DifficultySettings - Difficulty)];
			Vector2 NamePivot = Terraria.UI.MeasureString(Font, DifficultyName);
			Color NameColor = (Difficulty != Difficulty.HARDCORE) ? Color.White : Color.Red; // Go scary if Hardcore is selected.

			NamePivot.X = (float)Math.Round(NamePivot.X * 0.5);
			NamePivot.Y = (float)Math.Round(NamePivot.Y * 0.5);
			Terraria.UI.DrawString(Font, DifficultyName, Position, NameColor * Alpha, 0f, NamePivot, Scale);
		}

		private void DrawArrows(Vector2 Position, Vector2 Spacing)
		{
			Vector2 LeftVector = Position - Spacing - new Vector2(8f, 8f);
			Vector2 RightVector = Position + Spacing - new Vector2(8f, 8f);
			Rectangle ArrowRect = new Rectangle((int)LeftVector.X, (int)LeftVector.Y, 16, 16);
#if USE_ORIGINAL_CODE
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowRect, SpriteEffects.FlipHorizontally);
			ArrowRect.X = (int)RightVector.X;
			ArrowRect.Y = (int)RightVector.Y;
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowRect);
#else
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowRect, Main.ScreenMultiplier, SpriteEffects.FlipHorizontally); // ArrowR is being drawn
			ArrowRect.X = (int)RightVector.X;
			ArrowRect.Y = (int)RightVector.Y;
			SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowRect, Main.ScreenMultiplier);
#endif
		}

		private void DrawDescription(Difficulty Difficulty, Vector2 Position, float Scale, float Alpha)
		{
			Texture2D DifficultyIcon = DifficultyIcons[(int)Difficulty];
			SpriteFont Font = Terraria.UI.BoldSmallFont;
			string DifficultyDesc = Lang.MenuText[(int)(DifficultyDescStartIdx + DifficultySettings - Difficulty)];
			Vector2 DescPivot = Terraria.UI.MeasureString(Font, DifficultyDesc);
			Vector2 DifficultyVector = new Vector2(DifficultyIcon.Width >> 1, DifficultyIcon.Height >> 1);

#if USE_ORIGINAL_CODE
			Main.SpriteBatch.Draw(DifficultyIcon, Position, null, Color.White * Alpha, 0f, DifficultyVector, 1f + 0.1f * FlashTimer, SpriteEffects.None, 0f);
#else
			Main.SpriteBatch.Draw(DifficultyIcon, Position, null, Color.White * Alpha, 0f, DifficultyVector, (1f * Main.ScreenMultiplier) + 0.1f * FlashTimer, SpriteEffects.None, 0f);
#endif
			DescPivot.X = (float)Math.Round(DescPivot.X * 0.5);
			DescPivot.Y = (float)Math.Round(DescPivot.Y * 0.5);
			Terraria.UI.DrawString(Font, DifficultyDesc, Position + DescOffset, Color.White * Alpha, 0f, DescPivot, Scale);
		}

		public void Update()
		{
			if (TransitionTween > 0f)
			{
				TransitionTween -= TransitionSpeed;
			}
			else
			{
				TransitionTween = 0f;
			}
			if (FlashTimer > 0)
			{
				FlashTimer--;
			}
		}

		public bool SelectLeft()
		{
			PreviousSelected = Selected;
			int NewSelected = (int)Selected;
			NewSelected--;
			if (NewSelected < 0)
			{
				NewSelected = 2;
			}
			Selected = (Difficulty)NewSelected;
			TransitionTween = 1f;
			TransitionDirection = Direction.PREVIOUS;
			return true;
		}

		public bool SelectRight()
		{
			PreviousSelected = Selected;
			int NewSelected = (int)Selected;
			NewSelected++;
			if (NewSelected > 2)
			{
				NewSelected = 0;
			}
			Selected = (Difficulty)NewSelected;
			TransitionTween = 1f;
			TransitionDirection = Direction.NEXT;
			return true;
		}

		public bool SelectUp()
		{
			return false;
		}

		public bool SelectDown()
		{
			return false;
		}

		public void SetCursor(Vector2i CursorVector)
		{
			int CursorSelection = Math.Max(CursorVector.X, 0);
			Selected = (Difficulty)Math.Min(CursorSelection, 2);
		}

		public void Reset()
		{
			Selected = ResetValue;
		}

		public void Show()
		{
			ResetValue = Selected;
			FlashTimer = 0;
		}

		public void FlashSelection(int Duration)
		{
			FlashTimer = Duration;
		}

		public void CancelSelection()
		{
			Selected = ResetValue;
		}
	}
}
