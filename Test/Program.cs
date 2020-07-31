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
            /*
            for (int i = 0; i < 5; i++)
            {
                // создаем новый поток
                Thread myThread = new Thread(new ThreadStart(thread));
                myThread.Start(); // запускаем поток
            }*/
            
            Thread myThread = new Thread(new ThreadStart(Count));
            myThread.Start(); // запускаем поток
            
            SocketServer socketServer = new SocketServer();
            socketServer.StartServer();

            Console.WriteLine("The End :(");
        }

        private static void Start()
        {
            SocketServer socketServer = new SocketServer();
            socketServer.StartServer();

        }

        private static void Send(object socketServer)
        {
            SocketServer socketServer1 = (SocketServer)socketServer;
            socketServer1.SendMessageToClient("hello");
        }

        private static void Count()
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
        /*
private static void thread()
{
   lock (locker)
   {
       SocketClient socketClient = new SocketClient();
       try
       {
           Console.WriteLine($"Поток: {i}");
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
       i++;
   }

}
*/
    }
}
