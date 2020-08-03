using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class MessageArchive
    {
        public Dictionary<TcpClient, List<string>> Archive { get; private set; }

        public Action<TcpClient, string> AddMessageToArchive;

        public MessageArchive()
        {
            Archive = new Dictionary<TcpClient, List<string>>();

            AddMessageToArchive += (TcpClient client, string message) =>
            {
                if (!Archive.ContainsKey(client))
                {
                    var messageList = new List<string>();
                    messageList.Add(message);
                    Archive.Add(client, messageList);
                }
                else
                {
                    Archive[client].Add(message);
                }
            };
        }
    }
}
