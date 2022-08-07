using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Announce : Command
        {
            public Announce() : base("announce", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.Announce(player, args);
                return true;
            }
        }

        internal class ServerAnnounce : Command
        {
            public ServerAnnounce() : base("sannounce", permLevel: 110)
            {
            }

            protected override bool Process(Player player, TickTime time, string args)
            {
                player.GameServer.ChatManager.ServerAnnounce(args);
                return true;w
            }
        }
    }
}
