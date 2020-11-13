using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BakaNET;
using BakaNET.Protocols;

namespace BasicClient
{
    class Program
    {
        public static IPEndPoint endPoint;
        static string Name;
        public static void Main()
        {
            GetEndpoint();
            Client.Start(endPoint, new MessengerProtocol());
            Name = Console.ReadLine();
            StartSender();

        }


         public static async Task StartSender()
        {
            while (true)
            {
                var msg = new MessengerProtocol.StrMessage();
                msg.NameMessage = Name;
                msg.StringMessage = await Task.Run(() => Console.ReadLine());
                Client.Send(msg);
            }
        }
        static Task _t = new Task(async() =>
        {
            await Task.Delay(3500);
            Console.Write(".");
        });

         private static void GetEndpoint()
        {
            Console.WriteLine("write IP:Port");

            var IP = Console.ReadLine();
            IPAddress address;
            switch (IP)
            {
                case "lb":
                    endPoint = new IPEndPoint(IPAddress.Loopback, 50502);
                    break;

                default:
                    while (!IPAddress.TryParse(IP, out address))
                    {
                        Console.WriteLine("IP wrong try again!");
                        IP = Console.ReadLine();
                    }
                    endPoint = new IPEndPoint(address, 50502);
                    break;
            }

        }
    }
}
