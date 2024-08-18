using Microsoft.Xna.Framework;

namespace Terraria.CreateCharacter
{
	public class WidgetAnimation
	{
		private Vector2 StartPos;

		private Vector2 Difference;

		private float Progress;

		private readonly float Speed;

		public bool Finished => Progress >= 1f;

		public Vector2 Position => StartPos + Difference * Progress;

		public float Alpha => Progress;

		public WidgetAnimation(Vector2 Start, Vector2 End, float AnimSpeed)
		{
			StartPos = Start;
			Difference = End - Start;
			Speed = AnimSpeed;
			Progress = 0f;
		}

		public void Start()
		{
			Progress = 0f;
		}

		public void Update()
		{
			Progress += Speed;
			if (Progress >= 1f)
			{
				Progress = 1f;
			}
		}
	}
}
