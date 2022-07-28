using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Link : Command
        {
            public Link() : base("link", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (player?.World == null)
                    return false;

                var world = player.World;
                if (world.Id < 0 || (player.Rank < 80 && !(world is Test)))
                {
                    player.SendError("Forbidden.");
                    return false;
                }

                if (!player.CoreServerManager.WorldManager.PortalMonitor.AddPortal(world.Id))
                {
                    player.SendError("Link already exists.");
                    return false;
                }

                return true;
            }
        }
    }
}
