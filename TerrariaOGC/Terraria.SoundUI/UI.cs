using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.SoundUI
{
	internal class UI
	{
		public struct Icon
		{
			public Texture2D Texture;

			public Vector2 Position;

			public Rectangle Source;

			public Icon(Texture2D Texture, Vector2 Position, Rectangle Source)
			{
				this.Texture = Texture;
				this.Position = Position;
				this.Source = Source;
			}
		}

		private const int AddYSpacing = 10;

		private const float SliderStepSize = 0.05f;

		private readonly float StepSize;

		private readonly Terraria.UI ParentUI;

		private readonly ActivatableSlider SoundSlider, MusicSlider;

		private int UIDelay;

		private Icon SoundIcon, MusicIcon;

		private ActivatableSlider Selected;

		public static UI Create(Terraria.UI ParentUI)
		{
			int SliderSpacing = Assets.SliderEmptyRect.Height + AddYSpacing;
#if USE_ORIGINAL_CODE
			Vector2 Value = new Vector2(300f, 300f);
#else
			float IconX = 300f;
			float IconY = 300f;

			switch (Main.ScreenHeightPtr)
			{
				case 1:
					IconX = 475f;
					IconY = 350f;
					break;

				case 2:
					IconX = 800f;
					IconY = 500f;
					break;
			}

			Vector2 Value = new Vector2(IconX, IconY);
#endif
			Vector2 SliderPosition = Value + new Vector2(50f, 0f);

			Slider SoundActive = new Slider(Assets.SliderTex, Assets.SliderEmptyRect, Assets.SliderFullRect, SliderPosition);
			Slider SoundInactive = new Slider(Assets.SliderTex, Assets.SliderEmptyInactiveRect, Assets.SliderFullInactiveRect, SliderPosition);
			ActivatableSlider SoundSlider = new ActivatableSlider(SoundActive, SoundInactive);
			SliderPosition.Y += SliderSpacing;

			Slider MusicActive = new Slider(Assets.SliderTex, Assets.SliderEmptyRect, Assets.SliderFullRect, SliderPosition);
			Slider MusicInactive = new Slider(Assets.SliderTex, Assets.SliderEmptyInactiveRect, Assets.SliderFullInactiveRect, SliderPosition);
			ActivatableSlider MusicSlider = new ActivatableSlider(MusicActive, MusicInactive);
			
			Vector2 IconPosition = Value + new Vector2(0f, Assets.SliderEmptyRect.Height - Assets.SoundIconRect.Height);
			Icon SoundIcon = new Icon(Assets.SoundIconsTex, IconPosition, Assets.SoundIconRect);
			IconPosition.Y += SliderSpacing;
			Icon MusicIcon = new Icon(Assets.SoundIconsTex, IconPosition, Assets.MusicIconRect);
			return new UI(ParentUI, SoundIcon, MusicIcon, SoundSlider, MusicSlider, SliderStepSize);
		}

		public UI(Terraria.UI ParentUI, Icon SoundIcon, Icon MusicIcon, ActivatableSlider SoundSlider, ActivatableSlider MusicSlider, float StepSize)
		{
			this.ParentUI = ParentUI;
			this.SoundIcon = SoundIcon;
			this.MusicIcon = MusicIcon;
			this.SoundSlider = SoundSlider;
			this.MusicSlider = MusicSlider;
			SelectSoundSlider();
			this.StepSize = StepSize;
		}

		public void UpdateVolumes()
		{
			SoundSlider.Progress = ParentUI.SoundVolume;
			MusicSlider.Progress = ParentUI.MusicVolume;
		}

		private void SelectMusicSlider()
		{
			SoundSlider.Active = false;
			MusicSlider.Active = true;
			Selected = MusicSlider;
		}

		private void SelectSoundSlider()
		{
			SoundSlider.Active = true;
			MusicSlider.Active = false;
			Selected = SoundSlider;
		}

		public void Update()
		{
			if (UIDelay > 0)
			{
				UIDelay--;
				return;
			}
			bool InputRegistered = false;
			if (ParentUI.IsRightButtonDown())
			{
				Selected.Progress += StepSize;
				InputRegistered = true;
			}
			if (ParentUI.IsLeftButtonDown())
			{
				Selected.Progress -= StepSize;
				InputRegistered = true;
			}
			if (ParentUI.IsUpButtonDown() || ParentUI.IsDownButtonDown())
			{
				if (Selected == SoundSlider)
				{
					SelectMusicSlider();
				}
				else
				{
					SelectSoundSlider();
				}
				InputRegistered = true;
			}
			if (InputRegistered)
			{
				UIDelay = 12;
				Main.SoundVolume = ParentUI.SoundVolume = SoundSlider.Progress;
				Main.MusicVolume = ParentUI.MusicVolume = MusicSlider.Progress;
				ParentUI.SettingsDirty = true;
				Main.PlaySound(12);
			}
		}

		public void Draw(SpriteBatch screen)
		{
			screen.Draw(SoundIcon.Texture, SoundIcon.Position, (Rectangle?)SoundIcon.Source, Color.White);
			screen.Draw(MusicIcon.Texture, MusicIcon.Position, (Rectangle?)MusicIcon.Source, Color.White);
			SoundSlider.Draw(screen);
			MusicSlider.Draw(screen);
		}

		public void ControlDescription(StringBuilder StrBuilder)
		{
			if (Selected == MusicSlider)
			{
				StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_MUSIC_VOLUME));
				StrBuilder.Append(' ');
			}
			else if (Selected == SoundSlider)
			{
				StrBuilder.Append(Lang.Controls(Lang.CONTROLS.CHANGE_SOUND_VOLUME));
				StrBuilder.Append(' ');
			}
			StrBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
		}
	}
}
