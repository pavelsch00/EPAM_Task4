using System;
using Server;
using Client;
using System.Threading;


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
            Console.WriteLine(socketServer.RecieveMessageFromClient());
            Console.WriteLine(socketServer.ClientMessageDictionary.Values.Count);
        }

        private static void Count()
        {
            SocketClient socketClient = new SocketClient();
            try
            {
                socketClient.SendMessageToServer("fsf");
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
