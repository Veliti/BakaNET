using BakaNET.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BakaNET.Protocols
{

    public abstract class BaseLeaderBoardProtocol : Protocol
    {
        public Dictionary<string, BoardMessage> LeaderBoard; // IP , Name/Score !!!!!!!!! NOT IP PLZ CHANGE, USING IP FOR THIS IS DUMB AF !!!!!!!!!

        readonly string path = AppDomain.CurrentDomain.BaseDirectory + "test.json";
        private async void StoreLeaderBoard()
        {
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await fileStream.WriteAsync(JsonSerializer.SerializeToUtf8Bytes(LeaderBoard));
                }
        }
        private async void ReadLeaderBoard()
        {
            if (File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    LeaderBoard = new Dictionary<string, BoardMessage>(await JsonSerializer.DeserializeAsync<Dictionary<string, BoardMessage>>(fileStream));
                }   
            }
            else
            {
                LeaderBoard = new Dictionary<string, BoardMessage>();
            }
        }
        enum MessageTypes : byte
        {
            AskFor,
            BoardMessage
        }
        public override void HandleData(byte[] data)
        {
            using (PacketReader packet = new PacketReader(data))
            {
                switch (packet.ReadByte())
                {
                    case (byte)MessageTypes.AskFor:
                        HandleMessage(new AskFor(packet));
                        break;
                    case (byte)MessageTypes.BoardMessage:
                        HandleMessage(new BoardMessage(packet));
                        break;
                    default:
                        Console.WriteLine("wrong type");
                        break;
                }
            }
        }

        protected abstract void HandleMessage(AskFor askFor);
        protected abstract void HandleMessage(BoardMessage boardMessage);

        public class AskFor : IMessage
        {
            public AskFor(Comands comand)
            {
                Comand = (byte)comand;
            }
            public AskFor(PacketReader msg)
            {
                Decode(msg);
            }
            public byte Comand { get; set; }
            public enum Comands
            {
                Board,
                ID 
            }
            public void Decode(PacketReader msg)
            {
                Comand = msg.ReadByte();
            }
            public byte[] Encode()
            {
                using (PacketWriter pw = new PacketWriter())
                {
                    pw.Write((byte)MessageTypes.AskFor);
                    pw.Write(Comand);
                    return pw.GetBytes();
                }
            }
        }
        public class BoardMessage : IMessage
        {
            public BoardMessage(string name, int score)
            {
                Name = name;
                Score = score;
            }
            public BoardMessage(PacketReader msg)
            {
                Decode(msg);
            }
            public string Name { get; set; }
            public int Score { get; set; }
            public void Decode(PacketReader msg)
            {
                Name = msg.ReadString();
                Score = msg.Read();
            }
            public byte[] Encode()
            {
                using (PacketWriter pw = new PacketWriter())
                {
                    pw.Write((byte)MessageTypes.BoardMessage);
                    pw.Write(Name);
                    pw.Write(Score);
                    return pw.GetBytes();
                }
            }
        }

    }
}
