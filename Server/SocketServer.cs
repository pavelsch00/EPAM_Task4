using InternetCore;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    /// <summary>
    /// The class organizes the tcp server work.
    /// </summary>
    public class SocketServer : ServerCore, ICore
    {
        /// <summary>
        /// The field stores information about the size of the buffer.
        /// </summary>
        private const int _bufferSize = 1024;

        /// <inheritdoc cref="ServerCore(string, int)"/>
        public SocketServer(string ip, int port) : base(ip, port)
        {
        }

        /// <summary>
        /// The field warns about the event of a message arrival.
        /// </summary>
        private event ReceivingMessage Notification;

        /// <inheritdoc cref="ServerCore.WaitingForClientConnection"/>
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

        /// <inheritdoc cref="ServerCore.ReceiveMessage"/>
        public override void ReceiveMessage(object obj)
        {
            var client = (TcpClient)obj;

            NetworkStream networkStream = client.GetStream();

            while (true)
            {
                var buffer = new byte[_bufferSize];
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                Notification?.Invoke(client, message);

                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Method sent message for client.
        /// </summary>
        /// <param name="message">message</param>
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in TcpClients)
            {
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }

        /// <inheritdoc cref="ICore.StartChat"/>
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

        /// <inheritdoc cref="ICore.Disconnect"/>
        public void Disconnect()
        {
            messageWaitingThread.Abort();

            clientWaitingThread.Abort();

            Listener.Stop();
        }

        /// <summary>
        /// Method subscribes to event handling.
        /// </summary>
        /// <param name="eventHandler">delegate event handler</param>
        public void SubscribeToReceivingMessage(ReceivingMessage eventHandler) => Notification += eventHandler;

        /// <summary>
        /// Method unsubscribes to event handling.
        /// </summary>
        /// <param name="eventHandler">delegate event handler</param>
        public void UnsubscribeToReceivingMessage(ReceivingMessage eventHandler) => Notification -= eventHandler;

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
        /// The method returns information about object in string form.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() => $"Ip: {Ip}, Port: {Port}\n";
    }
}
