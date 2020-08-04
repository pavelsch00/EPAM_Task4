using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace InternetCore
{
    public abstract class ServerCore : Core
    {
        protected Thread _clientWaitingThread;

        public ServerCore(string ip, int port) : base(ip, port)
        {
            Listener = new TcpListener(IPAddress.Parse(ip), port);
            TcpClients = new List<TcpClient>();
            Listener.Start();
            _clientWaitingThread = new Thread(WaitingForClientConnection);
            _clientWaitingThread.Start();
        }

        public delegate void ReceivingMessage(TcpClient client, string message);

        protected List<TcpClient> TcpClients { get; set; }

        protected TcpListener Listener { get; set; }

        protected abstract void WaitingForClientConnection();

        public abstract void ReceiveMessage(object obj);
    }
}
