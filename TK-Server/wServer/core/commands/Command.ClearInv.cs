using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class ClearInv : Command
        {
            public ClearInv() : base("clearinv", permLevel: 80)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                for (int i = 4; i < 12; i++)
                    player.Inventory[i] = null;
                player.SendInfo("Inventory Cleared.");
                return true;
            }
        }
    }
}
