using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CreateRealm : Command
        {
            public CreateRealm() : base("createrealm", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickTime time, string color)
            {
                player.GameServer.WorldManager.Nexus.PortalMonitor.CreateNewRealm();
                return true;
            }
        }
        internal class RemoveRealm : Command
        {
            public RemoveRealm() : base("removerealm", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickTime time, string color)
            {
                player.World.GameServer.WorldManager.Nexus.PortalMonitor.RemovePortal(player.World.Id);
                player.World.FlagForClose();
                return true;
            }
        }
    }
}
