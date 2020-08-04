namespace InternetCore
{
    /// <summary>
    /// The interface describes the basic methods for the server and client
    /// </summary>
    public interface ICore
    {
        /// <summary>
        /// Method sent message for client/server.
        /// </summary>
        /// <param name="message">message</param>
        public void SendMessage(string message);

        /// <summary>
        /// The method starts a chat.
        /// Threads are started waiting for messages and client connections.
        /// </summary>
        public void StartChat();

        /// <summary>
        /// Streams and connection are closed.
        /// </summary>
        public void Disconnect();
    }
}
