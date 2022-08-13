using common.database.info;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace common.database.model
{

    public class DbTalisman : RedisObject
    {
        public DbTalismanEntry this[int type]
        {
            get
            {
                var v = GetValue<string>(type.ToString());
                return v != null ? JsonConvert.DeserializeObject<DbTalismanEntry>(v) : new DbTalismanEntry()
                {
                    IsNull = true
                };
            }
            set => SetValue(type.ToString(), JsonConvert.SerializeObject(value));
        }

        public DbTalisman(IDatabase database, int characterId, int? type = null, bool isAsync = false)
        {
            Init(database, $"talismans.{characterId}", type?.ToString(), isAsync);
        }

        public void Unlock(byte type, byte level, int exp, int goal, byte tier)
        {
            var field = type.ToString();
            var json = GetValue<string>(field);

            if (json == null)
                SetValue(field, JsonConvert.SerializeObject(new DbTalismanEntry()
                {
                    Type = type,
                    Level = level,
                    Exp = exp,
                    Goal = goal,
                    Tier = tier
                }));
        }

        public void Update(byte type, byte level, int exp, int goal, byte tier)
        {
            var field = type.ToString();
            var json = GetValue<string>(field);

            if (json == null)
                SetValue(field, JsonConvert.SerializeObject(new DbTalismanEntry()
                {
                    Type = type,
                    Level = level,
                    Exp = exp,
                    Goal = goal,
                    Tier = tier
                }));
            else
            {
                var entry = JsonConvert.DeserializeObject<DbTalismanEntry>(json);
                entry.Type = type;
                entry.Level = level;
                entry.Exp = exp;
                entry.Goal = goal;
                entry.Tier = tier;
                SetValue(field, JsonConvert.SerializeObject(entry));
            }
        }
    }
}
