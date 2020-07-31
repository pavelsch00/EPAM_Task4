using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class SocketClient
    {

        private const string _localhost = "localhost";

        public const int _port = 11000;

        public SocketClient()
        {
            IpHost = Dns.GetHostEntry(_localhost);
            IpAddr = IpHost.AddressList[0];
            IpEndPoint = new IPEndPoint(IpAddr, _port);
            TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private Socket TcpSocket { get; set; }

        private IPHostEntry IpHost { get; set; }

        private IPAddress IpAddr { get; set; }

        private IPEndPoint IpEndPoint { get; set; }

        public bool SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            if (!(message == string.Empty))
            {
                TcpSocket.Send(buffer);
                return true;
            }
            else
            {
                TcpSocket.Send(Encoding.UTF8.GetBytes("Error!"));
                TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                return false;
            }
        }

        public void SendMessageFromSocket()
        {
            while (true)
            {
                TcpSocket.Connect(IpEndPoint);
                Console.Write("Input message: ");
                var message = Console.ReadLine();
               
                if(!SendMessage(message))
                    continue;
                    
                var bytes = new byte[1024];
                int bytesRec = TcpSocket.Receive(bytes);

                Console.WriteLine("Server response: {0}\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

                if (message.IndexOf("<TheEnd>") == -1)
                    TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                else
                    break;
            }

            TcpSocket.Shutdown(SocketShutdown.Both);
            TcpSocket.Close();
        }
    }
}
