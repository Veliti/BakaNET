using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace BakaNET
{
    public class Connection
    {
        public static int bufferSize = 1024 * 2;

        public int id;
        public TCP tcp;

        public Connection( int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
        public void Disconnect()
        {
            tcp.Dispose();
            tcp = null;
        }

        public class TCP: IDisposable
        {
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;
            private readonly int id;
            private bool disposed = false;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = bufferSize;
                socket.SendBufferSize = bufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[bufferSize];

                stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);

                //TODO: WelcomePacket
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                if (disposed) return;
                try
                {
                    int _byteLenth = stream.EndRead(_result);
                    if(_byteLenth <= 0)
                    {
                        Server.Disconnect(id);
                    }
                    var _data = new byte[_byteLenth];
                    Array.Copy(receiveBuffer, _data, _data.Length);

                    Server.prot.HandleData(_data);

                    stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);

                }
                catch (Exception _ex)
                {
                    Server.Disconnect(id);
                }
            }
            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                socket.Close();
                stream.Close();
                receiveBuffer = null;
            }
        }
    }
}
