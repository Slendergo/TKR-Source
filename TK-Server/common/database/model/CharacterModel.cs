using common.database.extension;
using common.database.info;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.model
{
    public class CharacterModel : IRedisModel
    {
        private IDatabase _db;
        private string _key;

        public int AccountId { get; private set; }
        public bool[] BigSkills { get; private set; }
        public DateTime Creation { get; private set; }
        public bool EntryExist { get; private set; }
        public int Experience { get; private set; }
        public int ExperienceBoostLifetime { get; private set; }
        public int Fame { get; private set; }
        public byte[] FameStats { get; private set; }
        public int FinalFame { get; private set; }
        public bool HasBackpack { get; private set; }
        public int HealthPotions { get; private set; }
        public int HP { get; private set; }
        public int Id { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsUpgradeEnabled { get; private set; }
        public ushort[] Items { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int Level { get; private set; }
        public int LootDropBoostLifetime { get; private set; }
        public int MagicPotions { get; private set; }
        public Dictionary<MaxedStat, bool> MaxedStats { get; private set; }
        public int MP { get; private set; }
        public ushort ObjectType { get; private set; }
        public int PetId { get; private set; }
        public int Points { get; private set; }
        public int Skin { get; private set; }
        public int[] SmallSkills { get; private set; }
        public int[] Stats { get; private set; }
        public int Texture1 { get; private set; }
        public int Texture2 { get; private set; }

        public async Task AddAsync(CharacterInfo info)
        {
            ObjectType = info.ObjectType;
            Level = info.Level;
            Experience = 0;
            Fame = 0;
            Items = info.Items;
            Stats = info.Stats;
            HP = info.Stats[0];
            MP = info.Stats[1];
            Texture1 = 0;
            Texture2 = 0;
            Skin = info.Skin;
            PetId = 0;
            FameStats = new byte[0];
            Creation = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;

            await _db.WriteAsync(_key, "charType", ObjectType);
            await _db.WriteAsync(_key, "level", Level);
            await _db.WriteAsync(_key, "exp", Experience);
            await _db.WriteAsync(_key, "fame", Fame);
            await _db.WriteAsync(_key, "items", Items);
            await _db.WriteAsync(_key, "stats", Stats);
            await _db.WriteAsync(_key, "hp", HP);
            await _db.WriteAsync(_key, "mp", MP);
            await _db.WriteAsync(_key, "tex1", Texture1);
            await _db.WriteAsync(_key, "tex2", Texture2);
            await _db.WriteAsync(_key, "skin", Skin);
            await _db.WriteAsync(_key, "petId", PetId);
            await _db.WriteAsync(_key, "fameStats", FameStats);
            await _db.WriteAsync(_key, "createTime", Creation);
            await _db.WriteAsync(_key, "lastSeen", LastSeen);
        }

        public async Task DeleteAsync()
        {
            var buffer = BitConverter.GetBytes(Id);

            await _db.KeyDeleteAsync(_key, CommandFlags.FireAndForget);
            await _db.SetRemoveAsync($"alive.{AccountId}", buffer);
            await _db.ListRemoveAsync($"dead.{AccountId}", buffer);
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> account id, [1] -> character id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            AccountId = int.Parse(args[0]);
            Id = int.Parse(args[1]);

            _db = db;
            _key = KeyPattern(AccountId.ToString(), Id.ToString());

            EntryExist = await db.KeyExistsAsync(_key);
            MaxedStats = new Dictionary<MaxedStat, bool>();

            if (!EntryExist)
                return;

            var maxedStats = Enum.GetNames(typeof(MaxedStat));

            for (var i = 0; i < maxedStats.Length; i++)
            {
                Enum.TryParse<MaxedStat>(maxedStats[i], out var maxedStat);

                MaxedStats[maxedStat] = await db.ReadAsync<bool>(_key, maxedStat.ToField());
            }

            Creation = await db.ReadAsync<DateTime>(_key, "createTime");
            IsDead = await db.ReadAsync<bool>(_key, "dead");
            Experience = await db.ReadAsync<int>(_key, "exp");
            Fame = await db.ReadAsync<int>(_key, "fame");
            FameStats = await db.ReadAsync<byte[]>(_key, "fameStats");
            FinalFame = await db.ReadAsync<int>(_key, "finalFame");
            HasBackpack = await db.ReadAsync<bool>(_key, "hasBackpack");
            HealthPotions = await db.ReadAsync<int>(_key, "hpPotCount");
            HP = await db.ReadAsync<int>(_key, "hp");
            Items = await db.ReadAsync(_key, "items", Enumerable.Repeat((ushort)0xffff, 20).ToArray());
            LastSeen = await db.ReadAsync<DateTime>(_key, "lastSeen");
            LootDropBoostLifetime = await db.ReadAsync<int>(_key, "ldBoost");
            Level = await db.ReadAsync<int>(_key, "level");
            MagicPotions = await db.ReadAsync<int>(_key, "mpPotCount");
            MP = await db.ReadAsync<int>(_key, "mp");
            ObjectType = await db.ReadAsync<ushort>(_key, "charType");
            PetId = await db.ReadAsync<int>(_key, "petId");
            Points = await db.ReadAsync<int>(_key, "points");
            Skin = await db.ReadAsync<int>(_key, "skin");
            Stats = await db.ReadAsync<int[]>(_key, "stats");
            Texture1 = await db.ReadAsync<int>(_key, "tex1");
            Texture2 = await db.ReadAsync<int>(_key, "tex2");
            IsUpgradeEnabled = await db.ReadAsync<bool>(_key, "upgradeEnabled");
            ExperienceBoostLifetime = await db.ReadAsync<int>(_key, "xpBoost");
        }

        public async Task<bool> IsAliveAsync() => await _db.SetContainsAsync($"alive.{AccountId}", BitConverter.GetBytes(Id), CommandFlags.FireAndForget);

        public string KeyPattern(params string[] args) => $"char.{args[0]}.{args[1]}";

        public async Task SaveAsync()
        {
            foreach (var maxedStat in MaxedStats)
                await _db.WriteAsync(_key, maxedStat.Key.ToField(), maxedStat.Value);

            await _db.WriteAsync(_key, "createTime", Creation);
            await _db.WriteAsync(_key, "dead", IsDead);
            await _db.WriteAsync(_key, "exp", Experience);
            await _db.WriteAsync(_key, "fame", Fame);
            await _db.WriteAsync(_key, "fameStats", FameStats);
            await _db.WriteAsync(_key, "hasBackpack", HasBackpack);
            await _db.WriteAsync(_key, "hpPotCount", HealthPotions);
            await _db.WriteAsync(_key, "hp", HP);
            await _db.WriteAsync(_key, "items", Items);
            await _db.WriteAsync(_key, "lastSeen", LastSeen);
            await _db.WriteAsync(_key, "ldBoost", LootDropBoostLifetime);
            await _db.WriteAsync(_key, "level", Level);
            await _db.WriteAsync(_key, "mpPotCount", MagicPotions);
            await _db.WriteAsync(_key, "mp", MP);
            await _db.WriteAsync(_key, "charType", ObjectType);
            await _db.WriteAsync(_key, "petId", PetId);
            await _db.WriteAsync(_key, "points", Points);
            await _db.WriteAsync(_key, "skin", Skin);
            await _db.WriteAsync(_key, "stats", Stats);
            await _db.WriteAsync(_key, "tex1", Texture1);
            await _db.WriteAsync(_key, "tex2", Texture2);
            await _db.WriteAsync(_key, "upgradeEnabled", IsUpgradeEnabled);
            await _db.WriteAsync(_key, "xpBoost", ExperienceBoostLifetime);
        }

        public async Task SetDeathAsync(int fame)
        {
            IsDead = true;
            FinalFame = fame;

            var buffer = BitConverter.GetBytes(Id);

            await _db.SetRemoveAsync($"alive.{AccountId}", buffer, CommandFlags.FireAndForget);
            await _db.ListLeftPushAsync($"dead.{AccountId}", buffer, When.Always, CommandFlags.FireAndForget);
            await _db.WriteAsync(_key, "dead", IsDead);
            await _db.WriteAsync(_key, "finalFame", FinalFame);
        }
    }
}
