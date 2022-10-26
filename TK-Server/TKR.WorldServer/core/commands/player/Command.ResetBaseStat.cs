using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class ResetBaseStat : Command
        {
            public override string CommandName => "resetbasestat";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.Client.Account.SetBaseStat = 0;
                player.Stats.ReCalculateValues();
                player.SendInfo("Your Base Stat got reset!");
                return true;
            }
        }
    }
}
