using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct Time
	{
		private const int sunWidth = 64;

		private const int moonWidth = 50;

		public const int moonHeight = 50;

		public const float dayLength = 54000f;

		public const float NightLength = 32400f;

		public float DayRate;

		public float WorldTime;

		public bool DayTime;

		public bool IsBloodMoon;

		public byte MoonPhase;

		public Color intermediateCelestialColor;

		public Color intermediateBgColor;

		public short celestialX;

		public short celestialY;

		public float celestialRotation;

		public float celestialScale;

		public Color celestialColor;

		public Color bgColor;

		public Color tileColor;

		public Vector3 TileColorFore;

		public static bool xMas;

		static Time()
		{
		}

		public void reset(float speed)
		{
			DayRate = speed;
			WorldTime = 13500f;
			DayTime = true;
			IsBloodMoon = false;
			MoonPhase = 0;
			intermediateCelestialColor.A = byte.MaxValue;
			intermediateBgColor.A = byte.MaxValue;
			tileColor.A = byte.MaxValue;
			updateDay();
		}

		private void updateNight()
		{
			celestialX = (short)((int)(WorldTime / NightLength * (Main.ResolutionWidth + (moonWidth * 2))) - moonWidth);
			celestialRotation = WorldTime / (NightLength / 2) - 7.3f;
			float num = ((!(WorldTime < (NightLength / 2))) ? ((WorldTime / NightLength - 0.5f) * 2f) : (1f - WorldTime / (NightLength / 2)));
			num *= num;
			celestialY = (short)((int)(num * 250f) + 180);
			celestialScale = Lighting.MaxBrightness - num * 0.4f;
			if (IsBloodMoon)
			{
				if (WorldTime < (NightLength / 2))
				{
					float num2 = 1f - WorldTime / (NightLength / 2);
					intermediateCelestialColor.R = (byte)(num2 * 10f + 205f);
					intermediateCelestialColor.G = (byte)(num2 * 170f + 55f);
					intermediateCelestialColor.B = (byte)(num2 * 200f + 55f);
					intermediateBgColor.R = (byte)(40f - num2 * 40f + 35f);
					intermediateBgColor.G = (byte)(num2 * 20f + 15f);
					intermediateBgColor.B = (byte)(num2 * 20f + 15f);
				}
				else if (WorldTime >= (NightLength / 2))
				{
					float num2 = (WorldTime / NightLength - 0.5f) * 2f;
					intermediateCelestialColor.R = (byte)(num2 * 10f + 205f);
					intermediateCelestialColor.G = (byte)(num2 * 170f + 55f);
					intermediateCelestialColor.B = (byte)(num2 * 200f + 55f);
					intermediateBgColor.R = (byte)(40f - num2 * 40f + 35f);
					intermediateBgColor.G = (byte)(num2 * 20f + 15f);
					intermediateBgColor.B = (byte)(num2 * 20f + 15f);
				}
			}
			else if (WorldTime < (NightLength / 2))
			{
				float num2 = 1f - WorldTime / (NightLength / 2);
				intermediateCelestialColor.R = (byte)(num2 * 10f + 205f);
				intermediateCelestialColor.G = (byte)(num2 * 70f + 155f);
				intermediateCelestialColor.B = (byte)(num2 * 100f + 155f);
				intermediateBgColor.R = (byte)(num2 * 20f + 15f);
				intermediateBgColor.G = (byte)(num2 * 20f + 15f);
				intermediateBgColor.B = (byte)(num2 * 20f + 15f);
			}
			else if (WorldTime >= (NightLength / 2))
			{
				float num2 = (WorldTime / NightLength - 0.5f) * 2f;
				intermediateCelestialColor.R = (byte)(num2 * 50f + 205f);
				intermediateCelestialColor.G = (byte)(num2 * 100f + 155f);
				intermediateCelestialColor.B = (byte)(num2 * 100f + 155f);
				intermediateBgColor.R = (byte)(num2 * 10f + 15f);
				intermediateBgColor.G = (byte)(num2 * 20f + 15f);
				intermediateBgColor.B = (byte)(num2 * 20f + 15f);
			}
			else
			{
				intermediateCelestialColor = Color.White;
				intermediateBgColor = Color.White;
			}
		}

		private void updateDay()
		{
			celestialX = (short)((int)(WorldTime / dayLength * (Main.ResolutionWidth + (sunWidth * 2))) - sunWidth);
			celestialRotation = WorldTime / (dayLength / 2) - 7.3f;
			float num = ((!(WorldTime < (dayLength / 2))) ? ((WorldTime / dayLength - 0.5f) * 2f) : (1f - WorldTime / dayLength * 2f));
			num *= num;
			celestialY = (short)((int)(num * 250f) + 180);
			celestialScale = (Lighting.MaxBrightness - num * 0.4f) * 1.1f;
			if (WorldTime < 13500f)
			{
				float num2 = WorldTime / 13500f;
				intermediateCelestialColor.R = (byte)(num2 * 200f + 55f);
				intermediateCelestialColor.G = (byte)(num2 * 180f + 75f);
				intermediateCelestialColor.B = (byte)(num2 * 250f + 5f);
				intermediateBgColor.R = (byte)(num2 * 230f + 25f);
				intermediateBgColor.G = (byte)(num2 * 220f + 35f);
				intermediateBgColor.B = (byte)(num2 * 220f + 35f);
			}
			else if (WorldTime > 45900f)
			{
				float num2 = 1f - (WorldTime / dayLength - 0.85f) * 6.66666651f;
				intermediateCelestialColor.R = (byte)(num2 * 120f + 55f);
				intermediateCelestialColor.G = (byte)(num2 * 100f + 25f);
				intermediateCelestialColor.B = (byte)(num2 * 120f + 55f);
				intermediateBgColor.R = (byte)(num2 * 200f + 35f);
				intermediateBgColor.G = (byte)(num2 * 85f + 35f);
				intermediateBgColor.B = (byte)(num2 * 135f + 35f);
			}
			else if (WorldTime > 37800f)
			{
				float num2 = 1f - (WorldTime / dayLength - 0.7f) * 6.66666651f;
				intermediateCelestialColor.R = (byte)(num2 * 80f + 175f);
				intermediateCelestialColor.G = (byte)(num2 * 130f + 125f);
				intermediateCelestialColor.B = (byte)(num2 * 100f + 155f);
				intermediateBgColor.R = (byte)(num2 * 20f + 235f);
				intermediateBgColor.G = (byte)(num2 * 135f + 120f);
				intermediateBgColor.B = (byte)(num2 * 85f + 170f);
			}
			else
			{
				intermediateCelestialColor = Color.White;
				intermediateBgColor = Color.White;
			}
		}

		public void applyJungle(float light)
		{
			if (light > 1f)
			{
				light = 1f;
			}
			int r = intermediateBgColor.R;
			int g = intermediateBgColor.G;
			int b = intermediateBgColor.B;
			r -= (int)(30f * light * (r * (1f / 255f)));
			b -= (int)(90f * light * (b * (1f / 255f)));
			if (r < 15)
			{
				r = 15;
			}
			if (b < 15)
			{
				b = 15;
			}
			bgColor.R = (byte)r;
			bgColor.G = (byte)g;
			bgColor.B = (byte)b;
			bgColor.A = byte.MaxValue;
			r = intermediateCelestialColor.R;
			g = intermediateCelestialColor.G;
			b = intermediateCelestialColor.B;
			if (DayTime)
			{
				r -= (int)(30f * light * (r * (1f / 255f)));
				b -= (int)(10f * light * (g * (1f / 255f)));
			}
			else
			{
				r -= (int)(140f * light * (r * (1f / 255f)));
				g -= (int)(190f * light * (g * (1f / 255f)));
				b -= (int)(170f * light * (b * (1f / 255f)));
			}
			if (r < 15)
			{
				r = 15;
			}
			if (g < 15)
			{
				g = 15;
			}
			if (b < 15)
			{
				b = 15;
			}
			celestialColor.R = (byte)r;
			celestialColor.G = (byte)g;
			celestialColor.B = (byte)b;
			celestialColor.A = byte.MaxValue;
		}

		public void applyEvil(float light)
		{
			if (light > 1f)
			{
				light = 1f;
			}
			int r = intermediateBgColor.R;
			int g = intermediateBgColor.G;
			int b = intermediateBgColor.B;
			r -= (int)(100f * light * (r * (1f / 255f)));
			g -= (int)(140f * light * (g * (1f / 255f)));
			b -= (int)(80f * light * (b * (1f / 255f)));
			if (r < 15)
			{
				r = 15;
			}
			if (g < 15)
			{
				g = 15;
			}
			if (b < 15)
			{
				b = 15;
			}
			bgColor.R = (byte)r;
			bgColor.G = (byte)g;
			bgColor.B = (byte)b;
			bgColor.A = byte.MaxValue;
			r = intermediateCelestialColor.R;
			g = intermediateCelestialColor.G;
			b = intermediateCelestialColor.B;
			if (DayTime)
			{
				r -= (int)(100f * light * (r * (1f / 255f)));
				g -= (int)(100f * light * (g * (1f / 255f)));
			}
			else
			{
				r -= (int)(140f * light * (r * (1f / 255f)));
				g -= (int)(190f * light * (g * (1f / 255f)));
				b -= (int)(170f * light * (b * (1f / 255f)));
			}
			if (r < 15)
			{
				r = 15;
			}
			if (g < 15)
			{
				g = 15;
			}
			if (b < 15)
			{
				b = 15;
			}
			celestialColor.R = (byte)r;
			celestialColor.G = (byte)g;
			celestialColor.B = (byte)b;
			celestialColor.A = byte.MaxValue;
		}

		public void applyNothing()
		{
			celestialColor = intermediateCelestialColor;
			bgColor = intermediateBgColor;
		}

		public void finalizeColors()
		{
			if (IsBloodMoon)
			{
				if (bgColor.R < 35)
				{
					bgColor.R = 35;
				}
				if (bgColor.G < 35)
				{
					bgColor.G = 35;
				}
				if (bgColor.B < 35)
				{
					bgColor.B = 35;
				}
			}
			else
			{
				if (bgColor.R < 25)
				{
					bgColor.R = 25;
				}
				if (bgColor.G < 25)
				{
					bgColor.G = 25;
				}
				if (bgColor.B < 25)
				{
					bgColor.B = 25;
				}
			}
			tileColor.R = (byte)((bgColor.G + bgColor.B + bgColor.R * 8) / 10);
			tileColor.G = (byte)((bgColor.R + bgColor.B + bgColor.G * 8) / 10);
			tileColor.B = (byte)((bgColor.R + bgColor.G + bgColor.B * 8) / 10);
			TileColorFore.X = tileColor.R * (1f / 255f);
			TileColorFore.Y = tileColor.G * (1f / 255f);
			TileColorFore.Z = tileColor.B * (1f / 255f);
		}

		public bool Update()
		{
			WorldTime += DayRate;
			if (!DayTime)
			{
				if (WorldTime > NightLength)
				{
					WorldTime = 0f;
					DayTime = true;
					IsBloodMoon = false;
					MoonPhase = (byte)(MoonPhase + 1 & 7);
					updateDay();
					return true;
				}
				updateNight();
			}
			else
			{
				if (WorldTime > dayLength)
				{
					WorldTime = 0f;
					DayTime = false;
					updateNight();
					return true;
				}
				updateDay();
			}
			return false;
		}

		public static void CheckXMas()
		{
			DateTime now = DateTime.Now;
			xMas = now.Month == 12 && now.Day >= 15;
		}
	}
}
