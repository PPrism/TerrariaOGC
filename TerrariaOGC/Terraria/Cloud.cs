using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct Cloud
	{
		public const int MaxNumClouds = 20;

		public const int MaxNumCloudTypes = 4;

		public static Cloud[] ActiveCloud = new Cloud[MaxNumClouds];

		public static int NumClouds = MaxNumClouds;

		public static float WindSpeed = 0f;

		public static float SpeedOfWind = 0f; // Look, it was better than WindSpeedSpeed

		public static bool ResetClouds = true;

		public Vector2 CloudPos;

		public float CloudScale;

		public bool IsActive;

		public byte CloudType;

		public ushort CloudWidth;

		public ushort CloudHeight;

		public static void Initialize()
		{
			for (int CloudIdx = 0; CloudIdx < MaxNumClouds; CloudIdx++)
			{
				ActiveCloud[CloudIdx].Init();
			}
		}

		public void Init()
		{
			IsActive = false;
			CloudType = 0;
			CloudWidth = 0;
			CloudHeight = 0;
		}

		public unsafe static void AddCloud()
		{
			fixed (Cloud* CreatedCloud = ActiveCloud)
			{
				Cloud* CloudIdx = CreatedCloud;
				for (int Counter = MaxNumClouds - 1; Counter >= 0; Counter--)
				{
					if (!CloudIdx->IsActive)
					{
						CloudIdx->CloudType = (byte)Main.Rand.Next(MaxNumCloudTypes);
						int SpritePointer = CloudIdx->CloudType + (int)_sheetTiles.ID.CLOUD_0;
						CloudIdx->CloudScale = Main.Rand.Next(50, 131) * 0.01f;
						CloudIdx->CloudWidth = (ushort)(SpriteSheet<_sheetTiles>.Source[SpritePointer].Width * CloudIdx->CloudScale);
						CloudIdx->CloudHeight = (ushort)(SpriteSheet<_sheetTiles>.Source[SpritePointer].Height * CloudIdx->CloudScale);
						float Speed = WindSpeed;
						if (Speed > 0f)
						{
							CloudIdx->CloudPos.X = -CloudIdx->CloudWidth;
						}
						else
						{
							CloudIdx->CloudPos.X = Main.ResolutionWidth;
						}
						CloudIdx->CloudPos.Y = Main.Rand.Next(-135, 135) - Main.Rand.Next(180);
						CloudIdx->IsActive = true;
						break;
					}
					CloudIdx++;
				}
			}
		}

		public Color CloudColor(Color BgColor)
		{
			float SizeMultiplier = (CloudScale - 0.4f) * 0.9f;
			float R = 255f - (255 - BgColor.R) * 1.1f;
			float G = 255f - (255 - BgColor.G) * 1.1f;
			float B = 255f - (255 - BgColor.B) * 1.1f;
			float A = 255f;
			R *= SizeMultiplier;
			G *= SizeMultiplier;
			B *= SizeMultiplier;
			A *= SizeMultiplier;
			if (R < 0f)
			{
				R = 0f;
			}
			if (G < 0f)
			{
				G = 0f;
			}
			if (B < 0f)
			{
				B = 0f;
			}
			if (A < 0f)
			{
				A = 0f;
			}
			return new Color((byte)R, (byte)G, (byte)B, (byte)A);
		}

		public unsafe static void UpdateClouds()
		{
			if (ResetClouds)
			{
				ResetClouds = false;
				NumClouds = Main.Rand.Next(MaxNumClouds / 2, MaxNumClouds);
				do
				{
					WindSpeed = Main.Rand.Next(-100, 101) * 0.01f;
				}
				while (WindSpeed == 0f);
				for (int CloudIdx = 0; CloudIdx < MaxNumClouds; CloudIdx++)
				{
					ActiveCloud[CloudIdx].IsActive = false;
				}
				for (int j = 0; j < NumClouds; j++)
				{
					AddCloud();
				}
				for (int CloudIdx2 = 0; CloudIdx2 < NumClouds; CloudIdx2++)
				{
					int num = Main.Rand.Next(Main.ResolutionWidth);
					if (ActiveCloud[CloudIdx2].CloudPos.X < 0f)
					{
						ActiveCloud[CloudIdx2].CloudPos.X += num;
					}
					else
					{
						ActiveCloud[CloudIdx2].CloudPos.X -= num;
					}
				}
			}
			SpeedOfWind += Main.Rand.Next(-20, 21) * 0.0001f;
			if (SpeedOfWind < -0.002)
			{
				SpeedOfWind = -0.002f;
			}
			else if (SpeedOfWind > 0.002)
			{
				SpeedOfWind = 0.002f;
			}
			WindSpeed += SpeedOfWind;
			if (WindSpeed < -0.4)
			{
				WindSpeed = -0.4f;
			}
			else if (WindSpeed > 0.4)
			{
				WindSpeed = 0.4f;
			}
			NumClouds += Main.Rand.Next(-1, 2);
			if (NumClouds < 0)
			{
				NumClouds = 0;
			}
			else if (NumClouds > MaxNumClouds)
			{
				NumClouds = MaxNumClouds;
			}
			int Count = 0;
			for (int CloudIdx3 = 0; CloudIdx3 < MaxNumClouds; CloudIdx3++)
			{
				fixed (Cloud* CreatedCloud = &ActiveCloud[CloudIdx3])
				{
					if (CreatedCloud->IsActive)
					{
						CreatedCloud->Update();
						Count++;
					}
				}
			}
			if (Count < NumClouds)
			{
				AddCloud();
			}
		}

		public void Update()
		{
			CloudPos.X += WindSpeed * CloudScale * 2f;
			if (WindSpeed > 0f)
			{
				if (CloudPos.X > Main.ResolutionWidth)
				{
					IsActive = false;
				}
			}
			else if (CloudPos.X < -CloudWidth)
			{
				IsActive = false;
			}
		}
	}
}
