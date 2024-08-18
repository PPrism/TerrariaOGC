using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Terraria
{
	public static class WorldSelect
	{
#if USE_ORIGINAL_CODE
		private const int MAX_WORLDS = 8;
#else
		private const int MAX_WORLDS = 16; // ADDITION: Max number of worlds has been extended to that of the PS4/XBOne versions

		private static bool SecondPage = false; // Look, I could've made it like with the sessions but frankly, this is cooler.

		private static byte WorldCap = 8;
#endif

		private static byte selectedWorld;

		private static byte selectedSession;

		private static byte sessionListTop;

#if (!VERSION_INITIAL || IS_PATCHED)
		public static bool isLocalWorld;
#endif

		public static string worldPathName;

		private static string[] worldNames = new string[MAX_WORLDS];

		private static int cursorX = 0;

		private static JoinableSession session;

		public static void Update()
		{
#if !USE_ORIGINAL_CODE
			if (WorldCap != 11 && Main.ScreenHeightPtr == 1)
			{
				WorldCap = 11;
			}
			else if (WorldCap != 16 && Main.ScreenHeightPtr == 2)
			{
				WorldCap = 16;
			}

			if (Main.ScreenHeightPtr != 2 && UI.MainUI.IsButtonTriggered(Buttons.LeftTrigger))
			{
				Main.PlaySound(12);
				SecondPage = !SecondPage;

				if (cursorX == 0)
				{
					if (selectedWorld >= WorldCap)
					{
						selectedWorld -= WorldCap;
					}
					else if (selectedWorld <= WorldCap)
					{
						selectedWorld += WorldCap;
						if (selectedWorld >= MAX_WORLDS)
						{
							selectedWorld = MAX_WORLDS - 1;
						}
					}
				}
			}
#endif
			if (UI.MainUI.IsBackButtonTriggered())
			{
				UI.MainUI.PrevMenu();
			}
			else if (UI.MainUI.IsButtonTriggered(Buttons.Y))
			{
				UI.MainUI.ShowParty();
			}
			else if (cursorX == 0)
			{
				if (UI.MainUI.IsButtonTriggered(Buttons.A))
				{
					SelectWorld();
				}
				else if (UI.MainUI.IsButtonTriggered(Buttons.X))
				{
					DeleteWorld();
				}
			}
			else if (UI.MainUI.IsButtonTriggered(Buttons.A))
			{
				JoinSession();
			}
			else if (UI.MainUI.IsButtonTriggered(Buttons.X) && UI.MainUI.CanViewGamerCard())
			{
				UI.MainUI.ShowGamerCard(Gamer.GetFromGamertag(Netplay.availableSessions[selectedSession].HostTag));
			}
		}

		public static void UpdateCursor(int dx, int dy)
		{
			if (dx != 0 || dy != 0)
			{
				Main.PlaySound(12);
			}
			if (dx > 0)
			{
				cursorX = 1;
			}
			else if (dx < 0)
			{
				cursorX = 0;
			}
			int count = Netplay.availableSessions.Count;
			if (count == 0)
			{
				selectedSession = 0;
				sessionListTop = 0;
				cursorX = 0;
			}
			if (dy == 0)
			{
				return;
			}
			if (cursorX == 0)
			{
				dy += selectedWorld;
#if USE_ORIGINAL_CODE
				if (dy < 0)
				{
					dy += MAX_WORLDS;
				}
				else if (dy >= MAX_WORLDS)
				{
					dy -= MAX_WORLDS;
				}
#else
				int Reset = SecondPage ? MAX_WORLDS - WorldCap : WorldCap;

				if (dy < (SecondPage ? WorldCap : 0))
				{
					dy += Reset;
				}
				else if (dy >= (SecondPage ? MAX_WORLDS : WorldCap))
				{
					dy -= Reset;
				}
#endif
				selectedWorld = (byte)dy;
				return;
			}
			dy += selectedSession;
			if (dy < 0)
			{
				dy += count;
			}
			else if (dy >= count)
			{
				dy -= count;
			}
			selectedSession = (byte)dy;
			if (dy < sessionListTop)
			{
				sessionListTop = (byte)dy;
			}
			else if (dy > sessionListTop + (MAX_WORLDS - 1))
			{
				sessionListTop = (byte)(sessionListTop + (dy - (sessionListTop + (MAX_WORLDS - 1))));
			}

		}

		public static void Draw(WorldView view)
		{
			Rectangle rect = default;
			Color white = Color.White;
			int Spacing = 48;
			rect.X = view.SafeAreaOffsetLeft;
			rect.Y = view.SafeAreaOffsetTop;
#if USE_ORIGINAL_CODE
			rect.Width = 416;
			rect.Height = 446;
			Main.DrawRect(451, rect, 64);
			rect.X += 448;
			Main.DrawRect(451, rect, 64);
			UI.DrawStringCC(UI.BigFont, Lang.MenuText[83], view.SafeAreaOffsetLeft + (rect.Width >> 1), rect.Y + 32, white);
			UI.DrawStringCC(UI.BigFont, Lang.MenuText[84], rect.Center.X, rect.Y + 32, white);
			int count = Netplay.availableSessions.Count;
			if (count == 0)
			{
				UI.DrawStringCC(s: (!Netplay.IsFindingSessions()) ? Lang.MenuText[77] : Lang.MenuText[76], font: UI.SmallFont, x: rect.Center.X, y: rect.Center.Y, c: UI.mouseTextColor);
			}
			else // Below is for the network world entries.
			{
				rect.X += 8;
				rect.Y += 80;
				rect.Width = 400;
				rect.Height = 24;
				int num = Math.Min(count, sessionListTop + MAX_WORLDS);
				lock (Netplay.availableSessions)
				{
					for (int i = sessionListTop; i < num; i++)
					{
						int texId = 450;
						int alpha = 255;
						if (i == selectedSession && cursorX == 1)
						{
							alpha = UI.MouseTextBrightness;
							texId = 448;
						}
						Main.DrawRect(texId, rect, alpha);
						rect.Y += Spacing;
					}
					rect.Y -= Spacing * Math.Min(count, MAX_WORLDS);
					white = Color.White;
					for (int j = sessionListTop; j < num; j++)
					{
						JoinableSession joinableSession = Netplay.availableSessions[j];
						string s2 = (j + 1).ToStringLookup() + ".";
						UI.DrawStringLC(UI.BoldSmallFont, s2, rect.X, rect.Center.Y, white);
						UI.DrawStringLC(UI.BoldSmallFont, joinableSession.HostTag, rect.X + 32, rect.Center.Y, white);
						s2 = Lang.MenuText[78] + joinableSession.PlayerCount.ToStringLookup() + "/8";
						UI.DrawStringRC(UI.BoldSmallFont, s2, rect.Right, rect.Center.Y, white);
						rect.Y += Spacing;
					}
				}
			}

			// Below is for the local world entries.

			rect.X = view.SafeAreaOffsetLeft + 8;
			rect.Y = view.SafeAreaOffsetTop + 80;
			rect.Width = 400;
			rect.Height = 24;
			for (int k = 0; k < MAX_WORLDS; k++)
			{
				int texId2 = 450;
				int alpha2 = 255;
				if (k == selectedWorld && cursorX == 0)
				{
					alpha2 = UI.MouseTextBrightness;
					texId2 = 448;
				}
				else if (worldNames[k] == null)
				{
					alpha2 = 212;
					texId2 = 451;
				}
				Main.DrawRect(texId2, rect, alpha2);
				rect.Y += Spacing;
			}
			rect.Y -= 384;

			for (int l = 0; l < MAX_WORLDS; l++)
#else
			int RectWidth = 416;
			int RectHeight = 446;
			int RectJoinX = 448;
			int EntryWidth = 400;
			int EntryHeight = 24;
			int Start = SecondPage ? WorldCap : 0;
			int Limit = SecondPage ? MAX_WORLDS : WorldCap;
			int Reset = SecondPage ? MAX_WORLDS - WorldCap : WorldCap;

			switch (Main.ScreenHeightPtr)
			{
				case 1:
					RectWidth = 560;
					RectHeight = 608;
					RectJoinX = 592;
					EntryWidth = 544;
					EntryHeight = 28;
					Spacing += 1; // This isn't the direct equation but it needs to fit somehow so I'm reducing spacing by 1.
					break;

				case 2:
					RectWidth = 848;
					RectHeight = 932;
					RectJoinX = 880;
					EntryWidth = 832;
					EntryHeight = 35; // Close to *= 1.5, but not quite.
					Spacing = 53;
					break;
			}
			int TextAdjust = Spacing * Reset;

			rect.Width = RectWidth;
			rect.Height = RectHeight;
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, rect, 64);
			rect.X += RectJoinX;
			Main.DrawRect((int)_sheetSprites.ID.INVENTORY_BACK, rect, 64);
			UI.DrawStringCC(UI.BigFont, Lang.MenuText[83], view.SafeAreaOffsetLeft + (rect.Width >> 1), rect.Y + 32, white);
			UI.DrawStringCC(UI.BigFont, Lang.MenuText[84], rect.Center.X, rect.Y + 32, white);
			int count = Netplay.availableSessions.Count;
			if (count == 0)
			{
				UI.DrawStringCC(s: (!Netplay.IsFindingSessions()) ? Lang.MenuText[77] : Lang.MenuText[76], font: UI.SmallFont, x: rect.Center.X, y: rect.Center.Y, c: UI.mouseTextColor);
			}
			else // Below is for the network world entries.
			{
				rect.X += 8;
				rect.Y += 80;
				rect.Width = EntryWidth;
				rect.Height = EntryHeight;

				int num = Math.Min(count, sessionListTop + MAX_WORLDS);
				lock (Netplay.availableSessions)
				{
					for (int i = sessionListTop; i < num; i++)
					{
						int texId = (int)_sheetSprites.ID.INVENTORY_BACK12;
						int alpha = 255;
						if (i == selectedSession && cursorX == 1)
						{
							alpha = UI.MouseTextBrightness;
							texId = (int)_sheetSprites.ID.INVENTORY_BACK10;
						}
						Main.DrawRect(texId, rect, alpha);
						rect.Y += Spacing;
					}
					rect.Y -= Spacing * Math.Min(count, MAX_WORLDS);
					white = Color.White;
					for (int j = sessionListTop; j < num; j++)
					{
						JoinableSession joinableSession = Netplay.availableSessions[j];
						string s2 = (j + 1).ToStringLookup() + ".";
						UI.DrawStringLC(UI.BoldSmallFont, s2, rect.X, rect.Center.Y, white);
						UI.DrawStringLC(UI.BoldSmallFont, joinableSession.HostTag, rect.X + 32, rect.Center.Y, white);
						s2 = Lang.MenuText[78] + joinableSession.PlayerCount.ToStringLookup() + "/8";
						UI.DrawStringRC(UI.BoldSmallFont, s2, rect.Right, rect.Center.Y, white);
						rect.Y += Spacing;
					}
				}
			}

			// Below is for the local world entries.
			rect.X = view.SafeAreaOffsetLeft + 8;
			rect.Y = view.SafeAreaOffsetTop + 80;
			rect.Width = EntryWidth;
			rect.Height = EntryHeight;

			for (int k = Start; k < Limit; k++)
			{

				int texId2 = (int)_sheetSprites.ID.INVENTORY_BACK12;
				int alpha2 = 255;
				if (k == selectedWorld && cursorX == 0)
				{
					alpha2 = UI.MouseTextBrightness;
					texId2 = (int)_sheetSprites.ID.INVENTORY_BACK10;
				}
				else if (worldNames[k] == null)
				{
					alpha2 = 212;
					texId2 = (int)_sheetSprites.ID.INVENTORY_BACK;
				}
				Main.DrawRect(texId2, rect, alpha2);
				rect.Y += Spacing;
			}
			rect.Y -= TextAdjust;

			for (int l = Start; l < Limit; l++)
#endif
			{
				string s3 = worldNames[l] ?? Lang.MenuText[79];
				white = (worldNames[l] == null) ? new Color(200, 200, 220, 255) : new Color(255, 255, 255, 255);
				UI.DrawStringCC(UI.BoldSmallFont, s3, rect.Center.X, rect.Center.Y, white);
				rect.Y += Spacing;
			}
		}

		public static void ControlDescription(StringBuilder strBuilder)
		{
			if (cursorX == 0)
			{
				if (worldNames[selectedWorld] == null)
				{
					strBuilder.Append(Lang.Controls(Lang.CONTROLS.CREATE_WORLD));
				}
				else
				{
					strBuilder.Append(Lang.Controls(Lang.CONTROLS.SELECT));
				}
			}
			else
			{
				strBuilder.Append(Lang.Controls(Lang.CONTROLS.JOIN));
			}
			strBuilder.Append(' ');
			strBuilder.Append(Lang.Controls(Lang.CONTROLS.BACK));
			strBuilder.Append(' ');
			if (cursorX == 0)
			{
				if (worldNames[selectedWorld] != null)
				{
					strBuilder.Append(Lang.MenuText[17]);
					strBuilder.Append(' ');
				}
			}
			else if (UI.MainUI.CanViewGamerCard())
			{
				strBuilder.Append(Lang.Controls(Lang.CONTROLS.X_SHOW_GAMERCARD));
				strBuilder.Append(' ');
			}
			if (UI.MainUI.CanPlayOnline())
			{
				strBuilder.Append(Lang.Controls(Lang.CONTROLS.SHOW_PARTY));
#if !USE_ORIGINAL_CODE
				strBuilder.Append(' ');
#endif
			}
#if !USE_ORIGINAL_CODE
			if (Main.ScreenHeightPtr != 2)
			{
				Lang.CONTROLS PageSwitch = (Lang.CONTROLS)77;

				if (SecondPage)
				{
					PageSwitch++;
				}

				strBuilder.Append('\u0080' + Lang.Controls(PageSwitch).Substring(1));
			}
#endif
		}

		public static string WorldName()
		{
			return worldNames[selectedWorld];
		}

		private static void SelectWorld()
		{
#if (!VERSION_INITIAL || IS_PATCHED)
            isLocalWorld = true;
#endif
			Main.PlaySound(10);
			worldPathName = "world" + selectedWorld + ".wld";
			UI.MainUI.SetMenu(MenuMode.GAME_MODE);
		}

		public static void CreateWorld(string name)
		{
#if (!VERSION_INITIAL || IS_PATCHED)
            isLocalWorld = true;
#endif
			if (UI.MainUI.HasPlayerStorage())
			{
				worldNames[selectedWorld] = name;
			}
			Main.WorldName = name;
			WorldGen.CreateNewWorld();
		}

		public static bool LoadWorlds()
		{
			if (!UI.MainUI.HasPlayerStorage())
			{
				worldPathName = null;
				for (int i = 0; i < MAX_WORLDS; i++)
				{
					worldNames[i] = null;
				}
				return true;
			}
			bool result = true;
			try
			{
				using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Worlds"))
				{
					for (int j = 0; j < MAX_WORLDS; j++)
					{
						string text = "world" + j.ToStringLookup() + ".wld";
						if (storageContainer.FileExists(text))
						{
							try
							{
								using (Stream input = storageContainer.OpenFile(text, FileMode.Open))
								{
									using (BinaryReader binaryReader = new BinaryReader(input))
									{
										int num = binaryReader.ReadInt32();
										if (num > Main.OldWorldDataVersion)
										{
											binaryReader.ReadUInt32();
										}
										worldNames[j] = binaryReader.ReadString();
										binaryReader.Close();
									}
								}
							}
							catch
							{
								Main.ShowSaveIcon();
								storageContainer.DeleteFile(text);
								Main.HideSaveIcon();
								worldNames[j] = null;
								result = false;
							}
						}
						else
						{
							worldNames[j] = null;
						}
					}
					return result;
				}
			}
			catch (IOException)
			{
				UI.MainUI.ReadError();
				for (int k = 0; k < MAX_WORLDS; k++)
				{
					worldNames[k] = null;
				}
				return true;
			}
			catch (Exception)
			{
				return true;
			}
		}

		private static void DeleteWorld()
		{
			if (worldNames[selectedWorld] != null)
			{
				Main.PlaySound(10);
				UI.MainUI.SetMenu(MenuMode.CONFIRM_DELETE_WORLD);
			}
		}

		public static void EraseWorld()
		{
			if (!UI.MainUI.HasPlayerStorage())
			{
				return;
			}
			Main.ShowSaveIcon();
			try
			{
				using (StorageContainer storageContainer = UI.MainUI.OpenPlayerStorage("Worlds"))
				{
					worldPathName = "world" + selectedWorld + ".wld";
					storageContainer.DeleteFile(worldPathName);
				}
			}
			catch (IOException)
			{
				UI.MainUI.WriteError();
			}
			catch (Exception)
			{
			}
			Main.HideSaveIcon();
			worldNames[selectedWorld] = null;
		}

		public static AvailableNetworkSession Session()
		{
			List<SignedInGamer> list = new List<SignedInGamer>(1);
			list.Add(UI.MainUI.SignedInGamer);
			AvailableNetworkSessionCollection availableNetworkSessionCollection = NetworkSession.Find(NetworkSessionType.PlayerMatch, list, session.AvailableSession.SessionProperties);
			session = null;
			if (((ReadOnlyCollection<AvailableNetworkSession>)(object)availableNetworkSessionCollection).Count <= 0)
			{
				return null;
			}
			return ((ReadOnlyCollection<AvailableNetworkSession>)(object)availableNetworkSessionCollection)[0];
		}

		private static void JoinSession()
		{
			session = Netplay.availableSessions[selectedSession];
			Netplay.StopFindingSessions();
			UI.MainUI.SetMenu(MenuMode.NETPLAY);
			UI.MainUI.statusText = Lang.MenuText[80];
			Netplay.StartClient();
		}
	}
}
