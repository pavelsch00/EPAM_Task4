using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketServer
    {
        private const string _localhost = "localhost";

        private const int _port = 11000;

        public Action<string, Socket> AddedMessageToClient;

        public Dictionary<Socket, List<string>> ClientMessageDictionary { get; private set; }

        public SocketServer()
        {
            IpHost = Dns.GetHostEntry(_localhost);
            IpAddr = IpHost.AddressList[0];
            IpEndPoint = new IPEndPoint(IpAddr, _port);
            TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            ClientMessageDictionary = new Dictionary<Socket, List<string>>();

            AddedMessageToClient += (string message, Socket ClientSocket) =>
            {
                if (!ClientMessageDictionary.ContainsKey(ClientSocket))
                {
                    var messageList = new List<string>();
                    messageList.Add(message);
                    ClientMessageDictionary.Add(ClientSocket, messageList);
                }
                else
                {
                    ClientMessageDictionary[ClientSocket].Add(message);
                }
            };
        }

        private Socket TcpSocket { get; set; }

        public Socket ClientSocket { get; set; }

        private IPHostEntry IpHost { get; set; }

        private IPAddress IpAddr { get; set; }

        private IPEndPoint IpEndPoint { get; set; }

        private string ReceivingMessage()
        {
            var bytesArray = new byte[1024];

            int bytesRec = ClientSocket.Receive(bytesArray);

            return Encoding.UTF8.GetString(bytesArray, 0, bytesRec);
        }

        private void SendMessage(string message)
        {
            byte[] messageByteArray = Encoding.UTF8.GetBytes(message);

            ClientSocket.Send(messageByteArray);
        }

        public string RecieveMessageFromClient()
        {
            TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            TcpSocket.Bind(IpEndPoint);
            TcpSocket.Listen(100);

            ClientSocket = TcpSocket.Accept();
            string message = ReceivingMessage();
            AddedMessageToClient(message, ClientSocket);

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            return message;
        }

        public void SendMessageToClient(string message)
        {
            TcpSocket.Bind(IpEndPoint);
            TcpSocket.Listen(100);

            ClientSocket = TcpSocket.Accept();
            SendMessage(message);

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
        }
    }
}
