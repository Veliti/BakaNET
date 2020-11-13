using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace BakaNET.Encoding
{
    public class PacketWriter : BinaryWriter
    {
        private MemoryStream _ms;

        public PacketWriter() : base() {
            _ms = new MemoryStream();
            OutStream = _ms;
        }
        
        public void Write(Vector3 vector3)
        {
            Write(vector3.X);
            Write(vector3.Y);
            Write(vector3.Z);
        }
        public void Write(Quaternion quaternion)
        {
            Write(quaternion.X);
            Write(quaternion.Y);
            Write(quaternion.Z);
            Write(quaternion.W);
        }

        public byte[] GetBytes()
        {

            var data = _ms.ToArray();
            Dispose();
            return data;
        }
    }

    public class PacketReader : BinaryReader
    {
        public PacketReader(byte[] data)
            : base(new MemoryStream(data)) { } 

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }
        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }
    }
}
