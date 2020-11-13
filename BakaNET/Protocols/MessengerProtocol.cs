using BakaNET.Encoding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BakaNET.Protocols
{
    public class MessengerProtocol : Protocol
    {
        public override void HandleData(byte[] data)
        {
            using (var pReader = new PacketReader(data))
            {
                switch (pReader.ReadByte())
                {
                    case (byte)MessageTypes.StrMessage:
                        HandleStrMessage(new StrMessage(pReader));
                        break;
                    default:
                        Console.WriteLine("Wrong Type");
                        break;
                }
            }
        }

        private void HandleStrMessage(StrMessage message)
        {
            Console.WriteLine($"{message.NameMessage} -> {message.StringMessage}");
        }


        enum MessageTypes : byte
        {
            StrMessage,
        }

        public class StrMessage : IMessage
        {
            public string NameMessage { get; set; }
            public string StringMessage { get; set; }

            public StrMessage() { }
            public StrMessage(PacketReader packetReader)
            {
                Decode(packetReader);
            }
            public void Decode(PacketReader msg)
            {
                NameMessage = msg.ReadString();
                StringMessage = msg.ReadString();
            }

            public byte[] Encode()
            {
                using (PacketWriter pw = new PacketWriter())
                {
                    pw.Write((byte)MessageTypes.StrMessage);
                    pw.Write(NameMessage);
                    pw.Write(StringMessage);

                    return pw.GetBytes();
                }
            }
        }
    }
}
