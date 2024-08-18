using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terraria.CreateCharacter
{
	public class UI
	{
		private const int FlashDuration = 10;

		private readonly Terraria.UI ParentUI;

		private readonly CategorySelector Widget;

		private readonly IAttributeWidget[] Attributes;

		private int UIDelay;

		private readonly FastRandom RandGen;

		private Player SelPlayer;

		private IAttributeWidget PrevAttribute;

		private string Header;

		private Vector2 HeaderOrigin;

		private string Footer;

		private Vector2 FooterOrigin;

		private readonly SpriteFont Font;

		private readonly float FontScale;

		private IAttributeWidget SelectedAttribute => Attributes[Widget.SelectedIndex];

		private UI(Terraria.UI Base, CategorySelector CatWidget, IAttributeWidget[] CatAttributes)
		{
			ParentUI = Base;
			Widget = CatWidget;
			Attributes = CatAttributes;
			UIDelay = 0;
			RandGen = new FastRandom();
			Font = Terraria.UI.BigFont;
			FontScale = 0.525f;
		}

		public static UI Create(Terraria.UI Base)
		{
#if USE_ORIGINAL_CODE
			CategorySelector CatSelector = new CategorySelector(Assets.CategoryIcons, Assets.CategoryContainer, Assets.SelectedCategoryContainer, new Vector2(54f, 0f));
#else
			CategorySelector CatSelector = new CategorySelector(Assets.CategoryIcons, Assets.CategoryContainer, Assets.SelectedCategoryContainer, new Vector2(54f * Main.ScreenMultiplier, 0f));
#endif
			IAttributeWidget[] CharSettings = new IAttributeWidget[10];
			CharSettings[1] = HairAttributeWidget.Create(PlayerModifier.Hair, new Vector2i(4, 1), Lang.Controls(Lang.CONTROLS.HAIR_TYPE), Lang.Controls(Lang.CONTROLS.SELECT_TYPE));
			CharSettings[2] = ColorAttributeWidget.Create(PlayerModifier.HairColor, new Vector2i(4, 6), Lang.Controls(Lang.CONTROLS.HAIR_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[5] = ColorAttributeWidget.Create(PlayerModifier.Shirt, new Vector2i(4, 2), Lang.Controls(Lang.CONTROLS.SHIRT_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[6] = ColorAttributeWidget.Create(PlayerModifier.Undershirt, new Vector2i(2, 6), Lang.Controls(Lang.CONTROLS.UNDERSHIRT_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[7] = ColorAttributeWidget.Create(PlayerModifier.Pants, new Vector2i(9, 7), Lang.Controls(Lang.CONTROLS.PANTS_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[8] = ColorAttributeWidget.Create(PlayerModifier.Shoes, new Vector2i(0, 0), Lang.Controls(Lang.CONTROLS.SHOE_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[3] = ColorAttributeWidget.Create(PlayerModifier.Eyes, new Vector2i(9, 2), Lang.Controls(Lang.CONTROLS.EYE_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[4] = ColorAttributeWidget.Create(PlayerModifier.Skin, new Vector2i(3, 3), Lang.Controls(Lang.CONTROLS.SKIN_COLOR), Lang.Controls(Lang.CONTROLS.SELECT_COLOR));
			CharSettings[0] = GenderAttributeWidget.Create(PlayerModifier.Gender, GenderAttributeWidget.Gender.MALE, Lang.Controls(Lang.CONTROLS.GENDER), Lang.Controls(Lang.CONTROLS.SELECT_GENDER));
			CharSettings[9] = DifficultyAttributeWidget.Create(PlayerModifier.Difficulty, Difficulty.SOFTCORE, Lang.Controls(Lang.CONTROLS.DIFFICULTY), Lang.Controls(Lang.CONTROLS.SELECT_DIFFICULTY));
			return new UI(Base, CatSelector, CharSettings);
		}

		public void Update(Player ActivePlayer)
		{
			SelPlayer = null;
			if (UIDelay > 0)
			{
				UIDelay--;
			}
			bool RegCatInput = UpdateCategoryInput(ActivePlayer);
			bool RegWidgetInput = UpdateWidgetInput(ActivePlayer);
			if (RegCatInput || RegWidgetInput)
			{
				if (Widget.SelectedIndex == 0 && RegWidgetInput)
				{
					Main.PlaySound(ActivePlayer.male ? 1 : 20);
				}
				else
				{
					Main.PlaySound(12);
				}
				UIDelay = Terraria.UI.UI_DELAY;
			}
			Widget.Update();
			if (PrevAttribute != null)
			{
				PrevAttribute.Update();
			}
			SelectedAttribute.Update();
			if (ParentUI.IsButtonTriggered(Buttons.Start))
			{
				ParentUI.ClearInput();
				ParentUI.SetMenu(MenuMode.NAME_CHARACTER);
			}
			if (ParentUI.IsBackButtonTriggered())
			{
				ParentUI.SetMenu(MenuMode.CONFIRM_LEAVE_CREATE_CHARACTER);
			}
			SelPlayer = ActivePlayer;
		}

		private bool IsButtonDown(Buttons Button)
		{
			if (UIDelay == 0)
			{
				return ParentUI.IsButtonDown(Button);
			}
			return false;
		}

		private bool UpdateCategoryInput(Player ActivePlayer)
		{
			bool RegisteredInput = false;
			IAttributeWidget SelAttribute = SelectedAttribute;
			if ((IsButtonDown(Terraria.UI.BTN_NEXT_ITEM) || ParentUI.IsButtonTriggered(Buttons.A)) && Widget.SelectNext())
			{
				RegisteredInput = true;
				PrevAttribute = SelAttribute;
				SelectedAttribute.Show();
				UpdateHeaderAndFooter();
				if (ParentUI.IsButtonTriggered(Buttons.A))
				{
					PrevAttribute.FlashSelection(FlashDuration);
				}
			}
			if (IsButtonDown(Terraria.UI.BTN_PREV_ITEM) && Widget.SelectPrevious())
			{
				RegisteredInput = true;
				PrevAttribute = SelAttribute;
				SelectedAttribute.Show();
				UpdateHeaderAndFooter();
			}
			if (ParentUI.IsButtonTriggered(Buttons.Y))
			{
				Randomize(ActivePlayer);
				RegisteredInput = true;
			}
			return RegisteredInput;
		}

		private bool UpdateWidgetInput(Player ActivePlayer)
		{
			bool RegisteredInput = false;
			IAttributeWidget SelAttribute = SelectedAttribute;
			if (UIDelay == 0)
			{
				if (ParentUI.IsDownButtonDown() && SelAttribute.SelectDown())
				{
					RegisteredInput = true;
				}
				if (ParentUI.IsUpButtonDown() && SelAttribute.SelectUp())
				{
					RegisteredInput = true;
				}
				if (ParentUI.IsLeftButtonDown() && SelAttribute.SelectLeft())
				{
					RegisteredInput = true;
				}
				if (ParentUI.IsRightButtonDown() && SelAttribute.SelectRight())
				{
					RegisteredInput = true;
				}
			}
			if (RegisteredInput)
			{
				SelAttribute.Apply(ActivePlayer);
			}
			return RegisteredInput;
		}

		private void UpdateHeaderAndFooter()
		{
			Header = SelectedAttribute.WidgetDescription;
			HeaderOrigin = Font.MeasureString(Header) * 0.5f;
			Footer = $"{Widget.SelectedIndex + 1} / {Attributes.Length}";
			FooterOrigin = Font.MeasureString(Footer) * 0.5f;
		}

		public void Draw(WorldView CurView)
		{
#if USE_ORIGINAL_CODE
			int num = Assets.Frame.Width >> 1;
			Vector2 vector = new Vector2((Main.ResolutionWidth / 2) - num, CurView.SafeAreaOffsetTop + 32);
			Vector2 zero = Vector2.Zero;
			Main.SpriteBatch.Draw(Assets.Frame, vector, Color.White);
			zero.X = 228f;
			zero.Y = 32f;
			Widget.Draw(vector + zero);
			zero.Y = 204f;
			if (Widget.Scrolling)
			{
				float scrollTween = Widget.ScrollTween;
				float num2 = Math.Min(scrollTween, 0.5f) / 0.5f;
				float alpha = (Math.Max(0.5f, scrollTween) - 0.5f) / 0.5f;
				PrevAttribute.Draw(vector + zero, alpha);
				SelectedAttribute.Draw(vector + zero, 1f - num2);
			}
			else
			{
				SelectedAttribute.Draw(vector + zero, 1f);
			}
			zero.Y = 78f;
			Main.SpriteBatch.DrawString(Font, Header, vector + zero, Color.White, 0f, HeaderOrigin, FontScale, SpriteEffects.None, 0f);
			zero.Y = 340f;
			Main.SpriteBatch.DrawString(Font, Footer, vector + zero, Color.White, 0f, FooterOrigin, FontScale, SpriteEffects.None, 0f);
			zero.Y = 385f;
			zero.X = num;
			string text = Lang.Controls(Lang.CONTROLS.CREATE_CHARACTER);
			Vector2 value = Font.MeasureString(text);
			Main.SpriteBatch.DrawString(Font, text, vector + zero, Color.White, 0f, value * 0.5f, FontScale, SpriteEffects.None, 0f);
			if (SelPlayer != null)
			{
				SelPlayer.velocity.X = 1f;
				SelPlayer.PlayerFrame();
				zero.X = 520f;
				zero.Y = 110f;
				ParentUI.DrawPlayer(SelPlayer, vector + zero, 4f);
#else
			int FrameOffset = (int)((Assets.Frame.Width >> 1) * Main.ScreenMultiplier);
			Vector2 AreaPos = new Vector2((Main.ResolutionWidth / 2) - FrameOffset, CurView.SafeAreaOffsetTop + 32);
			Vector2 AreaAdjust = Vector2.Zero;
			Main.SpriteBatch.Draw(Assets.Frame, AreaPos, null, Color.White, 0f, default, Main.ScreenMultiplier, SpriteEffects.None, 0f);
#if VERSION_INITIAL || VERSION_101
			AreaAdjust.X = 228f * Main.ScreenMultiplier;
#else
			zero.X = 253f * (float)Main.ScreenMultiplier;
#endif
			AreaAdjust.Y = 32f * Main.ScreenMultiplier;
			Widget.Draw(AreaPos + AreaAdjust);
			AreaAdjust.Y = 204f * Main.ScreenMultiplier;
			if (Widget.Scrolling)
			{
				float scrollTween = Widget.ScrollTween;
				float MinAlpha = Math.Min(scrollTween, 0.5f) / 0.5f;
				float Alpha = (Math.Max(0.5f, scrollTween) - 0.5f) / 0.5f;
				PrevAttribute.Draw(AreaPos + AreaAdjust, Alpha);
				SelectedAttribute.Draw(AreaPos + AreaAdjust, 1f - MinAlpha);
			}
			else
			{
				SelectedAttribute.Draw(AreaPos + AreaAdjust, 1f);
			}
			AreaAdjust.Y = 78f * Main.ScreenMultiplier;
			Main.SpriteBatch.DrawString(Font, Header, AreaPos + AreaAdjust, Color.White, 0f, HeaderOrigin, FontScale, SpriteEffects.None, 0f);
			AreaAdjust.Y = 340f * Main.ScreenMultiplier;
			Main.SpriteBatch.DrawString(Font, Footer, AreaPos + AreaAdjust, Color.White, 0f, FooterOrigin, FontScale, SpriteEffects.None, 0f);
			AreaAdjust.Y = 385f * Main.ScreenMultiplier;
			AreaAdjust.X = FrameOffset;
			string CtlText = Lang.Controls(Lang.CONTROLS.CREATE_CHARACTER);
			Vector2 TextOrigin = Font.MeasureString(CtlText);
			Main.SpriteBatch.DrawString(Font, CtlText, AreaPos + AreaAdjust, Color.White, 0f, TextOrigin * 0.5f, FontScale, SpriteEffects.None, 0f);
			if (SelPlayer != null)
			{
				SelPlayer.velocity.X = 1f;
				SelPlayer.PlayerFrame();
#if VERSION_INITIAL || VERSION_101
				AreaAdjust.X = 520f * Main.ScreenMultiplier;
#else
				zero.X = 565f * (float)Main.ScreenMultiplier;
#endif
				AreaAdjust.Y = 110f * Main.ScreenMultiplier;
				ParentUI.DrawPlayer(SelPlayer, AreaPos + AreaAdjust, 4f * Main.ScreenMultiplier);
#endif
			}
		}

		public void Randomize(Player ActivePlayer)
		{
			Vector2i[] SettingArr = RandomCharacter.RandCreate(RandGen);
			for (int i = 0; i < Attributes.Length; i++)
			{
				if (i != 9)
				{
					IAttributeWidget AttributeWidget = Attributes[i];
					AttributeWidget.SetCursor(SettingArr[i]);
					AttributeWidget.Apply(ActivePlayer);
				}
			}
		}

		public void ApplyDefaultAttributes(Player ActivePlayer)
		{
			Randomize(ActivePlayer);
			IAttributeWidget AttributeWidget = Attributes[9];
			AttributeWidget.Reset();
			AttributeWidget.Apply(ActivePlayer);
			Widget.SelectedIndex = 0;
			UpdateHeaderAndFooter();
		}

		public void ControlDescription(StringBuilder StrBuilder)
		{
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_CATEGORY));
			StrBuilder.Append(' ');
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
			StrBuilder.Append(' ');
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.RANDOMIZE));
		}
	}
}
