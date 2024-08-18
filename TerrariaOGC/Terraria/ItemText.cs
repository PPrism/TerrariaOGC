using Microsoft.Xna.Framework;

namespace Terraria
{
	public struct ItemText
	{
		public const int ActiveTime = 56;

		public byte Active;

		public short LifeTime;

		public short NetID;

		public Vector2 Position;

		public float VelocityY;

		public float Alpha;

		public float AlphaDir;

		public float Scale;

		public Color Color;

		public int Stack;

		public string Text;

		public Vector2 TextSize;

		public void Init()
		{
			Active = 0;
		}

		public void Update(int WhoAmI, ItemTextPool TextPool)
		{
			Alpha += AlphaDir;
			if (Alpha <= 0.7f)
			{
				Alpha = 0.7f;
				AlphaDir = 0f - AlphaDir;
			}
			else if (Alpha >= 1f)
			{
				Alpha = 1f;
				AlphaDir = 0f - AlphaDir;
			}
			bool TopLimit = false;
			Vector2 Size = TextSize;
			Size *= Scale;
			Size.Y *= 0.8f;
			Rectangle ItemTextPlacement = default;
			Rectangle ItemTextPlacement2 = default;
			ItemTextPlacement.X = (int)(Position.X - Size.X * 0.5f);
			ItemTextPlacement.Y = (int)(Position.Y - Size.Y * 0.5f);
			ItemTextPlacement.Width = (int)Size.X;
			ItemTextPlacement.Height = (int)Size.Y;
			for (int ActiveText = 0; ActiveText < 4; ActiveText++)
			{
				if (TextPool.ItemText[ActiveText].Active == 0 || ActiveText == WhoAmI)
				{
					continue;
				}
				Vector2 Size2 = TextPool.ItemText[ActiveText].TextSize;
				Size2 *= TextPool.ItemText[ActiveText].Scale;
				Size2.Y *= 0.8f;
				ItemTextPlacement2.X = (int)(TextPool.ItemText[ActiveText].Position.X - Size2.X * 0.5f);
				ItemTextPlacement2.Y = (int)(TextPool.ItemText[ActiveText].Position.Y - Size2.Y * 0.5f);
				ItemTextPlacement2.Width = (int)Size2.X;
				ItemTextPlacement2.Height = (int)Size2.Y;
				if (ItemTextPlacement.Intersects(ItemTextPlacement2) && (Position.Y < TextPool.ItemText[ActiveText].Position.Y || (Position.Y == TextPool.ItemText[ActiveText].Position.Y && WhoAmI < ActiveText)))
				{
					TopLimit = true;
					int TextCount = TextPool.ActiveTexts;
					if (TextCount > ItemTextPool.MaxNumItemTexts - 1)
					{
						TextCount = ItemTextPool.MaxNumItemTexts - 1;
					}
					TextPool.ItemText[ActiveText].LifeTime = (LifeTime = (short)(ActiveTime + TextCount * 14));
				}
			}
			if (!TopLimit)
			{
				VelocityY *= 0.86f;
				if (Scale == 1f)
				{
					VelocityY *= 0.4f;
				}
			}
			else if (VelocityY > -6f)
			{
				VelocityY -= 0.2f;
			}
			else
			{
				VelocityY *= 0.86f;
			}
			Position.Y += VelocityY;
			if (--LifeTime <= 0)
			{
				Scale -= 0.03f;
				if (Scale < 0.1f)
				{
					Active = 0;
				}
				LifeTime = 0;
				return;
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
	}
}
