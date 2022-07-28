using common.database;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        public class SweepMarket : Command
        {
            public SweepMarket() : base("sweepmarket", permLevel: 100, alias: "sweep")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                DbMarketData.CleanMarket(Program.CoreServerManager.Database);
                player.SendInfo("Sweeped");
                return true;
            }
        }
    }
}
