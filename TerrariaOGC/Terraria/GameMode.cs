using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Terraria
{
	public static class GameMode
	{
		private static int CursorY = 2;

		public static void Update()
		{
			if (UI.MainUI.IsBackButtonTriggered())
			{
				UI.MainUI.PrevMenu();
			}
			else if (UI.MainUI.IsButtonTriggered(Buttons.A))
			{
				switch (CursorY)
				{
				case 0:
				{
					UI.MainUI.IsOnline = !UI.MainUI.IsOnline;
					UI.MainUI.SettingsDirty = true;
					break;
				}
				case 1:
				{
					UI.MainUI.IsInviteOnly = !UI.MainUI.IsInviteOnly;
					UI.MainUI.SettingsDirty = true;
					break;
				}
				case 2:
					StartGame();
					break;
				}
			}
		}

		public static void UpdateCursor(int XCoord, int YCoord)
		{
			if (!UI.MainUI.CanPlayOnline())
			{
				UI.MainUI.IsOnline = false;
				CursorY = 2;
			}
			else
			{
				if (YCoord == 0)
				{
					return;
				}
				Main.PlaySound(12);
				int YPlacement = CursorY;
				do
				{
					YPlacement += YCoord;
					if (YPlacement < 0)
					{
						YPlacement = 2;
					}
					else if (YPlacement > 2)
					{
						YPlacement = 0;
					}
				}
				while (YPlacement == 1 && !UI.MainUI.IsOnline);
				CursorY = (byte)YPlacement;
			}
		}

		public static void Draw(WorldView view)
		{
			Rectangle rect = default(Rectangle);

#if USE_ORIGINAL_CODE
			rect.X = (view.ViewWidth >> 1) - 190;
			rect.Y = 210;
			rect.Width = 380;
			rect.Height = 168;
#else
			rect.X = (view.ViewWidth >> 1) - (int)(190 * Main.ScreenMultiplier);
			rect.Y = (int)(210 * Main.ScreenMultiplier);
			rect.Width = (int)(380 * Main.ScreenMultiplier);
			rect.Height = (int)(168 * Main.ScreenMultiplier);
#endif

			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, rect, 64);
			Color c = Color.White;
#if USE_ORIGINAL_CODE
			int num = (view.ViewWidth >> 1) - 100;
			int num2 = 230;
#else
			int num = (view.ViewWidth >> 1) - (int)(100 * Main.ScreenMultiplier);
			int num2 = (int)(230 * Main.ScreenMultiplier);
#endif
			bool flag = UI.MainUI.CanPlayOnline();

#if USE_ORIGINAL_CODE
			if (CursorY == 0)
			{
				view.Ui.DrawInventoryCursor(num, num2, 1.0);
			}
			else
			{
				c = (flag ? Color.White : new Color(128, 128, 128, 255));
				SpriteSheet<_sheetSprites>.Draw(451, num, num2, c);
			}
			if (UI.MainUI.IsOnline)
			{
				SpriteSheet<_sheetSprites>.Draw(202, num + 10, num2 + 10, c);
			}

			UI.DrawStringLC(UI.SmallFont, Lang.MenuText[6], num + 60, num2 + 26, c);
			num2 += 64;

			if (CursorY == 1)
			{
				view.Ui.DrawInventoryCursor(num, num2, 1.0);
			}
			else
			{
				c = ((flag && UI.MainUI.IsOnline) ? Color.White : new Color(128, 128, 128, 255));
				SpriteSheet<_sheetSprites>.Draw(451, num, num2, c);
			}
			if (UI.MainUI.IsInviteOnly)
			{
				SpriteSheet<_sheetSprites>.Draw(202, num + 10, num2 + 10, c);
			}
			UI.DrawStringLC(UI.SmallFont, Lang.MenuText[7], num + 60, num2 + 26, c);
#else
			if (CursorY == 0)
			{
				view.Ui.DrawInventoryCursor(num, num2, Main.ScreenMultiplier);
			}
			else
			{
				c = (flag ? Color.White : new Color(128, 128, 128, 255));
				SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num, num2, c, Main.ScreenMultiplier);
			}
			if (UI.MainUI.IsOnline)
			{
				SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.CHECK, num + (int)(10 * Main.ScreenMultiplier), num2 + (int)(10 * Main.ScreenMultiplier), c, Main.ScreenMultiplier);
			}
			UI.DrawStringLC(UI.SmallFont, Lang.MenuText[6], num + (int)(60 * Main.ScreenMultiplier), num2 + (int)(26 * Main.ScreenMultiplier), c);
			
			num2 += (int)(64 * Main.ScreenMultiplier);

			if (CursorY == 1)
			{
				view.Ui.DrawInventoryCursor(num, num2, Main.ScreenMultiplier);
			}
			else
			{
				c = ((flag && UI.MainUI.IsOnline) ? Color.White : new Color(128, 128, 128, 255));
				SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.INVENTORY_BACK, num, num2, c, Main.ScreenMultiplier);
			}
			if (UI.MainUI.IsInviteOnly)
			{
				SpriteSheet<_sheetSprites>.DrawScaledTL((int)_sheetSprites.ID.CHECK, num + (int)(10 * Main.ScreenMultiplier), num2 + (int)(10 * Main.ScreenMultiplier), c, Main.ScreenMultiplier);
			}
			UI.DrawStringLC(UI.SmallFont, Lang.MenuText[7], num + (int)(60 * Main.ScreenMultiplier), num2 + (int)(26 * Main.ScreenMultiplier), c);
#endif
			string text = ((WorldSelect.WorldName() != null) ? Lang.MenuText[10] : Lang.MenuText[11]);
			float num3 = 1f;
			if (CursorY != 2)
			{
				c = new Color(240, 240, 240, 240);
			}
			else
			{
				num3 *= 1f + UI.cursorAlpha * 0.1f;
				c = new Color(UI.CursorColour.A, UI.CursorColour.A, 100, 255);
			}
			Vector2 pivot = UI.MeasureString(UI.BigFont, text);
			pivot.X *= 0.5f;
			pivot.Y *= 0.5f;

#if USE_ORIGINAL_CODE
			UI.DrawStringScaled(pos: new Vector2(view.ViewWidth >> 1, 454f), font: UI.BigFont, s: text, c: c, pivot: pivot, scale: num3);
#else
			float StringY = 454f;
			switch (Main.ScreenHeightPtr)
			{
				case 1:
					StringY = 620f;
					break;

				case 2:
					StringY = 940f;
					break;
			}
			UI.DrawStringScaled(pos: new Vector2(view.ViewWidth >> 1, StringY), font: UI.BigFont, s: text, c: c, pivot: pivot, scale: num3);
#endif
		}

		public static void ControlDescription(StringBuilder StringBuilder) // Unused, it relies on the default instead, which is identical to this.
		{
			StringBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
			StringBuilder.Append(' ');
			StringBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
		}

		private static void StartGame()
		{
			Main.PlaySound(10);
			string WorldName = WorldSelect.WorldName();
			if (WorldName == null)
			{
				UI.MainUI.SetMenu(MenuMode.WORLD_SIZE);
				return;
			}
			UI.MainUI.SetMenu(MenuMode.STATUS_SCREEN);
			Main.WorldName = WorldName;
			WorldGen.PlayWorld();
		}
	}
}
