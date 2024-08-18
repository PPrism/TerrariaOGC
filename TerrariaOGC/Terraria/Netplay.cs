using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace Terraria
{
	public sealed class Netplay
	{
		public enum ClientState
		{
			JOINING,
			WAITING_FOR_PLAYER_ID,
			WAITING_FOR_PLAYER_DATA_REQ,
			RECEIVED_PLAYER_DATA_REQ,
			WAITING_FOR_WORLD_INFO,
			ANNOUNCING_SPAWN_LOCATION,
			WAITING_FOR_TILE_DATA,
			PLAYING
		}

		public static bool PlayDisconnect;

		public static bool ToStopSession;

		public static bool HasHookEvents;

		public static bool isJoiningRemoteInvite;

		public static LocalNetworkGamer gamer;

		public static List<SignedInGamer> gamersWhoReceivedInvite = new List<SignedInGamer>(4);

		public static List<SignedInGamer> gamersWaitingToJoinInvite = new List<SignedInGamer>(4);

		public static List<UI> gamersWaitingForPlayerId = new List<UI>(4);

		public static List<UI> gamersWaitingToSendSpawn = new List<UI>(4);

		public static List<UI> gamersWaitingToSpawn = new List<UI>(4);

		public static List<NetClient> ClientList = new List<NetClient>(7);

		public static bool[] playerSlots = new bool[8];

		public static List<JoinableSession> availableSessions = new List<JoinableSession>(32);

		private static Thread sessionFinderThread = null;

		private static volatile bool stopSessionFinderThread = false;

		public static NetworkSession Session;

		public static InviteAcceptedEventArgs CurInvite;

		public static Thread sessionThread;

		public static AutoResetEvent SessionReadyEvent = new AutoResetEvent(initialState: false);

		public static ClientState clientState;

		public static int clientStatusCount;

		public static int clientStatusMax;

		public static void Init()
		{
			PlayDisconnect = false;
			HasHookEvents = false;
			ToStopSession = false;
			clientState = ClientState.JOINING;
			clientStatusCount = 0;
			clientStatusMax = 0;
		}

		public static void ResetSections()
		{
			for (int num = ClientList.Count - 1; num >= 0; num--)
			{
				ClientList[num].ResetSections();
			}
		}

		public static void ResetSections(ref Vector2i min, ref Vector2i max)
		{
			for (int num = ClientList.Count - 1; num >= 0; num--)
			{
				ClientList[num].ResetSections(ref min, ref max);
			}
		}

		public static void CreateSession()
		{
			NetworkSessionType networkSessionType = ((Main.NetMode != (byte)NetModeSetting.LOCAL) ? NetworkSessionType.PlayerMatch : NetworkSessionType.Local);
			try
			{
				List<SignedInGamer> list = new List<SignedInGamer>(1);
				list.Add(UI.MainUI.SignedInGamer);
				NetworkSessionProperties networkSessionProperties = new NetworkSessionProperties();
#if USE_ORIGINAL_CODE
				ulong xuid = UI.MainUI.SignedInGamer.GetXuid();
#else
				ulong xuid = 7934076125; // The Microsoft sample since the function is not implemented
#endif

				if (UI.MainUI.IsInviteOnly)
				{
                    networkSessionProperties[2] = -559038737;
                }
                else
                {
                    networkSessionProperties[0] = (int)xuid;
                    networkSessionProperties[1] = (int)(xuid >> 32);
#if (!VERSION_INITIAL || IS_PATCHED)
					networkSessionProperties[2] = 2;
#endif
				}
				int num = 8;
				int num2 = 0;
				if (!Main.IsTutorial() && UI.MainUI.IsInviteOnly)
				{
					num2 = 7;
				}
				Session = NetworkSession.Create(networkSessionType, list, num, num2, networkSessionProperties);
				Session.AllowJoinInProgress = true;
				Session.AllowHostMigration = false;
				HasHookEvents = true;
				Session.StartGame();
			}
			catch (Exception)
			{
				UI.Error(Lang.MenuText[5], Lang.InterfaceText[20]);
				UI.MainUI.CurMenuType = MenuType.MAIN;
				Main.NetMode = (byte)NetModeSetting.LOCAL;
				PlayDisconnect = true;
			}
		}

		public static void HookSessionEvents()
		{
#if USE_ORIGINAL_CODE
			Session.add_GamerJoined((EventHandler<GamerJoinedEventArgs>)GamerJoinedEventHandler);
			Session.add_GamerLeft((EventHandler<GamerLeftEventArgs>)GamerLeftEventHandler);
			Session.add_GameEnded((EventHandler<GameEndedEventArgs>)GameEndedEventHandler);
			Session.add_SessionEnded((EventHandler<NetworkSessionEndedEventArgs>)SessionEndedEventHandler);
#else
			Session.GamerJoined += GamerJoinedEventHandler;
            Session.GamerLeft += GamerLeftEventHandler;
            Session.GameEnded += GameEndedEventHandler;
            Session.SessionEnded += SessionEndedEventHandler;
#endif
			HasHookEvents = false;
		}

		public static void DisposeSession()
		{
#if USE_ORIGINAL_CODE
			Session.remove_GamerJoined((EventHandler<GamerJoinedEventArgs>)GamerJoinedEventHandler);
			Session.remove_GamerLeft((EventHandler<GamerLeftEventArgs>)GamerLeftEventHandler);
			Session.remove_GameEnded((EventHandler<GameEndedEventArgs>)GameEndedEventHandler);
			Session.remove_SessionEnded((EventHandler<NetworkSessionEndedEventArgs>)SessionEndedEventHandler);
#else
            Session.GamerJoined -= GamerJoinedEventHandler;
            Session.GamerLeft -= GamerLeftEventHandler;
            Session.GameEnded -= GameEndedEventHandler;
            Session.SessionEnded -= SessionEndedEventHandler;
#endif
            Session.Dispose();
			Session = null;
		}

		public static void Disconnect()
		{
			if (Session != null)
			{
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					if (Session.SessionState == NetworkSessionState.Playing)
					{
						Session.EndGame();
						Session.Update();
					}
					ClientList.Clear();
					for (int num = 7; num >= 0; num--)
					{
						playerSlots[num] = false;
					}
				}
				DisposeSession();
				gamer = null;
				gamersWaitingForPlayerId.Clear();
				gamersWaitingToSendSpawn.Clear();
				gamersWaitingToSpawn.Clear();
				for (int i = 0; i < 4; i++)
				{
					Main.UIInstance[i].LeaveSession();
				}
			}
			PlayDisconnect = false;
			HasHookEvents = false;
			ToStopSession = false;
			Main.NetMode = (byte)NetModeSetting.LOCAL;
		}

		public static void SetAsRemotePlayerSlot(int i)
		{
			Player player = Main.PlayerSet[i];
			UI ui = player.ui;
			if (ui != null)
			{
				if (player.CurrentView != null)
				{
					ui.setPlayer(-1);
				}
				else
				{
					ui.SetPlayer(null);
				}
			}
		}

		private static void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
		{
			NetworkGamer networkGamer = e.Gamer;
			if (Main.NetMode == (byte)NetModeSetting.LOCAL)
			{
				for (int i = 0; i < 4; i++)
				{
					UI uI = Main.UIInstance[i];
					if (uI.wasRemovedFromSessionWithoutOurConsent)
					{
						SignedInGamer signedInGamer = uI.SignedInGamer;
						if (signedInGamer != null && signedInGamer.Gamertag == networkGamer.Gamertag)
						{
							networkGamer.Tag = uI.ActivePlayer;
							return;
						}
					}
				}
			}
			else if (!networkGamer.IsLocal)
			{
				Main.CheckUserGeneratedContent = true;
			}
			int j = 0;
			Player player = null;
			if (Main.NetMode != (byte)NetModeSetting.CLIENT)
			{
				for (; playerSlots[j]; j++)
				{
				}
				playerSlots[j] = true;
			}
			if (networkGamer.IsLocal)
			{
				LocalNetworkGamer localNetworkGamer = (LocalNetworkGamer)networkGamer;
				SignedInGamer signedInGamer2 = localNetworkGamer.SignedInGamer;
				UI uI2 = Main.UIInstance[(int)signedInGamer2.PlayerIndex];
				uI2.localGamer = localNetworkGamer;
				if (Main.NetMode != (byte)NetModeSetting.CLIENT)
				{
					uI2.JoinSession(j);
					player = (Player)(networkGamer.Tag = Main.PlayerSet[j]);
					player.client = null;
					if (networkGamer.IsHost)
					{
						SessionReadyEvent.Set();
					}
					else
					{
						Main.JoinGame(uI2);
					}
				}
				else
				{
					gamersWaitingForPlayerId.Add(uI2);
				}
				if (gamer == null)
				{
					gamer = localNetworkGamer;
				}
			}
			else if (Main.NetMode == (byte)NetModeSetting.SERVER)
			{
				SetAsRemotePlayerSlot(j);
				NetClient netClient = null;
				for (int num = ClientList.Count - 1; num >= 0; num--)
				{
					if (ClientList[num].Machine == networkGamer.Machine)
					{
						netClient = ClientList[num];
						break;
					}
				}
				if (netClient == null)
				{
					netClient = new NetClient(networkGamer);
					ClientList.Add(netClient);
					NetMessage.syncPlayers();
				}
				player = Main.PlayerSet[j];
				netClient.GamerJoined(player);
				networkGamer.Tag = player;
				NetMessage.SendPlayerId(networkGamer, j);
			}
			else if (Main.NetMode == (byte)NetModeSetting.CLIENT && gamer != null)
			{
				NetMessage.CreateMessage0(11);
				NetMessage.SendMessage();
			}
		}

		private static void GamerLeftEventHandler(object sender, GamerLeftEventArgs e)
		{
			NetworkGamer networkGamer = e.Gamer;
			Player player = networkGamer.Tag as Player;
			if (networkGamer.IsLocal)
			{
				UI uI = Main.UIInstance[(int)((LocalNetworkGamer)networkGamer).SignedInGamer.PlayerIndex];
				if (Main.NetMode == (byte)NetModeSetting.LOCAL && uI.wasRemovedFromSessionWithoutOurConsent)
				{
					return;
				}
				uI.LeaveSession();
			}
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				return;
			}
			int whoAmI = player.WhoAmI;
			player.Active = 0;
			playerSlots[whoAmI] = false;
			if (Main.NetMode != (byte)NetModeSetting.SERVER)
			{
				return;
			}
			if (networkGamer.IsLocal)
			{
				if (UI.MainUI.IsInviteOnly)
				{
					Session.PrivateGamerSlots++;
				}
			}
			else
			{
				NetClient client = player.client;
				if (client.GamerLeft(player))
				{
					ClientList.Remove(client);
				}
				else if (networkGamer == client.NetGamer)
				{
					client.NetGamer = ((ReadOnlyCollection<NetworkGamer>)(object)client.Machine.Gamers)[0];
				}
			}
			NetMessage.CreateMessage2(14, whoAmI, 0);
			NetMessage.SendMessage();
			if (player.IsAnnounced)
			{
				player.IsAnnounced = false;
				NetMessage.SendText(player.oldName, 33, 255, 240, 20, -1);
			}
		}

		private static void GameEndedEventHandler(object sender, GameEndedEventArgs e)
		{
			if (Main.NetMode == (byte)NetModeSetting.CLIENT)
			{
				UI.Error(Lang.InterfaceText[66], Lang.InterfaceText[67]);
				Main.SaveOnExit = UI.MainUI.autoSave;
				UI.MainUI.ExitGame();
			}
		}

		private static void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
		{
			if (UI.MainUI.CurMenuMode != MenuMode.ERROR)
			{
				string desc;
				switch (e.EndReason)
				{
				case NetworkSessionEndReason.ClientSignedOut:
					GameEndedEventHandler(sender, new GameEndedEventArgs());
					return;
				case NetworkSessionEndReason.HostEndedSession:
				case NetworkSessionEndReason.RemovedByHost:
					desc = Lang.WorldGenText[46];
					break;
				default:
					desc = ((Main.NetMode != (byte)NetModeSetting.CLIENT) ? Lang.InterfaceText[36] : Lang.WorldGenText[46]);
					break;
				}
				UI.Error(Lang.MenuText[5], desc);
				Main.SaveOnExit = UI.MainUI.autoSave;
				UI.MainUI.ExitGame();
			}
		}
		public static void StartServer()
		{
			SessionReadyEvent.Reset();
			Thread thread = new Thread(ServerLoop);
			thread.IsBackground = true;
			thread.Start();
			sessionThread = thread;
        }

		public static void ServerLoop()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
            Init();
			CreateSession();
			while (!PlayDisconnect && Session != null)
			{
				Thread.Sleep(16);
			}
			ToStopSession = true;
			sessionThread = null;
		}

		public static void StartClient()
		{
			Thread thread = new Thread(ClientLoop);
			thread.IsBackground = true;
			thread.Start();
			sessionThread = thread;
		}

		public static void ClientLoop()
		{
#if USE_ORIGINAL_CODE
			Thread.CurrentThread.SetProcessorAffinity(new int[1]
			{
				5
			});
#endif
			Init();
			UI.MainUI.ActivePlayer.hostile = false;
			UI.MainUI.ActivePlayer.NetClone(UI.MainUI.netPlayer);
			for (int i = 0; i < 8; i++)
			{
				if (i != UI.MainUI.MyPlayer)
				{
					Main.PlayerSet[i].Active = 0;
				}
			}

#if (!VERSION_INITIAL || IS_PATCHED)
			Main.MaxTilesX = Main.SmallWorldW;
			Main.MaxTilesY = Main.SmallWorldH;
#endif

			WorldGen.clearWorld();
			if (UI.MainUI.CurMenuMode == MenuMode.NETPLAY)
			{
				Main.NetMode = (byte)NetModeSetting.CLIENT;
#if (!VERSION_INITIAL || IS_PATCHED)
				WorldSelect.isLocalWorld = false;
#endif
				try
				{
					if (isJoiningRemoteInvite)
					{
						isJoiningRemoteInvite = false;
						Session = NetworkSession.JoinInvited(gamersWaitingToJoinInvite);
						gamersWaitingToJoinInvite.Clear();
						gamersWhoReceivedInvite.Clear();
					}
					else
					{
						Session = NetworkSession.Join(WorldSelect.Session());
					}
				}
				catch (Exception)
				{
					UI.Error(Lang.MenuText[5], Lang.InterfaceText[21], rememberPreviousMenu: true);
					UI.MainUI.CurMenuType = MenuType.MAIN;
					Main.NetMode = (byte)NetModeSetting.LOCAL;
					PlayDisconnect = true;
					ToStopSession = true;
					goto IL_028d;
				}
				HasHookEvents = true;
				while (!PlayDisconnect && Session != null)
				{
					switch (clientState)
					{
					case ClientState.JOINING:
						UI.MainUI.FirstProgressStep(3, Lang.MenuText[8]);
						clientState = ClientState.WAITING_FOR_PLAYER_ID;
						break;
					case ClientState.WAITING_FOR_PLAYER_ID:
						if (UI.MainUI.Progress <= 0.999f)
						{
							UI.MainUI.Progress = UI.MainUI.Progress + 0.001f;
						}
						break;
					case ClientState.WAITING_FOR_PLAYER_DATA_REQ:
						if (UI.MainUI.Progress <= 0.999f)
						{
							UI.MainUI.Progress = UI.MainUI.Progress + 0.001f;
						}
						break;
					case ClientState.RECEIVED_PLAYER_DATA_REQ:
						UI.MainUI.NextProgressStep(Lang.MenuText[73]);
						clientState = ClientState.WAITING_FOR_WORLD_INFO;
						break;
					case ClientState.WAITING_FOR_WORLD_INFO:
						if (UI.MainUI.Progress <= 0.999f)
						{
							UI.MainUI.Progress = UI.MainUI.Progress + 0.001f;
						}
						break;
					case ClientState.WAITING_FOR_TILE_DATA:
						if (clientStatusMax > 0)
						{
							if (clientStatusCount >= clientStatusMax)
							{
								clientStatusMax = 0;
								clientStatusCount = 0;
								UI.MainUI.Progress = 1f;
							}
							else
							{
								UI.MainUI.statusText = Lang.InterfaceText[44];
								UI.MainUI.Progress = clientStatusCount / (float)clientStatusMax;
							}
						}
						break;
					}
					Thread.Sleep(0);
				}
				clientStatusCount = 0;
				clientStatusMax = 0;
				ToStopSession = true;
			}
			goto IL_028d;
			IL_028d:
			sessionThread = null;
		}

		public static void InviteAccepted()
		{
			PlayerIndex playerIndex = CurInvite.Gamer.PlayerIndex;
			if (Main.IsTrial)
			{
				MessageBox.Show(playerIndex, Lang.MenuText[5], Lang.InterfaceText[69], new string[1]
				{
					Lang.MenuText[90]
				});
			}
			else if (!UI.AllPlayersCanPlayOnline())
			{
				MessageBox.Show(playerIndex, Lang.MenuText[5], Lang.InterfaceText[68], new string[1]
				{
					Lang.MenuText[90]
				});
			}
			else
			{
				UI uI = Main.UIInstance[(int)playerIndex];
				uI.InviteAccepted(CurInvite);
			}
			CurInvite = null;
		}

		public static void NetworkSession_InviteAccepted(object sender, InviteAcceptedEventArgs e)
		{
			if (CurInvite == null)
			{
				CurInvite = e;
			}
		}

		public static bool IsFindingSessions()
		{
			return sessionFinderThread != null;
		}

		public static void StopFindingSessions()
		{
			availableSessions.Clear();
			if (sessionFinderThread != null)
			{
				stopSessionFinderThread = true;
			}
		}

		public static void FindSessions()
		{
			StopFindingSessions();
			if (UI.MainUI.CanPlayOnline())
			{
				if (sessionFinderThread != null)
				{
					sessionFinderThread.Join();
				}
				sessionFinderThread = new Thread(FindSessionsThread);
				sessionFinderThread.IsBackground = true;
				sessionFinderThread.Start();
			}
		}

		public static void FindSessionsThread()
		{
			NetworkSessionProperties networkSessionProperties = new NetworkSessionProperties();
#if (!VERSION_INITIAL || IS_PATCHED)
            networkSessionProperties[2] = 2;
#endif
			UI main = UI.MainUI;
			if (main.HasOnline())
			{
				SignedInGamer signedInGamer = main.SignedInGamer;
				if (signedInGamer != null)
				{
					try
					{
						List<SignedInGamer> list = new List<SignedInGamer>(1);
						list.Add(signedInGamer);
						FriendCollection friends = signedInGamer.GetFriends();
						int num = ((ReadOnlyCollection<FriendGamer>)(object)friends).Count - 1;
						while (num >= 0 && !stopSessionFinderThread)
						{
							FriendGamer friendGamer = ((ReadOnlyCollection<FriendGamer>)(object)friends)[num];
							if (friendGamer.IsJoinable)
							{
#if USE_ORIGINAL_CODE
								ulong xuid = friendGamer.GetXuid();
#else
                                ulong xuid = (ulong)(7934076125 + num); // The Microsoft sample since the function is not implemented
#endif
                                networkSessionProperties[0] = (int)xuid;
								networkSessionProperties[1] = (int)(xuid >> 32);

                                AvailableNetworkSessionCollection availableNetworkSessionCollection = NetworkSession.Find(NetworkSessionType.PlayerMatch, list, networkSessionProperties);
								if (((ReadOnlyCollection<AvailableNetworkSession>)(object)availableNetworkSessionCollection).Count > 0)
								{
									lock (availableSessions)
									{
										availableSessions.Add(new JoinableSession(((ReadOnlyCollection<AvailableNetworkSession>)(object)availableNetworkSessionCollection)[0]));
									}
								}
								if (stopSessionFinderThread)
								{
									break;
								}
								Thread.Sleep(JoinableSession.SearchDelay);
							}
                            num--;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			stopSessionFinderThread = false;
			sessionFinderThread = null;
		}

		public static void CheckOfflineSession()
		{
			for (int i = 0; i < 4; i++)
			{
				UI uI = Main.UIInstance[i];
				if (uI.wasRemovedFromSessionWithoutOurConsent)
				{
					if (Session.SessionState == NetworkSessionState.Playing)
					{
						Session.AddLocalGamer(uI.SignedInGamer);
						Session.Update();
						uI.wasRemovedFromSessionWithoutOurConsent = false;
					}
					else if (uI == UI.MainUI)
					{
						DisposeSession();
						CreateSession();
						HookSessionEvents();
						uI.wasRemovedFromSessionWithoutOurConsent = false;
					}
				}
			}
		}
	}
}
