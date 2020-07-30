using System;
using Server;
using Client;
using System.Threading;

namespace Test
{
    class Program
    {
        public static void Main(string[] args)
        {
            SocketServer socketServer = new SocketServer();
            SocketClient socketClient = new SocketClient();

            // создаем новый поток
            Thread myThread = new Thread(new ThreadStart(thread));
            myThread.Start(); // запускаем поток

            socketServer.GetMessage();

            

        }

        private static void thread()
        {
            SocketClient socketClient = new SocketClient();

            try
            {
                socketClient.SendMessageFromSocket();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
