using System.Net.Sockets;
using System.Threading;

namespace InternetCore
{
    public abstract class ClientCore : Core
    {
        protected readonly TcpClient client;

        protected readonly NetworkStream networkStream;

        public ClientCore(string ip, int port) : base(ip, port)
        {
            client = new TcpClient();
            client.Connect(Ip, Port);
            networkStream = client.GetStream();
            messageWaitingThread = new Thread(ReceiveMessage);
            messageWaitingThread.Start();
        }

        public delegate string ReceivingMessage(string message);

        public abstract void ReceiveMessage();
    }
}
