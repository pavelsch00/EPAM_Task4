﻿using System;
using Server;
using Client;
using System.Threading;
using System.Net.Sockets;

namespace Test
{
    class Program
    {
        static object locker = new object();
        static int i = 0;

        public static void Main(string[] args)
        {
            /*SocketServer server = new SocketServer("127.0.0.1", 5000);
            MessageArchive messageArchive = new MessageArchive();
            server.SubscribeToSaveMessages(delegate (TcpClient client, string message)
            {
                messageArchive.AddToRchive(client, message);
            });

            server.StartChat();
            server.StopServer();*/
            
            SocketClient client = new SocketClient("127.0.0.1", 5000);
            Translator translator = new Translator();
            client.SubscribeToTransliteMessages(delegate (string message)
            {
                return translator.TranslateMessage(message);
            });

            client.StartChat();
            client.StopClient();

        }
    }
}
