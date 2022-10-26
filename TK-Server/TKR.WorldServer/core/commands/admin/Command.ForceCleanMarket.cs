using TKR.Shared;
using TKR.Shared.database.market;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        public class ForceCleanMarket : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "forcecleanmarket";

            protected override bool Process(Player player, TickTime time, string args)
            {
                DbMarketData.ForceCleanMarket(player.GameServer.Database);
                player.SendInfo("Done.");
                return true;
            }
        }
    }
}
