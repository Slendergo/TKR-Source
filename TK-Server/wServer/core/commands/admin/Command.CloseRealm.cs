using common;
using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CloseRealm : Command
        {
            public override RankingType RankRequirement => RankingType.Admin;
            public override string CommandName => "closerealm";
            public override string Alias => "cr";

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (!(player.World is RealmWorld gw))
                {
                    player.SendError("You must be in a realm to clsoe it.");
                    return false;
                }
                gw.CloseRealm();
                return true;
            }
        }
    }
}
