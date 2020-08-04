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

        public SocketServer(string ip, int port)
        {
            Listener = new TcpListener(IPAddress.Parse(ip), port);
            TcpClients = new List<TcpClient>();
            Listener.Start();
            _clientWaitingThread = new Thread(WaitingForClientConnection);
            _clientWaitingThread.Start();
        }

        private List<TcpClient> TcpClients { get; set; }

        private TcpListener Listener { get; set; }

        public delegate void ReceivingMessage(TcpClient client, string message);

        public event ReceivingMessage Notification;

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
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);
                var destinationArray = new byte[byteCount];

                Array.Copy(buffer, destinationArray, byteCount);
                string message = Encoding.UTF8.GetString(destinationArray);

                Notification?.Invoke(client, message);

                Console.WriteLine(message);
            }
        }

        //recoment pls
        public void SubscribeToSaveMessages(ReceivingMessage message) => Notification += message;

        //recoment pls
        public void UnsubscribeNotToSaveMessages(ReceivingMessage message) => Notification -= message;

        public void StartChat()
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
