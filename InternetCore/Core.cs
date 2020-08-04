using System.Threading;

namespace InternetCore
{
    /// <summary>
    /// Abstract class stores information about stream, ip and port.
    /// </summary>
    public abstract class Core
    {
        /// <summary>
        /// The field stores information about the message waiting thread.
        /// </summary>
        protected Thread messageWaitingThread;

        /// <summary>
        /// Constructor initializes Ip and Port.
        /// </summary>
        /// <param name="ip">string ip</param>
        /// <param name="port">string port</param>
        public Core(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        /// <summary>
        /// The property stores information about ip.
        /// </summary>
        protected string Ip { get; private set; }

        /// <summary>
        /// The property stores information about port.
        /// </summary>
        protected int Port { get; private set; }
    }
}
