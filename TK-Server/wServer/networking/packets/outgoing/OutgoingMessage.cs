using common;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace wServer.networking.packets.outgoing
{
    public abstract class OutgoingMessage
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public abstract MessageId MessageId { get; }

        public void WriteNew(NWriter wtr) => Write(wtr);

        public int? Write(byte[] buff, int offset)
        {
            var s = new MemoryStream();
            using (var wtr = new NWriter(s))
            {
                Write(wtr);

                var bodyLength = (int)s.Position;
                var packetLength = bodyLength + 5;
                
                if (this is NewTick || this is Update)
                {
                    if(this is NewTick)
                        Console.WriteLine("Newtick: " + packetLength);
                    else
                        Console.WriteLine("Update: " + packetLength);
                }

                if (packetLength > buff.Length - offset)
                    return 0;

                try
                {
                    Buffer.BlockCopy(s.GetBuffer(), 0, buff, offset + 5, bodyLength);
                    Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(packetLength)), 0, buff, offset, 4);
                    buff[offset + 4] = (byte)MessageId;
                }   
                catch (ArgumentOutOfRangeException)
                {
                    return 0;
                }
                return packetLength;
            }
        }

        protected abstract void Write(NWriter wtr);

        public override string ToString()
        {
            var ret = new StringBuilder("{");
            var arr = GetType().GetProperties();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }
}
