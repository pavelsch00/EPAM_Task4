using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class SocketServer
    {
        private Thread _messageWaitingThread;

        private Thread _clientWaitingThread;

        private Action<TcpClient, string> ReceivingMessage;

        private bool _isSubscribing;

        public SocketServer(string ip, int port)
        {
            Listener = new TcpListener(IPAddress.Parse(ip), port);
            TcpClients = new List<TcpClient>();
            Listener.Start();
            _clientWaitingThread = new Thread(WaitingForClientConnection);
            _clientWaitingThread.Start();
            _isSubscribing = false;
            MessageArchives = new MessageArchive();
        }

        private List<TcpClient> TcpClients { get; set; }

        private TcpListener Listener { get; set; }

        public MessageArchive MessageArchives { get; private set; }

        private void SendMessageToAllClient(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach (var client in TcpClients)
                client.GetStream().Write(buffer, 0, buffer.Length);
        }

        private void WaitingForClientConnection()
        {
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                TcpClients.Add(client);

                _messageWaitingThread = new Thread(ReceivingMessagesFromClient);
                _messageWaitingThread.Start(client);
            }
        }

        private void ReceivingMessagesFromClient(object obj)
        {
            var client = (TcpClient)obj;

            NetworkStream networkStream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[1024];
                int byteCount;

                try
                {
                    byteCount = networkStream.Read(buffer, 0, buffer.Length);
                }
                catch (System.IO.IOException)
                {
                    TcpClients.Remove(client);
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer);

                if (_isSubscribing)
                    ReceivingMessage(client, message);

                Console.WriteLine(message);
            }
        }

        public void SubscribeToSaveMessages()
        {
            ReceivingMessage += MessageArchives.AddMessageToArchive;
            _isSubscribing = true;
        }

        public void UnsubscribeNotToSaveMessages()
        {
            ReceivingMessage -= MessageArchives.AddMessageToArchive;
            _isSubscribing = false;
        }

        public void ChatStart()
        {
            string message;

            while (true)
                if (!string.IsNullOrEmpty(message = Console.ReadLine()))
                    SendMessageToAllClient(message);
        }

        public void StopServer()
        {
            _messageWaitingThread.Join();

            _clientWaitingThread.Join();

            Listener.Stop();
        }
    }
}
