using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct CombatText
	{
		public byte Active;

		public bool Crit;

		public short LifeTime;

		public float Alpha;

		public float AlphaDir;

		public string Text;

		public Vector2 Position;

		public Vector2 Velocity;

		public Vector2 TextSize;

		public float Scale;

		public float Rotation;

#if !USE_ORIGINAL_CODE
		public byte Type;
#endif

		public void Init()
		{
			Active = 0;
		}

#if !USE_ORIGINAL_CODE
		public static void NewText(Vector2 Position, int Width, int Height, int Value, bool Crit = false, byte Type = 0) // Addition: see DrawCombatText
#else
		public static void NewText(Vector2 Position, int Width, int Height, int Value, bool Crit = false)
#endif
		{
			for (int ActiveText = 0; ActiveText < Main.MaxNumCombatText; ActiveText++)
			{
				if (Main.CombatTextSet[ActiveText].Active == 0)
				{
					Width >>= 1;
					Height >>= 1;
					Main.CombatTextSet[ActiveText].Text = Value.ToStringLookup();
					Vector2 LengthVector = UI.MeasureString(UI.CombatTextFont[Crit ? 1 : 0], Main.CombatTextSet[ActiveText].Text); // If critical, account for the crit font.
					Main.CombatTextSet[ActiveText].TextSize = LengthVector;
					Main.CombatTextSet[ActiveText].Alpha = 1f;
					Main.CombatTextSet[ActiveText].AlphaDir = -0.05f;
					Main.CombatTextSet[ActiveText].Active = 1;
					Main.CombatTextSet[ActiveText].Scale = 0f;
					Main.CombatTextSet[ActiveText].Position.X = Position.X + Width - LengthVector.X * 0.5f;
					Main.CombatTextSet[ActiveText].Position.Y = Position.Y + (Height >> 1) - LengthVector.Y * 0.5f;
					Main.CombatTextSet[ActiveText].Position.X += Main.Rand.Next(-Width, Width + 1);
					Main.CombatTextSet[ActiveText].Position.Y += Main.Rand.Next(-Height, Height + 1);
					Main.CombatTextSet[ActiveText].Crit = Crit;

#if !USE_ORIGINAL_CODE
					Main.CombatTextSet[ActiveText].Type = Type;
#endif

					if (Crit)
					{
						Main.CombatTextSet[ActiveText].LifeTime = 120;
						Main.CombatTextSet[ActiveText].Velocity.Y = -14f;
						Main.CombatTextSet[ActiveText].Velocity.X = Main.Rand.Next(-25, 26) * 0.05f;
						Main.CombatTextSet[ActiveText].Rotation = ((Main.CombatTextSet[ActiveText].Velocity.X < 0f) ? (-213f / (565f * (float)Math.PI)) : (213f / (565f * (float)Math.PI)));
					}
					else
					{
						Main.CombatTextSet[ActiveText].Rotation = 0f;
						Main.CombatTextSet[ActiveText].Velocity.Y = -7f;
						Main.CombatTextSet[ActiveText].LifeTime = 60;
					}
					break;
				}
			}
		}

		public void Update()
		{
			Alpha += AlphaDir;
			if (Alpha <= 0.6f)
			{
				AlphaDir = 0f - AlphaDir;
			}
			else if (Alpha >= 1f)
			{
				Alpha = 1f;
				AlphaDir = 0f - AlphaDir;
			}
			Velocity.Y *= 0.92f;
			if (Crit)
			{
				Velocity.Y *= 0.92f;
			}
			Velocity.X *= 0.93f;
			Position.X += Velocity.X;
			Position.Y += Velocity.Y;
			if (--LifeTime <= 0)
			{
				Scale -= 0.1f;
				if (Scale < 0.1f)
				{
					Active = 0;
				}
				LifeTime = 0;
				if (Crit)
				{
					AlphaDir = -1f;
					Scale += 0.07f;
				}
				return;
			}
			if (Crit)
			{
				if (Velocity.X < 0f)
				{
					Rotation += 0.001f;
				}
				else
				{
					Rotation -= 0.001f;
				}
			}
			if (Scale < 1f)
			{
				Scale += 0.1f;
			}
			if (Scale > 1f)
			{
				Scale = 1f;
			}
		}

		public static void UpdateCombatText()
		{
			for (int TextIdx = Main.MaxNumCombatText - 1; TextIdx >= 0; TextIdx--)
			{
				if (Main.CombatTextSet[TextIdx].Active != 0)
				{
					Main.CombatTextSet[TextIdx].Update();
				}
			}
		}
	}
}
