using InternetCore;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class SocketServer : ServerCore, ICore
    {
        public SocketServer(string ip, int port) : base(ip, port)
        {
        }

        public event ReceivingMessage Notification;

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in TcpClients)
                client.GetStream().Write(buffer, 0, buffer.Length);
        }

        protected override void WaitingForClientConnection()
        {
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                TcpClients.Add(client);

                _messageWaitingThread = new Thread(ReceiveMessage);
                _messageWaitingThread.Start(client);
            }
        }

        public override void ReceiveMessage(object obj)
        {
            var client = (TcpClient)obj;

            NetworkStream networkStream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[1024];
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                Notification?.Invoke(client, message);

                Console.WriteLine(message);
            }
        }

        public void SubscribeToReceivingMessage(ReceivingMessage message) => Notification += message;

        public void UnsubscribeToReceivingMessage(ReceivingMessage message) => Notification -= message;

        public void StartChat()
        {
            string message;

            while (true)
                if (!string.IsNullOrEmpty(message = Console.ReadLine()))
                    SendMessage(message);
        }

        public void Disconnect()
        {
            _messageWaitingThread.Join();

            _clientWaitingThread.Join();

            Listener.Stop();
        }
    }
}
