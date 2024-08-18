using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public class Slider
	{
		private readonly Texture2D Texture;

		private Rectangle Empty, Filled, LeftComponent, RightComponent;

		private Vector2 Position, RightComponentOffset;

		private float Progress;

		public float SliderProgress
		{
			get
			{
				return Progress;
			}
			set
			{
				Progress = value;
				if (Progress < 0f)
				{
					Progress = 0f;
				}
				else if (Progress > 1f)
				{
					Progress = 1f;
				}
				LeftComponent = Filled;
				LeftComponent.Width = (int)(LeftComponent.Width * Progress);
				RightComponent = Empty;
				RightComponent.Width = (int)(RightComponent.Width * (1f - Progress));
				RightComponent.X = LeftComponent.Width;
				RightComponentOffset = new Vector2(LeftComponent.Width, 0f);
			}
		}

		public Slider(Texture2D SliderTex, Rectangle EmptyRect, Rectangle FilledRect, Vector2 SliderPosition)
		{
			Texture = SliderTex;
			Empty = EmptyRect;
			Filled = FilledRect;
			Position = SliderPosition;
			SliderProgress = 0f;
		}

		public void Draw(SpriteBatch Screen)
		{
			Screen.Draw(Texture, Position, (Rectangle?)LeftComponent, Color.White);
			Screen.Draw(Texture, Position + RightComponentOffset, (Rectangle?)RightComponent, Color.White);
		}
	}
}
