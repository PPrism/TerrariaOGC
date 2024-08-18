using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace Terraria
{
	public class SpriteSheet<T>
	{
		protected static Texture2D Tex;

		public static Rectangle[] Source;

		public static void Draw(int id, int x, int y, int sy, int sh, Color c, float rotCenter, float scaleCenter)
		{
			Rectangle value = Source[id];
			value.Y += sy;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)value, c, rotCenter, new Vector2(value.Width >> 1, sh >> 1), scaleCenter, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, int x, int y, int sx, int sw, int sh, Color c)
		{
			Rectangle value = Source[id];
			value.X += sx;
			value.Width = sw;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)value, c);
		}

		public static void Draw(int id, ref Vector2 pos, int sy, int sh, Color c, SpriteEffects e)
		{
			Rectangle value = Source[id];
			value.Y += sy;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, 0f, default(Vector2), 1f, e, 0f);
		}

		public static void DrawRotated(int id, ref Vector2 pos, int sy, int sh, Color c, float rot, SpriteEffects e)
		{
			Rectangle value = Source[id];
			value.Y += sy;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, rot, new Vector2(value.Width >> 1, sh >> 1), 1f, e, 0f);
		}

		public static void DrawRotated(int id, ref Vector2 pos, int sy, int sh, Color c, float rot, ref Vector2 pivot, SpriteEffects e)
		{
			Rectangle value = Source[id];
			value.Y += sy;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, rot, pivot, 1f, e, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, int sy, int sh, Color c, float rot, ref Vector2 pivot, float scale, SpriteEffects e)
		{
			Rectangle value = Source[id];
			value.Y += sy;
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, rot, pivot, scale, e, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c, float rot, ref Vector2 pivot, float scale, SpriteEffects e)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, rot, pivot, scale, e, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c);
		}

		public static void Draw(int id, ref Vector2 pos, ref Rectangle s, Color c)
		{
			Rectangle value = Source[id];
			value.X += s.X;
			value.Y += s.Y;
			value.Width = s.Width;
			value.Height = s.Height;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c);
		}

		public static void DrawStretched(int id, Rectangle s, ref Rectangle dest, Color c)
		{
			s.X += Source[id].X;
			s.Y += Source[id].Y;
			Main.SpriteBatch.Draw(Tex, dest, (Rectangle?)s, c);
		}

		public static void DrawStretchedX(int id, ref Rectangle dest, Color c)
		{
			Rectangle value = Source[id];
			value.X += 4;
			value.Width -= 8;
			Main.SpriteBatch.Draw(Tex, dest, (Rectangle?)value, c);
		}

		public static void DrawStretchedY(int id, ref Rectangle dest, Color c)
		{
			Rectangle value = Source[id];
			value.Y += 4;
			value.Height -= 8;
			Main.SpriteBatch.Draw(Tex, dest, (Rectangle?)value, c);
		}

		public static void Draw(int id, int x, int y)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], Color.White);
		}

		public static void DrawCentered(int id, int x, int y, Rectangle rect, Color c)
		{
			rect.X += Source[id].X;
			rect.Y += Source[id].Y;
			Main.SpriteBatch.Draw(Tex, new Vector2(x - (rect.Width >> 1), y - (rect.Height >> 1)), (Rectangle?)rect, c);
		}

		public static void DrawCentered(int id, int x, int y, Rectangle rect, Color c, float scale)
		{
			rect.X += Source[id].X;
			rect.Y += Source[id].Y;
			Vector2 vector = new Vector2(rect.Width >> 1, rect.Height >> 1);
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)rect, c, 0f, vector, scale, SpriteEffects.None, 0f);
		}

		public static void DrawCentered(int id, ref Rectangle rect)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(rect.X + (rect.Width >> 1) - (Source[id].Width >> 1), rect.Y + (rect.Height >> 1) - (Source[id].Height >> 1)), (Rectangle?)Source[id], Color.White);
		}

		public static void DrawCentered(int id, ref Rectangle rect, float scale) // Custom Entry for Crafting Categories.
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(rect.X + (rect.Width >> 1) - ((Source[id].Width >> 1) * scale), rect.Y + (rect.Height >> 1) - ((Source[id].Height >> 1) * scale)), (Rectangle?)Source[id], Color.White, 0f, default, scale, SpriteEffects.None, 0f);
		}

		public static void DrawCentered(int id, ref Rectangle rect, SpriteEffects se)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(rect.X + (rect.Width >> 1) - (Source[id].Width >> 1), rect.Y + (rect.Height >> 1) - (Source[id].Height >> 1)), (Rectangle?)Source[id], Color.White, 0f, default, 1f, se, 0f);
		}

		public static void DrawCentered(int id, ref Rectangle rect, float scale, SpriteEffects se) // Custom entry for the menu arrows
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(rect.X + (rect.Width >> 1) - ((Source[id].Width >> 1) * scale), rect.Y + (rect.Height >> 1) - ((Source[id].Height >> 1) * scale)), (Rectangle?)Source[id], Color.White, 0f, default, scale, se, 0f);
		}

		public static void Draw(int id, int x, int y, Color c)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c);
		}

		public static void Draw(int id, int x, int y, Color c, SpriteEffects se)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, default(Vector2), 1f, se, 0f);
		}

		public static void DrawScaled(int id, int x, int y, float scaleCenter, Color c)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), scaleCenter, SpriteEffects.None, 0f);
		}

		public static void DrawRotated(int id, ref Vector2 pos, Color c, float rotCenter)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, rotCenter, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), 1f, SpriteEffects.None, 0f);
		}

#if !USE_ORIGINAL_CODE
		public static void DrawRotatedTL(int id, int x, int y, Color c, float rotTL, float scale)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, rotTL, default, scale, SpriteEffects.None, 0f);
		}
#else
		public static void DrawRotatedTL(int id, int x, int y, Color c, float rotTL)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, rotTL, default, 1f, SpriteEffects.None, 0f);
		}
#endif

		public static void DrawScaled(int id, ref Vector2 pos, Color c, float scaleCenter)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, 0f, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), scaleCenter, SpriteEffects.None, 0f);
		}

		public static void DrawScaledTL(int id, ref Vector2 pos, Color c, float scale)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
		}

		public static void DrawScaledTL(int id, int x, int y, Color c, float scale)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
		}

		public static void DrawScaled(int id, int x, int y, float scaleCenter, Color c, SpriteEffects e)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), scaleCenter, e, 0f);
		}

		public static void Draw(int id, int x, int y, Color c, float rotCenter, float scaleCenter)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, rotCenter, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), scaleCenter, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c, SpriteEffects se)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, 0f, default(Vector2), 1f, se, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c, float rotCenter, SpriteEffects se)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, rotCenter, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), 1f, se, 0f);
		}

		public static void DrawTL(int id, ref Vector2 pos, Color c, float scaleTopLeft)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, 0f, default(Vector2), scaleTopLeft, SpriteEffects.None, 0f);
		}

		public static void DrawTL(int id, int x, int y, Color c, float scaleTopLeft)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, default(Vector2), scaleTopLeft, SpriteEffects.None, 0f);
		}

		public static void DrawL(int id, int x, int y, Color c, float scaleCenterLeft)
		{
			Main.SpriteBatch.Draw(Tex, new Vector2(x, y), (Rectangle?)Source[id], c, 0f, new Vector2(0f, Source[id].Height >> 1), scaleCenterLeft, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c, float rot, float scale)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, rot, new Vector2(Source[id].Width >> 1, Source[id].Height >> 1), scale, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, ref Rectangle s, Color c, float rot, ref Vector2 pivot, float scale)
		{
			Rectangle value = Source[id];
			value.X += s.X;
			value.Y += s.Y;
			value.Width = s.Width;
			value.Height = s.Height;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, rot, pivot, scale, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, int sh, Color c, float rot, float scale = 1f)
		{
			Rectangle value = Source[id];
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, rot, new Vector2(value.Width >> 1, Source[id].Height >> 1), scale, SpriteEffects.None, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, int sh, Color c, SpriteEffects se)
		{
			Rectangle value = Source[id];
			value.Height = sh;
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)value, c, 0f, default(Vector2), 1f, se, 0f);
		}

		public static void Draw(int id, ref Vector2 pos, Color c, float rot, ref Vector2 pivot, float scale)
		{
			Main.SpriteBatch.Draw(Tex, pos, (Rectangle?)Source[id], c, rot, pivot, scale, SpriteEffects.None, 0f);
		}

		public static Vector2 GetCenterPivot(int id)
		{
			return new Vector2(Source[id].Width >> 1, Source[id].Height >> 1);
		}

		public static int GetCenterPivotY(int id)
		{
			return Source[id].Height >> 1;
		}
	}
}
