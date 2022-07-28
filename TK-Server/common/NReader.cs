using System;
using System.IO;
using System.Net;
using System.Text;

namespace common
{
    public class NReader : BinaryReader
    {
        public NReader(Stream s) : base(s, Encoding.UTF8)
        { }

        public string Read32UTF() => Encoding.UTF8.GetString(ReadBytes(ReadInt32()));

        public override double ReadDouble()
        {
            var arr = base.ReadBytes(8);

            Array.Reverse(arr);

            return BitConverter.ToDouble(arr, 0);
        }

        public override short ReadInt16() => IPAddress.NetworkToHostOrder(base.ReadInt16());

        public override int ReadInt32() => IPAddress.NetworkToHostOrder(base.ReadInt32());

        public override long ReadInt64() => IPAddress.NetworkToHostOrder(base.ReadInt64());

        public string ReadNullTerminatedString()
        {
            var ret = new StringBuilder();
            var b = ReadByte();

            while (b != 0)
            {
                ret.Append((char)b);
                b = ReadByte();
            }

            return ret.ToString();
        }

        public override float ReadSingle()
        {
            var arr = base.ReadBytes(4);

            Array.Reverse(arr);

            return BitConverter.ToSingle(arr, 0);
        }

        public override ushort ReadUInt16() => (ushort)IPAddress.NetworkToHostOrder((short)base.ReadUInt16());

        public override uint ReadUInt32() => (uint)IPAddress.NetworkToHostOrder((int)base.ReadUInt32());

        public override ulong ReadUInt64() => (ulong)IPAddress.NetworkToHostOrder((long)base.ReadUInt64());

        public string ReadUTF() => Encoding.UTF8.GetString(ReadBytes(ReadInt16()));
    }
}
