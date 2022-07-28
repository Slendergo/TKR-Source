using common.database.info;
using common.database.service;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace common.database.extension
{
    public static class MarketServiceExtensions
    {
        public static async Task WipeAsync(this MarketService service, bool isCleanup = false)
        {
            var offers = await service.Db.HashGetAllAsync(MarketService.Key);
            var accounts = new Dictionary<int, IList<ushort>>();
            var time = DateTime.UtcNow.ToUnixTimestamp();

            var trans1 = service.Db.CreateTransaction();

            for (var i = 0; i < offers.Length; i++)
            {
                var data = JsonConvert.DeserializeObject<MarketInfo>(offers[i].Value);

                if (isCleanup && data.OverAt >= time)
                    continue;

                if (accounts.ContainsKey(data.Id))
                    accounts[data.Id].Add(data.Item);
                else
                    accounts[data.Id] = new List<ushort>() { data.Item };

                await trans1.HashDeleteAsync(MarketService.Key, data.Id, CommandFlags.FireAndForget);
            }

            await trans1.ExecuteAsync(CommandFlags.FireAndForget);

            var trans2 = service.Db.CreateTransaction();

            foreach (var account in accounts)
            {
                var key = $"account.{account.Key}";
                var gifts = await trans2.ReadAsync<ushort[]>(key, "gifts");
                var marketItems = gifts.ToList();
                marketItems.AddRange(account.Value);

                await trans2.WriteAsync(key, "gifts", marketItems.ToArray());
                await trans2.HashDeleteAsync(key, "marketOffers", CommandFlags.FireAndForget);
            }

            await trans2.ExecuteAsync(CommandFlags.FireAndForget);
        }
    }
}
