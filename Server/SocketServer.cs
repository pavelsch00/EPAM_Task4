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

        private event ReceivingMessage Notification;

        protected override void WaitingForClientConnection()
        {
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                TcpClients.Add(client);

                messageWaitingThread = new Thread(ReceiveMessage);
                messageWaitingThread.Start(client);
            }
        }

        public override void ReceiveMessage(object obj)
        {
            var client = (TcpClient)obj;

            NetworkStream networkStream = client.GetStream();

            while (true)
            {
                var buffer = new byte[1024];
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                Notification?.Invoke(client, message);

                Console.WriteLine(message);
            }
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in TcpClients)
            {
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }

        public void SubscribeToReceivingMessage(ReceivingMessage message) => Notification += message;

        public void UnsubscribeToReceivingMessage(ReceivingMessage message) => Notification -= message;

        public void StartChat()
        {
            string message;

            while (true)
            {
                if (!string.IsNullOrEmpty(message = Console.ReadLine()))
                {
                    SendMessage(message);
                }
            }
        }

        public void Disconnect()
        {
            messageWaitingThread.Join();

            clientWaitingThread.Join();

            Listener.Stop();
        }

        /// <summary>
        /// The method compares two objects for equivalence.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            SocketServer socketServer = (SocketServer)obj;

            if (Ip != socketServer.Ip)
            {
                return false;
            }

            if (Port != socketServer.Port)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The method gets the hash code of the object.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() => HashCode.Combine(Ip) * HashCode.Combine(Port);

        /// <summary>
        /// The method returns information about the object in string form.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $"Ip: {Ip}, Port: {Port}\n";
    }
}
