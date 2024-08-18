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
using System.Collections.Generic;

using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Microsoft.Xna.Framework.Net
{
	public sealed class LocalNetworkGamer : NetworkGamer
	{
		#region Public Properties

		public bool IsDataAvailable
		{
			get
			{
				return packetQueue.Count > 0;
			}
		}

		public SignedInGamer SignedInGamer
		{
			get;
			private set;
		}

		#endregion

		#region Internal Variables

		internal Queue<NetworkSession.NetworkEvent> packetQueue;

		#endregion

		#region Internal Constructor

		internal LocalNetworkGamer(
			SignedInGamer gamer,
			NetworkSession session
		) : base(
			session
		) {
			SignedInGamer = gamer;

			packetQueue = new Queue<NetworkSession.NetworkEvent>();
		}

		#endregion

		#region Public Methods

		public void EnableSendVoice(NetworkGamer remoteGamer, bool enable)
		{
			// TODO: Actual stuff?! -flibit
		}

		public void SendPartyInvites()
		{
			// TODO: Actual stuff?! -flibit
		}

		public int ReceiveData(byte[] data, out NetworkGamer gamer)
		{
			return ReceiveData(data, 0, out gamer);
		}

		public int ReceiveData(byte[] data, int offset, out NetworkGamer sender)
		{
			sender = null;
			if (!IsDataAvailable)
			{
				return 0;
			}

			NetworkSession.NetworkEvent packet = packetQueue.Dequeue();
			int len = Math.Min(packet.Packet.Length, data.Length);
			Array.Copy(packet.Packet, 0, data, offset, len);

			foreach (NetworkGamer gamer in Session.AllGamers)
			{
				// FIXME: This is a bad equality check!
				if (gamer == packet.Gamer)
				{
					sender = gamer;
					return len;
				}
			}

			// We should never get here!
			return len;
		}

		public int ReceiveData(PacketReader data, out NetworkGamer sender)
		{
			sender = null;
			if (!IsDataAvailable)
			{
				return 0;
			}

			uint len = 0;
			NetworkSession.NetworkEvent packet = packetQueue.Dequeue();
			data.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
			data.BaseStream.Write(packet.Packet, 0, packet.Packet.Length);
			data.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

			foreach (NetworkGamer gamer in Session.AllGamers)
			{
				// FIXME: This is a bad equality check!
				if (gamer == packet.Gamer)
				{
					sender = gamer;
					return (int) len;
				}
			}

			// We should never get here!
			return (int) len;
		}

		public void SendData(byte[] data, SendDataOptions options)
		{
			SendData(data, 0, data.Length, options);
		}

		public void SendData(
			byte[] data,
			int offset,
			int count,
			SendDataOptions options
		) {
			// FIXME: Do we want to alloc like this? -flibit
			byte[] mem = new byte[count];
			Array.Copy(data, offset, mem, 0, mem.Length);
			foreach (NetworkGamer gamer in Session.AllGamers)
			{
				NetworkSession.NetworkEvent evt = new NetworkSession.NetworkEvent()
				{
					Type = NetworkSession.NetworkEventType.PacketSend,
					Gamer = gamer,
					Packet = mem,
					Reliable = options
				};
				Session.SendNetworkEvent(evt);
			}
		}

		public void SendData(
			byte[] data,
			SendDataOptions options,
			NetworkGamer recipient
		) {
			SendData(data, 0, data.Length, options, recipient);
		}

		public void SendData(
			byte[] data,
			int offset,
			int count,
			SendDataOptions options,
			NetworkGamer recipient
		) {
			// FIXME: Do we want to alloc like this? -flibit
			byte[] mem = new byte[count];
			Array.Copy(data, offset, mem, 0, mem.Length);
			NetworkSession.NetworkEvent evt = new NetworkSession.NetworkEvent()
			{
				Type = NetworkSession.NetworkEventType.PacketSend,
				Gamer = recipient,
				Packet = mem,
				Reliable = options
			};
			Session.SendNetworkEvent(evt);
		}

		public void SendData(PacketWriter data, SendDataOptions options)
		{
			// FIXME: Do we want to alloc like this? -flibit
			byte[] mem = (data.BaseStream as System.IO.MemoryStream).ToArray();
			data.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
			foreach (NetworkGamer gamer in Session.AllGamers)
			{
				NetworkSession.NetworkEvent evt = new NetworkSession.NetworkEvent()
				{
					Type = NetworkSession.NetworkEventType.PacketSend,
					Gamer = gamer,
					Packet = mem,
					Reliable = options
				};
				Session.SendNetworkEvent(evt);
			}
		}

		public void SendData(
			PacketWriter data,
			SendDataOptions options,
			NetworkGamer recipient
		) {
			// FIXME: Do we want to alloc like this? -flibit
			byte[] mem = (data.BaseStream as System.IO.MemoryStream).ToArray();
			data.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
			NetworkSession.NetworkEvent evt = new NetworkSession.NetworkEvent()
			{
				Type = NetworkSession.NetworkEventType.PacketSend,
				Gamer = recipient,
				Packet = mem,
				Reliable = options
			};
			Session.SendNetworkEvent(evt);
		}

		#endregion
	}
}
