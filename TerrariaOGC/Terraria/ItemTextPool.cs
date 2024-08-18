using Microsoft.Xna.Framework;

namespace Terraria
{
	public sealed class ItemTextPool
	{
		public const int MaxNumItemTexts = 4;

		public int ActiveTexts;

		public WorldView View;

		public ItemText[] ItemText;

		public ItemTextPool(WorldView ActiveView)
		{
			ActiveTexts = 0;
			View = ActiveView;
			ItemText = new ItemText[MaxNumItemTexts];
		}

		public void Clear()
		{
			for (int Texts = MaxNumItemTexts - 1; Texts >= 0; Texts--)
			{
				ItemText[Texts].Init();
			}
		}

		public void Update()
		{
			int Counter = 0;
			for (int Texts = MaxNumItemTexts - 1; Texts >= 0; Texts--)
			{
				if (ItemText[Texts].Active != 0)
				{
					Counter++;
					ItemText[Texts].Update(Texts, this);
				}
			}
			ActiveTexts = Counter;
		}

		public void NewText(ref Item NewItem, int ItemStack)
		{
			if (View.Ui.InventoryMode > 0 || !View.Ui.ShowItemText || NewItem.Active == 0)
			{
				return;
			}
			int CurrentTextCount = -1;
			for (int Texts = MaxNumItemTexts - 1; Texts >= 0; Texts--)
			{
				if (ItemText[Texts].Active != 0)
				{
					if (ItemText[Texts].NetID == NewItem.NetID && NewItem.PrefixType == 0)
					{
						ItemText[Texts].Stack += ItemStack;
						Main.StrBuilder.Length = 0;
						Main.StrBuilder.Append(Lang.ItemName(NewItem.NetID));
						Main.StrBuilder.Append(ItemText[Texts].Stack.ToStackString());
						Vector2 ItemTextSize = UI.BoldSmallFont.MeasureString(Main.StrBuilder);
						ItemText[Texts].Text = Main.StrBuilder.ToString();
						ItemText[Texts].TextSize = ItemTextSize;
						ItemText[Texts].LifeTime = Terraria.ItemText.ActiveTime;
						ItemText[Texts].Scale = 0f;
						ItemText[Texts].Position.X = NewItem.Position.X + (NewItem.Width - ItemTextSize.X) * 0.5f;
						ItemText[Texts].Position.Y = NewItem.Position.Y + (NewItem.Height >> 2) - ItemTextSize.Y * 0.5f;
						ItemText[Texts].VelocityY = -7f;
						return;
					}
				}
				else
				{
					CurrentTextCount = Texts;
				}
			}
			if (CurrentTextCount < 0)
			{
				float YLimit = Main.BottomWorld;
				for (int Texts = 0; Texts < MaxNumItemTexts; Texts++)
				{
					if (YLimit > ItemText[Texts].Position.Y)
					{
						CurrentTextCount = Texts;
						YLimit = ItemText[Texts].Position.Y;
					}
				}
			}
			if (CurrentTextCount >= 0)
			{
				string ItemName = NewItem.AffixName();
				ItemText[CurrentTextCount].Active = 1;
				ItemText[CurrentTextCount].LifeTime = Terraria.ItemText.ActiveTime;
				ItemText[CurrentTextCount].NetID = NewItem.NetID;
				ItemText[CurrentTextCount].Stack = ItemStack;
				if (ItemStack > 1)
				{
					ItemName += ItemStack.ToStackString();
				}
				ItemText[CurrentTextCount].Text = ItemName;
				Vector2 ItemTextSize = UI.MeasureString(UI.BoldSmallFont, ItemName);
				ItemText[CurrentTextCount].TextSize = ItemTextSize;
				ItemText[CurrentTextCount].Alpha = 1f;
				ItemText[CurrentTextCount].AlphaDir = -0.01f;
				ItemText[CurrentTextCount].Scale = 0f;
				ItemText[CurrentTextCount].VelocityY = -7f;
				ItemText[CurrentTextCount].Position.X = NewItem.Position.X + NewItem.Width * 0.5f - ItemTextSize.X * 0.5f;
				ItemText[CurrentTextCount].Position.Y = NewItem.Position.Y + NewItem.Height * 0.25f - ItemTextSize.Y * 0.5f;

				switch (NewItem.Rarity)
				{
					case 1:
						ItemText[CurrentTextCount].Color = new Color(150, 150, 255);
						break;
					case 2:
						ItemText[CurrentTextCount].Color = new Color(150, 255, 150);
						break;
					case 3:
						ItemText[CurrentTextCount].Color = new Color(255, 200, 150);
						break;
					case 4:
						ItemText[CurrentTextCount].Color = new Color(255, 150, 150);
						break;
					case 5:
						ItemText[CurrentTextCount].Color = new Color(255, 150, 255);
						break;
					case -1:
						ItemText[CurrentTextCount].Color = new Color(130, 130, 130);
						break;
					case 6:
						ItemText[CurrentTextCount].Color = new Color(210, 160, 255);
						break;
					default:
						ItemText[CurrentTextCount].Color = Color.White;
						break;
				}
			}
		}
	}
}
