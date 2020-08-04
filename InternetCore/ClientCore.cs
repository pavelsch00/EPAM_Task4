using System.Net.Sockets;
using System.Threading;

namespace InternetCore
{
    /// <summary>
    /// Abstract class stores information about TcpClient, NetworkStream and delegate ReceivingMessage.
    /// </summary>
    public abstract class ClientCore : Core
    {
        /// <summary>
        /// The field stores information about NetworkStream.
        /// </summary>
        protected readonly NetworkStream networkStream;

        /// <summary>
        /// The field stores information about TcpClient.
        /// </summary>
        protected readonly TcpClient tcpClient;

        /// <summary>
        /// Constructor initializes client, networkStream and creates a thread waiting for messages.
        /// </summary>
        /// <param name="ip">string ip</param>
        /// <param name="port">string port</param>
        public ClientCore(string ip, int port) : base(ip, port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(Ip, Port);
            networkStream = tcpClient.GetStream();
            messageWaitingThread = new Thread(ReceiveMessage);
            messageWaitingThread.Start();
        }

        /// <summary>
        /// The delegate is responsible for notification of the arrival of a message.
        /// </summary>
        /// <param name="message">string message</param>
        public delegate string ReceivingMessage(string message);

        /// <summary>
        /// The method is responsible for receiving messages.
        /// </summary>
        public abstract void ReceiveMessage();
    }
}
