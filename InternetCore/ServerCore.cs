using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace InternetCore
{
    /// <summary>
    /// Abstract class stores information about clientWaitingThread, TcpListener, TcpClients and delegate ReceivingMessage.
    /// </summary>
    public abstract class ServerCore : Core
    {
        /// <summary>
        /// The field stores information about the client waiting thread.
        /// </summary>
        protected Thread clientWaitingThread;

        /// <summary>
        /// Constructor initializes Listener, TcpClients and creates client waiting thread.
        /// </summary>
        /// <param name="ip">string ip</param>
        /// <param name="port">string port</param>
        public ServerCore(string ip, int port) : base(ip, port)
        {
            Listener = new TcpListener(IPAddress.Parse(ip), port);
            TcpClients = new List<TcpClient>();
            Listener.Start();
            clientWaitingThread = new Thread(WaitingForClientConnection);
            clientWaitingThread.Start();
        }

        /// <summary>
        /// The delegate is responsible for notification of the arrival of a message.
        /// </summary>
        /// <param name="client">TcpClient client</param>
        /// <param name="message">string message</param>
        public delegate void ReceivingMessage(TcpClient client, string message);

        /// <summary>
        /// The property stores information about TcpClients.
        /// </summary>
        protected List<TcpClient> TcpClients { get; set; }

        /// <summary>
        /// The property stores information about Listener.
        /// </summary>
        protected TcpListener Listener { get; set; }

        /// <summary>
        /// The method is waiting for clients to connect.
        /// </summary>
        protected abstract void WaitingForClientConnection();

        /// <summary>
        /// The method is responsible for receiving messages.
        /// </summary>
        /// <param name="obj">any tcp client</param>
        public abstract void ReceiveMessage(object obj);
    }
}
