using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using BakaNET.Protocols;

namespace BakaNET
{
    public static class Server
    {
        public static int MaxConnections { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Connection> connections = new Dictionary<int, Connection>();
        private static TcpListener tcpListener;
        public static Protocol prot;


        public static void Start(int port, int maxConnections, Protocol protocol)
        {
            Port = port;
            MaxConnections = maxConnections;
            prot = protocol;

            Console.WriteLine("Server started");
            InitializeDictionary();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start(MaxConnections);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPAcceptCallback), null);
            
        }

        private static void TCPAcceptCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPAcceptCallback), null);
            Console.WriteLine($"{((IPEndPoint)_client.Client.RemoteEndPoint).Address} connected!");
            for (int i = 1; i <= MaxConnections; i++)
            {
                if(connections[i].tcp.socket == null)
                {
                    connections[i].tcp.Connect(_client);
                    return;
                }
            }
            Console.WriteLine($"{((IPEndPoint)_client.Client.RemoteEndPoint).Address} failed to connect: server full");
        }

        public static void Send(byte[] dataBytes)
        {
            foreach (var client in connections)
            {

                if (client.Value.tcp.socket != null)
                {
                    client.Value.tcp.socket.Client.SendAsync(dataBytes, SocketFlags.None);
                }
            }
        }

        private static void InitializeDictionary()
        {
            for (int i = 1; i <= MaxConnections; i++)
            {
                connections.Add(i, new Connection(i));
            }
        }
        public static void Disconnect(int id)
        {
            Console.WriteLine($"{connections[id].tcp.socket.Client.RemoteEndPoint} disconnected!");
            connections[id].Disconnect();
            connections[id] = new Connection(id);
        }
    }
}
