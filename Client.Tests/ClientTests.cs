using Server;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace Client.Tests
{
    public class ClientTests
    {
        [Theory]
        [InlineData("127.0.2.1", 5000, "â ãîñòÿõ õîğîøî, à äîìà ëó÷øå", "v goñtyàh horoñho, à domà luchñhe")]
        [InlineData("127.0.1.8", 4000, "v goñtyàh horoñho, à domà luchñhe", "â ãîñòÿõ õîğîøî, à äîìà ëó÷øå")]
        public void ClientSendMessageToServer(string ip, int port, string actual, string expected)
        {
            var messageArchive = new MessageArchive();
            var translator = new Translator();
            var server = new SocketServer(ip, port);
            var client = new SocketClient(ip, port);

            client.SubscribeToReceivingMessage(delegate (string message)
            {
                return translator.TranslateMessage(message);
            });

            server.SubscribeToReceivingMessage(delegate (TcpClient client, string message)
            {
                messageArchive.AddToRchive(client, message);
            });

            //act
            var threadServer = new Thread(client.StartChat);
            threadServer.Start();
            var threadClient = new Thread(client.StartChat);
            threadClient.Start();

            server.SendMessage(expected);
            client.SendMessage(expected);
            actual = messageArchive.Archive.First().Value.First();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("127.0.5.1", 5000)]
        [InlineData("127.0.5.2", 5000)]
        public void Euqal(string ip, int port)
        {
            var server = new SocketServer(ip, port);
            var firstclient = new SocketClient(ip, port);

            //assert
            Assert.True(firstclient.Equals(firstclient));
        }
    }
}
