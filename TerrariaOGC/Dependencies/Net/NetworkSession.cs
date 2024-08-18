#region License
/* FNA.NetStub - XNA4 Xbox Live Stub DLL
 * Copyright 2019 Ethan "flibitijibibo" Lee
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public sealed class NetworkSession : IDisposable
	{
		#region Public Constants

		public const int MaxSupportedGamers = 31; // FIXME: ???
		public const int MaxPreviousGamers = 100; // FIXME: ???

		#endregion

		#region Network Event Structure

		internal enum NetworkEventType
		{
			PacketSend,
			GamerJoin,
			GamerLeave,
			HostChange,
			StateChange
		}

		internal struct NetworkEvent
		{
			public NetworkEventType Type;

			public NetworkGamer Gamer;
			public byte[] Packet;
			public SendDataOptions Reliable;
			public NetworkSessionState State;
			public NetworkSessionEndReason Reason;
		}

		#endregion

		#region Public Properties

		public bool IsDisposed
		{
			get;
			private set;
		}

		public GamerCollection<NetworkGamer> AllGamers
		{
			get;
			private set;
		}

		public GamerCollection<LocalNetworkGamer> LocalGamers
		{
			get;
			private set;
		}

		public GamerCollection<NetworkGamer> RemoteGamers
		{
			get;
			private set;
		}

		public GamerCollection<NetworkGamer> PreviousGamers
		{
			get;
			private set;
		}

		public bool AllowHostMigration
		{
			get;
			set;
		}

		public bool AllowJoinInProgress
		{
			get;
			set;
		}

		public int BytesPerSecondReceived
		{
			get;
			private set;
		}

		public int BytesPerSecondSent
		{
			get;
			private set;
		}

		public NetworkGamer Host
		{
			get;
			private set;
		}

		public bool IsEveryoneReady
		{
			get
			{
				foreach (LocalNetworkGamer gamer in LocalGamers)
				{
					if (!gamer.IsReady)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool IsHost
		{
			get
			{
				foreach (LocalNetworkGamer gamer in LocalGamers)
				{
					if (gamer.IsHost)
					{
						return true;
					}
				}
				return false;
			}
		}

		public int MaxGamers
		{
			// ArgumentOutOfRangeException: MaxGamers must be between 2 and 31 players for Windows-based and Xbox 360-based games.
			// InvalidOperationException: This session is not the host, and cannot change the value of MaxGamers.
			// ObjectDisposedException: The session has been disposed.
			get;
			set;
		}

		public int PrivateGamerSlots
		{
			// ArgumentOutOfRangeException: There are not enough slots available to support this new value.
			// InvalidOperationException: This session is not the host, and cannot change the value of PrivateGamerSlots.
			// ObjectDisposedException: The session has been disposed.
			get;
			set;
		}

		public NetworkSessionProperties SessionProperties
		{
			get;
			private set;
		}

		public NetworkSessionState SessionState
		{
			get;
			private set;
		}

		public NetworkSessionType SessionType
		{
			get;
			private set;
		}

		public TimeSpan SimulatedLatency
		{
			get;
			set;
		}

		public float SimulatedPacketLoss
		{
			get;
			set;
		}

		public delegate NetworkSession NetworkSessionAsynchronousCreate(
		NetworkSessionType sessionType,// Type of session being hosted.
		int maxLocalGamers,// Maximum number of local players on the same gaming machine in this network session.
		int maxGamers,      // Maximum number of players allowed in this network session.  For Zune-based games, this value must be between 2 and 8; 8 is the maximum number of players supported in the session.
		int privateGamerSlots, // Number of reserved private session slots created for the session. This value must be less than maximumGamers. 
		NetworkSessionProperties sessionProperties, // Properties of the session being created.
		int hostGamer,      // Gamer Index of the host
		bool isHost // If the session is for host or not 
	);

        #endregion

        #region Private Variables

        private int maxLocalGamers;

		private Queue<NetworkEvent> networkEvents;
        internal static List<NetworkSession> activeSessions = new List<NetworkSession>();

        private GamerCollection<NetworkGamer> _allGamers;
        private GamerCollection<LocalNetworkGamer> _localGamers;
        private GamerCollection<NetworkGamer> _remoteGamers;
        private GamerCollection<NetworkGamer> _previousGamers;
        private NetworkGamer hostingGamer;
        internal Queue<CommandEvent> commandQueue;
        internal MonoGamerPeer networkPeer;

        internal static AsyncCallback actCallback;
        internal static int actMaxLocal;
        internal static IEnumerable<SignedInGamer> actGamers;
        internal static int actMaxPriv;
        internal static NetworkSessionProperties actProp;
        internal static NetworkSessionType actType;

        #endregion

        #region Private Static Variables

        private static NetworkSessionAction activeAction = null;

		private static NetworkSession activeSession = null;

		#endregion

		#region Public Events

		public event EventHandler<GameStartedEventArgs> GameStarted;

		public event EventHandler<GameEndedEventArgs> GameEnded;

		public event EventHandler<GamerJoinedEventArgs> GamerJoined;

		public event EventHandler<GamerLeftEventArgs> GamerLeft;

		public event EventHandler<HostChangedEventArgs> HostChanged;

		public event EventHandler<NetworkSessionEndedEventArgs> SessionEnded;

// TODO: Leaderboards/TrueSkill
#pragma warning disable 0067
		public event EventHandler<WriteLeaderboardsEventArgs> WriteArbitratedLeaderboard;

		public event EventHandler<WriteLeaderboardsEventArgs> WriteUnarbitratedLeaderboard;

		public event EventHandler<WriteLeaderboardsEventArgs> WriteTrueSkill;
#pragma warning restore 0067

		#endregion

		#region Public Static Events

#pragma warning disable 0067
		public static event EventHandler<InviteAcceptedEventArgs> InviteAccepted;
#pragma warning restore 0067

		#endregion

		#region Async Object Type

		internal class NetworkSessionAction : IAsyncResult
		{
			public object AsyncState
			{
				get;
				private set;
			}

			public bool CompletedSynchronously
			{
				get
				{
					return false;
				}
			}

			public bool IsCompleted
			{
				get;
				internal set;
			}

			public WaitHandle AsyncWaitHandle
			{
				get;
				private set;
			}

			public readonly AsyncCallback Callback;

			public readonly int MaxLocalGamers;
			public readonly IEnumerable<SignedInGamer> LocalGamers;
			public readonly int MaxPrivateSlots;
			public readonly NetworkSessionProperties SessionProperties;
			public readonly NetworkSessionType SessionType;

			public NetworkSessionAction(
				object state,
				AsyncCallback callback,
				int maxLocal,
				IEnumerable<SignedInGamer> localGamers,
				int maxPrivateSlots,
				NetworkSessionProperties properties,
				NetworkSessionType type
			) {
				AsyncState = state;
				Callback = callback;
				IsCompleted = false;
				AsyncWaitHandle = new ManualResetEvent(true);

				MaxLocalGamers = maxLocal;
				LocalGamers = localGamers;
				MaxPrivateSlots = maxPrivateSlots;
				SessionProperties = properties;
				SessionType = type;
			}
		}

		#endregion

		#region Internal Constructor

		internal NetworkSession(
			NetworkSessionProperties properties,
			NetworkSessionType type,
			int maxGamers,
			int privateGamerSlots,
			int maxLocal,
			IEnumerable<SignedInGamer> localGamers
		) {
			SessionProperties = properties;
			SessionType = type;
			MaxGamers = maxGamers;
			PrivateGamerSlots = privateGamerSlots;

			// Create Gamer lists

			List<LocalNetworkGamer> locals = new List<LocalNetworkGamer>();
			if (localGamers == null)
			{
				// FIXME: Check for mismatch in SignedInGamers Count -flibit
				maxLocalGamers = maxLocal;
				for (int i = 0; i < Gamer.SignedInGamers.Count && i < maxLocalGamers; i += 1)
				{
					// FIXME: Guests are fake! -flibit
					if (!Gamer.SignedInGamers[i].IsGuest)
					{
						locals.Add(new LocalNetworkGamer(
							Gamer.SignedInGamers[i],
							this
						));
					}
				}
			}
			else
			{
				maxLocalGamers = 0;
				foreach (SignedInGamer gamer in localGamers)
				{
					locals.Add(new LocalNetworkGamer(gamer, this));
					maxLocalGamers += 1;
				}
			}
			LocalGamers = new GamerCollection<LocalNetworkGamer>(locals);

			List<NetworkGamer> remoteGamers = new List<NetworkGamer>();
			RemoteGamers = new GamerCollection<NetworkGamer>(remoteGamers);

			List<NetworkGamer> allGamers = new List<NetworkGamer>();
			allGamers.AddRange(remoteGamers);
			allGamers.AddRange(locals);
			AllGamers = new GamerCollection<NetworkGamer>(allGamers);

			PreviousGamers = new GamerCollection<NetworkGamer>(
				new List<NetworkGamer>()
			);

			// Create host data

			Host = LocalGamers[0];

			if (IsHost)
			{
				AllowHostMigration = false;
				AllowJoinInProgress = false;
				SessionState = NetworkSessionState.Lobby;
			}

			// Event hookups
			networkEvents = new Queue<NetworkEvent>();
			foreach (NetworkGamer gamer in AllGamers)
			{
				NetworkSession.NetworkEvent evt = new NetworkEvent()
				{
					Type = NetworkEventType.GamerJoin,
					Gamer = gamer
				};
				SendNetworkEvent(evt);
			}

			// Other defaults

			SimulatedLatency = TimeSpan.Zero;
			SimulatedPacketLoss = 0.0f;
			IsDisposed = false;

			// TODO: Everything below
			BytesPerSecondReceived = 0;
			BytesPerSecondSent = 0;
		}

        private NetworkSession()
        {
            activeSessions.Add(this);
        }

        private NetworkSession(NetworkSessionType sessionType, int maxGamers, int privateGamerSlots, NetworkSessionProperties sessionProperties, bool isHost, int hostGamer, AvailableNetworkSession availableSession) : this()
        {
            if (sessionProperties == null)
            {
                throw new ArgumentNullException("sessionProperties");
            }

            _allGamers = new GamerCollection<NetworkGamer>();
            _localGamers = new GamerCollection<LocalNetworkGamer>();
            //			for (int x = 0; x < Gamer.SignedInGamers.Count; x++) {
            //				GamerStates states = GamerStates.Local;
            //				if (x == 0)
            //					states |= GamerStates.Host;
            //				LocalNetworkGamer localGamer = new LocalNetworkGamer(this, (byte)x, states);
            //				localGamer.SignedInGamer = Gamer.SignedInGamers[x];
            //				_allGamers.AddGamer(localGamer);
            //				_localGamers.AddGamer(localGamer);
            //				
            //				// We will attach a property change handler to local gamers
            //				//  se that we can broadcast the change to other peers.
            //				localGamer.PropertyChanged += HandleGamerPropertyChanged;	
            //				
            //			}

            _remoteGamers = new GamerCollection<NetworkGamer>();
            _previousGamers = new GamerCollection<NetworkGamer>();
            hostingGamer = null;

            commandQueue = new Queue<CommandEvent>();

            //this.sessionType = sessionType;
            //this.maxGamers = maxGamers;
            //this.privateGamerSlots = privateGamerSlots;
            //this.sessionProperties = sessionProperties;
            //this.isHost = isHost;
            if (isHost)
                networkPeer = new MonoGamerPeer(this, null);
            else
            {
                if (networkPeer == null)
                    networkPeer = new MonoGamerPeer(this, availableSession);
            }

            CommandGamerJoined gj = new CommandGamerJoined(hostGamer, isHost, true);
            commandQueue.Enqueue(new CommandEvent(gj));
        }

        private NetworkSession(NetworkSessionType sessionType, int maxGamers, int privateGamerSlots, NetworkSessionProperties sessionProperties, bool isHost, int hostGamer)
            : this(sessionType, maxGamers, privateGamerSlots, sessionProperties, isHost, hostGamer, null)
        {
        }

        #endregion

        #region Public Methods

        public void Dispose()
		{
			// Flush packets
			foreach (LocalNetworkGamer gamer in LocalGamers)
			{
				gamer.packetQueue.Clear();
			}

			activeSession = null;
			IsDisposed = true;
		}

		public void Update()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("this");
			}

			while (networkEvents.Count > 0)
			{
				NetworkEvent evt = networkEvents.Dequeue();

				if (evt.Type == NetworkEventType.PacketSend)
				{
				}
				else if (evt.Type == NetworkEventType.GamerJoin)
				{
					if (GamerJoined != null)
					{
						GamerJoined(
							this,
							new GamerJoinedEventArgs(evt.Gamer)
						);
					}
				}
				else if (evt.Type == NetworkEventType.GamerLeave)
				{
					if (GamerLeft != null)
					{
						GamerLeft(
							this,
							new GamerLeftEventArgs(evt.Gamer)
						);
					}
				}
				else if (evt.Type == NetworkEventType.HostChange)
				{
					if (HostChanged != null)
					{
						HostChanged(
							this,
							new HostChangedEventArgs(
								Host,
								evt.Gamer
							)
						);
					}

					// FIXME: Is the timing on this accurate? -flibit
					Host = evt.Gamer;
				}
				else if (evt.Type == NetworkEventType.StateChange)
				{
					if (evt.State == NetworkSessionState.Playing)
					{
						if (GameStarted != null)
						{
							GameStarted(this, new GameStartedEventArgs());
						}
					}
					else if (evt.State == NetworkSessionState.Lobby)
					{
						if (GameEnded != null)
						{
							GameEnded(this, new GameEndedEventArgs());
						}
					}
					else // if (evt.State == NetworkSessionState.Ended)
					{
						SessionEnded(
							this,
							new NetworkSessionEndedEventArgs(evt.Reason)
						);
					}

					// FIXME: Is the timing on this accurate? -flibit
					SessionState = evt.State;
				}
			}
		}

		public void AddLocalGamer(SignedInGamer gamer)
		{
			if (LocalGamers.Count == maxLocalGamers)
			{
				throw new InvalidOperationException("LocalGamer max limit!");
			}
			LocalNetworkGamer adding = new LocalNetworkGamer(gamer, this);
			LocalGamers.collection.Add(adding);
			AllGamers.collection.Add(adding);
		}

		public NetworkGamer FindGamerById(byte gameId)
		{
			foreach (NetworkGamer g in AllGamers)
			{
				if (g.Id == gameId)
				{
					return g;
				}
			}
			return null;
		}

		public void ResetReady()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("this");
			}
			if (!IsHost)
			{
				throw new InvalidOperationException("This NetworkSession is not the host");
			}

			foreach (NetworkGamer gamer in AllGamers)
			{
				gamer.IsReady = false;
			}
		}

		public void StartGame()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("this");
			}
			if (!IsHost)
			{
				throw new InvalidOperationException("This NetworkSession is not the host");
			}
			if (SessionState != NetworkSessionState.Lobby)
			{
				throw new InvalidOperationException("NetworkSession is not Lobby");
			}

			NetworkEvent evt = new NetworkEvent()
			{
				Type = NetworkEventType.StateChange,
				State = NetworkSessionState.Playing
			};
			SendNetworkEvent(evt);
		}

		public void EndGame()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("this");
			}
			if (!IsHost)
			{
				throw new InvalidOperationException("This NetworkSession is not the host");
			}
			if (SessionState != NetworkSessionState.Playing)
			{
				throw new InvalidOperationException("NetworkSession is not Playing");
			}

			NetworkEvent evt = new NetworkEvent()
			{
				Type = NetworkEventType.StateChange,
				State = NetworkSessionState.Lobby
			};
			SendNetworkEvent(evt);
		}

		#endregion

		#region Internal Methods

		internal void SendNetworkEvent(NetworkEvent evt)
		{
			networkEvents.Enqueue(evt);
		}

		#endregion

		#region Public Static Create Methods

		public static NetworkSession Create(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			int maxGamers
		) {
			IAsyncResult result = BeginCreate(
				sessionType,
				maxLocalGamers,
				maxGamers,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndCreate(result);
		}

		public static NetworkSession Create(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			int maxGamers,
			int privateGamerSlots,
			NetworkSessionProperties sessionProperties
		) {
			IAsyncResult result = BeginCreate(
				sessionType,
				maxLocalGamers,
				maxGamers,
				privateGamerSlots,
				sessionProperties,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndCreate(result);
		}

		public static NetworkSession Create(
			NetworkSessionType sessionType,
			IEnumerable<SignedInGamer> localGamers,
			int maxGamers,
			int privateGamerSlots,
			NetworkSessionProperties sessionProperties
		) {
            IAsyncResult result = BeginCreate(
				sessionType,
				localGamers,
				maxGamers,
				privateGamerSlots,
				sessionProperties,
				null,
				null
			);
            /*while (!result.IsCompleted)		"And I would have gotten away with it too, if it weren't for you meddling breakpoints" - This dastardly while loop
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}*/
            return EndCreate(result);
        }

        private static NetworkSession Create(
            NetworkSessionType sessionType,
            int maxLocalGamers,
            int maxGamers,
            int privateGamerSlots,
            NetworkSessionProperties sessionProperties,
            int hostGamer,
            bool isHost)
        {

            NetworkSession session = null;

            try
            {
                if (sessionProperties == null)
                    sessionProperties = new NetworkSessionProperties();
                session = new NetworkSession(sessionType, maxGamers, privateGamerSlots, sessionProperties, isHost, hostGamer);

            }
            finally
            {
            }

            return session;
        }

        public static IAsyncResult BeginCreate(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			int maxGamers,
			AsyncCallback callback,
			object asyncState
		) {
			if (maxLocalGamers < 1 || maxLocalGamers > 4)
			{
				throw new ArgumentOutOfRangeException("maxLocalGamers");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				maxLocalGamers,
				null,
				0,
				new NetworkSessionProperties(),
				sessionType
			);
			return activeAction;
		}

		public static IAsyncResult BeginCreate(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			int maxGamers,
			int privateGamerSlots,
			NetworkSessionProperties sessionProperties,
			AsyncCallback callback,
			object asyncState
		) {
			if (maxLocalGamers < 1 || maxLocalGamers > 4)
			{
				throw new ArgumentOutOfRangeException("maxLocalGamers");
			}
			if (privateGamerSlots < 0 || privateGamerSlots > maxGamers)
			{
				throw new ArgumentOutOfRangeException("privateGamerSlots");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				maxLocalGamers,
				null,
				privateGamerSlots,
				sessionProperties,
				sessionType
			);
			return activeAction;
		}

		public static IAsyncResult BeginCreate(
			NetworkSessionType sessionType,
			IEnumerable<SignedInGamer> localGamers,
			int maxGamers,
			int privateGamerSlots,
			NetworkSessionProperties sessionProperties,
			AsyncCallback callback,
			object asyncState
		) {
            int hostGamerIdx = -1;

			SignedInGamer hostGamer = null;

            if (localGamers == null)
            {
                throw new ArgumentNullException("localGamers");
            }
            foreach (SignedInGamer gamer in localGamers)
            {
                if (gamer == null)
                {
                    throw new ArgumentException("gamer can not be null in list of localGamers.");
                }
                if (gamer.IsDisposed)
                {
                    throw new ObjectDisposedException("localGamers", "A gamer is disposed in the list of localGamers");
                }
                if (hostGamer == null)
                {
                    hostGamer = gamer;
                }
            }
            if (hostGamer == null)
            {
                throw new ArgumentException("Invalid gamer in localGamers.");
            }

            hostGamerIdx = (int)hostGamer.PlayerIndex;
			actGamers = localGamers;
            return BeginCreate(sessionType, hostGamerIdx, 4, maxGamers, privateGamerSlots, sessionProperties, callback, asyncState);

        }

        public static IAsyncResult BeginCreate(
			NetworkSessionType sessionType,
            int hostGamer,
            int maxLocalGamers,
            int maxGamers,
            int privateGamerSlots,
            NetworkSessionProperties sessionProperties,
            AsyncCallback callback,
            object asyncState)
        {
            if (maxLocalGamers < 1 || maxLocalGamers > 4)
                throw new ArgumentOutOfRangeException("Maximum local players must be between 1 and 4.");
            if (maxGamers < 2 || maxGamers > 32)
                throw new ArgumentOutOfRangeException("Maximum number of gamers must be between 2 and 32.");
            try
            {
                NetworkSessionAsynchronousCreate AsynchronousCreate = new NetworkSessionAsynchronousCreate(Create);
                return AsynchronousCreate.BeginInvoke(sessionType, maxLocalGamers, maxGamers, privateGamerSlots, sessionProperties, hostGamer, true, callback, asyncState);
            }
            finally
            {
				actMaxLocal = maxLocalGamers;
				actMaxPriv = privateGamerSlots;
				actProp = sessionProperties;
				actCallback = callback;
				actType = sessionType;
            }

        }

        public static NetworkSession EndCreate(IAsyncResult result)
		{
			if (result != activeAction)
			{
				//throw new ArgumentException("result");

                activeAction = new NetworkSessionAction(
                result.AsyncState,
                actCallback,
                actMaxLocal,
                actGamers,
                actMaxPriv,
                actProp,
                actType );
            }
			activeSession = new NetworkSession(
				activeAction.SessionProperties,
				activeAction.SessionType,
				69,
				activeAction.MaxPrivateSlots,
				activeAction.MaxLocalGamers,
				activeAction.LocalGamers
			);

			activeAction = null;
			return activeSession;
		}

		#endregion

		#region Public Static Find Methods

		public static AvailableNetworkSessionCollection Find(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			NetworkSessionProperties searchProperties
		) {
			IAsyncResult result = BeginFind(
				sessionType,
				maxLocalGamers,
				searchProperties,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndFind(result);
		}

		public static AvailableNetworkSessionCollection Find(
			NetworkSessionType sessionType,
			IEnumerable<SignedInGamer> localGamers,
			NetworkSessionProperties searchProperties
		) {
			IAsyncResult result = BeginFind(
				sessionType,
				localGamers,
				searchProperties,
				null,
				null
			);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndFind(result);
		}

		public static IAsyncResult BeginFind(
			NetworkSessionType sessionType,
			int maxLocalGamers,
			NetworkSessionProperties searchProperties,
			AsyncCallback callback,
			object asyncState
		) {
			if (sessionType == NetworkSessionType.Local)
			{
				throw new ArgumentException("sessionType");
			}
			if (maxLocalGamers < 1 || maxLocalGamers > 4)
			{
				throw new ArgumentOutOfRangeException("maxLocalGamers");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				maxLocalGamers,
				null,
				0,
				searchProperties,
				sessionType
			);
			return activeAction;
		}

		public static IAsyncResult BeginFind(
			NetworkSessionType sessionType,
			IEnumerable<SignedInGamer> localGamers,
			NetworkSessionProperties searchProperties,
			AsyncCallback callback,
			object asyncState
		) {
			if (sessionType == NetworkSessionType.Local)
			{
				throw new ArgumentException("sessionType");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			int locals = 0;
			foreach (SignedInGamer gamer in localGamers)
			{
				locals += 1;
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				locals,
				localGamers,
				0,
				searchProperties,
				sessionType
			);
			return activeAction;
		}

		public static AvailableNetworkSessionCollection EndFind(IAsyncResult result)
		{
			if (result != activeAction)
			{
				throw new ArgumentException("result");
			}

			List<AvailableNetworkSession> sessions = new List<AvailableNetworkSession>();

			activeAction = null;
			return new AvailableNetworkSessionCollection(sessions);
		}

		#endregion

		#region Public Static Join Methods

		public static NetworkSession Join(
			AvailableNetworkSession availableSession
		) {
			IAsyncResult result = BeginJoin(availableSession, null, null);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndJoin(result);
		}

		public static IAsyncResult BeginJoin(
			AvailableNetworkSession availableSession,
			AsyncCallback callback,
			object asyncState
		) {
			if (availableSession == null)
			{
				throw new ArgumentNullException("availableSession");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				4, // FIXME
				null,
				0,
				null,
				NetworkSessionType.PlayerMatch // FIXME
			);
			return activeAction;
		}

		public static NetworkSession EndJoin(IAsyncResult result)
		{
			if (result != activeAction)
			{
				throw new ArgumentException("result");
			}

			int actionMaxLocalGamers = activeAction.MaxLocalGamers;
			IEnumerable<SignedInGamer> actionLocalGamers = activeAction.LocalGamers;
			activeAction = null;

			activeSession = new NetworkSession(
				null, // FIXME
				NetworkSessionType.PlayerMatch, // FIXME
				MaxSupportedGamers, // FIXME
				4, // FIXME
				actionMaxLocalGamers,
				actionLocalGamers
			);
			return activeSession;
		}

		public static NetworkSession JoinInvited(
			int maxLocalGamers
		) {
			IAsyncResult result = BeginJoinInvited(maxLocalGamers, null, null);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndJoinInvited(result);
		}

		public static NetworkSession JoinInvited(
			IEnumerable<SignedInGamer> localGamers
		) {
			IAsyncResult result = BeginJoinInvited(localGamers, null, null);
			while (!result.IsCompleted)
			{
				if (!GamerServicesDispatcher.UpdateAsync())
				{
					activeAction.IsCompleted = true;
				}
			}
			return EndJoinInvited(result);
		}

		public static IAsyncResult BeginJoinInvited(
			int maxLocalGamers,
			AsyncCallback callback,
			object asyncState
		) {
			if (maxLocalGamers < 1 || maxLocalGamers > 4)
			{
				throw new ArgumentOutOfRangeException("maxLocalGamers");
			}
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				maxLocalGamers,
				null,
				0,
				null,
				NetworkSessionType.PlayerMatch // FIXME
			);
			return activeAction;
		}

		public static IAsyncResult BeginJoinInvited(
			IEnumerable<SignedInGamer> localGamers,
			AsyncCallback callback,
			object asyncState
		) {
			if (activeAction != null || activeSession != null)
			{
				throw new InvalidOperationException();
			}

			activeAction = new NetworkSessionAction(
				asyncState,
				callback,
				0,
				localGamers,
				0,
				null,
				NetworkSessionType.PlayerMatch // FIXME
			);
			return activeAction;
		}

		public static NetworkSession EndJoinInvited(IAsyncResult result)
		{
			if (result != activeAction)
			{
				throw new ArgumentException("result");
			}

			int actionMaxLocalGamers = activeAction.MaxLocalGamers;
			IEnumerable<SignedInGamer> actionLocalGamers = activeAction.LocalGamers;
			activeAction = null;

			activeSession = new NetworkSession(
				null, // FIXME
				NetworkSessionType.PlayerMatch, // FIXME
				MaxSupportedGamers, // FIXME
				4, // FIXME
				actionMaxLocalGamers,
				actionLocalGamers
			);
			return activeSession;
		}

		#endregion
	}
}
