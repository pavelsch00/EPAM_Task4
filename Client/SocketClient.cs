using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    public class SocketClient
    {
        private Thread _messageWaitingThread;

        private TcpClient _client;

        private NetworkStream _networkStream;

        public SocketClient(string ip, int port)
        {
            _client = new TcpClient();
            _client.Connect(ip, port);
             _networkStream = _client.GetStream();
            _messageWaitingThread = new Thread(ReceivingMessagesFromServer);
            _messageWaitingThread.Start();
        }

        public delegate string ReceivingMessage(string message);

        public event ReceivingMessage TranslateMessage;

        private void SendMessageToServer(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            _client.GetStream().Write(buffer, 0, buffer.Length);
        }

        private void ReceivingMessagesFromServer()
        {
            while (true)
            {
                var buffer = new byte[1024];
                int byteCount = _networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                if(!(TranslateMessage == null))
                    message = TranslateMessage.Invoke(message);

                Console.WriteLine(message);
            }
        }

        public void StartChat()
        {
            string message;

            while (true)
                if (!string.IsNullOrEmpty(message = Console.ReadLine()))
                    SendMessageToServer(message);
        }

        public void StopClient()
        {
            _messageWaitingThread.Join();
            _networkStream.Close();
            _client.Close();
        }

        public void SubscribeToTransliteMessages(ReceivingMessage message) => TranslateMessage += message;

        public void UnsubscribeNotToTransliteMessages(ReceivingMessage message) => TranslateMessage -= message;
    }
}
