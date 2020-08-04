using System.Threading;

namespace InternetCore
{
    public abstract class Core
    {
        protected Thread messageWaitingThread;

        public Core(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        protected string Ip { get; private set; }

        protected int Port { get; private set; }
    }
}
