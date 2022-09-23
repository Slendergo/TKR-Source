using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class ToggleMarket : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "togglemarket";

            protected override bool Process(Player player, TickTime time, string args)
            {
                var state = player.GameServer.WorldManager.Nexus.MarketEnabled = !player.GameServer.WorldManager.Nexus.MarketEnabled;
                player.SendInfo($"Market is now: {(state ? "Enabled" : "Disabled")}");
                return true;
            }
        }
    }
}
