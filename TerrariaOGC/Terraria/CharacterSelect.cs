#if !VERSION_INITIAL
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Terraria
{
	public static class CharacterSelect
	{
		private const int MaxCharacters = UI.MAX_LOAD_PLAYERS;

		private static sbyte hoveredCharacter;

		private static readonly string[] CharacterNames = new string[MaxCharacters];

		private static bool WaitNeeded = false;

		public static void Update(UI LocalInstance)
		{
			if (LocalInstance != UI.MainUI)
			{
				WaitNeeded = true;
			}

			for (int l = 0; l < UI.MAX_LOAD_PLAYERS; l++)
			{
				if (l < UI.MainUI.numLoadPlayers)
				{
					CharacterNames[l] = UI.MainUI.loadPlayer[l].CharacterName;
				}
				else
				{
					CharacterNames[l] = null;
				}
			}

			if (UI.MainUI.IsBackButtonTriggered() && (Netplay.gamersWhoReceivedInvite.Count < 2 || !Netplay.gamersWhoReceivedInvite.Contains(UI.MainUI.SignedInGamer)))
			{
				Netplay.gamersWhoReceivedInvite.Remove(UI.MainUI.SignedInGamer);
				if (Netplay.gamersWhoReceivedInvite.Count == 0)
				{
					Netplay.isJoiningRemoteInvite = false;
					Netplay.gamersWaitingToJoinInvite.Clear();
				}
				else
				{
					Netplay.gamersWaitingToJoinInvite.Remove(UI.MainUI.SignedInGamer);
				}
				UI.MainUI.SetMenu(MenuMode.TITLE, rememberPrevious: false, reset: true);
			}

			if (UI.MainUI.IsButtonTriggered(Buttons.A))
			{
				SelectCharacter();
			}

			if (hoveredCharacter >= 0 && hoveredCharacter < UI.MainUI.numLoadPlayers)
			{
				if (UI.MainUI.IsButtonTriggered(Buttons.X))
				{
					UI.MainUI.selectedPlayer = hoveredCharacter;
					Main.PlaySound(10);
					UI.MainUI.SetMenu(MenuMode.CONFIRM_DELETE_CHARACTER);
				}
				else
				{
					UI.MainUI.showPlayer = hoveredCharacter;
				}
			}
		}

		public static void UpdateCursor(int dx, int dy)
		{
			if (dx != 0 || dy != 0)
			{
				Main.PlaySound(12);
			}
			if (dy == 0)
			{
				return;
			}
			dy += hoveredCharacter;
			if (dy < 0)
			{
				dy += MaxCharacters;
			}
			else if (dy >= MaxCharacters)
			{
				dy -= MaxCharacters;
			}
			hoveredCharacter = (sbyte)dy;
			return;
		}

		public static void Draw(WorldView view)
		{
			int Spacing = 53;
			int TextAdjust = 212 + Spacing;
			int EntryWidth = 432;
			int EntryHeight = 35;
			int SALeftAdd = 288;
			int SATopAdd = 188;

			switch (Main.ScreenHeightPtr)
			{
				case 1:
					EntryWidth = 576; // *= 1.3
					SALeftAdd = 384; // *= 1.3
					SATopAdd = 251; // Close to *= 1.3
					break;

				case 2:
					EntryWidth *= 2;
					SALeftAdd *= 2;
					SATopAdd *= 2;
					break;
			}

			Rectangle rect = default;
			rect.X = view.SafeAreaOffsetLeft + SALeftAdd;
			rect.Y = view.SafeAreaOffsetTop + SATopAdd;
			rect.Width = EntryWidth;
			rect.Height = EntryHeight;
			for (int k = 0; k < MaxCharacters; k++)
			{
				int texId2 = (int)_sheetSprites.ID.INVENTORY_BACK12;
				int alpha2 = 255;
				if (k == hoveredCharacter)
				{
					alpha2 = UI.MouseTextBrightness;
					texId2 = (int)_sheetSprites.ID.INVENTORY_BACK10;
				}
				else if (CharacterNames[k] == null)
				{
					alpha2 = 212;
					texId2 = (int)_sheetSprites.ID.INVENTORY_BACK;
				}
				Main.DrawRect(texId2, rect, alpha2);
				rect.Y += Spacing;
			}

			rect.Y -= TextAdjust;

			for (int l = 0; l < MaxCharacters; l++)
			{
				string NameOrEmpty = CharacterNames[l] ?? Lang.MenuText[79];
				Color NameColour = (CharacterNames[l] == null) ? new Color(200, 200, 220, 255) : new Color(255, 255, 255, 255);

				switch (UI.MainUI.loadPlayer[l].difficulty) // ADDITION: 1.01 and above made character select akin to the world select, but unfortunately, it packed up colouring for different difficulties in exchange.
				{
					// In TerrariaOGC, it has returned.
					case 1: // Mediumcore
						NameColour = new Color(UI.mcColorR, UI.mcColorG, UI.mcColorB);
						break;
					case 2: // Hardcore
						NameColour = new Color(UI.hcColorR, UI.hcColorG, UI.hcColorB);
						break;
				}

				UI.DrawStringCC(UI.BoldSmallFont, NameOrEmpty, rect.Center.X, rect.Center.Y, NameColour);
				rect.Y += Spacing;
			}
		}

		public static void ControlDescription(StringBuilder strBuilder)
		{
			if (CharacterNames[hoveredCharacter] == null)
			{
				strBuilder.Append(Lang.Controls(Lang.CONTROLS.CREATE_CHARACTER));
			}
			else
			{
				strBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
			}
			strBuilder.Append(' ');
			strBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
			strBuilder.Append(' ');
			if (CharacterNames[hoveredCharacter] != null)
			{
				strBuilder.Append(Lang.MenuText[17]);
				strBuilder.Append(' ');
			}
		}

		private static void SelectCharacter()
		{
			Main.PlaySound(10);
			if (CharacterNames[hoveredCharacter] != null)
			{
				UI.MainUI.selectedPlayer = hoveredCharacter;
				UI.MainUI.SetPlayer(UI.MainUI.loadPlayer[hoveredCharacter].DeepCopy());
				UI.MainUI.playerPathName = UI.MainUI.loadPlayerPath[hoveredCharacter];

				if (Netplay.isJoiningRemoteInvite)
				{
					UI.MainUI.SetMenu(MenuMode.NETPLAY);
					UI.MainUI.statusText = Lang.MenuText[75];
				}
				else if (WaitNeeded)
				{
					UI.MainUI.SetMenu(MenuMode.WAITING_SCREEN);
				}
				else
				{
					UI.MainUI.SetMenu(MenuMode.WORLD_SELECT);
				}
			}
			else
			{
				Player[] PlayerArray = UI.MainUI.loadPlayer;
				sbyte PlayerLoadNumber = UI.MainUI.numLoadPlayers;

				PlayerArray[PlayerLoadNumber] = new Player();
				PlayerArray[PlayerLoadNumber].CharacterName = UI.MainUI.SignedInGamer.Gamertag;
				PlayerArray[PlayerLoadNumber].Inventory[0].SetDefaults("Copper Shortsword");
				PlayerArray[PlayerLoadNumber].Inventory[0].SetPrefix(-1);
				PlayerArray[PlayerLoadNumber].Inventory[1].SetDefaults("Copper Pickaxe");
				PlayerArray[PlayerLoadNumber].Inventory[1].SetPrefix(-1);
				PlayerArray[PlayerLoadNumber].Inventory[2].SetDefaults("Copper Axe");
				PlayerArray[PlayerLoadNumber].Inventory[2].SetPrefix(-1);
				UI.MainUI.CreateCharacterGUI.ApplyDefaultAttributes(PlayerArray[PlayerLoadNumber]);
				UI.MainUI.SetMenu(MenuMode.CREATE_CHARACTER);
			}
		}
	}
}
#endif