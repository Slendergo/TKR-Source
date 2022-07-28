using CA.Threading.Tasks;
using common.database.extension;
using common.database.info;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace common.database.service
{
    public sealed class MarketService : IRedisService
    {
        public const string Key = "market";

        private const int _updateTimeout = 1;

        private Dictionary<ushort, IList<MarketInfo>> _marketInfos;

        public MarketService(IDatabase db)
        {
            Db = db;

            Cts = new CancellationTokenSource();
            Routine = new InternalRoutine(TimeSpan.FromMinutes(_updateTimeout).Milliseconds, Core, logger.Log.Warn);
            Routine.AttachToParent(Cts.Token);

            _marketInfos = new Dictionary<ushort, IList<MarketInfo>>();
        }

        public CancellationTokenSource Cts { get; set; }
        public IDatabase Db { get; set; }
        public InternalRoutine Routine { get; set; }

        public IList<MarketInfo> this[ushort item] => _marketInfos.ContainsKey(item) ? _marketInfos[item] : new MarketInfo[0];

        public async Task AddOffersAsync(int accountId, MarketInfo[] infos)
        {
            var marketOffers = infos.ToList();

            var data = await Db.ReadAsync<string>(Key, accountId.ToString());

            if (data != null)
                marketOffers.AddRange(JsonConvert.DeserializeObject<MarketInfo[]>(data));

            await Db.WriteAsync(Key, accountId.ToString(), JsonConvert.SerializeObject(marketOffers));
        }

        public void Core(int delta)
        {
            var marketInfos = new Dictionary<ushort, IList<MarketInfo>>();
            var offers = Db.HashGetAll(Key);

            for (var i = 0; i < offers.Length; i++)
            {
                var data = JsonConvert.DeserializeObject<MarketInfo>(offers[i].Value);

                if (marketInfos.ContainsKey(data.Item))
                    marketInfos[data.Item].Add(data);
                else
                    marketInfos[data.Item] = new List<MarketInfo>() { data };
            }

            _marketInfos = marketInfos;
        }

        public async Task IncrementTreasury(int tax) => await Db.StringIncrementAsync("collectedTaxes", tax, CommandFlags.FireAndForget);

        public async Task RemoveOfferAsync(int accountId, MarketInfo info)
        {
            var marketOffers = new List<MarketInfo>();

            var data = await Db.ReadAsync<string>(Key, accountId.ToString());

            if (data != null)
                marketOffers.AddRange(JsonConvert.DeserializeObject<MarketInfo[]>(data));

            if (!marketOffers.Contains(info))
                return;

            marketOffers.Remove(info);

            await Db.WriteAsync(Key, accountId.ToString(), JsonConvert.SerializeObject(marketOffers));
        }

        public async Task<MarketInfo> SearchAsync(int id)
        {
            var data = await Db.ReadAsync<string>(Key, id.ToString());

            if (data == null)
                return new MarketInfo();

            return JsonConvert.DeserializeObject<MarketInfo>(data);
        }

        public void Start() => Routine.Start();

        public void Stop() => Cts.Cancel();
    }
}
