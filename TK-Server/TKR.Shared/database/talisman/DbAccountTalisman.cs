using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;

namespace TKR.Shared.database.talisman
{
    public sealed class DbActiveTalismans
    {
        private readonly IDatabase _db;

        public List<int> Activated { get; set; }

        [JsonIgnore] public int AccountId { get; private set; }
        [JsonIgnore] public int CharacterId { get; private set; }
        [JsonIgnore] public bool IsNull { get; private set; }

        public DbActiveTalismans(IDatabase db, int accountId, int charId)
        {
            _db = db;

            AccountId = accountId;
            CharacterId = charId;

            var json = (string)db.HashGet("active_talismans", $"{AccountId}.{charId}");

            if (json == null)
                IsNull = true;
            else
                JsonConvert.PopulateObject(json, this);
        }

        public void Update(byte type, bool active)
        {
            if (Activated == null)
            {
                Activated = new List<int>()
                {
                    type
                };
                return;
            }

            if (active)
            {
                Activated.Add(type);
                return;
            }

            _ = Activated.Remove(type);
        }

        public bool IsActive(byte type) => Activated == null ? false : Activated.Contains(type);

        public void Flush() => _db.HashSet("active_talismans", $"{AccountId}.{CharacterId}", JsonConvert.SerializeObject(this));
    }

    public class DbAccountTalisman : RedisObject
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

        public DbAccountTalisman(IDatabase database, int accountId, int? type = null, bool isAsync = false)
        {
            Init(database, $"talismans.{accountId}", type?.ToString(), isAsync);
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
                    Tier = tier,
                }));
        }

        public void Update(DbTalismanEntry entry) => Update(entry.Type, entry.Level, entry.Exp, entry.Goal, entry.Tier);

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
