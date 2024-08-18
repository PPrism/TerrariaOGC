using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct Dust
	{
		public const int MaxNumGlobalDust = 256;

		public const int MaxNumLocalDust = 128;

		public byte Active;

		public bool NoGravity;

		public bool NoLight;

		public ushort Type;

		public short Alpha;

		public Color Color;

		public float FadeIn;

		public float Rotation;

		public float Scale;

		public Rectangle Frame;

		public Vector2 Position;

		public Vector2 Velocity;

		public void Init()
		{
			Active = 0;
			NoGravity = false;
			NoLight = false;
			Type = 0;
			FadeIn = 0f;
		}

		public void GetAlpha(ref Color DustColor)
		{
			if (Type == 6 || Type == 75 || Type == 20 || Type == 21)
			{
				DustColor.A = 25;
				return;
			}
			if ((Type == 68 || Type == 70) && NoGravity)
			{
				DustColor = new Color(255, 255, 255, 0);
				return;
			}
			if (Type == 66)
			{
				DustColor.A = 0;
				return;
			}
			if (Type == 71)
			{
				DustColor = new Color(200, 200, 200, 0);
				return;
			}
			if (Type == 72)
			{
				DustColor = new Color(200, 200, 200, 200);
				return;
			}
			float AlphaMultiplier = (255 - Alpha) / 255f;
			if (Type == 15 || Type == 29 || Type == 35 || Type == 41 || Type == 44 || Type == 27 || Type == 45 || Type == 55 || Type == 56 || Type == 57 || Type == 58 || Type == 73 || Type == 74)
			{
				AlphaMultiplier = (AlphaMultiplier + 3f) * 0.25f;
			}
			else if (Type == 43)
			{
				AlphaMultiplier = (AlphaMultiplier + 9f) * 0.1f;
			}
			DustColor.R = (byte)(DustColor.R * AlphaMultiplier);
			DustColor.G = (byte)(DustColor.G * AlphaMultiplier);
			DustColor.B = (byte)(DustColor.B * AlphaMultiplier);
			DustColor.A -= (byte)Alpha;
		}

		public void GetColor(ref Color DustColor)
		{
			int R = Color.R - (255 - DustColor.R);
			int G = Color.G - (255 - DustColor.G);
			int B = Color.B - (255 - DustColor.B);
			int A = Color.A - (255 - DustColor.A);
			DustColor = new Color(R, G, B, A);
		}
	}
}
