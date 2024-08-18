using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.CreateCharacter
{
	public class HorizontalListSelector : ISelector
	{
		private const int Spacing = 4;

		private readonly Texture2D[] Options;

		private readonly Texture2D Background;

		private Vector2 BackgroundOrigin;

		private readonly Texture2D Picker;

		private Vector2 PickerOrigin;

		private int FlashTimer;

		public int Selected;

		private int RevertValue;

		private readonly int DefResetValue;

		public HorizontalListSelector(Texture2D[] OptionTexArr, Texture2D BGTex, Texture2D PickerTex, int ResetValue)
		{
			Background = BGTex;
			BackgroundOrigin = new Vector2(BGTex.Width >> 1, BGTex.Height >> 1);
			Options = OptionTexArr;
			Picker = PickerTex;
			PickerOrigin = new Vector2(PickerTex.Bounds.Width >> 1, PickerTex.Bounds.Height >> 1);
			Selected = ResetValue;
			RevertValue = ResetValue;
			DefResetValue = ResetValue;
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
			float DefaultScale = 2f;
#if USE_ORIGINAL_CODE
			float Scale = (FlashTimer > 0) ? (2f + 0.1f * FlashTimer) : 2f;
			float Offset = Background.Width * 2f + 4f;
#else
			float Scale = ((FlashTimer > 0) ? (2f + 0.1f * FlashTimer) : 2f) * Main.ScreenMultiplier;
			float Offset = (Background.Width * 2f + 4f) * Main.ScreenMultiplier;
			if (Main.ScreenHeightPtr != 0)
			{
				DefaultScale *= Main.ScreenMultiplier;
			}
#endif
			Color Color = Color.White * Alpha;
			int AdjustX = (Options.Length - 1) * (int)Offset - Spacing;
			Position.X -= AdjustX >> 1;
			Vector2 PosVect = Vector2.Zero;
			Vector2 TexOrigin = Vector2.Zero;
			Texture2D OptionTex = null;
			for (int OptionIdx = 0; OptionIdx < Options.Length; OptionIdx++)
			{
				Texture2D SelOptionTex = Options[OptionIdx];
				Vector2 SelOrigin = new Vector2(SelOptionTex.Width >> 1, SelOptionTex.Height >> 1);
				if (OptionIdx == Selected)
				{
					OptionTex = SelOptionTex;
					TexOrigin = SelOrigin;
					PosVect = Position;
				}
				else
				{
					Main.SpriteBatch.Draw(Background, Position, null, Color, 0f, BackgroundOrigin, DefaultScale, SpriteEffects.None, 0f);
					Main.SpriteBatch.Draw(SelOptionTex, Position, null, Color, 0f, SelOrigin, DefaultScale, SpriteEffects.None, 0f);
				}
				Position.X += Offset;
			}
			if (OptionTex != null)
			{
				Main.SpriteBatch.Draw(Picker, PosVect, null, Color, 0f, PickerOrigin, Scale, SpriteEffects.None, 0f);
				Main.SpriteBatch.Draw(OptionTex, PosVect, null, Color, 0f, TexOrigin, Scale, SpriteEffects.None, 0f);
			}
		}

		public bool SelectLeft()
		{
			if (Selected > 0)
			{
				Selected--;
			}
			else
			{
				Selected = Options.Length - 1;
			}
			return true;
		}

		public bool SelectRight()
		{
			if (Selected < Options.Length - 1)
			{
				Selected++;
			}
			else
			{
				Selected = 0;
			}
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

		public void Reset()
		{
			Selected = DefResetValue;
		}

		public void Show()
		{
			RevertValue = Selected;
			FlashTimer = 0;
		}

		public void FlashSelection(int Duration)
		{
			FlashTimer = Duration;
		}

		public void CancelSelection()
		{
			Selected = RevertValue;
		}

		public void SetCursor(Vector2i Cursor)
		{
			int Setting = Cursor.X;
			if (Setting < 0)
			{
				Setting = 0;
			}
			else if (Setting > Options.Length - 1)
			{
				Setting = Options.Length - 1;
			}
			Selected = Setting;
		}
	}
}
