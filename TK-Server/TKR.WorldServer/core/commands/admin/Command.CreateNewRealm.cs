using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.worlds.logic;

namespace TKR.WorldServer.core.commands
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
