using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ResetBaseStat : Command
        {
            public ResetBaseStat() : base("resetbasestat")
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                player.Client.Account.SetBaseStat = 0;
                player.Stats.Base.ReCalculateValues();
                player.Stats.ReCalculateValues();
                player.Stats.Boost.ReCalculateValues();
                player.SendInfo("Your Base Stat got reset!");
                return true;
            }
        }
    }
}
