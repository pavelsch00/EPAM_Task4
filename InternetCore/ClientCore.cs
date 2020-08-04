﻿using System.Net.Sockets;
using System.Threading;

namespace InternetCore
{
    public abstract class ClientCore : Core
    {
        protected readonly TcpClient _client;

        protected readonly NetworkStream _networkStream;

        public ClientCore(string ip, int port) : base(ip, port)
        {
            _client = new TcpClient();
            _client.Connect(Ip, Port);
            _networkStream = _client.GetStream();
            _messageWaitingThread = new Thread(ReceiveMessage);
            _messageWaitingThread.Start();
        }

        public delegate string ReceivingMessage(string message);

        public abstract void ReceiveMessage();
    }
}