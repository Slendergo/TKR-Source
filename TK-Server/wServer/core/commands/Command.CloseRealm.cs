using wServer.core.objects;
using wServer.core.worlds.logic;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class CloseRealm : Command
        {
            public CloseRealm() : base("closerealm", permLevel: 80)
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {
                if (!(player.World is RealmWorld gw))
                {
                    player.SendError("An undefined error occurred.");
                    return false;
                }

                gw.CloseRealm();
                return true;
            }
        }
    }
}
