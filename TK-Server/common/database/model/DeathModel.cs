using common.database.extension;
using common.database.info;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace common.database.model
{
    public class DeathModel : IRedisModel
    {
        private int _accountId;
        private IDatabase _db;
        private string _key;

        public DateTime DeathTime { get; private set; }
        public bool EntryExist { get; private set; }
        public int Id { get; private set; }
        public bool IsFirstBorn { get; private set; }
        public string Killer { get; private set; }
        public int Level { get; private set; }
        public ushort ObjectType { get; private set; }
        public int TotalFame { get; private set; }

        public async Task AddAsync(DeathInfo info)
        {
            ObjectType = info.ObjectType;
            Level = info.Level;
            TotalFame = info.TotalFame;
            Killer = info.Killer;
            IsFirstBorn = info.IsFirstBorn;
            DeathTime = DateTime.UtcNow;

            await _db.WriteAsync(_key, "objType", ObjectType);
            await _db.WriteAsync(_key, "level", Level);
            await _db.WriteAsync(_key, "totalFame", TotalFame);
            await _db.WriteAsync(_key, "killer", Killer);
            await _db.WriteAsync(_key, "firstBorn", IsFirstBorn);
            await _db.WriteAsync(_key, "deathTime", DeathTime);
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> account id, [1] -> character id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            Id = int.Parse(args[1]);

            _accountId = int.Parse(args[0]);
            _db = db;
            _key = KeyPattern(_accountId.ToString(), Id.ToString());

            EntryExist = await db.KeyExistsAsync(_key);

            if (!EntryExist)
                return;

            DeathTime = await db.ReadAsync<DateTime>(_key, "deathTime");
            IsFirstBorn = await db.ReadAsync<bool>(_key, "firstBorn");
            Killer = await db.ReadAsync<string>(_key, "killer");
            Level = await db.ReadAsync<int>(_key, "level");
            ObjectType = await db.ReadAsync<ushort>(_key, "objType");
            TotalFame = await db.ReadAsync<int>(_key, "totalFame");
        }

        public string KeyPattern(params string[] args) => $"death.{args[0]}.{args[1]}";
    }
}
