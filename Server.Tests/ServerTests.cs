using Client;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace Server.Tests
{
    public class ServerTests
    {
        [Theory]
        [InlineData("127.0.4", 4000, "Hello word!")]
        [InlineData("127.1.0.8", 5000, " ")]
        public void ClientSendMessageToServer(string ip, int port, string expected)
        {
            var messageArchive = new MessageArchive();
            var server = new SocketServer(ip, port);
            var client = new SocketClient(ip, port);

            server.SubscribeToReceivingMessage(delegate (TcpClient client, string message)
            {
                messageArchive.AddToRchive(client, message);
            });

            //act
            var threadServer = new Thread(client.StartChat);
            threadServer.Start();
            var threadClient = new Thread(client.StartChat);
            threadClient.Start();

            client.SendMessage(expected);
            var actual = messageArchive.Archive.First().Value.First();

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("127.0.1", 5000)]
        [InlineData("127.0.2", 5000)]
        public void Euqal(string ip, int port)
        {
            var firstServer = new SocketServer(ip, port);

            //assert
            Assert.True(firstServer.Equals(firstServer));
        }
    }
}
