using System;
using System.Collections.Generic;
using System.Text.Json;
using TKR.Shared;
using TKR.WorldServer.core.miscfile.stats;

namespace TKR.WorldServer.core.miscfile.datas
{
    public struct ObjectStats
    {
        public int Id;
        public float X;
        public float Y;
        public List<ValueTuple<StatDataType, object>> Stats;

        public ObjectStats(int id, float x, float y, List<ValueTuple<StatDataType, object>> stats)
        {
            Id = id;
            X = x;
            Y = y;
            Stats = stats;
        }

        public void Write(NWriter wtr)
        {
            wtr.Write(Id);
            wtr.Write(X);
            wtr.Write(Y);
            wtr.Write((short)Stats.Count);
            foreach ((var key, var value) in Stats)
            {
                wtr.Write((byte)key);

                if (value is int)
                {
                    wtr.Write((int)value);
                    continue;
                }

                if (value is string)
                {
                    wtr.WriteUTF(value as string);
                    continue;
                }

                if (value is bool)
                {
                    wtr.Write((bool)value ? 1 : 0);
                    continue;
                }

                if (value is ushort)
                {
                    wtr.Write((int)(ushort)value);
                    continue;
                }

                throw new InvalidOperationException($"Stat '{key}' of type '{value?.GetType().ToString() ?? "null"}' not supported.");
            }
        }
    }
}
