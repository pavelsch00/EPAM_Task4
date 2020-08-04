using InternetCore;
using System;
using System.Text;

namespace Client
{
    /// <summary>
    /// The class organizes the tcp client's work.
    /// </summary>
    public class SocketClient : ClientCore, ICore
    {
        /// <summary>
        /// The field stores information about the size of the buffer.
        /// </summary>
        private const int _bufferSize = 1024;

        /// <inheritdoc cref="ClientCore(string, int)"/>
        public SocketClient(string ip, int port) : base(ip, port)
        {
        }

        /// <summary>
        /// The field warns about the event of a message arrival.
        /// </summary>
        private event ReceivingMessage Notification;

        /// <summary>
        /// Method sent message for server.
        /// </summary>
        /// <param name="message">message</param>
        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            client.GetStream().Write(buffer, 0, buffer.Length);
        }

        /// <inheritdoc cref="ClientCore.ReceiveMessage"/>
        public override void ReceiveMessage()
        {
            while (true)
            {
                var buffer = new byte[_bufferSize];
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                if(!(Notification == null))
                {
                    message = Notification.Invoke(message);
                }

                Console.WriteLine(message);
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
            messageWaitingThread.Join();
            networkStream.Close();
            client.Close();
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

            SocketClient socketClient = (SocketClient)obj;

            if (Ip != socketClient.Ip)
            {
                return false;
            }

            if (Port != socketClient.Port)
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