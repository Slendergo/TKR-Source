using common;
using System;
using System.Collections.Generic;
using wServer.core;

namespace wServer
{
    public struct ObjectStats
    {
        public int Id;
        public Position Position;
        public KeyValuePair<StatDataType, object>[] Stats;

        public static ObjectStats Read(NReader rdr)
        {
            var ret = new ObjectStats
            {
                Id = rdr.ReadInt32(),
                Position = Position.Read(rdr),
                Stats = new KeyValuePair<StatDataType, object>[rdr.ReadInt16()]
            };

            for (var i = 0; i < ret.Stats.Length; i++)
            {
                var type = (StatDataType)rdr.ReadByte();

                if (type == StatDataType.Guild || type == StatDataType.Name)
                    ret.Stats[i] = new KeyValuePair<StatDataType, object>(type, rdr.ReadUTF());
                else
                    ret.Stats[i] = new KeyValuePair<StatDataType, object>(type, rdr.ReadInt32());
            }

            return ret;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Id);
            Position.Write(wtr);
            wtr.Write((short)Stats.Length);

            foreach (var i in Stats)
            {
                wtr.Write((byte)i.Key);

                if (i.Value is int)
                {
                    wtr.Write((int)i.Value);
                    continue;
                }

                if (i.Value is string)
                {
                    //Console.WriteLine(i.Key);
                    wtr.WriteUTF(i.Value as string);
                    continue;
                }

                if (i.Value is bool)
                {
                    wtr.Write((bool)i.Value ? 1 : 0);
                    continue;
                }

                if (i.Value is ushort)
                {
                    wtr.Write((int)(ushort)i.Value);
                    continue;
                }

                throw new InvalidOperationException($"Stat '{i.Key}' of type '{i.Value?.GetType().ToString() ?? "null"}' not supported.");
            }
        }
    }
}
