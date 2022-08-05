using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Announce : Command
        {
            public Announce() : base("announce", permLevel: 110)
            { }

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.Announce(args);
                return true;
            }
        }
    }
}
