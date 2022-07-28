using common.database;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        public class ForceCleanMarket : Command
        {
            public ForceCleanMarket() : base("forceCleanMarket", permLevel: 110, alias: "forceMarket", false)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (player.Client.Account.Name != "ModNidhogg")
                {
                    player.SendError("Nope.");
                    return false;
                }
                DbMarketData.ForceCleanMarket(Program.CoreServerManager.Database);
                player.SendInfo("Done.");
                return true;
            }
        }
    }
}
