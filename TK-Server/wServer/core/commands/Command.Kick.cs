using CA.Extensions.Concurrent;
using common.database;
using System.Linq;
using wServer.core.objects;

namespace wServer.core.commands
{
    public abstract partial class Command
    {
        internal class Kick : Command
        {
            public Kick() : base("kick", permLevel: 90)
            { }

            protected override bool Process(Player player, TickData time, string args)
            {
                var client = player.CoreServerManager.ConnectionManager.Clients
                    .KeyWhereAsParallel(_ => _.Account.Name.EqualsIgnoreCase(args) && !_.Account.Hidden)
                    .SingleOrDefault();
                if (client != default)
                {
                    client.Disconnect("KickCommand");
                    player.SendInfo("Player disconnected!");
                    return true;
                }

                player.SendError($"Player '{args}' could not be found!");
                return false;
            }
        }
    }
}
