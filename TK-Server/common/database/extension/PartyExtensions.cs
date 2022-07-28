using StackExchange.Redis;
using System.Threading.Tasks;

namespace common.database.extension
{
    public static class PartyExtensions
    {
        public static async Task WipeAsync(IDatabase db)
        {
            var trans = db.CreateTransaction();
            var lastAccountId = int.Parse(await db.StringGetAsync("nextAccId"));

            for (var i = 1; i < lastAccountId; i++)
                await trans.HashDeleteAsync($"account.{i}", "partyId", CommandFlags.FireAndForget);

            await trans.KeyDeleteAsync("party", CommandFlags.FireAndForget);
            await trans.ExecuteAsync(CommandFlags.FireAndForget);
        }
    }
}
