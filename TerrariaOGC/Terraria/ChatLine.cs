using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct ChatLine
	{
		public Color Color;

		public int ShowTime;

		public string Text;

#if (!VERSION_INITIAL || IS_PATCHED)
		public Vector2 Size;
#endif

		public void Init()
		{
			Color = Color.White;
			ShowTime = 0;
			Text = null;
		}
	}
}
