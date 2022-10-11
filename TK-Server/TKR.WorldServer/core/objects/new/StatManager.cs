using System;
using System.Collections.Generic;
using TKR.WorldServer.core.miscfile.datas;
using TKR.WorldServer.core.miscfile.stats;

namespace TKR.WorldServer.core.objects.@new
{
    public sealed class StatManager
    {
        private readonly EntityBase Host;
        private readonly Dictionary<StatDataType, int> IntValues = new Dictionary<StatDataType, int>();
        private readonly Dictionary<StatDataType, string> StringValues = new Dictionary<StatDataType, string>();
        private readonly Dictionary<StatDataType, float> SingleValues = new Dictionary<StatDataType, float>();

        public bool Updated { get; private set; }

        public StatManager(EntityBase host) => Host = host;

        public int GetIntStat(StatDataType statDataType) => IntValues.TryGetValue(statDataType, out var ret) ? ret : throw new Exception($"Unable to GetIntStat: {statDataType}");
        public bool GetBoolStat(StatDataType statDataType) => IntValues.TryGetValue(statDataType, out var ret) ? ret == 1 : throw new Exception($"Unable to GetIntStat: {statDataType}");
        public string GetStringStat(StatDataType statDataType) => StringValues.TryGetValue(statDataType, out var ret) ? ret : throw new Exception($"Unable to GetIntStat: {statDataType}");
        public float GetFloatStat(StatDataType statDataType) => SingleValues.TryGetValue(statDataType, out var ret) ? ret : throw new Exception($"Unable to GetIntStat: {statDataType}");

        public void SetIntStat(StatDataType statDataType, int value)
        {
            IntValues[statDataType] = value;
            Updated = true;
        }

        public void SetStringStat(StatDataType statDataType, string value)
        {
            StringValues[statDataType] = value;
            Updated = true;
        }

        public void SetBoolStat(StatDataType statDataType, bool value)
        {
            IntValues[statDataType] = value ? 1 : 0;
            Updated = true;
        }

        public void SetFloatValue(StatDataType statDataType, float value)
        {
            SingleValues[statDataType] = value;
            Updated = true;
        }

        public List<ValueTuple<StatDataType, object>> GetAll()
        {
            var ret = new List<ValueTuple<StatDataType, object>>();
            foreach ((var key, var value) in IntValues)
                ret.Add(ValueTuple.Create(key, value));
            foreach ((var key, var value) in StringValues)
                ret.Add(ValueTuple.Create(key, value));
            foreach ((var key, var value) in SingleValues)
                ret.Add(ValueTuple.Create(key, (int)value));
            return ret;
        }

        public ObjectDef Get() => new ObjectDef()
        {
            ObjectType = Host.ObjectType,
            Stats = new ObjectStats()
            {
                Id = Host.Id,
                X = Host.X,
                Y = Host.Y,
                Stats = GetAll()
            }
        };
    }
}
