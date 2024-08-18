using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	internal class BoxGraphic
	{
		private readonly Texture2D Graphic;

		public Color Color;

		private readonly Rectangle[] Destinations;

		private readonly Rectangle[] Sources;

		public int Width;

		public int Height;

		private static Rectangle[] GetBox9Rectangles(int RectWidth, int RectHeight, int RectBorder)
		{
			return new Rectangle[9]
			{
				new Rectangle(0, 0, RectBorder, RectBorder),
				new Rectangle(RectBorder, 0, RectWidth - RectBorder * 2, RectBorder),
				new Rectangle(RectWidth - RectBorder, 0, RectBorder, RectBorder),
				new Rectangle(0, RectBorder, RectBorder, RectHeight - RectBorder * 2),
				new Rectangle(RectBorder, RectBorder, RectWidth - RectBorder * 2, RectHeight - RectBorder * 2),
				new Rectangle(RectWidth - RectBorder, RectBorder, RectBorder, RectHeight - RectBorder * 2),
				new Rectangle(0, RectHeight - RectBorder, RectBorder, RectBorder),
				new Rectangle(RectBorder, RectHeight - RectBorder, RectWidth - RectBorder * 2, RectBorder),
				new Rectangle(RectWidth - RectBorder, RectHeight - RectBorder, RectBorder, RectBorder)
			};
		}

		public static BoxGraphic Create(int Width, int Height, Texture2D Graphic, int BorderWidth, Color Color)
		{
			Rectangle[] Box9Rectangles = GetBox9Rectangles(Width, Height, BorderWidth);
			Rectangle[] Box9Rectangles2 = GetBox9Rectangles(Graphic.Width, Graphic.Height, BorderWidth);
			return new BoxGraphic(Graphic, Color, Box9Rectangles2, Box9Rectangles, Width, Height);
		}

		public BoxGraphic(Texture2D RectGraphic, Color RectColor, Rectangle[] RectSources, Rectangle[] RectDestinations, int RectWidth, int RectHeight)
		{
			Graphic = RectGraphic;
			Color = RectColor;
			Destinations = RectDestinations;
			Sources = RectSources;
			Width = RectWidth;
			Height = RectHeight;
		}

		public void Draw(Vector2i Position, float Alpha)
		{
			for (int Rectangle = 0; Rectangle < 9; Rectangle++)
			{
				Rectangle DestinationRect = Destinations[Rectangle];
				DestinationRect.Offset(Position.X, Position.Y);
				Main.SpriteBatch.Draw(Graphic, DestinationRect, (Rectangle?)Sources[Rectangle], Color * Alpha);
			}
		}
	}
}
