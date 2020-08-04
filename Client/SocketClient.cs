using InternetCore;
using System;
using System.Text;

namespace Client
{
    public class SocketClient : ClientCore, ICore
    {
        private event ReceivingMessage Notification;

        public SocketClient(string ip, int port) : base(ip, port)
        {
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            _client.GetStream().Write(buffer, 0, buffer.Length);
        }

        public override void ReceiveMessage()
        {
            while (true)
            {
                var buffer = new byte[1024];
                int byteCount = _networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                if(!(Notification == null))
                    message = Notification.Invoke(message);

                Console.WriteLine(message);
            }
        }

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
            _networkStream.Close();
            _client.Close();
        }

        public void SubscribeToReceivingMessage(ReceivingMessage message) => Notification += message;

        public void UnsubscribeToReceivingMessage(ReceivingMessage message) => Notification -= message;
    }
}