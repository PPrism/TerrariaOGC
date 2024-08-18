using System;
using System.Net;
using System.Net.Sockets;

namespace Lidgren.Network
{
    public class NetSocket : INetSocket
    {
        private static bool _dontFragmentUnsupported;

        private Socket Socket { get; set; }
        
        public int ReceiveBufferSize {
            get => Socket.ReceiveBufferSize;
            set => Socket.ReceiveBufferSize = value;
        }
        
        public int SendBufferSize {
            get => Socket.SendBufferSize;
            set => Socket.SendBufferSize = value;
        }
        
        public bool Blocking {
            get => Socket.Blocking;
            set => Socket.Blocking = value;
        }

        public bool DontFragment
        {
            get => Socket.DontFragment;
            set
            {
                if (_dontFragmentUnsupported)
                {
                    return;
                }

                try
                {
                    Socket.DontFragment = value;
                }
                catch
                {
                    _dontFragmentUnsupported = true;
                }
            }
        }
        
        public bool DualMode {
            //get => Socket.DualMode;Not needed for Net4
            //set => Socket.DualMode = value;
            get => false;
            set {  }
        }

        public EndPoint LocalEndPoint => Socket.LocalEndPoint;

        public bool IsBound => Socket.IsBound;

        public int Available => Socket.Available;

        public NetSocket(AddressFamily addressFamily)
        {
            Socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int value) =>
            Socket.SetSocketOption(optionLevel, optionName, value);

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool value) =>
            Socket.SetSocketOption(optionLevel, optionName, value);

        public void Bind(EndPoint localEndpoint) => Socket.Bind(localEndpoint);

        public void IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue) =>
            Socket.IOControl(ioControlCode, optionInValue, optionOutValue);

        public void Shutdown(SocketShutdown receive) =>
            Socket.Shutdown(receive);

        public void Close(int timeout) => Socket.Close(timeout);

        public bool Poll(int microSeconds, SelectMode mode) => Socket.Poll(microSeconds, mode);

        public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEndpoint) =>
            Socket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEndpoint);

        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEndpoint) =>
            Socket.SendTo(buffer, offset, size, socketFlags, remoteEndpoint);
    }
}
