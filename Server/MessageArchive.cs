using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// The class is intended for storing messages from tсp clients.
    /// </summary>
    public class MessageArchive
    {
        /// <summary>
        /// Dictionary stores messages from tсp clients.
        /// </summary>
        public Dictionary<TcpClient, List<string>> Archive { get; private set; }

        /// <summary>
        /// The constructor initializes the dictionary.
        /// </summary>
        public MessageArchive()
        {
            Archive = new Dictionary<TcpClient, List<string>>();
        }

        /// <summary>
        /// The method adds a message from the client's tсp to the dictionary.
        /// </summary>
        public void AddToRchive(TcpClient client, string message)
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
        }

        /// <summary>
        /// The method compares two objects for equivalence.
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            MessageArchive messageArchive = (MessageArchive)obj;

            if (Archive != messageArchive.Archive)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The method gets the hash code of the object.
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() => HashCode.Combine(Archive);
    }
}
