using InternetCore;
using System;
using System.Text;

namespace Client
{
    public class SocketClient : ClientCore, ICore
    {
        public SocketClient(string ip, int port) : base(ip, port)
        {
        }

        private event ReceivingMessage Notification;

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            client.GetStream().Write(buffer, 0, buffer.Length);
        }

        public override void ReceiveMessage()
        {
            while (true)
            {
                var buffer = new byte[1024];
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
            networkStream.Close();
            client.Close();
        }

        public void SubscribeToReceivingMessage(ReceivingMessage message) => Notification += message;

        public void UnsubscribeToReceivingMessage(ReceivingMessage message) => Notification -= message;

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