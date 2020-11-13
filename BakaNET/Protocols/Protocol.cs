using BakaNET.Encoding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BakaNET.Protocols
{
    public abstract class Protocol
    {
        //Handles different types of IMessages 

        public abstract void HandleData(byte[] data);

    }
    public interface IMessage
    {

            //contains fields 

        public byte[] Encode();
        public void Decode(PacketReader msg); 
    }
}
