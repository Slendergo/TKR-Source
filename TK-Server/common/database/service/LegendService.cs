using CA.Threading.Tasks;
using common.database.info;
using common.database.model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace common.database.service
{
    public sealed class LegendService : IRedisService
    {
        public const string Key1 = "legend";
        public const string Key2 = "legends";
        public const int MaxLegends = 20;

        public readonly Dictionary<LegendType, TimeSpan> Types;

        private const int _updateTimeout = 1;

        private Dictionary<LegendType, LegendInfo[]> _legendInfos;

        public LegendService(IDatabase db)
        {
            Db = db;
            Cts = new CancellationTokenSource();
            Routine = new InternalRoutine(TimeSpan.FromMinutes(_updateTimeout).Milliseconds, Core, logger.Log.Warn);
            Routine.AttachToParent(Cts.Token);

            Types = new Dictionary<LegendType, TimeSpan>()
            {
                [LegendType.Week] = TimeSpan.FromDays(7),
                [LegendType.Month] = TimeSpan.FromDays(30),
                [LegendType.All] = TimeSpan.MaxValue
            };

            _legendInfos = new Dictionary<LegendType, LegendInfo[]>();

            foreach (var type in Types)
                _legendInfos[type.Key] = new LegendInfo[0];
        }

        public CancellationTokenSource Cts { get; set; }

        public IDatabase Db { get; set; }

        public InternalRoutine Routine { get; set; }

        public LegendInfo[] this[LegendType type] => _legendInfos[type];

        public void Core(int delta)
        {
            var legendInfos = new Dictionary<LegendType, LegendInfo[]>();

            foreach (var type in Types)
                legendInfos[type.Key] = Db.SortedSetRangeByRank($"{Key2}:{type.Key}:byFame", 0, MaxLegends - 1, Order.Descending)
                    .Select(_ => new LegendInfo()
                    {
                        AccountId = BitConverter.ToInt32(_, 0),
                        CharacterId = BitConverter.ToInt32(_, 4)
                    }).ToArray();

            _legendInfos = legendInfos;
        }

        [Obsolete("Use method 'IsLegendsAsync(int)' instead.")]
        public bool IsLegend(int accountId) => _legendInfos.Values.Any(_ => _.Any(__ => __.AccountId == accountId));

        public async Task<bool> IsLegendAsync(int accountId) => await Db.HashExistsAsync(Key1, accountId);

        public async Task<int> LastUpdateTimeAsync()
        {
            var time = await Db.StringGetAsync($"{Key2}:updateTime");

            if (time.IsNullOrEmpty)
                return -1;

            return int.Parse(time);
        }

        public async Task<CharacterModel[]> LeaderboardAsync(LegendType type)
        {
            var legends = _legendInfos[type];
            var characters = new CharacterModel[legends.Length];

            for (var i = 0; i < characters.Length; i++)
            {
                var character = new CharacterModel();

                await character.InitAsync(Db, legends[i].AccountId.ToString(), legends[i].CharacterId.ToString());

                characters[i] = character;
            }

            return characters;
        }

        public void Start() => Routine.Start();

        public void Stop() => Cts.Cancel();
    }
}
