using System;
using BakaNET;
using System.Text;

namespace BakaNET.Protocols
{
    public class MirrorProtocol : Protocol
    {
        public override void HandleData(byte[] data)
        {
            Server.Send(data);
        }
    }
}
