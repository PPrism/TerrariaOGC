using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct Star
	{
		public const int MaxNumStars = 96; // Less stars than PC 1.1.2 (130)

		public const int NumStarTypes = 5;

		public static Star[] star = new Star[MaxNumStars];

		public Vector2 position;

		public float scale;

		public float rotation;

		public int type;

		public float twinkle;

		public float twinkleSpeed;

		public float rotationSpeed;

		public unsafe static void SpawnStars()
		{
			for (int i = 0; i < MaxNumStars; i++)
			{
				star[i] = new Star
				{
					type = Main.Rand.Next(NumStarTypes),
					rotation = Main.Rand.Next(628) * 0.01f,
					scale = Main.Rand.Next(50, 120) * 0.01f,
					twinkle = Main.Rand.Next(101) * 0.01f,
					twinkleSpeed = Main.Rand.Next(40, 100) * 0.0001f,
					rotationSpeed = Main.Rand.Next(10, 40) * 0.0001f
				};
				int width = SpriteSheet<_sheetTiles>.Source[star[i].type + (int)_sheetTiles.ID.STAR_0].Width;
				int height = SpriteSheet<_sheetTiles>.Source[star[i].type + (int)_sheetTiles.ID.STAR_0].Height;
				int Location;
				do
				{
					Location = Main.Rand.Next(Main.ResolutionWidth - width);
				}
				while ((Location < (Main.ResolutionHeight / 20) || Location > (Main.ResolutionHeight - (Main.ResolutionHeight / 20))) && Main.Rand.Next(4) != 0);
				star[i].position.X = Location + (width >> 1);
				// I still can't figure out why they did the position calculations this way...
				do
				{
					Location = Main.Rand.Next(Main.ResolutionHeight - height);
				}
				while ((Location < (Main.ResolutionHeight / 20) || Location > (Main.ResolutionHeight - (Main.ResolutionHeight / 20))) && Main.Rand.Next(4) != 0);
				star[i].position.Y = Location + (height >> 1);

				if (Main.Rand.Next(2) == 0)
				{
					star[i].twinkleSpeed *= -1f;
				}
				if (Main.Rand.Next(2) == 0)
				{
					star[i].rotationSpeed *= -1f;
				}
			}
		}

		public unsafe static void UpdateStars()
		{
			for (int i = 0; i < MaxNumStars; i++)
			{
				star[i].twinkle += star[i].twinkleSpeed;
				if (star[i].twinkle > 1f)
				{
					star[i].twinkle = 1f;
					star[i].twinkleSpeed *= -1f;
				}
				else if ((double)star[i].twinkle < 0.5f)
				{
					star[i].twinkle = 0.5f;
					star[i].twinkleSpeed *= -1f;
				}
				star[i].rotation += star[i].rotationSpeed;
				if ((double)star[i].rotation > 6.28f)
				{
					star[i].rotation -= 6.28f;
				}
				if (star[i].rotation < 0f)
				{
					star[i].rotation += 6.28f;
				}
			}
		}
	}
}
