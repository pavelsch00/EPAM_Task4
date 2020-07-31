using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    public class SocketClient
    {

        private const string _localhost = "localhost";

        private const int _port = 11000;

        private Action<string> TranslateMessage;

        private Dictionary<char, string> _dictionary = new Dictionary<char, string>
        {
            {'а', "a"},
            {'б', "b"},
            {'в', "v"},
            {'г', "g"},
            {'д', "d"},
            {'е', "e"},
            {'ж', "zh"},
            {'з', "z"},
            {'и', "i"},
            {'й', "i"},
            {'к', "k"},
            {'л', "l"},
            {'м', "m"},
            {'н', "n"},
            {'о', "o"},
            {'п', "p"},
            {'р', "r"},
            {'с', "s"},
            {'т', "t"},
            {'у', "u"},
            {'ф', "f"},
            {'х', "h"},
            {'ц', "c"},
            {'ч', "ch"},
            {'ш', "sh"},
            {'щ', "sh"},
            {'ъ', ""},
            {'ы', "i"},
            {'ь', ""},
            {'э', "e"},
            {'ю', "yu"},
            {'я', "ya"},
            {'a', "а"},
            {'b', "б"},
            {'c', "ц"},
            {'d', "д"},
            {'e', "е"},
            {'f', "ф"},
            {'g', "г"},
            {'h', "х"},
            {'i', "и"},
            {'j', "дж"},
            {'k', "к"},
            {'l', "л"},
            {'m', "м"},
            {'n', "н"},
            {'o', "о"},
            {'p', "п"},
            {'q', "кв"},
            {'r', "р"},
            {'s', "с"},
            {'t', "т"},
            {'u', "у"},
            {'v', "в"},
            {'w', "в"},
            {'x', "кс"},
            {'y', "и"},
            {'z', "з"},
            {',', ","},
            {'.', "."},
            {'-', "-"},
            {'_', "_"},
            {'/', "/"},
            {'\\', "\\"},
            {'(', "("},
            {')', ")"},
            {' ', " "},
        };

        public SocketClient()
        {
            IpHost = Dns.GetHostEntry(_localhost);
            IpAddr = IpHost.AddressList[0];
            IpEndPoint = new IPEndPoint(IpAddr, _port);
            TcpSocket = new Socket(IpAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            TranslateMessage += (string message) =>
            {
                message = message.ToLower();

                for (int i = 0; i < message.Length; i++)
                {
                    message = message.Replace(message[i].ToString(), _dictionary[message[i]]);
                }

                Message = message;
            };
        }

        private Socket TcpSocket { get; set; }

        private IPHostEntry IpHost { get; set; }

        private IPAddress IpAddr { get; set; }

        private IPEndPoint IpEndPoint { get; set; }

        private string Message { get; set; }

        private void SendMessage(string message)
        {
            TranslateMessage(message);

            byte[] buffer = Encoding.UTF8.GetBytes(Message);

            if (!(Message == string.Empty))
                TcpSocket.Send(buffer);
        }

        public void StartClient()
        {
            TcpSocket.Connect(IpEndPoint);

            var bytes = new byte[1024];
            int bytesRec = TcpSocket.Receive(bytes);

            Console.WriteLine("Server response: {0}\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            TcpSocket.Shutdown(SocketShutdown.Both);
            TcpSocket.Close();
        }

        public void StartClient(string mes)
        {
            TcpSocket.Connect(IpEndPoint);

            SendMessage(mes);

            var bytes = new byte[1024];
            int bytesRec = TcpSocket.Receive(bytes);

            Console.WriteLine("Server response: {0}\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            TcpSocket.Shutdown(SocketShutdown.Both);
            TcpSocket.Close();
        }
    }
}
