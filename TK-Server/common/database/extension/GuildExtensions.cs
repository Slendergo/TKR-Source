using StackExchange.Redis;
using System.Threading.Tasks;

namespace common.database.extension
{
    public static class GuildExtensions
    {
        public static async Task WipeAsync(IDatabase db)
        {
            var trans = db.CreateTransaction();
            var lastGuildId = int.Parse(await db.StringGetAsync("nextGuildId"));

            for (var i = 1; i < lastGuildId; i++)
                await trans.HashSetAsync($"guild.{i}", "guildLootBoost", 0, flags: CommandFlags.FireAndForget);

            await trans.ExecuteAsync(CommandFlags.FireAndForget);
        }
    }
}
