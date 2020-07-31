using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketServer
    {
        private const string _localhost = "localhost";

        private const int _port = 11000;

        public Action<string> TranslatedMessage;

        public SocketServer()
        {
            IpHost = Dns.GetHostEntry(_localhost);
            IpAddr = IpHost.AddressList[0];
            IpEndPoint = new IPEndPoint(IpAddr, _port);
            TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private Socket TcpSocket { get; set; }

        public Socket socket { get; set; }

        private IPHostEntry IpHost { get; set; }

        private IPAddress IpAddr { get; set; }

        private IPEndPoint IpEndPoint { get; set; }

        public void StartServer()
        {
            TcpSocket.Bind(IpEndPoint);
            TcpSocket.Listen(100);
            while (true)
            {
                socket = TcpSocket.Accept();

                string message = ReceivingMessage();

                SendMessage(message);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        public string ReceivingMessage()
        {
            var bytesArray = new byte[1024];

            int bytesRec = socket.Receive(bytesArray);

            return Encoding.UTF8.GetString(bytesArray, 0, bytesRec);
        }

        public void SendMessageToClient(string message)
        {
            TcpSocket.Bind(IpEndPoint);
            TcpSocket.Listen(100);

            socket = TcpSocket.Accept();

            SendMessage(message);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

        }

        public void SendMessage(string message)
        {
            byte[] messageByteArray = Encoding.UTF8.GetBytes(message);

            socket.Send(messageByteArray);
        }
    }
}
