using System;
using Server;
using Client;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Test
{
    class Program
    {
        static object locker = new object();
        static int i = 0;

        public static void Main(string[] args)
        {
            Thread myThread = new Thread(new ThreadStart(Count));
            myThread.Start();
            
            SocketServer socketServer = new SocketServer();
            socketServer.StartServer();

            Console.WriteLine("The End :(");
        }

        private static void Count()
        {
            SocketClient socketClient = new SocketClient();
            try
            {
                socketClient.StartClient("Как прекрасен этот мир");
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
