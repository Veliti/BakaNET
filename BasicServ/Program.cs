using System;
using System.Threading.Tasks;
using BakaNET;
using BakaNET.Protocols;

namespace BasicServ
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start(50502, 10, new MirrorProtocol());

            while (true)
            {
                Console.ReadKey();
            }

        }
    }
}
