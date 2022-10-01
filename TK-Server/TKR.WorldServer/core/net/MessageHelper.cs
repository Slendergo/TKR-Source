using System.Collections.Generic;
using System.Text;
using System;
using System.Net;
using System.Xml.Linq;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.structures;
using NLog.LayoutRenderers;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace TKR.WorldServer.core.net
{
    public struct OutgoingMessageData
    {
        private MessageId MessageId;
        private List<byte> Buffer;
        private int Length;

        public OutgoingMessageData(MessageId messageId)
        {
            MessageId = messageId;
            Buffer = new List<byte>();
            Length = 0;
        }

        public void WriteByte(byte value)
        {
            Buffer.Add(value);
            Length += sizeof(byte);
        }

        public void WriteBoolean(bool value)
        {
            Buffer.Add((byte)(value ? 1 : 0));
            Length += sizeof(bool);
        }

        public void WriteFloat(float value)
        {
            var val = BitConverter.GetBytes(value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += sizeof(float);
        }

        public void WriteFloat(double value)
        {
            var val = BitConverter.GetBytes((float)value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += sizeof(float);
        }

        public void WriteDouble(double value)
        {
            var val = BitConverter.GetBytes(value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += sizeof(double);
        }

        public void WriteDouble(float value)
        {
            var val = BitConverter.GetBytes((double)value);
            Array.Reverse(val);
            Buffer.AddRange(val);
            Length += sizeof(double);
        }

        public void WriteInt16(short value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
            Length += sizeof(short);
        }

        public void WriteInt16(int value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value)));
            Length += sizeof(short);
        }

        public void WriteInt32(int value)
        {
            Buffer.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value)));
            Length += sizeof(int);
        }

        public void WriteUInt32(uint value)
        {
            Buffer.AddRange(BitConverter.GetBytes((uint)IPAddress.HostToNetworkOrder((int)value)));
            Length += sizeof(uint);
        }

        public void WriteUTF16(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt16((short)bytes.Length);
            Buffer.AddRange(bytes);
            Length += bytes.Length;
        }

        public void WriteUTF32(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt32(bytes.Length);
            Buffer.AddRange(bytes);
            Length += bytes.Length;
        }

        public void WriteCompressedInt(int value)
        {
            var v = (uint)value;
            while (v >= 128)
            {
                WriteByte((byte)(v | 128));
                v >>= 7;
            }
            WriteByte((byte)v);
        }

        public byte[] GetBuffer()
        {
            // 4 bytes
            // 1 byte
            // N bytes

            if (Buffer.Count != Length + 5)
            {
                Buffer.InsertRange(0, BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Length + 5)));
                Buffer.Insert(4, (byte)MessageId);
            }
            return Buffer.ToArray();
        }
    }

    public sealed class MessageHelper
    {
        public static OutgoingMessageData MapInfo(short width, short height, string name, string displayName, uint seed, byte difficulty, byte background, bool allowPlayerTeleport, bool showDisplays, string music, bool disableShooting, bool disableAbilities)
        {
            var message = new OutgoingMessageData(MessageId.MAPINFO);
            message.WriteInt16(width);
            message.WriteInt16(height);
            message.WriteUTF16(name);
            message.WriteUTF16(displayName);
            message.WriteUInt32(seed);
            message.WriteByte(background);
            message.WriteByte(difficulty);
            message.WriteBoolean(allowPlayerTeleport);
            message.WriteBoolean(showDisplays);
            message.WriteUTF16(music);
            message.WriteBoolean(disableShooting);
            message.WriteBoolean(disableAbilities);
            return message;
        }

        public static OutgoingMessageData NewTick(int tickId, int tickTime, List<ObjectStats> objectStats)
        {
            var message = new OutgoingMessageData(MessageId.NEWTICK);
            message.WriteInt32(tickId);
            message.WriteInt32(tickTime);
            message.WriteInt16(objectStats.Count);
            foreach(var objectStat in objectStats)
            {
                message.WriteInt32(objectStat.Id);
                message.WriteFloat(objectStat.X);
                message.WriteFloat(objectStat.Y);
                message.WriteInt16(objectStat.Stats.Length);
                for(var i = 0; i < objectStat.Stats.Length; i++)
                {
                    var stat = objectStat.Stats[i];
                    var type = stat.Value.GetType();
                    message.WriteByte((byte)stat.Key);
                    if (typeof(int) == type || typeof(ushort) == type)
                        message.WriteInt32((int)stat.Value);
                    else if (typeof(string) == type)
                        message.WriteUTF16((string)stat.Value);
                    else if (typeof(bool) == type)
                        message.WriteBoolean((bool)stat.Value);
                    else 
                        throw new InvalidOperationException($"Stat '{stat.Key}' of type '{stat.Value.GetType().ToString() ?? "null"}' not supported.");
                }
            }
            return message;
        }
    }
}
