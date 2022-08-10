using common;
using common.database;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        public class ForceCleanMarket : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "forcecleanmarket";

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (player.Client.Account.Name != "ModNidhogg")
                {
                    player.SendError("Nope.");
                    return false;
                }
                DbMarketData.ForceCleanMarket(player.GameServer.Database);
                player.SendInfo("Done.");
                return true;
            }
        }
    }
}
