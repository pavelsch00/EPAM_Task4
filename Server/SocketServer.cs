using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class SocketServer
    {
        private const string _localhost = "localhost";

        private const int _port = 11000;

        public SocketServer()
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

        public void GetMessage()
        {
            TcpSocket.Bind(IpEndPoint);
            TcpSocket.Listen(100);
            string data = null;
            int bytesRec = 0;

            while (true)
            {
                Socket handler = TcpSocket.Accept();

                byte[] bytesArray = new byte[1024];

                bytesRec = handler.Receive(bytesArray);

                data += Encoding.UTF8.GetString(bytesArray, 0, bytesRec);

                byte[] message = Encoding.UTF8.GetBytes(data);

                if(!(message == Encoding.UTF8.GetBytes(string.Empty)))
                    handler.Send(message);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}
