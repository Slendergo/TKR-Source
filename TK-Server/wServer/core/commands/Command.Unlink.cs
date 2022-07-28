using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Unlink : Command
        {
            public Unlink() : base("unlink", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickData time, string args)
            {
                if (player?.Owner == null)
                    return false;

                var world = player.Owner;
                if (world.Id < 0 || (player.Rank < 80 && !(world is Test)))
                {
                    player.SendError("Forbidden.");
                    return false;
                }

                if (!player.CoreServerManager.WorldManager.PortalMonitor.RemovePortal(player.Owner.Id))
                    player.SendError("Link not found.");
                else
                    player.SendInfo("Link removed.");

                return true;
            }
        }
    }
}
