using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Announce : Command
        {
            public Announce() : base("announce", permLevel: 110)
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                player.CoreServerManager.ChatManager.Announce(args);
                return true;
            }
        }
    }
}
