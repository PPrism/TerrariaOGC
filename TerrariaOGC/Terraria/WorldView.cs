using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.Tile;

namespace Terraria
{
	public class WorldView : IDisposable
	{
		public enum Type
		{
			FULLSCREEN,
			TOP_LEFT,
			TOP_RIGHT,
			BOTTOM_LEFT,
			BOTTOM_RIGHT,
			TOP,
			BOTTOM,
			NONE
		}

		private struct Spec
		{
			public short X;

			public short Y;

			public Color tileColor;
		}

		public const bool DRAW_TO_SCREEN = false;

		public const int OFFSCREEN_RANGE_X = 32;

		public const int OFFSCREEN_RANGE_TOP = 32;

		public const int OFFSCREEN_RANGE_BOTTOM = 64;

		public const int OFFSCREEN_RANGE_VERTICAL = 96;

		public const float CAVE_PARALLAX = 0.9f;

		public const float gfxQuality = 0.25f;

		private const double VIEWPORT_ANIM_STEPS = 30.0;

		private const double VIEWPORT_ANIM_THETA_DELTA = Math.PI / 60.0;

		private const double ZOOM_ANIM_STEPS = 90.0;

		private const double ZOOM_ANIM_THETA_DELTA = Math.PI / 180.0;

		private const int CLIP_AREA_EXTRA_WIDTH = 32;

		private const int CLIP_AREA_EXTRA_HEIGHT = 64;

		public const int MAX_BACKGROUNDS = 32;

		private bool SMOOTH_LIGHT = true;

		private bool isDisposed;

		public short ViewWidth;

#if VERSION_103 || VERSION_FINAL
		public Type viewType = Type.NONE;
		public float cameraX;
		public float cameraY;
		public int PlayerSafeAreaL = 0;
		public int PlayerSafeAreaT = 0;
#else
		private Type viewType = Type.NONE;
#endif
		public int SafeAreaOffsetLeft = Main.ResolutionWidth / 20;

		public int SafeAreaOffsetTop = Main.ResolutionHeight / 20;

		public int SafeAreaOffsetRight = Main.ResolutionWidth / 20;

		public int SafeAreaOffsetBottom = Main.ResolutionHeight / 20;

		private int currentSAFE_AREA_OFFSET_L = Main.ResolutionWidth / 20;

		private int currentSAFE_AREA_OFFSET_T = Main.ResolutionHeight / 20;

		private int currentSAFE_AREA_OFFSET_R = Main.ResolutionWidth / 20;

		private int currentSAFE_AREA_OFFSET_B = Main.ResolutionHeight / 20;

		private int targetSAFE_AREA_OFFSET_L = Main.ResolutionWidth / 20;

		private int targetSAFE_AREA_OFFSET_T = Main.ResolutionHeight / 20;

		private int targetSAFE_AREA_OFFSET_R = Main.ResolutionWidth / 20;

		private int targetSAFE_AREA_OFFSET_B = Main.ResolutionHeight / 20;

		private Viewport currentViewport;

		private Viewport targetViewport;

		public Viewport ActiveViewport;

		private double viewportAnimTheta;

		public Player Player;

		public UI Ui;

		public Time WorldTime;

		public float AtmoOpacity;

		public short FirstTileX;

		public short LastTileX;

		public short FirstTileY;

		public short LastTileY;

		public Rectangle ClipArea = new Rectangle(0, 0, 0, Main.ResolutionHeight + CLIP_AREA_EXTRA_HEIGHT);

		public Rectangle ViewArea = new Rectangle(0, 0, 0, Main.ResolutionHeight);

		public Vector2i ScreenPosition;

		public Vector2i ScreenLastPosition;

		public int quickBG = 2;

		public int bgDelay;

		public int bgStyle;

		public float[] bgAlpha = new float[8];

		public float[] bgAlpha2 = new float[8];

		public DustPool dustLocal;

		public ItemTextPool itemTextLocal;

		public Lighting Lighting = new Lighting();

		public int InactiveTiles;

		public int SandTiles;

		public int EvilTiles;

		public int SnowTiles;

		public int HolyTiles;

		public int MeteorTiles;

		public int JungleTiles;

		public int DungeonTiles;

#if !USE_ORIGINAL_CODE
		public int SkyTiles;
#endif

		public int MusicBox = -1;

		public bool[] drawNpcName = new bool[NPC.MaxNumNPCs];

		public Vector2i sceneWaterPos = default(Vector2i);

		public Vector2i sceneTilePos = default(Vector2i);

		public Vector2i sceneTile2Pos = default(Vector2i);

		public Vector2i sceneWallPos = default(Vector2i);

		public Vector2i sceneBackgroundPos = default(Vector2i);

		public Vector2i sceneBlackPos = default(Vector2i);

		private RenderTarget2D backWaterTarget;

		private RenderTarget2D waterTarget;

		private RenderTarget2D tileSolidTarget;

		private RenderTarget2D blackTarget;

		private RenderTarget2D tileNonSolidTarget;

		private RenderTarget2D wallTarget;

		private RenderTarget2D backgroundTarget;

		private static readonly short ViewportWidth = (short)Main.ResolutionWidth; // These 2 are here bc it looks neater

		private static readonly short ViewportHeight = (short)Main.ResolutionHeight;

		public static readonly short[] VIEW_WIDTH = new short[7]
		{
			ViewportWidth,
			ViewportWidth,
			ViewportWidth,
			ViewportWidth,
			ViewportWidth,
			(short)(ViewportWidth * 2),
			(short)(ViewportWidth * 2)
		};

		public static readonly byte[] SAFE_AREA_OFFSETS = new byte[28]
		{
			(byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X,
			(byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y,
			(byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X,
			(byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2),
			0,
			0,
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			0,
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2),
			0,
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			0,
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_X * 2),
			(byte)((byte)UI.FULLSCREEN_SAFE_AREA_OFFSET_Y * 2)
		};

		public static readonly Viewport[] VIEWPORT = new Viewport[7]
		{
			new Viewport(0, 0, ViewportWidth, ViewportHeight),
			new Viewport(0, 0, (ViewportWidth / 2), (ViewportHeight / 2)),
			new Viewport((ViewportWidth / 2), 0, (ViewportWidth / 2), (ViewportHeight / 2)),
			new Viewport(0, (ViewportHeight / 2), (ViewportWidth / 2), (ViewportHeight / 2)),
			new Viewport((ViewportWidth / 2), (ViewportHeight / 2), (ViewportWidth / 2), (ViewportHeight / 2)),
			new Viewport(0, 0, ViewportWidth, (ViewportHeight / 2)),
			new Viewport(0, (ViewportHeight / 2), ViewportWidth, (ViewportHeight / 2))
		};

		public static GraphicsDevice GraphicsDevice;

		private static Matrix halfpixelOffset;

		private static Matrix centerWideSplitscreen;

		private BasicEffect renderTargetProjection;

		public BasicEffect ScreenProjection;

		public static RasterizerState ScissorRastTest;

		private Matrix viewMtx = Matrix.Identity;

		private float worldScale = 1f;

		private float worldScaleTarget = 1f;

		private float worldScalePrevious = 1f;

		private double worldScaleAnimTheta;

		public static Texture2D[] backgroundTexture = new Texture2D[32]; // Made for 32, despite only being filled with 29 entries...

		private Spec[] spec = new Spec[512];

		public static Type getViewType(PlayerIndex controller, UI newUI = null)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 4; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI == newUI || uI.CurrentView != null)
				{
					num2++;
					if (uI.controller < controller)
					{
						num++;
					}
				}
			}
			switch (num2)
			{
			case 2:
				return (Type)(5 + num);
			case 3:
				switch (num)
				{
				case 0:
					return Type.TOP;
				case 1:
					return Type.BOTTOM_LEFT;
				default:
					return Type.BOTTOM_RIGHT;
				}
			case 4:
				return (Type)(1 + num);
			default:
				return Type.FULLSCREEN;
			}
		}

		public static bool AnyViewContains(int X, int Y)
		{
			int num = UI.NumActiveViews;
			do
			{
				if (UI.activeView[--num].ClipArea.Contains(X, Y))
				{
					return true;
				}
			}
			while (num > 0);
			return false;
		}

		public static bool AnyViewIntersects(ref Rectangle rect)
		{
			int num = UI.NumActiveViews;
			bool result;
			do
			{
				UI.activeView[--num].ClipArea.Intersects(ref rect, out result);
			}
			while (!result && num > 0);
			return result;
		}

		public static void LoadContent(ContentManager Content)
		{
			for (int i = 3; i < MAX_BACKGROUNDS; i++)
			{
				backgroundTexture[i] = Content.Load<Texture2D>("Images/Background_" + i);
			}
		}

		public static void Initialize(GraphicsDevice device)
		{
			GraphicsDevice = device;
#if USE_ORIGINAL_CODE
			Matrix.CreateTranslation(0.5f, 0.5f, 0f, out halfpixelOffset); // In D3D9, there exists a 'half-pixel offset' to handle rasterization, something not required in modern render drivers (D3D11, OpenGL, Vulkan).
#else
			Matrix.CreateTranslation(0.0f, 0.0f, 0f, out halfpixelOffset); // Since this is not required in the drivers used for FNA3D (which is not D3D9), having an offset to the pixels leads to gaps between each and every tile.
#endif
			Matrix.CreateTranslation(Main.ResolutionWidth / 2, 0f, 0f, out centerWideSplitscreen);
			ScissorRastTest = new RasterizerState();
			ScissorRastTest.ScissorTestEnable = true;
		}

		public WorldView()
		{
			dustLocal = new DustPool(this, Dust.MaxNumLocalDust);
			itemTextLocal = new ItemTextPool(this);
			bgAlpha[0] = 1f;
			bgAlpha2[0] = 1f;
			renderTargetProjection = new BasicEffect(GraphicsDevice)
			{
				World = Matrix.Identity,
				View = Matrix.Identity,
				TextureEnabled = true,
				VertexColorEnabled = true
			};
			ScreenProjection = new BasicEffect(GraphicsDevice)
			{
				World = Matrix.Identity,
				View = Matrix.Identity,
				TextureEnabled = true,
				VertexColorEnabled = true
			};
		}

		public void OnStartGame()
		{
            Lighting.StartWorkerThread();
        }

        public void onStopGame()
		{
            Lighting.StopWorkerThread();
            itemTextLocal.Clear();
		}

		public bool setViewType(Type type = Type.FULLSCREEN)
		{
			if (type == viewType)
			{
				return false;
			}
			if (type != 0)
			{
				SMOOTH_LIGHT = false;
			}
			else
			{
				SMOOTH_LIGHT = true;
				Zoom(1f);
			}
			Type type2 = viewType;
			bool flag = IsFullScreen();
			int num = ViewWidth;
			viewType = type;
			if (type != Type.NONE)
			{
				int num2 = VIEW_WIDTH[(int)type];
				if (num != num2)
				{
					ViewWidth = (short)num2;
					ClipArea.Width = num2 + 32;
					ViewArea.Width = num2;
					Lighting.SetWidth(num2);
				}
				currentViewport = ActiveViewport;
				targetViewport = VIEWPORT[(int)type];
				currentSAFE_AREA_OFFSET_L = SafeAreaOffsetLeft;
				currentSAFE_AREA_OFFSET_T = SafeAreaOffsetTop;
				currentSAFE_AREA_OFFSET_R = SafeAreaOffsetRight;
				currentSAFE_AREA_OFFSET_B = SafeAreaOffsetBottom;
				targetSAFE_AREA_OFFSET_L = SAFE_AREA_OFFSETS[(int)type << 2];
				targetSAFE_AREA_OFFSET_T = SAFE_AREA_OFFSETS[((int)type << 2) + 1];
				targetSAFE_AREA_OFFSET_R = SAFE_AREA_OFFSETS[((int)type << 2) + 2];
				targetSAFE_AREA_OFFSET_B = SAFE_AREA_OFFSETS[((int)type << 2) + 3];
				if (type2 != Type.NONE && flag == IsFullScreen() && num == ViewWidth)
				{
					viewportAnimTheta = Math.PI / 2.0;
					return false;
				}
				SafeAreaOffsetLeft = targetSAFE_AREA_OFFSET_L;
				SafeAreaOffsetTop = targetSAFE_AREA_OFFSET_T;
				SafeAreaOffsetRight = targetSAFE_AREA_OFFSET_R;
				SafeAreaOffsetBottom = targetSAFE_AREA_OFFSET_B;

				ActiveViewport = targetViewport;
				viewportAnimTheta = 0.0;
				UpdateProjection();
				
				int num3 = ViewWidth + (OFFSCREEN_RANGE_X * 2); // verify this
				int num4 = ViewportHeight + OFFSCREEN_RANGE_VERTICAL;
				if (backWaterTarget == null || backWaterTarget.Width != num3 || backWaterTarget.Height != num4)
				{
					DisposeRendertargets();
					backWaterTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					waterTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					blackTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					tileSolidTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					tileNonSolidTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					wallTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
					backgroundTarget = new RenderTarget2D(GraphicsDevice, num3, num4, mipMap: false, SurfaceFormat.Color, DepthFormat.None);
				}
				return true;
			}
			DisposeRendertargets();
			return true;
		}

		public void Dispose()
		{
            Lighting.StopWorkerThread();
            Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void DisposeRendertargets()
		{
			if (backWaterTarget != null)
			{
				backWaterTarget.Dispose();
				backWaterTarget = null;
				waterTarget.Dispose();
				waterTarget = null;
				tileSolidTarget.Dispose();
				tileSolidTarget = null;
				blackTarget.Dispose();
				blackTarget = null;
				tileNonSolidTarget.Dispose();
				tileNonSolidTarget = null;
				wallTarget.Dispose();
				wallTarget = null;
				backgroundTarget.Dispose();
				backgroundTarget = null;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					DisposeRendertargets();
				}
				isDisposed = true;
			}
		}

		public bool IsFullScreen()
		{
			return viewType == Type.FULLSCREEN;
		}

		public static void RestoreViewport()
		{
			GraphicsDevice.Viewport = VIEWPORT[0];
		}

		public void PrepareDraw(int pass)
		{
			ScreenLastPosition = ScreenPosition;
			Player.updateScreenPosition();
			if (ScreenPosition.X < (Lighting.OffScreenTiles * 16) + (OFFSCREEN_RANGE_X / 2))
			{
				ScreenPosition.X = (Lighting.OffScreenTiles * 16) + (OFFSCREEN_RANGE_X / 2);
			}
			else if (ScreenPosition.X + ViewWidth > Main.RightWorld - (Lighting.OffScreenTiles * 16) - OFFSCREEN_RANGE_X)
			{
				ScreenPosition.X = Main.RightWorld - ViewWidth - (Lighting.OffScreenTiles * 16) - OFFSCREEN_RANGE_X;
			}
			if (ScreenPosition.Y < (Lighting.OffScreenTiles * 16) + (OFFSCREEN_RANGE_TOP / 2))
			{
				ScreenPosition.Y = (Lighting.OffScreenTiles * 16) + (OFFSCREEN_RANGE_TOP / 2);
			}
			else if (ScreenPosition.Y + Main.ResolutionHeight > Main.BottomWorld - (Lighting.OffScreenTiles * 16) - (OFFSCREEN_RANGE_BOTTOM / 2))
			{
				ScreenPosition.Y = Main.BottomWorld - Main.ResolutionHeight - (Lighting.OffScreenTiles * 16) - (OFFSCREEN_RANGE_BOTTOM / 2);
			}
			ViewArea.X = ScreenPosition.X;
			ViewArea.Y = ScreenPosition.Y;
			ClipArea.X = ScreenPosition.X - (CLIP_AREA_EXTRA_WIDTH / 2);
			ClipArea.Y = ScreenPosition.Y - (CLIP_AREA_EXTRA_HEIGHT / 2);
			FirstTileX = (short)(ScreenPosition.X >> 4);
			FirstTileY = (short)(ScreenPosition.Y >> 4);
			LastTileX = (short)(FirstTileX + (ViewWidth >> 4));
			if (LastTileX > Main.MaxTilesX)
			{
				LastTileX = Main.MaxTilesX;
			}
			LastTileY = (short)(FirstTileY + (ViewportHeight >> 4));
			if (LastTileY > Main.MaxTilesY)
			{
				LastTileY = Main.MaxTilesY;
			}
			Lighting.LightTiles(this);
#if USE_ORIGINAL_CODE
			FirstTileX -= 2;
			if (FirstTileX < 0)
			{
				FirstTileX = 0;
			}
			FirstTileY -= 2;
			if (FirstTileY < 0)
			{
				FirstTileY = 0;
			}
			LastTileX += 2;
			if (LastTileX > Main.MaxTilesX)
			{
				LastTileX = Main.MaxTilesX;
			}
			LastTileY += 4;
#else
			FirstTileX -= (short)(2f * Main.ScreenMultiplier);
			if (FirstTileX < 0)
			{
				FirstTileX = 0;
			}
			FirstTileY -= (short)(2f * Main.ScreenMultiplier);
			if (FirstTileY < 0)
			{
				FirstTileY = 0;
			}
			LastTileX += (short)(2f * Main.ScreenMultiplier);
			if (LastTileX > Main.MaxTilesX)
			{
				LastTileX = Main.MaxTilesX;
			}
			LastTileY += (short)(4f * Main.ScreenMultiplier);
#endif
			if (LastTileY > Main.MaxTilesY)
			{
				LastTileY = Main.MaxTilesY;
			}
			if (pass > 0)
			{
				Vector2i vector2i = ScreenPosition;
				ScreenPosition.X &= -2;
				ScreenPosition.Y &= -2;
				switch (pass)
				{
				case 1:
					RenderBackground();
					break;
				case 2:
					if (FirstTileY <= Main.WorldSurface)
					{
						RenderBlack();
					}
					RenderSolidTiles();
					break;
				case 3:
					RenderWalls();
					break;
#if (!IS_PATCHED && VERSION_INITIAL)
				default:
#else
				case 4:
#endif
					RenderNonSolidTiles();
					RenderBackWater();
					RenderWater();
					break;
				}
				ScreenPosition = vector2i;
			}
			if (worldScaleAnimTheta > 0.0)
			{
				worldScaleAnimTheta -= Math.PI / 180.0;
				if (worldScaleAnimTheta <= 0.0)
				{
					worldScale = worldScaleTarget;
				}
				else
				{
					double num = 1.0 - Math.Sin(worldScaleAnimTheta);
					worldScale = (float)(worldScalePrevious + num * (worldScaleTarget - worldScalePrevious));
				}
				UpdateView();
			}
			ScreenProjection.View = viewMtx;
		}

		public void Zoom(float z)
		{
			if (z != worldScaleTarget)
			{
				worldScaleTarget = z;
				worldScalePrevious = worldScale;
				worldScaleAnimTheta = Math.PI / 2.0;
			}
		}

		private void UpdateProjection()
		{
			if (IsFullScreen())
			{
				ScreenProjection.Projection = halfpixelOffset * Matrix.CreateOrthographicOffCenter(0f, ActiveViewport.Width, ActiveViewport.Height, 0f, 0f, 1f);
			}
			else
			{
				ScreenProjection.Projection = halfpixelOffset * Matrix.CreateOrthographicOffCenter(0f, ActiveViewport.Width << 1, ActiveViewport.Height << 1, 0f, 0f, 1f);
			}
			renderTargetProjection.Projection = halfpixelOffset * Matrix.CreateOrthographicOffCenter(0f, ViewWidth + (OFFSCREEN_RANGE_X * 2), ViewportHeight + OFFSCREEN_RANGE_VERTICAL, 0f, 0f, 1f);
		}

		private void UpdateView()
		{
			int num = ViewWidth;
			int num2 = Main.ResolutionHeight;
			num >>= 1;
			num2 >>= 1;
			viewMtx = Matrix.CreateTranslation(-num, -num2, 0f) * Matrix.CreateScale(worldScale, worldScale, 1f) * Matrix.CreateTranslation(num, num2, 0f);
		}

		public void SetWorldView()
		{
			Main.SpriteBatch.End();
			ScreenProjection.View = viewMtx;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ScreenProjection);
		}

		public void SetScreenView()
		{
			Main.SpriteBatch.End();
			ScreenProjection.View = Matrix.Identity;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ScreenProjection);
		}

		public void SetScreenViewWideCentered()
		{
			Main.SpriteBatch.End();
			ScreenProjection.View = ((ViewWidth == ViewportWidth) ? Matrix.Identity : centerWideSplitscreen);
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ScreenProjection);
		}

		private void DrawCombatText()
		{
			for (int i = 0; i < Main.MaxNumCombatText; i++)
			{
				if (Main.CombatTextSet[i].Active != 0)
				{
					Vector2 textSize = Main.CombatTextSet[i].TextSize;
					textSize.X *= 0.5f;
					textSize.Y *= 0.5f;
					int num = (Main.CombatTextSet[i].Crit ? 1 : 0);
					float scale = Main.CombatTextSet[i].Scale;
					float num2 = Main.CombatTextSet[i].Alpha * scale;
					Vector2 position = Main.CombatTextSet[i].Position;
					position.X += textSize.X;
					position.X -= ScreenPosition.X;
					position.Y += textSize.Y;
					position.Y -= ScreenPosition.Y;

#if USE_ORIGINAL_CODE
					Color TextColor = new Color(num2, num2, num2, num2);
#else
					Color TextColor;
					switch (Main.CombatTextSet[i].Type) // ADDITION: Something like this is featured in later updates but for QoL purposes, I added this so you can tell between your own damage and the enemy's.
					{
						case 1: // Player Damage
							TextColor = new Color(255, 80, 90, 255);
							break;

						case 2: // Health Restoration
							TextColor = new Color(100, 255, 100, 255);
							break;

						case 3: // Mana Restoration
							TextColor = new Color(100, 100, 255, 255);
							break;

						default: // General
							TextColor = new Color(num2, num2, num2, num2);
							break;
					}
#endif

					UI.DrawString(UI.CombatTextFont[num], Main.CombatTextSet[i].Text, position, TextColor, Main.CombatTextSet[i].Rotation, textSize, scale);
				}
			}
		}

		private void DrawItemText()
		{
			for (int i = 0; i < ItemTextPool.MaxNumItemTexts; i++)
			{
				if (itemTextLocal.ItemText[i].Active != 0)
				{
					Vector2 textSize = itemTextLocal.ItemText[i].TextSize;
					textSize.X *= 0.5f;
					textSize.Y *= 0.5f;
					float num = itemTextLocal.ItemText[i].Color.R;
					float num2 = itemTextLocal.ItemText[i].Color.G;
					float num3 = itemTextLocal.ItemText[i].Color.B;
					float num4 = itemTextLocal.ItemText[i].Color.A;
					float scale = itemTextLocal.ItemText[i].Scale;
					_ = itemTextLocal.ItemText[i].Alpha;
					num *= scale;
					num3 *= scale;
					num2 *= scale;
					num4 *= scale;
					Vector2 position = itemTextLocal.ItemText[i].Position;
					position.X += textSize.X;
					position.X -= ScreenPosition.X;
					position.Y += textSize.Y;
					position.Y -= ScreenPosition.Y;
					UI.DrawStringScaled(UI.BoldSmallFont, itemTextLocal.ItemText[i].Text, position, new Color((int)num, (int)num2, (int)num3, (int)num4), textSize, scale);
				}
			}
		}

		private void DrawProjectiles()
		{
			for (int num = Projectile.MaxNumProjs - 1; num >= 0; num--)
			{
				if (Main.ProjectileSet[num].active != 0 && Main.ProjectileSet[num].type > 0 && !Main.ProjectileSet[num].hide)
				{
					Main.ProjectileSet[num].Draw(this);
				}
			}
		}

		private void DrawPlayers()
		{
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				Player player = Main.PlayerSet[i];
				if (player.Active == 0)
				{
					continue;
				}
				if (player.ghost)
				{
					Vector2 position = player.Position;
					player.Position = player.shadowPos[0];
					player.shadow = 0.5f;
					player.DrawGhost(this);
					player.Position = player.shadowPos[1];
					player.shadow = 0.7f;
					player.DrawGhost(this);
					player.Position = player.shadowPos[2];
					player.shadow = 0.9f;
					player.DrawGhost(this);
					player.Position = position;
					player.shadow = 0f;
					player.DrawGhost(this);
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				if (player.legs == 25)
				{
					flag = true;
				}
				else if (player.head == 5 && player.body == 5 && player.legs == 5)
				{
					flag = true;
				}
				else if (player.head == 7 && player.body == 7 && player.legs == 7)
				{
					flag = true;
				}
				else if (player.head == 22 && player.body == 14 && player.legs == 14)
				{
					flag = true;
				}
				else if (player.body == 17 && player.legs == 16 && (player.head == 29 || player.head == 30 || player.head == 31))
				{
					flag = true;
				}
				if (player.legs == 26)
				{
					flag2 = true;
				}
				else if (player.body == 19 && player.legs == 18)
				{
					if (player.head == 35 || player.head == 36 || player.head == 37)
					{
						flag2 = true;
					}
				}
				else if (player.body == 24 && player.legs == 23 && (player.head == 41 || player.head == 42 || player.head == 43))
				{
					flag2 = true;
					flag = true;
				}
				if (flag2)
				{
					Vector2 position2 = player.Position;
					player.ghostFade += player.ghostDir * 0.075f;
					if (player.ghostFade < 0.1f)
					{
						player.ghostDir = 1f;
						player.ghostFade = 0.1f;
					}
					if (player.ghostFade > 0.9f)
					{
						player.ghostDir = -1f;
						player.ghostFade = 0.9f;
					}
					player.Position.X = position2.X - player.ghostFade * 5f;
					player.shadow = player.ghostFade;
					player.Draw(this);
					player.Position.X = position2.X + player.ghostFade * 5f;
					player.shadow = player.ghostFade;
					player.Draw(this);
					player.Position = position2;
					player.Position.Y = position2.Y - player.ghostFade * 5f;
					player.shadow = player.ghostFade;
					player.Draw(this);
					player.Position.Y = position2.Y + player.ghostFade * 5f;
					player.shadow = player.ghostFade;
					player.Draw(this);
					player.Position = position2;
					player.shadow = 0f;
				}
				if (flag)
				{
					Vector2 position3 = player.Position;
					player.Position = player.shadowPos[0];
					player.shadow = 0.5f;
					player.Draw(this);
					player.Position = player.shadowPos[1];
					player.shadow = 0.7f;
					player.Draw(this);
					player.Position = player.shadowPos[2];
					player.shadow = 0.9f;
					player.Draw(this);
					player.Position = position3;
					player.shadow = 0f;
				}
				player.Draw(this);
			}
		}

		private unsafe void DrawItems()
		{
			Rectangle rectangle = default(Rectangle);
			Vector2 pos = default(Vector2);
			fixed (Item* ptr = Main.ItemSet)
			{
				Item* ptr2 = ptr;
				for (int num = Main.MaxNumItems - 1; num >= 0; num--)
				{
					if (ptr2->Active != 0)
					{
						int num2 = (int)ptr2->Position.X;
						int num3 = (int)ptr2->Position.Y;
						int num4 = (int)_sheetSprites.ID.ITEM_1 - 1 + ptr2->Type;
						rectangle.Width = SpriteSheet<_sheetSprites>.Source[num4].Width;
						rectangle.Height = SpriteSheet<_sheetSprites>.Source[num4].Height;
						rectangle.X = num2 + (ptr2->Width >> 1);
						rectangle.Y = num3 + (rectangle.Height >> 1) + ptr2->Height - rectangle.Height + 2;
						if (rectangle.Intersects(ViewArea))
						{
							int x = num2 + (ptr2->Width >> 1) >> 4;
							int y = num3 + (ptr2->Height >> 1) >> 4;
							Color colorUnsafe = Lighting.GetColorUnsafe(x, y);
							if ((ptr2->CanBePlacedInCoinSlot() || ptr2->Type == (int)Item.ID.HEART || ptr2->Type == (int)Item.ID.MANA_CRYSTAL) && Main.HasFocus && colorUnsafe.R > 60)
							{
								float num5 = Main.Rand.Next(500) - (Math.Abs(ptr2->Velocity.X) + Math.Abs(ptr2->Velocity.Y)) * 10f;
								if (num5 < colorUnsafe.R * 0.02f)
								{
									Dust* ptr3 = dustLocal.NewDust(num2, num3, ptr2->Width, ptr2->Height, 43, 0.0, 0.0, 254, default, 0.5);
									if (ptr3 != null)
									{
										ptr3->Velocity.X = 0f;
										ptr3->Velocity.Y = 0f;
									}
								}
							}
							float rot = ptr2->Velocity.X * 0.2f;
							float num6 = 1f;
							Color alpha = ptr2->GetAlpha(colorUnsafe);
							if (ptr2->Type == (int)Item.ID.HEART || ptr2->Type == (int)Item.ID.STAR)
							{
								num6 = UI.PulseScale * 0.25f + 0.75f;
								alpha.R = (byte)(alpha.R * num6);
								alpha.G = (byte)(alpha.G * num6);
								alpha.B = (byte)(alpha.B * num6);
								alpha.A = (byte)(alpha.A * num6);
							}
							else if (ptr2->Type == (int)Item.ID.SOUL_OF_LIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_NIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_FRIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_MIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_SIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_FLIGHT || ptr2->Type == (int)Item.ID.SOUL_OF_BLIGHT)
							{
								num6 = UI.PulseScale;
								alpha.R = (byte)(alpha.R * num6);
								alpha.G = (byte)(alpha.G * num6);
								alpha.B = (byte)(alpha.B * num6);
								alpha.A = (byte)(alpha.A * num6);
							}
							pos.X = rectangle.X - ScreenPosition.X;
							pos.Y = rectangle.Y - ScreenPosition.Y;
							SpriteSheet<_sheetSprites>.Draw(num4, ref pos, alpha, rot, num6);
							if (ptr2->Colour.PackedValue != 0)
							{
								SpriteSheet<_sheetSprites>.Draw(num4, ref pos, ptr2->GetColor(colorUnsafe), rot, num6);
							}
						}
					}
					ptr2++;
				}
			}
		}

		private unsafe void DrawBlack()
		{
			float num = (WorldTime.TileColorFore.X + WorldTime.TileColorFore.Y + WorldTime.TileColorFore.Z) * (142f / (339f * (float)Math.PI));
			Rectangle dest = default(Rectangle);
			dest.X = 32 + (FirstTileX << 4) - ScreenPosition.X;
			dest.Width = 16 * (16 / _sheetTiles.Source[(int)_sheetTiles.ID.BLACK_TILE].Width);
			Color black = Color.Black;
			int num2 = LastTileY - 1;
			if (num2 > Main.WorldSurface)
			{
				num2 = Main.WorldSurface;
			}
			for (int i = FirstTileX; i < LastTileX; i++)
			{
				int num3 = FirstTileY;
				fixed (Tile* ptr = &Main.TileSet[i, num3])
				{
					Tile* ptr2 = ptr;
					while (true)
					{
						float num4 = Lighting.BrightnessUnsafe(i, num3);
#if VERSION_101
						if (num4 < num && (num4 == 0f || ptr2->Liquid == 0 || (ptr2->WallType < 32 && ptr2->IsActive != 0 && Main.TileSolidNotSolidTop[ptr2->Type])))
#else
						if (num4 < num && (num4 == 0f || ptr2->Liquid == 0 || (ptr2->IsActive != 0 && Main.TileSolidNotSolidTop[ptr2->Type])))
#endif
						{
							if (dest.Height == 0)
							{
								dest.Y = 32 + (num3 << 4) - ScreenPosition.Y;
							}
							dest.Height += 16;
						}
						else if (dest.Height > 0)
						{
							SpriteSheet<_sheetTiles>.DrawStretchedY((int)_sheetTiles.ID.BLACK_TILE, ref dest, black);
							dest.Height = 0;
						}
						if (++num3 > num2)
						{
							break;
						}
						ptr2++;
					}
					if (dest.Height > 0)
					{
						SpriteSheet<_sheetTiles>.DrawStretchedY((int)_sheetTiles.ID.BLACK_TILE, ref dest, black);
						dest.Height = 0;
					}
				}
				dest.X += 16;
			}
		}

		private unsafe void DrawWalls()
		{
			int gfx = (int)(255f * (1f - gfxQuality) + 100f * gfxQuality);
			int gfx2 = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
			Vector2 pos = default(Vector2);
			Color color = default(Color);
			Rectangle s = default(Rectangle);
			int num = FirstTileX;
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num2 = FirstTileY;
					Tile* ptr2 = ptr + (num * (Main.LargeWorldH) + num2);
					do
					{
						int wall = ptr2->WallType;
						ptr2->CurrentFlags |= Flags.VISITED;
						if (wall > 0 && !ptr2->isFullTile())
						{
							color = Lighting.GetColorUnsafe(num, num2);
							int id = (int)_sheetTiles.ID.WALL_1 - 1 + wall;
							s.X = ptr2->wallFrameX << 1;
							s.Y = ptr2->wallFrameY << 1;
							s.Width = 32;
							s.Height = 32;
							pos.X = num * 16 - ScreenPosition.X - 8 + 32;
							pos.Y = num2 * 16 - ScreenPosition.Y - 8 + 32;
							if (SMOOTH_LIGHT && wall != 21 && !WorldGen.SolidTile(num, num2))
							{
								if (color.R > gfx || color.G > gfx * 1.1 || color.B > gfx * 1.2)
								{
									s.Width = 12;
									s.Height = 12;
									Color colorUnsafe = Lighting.GetColorUnsafe(num - 1, num2 - 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X += 12f;
									s.X += 12;
									s.Width = 8;
									colorUnsafe = Lighting.GetColorUnsafe(num, num2 - 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X += 8f;
									s.X += 8;
									s.Width = 12;
									colorUnsafe = Lighting.GetColorUnsafe(num + 1, num2 - 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.Y += 12f;
									s.Y += 12;
									s.Height = 8;
									colorUnsafe = Lighting.GetColorUnsafe(num + 1, num2);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X -= 8f;
									s.X -= 8;
									s.Width = 8;
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X -= 12f;
									s.X -= 12;
									s.Width = 12;
									colorUnsafe = Lighting.GetColorUnsafe(num - 1, num2);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.Y += 8f;
									s.Y += 8;
									s.Height = 12;
									colorUnsafe = Lighting.GetColorUnsafe(num - 1, num2 + 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X += 12f;
									s.X += 12;
									s.Width = 8;
									colorUnsafe = Lighting.GetColorUnsafe(num, num2 + 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
									pos.X += 8f;
									s.X += 8;
									s.Width = 12;
									colorUnsafe = Lighting.GetColorUnsafe(num + 1, num2 + 1);
									colorUnsafe.R = (byte)(color.R + colorUnsafe.R >> 1);
									colorUnsafe.G = (byte)(color.G + colorUnsafe.G >> 1);
									colorUnsafe.B = (byte)(color.B + colorUnsafe.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, colorUnsafe);
								}
								else if (color.R > gfx2 || color.G > gfx2 * 1.1 || color.B > gfx2 * 1.2)
								{
									s.Width = 16;
									s.Height = 16;
									Color c = (Lighting.Brighter(num, num2 - 1, num - 1, num2) ? Lighting.GetColorUnsafe(num - 1, num2) : Lighting.GetColorUnsafe(num, num2 - 1));
									c.R = (byte)(color.R + c.R >> 1);
									c.G = (byte)(color.G + c.G >> 1);
									c.B = (byte)(color.B + c.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, c);
									s.X += 16;
									pos.X += 16f;
									c = (Lighting.Brighter(num, num2 - 1, num + 1, num2) ? Lighting.GetColorUnsafe(num + 1, num2) : Lighting.GetColorUnsafe(num, num2 - 1));
									c.R = (byte)(color.R + c.R >> 1);
									c.G = (byte)(color.G + c.G >> 1);
									c.B = (byte)(color.B + c.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, c);
									s.Y += 16;
									pos.Y += 16f;
									c = (Lighting.Brighter(num, num2 + 1, num + 1, num2) ? Lighting.GetColorUnsafe(num + 1, num2) : Lighting.GetColorUnsafe(num, num2 + 1));
									c.R = (byte)(color.R + c.R >> 1);
									c.G = (byte)(color.G + c.G >> 1);
									c.B = (byte)(color.B + c.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, c);
									s.X -= 16;
									pos.X -= 16f;
									c = (Lighting.Brighter(num, num2 + 1, num - 1, num2) ? Lighting.GetColorUnsafe(num - 1, num2) : Lighting.GetColorUnsafe(num, num2 + 1));
									c.R = (byte)(color.R + c.R >> 1);
									c.G = (byte)(color.G + c.G >> 1);
									c.B = (byte)(color.B + c.B >> 1);
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, c);
								}
								else
								{
									SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, color);
								}
							}
							else
							{
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, color);
							}
						}
						ptr2++;
					}
					while (++num2 < LastTileY);
				}
				while (++num < LastTileX);
			}
		}

		private unsafe void DrawWires()
		{
			int gfx = (int)(50f * (1f - gfxQuality) + 2f * gfxQuality);
			Rectangle s = default(Rectangle);
			s.Width = 16;
			s.Height = 16;
			Vector2 pos = default(Vector2);
			fixed (Tile* ptr = Main.TileSet)
			{
				for (int i = FirstTileX; i < LastTileX; i++)
				{
					pos.X = 32 + i * 16 - ScreenPosition.X;
					Tile* ptr2 = ptr + (i * (Main.LargeWorldH) + FirstTileY);
					int num = FirstTileY;
					while (num < LastTileY)
					{
						if (ptr2->wire != 0)
						{
							pos.Y = 32 + num * 16 - ScreenPosition.Y;
							if (Lighting.IsNotBlackUnsafe(i, num))
							{
								ptr2--;
								int wire = ptr2->wire;
								ptr2 += 2;
								int wire2 = ptr2->wire;
								ptr2 -= (Main.LargeWorldH + 1);
								int wire3 = ptr2->wire;
								ptr2 += (Main.LargeWorldH * 2);
								int wire4 = ptr2->wire;
								ptr2 -= (Main.LargeWorldH);
								if (wire != 0)
								{
									if (wire2 != 0)
									{
										if (wire3 != 0)
										{
											if (wire4 != 0)
											{
												s.X = 18;
												s.Y = 18;
											}
											else
											{
												s.X = 54;
												s.Y = 0;
											}
										}
										else if (wire4 != 0)
										{
											s.X = 36;
											s.Y = 0;
										}
										else
										{
											s.X = 0;
											s.Y = 0;
										}
									}
									else if (wire3 != 0)
									{
										if (wire4 != 0)
										{
											s.X = 0;
											s.Y = 18;
										}
										else
										{
											s.X = 54;
											s.Y = 18;
										}
									}
									else if (wire4 != 0)
									{
										s.X = 36;
										s.Y = 18;
									}
									else
									{
										s.X = 36;
										s.Y = 36;
									}
								}
								else if (wire2 != 0)
								{
									if (wire3 != 0)
									{
										if (wire4 != 0)
										{
											s.X = 72;
											s.Y = 0;
										}
										else
										{
											s.X = 72;
											s.Y = 18;
										}
									}
									else if (wire4 != 0)
									{
										s.X = 0;
										s.Y = 36;
									}
									else
									{
										s.X = 18;
										s.Y = 36;
									}
								}
								else if (wire3 != 0)
								{
									if (wire4 != 0)
									{
										s.X = 18;
										s.Y = 0;
									}
									else
									{
										s.X = 54;
										s.Y = 36;
									}
								}
								else if (wire4 != 0)
								{
									s.X = 72;
									s.Y = 36;
								}
								else
								{
									s.X = 0;
									s.Y = 54;
								}
								Color colorUnsafe = Lighting.GetColorUnsafe(i, num);

								int WireIndex = (int)_sheetTiles.ID.WIRES;

								if (SMOOTH_LIGHT && (colorUnsafe.R > gfx || colorUnsafe.G > gfx * 1.1 || colorUnsafe.B > gfx * 1.2))
								{
									for (int j = 0; j < 4; j++)
									{
										int num2 = 0;
										int num3 = 0;
										Color c = colorUnsafe;
										Color color = colorUnsafe;
										switch (j)
										{
										case 0:
											color = ((!Lighting.Brighter(i, num - 1, i - 1, num)) ? Lighting.GetColorUnsafe(i, num - 1) : Lighting.GetColorUnsafe(i - 1, num));
											break;
										case 1:
											color = ((!Lighting.Brighter(i, num - 1, i + 1, num)) ? Lighting.GetColorUnsafe(i, num - 1) : Lighting.GetColorUnsafe(i + 1, num));
											num2 = 8;
											break;
										case 2:
											color = ((!Lighting.Brighter(i, num + 1, i - 1, num)) ? Lighting.GetColorUnsafe(i, num + 1) : Lighting.GetColorUnsafe(i - 1, num));
											num3 = 8;
											break;
										default:
											color = ((!Lighting.Brighter(i, num + 1, i + 1, num)) ? Lighting.GetColorUnsafe(i, num + 1) : Lighting.GetColorUnsafe(i + 1, num));
											num2 = 8;
											num3 = 8;
											break;
										}
										c.R = (byte)(colorUnsafe.R + color.R >> 1);
										c.G = (byte)(colorUnsafe.G + color.G >> 1);
										c.B = (byte)(colorUnsafe.B + color.B >> 1);
										Rectangle s2 = s;
										s2.X += num2;
										s2.Y += num3;
										s2.Width = 8;
										s2.Height = 8;
										pos.X += num2;
										pos.Y += num3;
										SpriteSheet<_sheetTiles>.Draw(WireIndex, ref pos, ref s2, c);
										pos.X -= num2;
										pos.Y -= num3;
									}
								}
								else
								{
									SpriteSheet<_sheetTiles>.Draw(WireIndex, ref pos, ref s, colorUnsafe);
								}
							}
						}
						num++;
						ptr2++;
					}
				}
			}
		}

		public unsafe void DrawBg(UI ui)
		{
            if (viewportAnimTheta > 0.0 && !Guide.IsVisible)
			{
				viewportAnimTheta -= Math.PI / 60.0;
				if (viewportAnimTheta <= 0.0)
				{
					SafeAreaOffsetLeft = targetSAFE_AREA_OFFSET_L;
					SafeAreaOffsetTop = targetSAFE_AREA_OFFSET_T;
					SafeAreaOffsetRight = targetSAFE_AREA_OFFSET_R;
					SafeAreaOffsetBottom = targetSAFE_AREA_OFFSET_B;
					ActiveViewport = targetViewport;
				}
				else
				{
					double num = 1.0 - Math.Sin(viewportAnimTheta);
					SafeAreaOffsetLeft = currentSAFE_AREA_OFFSET_L + (int)(num * (targetSAFE_AREA_OFFSET_L - currentSAFE_AREA_OFFSET_L));
					SafeAreaOffsetTop = currentSAFE_AREA_OFFSET_T + (int)(num * (targetSAFE_AREA_OFFSET_T - currentSAFE_AREA_OFFSET_T));
					SafeAreaOffsetRight = currentSAFE_AREA_OFFSET_R + (int)(num * (targetSAFE_AREA_OFFSET_R - currentSAFE_AREA_OFFSET_R));
					SafeAreaOffsetBottom = currentSAFE_AREA_OFFSET_B + (int)(num * (targetSAFE_AREA_OFFSET_B - currentSAFE_AREA_OFFSET_B));
					ActiveViewport.X = currentViewport.X + (int)(num * (targetViewport.X - currentViewport.X));
					ActiveViewport.Y = currentViewport.Y + (int)(num * (targetViewport.Y - currentViewport.Y));
				}
			}
			GraphicsDevice.Viewport = ActiveViewport;
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, ScreenProjection);
			int BackgroundTop;
			if (ui.CurMenuType == MenuType.MAIN)
			{
				BackgroundTop = -200;
				AtmoOpacity = 1f;
				WorldTime = Main.MenuTime;
				if (!WorldTime.DayTime)
				{
					WorldTime.intermediateBgColor.R = 35;
					WorldTime.intermediateBgColor.G = 35;
					WorldTime.intermediateBgColor.B = 35;
				}
				bgDelay = 1000;
				if (bgAlpha[1] > 0f)
				{
					WorldTime.applyEvil(bgAlpha[1]);
				}
				else
				{
					WorldTime.applyNothing();
				}
				WorldTime.finalizeColors();
				ScreenLastPosition = ScreenPosition;
				ScreenPosition.X += 2;
				ScreenPosition.Y = Main.WorldSurfacePixels - Main.ResolutionHeight - 1;
			}
			else
			{
				BackgroundTop = (int)(ScreenPosition.Y / (float)(Main.WorldSurfacePixels - 540) * -260f);
				WorldTime = Main.GameTime;
				if (JungleTiles > 0)
				{
					WorldTime.applyJungle(JungleTiles * 0.005f);
				}
				else if (EvilTiles > 0)
				{
					WorldTime.applyEvil(EvilTiles * 0.002f);
				}
				else
				{
					WorldTime.applyNothing();
				}
				WorldTime.finalizeColors();
				float num3 = Main.MaxTilesY / 1200f;
				num3 *= num3;
				AtmoOpacity = ((ScreenPosition.Y + (Main.ResolutionHeight / 2) >> 4) - (65f + 10f * num3)) / (Main.WorldSurface * 0.2f);
				if (AtmoOpacity >= 1f)
				{
					AtmoOpacity = 1f;
				}
				else
				{
					if (AtmoOpacity < 0f)
					{
						AtmoOpacity = 0f;
					}
					WorldTime.bgColor.R = (byte)(WorldTime.bgColor.R * AtmoOpacity);
					WorldTime.bgColor.G = (byte)(WorldTime.bgColor.G * AtmoOpacity);
					WorldTime.bgColor.B = (byte)(WorldTime.bgColor.B * AtmoOpacity);
				}
			}
			Vector2 pos = default(Vector2);
			if (ScreenPosition.Y >= Main.WorldSurfacePixels)
			{
				return;
			}
			Rectangle dest = default(Rectangle);
#if USE_ORIGINAL_CODE
			dest.X = -1; // Why is this set to -1?
#else
			dest.X = 0; // This makes it so the right-most side of the screen won't have a 1px vertical black line instead of BACKGROUND_0.
#endif
			dest.Y = BackgroundTop;
			dest.Width = ViewWidth;
			dest.Height = 1300 - BackgroundTop;
			SpriteSheet<_sheetTiles>.DrawStretchedX((int)_sheetTiles.ID.BACKGROUND_0, ref dest, WorldTime.bgColor);
			if (255 - WorldTime.bgColor.R - 100 > 0)
			{
				float num4 = EvilTiles * 0.002f;
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				num4 = 1f - num4 * 0.5f;
				if (EvilTiles <= 0)
				{
					num4 = 1f;
				}
				Color c = default(Color);
				Vector2 pos2 = default(Vector2);
				for (int i = 0; i < Star.MaxNumStars; i++)
				{
					fixed (Star* ptr = &Star.star[i])
					{
						float num5 = ptr->twinkle * num4;
						int num6 = (int)((255 - WorldTime.bgColor.R - 100) * num5);
						int num7 = (int)((255 - WorldTime.bgColor.G - 100) * num5);
						int num8 = (int)((255 - WorldTime.bgColor.B - 100) * num5);
						if (num6 < 0)
						{
							num6 = 0;
						}
						if (num7 < 0)
						{
							num7 = 0;
						}
						if (num8 < 0)
						{
							num8 = 0;
						}
						c.R = (byte)num6;
						c.G = (byte)(num7 * num4);
						c.B = (byte)(num8 * num4);
						pos2.X = ptr->position.X;
						if (ViewWidth > ViewportWidth)
						{
							pos2.X = (pos2.X - ViewportWidth) * 2f + (ViewportWidth*2);
						}
						pos2.Y = ptr->position.Y + BackgroundTop;
						SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.STAR_0 + ptr->type, ref pos2, c, ptr->rotation, ptr->scale * ptr->twinkle);
					}
				}
			}
			int num9 = WorldTime.celestialX;
			if (ViewWidth > ViewportWidth)
			{
				num9 = (num9 - ViewportWidth << 1) + (ViewportWidth * 2);
			}
			if (WorldTime.DayTime)
			{
				int id = ((ui.CurMenuType == MenuType.MAIN || ui.ActivePlayer.head != 12) ? (int)_sheetTiles.ID.SUN : (int)_sheetTiles.ID.SUN2);
				SpriteSheet<_sheetTiles>.Draw(id, num9, WorldTime.celestialY + BackgroundTop, WorldTime.celestialColor, WorldTime.celestialRotation, WorldTime.celestialScale);
			}
			else
			{
				SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.MOON, num9, WorldTime.celestialY + BackgroundTop, 50 * WorldTime.MoonPhase, 50, WorldTime.celestialColor, WorldTime.celestialRotation, WorldTime.celestialScale);
			}
			BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1200f) + 1190;
			float num10 = BackgroundTop - 50;
			for (int j = 0; j < Cloud.MaxNumClouds; j++)
			{
				if (!Cloud.ActiveCloud[j].IsActive || !(Cloud.ActiveCloud[j].CloudScale < 1f))
				{
					continue;
				}
				pos.Y = num10 + Cloud.ActiveCloud[j].CloudPos.Y;
				if (pos.Y < ViewportHeight && pos.Y > -Cloud.ActiveCloud[j].CloudHeight)
				{
					pos.X = Cloud.ActiveCloud[j].CloudPos.X;
					if (ViewWidth > ViewportWidth)
					{
						pos.X = (pos.X - ViewportWidth) * 2f + (ViewportWidth * 2);
					}
					Color c2 = Cloud.ActiveCloud[j].CloudColor(WorldTime.bgColor);
					if (AtmoOpacity < 1f)
					{
						c2.R = (byte)(c2.R * AtmoOpacity);
						c2.G = (byte)(c2.G * AtmoOpacity);
						c2.B = (byte)(c2.B * AtmoOpacity);
						c2.A = (byte)(c2.A * AtmoOpacity);
					}
					int id2 = (int)_sheetTiles.ID.CLOUD_0 + Cloud.ActiveCloud[j].CloudType;
					SpriteSheet<_sheetTiles>.DrawScaledTL(id2, ref pos, c2, Cloud.ActiveCloud[j].CloudScale);
				}
			}
			int num11 = (int)(backgroundTexture[7].Width * 2f);
			BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1300f) + 1090;
			int BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.15f, num11) - (num11 >> 1));
			int BackgroundLoops = ViewWidth / num11 + 2;
			if (ui.CurMenuType == MenuType.MAIN)
			{
				BackgroundTop = 100;
			}
			Color bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[0]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[0]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[0]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[0]);
			if (bgAlpha2[0] > 0f)
			{
				for (int k = 0; k < BackgroundLoops; k++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[7], new Vector2(BackgroundStart + num11 * k, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2f, SpriteEffects.None, 0f);
				}
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[1]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[1]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[1]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[1]);
			if (bgAlpha2[1] > 0f)
			{
				for (int l = 0; l < BackgroundLoops; l++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[23], new Vector2(BackgroundStart + num11 * l, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2f, SpriteEffects.None, 0f);
				}
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[2]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[2]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[2]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[2]);
			if (bgAlpha2[2] > 0f)
			{
				for (int m = 0; m < BackgroundLoops; m++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[24], new Vector2(BackgroundStart + num11 * m, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2f, SpriteEffects.None, 0f);
				}
			}
			num10 = BackgroundTop - 50;
			for (int n = 0; n < Cloud.MaxNumClouds; n++)
			{
				if (!Cloud.ActiveCloud[n].IsActive || !(Cloud.ActiveCloud[n].CloudScale < 1.15) || !(Cloud.ActiveCloud[n].CloudScale >= 1f))
				{
					continue;
				}
				pos.Y = num10 + Cloud.ActiveCloud[n].CloudPos.Y;
				if (pos.Y < ViewportHeight && pos.Y > -Cloud.ActiveCloud[n].CloudHeight)
				{
					pos.X = Cloud.ActiveCloud[n].CloudPos.X;
					if (ViewWidth > ViewportWidth)
					{
						pos.X = (pos.X - ViewportWidth) * 2f + (ViewportWidth * 2);
					}
					Color c3 = Cloud.ActiveCloud[n].CloudColor(WorldTime.bgColor);
					if (AtmoOpacity < 1f)
					{
						c3.R = (byte)(c3.R * AtmoOpacity);
						c3.G = (byte)(c3.G * AtmoOpacity);
						c3.B = (byte)(c3.B * AtmoOpacity);
						c3.A = (byte)(c3.A * AtmoOpacity);
					}
					int id3 = (int)_sheetTiles.ID.CLOUD_0 + Cloud.ActiveCloud[n].CloudType;
					SpriteSheet<_sheetTiles>.DrawScaledTL(id3, ref pos, c3, Cloud.ActiveCloud[n].CloudScale);
				}
			}
			if (HolyTiles > 0)
			{
				BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.17f, 8085.0) - 4042.0);
				BackgroundLoops = ViewWidth / 8085 + 2;
				BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1400f) + 900;
				if (ui.CurMenuType == MenuType.MAIN)
				{
					BackgroundTop = 230;
					BackgroundStart -= 500;
				}
				Color bgColor2 = WorldTime.bgColor;
				float num14 = HolyTiles * 0.0025f;
				if (num14 > 0.5f)
				{
					num14 = 0.5f;
				}
				bgColor2.R = (byte)(bgColor2.R * num14);
				bgColor2.G = (byte)(bgColor2.G * num14);
				bgColor2.B = (byte)(bgColor2.B * num14);
				bgColor2.A = (byte)(bgColor2.A * num14 * 0.8f);
				for (int num15 = 0; num15 < BackgroundLoops; num15++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[18], new Vector2(BackgroundStart + 8085 * num15, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[18].Width, backgroundTexture[18].Height), bgColor2, 0f, default(Vector2), 2.2f, SpriteEffects.None, 0f);
				}
				for (int num16 = 0; num16 < BackgroundLoops; num16++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[19], new Vector2(BackgroundStart + 8085 * num16 + 1700, BackgroundTop + 100), (Rectangle?)new Rectangle(0, 0, backgroundTexture[19].Width, backgroundTexture[19].Height), bgColor2, 0f, default(Vector2), 1.98f, SpriteEffects.None, 0f);
				}
			}
			int num17 = (int)(backgroundTexture[7].Width * 2.3f);
			BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.2f, num17) - (num17 >> 1));
			BackgroundLoops = ViewWidth / num17 + 2;
			BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1400f) + 1260;
			if (ui.CurMenuType == MenuType.MAIN)
			{
				BackgroundTop = 230;
				BackgroundStart -= 500;
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[0]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[0]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[0]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[0]);
			if (bgAlpha2[0] > 0f)
			{
				for (int num18 = 0; num18 < BackgroundLoops; num18++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[8], new Vector2(BackgroundStart + num17 * num18, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2.3f, SpriteEffects.None, 0f);
				}
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[1]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[1]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[1]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[1]);
			if (bgAlpha2[1] > 0f)
			{
				for (int num19 = 0; num19 < BackgroundLoops; num19++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[22], new Vector2(BackgroundStart + num17 * num19, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2.3f, SpriteEffects.None, 0f);
				}
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[2]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[2]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[2]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[2]);
			if (bgAlpha2[2] > 0f)
			{
				for (int num20 = 0; num20 < BackgroundLoops; num20++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[25], new Vector2(BackgroundStart + num17 * num20, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2.3f, SpriteEffects.None, 0f);
				}
			}
			bgColor = WorldTime.bgColor;
			bgColor.R = (byte)(bgColor.R * bgAlpha2[3]);
			bgColor.G = (byte)(bgColor.G * bgAlpha2[3]);
			bgColor.B = (byte)(bgColor.B * bgAlpha2[3]);
			bgColor.A = (byte)(bgColor.A * bgAlpha2[3]);
			if (bgAlpha2[3] > 0f)
			{
				for (int num21 = 0; num21 < BackgroundLoops; num21++)
				{
					Main.SpriteBatch.Draw(backgroundTexture[28], new Vector2(BackgroundStart + num17 * num21, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[7].Width, backgroundTexture[7].Height), bgColor, 0f, default(Vector2), 2.3f, SpriteEffects.None, 0f);
				}
			}
			num10 = BackgroundTop * 1.01f - 150f;
			for (int num22 = 0; num22 < Cloud.MaxNumClouds; num22++)
			{
				if (!Cloud.ActiveCloud[num22].IsActive || !(Cloud.ActiveCloud[num22].CloudScale > 2.3f))
				{
					continue;
				}
				pos.Y = num10 + Cloud.ActiveCloud[num22].CloudPos.Y;
				if (pos.Y < ViewportHeight && pos.Y > -Cloud.ActiveCloud[num22].CloudHeight)
				{
					pos.X = Cloud.ActiveCloud[num22].CloudPos.X;
					if (ViewWidth > ViewportWidth)
					{
						pos.X = (pos.X - ViewportWidth) * 2f + (ViewportWidth * 2);
					}
					Color c4 = Cloud.ActiveCloud[num22].CloudColor(WorldTime.bgColor);
					if (AtmoOpacity < 1f)
					{
						c4.R = (byte)(c4.R * AtmoOpacity);
						c4.G = (byte)(c4.G * AtmoOpacity);
						c4.B = (byte)(c4.B * AtmoOpacity);
						c4.A = (byte)(c4.A * AtmoOpacity);
					}
					int id4 = (int)_sheetTiles.ID.CLOUD_0 + Cloud.ActiveCloud[num22].CloudType;
					SpriteSheet<_sheetTiles>.DrawScaledTL(id4, ref pos, c4, Cloud.ActiveCloud[num22].CloudScale);
				}
			}
			int num23 = bgStyle;
			if (ui.CurMenuType != 0)
			{
				int num24 = ScreenPosition.X + (ViewWidth >> 1) >> 4;
				if (num24 < 380 || num24 > Main.MaxTilesX - 380)
				{
					num23 = 4;
				}
#if USE_ORIGINAL_CODE
				else if (InactiveTiles < ((ViewWidth == 960) ? 8000 : 16000))
#else
				else if (InactiveTiles < ((ViewWidth == Main.ResolutionWidth) ? (int)(8000 * Main.ScreenMultiplier) : (int)(16000 * Main.ScreenMultiplier)))
#endif
				{
					num23 = ((SandTiles > 1000) ? ((!Player.ZoneEvil && !Player.zoneHoly) ? 2 : 5) : (Player.zoneHoly ? 6 : (Player.ZoneEvil ? 1 : (Player.ZoneJungle ? 3 : 0))));
				}
			}
			float num25 = 0.05f;
			int num26 = 30;
			if (num23 == 0)
			{
				num26 = 120;
			}
			if (bgDelay < 0)
			{
				bgDelay++;
			}
			else
			{
				if (ui.CurMenuType == MenuType.MAIN)
				{
					num25 = 0.02f;
					if (!WorldTime.DayTime)
					{
						if (ui.CurMenuMode == MenuMode.CREDITS)
						{
							HolyTiles = 200;
							bgStyle = 6;
						}
						else
						{
							HolyTiles = 0;
							bgStyle = 1;
						}
					}
					else if (ui.CurMenuMode == MenuMode.CREDITS)
					{
						HolyTiles = 200;
						bgStyle = 3;
					}
					else
					{
						HolyTiles = 0;
						bgStyle = 0;
					}
					num23 = bgStyle;
				}
				if (num23 != bgStyle)
				{
					bgDelay++;
					if (bgDelay > num26)
					{
						bgDelay = -60;
						bgStyle = num23;
						if (num23 == 0)
						{
							bgDelay = 0;
						}
					}
				}
				else if (bgDelay > 0)
				{
					bgDelay--;
				}
			}
			if (quickBG > 0)
			{
				quickBG--;
				bgStyle = num23;
				num25 = 1f;
			}
			if (bgStyle == 2)
			{
				bgAlpha2[0] -= num25;
				if (bgAlpha2[0] < 0f)
				{
					bgAlpha2[0] = 0f;
				}
				bgAlpha2[1] += num25;
				if (bgAlpha2[1] > 1f)
				{
					bgAlpha2[1] = 1f;
				}
				bgAlpha2[2] -= num25;
				if (bgAlpha2[2] < 0f)
				{
					bgAlpha2[2] = 0f;
				}
				bgAlpha2[3] -= num25;
				if (bgAlpha2[3] < 0f)
				{
					bgAlpha2[3] = 0f;
				}
			}
			else if (bgStyle == 5 || bgStyle == 1 || bgStyle == 6)
			{
				bgAlpha2[0] -= num25;
				if (bgAlpha2[0] < 0f)
				{
					bgAlpha2[0] = 0f;
				}
				bgAlpha2[1] -= num25;
				if (bgAlpha2[1] < 0f)
				{
					bgAlpha2[1] = 0f;
				}
				bgAlpha2[2] += num25;
				if (bgAlpha2[2] > 1f)
				{
					bgAlpha2[2] = 1f;
				}
				bgAlpha2[3] -= num25;
				if (bgAlpha2[3] < 0f)
				{
					bgAlpha2[3] = 0f;
				}
			}
			else if (bgStyle == 4)
			{
				bgAlpha2[0] -= num25;
				if (bgAlpha2[0] < 0f)
				{
					bgAlpha2[0] = 0f;
				}
				bgAlpha2[1] -= num25;
				if (bgAlpha2[1] < 0f)
				{
					bgAlpha2[1] = 0f;
				}
				bgAlpha2[2] -= num25;
				if (bgAlpha2[2] < 0f)
				{
					bgAlpha2[2] = 0f;
				}
				bgAlpha2[3] += num25;
				if (bgAlpha2[3] > 1f)
				{
					bgAlpha2[3] = 1f;
				}
			}
			else
			{
				bgAlpha2[0] += num25;
				if (bgAlpha2[0] > 1f)
				{
					bgAlpha2[0] = 1f;
				}
				bgAlpha2[1] -= num25;
				if (bgAlpha2[1] < 0f)
				{
					bgAlpha2[1] = 0f;
				}
				bgAlpha2[2] -= num25;
				if (bgAlpha2[2] < 0f)
				{
					bgAlpha2[2] = 0f;
				}
				bgAlpha2[3] -= num25;
				if (bgAlpha2[3] < 0f)
				{
					bgAlpha2[3] = 0f;
				}
			}
			for (int num27 = 0; num27 < 7; num27++)
			{
				if (bgStyle == num27)
				{
					bgAlpha[num27] += num25;
					if (bgAlpha[num27] > 1f)
					{
						bgAlpha[num27] = 1f;
					}
				}
				else
				{
					bgAlpha[num27] -= num25;
					if (bgAlpha[num27] < 0f)
					{
						bgAlpha[num27] = 0f;
					}
				}
				bgColor = WorldTime.bgColor;
				bgColor.R = (byte)(bgColor.R * bgAlpha[num27]);
				bgColor.G = (byte)(bgColor.G * bgAlpha[num27]);
				bgColor.B = (byte)(bgColor.B * bgAlpha[num27]);
				bgColor.A = (byte)(bgColor.A * bgAlpha[num27]);
				if (num27 == 3 && bgAlpha[num27] > 0f)
				{
					int num28 = (int)(backgroundTexture[8].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.4f, num28) - (num28 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1660;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
					}
					BackgroundLoops = ViewWidth / num28 + 2;
					for (int num29 = 0; num29 < BackgroundLoops; num29++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[15], new Vector2(BackgroundStart + num28 * num29, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[8].Width, backgroundTexture[8].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num30 = (int)(backgroundTexture[8].Width * 2.62f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.43f, num30) - (num30 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1950f) + 1840;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 400;
						BackgroundStart -= 80;
					}
					BackgroundLoops = ViewWidth / num30 + 2;
					for (int num31 = 0; num31 < BackgroundLoops; num31++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[16], new Vector2(BackgroundStart + num30 * num31, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[8].Width, backgroundTexture[8].Height), bgColor, 0f, default(Vector2), 2.62f, SpriteEffects.None, 0f);
					}
					int num32 = (int)(backgroundTexture[8].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num32) - (num32 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2060;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
					}
					BackgroundLoops = ViewWidth / num32 + 2;
					for (int num33 = 0; num33 < BackgroundLoops; num33++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[17], new Vector2(BackgroundStart + num32 * num33, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[8].Width, backgroundTexture[8].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
				else if (num27 == 2 && bgAlpha[num27] > 0f)
				{
					int num34 = (int)(backgroundTexture[21].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.37f, num34) - (num34 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1750;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
					}
					BackgroundLoops = ViewWidth / num34 + 2;
					for (int num35 = 0; num35 < BackgroundLoops; num35++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[21], new Vector2(BackgroundStart + num34 * num35, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[21].Width, backgroundTexture[21].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num36 = (int)(backgroundTexture[20].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num36) - (num36 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2150;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
					}
					BackgroundLoops = ViewWidth / num36 + 2;
					for (int num37 = 0; num37 < BackgroundLoops; num37++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[20], new Vector2(BackgroundStart + num36 * num37, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[20].Width, backgroundTexture[20].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
				else if (num27 == 5 && bgAlpha[num27] > 0f)
				{
					int num38 = (int)(backgroundTexture[8].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.37f, num38) - (num38 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1750;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
					}
					BackgroundLoops = ViewWidth / num38 + 2;
					for (int num39 = 0; num39 < BackgroundLoops; num39++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[26], new Vector2(BackgroundStart + num38 * num39, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[26].Width, backgroundTexture[26].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num40 = (int)(backgroundTexture[8].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num40) - (num40 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2150;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
					}
					BackgroundLoops = ViewWidth / num40 + 2;
					for (int num41 = 0; num41 < BackgroundLoops; num41++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[27], new Vector2(BackgroundStart + num40 * num41, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[27].Width, backgroundTexture[27].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
				else if (num27 == 1 && bgAlpha[num27] > 0f)
				{
					int num42 = (int)(backgroundTexture[8].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.4f, num42) - (num42 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1500;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
					}
					BackgroundLoops = ViewWidth / num42 + 2;
					for (int num43 = 0; num43 < BackgroundLoops; num43++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[12], new Vector2(BackgroundStart + num42 * num43, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[12].Width, backgroundTexture[12].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num44 = (int)(backgroundTexture[8].Width * 2.62f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.43f, num44) - (num44 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1950f) + 1750;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 400;
						BackgroundStart -= 80;
					}
					BackgroundLoops = ViewWidth / num44 + 2;
					for (int num45 = 0; num45 < BackgroundLoops; num45++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[13], new Vector2(BackgroundStart + num44 * num45, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[13].Width, backgroundTexture[13].Height), bgColor, 0f, default(Vector2), 2.62f, SpriteEffects.None, 0f);
					}
					int num46 = (int)(backgroundTexture[8].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num46) - (num46 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2000;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
					}
					BackgroundLoops = ViewWidth / num46 + 2;
					for (int num47 = 0; num47 < BackgroundLoops; num47++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[14], new Vector2(BackgroundStart + num46 * num47, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[14].Width, backgroundTexture[14].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
				else if (num27 == 6 && bgAlpha[num27] > 0f)
				{
					int num48 = (int)(backgroundTexture[8].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.4f, num48) - (num48 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1500;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num48 + 2;
					for (int num49 = 0; num49 < BackgroundLoops; num49++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[29], new Vector2(BackgroundStart + num48 * num49, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[29].Width, backgroundTexture[29].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num50 = (int)(backgroundTexture[8].Width * 2.62f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.43f, num50) - (num50 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1950f) + 1750;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 400;
						BackgroundStart -= 80;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num50 + 2;
					for (int num51 = 0; num51 < BackgroundLoops; num51++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[30], new Vector2(BackgroundStart + num50 * num51, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[30].Width, backgroundTexture[30].Height), bgColor, 0f, default(Vector2), 2.62f, SpriteEffects.None, 0f);
					}
					int num52 = (int)(backgroundTexture[8].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num52) - (num52 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2000;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num52 + 2;
					for (int num53 = 0; num53 < BackgroundLoops; num53++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[31], new Vector2(BackgroundStart + num52 * num53, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[31].Width, backgroundTexture[31].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
				else if (num27 == 0 && bgAlpha[num27] > 0f)
				{
					int num54 = (int)(backgroundTexture[8].Width * 2.5f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.4f, num54) - (num54 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1800f) + 1500;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 320;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num54 + 2;
					for (int num55 = 0; num55 < BackgroundLoops; num55++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[9], new Vector2(BackgroundStart + num54 * num55, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[9].Width, backgroundTexture[9].Height), bgColor, 0f, default(Vector2), 2.5f, SpriteEffects.None, 0f);
					}
					int num56 = (int)(backgroundTexture[8].Width * 2.62f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.43f, num56) - (num56 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 1950f) + 1750;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 400;
						BackgroundStart -= 80;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num56 + 2;
					for (int num57 = 0; num57 < BackgroundLoops; num57++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[10], new Vector2(BackgroundStart + num56 * num57, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[8].Width, backgroundTexture[8].Height), bgColor, 0f, default(Vector2), 2.62f, SpriteEffects.None, 0f);
					}
					int num58 = (int)(backgroundTexture[8].Width * 2.68f);
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * 0.49f, num58) - (num58 >> 1));
					BackgroundTop = (int)(-ScreenPosition.Y / (float)Main.WorldSurfacePixels * 2100f) + 2000;
					if (ui.CurMenuType == MenuType.MAIN)
					{
						BackgroundTop = 480;
						BackgroundStart -= 120;
#if VERSION_101
						BackgroundTop -= 50;
#endif
					}
					BackgroundLoops = ViewWidth / num58 + 2;
					for (int num59 = 0; num59 < BackgroundLoops; num59++)
					{
						Main.SpriteBatch.Draw(backgroundTexture[11], new Vector2(BackgroundStart + num58 * num59, BackgroundTop), (Rectangle?)new Rectangle(0, 0, backgroundTexture[8].Width, backgroundTexture[8].Height), bgColor, 0f, default(Vector2), 2.68f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		public void DrawWorld()
		{
			Color white = Color.White;
			Rectangle destinationRectangle = default(Rectangle);
			destinationRectangle.Width = ViewWidth + (OFFSCREEN_RANGE_X * 2);
			destinationRectangle.Height = ViewportHeight + OFFSCREEN_RANGE_VERTICAL;
			destinationRectangle.X = sceneWaterPos.X - ScreenPosition.X;
			destinationRectangle.Y = sceneWaterPos.Y - ScreenPosition.Y;
			Main.SpriteBatch.Draw(backWaterTarget, destinationRectangle, white);
			destinationRectangle.X = (int)((sceneBackgroundPos.X - ScreenPosition.X + OFFSCREEN_RANGE_X) * CAVE_PARALLAX) - OFFSCREEN_RANGE_X;
			destinationRectangle.Y = sceneBackgroundPos.Y - ScreenPosition.Y;
			Main.SpriteBatch.Draw(backgroundTarget, destinationRectangle, white);
			if (FirstTileY <= Main.WorldSurface)
			{
				destinationRectangle.X = sceneBlackPos.X - ScreenPosition.X;
				destinationRectangle.Y = sceneBlackPos.Y - ScreenPosition.Y;
				Main.SpriteBatch.Draw(blackTarget, destinationRectangle, white);
			}
			destinationRectangle.X = sceneWallPos.X - ScreenPosition.X;
			destinationRectangle.Y = sceneWallPos.Y - ScreenPosition.Y;
			Main.SpriteBatch.Draw(wallTarget, destinationRectangle, white);
			DrawWoF();
			destinationRectangle.X = sceneTile2Pos.X - ScreenPosition.X;
			destinationRectangle.Y = sceneTile2Pos.Y - ScreenPosition.Y;
			Main.SpriteBatch.Draw(tileNonSolidTarget, destinationRectangle, white);
			destinationRectangle.X = sceneTilePos.X - ScreenPosition.X;
			destinationRectangle.Y = sceneTilePos.Y - ScreenPosition.Y;
			if (Player.detectCreature)
			{
				Main.SpriteBatch.Draw(tileSolidTarget, destinationRectangle, white);
				DrawGore();
				DrawNPCs(behindTiles: true);
				DrawNPCs();
			}
			else
			{
				DrawNPCs(behindTiles: true);
				Main.SpriteBatch.Draw(tileSolidTarget, destinationRectangle, white);
				DrawGore();
				DrawNPCs();
			}
			DrawProjectiles();
			DrawPlayers();
			DrawItems();
			dustLocal.DrawDust(this);
			Main.DustSet.DrawDust(this);
			destinationRectangle.X = sceneWaterPos.X - ScreenPosition.X;
			destinationRectangle.Y = sceneWaterPos.Y - ScreenPosition.Y;
			Main.SpriteBatch.Draw(waterTarget, destinationRectangle, white);
			DrawCombatText();
			DrawItemText();
		}

		private unsafe void DrawBackground()
		{
			float num = 0.9f;
			float num2 = num;
			float num3 = num;
			float num4 = 0f;
			if (HolyTiles > EvilTiles)
			{
				num4 = HolyTiles * 0.00125f;
			}
			else if (EvilTiles > HolyTiles)
			{
				num4 = EvilTiles * 0.00125f;
			}
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			float num5 = (ScreenPosition.Y - (Main.WorldSurface << 4)) / 300f;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			else if (num5 > 1f)
			{
				num5 = 1f;
			}
			float num6 = 1f - num5 + num * num5;
			Lighting.BrightnessLevel = Lighting.DefBrightness * (1f - num5) + 1f * num5;
			float num7 = (ScreenPosition.Y - (Main.ResolutionHeight / 2) + 200 - (Main.RockLayer << 4)) / 300f;
			if (num7 < 0f)
			{
				num7 = 0f;
			}
			else if (num7 > 1f)
			{
				num7 = 1f;
			}
			if (EvilTiles > 0)
			{
				num = 0.8f * num4 + num * (1f - num4);
				num2 = 0.75f * num4 + num2 * (1f - num4);
				num3 = 1.1f * num4 + num3 * (1f - num4);
			}
			else if (HolyTiles > 0)
			{
				num = 1f * num4 + num * (1f - num4);
				num2 = 0.7f * num4 + num2 * (1f - num4);
				num3 = 0.9f * num4 + num3 * (1f - num4);
			}
			num = num6 - num7 + num * num7;
			num2 = num6 - num7 + num2 * num7;
			num3 = num6 - num7 + num3 * num7;
			Lighting.DefBrightness = Lighting.MaxBrightness * (1f - num7) + num7;
			int BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0);
			int BackgroundLoops = ViewWidth / 96 + 2;
			int BackgroundTop = Main.WorldSurfacePixels - ScreenPosition.Y;
			int num11 = BackgroundStart;
			int num12 = -((BackgroundStart + ScreenPosition.X + 8) & 0xF);
			if (num12 == -8)
			{
				num12 = 8;
			}
			Vector2 pos = new Vector2(num11 + num12 + 32, BackgroundTop + 32);
			Rectangle s = default(Rectangle);
			s.X = num12;
			s.Width = 16;
			s.Height = 16;
			for (int i = 0; i < BackgroundLoops; i++)
			{
				int num13 = 15;
				while (num13 >= 0)
				{
					Color color = Lighting.GetColor(num11 + 8 + ScreenPosition.X >> 4, ScreenPosition.Y + BackgroundTop >> 4);
					color.R = (byte)(color.R * num);
					color.G = (byte)(color.G * num2);
					color.B = (byte)(color.B * num3);
					s.X += 16;
					SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_1, ref pos, ref s, color);
					num13--;
					num11 += 16;
				}
			}
			bool flag = false;
			if (Main.WorldSurfacePixels <= ScreenPosition.Y + Main.ResolutionHeight + OFFSCREEN_RANGE_TOP)
			{
				BackgroundTop = Main.WorldSurfacePixels - ScreenPosition.Y + 16;
				BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0) - 32;
				BackgroundLoops = (ViewWidth + 64) / 96 + 2;
				int BackgroundStartY;
				int BackgroundLoopsY;
				if (Main.WorldSurfacePixels < ScreenPosition.Y - 16)
				{
					BackgroundStartY = BackgroundTop % 96 - 96;
					BackgroundLoopsY = (Main.ResolutionHeight - BackgroundStartY + 96) / 96 + 1;
				}
				else
				{
					BackgroundStartY = BackgroundTop;
					BackgroundLoopsY = (Main.ResolutionHeight - BackgroundTop + 96) / 96 + 1;
				}

				int HeightAdjust = 540;
#if !USE_ORIGINAL_CODE
				if (Main.ScreenHeightPtr == 2)
				{
					HeightAdjust = 600; // Without this, good luck exploring the caverns in 1080p.
				}
#endif

				if (Main.RockLayerPixels < ScreenPosition.Y + HeightAdjust)
				{
					BackgroundLoopsY = (Main.RockLayerPixels - ScreenPosition.Y + HeightAdjust - BackgroundStartY) / 96;
					flag = true;
				}

				int num16 = BackgroundStart + ScreenPosition.X;
				num16 = -(num16 & 0xF);
				if (num16 == -8)
				{
					num16 = 8;
				}
				Vector2 pos2 = default(Vector2);
				Rectangle s2 = default(Rectangle);
				s2.Width = 16;
				s2.Height = 16;
				int j = 0;
				int num17 = BackgroundStart + 8 + ScreenPosition.X;
				for (; j < BackgroundLoops; j++)
				{
					int num18 = BackgroundStartY + 8 + ScreenPosition.Y >> 4;
					int num19 = 32 + BackgroundStartY;
					for (int k = 0; k < BackgroundLoopsY; k++)
					{
						for (int l = 0; l < 96; l += 16)
						{
							int num20 = 32 + BackgroundStart + 96 * j + l + num16;
							s2.X = l + num16 + 16;
							int num21 = num17 + l >> 4;
							fixed (Tile* ptr = &Main.TileSet[num21, num18])
							{
								Tile* ptr2 = ptr;
								for (int m = 0; m < 96; m += 16)
								{
									Color colorUnsafe = Lighting.GetColorUnsafe(num21, num18);
									pos2.X = num20;
									pos2.Y = num19 + m;
									s2.Y = m;
									if (colorUnsafe.R > 0 || colorUnsafe.G > 0 || colorUnsafe.B > 0)
									{
										if (SMOOTH_LIGHT && (colorUnsafe.R > 226 || colorUnsafe.G > 248.6f || colorUnsafe.B > 271.2f) && ptr2->IsActive == 0 && (ptr2->WallType == 0 || ptr2->WallType == 21))
										{
											s2.Width = 4;
											s2.Height = 4;
											Color c;
											if (ptr2[-(Main.LargeWorldH + 1)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 - 1, num18 - 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 8;
											s2.Y += 4;
											pos2.Y += 4f;
											if (ptr2[-(Main.LargeWorldH)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 - 1, num18);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 4;
											s2.Y += 8;
											pos2.Y += 8f;
											if (ptr2[-(Main.LargeWorldH - 1)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 - 1, num18 + 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Width = 8;
											s2.X += 4;
											pos2.X += 4f;
											if (ptr2[1].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21, num18 + 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 8;
											s2.Y -= 8;
											pos2.Y -= 8f;
											c.R = (byte)(colorUnsafe.R * num);
											c.G = (byte)(colorUnsafe.G * num2);
											c.B = (byte)(colorUnsafe.B * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 4;
											s2.Y -= 4;
											pos2.Y -= 4f;
											if (ptr2[-1].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21, num18 - 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Width = 4;
											s2.X += 8;
											pos2.X += 8f;
											if (ptr2[(Main.LargeWorldH - 1)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 + 1, num18 - 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 8;
											s2.Y += 4;
											pos2.Y += 4f;
											if (ptr2[(Main.LargeWorldH)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 + 1, num18);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Height = 4;
											s2.Y += 8;
											pos2.Y += 8f;
											if (ptr2[(Main.LargeWorldH + 1)].IsActive == 0)
											{
												c = Lighting.GetColorUnsafe(num21 + 1, num18 + 1);
												c.R = (byte)((colorUnsafe.R + c.R >> 1) * num);
												c.G = (byte)((colorUnsafe.G + c.G >> 1) * num2);
												c.B = (byte)((colorUnsafe.B + c.B >> 1) * num3);
											}
											else
											{
												c = colorUnsafe;
											}
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c);
											s2.Width = (s2.Height = 16);
											s2.X -= 12;
										}
										else if (SMOOTH_LIGHT && (colorUnsafe.R > 160 || colorUnsafe.G > 176f || colorUnsafe.B > 192f))
										{
											s2.Width = 8;
											s2.Height = 8;
											Color c2 = ((!Lighting.Brighter(num21, num18 - 1, num21 - 1, num18)) ? Lighting.GetColorUnsafe(num21, num18 - 1) : Lighting.GetColorUnsafe(num21 - 1, num18));
											c2.R = (byte)((colorUnsafe.R + c2.R >> 1) * num);
											c2.G = (byte)((colorUnsafe.G + c2.G >> 1) * num2);
											c2.B = (byte)((colorUnsafe.B + c2.B >> 1) * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c2);
											s2.Y += 8;
											pos2.Y += 8f;
											c2 = ((!Lighting.Brighter(num21, num18 + 1, num21 - 1, num18)) ? Lighting.GetColorUnsafe(num21, num18 + 1) : Lighting.GetColorUnsafe(num21 - 1, num18));
											c2.R = (byte)((colorUnsafe.R + c2.R >> 1) * num);
											c2.G = (byte)((colorUnsafe.G + c2.G >> 1) * num2);
											c2.B = (byte)((colorUnsafe.B + c2.B >> 1) * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c2);
											s2.X += 8;
											pos2.X += 8f;
											c2 = ((!Lighting.Brighter(num21, num18 + 1, num21 + 1, num18)) ? Lighting.GetColorUnsafe(num21, num18 + 1) : Lighting.GetColorUnsafe(num21 + 1, num18));
											c2.R = (byte)((colorUnsafe.R + c2.R >> 1) * num);
											c2.G = (byte)((colorUnsafe.G + c2.G >> 1) * num2);
											c2.B = (byte)((colorUnsafe.B + c2.B >> 1) * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c2);
											s2.Y -= 8;
											pos2.Y -= 8f;
											c2 = ((!Lighting.Brighter(num21, num18 - 1, num21 + 1, num18)) ? Lighting.GetColorUnsafe(num21, num18 - 1) : Lighting.GetColorUnsafe(num21 + 1, num18));
											c2.R = (byte)((colorUnsafe.R + c2.R >> 1) * num);
											c2.G = (byte)((colorUnsafe.G + c2.G >> 1) * num2);
											c2.B = (byte)((colorUnsafe.B + c2.B >> 1) * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, c2);
											s2.Width = (s2.Height = 16);
											s2.X -= 8;
										}
										else
										{
											colorUnsafe.R = (byte)(colorUnsafe.R * num);
											colorUnsafe.G = (byte)(colorUnsafe.G * num2);
											colorUnsafe.B = (byte)(colorUnsafe.B * num3);
											SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, colorUnsafe);
										}
									}
									else
									{
										SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.BACKGROUND_2, ref pos2, ref s2, colorUnsafe);
									}
									num18++;
									ptr2++;
								}
								num18 -= 6;
							}
						}
						num19 += 96;
						num18 += 6;
					}
					num17 += 96;
				}
				if (flag)
				{
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0);
					BackgroundLoops = (ViewWidth + 64) / 96 + 2;
					BackgroundTop = BackgroundStartY + BackgroundLoopsY * 96;
					if (BackgroundTop > -32)
					{
						Vector2 vector = new Vector2(32 + BackgroundStart + num16, 32 + BackgroundTop);
						int num22 = BackgroundStart + 8;
						for (int n = 0; n < BackgroundLoops; n++)
						{
							for (int num23 = 0; num23 < 96; num23 += 16)
							{
								Color color2 = Lighting.GetColor(num22 + ScreenPosition.X >> 4, ScreenPosition.Y + BackgroundTop >> 4);
								num22 += 16;
								color2.R = (byte)(color2.R * num);
								color2.G = (byte)(color2.G * num2);
								color2.B = (byte)(color2.B * num3);
								Main.SpriteBatch.Draw(backgroundTexture[4], vector, (Rectangle?)new Rectangle(num23 + num16 + 16, 0, 16, 16), color2);
								vector.X += 16f;
							}
						}
					}
				}
			}
			bool flag2 = false;
			int magmaLayerPixels = Main.MagmaLayerPixels;

			int HeightAdjust2 = 540;
#if !USE_ORIGINAL_CODE
			if (Main.ScreenHeightPtr == 2)
			{
				HeightAdjust2 = 600;
			}
#endif

			if (Main.RockLayerPixels <= ScreenPosition.Y + HeightAdjust2)
			{
				BackgroundTop = Main.RockLayerPixels - ScreenPosition.Y + HeightAdjust2 - 28;
				BackgroundStart = (int)(0.0 - Math.IEEERemainder(96f + ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0) - 32;
				BackgroundLoops = (ViewWidth + 64) / 96 + 2;
				int num14;
				int num15;
				if (Main.RockLayerPixels + Main.ResolutionHeight < ScreenPosition.Y - 16)
				{
					num14 = (int)(Math.IEEERemainder(BackgroundTop, 96.0) - 96.0);
					num15 = (Main.ResolutionHeight - num14 + 96) / 96 + 1;
				}
				else
				{
					num14 = BackgroundTop;
					num15 = (Main.ResolutionHeight - BackgroundTop + 96) / 96 + 1;
				}
				if (magmaLayerPixels < ScreenPosition.Y + HeightAdjust2)
				{
					num15 = (magmaLayerPixels - ScreenPosition.Y + HeightAdjust2 - num14) / 96;
					flag2 = true;
				}
				int num24 = BackgroundStart + ScreenPosition.X;
				num24 = -(num24 & 0xF);
				if (num24 == -8)
				{
					num24 = 8;
				}
				for (int num25 = 0; num25 < BackgroundLoops; num25++)
				{
					for (int num26 = 0; num26 < num15; num26++)
					{
						for (int num27 = 0; num27 < 96; num27 += 16)
						{
							int num28 = BackgroundStart + 96 * num25 + num27 + 8;
							int num29 = num28 + ScreenPosition.X >> 4;
							for (int num30 = 0; num30 < 96; num30 += 16)
							{
								int num31 = num14 + num26 * 96 + num30 + 8;
								int num32 = num31 + ScreenPosition.Y >> 4;
								Color colorUnsafe2 = Lighting.GetColorUnsafe(num29, num32);
								bool flag3 = false;
								int wall = Main.TileSet[num29, num32].WallType;
								if (wall == 0 || wall == 21 || Main.TileSet[num29 - 1, num32].WallType == 0 || Main.TileSet[num29 - 1, num32].WallType == 21 || Main.TileSet[num29 + 1, num32].WallType == 0 || Main.TileSet[num29 + 1, num32].WallType == 21)
								{
									flag3 = true;
								}
								if ((!flag3 && colorUnsafe2.R != 0 && colorUnsafe2.G != 0 && colorUnsafe2.B != 0) || (colorUnsafe2.R <= 0 && colorUnsafe2.G <= 0 && colorUnsafe2.B <= 0))
								{
									continue;
								}
								if (SMOOTH_LIGHT && colorUnsafe2.R < 230 && colorUnsafe2.G < 230 && colorUnsafe2.B < 230)
								{
									if ((colorUnsafe2.R > 226 || colorUnsafe2.G > 248.6 || colorUnsafe2.B > 271.2) && Main.TileSet[num29, num32].IsActive == 0)
									{
										for (int num33 = 0; num33 < 9; num33++)
										{
											int num34 = 0;
											int num35 = 0;
											int width = 4;
											int height = 4;
											Color color3 = colorUnsafe2;
											Color color4 = colorUnsafe2;
											switch (num33)
											{
											case 0:
												if (Main.TileSet[num29 - 1, num32 - 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 - 1, num32 - 1);
												}
												break;
											case 1:
												width = 8;
												num34 = 4;
												if (Main.TileSet[num29, num32 - 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29, num32 - 1);
												}
												break;
											case 2:
												if (Main.TileSet[num29 + 1, num32 - 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 + 1, num32 - 1);
												}
												num34 = 12;
												break;
											case 3:
												if (Main.TileSet[num29 - 1, num32].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 - 1, num32);
												}
												height = 8;
												num35 = 4;
												break;
											case 4:
												width = 8;
												height = 8;
												num34 = 4;
												num35 = 4;
												break;
											case 5:
												num34 = 12;
												num35 = 4;
												height = 8;
												if (Main.TileSet[num29 + 1, num32].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 + 1, num32);
												}
												break;
											case 6:
												if (Main.TileSet[num29 - 1, num32 + 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 - 1, num32 + 1);
												}
												num35 = 12;
												break;
											case 7:
												width = 8;
												height = 4;
												num34 = 4;
												num35 = 12;
												if (Main.TileSet[num29, num32 + 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29, num32 + 1);
												}
												break;
											default:
												if (Main.TileSet[num29 + 1, num32 + 1].IsActive == 0)
												{
													color4 = Lighting.GetColorUnsafe(num29 + 1, num32 + 1);
												}
												num34 = 12;
												num35 = 12;
												break;
											}
											color3.R = (byte)((colorUnsafe2.R + color4.R >> 1) * num);
											color3.G = (byte)((colorUnsafe2.G + color4.G >> 1) * num2);
											color3.B = (byte)((colorUnsafe2.B + color4.B >> 1) * num3);
											Main.SpriteBatch.Draw(backgroundTexture[3], new Vector2(32 + BackgroundStart + 96 * num25 + num27 + num34 + num24, 32 + num14 + 96 * num26 + num30 + num35), (Rectangle?)new Rectangle(num27 + num34 + num24 + 16, num30 + num35, width, height), color3);
										}
									}
									else if (colorUnsafe2.R > 160 || colorUnsafe2.G > 176.0 || colorUnsafe2.B > 192.0)
									{
										for (int num36 = 0; num36 < 4; num36++)
										{
											int num37 = 0;
											int num38 = 0;
											Color color5 = colorUnsafe2;
											Color color6 = colorUnsafe2;
											switch (num36)
											{
											case 0:
												color6 = ((!Lighting.Brighter(num29, num32 - 1, num29 - 1, num32)) ? Lighting.GetColorUnsafe(num29, num32 - 1) : Lighting.GetColorUnsafe(num29 - 1, num32));
												break;
											case 1:
												color6 = ((!Lighting.Brighter(num29, num32 - 1, num29 + 1, num32)) ? Lighting.GetColorUnsafe(num29, num32 - 1) : Lighting.GetColorUnsafe(num29 + 1, num32));
												num37 = 8;
												break;
											case 2:
												color6 = ((!Lighting.Brighter(num29, num32 + 1, num29 - 1, num32)) ? Lighting.GetColorUnsafe(num29, num32 + 1) : Lighting.GetColorUnsafe(num29 - 1, num32));
												num38 = 8;
												break;
											default:
												color6 = ((!Lighting.Brighter(num29, num32 + 1, num29 + 1, num32)) ? Lighting.GetColorUnsafe(num29, num32 + 1) : Lighting.GetColorUnsafe(num29 + 1, num32));
												num37 = 8;
												num38 = 8;
												break;
											}
											color5.R = (byte)((colorUnsafe2.R + color6.R >> 1) * num);
											color5.G = (byte)((colorUnsafe2.G + color6.G >> 1) * num2);
											color5.B = (byte)((colorUnsafe2.B + color6.B >> 1) * num3);
											Main.SpriteBatch.Draw(backgroundTexture[3], new Vector2(32 + BackgroundStart + 96 * num25 + num27 + num37 + num24, 32 + num14 + 96 * num26 + num30 + num38), (Rectangle?)new Rectangle(num27 + num37 + num24 + 16, num30 + num38, 8, 8), color5);
										}
									}
									else
									{
										colorUnsafe2.R = (byte)(colorUnsafe2.R * num);
										colorUnsafe2.G = (byte)(colorUnsafe2.G * num2);
										colorUnsafe2.B = (byte)(colorUnsafe2.B * num3);
										Main.SpriteBatch.Draw(backgroundTexture[3], new Vector2(32 + BackgroundStart + 96 * num25 + num27 + num24, 32 + num14 + 96 * num26 + num30), (Rectangle?)new Rectangle(num27 + num24 + 16, num30, 16, 16), colorUnsafe2);
									}
								}
								else
								{
									colorUnsafe2.R = (byte)(colorUnsafe2.R * num);
									colorUnsafe2.G = (byte)(colorUnsafe2.G * num2);
									colorUnsafe2.B = (byte)(colorUnsafe2.B * num3);
									Main.SpriteBatch.Draw(backgroundTexture[3], new Vector2(32 + BackgroundStart + 96 * num25 + num27 + num24, 32 + num14 + 96 * num26 + num30), (Rectangle?)new Rectangle(num27 + num24 + 16, num30, 16, 16), colorUnsafe2);
								}
							}
						}
					}
				}
				if (flag2)
				{
					BackgroundStart = (int)(0.0 - Math.IEEERemainder(ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0);
					BackgroundLoops = ViewWidth / 96 + 2;
					BackgroundTop = num14 + num15 * 96;
					Rectangle value = new Rectangle(0, Main.MagmaBGFrame << 4, 16, 16);
					int num39 = BackgroundStart + 8;
					for (int num40 = 0; num40 < BackgroundLoops; num40++)
					{
						for (int num41 = 0; num41 < 96; num41 += 16)
						{
							value.X = num41 + num24 + 16;
							Color color7 = Lighting.GetColor(num39 + ScreenPosition.X >> 4, ScreenPosition.Y + BackgroundTop >> 4);
							color7.R = (byte)(color7.R * num);
							color7.G = (byte)(color7.G * num2);
							color7.B = (byte)(color7.B * num3);
							Main.SpriteBatch.Draw(backgroundTexture[6], new Vector2(32 + BackgroundStart + 96 * num40 + num41 + num24, 32 + BackgroundTop), (Rectangle?)value, color7);
							num39 += 16;
						}
					}
				}
			}
			if (magmaLayerPixels <= ScreenPosition.Y + HeightAdjust2)
			{
				BackgroundTop = magmaLayerPixels - ScreenPosition.Y + HeightAdjust2 - 28;
				BackgroundStart = (int)(0.0 - Math.IEEERemainder(96f + ScreenPosition.X * CAVE_PARALLAX, 96.0) - 48.0) - 32;
				BackgroundLoops = (ViewWidth + 64) / 96 + 2;
				int BackgroundStartY;
				int BackgroundLoopsY;
				if (magmaLayerPixels + Main.ResolutionHeight < ScreenPosition.Y - 16)
				{
					BackgroundStartY = (int)(Math.IEEERemainder(BackgroundTop, 96.0) - 96.0);
					BackgroundLoopsY = (Main.ResolutionHeight - BackgroundStartY + 96) / 96 + 1;
				}
				else
				{
					BackgroundStartY = BackgroundTop;
					BackgroundLoopsY = (Main.ResolutionHeight - BackgroundTop + 96) / 96 + 1;
				}
				float num42 = BackgroundStart + ScreenPosition.X;
				num42 = 0f - (float)Math.IEEERemainder(num42, 16.0);
				num42 = (float)Math.Round(num42);
				int num43 = (int)num42;
				if (num43 == -8)
				{
					num43 = 8;
				}
				for (int num44 = 0; num44 < BackgroundLoops; num44++)
				{
					for (int num45 = 0; num45 < BackgroundLoopsY; num45++)
					{
						for (int num46 = 0; num46 < 96; num46 += 16)
						{
							int num47 = BackgroundStart + 96 * num44 + num46 + 8 + ScreenPosition.X >> 4;
							int num48 = BackgroundStartY + num45 * 96 + 8 + ScreenPosition.Y >> 4;
							for (int num49 = 0; num49 < 96; num49 += 16)
							{
								Color colorUnsafe3 = Lighting.GetColorUnsafe(num47, num48);
								bool flag4 = false;
								int wall2 = Main.TileSet[num47, num48].WallType;
								if (wall2 == 0 || wall2 == 21 || Main.TileSet[num47 - 1, num48].WallType == 0 || Main.TileSet[num47 - 1, num48].WallType == 21 || Main.TileSet[num47 + 1, num48].WallType == 0 || Main.TileSet[num47 + 1, num48].WallType == 21)
								{
									flag4 = true;
								}
								if ((flag4 || colorUnsafe3.R == 0 || colorUnsafe3.G == 0 || colorUnsafe3.B == 0) && (colorUnsafe3.R > 0 || colorUnsafe3.G > 0 || colorUnsafe3.B > 0))
								{
									if (SMOOTH_LIGHT && colorUnsafe3.R < 230 && colorUnsafe3.G < 230 && colorUnsafe3.B < 230)
									{
										if ((colorUnsafe3.R > 339f || colorUnsafe3.G > 372.9f || colorUnsafe3.B > 1278f / (float)Math.PI) && Main.TileSet[num47, num48].IsActive == 0)
										{
											for (int num50 = 0; num50 < 9; num50++)
											{
												int num51 = 0;
												int num52 = 0;
												int width2 = 4;
												int height2 = 4;
												Color color8 = colorUnsafe3;
												Color color9 = colorUnsafe3;
												switch (num50)
												{
												case 0:
													if (Main.TileSet[num47 - 1, num48 - 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 - 1, num48 - 1);
													}
													break;
												case 1:
													width2 = 8;
													num51 = 4;
													if (Main.TileSet[num47, num48 - 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47, num48 - 1);
													}
													break;
												case 2:
													if (Main.TileSet[num47 + 1, num48 - 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 + 1, num48 - 1);
													}
													num51 = 12;
													break;
												case 3:
													if (Main.TileSet[num47 - 1, num48].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 - 1, num48);
													}
													height2 = 8;
													num52 = 4;
													break;
												case 4:
													width2 = 8;
													height2 = 8;
													num51 = 4;
													num52 = 4;
													break;
												case 5:
													num51 = 12;
													num52 = 4;
													height2 = 8;
													if (Main.TileSet[num47 + 1, num48].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 + 1, num48);
													}
													break;
												case 6:
													if (Main.TileSet[num47 - 1, num48 + 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 - 1, num48 + 1);
													}
													num52 = 12;
													break;
												case 7:
													width2 = 8;
													height2 = 4;
													num51 = 4;
													num52 = 12;
													if (Main.TileSet[num47, num48 + 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47, num48 + 1);
													}
													break;
												default:
													if (Main.TileSet[num47 + 1, num48 + 1].IsActive == 0)
													{
														color9 = Lighting.GetColorUnsafe(num47 + 1, num48 + 1);
													}
													num51 = 12;
													num52 = 12;
													break;
												}
												color8.R = (byte)((colorUnsafe3.R + color9.R >> 1) * num);
												color8.G = (byte)((colorUnsafe3.G + color9.G >> 1) * num2);
												color8.B = (byte)((colorUnsafe3.B + color9.B >> 1) * num3);
												Main.SpriteBatch.Draw(backgroundTexture[5], new Vector2(32 + BackgroundStart + 96 * num44 + num46 + num51 + num43, 32 + BackgroundStartY + 96 * num45 + num49 + num52), (Rectangle?)new Rectangle(num46 + num51 + num43 + 16, num49 + 96 * Main.MagmaBGFrame + num52, width2, height2), color8);
											}
										}
										else if (colorUnsafe3.R > 240 || colorUnsafe3.G > 264.0 || colorUnsafe3.B > 288.0)
										{
											for (int num53 = 0; num53 < 4; num53++)
											{
												int num54 = 0;
												int num55 = 0;
												Color color10 = colorUnsafe3;
												Color color11 = colorUnsafe3;
												switch (num53)
												{
												case 0:
													color11 = ((!Lighting.Brighter(num47, num48 - 1, num47 - 1, num48)) ? Lighting.GetColorUnsafe(num47, num48 - 1) : Lighting.GetColorUnsafe(num47 - 1, num48));
													break;
												case 1:
													color11 = ((!Lighting.Brighter(num47, num48 - 1, num47 + 1, num48)) ? Lighting.GetColorUnsafe(num47, num48 - 1) : Lighting.GetColorUnsafe(num47 + 1, num48));
													num54 = 8;
													break;
												case 2:
													color11 = ((!Lighting.Brighter(num47, num48 + 1, num47 - 1, num48)) ? Lighting.GetColorUnsafe(num47, num48 + 1) : Lighting.GetColorUnsafe(num47 - 1, num48));
													num55 = 8;
													break;
												default:
													color11 = ((!Lighting.Brighter(num47, num48 + 1, num47 + 1, num48)) ? Lighting.GetColorUnsafe(num47, num48 + 1) : Lighting.GetColorUnsafe(num47 + 1, num48));
													num54 = 8;
													num55 = 8;
													break;
												}
												color10.R = (byte)((colorUnsafe3.R + color11.R >> 1) * num);
												color10.G = (byte)((colorUnsafe3.G + color11.G >> 1) * num2);
												color10.B = (byte)((colorUnsafe3.B + color11.B >> 1) * num3);
												Main.SpriteBatch.Draw(backgroundTexture[5], new Vector2(32 + BackgroundStart + 96 * num44 + num46 + num54 + num43, 32 + BackgroundStartY + 96 * num45 + num49 + num55), (Rectangle?)new Rectangle(num46 + num54 + num43 + 16, num49 + 96 * Main.MagmaBGFrame + num55, 8, 8), color10);
											}
										}
										else
										{
											colorUnsafe3.R = (byte)(colorUnsafe3.R * num);
											colorUnsafe3.G = (byte)(colorUnsafe3.G * num2);
											colorUnsafe3.B = (byte)(colorUnsafe3.B * num3);
											Main.SpriteBatch.Draw(backgroundTexture[5], new Vector2(32 + BackgroundStart + 96 * num44 + num46 + num43, 32 + BackgroundStartY + 96 * num45 + num49), (Rectangle?)new Rectangle(num46 + num43 + 16, num49 + 96 * Main.MagmaBGFrame, 16, 16), colorUnsafe3);
										}
									}
									else
									{
										colorUnsafe3.R = (byte)(colorUnsafe3.R * num);
										colorUnsafe3.G = (byte)(colorUnsafe3.G * num2);
										colorUnsafe3.B = (byte)(colorUnsafe3.B * num3);
										Main.SpriteBatch.Draw(backgroundTexture[5], new Vector2(32 + BackgroundStart + 96 * num44 + num46 + num43, 32 + BackgroundStartY + 96 * num45 + num49), (Rectangle?)new Rectangle(num46 + num43 + 16, num49 + 96 * Main.MagmaBGFrame, 16, 16), colorUnsafe3);
									}
								}
								num48++;
							}
						}
					}
				}
			}
			Lighting.BrightnessLevel = (Player.blind ? 1f : Lighting.DefBrightness);
		}

		private void RenderBlack()
		{
			GraphicsDevice.SetRenderTarget(blackTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawBlack();
			Main.SpriteBatch.End();
			sceneBlackPos.X = ScreenPosition.X - 32;
			sceneBlackPos.Y = ScreenPosition.Y - 32;
		}

		private void RenderWalls()
		{
			GraphicsDevice.SetRenderTarget(wallTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawWalls();
			Main.SpriteBatch.End();
			sceneWallPos.X = ScreenPosition.X - 32;
			sceneWallPos.Y = ScreenPosition.Y - 32;
		}

		private void RenderBackWater()
		{
			GraphicsDevice.SetRenderTarget(backWaterTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawWater(bg: true);
			Main.SpriteBatch.End();
		}

		private void RenderBackground()
		{
			GraphicsDevice.SetRenderTarget(backgroundTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawBackground();
			Main.SpriteBatch.End();
			sceneBackgroundPos.X = ScreenPosition.X - 32;
			sceneBackgroundPos.Y = ScreenPosition.Y - 32;
		}

		private void RenderSolidTiles()
		{
			GraphicsDevice.SetRenderTarget(tileSolidTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, renderTargetProjection);
			DrawSolidTiles();
			Main.SpriteBatch.End();
			sceneTilePos.X = ScreenPosition.X - 32;
			sceneTilePos.Y = ScreenPosition.Y - 32;
		}

		private void RenderNonSolidTiles()
		{
			GraphicsDevice.SetRenderTarget(tileNonSolidTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawNonSolidTiles();
			Main.SpriteBatch.End();
			sceneTile2Pos.X = ScreenPosition.X - 32;
			sceneTile2Pos.Y = ScreenPosition.Y - 32;
		}

		private void RenderWater()
		{
			GraphicsDevice.SetRenderTarget(waterTarget);
			GraphicsDevice.Clear(default(Color));
			Main.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, renderTargetProjection);
			DrawWater();
			if (Player.Inventory[Player.SelectedItem].Mech)
			{
				DrawWires();
			}
			Main.SpriteBatch.End();
			sceneWaterPos.X = ScreenPosition.X - 32;
			sceneWaterPos.Y = ScreenPosition.Y - 32;
		}

		public static void shine(ref Color newColor, int type)
		{
			int num;
			int num2;
			int num3;
			switch (type)
			{
			case 25:
				num = newColor.R * 243 >> 8;
				num2 = newColor.G * 217 >> 8;
				num3 = newColor.B * 281 >> 8;
				break;
			case 117:
				num = newColor.R * 281 >> 8;
				num2 = newColor.G;
				num3 = newColor.B * 307 >> 8;
				if (num > 255)
				{
					num = 255;
				}
				break;
			default:
				num = newColor.R * 409 >> 8;
				num2 = newColor.G * 409 >> 8;
				num3 = newColor.B * 409 >> 8;
				if (num > 255)
				{
					num = 255;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				break;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			newColor.R = (byte)num;
			newColor.G = (byte)num2;
			newColor.B = (byte)num3;
		}

		private unsafe void Highlight2x1(Tile* pTile, Tile.Flags mask)
		{
			pTile->CurrentFlags |= mask;
			pTile = ((pTile->FrameX != 0) ? (pTile - (Main.LargeWorldH)) : (pTile + (Main.LargeWorldH)));
			pTile->CurrentFlags |= mask;
		}

		private unsafe void Highlight2x2(Tile* pTile, Tile.Flags mask)
		{
			int num = ((pTile->FrameY == 0) ? 1 : (-1));
			pTile->CurrentFlags |= mask;
			pTile += num;
			pTile->CurrentFlags |= mask;
			pTile = ((((uint)(pTile->FrameX / 18) & (true ? 1u : 0u)) != 0) ? (pTile - (Main.LargeWorldH)) : (pTile + (Main.LargeWorldH)));
			pTile->CurrentFlags |= mask;
			pTile -= num;
			pTile->CurrentFlags |= mask;
		}

		private unsafe void Highlight1x3(Tile* pTile, Tile.Flags mask)
		{
			pTile->CurrentFlags |= mask;
			if (pTile->FrameY == 0)
			{
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile++;
			}
			else if (pTile->FrameY == 18)
			{
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= 2;
			}
			else
			{
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile--;
			}
			pTile->CurrentFlags |= mask;
		}

		private unsafe void Highlight2x3(Tile* pTile, Tile.Flags mask)
		{
			pTile->CurrentFlags |= mask;
			if (pTile->FrameY == 0)
			{
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile++;
				pTile->CurrentFlags |= mask;
				if (((pTile->FrameX / 18) & 1) == 0)
				{
					pTile += (Main.LargeWorldH - 2);
					pTile->CurrentFlags |= mask;
					pTile++;
					pTile->CurrentFlags |= mask;
					pTile++;
				}
				else
				{
					pTile -= (Main.LargeWorldH);
					pTile->CurrentFlags |= mask;
					pTile--;
					pTile->CurrentFlags |= mask;
					pTile--;
				}
			}
			else if (pTile->FrameY == 18)
			{
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= 2;
				pTile->CurrentFlags |= mask;
				if (((pTile->FrameX / 18) & 1) == 0)
				{
					pTile += (Main.LargeWorldH);
					pTile->CurrentFlags |= mask;
					pTile++;
					pTile->CurrentFlags |= mask;
					pTile++;
				}
				else
				{
					pTile -= (Main.LargeWorldH);
					pTile->CurrentFlags |= mask;
					pTile++;
					pTile->CurrentFlags |= mask;
					pTile++;
				}
			}
			else
			{
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile--;
				pTile->CurrentFlags |= mask;
				if (((pTile->FrameX / 18) & 1) == 0)
				{
					pTile += (Main.LargeWorldH);
					pTile->CurrentFlags |= mask;
					pTile++;
					pTile->CurrentFlags |= mask;
					pTile++;
				}
				else
				{
					pTile -= (Main.LargeWorldH);
					pTile->CurrentFlags |= mask;
					pTile++;
					pTile->CurrentFlags |= mask;
					pTile++;
				}
			}
			pTile->CurrentFlags |= mask;
		}

		private unsafe void Highlight4x2(Tile* pTile, Tile.Flags mask)
		{
			int num = ((pTile->FrameY == 0) ? 1 : (-1));
			pTile->CurrentFlags |= mask;
			pTile += num;
			pTile->CurrentFlags |= mask;
			switch ((pTile->FrameX / 18) & 3)
			{
			case 0:
				pTile += (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile += (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile += (Main.LargeWorldH);
				break;
			case 1:
				pTile -= (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile += (Main.LargeWorldH * 2);
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile += (Main.LargeWorldH);
				break;
			case 2:
				pTile += (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile -= (Main.LargeWorldH * 2);
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile -= (Main.LargeWorldH);
				break;
			default:
				pTile -= (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile -= (Main.LargeWorldH);
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile -= (Main.LargeWorldH);
				break;
			}
			pTile->CurrentFlags |= mask;
			pTile -= num;
			pTile->CurrentFlags |= mask;
		}

		private unsafe void Highlight2x5(Tile* pTile, Tile.Flags mask)
		{
			int num = ((pTile->FrameX == 0) ? (Main.LargeWorldH) : (-(Main.LargeWorldH)));
			pTile->CurrentFlags |= mask;
			pTile += num;
			pTile->CurrentFlags |= mask;
			switch (pTile->FrameY / 18)
			{
			case 0:
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile++;
				break;
			case 1:
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile += 2;
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile++;
				break;
			case 2:
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile += 3;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile++;
				break;
			case 3:
				pTile++;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile -= 2;
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile--;
				break;
			default:
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile += num;
				pTile->CurrentFlags |= mask;
				pTile--;
				pTile->CurrentFlags |= mask;
				pTile -= num;
				pTile->CurrentFlags |= mask;
				pTile--;
				break;
			}
			pTile->CurrentFlags |= mask;
			pTile += num;
			pTile->CurrentFlags |= mask;
		}

		private unsafe void DrawNonSolidTiles()
		{
			int gfx = (int)(255f * (1f - gfxQuality) + 30f * gfxQuality);
			int gfx2 = (int)(50f * (1f - gfxQuality) + 2f * gfxQuality);
			int num = 0;
			Rectangle s = default(Rectangle);
			Vector2 pos = default(Vector2);
			Main.TileSolid[10] = false;
			fixed (Tile* ptr = Main.TileSet)
			{
				int i;
				if (!Player.IsDead)
				{
					Tile* ptr2 = (Tile*)(((Main.FrameCounter & 0x20) == 0) ? ((IntPtr)(void*)null) : ((IntPtr)(ptr + (Player.tileInteractX * (Main.LargeWorldH) + Player.tileInteractY))));
					int num2 = (Player.XYWH.X + 10 >> 4) - 10;
					int num3 = (Player.XYWH.Y + 21 >> 4) - 8;
					for (i = 0; i < 20; i++)
					{
						Tile* ptr3 = ptr + ((num2 + i) * (Main.LargeWorldH) + num3);
						for (int j = 0; j < 16; j++)
						{
							if (ptr3->IsActive != 0)
							{
								Tile.Flags flags = ((ptr3 == ptr2) ? Flags.SELECTED : Flags.NEARBY);
								if ((ptr3->CurrentFlags & flags) != flags)
								{
									switch (ptr3->Type)
									{
									case 4:
									case 13:
									case 33:
									case 49:
									case 50:
									case 136:
									case 144:
										ptr3->CurrentFlags |= flags;
										break;
									case 11:
									case 128:
										Highlight2x3(ptr3, flags);
										break;
									case 10:
										Highlight1x3(ptr3, flags);
										break;
									case 21:
									case 55:
									case 85:
									case 97:
									case 125:
									case 132:
									case 139:
										Highlight2x2(ptr3, flags);
										break;
									case 29:
										Highlight2x1(ptr3, flags);
										break;
									case 79:
										Highlight4x2(ptr3, flags);
										break;
									case 104:
										Highlight2x5(ptr3, flags);
										break;
									}
								}
							}
							ptr3++;
						}
					}
				}
				i = FirstTileX - 1;
				int num4 = LastTileX + 2;
				int num5 = LastTileY + 2;
				do
				{
					int num6 = FirstTileY;
					Tile* ptr4 = ptr + (i * (Main.LargeWorldH) + num6 - 1);
					do
					{
						ptr4++;
						if (ptr4->IsActive == 0)
						{
							continue;
						}
						int num7 = ptr4->Type;
						if (Main.TileSolid[num7])
						{
							continue;
						}
						Color newColor = Lighting.GetColorUnsafe(i, num6);
						int num8 = 0;
						int height = 16;
						int num9 = 16;
						s.X = ptr4->FrameX;
						s.Y = ptr4->FrameY;
						switch (num7)
						{
						case 3:
						case 24:
						case 61:
						case 71:
						case 110:
							height = 20;
							break;
						case 4:
							num9 = 20;
							height = 20;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 396;
							}
							break;
						case 5:
							num9 = 20;
							height = 20;
							break;
						case 10:
						case 11:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 108;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 54;
							}
							break;
						case 13:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 36;
							}
							break;
						case 29:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 36;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 18;
							}
							break;
						case 21:
							height = 18;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 76;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 38;
							}
							break;
						case 33:
						case 49:
							num8 = -4;
							height = 20;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 44;
							}
							break;
						case 50:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 32;
							}
							break;
						case 73:
						case 74:
						case 113:
							num8 = -12;
							height = 32;
							break;
						case 78:
						case 105:
						case 142:
						case 143:
							num8 = 2;
							break;
						case 81:
							num8 = -8;
							num9 = 24;
							height = 26;
							break;
						case 85:
							num8 = 2;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 72;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 36;
							}
							break;
						case 104:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 176;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 88;
							}
							break;
						case 97:
						case 125:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 68;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 34;
							}
							break;
						case 128:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 104;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 52;
							}
							break;
						case 132:
							num8 = 2;
							height = 18;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 72;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 36;
							}
							break;
						case 135:
							num8 = 2;
							height = 18;
							break;
						case 55:
						case 79:
						case 136:
						case 144:
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 72;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 36;
							}
							break;
						case 14:
						case 15:
						case 16:
						case 17:
						case 18:
						case 20:
						case 26:
						case 27:
						case 32:
						case 69:
						case 72:
						case 77:
						case 80:
						case 124:
							height = 18;
							break;
						case 139:
							num8 = 2;
							if ((ptr4->CurrentFlags & Flags.SELECTED) != 0)
							{
								s.Y += 1512;
							}
							else if ((ptr4->CurrentFlags & Flags.NEARBY) != 0)
							{
								s.Y += 756;
							}
							break;
						}

#if VERSION_101
						if (num7 == 12 || num7 == 31)
						{
							if (num7 == 12)
							{
								s.Y += Main.HeartCrystalFrame * 36;
							}
							else
							{
								s.Y += Main.ShadowOrbFrame * 36;
							}
						}

						if (num7 == 17 || num7 == 77)
						{
							s.Y += Main.FurnaceFrame * 38;
						}

						if (num7 == 150)
						{
							s.Y += Main.CampfireFrame * 36;
							num8 = 2;
						}
#endif

						s.Width = num9;
						s.Height = height;
						pos.X = (i << 4) - ScreenPosition.X - (num9 - 16 >> 1) + 32;
						pos.Y = (num6 << 4) - ScreenPosition.Y + num8 + 32;
						if (Player.findTreasure)
						{
							switch (num7)
							{
							case 12:
							case 21:
							case 28:
							case 82:
							case 83:
							case 84:
								if (newColor.R < UI.MouseTextBrightness >> 1)
								{
									newColor.R = (byte)(UI.MouseTextBrightness >> 1);
								}
								if (newColor.G < 70)
								{
									newColor.G = 70;
								}
								if (newColor.B < 210)
								{
									newColor.B = 210;
								}
								newColor.A = UI.MouseTextBrightness;
								if (Main.Rand.Next(150) == 0)
								{
									Dust* ptr5 = dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 15, 0.0, 0.0, 150, default(Color), 0.8f);
									if (ptr5 != null)
									{
										ptr5->Velocity.X *= 0.1f;
										ptr5->Velocity.Y *= 0.1f;
										ptr5->NoLight = true;
									}
								}
								break;
							}
						}
						switch (num7)
						{
						case 4:
							if (ptr4->FrameX < 66 && Main.Rand.Next(40) == 0)
							{
								int num11 = ptr4->FrameY / 22;
								switch (num11)
								{
								case 0:
									num11 = 6;
									break;
								case 8:
									num11 = 75;
									break;
								default:
									num11 = 58 + num11;
									break;
								}
								if (ptr4->FrameX == 22)
								{
									dustLocal.NewDust(i * 16 + 6, num6 * 16, 4, 4, num11, 0.0, 0.0, 100);
								}
								else if (ptr4->FrameX == 44)
								{
									dustLocal.NewDust(i * 16 + 2, num6 * 16, 4, 4, num11, 0.0, 0.0, 100);
								}
								else
								{
									dustLocal.NewDust(i * 16 + 4, num6 * 16, 4, 4, num11, 0.0, 0.0, 100);
								}
							}
							break;
						case 24:
						case 32:
							if (Main.Rand.Next(500) == 0)
							{
								dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 14);
							}
							break;
						case 26:
						case 31:
							if (Main.Rand.Next(20) == 0)
							{
								dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 14, 0.0, 0.0, 100);
							}
							break;
						case 33:
							if (ptr4->FrameX == 0 && Main.Rand.Next(40) == 0)
							{
								dustLocal.NewDust(i * 16 + 4, num6 * 16 - 4, 4, 4, 6, 0.0, 0.0, 100);
							}
							break;
						case 34:
						case 35:
						case 36:
							if ((ptr4->FrameX == 0 || ptr4->FrameX == 36) && ptr4->FrameY == 18 && Main.Rand.Next(40) == 0)
							{
								dustLocal.NewDust(i * 16, num6 * 16 + 2, 14, 6, 6, 0.0, 0.0, 100);
							}
							break;
						case 49:
							if (Main.Rand.Next(20) == 0)
							{
								dustLocal.NewDust(i * 16 + 4, num6 * 16 - 4, 4, 4, 29, 0.0, 0.0, 100);
							}
							break;
						case 93:
							if (ptr4->FrameX == 0 && ptr4->FrameY == 0 && Main.Rand.Next(40) == 0)
							{
								dustLocal.NewDust(i * 16 + 4, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
							}
							break;
						case 98:
							if (ptr4->FrameX == 0 && ptr4->FrameY == 0 && Main.Rand.Next(40) == 0)
							{
								dustLocal.NewDust(i * 16 + 12, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
							}
							break;
						case 100:
							if (ptr4->FrameY != 0 || ptr4->FrameX >= 36 || Main.Rand.Next(40) != 0)
							{
								break;
							}
							if (ptr4->FrameX == 0)
							{
								if (Main.Rand.Next(3) == 0)
								{
									dustLocal.NewDust(i * 16 + 4, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
								}
								else
								{
									dustLocal.NewDust(i * 16 + 14, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
								}
							}
							else if (Main.Rand.Next(3) == 0)
							{
								dustLocal.NewDust(i * 16 + 6, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
							}
							else
							{
								dustLocal.NewDust(i * 16, num6 * 16 + 2, 4, 4, 6, 0.0, 0.0, 100);
							}
							break;
						case 61:
						{
							if (ptr4->FrameX != 144)
							{
								break;
							}
							if (Main.Rand.Next(60) == 0)
							{
								Dust* ptr7 = dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 44, 0.0, 0.0, 250, default(Color), 0.4f);
								if (ptr7 != null)
								{
									ptr7->FadeIn = 0.7f;
								}
							}
							byte b2 = (newColor.G = (byte)(245 - UI.MouseTextBrightness + (UI.MouseTextBrightness >> 1)));
							b2 = (newColor.B = b2);
							b2 = (newColor.R = b2);
							newColor.A = b2;
							break;
						}
						case 71:
						case 72:
							if (Main.Rand.Next(500) == 0)
							{
								dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 41, 0.0, 0.0, 250, default(Color), 0.8f);
							}
							break;
						case 17:
						case 77:
						case 133:
							if (Main.Rand.Next(40) == 0 && ptr4->FrameX == 18 && ptr4->FrameY == 18)
							{
								dustLocal.NewDust(i * 16 + 2, num6 * 16, 8, 6, 6, 0.0, 0.0, 100);
							}
							break;

#if !VERSION_INITIAL
						case 150:
							if (Main.Rand.Next(2) == 0 && ptr4->FrameY == 0)
							{
								Dust* ptrD = dustLocal.NewDust(i * 16, num6 * 16 - 4, 8, 8, 31, 0, 0, 100);
								if (ptrD != null)
								{
									if (ptr4->FrameX == 0)
									{
										ptrD->Position.X += Main.Rand.Next(8);
									}
									if (ptr4->FrameX == 36)
									{
										ptrD->Position.X -= Main.Rand.Next(8);
									}
									ptrD->Alpha += (short)Main.Rand.Next(100);
									ptrD->Velocity *= 0.2f;
									ptrD->Velocity.Y -= 0.5f + Main.Rand.Next(10) * 0.1f;
									ptrD->FadeIn = 0.5f + Main.Rand.Next(10) * 0.1f;
									if (Main.Rand.Next(4) == 0)
									{
										ptrD = dustLocal.NewDust(i * 16, num6 * 16, 8, 8, 6);
										if (ptrD != null)
										{
											if (ptr4->FrameX == 0)
											{
												ptrD->Position.X += Main.Rand.Next(8);
											}
											if (ptr4->FrameX == 36)
											{
												ptrD->Position.X -= Main.Rand.Next(8);
											}
											if (Main.Rand.Next(20) != 0)
											{
												ptrD->NoGravity = true;
												ptrD->Scale *= 1f + Main.Rand.Next(10) * 0.1f;
												ptrD->Velocity.Y -= 1f;
											}
										}
									}
								}
							}
							break;
#endif

						default:
						{
							if (Main.TileShine[num7] <= 0 || (newColor.R <= 20 && newColor.B <= 20 && newColor.G <= 20))
							{
								break;
							}
							int num10 = newColor.R;
							if (newColor.G > num10)
							{
								num10 = newColor.G;
							}
							if (newColor.B > num10)
							{
								num10 = newColor.B;
							}
							num10 /= 30;
							if (Main.Rand.Next(Main.TileShine[num7]) < num10 && (num7 != 21 || (ptr4->FrameX >= 36 && ptr4->FrameX < 180)))
							{
								Dust* ptr6 = dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 43, 0.0, 0.0, 254, default(Color), 0.5);
								if (ptr6 != null)
								{
									ptr6->Velocity.X = 0f;
									ptr6->Velocity.Y = 0f;
								}
							}
							break;
						}
						}
						if ((num7 == 5 && ptr4->FrameY >= 198 && ptr4->FrameX >= 22) || (num7 == 128 && ptr4->FrameX >= 100))
						{
							spec[num].X = (short)i;
							spec[num].Y = (short)num6;
							spec[num++].tileColor = newColor;
							if (num7 == 128)
							{
								s.X %= 100;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TILES_128, ref pos, ref s, newColor);
							}
						}
						else if (num7 == 129)
						{
							newColor.R = 200;
							newColor.G = 200;
							newColor.B = 200;
							newColor.A = 0;
							SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TILES_0 + num7, ref pos, ref s, newColor);
						}
						else
						{
							if (newColor.R <= 1 && newColor.G <= 1 && newColor.B <= 1)
							{
								continue;
							}
							if (num7 == 72 && ptr4->FrameX >= 36)
							{
								int num12 = ptr4->FrameY / 18;
								pos.X = i * 16 - ScreenPosition.X - 22 + 32;
								pos.Y = num6 * 16 - ScreenPosition.Y - 26 + 32;
								s.X = num12 * 62;
								s.Y = 0;
								s.Width = 60;
								s.Height = 42;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.SHROOM_TOPS, ref pos, ref s, newColor);
								continue;
							}
							if (num7 == 51)
							{
								newColor.R >>= 1;
								newColor.G >>= 1;
								newColor.B >>= 1;
								newColor.A >>= 1;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TILES_0 + num7, ref pos, ref s, newColor);
								continue;
							}
							if (num7 >= 82 && num7 <= 84)
							{
								if (num7 > 82)
								{
									int num13 = ptr4->FrameX / 18;
									if (num13 == 0 && WorldTime.DayTime)
									{
										num7 = 84;
									}
									else if (num13 == 1 && !WorldTime.DayTime)
									{
										num7 = 84;
									}
									else if (num13 == 3 && WorldTime.IsBloodMoon)
									{
										num7 = 84;
									}
									if (num7 == 84)
									{
										switch (num13)
										{
										case 0:
											if (Main.Rand.Next(100) == 0)
											{
												Dust* ptr9 = dustLocal.NewDust(i * 16, num6 * 16 - 4, 16, 16, 19, 0.0, 0.0, 160, default(Color), 0.1f);
												if (ptr9 != null)
												{
													ptr9->Velocity.X *= 0.5f;
													ptr9->Velocity.Y *= 0.5f;
													ptr9->NoGravity = true;
													ptr9->FadeIn = 1f;
												}
											}
											break;
										case 1:
											if (Main.Rand.Next(100) == 0)
											{
												dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 41, 0.0, 0.0, 250, default(Color), 0.8f);
											}
											break;
										case 3:
											if (Main.Rand.Next(200) == 0)
											{
												Dust* ptr10 = dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 14, 0.0, 0.0, 100, default(Color), 0.2f);
												if (ptr10 != null)
												{
													ptr10->FadeIn = 1.2f;
												}
											}
											if (Main.Rand.Next(75) == 0)
											{
												Dust* ptr11 = dustLocal.NewDust(i * 16, num6 * 16, 16, 16, 27, 0.0, 0.0, 100);
												if (ptr11 != null)
												{
													ptr11->Velocity.X *= 0.5f;
													ptr11->Velocity.Y *= 0.5f;
												}
											}
											break;
										case 4:
											if (Main.Rand.Next(150) == 0)
											{
												Dust* ptr12 = dustLocal.NewDust(i * 16, num6 * 16, 16, 8, 16);
												if (ptr12 != null)
												{
													ptr12->Velocity.X *= 0.333333343f;
													ptr12->Velocity.Y *= 0.333333343f;
													ptr12->Velocity.Y -= 0.7f;
													ptr12->Alpha = 50;
													ptr12->Scale *= 0.1f;
													ptr12->FadeIn = 0.9f;
													ptr12->NoGravity = true;
												}
											}
											break;
										case 5:
											if (Main.Rand.Next(40) == 0)
											{
												Dust* ptr8 = dustLocal.NewDust(i * 16, num6 * 16 - 6, 16, 16, 6, 0.0, 0.0, 0, default(Color), 1.5);
												if (ptr8 != null)
												{
													ptr8->Velocity.Y -= 2f;
													ptr8->NoGravity = true;
												}
											}
											newColor.A = (byte)(UI.MouseTextBrightness >> 1);
											newColor.G = UI.MouseTextBrightness;
											newColor.B = UI.MouseTextBrightness;
											break;
										}
									}
								}
								pos.Y = num6 * 16 - ScreenPosition.Y - 1 + 32;
								s.Height = 20;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TILES_0 + num7, ref pos, ref s, newColor);
								continue;
							}
							if (num7 == 80)
							{
								bool flag = false;
								bool flag2 = false;
								int num14 = i;
								switch (ptr4->FrameX)
								{
								case 36:
									num14--;
									break;
								case 54:
									num14++;
									break;
								case 108:
									num14 = ((ptr4->FrameY != 16) ? (num14 + 1) : (num14 - 1));
									break;
								}
								int num15 = num6;
								bool flag3 = false;
								if (Main.TileSet[num14, num15].Type == 80 && Main.TileSet[num14, num15].IsActive != 0)
								{
									flag3 = true;
								}
								while (Main.TileSet[num14, num15].IsActive == 0 || !Main.TileSolid[Main.TileSet[num14, num15].Type] || !flag3)
								{
									if (Main.TileSet[num14, num15].Type == 80 && Main.TileSet[num14, num15].IsActive != 0)
									{
										flag3 = true;
									}
									num15++;
									if (num15 > num6 + 20)
									{
										break;
									}
								}
								if (Main.TileSet[num14, num15].Type == 112)
								{
									flag = true;
								}
								else if (Main.TileSet[num14, num15].Type == 116)
								{
									flag2 = true;
								}
								int id = (flag ? (int)_sheetTiles.ID.EVIL_CACTUS : ((!flag2) ? ((int)_sheetTiles.ID.TILES_0 + num7) : (int)_sheetTiles.ID.GOOD_CACTUS));
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor);
								continue;
							}
							bool flag4 = Main.TileShine2[num7];
							int id2 = (int)_sheetTiles.ID.TILES_0 + num7;
							if (SMOOTH_LIGHT && Main.TileSolid[num7] && num7 != 137)
							{
								if (newColor.R > gfx || newColor.G > gfx * 1.1 || newColor.B > gfx * 1.2)
								{
									s.Width = 4;
									s.Height = 4;
									Color newColor2 = Lighting.GetColorUnsafe(i - 1, num6 - 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X += 4f;
									s.X += 4;
									s.Width = 8;
									newColor2 = Lighting.GetColorUnsafe(i, num6 - 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X += 8f;
									s.X += 8;
									s.Width = 4;
									newColor2 = Lighting.GetColorUnsafe(i + 1, num6 - 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.Y += 4f;
									s.Y += 4;
									s.Height = 8;
									newColor2 = Lighting.GetColorUnsafe(i + 1, num6);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X -= 12f;
									s.X -= 12;
									newColor2 = Lighting.GetColorUnsafe(i - 1, num6);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.Y += 8f;
									s.Y += 8;
									s.Height = 4;
									newColor2 = Lighting.GetColorUnsafe(i - 1, num6 + 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X += 4f;
									s.X += 4;
									s.Width = 8;
									newColor2 = Lighting.GetColorUnsafe(i, num6 + 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X += 8f;
									s.X += 8;
									s.Width = 4;
									newColor2 = Lighting.GetColorUnsafe(i + 1, num6 + 1);
									newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
									newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
									newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
									if (flag4)
									{
										shine(ref newColor2, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor2);
									pos.X -= 8f;
									pos.Y -= 8f;
									s.X -= 8;
									s.Y -= 8;
									s.Width = 8;
									s.Height = 8;
									if (flag4)
									{
										shine(ref newColor, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor);
								}
								else if (newColor.R > gfx2 || newColor.G > gfx2 * 1.1 || newColor.B > gfx2 * 1.2)
								{
									s.Width = 8;
									s.Height = 8;
									Color newColor3 = Lighting.GetColorUnsafe(i - 1, num6 - 1);
									newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
									newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
									newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
									if (flag4)
									{
										shine(ref newColor3, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor3);
									pos.X += 8f;
									s.X += 8;
									newColor3 = Lighting.GetColorUnsafe(i + 1, num6 - 1);
									newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
									newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
									newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
									if (flag4)
									{
										shine(ref newColor3, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor3);
									pos.Y += 8f;
									s.Y += 8;
									newColor3 = Lighting.GetColorUnsafe(i + 1, num6 + 1);
									newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
									newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
									newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
									if (flag4)
									{
										shine(ref newColor3, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor3);
									pos.X -= 8f;
									s.X -= 8;
									newColor3 = Lighting.GetColorUnsafe(i - 1, num6 + 1);
									newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
									newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
									newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
									if (flag4)
									{
										shine(ref newColor3, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor3);
								}
								else
								{
									if (flag4)
									{
										shine(ref newColor, num7);
									}
									SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor);
								}
								continue;
							}
							if (SMOOTH_LIGHT && flag4)
							{
								if (num7 == 21)
								{
									if (ptr4->FrameX >= 36 && ptr4->FrameX < 178)
									{
										shine(ref newColor, num7);
									}
								}
								else
								{
									shine(ref newColor, num7);
								}
							}
							SpriteSheet<_sheetTiles>.Draw(id2, ref pos, ref s, newColor);
							switch (num7)
							{
							case 139:
								newColor.R = 200;
								newColor.G = 200;
								newColor.B = 200;
								newColor.A = 0;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.MUSIC_BOX, ref pos, ref s, newColor);
								// BUG: The draw function here is for the glowmask for each music box; if active, it draws a sprite, and it draws blank if inactive.
								// The problem? It does not account for the highlighted and selected boxes as unlike other tiles of their type, they are vertically separated, not horizontally.
								// This changes in 1.02+, however this leads to the OG music boxes having fucked highlighted and selected sprites since this attempts to draw the glowmask for those notes using the highlighted/selected locations.
								// It is even worse for the exclusive music boxes since there isn't even a glowmask sprite for them, meaning all sprites for those placed music boxes are ruined.
								// What could fix it? Changing the source rectangle to always reflect the original dimensions, even if highlighted/selected, and then add glowmask sprites for the exclusive music boxes.
								break;
							case 144:
								newColor.R = 200;
								newColor.G = 200;
								newColor.B = 200;
								newColor.A = 0;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TIMER, ref pos, ref s, newColor);
								// These are also buggered.
								break;
							}
						}
					}
					while (++num6 < num5);
				}
				while (++i < num4);
				if (!Player.IsDead)
				{
					int num16 = (Player.XYWH.X + (Player.width / 2) >> 4) - 10;
					int num17 = (Player.XYWH.Y + (Player.height / 2) >> 4) - 8;
					for (i = -3; i < 23; i++)
					{
						Tile* ptr13 = ptr + ((num16 + i) * (Main.LargeWorldH) + num17 - 4);
						for (int k = -4; k < 20; k++)
						{
							ptr13->CurrentFlags &= ~Flags.HIGHLIGHT_MASK;
							ptr13++;
						}
					}
				}
			}
			Vector2 pos2 = default(Vector2);
			for (int l = 0; l < num; l++)
			{
				int x = spec[l].X;
				int y = spec[l].Y;
				Color tileColor = spec[l].tileColor;
				pos2.X = 32 + x * 16 - ScreenPosition.X;
				pos2.Y = 32 + y * 16 - ScreenPosition.Y;
				fixed (Tile* ptr14 = &Main.TileSet[x, y])
				{
					if (ptr14->Type == 128)
					{
						int num18 = ptr14->FrameY / 18;
						int frameX = ptr14->FrameX;
						int num19 = frameX / 100;
						frameX %= 100;
						SpriteEffects se = SpriteEffects.FlipHorizontally;
						if (frameX >= 36)
						{
							se = SpriteEffects.None;
						}
						pos2.X += -4f;
						pos2.Y += -12 - (num18 << 4);
						int sh = 54;
						switch (num18)
						{
						case 0:
							num19 += 60;
							sh = 36;
							break;
						case 1:
							num19 += 32;
							break;
						default:
							num19 += 107;
							break;
						}
						SpriteSheet<_sheetSprites>.Draw(num19, ref pos2, sh, tileColor, se);
						continue;
					}
					Rectangle s2 = default(Rectangle);
					if (ptr14->FrameX == 22)
					{
						int num20 = 0;
						if (ptr14->FrameY == 220)
						{
							num20 = 1;
						}
						else if (ptr14->FrameY == 242)
						{
							num20 = 2;
						}
						int num21 = 0;
						s2.Width = 80;
						s2.Height = 80;
						int num22 = 32;
						int num23 = ((y + 100 >= Main.MaxTilesY) ? (Main.MaxTilesY - y) : 100);
						for (int m = 0; m < num23; m++)
						{
							switch (ptr14[m].Type)
							{
							case 2:
								num21 = 0;
								break;
							case 23:
								num21 = 1;
								break;
							case 60:
								num21 = 2;
								s2.Width = 114;
								s2.Height = 96;
								num22 = 48;
								break;
							case 147:
								num21 = 4;
								break;
							case 109:
								num21 = 3;
								num20 += x % 3 * 3;
								s2.Height = 140;
								break;
							default:
								continue;
							}
							break;
						}
						s2.X = num20 * (s2.Width + 2);
						pos2.X -= num22;
						pos2.Y -= s2.Height - 16;
						SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TREE_TOPS_0 + num21, ref pos2, ref s2, tileColor);
					}
					else if (ptr14->FrameX == 44)
					{
						s2.Width = 40;
						s2.Height = 40;
						if (ptr14->FrameY == 220)
						{
							s2.Y = 42;
						}
						else if (ptr14->FrameY == 242)
						{
							s2.Y = 84;
						}
						int num24 = 0;
						int num25 = ((y + 100 >= Main.MaxTilesY) ? (Main.MaxTilesY - y) : 100);
						for (int n = 0; n < num25; n++)
						{
							switch (ptr14[n + (Main.LargeWorldH)].Type)
							{
							case 2:
								num24 = 0;
								break;
							case 23:
								num24 = 1;
								break;
							case 60:
								num24 = 2;
								break;
							case 147:
								num24 = 4;
								break;
							case 109:
								num24 = 3;
								s2.Y += x % 3 * 126;
								break;
							default:
								continue;
							}
							break;
						}
						pos2.X -= 24f;
						pos2.Y -= 12f;
						SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TREE_BRANCHES_0 + num24, ref pos2, ref s2, tileColor);
					}
					else
					{
						if (ptr14->FrameX != 66)
						{
							continue;
						}
						s2.X = 42;
						s2.Width = 40;
						s2.Height = 40;
						if (ptr14->FrameY == 220)
						{
							s2.Y = 42;
						}
						else if (ptr14->FrameY == 242)
						{
							s2.Y = 84;
						}
						int num26 = 0;
						int num27 = ((y + 100 >= Main.MaxTilesY) ? (Main.MaxTilesY - y) : 100);
						for (int num28 = 0; num28 < num27; num28++)
						{
							switch (ptr14[num28 - (Main.LargeWorldH)].Type)
							{
							case 2:
								num26 = 0;
								break;
							case 23:
								num26 = 1;
								break;
							case 60:
								num26 = 2;
								break;
							case 147:
								num26 = 4;
								break;
							case 109:
								num26 = 3;
								s2.Y += x % 3 * 126;
								break;
							default:
								continue;
							}
							break;
						}
						pos2.Y -= 12f;
						SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.TREE_BRANCHES_0 + num26, ref pos2, ref s2, tileColor);
						continue;
					}
				}
			}
			Main.TileSolid[10] = true;
		}

		private unsafe void DrawSolidTiles()
		{
			int gfx = (int)(255f * (1f - gfxQuality) + 30f * gfxQuality);
			int gfx2 = (int)(50f * (1f - gfxQuality) + 2f * gfxQuality);
			Main.TileSolid[10] = false;
			Rectangle s = default(Rectangle);
			Vector2 pos = default(Vector2);
			int num = LastTileX;
			int num2 = LastTileY;
			int num3 = FirstTileX;
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num4 = FirstTileY;
					Tile* ptr2 = ptr + (num3 * (Main.LargeWorldH) + num4 - 1);
					do
					{
						ptr2++;
						if (ptr2->IsActive == 0)
						{
							continue;
						}
						int type = ptr2->Type;
						if (!Main.TileSolid[type])
						{
							continue;
						}
						s.X = ptr2->FrameX;
						s.Y = ptr2->FrameY;
						s.Width = 16;
						s.Height = ((type == 137 || type == 138) ? 18 : 16);
						pos.X = num3 * 16 - ScreenPosition.X + 32;
						pos.Y = num4 * 16 - ScreenPosition.Y + 32;
						Color newColor = Lighting.GetColorUnsafe(num3, num4);
						if (Player.findTreasure)
						{
							switch (type)
							{
							case 6:
							case 7:
							case 8:
							case 9:
							case 22:
							case 63:
							case 64:
							case 65:
							case 66:
							case 67:
							case 68:
							case 107:
							case 108:
							case 111:
								if (newColor.R < UI.MouseTextBrightness >> 1)
								{
									newColor.R = (byte)(UI.MouseTextBrightness >> 1);
								}
								if (newColor.G < 70)
								{
									newColor.G = 70;
								}
								if (newColor.B < 210)
								{
									newColor.B = 210;
								}
								newColor.A = UI.MouseTextBrightness;
								if (Main.Rand.Next(150) == 0)
								{
									Dust* ptr3 = dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 15, 0.0, 0.0, 150, default(Color), 0.8f);
									if (ptr3 != null)
									{
										ptr3->Velocity.X *= 0.1f;
										ptr3->Velocity.Y *= 0.1f;
										ptr3->NoLight = true;
									}
								}
								break;
							}
						}
						switch (type)
						{
						case 22:
							if (Main.Rand.Next(400) == 0)
							{
								dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 14);
							}
							goto default;
						case 25:
							if (Main.Rand.Next(700) == 0)
							{
								dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 14);
							}
							break;
						case 23:
							if (Main.Rand.Next(500) == 0)
							{
								dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 14);
							}
							break;
						case 37:
							if (Main.Rand.Next(250) == 0)
							{
								Dust* ptr6 = dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 6, 0.0, 0.0, 0, default(Color), Main.Rand.Next(3));
								if (ptr6 != null && ptr6->Scale > 1f)
								{
									ptr6->NoGravity = true;
								}
							}
							break;
						case 58:
						case 76:
							if (Main.Rand.Next(250) == 0)
							{
								Dust* ptr4 = dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 6, 0.0, 0.0, 0, default(Color), Main.Rand.Next(3));
								if (ptr4 != null && ptr4->Scale > 1f)
								{
									ptr4->NoGravity = true;
									ptr4->NoLight = true;
								}
							}
							break;
						case 112:
							if (Main.Rand.Next(700) == 0)
							{
								dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 14);
							}
							break;
						default:
						{
							if (Main.TileShine[type] <= 0 || (newColor.R <= 20 && newColor.B <= 20 && newColor.G <= 20))
							{
								break;
							}
							int num5 = newColor.R;
							if (newColor.G > num5)
							{
								num5 = newColor.G;
							}
							if (newColor.B > num5)
							{
								num5 = newColor.B;
							}
							num5 /= 30;
							if (Main.Rand.Next(Main.TileShine[type]) < num5)
							{
								Dust* ptr5 = dustLocal.NewDust(num3 * 16, num4 * 16, 16, 16, 43, 0.0, 0.0, 254, default(Color), 0.5);
								if (ptr5 != null)
								{
									ptr5->Velocity.X = 0f;
									ptr5->Velocity.Y = 0f;
								}
							}
							break;
						}
						}
						if (newColor.R <= 1 && newColor.G <= 1 && newColor.B <= 1)
						{
							continue;
						}
						if (!Main.TileSolidTop[type] && (ptr2[-1].Liquid > 0 || ptr2[1].Liquid > 0 || ptr2[-(Main.LargeWorldH)].Liquid > 0 || ptr2[(Main.LargeWorldH)].Liquid > 0))
						{
							int num6 = 0;
							bool flag = false;
							bool flag2 = false;
							bool flag3 = false;
							bool flag4 = false;
							int num7 = 0;
							bool flag5 = false;
							if (ptr2[-(Main.LargeWorldH)].Liquid > num6)
							{
								num6 = ptr2[-(Main.LargeWorldH)].Liquid;
								flag = true;
							}
							else if (ptr2[-(Main.LargeWorldH)].Liquid > 0)
							{
								flag = true;
							}
							if (ptr2[(Main.LargeWorldH)].Liquid > num6)
							{
								num6 = ptr2[(Main.LargeWorldH)].Liquid;
								flag2 = true;
							}
							else if (ptr2[(Main.LargeWorldH)].Liquid > 0)
							{
								num6 = ptr2[(Main.LargeWorldH)].Liquid;
								flag2 = true;
							}
							if (ptr2[-1].Liquid > 0)
							{
								flag3 = true;
							}
							if (ptr2[1].Liquid > 240)
							{
								flag4 = true;
							}
							if (ptr2[-(Main.LargeWorldH)].Liquid > 0)
							{
								if (ptr2[-(Main.LargeWorldH)].Lava != 0)
								{
									num7 = 1;
								}
								else
								{
									flag5 = true;
								}
							}
							if (ptr2[(Main.LargeWorldH)].Liquid > 0)
							{
								if (ptr2[(Main.LargeWorldH)].Lava != 0)
								{
									num7 = 1;
								}
								else
								{
									flag5 = true;
								}
							}
							if (ptr2[-1].Liquid > 0)
							{
								if (ptr2[-1].Lava != 0)
								{
									num7 = 1;
								}
								else
								{
									flag5 = true;
								}
							}
							if (ptr2[1].Liquid > 0)
							{
								if (ptr2[1].Lava != 0)
								{
									num7 = 1;
								}
								else
								{
									flag5 = true;
								}
							}
							if (!flag5 || num7 != 1)
							{
								Vector2 pos2 = new Vector2(num3 * 16, num4 * 16);
								Rectangle s2 = new Rectangle(0, 4, 16, 16);
								if (flag4 && (flag || flag2))
								{
									flag = true;
									flag2 = true;
								}
								if ((!flag3 || (!flag && !flag2)) && (!flag4 || !flag3))
								{
									if (flag3)
									{
										s2.Height = 4;
									}
									else if (flag4 && !flag && !flag2)
									{
										pos2.Y += 12f;
										s2.Height = 4;
									}
									else
									{
#if (!IS_PATCHED && VERSION_INITIAL)
										int num8 = (int)((256 - num6) * (1f / 32f)) << 1;
#else
                                        int num8 = 256 - num6 >> 4;
#endif
                                        pos2.Y += num8;
										s2.Height -= num8;
										if (!flag || !flag2)
										{
											s2.Width = 4;
											if (!flag)
											{
												pos2.X += 12f;
											}
										}
									}
								}
								Color c = newColor;
								if (num4 >= Main.WorldSurface)
								{
									c.R >>= 1;
									c.G >>= 1;
									c.B >>= 1;
									c.A >>= 1;
									if (num7 == 1)
									{
										c.R += (byte)(c.R >> 1);
										c.G += (byte)(c.G >> 1);
										c.B += (byte)(c.B >> 1);
										c.A += (byte)(c.A >> 1);
									}
								}
								pos2.X -= ScreenPosition.X;
								pos2.X += 32f;
								pos2.Y -= ScreenPosition.Y;
								pos2.Y += 32f;
								SpriteSheet<_sheetTiles>.Draw((int)_sheetTiles.ID.LIQUID_0 + num7, ref pos2, ref s2, c);
							}
						}
						bool flag6 = Main.TileShine2[type];
						int id = (int)_sheetTiles.ID.TILES_0 + type;
						if (SMOOTH_LIGHT && type != 11 && type != 137)
						{
							if (newColor.R > gfx || newColor.G > gfx * 1.1 || newColor.B > gfx * 1.2)
							{
								s.Width = 4;
								s.Height = 4;
								Color newColor2 = Lighting.GetColorUnsafe(num3 - 1, num4 - 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X += 4f;
								s.X += 4;
								s.Width = 8;
								newColor2 = Lighting.GetColorUnsafe(num3, num4 - 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X += 8f;
								s.X += 8;
								s.Width = 4;
								newColor2 = Lighting.GetColorUnsafe(num3 + 1, num4 - 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.Y += 4f;
								s.Y += 4;
								s.Height = 8;
								newColor2 = Lighting.GetColorUnsafe(num3 + 1, num4);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X -= 12f;
								s.X -= 12;
								newColor2 = Lighting.GetColorUnsafe(num3 - 1, num4);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.Y += 8f;
								s.Y += 8;
								s.Height = 4;
								newColor2 = Lighting.GetColorUnsafe(num3 - 1, num4 + 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X += 4f;
								s.X += 4;
								s.Width = 8;
								newColor2 = Lighting.GetColorUnsafe(num3, num4 + 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X += 8f;
								s.X += 8;
								s.Width = 4;
								newColor2 = Lighting.GetColorUnsafe(num3 + 1, num4 + 1);
								newColor2.R = (byte)(newColor.R + newColor2.R >> 1);
								newColor2.G = (byte)(newColor.G + newColor2.G >> 1);
								newColor2.B = (byte)(newColor.B + newColor2.B >> 1);
								if (flag6)
								{
									shine(ref newColor2, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor2);
								pos.X -= 8f;
								pos.Y -= 8f;
								s.X -= 8;
								s.Y -= 8;
								s.Width = 8;
								s.Height = 8;
								if (flag6)
								{
									shine(ref newColor, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor);
							}
							else if (newColor.R > gfx2 || newColor.G > gfx2 * 1.1 || newColor.B > gfx2 * 1.2)
							{
								s.Width = 8;
								s.Height = 8;
								Color newColor3 = Lighting.GetColorUnsafe(num3 - 1, num4 - 1);
								newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
								newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
								newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
								if (flag6)
								{
									shine(ref newColor3, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor3);
								pos.X += 8f;
								s.X += 8;
								newColor3 = Lighting.GetColorUnsafe(num3 + 1, num4 - 1);
								newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
								newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
								newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
								if (flag6)
								{
									shine(ref newColor3, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor3);
								pos.Y += 8f;
								s.Y += 8;
								newColor3 = Lighting.GetColorUnsafe(num3 + 1, num4 + 1);
								newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
								newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
								newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
								if (flag6)
								{
									shine(ref newColor3, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor3);
								pos.X -= 8f;
								s.X -= 8;
								newColor3 = Lighting.GetColorUnsafe(num3 - 1, num4 + 1);
								newColor3.R = (byte)(newColor.R + newColor3.R >> 1);
								newColor3.G = (byte)(newColor.G + newColor3.G >> 1);
								newColor3.B = (byte)(newColor.B + newColor3.B >> 1);
								if (flag6)
								{
									shine(ref newColor3, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor3);
							}
							else
							{
								if (flag6)
								{
									shine(ref newColor, type);
								}
								SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor);
							}
						}
						else
						{
							if (SMOOTH_LIGHT && flag6)
							{
								shine(ref newColor, type);
							}
							SpriteSheet<_sheetTiles>.Draw(id, ref pos, ref s, newColor);
						}
					}
					while (++num4 < num2);
				}
				while (++num3 < num);
			}
			Main.TileSolid[10] = true;
		}

		private unsafe void DrawWater(bool bg = false)
		{
			int gfx = (int)(255f * (1f - gfxQuality) + 40f * gfxQuality);
			int gfx2 = (int)(255f * (1f - gfxQuality) + 140f * gfxQuality);
			int num = EvilTiles / 7;
			if (num > 50)
			{
				num = 50;
			}
			int num2 = 256 - num;
			int num3 = 256 - (num << 1);
			int num4 = FirstTileX;
			int num5 = LastTileX;
			int num6 = FirstTileY;
			int num7 = LastTileY;
			if (num4 < 5)
			{
				num4 = 5;
			}
			if (num6 < 5)
			{
				num6 = 5;
			}
			if (num5 > Main.MaxTilesX - 5)
			{
				num5 = Main.MaxTilesX - 5;
			}
			if (num7 > Main.MaxTilesY - 5)
			{
				num7 = Main.MaxTilesY - 5;
			}
			Vector2 pos = default;
			Vector2 vector = default;
			Rectangle s = default;
			fixed (Tile* ptr = Main.TileSet)
			{
				do
				{
					int num8 = num6;
					Tile* ptr2 = ptr + (num4 * (Main.LargeWorldH) + num8);
					vector.X = num4 << 4;
					do
					{
						int liquid = ptr2->Liquid;
						if (liquid > 0 && (bg || ptr2->IsActive == 0 || !Main.TileSolidNotSolidTop[ptr2->Type]) && (bg || Lighting.IsNotBlackUnsafe(num4, num8)))
						{
							Color colorUnsafe = Lighting.GetColorUnsafe(num4, num8);
							int num9 = ((ptr2->Lava == 0) ? (int)_sheetTiles.ID.LIQUID_0 : (int)_sheetTiles.ID.LIQUID_1);
							int num10 = (bg ? 255 : ((num9 == (int)_sheetTiles.ID.LIQUID_1) ? 230 : 128));
							vector.Y = num8 << 4;
							s.Width = 16;
							ptr2--;
							if (ptr2->Liquid == 0)
							{
								s.Y = 0;
								s.Height = Math.Max(1, liquid >> 4);
								vector.Y += 16 - s.Height;
							}
							else
							{
								s.Y = 4;
#if !VERSION_INITIAL
								s.Y += 2;
#endif
								s.Height = 16;
							}
							ptr2++;

#if !VERSION_INITIAL
							if ((colorUnsafe.R > 20 || colorUnsafe.B > 20 || colorUnsafe.G > 20) && s.Y == 0)
							{
								int num11 = colorUnsafe.R;
								if (colorUnsafe.G > num11)
								{
									num11 = colorUnsafe.G;
								}
								if (colorUnsafe.B > num11)
								{
									num11 = colorUnsafe.B;
								}
								num11 /= 30;
								if (Main.Rand.Next(20000) < num11)
								{
									Color newColor = Color.White;
									// Honey check goes here but since this is still essentially 1.1.2, no honey.
									Dust* ptr12 = dustLocal.NewDust(num4 << 4, (int)(vector.Y - 2f), 16, 8, 43, 0, 0, 254, newColor, 0.75f);
									if (ptr12 != null)
									{
										ptr12->Velocity *= 0f;
									}
								}
							}
#endif

							if (num9 == (int)_sheetTiles.ID.LIQUID_1 && dustLocal.LavaBubbles < 128 && Main.HasFocus)
							{
								if (liquid > 200 && Main.Rand.Next(700) == 0)
								{
									dustLocal.NewDust(num4 << 4, num8 << 4, 16, 16, 35);
								}
								else if (s.Y == 0 && Main.Rand.Next(350) == 0)
								{
									Dust* ptr3 = dustLocal.NewDust(num4 << 4, (num8 << 4) + (liquid >> 4) - 8, 16, 8, 35, 0.0, 0.0, 50, default(Color), 1.5);
									if (ptr3 != null)
									{
										ptr3->Velocity.X *= 1.6f;
										ptr3->Velocity.Y *= 0.8f;
										ptr3->Velocity.Y -= Main.Rand.Next(1, 7) * 0.1f;
										if (Main.Rand.Next(10) == 0)
										{
											ptr3->Velocity.Y *= Main.Rand.Next(2, 5);
										}
										ptr3->NoGravity = true;
									}
								}
							}
							colorUnsafe.R = (byte)(colorUnsafe.R * num10 >> 8);
							colorUnsafe.G = (byte)(colorUnsafe.G * num10 >> 8);
							colorUnsafe.B = (byte)(colorUnsafe.B * num10 >> 8);
							colorUnsafe.A = (byte)num10;
							if (num9 == (int)_sheetTiles.ID.LIQUID_0)
							{
								colorUnsafe.B = (byte)(colorUnsafe.B * num3 >> 8);
							}
							else
							{
								colorUnsafe.R = (byte)(colorUnsafe.R * num2 >> 8);
							}
							if (SMOOTH_LIGHT && !bg)
							{
								Color color = colorUnsafe;
								if ((num9 == (int)_sheetTiles.ID.LIQUID_0 && (color.R > gfx || color.G > gfx * 1.1 || color.B > gfx * 1.2)) || color.R > gfx2 || color.G > gfx2 * 1.1 || color.B > gfx2 * 1.2)
								{
									for (int i = 0; i < 4; i++)
									{
										int num11 = 0;
										int num12 = 0;
										int width = 8;
										int height = 8;
										Color colorUnsafe2 = Lighting.GetColorUnsafe(num4, num8);
										switch (i)
										{
										case 0:
											if (Lighting.Brighter(num4, num8 - 1, num4 - 1, num8))
											{
												ptr2 -= (Main.LargeWorldH);
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4 - 1, num8);
												}
												ptr2 += (Main.LargeWorldH);
											}
											else
											{
												ptr2--;
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4, num8 - 1);
												}
												ptr2++;
											}
											if (s.Height < 8)
											{
												height = s.Height;
											}
											break;
										case 1:
											if (Lighting.Brighter(num4, num8 - 1, num4 + 1, num8))
											{
												if (ptr2[(Main.LargeWorldH)].IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4 + 1, num8);
												}
											}
											else
											{
												ptr2--;
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4, num8 - 1);
												}
												ptr2++;
											}
											num11 = 8;
											if (s.Height < 8)
											{
												height = s.Height;
											}
											break;
										case 2:
											if (Lighting.Brighter(num4, num8 + 1, num4 - 1, num8))
											{
												ptr2 -= (Main.LargeWorldH);
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4 - 1, num8);
												}
												ptr2 += (Main.LargeWorldH);
											}
											else
											{
												ptr2++;
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4, num8 + 1);
												}
												ptr2--;
											}
											num12 = 8;
											height = 8 - (16 - s.Height);
											break;
										default:
											if (Lighting.Brighter(num4, num8 + 1, num4 + 1, num8))
											{
												if (ptr2[(Main.LargeWorldH)].IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4 + 1, num8);
												}
											}
											else
											{
												ptr2++;
												if (ptr2->IsActive == 0)
												{
													colorUnsafe2 = Lighting.GetColorUnsafe(num4, num8 + 1);
												}
												ptr2--;
											}
											num11 = 8;
											num12 = 8;
											height = 8 - (16 - s.Height);
											break;
										}
										colorUnsafe2.R = (byte)(colorUnsafe2.R * num10 + color.R >> 9);
										colorUnsafe2.G = (byte)(colorUnsafe2.G * num10 + color.G >> 9);
										colorUnsafe2.B = (byte)(colorUnsafe2.B * num10 + color.B >> 9);
										colorUnsafe2.A = (byte)num10;
										pos = vector;
										pos.X -= ScreenPosition.X - num11 - 32;
										pos.Y -= ScreenPosition.Y - num12 - 32;
										Rectangle s2 = s;
										s2.X += num11;
										s2.Y += num12;
										s2.Width = width;
										s2.Height = height;
										SpriteSheet<_sheetTiles>.Draw(num9, ref pos, ref s2, colorUnsafe2);
									}
								}
								else
								{
									pos = vector;
									pos.X -= ScreenPosition.X - 32;
									pos.Y -= ScreenPosition.Y - 32;
									SpriteSheet<_sheetTiles>.Draw(num9, ref pos, ref s, colorUnsafe);
								}
							}
							else
							{
								pos = vector;
								pos.X -= ScreenPosition.X - 32;
								pos.Y -= ScreenPosition.Y - 32;
#if VERSION_101
								if (s.Y == 0)
								{
									s.X = (int)((Main.LiquidFrame) * 16f);
								}
#endif
								SpriteSheet<_sheetTiles>.Draw(num9, ref pos, ref s, colorUnsafe);
							}
						}
						ptr2++;
					}
					while (++num8 < num7);
				}
				while (++num4 < num5);
			}
		}

		private void DrawGore()
		{
			Vector2 pivot = default;
			Vector2 pos = default;
			for (int i = 0; i < Gore.MaxNumGore; i++)
			{
				if (Main.GoreSet[i].Active != 0)
				{
					int num = (int)_sheetSprites.ID.GORE_0 + Main.GoreSet[i].Type;
					pivot.X = SpriteSheet<_sheetSprites>.Source[num].Width >> 1;
					pivot.Y = SpriteSheet<_sheetSprites>.Source[num].Height >> 1;
					pos.X = Main.GoreSet[i].Position.X + pivot.X;
					pos.Y = Main.GoreSet[i].Position.Y + pivot.Y;
					Color alpha = Main.GoreSet[i].GetAlpha(Lighting.GetColor((int)pos.X >> 4, (int)pos.Y >> 4));
					pos.X -= ScreenPosition.X;
					pos.Y -= ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.Draw(num, ref pos, alpha, Main.GoreSet[i].Rotation, ref pivot, Main.GoreSet[i].Scale);
				}
			}
		}

		private unsafe void DrawNPCs(bool behindTiles = false)
		{
			bool flag = false;
			Rectangle rectangle = default;
			rectangle.X = ScreenPosition.X - 300;
			rectangle.Y = ScreenPosition.Y - 300;
			rectangle.Width = ViewWidth + 600;

#if USE_ORIGINAL_CODE
			rectangle.Height = 1140;
#else
			rectangle.Height = Main.ResolutionHeight + 600;
#endif

			Vector2 pos = default;
			Color c = default;
			for (int num = NPC.MaxNumNPCs - 1; num >= 0; num--)
			{
				int type = Main.NPCSet[num].Type;
				if (type <= 0 || Main.NPCSet[num].Active == 0 || Main.NPCSet[num].IsBehindTiles != behindTiles)
				{
					continue;
				}
				if ((type == (int)NPC.ID.RETINAZER || type == (int)NPC.ID.SPAZMATISM) && !flag)
				{
					flag = true;
					for (int i = 0; i < NPC.MaxNumNPCs; i++)
					{
						if (Main.NPCSet[i].Active == 0 || num == i || (Main.NPCSet[i].Type != (int)NPC.ID.RETINAZER && Main.NPCSet[i].Type != (int)NPC.ID.SPAZMATISM))
						{
							continue;
						}
						float num2 = Main.NPCSet[i].Position.X + (Main.NPCSet[i].Width >> 1);
						float num3 = Main.NPCSet[i].Position.Y + (Main.NPCSet[i].Height >> 1);
						Vector2 vector = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1), Main.NPCSet[num].Position.Y + (Main.NPCSet[num].Height >> 1));
						float num4 = num2 - vector.X;
						float num5 = num3 - vector.Y;
						float rotCenter = (float)Math.Atan2(num5, num4) - 1.57f;
						bool flag2 = true;
						float num6 = num4 * num4 + num5 * num5;
						if (num6 > 4000000f)
						{
							flag2 = false;
						}
						while (flag2)
						{
							num6 = num4 * num4 + num5 * num5;
							if (num6 < 1600f)
							{
								flag2 = false;
								continue;
							}
							float num7 = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.CHAIN12].Height / (float)Math.Sqrt(num6);
							num4 *= num7;
							num5 *= num7;
							vector.X += num4;
							vector.Y += num5;
							num4 = num2 - vector.X;
							num5 = num3 - vector.Y;
							pos = vector;
							pos.X -= ScreenPosition.X;
							pos.Y -= ScreenPosition.Y;
							SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.CHAIN12, ref pos, Lighting.GetColor((int)vector.X >> 4, (int)vector.Y >> 4), rotCenter);
						}
					}
				}
				if (!rectangle.Intersects(Main.NPCSet[num].XYWH))
				{
					continue;
				}
				if (type == (int)NPC.ID.CLINGER)
				{
					bool flag3 = true;
					Vector2 vector2 = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1), Main.NPCSet[num].Position.Y + (Main.NPCSet[num].Height >> 1));
					float num8 = Main.NPCSet[num].AI0 * 16f + 8f - vector2.X;
					float num9 = Main.NPCSet[num].AI1 * 16f + 8f - vector2.Y;
					float rot = (float)Math.Atan2(num9, num8) - 1.57f;
					bool flag4 = true;
					do
					{
						float num10 = 0.75f;
						int sh = 28;
						float num11 = (float)Math.Sqrt(num8 * num8 + num9 * num9);
						if (num11 < 28f * num10)
						{
							sh = (int)num11 - 40 + 28;
							flag4 = false;
						}
						num11 = 20f * num10 / num11;
						num8 *= num11;
						num9 *= num11;
						vector2.X += num8;
						vector2.Y += num9;
						num8 = Main.NPCSet[num].AI0 * 16f + 8f - vector2.X;
						num9 = Main.NPCSet[num].AI1 * 16f + 8f - vector2.Y;
						pos = vector2;
						pos.X -= ScreenPosition.X;
						pos.Y -= ScreenPosition.Y;
						SpriteSheet<_sheetSprites>.Draw(flag3 ? (int)_sheetSprites.ID.CHAIN11 : (int)_sheetSprites.ID.CHAIN10, ref pos, sh, Lighting.GetColor((int)vector2.X >> 4, (int)vector2.Y >> 4), rot, num10);
						flag3 = !flag3;
					}
					while (flag4);
				}
				else if (Main.NPCSet[num].AIStyle == 13)
				{
					Vector2 vector3 = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1), Main.NPCSet[num].Position.Y + (Main.NPCSet[num].Height >> 1));
					float num12 = Main.NPCSet[num].AI0 * 16f + 8f - vector3.X;
					float num13 = Main.NPCSet[num].AI1 * 16f + 8f - vector3.Y;
					float rotCenter2 = (float)Math.Atan2(num13, num12) - 1.57f;
					bool flag5 = true;
					do
					{
						float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
						if (num14 < 40f)
						{
							flag5 = false;
						}
						num14 = 28f / num14;
						num12 *= num14;
						num13 *= num14;
						vector3.X += num12;
						vector3.Y += num13;
						num12 = Main.NPCSet[num].AI0 * 16f + 8f - vector3.X;
						num13 = Main.NPCSet[num].AI1 * 16f + 8f - vector3.Y;
						pos = vector3;
						pos.X -= ScreenPosition.X;
						pos.Y -= ScreenPosition.Y;
						SpriteSheet<_sheetSprites>.DrawRotated((type == 56) ? (int)_sheetSprites.ID.CHAIN5 : (int)_sheetSprites.ID.CHAIN4, ref pos, Lighting.GetColor((int)vector3.X >> 4, (int)vector3.Y >> 4), rotCenter2);
					}
					while (flag5);
				}
				if (type == (int)NPC.ID.SKELETRON_HAND)
				{
					Vector2 vector4 = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1) - 5f * Main.NPCSet[num].AI0, Main.NPCSet[num].Position.Y + 20f);
					for (int j = 0; j < 2; j++)
					{
						float num15 = Main.NPCSet[(int)Main.NPCSet[num].AI1].Position.X + (Main.NPCSet[(int)Main.NPCSet[num].AI1].Width >> 1) - vector4.X;
						float num16 = Main.NPCSet[(int)Main.NPCSet[num].AI1].Position.Y + (Main.NPCSet[(int)Main.NPCSet[num].AI1].Height >> 1) - vector4.Y;
						float num17;
						if (j == 0)
						{
							num15 -= 200f * Main.NPCSet[num].AI0;
							num16 += 130f;
							num17 = (float)Math.Sqrt(num15 * num15 + num16 * num16);
							num17 = 92f / num17;
						}
						else
						{
							num15 -= 50f * Main.NPCSet[num].AI0;
							num16 += 80f;
							num17 = (float)Math.Sqrt(num15 * num15 + num16 * num16);
							num17 = 60f / num17;
						}
						vector4.X += num15 * num17;
						vector4.Y += num16 * num17;
						pos.X = vector4.X - ScreenPosition.X;
						pos.Y = vector4.Y - ScreenPosition.Y;
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARM_BONE, ref pos, Lighting.GetColor((int)vector4.X >> 4, (int)vector4.Y >> 4), (float)Math.Atan2(num16, num15) - 1.57f);
						if (j == 0)
						{
							vector4.X += num15 * num17 * 0.5f;
							vector4.Y += num16 * num17 * 0.5f;
						}
						else if (Main.HasFocus)
						{
							vector4.X += num15 * num17 - 16f;
							vector4.Y += num16 * num17 - 6f;
							Dust* ptr = dustLocal.NewDust((int)vector4.X, (int)vector4.Y, 30, 10, 5, num15 * 0.02f, num16 * 0.02f, 0, default(Color), 2.0);
							if (ptr != null)
							{
								ptr->NoGravity = true;
							}
						}
					}
				}
				if (Main.NPCSet[num].AIStyle >= 33 && Main.NPCSet[num].AIStyle <= 36)
				{
					Vector2 vector5 = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1) - 5f * Main.NPCSet[num].AI0, Main.NPCSet[num].Position.Y + 20f);
					for (int k = 0; k < 2; k++)
					{
						float num18 = Main.NPCSet[(int)Main.NPCSet[num].AI1].Position.X + (Main.NPCSet[(int)Main.NPCSet[num].AI1].Width >> 1) - vector5.X;
						float num19 = Main.NPCSet[(int)Main.NPCSet[num].AI1].Position.Y + (Main.NPCSet[(int)Main.NPCSet[num].AI1].Height >> 1) - vector5.Y;
						float num20;
						if (k == 0)
						{
							num18 -= 200f * Main.NPCSet[num].AI0;
							num19 += 130f;
							num20 = (float)Math.Sqrt(num18 * num18 + num19 * num19);
							num20 = 92f / num20;
						}
						else
						{
							num18 -= 50f * Main.NPCSet[num].AI0;
							num19 += 80f;
							num20 = (float)Math.Sqrt(num18 * num18 + num19 * num19);
							num20 = 60f / num20;
						}
						vector5.X += num18 * num20;
						vector5.Y += num19 * num20;
						pos.X = vector5.X - ScreenPosition.X;
						pos.Y = vector5.Y - ScreenPosition.Y;
						SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ARM_BONE_2, ref pos, Lighting.GetColor((int)vector5.X >> 4, (int)vector5.Y >> 4), (float)Math.Atan2(num19, num18) - 1.57f);
						if (k == 0)
						{
							vector5.X += num18 * num20 * 0.5f;
							vector5.Y += num19 * num20 * 0.5f;
						}
						else if (Main.HasFocus)
						{
							vector5.X += num18 * num20 - 16f;
							vector5.Y += num19 * num20 - 6f;
							Dust* ptr2 = dustLocal.NewDust((int)vector5.X, (int)vector5.Y, 30, 10, 6, num18 * 0.02f, num19 * 0.02f, 0, default(Color), 2.5);
							if (ptr2 != null)
							{
								ptr2->NoGravity = true;
							}
						}
					}
				}
				else if (Main.NPCSet[num].AIStyle == 20)
				{
					Vector2 vector6 = new Vector2(Main.NPCSet[num].Position.X + (Main.NPCSet[num].Width >> 1), Main.NPCSet[num].Position.Y + (Main.NPCSet[num].Height >> 1));
					float num21 = Main.NPCSet[num].AI1 - vector6.X;
					float num22 = Main.NPCSet[num].AI2 - vector6.Y;
					float num23 = (float)Math.Atan2(num22, num21) - 1.57f;
					Main.NPCSet[num].Rotation = num23;
					bool flag6 = true;
					while (flag6)
					{
						int sh2 = 12;
						float num24 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
						if (num24 < 20f)
						{
							sh2 = (int)num24 - 20 + 12;
							flag6 = false;
						}
						num24 = 12f / num24;
						num21 *= num24;
						num22 *= num24;
						vector6.X += num21;
						vector6.Y += num22;
						num21 = Main.NPCSet[num].AI1 - vector6.X;
						num22 = Main.NPCSet[num].AI2 - vector6.Y;
						pos = vector6;
						pos.X -= ScreenPosition.X;
						pos.Y -= ScreenPosition.Y;
						SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHAIN, ref pos, sh2, Lighting.GetColor((int)vector6.X >> 4, (int)vector6.Y >> 4), num23);
					}
					pos.X = Main.NPCSet[num].AI1 - ScreenPosition.X;
					pos.Y = Main.NPCSet[num].AI2 - ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.SPIKE_BASE, ref pos, Lighting.GetColor((int)Main.NPCSet[num].AI1 >> 4, (int)Main.NPCSet[num].AI2 >> 4), num23 - 0.75f);
				}
				Color newColor = Lighting.GetColor(Main.NPCSet[num].XYWH.X + (Main.NPCSet[num].Width >> 1) >> 4, Main.NPCSet[num].XYWH.Y + (Main.NPCSet[num].Height >> 1) >> 4);
				if (behindTiles && type != (int)NPC.ID.WALL_OF_FLESH && type != (int)NPC.ID.WALL_OF_FLESH_EYE)
				{
					int num25 = Main.NPCSet[num].XYWH.X - 8 >> 4;
					int num26 = Main.NPCSet[num].XYWH.X + Main.NPCSet[num].Width + 8 >> 4;
					int num27 = Main.NPCSet[num].XYWH.Y - 8 >> 4;
					int num28 = Main.NPCSet[num].XYWH.Y + Main.NPCSet[num].Height + 8 >> 4;
					for (int l = num25; l <= num26; l++)
					{
						bool Shadowed = false;

						for (int num29 = num27; num29 <= num28; num29++)
						{
							if (Lighting.Brightness(l, num29) == 0f)
							{
								newColor.PackedValue = 0xFF000000;
								Shadowed = true;
								break;
							}
						}

						if (Shadowed)
						{
							break;
						}
					}
				}
				if (Main.NPCSet[num].IsPoisoned)
				{
					Player.buffColor(ref newColor, 0.65, 1.0, 0.75);
				}
				if (Player.detectCreature && Main.NPCSet[num].LifeMax > 1)
				{
					if (newColor.R < 150)
					{
						newColor.A = UI.MouseTextBrightness;
						if (newColor.R < 50)
						{
							newColor.R = 50;
						}
					}
					if (newColor.G < 200)
					{
						newColor.G = 200;
					}
					if (newColor.B < 100)
					{
						newColor.B = 100;
					}
					if (Main.HasFocus && Main.Rand.Next(52) == 0)
					{
						Dust* ptr3 = dustLocal.NewDust(Main.NPCSet[num].XYWH.X, Main.NPCSet[num].XYWH.Y, Main.NPCSet[num].Width, Main.NPCSet[num].Height, 15, 0.0, 0.0, 150, default(Color), 0.8f);
						if (ptr3 != null)
						{
							ptr3->Velocity.X *= 0.1f;
							ptr3->Velocity.Y *= 0.1f;
							ptr3->NoLight = true;
						}
					}
				}
				switch (type)
				{
				case (int)NPC.ID.KING_SLIME:
				{
					Vector2 vector8 = default(Vector2);
					vector8.Y = 0f - Main.NPCSet[num].Velocity.Y;
					vector8.X = Main.NPCSet[num].Velocity.X * -2f;
					float rotCenter4 = Main.NPCSet[num].Velocity.X * 0.05f;
					if (Main.NPCSet[num].FrameY == 120)
					{
						vector8.Y += 2f;
					}
					else if (Main.NPCSet[num].FrameY == 360)
					{
						vector8.Y -= 2f;
					}
					else if (Main.NPCSet[num].FrameY == 480)
					{
						vector8.Y -= 6f;
					}
					pos.X = Main.NPCSet[num].Position.X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) + vector8.X;
					pos.Y = Main.NPCSet[num].Position.Y - ScreenPosition.Y + (Main.NPCSet[num].Height >> 1) + vector8.Y;
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.NINJA, ref pos, newColor, rotCenter4);
					break;
				}
				case (int)NPC.ID.DUNGEON_SLIME:
				{
					Vector2 vector7 = default(Vector2);
					vector7.Y = Main.NPCSet[num].Velocity.Y * -0.3f;
					vector7.X = Main.NPCSet[num].Velocity.X * -0.6f;
					float rotCenter3 = Main.NPCSet[num].Velocity.X * 0.09f;
					if (Main.NPCSet[num].FrameY == 120)
					{
						vector7.Y += 2f;
					}
					else if (Main.NPCSet[num].FrameY == 360)
					{
						vector7.Y -= 2f;
					}
					else if (Main.NPCSet[num].FrameY == 480)
					{
						vector7.Y -= 6f;
					}
					pos.X = Main.NPCSet[num].Position.X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) + vector7.X;
					pos.Y = Main.NPCSet[num].Position.Y - ScreenPosition.Y + (Main.NPCSet[num].Height >> 1) + vector7.Y;
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.ITEM_327, ref pos, newColor, rotCenter3);
					break;
				}
				case (int)NPC.ID.ANTLION:
				case (int)NPC.ID.ALBINO_ANTLION:
				{
					pos.X = Main.NPCSet[num].Position.X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1);
					pos.Y = Main.NPCSet[num].Position.Y - ScreenPosition.Y + Main.NPCSet[num].Height + 14f;
					int id = ((type == (int)NPC.ID.ANTLION) ? (int)_sheetSprites.ID.ANTLIONBODY : (int)_sheetSprites.ID.ALBINOANTLIONBODY);
					SpriteSheet<_sheetSprites>.DrawRotated(id, ref pos, newColor, (0f - Main.NPCSet[num].Rotation) * 0.3f);
					break;
				}
				}
				float num30 = 0f;
				float num31 = 0f;
				int width = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.NPC_1 - 1 + type].Width;
				Vector2 pivot = default(Vector2);
				pivot.X = width >> 1;
				pivot.Y = Main.NPCSet[num].FrameHeight >> 1;
				switch ((NPC.ID)type)
				{
				case NPC.ID.WIZARD:
				case NPC.ID.MECHANIC:
					num30 = 2f;
					break;
				case NPC.ID.EYE_OF_CTHULHU:
					pivot.Y = 107f;
					break;
				case NPC.ID.OCRAM:
					// BUG: In all versions of Old-gen Console, Ocram has a major issue with his hitbox and sprite display.
					// The issue is that Ocram will rarely be aligned with his hitbox due to a variety of inconsistencies as the programmer copied a lot from the EoC.

					pivot.Y *= 0.5f;

					// Changing his dimensions, and the pivot value all go towards fixing this issue.
					// pivot.Y -= 5f;
					break;
				case NPC.ID.EATER_OF_SOULS:
				case NPC.ID.EATER_OF_WORLDS_HEAD:
				case NPC.ID.EATER_OF_WORLDS_BODY:
				case NPC.ID.EATER_OF_WORLDS_TAIL:
				case NPC.ID.BONE_SERPENT_HEAD:
				case NPC.ID.BONE_SERPENT_BODY:
				case NPC.ID.BONE_SERPENT_TAIL:
					num31 = 26f;
					break;
				case NPC.ID.GIANT_WORM_HEAD:
				case NPC.ID.GIANT_WORM_BODY:
				case NPC.ID.GIANT_WORM_TAIL:
					num31 = 8f;
					break;
				case NPC.ID.HARPY:
					num31 = 32f;
					break;
				case NPC.ID.CAVE_BAT:
				case NPC.ID.JUNGLE_BAT:
					num31 = 4f;
					break;
				case NPC.ID.HELLBAT:
					num31 = 10f;
					break;
				case NPC.ID.DEMON:
				case NPC.ID.SHARK:
				case NPC.ID.VOODOO_DEMON:
					num31 = 14f;
					break;
				case NPC.ID.BLUE_JELLYFISH:
				case NPC.ID.PINK_JELLYFISH:
				case NPC.ID.GREEN_JELLYFISH:
					num31 = 4f;
					pivot.Y += 4f;
					break;
				case NPC.ID.ANTLION:
					num31 = 4f;
					pivot.Y += 8f;
					break;
				case NPC.ID.SPIKE_BALL:
					num31 = -4f;
					break;
				case NPC.ID.BLAZING_WHEEL:
					num31 = -2f;
					break;
				case NPC.ID.CURSED_HAMMER:
				case NPC.ID.ENCHANTED_SWORD:
					num31 = 20f;
					break;
				case NPC.ID.DEVOURER_HEAD:
				case NPC.ID.DEVOURER_BODY:
				case NPC.ID.DEVOURER_TAIL:
				case NPC.ID.DIGGER_HEAD:
				case NPC.ID.DIGGER_BODY:
				case NPC.ID.DIGGER_TAIL:
				case NPC.ID.SEEKER_HEAD:
				case NPC.ID.SEEKER_BODY:
				case NPC.ID.SEEKER_TAIL:
					num31 = 13f;
					break;
				case NPC.ID.WYVERN_HEAD:
				case NPC.ID.WYVERN_LEGS:
				case NPC.ID.WYVERN_BODY1:
				case NPC.ID.WYVERN_BODY2:
				case NPC.ID.WYVERN_BODY3:
				case NPC.ID.WYVERN_TAIL:
				case NPC.ID.ARCH_WYVERN_HEAD:
				case NPC.ID.ARCH_WYVERN_LEGS:
				case NPC.ID.ARCH_WYVERN_BODY1:
				case NPC.ID.ARCH_WYVERN_BODY2:
				case NPC.ID.ARCH_WYVERN_BODY3:
				case NPC.ID.ARCH_WYVERN_TAIL:
					num31 = 56f;
					break;
				case NPC.ID.CORRUPTOR:
					num31 = 14f;
					break;
				case NPC.ID.RETINAZER:
				case NPC.ID.SPAZMATISM:
					pivot = new Vector2(55f, 107f);
					num31 = 30f;
					break;
				case NPC.ID.THE_DESTROYER_HEAD:
				case NPC.ID.THE_DESTROYER_BODY:
				case NPC.ID.THE_DESTROYER_TAIL:
					num31 = 30f;
					break;
				}
				num31 *= Main.NPCSet[num].Scale;
				pos = new Vector2(Main.NPCSet[num].Position.X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) - width * Main.NPCSet[num].Scale * 0.5f + pivot.X * Main.NPCSet[num].Scale, Main.NPCSet[num].Position.Y - ScreenPosition.Y + Main.NPCSet[num].Height - Main.NPCSet[num].FrameHeight * Main.NPCSet[num].Scale + 4f + pivot.Y * Main.NPCSet[num].Scale + num31 + num30);
				if (Main.NPCSet[num].AIStyle == 10 || type == 72)
				{
					newColor = Color.White;
				}
				SpriteEffects e = ((Main.NPCSet[num].SpriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				switch ((NPC.ID)type)
				{
				case NPC.ID.CURSED_HAMMER:
				case NPC.ID.ENCHANTED_SWORD:
				case NPC.ID.SHADOW_HAMMER:
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, Color.White, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					continue;
				default:
					if (type < (int)NPC.ID.ARCH_WYVERN_HEAD || type > (int)NPC.ID.ARCH_WYVERN_TAIL)
					{
						break;
					}
					goto case NPC.ID.WYVERN_HEAD;
				case NPC.ID.WYVERN_HEAD:
				case NPC.ID.WYVERN_LEGS:
				case NPC.ID.WYVERN_BODY1:
				case NPC.ID.WYVERN_BODY2:
				case NPC.ID.WYVERN_BODY3:
				case NPC.ID.WYVERN_TAIL:
					{
						c = Main.NPCSet[num].GetAlpha(newColor);
						byte b = (byte)((WorldTime.tileColor.R + WorldTime.tileColor.G + WorldTime.tileColor.B) / 3);
						if (c.R < b)
						{
							c.R = b;
						}
						if (c.G < b)
						{
							c.G = b;
						}
						if (c.B < b)
						{
							c.B = b;
						}
						SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						continue;
					}
				}
				switch ((NPC.ID)type)
				{
				case NPC.ID.CORRUPTOR:
					{
						for (int m = 1; m < 6; m += 2)
						{
							c = Main.NPCSet[num].GetAlpha(newColor);
							c.R = (byte)(c.R * (10 - m) / 15);
							c.G = (byte)(c.G * (10 - m) / 15);
							c.B = (byte)(c.B * (10 - m) / 15);
							c.A = (byte)(c.A * (10 - m) / 15);
							pos = new Vector2(Main.NPCSet[num].OldPos[m].X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) - width * Main.NPCSet[num].Scale * 0.5f + pivot.X * Main.NPCSet[num].Scale, Main.NPCSet[num].OldPos[m].Y - ScreenPosition.Y + Main.NPCSet[num].Height - Main.NPCSet[num].FrameHeight * Main.NPCSet[num].Scale + 4f + pivot.Y * Main.NPCSet[num].Scale + num31);
							SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						}
						break;
					}
				case NPC.ID.RETINAZER:
				case NPC.ID.SPAZMATISM:
				case NPC.ID.SKELETRON_PRIME:
				case NPC.ID.PRIME_CANNON:
				case NPC.ID.PRIME_SAW:
				case NPC.ID.PRIME_VICE:
				case NPC.ID.PRIME_LASER:
				case NPC.ID.PROBE:
				case NPC.ID.POSSESSED_ARMOR:
					{
						for (int num32 = 9; num32 >= 0; num32 -= 2)
						{
							c = Main.NPCSet[num].GetAlpha(newColor);
							c.R = (byte)(c.R * (10 - num32) / 20);
							c.G = (byte)(c.G * (10 - num32) / 20);
							c.B = (byte)(c.B * (10 - num32) / 20);
							c.A = (byte)(c.A * (10 - num32) / 20);
							pos = new Vector2(Main.NPCSet[num].OldPos[num32].X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) - width * Main.NPCSet[num].Scale * 0.5f + pivot.X * Main.NPCSet[num].Scale, Main.NPCSet[num].OldPos[num32].Y - ScreenPosition.Y + Main.NPCSet[num].Height - Main.NPCSet[num].FrameHeight * Main.NPCSet[num].Scale + 4f + pivot.Y * Main.NPCSet[num].Scale + num31);
							SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						}
						break;
					}
				}
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, Main.NPCSet[num].GetAlpha(newColor), Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
				if (Main.NPCSet[num].Colour.PackedValue != 0)
				{
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, Main.NPCSet[num].GetColor(newColor), Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
				}
				if (Main.NPCSet[num].IsConfused)
				{
					Vector2 pos2 = pos;
					pos2.Y -= SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.CONFUSE].Height + 20;
					c.PackedValue = 0x46FAFAFAu;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CONFUSE, ref pos2, c, Main.NPCSet[num].Velocity.X * -0.05f, UI.PulseScale + 0.2f);
				}
				switch ((NPC.ID)type)
				{
				case NPC.ID.RETINAZER:
					c.PackedValue = 0xFFFFFFu;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.EYE_LASER, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					continue;
				case NPC.ID.PROBE:
					c.PackedValue = 0xFFFFFFu;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.PROBE, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					continue;
				case NPC.ID.SKELETRON_PRIME:
					c.PackedValue = 0xC8C8C8u;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.BONE_EYES, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					continue;
				case NPC.ID.PRIME_LASER:
					c.PackedValue = 0xC8C8C8u;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.BONE_LASER, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					continue;
				case NPC.ID.CHAOS_ELEMENTAL:
				case NPC.ID.SPECTRAL_ELEMENTAL:
					{
						for (int n = 1; n < Main.NPCSet[num].OldPos.Length; n++)
						{
							c.R = (byte)(150 * (10 - n) / 15);
							c.G = (byte)(100 * (10 - n) / 15);
							c.B = (byte)(150 * (10 - n) / 15);
							c.A = (byte)(50 * (10 - n) / 15);
							pos = new Vector2(Main.NPCSet[num].OldPos[n].X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) - width * Main.NPCSet[num].Scale * 0.5f + pivot.X * Main.NPCSet[num].Scale, Main.NPCSet[num].OldPos[n].Y - ScreenPosition.Y + Main.NPCSet[num].Height - Main.NPCSet[num].FrameHeight * Main.NPCSet[num].Scale + 4f + pivot.Y * Main.NPCSet[num].Scale + num31);
							SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHAOS, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						}
						continue;
					}
				case NPC.ID.THE_DESTROYER_HEAD:
				case NPC.ID.THE_DESTROYER_BODY:
				case NPC.ID.THE_DESTROYER_TAIL:
					if (newColor.PackedValue != 0xFF000000)
					{
						c.PackedValue = 0xFFFFFFu;
						SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.DEST1 + type - (int)NPC.ID.THE_DESTROYER_HEAD, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
					}
					continue;
				}
				switch ((NPC.ID)type)
				{
				case NPC.ID.ILLUMINANT_BAT:
				case NPC.ID.ILLUMINANT_SLIME:
					{
						for (int num34 = 1; num34 < Main.NPCSet[num].OldPos.Length; num34++)
						{
							c.R = (byte)(150 * (10 - num34) / 15);
							c.G = (byte)(100 * (10 - num34) / 15);
							c.B = (byte)(150 * (10 - num34) / 15);
							c.A = (byte)(50 * (10 - num34) / 15);
							pos = new Vector2(Main.NPCSet[num].OldPos[num34].X - ScreenPosition.X + (Main.NPCSet[num].Width >> 1) - width * Main.NPCSet[num].Scale * 0.5f + pivot.X * Main.NPCSet[num].Scale, Main.NPCSet[num].OldPos[num34].Y - ScreenPosition.Y + Main.NPCSet[num].Height - Main.NPCSet[num].FrameHeight * Main.NPCSet[num].Scale + 4f + pivot.Y * Main.NPCSet[num].Scale + num31);
							SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.NPC_1 - 1 + type, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, c, Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						}
						break;
					}
				case NPC.ID.WRAITH:
					{
						SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.WRAITH_EYES, ref pos, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, new Color(255, 255, 255, 255), Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						for (int num33 = 1; num33 < 10; num33++)
						{
							Vector2 pos3 = pos - Main.NPCSet[num].Velocity * (num33 * 0.5f);
							SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.WRAITH_EYES, ref pos3, Main.NPCSet[num].FrameY, Main.NPCSet[num].FrameHeight, new Color(110 - num33 * 10, 110 - num33 * 10, 110 - num33 * 10, 110 - num33 * 10), Main.NPCSet[num].Rotation, ref pivot, Main.NPCSet[num].Scale, e);
						}
						break;
					}
				}
			}
		}

		private void DrawWoF()
		{
			Vector2 pos = default;
			if (NPC.WoF < 0 || !Player.IsHorrified)
			{
				return;
			}
			float num = Main.NPCSet[NPC.WoF].Position.X + (Main.NPCSet[NPC.WoF].Width >> 1);
			float num2 = Main.NPCSet[NPC.WoF].Position.Y + (Main.NPCSet[NPC.WoF].Height >> 1);
			for (int i = 0; i < Player.MaxNumPlayers; i++)
			{
				if (Main.PlayerSet[i].Active == 0 || !Main.PlayerSet[i].tongued || Main.PlayerSet[i].IsDead)
				{
					continue;
				}
				Vector2 vector = new Vector2(Main.PlayerSet[i].Position.X + 10f, Main.PlayerSet[i].Position.Y + 21f);
				float num3 = num - vector.X;
				float num4 = num2 - vector.Y;
				float rotCenter = (float)Math.Atan2(num4, num3) - 1.57f;
				bool flag = true;
				do
				{
					float num5 = num3 * num3 + num4 * num4;
					if (num5 < 1600f)
					{
						flag = false;
						continue;
					}
					num5 = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.CHAIN12].Height / (float)Math.Sqrt(num5);
					num3 *= num5;
					num4 *= num5;
					vector.X += num3;
					vector.Y += num4;
					num3 = num - vector.X;
					num4 = num2 - vector.Y;
					pos = vector;
					pos.X -= ScreenPosition.X;
					pos.Y -= ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.DrawRotated((int)_sheetSprites.ID.CHAIN12, ref pos, Lighting.GetColor((int)vector.X >> 4, (int)vector.Y >> 4), rotCenter);
				}
				while (flag);
			}
			float num6 = NPC.WoFBottom - NPC.WoFTop;
			for (int j = 0; j < NPC.MaxNumNPCs; j++)
			{
				if (Main.NPCSet[j].Active == 0 || Main.NPCSet[j].AIStyle != 29)
				{
					continue;
				}
				bool flag2 = Main.NPCSet[j].FrameCounter > 7f;
				num2 = NPC.WoFTop + num6 * Main.NPCSet[j].AI0;
				Vector2 vector2 = new Vector2(Main.NPCSet[j].Position.X + (Main.NPCSet[j].Width >> 1), Main.NPCSet[j].Position.Y + (Main.NPCSet[j].Height >> 1));
				float num7 = num - vector2.X;
				float num8 = num2 - vector2.Y;
				float rotCenter2 = (float)Math.Atan2(num8, num7) - 1.57f;
				bool flag3 = true;
				while (flag3)
				{
					SpriteEffects se = SpriteEffects.None;
					if (flag2)
					{
						se = SpriteEffects.FlipHorizontally;
						flag2 = false;
					}
					else
					{
						flag2 = true;
					}
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					if (num9 < 40f)
					{
						flag3 = false;
					}
					num9 = 28f / num9;
					num7 *= num9;
					num8 *= num9;
					vector2.X += num7;
					vector2.Y += num8;
					num7 = num - vector2.X;
					num8 = num2 - vector2.Y;
					pos = vector2;
					pos.X -= ScreenPosition.X;
					pos.Y -= ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.CHAIN12, ref pos, Lighting.GetColor((int)vector2.X >> 4, (int)vector2.Y >> 4), rotCenter2, se);
				}
			}
			int num10 = 140;
			// BUG: This is another bug I am unable to confirm the origin of. It most likely is a TerrariaOGC-made bug.
			// This bug results in the Wall having a top that the player can effortlessly reach.
			// This means the Wall will begin to escalate with the player's Y-position. No functional differences elsewhere.
			float num11 = NPC.WoFTop;
			float num12 = NPC.WoFBottom;
			num12 = ScreenPosition.Y + Main.ResolutionHeight;
			float num13 = ((num11 - ScreenPosition.Y) / num10) + 1;
			num13 *= num10;
			if (num13 > 0f)
			{
				num11 -= num13;
			}
			float num14 = num11;
			float num15 = Main.NPCSet[NPC.WoF].Position.X;
			float num16 = num12 - num11;
			SpriteEffects e = SpriteEffects.None;
			if (Main.NPCSet[NPC.WoF].SpriteDirection == 1)
			{
				e = SpriteEffects.FlipHorizontally;
			}
			if (Main.NPCSet[NPC.WoF].Direction > 0)
			{
				num15 -= 80f;
			}
			int num17 = 0;
			if (++NPC.WoFFront > 12)
			{
				num17 = 280;
				if (NPC.WoFFront > 17)
				{
					NPC.WoFFront = 0;
				}
			}
			else if (NPC.WoFFront > 6)
			{
				num17 = 140;
			}
			do
			{
				num16 = num12 - num14;
				if (num16 > num10)
				{
					num16 = num10;
				}
				int num18 = 0;
				int width = SpriteSheet<_sheetSprites>.Source[(int)_sheetSprites.ID.WALLOFFLESH].Width;
				do
				{
					int x = (int)num15 + (width >> 1) >> 4;
					int y = (int)num14 + num18 >> 4;
					pos.X = num15 - ScreenPosition.X;
					pos.Y = num14 + num18 - ScreenPosition.Y;
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.WALLOFFLESH, ref pos, num17 + num18, 16, Lighting.GetColor(x, y), e);
					num18 += 16;
				}
				while (num18 < num16);
				num14 += num10;
			}
			while (num14 < num12);
		}

		public void DrawNPCHouse()
		{
			for (int i = 0; i < NPC.MaxNumNPCs; i++)
			{
				if (Main.NPCSet[i].Active == 0 || !Main.NPCSet[i].IsTownNPC || Main.NPCSet[i].IsHomeless || Main.NPCSet[i].HomeTileX <= 0 || Main.NPCSet[i].HomeTileY <= 0 || Main.NPCSet[i].Type == (int)NPC.ID.OLD_MAN)
				{
					continue;
				}
				int homeTileX = Main.NPCSet[i].HomeTileX;
				int num = Main.NPCSet[i].HomeTileY - 1;
				while (Main.TileSet[homeTileX, num].IsActive == 0 || !Main.TileSolid[Main.TileSet[homeTileX, num].Type])
				{
					num--;
					if (num < 10)
					{
						break;
					}
				}
				int num2 = 18;
				if (Main.TileSet[homeTileX, num].Type == 19)
				{
					num2 -= 8;
				}
				num++;
				Color color = Lighting.GetColor(homeTileX, num);
				SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.HOUSE_BANNER_1, (homeTileX << 4) - ScreenPosition.X + 8 - 16, (num << 4) - ScreenPosition.Y + num2 - 20, color);
				int num3 = Main.NPCSet[i].GetHeadTextureID() + (int)_sheetSprites.ID.NPC_HEAD_1 - 1;
				float scaleCenter = 1f;
				float num4 = SpriteSheet<_sheetSprites>.Source[num3].Height;
				if (SpriteSheet<_sheetSprites>.Source[num3].Width > num4)
				{
					num4 = SpriteSheet<_sheetSprites>.Source[num3].Width;
				}
				if (num4 > 24f)
				{
					scaleCenter = 24f / num4;
				}
				SpriteSheet<_sheetSprites>.DrawScaled(num3, (homeTileX << 4) - ScreenPosition.X + 8, (num << 4) - ScreenPosition.Y + num2 + 2, scaleCenter, color);
			}
		}

		public void DrawGrid()
		{
#if VERSION_INITIAL // BUG: Idk how it got past QA but grid drawing for the ruler is fucked in the initial versions. It was fixed in 1.01.
			int num = (ScreenPosition.X & -16) - ScreenPosition.X;
			int num2 = (ScreenPosition.Y & -16) - ScreenPosition.Y;
			int num3 = ViewWidth >> 5;
			int num4 = Main.ResolutionHeight >> 5;
			Color c = new Color(100, 100, 100, 15);
			for (int i = 0; i <= num3; i++)
			{
				for (int j = 0; j <= num4; j++)
				{
					SpriteSheet<_sheetSprites>.Draw((int)_sheetSprites.ID.GRID, (i << 6) + num, (j << 6) + num2, c);
				}
			}
#else
			int ID = (int)_sheetSprites.ID.GRID;
			int TexWidth = SpriteSheet<_sheetSprites>.Source[ID].Width;
			int TexHeight = SpriteSheet<_sheetSprites>.Source[ID].Height;
			int num = (ScreenPosition.X & -16) - ScreenPosition.X;
			int num2 = (ScreenPosition.Y & -16) - ScreenPosition.Y;
			int num3 = ViewWidth / TexWidth;
			int num4 = Main.ResolutionHeight / TexHeight;
			Color c = new Color(100, 100, 100, 15);
			for (int i = 0; i <= num3 + 1; i++)
			{
				for (int j = 0; j <= num4 + 1; j++)
				{
					SpriteSheet<_sheetSprites>.Draw(ID, i * TexWidth + num, j * TexHeight + num2, c);
				}
			}
#endif
		}

		public unsafe void spawnSnow()
		{
			if (SnowTiles <= 1024 || Player.XYWH.Y >= Main.WorldSurfacePixels || dustLocal.SnowDust >= 32)
			{
				return;
			}
			int upperBound = 4096 / SnowTiles;
			if (Main.Rand.Next(upperBound) == 0)
			{
				int num = Main.Rand.Next(ViewWidth + (ViewportWidth / 30)) - (ViewportWidth / 60);
				int num2 = ScreenPosition.Y;
				if (num < 0 || num > ViewportWidth)
				{
					num2 += Main.Rand.Next(ViewportHeight / 2) + (ViewportHeight / 10);
				}
				num += ScreenPosition.X;
				Dust* ptr = dustLocal.NewDust(num, num2, 10, 10, 76);
				if (ptr != null)
				{
					ptr->Velocity.X = Cloud.WindSpeed + Main.Rand.Next(-10, 10) * 0.1f;
					ptr->Velocity.Y = 3f + Main.Rand.Next(30) * 0.1f * ptr->Scale;
				}
			}
		}
	}
}
