using common;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CreateNewRealmCommand : Command
        {
            public override string CommandName => "createrealm";
            public override RankingType RankRequirement => RankingType.Admin;

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.WorldManager.Nexus.PortalMonitor.CreateNewRealm();
                return true;
            }
        }
    }
}
