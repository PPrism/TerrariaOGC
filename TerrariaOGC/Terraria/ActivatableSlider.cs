using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public class ActivatableSlider
	{
		private Slider Selected;

		private readonly Slider ActiveSlider;

		private readonly Slider InactiveSlider;

		public float Progress
		{
			get
			{
				return ActiveSlider.SliderProgress;
			}
			set
			{
				ActiveSlider.SliderProgress = value;
				InactiveSlider.SliderProgress = value;
			}
		}

		public bool Active
		{
			get
			{
				return Selected == ActiveSlider;
			}
			set
			{
				if (value)
				{
					Selected = ActiveSlider;
				}
				else
				{
					Selected = InactiveSlider;
				}
			}
		}

		public ActivatableSlider(Slider Active, Slider Inactive)
		{
			ActiveSlider = Active;
			InactiveSlider = Inactive;
			Selected = Active;
		}

		public void Draw(SpriteBatch Screen)
		{
			Selected.Draw(Screen);
		}
	}
}
