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
                player.CoreServerManager.WorldManager.Nexus.PortalMonitor.CreateNewRealm();
                return true;
            }
        }
    }
}
