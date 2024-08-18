using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	internal class CategorySelector
	{
		private enum ScrollDirection
		{
			PREVIOUS = -1,
			NEXT = 1
		}

		private const float ScrollSpeed = 0.075f;

		private const float UnselectedSizeOffset = 0.75f;

		private readonly Texture2D[] Options;
		private readonly Texture2D Background, SelectionBackground;
		private Vector2 Spacing;

		public float ScrollTween;

		public bool Scrolling;

		private ScrollDirection ScrollDestination;

		private int PreviouslySelected, Selected; // PreviouslySelected appears to be set regularly, but goes unused.

		public int SelectedIndex
		{
			get
			{
				return Selected;
			}
			set
			{
				Selected = Math.Min(Options.Length - 1, Math.Max(0, value));
			}
		}

		public CategorySelector(Texture2D[] Icons, Texture2D Container, Texture2D SelectionContainer, Vector2 SpaceOffset)
		{
			Options = Icons;
			Background = Container;
			SelectionBackground = SelectionContainer;
			Spacing = SpaceOffset;
			Selected = 0;
			Scrolling = false;
			ScrollTween = 0f;
			ScrollDestination = ScrollDirection.PREVIOUS;
			PreviouslySelected = 0;
		}

		public void Update()
		{
			if (Scrolling)
			{
				ScrollTween -= ScrollSpeed;
				if (ScrollTween < 0f)
				{
					Scrolling = false;
					ScrollTween = 0f;
				}
			}
		}

		public void Draw(Vector2 Position)
		{
			Vector2 UnselectedVector = new Vector2(Background.Width >> 1, Background.Height >> 1);
			Vector2 SelectedVector = new Vector2(SelectionBackground.Width >> 1, SelectionBackground.Height >> 1);
			int MaxInDirection = 4; // Taking a wild stab with these 3 variables, its the best I can come up with considering everything.
			int MaxInPrevDirection = MaxInDirection + 1;
			int EntryCount = MaxInDirection * 2 + 1;

			for (int CategoryInList = EntryCount; CategoryInList > 0; CategoryInList--)
			{
				int Icon = Selected - MaxInPrevDirection + CategoryInList;
				float Visibility = MaxInDirection - Math.Abs(CategoryInList - MaxInPrevDirection + ScrollTween * (float)ScrollDestination);
				float ScaleAddition = 1f - 0.25f * Math.Abs(CategoryInList - MaxInPrevDirection + ScrollTween * (float)ScrollDestination);
				ScaleAddition = Math.Max(UnselectedSizeOffset, ScaleAddition);

				if (Icon < 0)
				{
					Icon += Options.Length;
				}
				else if (Icon >= Options.Length)
				{
					Icon -= Options.Length;
				}

				if (!(Visibility < 0f) && !(ScaleAddition < 0f))
				{
					Texture2D CategoryIcon = Options[Icon];
					Vector2 IconOrigin = new Vector2(CategoryIcon.Width >> 1, CategoryIcon.Height >> 1);
					Vector2 ScrollResult = Spacing * ScrollTween * (float)ScrollDestination;
					Vector2 IconPosition = Position + Spacing * (CategoryInList - MaxInPrevDirection) + ScrollResult;

#if USE_ORIGINAL_CODE
					if (Icon == Selected)
					{
						Main.SpriteBatch.Draw(SelectionBackground, IconPosition, null, Color.White * Visibility, 0f, SelectedVector, ScaleAddition, SpriteEffects.None, 0f);
					}
					else
					{
						Main.SpriteBatch.Draw(Background, IconPosition, null, Color.White * Visibility, 0f, UnselectedVector, 1f, SpriteEffects.None, 0f);
					}
					Main.SpriteBatch.Draw(CategoryIcon, IconPosition, null, Color.White * Visibility, 0f, IconOrigin, 1f, SpriteEffects.None, 0f);
#else
					if (Icon == Selected)
					{
						Main.SpriteBatch.Draw(SelectionBackground, IconPosition, null, Color.White * Visibility, 0f, SelectedVector, ScaleAddition * Main.ScreenMultiplier, SpriteEffects.None, 0f);
					}
					else
					{
						Main.SpriteBatch.Draw(Background, IconPosition, null, Color.White * Visibility, 0f, UnselectedVector, 1f * Main.ScreenMultiplier, SpriteEffects.None, 0f);
					}
					Main.SpriteBatch.Draw(CategoryIcon, IconPosition, null, Color.White * Visibility, 0f, IconOrigin, 1f * Main.ScreenMultiplier, SpriteEffects.None, 0f);
#endif
				}
			}

			if (!Scrolling)
			{
				Vector2 Distance = Spacing * (MaxInDirection - 0.25f);
				Vector2 LeftArrowPos = Position - Distance - new Vector2(8f, 8f);
				Rectangle ArrowPosition = new Rectangle((int)LeftArrowPos.X, (int)LeftArrowPos.Y, 16, 16);
#if USE_ORIGINAL_CODE
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition, SpriteEffects.FlipHorizontally);
				Vector2 RightArrowPos = Position + Distance - new Vector2(8f, 8f);
				ArrowPosition.X = (int)RightArrowPos.X;
				ArrowPosition.Y = (int)RightArrowPos.Y;
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition);
#else
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition, Main.ScreenMultiplier, SpriteEffects.FlipHorizontally); // These are for the 2 blue arrows going left and right.
				Vector2 RightArrowPos = Position + Distance - new Vector2(8f, 8f);
				ArrowPosition.X = (int)RightArrowPos.X;
				ArrowPosition.Y = (int)RightArrowPos.Y;
				SpriteSheet<_sheetSprites>.DrawCentered((int)_sheetSprites.ID.ARROWR, ref ArrowPosition, Main.ScreenMultiplier);
#endif
			}
		}

		private void StartScrolling(ScrollDirection Direction)
		{
			Scrolling = true;
			ScrollTween = 1f;
			ScrollDestination = Direction;
		}

		public bool SelectNext()
		{
			bool CanGoNext = false;
			if (!Scrolling)
			{
				PreviouslySelected = Selected;
				Selected++;
				if (Selected >= Options.Length)
				{
					Selected -= Options.Length;
				}
				CanGoNext = true;
				StartScrolling(ScrollDirection.NEXT);
			}
			return CanGoNext;
		}

		public bool SelectPrevious()
		{
			bool CanGoPrevious = false;
			if (!Scrolling)
			{
				PreviouslySelected = Selected;
				Selected--;
				if (Selected < 0)
				{
					Selected += Options.Length;
				}
				CanGoPrevious = true;
				StartScrolling(ScrollDirection.PREVIOUS);
			}
			return CanGoPrevious;
		}
	}
}
