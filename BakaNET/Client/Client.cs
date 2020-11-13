using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BakaNET.Protocols;

namespace BakaNET
{
    public class Client
    {
        static readonly TcpClient client = new TcpClient();
        static Socket socket;
        static IPEndPoint endPoint;
        static Protocol prot;
        static byte[] receiveBuffer;
        static readonly int bufferSize = 1024 * 2;



        public static void Start(IPEndPoint iPEnd, Protocol protocol)
        {
            endPoint = iPEnd;
            prot = protocol;

            Connect();
        }

        public static void Connect()
        {
            Console.WriteLine($"connecting to {endPoint.Address} : {endPoint.Port}");
            client.BeginConnect(endPoint.Address, endPoint.Port, ConnectCallback, null);

        }

        private static void ConnectCallback(IAsyncResult result)
        {
            client.EndConnect(result);
            socket = client.Client;
            receiveBuffer = new byte[bufferSize];
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReciveCallback, null);
        }

        private static void ReciveCallback(IAsyncResult result)
        {
            var dataSize = socket.EndReceive(result);
            var data = new byte[dataSize];
            Array.Copy(receiveBuffer, data, data.Length);
            prot.HandleData(data);

            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReciveCallback, null);
        }
        public static void Send(IMessage message)
        {
            socket.Send(message.Encode());
        }

    }

}