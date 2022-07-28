using common.database.extension;
using common.database.info;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace common.database.model
{
    public class ClassStatsModel : IRedisModel
    {
        private int _accountId;
        private IDatabase _db;
        private string _key;

        public bool EntryExist { get; private set; }

        public ClassStatInfo this[ushort type]
        {
            get
            {
                var classStat = _db.ReadAsync<string>(_key, type.ToString()).Result;

                return classStat != null
                    ? JsonConvert.DeserializeObject<ClassStatInfo>(classStat)
                    : new ClassStatInfo()
                    {
                        BestLevel = 0,
                        BestFame = 0,
                        IsNull = true
                    };
            }
#pragma warning disable
            set => _db.WriteAsync(_key, type.ToString(), JsonConvert.SerializeObject(value));
#pragma warning restore
        }

        /// <summary>
        /// <paramref name="args"/>: [0] -> account id
        /// </summary>
        /// <param name="db"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InitAsync(IDatabase db, params string[] args)
        {
            _accountId = int.Parse(args[0]);
            _db = db;
            _key = KeyPattern(_accountId.ToString());

            EntryExist = await db.KeyExistsAsync(_key);
        }

        public string KeyPattern(params string[] args) => $"classStats.{args[0]}";

        public async Task UnlockAsync(ushort type)
        {
            var info = this[type];

            if (info.IsNull)
                await _db.WriteAsync(_key, type.ToString(), JsonConvert.SerializeObject(new ClassStatInfo()
                {
                    BestLevel = 0,
                    BestFame = 0
                }));
        }

        public async Task UpdateAsync(CharacterModel model)
        {
            var field = model.ObjectType.ToString();
            var fame = Math.Max(model.Fame, model.FinalFame);
            var data = await _db.ReadAsync<string>(_key, field);

            if (data == null)
                await _db.WriteAsync(_key, field, JsonConvert.SerializeObject(new ClassStatInfo()
                {
                    BestLevel = model.Level,
                    BestFame = fame
                }));
            else
            {
                var classStat = JsonConvert.DeserializeObject<ClassStatInfo>(data);

                if (model.Level > classStat.BestLevel)
                    classStat.BestLevel = model.Level;

                if (fame > classStat.BestFame)
                    classStat.BestFame = fame;

                await _db.WriteAsync(_key, field, JsonConvert.SerializeObject(classStat));
            }
        }
    }
}
