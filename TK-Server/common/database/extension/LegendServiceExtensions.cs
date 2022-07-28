using common.database.service;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace common.database.extension
{
    public static class LegendServiceExtensions
    {
        public static async Task InsertAsync(this LegendService service, int accountId, int characterId, int fame)
        {
            var buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(accountId), 0, buffer, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(characterId), 0, buffer, 4, 4);

            var trans = service.Db.CreateTransaction();

            foreach (var type in service.Types)
            {
                await trans.SortedSetAddAsync($"{LegendService.Key2}:{type.Key}:byFame", buffer, fame, CommandFlags.FireAndForget);

                if (type.Value == TimeSpan.MaxValue)
                    continue;

                var time = DateTime.UtcNow.Add(type.Value).ToUnixTimestamp();

                await trans.SortedSetAddAsync($"{LegendService.Key2}:{type.Key}:byTimeOfDeath", buffer, time, CommandFlags.FireAndForget);
            }

            await trans.ExecuteAsync(CommandFlags.FireAndForget);

            foreach (var type in service.Types)
                await service.Db.SortedSetRankAsync($"{LegendService.Key2}:{type.Key}:byFame", buffer, Order.Descending).ContinueWith(async task =>
                {
                    if (!task.Result.HasValue || task.Result.Value >= LegendService.MaxLegends)
                        return;

                    await service.Db.WriteAsync(LegendService.Key1, accountId.ToString(), "");
                });

            await service.Db.StringSetAsync($"{LegendService.Key2}:updateTime", DateTime.UtcNow.ToUnixTimestamp(), flags: CommandFlags.FireAndForget);
        }

        public static async Task WipeAsync(this LegendService service)
        {
            foreach (var type in service.Types)
            {
                if (type.Value == TimeSpan.MaxValue)
                {
                    await service.Db.SortedSetRemoveRangeByRankAsync($"{LegendService.Key2}:{type.Key}:byFame", 0, -LegendService.MaxLegends - 1, CommandFlags.FireAndForget);
                    continue;
                }

                var outdated = await service.Db.SortedSetRangeByScoreAsync($"{LegendService.Key2}:{type.Key}:byTimeOfDeath", 0, DateTime.UtcNow.ToUnixTimestamp());
                var trans = service.Db.CreateTransaction();

                await trans.SortedSetRemoveAsync($"{LegendService.Key2}:{type.Key}:byFame", outdated, CommandFlags.FireAndForget);
                await trans.SortedSetRemoveAsync($"{LegendService.Key2}:{type.Key}:byTimeOfDeath", outdated, CommandFlags.FireAndForget);
                await trans.ExecuteAsync(CommandFlags.FireAndForget);
            }

            await service.Db.KeyDeleteAsync(LegendService.Key1, CommandFlags.FireAndForget);

            foreach (var type in service.Types)
                await service.Db.SortedSetRangeByRankAsync($"{LegendService.Key2}:{type.Key}:byFame", 0, LegendService.MaxLegends - 1, Order.Descending).ContinueWith(async task =>
                {
                    var buffer = task.Result;
                    var trans = service.Db.CreateTransaction();

                    for (var i = 0; i < buffer.Length; i++)
                        await trans.HashSetAsync(LegendService.Key1, BitConverter.ToInt32(buffer[i], 0), string.Empty, flags: CommandFlags.FireAndForget);

                    await trans.ExecuteAsync(CommandFlags.FireAndForget);
                });

            await service.Db.StringSetAsync($"{LegendService.Key2}:updateTime", DateTime.UtcNow.ToUnixTimestamp(), flags: CommandFlags.FireAndForget);
        }
    }
}
