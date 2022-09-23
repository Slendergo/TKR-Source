using TKR.Shared;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.core.commands
{
    public abstract partial class Command
    {
        internal class Announce : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "announce";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.Announce(player, args);
                return true;
            }
        }

        internal class ServerAnnounce : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "sannounce";

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.ServerAnnounce(args);
                return true;
            }
        }
    }
}
