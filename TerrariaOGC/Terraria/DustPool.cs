using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;

#if !VERSION_INITIAL
using System;
#endif

namespace Terraria
{
	public struct DustPool
	{
		public WorldView View;

		public short SnowDust;

		public short LavaBubbles;

		public short NextDust;

		public short PoolSize;

		public Dust[] Dusts;

		public DustPool(WorldView CurrentView, int MaxSize)
		{
			View = CurrentView;
			SnowDust = 0;
			LavaBubbles = 0;
			NextDust = 0;
			PoolSize = (short)MaxSize;
			Dusts = new Dust[MaxSize];
		}

		public void Init()
		{
			for (int DustIdx = PoolSize - 1; DustIdx >= 0; DustIdx--)
			{
				Dusts[DustIdx].Init();
			}
		}

		private bool ClipDust(int DustX, int DustY)
		{
			if (View == null)
			{
				return !WorldView.AnyViewContains(DustX, DustY);
			}
			return !View.ClipArea.Contains(DustX, DustY);
		}

		public unsafe Dust* NewDust(int Type, ref Rectangle Dimensions, double SpeedX = 0.0, double SpeedY = 0.0, int Alpha = 0, Color DustColor = default, double Scale = 1.0)
		{
			return NewDust(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height, Type, SpeedX, SpeedY, Alpha, DustColor, Scale);
		}

		public unsafe Dust* NewDust(int X, int Y, int Width, int Height, int Type, double SpeedX = 0.0, double SpeedY = 0.0, int Alpha = 0, Color newColor = default, double Scale = 1.0)
		{
			if (ClipDust(X, Y))
			{
				return null;
			}
			int NextIdx = NextDust;
			int MaxIdx = PoolSize - 1;
			for (int i = 0; i <= MaxIdx; i++)
			{
				int DustIdx = NextIdx & MaxIdx;
				fixed (Dust* ActiveDust = &Dusts[DustIdx])
				{
					if (ActiveDust->Active == 0)
					{
						ActiveDust->Active = 1;
						ActiveDust->NoGravity = false;
						ActiveDust->NoLight = false;
						ActiveDust->Type = (ushort)Type;
						ActiveDust->Color = newColor;
						ActiveDust->Alpha = (short)Alpha;
						ActiveDust->FadeIn = 0f;
						ActiveDust->Rotation = 0f;
						ActiveDust->Scale = (float)(Scale + Main.Rand.Next(-20, 21) * 0.01 * Scale);
						ActiveDust->Frame.X = 10 * Type;
						ActiveDust->Frame.Y = 10 * Main.Rand.Next(3);
						if (Type == -1)
						{
							ActiveDust->Frame.X -= 1000; // In 1.01, the sprite is 1.2 but none of the 1.2 ones are used.
							ActiveDust->Frame.Y += 30;
						}
						ActiveDust->Frame.Width = 8;
						ActiveDust->Frame.Height = 8;
						int DustWidth = Width;
						int DustHeight = Height;
						if (DustWidth < 5)
						{
							DustWidth = 5;
						}
						if (DustHeight < 5)
						{
							DustHeight = 5;
						}
						ActiveDust->Position.X = X + Main.Rand.Next(DustWidth - 4) + 4;
						ActiveDust->Position.Y = Y + Main.Rand.Next(DustHeight - 4) + 4;
						switch (Type)
						{
						case 6:
						case 29:
						case 59:
						case 60:
						case 61:
						case 62:
						case 63:
						case 64:
						case 65:
						case 75:
						case 135:
						case 158:
						case 169:
							ActiveDust->Velocity.X = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedX) * 0.3);
							ActiveDust->Velocity.Y = Main.Rand.Next(-10, 6) * 0.1f;
							ActiveDust->Scale *= 0.7f;
							break;
						case 127:
						case 187:
							ActiveDust->Velocity.X = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedX) * 0.3);
							ActiveDust->Velocity.Y = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedY) * 0.3);
							ActiveDust->Scale *= 0.7f;
							break;
						case 33:
						case 52:
						case 98:
						case 99:
						case 100:
						case 101:
						case 102:
						case 103:
						case 104:
						case 105:
							ActiveDust->Alpha = 170;
							ActiveDust->Velocity.X = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedX) * 0.5);
							ActiveDust->Velocity.Y = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedY) * 0.5 + 1.0);
							break;
						case 41:
							ActiveDust->Velocity.X = 0f;
							ActiveDust->Velocity.Y = 0f;
							break;
						case 80:
							ActiveDust->Alpha = 50;
							break;
						case 34:
							ActiveDust->Position.Y -= 8f;
							if (!Collision.WetCollision(ref ActiveDust->Position, 4, 4))
							{
								ActiveDust->Active = 0;
								NextDust = (short)DustIdx;
								return ActiveDust;
							}
							ActiveDust->Position.Y += 8f;
							ActiveDust->Velocity.X = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedX) * 0.1);
							ActiveDust->Velocity.Y = -0.5f;
							break;
						case 35:
						case 152:
							ActiveDust->Velocity.X = (float)((Main.Rand.Next(-20, 21) * 0.1 + SpeedX) * 0.1);
							ActiveDust->Velocity.Y = -0.5f;
							break;
						default:
							ActiveDust->Velocity.X = (float)(Main.Rand.Next(-20, 21) * 0.1 + SpeedX);
							ActiveDust->Velocity.Y = (float)(Main.Rand.Next(-20, 21) * 0.1 + SpeedY);
							break;
						}
						NextDust = (short)(DustIdx + 1);
						return ActiveDust;
					}
				}
				NextIdx++;
			}
			return null;
		}

		public unsafe void UpdateDust()
		{
			LavaBubbles = 0;
			SnowDust = 0;
			Vector3 RGB = default;
			fixed (Dust* CreatedDusts = Dusts)
			{
				Dust* ActiveDust = CreatedDusts;
				for (int DustIdx = PoolSize - 1; DustIdx >= 0; ActiveDust++, DustIdx--)
				{
					if (ActiveDust->Active == 0)
					{
						continue;
					}
					int DustType = ActiveDust->Type;
					float DustScale = ActiveDust->Scale;
					ActiveDust->Position.X += ActiveDust->Velocity.X;
					ActiveDust->Position.Y += ActiveDust->Velocity.Y;
					int DustX = (int)ActiveDust->Position.X;
					int DustY = (int)ActiveDust->Position.Y;

					if (ActiveDust->Type == 154)
					{
						ActiveDust->Rotation += ActiveDust->Velocity.X * 0.3f;
						ActiveDust->Scale -= 0.03f;
					}
					if (ActiveDust->Type == 172)
					{
						float A = ActiveDust->Scale * 0.5f;
						if (A > 1f)
						{
							A = 1f;
						}

						RGB.X = (A * 0f) * A;
						RGB.Y = (A * 0.25f) * A;
						RGB.Z = (A * 1f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 182)
					{
						ActiveDust->Rotation += 1f;
						float A = ActiveDust->Scale * 0.25f;
						if (A > 1f)
						{
							A = 1f;
						}

						RGB.X = (A * 1f) * A;
						RGB.Y = (A * 0.2f) * A;
						RGB.Z = (A * 0.1f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 157)
					{
						float A = ActiveDust->Scale * 0.2f;

						RGB.X = (A * 0.25f) * A;
						RGB.Y = (A * 1f) * A;
						RGB.Z = (A * 0.5f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 163)
					{
						float A = ActiveDust->Scale * 0.25f;

						RGB.X = (A * 0.25f) * A;
						RGB.Y = (A * 1f) * A;
						RGB.Z = (A * 0.05f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 170)
					{
						float A = ActiveDust->Scale * 0.5f;

						RGB.X = (A * 1f) * A;
						RGB.Y = (A * 1f) * A;
						RGB.Z = (A * 0.05f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 156)
					{
						float A = ActiveDust->Scale * 0.6f;

						RGB.X = (A * 0.5f) * A;
						RGB.Y = (A * 0.9f) * A;
						RGB.Z = (A * 1f) * A;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}
					if (ActiveDust->Type == 174)
					{
						float A = ActiveDust->Scale * 1f;
						if (A > 0.6f)
						{
							A = 0.6f;
						}

						RGB.X = A;
						RGB.Y = A * 0.4f;
						RGB.Z = 0f;
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), RGB);
					}

					switch (DustType)
					{
					case 6:
					case 29:
					case 59:
					case 60:
					case 61:
					case 62:
					case 63:
					case 64:
					case 65:
					case 75:
					case 127:
					case 135:
					case 158:
					case 167:
						if (!ActiveDust->NoLight)
						{
							if (ClipDust(DustX, DustY))
							{
								ActiveDust->Active = 0;
								continue;
							}
							DustScale *= 1.4f;
							switch (DustType)
							{
							case 6:
								if (DustScale > 0.6f)
								{
									DustScale = 0.6f;
								}
								RGB.X = DustScale;
								RGB.Y = DustScale * 0.65f;
								RGB.Z = DustScale * 0.4f;
								break;
							case 29:
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 0.1f;
								RGB.Y = DustScale * 0.4f;
								RGB.Z = DustScale;
								break;
							case 59:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = 0f;
								RGB.Y = DustScale * 0.1f;
								RGB.Z = DustScale * 1.3f;
								break;
							case 60:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = DustScale;
								RGB.Y = (RGB.Z = DustScale * 0.1f);
								break;
							case 61:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = 0f;
								RGB.Y = DustScale;
								RGB.Z = DustScale * 0.1f;
								break;
							case 62:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = (RGB.Z = DustScale * 0.9f);
								RGB.Y = 0f;
								break;
							default:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = (RGB.Y = (RGB.Z = DustScale * 1.3f));
								break;
							case 64:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = (RGB.Y = DustScale * 0.9f);
								RGB.Z = 0f;
								break;
							case 65:
								if (DustScale > 0.8f)
								{
									DustScale = 0.8f;
								}
								RGB.X = 0.5f * Main.DemonTorch + 1f * (1f - Main.DemonTorch);
								RGB.Y = 0.3f;
								RGB.Z = Main.DemonTorch + 0.5f * (1f - Main.DemonTorch);
								break;
							case 75:
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 0.7f;
								RGB.Y = DustScale;
								RGB.Z = DustScale * 0.2f;
								break;
							case 127:
								DustScale *= 1.3f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale;
								RGB.Y = DustScale * 0.45f;
								RGB.Z = DustScale * 0.2f;
								break;
							case 135:
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 0.2f;
								RGB.Y = DustScale * 0.7f;
								RGB.Z = DustScale;
								break;
							case 158:
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 1f;
								RGB.Y = DustScale * 0.5f;
								RGB.Z = 0;
								break;
							case 169:
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 1.1f;
								RGB.Y = DustScale * 1.1f;
								RGB.Z = DustScale * 0.2f;
								break;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
						}
						if (!ActiveDust->NoGravity)
						{
							ActiveDust->Velocity.Y += 0.05f;
						}
						break;
					default:
						switch (DustType)
						{
						case 14:
						case 16:
						case 46:
						case 124:
						case 186:
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							break;
						case 31:
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							if (ActiveDust->NoGravity)
							{
								ActiveDust->Alpha += 4;
								if (ActiveDust->Alpha > 255)
								{
									ActiveDust->Active = 0;
									continue;
								}
								ActiveDust->Velocity.X *= 1.02f;
								ActiveDust->Velocity.Y *= 1.02f;
								ActiveDust->Scale += 0.02f;
							}
							break;
						case 32:
							ActiveDust->Scale -= 0.01f;
							ActiveDust->Velocity.X *= 0.96f;
							ActiveDust->Velocity.Y += 0.1f;
							break;
						case 43:
							ActiveDust->Rotation += 0.1f * ActiveDust->Scale;
							if (DustScale > 0.048f)
							{
								RGB.X = (RGB.Y = (RGB.Z = DustScale * 1.01055562f));
								Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
								if (ActiveDust->Alpha < 255)
								{
									ActiveDust->Scale += 0.09f;
									if (ActiveDust->Scale >= 1f)
									{
										ActiveDust->Scale = 1f;
										ActiveDust->Alpha = 255;
									}
								}
								else if (ActiveDust->Scale < 0.5f)
								{
									ActiveDust->Scale -= 0.02f;
								}
								else if (ActiveDust->Scale < 0.8f)
								{
									ActiveDust->Scale -= 0.01f;
								}
								break;
							}
							ActiveDust->Active = 0;
							continue;
						case 15:
						case 57:
						case 58:
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							if (DustType != 15)
							{
								DustScale *= 0.8f;
							}
							if (ActiveDust->NoLight)
							{
								ActiveDust->Velocity.X *= 0.95f;
								ActiveDust->Velocity.Y *= 0.95f;
							}
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							switch (DustType)
							{
							case 15:
								Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.45f, DustScale * 0.55f, DustScale));
								break;
							case 57:
								Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.95f, DustScale * 0.95f, DustScale * 0.45f));
								break;
							case 58:
								Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale, DustScale * 0.55f, DustScale * 0.75f));
								break;
							}
							break;

						case 110:
						case 111:
						case 112:
						case 113:
						case 114:
							float ScaleAdjust = ActiveDust->Scale * 0.1f;
							if (DustType == 111)
							{
								ScaleAdjust = ActiveDust->Scale * 0.125f;
							}
							if (ScaleAdjust > 1f)
							{
								ScaleAdjust = 1f;
							}
							switch (DustType)
							{
							case 110:
								RGB.X = ScaleAdjust * 0.2f;
								RGB.Y = ScaleAdjust;
								RGB.Z = ScaleAdjust * 0.5f;
								break;
							case 111:
								RGB.X = ScaleAdjust * 0.2f;
								RGB.Y = ScaleAdjust * 0.7f;
								RGB.Z = ScaleAdjust;
								break;
							case 112:
								RGB.X = ScaleAdjust * 0.8f;
								RGB.Y = ScaleAdjust * 0.2f;
								RGB.Z = ScaleAdjust * 0.8f;
								break;
							case 113:
								RGB.X = ScaleAdjust * 0.2f;
								RGB.Y = ScaleAdjust * 0.3f;
								RGB.Z = ScaleAdjust * 1.3f;
								break;
							case 114:
								RGB.X = ScaleAdjust * 1.2f;
								RGB.Y = ScaleAdjust * 0.5f;
								RGB.Z = ScaleAdjust * 0.4f;
								break;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
							break;

						case 66:
							if (ActiveDust->Velocity.X < 0f)
							{
								ActiveDust->Rotation -= 1f;
							}
							else
							{
								ActiveDust->Rotation += 1f;
							}
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							ActiveDust->Scale += 0.02f;
							DustScale *= (1f / 318.75f);
							if (DustScale > (1f / 255f))
							{
								DustScale = (1f / 255f);
							}
							RGB.X = DustScale * ActiveDust->Color.R;
							RGB.Y = DustScale * ActiveDust->Color.G;
							RGB.Z = DustScale * ActiveDust->Color.B;
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
							break;
						case 20:
						case 21:
							ActiveDust->Scale += 0.005f;
							ActiveDust->Velocity.X *= 0.94f;
							ActiveDust->Velocity.Y *= 0.94f;
							if (DustType == 21)
							{
								DustScale *= 0.4f;
								RGB.X = DustScale * 0.8f;
								RGB.Y = DustScale * 0.3f;
							}
							else
							{
								DustScale *= 0.8f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale * 0.3f;
								RGB.Y = DustScale * 0.6f;
							}
							RGB.Z = DustScale;
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
							break;
						case 27:
						case 45:
							ActiveDust->Velocity.X *= 0.94f;
							ActiveDust->Velocity.Y *= 0.94f;
							ActiveDust->Scale += 0.002f;
							if (ActiveDust->NoLight)
							{
								DustScale *= 0.1f;
								ActiveDust->Scale -= 0.06f;
								if (ActiveDust->Scale < 1f)
								{
									ActiveDust->Scale -= 0.06f;
								}
								if (View != null)
								{
									if (View.Player.IsWet)
									{
										ActiveDust->Position.X += View.Player.velocity.X * 0.5f;
										ActiveDust->Position.Y += View.Player.velocity.Y * 0.5f;
									}
									else
									{
										ActiveDust->Position.X += View.Player.velocity.X;
										ActiveDust->Position.Y += View.Player.velocity.Y;
									}
								}
							}
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							Lighting.AddLight((int)ActiveDust->Position.X >> 4, (int)ActiveDust->Position.Y >> 4, new Vector3(DustScale * 0.6f, DustScale * 0.2f, DustScale));
							break;
						case 55:
						case 56:
						case 73:
						case 74:
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							switch (DustType)
							{
							case 55:
								DustScale *= 0.8f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB = new Vector3(DustScale, DustScale, DustScale * 0.6f);
								break;
							case 73:
								DustScale *= 0.8f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB = new Vector3(DustScale, DustScale * 0.35f, DustScale * 0.5f);
								break;
							case 74:
								DustScale *= 0.8f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB = new Vector3(DustScale * 0.35f, DustScale, DustScale * 0.5f);
								break;
							default:
								DustScale *= 1.2f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB = new Vector3(DustScale * 0.35f, DustScale * 0.5f, DustScale);
								break;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
							break;
						case 71:
						case 72:
							ActiveDust->Velocity.X *= 0.98f;
							ActiveDust->Velocity.Y *= 0.98f;
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.2f, 0f, DustScale * 0.1f));
							break;
						case 76:
							SnowDust++;
							ActiveDust->Scale += 0.009f;
							if (View != null)
							{
								if (Collision.SolidCollision(ref ActiveDust->Position, 1, 1))
								{
									ActiveDust->Active = 0;
									continue;
								}
								ActiveDust->Position.X += View.Player.velocity.X * 0.2f;
								ActiveDust->Position.Y += View.Player.velocity.Y * 0.2f;
							}
							break;
						default:
							if (!ActiveDust->NoGravity)
							{
								if (DustType != 41 && DustType != 44)
								{
									if (DustType == 107) 
									{
										ActiveDust->Velocity.Y *= 0.9f;
									}
									else
									{
										ActiveDust->Velocity.Y += 0.1f;
									}
								}
							}
							else if (DustType == 5)
							{
								ActiveDust->Scale -= 0.04f;
							}
							break;
						}
						break;
					}
					if (DustType == 33 || DustType == 52 || (DustType >= 98 && DustType <= 105) || DustType == 123)
					{
						if (ActiveDust->Velocity.X == 0f)
						{
							if (Collision.SolidCollision(ref ActiveDust->Position, 2, 2))
							{
								ActiveDust->Active = 0;
								continue;
							}
							ActiveDust->Rotation += 0.5f;
							ActiveDust->Scale -= 0.01f;
						}
						if (Collision.WetCollision(ref ActiveDust->Position, 4, 4))
						{
							ActiveDust->Scale -= 0.105f;
							ActiveDust->Alpha += 22;
						}
						else
						{
							ActiveDust->Scale -= 0.005f;
							ActiveDust->Alpha += 2;
						}
						if (ActiveDust->Alpha > 255)
						{
							ActiveDust->Active = 0;
							continue;
						}
						ActiveDust->Velocity.X *= 0.93f;
						if (ActiveDust->Velocity.Y > 4f)
						{
							ActiveDust->Velocity.Y = 4f;
						}
						if (ActiveDust->NoGravity)
						{
							if (ActiveDust->Velocity.X < 0f)
							{
								ActiveDust->Rotation -= 0.2f;
							}
							else
							{
								ActiveDust->Rotation += 0.2f;
							}
							ActiveDust->Scale += 0.03f;
							ActiveDust->Velocity.X *= 1.05f;
							ActiveDust->Velocity.Y += 0.15f;
						}
					}
					else if (DustType == 152 && ActiveDust->NoGravity)
					{
						ActiveDust->Scale += 0.03f;
						if (ActiveDust->Scale < 1f)
						{
							ActiveDust->Velocity.Y += 0.075f;
						}
						ActiveDust->Velocity.X *= 1.08f;
						if (ActiveDust->Velocity.X > 0f)
						{
							ActiveDust->Rotation += 0.01f;
						}
						else
						{
							ActiveDust->Rotation -= 0.01f;
						}
					}
					else if (DustType == 67 || DustType == 92)
					{
						if (DustScale > 1f)
						{
							DustScale = 1f;
						}
						Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(0f, DustScale * 0.8f, DustScale));
					}
					else if (ActiveDust->Type == 185)
					{
						float DustScale2 = ActiveDust->Scale;
						if (DustScale2 > 1f)
						{
							DustScale2 = 1f;
						}
						if (ActiveDust->NoLight)
						{
							DustScale2 *= 0.1f;
						}
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), new Vector3(DustScale2 * 0.1f, DustScale2 * 0.7f, DustScale2));
					}
					else if (ActiveDust->Type == 107)
					{
						float DustScale3 = ActiveDust->Scale * 0.5f;
						if (DustScale3 > 1f)
						{
							DustScale3 = 1f;
						}
						Lighting.AddLight((int)(ActiveDust->Position.X / 16f), (int)(ActiveDust->Position.Y / 16f), new Vector3(DustScale3 * 0.1f, DustScale3, DustScale3 * 0.4f));
					}
					else if (DustType == 34 || DustType == 35 || DustType == 152)
					{
						if (DustType == 35)
						{
							LavaBubbles++;
							if (ActiveDust->NoGravity)
							{
								ActiveDust->Scale += 0.03f;
								if (ActiveDust->Scale < 1f)
								{
									ActiveDust->Velocity.Y += 0.075f;
								}
								ActiveDust->Velocity.X *= 1.08f;
								if (ActiveDust->Velocity.X > 0f)
								{
									ActiveDust->Rotation += 0.01f;
								}
								else
								{
									ActiveDust->Rotation -= 0.01f;
								}
								ActiveDust->Velocity.X *= 0.99f;
								DustScale = DustScale * 0.6f + 0.018f;
								if (DustScale > 1f)
								{
									DustScale = 1f;
								}
								RGB.X = DustScale;
								RGB.Y = DustScale * 0.3f;
								RGB.Z = DustScale * 0.1f;
								Lighting.AddLight(DustX >> 4, (DustY >> 4) + 1, RGB);
								goto AfterYChange; // I could exclude this and make it an extra else-if statement, but I'm doing this.
							}
							DustScale = DustScale * 0.3f + 0.4f;
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							RGB.X = DustScale;
							RGB.Y = DustScale * 0.5f;
							RGB.Z = DustScale * 0.3f;
							Lighting.AddLight(DustX >> 4, DustY >> 4, RGB);
							ActiveDust->Scale -= 0.01f;
							ActiveDust->Velocity.Y = -0.2f;
							ActiveDust->Alpha += (short)Main.Rand.Next(2);
						}
						else if (DustType == 34)
						{
							ActiveDust->Scale += 0.005f;
							ActiveDust->Velocity.Y = -0.5f;
						}
						else
						{
							ActiveDust->Scale -= 0.01f;
							ActiveDust->Alpha++;
							ActiveDust->Velocity.Y = -0.7f;
						}
						if (++ActiveDust->Alpha > 255)
						{
							ActiveDust->Active = 0;
							continue;
						}
						ActiveDust->Position.Y -= 8f;
						if (!Collision.WetCollision(ref ActiveDust->Position, 4, 4))
						{
							ActiveDust->Active = 0;
							continue;
						}
						ActiveDust->Position.Y += 8f;
						ActiveDust->Velocity.X += Main.Rand.Next(-10, 10) * 0.002f;
						if (ActiveDust->Velocity.X < -0.25f)
						{
							ActiveDust->Velocity.X = -0.25f;
						}
						else if (ActiveDust->Velocity.X > 0.25f)
						{
							ActiveDust->Velocity.X = 0.25f;
						}
					}
					else
					{
						switch (DustType)
						{
						case 68:
							DustScale *= 0.3f;
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.1f, DustScale * 0.2f, DustScale));
							break;
						case 70:
							DustScale *= 0.3f;
							if (DustScale > 1f)
							{
								DustScale = 1f;
							}
							Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.5f, 0f, DustScale));
							break;
						}
					}
					switch (DustType)
					{
					case 41:
						ActiveDust->Velocity.X += Main.Rand.Next(-10, 11) * 0.01f;
						ActiveDust->Velocity.Y += Main.Rand.Next(-10, 11) * 0.01f;
						if (ActiveDust->Velocity.X > 0.75f)
						{
							ActiveDust->Velocity.X = 0.75f;
						}
						else if (ActiveDust->Velocity.X < -0.75f)
						{
							ActiveDust->Velocity.X = -0.75f;
						}
						if (ActiveDust->Velocity.Y > 0.75f)
						{
							ActiveDust->Velocity.Y = 0.75f;
						}
						else if (ActiveDust->Velocity.Y < -0.75f)
						{
							ActiveDust->Velocity.Y = -0.75f;
						}
						ActiveDust->Scale += 0.007f;
						DustScale = DustScale * 0.7f + 0.0049f;
						if (DustScale > 1f)
						{
							DustScale = 1f;
						}
						Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.4f, DustScale * 0.9f, DustScale));
						break;
					case 44:
						ActiveDust->Velocity.X += Main.Rand.Next(-10, 11) * 0.003f;
						ActiveDust->Velocity.Y += Main.Rand.Next(-10, 11) * 0.003f;
						if (ActiveDust->Velocity.X > 0.35f)
						{
							ActiveDust->Velocity.X = 0.35f;
						}
						else if (ActiveDust->Velocity.X < -0.35f)
						{
							ActiveDust->Velocity.X = -0.35f;
						}
						if (ActiveDust->Velocity.Y > 0.35f)
						{
							ActiveDust->Velocity.Y = 0.35f;
						}
						else if (ActiveDust->Velocity.Y < -0.35f)
						{
							ActiveDust->Velocity.Y = -0.35f;
						}
						ActiveDust->Scale += 0.0085f;
						DustScale = DustScale * 0.7f + 0.00595f;
						if (DustScale > 1f)
						{
							DustScale = 1f;
						}
						Lighting.AddLight(DustX >> 4, DustY >> 4, new Vector3(DustScale * 0.7f, DustScale, DustScale * 0.8f));
						break;
					default:
						ActiveDust->Velocity.X *= 0.99f;
						break;
					}
					AfterYChange:
					if (ActiveDust->FadeIn > 0f)
					{
						if (DustType == 46)
						{
							ActiveDust->Scale += 0.1f;
						}
						else
						{
							ActiveDust->Scale += 0.03f;
						}
						if (ActiveDust->Scale > ActiveDust->FadeIn)
						{
							ActiveDust->FadeIn = 0f;
						}
					}
					else
					{
						ActiveDust->Scale -= 0.01f;
					}
					if (ActiveDust->NoGravity)
					{
						ActiveDust->Velocity.X *= 0.92f;
						ActiveDust->Velocity.Y *= 0.92f;
						if (ActiveDust->FadeIn == 0f)
						{
							ActiveDust->Scale -= 0.04f;
						}
					}
					if (ActiveDust->Scale < 0.1f)
					{
						ActiveDust->Active = 0;
					}
					else if (DustType != 79)
					{
						ActiveDust->Rotation += ActiveDust->Velocity.X * 0.5f;
					}
				}
			}
		}

		public unsafe void DrawDust(WorldView DrawView)
		{
			fixed (Dust* CreatedDusts = Dusts)
			{
				Dust* DustIdx = CreatedDusts;
				Vector2 Pivot = new Vector2(4f, 4f);
				Vector2 DustPos = default;
				Rectangle value2 = new Rectangle(DrawView.ScreenPosition.X - 500, DrawView.ScreenPosition.Y - 50, Main.ResolutionWidth + 1000, Main.ResolutionHeight + 100);
				for (int SecondIdx = PoolSize - 1; SecondIdx >= 0; SecondIdx--)
				{
					if (DustIdx->Active != 0)
					{
						int DustX = (int)DustIdx->Position.X;
						int DustY = (int)DustIdx->Position.Y;
#if !VERSION_INITIAL
						if (DustIdx->Type >= 130 && DustIdx->Type <= 134)
						{
							value2.X -= 500;
							value2.Y -= 500;
							value2.Width += 1000;
							value2.Height += 1000;
						}
						if (View != null || new Rectangle(DustX, DustY, (int)Pivot.X, (int)Pivot.Y).Intersects(value2))
						{
							if (DustIdx->Type >= 130 && DustIdx->Type <= 134)
							{
								float Acceleration = Math.Abs(DustIdx->Velocity.X) + Math.Abs(DustIdx->Velocity.Y);
								Acceleration *= 0.3f;
								Acceleration *= 10f;
								if (Acceleration > 10f)
								{
									Acceleration = 10f;
								}
								for (int AccelStep = 0; AccelStep < Acceleration; AccelStep++)
								{
									Vector2 RDustVelocity = DustIdx->Velocity;
									Vector2 RDustDirection = DustIdx->Position - RDustVelocity * AccelStep;
									float RDustScale = DustIdx->Scale * (1f - AccelStep / 10f);
									Color RDustColor = ((!DustIdx->NoLight && DustIdx->Type != 6 && DustIdx->Type != 15 && (DustIdx->Type < 59 || DustIdx->Type > 64)) ? DrawView.Lighting.GetColor(DustX + 4 >> 4, DustY + 4 >> 4) : Color.White);
									DustIdx->GetAlpha(ref RDustColor);
									DustPos.X = RDustDirection.X - DrawView.ScreenPosition.X;
									DustPos.Y = RDustDirection.Y - DrawView.ScreenPosition.Y;
									SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.DUST, ref DustPos, ref DustIdx->Frame, RDustColor, DustIdx->Rotation, ref Pivot, RDustScale);
								}
							}
							Color DustColor = ((!DustIdx->NoLight || DustIdx->Type >= 86 || DustIdx->Type <= 91) && DustIdx->Type != 6 && DustIdx->Type != 15 && (DustIdx->Type < 59 || DustIdx->Type > 64)) ? DrawView.Lighting.GetColor(DustX + 4 >> 4, DustY + 4 >> 4) : Color.White;
#else
						if (View != null || DrawView.ClipArea.Contains(DustX, DustY))
						{
							Color DustColor = (!DustIdx->NoLight && DustIdx->Type != 6 && DustIdx->Type != 15 && (DustIdx->Type < 59 || DustIdx->Type > 64)) ? DrawView.Lighting.GetColor(DustX + 4 >> 4, DustY + 4 >> 4) : Color.White;
#endif
							DustIdx->GetAlpha(ref DustColor);
							if (DustColor.PackedValue == 0)
							{
								DustIdx->Active = 0;
							}
							else
							{
								DustPos.X = DustX - DrawView.ScreenPosition.X;
								DustPos.Y = DustY - DrawView.ScreenPosition.Y;
								SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.DUST, ref DustPos, ref DustIdx->Frame, DustColor, DustIdx->Rotation, ref Pivot, DustIdx->Scale);
								if (DustIdx->Color.PackedValue != 0)
								{
									DustIdx->GetColor(ref DustColor);
									SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.DUST, ref DustPos, ref DustIdx->Frame, DustColor, DustIdx->Rotation, ref Pivot, DustIdx->Scale);
								}
							}
						}
					}
					DustIdx++;
				}
			}
		}
	}
}
